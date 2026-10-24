using System;
using System.Collections.Generic;
using UnityEngine;
namespace GreyHarbor {
 public sealed class CargoEnemy2716 {
  public readonly string Name;public readonly MonsterKind Kind;public readonly int Level;public readonly Vector3 Home;public readonly Vector3[] Targets;public RaidActor Actor;
  public CargoEnemy2716(string name,MonsterKind kind,int level,float x,float z,Vector3 axis){Name=name;Kind=kind;Level=level;Home=RaidMap.PortPoint(x,z);Targets=new[]{Home,Home-axis*2,Home+axis*2};}
 }
 public sealed partial class RaidMap {
  // These records belong to the map instance, just like its actors and loot.
  public readonly List<CargoEnemy2716> CargoEnemies2716=new List<CargoEnemy2716>();
 }
 public sealed partial class RaidGame {
  // Exposes only the current map; changing maps cannot leave a stale global list.
  public List<CargoEnemy2716> CargoEnemies2716=>Map==null?null:Map.CargoEnemies2716;
  void PopulateCargoEnemies2716(){
   if(!Map.HarborWhitebox||Map.CargoEnemies2716.Count>0)return;
   var plans=new[]{
    new CargoEnemy2716("货场 / 南侧游荡异种",MonsterKind.Wanderer,1,89,74,Vector3.forward),
    new CargoEnemy2716("货场 / 西南疾行异种",MonsterKind.Runner,1,73,89,Vector3.right),
    new CargoEnemy2716("货场 / 西侧游荡异种",MonsterKind.Wanderer,1,78,102,Vector3.right),
    new CargoEnemy2716("货场 / 西北蓝级游荡异种",MonsterKind.Wanderer,2,67,107,Vector3.forward),
    new CargoEnemy2716("货场 / 北侧游荡异种",MonsterKind.Wanderer,1,98,121,Vector3.forward),
    new CargoEnemy2716("货场 / 东侧疾行异种",MonsterKind.Runner,1,112,97,Vector3.right),
    new CargoEnemy2716("货场 / 中北蓝级喷吐异种",MonsterKind.Spitter,2,94,109,Vector3.forward)
   };
   Physics.SyncTransforms();foreach(var p in plans){
    foreach(var point in p.Targets)if(!Map.HarborSurfaceClear(point)||Physics.CheckCapsule(point+Vector3.up*.35f,point+Vector3.up,.34f,SceneObstacle.MovementMask,QueryTriggerInteraction.Ignore))throw new InvalidOperationException("Cargo2716 blocked authored monster patrol: "+p.Name+" "+point);
    for(int i=0;i<p.Targets.Length;i++){var a=p.Targets[i];var b=p.Targets[(i+1)%p.Targets.Length];var d=b-a;if(d.sqrMagnitude>.00001f&&Physics.CapsuleCast(a+Vector3.up*.35f,a+Vector3.up,.34f,d.normalized,d.magnitude,SceneObstacle.MovementMask,QueryTriggerInteraction.Ignore))throw new InvalidOperationException("Cargo2716 blocked authored monster segment: "+p.Name+" "+a+" -> "+b);}
    p.Actor=SpawnMonster(p.Home,p.Kind,p.Name,p.Level);p.Actor.PatrolTargets=(Vector3[])p.Targets.Clone();p.Actor.WaitTime=2;p.Actor.AttackCooldown=2;Map.CargoEnemies2716.Add(p);
   }
  }
 }
}
