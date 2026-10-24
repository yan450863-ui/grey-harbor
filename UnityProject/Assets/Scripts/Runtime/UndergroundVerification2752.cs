using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
namespace GreyHarbor { public sealed partial class RaidVerification {
 void UndergroundPlace2752(Vector3 p,string label){Check(CargoClear2713(p),"2752 safe actual capsule "+label+" "+p);G.Player.transform.position=p;Physics.SyncTransforms();G.Rig.Tick(0);G.Vision.Refresh(p,G.Player.Aim);}
 void UndergroundWalk2752(Vector3 p,string label){Check(G.Map.UndergroundFloorContains2752(p,.34f),"2752 tunnel floor supports whole capsule "+label);Check(WalkMain273(p)&&CargoClear2713(G.Player.transform.position),"2752 real navigation and Actor.Move "+label+" / "+mainWalkFailure273);}
 void UndergroundImage2752(Texture2D image,string label){
  var pixels=image.GetPixels32();int lit=0,white=0;var tones=new HashSet<int>();double sum=0;
  foreach(var p in pixels){float l=(.2126f*p.r+.7152f*p.g+.0722f*p.b)/255f;sum+=l;if(l>.04f){lit++;tones.Add(Mathf.RoundToInt(l*255));}if(p.r>248&&p.g>248&&p.b>248)white++;}
  // Intentional flat whitebox: floor, wall faces and background may contain only 4-5 tones.
  // A whole-yard 25% visible-ground threshold would reject valid tunnel geometry.
  Check(lit>pixels.Length*.025f&&white<pixels.Length*.15f&&tones.Count>=4&&sum/pixels.Length<.9,"2752 rendered narrow tunnel contains real nonblank geometry without white clipping "+label+" lit="+lit/(float)pixels.Length+" tones="+tones.Count);
 } IEnumerator UndergroundPhoto2752(Vector3 center,string name,float size,bool top=false){
  var obj=new GameObject("UndergroundQACamera2752");var c=obj.AddComponent<Camera>();c.CopyFrom(G.View);c.enabled=false;c.allowHDR=false;c.orthographic=true;c.orthographicSize=size;c.nearClipPlane=.1f;c.farClipPlane=500;c.transform.rotation=top?Quaternion.Euler(90,0,0):Quaternion.Euler(58,35,0);c.transform.position=center-c.transform.forward*220;
  float fog=Shader.GetGlobalFloat("_HarborFogEnabled");var light=RenderSettings.ambientLight;
  try{yield return null;Shader.SetGlobalFloat("_HarborFogEnabled",0);var texture=HvRender277(c,name,top?1900:1500,top?1600:1000);try{UndergroundImage2752(texture,"actual whitebox overview "+name);}finally{Destroy(texture);}Check(RenderSettings.ambientLight==light,"2752 screenshot preserves scene light "+name);}finally{Shader.SetGlobalFloat("_HarborFogEnabled",fog);Destroy(obj);}
 }
 IEnumerator UndergroundGateCheck2752(RaidMap map){
  var gate=map.UndergroundGate2752;Check(gate!=null&&!gate.Portal&&!gate.Locked&&!gate.Open&&gate.Collider&&gate.Pivot,"2752 one ordinary unlocked physical whitebox gate");
  var bounds=gate.Collider.bounds;var normal=bounds.size.x<bounds.size.z?Vector3.right:Vector3.forward;var center=new Vector3(bounds.center.x,0,bounds.center.z);var a=center-normal*1.15f;var b=center+normal*1.15f;
  Check(map.UndergroundFloorContains2752(a,.34f)&&map.UndergroundFloorContains2752(b,.34f)&&CargoClear2713(a)&&CargoClear2713(b),"2752 southern gate has safe supported approach on both sides");
  UndergroundPlace2752(a,"gate closed approach");var start=G.Player.transform.position;for(int i=0;i<50;i++)G.Player.Move(normal*.05f);Physics.SyncTransforms();Check(Vector3.Dot(G.Player.transform.position-center,normal)<-.15f&&Vector3.Distance(start,G.Player.transform.position)>.1f,"2752 closed gate really blocks forward Actor.Move");
  UndergroundPlace2752(a,"gate locked approach");gate.Locked=true;try{FreightMonsterF2720(gate,false);Check(!gate.Open&&gate.Collider.bounds==bounds,"2752 temporary locked gate refuses actual F with no key");}finally{gate.Locked=false;}
  FreightMonsterF2720(gate,false);Check(gate.Open,"2752 actual F opens southern ordinary gate");UndergroundWalk2752(b,"gate first crossing");UndergroundWalk2752(a,"gate reverse crossing");
  UndergroundWalk2752(center,"open gate occupied doorway");var occupied=G.Player.transform.position;FreightMonsterF2720(gate,false);Check(!gate.Open&&Vector3.Distance(occupied,G.Player.transform.position)>.05f&&CargoClear2713(G.Player.transform.position)&&map.UndergroundFloorContains2752(G.Player.transform.position,.30f),"2752 occupied gate closes and silently moves player safely");
  UndergroundPlace2752(a,"gate reopen for continuous network");FreightMonsterF2720(gate,false);Check(gate.Open,"2752 gate reopened before all-network traversal");yield return UndergroundPhoto2752(center,"2752-southern-whitebox-gate.png",7);
 }
 void UndergroundWalls2752(RaidMap map){
  int checkedWalls=0;foreach(var c in map.Root.GetComponentsInChildren<Collider>()){
   if(c==map.UndergroundGate2752.Collider||!c.enabled||!c.gameObject.activeInHierarchy||(SceneObstacle.SightMask&(1<<c.gameObject.layer))==0)continue;var r=c.bounds;if(r.size.y<1.5f||Mathf.Min(r.size.x,r.size.z)>.7f)continue;
   var n=r.size.x<r.size.z?Vector3.right:Vector3.forward;var center=new Vector3(r.center.x,0,r.center.z);var a=center-n*.9f;var b=center+n*.9f;
   if(!map.UndergroundFloorContains2752(a,.34f)||!CargoClear2713(a)){var swap=a;a=b;b=swap;n=-n;if(!map.UndergroundFloorContains2752(a,.34f)||!CargoClear2713(a))continue;}
   Check(Physics.Linecast(a+Vector3.up*RaidGame.CombatHeight,b+Vector3.up*RaidGame.CombatHeight,SceneObstacle.SightMask,QueryTriggerInteraction.Ignore),"2752 wall blocks actual flashlight sight "+c.name);G.Vision.Refresh(a,n);Check(!G.Vision.CanSee(b)&&!G.Vision.SurfaceVisible(b),"2752 real vision respects tunnel wall without room gate "+c.name);if(++checkedWalls>=3)break;
  }
  Check(checkedWalls>0,"2752 real tunnel sight-blocking wall samples exercised");
 }
 IEnumerator RunUnderground2752(){
  var controls=G.Controls;bool enabled=G.enabled;string dir=Path.GetDirectoryName(G.SavePath);string[] names={"stash-v40.json","stash-v41.json","stash-v42.json"};var saves=Array.ConvertAll(names,n=>File.Exists(Path.Combine(dir,n))?File.ReadAllBytes(Path.Combine(dir,n)):null);
  try{
   Check(Application.version=="2.7.53"&&G.VerifyMode&&G.SavePath.EndsWith("verification-stash-2.7.53.json"),"2752 isolated underground verification save");FreshCamp();G.Controls=new TouchInput();G.HarborWalkthrough=true;yield return G.BeginTrainingLoad2721();G.Paused=true;G.enabled=false;
   var outdoor=G.Map;var ground=outdoor.StableGround137;var fingerprint=outdoor.GroundPaintFingerprint2721;Check(ground&&outdoor.GroundCacheUsed2721,"2752 surface hatches retain existing baked ground cache");foreach(var hatch in outdoor.Root.GetComponentsInChildren<Transform>())if(hatch.name.StartsWith("UndergroundSurface"))Check(hatch.GetComponentsInChildren<Collider>().Length==0,"2752 flush inspection hatch adds no raised physics obstruction "+hatch.name);Check(outdoor.UndergroundSurfaceEntries2752.Count==6,"2752 exactly six surface inspection-entry portals");
   var ids=new[]{"freight","repair","cargo","office","loading","south"};var unique=new HashSet<string>();foreach(var door in outdoor.UndergroundSurfaceEntries2752){Check(door.Portal&&door.Destination.StartsWith("underground-enter:")&&CargoClear2713(door.Position)&&outdoor.OnHarborSurface(door.Position),"2752 surface hatch has safe real capsule and floor "+door.Destination);unique.Add(door.Destination.Substring("underground-enter:".Length));}foreach(var id in ids)Check(unique.Contains(id),"2752 registered surface entry "+id);
   var first=outdoor.UndergroundSurfaceEntries2752.Find(d=>d.Destination=="underground-enter:freight");UndergroundPlace2752(first.Position,"first surface hatch");FreightMonsterF2720(first,true);yield return null;
   var inside=G.Map;var vision=G.Vision;var navigation=G.Navigation;Check(inside.IsInterior&&inside.InteriorId=="underground2752"&&inside.UndergroundWhitebox2752,"2752 enters one shared continuous underground map");Check(!vision.RoomGate&&Shader.GetGlobalFloat("_HarborRoomGate")==0&&Shader.GetGlobalTexture("_HarborVisibility")==vision.Texture,"2752 tunnel vision owns fog with no independent room black gates");
   Check(inside.Loot.Count==0&&G.Enemies.Count==0&&inside.Doors.FindAll(d=>!d.Portal).Count==1&&inside.Doors.FindAll(d=>d.Portal).Count==6,"2752 no containers or soldiers and only one ordinary gate plus six exits");Check(inside.UndergroundEntries2752.Count==6&&inside.UndergroundRoutes2752.Count>=5&&inside.UndergroundFloorAreas2752.Count>=5,"2752 two trunks and connecting loops have explicit continuous routes and floors");
   foreach(var e in inside.UndergroundEntries2752){Check(inside.UndergroundFloorContains2752(e.Arrival,.34f)&&inside.UndergroundFloorContains2752(e.ExitPosition,.34f)&&CargoClear2713(e.Arrival)&&CargoClear2713(e.ExitPosition),"2752 all six internal arrivals exits are physically supported and safe "+e.Id);var portal=inside.Doors.Find(d=>d.Portal&&d.Destination=="underground-exit:"+e.Id);Check(portal!=null&&portal.Position==e.ExitPosition,"2752 internal portal position matches exit record "+e.Id);}int seen=0;foreach(bool s in vision.Seen)if(s)seen++;Check(seen>0&&seen<vision.Seen.Length/2,"2752 arrival explores locally without revealing entire underground map");
   yield return UndergroundGateCheck2752(inside);
   Check(inside.UndergroundOpenAreas2753.Count==6,"2753 six open enlarged work areas");
   int areaIndex=0;foreach(var area in inside.UndergroundOpenAreas2753){
    Check(area.width>=12&&area.height>=12,"2753 enlarged area has usable combat and container width "+areaIndex);
    UndergroundWalk2752(new Vector3(area.center.x,0,area.center.y),"2753 connected open area center "+areaIndex);
    for(int x=0;x<3;x++)for(int z=0;z<3;z++){
     var point=new Vector3(Mathf.Lerp(area.xMin+1,area.xMax-1,x*.5f),0,Mathf.Lerp(area.yMin+1,area.yMax-1,z*.5f));
     Check(inside.UndergroundFloorContains2752(point,.34f)&&CargoClear2713(point),"2753 full wide platform supports capsule "+areaIndex+" "+x+" "+z);
     UndergroundWalk2752(point,"2753 actual open platform walk "+areaIndex+" "+x+" "+z);
    }areaIndex++;
   }
   int routeNumber=0;foreach(var route in inside.UndergroundRoutes2752){Check(route.Length>=2,"2752 route has continuous waypoints "+routeNumber);UndergroundWalk2752(route[0],"route start "+routeNumber);for(int i=1;i<route.Length;i++){
    var from=route[i-1];var to=route[i];int steps=Mathf.Max(1,Mathf.CeilToInt(Vector3.Distance(from,to)/.5f));for(int s=0;s<=steps;s++){var p=Vector3.Lerp(from,to,s/(float)steps);Check(inside.UndergroundFloorContains2752(p,.34f)&&CargoClear2713(p),"2752 route wall and platform support "+routeNumber+" "+i+" "+s);}Check(G.Navigation.ClearSegment(from,to),"2752 route straight segment has real swept capsule clearance "+routeNumber+" "+i);UndergroundWalk2752(to,"route "+routeNumber+" point "+i);yield return null;
   }routeNumber++;}UndergroundWalls2752(inside);
   var gate=inside.UndergroundGate2752;navigation=G.Navigation;var runningRoute=inside.UndergroundRoutes2752[0];UndergroundWalk2752(runningRoute[0],"non-full stamina fixture");G.Sprint2711.Toggle();G.Paused=false;G.MovePlayer2711((runningRoute[1]-runningRoute[0]).normalized*3.4f,.5f);G.Paused=true;Check(G.Sprint2711.Stamina<20&&G.Sprint2711.Intent,"2752 real tunnel running establishes non-full stamina and run intent");float stamina=G.Sprint2711.Stamina;bool intent=G.Sprint2711.Intent;var explored=G.Player.transform.position;G.Vision.Refresh(explored,Vector3.forward);
   for(int i=0;i<ids.Length;i++){
    string exitId=ids[(i+1)%ids.Length];var entry=inside.UndergroundEntries2752.Find(e=>e.Id==exitId);Check(entry!=null,"2752 internal exit metadata "+exitId);UndergroundWalk2752(entry.ExitPosition,"cross-area exit "+exitId);var exit=inside.Doors.Find(d=>d.Portal&&d.Destination=="underground-exit:"+exitId);FreightMonsterF2720(exit,true);yield return null;
    var surface=outdoor.UndergroundSurfaceEntries2752.Find(d=>d.Destination=="underground-enter:"+exitId);Check(G.Map==outdoor&&Vector3.Distance(G.Player.transform.position,surface.Position)<.03f&&CargoClear2713(G.Player.transform.position),"2752 cross-area exit returns corresponding real surface entry "+exitId);Check(outdoor.StableGround137==ground&&outdoor.GroundPaintFingerprint2721==fingerprint,"2752 underground switching preserves reused outdoor ground cache "+exitId);FreightMonsterF2720(surface,true);yield return null;
    Check(G.Map==inside&&G.Vision==vision&&G.Navigation==navigation&&gate.Open&&Vector3.Distance(G.Player.transform.position,entry.Arrival)<.03f,"2752 re-entry shares map vision navigation and open door state "+exitId);Check(vision.Explored(explored)&&!vision.RoomGate,"2752 exploration persists without room gate "+exitId);Check(inside.Loot.Count==0&&G.Enemies.Count==0,"2752 cached re-entry still contains no enemies or containers "+exitId);Check(G.Sprint2711.Stamina==stamina&&G.Sprint2711.Intent==intent,"2752 cross-area transfers preserve transient stamina intent "+exitId);
   }
   yield return UndergroundPhoto2752(new Vector3(inside.Bounds.center.x,0,inside.Bounds.center.y),"2753-expanded-continuous-network-top.png",Mathf.Max(inside.Bounds.width,inside.Bounds.height)*.52f,true);
   for(int i=0;i<Mathf.Min(2,inside.UndergroundRoutes2752.Count);i++){var r=inside.UndergroundRoutes2752[i];yield return UndergroundPhoto2752((r[0]+r[r.Length-1])*.5f,"2752-main-trunk-"+i+".png",18);}
   UndergroundPlace2752(inside.UndergroundEntries2752.Find(e=>e.Id=="south").Arrival,"normal gameplay south gate view");G.Player.Aim=(gate.Position-G.Player.transform.position).normalized;G.Rig.Tick(0);G.Vision.Bind();G.Vision.Refresh(G.Player.transform.position,G.Player.Aim);Shader.SetGlobalFloat("_HarborFogEnabled",1);G.Player.Draw(G.View,0);yield return null;var actual=HvRender277(G.View,"2753-actual-underground-flashlight.png",1600,900);try{UndergroundImage2752(actual,"actual camera flashlight wall visibility");}finally{Destroy(actual);}
   for(int i=0;i<names.Length;i++)Check(MainBytes273(saves[i],File.Exists(Path.Combine(dir,names[i]))?File.ReadAllBytes(Path.Combine(dir,names[i])):null),"2752 formal save bytes unchanged "+names[i]);
  }finally{G.Controls=controls;G.ClearShots();G.Paused=true;G.enabled=enabled;}
 }
}}