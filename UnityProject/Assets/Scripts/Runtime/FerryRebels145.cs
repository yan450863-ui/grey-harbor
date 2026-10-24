using UnityEngine;
namespace GreyHarbor {
 public sealed partial class RaidGame {
  void FerrySoldier146(RebelKind kind,string room,float x,float z,float ax,float az,float bx,float bz){
   var e=SpawnRebel145(new Vector3(x,0,z),kind,room);
   e.PatrolTargets=new[]{e.Home,new Vector3(ax,0,az),new Vector3(bx,0,bz)};
  }
  void PopulateFerryRebels145(){
   if(Map.InteriorId=="ferry1"){
    FerrySoldier146(RebelKind.Regular,"行李舱",-3.7f,-10,-3.7f,-12.5f,-2.6f,-9);
    FerrySoldier146(RebelKind.BlueRegular,"一楼客舱",0,8,0,5,0,10);
    InteriorMonster("服务间",MonsterKind.Wanderer,3.5f,-4.7f,3.1f,-4.3f,4.5f,-5.3f);
    InteriorMonster("一楼客舱",MonsterKind.Runner,1.9f,9.7f,1.9f,7.8f,1.9f,5);
   }else{
    FerrySoldier146(RebelKind.Heavy,"二楼观景休息区",1,-8.5f,1,-5,1,-10.5f);
    FerrySoldier146(RebelKind.Regular,"二楼客舱",0,6,0,4,0,8.5f);
    InteriorMonster("观景休息区西翼",MonsterKind.Wanderer,-4.2f,-6,-2.1f,-6,-2.1f,-4);
    InteriorMonster("驾驶室",MonsterKind.Spitter,5.2f,12.7f,5.2f,11.6f,5.2f,13.8f);
   }
  }
 }
}
