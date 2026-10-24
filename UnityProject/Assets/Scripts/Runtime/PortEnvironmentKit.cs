using UnityEngine;
namespace GreyHarbor {
 // Shared parametric environment pieces: no loot, AI or gameplay spawners.
 public sealed class PortEnvironmentKit {
  readonly RaidMap map; Transform part; Vector3 origin;
  public PortEnvironmentKit(RaidMap map){this.map=map;}
  void Begin(string name,Vector3 p){var g=new GameObject(name);g.transform.SetParent(map.Root,false);g.transform.position=p;part=g.transform;origin=p;}
  GameObject B(string name,float x,float y,float z,float w,float h,float d,string color,bool solid=false){var g=map.Box(name,origin+new Vector3(x,y,z),new Vector3(w,h,d),color,solid);g.transform.SetParent(part,true);g.GetComponent<Renderer>().sharedMaterial.SetFloat("_Detail",.09f);return g;}
  GameObject Cylinder(string name,float x,float y,float z,float diameter,float height,string color,bool solid=false){var g=GameObject.CreatePrimitive(PrimitiveType.Cylinder);g.name=name;g.transform.SetParent(part,false);g.transform.position=origin+new Vector3(x,y,z);g.transform.localScale=new Vector3(diameter,height/2,diameter);g.GetComponent<Renderer>().sharedMaterial=map.Mat(color,true);if(solid)SceneObstacle.Apply(g,ObstacleKind.High);else {if(Application.isPlaying)Object.Destroy(g.GetComponent<Collider>());else Object.DestroyImmediate(g.GetComponent<Collider>());}return g;}
  public Transform Building(Vector3 p,float w,float d,string wall,string roof){
   Begin("PortKit_ClosedBuilding",p);B("SolidBuildingShell",0,1.35f,0,w,2.7f,d,wall,true);B("Foundation",0,.14f,0,w+.18f,.28f,d+.18f,"555C58");
   B("FrontSill",0,.43f,-d/2-.035f,w,.14f,.08f,"85877B");B("RearSill",0,.43f,d/2+.035f,w,.14f,.08f,"85877B");
   float slope=12,half=d/2+.35f,rise=Mathf.Sin(slope*Mathf.Deg2Rad)*half;
   for(int side=-1;side<=1;side+=2){var sheet=B("CorrugatedRoof",0,2.83f+rise/2,side*half/2,w+.6f,.12f,half/Mathf.Cos(slope*Mathf.Deg2Rad),roof);sheet.transform.rotation=Quaternion.Euler(side*slope,0,0);
    for(float x=-w/2;x<=w/2;x+=.48f){var rib=B("RoofSeam",x,2.94f+rise/2,side*half/2,.038f,.045f,half/Mathf.Cos(slope*Mathf.Deg2Rad),"96968A");rib.transform.rotation=sheet.transform.rotation;}}
   B("RidgeCap",0,3.02f+rise,0,w+.72f,.12f,.15f,"8C8C7D");
   foreach(float x in new[]{-w/2+.18f,w/2-.18f}){B("Downpipe",x,1.4f,-d/2-.11f,.09f,2.8f,.09f,"454E4C");B("CornerPost",x,1.4f,d/2+.035f,.13f,2.8f,.07f,"7A7D70");}
   for(float x=-w/2+1;x<w/2-.4f;x+=2.4f){B("WindowFrame",x,1.9f,-d/2-.055f,1.3f,.72f,.1f,"343F40");B("DustyGlass",x,1.9f,-d/2-.12f,1.1f,.53f,.025f,"657F80");B("WindowBar",x,1.9f,-d/2-.14f,.035f,.59f,.035f,"929688");}
   return part;
  }
  public void Shutter(Vector3 p,float yaw,string code){Begin("PortKit_LockedDoor_"+code,p);B("DoorFrame",0,1.2f,0,2.4f,2.4f,.14f,"343C3A");B("ClosedShutter",0,1.12f,-.1f,2.15f,2.2f,.12f,"68736D",true);for(int i=0;i<12;i++)B("ShutterSlat",0,.18f+i*.17f,-.18f,2.1f,.022f,.025f,"92988A");B("LockBar",0,.62f,-.22f,2.18f,.095f,.085f,"BAA26B");B("Handle",.72f,1,-.25f,.12f,.28f,.1f,"323B38");B("Lamp",0,2.53f,-.28f,.45f,.16f,.32f,"DDC28B");part.rotation=Quaternion.Euler(0,yaw,0);}
  public void Container(Vector3 p,float w,float d,string color){Begin("PortKit_SealedContainer",p);B("ContainerBody",0,1.18f,0,w,2.36f,d,color,true);foreach(float x in new[]{-w/2,w/2}){B("CornerRail",x,1.2f,0,.11f,2.5f,d+.12f,"90A09A");}for(float z=-d/2+.25f;z<d/2;z+=.35f){B("RoofRib",0,2.4f,z,w,.055f,.055f,"8EAAA1");foreach(float x in new[]{-w/2-.02f,w/2+.02f})B("SideRib",x,1.2f,z,.055f,2.22f,.055f,"69877F");}B("FrontDoors",0,1.2f,-d/2-.025f,w-.15f,2.15f,.06f,color);for(int i=-1;i<=1;i+=2)B("LockingRod",i*w*.24f,1.18f,-d/2-.085f,.065f,1.95f,.06f,"BBC0A9");B("LowerRail",0,.13f,-d/2-.12f,w,.16f,.1f,"515F58");B("RustPatch",w*.23f,.29f,-d/2-.13f,.56f,.11f,.02f,"866851");}
  public void Wall(Vector3 p,float length,bool alongX){Begin("PortKit_PerimeterWall",p);B("ConcreteSolid",0,.9f,0,alongX?length:.36f,1.8f,alongX?.36f:length,"858C80",true);B("WallCap",0,1.83f,0,alongX?length+.1f:.48f,.13f,alongX?.48f:length+.1f,"AAA996");for(float i=-length/2;i<=length/2+.01f;i+=3){B("Pillar",alongX?i:0,1,alongX?0:i,.6f,2,.6f,"737D73",true);B("PillarCap",alongX?i:0,2.04f,alongX?0:i,.7f,.12f,.7f,"A3A796");}}
  public void Pallets(Vector3 p,int layers=2){layers=Mathf.Clamp(layers,1,3);Begin("PortKit_LowPallets",p);for(int l=0;l<layers;l++){float y=.08f+l*.18f;for(int i=0;i<5;i++)B("WoodSlat",-.56f+i*.28f,y,0,.22f,.07f,1.2f,i%2==0?"A2906E":"8F7B5B");foreach(float z in new[]{-.45f,.45f})B("WoodRunner",0,y-.045f,z,1.35f,.08f,.12f,"695F49");}B("PalletCollider",0,layers*.09f,0,1.4f,layers*.18f,1.2f,"887557",true).GetComponent<Renderer>().enabled=false;SceneObstacle.Apply(part.gameObject,ObstacleKind.Low);}
  public void TimberStack(Vector3 p){
   Begin("PortKit_HighTimberStack",p);
   // Dense shoulder-high timber bundle: a single uninterrupted blocking volume.
   B("DenseTimberCore",0,.9f,0,1.6f,1.8f,1.4f,"806C4F",true);
   foreach(float x in new[]{-.57f,.57f})B("BaseRunner",x,.08f,0,.23f,.16f,1.46f,"514C3B");
   for(int level=0;level<9;level++){
    float y=.25f+level*.175f;
    foreach(float z in new[]{-.715f,.715f}){B("PackedBoardFace",0,y,z,1.61f,.152f,.045f,level%3==0?"A18D68":level%3==1?"8F7856":"998260");B("BoardEndJoint",level%2==0?-.32f:.29f,y,z*1.02f,.025f,.145f,.025f,"5A5140");}
    foreach(float x in new[]{-.815f,.815f})B("PackedBoardEnd",x,y,0,.045f,.152f,1.44f,level%2==0?"948264":"A18C69");
   }
   for(int i=0;i<6;i++)B("TopTimber",-.67f+i*.27f,1.82f,0,.25f,.09f,1.48f,i%2==0?"A28E6C":"8E7B5D");
   foreach(float x in new[]{-.52f,.52f}){
    foreach(float z in new[]{-.75f,.75f})B("SteelPackingStrap",x,.96f,z,.095f,1.78f,.035f,"414F49");
    B("TopPackingStrap",x,1.875f,0,.095f,.025f,1.53f,"53615A");
   }
   B("WeatherCover",0,1.894f,.32f,1.5f,.025f,.56f,"6A7565");
   SceneObstacle.Apply(part.gameObject,ObstacleKind.High);
  }
  public void Barrel(Vector3 p,string color){Begin("PortKit_ClosedDrum",p);Cylinder("MetalDrum",0,.48f,0,.65f,.96f,color,true);Cylinder("TopRim",0,.95f,0,.69f,.06f,"879184");Cylinder("MidBand",0,.48f,0,.69f,.07f,"384C48");Cylinder("BottomRim",0,.045f,0,.69f,.07f,"384C48");Cylinder("Cap",.14f,1,.06f,.09f,.025f,"B6AF8B");SceneObstacle.Apply(part.gameObject,ObstacleKind.Low);}
  public void Bench(Vector3 p){Begin("PortKit_Workbench",p);B("BenchTop",0,.78f,0,2,.16f,.85f,"94896C",true);foreach(float x in new[]{-.8f,.8f})foreach(float z in new[]{-.3f,.3f})B("MetalLeg",x,.4f,z,.09f,.8f,.09f,"394B48");B("Shelf",0,.25f,0,1.8f,.09f,.65f,"5C6B61");B("ViceBase",.6f,.92f,0,.35f,.16f,.28f,"4A6160");B("ViceJaw",.6f,1.07f,0,.4f,.16f,.17f,"87928A");SceneObstacle.Apply(part.gameObject,ObstacleKind.Low);}
  public void Pickup(Vector3 p){Begin("PortKit_AbandonedPickup",p);B("Chassis",0,.43f,0,1.8f,.3f,3.6f,"364543",true);B("Hood",0,.85f,1.05f,1.72f,.53f,1.2f,"89938A",true);B("Cab",0,1.05f,-.02f,1.72f,.9f,1.2f,"78877D",true);B("Windshield",0,1.28f,.61f,1.5f,.39f,.035f,"384F55");B("Roof",0,1.56f,-.02f,1.83f,.11f,1.35f,"A0A398");B("BedFloor",0,.67f,-1.32f,1.62f,.1f,1.25f,"495B54");foreach(float x in new[]{-.83f,.83f}){B("BedRail",x,.91f,-1.32f,.12f,.48f,1.32f,"7C8B7E");foreach(float z in new[]{-1.2f,1.12f}){var wheel=Cylinder("RubberWheel",x,.38f,z,.7f,.22f,"263433");wheel.transform.rotation=Quaternion.Euler(0,0,90);}}B("Tailgate",0,.92f,-1.97f,1.78f,.47f,.13f,"829080");B("FrontBumper",0,.48f,1.76f,1.98f,.17f,.16f,"ABB0A0");foreach(float x in new[]{-.61f,.61f})B("Headlamp",x,.86f,1.67f,.32f,.19f,.055f,"C6BB8D");}
  public void Lamp(Vector3 p,float yaw=0){Begin("PortKit_YardLamp",p);B("Base",0,.13f,0,.48f,.26f,.48f,"646D63",true);B("Pole",0,1.65f,0,.1f,3.3f,.1f,"465651");B("Arm",.26f,3.23f,0,.62f,.08f,.08f,"55645C");B("Hood",.5f,3.16f,0,.48f,.18f,.32f,"3E504B");B("Glass",.5f,3.055f,0,.4f,.03f,.26f,"CDBB8A");part.rotation=Quaternion.Euler(0,yaw,0);SceneObstacle.Apply(part.gameObject,ObstacleKind.Low);}
 }
}
