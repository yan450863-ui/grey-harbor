using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
namespace GreyHarbor {
 public sealed partial class RaidVerification {
  // Scope is the 2.0 repair-district change and its shared transition contract.
  // Historical economy, combat, container and unrelated district suites are intentionally omitted.
  IEnumerator VerifyRepairVersion200(){
   FreshCamp();string saved=JsonUtility.ToJson(G.Save);
   string formalPath=Path.Combine(Path.GetDirectoryName(G.SavePath),"stash-v40.json");
   string formalBefore=File.Exists(formalPath)?File.ReadAllText(formalPath):null;
   Check(G.SavePath.Contains("2.0")&&!G.SavePath.EndsWith("stash-v40.json"),"200 Verification save is isolated from the formal inventory");
   G.HardMode=false;G.HarborWalkthrough=true;G.BeginTraining();G.Paused=true;yield return null;
   var outside=G.Map;var outsideVision=G.Vision;var outdoorActors=G.Enemies.ToArray();var player=G.Player;var bag=G.Bag;
   float zoom=G.View.orthographicSize;
   Check(outside.HarborWhitebox&&outside.Bounds.width==208&&outside.Bounds.height==180,"200 Existing harbor land boundary and scale retained");
   Check(outside.Loot.Count==30&&outdoorActors.Length==12,"200 Existing thirty outdoor containers and twelve enemies retained");
   var destinations=new[]{"portbooth","portservice","yardhome","yardshed","ferry1","repairannex"};
   Check(outside.Doors.Count==6&&outside.Doors.TrueForAll(d=>d.Portal&&Array.IndexOf(destinations,d.Destination)>=0),"200 Exterior has five original portals and the repair annex portal");
   foreach(var key in destinations)Check(outside.Doors.FindAll(d=>d.Destination==key).Count==1,"200 Unique exterior portal "+key);
   var portal=outside.Doors.Find(d=>d.Destination=="repairannex");
   Check(Vector3.Distance(portal.Position,RaidMap.PortPoint(22,65.48f))<.001f,"200 Annex portal belongs to the exterior shutter");
   Check(Vector3.Distance(RaidMap.RepairAnnexApproach200,RaidMap.PortPoint(22,64.65f))<.001f,"200 Exterior annex approach has an explicit clear standing point");
   Check(!outside.Loot.Exists(l=>RaidMap.RepairBounds149.Contains(new Vector2(l.Position.x,l.Position.z)))&&!Array.Exists(outdoorActors,e=>RaidMap.RepairBounds149.Contains(new Vector2(e.Home.x,e.Home.z))),"200 Repair district has no unrequested container or enemy population");
   Check(outside.RepairDistrictRoot149&&outside.RepairDistrictRoot149.GetComponentsInChildren<ContainerProp>(true).Length==0&&outside.RepairDistrictRoot149.GetComponentsInChildren<RaidActor>(true).Length==0,"200 Repair scenery remains a furnishing-only exterior group");
   var building=outside.MainBuildings.Find(b=>b.Name=="维修主楼");
   Check(building!=null&&building.Floors==4&&Mathf.Approximately(building.Height,14.4f),"200 Repair main building still reserves four floors");
   foreach(var name in new[]{"维修主楼","维修附属棚"}){
    var shell=outside.Root.Find(name);var c=shell?shell.GetComponent<BoxCollider>():null;
    Check(c&&c.enabled&&c.gameObject.layer==SceneObstacle.HighLayer,"200 Real exterior shell retains high movement/sight/shot collision "+name);
    Check(!Clear140(new Vector3(c.bounds.center.x,0,c.bounds.center.z)),"200 Player cannot walk into exterior building volume "+name);
    var from=new Vector3(c.bounds.max.x+1.5f,.8f,c.bounds.center.z);var to=new Vector3(c.bounds.min.x-1.5f,.8f,c.bounds.center.z);var delta=to-from;
    Check(Physics.Raycast(from,delta.normalized,delta.magnitude,SceneObstacle.SightMask,QueryTriggerInteraction.Ignore)&&Physics.Raycast(from,delta.normalized,delta.magnitude,SceneObstacle.ShotMask,QueryTriggerInteraction.Ignore),"200 Real exterior wall blocks vision and projectiles "+name);
   }
   var guards=outside.RepairDistrictRoot149.GetComponentsInChildren<FacadeClearance200>(true);
   Check(guards.Length==2,"200 Both repair building facades have local movement clearance");
   foreach(var key in new[]{"主楼","附属棚"})Check(Array.Exists(guards,g=>g.name.Contains(key)),"200 Local facade clearance present "+key);
   foreach(var guard in guards)VerifyFacadeClearance200(guard);
   var gate=RaidMap.YardGate;G.Player.transform.position=gate+Vector3.back*2;Physics.SyncTransforms();
   Check(Clear140(gate)&&Walk140(gate+Vector3.forward*3),"200 Existing courtyard gate still crosses to the repair road");
   Check(outside.RepairWalkPoints149.Count==16,"200 Existing sixteen repair route destinations retained");
   foreach(var point in outside.RepairWalkPoints149){
    Check(outside.OnHarborSurface(point)&&Clear140(point),"200 Repair route stand clear and on land "+point);
    Check(Walk140(point),"200 Actual player movement reaches repair route stand "+point);
   }
   Check(Clear140(RaidMap.RepairAnnexApproach200)&&Walk140(RaidMap.RepairAnnexApproach200),"200 Existing road connects to the uncluttered annex entry");
   Check(Walk140(building.Approach),"200 Main building closed entrance remains reachable");
   Check(outside.Doors.TrueForAll(d=>d.Destination!="repairmain"),"200 Main building has no premature interior entrance");
   Check(Walk140(gate+Vector3.forward*3)&&Walk140(gate+Vector3.back*2),"200 Road still returns through the courtyard north gate");
   VerifyRepairGround200(outside,false);
   // Eight wall faces are physically pushed against, then inspected from each playable diagonal view.
   foreach(float yaw in new[]{45f,135f,225f,315f}){
    G.Player.transform.position=RaidMap.RepairAnnexApproach200;G.Player.Move(Vector3.forward*1.2f);Physics.SyncTransforms();
    G.Rig.ResetView(yaw);G.Rig.Tick(0);G.Player.Aim=Vector3.forward;G.Vision.Refresh(G.Player.transform.position,G.Player.Aim);G.Player.Draw(G.View,0);
    yield return null;yield return Capture("200-annex-contact-view-"+yaw+".png");
    yield return RepairWorldPicture149("200-main-facade-view-"+yaw+".png",RaidMap.PortPoint(26.5f,88),yaw,14,false,1600,1200);
   }
   // The shared doorway interaction must retain its two-second and shooting-cancellation semantics.
   G.Player.transform.position=RaidMap.RepairAnnexApproach200;G.Player.Health=73;G.Magazine=17;
   G.Rig.ResetView(135);G.Rig.Tick(0);Physics.SyncTransforms();F111(false,0);F111(true,1);
   G.Controls=new TouchInput{Searching=true,Shooting=true};G.TickInteraction(.2f);
   Check(G.Map==outside&&G.DoorHold==0,"200 Shooting cancels annex-entry progress");
   F111(false,0);F111(true,1.99f);
   Check(G.Map==outside&&G.DoorHold>1.9f,"200 Annex entry cannot finish before two seconds");
   F111(false,0);Check(G.DoorHold==0,"200 Releasing F clears annex-entry progress");
   F111(true,1);G.Player.transform.position+=Vector3.back*3;F111(true,.1f);
   Check(G.Map==outside&&G.DoorHold==0,"200 Leaving annex interaction distance cancels entry");
   G.Player.transform.position=RaidMap.RepairAnnexApproach200;Physics.SyncTransforms();F111(false,0);F111(true,2.01f);G.Paused=true;yield return null;
   var inside=G.Map;var indoorVision=G.Vision;
   Check(inside.IsInterior&&inside.InteriorId=="repairannex","200 Annex entry switches to its own interior map");
   Check(!outside.Root.gameObject.activeInHierarchy&&inside.Root.gameObject.activeInHierarchy&&Array.TrueForAll(outdoorActors,e=>!e.gameObject.activeInHierarchy),"200 Exterior map and enemy actors are inactive indoors");
   Check(G.Player==player&&G.Bag==bag&&G.Player.Health==73&&G.Magazine==17,"200 Entry preserves player instance, inventory, health and ammunition");
   Check(Mathf.Abs(G.View.orthographicSize-zoom)<.001f&&Mathf.Abs(Mathf.DeltaAngle(G.Rig.Yaw,135))<.1f,"200 Entry preserves outdoor zoom and view direction");
   Check(RectClose200(inside.Bounds,new Rect(-9.15f,-9.65f,18.3f,19.3f)),"200 Annex floor uses its exterior-scale 18.3 by 19.3 metre footprint");
   Check(RectClose200(inside.InitialRoom,RaidMap.RepairHall200)&&Mathf.Abs(inside.InitialRoom.width*inside.InitialRoom.height/(17.8f*18.8f)-.74944f)<.02f,"200 Connected hall occupies about three quarters of the annex");
   Check(Vector3.Distance(inside.Spawn,new Vector3(-2.5f,0,-8.05f))<.001f&&Clear140(inside.Spawn),"200 Annex arrival stand is clear and beside the shutter");
   Check(inside.Loot.Count==0&&G.Enemies.Count==0&&inside.Root.GetComponentsInChildren<ContainerProp>(true).Length==0,"200 First annex interior has no containers or enemies");
   Check(inside.RepairRooms200.Count==3&&inside.Doors.Count==4,"200 Annex has three side-room doors and one return portal");
   Check(inside.Doors.FindAll(d=>d.Portal).Count==1&&Vector3.Distance(inside.Doors.Find(d=>d.Portal).Position,new Vector3(-2.5f,0,-9.08f))<.001f,"200 Return portal stays at the interior shutter");
   Check(G.Vision.RoomGate&&G.Vision.Explored(inside.Spawn)&&Shader.GetGlobalTexture("_HarborVisibility")==indoorVision.Texture,"200 Annex fog and minimap bind to the interior region");
   F111(true,3);Check(G.Map==inside&&G.DoorHold==0,"200 Held entry key cannot immediately trigger return");F111(false,0);
   VerifyRepairGround200(inside,true);VerifyRepairFurniture200(inside);VerifyRepairMiniMap200();
   G.Player.Aim=Vector3.forward;G.Rig.ResetView(45);G.Rig.Tick(0);G.Vision.Refresh(G.Player.transform.position,G.Player.Aim);yield return Capture("200-annex-entry-closed.png");
   foreach(var room in inside.RepairRooms200){
    var center=RoomCenter200(room.Area);
    Check(!room.Door.Open&&!room.Door.Locked&&!G.Vision.Explored(center),"200 Side room begins closed and fully unexplored "+room.Name);
    Check(!G.Vision.RoomAllowed(center)&&!G.Vision.CanSee(center),"200 Unopened room cannot be seen through flashlight "+room.Name);
    var hallSide=new Vector3(3.05f,0,room.Door.Position.z);
    Check(Clear140(hallSide)&&!G.ClearSight(hallSide,room.WalkPoint),"200 Closed side-room doorway blocks sight "+room.Name);
    Check(G.Navigation.FindPath(hallSide,room.WalkPoint).Count==0,"200 Closed side room has no side-wall bypass "+room.Name);
   }
   // Check real motion over the bay rather than merely its declared bounds or a nav mesh path.
   foreach(float z in new[]{-6f,0f,6f})foreach(float x in new[]{-5f,-1f,2.8f}){
    var point=new Vector3(x,0,z);Check(Clear140(point)&&Walk140(point),"200 Actual player traverses the unobstructed hall bay "+point);
   }
   foreach(var room in inside.RepairRooms200){
    var hallSide=new Vector3(3.05f,0,room.Door.Position.z);
    Check(Walk140(hallSide),"200 Player reaches side-room door through the large hall "+room.Name);
    F111(false,0);F111(true,.01f);
    Check(room.Door.Open&&G.Vision.Explored(RoomCenter200(room.Area)),"200 F opens side-room door and reveals its grey fog "+room.Name);
    Check(G.ClearSight(hallSide,room.WalkPoint)&&Walk140(room.WalkPoint),"200 Player actually passes through the opened side-room doorway "+room.Name);
    foreach(var other in inside.RepairRooms200)if(other!=room&&!other.Door.Open)Check(!G.Vision.Explored(RoomCenter200(other.Area)),"200 Opening one room leaves another unopened room black "+other.Name);
    var near=room.WalkPoint+Vector3.back*.6f;
    Check(Clear140(near)&&Walk140(near),"200 Side-room standing space accommodates movement "+room.Name);
    G.Player.Aim=Vector3.forward;G.Rig.Tick(0);G.Vision.Refresh(G.Player.transform.position,G.Player.Aim);yield return Capture("200-annex-room-"+inside.RepairRooms200.IndexOf(room)+".png");
    Check(Walk140(hallSide)&&G.ToggleRoomDoor(room.Door)&&!room.Door.Open,"200 Player can return to hall and close side-room door "+room.Name);
    Check(G.Vision.Explored(RoomCenter200(room.Area)),"200 Closing side-room door retains exploration memory "+room.Name);
    Check(G.ToggleRoomDoor(room.Door)&&room.Door.Open,"200 Reopening a searched room remains possible "+room.Name);
   }
   // Real walls, not black paint, must separate adjacent furnished side rooms.
   foreach(float z in new[]{2.8f,-3.2f}){
    var a=new Vector3(6.25f,.8f,z-.45f);var b=new Vector3(6.25f,.8f,z+.45f);
    Check(Physics.Linecast(a,b,SceneObstacle.SightMask,QueryTriggerInteraction.Ignore)&&Physics.Linecast(a,b,SceneObstacle.ShotMask,QueryTriggerInteraction.Ignore),"200 Physical partition blocks sight and bullets between side rooms at "+z);
    G.Player.transform.position=new Vector3(6.25f,0,z-.55f);Physics.SyncTransforms();G.Player.Move(Vector3.forward*1.1f);Physics.SyncTransforms();
    Check(G.Player.transform.position.z<z-.38f,"200 Physical partition stops direct passage between side rooms at "+z);
   }
   G.Player.transform.position=inside.Spawn;Physics.SyncTransforms();G.TickExtraction(6);
   Check(G.Map==inside&&G.Phase==GamePhase.Raid,"200 Indoor shutter never becomes an extraction point");
   G.Rig.ResetView(225);G.Rig.Tick(0);F111(false,0);F111(true,1);
   G.Controls=new TouchInput{Searching=true,Shooting=true};G.TickInteraction(.2f);
   Check(G.Map==inside&&G.DoorHold==0,"200 Shooting cancels annex-return progress");
   F111(false,0);F111(true,1.99f);Check(G.Map==inside&&G.DoorHold>1.9f,"200 Indoor return also requires two seconds");F111(true,.02f);G.Paused=true;yield return null;
   Check(G.Map==outside&&G.Vision==outsideVision&&G.Enemies.Count==12&&Vector3.Distance(G.Player.transform.position,RaidMap.RepairAnnexApproach200)<.03f,"200 Return restores outdoor instance, enemies and exact approach point");
   Check(Mathf.Abs(Mathf.DeltaAngle(G.Rig.Yaw,135))<.1f&&Mathf.Abs(G.View.orthographicSize-zoom)<.001f,"200 Return restores pre-entry camera direction and size");
   Check(!G.Vision.RoomGate&&Shader.GetGlobalTexture("_HarborVisibility")==outsideVision.Texture&&outside.Bounds.width==208,"200 Outdoor fog and minimap resume their larger bounds");
   F111(false,0);F111(true,2.01f);G.Paused=true;yield return null;
   Check(G.Map==inside&&G.Vision==indoorVision&&Mathf.Abs(Mathf.DeltaAngle(G.Rig.Yaw,225))<.1f,"200 Re-entry restores the same annex and saved indoor view");
   Check(inside.RepairRooms200.TrueForAll(r=>r.Door.Open&&G.Vision.Explored(RoomCenter200(r.Area))),"200 Annex room-door and exploration state survive re-entry");
   // Opening an unrelated existing room must not reuse or reset the annex state.
   G.Player.transform.position=inside.Spawn;Physics.SyncTransforms();F111(false,0);F111(true,2.01f);G.Paused=true;yield return null;
   G.Player.transform.position=outside.Doors.Find(d=>d.Destination=="portbooth").Position+Vector3.back*1.1f;Physics.SyncTransforms();G.SwitchInterior("portbooth");G.Paused=true;yield return null;
   var booth=G.Map;var boothVision=G.Vision;
   Check(booth!=inside&&booth.InteriorId=="portbooth"&&boothVision!=indoorVision,"200 Existing gatehouse and new annex have independent map/fog instances");
   Check(!inside.Root.gameObject.activeInHierarchy&&booth.Root.gameObject.activeInHierarchy,"200 Inactive annex does not leak geometry into another interior");
   G.SwitchInterior("portbooth");G.Paused=true;yield return null;
   G.Player.transform.position=RaidMap.RepairAnnexApproach200;Physics.SyncTransforms();G.SwitchInterior("repairannex");G.Paused=true;yield return null;
   Check(G.Map==inside&&G.Enemies.Count==0&&inside.RepairRooms200.TrueForAll(r=>r.Door.Open&&G.Vision.Explored(RoomCenter200(r.Area))),"200 Visiting another interior does not rebuild annex doors or fog");
   Check(inside.Root.GetComponent<RepairCutaway200>()&&inside.Root.Find("InteriorWall").GetComponent<Renderer>().sharedMaterial.shader.name=="GreyHarbor/RepairCutaway200","200 Annex uses a local wall cutaway shader while physical walls stay intact");
   foreach(float yaw in new[]{45f,135f,225f,315f}){
    G.Player.transform.position=new Vector3(-1.5f,0,-1.0f);G.Player.Aim=Vector3.forward;G.Rig.ResetView(yaw);G.Rig.Tick(0);G.Vision.Refresh(G.Player.transform.position,G.Player.Aim);G.Player.Draw(G.View,0);
    yield return null;yield return Capture("200-annex-play-view-"+yaw+".png");
   }
   G.Player.VisibleToPlayer=false;G.Player.Draw(G.View,0);
   yield return RepairWorldPicture149("200-annex-overview.png",Vector3.zero,45,13,false,1800,1400);
   yield return RepairWorldPicture149("200-annex-plan.png",Vector3.zero,0,11,true,1800,1900);
   G.Player.VisibleToPlayer=true;G.Player.Draw(G.View,0);
   G.Controls=new DesktopInput();G.Finish(false);yield return null;
   Check(G.Phase==GamePhase.Base&&!G.TrainingActive&&G.Enemies.Count==0&&G.ActiveSpits==0,"200 Ending the preview returns to a safe camp and clears enemies/projectiles");
   Check(!outside.Root&&!inside.Root&&!booth.Root,"200 Ending preview disposes outdoor and both visited interior roots");
   Check(JsonUtility.ToJson(G.Save)==saved,"200 Exiting the regional preview preserves exact camp inventory");
   Check(formalBefore==null?!File.Exists(formalPath):File.Exists(formalPath)&&File.ReadAllText(formalPath)==formalBefore,"200 Targeted verification leaves the formal save file unchanged");
  }
  static bool RectClose200(Rect a,Rect b)=>Mathf.Abs(a.x-b.x)<.001f&&Mathf.Abs(a.y-b.y)<.001f&&Mathf.Abs(a.width-b.width)<.001f&&Mathf.Abs(a.height-b.height)<.001f;
  static Vector3 RoomCenter200(Rect area)=>new Vector3(area.center.x,0,area.center.y);
  void VerifyFacadeClearance200(FacadeClearance200 guard){
   Check(guard.Padding>=.65f&&guard.Padding<1,"200 Facade movement margin covers shoulders at all intermediate view angles "+guard.name);
   Check(guard.Padding>.4475f,"200 Facade movement padding exceeds protruding shutter fittings "+guard.name);
   Check(guard.GetComponentsInChildren<Renderer>(true).Length==0,"200 Movement clearance adds no visible facade mesh "+guard.name);
   var colliders=guard.GetComponentsInChildren<BoxCollider>(true);
   Check(colliders.Length==4,"200 Building has four local facade movement strips "+guard.name);
   foreach(var c in colliders){
    Check(c.enabled&&!c.isTrigger&&c.gameObject.layer==SceneObstacle.LowLayer&&c.GetComponent<SceneObstacle>().Kind==ObstacleKind.Low,"200 Facade guard uses only the low movement class "+c.name);
    var b=c.bounds;bool x=b.size.x<b.size.z;var dir=x?Vector3.right:Vector3.forward;float len=x?b.size.x:b.size.z;
    var from=b.center-dir*(len*.5f+.05f);
    Check(!Array.Exists(Physics.RaycastAll(from,dir,len+.1f,SceneObstacle.SightMask,QueryTriggerInteraction.Ignore),h=>h.collider==c)&&!Array.Exists(Physics.RaycastAll(from,dir,len+.1f,SceneObstacle.ShotMask,QueryTriggerInteraction.Ignore),h=>h.collider==c),"200 Clearance strip cannot intercept vision or gunfire "+c.name);
   }
   var r=guard.Footprint;var center=new Vector3(r.center.x,0,r.center.y);
   var normals=new[]{Vector3.left,Vector3.right,Vector3.back,Vector3.forward};
   foreach(var normal in normals){
    Vector3 tangent=Mathf.Abs(normal.x)>.5f?Vector3.forward:Vector3.right;
    float extent=Mathf.Abs(normal.x)>.5f?r.width*.5f:r.height*.5f;
    float sideExtent=Mathf.Abs(normal.x)>.5f?r.height*.5f:r.width*.5f;
    bool found=false;Vector3 start=Vector3.zero;
    foreach(float fraction in new[]{0f,-.45f,.45f,-.7f,.7f}){
     var candidate=center+normal*(extent+guard.Padding+.9f)+tangent*sideExtent*fraction;
     if(G.Map.OnHarborSurface(candidate)&&Clear140(candidate)){start=candidate;found=true;break;}
    }
    Check(found,"200 Clear contact-test approach outside facade "+guard.name+" "+normal);
    G.Player.transform.position=start;Physics.SyncTransforms();
    for(int i=0;i<80;i++){G.Player.Move(-normal*.08f);Physics.SyncTransforms();}
    float distance=Vector3.Dot(G.Player.transform.position-center,normal)-extent;
    Check(distance>=guard.Padding+.285f&&distance<guard.Padding+.91f,"200 Actual capsule push stops beyond facade fittings "+guard.name+" "+normal+" distance "+distance.ToString("F3"));
    Check(!Physics.CheckCapsule(G.Player.transform.position+Vector3.up*.35f,G.Player.transform.position+Vector3.up,.299f,SceneObstacle.MovementMask),"200 Wall contact does not embed player capsule "+guard.name+" "+normal);
   }
  }
  void VerifyRepairGround200(RaidMap map,bool indoors){
   Check(map.StableGround137&&map.StableGround137.mipmapCount>1&&map.StableGround137.filterMode==FilterMode.Trilinear,"200 Filtered stable ground with mipmaps "+(indoors?"annex":"exterior"));
   var plane=map.Root.Find("Harbor_StableGround137");
   Check(plane&&plane.GetComponent<MeshFilter>().sharedMesh.vertexCount==4&&plane.GetComponentsInChildren<Collider>().Length==0,"200 One stable ground plane has no detail colliders "+(indoors?"annex":"exterior"));
   int activeGround=0;
   foreach(var r in map.Root.GetComponentsInChildren<Renderer>())if(r.enabled&&r.name=="Harbor_StableGround137")activeGround++;
   Check(activeGround==1,"200 Exactly one active stable ground presentation "+(indoors?"annex":"exterior"));
   if(indoors){
    foreach(var p in new[]{new Vector3(-5,0,-6),new Vector3(2.8f,0,6),new Vector3(6.25f,0,-6.1f),new Vector3(6.25f,0,0),new Vector3(6.25f,0,6)})
     Check(RepairGroundPixel149(map,p).a>.98f,"200 Annex floor has opaque paint at each hall/room sample "+p);
   }else{
    foreach(var p in new[]{RaidMap.YardGate,RaidMap.PortPoint(28,54),RaidMap.PortPoint(46,95),RaidMap.RepairAnnexApproach200})
     Check(RepairGroundPixel149(map,p).a>.98f,"200 Nearby road and annex boundary remain fully painted "+p);
   }
  }
  void VerifyRepairFurniture200(RaidMap map){
   foreach(string id in new[]{"PartsRack","ToolWallBench","EngineBench","WashStation","WorkLockers"}){
    var root=map.Root.Find("RepairInteriorKit200_"+id);
    Check(root&&root.GetComponentsInChildren<Renderer>().Length>0,"200 New reusable workshop model present "+id);
    var info=root.GetComponent<RepairAssetInfo149>();
    Check(info&&info.Asset==id&&!string.IsNullOrEmpty(info.CollisionProfile),"200 New workshop asset has envelope/classification metadata "+id);
    var colliders=root.GetComponentsInChildren<Collider>();
    Check(colliders.Length>0,"200 Workshop model has local collision shapes "+id);
    bool low=false,high=false,classes=true,rays=true;
    foreach(var c in colliders){
     var rule=c.GetComponent<SceneObstacle>();
     classes&=rule&&c.enabled&&!c.isTrigger&&c.gameObject.layer==(rule.Kind==ObstacleKind.Low?SceneObstacle.LowLayer:SceneObstacle.HighLayer);
     low|=c.gameObject.layer==SceneObstacle.LowLayer;high|=c.gameObject.layer==SceneObstacle.HighLayer;
     var b=c.bounds;var from=b.center-Vector3.right*(b.extents.x+.05f);
     bool blocksSight=Array.Exists(Physics.RaycastAll(from,Vector3.right,b.size.x+.1f,SceneObstacle.SightMask,QueryTriggerInteraction.Ignore),hit=>hit.collider==c);
     bool blocksShot=Array.Exists(Physics.RaycastAll(from,Vector3.right,b.size.x+.1f,SceneObstacle.ShotMask,QueryTriggerInteraction.Ignore),hit=>hit.collider==c);
     rays&=blocksSight==(c.gameObject.layer==SceneObstacle.HighLayer)&&blocksShot==(c.gameObject.layer==SceneObstacle.HighLayer);
    }
    Check(classes,"200 All local workshop colliders use their explicit low/high movement class "+id);
    Check(rays,"200 Actual workshop rays follow every local low/high collision shape "+id);
    if(id=="EngineBench"||id=="WashStation")Check(low&&!high,"200 Low working surface leaves sight and bullets open "+id);
    if(id=="PartsRack"||id=="ToolWallBench")Check(low&&high,"200 Mixed workshop model uses local high details and low stock "+id);
    if(id=="WorkLockers")Check(high&&!low,"200 Tall worker lockers block sight and bullets "+id);
   }
  }
  void VerifyRepairMiniMap200(){
   var viewport=new Rect(0,0,200,180);var origin=G.Map.Spawn;Vector2 previous=Vector2.zero;
   foreach(float yaw in new[]{45f,135f,225f,315f}){
    G.Rig.ResetView(yaw);G.Rig.Tick(0);
    var project=new MiniMapProjection(G.Map.Bounds,viewport,G.View.transform.rotation);
    var direction=project.Point(origin+Vector3.forward)-project.Point(origin);
    var real=G.View.WorldToScreenPoint(origin+Vector3.forward)-G.View.WorldToScreenPoint(origin);
    var screen=new Vector2(real.x,-real.y);
    Check(Vector2.Dot(direction.normalized,screen.normalized)>.999f,"200 Indoor minimap north direction follows live world camera "+yaw);
    if(previous!=Vector2.zero)Check(Vector2.Distance(previous.normalized,direction.normalized)>.5f,"200 Minimap changes its orientation after Q/E rotation "+yaw);
    previous=direction;
    foreach(var room in G.Map.RepairRooms200)Check(viewport.Contains(project.Point(room.WalkPoint)),"200 Each room marker remains inside its rotated minimap "+yaw+" "+room.Name);
   }
  }
 }
}
