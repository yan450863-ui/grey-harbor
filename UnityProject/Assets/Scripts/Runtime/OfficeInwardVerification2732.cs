using System;
using System.IO;
using System.Collections;
using UnityEngine;
namespace GreyHarbor {
 public sealed partial class RaidVerification {
  IEnumerator RunOfficeInward2732(){
   var controls=G.Controls;var random=UnityEngine.Random.state;string dir=Path.GetDirectoryName(G.SavePath);string[] formal={"stash-v40.json","stash-v41.json","stash-v42.json"};byte[][] original=Array.ConvertAll(formal,n=>File.Exists(Path.Combine(dir,n))?File.ReadAllBytes(Path.Combine(dir,n)):null);
   try{
    Check(Application.version=="2.7.32"&&G.SavePath.EndsWith("verification-stash-2.7.32.json"),"2732 isolated inward-door verification");
    FreshCamp();G.Controls=new TouchInput();G.HarborWalkthrough=true;yield return G.BeginTrainingLoad2721();G.Paused=true;var outdoor=G.Map;
    OfficePreview2726.Apply(G);FreightRoomWalk2717(RaidMap.OfficeArrival2726,"2732 office entry");var portal=outdoor.Doors.Find(d=>d.Portal&&d.Destination=="officeright1");FreightMonsterF2720(portal,true);yield return null;
    var indoor=G.Map;Check(indoor.OfficeOpenings2726.FindAll(o=>o.Door!=null).Count==12&&indoor.OfficeOpenings2726.Count==14,"2732 twelve doors and five sealed openings retained");
    yield return OfficeDoors2727();int positive=0,negative=0;
    foreach(var o in indoor.OfficeOpenings2726)if(o.Door!=null){
     var d=o.Door;var adjacent=indoor.OfficeRooms2726.FindAll(r=>o.X>=r.Area.xMin-.01f&&o.X<=r.Area.xMax+.01f&&o.Z>=r.Area.yMin-.01f&&o.Z<=r.Area.yMax+.01f);
     var room=adjacent.Find(r=>!r.Stair)??adjacent[0];var center=d.Collider.bounds.center;
     Check(room.Area.Contains(new Vector2(center.x,center.z)),"2732 open leaf is inside target room "+d.Name+" target="+room.Name);
     var n=OfficeNormal2727(o);float sign=Mathf.Sign(Vector3.Dot(room.Center-o.Position,n));Check(sign*Vector3.Dot(center-o.Position,n)>.8f,"2732 leaf swings toward room rather than corridor "+o.Position);
     if(d.OpenAngle>0)positive++;else negative++;
     Check(G.Navigation.ClearSegment(o.Position-n*.8f,o.Position+n*.8f),"2732 doorway capsule crossing clear after inward opening "+d.Name);
     FreightRoomWalk2717(o.Position-n*.8f,"2732 actual inward door approach");FreightRoomWalk2717(o.Position+n*.8f,"2732 actual inward door crossing");
     DoorCloseProbe2729(d,-sign,"2732 inward leaf automatic safe closing "+d.Name);
     FreightRoomWalk2717(o.Position-n*sign*.9f,"2732 reopen approach");FreightMonsterF2720(d,false);Check(d.Open,"2732 real F reopens inward leaf "+d.Name);yield return null;
    }
    Check(positive>0&&negative>0,"2732 both positive and negative rotations used according to room side");
    foreach(var room in indoor.OfficeRooms2726){FreightRoomWalk2717(room.Center,"2732 all room centres accessible "+room.Name);yield return null;}
    foreach(var p in new[]{new Vector3(0,0,-14),new Vector3(-4,0,-14),new Vector3(-4,0,-8),new Vector3(0,0,-8)})FreightRoomWalk2717(p,"2732 left bypass unaffected by inward leaves");
    yield return CargoPicture2715("2732-office-inward-doors.png",Vector3.zero,35,29,false,1800,1200);
    FreightRoomWalk2717(indoor.Spawn,"2732 exit");FreightMonsterF2720(indoor.OfficeExit2726,true);yield return null;FreightMonsterF2720(portal,true);yield return null;
    Check(G.Map==indoor&&indoor.OfficeOpenings2726.TrueForAll(o=>o.Door==null||o.Door.Open),"2732 correct inward geometry/state survives cache");
    for(int i=0;i<formal.Length;i++)Check(MainBytes273(original[i],File.Exists(Path.Combine(dir,formal[i]))?File.ReadAllBytes(Path.Combine(dir,formal[i])):null),"2732 formal save unchanged "+formal[i]);
   }finally{G.Controls=controls;UnityEngine.Random.state=random;G.Paused=true;}
  }
 }
}
