using System;using System.Collections;using System.IO;using UnityEngine;
namespace GreyHarbor {public sealed partial class RaidVerification {
 IEnumerator RunPlayerPresentation327(){
  var sprint=new SprintState2711();Check(sprint.Stamina==12&&SprintState2711.Maximum==12,"327 stamina starts and caps at12");Check(Mathf.Abs(SprintState2711.ResumeAt-3.2f)<.00001f,"327 auto-run resumes at3.2 preserving4/15 ratio");
  sprint.Toggle();sprint.Tick(12,true,Vector3.right,1);Check(sprint.Exhausted&&sprint.Stamina==0&&!sprint.CanRun&&sprint.Intent,"32712seconds sprint exhausts without losing run intention");Check(RaidUI.SprintStatus2711(sprint,false).Contains("1.5"),"327 recovery delay remains1.5seconds");sprint.Tick(1.5f,false,Vector3.zero,0);Check(sprint.Stamina==0,"327 no recovery within1.5second delay");sprint.Tick(3.19f/1.5f,false,Vector3.zero,0);Check(sprint.Exhausted&&!sprint.CanRun&&sprint.Stamina<3.2f,"327 no auto-run below3.2");Check(RaidUI.SprintStatus2711(sprint,false).Contains("3.2"),"327 auto-run hint uses new threshold");sprint.Tick(.01f,false,Vector3.zero,0);Check(!sprint.Exhausted&&sprint.CanRun&&sprint.Stamina>=3.2f,"327 auto-run unlocks at proportional threshold");sprint.Tick(20,false,Vector3.zero,0);Check(sprint.Stamina==12,"327 idle recovery cannot exceed12");
  sprint.Reset();sprint.Toggle();sprint.Tick(12,true,Vector3.right,1);sprint.Toggle();sprint.Tick(10,false,Vector3.zero,0);Check(!sprint.CanRun&&!sprint.Intent,"327 Shift-off disables automatic resuming");
  float hearing=SoundRules326.HearingRange327;Check(hearing>10.9f&&hearing<11.2f,"327 default hearing derived from centered screen vertical ground extent about11.1m");Check(Mathf.Abs(hearing*Mathf.Sin(RaidCamera.ViewTilt310*Mathf.Deg2Rad)-RaidCamera.GameplaySize)<.001f,"327 hearing scales to camera top-bottom half margin");Check(RaidUI.NearbyMapMeters318==18,"327 minimap18m independent of full map");
  for(int kind=0;kind<3;kind++){var icon=RaidUI.SoundIcon326(kind);int fractional=0,solid=0,clear=0;foreach(var p in icon.GetPixels32()){if(p.a==0)clear++;else if(p.a==255)solid++;else fractional++;}Check(icon.width==128&&icon.height==128&&icon.filterMode==FilterMode.Trilinear&&icon.mipmapCount==8,"327 high resolution mip-filtered icon "+kind);Check(fractional>10&&clear>100&&solid>100,"327 antialias coverage preserves crisp solid center and transparent boundary "+kind);File.WriteAllBytes(Path.Combine(folder,"327-sound-icon-"+kind+".png"),icon.EncodeToPNG());}
  for(int k=0;k<3;k++){
   var prior=RenderTexture.active;var rt=RenderTexture.GetTemporary(24,24,0,RenderTextureFormat.ARGB32);Graphics.Blit(RaidUI.SoundIcon326(k),rt);RenderTexture.active=rt;
   var actual=new Texture2D(24,24,TextureFormat.RGBA32,false);actual.ReadPixels(new Rect(0,0,24,24),0,0);actual.Apply();RenderTexture.active=prior;RenderTexture.ReleaseTemporary(rt);
   int mixed=0;foreach(var p in actual.GetPixels32())if(p.a>0&&p.a<255)mixed++;Check(mixed>10,"327 actual GPU icon minification retains antialiased boundary "+k);File.WriteAllBytes(Path.Combine(folder,"327-sound-icon-"+k+"-24px-gpu.png"),actual.EncodeToPNG());Destroy(actual);
  }
  yield return RunSound326();yield return RunMiniMapFix325();
  results.Add("327 COMPLETE:12 stamina,3.2 automatic resume,18m map,screen-relative hearing,actual resampled terrain and antialiased icons. Native whole-screen appearance requires visible playtest.");
 }
}}
