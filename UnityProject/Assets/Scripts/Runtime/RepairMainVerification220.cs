using System;
using System.Collections;
using System.IO;
using UnityEngine;
namespace GreyHarbor {
 public sealed partial class RaidVerification {
  // Exercise the new routes through the real input/movement path, with one inspection per floor.
  IEnumerator VerifyRepairMain220(){
   FreshCamp();string saved=JsonUtility.ToJson(G.Save);
   string formalPath=Path.Combine(Path.GetDirectoryName(G.SavePath),"stash-v40.json");
   string formalBefore=File.Exists(formalPath)?File.ReadAllText(formalPath):null;
   Check(G.SavePath.Contains("2.2")&&!G.SavePath.EndsWith("stash-v40.json"),"220 Verification uses isolated version save");
   G.HardMode=false;G.HarborWalkthrough=true;G.BeginTraining();G.Paused=true;yield return null;
   var outside=G.Map;var outsideVision=G.Vision;var actors=G.Enemies.ToArray();var player=G.Player;var bag=G.Bag;
   float zoom=G.View.orthographicSize;G.Player.Health=73;G.Magazine=17;
   Check(outside.Loot.Count==30&&actors.Length==12,"220 Exterior retains thirty containers and twelve enemies");
   var oldKeys=new[]{"portbooth","portservice","yardhome","yardshed","ferry1","repairannex"};
   Check(outside.Doors.Count==9&&outside.Doors.TrueForAll(d=>d.Portal),"220 Exterior has nine usable portals");
   foreach(var key in oldKeys)Check(outside.Doors.FindAll(d=>d.Destination==key).Count==1,"220 Original entrance retained "+key);
   for(int i=0;i<3;i++)Check(outside.Doors.FindAll(d=>d.Destination=="repair-entry:"+RaidMap.RepairMainGateKeys220[i]).Count==1,"220 Main entrance unique "+i);
   Check(!outside.Loot.Exists(l=>RaidMap.RepairBounds149.Contains(new Vector2(l.Position.x,l.Position.z)))&&!Array.Exists(actors,e=>RaidMap.RepairBounds149.Contains(new Vector2(e.Home.x,e.Home.z))),"220 No exterior repair loot or enemy additions");
   G.Player.transform.position=RaidMap.YardGate+Vector3.back*2;Physics.SyncTransforms();
   Check(Walk140(RaidMap.YardGate+Vector3.forward*3),"220 Actual walking passes courtyard gate");
   yield return RepairWorldPicture149("220-main-three-entrances.png",RaidMap.PortPoint(26.5f,87),45,18,false,2000,1400);
   var floors=new RaidMap[4];var visions=new RaidVision[4];
   for(int entry=0;entry<3;entry++){
    var approach=RaidMap.RepairMainApproach220(entry);
    Check(Clear140(approach)&&Walk140(approach),"220 Actual outdoor walk reaches shutter "+entry);
    float outdoorYaw=45+entry*90;G.Rig.ResetView(outdoorYaw);G.Rig.Tick(0);
    yield return RepairPortal220("main entry "+entry);
    Check(G.Map!=outside&&G.Map.RepairMain220&&G.Map.RepairFloor220==1,"220 Real F enters floor one through shutter "+entry);
    Check(Vector3.Distance(G.Player.transform.position,RaidMap.RepairMainEntryArrival220(entry))<.03f&&Clear140(G.Player.transform.position),"220 Corresponding arrival stand clear "+entry);
    if(entry==0){floors[0]=G.Map;visions[0]=G.Vision;}else Check(G.Map==floors[0]&&G.Vision==visions[0],"220 All shutters share persistent first floor "+entry);
    Check(!outside.Root.gameObject.activeInHierarchy&&Array.TrueForAll(actors,e=>!e.gameObject.activeInHierarchy)&&G.Enemies.Count==0,"220 Exterior geometry and enemies inactive indoors "+entry);
    Check(G.Player==player&&G.Bag==bag&&G.Player.Health==73&&G.Magazine==17&&Mathf.Abs(G.View.orthographicSize-zoom)<.001f,"220 Entry preserves player state and scale "+entry);
    F111(true,3);Check(G.Map==floors[0]&&G.DoorHold==0,"220 Held entry input cannot immediately exit "+entry);F111(false,0);
    int exit=(entry+1)%3;Check(Walk140(RaidMap.RepairMainEntryArrival220(exit)),"220 Actual indoor walk reaches chosen different exit "+exit);
    G.Rig.ResetView(225);G.Rig.Tick(0);yield return RepairPortal220("chosen exit "+exit);
    Check(G.Map==outside&&G.Vision==outsideVision&&Vector3.Distance(G.Player.transform.position,RaidMap.RepairMainApproach220(exit))<.03f,"220 Chosen shutter restores corresponding exterior stand "+exit);
    Check(Mathf.Abs(Mathf.DeltaAngle(G.Rig.Yaw,outdoorYaw))<.1f&&Mathf.Abs(G.View.orthographicSize-zoom)<.001f&&G.Enemies.Count==12&&Shader.GetGlobalTexture("_HarborVisibility")==outsideVision.Texture,"220 Exit restores original outdoor view scale fog and enemies "+exit);
   }
   Check(Walk140(RaidMap.RepairMainApproach220(1)),"220 Player walks to middle entrance for floor tour");
   F111(false,0);F111(true,2.01f);G.Paused=true;yield return null;
   for(int floor=1;floor<=4;floor++){
    floors[floor-1]=G.Map;visions[floor-1]=G.Vision;
    Check(G.Map.RepairMain220&&G.Map.RepairFloor220==floor&&RectClose200(G.Map.Bounds,new Rect(-13.5f,-16,27,32)),"220 Floor footprint and number correct "+floor);
    Check(G.Map.Loot.Count==0&&G.Enemies.Count==0&&G.Map.Root.GetComponentsInChildren<ContainerProp>(true).Length==0&&!Array.Exists(G.Map.Root.GetComponentsInChildren<RaidActor>(true),a=>a!=G.Player),"220 Floor has no searchable stock or enemies "+floor);
    Check(G.Vision.RoomGate&&Shader.GetGlobalTexture("_HarborVisibility")==G.Vision.Texture&&Mathf.Abs(G.View.orthographicSize-zoom)<.001f,"220 Active floor owns fog with unchanged scale "+floor);
    for(int previous=0;previous<floor-1;previous++)Check(!floors[previous].Root.gameObject.activeInHierarchy&&visions[previous]!=G.Vision,"220 Previous floor hidden with independent fog "+floor+"/"+(previous+1));
    if(floor==2){for(float x=3;x<9.4f;x+=.4f)for(float z=-11.7f;z<-11.25f;z+=.05f){var point=new Vector3(x,0,z);Check(G.Vision.RoomAllowed(point)&&G.Vision.Explored(point),"223 Second floor southeast public walkway has continuous fog memory "+point);}}
    yield return VerifyRepairRooms220(floor);
    VerifyRepairShell220(G.Map,floor);
    if(floor==2){
     G.Player.transform.position=new Vector3(10.65f,0,-10.6f);Physics.SyncTransforms();G.Player.Aim=Vector3.forward;G.Rig.ResetView(45);G.Rig.Tick(0);G.Vision.Refresh(G.Player.transform.position,G.Player.Aim);G.Paused=false;G.Notice="";G.NoticeTime=0;F111(false,0);
     yield return null;yield return new WaitForEndOfFrame();var screen=ScreenCapture.CaptureScreenshotAsTexture();File.WriteAllBytes(Path.Combine(folder,"223-stair-directions-ui.png"),screen.EncodeToPNG());UnityEngine.Object.Destroy(screen);G.Paused=true;
    }
    G.Rig.ResetView(45+floor*30);G.Rig.Tick(0);
    if(floor<4)yield return RepairStair220(floor+1,true);
   }
   var fourth=G.Map;Check(!fourth.Doors.Exists(d=>d.Portal&&d.Destination.StartsWith("repair-floor:5:")),"220 Fourth floor has no invalid fifth-floor portal");
   G.SwitchInterior("repair-floor:5:north");Check(G.Map==fourth,"220 Invalid floor five request cannot change map");
   for(int floor=3;floor>=1;floor--){
    yield return RepairStair220(floor,false);
    Check(G.Map==floors[floor-1]&&G.Vision==visions[floor-1],"220 South staircase restores cached map and fog "+floor);
    Check(Mathf.Abs(Mathf.DeltaAngle(G.Rig.Yaw,45+floor*30))<.1f&&Mathf.Abs(G.View.orthographicSize-zoom)<.001f,"220 Floor restores its saved camera and outdoor scale "+floor);
    Check(G.Map.RepairMainRooms210.TrueForAll(r=>r.Door.Locked?!r.Door.Open&&!G.Vision.RoomAllowed(RoomCenter200(r.Area)):r.Door.Open&&G.Vision.Explored(RoomCenter200(r.Area))),"220 Floor door states and exploration persist "+floor);
    for(int other=0;other<4;other++)Check(floors[other].Root.gameObject.activeInHierarchy==(other==floor-1),"220 Only current floor renders "+floor+"/"+(other+1));
   }
   Check(!G.Map.Doors.Exists(d=>d.Portal&&d.Destination.StartsWith("repair-floor:0:")),"220 First floor has no underground portal");
   // Revisit through the opposite up/down pair as well.
   yield return RepairStair220(2,false);Check(G.Map==floors[1],"220 South up staircase reaches persistent second floor");
   yield return RepairStair220(1,true);Check(G.Map==floors[0],"220 North down staircase returns to persistent first floor");
   Check(Walk140(RaidMap.RepairMainEntryArrival220(0)),"220 Floor tour returns to exterior shutter by actual walking");
   F111(false,0);F111(true,2.01f);G.Paused=true;yield return null;
   Check(G.Map==outside&&G.Vision==outsideVision&&outside.Loot.Count==30&&G.Enemies.Count==12,"220 Floor tour restores unchanged exterior population");
   VerifyRepairNoHole220(outside,"exterior");
   G.Controls=new DesktopInput();G.Finish(false);yield return null;
   Check(G.Phase==GamePhase.Base&&!G.TrainingActive&&G.Enemies.Count==0&&G.ActiveSpits==0&&!outside.Root&&Array.TrueForAll(floors,m=>!m.Root),"220 Finish disposes all four floors and exterior actors");
   Check(JsonUtility.ToJson(G.Save)==saved,"220 Preview preserves exact camp inventory");
   Check(formalBefore==null?!File.Exists(formalPath):File.Exists(formalPath)&&File.ReadAllText(formalPath)==formalBefore,"220 Formal save remains byte-for-byte unchanged");
  }
  IEnumerator RepairPortal220(string label){
   var before=G.Map;F111(false,0);F111(true,1);
   G.Controls=new TouchInput{Searching=true,Shooting=true};G.TickInteraction(.2f);
   Check(G.Map==before&&G.DoorHold==0,"220 Fire cancels portal hold "+label);
   F111(false,0);F111(true,1);F111(false,0);Check(G.Map==before&&G.DoorHold==0,"220 Release cancels portal hold "+label);
   F111(true,1.99f);Check(G.Map==before&&G.DoorHold>1.9f,"220 Portal requires full two seconds "+label);
   F111(true,.02f);G.Paused=true;yield return null;
  }
  IEnumerator RepairStair220(int target,bool north){
   int source=G.Map.RepairFloor220;string destination="repair-floor:"+target+":"+(north?"north":"south");
   var portal=G.Map.Doors.Find(d=>d.Portal&&d.Destination==destination);
   Check(portal!=null,"220 Intended staircase portal exists "+source+" to "+target+" "+north);
   if(portal==null)yield break;
   Check(Clear140(portal.Position)&&Walk140(portal.Position),"220 Actual player reaches staircase portal "+destination);
   yield return RepairPortal220(destination);
   Check(G.Map.RepairFloor220==target&&Vector3.Distance(G.Player.transform.position,RaidMap.RepairStairArrival220(north))<.03f&&Clear140(G.Player.transform.position),"220 Stair destination and safe landing correct "+destination);
   F111(true,3);Check(G.Map.RepairFloor220==target&&G.DoorHold==0,"220 Held stair input cannot bounce between floors "+destination);F111(false,0);
  }
  IEnumerator VerifyRepairRooms220(int floor){
   var map=G.Map;
   Check(map.RepairMainRooms210.FindAll(r=>r.ReservedStair).Count==2&&map.RepairMainRooms210.Exists(r=>!r.ReservedStair),"220 Floor has two stair enclosures and real rooms "+floor);
   var arrivalRoom=map.RepairMainRooms210.Find(r=>r.ReservedStair&&r.Area.Contains(new Vector2(G.Player.transform.position.x,G.Player.transform.position.z)));
   if(arrivalRoom!=null&&!arrivalRoom.Door.Open){
    Check(G.Vision.Explored(RoomCenter200(arrivalRoom.Area))&&G.Vision.RoomAllowed(G.Player.transform.position),"220 Stair landing reveals only its occupied enclosure "+floor);
    var insideDirection=(arrivalRoom.WalkPoint-arrivalRoom.ApproachPoint).normalized;
    var safe=arrivalRoom.Door.Position+insideDirection*.60f+Vector3.forward*1.26f;
    Check(Clear140(safe)&&Walk140(safe),"220 Player reaches safe door stand inside arrival stairwell "+floor);
    F111(false,0);F111(true,.01f);F111(false,0);
    Check(arrivalRoom.Door.Open&&Walk140(arrivalRoom.ApproachPoint),"220 Real F opens arrival stair door and actual player leaves landing "+floor);
   }
   foreach(var room in map.RepairMainRooms210){
    var center=RoomCenter200(room.Area);
    if(room==arrivalRoom){Check(room.Door.Open&&G.Vision.Explored(center),"220 Arrival stair remains revealed after entering hall "+floor);continue;}
    Check(!room.Door.Open&&!G.Vision.Explored(center)&&!G.Vision.RoomAllowed(center)&&!G.Vision.CanSee(center),"220 Closed room starts black "+floor+" "+room.Name);
    Check(Clear140(room.ApproachPoint)&&BlockedShot210(room.ApproachPoint,room.WalkPoint)&&!G.ClearSight(room.ApproachPoint,room.WalkPoint)&&Unreachable210(room.ApproachPoint,room.WalkPoint),"220 Closed room blocks walking sight and shots without bypass "+floor+" "+room.Name);
   }
   G.Player.Aim=Vector3.forward;G.Rig.Tick(0);G.Vision.Refresh(G.Player.transform.position,G.Player.Aim);G.Player.Draw(G.View,0);yield return Capture("220-floor-"+floor+"-closed.png");
   foreach(var room in map.RepairMainRooms210){
    Check(Walk140(room.ApproachPoint),"220 Actual player reaches room approach "+floor+" "+room.Name);
    if(!room.Door.Open){
     if(!room.ReservedStair){var front=room.Door.Position+(room.ApproachPoint-room.WalkPoint).normalized*1.05f;Check(Clear140(front)&&Walk140(front),"222 Player reaches room door within interaction range "+floor+" "+room.Name);}
     if(room.ReservedStair){
      var hallDirection=(room.ApproachPoint-room.WalkPoint).normalized;
      var front=room.Door.Position+hallDirection*.60f;
      Check(Clear140(front)&&Walk140(front),"221 Actual player approaches stair door straight from hall "+floor+" "+room.Name);
      Check(!room.Door.OpenSafety.Contains(new Vector2(G.Player.transform.position.x,G.Player.transform.position.z)),"221 Normal frontal stand is outside door sweep "+floor+" "+room.Name);
     }
     F111(false,0);F111(true,.01f);F111(false,0);
    }
    Check(Walk140(room.ApproachPoint),"222 Player returns to approach after F interaction "+floor+" "+room.Name);
    if(room.Door.Locked){
     Check(!room.Door.Open&&!G.Vision.RoomAllowed(RoomCenter200(room.Area))&&Unreachable210(room.ApproachPoint,room.WalkPoint),"220 Key room remains physically locked and black "+floor+" "+room.Name);
     var direction=(room.WalkPoint-room.ApproachPoint).normalized;
     for(int i=0;i<45;i++){G.Player.Move(direction*.08f);Physics.SyncTransforms();}
     Check(!room.Area.Contains(new Vector2(G.Player.transform.position.x,G.Player.transform.position.z)),"220 Actual movement cannot enter key room "+floor+" "+room.Name);
     // Return to the declared clear stand: actor and navigation capsule radii differ.
     G.Player.transform.position=room.ApproachPoint;Physics.SyncTransforms();continue;
    }
    Check(room.Door.Open&&G.Vision.Explored(RoomCenter200(room.Area)),"220 F opens room and reveals exploration "+floor+" "+room.Name);
    Check(G.ClearSight(room.ApproachPoint,room.WalkPoint)&&!BlockedShot210(room.ApproachPoint,room.WalkPoint),"220 Open doorway passes sight and shots "+floor+" "+room.Name);
    Check(Clear140(room.WalkPoint),"220 Open room walk destination has navigation capsule clearance "+floor+" "+room.Name+" "+room.WalkPoint);
    Check(Walk140(room.WalkPoint),"220 Actual player walks through open room doorway "+floor+" "+room.Name);
    if(room.ReservedStair){
     Check(room.Door.InstantToggle,"222 Narrow stair door uses direct toggle "+floor+" "+room.Name);
     var inside=room.Door.Position+(room.WalkPoint-room.ApproachPoint).normalized*.60f;
     Check(Clear140(inside)&&Walk140(inside),"222 Player reaches normal close stand inside stairwell "+floor+" "+room.Name);
     F111(false,0);F111(true,.01f);F111(false,0);
     Check(!room.Door.Open&&BlockedShot210(room.ApproachPoint,room.WalkPoint),"222 Real F closes from inside without swing clearance "+floor+" "+room.Name);
     F111(true,.01f);F111(false,0);Check(room.Door.Open,"222 Real F reopens from inside without sweep rejection "+floor+" "+room.Name);
     G.Player.transform.position=room.Door.Position;Physics.SyncTransforms();
     Check(!G.ToggleRoomDoor(room.Door)&&room.Door.Open,"222 Direct close still protects occupied doorway "+floor+" "+room.Name);
     G.Player.transform.position=inside;Physics.SyncTransforms();
    }
    foreach(var other in map.RepairMainRooms210)if(other!=room&&!other.Door.Open)Check(!G.Vision.Explored(RoomCenter200(other.Area)),"220 Opening room preserves other closed room fog "+floor+" "+other.Name);
    Check(Walk140(room.ApproachPoint)&&G.ToggleRoomDoor(room.Door)&&!room.Door.Open&&G.Vision.Explored(RoomCenter200(room.Area))&&BlockedShot210(room.ApproachPoint,room.WalkPoint),"220 Closing restores collision and keeps exploration "+floor+" "+room.Name);
    Check(G.ToggleRoomDoor(room.Door)&&room.Door.Open,"220 Visited room reopens for floor tour "+floor+" "+room.Name);
   }
   foreach(var point in map.RepairMainWalkPoints210)Check(Clear140(point)&&Walk140(point),"220 Actual hall and opened stair circulation clear "+floor+" "+point);
   if(floor==4){
    var key=map.RepairMainRooms210.Find(r=>r.Door.Locked&&!r.ReservedStair);
    Check(key!=null,"220 Fourth floor contains reserved key room");
    if(key!=null){bool empty=true;var inner=key.Area;inner.xMin+=.45f;inner.xMax-=.45f;inner.yMin+=.45f;inner.yMax-=.45f;
     foreach(var collider in map.Root.GetComponentsInChildren<Collider>(true)){var b=collider.bounds;if(b.max.y>.4f&&inner.Contains(new Vector2(b.center.x,b.center.z)))empty=false;}
     Check(empty,"220 Locked key room has no physical furnishings or stock");
    }
   }
   foreach(float yaw in new[]{45f,135f}){
    G.Player.transform.position=map.RepairMainWalkPoints210[map.RepairMainWalkPoints210.Count/2];Physics.SyncTransforms();
    G.Player.Aim=Vector3.forward;G.Rig.ResetView(yaw);G.Rig.Tick(0);G.Vision.Refresh(G.Player.transform.position,G.Player.Aim);G.Player.Draw(G.View,0);
    yield return null;yield return Capture("220-floor-"+floor+"-play-"+yaw+".png");
   }
   G.Player.VisibleToPlayer=false;G.Player.Draw(G.View,0);
   yield return RepairWorldPicture149("220-floor-"+floor+"-overview-45.png",Vector3.zero,45,26,false,2000,1600);
   yield return RepairWorldPicture149("220-floor-"+floor+"-overview-135.png",Vector3.zero,135,26,false,2000,1600);
   yield return RepairWorldPicture149("220-floor-"+floor+"-plan.png",Vector3.zero,0,18,true,2000,2000);
   G.Player.VisibleToPlayer=true;G.Player.Draw(G.View,0);
  }
  void VerifyRepairShell220(RaidMap map,int floor){
   VerifyRepairNoHole220(map,"floor "+floor);
   var ground=map.Root.Find("Harbor_StableGround137");
   Check(map.StableGround137&&ground&&ground.GetComponent<MeshFilter>().sharedMesh.vertexCount==4&&ground.GetComponent<Renderer>().sharedMaterial.GetFloat("_UseStableGround")==1,"220 Floor decoration uses one stable baked ground plane "+floor);
   var shell=map.Root.GetComponent<RepairWallModel220>();
   Check(shell&&shell.PerimeterWalls.Count==4&&shell.CapCount>=4,"220 Four complete perimeter walls and caps declared "+floor);
   if(shell)foreach(var wall in shell.PerimeterWalls){
    Check(wall&&wall.sharedMaterial&&wall.sharedMaterial.shader.name=="GreyHarbor/RepairWall220"&&wall.bounds.size.y>=3.19f,"220 Full-height perimeter uses uniform wall shader "+floor+" "+(wall?wall.name:"missing"));
   }
   foreach(var p in new[]{new Vector3(-13.395f,0,0),new Vector3(13.395f,0,0),new Vector3(0,0,-15.895f),new Vector3(0,0,15.895f)}){
    Check(Physics.Linecast(p+Vector3.up*.8f-((Mathf.Abs(p.x)>13)?Vector3.right:Vector3.forward)*.5f,p+Vector3.up*.8f+((Mathf.Abs(p.x)>13)?Vector3.right:Vector3.forward)*.5f,SceneObstacle.MovementMask,QueryTriggerInteraction.Ignore),"220 Full perimeter has physical wall "+floor+" "+p);
   }
  }
  void VerifyRepairNoHole220(RaidMap map,string label){
   bool clean=true;foreach(var component in map.Root.GetComponentsInChildren<MonoBehaviour>(true))if(component&&component.GetType().Name=="RepairCutaway200")clean=false;
   foreach(var renderer in map.Root.GetComponentsInChildren<Renderer>(true))foreach(var material in renderer.sharedMaterials)if(material&&material.shader&&(material.shader.name=="GreyHarbor/RepairCutaway200"||material.shader.name.IndexOf("PlayerHole",StringComparison.OrdinalIgnoreCase)>=0))clean=false;
   Check(clean,"220 No player-centred hole component or shader "+label);
  }
 }
}
