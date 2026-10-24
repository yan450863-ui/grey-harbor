using System;
using System.Collections.Generic;
namespace GreyHarbor {
 public enum ContainerTier { Auto=-1,Common,Advanced,Safe }
 public static class LootBalance {
  public static readonly int[] CountWeights={200,500,220,55,20,5};
  public static int Count(Random rng){int roll=rng.Next(1000);for(int i=0;i<6;i++){roll-=CountWeights[i];if(roll<0)return i+1;}return 6;}
  public static ContainerTier TierFor(SupplyType type,ContainerTier requested=ContainerTier.Auto){
   if(type==SupplyType.Safe)return ContainerTier.Safe;
   if(requested==ContainerTier.Safe)throw new ArgumentException("只有保险箱可以使用保险箱品质表");
   // Older placements may request Advanced; all non-safe containers now share Common.
   if(requested==ContainerTier.Auto||requested==ContainerTier.Common||requested==ContainerTier.Advanced)return ContainerTier.Common;
   throw new ArgumentOutOfRangeException(nameof(requested));
  }
  public static double[] QualityWeights(ContainerTier tier,double multiplier=1){
   if(tier<ContainerTier.Common||tier>ContainerTier.Safe)throw new ArgumentOutOfRangeException(nameof(tier));
   double[] basis=tier==ContainerTier.Safe?new double[]{5,30,40,23,2}:new double[]{60,32,7,1,0};
   multiplier=Math.Max(1,Math.Min(2,multiplier));double rare=basis[2]+basis[3]+basis[4],high=Math.Min(100,rare*multiplier),low=100-high;
   return new[]{low*basis[0]/(basis[0]+basis[1]),low*basis[1]/(basis[0]+basis[1]),high*basis[2]/rare,high*basis[3]/rare,high*basis[4]/rare};
  }
  public static ItemKind Pick(SupplyType type,Random rng,bool hard,ContainerTier tier=ContainerTier.Auto,CollectionCategory? category=null){
   var pool=new List<ItemKind>();for(int i=0;i<RaidInventory.KindCount;i++){var k=(ItemKind)i;if(Eligible(type,k)&&(!category.HasValue||(RaidInventory.Group(k)==ItemGroup.Collection&&RaidInventory.Category(k)==category.Value)))pool.Add(k);}
   if(pool.Count==0)throw new InvalidOperationException("容器类别没有有效候选");
   var weights=QualityWeights(TierFor(type,tier),hard?1.5:1);bool[] present=new bool[5];foreach(var k in pool)present[RaidInventory.Grade(k)]=true;double total=0;for(int i=0;i<5;i++)if(present[i])total+=weights[i];double roll=rng.NextDouble()*total;int grade=0;for(;grade<5;grade++)if(present[grade]){roll-=weights[grade];if(roll<0)break;}
   double sum=0;foreach(var k in pool)if(RaidInventory.Grade(k)==grade)sum+=ItemWeight(type,k);double choice=rng.NextDouble()*sum;
   foreach(var k in pool)if(RaidInventory.Grade(k)==grade){choice-=ItemWeight(type,k);if(choice<0)return k;}throw new InvalidOperationException("容器没有有效掉落候选");
  }
  public static bool Eligible(SupplyType type,ItemKind k){
   if(RetiredMedicine321.IsRetired(k)||k==ItemKind.GreenOptic30||KeyRules2760.IsKey(k)||KeyCase314.IsCase(k))return false;
   if(RaidInventory.Grade(k)==4&&(type!=SupplyType.Safe||RaidInventory.Group(k)!=ItemGroup.Collection))return false;
   if(type==SupplyType.Ammo)return AmmoRules270.IsAmmo(k);
   if(RaidInventory.Group(k)==ItemGroup.Collection)return ContainerLootRules.Weights[(int)type,(int)RaidInventory.Category(k)]>0;
   if(type==SupplyType.Medical)return ConsumableRules.IsRepair(k);
   if(type==SupplyType.Military)return PackLayouts338.IsNew(k)||ModRules30.IsMod(k)||WeaponRules279.IsWeapon(k)||AmmoRules270.IsAmmo(k)||ConsumableRules.IsRepair(k)||Array.IndexOf(RaidInventory.AvailableGear,k)>=0;
   return false;
  }
  static double ItemWeight(SupplyType type,ItemKind k)=>RaidInventory.Group(k)==ItemGroup.Collection?ContainerLootRules.Weights[(int)type,(int)RaidInventory.Category(k)]:type==SupplyType.Medical?(k==ItemKind.GreenArmorKit?80:20):12*WeaponRules279.DropWeight(k);
 }
}
