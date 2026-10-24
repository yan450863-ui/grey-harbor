using UnityEngine;
using System.Collections.Generic;
namespace GreyHarbor {
 public sealed partial class RaidMap {
  public bool IsOfficeBridge2741;
  public void BuildOfficeBridge2741(){
   IsInterior=OfficeWhitebox2726=IsOfficeBridge2741=true;OfficeFloor2733=2;InteriorId="officebridge2";InteriorTitle="办公双子塔 · 二楼连廊";Bounds=new Rect(-5.45f,-1.7f,10.9f,3.4f);InitialRoom=new Rect(-5.25f,-1.5f,10.5f,3);Root=new GameObject("OfficeConnectingBridge2741").transform;Spawn=new Vector3(4.55f,0,0);
   var floor=Box("OfficeBridgeFinishedFloor2742",new Vector3(0,-.14f,0),new Vector3(10.5f,.26f,3),"D1D5CD");floor.AddComponent<BoxCollider>();foreach(float z in new[]{-1.5f,1.5f})Box("OfficeBridgeWhiteboxSide2741",new Vector3(0,1.6f,z),new Vector3(10.5f,3.2f,.2f),"DCE0D3",true);
   foreach(float x in new[]{-5.25f,5.25f}){foreach(float z in new[]{-1.45f,1.45f})Box("OfficeBridgeEndFrame2741",new Vector3(x,1.6f,z),new Vector3(.2f,3.2f,.1f),"647A70",true);Box("OfficeBridgeEndFrame2741",new Vector3(x,2.775f,0),new Vector3(.2f,.85f,2.8f),"B9B99F",true);Box("OfficeBridgeEndDoor2744",new Vector3(x,1.15f,0),new Vector3(.1f,2.3f,2.8f),"7B493D",true);}
   Doors.Add(new WorldDoor{Name="连廊西端 · 进入左塔二楼",Destination="office-link:left:bridge",Portal=true,Position=new Vector3(-4.55f,0,0)});Doors.Add(new WorldDoor{Name="连廊东端 · 进入右塔二楼",Destination="office-link:right:bridge",Portal=true,Position=new Vector3(4.55f,0,0)});BuildOfficeBridgeDressing2742(floor);Physics.SyncTransforms();
  }
  void BuildOfficeBridgePortal2741(){var p=new Vector3(OfficeLeft2741?19:-19.3f,0,12.5f);Doors.Add(new WorldDoor{Name=OfficeLeft2741?"连廊入口 · 前往右塔":"连廊入口 · 前往左塔",Destination="office-link:bridge:"+(OfficeLeft2741?"left":"right"),Portal=true,Position=p});Box("OfficeBridgeEntrancePanel2741",new Vector3(OfficeLeft2741?19.87f:-19.87f,1.15f,12.5f),new Vector3(.045f,2.3f,2.7f),"A6C0BA");}
 }
 public sealed partial class RaidGame {
  bool TrySwitchOfficeLink2741(string destination){
   if(destination==null||!destination.StartsWith("office-link:"))return false;
   if(Phase!=GamePhase.Raid||Map==null||!Map.OfficeWhitebox2726||outsideArea==null)return true;
   var parts=destination.Split(':');if(parts.Length!=3)return true;bool bridge=parts[1]=="bridge",left=parts[1]=="left";
   if(bridge){if(Map.IsOfficeBridge2741||Map.OfficeFloor2733!=2||(parts[2]!="left"&&parts[2]!="right")||(parts[2]=="left")!=Map.OfficeLeft2741)return true;}else if(!Map.IsOfficeBridge2741||parts[2]!="bridge"||(parts[1]!="left"&&parts[1]!="right"))return true;
   var current=indoorAreas[Map.InteriorId];current.Yaw=Rig.DestinationYaw;current.Actors=new List<RaidActor>(Enemies);CancelArmorRepair();CloseInventory();ClearShots();Enemies.Clear();Player.transform.SetParent(transform,true);Map.Root.gameObject.SetActive(false);
   string key=bridge?"officebridge2":left?"officeleft2":"officeright2";
   if(!indoorAreas.TryGetValue(key,out var target)){
    Map=new RaidMap{HardLoot=ActiveHardMode};if(bridge)Map.BuildOfficeBridge2741();else Map.BuildOfficeInterior2726(2,left);Map.FinishInteriorDoors2743();InitializeLootGrids();Vision=new RaidVision(Map.Bounds){RoomGate=true};if(bridge)Vision.RevealArea(Map.InitialRoom);else foreach(var area in Map.OfficeInitialAreas2730)Vision.RevealArea(area);Navigation=new RaidNavigation(Map.Bounds,.25f);if(!bridge)SpawnOfficeSoldiers2747();target=new AreaState{Map=Map,Vision=Vision,Navigation=Navigation,Yaw=current.Yaw,Actors=new List<RaidActor>(Enemies)};indoorAreas.Add(key,target);Enemies.Clear();
   }
   Map=target.Map;Vision=target.Vision;Navigation=target.Navigation;Map.Root.gameObject.SetActive(true);Enemies.AddRange(target.Actors);Player.transform.SetParent(Map.Root,true);Player.transform.position=bridge?new Vector3(parts[2]=="right"?4.55f:-4.55f,0,0):new Vector3(left?19:-19.3f,0,12.5f);Rig.Target=Player.transform;Rig.ResetView(target.Yaw);View.orthographicSize=outsideArea.Zoom;Rig.Tick(0);ReloadTime=HealTime=ExtractTime=0;ResetWorldInteraction(true);Physics.SyncTransforms();Vision.Bind();Vision.Refresh(Player.transform.position,Player.Aim);NoticeTime=0;return true;
  }
 }
}
