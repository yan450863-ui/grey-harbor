using UnityEngine;
using System.Collections;
namespace GreyHarbor {
 public sealed partial class RaidVerification {
  IEnumerator VerifyPortContent132(){
   FreshCamp();string saved=JsonUtility.ToJson(G.Save);G.HarborWalkthrough=true;G.BeginTraining();G.Paused=true;yield return null;
   var outside=G.Map;Check(G.Enemies.Count==4&&outside.Loot.Count==3,"134 Exterior actors and containers present");
   for(int n=0;n<2;n++){
    string id=n==0?"portbooth":"portservice";var approach=n==0?new Vector3(-1.3f,0,-53):new Vector3(22,0,-43);
    G.Player.transform.position=approach;Physics.SyncTransforms();F111(false,0);F111(true,2.01f);G.Paused=true;yield return null;
    var map=G.Map;var actors=G.Enemies.ToArray();int expected=n==0?1:5;
    Check(map.InteriorId==id&&actors.Length==expected,"Fixed indoor monster count "+id);
    Check(map.Loot.Count==(n==0?3:8)&&map.Loot.TrueForAll(l=>l.Type!=SupplyType.Safe),"Container density without extra safe "+id);
    foreach(var e in actors){
     Check(!Physics.CheckCapsule(e.Home+Vector3.up*.35f,e.Home+Vector3.up,.34f,SceneObstacle.MovementMask)&&Vector3.Distance(e.Home,map.Spawn)>2.5f,"Spawn has room and arrival clearance "+e.name);
     foreach(var p in e.PatrolTargets)Check(!Physics.CheckCapsule(p+Vector3.up*.35f,p+Vector3.up,.34f,SceneObstacle.MovementMask)&&G.Navigation.FindPath(e.Home,p).Count>0,"Closed-room patrol waypoint reachable "+e.name+" "+p);
     e.WaitTime=0;e.PatrolIndex=1;float distance=0;bool clear=true;
     for(int j=0;j<80;j++){var before=e.transform.position;G.HarborPatrol(e,.05f);Physics.SyncTransforms();distance+=Vector3.Distance(before,e.transform.position);clear&=!Physics.CheckCapsule(e.transform.position+Vector3.up*.35f,e.transform.position+Vector3.up,.29f,SceneObstacle.MovementMask);}
     Check(distance>.6f&&clear,"Patrol moves without furniture clipping "+e.name);e.transform.position=e.Home;e.Route.Clear();e.WaitTime=2;
    }
    Physics.SyncTransforms();G.Player.transform.position=map.Spawn;G.Player.Aim=Vector3.forward;G.Vision.Refresh(G.Player.transform.position,G.Player.Aim);
    if(n==1){
     for(int i=0;i<4;i++){var d=map.Doors[i];var e=actors[i];Check(!G.Vision.RoomAllowed(e.Home)&&!G.Vision.CanSee(e.Home),"Closed room hides monster "+i);
      var loot=map.Loot[i*2];G.Player.transform.position=loot.Position;Physics.SyncTransforms();Check(!G.CanInteractLoot(loot),"Unknown room container cannot be searched "+i);
      G.Player.transform.position=new Vector3(i%2==0?-.9f:.9f,0,i<2?-3.5f:3.5f);Physics.SyncTransforms();Check(G.ToggleRoomDoor(d)&&d.Open,"Room door opens without spawn obstructing swing "+i);
     }
    }
    foreach(var l in map.Loot){
     Check(l.Items.Count>=1&&l.Items.Count<=6&&l.Items.TrueForAll(p=>!p.Identified),"Container uses existing unknown loot rules "+l.Name);
     Check(!Physics.CheckCapsule(l.Position+Vector3.up*.35f,l.Position+Vector3.up,.29f,SceneObstacle.MovementMask)&&G.Navigation.FindPath(map.Spawn,l.Position).Count>0,"Search stand is clear and navigable "+l.Name+" "+l.Position);
     G.Player.transform.position=l.Position;G.Player.Aim=(l.View.transform.position-l.Position).normalized;Physics.SyncTransforms();G.Vision.Refresh(G.Player.transform.position,G.Player.Aim);G.CloseInventory();F111(false,0);F111(true,l.SearchDuration+.01f);
     Check(l.Opened&&G.OpenLoot==l&&l.Prop.Searched,"F opens actual container and visual "+l.Name);
     G.Paused=false;G.TickIdentification(RaidGame.IdentificationSeconds(l.Items[0].Kind)+.01f);G.Paused=true;
     Check(l.Items[0].Identified,"First item identification works "+l.Name);G.CloseInventory();
    }
    G.Player.transform.position=n==0?new Vector3(0,0,-1):new Vector3(-5.5f,0,-3.2f);G.Player.Aim=n==0?Vector3.forward:Vector3.back;G.Rig.ResetView(45);G.Rig.Tick(0);G.Vision.Refresh(G.Player.transform.position,G.Player.Aim);
    yield return Capture("132-"+id+"-content.png");
    if(n==1){G.Player.transform.position=new Vector3(-5,0,3.2f);G.Player.Aim=Vector3.forward;G.Rig.Tick(0);G.Vision.Refresh(G.Player.transform.position,G.Player.Aim);yield return Capture("132-storage-content.png");}
    var searched=map.Loot[0];int remains=searched.Items.Count;var victim=actors[0];victim.Damage(999);G.MonsterDeath(victim);int total=map.Loot.Count;var corpse=map.Loot[total-1];Check(victim.Dead&&corpse.Items.Count>0,"Killed monster leaves searchable corpse "+id);
    G.Player.transform.position=map.Spawn;G.SwitchInterior(id);Check(G.Map==outside&&G.Enemies.Count==4&&!victim.gameObject.activeInHierarchy,"Indoor actors and corpses deactivate outdoors "+id);
    G.Player.transform.position=approach;G.SwitchInterior(id);Check(G.Map==map&&G.Enemies.Count==expected&&victim.Dead&&map.Loot.Count==total&&searched.Opened&&searched.Items.Count==remains,"Re-entry preserves deaths and searched containers without respawn "+id);
    G.SwitchInterior(id);
   }
   G.Controls=new DesktopInput();G.Finish(false);yield return null;Check(JsonUtility.ToJson(G.Save)==saved,"Port preview restores original formal inventory");
   FreshCamp();G.VerificationSmallMap=false;G.NextMap=RaidDestination.Harbor;Check(G.StartRaid(),"Formal harbor departure");
   G.Paused=true;yield return null;G.SwitchInterior("portservice");Check(!G.TrainingActive&&G.Enemies.Count==5&&G.Map.Loot.Count==8,"Same content in formal harbor");G.Finish(false);
  }
 }
}
