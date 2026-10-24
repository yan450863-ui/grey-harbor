using UnityEngine;
using System.Collections;
using System.IO;
namespace GreyHarbor {
 public sealed partial class RaidVerification {
  IEnumerator VerifyLogisticsYard136(){
   FreshCamp();G.HarborWalkthrough=true;G.BeginTraining();G.Paused=true;yield return null;
   var map=G.Map;var nav=G.Navigation;var props=map.Root.GetComponentsInChildren<YardAssetInfo>();
   Check(map.Bounds.width==190&&map.Bounds.height==180&&map.MainBuildings.Count==3,"136 Expanded harbor proportions preserved");
   Check(map.Loot.Count==3&&G.Enemies.Count==4&&map.Doors.Count==4&&map.SupplySites.Count==0,"136 No courtyard loot or monsters added; four exterior portals");
   Check(System.Array.FindAll(props,p=>p.Asset=="VegetableBed"||p.Asset=="FallowBed").Length==2,"136 One vegetable bed and one neglected bed");
   Check(System.Array.FindAll(props,p=>p.Asset=="Bicycle"||p.Asset=="OldBicycle").Length==2,"136 Two parked bicycles");
   Check(System.Array.FindAll(props,p=>p.Asset=="YoungTree"||p.Asset=="MatureTree").Length>=6,"136 Trees with different heights");
   foreach(var id in new[]{"WashStation","Clothesline","PicnicSet","RainwaterTank","CompostBay","GardenLeanTo","LowMeadow","TallGrass"})
    Check(System.Array.Exists(props,p=>p.Asset==id),"136 New outdoor asset placed "+id);
   foreach(var prop in props){
    var cols=prop.GetComponentsInChildren<Collider>();
    if(prop.Decoration||prop.Asset.Contains("Bed"))Check(cols.Length==0,"136 Ground plants remain traversable "+prop.Asset);
    else Check(cols.Length>0&&System.Array.TrueForAll(cols,c=>c.gameObject.layer==SceneObstacle.HighLayer||c.gameObject.layer==SceneObstacle.LowLayer),"136 Prop uses explicit obstacle classification "+prop.Asset);
    if(prop.Asset.Contains("Bicycle")||prop.Asset=="PicnicSet"||prop.Asset=="WashStation")Check(System.Array.TrueForAll(cols,c=>c.gameObject.layer==SceneObstacle.LowLayer),"136 Low domestic prop passes vision and shots "+prop.Asset);
    if(prop.Asset.Contains("Tree"))Check(cols.Length==1&&cols[0].bounds.size.x<=.51f,"136 Only the tree trunk collides "+prop.Asset);
   }
   var soil=map.Root.Find("Yard_GroundMaterials").GetComponent<GroundSurfaceRegions>();
   Check(soil.Regions.Count>=8&&soil.Regions[0].Finish==GroundFinish.Soil&&soil.Regions[0].Area==RaidMap.LogisticsYardBounds,"136 Large dirt area with garden soil and footpaths");
   Check(soil.GetComponentsInChildren<Collider>().Length==0,"136 Surface detail has no accidental collisions");
   foreach(var p in map.YardWalkPoints)Check(!Physics.CheckCapsule(p+Vector3.up*.35f,p+Vector3.up,.34f,SceneObstacle.MovementMask)&&nav.FindPath(map.Spawn,p).Count>0,"136 Courtyard destination clear and reachable "+p);
   bool walked=true;G.Player.transform.position=map.Spawn;
   foreach(var goal in map.YardWalkPoints){var path=nav.FindPath(G.Player.transform.position,goal);foreach(var p in path){int n=0;while(Vector3.Distance(G.Player.transform.position,p)>.08f&&n++<1500){G.Player.Move(Vector3.ClampMagnitude(p-G.Player.transform.position,.13f));Physics.SyncTransforms();}if(n>=1500)walked=false;}walked&=Vector3.Distance(G.Player.transform.position,goal)<.2f;}
   Check(walked,"136 Player walks from north gate to both house fronts garden and living area");
   foreach(var name in new[]{"旧院棚屋A","旧院棚屋B"}){var c=map.Root.Find(name).GetComponent<Collider>();Check(c.gameObject.layer==SceneObstacle.HighLayer&&Physics.CheckSphere(c.bounds.center,.2f,SceneObstacle.SightMask),"136 Courtyard house remains sealed "+name);}
   Check(!nav.ClearSegment(RaidMap.PortPoint(56,30),RaidMap.PortPoint(52,30)),"136 Spawn-side east fence remains sealed");
   var seal=map.Box("QA-yard-seal",RaidMap.YardGate+Vector3.up,new Vector3(6.6f,2,.6f),"FFFFFF",true);Physics.SyncTransforms();
   Check(new RaidNavigation(map.Bounds,.75f,map.OnHarborSurface).FindPath(map.Spawn,RaidMap.PortPoint(28,25)).Count==0,"136 Only north gate enters courtyard");
   seal.SetActive(false);Destroy(seal);Physics.SyncTransforms();foreach(var p in map.Exits)Check(nav.FindPath(map.Spawn,p).Count>0,"136 Harbor extraction still reachable "+p);
   var plan=map.HarborPlanTexture();var soilPixel=plan.GetPixel(60,78);Check(soilPixel.r>soilPixel.b&&soilPixel.g>soilPixel.b,"136 Courtyard soil also appears on minimap");
   var positions=new[]{RaidMap.PortPoint(28,42),RaidMap.PortPoint(23,24),RaidMap.PortPoint(28,35),RaidMap.PortPoint(34,36),RaidMap.PortPoint(33,26)};
   var names=new[]{"136-yard-north-gate.png","136-garden.png","136-bicycles.png","136-living-area.png","136-wash-and-utility.png"};
   for(int i=0;i<positions.Length;i++){G.Player.transform.position=positions[i];G.Player.Aim=Vector3.forward;G.Rig.ResetView(i==0||i==2?225:45);G.Rig.Tick(0);G.Vision.Refresh(positions[i],G.Player.Aim);yield return Capture(names[i]);}
   yield return YardOverview136("136-yard-overview.png",false);yield return YardOverview136("136-yard-plan.png",true);yield return YardAssetGallery136();
   G.Finish(false);Check(G.Phase==GamePhase.Base&&!G.HarborWalkthrough&&!G.TrainingActive,"136 Preview returns to camp");
  }
  IEnumerator YardOverview136(string name,bool top){
   yield return new WaitForEndOfFrame();Shader.SetGlobalFloat("_HarborFogEnabled",0);
   var ambient=RenderSettings.ambientLight;RenderSettings.ambientLight=new Color(.33f,.36f,.35f);var sun=GameObject.Find("LateAfternoon").GetComponent<Light>();float intensity=sun.intensity;sun.intensity=.64f;
   var go=new GameObject("QA-yard-overview");var cam=go.AddComponent<Camera>();cam.CopyFrom(G.View);cam.enabled=false;cam.transform.rotation=Quaternion.Euler(top?90:55,top?0:45,0);cam.transform.position=RaidMap.PortPoint(27,29)+Vector3.up*1-cam.transform.forward*70;cam.orthographicSize=top?22:23;cam.farClipPlane=150;
   SaveYardCamera136(cam,name,1800,1400);Destroy(go);RenderSettings.ambientLight=ambient;sun.intensity=intensity;Shader.SetGlobalFloat("_HarborFogEnabled",1);yield return null;
  }
  void SaveYardCamera136(Camera cam,string name,int w,int h){var rt=new RenderTexture(w,h,24);cam.targetTexture=rt;cam.Render();var prior=RenderTexture.active;RenderTexture.active=rt;var tex=new Texture2D(w,h,TextureFormat.RGB24,false);tex.ReadPixels(new Rect(0,0,w,h),0,0);tex.Apply();File.WriteAllBytes(Path.Combine(folder,name),tex.EncodeToPNG());RenderTexture.active=prior;cam.targetTexture=null;rt.Release();Destroy(rt);Destroy(tex);}
  IEnumerator YardAssetGallery136(){
   var library=new RaidMap{Root=new GameObject("QA-yard-asset-gallery").transform};library.Root.position=new Vector3(500,0,500);var k=new YardLivingKit(library);
   // Positions are initially world coordinates; retain them by moving the complete root afterwards.
   library.Root.position=Vector3.zero;
   k.Bicycle(new Vector3(-13,0,-5));k.Bicycle(new Vector3(-8,0,-5),0,true);k.GardenBed(new Vector3(-1,0,-5));k.GardenBed(new Vector3(6,0,-5),true);
   k.WashStation(new Vector3(13,0,-5));k.PicnicSet(new Vector3(-13,0,3));k.Clothesline(new Vector3(-6,0,3));k.WaterTank(new Vector3(1,0,3));k.Compost(new Vector3(5,0,3));k.LeanTo(new Vector3(12,0,3));
   k.Grass(new Vector3(-11,0,11),4,4,false,136);k.Grass(new Vector3(-4,0,11),4,4,true,136);k.Tree(new Vector3(3,0,11),0);k.Tree(new Vector3(11,0,11),1);
   library.Root.position=new Vector3(500,0,500);yield return null;Shader.SetGlobalFloat("_HarborFogEnabled",0);
   var go=new GameObject("QA-yard-library-camera");var cam=go.AddComponent<Camera>();cam.CopyFrom(G.View);cam.enabled=false;cam.transform.rotation=Quaternion.Euler(55,0,0);cam.transform.position=new Vector3(500,0,505)-cam.transform.forward*80;cam.orthographicSize=18;cam.farClipPlane=150;
   var ambient=RenderSettings.ambientLight;RenderSettings.ambientLight=new Color(.32f,.36f,.34f);var sun=GameObject.Find("LateAfternoon").GetComponent<Light>();float power=sun.intensity;sun.intensity=.68f;
   SaveYardCamera136(cam,"136-yard-assets.png",2000,1300);Destroy(go);Destroy(library.Root.gameObject);RenderSettings.ambientLight=ambient;sun.intensity=power;Shader.SetGlobalFloat("_HarborFogEnabled",1);
  }
 }
}
