using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using UnityEngine;
namespace GreyHarbor {
 public sealed partial class RaidVerification {
  string ViewN278(float n)=>n.ToString("0.00000",CultureInfo.InvariantCulture);
  void ViewSize278(string stage,List<string> rows){
   Check(G.View.orthographic&&Mathf.Abs(G.View.orthographicSize-10)<.0001f,"278 Gameplay camera retains size10 "+stage);
   Check(Mathf.Abs(RaidVision.Range-16)<.0001f&&Mathf.Abs(RaidVision.HalfAngle-30)<.0001f,"278 Camera zoom preserves16m/60degree flashlight "+stage);
   rows.Add(stage+","+ViewN278(G.View.orthographicSize)+","+ViewN278(G.View.aspect)+",16,60");
  }
  Vector3 ViewGround278(float x){
   var plane=new Plane(Vector3.up,G.Player.transform.position+Vector3.up*RaidGame.CombatHeight);var ray=G.View.ViewportPointToRay(new Vector3(x,.5f,0));
   Check(plane.Raycast(ray,out float enter),"278 Actual gameplay viewport ray intersects combat plane");return ray.GetPoint(enter);
  }
  void ViewEdges278(List<string> rows){
   G.Rig.ResetView(45);G.Rig.Tick(0);
   for(int i=0;i<4;i++){
    if(i>0){G.Rig.Turn(1);G.Rig.Tick(.31f);}Check(!G.Rig.Turning,"278 Real Q/E quarter-turn completes "+i);
    var eye=G.Player.transform.position+Vector3.up*RaidGame.CombatHeight;
    var left=G.View.WorldToViewportPoint(eye-G.Rig.Right*16);var right=G.View.WorldToViewportPoint(eye+G.Rig.Right*16);
    Check(left.z>0&&right.z>0&&left.x>=0&&left.x<=1&&right.x>=0&&right.x<=1,"278 Both lateral16m flashlight endpoints fit actual viewport at Q/E angle "+G.Rig.Yaw);
    float wide=Vector3.Distance(ViewGround278(0),ViewGround278(1));float actual=G.View.orthographicSize;
    G.View.orthographicSize=8.5f;float old=Vector3.Distance(ViewGround278(0),ViewGround278(1));G.View.orthographicSize=actual;
    Check(Mathf.Abs(wide/old-10f/8.5f)<.0001f,"278 Actual ground-plane visible width increases exactly10/8.5 at angle "+G.Rig.Yaw);
    rows.Add(ViewN278(G.Rig.Yaw)+","+ViewN278(left.x)+","+ViewN278(right.x)+","+ViewN278(wide)+","+ViewN278(old)+","+ViewN278(wide/old));
   }
   G.Rig.Turn(-1);G.Rig.Tick(.31f);Check(!G.Rig.Turning&&Mathf.Abs(Mathf.DeltaAngle(G.Rig.Yaw,225))<.001f,"278 Reverse E turn also settles at expected zoomed angle");
   G.ResetAkHandling276();var target=G.Player.transform.position+Vector3.up*RaidGame.CombatHeight+G.Rig.Right*6+G.Rig.Forward*2;
   var screen=G.View.WorldToScreenPoint(target);G.Controls=new TouchInput{AimPosition=new Vector2(screen.x,screen.y)};G.UpdateAim();
   var aim=target-G.Player.transform.position;aim.y=0;
   Check(Vector3.Dot(aim.normalized,G.Player.Aim)>.99999f,"278 True UpdateAim remains aligned with mouse target after zoom");
   Check(Vector2.Distance(G.AkCrosshairScreen276,new Vector2(screen.x,screen.y))<.05f,"278 Actual HUD crosshair projection matches player aim target after zoom");
   G.Vision.Refresh(G.Player.transform.position,G.Player.Aim);
   Check(Mathf.Abs(Shader.GetGlobalFloat("_HarborVisionRange")-16)<.001f&&Mathf.Abs(Shader.GetGlobalFloat("_HarborConeCos")-Mathf.Cos(30*Mathf.Deg2Rad))<.00001f,"278 Actual flashlight shader keeps16m/60degree configuration");
  }
  IEnumerator ViewPicture278(){
   MainArrivalDoor273();var groups=G.Map.MainDecorationGroups274;var group=groups.Find(p=>p.Center&&p.RoomName=="维修大厅");if(group==null)group=groups.Find(p=>p.Center);
   Check(group!=null,"278 Zoomed interior photograph has a central daily-object group");var r=group.Footprint;bool reached=false;float yaw=0;
   var stands=new[]{new Vector3(r.center.x,0,r.yMin-1.2f),new Vector3(r.center.x,0,r.yMax+1.2f),new Vector3(r.xMin-1.2f,0,r.center.y),new Vector3(r.xMax+1.2f,0,r.center.y)};
   for(int i=0;i<stands.Length;i++){
    var stand=stands[i];if(!Clear140(stand)||!G.Navigation.ClearSegment(stand,group.Anchor))continue;
    if(G.Navigation.FindPath(G.Player.transform.position,stand).Count==0)continue;MainWalk273(stand,"278 Central object front-facing photograph stand");yaw=i==0?0:i==1?180:i==2?90:270;reached=true;break;
   }
   // Only reachability is required: the prop's real collision may block its own centre ray.
   if(!reached)for(int i=0;i<stands.Length;i++){if(!Clear140(stands[i])||G.Navigation.FindPath(G.Player.transform.position,stands[i]).Count==0)continue;MainWalk273(stands[i],"278 Central object reachable photograph stand");yaw=i==0?0:i==1?180:i==2?90:270;reached=true;break;}
   Check(reached,"278 Player reaches central interior photograph position");G.ResetAkHandling276();yield return RoomsGameView276("278-zoomed-interior-live-fog.png",group.Anchor,yaw);
   Check(Mathf.Abs(G.View.orthographicSize-10)<.0001f,"278 True interior Camera.Render uses gameplay size10");
  }
  public IEnumerator VerifyGameplayView278(){
   var previous=G.Controls;string directory=Path.GetDirectoryName(G.SavePath);string[] formal={Path.Combine(directory,"stash-v40.json"),Path.Combine(directory,"stash-v41.json")};byte[][] saved=Array.ConvertAll(formal,p=>File.Exists(p)?File.ReadAllBytes(p):null);
   var stages=new List<string>{"stage,actual_size,aspect,flashlight_range_m,full_angle_deg"};var edges=new List<string>{"yaw,left_viewport_x,right_viewport_x,visible_width_m,old8_5_width_m,width_ratio"};
   try {
    Check(Path.GetFileName(G.SavePath)=="verification-stash-2.7.8.json","278 View verification uses isolated inventory");
    FreshCamp();G.HarborWalkthrough=false;G.HardMode=false;Check(Mathf.Abs(G.View.orthographicSize-9.3f)<.0001f,"278 Camp camera retains its separate9.3 size");
    Check(G.StartRaid()&&!G.TrainingActive,"278 Formal Depart enters actual gameplay camera");G.Paused=true;yield return null;ViewSize278("formal-depart",stages);G.Finish(false);yield return null;
    FreshCamp();string trainingSnapshot=JsonUtility.ToJson(G.Save);G.HarborWalkthrough=false;Check(G.BeginTraining(),"278 Ordinary training enters actual gameplay camera");G.Paused=true;yield return null;ViewSize278("ordinary-training",stages);G.Finish(false);yield return null;
    Check(JsonUtility.ToJson(G.Save)==trainingSnapshot,"278 Ordinary training restores exact isolated inventory");
    FreshCamp();trainingSnapshot=JsonUtility.ToJson(G.Save);G.HarborWalkthrough=true;Check(G.BeginTraining(),"278 Harbor training enters actual gameplay camera");G.Paused=true;G.Controls=new TouchInput();yield return null;var outside=G.Map;ViewSize278("harbor",stages);ViewEdges278(edges);
    G.SwitchInterior("repairmain1");G.Paused=true;yield return null;Check(G.Map.RepairMain220&&G.Map.RepairFloor220==1,"278 Actual main floor1 entered");ViewSize278("main-floor1",stages);
    G.SwitchInterior("repair-floor:2:north");G.Paused=true;yield return null;Check(G.Map.RepairFloor220==2,"278 Actual main floor2 entered");ViewSize278("main-floor2",stages);
    G.SwitchInterior("repair-floor:1:north");G.Paused=true;yield return null;Check(G.Map.RepairFloor220==1,"278 Cached main floor1 reentered");ViewSize278("main-floor1-return",stages);yield return ViewPicture278();
    G.SwitchInterior(G.Map.InteriorId);G.Paused=true;yield return null;Check(G.Map==outside,"278 Main exit restores original outside map");ViewSize278("main-outside",stages);
    G.SwitchInterior("repairannex");G.Paused=true;yield return null;Check(G.Map.InteriorId=="repairannex","278 Actual annex entered");ViewSize278("annex-entry",stages);
    G.SwitchInterior("repairannex");G.Paused=true;yield return null;Check(G.Map==outside,"278 Same annex portal exits to original outside map");ViewSize278("annex-outside",stages);G.Finish(false);yield return null;
    Check(JsonUtility.ToJson(G.Save)==trainingSnapshot,"278 Harbor camera preview restores exact isolated inventory");
    for(int i=0;i<formal.Length;i++)Check(MainBytes273(saved[i],File.Exists(formal[i])?File.ReadAllBytes(formal[i]):null),"278 Camera preview leaves formal inventory unchanged "+Path.GetFileName(formal[i]));
   }finally{G.Controls=previous;G.Paused=true;File.WriteAllLines(Path.Combine(folder,"278-gameplay-view-transitions.csv"),stages);File.WriteAllLines(Path.Combine(folder,"278-gameplay-view-edges.csv"),edges);}
  }
 }
}
