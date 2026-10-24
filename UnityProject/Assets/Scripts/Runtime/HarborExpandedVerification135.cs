using System.Collections;
using System.IO;
using UnityEngine;
namespace GreyHarbor {
 public sealed partial class RaidVerification {
  IEnumerator VerifyExpandedWhitebox135(){
   FreshCamp();G.VerificationSmallMap=false;G.NextMap=RaidDestination.Harbor;
   Check(G.StartRaid()&&G.Map.HarborWhitebox,"135 Formal departure loads expanded harbor");G.Paused=true;yield return null;
   var map=G.Map;var navigation=G.Navigation;
   Check(map.Bounds.width==190&&map.Bounds.height==180,"135 Harbor envelope is 190 by 180 metres");
   Check(map.Exits.Count==2&&map.Exits[0].x<0&&map.Exits[0].z>0&&map.Exits[1].x>0&&map.Exits[1].z<0,"135 Northwest and southeast extraction points");
   Check(map.MainBuildings.Count==3&&map.MainBuildings[0].Floors==4&&map.MainBuildings[1].Floors==4&&map.MainBuildings[2].Floors==5,"135 Three future main buildings reserve four four and five floors");
   foreach(var b in map.MainBuildings){
    var shell=map.Root.Find(b.Name);var col=shell.GetComponent<Collider>();
    Check(col&&col.bounds.size.y>=b.Height&&b.Height>=14&&shell.GetComponent<FutureMainBuilding>().Floors==b.Floors,"135 Main building has tall solid shell "+b.Name);
    Check(!Physics.CheckCapsule(b.Approach+Vector3.up*.35f,b.Approach+Vector3.up,.34f,SceneObstacle.MovementMask)&&navigation.FindPath(map.Spawn,b.Approach).Count>0,"135 Main building approach is reachable "+b.Name);
    var center=new Vector3(b.Footprint.center.x,0,b.Footprint.center.y);
    Check(!G.ClearSight(center-Vector3.forward*(b.Footprint.height/2+2),center+Vector3.forward*(b.Footprint.height/2+2)),"135 Main building blocks sight and shots "+b.Name);
   }
   Check(map.Doors.Count==4&&G.Enemies.Count==4&&map.Loot.Count==3&&map.SupplySites.Count==0,"135 Existing entrance content only; no new whitebox loot enemies or portals");
   Check(map.HarborSurfaces.Count==6,"135 Irregular mainland three dock decks and two connecting bridges");
   foreach(var p in map.RegionAnchors)Check(map.OnHarborSurface(p)&&navigation.FindPath(map.Spawn,p).Count>0,"135 District reachable "+p);
   foreach(var p in map.Exits)Check(navigation.FindPath(map.Spawn,p).Count>0,"135 Exit reachable "+p);
   foreach(var p in new[]{RaidMap.PortPoint(184,126),RaidMap.PortPoint(177,84),RaidMap.PortPoint(159,105),RaidMap.PortPoint(159,60),RaidMap.PortPoint(181,28)})
    Check(map.HarborSurfaceClear(p)&&navigation.FindPath(map.Spawn,p).Count>0,"135 Dock branch or bridge reachable "+p);
   int ground=0,stack=0;bool sideways=false,tilted=false;
   foreach(var b in map.CargoMazeBoxes){if(b.name.Contains("Stack")){stack++;continue;}ground++;float yaw=b.transform.eulerAngles.y;sideways|=yaw>75&&yaw<105;tilted|=Mathf.Abs(Mathf.DeltaAngle(yaw,0))>1&&Mathf.Abs(Mathf.DeltaAngle(yaw,90))>1;}
   Check(ground>=30&&stack>=6&&sideways&&tilted,"135 Cargo maze has mixed orientation short long and stacked blocks");
   foreach(var path in map.CargoPassages)for(int i=1;i<path.Length;i++)Check(navigation.ClearSegment(path[i-1],path[i]),"135 Reserved cargo passage is clear "+path[i]);
   var route=navigation.FindPath(map.Spawn,RaidMap.PortPoint(139,99));Check(route.Count>0,"135 Maze east exit connects to entrance");
   G.Player.transform.position=map.Spawn;bool walkOK=true;
   foreach(var p in route){int n=0;while(Vector3.Distance(G.Player.transform.position,p)>.08f&&n++<3000){var step=Vector3.ClampMagnitude(p-G.Player.transform.position,.15f);G.Player.Move(step);walkOK&=map.HarborSurfaceClear(G.Player.transform.position,.29f);}if(n>=3000)walkOK=false;}
   Check(walkOK&&Vector3.Distance(G.Player.transform.position,RaidMap.PortPoint(139,99))<.2f,"135 Player actually follows full route without clipping or crossing water");
   var water=RaidMap.PortPoint(174,108);Check(!map.OnHarborSurface(water)&&navigation.FindPath(map.Spawn,water).Count==0,"135 Water is excluded from navigation");
   Check(!navigation.ClearSegment(RaidMap.PortPoint(181,126),RaidMap.PortPoint(177,84)),"135 Navigation cannot shortcut across water between dock branches");
   G.Player.transform.position=RaidMap.PortPoint(185,126);Physics.SyncTransforms();for(int i=0;i<80;i++)G.Player.Move(Vector3.right*.15f);
   Check(map.HarborSurfaceClear(G.Player.transform.position,.29f)&&G.Player.transform.position.x<110,"135 Dock edge prevents walking off the platform");
   Vector3 yard=RaidMap.PortPoint(28,25);route=navigation.FindPath(map.Spawn,yard);bool crossed=false;var last=map.Spawn;
   foreach(var p in route){if((last.z+25)*(p.z+25)<=0&&Mathf.Abs(p.z-last.z)>.001f){float x=Mathf.Lerp(last.x,p.x,(-25-last.z)/(p.z-last.z));if(x>-54&&x<-48)crossed=true;}last=p;}
   Check(crossed,"135 Courtyard path enters through its sole north gate");
   Check(!navigation.ClearSegment(RaidMap.PortPoint(56,30),RaidMap.PortPoint(52,30)),"135 Cannot turn left from spawn through courtyard east fence");
   var seal=map.Box("QA-gate-seal",RaidMap.YardGate+Vector3.up,new Vector3(6.6f,2,.6f),"FFFFFF",true);Physics.SyncTransforms();
   var sealedNav=new RaidNavigation(map.Bounds,.75f,map.OnHarborSurface);Check(sealedNav.FindPath(map.Spawn,yard).Count==0,"135 Closing north gate disconnects courtyard");
   seal.SetActive(false);Destroy(seal);Physics.SyncTransforms();
   var plan=map.HarborPlanTexture();File.WriteAllBytes(Path.Combine(folder,"135-plan.png"),plan.EncodeToPNG());
   Check(plan.width==380&&plan.height==360&&plan.GetPixel(348,216)!=plan.GetPixel(132,108),"135 Minimap uses correct rectangular land-water plan");
   yield return WhiteboxOverview135("135-overview-top.png",true);
   yield return WhiteboxOverview135("135-overview-oblique.png",false);
   var views=new[]{map.Spawn,RaidMap.PortPoint(35,85),RaidMap.PortPoint(83,149),RaidMap.PortPoint(128,147),RaidMap.PortPoint(67,96),RaidMap.PortPoint(166,120),RaidMap.PortPoint(159,60)};
   var names=new[]{"135-entrance.png","135-repair-main.png","135-loading-main.png","135-office-main.png","135-cargo-maze.png","135-north-dock.png","135-dock-bridge.png"};
   for(int i=0;i<views.Length;i++){G.Player.transform.position=views[i];G.Player.Aim=Vector3.forward;Physics.SyncTransforms();G.Rig.ResetView(45);G.Rig.Tick(0);G.Vision.Refresh(views[i],G.Player.Aim);yield return Capture(names[i]);}
   G.Player.transform.position=map.Exits[1];G.TickExtraction(5.1f);Check(G.Phase==GamePhase.Base,"135 Southeast extraction works");
   FreshCamp();G.VerificationSmallMap=false;G.StartRaid();G.Paused=true;yield return null;G.Player.transform.position=G.Map.Exits[0];G.TickExtraction(5.1f);Check(G.Phase==GamePhase.Base,"135 Northwest extraction works");
   FreshCamp();var saved=File.ReadAllText(G.SavePath);G.HarborWalkthrough=true;G.BeginTraining();G.Paused=true;yield return null;
   Check(G.Map.Bounds.width==190&&G.TrainingActive&&File.ReadAllText(G.SavePath)==saved,"135 Direct harbor preview keeps formal loadout untouched");G.Finish(false);
  }
  IEnumerator WhiteboxOverview135(string name,bool top){
   yield return new WaitForEndOfFrame();Shader.SetGlobalFloat("_HarborFogEnabled",0);var ambient=RenderSettings.ambientLight;RenderSettings.ambientLight=new Color(.30f,.34f,.36f);var sun=GameObject.Find("LateAfternoon").GetComponent<Light>();float intensity=sun.intensity,shadowDistance=QualitySettings.shadowDistance;sun.intensity=.56f;QualitySettings.shadowDistance=500;
   var go=new GameObject("QA-expanded-overview");var cam=go.AddComponent<Camera>();cam.CopyFrom(G.View);cam.enabled=false;
   cam.transform.rotation=Quaternion.Euler(top?90:55,top?0:45,0);cam.transform.position=new Vector3(16,0,17)-cam.transform.forward*200;cam.orthographicSize=top?98:110;cam.farClipPlane=500;
   int w=top?2000:2200,h=top?1900:1600;var rt=new RenderTexture(w,h,24);cam.targetTexture=rt;cam.Render();var prior=RenderTexture.active;RenderTexture.active=rt;
   var tex=new Texture2D(w,h,TextureFormat.RGB24,false);tex.ReadPixels(new Rect(0,0,w,h),0,0);tex.Apply();File.WriteAllBytes(Path.Combine(folder,name),tex.EncodeToPNG());
   RenderTexture.active=prior;cam.targetTexture=null;rt.Release();Destroy(rt);Destroy(tex);Destroy(go);RenderSettings.ambientLight=ambient;sun.intensity=intensity;QualitySettings.shadowDistance=shadowDistance;Shader.SetGlobalFloat("_HarborFogEnabled",1);
  }
 }
}
