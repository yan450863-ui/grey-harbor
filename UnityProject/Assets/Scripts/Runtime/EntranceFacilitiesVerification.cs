using UnityEngine;
using System.Collections;
namespace GreyHarbor {public sealed partial class RaidVerification {
 IEnumerator VerifyFacilities130(){
  FreshCamp();G.HarborWalkthrough=true;G.BeginTraining();G.Paused=true;yield return null;
  foreach(var name in new[]{"GateKit_WaitingShelter","GateKit_CycleShelter","GateKit_Planter","GateKit_LitterBin","Entrance_FacilityPaving"})Check(G.Map.Root.Find(name)!=null,"130 Facility exists "+name);
  foreach(var p in new[]{new Vector3(.5f,0,-36),new Vector3(-1,0,-35.1f),new Vector3(2,0,-35.1f),new Vector3(20,0,-30.5f),new Vector3(7,0,-37),new Vector3(-13,0,-33)})Check(G.Navigation.FindPath(G.Map.Spawn,p).Count>0,"130 Waiting area, luggage search stands and pedestrian access "+p);
  Check(G.Map.Loot.Count==3&&G.Enemies.Count==4&&G.Map.Doors.Count==4,"134 Facilities with three containers and four patrols; two portals");
  var shelter=G.Map.Root.Find("GateKit_WaitingShelter");Check(!Physics.CheckSphere(new Vector3(.5f,.95f,-33),.25f,SceneObstacle.MovementMask),"130 Waiting shelter centre is walkable");
  G.Player.transform.position=new Vector3(.5f,0,-37);G.Rig.ResetView(45);G.Rig.Tick(0);yield return Capture("130-waiting-area.png");
  G.Player.transform.position=new Vector3(20,0,-31);G.Rig.ResetView(45);G.Rig.Tick(0);yield return Capture("130-cycle-shelter.png");G.Finish(false);
 }
}}
