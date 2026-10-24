using UnityEngine;
namespace GreyHarbor {
 // Placement intent accompanies actual furniture; dimensions still come from its model.
 public sealed class RepairMainFurniture274:MonoBehaviour {
  public string RoomName;public bool WallBack;public int WallSide=-1;
 }
 public sealed partial class RaidMap {
  // Side order: north, east, south, west. Models service towards local -Z.
  public static float MainWallYaw274(int side){return side==0?0:side==1?90:side==2?180:270;}
  public static Vector3 MainWallAnchor274(Rect r,int side,float along,float rear){
   float edge=side==0?r.yMax:side==1?r.xMax:side==2?r.yMin:r.xMin;
   bool shell=side==0||side==2?Mathf.Abs(edge)>MainNetHalfZ276-.01f:Mathf.Abs(edge)>MainNetHalfX276-.01f;
   float inset=(shell?0:.1f)+.08f+rear;
   return side==0?new Vector3(along,0,edge-inset):side==1?new Vector3(edge-inset,0,along):side==2?new Vector3(along,0,edge+inset):new Vector3(edge+inset,0,along);
  }
  static Rect MainNorthStairRoom274(){return RepairMainRoomArea276(1,"西北楼梯");}
  static Rect MainSouthStairRoom274(){return RepairMainRoomArea276(1,"东南楼梯");}
  public static Vector3 RepairMainStairAnchor274(bool north){
   var r=north?MainNorthStairRoom274():MainSouthStairRoom274();
   return new Vector3(north?r.xMin+.705f:r.xMax-.705f,0,r.yMax-4.80f);
  }
  public static Vector3 RepairMainArrivalAnchor274(bool north){
   var flight=RepairMainStairAnchor274(north);
   return new Vector3(north?-MainNetHalfX276+2.74f:MainNetHalfX276-2.74f,0,flight.z+(north?.31f:1.7f));
  }
  public static bool RepairMainSupportAnchor274(int floor,string name,out Vector3 position,out float yaw){
   position=Vector3.zero;yaw=0;
   if(floor==1&&(name=="检修工具箱"||name=="精密拆修工具箱")){
    var parts=RepairMainRoomArea276(1,"零件整理与待修件间");float wall=parts.xMax;
    position=new Vector3(wall+.1f+.08f+.45f,0,parts.yMin+parts.height*(name=="检修工具箱"?.26f:.73f));yaw=270;return true;
   }
   if(floor==2&&name=="拆修台工具箱"){
    var r=RepairMainRoomArea276(2,"部件拆修车间");
    position=new Vector3(r.center.x-1.7f,0,r.center.y-1.1f);return true;
   }
   if(floor==4&&name=="职工遗留手提箱"){
    var r=RepairMainRoomArea276(4,"职工休息室");
    position=MainWallAnchor274(r,0,r.center.x,.40f);return true;
   }
   return false;
  }
  Transform MainPlaced274(Transform t,string room,bool wall,int side=-1){
   var p=t.gameObject.AddComponent<RepairMainFurniture274>();p.RoomName=room;p.WallBack=wall;p.WallSide=side;return t;
  }
  Vector3 MainSupport274(int floor,string name){Vector3 p;float yaw;RepairMainSupportAnchor274(floor,name,out p,out yaw);return p;}
  public static Vector3 RepairMainOfficeDeskAnchor274(){var r=RepairMainRoomArea276(1,"维修值班办公室");return new Vector3(r.center.x,0,r.center.y);}
 }
}
