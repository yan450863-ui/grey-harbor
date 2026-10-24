using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace GreyHarbor {public sealed partial class RaidVerification {
 IEnumerator RunOfficeUpper2733(){
  var controls=G.Controls;var random=UnityEngine.Random.state;string dir=Path.GetDirectoryName(G.SavePath);string[] formal={"stash-v40.json","stash-v41.json","stash-v42.json"};byte[][] original=Array.ConvertAll(formal,n=>File.Exists(Path.Combine(dir,n))?File.ReadAllBytes(Path.Combine(dir,n)):null);
  try{
   Check(Application.version=="2.7.33"&&G.SavePath.EndsWith("verification-stash-2.7.33.json"),"2733 isolated second-floor whitebox verification");
   FreshCamp();G.Controls=new TouchInput();G.HarborWalkthrough=true;yield return G.BeginTrainingLoad2721();G.Paused=true;var outdoor=G.Map;var actors=G.Enemies.ToArray();int loot=outdoor.Loot.Count;
   OfficePreview2726.Apply(G);FreightRoomWalk2717(RaidMap.OfficeArrival2726,"2733 actual office entry");var entry=outdoor.Doors.Find(d=>d.Portal&&d.Destination=="officeright1");FreightMonsterF2720(entry,true);yield return null;
   var first=G.Map;Check(first.OfficeFloor2733==1&&first.OfficeOpenings2726.Count==14,"2733 floor one and five sealed openings retained");yield return OfficeDoors2727();
   var south=first.Doors.Find(d=>d.Destination=="office-floor:2:south");FreightRoomWalk2717(south.Position,"2733 actual first-floor south stairs");FreightMonsterF2720(south,true);yield return null;
   var second=G.Map;Check(second!=first&&second.InteriorId=="officeright2"&&second.OfficeFloor2733==2&&G.Enemies.Count==0&&second.Loot.Count==0,"2733 genuine separate second floor with no enemies/containers");
   Check(CargoClear2713(G.Player.transform.position)&&Vector3.Distance(G.Player.transform.position,RaidMap.OfficeStairArrival2733(false))<.001f,"2733 south stair arrival is aligned and capsule clear");
   Check(second.Bounds==first.Bounds&&second.OfficeRooms2726.Count==12&&second.OfficeRooms2726.FindAll(r=>r.Stair).Count==2,"2733 40x48 layout with ten rooms plus two aligned stair rooms");
   Check(second.OfficeOpenings2726.FindAll(o=>o.Door!=null).Count==11&&second.OfficeOpenings2726.FindAll(o=>o.Type=="hole").Count==2,"2733 V2 eleven doors and two wall holes");
   Check(second.OfficeOpenings2726.Find(o=>Mathf.Abs(o.X+2)<.01f&&Mathf.Abs(o.Z+14)<.01f)==null&&second.OfficeOpenings2726.Find(o=>Mathf.Abs(o.X-2)<.01f&&Mathf.Abs(o.Z-1)<.01f)==null,"2733 finance and project offices each keep only north door");
   Check(second.OfficeBlockers2726.Count==2&&Mathf.Abs(second.OfficeBlockers2726[0].width-1.6f)<.01f,"2733 central obstruction leaves V2 2.4m passage");
   Check(second.OfficeExit2726==null&&second.Doors.FindAll(d=>d.Portal).Count==2&&!second.Doors.Exists(d=>d.Destination.Contains("left")||d.Destination.Contains("3:")),"2733 only two downward stairs active; no bridge/third-floor exit");
   foreach(float h in new[]{.2f,1f,2f,2.8f})Check(Physics.Linecast(new Vector3(-20.5f,h,12.5f),new Vector3(-19.5f,h,12.5f),SceneObstacle.MovementMask,QueryTriggerInteraction.Ignore),"2733 bridge entrance fully sealed at height "+h);
   int covers=0;foreach(var t in second.Root.GetComponentsInChildren<Transform>())if(t.name=="OfficeFloorHoleReservedCover2733")covers++;Check(covers==2,"2733 two floor-hole locations are closed reservation panels rather than unsafe drops");
   foreach(var area in new[]{new Rect(-2,-24,4,48),new Rect(-20,-6,40,3),new Rect(-20,11,40,3)})OfficeAreaSeen2730(area,"2733 second-floor public corridor");
   yield return OfficeDoors2727();
   foreach(var o in second.OfficeOpenings2726)if(o.Door!=null){var adjacent=second.OfficeRooms2726.FindAll(r=>o.X>=r.Area.xMin-.01f&&o.X<=r.Area.xMax+.01f&&o.Z>=r.Area.yMin-.01f&&o.Z<=r.Area.yMax+.01f);var r=adjacent.Find(p=>!p.Stair)??adjacent[0];var b=o.Door.Collider.bounds;Check(r.Area.Contains(new Vector2(b.center.x,b.center.z)),"2733 door opens into target room "+r.Name);}
   foreach(var room in second.OfficeRooms2726){FreightRoomWalk2717(room.Center,"2733 actual visit "+room.Name);OfficeAreaSeen2730(room.Area,"2733 revealed room "+room.Name);yield return null;}
   foreach(var o in second.OfficeOpenings2726)if(o.Type=="hole"){var n=OfficeNormal2727(o);FreightRoomWalk2717(o.Position-n*.8f,"2733 wall hole approach");FreightRoomWalk2717(o.Position+n*.8f,"2733 wall hole crossing");}
   foreach(var p in new[]{new Vector3(.8f,0,3),new Vector3(.8f,0,7),new Vector3(-13,0,-4.5f),new Vector3(-13,0,12.5f),new Vector3(-18.7f,0,12.5f)})FreightRoomWalk2717(p,"2733 central passage and conference detour to sealed bridge");
   yield return CargoPicture2715("2733-office-second-floor-whitebox.png",Vector3.zero,35,29,false,1800,1200);
   yield return CargoPicture2715("2733-office-second-floor-overhead.png",Vector3.zero,0,27,true,1800,1800);
   var remembered=new Dictionary<WorldDoor,bool>();var close=second.OfficeOpenings2726.Find(o=>o.Door!=null&&o.X==2&&o.Z==-14);FreightRoomWalk2717(close.Position-Vector3.right*.95f,"2733 close second-floor office before caching");FreightMonsterF2720(close.Door,false);Check(!close.Door.Open,"2733 second floor has a closed room door for cache test");foreach(var d in second.Doors)remembered.Add(d,d.Open);
   var downNorth=second.Doors.Find(d=>d.Destination=="office-floor:1:north");FreightRoomWalk2717(downNorth.Position,"2733 actual north down stairs");FreightMonsterF2720(downNorth,true);yield return null;
   Check(G.Map==first&&CargoClear2713(G.Player.transform.position)&&Vector3.Distance(G.Player.transform.position,RaidMap.OfficeStairArrival2733(true))<.001f,"2733 north down stairs restore actual first-floor aligned safe arrival");
   var upNorth=first.Doors.Find(d=>d.Destination=="office-floor:2:north");FreightMonsterF2720(upNorth,true);yield return null;Check(G.Map==second,"2733 north up stairs restore same second-floor object");foreach(var d in remembered)Check(d.Key.Open==d.Value,"2733 mixed second-floor door state preserved "+d.Key.Name);
   foreach(var room in second.OfficeRooms2726)Check(G.Vision.Explored(room.Center),"2733 second-floor exploration survives switching "+room.Id);
   var downSouth=second.Doors.Find(d=>d.Destination=="office-floor:1:south");FreightRoomWalk2717(downSouth.Position,"2733 actual south down stairs");FreightMonsterF2720(downSouth,true);yield return null;Check(G.Map==first,"2733 all four stair directions work");
   FreightRoomWalk2717(first.Spawn,"2733 return to exterior exit");FreightMonsterF2720(first.OfficeExit2726,true);yield return null;Check(G.Map==outdoor&&G.Enemies.Count==actors.Length&&outdoor.Loot.Count==loot&&Array.TrueForAll(actors,e=>G.Enemies.Contains(e)),"2733 outdoor original actors/containers restored without duplication");
   for(int i=0;i<formal.Length;i++)Check(MainBytes273(original[i],File.Exists(Path.Combine(dir,formal[i]))?File.ReadAllBytes(Path.Combine(dir,formal[i])):null),"2733 formal save unchanged "+formal[i]);
  }finally{G.Controls=controls;UnityEngine.Random.state=random;G.Paused=true;}
 }
}}
