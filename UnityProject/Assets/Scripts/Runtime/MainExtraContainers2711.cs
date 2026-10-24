using System.Collections.Generic;
using UnityEngine;
namespace GreyHarbor {
 public sealed class MainExtraPlan2711 {
  public readonly int Floor;public readonly string Name,RoomName;public readonly ContainerLook Look;public readonly SupplyType Supply;public readonly float PreferredZ;public readonly bool SupportTable;
  public MainExtraPlan2711(int floor,string name,string room,ContainerLook look,SupplyType supply,float z,bool support=false){Floor=floor;Name=name;RoomName=room;Look=look;Supply=supply;PreferredZ=z;SupportTable=support;}
 }
 public sealed class MainExtraContainer2711 {
  public MainExtraPlan2711 Plan;public LootContainer Box;public MainSearchPlacement274 Placement;
 }
 public sealed partial class RaidMap {
  static readonly MainExtraPlan2711[] ExtraMainPlans2711={
   new MainExtraPlan2711(1,"一楼待修件周转箱","零件整理与待修件间",ContainerLook.CargoCrate,SupplyType.Tools,11.8f),
   new MainExtraPlan2711(1,"一楼维修耗材柜","维修大厅",ContainerLook.SupplyLocker,SupplyType.Tools,11.8f),
   new MainExtraPlan2711(1,"一楼清洁用品箱","设备与配电间",ContainerLook.StorageBin,SupplyType.Misc,5.8f),
   new MainExtraPlan2711(2,"二楼拆修替换件箱","部件拆修车间",ContainerLook.CargoCrate,SupplyType.Tools,-16.8f),
   new MainExtraPlan2711(2,"二楼检测耗材柜","精密测试间",ContainerLook.SupplyLocker,SupplyType.Misc,11.5f),
   new MainExtraPlan2711(3,"三楼电气备用件箱","电气装配室",ContainerLook.CargoCrate,SupplyType.Tools,-21f),
   new MainExtraPlan2711(3,"三楼会议记录箱","技术会议室",ContainerLook.ArchiveCase,SupplyType.Misc,8.8f,true),
   new MainExtraPlan2711(3,"三楼办公杂物柜","技术设计办公室",ContainerLook.SupplyLocker,SupplyType.Misc,3.2f),
   new MainExtraPlan2711(4,"四楼休息用品柜","职工休息室",ContainerLook.SupplyLocker,SupplyType.Misc,-15.8f),
   new MainExtraPlan2711(4,"四楼会议资料柜","会议室",ContainerLook.FileCabinet,SupplyType.Misc,4.2f)
  };
  public static MainExtraPlan2711[] MainExtraPlans2711(){return (MainExtraPlan2711[])ExtraMainPlans2711.Clone();}
  public static bool TryMainExtraPlan2711(string name,out MainExtraPlan2711 plan){foreach(var entry in ExtraMainPlans2711)if(entry.Name==name){plan=entry;return true;}plan=null;return false;}
  public readonly List<MainExtraContainer2711> MainExtraContainers2711=new List<MainExtraContainer2711>();
  void PlaceMainExtraContainers2711(System.Random random){
   if(MainExtraContainers2711.Count>0)return;
   // Build once with the map. Floor caching retains these same containers and their contents.
   foreach(var plan in ExtraMainPlans2711){
    if(plan.Floor!=RepairFloor220)continue;
    var seed=MainSearchSeed276(plan.Name);
    var box=MainBox240(random,plan.Name,plan.Look,plan.Supply,seed.x,seed.z,seed.x,0,seed.z,0,plan.SupportTable,ContainerTier.Common);
    var placement=MainSearchPlacements274.Find(p=>p.Box==box);
    MainExtraContainers2711.Add(new MainExtraContainer2711{Plan=plan,Box=box,Placement=placement});
   }
  }
  bool MainExtraSlotProtected2711(Bounds moved){
   var area=Rect.MinMaxRect(moved.min.x,moved.min.z,moved.max.x,moved.max.z);
   if(area.Overlaps(new Rect(-2.5f,Bounds.yMin,5,Bounds.height))||area.Overlaps(new Rect(Bounds.xMin,-1.7f,Bounds.width,3.4f)))return true;
   if(RepairFloor220==1&&area.Overlaps(DecorationExpand274(RepairMainInspection210,.45f)))return true;
   foreach(var bridge in RepairMainPublicAreas276(RepairFloor220))if((bridge.width<1.8f||bridge.height<.5f)&&area.Overlaps(DecorationExpand274(bridge,.08f)))return true;
   foreach(var door in Doors){
    if(DecorationNear274(area,door.Position,door.Portal?2:1.8f))return true;
    if(!door.Portal&&(area.Overlaps(DecorationExpand274(door.OpenSafety,.45f))||area.Overlaps(DecorationExpand274(door.ClosedSafety,.45f))))return true;
   }
   foreach(var point in RepairMainWalkPoints210)if(DecorationNear274(area,point,.85f))return true;
   foreach(var room in RepairMainRooms210)if(DecorationNear274(area,room.ApproachPoint,1.1f)||DecorationNear274(area,room.WalkPoint,1.1f))return true;
   return DecorationNear274(area,Spawn,1.7f)||DecorationNear274(area,RepairStairArrival220(true),2.1f)||DecorationNear274(area,RepairStairArrival220(false),2.1f);
  }
 }
}
