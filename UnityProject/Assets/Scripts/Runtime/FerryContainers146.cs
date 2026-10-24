using UnityEngine;
using System.Collections.Generic;
namespace GreyHarbor {
 public sealed partial class RaidMap {
  public readonly List<LootContainer> FerryLoot146=new List<LootContainer>();
  void FerryContainer146(System.Random rng,string room,ContainerLook look,SupplyType type,float sx,float sz,float x,float y,float z,float yaw=0,ContainerTier tier=ContainerTier.Common){
   var loot=AddContainer(new Vector3(sx,0,sz),new Vector3(x,y,z),look,type,rng,HarborDistrict.Coast,yaw,false,tier);
   loot.Name=room+" · "+loot.Name;FerryLoot146.Add(loot);
  }
  void PopulateFerryContainers146(){
   // Furniture/table supported cases and wall cabinets stay out of aisles, doors and stairs.
   var rng=new System.Random(System.Environment.TickCount);
   if(InteriorId=="ferry1"){
    FerryContainer146(rng,"行李舱",ContainerLook.RollingSuitcase,SupplyType.Misc,-3.2f,-13.1f,-2.5f,0,-14.1f);
    FerryFurniture144.Add(new FerryLivingKit144(this).Table(new Vector3(-6.1f,0,-10.7f)));
    FerryContainer146(rng,"行李舱",ContainerLook.Suitcase,SupplyType.Misc,-4.9f,-10.7f,-6.1f,.81f,-10.7f,90);
    FerryContainer146(rng,"备品储藏室",ContainerLook.Toolbox,SupplyType.Tools,-3.5f,-2.8f,-4.8f,.81f,-2.8f,90);
    FerryContainer146(rng,"卫生间",ContainerLook.MedicalCabinet,SupplyType.Medical,5.65f,-8.95f,7.0f,0,-8.95f,90);
    FerryContainer146(rng,"服务间",ContainerLook.StorageBin,SupplyType.Misc,4.1f,-5.9f,5.15f,0,-7.25f);
    FerryContainer146(rng,"一楼客舱",ContainerLook.Suitcase,SupplyType.Misc,-3.8f,11.6f,-5.2f,.81f,11.6f,90);
    FerryContainer146(rng,"一楼客舱",ContainerLook.Coat,SupplyType.Misc,5.9f,11.2f,7,0,11.2f,90);
    FerryContainer146(rng,"备品储藏室",ContainerLook.Coat,SupplyType.Misc,-6.3f,-3.2f,-7,0,-3.4f,-90);
    FerryContainer146(rng,"一楼客舱",ContainerLook.RollingSuitcase,SupplyType.Misc,5.9f,5.1f,6.7f,0,5.1f,90);
   }else{
    FerryContainer146(rng,"驾驶室",ContainerLook.FileCabinet,SupplyType.Misc,5.7f,14,6.8f,0,14,90);
    FerryContainer146(rng,"二楼客舱",ContainerLook.RollingSuitcase,SupplyType.Misc,-1.8f,5.7f,-2.5f,0,5.7f,90);
    FerryContainer146(rng,"观景休息区",ContainerLook.Suitcase,SupplyType.Misc,2.75f,-11.1f,4,.81f,-11.1f,90);
    FerryContainer146(rng,"观景休息区",ContainerLook.Coat,SupplyType.Misc,5.65f,-7.9f,7,0,-7.9f,90);
    FerryContainer146(rng,"观景休息区西翼",ContainerLook.StorageBin,SupplyType.Misc,-5.5f,-2.9f,-6.7f,0,-2.9f,90);
    FerryContainer146(rng,"二楼客舱",ContainerLook.Coat,SupplyType.Misc,-5.7f,5.7f,-7,0,5.7f,-90);
    FerryFurniture144.Add(new FerryLivingKit144(this).Table(new Vector3(-5.9f,0,11.4f),0,false));
    FerryContainer146(rng,"驾驶室",ContainerLook.PrecisionCase,SupplyType.Misc,-4.65f,11.4f,-5.9f,.81f,11.4f,90,ContainerTier.Common);
    FerryContainer146(rng,"观景休息区",ContainerLook.JewelryBox,SupplyType.Misc,2.75f,-5,4,.81f,-5,90,ContainerTier.Common);
   }
  }
 }
}
