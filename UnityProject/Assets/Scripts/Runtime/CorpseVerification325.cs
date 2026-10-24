using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
namespace GreyHarbor { public sealed partial class RaidVerification {
 static readonly ItemArea[] CorpseAreas325={ItemArea.CorpseWeapon325,ItemArea.CorpseArmor325,ItemArea.CorpseRigGear325,ItemArea.CorpsePackGear325,ItemArea.CorpseRig325,ItemArea.CorpseBag325};
 ItemStack CorpseItem325(ItemKind kind,int count=1,int cell=0,bool known=true)=>new ItemStack(kind,count,Certification319.Collection(kind)?ItemOrigin.Pending:ItemOrigin.Ordinary){Cell=cell,Identified=known,RaidFound=true};
 string CorpseState325(LootContainer box){var b=box.Corpse325;var lists=b==null?new[]{box.Items}:new[]{box.Items,b.WeaponItems,b.ArmorItems,b.RigGearItems,b.PackGearItems,b.RigItems,b.BagItems};return string.Join("|",lists.Select(l=>string.Join(";",l.Select(p=>JsonUtility.ToJson(p)))));}
 string CarryState325()=>string.Join("|",new[]{ItemArea.Weapon,ItemArea.SecondWeapon,ItemArea.Armor,ItemArea.RigEquipment,ItemArea.PackEquipment,ItemArea.Rig,ItemArea.Backpack,ItemArea.Safe}.Select(a=>G.InventoryFor(a)!=null?string.Join(";",G.InventoryFor(a).Slots.Select(p=>JsonUtility.ToJson(p))):JsonUtility.ToJson(G.GearAt(a))));
 void EmptyCorpseCarry325(){EmptyCarry320();G.Player.Health=PlayerLife321.Maximum;G.Paused=false;G.InventoryOpen=true;G.Panel=CampPanel.None;G.Phase=GamePhase.Raid;G.TrainingActive=false;G.Player.transform.position=new Vector3(500,0,500);G.Player.Aim=Vector3.forward;G.Controls=new TouchInput();G.CloseKeyCase314();Physics.SyncTransforms();}
 LootContainer CorpseFixture325(bool known=true){
  EmptyCorpseCarry325();var body=new CorpseLoot325{Soldier=true,RigRecoverable=true,PackRecoverable=true,WeaponRecoverable=true,ArmorRecoverable=true,EnemyName="QA corpse",RigColumns=4,RigRows=2,BagColumns=4,BagRows=3};
  body.WeaponItems.Add(CorpseItem325(ItemKind.AK,1,0,known));body.ArmorItems.Add(CorpseItem325(ItemKind.Armor,1,0,known));body.RigGearItems.Add(CorpseItem325(ItemKind.SmallRig,1,0,known));body.PackGearItems.Add(CorpseItem325(ItemKind.SmallPack,1,0,known));body.RigItems.Add(CorpseItem325(ItemKind.GreenAmmo,20,0,known));body.BagItems.Add(CorpseItem325(ItemKind.Medical,1,0,known));
  var box=new LootContainer{Corpse325=body,Position=G.Player.transform.position,Opened=true,Known=true,Name="QA corpse"};G.Map.Loot.Add(box);G.OpenLoot=box;return box;
 }
 bool CorpseFits325(List<ItemStack> items,int columns,int rows){var grid=new RaidInventory(columns,rows);foreach(var p in items){if(p==null||p.Count<1||p.Count>grid.StackLimit(p.Kind)||!grid.Fits(p,p.Cell))return false;grid.Slots.Add(p);}return true;}
 void CorpseIdentificationChecks325(){
  var box=CorpseFixture325(false);var body=box.Corpse325;
  Check(CorpseAreas325.All(a=>G.ItemAt(a,0)==null&&!G.CanMoveItem(a,0,ItemArea.Backpack,-1)),"325 six unknown corpse sources cannot move before identification");
  G.TickIdentification(.25f);Check(body.AllItems().All(p=>!p.Identified)&&box.IdentifyingId==body.WeaponItems[0].Id,"325 identification progresses on actual weapon source rather than empty ordinary loot");
  G.TickIdentification(.25f);Check(body.WeaponItems[0].Identified&&body.AllItems().Count(p=>p.Identified)==1,"325 first identification modifies exactly one real corpse item");
  string before=CorpseState325(box);G.CloseInventory();G.OpenLoot=box;G.InventoryOpen=true;Check(CorpseState325(box)==before&&body.WeaponItems[0].Identified,"325 close and reopen retains all contents and completed identification without reroll");
  for(int i=0;i<8;i++)G.TickIdentification(3);Check(body.AllItems().All(p=>p.Identified)&&CorpseAreas325.All(a=>G.ItemAt(a,0)!=null),"325 all six equipment and content sources become takeable after sequential identification");
  Check(G.InventoryFor(ItemArea.CorpseWeapon325).Fits(body.WeaponItems[0],0,null,body.WeaponItems[0])&&G.InventoryFor(ItemArea.CorpseArmor325).Fits(body.ArmorItems[0],0,null,body.ArmorItems[0]),"325 weapon two-high and armor three-high occupy valid real source grids");
  var copy=body.Copy();string saved=CorpseState325(box);copy.WeaponItems[0].Loaded314=4;copy.RigItems[0].Count=2;Check(CorpseState325(box)==saved,"325 corpse snapshots deeply copy source items");
 }
 void CorpseTransferChecks325(){
  var box=CorpseFixture325();G.Bag.Add(ItemKind.Cola,1);var carried=G.Bag.Slots[0];string corpse=CorpseState325(box),player=CarryState325();
  foreach(var area in CorpseAreas325)Check(!G.MoveItem(ItemArea.Backpack,carried.Cell,area,0)&&CorpseState325(box)==corpse&&CarryState325()==player,"325 cannot deposit or swap player item into corpse "+area);
  Check(!G.CanMoveItem(ItemArea.CorpseBag325,0,ItemArea.Backpack,carried.Cell)&&!G.MoveItem(ItemArea.CorpseBag325,0,ItemArea.Backpack,carried.Cell)&&CorpseState325(box)==corpse&&CarryState325()==player,"325 dragging corpse collectible onto different occupied item cannot return displaced item to corpse");
  Check(!G.SplitItem(ItemArea.CorpseRig325,0,10)&&CorpseState325(box)==corpse,"325 split cannot create new stacks in read-only corpse");
  Check(!G.CanUse(ItemArea.CorpseRig325,box.Corpse325.RigItems[0]),"325 corpse ammunition cannot directly reload player before pickup");box.Corpse325.RigItems.Add(CorpseItem325(ItemKind.GreenArmorKit,1,1));G.Armor.ArmorPoints=0;Check(!G.CanUse(ItemArea.CorpseRig325,box.Corpse325.RigItems[1])&&!G.UseItem(ItemArea.CorpseRig325,1)&&G.RepairTime==0,"325 corpse repair pack cannot be used directly before pickup");
  Check(!G.QuickTransfer(ItemArea.CorpseRigGear325,0)&&box.Corpse325.RigGearItems.Count==1,"325 populated corpse rig must be emptied before recovering its body");Check(!G.DropItem(ItemArea.CorpsePackGear325,0)&&box.Corpse325.PackGearItems.Count==1,"325 dropping populated corpse backpack cannot discard contained loot");
  Check(G.QuickTransfer(ItemArea.CorpseRig325,0)&&G.Chest.Count(ItemKind.GreenAmmo)==20&&box.Corpse325.RigItems.Count==1,"325 corpse ammo goes to player rig with source decremented once");Check(G.QuickTransfer(ItemArea.CorpseRig325,1)&&G.Chest.Count(ItemKind.GreenArmorKit)==1&&box.Corpse325.RigItems.Count==0,"325 corpse pack goes to player rig with repair points preserved");
  Check(G.QuickTransfer(ItemArea.CorpseRigGear325,0)&&box.Corpse325.RigGearItems.Count==0&&G.Bag.Slots.Any(p=>p.Kind==ItemKind.SmallRig),"325 emptied corpse rig body may be picked up without overwriting worn player rig");
  G.SetInventoryPreferences(false,G.AutoSafeThreshold);G.Bag.Slots.Clear();G.Chest.Slots.Clear();for(int i=0;i<G.Bag.SlotCapacity;i++)G.Bag.Slots.Add(new ItemStack(ItemKind.Cola,10){Cell=i});for(int i=0;i<G.Chest.SlotCapacity;i++)G.Chest.Slots.Add(new ItemStack(ItemKind.Cola,10){Cell=i});
  // Use light ammunition for a full-grid fixture without accidentally exceeding45kg.
  foreach(var p in G.Bag.Slots.Concat(G.Chest.Slots)){p.Kind=ItemKind.Green9mm;p.Count=100;}G.Bag.Slots[0].Count=1;G.Chest.Slots[0].Count=1;
  Check(G.CarryGrams320<45000,"325 full carry fixture remains under weight limit");Check(G.QuickTransfer(ItemArea.CorpseBag325,0)&&G.Safe.Count(ItemKind.Medical)==1&&box.Corpse325.BagItems.Count==0,"325 full rig and backpack still allow low-value collectible into safety case without automatic threshold");
  EmptyCorpseCarry325();G.OpenLoot=box;G.InventoryOpen=true;box.Corpse325.WeaponItems[0].Identified=true;G.Equipped=null;G.Magazine=G.MagazineFound=0;Check(G.QuickTransfer(ItemArea.CorpseWeapon325,0)&&G.Equipped!=null&&G.Equipped.Kind==ItemKind.AK&&box.Corpse325.WeaponItems.Count==0,"325 corpse weapon can equip empty primary slot exactly once");
  G.CloseInventory();Check(!G.MoveItem(ItemArea.CorpseArmor325,0,ItemArea.Backpack,-1),"325 closed corpse source is inaccessible after inventory closes");
 }
 void CorpseTransactionChecks325(){
  var box=CorpseFixture325();FillWeight320(45000);G.InventoryOpen=true;G.OpenLoot=box;G.Player.transform.position=box.Position;G.Paused=false;string before=CorpseState325(box),carry=CarryState325();
  Check(!G.MoveItem(ItemArea.CorpseRig325,0,ItemArea.Rig,0,1)&&G.CarryGrams320==45000&&CorpseState325(box)==before&&CarryState325()==carry,"325 overweight pickup restores all six corpse lists and player inventories atomically");
  EmptyCorpseCarry325();G.OpenLoot=box;G.InventoryOpen=true;Call314(G,"CaptureCarry");before=CorpseState325(box);carry=CarryState325();string saveBefore=JsonUtility.ToJson(G.Save),path=G.SavePath;string blocker=Path.Combine(folder,"corpse325-write-blocker");File.WriteAllText(blocker,"not a directory");G.SavePath=Path.Combine(blocker,"stash.json");
  Check(!G.MoveItem(ItemArea.CorpseRig325,0,ItemArea.Rig,0)&&CorpseState325(box)==before&&CarryState325()==carry&&JsonUtility.ToJson(G.Save)==saveBefore,"325 failed write rolls pickup back without corpse loss or duplicated player item");G.SavePath=path;
  box.Corpse325.WeaponItems[0].Attachments30.Add(CorpseItem325(ItemKind.BlueOptic30));before=CorpseState325(box);carry=CarryState325();G.SavePath=Path.Combine(blocker,"stash.json");Check(!G.RemoveMod30(ItemArea.CorpseWeapon325,0,ModSlot30.Optic)&&CorpseState325(box)==before&&CarryState325()==carry,"325 failed save restores detached corpse attachment and player destination");G.SavePath=path;
 }
 void CorpseWeaponChecks325(){
  var box=CorpseFixture325();var gun=box.Corpse325.WeaponItems[0];gun.MagazineReady314=true;gun.Loaded314=gun.LoadedFound314=7;gun.LoadedAmmo314=ItemKind.GreenAmmo;gun.Attachments30.Add(CorpseItem325(ItemKind.BlueOptic30));
  Check(G.UnloadCarriedGun314(ItemArea.CorpseWeapon325,0)&&box.Corpse325.WeaponItems[0].Loaded314==0&&G.Chest.Count(ItemKind.GreenAmmo)==7&&box.Corpse325.RigItems[0].Count==20,"325 corpse gun unloads real rounds into player without changing separate corpse rig ammo");
  Check(G.RemoveMod30(ItemArea.CorpseWeapon325,0,ModSlot30.Optic)&&box.Corpse325.WeaponItems[0].Attachments30.Count==0&&G.Chest.Count(ItemKind.BlueOptic30)==1&&box.Corpse325.WeaponItems.Count==1,"325 corpse gun attachment is taken into player not inserted into corpse weapon source");
  var optic=G.Chest.Slots.Find(p=>p.Kind==ItemKind.BlueOptic30);Check(!G.InstallMod30(ItemArea.CorpseWeapon325,0,ItemArea.Rig,optic.Id)&&!G.MoveItem(ItemArea.Rig,G.Chest.Slots.Find(p=>p.Kind==ItemKind.GreenAmmo).Cell,ItemArea.CorpseWeapon325,0),"325 mod installation and ammo drag cannot write back into corpse gun");
  var pouch=CorpseItem325(ItemKind.GreenKeyCase314,1,1);box.Corpse325.BagItems.Add(pouch);G.Safe.Add(ItemKind.EntranceStorageKey2760,1);Check(G.OpenKeyCase314At(ItemArea.CorpseBag325,1),"325 corpse key pouch may be inspected");int keyCell=G.Safe.Slots[0].Cell;Check(!G.MoveItem(ItemArea.Safe,keyCell,ItemArea.KeyCase314,0)&&pouch.Keys314.Count==0,"325 nested corpse key pouch cannot bypass read-only corpse destination");G.CloseKeyCase314();
 }
 void CorpseGeneratedChecks325(){
  foreach(RebelKind kind in Enum.GetValues(typeof(RebelKind)))for(int level=1;level<=2;level++)for(int seed=0;seed<8;seed++){
   var body=CorpseDropRules325.Soldier(kind,level,new System.Random(32500+seed),seed%2==1,(DropRegion230)(seed%11));var items=body.AllItems().ToList();
   Check(body.Soldier&&body.Level==level&&body.WeaponItems.Count<=1&&body.ArmorItems.Count<=1&&body.RigGearItems.Count<=1&&body.PackGearItems.Count<=1&&CorpseFits325(body.WeaponItems,4,2)&&CorpseFits325(body.ArmorItems,3,3)&&CorpseFits325(body.RigGearItems,2,2)&&CorpseFits325(body.PackGearItems,2,2)&&CorpseFits325(body.RigItems,body.RigColumns,body.RigRows)&&CorpseFits325(body.BagItems,body.BagColumns,body.BagRows)&&items.All(p=>p.RaidFound&&!p.Identified&&!RetiredMedicine321.IsRetired(p.Kind))&&items.Select(p=>p.Id).Distinct().Count()==items.Count,"325 seeded soldier drop fits physical gear and content grids with unique unknown items "+kind+" / "+level+" / "+seed);
   var expectedRig=RaidInventory.Container(new ItemStack(body.RigKind,1),true);var expectedBag=RaidInventory.Container(new ItemStack(body.PackKind,1),false);
   Check(body.RigColumns==expectedRig.Columns&&body.RigRows==expectedRig.Rows&&body.BagColumns==expectedBag.Columns&&body.BagRows==expectedBag.Rows&&body.RigRecoverable==(body.RigGearItems.Count==1)&&body.PackRecoverable==(body.PackGearItems.Count==1)&&body.ArmorItems.All(p=>p.ArmorReady&&p.ArmorPoints>=ArmorBalance.Max(p.Kind)*.25f-.5f&&p.ArmorPoints<=ArmorBalance.Max(p.Kind)*.70f+.5f)&&body.RigItems.Where(p=>ConsumableRules.IsRepair(p.Kind)).All(p=>p.RepairReady320&&p.RepairPoints320>=RepairKit320.Capacity(p.Kind)*.20f-.5f&&p.RepairPoints320<=RepairKit320.Capacity(p.Kind)*.80f+.5f)&&body.WeaponItems.All(p=>p.MagazineReady314&&p.Loaded314>=0&&p.Loaded314<=ModRules30.Profile(p).MagazineSize&&p.LoadedFound314==p.Loaded314&&p.Attachments30.Count<=2&&p.Attachments30.Select(m=>ModRules30.Get(m.Kind).Slot).Distinct().Count()==p.Attachments30.Count),"325 seeded soldier real capacities recoverable flags partial durability and legal fitted gun "+kind+" / "+level+" / "+seed);
   var budgetPool=ContainerDropRules230.Profile(ContainerLook.AmmoCase,SupplyType.Ammo,ContainerTier.Common,(DropRegion230)(seed%11));var budget=ContainerDropRules230.Contents(budgetPool,new System.Random(32500+seed),seed%2==1,false);Check(body.RigItems.Where(p=>AmmoRules270.IsAmmo(p.Kind)).Sum(p=>p.Count)+body.WeaponItems.Sum(p=>p.Loaded314)==budget.Where(p=>AmmoRules270.IsAmmo(p.Kind)).Sum(p=>p.Count),"325 gun loaded rounds consume same generated ammunition budget "+kind+" / "+level+" / "+seed);
  }
  foreach(MonsterKind kind in Enum.GetValues(typeof(MonsterKind)))for(int level=1;level<=2;level++)for(int seed=0;seed<8;seed++){
   var items=new List<ItemStack>();var body=CorpseDropRules325.Monster(kind,level,new System.Random(32500+seed),seed%2==1,(DropRegion230)(seed%11),items);
   Check(!body.Soldier&&body.Level==level&&body.AllItems().Count()==0&&items.Count>=1&&items.Count<=5&&CorpseFits325(items,body.BioColumns,body.BioRows)&&items.All(p=>p.RaidFound&&!p.Identified&&!RetiredMedicine321.IsRetired(p.Kind)&&RaidInventory.Category(p.Kind)==CollectionCategory.Biological&&RaidInventory.Grade(p.Kind)<=(level==1?2:3))&&items.Select(p=>p.Id).Distinct().Count()==items.Count,"325 seeded monster loot uses only fixed biological grid with capped quality and count "+kind+" / "+level+" / "+seed);
  }
 }
 void CorpseDeathChecks325(){
  EmptyCorpseCarry325();G.CloseInventory();G.Player.transform.position=new Vector3(900,0,900);int before=G.Map.Loot.Count;var soldier=G.SpawnRebel145(G.Player.transform.position,RebelKind.Heavy,"QA325");soldier.Damage(10000);G.RebelDeath145(soldier);var body=soldier.CorpseContainer325;Check(body!=null&&body.Corpse325.Soldier&&G.Map.Loot.Count==before+1,"325 real rebel death creates one structured cached corpse");string contents=CorpseState325(body);G.RebelDeath145(soldier);Check(G.Map.Loot.Count==before+1&&soldier.CorpseContainer325==body&&CorpseState325(body)==contents,"325 repeated rebel death callback cannot reroll or duplicate corpse");
  Check(!body.Opened&&G.CanInteractLoot(body)&&Mathf.Abs(body.SearchDuration-3)<.001f,"325 unopened rebel corpse with empty ordinary list is physically searchable for three seconds");F111(false,0);F111(false,0);F111(true,2.99f);Check(!body.Opened&&!G.InventoryOpen,"325 actual F hold does not open soldier corpse before search duration");F111(true,.02f);Check(body.Opened&&G.OpenLoot==body&&G.InventoryOpen&&CorpseState325(body)==contents,"325 actual F hold opens structured soldier sources without changing loot");G.CloseInventory();F111(false,0);
  G.Player.transform.position+=Vector3.right*5;var monster=(RaidActor)Call314(G,"Actor","QA325 monster",G.Player.transform.position,true);monster.ConfigureMonster(MonsterKind.Runner);monster.Damage(10000);before=G.Map.Loot.Count;G.MonsterDeath(monster);var bio=monster.CorpseContainer325;Check(bio!=null&&!bio.Corpse325.Soldier&&G.Map.Loot.Count==before+1,"325 real monster death creates one fixed biological corpse");contents=CorpseState325(bio);G.MonsterDeath(monster);Check(G.Map.Loot.Count==before+1&&monster.CorpseContainer325==bio&&CorpseState325(bio)==contents,"325 repeated monster death callback cannot reroll or duplicate corpse");
  Check(!bio.Opened&&G.CanInteractLoot(bio)&&Mathf.Abs(bio.SearchDuration-1.5f)<.001f,"325 unopened biological corpse uses normal1.5-second clothes search duration");F111(false,0);F111(true,bio.SearchDuration+.01f);Check(bio.Opened&&G.OpenLoot==bio&&G.InventoryOpen,"325 actual F opens biological corpse");var grid=G.EnsureLootGrid(bio);Check(grid.Columns==bio.Corpse325.BioColumns&&grid.Rows==bio.Corpse325.BioRows&&ReferenceEquals(grid.Slots,bio.Items),"325 monster search inventory retains fixed capacity and real source backing");
 }

