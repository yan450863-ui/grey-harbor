using UnityEngine;
using UnityEngine.Rendering;
namespace GreyHarbor {
 // Reusable metre-scale workshop equipment. Models have no loot, AI, doors or portals.
 // Cosmetic meshes never generate collision; each physical part below declares its rule.
 public sealed class RepairExteriorKit149 {
  readonly RaidMap map;
  Transform root;
  WorkshopWastelandArt art;
  WorkshopArtResources owned;
  const string Steel="4B625C", Edge="849488", Rust="917151", Dark="2E403A", Rubber="29352F";
  public RepairExteriorKit149(RaidMap m){map=m;}
  void Begin(string asset,Vector3 p,Vector3 envelope,string profile){
   root=new GameObject("RepairKit_"+asset).transform;root.SetParent(map.Root,false);root.position=p;
   var info=root.gameObject.AddComponent<RepairAssetInfo149>();info.Asset=asset;info.Envelope=envelope;info.CollisionProfile=profile;
   owned=root.gameObject.AddComponent<WorkshopArtResources>();art=new WorkshopWastelandArt(map);
  }
  void C(string name,string color,Vector3 p,Vector3 size,bool solidFog=true)=>art.Cube(name,color,p,size,solidFog);
  void T(string name,string color,Vector3 a,Vector3 b,float radius)=>art.Tube(name,color,a,b,radius);
  void Hit(string name,Vector3 p,Vector3 size,ObstacleKind kind,float yaw=0){
   var g=new GameObject("Collision_"+name+"_"+kind);g.transform.SetParent(root,false);g.transform.localPosition=p;g.transform.localRotation=Quaternion.Euler(0,yaw,0);
   g.AddComponent<BoxCollider>().size=size;SceneObstacle.Apply(g,kind);
  }
  void LowArm(string name,Vector3 a,Vector3 b,float width,float height){
   Vector3 d=b-a;float yaw=Mathf.Atan2(d.x,d.z)*Mathf.Rad2Deg;
   Hit(name,(a+b)*.5f,new Vector3(width,height,new Vector2(d.x,d.z).magnitude+width),ObstacleKind.Low,yaw);
  }
  void Cylinder(string name,string color,Vector3 p,Vector3 axis,float radius,float length){
   var g=GameObject.CreatePrimitive(PrimitiveType.Cylinder);g.name=name;g.transform.SetParent(root,false);g.transform.localPosition=p;
   g.transform.localRotation=Quaternion.FromToRotation(Vector3.up,axis.normalized);g.transform.localScale=new Vector3(radius*2,length*.5f,radius*2);
   var col=g.GetComponent<Collider>();col.enabled=false;if(Application.isPlaying)Object.Destroy(col);else Object.DestroyImmediate(col);
   var r=g.GetComponent<Renderer>();r.sharedMaterial=map.Mat(color,true);r.shadowCastingMode=ShadowCastingMode.On;
  }
  void Wheel(Vector3 p,float radius,float width=.12f){
   Cylinder("RubberWheel",Rubber,p,Vector3.right,radius,width);
   Cylinder("WheelHub",Edge,p+Vector3.right*(p.x>=0?width*.51f:-width*.51f),Vector3.right,radius*.45f,.026f);
  }
  void RustPatch(Vector3 p,Vector3 along,Vector3 up,float w,float h)=>art.Patch("MetalRust",Rust,p,along,up,w,h,true);
  void Rivets(Vector3 a,Vector3 b,int count){for(int i=0;i<count;i++)C("FrameBolt",Edge,Vector3.Lerp(a,b,count<2?0:(float)i/(count-1)),new Vector3(.035f,.035f,.035f));}
  Transform End(float yaw){
   art.Bake(root.gameObject,owned);root.rotation=Quaternion.Euler(0,yaw,0);
   foreach(var r in root.GetComponentsInChildren<Renderer>())r.shadowCastingMode=ShadowCastingMode.On;
   return root;
  }
  // 6.8 x 8.2 m. A central 3.4 m roof opening keeps the vehicle/work area visible.
  // Only the four posts and their small concrete feet collide; the ground stays open.
  public Transform ServiceCanopy(Vector3 p,float yaw=0){
   Begin("ServiceCanopy",p,new Vector3(6.8f,3.54f,8.2f),"Four high posts; four low feet; open work bay");
   foreach(float x in new[]{-3.00f,3.00f})foreach(float z in new[]{-3.45f,3.45f}){
    C("ConcreteFoot","939683",new Vector3(x,.09f,z),new Vector3(.54f,.18f,.54f));
    C("SteelColumn",Steel,new Vector3(x,1.72f,z),new Vector3(.22f,3.26f,.22f));
    C("PostBasePlate",Dark,new Vector3(x,.21f,z),new Vector3(.36f,.07f,.36f));
    foreach(float dx in new[]{-.13f,.13f})foreach(float dz in new[]{-.13f,.13f})C("AnchorBolt",Edge,new Vector3(x+dx,.27f,z+dz),new Vector3(.035f,.07f,.035f));
    C("LowerRust",Rust,new Vector3(x,.48f,z-.114f),new Vector3(.16f,.34f,.016f));
    T("DiagonalBrace",Steel,new Vector3(x,2.55f,z),new Vector3(x-Mathf.Sign(x)*.65f,3.27f,z),.045f);
    Hit("CanopyPost",new Vector3(x,1.72f,z),new Vector3(.24f,3.26f,.24f),ObstacleKind.High);
    Hit("CanopyFoot",new Vector3(x,.12f,z),new Vector3(.54f,.24f,.54f),ObstacleKind.Low);
   }
   foreach(float x in new[]{-3.00f,3.00f})C("LongEaveBeam",Steel,new Vector3(x,3.25f,0),new Vector3(.16f,.20f,7.75f));
   foreach(float z in new[]{-3.45f,3.45f})C("CrossBeam",Steel,new Vector3(0,3.25f,z),new Vector3(6.12f,.20f,.16f));
   foreach(int side in new[]{-1,1}){
    C("WeatheredRoofSheet","6E8176",new Vector3(side*2.55f,3.40f,0),new Vector3(1.70f,.07f,8.2f));
    for(int i=0;i<7;i++)C("RoofCorrugation",i%3==0?"91A093":"7B8D7E",new Vector3(side*2.55f-.69f+i*.23f,3.448f,0),new Vector3(.032f,.038f,8.14f));
    T("EaveGutter",Edge,new Vector3(side*3.33f,3.32f,-4.06f),new Vector3(side*3.33f,3.32f,4.06f),.06f);
    T("DrainDownpipe",Steel,new Vector3(side*3.16f,3.28f,3.45f),new Vector3(side*3.16f,.25f,3.45f),.035f);
    RustPatch(new Vector3(side*2.55f,3.442f,-2.7f),Vector3.right,Vector3.forward,.65f,.9f);
    C("DustyWorkLight",Dark,new Vector3(side*2.25f,3.14f,-3.45f),new Vector3(.94f,.13f,.19f));
    C("WorkLightGlass","BCB998",new Vector3(side*2.25f,3.06f,-3.45f),new Vector3(.77f,.022f,.11f));
   }
   return End(yaw);
  }
  // Vehicle can be placed independently at this root's X/Z origin, facing local +Z.
  // The four ground-level swing arms and two columns are separate physical pieces.
  public Transform VehicleLift(Vector3 p,float yaw=0){
   Begin("VehicleLift",p,new Vector3(3.97f,2.78f,3.22f),"Two high columns; low bases and four swing arms");
   foreach(int side in new[]{-1,1}){
    float x=side*1.67f;
    C("LiftAnchorPlate",Dark,new Vector3(x,.09f,0),new Vector3(.60f,.18f,.70f));
    C("LiftColumn","738877",new Vector3(x,1.45f,0),new Vector3(.31f,2.60f,.41f));
    C("ColumnCap",Edge,new Vector3(x,2.73f,0),new Vector3(.39f,.065f,.47f));
    C("LiftTrack",Dark,new Vector3(x-side*.175f,1.48f,0),new Vector3(.04f,2.28f,.18f));
    C("ParkedCarriage",Steel,new Vector3(x-side*.10f,.39f,0),new Vector3(.48f,.35f,.31f));
    for(int j=0;j<5;j++)C("CautionStripe",j%2==0?"A79D6E":"465950",new Vector3(x,.51f+j*.17f,-.216f),new Vector3(.27f,.09f,.014f));
    Rivets(new Vector3(x-.23f,.20f,-.26f),new Vector3(x+.23f,.20f,-.26f),3);
    foreach(int fore in new[]{-1,1}){
     Vector3 a=new Vector3(x-side*.08f,.24f,fore*.10f),b=new Vector3(side*.70f,.24f,fore*1.26f);
     T("SwingArm",Steel,a,b,.085f);C("LiftRubberPad",Rubber,b+Vector3.up*.075f,new Vector3(.28f,.10f,.28f));
     LowArm("LiftArm",a,b,.19f,.24f);
    }
    Hit("LiftPost",new Vector3(x,1.45f,0),new Vector3(.35f,2.60f,.45f),ObstacleKind.High);
    Hit("LiftBase",new Vector3(x,.15f,0),new Vector3(.62f,.30f,.72f),ObstacleKind.Low);
   }
   C("ControlBox",Dark,new Vector3(1.89f,1.20f,0),new Vector3(.13f,.32f,.25f));
   C("ControlButtons","ACA58A",new Vector3(1.964f,1.22f,-.04f),new Vector3(.019f,.045f,.045f));
   C("StopButton","91604F",new Vector3(1.964f,1.1f,.04f),new Vector3(.023f,.07f,.065f));
   T("HydraulicConduit",Dark,new Vector3(-1.67f,.08f,.28f),new Vector3(1.67f,.08f,.28f),.025f);
   RustPatch(new Vector3(-1.67f,.29f,-.213f),Vector3.right,Vector3.up,.25f,.15f);
   return End(yaw);
  }
  public Transform EngineHoist(Vector3 p,float yaw=0){
   Begin("EngineHoist",p,new Vector3(1.64f,2.27f,2.92f),"Low U-shaped base and engine; narrow high mast");
   foreach(int side in new[]{-1,1}){
    float x=side*.60f;
    C("CraneLeg","827859",new Vector3(x,.18f,.30f),new Vector3(.15f,.16f,2.14f));
    Wheel(new Vector3(x,.115f,1.23f),.115f,.085f);Wheel(new Vector3(x,.115f,-.74f),.115f,.085f);
    Hit("HoistLeg",new Vector3(x,.18f,.30f),new Vector3(.22f,.36f,2.25f),ObstacleKind.Low);
   }
   C("RearTie",Steel,new Vector3(0,.19f,-.69f),new Vector3(1.28f,.14f,.18f));
   C("CraneMast","8F8460",new Vector3(0,1.13f,-.68f),new Vector3(.16f,1.95f,.18f));
   T("CraneBoom","9D906A",new Vector3(0,1.79f,-.68f),new Vector3(0,2.19f,1.15f),.077f);
   T("ExtendedBoom",Edge,new Vector3(0,2.08f,.70f),new Vector3(0,2.19f,1.40f),.041f);
   T("HydraulicRam",Dark,new Vector3(0,.47f,-.56f),new Vector3(0,1.62f,.09f),.053f);
   T("ChromePiston","ADB4A3",new Vector3(0,1.20f,-.14f),new Vector3(0,1.89f,.28f),.025f);
   T("PumpHandle",Steel,new Vector3(.19f,.47f,-.64f),new Vector3(.19f,.95f,-1.05f),.019f);
   T("Chain",Dark,new Vector3(0,2.18f,1.40f),new Vector3(0,1.29f,1.40f),.023f);
   for(int i=0;i<7;i++)C("ChainLink",Edge,new Vector3(0,2.10f-i*.105f,1.40f),new Vector3(i%2==0?.065f:.032f,.075f,i%2==0?.032f:.065f));
   T("HangingHook",Edge,new Vector3(0,1.29f,1.40f),new Vector3(.11f,1.17f,1.40f),.024f);
   T("HangingHook",Edge,new Vector3(.11f,1.17f,1.40f),new Vector3(.17f,1.28f,1.40f),.024f);
   C("EngineBlock","4B5750",new Vector3(0,.86f,.83f),new Vector3(.60f,.44f,.57f));
   C("EngineSump",Dark,new Vector3(0,.58f,.83f),new Vector3(.48f,.13f,.46f));
   foreach(float x in new[]{-.29f,.29f})C("RockerCover",Edge,new Vector3(x,1.02f,.83f),new Vector3(.17f,.15f,.52f));
   for(int i=0;i<4;i++)T("EngineExhaust",Rust,new Vector3(.32f,.82f,.64f+i*.12f),new Vector3(.43f,.65f,.64f+i*.12f),.033f);
   T("EngineSling",Dark,new Vector3(-.25f,1.09f,.69f),new Vector3(0,1.27f,1.36f),.018f);
   T("EngineSling",Dark,new Vector3(.25f,1.09f,.97f),new Vector3(0,1.27f,1.36f),.018f);
   Hit("HoistMast",new Vector3(0,1.13f,-.68f),new Vector3(.20f,1.95f,.22f),ObstacleKind.High);
   Hit("SuspendedEngine",new Vector3(0,.83f,.85f),new Vector3(.89f,.68f,.65f),ObstacleKind.Low);
   RustPatch(new Vector3(.60f,.272f,.14f),Vector3.right,Vector3.forward,.12f,.48f);
   return End(yaw);
  }
  public Transform Compressor(Vector3 p,float yaw=0){
   Begin("AirCompressor",p,new Vector3(1.02f,1.22f,1.96f),"Low machinery envelope; bullets and sight pass over");
   Cylinder("PressureTank","708779",new Vector3(0,.49f,0),Vector3.forward,.32f,1.38f);
   foreach(float z in new[]{-.43f,.43f})Cylinder("TankBand",Edge,new Vector3(0,.49f,z),Vector3.forward,.333f,.055f);
   foreach(float x in new[]{-.32f,.32f})Wheel(new Vector3(x,.17f,.47f),.17f,.10f);
   C("TankFoot",Steel,new Vector3(0,.085f,-.49f),new Vector3(.56f,.17f,.18f));
   C("MotorMount",Steel,new Vector3(0,.88f,.18f),new Vector3(.65f,.07f,.60f));
   Cylinder("ElectricMotor",Dark,new Vector3(-.16f,1.04f,.20f),Vector3.forward,.16f,.48f);
   C("PumpHead",Edge,new Vector3(.19f,1.05f,.20f),new Vector3(.25f,.26f,.38f));
   for(int i=0;i<5;i++)C("CoolingFin","99A190",new Vector3(.19f,.967f+i*.04f,.20f),new Vector3(.29f,.014f,.40f));
   T("CopperAirline",Rust,new Vector3(.19f,1.05f,-.02f),new Vector3(.19f,.57f,-.69f),.025f);
   C("PressureSwitch",Dark,new Vector3(0,.90f,-.51f),new Vector3(.20f,.15f,.15f));
   C("PressureGauge",Edge,new Vector3(0,.99f,-.61f),new Vector3(.16f,.16f,.045f));
   C("GaugeFace","B7B39B",new Vector3(0,.99f,-.639f),new Vector3(.13f,.13f,.014f));
   T("GaugeNeedle",Dark,new Vector3(0,.99f,-.651f),new Vector3(.038f,1.026f,-.651f),.007f);
   T("AirOutlet",Edge,new Vector3(0,.49f,-.70f),new Vector3(0,.49f,-.84f),.023f);
   for(int i=0;i<18;i++){float a=i*Mathf.PI/9,b=(i+1)*Mathf.PI/9;T("CoiledAirHose",Dark,new Vector3(.43f,.55f+Mathf.Sin(a)*.24f,Mathf.Cos(a)*.28f),new Vector3(.43f,.55f+Mathf.Sin(b)*.24f,Mathf.Cos(b)*.28f),.016f);}
   T("PushHandle",Steel,new Vector3(-.23f,.63f,.63f),new Vector3(-.23f,.89f,.91f),.025f);
   T("PushHandle",Steel,new Vector3(.23f,.63f,.63f),new Vector3(.23f,.89f,.91f),.025f);
   T("HandleGrip",Rubber,new Vector3(-.23f,.89f,.91f),new Vector3(.23f,.89f,.91f),.032f);
   Hit("Compressor",new Vector3(0,.60f,.025f),new Vector3(1.02f,1.20f,1.90f),ObstacleKind.Low);
   RustPatch(new Vector3(0,.51f,-.711f),Vector3.right,Vector3.up,.22f,.14f);
   return End(yaw);
  }
  void Tyre(Vector3 p){
   for(int side=-1;side<=1;side+=2)for(int i=0;i<18;i++){
    float a=i*Mathf.PI/9,b=(i+1)*Mathf.PI/9;
    Vector3 q=p+new Vector3(Mathf.Cos(a)*.275f,Mathf.Sin(a)*.275f,side*.067f),r=p+new Vector3(Mathf.Cos(b)*.275f,Mathf.Sin(b)*.275f,side*.067f);
    T("StoredTyre",Rubber,q,r,.07f);
    if(side==-1&&i%3==0)T("TyreTread","3D4940",q-Vector3.forward*.039f,r-Vector3.forward*.039f,.016f);
   }
  }
  public Transform TyreRack(Vector3 p,float yaw=0){
   Begin("TyreRack",p,new Vector3(3.14f,2.05f,.86f),"High stocked rack envelope");
   foreach(float x in new[]{-1.49f,1.49f})foreach(float z in new[]{-.34f,.34f}){
    C("RackUpright",Steel,new Vector3(x,1.04f,z),new Vector3(.065f,1.97f,.065f));
    C("RackFoot",Edge,new Vector3(x,.04f,z),new Vector3(.17f,.08f,.15f));
   }
   foreach(float y in new[]{.23f,1.09f,1.91f})foreach(float z in new[]{-.32f,.32f})C("TyreShelfRail","6E8272",new Vector3(0,y,z),new Vector3(3.03f,.08f,.08f));
   foreach(float x in new[]{-1.01f,0,1.01f})foreach(float y in new[]{.57f,1.43f})Tyre(new Vector3(x,y,0));
   T("RackCrossBrace",Edge,new Vector3(-1.48f,.25f,.355f),new Vector3(1.48f,1.89f,.355f),.020f);
   T("RackCrossBrace",Edge,new Vector3(1.48f,.25f,.355f),new Vector3(-1.48f,1.89f,.355f),.020f);
   C("FadedRackTag","A29B7B",new Vector3(.96f,1.97f,-.346f),new Vector3(.33f,.095f,.019f));
   RustPatch(new Vector3(-1.49f,.41f,-.377f),Vector3.right,Vector3.up,.08f,.39f);
   Hit("TyreRack",new Vector3(0,1.025f,0),new Vector3(3.14f,2.05f,.86f),ObstacleKind.High);
   return End(yaw);
  }
  public Transform WeldingCart(Vector3 p,float yaw=0){
   Begin("WeldingCart",p,new Vector3(1.15f,1.58f,1.11f),"Low wheeled cart; narrow high gas cylinder");
   C("CartDeck",Steel,new Vector3(0,.25f,0),new Vector3(.98f,.10f,.85f));
   foreach(float x in new[]{-.43f,.43f})foreach(float z in new[]{-.33f,.33f})Wheel(new Vector3(x,.14f,z),.14f,.085f);
   C("WelderHousing","84907B",new Vector3(-.15f,.67f,-.07f),new Vector3(.50f,.72f,.59f));
   C("WelderPanel",Dark,new Vector3(-.15f,.91f,-.375f),new Vector3(.39f,.13f,.022f));
   C("Dial",Edge,new Vector3(-.28f,.91f,-.395f),new Vector3(.060f,.060f,.025f));
   C("Switch","A79665",new Vector3(-.055f,.91f,-.395f),new Vector3(.035f,.070f,.022f));
   for(int i=0;i<5;i++)C("CoolingSlat",Dark,new Vector3(-.15f,.70f-i*.055f,-.378f),new Vector3(.36f,.022f,.016f));
   Cylinder("ShieldingGasBottle","647F72",new Vector3(.34f,.85f,.14f),Vector3.up,.15f,1.22f);
   C("BottleShoulder",Edge,new Vector3(.34f,1.47f,.14f),new Vector3(.15f,.10f,.15f));
   T("BottleValve",Rust,new Vector3(.34f,1.45f,.14f),new Vector3(.34f,1.53f,.14f),.025f);
   T("CartHandle",Steel,new Vector3(-.42f,.25f,.39f),new Vector3(-.42f,1.10f,.39f),.027f);
   T("CartHandle",Steel,new Vector3(.13f,.25f,.39f),new Vector3(.13f,1.10f,.39f),.027f);
   T("Grip",Rubber,new Vector3(-.42f,1.10f,.39f),new Vector3(.13f,1.10f,.39f),.032f);
   T("GasTube",Dark,new Vector3(.34f,1.49f,.14f),new Vector3(-.12f,1.08f,.20f),.017f);
   for(int i=0;i<18;i++){float a=i*Mathf.PI/9,b=(i+1)*Mathf.PI/9;T("WeldingLead",Dark,new Vector3(-.437f,.67f+Mathf.Sin(a)*.23f,-.07f+Mathf.Cos(a)*.20f),new Vector3(-.437f,.67f+Mathf.Sin(b)*.23f,-.07f+Mathf.Cos(b)*.20f),.016f);}
   C("TorchGrip",Dark,new Vector3(-.17f,1.056f,-.10f),new Vector3(.18f,.035f,.05f));
   T("TorchTip",Edge,new Vector3(-.07f,1.07f,-.10f),new Vector3(.055f,1.08f,-.10f),.014f);
   RustPatch(new Vector3(-.15f,.34f,-.379f),Vector3.right,Vector3.up,.31f,.13f);
   Hit("WelderCart",new Vector3(-.095f,.55f,-.015f),new Vector3(.92f,1.10f,1.00f),ObstacleKind.Low);
   Hit("GasBottle",new Vector3(.34f,.85f,.14f),new Vector3(.31f,1.24f,.31f),ObstacleKind.High);
   return End(yaw);
  }
  public Transform AxleStand(Vector3 p,float yaw=0){
   Begin("AxleStand",p,new Vector3(.67f,.73f,.67f),"Low tripod stand");
   foreach(float x in new[]{-.27f,.27f})foreach(float z in new[]{-.27f,.27f})T("StandLeg","927A56",new Vector3(x,.025f,z),new Vector3(0,.42f,0),.031f);
   C("JackColumn",Steel,new Vector3(0,.43f,0),new Vector3(.11f,.49f,.11f));
   for(int i=0;i<5;i++)C("RatchetTooth",Edge,new Vector3(0,.34f+i*.055f,-.066f),new Vector3(.075f,.025f,.022f));
   C("AxleSaddle",Dark,new Vector3(0,.67f,0),new Vector3(.29f,.05f,.14f));
   foreach(float x in new[]{-.13f,.13f})C("SaddleLip",Edge,new Vector3(x,.70f,0),new Vector3(.04f,.055f,.14f));
   Hit("AxleStand",new Vector3(0,.36f,0),new Vector3(.67f,.72f,.67f),ObstacleKind.Low);
   return End(yaw);
  }
 }
}
