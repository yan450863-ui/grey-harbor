using UnityEngine;
using System.Collections.Generic;
namespace GreyHarbor {
 public sealed class YardRoom138 {public string Name;public Rect Area;public WorldDoor Door;public Vector3 WalkPoint;}
 public sealed partial class RaidMap {
  public bool YardInterior138=>InteriorId=="yardhome"||InteriorId=="yardshed";
  public readonly List<YardRoom138> YardRooms138=new List<YardRoom138>();
  public static readonly Vector3 YardHomeApproach138=PortPoint(18,29.4f),YardShedApproach138=PortPoint(39,14.4f);
  public void BuildYardInterior138(string key){
   bool home=key=="yardhome";float halfX=home?7:5,halfZ=home?6:5;
   IsInterior=true;InteriorId=key;InteriorTitle=home?"后勤院 / 生活小屋":"后勤院 / 工具房";
   Bounds=new Rect(-halfX-.15f,-halfZ-.15f,halfX*2+.3f,halfZ*2+.3f);InitialRoom=new Rect(-1.68f,-halfZ-.08f,3.36f,halfZ*2+.16f);
   Spawn=new Vector3(0,0,-halfZ+1.15f);Root=new GameObject("YardInterior139_"+key).transform;
   Box("YardInteriorFloor",new Vector3(0,-.12f,0),new Vector3(halfX*2,.24f,halfZ*2),"77796A");
   InteriorWall(-halfX,0,.18f,halfZ*2);InteriorWall(halfX,0,.18f,halfZ*2);InteriorWall(0,-halfZ,halfX*2,.18f);InteriorWall(0,halfZ,halfX*2,.18f);
   var floor=new GroundSurfaceArt(this,139001+(home?0:1));floor.Region(new Rect(-1.49f,-halfZ+.1f,2.98f,halfZ*2-.2f),home?GroundFinish.Terrazzo:GroundFinish.Concrete);
   var k=new YardLivingKit(this);
   if(home){
    AddYardRoom138("起居室",new Rect(-7,-6,5.4f,6),-3,new Vector3(-2.9f,0,-2));
    AddYardRoom138("卧室",new Rect(-7,0,5.4f,6),3,new Vector3(-2.9f,0,2.5f));
    AddYardRoom138("厨房",new Rect(1.6f,-6,5.4f,5),-3.3f,new Vector3(3.3f,0,-2.8f));
    AddYardRoom138("生活储物间",new Rect(1.6f,-1,5.4f,3),.5f,new Vector3(3,0,.6f));
    AddYardRoom138("上锁钥匙房",new Rect(1.6f,2,5.4f,4),4,new Vector3(3,0,4),true);
    InteriorWall(-4.3f,0,5.4f,.18f);InteriorWall(4.3f,-1,5.4f,.18f);InteriorWall(4.3f,2,5.4f,.18f);
    for(int i=0;i<5;i++){var r=YardRooms138[i].Area;r.xMin+=.1f;r.yMin+=.1f;r.xMax-=.1f;r.yMax-=.1f;floor.Region(r,i==2?GroundFinish.BrokenTile:i==3?GroundFinish.PatchedConcrete:GroundFinish.Concrete);}
    PortSofa(new Vector3(-4.3f,0,-5.1f));LowFurniture("YardCoffeeTable",new Vector3(-4.3f,.29f,-3.5f),new Vector3(1.2f,.58f,.60f),"85775D");Chair(new Vector3(-6,0,-2.5f),90);
    k.Bed(new Vector3(-4.15f,0,4.5f));
    LowFurniture("KitchenCounter",new Vector3(6.4f,.43f,-3.6f),new Vector3(.9f,.86f,2.7f),"829079");
    Box("KitchenSink",new Vector3(6.4f,.896f,-3.7f),new Vector3(.64f,.025f,.70f),"3D5B4D");k.Stove(new Vector3(6.35f,0,-1.6f),90);
    LowFurniture("SmallDiningTable",new Vector3(3.8f,.37f,-5.1f),new Vector3(1.3f,.74f,.65f),"8F8163");Chair(new Vector3(4.9f,0,-5.1f),90);
    k.Washer(new Vector3(6.25f,0,-.4f),90);
    Box("StorageWallRail",new Vector3(4.2f,1.6f,1.84f),new Vector3(1.5f,.10f,.08f),"7D8064");
    foreach(float hook in new[]{3.7f,4.2f,4.7f})Box("StorageHook",new Vector3(hook,1.48f,1.78f),new Vector3(.055f,.18f,.12f),"A9A88C");
   }else{
    AddYardRoom138("园艺工具间",new Rect(-5,-5,3.4f,10),0,new Vector3(-2.8f,0,.5f));
    AddYardRoom138("洗涤间",new Rect(1.6f,-5,3.4f,5),-2.5f,new Vector3(2.8f,0,-2.1f));
    AddYardRoom138("杂物储藏间",new Rect(1.6f,0,3.4f,5),2.5f,new Vector3(2.8f,0,2.7f));
    InteriorWall(3.3f,0,3.4f,.18f);
    foreach(var room in YardRooms138){var r=room.Area;r.xMin+=.1f;r.yMin+=.1f;r.xMax-=.1f;r.yMax-=.1f;floor.Region(r,room.Name=="洗涤间"?GroundFinish.BrokenTile:GroundFinish.PatchedConcrete);}
    Box("DomesticWorkbenchTop",new Vector3(-3.2f,.86f,3.5f),new Vector3(2.45f,.14f,.85f),"918063");
    foreach(float x in new[]{-4.2f,-2.2f})foreach(float z in new[]{3.2f,3.8f})Box("DomesticWorkbenchLeg",new Vector3(x,.4f,z),new Vector3(.1f,.8f,.1f),"536352");
    var hit=Box("DomesticWorkbenchCollision",new Vector3(-3.2f,.43f,3.5f),new Vector3(2.45f,.86f,.85f),"536352",true);hit.GetComponent<Renderer>().enabled=false;SceneObstacle.Apply(hit,ObstacleKind.Low);
    YardShelf138(new Vector3(-3.3f,0,-4.5f));k.Washer(new Vector3(4.15f,0,-3.8f));k.WashStation(new Vector3(4.1f,0,-1.45f),90);YardShelf138(new Vector3(3.2f,0,4.3f));
   }
   YardDomesticContainers139(home);if(home)PopulateYardHomeKeyRoom324();
   Box("YardExitDoor",new Vector3(0,1,-halfZ+.13f),new Vector3(1.8f,2,.08f),"506B5E");
   Box("ExitHandle",new Vector3(.63f,.95f,-halfZ+.21f),new Vector3(.07f,.19f,.07f),"BCAD82");
   Box("YardEntryMat",new Vector3(0,.067f,-halfZ+.62f),new Vector3(1.95f,.025f,.72f),"4F6050");
   Doors.Add(new WorldDoor{Name="返回后勤院",Destination=key,Portal=true,Position=new Vector3(0,0,-halfZ+.30f)});
   floor.Bake("Yard_IndoorGround139");DressYardInterior138(halfX,halfZ);FinalizeHarborGround137();Physics.SyncTransforms();
  }
  void AddYardRoom138(string name,Rect r,float z,Vector3 walk,bool locked=false){
   float side=r.xMin<0?-1:1,x=side*1.6f;
   float bottom=r.yMin,openingLow=z-.9f,openingHigh=z+.9f,top=r.yMax;
   if(openingLow>bottom)InteriorWall(x,(bottom+openingLow)/2,.18f,openingLow-bottom);
   if(top>openingHigh)InteriorWall(x,(openingHigh+top)/2,.18f,top-openingHigh);
   if(InteriorId=="yardhome"&&name=="起居室"){
    // Keep the opening and room metadata, without a leaf or interactable door.
    YardRooms138.Add(new YardRoom138{Name=name,Area=r,WalkPoint=walk,Door=new WorldDoor{Name="起居室敞口",Position=new Vector3(x,0,z),RevealRoom=r,Open=true}});return;
   }
   var hinge=new GameObject("YardRoomHinge_"+name).transform;hinge.SetParent(Root,false);hinge.position=new Vector3(x,0,openingLow);
   var slab=Box(locked?"YardLockedKeyDoor":"YardRoomDoor",new Vector3(x,1.06f,z),new Vector3(.12f,2.12f,1.8f),locked?"455D52":"697E67",true);slab.transform.SetParent(hinge,true);
   var pull=Box("YardRoomPull",new Vector3(x-side*.10f,.93f,z+.68f),new Vector3(.085f,.16f,.07f),"B8A777");pull.transform.SetParent(hinge,true);
   var door=new WorldDoor{Name=name+"门",Position=new Vector3(x,0,z),Pivot=hinge,Collider=slab.GetComponent<Collider>(),OpenAngle=side*100,Locked=locked,RevealRoom=r,ClosedSafety=new Rect(x-.4f,z-1.15f,.8f,2.3f),OpenSafety=new Rect(side<0?x-2.1f:x,z-1.15f,2.1f,2.3f)};
   if(locked){
    Box("KeyDoorPadlock",new Vector3(x-.15f,.82f,z+.63f),new Vector3(.08f,.18f,.13f),"B49C64").transform.SetParent(hinge,true);
    Box("KeyDoorWarningPlate",new Vector3(x-.071f,1.58f,z),new Vector3(.021f,.22f,.45f),"A79565").transform.SetParent(hinge,true);
   }
   Doors.Add(door);YardRooms138.Add(new YardRoom138{Name=name,Area=r,Door=door,WalkPoint=walk});
  }
  void YardShelf138(Vector3 p){
   LowFurniture("YardShelfBase",p+Vector3.up*.17f,new Vector3(1.7f,.34f,.60f),"596B56");
   foreach(float x in new[]{-.8f,.8f})foreach(float z in new[]{-.25f,.25f})Box("YardShelfPost",p+new Vector3(x,.86f,z),new Vector3(.065f,1.72f,.065f),"64765C");
   foreach(float y in new[]{.5f,1.02f,1.55f})Box("YardShelfBoard",p+Vector3.up*y,new Vector3(1.75f,.065f,.65f),"8B8364");
   foreach(float x in new[]{-.55f,0,.55f})Box("StoredBox",p+new Vector3(x,1.17f,0),new Vector3(.43f,.23f,.45f),"9B9576");
  }
  void DressYardInterior138(float halfX,float halfZ){
   var root=new GameObject("YardInteriorWear138");root.transform.SetParent(Root,false);var art=new WorkshopWastelandArt(this);
   foreach(var renderer in Root.GetComponentsInChildren<MeshRenderer>())if(renderer.name=="InteriorWall"){
    var b=renderer.bounds;bool horizontal=b.size.x>b.size.z;var along=horizontal?Vector3.right:Vector3.forward;var normal=horizontal?Vector3.forward:Vector3.right;
    foreach(float side in new[]{-1f,1f})art.WallSkin(new Vector3(b.center.x,0,b.center.z)+normal*side*((horizontal?b.size.z:b.size.x)/2+.012f),along,normal*side,horizontal?b.size.x:b.size.z,"657968");
   }
   art.StripLight(new Vector3(0,1.98f,halfZ-.13f),Vector3.right,Vector3.back);
   art.BrickScar(new Vector3(-halfX+.12f,.74f,-2.1f),Vector3.forward,Vector3.right);
   foreach(var room in YardRooms138)if(!room.Door.Locked){
    var r=room.Area;float side=r.xMin<0?-1:1;float z=r.center.y;
    art.Panel("SealedWindowFrame","3F584C",new Vector3(side*(halfX-.12f),1.6f,z),Vector3.forward,Vector3.up,1.32f,.85f);
    art.Panel("OpaqueDustyGlass","718575",new Vector3(side*(halfX-.16f),1.6f,z),Vector3.forward,Vector3.up,1.16f,.66f);
    art.Tube("WindowMullion","445D4E",new Vector3(side*(halfX-.18f),1.25f,z),new Vector3(side*(halfX-.18f),1.96f,z),.02f);
    art.StripLight(new Vector3(r.center.x,1.96f,r.yMax-.15f),Vector3.right,Vector3.back);
   }
   art.Bake(root,root.AddComponent<WorkshopArtResources>());
  }
 }
}
