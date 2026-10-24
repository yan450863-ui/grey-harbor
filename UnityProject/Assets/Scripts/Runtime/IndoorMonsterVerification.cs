using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace GreyHarbor { public sealed partial class RaidVerification {
 IEnumerator VerifyIndoorMonsters119(){
  FreshCamp();G.NextMap=RaidDestination.Training;G.StartRaid();G.Paused=true;yield return null;
  Check(G.Enemies.Count==4,"User revision: outdoor training stays at four separate patrols");
  var outside=G.Enemies.ToArray();var outdoor=outside[0];outdoor.Health=81;var outsidePos=outdoor.transform.position;
  string[] ids={"booth","repair","mainhouse"};int[] expected={1,3,10};var signatures=new Dictionary<string,string>();
  for(int mapIndex=0;mapIndex<ids.Length;mapIndex++){
   string id=ids[mapIndex];G.Player.transform.position=id=="booth"?RaidMap.BoothApproach:id=="repair"?RaidMap.RepairApproach:RaidMap.MainHouseApproach;
   G.SwitchInterior(id);G.Paused=true;yield return null;
   var actors=G.Enemies.ToArray();Check(actors.Length==expected[mapIndex],"Area-based fixed indoor density: "+id+" = "+expected[mapIndex]);
   var signature="";bool valid=true,patrol=true,clearDoor=true,entry=true;
   foreach(var e in actors){signature+=e.name+e.Home.ToString("F3")+e.Monster+";";
    valid&=e.transform.IsChildOf(G.Map.Root)&&e.IsMonster&&!Physics.CheckCapsule(e.Home+Vector3.up*.35f,e.Home+Vector3.up,.34f,SceneObstacle.MovementMask);
    entry&=Vector3.Distance(e.Home,G.Map.Spawn)>2.5f;
    foreach(var p in e.PatrolTargets){patrol&=G.Navigation.FindPath(e.Home,p).Count>0;valid&=!Physics.CheckCapsule(p+Vector3.up*.35f,p+Vector3.up,.34f,SceneObstacle.MovementMask);}
   }signatures[id]=signature;
   Check(valid,"Fixed spawn and all patrol waypoints avoid walls/furniture: "+id);Check(patrol,"Every patrol leg is navigable with room doors shut: "+id);Check(entry,"Arrival spot has no overlapping or point-blank spawn: "+id);
   foreach(var door in G.Map.Doors)if(!door.Portal)clearDoor&=!G.MonsterBlocksDoor(door);Check(clearDoor,"Initial spawns leave room door swing areas clear: "+id);
   foreach(var e in actors){var original=e.Home;e.WaitTime=0;e.PatrolIndex=1;e.Route.Clear();float traveled=0;bool collisionFree=true;
    for(int i=0;i<120;i++){var before=e.transform.position;G.HarborPatrol(e,.05f);Physics.SyncTransforms();traveled+=Vector3.Distance(before,e.transform.position);collisionFree&=!Physics.CheckCapsule(e.transform.position+Vector3.up*.35f,e.transform.position+Vector3.up,.29f,SceneObstacle.MovementMask);}
    Check(traveled>1&&collisionFree,"Indoor patrol actually advances without clipping: "+e.name);
    e.transform.position=original;e.Route.Clear();e.Alert=e.Memory=0;e.WaitTime=2;
   }Physics.SyncTransforms();
   // Closed-room enemies remain unseen, and cannot hurt the player through doors.
   G.Player.transform.position=G.Map.Spawn;G.Player.Health=100;G.Vision.Refresh(G.Player.transform.position,Vector3.forward);
   bool hidden=true;foreach(var e in actors)if(!G.Vision.RoomAllowed(e.Home))hidden&=!G.Vision.CanSee(e.Home);
   Check(hidden,"Unopened rooms do not reveal their enemies: "+id);
   var doorTest=G.Map.Doors.Find(d=>!d.Portal);var blocker=actors[0];var saved=blocker.transform.position;
   G.Player.transform.position=G.Map.Spawn;blocker.transform.position=new Vector3(doorTest.OpenSafety.center.x,0,doorTest.OpenSafety.center.y);Physics.SyncTransforms();
   Check(!G.ToggleRoomDoor(doorTest)&&!doorTest.Open,"Opening refuses to sweep a door through a monster: "+id);blocker.transform.position=saved;Physics.SyncTransforms();
   Check(G.ToggleRoomDoor(doorTest)&&doorTest.Open,"Door opens once monster clears swing: "+id);
   blocker.transform.position=doorTest.Position;Physics.SyncTransforms();Check(!G.ToggleRoomDoor(doorTest)&&doorTest.Open,"Closing refuses to trap a monster in doorway: "+id);blocker.transform.position=saved;Physics.SyncTransforms();
   foreach(var e in actors){e.Route.Add(G.Map.Spawn);e.RepathTime=9;}
   Check(G.ToggleRoomDoor(doorTest)&&!doorTest.Open&&G.Enemies.TrueForAll(e=>e.Route.Count==0&&e.RepathTime==0),"Door changes invalidate cached monster routes: "+id);
   // Persist death, corpse items, surviving enemy HP and positions independently for each room map.
   int lootBefore=G.Map.Loot.Count;var victim=actors[0];victim.Damage(999);G.MonsterDeath(victim);var corpse=G.Map.Loot[G.Map.Loot.Count-1];
   corpse.Opened=true;corpse.Items[0].Identified=true;int remaining=corpse.Items[0].Count;
   RaidActor survivor=actors.Length>1?actors[1]:null;if(survivor)survivor.Health=31;
   var map=G.Map;G.SwitchInterior(id);Check(G.Enemies.Count==4&&G.Enemies.Contains(outdoor)&&outdoor.Health==81&&outdoor.transform.position==outsidePos,"Outdoor patrol and HP are restored after leaving "+id);
   Check(!victim.gameObject.activeInHierarchy&&G.Enemies.TrueForAll(e=>e.transform.IsChildOf(G.Map.Root)),"Only current-map monsters remain active: "+id);
   G.SwitchInterior(id);Check(G.Map==map&&G.Enemies.Count==expected[mapIndex]&&G.Enemies.Contains(victim)&&victim.Dead&&(!survivor||survivor.Health==31),"Reentry retains enemy identities, death and remaining HP: "+id);
   Check(G.Map.Loot.Count==lootBefore+1&&G.Map.Loot.Contains(corpse)&&corpse.Opened&&corpse.Items[0].Identified&&corpse.Items[0].Count==remaining,"Reentry does not duplicate/reset corpse or searched loot: "+id);
   G.SwitchInterior(id);
  }
  G.Finish(false);Check(G.Phase==GamePhase.Base&&G.Enemies.Count==0,"Ending training cleans all new indoor actors");
  G.NextMap=RaidDestination.Training;G.StartRaid();G.Paused=true;yield return null;
  for(int n=0;n<ids.Length;n++){
   G.SwitchInterior(ids[n]);string signature="";foreach(var e in G.Enemies)signature+=e.name+e.Home.ToString("F3")+e.Monster+";";
   Check(signature==signatures[ids[n]]&&G.Enemies.TrueForAll(e=>!e.Dead&&e.Health==MonsterTuning.Health(e.Monster)),"New raid repeats fixed placement and resets health: "+ids[n]);
   // Production camera previews; expose rooms by actually opening doors.
   if(ids[n]=="booth"){G.Player.transform.position=new Vector3(0,0,.3f);G.ToggleRoomDoor(G.Map.Doors.Find(d=>!d.Portal));G.Player.Aim=Vector3.right;}
   if(ids[n]=="repair"){G.Player.transform.position=new Vector3(1,0,-5);G.Player.Aim=Vector3.forward;}
   if(ids[n]=="mainhouse"){G.Player.transform.position=RaidMap.MainHouseDoorApproach(0);G.ToggleRoomDoor(G.Map.Doors[0]);G.Player.Aim=Vector3.left;}
   G.Rig.ResetView(ids[n]=="booth"?225:45);G.Rig.Tick(0);G.Vision.Refresh(G.Player.transform.position,G.Player.Aim);yield return Capture("200-indoor-monsters-"+ids[n]+".png");
   G.SwitchInterior(ids[n]);
  }
  G.Finish(false);
 }
}}
