using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace GreyHarbor {
 public sealed partial class RaidVerification {
  IEnumerator FreightRooms2719(){
   var outdoor=G.Map;var maps=new List<RaidMap>();var visions=new List<RaidVision>();var contents=new List<string[]>();var memories=new List<bool[]>();freightRoomRows2717.Clear();freightRoomRows2717.Add("kind,name,x,y,z,detail");
   foreach(string key in new[]{"freightduty","freightwarehouse"}){
    var stand=key=="freightduty"?RaidMap.FreightDutyArrival2716:RaidMap.FreightWarehouseArrival2716;var portal=outdoor.Doors.Find(d=>d.Portal&&d.Destination==key);FreightRoomPlace2717(stand,key+" outside");FreightRoomHold2717(portal);yield return null;var indoor=G.Map;
    Check(indoor!=outdoor&&indoor.FreightInterior2716&&indoor.InteriorId==key&&G.Enemies.Count==0,"2719 enters correct actual furnished freight room "+key);FreightDoorHeaderCheck2719(false);
    foreach(var room in indoor.FreightRooms2716)Check(!room.Door.Open&&!G.Vision.RoomAllowed(room.WalkPoint),"2719 unknown adjacent room remains concealed "+room.Name);
    yield return FreightLiveImage2718("2719-"+key+"-initial-closed",indoor.Spawn,new Vector3(0,0,1),45);
    yield return FreightRoomDoors2717();yield return FreightRoomSearch2717();FreightDoorHeaderCheck2719(true);
    var p=key=="freightduty"?indoor.FreightRooms2716[0].WalkPoint:new Vector3(1.5f,0,3.2f);
    foreach(float yaw in new[]{45f,135f,225f,315f})yield return FreightLiveImage2718("2719-"+key+"-live-"+yaw,p,key=="freightduty"?new Vector3(-1,0,1):Vector3.right,yaw);
    yield return CargoPicture2715("2719-"+key+"-floor-materials.png",Vector3.zero,0,11,true,1800,1800);
    maps.Add(indoor);visions.Add(G.Vision);contents.Add(indoor.FreightLootRecords2717.ConvertAll(r=>JsonUtility.ToJson(new CargoItems2715{Items=r.Container.Items.ToArray()})).ToArray());memories.Add((bool[])G.Vision.Seen.Clone());FreightRoomWalk2717(indoor.Spawn,key+" exit");FreightRoomHold2717(indoor.FreightExit2716);yield return null;Check(G.Map==outdoor,"2719 exterior object restored "+key);
   }
   for(int m=0;m<maps.Count;m++){
    var indoor=maps[m];var stand=indoor.InteriorId=="freightduty"?RaidMap.FreightDutyArrival2716:RaidMap.FreightWarehouseArrival2716;FreightRoomPlace2717(stand,"2719 revisit");FreightRoomHold2717(outdoor.Doors.Find(d=>d.Destination==indoor.InteriorId));yield return null;
    Check(G.Map==indoor&&G.Vision==visions[m]&&G.Map.Loot.Count==6,"2719 two furnished rooms retain independent cached identity");bool preserved=true;for(int i=0;i<memories[m].Length;i++)if(memories[m][i]&&!G.Vision.Seen[i])preserved=false;Check(preserved,"2719 exploration memory survives room revisit");
    for(int i=0;i<indoor.FreightLootRecords2717.Count;i++){var box=indoor.FreightLootRecords2717[i].Container;Check(box.Opened&&contents[m][i]==JsonUtility.ToJson(new CargoItems2715{Items=box.Items.ToArray()}),"2719 searched contents survive "+box.Name);}
    FreightRoomWalk2717(indoor.Spawn,"2719 cached exit");FreightRoomHold2717(indoor.FreightExit2716);yield return null;Check(G.Map==outdoor,"2719 final room exit restores same exterior");
   }
  }
 }
}
