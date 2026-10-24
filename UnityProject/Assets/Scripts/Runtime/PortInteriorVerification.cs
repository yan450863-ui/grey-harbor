using UnityEngine;
using System.Collections;
namespace GreyHarbor {
 public sealed partial class RaidVerification {
  IEnumerator VerifyPortInterior131(){
   FreshCamp();G.HarborWalkthrough=true;G.BeginTraining();G.Paused=true;yield return null;
   var outside=G.Map;Check(outside.Doors.FindAll(d=>d.Destination=="portbooth"||d.Destination=="portservice").Count==2,"Two entrance buildings have portals");
   float zoom=G.View.orthographicSize;
   for(int building=0;building<2;building++){
    var approach=building==0?new Vector3(-1.3f,0,-53):new Vector3(22,0,-43);
    Check(G.Navigation.FindPath(outside.Spawn,approach).Count>0,"Exterior approach reachable "+building);
    G.Player.transform.position=approach;G.Rig.ResetView(135);G.Rig.Tick(0);Physics.SyncTransforms();F111(false,0);F111(true,1.9f);
    Check(G.Map==outside&&G.DoorHold>1.8f,"Two-second hold required "+building);
    F111(true,.11f);G.Paused=true;yield return null;
    Check(G.Map.InteriorId==(building==0?"portbooth":"portservice"),"Correct distinct interior "+building);
    var inside=G.Map;Check(!outside.Root.gameObject.activeInHierarchy,"Outside isolated "+building);
    Check(inside.Loot.Count==(building==0?3:8)&&G.Enemies.Count==(building==0?1:5),"Interior content counts "+building);
    Check(Mathf.Abs(G.View.orthographicSize-zoom)<.01f,"Character and map scale preserved "+building);
    Check(Mathf.Abs(Mathf.DeltaAngle(G.Rig.Yaw,135))<.1f,"Entry camera inherited "+building);
    Check(Shader.GetGlobalTexture("_HarborVisibility")==G.Vision.Texture,"Indoor fog texture bound "+building);
    Check(G.Vision.Explored(inside.Spawn),"Entry room already discovered "+building);
    G.Rig.ResetView(45);G.Rig.Tick(0);G.Player.Aim=Vector3.forward;G.Vision.Refresh(G.Player.transform.position,G.Player.Aim);
    yield return Capture("131-"+inside.InteriorId+"-entry.png");
    if(building==1){
     Check(inside.Bounds.width>18&&inside.Doors.Count==6,"Larger annex with four rooms and exit");
     for(int i=0;i<4;i++){
      var door=inside.Doors[i];var r=RaidMap.PortRooms[i];var center=new Vector3(r.center.x,0,r.center.y);
      Check(!G.Vision.Explored(center),"Unopened room stays black "+i);
      G.Player.transform.position=new Vector3(i%2==0?-.9f:.9f,0,i<2?-3.5f:3.5f);Physics.SyncTransforms();
      F111(false,0);F111(true,.01f);
      Check(door.Open&&G.Vision.Explored(center),"Opening reveals room "+i);
      Check(G.Navigation.FindPath(G.Player.transform.position,center).Count>0,"Room accessible after opening "+i);
      G.Player.transform.position=door.Position;Check(!G.ToggleRoomDoor(door),"Closing cannot trap player "+i);
      G.Player.transform.position=center+Vector3.forward*(i<2?1.2f:0);G.Player.Aim=Vector3.forward;G.Rig.Tick(0);G.Vision.Refresh(G.Player.transform.position,G.Player.Aim);
      yield return Capture("131-room-"+i+".png");
     }
    }
    G.Rig.ResetView(225);G.Player.transform.position=inside.Spawn;Physics.SyncTransforms();F111(false,0);F111(true,2.01f);G.Paused=true;yield return null;
    Check(G.Map==outside&&Mathf.Abs(Mathf.DeltaAngle(G.Rig.Yaw,135))<.1f,"Exit restores exterior camera "+building);
    Check(Vector3.Distance(G.Player.transform.position,approach)<.05f,"Exit returns to same entrance "+building);
    F111(false,0);F111(true,2.01f);G.Paused=true;yield return null;
    Check(G.Map==inside&&Mathf.Abs(Mathf.DeltaAngle(G.Rig.Yaw,225))<.1f,"Re-entry preserves indoor instance and view "+building);
    if(building==1)Check(inside.Doors.TrueForAll(d=>d.Portal||d.Locked||d.Open),"Room doors persist after re-entry");
    G.Player.transform.position=inside.Spawn;Physics.SyncTransforms();F111(false,0);F111(true,2.01f);G.Paused=true;yield return null;
   }
   G.Controls=new DesktopInput();G.Finish(false);yield return null;Check(!outside.Root,"Area roots disposed after session");
   FreshCamp();G.VerificationSmallMap=false;G.NextMap=RaidDestination.Harbor;Check(G.StartRaid(),"Formal departure succeeds");G.Paused=true;yield return null;
   G.Player.transform.position=new Vector3(-1.3f,0,-53);Physics.SyncTransforms();F111(false,0);F111(true,2.01f);G.Paused=true;yield return null;Check(!G.TrainingActive&&G.Map.InteriorId=="portbooth","Formal harbor also supports interior entry");G.Controls=new DesktopInput();G.Finish(false);yield return null;
   FreshCamp();G.NextMap=RaidDestination.Training;G.StartRaid();G.Paused=true;yield return null;G.Player.transform.position=RaidMap.BoothApproach;Physics.SyncTransforms();F111(false,0);F111(true,2.01f);G.Paused=true;yield return null;Check(G.Map.InteriorId=="booth"&&G.Map.Loot.Count==6&&G.Enemies.Count==1,"Existing training booth content remains separate");G.Controls=new DesktopInput();G.Finish(false);
  }
 }
}
