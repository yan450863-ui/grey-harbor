using System;
using System.IO;
using System.Collections;
using UnityEngine;
namespace GreyHarbor {
 public sealed partial class RaidVerification {
  IEnumerator RunOfficeSealed2731(){
   var controls=G.Controls;var random=UnityEngine.Random.state;string dir=Path.GetDirectoryName(G.SavePath);string[] formal={"stash-v40.json","stash-v41.json","stash-v42.json"};byte[][] original=Array.ConvertAll(formal,n=>File.Exists(Path.Combine(dir,n))?File.ReadAllBytes(Path.Combine(dir,n)):null);
   try{
    Check(Application.version=="2.7.31"&&G.SavePath.EndsWith("verification-stash-2.7.31.json"),"2731 isolated sealed-opening verification");
    FreshCamp();G.Controls=new TouchInput();G.HarborWalkthrough=true;yield return G.BeginTrainingLoad2721();G.Paused=true;var outdoor=G.Map;
    OfficePreview2726.Apply(G);FreightRoomWalk2717(RaidMap.OfficeArrival2726,"2731 office arrival");var portal=outdoor.Doors.Find(d=>d.Portal&&d.Destination=="officeright1");FreightMonsterF2720(portal,true);yield return null;
    var indoor=G.Map;Check(G.Vision.RoomGate&&indoor.OfficeOpenings2726.Count==14&&indoor.OfficeOpenings2726.FindAll(o=>o.Door!=null).Count==12&&indoor.OfficeOpenings2726.FindAll(o=>o.Type=="hole").Count==2,"2731 twelve retained doors and two original holes; five marked openings removed entirely");
    var sealedPoints=new[]{new Vector3(-2,0,-20),new Vector3(2,0,-8),new Vector3(2,0,1),new Vector3(-14,0,14),new Vector3(-17,0,-16)};
    for(int i=0;i<sealedPoints.Length;i++){
     var p=sealedPoints[i];var n=i<3?Vector3.right:Vector3.forward;
     Check(!indoor.OfficeOpenings2726.Exists(o=>Vector3.Distance(o.Position,p)<.01f)&&!indoor.Doors.Exists(d=>!d.Portal&&Vector3.Distance(d.Position,p)<.01f),"2731 marked position no longer opening or interactable door "+p);
     foreach(float height in new[]{.2f,1f,2f,2.8f})Check(Physics.Linecast(p-n*.5f+Vector3.up*height,p+n*.5f+Vector3.up*height,SceneObstacle.MovementMask,QueryTriggerInteraction.Ignore),"2731 continuous solid wall at former opening "+p+" height="+height);
     Check(!G.Navigation.ClearSegment(p-n*.8f,p+n*.8f),"2731 sealed opening blocks capsule crossing "+p);
    }
    foreach(var area in new[]{new Rect(-2,-24,4,48),new Rect(-20,-6,40,3),new Rect(-20,11,40,3)})OfficeAreaSeen2730(area,"2731 complete public corridor");
    foreach(var room in indoor.OfficeRooms2726)Check(!G.Vision.Explored(room.Center),"2731 sealed boundaries do not initially reveal closed room "+room.Id);
    foreach(var point in new[]{new Vector3(-2,0,-14),new Vector3(-5,0,14)}){
     var o=indoor.OfficeOpenings2726.Find(p=>Vector3.Distance(p.Position,point)<.01f);var n=OfficeNormal2727(o);FreightRoomPlace2717(point+n*(o.Orientation=="v"?.95f:-.95f),"2731 isolated connected-room exploration fixture");FreightMonsterF2720(o.Door,false);
     foreach(string id in Mathf.Abs(point.x+2)<.01f?new[]{"01","02"}:new[]{"08","09"})OfficeAreaSeen2730(indoor.OfficeRooms2726.Find(r=>r.Id==id).Area,"2731 original hole-connected group "+id);
     FreightMonsterF2720(o.Door,false);
    }
    yield return OfficeDoors2727();
    foreach(var room in indoor.OfficeRooms2726){FreightRoomWalk2717(room.Center,"2731 room remains reachable after sealing "+room.Name);OfficeAreaSeen2730(room.Area,"2731 opened room "+room.Id);yield return null;}
    foreach(var o in indoor.OfficeOpenings2726)if(o.Type=="hole"){var n=OfficeNormal2727(o);FreightRoomWalk2717(o.Position-n*.8f,"2731 original hole approach");FreightRoomWalk2717(o.Position+n*.8f,"2731 original hole crossing");}
    foreach(var p in new[]{new Vector3(0,0,-14),new Vector3(-4,0,-14),new Vector3(-4,0,-8),new Vector3(0,0,-8)})FreightRoomWalk2717(p,"2731 left reception bypass remains usable");
    G.Rig.Tick(0);G.Player.Draw(G.View,0);yield return null;var frame=HvRender277(G.View,"2731-office-live-fog.png",1600,900);Destroy(frame);
    yield return CargoPicture2715("2731-office-five-openings-sealed.png",Vector3.zero,35,29,false,1800,1200);
    var vision=G.Vision;FreightRoomWalk2717(indoor.Spawn,"2731 actual return to exit");FreightMonsterF2720(indoor.OfficeExit2726,true);yield return null;FreightMonsterF2720(portal,true);yield return null;
    Check(G.Map==indoor&&G.Vision==vision,"2731 same corrected wall map and exploration cache restored");OfficeAreaSeen2730(new Rect(-2,-24,4,48),"2731 cached corridor");
    foreach(var room in indoor.OfficeRooms2726)Check(G.Vision.Explored(room.Center),"2731 explored room memory preserved "+room.Id);
    for(int i=0;i<formal.Length;i++)Check(MainBytes273(original[i],File.Exists(Path.Combine(dir,formal[i]))?File.ReadAllBytes(Path.Combine(dir,formal[i])):null),"2731 formal save unchanged "+formal[i]);
   }finally{G.Controls=controls;UnityEngine.Random.state=random;G.Paused=true;}
  }
 }
}
