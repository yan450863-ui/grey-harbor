using System;
using System.Collections.Generic;
using UnityEngine;
namespace GreyHarbor {
 public sealed class OfficeRoom2726 {
  public string Id,Name;public Rect Area;public bool Stair;
  public Vector3 Center=>new Vector3(Area.center.x,0,Area.center.y);
 }
 public sealed class OfficeOpening2726 {
  public string Orientation,Type;public float X,Z,Width;public WorldDoor Door;
  public Vector3 Position=>new Vector3(X,0,Z);
 }
 public sealed partial class RaidMap {
  public bool OfficeWhitebox2726;
  public readonly List<OfficeRoom2726> OfficeRooms2726=new List<OfficeRoom2726>();
  public readonly List<OfficeOpening2726> OfficeOpenings2726=new List<OfficeOpening2726>();
  public readonly List<Rect> OfficeBlockers2726=new List<Rect>();
  public WorldDoor OfficeExit2726;
  public void BuildOfficeInterior2726(int floorNumber=1,bool leftTower=false){
   OfficeLeft2741=leftTower;
   OfficeFloor2733=Mathf.Clamp(floorNumber,1,3);
   IsInterior=OfficeWhitebox2726=true;InteriorId=(OfficeLeft2741?"officeleft":"officeright")+OfficeFloor2733;InteriorTitle="办公右塔 · "+(OfficeFloor2733==1?"一楼接待与货运办公":(OfficeFloor2733==2?"二楼财务与项目办公":"三楼管理与重要资料"));if(OfficeLeft2741)InteriorTitle="办公左塔 · "+OfficeFloor2733+"楼后勤与调度";LayoutRevision=2741;
   Bounds=new Rect(-20.4f,-24.4f,40.8f,48.8f);Spawn=new Vector3(0,0,-22.3f);InitialRoom=new Rect(-1.8f,-23.8f,3.6f,7);
   Root=new GameObject((OfficeLeft2741?"OfficeLeftFloor":"OfficeRightFloor")+OfficeFloor2733+"_2741").transform;
   var floor=Box("OfficeWhiteboxFloor2726",new Vector3(0,-.14f,0),new Vector3(40,.26f,48),"C9CEC7");
   // Support below the character capsule, independent of obstacle layers.
   floor.AddComponent<BoxCollider>();
   if(OfficeLeft2741)OfficeLeftPlan2741();else{OfficePlan2726();if(OfficeFloor2733==2)OfficeSecondPlan2733();if(OfficeFloor2733==3)OfficeThirdPlan2735();}OfficeWalls2726();ConfigureOfficeExploration2730();

   foreach(var r in OfficeRooms2726){
    Box("OfficeRoomTint2726_"+r.Id,new Vector3(r.Center.x,-.003f,r.Center.z),new Vector3(r.Area.width-.24f,.012f,r.Area.height-.24f),r.Stair?"BBCAC1":"D1D5CD");
    if(r.Stair)OfficeStair2726(r);
   }
   if(!OfficeLeft2741&&OfficeFloor2733==1){
   OfficeExit2726=new WorldDoor{Name="返回办公右塔室外",Destination="officeright1",Portal=true,Position=new Vector3(0,0,-23)};Doors.Add(OfficeExit2726);
   Box("OfficeReturnDoor2726",new Vector3(0,1.15f,-23.99f),new Vector3(3,2.3f,.08f),"9FB9B6");
   }OfficeFloorAccess2733();
   if(!OfficeLeft2741&&OfficeFloor2733==1){BuildOfficeFirstSurfaces2736(floor);BuildOfficeFirstDressing2736();BuildOfficeFirstBarrier2737();OfficeFirstWallDetails2736();}
   if(!OfficeLeft2741&&OfficeFloor2733==2){BuildOfficeFirstSurfaces2736(floor);BuildOfficeSecondDressing2738();OfficeSecondWallDetails2738();BuildOfficeSecondBarrier2738();}
   if(!OfficeLeft2741&&OfficeFloor2733==3){BuildOfficeFirstSurfaces2736(floor);BuildOfficeThirdDressing2740();BuildOfficeThirdBarrier2740();LockOfficeResearch2740();}
   if(OfficeLeft2741){BuildOfficeLeftSurfaces2742(floor);if(OfficeFloor2733==1)BuildOfficeLeftFirstDressing2742();else if(OfficeFloor2733==2)BuildOfficeLeftSecondDressing2742();else BuildOfficeLeftThirdDressing2742();BuildOfficeLeftBarrier2742();LockOfficeLeft2741();OfficeLeftCovers2741();}
   PopulateOfficeKeyRooms2760();BatchOfficeFirstFurniture2736();
   // No roof or cross-beams over playable whitebox floors.
   Physics.SyncTransforms();
  }
  void OfficePlan2726(){
   OfficeRooms2726.Add(new OfficeRoom2726{Id="S1",Name="南部主楼梯",Area=new Rect(-20f,-24f,6f,8f),Stair=true});
   OfficeRooms2726.Add(new OfficeRoom2726{Id="01",Name="安保值班室",Area=new Rect(-14f,-24f,12f,8f),Stair=false});
   OfficeRooms2726.Add(new OfficeRoom2726{Id="02",Name="接待大厅",Area=new Rect(-20f,-16f,18f,10f),Stair=false});
   OfficeRooms2726.Add(new OfficeRoom2726{Id="03",Name="货运业务大厅",Area=new Rect(2f,-24f,18f,18f),Stair=false});
   OfficeRooms2726.Add(new OfficeRoom2726{Id="04",Name="单证办公室",Area=new Rect(-20f,-3f,14f,14f),Stair=false});
   OfficeRooms2726.Add(new OfficeRoom2726{Id="05",Name="男卫生间",Area=new Rect(-6f,-3f,4f,7f),Stair=false});
   OfficeRooms2726.Add(new OfficeRoom2726{Id="06",Name="女卫生间",Area=new Rect(-6f,4f,4f,7f),Stair=false});
   OfficeRooms2726.Add(new OfficeRoom2726{Id="07",Name="会议室",Area=new Rect(2f,-3f,18f,14f),Stair=false});
   OfficeRooms2726.Add(new OfficeRoom2726{Id="08",Name="档案室",Area=new Rect(-20f,14f,12f,10f),Stair=false});
   OfficeRooms2726.Add(new OfficeRoom2726{Id="09",Name="设备杂物间",Area=new Rect(-8f,14f,6f,10f),Stair=false});
   OfficeRooms2726.Add(new OfficeRoom2726{Id="10",Name="员工休息室",Area=new Rect(2f,14f,12f,10f),Stair=false});
   OfficeRooms2726.Add(new OfficeRoom2726{Id="S2",Name="北部后勤楼梯",Area=new Rect(14f,16f,6f,8f),Stair=true});
   OfficeOpenings2726.Add(new OfficeOpening2726{Orientation="v",X=-2f,Z=-14f,Width=2f,Type="door"});
   OfficeOpenings2726.Add(new OfficeOpening2726{Orientation="v",X=-2f,Z=-8f,Width=2f,Type="door"});
   OfficeOpenings2726.Add(new OfficeOpening2726{Orientation="v",X=2f,Z=-14f,Width=2f,Type="door"});
   OfficeOpenings2726.Add(new OfficeOpening2726{Orientation="h",X=-13f,Z=-3f,Width=2f,Type="door"});
   OfficeOpenings2726.Add(new OfficeOpening2726{Orientation="h",X=-13f,Z=11f,Width=2f,Type="door"});
   OfficeOpenings2726.Add(new OfficeOpening2726{Orientation="v",X=-2f,Z=0.5f,Width=2f,Type="door"});
   OfficeOpenings2726.Add(new OfficeOpening2726{Orientation="v",X=-2f,Z=7.5f,Width=2f,Type="door"});
   OfficeOpenings2726.Add(new OfficeOpening2726{Orientation="v",X=2f,Z=7f,Width=2f,Type="door"});
   OfficeOpenings2726.Add(new OfficeOpening2726{Orientation="h",X=-5f,Z=14f,Width=2f,Type="door"});
   OfficeOpenings2726.Add(new OfficeOpening2726{Orientation="v",X=2f,Z=19f,Width=2f,Type="door"});
   OfficeOpenings2726.Add(new OfficeOpening2726{Orientation="h",X=17f,Z=16f,Width=2f,Type="door"});
   OfficeOpenings2726.Add(new OfficeOpening2726{Orientation="v",X=-14f,Z=-20f,Width=2f,Type="door"});
   OfficeOpenings2726.Add(new OfficeOpening2726{Orientation="h",X=-8f,Z=-16f,Width=2.2f,Type="hole"});
   OfficeOpenings2726.Add(new OfficeOpening2726{Orientation="v",X=-8f,Z=19f,Width=2.2f,Type="hole"});
   OfficeBlockers2726.Add(new Rect(-2f,-11.6f,4f,1.2f));
  }
  sealed class OfficeEdge2726 {public string Orientation;public float Fixed,Low,High;}
  void OfficeWalls2726(){
   var raw=new List<OfficeEdge2726>();
   Action<string,float,float,float> edge=(o,f,a,b)=>raw.Add(new OfficeEdge2726{Orientation=o,Fixed=f,Low=a,High=b});
   foreach(var r in OfficeRooms2726){edge("v",r.Area.xMin,r.Area.yMin,r.Area.yMax);edge("v",r.Area.xMax,r.Area.yMin,r.Area.yMax);edge("h",r.Area.yMin,r.Area.xMin,r.Area.xMax);edge("h",r.Area.yMax,r.Area.xMin,r.Area.xMax);}
   edge("v",-20,-24,24);edge("v",20,-24,24);edge("h",-24,-20,20);edge("h",24,-20,20);
   var grouped=new Dictionary<string,List<OfficeEdge2726>>();
   foreach(var e in raw){string key=e.Orientation+e.Fixed.ToString("R",System.Globalization.CultureInfo.InvariantCulture);if(!grouped.ContainsKey(key))grouped[key]=new List<OfficeEdge2726>();grouped[key].Add(e);}
   foreach(var group in grouped.Values){
    group.Sort((a,b)=>a.Low.CompareTo(b.Low));var merged=new List<OfficeEdge2726>();
    foreach(var e in group){if(merged.Count>0&&e.Low<=merged[merged.Count-1].High+.01f)merged[merged.Count-1].High=Mathf.Max(merged[merged.Count-1].High,e.High);else merged.Add(new OfficeEdge2726{Orientation=e.Orientation,Fixed=e.Fixed,Low=e.Low,High=e.High});}
    foreach(var e in merged){
     var cuts=new List<Vector2>();foreach(var o in OfficeOpenings2726)if(o.Orientation==e.Orientation&&Mathf.Abs((o.Orientation=="v"?o.X:o.Z)-e.Fixed)<.01f){float mid=o.Orientation=="v"?o.Z:o.X;if(mid+o.Width/2>e.Low&&mid-o.Width/2<e.High)cuts.Add(new Vector2(mid-o.Width/2,mid+o.Width/2));}
     // The outdoor portal has a visual slab, but does not leave a walk-out hole.
     cuts.Sort((a,b)=>a.x.CompareTo(b.x));float cursor=e.Low;
     foreach(var cut in cuts){float low=Mathf.Max(cut.x,e.Low),high=Mathf.Min(cut.y,e.High);if(low>cursor+.01f)OfficeWallPiece2726(e.Orientation,e.Fixed,cursor,low,0,3.2f);cursor=Mathf.Max(cursor,high);}
     if(cursor<e.High-.01f)OfficeWallPiece2726(e.Orientation,e.Fixed,cursor,e.High,0,3.2f);
    }
   }
   foreach(var o in OfficeOpenings2726){
    float mid=o.Orientation=="v"?o.Z:o.X,fix=o.Orientation=="v"?o.X:o.Z;
    if(o.Type=="hole"){OfficeBrokenWall2737(o);continue;}
    OfficeWallPiece2726(o.Orientation,fix,mid-o.Width/2,mid+o.Width/2,2.35f,3.2f,"OfficeDoorHeader2726");
    if(o.Type!="door")continue;
    var pivot=new GameObject("OfficeDoorHinge2726").transform;pivot.SetParent(Root,false);
    // Reception entrance: opposite jamb, swinging inward into the room.
    bool receptionEntrance=o.Orientation=="v"&&Mathf.Abs(o.X+2f)<.01f&&Mathf.Abs(o.Z+14f)<.01f;
    pivot.position=o.Position+(o.Orientation=="v"?(receptionEntrance?Vector3.forward:Vector3.back):Vector3.left)*o.Width/2;
    var size=o.Orientation=="v"?new Vector3(.10f,2.3f,o.Width):new Vector3(o.Width,2.3f,.10f);
    var slab=Box("OfficeRoomDoor2726",o.Position+Vector3.up*1.15f,size,"AABDB6",true);slab.transform.SetParent(pivot,true);
    var safety=o.Orientation=="v"?new Rect(o.X-.07f,o.Z-o.Width/2,.14f,o.Width):new Rect(o.X-o.Width/2,o.Z-.07f,o.Width,.14f);
    var adjacent=OfficeRooms2726.FindAll(r=>o.X>=r.Area.xMin-.01f&&o.X<=r.Area.xMax+.01f&&o.Z>=r.Area.yMin-.01f&&o.Z<=r.Area.yMax+.01f);
    // Select the room beyond this doorway; shared stair doors swing into the occupied room.
    var targetRoom=adjacent.Find(r=>!r.Stair)??(adjacent.Count>0?adjacent[0]:null);
    float inward=targetRoom==null?1f:Mathf.Sign(o.Orientation=="v"?targetRoom.Center.x-o.X:targetRoom.Center.z-o.Z);
    float openAngle=o.Orientation=="v"?90f*inward*(receptionEntrance?-1f:1f):-90f*inward;
    o.Door=new WorldDoor{Name=(adjacent.Count>0?adjacent[0].Name:"走廊")+"门",Destination=InteriorId,Position=o.Position,Pivot=pivot,Collider=slab.GetComponent<Collider>(),OpenAngle=openAngle,InstantToggle=true,ClosedSafety=safety,OpenSafety=safety,RevealRoom=adjacent.Count>0?adjacent[0].Area:InitialRoom};
    foreach(var r in adjacent)o.Door.ExtraRevealRooms.Add(r.Area);Doors.Add(o.Door);
   }
  }
  void OfficeWallPiece2726(string o,float fix,float low,float high,float bottom,float top,string name="OfficeWall2726"){
   var p=o=="v"?new Vector3(fix,(bottom+top)/2,(low+high)/2):new Vector3((low+high)/2,(bottom+top)/2,fix);
   var s=o=="v"?new Vector3(.20f,top-bottom,high-low):new Vector3(high-low,top-bottom,.20f);
   Box(name,p,s,"DCE0D3",true);
  }
  void OfficeStair2726(OfficeRoom2726 r){
   float x=r.Area.xMin+1.4f,z=r.Area.yMin+1.4f;
   for(int i=0;i<10;i++){float h=(i+1)*.22f;Box("OfficeReservedStair2726",new Vector3(x,h/2,z+i*.34f),new Vector3(2.3f,h,.34f),"A4B4AB",true);}
   Box("OfficeUpperFloorBarrier2726",new Vector3(x,2.45f,z+3.5f),new Vector3(2.3f,1.2f,.20f),"B89576",true);
  }
 }
}
