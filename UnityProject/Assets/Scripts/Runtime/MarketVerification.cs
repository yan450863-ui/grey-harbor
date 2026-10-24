using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
namespace GreyHarbor {
 public sealed partial class RaidVerification {
  void MarketFixture(){FreshCamp();G.Player.transform.position=RaidMap.Market;G.Panel=CampPanel.Market;G.InventoryOpen=true;G.Save.Coins=10000;G.Paused=false;Physics.SyncTransforms();}
  IEnumerator VerifyMarket120(){
   var controls=G.Controls;MarketFixture();G.Controls=new TouchInput{AimPosition=new Vector2(Screen.width*.6f,Screen.height*.5f)};
   Check(MarketRules.Catalog(0).Count==85&&!MarketRules.Listed(ItemKind.LargeRig)&&!MarketRules.Listed(ItemKind.LargePack),"Market lists released goods and excludes unreleased rare equipment");
   Check(MarketRules.Catalog(1).TrueForAll(k=>RaidInventory.Group(k)==ItemGroup.Combat)&&MarketRules.Catalog(2).TrueForAll(k=>RaidInventory.Group(k)==ItemGroup.Collection),"Market filters retain combat and collection categories");
   Check(MarketRules.Price(ItemKind.Cola,1)==12&&MarketRules.Price(ItemKind.Cola,2)==24&&MarketRules.Price(ItemKind.Ammo,30)==90&&MarketRules.Price(ItemKind.AK,1)==120,"Market uses integer unit prices consistently for every quantity");
   foreach(var k in MarketRules.Catalog(0)){foreach(int q in new[]{1,30,999}){int price=MarketRules.Price(k,q);Check(price==(int)Math.Ceiling((decimal)RaidInventory.Value(k)*1.15m)*q&&price>RaidInventory.Value(k)*q,"Market quote and direct resale loss "+k+" x"+q);}}
   Check(MarketRules.Price((ItemKind)(-1),1)==-1&&MarketRules.Price((ItemKind)999,1)==-1&&MarketRules.Price(ItemKind.Cola,0)==-1&&MarketRules.Price(ItemKind.Cola,-1)==-1&&MarketRules.Price(ItemKind.Cola,1000)==-1,"Invalid item IDs and quantities rejected");
   G.CloseInventory();var input=(TouchInput)G.Controls;input.Searching=false;G.TickCamp(.01f);input.Searching=true;G.TickCamp(.01f);Check(G.InventoryOpen&&G.Panel==CampPanel.Market,"F at trading counter opens market");input.Searching=false;G.TickCamp(.01f);
   Check(!Physics.CheckSphere(RaidMap.Market+Vector3.up*.5f,.3f,SceneObstacle.MovementMask),"Market interaction position is walkable");
   int coins=G.Save.Coins,ammo=G.Save.Stored[(int)ItemKind.Ammo];Check(G.BuyMarket(ItemKind.Ammo,181)&&G.Save.Coins==coins-543&&G.Save.Stored[(int)ItemKind.Ammo]==ammo+181,"Purchase deducts order price and adds exact ammo quantity");
   Check(G.Save.Warehouse.FindAll(p=>p.Kind==ItemKind.Ammo).TrueForAll(p=>p.Count<=300&&p.Origin==ItemOrigin.Ordinary&&!p.RaidFound),"Purchased ammunition uses warehouse stacking and ordinary origin");
   foreach(var k in new[]{ItemKind.AK,ItemKind.Armor,ItemKind.BlueArmor,ItemKind.SmallPack}){int cells=CampRules.Grid(G.Save).UsedCells;Check(G.BuyMarket(k,1)&&CampRules.Grid(G.Save).UsedCells==cells+RaidInventory.Width(k)*RaidInventory.Height(k),"Purchase preserves equipment footprint "+k);}
   int kept=G.Save.Coins;var stock=(int[])G.Save.Stored.Clone();G.LoadSave();G.BuildCamp();Check(G.Save.Coins==kept&&G.Save.Stored[(int)ItemKind.Ammo]==stock[(int)ItemKind.Ammo]&&G.Save.Stored[(int)ItemKind.Armor]==stock[(int)ItemKind.Armor],"Purchased stock and coins persist after reload");
   MarketFixture();G.Save.Coins=24;Check(G.BuyMarket(ItemKind.Cola,2)&&G.Save.Coins==0&&G.Save.Stored[(int)ItemKind.Cola]==2,"Exact balance purchase reaches zero without overdraft");
   Check(!G.BuyMarket(ItemKind.Cola,1)&&G.Save.Coins==0&&G.Save.Stored[(int)ItemKind.Cola]==2,"Insufficient coins leave inventory unchanged");
   MarketFixture();string before=JsonUtility.ToJson(G.Save);Check(!G.BuyMarket(ItemKind.LargePack,1)&&JsonUtility.ToJson(G.Save)==before,"Unlisted rare equipment cannot be bought");
   G.Save.Coins=int.MaxValue;G.Save.Warehouse.Clear();CampRules.Grid(G.Save).Add(ItemKind.Scrap,7200);CampRules.SyncStock(G.Save);before=JsonUtility.ToJson(G.Save);
   Check(!G.BuyMarket(ItemKind.Cola,1)&&JsonUtility.ToJson(G.Save)==before,"Full warehouse rejects purchase without charging");
   // A partial-stack fit followed by overflow must not partially charge or deliver.
   G.Save.Warehouse[0].Count=19;CampRules.SyncStock(G.Save);before=JsonUtility.ToJson(G.Save);Check(!G.BuyMarket(ItemKind.Scrap,2)&&JsonUtility.ToJson(G.Save)==before,"Partially fitting purchase rolls back all quantity");
   Check(G.BuyMarket(ItemKind.Scrap,1)&&G.Save.Stored[(int)ItemKind.Scrap]==7200,"Existing stack room allows purchase in full grid");
   G.Save.Warehouse.RemoveAll(p=>p.Cell%18==0);CampRules.SyncStock(G.Save);before=JsonUtility.ToJson(G.Save);Check(!G.BuyMarket(ItemKind.Armor,1)&&JsonUtility.ToJson(G.Save)==before,"Fragmented warehouse requires contiguous armor footprint");
   MarketFixture();string path=G.SavePath;File.WriteAllText(Path.Combine(folder,"market-save-blocker"),"block");G.SavePath=Path.Combine(folder,"market-save-blocker","stash.json");before=JsonUtility.ToJson(G.Save);Check(!G.BuyMarket(ItemKind.Cola,1)&&JsonUtility.ToJson(G.Save)==before,"Disk failure restores both coins and goods");G.SavePath=path;
   G.Panel=CampPanel.Warehouse;Check(!G.BuyMarket(ItemKind.Cola,1),"Purchase requires market panel");G.Panel=CampPanel.Market;G.Player.transform.position=RaidMap.CampSpawn;Check(!G.BuyMarket(ItemKind.Cola,1),"Purchase requires proximity to trading counter");G.Player.transform.position=RaidMap.Market;G.CampLocked=true;Check(!G.BuyMarket(ItemKind.Cola,1),"Locked save rejects purchase");G.CampLocked=false;
   MarketFixture();G.Save.Warehouse.Clear();CampRules.SyncStock(G.Save);int start=G.Save.Coins;G.BuyMarket(ItemKind.Cola,2);G.Panel=CampPanel.Warehouse;Check(G.SellWarehouse(new HashSet<string>{G.Save.Warehouse[0].Id})&&G.Save.Coins==start-4,"Bought items remain sellable at original price without direct profit");
   MarketFixture();G.Save.Coins=1000;G.NoticeTime=0;yield return Capture("210-market-buy.png");G.CloseInventory();G.Rig.Tick(0);yield return Capture("211-market-camp.png");G.Controls=controls;G.Paused=true;
  }
 }
}
