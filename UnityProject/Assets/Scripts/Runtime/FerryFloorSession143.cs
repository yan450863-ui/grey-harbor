using UnityEngine;
using System.Collections.Generic;
namespace GreyHarbor {
 public sealed partial class RaidGame {
  bool TrySwitchFerryFloor143(string destination){
   if(destination==null||!destination.StartsWith("ferry-floor:"))return false;
   if(Phase!=GamePhase.Raid||!Map.FerryWhitebox143||outsideArea==null)return true;
   string[] parts=destination.Split(':');if(parts.Length!=3||(parts[1]!="1"&&parts[1]!="2")||(parts[2]!="front"&&parts[2]!="rear"))return true;string key="ferry"+parts[1];if(key==Map.InteriorId)return true;
   var current=indoorAreas[Map.InteriorId];current.Yaw=Rig.DestinationYaw;current.Actors=new List<RaidActor>(Enemies);
   foreach(var e in Enemies){e.Windup=e.Lunge=e.AttackPose=0;e.RebelBurst=0;e.AttackCooldown=Mathf.Max(e.AttackCooldown,1);}
   CancelArmorRepair();CloseInventory();ClearShots();Enemies.Clear();Player.transform.SetParent(transform,true);Map.Root.gameObject.SetActive(false);
   if(!indoorAreas.TryGetValue(key,out var target)){
    Map=new RaidMap{HardLoot=ActiveHardMode};Map.BuildFerryInterior143(key);Map.FinishInteriorDoors2743();Vision=new RaidVision(Map.Bounds){RoomGate=true};Vision.RevealArea(Map.InitialRoom);Vision.RevealArea(RaidMap.FerryLobby143);Navigation=new RaidNavigation(Map.Bounds,.25f);PopulateInteriorMonsters();target=new AreaState{Map=Map,Vision=Vision,Navigation=Navigation,Yaw=current.Yaw,Actors=new List<RaidActor>(Enemies)};indoorAreas.Add(key,target);
   }
   Map=target.Map;Vision=target.Vision;Navigation=target.Navigation;Map.Root.gameObject.SetActive(true);Enemies.Clear();Enemies.AddRange(target.Actors);Player.transform.SetParent(Map.Root,true);Player.transform.position=RaidMap.FerryStairArrival143(parts[2]=="front");
   Vector2 arrival=new Vector2(Player.transform.position.x,Player.transform.position.z);foreach(var room in Map.FerryRooms143)if(room.Contains(arrival)){Vision.RevealArea(room.Area);foreach(var area in room.ExtraAreas)Vision.RevealArea(area);}
   Rig.Target=Player.transform;Rig.ResetView(target.Yaw);View.orthographicSize=outsideArea.Zoom;Rig.Tick(0);ReloadTime=HealTime=ExtractTime=0;ResetWorldInteraction(true);Physics.SyncTransforms();Vision.Bind();Vision.Refresh(Player.transform.position,Player.Aim);NoticeTime=0;return true;
  }
 }
}
