using UnityEngine;
using System.Collections.Generic;
namespace GreyHarbor {
 public sealed partial class RaidMap {
  public readonly List<LootContainer> NorthMiddleLoot143=new List<LootContainer>();
  public readonly List<Transform> PassengerProps143=new List<Transform>();
  public static readonly Vector3 FerryApproach143=PortPoint(185.15f,84);
  public Transform FerryModel143;public WorldDoor FerryEntrance143;
  void AddFerrySurfaces143(){AddHarborFloor("Dock_PassengerFerryDeck143","8D988D",184,67,202,67,202,101,198,109,188,109,184,101);AddHarborFloor("Dock_PassengerGangway143","96A69D",180,83,185.2f,83,185.2f,85.6f,180,85.6f);}
  void BuildNorthMiddleDock143(int seed){
   var k=new DockPassengerKit143(this);var reuse=new DockLoadingKit142(this);var old=new DockLivingKit141(this);
   PassengerProps143.Add(k.FishSortingBench(PortPoint(153,122)));PassengerProps143.Add(k.FishCrates(PortPoint(166,132),0));PassengerProps143.Add(k.IceCart(PortPoint(156.5f,135.6f),90));
   reuse.NetRack(PortPoint(150.5f,126),90);reuse.ShoreUtility(PortPoint(168.5f,119),90);reuse.WasteStation(PortPoint(149,119.5f));reuse.DockLamp(PortPoint(170,134.5f),90);reuse.StrapBasket(PortPoint(177,126.6f));
   DockPallet141(160,135.5f,1.55f,1.0f);DockPallet141(178.5f,126,1.55f,1.0f);DockRope141(163,134,.45f);old.Workbench(PortPoint(164,121));
   PassengerProps143.Add(k.WaitingBench(PortPoint(152,78.2f)));PassengerProps143.Add(k.WaitingBench(PortPoint(152,82),90));PassengerProps143.Add(k.WaitingBench(PortPoint(160,96.8f),180));PassengerProps143.Add(k.Kiosk(PortPoint(167.5f,76),180));PassengerProps143.Add(k.LuggageTrolley(PortPoint(167.5f,88),0));PassengerProps143.Add(k.TicketGate(PortPoint(174.7f,83.7f),90));
   reuse.NoticeBoard(PortPoint(161.8f,75.5f));reuse.WasteStation(PortPoint(150,96));reuse.ShoreUtility(PortPoint(169.5f,90),90);reuse.DockLamp(PortPoint(170,96),90);old.Workbench(PortPoint(152,85.3f));
   DockPallet141(164,86.5f,1.55f,1.05f);DockPallet141(160,77,1.55f,1.05f);
   FerryModel143=new FerryLivingKit144(this).FerryHull(PortPoint(193,88));PassengerProps143.Add(FerryModel143);PassengerProps143.Add(k.Gangway(PortPoint(182.6f,84.3f),true));
   FerryEntrance143=new WorldDoor{Name="登船 · 客船一楼",Destination="ferry1",Portal=true,Position=FerryApproach143};Doors.Add(FerryEntrance143);
   DockBasePaint141("Dock_PassengerFerryDeck143","8D988D");DockBasePaint141("Dock_PassengerGangway143","96A69D");
   for(float x=180.2f;x<185.2f;x+=.34f)PaintDockRect141(x,84.3f,.035f,2.35f,"647C72",.052f);
   for(float z=69;z<98;z+=2)PaintDockRect141(184.8f,z,.12f,.65f,"C6BC8B",.052f);
   PaintDockRect141(190,101,7.4f,.08f,"647E80",.052f);PaintDockRect141(176,83.7f,3,.08f,"B3A574",.052f);
   for(int i=0;i<4;i++)NorthMiddleLoot143.Add(DockLoot141[i]);var rng=new System.Random(seed^143071);
   ExtraLoot143(153,120.7f,153,122,.99f,ContainerLook.FishingChest,0,rng);
   ExtraLoot143(160,134.3f,160,135.5f,.10f,ContainerLook.MarineCooler,0,rng);
   ExtraLoot143(164,122.3f,164,121,.90f,ContainerLook.WaterproofTransitCase,180,rng);
   ExtraLoot143(178.5f,124.7f,178.5f,126,.10f,ContainerLook.FishingChest,0,rng);
   ExtraLoot143(151.7f,80,150.5f,80,0,ContainerLook.RollingSuitcase,-90,rng);
   ExtraLoot143(152,86.5f,152,85.3f,.90f,ContainerLook.WaterproofTransitCase,180,rng);
   ExtraLoot143(164,85.3f,164,86.5f,.10f,ContainerLook.CargoCrate,0,rng);
   ExtraLoot143(160,78.3f,160,77,.10f,ContainerLook.RollingSuitcase,180,rng);
   DressPassengerPier144();Physics.SyncTransforms();
  }
  void ExtraLoot143(float sx,float sz,float x,float z,float height,ContainerLook look,float yaw,System.Random rng){DockLootPoint141(sx,sz,x,z,height,look,yaw,rng);NorthMiddleLoot143.Add(DockLoot141[DockLoot141.Count-1]);}
 }
}
