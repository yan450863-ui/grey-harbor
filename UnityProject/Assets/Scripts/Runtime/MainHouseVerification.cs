using UnityEngine;
using System.Collections;
namespace GreyHarbor {
 public sealed partial class RaidVerification {
  IEnumerator VerifyMainHouse113(){
   FreshCamp();G.CloseInventory();string saved=JsonUtility.ToJson(G.Save);G.NextMap=RaidDestination.Training;G.StartRaid();G.Paused=true;yield return null;
   var outdoor=G.Map;float zoom=G.View.orthographicSize;G.Rig.ResetView(135);G.Rig.Tick(0);float unit=ScreenUnit112();
   Check(G.Map.Doors.FindAll(d=>d.Portal&&d.Destination=="mainhouse").Count==1&&G.Map.ClosedEntrances.Count==1,"Main house opens only A1; A2 remains sealed");
   Check(G.Navigation.FindPath(G.Map.Spawn,RaidMap.MainHouseApproach).Count>0,"Main house entry reachable from outdoor spawn");
   G.Player.transform.position=RaidMap.MainHouseApproach;Physics.SyncTransforms();F111(false,0);F111(true,1.9f);Check(G.Map==outdoor&&G.NearbyDoor.Destination=="mainhouse","Main house entry waits for full two-second hold");
   G.Controls=new TouchInput{Searching=true,Shooting=true};G.TickInteraction(.2f);Check(G.Map==outdoor&&G.DoorHold==0,"Firing interrupts main house entry");F111(true,3);Check(G.Map==outdoor,"Main house cannot enter after firing until F released");F111(false,0);F111(true,2.01f);G.Paused=true;yield return null;
   var house=G.Map;VerifyFloor115(house);VerifyGroundPlacement117(house);Check(house.Root.Find("WorkshopWastelandArt_114")==null,"Main house does not receive workshop-specific geometry");var doors=house.Doors.FindAll(d=>!d.Portal);
   Check(house.InteriorId=="mainhouse"&&doors.Count==6&&house.Doors.Count==7,"Main house contains six separately gated rooms and one exterior exit");
   var floor=house.Root.Find("MainHouseFloor").GetComponent<Renderer>().bounds;float area=(floor.size.x-.1f)*(floor.size.z-.1f);Check(Mathf.Abs(area-480)<.1f&&area>126*3.5f,"Main house is 480 square metres over 3.5 times repair workshop");
   Check(Mathf.Abs(G.View.orthographicSize-zoom)<.001f&&Mathf.Abs(ScreenUnit112()-unit)<.1f,"Main house retains outdoor character and furniture projection scale");
   Check(house.Loot.Count==20&&house.Loot.TrueForAll(l=>l.Type!=SupplyType.Safe),"Twenty sparse purpose-placed containers; no additional safe");
   Check(G.Vision.Explored(house.Spawn)&&G.Vision.Explored(new Vector3(0,0,8.8f)),"Main corridor grey-known from entrance to far end");
   bool unknown=true;foreach(var room in RaidMap.MainHouseRooms)unknown&=!G.Vision.Explored(new Vector3(room.center.x,0,room.center.y));Check(unknown,"All six rooms start completely unexplored");
   for(int i=0;i<6;i++){
    var d=doors[i];var room=RaidMap.MainHouseRooms[i];var approach=RaidMap.MainHouseDoorApproach(i);var inside=new Vector3(i%2==0?-3.2f:3.2f,0,approach.z);G.Player.transform.position=approach;Physics.SyncTransforms();
    Check(!G.ClearSight(approach,inside)&&Physics.Linecast(approach+Vector3.up*RaidGame.CombatHeight,inside+Vector3.up*RaidGame.CombatHeight,SceneObstacle.SightMask),"Closed main house room blocks sight and shot path: "+i);
    Check(G.Navigation.FindPath(approach,inside).Count==0,"Closed room door blocks walking route: "+i);
    F111(false,0);F111(true,.01f);Check(d.Open&&G.Vision.Explored(new Vector3(room.center.x,0,room.center.y)),"F opens and reveals only selected room: "+i);
    bool others=true;for(int j=i+1;j<6;j++){var r=RaidMap.MainHouseRooms[j];others&=!G.Vision.Explored(new Vector3(r.center.x,0,r.center.y));}Check(others,"Opening room does not reveal unopened adjacent rooms: "+i);
    Check(G.ClearSight(approach,inside)&&G.Navigation.FindPath(house.Spawn,inside).Count>0,"Opened room doorway clears sight and enables route: "+i);
    F111(false,0);F111(true,.01f);Check(!d.Open&&G.Vision.Explored(new Vector3(room.center.x,0,room.center.y))&&!G.ClearSight(approach,inside),"Closing room retains grey memory and restores blocking: "+i);
    F111(false,0);F111(true,.01f);
   }
   foreach(var l in house.Loot){
    var path=G.Navigation.FindPath(house.Spawn,l.Position);Check(path.Count>0&&Vector3.Distance(path[path.Count-1],l.Position)<.01f,"Main house container reachable at exact standing point: "+l.Position);
    G.Player.transform.position=l.Position;Physics.SyncTransforms();F111(false,0);F111(true,l.SearchDuration+.1f);Check(G.OpenLoot==l&&l.Opened,"Main house container searchable: "+l.Name+" "+l.Position);G.CloseInventory();
   }
   var loot=house.Loot[0];G.Player.transform.position=loot.Position;G.OpenLoot=loot;G.InventoryOpen=true;G.Paused=false;G.TickIdentification(2.1f);var item=loot.Items[0];Check(item.Identified&&G.QuickTransfer(ItemArea.Loot,item.Cell),"Main house loot identifies and transfers normally");G.CloseInventory();G.Paused=true;int remaining=loot.Items.Count;
   for(int i=0;i<6;i++){var r=RaidMap.MainHouseRooms[i];G.Player.transform.position=new Vector3(r.center.x,0,r.center.y);G.Player.Aim=Vector3.forward;G.Rig.ResetView(i%2==0?225:45);G.Rig.Tick(0);G.Vision.Refresh(G.Player.transform.position,G.Player.Aim);yield return Capture("130-mainhouse-room-"+i+".png");}
   G.Player.transform.position=Vector3.zero;G.Player.Aim=Vector3.forward;for(int i=0;i<4;i++){G.Rig.ResetView(45+90*i);G.Rig.Tick(0);G.View.orthographicSize=17;G.Vision.Refresh(G.Player.transform.position,G.Player.Aim);yield return Capture("131-mainhouse-overview-"+i+".png");}G.View.orthographicSize=zoom;G.Rig.ResetView(315);
   G.Player.transform.position=house.Spawn;F111(false,0);F111(true,2.01f);G.Paused=true;yield return null;Check(G.Map==outdoor&&Mathf.Abs(Mathf.DeltaAngle(G.Rig.Yaw,135))<.1f&&Vector3.Distance(G.Player.transform.position,RaidMap.MainHouseApproach)<.01f,"Main house exit restores original yard position and view");
   G.Player.transform.position=RaidMap.RepairApproach;F111(false,0);F111(true,2.01f);G.Paused=true;yield return null;var repair=G.Map;Check(repair.InteriorId=="repair"&&repair.Loot.Count==6&&!G.Vision.Explored(new Vector3(0,0,4)),"Workshop layout and exploration remain independent of main house");G.Player.transform.position=repair.Spawn;F111(false,0);F111(true,2.01f);G.Paused=true;yield return null;
   G.Player.transform.position=RaidMap.MainHouseApproach;F111(false,0);F111(true,2.01f);G.Paused=true;yield return null;Check(G.Map==house&&doors.TrueForAll(d=>d.Open)&&loot.Items.Count==remaining&&Mathf.Abs(Mathf.DeltaAngle(G.Rig.Yaw,315))<.1f,"Main house keeps six door states loot and its own view on reentry");Check(!outdoor.Root.gameObject.activeInHierarchy&&!repair.Root.gameObject.activeInHierarchy,"Inactive yard and workshop cannot interfere with main house physics");
   G.Controls=new DesktopInput();G.Finish(false);yield return null;Check(G.Phase==GamePhase.Base&&JsonUtility.ToJson(G.Save)==saved&&!house.Root&&!repair.Root&&!outdoor.Root,"Training finish clears main house and restores exact formal inventory");
  }
 }
}
