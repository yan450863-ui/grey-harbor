using System;using System.IO;using System.Collections;using System.Collections.Generic;using UnityEngine;
namespace GreyHarbor {public sealed partial class RaidVerification {
 sealed class OfficeFallCache2748 {public RaidMap Map;public RaidActor[] Actors;public RaidActor Hurt,Dead;public float Health,Armor;}
 readonly List<OfficeFallCache2748> fallCaches2748=new List<OfficeFallCache2748>();
 readonly HashSet<string> fallSearchRooms2748=new HashSet<string>();
 void OfficeFallMovedFurnitureSearch2748(){
  var map=G.Map;string id=!map.OfficeLeft2741&&map.OfficeFloor2733==1?"03":!map.OfficeLeft2741&&map.OfficeFloor2733==2?"04":map.OfficeLeft2741&&map.OfficeFloor2733==1?"04":null;
  if(id==null||!fallSearchRooms2748.Add(map.InteriorId))return;
  var room=map.OfficeRooms2726.Find(r=>r.Id==id);int checkedCount=0;
  foreach(var record in map.OfficeSearchRecords2736){
   if(record.RoomName!=room.Name)continue;checkedCount++;
   bool clear=OfficeNpcClear2747(record.Stand)&&OfficeNpcClear2747(record.AccessPoint)&&G.Navigation.ClearSegment(record.Stand,record.AccessPoint);
   var delta=record.AccessPoint-record.Stand;
   if(delta.sqrMagnitude>.0001f&&Physics.CapsuleCast(record.Stand+Vector3.up*.35f,record.Stand+Vector3.up*1.35f,.34f,delta.normalized,out var hit,delta.magnitude,SceneObstacle.MovementMask,QueryTriggerInteraction.Ignore)){clear=false;Debug.Log("2748 MOVED FURNITURE SEARCH CONFLICT "+map.InteriorId+" room="+room.Name+" box="+record.Container.Name+" collider="+hit.collider.name+" bounds="+hit.collider.bounds);}
   foreach(var point in new[]{record.Stand,record.AccessPoint})foreach(var collision in Physics.OverlapCapsule(point+Vector3.up*.35f,point+Vector3.up*1.35f,.34f,SceneObstacle.MovementMask,QueryTriggerInteraction.Ignore))Debug.Log("2748 SEARCH STAND CONFLICT "+map.InteriorId+" box="+record.Container.Name+" point="+point+" collider="+collision.name+" bounds="+collision.bounds);
   Check(clear,"2748 moved furniture preserves affected room container stand approach capsule and navigation "+map.InteriorId+" / "+record.Container.Name);
  }
  Check(checkedCount>0,"2748 affected furnished room search approaches explicitly checked "+map.InteriorId+" / "+room.Name);
 }
 void OfficeFallCacheCheck2748(){
  var m=G.Map;Check(G.Enemies.Count==m.OfficeFloor2733+3&&m.OfficeSoldierRecords2747.Count==m.OfficeFloor2733+3,"2748 affected floor retains 4/5/6 soldiers "+m.InteriorId);
  var cache=fallCaches2748.Find(q=>q.Map==m);
  if(cache==null){var actors=G.Enemies.ToArray();var hurt=actors[0];var dead=actors[1];hurt.Damage(7);hurt.NpcArmorPoints-=3;dead.Damage(dead.Health+1);cache=new OfficeFallCache2748{Map=m,Actors=actors,Hurt=hurt,Dead=dead,Health=hurt.Health,Armor=hurt.NpcArmorPoints};fallCaches2748.Add(cache);}
  Check(Array.TrueForAll(cache.Actors,e=>G.Enemies.Contains(e))&&cache.Hurt.Health==cache.Health&&cache.Hurt.NpcArmorPoints==cache.Armor&&cache.Dead.Dead&&!cache.Dead.HitCollider.enabled,"2748 original wounded and dead soldiers remain cached "+m.InteriorId);
  OfficeFallMovedFurnitureSearch2748();
 }
 bool OfficeFallLanding2748(Vector3 p,bool assert=true){
  Physics.SyncTransforms();var hits=Physics.OverlapCapsule(p+Vector3.up*.35f,p+Vector3.up*1.35f,.34f,SceneObstacle.MovementMask,QueryTriggerInteraction.Ignore);
  foreach(var hit in hits){string owners="";foreach(var r in G.Map.OfficeDressingRecords2736)if(hit.transform==r.View.transform||hit.transform.IsChildOf(r.View.transform))owners=r.Type+" / "+r.Name+" / "+r.RoomName+" / view="+r.View.transform.position;Debug.Log("2748 LANDING CONFLICT "+G.Map.InteriorId+" XZ="+p+" collider="+hit.name+" bounds="+hit.bounds+" owner="+owners);}
  if(assert)Check(hits.Length==0,"2748 precise unchanged XZ landing clears real .34m capsule "+G.Map.InteriorId+" "+p);return hits.Length==0;
 }
 bool OfficeFallLandingDisk2748(Vector3 center,bool assert=true){
  bool clear=OfficeFallLanding2748(center,false);
  for(int i=0;i<8;i++){float angle=i*Mathf.PI/4;clear&=OfficeFallLanding2748(center+new Vector3(Mathf.Cos(angle)*.6f,0,Mathf.Sin(angle)*.6f),false);}
  if(assert)Check(clear,"2748 full trigger disk center and eight .6m offsets clear exact landing capsules "+G.Map.InteriorId+" "+center);return clear;
 }
 IEnumerator RunOfficeFallLandingReport2748(){
  var input=G.Controls;bool enabled=G.enabled;try{FreshCamp();G.Controls=new TouchInput();G.HarborWalkthrough=true;yield return G.BeginTrainingLoad2721();G.Paused=true;G.SwitchInterior("officeright1");yield return null;G.enabled=false;bool clear=true;
   clear&=OfficeFallLandingDisk2748(new Vector3(14,0,-19),false);clear&=OfficeFallLandingDisk2748(new Vector3(-16,0,2),false);
   G.SwitchInterior("office-floor:2:south");yield return null;clear&=OfficeFallLandingDisk2748(new Vector3(8,0,-12),false);clear&=OfficeFallLandingDisk2748(new Vector3(-15,0,7),false);
   G.SwitchInterior("office-link:bridge:right");G.SwitchInterior("office-link:left:bridge");yield return null;clear&=OfficeFallLandingDisk2748(new Vector3(10,0,-17),false);
   G.SwitchInterior("office-floor:1:south");yield return null;clear&=OfficeFallLandingDisk2748(new Vector3(-14,0,7),false);Check(clear,"2748 all six exact XZ lower floor landing disks clear");
  }finally{G.Controls=input;G.Paused=true;G.enabled=enabled;}
 }
 IEnumerator OfficeFallPhoto2748(Vector3 center,string name){
  var go=new GameObject("OfficeFallReviewCamera2748");var cam=go.AddComponent<Camera>();cam.CopyFrom(G.View);cam.enabled=false;cam.allowHDR=false;cam.orthographic=true;cam.orthographicSize=4.5f;cam.nearClipPlane=.1f;cam.farClipPlane=250;cam.transform.rotation=Quaternion.Euler(52,35,0);cam.transform.position=center-cam.transform.forward*110;
  float fog=Shader.GetGlobalFloat("_HarborFogEnabled");try{yield return null;Shader.SetGlobalFloat("_HarborFogEnabled",0);foreach(var f in G.Map.Root.GetComponentsInChildren<ShowcaseSpriteFacing2717>())f.SyncCamera(cam);G.Player.Draw(cam,0);var image=HvRender277(cam,name,1200,1000);try{CargoImageCheck2713(image,name);}finally{Destroy(image);}}finally{Shader.SetGlobalFloat("_HarborFogEnabled",fog);Destroy(go);G.Player.Draw(G.View,0);foreach(var f in G.Map.Root.GetComponentsInChildren<ShowcaseSpriteFacing2717>())f.SyncCamera(G.View);}
 }
 IEnumerator OfficeFallOne2748(Vector3 center,int source,bool left,bool screenshot){
  Check(G.Map.OfficeFloor2733==source&&G.Map.OfficeLeft2741==left,"2748 source tower and floor correct");var upper=G.Map;
  var hole=upper.OfficeFallHoles2748.Find(h=>Vector3.Distance(h.Center,center)<.001f);Check(hole!=null&&hole.TriggerRadius==.65f&&hole.View&&hole.View.GetComponent<MeshRenderer>()!=null&&hole.View.GetComponentsInChildren<Collider>().Length==0,"2748 authored dark hole cracked edge visual and nonblocking trigger "+upper.InteriorId+" "+center);
  Vector3 stand=center+(!left&&source==2&&screenshot?new Vector3(.2f,0,.1f):Vector3.zero);
  G.Player.transform.position=stand;Physics.SyncTransforms();Check(CargoClear2713(stand),"2748 source hole has safe player capsule");
  G.TickOfficeFall2748(.9f);G.Paused=true;G.TickOfficeFall2748(10);Check(G.Map==upper,"2748 pause cannot advance fall countdown");G.Paused=false;G.InventoryOpen=true;G.TickOfficeFall2748(10);Check(G.Map==upper,"2748 backpack cannot advance fall countdown");G.InventoryOpen=false;
  G.Player.Dead=true;G.TickOfficeFall2748(10);Check(G.Map==upper,"2748 dead player cannot fall");G.Player.Dead=false;
  G.Player.transform.position=center+Vector3.right*1.4f;G.TickOfficeFall2748(.1f);G.Player.transform.position=stand;G.TickOfficeFall2748(1.9f);Check(G.Map==upper,"2748 leaving hole resets countdown and 1.9 seconds never falls");
  if(screenshot)yield return OfficeFallPhoto2748(center,left?"2748-left-third-floor-dark-hole.png":"2748-right-second-floor-dark-hole.png");
  G.TickOfficeFall2748(.11f);yield return null;
  Check(G.Map!=upper&&G.Map.OfficeLeft2741==left&&G.Map.OfficeFloor2733==source-1,"2748 actual two-second fall enters same tower lower floor");
  Check(Vector3.Distance(G.Player.transform.position,stand)<.001f,"2748 falling preserves exact original XZ including offset stand with no snap to hole center "+stand);OfficeFallLandingDisk2748(center);
  var room=G.Map.OfficeRooms2726.Find(r=>r.Area.Contains(new Vector2(stand.x,stand.z)));Check(room!=null&&G.Vision.RoomAllowed(stand)&&G.Vision.RoomAllowed(room.Center),"2748 arrival reveals entire landing room without black fog");OfficeFallCacheCheck2748();
  G.TickOfficeFall2748(5);Check(G.Map.OfficeFloor2733==source-1,"2748 landing cannot immediately chain fall");G.SwitchInterior("office-floor:"+source+":south");yield return null;OfficeFallCacheCheck2748();
 }
 IEnumerator RunOfficeFall2748(){
  var input=G.Controls;bool enabled=G.enabled;string dir=Path.GetDirectoryName(G.SavePath);string[] formal={"stash-v40.json","stash-v41.json","stash-v42.json"};var bytes=Array.ConvertAll(formal,n=>File.Exists(Path.Combine(dir,n))?File.ReadAllBytes(Path.Combine(dir,n)):null);
  try{
   Check(Application.version=="2.7.48"&&G.VerifyMode&&G.SavePath.EndsWith("verification-stash-2.7.48.json"),"2748 fall targeted QA uses isolated save");FreshCamp();G.Controls=new TouchInput();G.HarborWalkthrough=true;yield return G.BeginTrainingLoad2721();G.Paused=true;G.SwitchInterior("officeright1");yield return null;G.enabled=false;G.Paused=false;
   OfficeFallCacheCheck2748();Check(G.Map.OfficeFallHoles2748.Count==0,"2748 right floor one has no fall hole");OfficeFallLandingDisk2748(new Vector3(14,0,-19));OfficeFallLandingDisk2748(new Vector3(-16,0,2));
   G.SwitchInterior("office-floor:2:south");yield return null;OfficeFallCacheCheck2748();Check(G.Map.OfficeFallHoles2748.Count==2,"2748 right second floor contains two original fall locations");OfficeFallLandingDisk2748(new Vector3(8,0,-12));OfficeFallLandingDisk2748(new Vector3(-15,0,7));
   yield return OfficeFallOne2748(new Vector3(14,0,-19),2,false,true);yield return OfficeFallOne2748(new Vector3(-16,0,2),2,false,false);
   G.SwitchInterior("office-floor:3:south");yield return null;OfficeFallCacheCheck2748();Check(G.Map.OfficeFallHoles2748.Count==2,"2748 right third floor contains two original fall locations");yield return OfficeFallOne2748(new Vector3(8,0,-12),3,false,false);yield return OfficeFallOne2748(new Vector3(-15,0,7),3,false,false);
   G.SwitchInterior("office-floor:2:south");G.SwitchInterior("office-link:bridge:right");G.SwitchInterior("office-link:left:bridge");yield return null;OfficeFallCacheCheck2748();Check(G.Map.OfficeFallHoles2748.Count==1,"2748 left second floor contains one original fall location");OfficeFallLandingDisk2748(new Vector3(10,0,-17));
   G.SwitchInterior("office-floor:1:south");yield return null;OfficeFallCacheCheck2748();Check(G.Map.OfficeFallHoles2748.Count==0,"2748 left floor one has no fall hole");OfficeFallLandingDisk2748(new Vector3(-14,0,7));G.SwitchInterior("office-floor:2:south");yield return null;
   yield return OfficeFallOne2748(new Vector3(-14,0,7),2,true,false);G.SwitchInterior("office-floor:3:south");yield return null;OfficeFallCacheCheck2748();Check(G.Map.OfficeFallHoles2748.Count==1,"2748 left third floor contains one original fall location");yield return OfficeFallOne2748(new Vector3(10,0,-17),3,true,true);
   Check(fallCaches2748.Count==6,"2748 all six office floors retain original thirty soldier caches");for(int i=0;i<formal.Length;i++)Check(MainBytes273(bytes[i],File.Exists(Path.Combine(dir,formal[i]))?File.ReadAllBytes(Path.Combine(dir,formal[i])):null),"2748 formal save unchanged "+formal[i]);
  }finally{G.Controls=input;G.ClearShots();G.Paused=true;G.InventoryOpen=false;G.enabled=enabled;}
 }
}}
