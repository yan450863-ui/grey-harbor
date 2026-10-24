using System.Collections;
using UnityEngine;
namespace GreyHarbor { public sealed partial class RaidVerification {
 IEnumerator VerifyEntrance128(){
  FreshCamp();G.HarborWalkthrough=true;G.BeginTraining();G.Paused=true;yield return null;
  Check(G.Map.Root.Find("Entrance_Gatehouse")!=null&&G.Map.Root.Find("Entrance_ServiceBuilding")!=null,"128 Both entrance buildings use finished exterior kit");
  Check(G.Map.Root.Find("入口岗亭")==null&&G.Map.Root.Find("入口附属房")==null,"128 Replaced whitebox building volumes removed");
  Check(G.Map.Root.Find("维修主楼")!=null&&G.Map.Root.Find("装卸主楼")!=null,"128 Other districts remain whitebox");
  Check(G.Map.Loot.Count==3&&G.Enemies.Count==4&&G.Map.Doors.Count==4,"134 Entrance content and two portals");
  foreach(var p in new[]{new Vector3(-13,0,-59),new Vector3(-13,0,-49),new Vector3(-13,0,-30),new Vector3(-1.3f,0,-53),new Vector3(22,0,-43),new Vector3(18,0,-49),new Vector3(30,0,-57)})
   Check(G.Navigation.FindPath(G.Map.Spawn,p).Count>0,"128 Entrance route/door approach reachable "+p);
  Check(!G.Navigation.ClearSegment(new Vector3(-13,0,-62),new Vector3(-13,0,-66)),"128 Port arrival gate does not leak outside map");
  Check(!G.Navigation.ClearSegment(new Vector3(-2,0,-53),new Vector3(3,0,-53)),"128 Gatehouse remains sealed");
  Check(!G.ClearSight(new Vector3(-2,0,-53),new Vector3(8,0,-53)),"128 Finished building still blocks sight and gunfire");
  var region=G.Map.Root.Find("Entrance_GroundMaterials");Check(region!=null&&region.GetComponentInChildren<Collider>()==null,"128 Ground materials cannot snag movement");
  bool bounded=true;foreach(var r in region.GetComponent<GroundSurfaceRegions>().Regions)bounded&=r.Area.xMin>=RaidMap.EntranceArtBounds.xMin-.001f&&r.Area.xMax<=RaidMap.EntranceArtBounds.xMax+.001f&&r.Area.yMin>=RaidMap.EntranceArtBounds.yMin-.001f&&r.Area.yMax<=RaidMap.EntranceArtBounds.yMax+.001f;
  Check(bounded,"128 Ground art remains inside entrance district");
  int low=0;foreach(var c in G.Map.Root.GetComponentsInChildren<Collider>())if(c.name=="Entrance_Bollard"){low++;Check(c.gameObject.layer==SceneObstacle.LowLayer,"128 Bollard blocks movement but not sight/shots");}Check(low==4,"128 Four low bollards placed off road");
  var building=G.Map.Root.Find("Entrance_Gatehouse");Check(building.GetComponentInChildren<Collider>().gameObject.layer==SceneObstacle.HighLayer,"128 Gatehouse has high obstacle collision");
  G.Player.transform.position=G.Map.Spawn;G.Rig.Tick(0);yield return Capture("128-arrival.png");
  G.Player.transform.position=new Vector3(-2,0,-53);G.Rig.Tick(0);yield return Capture("128-gatehouse.png");
  G.Rig.ResetView(135);G.Rig.Tick(0);yield return Capture("128-gatehouse-rotated.png");
  G.Player.transform.position=new Vector3(22,0,-46);G.Rig.Tick(0);yield return Capture("128-service-building.png");
  G.Finish(false);
 }
}}
