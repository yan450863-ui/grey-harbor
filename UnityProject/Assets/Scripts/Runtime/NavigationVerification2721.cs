using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace GreyHarbor {
 public sealed partial class RaidVerification {
  void NavigationCompare2721(RaidNavigation lazy,RaidNavigation eager,Vector3 start,Vector3 goal,string label,bool mustReach=false){
   var a=lazy.FindPath(start,goal);var b=eager.FindPath(start,goal);bool same=a.Count==b.Count;
   if(same)for(int i=0;i<a.Count;i++)same&=(a[i]-b[i]).sqrMagnitude<.0000001f;
   Check(same,"2721 lazy/eager identical ordered waypoints "+label);
   if(mustReach)Check(a.Count>0,"2721 required real route exists "+label);
   Vector3 p=start;foreach(var next in a){Check(lazy.ClearSegment(p,next),"2721 unchanged exact swept clearance "+label);p=next;}
  }
  IEnumerator NavigationFixtures2721(){
   var root=new GameObject("QA2721_ActualNavigationColliders");var offset=new Vector3(1000,0,1000);var bounds=new Rect(994,994,12,12);
   var wall=GameObject.CreatePrimitive(PrimitiveType.Cube);wall.name="QA2721_ActualDoorSlab";wall.transform.SetParent(root.transform,false);wall.transform.position=offset+Vector3.up;wall.transform.localScale=new Vector3(.6f,2,8);SceneObstacle.Apply(wall,ObstacleKind.High);
   try{
    Physics.SyncTransforms();var lazy=new RaidNavigation(bounds,.5f);var eager=new RaidNavigation(bounds,.5f,null,true);
    Check(lazy.CheckedCells2721==0&&eager.CheckedCells2721==eager.TotalCells2721,"2721 constructor defers all sampling and eager reference samples every cell");
    NavigationCompare2721(lazy,eager,offset+new Vector3(-4,0,0),offset+new Vector3(4,0,0),"temporary physical wall detour",true);
    Check(lazy.CheckedCells2721>0,"2721 path demand samples unknown cells before deciding reachability");
    NavigationCompare2721(lazy,eager,offset+new Vector3(-4,0,-4),offset+new Vector3(-3,0,-4),"short direct physical route",true);
    wall.transform.localScale=new Vector3(.6f,2,14);Physics.SyncTransforms();lazy=new RaidNavigation(bounds,.5f);eager=new RaidNavigation(bounds,.5f,null,true);
    var closed=lazy.FindPath(offset+new Vector3(-4,0,0),offset+new Vector3(4,0,0));Check(closed.Count==0,"2721 real closed full-width door blocks route");
    NavigationCompare2721(lazy,eager,offset+new Vector3(-4,0,0),offset+new Vector3(4,0,0),"closed physical door");
    wall.SetActive(false);Physics.SyncTransforms();lazy=new RaidNavigation(bounds,.5f);eager=new RaidNavigation(bounds,.5f,null,true);
    Check(lazy.CheckedCells2721==0,"2721 door reconstruction invalidates previous cell and edge cache");
    NavigationCompare2721(lazy,eager,offset+new Vector3(-4,0,0),offset+new Vector3(4,0,0),"opened door after new navigation",true);
    Func<Vector3,bool> surface=p=>Mathf.Abs(p.x-offset.x)>.65f;
    lazy=new RaidNavigation(bounds,.5f,surface);eager=new RaidNavigation(bounds,.5f,surface,true);
    NavigationCompare2721(lazy,eager,offset+new Vector3(-4,0,0),offset+new Vector3(4,0,0),"missing land strip");
    Check(lazy.FindPath(offset+new Vector3(-4,0,0),offset+new Vector3(4,0,0)).Count==0&&!lazy.ClearSegment(offset+Vector3.left*4,offset+Vector3.right*4),"2721 unknown land and original segment samples cannot bridge missing surface");
    NavigationCompare2721(lazy,eager,offset+new Vector3(-4,0,-2),offset+new Vector3(-3,0,2),"remaining land",true);
   }finally{root.SetActive(false);Destroy(root);Physics.SyncTransforms();}
   yield return null;
  }
  // Called by the dedicated 2721 dispatcher; never uses a formal-save path.
  IEnumerator VerifyNavigation2721(){
   Check(G.VerifyMode&&G.SavePath.Contains("verification-stash"),"2721 navigation runs against isolated verification inventory");
   yield return NavigationFixtures2721();FreshCamp();G.HarborWalkthrough=true;Check(G.BeginTraining(),"2721 real harbor training starts");G.Paused=true;yield return null;
   var outdoor=G.Map;var surface=new Func<Vector3,bool>(outdoor.OnHarborSurface);
   var outsideNav=new RaidNavigation(outdoor.Bounds,.75f,surface);G.Navigation=outsideNav;
   outsideNav.WarmAround2721(outdoor.Spawn,.35);Check(outsideNav.CheckedCells2721>0&&outsideNav.CheckedCells2721<outsideNav.TotalCells2721,"2721 active current map performs only budgeted nearby warmup");
   var eager=new RaidNavigation(outdoor.Bounds,.75f,surface,true);var lazy=outsideNav;
   NavigationCompare2721(lazy,eager,outdoor.Spawn,outdoor.Spawn+Vector3.forward*2,"actual partially warmed spawn short route");
   for(int i=0;i<outdoor.Exits.Count;i++)NavigationCompare2721(lazy,eager,outdoor.Spawn,outdoor.Exits[i],"actual partially warmed harbor extraction "+i,true);
   int before;
   G.SwitchInterior("freightduty");G.Paused=true;yield return null;
   Check(G.Map.IsInterior&&!outdoor.Root.gameObject.activeInHierarchy,"2721 actual switch disables cached outside colliders");before=outsideNav.CheckedCells2721;outsideNav.WarmAround2721(outdoor.Spawn,5);Check(outsideNav.CheckedCells2721==before,"2721 cached inactive navigation refuses background sampling");
   var inner=G.Map;lazy=G.Navigation;eager=new RaidNavigation(inner.Bounds,.25f,null,true);
   foreach(var actor in G.Enemies)NavigationCompare2721(lazy,eager,inner.Spawn,actor.transform.position,"actual closed-room route");
   WorldDoor door=null;foreach(var candidate in inner.Doors)if(!candidate.Portal&&!candidate.Locked){door=candidate;break;}
   Check(door!=null,"2721 actual freight room has mutable physical door");var oldNav=G.Navigation;Check(G.ToggleRoomDoor(door),"2721 actual room door toggles and creates fresh navigation");
   Check(G.Navigation!=oldNav&&G.Navigation.CheckedCells2721==0,"2721 real door cache starts unknown after synchronized layout change");
   eager=new RaidNavigation(inner.Bounds,.25f,null,true);foreach(var actor in G.Enemies)NavigationCompare2721(G.Navigation,eager,inner.Spawn,actor.transform.position,"actual open-room route");
   before=oldNav.CheckedCells2721;oldNav.WarmAround2721(inner.Spawn,5);Check(oldNav.CheckedCells2721==before,"2721 superseded door navigation refuses background sampling");
   G.SwitchInterior("freightduty");G.Paused=true;yield return null;Check(G.Map==outdoor&&G.Navigation==outsideNav,"2721 returning outdoors restores exact cached navigation instance");
   G.Finish(false);yield return null;Check(G.Phase==GamePhase.Base,"2721 navigation verification returns to camp");
  }
 }
}
