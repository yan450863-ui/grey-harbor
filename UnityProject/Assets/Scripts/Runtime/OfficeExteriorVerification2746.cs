using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
namespace GreyHarbor {
 public sealed partial class RaidVerification {
  IEnumerator RunOfficeExterior2746(){
   int start=results.Count;var controls=G.Controls;var random=UnityEngine.Random.state;bool harbor=G.HarborWalkthrough;
   string directory=Path.GetDirectoryName(G.SavePath);string[] formal={"stash-v40.json","stash-v41.json","stash-v42.json"};byte[][] before=Array.ConvertAll(formal,n=>File.Exists(Path.Combine(directory,n))?File.ReadAllBytes(Path.Combine(directory,n)):null);
   Check(Application.version=="2.7.46"&&G.VerifyMode&&G.SavePath.EndsWith("verification-stash-2.7.46.json"),"2746 isolated exterior build and verification save");
   try{
    FreshCamp();string camp=JsonUtility.ToJson(G.Save);G.Controls=new TouchInput();G.HarborWalkthrough=true;yield return G.BeginTrainingLoad2721();G.Paused=true;var outdoor=G.Map;var actors=G.Enemies.ToArray();int loot=outdoor.Loot.Count;
    Check(outdoor.HarborWhitebox&&!outdoor.IsInterior&&outdoor.GroundCacheUsed2721,"2746 actual prepared harbor ground retained");
    Check(outdoor.Bounds==new Rect(-79,-73,208,180),"2746 original harbor extent retained");
    Check(outdoor.OfficeShells2727.Count==2,"2746 exactly two separated office exterior plans");
    var origin=RaidMap.PortPoint(60,153);var plot=new Rect(origin.x,origin.z,43,21);
    foreach(var shell in outdoor.OfficeShells2727){
     Check(Mathf.Abs(shell.Footprint.width-16)<.01f&&Mathf.Abs(shell.Footprint.height-18)<.01f&&shell.Floors==3&&Mathf.Abs(shell.Height-10.2f)<.01f,"2746 compact 16 by 18 three-storey shell "+shell.Name);
     Check(plot.Contains(shell.Footprint.min)&&plot.Contains(shell.Footprint.max),"2746 exterior footprint remains within its original map plot "+shell.Name);
    }
    if(outdoor.OfficeShells2727.Count==2){var a=outdoor.OfficeShells2727[0].Footprint;var b=outdoor.OfficeShells2727[1].Footprint;Check(!a.Overlaps(b)&&Mathf.Abs(Mathf.Abs(a.center.x-b.center.x)-23)<.01f,"2746 tower ground gap remains seven metres");}
    Check(outdoor.OfficeBridge2727,"2746 elevated exterior second-floor link exists");
    if(outdoor.OfficeBridge2727){var b=outdoor.OfficeBridge2727.GetComponent<Renderer>().bounds;Check(b.min.y>3.1f&&Mathf.Abs(b.size.x-7)<.05f,"2746 bridge deck remains elevated over ground path");}
    OfficeExteriorAppearance2746(outdoor);
    Check(outdoor.Doors.FindAll(d=>d.Portal&&d.Destination=="officeright1").Count==1&&!outdoor.Doors.Exists(d=>d.Portal&&d.Destination.StartsWith("office")&&d.Destination!="officeright1"),"2746 only the authored right main entrance connects from outdoors");
    var left=RaidMap.PortPoint(70,155);var blocked=outdoor.Root.Find("OfficeBlockedCar2746");Check(blocked&&blocked.GetComponentsInChildren<Collider>().Length>0,"2746 overturned car is the real blocked left-entry object");Check(Physics.CheckSphere(RaidMap.PortPoint(70,153.6f)+Vector3.up*.7f,.18f,SceneObstacle.MovementMask,QueryTriggerInteraction.Ignore),"2746 left entrance vehicle has physical blocking volume");
    Check(OfficePreview2726.Apply(G)&&CargoClear2713(G.Player.transform.position),"2746 authored outdoor preview start remains capsule safe");
    FreightRoomPlace2717(RaidMap.PortPoint(70,150.5f),"2746 clear real player start in front of left-entry overturned vehicle");var leftStart=G.Player.transform.position;bool clearVehicle=true;
    for(int step=0;step<50;step++){G.Player.Move(Vector3.forward*.10f);Physics.SyncTransforms();clearVehicle&=!Physics.CheckCapsule(G.Player.transform.position+Vector3.up*.35f,G.Player.transform.position+Vector3.up,.3f,SceneObstacle.MovementMask,QueryTriggerInteraction.Ignore);}
    var leftStop=G.Player.transform.position;Check(leftStop.z>leftStart.z+.2f&&leftStop.z<RaidMap.PortPoint(70,153.6f).z-.3f&&clearVehicle,"2746 actual Actor.Move advances towards blocked left door then stops without penetrating vehicle "+leftStop);
    RaycastHit vehicleHit;Check(Physics.CapsuleCast(leftStop+Vector3.up*.35f,leftStop+Vector3.up,.3f,Vector3.forward,out vehicleHit,.35f,SceneObstacle.MovementMask,QueryTriggerInteraction.Ignore)&&vehicleHit.collider.transform.IsChildOf(blocked),"2746 true movement stopped by overturned vehicle rather than building wall");
    // Actual player uses a .30m radius; retreat before using the route helper's conservative .34m clearance.
    G.Player.Move(Vector3.back*.4f);Physics.SyncTransforms();Check(CargoClear2713(G.Player.transform.position),"2746 blocked-left-door player can retreat safely without trapping");
    FreightRoomPlace2717(RaidMap.PortPoint(83,137),"2746 inherited original office road fixture");
    foreach(var p in new[]{RaidMap.PortPoint(83,150),RaidMap.PortPoint(93,150),RaidMap.OfficePreviewSpawn2726,RaidMap.OfficeArrival2726}){Check(outdoor.OnHarborSurface(p)&&outdoor.HarborSurfaceClear(p,.34f),"2746 inherited road and entrance supporting surface "+p);FreightRoomWalk2717(p,"2746 original road to main entrance");yield return null;}
    foreach(var p in new[]{RaidMap.PortPoint(81.5f,157),RaidMap.PortPoint(81.5f,167.5f),RaidMap.PortPoint(81.5f,172)}){Check(outdoor.OnHarborSurface(p)&&CargoClear2713(p),"2746 real player capsule clear in seven-metre gap "+p);FreightRoomWalk2717(p,"2746 true Actor.Move through courtyard and beneath bridge");yield return null;}
    yield return CargoPicture2715("2746-office-exterior-overview.png",RaidMap.PortPoint(81.5f,164)+Vector3.up*3,35,24,false,1800,1200);
    yield return CargoPicture2715("2746-office-exterior-rear.png",RaidMap.PortPoint(81.5f,164)+Vector3.up*3,215,24,false,1800,1200);
    yield return CargoPicture2715("2746-office-right-main-entry.png",RaidMap.PortPoint(93,155)+Vector3.up*1.6f,35,7,false,1600,1000);
    yield return CargoPicture2715("2746-office-left-blocked-entry.png",left+Vector3.up*1.6f,35,7,false,1600,1000);
    yield return CargoPicture2715("2746-office-bridge-ground-gap.png",RaidMap.PortPoint(81.5f,167.5f)+Vector3.up*1.5f,145,9,false,1600,1000);
    FreightRoomWalk2717(RaidMap.OfficeArrival2726,"2746 return to real right entrance");G.Rig.ResetView(45);G.Rig.Tick(0);G.Player.Aim=Vector3.forward;G.Vision.Refresh(G.Player.transform.position,G.Player.Aim);G.Player.Draw(G.View,0);yield return null;
    var image=HvRender277(G.View,"2746-office-exterior-live-camera.png",1600,900);try{CargoImageCheck2713(image,"2746 unchanged actual gameplay camera");}finally{Destroy(image);}
    var portal=outdoor.Doors.Find(d=>d.Portal&&d.Destination=="officeright1");FreightMonsterF2720(portal,true);yield return null;var indoor=G.Map;
    Check(indoor.OfficeWhitebox2726&&indoor.InteriorId=="officeright1"&&indoor.Loot.Count==25,"2746 actual held F enters existing finished right first floor");Check(Array.TrueForAll(actors,e=>!e.gameObject.activeInHierarchy),"2746 original exterior actors suspend indoors");
    FreightMonsterF2720(indoor.OfficeExit2726,true);yield return null;
    Check(G.Map==outdoor&&Vector3.Distance(G.Player.transform.position,RaidMap.OfficeArrival2726)<.1f&&outdoor.Loot.Count==loot&&G.Enemies.Count==actors.Length&&Array.TrueForAll(actors,e=>G.Enemies.Contains(e)),"2746 actual held F returns to original exterior cache with no actor or loot duplication");
    FreightMonsterF2720(portal,true);yield return null;Check(G.Map==indoor,"2746 repeated real entry reuses original finished indoor map");FreightMonsterF2720(indoor.OfficeExit2726,true);yield return null;Check(G.Map==outdoor&&outdoor.Loot.Count==loot,"2746 repeated exit keeps original exterior cache");
    G.Finish(false);yield return null;Check(G.Phase==GamePhase.Base&&JsonUtility.ToJson(G.Save)==camp,"2746 isolated exterior session returns unchanged camp inventory");
    for(int i=0;i<formal.Length;i++)Check(MainBytes273(before[i],File.Exists(Path.Combine(directory,formal[i]))?File.ReadAllBytes(Path.Combine(directory,formal[i])):null),"2746 formal save byte-identical "+formal[i]);
   }finally{G.ClearShots();G.CloseInventory();G.Controls=controls;G.HarborWalkthrough=harbor;UnityEngine.Random.state=random;G.Paused=true;var report=new List<string>();for(int i=start;i<results.Count;i++)report.Add(results[i]);report.Add("INFO Scope: twin-office exterior only. Compact original plot, distinct three-storey coloured exteriors, clear bridge ground path, inherited approach via real Actor.Move, actual held-F main entrance and original indoor/outdoor cache, blocked left entrance, Camera.Render front/rear/details/live view, isolated inventory and byte-identical formal saves. No unrelated full-map or indoor room regression.");File.WriteAllLines(Path.Combine(folder,"2746-office-exterior-verification.txt"),report);}
  }
  void OfficeExteriorAppearance2746(RaidMap map){
   foreach(bool right in new[]{false,true}){
    var facade=map.Root.Find("OfficeFacade2746_"+(right?"Right":"Left"));Check(facade,"2746 authored finished tower facade root "+(right?"right":"left"));var colours=new HashSet<string>();int visible=0;bool warm=false,blue=false;var floors=new bool[3];int windows=0;
    foreach(var t in facade.GetComponentsInChildren<Transform>())if(t.name=="OfficeWindow2746"){windows++;int floor=Mathf.Clamp((int)(t.position.y/3.4f),0,2);floors[floor]=true;}
    Check(windows>=12&&Array.TrueForAll(floors,f=>f),"2746 actual window markers identify all three storeys "+(right?"right":"left")+" windows="+windows);
    foreach(var r in facade.GetComponentsInChildren<Renderer>())if(r.enabled){
     visible++;foreach(var material in r.sharedMaterials)if(material){var c=material.color;colours.Add(ColorUtility.ToHtmlStringRGB(c));warm|=c.r>c.b+.07f;blue|=c.b>c.r+.04f||c.g>c.r+.04f;}
    }
    Check(visible>0&&colours.Count>=4&&warm&&blue,"2746 real visible tower materials have varied warm and blue/green colours rather than grey whitebox "+(right?"right":"left")+" renderers="+visible+" colours="+colours.Count);
   }
   bool oldWhitebox=false;foreach(var t in map.Root.GetComponentsInChildren<Transform>())oldWhitebox|=t.name.Contains("办公")&&t.name.Contains("外壳白膜");Check(!oldWhitebox,"2746 no old office exterior whitebox node remains");
   Check(map.Root.Find("OfficeBridgeExterior2746"),"2746 second-floor exterior bridge has finished visible enclosure");
  }
 }
}
