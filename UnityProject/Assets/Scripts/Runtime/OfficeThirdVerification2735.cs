using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace GreyHarbor {public sealed partial class RaidVerification {
 IEnumerator RunOfficeThird2735(){
  var controls=G.Controls;string dir=Path.GetDirectoryName(G.SavePath);string[] names={"stash-v40.json","stash-v41.json","stash-v42.json"};var original=Array.ConvertAll(names,n=>File.Exists(Path.Combine(dir,n))?File.ReadAllBytes(Path.Combine(dir,n)):null);
  try{
   Check(Application.version=="2.7.35"&&G.SavePath.EndsWith("verification-stash-2.7.35.json"),"2735 isolated third-floor verification");
   FreshCamp();G.Controls=new TouchInput();G.HarborWalkthrough=true;yield return G.BeginTrainingLoad2721();G.Paused=true;var outdoor=G.Map;
   OfficePreview2726.Apply(G);FreightRoomWalk2717(RaidMap.OfficeArrival2726,"2735 office entry");FreightMonsterF2720(outdoor.Doors.Find(d=>d.Portal&&d.Destination=="officeright1"),true);yield return null;
   var first=G.Map;yield return OfficeDoors2727();FreightRoomWalk2717(RaidMap.OfficeStairArrival2733(false),"2735 south stairs first floor");FreightMonsterF2720(first.Doors.Find(d=>d.Destination=="office-floor:2:south"),true);yield return null;
   var second=G.Map;Check(second.Doors.FindAll(d=>d.Portal).Count==4,"2735 second-floor two up and two down portals");
   foreach(bool north in new[]{false,true})foreach(int target in new[]{1,3}){var d=second.Doors.Find(p=>p.Destination=="office-floor:"+target+":"+(north?"north":"south"));Check(d!=null&&CargoClear2713(d.Position),"2735 second-floor stair point clear "+target+" "+north);}
   FreightMonsterF2720(second.Doors.Find(d=>d.Destination=="office-floor:3:south"),true);yield return null;var third=G.Map;
   Check(third.OfficeFloor2733==3&&third.InteriorId=="officeright3"&&third.Bounds==second.Bounds,"2735 separate 40x48 third floor");
   Check(CargoClear2713(G.Player.transform.position)&&Vector3.Distance(G.Player.transform.position,RaidMap.OfficeStairArrival2733(false))<.001f,"2735 safe south third-floor arrival");
   Check(third.OfficeRooms2726.Count==12&&third.OfficeOpenings2726.FindAll(o=>o.Door!=null).Count==12&&third.OfficeOpenings2726.FindAll(o=>o.Type=="hole").Count==3,"2735 ten rooms/two stairs, twelve doors/three holes");
   Check(third.Doors.FindAll(d=>d.Portal).Count==2&&third.OfficeExit2726==null&&!third.Doors.Exists(d=>d.Destination.Contains("left")||d.Destination.Contains("4:")),"2735 no bridge/fourth-floor/exterior portal");
   Check(G.Enemies.Count==0&&third.Loot.Count==0,"2735 whitebox without new monsters or containers");
   foreach(var area in new[]{new Rect(-2,-24,4,48),new Rect(-20,-6,40,3),new Rect(-20,11,40,3),new Rect(14,14,6,2)})OfficeAreaSeen2730(area,"2735 full public corridor including north vestibule");
   foreach(float h in new[]{.2f,1f,2f,2.8f})Check(Physics.Linecast(new Vector3(-20.5f,h,12.5f),new Vector3(-19.5f,h,12.5f),SceneObstacle.MovementMask,QueryTriggerInteraction.Ignore),"2735 third-floor west wall sealed "+h);
   Check(Physics.Linecast(new Vector3(0,1,1),new Vector3(0,1,5),SceneObstacle.MovementMask,QueryTriggerInteraction.Ignore),"2735 main corridor blocker prevents straight crossing");
   yield return OfficeDoors2727();
   foreach(var o in third.OfficeOpenings2726)if(o.Door!=null){var adjacent=third.OfficeRooms2726.FindAll(r=>o.X>=r.Area.xMin-.01f&&o.X<=r.Area.xMax+.01f&&o.Z>=r.Area.yMin-.01f&&o.Z<=r.Area.yMax+.01f);var room=adjacent.Find(r=>!r.Stair)??adjacent[0];var b=o.Door.Collider.bounds;Check(room.Area.Contains(new Vector2(b.center.x,b.center.z)),"2735 door inward "+room.Name);}
   foreach(var r in third.OfficeRooms2726){FreightRoomWalk2717(r.Center,"2735 visit "+r.Name);OfficeAreaSeen2730(r.Area,"2735 room group reveal "+r.Id);yield return null;}
   foreach(var o in third.OfficeOpenings2726)if(o.Type=="hole"){var n=OfficeNormal2727(o);FreightRoomWalk2717(o.Position-n*.8f,"2735 wall-hole approach");FreightRoomWalk2717(o.Position+n*.8f,"2735 wall-hole crossing");}
   foreach(var route in new[]{new[]{new Vector3(0,0,0),new Vector3(4,0,0),new Vector3(4,0,8),new Vector3(0,0,8)},new[]{new Vector3(0,0,-4.5f),new Vector3(-14,0,-4.5f),new Vector3(-14,0,12.5f),new Vector3(0,0,12.5f)}})foreach(var point in route)FreightRoomWalk2717(point,"2735 actual short/long room bypass");
   int covers=0;foreach(var t in third.Root.GetComponentsInChildren<Transform>())if(t.name=="OfficeFloorHoleReservedCover2733")covers++;Check(covers==2,"2735 two sealed floor-hole reservations");
   foreach(var p in new[]{new Vector3(8,0,-12),new Vector3(-15,0,7)}){FreightRoomWalk2717(p,"2735 solid reserved floor-panel crossing");Check(CargoClear2713(p),"2735 reserved-panel point capsule clear");}
   yield return CargoPicture2715("2735-office-third-floor-whitebox.png",Vector3.zero,35,29,false,1800,1200);yield return CargoPicture2715("2735-office-third-floor-overhead.png",Vector3.zero,0,27,true,1800,1800);
   var remembered=new Dictionary<WorldDoor,bool>();var close=third.OfficeOpenings2726.Find(o=>o.Door!=null&&o.X==2&&o.Z==-14);FreightRoomWalk2717(close.Position-Vector3.right*.95f,"2735 mixed door state before caching");FreightMonsterF2720(close.Door,false);Check(!close.Door.Open,"2735 closed room door cached");foreach(var d in third.Doors)remembered.Add(d,d.Open);
   FreightRoomWalk2717(RaidMap.OfficeStairArrival2733(true),"2735 north third-floor stairs");FreightMonsterF2720(third.Doors.Find(d=>d.Destination=="office-floor:2:north"),true);yield return null;Check(G.Map==second&&CargoClear2713(G.Player.transform.position),"2735 north down restores second floor");
   FreightMonsterF2720(second.Doors.Find(d=>d.Destination=="office-floor:3:north"),true);yield return null;Check(G.Map==third,"2735 north up restores third floor");foreach(var d in remembered)Check(d.Key.Open==d.Value,"2735 cached third-floor door "+d.Key.Name);
   foreach(var r in third.OfficeRooms2726)Check(G.Vision.Explored(r.Center),"2735 cached exploration "+r.Id);
   FreightRoomWalk2717(RaidMap.OfficeStairArrival2733(false),"2735 third-floor south down");FreightMonsterF2720(third.Doors.Find(d=>d.Destination=="office-floor:2:south"),true);yield return null;Check(G.Map==second,"2735 all four second/third-floor stair directions");
   var down=second.Doors.Find(d=>d.Destination=="office-floor:1:south");FreightRoomWalk2717(down.Position,"2735 distinct second-floor down marker");FreightMonsterF2720(down,true);yield return null;Check(G.Map==first,"2735 first/second-floor south down remains working");
   FreightRoomWalk2717(RaidMap.OfficeStairArrival2733(true),"2735 first-floor north up");FreightMonsterF2720(first.Doors.Find(d=>d.Destination=="office-floor:2:north"),true);yield return null;yield return OfficeDoors2727();
   down=second.Doors.Find(d=>d.Destination=="office-floor:1:north");FreightRoomWalk2717(down.Position,"2735 distinct north down marker");FreightMonsterF2720(down,true);yield return null;Check(G.Map==first,"2735 first/second-floor north down remains working");
   FreightRoomWalk2717(first.Spawn,"2735 return to exterior");FreightMonsterF2720(first.OfficeExit2726,true);yield return null;Check(G.Map==outdoor,"2735 exterior return preserved");
   for(int i=0;i<names.Length;i++)Check(MainBytes273(original[i],File.Exists(Path.Combine(dir,names[i]))?File.ReadAllBytes(Path.Combine(dir,names[i])):null),"2735 formal save unchanged "+names[i]);
  }finally{G.Controls=controls;G.Paused=true;}
 }
}}
