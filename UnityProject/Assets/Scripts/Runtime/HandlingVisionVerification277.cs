using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using UnityEngine;
using UnityEngine.Rendering;
namespace GreyHarbor {
 public sealed partial class RaidVerification {
  readonly List<string> hvShots277=new List<string>(),hvVision277=new List<string>();
  static string HvN277(float n){return n.ToString("0.00000",CultureInfo.InvariantCulture);}
  static bool HvNear277(float a,float b,float epsilon=.015f){return Mathf.Abs(a-b)<=epsilon;}
  Vector2 HvProject277(AkShotSample276 s,Vector3 p){var q=s.ViewProjection*new Vector4(p.x,p.y,p.z,1);return new Vector2(s.CameraPixelRect.x+(q.x/q.w+1)*.5f*s.CameraPixelRect.width,s.CameraPixelRect.y+(q.y/q.w+1)*.5f*s.CameraPixelRect.height);}
  void HvCursor277(TouchInput input,Vector2 offset,bool counter=false){
   var eye=G.Player.transform.position+Vector3.up*RaidGame.CombatHeight;var center=G.View.WorldToScreenPoint(eye);
   input.AimPosition=new Vector2(center.x,center.y)+offset;
   if(counter){
    // Invert the measured vertical projection analytically for a horizontal
    // baseline. Recoil still has its independent random angular spread.
    float tan=Mathf.Tan(G.AkHandling276.Recoil*Mathf.Deg2Rad),sin=Mathf.Sin(50*Mathf.Deg2Rad);
    input.AimPosition.y-=Mathf.Abs(offset.x)*sin*tan/Mathf.Sqrt(Mathf.Max(.01f,1-tan*tan));
   }
  }
  void HvRange277(){
   foreach(var e in G.Enemies){e.Dead=true;e.HitCollider.enabled=false;}
   foreach(var c in G.Map.Root.GetComponentsInChildren<Collider>())c.enabled=false;G.Player.HitCollider.enabled=true;
   var root=new GameObject("QA277LiveRange");root.transform.SetParent(G.Map.Root,false);root.transform.position=new Vector3(500,0,500);
   for(int side=0;side<4;side++){
    var wall=GameObject.CreatePrimitive(PrimitiveType.Cube);wall.name="QA277Plate"+side;wall.transform.SetParent(root.transform,false);
    bool x=side<2;wall.transform.localPosition=new Vector3(x?(side==0?-20:20):0,1.5f,x?0:(side==2?-20:20));wall.transform.localScale=x?new Vector3(.15f,3,40):new Vector3(40,3,.15f);SceneObstacle.Apply(wall,ObstacleKind.High);
   }Physics.SyncTransforms();
  }
  IEnumerator HvSeries277(TouchInput input,string label,float yaw,Vector2 offset,int count,Vector2 movement,bool counter=false,int turn=0){
   input.Shooting=false;input.Movement=Vector2.zero;G.Paused=true;G.InventoryOpen=false;G.Player.transform.position=new Vector3(500,0,500);G.Player.Health=100;G.Player.Dead=false;
   G.Rig.ResetView(yaw);G.Rig.Tick(0);G.ResetAkHandling276();G.AkHandling276.MovementStability=60;G.Magazine=30;G.ReloadTime=G.HealTime=0;G.CancelArmorRepair();G.ClearShots();Physics.SyncTransforms();HvCursor277(input,offset,counter);G.UpdateAim();
   G.Paused=false;input.Movement=movement;input.TurnPressed=turn;input.Shooting=true;float deadline=Time.realtimeSinceStartup+8;
   while(G.AkShots276.Count<count){if(Time.realtimeSinceStartup>deadline)throw new Exception("277 Live input fire timeout "+label+" shots="+G.AkShots276.Count);HvCursor277(input,offset,counter);yield return null;}
   input.Shooting=false;input.Movement=Vector2.zero;G.Paused=true;G.TickProjectiles(1.5f);
   Check(G.AkShots276.Count==count&&G.Magazine==30-count,"277 Actual Update fire creates and consumes exactly "+count+" projectiles "+label);
   bool upward=true,projection=true,ballistics=true,finite=true;float minRandom=100,maxRandom=-100;
   foreach(var s in G.AkShots276){
    upward&=Mathf.Abs(s.AdjustedScreen.x-s.RawScreen.x)<.12f&&(s.Recoil<.001f?Mathf.Abs(s.AdjustedScreen.y-s.RawScreen.y)<.12f:s.AdjustedScreen.y>s.RawScreen.y+.01f);
    projection&=Vector2.Distance(HvProject277(s,s.RawTarget),s.RawScreen)<.15f&&Vector2.Distance(HvProject277(s,s.AimTarget),s.AdjustedScreen)<.15f&&Vector2.Distance(s.InputScreen,s.RawScreen)<.15f;
    var delta=s.AimTarget-s.EyePosition;finite&=delta.magnitude>.3f&&Vector3.Dot(delta.normalized,s.RawAim)>0&&Mathf.Abs(delta.y)<.002f;
    ballistics&=Vector3.Angle(delta.normalized,s.Aim)<.02f&&Mathf.Abs(s.Direction.y)<.001f&&Mathf.Abs(Mathf.DeltaAngle(Vector3.SignedAngle(s.Aim,s.Direction,Vector3.up),s.RandomOffset))<.02f&&Mathf.Abs(s.RandomOffset)<=s.Spread+.002f;
    ballistics&=s.Ended&&s.Hit.StartsWith("QA277Plate")&&Vector3.Angle((s.Impact-s.Origin).normalized,s.Direction)<.03f;
    minRandom=Mathf.Min(minRandom,s.RandomOffset);maxRandom=Mathf.Max(maxRandom,s.RandomOffset);
    hvShots277.Add(Csv230(label)+","+s.Number+","+HvN277(s.Clock)+","+HvN277(s.Recoil)+","+HvN277(s.Spread)+","+HvN277(s.RandomOffset)+","+HvN277(s.Move)+","+HvN277(s.MoveDelta.magnitude)+","+HvN277(s.MoveDt)+","+s.MovementStability+","+HvN277(s.MovementExtraSpread)+","+HvN277(s.RawScreen.x)+","+HvN277(s.RawScreen.y)+","+HvN277(s.AdjustedScreen.x)+","+HvN277(s.AdjustedScreen.y)+","+HvN277(s.VerticalRecoilPixels)+","+HvN277(s.AimTarget.x)+","+HvN277(s.AimTarget.z)+","+Csv230(s.Hit));
   }
   Check(upward&&projection&&finite,"277 Historical screen target moves continuously upward, agrees with true aim and stays beyond player "+label);
   Check(ballistics,"277 Real planar projectile follows aim plus bounded independent spread and hits physical range "+label);
   if(count>=10)Check(maxRandom-minRandom>.5f,"277 Actual random dispersion remains after screen recoil compensation "+label);
   File.WriteAllLines(Path.Combine(folder,"277-live-input-shots.csv"),hvShots277);
  }
  void HvVerticalContinuity277(TouchInput input){
   G.Paused=true;G.Player.transform.position=new Vector3(500,0,500);G.Rig.ResetView(45);G.Rig.Tick(0);G.ResetAkHandling276();G.AkHandling276.Recoil=16;
   foreach(float y in new[]{180f,-180f}){
    HvCursor277(input,new Vector2(-2,y));G.UpdateAim();var left=G.AdjustedAimTarget277;var leftAim=G.Player.Aim;
    HvCursor277(input,new Vector2(2,y));G.UpdateAim();var right=G.AdjustedAimTarget277;
    Check(Vector3.Distance(left,right)<.2f&&Vector3.Angle(leftAim,G.Player.Aim)<4&&Vector3.Distance(right,G.Player.transform.position+Vector3.up*RaidGame.CombatHeight)>.3f,"277 Near-vertical +/-2px cursor crosses visual-facing centre without target jump or crossing player y="+y);
   }G.ResetAkHandling276();
  }
  IEnumerator HvHandling277(TouchInput input){
   HvRange277();G.Controls=input;G.Equipped=new ItemStack(ItemKind.AK,1);G.ResetAkHandling276();
   Check(WeaponRules270.Damage(ItemKind.AK)==18&&WeaponRules270.Penetration(ItemKind.AK)==10&&WeaponHandling277.AkMovementStability==60,"277 AK keeps18/10 with independent movement stability60");
   Check(HvNear277(WeaponHandling277.MovementExtraHalfAngle(60,1),.8f)&&HvNear277(WeaponHandling277.MovementExtraHalfAngle(80,1),.4f)&&HvNear277(WeaponHandling277.MovementExtraHalfAngle(30,1),1.4f)&&WeaponHandling277.MovementExtraHalfAngle(60,0)==0,"277 Independent stability parameter controls only movement extra dispersion");
   var offsets=new[]{new Vector2(180,0),new Vector2(-180,0),new Vector2(0,180),new Vector2(0,-180),new Vector2(127,127),new Vector2(-127,127),new Vector2(-127,-127),new Vector2(127,-127),new Vector2(-2,180),new Vector2(2,180),new Vector2(-2,-180),new Vector2(2,-180)};
   foreach(float yaw in new[]{45f,135f,225f,315f})for(int i=0;i<offsets.Length;i++)yield return HvSeries277(input,"yaw"+yaw+"_direction"+i,yaw,offsets[i],3,Vector2.zero);
   foreach(int turn in new[]{1,-1})foreach(var offset in new[]{new Vector2(180,0),new Vector2(-180,0),new Vector2(2,180),new Vector2(-2,-180)}){
    yield return HvSeries277(input,"turn"+turn+"_"+offset,45,offset,4,Vector2.zero,false,turn);
    var a=G.AkShots276;Check(Quaternion.Angle(a[0].CameraRotation,a[a.Count-1].CameraRotation)>1,"277 Actual Q/E camera rotation occurs during live recoil series "+turn+" "+offset);
   }
   HvVerticalContinuity277(input);
   yield return HvSeries277(input,"stationary_sweep",45,new Vector2(180,0),30,Vector2.zero);var stationary=new List<AkShotSample276>(G.AkShots276);
   Check(HvNear277(stationary[0].Spread,1.2f)&&HvNear277(stationary[29].Spread,4.95f)&&stationary[29].Recoil>=15,"277 Actual stationary first +/-1.2 and sustained +/-4.95 spread remain");
   for(int i=1;i<stationary.Count;i++)Check(stationary[i].Clock-stationary[i-1].Clock>=.109f,"277 Live firing interval remains at least .11 seconds "+i);
   yield return HvSeries277(input,"walking_sweep",45,new Vector2(180,0),30,new Vector2(0,1));var moving=new List<AkShotSample276>(G.AkShots276);
   Check(moving[0].Move>.95f&&moving[0].MoveDelta.magnitude>.001f&&HvNear277(moving[0].Spread,2)&&HvNear277(moving[29].Spread,5.75f)&&HvNear277(moving[29].MovementExtraSpread,.8f),"277 Real walking produces first +/-2.0 and maximum +/-5.75, driven by actual displacement");
   yield return HvSeries277(input,"mouse_down_counter",45,new Vector2(180,0),20,Vector2.zero,true);var counter=new List<AkShotSample276>(G.AkShots276);float compensation=0;
   foreach(var s in counter){var center=HvProject277(s,s.EyePosition);compensation=Mathf.Max(compensation,Mathf.Abs(s.AdjustedScreen.y-center.y));}
   Check(compensation<1&&counter[counter.Count-1].Recoil>10,"277 Actual downward mouse input cancels accumulated screen-up target lift while recoil persists");
   HvCursor277(input,new Vector2(180,0));G.UpdateAim();yield return HvGameCapture277("277-screen-up-game-view.png");
   var blocker=GameObject.CreatePrimitive(PrimitiveType.Cube);blocker.name="QA277MovementWall";blocker.transform.SetParent(G.Map.Root,false);
   G.Player.transform.position=new Vector3(500,0,500);G.Rig.ResetView(45);G.Rig.Tick(0);var direction=G.Rig.Forward;blocker.transform.position=G.Player.transform.position+direction*.405f+Vector3.up*1.5f;blocker.transform.rotation=Quaternion.LookRotation(direction);blocker.transform.localScale=new Vector3(10,3,.20f);SceneObstacle.Apply(blocker,ObstacleKind.High);Physics.SyncTransforms();
   G.ResetAkHandling276();G.Magazine=30;input.Movement=new Vector2(0,1);input.Shooting=false;G.Paused=false;
   float until=Time.realtimeSinceStartup+.3f;while(Time.realtimeSinceStartup<until){HvCursor277(input,new Vector2(180,0));yield return null;}
   Check(G.AkHandling276.Move<.02f&&G.LastActualMovement277.magnitude<.001f&&G.AkHandling276.MovementExtraSpread<.02f,"277 Pushing input into physical wall gives no full movement penalty");
   input.Movement=Vector2.zero;input.Shooting=true;float deadline=Time.realtimeSinceStartup+3;while(G.AkShots276.Count<1){if(Time.realtimeSinceStartup>deadline)throw new Exception("277 blocked stance first shot timeout");HvCursor277(input,new Vector2(180,0));yield return null;}
   input.Shooting=false;G.Paused=true;Check(HvNear277(G.AkShots276[0].Spread,1.2f)&&G.AkShots276[0].MovementExtraSpread<.02f,"277 No-action first shot remains stationary precision after blocked movement");
   blocker.SetActive(false);Destroy(blocker);var range=G.Map.Root.Find("QA277LiveRange");range.gameObject.SetActive(false);Destroy(range.gameObject);G.ClearShots();
   var enemy=new GameObject("QA277IndependentNPC");enemy.transform.SetParent(G.Map.Root,false);var npc=enemy.AddComponent<RaidActor>();npc.Initialize(true);npc.IsRebel=true;G.SpawnShot(npc,Vector3.right);
   Check(G.LastEnemyShotDamage276==14&&G.LastEnemyShotPen276==10&&Vector3.Distance(G.LastEnemyShotDirection276,Vector3.right)<.001f,"277 Actual NPC projectile retains independent14/10 without player handling");G.ClearShots();enemy.SetActive(false);Destroy(enemy);
  }
  Texture2D HvRender277(Camera cam,string name,int width,int height){
   var rt=new RenderTexture(width,height,24,RenderTextureFormat.ARGB32);var oldTarget=cam.targetTexture;var oldActive=RenderTexture.active;
   cam.targetTexture=rt;cam.Render();RenderTexture.active=rt;var tex=new Texture2D(width,height,TextureFormat.RGB24,false);tex.ReadPixels(new Rect(0,0,width,height),0,0);tex.Apply();cam.targetTexture=oldTarget;RenderTexture.active=oldActive;rt.Release();Destroy(rt);File.WriteAllBytes(Path.Combine(folder,name),tex.EncodeToPNG());return tex;
  }
  IEnumerator HvGameCapture277(string name){
   G.Paused=true;G.Vision.Refresh(G.Player.transform.position,G.Player.Aim);G.Player.Draw(G.View,0);yield return null;yield return new WaitForEndOfFrame();
   var tex=HvRender277(G.View,name,1600,900);int pixels=0;foreach(var c in tex.GetPixels32())if(c.r>10||c.g>10||c.b>10)pixels++;Destroy(tex);Check(pixels>1440&&G.View.orthographicSize==8.5f,"277 True game camera renders a valid unchanged-scale image "+name);
  }
  IEnumerator HvVision277(){
   G.Paused=true;G.Player.transform.position=new Vector3(500,0,500);G.Rig.ResetView(45);G.Rig.Tick(0);G.Player.Aim=G.Rig.Right;G.ClearShots();
   G.Vision.Dispose();G.Vision=new RaidVision(new Rect(480,480,40,40));G.Vision.MemoryBrightness=0;G.Vision.RoomGate=false;G.Vision.Refresh(G.Player.transform.position,G.Player.Aim);Shader.SetGlobalFloat("_HarborFogEnabled",1);
   Check(RaidVision.Range==16&&RaidVision.HalfAngle==30&&HvNear277(Shader.GetGlobalFloat("_HarborVisionRange"),16)&&HvNear277(Shader.GetGlobalFloat("_HarborConeCos"),Mathf.Cos(30*Mathf.Deg2Rad),.00001f),"277 CPU and bound flashlight use16 metres and60-degree full cone");
   var probe=GameObject.CreatePrimitive(PrimitiveType.Cube);probe.name="QA277VisibleSurfaceProbe";probe.transform.SetParent(G.Map.Root,false);probe.transform.localScale=new Vector3(.55f,.08f,.55f);probe.GetComponent<Collider>().enabled=false;
   var go=new GameObject("QA277ProbeCamera");var cam=go.AddComponent<Camera>();cam.CopyFrom(G.View);cam.enabled=false;cam.backgroundColor=Color.black;cam.transform.rotation=Quaternion.Euler(90,0,0);cam.orthographicSize=.65f;
   foreach(string shaderName in new[]{"GreyHarbor/World","GreyHarbor/RepairWall220"}){
    var shader=Shader.Find(shaderName);Check(shader!=null,"277 Actual representative renderer shader exists "+shaderName);var material=new Material(shader);material.SetColor("_Color",Color.white);material.SetFloat("_Detail",0);if(shaderName.Contains("RepairWall"))material.SetVector("_RepairWallRect220",new Vector4(480,480,520,520));probe.GetComponent<Renderer>().sharedMaterial=material;probe.GetComponent<Renderer>().shadowCastingMode=ShadowCastingMode.Off;probe.GetComponent<Renderer>().receiveShadows=false;
    var labels=new[]{"14m_inside","16_3m_outside","28deg_inside","32deg_outside"};var distances=new[]{14f,16.3f,14f,14f};var angles=new[]{0f,0f,28f,32f};
    for(int i=0;i<labels.Length;i++){
     var aim=G.Rig.Right;var point=G.Player.transform.position+(Quaternion.Euler(0,angles[i],0)*aim)*distances[i];probe.transform.position=point+Vector3.up*.04f;Physics.SyncTransforms();G.Vision.Clear();G.Vision.Refresh(G.Player.transform.position,aim);
     bool visible=G.Vision.CanSee(point),surface=G.Vision.SurfaceVisible(point),expected=i==0||i==2;Check(visible==expected&&surface==expected,"277 Actual CPU flashlight boundary "+labels[i]+" "+shaderName);
     cam.transform.position=point+Vector3.up*20;yield return null;var name="277-"+(shaderName.Contains("RepairWall")?"wall":"world")+"-"+labels[i]+"-live-fog.png";var tex=HvRender277(cam,name,256,256);float brightness=0;
     for(int x=125;x<=131;x++)for(int y=125;y<=131;y++){var c=tex.GetPixel(x,y);brightness+=(c.r+c.g+c.b)/3;}brightness/=49;Destroy(tex);
     Check(expected?brightness>.12f:brightness<.025f,"277 Real renderer pixels obey extended range and narrowed cone "+labels[i]+" "+shaderName+" brightness="+brightness);hvVision277.Add(shaderName+","+labels[i]+","+distances[i]+","+angles[i]+","+visible+","+surface+","+HvN277(brightness));
    }
    var target=G.Player.transform.position+G.Rig.Right*14;probe.transform.position=target+Vector3.up*.04f;
    var block=GameObject.CreatePrimitive(PrimitiveType.Cube);block.name="QA277RealVisionBlocker";block.transform.SetParent(G.Map.Root,false);block.transform.position=G.Player.transform.position+G.Rig.Right*7+Vector3.up*1.5f;block.transform.rotation=Quaternion.LookRotation(G.Rig.Right);block.transform.localScale=new Vector3(3,3,.25f);block.GetComponent<Renderer>().sharedMaterial=material;SceneObstacle.Apply(block,ObstacleKind.High);Physics.SyncTransforms();G.Vision.Clear();G.Vision.Refresh(G.Player.transform.position,G.Rig.Right);
    Check(!G.Vision.CanSee(target)&&!G.Vision.SurfaceVisible(target),"277 Physical high wall blocks actual CPU extended flashlight "+shaderName);cam.transform.position=target+Vector3.up*20;yield return null;var behind=HvRender277(cam,"277-"+(shaderName.Contains("RepairWall")?"wall":"world")+"-blocked-14m-live-fog.png",256,256);var black=behind.GetPixel(128,128);Destroy(behind);Check(Mathf.Max(black.r,Mathf.Max(black.g,black.b))<.025f,"277 Actual renderer behind wall remains black at14 metres "+shaderName);block.SetActive(false);Destroy(block);Destroy(material);
   }
   probe.SetActive(false);Destroy(probe);Destroy(go);G.Vision.Clear();G.Vision.Refresh(G.Player.transform.position,G.Rig.Right);yield return HvGameCapture277("277-16m-60degree-game-view.png");File.WriteAllLines(Path.Combine(folder,"277-flashlight-render-probes.csv"),hvVision277);
  }
  IEnumerator HvFloorCache277(){
   G.HarborWalkthrough=true;Check(G.BeginTraining(),"277 Start isolated actual harbor preview for flashlight cache");G.Paused=true;yield return null;G.SwitchInterior("repair-entry:"+RaidMap.RepairMainGateKeys220[1]);G.Paused=true;yield return null;
   var map=G.Map;var vision=G.Vision;var room=map.RepairMainRooms210.Find(r=>!r.ReservedStair&&!r.Door.Locked);Check(room!=null&&!room.Door.Open,"277 Actual main floor has unopened private room");G.Player.transform.position=room.ApproachPoint;G.Player.Aim=(room.WalkPoint-room.ApproachPoint).normalized;G.Rig.Tick(0);Physics.SyncTransforms();vision.Refresh(G.Player.transform.position,G.Player.Aim);
   Check(!vision.RoomAllowed(room.WalkPoint)&&!vision.Explored(room.WalkPoint)&&!vision.CanSee(room.WalkPoint)&&!G.ClearSight(room.ApproachPoint,room.WalkPoint),"277 Actual closed room door blocks sight and keeps nearby interior unknown black");yield return HvGameCapture277("277-main-closed-room-game-view.png");var seen=(bool[])vision.Seen.Clone();var actors=G.Enemies.ToArray();var hp=Array.ConvertAll(actors,e=>e.Health);var armor=Array.ConvertAll(actors,e=>e.NpcArmorPoints);
   G.SwitchInterior("repair-floor:2:north");G.Paused=true;yield return null;Check(G.Map.RepairFloor220==2&&Shader.GetGlobalFloat("_HarborVisionRange")==16&&HvNear277(Shader.GetGlobalFloat("_HarborConeCos"),Mathf.Cos(30*Mathf.Deg2Rad),.00001f),"277 New actual floor binds16m/60degree flashlight");G.SwitchInterior("repair-floor:1:south");G.Paused=true;yield return null;
   bool cache=G.Map==map&&G.Vision==vision;for(int i=0;i<seen.Length;i++)cache&=!seen[i]||vision.Seen[i];for(int i=0;i<actors.Length;i++)cache&=G.Enemies[i]==actors[i]&&actors[i].Health==hp[i]&&actors[i].NpcArmorPoints==armor[i];
   Check(cache&&Shader.GetGlobalTexture("_HarborVisibility")==vision.Texture&&Shader.GetGlobalFloat("_HarborVisionRange")==16&&!vision.RoomAllowed(room.WalkPoint)&&!room.Door.Open,"277 Actual1-to2-to1 cache preserves fog, closed room and NPC state with new flashlight range");G.Finish(false);yield return null;
  }
  IEnumerator VerifyHandlingVision277(){
   var previousInput=G.Controls;var previousRandom=UnityEngine.Random.state;string dir=Path.GetDirectoryName(G.SavePath);string[] formal={Path.Combine(dir,"stash-v40.json"),Path.Combine(dir,"stash-v41.json")};byte[][] bytes=Array.ConvertAll(formal,p=>File.Exists(p)?File.ReadAllBytes(p):null);
   Check(Path.GetFileName(G.SavePath)=="verification-stash-2.7.7.json","277 Handling/vision uses isolated verification inventory");hvShots277.Clear();hvVision277.Clear();hvShots277.Add("mode,shot,clock,recoil,spread,randomYaw,actualMoveRatio,moveDistance,moveDt,stability,movementExtra,rawScreenX,rawScreenY,adjustedScreenX,adjustedScreenY,recoilPixels,aimTargetX,aimTargetZ,hit");hvVision277.Add("shader,case,distance,angle,cpuVisible,cpuSurface,actualPixelBrightness");
   FreshCamp();string camp=JsonUtility.ToJson(G.Save);G.HardMode=false;G.HarborWalkthrough=false;Check(G.BeginTraining(),"277 Start isolated actual input training");G.Paused=true;yield return null;var input=new TouchInput();
   try{
    UnityEngine.Random.InitState(20261003);yield return HvHandling277(input);yield return HvVision277();G.Controls=previousInput;G.Finish(false);yield return null;Check(JsonUtility.ToJson(G.Save)==camp,"277 Input/render preview restores exact inventory");yield return HvFloorCache277();Check(JsonUtility.ToJson(G.Save)==camp,"277 Floor-cache preview restores exact inventory");
    for(int i=0;i<formal.Length;i++)Check(MainBytes273(bytes[i],File.Exists(formal[i])?File.ReadAllBytes(formal[i]):null),"277 Formal save remains byte-for-byte unchanged "+Path.GetFileName(formal[i]));
   }finally{G.Controls=previousInput;UnityEngine.Random.state=previousRandom;G.Paused=true;File.WriteAllLines(Path.Combine(folder,"277-live-input-shots.csv"),hvShots277);File.WriteAllLines(Path.Combine(folder,"277-flashlight-render-probes.csv"),hvVision277);File.WriteAllText(Path.Combine(folder,"277-capture-notes.txt"),"All PNGs use Camera.Render with real CPU visibility and shader fog. No ScreenCapture or fabricated HUD.\n*-game-view.png keeps the actual 8.5 orthographic game camera. Remote handling fixtures do not show the original map, and the game frame is not evidence that every 16m endpoint fits on screen.\n*-live-fog.png uses a separate close-up camera aimed at a physical renderer sample; its real World/RepairWall shader, range/cone and actual collider occlusion are checked by recorded pixels.\n");}
  }
 }
}
