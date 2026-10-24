using System;using System.IO;using System.Collections;using UnityEngine;
namespace GreyHarbor {public sealed partial class RaidVerification {
 IEnumerator RunTransit346(){
  Check(Application.version=="3.0.46"&&G.SavePath.Contains("verification"),"346 new build uses isolated verification save");
  FreshCamp();G.enabled=false;Call314(G,"CaptureCarry");Call314(G,"CaptureDrops");string stock=JsonUtility.ToJson(G.Save);
  Check(G.SelectDepartureMap317(RaidDestination.TransitCenter)&&G.CanConfirmDeparture317,"346 whitebox selectable from departure menu");
  yield return G.BeginTrainingLoad2721();G.enabled=false;
  Check(G.Map.TransitWhitebox346&&G.TrainingActive&&G.Phase==GamePhase.Raid,"346 actual map loading enters protected whitebox session");
  Check(G.Map.Bounds==new Rect(-50,-40,100,80),"346 V2 100 by 80 metre footprint");
  Check(G.Enemies.Count==0&&G.Map.Loot.Count==0&&G.Map.Exits.Count==0&&G.Map.Doors.Count==0,"346 no enemies loot portals or extraction points");
  Check(G.Map.TransitRooms346.Count==30,"346 all thirty V2 room footprints retained");
  Physics.SyncTransforms();var spawn=G.Player.transform.position;
  Check(!Physics.CheckCapsule(spawn+Vector3.up*.35f,spawn+Vector3.up,.34f,SceneObstacle.MovementMask),"346 spawn clear of solid geometry");
  int index=0;foreach(var room in G.Map.TransitRooms346){
   Vector3 goal=Vector3.zero;bool found=false;
   for(float y=room.yMin+1;y<room.yMax-1&&!found;y+=1)for(float x=room.xMin+1;x<room.xMax-1&&!found;x+=1){var p=RaidMap.TransitPoint346(x,y);if(!Physics.CheckCapsule(p+Vector3.up*.35f,p+Vector3.up,.34f,SceneObstacle.MovementMask)){goal=p;found=true;}}
   var route=found?G.Navigation.FindPath(spawn,goal):null;
   Check(found&&route.Count>0,"346 actual capsule navigation reaches room "+(++index));
   if(route!=null&&route.Count>0){var start=spawn;bool clear=true;G.Player.transform.position=spawn;foreach(var end in route){clear&=G.Navigation.ClearSegment(start,end);start=end;for(int step=0;step<3000&&Vector3.Distance(G.Player.transform.position,end)>.025f;step++)G.Player.Move(Vector3.ClampMagnitude(end-G.Player.transform.position,.1f));}Check(clear,"346 wall and obstacle clearance along route "+index);Check(Vector3.Distance(G.Player.transform.position,goal)<.1f,"346 real actor walks to room "+index);}
   yield return null;
  }
  foreach(var p in new[]{new Vector3(13,0,11),spawn}){G.Player.transform.position=p;G.TickExtraction(10);Check(G.Phase==GamePhase.Raid&&G.ExtractTime==0,"346 no fallback extraction trigger");}
  var ui=G.GetComponent<RaidUI>();G.Player.transform.position=spawn;G.Rig.Tick(0);Call314(G,"LateUpdate");yield return null;yield return new WaitForEndOfFrame();
  var shot=ScreenCapture.CaptureScreenshotAsTexture();File.WriteAllBytes(Path.Combine(folder,"transit346-spawn.png"),shot.EncodeToPNG());Destroy(shot);
  Check(ui.ToggleFullMap318(),"346 full floor map opens with M action");yield return null;yield return new WaitForEndOfFrame();shot=ScreenCapture.CaptureScreenshotAsTexture();File.WriteAllBytes(Path.Combine(folder,"transit346-map.png"),shot.EncodeToPNG());Destroy(shot);ui.CloseFullMap318();
  G.Player.transform.position=RaidMap.TransitPoint346(34.5f,42);G.Rig.Tick(0);Call314(G,"LateUpdate");yield return null;yield return new WaitForEndOfFrame();shot=ScreenCapture.CaptureScreenshotAsTexture();File.WriteAllBytes(Path.Combine(folder,"transit346-corridor.png"),shot.EncodeToPNG());Destroy(shot);
  G.Finish(false);Check(G.Phase==GamePhase.Base&&!G.TrainingActive&&JsonUtility.ToJson(G.Save)==stock,"346 leaving restores exact pre-test equipment and supplies");
  G.SelectDepartureMap317(RaidDestination.Training);yield return G.BeginTrainingLoad2721();G.enabled=false;Check(!G.Map.TransitWhitebox346&&G.Map.ExteriorPreview,"346 existing training remains separate");G.Finish(false);
  File.WriteAllLines(Path.Combine(folder,"transit346-checks.txt"),results);Application.Quit(0);
 }
}}
