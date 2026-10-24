using System;
using System.Collections.Generic;
using UnityEngine;
namespace GreyHarbor {
 public sealed partial class RaidGame {
  public const int ShotgunPellets30=8;public const float ShotgunFan30=12;
  public int LastShotgunHits30;public readonly Dictionary<RaidActor,int> LastShotgunTargets30=new Dictionary<RaidActor,int>();
  sealed class ShotgunVolley303 {public int Remaining=ShotgunPellets30,AmmoGrade;public float Damage,Penetration;public readonly Dictionary<RaidActor,int> Hits=new Dictionary<RaidActor,int>();}
  ShotgunVolley303 lastShotgunVolley303;
  void ClearShotgunVolley303(){lastShotgunVolley303=null;LastShotgunHits30=0;LastShotgunTargets30.Clear();}
  public void FireShotgun30(Vector3 direction){
   var p=ActiveWeapon279;if(p==null||p.Kind!=ItemKind.SG12||direction.sqrMagnitude<.00001f)return;Player.Shot();Player.Draw(View,0);
   Vector3 origin=Player.MuzzleOnPlane(View),chest=Player.transform.position+Vector3.up*CombatHeight;
   if(Physics.Linecast(chest,origin,out var block,SceneObstacle.SightMask,QueryTriggerInteraction.Ignore))origin=block.point-(origin-chest).normalized*.015f;
   var volley=new ShotgunVolley303{Damage=p.Damage,Penetration=p.Penetration,AmmoGrade=RaidInventory.Grade(MagazineAmmo)};lastShotgunVolley303=volley;LastShotgunHits30=0;LastShotgunTargets30.Clear();
   for(int n=0;n<ShotgunPellets30;n++){
    float spread=Mathf.Lerp(-ShotgunFan30,ShotgunFan30,(n+.5f)/ShotgunPellets30)+UnityEngine.Random.Range(-.3f,.3f);
    var ray=Quaternion.Euler(0,spread,0)*direction.normalized;
    var line=new GameObject("SG12PelletTrace30").AddComponent<LineRenderer>();line.transform.SetParent(Map.Root,false);line.material=new Material(Shader.Find("Sprites/Default"));line.startColor=new Color(1,.87f,.54f,.85f);line.endColor=new Color(1,.91f,.65f,.95f);line.startWidth=.025f;line.endWidth=.013f;line.positionCount=2;line.SetPosition(0,origin);line.SetPosition(1,origin+ray*.035f);line.enabled=Vision==null||Vision.CanSee(origin);
    shots.Add(new Projectile{Owner=Player,Origin303=origin,Position=origin,Velocity=ray*p.ProjectileSpeed,Life=1.5f,RemainingRange=p.Range,Line=line,Shotgun303=volley,AmmoGrade=volley.AmmoGrade});
   }
  }
  void RecordShotgunHit303(ShotgunVolley303 volley,RaidActor actor){if(!volley.Hits.ContainsKey(actor))volley.Hits[actor]=0;volley.Hits[actor]++;if(volley==lastShotgunVolley303){LastShotgunTargets30[actor]=volley.Hits[actor];LastShotgunHits30++;}}
  void CompleteShotgunPellet303(ShotgunVolley303 volley){if(--volley.Remaining!=0)return;foreach(var pair in volley.Hits)ResolveShotgunImpact303(pair.Key,pair.Value,volley.Damage,volley.Penetration,volley.AmmoGrade);}
  public void ResolveShotgunTarget30(RaidActor target,int pellets){var profile=ModRules30.Profile(Equipped)??BlueWeapons30.Shotgun;ResolveShotgunImpact303(target,pellets,profile.Damage,profile.Penetration,RaidInventory.Grade(MagazineAmmo));}
  void ResolveShotgunImpact303(RaidActor target,int pellets,float damage,float penetration,int ammoGrade){
   if(target==null||target.Dead||pellets<=0)return;pellets=Mathf.Clamp(pellets,1,ShotgunPellets30);
   // All pellets from one shell are applied once per actor, so a breaking shell cannot spill into health.
   ApplyCombatHit(target,damage*pellets/ShotgunPellets30,penetration*pellets/ShotgunPellets30,0,ammoGrade);
   if(target!=Player){HitMarker=.13f;target.Alert=1;target.LastKnown=Player.transform.position;target.LastSeen=Time.time;if(target.Dead&&!IsTrainingDummy315(target)){Kills++;if(target.IsRebel)RebelDeath145(target);else MonsterDeath(target);}}
  }
 }
}
