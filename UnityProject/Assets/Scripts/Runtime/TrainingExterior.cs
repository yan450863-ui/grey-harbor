using UnityEngine;
using System.Collections.Generic;
namespace GreyHarbor {
 public sealed partial class RaidMap {
  public bool ExteriorPreview;
  public readonly List<Vector3> ClosedEntrances=new List<Vector3>();
  public readonly List<Rect> ExteriorBuildings=new List<Rect>();
  public static readonly string[] ExteriorAreaNames={"南侧入口","中央院落","西侧维修棚","北侧主屋","东侧集装箱区","东侧撤离区"};
  public readonly List<Vector3> ExteriorAreaPoints=new List<Vector3>();
  public void BuildTrainingExterior(){
   ExteriorPreview=true;Bounds=new Rect(-19,-17,38,34);Spawn=new Vector3(0,0,-13);Extraction=new Vector3(16,0,-12);Exits.Add(Extraction);Root=new GameObject("Training_PortServiceYard_Exterior").transform;
   var kit=new PortEnvironmentKit(this);
   new YardSurfaceArt(this).Build();
   kit.Wall(new Vector3(-18,0,0),32,false);kit.Wall(new Vector3(18,0,0),32,false);kit.Wall(new Vector3(0,0,16),36,true);kit.Wall(new Vector3(-10,0,-16),16,true);kit.Wall(new Vector3(10,0,-16),16,true);
   Box("SouthGateBoundary",new Vector3(0,.8f,-16),new Vector3(4,1.6f,.15f),"536760",true);for(float x=-1.7f;x<2;x+=.45f)Box("GateBars",new Vector3(x,.85f,-16.13f),new Vector3(.075f,1.7f,.07f),"AAA58B");
   kit.Building(new Vector3(-1,0,11),13,7,"939585","687875");ExteriorBuildings.Add(new Rect(-7.5f,7.5f,13,7));
   kit.Building(new Vector3(-13,0,1),6,10,"8C907D","7B7768");ExteriorBuildings.Add(new Rect(-16,-4,6,10));
   kit.Building(new Vector3(8,0,-13),3,3,"8D947F","5E756D");ExteriorBuildings.Add(new Rect(6.5f,-14.5f,3,3));
   kit.Shutter(new Vector3(0,0,7.4f),0,"A1");kit.Shutter(new Vector3(5.6f,0,11),-90,"A2");kit.Shutter(new Vector3(-9.9f,0,1),-90,"B");
   ClosedEntrances.Add(new Vector3(6,0,11));
   kit.Container(new Vector3(11,0,3.6f),3.2f,5.4f,"527B76");kit.Container(new Vector3(11,0,-4.1f),3.2f,5.4f,"687E82");
   kit.Pickup(new Vector3(.7f,0,.7f));kit.Bench(new Vector3(-11.9f,0,-5.1f));kit.Barrel(new Vector3(-14.7f,0,-5.2f),"687754");kit.Barrel(new Vector3(-15.5f,0,-5.1f),"805F48");
   kit.TimberStack(new Vector3(-3.4f,0,-5));kit.Pallets(new Vector3(4.6f,0,-4.4f),3);kit.TimberStack(new Vector3(-4,0,3.1f));kit.Pallets(new Vector3(8,0,7.6f),2);kit.Pallets(new Vector3(7.7f,0,3.5f),2);
   foreach(var p in new[]{new Vector3(-2.4f,0,-15),new Vector3(2.4f,0,-15),new Vector3(-17,0,8),new Vector3(16.8f,0,9),new Vector3(16.8f,0,-9)})kit.Lamp(p,p.x<0?0:180);
   foreach(var p in new[]{new Vector3(-3.5f,0,-14.8f),new Vector3(3.5f,0,-14.8f),new Vector3(14,0,-14.5f)}){SceneObstacle.Apply(Box("Bollard",p+Vector3.up*.38f,new Vector3(.18f,.76f,.18f),"B3A375",true),ObstacleKind.Low);Box("BollardBand",p+Vector3.up*.55f,new Vector3(.19f,.16f,.19f),"344B46");}
   Zone(Spawn,new Vector2(3,2.3f),"A1BAB0");Zone(Extraction,new Vector2(2.5f,3),"8ABCA2");
   for(int i=0;i<5;i++)Box("ThresholdStripe",new Vector3(-1.1f+i*.52f,.055f,6.9f),new Vector3(.27f,.02f,.42f),"B9A775");
   
   TrainingContainers();AddBoothEntrance();AddRepairEntrance();AddMainHouseEntrance();
   ExteriorAreaPoints.AddRange(new[]{Spawn,new Vector3(1,0,-3),new Vector3(-7,0,1),new Vector3(0,0,5),new Vector3(15,0,2),Extraction});RegionAnchors.AddRange(ExteriorAreaPoints);Physics.SyncTransforms();
  }
 }
 public sealed class TrainingExteriorSession:MonoBehaviour {
  void Start(){var g=RaidGame.Instance;g.NextMap=RaidDestination.Training;g.StartRaid();string flag=System.IO.Path.Combine(Application.dataPath,"../QA-ui-check.flag");if(System.IO.File.Exists(flag)&&System.IO.File.ReadAllText(flag).Trim()=="containers"){var box=g.Map.Loot.Find(l=>l.Type==SupplyType.Tools);g.Player.transform.position=box.Position;g.Player.Aim=Vector3.forward;g.Rig.Tick(0);g.Vision.Refresh(g.Player.transform.position,g.Player.Aim);}if(System.IO.File.Exists(flag)&&System.IO.File.ReadAllText(flag).Trim()=="interior"){g.Player.transform.position=RaidMap.BoothApproach;g.SwitchBooth();g.Player.transform.position=new Vector3(0,0,.3f);g.Rig.Tick(0);g.Vision.Refresh(g.Player.transform.position,g.Player.Aim);}if(System.IO.File.Exists(flag)&&System.IO.File.ReadAllText(flag).Trim()=="repair"){g.Player.transform.position=RaidMap.RepairApproach;g.SwitchInterior("repair");g.Player.transform.position=new Vector3(.1f,0,.15f);g.Rig.Tick(0);g.Vision.Refresh(g.Player.transform.position,g.Player.Aim);}if(System.IO.File.Exists(flag)&&System.IO.File.ReadAllText(flag).Trim()=="mainhouse"){g.Player.transform.position=RaidMap.MainHouseApproach;g.SwitchInterior("mainhouse");g.Player.transform.position=RaidMap.MainHouseDoorApproach(0);g.Rig.Tick(0);g.Vision.Refresh(g.Player.transform.position,g.Player.Aim);}if(System.IO.File.Exists(flag)&&System.IO.File.ReadAllText(flag).Trim()=="monsters"){g.Player.transform.position=new Vector3(15,0,-3);g.Player.Aim=Vector3.forward;foreach(var e in g.Enemies)if(e.Monster==MonsterKind.Spitter){e.WaitTime=15;e.AttackCooldown=10;}g.Rig.Tick(0);Physics.SyncTransforms();g.Vision.Refresh(g.Player.transform.position,g.Player.Aim);}Destroy(this);}
 }
}
