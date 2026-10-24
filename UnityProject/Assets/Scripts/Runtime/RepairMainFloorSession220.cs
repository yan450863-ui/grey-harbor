using UnityEngine;
using System.Collections.Generic;
namespace GreyHarbor {public sealed partial class RaidGame {
 void RevealRepairArrival220(){
  Vision.RevealArea(Map.InitialRoom);foreach(var area in Map.RepairHallAreas220)Vision.RevealArea(area);
  var p=new Vector2(Player.transform.position.x,Player.transform.position.z);foreach(var room in Map.RepairMainRooms210)if(room.Area.Contains(p))Vision.RevealArea(room.Area);
 }
 bool TrySwitchRepairFloor220(string destination){
  if(destination==null)return false;
  if(destination.StartsWith("repair-exit:")){
   if(Phase!=GamePhase.Raid||!Map.RepairMain220||Map.RepairFloor220!=1||outsideArea==null)return true;
   int index=System.Array.IndexOf(RaidMap.RepairMainGateKeys220,destination.Substring(12));if(index<0)return true;
   outsideReturn=RaidMap.RepairMainApproach220(index);SwitchInterior("repairmain1");return true;
  }
  if(!destination.StartsWith("repair-floor:"))return false;
  if(Phase!=GamePhase.Raid||!Map.RepairMain220||outsideArea==null)return true;
  var parts=destination.Split(':');if(parts.Length!=3||!int.TryParse(parts[1],out int floor)||floor<1||floor>4||(parts[2]!="north"&&parts[2]!="south")||Mathf.Abs(floor-Map.RepairFloor220)!=1)return true;
  string key="repairmain"+floor;var current=indoorAreas[Map.InteriorId];current.Yaw=Rig.DestinationYaw;current.Actors=new List<RaidActor>(Enemies);
  foreach(var e in Enemies){e.Windup=e.Lunge=e.AttackPose=0;e.RebelBurst=0;e.AttackCooldown=Mathf.Max(e.AttackCooldown,1);}
  CancelArmorRepair();CloseInventory();ClearShots();Enemies.Clear();Player.transform.SetParent(transform,true);Map.Root.gameObject.SetActive(false);
  if(!indoorAreas.TryGetValue(key,out var target)){
   Map=new RaidMap{HardLoot=ActiveHardMode};if(floor==1)Map.BuildRepairMain210();else Map.BuildRepairMainUpper220(floor);Map.FinishInteriorDoors2743();
   Vision=new RaidVision(Map.Bounds){RoomGate=true};Vision.RevealArea(Map.InitialRoom);Navigation=new RaidNavigation(Map.Bounds,.25f);
   PopulateInteriorMonsters();
   target=new AreaState{Map=Map,Vision=Vision,Navigation=Navigation,Yaw=current.Yaw,Actors=new List<RaidActor>(Enemies)};indoorAreas.Add(key,target);
  }
  Map=target.Map;Vision=target.Vision;Navigation=target.Navigation;Map.Root.gameObject.SetActive(true);Enemies.Clear();Enemies.AddRange(target.Actors);Player.transform.SetParent(Map.Root,true);Player.transform.position=RaidMap.RepairStairArrival220(parts[2]=="north");RevealRepairArrival220();
  Rig.Target=Player.transform;Rig.ResetView(target.Yaw);View.orthographicSize=outsideArea.Zoom;Rig.Tick(0);ReloadTime=HealTime=ExtractTime=0;ResetWorldInteraction(true);Physics.SyncTransforms();Vision.Bind();Vision.Refresh(Player.transform.position,Player.Aim);NoticeTime=0;return true;
 }
}}
