using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using UnityEngine;
namespace GreyHarbor {
 public sealed partial class RaidVerification {
  readonly List<string> akRows276=new List<string>();
  static string AkNumber276(float n)=>n.ToString("0.00000",CultureInfo.InvariantCulture);
  float AkAngle276(Vector3 direction)=>Vector3.SignedAngle(Vector3.right,direction,Vector3.up);
  void AkAimInput276(TouchInput input,bool compensate){
   var aim=Quaternion.Euler(0,compensate?-G.AkHandling276.Recoil:0,0)*Vector3.right;
   var screen=G.View.WorldToScreenPoint(G.Player.transform.position+Vector3.up*RaidGame.CombatHeight+aim*12);
   input.AimPosition=new Vector2(screen.x,screen.y);
  }
  IEnumerator AkWait276(TouchInput input,float seconds,bool compensate=false){
   float elapsed=0;while(elapsed<seconds){AkAimInput276(input,compensate);yield return null;elapsed+=Time.deltaTime;}
  }
  IEnumerator AkLiveSeries276(TouchInput input,string mode,int count,bool moving,bool compensate,bool shortBursts=false){
   G.Paused=false;G.InventoryOpen=false;input.Shooting=false;input.Movement=Vector2.zero;
   G.Player.transform.position=Vector3.zero;G.ResetAkHandling276();G.Magazine=30;G.ReloadTime=G.HealTime=0;
   Physics.SyncTransforms();G.Rig.Tick(0);AkAimInput276(input,compensate);G.UpdateAim();
   UnityEngine.Random.InitState(20261002);input.Movement=moving?new Vector2(0,1):Vector2.zero;input.Shooting=true;
   float timeout=Time.realtimeSinceStartup+15;int pausedAfter=0;
   while(G.AkShots276.Count<count){
    if(Time.realtimeSinceStartup>timeout)throw new Exception("AK live fire timeout: "+mode);
    AkAimInput276(input,compensate);yield return null;
    int fired=G.AkShots276.Count;
    if(shortBursts&&fired>0&&fired%3==0&&fired<count&&pausedAfter!=fired){
     pausedAfter=fired;input.Shooting=false;yield return AkWait276(input,1.65f);input.Shooting=true;
    }
   }
   input.Shooting=false;input.Movement=Vector2.zero;G.Paused=true;
   Check(G.AkShots276.Count==count&&G.Magazine==30-count,"276 Real FirePlayer consumes one round for every recorded live projectile "+mode);
   // Advance the ordinary projectile collision path while player handling is frozen for inspection.
   G.TickProjectiles(.35f);foreach(var s in G.AkShots276){
    Check(s.Ended&&s.Hit=="AkRangePlate276","276 Live projectile hits the physical 12m range plate "+mode+" #"+s.Number);
    float actualAngle=Mathf.Atan2(-(s.Impact.z-s.Origin.z),s.Impact.x-s.Origin.x)*Mathf.Rad2Deg;
    Check(Mathf.Abs(Mathf.DeltaAngle(actualAngle,AkAngle276(s.Direction)))<.02f,"276 Collision impact follows actual projectile angle "+mode+" #"+s.Number);
    Check(Mathf.Abs(s.RandomOffset)<=s.Spread+.001f,"276 Actual random deviation stays inside current spread cone "+mode+" #"+s.Number);
    akRows276.Add(mode+","+s.Number+","+s.Burst+","+AkNumber276(s.Clock)+","+AkNumber276(s.Recoil)+","+AkNumber276(s.Spread)+","+AkNumber276(s.RandomOffset)+","+AkNumber276(s.Move)+","+AkNumber276(AkAngle276(s.RawAim))+","+AkNumber276(AkAngle276(s.Aim))+","+AkNumber276(AkAngle276(s.Direction))+","+AkNumber276(s.Origin.x)+","+AkNumber276(s.Origin.z)+","+AkNumber276(s.Impact.x)+","+AkNumber276(s.Impact.z)+","+s.Hit);
   }
  }
  float AkRms276(List<AkShotSample276> shots,bool random=false){float sum=0;foreach(var s in shots){float value=random?s.RandomOffset:AkAngle276(s.Direction);sum+=value*value;}return Mathf.Sqrt(sum/shots.Count);}
  void AkIdentity276(){
   Check(RaidInventory.Grade(ItemKind.AK)==0&&RaidInventory.GradeName(ItemKind.AK)=="绿色","276 AK keeps ID while showing green weapon grade");
   Check(RaidInventory.Tint(ItemKind.AK)==RaidInventory.Tint(ItemKind.GreenAmmo),"276 AK tint uses green quality");
   Check((int)ItemKind.AK==11&&RaidInventory.Width(ItemKind.AK)==3&&RaidInventory.Height(ItemKind.AK)==2,"276 Existing AK ID11 and 3x2 footprint are preserved");
   Check(MarketRules.UnitPrice(ItemKind.AK)==1196&&RaidInventory.Value(ItemKind.AK)==1040,"276 AK buy1196/sell1040 prices are unchanged");
   var old=JsonUtility.FromJson<ItemStack>("{\"Kind\":11,\"Count\":1,\"Identified\":true,\"Id\":\"legacy-ak-11\"}");var round=JsonUtility.FromJson<ItemStack>(JsonUtility.ToJson(old));
   Check(round.Kind==ItemKind.AK&&round.Id=="legacy-ak-11"&&round.Count==1,"276 Legacy integer AK save identity survives JsonUtility round trip");
   Check(G.Equipped!=null&&G.Equipped.Kind==ItemKind.AK,"276 Normal quick loadout equips the unchanged AK item");
   var profile=ContainerDropRules230.Profile(ContainerLook.MilitaryCase,SupplyType.Military,ContainerTier.Common,DropRegion230.Repair);
   var quality=ContainerDropRules230.QualityWeights(profile,false);double sum=0;foreach(var value in quality)sum+=value;
   Check(ContainerDropRules230.Eligible(profile,ItemKind.AK)&&Math.Abs(sum-1)<.000001&&quality[0]>0,"276 Military profile keeps AK eligible in normalized green quality pool");
   bool green=false,blue=false;for(int i=0;i<RaidInventory.KindCount;i++){var k=(ItemKind)i;if(k==ItemKind.AK&&ContainerDropRules230.Eligible(profile,k)){green|=RaidInventory.Grade(k)==0;blue|=RaidInventory.Grade(k)==1;}}
   Check(green&&!blue,"276 Military blue candidate filter no longer categorizes AK as blue");
   Check(WeaponRules270.Damage(ItemKind.AK)==18&&WeaponRules270.Penetration(ItemKind.AK)==10,"276 Handling keeps AK damage18/penetration10");
  }
  public IEnumerator VerifyAkHandling276(){
   var previousInput=G.Controls;var previousRandom=UnityEngine.Random.state;akRows276.Clear();
   akRows276.Add("mode,shot,burst,active_clock_s,recoil_yaw_deg,spread_half_deg,random_yaw_deg,movement,raw_yaw_deg,aim_yaw_deg,projectile_yaw_deg,muzzle_x,muzzle_z,impact_x,impact_z,hit");
   try {
    StandardRaid();AkIdentity276();foreach(var e in G.Enemies){e.Dead=true;e.HitCollider.enabled=false;}
    foreach(var c in G.Map.Root.GetComponentsInChildren<Collider>())c.enabled=false;
    var plate=new GameObject("AkRangePlate276");plate.transform.SetParent(G.Map.Root,false);plate.transform.position=new Vector3(12,.95f,0);plate.AddComponent<BoxCollider>().size=new Vector3(.08f,2,40);SceneObstacle.Apply(plate,ObstacleKind.High);
    var input=new TouchInput();G.Controls=input;Physics.SyncTransforms();
    yield return AkLiveSeries276(input,"short_3round_bursts",9,false,false,true);var shortShots=new List<AkShotSample276>(G.AkShots276);float shortRms=AkRms276(shortShots);
    Check(shortShots[0].Recoil==0&&shortShots[0].Spread==1.2f,"276 First shot retains useful original +-1.2degree precision and no inherited drift");
    Check(shortShots[2].Recoil<2&&shortShots[3].Recoil<.01f,"276 Three-shot bursts keep limited recoil and recover before the next burst");
    yield return AkLiveSeries276(input,"stationary_30round_sweep",30,false,false);var sweep=new List<AkShotSample276>(G.AkShots276);float sweepRms=AkRms276(sweep);
    Check(sweep[20].Recoil>=15&&sweep[29].Spread>4.9f&&sweepRms>shortRms*2,"276 Actual long sweep drifts and scatters substantially more than three-shot bursts");
    Check(AkRms276(sweep,true)>.5f,"276 Real full-auto shots contain independent random dispersion in addition to deterministic recoil");
    for(int i=1;i<sweep.Count;i++)Check(sweep[i].Clock-sweep[i-1].Clock>=.109f,"276 Live AK rate keeps at least .11seconds between rounds #"+(i+1));
    var h=G.AkHandling276;float recoil=h.Recoil,heat=h.Heat,clock=h.Clock;int mag=G.Magazine,count=G.AkShots276.Count;
    input.Shooting=true;G.FirePlayer();yield return new WaitForSeconds(.3f);
    Check(h.Recoil==recoil&&h.Heat==heat&&h.Clock==clock&&G.Magazine==mag&&G.AkShots276.Count==count,"276 Paused live game neither fires nor accumulates or recovers handling");
    G.Paused=false;G.InventoryOpen=true;G.FirePlayer();yield return new WaitForSeconds(.3f);
    Check(h.Recoil==recoil&&h.Heat==heat&&h.Clock==clock&&G.Magazine==mag,"276 Open inventory freezes handling and blocks firing");
    G.InventoryOpen=false;input.Shooting=false;yield return AkWait276(input,1.65f);
    Check(h.Recoil<.01f&&h.Heat<.01f,"276 Actual stop-fire time restores recoil and spread completely");
    yield return AkLiveSeries276(input,"moving_30round_sweep",30,true,false);var moving=new List<AkShotSample276>(G.AkShots276);
    Check(moving[0].Move>.95f&&moving[0].Spread>2.6f&&moving[29].Spread>6.35f,"276 Actual walking adds dispersion to first shot and sustained sweep");
    yield return AkLiveSeries276(input,"mouse_counter_30round_sweep",30,false,true);var counter=new List<AkShotSample276>(G.AkShots276);float counterRms=AkRms276(counter);
    Check(counter[20].Recoil>=15&&Mathf.Abs(AkAngle276(counter[20].Aim))<.4f&&counterRms<sweepRms*.5f,"276 Real reverse mouse input compensates cumulative recoil while random spread remains");
    G.Paused=false;input.Shooting=false;
    for(int i=0;i<4;i++){input.TurnPressed=1;yield return AkWait276(input,.4f);AkAimInput276(input,false);G.UpdateAim();Check(Mathf.Abs(Vector3.SignedAngle(G.RawPlayerAim276,G.Player.Aim,Vector3.up)-h.Recoil)<.05f,"276 Actual Q camera turn preserves handling yaw without extra angular error "+i);}
    G.Paused=true;G.ResetAkHandling276();G.Player.transform.position=Vector3.zero;G.Magazine=30;G.Rig.Tick(0);AkAimInput276(input,false);G.UpdateAim();
    var wall=new GameObject("AkBlockingWall276");wall.transform.SetParent(G.Map.Root,false);wall.transform.position=new Vector3(3,.95f,0);wall.AddComponent<BoxCollider>().size=new Vector3(.20f,2,8);SceneObstacle.Apply(wall,ObstacleKind.High);Physics.SyncTransforms();
    G.Paused=false;input.Shooting=true;while(G.AkShots276.Count<1){AkAimInput276(input,false);yield return null;}input.Shooting=false;G.Paused=true;G.TickProjectiles(.35f);
    Check(G.AkShots276[0].Ended&&G.AkShots276[0].Hit=="AkBlockingWall276"&&G.AkShots276[0].Impact.x<3.01f,"276 Real handling bullet remains blocked by the first solid wall");
    var enemyObject=new GameObject("AkIndependentSoldier276");enemyObject.transform.SetParent(G.Map.Root,false);enemyObject.transform.position=new Vector3(-2,0,5);var soldier=enemyObject.AddComponent<RaidActor>();soldier.Initialize(true);soldier.IsRebel=true;soldier.Rebel=RebelKind.Regular;
    G.SpawnShot(soldier,Vector3.right);Check(Vector3.Distance(G.LastEnemyShotDirection276,Vector3.right)<.001f&&G.LastEnemyShotDamage276==14&&G.LastEnemyShotPen276==10,"276 Actual NPC projectile direction and independent14/10 attack bypass player AK handling");
    var summary=new[]{"mode,shots,rms_actual_yaw_deg,rms_random_yaw_deg","short_3round_bursts,"+shortShots.Count+","+AkNumber276(shortRms)+","+AkNumber276(AkRms276(shortShots,true)),"stationary_30round_sweep,30,"+AkNumber276(sweepRms)+","+AkNumber276(AkRms276(sweep,true)),"moving_30round_sweep,30,"+AkNumber276(AkRms276(moving))+","+AkNumber276(AkRms276(moving,true)),"mouse_counter_30round_sweep,30,"+AkNumber276(counterRms)+","+AkNumber276(AkRms276(counter,true))};
    File.WriteAllLines(Path.Combine(folder,"276-ak-live-shots.csv"),akRows276);File.WriteAllLines(Path.Combine(folder,"276-ak-live-comparison.csv"),summary);
    File.WriteAllLines(Path.Combine(folder,"276-ak-handling-parameters.csv"),new[]{"parameter,value","damage,18","penetration,10","interval_s,.11","first_spread_half_deg,1.2","heat_per_shot_deg,.22","heat_cap_deg,3.75","moving_extra_half_deg,1.45","first_kick_deg,.85","kick_growth_per_burst_shot_deg,.08","kick_growth_cap_deg,.55","recoil_cap_deg,16","recovery_delay_s,.20","recoil_recovery_deg_per_s,12","heat_recovery_deg_per_s,2.8"});
    G.Equipped=new ItemStack(ItemKind.AK,1);yield return null;
    Check(h.Recoil==0&&h.Heat==0&&h.Shots==0,"276 Switching the equipped weapon resets handling scope");
    G.Magazine=30;G.Paused=false;input.Shooting=true;
    float scopeDeadline=Time.realtimeSinceStartup+2;while(h.Shots<1){if(Time.realtimeSinceStartup>scopeDeadline)throw new Exception("AK scope live shot timeout");AkAimInput276(input,false);yield return null;}input.Shooting=false;
    Check(h.Recoil>0&&h.Heat>0,"276 New equipped AK builds handling from actual live fire");
    G.Player.Damage(100);yield return null;yield return null;
    Check(G.Phase==GamePhase.Base&&h.Recoil==0&&h.Heat==0&&h.Shots==0,"276 Actual player death and return to camp clear prior-raid handling");
   }finally{if(akRows276.Count>1)File.WriteAllLines(Path.Combine(folder,"276-ak-live-shots.csv"),akRows276);G.Controls=previousInput;UnityEngine.Random.state=previousRandom;G.Paused=true;G.InventoryOpen=false;}
  }
 }
}
