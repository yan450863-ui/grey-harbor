using UnityEngine;
using System.Collections;
namespace GreyHarbor {
 public sealed partial class RaidVerification {
  void F111(bool down,float dt){G.Controls=new TouchInput{Searching=down};G.TickInteraction(dt);}
  IEnumerator VerifyInterior111(){
   FreshCamp();G.CloseInventory();string saved=JsonUtility.ToJson(G.Save);G.NextMap=RaidDestination.Training;G.StartRaid();G.Paused=true;yield return null;
   Check(DesktopInput.TurnDirection(true,false)==1&&DesktopInput.TurnDirection(false,true)==-1,"Q/E desktop directions exchanged");
   var tool=G.Map.Loot.Find(l=>l.Type==SupplyType.Tools);G.Player.transform.position=new Vector3(-12.25f,0,-5.9f);G.Player.Aim=Vector3.back;G.Vision.Refresh(G.Player.transform.position,G.Player.Aim);Physics.SyncTransforms();
   Check(!G.Vision.CanSee(G.LootBounds(tool).center)&&G.CanInteractLoot(tool),"Close container remains reachable outside flashlight cone");
   F111(false,0);F111(true,.4f);Check(G.Nearby==tool&&tool.Search>.3f,"Contact-edge search starts at close range");
   var competing=new WorldDoor{Name="Test adjacent portal",Portal=true,Position=G.Player.transform.position+Vector3.right*.1f};G.Map.Doors.Add(competing);F111(true,.2f);Check(G.Nearby==tool&&G.NearbyDoor==null&&tool.Search>.5f,"Held search locks target even when a nearer door becomes eligible");G.Map.Doors.Remove(competing);
   F111(false,0);Check(tool.Search==0,"Releasing F resets incomplete container search");F111(true,tool.SearchDuration+.1f);Check(G.OpenLoot==tool&&G.InventoryOpen,"Flashlight-independent search opens real tool container");
   G.Player.Aim=Vector3.forward;G.Vision.Refresh(G.Player.transform.position,G.Player.Aim);F111(false,.01f);Check(G.InventoryOpen,"Turning mouse does not close opened loot UI");G.CloseInventory();
   var outdoor=G.Map;var outdoorVision=G.Vision;var originalActor=G.Player;var bag=G.Bag;G.Player.Health=73;G.Magazine=17;
   Check(G.Navigation.FindPath(G.Map.Spawn,RaidMap.BoothApproach).Count>0,"Booth exterior approach reachable from spawn");
   G.Player.transform.position=RaidMap.BoothApproach;G.Rig.ResetView(135);G.Rig.Tick(0);Physics.SyncTransforms();F111(false,0);F111(true,1.99f);Check(!G.InsideBooth&&G.DoorHold>1.9f,"Entry requires full two seconds");
   F111(false,0);Check(G.DoorHold==0,"Releasing entry key clears portal progress");F111(true,1);G.Player.transform.position+=Vector3.forward*3;F111(true,.1f);Check(!G.InsideBooth&&G.DoorHold==0,"Leaving entry range cancels portal progress");
   G.Player.transform.position=RaidMap.BoothApproach;F111(false,0);F111(true,2.01f);G.Paused=true;yield return null;
   VerifyFloor115(G.Map);
   Check(G.InsideBooth&&!outdoor.Root.gameObject.activeInHierarchy&&G.Map.Root.gameObject.activeInHierarchy,"Transition isolates indoor and outdoor roots");
   Check(G.Player==originalActor&&G.Bag==bag&&G.Player.Health==73&&G.Magazine==17,"Transition preserves player health ammo and carried inventory");
   Check(Mathf.Abs(Mathf.DeltaAngle(G.Rig.Yaw,135))<.1f,"First indoor visit inherits pre-entry view");
   Check(G.Map.Loot.Count==6&&G.Map.Loot.TrueForAll(l=>l.Type!=SupplyType.Safe),"Interior has six dense loot points without extra safe");
   var inside=G.Map;var insideVision=G.Vision;var room=G.Map.Doors.Find(d=>!d.Portal);
   Check(G.Vision.Explored(new Vector3(-1,0,1))&&!G.Vision.Explored(new Vector3(2.5f,0,1)),"Main room grey-known while unopened storage stays black");
   Check(!G.ClearSight(new Vector3(0,0,.3f),new Vector3(2,0,.3f)),"Closed room door blocks sight and projectile collision mask");
   Check(Shader.GetGlobalTexture("_HarborVisibility")==G.Vision.Texture&&G.Map.Bounds.width==8,"Interior switches visibility texture and minimap bounds");
   F111(true,3);Check(G.InsideBooth&&G.DoorHold==0,"Held entry key cannot immediately trigger exit");F111(false,0);
   G.Player.transform.position=new Vector3(0,0,.3f);Physics.SyncTransforms();F111(true,.01f);Check(room.Open&&G.Vision.Explored(new Vector3(2.5f,0,1)),"Single F opens room door and reveals whole storage as grey memory");
   Check(G.ClearSight(new Vector3(0,0,.3f),new Vector3(2,0,.3f)),"Opened room doorway allows sight through gap");
   Check(G.Navigation.FindPath(G.Map.Spawn,new Vector3(2.4f,0,1.4f)).Count>0,"Opened door makes storage reachable");
   G.Player.transform.position=new Vector3(1,0,.3f);Check(!G.ToggleRoomDoor(room)&&room.Open,"Door refuses to close onto player");
   G.Player.transform.position=new Vector3(0,0,.3f);Check(G.ToggleRoomDoor(room)&&!room.Open&&G.Vision.Explored(new Vector3(2.5f,0,1)),"Closing door retains discovered storage memory");G.ToggleRoomDoor(room);
   foreach(var l in G.Map.Loot)Check(G.Navigation.FindPath(G.Map.Spawn,l.Position).Count>0,"Indoor search position accessible: "+l.Name+" "+l.Position);
   var file=G.Map.Loot.Find(l=>l.Prop.Look==ContainerLook.FileCabinet);G.Player.transform.position=file.Position;Physics.SyncTransforms();F111(false,0);F111(true,file.SearchDuration+.1f);Check(G.OpenLoot==file&&file.Opened,"Indoor file cabinet uses shared search interaction");G.Paused=false;G.TickIdentification(2.1f);var item=file.Items[0];Check(item.Identified&&G.QuickTransfer(ItemArea.Loot,item.Cell),"Indoor identified loot transfers to carried inventory");G.Paused=true;G.CloseInventory();int remain=file.Items.Count;
   G.Player.transform.position=new Vector3(-1.25f,0,-1.4f);G.Player.Aim=Vector3.forward;G.Rig.ResetView(225);G.Rig.Tick(0);G.Vision.Refresh(G.Player.transform.position,G.Player.Aim);yield return Capture("111-interior-open.png");
   for(int n=0;n<4;n++){G.Rig.ResetView(45+n*90);G.Rig.Tick(0);yield return Capture("111-interior-view-"+n+".png");}G.Rig.ResetView(225);
   G.Player.transform.position=G.Map.Spawn;Physics.SyncTransforms();F111(false,0);F111(true,1.99f);Check(G.InsideBooth,"Exit also requires two seconds");F111(true,.02f);G.Paused=true;yield return null;
   Check(G.Map==outdoor&&G.Vision==outdoorVision&&Mathf.Abs(Mathf.DeltaAngle(G.Rig.Yaw,135))<.1f,"Exit restores same exterior and pre-entry camera view");
   Check(Shader.GetGlobalTexture("_HarborVisibility")==outdoorVision.Texture&&G.Map.Bounds.width==38,"Exterior minimap bounds and fog texture restored");
   F111(false,0);F111(true,2.01f);G.Paused=true;yield return null;
   Check(G.Map==inside&&G.Vision==insideVision&&Mathf.Abs(Mathf.DeltaAngle(G.Rig.Yaw,225))<.1f,"Re-entry restores previous indoor view and area instance");
   Check(file.Opened&&file.Items.Count==remain&&room.Open,"Search contents and door state persist across indoor re-entry");
   G.Player.transform.position=G.Map.Spawn;G.TickExtraction(6);Check(G.InsideBooth&&G.Phase==GamePhase.Raid,"Indoor entrance never counts as extraction");
   G.Controls=new DesktopInput();G.Finish(false);yield return null;Check(G.Phase==GamePhase.Base&&!G.TrainingActive&&JsonUtility.ToJson(G.Save)==saved,"End training from indoors restores exact formal inventory");
   Check(!outdoor.Root&&!inside.Root,"Ending indoor session disposes both map roots");
   G.NextMap=RaidDestination.Training;G.StartRaid();G.Paused=true;G.Player.transform.position=RaidMap.BoothApproach;F111(false,0);F111(true,2.01f);G.Paused=true;yield return null;G.Rig.ResetView(45);G.Rig.Tick(0);G.Player.Aim=Vector3.forward;G.Vision.Refresh(G.Player.transform.position,G.Player.Aim);yield return Capture("110-interior-closed.png");G.Controls=new DesktopInput();G.Finish(false);
  }
 }
}
