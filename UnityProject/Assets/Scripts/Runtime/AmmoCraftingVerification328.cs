using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
namespace GreyHarbor {
 public sealed partial class RaidVerification {
  static readonly ItemKind[] AmmoKinds328={ItemKind.Green9mm,ItemKind.Green556,ItemKind.GreenAmmo,ItemKind.Green12Gauge,ItemKind.Blue9mm,ItemKind.Blue556,ItemKind.Ammo,ItemKind.Blue12Gauge};
  static int RecipeIndex328(ItemKind kind,bool rare=false){return Array.FindIndex(CampRules.Recipes,r=>r.Output==kind&&(kind!=ItemKind.PrecisionAmmoComponent328||r.Costs.Any(c=>c.Kind==(rare?ItemKind.RareMetalPowder:ItemKind.PrecisionShaftBushing))));}
  BaseSave EmptyProduction328(int level=2){var s=new BaseSave{StarterGranted=true,GearGranted=true,GridInitialized=true,CampLevel=level,RigGear=new ItemStack(ItemKind.StandardRig,1),PackGear=new ItemStack(ItemKind.StandardPack,1)};CampRules.Normalize(s);return s;}
  void ResetProduction328(int level=2){G.Save=EmptyProduction328(level);G.CampLocked=false;G.Phase=GamePhase.Base;G.Panel=CampPanel.Workbench;G.InventoryOpen=true;G.MenuCamp315=false;G.Chest.Slots.Clear();G.Bag.Slots.Clear();G.Safe.Slots.Clear();G.OpenLoot=null;G.Magazine=G.MagazineFound=0;G.MagazineAmmo=ItemKind.GreenAmmo;G.Equipped=null;G.SecondWeapon=null;G.Map.Loot.Clear();}
  void SupplyProduction328(ItemKind k,int count,bool certified=true){CampRules.Deposit(G.Save,k,count,certified&&Certification319.Collection(k));}
  string WarehouseState328()=>JsonUtility.ToJson(new WarehouseSnapshot328{Items=G.Save.Warehouse});
  [Serializable] sealed class WarehouseSnapshot328 {public List<ItemStack> Items;}
  void RecipeAndItemChecks328(){
   Check((int)ItemKind.YardHomeKey324==294&&(int)ItemKind.BasicAmmoComponent328==295&&(int)ItemKind.PrecisionAmmoComponent328==296&&RaidInventory.KindCount==297,"328 append-only component IDs preserve existing item IDs");
   Check(CampRules.Recipes.Length==20&&CampRules.Recipes.Count(r=>AmmoRules270.IsAmmo(r.Output))==8&&CampRules.Recipes.Count(r=>AmmoCrafting328.IsComponent(r.Output))==3,"328 three component routes eight ammunition recipes and nine preserved non-ammo recipes");
   foreach(var k in new[]{ItemKind.BasicAmmoComponent328,ItemKind.PrecisionAmmoComponent328}){
    Check(RaidInventory.Group(k)==ItemGroup.Crafting&&!Certification319.Collection(k)&&new ItemStack(k,1,ItemOrigin.Certified).Origin==ItemOrigin.Ordinary,"328 components are ordinary crafting materials without collection certification "+k);
    Check(RaidInventory.Width(k)==1&&RaidInventory.Height(k)==1&&RaidInventory.Limit(k)==100&&new RaidInventory(18,20,true).StackLimit(k)==100,"328 components occupy one cell and stack to100 in carry and warehouse "+k);
    Check(RaidInventory.Value(k)==(k==ItemKind.BasicAmmoComponent328?30:60)&&!MarketRules.Listed(k)&&MarketRules.UnitPrice(k)==-1&&!MarketRules.Catalog(0).Contains(k),"328 components have resale value but cannot be purchased "+k);
    Check(ReferenceEquals(ItemIcons.Get(k),ItemIcons.Get(k==ItemKind.BasicAmmoComponent328?ItemKind.GunParts:ItemKind.PrecisionShaftBushing)),"328 component icons reuse established colored pixel art "+k);
    foreach(SupplyType type in Enum.GetValues(typeof(SupplyType)))Check(!LootBalance.Eligible(type,k),"328 component excluded from normal loot pool "+type+" "+k);
    var inventory=new RaidInventory(3,1,true);Check(inventory.Add(k,201)==0&&inventory.Slots.Count==3&&inventory.Slots[0].Count==100&&inventory.Slots[2].Count==1,"328 warehouse splits 201 components into1001001 "+k);
   }
   int[] componentCounts={64,73,70,70,43,54,53,53};
   for(int i=0;i<AmmoKinds328.Length;i++){
    var recipe=CampRules.Recipes[RecipeIndex328(AmmoKinds328[i])];var component=i<4?ItemKind.BasicAmmoComponent328:ItemKind.PrecisionAmmoComponent328;
    Check(recipe.Amount==(i%4==3?150:300)&&recipe.Level==(i<4?1:2)&&recipe.Costs.Single(c=>c.Kind==component).Count==componentCounts[i],"328 ammunition output batch workbench level and component quantity "+recipe.Output);
    double cost=recipe.Costs.Sum(c=>c.Count*(AmmoCrafting328.IsComponent(c.Kind)?c.Kind==ItemKind.BasicAmmoComponent328?35d:67.5d:RaidInventory.Value(c.Kind)));double sale=recipe.Amount*RaidInventory.Value(recipe.Output);
    Check(sale/cost>=1.33&&sale/cost<=1.37,"328 full-chain own-material resale gain remains33to37percent "+recipe.Output);
   }
   Check(CampRules.Recipes[RecipeIndex328(ItemKind.BasicAmmoComponent328)].Amount==100&&CampRules.Recipes[RecipeIndex328(ItemKind.PrecisionAmmoComponent328)].Amount==100,"328 both intermediates produce100per batch");
  }
  void PlanningIsolationChecks328(){
   ResetProduction328();var basic=CampRules.Recipes[RecipeIndex328(ItemKind.BasicAmmoComponent328)];SupplyProduction328(ItemKind.Scrap,15);SupplyProduction328(ItemKind.GunParts,5);SupplyProduction328(ItemKind.Powder,5);string before=JsonUtility.ToJson(G.Save);
   Check(AmmoCrafting328.TryPlan(G.Save,basic,1,false,false,false,out var plan,out var reason)&&plan.Count(ItemKind.BasicAmmoComponent328)==100&&plan.Count(ItemKind.Scrap)==0&&JsonUtility.ToJson(G.Save)==before,"328 preview consumes certified component materials on a clone and leaves save untouched");
   foreach(int batches in new[]{0,2,4,-1,int.MaxValue})Check(!AmmoCrafting328.TryPlan(G.Save,basic,batches,true,true,false,out plan,out reason)&&JsonUtility.ToJson(G.Save)==before,"328 invalid batch request is rejected without changing save "+batches);
   Check(!AmmoCrafting328.TryPlan(G.Save,new CampRecipe("unregistered",ItemKind.BasicAmmoComponent328,100,0),1,true,false,false,out plan,out reason)&&JsonUtility.ToJson(G.Save)==before,"328 unregistered recipes cannot mint arbitrary items");
   foreach(var ordinary in new[]{ItemKind.Scrap,ItemKind.GunParts,ItemKind.Powder}){
    ResetProduction328();foreach(var c in basic.Costs)SupplyProduction328(c.Kind,c.Count,c.Kind!=ordinary);before=WarehouseState328();
    Check(!G.Craft(RecipeIndex328(ItemKind.BasicAmmoComponent328),true)&&WarehouseState328()==before&&G.Save.Stored[(int)ItemKind.BasicAmmoComponent328]==0,"328 bought or unmarked component ingredient cannot replace certified loot "+ordinary);
   }
   ResetProduction328();SupplyProduction328(ItemKind.Scrap,14);SupplyProduction328(ItemKind.GunParts,5);SupplyProduction328(ItemKind.Powder,5);before=WarehouseState328();
   Check(!G.Craft(RecipeIndex328(ItemKind.BasicAmmoComponent328),true)&&WarehouseState328()==before,"328 one missing ingredient rolls back the entire recipe");
   ResetProduction328();G.Bag.Add(ItemKind.Scrap,15,ItemOrigin.Certified);SupplyProduction328(ItemKind.GunParts,5);SupplyProduction328(ItemKind.Powder,5);before=WarehouseState328();int carried=G.Bag.Count(ItemKind.Scrap);
   Check(!G.Craft(RecipeIndex328(ItemKind.BasicAmmoComponent328),true)&&WarehouseState328()==before&&G.Bag.Count(ItemKind.Scrap)==carried,"328 production cannot silently consume materials still in backpack");
  }
  void ActualBatchChecks328(){
   foreach(int batches in new[]{1,3,5}){
    ResetProduction328();foreach(var c in CampRules.Recipes[RecipeIndex328(ItemKind.BasicAmmoComponent328)].Costs)SupplyProduction328(c.Kind,c.Count*batches);Check(G.Craft(RecipeIndex328(ItemKind.BasicAmmoComponent328),false,batches)&&G.Save.Stored[(int)ItemKind.BasicAmmoComponent328]==100*batches&&G.Save.Certified[(int)ItemKind.BasicAmmoComponent328]==0,"328 actual component craft commits supported batch count "+batches);
   }
   foreach(bool rare in new[]{false,true}){
    ResetProduction328();SupplyProduction328(ItemKind.BasicAmmoComponent328,50,false);SupplyProduction328(rare?ItemKind.RareMetalPowder:ItemKind.PrecisionShaftBushing,5);Check(G.Craft(RecipeIndex328(ItemKind.PrecisionAmmoComponent328,rare),false)&&G.Save.Stored[(int)ItemKind.BasicAmmoComponent328]==0&&G.Save.Stored[(int)ItemKind.PrecisionAmmoComponent328]==100,"328 actual precision route consumes five certified alternative inputs "+rare);
    ResetProduction328();SupplyProduction328(ItemKind.BasicAmmoComponent328,50,false);SupplyProduction328(rare?ItemKind.RareMetalPowder:ItemKind.PrecisionShaftBushing,5,false);string before=WarehouseState328();Check(!G.Craft(RecipeIndex328(ItemKind.PrecisionAmmoComponent328,rare),true)&&WarehouseState328()==before,"328 both precision alternatives reject purchased uncertified inputs "+rare);
   }
   foreach(var k in AmmoKinds328)foreach(int batches in new[]{1,3,5}){
    ResetProduction328();var recipe=CampRules.Recipes[RecipeIndex328(k)];foreach(var c in recipe.Costs)SupplyProduction328(c.Kind,c.Count*batches,false);
    Check(G.Craft(RecipeIndex328(k),false,batches)&&G.Save.Stored[(int)k]==recipe.Amount*batches&&G.Save.Warehouse.All(p=>p.Kind==k)&&G.Save.Certified[(int)k]==0,"328 actual finished ammunition consumes exact inventory and commits full batch "+k+" x"+batches);
    var disk=JsonUtility.FromJson<BaseSave>(File.ReadAllText(G.SavePath));Check(disk.Stored[(int)k]==recipe.Amount*batches&&disk.Warehouse.Sum(p=>p.Kind==k?p.Count:0)==recipe.Amount*batches,"328 finished output persists in isolated save "+k+" x"+batches);
   }
  }
  void AutomaticChainChecks328(){
   ResetProduction328();SupplyProduction328(ItemKind.Scrap,15);SupplyProduction328(ItemKind.GunParts,5);SupplyProduction328(ItemKind.Powder,5);
   Check(G.Craft(RecipeIndex328(ItemKind.Green9mm),false,1,true)&&G.Save.Stored[(int)ItemKind.Green9mm]==300&&G.Save.Stored[(int)ItemKind.BasicAmmoComponent328]==36&&G.Save.Stored[(int)ItemKind.Scrap]==0,"328 one-click green production keeps36unused newly processed components");
   ResetProduction328();SupplyProduction328(ItemKind.BasicAmmoComponent328,64,false);SupplyProduction328(ItemKind.Scrap,15);SupplyProduction328(ItemKind.GunParts,5);SupplyProduction328(ItemKind.Powder,5);
   Check(G.Craft(RecipeIndex328(ItemKind.Green9mm),false,1,true)&&G.Save.Stored[(int)ItemKind.BasicAmmoComponent328]==0&&G.Save.Certified[(int)ItemKind.Scrap]==15&&G.Save.Certified[(int)ItemKind.Powder]==5,"328 automatic processing uses existing components first without burning fresh collectibles");
   ResetProduction328();SupplyProduction328(ItemKind.Scrap,60);SupplyProduction328(ItemKind.GunParts,20);SupplyProduction328(ItemKind.Powder,20);
   Check(G.Craft(RecipeIndex328(ItemKind.Green9mm),false,5,true)&&G.Save.Stored[(int)ItemKind.Green9mm]==1500&&G.Save.Stored[(int)ItemKind.BasicAmmoComponent328]==80,"328 five-batch auto processing rounds component manufacturing upward and retains80surplus");
   foreach(bool rare in new[]{false,true}){
    ResetProduction328();SupplyProduction328(ItemKind.Scrap,15);SupplyProduction328(ItemKind.GunParts,5);SupplyProduction328(ItemKind.Powder,5);SupplyProduction328(rare?ItemKind.RareMetalPowder:ItemKind.PrecisionShaftBushing,5);
    Check(G.Craft(RecipeIndex328(ItemKind.Blue9mm),false,1,true,rare)&&G.Save.Stored[(int)ItemKind.Blue9mm]==300&&G.Save.Stored[(int)ItemKind.BasicAmmoComponent328]==50&&G.Save.Stored[(int)ItemKind.PrecisionAmmoComponent328]==57,"328 complete two-stage blue production preserves50basic57precision components "+rare);
   }
   ResetProduction328();SupplyProduction328(ItemKind.Scrap,15);SupplyProduction328(ItemKind.GunParts,5);SupplyProduction328(ItemKind.Powder,5);string before=WarehouseState328();
   Check(!G.Craft(RecipeIndex328(ItemKind.Blue9mm),true,1,true)&&WarehouseState328()==before&&G.Save.Stored[(int)ItemKind.BasicAmmoComponent328]==0,"328 missing final-stage material cannot partially commit base processing");
   ResetProduction328();SupplyProduction328(ItemKind.BasicAmmoComponent328,70,false);SupplyProduction328(ItemKind.Powder,3);before=WarehouseState328();Check(!G.Craft(RecipeIndex328(ItemKind.GreenAmmo),false)&&WarehouseState328()==before,"328 optional certified supplementary ammo input requires explicit permission");Check(G.Craft(RecipeIndex328(ItemKind.GreenAmmo),true)&&G.Save.Stored[(int)ItemKind.GreenAmmo]==300,"328 explicit certified-input permission permits supplementary firepowder consumption");
  }
  void TransactionFailureChecks328(){
   ResetProduction328();var full=new RaidInventory(18,20,true);full.Add(ItemKind.BasicAmmoComponent328,100);while(full.Slots.Count<360)full.Add(ItemKind.EntranceStorageKey2760,1);G.Save.Warehouse=full.Snapshot();CampRules.SyncStock(G.Save);string before=WarehouseState328();
   Check(!G.Craft(RecipeIndex328(ItemKind.Green9mm),false)&&WarehouseState328()==before&&G.Save.Stored[(int)ItemKind.BasicAmmoComponent328]==100,"328 full warehouse rejects ammo output without consuming components or growing storage");
   ResetProduction328();SupplyProduction328(ItemKind.Scrap,15);SupplyProduction328(ItemKind.GunParts,5);SupplyProduction328(ItemKind.Powder,5);before=WarehouseState328();string path=G.SavePath;string blocker=Path.Combine(folder,"328-production-save-blocker");File.WriteAllText(blocker,"intentional write failure");G.SavePath=Path.Combine(blocker,"stash.json");
   try{Check(!G.Craft(RecipeIndex328(ItemKind.Green9mm),false,1,true)&&WarehouseState328()==before&&G.Save.Stored[(int)ItemKind.BasicAmmoComponent328]==0&&G.Save.Stored[(int)ItemKind.Green9mm]==0,"328 failed disk write rolls back all raw consumption intermediates and finished ammo");}finally{G.SavePath=path;}
   G.Panel=CampPanel.Warehouse;before=WarehouseState328();Check(!G.Craft(RecipeIndex328(ItemKind.BasicAmmoComponent328),true)&&WarehouseState328()==before,"328 production requires the out-of-raid workbench panel");G.Panel=CampPanel.Workbench;G.Phase=GamePhase.Raid;Check(!G.Craft(RecipeIndex328(ItemKind.BasicAmmoComponent328),true)&&WarehouseState328()==before,"328 component production unavailable inside a raid");G.Phase=GamePhase.Base;
  }
  void PermissionAndMigrationChecks328(){
   long day=QuestRules319.DayAt(DateTime.UtcNow);var legacyTask=new Quest319{Id="long:cmd-entry",Template="cmd-entry",Accepted=true,Progress=1,Required=1,Completed=true,Period="long"};var secure=new ItemStack(ItemKind.HarborCoin,1,ItemOrigin.Certified){Cell=0};var quota=new ExchangeUse319{Id="eng-parts",Count=1,Day=day};
   var sameDay=new BaseSave{StarterGranted=true,GearGranted=true,GridInitialized=true,CampLevel=2,SafeItems=new List<ItemStack>{secure},Npc319=new NpcSave319{Initialized=true,Day=day,LastDay=day,Week=QuestRules319.WeekAt(day),SafeRows=3,Quests=new List<Quest319>{legacyTask},Exchanges=new List<ExchangeUse319>{quota}}};sameDay.Npc319.XP[2]=100;sameDay.Npc319.XP[3]=300;CampRules.Normalize(sameDay);
   Check(sameDay.Npc319.ProductionVersion328==1&&sameDay.Npc319.Quests.Count(q=>q.Period=="long")==15&&sameDay.Npc319.Quests.Any(q=>q.Id==legacyTask.Id&&q.Accepted&&q.Completed&&q.Progress==1&&!q.Claimed),"328 migration on an already initialized same-day save fills new long tasks and preserves prior flags");
   Check(sameDay.Npc319.SafeRows==3&&sameDay.SafeItems.Any(p=>p.Id==secure.Id&&p.Origin==ItemOrigin.Certified)&&sameDay.Npc319.Exchanges.Any(e=>e.Id==quota.Id&&e.Day==day&&e.Count==1)&&QuestRules319.Level(sameDay.Npc319,2)==2&&QuestRules319.Level(sameDay.Npc319,3)==3,"328 migration preserves secure storage exchange quota and previously earned relationship levels");
   var old=EmptyProduction328(2);Check(old.Npc319.ProductionVersion328==1&&old.Npc319.UpgradePermission328==2&&old.Npc319.GreenAmmoBlueprint328&&old.Npc319.BlueAmmoBlueprint328&&old.Npc319.AkBlueprint328,"328 old level2 save retains permanent workbench and blueprint permissions");
   old.Stored=new int[295];old.Certified=new int[295];CampRules.Deposit(old,ItemKind.YardHomeKey324,1);CampRules.Deposit(old,ItemKind.PrecisionAmmoComponent328,100);old.Stored=new int[295];old.Certified=new int[295];var ids=old.Warehouse.Select(p=>p.Id).OrderBy(p=>p).ToArray();CampRules.Normalize(old);Check(old.Stored.Length==297&&old.Certified.Length==297&&old.Stored[294]==1&&old.Stored[296]==100&&ids.SequenceEqual(old.Warehouse.Select(p=>p.Id).OrderBy(p=>p)),"328 existing saved arrays expand without shifting keys or losing newly stored components");
   var restored=JsonUtility.FromJson<BaseSave>(JsonUtility.ToJson(old));CampRules.Normalize(restored);Check(restored.Npc319.BlueAmmoBlueprint328&&restored.Stored[296]==100&&restored.Certified[296]==0,"328 new permissions and component stacks survive normalize and save reload");
   ResetProduction328(0);Check(!NpcProduction328.CanUpgrade(G.Save,1,out var reason)&&!NpcProduction328.CanMake(G.Save,CampRules.Recipes[RecipeIndex328(ItemKind.Green9mm)],out reason),"328 fresh characters require engineering permission and military blueprint");
   G.Npc319.XP[2]=G.Npc319.XP[3]=10000;NpcProduction328.Normalize(G.Save);Check(QuestRules319.Level(G.Npc319,2)==1&&QuestRules319.Level(G.Npc319,3)==1&&!G.Npc319.GreenAmmoBlueprint328,"328 XP added after migration cannot grind relationship or production rights");
   var daily=G.Npc319.Quests.First(q=>q.Period=="daily");daily.Completed=true;daily.XP=9999;var xp=(int[])G.Npc319.XP.Clone();Check(G.ClaimQuest319(daily.Id)&&xp.SequenceEqual(G.Npc319.XP)&&QuestRules319.Level(G.Npc319,2)==1&&QuestRules319.Level(G.Npc319,3)==1&&!G.Npc319.GreenAmmoBlueprint328,"328 actual daily reward claim never adds XP promotes relationships or grants blueprints");
   var engineer=G.Npc319.Quests.First(q=>q.Template=="eng-parts");Check(G.AcceptQuest319(engineer.Id),"328 engineering production task can be accepted independently");SupplyProduction328(ItemKind.Scrap,engineer.Required);Check(G.SubmitQuest319(engineer.Id)&&!NpcProduction328.CanUpgrade(G.Save,1,out reason),"328 completed but unclaimed engineering task does not unlock upgrade");Check(G.ClaimQuest319(engineer.Id)&&NpcProduction328.CanUpgrade(G.Save,1,out reason)&&QuestRules319.Level(G.Npc319,2)==2,"328 actual claimed engineering stage grants level1 upgrade permission");foreach(var cost in CampRules.UpgradeCosts(0))SupplyProduction328(cost.Kind,cost.Count);G.UpgradeCamp();Check(G.Save.CampLevel==1&&!NpcProduction328.CanMake(G.Save,CampRules.Recipes[RecipeIndex328(ItemKind.Green9mm)],out reason),"328 materials and engineering unlock upgrade bench but do not grant military blueprints");
   var military=G.Npc319.Quests.First(q=>q.Template=="mil-clear");military.Completed=true;Check(G.ClaimQuest319(military.Id)&&G.Npc319.GreenAmmoBlueprint328&&NpcProduction328.CanMake(G.Save,CampRules.Recipes[RecipeIndex328(ItemKind.Green9mm)],out reason)&&!G.Npc319.BlueAmmoBlueprint328,"328 claimed military stage independently grants all green ammo blueprints");
   engineer=G.Npc319.Quests.First(q=>q.Template=="eng-survey");engineer.Completed=true;Check(G.ClaimQuest319(engineer.Id)&&NpcProduction328.CanUpgrade(G.Save,2,out reason),"328 second engineering stage grants level2 upgrade permission");foreach(var cost in CampRules.UpgradeCosts(1))SupplyProduction328(cost.Kind,cost.Count);G.UpgradeCamp();Check(G.Save.CampLevel==2&&!NpcProduction328.CanMake(G.Save,CampRules.Recipes[RecipeIndex328(ItemKind.Blue9mm)],out reason),"328 bench level2 alone cannot bypass blue ammunition blueprint");military=G.Npc319.Quests.First(q=>q.Template=="mil-electronics");military.Completed=true;Check(G.ClaimQuest319(military.Id)&&G.Npc319.BlueAmmoBlueprint328&&G.Npc319.AkBlueprint328&&QuestRules319.Level(G.Npc319,3)==3,"328 second military reward grants blue ammo AK and relationship level3");
   QuestRules319.Refresh(G.Save,G.Npc319.Day+8);Check(G.Npc319.UpgradePermission328==2&&G.Npc319.GreenAmmoBlueprint328&&G.Npc319.BlueAmmoBlueprint328,"328 daily weekly rollover preserves permanent production unlocks");
   ResetProduction328(0);G.Save.CampLevel=2;military=G.Npc319.Quests.First(q=>q.Template=="mil-electronics");military.Completed=true;var full=new RaidInventory(18,20,true);while(full.Slots.Count<360)full.Add(ItemKind.EntranceStorageKey2760,1);G.Save.Warehouse=full.Snapshot();CampRules.SyncStock(G.Save);int coins=G.Save.Coins;string warehouse=WarehouseState328();Check(!G.ClaimQuest319(military.Id)&&!G.Npc319.Quests.First(q=>q.Template=="mil-electronics").Claimed&&!G.Npc319.BlueAmmoBlueprint328&&!G.Npc319.AkBlueprint328&&G.Save.Coins==coins&&WarehouseState328()==warehouse,"328 full warehouse blocks quest reward and rolls back coins claim and blueprint unlocks");
   ResetProduction328(0);engineer=G.Npc319.Quests.First(q=>q.Template=="eng-parts");engineer.Completed=true;string path=G.SavePath;string blocker=Path.Combine(folder,"328-quest-save-blocker");File.WriteAllText(blocker,"intentional write failure");G.SavePath=Path.Combine(blocker,"stash.json");try{Check(!G.ClaimQuest319(engineer.Id)&&!G.Npc319.Quests.First(q=>q.Template=="eng-parts").Claimed&&G.Npc319.UpgradePermission328==0,"328 failed reward save also rolls back newly granted production permission");}finally{G.SavePath=path;}
  }
  IEnumerator RunAmmoProduction328(){
   Check(G.VerifyMode&&G.SavePath.Contains("verification"),"328 uses isolated verification save only");FreshCamp();G.enabled=false;
   RecipeAndItemChecks328();yield return null;PlanningIsolationChecks328();ActualBatchChecks328();yield return null;AutomaticChainChecks328();TransactionFailureChecks328();yield return null;PermissionAndMigrationChecks328();results.Add("AMMO PRODUCTION328 COMPLETE");yield return null;
  }
 }
}
