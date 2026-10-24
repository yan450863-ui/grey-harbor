using System;
using System.Collections.Generic;
namespace GreyHarbor {
 public static class ContainerBonus147 {
  public const double Chance=.001;
  // One independent bonus roll per low/high-rate container; normal quality draws never roll red.
  public static void Add(List<ItemStack> items,SupplyType type,ContainerTier requested,Random rng,CollectionCategory? category=null,Predicate<ItemKind> filter=null){
   var tier=LootBalance.TierFor(type,requested);if(tier!=ContainerTier.Common&&tier!=ContainerTier.Advanced)return;
   if(rng.NextDouble()>=Chance)return;
   var red=new List<ItemKind>();
   for(int i=0;i<RaidInventory.KindCount;i++){
    var k=(ItemKind)i;if(RaidInventory.Group(k)!=ItemGroup.Collection||RaidInventory.Grade(k)!=4)continue;
    if(category.HasValue&&RaidInventory.Category(k)!=category.Value)continue;
    if(filter!=null&&!filter(k))continue;
    if(!category.HasValue&&filter==null&&type!=SupplyType.Ammo&&ContainerLootRules.Weights[(int)type,(int)RaidInventory.Category(k)]<=0)continue;
    red.Add(k);
   }
   if(red.Count==0)throw new InvalidOperationException("容器额外红品池为空");
   items.Add(new ItemStack(red[rng.Next(red.Count)],1,ItemOrigin.Certified){Identified=false,RaidFound=true});
  }
 }
}
