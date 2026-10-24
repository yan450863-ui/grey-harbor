using UnityEngine;
using System.Collections.Generic;
namespace GreyHarbor {
 public sealed partial class RaidMap {
  public readonly List<LootContainer> SouthDockLoot142=new List<LootContainer>();
  public readonly List<Transform> LoadingProps142=new List<Transform>();
  void BuildSouthDock142(int seed){
   var k=new DockLoadingKit142(this);var old=Root.Find("DockHighCargoStack");if(old){old.gameObject.SetActive(false);TransientObjects2721.Release(old.gameObject);}var band=Root.Find("DockStackBand");if(band){band.gameObject.SetActive(false);TransientObjects2721.Release(band.gameObject);}
   LoadingProps142.Add(k.WrappedLoad(PortPoint(165,24),0));LoadingProps142.Add(k.WrappedLoad(PortPoint(153,21.5f),8));
   LoadingProps142.Add(k.Forklift(PortPoint(154,26),90));LoadingProps142.Add(k.PalletJack(PortPoint(159,25.5f),25));
   LoadingProps142.Add(k.BarrelRack(PortPoint(150,29),0));LoadingProps142.Add(k.ShoreUtility(PortPoint(176,35.5f),90));
   LoadingProps142.Add(k.NetRack(PortPoint(153,41),0));LoadingProps142.Add(k.WasteStation(PortPoint(148.2f,36.6f),90));LoadingProps142.Add(k.WasteStation(PortPoint(181.9f,18.2f),0));
   LoadingProps142.Add(k.DockLamp(PortPoint(146.8f,21),-90));LoadingProps142.Add(k.DockLamp(PortPoint(182.25f,39),90));
   LoadingProps142.Add(k.SpoolStack(PortPoint(178.7f,24.2f),0));LoadingProps142.Add(k.StrapBasket(PortPoint(174.6f,40.4f),20));LoadingProps142.Add(k.NoticeBoard(PortPoint(151,39.5f),-90));LoadingProps142.Add(k.PipeBundle(PortPoint(178.4f,32),0));
   var baseKit=new DockLivingKit141(this);baseKit.Workbench(PortPoint(156,30.7f));baseKit.CargoTrolley(PortPoint(168.5f,21),-10);baseKit.Bollard(PortPoint(186.8f,10.3f));
   DockPallet141(155.25f,21.7f,1.55f,1.05f);DockPallet141(160,20.5f,1.65f,1.30f);DockPallet141(174.8f,38.4f,2.05f,.82f);DockPallet141(185.8f,21,1.55f,1.05f);
   DockRope141(162,34,.55f);DockRope141(170,39.5f,.43f);DockRope141(184,18,.44f);
   // All extra ground marks are baked into the existing stable texture, never stacked planes.
   PaintDockRect141(150.0f,31,0.26f,23,"4D6256",.049f);for(float z=19.8f;z<42;z+=.23f)PaintDockRect141(150,z,.22f,.06f,"8D9580",.050f);
   for(float z=22;z<31;z+=.33f)foreach(float x in new[]{153.15f,154.8f})PaintDockRect141(x,z,.12f,.24f,"667560",.049f);
   for(int i=0;i<9;i++)PaintDockRect141(155.4f+i*.26f,19.8f,.12f,.40f,"B1A06A",.051f);
   PaintDockRect141(173.7f,35.5f,3.1f,.07f,"AE9F6C",.050f);PaintDockRect141(173.7f,28.1f,.07f,2.2f,"AE9F6C",.050f);
   var decor=new GameObject("SouthDockLooseObjects142");decor.transform.SetParent(Root,false);var art=new WorkshopWastelandArt(this);var rng=new System.Random(142117);foreach(var center in new[]{PortPoint(149,20),PortPoint(152,32.8f),PortPoint(176,37.9f),PortPoint(181,20)})for(int i=0;i<8;i++){var p=center+new Vector3((float)rng.NextDouble()*.8f-.4f,.045f,(float)rng.NextDouble()*.8f-.4f);art.Cube(i%2==0?"DiscardedTimber":"RustyScrap",i%2==0?"9A8B67":"8A8063",p,new Vector3(.12f+(float)rng.NextDouble()*.20f,.035f,.05f+(float)rng.NextDouble()*.10f),false);}art.Bake(decor,decor.AddComponent<WorkshopArtResources>());
   for(int i=4;i<8;i++)SouthDockLoot142.Add(DockLoot141[i]);var random=new System.Random(seed^142082);
   SouthLootPoint142(155.25f,22.9f,155.25f,21.7f,.10f,ContainerLook.WaterproofTransitCase,180,random);
   SouthLootPoint142(156,29.45f,156,30.7f,.90f,ContainerLook.WeldingToolbox,0,random);
   SouthLootPoint142(160,21.75f,160,20.5f,.12f,ContainerLook.CargoCrate,180,random);
   SouthLootPoint142(149.85f,25.1f,148.6f,25.1f,0,ContainerLook.FuelDrum,-90,random);
   SouthLootPoint142(175.4f,28.1f,176.6f,28.1f,0,ContainerLook.RescueCabinet,90,random);
   SouthLootPoint142(174.8f,37.2f,174.8f,38.4f,.10f,ContainerLook.FishingChest,0,random);
   SouthLootPoint142(185.8f,19.8f,185.8f,21,.10f,ContainerLook.WaterproofTransitCase,0,random);
   SouthLootPoint142(157.5f,39.55f,157.5f,38.3f,0,ContainerLook.GardenChest,180,random);
  }
  void SouthLootPoint142(float sx,float sz,float x,float z,float height,ContainerLook look,float yaw,System.Random rng){DockLootPoint141(sx,sz,x,z,height,look,yaw,rng);SouthDockLoot142.Add(DockLoot141[DockLoot141.Count-1]);}
 }
}
