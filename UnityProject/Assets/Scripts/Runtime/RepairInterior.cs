using UnityEngine;
namespace GreyHarbor {
 public sealed partial class RaidMap {
  public static readonly Vector3 RepairApproach=new Vector3(-8.7f,0,1);
  public static readonly Rect RepairMain=new Rect(-4.6f,-7.1f,9.2f,8.5f),RepairParts=new Rect(-4.6f,1.4f,9.2f,5.7f);
  public void AddRepairEntrance(){Doors.Add(new WorldDoor{Name="进入维修棚",Destination="repair",Portal=true,Position=new Vector3(-9.5f,0,1)});}
  public void BuildRepairInterior(){
   IsInterior=true;InteriorId="repair";InteriorTitle="维修棚";InitialRoom=RepairMain;Bounds=new Rect(-4.8f,-7.3f,9.6f,14.6f);Spawn=new Vector3(3.35f,0,-2);
   Root=new GameObject("Training_Repair_Interior").transform;
   Box("WorkshopFloor",new Vector3(0,-.12f,0),new Vector3(9.1f,.24f,14.1f),"69726B");
   InteriorWall(-4.5f,0,.18f,14.1f);InteriorWall(4.5f,0,.18f,14.1f);InteriorWall(0,-7,9.1f,.18f);InteriorWall(0,7,9.1f,.18f);
   InteriorWall(-2.65f,1.3f,3.7f,.18f);InteriorWall(2.75f,1.3f,3.5f,.18f);
   var hinge=new GameObject("PartsRoomDoorHinge").transform;hinge.SetParent(Root,false);hinge.position=new Vector3(-.8f,0,1.3f);
   var slab=Box("PartsRoomDoor",new Vector3(-.8f,1.075f,2.2f),new Vector3(.12f,2.15f,1.8f),"667B68",true);slab.transform.SetParent(hinge,true);
   var pull=Box("DoorPull",new Vector3(-.91f,.93f,2.85f),new Vector3(.10f,.18f,.06f),"BCAD81");pull.transform.SetParent(hinge,true);hinge.localRotation=Quaternion.Euler(0,90,0);
   Doors.Add(new WorldDoor{Name="备件间门",Position=new Vector3(.1f,0,1.3f),Pivot=hinge,Collider=slab.GetComponent<Collider>(),ClosedYaw=90,OpenAngle=-105,RevealRoom=RepairParts,ClosedSafety=new Rect(-1.2f,.82f,2.6f,.96f),OpenSafety=new Rect(-1.7f,1.15f,1.25f,2.5f)});
   Box("RepairExit",new Vector3(4.36f,1,-2),new Vector3(.07f,2,1.2f),"536B61");Box("ExitPull",new Vector3(4.28f,.92f,-1.65f),new Vector3(.07f,.18f,.06f),"BCAA80");
   Doors.Add(new WorldDoor{Name="返回室外",Portal=true,Destination="repair",Position=new Vector3(4.15f,0,-2)});
   // 9 x 14 m footprint (2.1x original). Six containers preserve the accepted sparse density.
   var rng=new System.Random(RaidGame.Instance.RaidSeed^112);
   LowFurniture("WorkBench",new Vector3(-3.70f,.4f,-2.4f),new Vector3(.95f,.8f,2.6f),"495D53");Box("BenchTop",new Vector3(-3.70f,.86f,-2.4f),new Vector3(1.04f,.12f,2.68f),"9B8D6B");
   AddContainer(new Vector3(-2.62f,0,-2.2f),new Vector3(-3.68f,.93f,-2.2f),ContainerLook.Toolbox,SupplyType.Tools,rng,HarborDistrict.Repair,-90,false);
   AddContainer(new Vector3(-3.45f,0,.2f),new Vector3(-4.30f,0,.2f),ContainerLook.Coat,SupplyType.Misc,rng,HarborDistrict.Repair,-90,false);
   AddContainer(new Vector3(2.9f,0,5.2f),new Vector3(2.9f,0,6.45f),ContainerLook.SupplyLocker,SupplyType.Tools,rng,HarborDistrict.Repair);
   AddContainer(new Vector3(-2,0,-4.8f),new Vector3(-2,0,-5.9f),ContainerLook.Toolbox,SupplyType.Tools,rng,HarborDistrict.Repair,180);
   AddContainer(new Vector3(3.25f,0,-4.5f),new Vector3(4.3f,0,-4.5f),ContainerLook.MedicalCabinet,SupplyType.Medical,rng,HarborDistrict.Repair,90,false);
   AddContainer(new Vector3(-.4f,0,5.2f),new Vector3(-.4f,0,6.45f),ContainerLook.FileCabinet,SupplyType.Misc,rng,HarborDistrict.Repair);
   Chair(new Vector3(.3f,0,-5.8f),20);Chair(new Vector3(1.25f,0,-6.25f),-30);
   Box("ToolPegboard",new Vector3(-4.35f,1.55f,-2.4f),new Vector3(.06f,.8f,2.6f),"5D7062");
   for(int i=0;i<5;i++){Box("HangingTool",new Vector3(-4.29f,1.65f,-3.3f+i*.42f),new Vector3(.05f,.34f,.06f),"ACAF9C");Box("ToolHead",new Vector3(-4.27f,1.85f,-3.3f+i*.42f),new Vector3(.07f,.10f,.17f),"87988D");}
   Box("BenchVise",new Vector3(-3.62f,1.02f,-3.3f),new Vector3(.36f,.2f,.29f),"657B73");
   LowFurniture("ServiceCart",new Vector3(3.6f,.37f,-.1f),new Vector3(.85f,.74f,1.1f),"5D7268");Box("CartTop",new Vector3(3.6f,.79f,-.1f),new Vector3(.93f,.09f,1.18f),"8B9682");Box("MotorHousing",new Vector3(3.6f,.96f,-.1f),new Vector3(.47f,.28f,.65f),"465B55");
   foreach(float z in new[]{-.46f,.25f})foreach(float x in new[]{3.3f,3.9f})Box("CartWheel",new Vector3(x,.12f,z),new Vector3(.12f,.24f,.20f),"293C35");
   // Open shelving with visible spare parts; no loot or F prompt.
   LowFurniture("ShelfBase",new Vector3(-3.65f,.18f,5.9f),new Vector3(1.1f,.36f,1.45f),"58695A");
   foreach(float x in new[]{-4.14f,-3.16f})foreach(float z in new[]{5.25f,6.55f})Box("ShelfPost",new Vector3(x,.85f,z),new Vector3(.07f,1.7f,.07f),"63776A");
   foreach(float y in new[]{.5f,1.05f,1.6f})Box("ShelfBoard",new Vector3(-3.65f,y,5.9f),new Vector3(1.1f,.07f,1.45f),"8A866A");
   for(int i=0;i<3;i++)Box("SpareParts",new Vector3(-3.7f,1.22f,5.4f+i*.43f),new Vector3(.55f,.25f,.26f),"718276");
   Box("WornRug",new Vector3(.65f,.025f,-6.1f),new Vector3(2.05f,.025f,1.45f),"526355");var floors=new GroundSurfaceArt(this);floors.Region(new Rect(-4.41f,-6.91f,8.82f,8.21f),GroundFinish.PatchedConcrete);floors.Region(new Rect(-4.41f,1.3f,8.82f,5.61f),GroundFinish.BrokenTile);floors.Bake();new WorkshopWastelandArt(this).Build();Physics.SyncTransforms();
  }
  void LowFurniture(string name,Vector3 p,Vector3 size,string color){SceneObstacle.Apply(Box(name,p,size,color,true),ObstacleKind.Low);}
  void Chair(Vector3 p,float yaw){var root=new GameObject("Decor_Chair").transform;root.SetParent(Root,false);root.position=p;var seat=Box("ChairSeat",p+Vector3.up*.43f,new Vector3(.53f,.12f,.53f),"827657",true);SceneObstacle.Apply(seat,ObstacleKind.Low);seat.transform.SetParent(root,true);var back=Box("ChairBack",p+new Vector3(0,.74f,-.23f),new Vector3(.53f,.6f,.08f),"827657");back.transform.SetParent(root,true);foreach(float x in new[]{-.21f,.21f})foreach(float z in new[]{-.21f,.21f}){var leg=Box("ChairLeg",p+new Vector3(x,.20f,z),new Vector3(.06f,.4f,.06f),"4A6053");leg.transform.SetParent(root,true);}root.rotation=Quaternion.Euler(0,yaw,0);}
 }
}
