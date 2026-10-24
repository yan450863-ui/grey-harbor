using System.Collections.Generic;
using UnityEngine;
namespace GreyHarbor {
 public sealed class AnnexLootRecord2718 {
  public string RoomName;public LootContainer Container;public Vector3 AccessPoint;public bool BenchMounted,WallMounted;
 }
 public sealed partial class RaidMap {
  public readonly List<AnnexLootRecord2718> AnnexLootRecords2718=new List<AnnexLootRecord2718>();
  void BuildRepairAnnexContainers2718(){
   var rng=new System.Random((RaidGame.Instance?RaidGame.Instance.RaidSeed:2718)^2002718);
   // The marked central repair bay, shutter approach and both existing patrols remain free.
   AnnexLoot2718("大维修大厅","工作台工具箱",ContainerLook.Toolbox,new Vector3(-8.00f,.895f,-1.1f),270,new Vector3(-6.90f,0,-1.1f),new Vector3(-5.9f,0,-1.1f),rng,true);
   AnnexLoot2718("大维修大厅","维修记录文件柜",ContainerLook.FileCabinet,new Vector3(-8.32f,0,-3.95f),270,new Vector3(-6.95f,0,-3.95f),new Vector3(-5.9f,0,-3.95f),rng);
   AnnexLoot2718("大维修大厅","检修电气柜",ContainerLook.ElectricalCabinet,new Vector3(-8.32f,0,-8.05f),270,new Vector3(-6.95f,0,-8.05f),new Vector3(-5.9f,0,-8.05f),rng);
   AnnexLoot2718("大维修大厅","待用备件木箱",ContainerLook.CargoCrate,new Vector3(2.90f,0,8.55f),0,new Vector3(2.90f,0,7.2f),new Vector3(2.9f,0,6.5f),rng);
   AnnexLoot2718("工具与备件间","货架旁备件工具箱",ContainerLook.Toolbox,new Vector3(5.05f,0,8.60f),0,new Vector3(5.10f,0,7.35f),new Vector3(6.25f,0,7.35f),rng);
   AnnexLoot2718("工具与备件间","备用检修工具箱",ContainerLook.Toolbox,new Vector3(8.10f,0,3.50f),90,new Vector3(7.15f,0,3.50f),new Vector3(7.15f,0,4.55f),rng);
   AnnexLoot2718("工人休息室","工人随身行李",ContainerLook.Suitcase,new Vector3(8.25f,0,-.10f),90,new Vector3(6.95f,0,-.10f),new Vector3(6.25f,0,0),rng);
   AnnexLoot2718("更衣洗漱区","更衣区工作外套",ContainerLook.Coat,new Vector3(8.72f,0,-6.15f),90,new Vector3(7.35f,0,-6.15f),new Vector3(6.25f,0,-6.1f),rng,false,true);
   Physics.SyncTransforms();
  }
  void AnnexLoot2718(string room,string name,ContainerLook look,Vector3 model,float yaw,Vector3 stand,Vector3 access,System.Random rng,bool bench=false,bool wall=false){
   var loot=AddContainer(stand,model,look,SupplyType.Misc,rng,HarborDistrict.Repair,yaw,false,ContainerTier.Common,DropRegion230.Repair);loot.Name=name;
   if(!bench&&!wall)FreightPlaceOnFloor2717(loot.View);
   AnnexLootRecords2718.Add(new AnnexLootRecord2718{RoomName=room,Container=loot,AccessPoint=access,BenchMounted=bench,WallMounted=wall});
  }
 }
}
