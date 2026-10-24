using System;
using System.IO;
using System.Collections;
using UnityEngine;
namespace GreyHarbor {
 public sealed partial class RaidVerification {
  IEnumerator RunFreightPerformance2719(){
   string dir=Path.GetDirectoryName(G.SavePath);string[] formal={"stash-v40.json","stash-v41.json","stash-v42.json"};byte[][] bytes=Array.ConvertAll(formal,n=>File.Exists(Path.Combine(dir,n))?File.ReadAllBytes(Path.Combine(dir,n)):null);
   FreshCamp();string inventory=JsonUtility.ToJson(G.Save);var controls=G.Controls;int sync=QualitySettings.vSyncCount,cap=Application.targetFrameRate;var target=new RenderTexture(1600,900,24,RenderTextureFormat.ARGB32);target.Create();var original=G.View.targetTexture;bool enabled=G.View.enabled;G.View.targetTexture=target;G.View.enabled=false;G.Controls=new TouchInput();
   performanceRows2717.Clear();performanceRows2717.Add("site,measuredFPS,meanFrameMs,p95FrameMs,p99FrameMs,p95VisionMs,mainThreadLastMs,drawCallsLast,managedMemoryDeltaBytes,activeAreaActors,containers,actualMovementMetres");
   try{QualitySettings.vSyncCount=0;Application.targetFrameRate=-1;G.HarborWalkthrough=true;Check(G.BeginTraining(),"2719 changed-region graphics performance preview");yield return null;
    Check(FreightPreview2716.Apply(G),"2719 actual freight performance location");yield return PerformanceSite2717("2719货运场室外",G.Player.transform.position);
    G.Paused=true;G.SwitchInterior("freightduty");foreach(var room in G.Map.FreightRooms2716){room.Door.Open=true;G.Vision.RevealArea(room.Area);}yield return PerformanceSite2717("2719值班房墙地板",G.Map.Spawn);G.Paused=true;G.SwitchInterior("freightduty");
    G.SwitchInterior("freightwarehouse");foreach(var room in G.Map.FreightRooms2716){room.Door.Open=true;G.Vision.RevealArea(room.Area);}yield return PerformanceSite2717("2719库房墙地板",G.Map.Spawn);G.Paused=true;G.SwitchInterior("freightwarehouse");
    G.Finish(false);yield return null;Check(G.Phase==GamePhase.Base&&JsonUtility.ToJson(G.Save)==inventory,"2719 performance restores isolated preview inventory");for(int i=0;i<formal.Length;i++)Check(MainBytes273(bytes[i],File.Exists(Path.Combine(dir,formal[i]))?File.ReadAllBytes(Path.Combine(dir,formal[i])):null),"2719 performance formal save unchanged "+formal[i]);
   }finally{G.View.targetTexture=original;G.View.enabled=enabled;target.Release();Destroy(target);G.Controls=controls;G.Paused=true;QualitySettings.vSyncCount=sync;Application.targetFrameRate=cap;File.WriteAllLines(Path.Combine(folder,"performance-regional-2719.csv"),performanceRows2717);File.WriteAllText(Path.Combine(folder,"performance-regional-2719-environment.txt"),"GPU: "+SystemInfo.graphicsDeviceName+"\nAPI: "+SystemInfo.graphicsDeviceType+"\nCPU: "+SystemInfo.processorType+"\nMethod: changed freight yard and five door headers only; 60 warmup + 300 sample frames per site; real movement with --performance-moving-2717; explicit Camera.Render into1600x900 RT; automatic camera disabled; uncapped vSync0. Hidden-window run, actual graphics submitted; short samples exclude scene generation and do not guarantee long foreground play.\n");}
  }
 }
}
