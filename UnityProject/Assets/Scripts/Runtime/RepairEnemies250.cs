using UnityEngine;
namespace GreyHarbor {public sealed partial class RaidGame {
 void RepairSoldier250(string label,RebelKind kind,Vector3 home,Vector3 a,Vector3 b){var e=SpawnRebel145(home,kind,"维修区 / "+label);e.PatrolTargets=new[]{home,a,b};e.AttackCooldown=2;e.WaitTime=2;}
 void RepairSoldier250(string label,RebelKind kind,float x,float z,float ax,float az,float bx,float bz){RepairSoldier250(label,kind,new Vector3(x,0,z),new Vector3(ax,0,az),new Vector3(bx,0,bz));}
 void PopulateRepairExteriorEnemies250(){
  RepairSoldier250("外围南段巡逻",RebelKind.Regular,RaidMap.PortPoint(6,75),RaidMap.PortPoint(6,72),RaidMap.PortPoint(6,78));
  var rear=SpawnMonster(RaidMap.PortPoint(22,124),MonsterKind.Runner,"维修区 / 后场疾行异种");rear.PatrolTargets=new[]{rear.Home,RaidMap.PortPoint(20,124),RaidMap.PortPoint(24,124)};rear.WaitTime=2;rear.AttackCooldown=2;
  RepairSoldier250("外围巡逻",RebelKind.Regular,RaidMap.PortPoint(45,95),RaidMap.PortPoint(45,92),RaidMap.PortPoint(45,97));
  var e=SpawnMonster(RaidMap.PortPoint(6,100),MonsterKind.Wanderer,"维修区 / 西侧游荡异种");e.PatrolTargets=new[]{e.Home,RaidMap.PortPoint(6,96),RaidMap.PortPoint(6,105)};e.WaitTime=2;e.AttackCooldown=2;
  PopulateCargoEnemies2716();
 }
 void PopulateRepairAnnexEnemies250(){
  RepairSoldier250("附属房工作厅",RebelKind.Regular,-1,3,-1,1,1,4.5f);
  InteriorMonster("维修区 / 附属房工具间",MonsterKind.Runner,6.2f,4.2f,5.6f,3.9f,6.4f,4.2f);
 }
 void RepairMainSoldier273(string label,RebelKind kind,float x,float z,float ax,float az,float bx,float bz){
  RepairSoldier250(label,kind,RaidMap.MainPoint273(new Vector3(x,0,z)),RaidMap.MainPoint273(new Vector3(ax,0,az)),RaidMap.MainPoint273(new Vector3(bx,0,bz)));
 }
 void RepairMainMonster273(string location,MonsterKind kind,float x,float z,float ax,float az,float bx,float bz){
  var home=RaidMap.MainPoint273(new Vector3(x,0,z));var a=RaidMap.MainPoint273(new Vector3(ax,0,az));var b=RaidMap.MainPoint273(new Vector3(bx,0,bz));
  InteriorMonster(location,kind,home.x,home.z,a.x,a.z,b.x,b.z);
 }
 void PopulateRepairMainEnemies250(){
  Map.PrepareMainPatrol276();
  foreach(var p in Map.MainPatrolPlans276){
   var e=p.Soldier?SpawnRebel145(p.Home,p.Rebel,"维修区 / "+p.Label):SpawnMonster(p.Home,p.Monster,"维修区 / "+p.Label,p.Level);
   e.PatrolTargets=(Vector3[])p.Targets.Clone();e.WaitTime=2;e.AttackCooldown=2;
  }
 }
}}
