using UnityEngine;
using System.Collections.Generic;
namespace GreyHarbor {
 public sealed class GroundMaterialInfo {
  public readonly GroundFinish Finish; public readonly string Name,Category,Scenes,Condition;
  public GroundMaterialInfo(GroundFinish finish,string name,string category,string scenes,string condition){Finish=finish;Name=name;Category=category;Scenes=scenes;Condition=condition;}
 }
 public static class GroundMaterialCatalog {
  // Stable enum values; wear is described separately from the underlying material family.
  public static readonly GroundMaterialInfo[] All={
   new GroundMaterialInfo(GroundFinish.Concrete,"普通水泥","整体硬质地面","仓库、维修棚、车库、储物间","磨损／少量裂缝"),
   new GroundMaterialInfo(GroundFinish.PatchedConcrete,"修补水泥","整体硬质地面","旧车间、维修区、货物堆场","水泥的修补状态"),
   new GroundMaterialInfo(GroundFinish.BrokenTile,"破碎瓷砖","块材铺装","医务室、厨房、卫生间、值班室","缺角／缺砖／露砂浆"),
   new GroundMaterialInfo(GroundFinish.Asphalt,"沥青","道路铺面","港区道路、停车场、运输通道","粗骨料／少量修补"),
   new GroundMaterialInfo(GroundFinish.Gravel,"压实碎石","松散地表","临时堆场、施工便道、道路边缘","碎石嵌入土层"),
   new GroundMaterialInfo(GroundFinish.Soil,"压实泥土","自然地表","院落边缘、荒地、施工区","浅车辙／干湿斑驳"),
   new GroundMaterialInfo(GroundFinish.Terrazzo,"旧水磨石","整体硬质地面","老办公楼、公共走廊、楼梯平台","细碎石粒／稀疏分格条"),
   new GroundMaterialInfo(GroundFinish.Epoxy,"刷漆水泥／环氧地坪","涂层地面","设备间、加工车间、实验区、洁净工作间","磨损涂层／露出水泥"),
   new GroundMaterialInfo(GroundFinish.Steel,"防滑花纹钢板","金属铺面","装卸平台、集装箱入口、船舱、检修平台","防滑纹／螺栓／局部锈斑")
  };
 }
}
