using UnityEngine;
namespace GreyHarbor {
 public sealed partial class RaidMap {
  bool MainPublicDressed276;
  // Small public service groups use the same real collision and circulation checks as room dressing.
  public void AddMainPublicDressing276(){
   if(!RepairMain220||!Root||MainPublicDressed276||RepairFloor220<1||RepairFloor220>4)return;
   MainPublicDressed276=true;Physics.SyncTransforms();
   if(RepairFloor220==1){
    DecorationCenterIsland274(new RepairMainRoom210{Name="维修接件登记区",Area=new Rect(MainHall276.xMin,-MainNetHalfZ276,11.5f,12f)});
    DecorationCenterIsland274(new RepairMainRoom210{Name="维修完成签字区",Area=Rect.MinMaxRect(2.8f,11.9f,MainHall276.xMax,MainNetHalfZ276)});
   }else{
    DecorationCenterIsland274(new RepairMainRoom210{Name="楼层等候与记录区",Area=Rect.MinMaxRect(-14.3f,16.3f,-2.5f,MainNetHalfZ276)});
    if(RepairFloor220>=3)DecorationCenterIsland274(new RepairMainRoom210{Name="公共等候与档案资料区",Area=Rect.MinMaxRect(-MainNetHalfX276,10.5f,-9.9f,16.2f)});
   }
   Physics.SyncTransforms();
  }
 }
}
