using UnityEngine;
namespace GreyHarbor {
 public static class RebelTuning145 {
  public const float Health=100,Damage=14,Penetration=10;
  public static ItemKind Armor(RebelKind k)=>k==RebelKind.BlueRegular?ItemKind.BlueArmor:ItemKind.Armor;
  public static float HealthFor(RebelKind k)=>NpcRules270.Health(k);
  public static float ArmorScale(RebelKind k)=>NpcRules270.Heavy(k)?1.5f:1;
  public static float MaxArmor(RebelKind k)=>NpcRules270.Armor(k);
  public static string Name(RebelKind k)=>SoldierProfiles2745.IsNew(k)?SoldierProfiles2745.Name(k):k==RebelKind.BlueHeavy?"二级蓝色持盾重甲士兵":NpcRules270.Heavy(k)?"绿色持盾重甲士兵":k==RebelKind.BlueRegular?"蓝色普通士兵":"绿色普通士兵";
  public static float Windup(RebelKind k)=>SoldierProfiles2745.IsNew(k)?SoldierProfiles2745.Windup(k):NpcRules270.Heavy(k)?1.5f:.6f;
  public static float Cooldown(RebelKind k)=>SoldierProfiles2745.IsNew(k)?SoldierProfiles2745.Cooldown(k):NpcRules270.Heavy(k)?1.8f:1.4f;
  public static float Speed(RebelKind k)=>SoldierProfiles2745.IsNew(k)?SoldierProfiles2745.Speed(k):NpcRules270.Heavy(k)?1.15f:1.6f;
 }
 public sealed partial class RaidGame {
  internal int ActiveGunShots145=>shots.FindAll(s=>!s.Spit).Count;
  public RaidActor SpawnRebel145(Vector3 p,RebelKind k,string location=null){
   var e=Actor((location??"叛军")+" / "+RebelTuning145.Name(k),p,true);
   e.IsRebel=true;e.Rebel=k;e.RebelArmor=null;NpcCombat275.InitializeSoldier(e,k);e.Body.sprite=RebelArt145.Get(k,0);e.RefreshWeaponArt279();e.WaitTime=2;e.AttackCooldown=1;Enemies.Add(e);return e;
  }
  public void TickRebel145(RaidActor e,float dt){
   if(e.Dead||!e.gameObject.activeInHierarchy||Player.Dead)return;
   if(SoldierProfiles2745.IsNew(e.Rebel)){TickNewSoldier2745(e,dt);return;}
   float dist=Vector3.Distance(Player.transform.position,e.transform.position);e.AttackCooldown=Mathf.Max(0,e.AttackCooldown-dt);
   bool visible=dist<10.5f&&ClearSight(e.transform.position,Player.transform.position);
   if(visible){e.Alert=Mathf.Min(1,e.Alert+dt/.85f);e.LastKnown=Player.transform.position;e.Memory=7;}
   else {e.Memory=Mathf.Max(0,e.Memory-dt);if(e.Memory<=0)e.Alert=Mathf.Max(0,e.Alert-dt*.5f);e.Windup=0;e.RebelBurst=0;}
   if(e.Windup>0){
    e.Windup=Mathf.Max(0,e.Windup-dt);
    if(e.Windup<=0&&visible){e.RebelBurst=3;e.RebelShotDelay=0;}
    else return;
   }
   if(e.RebelBurst>0){
    e.RebelShotDelay-=dt;
    if(visible&&e.RebelShotDelay<=0){var delta=Player.transform.position-e.transform.position;delta.y=0;e.Aim=delta.normalized;SpawnShot(e,Quaternion.Euler(0,UnityEngine.Random.Range(-4f,4f),0)*e.Aim);audioPlayer.PlayOneShot(enemyShotClip,Mathf.Clamp01(1-dist/SoundRules326.HearingRange327));e.RebelBurst--;e.RebelShotDelay=.20f;if(e.RebelBurst==0)e.AttackCooldown=RebelTuning145.Cooldown(e.Rebel);}
    return;
   }
   if(e.Alert>=1&&visible&&e.AttackCooldown<=0){e.Aim=(Player.transform.position-e.transform.position).normalized;e.Windup=RebelTuning145.Windup(e.Rebel);return;}
   if(e.Alert>.1f&&e.Memory>0){var delta=e.LastKnown-e.transform.position;delta.y=0;if(delta.sqrMagnitude>.01f)e.Aim=delta.normalized;if((!visible||dist>6.5f)&&delta.magnitude>1)FollowPath(e,e.LastKnown,RebelTuning145.Speed(e.Rebel),dt);}
   else if(e.PatrolTargets!=null){
    if(e.WaitTime>0){e.WaitTime=Mathf.Max(0,e.WaitTime-dt);return;}
    var goal=e.PatrolTargets[e.PatrolIndex%e.PatrolTargets.Length];if(Vector3.Distance(e.transform.position,goal)<.25f){e.PatrolIndex++;e.WaitTime=1.2f;e.Route.Clear();}else if(!FollowPath(e,goal,e.Rebel==RebelKind.Heavy?.8f:1.1f,dt)){e.PatrolIndex++;e.WaitTime=.4f;}
   }
  }
  public void RebelDeath145(RaidActor e){CreateCorpse325(e,true);
  }
 }
}

