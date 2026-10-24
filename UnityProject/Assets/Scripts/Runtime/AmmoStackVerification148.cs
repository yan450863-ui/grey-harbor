using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
namespace GreyHarbor { public sealed partial class RaidVerification {
 void AmmoCamp148(){FreshCamp();G.Chest.Slots.Clear();G.Bag.Slots.Clear();G.Safe.Slots.Clear();G.Save.Warehouse.Clear();CampRules.SyncStock(G.Save);G.Panel=CampPanel.Warehouse;G.InventoryOpen=true;}
 IEnumerator VerifyAmmoStacks148(){
  var carried=new RaidInventory(5,4);carried.Add(ItemKind.Ammo,150);
  Check(carried.Slots.Count==2&&carried.At(0).Count==100&&carried.At(1).Count==50&&carried.Count(ItemKind.Ammo)==150,"148 150 reserve rounds occupy two carried cells without loss");
  var warehouse=new RaidInventory(18,20,true);warehouse.Add(ItemKind.Ammo,601);
  Check(warehouse.Slots.Count==3&&warehouse.At(0).Count==300&&warehouse.At(1).Count==300&&warehouse.At(2).Count==1,"148 601 warehouse rounds split 300 / 300 / 1");
  var legacy=new BaseSave{Version=7,StarterGranted=true,GridInitialized=true,GearGranted=true,RigGear=new ItemStack(ItemKind.StandardRig,1),PackGear=new ItemStack(ItemKind.StandardPack,1)};
  var oldWarehouse=new ItemStack(ItemKind.Ammo,180,ItemOrigin.Certified){Cell=3};var oldCarry=new ItemStack(ItemKind.Ammo,60,ItemOrigin.Certified){Cell=4,RaidFound=true};legacy.Warehouse.Add(oldWarehouse);legacy.CampRig.Add(oldCarry);CampRules.Normalize(legacy);
  Check(legacy.Warehouse[0].Id==oldWarehouse.Id&&legacy.Warehouse[0].Cell==3&&legacy.Warehouse[0].Count==180&&legacy.CampRig[0].Id==oldCarry.Id&&legacy.CampRig[0].Cell==4&&legacy.CampRig[0].Count==60&&legacy.CampRig[0].RaidFound&&legacy.Certified[0]==180,"148 Existing 180 / 60 saves retain quantity identity layout and certification");
  AmmoCamp148();CampRules.Deposit(G.Save,ItemKind.Ammo,300);int cell=FindCell(ItemArea.Warehouse,ItemKind.Ammo);
  Check(G.QuickTransfer(ItemArea.Warehouse,cell)&&G.Chest.Count(ItemKind.Ammo)==300&&G.Chest.Slots.Count==3&&G.Chest.Slots.TrueForAll(p=>p.Count==100)&&G.Save.Stored[0]==0,"148 Double-click warehouse 300 splits into three rig stacks");
  while(G.Chest.Slots.Count>0)Check(G.QuickTransfer(ItemArea.Rig,G.Chest.Slots[0].Cell),"148 Rig stack returns to warehouse");
  Check(G.Save.Stored[0]==300&&G.Save.Warehouse.Count==1&&G.Save.Warehouse[0].Count==300,"148 Returning three rig stacks merges to one warehouse stack");
  Check(G.SortWarehouse(),"148 Warehouse sort accepts 300 stack");G.LoadSave();G.BuildCamp();
  Check(G.Save.Stored[0]==300&&G.Save.Warehouse[0].Count==300&&G.Chest.Count(ItemKind.Ammo)==0,"148 Warehouse 300 persists after save reload");
  AmmoCamp148();CampRules.Deposit(G.Save,ItemKind.Ammo,301);cell=FindCell(ItemArea.Warehouse,ItemKind.Ammo);
  Check(G.MoveItem(ItemArea.Warehouse,cell,ItemArea.Backpack,7)&&G.Bag.At(7).Count==100&&G.Bag.Count(ItemKind.Ammo)==300&&G.Bag.Slots.Count==3&&G.Save.Stored[0]==1,"148 Dragging warehouse 300 uses target then splits remainder");
  AmmoCamp148();G.Chest=new RaidInventory(1,1);CampRules.Deposit(G.Save,ItemKind.Ammo,300);cell=FindCell(ItemArea.Warehouse,ItemKind.Ammo);
  Check(G.MoveItem(ItemArea.Warehouse,cell,ItemArea.Rig,-1)&&G.Chest.At(0).Count==100&&G.Save.Stored[0]==200,"148 Single free carry cell takes only 100 and leaves source 200");
  AmmoCamp148();G.Chest.Add(ItemKind.Ammo,90);G.Bag.Add(ItemKind.Ammo,20);
  Check(G.MoveItem(ItemArea.Backpack,0,ItemArea.Rig,0)&&G.Chest.At(0).Count==100&&G.Bag.At(0).Count==10,"148 Drag merge fills carried 100 cap without losing overflow");
  AmmoCamp148();G.Bag.Add(ItemKind.Ammo,100);
  Check(G.SplitItem(ItemArea.Backpack,0)&&G.Bag.At(0).Count==50&&G.Bag.At(1).Count==50,"148 Splitting carried 100 preserves two 50 stacks");
  Check(G.MoveItem(ItemArea.Backpack,1,ItemArea.Backpack,0)&&G.Bag.At(0).Count==100&&G.Bag.Slots.Count==1,"148 Split stacks merge back to carried 100");
  AmmoCamp148();G.Chest.Add(ItemKind.Ammo,100,ItemOrigin.Certified);G.Safe.Add(ItemKind.Ammo,100,ItemOrigin.Certified);G.Bag.Add(ItemKind.Ammo,100);CampRules.Deposit(G.Save,ItemKind.Ammo,300);
  Check(G.SortWarehouse(),"148 Save carried and secure 100 alongside warehouse 300");G.LoadSave();G.BuildCamp();
  Check(G.Chest.At(0).Count==100&&G.Safe.At(0).Count==100&&G.Bag.At(0).Count==100&&G.Save.Stored[0]==300,"148 Carry safe and warehouse new caps survive save reload");
  MarketFixture();G.Save.Warehouse.Clear();CampRules.SyncStock(G.Save);int coins=G.Save.Coins;
  Check(G.BuyMarket(ItemKind.Ammo,601)&&G.Save.Coins==coins-1803&&G.Save.Stored[0]==601&&G.Save.Warehouse.Count==3&&G.Save.Warehouse[0].Count==300,"148 Buying 601 costs 1803 and creates warehouse 300 stacks");
  G.Panel=CampPanel.Warehouse;var sale=G.Save.Warehouse[0];
  Check(G.SellWarehouse(new HashSet<string>{sale.Id})&&G.Save.Coins==coins-1803+600&&G.Save.Stored[0]==301,"148 Selling warehouse 300 credits actual 600 coins");
  AmmoCamp148();CampRules.Deposit(G.Save,ItemKind.Ammo,300);string path=G.SavePath;File.WriteAllText(Path.Combine(folder,"ammo148-save-blocker"),"blocked");G.SavePath=Path.Combine(folder,"ammo148-save-blocker","stash.json");cell=FindCell(ItemArea.Warehouse,ItemKind.Ammo);
  Check(!G.QuickTransfer(ItemArea.Warehouse,cell)&&G.Save.Stored[0]==300&&G.Chest.Count(ItemKind.Ammo)==0&&G.Bag.Count(ItemKind.Ammo)==0,"148 Failed save rolls back warehouse-to-carry split");G.SavePath=path;
  LiveRaid();G.Chest.Slots.Clear();G.Bag.Slots.Clear();G.Safe.Slots.Clear();bool auto=G.AutoSafeEnabled;int threshold=G.AutoSafeThreshold;G.SetInventoryPreferences(false,threshold);
  G.Chest.Add(ItemKind.Ammo,93,ItemOrigin.Certified,true);var loot=Loot124(ItemKind.Ammo,18);
  Check(G.QuickTransfer(ItemArea.Loot,0)&&loot.Items.Count==0&&G.Chest.At(0).Count==100&&G.Chest.At(1).Count==11&&G.Chest.Count(ItemKind.Ammo)==111,"148 Loot pickup fills carried 100 and creates overflow 11");
  G.Chest.Slots.Clear();G.Bag.Slots.Clear();G.Chest.Add(ItemKind.Cola,G.Chest.SlotCapacity);loot=Loot124(ItemKind.Ammo,100);
  Check(G.QuickTransfer(ItemArea.Loot,0)&&loot.Items.Count==0&&G.Bag.At(0).Count==100,"148 Full rig sends 100-round pickup to backpack");
  G.CloseInventory();G.Chest.Slots.Clear();G.Bag.Slots.Clear();G.Chest.Add(ItemKind.Ammo,100);G.Magazine=G.MagazineFound=0;G.ReloadTime=G.HealTime=G.RepairTime=0;
  for(int n=1;n<=4;n++){G.Magazine=0;G.BeginReload();G.TickActions(2.2f);int loaded=n<4?30:10;Check(G.Magazine==loaded&&G.Chest.Count(ItemKind.Ammo)==Math.Max(0,100-n*30),"148 Reload from 100 reserve conserves 30-round magazine pass "+n);}
  G.Chest.Slots.Clear();G.Chest.Add(ItemKind.Ammo,95);G.Magazine=30;G.MagazineFound=0;
  Check(G.UnloadMagazine()&&G.Magazine==0&&G.Chest.Count(ItemKind.Ammo)==125&&G.Chest.At(0).Count==100&&G.Chest.At(1).Count==25,"148 Unloading magazine fills 100 then overflows 25");
  Check(MarketRules.UnitPrice(ItemKind.Ammo)==3&&RaidInventory.Value(ItemKind.Ammo)==2&&RaidInventory.Limit(ItemKind.Medkit)==5,"148 Unit buy sell prices and medicine limits unchanged");G.SetInventoryPreferences(auto,threshold);G.Paused=true;yield return null;
 }
}}
