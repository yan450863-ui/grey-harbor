using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
namespace GreyHarbor {
 public sealed partial class RaidVerification {
  IEnumerator VerifyRepairExterior149(){
   FreshCamp();G.HardMode=false;G.HarborWalkthrough=true;G.BeginTraining();G.Paused=true;yield return null;
   var m=G.Map;
   Check(G.SavePath.Contains("1.49")&&!G.SavePath.EndsWith("stash-v40.json"),"149 Repair verification uses an isolated save");
   Check(m.HarborWhitebox&&m.Bounds.width==208&&m.Bounds.height==180,"149 Existing harbor boundary and scale retained");
   Check(m.Loot.Count==30&&G.Enemies.Count==12,"149 Existing outdoor thirty containers and twelve enemies retained");
   var destinations=new[]{"portbooth","portservice","yardhome","yardshed","ferry1"};
   Check(m.Doors.Count==destinations.Length&&m.Doors.TrueForAll(d=>d.Portal&&Array.IndexOf(destinations,d.Destination)>=0),"149 Five existing exterior portals retained with no repair interior added");
   foreach(var destination in destinations)Check(m.Doors.FindAll(d=>d.Destination==destination).Count==1,"149 Existing portal preserved "+destination);
   Check(!m.Loot.Exists(l=>RaidMap.RepairBounds149.Contains(new Vector2(l.Position.x,l.Position.z)))&&!G.Enemies.Exists(e=>RaidMap.RepairBounds149.Contains(new Vector2(e.Home.x,e.Home.z))),"149 Repair exterior has no new loot or enemy spawns");
   Check(m.RepairDistrictRoot149&&m.RepairDistrictRoot149.IsChildOf(m.Root),"149 New repair scenery grouped under the repair district root");
   Check(m.RepairDistrictRoot149.GetComponentsInChildren<RaidActor>(true).Length==0&&m.RepairDistrictRoot149.GetComponentsInChildren<ContainerProp>(true).Length==0,"149 Repair scenery contains no search prop or enemy actor");
   var building=m.MainBuildings.Find(b=>b.Name=="维修主楼");
   Check(building!=null&&building.Floors==4&&Mathf.Approximately(building.Height,14.4f)&&Mathf.Approximately(building.Footprint.width,27)&&Mathf.Approximately(building.Footprint.height,32),"149 Repair main building retains four storeys and 27 by 32 metre footprint");
   foreach(var name in new[]{"维修主楼","维修附属棚"}){
    var shell=m.Root.Find(name);var col=shell?shell.GetComponent<BoxCollider>():null;
    Check(col&&col.enabled&&col.gameObject.layer==SceneObstacle.HighLayer,"149 Future interior remains a sealed high obstacle "+name);
    Check(!Clear140(new Vector3(col.bounds.center.x,0,col.bounds.center.z)),"149 Player cannot stand inside sealed shell "+name);
    var near=new Vector3(col.bounds.max.x+1.2f,.8f,col.bounds.center.z);var far=new Vector3(col.bounds.min.x-1.2f,.8f,col.bounds.center.z);var delta=far-near;
    Check(Physics.Raycast(near,delta.normalized,delta.magnitude,SceneObstacle.SightMask,QueryTriggerInteraction.Ignore)&&Physics.Raycast(near,delta.normalized,delta.magnitude,SceneObstacle.ShotMask,QueryTriggerInteraction.Ignore),"149 Sealed shell blocks sight and shots "+name);
   }
   Physics.SyncTransforms();var gate=RaidMap.YardGate;
   Check(Clear140(gate)&&G.Navigation.ClearSegment(gate+Vector3.back*2,gate+Vector3.forward*2),"149 Existing courtyard north gate remains open into the shared road");
   G.Player.transform.position=gate+Vector3.back*2;Physics.SyncTransforms();
   Check(Walk140(gate+Vector3.forward*3),"149 Player actually crosses courtyard north gate to repair approach");
   Check(m.RepairWalkPoints149.Count>=10,"149 Repair layout exposes its main walking destinations");
   foreach(var p in m.RepairWalkPoints149){
    Check(m.OnHarborSurface(p)&&Clear140(p),"149 Repair walk destination is clear and on land "+p);
    Check(Walk140(p),"149 Player actually walks successive repair destination "+p);
   }
   var entry=building.Approach;
   Check(Walk140(entry),"149 Player reaches the repair main building closed entrance");
   foreach(var p in new[]{RaidMap.PortPoint(44,105),RaidMap.PortPoint(44,123),RaidMap.PortPoint(26,125),RaidMap.PortPoint(10.5f,124),RaidMap.PortPoint(10.5f,95),RaidMap.PortPoint(10.5f,62),RaidMap.PortPoint(43,58),RaidMap.PortPoint(46,95),RaidMap.PortPoint(51,95)})
    Check(Clear140(p)&&Walk140(p),"149 Actual route passes around repair shells and joins main road "+p);
   // Keep both nearby approaches in the same world, rather than validating a distant spawn path.
   Check(Walk140(gate+Vector3.forward*3)&&Walk140(gate+Vector3.back*2),"149 Actual return route reaches the courtyard without a map transition");
   Check(G.Map==m&&!G.InsideBooth,"149 Repair route stays on the outdoor map");
   VerifyRepairObstacles149(m);
   var repairAssets=m.RepairDistrictRoot149.GetComponentsInChildren<RepairAssetInfo149>();
   foreach(var asset in new[]{"VehicleLift","ServiceCanopy","EngineHoist","AirCompressor","TyreRack","WeldingCart"})Check(Array.Exists(repairAssets,a=>a.Asset==asset),"149 Reusable workshop asset placed "+asset);
   foreach(var p in new[]{RaidMap.PortPoint(40.3f,70.5f),RaidMap.PortPoint(40.3f,73),RaidMap.PortPoint(40.3f,75.5f)})Check(Clear140(p)&&Walk140(p),"149 Player walks the service canopy side aisle "+p);
   var ground=m.Root.Find("Repair_GroundMaterials149");var regions=ground?ground.GetComponent<GroundSurfaceRegions>():null;
   Check(regions&&regions.Regions.Count>=4,"149 Repair area has several classified ground regions");
   foreach(var finish in new[]{GroundFinish.Asphalt,GroundFinish.PatchedConcrete,GroundFinish.Gravel,GroundFinish.Soil})
    Check(regions.Regions.Exists(r=>r.Finish==finish),"149 Repair ground finish present "+finish);
   Check(ground.GetComponentsInChildren<Renderer>().Length==0&&ground.GetComponentsInChildren<Collider>().Length==0,"149 Repair ground detail is baked without fragment renderers or collision");
   Check(m.StableGround137&&m.StableGround137.mipmapCount>1&&m.StableGround137.filterMode==FilterMode.Trilinear,"149 Repair textures retain filtered stable ground and mipmaps");
   var plane=m.Root.Find("Harbor_StableGround137");
   Check(plane&&plane.GetComponent<MeshFilter>().sharedMesh.vertexCount==4&&plane.GetComponentsInChildren<Collider>().Length==0,"149 One harbor ground plane presents the repair surface");
   int painted=0;bool opaque=true;var bounds=RaidMap.RepairBounds149;
   for(float x=bounds.xMin+1;x<bounds.xMax-1;x+=3)for(float z=bounds.yMin+1;z<bounds.yMax-1;z+=3){var p=new Vector3(x,0,z);if(!m.OnHarborSurface(p))continue;painted++;opaque&=RepairGroundPixel149(m,p).a>.98f;}
   Check(painted>100&&opaque,"149 Repair ground and shared boundaries have no unpainted cells");
   foreach(var p in new[]{gate,gate+Vector3.forward*3,RaidMap.PortPoint(28,54),RaidMap.PortPoint(43,54),RaidMap.PortPoint(46,95)})
    Check(RepairGroundPixel149(m,p).a>.98f,"149 Opaque ground at nearby road junction "+p);
   foreach(var p in new[]{RaidMap.PortPoint(3,50),RaidMap.PortPoint(1.5f,53),RaidMap.PortPoint(3,129.5f)}){
    Check(!m.OnHarborSurface(p),"149 Boundary paint sample is outside the actual harbor land union "+p);
    Check(RepairGroundPixel149(m,p).a<=.02f,"149 Stable repair ground leaves exterior water transparent "+p);
   }
   G.Player.transform.position=RaidMap.RepairPreviewSpawn149;G.Player.Aim=Vector3.forward;Physics.SyncTransforms();G.Rig.ResetView(45);G.Rig.Tick(0);G.Vision.Refresh(G.Player.transform.position,G.Player.Aim);G.Player.Draw(G.View,0);yield return null;var playable=RenderFrame();File.WriteAllBytes(Path.Combine(folder,"149-repair-play-view.png"),playable.EncodeToPNG());Destroy(playable);
   yield return RepairGroundMotion149();
   yield return RepairWorldPicture149("149-repair-south-front.png",RaidMap.PortPoint(30,78),45,22,false,1800,1400);
   yield return RepairWorldPicture149("149-repair-west-lane.png",RaidMap.PortPoint(12,105),135,23,false,1800,1400);
   yield return RepairWorldPicture149("149-repair-overview.png",RaidMap.PortPoint(27,92),45,43,false,1800,1800);
   yield return RepairWorldPicture149("149-repair-plan.png",RaidMap.PortPoint(24,90),0,45,true,1800,2200);
   var plan=m.HarborPlanTexture();File.WriteAllBytes(Path.Combine(folder,"149-harbor-plan.png"),plan.EncodeToPNG());
   Check(plan.width==416&&plan.height==360,"149 Full harbor plan retains world proportions");
   G.Controls=new DesktopInput();G.Finish(false);yield return null;
   Check(G.Phase==GamePhase.Base&&!m.Root&&G.Enemies.Count==0,"149 Leaving repair preview disposes the scene and returns to camp");
  }
  Color RepairGroundPixel149(RaidMap m,Vector3 p){var r=m.StableGroundBounds137;return r.Contains(new Vector2(p.x,p.z))?m.StableGround137.GetPixelBilinear((p.x-r.xMin)/r.width,(p.z-r.yMin)/r.height):Color.clear;}
  void VerifyRepairObstacles149(RaidMap m){
   int low=0,high=0;Collider lowSample=null,highSample=null;
   foreach(var c in m.RepairDistrictRoot149.GetComponentsInChildren<Collider>(true)){
    if(!c.enabled||c.isTrigger)continue;
    var rule=c.GetComponentInParent<SceneObstacle>();
    Check(rule&&rule.Kind!=ObstacleKind.Decoration&&(c.gameObject.layer==SceneObstacle.LowLayer||c.gameObject.layer==SceneObstacle.HighLayer),"149 Repair solid prop uses explicit collision class "+c.name);
    if(c.gameObject.layer==SceneObstacle.LowLayer){low++;if(!lowSample&&c.bounds.min.y<.9f&&c.bounds.max.y>.4f)lowSample=c;}
    else{high++;if(!highSample&&c.bounds.min.y<.9f&&c.bounds.max.y>1)highSample=c;}
   }
   Check(low>0&&high>0&&lowSample&&highSample,"149 Repair environment provides both low and high obstacles");
   foreach(var sample in new[]{lowSample,highSample}){
    var b=sample.bounds;var center=new Vector3(b.center.x,0,b.center.z);
    Check(!Clear140(center),"149 Actual movement capsule blocked by "+sample.gameObject.layer+" "+sample.name);
    var start=b.center-Vector3.right*(b.extents.x+.05f);float length=b.size.x+.10f;
    var hits=Physics.RaycastAll(start,Vector3.right,length,SceneObstacle.SightMask,QueryTriggerInteraction.Ignore);
    bool sight=Array.Exists(hits,h=>h.collider==sample);hits=Physics.RaycastAll(start,Vector3.right,length,SceneObstacle.ShotMask,QueryTriggerInteraction.Ignore);
    bool shot=Array.Exists(hits,h=>h.collider==sample);
    bool tall=sample.gameObject.layer==SceneObstacle.HighLayer;
    Check(sight==tall&&shot==tall,"149 Actual ray filters match low/high sight and shot rules "+sample.name);
   }
  }
  IEnumerator RepairGroundMotion149(){
   G.Player.transform.position=RaidMap.PortPoint(44,93);Physics.SyncTransforms();
   Check(Clear140(G.Player.transform.position)&&G.Navigation.ClearSegment(G.Player.transform.position,G.Player.transform.position+Vector3.forward*1.4f),"149 Ground movement sample has a clear walking strip");
   G.Rig.ResetView(45);G.Rig.Tick(0);Shader.SetGlobalFloat("_HarborFogEnabled",0);
   var samples=new[]{RaidMap.PortPoint(45.4f,93),RaidMap.PortPoint(45.4f,94),RaidMap.PortPoint(46,93),RaidMap.PortPoint(46,94)};
   Color[] baseline=null;float variation=0;bool inView=true;var start=G.Player.transform.position;
   for(int i=0;i<16;i++){
    G.Player.Move(Vector3.forward*.06f);G.Rig.Tick(0);Shader.SetGlobalFloat("_HarborFogEnabled",0);
    var frame=RenderFrame();var current=new Color[samples.Length];
    for(int n=0;n<samples.Length;n++){var vp=G.View.WorldToViewportPoint(samples[n]+Vector3.up*.05f);inView&=vp.x>0&&vp.x<1&&vp.y>0&&vp.y<1;current[n]=frame.GetPixelBilinear(vp.x,vp.y);if(baseline!=null)variation=Mathf.Max(variation,ColorDistance(current[n],baseline[n]));}
    if(baseline==null)baseline=current;
    if(i==0||i==15)File.WriteAllBytes(Path.Combine(folder,"149-ground-walk-"+i+".png"),frame.EncodeToPNG());Destroy(frame);yield return null;
   }
   Check(inView,"149 All tracked ground samples remain visible during the walk");
   Check(Vector3.Distance(G.Player.transform.position,start)>.8f,"149 Stability samples include actual player movement with following camera");
   results.Add("PROFILE 149 repair tracked ground RGB variation "+variation.ToString("F4"));
   Check(variation<.12f,"149 Repair ground colors remain stable while walking");Shader.SetGlobalFloat("_HarborFogEnabled",1);
  }
  IEnumerator RepairWorldPicture149(string name,Vector3 focus,float yaw,float zoom,bool top,int width,int height){
   yield return new WaitForEndOfFrame();Shader.SetGlobalFloat("_HarborFogEnabled",0);
   var ambient=RenderSettings.ambientLight;RenderSettings.ambientLight=new Color(.33f,.36f,.35f);
   var sunObject=GameObject.Find("LateAfternoon");var sun=sunObject?sunObject.GetComponent<Light>():null;float power=sun?sun.intensity:0;if(sun)sun.intensity=.68f;
   var go=new GameObject("QA149RepairCamera");var cam=go.AddComponent<Camera>();cam.CopyFrom(G.View);cam.enabled=false;
   cam.transform.rotation=Quaternion.Euler(top?90:55,yaw,0);cam.transform.position=focus+Vector3.up-cam.transform.forward*130;cam.orthographicSize=zoom;cam.farClipPlane=250;
   SaveYardCamera136(cam,name,width,height);Destroy(go);RenderSettings.ambientLight=ambient;if(sun)sun.intensity=power;Shader.SetGlobalFloat("_HarborFogEnabled",1);yield return null;
  }
 }
}


