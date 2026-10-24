using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace GreyHarbor {
 public sealed partial class RaidVerification {
  readonly List<string> cargoEnemyRows2716=new List<string>{"type,name,x,y,z,detail"};
  void CargoEnemyRow2716(string type,RaidActor actor,string detail){var p=actor.transform.position;cargoEnemyRows2716.Add(type+","+Csv230(actor.name)+","+WN279(p.x)+","+WN279(p.y)+","+WN279(p.z)+","+Csv230(detail));File.WriteAllLines(Path.Combine(folder,"2716-cargo-actual-enemies.csv"),cargoEnemyRows2716);}
  bool CargoEnemyClear2716(Vector3 p)=>G.Map.HarborSurfaceClear(p)&&!Physics.CheckCapsule(p+Vector3.up*.35f,p+Vector3.up,.34f,SceneObstacle.MovementMask,QueryTriggerInteraction.Ignore);
  void CargoEnemyRoster2716(){
   var map=G.Map;var area=RaidMap.CargoBounds2712;var actual=G.Enemies.FindAll(e=>area.Contains(new Vector2(e.Home.x,e.Home.z)));Check(actual.Count==7&&map.CargoEnemies2716.Count==7,"2716 Exactly seven real central monsters and seven current-map records");var unique=new HashSet<RaidActor>();int green=0,blue=0,wanderers=0,runners=0,spitters=0;
   foreach(var p in map.CargoEnemies2716){var e=p.Actor;Check(e&&unique.Add(e)&&actual.Contains(e)&&G.Enemies.Contains(e)&&e.gameObject.activeInHierarchy&&e.IsMonster&&!e.IsRebel&&!e.Dead&&e.name==p.Name&&e.Monster==p.Kind&&e.NpcLevel==p.Level&&Vector3.Distance(e.Home,p.Home)<.001f,"2716 Actual central actor matches authored monster identity "+p.Name);
    if(e.NpcLevel==1)green++;else if(e.NpcLevel==2)blue++;if(e.Monster==MonsterKind.Wanderer)wanderers++;if(e.Monster==MonsterKind.Runner)runners++;if(e.Monster==MonsterKind.Spitter)spitters++;
    float oldHP=e.Monster==MonsterKind.Wanderer?150:e.Monster==MonsterKind.Runner?100:120;float budget=oldHP*(e.NpcLevel==2?1.25f:1);int armorHits=Mathf.FloorToInt(Mathf.CeilToInt(budget/18)*.4f);float armor=armorHits*10,health=budget-armorHits*18;
    Check(Mathf.Abs(e.NpcArmorPoints-armor)<.001f&&Mathf.Abs(e.NpcMaxArmor-armor)<.001f&&Mathf.Abs(e.Health-health)<.001f&&Mathf.Abs(e.NpcMaxHealth-health)<.001f&&e.NpcAmmo==(e.NpcLevel==2?ItemKind.Ammo:ItemKind.GreenAmmo),"2716 Actual monster level preserves independent existing armor/health allocation "+e.name+" armor="+e.NpcArmorPoints+" HP="+e.Health);
    Check(CargoEnemyClear2716(e.Home)&&e.PatrolTargets!=null&&e.PatrolTargets.Length==3,"2716 Actual spawn has ground/capsule and three patrol targets "+e.name);float arrivalDistance=float.MaxValue;foreach(var t in map.CargoGroundTransitions2715)arrivalDistance=Mathf.Min(arrivalDistance,Vector3.Distance(e.Home,t.Outer));Check(arrivalDistance>=14,"2716 Authored monster spawn leaves natural road arrivals distant "+e.name+" nearest="+arrivalDistance);
    for(int i=0;i<e.PatrolTargets.Length;i++){var a=e.PatrolTargets[i];var b=e.PatrolTargets[(i+1)%e.PatrolTargets.Length];Check(Vector3.Distance(a,p.Targets[i])<.001f&&CargoEnemyClear2716(a)&&G.Navigation.ClearSegment(a,b),"2716 Actual patrol point and connecting segment have .34m swept clearance "+e.name+"/"+i);CargoEnemyRow2716("patrol-point",e,"index="+i+" target="+a+" next="+b+" actual swept clear");}
    CargoEnemyRow2716("spawn",e,"kind="+e.Monster+" level="+e.NpcLevel+" armor="+armor+" health="+health+" nearestOuter="+arrivalDistance);
   }
   Check(green==5&&blue==2&&wanderers==4&&runners==2&&spitters==1,"2716 Real central roster has five green/two blue and four wanderers/two runners/one spitter");Check(map.CargoEnemies2716.Find(p=>p.Kind==MonsterKind.Wanderer&&p.Level==2)!=null&&map.CargoEnemies2716.Find(p=>p.Kind==MonsterKind.Spitter&&p.Level==2)!=null,"2716 Two blue monsters are one wanderer and one spitter");Check(map.CargoBoxes2712.Count==64&&map.CargoLoot2714.Count==20&&map.CargoDressingRecords2714.Count==77,"2716 NPC addition preserves64 maze boxes20 cargo searches77 decoration groups");
  }
  void CargoEnemyPatrol2716(){
   var player=G.Player.transform.position;G.Player.transform.position=new Vector3(-1000,0,-1000);
   try{foreach(var p in G.Map.CargoEnemies2716){var e=p.Actor;var original=e.transform.position;float alert=e.Alert,memory=e.Memory;int originalIndex=e.PatrolIndex;float wait=e.WaitTime;float distance=0;bool collision=false;e.transform.position=e.Home;e.PatrolIndex=1;e.WaitTime=0;e.Alert=e.Memory=0;e.Route.Clear();
     try{for(int tick=0;tick<800;tick++){var before=e.transform.position;G.HarborPatrol(e,.025f);Physics.SyncTransforms();distance+=Vector3.Distance(before,e.transform.position);collision|=!CargoEnemyClear2716(e.transform.position);}CargoEnemyRow2716("actual-patrol",e,"20sec production HarborPatrol Actor.Move; distance="+distance+" reachedIndex="+e.PatrolIndex+" collision="+collision);Check(distance>5&&e.PatrolIndex>=3&&!collision,"2716 Real monster patrol traverses both authored endpoints without collision "+e.name+" moved="+distance+" index="+e.PatrolIndex+" collision="+collision);}
     finally{e.transform.position=original;e.Alert=alert;e.Memory=memory;e.PatrolIndex=originalIndex;e.WaitTime=wait;e.Route.Clear();Physics.SyncTransforms();}
    }}finally{G.Player.transform.position=player;Physics.SyncTransforms();}
  }
  void CargoEnemyShot2716(RaidActor target){
   G.ClearShots();G.Player.Aim=(target.transform.position-G.Player.transform.position).normalized;G.Rig.Tick(0);G.Player.Draw(G.View,0);Physics.SyncTransforms();var origin=G.Player.MuzzleOnPlane(G.View);var direction=target.transform.position+Vector3.up*RaidGame.CombatHeight-origin;direction.y=0;Check(direction.magnitude>.2f&&direction.magnitude<6,"2716 Actual cargo firing lane reaches intended monster "+target.name);G.SpawnShot(G.Player,direction.normalized);G.TickProjectiles(.2f);G.ClearShots();
  }
  void CargoEnemyDeath2716(CargoEnemy2716 p){
   var e=p.Actor;G.Player.transform.position=p.Targets[1];Check(CargoEnemyClear2716(G.Player.transform.position),"2716 Actual local player firing stand has .34m clearance "+e.name);int armorRounds=Mathf.CeilToInt(e.NpcArmorPoints/10),healthRounds=Mathf.CeilToInt(e.Health/18),total=armorRounds+healthRounds;float startArmor=e.NpcArmorPoints,startHP=e.Health;int loot=G.Map.Loot.Count,kills=G.Kills;G.MagazineAmmo=e.NpcLevel==2?ItemKind.Ammo:ItemKind.GreenAmmo;
   for(int n=1;n<=total;n++){CargoEnemyShot2716(e);float expectedArmor=Mathf.Max(0,startArmor-n*10),expectedHP=n<=armorRounds?startHP:Mathf.Max(0,startHP-(n-armorRounds)*18);CargoEnemyRow2716("actual-projectile",e,"hit="+n+" ammo="+G.MagazineAmmo+" armor="+e.NpcArmorPoints+" HP="+e.Health+" dead="+e.Dead);Check(Mathf.Abs(e.NpcArmorPoints-expectedArmor)<.01f&&Mathf.Abs(e.Health-expectedHP)<.01f&&e.Dead==(n==total),"2716 Actual same-level projectile damages armor first then health "+e.name+" hit="+n+" armor="+e.NpcArmorPoints+" HP="+e.Health);}
   Check(e.Dead&&!e.HitCollider.enabled&&G.Map.Loot.Count==loot+1&&G.Kills==kills+1,"2716 Actual central monster death creates one corpse and kill "+e.name);var corpse=G.Map.Loot[loot];Check(Vector3.Distance(corpse.Position,e.transform.position)<.001f&&corpse.Items.Count>=1&&corpse.Items.Count<=3&&corpse.Items.TrueForAll(item=>RaidInventory.Category(item.Kind)==CollectionCategory.Biological&&!item.Identified&&item.RaidFound)&&corpse.SearchDuration>0,"2716 Actual local corpse uses existing level-based unknown biological loot "+e.name);for(int i=0;i<2;i++)CargoEnemyShot2716(e);Check(G.Map.Loot.Count==loot+1&&G.Kills==kills+1,"2716 Subsequent actual projectiles cannot duplicate the same monster corpse "+e.name);
  }
  IEnumerator VerifyCargoEnemies2716(){
   Check(G.Phase==GamePhase.Raid&&G.Map.HarborWhitebox&&G.Paused,"2716 NPC module tests current paused actual harbor session without rebuilding it");CargoEnemyRoster2716();CargoEnemyPatrol2716();yield return null;
   var position=G.Player.transform.position;var aim=G.Player.Aim;var weapon=G.Equipped;var ammo=G.MagazineAmmo;var colliderStates=new Dictionary<Collider,bool>();foreach(var actor in G.Enemies)if(actor.HitCollider){colliderStates[actor.HitCollider]=actor.HitCollider.enabled;actor.HitCollider.enabled=false;}
   try{G.Equipped=new ItemStack(ItemKind.AK,1);var green=G.Map.CargoEnemies2716.Find(p=>p.Kind==MonsterKind.Runner&&p.Level==1);var blue=G.Map.CargoEnemies2716.Find(p=>p.Kind==MonsterKind.Spitter&&p.Level==2);foreach(var p in new[]{green,blue}){p.Actor.HitCollider.enabled=true;CargoEnemyDeath2716(p);}}
   finally{G.ClearShots();foreach(var entry in colliderStates)if(entry.Key){var actor=entry.Key.GetComponent<RaidActor>();entry.Key.enabled=entry.Value&&actor&&!actor.Dead;}G.Equipped=weapon;G.MagazineAmmo=ammo;G.Player.transform.position=position;G.Player.Aim=aim;G.Rig.Tick(0);G.Player.Draw(G.View,0);Physics.SyncTransforms();File.WriteAllLines(Path.Combine(folder,"2716-cargo-actual-enemies.csv"),cargoEnemyRows2716);}
  }
 }
}

