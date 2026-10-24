using UnityEngine;
using System;
using System.Collections;
using System.IO;
namespace GreyHarbor {
 public sealed partial class RaidVerification {
  IEnumerator VerifyDock141(){
   FreshCamp();string saved=JsonUtility.ToJson(G.Save);G.HarborWalkthrough=true;G.HardMode=false;G.BeginTraining();G.Paused=true;yield return null;var map=G.Map;
   Check(G.SavePath.Contains("1.41")&&map.Bounds.width==190&&map.Bounds.height==180,"141 Isolated preview and unchanged full map scale");
   Check(map.DockBoats141.Count==2&&map.HarborSurfaces.Count==10,"141 Two boats and two gangways extend existing land union");
   Check(map.DockLoot141.Count==8&&map.Loot.Count==14,"141 Eight dispersed dock sites, previous six outdoor sites retained");
   Check(map.DockLoot141.FindAll(l=>l.Tier==ContainerTier.Advanced).Count==1&&map.DockLoot141.FindAll(l=>l.Tier==ContainerTier.Common).Count==7&&!map.DockLoot141.Exists(l=>l.Type==SupplyType.Safe),"141 Seven common and one advanced container, no safe added");
   Check(map.Doors.Count==4&&!map.ExteriorBuildings.Exists(r=>r.Contains(new Vector2(RaidMap.DockPreviewSpawn141.x,RaidMap.DockPreviewSpawn141.z))),"141 No dock indoor portals");
   foreach(var name in new[]{"服务码头小屋","码头维修小屋","码头货物棚","Dock_CargoPlaceholder"})Check(!map.Root.Find(name),"141 Solid dock placeholder removed "+name);
   Check(map.StableGround137&&map.StableGround137.mipmapCount>1&&map.StableGround137.filterMode==FilterMode.Trilinear,"141 Dock paint shares stable mipmapped ground");
   Check(map.HarborSurfaceClear(RaidMap.DockPreviewSpawn141)&&Clear140(RaidMap.DockPreviewSpawn141),"141 Dock preview arrival clear");
   var monsters=G.Enemies.FindAll(e=>e.name.StartsWith("码头 / "));
   Check(monsters.Count==4&&G.Enemies.Count==12,"141 Four pier patrols plus eight existing outdoor monsters");
   foreach(var e in monsters){Check(Vector3.Distance(e.Home,RaidMap.DockPreviewSpawn141)>7&&Vector3.Distance(e.Home,map.Exits[1])>10,"141 Pier monster avoids arrival and extraction "+e.name);Patrol140(e);}
   foreach(var p in new[]{RaidMap.PortPoint(182,128),RaidMap.PortPoint(177,84),RaidMap.PortPoint(159,60),map.Exits[1],RaidMap.YardGate}){var path=G.Navigation.FindPath(map.Spawn,p);Check(Clear140(p)&&path.Count>0&&Vector3.Distance(path[path.Count-1],p)<.2f,"141 Platform branch, bridge or connection remains reachable "+p);}
   foreach(var boat in map.DockBoats141){
    G.Player.transform.position=boat.Shore;Physics.SyncTransforms();Check(map.HarborSurfaceClear(boat.Board)&&Clear140(boat.Board)&&Walk140(boat.Board)&&Walk140(boat.Deck),"141 Actor actually boards by gangway "+boat.Name);
    Check(boat.Model&&boat.Model.Find("BoatDeck141")&&boat.Model.GetComponent<DockAssetInfo141>(),"141 Complete reusable vessel model "+boat.Name);
    G.Player.Aim=Vector3.forward;G.Rig.ResetView(45);G.Rig.Tick(0);G.Vision.Refresh(G.Player.transform.position,G.Player.Aim);yield return Capture(boat.Name=="旧渔船"?"141-fishing-boat.png":"141-work-barge.png");
    G.Player.transform.position=boat.Center+Vector3.right*(boat.Name=="旧渔船"?1.35f:1.8f)+(boat.Name=="旧渔船"?Vector3.forward*.9f:Vector3.back*1.3f);Physics.SyncTransforms();Check(Clear140(G.Player.transform.position),"141 Hull edge check starts clear "+boat.Name);for(int i=0;i<40;i++){G.Player.Move(Vector3.right*.12f);Physics.SyncTransforms();}Check(map.HarborSurfaceClear(G.Player.transform.position,.29f)&&G.Player.transform.position.x<boat.Center.x+(boat.Name=="旧渔船"?1.8f:2.3f),"141 Hull edge stops walking into sea "+boat.Name);
    Check(Walk140(boat.Deck)&&Walk140(boat.Shore),"141 Actor returns over gangway "+boat.Name);
   }
   var water=RaidMap.PortPoint(174,110);Check(!map.OnHarborSurface(water)&&G.Navigation.FindPath(map.Spawn,water).Count==0,"141 Sea remains nonwalkable outside ships");
   Check(!G.Navigation.ClearSegment(RaidMap.PortPoint(184,126),RaidMap.PortPoint(177,84)),"141 Ships do not create water shortcut between platforms");
   var shoreLoot=map.DockLoot141.FindAll(l=>l!=map.DockLoot141[1]&&l!=map.DockLoot141[6]&&l!=map.DockLoot141[7]);bool dispersed=true;for(int i=0;i<shoreLoot.Count;i++)for(int j=i+1;j<shoreLoot.Count;j++)dispersed&=Vector3.Distance(shoreLoot[i].Position,shoreLoot[j].Position)>8;Check(dispersed,"141 Shore searches separated across work zones");
   G.Player.transform.position=RaidMap.DockPreviewSpawn141;Physics.SyncTransforms();
   foreach(var loot in map.DockLoot141){
    var look=loot.Prop.Look;var category=DockContainers141.Category(look);if(category.HasValue)Check(loot.Items.TrueForAll(p=>RaidInventory.Group(p.Kind)==ItemGroup.Collection&&RaidInventory.Category(p.Kind)==category.Value),"141 Container category matches role "+look);if(look==ContainerLook.MarineCooler)Check(loot.Items.TrueForAll(p=>DockContainers141.CoolerEligible(p.Kind)),"141 Cooler yields living or biological supplies");
    G.Bag.Slots.Clear();G.Chest.Slots.Clear();yield return Search140(loot);G.Rig.ResetView(45);G.Rig.Tick(0);yield return Capture("141-search-"+look+".png");
    for(int q=0;q<4;q++){G.Rig.ResetView(45+q*90);G.Rig.Tick(0);Check(G.CanInteractLoot(loot),"141 Close search valid after camera rotation "+look+" "+q);}
   }
   foreach(ContainerLook look in Enum.GetValues(typeof(ContainerLook)))if(DockContainers141.IsDock(look)){
    var rng=new System.Random(141+(int)look);bool valid=true,noRed=true;for(int n=0;n<1000;n++){var tier=look==ContainerLook.DocumentTube?ContainerTier.Advanced:ContainerTier.Common;var contents=DockContainers141.Contents(look,rng,n>=500,tier);valid&=contents.Count>=1&&contents.Count<=6;foreach(var p in contents){noRed&=RaidInventory.Grade(p.Kind)<4;var cat=DockContainers141.Category(look);if(cat.HasValue)valid&=RaidInventory.Category(p.Kind)==cat.Value;if(look==ContainerLook.MarineCooler)valid&=DockContainers141.CoolerEligible(p.Kind);}}
    Check(valid&&noRed,"141 New profile samples retain count category and red exclusion "+look);
   }
   var high=map.DockLoot141.Find(l=>l.Prop.Look==ContainerLook.MarineControlBox).Prop.transform.Find("Body").GetComponent<Collider>();Check(high.gameObject.layer==SceneObstacle.HighLayer,"141 Control cabinet blocks movement sight and shots");
   var low=map.DockLoot141.Find(l=>l.Prop.Look==ContainerLook.MarineCooler).Prop.transform.Find("Body").GetComponent<Collider>();Check(low.gameObject.layer==SceneObstacle.LowLayer,"141 Cooler is movement-only low obstacle");
   G.Player.transform.position=RaidMap.PortPoint(169,26);G.Player.Aim=Vector3.right;G.Player.Draw(G.View,0);var enemy=monsters[3];var oldHome=enemy.Home;var muzzle=G.Player.MuzzleOnPlane(G.View);enemy.transform.position=new Vector3(muzzle.x+2,0,muzzle.z);enemy.Health=MonsterTuning.Health(enemy.Monster);Physics.SyncTransforms();int corpses=map.Loot.Count,hits=Mathf.CeilToInt(enemy.Health/ArmorBalance.AKDamage);for(int i=0;i<hits;i++){G.SpawnShot(G.Player,Vector3.right);G.TickProjectiles(.1f);}Check(enemy.Dead&&map.Loot.Count==corpses+1,"141 Actual AK kills pier monster and produces one corpse");
   G.Player.transform.position=RaidMap.YardHomeApproach138;G.SwitchInterior("yardhome");G.Paused=true;yield return null;Check(G.Enemies.Count==3&&!enemy.gameObject.activeInHierarchy,"141 Pier actors isolate from existing yard interior");G.SwitchInterior("yardhome");G.Paused=true;Check(G.Map==map&&G.Enemies.Contains(enemy)&&enemy.Dead&&map.Loot.Count==corpses+1,"141 Dock searches and monster death persist across interior visit");
   yield return DockOverview141();yield return DockContainerGallery141();yield return DockBoatGallery141();
   G.Player.transform.position=map.Exits[1];G.Controls=new TouchInput();G.Paused=false;G.TickExtraction(5.1f);yield return null;Check(G.Phase==GamePhase.Base&&JsonUtility.ToJson(G.Save)==saved,"141 Southeast extraction restores preview inventory");
  }
  IEnumerator DockOverview141(){
   yield return new WaitForEndOfFrame();Shader.SetGlobalFloat("_HarborFogEnabled",0);var go=new GameObject("DockOverview141");var cam=go.AddComponent<Camera>();cam.CopyFrom(G.View);cam.enabled=false;cam.transform.rotation=Quaternion.Euler(90,0,0);cam.transform.position=RaidMap.PortPoint(163,85)+Vector3.up*150;cam.orthographicSize=65;cam.farClipPlane=300;SaveYardCamera136(cam,"141-dock-overview-top.png",1500,2200);
   cam.transform.rotation=Quaternion.Euler(55,45,0);cam.transform.position=RaidMap.PortPoint(170,35)-cam.transform.forward*100;cam.orthographicSize=29;SaveYardCamera136(cam,"141-south-dock-overview.png",1800,1300);Destroy(go);G.Vision.Bind();yield return null;
  }
  IEnumerator DockContainerGallery141(){
   var map=new RaidMap{Root=new GameObject("MarineContainerGallery141").transform};var origin=new Vector3(240,0,180);map.Box("DisplayFloor",origin+Vector3.down*.12f,new Vector3(27,.2f,10),"536B67");var art=new ContainerArt(map);int i=0;foreach(ContainerLook look in Enum.GetValues(typeof(ContainerLook)))if(DockContainers141.IsDock(look))art.Build(look,origin+new Vector3(-9+3*i++,0,0),0,false);
   var target=G.Rig.Target;var focus=new GameObject("MarineFocus141");focus.transform.SetParent(map.Root);focus.transform.position=origin;float yaw=G.Rig.Yaw,zoom=G.View.orthographicSize;G.Rig.Target=focus.transform;G.Rig.ResetView(15);G.Rig.Tick(0);G.View.orthographicSize=7.5f;Shader.SetGlobalFloat("_HarborFogEnabled",0);yield return Capture("141-marine-containers-closed.png");
   foreach(var prop in map.Root.GetComponentsInChildren<ContainerProp>()){Check(prop.Opening&&prop.transform.Find("Body"),"141 Reusable marine container has body and opening "+prop.Look);var before=prop.Opening.localRotation;prop.SetSearched();Check(prop.Opening.localRotation!=before,"141 Marine container visibly opens "+prop.Look);var after=prop.Opening.localRotation;prop.SetSearched();Check(prop.Opening.localRotation==after,"141 Marine container opening idempotent "+prop.Look);}
   yield return Capture("141-marine-containers-open.png");G.Rig.Target=target;G.Rig.ResetView(yaw);G.Rig.Tick(0);G.View.orthographicSize=zoom;G.Vision.Bind();Destroy(map.Root.gameObject);
  }
  IEnumerator DockBoatGallery141(){
   var map=new RaidMap{Root=new GameObject("BoatLibrary141").transform};var origin=new Vector3(240,0,180);map.Box("GalleryWater",origin+Vector3.down*.63f,new Vector3(38,.3f,27),"3F666E");var kit=new DockLivingKit141(map);kit.FishingBoat(origin+Vector3.left*6);kit.WorkBarge(origin+Vector3.right*6);
   var target=G.Rig.Target;var focus=new GameObject("BoatFocus141");focus.transform.SetParent(map.Root);focus.transform.position=origin;float yaw=G.Rig.Yaw,zoom=G.View.orthographicSize;G.Rig.Target=focus.transform;G.Rig.ResetView(15);G.Rig.Tick(0);G.View.orthographicSize=10;Shader.SetGlobalFloat("_HarborFogEnabled",0);yield return Capture("141-boat-library.png");G.Rig.Target=target;G.Rig.ResetView(yaw);G.Rig.Tick(0);G.View.orthographicSize=zoom;G.Vision.Bind();Destroy(map.Root.gameObject);
  }
 }
}
