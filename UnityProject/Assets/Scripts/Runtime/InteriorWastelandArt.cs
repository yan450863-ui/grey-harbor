using UnityEngine;
using System.Collections.Generic;
namespace GreyHarbor {
 // Surface dressing follows existing walls and furnishings. No gameplay colliders are added.
 public sealed class InteriorWastelandArt {
  readonly RaidMap map;readonly WorkshopWastelandArt art;readonly System.Random rng=new System.Random(116029);
  bool House=>map.InteriorId=="mainhouse";
  float R(float a,float b)=>a+(float)rng.NextDouble()*(b-a);
  public InteriorWastelandArt(RaidMap m){map=m;art=new WorkshopWastelandArt(m);}
  public void Build(){
   var root=new GameObject("InteriorWastelandArt_116");root.transform.SetParent(map.Root,false);var lifetime=root.AddComponent<WorkshopArtResources>();
   var skins=new Dictionary<Material,Material>();
   foreach(var renderer in map.Root.GetComponentsInChildren<MeshRenderer>()){
    var original=renderer.sharedMaterial;if(original==null||!original.HasProperty("_Detail"))continue;
    if(!skins.TryGetValue(original,out var skin)){skin=new Material(original);skin.name="InteriorWeathered_"+original.name;skin.SetFloat("_Detail",.07f);skins.Add(original,skin);lifetime.Materials.Add(skin);}renderer.sharedMaterial=skin;
    if(renderer.name=="InteriorWall"){var wall=new Material(skin);wall.color=PixelArt.Hex("8E927F");renderer.sharedMaterial=wall;lifetime.Materials.Add(wall);DressWall(renderer.bounds);}
   }
   FurnitureWear();if(House)HouseDetails();else BoothDetails();art.Bake(root,lifetime);
  }
  void DressWall(Bounds b){
   bool alongX=b.size.x>b.size.z;Vector3 along=alongX?Vector3.right:Vector3.forward;float length=alongX?b.size.x:b.size.z;
   foreach(float sign in new[]{-1f,1f}){
    Vector3 normal=(alongX?Vector3.forward:Vector3.right)*sign;
    Vector3 p=new Vector3(b.center.x,0,b.center.z)+normal*((alongX?b.extents.z:b.extents.x)+.016f);
    float maxX=House?11.99f:3.69f,maxZ=House?9.99f:3.29f;
    if(Mathf.Abs(p.x)>maxX||Mathf.Abs(p.z)>maxZ)continue;
    string paint="5F776C";
    if(House&&p.x>2.05f&&Mathf.Abs(p.z)<3.3f)paint="74877D";
    if(House&&p.z<-3.3f)paint="6C7969";
    art.WallSkin(p,along,normal,length,paint);
   }
  }
  void Window(Vector3 p,Vector3 along,Vector3 normal,float w){
   art.Panel("WindowFrame","344B43",p,along,Vector3.up,w,.78f);
   art.Panel("DustyOpaqueGlass","718476",p+normal*.016f,along,Vector3.up,w-.12f,.64f);
   for(int i=-1;i<=1;i++)art.Panel("WindowMullion","425B4D",p+along*i*(w-.16f)/2+normal*.025f,along,Vector3.up,.045f,.72f);
   art.Panel("WindowCrossbar","425B4D",p+normal*.027f,along,Vector3.up,w,.036f);
   art.Stroke("GlassFracture","A1A38A",p+normal*.03f+along*.16f+Vector3.up*.29f,p+normal*.03f-along*.21f-Vector3.up*.2f,.014f,normal,true);
   art.Tube("WindowSill","667160",p-along*w*.52f-Vector3.up*.41f+normal*.045f,p+along*w*.52f-Vector3.up*.41f+normal*.045f,.04f);
  }
  void Fuse(Vector3 p,Vector3 along,Vector3 normal){
   art.Panel("ElectricalPanel","425B51",p,along,Vector3.up,.47f,.63f);
   art.Panel("ElectricalDoor","76816B",p+normal*.02f,along,Vector3.up,.40f,.55f);
   art.Panel("FadedWarning","AEA079",p+normal*.026f+Vector3.up*.08f,along,Vector3.up,.14f,.14f);
   for(int i=0;i<3;i++)art.Panel("PanelVent","425B51",p+normal*.029f-Vector3.up*(.14f+i*.04f),along,Vector3.up,.23f,.012f);
   art.Tube("ElectricalConduit","737663",p+Vector3.up*.32f+normal*.055f,new Vector3(p.x,2.06f,p.z)+normal*.055f,.023f);
  }
  void Paper(float x,float z,float y=.044f){
   art.Floor("LoosePaper","AAA58D",x,z,.22f,.29f,y);
   for(int i=0;i<4;i++)art.Floor("FadedPaperPrint","747B69",x,z-.075f+i*.045f,.14f,.009f,y+.002f);
  }
  void EdgeDebris(Rect r,int count){
   for(int i=0;i<count;i++){
    float x=i%2==0?r.xMin+.17f:r.xMax-.17f,z=R(r.yMin+.25f,r.yMax-.25f);
    art.Cube("WallFootDebris",i%3==0?"99947C":"767B68",new Vector3(x,.046f,z),new Vector3(R(.045f,.12f),R(.025f,.055f),R(.045f,.14f)),false);
   }
  }
  void FurnitureWear(){
   foreach(var r in map.Root.GetComponentsInChildren<MeshRenderer>()){
    Bounds b=r.bounds;
    if(r.name=="PinnedNote"||r.name=="PinnedPaper"){
     float z=b.center.z+(b.center.z>0?-1:1)*(b.extents.z+.006f);
     for(int i=0;i<4;i++)art.Panel("NoticePrint","78765E",new Vector3(b.center.x,b.center.y-.12f+i*.065f,z),Vector3.right,Vector3.up,b.size.x*.68f,.014f);
    }
    if(r.name=="RestTable"||r.name=="WorkshopBench")for(int i=0;i<6;i++)art.Floor("TableWear","756C51",R(b.min.x+.1f,b.max.x-.1f),R(b.min.z+.1f,b.max.z-.1f),R(.10f,.22f),.017f,b.max.y+.003f);
   }
  }
  void BoothDetails(){
   Window(new Vector3(-3.585f,1.7f,1.95f),Vector3.forward,Vector3.right,1.48f);
   Window(new Vector3(-2.65f,1.72f,-3.175f),Vector3.right,Vector3.forward,1.0f);
   Fuse(new Vector3(.55f,1.3f,-3.178f),Vector3.right,Vector3.forward);
   art.StripLight(new Vector3(-1.1f,2.02f,3.15f),Vector3.right,Vector3.back);
   art.StripLight(new Vector3(3.55f,1.99f,.75f),Vector3.forward,Vector3.left);
   art.BrickScar(new Vector3(3.575f,.65f,.15f),Vector3.forward,Vector3.left);
   art.Patch("StorageDamp","616D58",new Vector3(2.45f,.36f,3.174f),Vector3.right,Vector3.up,1.1f,.48f,true);
   EdgeDebris(new Rect(-3.58f,-3.15f,4.43f,6.3f),9);EdgeDebris(new Rect(1.13f,-3.15f,2.44f,6.3f),6);
   Paper(-.7f,2.48f);Paper(-.35f,2.62f);Paper(-2.95f,2.95f);
   // Radio is fixed to the existing suitcase desk, not a new searchable object.
   art.Cube("DeadRadio","3C5146",new Vector3(-3.06f,.909f,2.58f),new Vector3(.25f,.23f,.16f));
   art.Panel("RadioDial","8C9176",new Vector3(-3.06f,.934f,2.49f),Vector3.right,Vector3.up,.14f,.06f);
   art.Tube("RadioAerial","657766",new Vector3(-3.13f,1.02f,2.6f),new Vector3(-3.16f,1.34f,2.6f),.009f);
   art.Patch("BootDirt","626B56",new Vector3(-1.25f,.042f,-2.62f),Vector3.right,Vector3.forward,1.0f,.4f,false);
  }
  void HouseDetails(){
   // Discontinuous partitions are dressed separately, so coatings never span doorways.
   foreach(float z in new[]{-8.45f,-3.3f,3.3f,8.5f}){
    art.StripLight(new Vector3(-1.86f,2.01f,z),Vector3.forward,Vector3.right);
   }
   Fuse(new Vector3(1.875f,1.35f,-8.7f),Vector3.forward,Vector3.left);
   for(int room=0;room<6;room++){
    Rect r=RaidMap.MainHouseRooms[room];float sign=room%2==0?-1:1,zc=r.center.y;
    Vector3 normal=sign<0?Vector3.right:Vector3.left;
    Window(new Vector3(sign*11.875f,1.75f,zc+1.65f),Vector3.forward,normal,1.5f);
    art.StripLight(new Vector3(sign*11.83f,2.02f,zc-1.35f),Vector3.forward,normal);
    EdgeDebris(new Rect(r.xMin+.22f,r.yMin+.20f,r.width-.44f,r.height-.40f),12);
    if(room!=3)art.BrickScar(new Vector3(sign*11.875f,.65f,zc+.5f),Vector3.forward,normal);
   }
   Office();Rest();Tools();Clinic();Archive();Storage();
  }
  void Office(){
   Paper(-8.7f,-9.15f);Paper(-8.3f,-9.3f);Paper(-9.2f,-9.04f);
   art.Panel("OfficePinboardFrame","485C4E",new Vector3(-4,1.43f,-9.873f),Vector3.right,Vector3.up,1.6f,.8f);
   art.Panel("FadedEvacuationPlan","969D84",new Vector3(-4,1.43f,-9.858f),Vector3.right,Vector3.up,1.42f,.63f);
   foreach(float x in new[]{-4.48f,-4.06f,-3.6f})art.Panel("PlanBlocks","697D69",new Vector3(x,1.45f,-9.849f),Vector3.right,Vector3.up,.25f,.28f);
   art.Stroke("PlanRoute","B0A67C",new Vector3(-4.55f,1.2f,-9.84f),new Vector3(-3.45f,1.2f,-9.84f),.018f,Vector3.forward,true);
  }
  void Rest(){
   art.Floor("SofaThrow","8C8870",5.12f,-8.8f,.58f,.77f,.564f);
   for(int i=0;i<5;i++)art.Floor("FrayedThrow","626C59",4.91f+i*.11f,-8.44f,.045f,.09f,.568f);
   art.Panel("SofaTornFabric","4C5D4E",new Vector3(5.25f,.3f,-8.342f),Vector3.right,Vector3.up,.28f,.10f);
   art.Cube("EmptyMug","9B9C83",new Vector3(6.18f,.72f,-5.1f),new Vector3(.13f,.16f,.13f),false);
   art.Floor("TableMagazine","A19A7F",5.65f,-5.05f,.32f,.25f,.647f);
   art.Panel("RestCoatRail","506451",new Vector3(8.45f,1.6f,-9.875f),Vector3.right,Vector3.up,1.2f,.07f);
   for(int i=0;i<4;i++)art.Tube("EmptyCoatHooks","938B70",new Vector3(8.05f+i*.26f,1.6f,-9.86f),new Vector3(8.05f+i*.26f,1.53f,-9.77f),.016f);
  }
  void Tools(){
   art.Panel("ToolPegboard","6B7058",new Vector3(-5.6f,1.48f,-3.175f),Vector3.right,Vector3.up,2.0f,.82f);
   for(int i=0;i<5;i++){
    float x=-6.35f+i*.36f;art.Stroke("HangingTools","3D5046",new Vector3(x,1.7f,-3.16f),new Vector3(x,1.18f,-3.16f),.04f,Vector3.forward,true);
    art.Panel("ToolHeads","89917A",new Vector3(x,1.69f,-3.147f),Vector3.right,Vector3.up,.16f,.085f);
   }
   art.Patch("BenchOil","424D3F",new Vector3(-5.7f,.04f,-2.6f),Vector3.right,Vector3.forward,1.9f,.65f,false);
   Fuse(new Vector3(-11.872f,1.35f,-1.9f),Vector3.forward,Vector3.right);
  }
  void Clinic(){
   // Bed fittings stay within the existing low furniture's footprint.
   art.Floor("ClinicBlanket","7E9485",5.63f,-1.8f,1.05f,.87f,.724f);
   for(int i=0;i<3;i++)art.Floor("BlanketFold","6A8174",5.27f+i*.29f,-1.8f,.025f,.80f,.728f);
   foreach(float z in new[]{-2.22f,-1.38f})art.Tube("BedRail","859584",new Vector3(5,.8f,z),new Vector3(7,.8f,z),.025f);
   art.Panel("ClinicSign","A9B09A",new Vector3(9.4f,1.6f,3.175f),Vector3.right,Vector3.up,.62f,.62f);
   art.Panel("FadedMedicalCross","886B5B",new Vector3(9.4f,1.6f,3.16f),Vector3.right,Vector3.up,.12f,.40f);
   art.Panel("FadedMedicalCross","886B5B",new Vector3(9.4f,1.6f,3.158f),Vector3.right,Vector3.up,.40f,.12f);
   Paper(8.55f,2.85f);Paper(8.83f,2.79f);
  }
  void Archive(){
   foreach(float z in new[]{4.9f,5.6f,6.3f}){Paper(-10.1f,z);Paper(-10.35f,z+.1f);}
   art.Patch("ArchiveDamp","616D58",new Vector3(-9.3f,.46f,9.875f),Vector3.right,Vector3.up,1.9f,.65f,true);
   for(int i=0;i<5;i++)art.Stroke("ArchiveLeak","5A6755",new Vector3(-9.8f+i*.23f,1.6f,9.869f),new Vector3(-9.8f+i*.23f,.6f,9.869f),.035f,Vector3.back,true);
   art.Panel("RecordsNotice","97977C",new Vector3(-4,1.5f,9.875f),Vector3.right,Vector3.up,.6f,.8f);
   for(int i=0;i<6;i++)art.Panel("RecordsNoticePrint","686E5B",new Vector3(-4,1.28f+i*.08f,9.863f),Vector3.right,Vector3.up,.4f,.02f);
  }
  void Storage(){
   // One restrained dry infestation, no new light, enemy, hazard or loot source.
   Vector3 p=new Vector3(11.873f,1.68f,8.1f),along=Vector3.forward,normal=Vector3.left;
   art.Panel("StorageVent","34473D",p,along,Vector3.up,.93f,.58f);
   for(int i=0;i<6;i++)if(i!=3)art.Panel("StorageVentBars","75816B",p+normal*.016f+along*((i-2.5f)*.14f),along,Vector3.up,.033f,.49f);
   art.Patch("DryAlienScar","55543F",p+normal*.029f-Vector3.up*.37f,along,Vector3.up,1.03f,.49f,true);
   for(int i=0;i<5;i++){
    Vector3 a=p+normal*.038f-Vector3.up*.18f;
    for(int j=0;j<4;j++){Vector3 b=a+new Vector3(-.008f,-.18f,(i-2)*.065f);art.Tube("DryAlienTendrils","454C38",a,b,.019f-j*.003f);a=b;}
   }
   for(int i=0;i<10;i++)if(i%3!=0)art.Floor("StorageWornPaint","9A926C",10.5f,7.25f+i*.2f,.06f,.12f,.04f);
   art.Patch("DryAlienResidue","555B43",new Vector3(11.55f,.047f,8.15f),Vector3.right,Vector3.forward,.42f,.83f,false);
  }
 }
}
