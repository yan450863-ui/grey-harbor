using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Rendering;
namespace GreyHarbor {
 public sealed partial class RaidVerification {
  bool DressingBounds274(GameObject root,out Bounds bounds){
   bounds=new Bounds();bool any=false;foreach(var r in root.GetComponentsInChildren<Renderer>())if(r.enabled){if(!any){bounds=r.bounds;any=true;}else bounds.Encapsulate(r.bounds);}return any;
  }
  float DressingDistance274(Rect r,Vector3 p){float x=p.x-Mathf.Clamp(p.x,r.xMin,r.xMax),z=p.z-Mathf.Clamp(p.z,r.yMin,r.yMax);return Mathf.Sqrt(x*x+z*z);}
  void DressingWallFurniture274(List<string> rows){
   int wallCount=0,centerCount=0;
   foreach(var f in G.Map.Root.GetComponentsInChildren<RepairMainFurniture274>()){
    Bounds b;Check(DressingBounds274(f.gameObject,out b),"274 Furnishing has real rendered geometry "+f.name);
    if(!f.WallBack){centerCount++;continue;}wallCount++;
    Check(f.WallSide>=0&&f.WallSide<=3,"274 Wall furnishing declares service direction "+f.name);
    Vector3 outward=f.WallSide==0?Vector3.forward:f.WallSide==1?Vector3.right:f.WallSide==2?Vector3.back:Vector3.left;
    Vector3 edge=new Vector3(f.WallSide==1?b.max.x:f.WallSide==3?b.min.x:b.center.x,.8f,f.WallSide==0?b.max.z:f.WallSide==2?b.min.z:b.center.z);
    float gap=float.PositiveInfinity;Collider wall=null;
    foreach(var hit in Physics.RaycastAll(edge,outward,1,SceneObstacle.SightMask,QueryTriggerInteraction.Ignore))if(hit.collider.name=="InteriorWall"&&hit.distance<gap){gap=hit.distance;wall=hit.collider;}
    Check(wall!=null&&gap>=.049f&&gap<=.151f,"274 Cabinet/rack rear lies .05-.15m from actual solid wall "+f.RoomName+" / "+f.name+" gap="+gap);
    Check(Vector3.Dot(f.transform.forward,outward)>.99f,"274 Cabinet/rack front faces the room "+f.name);
   rows.Add(G.Map.RepairFloor220+","+Csv230(f.RoomName)+","+Csv230(f.name)+",wall,"+gap+","+b.center.x+","+b.center.z+","+b.size.x+","+b.size.z);
   }
   foreach(var c in G.Map.Root.GetComponentsInChildren<Collider>())if(c.name=="RepairOfficeDesk220"||c.name=="RepairMeetingTable220")centerCount++;
   Check(wallCount>=2&&centerCount>=1,"274 Floor retains wall furnishings and a grouped interior arrangement "+G.Map.RepairFloor220);
  }
  void DressingShadow274(){
   int walls=0,contact=0;
   foreach(var r in G.Map.Root.GetComponentsInChildren<Renderer>()){
    if(r.sharedMaterial&&r.sharedMaterial.shader&&r.sharedMaterial.shader.name=="GreyHarbor/RepairWall220"){
     walls++;Check(r.shadowCastingMode==ShadowCastingMode.Off&&!r.receiveShadows,"274 Wall rejects outdoor sun shadow bands "+r.name);
    }else if(r.shadowCastingMode==ShadowCastingMode.On&&r.bounds.size.y>.2f)contact++;
   }
   Check(walls>4&&contact>0,"274 Physical wall shadow treatment retains furniture contact shadows floor "+G.Map.RepairFloor220);
   Check(RaidVision.HalfAngle==35&&G.Vision.RoomGate&&Shader.GetGlobalFloat("_HarborFogEnabled")==1,"274 Actual 70-degree flashlight and black room fog remain enabled");
  }
  void DressingItems274(List<string> rows,HashSet<string> types){
   var map=G.Map;Check(map.MainDecorationItems274.Count>0,"274 Floor contains new decoration with declared occupancy "+map.RepairFloor220);
   int volumes=0;
   foreach(var item in map.MainDecorationItems274){
    Bounds b=new Bounds();Check(item.View&&DressingBounds274(item.View,out b),"274 Actual decoration geometry exists "+item.Name);
    types.Add(item.Type.ToString());
    Check(item.WallMounted?b.size.magnitude>.15f:b.size.x>.02f&&b.size.y>.02f&&b.size.z>.005f,"274 Decoration has visible geometry and floor objects have volume "+item.Name);
    if(!item.WallMounted&&item.Kind!=ObstacleKind.Decoration){
     volumes++;bool collider=false;foreach(var c in item.View.GetComponentsInChildren<Collider>())if(c.enabled&&(SceneObstacle.MovementMask&(1<<c.gameObject.layer))!=0)collider=true;
     Check(collider,"274 Floor furnishing uses actual physical collision "+item.Name);
     foreach(var box in map.Loot)Check(DressingDistance274(item.Footprint,box.Position)>=.34f,"274 Decoration keeps searchable actor stand clear "+item.Name+" / "+box.Name);
     foreach(var door in map.Doors)if(!door.Portal)Check(!item.Footprint.Overlaps(door.ClosedSafety)&&!item.Footprint.Overlaps(door.OpenSafety),"274 Decoration avoids door opening and closing occupancy "+item.Name+" / "+door.Name);
    }
    rows.Add(map.RepairFloor220+","+Csv230(item.RoomName)+","+Csv230(item.Name)+","+Csv230(item.Type.ToString())+","+item.Footprint.xMin+","+item.Footprint.yMin+","+item.Footprint.width+","+item.Footprint.height+","+item.Kind+","+item.WallMounted);
   }
   Check(volumes>=3,"274 New dressing includes substantial floor objects "+map.RepairFloor220);
   foreach(var box in map.Loot)Check(Clear140(box.Position),"274 All existing search stands remain physically clear "+box.Name);
  }
  void DressingSearchPlacement274(){
   Check(G.Map.MainSearchPlacements274.Count==G.Map.Loot.Count,"274 Every moved searchable prop has an actual placement record");
   foreach(var p in G.Map.MainSearchPlacements274){
    Bounds b=new Bounds();Check(p.Box.Prop&&DressingBounds274(p.Box.Prop.gameObject,out b),"274 Search prop has real closed bounds "+p.Box.Name);
    if(p.Support){Bounds support;Check(DressingBounds274(p.Support,out support),"274 Box support exists "+p.Box.Name);b.Encapsulate(support);}
    if(p.WallAligned){
     Vector3 outward=-p.Inward;var edge=new Vector3(outward.x>.5f?b.max.x:outward.x<-.5f?b.min.x:b.center.x,.8f,outward.z>.5f?b.max.z:outward.z<-.5f?b.min.z:b.center.z);
     float gap=float.PositiveInfinity;foreach(var hit in Physics.RaycastAll(edge,outward,1,SceneObstacle.SightMask,QueryTriggerInteraction.Ignore))if(hit.collider.name=="InteriorWall")gap=Mathf.Min(gap,hit.distance);
     Check(gap>=.049f&&gap<=.151f,"274 Search prop and support actually back onto solid wall "+p.Box.Name+" gap="+gap);
    }
    if(p.Box.Prop.transform.position.y>.1f){
     var anchor=p.Box.Prop.transform.position;float surface=float.NegativeInfinity;
     foreach(var hit in Physics.RaycastAll(anchor+Vector3.up*.1f,Vector3.down,2,SceneObstacle.MovementMask,QueryTriggerInteraction.Ignore))
      if(!hit.collider.transform.IsChildOf(p.Box.Prop.transform)&&hit.point.y<=anchor.y+.02f)surface=Mathf.Max(surface,hit.point.y);
     float gap=anchor.y-surface;Check(gap>=-.021f&&gap<=.071f,"274 Elevated search prop rests on actual support surface "+p.Box.Name+" gap="+gap);
    }
   }
  }
  IEnumerator DressingGroups274(int floor){
   int groups=0;bool computerPicture=false;foreach(var group in G.Map.MainDecorationGroups274){if(!group.Center)continue;groups++;
    Check(group.Items.Count>=3,"274 Central daily-object group contains several real objects "+group.Name);
    var r=group.Footprint;const float aisle=.60f;
    var corners=new[]{new Vector3(r.xMin-aisle,0,r.yMin-aisle),new Vector3(r.xMax+aisle,0,r.yMin-aisle),new Vector3(r.xMax+aisle,0,r.yMax+aisle),new Vector3(r.xMin-aisle,0,r.yMax+aisle)};
    MainWalk273(corners[0],"274 central object group aisle "+group.Name);
    for(int i=1;i<=4;i++){
     Check(G.Navigation.ClearSegment(G.Player.transform.position,corners[i%4]),"274 Actual central group can be passed on side "+i+" "+group.Name);
     MainWalk273(corners[i%4],"274 around daily object group "+group.Name);
    }
    bool workstation=group.Items.Exists(i=>i.Type==MainDecorationType274.Workstation);
    if(groups==1)yield return DressingGameView274("274-floor"+floor+"-central-objects-game-view.png",group.Anchor,45);
    if(workstation&&!computerPicture){computerPicture=true;yield return DressingGameView274("274-floor"+floor+"-central-computer-game-view.png",group.Anchor,45);}
   }
   Check(groups>0,"274 Expanded interior contains a passable central furnishing group floor "+floor);
  }
  void DressingMeeting274(){
   var room=G.Map.RepairMainRooms210.Find(r=>r.Name=="会议室");Check(room!=null,"274 Fourth-floor meeting room exists");
   var table=Array.Find(G.Map.Root.GetComponentsInChildren<Collider>(),c=>c.name=="RepairMeetingTable220"&&room.Area.Contains(new Vector2(c.bounds.center.x,c.bounds.center.z)));
   Check(table!=null&&Vector3.Distance(table.bounds.size,new Vector3(4.4f,.84f,1.4f))<.001f,"274 Actual meeting table retains physical dimensions");
   var occupied=table.bounds;int chairs=0;
   foreach(var t in G.Map.Root.GetComponentsInChildren<Transform>())if(t.name=="Decor_Chair"&&room.Area.Contains(new Vector2(t.position.x,t.position.z))){
    chairs++;Bounds chair;Check(DressingBounds274(t.gameObject,out chair),"274 Meeting chair has actual rendered geometry");occupied.Encapsulate(chair);
   }
   Check(chairs==3,"274 Actual meeting room still has exactly three chairs");
   // Follow the local table-and-chair footprint. Room-wide corner goals would
   // also cross independent daily-object islands and would not measure its aisle.
   const float clearance=.50f;var ring=new[]{new Vector3(occupied.min.x-clearance,0,occupied.min.z-clearance),new Vector3(occupied.max.x+clearance,0,occupied.min.z-clearance),new Vector3(occupied.max.x+clearance,0,occupied.max.z+clearance),new Vector3(occupied.min.x-clearance,0,occupied.max.z+clearance)};
   MainWalk273(room.WalkPoint,"274 enter local meeting table aisle");MainWalk273(ring[0],"274 meeting table and chair aisle start");
   for(int i=1;i<=4;i++){
    var goal=ring[i%4];Check(room.Area.Contains(new Vector2(goal.x,goal.z))&&G.Navigation.ClearSegment(G.Player.transform.position,goal),"274 Complete table and chair group has direct .34m aisle on side "+i);
    MainWalk273(goal,"274 actual local meeting group circuit side "+i);
   }
   MainWalk273(room.ApproachPoint,"274 leave local meeting group");
  }
  IEnumerator DressingWorld274(string name,bool top){
   Check(Shader.GetGlobalFloat("_HarborFogEnabled")==1,"274 World capture uses live fog "+name);yield return new WaitForEndOfFrame();
   var go=new GameObject("QA274ActualFogCamera");var cam=go.AddComponent<Camera>();cam.CopyFrom(G.View);cam.enabled=false;
   cam.transform.rotation=Quaternion.Euler(top?90:55,top?0:45,0);cam.transform.position=Vector3.up-cam.transform.forward*130;
   cam.orthographicSize=top?35:48;cam.farClipPlane=250;SaveYardCamera136(cam,name,2000,top?2200:1600);Destroy(go);yield return null;
  }
  IEnumerator DressingGameView274(string name,Vector3 target,float yaw){
   G.CloseInventory();G.Rig.ResetView(yaw);G.Rig.Tick(0);target.y=RaidGame.CombatHeight;
   var cursor=G.View.WorldToScreenPoint(target);G.Controls=new TouchInput{AimPosition=new Vector2(cursor.x,cursor.y)};G.UpdateAim();
   G.Vision.Refresh(G.Player.transform.position,G.Player.Aim);G.Player.Draw(G.View,0);
   G.Paused=true;G.NoticeTime=0;
   yield return null;yield return new WaitForEndOfFrame();
   var intended=target-G.Player.transform.position;intended.y=0;Check(Vector3.Dot(G.Player.Aim,intended.normalized)>.999f,"274 Actual rendered flashlight stays aimed at requested target "+name);
   Check(!G.InventoryOpen&&Shader.GetGlobalFloat("_HarborFogEnabled")==1,"274 Actual game camera keeps flashlight and fog "+name);
   // Hidden player windows do not render a swapchain/HUD framebuffer. Render
   // the unchanged gameplay camera directly; these images contain no IMGUI.
   var rt=new RenderTexture(1600,900,24,RenderTextureFormat.ARGB32);var prior=RenderTexture.active;var targetTexture=G.View.targetTexture;
   G.View.targetTexture=rt;G.View.Render();RenderTexture.active=rt;
   var screenshot=new Texture2D(1600,900,TextureFormat.RGB24,false);screenshot.ReadPixels(new Rect(0,0,1600,900),0,0);screenshot.Apply();
   G.View.targetTexture=targetTexture;RenderTexture.active=prior;rt.Release();Destroy(rt);
   int visible=0;foreach(var pixel in screenshot.GetPixels32())if(pixel.r>10||pixel.g>10||pixel.b>10)visible++;
   File.WriteAllBytes(Path.Combine(folder,name),screenshot.EncodeToPNG());Destroy(screenshot);
   Check(visible>=1600*900/1000,"274 Actual game-view image contains non-black scene pixels "+name+" count="+visible);F111(false,0);yield return null;
  }
  IEnumerator DressingStairPictures274(int floor){
   var room=G.Map.RepairMainRooms210.Find(r=>r.ReservedStair&&r.Area.center.x>0);Check(room!=null,"274 Southeast staircase exists floor "+floor);
   MainWalk273(RaidMap.RepairStairArrival220(false),"274 southeast stair close view");
   Vector3 east=new Vector3(room.Area.xMax-.15f,0,G.Player.transform.position.z),north=new Vector3(room.Area.xMax-.15f,0,room.Area.yMax-.15f);
   yield return DressingGameView274("274-floor"+floor+"-southeast-east-45-game-view.png",east,45);
   G.Rig.Turn(1);G.Rig.Tick(.31f);Check(Mathf.Abs(Mathf.DeltaAngle(G.Rig.Yaw,135))<.01f,"274 Actual Q turn reaches southeast wall view floor "+floor);
   yield return DressingGameView274("274-floor"+floor+"-southeast-north-135-game-view.png",north,G.Rig.Yaw);
   G.Rig.Turn(-1);G.Rig.Tick(.31f);Check(Mathf.Abs(Mathf.DeltaAngle(G.Rig.Yaw,45))<.01f,"274 Actual E turn restores southeast wall view floor "+floor);
   var south=new Vector3(room.Area.xMax-.15f,0,room.Area.yMin+.15f);yield return DressingGameView274("274-floor"+floor+"-southeast-south-45-game-view.png",south,G.Rig.Yaw);
  }
  IEnumerator DressingSearch274(LootContainer box,List<string> rows){
   int count=box.Items.Count;Check(count>0&&WalkMain273(box.Position),"274 Actual player reaches preserved search stand "+box.Name);
   G.Player.Aim=(box.Prop.transform.position-G.Player.transform.position).normalized;G.Rig.Tick(0);G.Vision.Refresh(G.Player.transform.position,G.Player.Aim);
   Check(G.CanInteractLoot(box),"274 Furniture permits interaction with actual box "+box.Name);F111(false,0);F111(true,box.SearchDuration+.02f);
   Check(box.Opened&&G.OpenLoot==box&&box.Prop.Searched,"274 Real F opens intended box among new dressing "+box.Name);
   G.Paused=false;for(int n=0;n<7;n++)G.TickIdentification(3);G.Paused=true;
   Check(box.Items.Count==count&&box.Items.TrueForAll(i=>i.Identified),"274 Dressing leaves searched contents intact "+box.Name);
   rows.Add(G.Map.RepairFloor220+","+Csv230(box.Name)+","+box.Position.x+","+box.Position.z+","+count);G.CloseInventory();F111(false,0);yield return null;
  }
  IEnumerator VerifyMainDressing274(){
   string directory=Path.GetDirectoryName(G.SavePath);string[] formal={Path.Combine(directory,"stash-v40.json"),Path.Combine(directory,"stash-v41.json")};
   byte[][] before=Array.ConvertAll(formal,p=>File.Exists(p)?File.ReadAllBytes(p):null);
   Check(Path.GetFileName(G.SavePath)=="verification-stash-2.7.4.json","274 Dressing verification uses isolated inventory");
   File.WriteAllLines(Path.Combine(folder,"274-capture-notes.txt"),new[]{"*-game-view.png: actual current gameplay camera rendered offscreen; real flashlight and black fog retained; HUD/IMGUI is not included.","*-perspective-live-fog.png and *-plan-live-fog.png: additional inspection camera views of the real runtime floor; real flashlight/fog globals retained.","Game-view captures require at least 1440 non-black pixels in the 1600x900 image. No hidden-window ScreenCapture images are used."});
   FreshCamp();string camp=JsonUtility.ToJson(G.Save);G.HardMode=false;G.HarborWalkthrough=true;G.BeginTraining();G.Paused=true;yield return null;
   var outside=G.Map;var floors=new MainState273[4];int boxes=0,enemies=0;
   var types=new HashSet<string>();var items=new List<string>{"floor,room,name,type,xMin,zMin,width,depth,obstacleKind,wallMounted"};
   var furniture=new List<string>{"floor,room,name,placement,rearGap,centerX,centerZ,width,depth"};var searches=new List<string>{"floor,name,standX,standZ,itemCount"};var patrol=new List<string>{"floor,actor,actualDistance60seconds,collision"};
   G.Player.transform.position=RaidMap.RepairMainApproach220(1);Physics.SyncTransforms();yield return MainPortal273(outside.Doors.Find(d=>d.Destination=="repair-entry:"+RaidMap.RepairMainGateKeys220[1]),"274 main entry");
   for(int floor=1;floor<=4;floor++){
    Check(G.Map.RepairMain220&&G.Map.RepairFloor220==floor,"274 Enter affected dressed floor "+floor);
    Check(G.Map.Loot.Count==(floor==2?8:floor==4?6:7)&&G.Enemies.Count==(floor==1||floor==4?3:2),"274 Furniture preserves floor box and actor counts "+floor);
    yield return MainRooms273(floor);if(floor==4)DressingMeeting274();DressingWallFurniture274(furniture);DressingShadow274();DressingItems274(items,types);DressingSearchPlacement274();
    yield return DressingGroups274(floor);MainPatrol273(floor,patrol);enemies+=G.Enemies.Count;
    foreach(var box in G.Map.Loot){yield return DressingSearch274(box,searches);boxes++;}
    foreach(var door in G.Map.Doors)if(door.Portal)MainWalk273(door.Destination.StartsWith("repair-exit:")?door.Position+Vector3.forward:door.Position,"274 changed furnishing preserves portal "+door.Name);
    yield return DressingStairPictures274(floor);yield return DressingWorld274("274-floor"+floor+"-perspective-live-fog.png",false);yield return DressingWorld274("274-floor"+floor+"-plan-live-fog.png",true);
    floors[floor-1]=MainSnapshot273();if(floor<4)yield return MainStair273(floor+1,true);
   }
   Check(boxes==28&&enemies==10,"274 All 28 actual box searches and 10 actual patrols exercised");
   foreach(var type in new[]{MainDecorationType274.Workstation,MainDecorationType274.WorkTable,MainDecorationType274.FallenChair,MainDecorationType274.LowCabinet,MainDecorationType274.PaperBoxes})
    Check(types.Contains(type.ToString()),"274 Visible daily-object type appears in actual four-floor dressing "+type);
   for(int floor=3;floor>=1;floor--){yield return MainStair273(floor,false);MainCache273(floors[floor-1]);}
   for(int floor=2;floor<=4;floor++){yield return MainStair273(floor,false);MainCache273(floors[floor-1]);}
   for(int floor=3;floor>=1;floor--){yield return MainStair273(floor,true);MainCache273(floors[floor-1]);}
   for(int entry=0;entry<3;entry++){
    var exit=G.Map.Doors.Find(d=>d.Destination=="repair-exit:"+RaidMap.RepairMainGateKeys220[entry]);MainWalk273(exit.Position+Vector3.forward,"274 shutter access "+entry);yield return MainPortal273(exit,"274 shutter exit "+entry);
    Check(G.Map==outside,"274 Chosen shutter restores exterior "+entry);G.Player.transform.position=RaidMap.RepairMainApproach220(entry);Physics.SyncTransforms();
    yield return MainPortal273(outside.Doors.Find(d=>d.Destination=="repair-entry:"+RaidMap.RepairMainGateKeys220[entry]),"274 shutter entry "+entry);MainCache273(floors[0]);
   }
   File.WriteAllLines(Path.Combine(folder,"274-decoration-occupancy.csv"),items);File.WriteAllLines(Path.Combine(folder,"274-wall-furniture.csv"),furniture);File.WriteAllLines(Path.Combine(folder,"274-box-searches.csv"),searches);File.WriteAllLines(Path.Combine(folder,"274-enemy-patrols.csv"),patrol);
   File.WriteAllLines(Path.Combine(folder,"274-visible-object-types.txt"),new List<string>(types).ToArray());
   G.Controls=new DesktopInput();G.Finish(false);yield return null;Check(JsonUtility.ToJson(G.Save)==camp,"274 Preview restores exact camp inventory");
   for(int i=0;i<formal.Length;i++)Check(MainBytes273(before[i],File.Exists(formal[i])?File.ReadAllBytes(formal[i]):null),"274 Formal save stays byte-for-byte unchanged "+Path.GetFileName(formal[i]));
  }
 }
}
