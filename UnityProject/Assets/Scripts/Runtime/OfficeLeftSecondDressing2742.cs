using UnityEngine;
namespace GreyHarbor { public sealed partial class RaidMap {
 void BuildOfficeLeftSecondDressing2742(){
  // Dispatch consoles and service equipment leave the room centres and two bypasses open.
  OfficeProp2736("01",FutureProp2717.Kitchenette,-11,-23,180);
  OfficeProp2736("01",FutureProp2717.StaffFridge,-5,-23,180);
  OfficeProp2736("01",FutureProp2717.OfficeSofa,-11,-17,0);
  OfficeProp2736("01",FutureProp2717.CoffeeCorner,-5,-17,0);
  OfficeProp2736("01",FutureProp2717.RoundMeetingSet,-11,-20,0);
  OfficeProp2736("01",FutureProp2717.CoatStand,-13,-22,0);
  OfficeProp2736("01",FutureProp2717.OverflowWasteBin,-5,-21,0);
  OfficeWall2736("01",FutureProp2717.NoticeBoard,-10,1.4f,-23.82f,180);
  OfficeWall2736("01",FutureProp2717.WallClock,-6,2.2f,-23.82f,180);
  OfficeWall2736("01",FutureProp2717.WallAirConditioner,-13.82f,2.3f,-18,270);
  foreach(float x in new[]{-13f,-6f})OfficeProp2736("02",FutureProp2717.SingleWorkstation,x,-14.8f,180);
  OfficeProp2736("02",FutureProp2717.SecurityDesk,-11,-7,0);
  OfficeProp2736("02",FutureProp2717.LowDrawerCabinet,-15,-7,0);
  OfficeProp2736("02",FutureProp2717.RecordTrolley,-7,-10,90);
  OfficeProp2736("02",FutureProp2717.PortableFan,-13,-9,0);
  OfficeProp2736("02",FutureProp2717.PaperBoxStack,-14,-12,0);
  OfficeProp2736("02",FutureProp2717.OverflowWasteBin,-6,-7,0);
  OfficeWall2736("02",FutureProp2717.NoticeBoard,-19.82f,1.5f,-12,270);
  OfficeWall2736("02",FutureProp2717.WallClock,-10,2.2f,-6.18f,0);
  OfficeWall2736("02",FutureProp2717.WallAirConditioner,-15,2.4f,-6.18f,0);
  foreach(float x in new[]{7f,12f})foreach(float z in new[]{-21f,-8f})OfficeProp2736("03",FutureProp2717.DoubleWorkstation,x,z,z<-15?180:0);
  foreach(float z in new[]{-21f,-17f,-9f})OfficeProp2736("03",FutureProp2717.ControlPanel,3,z,270);
  OfficeProp2736("03",FutureProp2717.PlanMapBoard,10,-23.3f,180);
  OfficeProp2736("03",FutureProp2717.FourSeatBench,16,-8,0);
  OfficeProp2736("03",FutureProp2717.Copier,16,-12,90);
  OfficeProp2736("03",FutureProp2717.WaterDispenser,3,-7,0);
  OfficeProp2736("03",FutureProp2717.PaperShredder,15,-12,0);
  OfficeProp2736("03",FutureProp2717.RecordTrolley,7,-18,20);
  OfficeProp2736("03",FutureProp2717.FallenOfficeChair,16,-16,35);
  OfficeProp2736("03",FutureProp2717.PaperBoxStack,16,-20,0);
  OfficeProp2736("03",FutureProp2717.OfficePlant,4,-23,0);
  OfficeProp2736("03",FutureProp2717.OverflowWasteBin,5,-7,0);
  OfficeWall2736("03",FutureProp2717.Whiteboard,11,1.4f,-6.18f,0);
  OfficeWall2736("03",FutureProp2717.WallClock,16,2.2f,-6.18f,0);
  OfficeWall2736("03",FutureProp2717.WallAirConditioner,19.82f,2.3f,-15,90);
  OfficeProp2736("04",FutureProp2717.DoubleWorkstation,-13,1.8f,180);OfficeProp2736("04",FutureProp2717.DoubleWorkstation,-6,-1.2f,180);
  foreach(float z in new[]{0f,3f,7f})OfficeProp2736("04",FutureProp2717.ServerRack,-3,z,90);
  foreach(float x in new[]{-18f,-16f,-8f,-5f})OfficeProp2736("04",FutureProp2717.ControlPanel,x,10.4f,0);
  OfficeProp2736("04",FutureProp2717.RecordTrolley,-8,6,0);
  OfficeProp2736("04",FutureProp2717.BrokenMonitor,-7,8,20);
  OfficeProp2736("04",FutureProp2717.PortableFan,-5,4,90);
  OfficeProp2736("04",FutureProp2717.OverflowWasteBin,-5,-1.5f,0);
  OfficeWall2736("04",FutureProp2717.NoticeBoard,-19.82f,1.5f,6,270);
  OfficeWall2736("04",FutureProp2717.WallClock,-8,2.2f,-2.82f,180);
  OfficeWall2736("04",FutureProp2717.WallAirConditioner,-19.82f,2.4f,1,270);
  foreach(float z in new[]{0f,8f}){
   OfficeProp2736("05",FutureProp2717.MeetingTable,10,z,0);
   foreach(float x in new[]{8.8f,10f,11.2f}){OfficeProp2736("05",FutureProp2717.VisitorChair,x,z-1.2f,0);OfficeProp2736("05",FutureProp2717.VisitorChair,x,z+1.2f,180);}
  }
  OfficeProp2736("05",FutureProp2717.ProjectionScreen,17,1,90);
  OfficeProp2736("05",FutureProp2717.ProjectionCart,16,7,0);
  OfficeProp2736("05",FutureProp2717.ConferenceLectern,5,-1.5f,180);
  OfficeProp2736("05",FutureProp2717.FoldedTableCart,4,8,270);
  OfficeProp2736("05",FutureProp2717.WaterDispenser,3,10,0);
  OfficeProp2736("05",FutureProp2717.OfficePlant,3,-2,0);
  OfficeWall2736("05",FutureProp2717.Whiteboard,11,1.4f,10.82f,0);
  OfficeWall2736("05",FutureProp2717.WallClock,17,2.2f,-2.82f,180);
  foreach(float x in new[]{-15f,-10f})OfficeProp2736("06",FutureProp2717.SingleWorkstation,x,23,0);
  foreach(float z in new[]{16f,20f,23f})OfficeProp2736("06",FutureProp2717.ServerRack,-8.7f,z,90);
  OfficeProp2736("06",FutureProp2717.TunnelWorkBench,-17,14.8f,180);
  OfficeProp2736("06",FutureProp2717.ToolTrolley,-10,16,90);
  OfficeProp2736("06",FutureProp2717.PortableFan,-16,21,0);
  OfficeProp2736("06",FutureProp2717.PaperBoxStack,-11,21,0);
  OfficeWall2736("06",FutureProp2717.NoticeBoard,-15,1.4f,23.82f,0);
  OfficeWall2736("06",FutureProp2717.WallClock,-10,2.2f,23.82f,0);
  OfficeWall2736("06",FutureProp2717.WallAirConditioner,-19.82f,2.3f,19,270);
  var living=new FerryLivingKit144(this);var repair=new RepairInteriorKit200(this);
  OfficeTrack2736("07","Toilet",living.Toilet(new Vector3(-7,0,23),0).gameObject);
  OfficeTrack2736("07","Toilet",living.Toilet(new Vector3(-3,0,23),0).gameObject);
  OfficeTrack2736("07","WashStation",repair.WashStation(new Vector3(-7.45f,0,19),270).gameObject);
  OfficeProp2736("07",FutureProp2717.MopBucket,-3,21,0);
  OfficeProp2736("07",FutureProp2717.OverflowWasteBin,-6.5f,21,0);
  OfficeWall2736("07",FutureProp2717.WallClock,-5,2.3f,23.82f,0);
  OfficeProp2736("08",FutureProp2717.ExecutiveDesk,7,15.6f,180);
  OfficeProp2736("08",FutureProp2717.OfficeChair,7,17.6f,0);
  OfficeProp2736("08",FutureProp2717.OfficeSofa,4,18.5f,270);
  OfficeProp2736("08",FutureProp2717.Bookcase,11,23.5f,0);
  OfficeProp2736("08",FutureProp2717.CoffeeCorner,11,14.6f,180);
  OfficeProp2736("08",FutureProp2717.CoatStand,3,22.5f,0);
  OfficeProp2736("08",FutureProp2717.OfficePlant,13,22.8f,0);
  OfficeProp2736("08",FutureProp2717.PortableFan,10,18,0);
  OfficeWall2736("08",FutureProp2717.NoticeBoard,4,1.5f,23.82f,0);
  OfficeWall2736("08",FutureProp2717.WallClock,11,2.2f,23.82f,0);
  OfficeWall2736("08",FutureProp2717.WallAirConditioner,13.82f,2.3f,18,90);
  foreach(string id in new[]{"S1","S2"}){var r=OfficeRooms2726.Find(v=>v.Id==id);OfficeProp2736(id,FutureProp2717.FireExtinguisher,r.Area.xMax-.5f,r.Area.yMin+.6f,0);OfficeWall2736(id,FutureProp2717.NoticeBoard,r.Area.xMax-.18f,1.5f,r.Area.yMax-1,90);}
  var rng=new System.Random((RaidGame.Instance?RaidGame.Instance.RaidSeed:2742)^22742);
  OfficeLoot2736("01","茶水间储物柜",ContainerLook.KitchenCupboard,-3,-22,90,rng);OfficeLoot2736("01","调度员行李",ContainerLook.RollingSuitcase,-3,-18,90,rng);
  foreach(float z in new[]{-14f,-10f,-7.2f})OfficeLoot2736("02","调度值班交接柜"+z,ContainerLook.FileCabinet,-19.2f,z,270,rng);
  foreach(float z in new[]{-22f,-18f,-10f})OfficeLoot2736("03","港口调度档案柜"+z,ContainerLook.FileCabinet,19.2f,z,90,rng);
  OfficeLoot2736("03","调度大厅封存资料箱",ContainerLook.ArchiveCase,15,-23.2f,180,rng);
  foreach(float z in new[]{0f,4f,9f})OfficeLoot2736("04","监控设备记录柜"+z,ContainerLook.SupplyLocker,-19.2f,z,270,rng);
  OfficeLoot2736("05","调度会议文件柜",ContainerLook.FileCabinet,19.2f,-1,90,rng);OfficeLoot2736("05","会议器材箱",ContainerLook.PrecisionCase,19.2f,9,90,rng);
  OfficeLoot2736("06","通信室维修箱",ContainerLook.Toolbox,-19.2f,16,270,rng);OfficeLoot2736("06","通信备件柜",ContainerLook.SupplyLocker,-19.2f,22,270,rng);
  OfficeLoot2736("07","应急药柜",ContainerLook.MedicalCabinet,-2.2f,16.2f,90,rng,true);
  OfficeLoot2736("08","主管调度资料柜",ContainerLook.FileCabinet,13.2f,16.8f,90,rng);OfficeLoot2736("08","主管个人公文箱",ContainerLook.Suitcase,13.2f,20.2f,90,rng);OfficeLoot2736("08","主管交接文件箱",ContainerLook.ArchiveCase,4,23.2f,0,rng);
  OfficeLeftSafe2742("08","调度主管小保险箱",new Vector3(8,0,23),0,SafeSize240.Small,rng);
  Physics.SyncTransforms();
 }
 void OfficeLeftSafe2742(string id,string name,Vector3 model,float yaw,SafeSize240 size,System.Random rng){
  var toward=Quaternion.Euler(0,yaw,0)*Vector3.back;var stand=model+toward*1.35f;
  var safe=AddContainer(stand,model,ContainerLook.Safe,SupplyType.Safe,rng,HarborDistrict.Office,yaw,false,ContainerTier.Safe,DropRegion230.Office,size);safe.Name=name;FreightPlaceOnFloor2717(safe.View);
  OfficeSearchRecords2736.Add(new FreightLootRecord2717{Container=safe,Model=safe.View,Stand=stand,AccessPoint=stand+toward*.6f,RoomName=OfficeRooms2726.Find(r=>r.Id==id).Name,Yaw=yaw});
 }
}}
