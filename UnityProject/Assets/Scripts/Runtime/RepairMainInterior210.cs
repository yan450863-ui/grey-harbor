using UnityEngine;
using System.Collections.Generic;
namespace GreyHarbor {
 // Layout records contain only geometry and door state; no actors or searchable stock.
 public sealed class RepairMainRoom210 {
  public string Name; public Rect Area; public WorldDoor Door;
  public Vector3 WalkPoint,ApproachPoint; public bool ReservedStair;
 }
 public sealed partial class RaidMap {
  public static Rect RepairMainNet210=>MainNet276;
  public static Rect RepairMainHall210=>MainHall276;
  public static readonly Rect RepairMainInspection210=MainRect273(new Rect(-3.8f,-7.2f,7.6f,16.2f));
  public readonly List<RepairMainRoom210> RepairMainRooms210=new List<RepairMainRoom210>();
  public readonly List<Vector3> RepairMainWalkPoints210=new List<Vector3>();
  public void BuildRepairMain210(){
   IsInterior=true;RepairMain220=true;RepairFloor220=1;InteriorId="repairmain1";InteriorTitle="维修主楼 · 一楼维修大厅";LayoutRevision=276;
   Bounds=MainBounds276;InitialRoom=RepairMainHall210;Spawn=MainPoint273(new Vector3(8.5f,0,-14.5f));
   Root=new GameObject("RepairMain_Floor1_210").transform;
   RepairHallAreas220.AddRange(RepairMainPublicAreas276(1));
   MainShell276("RepairMainFloor210");
   RepairMainRoomAdd210("零件整理与待修件间",new Rect(-13.29f,-15.79f,4.49f,23.59f),-8.8f,-10.6f,-1,false);
   RepairMainRoomAdd210("西北楼梯间",new Rect(-13.29f,7.8f,4.49f,7.99f),-8.8f,10.5f,-1,true);
   RepairMainRoomAdd210("东南楼梯间",new Rect(9.8f,-11.5f,3.49f,6),9.8f,-8.5f,1,true);
   RepairMainRoomAdd210("维修值班办公室",new Rect(9.8f,-5.5f,3.49f,9),9.8f,-1,1,false);
   RepairMainRoomAdd210("设备与配电间",new Rect(9.8f,3.5f,3.49f,12.29f),9.8f,8,1,false);
   var floors=new GroundSurfaceArt(this,210031);
   floors.Region(MainNet276,GroundFinish.PatchedConcrete,.11f);
   foreach(var room in RepairMainRooms210){var r=room.Area;r.xMin+=.12f;r.xMax-=.12f;r.yMin+=.12f;r.yMax-=.12f;floors.Region(r,room.Name=="维修值班办公室"?GroundFinish.BrokenTile:room.ReservedStair?GroundFinish.Concrete:GroundFinish.PatchedConcrete,.11f);}
   floors.Bake("RepairMain_Ground210");
   RepairMainFurnish210();RepairMainDress210();RepairMainExit210();AddRepairStairs220(1);
   foreach(var p in new[]{Spawn,new Vector3(8,0,-16),new Vector3(8,0,-6),new Vector3(8,0,6),new Vector3(8,0,16),new Vector3(0,0,16),new Vector3(-6,0,16),new Vector3(-6,0,-7),new Vector3(-6,0,-16),new Vector3(0,0,-16),Vector3.zero})RepairMainWalkPoints210.Add(p);
   // Walls render normally; no player-centred cutaway.
   PlaceRepairMainContainers240();PrepareMainPatrol276();AddMainDecoration274();FinishRepairWalls220();FinalizeHarborGround137();Physics.SyncTransforms();
  }
  void RepairMainWall210(float x,float z,float w,float d){Box("InteriorWall",new Vector3(x,1.6f,z),new Vector3(w,3.2f,d),"929580",true);}
  void RepairMainRoomAdd210(string name,Rect area,float x,float z,int side,bool stair){
   Rect planned;Vector3 position;int plannedSide;
   if(TryMainRoomPlan276(RepairFloor220,name,out planned,out position,out plannedSide)){area=planned;x=position.x;z=position.z;side=plannedSide;}
   else {area=MainRect273(area);position=MainPoint273(new Vector3(x,0,z));x=position.x;z=position.z;}
   float opposite=side<0?area.xMin:area.xMax;
   if(Mathf.Abs(opposite)<MainNetHalfX276-.01f)MainRoomEdge276(opposite,area.center.y,.20f,area.height);
   foreach(float end in new[]{area.yMin,area.yMax})if(Mathf.Abs(end)<MainNetHalfZ276-.01f)MainRoomEdge276(area.center.x,end,area.width,.20f);
   const float half=.9f;float low=z-half,high=z+half;
   if(low>area.yMin)MainRoomEdge276(x,(area.yMin+low)*.5f,.20f,low-area.yMin);
   if(high<area.yMax)MainRoomEdge276(x,(high+area.yMax)*.5f,.20f,area.yMax-high);
   Box("InteriorWall",new Vector3(x,2.75f,z),new Vector3(.20f,.90f,1.8f),"929580",true);
   var hinge=new GameObject("RepairMainRoomHinge210_"+name).transform;hinge.SetParent(Root,false);hinge.position=new Vector3(x,0,low);
   var slab=Box("RepairMainDoor210",new Vector3(x,1.15f,z),new Vector3(.12f,2.30f,1.8f),stair?"556B60":"718572",true);slab.transform.SetParent(hinge,true);
   Box("RepairMainDoorPull210",new Vector3(x-side*.105f,1,z+.63f),new Vector3(.08f,.20f,.07f),"B3A27B").transform.SetParent(hinge,true);
   var door=new WorldDoor{Name=name+"门",Destination="repairmain1",Position=new Vector3(x,0,z),Pivot=hinge,Collider=slab.GetComponent<Collider>(),OpenAngle=side*100,Locked=false,Portal=false,RevealRoom=area,ClosedSafety=new Rect(x-.42f,z-1.18f,.84f,2.36f),OpenSafety=new Rect(side<0?x-2.12f:x,z-1.2f,2.12f,2.4f)};
   Doors.Add(door);RepairMainRooms210.Add(new RepairMainRoom210{Name=name,Area=area,Door=door,ApproachPoint=new Vector3(x-side*1.4f,0,z),WalkPoint=new Vector3(x+side*1.4f,0,z),ReservedStair=stair});
  }
  void RepairMainFurnish210(){
   var k=new RepairInteriorKit200(this);var e=new RepairExteriorKit149(this);
   var parts=RepairMainRooms210[0].Area;var office=RepairMainRooms210[3].Area;var equipment=RepairMainRooms210[4].Area;
   MainPlaced274(k.ToolWallBench(MainSupport274(1,"检修工具箱"),270),"维修大厅",true,3);
   MainPlaced274(k.ToolWallBench(MainSupport274(1,"精密拆修工具箱"),270),"维修大厅",true,3);
   MainPlaced274(k.EngineBench(new Vector3(-7.8f,0,MainNetHalfZ276-3.3f)),"维修大厅",false);
   MainPlaced274(k.PartsRack(new Vector3(8.8f,0,MainNetHalfZ276-.42f)),"维修大厅",true,0);
   MainPlaced274(e.VehicleLift(new Vector3(-9.5f,0,-1.7f),90),"维修大厅",false);
   MainPlaced274(e.EngineHoist(new Vector3(-12,0,2.6f),90),"维修大厅",false);
   // Rear wheels extend to local Z=.47, beyond the .425m deck edge.
   MainPlaced274(e.WeldingCart(new Vector3(equipment.xMin-.1f-.08f-.47f,0,5.4f),90),"维修大厅",true,1);
   MainPlaced274(e.Compressor(new Vector3(-10,0,MainNetHalfZ276-1.055f)),"维修大厅",true,0);
   e.AxleStand(new Vector3(-10.75f,0,-5));e.AxleStand(new Vector3(-9.75f,0,-5));
   MainPlaced274(e.TyreRack(MainWallAnchor274(parts,2,parts.xMin+2.4f,.43f),180),"零件整理与待修件间",true,2);
   foreach(float fraction in new[]{.18f,.42f,.73f})MainPlaced274(k.PartsRack(MainWallAnchor274(parts,3,parts.yMin+parts.height*fraction,.34f),270),"零件整理与待修件间",true,3);
   MainPlaced274(k.EngineBench(new Vector3(parts.center.x,0,parts.yMin+parts.height*.64f)),"零件整理与待修件间",false);
   MainPlaced274(k.WorkLockers(MainWallAnchor274(parts,0,parts.center.x,.276f)),"零件整理与待修件间",true,0);
   Vector3 desk=RepairMainOfficeDeskAnchor274();
   LowFurniture("RepairOfficeDesk210",desk+Vector3.up*.42f,new Vector3(2.2f,.84f,.85f),"798473");
   Chair(desk+new Vector3(0,0,-1.35f),180);
   MainPlaced274(k.WorkLockers(MainWallAnchor274(office,0,office.xMin+2f,.276f)),"维修值班办公室",true,0);
   MainPlaced274(k.PartsRack(MainWallAnchor274(equipment,0,equipment.center.x,.34f)),"设备与配电间",true,0);
   MainPlaced274(k.WashStation(MainWallAnchor274(equipment,1,equipment.yMax-3f,.325f),90),"设备与配电间",true,1);
   MainPlaced274(e.Compressor(MainWallAnchor274(equipment,2,equipment.center.x,.975f),180),"设备与配电间",true,2);
  }
  void RepairMainStairs210(RepairMainRoom210 room){
   // A shut, locked full-height enclosure prevents access to every tread and landing.
   float x=room.Area.center.x;float start=room.Area.yMax-4.7f;
   for(int i=0;i<11;i++){float h=(i+1)*.23f;Box("ReservedStairTread210",new Vector3(x,h*.5f,start+i*.34f),new Vector3(2.45f,h,.34f),i%2==0?"89907B":"737E6D",true);}
   Box("ReservedStairLanding210",new Vector3(x,2.55f,room.Area.yMax-.58f),new Vector3(2.45f,.18f,.9f),"7D8875",true);
   var a=new WorkshopWastelandArt(this);
   foreach(float dx in new[]{-1.13f,1.13f}){
    for(int i=0;i<5;i++){float z=start+i*.70f,y=.96f+i*.47f;a.Tube("ReservedStairRailPosts210","4C6458",new Vector3(x+dx,y-.83f,z),new Vector3(x+dx,y,z),.026f);}
    a.Tube("ReservedStairHandrail210","A1A58C",new Vector3(x+dx,.98f,start),new Vector3(x+dx,2.85f,start+2.8f),.04f);
   }
   RepairMainArtBake210(a,"ReservedStairArt210");
  }
  void RepairMainDress210(){
   var a=new WorkshopWastelandArt(this);
   foreach(var renderer in Root.GetComponentsInChildren<MeshRenderer>())if(renderer.name=="InteriorWall"){
    var b=renderer.bounds;if(b.min.y>.1f)continue;bool horizontal=b.size.x>b.size.z;Vector3 along=horizontal?Vector3.right:Vector3.forward,normal=horizontal?Vector3.forward:Vector3.right;
    foreach(float side in new[]{-1f,1f})a.WallSkin(new Vector3(b.center.x,0,b.center.z)+normal*side*((horizontal?b.size.z:b.size.x)*.5f+.012f),along,normal*side,horizontal?b.size.x:b.size.z,"647769");
   }
   foreach(float x in new[]{-5.8f,0,5.8f})a.StripLight(new Vector3(MainPoint273(new Vector3(x,0,0)).x,2.65f,MainNetHalfZ276-.14f),Vector3.right,Vector3.back);
   foreach(var room in RepairMainRooms210){a.StripLight(new Vector3(room.Area.center.x,2.65f,room.Area.yMax-.15f),Vector3.right,Vector3.back);a.Panel("RepairMainRoomSign210",room.ReservedStair?"968961":"A09B80",room.Door.Position+new Vector3(room.Area.center.x<0?.13f:-.13f,2.55f,0),Vector3.forward,Vector3.up,1.1f,.29f);}
   a.Panel("RepairOfficeNotice210","7B7155",new Vector3(RepairMainOfficeDeskAnchor274().x,1.6f,RepairMainRoomArea276(1,"维修值班办公室").yMax-.15f),Vector3.right,Vector3.up,2,.8f);
   for(int i=0;i<4;i++)a.Panel("RepairPinnedWorkOrders210","BDB497",new Vector3(RepairMainOfficeDeskAnchor274().x-.75f+i*.48f,1.63f,RepairMainRoomArea276(1,"维修值班办公室").yMax-.18f),Vector3.right,Vector3.up,.31f,.48f);
   a.Cube("RepairOfficeRadio210","354D43",RepairMainOfficeDeskAnchor274()+new Vector3(.6f,.98f,0),new Vector3(.45f,.22f,.25f));
   a.Cube("RepairOfficeLedger210","ABAA8B",RepairMainOfficeDeskAnchor274()+new Vector3(-.35f,.885f,-.05f),new Vector3(.46f,.04f,.36f));
   for(int i=0;i<3;i++){float z=MainPoint273(new Vector3(0,0,9.9f)).z+i*1.25f;a.Cube("RepairDistributionCabinet210","556E61",new Vector3(MainNetHalfX276-.22f,1.36f,z),new Vector3(.20f,1.75f,.8f));a.Cube("RepairCabinetHandle210","ACAA89",new Vector3(MainNetHalfX276-.35f,1.34f,z-.23f),new Vector3(.035f,.22f,.045f));}
   a.BrickScar(new Vector3(RepairMainRoomArea276(1,"零件整理与待修件间").xMax+.13f,.8f,-5f),Vector3.forward,Vector3.right);
   foreach(var window in RepairMainWindowPoints276(1)){a.Panel("RepairMainWindowFrame210","3A5145",window,Vector3.forward,Vector3.up,2.8f,.9f);a.Panel("RepairMainDustyGlass210","718576",window+Vector3.right*.03f,Vector3.forward,Vector3.up,2.62f,.73f);}
   // Wall coating remains attached to the solid partition shell.
   RepairMainArtBake210(a,"RepairMainWallWear210");
   var paint=new WorkshopWastelandArt(this);
   foreach(float x in new[]{-3.8f,3.8f})for(int i=0;i*.62f<RepairMainInspection210.height-.2f;i++)if(i%5!=0)paint.Floor("RepairMainInspectionPaint210","A99B70",MainPoint273(new Vector3(x,0,0)).x,RepairMainInspection210.yMin+.1f+i*.62f,.085f,.43f,.16f);
   foreach(float z in new[]{-7.2f,9})paint.Floor("RepairMainInspectionPaint210","A99B70",0,MainPoint273(new Vector3(0,0,z)).z,RepairMainInspection210.width,.085f,.16f);
   foreach(float x in new[]{-1.2f,1.2f})for(int i=0;i<12;i++)paint.Floor("RepairMainTyreMarks210","5F6857",x,MainPoint273(new Vector3(0,0,-10.4f)).z+i*.31f,.075f,.18f,.164f);
   foreach(var p in new[]{new Vector3(-7,.165f,-2),new Vector3(-1.5f,.165f,14.3f),new Vector3(8.5f,.165f,13.1f)})paint.Patch("RepairMainOil210","505C4D",MainPoint273(p),Vector3.right,Vector3.forward,1.3f,.8f,false);
   paint.Floor("RepairMainDrain210","394B41",MainPoint273(new Vector3(-7.1f,0,0)).x,MainPoint273(new Vector3(0,0,9.7f)).z,2.1f,.30f,.166f);
   for(int i=0;i<12;i++)paint.Floor("RepairMainDrainGrate210","919782",MainPoint273(new Vector3(-7.1f,0,0)).x-.95f+i*.17f,MainPoint273(new Vector3(0,0,9.7f)).z,.055f,.28f,.169f);
   RepairPaint149(paint);
  }
  void RepairMainExit210(){
   var a=new WorkshopWastelandArt(this);
   for(int index=0;index<3;index++){
    float x=RepairMainEntryX220(index);a.Cube("RepairMainIndoorShutter210","607767",new Vector3(x,1.6f,-MainNetHalfZ276+.06f),new Vector3(4.7f,3.2f,.08f));
    for(int i=0;i<20;i++)a.Cube("RepairMainShutterSlat210","8B9882",new Vector3(x,.1f+i*.157f,-MainNetHalfZ276+.12f),new Vector3(4.6f,.025f,.035f));
    a.Cube("RepairMainShutterPull210","B0A37F",new Vector3(x,.92f,-MainNetHalfZ276+.17f),new Vector3(.65f,.12f,.055f));
    Doors.Add(new WorldDoor{Name=RepairMainGateNames220[index]+" · 返回室外",Destination="repair-exit:"+RepairMainGateKeys220[index],Portal=true,Position=new Vector3(x,0,-MainNetHalfZ276+.14f)});
   }
   RepairMainArtBake210(a,"RepairMainExitArt210");
  }
  void RepairMainArtBake210(WorkshopWastelandArt art,string name){var g=new GameObject(name);g.transform.SetParent(Root,false);art.Bake(g,g.AddComponent<WorkshopArtResources>());}
 }
}



