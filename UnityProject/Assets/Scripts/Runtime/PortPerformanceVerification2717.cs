using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using UnityEngine;
using Unity.Profiling;
namespace GreyHarbor {
 public sealed partial class RaidVerification {
  readonly List<string> performanceRows2717=new List<string>();
  IEnumerator PerformanceSite2717(string name,Vector3 position){
   G.Player.transform.position=position;G.Player.Health=100000;G.Paused=false;G.InventoryOpen=false;G.ExtractTime=0;Physics.SyncTransforms();G.Rig.Tick(0);var input=(TouchInput)G.Controls;
   input.Movement=Vector2.zero;for(int i=0;i<60;i++){input.AimPosition=new Vector2(Screen.width*.7f,Screen.height*.5f);yield return null;}
   var frames=new List<double>();var vision=new List<double>();double total=0,moved=0;bool moving=Array.IndexOf(Environment.GetCommandLineArgs(),"--performance-moving-2717")>=0;long previous=Stopwatch.GetTimestamp();long allocatedBefore=GC.GetTotalMemory(false);int framesCount=300;
   using(var main=ProfilerRecorder.StartNew(ProfilerCategory.Internal,"Main Thread",1))
   using(var draws=ProfilerRecorder.StartNew(ProfilerCategory.Render,"Draw Calls Count",1)){
    for(int i=0;i<framesCount;i++){input.Movement=moving?new Vector2(Mathf.Sin(i*.035f)*.7f,0):Vector2.zero;input.AimPosition=new Vector2(Screen.width*.5f+Mathf.Cos(i*.035f)*450,Screen.height*.5f+Mathf.Sin(i*.035f)*260);yield return null;G.View.Render();long now=Stopwatch.GetTimestamp();double ms=(now-previous)*1000.0/Stopwatch.Frequency;previous=now;total+=ms;frames.Add(ms);vision.Add(RaidVision.LastRefreshMilliseconds2717);moved+=G.LastActualMovement277.magnitude;}
    frames.Sort();vision.Sort();double fps=framesCount*1000/total,p95=frames[(int)(frames.Count*.95)],p99=frames[(int)(frames.Count*.99)],v95=vision[(int)(vision.Count*.95)];long memory=GC.GetTotalMemory(false)-allocatedBefore;
    string row=GalleryQuote2717(name)+","+fps.ToString("F2",CultureInfo.InvariantCulture)+","+(total/framesCount).ToString("F3",CultureInfo.InvariantCulture)+","+p95.ToString("F3",CultureInfo.InvariantCulture)+","+p99.ToString("F3",CultureInfo.InvariantCulture)+","+v95.ToString("F3",CultureInfo.InvariantCulture)+","+(main.Valid?main.LastValue/1000000.0:-1).ToString("F3",CultureInfo.InvariantCulture)+","+(draws.Valid?draws.LastValue:-1)+","+memory+","+G.Enemies.Count+","+G.Map.Loot.Count;
    row+=","+moved.ToString("F3",CultureInfo.InvariantCulture);performanceRows2717.Add(row);UnityEngine.Debug.Log("PERF2717 "+row);Check(G.Phase==GamePhase.Raid&&!G.Player.Dead,"2717 Performance sample completed with real AI/render/UI/fog "+name);
   }
   input.Movement=Vector2.zero;G.Paused=true;
  }
  IEnumerator RunPortPerformance2717(){
   var args=Environment.GetCommandLineArgs();bool baseline=Array.IndexOf(args,"--performance-baseline-2717")>=0;string suffix=baseline?"baseline":Array.IndexOf(args,"--performance-moving-2717")>=0?"moving":Array.IndexOf(args,"--performance-capped-2717")>=0?"capped":"final";string dir=Path.GetDirectoryName(G.SavePath);string[] formal={"stash-v40.json","stash-v41.json","stash-v42.json"};byte[][] before=Array.ConvertAll(formal,n=>File.Exists(Path.Combine(dir,n))?File.ReadAllBytes(Path.Combine(dir,n)):null);
   FreshCamp();var renderedTarget2717=new RenderTexture(1600,900,24,RenderTextureFormat.ARGB32);renderedTarget2717.Create();var originalTarget2717=G.View.targetTexture;bool originalCameraEnabled2717=G.View.enabled;G.View.targetTexture=renderedTarget2717;G.View.enabled=false;string inventory=JsonUtility.ToJson(G.Save);var controls=G.Controls;var random=UnityEngine.Random.state;int sync=QualitySettings.vSyncCount,cap=Application.targetFrameRate;G.Controls=new TouchInput();performanceRows2717.Clear();performanceRows2717.Add("site,measuredFPS,meanFrameMs,p95FrameMs,p99FrameMs,p95VisionMs,mainThreadLastMs,drawCallsLast,managedMemoryDeltaBytes,activeAreaActors,containers,actualMovementMetres");
   try{
    // Uncapped headroom shows whether 60 Hz can be sustained. Real 1600x900
    // renderer, HUD, flashlight, AI and current map remain enabled.
    QualitySettings.vSyncCount=0;Application.targetFrameRate=Array.IndexOf(args,"--performance-capped-2717")>=0?60:-1;G.HarborWalkthrough=true;Check(G.BeginTraining(),"2717 Real harbor performance training begins");yield return null;
    yield return PerformanceSite2717("南侧入口",G.Map.Spawn);
    yield return PerformanceSite2717("后勤旧院",RaidMap.PortPoint(28,42));
    yield return PerformanceSite2717("维修外围",RaidMap.RepairPreviewSpawn149);
    Check(CargoPreview2712.Apply(G),"2717 Central performance preview anchor");yield return PerformanceSite2717("中央集装箱迷宫",G.Player.transform.position);
    yield return PerformanceSite2717("北部码头",RaidMap.PortPoint(166,120));
    yield return PerformanceSite2717("客运码头",RaidMap.DockPreviewSpawn141);
    yield return PerformanceSite2717("南部码头",RaidMap.PortPoint(165,28));
    Check(FreightPreview2716.Apply(G),"2717 Freight performance preview anchor");yield return PerformanceSite2717("西北货运场",G.Player.transform.position);
    G.SwitchInterior("freightduty");yield return PerformanceSite2717("货运值班房",G.Map.Spawn);G.SwitchInterior("freightduty");
    G.SwitchInterior("freightwarehouse");yield return PerformanceSite2717("货运库房",G.Map.Spawn);G.SwitchInterior("freightwarehouse");
    G.SwitchInterior("repairmain1");yield return PerformanceSite2717("维修主楼一层",G.Map.Spawn);
    G.SwitchInterior("repair-floor:2:north");G.SwitchInterior("repair-floor:3:north");G.SwitchInterior("repair-floor:4:north");yield return PerformanceSite2717("维修主楼四层",G.Player.transform.position);
    G.Finish(false);yield return null;Check(G.Phase==GamePhase.Base&&JsonUtility.ToJson(G.Save)==inventory,"2717 Performance run restores temporary inventory");
    for(int i=0;i<formal.Length;i++)Check(MainBytes273(before[i],File.Exists(Path.Combine(dir,formal[i]))?File.ReadAllBytes(Path.Combine(dir,formal[i])):null),"2717 Performance formal save unchanged "+formal[i]);
   }finally{G.View.targetTexture=originalTarget2717;G.View.enabled=originalCameraEnabled2717;renderedTarget2717.Release();Destroy(renderedTarget2717);QualitySettings.vSyncCount=sync;Application.targetFrameRate=cap;G.Controls=controls;G.Paused=true;UnityEngine.Random.state=random;File.WriteAllLines(Path.Combine(folder,"performance-2717-"+suffix+".csv"),performanceRows2717);File.WriteAllText(Path.Combine(folder,"performance-2717-"+suffix+"-environment.txt"),"GPU: "+SystemInfo.graphicsDeviceName+"\nAPI: "+SystemInfo.graphicsDeviceType+"\nCPU: "+SystemInfo.processorType+"\nResolution: "+Screen.width+"x"+Screen.height+"\nOriginal vSync/targetFPS: "+sync+"/"+cap+"\nHeadroom samples: 60 warmup + 300 frames per site, active AI/HUD/fog; hidden desktop game window, explicit Camera.Render into 1600x900 RenderTexture every sampled frame, automatic camera disabled to avoid duplicate render. Timing excludes scene construction and explicit QA logging. Background timing does not replace foreground manual play. FPS shortfalls are reported as measured data, not hidden behind a passing correctness assertion.\n");}
  }
 }
}
