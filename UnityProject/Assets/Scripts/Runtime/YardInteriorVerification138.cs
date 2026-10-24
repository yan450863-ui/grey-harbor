using UnityEngine;
using System.Collections;
using System.IO;
namespace GreyHarbor {
 public sealed partial class RaidVerification {
  IEnumerator VerifyYardInterior138(){
   
   FreshCamp();G.HarborWalkthrough=true;G.BeginTraining();G.Paused=true;yield return null;
   var outside=G.Map;float zoom=G.View.orthographicSize;
   var outdoorProps=outside.Root.GetComponentsInChildren<YardAssetInfo>();
   foreach(var id in new[]{"PottingBench","RainBarrels","GardenCart","FirewoodStack"})
    Check(System.Array.Exists(outdoorProps,p=>p.Asset==id),"139 New yard domestic furnishing "+id);
   foreach(var spec in new[]{"yardhome","yardshed"}){
    bool home=spec=="yardhome";Vector3 approach=home?RaidMap.YardHomeApproach138:RaidMap.YardShedApproach138;
    Check(G.Navigation.FindPath(outside.Spawn,approach).Count>0,"139 Exterior house door approach "+spec);
    G.Player.transform.position=approach;G.Player.Health=73;G.Magazine=17;G.Rig.ResetView(135);G.Rig.Tick(0);Physics.SyncTransforms();
    F111(false,0);F111(true,1);
    G.Controls=new TouchInput{Searching=true,Shooting=true};G.TickInteraction(.2f);
    Check(G.Map==outside&&G.DoorHold==0,"139 Firing cancels house-entry countdown "+spec);
    F111(false,0);F111(true,1.99f);Check(G.Map==outside&&G.DoorHold>1.9f,"139 Entry requires two-second hold "+spec);
    F111(true,.02f);G.Paused=true;yield return null;
    var inside=G.Map;Check(inside.InteriorId==spec&&inside.IsInterior,"139 Separate interior loaded "+spec);
    Check(!outside.Root.gameObject.activeInHierarchy&&inside.Root.gameObject.activeInHierarchy,"139 Exterior and interior isolated "+spec);
    Check(G.Player.Health==73&&G.Magazine==17,"139 Health and ammunition survive entry "+spec);
    Check(Mathf.Abs(G.View.orthographicSize-zoom)<.001f&&Mathf.Abs(Mathf.DeltaAngle(G.Rig.Yaw,135))<.1f,"139 Character scale and entry angle preserved "+spec);
    Check(inside.Loot.Count==(home?6:4)&&G.Enemies.Count==(home?3:2),"140 Planned domestic loot and fixed indoor enemy counts "+spec);
    Check(Mathf.Abs(inside.Bounds.width-(home?14.3f:10.3f))<.01f&&Mathf.Abs(inside.Bounds.height-(home?12.3f:10.3f))<.01f,"139 Expanded floor dimensions "+spec);
    Check(inside.Loot.TrueForAll(l=>l.Tier==ContainerTier.Common&&l.Type!=SupplyType.Safe),"139 Domestic rooms retain common quality table "+spec);
    Check(inside.Loot.TrueForAll(l=>Mathf.Abs(l.Position.x)>2),"139 Central corridor kept clear of search points "+spec);
    Check(inside.YardRooms138.Count==(home?5:3),"139 Correct room layout "+spec);
    Check(inside.StableGround137&&inside.StableGround137.mipmapCount>1,"139 New indoor ground uses stable paint "+spec);
    Check(G.Vision.RoomGate&&G.Vision.Explored(inside.Spawn)&&Shader.GetGlobalTexture("_HarborVisibility")==G.Vision.Texture,"139 Fog and minimap bind indoor region "+spec);
    G.Rig.ResetView(45);G.Rig.Tick(0);G.Player.Aim=Vector3.forward;G.Vision.Refresh(G.Player.transform.position,G.Player.Aim);yield return Capture("139-"+spec+"-entry.png");
    foreach(var room in inside.YardRooms138){
     var center=new Vector3(room.Area.center.x,0,room.Area.center.y);
     Check(!inside.Loot.Exists(l=>room.Area.Contains(new Vector2(G.LootBounds(l).center.x,G.LootBounds(l).center.z))&&G.CanInteractLoot(l)),"139 Closed room does not expose loot "+room.Name);
     Check(!G.Vision.Explored(center),"139 Unopened room starts black "+room.Name);
     G.Player.transform.position=new Vector3(room.Door.Position.x*.25f,0,room.Door.Position.z);Physics.SyncTransforms();F111(false,0);F111(true,.01f);
     if(room.Door.Locked){
      Check(!room.Door.Open&&!G.Vision.Explored(center),"139 Key room remains locked and unrevealed");
      Check(G.Navigation.FindPath(G.Player.transform.position,center).Count==0,"139 Key room cannot be entered");
      Check(!inside.Loot.Exists(l=>room.Area.Contains(new Vector2(l.Prop.transform.position.x,l.Prop.transform.position.z))),"139 Key room contains no loot");yield return Capture("139-key-room-locked.png");continue;
     }
     var dc=(BoxCollider)room.Door.Collider;bool furnitureClear=true;foreach(var c in Physics.OverlapBox(dc.transform.TransformPoint(dc.center),Vector3.Scale(dc.size,dc.transform.lossyScale)*.48f,dc.transform.rotation,SceneObstacle.MovementMask))if(c!=dc&&c.name!="InteriorWall"&&c.name!="Skirting")furnitureClear=false;
     Check(furnitureClear,"139 Open room door clears static furniture "+room.Name);
     Check(room.Door.Open&&G.Vision.Explored(center),"139 Opening reveals ordinary room "+room.Name);
     var path=G.Navigation.FindPath(G.Player.transform.position,room.WalkPoint);Check(path.Count>0,"139 Furnished room remains reachable "+room.Name);
     bool walked=true;foreach(var p in path){int n=0;while(Vector3.Distance(G.Player.transform.position,p)>.08f&&n++<1000){G.Player.Move(Vector3.ClampMagnitude(p-G.Player.transform.position,.09f));Physics.SyncTransforms();}if(n>=1000)walked=false;}
     Check(walked&&Vector3.Distance(G.Player.transform.position,room.WalkPoint)<.2f,"139 Player actually walks through room door "+room.Name);
     G.Player.Aim=Vector3.forward;G.Rig.Tick(0);G.Vision.Refresh(G.Player.transform.position,G.Player.Aim);yield return Capture("139-"+spec+"-room-"+inside.YardRooms138.IndexOf(room)+".png");
    }
    foreach(var loot in inside.Loot){
     G.CloseInventory();var route=G.Navigation.FindPath(G.Player.transform.position,loot.Position);if(route.Count==0){foreach(var pos in new[]{G.Player.transform.position,loot.Position}){string hits="";foreach(var c in Physics.OverlapCapsule(pos+Vector3.up*.35f,pos+Vector3.up,.34f,SceneObstacle.MovementMask))hits+=c.name+" "+c.bounds+"; ";Debug.Log("139 BLOCKED ROUTE "+pos+" hits "+hits);} }Check(route.Count>0,"139 Search standing point reachable "+loot.Name);
     bool reached=true;foreach(var p in route){int n=0;while(Vector3.Distance(G.Player.transform.position,p)>.08f&&n++<1000){G.Player.Move(Vector3.ClampMagnitude(p-G.Player.transform.position,.09f));Physics.SyncTransforms();}if(n>=1000)reached=false;}
     Check(reached&&Vector3.Distance(G.Player.transform.position,loot.Position)<.2f,"139 Walk to container without clipping "+loot.Name);
     Check(G.CanInteractLoot(loot),"139 Container search unobstructed "+loot.Name);
     var pivot=loot.Prop.Opening;Vector3 offset=pivot?pivot.localPosition:Vector3.zero;F111(false,0);F111(true,loot.SearchDuration+.02f);
     Check(G.OpenLoot==loot&&loot.Opened&&G.InventoryOpen,"139 F opens expected container "+loot.Name);
     Check(loot.Prop.Searched&&(!pivot||pivot.localRotation!=Quaternion.identity||pivot.localPosition!=offset),"139 Container open state visible "+loot.Name);
     G.Paused=false;for(int identify=0;identify<6;identify++)G.TickIdentification(3);G.Paused=true;Check(loot.Items.TrueForAll(i=>i.Identified),"139 Sequential identification completes "+loot.Name);G.CloseInventory();
     Check(loot.Items.Count>=1&&loot.Items.Count<=6&&loot.Items.TrueForAll(i=>RaidInventory.Grade(i.Kind)<4),"139 Common loot size and no red "+loot.Name);
    }
    yield return YardIndoorOverview138(home?"139-home-plan.png":"139-shed-plan.png",true);
    yield return YardIndoorOverview138(home?"139-home-overview.png":"139-shed-overview.png",false);
    G.Rig.ResetView(225);G.Player.transform.position=inside.Spawn;Physics.SyncTransforms();F111(false,0);F111(true,2.01f);G.Paused=true;yield return null;
    Check(G.Map==outside&&Vector3.Distance(G.Player.transform.position,approach)<.03f,"139 Exit restores exterior return point "+spec);
    Check(Mathf.Abs(Mathf.DeltaAngle(G.Rig.Yaw,135))<.1f&&Mathf.Abs(G.View.orthographicSize-zoom)<.001f,"139 Exit restores outdoor angle and zoom "+spec);
    Check(Shader.GetGlobalTexture("_HarborVisibility")==G.Vision.Texture&&!G.Vision.RoomGate,"139 Outdoor fog and minimap restored "+spec);
    F111(false,0);F111(true,2.01f);G.Paused=true;yield return null;
    Check(G.Map==inside&&Mathf.Abs(Mathf.DeltaAngle(G.Rig.Yaw,225))<.1f,"139 Re-entry preserves indoor instance and angle "+spec);
    Check(inside.YardRooms138.TrueForAll(r=>r.Door.Locked?!r.Door.Open:r.Door.Open),"139 Door and key-room states persist "+spec);
    Check(inside.Loot.TrueForAll(l=>l.Opened&&l.Prop.Searched&&l.Items.TrueForAll(i=>i.Identified)),"139 Container search and identification persist on re-entry "+spec);
    G.Player.transform.position=inside.Spawn;Physics.SyncTransforms();F111(false,0);F111(true,2.01f);G.Paused=true;yield return null;
   }
   yield return DomesticContainerGallery139();
   G.Controls=new DesktopInput();G.Finish(false);yield return null;Check(!outside.Root,"139 Indoor and outdoor map roots disposed after session");
  }
  IEnumerator YardIndoorOverview138(string name,bool top){
   yield return new WaitForEndOfFrame();
   var go=new GameObject("QAIndoor138");var cam=go.AddComponent<Camera>();cam.CopyFrom(G.View);cam.enabled=false;cam.transform.rotation=Quaternion.Euler(top?90:60,top?0:45,0);cam.transform.position=Vector3.up*.6f-cam.transform.forward*45;cam.orthographicSize=G.Map.InteriorId=="yardhome"?10f:8f;cam.farClipPlane=100;
   var ambient=RenderSettings.ambientLight;RenderSettings.ambientLight=new Color(.40f,.44f,.40f);float memory=G.Vision.MemoryBrightness;G.Vision.MemoryBrightness=.78f;G.Vision.Bind();
   SaveYardCamera136(cam,name,1500,1300);Destroy(go);RenderSettings.ambientLight=ambient;G.Vision.MemoryBrightness=memory;G.Vision.Bind();yield return null;
  }
 }
}
