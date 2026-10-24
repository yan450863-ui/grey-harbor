using UnityEngine;
namespace GreyHarbor {
 public sealed partial class RaidGame {
  // Fixed placements: approx. one monster per 7.4 x 6.6 m booth.
  // Every patrol stays in its own room until the monster detects the player.
  void PopulateInteriorMonsters(){
   if(Map.RepairMain220){PopulateRepairMainEnemies250();}
   else if(Map.InteriorId=="repairannex"){PopulateRepairAnnexEnemies250();}
   else if(Map.FerryWhitebox143){PopulateFerryRebels145();}
   else if(Map.YardInterior138){PopulateYardIndoorMonsters140();}
   else if(Map.InteriorId=="portbooth"||Map.InteriorId=="portservice"){PopulatePortInteriorMonsters();}
   else if(Map.InteriorId=="booth"){
    InteriorMonster("储物间",MonsterKind.Wanderer,2.4f,.7f,2.4f,1.6f,2.8f,.1f);
   }else if(Map.InteriorId=="repair"){
    InteriorMonster("工作区南",MonsterKind.Wanderer,-1.2f,-3.2f,-1.5f,-1,1,-3.4f);
    InteriorMonster("工作区北",MonsterKind.Runner,-2.4f,0,-2.4f,-1.3f,-.4f,.1f);
    InteriorMonster("备件间",MonsterKind.Spitter,1.8f,4,1.5f,5.1f,-.6f,4);
   }else if(Map.InteriorId=="mainhouse"){
    InteriorMonster("办公室西",MonsterKind.Wanderer,-8,-6.2f,-9,-6.2f,-8,-4.3f);
    InteriorMonster("办公室东",MonsterKind.Runner,-4.7f,-8,-4.7f,-5,-6,-5.3f);
    InteriorMonster("休息室",MonsterKind.Wanderer,8.6f,-6.6f,9,-8.2f,8.6f,-4.4f);
    InteriorMonster("工具间西",MonsterKind.Wanderer,-9,0,-8,1.2f,-7,0);
    InteriorMonster("工具间东",MonsterKind.Shell,-5,0,-4.9f,1.5f,-6.2f,.7f);
    InteriorMonster("医务室",MonsterKind.Wanderer,8.7f,0,9.5f,.5f,8.7f,-1);
    InteriorMonster("档案室西",MonsterKind.Wanderer,-9,6.7f,-9,7.7f,-7.6f,6.7f);
    InteriorMonster("档案室东",MonsterKind.Spitter,-5,8,-3.6f,8.4f,-4.8f,5.5f);
    InteriorMonster("主储藏室西",MonsterKind.Runner,5.6f,6.8f,4.9f,6.3f,6.4f,6.3f);
    InteriorMonster("主储藏室东",MonsterKind.Shell,8.6f,7,9.6f,7.7f,8,6.3f);
   }
  }
  void InteriorMonster(string location,MonsterKind kind,float x,float z,float ax,float az,float bx,float bz){
   var e=SpawnMonster(new Vector3(x,0,z),kind,location+" / "+HarborContent.MonsterNames[(int)kind]);
   e.PatrolTargets=new[]{e.Home,new Vector3(ax,0,az),new Vector3(bx,0,bz)};e.WaitTime=2;e.AttackCooldown=2;
  }
  internal bool MonsterBlocksDoor(WorldDoor door){
   if(door.InstantToggle&&!door.Open)return false;
   Rect area=door.Open?door.ClosedSafety:door.OpenSafety;
   foreach(var e in Enemies)if(e&&!e.Dead&&e.gameObject.activeInHierarchy){
    var p=e.transform.position;float dx=p.x-Mathf.Clamp(p.x,area.xMin,area.xMax),dz=p.z-Mathf.Clamp(p.z,area.yMin,area.yMax);
    if(dx*dx+dz*dz<.31f*.31f)return true;
   }return false;
  }
 }
}
