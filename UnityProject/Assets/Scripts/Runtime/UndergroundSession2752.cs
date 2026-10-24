using System;using System.Collections.Generic;using UnityEngine;
namespace GreyHarbor {
 public sealed partial class RaidMap {
  public readonly List<WorldDoor> UndergroundSurfaceEntries2752=new List<WorldDoor>();
  static readonly string[] UndergroundSurfaceIds2752={"freight","repair","cargo","office","loading","south"};
  static readonly string[] UndergroundSurfaceNames2752={"西北货运场","维修附属房","集装箱区检修井","办公楼设备区","装卸区","南部入口附近"};
  public void AddUndergroundSurfaceEntries2752(){
   if(!HarborWhitebox||UndergroundSurfaceEntries2752.Count>0)return;
   var preferred=new[]{PortPoint(18,150),PortPoint(42,79),PortPoint(121,102),PortPoint(103,149),PortPoint(129,149),PortPoint(74,47)};
   Physics.SyncTransforms();
   for(int i=0;i<preferred.Length;i++){
    Vector3 p=UndergroundSurfaceSafePoint2752(preferred[i]);
    var portal=new WorldDoor{Name="进入地下管网 · "+UndergroundSurfaceNames2752[i],Destination="underground-enter:"+UndergroundSurfaceIds2752[i],Portal=true,Position=p};
    UndergroundSurfaceEntries2752.Add(portal);Doors.Add(portal);
    BuildOpenSurfaceHatch2756(p,i);
   }
  }
  Vector3 UndergroundSurfaceSafePoint2752(Vector3 preferred){
   for(int ring=0;ring<=16;ring++)for(int ix=-ring;ix<=ring;ix++)for(int iz=-ring;iz<=ring;iz++){
    if(ring>0&&Mathf.Max(Mathf.Abs(ix),Mathf.Abs(iz))!=ring)continue;
    var p=preferred+new Vector3(ix*.5f,0,iz*.5f);
    if(!OnHarborSurface(p)||!HarborSurfaceClear(p,.9f)||Physics.CheckCapsule(p+Vector3.up*.35f,p+Vector3.up*1.35f,.85f,SceneObstacle.MovementMask,QueryTriggerInteraction.Ignore))continue;
    bool close=false;foreach(var d in Doors)if(Vector3.Distance(d.Position,p)<2.5f){close=true;break;}
    if(!close)foreach(var loot in Loot)if(Vector3.Distance(loot.Position,p)<2.2f){close=true;break;}
    if(!close)return p;
   }
   throw new InvalidOperationException("No safe underground surface connection near "+preferred);
  }
 }
 public sealed partial class RaidGame {
  internal static RaidNavigation UndergroundNavigation2752(RaidMap map)=>new RaidNavigation(map.Bounds,.25f,p=>map.UndergroundFloorContains2752(p,.34f));
  bool TrySwitchUnderground2752(string destination){
   bool entering=destination!=null&&destination.StartsWith("underground-enter:");
   bool leaving=destination!=null&&destination.StartsWith("underground-exit:");
   if(!entering&&!leaving)return false;
   if(Phase!=GamePhase.Raid||Map==null)return true;
   string id=destination.Substring(entering?18:17),key="underground2752";
   if(entering&&(!Map.HarborWhitebox||Map.IsInterior))return true;
   if(leaving&&(!Map.UndergroundWhitebox2752||outsideArea==null))return true;
   var exterior=entering?Map:outsideArea.Map;
   var portal=exterior.UndergroundSurfaceEntries2752.Find(d=>d.Destination=="underground-enter:"+id);
   if(portal==null)return true;
   if(entering)outsideArea=new AreaState{Map=Map,Vision=Vision,Navigation=Navigation,Yaw=Rig.DestinationYaw,Zoom=View.orthographicSize,Actors=new List<RaidActor>(Enemies)};
   else {var current=indoorAreas[key];current.Yaw=Rig.DestinationYaw;current.Navigation=Navigation;current.Actors=new List<RaidActor>(Enemies);}
   foreach(var e in Enemies){e.Windup=e.Lunge=e.AttackPose=0;e.RebelBurst=0;e.AttackCooldown=Mathf.Max(e.AttackCooldown,1);}
   CancelArmorRepair();CloseInventory();ClearShots();Enemies.Clear();Player.transform.SetParent(transform,true);Map.Root.gameObject.SetActive(false);
   bool firstUnderground=entering&&!indoorAreas.ContainsKey(key);
   if(firstUnderground){
    var underground=new RaidMap{HardLoot=ActiveHardMode};underground.BuildUnderground2752();
    var vision=new RaidVision(underground.Bounds){RoomGate=false};
    var navigation=UndergroundNavigation2752(underground);
    indoorAreas.Add(key,new AreaState{Map=underground,Vision=vision,Navigation=navigation,Yaw=outsideArea.Yaw});
   }
   var target=entering?indoorAreas[key]:outsideArea;
   Map=target.Map;Vision=target.Vision;Navigation=target.Navigation;Map.Root.gameObject.SetActive(true);Enemies.AddRange(target.Actors);Player.transform.SetParent(Map.Root,true);
   if(firstUnderground){PopulateUndergroundEnemies2757();target.Actors=new List<RaidActor>(Enemies);}
   var entry=entering?Map.UndergroundEntries2752.Find(e=>e.Id==id):null;
   Player.transform.position=entering?entry.Arrival:portal.Position;
   if(!entering)outsideReturn=portal.Position;
   Rig.Target=Player.transform;Rig.ResetView(target.Yaw);View.orthographicSize=outsideArea.Zoom;Rig.Tick(0);
   ReloadTime=HealTime=ExtractTime=0;ResetWorldInteraction(true);Physics.SyncTransforms();Vision.Bind();Vision.Refresh(Player.transform.position,Player.Aim);
   NoticeTime=0;
   return true;
  }
 }
}
