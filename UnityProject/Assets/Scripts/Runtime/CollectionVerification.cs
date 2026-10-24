using System;
using System.Collections.Generic;
using UnityEngine;
namespace GreyHarbor {public sealed partial class RaidVerification {
 void VerifyCollections126(){
  int[,] counts=new int[9,5];var names=new HashSet<string>();var red=new HashSet<ItemKind>();var all=new List<ItemKind>();
  foreach(ItemKind k in Enum.GetValues(typeof(ItemKind))){if(RaidInventory.Group(k)!=ItemGroup.Collection)continue;all.Add(k);counts[(int)RaidInventory.Category(k),RaidInventory.Grade(k)]++;Check(names.Add(RaidInventory.Name(k)),"126 Unique collection name "+k);if(RaidInventory.Grade(k)==4)red.Add(k);
   Check(MarketRules.Listed(k)&&MarketCategories.Main(k)==6&&MarketCategories.Sub(k)==(int)RaidInventory.Category(k)-1,"126 Collection market classification "+k);
   var tex=Resources.Load<Texture2D>("ItemArt/"+k);bool clear=false,solid=false;if(tex!=null)foreach(var px in tex.GetPixels32()){clear|=px.a==0;solid|=px.a>240;}Check(tex!=null&&clear&&solid,"126 Authored transparent art "+k);
   var carry=new RaidInventory(5,4);carry.Add(k,2);var wh=new RaidInventory(18,20,true);wh.Add(k,21);Check(carry.UsedCells==2&&carry.Slots.Count==2&&wh.Slots.Count==2&&wh.At(0).Count==20,"126 Collection single carry and warehouse twenty "+k);
   Check(LootBalance.Eligible(SupplyType.Safe,k),"126 Every collection can be found in a safe "+k);
  }
  Check(all.Count==72&&red.Count==8&&RaidInventory.KindCount==89,"126 72 collections and eight red treasures");for(int c=1;c<=8;c++)for(int q=0;q<5;q++)Check(counts[c,q]==(q==4?1:2),"126 Category-quality coverage "+c+" / "+q);
  foreach(var k in red)for(int t=0;t<5;t++)Check(!LootBalance.Eligible((SupplyType)t,k),"126 No red treasure in common containers "+k+" / "+t);
  var seen=new HashSet<ItemKind>();var rng=new System.Random(126);for(int i=0;i<100000;i++)seen.Add(LootBalance.Pick(SupplyType.Safe,rng,false));Check(all.TrueForAll(k=>seen.Contains(k)),"126 All 72 collections actually reachable through random loot");
  var old=new BaseSave{Version=7,Stored=new int[32],Certified=new int[32],StarterGranted=true,GridInitialized=true,GearGranted=true};old.Warehouse.Add(new ItemStack(ItemKind.Cola,7,ItemOrigin.Certified){Cell=0});CampRules.Normalize(old);Check(old.Stored.Length==89&&old.Stored[(int)ItemKind.Cola]==7&&old.Certified[(int)ItemKind.Cola]==7,"126 Old 32-type save expands without losing stock");
  foreach(var k in all)CampRules.Deposit(old,k,1);var reloaded=JsonUtility.FromJson<BaseSave>(JsonUtility.ToJson(old));CampRules.Normalize(reloaded);Check(all.TrueForAll(k=>CampRules.Available(reloaded,k)>0),"126 Every collection survives save round trip");
  MarketFixture();G.Save.Coins=100000;foreach(var k in red)Check(G.BuyMarket(k,1),"126 Buy red collection "+k);G.LoadSave();G.BuildCamp();Check(red.Count==G.Save.Warehouse.FindAll(p=>RaidInventory.Grade(p.Kind)==4).Count,"126 New red purchases persist");
  G.Panel=CampPanel.Warehouse;var ids=new HashSet<string>();int worth=0;foreach(var p in G.Save.Warehouse)if(RaidInventory.Grade(p.Kind)==4){ids.Add(p.Id);worth+=RaidInventory.Value(p.Kind)*p.Count;}int coins=G.Save.Coins;Check(G.SellWarehouse(ids)&&G.Save.Coins==coins+worth,"126 Sell red selection returns exact fixed prices");FreshCamp();
 }
}}
