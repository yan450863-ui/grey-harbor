using UnityEngine;
namespace GreyHarbor {
 public sealed partial class RaidMap {
  void BuildOfficeLeftFirstDressing2742(){
   // The key-room hook furnishes the duty room separately; other stores and workshops use equipment and cargo.
   foreach(float x in new[]{-12f,-10.4f,-5.3f})OfficeProp2736("01",FutureProp2717.PersonalLocker,x,-23.35f,180);
   OfficeProp2736("01",FutureProp2717.WaitingBench,-11,-17,0);
   OfficeProp2736("01",FutureProp2717.OfficeSofa,-5,-17,0);
   OfficeProp2736("01",FutureProp2717.CoffeeCorner,-12.3f,-21,270);
   OfficeProp2736("01",FutureProp2717.StaffFridge,-12.6f,-18.8f,270);
   OfficeProp2736("01",FutureProp2717.CoatStand,-9,-23,180);
   OfficeProp2736("01",FutureProp2717.PortableFan,-6,-22.2f,180);
   OfficeProp2736("01",FutureProp2717.WaterDispenser,-6.5f,-17,0);
   OfficeProp2736("01",FutureProp2717.OverflowWasteBin,-12.8f,-22.9f,0);
   OfficeProp2736("01",FutureProp2717.PaperScatter,-10,-18.4f,12);
   OfficeWall2736("01",FutureProp2717.NoticeBoard,-10,1.4f,-23.82f,180);
   OfficeWall2736("01",FutureProp2717.WallClock,-5,2.2f,-23.82f,180);
   OfficeWall2736("01",FutureProp2717.WallAirConditioner,-13.82f,2.3f,-22,270);

   foreach(float x in new[]{-14f,-12f})OfficeProp2736("02",FutureProp2717.OpenArchiveShelf,x,-15.3f,180);
   foreach(float x in new[]{-13.5f,-8f})OfficeProp2736("02",FutureProp2717.CargoCage,x,-7.2f,0);
   OfficeProp2736("02",FutureProp2717.WrappedPallet,-14.5f,-12.2f,7);
   OfficeProp2736("02",FutureProp2717.OpenCargoCrate,-7,-12,352);
   OfficeProp2736("02",FutureProp2717.ToolTrolley,-5,-7,0);
   OfficeProp2736("02",FutureProp2717.ForkliftBattery,-10.5f,-14.9f,180);
   OfficeProp2736("02",FutureProp2717.HandTruck,-15.5f,-7,0);
   OfficeProp2736("02",FutureProp2717.CargoStraps,-13,-9.3f,18);
   OfficeProp2736("02",FutureProp2717.SpilledPacking,-7,-8.9f,4);
   OfficeProp2736("02",FutureProp2717.PaperBoxStack,-5,-15,180);
   OfficeProp2736("02",FutureProp2717.FireExtinguisher,-3,-15,180);
   OfficeWall2736("02",FutureProp2717.NoticeBoard,-13,1.5f,-6.18f,0);
   OfficeWall2736("02",FutureProp2717.WallClock,-5,2.2f,-6.18f,0);

   foreach(float x in new[]{6f,11f,15f})OfficeProp2736("03",FutureProp2717.TunnelWorkBench,x,-23,180);
   OfficeProp2736("03",FutureProp2717.TunnelWorkBench,4,-10,270);
   OfficeProp2736("03",FutureProp2717.PumpAssembly,7,-19,10);
   OfficeProp2736("03",FutureProp2717.Winch,14,-19,352);
   OfficeProp2736("03",FutureProp2717.PipeManifold,7,-8,0);
   OfficeProp2736("03",FutureProp2717.ForkliftBattery,12,-8,0);
   OfficeProp2736("03",FutureProp2717.ToolTrolley,15,-8,0);
   OfficeProp2736("03",FutureProp2717.ToolTrolley,8,-21.3f,180);
   OfficeProp2736("03",FutureProp2717.HandTruck,4,-21,270);
   OfficeProp2736("03",FutureProp2717.CargoStraps,13,-17.5f,18);
   OfficeProp2736("03",FutureProp2717.WheelChocks,6,-17,15);
   OfficeProp2736("03",FutureProp2717.ServiceLadder,4,-23,180);
   OfficeProp2736("03",FutureProp2717.PortableFan,16,-21.5f,180);
   OfficeProp2736("03",FutureProp2717.OverflowWasteBin,6,-7,0);
   OfficeProp2736("03",FutureProp2717.InsulationDebris,14,-9.6f,6);
   OfficeProp2736("03",FutureProp2717.FireExtinguisher,3,-7,0);
   OfficeWall2736("03",FutureProp2717.NoticeBoard,9,1.5f,-6.18f,0);
   OfficeWall2736("03",FutureProp2717.WallClock,14,2.2f,-6.18f,0);
   OfficeWall2736("03",FutureProp2717.WallAirConditioner,19.82f,2.3f,-16,90);

   // The southern and northern doors at x=-12 stay joined by an unobstructed freight aisle.
   foreach(float z in new[]{0f,7.7f})OfficeProp2736("04",FutureProp2717.WrappedPallet,z<1?-15.3f:-16f,z<1?z:5.5f,z<1?7:354);
   OfficeProp2736("04",FutureProp2717.CargoCage,-7.5f,1,7);
   OfficeProp2736("04",FutureProp2717.OpenCargoCrate,-5,4.8f,352);
   OfficeProp2736("04",FutureProp2717.LoadingScale,-7,7,0);
   OfficeProp2736("04",FutureProp2717.PalletJack,-9,8.6f,340);
   OfficeProp2736("04",FutureProp2717.RollerConveyor,-7,-1.5f,0);
   OfficeProp2736("04",FutureProp2717.HandTruck,-3,1,90);
   OfficeProp2736("04",FutureProp2717.CargoStraps,-8,3,18);
   OfficeProp2736("04",FutureProp2717.SpilledPacking,-15.5f,2.7f,6);
   OfficeProp2736("04",FutureProp2717.WheelChocks,-14.8f,5,350);
   OfficeProp2736("04",FutureProp2717.PaperBoxStack,-3,8,90);
   OfficeProp2736("04",FutureProp2717.FireExtinguisher,-3,-2,180);
   OfficeWall2736("04",FutureProp2717.NoticeBoard,-19.82f,1.5f,6,270);
   OfficeWall2736("04",FutureProp2717.WallClock,-5,2.2f,-2.82f,180);

   foreach(float x in new[]{6f,9f,12f,15f})OfficeProp2736("05",FutureProp2717.ControlPanel,x,10.5f,0);
   OfficeProp2736("05",FutureProp2717.FloorServicePanel,19.5f,1,90);
   OfficeProp2736("05",FutureProp2717.TunnelWorkBench,7,-2,180);
   OfficeProp2736("05",FutureProp2717.ToolTrolley,4,-1.5f,180);
   OfficeProp2736("05",FutureProp2717.ForkliftBattery,11,-1.9f,180);
   OfficeProp2736("05",FutureProp2717.PumpAssembly,15,-1.5f,180);
   OfficeProp2736("05",FutureProp2717.CableTray,4,8,0);
   OfficeProp2736("05",FutureProp2717.InsulationDebris,6,7,8);
   OfficeProp2736("05",FutureProp2717.ServiceLadder,19.3f,5,90);
   OfficeProp2736("05",FutureProp2717.FireExtinguisher,3,10,0);
   OfficeWall2736("05",FutureProp2717.UtilityMeter,19.65f,1.15f,-1,90);
   OfficeWall2736("05",FutureProp2717.NoticeBoard,7,1.5f,-2.82f,180);
   OfficeWall2736("05",FutureProp2717.WallClock,12,2.2f,-2.82f,180);

   var living=new FerryLivingKit144(this);var repair=new RepairInteriorKit200(this);
   OfficeTrack2736("07","Toilet",living.Toilet(new Vector3(-7,0,22.6f),0).gameObject);
   OfficeTrack2736("07","Toilet",living.Toilet(new Vector3(-4,0,22.6f),0).gameObject);
   OfficeTrack2736("07","WashStation",repair.WashStation(new Vector3(-7.4f,0,18),270).gameObject);
   OfficeProp2736("07",FutureProp2717.MopBucket,-7,15.2f,180);
   OfficeProp2736("07",FutureProp2717.CleaningCloset,-2.6f,21,90);
   OfficeProp2736("07",FutureProp2717.OverflowWasteBin,-7,20.7f,270);
   OfficeProp2736("07",FutureProp2717.PaperScatter,-3.3f,22,10);
   OfficeWall2736("07",FutureProp2717.WallClock,-4,2.2f,23.82f,0);

   foreach(float x in new[]{4f,10f})OfficeProp2736("08",FutureProp2717.OpenArchiveShelf,x,23.4f,0);
   OfficeProp2736("08",FutureProp2717.TunnelWorkBench,7,14.8f,180);
   OfficeProp2736("08",FutureProp2717.ToolTrolley,3.5f,15.5f,180);
   OfficeProp2736("08",FutureProp2717.PersonalLocker,3,22,270);
   OfficeProp2736("08",FutureProp2717.HandTruck,11,21.5f,0);
   OfficeProp2736("08",FutureProp2717.PaperBoxStack,4,20.8f,270);
   OfficeProp2736("08",FutureProp2717.ForkliftBattery,9.5f,14.9f,180);
   OfficeProp2736("08",FutureProp2717.PortableFan,9,21.7f,0);
   OfficeProp2736("08",FutureProp2717.CargoStraps,6,21,8);
   OfficeProp2736("08",FutureProp2717.FireExtinguisher,3,14.8f,180);
   OfficeWall2736("08",FutureProp2717.NoticeBoard,7,1.5f,23.82f,0);
   OfficeWall2736("08",FutureProp2717.WallClock,11.5f,2.2f,23.82f,0);
   foreach(string id in new[]{"S1","S2"}){
    var r=OfficeRooms2726.Find(a=>a.Id==id);
    OfficeProp2736(id,FutureProp2717.FireExtinguisher,r.Area.xMax-.5f,r.Area.yMin+.6f,0);
    OfficeWall2736(id,FutureProp2717.NoticeBoard,r.Area.xMax-.18f,1.5f,r.Area.yMax-1,90);
   }
   BuildOfficeLeftFirstContainers2742();Physics.SyncTransforms();
  }
  void BuildOfficeLeftFirstContainers2742(){
   var rng=new System.Random((RaidGame.Instance?RaidGame.Instance.RaidSeed:2742)^202742);
   OfficeLoot2736("01","维修员工个人行李",ContainerLook.RollingSuitcase,-3,-22,90,rng);
   OfficeLoot2736("01","更衣室个人物品柜",ContainerLook.SupplyLocker,-3,-18,90,rng);
   OfficeLoot2736("02","机修备用零件箱",ContainerLook.StorageBin,-19.2f,-14,270,rng);
   OfficeLoot2736("02","电机配件储物箱",ContainerLook.CargoCrate,-19.2f,-10.5f,270,rng);
   OfficeLoot2736("02","封存备件工具箱",ContainerLook.Toolbox,-19.2f,-7.2f,270,rng);
   OfficeLoot2736("02","备件领用记录柜",ContainerLook.FileCabinet,-3,-8,90,rng);
   OfficeLoot2736("03","维修焊接工具箱",ContainerLook.WeldingToolbox,19.2f,-22,90,rng);
   OfficeLoot2736("03","机修手工具箱",ContainerLook.Toolbox,19.2f,-18,90,rng);
   OfficeLoot2736("03","维修配件储物箱",ContainerLook.StorageBin,19.2f,-14,90,rng);
   OfficeLoot2736("03","维修耗材柜",ContainerLook.SupplyLocker,19.2f,-10,90,rng);
   OfficeLoot2736("04","待转运货物箱",ContainerLook.CargoCrate,-19.2f,0,270,rng);
   OfficeLoot2736("04","卸货待检储物箱",ContainerLook.StorageBin,-19.2f,4,270,rng);
   OfficeLoot2736("04","封装运输工具箱",ContainerLook.WaterproofTransitCase,-19.2f,8,270,rng);
   OfficeLoot2736("04","暂存货物交接柜",ContainerLook.FileCabinet,-5.8f,10.3f,0,rng);
   OfficeLoot2736("05","备用电气工具箱",ContainerLook.Toolbox,19.2f,8.5f,90,rng);
   OfficeLoot2736("05","配电检修备件柜",ContainerLook.ElectricalCabinet,19.2f,-1.5f,90,rng);
   OfficeLoot2736("07","卫生间应急药柜",ContainerLook.MedicalCabinet,-2.2f,15.4f,90,rng,true);
   OfficeLoot2736("08","手工具保管箱",ContainerLook.Toolbox,13.2f,15.5f,90,rng);
   OfficeLoot2736("08","电动工具箱",ContainerLook.WeldingToolbox,13.2f,19.5f,90,rng);
   OfficeLoot2736("08","维修工具储物柜",ContainerLook.SupplyLocker,13.2f,23,90,rng);
   OfficeLeftSafe2742("08","维修工具库大保险箱",new Vector3(7,0,23),0,SafeSize240.Large,rng);
  }
 }
}
