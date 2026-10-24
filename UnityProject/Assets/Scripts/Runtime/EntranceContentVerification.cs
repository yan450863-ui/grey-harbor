using UnityEngine;
using System.Collections;
namespace GreyHarbor {
 public sealed partial class RaidVerification {
  IEnumerator VerifyEntranceContent134(){
   FreshCamp();G.HardMode=false;G.HarborWalkthrough=true;G.BeginTraining();G.Paused=true;yield return null;
   var outside=G.Map;var actors=G.Enemies.ToArray();var containers=outside.Loot.ToArray();
   yield return RollingCaseGallery134();
   G.Player.transform.position=new Vector3(.5f,0,-36.5f);G.Player.Aim=Vector3.forward;G.Rig.ResetView(45);G.Rig.Tick(0);G.Vision.Refresh(G.Player.transform.position,G.Player.Aim);yield return Capture("134-waiting-luggage-closed.png");
   Check(actors.Length==4&&System.Array.FindAll(actors,e=>e.Monster==MonsterKind.Wanderer).Length==2&&System.Array.FindAll(actors,e=>e.Monster==MonsterKind.Runner).Length==1&&System.Array.FindAll(actors,e=>e.Monster==MonsterKind.Spitter).Length==1,"Four entrance patrols have the approved mix");
   Check(containers.Length==3&&System.Array.FindAll(containers,l=>l.Prop.Look==ContainerLook.RollingSuitcase).Length==2&&System.Array.FindAll(containers,l=>l.Prop.Look==ContainerLook.Toolbox).Length==1,"Two rolling cases and one bench toolbox only");
   Check(System.Array.TrueForAll(containers,l=>l.Tier==ContainerTier.Common&&l.Type!=SupplyType.Safe),"Exterior containers use common non-safe loot table");
   foreach(var e in actors){
    Check(!Physics.CheckCapsule(e.Home+Vector3.up*.35f,e.Home+Vector3.up,.34f,SceneObstacle.MovementMask)&&Vector3.Distance(e.Home,outside.Spawn)>10,"Monster spawn clear and away from arrival "+e.name);
    foreach(var p in e.PatrolTargets)Check(!Physics.CheckCapsule(p+Vector3.up*.35f,p+Vector3.up,.34f,SceneObstacle.MovementMask)&&G.Navigation.FindPath(e.Home,p).Count>0,"Exterior patrol waypoint clear and reachable "+p);
    Check(outside.Doors.TrueForAll(d=>Vector3.Distance(d.Position,e.Home)>5),"Exterior monster keeps distance from building entrances "+e.name);
    e.WaitTime=0;e.PatrolIndex=1;float traveled=0;bool clear=true;
    for(int i=0;i<100;i++){var before=e.transform.position;G.HarborPatrol(e,.05f);Physics.SyncTransforms();traveled+=Vector3.Distance(before,e.transform.position);clear&=!Physics.CheckCapsule(e.transform.position+Vector3.up*.35f,e.transform.position+Vector3.up,.29f,SceneObstacle.MovementMask);}
    Check(traveled>1&&clear,"Exterior monster patrol actually moves without clipping "+e.name);e.transform.position=e.Home;e.Route.Clear();e.WaitTime=2;
   }
   foreach(var l in containers){
    Check(!Physics.CheckCapsule(l.Position+Vector3.up*.35f,l.Position+Vector3.up,.29f,SceneObstacle.MovementMask)&&G.Navigation.FindPath(outside.Spawn,l.Position).Count>0,"Container search stand clear and reachable "+l.Name);
    if(l.Prop.Look==ContainerLook.RollingSuitcase){Check(l.Prop.transform.Find("TelescopicRail")&&l.Prop.transform.Find("RollingWheel")&&l.Prop.Opening,"Rolling suitcase has wheels pull handle and opening");Check(l.Prop.transform.Find("CaseBody").gameObject.layer==SceneObstacle.LowLayer,"Rolling suitcase blocks movement as low obstacle");}
    G.Player.transform.position=l.Position;Physics.SyncTransforms();G.Player.Aim=Vector3.forward;G.Vision.Refresh(G.Player.transform.position,G.Player.Aim);F111(false,0);F111(true,l.SearchDuration+.01f);
    Check(l.Opened&&G.OpenLoot==l&&l.Prop.Searched,"F search opens exterior container "+l.Name);
    G.Paused=false;G.TickIdentification(RaidGame.IdentificationSeconds(l.Items[0].Kind)+.01f);var item=l.Items[0];Check(item.Identified&&G.QuickTransfer(ItemArea.Loot,item.Cell),"Exterior loot identifies and transfers "+l.Name);G.Paused=true;G.CloseInventory();
   }
   Check(G.Navigation.FindPath(outside.Spawn,new Vector3(.5f,0,-36)).Count>0&&G.Navigation.FindPath(outside.Spawn,new Vector3(-1.3f,0,-53)).Count>0&&G.Navigation.FindPath(outside.Spawn,new Vector3(22,0,-43)).Count>0,"Luggage and bench preserve waiting area and both building approaches");
   foreach(var p in outside.Exits)Check(G.Navigation.FindPath(outside.Spawn,p).Count>0,"Extraction remains reachable "+p);
   G.Player.transform.position=new Vector3(.5f,0,-36.5f);G.Player.Aim=Vector3.forward;G.Rig.ResetView(45);G.Rig.Tick(0);G.Vision.Refresh(G.Player.transform.position,G.Player.Aim);yield return Capture("134-waiting-luggage-open.png");
   G.Player.transform.position=new Vector3(12,0,-38.4f);G.Player.Aim=Vector3.forward;G.Rig.Tick(0);G.Vision.Refresh(G.Player.transform.position,G.Player.Aim);yield return Capture("134-maintenance-toolbox.png");
   G.Player.transform.position=new Vector3(24.5f,0,-54.5f);G.Player.Aim=Vector3.forward;G.Rig.Tick(0);G.Vision.Refresh(G.Player.transform.position,G.Player.Aim);yield return Capture("134-parking-patrol.png");
   G.Player.transform.position=new Vector3(22,0,-43);Physics.SyncTransforms();F111(false,0);F111(true,2.01f);G.Paused=true;yield return null;
   var inside=G.Map;Check(inside.InteriorId=="portservice"&&G.Enemies.Count==5&&System.Array.TrueForAll(actors,e=>!e.gameObject.activeInHierarchy),"Exterior actors isolate from annex interior");
   var locked=inside.Doors.Find(d=>d.Locked);Check(locked!=null&&!locked.Open&&!locked.Portal&&inside.Bounds.xMin<=-14,"Storage has a reserved locked side room");
   G.Player.transform.position=new Vector3(-.9f,0,3.5f);Physics.SyncTransforms();Check(G.ToggleRoomDoor(inside.Doors[2]),"Storage entrance still opens");
   G.Player.transform.position=RaidMap.PortLockedDoorApproach;Physics.SyncTransforms();F111(false,0);F111(true,.01f);
   Check(G.NearbyDoor==locked&&!locked.Open&&G.Notice.Contains("钥匙"),"Locked-door F interaction explains key requirement without opening");
   Check(!G.Vision.Explored(new Vector3(-11.5f,0,3)),"Reserved key room stays black");
   Check(!G.ClearSight(RaidMap.PortLockedDoorApproach,new Vector3(-11.5f,0,2.4f))&&G.Navigation.FindPath(RaidMap.PortLockedDoorApproach,new Vector3(-11.5f,0,2.4f)).Count==0,"Sealed key room blocks shots sight and navigation");
   for(int i=0;i<12;i++)G.Player.Move(Vector3.left*.15f);Check(G.Player.transform.position.x>-9,"Player cannot walk into locked room");
   G.Player.transform.position=new Vector3(-6.8f,0,2.4f);G.Player.Aim=Vector3.left;G.Rig.ResetView(45);G.Rig.Tick(0);G.Vision.Refresh(G.Player.transform.position,G.Player.Aim);yield return Capture("134-storage-locked-door.png");G.Rig.ResetView(225);G.Rig.Tick(0);yield return Capture("134-storage-locked-door-reverse.png");
   G.SwitchInterior("portservice");Check(G.Map==outside&&G.Enemies.Count==4&&G.Enemies.Contains(actors[0])&&outside.Loot.Count==3&&containers[0].Opened,"Returning restores exterior actors and searched luggage without duplicates");
   var victim=actors[0];victim.Damage(999);G.MonsterDeath(victim);Check(outside.Loot.Count==4&&outside.Loot[3].Items.Count>0,"Exterior kill creates one loot corpse");
   G.SwitchInterior("portservice");Check(G.Map==inside&&inside.Doors.Find(d=>d.Locked)==locked&&!locked.Open,"Re-entry preserves the locked room");
   G.SwitchInterior("portservice");Check(G.Enemies.Contains(victim)&&victim.Dead&&outside.Loot.Count==4,"Exterior death and corpse persist across room visit");
   G.Controls=new DesktopInput();G.Finish(false);yield return null;
   FreshCamp();G.VerificationSmallMap=false;G.NextMap=RaidDestination.Harbor;Check(G.StartRaid()&&G.Enemies.Count==4&&G.Map.Loot.Count==3,"Formal harbor has same entrance content");G.Finish(false);
   FreshCamp();G.NextMap=RaidDestination.Training;G.StartRaid();Check(G.Enemies.Count==4&&G.Map.Loot.Count==6,"Training exterior still uses original content");G.Finish(false);
  }
 }
}
