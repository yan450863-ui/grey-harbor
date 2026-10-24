using UnityEngine;
using System.Collections;
using System.IO;
namespace GreyHarbor {
 public sealed partial class RaidVerification {
  IEnumerator VerifyYardBoundary137(){
   FreshCamp();G.HarborWalkthrough=true;G.BeginTraining();G.Paused=true;yield return null;
   var m=G.Map;var nav=G.Navigation;
   Check(m.Loot.Count==3&&G.Enemies.Count==4&&m.Doors.Count==4,"137 Existing entrance content retained; courtyard has no loot or enemies; two house entrances added");
   Check(RaidMap.LogisticsYardBounds.width>45&&RaidMap.LogisticsYardBounds.yMin<-64,"137 Yard extends to entrance sidewalk and south boundary");
   foreach(var name in new[]{"YardWest","YardEast","YardSouth","YardNorthWest","YardNorthEast"}){
    var wall=m.Root.Find(name);Check(wall&&wall.GetComponent<BoxCollider>()&&wall.GetComponentsInChildren<MeshFilter>().Length>5,"137 Finished boundary wall "+name);
    Check(wall.gameObject.layer==SceneObstacle.HighLayer,"137 Wall blocks movement vision and bullets "+name);
   }
   Check(!nav.ClearSegment(new Vector3(-23,0,-43),new Vector3(-27,0,-43)),"137 Entrance-side wall prevents direct yard entry");
   Check(!nav.ClearSegment(new Vector3(-48,0,-63),new Vector3(-48,0,-66)),"137 South boundary blocks passage");
   Check(!m.OnHarborSurface(new Vector3(-48,0,-65.6f)),"137 No walking strip exists outside south yard fence");
   Check(nav.ClearSegment(RaidMap.YardGate+Vector3.forward*3,RaidMap.YardGate+Vector3.back*3),"137 North gate remains open and usable");
   var seal=m.Box("QA137GateSeal",RaidMap.YardGate+Vector3.up,new Vector3(6.6f,2,.7f),"FFFFFF",true);Physics.SyncTransforms();
   Check(new RaidNavigation(m.Bounds,.75f,m.OnHarborSurface).FindPath(m.Spawn,RaidMap.PortPoint(49,33)).Count==0,"137 North gate is the only yard entrance");
   seal.SetActive(false);Destroy(seal);Physics.SyncTransforms();
   foreach(var p in m.YardWalkPoints)Check(nav.FindPath(m.Spawn,p).Count>0,"137 Expanded yard walking route "+p);
   foreach(var p in m.Exits)Check(nav.FindPath(m.Spawn,p).Count>0,"137 Extraction route retained "+p);
   Check(m.StableGround137&&m.StableGround137.mipmapCount>1&&m.StableGround137.filterMode==FilterMode.Trilinear,"137 Ground uses filtered texture with mipmaps");
   var floor=m.Root.Find("Harbor_StableGround137");Check(floor.GetComponent<MeshFilter>().sharedMesh.vertexCount==4&&floor.GetComponentsInChildren<Collider>().Length==0,"137 One stable ground plane adds no collision");
   foreach(var region in m.Root.GetComponentsInChildren<GroundSurfaceRegions>())Check(region.GetComponentsInChildren<Renderer>().Length==0,"137 Surface fragments removed "+region.name);
   foreach(var p in new[]{new Vector2(-26,-40),new Vector2(-24,-40),new Vector2(-52,-64),new Vector2(-52,-23)}){
    var c=m.StableGround137.GetPixelBilinear((p.x-m.StableGroundBounds137.xMin)/m.StableGroundBounds137.width,(p.y-m.StableGroundBounds137.yMin)/m.StableGroundBounds137.height);
    Check(c.a>.95f,"137 No white ground seam "+p);
   }
   int duplicate=0;foreach(var c in m.Root.GetComponentsInChildren<Collider>())if(c.name=="Harbor_PerimeterFence"&&c.bounds.Contains(new Vector3(-48,1,-65)))duplicate++;
   Check(duplicate==0,"137 Yard south wall replaces the duplicate map fence");
   G.Player.transform.position=RaidMap.PortPoint(50.5f,33);G.Rig.ResetView(45);G.Rig.Tick(0);Shader.SetGlobalFloat("_HarborFogEnabled",0);
   var samples=new[]{RaidMap.PortPoint(48,30),RaidMap.PortPoint(49,30),RaidMap.PortPoint(48,31),RaidMap.PortPoint(49,31)};
   Color[] baseline=null;float variation=0;Vector3 walkStart=G.Player.transform.position;
   for(int i=0;i<16;i++){
    G.Player.Move(Vector3.forward*.045f);G.Rig.Tick(0);
    var image=RenderFrame();var current=new Color[samples.Length];
    for(int n=0;n<samples.Length;n++){var vp=G.View.WorldToViewportPoint(samples[n]+Vector3.up*.051f);current[n]=image.GetPixelBilinear(vp.x,vp.y);if(baseline!=null)variation=Mathf.Max(variation,ColorDistance(current[n],baseline[n]));}
    if(i==0)baseline=current;if(i%5==0)File.WriteAllBytes(Path.Combine(folder,"137-ground-walk-"+i+".png"),image.EncodeToPNG());Destroy(image);yield return null;Shader.SetGlobalFloat("_HarborFogEnabled",0);
   }
   Check(Vector3.Distance(G.Player.transform.position,walkStart)>.4f,"137 Ground stability test includes actual camera-following movement");
   results.Add("PROFILE tracked ground RGB variation "+variation.ToString("F4"));
   Check(variation<.12f,"137 Ground colors remain stable while camera follows actual walking");
   Shader.SetGlobalFloat("_HarborFogEnabled",1);
   yield return VerifyLogisticsYard136();
   FreshCamp();G.HarborWalkthrough=true;G.BeginTraining();G.Paused=true;yield return null;
   foreach(var entry in new[]{new Vector3(-51,0,-29),new Vector3(-27.5f,0,-43),new Vector3(-51,0,-62)}){
    G.Player.transform.position=entry;G.Player.Aim=Vector3.forward;G.Rig.ResetView(entry.z<-60?225:45);G.Rig.Tick(0);G.Vision.Refresh(entry,G.Player.Aim);
    yield return Capture(entry.z<-60?"137-south-boundary.png":entry.x>-30?"137-entrance-seam.png":"137-north-gate.png");
   }
   yield return YardOverview137(false);yield return YardOverview137(true);
   G.Finish(false);
  }
  IEnumerator YardOverview137(bool top){
   yield return new WaitForEndOfFrame();Shader.SetGlobalFloat("_HarborFogEnabled",0);
   var ambient=RenderSettings.ambientLight;RenderSettings.ambientLight=new Color(.33f,.36f,.35f);var sun=GameObject.Find("LateAfternoon").GetComponent<Light>();float old=sun.intensity;sun.intensity=.64f;
   var go=new GameObject("QA137Overview");var cam=go.AddComponent<Camera>();cam.CopyFrom(G.View);cam.enabled=false;cam.transform.rotation=Quaternion.Euler(top?90:55,top?0:45,0);cam.transform.position=new Vector3(-44,1,-43)-cam.transform.forward*80;cam.orthographicSize=top?27:26;cam.farClipPlane=160;
   SaveYardCamera136(cam,top?"137-yard-plan.png":"137-yard-overview.png",1800,1400);Destroy(go);
   RenderSettings.ambientLight=ambient;sun.intensity=old;Shader.SetGlobalFloat("_HarborFogEnabled",1);yield return null;
  }
 }
}
