using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
namespace GreyHarbor {
 public sealed partial class RaidVerification {
  // Focused checks for the expanded main building. No legacy map or economy suites.
  sealed class MainState273 {
   public RaidMap Map;public RaidVision Vision;public LootContainer[] Boxes;public RaidActor[] Actors;
   public string[] Contents;public bool[] Doors,Dead;public float[] Health;
  }
  string mainWalkFailure273;
  bool MainWalkFail273(string reason,Vector3 goal,List<Vector3> route=null){
   mainWalkFailure273=reason;var here=G.Player.transform.position;
   Debug.Log("273 WALK FAILURE reason="+reason+" floor="+G.Map.RepairFloor220+" player="+here+" goal="+goal+" routeCount="+(route==null?0:route.Count));
   if(route!=null)for(int i=0;i<route.Count;i++)Debug.Log("273 WALK ROUTE "+i+" "+route[i]);
   foreach(var p in new[]{here,goal})foreach(var c in Physics.OverlapCapsule(p+Vector3.up*.35f,p+Vector3.up,.34f,SceneObstacle.MovementMask,QueryTriggerInteraction.Ignore))
    Debug.Log("273 WALK OVERLAP point="+p+" collider="+c.name+" bounds="+c.bounds);
   foreach(var room in G.Map.RepairMainRooms210)Debug.Log("273 WALK DOOR "+room.Name+" open="+room.Door.Open+" position="+room.Door.Position+" collider="+room.Door.Collider.bounds);
   return false;
  }
  bool WalkMain273(Vector3 goal){
   const float step=.01f,waypointTolerance=.0001f;
   mainWalkFailure273=null;Physics.SyncTransforms();
   if(!Clear140(goal))return MainWalkFail273("goal capsule blocked",goal);
   var route=G.Navigation.FindPath(G.Player.transform.position,goal);
   if(route.Count==0)return MainWalkFail273("navigation returned empty route",goal,route);
   if(Vector3.Distance(route[route.Count-1],goal)>waypointTolerance)return MainWalkFail273("route endpoint falls short: "+route[route.Count-1],goal,route);
   var anchor=G.Player.transform.position;foreach(var p in route){if(!G.Navigation.ClearSegment(anchor,p)){
     var delta=p-anchor;if(delta.magnitude>.001f&&Physics.CapsuleCast(anchor+Vector3.up*.35f,anchor+Vector3.up,.34f,delta.normalized,out var hit,delta.magnitude,SceneObstacle.MovementMask,QueryTriggerInteraction.Ignore))
      Debug.Log("273 WALK SEGMENT HIT from="+anchor+" to="+p+" collider="+hit.collider.name+" distance="+hit.distance+" bounds="+hit.collider.bounds);
     return MainWalkFail273("planned .34m segment blocked: "+anchor+" to "+p,goal,route);
    }anchor=p;}
   // Navigation validates exact .34m swept segments, whereas Actor.Move sweeps
   // x and z separately with its .30m capsule. Stopping .08m before a corner
   // and taking a .09m diagonal step cut inside the validated navigation route.
   // Follow every waypoint to float precision using real Move calls, with 1cm
   // steps. Keep the stricter .34m check; never teleport or change actor physics.
   float routeLength=0;anchor=G.Player.transform.position;foreach(var p in route){routeLength+=Vector3.Distance(anchor,p);anchor=p;}
   int budget=Mathf.CeilToInt(routeLength/step)+route.Count*16+256;
   int moves=0;foreach(var p in route){int stalled=0;
    while(Vector3.Distance(G.Player.transform.position,p)>waypointTolerance){
     if(++moves>budget)return MainWalkFail273("movement budget "+budget+" for route length "+routeLength+" exceeded at waypoint "+p,goal,route);var before=G.Player.transform.position;
     G.Player.Move(Vector3.ClampMagnitude(p-before,step));Physics.SyncTransforms();
     if(!Clear140(G.Player.transform.position))return MainWalkFail273("actual movement .34m capsule blocked: previous="+before+" waypoint="+p,goal,route);
     if(Vector3.Distance(before,G.Player.transform.position)<.00001f){if(++stalled>12)return MainWalkFail273("actual movement stalled at waypoint "+p,goal,route);}else stalled=0;
    }
   }
   if(Vector3.Distance(G.Player.transform.position,goal)>waypointTolerance)return MainWalkFail273("actual final position falls short",goal,route);
   if(!Clear140(G.Player.transform.position))return MainWalkFail273("final player capsule blocked",goal,route);return true;
  }
  void MainWalk273(Vector3 goal,string label){bool walked=WalkMain273(goal);Check(walked,"273 Actual player walk and .34m swept clearance "+label+" "+goal+(walked?"":" reason="+mainWalkFailure273));}
  IEnumerator MainPortal273(WorldDoor door,string label){
   Check(door!=null&&door.Portal,"273 Portal exists "+label);var before=G.Map;
   G.CloseInventory();F111(false,0);Check(G.NearbyDoor==door,"273 Real F selects intended portal "+label);
   F111(true,1.99f);Check(G.Map==before&&G.DoorHold>1.9f,"273 Portal holds for full two seconds "+label);
   F111(true,.02f);G.Paused=true;yield return null;
   Check(G.Map!=before&&Clear140(G.Player.transform.position),"273 Portal changes map with clear arrival "+label);
   F111(true,3);Check(G.DoorHold==0,"273 Held F cannot bounce across portal "+label);F111(false,0);
  }
  IEnumerator MainStair273(int target,bool north){
   string key="repair-floor:"+target+":"+(north?"north":"south");
   var d=G.Map.Doors.Find(p=>p.Portal&&p.Destination==key);Check(d!=null,"273 Stair route exists "+key);
   MainWalk273(d.Position,key);yield return MainPortal273(d,key);
   Check(G.Map.RepairFloor220==target&&Vector3.Distance(G.Player.transform.position,RaidMap.RepairStairArrival220(north))<.03f,"273 Correct staircase landing "+key);
  }
  void MainArrivalDoor273(){
   var room=G.Map.RepairMainRooms210.Find(r=>r.ReservedStair&&r.Area.Contains(new Vector2(G.Player.transform.position.x,G.Player.transform.position.z)));
   if(room==null||room.Door.Open)return;
   Check(G.Vision.RoomAllowed(G.Player.transform.position)&&G.Vision.Explored(new Vector3(room.Area.center.x,0,room.Area.center.y)),"273 Occupied landing has its own room visibility");
   var inward=(room.WalkPoint-room.ApproachPoint).normalized;
   var stand=room.Door.Position+inward*.60f+Vector3.forward*1.26f;
   MainWalk273(stand,"inside landing door");
   Check(G.ToggleRoomDoor(room.Door)&&room.Door.Open,"273 Player opens stair enclosure from arrival side");
   MainWalk273(room.ApproachPoint,"leave occupied landing");
  }
  IEnumerator MainRooms273(int floor){
   MainArrivalDoor273();var map=G.Map;
   Check(map.RepairMainRooms210.FindAll(r=>r.ReservedStair).Count==2,"273 Two stair enclosures on floor "+floor);
   foreach(var room in map.RepairMainRooms210){
    var center=new Vector3(room.Area.center.x,0,room.Area.center.y);
    if(!room.Door.Open){
     Check(!G.Vision.RoomAllowed(center)&&!G.Vision.Explored(center),"273 Unopened room remains black "+floor+" "+room.Name);
     foreach(var box in map.Loot)if(room.Area.Contains(new Vector2(box.Prop.transform.position.x,box.Prop.transform.position.z)))
      Check(!G.Vision.RoomAllowed(G.LootBounds(box).center),"273 Closed room gates container visibility "+box.Name);
     var path=G.Navigation.FindPath(room.ApproachPoint,room.WalkPoint);
     Check(path.Count==0||Vector3.Distance(path[path.Count-1],room.WalkPoint)>.2f,"273 Closed door blocks navigation "+room.Name);
    }
   }
   foreach(var room in map.RepairMainRooms210){
    var center=new Vector3(room.Area.center.x,0,room.Area.center.y);
    MainWalk273(room.ApproachPoint,"room approach "+room.Name);
    if(room.Door.Locked){
     Check(!G.ToggleRoomDoor(room.Door)&&!room.Door.Open&&!G.Vision.RoomAllowed(center),"273 Locked room remains closed and black "+room.Name);
     Check(!map.Loot.Exists(l=>room.Area.Contains(new Vector2(l.Prop.transform.position.x,l.Prop.transform.position.z))),"273 Locked room remains without searchable stock");continue;
    }
    if(!room.Door.Open){
     var stand=room.Door.Position+(room.ApproachPoint-room.WalkPoint).normalized*(room.ReservedStair?.60f:1.05f);
     MainWalk273(stand,"F door stand "+room.Name);F111(false,0);Check(G.NearbyDoor==room.Door,"273 Real F selects room door "+room.Name);
     F111(true,.01f);F111(false,0);Check(room.Door.Open,"273 Real F opens room door "+room.Name);
    }
    Check(G.Vision.RoomAllowed(center)&&G.Vision.Explored(center),"273 Opening reveals only the accessed room "+room.Name);
    MainWalk273(room.WalkPoint,"through door "+room.Name);MainWalk273(room.ApproachPoint,"return from room "+room.Name);
    Check(G.ToggleRoomDoor(room.Door)&&!room.Door.Open&&G.Vision.Explored(center),"273 Close restores door while retaining exploration "+room.Name);
    var blocked=G.Navigation.FindPath(room.ApproachPoint,room.WalkPoint);
    Check(blocked.Count==0||Vector3.Distance(blocked[blocked.Count-1],room.WalkPoint)>.2f,"273 Closing restores physical barrier "+room.Name);
    Check(G.ToggleRoomDoor(room.Door)&&room.Door.Open,"273 Room door reopens "+room.Name);
    var position=G.Player.transform.position;G.Player.transform.position=room.Door.Position;Physics.SyncTransforms();
    Check(!G.ToggleRoomDoor(room.Door)&&room.Door.Open,"273 Door cannot close on player's capsule "+room.Name);
    G.Player.transform.position=position;Physics.SyncTransforms();yield return null;
   }
   foreach(var point in map.RepairMainWalkPoints210)MainWalk273(point,"floor circulation "+floor);
  }
  void MainMeeting273(){
   var room=G.Map.RepairMainRooms210.Find(r=>r.Name=="会议室");Check(room!=null,"273 Fourth-floor meeting room exists");
   var table=Array.Find(G.Map.Root.GetComponentsInChildren<Collider>(),c=>c.name=="RepairMeetingTable220"&&room.Area.Contains(new Vector2(c.bounds.center.x,c.bounds.center.z)));
   Check(table!=null,"273 Meeting table physically belongs to meeting room");int chairs=0;
   Check(Vector3.Distance(table.bounds.size,new Vector3(4.4f,.84f,1.4f))<.001f,"273 Meeting table retains physical furniture dimensions");
   foreach(var t in G.Map.Root.GetComponentsInChildren<Transform>())if(t.name=="Decor_Chair"&&room.Area.Contains(new Vector2(t.position.x,t.position.z)))chairs++;
   Check(chairs==3,"273 Actual fourth-floor meeting room contains exactly three chairs");
   // Four aisle corners derive from the current room, so the test reaches both ends
   // and both sides of the actual table after expansion, without old coordinates.
   var b=table.bounds;float left=(room.Area.xMin+.45f+b.min.x-.34f)*.5f,right=(b.max.x+.34f+room.Area.xMax-.45f)*.5f;
   float south=(room.Area.yMin+.45f+b.min.z-.34f)*.5f,north=(b.max.z+.34f+room.Area.yMax-.45f)*.5f;
   var corners=new[]{new Vector3(left,0,south),new Vector3(right,0,south),new Vector3(right,0,north),new Vector3(left,0,north)};
   MainWalk273(room.WalkPoint,"enter meeting room");MainWalk273(corners[0],"meeting aisle start");
   for(int i=1;i<=4;i++){
    var goal=corners[i%4];Check(G.Navigation.ClearSegment(G.Player.transform.position,goal),"273 Meeting room aisle directly passes around table side "+i);
    MainWalk273(goal,"meeting table side "+i);Check(room.Area.Contains(new Vector2(G.Player.transform.position.x,G.Player.transform.position.z)),"273 Table circulation remains inside meeting room "+i);
   }
   MainWalk273(room.ApproachPoint,"leave meeting room");
  }
  void MainPatrol273(int floor,List<string> rows){
   var player=G.Player.transform.position;G.Player.transform.position=new Vector3(-1000,0,-1000);
   foreach(var e in G.Enemies){
    Check(Clear140(e.Home)&&e.PatrolTargets!=null&&e.PatrolTargets.Length>1,"273 Enemy home and multiple patrol targets "+floor+" "+e.name);
    foreach(var goal in e.PatrolTargets){
     var path=G.Navigation.FindPath(e.Home,goal);bool clear=Clear140(goal)&&path.Count>0&&Vector3.Distance(path[path.Count-1],goal)<.2f;var last=e.Home;
     foreach(var p in path){clear&=G.Navigation.ClearSegment(last,p);last=p;}Check(clear,"273 Enemy patrol route has .34m swept clearance "+e.name+" "+goal);
    }
    var original=e.transform.position;e.transform.position=e.Home;e.Route.Clear();e.WaitTime=0;e.PatrolIndex=1;float distance=0;bool collision=false;
    for(int tick=0;tick<1200;tick++){
     var prev=e.transform.position;if(e.IsRebel)G.TickRebel145(e,.05f);else G.HarborPatrol(e,.05f);Physics.SyncTransforms();distance+=Vector3.Distance(prev,e.transform.position);
     collision|=Physics.CheckCapsule(e.transform.position+Vector3.up*.35f,e.transform.position+Vector3.up,.29f,SceneObstacle.MovementMask,QueryTriggerInteraction.Ignore);
    }
    Check(distance>1&&!collision,"273 Actual 60-second enemy patrol advances without collision "+floor+" "+e.name);
    rows.Add(floor+","+Csv230(e.name)+","+distance+","+collision);e.transform.position=original;e.Route.Clear();e.WaitTime=2;e.PatrolIndex=0;
   }
   G.Player.transform.position=player;Physics.SyncTransforms();
  }
  void MainShell273(int floor,float zoom){
   var m=G.Map;Check(RectClose200(m.Bounds,new Rect(-27,-32,54,64)),"273 Expanded 54x64m footprint floor "+floor);
   Check(Mathf.Abs(G.View.orthographicSize-zoom)<.001f&&G.Player.transform.lossyScale==Vector3.one,"273 Character scale and gameplay camera magnification retained floor "+floor);
   Check(G.Vision.RoomGate&&Shader.GetGlobalTexture("_HarborVisibility")==G.Vision.Texture,"273 Expanded floor owns bound visibility texture "+floor);
   var shell=m.Root.GetComponent<RepairWallModel220>();Check(shell&&shell.PerimeterWalls.Count==4,"273 Four expanded perimeter walls floor "+floor);
   foreach(var wall in shell.PerimeterWalls)Check(Vector4.Distance(wall.sharedMaterial.GetVector("_RepairWallRect220"),new Vector4(-26.65f,-31.65f,26.65f,31.65f))<.001f,"273 Wall shader clips against expanded perimeter "+wall.name);
   foreach(var d in m.Doors)if(!d.Portal)Check(Vector3.Distance(d.Collider.transform.lossyScale,new Vector3(.12f,2.3f,1.8f))<.001f,"273 Room door retains physical dimensions "+d.Name);
   // Painted regions intentionally stop .12m inside room walls. Their bake bounds
   // need to cover the expanded floor, but need not equal its physical slab edges.
   var paint=m.StableGroundBounds137;var net=RaidMap.RepairMainNet210;const float border=.20f;
   bool ground=m.StableGround137&&Mathf.Abs(paint.xMin-net.xMin)<=border&&Mathf.Abs(paint.xMax-net.xMax)<=border&&Mathf.Abs(paint.yMin-net.yMin)<=border&&Mathf.Abs(paint.yMax-net.yMax)<=border;
   Check(ground,"273 Stable ground covers expanded net within .20m painted border floor "+floor+" actual="+paint+" physical="+net);
   foreach(float x in new[]{-26f,26f})foreach(float z in new[]{-31f,31f})Check(RepairGroundPixel149(m,new Vector3(x,0,z)).a>.5f,"273 Expanded far-corner ground is opaque "+floor+" "+x+"/"+z);
   var viewport=new Rect(0,0,220,220);foreach(float yaw in new[]{0f,45f,90f,135f}){
    G.Rig.ResetView(yaw);G.Rig.Tick(0);var project=new MiniMapProjection(m.Bounds,viewport,G.View.transform.rotation);
    foreach(float x in new[]{m.Bounds.xMin,m.Bounds.xMax})foreach(float z in new[]{m.Bounds.yMin,m.Bounds.yMax})Check(viewport.Contains(project.Point(new Vector3(x,0,z))),"273 Expanded corner remains inside rotating minimap "+floor+" "+yaw);
   }G.Rig.ResetView(45);G.Rig.Tick(0);
  }
  void MainRules273(){
   var q=LootBalance.QualityWeights(ContainerTier.Common);double[] ordinary={60,32,7,1,0};
   for(int i=0;i<5;i++)Check(Math.Abs(q[i]-ordinary[i])<.000001,"273 Unchanged ordinary base rarity coefficient "+i);
   Check(ContainerDropRules230.BonusRedChance==.001,"273 Ordinary separate bonus red chance unchanged");
   foreach(var size in new[]{SafeSize240.Small,SafeSize240.Large}){
    var p=ContainerDropRules230.Profile(ContainerLook.Safe,SupplyType.Safe,ContainerTier.Safe,DropRegion230.Repair,size);
    var normal=ContainerDropRules230.QualityWeights(p,false);var hard=ContainerDropRules230.QualityWeights(p,true);
    double[] expected=size==SafeSize240.Small?new double[]{5.0/98,30.0/98,40.0/98,23.0/98,0}:new double[]{0,.05,.45,.50,0};
    for(int i=0;i<5;i++)Check(Math.Abs(normal[i]-expected[i])<.000001&&Math.Abs(normal[i]-hard[i])<.000001,"273 Safe fixed independent rarity "+size+" "+i);
    Check(p.AllowRed&&MainSafeRules240.RedChance(size)==(size==SafeSize240.Small?.01:.05),"273 Size safe separate bonus unchanged "+size);
   }
  }
  MainState273 MainSnapshot273(){
   var s=new MainState273{Map=G.Map,Vision=G.Vision,Boxes=G.Map.Loot.ToArray(),Actors=G.Enemies.ToArray(),Doors=G.Map.Doors.ConvertAll(d=>d.Open).ToArray()};
   s.Contents=Array.ConvertAll(s.Boxes,b=>JsonUtility.ToJson(new ItemList230{Items=b.Items}));s.Dead=Array.ConvertAll(s.Actors,e=>e.Dead);s.Health=Array.ConvertAll(s.Actors,e=>e.Health);return s;
  }
  void MainCache273(MainState273 s){
   Check(G.Map==s.Map&&G.Vision==s.Vision&&G.Map.Loot.Count==s.Boxes.Length&&G.Enemies.Count==s.Actors.Length,"273 Floor revisit reuses map fog loot and actor counts "+G.Map.RepairFloor220);
   for(int i=0;i<s.Boxes.Length;i++)Check(G.Map.Loot[i]==s.Boxes[i]&&s.Boxes[i].Opened&&s.Boxes[i].Prop.Searched&&JsonUtility.ToJson(new ItemList230{Items=s.Boxes[i].Items})==s.Contents[i],"273 Searched box and identified contents persist "+s.Boxes[i].Name);
   for(int i=0;i<s.Actors.Length;i++)Check(G.Enemies[i]==s.Actors[i]&&G.Enemies[i].Dead==s.Dead[i]&&G.Enemies[i].Health==s.Health[i],"273 Actor identity health and dead state cached without respawn "+s.Actors[i].name);
   for(int i=0;i<s.Doors.Length;i++)Check(G.Map.Doors[i].Open==s.Doors[i],"273 Room door state persists floor "+G.Map.RepairFloor220+" door "+i);
  }
  static bool MainBytes273(byte[] a,byte[] b){if(a==null||b==null)return a==b;if(a.Length!=b.Length)return false;for(int i=0;i<a.Length;i++)if(a[i]!=b[i])return false;return true;}
  IEnumerator VerifyMainExpansion273(){
   int firstResult=results.Count;string saveDir=Path.GetDirectoryName(G.SavePath);
   string[] formal={Path.Combine(saveDir,"stash-v40.json"),Path.Combine(saveDir,"stash-v41.json")};
   byte[][] before=Array.ConvertAll(formal,p=>File.Exists(p)?File.ReadAllBytes(p):null);
   Check(Path.GetFileName(G.SavePath)=="verification-stash-2.7.3.json","273 Uses isolated verification-stash-2.7.3 inventory");
   FreshCamp();string camp=JsonUtility.ToJson(G.Save);MainRules273();G.HardMode=false;G.HarborWalkthrough=true;G.BeginTraining();G.Paused=true;yield return null;
   var outside=G.Map;var exteriorActors=G.Enemies.ToArray();float zoom=G.View.orthographicSize;var player=G.Player;var actorScale=player.transform.lossyScale;
   var floors=new MainState273[4];var patrolRows=new List<string>{"floor,actor,actualDistance60seconds,collision"};var boxRows=new List<string>{"floor,container,standX,standZ,opened"};
   G.Player.transform.position=RaidMap.RepairMainApproach220(1);Physics.SyncTransforms();
   yield return MainPortal273(outside.Doors.Find(d=>d.Destination=="repair-entry:"+RaidMap.RepairMainGateKeys220[1]),"middle entrance");
   for(int floor=1;floor<=4;floor++){
    Check(G.Map.RepairMain220&&G.Map.RepairFloor220==floor,"273 Real runtime floor "+floor);MainShell273(floor,zoom);
    Check(G.Map.Loot.Count==(floor==2?8:floor==4?6:7)&&G.Enemies.Count==(floor==1||floor==4?3:2),"273 Declared box and enemy population floor "+floor);
    Check(G.Player==player&&G.Player.transform.lossyScale==actorScale&&!outside.Root.gameObject.activeInHierarchy&&Array.TrueForAll(exteriorActors,e=>!e.gameObject.activeInHierarchy),"273 Same physical player and isolated exterior on floor "+floor);
    yield return MainRooms273(floor);if(floor==4)MainMeeting273();MainPatrol273(floor,patrolRows);
    foreach(var box in G.Map.Loot){
     var profile=box.DropProfile230;Check(profile!=null&&profile.Region==DropRegion230.Repair&&box.Items.Count>=1&&box.Items.Count<=7,"273 Actual box retains repair profile and bounded quantity "+box.Name);
     Check(box.Items.TrueForAll(i=>ContainerDropRules230.Eligible(profile,i.Kind,RaidInventory.Grade(i.Kind)==4&&(profile.Usage!=ContainerUsage230.Safe||profile.SafeSize!=SafeSize240.Legacy))),"273 Actual contents retain declared eligibility "+box.Name);
     bool walked=WalkMain273(box.Position);
     if(!ProbeMain240(walked,"273 Actual player reaches searchable stand with .34m clearance "+box.Name))continue;
     G.Player.Aim=(box.Prop.transform.position-G.Player.transform.position).normalized;G.Rig.Tick(0);G.Vision.Refresh(G.Player.transform.position,G.Player.Aim);
     if(!ProbeMain240(G.CanInteractLoot(box),"273 Search stand reaches intended physical prop "+box.Name))continue;
     F111(false,0);F111(true,box.SearchDuration+.02f);
     bool opened=box.Opened&&G.OpenLoot==box&&box.Prop.Searched;ProbeMain240(opened,"273 Actual F opens intended container "+box.Name);
     if(opened){G.Paused=false;for(int n=0;n<7;n++)G.TickIdentification(3);G.Paused=true;Check(box.Items.TrueForAll(i=>i.Identified),"273 Identification finishes in reached box "+box.Name);}
     G.CloseInventory();F111(false,0);boxRows.Add(floor+","+Csv230(box.Name)+","+box.Position.x+","+box.Position.z+","+opened);yield return null;
    }
    foreach(var d in G.Map.Doors)if(d.Portal){var goal=d.Destination.StartsWith("repair-exit:")?d.Position+Vector3.forward:d.Position;MainWalk273(goal,"every available portal "+d.Name);}
    yield return RepairWorldPicture149("273-main-floor"+floor+"-world.png",Vector3.zero,45,48,false,2000,1600);
    yield return RepairWorldPicture149("273-main-floor"+floor+"-plan.png",Vector3.zero,0,35,true,2000,2200);
    // State markers exercise cache persistence without introducing unrelated combat/drop tests.
    G.Enemies[0].Health=Mathf.Max(1,G.Enemies[0].Health-7);G.Enemies[0].Dead=true;G.Enemies[0].HitCollider.enabled=false;
    floors[floor-1]=MainSnapshot273();if(floor<4)yield return MainStair273(floor+1,true);
   }
   for(int floor=3;floor>=1;floor--){yield return MainStair273(floor,false);MainCache273(floors[floor-1]);}
   for(int floor=2;floor<=4;floor++){yield return MainStair273(floor,false);MainCache273(floors[floor-1]);}
   for(int floor=3;floor>=1;floor--){yield return MainStair273(floor,true);MainCache273(floors[floor-1]);}
   for(int entry=0;entry<3;entry++){
    var exit=G.Map.Doors.Find(d=>d.Destination=="repair-exit:"+RaidMap.RepairMainGateKeys220[entry]);
    Check(exit!=null,"273 Exterior exit exists "+entry);MainWalk273(exit.Position+Vector3.forward,"chosen exterior exit "+entry);
    yield return MainPortal273(exit,"exit "+entry);
    Check(G.Map==outside&&Vector3.Distance(G.Player.transform.position,RaidMap.RepairMainApproach220(entry))<.03f&&G.Enemies.Count==exteriorActors.Length,"273 Exit restores corresponding unchanged exterior "+entry);
    // Outdoor geometry is unchanged; place at each declared shutter to focus input checks on the affected interior.
    G.Player.transform.position=RaidMap.RepairMainApproach220(entry);Physics.SyncTransforms();
    yield return MainPortal273(outside.Doors.Find(d=>d.Destination=="repair-entry:"+RaidMap.RepairMainGateKeys220[entry]),"entrance "+entry);
    Check(Vector3.Distance(G.Player.transform.position,RaidMap.RepairMainEntryArrival220(entry))<.03f,"273 Each of three shutters has correct expanded arrival "+entry);MainCache273(floors[0]);
   }
   File.WriteAllLines(Path.Combine(folder,"main-expansion-273-patrol.csv"),patrolRows);File.WriteAllLines(Path.Combine(folder,"main-expansion-273-search.csv"),boxRows);
   G.Controls=new DesktopInput();G.Finish(false);yield return null;
   Check(JsonUtility.ToJson(G.Save)==camp,"273 Preview restores camp inventory exactly");
   for(int i=0;i<formal.Length;i++)Check(MainBytes273(before[i],File.Exists(formal[i])?File.ReadAllBytes(formal[i]):null),"273 Formal save remains byte-for-byte unchanged "+Path.GetFileName(formal[i]));
   Check(!results.GetRange(firstResult,results.Count-firstResult).Exists(s=>s.StartsWith("FAIL")),"273 Every affected expansion placement and interaction check passes");
  }
 }
}
