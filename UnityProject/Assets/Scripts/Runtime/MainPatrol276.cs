using System;
using System.Collections.Generic;
using UnityEngine;
namespace GreyHarbor {
 public sealed class MainPatrolPlan276 {
  public string RoomName,Label;public bool Soldier;public RebelKind Rebel;public MonsterKind Monster;public int Level=1;
  public Vector3 Home;public Vector3[] Targets;
 }
 public sealed partial class RaidMap {
  public readonly List<MainPatrolPlan276> MainPatrolPlans276=new List<MainPatrolPlan276>();
  bool MainPatrolPointClear276(Vector3 p){
   if(!RepairMainNet210.Contains(new Vector2(p.x,p.z)))return false;
   return !Physics.CheckCapsule(p+Vector3.up*.35f,p+Vector3.up,.34f,SceneObstacle.MovementMask,QueryTriggerInteraction.Ignore);
  }
  bool MainPatrolSegmentClear276(Vector3 a,Vector3 b){
   var delta=b-a;if(!MainPatrolPointClear276(a)||!MainPatrolPointClear276(b))return false;
   return !Physics.CapsuleCast(a+Vector3.up*.35f,a+Vector3.up,.34f,delta.normalized,delta.magnitude,SceneObstacle.MovementMask,QueryTriggerInteraction.Ignore);
  }
  bool MainPatrolRoomContains276(Rect area,Vector3 p,bool hall){
   if(p.x<area.xMin+.55f||p.x>area.xMax-.55f||p.z<area.yMin+.55f||p.z>area.yMax-.55f)return false;
   if(hall)foreach(var room in RepairMainRooms210)if(room.Area.Contains(new Vector2(p.x,p.z)))return false;
   foreach(var door in Doors)if(Vector3.Distance(p,door.Position)<1.4f)return false;
   foreach(var plan in MainPatrolPlans276)foreach(var q in plan.Targets)if(Vector3.Distance(p,q)<2.1f)return false;
   return true;
  }
  void AddMainPatrol276(string roomName,string label,bool soldier,RebelKind rebel,MonsterKind monster,int level){
   bool hall=roomName=="维修大厅";var room=RepairMainRooms210.Find(r=>r.Name==roomName);
   IEnumerable<Rect> areas=hall?RepairMainPublicAreas276(RepairFloor220):new[]{room!=null?room.Area:RepairMainRoomArea276(RepairFloor220,roomName)};
   foreach(var area in areas)foreach(float fz in new[]{.35f,.65f,.5f,.2f,.8f,.1f,.9f})foreach(float fx in new[]{.35f,.65f,.5f,.2f,.8f}){
    var home=new Vector3(Mathf.Lerp(area.xMin,area.xMax,fx),0,Mathf.Lerp(area.yMin,area.yMax,fz));
    foreach(var axis in new[]{Vector3.forward,Vector3.right}){
     var a=home-axis*1.3f;var b=home+axis*1.3f;
     if(!MainPatrolRoomContains276(area,home,hall)||!MainPatrolRoomContains276(area,a,hall)||!MainPatrolRoomContains276(area,b,hall))continue;
     if(!MainPatrolSegmentClear276(a,b))continue;
     MainPatrolPlans276.Add(new MainPatrolPlan276{RoomName=roomName,Label=label,Soldier=soldier,Rebel=rebel,Monster=monster,Level=level,Home=home,Targets=new[]{home,a,b}});return;
    }
   }
   throw new InvalidOperationException("No clear NPC patrol in floor "+RepairFloor220+" room "+roomName);
  }
  public void PrepareMainPatrol276(){
   if(MainPatrolPlans276.Count>0)return;Physics.SyncTransforms();
   switch(RepairFloor220){
    case 1:
     AddMainPatrol276("维修大厅","一楼检修区南",false,RebelKind.Regular,MonsterKind.Runner,1);
     AddMainPatrol276("维修大厅","一楼检修大厅",true,RebelKind.Regular,MonsterKind.Wanderer,1);
     AddMainPatrol276("零件整理与待修件间","一楼备件间",false,RebelKind.Regular,MonsterKind.Wanderer,1);break;
    case 2:
     AddMainPatrol276("部件拆修车间","二楼拆修车间",false,RebelKind.Regular,MonsterKind.Wanderer,1);
     AddMainPatrol276("维修工艺与备件整理间","二楼备件库",true,RebelKind.BlueRegular,MonsterKind.Wanderer,2);break;
    case 3:
     AddMainPatrol276("技术设计办公室","三楼技术办公室",true,RebelKind.Regular,MonsterKind.Wanderer,1);
     AddMainPatrol276("继电与仪表测试室","三楼继电器室",false,RebelKind.Regular,MonsterKind.Spitter,2);break;
    case 4:
     AddMainPatrol276("人事与排班办公室","四楼排班办公室",false,RebelKind.Regular,MonsterKind.Wanderer,1);
     AddMainPatrol276("管理办公室","四楼管理室",true,RebelKind.Heavy,MonsterKind.Wanderer,1);
     AddMainPatrol276("职工休息室","四楼休息室",false,RebelKind.Regular,MonsterKind.Runner,2);break;
   }
  }
 }
}
