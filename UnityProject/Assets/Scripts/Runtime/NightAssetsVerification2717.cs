using System;
using System.IO;
using System.Collections;
using UnityEngine;
namespace GreyHarbor {
 public sealed partial class RaidVerification {
  IEnumerator RunNightAssets2717(){
   Check(Application.version=="2.7.17"&&Path.GetFileName(G.SavePath)=="verification-stash-2.7.17.json","2717 Version and isolated verification save");
   string dir=Path.GetDirectoryName(G.SavePath);string[] formal={"stash-v40.json","stash-v41.json","stash-v42.json"};byte[][] before=Array.ConvertAll(formal,n=>File.Exists(Path.Combine(dir,n))?File.ReadAllBytes(Path.Combine(dir,n)):null);
   var controls=G.Controls;var random=UnityEngine.Random.state;var args=Environment.GetCommandLineArgs();bool capture=Array.IndexOf(args,"--capture-night-assets-2717")>=0,galleryOnly=Array.IndexOf(args,"--verify-gallery-2717")>=0||Array.IndexOf(args,"--verify-models-2717")>=0,freightOnly=Array.IndexOf(args,"--verify-freight-2717")>=0;
   galleryOnly|=GalleryContentOnly2717;FreshCamp();string inventory=JsonUtility.ToJson(G.Save);G.Controls=new TouchInput();Color originalAmbient=RenderSettings.ambientLight;var originalSunObject=GameObject.Find("LateAfternoon");var originalSun=originalSunObject?originalSunObject.GetComponent<Light>():null;float originalIntensity=originalSun?originalSun.intensity:0;
   try{
    if(!galleryOnly){G.HarborWalkthrough=true;Check(G.BeginTraining(),"2717 Actual full harbor preview begins");G.Paused=true;yield return null;
    Check(FreightPreview2716.Apply(G),"2717 Freight preview starts on clear route");
    if(!capture){FreightPhysical2716();yield return FreightRoutesCheck2716();yield return FreightSearchCheck2716();CheckFreightArt2717();yield return RunFreightRooms2717();}
    yield return CargoPicture2715("2717-freight-yard-overhead.png",RaidMap.PortPoint(27.5f,155),0,22,true,1600,1600);
    yield return CargoPicture2715("2717-freight-yard-oblique.png",RaidMap.PortPoint(27.5f,155),35,25,false,1800,1200);
    yield return CargoPicture2715("2717-truck-gate-road.png",RaidMap.PortPoint(11,161),115,10,false,1600,1100);
    FreightPlace2716(RaidMap.FreightExitPoint2716,"2717 final freight extraction");G.ExtractTime=0;G.TickExtraction(2);Check(G.Phase==GamePhase.Raid&&Mathf.Abs(G.ExtractTime-2)<.001f,"2717 Gate extraction retains five seconds");G.TickExtraction(3.05f);yield return null;Check(G.Phase==GamePhase.Base&&!G.TrainingActive&&JsonUtility.ToJson(G.Save)==inventory,"2717 Freight extraction restores inventory");
    }
    if(!freightOnly){G.AssetShowcaseMode2717=true;G.HarborWalkthrough=true;Check(G.BeginTraining(),"2717 Independent asset gallery begins");G.Paused=true;yield return null;Shader.SetGlobalFloat("_HarborFogEnabled",0);
    if(!capture)yield return RunAssetGallery2717();
    else yield return GalleryPictures2717();
    G.Player.transform.position=G.Map.Extraction;Physics.SyncTransforms();G.ExtractTime=0;G.TickExtraction(5.1f);yield return null;Check(G.Phase==GamePhase.Base&&!G.AssetShowcaseMode2717&&JsonUtility.ToJson(G.Save)==inventory,"2717 Gallery extraction restores inventory and exits temporary mode");
    var restoredSunObject=GameObject.Find("LateAfternoon");var restoredSun=restoredSunObject?restoredSunObject.GetComponent<Light>():null;Check(ColorDistance(RenderSettings.ambientLight,originalAmbient)<.0001f&&(!originalSun||restoredSun&&Mathf.Abs(restoredSun.intensity-originalIntensity)<.0001f),"2717 Gallery lighting restores the real camp profile after exit");
    }
    for(int i=0;i<formal.Length;i++)Check(MainBytes273(before[i],File.Exists(Path.Combine(dir,formal[i]))?File.ReadAllBytes(Path.Combine(dir,formal[i])):null),"2717 Formal save unchanged "+formal[i]);
   }finally{G.Controls=controls;UnityEngine.Random.state=random;G.Paused=true;File.WriteAllLines(Path.Combine(folder,"2717-freight-scene.csv"),freightRows2716);}
  }
  void CheckFreightArt2717(){
   var map=G.Map;Check(map.FreightClusters2717.Count==5,"2717 Five purpose-based scattered stock clusters");int total=0;foreach(var c in map.FreightClusters2717){Check(c.AcceptedGroups>=4,"2717 Populated irregular stock cluster "+c.Name+" count="+c.AcceptedGroups);total+=c.AcceptedGroups;}
   Check(total==map.FreightExteriorRecords2716.Count,"2717 All actually accepted stock groups belong to clusters");
   int angled=0,colored=0;foreach(var r in map.FreightExteriorRecords2716){var a=r.View.GetComponent<FreightStockAppearance2717>();Check(a&&a.PartCount>0,"2717 Actual stock appearance metadata "+r.Type);if(Mathf.Abs(Mathf.DeltaAngle(a.Yaw,Mathf.Round(a.Yaw/90)*90))>5)angled++;foreach(var renderer in r.View.GetComponentsInChildren<Renderer>())foreach(var m in renderer.sharedMaterials)if(m&&m.HasProperty("_Color")){var color=m.color;if(Mathf.Max(color.r,color.g,color.b)-Mathf.Min(color.r,color.g,color.b)>.06f)colored++;}}
   Check(angled>total*.65f&&colored>total,"2717 Majority of stock is oblique and retains wood, paint, sacks and metal colors angled="+angled+" colored="+colored);
   Check(map.FreightSurfaceFeatures2717.Count>=10&&map.FreightEdgeGrass2717.Count>=6,"2717 Old concrete, mud, gravel, tracks, cracks, oils and puddles replace clean grid");
   foreach(var f in map.FreightSurfaceFeatures2717)Check(f.Area.width*f.Area.height>0&&f.Polygon.Length>=3&&!string.IsNullOrEmpty(f.PaletteHex),"2717 Real ground feature "+f.Name);
   Check(map.FreightExternalRoad2716&&map.FreightExternalRoad2716.GetComponentsInChildren<Collider>().Length==0,"2717 Exterior road remains a noncolliding background");
   Check(map.Bounds==new Rect(-79,-73,208,180)&&!map.OnHarborSurface(RaidMap.PortPoint(4,161)),"2717 Playable harbor bounds retained");
   RaycastHit hit;Check(Physics.Raycast(RaidMap.PortPoint(8.5f,161)+Vector3.up*.8f,Vector3.left,out hit,5,SceneObstacle.MovementMask,QueryTriggerInteraction.Ignore)&&hit.collider==map.FreightBoundaryStop2716.GetComponent<Collider>(),"2717 Gate outer boundary still blocks beyond extraction");
  }
 }
}
