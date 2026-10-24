using System;
using System.Collections.Generic;
using UnityEngine;
namespace GreyHarbor {
 public sealed class OfficeSoldierRecord2747 {
  public string Tower,RoomId,RoomName;public int Floor,Group;public RebelKind Kind;
  public Vector3 Home;public Vector3[] Targets;public RaidActor Actor;public bool UsedFallback;
 }
 public sealed partial class RaidMap {
  public readonly List<OfficeSoldierRecord2747> OfficeSoldierRecords2747=new List<OfficeSoldierRecord2747>();
  public bool OfficeSoldierPointSafe2747(OfficeRoom2726 room,Vector3 p){
   if(room==null||room.Stair||(!OfficeLeft2741&&OfficeFloor2733==3&&room.Id=="10")||(OfficeLeft2741&&OfficeFloor2733==1&&room.Id=="06"))return false;
   foreach(var landing in OfficeFallReservedPoints2748())if(Vector3.Distance(p,landing)<2f)return false;
   const float margin=.65f;
   if(p.x<room.Area.xMin+margin||p.x>room.Area.xMax-margin||p.z<room.Area.yMin+margin||p.z>room.Area.yMax-margin)return false;
   if(Physics.CheckCapsule(p+Vector3.up*.35f,p+Vector3.up*1.35f,.34f,SceneObstacle.MovementMask,QueryTriggerInteraction.Ignore))return false;
   foreach(var d in Doors)if(Vector3.Distance(p,d.Position)<(d.Portal?5f:2.7f))return false;
   foreach(bool north in new[]{false,true})if(Vector3.Distance(p,OfficeStairArrival2733(north))<5)return false;
   if(!OfficeLeft2741&&OfficeFloor2733==1&&Vector3.Distance(p,Spawn)<5)return false;
   foreach(var r in OfficeSearchRecords2736)if(Vector3.Distance(p,r.Stand)<1f||UnloadingEnemyDistance2725(p,r.Stand,r.AccessPoint)<.85f)return false;
   return true;
  }
 }
 public sealed partial class RaidGame {
  public bool OfficeSoldierPatrolClear2747(OfficeRoom2726 room,Vector3 a,Vector3 b){
   if(!Map.OfficeSoldierPointSafe2747(room,a)||!Map.OfficeSoldierPointSafe2747(room,b)||Navigation==null||!Navigation.ClearSegment(a,b))return false;
   var delta=b-a;if(delta.sqrMagnitude>.0001f&&Physics.CapsuleCast(a+Vector3.up*.35f,a+Vector3.up*1.35f,.34f,delta.normalized,delta.magnitude,SceneObstacle.MovementMask,QueryTriggerInteraction.Ignore))return false;
   int n=Mathf.Max(1,Mathf.CeilToInt(delta.magnitude/.2f));for(int i=0;i<=n;i++)if(!Map.OfficeSoldierPointSafe2747(room,Vector3.Lerp(a,b,i/(float)n)))return false;
   return true;
  }
  public void SpawnOfficeSoldiers2747(){
   if(Map==null||!Map.OfficeWhitebox2726||Map.IsOfficeBridge2741)return;
   if(Map.OfficeSoldierRecords2747.Count>0)return;
   Physics.SyncTransforms();int floor=Map.OfficeFloor2733;bool left=Map.OfficeLeft2741;
   RebelKind[][] roster={
    new[]{RebelKind.Regular,RebelKind.Smg,RebelKind.Heavy,RebelKind.Smg},
    new[]{RebelKind.Regular,RebelKind.Smg,RebelKind.BlueRegular,RebelKind.Marksman,RebelKind.Smg},
    new[]{RebelKind.BlueRegular,RebelKind.Smg,RebelKind.BlueHeavy,RebelKind.BlueMarksman,RebelKind.Regular,RebelKind.BlueSmg}
   };
   if(left)roster=new[]{
    new[]{RebelKind.Regular,RebelKind.Smg,RebelKind.Heavy,RebelKind.BlueRegular},
    new[]{RebelKind.Smg,RebelKind.BlueRegular,RebelKind.Marksman,RebelKind.Regular,RebelKind.BlueSmg},
    new[]{RebelKind.BlueRegular,RebelKind.BlueSmg,RebelKind.Heavy,RebelKind.BlueMarksman,RebelKind.Regular,RebelKind.BlueHeavy}
   };
   var prepared=new List<OfficeSoldierRecord2747>();int first=floor==3?3:2;
   string[] rooms={"03",left?(floor==3?"06":"05"):"07"};
   for(int group=0;group<2;group++){
    var room=Map.OfficeRooms2726.Find(r=>r.Id==rooms[group]);int offset=group==0?0:first,count=group==0?first:roster[floor-1].Length-first;
    PrepareOfficeSquad2747(prepared,room,roster[floor-1],offset,count,group+1);
   }
   foreach(var record in prepared){
    record.Actor=SpawnRebel145(record.Home,record.Kind,"办公"+record.Tower+"塔"+floor+"楼 / "+record.RoomName+"小组"+record.Group);
    record.Actor.Home=record.Home;record.Actor.PatrolTargets=(Vector3[])record.Targets.Clone();record.Actor.WaitTime=2;record.Actor.AttackCooldown=2;
    record.Actor.Aim=(new Vector3(0,0,record.Home.z)-record.Home).normalized;
   }
   Map.OfficeSoldierRecords2747.AddRange(prepared);
  }
  void PrepareOfficeSquad2747(List<OfficeSoldierRecord2747> all,OfficeRoom2726 room,RebelKind[] kinds,int offset,int count,int group){
   if(room==null)throw new InvalidOperationException("2747 squad room missing "+group);
   var candidates=new List<Vector3>();
   for(float z=room.Area.yMin+.8f;z<room.Area.yMax-.8f;z+=.6f)for(float x=room.Area.xMin+.8f;x<room.Area.xMax-.8f;x+=.6f){var p=new Vector3(x,0,z);if(Map.OfficeSoldierPointSafe2747(room,p))candidates.Add(p);}
   candidates.Sort((a,b)=>{int c=(a-room.Center).sqrMagnitude.CompareTo((b-room.Center).sqrMagnitude);return c!=0?c:a.z!=b.z?a.z.CompareTo(b.z):a.x.CompareTo(b.x);});
   foreach(var anchor in candidates){
    var near=new List<Vector3>(candidates);near.Sort((a,b)=>{int c=(a-anchor).sqrMagnitude.CompareTo((b-anchor).sqrMagnitude);return c!=0?c:a.z!=b.z?a.z.CompareTo(b.z):a.x.CompareTo(b.x);});
    var trial=new List<OfficeSoldierRecord2747>();
    foreach(var home in near){
     if(Vector3.Distance(home,anchor)>4.5f)break;bool spaced=true;
     foreach(var existing in trial)if(Vector3.Distance(home,existing.Home)<1.6f){spaced=false;break;}if(!spaced)continue;
     var kind=kinds[offset+trial.Count];bool sniper=SoldierProfiles2745.IsMarksman(kind);Vector3 target=home;bool found=sniper;
     if(!sniper)foreach(float angle in new[]{0f,90f,180f,270f,45f,135f,225f,315f}){
      var p=home+Quaternion.Euler(0,angle,0)*Vector3.forward*.8f;
      if(!OfficeSoldierPatrolClear2747(room,home,p))continue;bool gap=true;
      foreach(var existing in trial)foreach(var q in existing.Targets)if(Vector3.Distance(p,q)<1.3f){gap=false;break;}
      if(!gap)continue;target=p;found=true;break;
     }
     if(!found)continue;
     bool cross=true;foreach(var existing in trial)foreach(var q in existing.Targets)if(Vector3.Distance(home,q)<1.3f){cross=false;break;}if(!cross)continue;
     trial.Add(new OfficeSoldierRecord2747{Tower=Map.OfficeLeft2741?"左":"右",Floor=Map.OfficeFloor2733,Group=group,RoomId=room.Id,RoomName=room.Name,Kind=kind,Home=home,Targets=new[]{home,target},UsedFallback=Vector3.Distance(anchor,room.Center)>2});
     if(trial.Count==count){all.AddRange(trial);return;}
    }
   }
   throw new InvalidOperationException("2747 no safe grouped soldier placement: "+Map.InteriorId+" / "+room.Name+" / expected "+count+", no partial squad spawned");
  }
 }
}
