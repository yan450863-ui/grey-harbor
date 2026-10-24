using UnityEngine;
namespace GreyHarbor {
 public sealed partial class RaidMap {
  public static string MainSearchRoom276(string name){
   if(TryMainExtraPlan2711(name,out var extra))return extra.RoomName;
   switch(name){
    case "待修零件储料箱":return "零件整理与待修件间";
    case "配电设备柜":return "设备与配电间";
    case "维修值班记录柜":case "值班急救柜":return "维修值班办公室";
    case "拆修台工具箱":case "部件运输箱":return "部件拆修车间";
    case "精密测试设备箱":return "精密测试间";
    case "校验工具箱":return "工具校验间";
    case "二楼 · 小保险箱":case "备件维护材料箱":return "维修工艺与备件整理间";
    case "电气装配设备柜":return "电气装配室";
    case "仪表测试电子箱":return "继电与仪表测试室";
    case "技术设计档案箱":return "技术设计办公室";
    case "维修登记文件柜":return "维修记录登记室";
    case "三楼 · 小保险箱":return "工程资料档案室";
    case "配电监测精密箱":return "配电监测办公室";
    case "职工遗留手提箱":return "职工休息室";
    case "更衣室旧外套":return "更衣与清洁室";
    case "人事排班档案箱":return "人事与排班办公室";
    case "管理电子设备箱":case "四楼 · 大保险箱":return "管理办公室";
    case "值班管理文件柜":return "值班管理室";
    default:return "维修大厅";
   }
  }
  Rect MainSearchArea276(string name){
   string room=MainSearchRoom276(name);
   if(room!="维修大厅")return RepairMainRoomArea276(RepairFloor220,room);
   if(name=="二楼巡检弹药箱")return new Rect(2.6f,1.8f,17.44f,6.1f);
   if(name=="楼层装备补给箱"||name=="楼层急救柜")return new Rect(-20.04f,-1.7f,40.08f,3.4f);
   return MainHall276;
  }
  Vector3 MainSearchSeed276(string name){
   Rect area=MainSearchArea276(name);float t=.5f;
   if(TryMainExtraPlan2711(name,out var extra))return new Vector3(area.center.x,0,extra.PreferredZ);
   if(name=="维修值班记录柜"||name=="二楼 · 小保险箱"||name=="四楼 · 大保险箱")t=.25f;
   if(name=="值班急救柜"||name=="备件维护材料箱"||name=="管理电子设备箱")t=.75f;
   return new Vector3(area.center.x,0,Mathf.Lerp(area.yMin,area.yMax,t));
  }
 }
}
