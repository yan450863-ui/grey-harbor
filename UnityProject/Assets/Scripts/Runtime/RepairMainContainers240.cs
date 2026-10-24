using UnityEngine;
namespace GreyHarbor {
 public sealed partial class RaidMap {
  void PlaceRepairMainContainers240(){
   var r=new System.Random(System.Environment.TickCount^RepairFloor220*24001);
   if(RepairFloor220==1){
    MainBox240(r,"检修工具箱",ContainerLook.Toolbox,SupplyType.Tools,-6.6f,-7.9f,-7.95f,.92f,-7.9f,270,false,ContainerTier.Common);
    MainBox240(r,"精密拆修工具箱",ContainerLook.Toolbox,SupplyType.Tools,-6.6f,2.7f,-7.95f,.92f,2.7f,270,false,ContainerTier.Common);
    MainBox240(r,"待修零件储料箱",ContainerLook.CargoCrate,SupplyType.Tools,-10.1f,-.5f,-11.5f,0,-.5f,270,false,ContainerTier.Common);
    MainBox240(r,"配电设备柜",ContainerLook.ElectricalCabinet,SupplyType.Misc,12,8.3f,12.6f,0,7,90,false,ContainerTier.Common);
    MainBox240(r,"维修值班记录柜",ContainerLook.FileCabinet,SupplyType.Misc,12.1f,-1.7f,12.75f,0,-2.85f,90,false,ContainerTier.Common);
    MainBox240(r,"值班急救柜",ContainerLook.MedicalCabinet,SupplyType.Medical,11.7f,-.25f,12.95f,0,-.25f,90,false,ContainerTier.Common);
    MainBox240(r,"巡检弹药箱",ContainerLook.AmmoCase,SupplyType.Ammo,6.5f,13.2f,6.5f,0,14.7f,0,true,ContainerTier.Common);
   }else if(RepairFloor220==2){
    MainBox240(r,"拆修台工具箱",ContainerLook.Toolbox,SupplyType.Tools,-7.6f,-3.5f,-7.6f,.94f,-5,0,false,ContainerTier.Common);
    MainBox240(r,"部件运输箱",ContainerLook.CargoCrate,SupplyType.Tools,-5.25f,-11.7f,-4,0,-11.7f,90,false,ContainerTier.Common);
    MainBox240(r,"精密测试设备箱",ContainerLook.PrecisionCase,SupplyType.Misc,-10.3f,4.5f,-11.8f,0,4.5f,270,true,ContainerTier.Common);
    MainBox240(r,"校验工具箱",ContainerLook.Toolbox,SupplyType.Tools,8,-2.45f,8,0,-3.4f,180,true,ContainerTier.Common);
    MainBox240(r,"二楼 · 小保险箱",ContainerLook.Safe,SupplyType.Safe,10.5f,12.8f,11.9f,0,12.8f,90,false,ContainerTier.Safe,SafeSize240.Small);
    MainBox240(r,"备件维护材料箱",ContainerLook.CargoCrate,SupplyType.Tools,8,12,8,0,13.3f,0,false,ContainerTier.Common);
    MainBox240(r,"楼层装备补给箱",ContainerLook.MilitaryCase,SupplyType.Military,8.5f,0,10,0,0,90,true,ContainerTier.Common,SafeSize240.Legacy,1,false);
    MainBox240(r,"二楼巡检弹药箱",ContainerLook.AmmoCase,SupplyType.Ammo,6.5f,5.8f,6.5f,0,7.15f,0,true,ContainerTier.Common);
   }else if(RepairFloor220==3){
    MainBox240(r,"电气装配设备柜",ContainerLook.ElectricalCabinet,SupplyType.Misc,-10.4f,-12,-11.8f,0,-12,270,false,ContainerTier.Common);
    MainBox240(r,"仪表测试电子箱",ContainerLook.PrecisionCase,SupplyType.Misc,-10.4f,-5,-11.8f,0,-5,270,true,ContainerTier.Common);
    MainBox240(r,"技术设计档案箱",ContainerLook.ArchiveCase,SupplyType.Misc,-8,3.5f,-8,0,4.8f,0,true,ContainerTier.Common);
    MainBox240(r,"维修登记文件柜",ContainerLook.FileCabinet,SupplyType.Misc,7.8f,-13.75f,7.8f,0,-15.1f,180,false,ContainerTier.Common);
    MainBox240(r,"三楼 · 小保险箱",ContainerLook.Safe,SupplyType.Safe,4.65f,-3.5f,3.35f,0,-3.1f,270,false,ContainerTier.Safe,SafeSize240.Small);
    MainBox240(r,"配电监测精密箱",ContainerLook.PrecisionCase,SupplyType.Misc,8,10.85f,8,0,12.2f,0,true,ContainerTier.Common);
    MainBox240(r,"楼层急救柜",ContainerLook.MedicalCabinet,SupplyType.Medical,-1.3f,-7,-2.5f,0,-7,270,false,ContainerTier.Common);
   }else{
    MainBox240(r,"职工遗留手提箱",ContainerLook.Suitcase,SupplyType.Misc,-6.4f,-10.15f,-7,.68f,-9.15f,0,false,ContainerTier.Common);
    MainBox240(r,"更衣室旧外套",ContainerLook.Coat,SupplyType.Misc,-11.8f,-5.2f,-13.08f,0,-5.2f,270,false,ContainerTier.Common);
    MainBox240(r,"人事排班档案箱",ContainerLook.ArchiveCase,SupplyType.Misc,4.6f,3,3.4f,0,3,270,true,ContainerTier.Common);
    MainBox240(r,"管理电子设备箱",ContainerLook.PrecisionCase,SupplyType.Misc,-8,3.5f,-8,0,4.8f,0,true,ContainerTier.Common);
    MainBox240(r,"四楼 · 大保险箱",ContainerLook.Safe,SupplyType.Safe,-10.35f,3.1f,-11.7f,0,3.1f,270,false,ContainerTier.Safe,SafeSize240.Large);
    MainBox240(r,"值班管理文件柜",ContainerLook.FileCabinet,SupplyType.Misc,7.8f,-13.75f,7.8f,0,-15.1f,180,false,ContainerTier.Common);
   }
   PlaceMainExtraContainers2711(r);
  }
  LootContainer MainBox240(System.Random r,string name,ContainerLook look,SupplyType type,float sx,float sz,float mx,float my,float mz,float yaw,bool support,ContainerTier tier,SafeSize240 size=SafeSize240.Legacy,int fixedCount=0,bool bonusRed=true){
   // Move the full-size prop as one unit; retain its nearby, ground-level search approach.
   var model=MainSearchSeed276(name);model.y=my;var legacy=model;bool existing=RepairMainSupportAnchor274(RepairFloor220,name,out var anchor,out var supportYaw);
   if(existing){model=anchor;yaw=supportYaw;model.y=name=="拆修台工具箱"?.75f:name=="职工遗留手提箱"?.68f:.91f;if(name=="拆修台工具箱")model.x+=.65f;}
   var stand=model+new Vector3(sx-mx,-model.y,sz-mz);mx=model.x;my=model.y;mz=model.z;GameObject table=null;
   if(support&&look>=ContainerLook.JewelryBox&&look<=ContainerLook.ArchiveCase){table=Box("SearchSupportTable240",new Vector3(mx,my+.42f,mz),new Vector3(1.5f,.84f,.95f),"85816A",true);SceneObstacle.Apply(table,ObstacleKind.Low);table.transform.rotation=Quaternion.Euler(0,yaw,0);my+=.865f;support=false;}
   var box=AddContainer(stand,new Vector3(mx,my,mz),look,type,r,HarborDistrict.Repair,yaw,support,tier,DropRegion230.Repair,size,fixedCount,bonusRed);box.Name=name;if(size==SafeSize240.Large)box.SearchDuration=8;PlaceMainSearch274(box,table,existing,legacy);return box;
  }
 }
}
