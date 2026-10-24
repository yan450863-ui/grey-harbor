using System.Collections.Generic;
using UnityEngine;
namespace GreyHarbor {
 public sealed partial class RaidMap {
  public readonly List<FreightLootRecord2717> OfficeSearchRecords2736=new List<FreightLootRecord2717>();
  void BuildOfficeFirstContainers2736(){
   var rng=new System.Random((RaidGame.Instance?RaidGame.Instance.RaidSeed:2736)^202736);
   OfficeLoot2736("01","保安交接记录柜",ContainerLook.FileCabinet,-3,-22,90,rng);
   OfficeLoot2736("01","值班员个人物品柜",ContainerLook.SupplyLocker,-3,-18,90,rng);
   OfficeLoot2736("02","访客登记文件柜",ContainerLook.FileCabinet,-19.2f,-14,270,rng);
   OfficeLoot2736("02","待领访客手提箱",ContainerLook.Suitcase,-19.2f,-7.2f,270,rng);
   OfficeLoot2736("02","前台备用文具抽屉",ContainerLook.BedsideDrawer,-15,-6.7f,0,rng);
   foreach(float z in new[]{-22f,-18f,-14f,-10f})OfficeLoot2736("03","货运业务档案柜"+(-z),ContainerLook.FileCabinet,19.2f,z,90,rng);
   OfficeLoot2736("03","业务员公文箱",ContainerLook.Suitcase,15,-23.2f,180,rng);
   OfficeLoot2736("04","提单文件柜",ContainerLook.FileCabinet,-19,10.2f,0,rng);
   OfficeLoot2736("04","签收单文件柜",ContainerLook.FileCabinet,-11,10.2f,0,rng);
   OfficeLoot2736("04","单证室文具抽屉",ContainerLook.BedsideDrawer,-6.7f,4,90,rng);
   OfficeLoot2736("05","男卫应急药柜",ContainerLook.MedicalCabinet,-2.2f,-1.8f,90,rng,true);
   OfficeLoot2736("06","女卫应急药柜",ContainerLook.MedicalCabinet,-2.2f,5.2f,90,rng,true);
   OfficeLoot2736("07","会议资料文件柜",ContainerLook.FileCabinet,19.2f,-1,90,rng);
   OfficeLoot2736("07","会议备用器材柜",ContainerLook.SupplyLocker,19.2f,4,90,rng);
   OfficeLoot2736("07","会议人员公文箱",ContainerLook.Suitcase,19.2f,9,90,rng);
   OfficeLoot2736("08","旧提单档案柜",ContainerLook.FileCabinet,-18,14.7f,180,rng);
   OfficeLoot2736("08","封存合同档案柜",ContainerLook.FileCabinet,-14,14.7f,180,rng);
   OfficeLoot2736("08","档案交接文件箱",ContainerLook.ArchiveCase,-8.7f,15,90,rng);
   OfficeLoot2736("09","网络维修工具箱",ContainerLook.Toolbox,-2.8f,22.5f,90,rng);
   OfficeLoot2736("10","员工个人行李",ContainerLook.RollingSuitcase,11,23.2f,0,rng);
   OfficeLoot2736("10","茶水间储物柜",ContainerLook.KitchenCupboard,11,14.7f,180,rng);
   OfficeSmallSafe2737(rng);
  }
  void OfficeLoot2736(string roomId,string name,ContainerLook look,float x,float z,float yaw,System.Random rng,bool mounted=false){
   var model=new Vector3(x,0,z);var toward=Quaternion.Euler(0,yaw,0)*Vector3.back;
   var stand=model+toward*1.35f;var access=stand+toward*.60f;
   var room=OfficeRooms2726.Find(r=>r.Id==roomId);
   var loot=AddContainer(stand,model,look,SupplyType.Misc,rng,HarborDistrict.Office,yaw,false,ContainerTier.Common,DropRegion230.Office);loot.Name=name;
   if(!mounted)FreightPlaceOnFloor2717(loot.View);
   OfficeSearchRecords2736.Add(new FreightLootRecord2717{Container=loot,Model=loot.View,Stand=stand,AccessPoint=access,RoomName=room.Name,Yaw=yaw,WallMounted=mounted});
  }
 }
}
