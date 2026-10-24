using UnityEngine;
namespace GreyHarbor { public sealed partial class RaidMap {
 void BuildOfficeThirdContainers2740(){
  var rng=new System.Random((RaidGame.Instance?RaidGame.Instance.RaidSeed:2740)^202740);
  OfficeLoot2736("01","茶水间储物柜",ContainerLook.KitchenCupboard,-3,-23,90,rng);
  OfficeLoot2736("01","员工茶水间行李",ContainerLook.RollingSuitcase,-3,-17.5f,90,rng);
  OfficeLoot2736("02","管理访客登记柜",ContainerLook.FileCabinet,-19.2f,-14,270,rng);
  OfficeLoot2736("02","接待室访客公文箱",ContainerLook.Suitcase,-19.2f,-7.2f,270,rng);
  OfficeLoot2736("03","董事会议合同柜",ContainerLook.FileCabinet,19.2f,-22,90,rng);
  OfficeLoot2736("03","董事会议备用器材柜",ContainerLook.SupplyLocker,19.2f,-10,90,rng);
  OfficeLoot2736("03","董事会议资料箱",ContainerLook.ArchiveCase,3,-10,270,rng);
  OfficeLoot2736("04","经理重要文件柜",ContainerLook.FileCabinet,-8.8f,-1,90,rng);
  OfficeLoot2736("04","经理私人公文箱",ContainerLook.Suitcase,-8.8f,9,90,rng);
  OfficeLoot2736("05","三楼卫生间应急药柜",ContainerLook.MedicalCabinet,-2.2f,-1.8f,90,rng,true);
  OfficeLoot2736("06","行政人事文件柜",ContainerLook.FileCabinet,-2.8f,5,90,rng);
  OfficeLoot2736("06","行政封存合同柜",ContainerLook.FileCabinet,-2.8f,8,90,rng);
  OfficeLoot2736("06","行政交接文件箱",ContainerLook.ArchiveCase,-2.8f,9.3f,90,rng);
  OfficeLoot2736("07","管理项目文件柜",ContainerLook.FileCabinet,19.2f,-1,90,rng);
  OfficeLoot2736("07","管理合同资料箱",ContainerLook.ArchiveCase,19.2f,8.5f,90,rng);
  OfficeLoot2736("07","管理职员公文箱",ContainerLook.Suitcase,4,10,0,rng);
  OfficeLoot2736("08","重要资料封存柜",ContainerLook.FileCabinet,-18,14.7f,180,rng);
  OfficeLoot2736("08","重要资料交接箱",ContainerLook.ArchiveCase,-11,14.7f,180,rng);
  OfficeLoot2736("09","通信设备备件工具箱",ContainerLook.Toolbox,-2.8f,22.5f,90,rng);
  OfficeLoot2736("09","通信检修记录柜",ContainerLook.FileCabinet,-2.8f,16.5f,90,rng);
  OfficeThirdSafe2740("04","总经理小保险箱",new Vector3(-19.2f,0,6),270,SafeSize240.Small,rng);
  OfficeThirdSafe2740("08","重要资料库大保险箱",new Vector3(-14,0,23),0,SafeSize240.Large,rng);
 }
 void OfficeThirdSafe2740(string id,string name,Vector3 model,float yaw,SafeSize240 size,System.Random rng){
  var toward=Quaternion.Euler(0,yaw,0)*Vector3.back;var stand=model+toward*1.35f;
  var safe=AddContainer(stand,model,ContainerLook.Safe,SupplyType.Safe,rng,HarborDistrict.Office,yaw,false,ContainerTier.Safe,DropRegion230.Office,size);
  safe.Name=name;FreightPlaceOnFloor2717(safe.View);
  OfficeSearchRecords2736.Add(new FreightLootRecord2717{Container=safe,Model=safe.View,Stand=stand,AccessPoint=stand+toward*.6f,RoomName=OfficeRooms2726.Find(r=>r.Id==id).Name,Yaw=yaw});
 }
}}
