using System;using System.IO;using System.Collections;using UnityEngine;
namespace GreyHarbor {public sealed partial class RaidVerification {
 IEnumerator WaitResolution342(int w,int h,FullScreenMode mode){
  for(int i=0;i<180&&(Screen.width!=w||Screen.height!=h||Screen.fullScreenMode!=mode);i++)yield return null;
  yield return new WaitForSeconds(.25f);
 }
 IEnumerator ShotResolution342(string name){yield return new WaitForEndOfFrame();var t=ScreenCapture.CaptureScreenshotAsTexture();File.WriteAllBytes(Path.Combine(folder,name+".png"),t.EncodeToPNG());Destroy(t);}
 IEnumerator RunClarity342(){
  G.enabled=false;var native=Display.main;
  results.Add("Native monitor: "+native.systemWidth+"x"+native.systemHeight);Debug.Log(results[results.Count-1]);
  results.Add("Initial render: "+Screen.width+"x"+Screen.height+" "+Screen.fullScreenMode);Debug.Log(results[results.Count-1]);
  results.Add("Camera target: "+(G.View.targetTexture?G.View.targetTexture.name:"direct screen")+"; dynamic resolution="+G.View.allowDynamicResolution+"; texture limit="+QualitySettings.globalTextureMipmapLimit+"; MSAA="+QualitySettings.antiAliasing);
  Screen.SetResolution(1600,900,FullScreenMode.FullScreenWindow);yield return WaitResolution342(1600,900,FullScreenMode.FullScreenWindow);
  Check(Screen.width==1600&&Screen.height==900&&Screen.fullScreenMode==FullScreenMode.FullScreenWindow,"low-resolution borderless fullscreen reproduced");
  yield return ShotResolution342("clarity-low-1600x900");
  // The former implementation returned here solely because the mode was already borderless.
  results.Add("Former same-mode early return would preserve "+Screen.width+"x"+Screen.height);
  G.ApplyDisplay311(DisplayMode311.Borderless);yield return WaitResolution342(native.systemWidth,native.systemHeight,FullScreenMode.FullScreenWindow);
  Check(Screen.width==native.systemWidth&&Screen.height==native.systemHeight,"selecting already-active fullscreen repairs low rendering resolution");
  yield return ShotResolution342("clarity-native");
  G.ApplyDisplay311(DisplayMode311.Borderless);yield return null;
  Check(Screen.width==native.systemWidth&&Screen.height==native.systemHeight,"reapplying fullscreen retains native resolution");
  G.ApplyDisplay311(DisplayMode311.Windowed);yield return WaitResolution342(Mathf.Min(1600,native.systemWidth),Mathf.Min(900,native.systemHeight),FullScreenMode.Windowed);
  Check(Screen.fullScreenMode==FullScreenMode.Windowed&&Screen.width<=1600&&Screen.height<=900,"windowed mode remains bounded");
  G.ApplyDisplay311(DisplayMode311.Borderless);yield return WaitResolution342(native.systemWidth,native.systemHeight,FullScreenMode.FullScreenWindow);
  Check(Screen.width==native.systemWidth&&Screen.height==native.systemHeight&&Screen.fullScreenMode==FullScreenMode.FullScreenWindow,"window-to-fullscreen roundtrip restores native render resolution");
  File.WriteAllLines(Path.Combine(folder,"clarity-342.txt"),results);Application.Quit(0);
 }
}}