 IEnumerator CorpseAreaCacheChecks325(){
  G.CloseInventory();G.Player.transform.position=G.Map.Spawn;G.SwitchInterior("ferry1");G.enabled=false;G.Paused=false;yield return null;
  var inside=G.Map;var enemy=G.Enemies.Find(e=>e.IsRebel);Check(inside.IsInterior&&enemy!=null,"325 actual ferry soldier exists for persistent corpse test");
  if(enemy==null)yield break;
  enemy.Damage(10000);G.RebelDeath145(enemy);var corpse=enemy.CorpseContainer325;corpse.Opened=true;
  var item=corpse.Corpse325.AllItems().FirstOrDefault();if(item!=null)item.Identified=true;
  string contents=CorpseState325(corpse);int count=inside.Loot.Count;
  G.SwitchInterior("ferry1");Check(!G.Map.IsInterior,"325 actual corpse room exits to exterior");
  G.SwitchInterior("ferry1");yield return null;
  Check(ReferenceEquals(G.Map,inside)&&G.Map.Loot.Contains(corpse)&&G.Map.Loot.Count==count&&corpse.Opened&&CorpseState325(corpse)==contents,"325 re-entering actual ferry preserves generated corpse and completed identification");
  G.RebelDeath145(enemy);Check(G.Map.Loot.Count==count&&CorpseState325(corpse)==contents,"325 death callback after room re-entry still cannot duplicate corpse");
  G.SwitchInterior("ferry1");G.Paused=false;yield return null;
 }

 IEnumerator RunCorpseLoot325(){
  Check(G.VerifyMode&&G.SavePath.Contains("verification-stash"),"325 corpse verification uses isolated save");FreshCamp();G.VerificationSmallMap=false;G.enabled=false;G.Chest.Slots.Clear();G.Bag.Slots.Clear();G.Safe.Slots.Clear();Check(G.StartRaid(),"325 temporary raid starts for real inventory operations");G.enabled=false;G.Paused=false;foreach(var e in G.Enemies){e.Dead=true;e.HitCollider.enabled=false;}yield return null;
  CorpseIdentificationChecks325();CorpseTransferChecks325();CorpseTransactionChecks325();CorpseWeaponChecks325();CorpseGeneratedChecks325();CorpseDeathChecks325();yield return RunCorpsePanel325();yield return CorpseAreaCacheChecks325();G.Finish(false);Check(G.Phase==GamePhase.Base,"325 corpse tests return to safe camp");results.Add("CORPSE LOOT325 COMPLETE");yield return null;
 }
} }
