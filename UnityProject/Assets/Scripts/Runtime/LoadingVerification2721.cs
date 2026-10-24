using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace GreyHarbor {
 public sealed partial class RaidVerification {
  IEnumerator VerifyLoading2721(){
   Check(Application.version=="2.7.21"&&G.VerifyMode&&Path.GetFileName(G.SavePath)=="verification-stash-2.7.21.json","2721 loading verification uses isolated inventory");
   string directory=Path.GetDirectoryName(G.SavePath);string[] formal={"stash-v40.json","stash-v41.json","stash-v42.json"};byte[][] before=Array.ConvertAll(formal,n=>File.Exists(Path.Combine(directory,n))?File.ReadAllBytes(Path.Combine(directory,n)):null);
   var input=G.Controls;var rows=new List<string>{"attempt,stage,milliseconds"};double boot=G.CampBootMilliseconds2721;
   Check(G.Phase==GamePhase.Base&&!G.Map.HarborWhitebox&&!G.Map.IsHarbor&&G.Navigation==null&&G.Enemies.Count==0,"2721 initial boot creates only camp and basic resources");
   VerifyGraphicsPreferences2721();
   try{
    FreshCamp();string camp=JsonUtility.ToJson(G.Save);G.Controls=new TouchInput();
    for(int attempt=0;attempt<2;attempt++){
     G.HarborWalkthrough=true;var pending=G.BeginTrainingLoad2721();Check(pending.MoveNext()&&G.TrainingActive,"2721 asynchronous harbor request takes isolated training snapshot");
     var coroutine=pending.Current as IEnumerator;Check(coroutine!=null&&coroutine.MoveNext()&&G.IsLoading2721,"2721 loading becomes gated before constructing map");
     Check(!G.RequestRaidLoad2721()&&!G.BeginTraining(),"2721 repeated requests cannot create duplicate maps during loading");
     var ui=G.GetComponent<RaidUI>();ui.enabled=true;yield return null;yield return new WaitForEndOfFrame();
     do{yield return coroutine.Current;}while(coroutine.MoveNext());while(pending.MoveNext())yield return pending.Current;
     G.Paused=true;yield return null;
     Check(!G.IsLoading2721&&G.Phase==GamePhase.Raid&&G.TrainingActive&&G.Map.HarborWhitebox&&G.Map.GroundCacheUsed2721,"2721 asynchronous actual harbor finishes using prebuilt ground cache");
     Check(G.Player&&!G.Player.Dead&&G.Enemies.Count==26&&G.Map.Loot.Count==54&&G.Map.FreightEnemies2720.Count==3&&G.Map.CargoEnemies2716.Count==7,"2721 staged world preserves actual player,26 exterior actors,54 containers and fixed rosters");
     Check(G.Navigation.CheckedCells2721<G.Navigation.TotalCells2721&&G.Navigation.ClearSegment(G.Map.Spawn,G.Map.Spawn+Vector3.forward*.5f),"2721 spawn is physically ready without initial full navigation scan");
     Check(G.LoadingTimings2721.Count>=14&&G.LastMapLoadMilliseconds2721>0,"2721 actual stage and complete timing available");
     if(attempt==0){
      var sample=new List<string>{"frame,elapsed_ms,frame_gap_ms,path_requests,new_cells"};
      double begin=Time.realtimeSinceStartupAsDouble,last=begin,maxGap=0;int requests=G.Navigation.PathRequests2717,cells=G.Navigation.CheckedCells2721,startRequests=requests,index=0;
      int sync=QualitySettings.vSyncCount,limit=Application.targetFrameRate;
      try{
       // Hidden windows skip presentation and cannot pace with vSync. A real 60-Hz
       // update cadence prevents thousands of artificial warmup frames before patrol.
       QualitySettings.vSyncCount=0;Application.targetFrameRate=60;G.Paused=false;
       while(Time.realtimeSinceStartupAsDouble-begin<6){
        yield return null;double now=Time.realtimeSinceStartupAsDouble,gap=(now-last)*1000;maxGap=Math.Max(maxGap,gap);
        int nextRequests=G.Navigation.PathRequests2717,nextCells=G.Navigation.CheckedCells2721;
        sample.Add((index++)+","+((now-begin)*1000).ToString("F3",System.Globalization.CultureInfo.InvariantCulture)+","+gap.ToString("F3",System.Globalization.CultureInfo.InvariantCulture)+","+(nextRequests-requests)+","+(nextCells-cells));
        last=now;requests=nextRequests;cells=nextCells;
       }
      }finally{QualitySettings.vSyncCount=sync;Application.targetFrameRate=limit;}
      G.Paused=true;File.WriteAllLines(Path.Combine(folder,"2721-cold-navigation-frames.csv"),sample);
      Check(requests>startRequests&&G.Player&&!G.Player.Dead,"2721 six-second real update sample crosses first cold patrol requests without blocking game lifecycle");
      rows.Add("RUN,hidden-player-max-frame-gap,"+maxGap.ToString("F3",System.Globalization.CultureInfo.InvariantCulture));
     }
     foreach(var stage in G.LoadingTimings2721)rows.Add(attempt+","+stage);rows.Add(attempt+",TOTAL,"+G.LastMapLoadMilliseconds2721.ToString("F3",System.Globalization.CultureInfo.InvariantCulture));
     var texture=G.Map.StableGround137;var hash=HarborGroundCache2721.HashBytes(texture.GetRawTextureData());var expected=Resources.Load<HarborGroundCache2721>(HarborGroundCache2721.ResourceName);Check(hash==expected.PixelHash,"2721 runtime ground raw pixels and all mip levels match offline export exactly");
     var plan=G.Map.HarborPlanTexture();Check(G.Map.HarborPlanCacheUsed2721&&HarborGroundCache2721.HashBytes(plan.GetRawTextureData())==expected.PlanPixelHash,"2721 actual harbor minimap uses byte-identical prebuilt collision overlay");
     Check(FreightPreview2716.Apply(G),"2721 freight preview remains reachable after phased loading");
     var outside=G.Map;var outsideNav=G.Navigation;var actor=outside.FreightEnemies2720[0].Actor;var position=actor.transform.position;
     G.SwitchInterior("freightduty");yield return null;Check(G.Map.FreightInterior2716&&G.Enemies.Count==1,"2721 independent freight interior loads only on entry");
     int sampled=outsideNav.CheckedCells2721;outsideNav.WarmAround2721(position,1);Check(outsideNav.CheckedCells2721==sampled,"2721 unloaded outer collision cannot contaminate background navigation");
     G.SwitchInterior("freightduty");yield return null;Check(G.Map==outside&&G.Navigation==outsideNav&&actor.transform.position==position&&G.Enemies.Contains(actor)&&texture&&outside.GroundCacheUsed2721,"2721 returning area preserves actors and shared ground cache");
     G.Finish(false);yield return null;Check(G.Phase==GamePhase.Base&&!G.TrainingActive&&!G.Map.HarborWhitebox&&JsonUtility.ToJson(G.Save)==camp&&texture&&plan,"2721 finishing training restores exact inventory while shared cached ground and minimap remain reusable");
    }
    FreshCamp();var carryBag=G.Bag;var carryChest=G.Chest;var carryGun=G.Equipped;
    bool verify=G.VerifyMode;
    try{
     G.VerifyMode=false;G.VerificationSmallMap=false;
     Check(G.RequestRaidLoad2721()&&G.IsLoading2721,"2721 ordinary harbor departure enters asynchronous loading using isolated save");
     var prepared=JsonUtility.FromJson<BaseSave>(File.ReadAllText(G.SavePath));
     Check(prepared.InRaid&&prepared.CampBag.Count==0&&prepared.CampRig.Count==0&&(prepared.Weapon==null||prepared.Weapon.Count==0),"2721 departure commits carry transfer before asynchronous construction");
     Check(!G.RequestRaidLoad2721(),"2721 ordinary departure blocks duplicate requests");
     while(G.IsLoading2721)yield return null;G.Paused=true;
     Check(G.Phase==GamePhase.Raid&&!G.TrainingActive&&G.Map.HarborWhitebox&&G.Map.GroundCacheUsed2721&&G.Map.HarborPlanCacheUsed2721&&G.Bag==carryBag&&G.Chest==carryChest&&G.Equipped==carryGun&&G.Magazine>0,"2721 ordinary asynchronous departure retains equipped gun, ammunition and carried inventories");
     foreach(var stage in G.LoadingTimings2721)rows.Add("ordinary,"+stage);rows.Add("ordinary,TOTAL,"+G.LastMapLoadMilliseconds2721.ToString("F3",System.Globalization.CultureInfo.InvariantCulture));
     G.Finish(true);Check(G.Phase==GamePhase.Base&&!G.Save.InRaid,"2721 ordinary departure returns and saves settlement normally");
    }finally{G.VerifyMode=verify;}
    for(int i=0;i<formal.Length;i++)Check(MainBytes273(before[i],File.Exists(Path.Combine(directory,formal[i]))?File.ReadAllBytes(Path.Combine(directory,formal[i])):null),"2721 formal-save bytes unchanged "+formal[i]);
    G.Paused=true;G.GetComponent<RaidUI>().OpenInventorySettings();yield return null;yield return new WaitForEndOfFrame();yield return null;
   }finally{G.Controls=input;G.Paused=true;rows.Add("BOOT,camp-code,"+boot.ToString("F3",System.Globalization.CultureInfo.InvariantCulture));File.WriteAllLines(Path.Combine(folder,"2721-loading-times.csv"),rows);}
  }
 }
}
