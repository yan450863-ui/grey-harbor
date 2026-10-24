using System;
using System.IO;
using System.Collections;
using UnityEngine;
namespace GreyHarbor {
 public sealed partial class RaidVerification {
  IEnumerator RunNewFreightModels2719(){
   Check(Application.version=="2.7.19"&&Path.GetFileName(G.SavePath)=="verification-stash-2.7.19.json","2719 gallery test uses isolated current-version save");
   string dir=Path.GetDirectoryName(G.SavePath);string[] formal={"stash-v40.json","stash-v41.json","stash-v42.json"};byte[][] bytes=Array.ConvertAll(formal,n=>File.Exists(Path.Combine(dir,n))?File.ReadAllBytes(Path.Combine(dir,n)):null);
   var controls=G.Controls;var random=UnityEngine.Random.state;FreshCamp();string inventory=JsonUtility.ToJson(G.Save);G.Controls=new TouchInput();G.AssetShowcaseMode2717=true;G.HarborWalkthrough=true;
   try{
    Check(G.BeginTraining(),"2719 independent asset map begins");G.Paused=true;yield return null;yield return RunFreightOperationGallery2719();
    G.Player.transform.position=G.Map.Extraction;G.ExtractTime=0;Physics.SyncTransforms();G.TickExtraction(5.1f);yield return null;
    Check(G.Phase==GamePhase.Base&&JsonUtility.ToJson(G.Save)==inventory,"2719 independent model testing restores inventory");
    G.AssetShowcaseMode2717=false;G.HarborWalkthrough=true;Check(G.BeginTraining(),"2719 actual yard model-height followup begins");G.Paused=true;yield return null;Check(FreightPreview2716.Apply(G),"2719 actual yard safe model-height followup anchor");
    FreightOperationsCheck2719();yield return FreightRoutesCheck2716();yield return FreightSearchCheck2716();
    yield return CargoPicture2715("2719-freight-yard-northeast.png",RaidMap.PortPoint(25,154),45,23,false,1800,1200);
    yield return CargoPicture2715("2719-freight-working-bays.png",RaidMap.PortPoint(35.5f,170.6f),135,8,false,1800,1000);
    yield return FreightLiveImage2718("2719-outdoor-live",RaidMap.FreightPreviewSpawn2716,new Vector3(1,0,1),45);
    G.Finish(false);yield return null;Check(G.Phase==GamePhase.Base&&JsonUtility.ToJson(G.Save)==inventory,"2719 yard followup restores inventory");
    for(int i=0;i<formal.Length;i++)Check(MainBytes273(bytes[i],File.Exists(Path.Combine(dir,formal[i]))?File.ReadAllBytes(Path.Combine(dir,formal[i])):null),"2719 independent gallery and height followup preserve formal save "+formal[i]);
   }finally{G.Controls=controls;UnityEngine.Random.state=random;G.Paused=true;File.WriteAllLines(Path.Combine(folder,"2719-final-working-bays.csv"),freightRows2716);}
  }
 }
}
