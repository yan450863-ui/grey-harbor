using UnityEngine;
namespace GreyHarbor {
 public sealed partial class RaidMap {
  public LootContainer AddContainer(Vector3 stand,Vector3 model,ContainerLook look,SupplyType type,System.Random rng,HarborDistrict district,float yaw=0,bool support=true,ContainerTier tier=ContainerTier.Auto,DropRegion230? region=null,SafeSize240 safeSize=SafeSize240.Legacy,int fixedCount=0,bool bonusRed=true){
   string[] names={"手提箱","挂着的外套","工具箱","壁挂药柜","文件柜","后勤物资柜","保险箱","弹药箱","军用物资箱","拉杆行李箱","床头抽屉","旧衣柜","厨房壁柜","带盖储物箱","珠宝收藏盒","精密电子设备箱","密封档案箱","防雨收纳箱","农具长箱","后勤木货箱","电气检修柜","化工储料桶","保温冷藏箱","渔具长箱","防水运输箱","焊接工具箱","救援用品柜","船用电控箱","防水文件筒"};
   CollectionCategory? category=HighValueContainers139.Category(look);if(category.HasValue){type=SupplyType.Misc;tier=ContainerTier.Common;}
   if(OutdoorContainers140.IsOutdoor(look)){category=OutdoorContainers140.Category(look);type=OutdoorContainers140.Supply(look);}
   if(DockContainers141.IsDock(look)){category=DockContainers141.Category(look);type=DockContainers141.Supply(look);if(look==ContainerLook.DocumentTube&&tier==ContainerTier.Auto)tier=ContainerTier.Common;}
   var prop=new ContainerArt(this).Build(look,model,yaw,support);if(safeSize!=SafeSize240.Legacy)MainSafeRules240.Dress(prop,safeSize);
   var l=new LootContainer{Position=stand,Name=names[(int)look],View=prop.gameObject,Prop=prop,Type=type,Tier=LootBalance.TierFor(type,tier),District=district,SearchDuration=HarborContent.SearchSeconds(type)};l.DropProfile230=ContainerDropRules230.Profile(look,type,l.Tier,region??DropRegionFor230(stand,district),safeSize);l.DropProfile230.FixedCount=fixedCount;l.DropProfile230.EnableBonusRed=bonusRed;ContainerDropRules230.Validate(l.DropProfile230);l.Items.AddRange(ContainerDropRules230.Contents(l.DropProfile230,rng,HardLoot));Loot.Add(l);return l;
  }
  void TrainingContainers(){
   var r=new System.Random(System.Environment.TickCount);
   AddContainer(new Vector3(-11.9f,0,-6.15f),new Vector3(-12.25f,.865f,-5.1f),ContainerLook.Toolbox,SupplyType.Tools,r,HarborDistrict.Repair,0,false);
   AddContainer(new Vector3(-8.65f,0,-1.8f),new Vector3(-9.89f,0,-1.8f),ContainerLook.MedicalCabinet,SupplyType.Medical,r,HarborDistrict.Repair,-90,false);
   AddContainer(new Vector3(7,0,9.1f),new Vector3(7,0,10.25f),ContainerLook.SupplyLocker,SupplyType.Misc,r,HarborDistrict.Loading);
   AddContainer(new Vector3(8,0,6.45f),new Vector3(8,.30f,7.6f),ContainerLook.AmmoCase,SupplyType.Ammo,r,HarborDistrict.Loading,0,false);
   AddContainer(new Vector3(14.25f,0,5.2f),new Vector3(13.25f,0,5.2f),ContainerLook.MilitaryCase,SupplyType.Military,r,HarborDistrict.Containers,90);
   AddContainer(new Vector3(4.8f,0,-13),new Vector3(6,0,-13),ContainerLook.Safe,SupplyType.Safe,r,HarborDistrict.Office,90);
   Box("SafeWeatherCanopy",new Vector3(5.95f,2.1f,-13),new Vector3(1.55f,.14f,1.8f),"65776B");
   foreach(float z in new[]{-13.8f,-12.2f})Box("CanopyBracket",new Vector3(6.45f,1.88f,z),new Vector3(.11f,.44f,.08f),"465B50");
  }
 }
}
