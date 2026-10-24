using UnityEngine;
using System;
using System.Collections;
namespace GreyHarbor {
 public sealed partial class RaidVerification {
  bool Clear140(Vector3 p)=>!Physics.CheckCapsule(p+Vector3.up*.35f,p+Vector3.up,.34f,SceneObstacle.MovementMask);
  void Patrol140(RaidActor e){
   Check(Clear140(e.Home),"140 Fixed monster spawn clear "+e.name);
   Check(e.Health==MonsterTuning.Health(e.Monster),"140 Existing monster health retained "+e.name);
   foreach(var p in e.PatrolTargets){var route=G.Navigation.FindPath(e.Home,p);Check(Clear140(p)&&route.Count>0&&Vector3.Distance(route[route.Count-1],p)<.2f,"140 Patrol waypoint reachable "+e.name+" "+p);}
   e.WaitTime=0;e.PatrolIndex=1;float distance=0;bool clear=true;
   for(int i=0;i<150;i++){var before=e.transform.position;G.HarborPatrol(e,.05f);Physics.SyncTransforms();distance+=Vector3.Distance(before,e.transform.position);clear&=!Physics.CheckCapsule(e.transform.position+Vector3.up*.35f,e.transform.position+Vector3.up,.29f,SceneObstacle.MovementMask);}
   Check(distance>.5f&&clear,"140 Actual patrol moves without clipping "+e.name);
   e.transform.position=e.Home;e.Route.Clear();e.WaitTime=2;e.PatrolIndex=0;Physics.SyncTransforms();
  }
  bool Walk140(Vector3 goal){var route=G.Navigation.FindPath(G.Player.transform.position,goal);if(route.Count==0)return false;
   foreach(var p in route){int n=0;while(Vector3.Distance(G.Player.transform.position,p)>.08f&&n++<1600){G.Player.Move(Vector3.ClampMagnitude(p-G.Player.transform.position,.09f));Physics.SyncTransforms();}if(n>=1600)return false;}return Vector3.Distance(G.Player.transform.position,goal)<.2f;
  }
  IEnumerator Search140(LootContainer loot){
   Check(Clear140(loot.Position)&&Walk140(loot.Position),"140 Walk to unobstructed search stand "+loot.Name);
   G.Player.Aim=(loot.Prop.transform.position-G.Player.transform.position).normalized;G.Rig.Tick(0);G.Vision.Refresh(G.Player.transform.position,G.Player.Aim);
   Check(G.CanInteractLoot(loot),"140 Search does not require distant flashlight alignment "+loot.Name);
   F111(false,0);F111(true,loot.SearchDuration+.02f);
   Check(loot.Opened&&G.OpenLoot==loot&&loot.Prop.Searched,"140 F searches correct container and opens its prop "+loot.Name);
   G.Paused=false;for(int i=0;i<6;i++)G.TickIdentification(3);G.Paused=true;
   Check(loot.Items.Count>=1&&loot.Items.Count<=6&&loot.Items.TrueForAll(p=>p.Identified&&RaidInventory.Grade(p.Kind)<4),"140 Common container contents identified with no red "+loot.Name);
   var cat=OutdoorContainers140.IsOutdoor(loot.Prop.Look)?OutdoorContainers140.Category(loot.Prop.Look):null;
   if(cat.HasValue)Check(loot.Items.TrueForAll(p=>RaidInventory.Group(p.Kind)==ItemGroup.Collection&&RaidInventory.Category(p.Kind)==cat.Value),"140 Outdoor category matches use "+loot.Name);
   var item=loot.Items[0];G.Paused=false;Check(G.QuickTransfer(ItemArea.Loot,item.Cell),"140 Identified item can be picked up "+loot.Name);G.Paused=true;G.CloseInventory();yield return null;
  }
  IEnumerator VerifyYardContent140(){
   FreshCamp();G.HardMode=false;G.HarborWalkthrough=true;G.BeginTraining();G.Paused=true;yield return null;
   var outside=G.Map;var outdoorActors=G.Enemies.ToArray();var yardActors=Array.FindAll(outdoorActors,e=>e.name.StartsWith("后勤院 / "));
   var yardLoot=outside.Loot.FindAll(l=>l.Prop&&OutdoorContainers140.IsOutdoor(l.Prop.Look));
   Check(G.SavePath.Contains("1.40")&&!G.SavePath.EndsWith("stash-v40.json"),"140 Verification uses isolated save");
   Check(outside.Loot.Count==6&&yardLoot.Count==3,"140 Low density: three entrance and three yard outdoor containers");
   Check(yardLoot.TrueForAll(l=>l.Tier==ContainerTier.Common&&l.Type!=SupplyType.Safe),"140 No premium or safe profile added outdoors");
   Check(!outside.Loot.Exists(l=>l.Prop&&(l.Prop.Look==ContainerLook.CargoCrate||l.Prop.Look==ContainerLook.FuelDrum)),"140 Cargo crate and chemical drum retained as assets only");
   Check(outdoorActors.Length==8&&yardActors.Length==4,"140 Four entrance patrols retained and four yard patrols added");
   foreach(MonsterKind kind in Enum.GetValues(typeof(MonsterKind)))Check(Array.FindAll(yardActors,e=>e.Monster==kind).Length==1,"140 One yard patrol per species "+kind);
   foreach(var e in yardActors){Check(Vector3.Distance(e.Home,RaidMap.PortPoint(28,42))>7&&outside.Doors.TrueForAll(d=>Vector3.Distance(d.Position,e.Home)>4),"140 Yard spawns away from arrival and house entries "+e.name);Patrol140(e);}
   G.Player.transform.position=RaidMap.PortPoint(28,42);Physics.SyncTransforms();
   foreach(var loot in yardLoot){yield return Search140(loot);G.Rig.ResetView(45);G.Rig.Tick(0);yield return Capture("140-yard-"+loot.Prop.Look+".png");}
   foreach(var key in new[]{"yardhome","yardshed"}){
    G.CloseInventory();G.Player.transform.position=key=="yardhome"?RaidMap.YardHomeApproach138:RaidMap.YardShedApproach138;Physics.SyncTransforms();G.Rig.ResetView(135);G.Rig.Tick(0);G.SwitchInterior(key);G.Paused=true;yield return null;
    var inside=G.Map;var actors=G.Enemies.ToArray();bool home=key=="yardhome";
    Check(inside.InteriorId==key&&actors.Length==(home?3:2)&&inside.Loot.Count==(home?6:4),"140 Fixed indoor density and unchanged container count "+key);
    Check(Array.TrueForAll(outdoorActors,e=>!e.gameObject.activeInHierarchy),"140 Outdoor actors inactive indoors "+key);
    Check(inside.Doors.TrueForAll(d=>d.Portal||!G.MonsterBlocksDoor(d)),"140 Initial monsters do not block door opening "+key);
    foreach(var e in actors){Check(Vector3.Distance(e.Home,inside.Spawn)>2.5f,"140 Room monster away from entry "+e.name);Patrol140(e);}
    foreach(var room in inside.YardRooms138){
     var center=new Vector3(room.Area.center.x,0,room.Area.center.y);
     Check(!G.Vision.Explored(center),"140 Closed room starts black "+room.Name);
     G.Player.transform.position=new Vector3(room.Door.Position.x*.25f,0,room.Door.Position.z);Physics.SyncTransforms();F111(false,0);F111(true,.01f);
     if(room.Door.Locked){Check(!room.Door.Open&&!G.Vision.Explored(center)&&!inside.Loot.Exists(l=>room.Area.Contains(new Vector2(l.Prop.transform.position.x,l.Prop.transform.position.z)))&&!Array.Exists(actors,e=>room.Area.Contains(new Vector2(e.Home.x,e.Home.z))),"140 Reserved key room locked and empty");continue;}
     Check(room.Door.Open&&G.Vision.Explored(center)&&Walk140(room.WalkPoint),"140 Room door opens and remains walkable with monsters "+room.Name);
    }
    foreach(var loot in inside.Loot.ToArray()){G.Bag.Slots.Clear();G.Chest.Slots.Clear();yield return Search140(loot);}
    G.Player.transform.position=inside.Spawn;G.Player.Aim=Vector3.forward;G.Rig.ResetView(45);G.Rig.Tick(0);G.Vision.Refresh(G.Player.transform.position,G.Player.Aim);yield return Capture("140-"+key+"-entry.png");
    yield return YardIndoorOverview138("140-"+key+"-overview.png",false);
    var remembered=actors[0];remembered.Health=37;var position=remembered.transform.position;G.Rig.ResetView(225);G.Rig.Tick(0);G.SwitchInterior(key);G.Paused=true;yield return null;
    Check(G.Map==outside&&G.Enemies.Count==8&&Array.TrueForAll(actors,e=>!e.gameObject.activeInHierarchy)&&Mathf.Abs(Mathf.DeltaAngle(G.Rig.Yaw,135))<.1f,"140 Return restores outdoor actors and camera "+key);
    G.SwitchInterior(key);G.Paused=true;yield return null;
    Check(G.Map==inside&&G.Enemies.Contains(remembered)&&remembered.Health==37&&remembered.transform.position==position&&Mathf.Abs(Mathf.DeltaAngle(G.Rig.Yaw,225))<.1f,"140 Re-entry keeps monster state and indoor camera "+key);
    if(home){
     var spit=Array.Find(actors,e=>e.Monster==MonsterKind.Spitter);G.Armor=G.Helmet=null;G.Player.Health=100;G.Player.transform.position=new Vector3(spit.transform.position.x,0,spit.transform.position.z-1);Physics.SyncTransforms();spit.AttackCooldown=0;spit.Alert=1;spit.Memory=7;G.TickMonster(spit,.01f);Check(spit.Windup>0&&G.Player.Health==100,"140 Indoor spitter has harmless windup");G.TickMonster(spit,.81f);Check(G.ActiveSpits==1,"140 Indoor spitter launches actual projectile");G.TickProjectiles(.25f);Check(G.Player.Health==80,"140 Indoor spit damages player by configured amount");
     var enemy=remembered;enemy.Health=MonsterTuning.Health(enemy.Monster);G.Player.transform.position=new Vector3(-3,0,-3.6f);G.Player.Aim=Vector3.forward;G.Player.Draw(G.View,0);var muzzle=G.Player.MuzzleOnPlane(G.View);enemy.transform.position=new Vector3(muzzle.x,0,muzzle.z+1.5f);Physics.SyncTransforms();int before=inside.Loot.Count,hits=Mathf.CeilToInt(enemy.Health/ArmorBalance.AKDamage);
     for(int i=0;i<hits-1;i++){G.SpawnShot(G.Player,Vector3.forward);G.TickProjectiles(.1f);}Check(!enemy.Dead,"140 Indoor monster survives until expected AK hit count");G.SpawnShot(G.Player,Vector3.forward);G.TickProjectiles(.1f);Check(enemy.Dead&&!enemy.HitCollider.enabled&&inside.Loot.Count==before+1,"140 Actual AK kills indoor monster and creates one corpse");
     spit.AttackDirection=Vector3.back;G.SpawnSpit(spit);G.SwitchInterior(key);Check(G.ActiveSpits==0&&!enemy.gameObject.activeInHierarchy,"140 Projectile cannot cross map transition");G.SwitchInterior(key);Check(G.Enemies.Contains(enemy)&&enemy.Dead&&inside.Loot.Count==before+1,"140 Dead monster does not respawn and corpse persists on re-entry");
    }
    G.Player.transform.position=inside.Spawn;G.SwitchInterior(key);G.Paused=true;yield return null;
   }
   yield return OutdoorGallery140();G.Controls=new DesktopInput();G.Finish(false);yield return null;Check(!outside.Root&&G.Enemies.Count==0&&G.ActiveSpits==0,"140 Leaving preview disposes monsters containers and shots");
  }
  IEnumerator OutdoorGallery140(){
   var map=new RaidMap{Root=new GameObject("OutdoorContainerLibrary140").transform};var origin=new Vector3(110,0,110);map.Box("LibraryFloor",origin+Vector3.down*.12f,new Vector3(18,.2f,8),"53665F");var art=new ContainerArt(map);int i=0;
   foreach(var look in new[]{ContainerLook.OutdoorTote,ContainerLook.GardenChest,ContainerLook.CargoCrate,ContainerLook.ElectricalCabinet,ContainerLook.FuelDrum})art.Build(look,origin+new Vector3(-6+3*i++,0,0),0,false);
   var target=G.Rig.Target;var focus=new GameObject("LibraryFocus140");focus.transform.SetParent(map.Root);focus.transform.position=origin;float yaw=G.Rig.Yaw,zoom=G.View.orthographicSize;G.Rig.Target=focus.transform;G.Rig.ResetView(15);G.Rig.Tick(0);G.View.orthographicSize=5.7f;Shader.SetGlobalFloat("_HarborFogEnabled",0);yield return Capture("140-outdoor-containers-closed.png");
   foreach(var prop in map.Root.GetComponentsInChildren<ContainerProp>()){Check(prop.Opening&&prop.transform.Find("Body"),"140 Reusable prop has searchable body and opening "+prop.Look);var initial=prop.Opening.localRotation;prop.SetSearched();Check(prop.Searched&&prop.Opening.localRotation!=initial,"140 Opening visible "+prop.Look);var opened=prop.Opening.localRotation;prop.SetSearched();Check(prop.Opening.localRotation==opened,"140 Opening idempotent "+prop.Look);}
   yield return Capture("140-outdoor-containers-open.png");G.Rig.Target=target;G.Rig.ResetView(yaw);G.Rig.Tick(0);G.View.orthographicSize=zoom;G.Vision.Bind();Destroy(map.Root.gameObject);
  }
 }
}
