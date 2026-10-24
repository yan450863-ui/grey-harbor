using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
namespace GreyHarbor {
 public sealed partial class RaidMap {
  public readonly List<FreightLootRecord2717> UnloadingSearchRecords2724=new List<FreightLootRecord2717>();
  public readonly List<FreightDressingRecord2717> UnloadingDressingRecords2724=new List<FreightDressingRecord2717>();
  public int UnloadingWallRendererCount2724 {get;private set;}
  void BuildUnloadingDressing2724(bool warehouse){
   var rng=new System.Random((RaidGame.Instance?RaidGame.Instance.RaidSeed:2724)^2724);
   var kit=new FutureAssetKit2717(this);
   if(warehouse){
    UnloadingLoot2724("收货分拣大厅",ContainerLook.CargoCrate,new Vector3(-6,0,-5.95f),180,new Vector3(-6,0,-4.6f),"待验收木货箱",rng);
    UnloadingLoot2724("收货分拣大厅",ContainerLook.WaterproofTransitCase,new Vector3(6,0,-5.95f),180,new Vector3(6,0,-4.6f),"封签运输箱",rng);
    UnloadingLoot2724("收货分拣大厅",ContainerLook.OutdoorTote,new Vector3(-6,0,5.95f),0,new Vector3(-6,0,4.6f),"包装耗材箱",rng);
    UnloadingLoot2724("收货分拣大厅",ContainerLook.CargoCrate,new Vector3(6,0,5.95f),0,new Vector3(6,0,4.6f),"待出库木货箱",rng);
    UnloadingLoot2724("收货分拣大厅",ContainerLook.Toolbox,new Vector3(-9.9f,0,0),270,new Vector3(-8.45f,0,0),"装卸维修工具箱",rng);
    UnloadingLoot2724("收货分拣大厅",ContainerLook.ElectricalCabinet,new Vector3(10.15f,0,0),90,new Vector3(8.6f,0,0),"仓库检修电柜",rng);
    var repair=new RepairInteriorKit200(this);
    foreach(float x in new[]{-10.4f,10.4f})foreach(float z in new[]{-3.55f,3.55f})UnloadingTrack2724("PartsRack","收货分拣大厅",repair.PartsRack(new Vector3(x,0,z),x<0?270:90).gameObject,ObstacleKind.High);
    UnloadingTrack2724("PackingBench","分拣工作组",FreightPackingBench2717(new Vector3(5.7f,0,2.5f),0));
    UnloadingTrack2724("CargoScale","分拣工作组",kit.Build(FutureProp2717.LoadingScale,new Vector3(8.0f,0,2.1f)));
    UnloadingTrack2724("HandTruck","待发货区",kit.Build(FutureProp2717.HandTruck,new Vector3(8.2f,0,-3.0f),165));
    UnloadingTrack2724("WrappedPallet","收货暂存区",kit.Build(FutureProp2717.WrappedPallet,new Vector3(-6.2f,0,-2.7f),4),ObstacleKind.High);
    UnloadingTrack2724("CargoCage","收货暂存区",kit.Build(FutureProp2717.CargoCage,new Vector3(-8.1f,0,-2.5f),0),ObstacleKind.High);
    UnloadingTrack2724("OpenCargoCrate","打包区",kit.Build(FutureProp2717.OpenCargoCrate,new Vector3(-6.2f,0,2.65f),-8));
    UnloadingTrack2724("PalletJack","打包区",kit.Build(FutureProp2717.PalletJack,new Vector3(-8.1f,0,2.4f),170));
    UnloadingTrack2724("CargoStraps","分拣工作组",kit.Build(FutureProp2717.CargoStraps,new Vector3(4.9f,0,3.6f),12));
    UnloadingTrack2724("PaperBoxes","分拣工作组",FreightPaperBoxes2717(new Vector3(7.4f,0,4.4f),90));
   }else{
    UnloadingLoot2724("调度值班室",ContainerLook.FileCabinet,new Vector3(-3.35f,0,-.85f),270,new Vector3(-2.05f,0,-.85f),"装卸调度文件柜",rng);
    // Walk around the east tip of the opened door, then approach the box from its west front.
    UnloadingLoot2724("杂物间",ContainerLook.OutdoorTote,new Vector3(3.05f,0,-1.85f),90,new Vector3(1.9f,0,-1.75f),"员工用品收纳箱",rng,new Vector3(2.85f,0,-.6f));
    UnloadingTrack2724("ComputerDesk","调度值班室",FreightDesk2717(new Vector3(-1.8f,0,2.22f),0,true));
    UnloadingTrack2724("OfficeChair","调度值班室",FreightChair2717(new Vector3(-1.8f,0,1.02f),180,true));
    UnloadingTrack2724("LowCabinet","调度值班室",FreightCabinet2717(new Vector3(-3.35f,0,1.2f),270,false));
    UnloadingTrack2724("VisitorChair","调度值班室",FreightChair2717(new Vector3(-2.6f,0,-2.2f),180,false));
    UnloadingTrack2724("SupplyCabinet","杂物间",FreightCabinet2717(new Vector3(2.85f,0,2.3f),0,true),ObstacleKind.High);
    UnloadingTrack2724("PaperBoxes","调度值班室",FreightPaperBoxes2717(new Vector3(.15f,0,2.3f),0));
   }
   UnloadingWallDressing2724(warehouse);
   Physics.SyncTransforms();
  }
  void UnloadingLoot2724(string room,ContainerLook look,Vector3 model,float yaw,Vector3 stand,string name,System.Random rng,Vector3? access=null){
   var box=AddContainer(stand,model,look,SupplyType.Misc,rng,HarborDistrict.Loading,yaw,false,ContainerTier.Common,InteriorId=="unloadingduty"?DropRegion230.Office:DropRegion230.Freight);
   box.Name=name;FreightPlaceOnFloor2717(box.View);
   UnloadingSearchRecords2724.Add(new FreightLootRecord2717{Container=box,Model=box.View,Stand=stand,AccessPoint=access??new Vector3(0,0,stand.z),RoomName=room,Yaw=yaw,WallMounted=false});
  }
  void UnloadingTrack2724(string type,string room,GameObject view,ObstacleKind kind=ObstacleKind.Low,bool wall=false){
   if(!wall)FreightPlaceOnFloor2717(view);Physics.SyncTransforms();
   view.name="Unloading2724_"+type+"_"+UnloadingDressingRecords2724.Count;
   UnloadingDressingRecords2724.Add(new FreightDressingRecord2717{Name=view.name,Type=type,RoomName=room,View=view,Footprint=FreightActualBounds2717(view),Kind=kind,WallMounted=wall});
  }
  void UnloadingWallDressing2724(bool warehouse){
   var art=new WorkshopWastelandArt(this);
   foreach(var renderer in Root.GetComponentsInChildren<MeshRenderer>())if(renderer.name=="UnloadingWhiteboxWall2723"){
    var b=renderer.bounds;bool horizontal=b.size.x>b.size.z;
    var along=horizontal?Vector3.right:Vector3.forward;var normal=horizontal?Vector3.forward:Vector3.right;
    float length=horizontal?b.size.x:b.size.z,thickness=horizontal?b.size.z:b.size.x;
    foreach(float sign in new[]{-1f,1f})art.WallSkin(new Vector3(b.center.x,0,b.center.z)+normal*sign*(thickness/2+.012f),along,normal*sign,length,warehouse?"748A8D":"81938A");
   }
   var wallSkin=FreightModelRoot2717("UnloadingWallSkin2724",Vector3.zero);FreightBakeModel2717(wallSkin,art,0);
   UnloadingTrack2724("WallSkins","全室",wallSkin,ObstacleKind.Decoration,true);
   // Wall-mounted panels add identity without new collision or overhead beams.
   var before=FreightDressingRecords2717.Count;
   FreightWallPanel2717("UnloadingDispatchBoard2724","全室",warehouse?new Vector3(-3.2f,1.65f,6.775f):new Vector3(-1.85f,1.65f,2.775f),0,"Notice");
   if(warehouse)FreightWallPanel2717("UnloadingWarehouseWindow2724","全室",new Vector3(-10.775f,2.4f,0),270,"Window");
   else FreightWallPanel2717("UnloadingDutyWindow2724","全室",new Vector3(-3.775f,1.75f,1.8f),270,"Window");
   for(int i=before;i<FreightDressingRecords2717.Count;i++)UnloadingDressingRecords2724.Add(FreightDressingRecords2717[i]);
   var shader=GreyHarbor.FlashlightShaders2763.Load("FreightWall2718");if(!shader)throw new System.InvalidOperationException("Freight wall material missing");
   var life=Root.gameObject.AddComponent<WorkshopArtResources>();var copies=new Dictionary<Material,Material>();
   var inner=new Vector4(Bounds.xMin+.43f,Bounds.yMin+.43f,Bounds.xMax-.43f,Bounds.yMax-.43f);
   foreach(var r in Root.GetComponentsInChildren<MeshRenderer>()){
    bool wall=r.name.StartsWith("UnloadingWhitebox")&&r.name!="UnloadingWhiteboxFloor2723";
    foreach(var item in UnloadingDressingRecords2724)if(item.WallMounted&&item.View&&r.transform.IsChildOf(item.View.transform)){wall=true;break;}
    if(!wall||!r.sharedMaterial)continue;
    if(!copies.TryGetValue(r.sharedMaterial,out var mat)){mat=new Material(r.sharedMaterial){shader=shader,name="UnloadingWall2724_"+r.sharedMaterial.name};mat.SetVector("_FreightInnerRect2718",inner);copies.Add(r.sharedMaterial,mat);life.Materials.Add(mat);}
    r.sharedMaterial=mat;r.shadowCastingMode=ShadowCastingMode.Off;r.receiveShadows=false;UnloadingWallRendererCount2724++;
   }
  }
 }
}
