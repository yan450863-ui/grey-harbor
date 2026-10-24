using System;
using System.Collections;
using System.IO;
using UnityEngine;
namespace GreyHarbor {
 public sealed partial class RaidVerification {
  // Focused first-floor acceptance: doorway contract, usable routes, local fog and persistent rooms.
  IEnumerator VerifyRepairMain210(){
   FreshCamp();string saved=JsonUtility.ToJson(G.Save);
   string formalPath=Path.Combine(Path.GetDirectoryName(G.SavePath),"stash-v40.json");
   string formalBefore=File.Exists(formalPath)?File.ReadAllText(formalPath):null;
   Check(G.SavePath.Contains("2.1")&&!G.SavePath.EndsWith("stash-v40.json"),"210 Verification uses an isolated version save");
   G.HardMode=false;G.HarborWalkthrough=true;G.BeginTraining();G.Paused=true;yield return null;
   var outside=G.Map;var outsideVision=G.Vision;var outdoorActors=G.Enemies.ToArray();var player=G.Player;var bag=G.Bag;
   float zoom=G.View.orthographicSize;
   Check(outside.HarborWhitebox&&outside.Bounds.width==208&&outside.Bounds.height==180,"210 Harbor scale and boundaries retained");
   Check(outside.Loot.Count==30&&outdoorActors.Length==12,"210 Thirty exterior containers and twelve exterior enemies retained");
   var destinations=new[]{"portbooth","portservice","yardhome","yardshed","ferry1","repairannex","repairmain1"};
   Check(outside.Doors.Count==7&&outside.Doors.TrueForAll(d=>d.Portal&&Array.IndexOf(destinations,d.Destination)>=0),"210 Seven intended exterior entrances retained");
   foreach(var key in destinations)Check(outside.Doors.FindAll(d=>d.Destination==key).Count==1,"210 Exterior portal is unique "+key);
   Check(Vector3.Distance(outside.Doors.Find(d=>d.Destination=="repairmain1").Position,RaidMap.PortPoint(35,87.48f))<.001f,"210 Main entrance matches the exterior shutter");
   var building=outside.MainBuildings.Find(b=>b.Name=="维修主楼");
   Check(building!=null&&building.Floors==4&&Mathf.Approximately(building.Height,14.4f),"210 Exterior main building still reserves four floors");
   Check(!outside.Loot.Exists(l=>RaidMap.RepairBounds149.Contains(new Vector2(l.Position.x,l.Position.z)))&&!Array.Exists(outdoorActors,e=>RaidMap.RepairBounds149.Contains(new Vector2(e.Home.x,e.Home.z))),"210 Repair district has no added exterior loot or enemies");
   G.Player.transform.position=RaidMap.YardGate+Vector3.back*2;Physics.SyncTransforms();
   Check(Walk140(RaidMap.YardGate+Vector3.forward*3)&&Clear140(RaidMap.RepairMainApproach210)&&Walk140(RaidMap.RepairMainApproach210),"210 Actual walking connects courtyard gate, repair road and main entrance");
   yield return RepairWorldPicture149("210-main-entrance-exterior.png",RaidMap.PortPoint(35,87),45,14,false,1600,1200);
   G.Player.Health=73;G.Magazine=17;G.Rig.ResetView(135);G.Rig.Tick(0);Physics.SyncTransforms();F111(false,0);F111(true,1);
   G.Controls=new TouchInput{Searching=true,Shooting=true};G.TickInteraction(.2f);
   Check(G.Map==outside&&G.DoorHold==0,"210 Shooting cancels main-entry progress");
   F111(false,0);F111(true,1.99f);Check(G.Map==outside&&G.DoorHold>1.9f,"210 Physical main entry cannot finish before two seconds");
   F111(false,0);Check(G.DoorHold==0,"210 Releasing F cancels main-entry progress");
   F111(true,1);G.Player.transform.position+=Vector3.back*3;Physics.SyncTransforms();F111(true,.1f);
   Check(G.Map==outside&&G.DoorHold==0,"210 Leaving the doorway cancels main-entry progress");
   Check(Walk140(RaidMap.RepairMainApproach210),"210 Player walks back into main entrance interaction range");
   var returnPoint=G.Player.transform.position;F111(false,0);F111(true,2.01f);G.Paused=true;yield return null;
   var inside=G.Map;var indoorVision=G.Vision;
   Check(inside.IsInterior&&inside.InteriorId=="repairmain1"&&inside!=outside&&indoorVision!=outsideVision,"210 Main first floor owns independent map and fog instances");
   Check(!outside.Root.gameObject.activeInHierarchy&&inside.Root.gameObject.activeInHierarchy&&Array.TrueForAll(outdoorActors,e=>!e.gameObject.activeInHierarchy),"210 Main entry deactivates exterior geometry and actors");
   Check(G.Player==player&&G.Bag==bag&&G.Player.Health==73&&G.Magazine==17,"210 Main entry preserves player, inventory, health and ammunition");
   Check(Mathf.Abs(G.View.orthographicSize-zoom)<.001f&&Mathf.Abs(Mathf.DeltaAngle(G.Rig.Yaw,135))<.1f,"210 Main entry preserves exterior zoom and view");
   Check(Vector3.Distance(inside.Spawn,new Vector3(8.5f,0,-14.5f))<.001f&&Clear140(inside.Spawn),"210 Main arrival stand is clear beside the exterior shutter");
   Check(RectClose200(inside.Bounds,new Rect(-13.5f,-16,27,32)),"210 First floor retains the main building footprint");
   Check(inside.Loot.Count==0&&G.Enemies.Count==0&&inside.Root.GetComponentsInChildren<ContainerProp>(true).Length==0&&!Array.Exists(inside.Root.GetComponentsInChildren<RaidActor>(true),a=>a!=G.Player),"210 First floor has no added containers or enemy actors");
   Check(inside.Doors.FindAll(d=>d.Portal).Count==1&&inside.RepairMainRooms210.FindAll(r=>r.ReservedStair).Count==2&&inside.RepairMainRooms210.Exists(r=>!r.ReservedStair),"210 Main floor has a return portal, side rooms and two reserved stairs");
   Check(G.Vision.RoomGate&&G.Vision.Explored(inside.Spawn)&&Shader.GetGlobalTexture("_HarborVisibility")==indoorVision.Texture,"210 Interior fog and room exploration bind to main floor");
   F111(true,3);Check(G.Map==inside&&G.DoorHold==0,"210 Held entry key cannot immediately return outside");F111(false,0);
   VerifyRepairMainMiniMap210();G.Rig.ResetView(45);G.Rig.Tick(0);
   foreach(var room in inside.RepairMainRooms210){
    var center=RoomCenter200(room.Area);
    Check(!room.Door.Open&&room.Door.Locked==room.ReservedStair&&!G.Vision.Explored(center),"210 Side region starts closed and unexplored "+room.Name);
    Check(!G.Vision.RoomAllowed(center)&&!G.Vision.CanSee(center),"210 Closed side region remains black under flashlight "+room.Name);
    Check(Clear140(room.ApproachPoint)&&!G.ClearSight(room.ApproachPoint,room.WalkPoint)&&BlockedShot210(room.ApproachPoint,room.WalkPoint),"210 Closed door blocks sight and projectiles "+room.Name);
    Check(Unreachable210(room.ApproachPoint,room.WalkPoint),"210 Closed region has no wall bypass "+room.Name);
    var side=(room.WalkPoint-room.ApproachPoint).normalized;
    var wall=room.Door.Position+Vector3.forward*2;
    Check(Physics.Linecast(wall-side*.8f+Vector3.up*.8f,wall+side*.8f+Vector3.up*.8f,SceneObstacle.SightMask,QueryTriggerInteraction.Ignore)&&BlockedShot210(wall-side*.8f,wall+side*.8f)&&!Clear140(wall),"210 Physical partition blocks motion, sight and shots beside the door "+room.Name);
   }
   G.Player.Aim=Vector3.forward;G.Vision.Refresh(G.Player.transform.position,G.Player.Aim);G.Player.Draw(G.View,0);yield return Capture("210-main-entry-closed.png");
   Check(inside.RepairMainWalkPoints210.Count>=4,"210 Main hall declares usable circulation destinations");
   foreach(var point in inside.RepairMainWalkPoints210)Check(Clear140(point)&&Walk140(point),"210 Actual player traverses furnished main hall "+point);
   foreach(var room in inside.RepairMainRooms210){
    Check(Walk140(room.ApproachPoint),"210 Actual player reaches side door through main hall "+room.Name);
    F111(false,0);F111(true,.01f);
    if(room.ReservedStair){
     Check(room.Door.Locked&&!room.Door.Open&&G.Map==inside&&!G.Vision.RoomAllowed(RoomCenter200(room.Area)),"210 F cannot open or enter reserved upper stairs "+room.Name);
     var direction=(room.WalkPoint-room.ApproachPoint).normalized;
     for(int i=0;i<45;i++){G.Player.Move(direction*.08f);Physics.SyncTransforms();}
     Check(Vector3.Dot(G.Player.transform.position-room.Door.Position,direction)<0&&!room.Area.Contains(new Vector2(G.Player.transform.position.x,G.Player.transform.position.z)),"210 Physical walking stops outside locked stairs "+room.Name);
     // Actor movement uses .30m clearance while navigation uses .34m; test from a clear hall stand.
     Check(Walk140(room.ApproachPoint)&&Clear140(G.Player.transform.position),"210 Player can leave locked stair door "+room.Name);
     Check(Unreachable210(room.ApproachPoint,room.WalkPoint),"210 Locked stairs remain inaccessible after interaction "+room.Name);continue;
    }
    Check(room.Door.Open&&G.Vision.Explored(RoomCenter200(room.Area)),"210 F opens side door and reveals exploration memory "+room.Name);
    Check(G.ClearSight(room.ApproachPoint,room.WalkPoint)&&!BlockedShot210(room.ApproachPoint,room.WalkPoint)&&Clear140(room.WalkPoint)&&Walk140(room.WalkPoint),"210 Open doorway passes sight, shots and actual player movement "+room.Name);
    foreach(var other in inside.RepairMainRooms210)if(other!=room&&!other.Door.Open)Check(!G.Vision.Explored(RoomCenter200(other.Area)),"210 Opening one room preserves other closed regions "+other.Name);
    G.Player.Aim=Vector3.forward;G.Rig.Tick(0);G.Vision.Refresh(G.Player.transform.position,G.Player.Aim);G.Player.Draw(G.View,0);yield return Capture("210-main-room-"+inside.RepairMainRooms210.IndexOf(room)+".png");
    Check(Walk140(room.ApproachPoint)&&G.ToggleRoomDoor(room.Door)&&!room.Door.Open,"210 Actual walking returns to hall and allows closing side door "+room.Name);
    Check(G.Vision.Explored(RoomCenter200(room.Area))&&BlockedShot210(room.ApproachPoint,room.WalkPoint),"210 Closing side door restores collision and retains explored memory "+room.Name);
    Check(G.ToggleRoomDoor(room.Door)&&room.Door.Open,"210 Visited side room reopens "+room.Name);
   }
   Check(Walk140(inside.Spawn),"210 Main hall and opened rooms return to exit by actual walking");
   G.TickExtraction(6);Check(G.Map==inside&&G.Phase==GamePhase.Raid,"210 Main shutter is a return doorway rather than an extraction zone");
   G.Rig.ResetView(225);G.Rig.Tick(0);F111(false,0);F111(true,1);G.Controls=new TouchInput{Searching=true,Shooting=true};G.TickInteraction(.2f);
   Check(G.Map==inside&&G.DoorHold==0,"210 Shooting cancels main-return progress");
   F111(false,0);F111(true,1.99f);Check(G.Map==inside&&G.DoorHold>1.9f,"210 Main return requires two seconds");F111(true,.02f);G.Paused=true;yield return null;
   Check(G.Map==outside&&G.Vision==outsideVision&&G.Enemies.Count==12&&Vector3.Distance(G.Player.transform.position,returnPoint)<.03f,"210 Return restores exact exterior map, fog, enemies and standing point");
   Check(Mathf.Abs(Mathf.DeltaAngle(G.Rig.Yaw,135))<.1f&&Mathf.Abs(G.View.orthographicSize-zoom)<.001f&&!G.Vision.RoomGate&&Shader.GetGlobalTexture("_HarborVisibility")==outsideVision.Texture,"210 Return restores exterior camera, scale and fog binding");
   F111(false,0);F111(true,2.01f);G.Paused=true;yield return null;
   Check(G.Map==inside&&G.Vision==indoorVision&&Mathf.Abs(Mathf.DeltaAngle(G.Rig.Yaw,225))<.1f&&Mathf.Abs(G.View.orthographicSize-zoom)<.001f,"210 Reentry restores main floor instance, local fog and saved view");
   Check(inside.RepairMainRooms210.TrueForAll(r=>r.ReservedStair?r.Door.Locked&&!r.Door.Open&&!G.Vision.RoomAllowed(RoomCenter200(r.Area)):r.Door.Open&&G.Vision.Explored(RoomCenter200(r.Area))),"210 Reentry retains explored rooms and locked stair reservations");
   G.Player.transform.position=inside.Spawn;Physics.SyncTransforms();F111(false,0);F111(true,2.01f);G.Paused=true;yield return null;
   G.Player.transform.position=RaidMap.RepairAnnexApproach200;Physics.SyncTransforms();G.SwitchInterior("repairannex");G.Paused=true;yield return null;
   var annex=G.Map;var annexVision=G.Vision;
   Check(annex!=inside&&annex.InteriorId=="repairannex"&&annexVision!=indoorVision&&annex.RepairRooms200.TrueForAll(r=>!r.Door.Open&&!G.Vision.Explored(RoomCenter200(r.Area))),"210 Old annex starts with its independent map, doors and fog");
   Check(!inside.Root.gameObject.activeInHierarchy&&annex.Root.gameObject.activeInHierarchy&&annex.Loot.Count==0&&G.Enemies.Count==0,"210 Main geometry and population do not leak into annex");
   G.SwitchInterior("repairannex");G.Paused=true;yield return null;
   G.Player.transform.position=RaidMap.RepairMainApproach210;Physics.SyncTransforms();G.SwitchInterior("repairmain1");G.Paused=true;yield return null;
   Check(G.Map==inside&&G.Vision==indoorVision&&inside.RepairMainRooms210.TrueForAll(r=>r.ReservedStair?!r.Door.Open:r.Door.Open&&G.Vision.Explored(RoomCenter200(r.Area))),"210 Annex visit does not rebuild main rooms or exploration");
   Check(!inside.Root.GetComponent<RepairCutaway200>()&&!Array.Exists(inside.Root.GetComponentsInChildren<Renderer>(true),r=>r.sharedMaterial&&r.sharedMaterial.shader.name=="GreyHarbor/RepairCutaway200"),"210 Main floor walls have no player-centred cutaway shader");
   Check(!annex.Root.GetComponent<RepairCutaway200>()&&!Array.Exists(annex.Root.GetComponentsInChildren<Renderer>(true),r=>r.sharedMaterial&&r.sharedMaterial.shader.name=="GreyHarbor/RepairCutaway200"),"210 Annex walls have no player-centred cutaway shader");
   foreach(float yaw in new[]{45f,135f}){
    G.Player.transform.position=inside.RepairMainWalkPoints210[inside.RepairMainWalkPoints210.Count/2];Physics.SyncTransforms();G.Player.Aim=Vector3.forward;G.Rig.ResetView(yaw);G.Rig.Tick(0);G.Vision.Refresh(G.Player.transform.position,G.Player.Aim);G.Player.Draw(G.View,0);
    yield return null;yield return Capture("210-main-play-view-"+yaw+".png");
   }
   G.Player.VisibleToPlayer=false;G.Player.Draw(G.View,0);
   yield return RepairWorldPicture149("210-main-overview-45.png",Vector3.zero,45,26,false,2000,1600);
   yield return RepairWorldPicture149("210-main-overview-135.png",Vector3.zero,135,26,false,2000,1600);
   yield return RepairWorldPicture149("210-main-plan.png",Vector3.zero,0,18,true,2000,2000);
   G.Player.VisibleToPlayer=true;G.Player.Draw(G.View,0);G.Controls=new DesktopInput();G.Finish(false);yield return null;
   Check(G.Phase==GamePhase.Base&&!G.TrainingActive&&G.Enemies.Count==0&&G.ActiveSpits==0&&!outside.Root&&!inside.Root&&!annex.Root,"210 Preview exit returns to camp and disposes visited maps and projectiles");
   Check(JsonUtility.ToJson(G.Save)==saved,"210 First-floor preview preserves exact camp inventory");
   Check(formalBefore==null?!File.Exists(formalPath):File.Exists(formalPath)&&File.ReadAllText(formalPath)==formalBefore,"210 First-floor verification leaves the formal save file unchanged");
  }
  bool BlockedShot210(Vector3 from,Vector3 to)=>Physics.Linecast(from+Vector3.up*.8f,to+Vector3.up*.8f,SceneObstacle.ShotMask,QueryTriggerInteraction.Ignore);
  bool Unreachable210(Vector3 from,Vector3 to){var path=G.Navigation.FindPath(from,to);return path.Count==0||Vector3.Distance(path[path.Count-1],to)>.2f;}
  void VerifyRepairMainMiniMap210(){
   var viewport=new Rect(0,0,200,180);var origin=G.Map.Spawn;Vector2 previous=Vector2.zero;
   foreach(float yaw in new[]{45f,135f}){
    G.Rig.ResetView(yaw);G.Rig.Tick(0);var project=new MiniMapProjection(G.Map.Bounds,viewport,G.View.transform.rotation);
    var direction=project.Point(origin+Vector3.forward)-project.Point(origin);var real=G.View.WorldToScreenPoint(origin+Vector3.forward)-G.View.WorldToScreenPoint(origin);
    Check(Vector2.Dot(direction.normalized,new Vector2(real.x,-real.y).normalized)>.999f,"210 Main minimap north follows live camera "+yaw);
    if(previous!=Vector2.zero)Check(Vector2.Distance(previous.normalized,direction.normalized)>.5f,"210 Main minimap responds to rotated view");previous=direction;
    Check(viewport.Contains(project.Point(origin)),"210 Main spawn stays inside local minimap "+yaw);
    foreach(var room in G.Map.RepairMainRooms210)Check(viewport.Contains(project.Point(room.WalkPoint)),"210 Main side-region marker stays in local minimap "+yaw+" "+room.Name);
   }
  }
 }
}



