using UnityEngine;
using System.Collections.Generic;
namespace GreyHarbor {
 // Reusable exterior props in metre units. Visual meshes never supply accidental colliders.
 public sealed partial class GateParkingKit {
  readonly RaidMap map;Transform root;Vector3 origin;
  public GateParkingKit(RaidMap m){map=m;}
  void Begin(string name,Vector3 p){root=new GameObject(name).transform;root.SetParent(map.Root,false);root.position=p;origin=p;}
  GameObject B(string n,Vector3 p,Vector3 s,string color){var g=map.Box(n,origin+p,s,color);g.transform.SetParent(root,true);return g;}
  void Solid(Vector3 center,Vector3 size,ObstacleKind kind){var g=new GameObject("Collision_"+kind);g.transform.SetParent(root,false);g.transform.localPosition=center;g.AddComponent<BoxCollider>().size=size;SceneObstacle.Apply(g,kind);}
  void Wheel(float x,float z,float r){var g=GameObject.CreatePrimitive(PrimitiveType.Cylinder);g.name="RubberWheel";g.transform.SetParent(root,false);g.transform.localPosition=new Vector3(x,r,z);g.transform.localRotation=Quaternion.Euler(0,0,90);g.transform.localScale=new Vector3(r*2,.13f,r*2);g.GetComponent<Renderer>().sharedMaterial=map.Mat("26332F",true);if(Application.isPlaying)Object.Destroy(g.GetComponent<Collider>());else Object.DestroyImmediate(g.GetComponent<Collider>());B("WheelHub",new Vector3(x+Mathf.Sign(x)*.15f,r,z),new Vector3(.035f,r,r),"768278");}
  public void Vehicle(Vector3 p,int type,float yaw=0){
   string[] names={"Sedan","ServiceVan","ShuttleBus"};Begin("GateKit_"+names[type],p);
   float w=type==2?2.5f:1.9f,l=type==2?10:type==1?5:4.5f,h=type==2?3: type==1?2.1f:1.45f;
   string body=type==0?"7B8A7D":type==1?"A49F88":"B3AC87";
   B("Chassis",new Vector3(0,.45f,0),new Vector3(w,.35f,l),"384941");
   B("Body",new Vector3(0,.86f,0),new Vector3(w,.65f,l-.12f),body);
   if(type==0){B("Bonnet",new Vector3(0,1.07f,l*.30f),new Vector3(w-.1f,.22f,1.35f),"929E8D");B("Cabin",new Vector3(0,1.3f,-.18f),new Vector3(w-.17f,.5f,2.3f),"3B5356");B("Roof",new Vector3(0,1.59f,-.27f),new Vector3(w-.16f,.12f,1.6f),body);}
   else {B("UpperCabin",new Vector3(0,(h+1.08f)/2,0),new Vector3(w-.08f,h-1.08f,l-.22f),body);B("Roof",new Vector3(0,h+.06f,0),new Vector3(w+.06f,.12f,l-.12f),"919C8C");}
   int count=type==2?7:type==1?2:2;float start=type==2?-l/2+.8f:-1f,step=type==2?1.14f:1.05f;
   for(int side=-1;side<=1;side+=2){for(int i=0;i<count;i++)B("SideWindow",new Vector3(side*(w/2+.013f),type==2?2.12f:type==1?1.55f:1.32f,start+i*step),new Vector3(.035f,type==2?.8f:.40f,type==2?.91f:.78f),"3B5355");
    B("SideTrim",new Vector3(side*(w/2+.025f),.92f,0),new Vector3(.045f,.11f,l-.2f),"4D625B");
    foreach(float z in new[]{-l*.31f,l*.31f})Wheel(side*(w/2-.02f),z,type==2?.49f:.34f);
    B("Mirror",new Vector3(side*(w/2+.16f),type==2?2.1f:1.4f,l*.36f),new Vector3(.22f,.2f,.32f),"3C4B44");}
   if(type!=0)B("Windshield",new Vector3(0,type==2?2.13f:1.56f,l/2-.08f),new Vector3(w-.3f,type==2?1:.6f,.04f),"476166");
   foreach(float x in new[]{-w*.33f,w*.33f}){B("Headlight",new Vector3(x,.9f,l/2+.005f),new Vector3(.32f,.20f,.045f),"C8C6A8");B("TailLight",new Vector3(x,.82f,-l/2-.005f),new Vector3(.2f,.17f,.035f),"8E5D45");}
   B("FrontGrille",new Vector3(0,.7f,l/2+.012f),new Vector3(w*.43f,.18f,.035f),"34463F");
   B("Bumper",new Vector3(0,.5f,l/2+.04f),new Vector3(w+.07f,.15f,.13f),"707F71");
   if(type==2){B("BusDoor",new Vector3(w/2+.033f,1.52f,l*.32f),new Vector3(.045f,1.86f,.78f),"354B4B");B("RoofVent",new Vector3(0,h+.17f,-1),new Vector3(1.1f,.14f,1.4f),"66786C");}
   Solid(new Vector3(0,h/2,0),new Vector3(w+.15f,h,l+.16f),ObstacleKind.High);root.rotation=Quaternion.Euler(0,yaw,0);
  }
  // Faceted canopy lives above actor height; only the trunk blocks movement and shots.
  public void Tree(Vector3 p,int size){
   Begin("GateKit_Tree_"+new[]{"Small","Medium","Large"}[size],p);float height=3.4f+size*1.7f,radius=1.05f+size*.65f,trunk=.20f+size*.14f;
   B("Trunk",new Vector3(0,height*.34f,0),new Vector3(trunk,height*.68f,trunk),"655E48");
   Solid(new Vector3(0,height*.34f,0),new Vector3(trunk,height*.68f,trunk),ObstacleKind.High);
   Crown(new Vector3(0,height*.73f,0),radius,height*.30f,size);
   Crown(new Vector3(-radius*.45f,height*.66f,radius*.20f),radius*.62f,height*.20f,size+1);
  }
  void Crown(Vector3 center,float radius,float vertical,int variant){
   var vertices=new List<Vector3>();var triangles=new List<int>();var colors=new List<Color>();int slices=7;float[] ring={0,.72f,1,.68f,0};
   for(int layer=0;layer<4;layer++)for(int i=0;i<slices;i++){
    float a=i*2*Mathf.PI/slices,b=(i+1)*2*Mathf.PI/slices;
    Vector3 p0=center+new Vector3(Mathf.Cos(a)*radius*ring[layer],(layer/4f*2-1)*vertical,Mathf.Sin(a)*radius*ring[layer]);
    Vector3 p1=center+new Vector3(Mathf.Cos(b)*radius*ring[layer],(layer/4f*2-1)*vertical,Mathf.Sin(b)*radius*ring[layer]);
    Vector3 p2=center+new Vector3(Mathf.Cos(a)*radius*ring[layer+1],((layer+1)/4f*2-1)*vertical,Mathf.Sin(a)*radius*ring[layer+1]);
    Vector3 p3=center+new Vector3(Mathf.Cos(b)*radius*ring[layer+1],((layer+1)/4f*2-1)*vertical,Mathf.Sin(b)*radius*ring[layer+1]);
    foreach(var p in new[]{p0,p2,p1,p1,p2,p3}){triangles.Add(vertices.Count);vertices.Add(p);colors.Add(Color.white);}
   }
   var g=new GameObject("FacetedCanopy");g.transform.SetParent(root,false);var mesh=new Mesh{name="TreeCanopy_"+variant};mesh.SetVertices(vertices);mesh.SetTriangles(triangles,0);mesh.RecalculateNormals();g.AddComponent<MeshFilter>().sharedMesh=mesh;g.AddComponent<MeshRenderer>().sharedMaterial=map.Mat(variant%2==0?"657653":"74805A",true);var owned=root.GetComponent<WorkshopArtResources>()??root.gameObject.AddComponent<WorkshopArtResources>();owned.Meshes.Add(mesh);
  }
  public void Grass(Vector3 p,bool lawn){
   Begin(lawn?"GateKit_LawnPatch":"GateKit_LowGrass",p);float spread=lawn?1.8f:.38f;
   if(lawn)B("SoilGrassBase",new Vector3(0,.073f,0),new Vector3(3.8f,.008f,3.4f),"667452");
   var rng=new System.Random(lawn?129:31);for(int i=0;i<(lawn?55:7);i++){float x=((float)rng.NextDouble()*2-1)*spread,z=((float)rng.NextDouble()*2-1)*spread*.85f,h=.10f+(float)rng.NextDouble()*.18f;var g=B("GrassBlade",new Vector3(x,.08f+h/2,z),new Vector3(.035f,h,.045f),i%3==0?"94916A":"78815B");g.transform.localRotation=Quaternion.Euler(i%3*12,0,i%5*8-16);}
  }
  public void WheelStop(Vector3 p){Begin("GateKit_WheelStop",p);B("ConcreteStop",new Vector3(0,.13f,0),new Vector3(1.5f,.26f,.22f),"9B9D82");Solid(new Vector3(0,.13f,0),new Vector3(1.5f,.26f,.22f),ObstacleKind.Low);}
  public void Barrier(Vector3 p){Begin("GateKit_RaisedBarrier",p);B("Housing",new Vector3(0,.57f,0),new Vector3(.5f,1.14f,.5f),"AD9A65");B("RaisedArm",new Vector3(0,2.8f,0),new Vector3(.13f,3.2f,.16f),"C2C3A6");for(int i=0;i<5;i++)B("ArmStripe",new Vector3(0,1.5f+i*.6f,-.09f),new Vector3(.14f,.22f,.025f),"966F53");Solid(new Vector3(0,.57f,0),new Vector3(.5f,1.14f,.5f),ObstacleKind.Low);}
 }
 public sealed partial class RaidMap {
  void BuildEntranceLandscape(){
   var k=new GateParkingKit(this);var surface=new GroundSurfaceArt(this,129);surface.Region(new Rect(11,-53.7f,24,11.2f),GroundFinish.Asphalt,.033f);surface.Bake("Parking_Asphalt");
   k.Vehicle(new Vector3(14.5f,0,-49.3f),0);k.Vehicle(new Vector3(20.5f,0,-49.3f),1);k.Vehicle(new Vector3(31.5f,0,-48.5f),2);
   foreach(float x in new[]{14.5f,17.5f,20.5f})k.WheelStop(new Vector3(x,0,-47.1f));
   foreach(float x in new[]{29.8f,33.2f})Box("BusBayLine",new Vector3(x,.078f,-48.5f),new Vector3(.10f,.01f,10.8f),"B0B29A");
   k.Barrier(new Vector3(-7.8f,0,-59));
   k.Tree(new Vector3(-22,0,-59),0);k.Tree(new Vector3(-21.8f,0,-40),1);k.Tree(new Vector3(10,0,-33),2);k.Tree(new Vector3(37,0,-30),1);
   k.Grass(new Vector3(-21.8f,0,-48),true);k.Grass(new Vector3(10,0,-34),true);k.Grass(new Vector3(37,0,-30),true);
   for(int i=0;i<8;i++)k.Grass(new Vector3(36.5f,0,-51+i*2.2f),false);
  }
 }
}
