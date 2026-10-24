using UnityEngine;
namespace GreyHarbor {
 public sealed partial class RaidGame {
  public RaidDestination NextMap=RaidDestination.Harbor;
  public bool TrainingActive,VerificationSmallMap,HarborWalkthrough;
  public int RaidSeed;
  string trainingSnapshot;
  public bool BeginTraining(){if(!PrepareTraining2721())return false;BuildRaid();CompleteTraining2721();return true;}
  void EndTraining(){Save=JsonUtility.FromJson<BaseSave>(trainingSnapshot);TrainingActive=false;HarborWalkthrough=false;AssetShowcaseMode2717=false;SetGalleryLighting2717(false);trainingSnapshot=null;ModRange315=false;RangeGun315=null;BuildCamp();GetComponent<RaidUI>()?.RestoreRangePage315();Notify("训练结束，已恢复进入前的装备与物资。");}
  void PopulateMonsters(){
   var rng=new System.Random(RaidSeed^0x1357);
   if(Map.IsHarbor){foreach(var site in Map.MonsterSites){var k=(MonsterKind)HarborContent.Pick(rng,HarborContent.MonsterWeights,(int)site.District);SpawnMonster(site.Position,k);}}
   else {SpawnMonster(new Vector3(-8,0,5),MonsterKind.Wanderer,"Guard_Office");SpawnMonster(new Vector3(1,0,3),MonsterKind.Runner,"Guard_Storage");SpawnMonster(new Vector3(8,0,1),MonsterKind.Shell,"Guard_Yard");SpawnMonster(new Vector3(13,0,7),MonsterKind.Wanderer,"Guard_Exit");}
  }
  public RaidActor SpawnMonster(Vector3 pos,MonsterKind kind,string name=null,int level=1){var e=Actor(name??HarborContent.MonsterNames[(int)kind],pos,true);e.ConfigureMonster(kind);NpcRules270.SetMonsterLevel(e,level);Enemies.Add(e);return e;}
  public void MonsterDeath(RaidActor e){CreateCorpse325(e,false);}
  public void TickMonster(RaidActor e,float dt){
   if(e.Dead||!e.gameObject.activeInHierarchy||Player.Dead)return;
   e.MonsterMoving=false;e.AttackPose=Mathf.Max(0,e.AttackPose-dt);
   Vector3 delta=Player.transform.position-e.transform.position;float dist=delta.magnitude;
   if(dist>32&&e.Alert<.1f)return;
   e.AttackCooldown=Mathf.Max(0,e.AttackCooldown-dt);
   bool visible=dist<(e.Monster==MonsterKind.Runner?11:9)&&ClearSight(e.transform.position,Player.transform.position);
   if(visible){e.Alert=Mathf.Min(1,e.Alert+dt/.65f);e.LastKnown=Player.transform.position;e.Memory=7;}
   else {e.Memory=Mathf.Max(0,e.Memory-dt);if(e.Memory==0)e.Alert=Mathf.Max(0,e.Alert-dt*.5f);}
   // Aim is committed for the entire windup; there is no last-frame target tracking.
   if(e.Windup>0){e.Windup=Mathf.Max(0,e.Windup-dt);if(e.Windup==0){
     e.AttackPose=.25f;
     if(MonsterTuning.LungeDuration(e.Monster)>0)e.Lunge=MonsterTuning.LungeDuration(e.Monster);
     else if(e.Monster==MonsterKind.Spitter)SpawnSpit(e);
     else MonsterHit(e,MonsterTuning.HitReach(e.Monster));
     e.AttackCooldown=MonsterTuning.Cooldown(e.Monster);
    }return;}
   if(e.Lunge>0){float advance=Mathf.Min(dt,e.Lunge);e.Lunge=Mathf.Max(0,e.Lunge-dt);e.Move(e.AttackDirection*MonsterTuning.LungeSpeed(e.Monster)*advance);
    if(!e.LungeHit&&Vector3.Distance(e.transform.position,Player.transform.position)<MonsterTuning.HitReach(e.Monster)&&ClearSight(e.transform.position,Player.transform.position)){MonsterHit(e,MonsterTuning.HitReach(e.Monster));e.LungeHit=true;}return;}
   if(e.Alert>.1f){
    if(delta.sqrMagnitude>.01f)e.Aim=delta.normalized;
    if(e.Alert>=1&&visible&&dist<MonsterTuning.Reach(e.Monster)&&e.AttackCooldown<=0){
     e.Windup=MonsterTuning.Windup(e.Monster);e.AttackDirection=e.Aim;e.LungeHit=false;EmitSound326(e,SoundKind326.Creature);
     if(e.Monster==MonsterKind.Spitter)PlaySpitCue(e);return;
    }
    // Spitters hold their ground in range, never kite backwards indefinitely.
    bool holding=e.Monster==MonsterKind.Spitter&&visible&&dist<7.5f;
    if(e.Memory>0&&dist>.85f&&!holding)FollowPath(e,e.LastKnown,MonsterTuning.Speed(e.Monster),dt);
   }else if(e.PatrolTargets!=null)HarborPatrol(e,dt);else Patrol(e,dt);
  }
  void MonsterHit(RaidActor e,float reach){
   if(Player.Dead||Vector3.Distance(e.transform.position,Player.transform.position)>reach||!ClearSight(e.transform.position,Player.transform.position))return;
   // A sidestep behind the committed attack avoids melee as well as a lunge.
   Vector3 toward=Player.transform.position-e.transform.position;
   if(toward.sqrMagnitude>.04f&&Vector3.Dot(e.AttackDirection,toward.normalized)<.2f)return;
   ApplyCombatHit(Player,NpcCombat275.IndependentDamage(e),NpcCombat275.IndependentPenetration(e),0,NpcCombat275.AttackGrade345(e));HurtFlash=.3f;
  }
  void PopulateTrainingMonsters(){
   AddTrainingMonster(MonsterKind.Wanderer,new Vector3(-6,0,-4),new Vector3(-8,0,-6),new Vector3(-6,0,-1));
   AddTrainingMonster(MonsterKind.Runner,new Vector3(-8,0,6),new Vector3(-8,0,3),new Vector3(-5,0,6));
   AddTrainingMonster(MonsterKind.Shell,new Vector3(7,0,12),new Vector3(7,0,10),new Vector3(9,0,12));
   AddTrainingMonster(MonsterKind.Spitter,new Vector3(15,0,3),new Vector3(15,0,5),new Vector3(15,0,1));
  }
  void AddTrainingMonster(MonsterKind kind,Vector3 home,Vector3 a,Vector3 b){var e=SpawnMonster(home,kind);e.PatrolTargets=new[]{home,a,b};e.WaitTime=2;}
  public bool HarborPatrol(RaidActor e,float dt){if(e.WaitTime>0){e.WaitTime-=dt;return true;}if(e.PatrolTargets==null){e.PatrolTargets=new[]{e.Home+new Vector3(7,0,5),e.Home+new Vector3(-5,0,7),e.Home+new Vector3(-6,0,-5),e.Home};}var goal=e.PatrolTargets[e.PatrolIndex%e.PatrolTargets.Length];if(Vector3.Distance(e.transform.position,goal)<.35f){e.PatrolIndex++;e.WaitTime=1.2f;e.Route.Clear();return true;}if(!FollowPath(e,goal,MonsterTuning.PatrolSpeed(e.Monster),dt)){e.PatrolIndex++;e.WaitTime=.4f;}return true;}
 }
}

