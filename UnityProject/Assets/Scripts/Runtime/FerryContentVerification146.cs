using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
namespace GreyHarbor {
 public sealed partial class RaidVerification {
  void VerifyEnemyLoot146(){
   foreach(MonsterKind kind in Enum.GetValues(typeof(MonsterKind))){
    var kinds=new HashSet<ItemKind>();bool valid=true;int total=0,multi=0;
    var rng=new System.Random(146000+(int)kind);
    for(int i=0;i<10000;i++){var drops=EnemyLoot146.Monster(kind,rng);total+=drops.Count;if(drops.Count>1)multi++;valid&=drops.Count>=1&&drops.Count<=3;
     foreach(var item in drops){kinds.Add(item.Kind);valid&=Array.IndexOf(EnemyLoot146.Fragments,item.Kind)>=0&&item.Count==1&&item.RaidFound&&!item.Identified;}
    }
    Check(valid&&kinds.Count==6&&total>12300&&total<13100&&multi>2300&&multi<2700,"146 Random fragment-only pool, variable quantities and no fixed cloth/ammo reward "+kind);
   }
   foreach(RebelKind kind in Enum.GetValues(typeof(RebelKind))){
    bool valid=true;int collectibles=0;var supplies=new HashSet<ItemKind>();var kinds=new HashSet<ItemKind>();var rng=new System.Random(146100+(int)kind);
    for(int i=0;i<10000;i++){var drops=EnemyLoot146.Soldier(kind,rng);int collection=0,combat=0;valid&=drops.Count>=1&&drops.Count<=3;
     foreach(var item in drops){valid&=item.RaidFound&&!item.Identified&&RaidInventory.Grade(item.Kind)<4;
      if(RaidInventory.Group(item.Kind)==ItemGroup.Collection){collection++;kinds.Add(item.Kind);valid&=RaidInventory.Category(item.Kind)==CollectionCategory.Collectibles&&item.Count==1;}
      else {combat++;supplies.Add(item.Kind);valid&=item.Kind==ItemKind.Ammo?item.Count>=8&&item.Count<=18:item.Count==1;}
     }valid&=combat>=1&&combat<=2&&collection<=1;collectibles+=collection;
    }
    int expected=kind==RebelKind.Regular?3500:kind==RebelKind.BlueRegular?4000:4500;
    Check(valid&&supplies.Count==5&&kinds.Count>=7&&Math.Abs(collectibles-expected)<250,"146 Random supplies and optional common-profile collectibles; no red/gear "+kind);
   }
  }
  void ResetRebel146(RaidActor e){
   e.Dead=false;e.Health=RebelTuning145.HealthFor(e.Rebel);e.HitCollider.enabled=true;
   e.RebelArmor=new ItemStack(RebelTuning145.Armor(e.Rebel),1){ArmorPoints=RebelTuning145.MaxArmor(e.Rebel)};
   e.Windup=e.AttackCooldown=e.RebelShotDelay=0;e.RebelBurst=0;e.Alert=1;e.Memory=7;
  }
  void VerifyRebelArmor146(RaidActor e){
   ResetRebel146(e);float hp=e.Health,max=e.RebelArmor.ArmorPoints;
   Check(hp==(e.Rebel==RebelKind.Heavy?150:100)&&max==(e.Rebel==RebelKind.Regular?80:120),"146 Configured full health/armor "+e.Rebel);
   G.ApplyCombatHit(e,14,10,1,1);Check(e.RebelArmor.ArmorPoints==max-10&&e.Health==hp,"146 NPC armor over item cap is not clamped "+e.Rebel);
   int armorHits=Mathf.CeilToInt(max/10),healthHits=Mathf.CeilToInt(hp/14);
   for(int i=1;i<armorHits;i++)G.ApplyCombatHit(e,14,10,1,1);
   Check(e.Health==hp&&e.RebelArmor.ArmorPoints==0&&!e.Dead,"146 Armor-breaking round has no HP overflow "+e.Rebel);
   for(int i=0;i<healthHits-1;i++)G.ApplyCombatHit(e,14,10,1,1);
   Check(!e.Dead&&e.Health>0,"146 Survives until last expected AK round "+e.Rebel);
   G.ApplyCombatHit(e,14,10,1,1);Check(e.Dead&&e.Health==0,"146 Blue AK kills in "+(armorHits+healthHits)+" hits "+e.Rebel);
   ResetRebel146(e);G.ApplyCombatHit(e,14,10,0,0);
   Check(Mathf.Abs(e.RebelArmor.ArmorPoints-(max-(e.Rebel==RebelKind.BlueRegular?8:10)))<.01f&&e.Health==hp,"146 Ammo-grade armor reduction retained "+e.Rebel);
   ResetRebel146(e);e.Alert=e.Memory=0;e.WaitTime=2;
  }
  void VerifyFerryActors146(bool upper){
   Check(G.Enemies.Count==4&&G.Enemies.FindAll(e=>e.IsRebel).Count==2&&G.Enemies.FindAll(e=>e.IsMonster).Count==2,"146 Two monsters and two soldiers on "+G.Map.InteriorId);
   Check(G.Enemies.FindAll(e=>e.IsRebel&&e.Rebel==RebelKind.Heavy).Count==(upper?1:0),"146 Green shield heavy only on second floor");
   foreach(var d in G.Map.Doors)if(!d.Portal)Check(!G.MonsterBlocksDoor(d),"146 Enemy spawns clear door swing "+d.Name);
   foreach(var e in G.Enemies){
    Check(Clear140(e.Home)&&Vector3.Distance(e.Home,G.Map.Spawn)>2.5f,"146 Enemy spawn clear of furniture and entry "+e.name);
    foreach(var goal in e.PatrolTargets)Check(Clear140(goal),"146 Patrol point clear "+e.name+" "+goal);
   }
  }
  void VerifyFerryPatrol146(){
   var pp=G.Player.transform.position;G.Player.transform.position=new Vector3(100,0,100);Physics.SyncTransforms();
   foreach(var e in G.Enemies){var home=e.transform.position;e.Alert=e.Memory=0;e.WaitTime=0;e.PatrolIndex=1;e.Route.Clear();
    for(int i=0;i<50;i++){if(e.IsRebel)G.TickRebel145(e,.05f);else G.HarborPatrol(e,.05f);Physics.SyncTransforms();}
    Check(Vector3.Distance(home,e.transform.position)>.4f,"146 Actual patrol moves "+e.name);e.transform.position=home;e.Route.Clear();e.PatrolIndex=0;e.WaitTime=2;e.Alert=e.Memory=0;
   }G.Player.transform.position=pp;Physics.SyncTransforms();
  }
  void VerifyWindup146(RaidActor e){
   var pp=G.Player.transform.position;var home=e.transform.position;float hp=G.Player.Health;var armor=G.Armor;
   ResetRebel146(e);e.transform.position=new Vector3(0,0,8);G.Player.transform.position=new Vector3(0,0,5);
   G.Armor=new ItemStack(ItemKind.BlueArmor,1);ArmorBalance.Ensure(G.Armor);G.Player.Health=100;Physics.SyncTransforms();G.Rig.Tick(0);G.ClearShots();
   G.TickRebel145(e,.01f);Check(Mathf.Abs(e.Windup-RebelTuning145.Windup(e.Rebel))<.001f&&G.ActiveGunShots145==0,"146 Harmless configured attack windup "+e.Rebel);
   G.TickRebel145(e,RebelTuning145.Windup(e.Rebel)-.01f);Check(e.Windup>0&&G.ActiveGunShots145==0,"146 No early firing "+e.Rebel);
   G.TickRebel145(e,.02f);G.TickProjectiles(.25f);Check(G.TotalArmor==110&&G.Player.Health==100&&e.RebelBurst==2,"146 Actual soldier bullet applies blue AK armor damage "+e.Rebel);
   G.TickRebel145(e,.2f);G.TickProjectiles(.25f);G.TickRebel145(e,.2f);G.TickProjectiles(.25f);
   Check(e.RebelBurst==0&&e.AttackCooldown>0&&G.TotalArmor==90,"146 Three-shot burst and cooldown "+e.Rebel);
   G.ClearShots();ResetRebel146(e);G.TickRebel145(e,.01f);
   var cover=G.Map.Box("Cover146",new Vector3(0,1,6.5f),new Vector3(2,2,.2f),"687C6B",true);Physics.SyncTransforms();G.TickRebel145(e,2);
   Check(e.Windup==0&&e.RebelBurst==0&&G.ActiveGunShots145==0,"146 Wall breaks aim and stops bullets "+e.Rebel);cover.SetActive(false);Destroy(cover);
   ResetRebel146(e);e.transform.position=home;e.Alert=e.Memory=0;e.WaitTime=2;G.Player.transform.position=pp;G.Player.Health=hp;G.Armor=armor;G.ClearShots();Physics.SyncTransforms();
  }
  IEnumerator VerifyFerryCorpse146(RaidActor e){
   float z=e.IsMonster?7:8;G.Player.transform.position=new Vector3(0,0,z-3);e.transform.position=new Vector3(0,0,z);
   foreach(var other in G.Enemies)if(other!=e)other.HitCollider.enabled=false;
   if(e.IsRebel)ResetRebel146(e);else {e.Dead=false;e.Health=MonsterTuning.Health(e.Monster);e.HitCollider.enabled=true;}
   G.Player.Aim=Vector3.forward;G.Rig.Tick(0);Physics.SyncTransforms();int before=G.Map.Loot.Count,kills=G.Kills;
   int hits=e.IsRebel?Mathf.CeilToInt(e.Health/14)+Mathf.CeilToInt(e.RebelArmor.ArmorPoints/10):Mathf.CeilToInt(e.Health/14);
   for(int i=0;i<hits;i++){G.SpawnShot(G.Player,Vector3.forward);G.TickProjectiles(.2f);}
   Check(e.Dead&&G.Kills==kills+1&&G.Map.Loot.Count==before+1,"146 Actual AK creates exactly one searchable corpse "+e.name);
   foreach(var other in G.Enemies)if(other!=e&&!other.Dead)other.HitCollider.enabled=true;
   var loot=G.Map.Loot[G.Map.Loot.Count-1];Check(loot.Items.Count>=1&&loot.Items.Count<=3&&loot.Items.TrueForAll(p=>!p.Identified&&p.RaidFound),"146 Corpse holds unknown randomized items "+e.name);
   if(e.IsMonster)Check(loot.Items.TrueForAll(p=>Array.IndexOf(EnemyLoot146.Fragments,p.Kind)>=0),"146 Actual monster corpse contains only alien fragments");
   string snapshot=string.Join(",",loot.Items.ConvertAll(p=>p.Kind+":"+p.Count));Check(Walk140(loot.Position),"146 Corpse approach reachable");G.EnsureLootGrid(loot);G.Player.Aim=Vector3.forward;G.Vision.Refresh(G.Player.transform.position,G.Player.Aim);
   F111(false,0);F111(true,loot.SearchDuration+.02f);Check(loot.Opened&&G.OpenLoot==loot,"146 F searches corpse");G.Paused=false;for(int i=0;i<6;i++)G.TickIdentification(3);G.Paused=true;
   Check(loot.Items.TrueForAll(p=>p.Identified),"146 Corpse identification completes");G.CloseInventory();F111(false,0);F111(true,.01f);
   Check(G.OpenLoot==loot&&snapshot==string.Join(",",loot.Items.ConvertAll(p=>p.Kind+":"+p.Count)),"146 Reopening corpse preserves item roll and identification");
   G.Bag.Slots.Clear();G.Chest.Slots.Clear();G.Paused=false;Check(G.QuickTransfer(ItemArea.Loot,loot.Items[0].Cell),"146 Random corpse reward can be collected");G.Paused=true;G.CloseInventory();G.ClearShots();yield return null;
  }
  IEnumerator VerifyFerryContent146(){
   VerifyEnemyLoot146();FreshCamp();string saved=JsonUtility.ToJson(G.Save);G.HarborWalkthrough=true;G.HardMode=false;G.BeginTraining();G.Paused=true;yield return null;
   var outside=G.Map;float zoom=G.View.orthographicSize;var player=G.Player;
   Check(G.SavePath.Contains("1.46")&&!G.SavePath.EndsWith("stash-v40.json"),"146 Verification save isolated from formal save");
   Check(outside.Loot.Count==30&&G.Enemies.Count==12,"146 Outdoor density unchanged");
   G.Player.transform.position=RaidMap.FerryApproach143;G.Rig.ResetView(135);G.Rig.Tick(0);Physics.SyncTransforms();
   F111(false,0);F111(true,1.99f);Check(G.Map==outside,"146 Boarding requires two seconds");F111(true,.02f);G.Paused=true;yield return null;var first=G.Map;
   Check(first.FerryLoot146.Count==7&&first.Loot.Count==7&&G.Player==player&&G.View.orthographicSize==zoom,"146 First floor seven containers and unchanged player/map scale");
   foreach(var l in first.FerryLoot146)Debug.Log("146 STAND "+l.Name+" "+Clear140(l.Position));VerifyFerryActors146(false);VerifyClosedCabinWalls145();yield return VerifyFerryRooms145();VerifyFerryPatrol146();
   foreach(var e in G.Enemies)if(e.IsRebel)VerifyRebelArmor146(e);
   // Prevent other actors intercepting test shots, while keeping all final placements intact.
   foreach(var e in G.Enemies)if(e.IsRebel){foreach(var other in G.Enemies)if(other!=e)other.HitCollider.enabled=false;VerifyWindup146(e);foreach(var other in G.Enemies)if(other!=e)other.HitCollider.enabled=true;}
   foreach(var loot in first.FerryLoot146){Check(loot.Tier==ContainerTier.Common,"146 Ordinary ship container tier "+loot.Name);G.Bag.Slots.Clear();G.Chest.Slots.Clear();yield return Search140(loot);}
   yield return FerryPicture143("146-ferry-floor1.png");
   var deadSoldier=G.Enemies.Find(e=>e.IsRebel&&e.Rebel==RebelKind.Regular);var deadMonster=G.Enemies.Find(e=>e.IsMonster&&e.Monster==MonsterKind.Runner);
   yield return VerifyFerryCorpse146(deadSoldier);yield return VerifyFerryCorpse146(deadMonster);int firstLoot=first.Loot.Count;
   G.Rig.ResetView(225);G.Rig.Tick(0);Check(Walk140(new Vector3(2.8f,0,12.3f)),"146 Lower front stair reachable");
   F111(false,0);F111(true,1.2f);G.Controls=new TouchInput{Shooting=true,Searching=true};G.TickInteraction(.1f);Check(G.Map==first&&G.DoorHold==0,"146 Shooting cancels floor-change hold");
   F111(false,0);F111(true,2.01f);G.Paused=true;yield return null;var second=G.Map;foreach(var l in second.FerryLoot146)Debug.Log("146 STAND "+l.Name+" "+Clear140(l.Position));VerifyFerryActors146(true);
   Check(second.FerryLoot146.Count==5&&second.Loot.Count==5&&G.View.orthographicSize==zoom,"146 Second floor five containers");
   var key=second.FerryRooms143.Find(r=>r.Door.Locked);var center=new Vector3(key.Area.center.x,0,key.Area.center.y);
   Check(!G.Vision.RoomAllowed(center)&&!second.Loot.Exists(l=>key.Contains(new Vector2(l.Prop.transform.position.x,l.Prop.transform.position.z)))&&!G.Enemies.Exists(e=>key.Contains(new Vector2(e.Home.x,e.Home.z))),"146 Locked VIP room black and empty");
   yield return VerifyFerryRooms145();VerifyFerryPatrol146();var heavy=G.Enemies.Find(e=>e.IsRebel&&e.Rebel==RebelKind.Heavy);VerifyRebelArmor146(heavy);
   foreach(var other in G.Enemies)if(other!=heavy)other.HitCollider.enabled=false;VerifyWindup146(heavy);foreach(var other in G.Enemies)if(other!=heavy)other.HitCollider.enabled=true;
   foreach(var loot in second.FerryLoot146){G.Bag.Slots.Clear();G.Chest.Slots.Clear();yield return Search140(loot);}
   Check(!G.Vision.RoomAllowed(center)&&key.Door.Locked&&!key.Door.Open,"146 Searching adjacent rooms never reveals key room");yield return FerryPicture143("146-ferry-floor2.png");
   heavy.Health=123;heavy.RebelArmor.ArmorPoints=105;heavy.Windup=.5f;heavy.RebelBurst=2;
   G.Rig.ResetView(45);G.Rig.Tick(0);Check(Walk140(new Vector3(0,0,-13.2f)),"146 Upper rear stair reachable");
   F111(false,0);F111(true,2.01f);G.Paused=true;yield return null;
   Check(G.Map==first&&G.Enemies.Count==4&&G.Enemies.Contains(deadSoldier)&&deadSoldier.Dead&&deadMonster.Dead&&first.Loot.Count==firstLoot&&Mathf.Abs(Mathf.DeltaAngle(G.Rig.Yaw,225))<.1f,"146 Floor revisit keeps dead actors, corpses and camera");
   Check(Walk140(new Vector3(0,0,-13.2f)),"146 Lower rear stair reachable");F111(false,0);F111(true,2.01f);G.Paused=true;yield return null;
   Check(G.Map==second&&G.Enemies.Count==4&&G.Enemies.Contains(heavy)&&heavy.Health==123&&heavy.RebelArmor.ArmorPoints==105&&heavy.Windup==0&&heavy.RebelBurst==0&&Mathf.Abs(Mathf.DeltaAngle(G.Rig.Yaw,45))<.1f,"146 Heavy health/armor above green item cap retained across floor cache");
   Check(second.FerryLoot146.TrueForAll(l=>l.Opened&&l.Prop.Searched)&&!G.Vision.RoomAllowed(center),"146 Searched containers and locked room persist");
   G.SwitchInterior("ferry-floor:1:front");G.Paused=true;yield return null;Check(Walk140(new Vector3(-6,0,0)),"146 Boarding lobby exit still reachable");F111(false,0);F111(true,2.01f);G.Paused=true;yield return null;
   Check(G.Map==outside&&G.Enemies.Count==12&&Mathf.Abs(Mathf.DeltaAngle(G.Rig.Yaw,135))<.1f&&G.ActiveGunShots145==0,"146 Exit restores outside actors/camera and clears projectiles");
   G.Player.transform.position=outside.Exits[1];G.Controls=new TouchInput();G.Paused=false;G.TickExtraction(5.1f);yield return null;Check(G.Phase==GamePhase.Base&&JsonUtility.ToJson(G.Save)==saved,"146 Preview extraction restores prior inventory");
  }
 }
}
