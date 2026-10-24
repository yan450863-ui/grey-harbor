using System;
using System.Collections.Generic;
using UnityEngine;
namespace GreyHarbor {
 public sealed class UndergroundEnemy2757 {
  public int ZoneIndex,Level=1;
  public string ZoneName,Name;
  public MonsterKind Kind;
  public Vector3 Home;
  public Vector3[] Targets;
  public RaidActor Actor;
 }
 public sealed partial class RaidMap {
  public readonly List<UndergroundEnemy2757> UndergroundEnemies2757=new List<UndergroundEnemy2757>();
  public bool UndergroundEnemyPointClear2757(Vector3 p){
   if(!UndergroundFloorContains2752(p,.45f)||Physics.CheckCapsule(p+Vector3.up*.35f,p+Vector3.up*1.35f,.45f,SceneObstacle.MovementMask,QueryTriggerInteraction.Ignore))return false;
   foreach(var entry in UndergroundEntries2752)if(Vector3.Distance(p,entry.Arrival)<5)return false;
   foreach(var door in Doors)if(Vector3.Distance(p,door.Position)<3)return false;
   foreach(var loot in Loot)if(Vector3.Distance(p,loot.Position)<1.8f)return false;
   return true;
  }
 }
 public sealed partial class RaidGame {
  public List<UndergroundEnemy2757> UndergroundEnemies2757=>Map==null?null:Map.UndergroundEnemies2757;
  void PopulateUndergroundEnemies2757(){
   if(!Map.UndergroundWhitebox2752||Map.UndergroundEnemies2757.Count>0)return;
   var names=new[]{"西北泵站","东侧排水平台","中部检修平台","西南汇流平台","东南沉降平台","北部主汇流平台"};
   var kinds=new[]{
    new[]{MonsterKind.Pipewhisker,MonsterKind.BallastHauler},
    new[]{MonsterKind.PaperbackCreeper,MonsterKind.Pipewhisker},
    new[]{MonsterKind.Pipewhisker,MonsterKind.PaperbackCreeper,MonsterKind.BallastHauler},
    new[]{MonsterKind.Pipewhisker,MonsterKind.PaperbackCreeper},
    new[]{MonsterKind.BallastHauler,MonsterKind.Pipewhisker,MonsterKind.PaperbackCreeper},
    new[]{MonsterKind.Pipewhisker,MonsterKind.BallastHauler}
   };
   var rng=new System.Random(2757);Physics.SyncTransforms();
   // Choose the whole roster before spawning; the map-local records survive every portal switch.
   for(int zone=0;zone<kinds.Length;zone++){
    var area=Map.UndergroundOpenAreas2753[zone];var candidates=new List<Vector3>();
    for(float x=area.xMin+1.2f;x<area.xMax-1.2f;x+=.6f)for(float z=area.yMin+1.2f;z<area.yMax-1.2f;z+=.6f)candidates.Add(new Vector3(x,0,z));
    for(int i=candidates.Count-1;i>0;i--){int j=rng.Next(i+1);var old=candidates[i];candidates[i]=candidates[j];candidates[j]=old;}
    for(int member=0;member<kinds[zone].Length;member++){
     UndergroundEnemy2757 selected=null;
     foreach(var home in candidates){
      if(!Map.UndergroundEnemyPointClear2757(home))continue;
      bool nearby=false;foreach(var previous in Map.UndergroundEnemies2757)if(Vector3.Distance(home,previous.Home)<3){nearby=true;break;}if(nearby)continue;
      int start=rng.Next(16);
      foreach(float length in new[]{2.0f,1.6f,1.2f}){
       for(int step=0;step<16;step++){
        float angle=(start+step)*Mathf.PI/8;var endpoint=home+new Vector3(Mathf.Cos(angle),0,Mathf.Sin(angle))*length;
        if(!area.Contains(new Vector2(endpoint.x,endpoint.z))||!Map.UndergroundEnemyPointClear2757(endpoint)||!Navigation.ClearSegment(home,endpoint))continue;
        var direction=endpoint-home;bool routeClear=true;
        int samples=Mathf.CeilToInt(length/.25f);for(int sample=1;sample<samples;sample++)if(!Map.UndergroundEnemyPointClear2757(Vector3.Lerp(home,endpoint,sample/(float)samples))){routeClear=false;break;}if(!routeClear)continue;
        if(Physics.CapsuleCast(home+Vector3.up*.35f,home+Vector3.up*1.35f,.45f,direction.normalized,direction.magnitude,SceneObstacle.MovementMask,QueryTriggerInteraction.Ignore))continue;
        nearby=false;foreach(var previous in Map.UndergroundEnemies2757)if(Vector3.Distance(endpoint,previous.Home)<2){nearby=true;break;}if(nearby)continue;
        selected=new UndergroundEnemy2757{ZoneIndex=zone,ZoneName=names[zone],Name="地下 / "+names[zone]+" / "+kinds[zone][member]+" "+(member+1),Kind=kinds[zone][member],Home=home,Targets=new[]{home,endpoint}};break;
       }
       if(selected!=null)break;
      }
      if(selected!=null)break;
     }
     if(selected==null)throw new InvalidOperationException("Underground2757 no safe monster patrol in "+names[zone]+" member "+member);
     Map.UndergroundEnemies2757.Add(selected);
    }
   }
   foreach(var plan in Map.UndergroundEnemies2757){
    plan.Actor=SpawnMonster(plan.Home,plan.Kind,plan.Name,plan.Level);plan.Actor.PatrolTargets=(Vector3[])plan.Targets.Clone();plan.Actor.WaitTime=1.5f;plan.Actor.AttackCooldown=2;
   }
  }
 }
}
