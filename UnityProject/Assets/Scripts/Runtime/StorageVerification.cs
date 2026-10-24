using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
namespace GreyHarbor {
 public sealed partial class RaidVerification {
  int FindCell(ItemArea area,ItemKind kind)=>G.InventoryFor(area).Slots.Find(p=>p.Kind==kind).Cell;
  void LiveRaid(){StandardRaid();G.Paused=false;foreach(var e in G.Enemies){e.Dead=true;e.HitCollider.enabled=false;}}
  IEnumerator LegacyStorageStart(){
   folder=Path.GetFullPath(Path.Combine(Application.dataPath,"../../QA"));Directory.CreateDirectory(folder);Application.logMessageReceived+=OnLog;yield return new WaitForSeconds(.5f);
   foreach(var k in RaidInventory.AvailableGear)Check(Resources.Load<Texture2D>("ItemArt/"+k)!=null,"Authored equipment art "+k);
   VerifyStorageModel();VerifyTransfers16();VerifySafety16();VerifySales16();
   StandardRaid();yield return VerifyVisionAndRoutes();yield return VerifyPresentation03();yield return VerifyDepthAndMotion04();yield return VerifyWallsAndSpawn05();
   yield return VerifyArt();VerifySearch16();VerifyTraining16();yield return VerifyHarbor16();
   results.Add("VERIFICATION COMPLETE");File.WriteAllLines(Path.Combine(folder,"verification.txt"),results);Debug.Log("GREY HARBOR VERIFICATION COMPLETE");yield return new WaitForSeconds(.2f);Application.Quit(0);
  }
  void VerifyStorageModel(){
   var inv=new RaidInventory(5,4);Check(inv.Add(ItemKind.AK,1)==0&&inv.Add(ItemKind.Helmet,1)==0&&inv.UsedCells==10,"Weapon 3x2 and helmet 2x2 reserve ten physical cells");
   Check(inv.At(6).Kind==ItemKind.AK&&inv.At(9).Kind==ItemKind.Helmet,"Interior cells resolve to their single item anchor");
   Check(!inv.Fits(ItemKind.AK,4)&&inv.Add(ItemKind.Armor,1)==1,"Grid blocks edge crossing and fragmented space for 3x3 armor");
   var snap=inv.Snapshot();var clone=inv.Clone();Check(clone.Slots[0].Id==snap[0].Id&&clone.Slots[0].Cell==snap[0].Cell,"Item identity and position survive grid snapshot restoration");
   var pack=RaidInventory.Container(new ItemStack(ItemKind.LargePack,1),false);var rig=RaidInventory.Container(new ItemStack(ItemKind.LargeRig,1),true);Check(pack.Columns==6&&pack.Rows==6&&rig.Columns==6&&rig.Rows==3,"Largest backpack and rig expose exact 6x6 and 6x3 storage");
   Check(RaidInventory.GradeName(ItemKind.Scrap)=="绿色"&&RaidInventory.GradeName(ItemKind.Medkit)=="蓝色"&&RaidInventory.GradeName(ItemKind.Circuit)=="紫色"&&RaidInventory.GradeName(ItemKind.Intel)=="金色","Rarity palette has green blue purple gold and no white tier");
   var legacy=new BaseSave{Version=4,StarterGranted=true};legacy.Stored[0]=130;legacy.Certified[0]=60;legacy.CampBag.Add(new ItemStack(ItemKind.AK,1){Cell=19});CampRules.Normalize(legacy);
   Check(legacy.Version==5&&legacy.Stored[0]==130&&legacy.Certified[0]==60&&legacy.CampBag[0].Kind==ItemKind.AK&&legacy.RigGear.Kind==ItemKind.StandardRig,"Legacy stock, certification and old carried weapons migrate without loss");
   var warehouse=CampRules.Grid(legacy);Check(warehouse.Slots.FindAll(p=>p.Kind==ItemKind.Ammo).Count==3,"Legacy totals split into bounded physical stacks");
   bool unique=true;var ids=new HashSet<string>();foreach(var p in warehouse.Slots)unique&=!string.IsNullOrEmpty(p.Id)&&ids.Add(p.Id);Check(unique,"Migrated warehouse items have unique stable identities");
   CampRules.Normalize(legacy);Check(legacy.Stored[(int)ItemKind.BlueHelmet]==1&&legacy.Stored[(int)ItemKind.SmallRig]==1,"New equipment starter package is granted exactly once");
  }
  void VerifyTransfers16(){
   FreshCamp();foreach(var k in RaidInventory.AvailableGear){int cell=FindCell(ItemArea.Warehouse,k);Check(G.UseItem(ItemArea.Warehouse,cell),"Green/blue equipment can be equipped "+k);}FreshCamp();Check(G.Chest.SlotCapacity==10&&G.Bag.SlotCapacity==20&&G.Safe.SlotCapacity==9,"Standard gear restores 10 rig cells 20 pack cells and 9 safe cells");
   int c=FindCell(ItemArea.Warehouse,ItemKind.AK);string id=G.ItemAt(ItemArea.Warehouse,c).Id;Check(G.MoveItem(ItemArea.Warehouse,c,ItemArea.Warehouse,246)&&G.ItemAt(ItemArea.Warehouse,246).Id==id&&G.ItemAt(ItemArea.Warehouse,c)==null,"Manual warehouse movement preserves identity and leaves source holes");
   G.LoadSave();G.BuildCamp();G.Panel=CampPanel.Warehouse;G.InventoryOpen=true;Check(G.ItemAt(ItemArea.Warehouse,246)?.Id==id,"Warehouse manual positions persist across reload");
   c=FindCell(ItemArea.Warehouse,ItemKind.Ammo);int n=G.ItemAt(ItemArea.Warehouse,c).Count;int stock=G.Save.Stored[0],carried=G.Chest.Count(ItemKind.Ammo);Check(G.QuickTransfer(ItemArea.Warehouse,c)&&G.Chest.Count(ItemKind.Ammo)==carried+n&&G.Save.Stored[0]==stock-n,"Warehouse ammunition double transfer prioritizes rig and conserves quantity");
   c=G.Chest.Slots[0].Cell;n=G.Chest.At(c).Count;stock=G.Save.Stored[0];Check(G.QuickTransfer(ItemArea.Rig,c)&&G.Save.Stored[0]==stock+n,"Rig quick transfer deposits into current warehouse");
   c=FindCell(ItemArea.Warehouse,ItemKind.Ammo);n=G.ItemAt(ItemArea.Warehouse,c).Count;int total=G.Save.Stored[0];Check(G.SplitItem(ItemArea.Warehouse,c,7)&&G.ItemAt(ItemArea.Warehouse,c).Count==n-7&&G.Save.Stored[0]==total,"Chosen split quantity creates a separate stack without changing totals");
   c=FindCell(ItemArea.Warehouse,ItemKind.StandardPack);Check(G.UseItem(ItemArea.Warehouse,c)&&G.Bag.SlotCapacity==20&&G.PackGear.Kind==ItemKind.StandardPack,"Right-click equipment use changes backpack capacity");
   G.Bag.Add(ItemKind.Scrap,200);c=FindCell(ItemArea.Warehouse,ItemKind.SmallPack);string before=JsonUtility.ToJson(G.Save);Check(!G.UseItem(ItemArea.Warehouse,c)&&G.PackGear.Kind==ItemKind.StandardPack&&G.Bag.Count(ItemKind.Scrap)==200,"Smaller backpack rejects contents that cannot fit without dropping anything");
   Check(!G.DropItem(ItemArea.PackEquipment,0)&&G.PackGear!=null&&G.Bag.Count(ItemKind.Scrap)==200,"Nonempty equipped containers cannot be removed or discarded");
   G.Bag.Slots.Clear();c=FindCell(ItemArea.Warehouse,ItemKind.SmallPack);Check(G.UseItem(ItemArea.Warehouse,c)&&G.Bag.Columns==4&&G.Bag.Rows==3,"Empty backpack can be changed to small 4x3 gear");
   G.Bag.Add(ItemKind.Scrap,5);c=G.Bag.Slots[0].Cell;Check(G.DropItem(ItemArea.Backpack,c)&&G.Bag.Slots.Count==0&&G.Map.Loot.Count==1&&G.Map.Loot[0].Items[0].Count==5,"Discard action drops the whole selected stack at player feet");
   G.LoadSave();G.BuildCamp();Check(G.Map.Loot.Count==1&&G.Map.Loot[0].Items[0].Count==5,"Camp discarded stacks survive restarting");
   G.Panel=CampPanel.Warehouse;G.InventoryOpen=true;c=FindCell(ItemArea.Warehouse,ItemKind.Helmet);Check(G.UseItem(ItemArea.Warehouse,c)&&G.Helmet!=null,"Helmet can be equipped from warehouse");
   c=FindCell(ItemArea.Warehouse,ItemKind.Armor);Check(G.UseItem(ItemArea.Warehouse,c)&&G.Armor!=null,"Armor can be equipped independently from chest rig");
   FreshCamp();G.Bag.Add(ItemKind.Scrap,8);string path=G.SavePath;File.WriteAllText(Path.Combine(folder,"save-blocker"),"blocked");G.SavePath=Path.Combine(folder,"save-blocker","stash.json");int old=G.Save.Stored[(int)ItemKind.Scrap];Check(!G.QuickTransfer(ItemArea.Backpack,G.Bag.Slots[0].Cell)&&G.Bag.Count(ItemKind.Scrap)==8&&G.Save.Stored[(int)ItemKind.Scrap]==old,"Failed disk save rolls back inventory and warehouse transactions");G.SavePath=path;
  }
  void VerifySafety16(){
   FreshCamp();G.Bag.Add(ItemKind.Cola,3,ItemOrigin.Certified);Check(G.MoveItem(ItemArea.Backpack,G.Bag.Slots[0].Cell,ItemArea.Safe,0)&&G.Safe.Count(ItemKind.Cola)==3,"Carried supplies can be moved into secure container");
   foreach(var k in new[]{ItemKind.AK,ItemKind.Helmet,ItemKind.Armor,ItemKind.LargeRig,ItemKind.LargePack}){CampRules.Deposit(G.Save,k,1);int c=FindCell(ItemArea.Warehouse,k);Check(!G.MoveItem(ItemArea.Warehouse,c,ItemArea.Safe,3),"Safe container rejects wearable "+k);}
   Check(G.StartRaid(),"Prepared player enters raid with new container system");G.Paused=false;foreach(var e in G.Enemies)e.Dead=true;
   Check(G.Safe.Count(ItemKind.Cola)==3&&G.Safe.FoundValue==0,"Entry carried secure supplies are certified but not counted as new loot");
   var box=G.Map.AddLoot(G.Player.transform.position,"验证物资",ItemKind.Intel,1,ItemKind.Scrap,0,false);box.Opened=true;box.Items.RemoveAll(p=>p.Count<=0);foreach(var p in box.Items)p.RaidFound=true;G.OpenLoot=box;G.InventoryOpen=true;G.Vision.Refresh(G.Player.transform.position,G.Player.Aim);G.EnsureLootGrid(box);
   Check(G.MoveItem(ItemArea.Loot,box.Items[0].Cell,ItemArea.Safe,1)&&G.Safe.Count(ItemKind.Intel)==1&&G.RaidGain==600,"New raid loot enters safe and contributes to current carried gain");
   Check(G.DropItem(ItemArea.Safe,1)&&G.Safe.Count(ItemKind.Intel)==0&&G.RaidGain==0,"Discarded newly found loot no longer contributes to carried gain");
   var drop=G.Map.Loot[G.Map.Loot.Count-1];G.OpenLoot=drop;Check(G.MoveItem(ItemArea.Loot,0,ItemArea.Safe,1)&&G.RaidGain==600,"Picking loot back up does not multiply gain");
   G.Player.Health=0;G.Finish(false);Check(G.Phase==GamePhase.Base&&G.Safe.Count(ItemKind.Cola)==3&&G.Safe.Count(ItemKind.Intel)==1&&G.Bag.Slots.Count==0&&G.Equipped==null&&G.PackGear==null,"Death preserves secure contents and loses other equipment and containers");
   FreshCamp();G.Safe.Add(ItemKind.Medkit,1);G.StartRaid();G.Finish(false);Check(G.Safe.Count(ItemKind.Medkit)==1,"Voluntary abandonment also preserves safe contents");
   LiveRaid();G.Bag.Add(ItemKind.Cola,2,ItemOrigin.Certified,true);G.InventoryOpen=true;Check(G.MoveItem(ItemArea.Backpack,G.Bag.Slots[0].Cell,ItemArea.Safe,0),"Live safe transfer persists before application exit");G.LoadSave();G.BuildCamp();Check(G.Safe.Count(ItemKind.Cola)==2&&G.Equipped==null&&G.Bag.Slots.Count==0,"Interrupted raid reload keeps safe and removes unprotected inventory");
   LiveRaid();G.Chest.Slots.Clear();G.Bag.Add(ItemKind.Ammo,12);G.Magazine=0;G.BeginReload();Check(G.ReloadTime==0,"R shortcut still cannot use backpack ammunition");G.InventoryOpen=true;int ammo=FindCell(ItemArea.Backpack,ItemKind.Ammo);Check(G.UseItem(ItemArea.Backpack,ammo)&&G.ReloadTime>0,"Right-click backpack ammunition starts manual timed reload");G.TickActions(2.2f);Check(G.Magazine==12&&G.Bag.Count(ItemKind.Ammo)==0,"Manual reload consumes exactly the selected ammunition");
   G.Bag.Add(ItemKind.Medkit,1);G.Player.Health=40;G.InventoryOpen=true;Check(G.UseItem(ItemArea.Backpack,FindCell(ItemArea.Backpack,ItemKind.Medkit)),"Right-click backpack medicine starts treatment");G.TickActions(2.6f);Check(G.Player.Health==85&&G.Bag.Count(ItemKind.Medkit)==0,"Manual treatment heals and consumes one medicine");
   G.Safe.Add(ItemKind.Medkit,1);Check(!G.CanUse(ItemArea.Safe,G.Safe.Slots[0]),"Secure supplies must be taken out before use");
   G.Finish(true);Check(G.Safe.Count(ItemKind.Medkit)==1&&G.Equipped!=null,"Successful extraction restores both carried gear and secure contents");
  }
  void VerifySales16(){FreshCamp();CampRules.Deposit(G.Save,ItemKind.Cola,5);CampRules.Deposit(G.Save,ItemKind.Intel,1,true);var selected=new HashSet<string>();int expected=0;foreach(var p in G.Save.Warehouse)if(p.Kind==ItemKind.Cola||p.Kind==ItemKind.Intel){selected.Add(p.Id);expected+=p.Count*RaidInventory.Value(p.Kind);}int coins=G.Save.Coins;Check(G.SellWarehouse(selected)&&G.Save.Coins==coins+expected&&G.Save.Stored[(int)ItemKind.Cola]==0&&G.Save.Certified[(int)ItemKind.Intel]==0,"Batch sale removes selected stacks and credits exact value");Check(!G.SellWarehouse(selected)&&G.Save.Coins==coins+expected,"Submitting old sale selection cannot duplicate coins");
   var amounts=(int[])G.Save.Stored.Clone();Check(G.SortWarehouse(),"Warehouse sorting packs existing items");bool unchanged=true;for(int i=0;i<amounts.Length;i++)unchanged&=amounts[i]==G.Save.Stored[i];Check(unchanged,"Sorting preserves all item quantities");G.LoadSave();G.BuildCamp();Check(G.Save.Coins==coins+expected,"Sale coins persist after restart");
   FreshCamp();G.Save.Warehouse.Clear();var grid=CampRules.Grid(G.Save);grid.Add(ItemKind.Scrap,(grid.SlotCapacity-1)*10);grid.Add(ItemKind.Intel,1);CampRules.SyncStock(G.Save);G.Save.CampLevel=2;G.Panel=CampPanel.Workbench;G.Craft(7,false);Check(G.Save.Stored[(int)ItemKind.Intel]==1&&G.Save.Stored[(int)ItemKind.AK]==0,"Crafting with insufficient physical output space rolls back costs");
  }
  void VerifySearch16(){LiveRaid();G.InventoryOpen=false;G.Player.transform.position=new Vector3(-6,0,-11);G.Player.Aim=Vector3.forward;G.Vision.Refresh(G.Player.transform.position,G.Player.Aim);var box=G.Map.AddLoot(G.Player.transform.position+Vector3.forward*.4f,"搜索验证",ItemKind.Ammo,12,ItemKind.Cola,1,false);box.SearchDuration=2;var input=new TouchInput{Searching=true};G.Controls=input;G.TickInteraction(.5f);var ui=G.GetComponent<RaidUI>();Check(ui.SearchVisible&&!box.Opened&&!G.InventoryOpen,"Partial search shows unknown read-only content");input.Searching=false;G.TickInteraction(.1f);Check(box.Search==0&&!ui.SearchVisible,"Releasing F cancels search and resets progress");input.Searching=true;G.TickInteraction(2.1f);Check(G.InventoryOpen&&G.OpenLoot==box&&box.Opened&&!ui.SearchVisible,"Completed search opens physical loot grid");int count=G.Chest.Count(ItemKind.Ammo);G.EnsureLootGrid(box);Check(G.QuickTransfer(ItemArea.Loot,box.Items[0].Cell)&&G.Chest.Count(ItemKind.Ammo)==count+12,"Completed container ammunition can quick-transfer to rig");G.Paused=true;}
  void VerifyTraining16(){FreshCamp();G.Safe.Add(ItemKind.Intel,1);G.NextMap=RaidDestination.Training;G.StartRaid();Check(G.TrainingActive&&G.Safe.Slots.Count==0,"Training starts with separate empty safe container");G.Safe.Add(ItemKind.Cola,8);G.Finish(true);Check(!G.TrainingActive&&G.Safe.Count(ItemKind.Intel)==1&&G.Safe.Count(ItemKind.Cola)==0,"Training cannot export protected loot and restores original safe");}
 }
}
