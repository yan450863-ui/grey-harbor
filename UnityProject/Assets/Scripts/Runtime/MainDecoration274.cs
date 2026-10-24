using UnityEngine;
using System.Collections.Generic;
namespace GreyHarbor {
 public enum MainDecorationType274 { ToolPanel,ServicePipes,CableReel,CrateShelf,UtilityCabinet,CleaningRack,NoticeBoard,Workstation,WorkTable,FallenChair,LowCabinet,PaperBoxes }
 public sealed class MainDecorationItem274 {
  public string Name,RoomName;public MainDecorationType274 Type;public Vector3 Anchor;public Rect Footprint;public ObstacleKind Kind;public bool WallMounted;public GameObject View;
 }
 public sealed class MainDecorationGroup274 {
  public string Name,RoomName;public Vector3 Anchor;public Rect Footprint;public bool Center;
  public readonly List<MainDecorationItem274> Items=new List<MainDecorationItem274>();
 }
 public sealed partial class RaidMap {
  public readonly List<MainDecorationItem274> MainDecorationItems274=new List<MainDecorationItem274>();
  public readonly List<MainDecorationGroup274> MainDecorationGroups274=new List<MainDecorationGroup274>();
  // Invoke after the existing furniture, doors, stairs, search props and walk points are built.
  // This creates geometry only: no loot, actors, inventory or economy changes.
  public void AddMainDecoration274(){
   if(!RepairMain220||!Root||MainDecorationGroups274.Count>0)return;
   Physics.SyncTransforms();
   foreach(var room in RepairMainRooms210){
    DecorationWallRow274(room);
    if(!room.ReservedStair&&room.Name!="钥匙管理室")DecorationCenterIsland274(room);
   }
   if(RepairFloor220==1){
    foreach(var area in RepairMainPublicAreas276(RepairFloor220)){
     var hall=new RepairMainRoom210{Name="维修大厅",Area=area};DecorationCenterIsland274(hall);
    }
    var parts=RepairMainRoomArea276(1,"零件整理与待修件间");
    DecorationWallRow274(new RepairMainRoom210{Name="维修大厅",Area=new Rect(parts.xMax,parts.yMin,Mathf.Min(10,MainHall276.width),parts.height)},true);
   }
   AddMainPublicDressing276();AddCorridorDressing277();Physics.SyncTransforms();
  }
  static Rect DecorationExpand274(Rect r,float n)=>new Rect(r.xMin-n,r.yMin-n,r.width+n*2,r.height+n*2);
  static bool DecorationNear274(Rect r,Vector3 p,float margin){float x=p.x-Mathf.Clamp(p.x,r.xMin,r.xMax),z=p.z-Mathf.Clamp(p.z,r.yMin,r.yMax);return x*x+z*z<margin*margin;}
  static bool DecorationSegment274(Rect area,Vector3 a,Vector3 b,float margin){
   var r=DecorationExpand274(area,margin);float lo=0,hi=1;
   return DecorationSlab274(a.x,b.x-a.x,r.xMin,r.xMax,ref lo,ref hi)&&DecorationSlab274(a.z,b.z-a.z,r.yMin,r.yMax,ref lo,ref hi);
  }
  static bool DecorationSlab274(float start,float delta,float min,float max,ref float lo,ref float hi){
   if(Mathf.Abs(delta)<.0001f)return start>=min&&start<=max;
   float a=(min-start)/delta,b=(max-start)/delta;if(a>b){float t=a;a=b;b=t;}
   lo=Mathf.Max(lo,a);hi=Mathf.Min(hi,b);return lo<=hi;
  }
  bool DecorationProtected274(Rect area,bool floor){
   foreach(var window in RepairMainWindowPoints276(RepairFloor220))if(DecorationNear274(area,window,(RepairFloor220==1?1.4f:1.15f)+.15f))return true;
   if(floor){
    // New 5m spine / 3.4m cross passage, sized from the current building bounds.
    if(area.Overlaps(new Rect(-2.5f,Bounds.yMin,5,Bounds.height))||area.Overlaps(new Rect(Bounds.xMin,-1.7f,Bounds.width,3.4f)))return true;
    if(RepairFloor220==1&&area.Overlaps(DecorationExpand274(RepairMainInspection210,.45f)))return true;
    foreach(var p in RepairMainWalkPoints210)if(DecorationNear274(area,p,.85f))return true;
    if(DecorationNear274(area,Spawn,1.7f)||DecorationNear274(area,RepairStairArrival220(true),2)||DecorationNear274(area,RepairStairArrival220(false),2))return true;
    // These plans are populated before decoration, using the actual new room geometry.
    foreach(var plan in MainPatrolPlans276){
     if(DecorationNear274(area,plan.Home,1.05f))return true;var previous=plan.Home;
     if(plan.Targets!=null)foreach(var target in plan.Targets){
      if(DecorationSegment274(area,previous,target,1.05f)||DecorationSegment274(area,plan.Home,target,1.05f))return true;
      previous=target;
     }
     if(DecorationSegment274(area,previous,plan.Home,1.05f))return true;
    }
   }
   foreach(var door in Doors){
    if(DecorationNear274(area,door.Position,floor?1.8f:1.1f))return true;
    if(floor&&!door.Portal&&(area.Overlaps(DecorationExpand274(door.OpenSafety,.65f))||area.Overlaps(DecorationExpand274(door.ClosedSafety,.65f))))return true;
   }
   foreach(var box in Loot)if(DecorationNear274(area,box.Position,floor?1.5f:.8f))return true;
   return false;
  }
  bool DecorationClear274(Rect area,float clearance,int backingSide=-1,float backingEdge=0){
   var expanded=DecorationExpand274(area,clearance);
   // A wall row keeps side/front clearance while its rear is intentionally .08m from its own wall.
   // Clip only the outward part of the clearance envelope; other walls and physical props still count.
   if(backingSide==3)expanded.xMin=Mathf.Max(expanded.xMin,backingEdge+.025f);
   if(backingSide==1)expanded.xMax=Mathf.Min(expanded.xMax,backingEdge-.025f);
   if(backingSide==2)expanded.yMin=Mathf.Max(expanded.yMin,backingEdge+.025f);
   if(backingSide==0)expanded.yMax=Mathf.Min(expanded.yMax,backingEdge-.025f);
   foreach(var col in Root.GetComponentsInChildren<Collider>()){
    if(!col.enabled||!col.gameObject.activeInHierarchy||(SceneObstacle.MovementMask&(1<<col.gameObject.layer))==0)continue;
    var b=col.bounds;if(b.min.y>1.9f||b.max.y<.15f)continue;
    if(expanded.Overlaps(new Rect(b.min.x,b.min.z,b.size.x,b.size.z)))return false;
   }
   // Newly created local colliders may not have synced bounds yet; records supply their exact footprint.
   foreach(var item in MainDecorationItems274)if(!item.WallMounted&&expanded.Overlaps(item.Footprint))return false;
   return true;
  }
  void DecorationWallRow274(RepairMainRoom210 room,bool outerOnly=false){
   var r=room.Area;bool quiet=room.ReservedStair||room.Name=="钥匙管理室";
   MainDecorationType274[] types;
   if(quiet)types=new[]{MainDecorationType274.NoticeBoard,MainDecorationType274.ServicePipes,MainDecorationType274.NoticeBoard};
   else if(room.Name.Contains("清洁")||room.Name.Contains("更衣")||room.Name.Contains("休息"))types=new[]{MainDecorationType274.CleaningRack,MainDecorationType274.NoticeBoard,MainDecorationType274.LowCabinet};
   else if(room.Name.Contains("办公")||room.Name.Contains("登记")||room.Name.Contains("管理")||room.Name.Contains("会议")||room.Name.Contains("排班")||room.Name.Contains("档案"))types=new[]{MainDecorationType274.UtilityCabinet,MainDecorationType274.NoticeBoard,MainDecorationType274.ServicePipes};
   else types=new[]{MainDecorationType274.ToolPanel,room.Name.Contains("配电")?MainDecorationType274.UtilityCabinet:MainDecorationType274.CableReel,MainDecorationType274.CrateShelf};
   // Prefer the outer wall; horizontal walls also offer middle sections in short rooms.
   int outer=r.center.x<0?3:1;
   foreach(int count in new[]{3,2})foreach(int side in outerOnly?new[]{outer}:new[]{outer,0,2}){
    bool vertical=side==1||side==3;float edge=side==3?r.xMin:side==1?r.xMax:side==0?r.yMax:r.yMin;
    float shell=side==3?MainNet276.xMin:side==1?MainNet276.xMax:side==0?MainNet276.yMax:MainNet276.yMin;
    // Room partitions are centered on the plan edge; the shell plan already names its inner face.
    if(Mathf.Abs(edge-shell)>.02f)edge+=(side==3||side==2)?.10f:-.10f;
    float low=vertical?r.yMin:r.xMin,high=vertical?r.yMax:r.xMax;
    float yaw=side==3?270:side==1?90:side==0?0:180;
    foreach(float fraction in new[]{.5f,.32f,.68f,.20f,.80f,.12f,.88f,.4f,.6f}){
     float center=Mathf.Lerp(low,high,fraction);var positions=new Vector3[count];bool safe=true;
     for(int i=0;i<count;i++){
      // Two-piece fallback retains a real floor object for non-stair furniture rows.
      int t=count==2&&!quiet?(i==0?1:2):i;
      var type=types[t];bool mount=DecorationWallMounted274(type);var size=DecorationSize274(type);
      float along=center+(i-(count-1)*.5f)*1.85f,inset=mount?.13f:size.z*.5f+.08f;
      positions[i]=vertical?new Vector3(edge+(side==3?inset:-inset),0,along):new Vector3(along,0,edge+(side==2?inset:-inset));
      var area=DecorationFootprint274(positions[i],size,yaw);
      if((vertical?area.yMin:area.xMin)<low+.65f||(vertical?area.yMax:area.xMax)>high-.65f||DecorationProtected274(area,!mount)||(!mount&&!DecorationClear274(area,.45f,side,edge))){safe=false;break;}
     }
     if(!safe)continue;
     var group=new MainDecorationGroup274{Name="MainWallRow274_"+RepairFloor220+"_"+room.Name,RoomName=room.Name,Anchor=positions[0],Center=false};
     for(int i=0;i<count;i++){int t=count==2&&!quiet?(i==0?1:2):i;group.Items.Add(DecorationAsset274(types[t],positions[i],yaw,room.Name,group.Name+"_"+i));}
     DecorationFinishGroup274(group);return;
    }
   }
   var marker=new Vector3(outer==3?r.xMin+.13f:r.xMax-.13f,0,r.center.y);float markerYaw=outer==3?270:90;
   if(!DecorationProtected274(DecorationFootprint274(marker,DecorationSize274(MainDecorationType274.NoticeBoard),markerYaw),false)){
    var group=new MainDecorationGroup274{Name="MainWallMarker274_"+RepairFloor220+"_"+room.Name,RoomName=room.Name,Anchor=marker,Center=false};
    group.Items.Add(DecorationAsset274(MainDecorationType274.NoticeBoard,marker,markerYaw,room.Name,group.Name+"_0"));DecorationFinishGroup274(group);
   }
  }
  sealed class DecorationArrangement274 {
   public MainDecorationType274[] Types;public Vector3[] Offsets;
   public DecorationArrangement274(MainDecorationType274[] types,params Vector3[] offsets){Types=types;Offsets=offsets;}
  }
  void DecorationCenterIsland274(RepairMainRoom210 room){
   var r=room.Area;bool clerical=room.Name.Contains("办公")||room.Name.Contains("登记")||room.Name.Contains("管理")||room.Name.Contains("排班");
   bool workshop=!clerical&&(room.Name.Contains("车间")||room.Name.Contains("装配")||room.Name.Contains("整理")||room.Name.Contains("配电")||room.Name.Contains("校验")||room.Name.Contains("维修大厅")||room.Name.Contains("会议"));
   bool rest=room.Name.Contains("休息")||room.Name.Contains("更衣")||room.Name.Contains("清洁");
   bool archive=room.Name.Contains("档案")||room.Name.Contains("库");
   var desk=workshop||rest?MainDecorationType274.WorkTable:MainDecorationType274.Workstation;
   var arrangements=new List<DecorationArrangement274>();
   if(rest||archive)arrangements.Add(new DecorationArrangement274(new[]{MainDecorationType274.LowCabinet,MainDecorationType274.PaperBoxes},Vector3.zero,new Vector3(1.06f,0,0)));
   if(!archive)arrangements.Add(new DecorationArrangement274(new[]{desk,MainDecorationType274.FallenChair},Vector3.zero,new Vector3(-.35f,0,-1.22f)));
   if(!rest&&!archive)arrangements.Insert(0,new DecorationArrangement274(new[]{desk,MainDecorationType274.LowCabinet,MainDecorationType274.PaperBoxes},Vector3.zero,new Vector3(1.70f,0,0),new Vector3(1.70f,0,-.80f)));
   if(rest)arrangements.Insert(0,new DecorationArrangement274(new[]{MainDecorationType274.WorkTable,MainDecorationType274.FallenChair,MainDecorationType274.PaperBoxes},Vector3.zero,new Vector3(-.35f,0,-1.22f),new Vector3(1.70f,0,0)));
   if(archive)arrangements.Insert(0,new DecorationArrangement274(new[]{MainDecorationType274.LowCabinet,MainDecorationType274.PaperBoxes,MainDecorationType274.FallenChair},Vector3.zero,new Vector3(1.06f,0,0),new Vector3(-1.10f,0,.20f)));
   // Small offices use a clerical spill-over pair when a second workstation cannot fit.
   if(!rest&&!archive)arrangements.Add(new DecorationArrangement274(new[]{MainDecorationType274.LowCabinet,MainDecorationType274.PaperBoxes},Vector3.zero,new Vector3(1.06f,0,0)));
   var candidates=new List<Vector3>();
   foreach(float z in new[]{.5f,.25f,.75f,.15f,.85f})foreach(float x in new[]{.23f,.77f,.5f,.36f,.64f})candidates.Add(new Vector3(Mathf.Lerp(r.xMin,r.xMax,x),0,Mathf.Lerp(r.yMin,r.yMax,z)));
   foreach(var arrangement in arrangements)foreach(float yaw in new[]{0f,90f,180f,270f})foreach(var p in candidates){
    var rotation=Quaternion.Euler(0,yaw,0);var positions=new Vector3[arrangement.Types.Length];Rect area=default(Rect);
    for(int i=0;i<positions.Length;i++){
     positions[i]=p+rotation*arrangement.Offsets[i];var itemArea=DecorationFootprint274(positions[i],DecorationSize274(arrangement.Types[i]),yaw);
     if(i==0)area=itemArea;else{area.xMin=Mathf.Min(area.xMin,itemArea.xMin);area.xMax=Mathf.Max(area.xMax,itemArea.xMax);area.yMin=Mathf.Min(area.yMin,itemArea.yMin);area.yMax=Mathf.Max(area.yMax,itemArea.yMax);}
    }
    if(area.xMin<r.xMin+.98f||area.xMax>r.xMax-.98f||area.yMin<r.yMin+.98f||area.yMax>r.yMax-.98f)continue;
    // Actual collider bounds replace the obsolete fixed center exclusion rectangle.
    if(DecorationProtected274(DecorationExpand274(area,.35f),true)||!DecorationClear274(area,.95f))continue;
    var group=new MainDecorationGroup274{Name="MainCenterIsland274_"+RepairFloor220+"_"+room.Name+"_"+MainDecorationGroups274.Count,RoomName=room.Name,Anchor=p,Center=true};
    for(int i=0;i<positions.Length;i++)group.Items.Add(DecorationAsset274(arrangement.Types[i],positions[i],yaw,room.Name,group.Name+"_"+i));
    DecorationFinishGroup274(group);return;
   }
  }
  void DecorationFinishGroup274(MainDecorationGroup274 group){
   if(group.Items.Count==0)return;var area=group.Items[0].Footprint;
   foreach(var item in group.Items){area.xMin=Mathf.Min(area.xMin,item.Footprint.xMin);area.xMax=Mathf.Max(area.xMax,item.Footprint.xMax);area.yMin=Mathf.Min(area.yMin,item.Footprint.yMin);area.yMax=Mathf.Max(area.yMax,item.Footprint.yMax);}group.Footprint=area;MainDecorationGroups274.Add(group);
  }
  static bool DecorationWallMounted274(MainDecorationType274 type)=>type==MainDecorationType274.ToolPanel||type==MainDecorationType274.ServicePipes||type==MainDecorationType274.CleaningRack||type==MainDecorationType274.NoticeBoard;
  static Vector3 DecorationSize274(MainDecorationType274 type){
   switch(type){
    case MainDecorationType274.ToolPanel:return new Vector3(1.45f,1.1f,.20f);
    case MainDecorationType274.ServicePipes:return new Vector3(1.55f,1.8f,.22f);
    case MainDecorationType274.NoticeBoard:return new Vector3(1.25f,.9f,.16f);
    case MainDecorationType274.CleaningRack:return new Vector3(1.35f,1.6f,.20f);
    case MainDecorationType274.CableReel:return new Vector3(1.10f,.85f,.72f);
    case MainDecorationType274.CrateShelf:return new Vector3(1.45f,1.68f,.70f);
    case MainDecorationType274.UtilityCabinet:return new Vector3(1.15f,1.55f,.62f);
    case MainDecorationType274.Workstation:case MainDecorationType274.WorkTable:return new Vector3(2.2f,.84f,.85f);
    case MainDecorationType274.FallenChair:return new Vector3(.82f,.78f,1.35f);
    case MainDecorationType274.LowCabinet:return new Vector3(.95f,.90f,.62f);
    default:return new Vector3(.95f,.55f,.72f);
   }
  }
  static Rect DecorationFootprint274(Vector3 p,Vector3 size,float yaw){bool turn=Mathf.Abs(Mathf.Sin(yaw*Mathf.Deg2Rad))>.5f;float w=turn?size.z:size.x,d=turn?size.x:size.z;return new Rect(p.x-w*.5f,p.z-d*.5f,w,d);}
  MainDecorationItem274 DecorationAsset274(MainDecorationType274 type,Vector3 p,float yaw,string room,string name){
   var a=new WorkshopWastelandArt(this);var size=DecorationSize274(type);bool mount=DecorationWallMounted274(type);
   switch(type){
    case MainDecorationType274.ToolPanel:DecorationTools274(a);break;
    case MainDecorationType274.ServicePipes:DecorationPipes274(a);break;
    case MainDecorationType274.NoticeBoard:DecorationNotice274(a);break;
    case MainDecorationType274.CleaningRack:DecorationCleaning274(a);break;
    case MainDecorationType274.CableReel:DecorationReel274(a);break;
    case MainDecorationType274.CrateShelf:DecorationShelf274(a);break;
    case MainDecorationType274.UtilityCabinet:DecorationCabinet274(a,true);break;
    case MainDecorationType274.LowCabinet:DecorationCabinet274(a,false);break;
    case MainDecorationType274.Workstation:DecorationDesk274(a,true);break;
    case MainDecorationType274.WorkTable:DecorationDesk274(a,false);break;
    case MainDecorationType274.FallenChair:DecorationChair274(a);break;
    case MainDecorationType274.PaperBoxes:DecorationPaper274(a);break;
   }
   var view=new GameObject(name+"_"+type);view.transform.SetParent(Root,false);a.Bake(view,view.AddComponent<WorkshopArtResources>());view.transform.position=p;view.transform.rotation=Quaternion.Euler(0,yaw,0);
   var kind=mount?ObstacleKind.Decoration:type==MainDecorationType274.UtilityCabinet||type==MainDecorationType274.CrateShelf?ObstacleKind.High:ObstacleKind.Low;
   if(!mount){var body=new GameObject("DecorationBody274_"+type);body.transform.SetParent(view.transform,false);body.transform.localPosition=Vector3.up*(size.y*.5f);body.AddComponent<BoxCollider>().size=size;SceneObstacle.Apply(body,kind);}
   if(type==MainDecorationType274.Workstation){
    // The table is low cover; the monitor's actual volume also intercepts sight and gunfire.
    var screen=new GameObject("DecorationMonitorCollision274");screen.transform.SetParent(view.transform,false);screen.transform.localPosition=new Vector3(.25f,1.21f,.16f);screen.AddComponent<BoxCollider>().size=new Vector3(.66f,.43f,.105f);SceneObstacle.Apply(screen,ObstacleKind.High);
   }
   var item=new MainDecorationItem274{Name=name,RoomName=room,Type=type,Anchor=p,Footprint=DecorationFootprint274(p,size,yaw),Kind=kind,WallMounted=mount,View=view};MainDecorationItems274.Add(item);return item;
  }
  static void DecorationTools274(WorkshopWastelandArt a){
   a.Cube("OldToolPegboard274","617064",new Vector3(0,1.75f,0),new Vector3(1.42f,.95f,.065f));
   for(int row=0;row<4;row++)for(int x=0;x<8;x++)a.Cube("PegboardHoles274","3F5146",new Vector3(-.57f+x*.16f,1.42f+row*.21f,-.038f),new Vector3(.025f,.025f,.006f));
   for(int i=0;i<4;i++){float x=-.43f+i*.28f;float h=i%2==0?.40f:.29f;a.Cube("HangingSpanner274","A29E80",new Vector3(x,1.72f,-.07f),new Vector3(.045f,h,.025f));a.Cube("SpannerJaw274","8C9278",new Vector3(x,1.72f+h*.5f,-.07f),new Vector3(.12f,.08f,.035f));}
   a.Cube("OldHammerHandle274","9C815B",new Vector3(.45f,1.8f,-.07f),new Vector3(.06f,.37f,.05f));a.Cube("OldHammerHead274","4D6255",new Vector3(.45f,2,-.07f),new Vector3(.25f,.095f,.07f));
  }
  static void DecorationPipes274(WorkshopWastelandArt a){
   foreach(float x in new[]{-.52f,.48f}){a.Tube("WallServiceConduit274","797E68",new Vector3(x,.48f,-.05f),new Vector3(x,2.4f,-.05f),.035f);for(int i=0;i<3;i++)a.Cube("PipeBracket274","475E50",new Vector3(x,.7f+i*.65f,-.02f),new Vector3(.16f,.08f,.10f));}
   a.Tube("OverheadCable274","3E5146",new Vector3(-.6f,2.32f,-.07f),new Vector3(.65f,2.32f,-.07f),.022f);
   a.Cube("WallJunctionBox274","637A69",new Vector3(.02f,1.64f,-.06f),new Vector3(.56f,.5f,.13f));a.Cube("JunctionLatch274","B6A784",new Vector3(.18f,1.62f,-.14f),new Vector3(.06f,.17f,.03f));a.Cube("WornVoltagePlate274","AB996F",new Vector3(-.05f,1.7f,-.14f),new Vector3(.19f,.13f,.012f));
  }
  static void DecorationNotice274(WorkshopWastelandArt a){
   a.Cube("OldWallNoticeFrame274","536B5A",new Vector3(0,1.94f,0),new Vector3(1.22f,.82f,.055f));a.Cube("OldNoticeBacking274","8C876B",new Vector3(0,1.94f,-.032f),new Vector3(1.08f,.69f,.02f));
   for(int i=0;i<3;i++){float x=-.34f+i*.34f;a.Cube("PinnedDailyPaper274",i==1?"9EA587":"B9B297",new Vector3(x,1.96f,-.046f),new Vector3(.25f,i==1?.42f:.51f,.012f));for(int line=0;line<3;line++)a.Cube("DailyPaperLines274","797E67",new Vector3(x,1.88f+line*.07f,-.055f),new Vector3(.18f,.012f,.005f));}
   a.Cube("DirectionalMarker274","B4A473",new Vector3(.43f,1.59f,-.042f),new Vector3(.14f,.08f,.025f));
  }
  static void DecorationCleaning274(WorkshopWastelandArt a){
   a.Cube("WallCleaningRail274","6E7865",new Vector3(0,1.84f,0),new Vector3(1.28f,.075f,.08f));
   foreach(float x in new[]{-.4f,0f,.38f}){a.Tube("HangingCleaningHandle274","9B8964",new Vector3(x,.55f,-.04f),new Vector3(x,1.82f,-.04f),.025f);a.Cube("CleaningBrush274",x==0?"77856E":"8F927B",new Vector3(x,.52f,-.05f),new Vector3(.29f,.17f,.12f));}
   a.Cube("HangingWorkCloth274","788274",new Vector3(.51f,1.47f,-.06f),new Vector3(.18f,.45f,.045f));
  }
  static void DecorationReel274(WorkshopWastelandArt a){
   a.Cube("CableReelStand274","506A58",new Vector3(0,.14f,0),new Vector3(1.03f,.28f,.64f));
   a.Tube("CableReelAxle274","859179",new Vector3(-.53f,.48f,0),new Vector3(.53f,.48f,0),.045f);
   for(int end=-1;end<=1;end+=2)for(int i=0;i<16;i++){float t=i*Mathf.PI*2/16,u=(i+1)*Mathf.PI*2/16;a.Tube("ReelWoodRim274","9A8862",new Vector3(end*.42f,.48f+Mathf.Cos(t)*.30f,Mathf.Sin(t)*.30f),new Vector3(end*.42f,.48f+Mathf.Cos(u)*.30f,Mathf.Sin(u)*.30f),.028f);if(i%4==0)a.Tube("ReelSpokes274","857451",new Vector3(end*.42f,.48f,0),new Vector3(end*.42f,.48f+Mathf.Cos(t)*.28f,Mathf.Sin(t)*.28f),.032f);}
   for(int ring=0;ring<7;ring++)for(int i=0;i<12;i++){float t=i*Mathf.PI*2/12,u=(i+1)*Mathf.PI*2/12,x=-.31f+ring*.10f;a.Tube("WoundIndustrialCable274","394E43",new Vector3(x,.48f+Mathf.Cos(t)*.22f,Mathf.Sin(t)*.22f),new Vector3(x,.48f+Mathf.Cos(u)*.22f,Mathf.Sin(u)*.22f),.042f);}
  }
  static void DecorationShelf274(WorkshopWastelandArt a){
   foreach(float x in new[]{-.67f,.67f})foreach(float z in new[]{-.30f,.30f})a.Cube("OldShelfUpright274","577160",new Vector3(x,.84f,z),new Vector3(.07f,1.68f,.07f));
   foreach(float y in new[]{.14f,.81f,1.48f})a.Cube("OldShelfDeck274","817E63",new Vector3(0,y,0),new Vector3(1.42f,.07f,.68f));
   for(int i=0;i<3;i++){float x=i==0?-.32f:.31f,y=i==2?1.1f:.42f;a.Cube("UnsearchableWoodParcel274",i==2?"8D7E5C":"998963",new Vector3(x,y,0),new Vector3(.52f,.49f,.52f));a.Cube("ParcelBand274","5C6652",new Vector3(x,y,-.267f),new Vector3(.06f,.49f,.025f));a.Cube("ParcelPaperTag274","B5AB87",new Vector3(x+.12f,y,-.282f),new Vector3(.15f,.12f,.009f));}
  }
  static void DecorationCabinet274(WorkshopWastelandArt a,bool high){
   float w=high?1.15f:.95f,h=high?1.55f:.9f,d=.62f;
   a.Cube(high?"OldUtilityCabinet274":"LowOfficeCabinet274","647B67",new Vector3(0,h*.5f,0),new Vector3(w,h,d));
   for(int i=0;i<(high?4:3);i++){float y=.15f+i*(high?.35f:.25f);a.Cube("UnsearchableCabinetDrawer274",i%2==0?"819079":"74856E",new Vector3(0,y,-d*.5f-.015f),new Vector3(w-.10f,high?.29f:.20f,.025f));a.Cube("OldDrawerHandle274","B0AC88",new Vector3(0,y,-d*.5f-.039f),new Vector3(.23f,.032f,.026f));}
   a.Cube("CabinetRustPatch274","8B7759",new Vector3(w*.30f,.12f,-d*.5f-.032f),new Vector3(.21f,.07f,.009f));
  }
  static void DecorationDesk274(WorkshopWastelandArt a,bool computer){
   a.Cube("OrdinaryDeskTop274","93896D",new Vector3(0,.80f,0),new Vector3(2.2f,.08f,.85f));
   foreach(float x in new[]{-.98f,.98f})foreach(float z in new[]{-.32f,.32f})a.Cube("OrdinaryDeskLeg274","526B59",new Vector3(x,.38f,z),new Vector3(.075f,.76f,.075f));
   a.Cube("DeskLowerBrace274","67755E",new Vector3(0,.22f,.31f),new Vector3(2.0f,.065f,.055f));
   if(computer){
    a.Cube("DesktopMonitorFoot274","485F50",new Vector3(.25f,.87f,.13f),new Vector3(.29f,.055f,.22f));a.Cube("DesktopMonitorStem274","647664",new Vector3(.25f,.98f,.16f),new Vector3(.065f,.22f,.065f));
    a.Cube("DesktopMonitor274","344C40",new Vector3(.25f,1.21f,.16f),new Vector3(.66f,.43f,.105f));a.Cube("UnlitMonitorScreen274","73877A",new Vector3(.25f,1.22f,.10f),new Vector3(.55f,.32f,.012f));
    a.Cube("OrdinaryKeyboard274","556B59",new Vector3(.25f,.875f,-.19f),new Vector3(.56f,.045f,.18f));for(int i=0;i<7;i++)a.Cube("KeyboardKeyRows274","82917A",new Vector3(.03f+i*.07f,.901f,-.19f),new Vector3(.045f,.008f,.11f));
    a.Cube("OfficeMouse274","4C6554",new Vector3(.71f,.89f,-.19f),new Vector3(.085f,.075f,.13f));a.Cube("DesktopComputerTower274","455E4F",new Vector3(.74f,.30f,.02f),new Vector3(.28f,.56f,.42f));
    for(int i=0;i<5;i++)a.Cube("ComputerVent274","8B967D",new Vector3(.74f,.29f+i*.045f,-.197f),new Vector3(.16f,.013f,.008f));
    a.Tube("DeskCable274","32483C",new Vector3(.29f,.87f,.23f),new Vector3(.73f,.60f,.27f),.012f);a.Tube("DeskCable274","32483C",new Vector3(.73f,.6f,.27f),new Vector3(.73f,.12f,.27f),.012f);
   }else{
    a.Cube("BenchVice274","596F5C",new Vector3(.57f,.98f,.05f),new Vector3(.30f,.28f,.25f));a.Tube("ViceScrew274","A6A383",new Vector3(.30f,.98f,-.02f),new Vector3(.8f,.98f,-.02f),.025f);
    for(int i=0;i<3;i++)a.Cube("BenchLooseParts274","7C8B73",new Vector3(-.31f+i*.16f,.875f,-.02f),new Vector3(.10f,.055f,.17f));
   }
   a.Cube("DeskDocumentFolder274","ABA38A",new Vector3(-.62f,.872f,-.03f),new Vector3(.46f,.045f,.34f));a.Cube("DeskCup274","A0A487",new Vector3(-.84f,.95f,.23f),new Vector3(.13f,.20f,.13f));
  }
  static void DecorationOrientedCube274(WorkshopWastelandArt a,string name,string color,Vector3 p,Vector3 s,Quaternion q){
   var x=q*Vector3.right;var y=q*Vector3.up;var z=q*Vector3.forward;
   a.Panel(name,color,p+z*s.z*.5f,x,y,s.x,s.y);a.Panel(name,color,p-z*s.z*.5f,x,y,s.x,s.y);
   a.Panel(name,color,p+x*s.x*.5f,z,y,s.z,s.y);a.Panel(name,color,p-x*s.x*.5f,z,y,s.z,s.y);
   a.Panel(name,color,p+y*s.y*.5f,x,z,s.x,s.z);a.Panel(name,color,p-y*s.y*.5f,x,z,s.x,s.z);
  }
  static void DecorationChair274(WorkshopWastelandArt a){
   var tilt=Quaternion.Euler(80,0,-12);
   var parts=new List<Vector3>{Vector3.zero,new Vector3(0,.36f,.23f)};
   var sizes=new List<Vector3>{new Vector3(.54f,.065f,.50f),new Vector3(.54f,.49f,.065f)};
   foreach(float x in new[]{-.23f,.23f})foreach(float z in new[]{-.21f,.21f}){parts.Add(new Vector3(x,-.22f,z));sizes.Add(new Vector3(.055f,.44f,.055f));}
   // Ground the actual tilted mesh using all eight transformed corners of every visible part.
   float minY=float.PositiveInfinity;
   for(int i=0;i<parts.Count;i++)foreach(float x in new[]{-1f,1f})foreach(float y in new[]{-1f,1f})foreach(float z in new[]{-1f,1f}){
    var corner=tilt*(parts[i]+Vector3.Scale(sizes[i],new Vector3(x,y,z))*.5f);minY=Mathf.Min(minY,corner.y);
   }
   var origin=new Vector3(0,.01f-minY,0);
   for(int i=0;i<parts.Count;i++)DecorationOrientedCube274(a,i==0?"FallenChairSeat274":i==1?"FallenChairBack274":"FallenChairLeg274",i==0?"89866B":i==1?"798572":"526B59",origin+tilt*parts[i],sizes[i],tilt);
  }
  static void DecorationPaper274(WorkshopWastelandArt a){
   a.Cube("OpenOfficePaperBox274","A38D68",new Vector3(-.16f,.18f,0),new Vector3(.55f,.36f,.52f));a.Cube("OpenPaperBoxInside274","6B705C",new Vector3(-.16f,.364f,0),new Vector3(.47f,.015f,.44f));
   a.Cube("StackedOfficeFolder274","B7B095",new Vector3(-.16f,.40f,.02f),new Vector3(.38f,.075f,.31f));
   a.Cube("SmallClosedPaperBox274","8F825F",new Vector3(.29f,.12f,-.13f),new Vector3(.32f,.24f,.35f));a.Cube("PaperBoxTape274","B4A985",new Vector3(.29f,.248f,-.13f),new Vector3(.055f,.012f,.35f));
   foreach(float x in new[]{-.30f,.12f,.35f}){a.Cube("ScatteredDailyPaper274","B9B296",new Vector3(x,.018f,.29f),new Vector3(.19f,.012f,.13f));a.Cube("DailyPaperPrint274","7A806B",new Vector3(x,.026f,.29f),new Vector3(.13f,.006f,.014f));}
  }
 }
}
