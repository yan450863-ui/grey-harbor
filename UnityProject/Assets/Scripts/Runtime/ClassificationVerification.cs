using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
namespace GreyHarbor {
 public sealed partial class RaidVerification {
  void VerifyClassification18(){
   var covered=new HashSet<CollectionCategory>();bool priced=true,combat=true;int[] lo={7,21,70,210,1400},hi={13,39,130,390,2600};
   int[] prior={2,40,25,100,35,15,50,15,45,180,600,104,25,45,100,120,260,80,160,320,90,180,360,240,417,30,110,2000,40,20,80,25};
   foreach(ItemKind k in Enum.GetValues(typeof(ItemKind))){if(RaidInventory.Group(k)==ItemGroup.Collection){covered.Add(RaidInventory.Category(k));int g=RaidInventory.Grade(k),v=RaidInventory.Value(k);priced&=v>=lo[g]&&v<=hi[g]&&!string.IsNullOrEmpty(RaidInventory.Use(k));}else combat&=RaidInventory.Value(k)==((int)k<prior.Length?prior[(int)k]:40)&&RaidInventory.Category(k)==CollectionCategory.None;}
   Check(covered.Count==8&&!covered.Contains(CollectionCategory.None),"All eight collection categories have actual items");
   Check(priced&&RaidInventory.Value(ItemKind.Cola)==10,"Collection fixed prices obey rarity bands and cola costs ten");
   Check(combat,"Combat prices follow confirmed blue baseline independently of collection categories");
   Check(RaidInventory.Category(ItemKind.Medical)==CollectionCategory.MedicalMaterials&&RaidInventory.Group(ItemKind.Medkit)==ItemGroup.Combat,"Medical crafting materials are separate from combat medkits");
   Check((int)ItemKind.BlueArmor==24&&(int)ItemKind.HarborCoin==25&&(int)ItemKind.BronzeStatuette==26,"New item IDs append without renumbering old saves");
   foreach(var k in new[]{ItemKind.HarborCoin,ItemKind.BronzeStatuette}){var tex=Resources.Load<Texture2D>("ItemArt/"+k);Check(tex!=null&&ItemIcons.Bounds(k).width>0,"Imported collectible artwork exists: "+k);}
   var old=new BaseSave{Version=6,GridInitialized=true,GearGranted=true,StarterGranted=true,PackGear=new ItemStack(ItemKind.StandardPack,1)};
   old.CampBag.Add(new ItemStack(ItemKind.Armor,1,ItemOrigin.Certified){Cell=0});old.CampBag.Add(new ItemStack(ItemKind.BlueArmor,1,ItemOrigin.Certified){Cell=1});old.CampBag.Add(new ItemStack(ItemKind.Helmet,1){Cell=2});
   CampRules.Normalize(old);var bag=RaidInventory.Container(old.PackGear,false);bag.Restore(old.CampBag);
   Check(old.Version==7&&CampRules.Available(old,ItemKind.Armor)==1&&CampRules.Available(old,ItemKind.BlueArmor)==1&&CampRules.Available(old,ItemKind.Helmet)==0&&old.Coins==120&&old.Certified[(int)ItemKind.BlueArmor]==1,"v6 overlapping single-cell gear migrates with certified overflow in warehouse");
   Check(bag.W(ItemKind.Armor)==3&&bag.H(ItemKind.Helmet)==2&&bag.Slots.TrueForAll(p=>bag.Fits(p.Kind,p.Cell,p)),"Migrated carry gear does not overlap");
   string json=JsonUtility.ToJson(old);CampRules.Normalize(old);Check(json==JsonUtility.ToJson(old),"Migrating twice does not duplicate or move inventory");
   bool coin=false,statue=false;var rng=new System.Random(2818);for(int i=0;i<1500;i++){coin|=HarborContent.Contents(SupplyType.Misc,rng).Exists(p=>p.Kind==ItemKind.HarborCoin);statue|=HarborContent.Contents(SupplyType.Safe,rng).Exists(p=>p.Kind==ItemKind.BronzeStatuette);}
   Check(coin&&statue,"Both new collectibles can drop through normal containers");
   FreshCamp();CampRules.Deposit(G.Save,ItemKind.HarborCoin,20,true);int cell=FindCell(ItemArea.Warehouse,ItemKind.HarborCoin);Check(G.QuickTransfer(ItemArea.Warehouse,cell)&&G.Bag.Count(ItemKind.HarborCoin)==20&&G.Bag.Slots.TrueForAll(p=>p.Count==1),"New collectible stack splits into twenty carried singles");
   Check(!G.CanUse(ItemArea.Backpack,G.Bag.At(0)),"Collectibles cannot be consumed as medicine");
   G.QuickTransfer(ItemArea.Backpack,0);var s=G.Save.Warehouse.Find(p=>p.Kind==ItemKind.HarborCoin);int coins=G.Save.Coins;Check(G.SellWarehouse(new HashSet<string>{s.Id})&&G.Save.Coins==coins+30,"Collectible sells at fixed unit price");
   var save=JsonUtility.FromJson<BaseSave>(JsonUtility.ToJson(G.Save));CampRules.Normalize(save);Check(CampRules.Available(save,ItemKind.HarborCoin)==19,"New IDs and quantities survive save reload");
  }
 }
}
