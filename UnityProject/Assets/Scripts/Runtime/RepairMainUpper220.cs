using UnityEngine;
using System.Collections.Generic;
namespace GreyHarbor {
 public sealed partial class RaidMap {
  public bool RepairMain220;
  public int RepairFloor220;
  public readonly List<Rect> RepairHallAreas220=new List<Rect>();
  public static Vector3 RepairStairArrival220(bool north){return RepairMainArrivalAnchor274(north);}
  public void BuildRepairMainUpper220(int floor){
   floor=Mathf.Clamp(floor,2,4);RepairMain220=true;RepairFloor220=floor;
   IsInterior=true;InteriorId="repairmain"+floor;InteriorTitle="维修主楼 · "+floor+"楼"+(floor==2?"部件维修与测试":floor==3?"电气技术与档案":"管理与职工休息");LayoutRevision=276;
   Bounds=MainBounds276;InitialRoom=new Rect(-2.5f,-MainNetHalfZ276,5,MainNet276.height);Spawn=new Vector3(0,0,0);
   Root=new GameObject("RepairMain_Floor"+floor+"_220").transform;
   RepairHallAreas220.AddRange(RepairMainPublicAreas276(floor));
   MainShell276("RepairMainFloor220");
   var edges=new HashSet<string>();
   UpperRoom220("西北楼梯",new Rect(-13.29f,7.8f,4.49f,7.99f),-8.8f,10.5f,-1,true,edges);
   UpperRoom220("东南楼梯",new Rect(9.8f,-11.5f,3.49f,6),9.8f,-8.5f,1,true,edges);
   if(floor==2){
    UpperRoom220("部件拆修车间",new Rect(-13.29f,-15.79f,10.69f,13.99f),-2.6f,-4.6f,-1,false,edges);
    UpperRoom220("精密测试间",new Rect(-13.29f,1.8f,10.69f,6),-2.6f,4.4f,-1,false,edges);
    UpperRoom220("工具校验间",new Rect(2.6f,-5.5f,10.69f,3.7f),2.6f,-3.6f,1,false,edges);
    UpperRoom220("维修工艺与备件整理间",new Rect(2.6f,8.7f,10.69f,7.09f),2.6f,11.9f,1,false,edges);
   }else{
    string[] names=floor==3?new[]{"电气装配室","继电与仪表测试室","技术设计办公室","维修记录登记室","工程资料档案室","技术会议室","配电监测办公室"}:new[]{"职工休息室","更衣与清洁室","管理办公室","值班管理室","会议室","人事与排班办公室","钥匙管理室"};
    UpperRoom220(names[0],new Rect(-13.29f,-15.79f,10.69f,7.39f),-2.6f,-11.7f,-1,false,edges);
    UpperRoom220(names[1],new Rect(-13.29f,-8.4f,10.69f,6.6f),-2.6f,-4.7f,-1,false,edges);
    UpperRoom220(names[2],new Rect(-13.29f,1.8f,10.69f,6),-2.6f,4.4f,-1,false,edges);
    UpperRoom220(names[3],new Rect(2.6f,-15.79f,10.69f,4.29f),2.6f,-13.6f,1,false,edges);
    UpperRoom220(names[4],new Rect(2.6f,-5.5f,10.69f,3.7f),2.6f,-3.6f,1,false,edges);
    UpperRoom220(names[5],new Rect(2.6f,1.8f,10.69f,6.9f),2.6f,5.2f,1,false,edges);
    UpperRoom220(names[6],new Rect(2.6f,8.7f,10.69f,7.09f),2.6f,11.9f,1,false,edges);
    if(floor==4){var key=RepairMainRooms210[RepairMainRooms210.Count-1];key.Door.Locked=true;key.Door.Name="钥匙管理室门 · 已锁闭";}
   }
   foreach(var room in RepairMainRooms210)room.Door.Destination=InteriorId;
   var ground=new GroundSurfaceArt(this,220030+floor);ground.Region(RepairMainNet210,GroundFinish.PatchedConcrete,.11f);
   foreach(var room in RepairMainRooms210){var r=room.Area;r.xMin+=.12f;r.xMax-=.12f;r.yMin+=.12f;r.yMax-=.12f;ground.Region(r,room.ReservedStair?GroundFinish.Concrete:floor==2?GroundFinish.PatchedConcrete:GroundFinish.BrokenTile,.11f);}
   ground.Bake("RepairMainUpperGround220");
   FurnishRepairUpper220(floor);DressRepairUpper220(floor);AddRepairStairs220(floor);
   foreach(float z in new[]{-22f,-18f,-11f,-4.5f,0,6f,14f,21.5f})RepairMainWalkPoints210.Add(new Vector3(0,0,z));
   foreach(var p in new[]{new Vector3(8,0,-11.4f),new Vector3(8,0,0),new Vector3(-7,0,0),new Vector3(-5,0,19.3f)})RepairMainWalkPoints210.Add(p);
   PlaceRepairMainContainers240();PopulateRepairKeyRoom2760();PrepareMainPatrol276();AddMainDecoration274();FinishRepairWalls220();FinalizeHarborGround137();Physics.SyncTransforms();
  }
  void UpperRoom220(string name,Rect area,float x,float z,int side,bool stair,HashSet<string> edges){
   RepairMainRoomAdd210(name,area,x,z,side,stair);var room=RepairMainRooms210[RepairMainRooms210.Count-1];room.Door.Locked=false;area=room.Area;
  }
  public void AddRepairStairs220(int floor){
   RepairMain220=true;RepairFloor220=floor;
   foreach(var room in RepairMainRooms210){
    if(!room.ReservedStair)continue;bool north=room.Area.center.x<0;
    room.Name=north?"西北楼梯":"东南楼梯";room.Door.Name=room.Name+"门";room.Door.Locked=false;
    int side=north?-1:1;float doorX=room.Door.Position.x,doorZ=room.Door.Position.z;
    // Open into the stair enclosure; keep the normal hall approach outside the sweep.
    room.Door.InstantToggle=true;room.Door.ClosedSafety=new Rect(doorX-.06f,doorZ-.9f,.12f,1.8f);
    room.Door.OpenAngle=side*180;room.Door.OpenSafety=new Rect(side<0?doorX-2.12f:doorX,doorZ-2.12f,2.12f,3.32f);
    Vector3 arrival=RepairStairArrival220(north);
    Vector3 flight=RepairMainStairAnchor274(north);float x=flight.x,start=flight.z;
    var art=new WorkshopWastelandArt(this);
    for(int i=0;i<11;i++){float h=(i+1)*.23f;Box("RepairStairTread220",new Vector3(x,h*.5f,start+i*.34f),new Vector3(1.25f,h,.34f),i%2==0?"89907B":"737E6D",true);}
    Box("RepairStairLanding220",new Vector3(x,2.55f,start+4.12f),new Vector3(1.25f,.18f,.9f),"7D8875",true);
    foreach(float dx in new[]{-.58f,.58f}){
     for(int i=0;i<5;i++){float z=start+i*.70f,y=.96f+i*.47f;art.Tube("RepairStairRailPosts220","4C6458",new Vector3(x+dx,y-.83f,z),new Vector3(x+dx,y,z),.026f);}
     art.Tube("RepairStairHandrail220","A1A58C",new Vector3(x+dx,.98f,start),new Vector3(x+dx,2.85f,start+2.8f),.04f);
    }
    Vector3 up=arrival+Vector3.forward*(north?-2.45f:-2.2f),down=arrival+Vector3.forward*(north?3.1f:2.2f);
    if(floor<4)RepairStairPortal220(art,up,floor+1,north,true);
    if(floor>1)RepairStairPortal220(art,down,floor-1,north,false);
    RepairMainArtBake210(art,"RepairStairArt220");
    RepairMainWalkPoints210.Add(RepairStairArrival220(north));if(floor<4)RepairMainWalkPoints210.Add(up);if(floor>1)RepairMainWalkPoints210.Add(down);
   }
  }
  void RepairStairPortal220(WorkshopWastelandArt art,Vector3 p,int target,bool north,bool up){
   Doors.Add(new WorldDoor{Name=(north?"西北":"东南")+"楼梯 · "+(up?"上楼":"下楼")+"至"+target+"楼",Destination="repair-floor:"+target+":"+(north?"north":"south"),Portal=true,Position=p});
   art.Floor(up?"RepairStairUpMarker220":"RepairStairDownMarker220",up?"D5A24F":"59A9BC",p.x,p.z,1.0f,1.0f,.17f);
   // All interaction footprints are on the clear ground-level strip beside the flight.
   art.Panel("RepairStairDirectionPlate220",up?"B3A77F":"819A8D",new Vector3(north?-MainNetHalfX276+.13f:MainNetHalfX276-.13f,1.9f,p.z),Vector3.forward,Vector3.up,.55f,.4f);
  }
  void FurnishRepairUpper220(int floor){
   var kit=new RepairInteriorKit200(this);
   foreach(var room in RepairMainRooms210){
    if(room.ReservedStair||room.Name=="钥匙管理室")continue;var r=room.Area;string name=room.Name;
    Vector3 center=new Vector3(r.center.x,0,r.center.y);float left=r.xMin+1.7f,right=r.xMax-1.5f;
    if(floor==2||floor==3&&(name.Contains("装配")||name.Contains("测试"))){
     MainPlaced274(kit.ToolWallBench(MainWallAnchor274(r,0,left,.45f)),name,true,0);
     MainPlaced274(kit.PartsRack(MainWallAnchor274(r,2,left,.34f),180),name,true,2);
     MainPlaced274(kit.WorkLockers(MainWallAnchor274(r,0,right,.276f)),name,true,0);
     Vector3 assembly=name=="部件拆修车间"?MainSupport274(2,"拆修台工具箱"):center+new Vector3(-1.7f,0,-1.1f);
     MainPlaced274(kit.EngineBench(assembly),name,false);
     MainPlaced274(kit.EngineBench(center+new Vector3(1.7f,0,1.1f)),name,false);
     if(name=="部件拆修车间"){
      MainPlaced274(kit.ToolWallBench(MainWallAnchor274(r,3,r.yMax-5f,.45f),270),name,true,3);
      MainPlaced274(kit.PartsRack(MainWallAnchor274(r,3,r.yMin+3f,.34f),270),name,true,3);
     }
    }else if(name.Contains("档案")){
     foreach(float dx in new[]{-3f,0,3f})UpperFiles220(MainWallAnchor274(r,0,r.center.x+dx,.26f),2.1f);
     UpperDesk220(center,false);
    }else if(name.Contains("休息")){
     UpperTable220(center,new Vector2(3.3f,1.4f),4);
     MainPlaced274(kit.WashStation(MainWallAnchor274(r,0,left,.325f)),name,true,0);
     UpperSofa220(MainSupport274(4,"职工遗留手提箱"));
     MainPlaced274(kit.WorkLockers(MainWallAnchor274(r,0,right,.276f)),name,true,0);
    }else if(name.Contains("更衣")){
     foreach(float px in new[]{left,right})MainPlaced274(kit.WorkLockers(MainWallAnchor274(r,0,px,.276f)),name,true,0);
     MainPlaced274(kit.WashStation(MainWallAnchor274(r,2,left,.325f),180),name,true,2);
     LowFurniture("RepairChangingBench220",center+Vector3.up*.25f,new Vector3(3,.5f,.65f),"85856B");
    }else if(name.Contains("会议")){
     UpperTable220(center,new Vector2(4.4f,1.4f),6,floor!=4);
     UpperFiles220(MainWallAnchor274(r,0,right,.26f),1.6f);
    }else{
     // Two desks form one centred work island, including the chairs and desk details.
     if(r.height<7)UpperDesk220(center,true);else {UpperDesk220(center+new Vector3(-1.45f,0,1.1f),true);UpperDesk220(center+new Vector3(1.45f,0,-1.1f),floor==3);}
     UpperFiles220(MainWallAnchor274(r,0,left,.26f),1.8f);
     MainPlaced274(kit.WorkLockers(MainWallAnchor274(r,0,right,.276f)),name,true,0);
    }
   }
   if(floor==2){
    var publicEast=Rect.MinMaxRect(2.6f,1.8f,MainNetHalfX276,8f);
    foreach(float z in new[]{6.3f})MainPlaced274(kit.ToolWallBench(MainWallAnchor274(publicEast,1,z,.45f),90),"公共测试工区",true,1);
    foreach(float z in new[]{3.8f,6.2f})MainPlaced274(kit.EngineBench(new Vector3(publicEast.center.x,0,z)),"公共测试工区",false);
    var entry=Rect.MinMaxRect(2.5f,-MainNetHalfZ276,MainNetHalfX276,-15f);
    MainPlaced274(kit.PartsRack(MainWallAnchor274(entry,2,8f,.34f),180),"东南入口区",true,2);
    MainPlaced274(kit.ToolWallBench(MainWallAnchor274(entry,2,15.5f,.45f),180),"东南入口区",true,2);
    MainPlaced274(kit.ToolWallBench(new Vector3(-7.8f,0,MainNetHalfZ276-.53f)),"西北公共工区",true,0);
    var stair=RepairMainRoomArea276(floor,"西北楼梯");
    MainPlaced274(kit.PartsRack(new Vector3(stair.xMax+.52f,0,stair.yMax-2.5f),270),"西北公共工区",true,3);
   }else {
    UpperFiles220(new Vector3(-7.8f,0,MainNetHalfZ276-.34f),3.6f);
    UpperDesk220(new Vector3(-7.8f,0,20.3f),true);
   }
  }
  void UpperDesk220(Vector3 p,bool monitor){
   var desk=Box("RepairOfficeDesk220",p+Vector3.up*.42f,new Vector3(2.2f,.84f,.85f),"7C806C",true);SceneObstacle.Apply(desk,ObstacleKind.Low);MainPlaced274(desk.transform,InteriorTitle,false);Chair(p+new Vector3(0,0,-1.35f),180);
   var a=new WorkshopWastelandArt(this);a.Cube("RepairLedger220","ABA489",p+new Vector3(-.60f,.865f,-.06f),new Vector3(.45f,.04f,.35f));
   a.Cube("RepairDeskLampFoot220","435B4F",p+new Vector3(.8f,.88f,.15f),new Vector3(.22f,.05f,.20f));a.Tube("RepairDeskLampStem220","687765",p+new Vector3(.8f,.89f,.15f),p+new Vector3(.65f,1.25f,.1f),.025f);a.Cube("RepairDeskLampShade220","81907B",p+new Vector3(.62f,1.25f,.10f),new Vector3(.30f,.10f,.23f));
   if(monitor){a.Cube("RepairOfficeMonitor220","364B40",p+new Vector3(.2f,1.08f,.17f),new Vector3(.55f,.4f,.28f));a.Cube("RepairOfficeMonitorScreen220","778F81",p+new Vector3(.2f,1.1f,.022f),new Vector3(.44f,.27f,.012f));a.Cube("RepairKeyboard220","5B6A58",p+new Vector3(.2f,.88f,-.23f),new Vector3(.55f,.05f,.18f));}
   RepairMainArtBake210(a,"RepairDeskDetail220");
  }
  void UpperFiles220(Vector3 p,float w,float yaw=0){
   var g=Box("RepairArchiveCabinet220",p+Vector3.up*.95f,new Vector3(w,1.9f,.52f),"657A68",true);g.transform.rotation=Quaternion.Euler(0,yaw,0);MainPlaced274(g.transform,InteriorTitle,true,Mathf.RoundToInt(Mathf.Repeat(yaw,360)/90));
   var a=new WorkshopWastelandArt(this);for(int i=0;i<4;i++){Vector3 local=new Vector3(0,.28f+i*.44f,-.28f);Vector3 q=p+Quaternion.Euler(0,yaw,0)*local;a.Panel("RepairArchiveDrawer220",i%2==0?"83917B":"73856E",q,Quaternion.Euler(0,yaw,0)*Vector3.right,Vector3.up,w-.10f,.38f);a.Cube("RepairArchiveLabel220","B5AF90",q+Quaternion.Euler(0,yaw,0)*new Vector3(0,.03f,-.025f),new Vector3(.24f,.09f,.02f));}RepairMainArtBake210(a,"RepairArchiveDetail220");
  }
  void UpperTable220(Vector3 p,Vector2 size,int chairs,bool southRow=true){
   var table=Box("RepairMeetingTable220",p+Vector3.up*.42f,new Vector3(size.x,.84f,size.y),"8A8469",true);SceneObstacle.Apply(table,ObstacleKind.Low);MainPlaced274(table.transform,InteriorTitle,false);
   // Four-floor meeting room needs the south aisle clear beside the staircase wall.
   for(int i=0;i<chairs/2;i++){float x=p.x-size.x*.34f+i*(size.x*.68f/Mathf.Max(1,chairs/2-1));if(southRow)Chair(new Vector3(x,0,p.z-size.y*.5f-.95f),180);Chair(new Vector3(x,0,p.z+size.y*.5f+.95f),0);}
   var a=new WorkshopWastelandArt(this);a.Cube("RepairMeetingNotes220","B5AF92",p+new Vector3(-.7f,.865f,0),new Vector3(.65f,.035f,.43f));a.Cube("RepairMeetingRadio220","3F594C",p+new Vector3(.7f,.97f,.1f),new Vector3(.4f,.22f,.25f));RepairMainArtBake210(a,"RepairMeetingDetail220");
  }
  void UpperSofa220(Vector3 p){
   var sofa=new GameObject("RepairBreakSofaGroup274").transform;sofa.SetParent(Root,false);sofa.position=p;
   var seat=Box("RepairBreakSofa220",p+Vector3.up*.34f,new Vector3(2.6f,.68f,.85f),"687D6B",true);SceneObstacle.Apply(seat,ObstacleKind.Low);seat.transform.SetParent(sofa,true);
   Box("RepairBreakSofaBack220",p+new Vector3(0,.84f,.31f),new Vector3(2.6f,.50f,.18f),"778672").transform.SetParent(sofa,true);
   MainPlaced274(sofa,"职工休息室",true,0);
  }
  void DressRepairUpper220(int floor){
   var a=new WorkshopWastelandArt(this);
   foreach(var renderer in Root.GetComponentsInChildren<MeshRenderer>())if(renderer.name=="InteriorWall"){
    var b=renderer.bounds;if(b.min.y>.1f)continue;bool horizontal=b.size.x>b.size.z;Vector3 along=horizontal?Vector3.right:Vector3.forward,normal=horizontal?Vector3.forward:Vector3.right;
    foreach(float side in new[]{-1f,1f})a.WallSkin(new Vector3(b.center.x,0,b.center.z)+normal*side*((horizontal?b.size.z:b.size.x)*.5f+.012f),along,normal*side,horizontal?b.size.x:b.size.z,"647769");
   }
   foreach(var room in RepairMainRooms210){a.StripLight(new Vector3(room.Area.center.x,2.65f,room.Area.yMax-.16f),Vector3.right,Vector3.back);a.Panel("RepairUpperRoomPlate220","AAA182",room.Door.Position+new Vector3(room.Area.center.x<0?.14f:-.14f,2.55f,0),Vector3.forward,Vector3.up,1.15f,.29f);
    if(!room.ReservedStair&&room.Name!="钥匙管理室"){float z=room.Area.yMax-.15f;a.Panel("RepairUpperNoticeBoard220","68694E",new Vector3(room.Area.center.x,1.9f,z),Vector3.right,Vector3.up,2,.8f);for(int i=0;i<3;i++)a.Panel("RepairUpperPinnedSheets220","BBB294",new Vector3(room.Area.center.x-.6f+i*.6f,1.9f,z-.02f),Vector3.right,Vector3.up,.4f,.5f);}
   }
   foreach(float z in new[]{-12f,-6f,0,6f,12f})a.StripLight(MainPoint273(new Vector3(0,2.8f,z)),Vector3.right,Vector3.back);
   foreach(var window in RepairMainWindowPoints276(floor)){a.Panel("RepairUpperWindowFrame220","3A5145",window,Vector3.forward,Vector3.up,2.3f,.86f);a.Panel("RepairUpperDustyWindow220","748778",window-Vector3.right*Mathf.Sign(window.x)*.025f,Vector3.forward,Vector3.up,2.12f,.69f);}
   RepairMainArtBake210(a,"RepairUpperWallWear220");
   var paint=new WorkshopWastelandArt(this);foreach(float x in new[]{-2.05f,2.05f})for(int i=0;i*.68f<MainNet276.height-1f;i++)if(i%4!=0)paint.Floor("RepairUpperHallGuide220","A2976F",x,-MainNetHalfZ276+.49f+i*.68f,.055f,.4f,.16f);
   if(floor==2)foreach(var p in new[]{new Vector3(-8,.165f,-10),new Vector3(7.7f,.165f,3.4f),new Vector3(-10,.165f,4)})paint.Patch("RepairUpperWorkWear220","5B6250",MainPoint273(p),Vector3.right,Vector3.forward,1.3f,.8f,false);
   RepairPaint149(paint);
  }
 }
}
