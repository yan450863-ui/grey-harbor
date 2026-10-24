using System.Collections.Generic;
using UnityEngine;
namespace GreyHarbor {
 public sealed class FreightRoom2716 {public string Name;public Rect Area;public WorldDoor Door;public Vector3 WalkPoint;}
 public sealed partial class RaidMap {
  public bool FreightInterior2716;
  public float FreightCeilingHeight2716;
  public Rect FreightLobby2716;
  public WorldDoor FreightExit2716;
  public readonly List<FreightRoom2716> FreightRooms2716=new List<FreightRoom2716>();
  public readonly List<Rect> FreightInitialAreas2716=new List<Rect>();
  public static bool IsFreightInteriorKey2716(string key)=>key=="freightduty"||key=="freightwarehouse";
  public void BuildFreightInterior2716(string key){
   if(!IsFreightInteriorKey2716(key))throw new System.ArgumentException("Unknown freight interior",nameof(key));
   bool duty=key=="freightduty";float width=duty?17:18,depth=17;
   IsInterior=FreightInterior2716=true;InteriorId=key;InteriorTitle=duty?"货运值班房 / 办公与生活":"货运库房 / 仓储与收发";FreightCeilingHeight2716=duty?3:4.2f;
   Bounds=new Rect(-width/2-.2f,-depth/2-.2f,width+.4f,depth+.4f);Spawn=new Vector3(0,0,-6.95f);Root=new GameObject("FreightInterior2717_"+key).transform;
   Box("FreightInteriorFloor2717",new Vector3(0,-.12f,0),new Vector3(width,.24f,depth),"727C6E");
   FreightWall2716(-width/2+.09f,0,.18f,depth);FreightWall2716(width/2-.09f,0,.18f,depth);FreightWall2716(0,-depth/2+.09f,width,.18f);FreightWall2716(0,depth/2-.09f,width,.18f);
   if(duty){
    FreightLobby2716=new Rect(-1.4f,-8.32f,2.8f,16.64f);InitialRoom=FreightLobby2716;FreightInitialAreas2716.Add(InitialRoom);
    FreightAddRoom2716("值班办公室",new Rect(-8.32f,-8.32f,7.02f,8.32f),-1.3f,-3.8f,-1);
    FreightAddRoom2716("员工休息区",new Rect(1.3f,-8.32f,7.02f,8.32f),1.3f,-3.8f,1);
    FreightAddRoom2716("货运资料室",new Rect(-8.32f,0,7.02f,8.32f),-1.3f,3.8f,-1);
    FreightAddRoom2716("卫生间",new Rect(1.3f,0,7.02f,8.32f),1.3f,3.8f,1);
    FreightWall2716(-4.81f,0,7.02f,.18f);FreightWall2716(4.81f,0,7.02f,.18f);
   }else{
    // The warehouse hall is an L around one small receiving room. Two initial
    // reveal rectangles cover the hall, without exposing the unopened office.
    FreightLobby2716=new Rect(-3.19f,-8.32f,12.01f,16.64f);InitialRoom=FreightLobby2716;FreightInitialAreas2716.Add(InitialRoom);FreightInitialAreas2716.Add(new Rect(-8.82f,-8.32f,5.72f,10.62f));
    FreightAddRoom2716("货物收发室",new Rect(-8.82f,2.3f,5.72f,6.02f),-3.1f,5.1f,-1);
    FreightWall2716(-5.96f,2.3f,5.72f,.18f);
    // No ceiling or spanning roof beams: the high warehouse remains readable.
   }
   FreightBakeFloor2717(width,depth);
   // The return portal stands inside the shell with full capsule clearance.
   Box("FreightReturnDoor2716",new Vector3(0,1.2f,-depth/2+.24f),new Vector3(duty?1.8f:2.4f,2.4f,.08f),"627C70");
   Box("FreightReturnPull2716",new Vector3(duty ? .55f : 0,.95f,-depth/2+.30f),new Vector3(.30f,.08f,.065f),"CFD1C5");
   FreightExit2716=new WorldDoor{Name=duty?"返回货运值班房室外":"返回货运库房室外",Destination=key,Portal=true,Position=new Vector3(0,0,-depth/2+.60f)};Doors.Add(FreightExit2716);
   BuildFreightDressing2717();FinishFreightWalls2718();FinalizeHarborGround137();Physics.SyncTransforms();
  }
  void FreightWall2716(float x,float z,float width,float depth){Box("FreightWhiteboxWall2716",new Vector3(x,FreightCeilingHeight2716/2,z),new Vector3(width,FreightCeilingHeight2716,depth),"CBCAB7",true);}
  void FreightAddRoom2716(string name,Rect area,float x,float z,int side){
   const float halfDoor=.8f;float low=z-halfDoor,high=z+halfDoor;
   if(low>area.yMin)FreightWall2716(x,(area.yMin+low)/2,.18f,low-area.yMin);
   if(high<area.yMax)FreightWall2716(x,(high+area.yMax)/2,.18f,area.yMax-high);
   FreightDoorHeader2719(name,x,z,halfDoor*2);
   var hinge=new GameObject("FreightRoomHinge2716_"+name).transform;hinge.SetParent(Root,false);hinge.position=new Vector3(x,0,low);
   var slab=Box("FreightRoomDoor2716",new Vector3(x,1.075f,z),new Vector3(.12f,2.15f,halfDoor*2),"A7B3AA",true);slab.transform.SetParent(hinge,true);
   Box("FreightDoorPull2716",new Vector3(x-side*.09f,.95f,z+.58f),new Vector3(.075f,.18f,.065f),"D1D2C5").transform.SetParent(hinge,true);
   var door=new WorldDoor{Name=name+"门",Destination=InteriorId,Position=new Vector3(x,0,z),Pivot=hinge,Collider=slab.GetComponent<Collider>(),OpenAngle=side*95,RevealRoom=area,ClosedSafety=new Rect(x-.45f,z-1.1f,.90f,2.2f),OpenSafety=new Rect(side<0?x-1.9f:x-.1f,z-1.1f,2,1.4f)};
   Doors.Add(door);FreightRooms2716.Add(new FreightRoom2716{Name=name,Area=area,Door=door,WalkPoint=new Vector3(area.center.x,0,area.center.y)});
  }
  void FreightBakeFloor2716(float width,float depth){
   // Reuse the single stable world-fog ground plane directly. Whitebox floors
   // remain flat and empty, with no surface meshes competing for depth.
   FreightFloorRegion2716(new Rect(-width/2,-depth/2,width,depth),"BAC3BC");
   foreach(var room in FreightRooms2716){Rect r=room.Area;r.xMin+=.10f;r.xMax-=.10f;r.yMin+=.10f;r.yMax-=.10f;FreightFloorRegion2716(r,room.Name=="卫生间"?"D5D7CD":room.Name=="员工休息区"?"C3C8BA":"C8CDC6");}
  }
  void FreightFloorRegion2716(Rect area,string color){RegisterGround137(new List<Vector3>{new Vector3(area.xMin,.05f,area.yMin),new Vector3(area.xMin,.05f,area.yMax),new Vector3(area.xMax,.05f,area.yMax),new Vector3(area.xMax,.05f,area.yMin)},new List<int>{0,1,2,0,2,3},PixelArt.Hex(color));}
 }
 public sealed partial class RaidUI {
  void FreightGuide2716(){
   string name=G.Map.InteriorId=="freightduty"?"值班房公共走廊":"库房高顶通间";var p=new Vector2(G.Player.transform.position.x,G.Player.transform.position.z);
   foreach(var room in G.Map.FreightRooms2716)if(room.Area.Contains(p)){name=room.Name;break;}
   Fill(new Rect(28,109,353,76),panel);Label(48,118,320,24,name,18,mint);Label(48,145,320,20,"F开关房门 · 长按F返回室外",13,muted);
  }
 }
}

