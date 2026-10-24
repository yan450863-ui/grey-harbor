using UnityEngine;
using System.Collections;
namespace GreyHarbor {public sealed partial class RaidVerification {
 IEnumerator VerifyParking129(){
  FreshCamp();G.HarborWalkthrough=true;G.BeginTraining();G.Paused=true;yield return null;
  foreach(var name in new[]{"GateKit_Sedan","GateKit_ServiceVan","GateKit_ShuttleBus","GateKit_Tree_Small","GateKit_Tree_Medium","GateKit_Tree_Large","GateKit_LawnPatch","GateKit_LowGrass","GateKit_RaisedBarrier","GateKit_WheelStop"})Check(G.Map.Root.Find(name)!=null,"129 Placed reusable asset "+name);
  foreach(Transform t in G.Map.Root){if(t.name.StartsWith("GateKit_Lawn")||t.name.StartsWith("GateKit_LowGrass"))Check(t.GetComponentsInChildren<Collider>().Length==0,"129 Grass has no movement or sight collision");if(t.name.StartsWith("GateKit_Tree")){var c=t.GetComponentsInChildren<Collider>();Check(c.Length==1&&c[0].bounds.size.x<.6f,"129 Only trunk collides, canopy does not block ground");}}
  foreach(var p in new[]{new Vector3(14.5f,0,-49.3f),new Vector3(20.5f,0,-49.3f),new Vector3(31.5f,0,-48.5f)})Check(Physics.CheckSphere(p+Vector3.up,.2f,SceneObstacle.SightMask),"129 Vehicle blocks movement/sight/shots "+p);
  foreach(var p in new[]{new Vector3(-13,0,-58),new Vector3(-13,0,-30),new Vector3(17.5f,0,-49.3f),new Vector3(25,0,-48),new Vector3(22,0,-43),new Vector3(37,0,-56)})Check(G.Navigation.FindPath(G.Map.Spawn,p).Count>0,"129 Parking aisles and road remain accessible "+p);
  G.Player.transform.position=new Vector3(17.5f,0,-54);G.Rig.ResetView(45);G.Rig.Tick(0);yield return Capture("129-parking.png");
  G.Player.transform.position=new Vector3(27,0,-49);G.Rig.ResetView(135);G.Rig.Tick(0);yield return Capture("129-bus.png");
  G.Player.transform.position=new Vector3(-18,0,-40);G.Rig.ResetView(45);G.Rig.Tick(0);yield return Capture("129-trees.png");
  G.Finish(false);
 }
}}
