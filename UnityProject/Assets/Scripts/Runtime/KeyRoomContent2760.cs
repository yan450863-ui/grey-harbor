using System.Collections.Generic;
using UnityEngine;
namespace GreyHarbor {
 public sealed class KeyRoomRecord2760 {
  public string Name;public Rect Area;public WorldDoor Door;public ItemKind Key;public readonly List<LootContainer> Containers=new List<LootContainer>();
 }
 public sealed partial class RaidMap {
  public readonly List<KeyRoomRecord2760> KeyRooms2760=new List<KeyRoomRecord2760>();
  public readonly List<LootContainer> OfficeCoats2760=new List<LootContainer>();
  KeyRoomRecord2760 KeyRoom2760(string name,Rect area,WorldDoor door,ItemKind key){
   door.Locked=true;door.Name=name+"门";door.KeyKind2760=key;
   var room=new KeyRoomRecord2760{Name=name,Area=area,Door=door,Key=key};KeyRooms2760.Add(room);return room;
  }
  System.Random KeyRng2760(int salt)=>new System.Random((RaidGame.Instance?RaidGame.Instance.RaidSeed:2760)^salt);
  void KeyLoot2760(KeyRoomRecord2760 room,ContainerLook look,float x,float z,float yaw,System.Random rng,SafeSize240 size=SafeSize240.Legacy){
   var model=new Vector3(x,0,z);var toward=Quaternion.Euler(0,yaw,0)*Vector3.back;
   // The compact VIP cabin's inward leaf ends near z=-12.85. Approach its southern luggage from below that swing.
   float distance=InteriorId=="ferry2"&&z<-14&&yaw==180?.7f:InteriorId=="repairmain4"&&z==17&&yaw==180?.85f:1.1f;var stand=model+toward*distance;
   if(look==ContainerLook.JewelryBox){
    var cabinet=Box("KeyRoomPersonalDrawer2760",model+Vector3.up*.36f,new Vector3(1.15f,.72f,.65f),"877158",true);cabinet.transform.rotation=Quaternion.Euler(0,yaw,0);SceneObstacle.Apply(cabinet,ObstacleKind.Low);
    foreach(float h in new[]{.24f,.52f}){var front=Box("KeyRoomDrawerFront2760",model+Quaternion.Euler(0,yaw,0)*new Vector3(0,h,-.34f),new Vector3(1.03f,.2f,.04f),"A28B6A");front.transform.rotation=cabinet.transform.rotation;}
    model.y=.73f;
   }
   var supply=look==ContainerLook.Safe?SupplyType.Safe:look==ContainerLook.Toolbox?SupplyType.Tools:SupplyType.Misc;
   var region=InteriorId.StartsWith("repair")?DropRegion230.Repair:InteriorId.StartsWith("ferry")?DropRegion230.PassengerDock:InteriorId.StartsWith("port")?DropRegion230.Entrance:InteriorId=="yardhome"?DropRegion230.Yard:DropRegion230.Office;
   var district=region==DropRegion230.Repair?HarborDistrict.Repair:region==DropRegion230.Yard?HarborDistrict.Entrance:HarborDistrict.Office;
   var loot=AddContainer(stand,model,look,supply,rng,district,yaw,false,look==ContainerLook.Safe?ContainerTier.Safe:ContainerTier.Common,region,size);
   loot.Name=room.Name+" · "+(look==ContainerLook.Safe?(size==SafeSize240.Large?"大保险箱":"小保险箱"):loot.Name)+" "+(room.Containers.Count+1);
   if(size==SafeSize240.Large)loot.SearchDuration=8;room.Containers.Add(loot);
   if(OfficeRooms2726.Count>0)OfficeSearchRecords2736.Add(new FreightLootRecord2717{Container=loot,Model=loot.View,Stand=stand,AccessPoint=stand+toward*.4f,RoomName=room.Name,Yaw=yaw});
  }
  void PopulatePortKeyRoom2760(){
   var room=KeyRoom2760("入口储藏室",PortLockedRoomBounds,Doors.Find(d=>d.Name=="储藏室上锁房间"),ItemKind.EntranceStorageKey2760);var rng=KeyRng2760(276001);
   KeyLoot2760(room,ContainerLook.Safe,-13,4.8f,0,rng,SafeSize240.Small);
   KeyLoot2760(room,ContainerLook.FileCabinet,-11.5f,4.9f,0,rng);KeyLoot2760(room,ContainerLook.FileCabinet,-10,4.9f,0,rng);
   foreach(float x in new[]{-13f,-11.5f,-10f})KeyLoot2760(room,ContainerLook.StorageBin,x,1.1f,180,rng);
   NoticeBoard114(new Vector3(-11.5f,1.6f,.66f));
  }
  void PopulateFerryKeyRoom2760(){
   if(InteriorId!="ferry2")return;var vip=FerryRooms143.Find(r=>r.Door.Locked);
   var room=KeyRoom2760("客轮贵宾舱",vip.Area,vip.Door,ItemKind.FerryVipKey2760);var rng=KeyRng2760(276002);
   // Keep the same inward-opening 1.90 metre leaf. A 95 degree opening leaves a continuous luggage-side passage.
   room.Door.OpenAngle=-95;
   var rotation=Quaternion.Euler(0,room.Door.OpenAngle,0);var along=rotation*Vector3.forward*1.90f;var cross=rotation*Vector3.right*.10f;
   var center=room.Door.Pivot.position+along*.5f;float halfX=(Mathf.Abs(along.x)+Mathf.Abs(cross.x))*.5f+.32f,halfZ=(Mathf.Abs(along.z)+Mathf.Abs(cross.z))*.5f+.32f;
   room.Door.OpenSafety=new Rect(center.x-halfX,center.z-halfZ,halfX*2,halfZ*2);
   KeyLoot2760(room,ContainerLook.Safe,-6.4f,-9.7f,0,rng,SafeSize240.Small);KeyLoot2760(room,ContainerLook.StorageBin,-5.05f,-9.7f,0,rng);
   KeyLoot2760(room,ContainerLook.RollingSuitcase,-6.4f,-14.1f,180,rng);KeyLoot2760(room,ContainerLook.Suitcase,-5.05f,-14.1f,180,rng);
   KeyLoot2760(room,ContainerLook.Suitcase,-3.75f,-14.1f,180,rng);KeyLoot2760(room,ContainerLook.JewelryBox,-6.6f,-11.8f,270,rng);
   NoticeBoard114(new Vector3(-5.1f,1.65f,-14.84f));
  }
  void PopulateRepairKeyRoom2760(){
   if(RepairFloor220!=4)return;var key=RepairMainRooms210.Find(r=>r.Name=="钥匙管理室");
   var room=KeyRoom2760(key.Name,key.Area,key.Door,ItemKind.RepairManagementKey2760);var rng=KeyRng2760(276003);
   KeyLoot2760(room,ContainerLook.Safe,19.1f,22.5f,90,rng,SafeSize240.Large);
   KeyLoot2760(room,ContainerLook.FileCabinet,14.4f,23.1f,0,rng);KeyLoot2760(room,ContainerLook.FileCabinet,16.6f,23.1f,0,rng);
   KeyLoot2760(room,ContainerLook.PrecisionCase,14.4f,17f,180,rng);KeyLoot2760(room,ContainerLook.PrecisionCase,16.6f,17f,180,rng);KeyLoot2760(room,ContainerLook.Toolbox,19f,17f,180,rng);
   PortDesk(new Vector3(18.2f,0,20.4f),false);Chair(new Vector3(18.2f,0,19.2f),180);NoticeBoard114(new Vector3(15.8f,1.65f,23.63f));
  }
  void PopulateOfficeKeyRooms2760(){
   var rng=KeyRng2760(276010+OfficeFloor2733+(OfficeLeft2741?10:0));
   if(OfficeLeft2741&&OfficeFloor2733==1){
    var r=OfficeRooms2726.Find(v=>v.Id=="06");var room=KeyRoom2760(r.Name,r.Area,OfficeLeftLockedDoor2741,ItemKind.OfficeDutyKey2760);
    KeyLoot2760(room,ContainerLook.Safe,-18.8f,22.8f,270,rng,SafeSize240.Large);KeyLoot2760(room,ContainerLook.JewelryBox,-11.5f,23.2f,0,rng);
    foreach(float z in new[]{16f,18.5f,21f})KeyLoot2760(room,ContainerLook.FileCabinet,-8.8f,z,90,rng);
    foreach(float x in new[]{-18.8f,-16.5f,-10f})KeyLoot2760(room,ContainerLook.StorageBin,x,14.8f,180,rng);
    OfficeProp2736("06",FutureProp2717.SingleWorkstation,-15.2f,22.8f,0);OfficeProp2736("06",FutureProp2717.OfficeChair,-15.2f,21.4f,0);
    OfficeProp2736("06",FutureProp2717.OfficeSofa,-18.9f,18.3f,270);OfficeWall2736("06",FutureProp2717.NoticeBoard,-14.5f,1.6f,23.82f,0);
   }
   if(!OfficeLeft2741&&OfficeFloor2733==3){
    var r=OfficeRooms2726.Find(v=>v.Id=="10");var room=KeyRoom2760(r.Name,r.Area,OfficeResearchDoor2740,ItemKind.OfficeResearchKey2760);
    KeyLoot2760(room,ContainerLook.Safe,12.8f,22.7f,90,rng,SafeSize240.Large);KeyLoot2760(room,ContainerLook.Safe,9.8f,23.1f,0,rng,SafeSize240.Small);
    foreach(float x in new[]{3.5f,5.1f,8.8f})KeyLoot2760(room,ContainerLook.FileCabinet,x,23.2f,0,rng);KeyLoot2760(room,ContainerLook.FileCabinet,2.8f,15.5f,270,rng);
    foreach(float x in new[]{5f,7.3f,9.6f,12f})KeyLoot2760(room,ContainerLook.PrecisionCase,x,14.8f,180,rng);
    OfficeProp2736("10",FutureProp2717.SingleWorkstation,6.3f,20.8f,0);OfficeProp2736("10",FutureProp2717.OfficeChair,6.3f,19.4f,0);
    OfficeProp2736("10",FutureProp2717.MeetingTable,8.7f,18.5f,0);OfficeProp2736("10",FutureProp2717.VisitorChair,8.7f,17.3f,180);
    OfficeWall2736("10",FutureProp2717.NoticeBoard,8.5f,1.6f,23.82f,0);OfficeProp2736("10",FutureProp2717.ServerRack,13,17.5f,90);
   }
   // One searchable wall coat per floor, outside all locked rooms.
   string id=OfficeLeft2741?"01":OfficeFloor2733==3?"01":"10";
   var coatRoom=OfficeRooms2726.Find(r=>r.Id==id);var p=OfficeLeft2741||OfficeFloor2733==3?new Vector3(-7.5f,0,-23.82f):new Vector3(12,0,23.82f);float yaw=p.z<0?180:0;
   var toward=Quaternion.Euler(0,yaw,0)*Vector3.back;var stand=p+toward*1.2f;
   var coat=AddContainer(stand,p,ContainerLook.Coat,SupplyType.Misc,rng,HarborDistrict.Office,yaw,false,ContainerTier.Common,DropRegion230.Office);coat.Name=(OfficeLeft2741?"左塔":"右塔")+OfficeFloor2733+"楼 · "+coatRoom.Name+"外套";OfficeCoats2760.Add(coat);
   OfficeSearchRecords2736.Add(new FreightLootRecord2717{Container=coat,Model=coat.View,Stand=stand,AccessPoint=stand+toward*.4f,RoomName=coatRoom.Name,Yaw=yaw,WallMounted=true});
  }
 }
}
