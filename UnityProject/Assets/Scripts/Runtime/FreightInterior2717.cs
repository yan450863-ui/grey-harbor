using System;
using System.Collections.Generic;
using UnityEngine;
namespace GreyHarbor {
 public sealed class FreightDressingRecord2717 {public string Type,RoomName,Name;public GameObject View;public Rect Footprint;public ObstacleKind Kind;public bool WallMounted;}
 public sealed class FreightLootRecord2717 {public LootContainer Container;public GameObject Model;public Vector3 Stand,AccessPoint;public string RoomName;public float Yaw;public bool WallMounted;}
 public sealed class FreightWalkRoute2717 {public string Name;public Vector3[] Points;public float Width;}
 public sealed class FreightFloorZone2717 {public string Name,Material;public Rect Area;public Color Color;}
 public sealed partial class RaidMap {
  public readonly List<FreightDressingRecord2717> FreightDressingRecords2717=new List<FreightDressingRecord2717>();
  public readonly List<FreightLootRecord2717> FreightLootRecords2717=new List<FreightLootRecord2717>();
  public readonly List<FreightWalkRoute2717> FreightWalkRoutes2717=new List<FreightWalkRoute2717>();
  public readonly List<FreightFloorZone2717> FreightFloorZones2717=new List<FreightFloorZone2717>();
  public const float FreightFloorY2717=.05f;
  void BuildFreightDressing2717(){
   FreightWalkRoutes2717.Add(new FreightWalkRoute2717{Name="中央通行带",Width=2.4f,Points=new[]{new Vector3(0,0,-7),new Vector3(0,0,7)}});
   if(InteriorId=="freightwarehouse")FreightWalkRoutes2717.Add(new FreightWalkRoute2717{Name="库房横向分拣通道",Width=2.4f,Points=new[]{new Vector3(-7.4f,0,0),new Vector3(7.4f,0,0)}});
   foreach(var room in FreightRooms2716){float side=room.Area.center.x<room.Door.Position.x?-1:1;FreightWalkRoutes2717.Add(new FreightWalkRoute2717{Name=room.Name+"内侧通行",Width=.9f,Points=new[]{room.Door.Position+Vector3.right*side*1.1f,room.WalkPoint}});}
   var rng=new System.Random((RaidGame.Instance?RaidGame.Instance.RaidSeed:2717)^2717);
   if(InteriorId=="freightduty"){
    FreightLoot2717("值班办公室",ContainerLook.FileCabinet,new Vector3(-7.90f,0,-2.8f),270,new Vector3(-6.60f,0,-2.8f),"值班文件柜",rng,new Vector3(-6.60f,0,-4.16f));
    FreightLoot2717("值班办公室",ContainerLook.Suitcase,new Vector3(-4,0,-7.75f),180,new Vector3(-4,0,-6.45f),"未领取公文箱",rng);
    FreightLoot2717("员工休息区",ContainerLook.RollingSuitcase,new Vector3(3,0,-7.65f),180,new Vector3(3,0,-6.35f),"工人行李箱",rng);
    FreightLoot2717("员工休息区",ContainerLook.Coat,new Vector3(8.17f,0,-2.4f),90,new Vector3(6.85f,0,-2.4f),"挂在休息区的外套",rng,new Vector3(6.70f,0,-4.16f));
    FreightLoot2717("货运资料室",ContainerLook.FileCabinet,new Vector3(-7.85f,0,4.2f),270,new Vector3(-6.55f,0,4.2f),"货运资料柜",rng);
    FreightLoot2717("卫生间",ContainerLook.MedicalCabinet,new Vector3(8.15f,0,2.2f),90,new Vector3(6.85f,0,2.2f),"洗漱应急药柜",rng);
    FreightDressDuty2717();
   }else{
    FreightLoot2717("仓储通间",ContainerLook.CargoCrate,new Vector3(-5.6f,0,-7.6f),180,new Vector3(-5.6f,0,-6.3f),"西侧待转运木箱",rng,new Vector3(0,0,-6.3f));
    FreightLoot2717("仓储通间",ContainerLook.WaterproofTransitCase,new Vector3(5.6f,0,-7.6f),180,new Vector3(5.6f,0,-6.3f),"封存运输箱",rng,new Vector3(0,0,-6.3f));
    FreightLoot2717("仓储通间",ContainerLook.Toolbox,new Vector3(8.12f,0,-3),90,new Vector3(6.82f,0,-3),"分拣维修工具箱",rng,new Vector3(1.5f,0,-3));
    FreightLoot2717("仓储通间",ContainerLook.ElectricalCabinet,new Vector3(8.05f,0,3.2f),90,new Vector3(6.75f,0,3.2f),"库房检修电气柜",rng,new Vector3(1.5f,0,3.2f));
    FreightLoot2717("仓储通间",ContainerLook.OutdoorTote,new Vector3(4.2f,0,7.75f),0,new Vector3(4.2f,0,6.45f),"打包耗材收纳箱",rng,new Vector3(1.5f,0,6.45f));
    FreightLoot2717("货物收发室",ContainerLook.FileCabinet,new Vector3(-8.15f,0,5.6f),270,new Vector3(-6.85f,0,5.6f),"收发单据柜",rng);
    FreightDressWarehouse2717();
   }
   FreightDressWalls2717();Physics.SyncTransforms();
  }
  void FreightLoot2717(string roomName,ContainerLook look,Vector3 model,float yaw,Vector3 stand,string name,System.Random rng,Vector3? access=null){
   var room=FreightRooms2716.Find(r=>r.Name==roomName);Vector3 approach=access??(room!=null?room.WalkPoint:new Vector3(0,0,stand.z));bool mount=look==ContainerLook.Coat||look==ContainerLook.MedicalCabinet;
   var l=AddContainer(stand,model,look,SupplyType.Misc,rng,HarborDistrict.Office,yaw,false,ContainerTier.Common,InteriorId=="freightduty"?DropRegion230.Office:DropRegion230.Freight);l.Name=name;if(!mount)FreightPlaceOnFloor2717(l.View);
   FreightLootRecords2717.Add(new FreightLootRecord2717{Container=l,Model=l.View,Stand=stand,AccessPoint=approach,RoomName=roomName,Yaw=yaw,WallMounted=mount});
  }
  void FreightDressDuty2717(){
   var living=new FerryLivingKit144(this);var repair=new RepairInteriorKit200(this);
   FreightTrack2717("ComputerDesk","值班办公室",FreightDesk2717(new Vector3(-5.7f,0,-1.5f),0,true));
   FreightTrack2717("OfficeChair","值班办公室",FreightChair2717(new Vector3(-5.7f,0,-2.75f),180,true));
   FreightTrack2717("RadioDesk","值班办公室",FreightDesk2717(new Vector3(-7.55f,0,-5.6f),270,false,true));
   FreightTrack2717("VisitorChair","值班办公室",FreightChair2717(new Vector3(-6.25f,0,-5.6f),90,false));
   FreightTrack2717("LowCabinet","值班办公室",FreightCabinet2717(new Vector3(-2.3f,0,-7.3f),180,false));
   FreightTrack2717("PaperBoxes","值班办公室",FreightPaperBoxes2717(new Vector3(-7.6f,0,-7.4f),0));
   FreightTrack2717("Sofa","员工休息区",living.Sofa(new Vector3(4.9f,0,-.73f)).gameObject);
   FreightTrack2717("TeaTable","员工休息区",living.Table(new Vector3(4.7f,0,-2.7f)).gameObject);
   FreightTrack2717("VisitorChair","员工休息区",FreightChair2717(new Vector3(2.75f,0,-2.7f),270,false));
   FreightTrack2717("TeaCounter","员工休息区",living.ServiceCounter(new Vector3(7.7f,0,-5.2f),90).gameObject);
   FreightTrack2717("RestLockers","员工休息区",repair.WorkLockers(new Vector3(5.5f,0,-7.86f),180).gameObject,ObstacleKind.High);
   FreightTrack2717("ArchiveShelf","货运资料室",FreightArchiveShelf2717(new Vector3(-6.7f,0,7.87f),0),ObstacleKind.High);
   FreightTrack2717("ArchiveShelf","货运资料室",FreightArchiveShelf2717(new Vector3(-3.5f,0,7.87f),0),ObstacleKind.High);
   FreightTrack2717("ReadingTable","货运资料室",FreightDesk2717(new Vector3(-4.7f,0,6.1f),0,false));
   FreightTrack2717("VisitorChair","货运资料室",FreightChair2717(new Vector3(-4.7f,0,4.95f),180,false));
   FreightTrack2717("PaperBoxes","货运资料室",FreightPaperBoxes2717(new Vector3(-2.3f,0,1.1f),90));
   FreightTrack2717("WashStation","卫生间",repair.WashStation(new Vector3(7.80f,0,4.4f),90).gameObject);
   FreightTrack2717("Toilet","卫生间",living.Toilet(new Vector3(6.8f,0,7.65f)).gameObject);
   FreightTrack2717("Toilet","卫生间",living.Toilet(new Vector3(4,0,7.65f)).gameObject);
   FreightTrack2717("PrivacyScreen","卫生间",FreightPrivacy2717(new Vector3(5.4f,0,7.25f)),ObstacleKind.High);
   FreightTrack2717("WashBench","卫生间",new DockPassengerKit143(this).WaitingBench(new Vector3(2.65f,0,6.2f),90).gameObject);
  }
  void FreightDressWarehouse2717(){
   var repair=new RepairInteriorKit200(this);var loading=new DockLoadingKit142(this);var dock=new DockLivingKit141(this);
   foreach(float z in new[]{-6.5f,-2.75f})FreightTrack2717("PartsRack","仓储通间",repair.PartsRack(new Vector3(-8.25f,0,z),270).gameObject,ObstacleKind.High);
   foreach(float z in new[]{-6.15f,6.60f})FreightTrack2717("PartsRack","仓储通间",repair.PartsRack(new Vector3(8.25f,0,z),90).gameObject,ObstacleKind.High);
   FreightTrack2717("WrappedFreight","仓储通间",loading.WrappedLoad(new Vector3(-5.9f,0,-3.9f)).gameObject,ObstacleKind.High);
   FreightTrack2717("CrateStack","仓储通间",CargoFreightModel2715("CrateStack",new Vector3(-3.3f,0,-4.6f),0),ObstacleKind.High);
   FreightTrack2717("WrappedFreight","仓储通间",loading.WrappedLoad(new Vector3(4.8f,0,-4.5f)).gameObject,ObstacleKind.High);
   FreightTrack2717("PackingBench","仓储通间",FreightPackingBench2717(new Vector3(4.1f,0,4.5f),0));
   FreightTrack2717("PalletJack","仓储通间",loading.PalletJack(new Vector3(6.9f,0,5.3f),0).gameObject);
   FreightTrack2717("CableDrum","仓储通间",dock.CableDrum(new Vector3(-6.6f,0,-6)).gameObject);
   FreightTrack2717("ReceivingDesk","货物收发室",FreightDesk2717(new Vector3(-4.8f,0,7.1f),0,true));
   FreightTrack2717("OfficeChair","货物收发室",FreightChair2717(new Vector3(-4.8f,0,5.95f),180,true));
   FreightTrack2717("ReceivingLockers","货物收发室",repair.WorkLockers(new Vector3(-7.4f,0,7.8f)).gameObject,ObstacleKind.High);
  }
  void FreightTrack2717(string type,string room,GameObject view,ObstacleKind kind=ObstacleKind.Low,bool mount=false){
   if(!mount)FreightPlaceOnFloor2717(view);Physics.SyncTransforms();var rect=FreightActualBounds2717(view);view.name="Freight2717_"+type+"_"+FreightDressingRecords2717.Count;
   FreightDressingRecords2717.Add(new FreightDressingRecord2717{Name=view.name,Type=type,RoomName=room,View=view,Footprint=rect,Kind=kind,WallMounted=mount});
  }
  void FreightPlaceOnFloor2717(GameObject view){float lowest=float.MaxValue;foreach(var r in view.GetComponentsInChildren<Renderer>())if(r.enabled)lowest=Mathf.Min(lowest,r.bounds.min.y);if(lowest<float.MaxValue)view.transform.position+=Vector3.up*(FreightFloorY2717-lowest);}
  Rect FreightActualBounds2717(GameObject view){bool found=false;Bounds b=new Bounds();foreach(var r in view.GetComponentsInChildren<Renderer>())if(r.enabled){if(!found){b=r.bounds;found=true;}else b.Encapsulate(r.bounds);}foreach(var c in view.GetComponentsInChildren<Collider>())if(c.enabled){if(!found){b=c.bounds;found=true;}else b.Encapsulate(c.bounds);}return new Rect(b.min.x,b.min.z,b.size.x,b.size.z);}
  void FreightBakeFloor2717(float width,float depth){
   FreightFloorRegion2716(new Rect(-width/2,-depth/2,width,depth),"777F73");
   if(InteriorId=="freightduty"){
    FreightFloorZone2717("公共走廊",FreightLobby2716,"SpeckledTerrazzo","7F9083",1.2f);
    foreach(var room in FreightRooms2716){Rect r=room.Area;r.xMin+=.10f;r.xMax-=.10f;r.yMin+=.10f;r.yMax-=.10f;
     string finish=room.Name=="员工休息区"?"WarmTimber":room.Name=="卫生间"?"PorcelainTile":room.Name=="货运资料室"?"ArchiveConcrete":"OfficeVinyl";
     string color=finish=="WarmTimber"?"9B8666":finish=="PorcelainTile"?"B6B9A8":finish=="ArchiveConcrete"?"7D8A79":"A29E83";FreightFloorZone2717(room.Name,r,finish,color,finish=="WarmTimber"? .25f:finish=="PorcelainTile"? .5f:1.4f);
    }
   }else{
    FreightFloorZone2717("仓储通间",new Rect(-8.82f,-8.32f,17.64f,16.64f),"WornConcrete","7C8375",0);
    var r=FreightRooms2716[0].Area;r.xMin+=.1f;r.xMax-=.1f;r.yMin+=.1f;r.yMax-=.1f;FreightFloorZone2717("货物收发室",r,"OfficeVinyl","A49E80",1.4f);
    foreach(float x in new[]{-1.24f,1.24f})for(float z=-7.9f;z<7.9f;z+=.76f)FreightFloorRegion2716(new Rect(x-.028f,z,.056f,.43f),"B0A15F");
    foreach(float z in new[]{-1.24f,1.24f})for(float x=-8;x<8;x+=.76f)FreightFloorRegion2716(new Rect(x,z-.028f,.43f,.056f),"B0A15F");
   }
  }
  void FreightFloorZone2717(string name,Rect area,string material,string color,float step){
   // Floor finish is painted per world-space texel after the common stable bake.
   // Keeping one plane avoids decorative overlays fighting for depth.
   FreightFloorZones2717.Add(new FreightFloorZone2717{Name=name,Area=area,Material=material,Color=PixelArt.Hex(color)});FreightFloorRegion2716(area,color);
  }
 }
}

