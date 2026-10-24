using UnityEngine;
namespace GreyHarbor {public sealed partial class RaidMap {
 public void BuildOfficeSecondDressing2738(){
  if(OfficeFloor2733!=2)return;
  // Department work groups occupy the edges; room centres, hole reservations and doors remain clear.
  foreach(float x in new[]{-11f,-6f})OfficeProp2736("01",FutureProp2717.SingleWorkstation,x,-23,180);
  OfficeProp2736("01",FutureProp2717.TunnelWorkBench,-11,-17,0);
  OfficeProp2736("01",FutureProp2717.ToolTrolley,-12.8f,-18.8f,270);
  OfficeProp2736("01",FutureProp2717.BrokenMonitor,-11,-21.2f,0);
  OfficeProp2736("01",FutureProp2717.PaperBoxStack,-5,-17,0);
  OfficeProp2736("01",FutureProp2717.PortableFan,-10,-21.6f,180);
  OfficeProp2736("01",FutureProp2717.OverflowWasteBin,-12.7f,-22,0);
  OfficeWall2736("01",FutureProp2717.NoticeBoard,-10,1.4f,-23.82f,180);
  OfficeWall2736("01",FutureProp2717.WallClock,-5,2.2f,-23.82f,180);
  OfficeWall2736("01",FutureProp2717.WallAirConditioner,-13.82f,2.3f,-18.5f,270);
  foreach(float x in new[]{-16f,-11f,-6f})OfficeProp2736("02",FutureProp2717.SingleWorkstation,x,-14.7f,180);
  foreach(float x in new[]{-16f,-12f})OfficeProp2736("02",FutureProp2717.LowDrawerCabinet,x,-6.6f,0);
  OfficeProp2736("02",FutureProp2717.Copier,-18.8f,-10,270);
  OfficeProp2736("02",FutureProp2717.PaperShredder,-18.8f,-11.5f,270);
  OfficeProp2736("02",FutureProp2717.ArchiveCart,-7,-12,90);
  OfficeProp2736("02",FutureProp2717.WaitingBench,-14,-11,0);
  OfficeProp2736("02",FutureProp2717.PaperBoxStack,-13,-8,0);
  OfficeProp2736("02",FutureProp2717.OfficePlant,-19,-6.8f,0);
  OfficeProp2736("02",FutureProp2717.WaterDispenser,-5,-6.7f,0);
  OfficeProp2736("02",FutureProp2717.OverflowWasteBin,-9,-6.7f,0);
  OfficeWall2736("02",FutureProp2717.NoticeBoard,-19.82f,1.5f,-13,270);
  OfficeWall2736("02",FutureProp2717.WallClock,-14,2.2f,-6.18f,0);
  OfficeWall2736("02",FutureProp2717.WallAirConditioner,-2.18f,2.3f,-12,90);
  foreach(float x in new[]{7f,11f,15f})foreach(float z in new[]{-21.8f,-8f})OfficeProp2736("03",FutureProp2717.DoubleWorkstation,x,z,z<-15?180:0);
  foreach(float x in new[]{7f,11f,15f})OfficeProp2736("03",FutureProp2717.OfficeChair,x+.8f,-9.4f,180);
  OfficeProp2736("03",FutureProp2717.Copier,18,-7,0);
  OfficeProp2736("03",FutureProp2717.PaperShredder,17,-7,0);
  OfficeProp2736("03",FutureProp2717.MailSortingShelf,4,-22,180);
  OfficeProp2736("03",FutureProp2717.WaitingBench,4,-18,270);
  OfficeProp2736("03",FutureProp2717.PaperBoxStack,18,-20.5f,0);
  OfficeProp2736("03",FutureProp2717.PaperBoxStack,18,-12,0);
  OfficeProp2736("03",FutureProp2717.WaterDispenser,4,-7,0);
  OfficeProp2736("03",FutureProp2717.OfficePlant,18,-16,0);
  OfficeProp2736("03",FutureProp2717.OverflowWasteBin,6,-23,0);
  OfficeProp2736("03",FutureProp2717.PlanMapBoard,10,-23.3f,180);
  OfficeWall2736("03",FutureProp2717.NoticeBoard,10,1.5f,-6.18f,0);
  OfficeWall2736("03",FutureProp2717.WallClock,14,2.2f,-6.18f,0);
  OfficeWall2736("03",FutureProp2717.WallAirConditioner,19.82f,2.3f,-14,90);
  foreach(float x in new[]{-17f,-9f}){
   OfficeProp2736("04",FutureProp2717.MeetingTable,x,7,90);
   foreach(float z in new[]{5.8f,7f,8.2f}){
    OfficeProp2736("04",FutureProp2717.VisitorChair,x-1.2f,z,90);
    OfficeProp2736("04",FutureProp2717.VisitorChair,x+1.2f,x==-17f&&z==7f?9.7f:z,270);
   }
  }
  OfficeProp2736("04",FutureProp2717.ProjectionScreen,-19,0,270);
  OfficeProp2736("04",FutureProp2717.ProjectionCart,-18.7f,-2,180);
  OfficeProp2736("04",FutureProp2717.ConferenceLectern,-10,-1.8f,180);
  OfficeProp2736("04",FutureProp2717.MeetingEquipmentCabinet,-7,10.4f,0);
  OfficeProp2736("04",FutureProp2717.FoldedTableCart,-7,0,90);
  OfficeProp2736("04",FutureProp2717.WaterDispenser,-7,3,90);
  OfficeProp2736("04",FutureProp2717.OfficePlant,-19.2f,10,0);
  OfficeProp2736("04",FutureProp2717.OverflowWasteBin,-7,8,90);
  OfficeWall2736("04",FutureProp2717.Whiteboard,-19.82f,1.4f,4,270);
  OfficeWall2736("04",FutureProp2717.WallClock,-7,2.2f,-2.82f,180);
  var living=new FerryLivingKit144(this);var repair=new RepairInteriorKit200(this);
  foreach(string id in new[]{"05","06"}){
   float offset=id=="05"?0:7;
   OfficeTrack2736(id,"Toilet",living.Toilet(new Vector3(-5.15f,0,-2.1f+offset),180).gameObject);
   OfficeTrack2736(id,"Toilet",living.Toilet(new Vector3(-5.15f,0,2.8f+offset),0).gameObject);
   OfficeTrack2736(id,"WashStation",repair.WashStation(new Vector3(-5.45f,0,.5f+offset),270).gameObject);
  }
  foreach(float x in new[]{7f,11f,15f})foreach(float z in new[]{-1.4f,9.4f})OfficeProp2736("07",FutureProp2717.SingleWorkstation,x,z,z<0?180:0);
  OfficeProp2736("07",FutureProp2717.DraftingTable,18,2,90);
  OfficeProp2736("07",FutureProp2717.BlueprintRack,18,5,90);
  OfficeProp2736("07",FutureProp2717.PlanMapBoard,10,10.4f,0);
  OfficeProp2736("07",FutureProp2717.RoundMeetingSet,11,2,0);
  OfficeProp2736("07",FutureProp2717.RecordTrolley,4,1,270);
  OfficeProp2736("07",FutureProp2717.PaperBoxStack,4,4,270);
  OfficeProp2736("07",FutureProp2717.WaterDispenser,3,10,0);
  OfficeProp2736("07",FutureProp2717.OfficePlant,3,-2,0);
  OfficeProp2736("07",FutureProp2717.OverflowWasteBin,17,10,0);
  OfficeProp2736("07",FutureProp2717.FallenOfficeChair,16,6.5f,0);
  OfficeWall2736("07",FutureProp2717.Whiteboard,11,1.4f,10.82f,0);
  OfficeWall2736("07",FutureProp2717.WallClock,17,2.2f,-2.82f,180);
  OfficeWall2736("07",FutureProp2717.WallAirConditioner,19.82f,2.3f,6,90);
  foreach(float x in new[]{-18f,-14f,-10f})OfficeProp2736("08",FutureProp2717.OpenArchiveShelf,x,23.4f,0);
  foreach(float z in new[]{16.3f,20.5f})OfficeProp2736("08",FutureProp2717.OpenArchiveShelf,-19.5f,z,270);
  OfficeProp2736("08",FutureProp2717.ArchiveCart,-17,21,0);
  OfficeProp2736("08",FutureProp2717.PaperBoxStack,-10,21,0);
  OfficeProp2736("08",FutureProp2717.SingleWorkstation,-10.5f,16.5f,180);
  OfficeProp2736("08",FutureProp2717.Bookcase,-16,14.5f,180);
  OfficeProp2736("08",FutureProp2717.OverflowWasteBin,-19,22.5f,0);
  OfficeWall2736("08",FutureProp2717.NoticeBoard,-14,1.5f,23.82f,0);
  OfficeWall2736("08",FutureProp2717.WallClock,-18,2.2f,23.82f,0);
  foreach(float x in new[]{-7f,-5.7f})OfficeProp2736("09",FutureProp2717.ServerRack,x,23,0);
  OfficeProp2736("09",FutureProp2717.TunnelWorkBench,-6.8f,15.3f,180);
  OfficeProp2736("09",FutureProp2717.ToolTrolley,-3,16,90);
  OfficeProp2736("09",FutureProp2717.PortableFan,-7,21,270);
  OfficeProp2736("09",FutureProp2717.PaperBoxStack,-3,20,90);
  OfficeProp2736("09",FutureProp2717.ControlPanel,-2.7f,18,90);
  OfficeWall2736("09",FutureProp2717.NoticeBoard,-6,1.5f,23.82f,0);
  OfficeWall2736("09",FutureProp2717.WallAirConditioner,-2.18f,2.3f,21,90);
  OfficeProp2736("10",FutureProp2717.ExecutiveDesk,7,15.5f,180);
  OfficeProp2736("10",FutureProp2717.OfficeChair,7,17.5f,0);
  OfficeProp2736("10",FutureProp2717.OfficeSofa,7,23.25f,0);
  OfficeProp2736("10",FutureProp2717.CoffeeCorner,4,14.6f,180);
  OfficeProp2736("10",FutureProp2717.WaterDispenser,12.8f,14.7f,180);
  OfficeProp2736("10",FutureProp2717.Bookcase,11,23.5f,0);
  OfficeProp2736("10",FutureProp2717.RoundMeetingSet,11.3f,20,0);
  OfficeProp2736("10",FutureProp2717.CoatStand,3,22.5f,0);
  OfficeProp2736("10",FutureProp2717.OfficePlant,13,22.5f,0);
  OfficeProp2736("10",FutureProp2717.OverflowWasteBin,10,14.7f,0);
  OfficeWall2736("10",FutureProp2717.NoticeBoard,7,1.5f,23.82f,0);
  OfficeWall2736("10",FutureProp2717.WallClock,11,2.2f,23.82f,0);
  OfficeWall2736("10",FutureProp2717.WallAirConditioner,13.82f,2.3f,17,90);
  foreach(string id in new[]{"S1","S2"}){
   var room=OfficeRooms2726.Find(r=>r.Id==id);
   OfficeProp2736(id,FutureProp2717.FireExtinguisher,room.Area.xMax-.5f,room.Area.yMin+.6f,0);
   OfficeWall2736(id,FutureProp2717.NoticeBoard,room.Area.xMax-.18f,1.5f,room.Area.yMax-1,90);
  }
  OfficeProp2736("09",FutureProp2717.PaperScatter,-4.8f,21,0);
  OfficeProp2736("01",FutureProp2717.PaperScatter,-6.5f,-21,0);
  OfficeProp2736("02",FutureProp2717.PaperScatter,-15,-8.5f,15);
  OfficeProp2736("07",FutureProp2717.PaperScatter,14,5.5f,20);
  BuildOfficeSecondContainers2738();Physics.SyncTransforms();
 }
}}


