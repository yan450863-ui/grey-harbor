using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace GreyHarbor {public sealed partial class RaidVerification {
 void OfficeWallPixel2736(string label,Vector3 face,Vector3 inward,bool known){
  MeshRenderer wall=null;foreach(var r in G.Map.Root.GetComponentsInChildren<MeshRenderer>())if(r.name=="OfficeWall2726"){var b=r.bounds;b.Expand(.07f);if(b.Contains(face+Vector3.up*1.65f)){wall=r;break;}}
  Check(wall!=null,"2736 actual wall exists "+label);if(wall)FreightWallRendererProbe2718(wall,"2736-"+label,face+Vector3.up*1.65f,inward,known);
 }
 void OfficeFloorPixel2736(string label,Vector3 point,bool known){
  var r=G.Map.OfficeFloorRenderer2736;int layer=r.gameObject.layer;var go=new GameObject("QA2736_floor_camera");var camera=go.AddComponent<Camera>();camera.enabled=false;camera.orthographic=true;camera.orthographicSize=.3f;camera.nearClipPlane=.1f;camera.farClipPlane=5;camera.clearFlags=CameraClearFlags.SolidColor;camera.backgroundColor=Color.black;camera.cullingMask=1<<31;camera.transform.position=point+Vector3.up*3;camera.transform.rotation=Quaternion.Euler(90,0,0);Texture2D frame=null;
  try{r.gameObject.layer=31;frame=HvRender277(camera,"2736-floor-pixel-"+label+".png",64,64);float sum=0,peak=0;for(int y=16;y<48;y++)for(int x=16;x<48;x++){float b=frame.GetPixel(x,y).grayscale;sum+=b;peak=Mathf.Max(peak,b);}Check(known?sum/1024>.02f:peak<.012f,"2736 actual floor GPU "+label+" readable="+known+" mean="+sum/1024+" peak="+peak);}finally{r.gameObject.layer=layer;if(frame)Destroy(frame);Destroy(go);}
 }
 IEnumerator RunOfficeFirst2736(){
  var input=G.Controls;string dir=Path.GetDirectoryName(G.SavePath);string[] names={"stash-v40.json","stash-v41.json","stash-v42.json"};var original=Array.ConvertAll(names,n=>File.Exists(Path.Combine(dir,n))?File.ReadAllBytes(Path.Combine(dir,n)):null);
  try{
   Check(Application.version=="2.7.36"&&G.SavePath.EndsWith("verification-stash-2.7.36.json"),"2736 isolated first-floor art/search verification");FreshCamp();G.Controls=new TouchInput();G.HarborWalkthrough=true;yield return G.BeginTrainingLoad2721();G.Paused=true;var outdoor=G.Map;var exteriorLoot=outdoor.Loot.ToArray();var actors=G.Enemies.ToArray();
   OfficePreview2726.Apply(G);FreightRoomWalk2717(RaidMap.OfficeArrival2726,"2736 office arrival");FreightMonsterF2720(outdoor.Doors.Find(d=>d.Portal&&d.Destination=="officeright1"),true);yield return null;var first=G.Map;
   Check(first.OfficeFloor2733==1&&first.OfficeRooms2726.Count==12&&first.OfficeOpenings2726.Count==14&&first.OfficeOpenings2726.FindAll(o=>o.Door!=null).Count==12,"2736 confirmed first-floor room and door topology retained");
   Check(first.OfficeFloorTexture2736&&first.OfficeFloorRenderer2736&&first.OfficeWallRenderers2736>20,"2736 finished floor and fog-aware wall materials exist");
   Check(first.OfficeDressingRecords2736.Count>=100&&first.OfficeSearchRecords2736.Count==24,"2736 actual populated first-floor dressing and twenty-four search models");
   Check(first.OfficeCombinedRenderers2736<first.OfficeOriginalRenderers2736/2,"2736 static furniture batching reduces renderers "+first.OfficeOriginalRenderers2736+" to "+first.OfficeCombinedRenderers2736);var views=first.OfficeRoomBatches2736;UnloadingPalette2724(views,"2736 office first-floor furniture");
   var palette=new HashSet<int>();foreach(var c in first.OfficeFloorTexture2736.GetPixels32())palette.Add((c.r<<16)|(c.g<<8)|c.b);Check(palette.Count>300,"2736 finished floor contains real colour/grain rather than flat whitebox");File.WriteAllBytes(Path.Combine(folder,"2736-office-floor-texture.png"),first.OfficeFloorTexture2736.EncodeToPNG());
   OfficeAreaSeen2730(new Rect(14,14,6,2),"2736 north stair vestibule before doors");
   OfficeWallPixel2736("corridor-known",new Vector3(20.1f,0,12.5f),Vector3.left,true);OfficeWallPixel2736("meeting-closed",new Vector3(20.1f,0,4),Vector3.left,false);
   OfficeFloorPixel2736("hall-known",new Vector3(0,0,-20),true);OfficeFloorPixel2736("meeting-closed",new Vector3(10,0,4),false);
   yield return OfficeDoors2727();
   foreach(var r in first.OfficeRooms2726){FreightRoomWalk2717(r.Center,"2736 decorated room visit "+r.Name);OfficeAreaSeen2730(r.Area,"2736 decorated room reveal "+r.Id);yield return null;}
   foreach(var o in first.OfficeOpenings2726)if(o.Type=="hole"){var n=OfficeNormal2727(o);FreightRoomWalk2717(o.Position-n*.85f,"2736 original wall-hole approach");FreightRoomWalk2717(o.Position+n*.85f,"2736 original wall-hole crossing");}
   foreach(var p in new[]{new Vector3(0,0,-14),new Vector3(-4,0,-14),new Vector3(-4,0,-8),new Vector3(0,0,-8),new Vector3(17,0,15)})FreightRoomWalk2717(p,"2736 original left bypass and north door vestibule");
   OfficeWallPixel2736("meeting-open",new Vector3(20.1f,0,4),Vector3.left,true);OfficeFloorPixel2736("meeting-open",new Vector3(10,0,4),true);
   foreach(var o in first.OfficeOpenings2726)if(o.Door!=null){var normal=OfficeNormal2727(o);Vector3 stand=o.Position-normal*.95f;if(!OfficeReachable2727(stand))stand=o.Position+normal*.95f;FreightRoomWalk2717(stand,"2736 decorated door close approach");FreightMonsterF2720(o.Door,false);Check(!o.Door.Open,"2736 furnished room door still closes "+o.Door.Name);FreightMonsterF2720(o.Door,false);Check(o.Door.Open,"2736 furnished room door still opens inward "+o.Door.Name);}
   foreach(var rec in first.OfficeSearchRecords2736){var renderers=rec.Model.GetComponentsInChildren<Renderer>();var b=renderers[0].bounds;foreach(var renderer in renderers)b.Encapsulate(renderer.bounds);var room=first.OfficeRooms2726.Find(r=>r.Name==rec.RoomName);Check(room!=null&&room.Area.Contains(new Vector2(b.center.x,b.center.z)),"2736 searchable model belongs to real room "+rec.Container.Name);}
   bool clearSearch=true;foreach(var rec in first.OfficeSearchRecords2736)foreach(var point in new[]{rec.AccessPoint,rec.Stand})if(!CargoClear2713(point)){clearSearch=false;foreach(var collider in Physics.OverlapCapsule(point+Vector3.up*.35f,point+Vector3.up,.34f,SceneObstacle.MovementMask,QueryTriggerInteraction.Ignore))Debug.Log("2736 SEARCH BLOCKED "+rec.Container.Name+" point="+point+" collider="+collider.name+" bounds="+collider.bounds);}Check(clearSearch,"2736 all twenty-four search approach and standing capsules clear");
   yield return UnloadingSearchRecords2724(first.OfficeSearchRecords2736,24,24);var contents=first.OfficeSearchRecords2736.ConvertAll(r=>FreightMonsterItems2720(r.Container));
   yield return CargoPicture2715("2736-office-first-floor-decorated.png",Vector3.zero,35,29,false,1800,1200);yield return CargoPicture2715("2736-office-first-floor-overhead.png",Vector3.zero,0,27,true,1800,1800);
   foreach(var p in new[]{new Vector3(-8,0,-11),new Vector3(10,0,-15),new Vector3(10,0,4)}){FreightRoomWalk2717(p,"2736 actual decorated gameplay photo stand");G.Rig.Tick(0);G.Player.Draw(G.View,0);yield return null;var frame=HvRender277(G.View,"2736-live-room-"+Mathf.RoundToInt(p.z)+".png",1600,900);UnloadingLiveImage2724(frame,"2736 real office camera");Destroy(frame);}
   foreach(bool north in new[]{false,true}){var up=first.Doors.Find(d=>d.Destination=="office-floor:2:"+(north?"north":"south"));FreightRoomWalk2717(up.Position,"2736 furnished stairs up approach");FreightMonsterF2720(up,true);yield return null;var second=G.Map;Check(second.OfficeFloor2733==2&&second.Loot.Count==0&&second.OfficeDressingRecords2736.Count==0&&!second.OfficeFloorTexture2736,"2736 second floor retains whitebox");var down=second.Doors.Find(d=>d.Destination=="office-floor:1:"+(north?"north":"south"));FreightRoomWalk2717(down.Position,"2736 unchanged upper stairs return");FreightMonsterF2720(down,true);yield return null;Check(G.Map==first&&first.Loot.Count==24,"2736 furnished floor cache restores without duplication");}
   for(int i=0;i<contents.Count;i++){var box=first.OfficeSearchRecords2736[i].Container;Check(box.Opened&&box.Prop.Searched&&box.Items.TrueForAll(it=>it.Identified)&&contents[i]==FreightMonsterItems2720(box),"2736 search content and identification cache "+box.Name);}
   OfficeAreaSeen2730(new Rect(14,14,6,2),"2736 cached north vestibule");FreightRoomWalk2717(first.Spawn,"2736 furnished lobby exterior exit");FreightMonsterF2720(first.OfficeExit2726,true);yield return null;Check(G.Map==outdoor&&outdoor.Loot.Count==exteriorLoot.Length&&G.Enemies.Count==actors.Length,"2736 exterior restored");
   for(int i=0;i<names.Length;i++)Check(MainBytes273(original[i],File.Exists(Path.Combine(dir,names[i]))?File.ReadAllBytes(Path.Combine(dir,names[i])):null),"2736 formal save unchanged "+names[i]);
  }finally{G.CloseInventory();G.Controls=input;G.Paused=true;}
 }
}}
