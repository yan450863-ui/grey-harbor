using UnityEngine;
namespace GreyHarbor { public sealed partial class RaidMap {
 public void BuildOfficeThirdDressing2740(){
  if(OfficeFloor2733!=3)return;
  // Each room keeps its centre and a direct service lane clear. The research key room is furnished separately.
  OfficeProp2736("01",FutureProp2717.OfficeSofa,-11,-23.2f,180);
  OfficeProp2736("01",FutureProp2717.CoffeeCorner,-11,-17,0);
  OfficeProp2736("01",FutureProp2717.StaffFridge,-13,-17,0);
  OfficeProp2736("01",FutureProp2717.WaterDispenser,-6,-17,0);
  OfficeProp2736("01",FutureProp2717.MeetingTable,-5.8f,-20,90);
  OfficeProp2736("01",FutureProp2717.VisitorChair,-7,-20,90);
  OfficeProp2736("01",FutureProp2717.VisitorChair,-4.6f,-20,270);
  OfficeProp2736("01",FutureProp2717.CoatStand,-13,-22,0);
  OfficeProp2736("01",FutureProp2717.OfficePlant,-10,-17,0);
  OfficeProp2736("01",FutureProp2717.OverflowWasteBin,-12,-18.5f,0);
  OfficeWall2736("01",FutureProp2717.NoticeBoard,-10,1.4f,-23.82f,180);
  OfficeWall2736("01",FutureProp2717.WallClock,-5,2.2f,-23.82f,180);
  OfficeWall2736("01",FutureProp2717.WallAirConditioner,-13.82f,2.3f,-18.5f,270);

  OfficeProp2736("02",FutureProp2717.ReceptionCounter,-15,-13,180);
  OfficeProp2736("02",FutureProp2717.OfficeChair,-15,-14.4f,0);
  OfficeProp2736("02",FutureProp2717.OfficeSofa,-14,-7,0);
  OfficeProp2736("02",FutureProp2717.WaitingBench,-5,-13,90);
  OfficeProp2736("02",FutureProp2717.LobbyDirectory,-9,-7,0);
  OfficeProp2736("02",FutureProp2717.VisitorKiosk,-5,-7.2f,0);
  OfficeProp2736("02",FutureProp2717.OfficePlant,-17,-15,0);
  OfficeProp2736("02",FutureProp2717.OfficePlant,-10,-14.8f,0);
  OfficeProp2736("02",FutureProp2717.UmbrellaStand,-6,-15,0);
  OfficeProp2736("02",FutureProp2717.WaterDispenser,-17,-9,0);
  OfficeProp2736("02",FutureProp2717.OverflowWasteBin,-16,-7,0);
  OfficeWall2736("02",FutureProp2717.NoticeBoard,-19.82f,1.5f,-10.5f,270);
  OfficeWall2736("02",FutureProp2717.WallClock,-14,2.2f,-6.18f,0);
  OfficeWall2736("02",FutureProp2717.WallAirConditioner,-2.18f,2.3f,-13,90);

  foreach(float z in new[]{-21f,-9f}){
   OfficeProp2736("03",FutureProp2717.MeetingTable,11,z,0);
   foreach(float x in new[]{9.8f,11f,12.2f}){
    OfficeProp2736("03",FutureProp2717.VisitorChair,x,z-1.2f,0);
    OfficeProp2736("03",FutureProp2717.VisitorChair,x,z+1.2f,180);
   }
  }
  OfficeProp2736("03",FutureProp2717.ProjectionScreen,11,-23.1f,180);
  OfficeProp2736("03",FutureProp2717.ProjectionCart,16,-21,180);
  OfficeProp2736("03",FutureProp2717.ConferenceLectern,5,-22,180);
  OfficeProp2736("03",FutureProp2717.MeetingEquipmentCabinet,4,-7,0);
  OfficeProp2736("03",FutureProp2717.FoldedTableCart,18,-7,0);
  OfficeProp2736("03",FutureProp2717.WaterDispenser,4,-18,270);
  OfficeProp2736("03",FutureProp2717.OfficePlant,18,-16,0);
  OfficeProp2736("03",FutureProp2717.OverflowWasteBin,18,-18,0);
  OfficeWall2736("03",FutureProp2717.Whiteboard,11,1.4f,-6.18f,0);
  OfficeWall2736("03",FutureProp2717.WallClock,17,2.2f,-6.18f,0);
  OfficeWall2736("03",FutureProp2717.WallAirConditioner,19.82f,2.3f,-15,90);

  // The manager's north/south bypass stays on x = -14.
  OfficeProp2736("04",FutureProp2717.ExecutiveDesk,-17,0,270);
  OfficeProp2736("04",FutureProp2717.OfficeChair,-18.5f,0,270);
  OfficeProp2736("04",FutureProp2717.VisitorChair,-15.4f,-1,90);
  OfficeProp2736("04",FutureProp2717.VisitorChair,-15.4f,1,90);
  OfficeProp2736("04",FutureProp2717.OfficeSofa,-11,10.2f,0);
  OfficeProp2736("04",FutureProp2717.MeetingTable,-11,7,0);
  OfficeProp2736("04",FutureProp2717.VisitorChair,-11,5.8f,0);
  OfficeProp2736("04",FutureProp2717.Bookcase,-19.4f,9.5f,270);
  OfficeProp2736("04",FutureProp2717.WaterDispenser,-9.2f,3,90);
  OfficeProp2736("04",FutureProp2717.CoatStand,-19,-2,0);
  OfficeProp2736("04",FutureProp2717.OfficePlant,-9,-2,0);
  OfficeProp2736("04",FutureProp2717.OverflowWasteBin,-18,-2,0);
  OfficeWall2736("04",FutureProp2717.NoticeBoard,-19.82f,1.5f,3,270);
  OfficeWall2736("04",FutureProp2717.WallClock,-10,2.2f,-2.82f,180);
  OfficeWall2736("04",FutureProp2717.WallAirConditioner,-8.18f,2.3f,3,90);

  var living=new FerryLivingKit144(this);var repair=new RepairInteriorKit200(this);
  OfficeTrack2736("05","Toilet",living.Toilet(new Vector3(-7.1f,0,-2),180).gameObject);
  OfficeTrack2736("05","Toilet",living.Toilet(new Vector3(-7.1f,0,2.9f),0).gameObject);
  OfficeTrack2736("05","WashStation",repair.WashStation(new Vector3(-7.4f,0,.5f),270).gameObject);
  OfficeProp2736("05",FutureProp2717.OverflowWasteBin,-3.2f,3,0);
  OfficeProp2736("05",FutureProp2717.FireExtinguisher,-3.2f,-2.4f,0);
  OfficeWall2736("05",FutureProp2717.WallClock,-4.5f,2.2f,3.82f,0);

  foreach(float x in new[]{-7f,-5f}){
   OfficeProp2736("06",FutureProp2717.OpenArchiveShelf,x,10.5f,0);
  }
  OfficeProp2736("06",FutureProp2717.TallFileCabinet,-7,4.55f,180);
  OfficeProp2736("06",FutureProp2717.ArchiveCart,-7,5.8f,270);
  OfficeProp2736("06",FutureProp2717.PaperBoxStack,-7,9.3f,270);
  OfficeWall2736("06",FutureProp2717.NoticeBoard,-5,1.4f,10.82f,0);
  OfficeWall2736("06",FutureProp2717.WallClock,-3,2.2f,10.82f,0);

  // A broad lane on x = 4 bypasses the collapsed corridor equipment.
  foreach(float x in new[]{7f,11f,15f})foreach(float z in new[]{-1.4f,9.4f})OfficeProp2736("07",FutureProp2717.SingleWorkstation,x,z,z<4?180:0);
  OfficeProp2736("07",FutureProp2717.RoundMeetingSet,15,4,0);
  OfficeProp2736("07",FutureProp2717.Copier,18,3,90);
  OfficeProp2736("07",FutureProp2717.PaperShredder,18,5,90);
  OfficeProp2736("07",FutureProp2717.MailSortingShelf,6,5,90);
  OfficeProp2736("07",FutureProp2717.RecordTrolley,6,2,90);
  OfficeProp2736("07",FutureProp2717.LowDrawerCabinet,10,10.4f,0);
  OfficeProp2736("07",FutureProp2717.PaperBoxStack,17,10,0);
  OfficeProp2736("07",FutureProp2717.OfficePlant,3,-2,0);
  OfficeProp2736("07",FutureProp2717.WaterDispenser,3,10,0);
  OfficeProp2736("07",FutureProp2717.FallenOfficeChair,17,6.5f,15);
  OfficeWall2736("07",FutureProp2717.Whiteboard,11,1.4f,10.82f,0);
  OfficeWall2736("07",FutureProp2717.WallClock,17,2.2f,-2.82f,180);
  OfficeWall2736("07",FutureProp2717.WallAirConditioner,19.82f,2.3f,6,90);

  foreach(float x in new[]{-18f,-11.3f})OfficeProp2736("08",FutureProp2717.PersonalLocker,x,23.2f,0);
  foreach(float z in new[]{18f,21.5f})OfficeProp2736("08",FutureProp2717.OpenArchiveShelf,-19.5f,z,270);
  OfficeProp2736("08",FutureProp2717.TallFileCabinet,-16,18,90);
  OfficeProp2736("08",FutureProp2717.TallFileCabinet,-10.6f,22,90);
  OfficeProp2736("08",FutureProp2717.ArchiveCart,-18,21,270);
  OfficeProp2736("08",FutureProp2717.PaperBoxStack,-17,23,0);
  OfficeProp2736("08",FutureProp2717.LowDrawerCabinet,-12,23.3f,0);
  OfficeWall2736("08",FutureProp2717.NoticeBoard,-18,1.5f,23.82f,0);
  OfficeWall2736("08",FutureProp2717.WallClock,-11,2.2f,23.82f,0);
  OfficeWall2736("08",FutureProp2717.WallAirConditioner,-19.82f,2.3f,16,270);

  foreach(float x in new[]{-8.5f,-6.5f})OfficeProp2736("09",FutureProp2717.ServerRack,x,23,0);
  OfficeProp2736("09",FutureProp2717.ServerRack,-4.5f,20.5f,0);
  OfficeProp2736("09",FutureProp2717.TunnelWorkBench,-8,15.3f,180);
  OfficeProp2736("09",FutureProp2717.ControlPanel,-9.4f,16.8f,270);
  OfficeProp2736("09",FutureProp2717.ToolTrolley,-8.5f,21,270);
  OfficeProp2736("09",FutureProp2717.PortableFan,-7,20.5f,0);
  OfficeProp2736("09",FutureProp2717.PaperBoxStack,-3,19,90);
  OfficeProp2736("09",FutureProp2717.FireExtinguisher,-9,22,0);
  OfficeWall2736("09",FutureProp2717.NoticeBoard,-6,1.4f,23.82f,0);
  OfficeWall2736("09",FutureProp2717.WallClock,-3,2.2f,23.82f,0);
  OfficeWall2736("09",FutureProp2717.WallAirConditioner,-2.18f,2.3f,19,90);
  foreach(string id in new[]{"S1","S2"}){
   var room=OfficeRooms2726.Find(r=>r.Id==id);
   OfficeProp2736(id,FutureProp2717.FireExtinguisher,room.Area.xMax-.5f,room.Area.yMin+.6f,0);
   OfficeWall2736(id,FutureProp2717.NoticeBoard,room.Area.xMax-.18f,1.5f,room.Area.yMax-1,90);
  }
  BuildOfficeThirdContainers2740();Physics.SyncTransforms();
 }
}}
