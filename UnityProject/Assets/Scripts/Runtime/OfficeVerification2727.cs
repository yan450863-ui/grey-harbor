using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
namespace GreyHarbor {
 public sealed partial class RaidVerification {
  Vector3 OfficeNormal2727(OfficeOpening2726 opening)=>Convert.ToString(opening.Orientation)=="v"?Vector3.right:Vector3.forward;
  bool OfficeReachable2727(Vector3 point){
   if(!CargoClear2713(point))return false;var path=G.Navigation.FindPath(G.Player.transform.position,point);
   return path.Count>0&&Vector3.Distance(path[path.Count-1],point)<.001f;
  }
  IEnumerator OfficeExteriorCheck2727(){
   var map=G.Map;
   Check(map.HarborWhitebox&&!map.IsInterior&&map.GroundCacheUsed2721&&map.HarborPlanCacheUsed2721,"2727 restored harbor ground and overview use prepared caches");
   Check(map.Bounds==new Rect(-79,-73,208,180),"2727 original 208 by 180 map bounds restored without north extension");
   Check(map.OfficeShells2727.Count==2,"2727 two independent exterior office towers");
   var plotOrigin=RaidMap.PortPoint(60,153);var plot=new Rect(plotOrigin.x,plotOrigin.z,43,21);
   foreach(var shell in map.OfficeShells2727){
    Check(Mathf.Abs(shell.Footprint.width-16)<.01f&&Mathf.Abs(shell.Footprint.height-18)<.01f&&shell.Floors==3&&Mathf.Abs(shell.Height-10.2f)<.01f,"2727 independent compact three-storey exterior shell "+shell.Name);
    Check(plot.Contains(shell.Footprint.min)&&plot.Contains(shell.Footprint.max),"2727 exterior shell fits existing 43 by 21 plot "+shell.Name);
   }
   if(map.OfficeShells2727.Count==2){
    var a=map.OfficeShells2727[0].Footprint;var b=map.OfficeShells2727[1].Footprint;
    Check(!a.Overlaps(b)&&Mathf.Abs(Mathf.Abs(a.center.x-b.center.x)-23)<.01f,"2727 towers remain separated by seven metre ground gap");
   }
   Check(map.OfficeBridge2727!=null,"2727 visible second-floor bridge exists");
   if(map.OfficeBridge2727!=null){
    var rendererBounds=new Bounds();bool first=true;foreach(var r in map.OfficeBridge2727.GetComponentsInChildren<Renderer>()){if(first){rendererBounds=r.bounds;first=false;}else rendererBounds.Encapsulate(r.bounds);}
    Check(!first&&rendererBounds.min.y>=3.2f&&rendererBounds.size.x>=6.9f&&rendererBounds.size.z>=2.9f,"2727 bridge is elevated to floor two with clear ground beneath");
   }
   Check(map.Doors.FindAll(d=>d.Portal&&d.Destination=="officeright1").Count==1&&!map.Doors.Exists(d=>d.Portal&&d.Destination.StartsWith("office")&&d.Destination!="officeright1"),"2727 only right-floor-one portal is active; left and upper interiors not exposed");
   var left=RaidMap.PortPoint(70,155);Check(Physics.CheckSphere(left+Vector3.up,.18f,SceneObstacle.MovementMask,QueryTriggerInteraction.Ignore),"2727 left-tower main entry has actual blocking volume");
   foreach(var p in new[]{RaidMap.PortPoint(58,185),RaidMap.PortPoint(87,200),RaidMap.PortPoint(116,220)})Check(!map.Bounds.Contains(new Vector2(p.x,p.z))&&!map.OnHarborSurface(p),"2727 obsolete northern extended land absent "+p);
   Check(OfficePreview2726.Apply(G)&&Vector3.Distance(G.Player.transform.position,RaidMap.OfficePreviewSpawn2726)<.001f&&CargoClear2713(G.Player.transform.position),"2727 preview starts on original office forecourt");
   FreightRoomPlace2717(RaidMap.PortPoint(83,137),"2727 inherited original office road");
   foreach(var p in new[]{RaidMap.PortPoint(83,150),RaidMap.PortPoint(93,150),RaidMap.OfficePreviewSpawn2726,RaidMap.OfficeArrival2726}){FreightRoomWalk2717(p,"2727 original road to right-tower entry "+p);yield return null;}
   foreach(var p in new[]{RaidMap.PortPoint(81.5f,157),RaidMap.PortPoint(81.5f,167.5f),RaidMap.PortPoint(81.5f,172)}){
    Check(map.OnHarborSurface(p)&&map.HarborSurfaceClear(p,.34f)&&CargoClear2713(p),"2727 between-tower ground remains capsule clear "+p);
    FreightRoomWalk2717(p,"2727 real player walks through tower gap and beneath second-floor bridge");yield return null;
   }
   FreightRoomWalk2717(RaidMap.OfficeArrival2726,"2727 return to active entrance");
   var plan=map.HarborPlanTexture();Check(plan!=null&&map.HarborPlanCacheUsed2721,"2727 actual restored harbor overview remains cache-backed");File.WriteAllBytes(Path.Combine(folder,"2727-office-harbor-plan.png"),plan.EncodeToPNG());
  }
  bool OfficeDoorApproach2727(OfficeOpening2726 opening,out Vector3 stand){
   var normal=OfficeNormal2727(opening);foreach(float distance in new[]{.95f,1.2f})foreach(float sign in new[]{-1f,1f}){
    var point=opening.Position+normal*distance*sign;if(OfficeReachable2727(point)){stand=point;return true;}
   }stand=Vector3.zero;return false;
  }
  IEnumerator OfficeDoors2727(){
   var pending=G.Map.OfficeOpenings2726.FindAll(o=>o.Door!=null&&!o.Door.Portal&&!o.Door.Locked);int rounds=0;
   while(pending.Count>0){bool changed=false;for(int i=pending.Count-1;i>=0;i--){var o=pending[i];if(!OfficeDoorApproach2727(o,out var stand))continue;
     Check(!o.Door.Open,"2727 whitebox door initially closed "+o.Door.Name);FreightRoomWalk2717(stand,"2727 actual reachable door approach "+o.Door.Name);
     FreightMonsterF2720(o.Door,false);Check(o.Door.Open,"2727 actual F opens whitebox door "+o.Door.Name);pending.RemoveAt(i);changed=true;yield return null;
    }Check(changed&&++rounds<100,"2727 iterative real door opening progresses without unreachable rooms; remaining="+pending.Count);
   }
  }
  void OfficeStructure2727(){
   var map=G.Map;Check(map.OfficeWhitebox2726&&map.IsInterior&&map.InteriorId=="officeright1","2727 right tower first-floor independent whitebox map");
   Check(map.OfficeRooms2726.Count==12&&map.OfficeRooms2726.FindAll(r=>r.Stair).Count==2&&map.OfficeRooms2726.FindAll(r=>!r.Stair).Count==10,"2727 accepted 10 functional rooms and two stair spaces");
   Check(G.Enemies.Count==0&&map.Loot.Count==0,"2727 first-floor whitebox has no added monsters or search containers");
   var floor=map.Root.Find("OfficeWhiteboxFloor2726");Check(floor&&floor.GetComponent<Collider>()&&Mathf.Abs(floor.GetComponent<Renderer>().bounds.size.x-40)<.01f&&Mathf.Abs(floor.GetComponent<Renderer>().bounds.size.z-48)<.01f,"2727 actual floor has 40 by 48 metre supporting whitebox geometry");
   var ids=new HashSet<string>();foreach(var room in map.OfficeRooms2726){Check(!string.IsNullOrEmpty(room.Name)&&ids.Add(Convert.ToString(room.Id))&&room.Area.Contains(new Vector2(room.Center.x,room.Center.z))&&CargoClear2713(room.Center),"2727 actual unique room centre is within authored room and capsule-safe "+room.Name);}
   // Physical perimeter: midpoint probes exclude the south entrance at x=0.
   foreach(var p in new[]{new Vector3(-20,1,0),new Vector3(20,1,0),new Vector3(0,1,24),new Vector3(-12,1,-24)})
    Check(Physics.CheckSphere(p,.18f,SceneObstacle.MovementMask,QueryTriggerInteraction.Ignore),"2727 outer wall has real blocking volume "+p);
   int doors=0;foreach(var opening in map.OfficeOpenings2726)if(opening.Door!=null&&!opening.Door.Portal){doors++;Check(opening.Door.InstantToggle&&Mathf.Abs(opening.Door.OpenAngle-90)<.001f,"2727 ordinary F door retains instantaneous right-angle hinge "+opening.Door.Name);var normal=OfficeNormal2727(opening);
    Check(opening.Door.Collider&&opening.Door.Pivot&&Physics.Linecast(opening.Position-normal*.5f+Vector3.up*2.8f,opening.Position+normal*.5f+Vector3.up*2.8f,SceneObstacle.MovementMask,QueryTriggerInteraction.Ignore),"2727 actual room door has collider, hinge and connected solid header "+opening.Door.Name);
   }Check(doors==17,"2727 simplified floor contains exactly 17 ordinary room doors");
   Check(map.OfficeBlockers2726.Count==1,"2727 only one main-corridor blocker authored");
   int stairParts=0,barriers=0;foreach(var t in map.Root.GetComponentsInChildren<Transform>()){
    if(t.name=="OfficeReservedStair2726")stairParts++;if(t.name=="OfficeUpperFloorBarrier2726"){barriers++;Check(t.GetComponent<Collider>()&&t.GetComponent<Collider>().enabled,"2727 reserved stair upper-floor barrier has real collision");}
   }Check(stairParts==20&&barriers==2,"2727 two reserved stairs contain visible physical steps and sealed unfinished upper landings");
  }
  IEnumerator OfficeRoutes2727(){
   foreach(var room in G.Map.OfficeRooms2726){FreightRoomWalk2717(room.Center,"2727 real player reaches "+room.Name);yield return null;}
   int holes=0;foreach(var opening in G.Map.OfficeOpenings2726)if(opening.Door==null&&Mathf.Abs(opening.Position.x)<19.9f&&Mathf.Abs(opening.Position.z)<23.9f){
    holes++;var normal=OfficeNormal2727(opening);var a=opening.Position-normal*.9f;var b=opening.Position+normal*.9f;
    Check(CargoClear2713(a)&&CargoClear2713(b)&&G.Navigation.ClearSegment(a,b),"2727 actual wall-hole capsule crossing is clear "+holes);FreightRoomWalk2717(a,"2727 wall-hole approach "+holes);FreightRoomWalk2717(b,"2727 actual wall-hole crossing "+holes);yield return null;
   }Check(holes==2,"2727 accepted room-wall shortcuts are actual traversable openings");
   int index=0;foreach(var rect in G.Map.OfficeBlockers2726){bool alongZ=rect.width>rect.height;var normal=alongZ?Vector3.forward:Vector3.right;var center=new Vector3(rect.center.x,0,rect.center.y);float half=(alongZ?rect.height:rect.width)*.5f;var a=center-normal*(half+2);var b=center+normal*(half+2);foreach(float clearance in new[]{1f,1.5f,2f,2.5f,3f}){var pa=center-normal*(half+clearance);var pb=center+normal*(half+clearance);if(CargoClear2713(pa)&&CargoClear2713(pb)&&!G.Navigation.ClearSegment(pa,pb)){a=pa;b=pb;break;}}
    foreach(var pp in new[]{a,b})foreach(var cc in Physics.OverlapCapsule(pp+Vector3.up*.35f,pp+Vector3.up,.34f,SceneObstacle.MovementMask,QueryTriggerInteraction.Ignore))Debug.Log("OFFICE BLOCKPOINT "+index+" point="+pp+" object="+cc.name+" bounds="+cc.bounds);Check(CargoClear2713(a),"2727 blocker near probe clear "+index);Check(CargoClear2713(b),"2727 blocker far probe clear "+index);Check(!G.Navigation.ClearSegment(a,b),"2727 authored corridor obstacle actually blocks direct travel "+index);FreightRoomWalk2717(a,"2727 blocker near side "+index);var path=G.Navigation.FindPath(a,b);float length=0;var anchor=a;foreach(var p in path){length+=Vector3.Distance(anchor,p);anchor=p;}
    Check(path.Count>1&&Vector3.Distance(path[path.Count-1],b)<.001f&&length>Vector3.Distance(a,b)+.5f,"2727 actual navigation chooses longer room/corridor bypass "+index);FreightRoomWalk2717(b,"2727 actual player completes obstacle bypass "+index);index++;yield return null;
   }
   foreach(float side in new[]{-4f,5f}){
    var a=new Vector3(0,0,-14);var b=new Vector3(side,0,-14);var c=new Vector3(side,0,-8);var d=new Vector3(0,0,-8);
    FreightRoomWalk2717(a,"2727 alternate bypass starting corridor "+side);
    foreach(var p in new[]{b,c,d}){Check(CargoClear2713(p),"2727 authored left/right room detour stand is capsule safe "+p);FreightRoomWalk2717(p,"2727 actual left/right bypass route "+side);yield return null;}
    Check(G.Navigation.ClearSegment(b,c),"2727 each room-side bypass is continuous along blocker "+side);
   }
   var viewport=new Rect(0,0,220,220);foreach(float yaw in new[]{0f,45f,90f,135f}){
    G.Rig.ResetView(yaw);G.Rig.Tick(0);var projection=new MiniMapProjection(G.Map.Bounds,viewport,G.View.transform.rotation);
    foreach(float x in new[]{G.Map.Bounds.xMin,G.Map.Bounds.xMax})foreach(float z in new[]{G.Map.Bounds.yMin,G.Map.Bounds.yMax})Check(viewport.Contains(projection.Point(new Vector3(x,0,z))),"2727 reduced-floor corners remain inside rotating minimap "+yaw);
   }G.Rig.ResetView(45);G.Rig.Tick(0);
   Check(!G.Map.Doors.Exists(d=>d.Portal&&d!=G.Map.OfficeExit2726),"2727 unfinished upper floors cannot be entered through stair portals");
  }
  IEnumerator OfficePictures2727(){
   FreightRoomWalk2717(G.Map.Spawn,"2727 real entrance camera position");G.Rig.Tick(0);G.Player.Aim=Vector3.forward;G.Vision.Refresh(G.Player.transform.position,G.Player.Aim);G.Player.Draw(G.View,0);yield return null;
   float fog=Shader.GetGlobalFloat("_HarborFogEnabled");var frame=HvRender277(G.View,"2727-office-right1-live-fog.png",1600,900);try{CargoImageCheck2713(frame,"2727 actual gameplay camera whitebox");}finally{Destroy(frame);}Check(Shader.GetGlobalFloat("_HarborFogEnabled")==fog,"2727 actual camera render retains gameplay fog state");
   yield return CargoPicture2715("2727-office-right1-overhead.png",Vector3.zero,0,27,true,1800,1800);
   yield return CargoPicture2715("2727-office-right1-oblique.png",Vector3.zero,35,29,false,1800,1200);
   yield return CargoPicture2715("2727-office-right1-door-header-detail.png",new Vector3(-2,1,-14),35,8,false,1600,1000);
  }
  IEnumerator RunOffice2727(){
   int start=results.Count;var controls=G.Controls;var random=UnityEngine.Random.state;string dir=Path.GetDirectoryName(G.SavePath);string[] formal={"stash-v40.json","stash-v41.json","stash-v42.json"};byte[][] original=Array.ConvertAll(formal,n=>File.Exists(Path.Combine(dir,n))?File.ReadAllBytes(Path.Combine(dir,n)):null);
   Check(Application.version=="2.7.27"&&G.VerifyMode&&G.SavePath.EndsWith("verification-stash-2.7.27.json"),"2727 isolated version-specific right tower whitebox verification");
   try{
    FreshCamp();string inventory=JsonUtility.ToJson(G.Save);G.Controls=new TouchInput();G.HarborWalkthrough=true;yield return G.BeginTrainingLoad2721();G.Paused=true;var outdoor=G.Map;var actors=G.Enemies.ToArray();int boxes=outdoor.Loot.Count;
    yield return OfficeExteriorCheck2727();var arrival=RaidMap.OfficeArrival2726;var portal=outdoor.Doors.Find(d=>d.Portal&&d.Destination=="officeright1");
    Check(portal!=null,"2727 right-tower main door is connected to real first-floor map");yield return CargoPicture2715("2727-office-right-exterior-whitebox.png",RaidMap.PortPoint(81.5f,164),35,29,false,1800,1200);
    FreightMonsterF2720(portal,true);yield return null;var indoor=G.Map;OfficeStructure2727();Check(Array.TrueForAll(actors,e=>!e.gameObject.activeInHierarchy),"2727 original exterior actors are suspended indoors");yield return OfficeDoors2727();yield return OfficeRoutes2727();yield return OfficePictures2727();
    // Preserve a mixed open/closed state rather than merely checking all-open defaults.
    var remembered=new Dictionary<WorldDoor,bool>();var closable=indoor.OfficeOpenings2726.Find(o=>o.Door!=null&&!o.Door.Portal&&!o.Door.Locked&&o.Door.Open&&OfficeDoorApproach2727(o,out var unused));
    Check(closable!=null,"2727 an actual reachable ordinary door is available for cache state variation");OfficeDoorApproach2727(closable,out var closeStand);FreightRoomWalk2717(closeStand,"2727 actual close-door approach");FreightMonsterF2720(closable.Door,false);Check(!closable.Door.Open,"2727 real F closes selected door before cache test");foreach(var d in indoor.Doors)remembered.Add(d,d.Open);
    FreightRoomWalk2717(indoor.Spawn,"2727 real interior return to main exit");FreightMonsterF2720(indoor.OfficeExit2726,true);yield return null;
    Check(G.Map==outdoor&&Vector3.Distance(G.Player.transform.position,arrival)<.1f&&G.Enemies.Count==actors.Length&&outdoor.Loot.Count==boxes&&Array.TrueForAll(actors,e=>G.Enemies.Contains(e)),"2727 actual held F restores corresponding exterior and original actors/containers");
    FreightMonsterF2720(portal,true);yield return null;Check(G.Map==indoor&&G.Enemies.Count==0&&G.Map.Loot.Count==0,"2727 re-entry restores same first-floor map without content generation");foreach(var pair in remembered)Check(indoor.Doors.Contains(pair.Key)&&pair.Key.Open==pair.Value,"2727 cached original mixed door state preserved "+pair.Key.Name);
    FreightRoomWalk2717(indoor.Spawn,"2727 cached exit approach");FreightMonsterF2720(indoor.OfficeExit2726,true);yield return null;Check(G.Map==outdoor&&G.Enemies.Count==actors.Length&&outdoor.Loot.Count==boxes,"2727 repeated portal return does not duplicate existing content");
    G.Finish(false);yield return null;Check(G.Phase==GamePhase.Base&&JsonUtility.ToJson(G.Save)==inventory,"2727 isolated preview restores original camp inventory");for(int i=0;i<formal.Length;i++)Check(MainBytes273(original[i],File.Exists(Path.Combine(dir,formal[i]))?File.ReadAllBytes(Path.Combine(dir,formal[i])):null),"2727 formal save unchanged "+formal[i]);
   }finally{G.ClearShots();G.CloseInventory();G.Controls=controls;UnityEngine.Random.state=random;G.Paused=true;var report=new List<string>();for(int i=start;i<results.Count;i++)report.Add(results[i]);report.Add("INFO Scope: right-tower first-floor 40x48 whitebox with ten functional rooms and two stairs; two separated compact 16x18 exterior shells and elevated floor-two bridge. Restored original bounds, inherited road Actor.Move, bridge ground clearance, prepared ground/plan caches and rotating minimap; real held-F portals, all seventeen ordinary F doors, every room centre, two wall holes, one blocked corridor with both bypasses, headers, sealed upper access, map cache, unchanged formal saves, and actual Camera.Render. No unrelated economy/art/full-map suites.");File.WriteAllLines(Path.Combine(folder,"2727-office-right1-targeted-verification.txt"),report);}
  }
 }
}




