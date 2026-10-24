using System;
using System.Collections.Generic;
using UnityEngine;

namespace GreyHarbor {
 public sealed class FreightEnemyPlan2720 {
  public string Name,RoomName;
  public MonsterKind Kind;
  public int Level;
  public Vector3 Home;
  public Vector3[] Targets;
  public RaidActor Actor;
 }
 public sealed partial class RaidMap {
  public readonly List<FreightEnemyPlan2720> FreightEnemies2720=new List<FreightEnemyPlan2720>();
  const float FreightEnemyRadius2720=.34f;

  // The selected pockets are authored against the 2.7.19 furniture and equipment.
  // A blocked pocket fails explicitly; it never moves furniture or spills into another room.
  public void PrepareFreightEnemyPlans2720(){
   if(FreightEnemies2720.Count>0)return;
   if(!HarborWhitebox&&!FreightInterior2716)return;
   Physics.SyncTransforms();
   var prepared=new List<FreightEnemyPlan2720>();
   if(HarborWhitebox){
    FreightEnemySelect2720(prepared,"货运场 / 北侧周转区游荡异种","北侧周转区",MonsterKind.Wanderer,1,
     FreightRect2716(13.5f,169.8f,5,3.4f),PortPoint(16,171.5f),Vector3.right,1.1f,null);
    FreightEnemySelect2720(prepared,"货运场 / 库房东侧游荡异种","库房东侧作业道",MonsterKind.Wanderer,1,
     FreightRect2716(45.8f,168.2f,2.6f,4.6f),PortPoint(47,170.5f),Vector3.forward,1.1f,null);
    FreightEnemySelect2720(prepared,"货运场 / 东南作业道疾行异种","东南作业道",MonsterKind.Runner,1,
     FreightRect2716(45.2f,145.7f,2.6f,3.6f),PortPoint(46.5f,147.5f),Vector3.forward,1.0f,null);
    FreightEnemySelect2720(prepared,"货运场 / 西北围栏游荡异种","西北周转角",MonsterKind.Wanderer,1,
     FreightRect2716(5.6f,168.5f,6,6.7f),PortPoint(7.5f,173),Vector3.forward,.65f,null,true);
    FreightEnemySelect2720(prepared,"货运场 / 库后疾行异种","库后周转道",MonsterKind.Runner,1,
     FreightRect2716(27,170,10,5.2f),PortPoint(31,174),Vector3.right,.65f,null,true);
    FreightEnemySelect2720(prepared,"货运场 / 西侧作业道游荡异种","库房西侧作业道",MonsterKind.Wanderer,1,
     FreightRect2716(23.7f,157,4,9.5f),PortPoint(26,162),Vector3.forward,.65f,null,true);
    FreightEnemySelect2720(prepared,"货运场 / 东侧楼间疾行异种","东侧楼间作业道",MonsterKind.Runner,1,
     FreightRect2716(45.3f,152,4.4f,9),PortPoint(49,157),Vector3.forward,.65f,null,true);
   }else if(InteriorId=="freightduty"){
    var room=FreightRooms2716.Find(r=>r.Name=="货运资料室");
    if(room==null)throw new InvalidOperationException("Freight2720 missing authored room: 货运资料室");
    // Below the reading table/chair, east of the searchable archive cabinet.
    FreightEnemySelect2720(prepared,"货运值班房 / 资料室游荡异种",room.Name,MonsterKind.Wanderer,1,
     new Rect(-7,1.4f,4.4f,2),new Vector3(-5,0,2.3f),Vector3.right,1.1f,room);
    FreightEnemyRoom2722(prepared,"值班办公室",MonsterKind.Wanderer,1,new Vector3(-4.7f,0,-5),4.5f);
    FreightEnemyRoom2722(prepared,"员工休息区",MonsterKind.Runner,1,new Vector3(5.5f,0,-4.6f),4.5f);
    FreightEnemyRoom2722(prepared,"卫生间",MonsterKind.Wanderer,2,new Vector3(4.3f,0,3.3f),6);
   }else if(InteriorId=="freightwarehouse"){
    // Open hall west of the packing bench, away from receiving-room door/search approaches.
    FreightEnemySelect2720(prepared,"货运库房 / 通间蓝级喷吐异种","仓储通间",MonsterKind.Spitter,2,
     new Rect(-2.4f,2.3f,2.4f,3.5f),new Vector3(-1.2f,0,4.1f),Vector3.forward,.9f,null);
    FreightEnemyRoom2722(prepared,"货物收发室",MonsterKind.Wanderer,1,new Vector3(-5.5f,0,3.6f),9);
    FreightEnemySelect2720(prepared,"货运库房 / 西侧通间疾行异种","仓储通间",MonsterKind.Runner,1,
     new Rect(-7.7f,-.8f,4.2f,3),new Vector3(-6.3f,0,1),Vector3.right,.65f,null,true);
    FreightEnemySelect2720(prepared,"货运库房 / 东侧通间游荡异种","仓储通间",MonsterKind.Wanderer,2,
     new Rect(3.5f,-.3f,4.3f,3),new Vector3(6.5f,0,1.2f),Vector3.right,.65f,null,true);
   }
   FreightEnemies2720.AddRange(prepared);
  }

