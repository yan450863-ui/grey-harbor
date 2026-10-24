using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace GreyHarbor {
 public sealed partial class RaidVerification {
  readonly List<string> freightRows2716=new List<string>{"type,name,x,z,detail"};
  void FreightRow2716(string type,string name,Vector3 p,string detail){freightRows2716.Add(type+","+Csv230(name)+","+CargoNumber2713(p.x)+","+CargoNumber2713(p.z)+","+Csv230(detail));}
  void FreightPlace2716(Vector3 p,string label){Check(CargoClear2713(p),"2716 Safe fixture position "+label+" "+p);G.Player.transform.position=p;Physics.SyncTransforms();G.Rig.Tick(0);G.Vision.Refresh(p,G.Player.Aim);}
  void FreightPhysical2716(){
   var map=G.Map;Check(map.FreightExteriorRecords2716.Count>=35,"2716 Freight yard has substantial grouped decoration count="+map.FreightExteriorRecords2716.Count);Check(map.FreightSearchRecords2716.Count==4,"2716 Exactly four real outdoor search containers");
   Check(map.FreightBuildingPlans2716.Count==2&&map.FreightTruck2716&&map.FreightGate2716&&map.FreightBoundaryStop2716,"2716 Two exterior buildings, overturned truck and working gate boundary exist");
   Check(Vector3.Dot(map.FreightTruck2716.transform.up,Vector3.up)<.65f,"2716 Freight truck is visibly overturned, not upright");
   float truckMin=float.MaxValue;foreach(var renderer in map.FreightTruck2716.GetComponentsInChildren<Renderer>())truckMin=Mathf.Min(truckMin,renderer.bounds.min.y);Check(truckMin>=.049f&&truckMin<.1f,"2716 Overturned truck rests on rendered ground without sinking or floating");
   Check(!map.FreightBoundaryStop2716.GetComponent<Renderer>().enabled&&map.FreightBoundaryStop2716.GetComponent<Collider>().enabled,"2716 Open truck gate preserves invisible outer map boundary");
   for(float z=157.4f;z<=164.61f;z+=.4f)Check(CargoClear2713(RaidMap.PortPoint(5.9f,z)),"2716 Real open gate spans truck-width frontage z="+z);
   var types=new HashSet<string>();var views=new HashSet<GameObject>();int overlaps=0;var all=map.Root.GetComponentsInChildren<Collider>();
   foreach(var r in map.FreightExteriorRecords2716){Check(r.View&&views.Add(r.View),"2716 Distinct visible decoration group "+r.Type);types.Add(r.Type);FreightRow2716("decoration",r.Type,r.View.transform.position,r.Footprint.ToString());
    foreach(var c in r.View.GetComponentsInChildren<Collider>())if(c.enabled&&!c.isTrigger&&(SceneObstacle.MovementMask&(1<<c.gameObject.layer))!=0)foreach(var other in all){if(!other.enabled||other.isTrigger||other.transform.IsChildOf(r.View.transform)||(SceneObstacle.MovementMask&(1<<other.gameObject.layer))==0||!c.bounds.Intersects(other.bounds))continue;Vector3 d;float depth;if(Physics.ComputePenetration(c,c.transform.position,c.transform.rotation,other,other.transform.position,other.transform.rotation,out d,out depth)&&depth>.015f){if(overlaps++<20)Debug.Log("2716 DECOR OVERLAP "+r.Type+" "+c.name+" / "+other.name+" depth="+depth);}}
   }
   Check(types.Count>=8&&overlaps==0,"2716 Varied grouped scenery has no collision interpenetration with other props/buildings types="+types.Count+" overlaps="+overlaps);
   Check(!G.Enemies.Exists(e=>RaidMap.FreightExteriorBounds2716.Contains(new Vector2(e.Home.x,e.Home.z))),"2716 Freight yard adds no monsters");
   Check(map.Exits.Count==2&&Vector3.Distance(map.Extraction,RaidMap.FreightExitPoint2716)<.001f&&map.Exits.Contains(RaidMap.FreightExitPoint2716),"2716 Existing northwest extraction relocates to truck gate without adding extra exits");
   Check(CargoClear2713(RaidMap.FreightExitPoint2716),"2716 Gate extraction trigger lies on free actual ground");
   Check(map.CargoBoxes2712.Count==64&&map.CargoLoot2714.Count==20&&map.CargoDressingRecords2714.Count==77,"2716 Central layout and searched cargo counts preserved while adding monsters");
  }
  IEnumerator FreightRoutesCheck2716(){
   Check(G.Map.FreightRoutes2716.Count>=4,"2716 Truck and pedestrian circulation routes recorded");
   foreach(var route in G.Map.FreightRoutes2716){FreightPlace2716(route.Points[0],route.Name+" start");for(int i=1;i<route.Points.Length;i++){CargoLaneWidth2713(route.Points[i-1],route.Points[i],route.Width,"2716 "+route.Name+"/"+i);Check(G.Navigation.ClearSegment(route.Points[i-1],route.Points[i]),"2716 Actual freight route sweep "+route.Name+"/"+i);CargoWalk2713(route.Points[i],"2716 "+route.Name+"/"+i);}yield return null;}
   var map=G.Map;Check(map.StableGround137&&map.StableGround137.mipmapCount>1,"2716 Freight ground uses stable filtered bake");int painted=0;foreach(var route in map.FreightRoutes2716)for(int i=1;i<route.Points.Length;i++){int steps=Mathf.CeilToInt(Vector3.Distance(route.Points[i-1],route.Points[i])*2);for(int j=0;j<=steps;j++){var p=Vector3.Lerp(route.Points[i-1],route.Points[i],j/(float)Mathf.Max(1,steps));if(CargoGroundPixel2715(p).a>.98f)painted++;else Check(false,"2716 Missing freight ground on route "+route.Name+" "+p);}}Check(painted>40,"2716 Freight and connecting routes have continuous actual painted ground samples="+painted);
  }
  IEnumerator FreightSearchCheck2716(){
   foreach(var r in G.Map.FreightSearchRecords2716){var box=r.Container;Check(G.Map.Loot.Contains(box)&&box.DropProfile230.Tier==ContainerTier.Common&&box.DropProfile230.FixedCount==0,"2716 New outdoor search point registered with ordinary existing rules "+box.Name);FreightPlace2716(r.AccessPoint,box.Name+" access");Check(G.Navigation.ClearSegment(r.AccessPoint,r.Stand),"2716 Actual clear search access "+box.Name);CargoWalk2713(r.Stand,"2716 actual search stand "+box.Name);G.Rig.Tick(0);G.Player.Aim=(box.Prop.transform.position-G.Player.transform.position).normalized;G.Vision.Refresh(G.Player.transform.position,G.Player.Aim);Check(G.CanInteractLoot(box),"2716 Real search prop visible and reachable "+box.Name);string before=JsonUtility.ToJson(new CargoItems2715{Items=box.Items.ToArray()});
    SprintInteract2711(false,0,box.Prop.transform.position);SprintInteract2711(false,0,box.Prop.transform.position);Check(G.Nearby==box,"2716 F selects intended outdoor container "+box.Name);SprintInteract2711(true,box.SearchDuration+.02f,box.Prop.transform.position);Check(box.Opened&&G.OpenLoot==box&&box.Prop.Searched&&before==JsonUtility.ToJson(new CargoItems2715{Items=box.Items.ToArray()}),"2716 Actual F opens container without contents reroll "+box.Name);G.Paused=false;for(int i=0;i<7;i++)G.TickIdentification(3);G.Paused=true;Check(box.Items.TrueForAll(item=>item.Identified),"2716 Search identification completes "+box.Name);G.CloseInventory();SprintInteract2711(false,0,box.Prop.transform.position);FreightRow2716("actual-F-search",box.Name,r.Stand,"items="+box.Items.Count);yield return null;
   }
  }
  void FreightHoldDoor2716(WorldDoor door){SprintInteract2711(false,0,door.Position);SprintInteract2711(false,0,door.Position);Check(G.NearbyDoor==door,"2716 Real F selects portal "+door.Name);SprintInteract2711(true,2.03f,door.Position);}
  IEnumerator FreightInteriorsCheck2716(){
   var outdoor=G.Map;var outdoorActors=G.Enemies.ToArray();var cargoActors=outdoor.CargoEnemies2716.ToArray();
   foreach(string key in new[]{"freightduty","freightwarehouse"}){
    var portal=outdoor.Doors.Find(d=>d.Portal&&d.Destination==key);Check(portal!=null,"2716 Correct exterior portal registered "+key);var stand=key=="freightduty"?RaidMap.PortPoint(34,141.5f):RaidMap.PortPoint(45.5f,163);FreightPlace2716(stand,key+" outside door");FreightHoldDoor2716(portal);yield return null;
    var indoor=G.Map;Check(indoor!=outdoor&&indoor.FreightInterior2716&&indoor.InteriorId==key&&G.InsideBooth,"2716 Real held F enters separate whitebox interior "+key);Check(indoor.Loot.Count==0&&G.Enemies.Count==0,"2716 Whitebox interior has no containers or monsters "+key);Check(indoor.FreightRooms2716.Count==(key=="freightduty"?4:1),"2716 Whitebox intended subroom count "+key);Check(CargoClear2713(G.Player.transform.position),"2716 Whitebox arrival has actual free ground "+key);
    foreach(var room in indoor.FreightRooms2716){var d=room.Door;if(d==null)continue;Check(!d.Open&&!G.Vision.RoomAllowed(new Vector3(room.Area.center.x,0,room.Area.center.y)),"2716 Closed whitebox room initially concealed "+room.Name);Vector3 approach=d.Position+Vector3.right*(room.Area.center.x<d.Position.x?.95f:-.95f);FreightPlace2716(approach,room.Name+" door side");SprintInteract2711(false,0,d.Position);SprintInteract2711(false,0,d.Position);Check(G.NearbyDoor==d,"2716 Room door selected by F "+room.Name);SprintInteract2711(true,0,d.Position);Check(d.Open&&G.Vision.RoomAllowed(new Vector3(room.Area.center.x,0,room.Area.center.y)),"2716 F opens and reveals whitebox room "+room.Name);SprintInteract2711(false,0,d.Position);CargoWalk2713(room.WalkPoint,"2716 room traversal "+room.Name);}
    yield return CargoPicture2715("2716-"+key+"-whitebox.png",Vector3.zero,35,9,false,1600,1200);
    var exit=indoor.FreightExit2716;FreightPlace2716(indoor.Spawn,key+" exit approach");FreightHoldDoor2716(exit);yield return null;Check(G.Map==outdoor&&Vector3.Distance(G.Player.transform.position,stand)<.1f,"2716 Exit returns to matching exterior doorway "+key);Check(G.Enemies.Count==outdoorActors.Length&&Array.TrueForAll(outdoorActors,e=>G.Enemies.Contains(e))&&Array.TrueForAll(cargoActors,r=>G.Map.CargoEnemies2716.Contains(r)),"2716 Outdoor actor identities survive whitebox visit "+key);
    FreightHoldDoor2716(portal);yield return null;Check(G.Map==indoor&&G.Map.Loot.Count==0&&G.Enemies.Count==0,"2716 Whitebox revisit restores same empty map "+key);foreach(var room in indoor.FreightRooms2716)if(room.Door!=null)Check(room.Door.Open,"2716 Whitebox door state cached "+room.Name);FreightPlace2716(indoor.Spawn,key+" cached exit");FreightHoldDoor2716(exit);yield return null;
   }
  }
  IEnumerator FreightPictures2716(){
   FreightPreview2716.Apply(G);yield return CargoPicture2715("2716-freight-yard-overhead.png",RaidMap.PortPoint(27.5f,155),0,22,true,1600,1600);yield return CargoPicture2715("2716-freight-yard-oblique.png",RaidMap.PortPoint(27.5f,155),35,25,false,1800,1200);yield return CargoPicture2715("2716-truck-gate-extraction.png",RaidMap.PortPoint(11,161),115,10,false,1600,1100);
   yield return CargoPicture2715("2716-overturned-truck-detail.png",G.Map.FreightTruck2716.transform.position+Vector3.up,125,8,false,1800,1200);
   foreach(var level in new[]{1,2}){var enemy=G.Map.CargoEnemies2716.Find(r=>r.Level==level&&r.Kind==(level==1?MonsterKind.Wanderer:MonsterKind.Spitter));FreightPlace2716(enemy.Targets[1],"actual monster photo stand "+level);G.Player.Aim=(enemy.Actor.transform.position-G.Player.transform.position).normalized;G.Vision.Refresh(G.Player.transform.position,G.Player.Aim);Check(G.Vision.CanSee(enemy.Actor.transform.position),"2716 Real player flashlight sees nearby monster level="+level);yield return null;yield return CargoPicture2715("2716-cargo-level"+level+"-monster-detail.png",enemy.Actor.transform.position,45,7,false,1600,1100);}
   var center=RaidMap.CargoBounds2712.center;yield return CargoPicture2715("2716-cargo-monsters-overview.png",new Vector3(center.x,0,center.y),35,42,false,1800,1200);
  }
  void FreightFinalArt2716(){
   var map=G.Map;Check(map.FreightExternalRoad2716&&map.FreightExternalRoad2716.GetComponentsInChildren<Collider>().Length==0,"2716 Gate exterior has visible non-colliding road background");
   Check(map.Bounds==new Rect(-79,-73,208,180)&&!map.OnHarborSurface(RaidMap.PortPoint(4,161)),"2716 Road backdrop does not expand playable harbor surface or bounds");
   var boundary=map.FreightBoundaryStop2716.GetComponent<Collider>();Check(boundary.enabled&&!map.FreightBoundaryStop2716.GetComponent<Renderer>().enabled,"2716 Gate outer boundary remains physically closed beyond extraction");
   RaycastHit hit;Check(Physics.Raycast(RaidMap.PortPoint(8.5f,161)+Vector3.up*.8f,Vector3.left,out hit,5,SceneObstacle.MovementMask,QueryTriggerInteraction.Ignore)&&hit.collider==boundary,"2716 Gate departure direction is stopped at existing outer map boundary");
   Check(CargoClear2713(RaidMap.FreightExitPoint2716)&&map.FreightExteriorRecords2716.Count==45&&map.FreightSearchRecords2716.Count==4,"2716 Art finish preserves actual extraction access and scenery/search counts");
  }
  IEnumerator VerifyFreightCargo2716(){
   Check(Application.version=="2.7.16"&&Path.GetFileName(G.SavePath)=="verification-stash-2.7.16.json","2716 Dedicated version and isolated verification save");string dir=Path.GetDirectoryName(G.SavePath);string[] formal={"stash-v40.json","stash-v41.json","stash-v42.json"};byte[][] before=Array.ConvertAll(formal,n=>File.Exists(Path.Combine(dir,n))?File.ReadAllBytes(Path.Combine(dir,n)):null);var controls=G.Controls;bool harbor=G.HarborWalkthrough;var random=UnityEngine.Random.state;FreshCamp();string inventory=JsonUtility.ToJson(G.Save);G.HarborWalkthrough=true;Check(G.BeginTraining(),"2716 Actual full harbor built in isolated preview session");G.Paused=true;yield return null;
   try{G.Controls=new TouchInput();Check(FreightPreview2716.Apply(G),"2716 Dedicated freight preview starts safely");bool capture=Array.IndexOf(Environment.GetCommandLineArgs(),"--capture-freight-cargo-2716")>=0;if(!capture){FreightPhysical2716();yield return FreightRoutesCheck2716();yield return FreightSearchCheck2716();yield return FreightInteriorsCheck2716();}else FreightFinalArt2716();yield return FreightPictures2716();if(!capture)yield return VerifyCargoEnemies2716();FreightPlace2716(RaidMap.FreightExitPoint2716,"actual extraction");G.ExtractTime=0;G.TickExtraction(2);Check(G.Phase==GamePhase.Raid&&Mathf.Abs(G.ExtractTime-2)<.001f,"2716 Gate starts existing five-second extraction countdown");G.TickExtraction(3.05f);yield return null;Check(G.Phase==GamePhase.Base&&!G.TrainingActive&&JsonUtility.ToJson(G.Save)==inventory,"2716 Actual gate extraction finishes preview and restores inventory");for(int i=0;i<formal.Length;i++)Check(MainBytes273(before[i],File.Exists(Path.Combine(dir,formal[i]))?File.ReadAllBytes(Path.Combine(dir,formal[i])):null),"2716 Formal save unchanged "+formal[i]);}
   finally{G.Controls=controls;G.HarborWalkthrough=harbor;UnityEngine.Random.state=random;G.Paused=true;File.WriteAllLines(Path.Combine(folder,Array.IndexOf(Environment.GetCommandLineArgs(),"--capture-freight-cargo-2716")>=0?"2716-freight-capture-scene.csv":"2716-freight-actual-scene.csv"),freightRows2716);}
  }
 }
}



