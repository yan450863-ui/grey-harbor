using System;
using System.Collections.Generic;
using UnityEngine;
namespace GreyHarbor {
 public sealed class MapBuildStep2721 {
  public string Name;public Action Build;
  public MapBuildStep2721(string name,Action build){Name=name;Build=build;}
 }
 public sealed partial class RaidMap {
  public IEnumerable<MapBuildStep2721> HarborBuildSteps2721(int seed){
   yield return new MapBuildStep2721("准备港口道路与建筑",()=>{
    IsHarbor=HarborWhitebox=true;Bounds=new Rect(-79,-73,208,180);Spawn=new Vector3(-8,0,-56);Extraction=FreightExitPoint2716;
    Exits.Add(Extraction);Exits.Add(PortPoint(185,15));Root=new GameObject("Harbor_208x180_Whitebox").transform;
    Box("Sea",new Vector3(16,-.7f,17),new Vector3(240,.3f,230),"566D78");
    BuildHarborSurface();BuildHarborRoads();BuildHarborMainBuildings();
   });
   yield return new MapBuildStep2721("准备货运场",()=>BuildFreightExterior2716());
   yield return new MapBuildStep2721("准备集装箱区",()=>BuildCargoMaze());
   yield return new MapBuildStep2721("准备入口区域",()=>BuildHarborEntrance(seed));
   yield return new MapBuildStep2721("准备后勤院",()=>{BuildYardBoundary137();BuildLogisticsYard();DressExpandedYard138();});
   yield return new MapBuildStep2721("准备维修区域",()=>{BuildRepairExterior149();BuildRepairUpgrade200();});
   yield return new MapBuildStep2721("准备后勤物资",()=>BuildYardOutdoorContent140(seed));
   yield return new MapBuildStep2721("准备码头区域",()=>BuildDockDistrict141(seed));
   yield return new MapBuildStep2721("准备南侧码头",()=>BuildSouthDock142(seed));
   yield return new MapBuildStep2721("准备北侧码头",()=>BuildNorthMiddleDock143(seed));
   yield return new MapBuildStep2721("准备地面与撤离点",()=>{
    RegionAnchors.AddRange(new[]{Spawn,PortPoint(44,94),PortPoint(94,105),PortPoint(93,149),PortPoint(128,147),PortPoint(137,101),PortPoint(28,42),PortPoint(166,120),PortPoint(168,94),PortPoint(165,28)});
    Zone(Spawn,new Vector2(4,4),"C7AA58");foreach(var exit in Exits)Zone(exit,new Vector2(3.6f,3.6f),"4CAD88");
    BuildCargoGround2715();BuildFreightGround2716();FinalizeHarborGround137();Physics.SyncTransforms();
   });
   yield return new MapBuildStep2721("准备地下管网连接口",()=>AddUndergroundSurfaceEntries2752());
  }
 }
}
