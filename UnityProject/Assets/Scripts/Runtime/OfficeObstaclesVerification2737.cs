using UnityEngine;
using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
namespace GreyHarbor {public sealed partial class RaidVerification {
 IEnumerator RunOfficeObstacles2737(){
  var input=G.Controls;string dir=Path.GetDirectoryName(G.SavePath);string[] names={"stash-v40.json","stash-v41.json","stash-v42.json"};var before=Array.ConvertAll(names,n=>File.Exists(Path.Combine(dir,n))?File.ReadAllBytes(Path.Combine(dir,n)):null);
  try{
   Check(Application.version=="2.7.37"&&G.SavePath.EndsWith("verification-stash-2.7.37.json"),"2737 isolated safe and broken-wall verification");FreshCamp();G.Controls=new TouchInput();G.HarborWalkthrough=true;yield return G.BeginTrainingLoad2721();G.Paused=true;var outside=G.Map;
   OfficePreview2726.Apply(G);FreightRoomWalk2717(RaidMap.OfficeArrival2726,"2737 enter office");FreightMonsterF2720(outside.Doors.Find(d=>d.Portal&&d.Destination=="officeright1"),true);yield return null;var first=G.Map;
   Check(first.Loot.Count==25&&first.OfficeSearchRecords2736.Count==25,"2737 twenty-four previous boxes plus one small safe");
   Check(first.OfficeSmallSafeBox2737!=null&&first.OfficeSmallSafeBox2737.DropProfile230.Tier==ContainerTier.Safe&&first.OfficeSmallSafeBox2737.DropProfile230.SafeSize==SafeSize240.Small,"2737 actual small-safe independent drop profile");
   Check(first.OfficeBrokenWallModels2737.Count==2&&first.OfficeOpenings2726.Count==14&&first.OfficeBlockers2726.Count==1,"2737 two broken walls preserve original topology");
   int oldBlocks=0;foreach(var t in first.Root.GetComponentsInChildren<Transform>())if(t.name=="OfficeCorridorBlock2726")oldBlocks++;Check(oldBlocks==0&&first.OfficeCorridorFurniture2737&&first.OfficeCorridorFurniture2737.GetComponentsInChildren<Collider>().Length>=2,"2737 original whitebox obstruction replaced by actual furniture colliders");
   foreach(float x in new[]{-1.4f,-.7f,0,.7f,1.4f})Check(!G.Navigation.ClearSegment(new Vector3(x,0,-12.2f),new Vector3(x,0,-9.8f)),"2737 collapsed furniture blocks direct walking at x="+x);
   yield return OfficeDoors2727();
   foreach(var p in new[]{new Vector3(0,0,-14),new Vector3(-4,0,-14),new Vector3(-4,0,-8),new Vector3(0,0,-8)})FreightRoomWalk2717(p,"2737 left room bypass around real furniture");
   foreach(var hole in first.OfficeOpenings2726)if(hole.Type=="hole"){
    var normal=OfficeNormal2727(hole);var along=hole.Orientation=="v"?Vector3.forward:Vector3.right;
    foreach(float offset in new[]{-.5f,0,.5f}){FreightRoomWalk2717(hole.Position+along*offset-normal*.85f,"2737 broken wall approach");FreightRoomWalk2717(hole.Position+along*offset+normal*.85f,"2737 broken wall crossing");}
    foreach(var room in first.OfficeRooms2726.FindAll(r=>hole.X>=r.Area.xMin-.01f&&hole.X<=r.Area.xMax+.01f&&hole.Z>=r.Area.yMin-.01f&&hole.Z<=r.Area.yMax+.01f))OfficeAreaSeen2730(room.Area,"2737 broken wall shared exploration "+room.Name);
   }
   foreach(var r in first.OfficeSearchRecords2736)Check(CargoClear2713(r.Stand)&&CargoClear2713(r.AccessPoint),"2737 existing and new box approaches remain clear "+r.Container.Name);
   var safe=first.OfficeSearchRecords2736.Find(r=>r.Container==first.OfficeSmallSafeBox2737);FreightRoomWalk2717(safe.AccessPoint,"2737 safe approach");FreightRoomWalk2717(safe.Stand,"2737 safe search position");Check(G.CanInteractLoot(safe.Container),"2737 safe visible and interactable");var content=FreightMonsterItems2720(safe.Container);SprintInteract2711(false,0,safe.Model.transform.position);SprintInteract2711(false,0,safe.Model.transform.position);Check(G.Nearby==safe.Container,"2737 real F selects new small safe");SprintInteract2711(true,safe.Container.SearchDuration+.02f,safe.Model.transform.position);Check(safe.Container.Opened&&G.OpenLoot==safe.Container&&content==FreightMonsterItems2720(safe.Container),"2737 real held F searches small safe without reroll");G.Paused=false;for(int i=0;i<8;i++)G.TickIdentification(3);G.Paused=true;Check(safe.Container.Items.TrueForAll(it=>it.Identified),"2737 safe items identified");content=FreightMonsterItems2720(safe.Container);G.CloseInventory();SprintInteract2711(false,0,safe.Model.transform.position);
   yield return UnloadingSearchRecords2724(first.OfficeSearchRecords2736.FindAll(r=>r.RoomName=="员工休息室"&&r.Container!=safe.Container),2,25);
   yield return CargoPicture2715("2737-staff-room-small-safe.png",new Vector3(8,0,19),35,7.5f,false,1400,1000);yield return CargoPicture2715("2737-broken-wall-security-reception.png",new Vector3(-8,0,-16),35,5,false,1400,1000);yield return CargoPicture2715("2737-broken-wall-archives.png",new Vector3(-8,0,19),35,5,false,1400,1000);yield return CargoPicture2715("2737-collapsed-corridor-furniture.png",new Vector3(0,0,-11),35,5,false,1400,1000);
   var up=first.Doors.Find(d=>d.Destination=="office-floor:2:north");FreightRoomWalk2717(up.Position,"2737 north stairs cache transition");FreightMonsterF2720(up,true);yield return null;var second=G.Map;Check(second.OfficeFloor2733==2&&second.Loot.Count==0&&second.OfficeBrokenWallModels2737.Count==0,"2737 upper whitebox unchanged");var down=second.Doors.Find(d=>d.Destination=="office-floor:1:north");FreightRoomWalk2717(down.Position,"2737 north return");FreightMonsterF2720(down,true);yield return null;Check(G.Map==first&&first.OfficeSmallSafeBox2737==safe.Container&&safe.Container.Opened&&safe.Container.Prop.Searched&&content==FreightMonsterItems2720(safe.Container)&&first.Loot.Count==25,"2737 safe and contents survive cached floor return");
   foreach(var p in new[]{new Vector3(0,0,-8),new Vector3(-4,0,-8),new Vector3(-4,0,-14),new Vector3(0,0,-14)})FreightRoomWalk2717(p,"2737 reverse furniture bypass");FreightRoomWalk2717(first.Spawn,"2737 exit");FreightMonsterF2720(first.OfficeExit2726,true);yield return null;Check(G.Map==outside,"2737 original exterior restored");
   for(int i=0;i<names.Length;i++)Check(MainBytes273(before[i],File.Exists(Path.Combine(dir,names[i]))?File.ReadAllBytes(Path.Combine(dir,names[i])):null),"2737 formal save unchanged "+names[i]);
  }finally{G.CloseInventory();G.Controls=input;G.Paused=true;}
 }
}}
