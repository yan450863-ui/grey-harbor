using UnityEngine;
namespace GreyHarbor {public sealed partial class RaidMap {
 void OfficeThirdPlan2735(){
  OfficeRooms2726.Clear();OfficeOpenings2726.Clear();OfficeBlockers2726.Clear();
  OfficeRooms2726.Add(new OfficeRoom2726{Id="S1",Name="南部主楼梯",Area=new Rect(-20f,-24f,6f,8f),Stair=true});
  OfficeRooms2726.Add(new OfficeRoom2726{Id="01",Name="茶水休息室",Area=new Rect(-14f,-24f,12f,8f),Stair=false});
  OfficeRooms2726.Add(new OfficeRoom2726{Id="02",Name="管理接待室",Area=new Rect(-20f,-16f,18f,10f),Stair=false});
  OfficeRooms2726.Add(new OfficeRoom2726{Id="03",Name="董事会议室",Area=new Rect(2f,-24f,18f,18f),Stair=false});
  OfficeRooms2726.Add(new OfficeRoom2726{Id="04",Name="总经理办公室",Area=new Rect(-20f,-3f,12f,14f),Stair=false});
  OfficeRooms2726.Add(new OfficeRoom2726{Id="05",Name="卫生间",Area=new Rect(-8f,-3f,6f,7f),Stair=false});
  OfficeRooms2726.Add(new OfficeRoom2726{Id="06",Name="行政档案室",Area=new Rect(-8f,4f,6f,7f),Stair=false});
  OfficeRooms2726.Add(new OfficeRoom2726{Id="07",Name="管理办公区",Area=new Rect(2f,-3f,18f,14f),Stair=false});
  OfficeRooms2726.Add(new OfficeRoom2726{Id="08",Name="重要资料库",Area=new Rect(-20f,14f,10f,10f),Stair=false});
  OfficeRooms2726.Add(new OfficeRoom2726{Id="09",Name="通信机房",Area=new Rect(-10f,14f,8f,10f),Stair=false});
  OfficeRooms2726.Add(new OfficeRoom2726{Id="10",Name="项目研究室",Area=new Rect(2f,14f,12f,10f),Stair=false});
  OfficeRooms2726.Add(new OfficeRoom2726{Id="S2",Name="北部后勤楼梯",Area=new Rect(14f,16f,6f,8f),Stair=true});
  OfficeOpenings2726.Add(new OfficeOpening2726{Orientation="v",X=-14f,Z=-20f,Width=2f,Type="door"});
  OfficeOpenings2726.Add(new OfficeOpening2726{Orientation="v",X=-2f,Z=-10f,Width=2f,Type="door"});
  OfficeOpenings2726.Add(new OfficeOpening2726{Orientation="v",X=2f,Z=-14f,Width=2f,Type="door"});
  OfficeOpenings2726.Add(new OfficeOpening2726{Orientation="h",X=-14f,Z=-3f,Width=2f,Type="door"});
  OfficeOpenings2726.Add(new OfficeOpening2726{Orientation="h",X=-14f,Z=11f,Width=2f,Type="door"});
  OfficeOpenings2726.Add(new OfficeOpening2726{Orientation="v",X=-2f,Z=0.5f,Width=2f,Type="door"});
  OfficeOpenings2726.Add(new OfficeOpening2726{Orientation="v",X=-2f,Z=7.5f,Width=2f,Type="door"});
  OfficeOpenings2726.Add(new OfficeOpening2726{Orientation="v",X=2f,Z=0f,Width=2f,Type="door"});
  OfficeOpenings2726.Add(new OfficeOpening2726{Orientation="v",X=2f,Z=8f,Width=2f,Type="door"});
  OfficeOpenings2726.Add(new OfficeOpening2726{Orientation="h",X=-6f,Z=14f,Width=2f,Type="door"});
  OfficeOpenings2726.Add(new OfficeOpening2726{Orientation="v",X=2f,Z=19f,Width=2f,Type="door"});
  OfficeOpenings2726.Add(new OfficeOpening2726{Orientation="h",X=17f,Z=16f,Width=2f,Type="door"});
  OfficeOpenings2726.Add(new OfficeOpening2726{Orientation="h",X=-8f,Z=-16f,Width=2.2f,Type="hole"});
  OfficeOpenings2726.Add(new OfficeOpening2726{Orientation="v",X=-10f,Z=19f,Width=2.2f,Type="hole"});
  OfficeOpenings2726.Add(new OfficeOpening2726{Orientation="v",X=-8f,Z=7.5f,Width=2.2f,Type="hole"});
  OfficeBlockers2726.Add(new Rect(-2f,2.3999999999999986f,4f,1.2000000000000028f));
 }
 public static Vector3 OfficeStairPoint2735(int floor,int target,bool north){
  var p=OfficeStairArrival2733(north);return floor==2&&target==1?p+Vector3.forward*2:p;
 }
 void OfficeStairPortal2735(bool north,int target){
  bool up=target>OfficeFloor2733;var p=OfficeStairPoint2735(OfficeFloor2733,target,north);
  Doors.Add(new WorldDoor{Name=(north?"北部":"南部")+"楼梯 · "+(up?"上楼至":"下楼至")+target+"楼",Destination="office-floor:"+target+":"+(north?"north":"south"),Portal=true,Position=p});
  Box("OfficeStairDirection2733",p+Vector3.up*.02f,new Vector3(.9f,.035f,.9f),up?"D5A24F":"59A9BC");
 }
 void OfficeFloorCovers2735(Vector3[] points){
  foreach(var p in points)BuildOfficeFloorHole2748(p);
 }
}}
