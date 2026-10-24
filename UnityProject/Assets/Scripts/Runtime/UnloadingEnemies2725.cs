using System;
using System.Collections.Generic;
using UnityEngine;

namespace GreyHarbor {
 public sealed partial class RaidGame {
  internal void PopulateUnloadingEnemies2725(){
   if(Map==null||(!Map.HarborWhitebox&&!Map.UnloadingInterior2723))return;
   Map.PrepareUnloadingEnemies2725();
   foreach(var plan in Map.UnloadingEnemies2725){
    if(plan.Actor)continue;
    var actor=SpawnMonster(plan.Home,plan.Kind,plan.Name,plan.Level);
    actor.PatrolTargets=(Vector3[])plan.Targets.Clone();
    actor.WaitTime=2;actor.AttackCooldown=2;plan.Actor=actor;
   }
  }
 }
 public sealed partial class RaidMap {
  public readonly List<FreightEnemyPlan2720> UnloadingEnemies2725=new List<FreightEnemyPlan2720>();
  public const float UnloadingEnemyRadius2725=.34f;
  public void PrepareUnloadingEnemies2725(){
   if(UnloadingEnemies2725.Count>0)return;
   if(!HarborWhitebox&&(!UnloadingInterior2723||InteriorId!="unloadingwarehouse"))return;
   Physics.SyncTransforms();
   var prepared=new List<FreightEnemyPlan2720>();
   if(HarborWhitebox){
    UnloadingEnemySelect2725(prepared,"装卸区 / 作业道游荡异种",MonsterKind.Wanderer,
     new Rect(125.5f-79,142-73,5,6.5f),PortPoint(128,145),.7f);
    UnloadingEnemySelect2725(prepared,"装卸区 / 雨棚南侧疾行异种",MonsterKind.Runner,
     new Rect(134.5f-79,139-73,10,3.7f),PortPoint(139.5f,140.5f),.6f);
    UnloadingEnemySelect2725(prepared,"装卸区 / 叉车侧道游荡异种",MonsterKind.Wanderer,
     new Rect(117-79,152-73,7,7.5f),PortPoint(118.5f,154),.6f);
   }else{
    UnloadingEnemySelect2725(prepared,"装卸仓库 / 北侧游荡异种",MonsterKind.Wanderer,
     new Rect(-4,1.5f,3.1f,4),new Vector3(-2.5f,0,3.8f),.7f);
    UnloadingEnemySelect2725(prepared,"装卸仓库 / 北侧疾行异种",MonsterKind.Runner,
     new Rect(.9f,1.5f,3.1f,4),new Vector3(2.5f,0,3.8f),.7f);
   }
   UnloadingEnemies2725.AddRange(prepared);
  }
  // Stable pocket scans preserve furniture and search approaches. All three edges
  // are checked against the real movement capsule and the current map geometry.
  void UnloadingEnemySelect2725(List<FreightEnemyPlan2720> prepared,string name,MonsterKind kind,Rect pocket,Vector3 preferred,float reach){
   var candidates=new List<Vector3>{preferred};
   for(float z=pocket.yMin+.5f;z<=pocket.yMax-.5f;z+=.25f)
    for(float x=pocket.xMin+.5f;x<=pocket.xMax-.5f;x+=.25f)candidates.Add(new Vector3(x,0,z));
   candidates.Sort((a,b)=>{int c=(a-preferred).sqrMagnitude.CompareTo((b-preferred).sqrMagnitude);if(c!=0)return c;c=a.z.CompareTo(b.z);return c!=0?c:a.x.CompareTo(b.x);});
   var nav=new RaidNavigation(pocket,.25f,p=>UnloadingEnemyDomain2725(pocket,p));
   foreach(var home in candidates)foreach(float angle in new[]{0f,90f,180f,270f}){
    var turn=Quaternion.Euler(0,angle,0);
    var points=new[]{home,home+turn*new Vector3(-reach,0,-reach*.55f),home+turn*new Vector3(reach,0,-reach*.55f)};
    bool valid=true;
    for(int i=0;i<points.Length&&valid;i++){
     var a=points[i];var b=points[(i+1)%points.Length];
     if(!UnloadingEnemyDomain2725(pocket,a)||!UnloadingEnemyAccess2725(a)||Physics.CheckCapsule(a+Vector3.up*.35f,a+Vector3.up*1.35f,UnloadingEnemyRadius2725,SceneObstacle.MovementMask,QueryTriggerInteraction.Ignore)){valid=false;break;}
     foreach(var existing in prepared)foreach(var q in existing.Targets)if(Vector3.Distance(a,q)<(HarborWhitebox?4:2)){valid=false;break;}
     if(!valid||!nav.ClearSegment(a,b)){valid=false;break;}
     var delta=b-a;
     if(Physics.CapsuleCast(a+Vector3.up*.35f,a+Vector3.up*1.35f,UnloadingEnemyRadius2725,delta.normalized,delta.magnitude,SceneObstacle.MovementMask,QueryTriggerInteraction.Ignore)){valid=false;break;}
     int samples=Mathf.Max(1,Mathf.CeilToInt(delta.magnitude/.2f));
     for(int j=0;j<=samples;j++){var p=Vector3.Lerp(a,b,j/(float)samples);if(!UnloadingEnemyDomain2725(pocket,p)||!UnloadingEnemyAccess2725(p)){valid=false;break;}}
    }
    if(!valid)continue;
    prepared.Add(new FreightEnemyPlan2720{Name=name,RoomName=HarborWhitebox?"装卸区室外":"收货分拣大厅",Kind=kind,Level=1,Home=home,Targets=points});return;
   }
   throw new InvalidOperationException("Unloading2725 no safe three-point patrol: "+name+" / "+pocket);
  }
  bool UnloadingEnemyDomain2725(Rect pocket,Vector3 p){
   const float margin=.4f;
   if(p.x<pocket.xMin+margin||p.x>pocket.xMax-margin||p.z<pocket.yMin+margin||p.z>pocket.yMax-margin)return false;
   if(HarborWhitebox)return UnloadingBounds2723.Contains(new Vector2(p.x,p.z))&&HarborSurfaceClear(p,UnloadingEnemyRadius2725);
   return UnloadingInterior2723&&InteriorId=="unloadingwarehouse"&&Bounds.Contains(new Vector2(p.x,p.z));
  }
  bool UnloadingEnemyAccess2725(Vector3 p){
   if(HarborWhitebox){
    foreach(var entry in new[]{UnloadingPreviewSpawn2723,UnloadingWarehouseDoor2723,UnloadingWarehouseArrival2723,UnloadingDutyDoor2723,UnloadingDutyArrival2723})if(Vector3.Distance(p,entry)<6)return false;
   }else if(Vector3.Distance(p,Spawn)<6)return false;
   foreach(var door in Doors)if(Vector3.Distance(p,door.Position)<1.5f)return false;
   var searches=HarborWhitebox?UnloadingOutdoorSearch2724:UnloadingSearchRecords2724;
   foreach(var search in searches)if(Vector3.Distance(p,search.Stand)<1.5f||UnloadingEnemyDistance2725(p,search.Stand,search.AccessPoint)<.85f)return false;
   return true;
  }
  public static float UnloadingEnemyDistance2725(Vector3 p,Vector3 a,Vector3 b){
   var delta=b-a;float t=delta.sqrMagnitude>.00001f?Mathf.Clamp01(Vector3.Dot(p-a,delta)/delta.sqrMagnitude):0;
   return Vector3.Distance(p,a+delta*t);
  }
 }
}
