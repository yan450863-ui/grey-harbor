using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using UnityEngine;
namespace GreyHarbor {
 public sealed partial class RaidVerification {
  readonly List<string> sprintRows2711=new List<string>();
  void SprintRecord2711(string label,float dt){var s=G.Sprint2711;sprintRows2711.Add(label+","+WN279(dt)+","+WN279(s.Stamina)+","+s.Intent+","+s.Exhausted+","+s.Running+","+WN279(G.LastActualMovement277.magnitude)+","+WN279(s.LastRunningSeconds));File.WriteAllLines(Path.Combine(folder,"2711-real-sprint-state.csv"),sprintRows2711);}
  void SprintStep2711(Vector3 velocity,float dt,string label){
   // Each independent distance probe starts at the origin of the empty lane.
   // This removes distant-coordinate quantization without changing actor physics.
   G.Player.transform.position=Vector3.zero;Physics.SyncTransforms();G.Paused=false;G.MovePlayer2711(velocity,dt);G.Paused=true;SprintRecord2711(label,dt);
  }
  IEnumerator SprintShift2711(TouchInput input){input.Movement=Vector2.zero;input.Shooting=false;input.SprintPressed=true;G.Paused=false;yield return null;G.Paused=true;Check(!input.SprintPressed,"2711 Actual Update consumes Shift toggle exactly once");}
  void SprintDeplete2711(){for(int i=0;i<20;i++)SprintStep2711(Vector3.right*3.4f,1,"deplete_"+i);Check(G.Sprint2711.Stamina==0&&G.Sprint2711.Intent&&G.Sprint2711.Exhausted&&!G.Sprint2711.CanRun,"2711 Twenty real moving seconds exhaust20 stamina while preserving run intent");}
  IEnumerator SprintMovement2711(TouchInput input){
   sprintRows2711.Clear();sprintRows2711.Add("case,dt,stamina,intent,exhausted,running,actualDistance,actualRunningSeconds");WeaponsRemote279();G.Player.transform.position=Vector3.zero;G.ResetSprint2711();
   Check(G.Sprint2711.Stamina==20&&!G.Sprint2711.Intent&&!G.Sprint2711.Exhausted,"2711 New action begins at20 stamina with walking intent");
   SprintStep2711(Vector3.right*3.4f,1,"walk");Check(WNear279(G.LastActualMovement277.magnitude,3.4f)&&G.Sprint2711.Stamina==20&&!G.Sprint2711.Running,"2711 Real walking keeps3.4 speed and full stamina");
   yield return SprintShift2711(input);Check(G.Sprint2711.Intent&&G.Sprint2711.CanRun,"2711 Shift turns persistent run intent on");SprintStep2711(Vector3.right*3.4f,1,"run_one_second");
   Check(WNear279(G.LastActualMovement277.magnitude,5.1f)&&WNear279(G.Sprint2711.Stamina,19)&&G.Sprint2711.Running,"2711 Real running moves1.5 times walking speed and spends1 stamina per second");
   float stamina=G.Sprint2711.Stamina;SprintStep2711(Vector3.zero,.5f,"stationary_recover");Check(WNear279(G.Sprint2711.Stamina,stamina+.75f)&&!G.Sprint2711.Running,"2711 Stationary run intent does not spend and recovers1.5 per second");
   G.ResetSprint2711();yield return SprintShift2711(input);SprintDeplete2711();SprintStep2711(Vector3.right*3.4f,1,"empty_walk_recover1");Check(WNear279(G.LastActualMovement277.magnitude,3.4f)&&WNear279(G.Sprint2711.Stamina,1.5f)&&G.Sprint2711.Exhausted,"2711 Exhausted actor really walks and recovers without immediately running");
   SprintStep2711(Vector3.right*3.4f,1,"empty_walk_recover2");Check(WNear279(G.Sprint2711.Stamina,3)&&!G.Sprint2711.CanRun,"2711 Recovery below4 retains exhaustion gate");SprintStep2711(Vector3.right*3.4f,2f/3,"recover_to4");
   Check(WNear279(G.Sprint2711.Stamina,4)&&G.Sprint2711.CanRun&&!G.Sprint2711.Exhausted,"2711 Recovery to4 releases fatigue while intent remains on");SprintStep2711(Vector3.right*3.4f,1,"automatic_resume");Check(WNear279(G.LastActualMovement277.magnitude,5.1f)&&G.Sprint2711.Running,"2711 Next real movement automatically resumes running at recovered threshold");
   G.ResetSprint2711();G.Sprint2711.Toggle();for(int i=0;i<19;i++)SprintStep2711(Vector3.right*3.4f,1,"split_setup_"+i);SprintStep2711(Vector3.right*3.4f,.5f,"split_setup_half");Check(WNear279(G.Sprint2711.Stamina,.5f),"2711 Real movement leaves half-second running budget");SprintStep2711(Vector3.right*3.4f,1,"exhaustion_split_frame");
   Check(WNear279(G.LastActualMovement277.magnitude,4.25f)&&WNear279(G.Sprint2711.Stamina,.75f)&&G.Sprint2711.Exhausted&&!G.Sprint2711.CanRun,"2711 Exhaustion frame splits .5second run plus .5second walk, then recovers .75 without bypassing fatigue");
   G.ResetSprint2711();yield return SprintShift2711(input);SprintDeplete2711();yield return SprintShift2711(input);Check(!G.Sprint2711.Intent&&G.Sprint2711.Exhausted,"2711 Shift off during exhaustion does not bypass fatigue gate");SprintStep2711(Vector3.right*3.4f,3,"off_recovery");SprintStep2711(Vector3.right*3.4f,1,"off_stays_walking");
   Check(!G.Sprint2711.Intent&&!G.Sprint2711.Running&&!G.Sprint2711.CanRun&&WNear279(G.LastActualMovement277.magnitude,3.4f),"2711 Recovered stamina cannot auto-run after intent is switched off");
   G.ResetSprint2711();yield return SprintShift2711(input);SprintStep2711(Vector3.right*3.4f,1,"pause_setup");stamina=G.Sprint2711.Stamina;input.SprintPressed=true;input.Movement=Vector2.one;G.Paused=true;yield return new WaitForSeconds(.15f);input.Movement=Vector2.zero;
   Check(G.Sprint2711.Stamina==stamina&&G.Sprint2711.Intent&&!G.Sprint2711.Running&&!input.SprintPressed,"2711 Paused actual Update freezes stamina and discards paused Shift press");Check(RaidUI.SprintStatus2711(G.Sprint2711,true)=="暂停"&&WNear279(RaidUI.SprintFill2711(G.Sprint2711),stamina/20),"2711 Actual HUD status and normalized fill report paused state");
   var wall=GameObject.CreatePrimitive(PrimitiveType.Cube);wall.name="QA2711SprintWall";wall.transform.SetParent(G.Map.Root,false);wall.transform.position=new Vector3(.405f,1.5f,0);wall.transform.localScale=new Vector3(.2f,3,10);SceneObstacle.Apply(wall,ObstacleKind.High);Physics.SyncTransforms();G.ResetSprint2711();G.Sprint2711.Toggle();SprintStep2711(Vector3.right*3.4f,1,"wall_blocked");
   Check(G.LastActualMovement277.magnitude<.00001f&&G.Sprint2711.Stamina==20&&!G.Sprint2711.Running,"2711 Real wall prevents movement and stamina cost despite run intent");var slide=new Vector3(1,0,1).normalized*3.4f;SprintStep2711(slide,1,"wall_slide");
   Check(G.LastActualMovement277.magnitude>.5f&&G.LastActualMovement277.magnitude<5.1f&&WNear279(G.Sprint2711.Stamina,19)&&WNear279(G.Sprint2711.LastRunningSeconds,1)&&G.Sprint2711.Running,"2711 Real partial wall slide still spends entire moving run second rather than healing");SprintStep2711(slide,1,"inventory_recovery_setup");stamina=G.Sprint2711.Stamina;var heldPosition=G.Player.transform.position;G.InventoryOpen=true;G.Paused=false;G.MovePlayer2711(slide,1);G.Paused=true;
   Check(G.Player.transform.position==heldPosition&&WNear279(G.Sprint2711.Stamina,stamina+1.5f)&&!G.Sprint2711.Running,"2711 Backpack prevents movement and recovers1.5 stamina per second");G.InventoryOpen=false;wall.SetActive(false);Destroy(wall);G.ResetSprint2711();G.Sprint2711.Toggle();
   Check(RaidUI.SprintStatus2711(G.Sprint2711,false).Contains("移动时跑")&&RaidUI.SprintFill2711(G.Sprint2711)==1,"2711 Actual HUD distinguishes enabled intent from moving run");
   G.Finish(false);yield return null;Check(G.Sprint2711.Stamina==20&&!G.Sprint2711.Intent,"2711 Return to camp resets transient stamina and intent");Check(G.BeginTraining(),"2711 Begin another isolated action");G.Paused=true;yield return null;WeaponsRemote279();Check(G.Sprint2711.Stamina==20&&!G.Sprint2711.Intent&&!G.Sprint2711.Exhausted,"2711 New actual action resets stamina independently of previous fatigue");
  }
  IEnumerator SprintSpread2711(TouchInput input){
   var rows=new List<string>{"weapon,running,moveDistance,moveDt,actualSpeedRatio,reportedSpeedRatio,extra,spread,stamina"};
   var guns=new[]{ItemKind.AK,ItemKind.MP5,ItemKind.SKS};float[] first={1.2f,1.6f,.35f},walkExtra={.8f,.4f,1.4f};
   for(int i=0;i<guns.Length;i++)foreach(bool running in new[]{false,true}){
    WeaponsEquipFixture279(guns[i],input);G.ResetSprint2711();if(running)G.Sprint2711.Toggle();G.Player.transform.position=Vector3.zero;G.Rig.Tick(0);WeaponsAim279(input);G.UpdateAim();yield return new WaitForSeconds(.36f);
    G.Paused=false;G.MovePlayer2711(Vector3.right*3.4f,.05f);float ratio=G.LastActualMovement277.magnitude/(3.4f*.05f);G.TickAkHandling276(.05f,ratio);G.Rig.Tick(0);WeaponsAim279(input);G.UpdateAim();G.FirePlayer();G.Paused=true;
    Check(G.AkShots276.Count==1,"2711 Actual single shot samples moving weapon "+guns[i]+" running="+running);var sample=G.AkShots276[0];float actualRatio=Mathf.Clamp(sample.MoveDelta.magnitude/(3.4f*sample.MoveDt),0,1.5f);
    rows.Add(guns[i]+","+running+","+WN279(sample.MoveDelta.magnitude)+","+WN279(sample.MoveDt)+","+WN279(actualRatio)+","+WN279(sample.Move)+","+WN279(sample.MovementExtraSpread)+","+WN279(sample.Spread)+","+WN279(G.Sprint2711.Stamina));File.WriteAllLines(Path.Combine(folder,"2711-real-walk-run-dispersion.csv"),rows);
    Check(WNear279(actualRatio,running?1.5f:1)&&WNear279(sample.Move,actualRatio)&&WNear279(sample.MovementExtraSpread,walkExtra[i]*actualRatio)&&WNear279(sample.Spread,first[i]+walkExtra[i]*actualRatio),"2711 Actual speed drives unchanged walking and1.5x running dispersion "+guns[i]+" running="+running+" ratio="+actualRatio+" spread="+sample.Spread);
    G.ClearShots();
   }
  }
  IEnumerator SprintSks2711(TouchInput input){
   Check(WeaponRules279.SKS.Damage==28&&WeaponRules279.SKS.Penetration==10&&WeaponRules279.AK.Damage==18&&WeaponRules279.MP5.Damage==12,"2711 SKS changed damage28 with penetration10 and other guns retained");var rows=new List<string>{"weapon,ammo,targetLevel,expectedGradeMultiplier,targetMetres,armored,remainingHealth,remainingArmor,termination,travelMetres"};
   foreach(var ammo in new[]{ItemKind.GreenAmmo,ItemKind.Ammo}){yield return WeaponsShot279(ItemKind.SKS,ammo,3,true,rows,input);yield return WeaponsShot279(ItemKind.SKS,ammo,3,false,rows,input);}File.WriteAllLines(Path.Combine(folder,"2711-real-sks-hits.csv"),rows);yield return null;var npc=new List<string>{"ammo,targetMetres,stepSeconds,remainingHealth,termination,travelMetres"};RangeNpc278(npc);File.WriteAllLines(Path.Combine(folder,"2711-independent-npc-shot.csv"),npc);
  }
  sealed class SprintFloorCache2711 {
   public RaidMap Map;public RaidVision Vision;public LootContainer[] Boxes;public RaidActor[] Actors;public string[] Contents;public bool[] Opened,Searched,Doors,Seen,Dead;public float[] Health,Armor;
  }
  SprintFloorCache2711 SprintFloorSnapshot2711(){
   var c=new SprintFloorCache2711{Map=G.Map,Vision=G.Vision,Boxes=G.Map.Loot.ToArray(),Actors=G.Enemies.ToArray(),Doors=G.Map.Doors.ConvertAll(d=>d.Open).ToArray(),Seen=(bool[])G.Vision.Seen.Clone()};
   c.Contents=Array.ConvertAll(c.Boxes,b=>JsonUtility.ToJson(new ItemList230{Items=b.Items}));c.Opened=Array.ConvertAll(c.Boxes,b=>b.Opened);c.Searched=Array.ConvertAll(c.Boxes,b=>b.Prop.Searched);c.Health=Array.ConvertAll(c.Actors,e=>e.Health);c.Armor=Array.ConvertAll(c.Actors,e=>e.NpcArmorPoints);c.Dead=Array.ConvertAll(c.Actors,e=>e.Dead);return c;
  }
  void SprintFloorCacheCheck2711(SprintFloorCache2711 c){
   bool cache=G.Map==c.Map&&G.Vision==c.Vision&&G.Map.Loot.Count==c.Boxes.Length&&G.Enemies.Count==c.Actors.Length;
   for(int i=0;i<c.Boxes.Length;i++)cache&=G.Map.Loot[i]==c.Boxes[i]&&c.Boxes[i].Opened==c.Opened[i]&&c.Boxes[i].Prop.Searched==c.Searched[i]&&JsonUtility.ToJson(new ItemList230{Items=c.Boxes[i].Items})==c.Contents[i];
   for(int i=0;i<c.Actors.Length;i++)cache&=G.Enemies[i]==c.Actors[i]&&c.Actors[i].Health==c.Health[i]&&c.Actors[i].NpcArmorPoints==c.Armor[i]&&c.Actors[i].Dead==c.Dead[i];
   for(int i=0;i<c.Doors.Length;i++)cache&=G.Map.Doors[i].Open==c.Doors[i];for(int i=0;i<c.Seen.Length;i++)cache&=!c.Seen[i]||G.Vision.Seen[i];
   Check(cache,"2711 Actual floor cache preserves all38 box identities/search/contents, doors, fog and10 NPC pools floor="+G.Map.RepairFloor220);
  }
  void SprintInteract2711(bool held,float dt,Vector3 target){
   G.Rig.Tick(0);var marker=new Vector3(target.x,.65f,target.z);var cursor=G.View.WorldToScreenPoint(marker);G.Controls=new TouchInput{Searching=held,AimPosition=new Vector2(cursor.x,cursor.y)};
   var direction=target-G.Player.transform.position;direction.y=0;if(direction.sqrMagnitude>.000001f)G.Player.Aim=direction.normalized;G.Vision.Refresh(G.Player.transform.position,G.Player.Aim);G.TickInteraction(dt);
  }
  void SprintDoorDiagnostic2711(WorldDoor expected){
   if(G.NearbyDoor==expected)return;Debug.Log("2711 DOOR SELECT floor="+G.Map.RepairFloor220+" expected="+expected.Name+" selectedDoor="+(G.NearbyDoor==null?"null":G.NearbyDoor.Name)+" selectedBox="+(G.Nearby==null?"null":G.Nearby.Name)+" player="+G.Player.transform.position+" door="+expected.Position+" distance="+Vector3.Distance(G.Player.transform.position,expected.Position)+" aim="+G.Player.Aim+" cursor="+G.Controls.AimScreen);
   foreach(var box in G.Map.Loot)if(G.CanInteractLoot(box))Debug.Log("2711 DOOR CANDIDATE box="+box.Name+" bounds="+G.LootBounds(box));var from=G.Player.transform.position+Vector3.up*RaidGame.CombatHeight;var to=expected.Position+Vector3.up*RaidGame.CombatHeight;foreach(var hit in Physics.RaycastAll(from,(to-from).normalized,Vector3.Distance(from,to),SceneObstacle.SightMask,QueryTriggerInteraction.Ignore))Debug.Log("2711 DOOR REACH collider="+hit.collider.name+" ownDoor="+(hit.collider==expected.Collider)+" distance="+hit.distance);
  }
  IEnumerator SprintRoomDoors2711(){
   MainArrivalDoor273();foreach(var room in G.Map.RepairMainRooms210){
    if(room.Door.Locked)continue;MainWalk273(room.ApproachPoint,"2711 extra boxes preserve room approach "+room.Name);
    if(!room.Door.Open){var stand=room.Door.Position+(room.ApproachPoint-room.WalkPoint).normalized*(room.ReservedStair?.60f:1.05f);MainWalk273(stand,"2711 actual door interaction "+room.Name);
     // A transfer first requires an F release. That tick intentionally returns
     // before selecting; the next released tick performs the real selection.
     SprintInteract2711(false,0,room.Door.Position);SprintInteract2711(false,0,room.Door.Position);SprintDoorDiagnostic2711(room.Door);Check(G.NearbyDoor==room.Door,"2711 New box placement does not intercept intended room door "+room.Name);SprintInteract2711(true,.01f,room.Door.Position);SprintInteract2711(false,0,room.Door.Position);Check(room.Door.Open,"2711 Real F opens affected room door "+room.Name);}
    MainWalk273(room.WalkPoint,"2711 actual doorway traversal "+room.Name);MainWalk273(room.ApproachPoint,"2711 doorway return "+room.Name);yield return null;
   }
  }
  IEnumerator SprintSearchExtra2711(MainExtraContainer2711 extra,List<string> rows){
   var box=extra.Box;int count=box.Items.Count;Check(count>=1&&count<=7,"2711 Added ordinary box has original bounded quantity "+box.Name);MainWalk273(box.Position,"2711 reach added searchable stand "+box.Name);G.Player.Aim=(box.Prop.transform.position-G.Player.transform.position).normalized;G.Rig.Tick(0);G.Vision.Refresh(G.Player.transform.position,G.Player.Aim);
   Check(G.CanInteractLoot(box),"2711 Real added-box prop is visible and in physical interaction range "+box.Name);SprintInteract2711(false,0,box.Prop.transform.position);SprintInteract2711(false,0,box.Prop.transform.position);SprintInteract2711(true,box.SearchDuration+.02f,box.Prop.transform.position);Check(box.Opened&&G.OpenLoot==box&&box.Prop.Searched,"2711 Real F searches intended added box without adjacent interaction theft "+box.Name);
   G.Paused=false;for(int i=0;i<7;i++)G.TickIdentification(3);G.Paused=true;Check(box.Items.Count==count&&box.Items.TrueForAll(item=>item.Identified),"2711 New search keeps exact contents and identifies all items "+box.Name);var propPosition=box.Prop.transform.position;rows.Add(G.Map.RepairFloor220+","+Csv230(box.Name)+","+Csv230(extra.Plan.RoomName)+","+WN279(box.Position.x)+","+WN279(box.Position.z)+","+WN279(propPosition.x)+","+WN279(propPosition.y)+","+WN279(propPosition.z)+","+count+",true");File.WriteAllLines(Path.Combine(folder,"2711-new-box-real-search.csv"),rows);G.CloseInventory();F111(false,0);yield return null;
  }
  void SprintPatrolClear2711(List<string> rows){
   var player=G.Player.transform.position;G.Player.transform.position=new Vector3(-1000,0,-1000);
   foreach(var actor in G.Enemies){
    Check(Clear140(actor.Home)&&actor.PatrolTargets!=null&&actor.PatrolTargets.Length>=2,"2711 Extra box does not block NPC home "+actor.name);
    foreach(var target in actor.PatrolTargets){var route=G.Navigation.FindPath(actor.Home,target);bool clear=Clear140(target)&&route.Count>0&&Vector3.Distance(route[route.Count-1],target)<.003f;var last=actor.Home;foreach(var point in route){clear&=G.Navigation.ClearSegment(last,point);last=point;}Check(clear,"2711 Extra box leaves .34m NPC patrol route "+actor.name+" "+target);}
    var original=actor.transform.position;float wait=actor.WaitTime;int index=actor.PatrolIndex;actor.transform.position=actor.Home;actor.WaitTime=0;actor.PatrolIndex=1;actor.Route.Clear();float travelled=0;bool blocked=false;
    for(int tick=0;tick<120;tick++){var before=actor.transform.position;if(actor.IsRebel)G.TickRebel145(actor,.05f);else G.HarborPatrol(actor,.05f);Physics.SyncTransforms();travelled+=Vector3.Distance(before,actor.transform.position);blocked|=Physics.CheckCapsule(actor.transform.position+Vector3.up*.35f,actor.transform.position+Vector3.up,.29f,SceneObstacle.MovementMask,QueryTriggerInteraction.Ignore);}
    Check(travelled>.5f&&!blocked,"2711 Actual NPC patrol advances without new-box collision "+actor.name);rows.Add(G.Map.RepairFloor220+","+Csv230(actor.name)+","+WN279(travelled)+","+blocked);actor.transform.position=original;actor.WaitTime=wait;actor.PatrolIndex=index;actor.Route.Clear();
   }G.Player.transform.position=player;Physics.SyncTransforms();File.WriteAllLines(Path.Combine(folder,"2711-affected-npc-patrol.csv"),rows);
  }
  IEnumerator SprintHudCapture2711(){
   var extra=G.Map.MainExtraContainers2711.Find(e=>e.Plan.RoomName=="维修大厅")??G.Map.MainExtraContainers2711[0];MainWalk273(extra.Box.Position,"2711 main HUD photograph stand");G.CloseInventory();G.Rig.Tick(0);var aim=G.View.WorldToScreenPoint(extra.Box.Prop.transform.position);G.Controls=new TouchInput{AimPosition=new Vector2(aim.x,aim.y)};G.UpdateAim();G.Vision.Refresh(G.Player.transform.position,G.Player.Aim);G.NoticeTime=0;G.Paused=false;yield return null;yield return new WaitForEndOfFrame();
   var image=ScreenCapture.CaptureScreenshotAsTexture();int lit=0;if(image!=null)foreach(var p in image.GetPixels32())if(p.r>15||p.g>15||p.b>15)lit++;bool available=image!=null&&lit>1440;
   if(image!=null){File.WriteAllBytes(Path.Combine(folder,available?"2711-main-actual-stamina-hud.png":"2711-hud-unavailable-black.png"),image.EncodeToPNG());Destroy(image);}G.Paused=true;
   if(available)Check(true,"2711 Actual displayed main frame captures native stamina HUD with ScreenCapture");else {string note="INFO 2711 Native HUD screenshot unavailable: hidden window did not provide usable displayed pixels; gameplay and actual HUD API checks remain independently verified.";results.Add(note);Debug.Log(note);}
   File.WriteAllText(Path.Combine(folder,"2711-stamina-hud-state.txt"),(available?"Actual ScreenCapture after WaitForEndOfFrame; native OnGUI HUD retained.":"ScreenCapture unavailable/black from hidden window; this image is not valid HUD evidence.")+" Stamina="+G.Sprint2711.Stamina+" Intent="+G.Sprint2711.Intent+" status="+RaidUI.SprintStatus2711(G.Sprint2711,false)+"\n");
  }
  IEnumerator SprintMainLoot2711(){
   G.HarborWalkthrough=true;Check(G.BeginTraining(),"2711 Begin isolated actual main building preview");G.Paused=true;yield return null;G.SwitchInterior("repair-entry:"+RaidMap.RepairMainGateKeys220[1]);G.Paused=true;yield return null;
   G.ResetSprint2711();G.Sprint2711.Toggle();G.Paused=false;G.MovePlayer2711(Vector3.forward*3.4f,.1f);G.Paused=true;float stamina=G.Sprint2711.Stamina;bool intent=G.Sprint2711.Intent,exhausted=G.Sprint2711.Exhausted;
   int[] counts={10,10,10,8},added={3,2,3,2},enemies={3,2,2,3};var states=new SprintFloorCache2711[4];var standRows=new List<string>{"floor,name,extra,standX,standZ,propX,propY,propZ"};var searches=new List<string>{"floor,name,room,standX,standZ,propX,propY,propZ,items,opened"};var patrol=new List<string>{"floor,actor,actualDistance6seconds,collision"};int searched=0;
   double[] quality={60,32,7,1,0};var actualQuality=LootBalance.QualityWeights(ContainerTier.Common);for(int i=0;i<5;i++)Check(actualQuality[i]==quality[i],"2711 Existing ordinary quality coefficient retained "+i);Check(ContainerDropRules230.BonusRedChance==.001,"2711 Existing separate bonus red chance retained");
   for(int floor=1;floor<=4;floor++){
    var map=G.Map;Check(map.RepairFloor220==floor&&map.Loot.Count==counts[floor-1]&&map.MainExtraContainers2711.Count==added[floor-1]&&G.Enemies.Count==enemies[floor-1]&&map.MainSearchPlacements274.Count==counts[floor-1],"2711 Actual four-floor population totals38 searchable boxes and10 NPC floor="+floor);
    yield return SprintRoomDoors2711();foreach(var point in map.RepairMainWalkPoints210)MainWalk273(point,"2711 new boxes preserve floor circulation");
    foreach(var placement in map.MainSearchPlacements274){var box=placement.Box;MainWalk273(box.Position,"2711 all38 existing/additional search stands remain physically reachable "+box.Name);var pos=box.Prop.transform.position;standRows.Add(floor+","+Csv230(box.Name)+","+map.MainExtraContainers2711.Exists(e=>e.Box==box)+","+WN279(box.Position.x)+","+WN279(box.Position.z)+","+WN279(pos.x)+","+WN279(pos.y)+","+WN279(pos.z));}
    File.WriteAllLines(Path.Combine(folder,"2711-all38-real-search-stands.csv"),standRows);
    foreach(var extra in map.MainExtraContainers2711){var profile=extra.Box.DropProfile230;Check(profile!=null&&profile.Tier==ContainerTier.Common&&profile.FixedCount==0&&profile.EnableBonusRed&&profile.Region==DropRegion230.Repair&&extra.Placement!=null&&extra.Placement.Box==extra.Box&&map.MainSearchPlacements274.Contains(extra.Placement),"2711 Added box uses ordinary unchanged profile and real registered placement "+extra.Box.Name);Check(extra.Placement.RoomName==extra.Plan.RoomName&&extra.Placement.RoomArea.Contains(new Vector2(extra.Box.Prop.transform.position.x,extra.Box.Prop.transform.position.z)),"2711 Added prop physically occupies its intended room "+extra.Box.Name);Check(extra.Box.Items.TrueForAll(p=>ContainerDropRules230.Eligible(profile,p.Kind,RaidInventory.Grade(p.Kind)==4)),"2711 Added contents remain eligible for ordinary category "+extra.Box.Name);yield return SprintSearchExtra2711(extra,searches);searched++;}
    foreach(var portal in map.Doors)if(portal.Portal)MainWalk273(portal.Destination.StartsWith("repair-exit:")?portal.Position+Vector3.forward:portal.Position,"2711 real stairs/exit path remains free "+portal.Name);
    SprintPatrolClear2711(patrol);states[floor-1]=SprintFloorSnapshot2711();Check(G.Sprint2711.Stamina==stamina&&G.Sprint2711.Intent==intent&&G.Sprint2711.Exhausted==exhausted,"2711 Paused traversal and actual floor transfer preserve stamina/intent floor="+floor);if(floor<4)yield return MainStair273(floor+1,true);
   }
   Check(searched==10,"2711 All10 new containers searched with real F");for(int floor=3;floor>=1;floor--){yield return MainStair273(floor,false);SprintFloorCacheCheck2711(states[floor-1]);Check(G.Sprint2711.Stamina==stamina&&G.Sprint2711.Intent==intent&&G.Sprint2711.Exhausted==exhausted,"2711 Cached floor returns preserve sprint state "+floor);}if(Array.IndexOf(Environment.GetCommandLineArgs(),"--capture-sprint-hud-2711")>=0)yield return SprintHudCapture2711();else{string note="INFO 2711 Native HUD screenshot not requested: hidden-window ScreenCapture is unavailable. Actual OnGUI rendered during gameplay checks; status/fill API verified. No substitute image produced.";results.Add(note);Debug.Log(note);File.WriteAllText(Path.Combine(folder,"2711-stamina-hud-state.txt"),note+"\n");}G.Finish(false);yield return null;
  }
  IEnumerator VerifySprintLoot2711(){
   Check(Application.version=="2.7.11"&&Path.GetFileName(G.SavePath)=="verification-stash-2.7.11.json","2711 Current build uses dedicated isolated inventory");string dir=Path.GetDirectoryName(G.SavePath);string[] formal={"stash-v40.json","stash-v41.json","stash-v42.json"};byte[][] before=Array.ConvertAll(formal,n=>File.Exists(Path.Combine(dir,n))?File.ReadAllBytes(Path.Combine(dir,n)):null);var controls=G.Controls;var random=UnityEngine.Random.state;
   FreshCamp();string camp=JsonUtility.ToJson(G.Save);G.HardMode=false;G.HarborWalkthrough=false;Check(G.BeginTraining(),"2711 Isolated actual action for sprint and affected weapon paths");G.Paused=true;yield return null;var input=new TouchInput();
   try{G.Controls=input;UnityEngine.Random.InitState(2711);yield return SprintMovement2711(input);yield return SprintSpread2711(input);yield return SprintSks2711(input);G.Finish(false);yield return null;Check(JsonUtility.ToJson(G.Save)==camp,"2711 Sprint/weapon preview restores original inventory");yield return SprintMainLoot2711();Check(JsonUtility.ToJson(G.Save)==camp,"2711 New-main-box preview restores original inventory");for(int i=0;i<formal.Length;i++)Check(MainBytes273(before[i],File.Exists(Path.Combine(dir,formal[i]))?File.ReadAllBytes(Path.Combine(dir,formal[i])):null),"2711 Formal save protected "+formal[i]);}
   finally{G.Controls=controls;UnityEngine.Random.state=random;G.Paused=true;File.WriteAllLines(Path.Combine(folder,"2711-real-sprint-state.csv"),sprintRows2711);}
  }
 }
}
