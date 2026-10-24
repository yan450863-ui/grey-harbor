using UnityEngine;
namespace GreyHarbor {
 public sealed partial class RaidGame {
  AudioClip spitCue;
  void PlaySpitCue(RaidActor e){if(!spitCue)spitCue=Sound(185,.32f,.22f);audioPlayer.PlayOneShot(spitCue,Mathf.Clamp01(1-Vector3.Distance(e.transform.position,Player.transform.position)/SoundRules326.HearingRange327));}
  internal void SpawnSpit(RaidActor actor){
   if(actor.Dead||!actor.gameObject.activeInHierarchy)return;
   Vector3 direction=actor.AttackDirection.normalized;if(direction.sqrMagnitude<.5f)return;
   Vector3 chest=actor.transform.position+Vector3.up*CombatHeight,origin=chest+direction*.38f;
   if(Physics.Linecast(chest,origin,SceneObstacle.SightMask,QueryTriggerInteraction.Ignore))return;
   var line=new GameObject("CorrosiveSpit").AddComponent<LineRenderer>();line.transform.SetParent(Map.Root,false);
   line.material=new Material(Shader.Find("Sprites/Default"));line.startColor=PixelArt.Hex("DAE37A");line.endColor=PixelArt.Hex("6E873C");
   line.startWidth=.19f;line.endWidth=.095f;line.numCapVertices=3;line.positionCount=2;line.SetPosition(0,origin);line.SetPosition(1,origin-direction*.28f);
   shots.Add(new Projectile{Owner=actor,Position=origin,Velocity=direction*7,Life=1.7f,Line=line,Spit=true,Damage=NpcCombat275.IndependentDamage(actor),Penetration=NpcCombat275.IndependentPenetration(actor),AmmoGrade=NpcCombat275.AttackGrade345(actor)});
  }
  internal int ActiveSpits=>shots.FindAll(s=>s.Spit).Count;
  internal Vector3 FirstSpitDirection=>shots.Find(s=>s.Spit)?.Velocity.normalized??Vector3.zero;
  internal void ClearShots(){ClearShotgunVolley303();foreach(var b in shots)if(b.Line){Destroy(b.Line.material);Destroy(b.Line.gameObject);}shots.Clear();}
 }
}
