using UnityEngine;
using System.Collections.Generic;
namespace GreyHarbor {
 public sealed class AkShotSample276 {
  public ItemKind Weapon,Ammo;public int Number,Burst;public float Clock,Recoil,Spread,RandomOffset,Move;
  public Vector3 RawAim,Aim,Origin,Direction,Impact;public string Hit="";public bool Ended;
  public Vector2 InputScreen,RawScreen,AdjustedScreen;public Vector3 RawTarget,AimTarget,MoveDelta;
  public Quaternion CameraRotation;public float VerticalRecoilPixels,MovementExtraSpread,MoveDt;public int MovementStability;
  public Vector3 EyePosition,CameraPosition;public Matrix4x4 ViewProjection;public Rect CameraPixelRect;
 }
 public sealed class GreenAkHandling276 {
  public const float FirstSpread=1.2f,HeatPerShot=.22f,MaxHeat=3.75f,MoveSpread=WeaponHandling277.BaseWalkingSpread*(1-WeaponHandling277.AkMovementStability/100f);
  public const float FirstKick=.85f,KickGrowth=.08f,MaxKickGrowth=.55f,MaxRecoil=16;
  public const float RecoveryDelay=.20f,RecoilRecovery=12,HeatRecovery=2.8f;
  public float Recoil,Heat,SinceShot=10,Clock,Move,LastRandomOffset;public int Burst,Shots;
  public WeaponProfile279 Profile=WeaponRules279.AK;
  int movementStability277=WeaponHandling277.AkMovementStability;
  public int MovementStability {get=>movementStability277;set=>movementStability277=Mathf.Clamp(value,0,100);}
  public float MovementExtraSpread=>WeaponHandling277.MovementExtraHalfAngle(MovementStability,Move);
  public float Spread=>Profile.FirstSpread+Heat+MovementExtraSpread;
  public void Reset(){Recoil=Heat=Clock=Move=LastRandomOffset=0;SinceShot=10;Burst=Shots=0;}
  public void Tick(float dt,float movement){
   dt=Mathf.Max(0,dt);Clock+=dt;Move=Mathf.Clamp(movement,0,SprintState2711.SpeedMultiplier);
   float old=SinceShot;SinceShot+=dt;float recovery=Mathf.Max(0,SinceShot-Mathf.Max(old,Profile.RecoveryDelay));
   if(recovery>0){Recoil=Mathf.MoveTowards(Recoil,0,recovery*Profile.RecoilRecovery);Heat=Mathf.MoveTowards(Heat,0,recovery*Profile.HeatRecovery);}
   if(SinceShot>=.35f)Burst=0;
  }
  public Vector3 Direction(Vector3 aim){LastRandomOffset=Random.Range(-Spread,Spread);return Quaternion.Euler(0,LastRandomOffset,0)*aim;}
  public void Fired(float recoilMultiplier=1){Recoil=Mathf.Min(Profile.MaxRecoil*recoilMultiplier,Recoil+(Profile.FirstKick+Mathf.Min(Burst*Profile.KickGrowth,Profile.MaxKickGrowth))*recoilMultiplier);Heat=Mathf.Min(Profile.MaxHeat,Heat+Profile.HeatPerShot);Burst++;Shots++;SinceShot=0;}
 }
 public sealed partial class RaidGame {
  public readonly GreenAkHandling276 AkHandling276=new GreenAkHandling276();
  public readonly List<AkShotSample276> AkShots276=new List<AkShotSample276>();
  public Vector3 LastEnemyShotDirection276;public float LastEnemyShotDamage276,LastEnemyShotPen276;
  public Vector3 RawPlayerAim276=Vector3.forward;float akAimDistance276=8;bool akPending276;ItemStack akEquipped276;
  public Vector3 RawAimTarget277,AdjustedAimTarget277,LastActualMovement277;
  public Vector2 RawInputScreen277,RawAimScreen277,AdjustedAimScreen277;
  public float RecoilPixels277,LastMovementDt277;
  public bool UsesAkHandling276=>Equipped!=null&&Equipped.Kind==ItemKind.AK;
  public WeaponProfile279 ActiveWeapon279=>Equipped==null?null:ModRules30.Profile(Equipped);
  public int MagazineCapacity279=>ActiveWeapon279?.MagazineSize??0;
  public bool UsesWeaponHandling279=>ActiveWeapon279!=null;
  bool triggerHeld279=true,triggerBlocked279=true;
  public bool TickPlayerTrigger279(bool fire,bool allowed){
   if(!allowed){triggerHeld279=fire;triggerBlocked279=true;return false;}
   if(!fire){triggerHeld279=triggerBlocked279=false;return false;}
   if(triggerBlocked279){triggerHeld279=true;return false;}
   bool pressed=!triggerHeld279;triggerHeld279=true;return ActiveWeapon279!=null&&(ActiveWeapon279.Automatic||pressed);
  }
  public void WeaponChanged279(){ResetScope30();ResetAkHandling276();triggerBlocked279=true;ReloadTime=0;reloadId=null;RestoreActiveMagazine314();}
  public void ResetAkHandling276(){
   AkHandling276.Reset();AkHandling276.Profile=ActiveWeapon279??WeaponRules279.AK;AkHandling276.MovementStability=AkHandling276.Profile.MovementStability;AkShots276.Clear();akPending276=false;akEquipped276=Equipped;RawPlayerAim276=Player?Player.Aim:Vector3.forward;
   LastActualMovement277=Vector3.zero;LastMovementDt277=RecoilPixels277=0;
   var anchor=Player?Player.transform.position+Vector3.up*CombatHeight:Vector3.up*CombatHeight;
   RawAimTarget277=AdjustedAimTarget277=anchor+RawPlayerAim276*8;akAimDistance276=8;
  }
  void CheckAkScope276(){
   // New raids reset explicitly. Interior/floor transfers retain the same active-raid handling state.
   if(Phase!=GamePhase.Raid||akEquipped276!=Equipped){
    if(AkHandling276.Shots>0||AkHandling276.Heat>0||akEquipped276!=Equipped)ResetAkHandling276();
   }
  }
  public void TickAkHandling276(float dt,float movement){
   if(Phase!=GamePhase.Raid||Paused||InventoryOpen)return;
   if(UsesWeaponHandling279){AkHandling276.Profile=ActiveWeapon279;AkHandling276.MovementStability=ActiveWeapon279.MovementStability;AkHandling276.Tick(dt,ScopeReady30?movement*.65f:movement);}else AkHandling276.Reset();
  }
  void ApplyAkAim276(Vector3 aim){
   RawPlayerAim276=aim.normalized;RawInputScreen277=Controls.AimScreen;
   RawAimTarget277=Player.transform.position+Vector3.up*CombatHeight+aim;ReprojectAkAim277();
  }
  void ReprojectAkAim277(){
   var anchor=Player.transform.position+Vector3.up*CombatHeight;
   WeaponHandling277.ProjectScreenUpRecoil(View,anchor,RawAimTarget277,UsesWeaponHandling279?AkHandling276.Recoil:0,out AdjustedAimTarget277,out RawAimScreen277,out AdjustedAimScreen277,out RecoilPixels277);
   var aim=AdjustedAimTarget277-anchor;aim.y=0;akAimDistance276=aim.magnitude;
   if(aim.sqrMagnitude>.000001f)Player.Aim=aim.normalized;
  }
  Vector3 PlayerShotDirection276(){
   akPending276=UsesWeaponHandling279;AkHandling276.Profile=ActiveWeapon279??WeaponRules279.AK;AkHandling276.MovementStability=AkHandling276.Profile.MovementStability;
   return akPending276?AkHandling276.Direction(Player.Aim):Quaternion.Euler(0,Random.Range(-1.2f,1.2f),0)*Player.Aim;
  }
  void FinishAkShot276(){if(UsesWeaponHandling279){AkHandling276.Fired(ScopeReady30?1:1.3f);ReprojectAkAim277();}}
  AkShotSample276 RecordAkProjectile276(Vector3 origin,Vector3 direction){
   bool handled=akPending276;akPending276=false;if(!VerifyMode||!handled)return null;
   var s=new AkShotSample276{Number=AkHandling276.Shots+1,Burst=AkHandling276.Burst+1,Clock=AkHandling276.Clock,Recoil=AkHandling276.Recoil,Spread=AkHandling276.Spread,RandomOffset=AkHandling276.LastRandomOffset,Move=AkHandling276.Move,RawAim=RawPlayerAim276,Aim=Player.Aim,Origin=origin,Direction=direction};
   s.Weapon=Equipped.Kind;s.Ammo=MagazineAmmo;s.InputScreen=RawInputScreen277;s.RawScreen=RawAimScreen277;s.AdjustedScreen=AdjustedAimScreen277;s.RawTarget=RawAimTarget277;s.AimTarget=AdjustedAimTarget277;s.CameraRotation=View.transform.rotation;s.VerticalRecoilPixels=RecoilPixels277;
   s.MovementStability=AkHandling276.MovementStability;s.MovementExtraSpread=AkHandling276.MovementExtraSpread;s.MoveDelta=LastActualMovement277;s.MoveDt=LastMovementDt277;
   s.EyePosition=Player.transform.position+Vector3.up*CombatHeight;s.CameraPosition=View.transform.position;s.ViewProjection=View.projectionMatrix*View.worldToCameraMatrix;s.CameraPixelRect=View.pixelRect;
   if(AkShots276.Count>=512)AkShots276.RemoveAt(0);AkShots276.Add(s);return s;
  }
  public Vector2 AkCrosshairScreen276 {
   get {if(!UsesWeaponHandling279)return Controls.AimScreen;var p=View.WorldToScreenPoint(Player.transform.position+Vector3.up*CombatHeight+Player.Aim*akAimDistance276);return new Vector2(p.x,p.y);}
  }
  public float AkCrosshairRadius276 {
   get {if(!UsesWeaponHandling279)return 7;var p=Player.transform.position+Vector3.up*CombatHeight;var a=View.WorldToScreenPoint(p+Player.Aim*akAimDistance276);float radius=0;
    float halfAngle=AkHandling276.Spread+(Equipped?.Kind==ItemKind.SG12?ShotgunFan30:0);
    foreach(float sign in new[]{-1f,1f}){var b=View.WorldToScreenPoint(p+(Quaternion.Euler(0,halfAngle*sign,0)*Player.Aim)*akAimDistance276);radius=Mathf.Max(radius,Vector2.Distance(new Vector2(a.x,a.y),new Vector2(b.x,b.y)));}return radius;
   }
  }
 }
}
