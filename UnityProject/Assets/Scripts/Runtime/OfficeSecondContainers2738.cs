using UnityEngine;
namespace GreyHarbor {public sealed partial class RaidMap {
 void BuildOfficeSecondContainers2738(){
  var rng=new System.Random((RaidGame.Instance?RaidGame.Instance.RaidSeed:2738)^202738);
  OfficeLoot2736("01","技术员备件储物柜",ContainerLook.SupplyLocker,-3,-22,90,rng);
  OfficeLoot2736("01","技术维修工具箱",ContainerLook.Toolbox,-3,-18,90,rng);
  OfficeLoot2736("02","财务票据文件柜",ContainerLook.FileCabinet,-19.2f,-14,270,rng);
  OfficeLoot2736("02","人事合同文件柜",ContainerLook.FileCabinet,-19.2f,-7.2f,270,rng);
  OfficeLoot2736("02","财务备用文具抽屉",ContainerLook.BedsideDrawer,-3,-14,90,rng);
  OfficeLoot2736("02","员工人事资料箱",ContainerLook.ArchiveCase,-3,-10.5f,90,rng);
  foreach(float z in new[]{-22f,-18f,-14f,-10f})OfficeLoot2736("03","开放办公个人文件柜"+(-z),ContainerLook.FileCabinet,19.2f,z,90,rng);
  OfficeLoot2736("03","办公人员公文箱",ContainerLook.Suitcase,3,-11,270,rng);
  OfficeLoot2736("04","会议资料文件柜",ContainerLook.FileCabinet,-11,10.2f,0,rng);
  OfficeLoot2736("04","会议备用文件箱",ContainerLook.ArchiveCase,-15,-2.3f,180,rng);
  OfficeLoot2736("04","会议人员公文箱",ContainerLook.Suitcase,-19.2f,3,270,rng);
  OfficeLoot2736("05","男卫应急药柜",ContainerLook.MedicalCabinet,-2.2f,-1.8f,90,rng,true);
  OfficeLoot2736("06","女卫应急药柜",ContainerLook.MedicalCabinet,-2.2f,5.2f,90,rng,true);
  OfficeLoot2736("07","项目合同文件柜",ContainerLook.FileCabinet,19.2f,-1,90,rng);
  OfficeLoot2736("07","项目图纸文件箱",ContainerLook.ArchiveCase,19.2f,8.5f,90,rng);
  OfficeLoot2736("07","项目人员公文箱",ContainerLook.Suitcase,4,10,0,rng);
  OfficeLoot2736("08","人事旧档案柜",ContainerLook.FileCabinet,-18,14.7f,180,rng);
  OfficeLoot2736("08","项目封存档案柜",ContainerLook.FileCabinet,-14,14.7f,180,rng);
  OfficeLoot2736("08","档案交接文件箱",ContainerLook.ArchiveCase,-8.7f,15,90,rng);
  OfficeLoot2736("09","网络备件工具箱",ContainerLook.Toolbox,-2.8f,22.5f,90,rng);
  OfficeLoot2736("10","主管私人文件柜",ContainerLook.FileCabinet,13.2f,16.8f,90,rng);
  var model=new Vector3(-7,0,-6.6f);var stand=model+Vector3.back*1.35f;
  var safe=AddContainer(stand,model,ContainerLook.Safe,SupplyType.Safe,rng,HarborDistrict.Office,0,false,ContainerTier.Safe,DropRegion230.Office,SafeSize240.Small);
  safe.Name="财务与人事小保险箱";FreightPlaceOnFloor2717(safe.View);
  OfficeSearchRecords2736.Add(new FreightLootRecord2717{Container=safe,Model=safe.View,Stand=stand,AccessPoint=stand+Vector3.back*.6f,RoomName="财务与人事",Yaw=0});
 }
}}
