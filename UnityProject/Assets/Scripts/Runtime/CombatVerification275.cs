using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace GreyHarbor {
 public sealed partial class RaidVerification {
  static bool CombatNear275(float a,float b){return Mathf.Abs(a-b)<.015f;}
  void CombatValue275(float actual,float expected,string label){Check(CombatNear275(actual,expected),"275 "+label+" actual="+actual+" expected="+expected);}
  ItemStack CombatArmor275(float points){var a=new ItemStack(ItemKind.BlueArmor,1);ArmorBalance.Ensure(a);a.ArmorPoints=points;return a;}
  void CombatPools275(){
   Check(WeaponRules270.Damage(ItemKind.AK)==18&&WeaponRules270.Penetration(ItemKind.AK)==10&&ArmorBalance.AKDamage==18&&ArmorBalance.AKPenetration==10,"275 Player AK is 18 damage / 10 penetration");
   foreach(int grade in new[]{0,1,2,3}){
    float m=grade==0?.5f:grade==1?.8f:1;float armor=20;
    float health=ArmorBalance.HitPool146(ref armor,18,10,grade,2);
    Check(health==0&&CombatNear275(armor,20-10*m),"275 Pool ammo grade "+grade+" controls armor penetration against grade 2");
    armor=1;health=ArmorBalance.HitPool146(ref armor,18,10,grade,2);
    Check(armor==0&&health==0,"275 Armor-breaking round absorbs all HP damage, grade "+grade);
    CombatValue275(ArmorBalance.HitPool146(ref armor,18,10,grade,2),18*m,"Broken pool retains defense grade "+grade);
   }
   foreach(int weaponGrade in new[]{0,3}){
    var legacy=CombatArmor275(20);var current=CombatArmor275(20);
    Check(ArmorBalance.Hit(legacy,18,10,weaponGrade,0)==ArmorBalance.Hit(current,18,10,0)&&CombatNear275(legacy.ArmorPoints,current.ArmorPoints),"275 Legacy weapon grade ignored for equipped armor "+weaponGrade);
    float oldPool=20,newPool=20;
    Check(ArmorBalance.HitPool146(ref oldPool,18,10,weaponGrade,0,1)==ArmorBalance.HitPool146(ref newPool,18,10,0,1)&&CombatNear275(oldPool,newPool),"275 Legacy weapon grade ignored for pooled armor "+weaponGrade);
    legacy.ArmorPoints=0;CombatValue275(ArmorBalance.Hit(legacy,18,10,weaponGrade,0),14.4f,"Broken blue armor still reduces green HP damage, legacy weapon "+weaponGrade);
   }
   CombatValue275(ArmorBalance.Hit(null,18,10,0),18,"No equipped armor leaves full HP damage");
  }
  void CombatDisable275(){
   G.ClearShots();foreach(var e in G.Enemies)if(e&&e.HitCollider)e.HitCollider.enabled=false;
   if(G.ArmorDummy&&G.ArmorDummy.HitCollider)G.ArmorDummy.HitCollider.enabled=false;
   G.Player.Dead=false;G.Player.Health=100;G.Player.HitCollider.enabled=true;
   G.Player.transform.position=new Vector3(500,0,500);G.Player.Aim=Vector3.forward;G.Rig.Tick(0);Physics.SyncTransforms();
  }
  void CombatDiscard275(RaidActor e){G.Enemies.Remove(e);e.gameObject.SetActive(false);Destroy(e.gameObject);}
  // The empty remote firing lane isolates projectile handling from unrelated map geometry.
  void CombatPlayerShot275(RaidActor target,ItemKind ammo,bool changeInFlight=false){
   G.ClearShots();target.HitCollider.enabled=!target.Dead;G.Player.transform.position=new Vector3(500,0,500);G.Player.Aim=Vector3.forward;
   G.Rig.Tick(0);G.Player.Draw(G.View,0);Physics.SyncTransforms();G.MagazineAmmo=ammo;G.SpawnShot(G.Player,Vector3.forward);
   var muzzle=G.Player.MuzzleOnPlane(G.View);target.transform.position=new Vector3(muzzle.x,0,muzzle.z+3);Physics.SyncTransforms();
   if(changeInFlight)G.MagazineAmmo=ItemKind.Ammo;G.TickProjectiles(.2f);G.ClearShots();
  }
  void CombatNpcShot275(RaidActor shooter,bool changeInFlight=false){
   G.ClearShots();shooter.transform.position=new Vector3(500,0,504);shooter.Aim=Vector3.back;
   G.Player.transform.position=new Vector3(500,0,500);G.Rig.Tick(0);shooter.Draw(G.View,0);Physics.SyncTransforms();G.SpawnShot(shooter,Vector3.back);
   var muzzle=shooter.MuzzleOnPlane(G.View);G.Player.transform.position=new Vector3(muzzle.x,0,muzzle.z-3);Physics.SyncTransforms();
   if(changeInFlight)shooter.NpcAmmo=ItemKind.Ammo;G.TickProjectiles(.2f);G.ClearShots();
  }
  void CombatBudget275(RaidActor e,float budget,int ammoGrade,List<string> rows){
   int baseline=Mathf.CeilToInt(budget/18),armorRounds=Mathf.FloorToInt(baseline*.4f);float expectedArmor=armorRounds*10,expectedHP=budget-armorRounds*18;
   Check(CombatNear275(e.NpcArmorPoints,expectedArmor)&&CombatNear275(e.NpcMaxArmor,expectedArmor)&&CombatNear275(e.Health,expectedHP)&&CombatNear275(e.NpcMaxHealth,expectedHP),"275 Actual spawn splits original HP budget "+e.name+" level="+e.NpcLevel+" budget="+budget);
   Check(NpcCombat275.BaselineHits(budget)==baseline&&NpcCombat275.ArmorHits(budget)==armorRounds&&CombatNear275(NpcCombat275.ArmorForBudget(budget),expectedArmor)&&CombatNear275(NpcCombat275.HealthForBudget(budget),expectedHP),"275 NPC reference budget APIs match independent expectation "+e.name);
   int defense=e.NpcLevel-1,gap=defense-ammoGrade;float m=gap<=0?1:gap==1?.8f:.5f;
   int breakHits=Mathf.CeilToInt(expectedArmor/(10*m)),hpHits=Mathf.CeilToInt(expectedHP/(18*m)),total=breakHits+hpHits;
   for(int hit=1;hit<=total;hit++){
    G.ApplyCombatHit(e,18,10,hit%2==0?0:3,ammoGrade);
    float armor=Mathf.Max(0,expectedArmor-hit*10*m),hp=hit<=breakHits?expectedHP:Mathf.Max(0,expectedHP-(hit-breakHits)*18*m);
    Check(CombatNear275(e.NpcArmorPoints,armor)&&CombatNear275(e.Health,hp)&&e.Dead==(hit==total),"275 Actual NPC armor-first hit "+e.name+" ammo="+ammoGrade+" hit="+hit+"/"+total+" armor="+e.NpcArmorPoints+" HP="+e.Health);
   }
   Check(CombatNear275(e.NpcMaxArmor,expectedArmor)&&CombatNear275(e.NpcMaxHealth,expectedHP),"275 Hits preserve maximum pools "+e.name);
   if(ammoGrade>=defense)Check(total==baseline,"275 Same or higher grade keeps original kill-round budget "+e.name+" hits="+total);
   rows.Add(e.name+","+e.NpcLevel+","+budget+","+expectedArmor+","+expectedHP+","+ammoGrade+","+breakHits+","+hpHits+","+total);CombatDiscard275(e);
  }
  void CombatSpawns275(List<string> rows){
   var rebels=new[]{RebelKind.Regular,RebelKind.Heavy,RebelKind.BlueRegular,RebelKind.BlueHeavy};var budgets=new[]{196f,252f,245f,315f};
   for(int i=0;i<rebels.Length;i++)foreach(int ammo in new[]{0,1}){
    var e=G.SpawnRebel145(new Vector3(510+i,0,510),rebels[i],"275 budget");e.HitCollider.enabled=false;
    Check(e.NpcLevel==(i<2?1:2)&&e.NpcAmmo==(i<2?ItemKind.GreenAmmo:ItemKind.Ammo),"275 Soldier spawn assigns defense and independent ammo "+rebels[i]);CombatBudget275(e,budgets[i],ammo,rows);
   }
   var kinds=new[]{MonsterKind.Runner,MonsterKind.Wanderer,MonsterKind.Spitter,MonsterKind.Shell};var bases=new[]{100f,150f,120f,360f};
   for(int i=0;i<kinds.Length;i++)for(int level=1;level<=2;level++)foreach(int ammo in new[]{0,1}){
    var e=G.SpawnMonster(new Vector3(520+i,0,510),kinds[i],"275 "+kinds[i],level);e.HitCollider.enabled=false;
    Check(e.NpcLevel==level,"275 Monster spawn assigns defense level "+kinds[i]+" "+level);CombatBudget275(e,bases[i]*(level==2?1.25f:1),ammo,rows);
   }
  }
  void CombatProjectile275(){
   CombatDisable275();G.Equipped=new ItemStack(ItemKind.AK,1);
   var target=G.SpawnRebel145(new Vector3(500,0,504),RebelKind.BlueRegular,"275 projectile target");float armor=target.NpcArmorPoints,hp=target.Health;
   CombatPlayerShot275(target,ItemKind.GreenAmmo,true);
   Check(CombatNear275(target.NpcArmorPoints,armor-8)&&CombatNear275(target.Health,hp),"275 Real AK projectile captures green ammo at launch and removes 8 blue armor");
   target.NpcArmorPoints=1;CombatPlayerShot275(target,ItemKind.GreenAmmo);
   Check(target.NpcArmorPoints==0&&CombatNear275(target.Health,hp),"275 Real armor-breaking AK bullet has no HP overflow");
   CombatPlayerShot275(target,ItemKind.GreenAmmo);CombatValue275(target.Health,hp-14.4f,"Real green AK hits exposed blue NPC HP");
   CombatPlayerShot275(target,ItemKind.Ammo);CombatValue275(target.Health,hp-32.4f,"Real blue AK hits exposed blue NPC HP");CombatDiscard275(target);
   var shooter=G.SpawnRebel145(new Vector3(500,0,504),RebelKind.Regular,"275 independent soldier");shooter.HitCollider.enabled=false;
   foreach(var cosmetic in new[]{ItemKind.AK,ItemKind.Cola}){
    shooter.NpcWeapon=cosmetic;shooter.NpcAmmo=ItemKind.GreenAmmo;G.Armor=null;G.Player.Health=100;CombatNpcShot275(shooter);
    CombatValue275(G.Player.Health,86,"Actual soldier attack remains 14 with cosmetic "+cosmetic);
   }
   shooter.NpcAmmo=ItemKind.GreenAmmo;G.Armor=CombatArmor275(15);G.Player.Health=100;CombatNpcShot275(shooter,true);
   Check(CombatNear275(G.TotalArmor,7)&&G.Player.Health==100,"275 Real soldier projectile captures green ammo and uses independent 10 penetration");
   shooter.NpcAmmo=ItemKind.GreenAmmo;G.Armor=CombatArmor275(0);G.Player.Health=100;CombatNpcShot275(shooter);
   CombatValue275(G.Player.Health,88.8f,"Actual green soldier attacks broken blue armor HP at 14 x .8");
   shooter.NpcAmmo=ItemKind.Ammo;G.Player.Health=100;CombatNpcShot275(shooter);CombatValue275(G.Player.Health,86,"Actual blue soldier attacks same-grade exposed HP at 14");CombatDiscard275(shooter);
   var dummy=G.ArmorDummy;Check(dummy!=null,"275 Training includes independent armor dummy");dummy.TestArmor=CombatArmor275(1);dummy.Health=100;dummy.Dead=false;int dummyHits=G.DummyHits;
   CombatPlayerShot275(dummy,ItemKind.GreenAmmo);Check(dummy.TestArmor.ArmorPoints==0&&dummy.Health==100&&dummy.NpcArmorPoints==0,"275 Dummy uses TestArmor and absorbs break round without NPC pool");
   CombatPlayerShot275(dummy,ItemKind.GreenAmmo);Check(CombatNear275(dummy.Health,85.6f)&&G.DummyHits==dummyHits+2,"275 Dummy exposed HP uses only ammo grade");dummy.HitCollider.enabled=false;
  }
  void CombatMonsterAttacks275(){
   var kinds=new[]{MonsterKind.Runner,MonsterKind.Wanderer,MonsterKind.Spitter,MonsterKind.Shell};var damages=new[]{22f,18f,20f,35f};
   for(int i=0;i<kinds.Length;i++)for(int level=1;level<=2;level++){
    CombatDisable275();var e=G.SpawnMonster(new Vector3(500,0,501),kinds[i],"275 attack "+kinds[i],level);e.HitCollider.enabled=false;
    Check(NpcCombat275.IndependentDamage(e)==damages[i]&&NpcCombat275.IndependentPenetration(e)==damages[i],"275 Monster independent damage and penetration "+kinds[i]+" level "+level);
    G.Armor=CombatArmor275(0);G.Player.Health=100;e.AttackDirection=e.Aim=Vector3.back;e.Windup=.01f;e.Lunge=0;e.LungeHit=false;Physics.SyncTransforms();
    G.TickMonster(e,.02f);if(kinds[i]==MonsterKind.Runner)G.TickMonster(e,.01f);if(kinds[i]==MonsterKind.Spitter){Check(G.ActiveSpits==1,"275 Spitter launches actual projectile level "+level);G.TickProjectiles(.3f);}
    CombatValue275(G.Player.Health,100-damages[i]*.8f,"Actual monster attack retains grade 0 against broken blue armor "+kinds[i]+" level "+level);
    G.ClearShots();CombatDiscard275(e);
   }
  }
  void CombatCorpse275(RaidActor victim,int total){
   CombatDisable275();int drops=G.Map.Loot.Count,kills=G.Kills;G.Equipped=new ItemStack(ItemKind.AK,1);
   for(int hit=1;hit<=total;hit++){CombatPlayerShot275(victim,ItemKind.GreenAmmo);Check(victim.Dead==(hit==total),"275 Real AK kill budget "+victim.name+" hit "+hit+"/"+total);}
   Check(victim.Dead&&!victim.HitCollider.enabled&&G.Map.Loot.Count==drops+1&&G.Kills==kills+1,"275 Actual projectile death creates exactly one corpse and kill "+victim.name);
   var corpse=G.Map.Loot[drops];Check(corpse.Items.Count>0&&corpse.Items.TrueForAll(p=>!p.Identified),"275 Actual corpse starts searchable with unknown items "+victim.name);
   for(int i=0;i<3;i++)CombatPlayerShot275(victim,ItemKind.GreenAmmo);
   Check(G.Map.Loot.Count==drops+1&&G.Kills==kills+1,"275 Further actual shots cannot duplicate dead NPC corpse "+victim.name);CombatDiscard275(victim);
  }
  void CombatRepair275(){
   CombatDisable275();G.Bag.Slots.Clear();G.Chest.Slots.Clear();G.Bag.Add(ItemKind.GreenArmorKit,2);G.Armor=CombatArmor275(0);G.Player.Health=100;G.ReloadTime=G.HealTime=0;G.CancelArmorRepair();
   G.Paused=false;var kit=G.Bag.Slots.Find(p=>p.Kind==ItemKind.GreenArmorKit);Check(G.BeginArmorRepair(ItemArea.Backpack,kit.Cell),"275 Broken blue armor can begin actual repair");
   G.TickArmorRepair(3.99f);Check(G.TotalArmor==0&&G.Bag.Count(ItemKind.GreenArmorKit)==2,"275 Repair has no early restoration or consumption");
   G.TickArmorRepair(.02f);Check(CombatNear275(G.TotalArmor,24)&&G.Bag.Count(ItemKind.GreenArmorKit)==1,"275 Completed green kit repairs blue armor by 30 x .8 and consumes once");
   kit=G.Bag.Slots.Find(p=>p.Kind==ItemKind.GreenArmorKit);Check(G.BeginArmorRepair(ItemArea.Backpack,kit.Cell),"275 Begin repair to test real bullet cancellation");
   var shooter=G.SpawnRebel145(new Vector3(500,0,504),RebelKind.Regular,"275 repair interruption");shooter.HitCollider.enabled=false;CombatNpcShot275(shooter);
   Check(G.RepairTime==0&&CombatNear275(G.TotalArmor,16)&&G.Player.Health==100,"275 Absorbed real green soldier bullet cancels repair and removes 8 armor");
   G.TickArmorRepair(5);Check(CombatNear275(G.TotalArmor,16)&&G.Bag.Count(ItemKind.GreenArmorKit)==1,"275 Interrupted repair cannot restore or consume later");CombatDiscard275(shooter);
   G.Bag.Slots.Clear();G.Bag.Add(ItemKind.ArmorKit,1);G.Armor.ArmorPoints=115;kit=G.Bag.Slots[0];Check(G.BeginArmorRepair(ItemArea.Backpack,kit.Cell),"275 Same-grade repair starts near armor maximum");G.TickArmorRepair(4.01f);
   Check(G.TotalArmor==120&&G.Bag.Count(ItemKind.ArmorKit)==0,"275 Repair caps at original blue armor maximum and consumes once");G.Paused=true;
  }
  IEnumerator CombatCache275(){
   G.HarborWalkthrough=true;Check(G.BeginTraining(),"275 Start isolated actual harbor preview");G.Paused=true;yield return null;
   G.SwitchInterior("repair-entry:"+RaidMap.RepairMainGateKeys220[1]);G.Paused=true;yield return null;
   Check(G.Map.RepairMain220&&G.Map.RepairFloor220==1&&G.Enemies.Count>=2,"275 Cache probe uses actual main floor actors");var map=G.Map;var exposed=G.Enemies[0];var armored=G.Enemies[1];
   int grade=exposed.NpcLevel-1;int armorHits=Mathf.CeilToInt(exposed.NpcArmorPoints/10);
   for(int i=0;i<armorHits;i++)G.ApplyCombatHit(exposed,18,10,3,grade);G.ApplyCombatHit(exposed,18,10,0,grade);
   G.ApplyCombatHit(armored,18,10,3,armored.NpcLevel-1);float hp=exposed.Health,remaining=armored.NpcArmorPoints,otherHP=armored.Health,maxA=exposed.NpcMaxArmor,maxHP=exposed.NpcMaxHealth;
   Check(exposed.NpcArmorPoints==0&&hp>18&&!exposed.Dead&&remaining>0,"275 Cache probe holds both exposed wounded HP and partly damaged armor");
   for(int trip=0;trip<2;trip++){
    G.SwitchInterior("repair-floor:2:north");G.Paused=true;yield return null;Check(G.Map.RepairFloor220==2,"275 Cache leaves first floor "+trip);
    G.SwitchInterior("repair-floor:1:south");G.Paused=true;yield return null;
    Check(G.Map==map&&G.Enemies.Contains(exposed)&&G.Enemies.Contains(armored)&&exposed.NpcArmorPoints==0&&CombatNear275(exposed.Health,hp)&&CombatNear275(armored.NpcArmorPoints,remaining)&&CombatNear275(armored.Health,otherHP)&&CombatNear275(exposed.NpcMaxArmor,maxA)&&CombatNear275(exposed.NpcMaxHealth,maxHP),"275 Actual floor cache preserves actor identity and both damaged pools trip "+trip);
   }
   G.ApplyCombatHit(exposed,18,10,3,grade);Check(exposed.NpcArmorPoints==0&&CombatNear275(exposed.Health,hp-18),"275 Next hit after cache continues exposed HP without armor refill");
   G.Finish(false);yield return null;
  }
  IEnumerator VerifyCombat275(){
   string directory=Path.GetDirectoryName(G.SavePath);string[] formal={Path.Combine(directory,"stash-v40.json"),Path.Combine(directory,"stash-v41.json")};byte[][] before=Array.ConvertAll(formal,p=>File.Exists(p)?File.ReadAllBytes(p):null);
   Check(Path.GetFileName(G.SavePath)=="verification-stash-2.7.5.json","275 Combat verification uses isolated inventory");CombatPools275();FreshCamp();string camp=JsonUtility.ToJson(G.Save);G.HardMode=false;G.HarborWalkthrough=false;
   Check(G.BeginTraining(),"275 Start isolated combat training");G.Paused=true;yield return null;CombatDisable275();
   var rows=new List<string>{"actor,level,originalBudget,maxArmor,maxHealth,ammoGrade,armorRounds,healthRounds,totalRounds"};CombatSpawns275(rows);File.WriteAllLines(Path.Combine(folder,"combat-275-npc-hit-budgets.csv"),rows);
   CombatProjectile275();CombatMonsterAttacks275();CombatCorpse275(G.SpawnRebel145(new Vector3(500,0,504),RebelKind.Regular,"275 corpse soldier"),11);CombatCorpse275(G.SpawnMonster(new Vector3(500,0,504),MonsterKind.Runner,"275 corpse runner"),6);CombatRepair275();
   G.Controls=new DesktopInput();G.Finish(false);yield return null;Check(JsonUtility.ToJson(G.Save)==camp,"275 Combat preview restores exact camp inventory");
   yield return CombatCache275();Check(JsonUtility.ToJson(G.Save)==camp,"275 Cache preview restores exact camp inventory");
   for(int i=0;i<formal.Length;i++)Check(MainBytes273(before[i],File.Exists(formal[i])?File.ReadAllBytes(formal[i]):null),"275 Formal save stays byte-for-byte unchanged "+Path.GetFileName(formal[i]));
  }
 }
}
