using UnityEngine;
namespace GreyHarbor {
 public sealed partial class RaidGame {
  // Closing is atomic: test the final leaf position and a short local escape first.
  internal Bounds ClosedDoorBounds2729(WorldDoor door){
   Quaternion rotation=door.Pivot.localRotation;
   try{door.Pivot.localRotation=Quaternion.Euler(0,door.ClosedYaw,0);Physics.SyncTransforms();return door.Collider.bounds;}
   finally{door.Pivot.localRotation=rotation;Physics.SyncTransforms();}
  }
  bool DoorStepClear2729(Vector3 p,Collider leaf){
   if(!Map.Bounds.Contains(new Vector2(p.x,p.z)))return false;
   if(Map.HarborWhitebox&&!Map.HarborSurfaceClear(p,.34f))return false;
   if(Map.UndergroundWhitebox2752&&!Map.UndergroundFloorContains2752(p,.34f))return false;
   foreach(var col in Physics.OverlapCapsule(p+Vector3.up*.35f,p+Vector3.up,.34f,SceneObstacle.MovementMask|(1<<9),QueryTriggerInteraction.Ignore)){
    if(col==leaf||col==Player.HitCollider||col.transform.IsChildOf(Player.transform))continue;
    return false;
   }
   return true;
  }
  internal bool ClearPlayerForDoorClose2729(WorldDoor door){
   if(!door.Pivot||!door.Collider)return false;
   var bounds=ClosedDoorBounds2729(door);var area=new Rect(bounds.min.x,bounds.min.z,bounds.size.x,bounds.size.z);
   Vector3 origin=Player.transform.position;
   if(!DoorFootprintOccupied222(area,new Vector2(origin.x,origin.z),.34f))return true;
   bool vertical=bounds.size.z>=bounds.size.x;
   var normal=vertical?Vector3.right:Vector3.forward;var tangent=vertical?Vector3.forward:Vector3.right;
   float side=Vector3.Dot(origin-bounds.center,normal),half=vertical?bounds.extents.x:bounds.extents.z;
   float preferred=Mathf.Abs(side)>.02f?Mathf.Sign(side):(Vector3.Dot(Player.Aim,normal)>0?-1:1);
   Vector3 best=origin;float bestDistance=float.PositiveInfinity;
   foreach(float sign in new[]{preferred,-preferred}){
    // Never push a player who is already on one side through to the other room.
    if(Mathf.Abs(side)>.02f&&sign!=preferred)continue;
    foreach(float offset in new[]{0f,.15f,-.15f,.30f,-.30f,.50f,-.50f,.75f,-.75f}){
     var target=origin+normal*(sign*(half+.36f)-side)+tangent*offset;
     float distance=Vector3.Distance(origin,target);if(distance>1.2f||distance>=bestDistance)continue;
     if(DoorFootprintOccupied222(area,new Vector2(target.x,target.z),.34f)||!DoorStepClear2729(target,door.Collider))continue;
     bool clear=true;int steps=Mathf.Max(1,Mathf.CeilToInt(distance/.06f));
     for(int i=0;i<=steps;i++)if(!DoorStepClear2729(Vector3.Lerp(origin,target,i/(float)steps),door.Collider)){clear=false;break;}
     if(clear){best=target;bestDistance=distance;}
    }
   }
   // If both nearby sides are obstructed, retain the open door and player position.
   if(float.IsPositiveInfinity(bestDistance))return false;
   Player.transform.position=best;Physics.SyncTransforms();Rig.Tick(0);Vision.Refresh(best,Player.Aim);return true;
  }
 }
}
