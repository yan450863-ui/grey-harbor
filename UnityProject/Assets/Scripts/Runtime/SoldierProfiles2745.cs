using UnityEngine;
namespace GreyHarbor {
 // Independent NPC profiles. Player weapon tuning never changes these values.
 public static class SoldierProfiles2745 {
  public static bool IsNew(RebelKind k)=>IsSmg(k)||IsMarksman(k);
  public static bool IsSmg(RebelKind k)=>k==RebelKind.Smg||k==RebelKind.BlueSmg;
  public static bool IsMarksman(RebelKind k)=>k==RebelKind.Marksman||k==RebelKind.BlueMarksman;
  public static ItemKind Weapon(RebelKind k)=>IsSmg(k)?ItemKind.MP5:IsMarksman(k)?ItemKind.SKS:ItemKind.AK;
  public static ItemKind Ammo(RebelKind k)=>IsSmg(k)?(NpcRules270.Level(k)==2?ItemKind.Blue9mm:ItemKind.Green9mm):(NpcRules270.Level(k)==2?ItemKind.Ammo:ItemKind.GreenAmmo);
  public static string Name(RebelKind k)=>(NpcRules270.Level(k)==2?"蓝色":"绿色")+(IsSmg(k)?"冲锋枪士兵":"狙击士兵");
  public static float Damage(RebelKind k)=>IsSmg(k)?10:IsMarksman(k)?22:14;
  public static float Range(RebelKind k)=>IsSmg(k)?8:IsMarksman(k)?15:10.5f;
  public static float Windup(RebelKind k)=>IsMarksman(k)?1:.45f;
  public static float Cooldown(RebelKind k)=>IsMarksman(k)?2.2f:1.4f;
  public static float Speed(RebelKind k)=>IsMarksman(k)?1.1f:1.8f;
  public static int Burst(RebelKind k)=>IsMarksman(k)?1:4;
  public static float ShotInterval(RebelKind k)=>IsMarksman(k)?2.2f:.13f;
  public static float Spread(RebelKind k,bool moving)=>IsMarksman(k)?1.4f:moving?9:4;
 }
 public sealed partial class RaidGame {
  public void TickNewSoldier2745(RaidActor e,float dt){
   if(e.Dead||!e.gameObject.activeInHierarchy||Player.Dead)return;
   bool marksman=SoldierProfiles2745.IsMarksman(e.Rebel);
   float dist=Vector3.Distance(Player.transform.position,e.transform.position);
   e.AttackCooldown=Mathf.Max(0,e.AttackCooldown-dt);
   bool visible=dist<SoldierProfiles2745.Range(e.Rebel)&&ClearSight(e.transform.position,Player.transform.position);
   if(visible){e.Alert=Mathf.Min(1,e.Alert+dt/.85f);e.LastKnown=Player.transform.position;e.Memory=marksman?3:7;}
   else {e.Memory=Mathf.Max(0,e.Memory-dt);if(e.Memory<=0)e.Alert=Mathf.Max(0,e.Alert-dt*.5f);e.Windup=0;e.RebelBurst=0;}
   // Aiming must complete under uninterrupted sight; hidden targets cancel the shot.
   if(e.Windup>0){
    e.Aim=(Player.transform.position-e.transform.position).normalized;
    e.Windup=Mathf.Max(0,e.Windup-dt);
    if(e.Windup<=0&&visible){e.RebelBurst=SoldierProfiles2745.Burst(e.Rebel);e.RebelShotDelay=0;}
    else return;
   }
   if(e.RebelBurst>0){
    bool moving=false;
    if(!marksman&&visible&&dist>5){var before=e.transform.position;FollowPath(e,e.LastKnown,SoldierProfiles2745.Speed(e.Rebel)*.55f,dt);moving=(e.transform.position-before).sqrMagnitude>.000001f;}
    e.RebelShotDelay-=dt;
    if(visible&&e.RebelShotDelay<=0&&ClearSight(e.transform.position,Player.transform.position)){
     var delta=Player.transform.position-e.transform.position;delta.y=0;e.Aim=delta.normalized;
     float spread=SoldierProfiles2745.Spread(e.Rebel,moving);
     SpawnShot(e,Quaternion.Euler(0,UnityEngine.Random.Range(-spread,spread),0)*e.Aim);
     audioPlayer.PlayOneShot(enemyShotClip,Mathf.Clamp01(1-dist/SoundRules326.HearingRange327));
     e.RebelBurst--;e.RebelShotDelay=SoldierProfiles2745.ShotInterval(e.Rebel);
     if(e.RebelBurst==0)e.AttackCooldown=SoldierProfiles2745.Cooldown(e.Rebel);
    }
    return;
   }
   if(e.Alert>=1&&visible&&e.AttackCooldown<=0){e.Aim=(Player.transform.position-e.transform.position).normalized;e.Windup=SoldierProfiles2745.Windup(e.Rebel);return;}
   if(e.Alert>.1f&&e.Memory>0){
    var delta=e.LastKnown-e.transform.position;delta.y=0;if(delta.sqrMagnitude>.01f)e.Aim=delta.normalized;
    if(marksman){
     // A stationed marksman investigates only near its guard point, never follows across the floor.
     if(!visible){var target=e.Home+Vector3.ClampMagnitude(e.LastKnown-e.Home,3);if(Vector3.Distance(e.transform.position,target)>.8f)FollowPath(e,target,SoldierProfiles2745.Speed(e.Rebel),dt);}
    }else if((!visible||dist>5)&&delta.magnitude>1)FollowPath(e,e.LastKnown,SoldierProfiles2745.Speed(e.Rebel),dt);
   }else if(marksman){if(Vector3.Distance(e.transform.position,e.Home)>.3f)FollowPath(e,e.Home,SoldierProfiles2745.Speed(e.Rebel),dt);}
   else if(e.PatrolTargets!=null&&e.PatrolTargets.Length>0){
    if(e.WaitTime>0){e.WaitTime=Mathf.Max(0,e.WaitTime-dt);return;}
    var goal=e.PatrolTargets[e.PatrolIndex%e.PatrolTargets.Length];
    if(Vector3.Distance(e.transform.position,goal)<.25f){e.PatrolIndex++;e.WaitTime=1.2f;e.Route.Clear();}
    else if(!FollowPath(e,goal,1.2f,dt)){e.PatrolIndex++;e.WaitTime=.4f;}
   }
  }
 }
}