  void FreightEnemyRoom2722(List<FreightEnemyPlan2720> prepared,string roomName,MonsterKind kind,int level,Vector3 preferred,float spawnBuffer){
   var room=FreightRooms2716.Find(r=>r.Name==roomName);
   if(room==null)throw new InvalidOperationException("Freight2722 missing authored room: "+roomName);
   FreightEnemySelect2720(prepared,"货运室内 / "+roomName+(level==2?"蓝级":"绿级")+kind,roomName,kind,level,
    room.Area,preferred,Vector3.right,.65f,room,true,spawnBuffer);
  }
  // New patrols search only the authored area, with stable nearest-first candidates.
  // Closed doors, physical capsules and searchable approaches remain authoritative.
  IEnumerable<Vector3> FreightEnemyCandidates2722(Rect pocket,Vector3 preferred,bool scan){
   foreach(var offset in new[]{Vector2.zero,new Vector2(.2f,0),new Vector2(-.2f,0),new Vector2(0,.2f),new Vector2(0,-.2f),
    new Vector2(.4f,0),new Vector2(-.4f,0),new Vector2(0,.4f),new Vector2(0,-.4f)})yield return preferred+new Vector3(offset.x,0,offset.y);
   if(!scan)yield break;
   var candidates=new List<Vector3>();
   for(float z=pocket.yMin+.55f;z<=pocket.yMax-.55f;z+=.35f)
    for(float x=pocket.xMin+.55f;x<=pocket.xMax-.55f;x+=.35f)candidates.Add(new Vector3(x,0,z));
   candidates.Sort((a,b)=>{int order=(a-preferred).sqrMagnitude.CompareTo((b-preferred).sqrMagnitude);if(order!=0)return order;order=a.z.CompareTo(b.z);return order!=0?order:a.x.CompareTo(b.x);});
   foreach(var p in candidates)yield return p;
  }
  void FreightEnemySelect2720(List<FreightEnemyPlan2720> prepared,string name,string roomName,MonsterKind kind,int level,
   Rect pocket,Vector3 preferred,Vector3 axis,float reach,FreightRoom2716 room,bool scan=false,float spawnBuffer=0){
   // Population can precede global navigation creation. A small local navigation instance
   // checks these exact authored segments without consulting the previous map's navigation.
   var navigation=new RaidNavigation(pocket,.25f,p=>FreightEnemyDomain2720(pocket,p,room));
   foreach(var home in FreightEnemyCandidates2722(pocket,preferred,scan))foreach(var patrolAxis in scan?new[]{axis,axis==Vector3.right?Vector3.forward:Vector3.right}:new[]{axis}){
    var points=new[]{home,home-patrolAxis*reach,home+patrolAxis*reach};bool clear=true;
    foreach(var p in points)if(!FreightEnemyPoint2720(pocket,p,room,prepared,spawnBuffer)){clear=false;break;}
    if(!clear)continue;
    for(int i=0;i<points.Length;i++){
     var a=points[i];var b=points[(i+1)%points.Length];var delta=b-a;
     if(!navigation.ClearSegment(a,b)||!FreightEnemySegment2720(pocket,a,b,room,spawnBuffer)
      ||Physics.CapsuleCast(a+Vector3.up*.35f,a+Vector3.up*1.35f,FreightEnemyRadius2720,
       delta.normalized,delta.magnitude,SceneObstacle.MovementMask,QueryTriggerInteraction.Ignore)){clear=false;break;}
    }
    if(!clear)continue;
    prepared.Add(new FreightEnemyPlan2720{Name=name,RoomName=roomName,Kind=kind,Level=level,Home=home,Targets=points});return;
   }
   throw new InvalidOperationException("Freight2720 no safe three-point patrol in authored pocket: "+name+
    " / "+roomName+" / "+pocket+"; preserve 2.7.19 furniture, closed walls, entrances and search access.");
  }
  bool FreightEnemyDomain2720(Rect pocket,Vector3 p,FreightRoom2716 room){
   const float margin=.4f;
   if(p.x<pocket.xMin+margin||p.x>pocket.xMax-margin||p.z<pocket.yMin+margin||p.z>pocket.yMax-margin)return false;
   var v=new Vector2(p.x,p.z);
   if(HarborWhitebox)return FreightExteriorBounds2716.Contains(v)&&HarborSurfaceClear(p,FreightEnemyRadius2720);
   if(!Bounds.Contains(v))return false;
   if(room!=null)return p.x>=room.Area.xMin+.5f&&p.x<=room.Area.xMax-.5f&&p.z>=room.Area.yMin+.5f&&p.z<=room.Area.yMax-.5f;
   foreach(var closedRoom in FreightRooms2716)if(closedRoom.Area.Contains(v))return false;
   return true;
  }
  bool FreightEnemyPoint2720(Rect pocket,Vector3 p,FreightRoom2716 room,List<FreightEnemyPlan2720> prepared,float spawnBuffer=0){
   if(!FreightEnemyDomain2720(pocket,p,room)||!FreightEnemyAccess2720(p,spawnBuffer))return false;
   if(Physics.CheckCapsule(p+Vector3.up*.35f,p+Vector3.up*1.35f,FreightEnemyRadius2720,
    SceneObstacle.MovementMask,QueryTriggerInteraction.Ignore))return false;
   foreach(var plan in prepared)foreach(var q in plan.Targets)if(Vector3.Distance(p,q)<(HarborWhitebox?8:2))return false;
   return true;
  }
  bool FreightEnemyAccess2720(Vector3 p,float spawnBuffer=0){
   if(HarborWhitebox){
    // All outdoor patrols begin beyond preview flashlight range; gate and portal approaches stay free.
    if(Vector3.Distance(p,FreightPreviewSpawn2716)<16)return false;
    foreach(var entry in new[]{FreightPreviewSpawn2716,FreightExitPoint2716,FreightDutyDoor2716,FreightDutyArrival2716,
     FreightWarehouseDoor2716,FreightWarehouseArrival2716})if(Vector3.Distance(p,entry)<6)return false;
    foreach(var search in FreightSearchRecords2716)
     if(Vector3.Distance(p,search.Stand)<1.5f||FreightEnemyDistance2720(p,search.Stand,search.AccessPoint)<.85f)return false;
   }else{
    if(Vector3.Distance(p,Spawn)<(spawnBuffer>0?spawnBuffer:(InteriorId=="freightwarehouse"?9:6)))return false;
    foreach(var door in Doors)if(Vector3.Distance(p,door.Position)<1.5f)return false;
    foreach(var search in FreightLootRecords2717)
     if(Vector3.Distance(p,search.Stand)<1.5f||FreightEnemyDistance2720(p,search.Stand,search.AccessPoint)<.85f)return false;
   }
   return true;
  }
  bool FreightEnemySegment2720(Rect pocket,Vector3 a,Vector3 b,FreightRoom2716 room,float spawnBuffer=0){
   int samples=Mathf.Max(1,Mathf.CeilToInt(Vector3.Distance(a,b)/.2f));
   for(int i=0;i<=samples;i++){var p=Vector3.Lerp(a,b,i/(float)samples);if(!FreightEnemyDomain2720(pocket,p,room)||!FreightEnemyAccess2720(p,spawnBuffer))return false;}
   return true;
  }
  static float FreightEnemyDistance2720(Vector3 p,Vector3 a,Vector3 b){
   var delta=b-a;float along=delta.sqrMagnitude>.00001f?Mathf.Clamp01(Vector3.Dot(p-a,delta)/delta.sqrMagnitude):0;
   return Vector3.Distance(p,a+delta*along);
  }
 }
}
