using UnityEngine;
using System.Collections.Generic;
namespace GreyHarbor {public sealed partial class RaidGame {
 bool TrySwitchOfficeFloor2733(string destination){
  bool falling=destination!=null&&destination.StartsWith("office-fall:");
  if(destination==null||(!falling&&!destination.StartsWith("office-floor:")))return false;
  if(Phase!=GamePhase.Raid||!Map.OfficeWhitebox2726||Map.IsOfficeBridge2741||outsideArea==null)return true;
  var parts=destination.Split(':');int floor;Vector3 arrival;
  if(falling){
   if(parts.Length!=4||!int.TryParse(parts[1],out floor)||floor!=Map.OfficeFloor2733-1||floor<1||!float.TryParse(parts[2],System.Globalization.NumberStyles.Float,System.Globalization.CultureInfo.InvariantCulture,out float x)||!float.TryParse(parts[3],System.Globalization.NumberStyles.Float,System.Globalization.CultureInfo.InvariantCulture,out float z))return true;
   arrival=new Vector3(x,0,z);if(Vector3.Distance(arrival,Player.transform.position)>.01f||!Map.OfficeFallHoles2748.Exists(h=>Vector3.Distance(arrival,h.Center)<=h.TriggerRadius+.001f))return true;
  }else{
   if(parts.Length!=3||!int.TryParse(parts[1],out floor)||floor<1||floor>3||(parts[2]!="north"&&parts[2]!="south")||Mathf.Abs(floor-Map.OfficeFloor2733)!=1)return true;
   arrival=RaidMap.OfficeStairArrival2733(parts[2]=="north");
  }
  var current=indoorAreas[Map.InteriorId];current.Yaw=Rig.DestinationYaw;current.Actors=new List<RaidActor>(Enemies);
  CancelArmorRepair();CloseInventory();ClearShots();Enemies.Clear();Player.transform.SetParent(transform,true);Map.Root.gameObject.SetActive(false);
  bool left=Map.OfficeLeft2741;string key=(left?"officeleft":"officeright")+floor;
  if(!indoorAreas.TryGetValue(key,out var target)){
   Map=new RaidMap{HardLoot=ActiveHardMode};Map.BuildOfficeInterior2726(floor,left);Map.FinishInteriorDoors2743();InitializeLootGrids();
   Vision=new RaidVision(Map.Bounds){RoomGate=true};foreach(var area in Map.OfficeInitialAreas2730)Vision.RevealArea(area);
   Navigation=new RaidNavigation(Map.Bounds,.25f);SpawnOfficeSoldiers2747();target=new AreaState{Map=Map,Vision=Vision,Navigation=Navigation,Yaw=current.Yaw,Actors=new List<RaidActor>(Enemies)};indoorAreas.Add(key,target);Enemies.Clear();
  }
  Map=target.Map;Vision=target.Vision;Navigation=target.Navigation;Map.Root.gameObject.SetActive(true);Enemies.AddRange(target.Actors);
  Physics.SyncTransforms();
  if(falling&&!OfficeFallLandingClear2748(arrival)){
   Map.Root.gameObject.SetActive(false);Map=current.Map;Vision=current.Vision;Navigation=current.Navigation;Enemies.Clear();Enemies.AddRange(current.Actors);Map.Root.gameObject.SetActive(true);Player.transform.SetParent(Map.Root,true);Physics.SyncTransforms();Vision.Bind();Vision.Refresh(Player.transform.position,Player.Aim);OfficeFallBlocked2748=true;return true;
  }
  Player.transform.SetParent(Map.Root,true);Player.transform.position=arrival;OfficeFallBlocked2748=false;
  if(falling){var landedRoom=Map.OfficeRooms2726.Find(r=>r.Area.Contains(new Vector2(arrival.x,arrival.z)));if(landedRoom!=null){Vision.RevealArea(Rect.MinMaxRect(landedRoom.Area.xMin-.25f,landedRoom.Area.yMin-.25f,landedRoom.Area.xMax+.25f,landedRoom.Area.yMax+.25f));foreach(var o in Map.OfficeOpenings2726)if(o.Door!=null&&o.Door.RevealRoom==landedRoom.Area)foreach(var area in o.Door.ExtraRevealRooms)Vision.RevealArea(area);}}

  var room=Map.OfficeRooms2726.Find(r=>r.Stair&&r.Area.Contains(new Vector2(Player.transform.position.x,Player.transform.position.z)));
  if(room!=null)Vision.RevealArea(Rect.MinMaxRect(room.Area.xMin-.25f,room.Area.yMin-.25f,room.Area.xMax+.25f,room.Area.yMax+.25f));
  Rig.Target=Player.transform;Rig.ResetView(target.Yaw);View.orthographicSize=outsideArea.Zoom;Rig.Tick(0);ReloadTime=HealTime=ExtractTime=0;ResetWorldInteraction(true);Physics.SyncTransforms();Vision.Bind();Vision.Refresh(Player.transform.position,Player.Aim);
  if(falling)Notify("下落至"+floor+"楼");else NoticeTime=0;return true;
 }
}}
