using System;using System.IO;using System.Linq;using System.Collections;using UnityEngine;
namespace GreyHarbor {public sealed partial class RaidVerification {
 IEnumerator PreviewBubbles340(){HudCamp329();G.InventoryOpen=false;G.HarborWalkthrough=true;G.ModRange315=false;G.BeginTraining();G.enabled=false;G.InventoryOpen=G.Paused=false;var entrance=G.Map.Doors.Find(d=>d.Portal&&d.Destination=="portbooth");G.Player.transform.position=entrance.Position;G.SwitchInterior("portbooth");yield return null;yield return null;
 var box=G.Map.Loot.FirstOrDefault(b=>b.Prop&&b.Prop.Look==ContainerLook.Coat)??G.Map.Loot.First();G.Player.transform.position=box.Position+Vector3.right*1.1f;G.Rig.Target=G.Player.transform;G.NearbyDoor=null;G.Nearby=box;G.NoticeTime=0;
 foreach(float yaw in new[]{45f,135f}){G.Rig.ResetView(yaw);G.Rig.Tick(0);G.Player.Aim=(box.Position-G.Player.transform.position).normalized;Call314(G,"LateUpdate");yield return ShotBubble340("container-"+yaw);}
 G.Nearby=null;var door=G.Map.Doors.First(d=>d.Portal);G.Player.transform.position=door.Position+Vector3.right;G.NearbyDoor=door;G.Rig.ResetView(45);G.Rig.Tick(0);G.Player.Aim=(door.Position-G.Player.transform.position).normalized;Call314(G,"LateUpdate");G.DoorHold=0;yield return ShotBubble340("door-idle");G.DoorHold=1;yield return ShotBubble340("door-progress");G.Finish(false);Application.Quit(0);
 }
 IEnumerator ShotBubble340(string name){G.NoticeTime=0;yield return null;yield return null;yield return new WaitForEndOfFrame();var picture=ScreenCapture.CaptureScreenshotAsTexture();File.WriteAllBytes(Path.Combine(folder,"native-340-"+name+".png"),picture.EncodeToPNG());Destroy(picture);}
}}
