using UnityEngine;
using System.Collections.Generic;
namespace GreyHarbor {
 public sealed class WorldDoor {
  public ItemKind KeyKind2760=(ItemKind)(-1);
  public readonly List<Rect> ExtraRevealRooms=new List<Rect>();
  public string Name; public string Destination="booth"; public float ClosedYaw,OpenAngle=105; public Rect RevealRoom,ClosedSafety,OpenSafety; public Vector3 Position; public bool Portal,Open,Locked,InstantToggle; public Transform Pivot; public Collider Collider;
 }
 public sealed partial class RaidMap {
  public bool IsInterior; public string InteriorId="",InteriorTitle=""; public Rect InitialRoom; public int LayoutRevision;
  public readonly List<WorldDoor> Doors=new List<WorldDoor>();
  public static readonly Vector3 BoothApproach=new Vector3(8,0,-10.65f);
  public static readonly Rect BoothMain=new Rect(-3.85f,-3.5f,4.99f,7);
  public static readonly Rect BoothStorage=new Rect(1.14f,-3.5f,2.71f,7);
  public void AddBoothEntrance(){
   // Exterior building stays physically solid: the doorway is a map transition.
   Box("BoothEntranceDoor",new Vector3(8,1,-11.47f),new Vector3(1.12f,2,.10f),"536B61");
   Box("BoothDoorHandle",new Vector3(8.36f,.92f,-11.39f),new Vector3(.06f,.19f,.07f),"CAB78D");
   Box("BoothDoorStep",new Vector3(8,.055f,-11.02f),new Vector3(1.5f,.11f,.7f),"9C9B83");
   Doors.Add(new WorldDoor{Name="进入值班亭",Portal=true,Position=new Vector3(8,0,-11.2f)});
  }
  public void BuildBoothInterior(){
   IsInterior=true;InteriorId="booth";InteriorTitle="值班亭";InitialRoom=BoothMain;Bounds=new Rect(-4,-3.7f,8,7.4f);Spawn=new Vector3(-1.25f,0,-2.22f);
   Root=new GameObject("Training_Booth_Interior").transform;
   Box("InteriorFloor",new Vector3(0,-.12f,0),new Vector3(7.6f,.24f,6.8f),"737869");
   InteriorWall(-3.7f,0,.18f,6.6f);InteriorWall(3.7f,0,.18f,6.6f);InteriorWall(0,3.3f,7.6f,.18f);InteriorWall(0,-3.3f,7.6f,.18f);
   InteriorWall(1,-1.9f,.18f,2.6f);InteriorWall(1,2.25f,.18f,2.1f);
   var hinge=new GameObject("StorageDoorHinge").transform;hinge.SetParent(Root,false);hinge.position=new Vector3(1,0,-.6f);
   var slab=Box("StorageDoor",new Vector3(1,1.075f,.3f),new Vector3(.12f,2.15f,1.8f),"667B68",true);slab.transform.SetParent(hinge,true);
   var pull=Box("StorageDoorHandle",new Vector3(.89f,.93f,.95f),new Vector3(.10f,.18f,.06f),"BCAD81");pull.transform.SetParent(hinge,true);
   Doors.Add(new WorldDoor{Name="储物间门",RevealRoom=BoothStorage,ClosedSafety=new Rect(.52f,-.98f,.96f,2.56f),OpenSafety=new Rect(1.15f,-1.4f,1.95f,1.15f),Position=new Vector3(1,0,.3f),Pivot=hinge,Collider=slab.GetComponent<Collider>()});
   Box("ExitDoor",new Vector3(-1.25f,1,-3.17f),new Vector3(1.12f,2,.08f),"536B61");
   Box("ExitDoorHandle",new Vector3(-.87f,.93f,-3.11f),new Vector3(.06f,.18f,.09f),"C5B68E");
   Doors.Add(new WorldDoor{Name="返回室外",Portal=true,Position=new Vector3(-1.25f,0,-3.05f)});
   // Four loot points in the main room and two in the storage room; all against walls.
   var rng=new System.Random(RaidGame.Instance.RaidSeed^111);
   AddContainer(new Vector3(-2.35f,0,1.5f),new Vector3(-2.45f,0,2.45f),ContainerLook.Suitcase,SupplyType.Misc,rng,HarborDistrict.Office);
   AddContainer(new Vector3(.1f,0,1.5f),new Vector3(.18f,0,2.82f),ContainerLook.FileCabinet,SupplyType.Misc,rng,HarborDistrict.Office);
   AddContainer(new Vector3(-2.68f,0,-.5f),new Vector3(-3.53f,0,-.5f),ContainerLook.Coat,SupplyType.Misc,rng,HarborDistrict.Office,-90,false);
   AddContainer(new Vector3(-2.62f,0,-2.08f),new Vector3(-3.52f,0,-2.15f),ContainerLook.MedicalCabinet,SupplyType.Medical,rng,HarborDistrict.Office,-90,false);
   AddContainer(new Vector3(2.48f,0,1.65f),new Vector3(2.6f,0,2.72f),ContainerLook.SupplyLocker,SupplyType.Misc,rng,HarborDistrict.Office);
   AddContainer(new Vector3(2.9f,0,-1.5f),new Vector3(2.55f,0,-2.42f),ContainerLook.Toolbox,SupplyType.Tools,rng,HarborDistrict.Repair,180);
   // Small furnishings make the room readable without narrowing its central path.
   SceneObstacle.Apply(Box("ChairSeat",new Vector3(-3.1f,.40f,1.4f),new Vector3(.44f,.12f,.44f),"796F55",true),ObstacleKind.Low);
   Box("ChairBack",new Vector3(-3.1f,.73f,1.2f),new Vector3(.44f,.55f,.09f),"796F55");
   Box("NoticeBoard",new Vector3(-.8f,1.55f,3.18f),new Vector3(1.22f,.75f,.065f),"7F755B");
   for(int i=0;i<3;i++)Box("PinnedNote",new Vector3(-1.18f+i*.37f,1.56f,3.13f),new Vector3(.27f,.42f,.018f),"C1BCA1");
   Box("EntryMat",new Vector3(-1.25f,.014f,-2.72f),new Vector3(1.4f,.022f,.65f),"485E51");
   
   var floors=new GroundSurfaceArt(this,115011);floors.Region(new Rect(-3.6f,-3.2f,4.6f,6.4f),GroundFinish.BrokenTile);floors.Region(new Rect(1,-3.2f,2.6f,6.4f),GroundFinish.Concrete);floors.Bake();new InteriorWastelandArt(this).Build();Physics.SyncTransforms();
  }
  void InteriorWall(float x,float z,float w,float d){Box("InteriorWall",new Vector3(x,1.15f,z),new Vector3(w,2.3f,d),"949985",true);Box("Skirting",new Vector3(x,.15f,z),new Vector3(w+.015f,.25f,d+.015f),"546557");}
 }
}
