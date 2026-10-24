using UnityEngine;
using UnityEngine.Rendering;
namespace GreyHarbor {
 // Reusable exterior masonry boundary. Collision belongs to the continuous wall, never to its surface marks.
 public sealed class YardBoundaryKit137 {
  readonly RaidMap map;public YardBoundaryKit137(RaidMap m){map=m;}
  GameObject Finish(string name,Vector3 p,float yaw,WorkshopWastelandArt art){
   var g=new GameObject(name);g.transform.SetParent(map.Root,false);g.transform.position=p;g.transform.rotation=Quaternion.Euler(0,yaw,0);
   art.Bake(g,g.AddComponent<WorkshopArtResources>());
   foreach(var r in g.GetComponentsInChildren<Renderer>()) {r.shadowCastingMode=ShadowCastingMode.On;r.sharedMaterial.SetFloat("_Detail",.015f);}
   return g;
  }
  void PillarArt(WorkshopWastelandArt a,float x){
   a.Cube("BrickPillar","7C7D69",new Vector3(x,1.10f,0),new Vector3(.64f,2.20f,.64f));
   a.Cube("PillarCap","A29E85",new Vector3(x,2.23f,0),new Vector3(.80f,.16f,.80f));
   for(int y=0;y<9;y++)a.Panel("MasonryJoints","666C5B",new Vector3(x,.16f+y*.23f,.326f),Vector3.right,Vector3.up,.62f,.025f);
  }
  public GameObject Wall(Vector3 p,float length,float yaw=0){
   var a=new WorkshopWastelandArt(map);
   a.Cube("WeatheredYardWall","90927A",new Vector3(0,.99f,0),new Vector3(length,1.98f,.42f));
   a.Cube("DarkStonePlinth","636A58",new Vector3(0,.18f,0),new Vector3(length,.36f,.48f));
   a.Cube("SlopedWallCap","AAA58B",new Vector3(0,2.01f,0),new Vector3(length,.12f,.54f));
   int count=Mathf.CeilToInt(length/3.8f);
   for(int i=0;i<=count;i++)PillarArt(a,-length/2+length*i/count);
   foreach(int side in new[]{-1,1}){
    Vector3 inward=Vector3.forward*side;
    for(float x=-length/2+.9f;x<length/2-.2f;x+=2.9f){
     a.Patch("WeatheredPlaster","A6A187",new Vector3(x,.8f+(Mathf.Abs(x)%1),side*.219f),Vector3.right,Vector3.up,.72f,.48f,true);
     a.Patch("WallDamp","747D64",new Vector3(x+.44f,.30f,side*.224f),Vector3.right,Vector3.up,.55f,.30f,true);
     if(Mathf.FloorToInt(x+length)%3==0)a.BrickScar(new Vector3(x,1.05f,side*.23f),Vector3.right,inward);
    }
   }
   var g=Finish("YardBoundary_Wall",p,yaw,a);var info=g.AddComponent<YardAssetInfo>();info.Asset="YardBoundaryWall";
   var c=g.AddComponent<BoxCollider>();c.center=new Vector3(0,1.13f,0);c.size=new Vector3(length,2.26f,.64f);SceneObstacle.Apply(g,ObstacleKind.High);
   return g;
  }
  public GameObject OpenGate(Vector3 p,float width=6){
   var a=new WorkshopWastelandArt(map);
   foreach(int side in new[]{-1,1}){
    float x=side*(width/2+.34f);PillarArt(a,x);
    float leaf=side*(width/2+1.75f);
    a.Cube("OpenSteelGate","4D665B",new Vector3(leaf,1.0f,.47f),new Vector3(2.7f,1.72f,.13f));
    a.Cube("GateTopRail","30483F",new Vector3(leaf,1.89f,.47f),new Vector3(2.79f,.09f,.19f));
    a.Cube("GateBottomRail","30483F",new Vector3(leaf,.15f,.47f),new Vector3(2.79f,.09f,.19f));
    for(int i=0;i<8;i++)a.Cube("GateRaisedRibs","71816C",new Vector3(leaf-1.16f+i*.33f,1,.556f),new Vector3(.035f,1.62f,.045f));
    a.Panel("GateRust","8C7252",new Vector3(leaf+.33f,.47f,.567f),Vector3.right,Vector3.up,.57f,.19f);
    a.Cube("GateHinge","536451",new Vector3(x,.43f,.37f),new Vector3(.13f,.18f,.23f));
    a.Cube("GateHinge","536451",new Vector3(x,1.63f,.37f),new Vector3(.13f,.18f,.23f));
   }
   var g=Finish("YardBoundary_OpenNorthGate",p,0,a);g.AddComponent<YardAssetInfo>().Asset="YardOpenGate";
   foreach(int side in new[]{-1,1}){
    var post=new GameObject("GateCollision");post.transform.SetParent(g.transform,false);var c=post.AddComponent<BoxCollider>();c.center=new Vector3(side*(width/2+.34f),1.13f,0);c.size=new Vector3(.68f,2.26f,.68f);SceneObstacle.Apply(post,ObstacleKind.High);
    var leaf=new GameObject("FoldedGateCollision");leaf.transform.SetParent(g.transform,false);c=leaf.AddComponent<BoxCollider>();c.center=new Vector3(side*(width/2+1.75f),1,.47f);c.size=new Vector3(2.7f,1.9f,.22f);SceneObstacle.Apply(leaf,ObstacleKind.High);
   }
   return g;
  }
 }
 public sealed partial class RaidMap {
  void BuildYardBoundary137(){
   var kit=new YardBoundaryKit137(this);
   kit.Wall(new Vector3(-71,0,-45),40,90).name="YardWest";
   kit.Wall(new Vector3(-25,0,-45),40,90).name="YardEast";
   kit.Wall(new Vector3(-48,0,-65),46).name="YardSouth";
   kit.Wall(new Vector3(-62.5f,0,-25),17).name="YardNorthWest";
   kit.Wall(new Vector3(-36.5f,0,-25),23).name="YardNorthEast";
   kit.OpenGate(YardGate);
   var ground=new GroundSurfaceArt(this,137137);
   ground.Region(new Rect(-71,-25,47,11),GroundFinish.Soil,.036f);
   ground.Region(new Rect(-71,-22,47,6),GroundFinish.Asphalt,.073f);
   ground.Region(new Rect(-54,-29,6,7),GroundFinish.PatchedConcrete,.083f);
   ground.Region(new Rect(-24.8f,-64.8f,2.5f,39.6f),GroundFinish.Concrete,.086f);
   ground.Bake("Yard_EntranceSeam");
   var k=new YardLivingKit(this);
   k.Grass(PortPoint(50.5f,39),4,11,false,137);
   k.Grass(PortPoint(50,10.2f),6,2.3f,true,138);
   k.Grass(PortPoint(51.5f,26),2.3f,8,true,139);
   k.Tree(PortPoint(50.5f,44),0,137);
   k.Tree(PortPoint(51.5f,12.5f),1,138);
  }
 }
}
