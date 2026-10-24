using UnityEngine;
namespace GreyHarbor {
 public sealed partial class RaidMap {
  public bool OfficeLeft2741;
  public WorldDoor OfficeLeftLockedDoor2741;
  void OfficeLeftPlan2741(){
   OfficeRooms2726.Clear();OfficeOpenings2726.Clear();OfficeBlockers2726.Clear();
   if(OfficeFloor2733==1){
    OfficeRooms2726.Add(new OfficeRoom2726{Id="S1",Name="南楼梯",Area=new Rect(-20f,-24f,6f,8f),Stair=true});
    OfficeRooms2726.Add(new OfficeRoom2726{Id="01",Name="更衣休息室",Area=new Rect(-14f,-24f,12f,8f),Stair=false});
    OfficeRooms2726.Add(new OfficeRoom2726{Id="02",Name="备件库",Area=new Rect(-20f,-16f,18f,10f),Stair=false});
    OfficeRooms2726.Add(new OfficeRoom2726{Id="03",Name="维修工间",Area=new Rect(2f,-24f,18f,18f),Stair=false});
    OfficeRooms2726.Add(new OfficeRoom2726{Id="04",Name="货物暂存间",Area=new Rect(-20f,-3f,18f,14f),Stair=false});
    OfficeRooms2726.Add(new OfficeRoom2726{Id="05",Name="配电室",Area=new Rect(2f,-3f,18f,14f),Stair=false});
    OfficeRooms2726.Add(new OfficeRoom2726{Id="06",Name="后勤值班室",Area=new Rect(-20f,14f,12f,10f),Stair=false});
    OfficeRooms2726.Add(new OfficeRoom2726{Id="07",Name="卫生间",Area=new Rect(-8f,14f,6f,10f),Stair=false});
    OfficeRooms2726.Add(new OfficeRoom2726{Id="08",Name="工具库",Area=new Rect(2f,14f,12f,10f),Stair=false});
    OfficeRooms2726.Add(new OfficeRoom2726{Id="S2",Name="北楼梯",Area=new Rect(14f,16f,6f,8f),Stair=true});
    OfficeOpenings2726.Add(new OfficeOpening2726{Orientation="v",X=-14f,Z=-20f,Width=2f,Type="door"});
    OfficeOpenings2726.Add(new OfficeOpening2726{Orientation="v",X=-2f,Z=-12f,Width=2f,Type="door"});
    OfficeOpenings2726.Add(new OfficeOpening2726{Orientation="v",X=2f,Z=-14f,Width=2f,Type="door"});
    OfficeOpenings2726.Add(new OfficeOpening2726{Orientation="h",X=-12f,Z=-3f,Width=2f,Type="door"});
    OfficeOpenings2726.Add(new OfficeOpening2726{Orientation="h",X=-12f,Z=11f,Width=2f,Type="door"});
    OfficeOpenings2726.Add(new OfficeOpening2726{Orientation="v",X=2f,Z=4f,Width=2f,Type="door"});
    OfficeOpenings2726.Add(new OfficeOpening2726{Orientation="h",X=-14f,Z=14f,Width=2f,Type="door"});
    OfficeOpenings2726.Add(new OfficeOpening2726{Orientation="h",X=-5f,Z=14f,Width=2f,Type="door"});
    OfficeOpenings2726.Add(new OfficeOpening2726{Orientation="v",X=2f,Z=19f,Width=2f,Type="door"});
    OfficeOpenings2726.Add(new OfficeOpening2726{Orientation="h",X=17f,Z=16f,Width=2f,Type="door"});
    OfficeOpenings2726.Add(new OfficeOpening2726{Orientation="h",X=-8f,Z=-16f,Width=2.2f,Type="hole"});
    OfficeBlockers2726.Add(new Rect(-2f,2.3999999999999986f,4f,1.2000000000000028f));
   }
   else if(OfficeFloor2733==2){
    OfficeRooms2726.Add(new OfficeRoom2726{Id="S1",Name="南楼梯",Area=new Rect(-20f,-24f,6f,8f),Stair=true});
    OfficeRooms2726.Add(new OfficeRoom2726{Id="01",Name="茶水休息室",Area=new Rect(-14f,-24f,12f,8f),Stair=false});
    OfficeRooms2726.Add(new OfficeRoom2726{Id="02",Name="调度值班室",Area=new Rect(-20f,-16f,18f,10f),Stair=false});
    OfficeRooms2726.Add(new OfficeRoom2726{Id="03",Name="港口调度大厅",Area=new Rect(2f,-24f,18f,18f),Stair=false});
    OfficeRooms2726.Add(new OfficeRoom2726{Id="04",Name="监控室",Area=new Rect(-20f,-3f,18f,14f),Stair=false});
    OfficeRooms2726.Add(new OfficeRoom2726{Id="05",Name="调度会议室",Area=new Rect(2f,-3f,18f,14f),Stair=false});
    OfficeRooms2726.Add(new OfficeRoom2726{Id="06",Name="通信室",Area=new Rect(-20f,14f,12f,10f),Stair=false});
    OfficeRooms2726.Add(new OfficeRoom2726{Id="07",Name="卫生间",Area=new Rect(-8f,14f,6f,10f),Stair=false});
    OfficeRooms2726.Add(new OfficeRoom2726{Id="08",Name="调度主管室",Area=new Rect(2f,14f,12f,10f),Stair=false});
    OfficeRooms2726.Add(new OfficeRoom2726{Id="S2",Name="北楼梯",Area=new Rect(14f,16f,6f,8f),Stair=true});
    OfficeOpenings2726.Add(new OfficeOpening2726{Orientation="v",X=-14f,Z=-20f,Width=2f,Type="door"});
    OfficeOpenings2726.Add(new OfficeOpening2726{Orientation="v",X=-2f,Z=-14f,Width=2f,Type="door"});
    OfficeOpenings2726.Add(new OfficeOpening2726{Orientation="v",X=-2f,Z=-8f,Width=2f,Type="door"});
    OfficeOpenings2726.Add(new OfficeOpening2726{Orientation="v",X=2f,Z=-14f,Width=2f,Type="door"});
    OfficeOpenings2726.Add(new OfficeOpening2726{Orientation="h",X=-12f,Z=-3f,Width=2f,Type="door"});
    OfficeOpenings2726.Add(new OfficeOpening2726{Orientation="h",X=-12f,Z=11f,Width=2f,Type="door"});
    OfficeOpenings2726.Add(new OfficeOpening2726{Orientation="v",X=2f,Z=4f,Width=2f,Type="door"});
    OfficeOpenings2726.Add(new OfficeOpening2726{Orientation="h",X=-14f,Z=14f,Width=2f,Type="door"});
    OfficeOpenings2726.Add(new OfficeOpening2726{Orientation="h",X=-5f,Z=14f,Width=2f,Type="door"});
    OfficeOpenings2726.Add(new OfficeOpening2726{Orientation="v",X=2f,Z=19f,Width=2f,Type="door"});
    OfficeOpenings2726.Add(new OfficeOpening2726{Orientation="h",X=17f,Z=16f,Width=2f,Type="door"});
    OfficeOpenings2726.Add(new OfficeOpening2726{Orientation="h",X=-8f,Z=-16f,Width=2.2f,Type="hole"});
    OfficeBlockers2726.Add(new Rect(-2f,-11.6f,4f,1.1999999999999993f));
   }
   else if(OfficeFloor2733==3){
    OfficeRooms2726.Add(new OfficeRoom2726{Id="S1",Name="南楼梯",Area=new Rect(-20f,-24f,6f,8f),Stair=true});
    OfficeRooms2726.Add(new OfficeRoom2726{Id="01",Name="技术休息室",Area=new Rect(-14f,-24f,12f,8f),Stair=false});
    OfficeRooms2726.Add(new OfficeRoom2726{Id="02",Name="检测室",Area=new Rect(-20f,-16f,18f,10f),Stair=false});
    OfficeRooms2726.Add(new OfficeRoom2726{Id="03",Name="设备测试大厅",Area=new Rect(2f,-24f,18f,18f),Stair=false});
    OfficeRooms2726.Add(new OfficeRoom2726{Id="04",Name="技术办公室",Area=new Rect(-20f,-3f,18f,7f),Stair=false});
    OfficeRooms2726.Add(new OfficeRoom2726{Id="05",Name="仪表测试室",Area=new Rect(-20f,4f,18f,7f),Stair=false});
    OfficeRooms2726.Add(new OfficeRoom2726{Id="06",Name="数据机房",Area=new Rect(2f,-3f,18f,14f),Stair=false});
    OfficeRooms2726.Add(new OfficeRoom2726{Id="07",Name="技术资料库",Area=new Rect(-20f,14f,18f,10f),Stair=false});
    OfficeRooms2726.Add(new OfficeRoom2726{Id="08",Name="技术主管办公室",Area=new Rect(2f,14f,12f,10f),Stair=false});
    OfficeRooms2726.Add(new OfficeRoom2726{Id="S2",Name="北楼梯",Area=new Rect(14f,16f,6f,8f),Stair=true});
    OfficeOpenings2726.Add(new OfficeOpening2726{Orientation="v",X=-14f,Z=-20f,Width=2f,Type="door"});
    OfficeOpenings2726.Add(new OfficeOpening2726{Orientation="v",X=-2f,Z=-12f,Width=2f,Type="door"});
    OfficeOpenings2726.Add(new OfficeOpening2726{Orientation="v",X=2f,Z=-14f,Width=2f,Type="door"});
    OfficeOpenings2726.Add(new OfficeOpening2726{Orientation="v",X=2f,Z=-8f,Width=2f,Type="door"});
    OfficeOpenings2726.Add(new OfficeOpening2726{Orientation="v",X=-2f,Z=0.5f,Width=2f,Type="door"});
    OfficeOpenings2726.Add(new OfficeOpening2726{Orientation="v",X=-2f,Z=7.5f,Width=2f,Type="door"});
    OfficeOpenings2726.Add(new OfficeOpening2726{Orientation="v",X=2f,Z=4f,Width=2f,Type="door"});
    OfficeOpenings2726.Add(new OfficeOpening2726{Orientation="h",X=-12f,Z=14f,Width=2f,Type="door"});
    OfficeOpenings2726.Add(new OfficeOpening2726{Orientation="v",X=2f,Z=19f,Width=2f,Type="door"});
    OfficeOpenings2726.Add(new OfficeOpening2726{Orientation="h",X=17f,Z=16f,Width=2f,Type="door"});
    OfficeOpenings2726.Add(new OfficeOpening2726{Orientation="h",X=-8f,Z=-16f,Width=2.2f,Type="hole"});
    OfficeOpenings2726.Add(new OfficeOpening2726{Orientation="h",X=-12f,Z=4f,Width=2.2f,Type="hole"});
    OfficeBlockers2726.Add(new Rect(-2f,-11.6f,4f,1.1999999999999993f));
   }
  }
  void LockOfficeLeft2741(){
   if(OfficeFloor2733!=1)return;
   var opening=OfficeOpenings2726.Find(o=>o.Orientation=="h"&&Mathf.Abs(o.X+14)<.01f&&Mathf.Abs(o.Z-14)<.01f);
   if(opening==null||opening.Door==null)return;
   OfficeLeftLockedDoor2741=opening.Door;OfficeLeftLockedDoor2741.Locked=true;OfficeLeftLockedDoor2741.Name="后勤值班室门 · 需要钥匙";
  }
  void OfficeLeftCovers2741(){
   // Design fall coordinates refer to the lower-left corner of each 2m square.
   if(OfficeFloor2733==2)OfficeFloorCovers2735(new[]{new Vector3(-14,0,7)});
   if(OfficeFloor2733==3)OfficeFloorCovers2735(new[]{new Vector3(10,0,-17)});
  }
 }
}
