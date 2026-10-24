using UnityEngine;
using System.Collections.Generic;
namespace GreyHarbor {
 public sealed partial class RaidGame {
  sealed class AreaState {public RaidMap Map;public RaidVision Vision;public RaidNavigation Navigation;public float Yaw,Zoom;public List<RaidActor> Actors=new List<RaidActor>();}
  AreaState outsideArea;readonly Dictionary<string,AreaState> indoorAreas=new Dictionary<string,AreaState>();Vector3 outsideReturn;
  public bool InsideBooth=>Map!=null&&Map.IsInterior;
  void ClearInteriorSession(){
   var all=new List<AreaState>(indoorAreas.Values);if(outsideArea!=null)all.Add(outsideArea);
   foreach(var a in all)if(a.Map!=Map){a.Vision?.Dispose();if(a.Map.Root){a.Map.Root.gameObject.SetActive(false);Destroy(a.Map.Root.gameObject);}}
   outsideArea=null;indoorAreas.Clear();ResetWorldInteraction(false);
  }
  internal void SwitchBooth()=>SwitchInterior("booth");
  internal void SwitchInterior(string destination){
   if(TrySwitchUnderground2752(destination))return;
   if(TrySwitchOfficeLink2741(destination))return;
   if(TrySwitchOfficeFloor2733(destination))return;
   if(TrySwitchRepairFloor220(destination))return;
   int mainEntry=-1;if(destination!=null&&destination.StartsWith("repair-entry:")){mainEntry=System.Array.IndexOf(RaidMap.RepairMainGateKeys220,destination.Substring(13));if(mainEntry<0||Map.IsInterior)return;destination="repairmain1";}
   if(TrySwitchFerryFloor143(destination))return;
   if(Phase!=GamePhase.Raid||(!TrainingActive&&!Map.IsHarbor&&!Map.IsInterior))return;
   bool entering=!Map.IsInterior;string key=entering?destination:Map.InteriorId;
   if(entering&&key!="booth"&&key!="repair"&&key!="mainhouse"&&key!="portbooth"&&key!="portservice"&&key!="yardhome"&&key!="yardshed"&&key!="ferry1"&&key!="repairannex"&&key!="repairmain1"&&!RaidMap.IsFreightInteriorKey2716(key)&&!RaidMap.IsUnloadingInteriorKey2723(key)&&key!="officeright1")return;
   if(entering){outsideReturn=key=="officeright1"?RaidMap.OfficeArrival2726:key=="unloadingwarehouse"?RaidMap.UnloadingWarehouseArrival2723:key=="unloadingduty"?RaidMap.UnloadingDutyArrival2723:key=="freightduty"?RaidMap.FreightDutyArrival2716:key=="freightwarehouse"?RaidMap.FreightWarehouseArrival2716:Player.transform.position;outsideArea=new AreaState{Map=Map,Vision=Vision,Navigation=Navigation,Yaw=Rig.DestinationYaw,Zoom=View.orthographicSize,Actors=new List<RaidActor>(Enemies)};}
   else {indoorAreas[key].Yaw=Rig.DestinationYaw;indoorAreas[key].Actors=new List<RaidActor>(Enemies);}
   foreach(var e in Enemies){e.Windup=e.Lunge=e.AttackPose=0;e.RebelBurst=0;e.AttackCooldown=Mathf.Max(e.AttackCooldown,1);}
   CancelArmorRepair();CloseInventory();ClearShots();Enemies.Clear();
   Player.transform.SetParent(transform,true);Map.Root.gameObject.SetActive(false);
   if(entering&&!indoorAreas.ContainsKey(key)){
    Map=new RaidMap{HardLoot=ActiveHardMode};if(key=="officeright1")Map.BuildOfficeInterior2726();else if(RaidMap.IsUnloadingInteriorKey2723(key))Map.BuildUnloadingInterior2723(key);else if(RaidMap.IsFreightInteriorKey2716(key))Map.BuildFreightInterior2716(key);else if(key=="repairmain1")Map.BuildRepairMain210();else if(key=="repairannex")Map.BuildRepairAnnex200();else if(key=="ferry1")Map.BuildFerryInterior143(key);else if(key=="yardhome"||key=="yardshed")Map.BuildYardInterior138(key);else if(key=="portbooth"||key=="portservice")Map.BuildPortInterior(key);else if(key=="mainhouse")Map.BuildMainHouseInterior();else if(key=="repair")Map.BuildRepairInterior();else Map.BuildBoothInterior();Map.FinishInteriorDoors2743();InitializeLootGrids();
    Vision=new RaidVision(Map.Bounds);Vision.RoomGate=true;Vision.RevealArea(Map.InitialRoom);if(Map.YardInterior138)foreach(var room in Map.YardRooms138)if(room.Door.Open)Vision.RevealArea(room.Area);if(key=="repairmain1")Vision.RevealArea(RaidMap.MainRect273(new Rect(9.7f,-15.79f,3.59f,4.29f)));if(Map.FerryWhitebox143)Vision.RevealArea(RaidMap.FerryLobby143);
    if(Map.OfficeWhitebox2726)foreach(var area in Map.OfficeInitialAreas2730)Vision.RevealArea(area);
    if(Map.FreightInterior2716)foreach(var area in Map.FreightInitialAreas2716)Vision.RevealArea(area);if(Map.UnloadingInterior2723)foreach(var area in Map.InitialAreas2723)Vision.RevealArea(area);
    Navigation=new RaidNavigation(Map.Bounds,.25f);if(Map.OfficeWhitebox2726)SpawnOfficeSoldiers2747();else if(Map.FreightInterior2716)PopulateFreightEnemies2720();else if(Map.UnloadingInterior2723)PopulateUnloadingEnemies2725();else PopulateInteriorMonsters();indoorAreas.Add(key,new AreaState{Map=Map,Vision=Vision,Navigation=Navigation,Yaw=outsideArea.Yaw,Actors=new List<RaidActor>(Enemies)});
   }
   Enemies.Clear();var target=entering?indoorAreas[key]:outsideArea;Map=target.Map;Vision=target.Vision;Navigation=target.Navigation;Enemies.AddRange(target.Actors);
   Map.Root.gameObject.SetActive(true);Player.transform.SetParent(Map.Root,true);Player.transform.position=entering?(mainEntry>=0?RaidMap.RepairMainEntryArrival220(mainEntry):Map.Spawn):outsideReturn;
   if(entering&&Map.RepairMain220)RevealRepairArrival220();
   // Keep the outdoor projection size: no character or environment magnification indoors.
   Rig.Target=Player.transform;Rig.ResetView(target.Yaw);View.orthographicSize=outsideArea.Zoom;Rig.Tick(0);
   ReloadTime=HealTime=ExtractTime=0;ResetWorldInteraction(true);Physics.SyncTransforms();Vision.Bind();Vision.Refresh(Player.transform.position,Player.Aim);
   NoticeTime=0;
  }
  internal static bool DoorFootprintOccupied222(Rect area,Vector2 p,float radius){float dx=p.x-Mathf.Clamp(p.x,area.xMin,area.xMax),dz=p.y-Mathf.Clamp(p.y,area.yMin,area.yMax);return dx*dx+dz*dz<radius*radius;}
  internal bool ToggleRoomDoor(WorldDoor door){
   if(door==null||door.Portal||door.Pivot==null)return false;
   bool keyLocked=door.Locked&&KeyRules2760.IsKey(door.KeyKind2760);
   if(door.Locked&&!keyLocked){Notify(door.Name.Contains("楼梯")?"楼梯封闭，上层暂未开放。":"房门已上锁，暂未开放。");return false;}
   if(keyLocked&&FindCarriedKey2760(door.KeyKind2760,out _) == null){Notify("需要随身携带："+RaidInventory.Name(door.KeyKind2760));return false;}
   Vector2 p=new Vector2(Player.transform.position.x,Player.transform.position.z);
   if(!door.Open&&!door.InstantToggle&&door.OpenSafety.Contains(p)){Notify("请让开门扇再开门。");return false;}
   if(MonsterBlocksDoor(door)){Notify("怪物挡住了门，请等它离开门扇范围。");return false;}
   if(door.Open&&!ClearPlayerForDoorClose2729(door))return false;
   if(keyLocked&&!SpendDoorKey2760(door))return false;
   door.Open=!door.Open;door.Pivot.localRotation=Quaternion.Euler(0,door.ClosedYaw+(door.Open?door.OpenAngle:0),0);Map.LayoutRevision++;
   if(door.Open){Vision.RevealArea(door.RevealRoom);foreach(var area in door.ExtraRevealRooms)Vision.RevealArea(area);}
   Physics.SyncTransforms();Navigation=Map.UndergroundWhitebox2752?UndergroundNavigation2752(Map):new RaidNavigation(Map.Bounds,.25f);if(indoorAreas.TryGetValue(Map.InteriorId,out var current))current.Navigation=Navigation;
   foreach(var e in Enemies){e.Route.Clear();e.RepathTime=e.StuckTime=0;}
   Vision.Refresh(Player.transform.position,Player.Aim);return true;
  }
 }
}


