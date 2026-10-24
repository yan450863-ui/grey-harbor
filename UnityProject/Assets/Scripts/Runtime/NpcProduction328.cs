using System;
using System.Collections.Generic;
using UnityEngine;
namespace GreyHarbor {
 // Production permissions are permanent rewards from claimed long quests.
 // Old relationship and workbench progress is captured once, independently of daily resets.
 public static class NpcProduction328 {
  public static int RelationshipLevel(NpcSave319 state,int npc){
   if(npc<=0||npc>=5||state==null)return 1;
   int legacy=state.LegacyLevels328!=null&&npc<state.LegacyLevels328.Length?state.LegacyLevels328[npc]:1;
   int claimed=state.Quests==null?0:state.Quests.FindAll(q=>q!=null&&q.Npc==npc&&q.Period=="long"&&q.Claimed).Count;
   return Mathf.Clamp(Math.Max(legacy,1+Math.Min(2,claimed)),1,3);
  }
  public static int ClaimedStages(NpcSave319 state,int npc)=>state?.Quests?.FindAll(q=>q!=null&&q.Npc==npc&&q.Period=="long"&&q.Claimed).Count??0;
  static bool Claimed(NpcSave319 state,string template)=>state.Quests.Exists(q=>q!=null&&q.Period=="long"&&(q.Template==template||q.Id=="long:"+template)&&q.Claimed);
  public static void Normalize(BaseSave save){
   if(save==null)return;
   var state=save.Npc319=save.Npc319??new NpcSave319();
   state.Quests=state.Quests??new List<Quest319>();state.Exchanges=state.Exchanges??new List<ExchangeUse319>();state.PendingItems=state.PendingItems??new List<ItemStack>();
   if(state.XP==null||state.XP.Length!=5)Array.Resize(ref state.XP,5);
   if(state.LegacyLevels328==null||state.LegacyLevels328.Length!=5)Array.Resize(ref state.LegacyLevels328,5);
   if(state.ProductionVersion328<1){
    for(int i=0;i<5;i++)state.LegacyLevels328[i]=Math.Max(state.LegacyLevels328[i],i==0?1:state.XP[i]>=300?3:state.XP[i]>=100?2:1);
    state.UpgradePermission328=Math.Max(state.UpgradePermission328,Math.Max(Mathf.Clamp(save.CampLevel,0,2),state.LegacyLevels328[2]-1));
    state.GreenAmmoBlueprint328|=save.CampLevel>=1||state.LegacyLevels328[3]>=2;
    state.BlueAmmoBlueprint328|=save.CampLevel>=2||state.LegacyLevels328[3]>=3;
    state.AkBlueprint328|=save.CampLevel>=1||state.LegacyLevels328[3]>=3;
    state.ProductionVersion328=1;
   }
   // Merge by stable ID: never replace accepted/progress/completion/claim flags.
   foreach(var authored in QuestRules319.LongTasks()){
    var existing=state.Quests.Find(q=>q!=null&&q.Id==authored.Id);
    if(existing==null)state.Quests.Add(authored);
    else {existing.XP=0;existing.Description=authored.Description;}
   }
   foreach(var quest in state.Quests)if(quest!=null)quest.XP=0;
   ApplyClaimedUnlocks(state);
  }
  public static void ApplyClaimedUnlocks(NpcSave319 state){
   if(state==null||state.Quests==null)return;
   if(Claimed(state,"eng-parts"))state.UpgradePermission328=Math.Max(1,state.UpgradePermission328);
   if(Claimed(state,"eng-survey"))state.UpgradePermission328=Math.Max(2,state.UpgradePermission328);
   state.GreenAmmoBlueprint328|=Claimed(state,"mil-clear");
   state.BlueAmmoBlueprint328|=Claimed(state,"mil-electronics");
   state.AkBlueprint328|=Claimed(state,"mil-electronics");
  }
  public static string UnlockText(Quest319 quest){
   if(quest==null||quest.Period!="long")return "";
   var template=quest.Template??"";if(template.Length==0&&quest.Id!=null&&quest.Id.StartsWith("long:"))template=quest.Id.Substring(5);
   switch(template){case "eng-parts":return "工作台一级升级资格";case "eng-survey":return "工作台二级升级资格";case "mil-clear":return "全部绿色弹药图纸";case "mil-electronics":return "全部蓝色弹药与AK组装图纸";default:return "";}
  }
  public static bool CanUpgrade(BaseSave save,int targetLevel,out string reason){
   reason="";if(save==null||targetLevel<1||targetLevel>2||targetLevel!=save.CampLevel+1){reason="工作台升级目标无效，当前最高为二级。";return false;}
   var state=save.Npc319;if(state!=null&&state.UpgradePermission328>=targetLevel)return true;
   reason=targetLevel==1?"先完成工程师「工具零件回收」并领取奖励，取得一级升级资格。":"先完成工程师「核查货运设施」并领取奖励，取得二级升级资格。";return false;
  }
  public static bool CanMake(BaseSave save,CampRecipe recipe,out string reason){
   reason="";if(save==null||recipe==null){reason="制作配方无效。";return false;}
   int required=recipe.Level;if(recipe.Output==ItemKind.PrecisionAmmoComponent328)required=Math.Max(2,required);
   if(save.CampLevel<required){reason="此配方需要工作台"+required+"级；先向工程师取得升级资格，再提交升级材料。";return false;}
   var state=save.Npc319;
   if(AmmoRules270.IsAmmo(recipe.Output)){
    bool green=RaidInventory.Grade(recipe.Output)==0;
    if(state!=null&&(green?state.GreenAmmoBlueprint328:state.BlueAmmoBlueprint328))return true;
    reason=green?"先完成军工代表「清除叛军巡逻」并领取奖励，解锁绿色弹药图纸。":"先完成军工代表「回收电子零件」并领取奖励，解锁蓝色弹药图纸。";return false;
   }
   if(recipe.Output==ItemKind.AK&&(state==null||!state.AkBlueprint328)){reason="先完成军工代表「回收电子零件」并领取奖励，解锁AK组装图纸。";return false;}
   return true;
  }
  public static string Guide(BaseSave save,int npc){
   var state=save?.Npc319;
   if(npc==2)return "先工程师取得升级资格，再向军工代表解锁图纸 · 一级资格："+(state!=null&&state.UpgradePermission328>=1?"已获":"未获")+" · 二级资格："+(state!=null&&state.UpgradePermission328>=2?"已获":"未获");
   if(npc==3)return "绿色弹药："+(state!=null&&state.GreenAmmoBlueprint328?"已解锁":"完成合作01并领奖")+" · 蓝色弹药／AK："+(state!=null&&state.BlueAmmoBlueprint328&&state.AkBlueprint328?"已解锁":"完成合作02并领奖");
   return "长期合作领奖推进关系等级；每日与每周只发奖励，不提升等级。";
  }
 }
}
