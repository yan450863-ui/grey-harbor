using System;using System.IO;using System.Linq;using System.Collections;using UnityEngine;
namespace GreyHarbor {public sealed partial class RaidVerification {
 IEnumerator PhotoMap333(string label){
  var ui=G.GetComponent<RaidUI>();G.enabled=false;G.InventoryOpen=G.Paused=G.MenuCamp315=false;G.NoticeTime=0;G.Rig.Tick(0);G.Player.Draw(G.View,0);ui.enabled=true;
  int repaint=ui.HudRepaints332;float start=Time.realtimeSinceStartup;
  while(ui.HudRepaints332<repaint+3&&Time.realtimeSinceStartup-start<10)yield return null;
  Check(ui.HudRepaints332>=repaint+3,"333 normal visible window repaints "+label);
  var prepared=ui.PreparedTerrain331 as Texture2D;Check(prepared&&prepared.GetPixels32().Count(c=>c.r>50||c.g>60||c.b>65)>prepared.width*prepared.height*.05f,"333 production LateUpdate contains real terrain "+label);
  yield return new WaitForEndOfFrame();var shot=ScreenCapture.CaptureScreenshotAsTexture();Check(shot!=null,"333 native final framebuffer captured "+label);
  if(shot){File.WriteAllBytes(Path.Combine(folder,"native-333-"+label+".png"),shot.EncodeToPNG());Destroy(shot);}
  Check(ui.TerrainAtlasBuilds330==0&&ui.TerrainGpuDraws330==0,"333 normal game map never relies on immediate GPU map rendering "+label);
 }
 IEnumerator RunMapNative333(){
  Application.runInBackground=true;Application.targetFrameRate=30;Check(G.VerifyMode&&G.SavePath.Contains("verification"),"333 isolated save enabled");
  HudCamp329();G.InventoryOpen=false;G.HarborWalkthrough=G.ModRange315=false;Check(G.BeginTraining(),"333 training starts");G.ReloadTime=1.5f;
  yield return PhotoMap333("training");var ui=G.GetComponent<RaidUI>();int builds=ui.PictureBuilds333,uploads=ui.PictureUploads332;
  yield return PhotoMap333("training-stationary");Check(builds==ui.PictureBuilds333&&uploads==ui.PictureUploads332,"333 stationary frames reuse map and crop");
  G.ReloadTime=0;G.Player.transform.position+=new Vector3(3,0,3);G.Rig.Turn(1);G.Rig.Tick(.3f);yield return PhotoMap333("training-moved-turned");Check(builds==ui.PictureBuilds333&&ui.PictureUploads332>uploads,"333 movement and Q-E camera update crop without rebuilding map");
  Check(ui.ToggleFullMap318(),"333 M opens full map");yield return null;Check(ui.ToggleFullMap318(),"333 M closes full map");yield return PhotoMap333("training-after-M");CheckPicture332(ui,G.Map,G.Player.transform.position,"training");CheckPicture332(ui,G.Map,new Vector3(G.Map.Bounds.xMin,0,G.Map.Bounds.yMin),"map-edge");
  G.Finish(false);yield return null;HudCamp329();G.InventoryOpen=false;G.HarborWalkthrough=true;Check(G.BeginTraining()&&G.Map.HarborWhitebox,"333 harbor starts");yield return PhotoMap333("harbor");CheckPicture332(ui,G.Map,RaidMap.PortPoint(100,90),"harbor");
  var door=G.Map.Doors.Find(d=>d.Portal&&d.Destination=="portbooth");Check(door!=null,"333 harbor room portal exists");G.Player.transform.position=door.Position;G.SwitchInterior("portbooth");yield return PhotoMap333("interior");CheckPicture332(ui,G.Map,G.Player.transform.position,"interior");
  builds=ui.PictureBuilds333;G.Map.LayoutRevision++;yield return PhotoMap333("interior-revision");Check(ui.PictureBuilds333==builds+1,"333 layout revision rebuilds image exactly once");
  Check(ui.ToggleFullMap318(),"333 full exterior map opens while indoors");yield return null;ui.CloseFullMap318();yield return PhotoMap333("interior-after-M");G.SwitchInterior("portbooth");yield return PhotoMap333("harbor-return");
  G.Finish(false);yield return new WaitForEndOfFrame();Check(ui.PreparedTerrain331==null,"333 returning to camp clears minimap");BubbleChecks331();
  results.Add("333 complete: visible normal-window final framebuffer screenshots; production image cache, rotation, M, harbor, interior, revision, movement cost verified.");
  File.WriteAllLines(Path.Combine(folder,"3.0.33-visible-map-"+SystemInfo.graphicsDeviceType+".txt"),results);Application.Quit(results.Any(x=>x.StartsWith("FAIL"))?1:0);
 }
}}
