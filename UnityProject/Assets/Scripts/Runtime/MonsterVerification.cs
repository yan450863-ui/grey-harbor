using System;
using System.Collections;
using UnityEngine;
namespace GreyHarbor { public sealed partial class RaidVerification {
 void ResetMonster118(RaidActor e,Vector3 p){e.transform.position=p;e.Dead=false;e.HitCollider.enabled=true;e.ConfigureMonster(e.Monster);e.Windup=e.Lunge=e.AttackPose=e.AttackCooldown=0;e.Alert=1;e.Memory=7;e.Route.Clear();e.AttackDirection=Vector3.back;e.Aim=Vector3.back;e.WaitTime=0;}
 IEnumerator VerifyMonsters118(){
  FreshCamp();G.NextMap=RaidDestination.Training;G.StartRaid();G.Paused=true;yield return null;
  string trainingStock=JsonUtility.ToJson(G.Save);G.Armor=G.Helmet=null;
  Check(G.Enemies.Count==4,"Training has one of each of the four species");
  bool clear=true,patrol=true,safe=true;foreach(var e in G.Enemies){
   clear&=!Physics.CheckCapsule(e.transform.position+Vector3.up*.35f,e.transform.position+Vector3.up,.34f,SceneObstacle.MovementMask);
   safe&=Vector3.Distance(e.transform.position,G.Map.Spawn)>9;
   foreach(var p in e.PatrolTargets)patrol&=G.Navigation.FindPath(e.Home,p).Count>0;
  }
  Check(clear&&safe,"Four training spawns are clear and outside arrival detection range");Check(patrol,"All training patrol legs have valid swept navigation paths");
  foreach(var e in G.Enemies){e.Alert=e.Memory=0;e.WaitTime=0;e.PatrolIndex=1;Vector3 p=e.transform.position;for(int i=0;i<80;i++){G.HarborPatrol(e,.05f);Physics.SyncTransforms();}Check(Vector3.Distance(p,e.transform.position)>.5f,"Actual patrol advances without getting stuck: "+e.Monster);e.transform.position=e.Home;}
  var subjects=G.Enemies.ToArray();G.Player.transform.position=new Vector3(0,0,-13);
  foreach(var e in subjects){
   var idle=MonsterArt.Get(e.Monster,0);var attack=MonsterArt.Get(e.Monster,4);
   Check(idle&&attack&&idle!=attack&&idle.texture.filterMode==FilterMode.Point&&idle.bounds.size.y>.85f,"Distinct transparent idle/windup art and pixel sampling: "+e.Monster);
   Check(e.Health==MonsterTuning.Health(e.Monster),"Level-one health baseline: "+e.Monster);
   ResetMonster118(e,new Vector3(0,0,-12));G.Player.Health=100;Physics.SyncTransforms();G.TickMonster(e,.01f);
   Check(e.Windup>0&&G.Player.Health==100,"Attack begins with harmless windup: "+e.Monster);
   G.TickMonster(e,MonsterTuning.Windup(e.Monster)+.01f);
   if(e.Monster==MonsterKind.Runner)G.TickMonster(e,.01f);
   if(e.Monster==MonsterKind.Spitter){Check(G.Player.Health==100&&G.ActiveSpits==1,"Spitter releases a traveling projectile instead of instant damage");G.TickProjectiles(.2f);}
   Check(Mathf.Abs(G.Player.Health-(100-MonsterTuning.Damage(e.Monster)))<.01f,"Attack deals configured level-one damage: "+e.Monster);
   float health=G.Player.Health;G.TickMonster(e,.02f);G.TickProjectiles(.02f);Check(G.Player.Health==health,"Recovery prevents immediate double-hit: "+e.Monster);
   e.transform.position=e.Home;e.Alert=e.Memory=e.Windup=e.Lunge=0;
  }
  var spit=Array.Find(subjects,e=>e.Monster==MonsterKind.Spitter);
  ResetMonster118(spit,new Vector3(0,0,-8));G.Player.transform.position=new Vector3(0,0,-13);G.Player.Health=100;Physics.SyncTransforms();G.TickMonster(spit,.01f);Vector3 aim=spit.AttackDirection,windupPosition=spit.transform.position;
  G.Player.transform.position=new Vector3(3,0,-13);Physics.SyncTransforms();G.TickMonster(spit,.81f);
  Check(G.ActiveSpits==1&&Vector3.Dot(aim,G.FirstSpitDirection)>.999f&&spit.transform.position==windupPosition,"Spitter stays still and fires at windup-locked position");
  for(int i=0;i<40;i++)G.TickProjectiles(.05f);Check(G.Player.Health==100&&G.ActiveSpits==0,"Sidestep dodges spit and expired projectiles clean up");
  ResetMonster118(spit,new Vector3(0,0,-8));G.Player.transform.position=new Vector3(0,0,-13);spit.AttackCooldown=2.5f;var hold=spit.transform.position;G.TickMonster(spit,.5f);Check(spit.transform.position==hold&&spit.Windup==0,"Spitter holds range without backward kiting or bypassing cooldown");
  // The same actual projectile must cross a low object and stop at a high object.
  var block=GameObject.CreatePrimitive(PrimitiveType.Cube);block.name="QA_SpitObstacle";block.transform.SetParent(G.Map.Root);block.transform.position=new Vector3(0,.8f,-10.5f);block.transform.localScale=new Vector3(2,1.6f,.3f);
  SceneObstacle.Apply(block,ObstacleKind.High);Physics.SyncTransforms();spit.AttackDirection=Vector3.back;G.Player.Health=100;G.SpawnSpit(spit);G.TickProjectiles(1);Check(G.Player.Health==100&&G.ActiveSpits==0,"High obstacle blocks actual acid projectile");
  SceneObstacle.Apply(block,ObstacleKind.Low);Physics.SyncTransforms();G.HealTime=2;G.SpawnSpit(spit);G.TickProjectiles(1);Check(G.Player.Health==80&&G.HealTime==0,"Low obstacle permits spit; hit interrupts healing");block.SetActive(false);Destroy(block);
  // Real AK hits, kills and one searchable corpse for every species.
  foreach(var e in subjects){
   foreach(var other in subjects)if(other!=e)other.gameObject.SetActive(false);
   e.gameObject.SetActive(true);ResetMonster118(e,new Vector3(0,0,-8));G.Player.transform.position=new Vector3(0,0,-13);G.Player.Aim=Vector3.forward;G.Player.Draw(G.View,0);
   var muzzle=G.Player.MuzzleOnPlane(G.View);e.transform.position=new Vector3(muzzle.x,0,muzzle.z+2);Physics.SyncTransforms();int drops=G.Map.Loot.Count,hits=Mathf.CeilToInt(e.Health/ArmorBalance.AKDamage);
   for(int i=0;i<hits-1;i++){G.SpawnShot(G.Player,Vector3.forward);G.TickProjectiles(.1f);}Check(!e.Dead&&e.Health>0,"AK does not kill before expected hit count: "+e.Monster);
   G.SpawnShot(G.Player,Vector3.forward);G.TickProjectiles(.1f);Check(e.Dead&&!e.HitCollider.enabled&&G.Map.Loot.Count==drops+1,"Expected AK hit count creates one searchable corpse: "+e.Monster);
   var loot=G.Map.Loot[G.Map.Loot.Count-1];Check(loot.Items.Count>0&&loot.Items.TrueForAll(p=>p.RaidFound&&!p.Identified&&p.Origin==ItemOrigin.Certified),"Corpse contents use certified sequential search: "+e.Monster);
  }
  foreach(var e in subjects){e.gameObject.SetActive(true);ResetMonster118(e,e.Home);e.Alert=e.Memory=0;}
  // Live projectiles and pending attacks cannot cross independent maps.
  G.Player.Health=100;G.Player.transform.position=RaidMap.BoothApproach;spit.AttackDirection=Vector3.back;spit.Health=37;spit.Windup=.5f;G.SpawnSpit(spit);Vector3 outsidePosition=spit.transform.position;G.Rig.ResetView(135);G.Rig.Tick(0);
  G.SwitchBooth();Check(G.Map.IsInterior&&G.Enemies.Count==1&&G.ActiveSpits==0&&!spit.gameObject.activeInHierarchy,"Entering building clears shots and removes exterior enemies from active simulation");G.TickEnemies(10);Check(G.Player.Health==100&&spit.transform.position==outsidePosition,"Outdoor monsters cannot move or attack an indoor player");
  G.SwitchBooth();Check(G.Enemies.Count==4&&G.Enemies.Contains(spit)&&spit.Health==37&&spit.Windup==0&&Mathf.Abs(G.Rig.DestinationYaw-135)<.01f,"Returning preserves monster identity health position and camera, cancels pending windup");
  // Screenshot each production sprite at the normal gameplay zoom in all camera quadrants.
  foreach(var other in subjects)other.gameObject.SetActive(false);
  G.Player.transform.position=new Vector3(0,0,-12);G.Player.Draw(G.View,1);G.Rig.ResetView(45);G.Rig.Tick(0);
  for(int k=0;k<subjects.Length;k++){
   var e=subjects[k];e.gameObject.SetActive(true);ResetMonster118(e,new Vector3(0,0,-8.7f));e.Aim=Vector3.left;e.Draw(G.View,1);G.Player.Aim=Vector3.forward;G.Vision.Refresh(G.Player.transform.position,G.Player.Aim);yield return Capture("190-monster-"+e.Monster+"-idle.png");
   e.Windup=.4f;yield return Capture("191-monster-"+e.Monster+"-windup.png");e.Windup=0;e.gameObject.SetActive(false);
  }
  spit.gameObject.SetActive(true);spit.transform.position=new Vector3(0,0,-8.7f);spit.Aim=spit.AttackDirection=Vector3.back;spit.Draw(G.View,1);G.SpawnSpit(spit);G.TickProjectiles(.13f);yield return Capture("192-spit-flight.png");G.TickProjectiles(2);
  for(int q=0;q<4;q++){G.Rig.ResetView(45+90*q);G.Rig.Tick(0);G.Player.Aim=Vector3.forward;G.Vision.Refresh(G.Player.transform.position,G.Player.Aim);spit.Draw(G.View,0);Check(spit.Body.enabled&&spit.Body.material.shader.name=="GreyHarbor/Sprite","Monster keeps depth-aware rendering after view switch "+q);}
  G.Finish(false);Check(G.Phase==GamePhase.Base&&G.Enemies.Count==0&&G.ActiveSpits==0&&JsonUtility.ToJson(G.Save)==trainingStock,"Leaving monster training cleans actors/projectiles and restores formal inventory");
 }
}}
