using UnityEngine;
namespace GreyHarbor {
 public static class OutdoorContainers140 {
  public static bool IsOutdoor(ContainerLook look)=>(int)look>=(int)ContainerLook.OutdoorTote&&(int)look<=(int)ContainerLook.FuelDrum;
  public static CollectionCategory? Category(ContainerLook look){
   switch(look){case ContainerLook.OutdoorTote:return CollectionCategory.Living;case ContainerLook.GardenChest:return CollectionCategory.ToolsParts;case ContainerLook.ElectricalCabinet:return CollectionCategory.Electronics;case ContainerLook.FuelDrum:return CollectionCategory.EnergyChemicals;default:return null;}
  }
  public static SupplyType Supply(ContainerLook look)=>look==ContainerLook.GardenChest||look==ContainerLook.FuelDrum?SupplyType.Tools:SupplyType.Misc;
 }
 public sealed partial class RaidMap {
  void BuildYardOutdoorContent140(int seed){
   var rng=new System.Random(seed^14001);
   Box("OutdoorToteRaisedBase",PortPoint(40.8f,44.15f)+Vector3.up*.06f,new Vector3(1.25f,.12f,.95f),"857A5F");
   YardOutdoor140(40.8f,43,40.8f,44.15f,.12f,ContainerLook.OutdoorTote,0,rng);
   YardOutdoor140(23.7f,22,22.5f,22,0,ContainerLook.GardenChest,-90,rng);
   YardOutdoor140(47.8f,30.4f,49,30.4f,0,ContainerLook.ElectricalCabinet,90,rng);
  }
  void YardOutdoor140(float sx,float sz,float x,float z,float height,ContainerLook look,float yaw,System.Random rng){
   AddContainer(PortPoint(sx,sz),PortPoint(x,z)+Vector3.up*height,look,OutdoorContainers140.Supply(look),rng,HarborDistrict.Entrance,yaw,false,ContainerTier.Common);
  }
 }
}
