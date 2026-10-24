using UnityEngine;
namespace GreyHarbor {
 public sealed partial class RaidGame {
  void PopulateFreightEnemies2720(){
   if(Map==null||(!Map.HarborWhitebox&&!Map.FreightInterior2716))return;
   Map.PrepareFreightEnemyPlans2720();
   foreach(var plan in Map.FreightEnemies2720){
    if(plan.Actor)continue;
    var actor=SpawnMonster(plan.Home,plan.Kind,plan.Name,plan.Level);
    actor.PatrolTargets=(Vector3[])plan.Targets.Clone();
    actor.WaitTime=2;actor.AttackCooldown=2;plan.Actor=actor;
   }
  }
 }
}
