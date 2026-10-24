using UnityEngine;
using UnityEngine.Rendering;
namespace GreyHarbor {
 // Metre-scale static workshop furnishings. Local -Z is the service/front side.
 // There are no loot points, enemies or room-sized collision envelopes in these models.
 public sealed class RepairInteriorKit200 {
  readonly RaidMap map;Transform root;WorkshopWastelandArt art;WorkshopArtResources owned;
  const string Steel="52685F",Edge="91A18E",Dark="30463E",Rust="937153",Rubber="293A32",Paint="748774";
  public RepairInteriorKit200(RaidMap m){map=m;}
  void Begin(string asset,Vector3 p,Vector3 envelope,string profile){
   root=new GameObject("RepairInteriorKit200_"+asset).transform;root.SetParent(map.Root,false);root.position=p;
   var info=root.gameObject.AddComponent<RepairAssetInfo149>();info.Asset=asset;info.Envelope=envelope;info.CollisionProfile=profile;
   owned=root.gameObject.AddComponent<WorkshopArtResources>();art=new WorkshopWastelandArt(map);
  }
  void C(string name,string color,Vector3 p,Vector3 size)=>art.Cube(name,color,p,size);
  void T(string name,string color,Vector3 a,Vector3 b,float radius)=>art.Tube(name,color,a,b,radius);
  void Hit(string name,Vector3 p,Vector3 size,ObstacleKind kind){
   var g=new GameObject("Collision_"+name+"_"+kind);g.transform.SetParent(root,false);g.transform.localPosition=p;
   g.AddComponent<BoxCollider>().size=size;SceneObstacle.Apply(g,kind);
  }
  void RustMark(Vector3 p,float w,float h)=>art.Patch("Weathering",Rust,p,Vector3.right,Vector3.up,w,h,true);
  void Handle(Vector3 p,float w){
   foreach(float x in new[]{-w*.5f,w*.5f})T("DoorHandle",Dark,p+Vector3.right*x+Vector3.forward*.035f,p+Vector3.right*x,.014f);
   T("DoorHandle",Edge,p-Vector3.right*w*.5f,p+Vector3.right*w*.5f,.019f);
  }
  void Bolt(Vector3 p)=>C("Fasteners",Edge,p,new Vector3(.026f,.026f,.025f));
  void Leg(float x,float z,float h){C("SteelFrame",Steel,new Vector3(x,h*.5f,z),new Vector3(.065f,h,.065f));C("FootPlate",Dark,new Vector3(x,.028f,z),new Vector3(.13f,.055f,.13f));}
  void OpenTray(Vector3 p,Vector3 size,string color){
   C("PartsTrays",color,p,new Vector3(size.x,.035f,size.z));
   foreach(float x in new[]{-1f,1f})C("PartsTrays",color,p+new Vector3(x*(size.x-.035f)*.5f,size.y*.5f,0),new Vector3(.035f,size.y,size.z));
   C("PartsTrays",color,p+new Vector3(0,size.y*.5f,(size.z-.035f)*.5f),new Vector3(size.x,size.y,.035f));
   C("PartsTrays",color,p+new Vector3(0,size.y*.28f,-(size.z-.035f)*.5f),new Vector3(size.x,size.y*.56f,.035f));
   C("FadedLabels","B0AB8D",p+new Vector3(0,size.y*.3f,-size.z*.5f-.006f),new Vector3(size.x*.47f,.063f,.012f));
  }
  void Ring(Vector3 p,float radius,float thickness,Vector3 axis){
   Vector3 right=Vector3.Cross(axis,Mathf.Abs(axis.y)>.9f?Vector3.forward:Vector3.up).normalized,up=Vector3.Cross(axis,right);
   for(int i=0;i<12;i++){float a=i*Mathf.PI/6,b=(i+1)*Mathf.PI/6;T("StoredMetalParts",Edge,p+(right*Mathf.Cos(a)+up*Mathf.Sin(a))*radius,p+(right*Mathf.Cos(b)+up*Mathf.Sin(b))*radius,thickness);}
  }
  Transform End(float yaw){art.Bake(root.gameObject,owned);root.rotation=Quaternion.Euler(0,yaw,0);foreach(var r in root.GetComponentsInChildren<Renderer>())r.shadowCastingMode=ShadowCastingMode.On;return root;}
  // Four upright colliders and stocked shelf zones; the two upper empty gaps remain open to rays.
  public Transform PartsRack(Vector3 p,float yaw=0){
   Begin("PartsRack",p,new Vector3(2.44f,2.03f,.68f),"High narrow posts and stocked upper bins; low bottom stock; no room-wide blocker");
   foreach(float x in new[]{-1.15f,1.15f})foreach(float z in new[]{-.27f,.27f}){
    Leg(x,z,2);Hit("Upright",new Vector3(x,1,z),new Vector3(.08f,2,.08f),ObstacleKind.High);
    for(int i=0;i<7;i++)C("MountingSlots",Dark,new Vector3(x,.34f+i*.22f,-.309f),new Vector3(.025f,.035f,.008f));
   }
   foreach(float y in new[]{.16f,.87f,1.57f,1.98f}){
    C("ShelfDecks",Paint,new Vector3(0,y,0),new Vector3(2.28f,.045f,.58f));
    foreach(float z in new[]{-.29f,.29f})C("SteelFrame",Steel,new Vector3(0,y-.025f,z),new Vector3(2.34f,.075f,.045f));
   }
   T("RearBrace",Steel,new Vector3(-1.14f,.2f,.30f),new Vector3(1.14f,1.94f,.30f),.014f);
   T("RearBrace",Steel,new Vector3(1.14f,.2f,.30f),new Vector3(-1.14f,1.94f,.30f),.014f);
   foreach(float x in new[]{-.73f,.03f,.77f}){
    OpenTray(new Vector3(x,.20f,-.02f),new Vector3(.63f,.27f,.51f),"6C7F6B");
    Hit("LowerTray",new Vector3(x,.32f,-.02f),new Vector3(.64f,.31f,.52f),ObstacleKind.Low);
    for(int i=0;i<3;i++){C("StoredMetalParts",i%2==0?Edge:Rust,new Vector3(x-.18f+i*.16f,.27f,-.06f+i*.05f),new Vector3(.09f,.09f,.18f));}
   }
   foreach(float x in new[]{-.75f,0,.75f}){
    C("DustyCartons","988B6B",new Vector3(x,1.06f,.01f),new Vector3(.62f,.34f,.49f));
    C("CartonTape","B0A17B",new Vector3(x,1.237f,.01f),new Vector3(.075f,.014f,.50f));
    C("FadedLabels","BCB394",new Vector3(x,1.08f,-.247f),new Vector3(.22f,.11f,.014f));
    Hit("SpareCarton",new Vector3(x,1.06f,.01f),new Vector3(.62f,.34f,.49f),ObstacleKind.High);
   }
   OpenTray(new Vector3(-.74f,1.60f,-.02f),new Vector3(.64f,.24f,.50f),"7D846C");
   Hit("TopPartsBin",new Vector3(-.74f,1.72f,-.02f),new Vector3(.64f,.28f,.50f),ObstacleKind.High);
   foreach(float x in new[]{-.85f,-.63f})Ring(new Vector3(x,1.76f,-.04f),.09f,.019f,Vector3.forward);
   foreach(float x in new[]{.1f,.38f,.72f}){
    C("FilterBoxes",Dark,new Vector3(x,1.72f,.035f),new Vector3(.19f,.22f,.39f));
    for(int j=0;j<4;j++)C("FilterFins",Edge,new Vector3(x,1.65f+j*.042f,-.17f),new Vector3(.18f,.015f,.018f));
    Hit("FilterStock",new Vector3(x,1.72f,.035f),new Vector3(.21f,.24f,.40f),ObstacleKind.High);
   }
   RustMark(new Vector3(-.85f,.85f,-.319f),.35f,.08f);return End(yaw);
  }
  public Transform ToolWallBench(Vector3 p,float yaw=0){
   Begin("ToolWallBench",p,new Vector3(2.80f,1.94f,.94f),"Low 0.89m bench; narrow high rear tool board only");
   foreach(float x in new[]{-1.23f,1.23f})foreach(float z in new[]{-.33f,.32f})Leg(x,z,.84f);
   C("WorkTop","A29777",new Vector3(0,.84f,0),new Vector3(2.80f,.10f,.90f));
   C("SteelFrame",Steel,new Vector3(0,.16f,.03f),new Vector3(2.50f,.07f,.69f));
   C("ToolBoard",Dark,new Vector3(0,1.42f,.39f),new Vector3(2.74f,1.02f,.055f));
   foreach(float x in new[]{-1.33f,1.33f})C("SteelFrame",Paint,new Vector3(x,1.42f,.40f),new Vector3(.06f,1.04f,.07f));
   for(int row=0;row<5;row++)for(int col=0;col<14;col++)C("PegHoles","253D35",new Vector3(-1.18f+col*.18f,1.04f+row*.18f,.356f),new Vector3(.014f,.014f,.009f));
   for(int i=0;i<6;i++){
    float x=-1.05f+i*.24f,len=.23f+i*.027f;
    T("HangingWrenches",Edge,new Vector3(x,1.60f,.315f),new Vector3(x,1.60f-len,.315f),.014f);
    C("HangingWrenches",Edge,new Vector3(x,1.64f,.315f),new Vector3(.060f,.04f,.031f));
    foreach(float dx in new[]{-.036f,.036f})C("HangingWrenches",Edge,new Vector3(x+dx,1.672f,.315f),new Vector3(.022f,.065f,.03f));
    T("PegHooks",Rust,new Vector3(x,1.74f,.355f),new Vector3(x,1.74f,.285f),.009f);
   }
   foreach(float x in new[]{.50f,.72f,.94f}){
    T("Screwdrivers",Edge,new Vector3(x,1.24f,.31f),new Vector3(x,1.51f,.31f),.010f);
    C("ScrewdriverGrips",x>.8f?"967F59":"6D8368",new Vector3(x,1.61f,.31f),new Vector3(.055f,.19f,.055f));
   }
   C("ViseBody",Steel,new Vector3(-.96f,.995f,-.20f),new Vector3(.29f,.21f,.26f));
   foreach(float z in new[]{-.32f,-.09f})C("ViseJaws",Edge,new Vector3(-.96f,1.10f,z),new Vector3(.34f,.095f,.06f));
   T("ViseSpindle",Edge,new Vector3(-.96f,1.01f,-.30f),new Vector3(-.96f,1.01f,-.45f),.020f);
   T("ViseHandle",Rust,new Vector3(-1.09f,1.01f,-.43f),new Vector3(-.83f,1.01f,-.43f),.017f);
   OpenTray(new Vector3(.85f,.907f,-.06f),new Vector3(.64f,.11f,.39f),Steel);
   foreach(float x in new[]{.66f,.86f,1.06f})T("LooseSockets",Edge,new Vector3(x,.93f,-.11f),new Vector3(x,.99f,-.11f),.036f);
   foreach(float x in new[]{-.58f,.02f}){
    C("DrawerUnit",Paint,new Vector3(x,.57f,.0f),new Vector3(.52f,.39f,.66f));
    for(int i=0;i<3;i++){C("DrawerFront",i%2==0?"7B8C76":"637C69",new Vector3(x,.455f+i*.11f,-.341f),new Vector3(.48f,.097f,.03f));Handle(new Vector3(x,.455f+i*.11f,-.371f),.16f);}
   }
   for(int i=0;i<7;i++)art.Stroke("TopScars","76694F",new Vector3(-.43f+i*.18f,.898f,-.31f),new Vector3(-.34f+i*.18f,.898f,-.16f),.007f,Vector3.up,true);
   RustMark(new Vector3(.98f,.79f,-.458f),.36f,.05f);
   Hit("Workbench",new Vector3(0,.445f,0),new Vector3(2.80f,.89f,.90f),ObstacleKind.Low);
   Hit("RearBoard",new Vector3(0,1.42f,.40f),new Vector3(2.76f,1.04f,.08f),ObstacleKind.High);return End(yaw);
  }
  public Transform EngineBench(Vector3 p,float yaw=0){
   Begin("EngineBench",p,new Vector3(2.00f,1.15f,1.10f),"Low assembly table and low dismantled engine; no high ray blocker");
   foreach(float x in new[]{-.84f,.84f})foreach(float z in new[]{-.43f,.43f})Leg(x,z,.69f);
   C("WorkTop",Steel,new Vector3(0,.69f,0),new Vector3(2,.08f,1.10f));
   C("LowerShelf",Dark,new Vector3(0,.18f,0),new Vector3(1.75f,.065f,.89f));
   OpenTray(new Vector3(.54f,.23f,0),new Vector3(.65f,.16f,.58f),"8C8164");
   C("OilDripTray",Dark,new Vector3(-.27f,.754f,.0f),new Vector3(1.05f,.04f,.75f));
   C("EngineBlock","4C5F54",new Vector3(-.27f,.93f,.03f),new Vector3(.69f,.29f,.55f));
   C("EngineSump",Rubber,new Vector3(-.27f,.803f,.03f),new Vector3(.59f,.065f,.45f));
   foreach(float x in new[]{-.55f,.01f})C("RockerCover",Paint,new Vector3(x,1.095f,.03f),new Vector3(.19f,.09f,.50f));
   for(int i=0;i<4;i++){
    float z=-.14f+i*.11f;
    T("ExhaustManifold",Rust,new Vector3(.07f,.965f,z),new Vector3(.23f,.86f,z),.025f);
    T("IgnitionLeads",Dark,new Vector3(-.55f,1.14f,z),new Vector3(-.31f,1.077f,z+.015f),.009f);
    Bolt(new Vector3(-.62f,1.10f,z));Bolt(new Vector3(.085f,1.10f,z));
   }
   Ring(new Vector3(-.27f,.952f,-.276f),.10f,.020f,Vector3.forward);
   T("Crankshaft",Edge,new Vector3(-.27f,.95f,-.28f),new Vector3(-.27f,.95f,-.41f),.021f);
   foreach(float x in new[]{.53f,.79f}){Ring(new Vector3(x,.79f,-.22f),.075f,.018f,Vector3.up);C("LoosePistons",Edge,new Vector3(x,.825f,.10f),new Vector3(.12f,.17f,.12f));}
   C("Rag","A99C7F",new Vector3(.48f,.748f,.37f),new Vector3(.31f,.025f,.18f));
   T("Spanner",Edge,new Vector3(.33f,.75f,-.39f),new Vector3(.68f,.75f,-.39f),.017f);
   RustMark(new Vector3(.82f,.667f,-.558f),.23f,.05f);
   Hit("AssemblyBench",new Vector3(0,.37f,0),new Vector3(2,.74f,1.10f),ObstacleKind.Low);
   Hit("Engine",new Vector3(-.25f,.945f,.0f),new Vector3(1.02f,.40f,.81f),ObstacleKind.Low);return End(yaw);
  }
  public Transform WashStation(Vector3 p,float yaw=0){
   Begin("WashStation",p,new Vector3(1.80f,1.04f,.70f),"Low washstand; tap and splashback are cosmetic");
   C("SinkCabinet",Paint,new Vector3(0,.38f,.025f),new Vector3(1.72f,.76f,.53f));
   foreach(float x in new[]{-.42f,.42f}){
    C("CabinetDoors","82917C",new Vector3(x,.39f,-.251f),new Vector3(.82f,.63f,.03f));Handle(new Vector3(x+Mathf.Sign(x)*-.28f,.46f,-.284f),.13f);
    foreach(float y in new[]{.16f,.61f})C("CabinetHinges",Dark,new Vector3(x+Mathf.Sign(x)*.365f,y,-.275f),new Vector3(.034f,.08f,.027f));
   }
   C("SinkWorktop","A7AD95",new Vector3(0,.78f,0),new Vector3(1.80f,.05f,.65f));
   foreach(float x in new[]{-.44f,.44f}){
    C("BasinWell",Dark,new Vector3(x,.813f,-.025f),new Vector3(.65f,.018f,.43f));
    C("BasinBase","889F92",new Vector3(x,.821f,-.025f),new Vector3(.54f,.014f,.32f));
    foreach(float dx in new[]{-.315f,.315f})C("BasinRim","BCC1AA",new Vector3(x+dx,.84f,-.025f),new Vector3(.04f,.046f,.43f));
    foreach(float z in new[]{-.225f,.175f})C("BasinRim","BCC1AA",new Vector3(x,.84f,z),new Vector3(.65f,.046f,.035f));
    C("DrainHole",Dark,new Vector3(x,.832f,-.06f),new Vector3(.075f,.006f,.075f));
    T("Tap",Edge,new Vector3(x,.83f,.22f),new Vector3(x,1.015f,.22f),.022f);
    T("Tap",Edge,new Vector3(x,1.015f,.22f),new Vector3(x,1.015f,.035f),.022f);
    T("TapSpout",Edge,new Vector3(x,1.015f,.035f),new Vector3(x,.962f,.035f),.020f);
    foreach(float dx in new[]{-.13f,.13f}){C("ValveBase",Dark,new Vector3(x+dx,.825f,.22f),new Vector3(.067f,.035f,.067f));T("ValveLever",Edge,new Vector3(x+dx-.035f,.87f,.22f),new Vector3(x+dx+.035f,.87f,.22f),.012f);}
   }
   C("Splashback","A5AD97",new Vector3(0,.91f,.301f),new Vector3(1.80f,.24f,.04f));
   C("SoapDish",Dark,new Vector3(.0f,.83f,.09f),new Vector3(.12f,.04f,.11f));C("Soap","BEB996",new Vector3(0,.861f,.09f),new Vector3(.085f,.025f,.07f));
   T("TowelRail",Edge,new Vector3(-.35f,.54f,-.31f),new Vector3(.35f,.54f,-.31f),.017f);
   C("WorkTowel","9F987B",new Vector3(.15f,.40f,-.327f),new Vector3(.26f,.29f,.025f));
   RustMark(new Vector3(-.68f,.12f,-.273f),.16f,.15f);
   Hit("WashCabinet",new Vector3(0,.43f,0),new Vector3(1.8f,.86f,.65f),ObstacleKind.Low);return End(yaw);
  }
  public Transform WorkLockers(Vector3 p,float yaw=0){
   Begin("WorkLockers",p,new Vector3(1.80f,2.03f,.64f),"High occupied lockers within their own 1.8x0.55m footprint");
   C("LockerCarcass",Dark,new Vector3(0,1.02f,.0f),new Vector3(1.80f,1.97f,.55f));
   C("LockerPlinth",Steel,new Vector3(0,.055f,0),new Vector3(1.78f,.11f,.53f));
   foreach(float x in new[]{-.60f,0,.60f}){
    C("LockerDoors",x==0?"718772":"81917B",new Vector3(x,1.06f,-.291f),new Vector3(.565f,1.83f,.026f));
    for(int i=0;i<4;i++){
     C("VentSlats",Dark,new Vector3(x,1.77f-i*.064f,-.308f),new Vector3(.35f,.022f,.012f));
     C("VentSlats",Dark,new Vector3(x,.38f-i*.060f,-.308f),new Vector3(.35f,.020f,.012f));
    }
    C("NamePlate",Edge,new Vector3(x,1.44f,-.311f),new Vector3(.23f,.075f,.014f));
    C("FadedLabels","BAB294",new Vector3(x,1.44f,-.321f),new Vector3(.18f,.043f,.007f));
    Handle(new Vector3(x+.19f,1.05f,-.337f),.10f);
    C("LockHasp",Dark,new Vector3(x+.19f,.94f,-.331f),new Vector3(.055f,.070f,.025f));
    foreach(float y in new[]{.56f,1.5f})C("CabinetHinges",Edge,new Vector3(x-.26f,y,-.316f),new Vector3(.025f,.13f,.025f));
    RustMark(new Vector3(x-.13f,.21f,-.315f),.18f,.13f);RustMark(new Vector3(x+.2f,1.67f,-.315f),.09f,.22f);
   }
   C("DustyTop",Paint,new Vector3(0,2.005f,0),new Vector3(1.8f,.05f,.55f));
   // Door seams and padlocks are visual only; these wardrobes are not searchable.
   Hit("LockerBody",new Vector3(0,1.015f,-.019f),new Vector3(1.80f,2.03f,.59f),ObstacleKind.High);return End(yaw);
  }
 }
}
