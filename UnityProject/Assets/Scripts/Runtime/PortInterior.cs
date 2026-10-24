using UnityEngine;
namespace GreyHarbor {
 public sealed partial class RaidMap {
  public static readonly string[] PortRoomNames={"接待室","员工休息室","储物间","洗手间"};
  public static readonly Rect[] PortRooms={new Rect(-9,-7,7,7),new Rect(2,-7,7,7),new Rect(-9,0,7,7),new Rect(2,0,7,7)};
  public void BuildPortInterior(string key){
   bool booth=key=="portbooth";IsInterior=true;InteriorId=key;InteriorTitle=booth?"港口 / 值班室":"港口 / 附属房";
   float w=booth?8:18,d=booth?7:14;
   Bounds=new Rect(-w/2-.2f,-d/2-.2f,w+.4f,d+.4f);InitialRoom=booth?Bounds:new Rect(-2.08f,-7.1f,4.16f,14.2f);
   Spawn=new Vector3(0,0,-d/2+1.25f);Root=new GameObject("Port_Interior_"+key).transform;
   Box("PortInteriorFloor",new Vector3(0,-.12f,0),new Vector3(w,.24f,d),"77796A");
   if(booth)InteriorWall(-w/2,0,.18f,d);else{InteriorWall(-9,-2.7f,.18f,8.6f);InteriorWall(-9,5.1f,.18f,3.8f);Box("PortStorageDoorHeader2760",new Vector3(-9,2.275f,2.4f),new Vector3(.18f,.25f,1.6f),"777D6D",true);}InteriorWall(w/2,0,.18f,d);InteriorWall(0,-d/2,w,.18f);InteriorWall(0,d/2,w,.18f);
   var floor=new GroundSurfaceArt(this,131031);
   if(booth){floor.Region(new Rect(-3.9f,-3.4f,7.8f,6.8f),GroundFinish.BrokenTile);PortDesk(new Vector3(-1.5f,0,1.9f),true);Chair(new Vector3(-1.5f,0,.8f),0);Chair(new Vector3(2.5f,0,-.7f),90);NoticeBoard114(new Vector3(1.8f,1.45f,3.34f));PortAnchor("FutureLoot_Desk",new Vector3(-2.3f,.91f,1.9f));}
   else{
    foreach(float x in new[]{-2f,2f}){float start=-7;foreach(float z in new[]{-3.5f,3.5f}){float end=z-.9f;InteriorWall(x,(start+end)/2,.18f,end-start);start=z+.9f;}InteriorWall(x,(start+7)/2,.18f,7-start);}
    InteriorWall(-5.5f,0,7,.18f);InteriorWall(5.5f,0,7,.18f);
    for(int i=0;i<4;i++){PortRoomDoor(i);var r=PortRooms[i];r.xMin+=.1f;r.yMin+=.1f;r.width-=.2f;r.height-=.2f;floor.Region(r,i==2?GroundFinish.PatchedConcrete:i==0?GroundFinish.Terrazzo:GroundFinish.BrokenTile);}
    floor.Region(new Rect(-1.9f,-6.9f,3.8f,13.8f),GroundFinish.Terrazzo);
    PortDesk(new Vector3(-5.6f,0,-5.6f),true);Chair(new Vector3(-5.6f,0,-4.5f),180);Chair(new Vector3(-7.6f,0,-2.1f),90);Chair(new Vector3(-7.6f,0,-3.1f),90);
    NoticeBoard114(new Vector3(-5.4f,1.45f,-6.84f));PortAnchor("FutureLoot_ReceptionDesk",new Vector3(-6.4f,.91f,-5.6f));
    PortSofa(new Vector3(5.5f,0,-5.9f));LowFurniture("RestCoffeeTable",new Vector3(5.5f,.3f,-3.8f),new Vector3(1.8f,.6f,.85f),"80765C");Chair(new Vector3(7.8f,0,-3.7f),-90);PortAnchor("FutureLoot_Sofa",new Vector3(5,.6f,-5.9f));
    DecorShelf114(new Vector3(-7.9f,0,4.9f));DecorShelf114(new Vector3(-4.1f,0,5.6f));PortDesk(new Vector3(-5.6f,0,1.2f),false);PortAnchor("FutureLoot_StorageBench",new Vector3(-5.6f,.91f,1.2f));
    LowFurniture("WashCounter",new Vector3(7.8f,.43f,2.4f),new Vector3(1.1f,.86f,2.4f),"999D8E");
    for(int i=0;i<2;i++){Box("SinkRim",new Vector3(7.8f,.9f,1.85f+i*1.1f),new Vector3(.8f,.1f,.8f),"B7B9A6");Box("SinkBasin",new Vector3(7.8f,.958f,1.85f+i*1.1f),new Vector3(.58f,.025f,.57f),"677F78");Box("Tap",new Vector3(8.15f,1.09f,1.85f+i*1.1f),new Vector3(.07f,.25f,.07f),"909C91");}
    LowFurniture("ToiletBase",new Vector3(5,.26f,5.8f),new Vector3(.7f,.52f,.9f),"B1B3A1");Box("ToiletSeat",new Vector3(5,.57f,5.6f),new Vector3(.8f,.1f,.82f),"C1C1AB");Box("Cistern",new Vector3(5,.78f,6.2f),new Vector3(.8f,.72f,.35f),"ABB3A3");
    Box("PrivacyScreen",new Vector3(6.2f,.95f,5.6f),new Vector3(.12f,1.9f,2),"6B8279",true);PortAnchor("FutureLoot_WallCoat",new Vector3(-8.8f,1.2f,6.2f));
   }
   Box("PortExitDoor",new Vector3(0,1,-d/2+.14f),new Vector3(1.5f,2,.08f),"4D6C61");Box("ExitPull",new Vector3(.5f,.95f,-d/2+.21f),new Vector3(.07f,.2f,.08f),"BDA77B");
   Box("EntryMat",new Vector3(0,.02f,-d/2+.7f),new Vector3(1.8f,.03f,.8f),"43534B");
   Doors.Add(new WorldDoor{Name="返回港口室外",Portal=true,Destination=key,Position=new Vector3(0,0,-d/2+.35f)});
   if(!booth){AddPortLockedRoom();floor.Region(PortLockedRoomBounds,GroundFinish.PatchedConcrete);PopulatePortKeyRoom2760();}PortInteriorContainers(booth);floor.Bake();DressPortInterior();Physics.SyncTransforms();
  }
  void PortAnchor(string name,Vector3 position){var a=new GameObject(name);a.transform.SetParent(Root,false);a.transform.position=position;}
  void PortDesk(Vector3 p,bool radio){
   LowFurniture("PortDesk",p+Vector3.up*.42f,new Vector3(2.8f,.84f,.95f),"59665C");Box("DeskTop",p+Vector3.up*.88f,new Vector3(2.95f,.09f,1.06f),"978970");
   for(int i=0;i<3;i++)Box("DeskDrawer",p+new Vector3(-.9f,.2f+i*.2f,-.49f),new Vector3(.6f,.16f,.03f),"798471");
   Box("PaperStack",p+new Vector3(-.6f,.95f,0),new Vector3(.42f,.06f,.56f),"B7B199");
   if(radio){Box("RadioBody",p+new Vector3(.7f,1.05f,0),new Vector3(.65f,.29f,.4f),"354B45");Box("RadioDisplay",p+new Vector3(.7f,1.09f,-.21f),new Vector3(.26f,.09f,.025f),"9EAF97");Box("RadioAntenna",p+new Vector3(.94f,1.43f,.09f),new Vector3(.025f,.6f,.025f),"3B453E");}
  }
  void PortSofa(Vector3 p){LowFurniture("WaitingSofa",p+Vector3.up*.28f,new Vector3(3,.56f,1.1f),"657565");Box("SofaBack",p+new Vector3(0,.78f,-.5f),new Vector3(3,.65f,.18f),"77816B");foreach(float x in new[]{-1.4f,1.4f})Box("SofaArm",p+new Vector3(x,.6f,0),new Vector3(.2f,.5f,1.1f),"69785F");foreach(float x in new[]{-.85f,0,.85f})Box("SofaCushion",p+new Vector3(x,.6f,0),new Vector3(.8f,.12f,.86f),"83866F");}
  void PortRoomDoor(int i){
   float side=i%2==0?-1:1,x=side*2,z=i<2?-3.5f:3.5f;var hinge=new GameObject("PortRoomDoorHinge_"+i).transform;hinge.SetParent(Root,false);hinge.position=new Vector3(x,0,z-.9f);
   var slab=Box("PortRoomDoor_"+i,new Vector3(x,1.075f,z),new Vector3(.12f,2.15f,1.8f),"667B68",true);slab.transform.SetParent(hinge,true);
   var pull=Box("DoorPull",new Vector3(x-side*.11f,.93f,z+.65f),new Vector3(.1f,.18f,.06f),"BCAD81");pull.transform.SetParent(hinge,true);
   Doors.Add(new WorldDoor{Name=PortRoomNames[i]+"门",Position=new Vector3(x,0,z),Pivot=hinge,Collider=slab.GetComponent<Collider>(),OpenAngle=side*100,RevealRoom=PortRooms[i],ClosedSafety=new Rect(x-.48f,z-1.3f,.96f,2.6f),OpenSafety=new Rect(side<0?x-2.4f:x,z-1.25f,2.4f,2.5f)});
  }
  void DressPortInterior(){
   var root=new GameObject("PortInteriorWear");root.transform.SetParent(Root,false);var life=root.AddComponent<WorkshopArtResources>();var art=new WorkshopWastelandArt(this);
   foreach(var r in Root.GetComponentsInChildren<MeshRenderer>())if(r.name=="InteriorWall"){
    var b=r.bounds;bool horizontal=b.size.x>b.size.z;var along=horizontal?Vector3.right:Vector3.forward;var normal=horizontal?Vector3.forward:Vector3.right;float length=horizontal?b.size.x:b.size.z,depth=horizontal?b.size.z:b.size.x;
    foreach(float side in new[]{-1f,1f})art.WallSkin(new Vector3(b.center.x,0,b.center.z)+normal*side*(depth/2+.012f),along,normal*side,length);
   }
   float north=InteriorId=="portbooth"?3.35f:6.85f;
   art.StripLight(new Vector3(0,1.95f,north),Vector3.right,Vector3.back);
   art.BrickScar(new Vector3(InteriorId=="portbooth"?-2.8f:-7,.8f,north),Vector3.right,Vector3.back);
   if(InteriorId=="portbooth"){
    art.Cube("SealedWindow","3E534A",new Vector3(-3.83f,1.57f,-.4f),new Vector3(.1f,.9f,2.3f));
    art.Panel("DustyGlass","788A7B",new Vector3(-3.76f,1.57f,-.4f),Vector3.forward,Vector3.up,2.1f,.7f);
    for(int i=0;i<3;i++)art.Panel("WindowBars","40554B",new Vector3(-3.73f,1.57f,-1.35f+i*.95f),Vector3.forward,Vector3.up,.05f,.8f);
   }else{
    art.Panel("WashMirror","84998D",new Vector3(8.84f,1.48f,2.4f),Vector3.forward,Vector3.up,2.3f,.8f);
    for(int i=0;i<4;i++){float x=i%2==0?-1.88f:1.88f,z=i<2?-3.5f:3.5f;art.Panel("RoomPlaque","B2AC8A",new Vector3(x,1.5f,z+1.2f),Vector3.forward,Vector3.up,.42f,.25f);}
    art.StripLight(new Vector3(-5.5f,1.95f,-6.8f),Vector3.right,Vector3.forward);art.StripLight(new Vector3(5.5f,1.95f,-6.8f),Vector3.right,Vector3.forward);
   }
   art.Bake(root,life);
  }
 }
}
