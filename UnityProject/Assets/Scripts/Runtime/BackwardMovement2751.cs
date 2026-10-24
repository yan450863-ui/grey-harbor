using System;using System.IO;using System.Collections;using UnityEngine;
namespace GreyHarbor {
 public sealed partial class RaidGame {
  public Vector3 PlayerWalkVelocity2751(Vector2 movement){
   movement=Vector2.ClampMagnitude(movement,1);
   Vector3 direction=Rig.Right*movement.x+Rig.Forward*movement.y;
   Player.Backward=RaidActor.IsBackward(direction,Player.Aim);
   return direction*(Player.Backward?2.55f:3.4f);
  }
 }
 public sealed partial class RaidVerification {
  IEnumerator RunBackward2751(){
   string dir=Path.GetDirectoryName(G.SavePath);var names=new[]{"stash-v40.json","stash-v41.json","stash-v42.json"};var before=Array.ConvertAll(names,n=>File.Exists(Path.Combine(dir,n))?File.ReadAllBytes(Path.Combine(dir,n)):null);
   Check(Application.version=="2.7.51"&&G.SavePath.EndsWith("verification-stash-2.7.51.json"),"2751 isolated save and version");
   // Rotate both aim and travel through world directions; assert actual rear-sector boundaries.
   foreach(float heading in new[]{0f,45f,90f,180f,270f})foreach(float angle in new[]{0f,45f,90f,119.9f,120f,120.1f,135f,180f,225f,239.9f,240f,240.1f,270f}){
    var aim=Quaternion.Euler(0,heading,0)*Vector3.forward;var move=Quaternion.Euler(0,angle,0)*aim;
    Check(RaidActor.IsBackward(move,aim)==(angle>=120&&angle<=240),"2751 rear120 sector heading="+heading+" movementAngle="+angle);
   }
   Check(!RaidActor.IsBackward(Vector3.zero,Vector3.forward)&&!RaidActor.IsBackward(Vector3.back,Vector3.zero),"2751 stationary and undefined aim do not slow");
   StandardRaid();G.enabled=false;G.Paused=false;G.InventoryOpen=false;var start=G.Player.transform.position;
   foreach(float camera in new[]{45f,135f,225f,315f})foreach(Vector2 key in new[]{Vector2.up,Vector2.right,Vector2.down,Vector2.left,new Vector2(1,1),new Vector2(-1,-1)})foreach(float angle in new[]{0f,90f,119.9f,120f,180f,240f,240.1f}){
    G.Rig.ResetView(camera);G.Rig.Tick(0);var travel=(G.Rig.Right*key.x+G.Rig.Forward*key.y).normalized;
    G.Player.Aim=Quaternion.Euler(0,angle,0)*travel;G.Player.Facing=-1;var velocity=G.PlayerWalkVelocity2751(key);
    bool rear=angle>=120&&angle<=240;Check(G.Player.Backward==rear&&Mathf.Abs(velocity.magnitude-(rear?2.55f:3.4f))<.0001f,"2751 actual camera-relative WASD diagonal speed camera="+camera+" key="+key+" angle="+angle);
    // Sprite flip must not decide speed.
    G.Player.Facing=1;Check(Vector3.Distance(G.PlayerWalkVelocity2751(key),velocity)<.00001f,"2751 sprite facing does not change world movement speed");
   }
   G.Rig.ResetView(45);G.Rig.Tick(0);
   foreach(bool back in new[]{false,true})foreach(bool run in new[]{false,true}){
    G.Player.transform.position=start;Physics.SyncTransforms();G.Player.Aim=back?-G.Rig.Forward:G.Rig.Forward;G.ResetSprint2711();if(run)G.Sprint2711.Toggle();var velocity=G.PlayerWalkVelocity2751(Vector2.up);G.MovePlayer2711(velocity,.1f);
    float expected=(back?2.55f:3.4f)*(run?1.5f:1)*.1f;Check(Mathf.Abs(Vector3.Distance(start,G.Player.transform.position)-expected)<.001f,"2751 actual swept player walk/run distance backward="+back+" sprint="+run);
    Check(Mathf.Abs(G.Sprint2711.Stamina-(run?19.9f:20))<.001f,"2751 existing sprint consumption unchanged");
   }
   FreshCamp();G.CloseInventory();G.Rig.ResetView(45);G.Rig.Tick(0);G.Player.Aim=-G.Rig.Forward;Check(Mathf.Abs(G.PlayerWalkVelocity2751(Vector2.up).magnitude-2.55f)<.0001f,"2751 camp uses same rear cone movement rule");
   for(int i=0;i<names.Length;i++)Check(MainBytes273(before[i],File.Exists(Path.Combine(dir,names[i]))?File.ReadAllBytes(Path.Combine(dir,names[i])):null),"2751 formal save unchanged "+names[i]);
   G.Paused=true;yield return null;
  }
 }
}
