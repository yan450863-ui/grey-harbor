using System.Collections.Generic;
using UnityEngine;
namespace GreyHarbor {
 public sealed class FreightBuildingPlan2716 {public string Key,Name;public Rect Footprint;public GameObject View;public Vector3 Door,Arrival;public float Yaw;}
 public sealed class FreightExteriorRecord2716 {public string Type,Group;public GameObject View;public Vector3 Anchor;public Rect Footprint;public ObstacleKind Kind;}
 public sealed class FreightSearchRecord2716 {public LootContainer Container;public GameObject Model;public Vector3 Stand,AccessPoint;public Rect Footprint;public float Yaw;public ContainerLook Look;}
 public sealed class FreightRoute2716 {public string Name;public float Width;public Vector3[] Points;}
 public sealed partial class RaidMap {
  public static readonly Rect FreightExteriorBounds2716=new Rect(-74,62,45,39);
  public static readonly Vector3 FreightPreviewSpawn2716=PortPoint(17,145),FreightExitPoint2716=PortPoint(8.5f,161);
  public static readonly Vector3 FreightDutyDoor2716=PortPoint(34,142.90f),FreightDutyArrival2716=PortPoint(34,141.50f);
  public static readonly Vector3 FreightWarehouseDoor2716=PortPoint(44.10f,163),FreightWarehouseArrival2716=PortPoint(45.50f,163);
  public static readonly Vector3 FreightGateCenter2716=PortPoint(5,161);
  public const float FreightGateOpening2716=8;
  public readonly List<FreightBuildingPlan2716> FreightBuildingPlans2716=new List<FreightBuildingPlan2716>();
  public readonly List<FreightExteriorRecord2716> FreightExteriorRecords2716=new List<FreightExteriorRecord2716>();
  public readonly List<FreightSearchRecord2716> FreightSearchRecords2716=new List<FreightSearchRecord2716>();
  public readonly List<FreightRoute2716> FreightRoutes2716=new List<FreightRoute2716>();
  public Transform FreightExteriorRoot2716;public GameObject FreightTruck2716,FreightGate2716,FreightBoundaryStop2716;
  bool freightExteriorBuilt2716;
  static Rect FreightRect2716(float x,float z,float w,float d)=>new Rect(x-79,z-73,w,d);
  GameObject FreightObject2716(string name,Vector3 p){var g=new GameObject(name);g.transform.SetParent(FreightExteriorRoot2716?FreightExteriorRoot2716:Root,false);g.transform.position=p;return g;}
  void FreightCube2716(GameObject parent,string name,Vector3 p,Vector3 size,string color,ObstacleKind? kind=null){CargoCube2714(parent,name,p,size,color,kind);}
  public void BuildFreightBuildings2716(){
   if(FreightBuildingPlans2716.Count>0)return;
   if(!FreightExteriorRoot2716){var root=new GameObject("NorthwestFreightExterior2716");root.transform.SetParent(Root,false);FreightExteriorRoot2716=root.transform;}
   FreightBuilding2716("freightduty","货运值班房",FreightRect2716(28,143,12,12),3.5f,FreightDutyDoor2716,FreightDutyArrival2716,0);
   FreightBuilding2716("freightwarehouse","货运库房",FreightRect2716(31,157,13,12),5,FreightWarehouseDoor2716,FreightWarehouseArrival2716,-90);
  }
  void FreightBuilding2716(string key,string name,Rect area,float height,Vector3 door,Vector3 arrival,float yaw){
   var center=new Vector3(area.center.x,0,area.center.y);var g=FreightObject2716(name+"_Exterior2716",center);
   FreightCube2716(g,"SolidBuildingShell",new Vector3(0,height/2,0),new Vector3(area.width,height,area.height),key=="freightduty"?"9DA694":"798D87",ObstacleKind.High);
   FreightCube2716(g,"ConcreteFoundation",new Vector3(0,.14f,0),new Vector3(area.width+.10f,.28f,area.height+.10f),"717C71");
   bool duty=key=="freightduty";float half=area.height/2+.25f,slope=duty?8:10,rise=Mathf.Sin(slope*Mathf.Deg2Rad)*half;
   for(int side=-1;side<=1;side+=2){var roof=FreightObject2716(name+"_RoofSheet",center+new Vector3(0,height+rise/2+.12f,side*half/2));roof.transform.SetParent(g.transform,true);roof.transform.rotation=Quaternion.Euler(side*slope,0,0);
    FreightCube2716(roof,"PitchedRoof",Vector3.zero,new Vector3(area.width+.45f,.14f,half/Mathf.Cos(slope*Mathf.Deg2Rad)),duty?"6F7E75":"677A77");
    for(float x=-area.width/2;x<=area.width/2;x+=.52f)FreightCube2716(roof,"StandingRoofSeam",new Vector3(x,.085f,0),new Vector3(.035f,.035f,half/Mathf.Cos(slope*Mathf.Deg2Rad)),"89958B");
   }
   FreightCube2716(g,"RoofRidge",new Vector3(0,height+rise+.18f,0),new Vector3(area.width+.5f,.10f,.18f),"97A295");
   foreach(float x in new[]{-area.width/2+.12f,area.width/2-.12f})foreach(float z in new[]{-area.height/2-.025f,area.height/2+.025f}){
    FreightCube2716(g,"FacadeCornerPost",new Vector3(x,height/2,z),new Vector3(.14f,height,.07f),"C0C2AB");
    FreightCube2716(g,"RainwaterPipe",new Vector3(x,.5f*height,z+.05f),new Vector3(.095f,height,.095f),"51665D");
   }
   foreach(float z in new[]{-area.height/2-.055f,area.height/2+.055f})FreightCube2716(g,"BaseCourse",new Vector3(0,.44f,z),new Vector3(area.width,.18f,.08f),"838E7D");
   if(duty){foreach(float x in new[]{-4f,3.7f})FreightWindow2716(g,new Vector3(x,2,-6.065f),0,2.2f,1.1f);foreach(float z in new[]{-3f,2.4f})FreightWindow2716(g,new Vector3(6.065f,2,z),-90,2.4f,1.15f);FreightWindow2716(g,new Vector3(-6.065f,2,0),90,2.4f,1.15f);}
   else{foreach(float x in new[]{-4.1f,0,4.1f})FreightWindow2716(g,new Vector3(x,3.75f,-6.065f),0,2.35f,.7f);foreach(float z in new[]{-3.8f,3.8f})FreightWindow2716(g,new Vector3(6.565f,3.75f,z),-90,1.65f,.7f);for(float x=-6.1f;x<6.4f;x+=.65f)foreach(float z in new[]{-6.065f,6.065f})FreightCube2716(g,"MetalFacadeRib",new Vector3(x,2.4f,z),new Vector3(.045f,4.45f,.045f),"93A39A");}
   var entrance=FreightObject2716(name+"_PortalFace",door);entrance.transform.SetParent(g.transform,true);entrance.transform.rotation=Quaternion.Euler(0,yaw,0);
   float width=duty?1.8f:2.4f,dh=duty?2.5f:3;
   FreightCube2716(entrance,"ClosedPortalDoor",new Vector3(0,dh*.5f,-.025f),new Vector3(width,dh,.08f),duty?"506B60":"788F80");
   foreach(float x in new[]{-width/2-.055f,width/2+.055f})FreightCube2716(entrance,"DoorJamb",new Vector3(x,dh/2,-.02f),new Vector3(.11f,dh+.14f,.15f),"354A42");
   FreightCube2716(entrance,"DoorHeader",new Vector3(0,dh+.06f,-.025f),new Vector3(width+.22f,.12f,.16f),"354A42");
   if(duty){FreightCube2716(entrance,"DoorGlass",new Vector3(0,1.75f,-.075f),new Vector3(1.32f,.75f,.018f),"394F51");FreightCube2716(entrance,"DoorHandle",new Vector3(.59f,1.03f,-.15f),new Vector3(.065f,.30f,.10f),"B3B6A2");}
   else for(int i=0;i<18;i++)FreightCube2716(entrance,"ShutterCorrugation",new Vector3(0,.14f+i*.16f,-.081f),new Vector3(width-.1f,.032f,.027f),"A1AF9C");
   FreightCube2716(entrance,"DoorLamp",new Vector3(0,dh+.36f,-.23f),new Vector3(.46f,.16f,.3f),"B6B296");
   FreightCube2716(entrance,"ShallowEntranceCanopy",new Vector3(0,dh+.53f,-.25f),new Vector3(width+.55f,.09f,.65f),"667B6E");
   var plan=new FreightBuildingPlan2716{Key=key,Name=name,Footprint=area,View=g,Door=door,Arrival=arrival,Yaw=yaw};FreightBuildingPlans2716.Add(plan);ExteriorBuildings.Add(area);
   Doors.Add(new WorldDoor{Name="进入"+name,Destination=key,Portal=true,Position=door});
  }
  void FreightWindow2716(GameObject parent,Vector3 p,float yaw,float w,float h){var frame=new GameObject("IndustrialWindow");frame.transform.SetParent(parent.transform,false);frame.transform.localPosition=p;frame.transform.localRotation=Quaternion.Euler(0,yaw,0);FreightCube2716(frame,"WindowFrame",Vector3.zero,new Vector3(w+.16f,h+.16f,.10f),"4D655C");FreightCube2716(frame,"DustyWindowGlass",new Vector3(0,0,-.066f),new Vector3(w,h,.023f),"708D8D");foreach(float x in new[]{-w*.26f,w*.26f})FreightCube2716(frame,"SteelWindowMullion",new Vector3(x,0,-.086f),new Vector3(.035f,h,.025f),"BAC0AC");FreightCube2716(frame,"WindowSill",new Vector3(0,-h/2-.09f,-.11f),new Vector3(w+.22f,.1f,.22f),"949E8D");}
  void FreightRouteAdd2716(string name,float width,params Vector3[] points){FreightRoutes2716.Add(new FreightRoute2716{Name=name,Width=width,Points=points});}
  public void BuildFreightExterior2716(){
   if(freightExteriorBuilt2716)return;freightExteriorBuilt2716=true;BuildFreightBuildings2716();
   FreightRouteAdd2716("GateTruckLane",8,PortPoint(9.2f,161),PortPoint(25.5f,161));
   FreightRouteAdd2716("ExitInnerApproach",1,FreightExitPoint2716,PortPoint(9.2f,161));
   FreightRouteAdd2716("ExistingFreightRoad",5,PortPoint(17,138),PortPoint(17,166));
   FreightRouteAdd2716("SouthBuildingApproach",1.8f,PortPoint(17,141.5f),FreightDutyArrival2716,PortPoint(47,141.5f));
   FreightRouteAdd2716("WestBuildingWalk",1.8f,PortPoint(26,141.5f),PortPoint(26,173.5f));
   FreightRouteAdd2716("EastBuildingWalk",2.4f,PortPoint(47,141.5f),PortPoint(47,173.5f));
   FreightRouteAdd2716("NorthBuildingWalk",1.8f,PortPoint(26,173.5f),PortPoint(47,173.5f));
   FreightRouteAdd2716("BetweenBuildings",1.4f,PortPoint(26,156),PortPoint(47,156));
   FreightGateBuild2716();FreightExternalBackdropBuild2716();FreightTruckBuild2716();Physics.SyncTransforms();
   var rng=new System.Random((RaidGame.Instance?RaidGame.Instance.RaidSeed:2716)^2716);
   FreightSearchAdd2716("货运工具箱",ContainerLook.WeldingToolbox,PortPoint(22.5f,148),0,rng);
   FreightSearchAdd2716("后场日用箱",ContainerLook.OutdoorTote,PortPoint(22.5f,170),180,rng);
   FreightSearchAdd2716("散货检查箱",ContainerLook.CargoCrate,PortPoint(42.6f,151),0,rng);
   FreightSearchAdd2716("库后运输箱",ContainerLook.WaterproofTransitCase,PortPoint(37,171.2f),180,rng);
   BuildFreightOperations2719();
   Physics.SyncTransforms();Debug.Log("Freight2716 exterior: "+FreightExteriorRecords2716.Count+" groups / "+FreightSearchRecords2716.Count+" ordinary searches");
  }
  void FreightGateBuild2716(){
   FreightGate2716=FreightObject2716("FreightWestTruckGate2716",FreightGateCenter2716);
   foreach(float z in new[]{-4.3f,4.3f}){FreightCube2716(FreightGate2716,"ConcreteGatePost",new Vector3(.3f,1.4f,z),new Vector3(.6f,2.8f,.6f),"7F8F81",ObstacleKind.High);FreightCube2716(FreightGate2716,"GatePostCap",new Vector3(.3f,2.84f,z),new Vector3(.69f,.12f,.69f),"B3BAA7");}
   for(int side=-1;side<=1;side+=2){var leaf=new GameObject(side<0?"OpenSouthGateLeaf":"OpenNorthGateLeaf");leaf.transform.SetParent(FreightGate2716.transform,false);leaf.transform.localPosition=new Vector3(.3f,0,side*4);leaf.transform.localRotation=Quaternion.Euler(0,side<0?100:-100,0);
    float inward=side<0?1:-1;FreightCube2716(leaf,"HeavySteelGatePanel",new Vector3(0,1.3f,inward*1.85f),new Vector3(.13f,2.3f,3.7f),"4F6A62",ObstacleKind.High);
    for(int i=0;i<9;i++)FreightCube2716(leaf,"GateVerticalRib",new Vector3(-.085f,1.3f,inward*(.18f+i*.41f)),new Vector3(.055f,2.21f,.055f),"85998A");
    foreach(float y in new[]{.3f,2.3f})FreightCube2716(leaf,"GateCrossRail",new Vector3(-.10f,y,inward*1.85f),new Vector3(.07f,.09f,3.65f),"A0AF99");
    FreightCube2716(leaf,"LatchPlate",new Vector3(-.11f,1.07f,inward*3.4f),new Vector3(.055f,.34f,.22f),"BAAE81");
   }
   // The visual gate opens in the perimeter. The map's land edge remains closed.
   FreightBoundaryStop2716=Box("FreightOuterMapBoundary2716",PortPoint(4.8f,161)+Vector3.up*1.4f,new Vector3(.16f,2.8f,8),"697E73",true);FreightBoundaryStop2716.transform.SetParent(FreightExteriorRoot2716,true);FreightBoundaryStop2716.GetComponent<Renderer>().enabled=false;
  }
  void FreightTruckBuild2716(){
   var truck=FreightObject2716("OverturnedFreightTruck2716",PortPoint(10.2f,151.5f));FreightTruck2716=truck;
   FreightCube2716(truck,"HeavyTruckChassis",new Vector3(0,.8f,0),new Vector3(2.2f,.4f,7.7f),"394F47",ObstacleKind.High);
   FreightCube2716(truck,"TruckCab",new Vector3(0,1.7f,-2.6f),new Vector3(2.4f,1.75f,2.1f),"8A9F8E",ObstacleKind.High);
   FreightCube2716(truck,"CabHood",new Vector3(0,1.3f,-3.6f),new Vector3(2.27f,.65f,.8f),"748C80",ObstacleKind.High);
   FreightCube2716(truck,"CabRoof",new Vector3(0,2.62f,-2.6f),new Vector3(2.5f,.12f,2.18f),"B1BAA0");
   FreightCube2716(truck,"TruckWindshield",new Vector3(0,2.02f,-3.66f),new Vector3(1.94f,.76f,.03f),"3B5656");
   foreach(float x in new[]{-1.217f,1.217f}){FreightCube2716(truck,"CabSideGlass",new Vector3(x,2.05f,-2.6f),new Vector3(.03f,.67f,1.38f),"426361");FreightCube2716(truck,"TruckDoorHandle",new Vector3(x*1.035f,1.56f,-2.18f),new Vector3(.06f,.09f,.25f),"C1BE9E");}
   FreightCube2716(truck,"FrontBumper",new Vector3(0,.9f,-4.05f),new Vector3(2.6f,.25f,.18f),"929F8A");
   FreightCube2716(truck,"RadiatorGrille",new Vector3(0,1.32f,-4.014f),new Vector3(1.4f,.5f,.035f),"2F4741");foreach(float x in new[]{-.93f,.93f})FreightCube2716(truck,"TruckHeadlight",new Vector3(x,1.4f,-4.025f),new Vector3(.35f,.25f,.03f),"C8BA8A");
   FreightCube2716(truck,"TiltedCargoBody",new Vector3(0,2.0f,1.34f),new Vector3(2.5f,2.5f,5.55f),"927F63",ObstacleKind.High);
   // Once rolled, the cargo roof becomes a prominent upright face. Its ribs and
   // repairs remain cosmetic so the accepted truck collision is unchanged.
   for(int i=0;i<11;i++)FreightCube2716(truck,"CargoRoofCrossRib",new Vector3(0,3.28f,-1.15f+i*.5f),new Vector3(2.43f,.035f,.065f),i%3==0?"AA9A78":"B7A883");
   foreach(float x in new[]{-1.12f,1.12f})FreightCube2716(truck,"CargoRoofEdgeSeam",new Vector3(x,3.275f,1.34f),new Vector3(.06f,.025f,5.43f),"B0A181");
   FreightCube2716(truck,"RoofFadedRepair",new Vector3(-.37f,3.257f,.35f),new Vector3(.76f,.012f,.69f),"AB9D7D");
   FreightCube2716(truck,"RoofOxidizedPatch",new Vector3(.51f,3.257f,2.62f),new Vector3(.56f,.012f,.88f),"746F59");
   FreightCube2716(truck,"RoofScrapeWear",new Vector3(-.62f,3.258f,3.38f),new Vector3(.53f,.014f,.44f),"B8AE8B");
   foreach(float x in new[]{-1.275f,1.275f})for(int i=0;i<11;i++)FreightCube2716(truck,"CargoSideRib",new Vector3(x,2.0f,-1.15f+i*.5f),new Vector3(.045f,2.39f,.045f),"BEAD88");
   FreightCube2716(truck,"CargoRearDoors",new Vector3(0,2.0f,4.135f),new Vector3(2.34f,2.33f,.045f),"786D57");foreach(float x in new[]{-.65f,.65f})FreightCube2716(truck,"RearLockingRod",new Vector3(x,2,4.18f),new Vector3(.055f,2.15f,.06f),"B8B397");
   foreach(float z in new[]{-2.8f,1.7f,2.8f})foreach(float x in new[]{-1.15f,1.15f}){
    var wheel=GameObject.CreatePrimitive(PrimitiveType.Cylinder);wheel.name="TruckRubberWheel";wheel.transform.SetParent(truck.transform,false);wheel.transform.localPosition=new Vector3(x,.61f,z);wheel.transform.localRotation=Quaternion.Euler(0,0,90);wheel.transform.localScale=new Vector3(1.18f,.17f,1.18f);wheel.GetComponent<Renderer>().sharedMaterial=Mat("293C35",true);SceneObstacle.Apply(wheel,ObstacleKind.High);
    FreightCube2716(truck,"VisibleAxle",new Vector3(0,.6f,z),new Vector3(2.6f,.13f,.13f),"697C6A");
   }
   truck.transform.rotation=Quaternion.Euler(0,5,58);Physics.SyncTransforms();var bounds=CargoBounds2714(truck);float minY=float.MaxValue;foreach(var r in truck.GetComponentsInChildren<Renderer>())minY=Mathf.Min(minY,r.bounds.min.y);truck.transform.position+=Vector3.up*(.065f-minY);Physics.SyncTransforms();
  }
  bool FreightPocket2716(Rect r,GameObject own){
   var domain=FreightExteriorBounds2716;if(r.xMin<domain.xMin+.35f||r.xMax>domain.xMax-.05f||r.yMin<domain.yMin+.1f||r.yMax>domain.yMax-.15f)return false;
   foreach(var route in FreightRoutes2716)for(int i=1;i<route.Points.Length;i++)if(CargoSegmentRect2714(route.Points[i-1],route.Points[i],r)<route.Width/2+.15f)return false;
   foreach(var p in FreightBuildingPlans2716)if(CargoSegmentRect2714(p.Arrival,p.Door,r)<.8f)return false;
   foreach(var s in FreightSearchRecords2716)if(CargoSegmentRect2714(s.Stand,s.AccessPoint,r)<.52f)return false;
   foreach(var c in Physics.OverlapBox(new Vector3(r.center.x,1.45f,r.center.y),new Vector3(r.width/2+.09f,1.41f,r.height/2+.09f),Quaternion.identity,SceneObstacle.MovementMask,QueryTriggerInteraction.Ignore))if(own==null||!c.transform.IsChildOf(own.transform))return false;
   return true;
  }
  bool FreightAccess2716(Vector3 stand,out Vector3 access){access=stand;var bottom=stand+Vector3.up*.36f;var top=stand+Vector3.up*1.35f;if(!HarborSurfaceClear(stand)||Physics.CheckCapsule(bottom,top,.34f,SceneObstacle.MovementMask,QueryTriggerInteraction.Ignore))return false;float best=float.MaxValue;bool found=false;foreach(var route in FreightRoutes2716)for(int i=1;i<route.Points.Length;i++){var a=route.Points[i-1];var d=route.Points[i]-a;var p=a+d*Mathf.Clamp01(Vector3.Dot(stand-a,d)/d.sqrMagnitude);var delta=p-stand;float dist=delta.magnitude;if(dist>=best||dist>8)continue;if(dist>.01f&&Physics.CapsuleCast(bottom,top,.34f,delta/dist,dist,SceneObstacle.MovementMask,QueryTriggerInteraction.Ignore))continue;access=p;best=dist;found=true;}return found;}
  void FreightSearchAdd2716(string name,ContainerLook look,Vector3 preferred,float yaw,System.Random rng){
   foreach(float offset in new[]{0f,.45f,-.45f,.85f,-.85f}){var model=preferred+Vector3.right*offset;var stand=model+Quaternion.Euler(0,yaw,0)*Vector3.back*1.3f;var envelope=CargoRect2714(model,new Vector2(1.8f,1.25f),yaw);Vector3 access;
    if(!FreightPocket2716(envelope,null)||!FreightAccess2716(stand,out access))continue;
    var box=AddContainer(stand,model,look,SupplyType.Misc,rng,HarborDistrict.Loading,yaw,false,ContainerTier.Common,DropRegion230.Freight);box.Name=name;box.View.transform.SetParent(FreightExteriorRoot2716,true);Physics.SyncTransforms();var actual=CargoBounds2714(box.View);
    if(!FreightPocket2716(actual,box.View)||!FreightAccess2716(stand,out access)){Loot.Remove(box);box.View.SetActive(false);TransientObjects2721.Release(box.View);continue;}
    FreightSearchRecords2716.Add(new FreightSearchRecord2716{Container=box,Model=box.View,Stand=stand,AccessPoint=access,Footprint=actual,Yaw=yaw,Look=look});return;
   }Debug.LogError("Freight2716 no safe search position: "+name);
  }
  bool FreightDecorAdd2716(string type,Vector3 design,string group){
   var anchor=PortPoint(design.x,design.y);float yaw=design.z;var size=type=="LooseParcels"||type=="MetalOffcuts"?new Vector2(.94f,.72f):CargoDecorSize2715(type);if(!FreightPocket2716(CargoRect2714(anchor,size,yaw),null))return false;
   GameObject view;var kit=new DockLoadingKit142(this);if(type=="LooseParcels"||type=="MetalOffcuts")view=FreightCompactStock2716(type,anchor,yaw);else if(type=="WrappedLoad")view=kit.WrappedLoad(anchor,yaw).gameObject;else if(type=="BarrelRack")view=kit.BarrelRack(anchor,yaw).gameObject;else if(type=="Spool")view=kit.SpoolStack(anchor,yaw).gameObject;else if(type=="PipeBundle")view=kit.PipeBundle(anchor,yaw).gameObject;else if(type=="PalletJack")view=kit.PalletJack(anchor,yaw).gameObject;else if(type=="CrateStack"||type=="TimberStack"||type=="ScrapRack")view=CargoFreightModel2715(type,anchor,yaw);else view=CargoSimple2714(type,anchor,yaw);
   view.name="FreightDressing2716_"+type+"_"+FreightExteriorRecords2716.Count;view.transform.SetParent(FreightExteriorRoot2716,true);Physics.SyncTransforms();var area=CargoBounds2714(view);if(!FreightPocket2716(area,view)){view.SetActive(false);TransientObjects2721.Release(view);return false;}
   FreightExteriorRecords2716.Add(new FreightExteriorRecord2716{Type=type,Group=group,View=view,Anchor=anchor,Footprint=area,Kind=type=="WrappedLoad"||type=="CrateStack"||type=="ScrapRack"?ObstacleKind.High:ObstacleKind.Low});return true;
  }
  GameObject FreightCompactStock2716(string type,Vector3 p,float yaw){var g=FreightObject2716(type,p);if(type=="LooseParcels"){
   FreightCube2716(g,"TiedParcel",new Vector3(-.23f,.21f,0),new Vector3(.42f,.42f,.60f),"B3A07A",ObstacleKind.Low);FreightCube2716(g,"SmallShippingCarton",new Vector3(.25f,.13f,.09f),new Vector3(.36f,.26f,.45f),"927E5D",ObstacleKind.Low);FreightCube2716(g,"ParcelString",new Vector3(-.23f,.424f,0),new Vector3(.04f,.012f,.60f),"D0C6A4");FreightCube2716(g,"PaperWaybill",new Vector3(.25f,.268f,.09f),new Vector3(.18f,.01f,.18f),"C9C5AC");
  }else{for(int i=0;i<4;i++)FreightCube2716(g,"StackedScrapPlate",new Vector3(-.06f+i*.035f,.045f+i*.08f,-.1f+i*.065f),new Vector3(.79f,.08f,.43f),i%2==0?"7C8570":"68796C",ObstacleKind.Low);FreightCube2716(g,"ScrapTie",new Vector3(0,.37f,0),new Vector3(.075f,.03f,.66f),"AC9D76");}g.transform.rotation=Quaternion.Euler(0,yaw,0);return g;}
 }
}
