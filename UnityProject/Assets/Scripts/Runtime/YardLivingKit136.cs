using UnityEngine;
using UnityEngine.Rendering;
using System.Collections.Generic;
namespace GreyHarbor {
 // All dimensions in metres. Reusable assets contain no loot, AI or indoor portals.
 public sealed partial class YardLivingKit {
  readonly RaidMap map;Transform root;WorkshopWastelandArt art;
  public YardLivingKit(RaidMap m){map=m;}
  void Begin(string name,Vector3 p){root=new GameObject("YardKit_"+name).transform;root.SetParent(map.Root,false);root.position=p;root.gameObject.AddComponent<YardAssetInfo>().Asset=name;art=new WorkshopWastelandArt(map);}
  void C(string name,string color,Vector3 p,Vector3 size,bool surface=true)=>art.Cube(name,color,p,size,surface);
  void T(string name,string color,Vector3 a,Vector3 b,float r)=>art.Tube(name,color,a,b,r);
  void Hit(Vector3 p,Vector3 size,ObstacleKind kind){var go=new GameObject("Collision_"+kind);go.transform.SetParent(root,false);go.transform.localPosition=p;go.AddComponent<BoxCollider>().size=size;SceneObstacle.Apply(go,kind);}
  Transform End(float yaw=0,bool shadows=true){art.Bake(root.gameObject,root.gameObject.AddComponent<WorkshopArtResources>());root.rotation=Quaternion.Euler(0,yaw,0);if(shadows)foreach(var r in root.GetComponentsInChildren<Renderer>())r.shadowCastingMode=ShadowCastingMode.On;return root;}
  public Transform Bicycle(Vector3 p,float yaw=0,bool old=false){
   Begin(old?"OldBicycle":"Bicycle",p);string steel=old?"826E51":"5F8175";var rear=new Vector3(-.62f,.38f,0);var front=new Vector3(.62f,.38f,0);var crank=new Vector3(-.06f,.37f,0);var seat=new Vector3(-.24f,.88f,0);var head=new Vector3(.43f,.88f,0);
   foreach(var center in new[]{rear,front}){
    for(int i=0;i<24;i++){float a=i*Mathf.PI/12,b=(i+1)*Mathf.PI/12;T("Tyre","2B3935",center+new Vector3(Mathf.Cos(a),Mathf.Sin(a),0)*.35f,center+new Vector3(Mathf.Cos(b),Mathf.Sin(b),0)*.35f,.034f);
     T("Rim","989D88",center+new Vector3(Mathf.Cos(a),Mathf.Sin(a),.013f)*.31f,center+new Vector3(Mathf.Cos(b),Mathf.Sin(b),.013f)*.31f,.009f);
     if(i%3==0)T("Spokes","849183",center+Vector3.forward*.017f,center+new Vector3(Mathf.Cos(a)*.29f,Mathf.Sin(a)*.29f,.017f),.006f);}
   }
   foreach(var pair in new[]{new[]{rear,crank},new[]{rear,seat},new[]{crank,seat},new[]{crank,head},new[]{seat,head},new[]{head,front}})T("SteelFrame",steel,pair[0],pair[1],.026f);
   T("SeatPost","768776",seat,seat+Vector3.up*.10f,.025f);C("Saddle","353B35",seat+Vector3.up*.13f,new Vector3(.30f,.065f,.17f));
   T("HandleStem","829387",head,head+new Vector3(.06f,.23f,0),.024f);T("Handlebar","4C6056",head+new Vector3(.06f,.23f,-.28f),head+new Vector3(.06f,.23f,.28f),.023f);
   T("Chain","515849",rear+Vector3.forward*.07f,crank+Vector3.forward*.07f,.01f);T("CrankArm","8C9282",crank+Vector3.forward*.11f,crank+new Vector3(.12f,-.10f,.11f),.019f);
   C("Pedal","38483C",crank+new Vector3(.12f,-.1f,.16f),new Vector3(.12f,.035f,.15f));
   C("RearCarrier",steel,new Vector3(-.57f,.86f,0),new Vector3(.45f,.035f,.28f));T("CarrierStay",steel,new Vector3(-.78f,.85f,0),rear,.017f);
   T("Kickstand","5B6C5C",crank,new Vector3(-.15f,.04f,-.23f),.014f);Hit(new Vector3(0,.57f,0),new Vector3(1.95f,1.14f,.58f),ObstacleKind.Low);return End(yaw);
  }
  public Transform GardenBed(Vector3 p,bool neglected=false){
   Begin(neglected?"FallowBed":"VegetableBed",p);C("CultivatedSoil","675F46",new Vector3(0,.056f,0),new Vector3(4,.08f,6),false);
   for(int i=0;i<8;i++){float z=-2.65f+i*.75f;C("RaisedFurrow",i%2==0?"76654C":"827255",new Vector3(0,.115f,z),new Vector3(3.6f,.12f,.44f),false);
    for(int j=0;j<6;j++){if(neglected&&(i+j)%3!=0)continue;float x=-1.5f+j*.6f;var q=new Vector3(x,.22f,z);T("CropStem","6C794C",q,q+Vector3.up*.21f,.014f);
     for(int k=0;k<4;k++){float a=k*Mathf.PI/2;var dir=new Vector3(Mathf.Cos(a),.45f,Mathf.Sin(a)).normalized;art.Panel("VegetableLeaves",neglected?"848361":"647F4C",q+dir*.14f,Vector3.Cross(Vector3.up,dir).normalized,dir,.13f,.32f,false);}
    }
   }
   foreach(float x in new[]{-2.05f,2.05f})C("WeatheredBorder","8C8265",new Vector3(x,.08f,0),new Vector3(.085f,.12f,6.1f),false);
   T("GardenMarker","9A8B66",new Vector3(1.8f,.02f,-2.8f),new Vector3(1.8f,.55f,-2.8f),.023f);return End(0,false);
  }
  public Transform WashStation(Vector3 p,float yaw=0){
   Begin("WashStation",p);C("ConcreteStand","979B84",new Vector3(0,.48f,0),new Vector3(1.8f,.96f,.67f));
   C("DarkSink","465649",new Vector3(0,1.0f,0),new Vector3(1.47f,.03f,.50f));
   foreach(float x in new[]{-.86f,.86f})C("SinkEnd","A6AA91",new Vector3(x,1.09f,0),new Vector3(.13f,.21f,.72f));
   foreach(float z in new[]{-.30f,.30f})C("SinkLip","A6AA91",new Vector3(0,1.09f,z),new Vector3(1.75f,.21f,.12f));
   T("WaterPipe","72887C",new Vector3(.4f,.1f,.42f),new Vector3(.4f,1.35f,.42f),.033f);T("Tap","95A292",new Vector3(.4f,1.35f,.42f),new Vector3(.4f,1.35f,.03f),.029f);C("TapHandle","738273",new Vector3(.4f,1.4f,.27f),new Vector3(.19f,.03f,.05f));
   C("DrainGrate","384B40",new Vector3(.4f,.021f,-.75f),new Vector3(.7f,.024f,.38f),false);Hit(new Vector3(0,.58f,0),new Vector3(1.9f,1.16f,.82f),ObstacleKind.Low);return End(yaw);
  }
  public Transform Clothesline(Vector3 p,float yaw=0){
   Begin("Clothesline",p);foreach(float x in new[]{-3f,3f}){T("SteelPost","67776B",new Vector3(x,.01f,0),new Vector3(x,2.05f,0),.055f);Hit(new Vector3(x,1,0),new Vector3(.14f,2,.14f),ObstacleKind.High);}
   for(int i=0;i<12;i++){float x=-3+i*.5f;T("SaggingLine","A59E81",new Vector3(x,1.96f+Mathf.Abs(x)*.03f,0),new Vector3(x+.5f,1.96f+Mathf.Abs(x+.5f)*.03f,0),.008f);}
   foreach(float x in new[]{-1.6f,-.4f,1.0f}){C("FadedLaundry",x<0?"9A9E83":"8A9380",new Vector3(x,1.55f,0),new Vector3(.75f,.85f,.035f));for(int k=0;k<5;k++)C("FabricFold","7D8774",new Vector3(x-.28f+k*.14f,1.53f,-.021f),new Vector3(.018f,.70f,.006f));}
   return End(yaw);
  }
  public Transform PicnicSet(Vector3 p,float yaw=0){
   Begin("PicnicSet",p);for(int i=0;i<6;i++)C("TableBoard",i%2==0?"978667":"84775E",new Vector3(0,.8f,-.48f+i*.19f),new Vector3(2.4f,.08f,.17f));
   foreach(float x in new[]{-.86f,.86f}){T("TableLeg","566553",new Vector3(x,.05f,-.4f),new Vector3(x,.77f,.4f),.05f);T("TableLeg","566553",new Vector3(x,.05f,.4f),new Vector3(x,.77f,-.4f),.05f);}
   foreach(float z in new[]{-1.0f,1.0f}){C("BenchSeat","8F8064",new Vector3(0,.45f,z),new Vector3(2.5f,.085f,.4f));foreach(float x in new[]{-.85f,.85f})C("BenchLeg","566653",new Vector3(x,.22f,z),new Vector3(.09f,.44f,.35f));Hit(new Vector3(0,.25f,z),new Vector3(2.55f,.5f,.45f),ObstacleKind.Low);}
   Hit(new Vector3(0,.44f,0),new Vector3(2.45f,.88f,1.15f),ObstacleKind.Low);return End(yaw);
  }
  public Transform WaterTank(Vector3 p,float yaw=0){
   Begin("RainwaterTank",p);C("Tank","657C74",new Vector3(0,1,0),new Vector3(1.65f,1.65f,1.4f));for(int i=0;i<4;i++)C("TankRib","849083",new Vector3(0,.35f+i*.4f,0),new Vector3(1.7f,.07f,1.45f));
   C("Lid","849388",new Vector3(0,1.88f,0),new Vector3(1.78f,.12f,1.51f));foreach(float x in new[]{-.6f,.6f})C("Stand","484F42",new Vector3(x,.12f,0),new Vector3(.16f,.24f,1.4f));
   T("OutletTap","A09976",new Vector3(.4f,.36f,-.73f),new Vector3(.4f,.36f,-.99f),.035f);Hit(new Vector3(0,1,0),new Vector3(1.78f,2,1.51f),ObstacleKind.High);return End(yaw);
  }
  public Transform Compost(Vector3 p,float yaw=0){
   Begin("CompostBay",p);for(int i=0;i<5;i++){float y=.12f+i*.17f;foreach(float x in new[]{-.8f,.8f})C("SlattedSide","88765A",new Vector3(x,y,0),new Vector3(.11f,.12f,1.6f));C("SlattedBack","786D53",new Vector3(0,y,.78f),new Vector3(1.6f,.12f,.1f));}
   C("CompostFill","525D42",new Vector3(0,.3f,0),new Vector3(1.45f,.58f,1.45f));Hit(new Vector3(0,.44f,0),new Vector3(1.75f,.88f,1.75f),ObstacleKind.Low);return End(yaw);
  }
  public Transform LeanTo(Vector3 p,float yaw=0){
   Begin("GardenLeanTo",p);foreach(float x in new[]{-1.6f,1.6f})foreach(float z in new[]{-.75f,.75f}){C("TimberPost","736C52",new Vector3(x,1.15f,z),new Vector3(.12f,2.3f,.12f));Hit(new Vector3(x,1.15f,z),new Vector3(.15f,2.3f,.15f),ObstacleKind.High);}
   C("OldSheetRoof","6D7F73",new Vector3(0,2.33f,0),new Vector3(3.6f,.1f,2.1f));for(int i=0;i<10;i++)C("RoofRib","869081",new Vector3(-1.62f+i*.36f,2.39f,0),new Vector3(.035f,.035f,2.1f));
   C("ShelfBoard","918063",new Vector3(0,.8f,.68f),new Vector3(2.8f,.09f,.45f));Hit(new Vector3(0,.43f,.68f),new Vector3(2.8f,.86f,.5f),ObstacleKind.Low);return End(yaw);
  }
  public Transform Grass(Vector3 p,float w,float d,bool tall,int seed){
   Begin(tall?"TallGrass":"LowMeadow",p);var rng=new System.Random(seed);
   for(int i=0;i<w*d*(tall?7:9);i++){float x=((float)rng.NextDouble()-.5f)*w,z=((float)rng.NextDouble()-.5f)*d,h=(tall?.55f:.12f)+(float)rng.NextDouble()*(tall?.65f:.17f);
    for(int k=0;k<3;k++){float a=(float)rng.NextDouble()*Mathf.PI*2;Vector3 right=new Vector3(Mathf.Cos(a),0,Mathf.Sin(a));var up=(Vector3.up+new Vector3((float)rng.NextDouble()*.4f,0,(float)rng.NextDouble()*.4f)).normalized;art.Panel("GrassBlades",i%4==0?"9A9470":i%4==1?"6F8155":"7E8B5B",new Vector3(x,.035f+h/2,z),right,up,tall?.06f:.045f,h,false);
     if(tall&&i%4==0)T("SeedHeads","9D9470",new Vector3(x,h*.87f,z),new Vector3(x,h+.08f,z),.018f);}
   }
   var info=root.gameObject.GetComponent<YardAssetInfo>();info.Decoration=true;return End(0,false);
  }
  public Transform Tree(Vector3 p,int size,int seed=136){
   Begin(size==0?"YoungTree":"MatureTree",p);float h=size==0?3.8f:6.7f,r=size==0?1.3f:2.1f;T("TreeTrunk","6A614D",Vector3.zero,Vector3.up*h*.73f,size==0?.12f:.23f);
   foreach(int side in new[]{-1,1})T("TreeBranch","73674F",Vector3.up*h*.48f,new Vector3(side*r*.68f,h*.73f,r*.35f),.065f);
   Hit(new Vector3(0,h*.36f,0),new Vector3(size==0?.28f:.5f,h*.72f,size==0?.28f:.5f),ObstacleKind.High);
   End();var owned=root.GetComponent<WorkshopArtResources>();var rng=new System.Random(seed);
   for(int crown=0;crown<3;crown++){
    Vector3 center=new Vector3((crown-1)*r*.62f,h*.74f+(crown==1?.6f:0),crown%2*r*.25f);var verts=new List<Vector3>();var tris=new List<int>();int sides=9;float[] rings={0,.75f,1,.70f,0};
    for(int layer=0;layer<4;layer++)for(int i=0;i<sides;i++){float a=i*2*Mathf.PI/sides,b=(i+1)*2*Mathf.PI/sides;float ra=r*(crown==1?1:.72f);Vector3 a0=center+new Vector3(Mathf.Cos(a)*ra*rings[layer],(layer-2)*r*.38f,Mathf.Sin(a)*ra*rings[layer]),a1=center+new Vector3(Mathf.Cos(b)*ra*rings[layer],(layer-2)*r*.38f,Mathf.Sin(b)*ra*rings[layer]),b0=center+new Vector3(Mathf.Cos(a)*ra*rings[layer+1],(layer-1)*r*.38f,Mathf.Sin(a)*ra*rings[layer+1]),b1=center+new Vector3(Mathf.Cos(b)*ra*rings[layer+1],(layer-1)*r*.38f,Mathf.Sin(b)*ra*rings[layer+1]);
     foreach(var q in new[]{a0,b0,a1,a1,b0,b1}){tris.Add(verts.Count);verts.Add(q);}}
    var mesh=new Mesh{name="BroadleafCrown"};mesh.SetVertices(verts);mesh.SetTriangles(tris,0);mesh.RecalculateNormals();var go=new GameObject("BroadleafCrown");go.transform.SetParent(root,false);go.AddComponent<MeshFilter>().sharedMesh=mesh;go.AddComponent<MeshRenderer>().sharedMaterial=map.Mat(crown%2==0?"65794E":"78835A",true);owned.Meshes.Add(mesh);
   }
   return root;
  }
 }
}
