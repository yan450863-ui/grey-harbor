using UnityEngine;
namespace GreyHarbor {
 public sealed partial class RaidMap {
  public static readonly Vector3 MainHouseApproach=new Vector3(0,0,6.2f);
  public static readonly string[] MainHouseRoomNames={"值班办公室","员工休息室","工具间","医务室","档案室","主储藏室"};
  public static readonly Rect[] MainHouseRooms={
   new Rect(-12.1f,-10.1f,10.2f,6.8f),new Rect(1.9f,-10.1f,10.2f,6.8f),
   new Rect(-12.1f,-3.3f,10.2f,6.6f),new Rect(1.9f,-3.3f,10.2f,6.6f),
   new Rect(-12.1f,3.3f,10.2f,6.8f),new Rect(1.9f,3.3f,10.2f,6.8f)};
  public static Vector3 MainHouseDoorApproach(int room){return new Vector3(room%2==0?-.9f:.9f,0,room<2?-6.6f:room<4?0:6.6f);}
  public void AddMainHouseEntrance(){Doors.Add(new WorldDoor{Name="进入主屋",Destination="mainhouse",Portal=true,Position=new Vector3(0,0,7.1f)});}
  public void BuildMainHouseInterior(){
   IsInterior=true;InteriorId="mainhouse";InteriorTitle="主屋";InitialRoom=new Rect(-2.1f,-10.1f,4.2f,20.2f);Bounds=new Rect(-12.3f,-10.3f,24.6f,20.6f);Spawn=new Vector3(0,0,-8.85f);
   Root=new GameObject("Training_MainHouse_Interior").transform;
   Box("MainHouseFloor",new Vector3(0,-.12f,0),new Vector3(24.1f,.24f,20.1f),"69726B");
   InteriorWall(-12,0,.18f,20.1f);InteriorWall(12,0,.18f,20.1f);InteriorWall(0,-10,24.1f,.18f);InteriorWall(0,10,24.1f,.18f);
   foreach(float z in new[]{-3.3f,3.3f}){InteriorWall(-7,z,10,.18f);InteriorWall(7,z,10,.18f);}
   // Corridor partitions have six real door gaps, all opening into rooms.
   foreach(float x in new[]{-2f,2f}){float start=-10;foreach(float center in new[]{-6.6f,0,6.6f}){float end=center-.9f;InteriorWall(x,(start+end)/2,.18f,end-start);start=center+.9f;}InteriorWall(x,(start+10)/2,.18f,10-start);}
   for(int i=0;i<6;i++)MainHouseRoomDoor(i);
   Box("MainHouseExit",new Vector3(0,1,-9.86f),new Vector3(1.6f,2,.07f),"536B61");Box("ExitPull",new Vector3(.55f,.92f,-9.78f),new Vector3(.06f,.18f,.07f),"BCAA80");
   Doors.Add(new WorldDoor{Name="返回室外",Portal=true,Destination="mainhouse",Position=new Vector3(0,0,-9.65f)});
   var rng=new System.Random(RaidGame.Instance.RaidSeed^113);
   // Office: a desk case, filing cabinet and wall coat.
   AddContainer(new Vector3(-6,0,-7.55f),new Vector3(-6,0,-8.7f),ContainerLook.Suitcase,SupplyType.Misc,rng,HarborDistrict.Office,180);
   AddContainer(new Vector3(-10.9f,0,-5.2f),new Vector3(-10.9f,0,-4),ContainerLook.FileCabinet,SupplyType.Misc,rng,HarborDistrict.Office);
   AddContainer(new Vector3(-10.8f,0,-7.3f),new Vector3(-11.8f,0,-7.3f),ContainerLook.Coat,SupplyType.Misc,rng,HarborDistrict.Office,-90,false);
   Chair(new Vector3(-7.2f,0,-8.2f),90);NoticeBoard114(new Vector3(-8.6f,1.45f,-9.83f));
   // Rest room: suitcase rests on sofa; coats hang on wall.
   LowFurniture("RestSofa",new Vector3(6,.28f,-8.8f),new Vector3(2.6f,.56f,.9f),"707B68");Box("SofaBack",new Vector3(6,.79f,-9.2f),new Vector3(2.6f,.65f,.15f),"7D8772");
   AddContainer(new Vector3(6,0,-7.7f),new Vector3(6,.57f,-8.8f),ContainerLook.Suitcase,SupplyType.Misc,rng,HarborDistrict.Office,180,false);
   AddContainer(new Vector3(10.8f,0,-7.4f),new Vector3(11.8f,0,-7.4f),ContainerLook.Coat,SupplyType.Misc,rng,HarborDistrict.Office,90,false);
   AddContainer(new Vector3(10.8f,0,-5.2f),new Vector3(10.8f,0,-4),ContainerLook.SupplyLocker,SupplyType.Misc,rng,HarborDistrict.Office);
   LowFurniture("RestTable",new Vector3(6,.32f,-5.1f),new Vector3(1.3f,.64f,.8f),"8B8065");Chair(new Vector3(4.85f,0,-5.1f),90);Chair(new Vector3(7.15f,0,-5.1f),-90);
   // Tools room: two working tables and a spare-parts cupboard.
   AddContainer(new Vector3(-8.8f,0,-1.3f),new Vector3(-8.8f,0,-2.45f),ContainerLook.Toolbox,SupplyType.Tools,rng,HarborDistrict.Repair,180);
   AddContainer(new Vector3(-10.8f,0,1.5f),new Vector3(-10.8f,0,2.55f),ContainerLook.Toolbox,SupplyType.Tools,rng,HarborDistrict.Repair);
   AddContainer(new Vector3(-5.4f,0,1.2f),new Vector3(-5.4f,0,2.55f),ContainerLook.SupplyLocker,SupplyType.Tools,rng,HarborDistrict.Repair);
   LowFurniture("WorkshopBench",new Vector3(-5.6f,.4f,-2.45f),new Vector3(2.2f,.8f,.85f),"697A6B");Box("MotorPart",new Vector3(-5.6f,.97f,-2.45f),new Vector3(.6f,.34f,.45f),"465B55");Chair(new Vector3(-6.1f,0,-1.5f),0);
   // First aid room: cupboards stay attached to the side wall.
   AddContainer(new Vector3(10.6f,0,-1.7f),new Vector3(11.8f,0,-1.7f),ContainerLook.MedicalCabinet,SupplyType.Medical,rng,HarborDistrict.Office,90,false);
   AddContainer(new Vector3(10.6f,0,1.5f),new Vector3(11.8f,0,1.5f),ContainerLook.MedicalCabinet,SupplyType.Medical,rng,HarborDistrict.Office,90,false);
   AddContainer(new Vector3(7.8f,0,1.2f),new Vector3(7.8f,0,2.55f),ContainerLook.SupplyLocker,SupplyType.Medical,rng,HarborDistrict.Office);
   LowFurniture("ClinicBed",new Vector3(6,.36f,-1.8f),new Vector3(2.4f,.72f,1),"B5B79D");Box("ClinicPillow",new Vector3(6.7f,.78f,-1.8f),new Vector3(.6f,.12f,.85f),"D0CFB4");Chair(new Vector3(4.7f,0,1.9f),-25);
   // Records room: cabinets and a case on the records desk.
   AddContainer(new Vector3(-10.8f,0,8.2f),new Vector3(-10.8f,0,9.35f),ContainerLook.FileCabinet,SupplyType.Misc,rng,HarborDistrict.Office);
   AddContainer(new Vector3(-7.9f,0,8.2f),new Vector3(-7.9f,0,9.35f),ContainerLook.FileCabinet,SupplyType.Misc,rng,HarborDistrict.Office);
   AddContainer(new Vector3(-6,0,5.3f),new Vector3(-6,0,4.25f),ContainerLook.Suitcase,SupplyType.Misc,rng,HarborDistrict.Office,180);
   Chair(new Vector3(-7.2f,0,4.5f),90);DecorShelf114(new Vector3(-11.05f,0,5.65f));
   // Main storage: five containers distributed along the perimeter; no extra safe.
   AddContainer(new Vector3(4.4f,0,8.1f),new Vector3(4.4f,0,9.35f),ContainerLook.SupplyLocker,SupplyType.Misc,rng,HarborDistrict.Loading);
   AddContainer(new Vector3(8,0,8.1f),new Vector3(8,0,9.35f),ContainerLook.SupplyLocker,SupplyType.Tools,rng,HarborDistrict.Loading);
   AddContainer(new Vector3(10.3f,0,6.6f),new Vector3(11.4f,0,6.6f),ContainerLook.MilitaryCase,SupplyType.Military,rng,HarborDistrict.Loading,90);
   AddContainer(new Vector3(8.4f,0,5.3f),new Vector3(8.4f,0,4.35f),ContainerLook.AmmoCase,SupplyType.Ammo,rng,HarborDistrict.Loading,180);
   AddContainer(new Vector3(4.4f,0,5.3f),new Vector3(4.4f,0,4.35f),ContainerLook.Toolbox,SupplyType.Tools,rng,HarborDistrict.Loading,180);
   var floors=new GroundSurfaceArt(this,115013);floors.Region(new Rect(-2,-9.91f,4,19.82f),GroundFinish.Terrazzo);for(int i=0;i<6;i++){var r=MainHouseRooms[i];r.xMin=Mathf.Max(-11.91f,r.xMin);r.xMax=Mathf.Min(11.91f,r.xMax);r.yMin=Mathf.Max(-9.91f,r.yMin);r.yMax=Mathf.Min(9.91f,r.yMax);if(i%2==0)r.xMax=-2;else r.xMin=2;floors.Region(r,i==3?GroundFinish.Epoxy:i==2?GroundFinish.PatchedConcrete:i>=4?GroundFinish.Concrete:GroundFinish.BrokenTile);}floors.Bake();new InteriorWastelandArt(this).Build();Physics.SyncTransforms();
  }
  void MainHouseRoomDoor(int room){
   float side=room%2==0?-1:1,x=side*2,z=room<2?-6.6f:room<4?0:6.6f;
   var hinge=new GameObject("MainHouseDoorHinge_"+room).transform;hinge.SetParent(Root,false);hinge.position=new Vector3(x,0,z-.9f);
   var slab=Box("MainHouseRoomDoor_"+room,new Vector3(x,1.075f,z),new Vector3(.12f,2.15f,1.8f),"667B68",true);slab.transform.SetParent(hinge,true);
   var pull=Box("DoorPull",new Vector3(x-side*.11f,.93f,z+.65f),new Vector3(.1f,.18f,.06f),"BCAD81");pull.transform.SetParent(hinge,true);
   Doors.Add(new WorldDoor{Name=MainHouseRoomNames[room]+"门",Position=new Vector3(x,0,z),Pivot=hinge,Collider=slab.GetComponent<Collider>(),ClosedYaw=0,OpenAngle=side*100,RevealRoom=MainHouseRooms[room],ClosedSafety=new Rect(x-.48f,z-1.3f,.96f,2.6f),OpenSafety=new Rect(side<0?x-2.4f:x,z-1.25f,2.4f,2.5f)});
  }
  void NoticeBoard114(Vector3 p){Box("OfficeNoticeBoard",p,new Vector3(1.5f,.85f,.07f),"806F50");for(int i=0;i<3;i++)Box("PinnedPaper",p+new Vector3(-.46f+i*.46f,0,.05f),new Vector3(.34f,.55f,.02f),"BDBBA1");}
  void DecorShelf114(Vector3 p){LowFurniture("RecordsShelfBase",p+Vector3.up*.17f,new Vector3(1,.34f,1.8f),"58695A");foreach(float x in new[]{-.45f,.45f})foreach(float z in new[]{-.84f,.84f})Box("RecordsShelfPost",p+new Vector3(x,.88f,z),new Vector3(.06f,1.76f,.06f),"63776A");foreach(float y in new[]{.5f,1.05f,1.6f})Box("RecordsShelfBoard",p+new Vector3(0,y,0),new Vector3(1,.07f,1.8f),"8A866A");for(int i=0;i<4;i++)Box("RecordBundle",p+new Vector3(0,1.23f,-.62f+i*.41f),new Vector3(.6f,.29f,.27f),"9A977A");}
 }
}
