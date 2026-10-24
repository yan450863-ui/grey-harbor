using System;
using System.IO;
using System.Collections;
using UnityEngine;
namespace GreyHarbor {
 public sealed partial class RaidVerification {
  IEnumerator RunOfficeDoor2728(){
   var controls=G.Controls;var random=UnityEngine.Random.state;
   string dir=Path.GetDirectoryName(G.SavePath);string[] formal={"stash-v40.json","stash-v41.json","stash-v42.json"};
   byte[][] before=Array.ConvertAll(formal,n=>File.Exists(Path.Combine(dir,n))?File.ReadAllBytes(Path.Combine(dir,n)):null);
   try{
    Check(Application.version=="2.7.28"&&G.SavePath.EndsWith("verification-stash-2.7.28.json"),"2728 isolated door-fix verification");
    FreshCamp();G.Controls=new TouchInput();G.HarborWalkthrough=true;yield return G.BeginTrainingLoad2721();G.Paused=true;
    var outdoor=G.Map;Check(OfficePreview2726.Apply(G),"2728 office preview arrival");
    FreightRoomWalk2717(RaidMap.OfficeArrival2726,"2728 office entry");
    var portal=outdoor.Doors.Find(d=>d.Portal&&d.Destination=="officeright1");FreightMonsterF2720(portal,true);yield return null;
    var indoor=G.Map;var opening=indoor.OfficeOpenings2726.Find(o=>o.Orientation=="v"&&Mathf.Abs(o.X+2)<.01f&&Mathf.Abs(o.Z+14)<.01f);
    var door=opening.Door;Check(door.Name=="接待大厅门"&&!door.Open,"2728 entrance-left reception door starts closed");
    Check(Vector3.Distance(door.Pivot.position,new Vector3(-2,0,-13))<.001f,"2728 hinge attached to opposite north jamb");
    Check(door.Collider.bounds.size.z>1.99f&&Mathf.Abs(door.Collider.bounds.center.x+2)<.01f,"2728 closed slab seals original doorway");
    var hall=new Vector3(0,0,-14);var room=new Vector3(-4,0,-14);
    FreightRoomWalk2717(hall,"2728 approach reception from entrance");
    Check(!G.Navigation.ClearSegment(hall,room),"2728 closed door blocks crossing");
    for(int i=0;i<3;i++){
     FreightRoomWalk2717(new Vector3(-.9f,0,-14),"2728 corridor-side F approach");FreightMonsterF2720(door,false);
     Check(door.Open&&door.Collider.bounds.max.x<=-1.99f&&Mathf.Abs(door.Collider.bounds.center.z+13)<.01f,"2728 door opens inside room against north jamb");
     Check(G.Navigation.ClearSegment(hall,room),"2728 open doorway clear for .34m capsule");
     FreightRoomWalk2717(room,"2728 actual walk into reception");FreightRoomWalk2717(hall,"2728 actual walk back to corridor");
     FreightRoomWalk2717(new Vector3(0,0,-16),"2728 corridor remains clear south of open door");FreightRoomWalk2717(new Vector3(0,0,-12.2f),"2728 corridor remains clear north of open door");
     FreightRoomWalk2717(new Vector3(-.9f,0,-14),"2728 safe corridor-side close");FreightMonsterF2720(door,false);Check(!door.Open,"2728 actual F closes door repeatedly");
    }
    FreightMonsterF2720(door,false);FreightRoomWalk2717(opening.Position,"2728 stand inside door threshold");
    Check(!G.ToggleRoomDoor(door)&&door.Open,"2728 closing protects player in doorway");
    FreightRoomWalk2717(room,"2728 inward-open door leaves room approach clear");
    yield return CargoPicture2715("2728-reception-door-inward.png",new Vector3(-2,1,-14),35,8,false,1600,1000);
    FreightRoomWalk2717(indoor.Spawn,"2728 return to floor exit");FreightMonsterF2720(indoor.OfficeExit2726,true);yield return null;
    FreightMonsterF2720(portal,true);yield return null;
    Check(G.Map==indoor&&door.Open&&door.Collider.bounds.max.x<=-1.99f,"2728 inward door geometry and state survive interior cache");
    FreightRoomWalk2717(hall,"2728 cached corridor approach");FreightRoomWalk2717(room,"2728 cached door remains traversable");
    for(int i=0;i<formal.Length;i++)Check(MainBytes273(before[i],File.Exists(Path.Combine(dir,formal[i]))?File.ReadAllBytes(Path.Combine(dir,formal[i])):null),"2728 formal save unchanged "+formal[i]);
   }finally{G.Controls=controls;UnityEngine.Random.state=random;G.Paused=true;}
  }
 }
}
