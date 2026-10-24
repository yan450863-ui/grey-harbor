using UnityEngine;
using System.Collections.Generic;
namespace GreyHarbor {
 public sealed class FerryRoom143 {public string Name;public Rect Area;public WorldDoor Door;public readonly List<Rect> ExtraAreas=new List<Rect>();public bool Contains(Vector2 p){if(Area.Contains(p))return true;foreach(var area in ExtraAreas)if(area.Contains(p))return true;return false;}}
 public sealed partial class RaidMap {
  public bool FerryWhitebox143;public readonly List<FerryRoom143> FerryRooms143=new List<FerryRoom143>();
  public static readonly Rect FerryLobby143=new Rect(-7.10f,-2,14.2f,4);
  public static Vector3 FerryStairArrival143(bool front)=>front?new Vector3(2.8f,0,11.15f):new Vector3(0,0,-12.0f);
  public void BuildFerryInterior143(string key){
   FerryWhitebox143=IsInterior=true;InteriorId=key;InteriorTitle=key=="ferry1"?"客船 · 一楼":"客船 · 二楼";Bounds=new Rect(-7.3f,-15.1f,14.6f,30.2f);InitialRoom=key=="ferry1"?new Rect(-1.42f,-14.8f,2.84f,16.8f):FerryLobby143;Spawn=new Vector3(-5.7f,0,0);Root=new GameObject(key=="ferry1"?"PassengerFerry_Floor1_144":"PassengerFerry_Floor2_144").transform;
   Box("FerryFloor",new Vector3(0,-.10f,0),new Vector3(14.4f,.20f,30),"A5AFAB");
   InteriorWall(-7.2f,0,.18f,30);InteriorWall(7.2f,0,.18f,30);InteriorWall(0,-15,14.4f,.18f);InteriorWall(0,15,14.4f,.18f);
   if(key=="ferry1"){
    FerryVerticalPartition143(-1.5f,-14.9f,-2,new[]{-11f,-4f});FerryVerticalPartition143(1.5f,-14.9f,-2,new[]{-11f,-5f});
    InteriorWall(-4.35f,-7,5.7f,.16f);InteriorWall(4.35f,-8,5.7f,.16f);
    // Close side rooms against the boarding lobby; their corridor doors are the only entrances.
    InteriorWall(-4.35f,-2,5.7f,.16f);InteriorWall(4.35f,-2,5.7f,.16f);FerryHorizontalPartition143(2);
    FerryAddRoom143("行李舱",new Rect(-7.1f,-14.8f,5.50f,7.7f),new Vector3(-1.5f,0,-11),true);
    FerryAddRoom143("备品储藏室",new Rect(-7.1f,-6.9f,5.50f,4.8f),new Vector3(-1.5f,0,-4),true);
    FerryAddRoom143("卫生间",new Rect(1.60f,-14.8f,5.50f,6.7f),new Vector3(1.5f,0,-11),true);
    FerryAddRoom143("服务间",new Rect(1.60f,-7.9f,5.50f,5.8f),new Vector3(1.5f,0,-5),true);
    FerryAddRoom143("一楼客舱",new Rect(-7.1f,2.1f,14.20f,12.7f),new Vector3(0,0,2),false);
    FerryExit143();
   }else{
    FerryHorizontalPartition143(-2);FerryHorizontalPartition143(2);FerryHorizontalPartition143(10);
    FerryAddRoom143("驾驶室",new Rect(-7.1f,10.1f,14.2f,4.7f),new Vector3(0,0,10),false);
    // Front stair lands inside the bridge; opening its door also reveals the adjoining passenger room.
    FerryRooms143[0].Door.RevealRoom=new Rect(-7.1f,2.1f,14.2f,12.7f);
    FerryAddRoom143("二楼客舱",new Rect(-7.1f,2.1f,14.2f,7.8f),new Vector3(0,0,2),false);
    FerryAddRoom143("二楼观景休息区",new Rect(-2.9f,-14.8f,10f,12.7f),new Vector3(0,0,-2),false);
    var lounge=FerryRooms143[FerryRooms143.Count-1];var extra=new Rect(-7.1f,-8.9f,4.1f,6.8f);lounge.ExtraAreas.Add(extra);lounge.Door.ExtraRevealRooms.Add(extra);
    FerryVerticalPartition143(-3,-14.9f,-9,new[]{-11.4f});InteriorWall(-5.1f,-9,4.2f,.16f);
    FerryAddRoom143("上锁贵宾舱",new Rect(-7.1f,-14.8f,4.0f,5.7f),new Vector3(-3,0,-11.4f),true);var locked=FerryRooms143[FerryRooms143.Count-1].Door;locked.Locked=true;
    Box("FerryKeyDoorPadlock",new Vector3(-2.90f,.96f,-10.80f),new Vector3(.16f,.23f,.16f),"B39A66").transform.SetParent(locked.Pivot,true);Box("FerryKeyDoorPlate",new Vector3(-2.935f,1.53f,-11.4f),new Vector3(.025f,.27f,.56f),"AA986B").transform.SetParent(locked.Pivot,true);
   }
   FerryStairs143(false,key);FerryStairs143(true,key);
   DressFerryInterior144();PopulateFerryContainers146();PopulateFerryKeyRoom2760();Physics.SyncTransforms();
  }
  void FerryBlock143(string id,float x,float z,float w,float h,float d){var b=Box(id,new Vector3(x,h/2,z),new Vector3(w,h,d),"ADB6B0",true);SceneObstacle.Apply(b,h>1.35f?ObstacleKind.High:ObstacleKind.Low);}
  void FerrySeats143(float first,float last){for(float z=first;z<=last;z+=2.5f)foreach(float x in new[]{-2.7f,2.7f}){FerryBlock143("WhiteboxSeatRow",x,z,1.7f,.5f,.8f);Box("SeatBack",new Vector3(x,.79f,z+.34f),new Vector3(1.7f,.58f,.12f),"919FA1");}}
  void FerryVerticalPartition143(float x,float start,float end,float[] doors){float cursor=start;foreach(float center in doors){float a=center-.95f;if(a>cursor)InteriorWall(x,(cursor+a)/2,.16f,a-cursor);cursor=center+.95f;}if(cursor<end)InteriorWall(x,(cursor+end)/2,.16f,end-cursor);}
  void FerryHorizontalPartition143(float z){InteriorWall(-4.07f,z,6.05f,.16f);InteriorWall(4.07f,z,6.05f,.16f);}
  void FerryAddRoom143(string name,Rect room,Vector3 position,bool vertical){
   float width=vertical?1.90f:2.09f;var pivot=new GameObject("FerryRoomDoorHinge").transform;pivot.SetParent(Root,false);pivot.position=position+(vertical?Vector3.back:Vector3.left)*width/2;
   var slab=Box("FerryRoomDoor",position+Vector3.up*1.05f,vertical?new Vector3(.10f,2.1f,width):new Vector3(width,2.1f,.10f),"7D9097",true);slab.transform.SetParent(pivot,true);
   var door=new WorldDoor{Name=name,OpenAngle=vertical&&position.x<0?-105:105,Position=position,Pivot=pivot,Collider=slab.GetComponent<Collider>(),RevealRoom=room,ClosedSafety=vertical?new Rect(position.x-.45f,position.z-width/2-.15f,.9f,width+.3f):new Rect(position.x-width/2-.15f,position.z-.45f,width+.3f,.9f),OpenSafety=vertical?new Rect(position.x<0?position.x-width-.18f:position.x,position.z-width/2-.18f,width+.18f,width+.36f):new Rect(position.x-width/2-.18f,position.z-width-.18f,width+.36f,width+.18f)};
   // Use the actual opened slab footprint, rather than the full swing square, for safe interaction.
   var rotation=Quaternion.Euler(0,door.OpenAngle,0);Vector3 along=rotation*(vertical?Vector3.forward:Vector3.right)*width;Vector3 cross=rotation*(vertical?Vector3.right:Vector3.forward)*.10f;Vector3 center=pivot.position+along*.5f;float halfX=(Mathf.Abs(along.x)+Mathf.Abs(cross.x))*.5f+.32f,halfZ=(Mathf.Abs(along.z)+Mathf.Abs(cross.z))*.5f+.32f;door.OpenSafety=new Rect(center.x-halfX,center.z-halfZ,halfX*2,halfZ*2);
   Doors.Add(door);FerryRooms143.Add(new FerryRoom143{Name=name,Area=room,Door=door});
  }
  void FerryStairs143(bool front,string key){var p=front?new Vector3(2.8f,0,12.3f):new Vector3(0,0,-13.2f);for(int i=0;i<5;i++){var step=p+(front?Vector3.forward:Vector3.back)*(.45f+i*.27f);float h=.12f+i*.13f;var block=Box("FerryStairStep",step+Vector3.up*h/2,new Vector3(1.45f,h,.28f),"D1D6CE",true);SceneObstacle.Apply(block,ObstacleKind.Low);}Doors.Add(new WorldDoor{Name=(front?"前部楼梯 · ":"后部楼梯 · ")+(key=="ferry1"?"上二楼":"下一楼"),Portal=true,Destination="ferry-floor:"+(key=="ferry1"?"2":"1")+":"+(front?"front":"rear"),Position=p});}
  void FerryExit143(){Box("FerryExitDoor",new Vector3(-7.08f,1.15f,0),new Vector3(.05f,2.3f,1.6f),"536E79");Doors.Add(new WorldDoor{Name="下船 · 返回中部码头",Portal=true,Destination="ferry-out",Position=new Vector3(-6.65f,0,0)});}
 }
 public sealed partial class RaidUI {
  void FerryGuide143(){Fill(new Rect(28,109,353,74),panel);string area="登船大厅";Vector2 p=new Vector2(G.Player.transform.position.x,G.Player.transform.position.z);foreach(var room in G.Map.FerryRooms143)if(room.Contains(p)){area=room.Name;break;}Label(48,119,320,23,area,18,mint);Label(48,147,320,24,"废弃客轮 · 前后楼梯长按F两秒",13,muted);}
 }
}
