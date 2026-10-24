using UnityEngine;
namespace GreyHarbor {
 public sealed partial class RaidMap {
  public static readonly Vector3[] EntranceLuggagePositions={new Vector3(-1,0,-34),new Vector3(2,0,-34)};
  public static readonly Vector3 MaintenanceTable=new Vector3(12,0,-36.8f);
  void BuildEntranceContent(int seed){
   var rng=new System.Random(seed^134);
   foreach(var p in EntranceLuggagePositions)AddContainer(p+Vector3.back*1.1f,p,ContainerLook.RollingSuitcase,SupplyType.Misc,rng,HarborDistrict.Entrance,0,false,ContainerTier.Common);
   PortDesk(MaintenanceTable,false);
   Box("OutdoorWorkbenchVise",MaintenanceTable+new Vector3(-.95f,1.02f,.12f),new Vector3(.32f,.22f,.25f),"41574D");
   AddContainer(MaintenanceTable+Vector3.back*1.2f,MaintenanceTable+Vector3.up*.925f,ContainerLook.Toolbox,SupplyType.Tools,rng,HarborDistrict.Entrance,0,false,ContainerTier.Common);
  }
 }
 public sealed partial class RaidGame {
  void PopulateEntranceMonsters(){
   InteriorMonster("门岗西侧围栏",MonsterKind.Wanderer,-19,-47,-19,-51,-19,-42);
   InteriorMonster("停车场",MonsterKind.Wanderer,24.5f,-50,24.5f,-53,24.5f,-46);
   InteriorMonster("附属房西侧路口",MonsterKind.Runner,13,-43.5f,13,-40,10.5f,-43.5f);
   InteriorMonster("附属房东侧绿化带",MonsterKind.Spitter,35,-34,34,-32,35,-38);
  }
 }
}
