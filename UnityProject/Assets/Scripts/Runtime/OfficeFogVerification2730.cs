using System;
using System.IO;
using System.Collections;
using UnityEngine;
namespace GreyHarbor {
 public sealed partial class RaidVerification {
  void OfficeAreaSeen2730(Rect area,string label){
   bool seen=true,surface=true;var texture=G.Vision.Texture.GetPixels32();int samples=0;
   for(float x=area.xMin+.12f;x<area.xMax;x+=.5f)for(float z=area.yMin+.12f;z<area.yMax;z+=.5f){int i=G.Vision.Index(new Vector3(x,0,z));samples++;seen&=i>=0&&G.Vision.Seen[i]&&texture[i].g==255;surface&=i>=0&&G.Vision.SurfaceSeen[i]&&texture[i].b==255;}
   Check(seen&&surface,"2730 complete exploration and GPU surface mask "+label+" samples="+samples);
  }
  IEnumerator RunOfficeFog2730(){
   var controls=G.Controls;var random=UnityEngine.Random.state;string dir=Path.GetDirectoryName(G.SavePath);string[] formal={"stash-v40.json","stash-v41.json","stash-v42.json"};byte[][] original=Array.ConvertAll(formal,n=>File.Exists(Path.Combine(dir,n))?File.ReadAllBytes(Path.Combine(dir,n)):null);
   try{
    Check(Application.version=="2.7.30"&&G.SavePath.EndsWith("verification-stash-2.7.30.json"),"2730 isolated office door/fog verification");
    FreshCamp();G.Controls=new TouchInput();G.HarborWalkthrough=true;yield return G.BeginTrainingLoad2721();G.Paused=true;var outdoor=G.Map;
    OfficePreview2726.Apply(G);FreightRoomWalk2717(RaidMap.OfficeArrival2726,"2730 office arrival");var portal=outdoor.Doors.Find(d=>d.Portal&&d.Destination=="officeright1");FreightMonsterF2720(portal,true);yield return null;
    var indoor=G.Map;Check(G.Vision.RoomGate,"2730 unopened separate rooms retain exploration gating");
    Check(indoor.OfficeOpenings2726.FindAll(o=>o.Door!=null).Count==12&&indoor.OfficeOpenings2726.FindAll(o=>o.Type=="passage").Count==5&&indoor.OfficeOpenings2726.FindAll(o=>o.Type=="hole").Count==2,"2730 exactly five marked leaves removed; original two wall holes retained");
    foreach(var area in new[]{new Rect(-2,-24,4,48),new Rect(-20,-6,40,3),new Rect(-20,11,40,3)})OfficeAreaSeen2730(area,"initial public corridor");
    foreach(string id in new[]{"S1","01","02","03","07","08","09"})OfficeAreaSeen2730(indoor.OfficeRooms2726.Find(r=>r.Id==id).Area,"initial open connected room "+id);
    foreach(string id in new[]{"04","05","06","10","S2"})Check(!G.Vision.Explored(indoor.OfficeRooms2726.Find(r=>r.Id==id).Center)&&!G.Vision.RoomAllowed(indoor.OfficeRooms2726.Find(r=>r.Id==id).Center),"2730 closed independent room stays unrevealed "+id);
    foreach(var o in indoor.OfficeOpenings2726)if(o.Type=="passage"){
     var normal=OfficeNormal2727(o);Check(o.Door==null&&!Physics.CheckSphere(o.Position+Vector3.up,.18f,SceneObstacle.MovementMask,QueryTriggerInteraction.Ignore),"2730 marked opening has no door leaf "+o.Position);
     Check(Physics.Linecast(o.Position-normal*.5f+Vector3.up*2.8f,o.Position+normal*.5f+Vector3.up*2.8f,SceneObstacle.MovementMask,QueryTriggerInteraction.Ignore),"2730 marked opening retains connected door header "+o.Position);
     Check(G.Navigation.ClearSegment(o.Position-normal*.8f,o.Position+normal*.8f),"2730 marked opening capsule crossing clear "+o.Position);
    }
    // Isolate the opening effect from the already explored public component.
    foreach(var point in new[]{new Vector3(-2,0,-14),new Vector3(-5,0,14)}){
     G.Vision.Clear();foreach(var area in new[]{new Rect(-2.25f,-24.25f,4.5f,48.5f),new Rect(-20.25f,-6.25f,40.5f,3.5f),new Rect(-20.25f,10.75f,40.5f,3.5f)})G.Vision.RevealArea(area);
     var o=indoor.OfficeOpenings2726.Find(p=>Vector3.Distance(p.Position,point)<.01f);var normal=OfficeNormal2727(o);var stand=point+normal*(o.Orientation=="v"?.95f:-.95f);FreightRoomPlace2717(stand,"2730 connected group door approach");FreightMonsterF2720(o.Door,false);
     foreach(string id in Mathf.Abs(point.x+2)<.01f?new[]{"01","02"}:new[]{"08","09"})OfficeAreaSeen2730(indoor.OfficeRooms2726.Find(r=>r.Id==id).Area,"one F reveals entire hole-connected room "+id);
    }
    foreach(var area in indoor.OfficeInitialAreas2730)G.Vision.RevealArea(area);
    // Remaining doors: same genuine iterative F opening, then visit every room.
    foreach(var o in indoor.OfficeOpenings2726)if(o.Door!=null&&o.Door.Open){G.Player.transform.position=o.Position+OfficeNormal2727(o)*.95f;Physics.SyncTransforms();G.ToggleRoomDoor(o.Door);}
    yield return OfficeDoors2727();
    foreach(var room in indoor.OfficeRooms2726){OfficeAreaSeen2730(room.Area,"opened room "+room.Name);FreightRoomWalk2717(room.Center,"2730 all rooms reachable "+room.Name);yield return null;}
    foreach(var o in indoor.OfficeOpenings2726)if(o.Door==null){var normal=OfficeNormal2727(o);FreightRoomWalk2717(o.Position-normal*.8f,"2730 doorless approach");FreightRoomWalk2717(o.Position+normal*.8f,"2730 actual doorless crossing");}
    foreach(float side in new[]{-4f,5f})foreach(var p in new[]{new Vector3(0,0,-14),new Vector3(side,0,-14),new Vector3(side,0,-8),new Vector3(0,0,-8)})FreightRoomWalk2717(p,"2730 bypass retained");
    FreightRoomWalk2717(new Vector3(0,0,4),"2730 physical visibility probe");G.Player.Aim=Vector3.right;G.Vision.Refresh(G.Player.transform.position,G.Player.Aim);
    Check(!G.Vision.CanSee(new Vector3(10,0,4)),"2730 explored room does not grant sight through solid walls");
    foreach(var area in new[]{new Rect(-2,-24,4,48),new Rect(-20,-6,40,3),new Rect(-20,11,40,3)})OfficeAreaSeen2730(area,"corridor memory persists after turning/moving");
    G.Rig.Tick(0);G.Player.Draw(G.View,0);yield return null;var frame=HvRender277(G.View,"2730-office-corridor-live-fog.png",1600,900);Destroy(frame);
    yield return CargoPicture2715("2730-office-twelve-doors-overview.png",Vector3.zero,35,29,false,1800,1200);
    var vision=G.Vision;FreightRoomWalk2717(indoor.Spawn,"2730 exit");FreightMonsterF2720(indoor.OfficeExit2726,true);yield return null;FreightMonsterF2720(portal,true);yield return null;
    Check(G.Map==indoor&&G.Vision==vision&&G.Vision.RoomGate,"2730 same explored map/vision restored from cache");OfficeAreaSeen2730(new Rect(-2,-24,4,48),"cached public corridor");
    foreach(var room in indoor.OfficeRooms2726)Check(G.Vision.Explored(room.Center),"2730 revealed room memory survives portal "+room.Id);
    for(int i=0;i<formal.Length;i++)Check(MainBytes273(original[i],File.Exists(Path.Combine(dir,formal[i]))?File.ReadAllBytes(Path.Combine(dir,formal[i])):null),"2730 formal save unchanged "+formal[i]);
   }finally{G.Controls=controls;UnityEngine.Random.state=random;G.Paused=true;}
  }
 }
}
