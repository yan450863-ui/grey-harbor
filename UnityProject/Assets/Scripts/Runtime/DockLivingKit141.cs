using UnityEngine;
using UnityEngine.Rendering;
namespace GreyHarbor {
 public sealed class DockLivingKit141 {
  readonly RaidMap map;Transform root;WorkshopWastelandArt art;
  public DockLivingKit141(RaidMap map){this.map=map;}
  void Begin(string id,Vector3 p){root=new GameObject("DockKit_"+id).transform;root.SetParent(map.Root,false);root.position=p;root.gameObject.AddComponent<DockAssetInfo141>().Asset=id;art=new WorkshopWastelandArt(map);}
  void C(string id,string color,Vector3 p,Vector3 size,bool surface=true)=>art.Cube(id,color,p,size,surface);
  void T(string id,string color,Vector3 a,Vector3 b,float r)=>art.Tube(id,color,a,b,r);
  void Hit(Vector3 p,Vector3 size,ObstacleKind kind){var go=new GameObject("DockCollision_"+kind);go.transform.SetParent(root,false);go.transform.localPosition=p;go.AddComponent<BoxCollider>().size=size;SceneObstacle.Apply(go,kind);}
  Transform End(float yaw=0){art.Bake(root.gameObject,root.gameObject.AddComponent<WorkshopArtResources>());root.rotation=Quaternion.Euler(0,yaw,0);foreach(var r in root.GetComponentsInChildren<Renderer>())r.shadowCastingMode=ShadowCastingMode.On;return root;}
  void Tyre(Vector3 p){for(int i=0;i<16;i++){float a=i*Mathf.PI/8,b=(i+1)*Mathf.PI/8;T("RubberFender","293F40",p+new Vector3(0,Mathf.Cos(a)*.27f,Mathf.Sin(a)*.27f),p+new Vector3(0,Mathf.Cos(b)*.27f,Mathf.Sin(b)*.27f),.065f);}}
  void Hull(bool fish){float w=fish?4:5,l=fish?10:12,stern=-l/2;string paint=fish?"648681":"8A7454";
   var ring=new[]{new Vector3(-w/2,0,stern),new Vector3(w/2,0,stern),new Vector3(w/2,0,l/2-3),new Vector3(w/2-1,0,l/2),new Vector3(-w/2+1,0,l/2),new Vector3(-w/2,0,l/2-3)};
   var deck=new GameObject("BoatDeck141");deck.transform.SetParent(root,false);var mesh=new Mesh{name="BoatDeck141"};var vertices=new Vector3[ring.Length+1];vertices[0]=Vector3.up*.025f;for(int i=0;i<ring.Length;i++)vertices[i+1]=ring[i]+Vector3.up*.025f;var triangles=new System.Collections.Generic.List<int>();for(int i=0;i<ring.Length;i++){triangles.Add(0);triangles.Add((i+1)%ring.Length+1);triangles.Add(i+1);}mesh.vertices=vertices;mesh.SetTriangles(triangles,0);mesh.RecalculateNormals();mesh.RecalculateBounds();deck.AddComponent<MeshFilter>().sharedMesh=mesh;deck.AddComponent<MeshRenderer>().sharedMaterial=map.Mat(fish?"91866A":"738779",false);deck.AddComponent<WorkshopArtResources>().Meshes.Add(mesh);
   for(int i=1;i<ring.Length;i++){Vector3 a=ring[i],b=ring[(i+1)%ring.Length];var direction=(b-a).normalized;art.Panel("WeatheredHull",paint,(a+b)/2+Vector3.down*.18f,direction,Vector3.up,Vector3.Distance(a,b),.67f);T("Gunwale","A2A98D",a+Vector3.up*.26f,b+Vector3.up*.26f,.065f);T("Waterline","3C5654",a-Vector3.up*.45f,b-Vector3.up*.45f,.06f);}
   foreach(float x in new[]{-w/2+.45f,w/2-.45f}){C("SternShoulder",paint,new Vector3(x,-.12f,stern),new Vector3(.85f,.70f,.16f));C("SternCap","A2A98D",new Vector3(x,.28f,stern),new Vector3(.88f,.09f,.22f));}
   foreach(float z in new[]{stern+2,0,l/2-3.3f})foreach(float side in new[]{-1.1f,1.1f}){Tyre(new Vector3(side*(w/2+.10f),-.04f,z));T("FenderCord","A29672",new Vector3(side*(w/2+.03f),.35f,z),new Vector3(side*(w/2+.10f),.04f,z),.02f);}
   C("ClosedWheelhouse",fish?"AAB09C":"869184",new Vector3(0,1.25f,l/2-1.9f),new Vector3(fish?1.8f:2,2.5f,2.1f));Hit(new Vector3(0,1.25f,l/2-1.9f),new Vector3(fish?1.8f:2,2.5f,2.1f),ObstacleKind.High);
   C("WheelhouseRoof","788D83",new Vector3(0,2.58f,l/2-1.9f),new Vector3(fish?2.08f:2.25f,.16f,2.32f));C("DarkBridgeGlass","3B595B",new Vector3(0,1.7f,l/2-2.965f),new Vector3(1.43f,.7f,.03f));C("ClosedCabinDoor","5D746A",new Vector3(.48f,.9f,l/2-2.982f),new Vector3(.55f,1.65f,.03f));
   T("Antenna","708B80",new Vector3(.62f,2.65f,l/2-1.3f),new Vector3(.62f,3.65f,l/2-1.3f),.026f);
  }
  public Transform FishingBoat(Vector3 p){Begin("FishingBoat",p);Hull(true);
   foreach(float x in new[]{-.9f,.9f})T("NetFrame","887B59",new Vector3(x,.1f,.25f),new Vector3(x,1.9f,.25f),.055f);T("CrossNetBar","887B59",new Vector3(-.9f,1.9f,.25f),new Vector3(.9f,1.9f,.25f),.055f);
   for(int i=0;i<7;i++){float x=-.72f+i*.24f;T("NetStrand","707B63",new Vector3(x,1.7f,.27f),new Vector3(x+.17f,.55f,.34f),.015f);}for(int i=0;i<5;i++)T("NetCross","7A8368",new Vector3(-.73f,.67f+i*.23f,.30f),new Vector3(.75f,.67f+i*.23f,.30f),.012f);
   C("FishSortingBench","8E9B87",new Vector3(1.35f,.52f,-.6f),new Vector3(.65f,1.04f,1.8f));Hit(new Vector3(1.35f,.52f,-.6f),new Vector3(.65f,1.04f,1.8f),ObstacleKind.Low);
   C("NetBasket","6D7E5B",new Vector3(-1.25f,.23f,-.4f),new Vector3(.7f,.46f,.9f));Hit(new Vector3(-1.25f,.23f,-.4f),new Vector3(.7f,.46f,.9f),ObstacleKind.Low);
   return End();
  }
  public Transform WorkBarge(Vector3 p){Begin("WorkBarge",p);Hull(false);
   C("WinchBase","55746D",new Vector3(-1.75f,.35f,1.6f),new Vector3(.7f,.7f,.8f));Hit(new Vector3(-1.75f,.35f,1.6f),new Vector3(.7f,.7f,.8f),ObstacleKind.Low);for(int i=0;i<6;i++)T("WinchCable","9E8B63",new Vector3(-2.03f+i*.10f,.7f,1.25f),new Vector3(-2.03f+i*.10f,.7f,1.9f),.026f);
   return End();
  }
  public Transform Gangway(Vector3 p,float length=4){Begin("BoardingGangway",p);C("GangwayDeck","819388",new Vector3(0,.04f,0),new Vector3(2.2f,.06f,length),false);for(float z=-length/2+.2f;z<length/2;z+=.38f)C("AntiSlipBar","A7A88A",new Vector3(0,.083f,z),new Vector3(2.08f,.022f,.035f),false);
   foreach(float x in new[]{-1.1f,1.1f}){C("GangwayKerb","556D65",new Vector3(x,.13f,0),new Vector3(.065f,.20f,length));T("BoardingHandrail","839388",new Vector3(x,.80f,-length/2),new Vector3(x,.80f,length/2),.036f);}return End();
  }
  public Transform OpenShelter(Vector3 p,float yaw=0){Begin("OpenCargoShelter",p);foreach(float x in new[]{-2.8f,2.8f})foreach(float z in new[]{-1.9f,1.9f}){C("SteelPost","6B7B6C",new Vector3(x,1.45f,z),new Vector3(.16f,2.9f,.16f));Hit(new Vector3(x,1.45f,z),new Vector3(.20f,2.9f,.20f),ObstacleKind.High);}C("SheetRoof","718B87",new Vector3(0,2.96f,0),new Vector3(6.25f,.12f,4.5f));for(int i=0;i<14;i++)C("RoofCorrugation","929A84",new Vector3(-2.99f+i*.46f,3.04f,0),new Vector3(.035f,.028f,4.5f));T("ShelterCrossBrace","7D7560",new Vector3(-2.8f,2.2f,1.9f),new Vector3(2.8f,2.82f,1.9f),.032f);return End(yaw);}
  public Transform Workbench(Vector3 p,float yaw=0){Begin("DockWorkbench",p);C("SteelBenchTop","9C9C83",new Vector3(0,.86f,0),new Vector3(2.2f,.12f,.9f));foreach(float x in new[]{-.85f,.85f})foreach(float z in new[]{-.32f,.32f})C("BenchLeg","5F7770",new Vector3(x,.4f,z),new Vector3(.09f,.8f,.09f));Hit(new Vector3(0,.43f,0),new Vector3(2.2f,.86f,.9f),ObstacleKind.Low);C("BenchVice","657D76",new Vector3(.79f,.99f,.02f),new Vector3(.23f,.18f,.34f));return End(yaw);}
  public Transform JibCrane(Vector3 p,float yaw=0){Begin("SmallJibCrane",p);C("CraneFoot","697D73",new Vector3(0,.28f,0),new Vector3(1.9f,.56f,1.9f));C("CraneMast","A58A59",new Vector3(0,2.25f,0),new Vector3(.55f,4.5f,.55f));Hit(new Vector3(0,1.45f,0),new Vector3(1.9f,2.9f,1.9f),ObstacleKind.High);T("JibBeam","BA9A61",new Vector3(0,4.3f,0),new Vector3(4.4f,5.2f,0),.18f);T("JibBrace","83775A",new Vector3(0,3.0f,0),new Vector3(3.7f,5.05f,0),.10f);T("HoistCable","384F4C",new Vector3(4.4f,5.2f,0),new Vector3(4.4f,2.1f,0),.025f);T("Hook","808C7C",new Vector3(4.4f,2.1f,0),new Vector3(4.52f,1.82f,0),.057f);return End(yaw);}
  public Transform Bollard(Vector3 p,float yaw=0){Begin("MooringBollard",p);C("BasePlate","71847C",new Vector3(0,.075f,0),new Vector3(.70f,.15f,.58f));C("IronBollard","536E69",new Vector3(0,.32f,0),new Vector3(.25f,.64f,.25f));C("TieBar","829080",new Vector3(0,.55f,0),new Vector3(.67f,.17f,.30f));Hit(new Vector3(0,.30f,0),new Vector3(.70f,.60f,.58f),ObstacleKind.Low);return End(yaw);}
  public Transform CableDrum(Vector3 p,float yaw=0){Begin("DockCableDrum",p);foreach(float x in new[]{-.48f,.48f}){C("SpoolFlange","9C8A68",new Vector3(x,.57f,0),new Vector3(.10f,1.14f,1.14f));C("SpoolHub","5B7266",new Vector3(x,.57f,0),new Vector3(.14f,.21f,.21f));}C("DarkCable","3E5752",new Vector3(0,.57f,0),new Vector3(.9f,.78f,.78f));for(int i=0;i<9;i++)C("CableWind","68745D",new Vector3(-.4f+i*.10f,.99f,0),new Vector3(.026f,.023f,.72f));Hit(new Vector3(0,.55f,0),new Vector3(1.12f,1.1f,1.16f),ObstacleKind.Low);return End(yaw);}
  public Transform CargoTrolley(Vector3 p,float yaw=0){Begin("DockCargoTrolley",p);C("LoadingTray","879688",new Vector3(0,.25f,0),new Vector3(1.20f,.13f,1.7f));foreach(float x in new[]{-.47f,.47f})foreach(float z in new[]{-.63f,.63f})C("SmallWheel","2F4947",new Vector3(x,.11f,z),new Vector3(.18f,.22f,.15f));foreach(float x in new[]{-.48f,.48f})T("PushStem","829588",new Vector3(x,.30f,.74f),new Vector3(x,.93f,.94f),.04f);T("PushHandle","87988A",new Vector3(-.48f,.93f,.94f),new Vector3(.48f,.93f,.94f),.045f);Hit(new Vector3(0,.43f,.08f),new Vector3(1.30f,.86f,1.9f),ObstacleKind.Low);return End(yaw);}
 }
}
