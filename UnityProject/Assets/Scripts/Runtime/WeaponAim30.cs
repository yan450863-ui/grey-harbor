using UnityEngine;
namespace GreyHarbor {
 public enum AimReticleMode306 { None,HipFire,ScopeCross }
 public sealed partial class RaidGame {
  public bool ScopeReady30;public float ScopeHold30;public float ScopeBlend309 {get;private set;}
  public const float ScopeFadeSeconds309=.12f;
  public float ScopeOpacity309=>Mathf.SmoothStep(0,1,ScopeBlend309);
  public bool ScopeShadeVisible309=>ScopeBlend309>0&&Phase==GamePhase.Raid&&!Paused&&!InventoryOpen&&GetComponent<RaidUI>()?.FullMapOpen318!=true&&HasScope30&&Player&&!Player.Dead;LineRenderer laser30;public bool? AimOverride30;
  public bool ScopeAimRequested306=>Phase==GamePhase.Raid&&!Paused&&!InventoryOpen&&Player&&!Player.Dead&&HasScope30&&(AimOverride30??(Controls is IWeaponAim30 input&&input.Aiming30));
  public AimReticleMode306 ReticleMode306 {get {if(Phase!=GamePhase.Raid||Paused||InventoryOpen||!Player||Player.Dead)return AimReticleMode306.None;if(ScopeAimRequested306)return ScopeVisible305?AimReticleMode306.ScopeCross:AimReticleMode306.None;return AimReticleMode306.HipFire;}}
  public bool ScopeVisible305=>ScopeReady30&&Phase==GamePhase.Raid&&!Paused&&!InventoryOpen&&HasScope30;
  public bool HasScope30=>ModRules30.Installed(Equipped,ModSlot30.Optic)!=null;
  public void ResetScope30(){ScopeReady30=false;ScopeHold30=0;ScopeBlend309=0;Shader.SetGlobalFloat("_HarborScopeBlend335",0);}
  public void TickScope30(float dt){bool held=AimOverride30??(Controls is IWeaponAim30 input&&input.Aiming30);
   if(Phase!=GamePhase.Raid||Paused||InventoryOpen||GetComponent<RaidUI>()?.FullMapOpen318==true||!HasScope30||Player==null||Player.Dead||ReloadTime>0||HealTime>0||RepairTime>0){ResetScope30();return;}
   ScopeReady30=held;if(held&&Rig)Rig.Shake=0;ScopeHold30=held?ScopeHold30+Mathf.Max(0,dt):0;
   // Aiming is immediate. Only the presentation fades; it never gates firing or recoil.
   ScopeBlend309=Mathf.MoveTowards(ScopeBlend309,held?1:0,Mathf.Max(0,dt)/ScopeFadeSeconds309);
  }
  public void RenderScope30(){
   bool laser=Phase==GamePhase.Raid&&!InventoryOpen&&!Paused&&Equipped!=null&&ModRules30.Installed(Equipped,ModSlot30.Laser)!=null;
   if(!laser){if(laser30)laser30.enabled=false;return;}
   if(laser30==null){laser30=new GameObject("WeaponLaser30").AddComponent<LineRenderer>();laser30.transform.SetParent(transform);laser30.material=new Material(RenderShaders307.Load("HarborLaser307"));laser30.startColor=new Color(1,.2f,.15f,.75f);laser30.endColor=new Color(1,.3f,.2f,.4f);laser30.startWidth=laser30.endWidth=.012f;laser30.positionCount=2;}
   // Keep the thin laser at least 1.25 screen pixels wide so horizontal beams do not fall between raster rows.
   laser30.startWidth=laser30.endWidth=Mathf.Max(.012f,View.orthographicSize*2f/Mathf.Max(1,View.pixelHeight)*1.25f);
   laser30.sharedMaterial.SetVector("_Anchor",Player.transform.position+Vector3.up*.08f);var origin=Player.MuzzleOnPlane(View);Vector3 end=origin;float range=ActiveWeapon279.Range;for(float d=.1f;d<=range;d+=.1f){var p=origin+Player.Aim*d;if(!(ScopeShadeVisible309?Vision.LaserPath338(new Vector3(p.x,Player.transform.position.y,p.z)):Vision.CanSee(new Vector3(p.x,Player.transform.position.y,p.z))))break;if(Physics.Linecast(origin,p,SceneObstacle.SightMask,QueryTriggerInteraction.Ignore))break;end=p;}laser30.SetPosition(0,origin);laser30.SetPosition(1,end);laser30.enabled=true;
  }
  void DisposeScope30(){if(laser30&&laser30.sharedMaterial)Destroy(laser30.sharedMaterial);}
 }
 public sealed partial class RaidUI {
  void DrawScope30(){
   if(!G.ScopeShadeVisible309)return;
   CircularScope336.Draw(G.ScopeFrame336,G.HitMarker>0?red:white,G.ScopeOpacity309,G.ReticleMode306==AimReticleMode306.ScopeCross);
  }
 }
}
