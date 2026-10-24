using System;
using System.Collections.Generic;
using UnityEngine;
namespace GreyHarbor {
 public enum RaidDestination { Harbor,Training,TransitCenter }
 public enum HarborDistrict { Entrance,Repair,Containers,Loading,Office,Coast }
 public enum SupplyType { Misc,Tools,Medical,Ammo,Military,Safe }
 public enum MonsterKind { Wanderer,Runner,Shell,Spitter,PaperbackCreeper,BallastHauler,Pipewhisker }
 public static class HarborContent {
  public static readonly string[] SupplyNames={"杂物箱","工具箱","医疗箱","弹药箱","军用物资箱","保险箱"};
  public static readonly string[] DistrictNames={"南侧入口区","西侧维修区","中部集装箱区","办公楼","装卸区","外围堤岸"};
  public static readonly string[] MonsterNames={"游荡者","疾行兽","重壳兽","腐液喷吐者","纸背伏行体","负索驮背体","管须裂节体"};
  // Every entry is positive: location changes probability, never eligibility.
  public static readonly int[,] SupplyWeights={{40,12,20,18,9,1},{18,45,10,15,10,2},{24,19,12,20,23,2},{12,20,12,22,30,4},{22,12,22,10,28,6},{35,18,14,18,14,1}};
  public static readonly int[,] MonsterWeights={{70,22,2,6},{48,27,13,12},{24,48,15,13},{25,27,34,14},{34,30,20,16},{48,30,10,12}};
  public static int Pick(System.Random rng,int[,] weights,int district){int sum=0;for(int i=0;i<weights.GetLength(1);i++)sum+=weights[district,i];int roll=rng.Next(sum);for(int i=0;i<weights.GetLength(1);i++){roll-=weights[district,i];if(roll<0)return i;}return 0;}
  public static float SearchSeconds(SupplyType t)=>t==SupplyType.Safe?6:t==SupplyType.Military?3.5f:t==SupplyType.Tools?2.2f:1.5f;
  public static List<ItemStack> Contents(SupplyType type,System.Random r,bool hard=false,ContainerTier tier=ContainerTier.Auto,CollectionCategory? category=null){
   var list=new List<ItemStack>();int count=LootBalance.Count(r);
   for(int i=0;i<count;i++){ItemKind k=ItemKind.Scrap;
    k=LootBalance.Pick(type,r,hard,tier,category);
    list.Add(new ItemStack(k,AmmoRules270.IsAmmo(k)?r.Next(8,19):1,ItemOrigin.Certified){Identified=false,RaidFound=true});
   }ContainerBonus147.Add(list,type,tier,r,category);return list;
  }
  public static List<ItemStack> Drops(MonsterKind kind)=>EnemyLoot146.Monster(kind,new System.Random(UnityEngine.Random.Range(0,int.MaxValue)));
 }
 public sealed class HarborSpawn {public Vector3 Position;public HarborDistrict District;public HarborSpawn(float x,float z,HarborDistrict d){Position=new Vector3(x,0,z);District=d;}}
 public sealed partial class RaidMap {
  public Rect Bounds=new Rect(-19,-17,38,34);
  public bool IsHarbor,HardLoot;
  public readonly List<Vector3> Exits=new List<Vector3>();
  public readonly List<HarborSpawn> MonsterSites=new List<HarborSpawn>(),SupplySites=new List<HarborSpawn>();
  public readonly List<Vector3> RegionAnchors=new List<Vector3>();
  public void BuildHarborLegacy(int seed){
   IsHarbor=true;Bounds=new Rect(-54,-48,108,100);Spawn=new Vector3(-42,0,-39);Extraction=new Vector3(-42,0,-44);
   Exits.Add(Extraction);Exits.Add(new Vector3(47,0,45));Root=new GameObject("GreyHarborCargoPort").transform;
   var rng=new System.Random(seed);
   Box("Water",new Vector3(0,-.75f,0),new Vector3(170,.35f,156),"294B55");
   Box("PortApron",new Vector3(-5,-.2f,0),new Vector3(94,.4f,92),"596563");
   Box("Pier",new Vector3(46,-.2f,39),new Vector3(12,.4f,18),"65716B");
   // Physical seawalls keep navigation and player movement off the water.
   Wall(-52,0,.5f,92);Wall(-5,-46,94,.5f);Wall(-5,46,94,.5f);
   Wall(42,-8,.5f,76);Wall(52,39,.5f,18);Wall(47,48,10,.5f);Wall(47,30,10,.5f);
   for(int z=-42;z<46;z+=4)for(int x=-48;x<42;x+=4){Box("PavingJoint",new Vector3(x,.02f,z),new Vector3(3.94f,.012f,.022f),"4A5758");}
   foreach(float x in new[]{-31f,1f,36f}){Box("ServiceRoad",new Vector3(x,.025f,0),new Vector3(5,.018f,86),"414F52");for(int z=-41;z<45;z+=6)Box("LaneMark",new Vector3(x,.04f,z),new Vector3(.12f,.015f,2),"A9A581");}
   foreach(float z in new[]{-27f,-7f,13f,25f})Box("CrossLane",new Vector3(-3,.035f,z),new Vector3(81,.02f,4),"485658");
   Building(-43,-30,12,8,"GATEHOUSE");Building(-42,-12,12,16,"REPAIR A");Building(-42,14,12,16,"REPAIR B");
   Building(-18,36,22,16,"WAREHOUSE 01");Building(10,36,22,16,"WAREHOUSE 02");Building(32,35,12,14,"PORT OFFICE");
   foreach(float x in new[]{-21f,-11f,10f,21f})foreach(float z in new[]{-18f,3f,19f}){
    Container(x,z,5.5f,10,rng.Next(3)==0?"826D55":rng.Next(2)==0?"577573":"687B80");
    if(z<15)Container(x,z+12,5.5f,3,"807760");
   }
   // Forklift, repair benches, crane feet and cargo provide cover without closing the three routes.
   foreach(var p in new[]{new Vector3(-35,0,0),new Vector3(-25,0,19),new Vector3(28,0,-16),new Vector3(35,0,5),new Vector3(29,0,20)}){Crate(p.x,p.z);Crate(p.x+1.3f,p.z);}
   for(int i=0;i<3;i++){float z=-23+i*22;Box("DockBollard",new Vector3(40,.4f,z),new Vector3(.6f,.8f,.6f),"8C8265",true);Box("DockStripe",new Vector3(39,.06f,z),new Vector3(.3f,.03f,4),"C0A46A");}
   Box("CraneFoot",new Vector3(25,1.5f,44),new Vector3(1.4f,3,1.4f),"94734F",true);Box("CraneBeam",new Vector3(34,5.5f,44),new Vector3(20,.55f,.65f),"A68452");Box("CraneMast",new Vector3(25,3.4f,44),new Vector3(.8f,4,.8f),"A68452");
   for(int i=0;i<3;i++)Box("ParkedTruck",new Vector3(-27+i*7,1,-38),new Vector3(3,2,6),i%2==0?"69756A":"8A8269",true);
   RegionAnchors.AddRange(new[]{new Vector3(-42,0,-39),new Vector3(-37,0,0),new Vector3(0,0,0),new Vector3(-5,0,26),new Vector3(32,0,35),new Vector3(36,0,-20)});
   
   Zone(Spawn,new Vector2(3,3),"759D97");for(int i=0;i<Exits.Count;i++){Zone(Exits[i],new Vector2(3.6f,3.6f),"71B99D");}
   // Sites are hand placed and walkable; their type and contents are rerolled each raid.
   Sites(HarborDistrict.Entrance,new float[]{-47,-31,-44,-28,-38,-31,-35,-35,-48,-38,-16,-33,0,-38});
   Sites(HarborDistrict.Repair,new float[]{-46,-17,-38,-17,-46,-8,-38,-8,-46,9,-38,9,-46,19,-38,19,-35,1,-48,1});
   Sites(HarborDistrict.Containers,new float[]{-25,-22,-7,-22,6,-22,25,-22,-25,-3,-7,-3,6,-3,25,-3,-25,13,-7,13,6,13,25,13,-4,20,16,25});
   Sites(HarborDistrict.Loading,new float[]{-26,31,-10,31,-26,40,-10,40,2,31,18,31,2,40,18,40,-31,38,-3,44});
   Sites(HarborDistrict.Office,new float[]{29,31,35,31,29,39,35,39,26,27,38,44});
   Sites(HarborDistrict.Coast,new float[]{38,-32,38,-18,38,-3,38,14,39,26,46,35,48,41});
   var safeSites=SelectSafeSites(rng);foreach(var site in SupplySites){if(safeSites.Contains(site)){AddSupply(site,SupplyType.Safe,rng);continue;}if(rng.NextDouble()>.72)continue;var type=(SupplyType)HarborContent.Pick(rng,HarborContent.SupplyWeights,(int)site.District);if(type==SupplyType.Safe)type=SupplyType.Military;AddSupply(site,type,rng);}
   MonsterPoints();Physics.SyncTransforms();
  }
  void Building(float x,float z,float w,float d,string title){Box("RoomFloor",new Vector3(x,.04f,z),new Vector3(w,.02f,d),"7C8174");Wall(x-w/2,z,.4f,d);Wall(x+w/2,z,.4f,d);foreach(float edge in new[]{z-d/2,z+d/2}){Wall(x-w/4-1,edge,w/2-2,.4f);Wall(x+w/4+1,edge,w/2-2,.4f);}}
  void Sites(HarborDistrict d,float[] pairs){for(int i=0;i<pairs.Length;i+=2)SupplySites.Add(new HarborSpawn(pairs[i],pairs[i+1],d));}
  void MonsterPoints(){
   AddMonsterSites(HarborDistrict.Entrance,new float[]{-29,-25,-9,-32,15,-33});
   AddMonsterSites(HarborDistrict.Repair,new float[]{-35,-18,-33,-3,-37,7,-33,22,-46,15});
   AddMonsterSites(HarborDistrict.Containers,new float[]{-26,-16,-5,-16,4,-8,26,-12,-26,4,-5,5,4,14,26,11,-4,22});
   AddMonsterSites(HarborDistrict.Loading,new float[]{-23,26,-15,32,-23,41,-2,27,7,32,16,41});
   AddMonsterSites(HarborDistrict.Office,new float[]{29,26,32,33,40,41});
   AddMonsterSites(HarborDistrict.Coast,new float[]{36,-25,36,-10,36,14,39,28});
  }
  void AddMonsterSites(HarborDistrict d,float[] pairs){for(int i=0;i<pairs.Length;i+=2)MonsterSites.Add(new HarborSpawn(pairs[i],pairs[i+1],d));}
  public System.Collections.Generic.List<HarborSpawn> SelectSafeSites(System.Random rng){var candidates=new System.Collections.Generic.List<HarborSpawn>(SupplySites);var chosen=new System.Collections.Generic.List<HarborSpawn>();for(int i=0;i<2;i++){int sum=0;foreach(var s in candidates)sum+=HarborContent.SupplyWeights[(int)s.District,5];int roll=rng.Next(sum);foreach(var s in candidates){roll-=HarborContent.SupplyWeights[(int)s.District,5];if(roll<0){chosen.Add(s);candidates.Remove(s);break;}}}return chosen;}
  void AddSupply(HarborSpawn site,SupplyType type,System.Random rng){
   ContainerLook look=type==SupplyType.Tools?ContainerLook.Toolbox:type==SupplyType.Medical?ContainerLook.MedicalCabinet:type==SupplyType.Ammo?ContainerLook.AmmoCase:type==SupplyType.Military?ContainerLook.MilitaryCase:type==SupplyType.Safe?ContainerLook.Safe:site.District==HarborDistrict.Office?(rng.Next(2)==0?ContainerLook.FileCabinet:ContainerLook.Suitcase):ContainerLook.SupplyLocker;
   AddContainer(site.Position,site.Position+Vector3.forward*1.1f,look,type,rng,site.District);
  }
 }
}

