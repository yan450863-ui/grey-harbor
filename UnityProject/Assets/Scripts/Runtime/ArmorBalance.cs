using System;
using UnityEngine;
namespace GreyHarbor {
 public static class ArmorBalance {
  public const float AKDamage=18,AKPenetration=10;
  public static bool Defensive(ItemKind kind)=>RaidInventory.IsArmor(kind);
  public static float Max(ItemKind kind)=>ArmorLoadout320.Maximum(kind);
  public static void Ensure(ItemStack p){if(p==null||!Defensive(p.Kind))return;if(!p.ArmorReady){p.ArmorPoints=Max(p.Kind);p.ArmorReady=true;}p.ArmorPoints=Mathf.Clamp(p.ArmorPoints,0,Max(p.Kind));}
  public static float Multiplier(int attackGrade,int defenseGrade){int gap=defenseGrade-attackGrade;return gap<=0?1:gap==1?.8f:.5f;}
  public static float Hit(ItemStack armor,float damage,float penetration,int ammoGrade){
   Ensure(armor);bool defensive=armor!=null&&Defensive(armor.Kind);
   if(defensive&&armor.ArmorPoints>0){armor.ArmorPoints=Mathf.Max(0,armor.ArmorPoints-penetration*Multiplier(ammoGrade,RaidInventory.Grade(armor.Kind)));return 0;}
   return damage*(defensive?Multiplier(ammoGrade,RaidInventory.Grade(armor.Kind)):1);
  }
  public static float HitPool146(ref float armor,float damage,float penetration,int ammoGrade,int defenseGrade){
   if(armor>0){armor=Mathf.Max(0,armor-penetration*Multiplier(ammoGrade,defenseGrade));return 0;}
   return damage*Multiplier(ammoGrade,defenseGrade);
  }
  // Historical callers retain their signatures; weapon quality no longer controls either stage.
  public static float Hit(ItemStack armor,float damage,float penetration,int weaponGrade,int ammoGrade)=>Hit(armor,damage,penetration,ammoGrade);
  public static float HitPool146(ref float armor,float damage,float penetration,int weaponGrade,int ammoGrade,int defenseGrade)=>HitPool146(ref armor,damage,penetration,ammoGrade,defenseGrade);
  public static int RepairPrice(ItemStack p){Ensure(p);return p==null?0:Mathf.CeilToInt((Max(p.Kind)-p.ArmorPoints)*Economy270.Scale/10);}
  public static void Normalize(BaseSave save){foreach(var list in new[]{save.Warehouse,save.CampRig,save.CampBag,save.SafeItems,save.Prepared})if(list!=null)foreach(var p in list)Ensure(p);foreach(var p in new[]{save.Helmet,save.Armor})Ensure(p);if(save.CampDrops!=null)foreach(var d in save.CampDrops)foreach(var p in d.Items)Ensure(p);}
 }
}
