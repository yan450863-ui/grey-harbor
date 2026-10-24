using System.Collections.Generic;
using UnityEngine;
namespace GreyHarbor {
 public sealed class UnloadingExteriorRecord2724 {
  public string Type,Group;public GameObject View;public Rect Footprint;public Vector3 Anchor;public float Yaw;
 }
 public sealed partial class RaidMap {
  public readonly List<UnloadingExteriorRecord2724> UnloadingExteriorRecords2724=new List<UnloadingExteriorRecord2724>();
  public readonly List<Rect> UnloadingProtectedAreas2724=new List<Rect>();
  public readonly List<FreightLootRecord2717> UnloadingOutdoorSearch2724=new List<FreightLootRecord2717>();
  public GameObject UnloadingTruck2724,UnloadingCanopy2724;
  static Rect UnloadingDesignRect2724(float x,float z,float w,float d)=>new Rect(x-79,z-73,w,d);
  void BuildUnloadingExteriorArt2724(){
   UnloadingProtectedAreas2724.Add(UnloadingDesignRect2724(125,137,6,22));
   UnloadingProtectedAreas2724.Add(UnloadingDesignRect2724(120,140.1f,2,1.9f));
   UnloadingProtectedAreas2724.Add(UnloadingDesignRect2724(127,158,4,2));
   UnloadingProtectedAreas2724.Add(UnloadingDesignRect2724(131.5f,149.5f,2,10.2f));
   foreach(var plan in BuildingPlans2723)UnloadingFacade2724(plan);
   UnloadingCanopyBuild2724();UnloadingTruckBuild2724();
   var kit=new FutureAssetKit2717(this);var dock=new DockLoadingKit142(this);
   UnloadingTrack2724(dock.Forklift(PortPoint(121,154),-5).gameObject,"叉车","仓库收货");
   UnloadingTrack2724(kit.Build(FutureProp2717.WrappedPallet,PortPoint(141.1f,146.1f),7),"打包货垛","雨棚暂存");
   UnloadingTrack2724(kit.Build(FutureProp2717.WrappedPallet,PortPoint(137.3f,147.3f),-8),"篷布货垛","雨棚暂存");
   UnloadingTrack2724(kit.Build(FutureProp2717.CargoCage,PortPoint(141.8f,148.1f),4),"待运货笼","雨棚暂存");
   UnloadingTrack2724(kit.Build(FutureProp2717.OpenCargoCrate,PortPoint(135.9f,145.6f),-12),"开盖木货箱","雨棚验货");
   UnloadingTrack2724(kit.Build(FutureProp2717.PalletJack,PortPoint(139.2f,143.7f),18),"液压托盘车","雨棚作业");
   UnloadingTrack2724(dock.StrapBasket(PortPoint(143.1f,144.5f),13).gameObject,"捆扎带篮","雨棚作业");
   UnloadingTrack2724(kit.Build(FutureProp2717.CargoStraps,PortPoint(140.2f,147.5f),-15),"卸下的绑带","雨棚验货");
   UnloadingTrack2724(dock.SpoolStack(PortPoint(143.1f,152.7f),-5).gameObject,"电缆盘","东侧备货");
   UnloadingTrack2724(kit.Build(FutureProp2717.WrappedPallet,PortPoint(143.2f,155.6f),11),"待入库货垛","东侧备货");
   UnloadingTrack2724(kit.Build(FutureProp2717.LoadingScale,PortPoint(134.8f,151.7f),90),"地磅秤台","收货称重");
   UnloadingTrack2724(kit.Build(FutureProp2717.HandTruck,PortPoint(133.9f,156.6f),-12),"手推搬运车","收货称重");
   UnloadingTrack2724(kit.Build(FutureProp2717.ToolTrolley,PortPoint(122.7f,150.2f),18),"叉车工具车","叉车维护");
   UnloadingTrack2724(kit.Build(FutureProp2717.ForkliftBattery,PortPoint(119.4f,150.9f),-5),"备用电池","叉车维护");
   UnloadingTrack2724(kit.Build(FutureProp2717.WheelChocks,PortPoint(137.1f,152.6f),0),"停车止轮块","货车停车");
   UnloadingTrack2724(dock.NoticeBoard(PortPoint(123.6f,140.7f),0).gameObject,"值班公告牌","值班房门前");
   UnloadingTrack2724(dock.WasteStation(PortPoint(118.7f,139.7f),-8).gameObject,"分类废料桶","值班房侧边");
   UnloadingTrack2724(dock.BarrelRack(PortPoint(118.7f,157.4f),8).gameObject,"维护油桶架","叉车维护");
   UnloadingPalletStack2724(PortPoint(123.1f,157.6f));
   var rng=new System.Random((RaidGame.Instance?RaidGame.Instance.RaidSeed:2724)^2724);
   UnloadingSearchAdd2724("雨棚打包耗材箱",ContainerLook.OutdoorTote,PortPoint(135.9f,148.1f),0,PortPoint(135.9f,146.8f),PortPoint(132.5f,146.8f),"雨棚暂存",rng);
   UnloadingSearchAdd2724("待转运木箱",ContainerLook.CargoCrate,PortPoint(143.4f,158.5f),90,PortPoint(142.1f,158.5f),PortPoint(141.1f,159.25f),"仓库收货",rng);
   UnloadingSearchAdd2724("叉车检修工具箱",ContainerLook.WeldingToolbox,PortPoint(122.7f,152.1f),270,PortPoint(124,152.1f),PortPoint(128,152.1f),"叉车维护",rng);
   Physics.SyncTransforms();
  }
  void UnloadingSearchAdd2724(string name,ContainerLook look,Vector3 model,float yaw,Vector3 stand,Vector3 access,string group,System.Random rng){
   var box=AddContainer(stand,model,look,SupplyType.Misc,rng,HarborDistrict.Office,yaw,false,ContainerTier.Common,DropRegion230.Loading);box.Name=name;box.View.transform.SetParent(UnloadingRoot2723,true);
   UnloadingOutdoorSearch2724.Add(new FreightLootRecord2717{Container=box,Model=box.View,Stand=stand,AccessPoint=access,RoomName=group,Yaw=yaw,WallMounted=false});
  }
  void UnloadingTrack2724(GameObject view,string type,string group){
   view.transform.SetParent(UnloadingRoot2723,true);Physics.SyncTransforms();
   var b=CargoBounds2714(view);UnloadingExteriorRecords2724.Add(new UnloadingExteriorRecord2724{Type=type,Group=group,View=view,Anchor=view.transform.position,Yaw=view.transform.eulerAngles.y,Footprint=b});
  }
  GameObject UnloadingArtRoot2724(string name,Vector3 position){var g=new GameObject("Unloading2724_"+name);g.transform.SetParent(UnloadingRoot2723,false);g.transform.position=position;return g;}
  void UnloadingHit2724(GameObject parent,string name,Vector3 p,Vector3 size,ObstacleKind kind){var g=new GameObject(name);g.transform.SetParent(parent.transform,false);g.transform.localPosition=p;g.AddComponent<BoxCollider>().size=size;SceneObstacle.Apply(g,kind);}
  void UnloadingBake2724(GameObject g,WorkshopWastelandArt art){art.Bake(g,g.AddComponent<WorkshopArtResources>());}
  void UnloadingFacade2724(UnloadingBuildingPlan2723 plan){
   bool warehouse=plan.Key=="unloadingwarehouse";var r=plan.Footprint;float w=r.width,d=r.height,h=plan.Height;
   plan.View.GetComponent<Renderer>().sharedMaterial=Mat(warehouse?"81948B":"B1B09B",true);
   var g=UnloadingArtRoot2724(plan.Name+"_外墙屋顶",new Vector3(r.center.x,0,r.center.y));var art=new WorkshopWastelandArt(this);
   // Batched cosmetic shell surfaces add no duplicate building collision.
   foreach(float z in new[]{-d/2-.03f,d/2+.03f}){
    art.Cube("WeatheredBase","667A70",new Vector3(0,.35f,z),new Vector3(w,.5f,.07f));
    art.Cube("UpperEave","C1C2AC",new Vector3(0,h-.10f,z),new Vector3(w+.20f,.12f,.13f));
    if(warehouse)for(float x=-w/2+.3f;x<w/2;x+=.65f)art.Cube("MetalFacadeSeam","A0AC9C",new Vector3(x,h/2,z-.025f),new Vector3(.035f,h-.5f,.035f));
   }
   foreach(float x in new[]{-w/2-.03f,w/2+.03f}){
    art.Cube("WeatheredSideBase","667A70",new Vector3(x,.35f,0),new Vector3(.07f,.5f,d));
    foreach(float z in new[]{-d/2+.12f,d/2-.12f})art.Cube("RainwaterDownpipe","50685E",new Vector3(x,h/2,z),new Vector3(.09f,h,.09f));
   }
   // Low pitched sheets read as a warehouse roof from the outdoor camera.
   float half=d/2+.23f,rise=warehouse?.65f:.4f;
   foreach(int side in new[]{-1,1}){
    Vector3 along=new Vector3(0,-side*rise,half).normalized;
    art.Panel("ColoredRoofSheet",warehouse?"597C78":"748576",new Vector3(0,h+rise/2+.10f,side*half/2),Vector3.right,along,w+.46f,Mathf.Sqrt(half*half+rise*rise));
    for(float x=-w/2;x<=w/2;x+=.55f)art.Tube("StandingRoofSeam","93A49A",new Vector3(x,h+.13f,side*half),new Vector3(x,h+rise+.13f,0),.015f);
   }
   art.Cube("RoofRidgeCap","A6B0A1",new Vector3(0,h+rise+.15f,0),new Vector3(w+.52f,.08f,.13f));
   float windowY=warehouse?3.6f:1.9f;
   foreach(float x in warehouse?new[]{-7.5f,-3.5f,3.5f,7.5f}:new[]{-2.3f,2.3f}){
    float ww=warehouse?2.1f:1.5f,wh=warehouse?.65f:.95f;
    art.Cube("IndustrialWindowFrame","4A655D",new Vector3(x,windowY,-d/2-.07f),new Vector3(ww+.15f,wh+.15f,.12f));
    art.Cube("MutedTealGlass","6A8B8E",new Vector3(x,windowY,-d/2-.145f),new Vector3(ww,wh,.025f));
    art.Cube("WindowMullion","ADB7A7",new Vector3(x,windowY,-d/2-.165f),new Vector3(.045f,wh,.025f));
   }
   float dx=plan.Door.x-r.center.x,dz=plan.Door.z-r.center.y,width=warehouse?4:1.8f,dh=warehouse?3.3f:2.5f;
   if(warehouse)for(int i=0;i<19;i++)art.Cube("RollingShutterRib","AAB7A3",new Vector3(dx,.14f+i*.165f,dz-.145f),new Vector3(width-.14f,.033f,.025f));
   else{art.Cube("DoorGlass","435F64",new Vector3(dx,1.75f,dz-.145f),new Vector3(1.3f,.68f,.025f));art.Cube("DoorHandle","C8BE99",new Vector3(dx+.58f,1.05f,dz-.18f),new Vector3(.06f,.28f,.06f));}
   art.Cube("DoorLampHousing","8F9E87",new Vector3(dx,dh+.35f,dz-.22f),new Vector3(.5f,.14f,.28f));
   art.Cube("DoorLampLens","D4C58C",new Vector3(dx,dh+.27f,dz-.24f),new Vector3(.35f,.025f,.18f));
   art.Cube("EntranceSignBacking","304D48",new Vector3(dx,dh+.62f,dz-.08f),new Vector3(width*.65f,.25f,.06f));
   for(int i=0;i<5;i++)art.Cube("FadedSignLetter","C5C7AC",new Vector3(dx+(i-2)*width*.08f,dh+.62f,dz-.12f),new Vector3(width*.045f,.13f,.015f));
   UnloadingBake2724(g,art);
  }
  void UnloadingCanopyBuild2724(){
   var g=UnloadingArtRoot2724("SteelRainShelter",PortPoint(139,146));UnloadingCanopy2724=g;var art=new WorkshopWastelandArt(this);
   foreach(float x in new[]{-4.8f,4.8f})foreach(float z in new[]{-2.8f,2.8f}){
    art.Cube("RustPatchedPost","6D837C",new Vector3(x,1.8f,z),new Vector3(.18f,3.6f,.18f));
    art.Cube("PostBase","A69C7E",new Vector3(x,.13f,z),new Vector3(.32f,.26f,.32f));
    art.Cube("PostRust","916F4F",new Vector3(x,.42f,z-.095f),new Vector3(.15f,.25f,.012f));
    UnloadingHit2724(g,"PostCollision",new Vector3(x,1.8f,z),new Vector3(.22f,3.6f,.22f),ObstacleKind.High);
   }
   // Keep the near face open. Narrow pitched roof strips cover the rear third,
   // leaving all cargo and the two-metre handling aisle visible from four views.
   foreach(float z in new[]{-2.8f,2.8f})art.Cube("SteelEave","55766D",new Vector3(0,3.6f,z),new Vector3(9.8f,.13f,.12f));
   foreach(float x in new[]{-4.8f,4.8f})art.Cube("SideRoofRail","819789",new Vector3(x,3.6f,0),new Vector3(.12f,.13f,5.8f));
   for(int i=0;i<3;i++)art.Cube("RemainingRoofSheet",i==1?"8C9883":"668C87",new Vector3(-3.2f+i*3.2f,3.66f,1.9f),new Vector3(3.1f,.065f,1.5f));
   art.Cube("DamagedRoofSheet","976F4E",new Vector3(2.7f,3.66f,1.92f),new Vector3(.55f,.018f,.85f));
   UnloadingBake2724(g,art);UnloadingTrack2724(g,"开放式残损雨棚","雨棚暂存");
  }
  void UnloadingTruckBuild2724(){
   var g=UnloadingArtRoot2724("ParkedFreightTruck",PortPoint(139,155));UnloadingTruck2724=g;var art=new WorkshopWastelandArt(this);
   art.Cube("TruckChassis","354D49",new Vector3(0,.65f,0),new Vector3(2.25f,.30f,6.9f));
   art.Cube("TruckCab","527B79",new Vector3(0,1.45f,-2.55f),new Vector3(2.4f,1.75f,1.8f));
   art.Cube("CabRoof","91A292",new Vector3(0,2.35f,-2.55f),new Vector3(2.46f,.11f,1.88f));
   art.Cube("Windshield","344E55",new Vector3(0,1.88f,-3.46f),new Vector3(1.9f,.72f,.028f));
   art.Cube("Bumper","9AAB9A",new Vector3(0,.78f,-3.53f),new Vector3(2.48f,.20f,.17f));
   art.Cube("Grille","2D4442",new Vector3(0,1.08f,-3.466f),new Vector3(1.4f,.28f,.025f));
   foreach(float x in new[]{-1.21f,1.21f}){art.Cube("SideGlass","4D7175",new Vector3(x,1.88f,-2.58f),new Vector3(.028f,.66f,1.14f));art.Cube("Mirror","7F978D",new Vector3(x*1.04f,1.96f,-3.13f),new Vector3(.10f,.24f,.15f));}
   foreach(float x in new[]{-.94f,.94f})art.Cube("Headlights","CEBC83",new Vector3(x,1.09f,-3.477f),new Vector3(.29f,.21f,.026f));
   art.Cube("FreightBody","92927B",new Vector3(0,1.8f,.96f),new Vector3(2.45f,2.10f,4.92f));
   foreach(float x in new[]{-1.24f,1.24f})for(int i=0;i<13;i++)art.Cube("CargoSideRib","B1B398",new Vector3(x,1.78f,-1.36f+i*.38f),new Vector3(.035f,1.94f,.045f));
   art.Cube("RearDoor","6F857C",new Vector3(0,1.8f,3.432f),new Vector3(2.3f,1.94f,.03f));
   foreach(float x in new[]{-.60f,.60f})art.Cube("RearLockingBar","B4BAA2",new Vector3(x,1.8f,3.46f),new Vector3(.05f,1.88f,.045f));
   foreach(float x in new[]{-1.14f,1.14f})foreach(float z in new[]{-2.5f,1.72f,2.75f}){art.Tube("RubberWheel","293835",new Vector3(x-.14f,.44f,z),new Vector3(x+.14f,.44f,z),.44f);art.Cube("WheelHub","849184",new Vector3(x*1.12f,.44f,z),new Vector3(.018f,.24f,.24f));}
   UnloadingHit2724(g,"TruckBodyCollision",new Vector3(0,1.40f,0),new Vector3(2.52f,2.80f,7.08f),ObstacleKind.High);
   UnloadingBake2724(g,art);UnloadingTrack2724(g,"停靠货车","货车停车");
  }
  void UnloadingPalletStack2724(Vector3 p){
   var g=UnloadingArtRoot2724("EmptyPalletStack",p);var art=new WorkshopWastelandArt(this);
   for(int layer=0;layer<4;layer++){
    float y=.08f+layer*.18f;foreach(float z in new[]{-.43f,.43f})art.Cube("TimberBearer","705E42",new Vector3(0,y-.02f,z),new Vector3(1.45f,.1f,.12f));
    for(int i=0;i<6;i++)art.Cube("UnevenTimberSlat",i%3==0?"AF9368":"94764D",new Vector3(-.62f+i*.25f,y+.06f,0),new Vector3(.20f,.065f,1.1f));
   }
   UnloadingHit2724(g,"PalletCollision",Vector3.up*.37f,new Vector3(1.48f,.74f,1.14f),ObstacleKind.Low);g.transform.rotation=Quaternion.Euler(0,-7,0);
   UnloadingBake2724(g,art);UnloadingTrack2724(g,"空托盘垛","仓库收货");
  }
 }
}
