using System.Collections.Generic;
using UnityEngine;
namespace GreyHarbor {
 public enum SoundKind326 { Footstep, Gunshot, Creature }
 public sealed class HeardSound326 {
  public SoundKind326 Kind;public Vector3 Direction;public float Distance,Remaining,Duration;
 }
 public sealed class SoundState326 {
  public readonly List<HeardSound326> Heard=new List<HeardSound326>();
  public float OwnNoise {get;private set;}
  float pulse,hold,movement,movementHold;
  public void Clear(){Heard.Clear();OwnNoise=pulse=hold=movement=movementHold=0;}
  public void Own(float strength){pulse=Mathf.Max(pulse,Mathf.Clamp01(strength));OwnNoise=Mathf.Max(OwnNoise,pulse);hold=.18f;}
  public void Moving(float strength){movement=Mathf.Clamp01(strength);movementHold=.12f;OwnNoise=Mathf.Max(OwnNoise,movement);}
  public void Tick(float dt){dt=Mathf.Max(0,dt);movementHold=Mathf.Max(0,movementHold-dt);hold=Mathf.Max(0,hold-dt);if(hold<=0)pulse=Mathf.MoveTowards(pulse,0,dt*.85f);OwnNoise=Mathf.MoveTowards(OwnNoise,Mathf.Max(pulse,movementHold>0?movement:0),dt*.65f);for(int i=Heard.Count-1;i>=0;i--){Heard[i].Remaining-=dt;if(Heard[i].Remaining<=0)Heard.RemoveAt(i);}}
  public void Hear(SoundKind326 kind,Vector3 delta,float radius){
   delta.y=0;float distance=delta.magnitude;if(distance>radius||distance<.05f)return;
   // Store a coarse bearing at the instant of hearing. No reference to a living enemy is retained.
   int bucket=(Mathf.RoundToInt(Mathf.Atan2(delta.z,delta.x)*Mathf.Rad2Deg/45)+8)%8;
   float angle=bucket*45*Mathf.Deg2Rad;Vector3 direction=new Vector3(Mathf.Cos(angle),0,Mathf.Sin(angle));
   float duration=kind==SoundKind326.Footstep?1.15f:kind==SoundKind326.Gunshot?2f:1.6f;
   foreach(var old in Heard)if(old.Kind==kind&&Vector3.Dot(old.Direction,direction)>.99f){old.Distance=distance;old.Remaining=old.Duration=duration;return;}
   if(Heard.Count>=24)Heard.RemoveAt(0);
   Heard.Add(new HeardSound326{Kind=kind,Direction=direction,Distance=distance,Remaining=duration,Duration=duration});
  }
 }
 public static class SoundRules326 {
  public static float HearingRange327=>RaidCamera.GameplaySize/Mathf.Sin(RaidCamera.ViewTilt310*Mathf.Deg2Rad);
  public static Color DistanceColor(float distance)=>distance<HearingRange327/3?new Color(1,.36f,.16f):distance<HearingRange327*2/3?new Color(1,.80f,.28f):new Color(.94f,.97f,1);
  public static Vector2 EdgePoint(MiniMapProjection projection,Vector3 direction,Rect frame,int kindIndex){
   Vector2 d=projection.Point(direction)-projection.Point(Vector3.zero);if(d.sqrMagnitude<.000001f)d=Vector2.up;d.Normalize();
   // Keep each glyph inside the terrain viewport, with a small tangent offset for different types.
   Vector2 half=frame.size*.5f-Vector2.one*12;
   float t=Mathf.Min(Mathf.Abs(d.x)<.00001f?float.PositiveInfinity:half.x/Mathf.Abs(d.x),Mathf.Abs(d.y)<.00001f?float.PositiveInfinity:half.y/Mathf.Abs(d.y));
   Vector2 point=frame.center+d*t+new Vector2(-d.y,d.x)*(Mathf.Clamp(kindIndex,0,2)-1)*12;
   point.x=Mathf.Clamp(point.x,frame.xMin+10,frame.xMax-10);point.y=Mathf.Clamp(point.y,frame.yMin+10,frame.yMax-10);return point;
  }
 }
 public sealed partial class RaidGame {
  public readonly SoundState326 Sound326=new SoundState326();
  RaidMap soundMap326;AudioClip stepClip326,creatureClip326;
  internal void TickSound326(float dt){
   if(soundMap326!=Map||Phase!=GamePhase.Raid){Sound326.Clear();soundMap326=Map;if(Player)Player.FootstepDistance326=0;foreach(var e in Enemies)if(e)e.FootstepDistance326=0;}
   if(Phase==GamePhase.Raid&&!Paused)Sound326.Tick(dt);
  }
  internal void ActorMoved326(RaidActor actor,float distance){
   if(distance<=.00001f||!CanSound326(actor))return;
   bool own=actor==Player;bool running=own&&Sprint2711.CanRun;
   if(own)Sound326.Moving(running?.55f:.25f);
   actor.FootstepDistance326+=distance;if(actor.FootstepDistance326<.80f)return;actor.FootstepDistance326%=.80f;
   EmitSound326(actor,SoundKind326.Footstep,own&&running?22:18);
  }
  bool CanSound326(RaidActor actor)=>Phase==GamePhase.Raid&&!Paused&&Player&&!Player.Dead&&actor&&!actor.Dead&&actor.gameObject.activeInHierarchy&&Map!=null&&Map.Root&&Map.Root.gameObject.activeInHierarchy;
  internal void EmitSound326(RaidActor actor,SoundKind326 kind,float radius=0){
   if(!CanSound326(actor))return;
   bool own=actor==Player;
   if(radius<=0)radius=kind==SoundKind326.Gunshot?(own?ModRules30.NoiseRange(Equipped):(actor.NpcWeapon==ItemKind.MP5||actor.NpcWeapon==ItemKind.Dagger?24:30)):kind==SoundKind326.Creature?20:18;
   if(own){if(kind==SoundKind326.Gunshot)Sound326.Own(Mathf.Clamp01(radius/18));}
   else Sound326.Hear(kind,actor.transform.position-Player.transform.position,Mathf.Min(radius,SoundRules326.HearingRange327));
   // Gunfire already has its actual shot audio. Footsteps and attack cues share this distance attenuation.
   if(kind==SoundKind326.Gunshot)return;
   float distance=own?0:Vector3.Distance(actor.transform.position,Player.transform.position);if(!own)radius=Mathf.Min(radius,SoundRules326.HearingRange327);if(distance>radius)return;
   if(kind==SoundKind326.Creature&&actor.Monster==MonsterKind.Spitter)return;
   if(kind==SoundKind326.Footstep){if(!stepClip326)stepClip326=Sound(90,.075f,.085f);audioPlayer.PlayOneShot(stepClip326,Mathf.Lerp(.12f,.65f,1-distance/radius));}
   else {if(!creatureClip326)creatureClip326=Sound(65,.24f,.16f);audioPlayer.PlayOneShot(creatureClip326,Mathf.Lerp(.15f,.75f,1-distance/radius));}
  }
 }
}
