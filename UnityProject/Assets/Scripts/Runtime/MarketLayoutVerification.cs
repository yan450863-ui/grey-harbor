using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
namespace GreyHarbor {
 public sealed partial class RaidVerification {
  IEnumerator VerifyMarket121(){
   foreach(var kind in MarketRules.Catalog(0)){int count=0;for(int c=0;c<7;c++)if(MarketCategories.Matches(kind,c,-1))count++;Check(count==1&&MarketCategories.Sub(kind)>=0&&MarketCategories.Sub(kind)<MarketCategories.Children[MarketCategories.Main(kind)].Length,"Market has exactly one valid category for "+kind);}
   Check(MarketCategories.Main(ItemKind.GunParts)==6&&MarketCategories.Main(ItemKind.Medkit)==1&&MarketCategories.Main(ItemKind.Medical)==6,"Crafting parts and medical materials remain collectibles");
   Check(MarketCategories.Goods(3,-1).Count==0&&MarketCategories.Goods(5,-1).Count==0&&MarketCategories.Goods(1,1).Count==2&&MarketCategories.Goods(1,0).Count==3,"Accessories and keys remain empty; supply categories contain two repair kits and three medicines");
   MarketFixture();var touch=new TouchInput();var prior=G.Controls;G.Controls=touch;int coins=G.Save.Coins;
   Check(G.BuyMarket(ItemKind.Cola,2)&&G.Save.Trades.Count==1&&G.Save.Trades[0].Buying&&G.Save.Trades[0].Coins==24&&G.Save.Trades[0].Items[0].Count==2,"Successful purchase creates exact transaction record");
   var ids=new HashSet<string>();foreach(var p in G.Save.Warehouse)if(p.Kind==ItemKind.Cola)ids.Add(p.Id);
   Check(G.SellWarehouse(ids)&&G.Save.Trades.Count==2&&!G.Save.Trades[0].Buying&&G.Save.Trades[0].Coins==20&&G.Save.Coins==coins-4,"Trading counter sells warehouse goods and records sale");
   Check(!G.SellWarehouse(ids)&&G.Save.Trades.Count==2,"Repeated sale cannot create duplicate credit or record");
   G.LoadSave();G.BuildCamp();Check(G.Save.Trades.Count==2&&G.Save.Trades[0].Items[0].Kind==ItemKind.Cola&&G.Save.Trades[1].Coins==24,"Transaction records survive save reload in newest first order");
   var old=new BaseSave{StarterGranted=true,Trades=null};CampRules.Normalize(old);Check(old.Trades!=null&&old.Trades.Count==0,"Old saves receive empty history without fabricated records");
   MarketFixture();string stock=JsonUtility.ToJson(G.Save);foreach(var k in MarketCategories.Goods(6,0)){}Check(JsonUtility.ToJson(G.Save)==stock,"Category browsing does not mutate stored positions or goods");
   G.Save.Coins=0;Check(!G.BuyMarket(ItemKind.Cola,1)&&G.Save.Trades.Count==0,"Rejected purchase leaves history empty");
   MarketFixture();string path=G.SavePath;File.WriteAllText(Path.Combine(folder,"market121-save-blocker"),"block");G.SavePath=Path.Combine(folder,"market121-save-blocker","save.json");stock=JsonUtility.ToJson(G.Save);
   Check(!G.BuyMarket(ItemKind.Cola,1)&&JsonUtility.ToJson(G.Save)==stock,"Failed purchase save rolls back transaction history");
   var first=G.Save.Warehouse[0];Check(!G.SellWarehouse(new HashSet<string>{first.Id})&&JsonUtility.ToJson(G.Save)==stock,"Failed sale save rolls back coins goods and history");G.SavePath=path;
   G.Player.transform.position=RaidMap.CampSpawn;Check(!G.SellWarehouse(new HashSet<string>{G.Save.Warehouse[0].Id}),"Market sale rejects remote interaction");
   MarketFixture();G.Save.Warehouse.Clear();CampRules.Deposit(G.Save,ItemKind.Ammo,360);CampRules.Deposit(G.Save,ItemKind.Cola,2,true);ids.Clear();foreach(var p in G.Save.Warehouse)ids.Add(p.Id);
   Check(G.SellWarehouse(ids)&&G.Save.Trades[0].Coins==740&&G.Save.Trades[0].Items.Count==2&&G.Save.Trades[0].Items[0].Count==360,"Bulk sale aggregates equal goods in one complete record");
   for(int i=0;i<205;i++)TradeRecord.Append(G.Save,true,i,new[]{new ItemStack(ItemKind.Cola,1)});Check(G.Save.Trades.Count==200&&G.Save.Trades[0].Coins==204&&G.Save.Trades[199].Coins==5,"History retention keeps newest 200 entries");
   G.Controls=prior;G.Paused=true;yield return null;
  }
 }
}
