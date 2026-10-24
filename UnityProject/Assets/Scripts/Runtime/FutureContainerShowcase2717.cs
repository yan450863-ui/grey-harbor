using System;
using System.Collections.Generic;
using UnityEngine;
namespace GreyHarbor {
 public enum FutureContainerKind2717 { OfficeArchive,ReceivingCabinet,StaffLocker,FirstAidCabinet,LoadingToolbox,PartsCrate,TunnelMaintenance,ElectricalInspection }
 public sealed class FutureContainerRecord2717 {
  public string Id,Name,Category;public FutureContainerKind2717 Kind;public LootContainer Container;public GameObject View;
  public Vector3 Model,Stand,AccessPoint;public Rect Footprint;public Bounds VisualBounds;public ContainerLook Look;public SupplyType Type;public DropRegion230 Region;
 }
 public sealed partial class RaidMap {
  public readonly List<FutureContainerRecord2717> FutureContainerRecords2717=new List<FutureContainerRecord2717>();
  // Reusable registration: one detailed visible model, its original piece colliders,
  // and the existing Common profile. This never calls AddContainer on a kit model.
  public FutureContainerRecord2717 BuildFutureSearchContainer2717(FutureContainerKind2717 kind,Vector3 model,float yaw,System.Random rng){
   string name,category;ContainerLook look;SupplyType type=SupplyType.Misc;DropRegion230 region;FutureProp2717 propId=FutureProp2717.TallFileCabinet;bool standard=false;
   switch(kind){
    case FutureContainerKind2717.OfficeArchive:name="办公档案柜";category="办公 / 档案";look=ContainerLook.FileCabinet;region=DropRegion230.Office;break;
    case FutureContainerKind2717.ReceivingCabinet:name="资料收件柜";category="办公 / 收件";propId=FutureProp2717.ParcelLocker;look=ContainerLook.ArchiveCase;region=DropRegion230.Office;break;
    case FutureContainerKind2717.StaffLocker:name="员工储物柜";category="办公 / 生活";propId=FutureProp2717.PersonalLocker;look=ContainerLook.Wardrobe;region=DropRegion230.Office;break;
    case FutureContainerKind2717.FirstAidCabinet:name="急救柜";category="办公 / 医疗资料";look=ContainerLook.RescueCabinet;region=DropRegion230.Office;standard=true;break;
    case FutureContainerKind2717.LoadingToolbox:name="装卸工具车";category="装卸 / 工具";propId=FutureProp2717.ToolTrolley;look=ContainerLook.Toolbox;region=DropRegion230.Loading;type=SupplyType.Tools;break;
    case FutureContainerKind2717.PartsCrate:name="装卸零件箱";category="装卸 / 零件";propId=FutureProp2717.OpenCargoCrate;look=ContainerLook.StorageBin;region=DropRegion230.Loading;type=SupplyType.Tools;break;
    case FutureContainerKind2717.TunnelMaintenance:name="管网维护柜";category="管网 / 维修";propId=FutureProp2717.ControlPanel;look=ContainerLook.SupplyLocker;region=DropRegion230.Repair;type=SupplyType.Tools;break;
    case FutureContainerKind2717.ElectricalInspection:name="电气检修箱";category="管网 / 电气";look=ContainerLook.MarineControlBox;region=DropRegion230.Repair;standard=true;break;
    default:throw new ArgumentOutOfRangeException("kind");
   }
   ContainerProp search;GameObject view;
   if(standard){search=new ContainerArt(this).Build(look,model,yaw,false);view=search.gameObject;}
   else {view=new FutureAssetKit2717(this).Build(propId,model,yaw);search=view.AddComponent<ContainerProp>();search.Look=look;}
   view.name="FutureSearch2717_"+kind;
   bool first=true;Bounds visual=new Bounds(model,Vector3.zero);
   foreach(var renderer in view.GetComponentsInChildren<Renderer>())if(renderer.enabled){if(first){visual=renderer.bounds;first=false;}else visual.Encapsulate(renderer.bounds);}
   if(first)throw new InvalidOperationException("搜索容器必须有真实外观："+name);
   // WorldInteraction uses a named Body renderer for its bounds. The disabled,
   // nonphysical proxy is exactly the real visible mesh envelope, not a second shell.
   var originalBody=view.transform.Find("Body");if(originalBody)originalBody.name="CabinetBody";
   var interactionBody=Box("Body",visual.center,visual.size,"708969");interactionBody.transform.SetParent(view.transform,true);interactionBody.GetComponent<Renderer>().enabled=false;
   var front=Quaternion.Euler(0,yaw,0)*Vector3.back;float halfDepth=Mathf.Abs(front.x)*visual.extents.x+Mathf.Abs(front.z)*visual.extents.z;
   var center=new Vector3(visual.center.x,0,visual.center.z);var stand=center+front*(halfDepth+1.05f);var access=center+front*(halfDepth+2.6f);
   var box=new LootContainer {Name=name,Position=stand,View=view,Prop=search,Type=type,Tier=ContainerTier.Common,District=region==DropRegion230.Office?HarborDistrict.Office:region==DropRegion230.Loading?HarborDistrict.Loading:HarborDistrict.Repair,SearchDuration=HarborContent.SearchSeconds(type)};
   box.DropProfile230=ContainerDropRules230.Profile(look,type,ContainerTier.Common,region);box.DropProfile230.Name=name;
   box.DropProfile230.FixedCount=0;ContainerDropRules230.Validate(box.DropProfile230);box.Items.AddRange(ContainerDropRules230.Contents(box.DropProfile230,rng,HardLoot));Loot.Add(box);
   var record=new FutureContainerRecord2717 {Id="FutureSearch2717_"+kind,Kind=kind,Name=name,Category=category,Container=box,View=view,Model=model,Stand=stand,AccessPoint=access,Footprint=new Rect(visual.min.x,visual.min.z,visual.size.x,visual.size.z),VisualBounds=visual,Look=look,Type=type,Region=region};
   FutureContainerRecords2717.Add(record);return record;
  }
  public void BuildFutureContainerShowcase2717(){
   if(!AssetShowcase2717)throw new InvalidOperationException("新容器试玩仅由独立素材展厅调用");
   if(FutureContainerRecords2717.Count!=0)return;
   var rng=new System.Random(2717008);
   for(int i=0;i<8;i++){var record=BuildFutureSearchContainer2717((FutureContainerKind2717)i,new Vector3(35+i*5,.055f,-10),0,rng);GalleryLabel2717(record.Name,new Vector3(record.Stand.x,.071f,record.Stand.z-.52f),.09f,"DAD8B5");}
   ShowcaseSectorStarts2717.Add(new Vector3(31,0,-13.5f));GalleryLabel2717("可搜索容器试玩 · 长按 F · 展厅物品不带出",new Vector3(52.5f,.071f,-16.5f),.12f,"DAD8B5");Physics.SyncTransforms();
  }
 }
}
