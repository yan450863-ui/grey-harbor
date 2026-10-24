using System;using System.IO;using System.Collections;using UnityEngine;
namespace GreyHarbor {public sealed partial class RaidVerification {
 IEnumerator RunHudNative332(){
  Application.runInBackground=true;Application.targetFrameRate=30;
  HudCamp329();G.InventoryOpen=false;G.HarborWalkthrough=false;G.ModRange315=false;Check(G.BeginTraining(),"332 native training starts");G.enabled=false;G.InventoryOpen=G.Paused=false;G.MenuCamp315=false;var ui=G.GetComponent<RaidUI>();ui.enabled=true;G.Rig.Tick(0);G.Player.Draw(G.View,0);G.NoticeTime=0;G.ReloadTime=1.5f;
  float start=Time.realtimeSinceStartup;while(ui.HudRepaints332<5&&Time.realtimeSinceStartup-start<12)yield return null;
  results.Add("NATIVE HUD repaints="+ui.HudRepaints332+" renderer="+SystemInfo.graphicsDeviceType+" prepared="+(ui.PreparedTerrain331?ui.PreparedTerrain331.GetType().Name:"null"));
  if(ui.PreparedTerrain331 is Texture2D){File.WriteAllBytes(Path.Combine(folder,"native-prepared.png"),((Texture2D)ui.PreparedTerrain331).EncodeToPNG());var gpu=ReadGpu330(ui.PreparedTerrain331,"native-prepared-gpu.png");Destroy(gpu);}
  if(ui.HudRepaints332>=5){bool captured=false;StartCoroutine(CaptureNative332(()=>captured=true));start=Time.realtimeSinceStartup;while(!captured&&Time.realtimeSinceStartup-start<8)yield return null;results.Add("NATIVE screenshot completed="+captured);}
  File.WriteAllLines(Path.Combine(folder,"hud332-native.txt"),results);Application.Quit(0);
 }
 IEnumerator CaptureNative332(Action done){yield return new WaitForEndOfFrame();var image=ScreenCapture.CaptureScreenshotAsTexture();if(image){File.WriteAllBytes(Path.Combine(folder,"hud332-native.png"),image.EncodeToPNG());Destroy(image);}done();}
}}
