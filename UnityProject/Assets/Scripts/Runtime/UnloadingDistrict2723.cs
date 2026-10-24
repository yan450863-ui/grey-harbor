using System.Collections.Generic;
using UnityEngine;

namespace GreyHarbor {
 public sealed class UnloadingBuildingPlan2723 {
  public string Key,Name;public Rect Footprint;public GameObject View;public Vector3 Door,Arrival;public float Height;
 }
 public sealed class UnloadingPlaceholder2723 {
  public string Type;public GameObject View;public Rect Footprint;
 }
 public sealed partial class RaidMap {
  // Coordinates are converted from the agreed harbour design drawing to world metres.
  public static readonly Rect UnloadingBounds2723=new Rect(117-79,139-73,28,36);
  public static readonly Vector3 UnloadingPreviewSpawn2723=PortPoint(128,153);
  public static readonly Vector3 UnloadingWarehouseDoor2723=PortPoint(129,160);
  public static readonly Vector3 UnloadingWarehouseArrival2723=PortPoint(129,159f);
  public static readonly Vector3 UnloadingDutyDoor2723=PortPoint(121,142);
  public static readonly Vector3 UnloadingDutyArrival2723=PortPoint(121,141f);
  public readonly List<UnloadingBuildingPlan2723> BuildingPlans2723=new List<UnloadingBuildingPlan2723>();
  public readonly List<UnloadingPlaceholder2723> PlaceholderRecords2723=new List<UnloadingPlaceholder2723>();
  public Transform UnloadingRoot2723;

  public void BuildUnloadingDistrict2723(){
   if(UnloadingRoot2723)return;
   UnloadingRoot2723=new GameObject("UnloadingDistrict2724").transform;UnloadingRoot2723.SetParent(Root,false);
   UnloadingBuilding2723("unloadingwarehouse","装卸仓库",120,160,22,14,4.5f,UnloadingWarehouseDoor2723,UnloadingWarehouseArrival2723,4);
   UnloadingBuilding2723("unloadingduty","装卸值班间",117,142,8,6,3.4f,UnloadingDutyDoor2723,UnloadingDutyArrival2723,1.8f);
   var warehouse=BuildingPlans2723[0];warehouse.View.AddComponent<FutureMainBuilding>().Floors=1;
   MainBuildings.Add(new MainBuildingPlan("装卸仓库",warehouse.Footprint,1,warehouse.Height,warehouse.Arrival));
   BuildUnloadingExteriorArt2724();
  }
  GameObject UnloadingCube2723(string name,Vector3 position,Vector3 size,string color,bool solid=false){
   var g=Box("Unloading2723_"+name,position,size,color,solid);g.transform.SetParent(UnloadingRoot2723,true);return g;
  }
  void UnloadingBuilding2723(string key,string name,float x,float z,float width,float depth,float height,Vector3 door,Vector3 arrival,float doorWidth){
   var p=PortPoint(x+width/2,z+depth/2);var shell=Block(name+"_工业外墙",p.x,p.z,width,depth,height);shell.transform.SetParent(UnloadingRoot2723,true);
   var r=new Rect(p.x-width/2,p.z-depth/2,width,depth);
   BuildingPlans2723.Add(new UnloadingBuildingPlan2723{Key=key,Name=name,Footprint=r,View=shell,Door=door,Arrival=arrival,Height=height});
   float dh=key=="unloadingwarehouse"?3.3f:2.5f;
   UnloadingCube2723(key+"_ClosedPortal",door+new Vector3(0,dh/2,-.075f),new Vector3(doorWidth,dh,.10f),key=="unloadingwarehouse"?"738F86":"526D67");
   foreach(float side in new[]{-1f,1f})UnloadingCube2723(key+"_DoorJamb",door+new Vector3(side*(doorWidth/2+.08f),dh/2,-.13f),new Vector3(.16f,dh+.16f,.16f),"B6BBAA");
   UnloadingCube2723(key+"_DoorHeader",door+new Vector3(0,dh+.08f,-.13f),new Vector3(doorWidth+.32f,.16f,.16f),"B6BBAA");
   Doors.Add(new WorldDoor{Name="进入"+name,Destination=key,Portal=true,Position=door-Vector3.forward*.25f});
  }
 }
}
