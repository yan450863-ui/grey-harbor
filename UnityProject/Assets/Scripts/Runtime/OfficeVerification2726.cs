using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
namespace GreyHarbor {
 public sealed partial class RaidVerification {
  Vector3 OfficeNormal2726(OfficeOpening2726 opening)=>Convert.ToString(opening.Orientation)=="v"?Vector3.right:Vector3.forward;
  bool OfficeReachable2726(Vector3 point){
   if(!CargoClear2713(point))return false;var path=G.Navigation.FindPath(G.Player.transform.position,point);
   return path.Count>0&&Vector3.Distance(path[path.Count-1],point)<.001f;
  }
  IEnumerator OfficeExteriorCheck2726(){
   var map=G.Map;Check(map.HarborWhitebox&&!map.IsInterior&&map.GroundCacheUsed2721&&map.HarborPlanCacheUsed2721,"2726 actual expanded harbor ground and overview use prepared caches");
   var building=map.MainBuildings.Find(b=>b.Name=="办公右塔一楼");Check(building!=null&&Mathf.Abs(building.Footprint.width-54)<.01f&&Mathf.Abs(building.Footprint.height-64)<.01f&&building.Floors==1&&Vector3.Distance(building.Approach,RaidMap.OfficeArrival2726)<.001f,"2726 actual main-building record is the right first-storey 54 by 64 shell");
   Check(!map.MainBuildings.Exists(b=>Mathf.Abs(b.Footprint.width-43)<.01f&&Mathf.Abs(b.Footprint.height-21)<.01f),"2726 obsolete 43 by 21 office placeholder is removed");
   Check(OfficePreview2726.Apply(G)&&Vector3.Distance(G.Player.transform.position,RaidMap.OfficePreviewSpawn2726)<.001f&&CargoClear2713(G.Player.transform.position),"2726 actual office preview uses its authored safe start");
   foreach(float x in new[]{58f,116f}){
    for(float z=180;z<=220;z+=5){var p=RaidMap.PortPoint(x,z);Check(map.Bounds.Contains(new Vector2(p.x,p.z))&&map.OnHarborSurface(p)&&map.HarborSurfaceClear(p,.34f)&&CargoClear2713(p),"2726 enlarged office-side land covers bounds and full player capsule without coastline obstruction x="+x+" z="+z);}
    var a=RaidMap.PortPoint(x,172);var b=RaidMap.PortPoint(x,178);Check(CargoClear2713(a)&&CargoClear2713(b)&&G.Navigation.ClearSegment(a,b),"2726 old/new land union has no internal coastline fence at z174 x="+x);
   }
   foreach(var p in new[]{RaidMap.PortPoint(56,200),RaidMap.PortPoint(118,200),RaidMap.PortPoint(87,222)})Check(Physics.CheckSphere(p+Vector3.up*.4f,.18f,SceneObstacle.MovementMask,QueryTriggerInteraction.Ignore),"2726 enlarged exposed land edge retains real protective coast collision "+p);
   foreach(var p in new[]{RaidMap.PortPoint(55.5f,200),RaidMap.PortPoint(118.5f,200),RaidMap.PortPoint(87,222.5f)})Check(!map.OnHarborSurface(p),"2726 enlarged land does not silently bridge beyond exposed boundary "+p);
   FreightRoomPlace2717(RaidMap.PortPoint(83,137),"2726 original office approach road fixture");FreightRoomWalk2717(RaidMap.OfficePreviewSpawn2726,"2726 actual old road to new office forecourt");FreightRoomWalk2717(RaidMap.OfficeArrival2726,"2726 actual continued old road to right-tower main entry");yield return null;
  }
  bool OfficeDoorApproach2726(OfficeOpening2726 opening,out Vector3 stand){
   var normal=OfficeNormal2726(opening);foreach(float distance in new[]{.95f,1.2f})foreach(float sign in new[]{-1f,1f}){
    var point=opening.Position+normal*distance*sign;if(OfficeReachable2726(point)){stand=point;return true;}
   }stand=Vector3.zero;return false;
  }
  IEnumerator OfficeDoors2726(){
   var pending=G.Map.OfficeOpenings2726.FindAll(o=>o.Door!=null&&!o.Door.Portal&&!o.Door.Locked);int rounds=0;
   while(pending.Count>0){bool changed=false;for(int i=pending.Count-1;i>=0;i--){var o=pending[i];if(!OfficeDoorApproach2726(o,out var stand))continue;
     Check(!o.Door.Open,"2726 whitebox door initially closed "+o.Door.Name);FreightRoomWalk2717(stand,"2726 actual reachable door approach "+o.Door.Name);
     FreightMonsterF2720(o.Door,false);Check(o.Door.Open,"2726 actual F opens whitebox door "+o.Door.Name);pending.RemoveAt(i);changed=true;yield return null;
    }Check(changed&&++rounds<100,"2726 iterative real door opening progresses without unreachable rooms; remaining="+pending.Count);
   }
  }
  void OfficeStructure2726(){
   var map=G.Map;Check(map.OfficeWhitebox2726&&map.IsInterior&&map.InteriorId=="officeright1","2726 right tower first-floor independent whitebox map");
   Check(map.OfficeRooms2726.Count==22&&map.OfficeRooms2726.FindAll(r=>r.Stair).Count==2&&map.OfficeRooms2726.FindAll(r=>!r.Stair).Count==20,"2726 accepted 20 functional rooms and two stair spaces");
   Check(G.Enemies.Count==0&&map.Loot.Count==0,"2726 first-floor whitebox has no added monsters or search containers");
   var floor=map.Root.Find("OfficeWhiteboxFloor2726");Check(floor&&floor.GetComponent<Collider>()&&Mathf.Abs(floor.GetComponent<Renderer>().bounds.size.x-54)<.01f&&Mathf.Abs(floor.GetComponent<Renderer>().bounds.size.z-64)<.01f,"2726 actual floor has 54 by 64 metre supporting whitebox geometry");
   var ids=new HashSet<string>();foreach(var room in map.OfficeRooms2726){Check(!string.IsNullOrEmpty(room.Name)&&ids.Add(Convert.ToString(room.Id))&&room.Area.Contains(new Vector2(room.Center.x,room.Center.z))&&CargoClear2713(room.Center),"2726 actual unique room centre is within authored room and capsule-safe "+room.Name);}
   // Physical perimeter: midpoint probes exclude the south entrance at x=0.
   foreach(var p in new[]{new Vector3(-27,1,0),new Vector3(27,1,0),new Vector3(0,1,32),new Vector3(-12,1,-32)})
    Check(Physics.CheckSphere(p,.18f,SceneObstacle.MovementMask,QueryTriggerInteraction.Ignore),"2726 outer wall has real blocking volume "+p);
   int doors=0;foreach(var opening in map.OfficeOpenings2726)if(opening.Door!=null&&!opening.Door.Portal){doors++;var normal=OfficeNormal2726(opening);
    Check(opening.Door.Collider&&opening.Door.Pivot&&Physics.Linecast(opening.Position-normal*.5f+Vector3.up*2.8f,opening.Position+normal*.5f+Vector3.up*2.8f,SceneObstacle.MovementMask,QueryTriggerInteraction.Ignore),"2726 actual room door has collider, hinge and connected solid header "+opening.Door.Name);
   }Check(doors>20,"2726 many-room maze contains actual door connections");
   Check(map.OfficeBlockers2726.Count==5,"2726 three main-corridor and two cross-corridor blockers authored");
   int stairParts=0,barriers=0;foreach(var t in map.Root.GetComponentsInChildren<Transform>()){
    if(t.name=="OfficeReservedStair2726")stairParts++;if(t.name=="OfficeUpperFloorBarrier2726"){barriers++;Check(t.GetComponent<Collider>()&&t.GetComponent<Collider>().enabled,"2726 reserved stair upper-floor barrier has real collision");}
   }Check(stairParts==20&&barriers==2,"2726 two reserved stairs contain visible physical steps and sealed unfinished upper landings");
  }
  IEnumerator OfficeRoutes2726(){
   foreach(var room in G.Map.OfficeRooms2726){FreightRoomWalk2717(room.Center,"2726 real player reaches "+room.Name);yield return null;}
   int holes=0;foreach(var opening in G.Map.OfficeOpenings2726)if(opening.Door==null&&Mathf.Abs(opening.Position.x)<26.9f&&Mathf.Abs(opening.Position.z)<31.9f){
    holes++;var normal=OfficeNormal2726(opening);var a=opening.Position-normal*.9f;var b=opening.Position+normal*.9f;
    Check(CargoClear2713(a)&&CargoClear2713(b)&&G.Navigation.ClearSegment(a,b),"2726 actual wall-hole capsule crossing is clear "+holes);FreightRoomWalk2717(a,"2726 wall-hole approach "+holes);FreightRoomWalk2717(b,"2726 actual wall-hole crossing "+holes);yield return null;
   }Check(holes>=6,"2726 accepted room-wall shortcuts are actual traversable openings");
   int index=0;foreach(var rect in G.Map.OfficeBlockers2726){bool alongZ=rect.width>rect.height;var normal=alongZ?Vector3.forward:Vector3.right;var center=new Vector3(rect.center.x,0,rect.center.y);float half=(alongZ?rect.height:rect.width)*.5f;var a=center-normal*(half+2);var b=center+normal*(half+2);foreach(float clearance in new[]{1f,1.5f,2f,2.5f,3f}){var pa=center-normal*(half+clearance);var pb=center+normal*(half+clearance);if(CargoClear2713(pa)&&CargoClear2713(pb)&&!G.Navigation.ClearSegment(pa,pb)){a=pa;b=pb;break;}}
    foreach(var pp in new[]{a,b})foreach(var cc in Physics.OverlapCapsule(pp+Vector3.up*.35f,pp+Vector3.up,.34f,SceneObstacle.MovementMask,QueryTriggerInteraction.Ignore))Debug.Log("OFFICE BLOCKPOINT "+index+" point="+pp+" object="+cc.name+" bounds="+cc.bounds);Check(CargoClear2713(a),"2726 blocker near probe clear "+index);Check(CargoClear2713(b),"2726 blocker far probe clear "+index);Check(!G.Navigation.ClearSegment(a,b),"2726 authored corridor obstacle actually blocks direct travel "+index);FreightRoomWalk2717(a,"2726 blocker near side "+index);var path=G.Navigation.FindPath(a,b);float length=0;var anchor=a;foreach(var p in path){length+=Vector3.Distance(anchor,p);anchor=p;}
    Check(path.Count>1&&Vector3.Distance(path[path.Count-1],b)<.001f&&length>Vector3.Distance(a,b)+.5f,"2726 actual navigation chooses longer room/corridor bypass "+index);FreightRoomWalk2717(b,"2726 actual player completes obstacle bypass "+index);index++;yield return null;
   }
   Check(!G.Map.Doors.Exists(d=>d.Portal&&d!=G.Map.OfficeExit2726),"2726 unfinished upper floors cannot be entered through stair portals");
  }
  IEnumerator OfficePictures2726(){
   FreightRoomWalk2717(G.Map.Spawn,"2726 real entrance camera position");G.Rig.Tick(0);G.Player.Aim=Vector3.forward;G.Vision.Refresh(G.Player.transform.position,G.Player.Aim);G.Player.Draw(G.View,0);yield return null;
   float fog=Shader.GetGlobalFloat("_HarborFogEnabled");var frame=HvRender277(G.View,"2726-office-right1-live-fog.png",1600,900);try{CargoImageCheck2713(frame,"2726 actual gameplay camera whitebox");}finally{Destroy(frame);}Check(Shader.GetGlobalFloat("_HarborFogEnabled")==fog,"2726 actual camera render retains gameplay fog state");
   yield return CargoPicture2715("2726-office-right1-overhead.png",Vector3.zero,0,35,true,1800,1800);
   yield return CargoPicture2715("2726-office-right1-oblique.png",Vector3.zero,35,37,false,1800,1200);
   yield return CargoPicture2715("2726-office-right1-door-header-detail.png",new Vector3(0,1,-13),35,8,false,1600,1000);
  }
  IEnumerator RunOffice2726(){
   int start=results.Count;var controls=G.Controls;var random=UnityEngine.Random.state;string dir=Path.GetDirectoryName(G.SavePath);string[] formal={"stash-v40.json","stash-v41.json","stash-v42.json"};byte[][] original=Array.ConvertAll(formal,n=>File.Exists(Path.Combine(dir,n))?File.ReadAllBytes(Path.Combine(dir,n)):null);
   Check(Application.version=="2.7.26"&&G.VerifyMode&&G.SavePath.EndsWith("verification-stash-2.7.26.json"),"2726 isolated version-specific right tower whitebox verification");
   try{
    FreshCamp();string inventory=JsonUtility.ToJson(G.Save);G.Controls=new TouchInput();G.HarborWalkthrough=true;yield return G.BeginTrainingLoad2721();G.Paused=true;var outdoor=G.Map;var actors=G.Enemies.ToArray();int boxes=outdoor.Loot.Count;
    yield return OfficeExteriorCheck2726();var arrival=RaidMap.OfficeArrival2726;var portal=outdoor.Doors.Find(d=>d.Portal&&d.Destination=="officeright1");
    Check(portal!=null,"2726 right-tower main door is connected to real first-floor map");yield return CargoPicture2715("2726-office-right-exterior-whitebox.png",RaidMap.PortPoint(87,180),35,40,false,1800,1200);
    FreightMonsterF2720(portal,true);yield return null;var indoor=G.Map;OfficeStructure2726();Check(Array.TrueForAll(actors,e=>!e.gameObject.activeInHierarchy),"2726 original exterior actors are suspended indoors");yield return OfficeDoors2726();yield return OfficeRoutes2726();yield return OfficePictures2726();
    // Preserve a mixed open/closed state rather than merely checking all-open defaults.
    var remembered=new Dictionary<WorldDoor,bool>();var closable=indoor.OfficeOpenings2726.Find(o=>o.Door!=null&&!o.Door.Portal&&!o.Door.Locked&&o.Door.Open&&OfficeDoorApproach2726(o,out var unused));
    Check(closable!=null,"2726 an actual reachable ordinary door is available for cache state variation");OfficeDoorApproach2726(closable,out var closeStand);FreightRoomWalk2717(closeStand,"2726 actual close-door approach");FreightMonsterF2720(closable.Door,false);Check(!closable.Door.Open,"2726 real F closes selected door before cache test");foreach(var d in indoor.Doors)remembered.Add(d,d.Open);
    FreightRoomWalk2717(indoor.Spawn,"2726 real interior return to main exit");FreightMonsterF2720(indoor.OfficeExit2726,true);yield return null;
    Check(G.Map==outdoor&&Vector3.Distance(G.Player.transform.position,arrival)<.1f&&G.Enemies.Count==actors.Length&&outdoor.Loot.Count==boxes&&Array.TrueForAll(actors,e=>G.Enemies.Contains(e)),"2726 actual held F restores corresponding exterior and original actors/containers");
    FreightMonsterF2720(portal,true);yield return null;Check(G.Map==indoor&&G.Enemies.Count==0&&G.Map.Loot.Count==0,"2726 re-entry restores same first-floor map without content generation");foreach(var pair in remembered)Check(indoor.Doors.Contains(pair.Key)&&pair.Key.Open==pair.Value,"2726 cached original mixed door state preserved "+pair.Key.Name);
    FreightRoomWalk2717(indoor.Spawn,"2726 cached exit approach");FreightMonsterF2720(indoor.OfficeExit2726,true);yield return null;Check(G.Map==outdoor&&G.Enemies.Count==actors.Length&&outdoor.Loot.Count==boxes,"2726 repeated portal return does not duplicate existing content");
    G.Finish(false);yield return null;Check(G.Phase==GamePhase.Base&&JsonUtility.ToJson(G.Save)==inventory,"2726 isolated preview restores original camp inventory");for(int i=0;i<formal.Length;i++)Check(MainBytes273(original[i],File.Exists(Path.Combine(dir,formal[i]))?File.ReadAllBytes(Path.Combine(dir,formal[i])):null),"2726 formal save unchanged "+formal[i]);
   }finally{G.ClearShots();G.CloseInventory();G.Controls=controls;UnityEngine.Random.state=random;G.Paused=true;var report=new List<string>();for(int i=start;i<results.Count;i++)report.Add(results[i]);report.Add("INFO Scope: right-tower first-floor 54x64 whitebox and directly affected northern land extension only. Actual preview spawn, inherited road Actor.Move, prepared ground/plan caches, land-union capsule samples and protective exposed edges; real held-F portals, iterative real-F room doors, Actor.Move to every room, six wall holes, five blocked corridors with actual bypasses, door headers, sealed upper-floor access, map cache, unchanged formal saves, and actual Camera.Render. No unrelated economy/art/full-map suites.");File.WriteAllLines(Path.Combine(folder,"2726-office-right1-targeted-verification.txt"),report);}
  }
 }
}



