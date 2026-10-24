using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using UnityEngine;
namespace GreyHarbor {
 public sealed partial class RaidVerification {
  static string CorridorNumber277(float value)=>value.ToString("0.000",CultureInfo.InvariantCulture);
  static Rect CorridorExpand277(Rect r,float n)=>new Rect(r.xMin-n,r.yMin-n,r.width+2*n,r.height+2*n);
  static bool CorridorNear277(Rect r,Vector3 p,float radius){float x=p.x-Mathf.Clamp(p.x,r.xMin,r.xMax),z=p.z-Mathf.Clamp(p.z,r.yMin,r.yMax);return x*x+z*z<radius*radius;}
  bool CorridorVolume277(GameObject view,out Bounds bounds){
   bounds=new Bounds();bool any=false;foreach(var r in view.GetComponentsInChildren<Renderer>())if(r.enabled){if(!any){bounds=r.bounds;any=true;}else bounds.Encapsulate(r.bounds);}return any;
  }
  float CorridorRearGap277(CorridorItem277 item,Bounds bounds){
   if(item.BackingSide<0||item.BackingSide>3)return -1;
   var outwards=item.BackingSide==0?Vector3.forward:item.BackingSide==1?Vector3.right:item.BackingSide==2?Vector3.back:Vector3.left;
   var edge=new Vector3(item.BackingSide==1?bounds.max.x:item.BackingSide==3?bounds.min.x:bounds.center.x,Mathf.Clamp(bounds.center.y,.20f,2.4f),item.BackingSide==0?bounds.max.z:item.BackingSide==2?bounds.min.z:bounds.center.z);
   float gap=float.PositiveInfinity;foreach(var hit in Physics.RaycastAll(edge,outwards,.45f,SceneObstacle.SightMask,QueryTriggerInteraction.Ignore))if(hit.collider.name=="InteriorWall")gap=Mathf.Min(gap,hit.distance);
   Check(!float.IsInfinity(gap)&&gap>=(item.WallMounted?-.002f:.045f)&&gap<=(item.WallMounted?.20f:.16f),"277 New corridor model backs onto an actual wall "+item.Name+" gap="+gap);return gap;
  }
  void CorridorItemsCheck277(List<string> rows,HashSet<string> types){
   var map=G.Map;Physics.SyncTransforms();Check(map.CorridorItems277.Count>0,"277 Runtime interior contains the new corridor dressing "+map.InteriorId);
   int physical=0;
   foreach(var item in map.CorridorItems277){
    Bounds b=new Bounds();Check(item.View&&CorridorVolume277(item.View,out b)&&b.size.magnitude>.1f,"277 New corridor model has actual rendered geometry "+item.Name);types.Add(item.Type.ToString());
    Check(item.View.GetComponentsInChildren<ContainerProp>().Length==0&&item.View.GetComponentsInChildren<RaidActor>().Length==0,"277 New corridor dressing has no search prop or actor component "+item.Name);
    int bodies=0,highBodies=0,lowBodies=0;foreach(var c in item.View.GetComponentsInChildren<Collider>())if(c.enabled&&(SceneObstacle.MovementMask&(1<<c.gameObject.layer))!=0){
     bodies++;if(c.gameObject.layer==SceneObstacle.HighLayer)highBodies++;else lowBodies++;
     var rule=c.GetComponent<SceneObstacle>();Check(rule&&rule.Kind!=ObstacleKind.Decoration&&c.gameObject.layer==(rule.Kind==ObstacleKind.High?SceneObstacle.HighLayer:SceneObstacle.LowLayer),"277 New corridor component uses its own declared high/low geometry collision "+item.Name+" / "+c.name);
     var bounds=c.bounds;var footprint=CorridorExpand277(item.Footprint,.025f);
     Check(bounds.min.x>=footprint.xMin&&bounds.max.x<=footprint.xMax&&bounds.min.z>=footprint.yMin&&bounds.max.z<=footprint.yMax,"277 Declared footprint contains real new collider "+item.Name);
    }
    if(item.Kind==ObstacleKind.Decoration)Check(bodies==0,"277 Pure corridor decoration has no movement collision "+item.Name);
    if(item.Kind!=ObstacleKind.Decoration){
     physical++;Check(bodies>0&&b.size.x>.02f&&b.size.y>.02f&&b.size.z>.02f,"277 Corridor floor object has real volume and collision "+item.Name);
     Check(item.Kind==ObstacleKind.High?highBodies>0:lowBodies>0&&highBodies==0,"277 Whole-model collision classification matches actual component cover "+item.Name);
     foreach(var connection in map.CorridorProtectedConnections277)Check(!item.Footprint.Overlaps(connection),"277 New object leaves existing narrow connection unchanged "+item.Name);
     foreach(var lane in map.CorridorLanes277)Check(!item.Footprint.Overlaps(lane.Area),"277 New object stays outside reserved main walking strip "+item.Name+" / "+lane.Name);
     foreach(var door in map.Doors)if(!door.Portal)Check(!item.Footprint.Overlaps(door.OpenSafety)&&!item.Footprint.Overlaps(door.ClosedSafety),"277 New object avoids door swing and shut footprints "+item.Name+" / "+door.Name);
     foreach(var door in map.Doors)if(door.Portal)Check(!CorridorNear277(item.Footprint,door.Position,.85f),"277 New object preserves entrance/stair portal approach "+item.Name+" / "+door.Name);
     foreach(var box in map.Loot)Check(!CorridorNear277(item.Footprint,box.Position,.34f),"277 New object does not occupy a search capsule stand "+item.Name+" / "+box.Name);
     if(map.RepairMain220)foreach(bool north in new[]{true,false})Check(!CorridorNear277(item.Footprint,RaidMap.RepairStairArrival220(north),.75f),"277 New object leaves actual staircase arrival clear "+item.Name);
    }
    float gap=CorridorRearGap277(item,b);
    rows.Add(Csv230(map.InteriorId)+","+map.RepairFloor220+","+Csv230(item.Name)+","+item.Type+","+item.Kind+","+item.WallMounted+","+bodies+","+CorridorNumber277(item.Footprint.xMin)+","+CorridorNumber277(item.Footprint.yMin)+","+CorridorNumber277(item.Footprint.width)+","+CorridorNumber277(item.Footprint.height)+","+CorridorNumber277(gap));
   }
   Check(physical>0,"277 Each affected corridor includes new physical obstacles "+map.InteriorId);
   foreach(var box in map.Loot)Check(Clear140(box.Position),"277 Existing search stand remains physically clear after new corridor dressing "+map.InteriorId+" / "+box.Name);
   foreach(var room in map.RepairMainRooms210)Check(Clear140(room.ApproachPoint),"277 Existing main room door approach remains clear "+room.Name);
  }
  void CorridorNearbyPatrol277(List<string> rows){
   foreach(var actor in G.Enemies){
    var targets=actor.PatrolTargets;if(targets==null||targets.Length==0)targets=new[]{actor.Home};var previous=actor.Home;
    for(int i=0;i<=targets.Length;i++){
     var target=i==targets.Length?actor.Home:targets[i];var around=Rect.MinMaxRect(Mathf.Min(previous.x,target.x)-.85f,Mathf.Min(previous.z,target.z)-.85f,Mathf.Max(previous.x,target.x)+.85f,Mathf.Max(previous.z,target.z)+.85f);
     bool near=G.Map.CorridorItems277.Exists(p=>p.Kind!=ObstacleKind.Decoration&&p.Footprint.Overlaps(around));
     if(near){Check(Clear140(previous)&&Clear140(target)&&G.Navigation.ClearSegment(previous,target),"277 Nearby existing NPC waypoint and segment remain clear "+actor.name+" #"+i);rows.Add(Csv230(G.Map.InteriorId)+","+Csv230(actor.name)+","+i+","+CorridorNumber277(previous.x)+","+CorridorNumber277(previous.z)+","+CorridorNumber277(target.x)+","+CorridorNumber277(target.z)+",true");}
     previous=target;
    }
   }
  }
  void CorridorLanesCheck277(List<string> rows){
   Check(G.Map.CorridorLanes277.Count>0,"277 Affected interior declares its representative main corridor "+G.Map.InteriorId);
   foreach(var lane in G.Map.CorridorLanes277){
    Check(lane.ClearWidth>=1.8f-.001f&&lane.Points!=null&&lane.Points.Length>=2,"277 Newly reserved main corridor is at least 1.8m wide "+lane.Name);
    const float half=.9f;int samples=0;float length=0;
    for(int i=0;i<lane.Points.Length;i++){
     var p=lane.Points[i];Check(!Physics.CheckCapsule(p+Vector3.up*.91f,p+Vector3.up,half,SceneObstacle.MovementMask,QueryTriggerInteraction.Ignore),"277 Actual physical 1.8m main corridor cross-section is open "+lane.Name+" #"+i);
     if(i==0)continue;var a=lane.Points[i-1];var delta=p-a;length+=delta.magnitude;
     Check(delta.sqrMagnitude<.000001f||!Physics.CapsuleCast(a+Vector3.up*.91f,a+Vector3.up,half,delta.normalized,delta.magnitude,SceneObstacle.MovementMask,QueryTriggerInteraction.Ignore),"277 Actual main corridor retains 1.8m width over its full straight segment "+lane.Name+" #"+i);
     int n=Mathf.Max(1,Mathf.CeilToInt(delta.magnitude/.5f));for(int k=0;k<=n;k++){var point=Vector3.Lerp(a,p,k/(float)n);Check(!Physics.CheckCapsule(point+Vector3.up*.91f,point+Vector3.up,half,SceneObstacle.MovementMask,QueryTriggerInteraction.Ignore),"277 Main corridor half-metre width probe "+lane.Name+" #"+samples);samples++;}
    }
    MainWalk273(lane.Points[0],"277 representative public corridor start "+lane.Name);
    for(int i=1;i<lane.Points.Length;i++){Check(G.Navigation.ClearSegment(G.Player.transform.position,lane.Points[i]),"277 Actual .34m public corridor travel segment is clear "+lane.Name);MainWalk273(lane.Points[i],"277 representative corridor actual Actor.Move "+lane.Name+" #"+i);}
    rows.Add(Csv230(G.Map.InteriorId)+","+Csv230(lane.Name)+","+CorridorNumber277(lane.ClearWidth)+",1.8,"+samples+","+CorridorNumber277(length)+",true");
   }
  }
  IEnumerator CorridorPicture277(string filename){
   var item=G.Map.CorridorItems277.Find(p=>p.Kind!=ObstacleKind.Decoration);Check(item!=null,"277 Corridor close-view has a real new physical object "+filename);
   var r=item.Footprint;bool reached=false;foreach(var p in new[]{new Vector3(r.center.x,0,r.yMin-1.15f),new Vector3(r.center.x,0,r.yMax+1.15f),new Vector3(r.xMin-1.15f,0,r.center.y),new Vector3(r.xMax+1.15f,0,r.center.y)}){
    if(!Clear140(p))continue;var route=G.Navigation.FindPath(G.Player.transform.position,p);if(route.Count==0)continue;MainWalk273(p,"277 new corridor object photograph stand");reached=true;break;
   }
   Check(reached,"277 Actual player reaches corridor photograph stand "+filename);G.ResetAkHandling276();yield return RoomsGameView276(filename,item.Anchor,45);
  }
  public IEnumerator VerifyCorridor277(){
   var previousInput=G.Controls;var itemRows=new List<string>{"interior,floor,name,type,kind,mounted,physical_colliders,x,z,width,depth,actual_wall_gap_m"};var laneRows=new List<string>{"interior,lane,declared_width_m,actual_width_checked_m,samples,length_m,actor_move_passed"};var patrolRows=new List<string>{"interior,actor,segment,from_x,from_z,to_x,to_z,clear"};var types=new HashSet<string>();
   try {
    FreshCamp();G.HarborWalkthrough=true;Check(G.BeginTraining(),"277 Corridor fixture enters real harbor training session");G.Paused=true;G.Controls=new TouchInput();yield return null;
    G.SwitchInterior("repairmain1");G.Paused=true;yield return null;
    for(int floor=1;floor<=4;floor++){
     Check(G.Map.RepairMain220&&G.Map.RepairFloor220==floor,"277 New corridor is checked on actual main floor "+floor);MainArrivalDoor273();
     CorridorItemsCheck277(itemRows,types);CorridorNearbyPatrol277(patrolRows);CorridorLanesCheck277(laneRows);
     if(floor==3)yield return CorridorPicture277("277-corridor-main-floor3-game-view.png");
     if(floor<4){G.SwitchInterior("repair-floor:"+(floor+1)+":north");G.Paused=true;yield return null;}
    }
    G.SwitchInterior(G.Map.InteriorId);G.Paused=true;yield return null;G.SwitchInterior("repairannex");G.Paused=true;yield return null;
    Check(G.Map.InteriorId=="repairannex","277 New dressing is checked in the actual annex interior");CorridorItemsCheck277(itemRows,types);CorridorNearbyPatrol277(patrolRows);CorridorLanesCheck277(laneRows);yield return CorridorPicture277("277-corridor-annex-game-view.png");
    Check(types.Count>=4,"277 New corridor dressing provides several distinct model types across affected interiors");
    File.WriteAllLines(Path.Combine(folder,"277-corridor-new-models.csv"),itemRows);File.WriteAllLines(Path.Combine(folder,"277-corridor-width-and-travel.csv"),laneRows);File.WriteAllLines(Path.Combine(folder,"277-corridor-nearby-patrol.csv"),patrolRows);
   }finally{G.Controls=previousInput;G.Paused=true;G.InventoryOpen=false;}
  }
 }
}
