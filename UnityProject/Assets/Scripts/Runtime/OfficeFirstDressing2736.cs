using System.Collections.Generic;
using UnityEngine;
namespace GreyHarbor {
 public sealed partial class RaidMap {
  public readonly List<FreightDressingRecord2717> OfficeDressingRecords2736=new List<FreightDressingRecord2717>();
  public void BuildOfficeFirstDressing2736(){
   if(OfficeFloor2733!=1)return;
   // Desks and service groups flank the walking routes rather than filling room centres.
   OfficeProp2736("01",FutureProp2717.SecurityDesk,-11,-23,180);
   OfficeProp2736("01",FutureProp2717.SingleWorkstation,-5.5f,-23,180);
   OfficeProp2736("01",FutureProp2717.PersonalLocker,-12,-17,0);
   OfficeWall2736("01",FutureProp2717.KeyCabinet,-10,1.0f,-16.18f,0);
   OfficeProp2736("01",FutureProp2717.WaitingBench,-5,-17,0);
   OfficeProp2736("01",FutureProp2717.OverflowWasteBin,-12.8f,-22,0);
   OfficeProp2736("01",FutureProp2717.PortableFan,-10,-21.8f,180);
   OfficeWall2736("01",FutureProp2717.NoticeBoard,-10,1.4f,-23.82f,180);
   OfficeWall2736("01",FutureProp2717.WallClock,-5,2.2f,-23.82f,180);
   OfficeProp2736("02",FutureProp2717.ReceptionCounter,-15,-13,180);
   OfficeProp2736("02",FutureProp2717.OfficeChair,-15,-14.4f,0);
   foreach(float z in new[]{-12f,-9f})OfficeProp2736("02",FutureProp2717.WaitingBench,-18.7f,z,270);
   OfficeProp2736("02",FutureProp2717.WaitingBench,-10,-7,0);
   OfficeProp2736("02",FutureProp2717.LobbyDirectory,-13,-7,0);
   OfficeProp2736("02",FutureProp2717.VisitorKiosk,-6,-10.8f,90);
   OfficeProp2736("02",FutureProp2717.OfficePlant,-17,-15,0);
   OfficeProp2736("02",FutureProp2717.OfficePlant,-6,-6.8f,0);
   OfficeProp2736("02",FutureProp2717.UmbrellaStand,-6,-15,0);
   OfficeProp2736("02",FutureProp2717.WaterDispenser,-17,-9,0);
   OfficeProp2736("02",FutureProp2717.OverflowWasteBin,-16,-7,0);
   OfficeWall2736("02",FutureProp2717.NoticeBoard,-19.82f,1.5f,-10.5f,270);
   OfficeWall2736("02",FutureProp2717.WallClock,-14,2.2f,-6.18f,0);
   // Business hall: two separated rows of paired freight-document workstations.
   foreach(float x in new[]{7f,11f,15f})foreach(float z in new[]{-20f,-8f})OfficeProp2736("03",FutureProp2717.DoubleWorkstation,x==15f&&z==-20f?17f:x,z,z<-15?180:0);
   foreach(float x in new[]{7f,11f,15f})OfficeProp2736("03",FutureProp2717.OfficeChair,x+.8f,-9.4f,180);
   OfficeProp2736("03",FutureProp2717.Copier,18,-7,0);
   OfficeProp2736("03",FutureProp2717.PaperShredder,17,-7,0);
   OfficeProp2736("03",FutureProp2717.MailSortingShelf,4,-22,180);
   OfficeProp2736("03",FutureProp2717.WaitingBench,4,-18,270);
   OfficeProp2736("03",FutureProp2717.PaperBoxStack,18,-20,0);
   OfficeProp2736("03",FutureProp2717.PaperBoxStack,18,-12,0);
   OfficeProp2736("03",FutureProp2717.WaterDispenser,4,-7,0);
   OfficeProp2736("03",FutureProp2717.OfficePlant,18,-16,0);
   OfficeProp2736("03",FutureProp2717.OverflowWasteBin,6,-23,0);
   OfficeProp2736("03",FutureProp2717.PlanMapBoard,10,-23.3f,180);
   OfficeWall2736("03",FutureProp2717.NoticeBoard,10,1.5f,-6.18f,0);
   OfficeWall2736("03",FutureProp2717.WallClock,14,2.2f,-6.18f,0);
   foreach(float z in new[]{0f,4f,8f})OfficeProp2736("04",FutureProp2717.SingleWorkstation,-17,z,270);
   foreach(float z in new[]{0f,8f})OfficeProp2736("04",FutureProp2717.SingleWorkstation,-9,z,90);
   OfficeProp2736("04",FutureProp2717.Copier,-19,1.8f,270);
   OfficeProp2736("04",FutureProp2717.PaperShredder,-19,6,270);
   OfficeProp2736("04",FutureProp2717.PaperBoxStack,-15.5f,10,0);
   OfficeProp2736("04",FutureProp2717.OpenArchiveShelf,-15.8f,-2.4f,180);
   OfficeProp2736("04",FutureProp2717.OfficePlant,-19,-2,0);
   OfficeProp2736("04",FutureProp2717.OverflowWasteBin,-19,7.5f,0);
   OfficeProp2736("04",FutureProp2717.VisitorChair,-10,2,90);
   OfficeProp2736("04",FutureProp2717.VisitorChair,-10,6,90);
   OfficeProp2736("04",FutureProp2717.RecordTrolley,-7,2,90);
   OfficeWall2736("04",FutureProp2717.NoticeBoard,-19.82f,1.5f,4,270);
   OfficeWall2736("04",FutureProp2717.WallClock,-7,2.2f,-2.82f,180);
   var living=new FerryLivingKit144(this);var repair=new RepairInteriorKit200(this);
   foreach(string id in new[]{"05","06"}){
    float offset=id=="05"?0:7;
    OfficeTrack2736(id,"Toilet",living.Toilet(new Vector3(-5.15f,0,-2.1f+offset),180).gameObject);
    OfficeTrack2736(id,"Toilet",living.Toilet(new Vector3(-5.15f,0,2.8f+offset),0).gameObject);
    OfficeTrack2736(id,"WashStation",repair.WashStation(new Vector3(-5.45f,0,.5f+offset),270).gameObject);



   }
   foreach(float z in new[]{0f,8f}){
    OfficeProp2736("07",FutureProp2717.MeetingTable,11,z,0);
    foreach(float x in new[]{9.8f,11f,12.2f}){
     OfficeProp2736("07",FutureProp2717.VisitorChair,x,z-1.2f,0);
     OfficeProp2736("07",FutureProp2717.VisitorChair,x,z+1.2f,180);
    }
   }
   OfficeProp2736("07",FutureProp2717.ProjectionScreen,16.5f,1,90);
   OfficeProp2736("07",FutureProp2717.ProjectionCart,15,7,0);
   OfficeProp2736("07",FutureProp2717.ConferenceLectern,6,-1.5f,180);
   OfficeProp2736("07",FutureProp2717.OfficePlant,3,-2,0);
   OfficeProp2736("07",FutureProp2717.WaterDispenser,3,10,0);
   OfficeProp2736("07",FutureProp2717.FoldedTableCart,5,3,0);
   OfficeWall2736("07",FutureProp2717.Whiteboard,11,1.4f,10.82f,0);
   OfficeWall2736("07",FutureProp2717.WallClock,17,2.2f,-2.82f,180);
   foreach(float x in new[]{-18f,-14f,-10f})OfficeProp2736("08",FutureProp2717.OpenArchiveShelf,x,23.4f,0);
   foreach(float z in new[]{16.3f,20.5f})OfficeProp2736("08",FutureProp2717.OpenArchiveShelf,-19.5f,z,270);
   OfficeProp2736("08",FutureProp2717.ArchiveCart,-17,21,0);
   OfficeProp2736("08",FutureProp2717.PaperBoxStack,-10,21,0);
   OfficeProp2736("08",FutureProp2717.SingleWorkstation,-10.5f,16.5f,180);
   OfficeWall2736("08",FutureProp2717.NoticeBoard,-14,1.5f,23.82f,0);
   OfficeWall2736("08",FutureProp2717.WallClock,-18,2.2f,23.82f,0);
   OfficeProp2736("09",FutureProp2717.ServerRack,-7,23,0);
   OfficeProp2736("09",FutureProp2717.ServerRack,-5.7f,23,0);
   OfficeProp2736("09",FutureProp2717.TunnelWorkBench,-6.8f,15.3f,180);
   OfficeProp2736("09",FutureProp2717.ToolTrolley,-3,16,90);
   OfficeProp2736("09",FutureProp2717.PortableFan,-7,21,270);
   OfficeProp2736("09",FutureProp2717.PaperBoxStack,-3,20,90);
   OfficeProp2736("09",FutureProp2717.ControlPanel,-2.7f,18,90);
   OfficeWall2736("09",FutureProp2717.NoticeBoard,-6,1.5f,23.82f,0);
   OfficeProp2736("10",FutureProp2717.OfficeSofa,7,23.25f,0);
   OfficeProp2736("10",FutureProp2717.CoffeeCorner,7,14.6f,180);
   OfficeProp2736("10",FutureProp2717.StaffFridge,4,14.7f,180);
   OfficeProp2736("10",FutureProp2717.WaterDispenser,9,14.7f,180);
   OfficeProp2736("10",FutureProp2717.MeetingTable,11,19,90);
   OfficeProp2736("10",FutureProp2717.VisitorChair,9.8f,18,90);
   OfficeProp2736("10",FutureProp2717.VisitorChair,9.8f,20,90);
   OfficeProp2736("10",FutureProp2717.CoatStand,3,22.5f,0);
   OfficeProp2736("10",FutureProp2717.OfficePlant,13,22.5f,0);
   OfficeProp2736("10",FutureProp2717.OverflowWasteBin,10,14.7f,0);
   OfficeWall2736("10",FutureProp2717.NoticeBoard,7,1.5f,23.82f,0);
   OfficeWall2736("10",FutureProp2717.WallClock,11,2.2f,23.82f,0);
   foreach(string id in new[]{"S1","S2"}){
    var room=OfficeRooms2726.Find(r=>r.Id==id);
    OfficeProp2736(id,FutureProp2717.FireExtinguisher,room.Area.xMax-.5f,room.Area.yMin+.6f,0);
    OfficeWall2736(id,FutureProp2717.NoticeBoard,room.Area.xMax-.18f,1.5f,room.Area.yMax-1,90);
   }
   BuildOfficeFirstContainers2736();Physics.SyncTransforms();
  }
  void OfficeProp2736(string room,FutureProp2717 id,float x,float z,float yaw){OfficeTrack2736(room,id.ToString(),new FutureAssetKit2717(this).Build(id,new Vector3(x,0,z),yaw));}
  void OfficeWall2736(string room,FutureProp2717 id,float x,float y,float z,float yaw){OfficeTrack2736(room,id.ToString(),new FutureAssetKit2717(this).Build(id,new Vector3(x,y,z),yaw),true);}
  void OfficeTrack2736(string roomId,string type,GameObject view,bool mounted=false){
   if(!mounted)FreightPlaceOnFloor2717(view);Physics.SyncTransforms();
   var room=OfficeRooms2726.Find(r=>r.Id==roomId);view.name="OfficeFirst2736_"+type+"_"+OfficeDressingRecords2736.Count;
   OfficeDressingRecords2736.Add(new FreightDressingRecord2717{Name=view.name,Type=type,RoomName=room==null?roomId:room.Name,View=view,Footprint=FreightActualBounds2717(view),Kind=mounted?ObstacleKind.Decoration:ObstacleKind.Low,WallMounted=mounted});
  }
 }
}



