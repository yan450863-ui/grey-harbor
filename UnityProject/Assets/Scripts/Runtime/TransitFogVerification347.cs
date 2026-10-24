using System;using System.IO;using System.Collections;using UnityEngine;
namespace GreyHarbor {public sealed partial class RaidVerification {
 IEnumerator RunTransitFog347(){
  Check(Application.version=="3.0.47"&&G.SavePath.Contains("verification"),"347 new build and isolated save");
  FreshCamp();G.enabled=false;Call314(G,"CaptureCarry");Call314(G,"CaptureDrops");string stock=JsonUtility.ToJson(G.Save);
  G.SelectDepartureMap317(RaidDestination.TransitCenter);yield return G.BeginTrainingLoad2721();G.enabled=false;
  Check(G.Map.TransitWhitebox346&&G.Map.Exits.Count==0&&G.Enemies.Count==0,"347 interior whitebox retains no exits or enemies");
  Check(!G.Vision.RoomGate&&G.Vision.MemoryBrightness==.33f,"347 sewer continuous flashlight and exploration-memory rules");
  int explored=0;foreach(bool seen in G.Vision.Seen)if(seen)explored++;
  Check(explored>0&&explored<G.Vision.Seen.Length/20,"347 entering no longer reveals whole floor");
  var spawn=G.Map.Spawn;G.Player.Aim=Vector3.forward;G.Vision.Clear();G.Vision.Refresh(spawn,G.Player.Aim);
  var front=spawn+Vector3.forward*4;var blocked=RaidMap.TransitPoint346(64,66);var remote=RaidMap.TransitPoint346(88,6);
  Check(G.Vision.CanSee(front)&&G.Vision.Explored(front),"347 flashlight illuminates nearby unobstructed floor");
  Check(!G.Vision.CanSee(blocked)&&!G.Vision.Explored(blocked),"347 wall blocks visibility and discovery of adjacent room");
  Check(!G.Vision.Explored(remote)&&!G.Vision.SurfaceSeen[G.Vision.Index(remote)],"347 unknown far rooms remain fully black");
  G.Player.Aim=Vector3.back;G.Vision.Refresh(spawn,G.Player.Aim);
  Check(!G.Vision.CanSee(front)&&G.Vision.Explored(front)&&!G.Vision.Current[G.Vision.Index(front)],"347 turning preserves dim memory without live sight");
  Check(Mathf.Approximately(Shader.GetGlobalFloat("_HarborMemoryBrightness"),.33f),"347 shader receives sewer memory brightness");
  G.Player.Aim=Vector3.forward;G.Vision.Refresh(spawn,G.Player.Aim);G.Rig.Tick(0);Call314(G,"LateUpdate");yield return null;yield return new WaitForEndOfFrame();
  var shot=ScreenCapture.CaptureScreenshotAsTexture();File.WriteAllBytes(Path.Combine(folder,"transit347-fog.png"),shot.EncodeToPNG());Destroy(shot);
  G.Player.transform.position=RaidMap.TransitPoint346(34.5f,42);G.Player.Aim=Vector3.right;G.Vision.Refresh(G.Player.transform.position,G.Player.Aim);G.Rig.Tick(0);Call314(G,"LateUpdate");yield return null;yield return new WaitForEndOfFrame();
  shot=ScreenCapture.CaptureScreenshotAsTexture();File.WriteAllBytes(Path.Combine(folder,"transit347-corridor.png"),shot.EncodeToPNG());Destroy(shot);
  var ui=G.GetComponent<RaidUI>();Check(ui.ToggleFullMap318(),"347 M overview remains usable for whitebox inspection");ui.CloseFullMap318();
  G.Finish(false);Check(G.Phase==GamePhase.Base&&JsonUtility.ToJson(G.Save)==stock,"347 leaving restores original equipment and supplies");
  G.SelectDepartureMap317(RaidDestination.Training);yield return G.BeginTrainingLoad2721();G.enabled=false;explored=0;foreach(bool seen in G.Vision.Seen)if(seen)explored++;
  Check(explored==G.Vision.Seen.Length&&G.Vision.MemoryBrightness==.52f,"347 normal outdoor training keeps original visibility");G.Finish(false);
  File.WriteAllLines(Path.Combine(folder,"transit347-fog-checks.txt"),results);Application.Quit(0);
 }
}}
