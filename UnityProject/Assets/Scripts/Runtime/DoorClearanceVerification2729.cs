using System;
using System.IO;
using System.Collections;
using UnityEngine;
namespace GreyHarbor {
 public sealed partial class RaidVerification {
  void DoorCloseProbe2729(WorldDoor door,float sign,string label){
   var bounds=G.ClosedDoorBounds2729(door);bool vertical=bounds.size.z>=bounds.size.x;
   var normal=vertical?Vector3.right:Vector3.forward;
   Vector3 center=new Vector3(bounds.center.x,0,bounds.center.z),origin=center+normal*sign*.12f;
   if(!door.Open){G.Player.transform.position=center+normal*.9f;Physics.SyncTransforms();Check(G.ToggleRoomDoor(door),"2729 open for close probe "+label);}
   G.Player.transform.position=origin;Physics.SyncTransforms();
   FreightMonsterF2720(door,false);
   var target=G.Player.transform.position;
   Check(!door.Open,"2729 threshold F closes without requiring manual move "+label);
   Check(Vector3.Distance(origin,target)>.01f&&Vector3.Distance(origin,target)<=1.2f,"2729 short automatic displacement "+label);
   Check(sign*Vector3.Dot(target-center,normal)>0,"2729 player remains on original side "+label);
   Check(CargoClear2713(target),"2729 final player capsule clear of closed door and walls "+label);
   Check(G.Navigation.ClearSegment(target,target+normal*sign*.5f),"2729 player can walk away after closing "+label);
   G.Player.Move(normal*sign*.5f);Check(Vector3.Distance(G.Player.transform.position,target)>.45f,"2729 actual Actor.Move escapes closed door "+label);
  }
  WorldDoor DoorFixture2729(Vector3 center,float yaw,float angle){
   var pivot=new GameObject("DoorFixture2729").transform;pivot.SetParent(G.Map.Root,false);pivot.position=center-Vector3.forward;
   var slab=GameObject.CreatePrimitive(PrimitiveType.Cube);slab.name="DoorFixtureLeaf2729";slab.transform.SetParent(pivot,false);slab.transform.localPosition=new Vector3(0,1.15f,1);slab.transform.localScale=new Vector3(.1f,2.3f,2);slab.layer=SceneObstacle.HighLayer;
   pivot.localRotation=Quaternion.Euler(0,yaw,0);var b=slab.GetComponent<Collider>();Physics.SyncTransforms();
   var d=new WorldDoor{Name="2729 angled legacy fixture",Pivot=pivot,Collider=b,ClosedYaw=yaw,OpenAngle=angle,Position=new Vector3(b.bounds.center.x,0,b.bounds.center.z),ClosedSafety=new Rect(center.x-2,center.z-2,4,4),OpenSafety=new Rect(center.x-2,center.z-2,4,4)};
   G.Map.Doors.Add(d);return d;
  }
  IEnumerator RunDoorClearance2729(){
   var controls=G.Controls;var random=UnityEngine.Random.state;string dir=Path.GetDirectoryName(G.SavePath);
   string[] formal={"stash-v40.json","stash-v41.json","stash-v42.json"};byte[][] before=Array.ConvertAll(formal,n=>File.Exists(Path.Combine(dir,n))?File.ReadAllBytes(Path.Combine(dir,n)):null);
   try{
    Check(Application.version=="2.7.29"&&G.SavePath.EndsWith("verification-stash-2.7.29.json"),"2729 isolated door clearance verification");
    FreshCamp();G.Controls=new TouchInput();G.HarborWalkthrough=true;yield return G.BeginTrainingLoad2721();G.Paused=true;
    var outdoor=G.Map;OfficePreview2726.Apply(G);FreightRoomWalk2717(RaidMap.OfficeArrival2726,"2729 entry");
    var portal=outdoor.Doors.Find(d=>d.Portal&&d.Destination=="officeright1");FreightMonsterF2720(portal,true);yield return null;
    var indoor=G.Map;yield return OfficeDoors2727();
    var openings=indoor.OfficeOpenings2726.FindAll(o=>o.Door!=null);
    foreach(var o in openings){foreach(float side in new[]{-1f,1f})DoorCloseProbe2729(o.Door,side,o.Door.Name+" "+o.Position+" side="+side);yield return null;}
    var security=openings.Find(o=>Mathf.Abs(o.X+2)<.01f&&Mathf.Abs(o.Z+20)<.01f).Door;
    G.Player.transform.position=security.Position+Vector3.right*.9f;Physics.SyncTransforms();Check(G.ToggleRoomDoor(security),"2729 reported security door opens");
    G.Player.transform.position=security.Position;Physics.SyncTransforms();FreightMonsterF2720(security,false);Check(!security.Open&&CargoClear2713(G.Player.transform.position),"2729 exactly on door plane also closes safely");
    yield return CargoPicture2715("2729-security-door-safe-close.png",security.Position+Vector3.up,35,7,false,1600,1000);
    foreach(float angle in new[]{-105f,180f}){
     var d=DoorFixture2729(new Vector3(10,0,4),90,angle);d.Open=true;d.Pivot.localRotation=Quaternion.Euler(0,d.ClosedYaw+d.OpenAngle,0);Physics.SyncTransforms();
     DoorCloseProbe2729(d,-1,"legacy yaw90 angle"+angle);d.Open=true;d.Pivot.localRotation=Quaternion.Euler(0,d.ClosedYaw+d.OpenAngle,0);Physics.SyncTransforms();DoorCloseProbe2729(d,1,"legacy reverse side angle"+angle);
     // A close well clear of the leaf must not shift the player merely for a broad historical safety rectangle.
     d.Open=true;d.Pivot.localRotation=Quaternion.Euler(0,d.ClosedYaw+d.OpenAngle,0);Physics.SyncTransforms();var bounds=G.ClosedDoorBounds2729(d);var stand=new Vector3(bounds.center.x,0,bounds.center.z)+Vector3.forward*1.5f;
     G.Player.transform.position=stand;Physics.SyncTransforms();Check(G.ToggleRoomDoor(d)&&G.Player.transform.position==stand,"2729 clear player stays still with broad legacy safety area "+angle);
     G.Map.Doors.Remove(d);d.Pivot.gameObject.SetActive(false);Destroy(d.Pivot.gameObject);yield return null;
    }
    // Local space obstructed: no clipping or teleport to another room; preserve open state.
    G.Player.transform.position=security.Position+Vector3.right*.9f;Physics.SyncTransforms();G.ToggleRoomDoor(security);
    var blockers=new System.Collections.Generic.List<GameObject>();foreach(float side in new[]{-1f,1f}){
     var b=GameObject.CreatePrimitive(PrimitiveType.Cube);b.transform.SetParent(indoor.Root,false);b.transform.position=security.Position+Vector3.right*side*.85f+Vector3.up*.7f;b.transform.localScale=new Vector3(.35f,1.4f,3.2f);b.layer=side<0?SceneObstacle.HighLayer:9;blockers.Add(b);
    }
    G.Player.transform.position=security.Position+Vector3.right*.12f;Physics.SyncTransforms();var original=G.Player.transform.position;
    Check(!G.ToggleRoomDoor(security)&&security.Open&&G.Player.transform.position==original,"2729 obstacle and actor blocked destinations leave player and door unchanged");
    foreach(var b in blockers){b.SetActive(false);Destroy(b);}Physics.SyncTransforms();FreightMonsterF2720(security,false);Check(!security.Open&&CargoClear2713(G.Player.transform.position),"2729 normal closing works once obstruction removed");
    var saved=G.Player.transform.position;FreightRoomWalk2717(indoor.Spawn,"2729 exit after clearance");FreightMonsterF2720(indoor.OfficeExit2726,true);yield return null;
    FreightMonsterF2720(portal,true);yield return null;Check(G.Map==indoor&&!security.Open,"2729 closed door state survives map cache");
    G.Player.transform.position=saved;Physics.SyncTransforms();Check(CargoClear2713(saved),"2729 safe close position remains clear in cached interior");
    Check(!G.ToggleRoomDoor(indoor.OfficeExit2726),"2729 portal unaffected by ordinary closing helper");
    for(int i=0;i<formal.Length;i++)Check(MainBytes273(before[i],File.Exists(Path.Combine(dir,formal[i]))?File.ReadAllBytes(Path.Combine(dir,formal[i])):null),"2729 formal save unchanged "+formal[i]);
   }finally{G.Controls=controls;UnityEngine.Random.state=random;G.Paused=true;}
  }
 }
}
