namespace GreyHarbor {
 public sealed partial class RaidGame {
  void PopulateDockMonsters141(){
   YardOutdoorMonster141("北部服务平台",MonsterKind.Wanderer,164,127,164,124.5f,166,127.5f);
   YardOutdoorMonster141("中部维修平台",MonsterKind.Runner,166,78,166,79.8f,168,78.4f);
   YardOutdoorMonster141("南部货棚外",MonsterKind.Shell,164,36,162.5f,35,164.2f,38);
   YardOutdoorMonster141("南部装卸场",MonsterKind.Spitter,174,23.5f,174,25.5f,176,24.5f);
  }
  void YardOutdoorMonster141(string title,MonsterKind kind,float x,float z,float ax,float az,float bx,float bz){var p=RaidMap.PortPoint(x,z);var a=RaidMap.PortPoint(ax,az);var b=RaidMap.PortPoint(bx,bz);InteriorMonster("码头 / "+title,kind,p.x,p.z,a.x,a.z,b.x,b.z);}
 }
}
