using UnityEngine;
namespace GreyHarbor {
 public sealed partial class RaidGame {
  void PopulateYardOutdoorMonsters140(){
   YardOutdoorMonster140("农田旁",MonsterKind.Wanderer,23,16,23,19,24,17);
   YardOutdoorMonster140("生活小屋西侧",MonsterKind.Runner,11.8f,39,11.8f,36.5f,12,37.5f);
   YardOutdoorMonster140("晾晒生活区",MonsterKind.Spitter,31,35,30.5f,34.5f,31,37);
   YardOutdoorMonster140("东侧作业区",MonsterKind.Shell,49.8f,37,48.5f,38,50.3f,35.5f);
  }
  void YardOutdoorMonster140(string title,MonsterKind kind,float x,float z,float ax,float az,float bx,float bz){
   var p=RaidMap.PortPoint(x,z);var a=RaidMap.PortPoint(ax,az);var b=RaidMap.PortPoint(bx,bz);InteriorMonster("后勤院 / "+title,kind,p.x,p.z,a.x,a.z,b.x,b.z);
  }
  void PopulateYardIndoorMonsters140(){
   if(Map.InteriorId=="yardhome"){
    InteriorMonster("后勤生活小屋 / 起居室",MonsterKind.Wanderer,-3,-1.5f,-4,-1.5f,-3.5f,-2.2f);
    InteriorMonster("后勤生活小屋 / 卧室",MonsterKind.Runner,-2.6f,4.9f,-2.3f,4.65f,-2.4f,5.35f);
    InteriorMonster("后勤生活小屋 / 厨房",MonsterKind.Spitter,3.5f,-1.65f,4.4f,-1.8f,4.3f,-2.2f);
   }else if(Map.InteriorId=="yardshed"){
    InteriorMonster("后勤工具房 / 园艺工具间",MonsterKind.Wanderer,-3.3f,1.8f,-4.1f,2.1f,-3.4f,1.5f);
    InteriorMonster("后勤工具房 / 杂物间",MonsterKind.Wanderer,4.2f,1,4.4f,1.8f,4.25f,.9f);
   }
  }
 }
}
