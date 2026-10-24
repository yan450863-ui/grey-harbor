using System;using System.IO;using System.Collections;using System.Reflection;using UnityEngine;
namespace GreyHarbor {public sealed partial class RaidVerification {
 object Projectile345(){var list=(IList)typeof(RaidGame).GetField("shots",BindingFlags.Instance|BindingFlags.NonPublic).GetValue(G);Check(list.Count==1,"345 exactly one real projectile");return list[0];}
 T ShotField345<T>(object shot,string field)=>(T)shot.GetType().GetField(field).GetValue(shot);
 void PlayerArmor345(float points){G.Player.Dead=false;G.Player.Health=120;G.Armor=new ItemStack(ItemKind.BlueArmor,1);ArmorBalance.Ensure(G.Armor);G.Armor.ArmorPoints=points;G.Helmet=null;}
 IEnumerator RunEnemyBalance345(){
  Check(Application.version=="3.0.45"&&Path.GetFileName(G.SavePath)=="verification-stash-3.0.45.json","345 built version and isolated save");
  FreshCamp();G.HarborWalkthrough=true;G.enabled=false;yield return G.BeginTrainingLoad2721();G.enabled=false;G.Paused=false;G.InventoryOpen=false;CombatDisable275();
  var kinds=(MonsterKind[])Enum.GetValues(typeof(MonsterKind));float[] health={90,54,198,72,108,144,90},armor={20,20,70,20,20,50,20},damage={22,26,42,24,28,38,24};int[] rounds={7,5,18,6,8,13,7};
  for(int i=0;i<kinds.Length;i++)for(int level=1;level<=2;level++){
   var enemy=G.SpawnMonster(new Vector3(510+i,0,510),kinds[i],"QA345 "+kinds[i],level);float scale=level==2?1.25f:1;
   Check(CombatNear275(enemy.Health,health[i]*scale)&&CombatNear275(enemy.NpcArmorPoints,armor[i]*scale)&&enemy.Health==enemy.NpcMaxHealth&&enemy.NpcArmorPoints==enemy.NpcMaxArmor,"345 explicit spawn pools "+kinds[i]+" level="+level);
   Check(NpcCombat275.IndependentDamage(enemy)==damage[i]&&NpcCombat275.IndependentPenetration(enemy)==damage[i]&&NpcCombat275.AttackGrade345(enemy)==level-1,"345 independent monster attack and grade "+kinds[i]+" level="+level);
   int expected=level==1?rounds[i]:Mathf.CeilToInt(armor[i]*scale/10)+Mathf.CeilToInt(health[i]*scale/18),hits=0;
   while(!enemy.Dead&&hits<40){G.ApplyCombatHit(enemy,18,10,0,level-1);hits++;if(hits==Mathf.CeilToInt(armor[i]*scale/10))Check(enemy.NpcArmorPoints==0&&CombatNear275(enemy.Health,health[i]*scale),"345 breaking armor cannot spill into HP "+kinds[i]+" level="+level);}
   Check(enemy.Dead&&hits==expected,"345 actual same-grade AK kill rounds "+kinds[i]+" level="+level+" hits="+hits);CombatDiscard275(enemy);
  }
  Check(MonsterTuning.Speed(MonsterKind.Wanderer)==1.9f&&MonsterTuning.Speed(MonsterKind.Runner)==3.15f,"345 older pursuit speeds modestly increased");
  Check(MonsterTuning.Speed(MonsterKind.PaperbackCreeper)==4.2f&&MonsterTuning.Speed(MonsterKind.BallastHauler)==3.7f&&MonsterTuning.Speed(MonsterKind.Pipewhisker)==4.1f&&MonsterTuning.Windup(MonsterKind.PaperbackCreeper)==.28f&&MonsterTuning.Windup(MonsterKind.BallastHauler)==.3f&&MonsterTuning.Windup(MonsterKind.Pipewhisker)==.18f,"345 underground speed and windup retained");
  foreach(RebelKind kind in Enum.GetValues(typeof(RebelKind))){var enemy=G.SpawnRebel145(new Vector3(515,0,515),kind,"QA345 soldier");bool blue=NpcRules270.Level(kind)==2,heavy=NpcRules270.Heavy(kind);float h=heavy?(blue?162:108):(blue?108:90),a=heavy?(blue?50:40):(blue?40:20);Check(enemy.Health==h&&enemy.NpcArmorPoints==a&&NpcCombat275.AttackGrade345(enemy)==(blue?1:0),"345 soldier endurance retained and grade follows level "+kind);CombatDiscard275(enemy);}
  for(int level=1;level<=2;level++){
   var enemy=G.SpawnMonster(new Vector3(500,0,501),MonsterKind.Wanderer,"QA345 melee",level);enemy.Alert=1;enemy.Memory=7;enemy.AttackCooldown=0;G.Player.transform.position=new Vector3(500,0,500);PlayerArmor345(120);Physics.SyncTransforms();
   G.TickMonster(enemy,.01f);Check(enemy.Windup>0&&G.TotalArmor==120,"345 melee telegraph causes no damage level="+level);G.TickMonster(enemy,.6f);float multiplier=level==2?1:.8f;
   Check(CombatNear275(G.TotalArmor,120-22*multiplier)&&G.Player.Health==120,"345 actual melee applies level quality to blue armor level="+level);
   PlayerArmor345(0);enemy.AttackDirection=Vector3.back;Call314(G,"MonsterHit",enemy,1.35f);Check(CombatNear275(G.Player.Health,120-22*multiplier),"345 broken armor health multiplier follows melee grade level="+level);CombatDiscard275(enemy);
   foreach(bool broken in new[]{false,true}){
    G.ClearShots();enemy=G.SpawnMonster(new Vector3(500,0,503),MonsterKind.Spitter,"QA345 spit",level);enemy.AttackDirection=Vector3.back;PlayerArmor345(broken?0:120);G.SpawnSpit(enemy);var shot=Projectile345();var origin=ShotField345<Vector3>(shot,"Position");
    Check(ShotField345<int>(shot,"AmmoGrade")==level-1&&ShotField345<float>(shot,"Penetration")==24,"345 real spit captures grade and independent penetration level="+level);
    G.Player.transform.position=new Vector3(origin.x,0,origin.z-2);enemy.NpcLevel=level==2?1:2;Physics.SyncTransforms();G.TickProjectiles(.4f);
    Check(G.ActiveSpits==0&&CombatNear275(G.TotalArmor,broken?0:120-24*multiplier)&&CombatNear275(G.Player.Health,broken?120-24*multiplier:120),"345 real spit hit retains launch grade through level change level="+level+" broken="+broken);G.ClearShots();CombatDiscard275(enemy);
   }
   foreach(bool broken in new[]{false,true}){
    G.ClearShots();enemy=G.SpawnRebel145(new Vector3(500,0,504),level==2?RebelKind.BlueRegular:RebelKind.Regular,"QA345 shot");enemy.Aim=Vector3.back;enemy.NpcAmmo=level==2?ItemKind.GreenAmmo:ItemKind.Ammo;PlayerArmor345(broken?0:120);G.SpawnShot(enemy,Vector3.back);var shot=Projectile345();var origin=ShotField345<Vector3>(shot,"Position");
    Check(ShotField345<int>(shot,"AmmoGrade")==level-1&&ShotField345<float>(shot,"Penetration")==10,"345 soldier shot uses enemy level despite stale carried ammunition level="+level);
    G.Player.transform.position=new Vector3(origin.x,0,origin.z-3);enemy.NpcLevel=level==2?1:2;Physics.SyncTransforms();G.TickProjectiles(.2f);
    Check(G.ActiveGunShots145==0&&CombatNear275(G.TotalArmor,broken?0:120-10*multiplier)&&CombatNear275(G.Player.Health,broken?120-14*multiplier:120),"345 real soldier projectile uses captured attack grade level="+level+" broken="+broken);G.ClearShots();CombatDiscard275(enemy);
   }
  }
  G.Equipped=new ItemStack(ItemKind.AK,1);var target=G.SpawnMonster(new Vector3(500,0,503),MonsterKind.Wanderer,"QA345 player regression",2);target.NpcArmorPoints=20;CombatPlayerShot275(target,ItemKind.GreenAmmo);Check(CombatNear275(target.NpcArmorPoints,12),"345 player green ammunition keeps actual loaded quality");target.NpcArmorPoints=20;CombatPlayerShot275(target,ItemKind.Ammo);Check(CombatNear275(target.NpcArmorPoints,10),"345 player blue ammunition keeps actual loaded quality");CombatDiscard275(target);
  G.Finish(false);File.WriteAllLines(Path.Combine(folder,"enemy-balance-345.txt"),results);Application.Quit(0);
 }
}}
