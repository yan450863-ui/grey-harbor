using System;
using System.IO;
using System.Collections;
using UnityEngine;
namespace GreyHarbor {public sealed partial class RaidVerification {
 IEnumerator RunOfficeLanding2734(){
  var controls=G.Controls;string dir=Path.GetDirectoryName(G.SavePath);string[] names={"stash-v40.json","stash-v41.json","stash-v42.json"};var original=Array.ConvertAll(names,n=>File.Exists(Path.Combine(dir,n))?File.ReadAllBytes(Path.Combine(dir,n)):null);
  try{
   Check(Application.version=="2.7.34"&&G.SavePath.EndsWith("verification-stash-2.7.34.json"),"2734 isolated landing verification");
   FreshCamp();G.Controls=new TouchInput();G.HarborWalkthrough=true;yield return G.BeginTrainingLoad2721();G.Paused=true;var outdoor=G.Map;
   OfficePreview2726.Apply(G);FreightRoomWalk2717(RaidMap.OfficeArrival2726,"2734 office entry");FreightMonsterF2720(outdoor.Doors.Find(d=>d.Portal&&d.Destination=="officeright1"),true);yield return null;
   var first=G.Map;var landing=new Rect(14,14,6,2);OfficeAreaSeen2730(landing,"2734 first-floor north landing before opening doors");
   Check(!G.Vision.Explored(new Vector3(17,0,19)),"2734 closed first-floor stair room remains gated");
   yield return OfficeDoors2727();FreightRoomWalk2717(new Vector3(17,0,15),"2734 first-floor landing real movement");
   FreightRoomWalk2717(RaidMap.OfficeStairArrival2733(true),"2734 first-floor north stairs");FreightMonsterF2720(first.Doors.Find(d=>d.Destination=="office-floor:2:north"),true);yield return null;
   var second=G.Map;OfficeAreaSeen2730(landing,"2734 second-floor landing before opening doors");
   Check(!G.Vision.Explored(new Vector3(8,0,20)),"2734 closed supervisor office still gated");
   var door=second.OfficeOpenings2726.Find(o=>o.Door!=null&&o.X==17&&o.Z==16).Door;
   FreightRoomWalk2717(new Vector3(17,0,17.1f),"2734 second-floor stair door inside approach");FreightMonsterF2720(door,false);Check(door.Open,"2734 north door opens from stair side");
   FreightRoomWalk2717(new Vector3(17,0,15),"2734 formerly black landing crossing");OfficeAreaSeen2730(landing,"2734 open door landing surface");
   G.Rig.Tick(0);G.Player.Draw(G.View,0);yield return null;var frame=HvRender277(G.View,"2734-north-landing-live-fog.png",1600,900);Destroy(frame);
   FreightMonsterF2720(door,false);Check(!door.Open,"2734 door closes from landing");OfficeAreaSeen2730(landing,"2734 closed door landing remains explored");
   FreightMonsterF2720(door,false);Check(door.Open,"2734 door reopens from landing");FreightRoomWalk2717(RaidMap.OfficeStairArrival2733(true),"2734 return through north door");
   FreightMonsterF2720(second.Doors.Find(d=>d.Destination=="office-floor:1:north"),true);yield return null;Check(G.Map==first,"2734 down stairs restores first floor");OfficeAreaSeen2730(landing,"2734 first-floor cached landing");
   FreightMonsterF2720(first.Doors.Find(d=>d.Destination=="office-floor:2:north"),true);yield return null;Check(G.Map==second&&door.Open,"2734 up stairs restores second-floor door cache");OfficeAreaSeen2730(landing,"2734 second-floor cached landing");
   for(int i=0;i<names.Length;i++)Check(MainBytes273(original[i],File.Exists(Path.Combine(dir,names[i]))?File.ReadAllBytes(Path.Combine(dir,names[i])):null),"2734 formal save unchanged "+names[i]);
  }finally{G.Controls=controls;G.Paused=true;}
 }
}}
