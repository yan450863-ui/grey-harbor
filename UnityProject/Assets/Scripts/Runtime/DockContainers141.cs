using UnityEngine;
using System;
using System.Collections.Generic;
namespace GreyHarbor {
 public static class DockContainers141 {
  public static bool IsDock(ContainerLook look)=>(int)look>=(int)ContainerLook.MarineCooler&&(int)look<=(int)ContainerLook.DocumentTube;
  public static CollectionCategory? Category(ContainerLook look){switch(look){case ContainerLook.FishingChest:case ContainerLook.WeldingToolbox:return CollectionCategory.ToolsParts;case ContainerLook.MarineControlBox:return CollectionCategory.Electronics;case ContainerLook.DocumentTube:return CollectionCategory.Information;default:return null;}}
  public static SupplyType Supply(ContainerLook look)=>look==ContainerLook.RescueCabinet?SupplyType.Medical:look==ContainerLook.FishingChest||look==ContainerLook.WeldingToolbox?SupplyType.Tools:SupplyType.Misc;
  public static bool CoolerEligible(ItemKind k)=>RaidInventory.Group(k)==ItemGroup.Collection&&RaidInventory.Grade(k)<4&&(RaidInventory.Category(k)==CollectionCategory.Living||RaidInventory.Category(k)==CollectionCategory.Biological);
  public static List<ItemStack> Contents(ContainerLook look,System.Random rng,bool hard,ContainerTier tier){
   if(look!=ContainerLook.MarineCooler)return HarborContent.Contents(Supply(look),rng,hard,tier,Category(look));
   var pool=new List<ItemKind>();for(int i=0;i<RaidInventory.KindCount;i++)if(CoolerEligible((ItemKind)i))pool.Add((ItemKind)i);
   var weights=LootBalance.QualityWeights(tier,hard?1.5:1);bool[] present=new bool[5];foreach(var k in pool)present[RaidInventory.Grade(k)]=true;double total=0;for(int i=0;i<5;i++)if(present[i])total+=weights[i];
   var items=new List<ItemStack>();int count=LootBalance.Count(rng);for(int n=0;n<count;n++){double roll=rng.NextDouble()*total;int grade=0;for(;grade<5;grade++)if(present[grade]){roll-=weights[grade];if(roll<0)break;}var valid=pool.FindAll(k=>RaidInventory.Grade(k)==grade);items.Add(new ItemStack(valid[rng.Next(valid.Count)],1,ItemOrigin.Certified){Identified=false,RaidFound=true});}ContainerBonus147.Add(items,SupplyType.Misc,tier,rng,null,k=>RaidInventory.Category(k)==CollectionCategory.Living||RaidInventory.Category(k)==CollectionCategory.Biological);return items;
  }
 }
 public sealed partial class RaidMap {
  public readonly List<LootContainer> DockLoot141=new List<LootContainer>();
  void DockLootPoint141(float sx,float sz,float x,float z,float height,ContainerLook look,float yaw,System.Random rng,ContainerTier tier=ContainerTier.Common){DockLoot141.Add(AddContainer(PortPoint(sx,sz),PortPoint(x,z)+Vector3.up*height,look,SupplyType.Misc,rng,HarborDistrict.Coast,yaw,false,tier));}
  void BuildDockLoot141(int seed){var rng=new System.Random(seed^141071);
   DockLootPoint141(151,131.5f,149.8f,131.5f,0,ContainerLook.RescueCabinet,-90,rng);
   DockLootPoint141(181,132.9f,181,134,.10f,ContainerLook.MarineCooler,0,rng);
   DockLootPoint141(154,89.7f,154,90.9f,.90f,ContainerLook.WeldingToolbox,0,rng);
   DockLootPoint141(168,93.8f,168,95,.10f,ContainerLook.FishingChest,0,rng);
   DockLootPoint141(153,35,151.8f,35,0,ContainerLook.MarineControlBox,-90,rng);
   DockLootPoint141(172.5f,37.8f,172.5f,39,.12f,ContainerLook.CargoCrate,0,rng);
   DockLootPoint141(179.85f,49.2f,178.7f,49.2f,.10f,ContainerLook.WaterproofTransitCase,-90,rng);
   DockLootPoint141(180.2f,51.8f,181.35f,51.8f,.82f,ContainerLook.DocumentTube,90,rng,ContainerTier.Common);
  }
 }
}
