using System;
using System.Collections.Generic;
using UnityEngine;
namespace GreyHarbor {
 public sealed class MainSearchPlacement274 {
  public LootContainer Box;public GameObject Support;public bool WallAligned,ExistingSupport;public string RoomName;public Rect RoomArea;
  public Vector3 WallPoint,Inward;public Bounds ClosedBounds;
 }
 public sealed partial class RaidMap {
  public readonly List<MainSearchPlacement274> MainSearchPlacements274=new List<MainSearchPlacement274>();
  static Bounds MainSearchBounds274(LootContainer box,GameObject support){
   var renderers=box.Prop.GetComponentsInChildren<Renderer>();var bounds=new Bounds(box.Prop.transform.position,Vector3.zero);
   bool first=true;foreach(var r in renderers){if(first){bounds=r.bounds;first=false;}else bounds.Encapsulate(r.bounds);}
   if(support)foreach(var r in support.GetComponentsInChildren<Renderer>())bounds.Encapsulate(r.bounds);return bounds;
  }
  void MainMoveSearch274(LootContainer box,GameObject support,Vector3 shift){box.Prop.transform.position+=shift;if(support)support.transform.position+=shift;Physics.SyncTransforms();}
  static Bounds MainInteractionBounds274(LootContainer box){
   foreach(string name in new[]{"CaseBody","WallCabinet","CoatBody","Body"}){var t=box.Prop.transform.Find(name);if(t){var renderer=t.GetComponent<Renderer>();if(renderer)return renderer.bounds;}}
   return new Bounds(box.Position+Vector3.up*.3f,new Vector3(.7f,.6f,.55f));
  }
  static float MainContactDistance274(Bounds b,Vector3 stand){
   var contact=new Vector3(Mathf.Clamp(stand.x,b.min.x,b.max.x),0,Mathf.Clamp(stand.z,b.min.z,b.max.z));return Vector3.Distance(contact,stand);
  }
  bool MainSearchSlot274(LootContainer box,GameObject support,Bounds bounds,Vector3 shift,Vector3 stand){
   if(TryMainExtraPlan2711(box.Name,out var extra)){var moved=bounds;moved.center+=shift;if(MainExtraSlotProtected2711(moved))return false;}
   // A physically empty gap alone can still let a deeper neighbouring crate win F selection.
   // Keep each assigned search stand outside the other container's actual interaction range.
   var contactBounds=MainInteractionBounds274(box);contactBounds.center+=shift;
   foreach(var placement in MainSearchPlacements274){
    if(MainContactDistance274(MainInteractionBounds274(placement.Box),stand)<RaidGame.InteractionRange+.15f)return false;
    if(MainContactDistance274(contactBounds,placement.Box.Position)<RaidGame.InteractionRange+.15f)return false;
   }
   foreach(var door in Doors)if(Vector3.Distance(door.Position,stand)<RaidGame.InteractionRange+.15f)return false;
   var center=bounds.center+shift;var half=bounds.extents;half.x+=.02f;half.z+=.02f;
   foreach(var c in Physics.OverlapBox(center,half,Quaternion.identity,SceneObstacle.MovementMask,QueryTriggerInteraction.Ignore))
    if(!c.transform.IsChildOf(box.Prop.transform)&&(!support||!c.transform.IsChildOf(support.transform)))return false;
   foreach(var c in Physics.OverlapCapsule(stand+Vector3.up*.35f,stand+Vector3.up,.34f,SceneObstacle.MovementMask,QueryTriggerInteraction.Ignore))
    if(!c.transform.IsChildOf(box.Prop.transform)&&(!support||!c.transform.IsChildOf(support.transform)))return false;
   return true;
  }
  void PlaceMainSearch274(LootContainer box,GameObject support,bool existingSupport,Vector3 legacy){
   Physics.SyncTransforms();
   if(existingSupport){
    var serviceDirection=box.Prop.transform.rotation*Vector3.back;box.Position=box.Prop.transform.position+serviceDirection*1.35f;box.Position.y=0;
    MainSearchPlacements274.Add(new MainSearchPlacement274{Box=box,Support=support,RoomName=MainSearchRoom276(box.Name),RoomArea=MainSearchArea276(box.Name),ExistingSupport=true,Inward=serviceDirection,ClosedBounds=MainSearchBounds274(box,support)});return;
   }
   string roomName=MainSearchRoom276(box.Name);var room=RepairMainRooms210.Find(r=>r.Name==roomName);
   bool north=room==null&&box.Name=="巡检弹药箱";
   Rect area=MainSearchArea276(box.Name);
   float side=legacy.x<0?-1:1;if(box.Name=="楼层急救柜")side=-1;if(box.Name=="楼层装备补给箱")side=1;float yaw=north?0:side<0?270:90;
   box.Prop.transform.rotation=Quaternion.Euler(0,yaw,0);if(support)support.transform.rotation=Quaternion.Euler(0,yaw,0);Physics.SyncTransforms();
   var inward=north?Vector3.back:side<0?Vector3.right:Vector3.left;
   var b=MainSearchBounds274(box,support);float edge=north?area.yMax:side<0?area.xMin:area.xMax;
   if(north){if(Mathf.Abs(edge-MainNetHalfZ276)>.02f)edge-=.1f;}
   // The public hall rectangle already starts at the divider's inner face.
   else if(!(TryMainExtraPlan2711(box.Name,out var extra)&&extra.RoomName=="维修大厅")&&Mathf.Abs(Mathf.Abs(edge)-MainNetHalfX276)>.02f)edge+=side<0?.1f:-.1f;
   Vector3 shift=north?Vector3.forward*(edge-.08f-b.max.z):Vector3.right*(side<0?edge+.08f-b.min.x:edge-.08f-b.max.x);
   float desired=north?legacy.x:legacy.z;
   float margin=(north?b.extents.x:b.extents.z)+.45f;
   float lo=(north?area.xMin:area.yMin)+margin,hi=(north?area.xMax:area.yMax)-margin;
   desired=Mathf.Clamp(desired,lo,hi);Vector3 chosen=shift,stand=Vector3.zero;bool found=false;
   for(int n=0;n<21;n++){
    float offset=n==0?0:((n+1)/2)*1.7f*(n%2==1?1:-1);float along=desired+offset;
    if(along<lo||along>hi)continue;
    var candidate=shift+(north?Vector3.right*(along-b.center.x):Vector3.forward*(along-b.center.z));var moved=b; moved.center+=candidate;
    var front=north?new Vector3(moved.center.x,0,moved.min.z):side<0?new Vector3(moved.max.x,0,moved.center.z):new Vector3(moved.min.x,0,moved.center.z);
    var approach=front+inward*.85f;
    if(!MainSearchSlot274(box,support,b,candidate,approach))continue;chosen=candidate;stand=approach;found=true;break;
   }
   if(!found)throw new InvalidOperationException("No clear wall slot for "+box.Name+" floor "+RepairFloor220);
   MainMoveSearch274(box,support,chosen);box.Position=stand;
   var placed=MainSearchBounds274(box,support);var wall=north?new Vector3(placed.center.x,0,edge):new Vector3(edge,0,placed.center.z);
   MainSearchPlacements274.Add(new MainSearchPlacement274{Box=box,Support=support,RoomName=roomName,RoomArea=area,WallAligned=true,WallPoint=wall,Inward=inward,ClosedBounds=placed});
  }
 }
}
