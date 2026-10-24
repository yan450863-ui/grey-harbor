using System;using System.IO;using System.Collections;using System.Collections.Generic;using UnityEngine;
namespace GreyHarbor {public sealed partial class RaidVerification {
 sealed class OfficeSoldierCache2747 {public RaidMap Map;public RaidActor[] Actors;public RaidActor Hurt,Dead;public Vector3 Position;public float Health,Armor,Alert,Memory;}
 readonly List<OfficeSoldierCache2747> officeSoldierCache2747=new List<OfficeSoldierCache2747>();
 bool OfficeNpcClear2747(Vector3 p)=>!Physics.CheckCapsule(p+Vector3.up*.35f,p+Vector3.up*1.35f,.34f,SceneObstacle.MovementMask,QueryTriggerInteraction.Ignore);
 void OfficeSoldierFloor2747(){
  var m=G.Map;int count=m.OfficeFloor2733+3;var records=m.OfficeSoldierRecords2747;
  Check(records.Count==count&&G.Enemies.Count==count,"2747 first load contains authored 4/5/6 actual soldiers "+m.InteriorId);
  var groups=new Dictionary<int,List<OfficeSoldierRecord2747>>();
  foreach(var r in records){
   var e=r.Actor;var room=m.OfficeRooms2726.Find(q=>q.Id==r.RoomId);Check(e&&G.Enemies.Contains(e)&&e.IsRebel&&!e.IsMonster&&!e.Dead&&e.gameObject.activeInHierarchy&&e.transform.parent==m.Root,"2747 live soldier belongs to active floor "+e.name);
   Check(e.Rebel==r.Kind&&e.NpcWeapon==SoldierProfiles2745.Weapon(r.Kind)&&e.NpcAmmo==SoldierProfiles2745.Ammo(r.Kind)&&e.NpcLevel==NpcRules270.Level(r.Kind),"2747 role carried weapon ammo grade matches live NPC "+e.name);
   Check(room!=null&&room.Area.Contains(new Vector2(e.Home.x,e.Home.z))&&OfficeNpcClear2747(e.Home),"2747 soldier capsule does not intersect furniture or wall "+e.name);
   foreach(var p in e.PatrolTargets)Check(OfficeNpcClear2747(p)&&room.Area.Contains(new Vector2(p.x,p.z))&&G.Navigation.ClearSegment(e.Home,p),"2747 two patrol positions stay clear within squad room "+e.name);
   foreach(var d in m.Doors)Check(Vector3.Distance(e.Home,d.Position)>=(d.Portal?5f:2.7f)-.001f,"2747 guard separated from door sweep or transition spawn "+e.name+" / "+d.Name);
   Check(Vector3.Distance(e.Home,G.Player.transform.position)>5,"2747 arrival has no point blank soldier "+e.name);
   Check(!(m.OfficeLeft2741&&m.OfficeFloor2733==1&&r.RoomId=="06")&&!(!m.OfficeLeft2741&&m.OfficeFloor2733==3&&r.RoomId=="10"),"2747 sealed duty and research rooms contain no soldier "+e.name);
   if(!groups.ContainsKey(r.Group))groups.Add(r.Group,new List<OfficeSoldierRecord2747>());groups[r.Group].Add(r);
  }
  Check(groups.Count==2,"2747 soldiers concentrated at two combat nodes "+m.InteriorId);
  foreach(var pair in groups){var list=pair.Value;Check(list.Count>=2&&list.Count<=3,"2747 squad has two or three soldiers "+m.InteriorId+" / "+pair.Key);foreach(var a in list)foreach(var b in list)if(a!=b)Check(a.RoomId==b.RoomId&&Vector3.Distance(a.Home,b.Home)>=1.59f&&Vector3.Distance(a.Home,b.Home)<=9.01f,"2747 squad remains grouped with separate bodies "+m.InteriorId+" / "+pair.Key);}
  foreach(var pair in groups)OfficeSoldierPatrol2747(pair.Value.Find(r=>!SoldierProfiles2745.IsMarksman(r.Kind)).Actor);
  int before=G.Enemies.Count;G.SpawnOfficeSoldiers2747();Check(G.Enemies.Count==before&&records.Count==before,"2747 repeated spawn request does not duplicate soldiers "+m.InteriorId);
 }
 void OfficeSoldierPatrol2747(RaidActor e){var player=G.Player.transform.position;var start=e.transform.position;float cooldown=e.AttackCooldown;G.Player.transform.position=new Vector3(3000,0,3000);e.WaitTime=0;e.PatrolIndex=1;e.Route.Clear();float travelled=0;bool clear=true;for(int i=0;i<24;i++){var before=e.transform.position;G.TickRebel145(e,.05f);Physics.SyncTransforms();travelled+=Vector3.Distance(before,e.transform.position);clear&=OfficeNpcClear2747(e.transform.position);}Check(travelled>.3f&&clear&&G.ActiveGunShots145==0,"2747 actual squad guard patrol advances without scenery collision "+e.name);e.transform.position=start;e.Alert=0;e.Memory=0;e.WaitTime=2;e.PatrolIndex=0;e.AttackCooldown=cooldown;e.Route.Clear();e.RouteIndex=0;e.RepathTime=0;G.Player.transform.position=player;Physics.SyncTransforms();}
 void OfficeSoldierDamage2747(){var a=G.Enemies.ToArray();var hurt=a[0];hurt.Damage(11);hurt.NpcArmorPoints-=7;hurt.Alert=.73f;hurt.Memory=3.5f;var dead=a[1];dead.Damage(dead.Health+1);officeSoldierCache2747.Add(new OfficeSoldierCache2747{Map=G.Map,Actors=a,Hurt=hurt,Dead=dead,Position=hurt.transform.position,Health=hurt.Health,Armor=hurt.NpcArmorPoints,Alert=hurt.Alert,Memory=hurt.Memory});Check(dead.Dead&&!dead.HitCollider.enabled,"2747 actual lethal damage disables guard hit capsule "+G.Map.InteriorId);}
 void OfficeSoldierCached2747(){var state=officeSoldierCache2747.Find(q=>q.Map==G.Map);Check(state!=null&&G.Enemies.Count==state.Actors.Length&&Array.TrueForAll(state.Actors,e=>G.Enemies.Contains(e)),"2747 floor returns original soldiers without respawn "+G.Map.InteriorId);Check(state.Hurt.Health==state.Health&&state.Hurt.NpcArmorPoints==state.Armor&&state.Hurt.Alert==state.Alert&&state.Hurt.Memory==state.Memory&&state.Hurt.transform.position==state.Position&&state.Dead.Dead&&!state.Dead.HitCollider.enabled,"2747 damage armor alert memory death survive area cache "+G.Map.InteriorId);}
 void OfficeSoldierInactive2747(){G.ClearShots();foreach(var s in officeSoldierCache2747)if(s.Map!=G.Map){Check(!s.Map.Root.gameObject.activeInHierarchy&&Array.TrueForAll(s.Actors,e=>!G.Enemies.Contains(e)&&!e.gameObject.activeInHierarchy),"2747 previous floor actors excluded from active tick "+s.Map.InteriorId);var old=G.Enemies.ToArray();G.Enemies.Clear();G.Enemies.AddRange(s.Actors);G.TickEnemies(.5f);G.Enemies.Clear();G.Enemies.AddRange(old);Check(s.Hurt.Health==s.Health&&s.Hurt.NpcArmorPoints==s.Armor&&s.Hurt.Alert==s.Alert&&s.Hurt.Memory==s.Memory&&s.Hurt.transform.position==s.Position&&G.ActiveGunShots145==0,"2747 inactive floor cannot move or attack even if presented to enemy tick "+s.Map.InteriorId);}}
 IEnumerator OfficeSoldierPortal2747(string destination){
  var d=G.Map.Doors.Find(q=>q.Destination==destination);Check(d!=null,"2747 requested floor bridge portal exists "+destination);
  for(int n=0;n<G.Map.Doors.Count&&!OfficeReachable2727(d.Position);n++){
   WorldDoor next=null;Vector3 stand=Vector3.zero;float best=float.MaxValue;
   foreach(var candidate in G.Map.Doors)if(!candidate.Portal&&!candidate.Locked&&!candidate.Open&&candidate.Collider&&DoorStand2743(candidate,out var p)){float score=Vector3.Distance(p,d.Position);if(score<best){best=score;next=candidate;stand=p;}}
   Check(next!=null,"2747 route can open a reachable necessary door "+destination);FreightRoomWalk2717(stand,"2747 transition route door approach");FreightMonsterF2720(next,false);
  }
  FreightRoomWalk2717(d.Position,"2747 actual Actor.Move reaches transition "+destination);FreightMonsterF2720(d,true);yield return null;Check(CargoClear2713(G.Player.transform.position),"2747 actual held F arrival capsule safe "+destination);OfficeSoldierInactive2747();
 }
 IEnumerator OfficeSoldierPhoto2747(string name){
  var room=G.Map.OfficeRooms2726.Find(q=>q.Id=="03");bool paused=G.Paused,enabled=G.enabled;G.Paused=true;G.enabled=false;
  var actors=G.Enemies.ToArray();bool[] visibility=Array.ConvertAll(actors,e=>e.VisibleToPlayer);var go=new GameObject("OfficeSoldierReviewCamera2747");var cam=go.AddComponent<Camera>();cam.CopyFrom(G.View);cam.enabled=false;cam.allowHDR=false;cam.orthographic=true;cam.nearClipPlane=.1f;cam.farClipPlane=250;cam.orthographicSize=10;cam.transform.rotation=Quaternion.Euler(52,35,0);cam.transform.position=room.Center+Vector3.up-cam.transform.forward*110;
  float fog=Shader.GetGlobalFloat("_HarborFogEnabled");var ambient=RenderSettings.ambientLight;var sunObject=GameObject.Find("LateAfternoon");var sun=sunObject?sunObject.GetComponent<Light>():null;float power=sun?sun.intensity:0,shadowDistance=QualitySettings.shadowDistance;
  try{
   yield return null;
   Shader.SetGlobalFloat("_HarborFogEnabled",0);RenderSettings.ambientLight=new Color(.33f,.36f,.35f);if(sun)sun.intensity=.64f;QualitySettings.shadowDistance=170;
   foreach(var facing in G.Map.Root.GetComponentsInChildren<ShowcaseSpriteFacing2717>())facing.SyncCamera(cam);
   foreach(var e in actors){e.VisibleToPlayer=true;e.Draw(cam,0);}
   // Render immediately after forcing actual NPCs visible and facing this camera: no gameplay LateUpdate can hide them.
   var image=HvRender277(cam,name,1600,1100);try{CargoImageCheck2713(image,name);}finally{Destroy(image);}
  }finally{
   Shader.SetGlobalFloat("_HarborFogEnabled",fog);RenderSettings.ambientLight=ambient;if(sun)sun.intensity=power;QualitySettings.shadowDistance=shadowDistance;
   for(int i=0;i<actors.Length;i++){actors[i].VisibleToPlayer=visibility[i];actors[i].Draw(G.View,0);}foreach(var facing in G.Map.Root.GetComponentsInChildren<ShowcaseSpriteFacing2717>())facing.SyncCamera(G.View);Destroy(go);G.Paused=paused;G.enabled=enabled;
  }
 }
 IEnumerator RunOfficeSoldierCapture2747(){var input=G.Controls;bool paused=G.Paused;try{FreshCamp();G.Controls=new TouchInput();G.HarborWalkthrough=true;yield return G.BeginTrainingLoad2721();G.Paused=true;G.SwitchInterior("officeright1");yield return null;yield return OfficeSoldierPhoto2747("2747-right-first-floor-squad.png");G.SwitchInterior("office-floor:2:south");yield return null;G.SwitchInterior("office-link:bridge:right");yield return null;G.SwitchInterior("office-link:left:bridge");yield return null;yield return OfficeSoldierPhoto2747("2747-left-second-floor-squad.png");}finally{G.Controls=input;G.Paused=paused;}}
 IEnumerator RunOfficeSoldiers2747(){
  var input=G.Controls;string dir=Path.GetDirectoryName(G.SavePath);string[] formal={"stash-v40.json","stash-v41.json","stash-v42.json"};var bytes=Array.ConvertAll(formal,n=>File.Exists(Path.Combine(dir,n))?File.ReadAllBytes(Path.Combine(dir,n)):null);
  try{
   Check(Application.version=="2.7.47"&&G.VerifyMode&&G.SavePath.EndsWith("verification-stash-2.7.47.json"),"2747 isolated office soldier targeted QA");FreshCamp();G.Controls=new TouchInput();G.HarborWalkthrough=true;yield return G.BeginTrainingLoad2721();G.Paused=true;G.SwitchInterior("officeright1");yield return null;
   OfficeSoldierFloor2747();yield return OfficeSoldierPhoto2747("2747-right-first-floor-squad.png");OfficeSoldierDamage2747();
   yield return OfficeSoldierPortal2747("office-floor:2:south");OfficeSoldierFloor2747();OfficeSoldierDamage2747();
   yield return OfficeSoldierPortal2747("office-floor:3:south");OfficeSoldierFloor2747();OfficeSoldierDamage2747();
   yield return OfficeSoldierPortal2747("office-floor:2:south");OfficeSoldierCached2747();
   yield return OfficeSoldierPortal2747("office-link:bridge:right");Check(G.Enemies.Count==0,"2747 bridge remains free of soldiers");foreach(float x in new[]{4.55f,0,-4.55f})FreightRoomWalk2717(new Vector3(x,0,0),"2747 true bridge traversal");
   yield return OfficeSoldierPortal2747("office-link:left:bridge");OfficeSoldierFloor2747();yield return OfficeSoldierPhoto2747("2747-left-second-floor-squad.png");OfficeSoldierDamage2747();
   yield return OfficeSoldierPortal2747("office-floor:1:south");OfficeSoldierFloor2747();OfficeSoldierDamage2747();
   yield return OfficeSoldierPortal2747("office-floor:2:south");OfficeSoldierCached2747();
   yield return OfficeSoldierPortal2747("office-floor:3:south");OfficeSoldierFloor2747();OfficeSoldierDamage2747();
   yield return OfficeSoldierPortal2747("office-floor:2:south");OfficeSoldierCached2747();
   yield return OfficeSoldierPortal2747("office-floor:1:south");OfficeSoldierCached2747();yield return OfficeSoldierPortal2747("office-floor:2:south");OfficeSoldierCached2747();
   yield return OfficeSoldierPortal2747("office-link:bridge:left");yield return OfficeSoldierPortal2747("office-link:right:bridge");OfficeSoldierCached2747();
   yield return OfficeSoldierPortal2747("office-floor:3:south");OfficeSoldierCached2747();yield return OfficeSoldierPortal2747("office-floor:2:south");yield return OfficeSoldierPortal2747("office-floor:1:south");OfficeSoldierCached2747();
   int total=0;foreach(var s in officeSoldierCache2747)total+=s.Actors.Length;Check(officeSoldierCache2747.Count==6&&total==30,"2747 six tower floors total thirty soldiers without bridge or exterior additions");
   for(int i=0;i<formal.Length;i++)Check(MainBytes273(bytes[i],File.Exists(Path.Combine(dir,formal[i]))?File.ReadAllBytes(Path.Combine(dir,formal[i])):null),"2747 formal save unchanged "+formal[i]);
  }finally{G.Controls=input;G.ClearShots();G.Paused=true;G.CloseInventory();}
 }
}}
