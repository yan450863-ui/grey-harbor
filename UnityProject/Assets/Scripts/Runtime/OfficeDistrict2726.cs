using System.Collections.Generic;
using UnityEngine;
namespace GreyHarbor {
 public sealed partial class RaidMap {
  public static readonly Vector3 OfficeArrival2726=PortPoint(93,153.6f);
  public static readonly Vector3 OfficePreviewSpawn2726=PortPoint(93,150.2f);
  public readonly List<MainBuildingPlan> OfficeShells2727=new List<MainBuildingPlan>();
  public GameObject OfficeBridge2727;
  public void BuildOfficeDistrict2726(){
   foreach(bool right in new[]{false,true}){
    float x=right?93:70;var p=PortPoint(x,164);string name=right?"办公右塔":"办公左塔";
    var shell=Block(name+"办公外墙",p.x,p.z,16,18,10.2f);shell.AddComponent<FutureMainBuilding>().Floors=3;
    var plan=new MainBuildingPlan(name,new Rect(p.x-8,p.z-9,16,18),3,10.2f,right?OfficeArrival2726:PortPoint(70,153.6f));
    MainBuildings.Add(plan);OfficeShells2727.Add(plan);
    BuildOfficeFacade2746(shell,right);
    var front=PortPoint(x,155);
    if(right)Doors.Add(new WorldDoor{Name="进入办公右塔一楼",Destination="officeright1",Portal=true,Position=front-Vector3.forward*.30f});
    else BuildOfficeBlockedCar2746();
   }
   // Upper-level exterior link only; first-floor courtyard stays passable below.
   var bridge=PortPoint(81.5f,167.5f);OfficeBridge2727=Box("OfficeSecondFloorBridge2727",bridge+Vector3.up*3.4f,new Vector3(7,.20f,3),"A6C3BE",true);
   foreach(float dz in new[]{-1.45f,1.45f})Box("OfficeBridgeRail2727",bridge+new Vector3(0,4.1f,dz),new Vector3(7,1.2f,.10f),"627B76",true);
   BuildOfficeOutdoorDetails2746();
  }
 }
 public static class OfficePreview2726 {
  public static bool Apply(RaidGame g){
   if(!g||g.IsLoading2721||!g.Map.HarborWhitebox)return false;
   var p=RaidMap.OfficePreviewSpawn2726;if(!g.Map.HarborSurfaceClear(p,.34f)||!g.Navigation.ClearSegment(p,p))return false;
   g.Player.transform.position=p;Physics.SyncTransforms();g.Rig.ResetView(45);g.Rig.Tick(0);g.Vision.Refresh(p,g.Player.Aim);g.Notify("办公右塔：右塔三层办公内装，左塔三层内装与二楼连廊开放。");return true;
  }
 }
 public sealed partial class RaidUI {
  bool OfficeGuide2726(){
   var pos=G.Player.transform.position;bool inside=G.Map.OfficeWhitebox2726;
   if(!inside&&(!G.Map.HarborWhitebox||!new Rect(60-79,145-73,43,30).Contains(new Vector2(pos.x,pos.z))))return false;
   Fill(new Rect(28,109,353,90),panel);string name="办公双子塔";
   if(inside){foreach(var r in G.Map.OfficeRooms2726)if(r.Area.Contains(new Vector2(pos.x,pos.z))){name=r.Name;break;}}
   Label(48,118,320,24,name,18,mint);
   Label(48,145,320,20,inside?"F开关房门 · 楼梯长按F两秒":"右塔正门长按F进入 · M全图",13,muted);
   string guide=!inside?"左右塔分离 · 二楼连廊连接":G.Map.IsOfficeBridge2741?"长按F两秒进入左塔或右塔":G.Map.OfficeLeft2741?(G.Map.OfficeFloor2733==1?"左塔1楼 · 后勤值班室钥匙房":G.Map.OfficeFloor2733==2?"左塔2楼 · 连廊连接右塔":"左塔3楼 · 技术主管室开放"):(G.Map.OfficeFloor2733==1?"右塔1楼 · 家具与搜索点已接入":G.Map.OfficeFloor2733==2?"右塔2楼 · 连廊连接左塔":"右塔3楼 · 研究室钥匙房");Label(48,169,320,20,guide,13,muted);if(inside)OfficeStairLabels2733();return true;
  }
 }
}
