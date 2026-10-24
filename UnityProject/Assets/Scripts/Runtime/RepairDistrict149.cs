using UnityEngine;
using System.Collections.Generic;
namespace GreyHarbor { public sealed partial class RaidMap {
 public static readonly Rect RepairBounds149=new Rect(-78.4f,-25,46.65f,83);
 public static readonly Vector3 RepairPreviewSpawn149=PortPoint(35,85);
 public readonly List<Vector3> RepairWalkPoints149=new List<Vector3>();
 public Transform RepairDistrictRoot149;
 Rect RepairRect149(float x,float z,float w,float d){var p=PortPoint(x,z);return new Rect(p.x,p.z,w,d);}
 void RepairParent149(Transform t){t.SetParent(RepairDistrictRoot149,true);}
 void RepairPaint149(WorkshopWastelandArt art){
  var g=new GameObject("Repair_GroundPaint149");g.transform.SetParent(Root,false);art.Bake(g,g.AddComponent<WorkshopArtResources>());
  foreach(var f in g.GetComponentsInChildren<MeshFilter>()){var v=f.sharedMesh.vertices;for(int i=0;i<v.Length;i++)v[i]=f.transform.TransformPoint(v[i]);RegisterGround137(new List<Vector3>(v),new List<int>(f.sharedMesh.triangles),f.GetComponent<Renderer>().sharedMaterial.color);f.GetComponent<Renderer>().enabled=false;}
 }
 void BuildRepairExterior149(){
  RepairDistrictRoot149=new GameObject("WestRepairExterior149").transform;RepairDistrictRoot149.SetParent(Root,false);
  DressRepairBuildings149();
  var surface=new GroundSurfaceArt(this,149029);
  // Finish to the existing yard wall and road edge; no raw strip between districts.
  surface.Region(RepairBounds149,GroundFinish.Soil,.090f);
  surface.Region(RepairRect149(10,59,36.65f,68),GroundFinish.PatchedConcrete,.112f);
  surface.Region(RepairRect149(2.7f,57,6.6f,73),GroundFinish.Asphalt,.142f);
  surface.Region(RepairRect149(.6f,51,46.65f,6),GroundFinish.Asphalt,.145f);
  surface.Region(RepairRect149(25.7f,48,4.6f,10.7f),GroundFinish.PatchedConcrete,.146f);
  surface.Region(RepairRect149(31.5f,61,15.1f,20.8f),GroundFinish.Concrete,.128f);
  surface.Region(RepairRect149(10,123,29,7),GroundFinish.Gravel,.128f);
  surface.Region(RepairRect149(40.8f,87.4f,6.45f,35.6f),GroundFinish.Concrete,.128f);
  surface.Region(RepairRect149(10,57,4,28.6f),GroundFinish.Gravel,.128f);
  surface.Region(RepairRect149(10,86,3,38),GroundFinish.Concrete,.128f);
  surface.Bake("Repair_GroundMaterials149");
  var paint=new WorkshopWastelandArt(this);
  for(float z=61;z<128;z+=6)paint.Floor("RepairWornRoadDash","AFAD87",PortPoint(6,z).x,PortPoint(6,z).z,.12f,2.1f,.170f);
  for(int i=0;i<5;i++)paint.Floor("RepairRoadCrosswalk","BAB69A",PortPoint(25.5f+i*1.25f,54).x,PortPoint(28,54).z,.6f,3,.171f);
  foreach(float x in new[]{34.5f,38.6f,41.15f,45.3f})paint.Floor("RepairBayPaint","B2A172",PortPoint(x,71).x,PortPoint(x,71).z,.12f,12,.173f);
  foreach(float z in new[]{64.6f,77.4f}){var p=PortPoint(39.9f,z);paint.Floor("RepairBayPaint","B2A172",p.x,p.z,11.1f,.12f,.173f);}
  // Flush oil stains and tyre scuffs are baked with the floor, never loose depth layers.
  foreach(var p in new[]{PortPoint(36.5f,67.8f),PortPoint(43.2f,73.5f),PortPoint(34,80)})paint.Patch("RepairOilStain","4E554B",p+Vector3.up*.176f,Vector3.right,Vector3.forward,2.0f,1.6f,false);
  for(int side=-1;side<=1;side+=2)for(int i=0;i<7;i++){var p=PortPoint(36.5f+side*.69f,59.5f+i*.55f);paint.Floor("RepairTyreScuff","62695B",p.x,p.z,.11f,.28f,.174f);}
  RepairPaint149(paint);
  var kit=new RepairExteriorKit149(this);
  RepairParent149(kit.VehicleLift(PortPoint(36.5f,68)));
  RepairParent149(kit.VehicleLift(PortPoint(43.2f,73)));
  RepairParent149(kit.ServiceCanopy(PortPoint(43.2f,73)));
  RepairParent149(kit.EngineHoist(PortPoint(34,80),20));
  RepairParent149(kit.Compressor(PortPoint(33,73),90));
  RepairParent149(kit.TyreRack(PortPoint(18,61),0));
  RepairParent149(kit.TyreRack(PortPoint(34,126.5f),0));
  RepairParent149(kit.WeldingCart(PortPoint(33,77.2f),90));
  RepairParent149(kit.AxleStand(PortPoint(34.5f,63.4f)));RepairParent149(kit.AxleStand(PortPoint(35.4f,63.4f)));
  var parking=new GateParkingKit(this);
  parking.Vehicle(PortPoint(36.5f,68),1);RepairParent149(Root.GetChild(Root.childCount-1));
  parking.Vehicle(PortPoint(43.2f,73),0);RepairParent149(Root.GetChild(Root.childCount-1));
  parking.Vehicle(PortPoint(18,126),0,90);RepairParent149(Root.GetChild(Root.childCount-1));
  // West verge and rear service strip use vegetation instead of empty whitebox ground.
  var living=new YardLivingKit(this);
  foreach(var p in new[]{PortPoint(1.8f,64),PortPoint(1.8f,82),PortPoint(1.8f,106),PortPoint(42.7f,126.7f)})RepairParent149(living.Tree(p,0,149));
  foreach(var p in new[]{new Vector4(1.5f,72,1.3f,14),new Vector4(1.5f,96,1.3f,17),new Vector4(12.5f,60,3.5f,3.2f),new Vector4(44.8f,126.9f,2.1f,4.5f),new Vector4(37.5f,128,5,1.6f)})RepairParent149(living.Grass(PortPoint(p.x,p.y),p.z,p.w,true,149));
  RepairParent149(living.WaterTank(PortPoint(26.8f,127.4f)));
  var port=new PortEnvironmentKit(this);
  port.Bench(PortPoint(18.2f,64.5f));RepairParent149(Root.GetChild(Root.childCount-1));
  port.Bench(PortPoint(31.7f,124.7f));RepairParent149(Root.GetChild(Root.childCount-1));
  port.Lamp(PortPoint(11.3f,61.5f),0);RepairParent149(Root.GetChild(Root.childCount-1));
  port.Lamp(PortPoint(45.8f,84.5f),180);RepairParent149(Root.GetChild(Root.childCount-1));
  port.Lamp(PortPoint(11.1f,122.5f),0);RepairParent149(Root.GetChild(Root.childCount-1));
  // Flush drainage belongs to the lane edge; all grates are cosmetic.
  var detail=new WorkshopWastelandArt(this);
  foreach(float x in new[]{10.1f,46.9f})for(float z=63;z<122;z+=3){var p=PortPoint(x,z);detail.Cube("RepairDrainEdge","6D7566",p+Vector3.up*.08f,new Vector3(.30f,.06f,.8f),false);for(int j=0;j<6;j++)detail.Cube("RepairDrainSlots","384D42",p+new Vector3(0,.12f,-.3f+j*.12f),new Vector3(.23f,.035f,.048f),false);}
  // Kerb fragments stop outside junctions. They do not create a second blocked road.
  foreach(float z in new[]{69f,80f,95f,109f,117f})detail.Cube("RepairCurb","929B84",PortPoint(9.6f,z)+Vector3.up*.10f,new Vector3(.22f,.16f,3),false);
  detail.Bake(RepairDistrictRoot149.gameObject,RepairDistrictRoot149.gameObject.AddComponent<WorkshopArtResources>());
  RepairWalkPoints149.AddRange(new[]{PortPoint(28,51),PortPoint(28,60),PortPoint(36,60),PortPoint(32.8f,67),PortPoint(38.9f,70),PortPoint(39.6f,80),PortPoint(35,85),PortPoint(44,87),PortPoint(44,105),PortPoint(44,123),PortPoint(26,123),PortPoint(10.5f,124),PortPoint(10.5f,95),PortPoint(10.5f,62),PortPoint(43,58),PortPoint(46,95)});
 }
 void DressRepairBuildings149(){
  var shell=Root.Find("维修主楼");shell.GetComponent<Renderer>().sharedMaterial=Mat("85897A",true);
  var annex=Root.Find("维修附属棚");annex.GetComponent<Renderer>().sharedMaterial=Mat("637467",true);
  // Replace early window bands; retain only the original solid footprints and floor metadata.
  var remove=new List<GameObject>();foreach(Transform c in Root)if(c.name.StartsWith("维修主楼_"))remove.Add(c.gameObject);
  foreach(var c in remove){c.SetActive(false);if(Application.isPlaying)Object.Destroy(c);else Object.DestroyImmediate(c);}
  var art=new WorkshopWastelandArt(this);var center=PortPoint(26.5f,104);float left=center.x-13.5f,right=center.x+13.5f,front=center.z-16,rear=center.z+16;
  art.Cube("RepairMainPlinth","667567",new Vector3(center.x,.68f,front-.065f),new Vector3(27,1.36f,.13f));
  for(int floor=0;floor<4;floor++){
   float h=floor*3.6f;
   art.Cube("RepairConcreteBand","ABB09A",new Vector3(center.x,h+3.4f,front-.08f),new Vector3(27.12f,.20f,.19f));
   art.Cube("RepairConcreteBand","9CA58E",new Vector3(right+.06f,h+3.4f,center.z),new Vector3(.16f,.20f,32.1f));
   if(floor>0)for(int n=0;n<6;n++)RepairWindow149(art,new Vector3(left+2.5f+n*4.4f,h+1.7f,front-.105f),Vector3.right,Vector3.back,2.65f,1.3f);
   for(int n=0;n<6;n++){float z=front+3+n*5.2f;RepairWindow149(art,new Vector3(right+.10f,h+1.85f,z),Vector3.forward,Vector3.right,3.1f,1.45f);RepairWindow149(art,new Vector3(left-.10f,h+1.85f,z),Vector3.forward,Vector3.left,2.65f,1.4f);}
  }
  for(float x=left+.15f;x<right;x+=4.45f)art.Cube("RepairPier","9FA68F",new Vector3(x,7.2f,front-.12f),new Vector3(.32f,14.4f,.27f));
  foreach(float x in new[]{19f,26.5f,35f})RepairShutter149(art,PortPoint(x,88),4.7f,3.05f);
  foreach(var p in new[]{new Vector3(left+.24f,0,front-.29f),new Vector3(right-.26f,0,front-.29f),new Vector3(right+.22f,0,rear-.20f)})art.Tube("RepairDownpipe","526B59",p+Vector3.up*.18f,p+Vector3.up*14.8f,.065f);
  art.Cube("RepairFlatRoof","8C9482",new Vector3(center.x,14.5f,center.z),new Vector3(27.6f,.20f,32.6f));
  foreach(float z in new[]{front-.14f,rear+.14f})art.Cube("RepairParapet","ABB29B",new Vector3(center.x,14.8f,z),new Vector3(27.7f,.50f,.30f));
  foreach(float x in new[]{left-.14f,right+.14f})art.Cube("RepairParapet","9AA68F",new Vector3(x,14.8f,center.z),new Vector3(.30f,.50f,32.4f));
  foreach(float z in new[]{96f,111f}){var p=PortPoint(24,z);art.Cube("RepairRoofVent","62786A",p+Vector3.up*15.0f,new Vector3(3.7f,.60f,2.0f));for(int i=0;i<7;i++)art.Cube("RepairRoofVentSlots","3D5A4A",p+new Vector3(0,15.325f,-.70f+i*.23f),new Vector3(3.3f,.04f,.11f));}
  for(int i=0;i<23;i++){float x=left+1+(i%7)*3.7f,y=4.3f+(i%3)*3.6f;art.Patch("RepairPlasterLoss",i%2==0?"727D66":"B0A889",new Vector3(x,y,front-.19f),Vector3.right,Vector3.up,.8f,1.1f,true);}
  var ap=PortPoint(22,75.5f);float af=ap.z-9.5f;
  art.Cube("RepairAnnexBase","536B57",new Vector3(ap.x,.50f,af-.09f),new Vector3(18,1.0f,.18f));
  for(float x=13.3f;x<31;x+=.55f){var p=PortPoint(x,66);art.Cube("RepairAnnexMetalRib","7C8E77",p+new Vector3(0,2.5f,-.10f),new Vector3(.04f,3.9f,.08f));}
  RepairShutter149(art,PortPoint(22,66),5.1f,3.2f);
  for(float z=66.5f;z<85;z+=.54f){var p=PortPoint(31,z);art.Cube("RepairAnnexMetalRib","799078",p+new Vector3(.07f,2.4f,0),new Vector3(.08f,4,.04f));}
  art.Cube("RepairAnnexRoof","7B8976",ap+Vector3.up*4.56f,new Vector3(18.8f,.18f,19.7f));
  for(float x=13;x<=31;x+=.7f)art.Cube("RepairAnnexRoofRib","9DA48B",new Vector3(PortPoint(x,0).x,4.7f,ap.z),new Vector3(.045f,.045f,19.5f));
  art.Bake(RepairDistrictRoot149.gameObject,RepairDistrictRoot149.gameObject.AddComponent<WorkshopArtResources>());
 }
 void RepairWindow149(WorkshopWastelandArt a,Vector3 p,Vector3 along,Vector3 outward,float w,float h){
  a.Panel("RepairWindowFrame","475C50",p,along,Vector3.up,w+.18f,h+.14f);a.Panel("RepairDustyGlass","607F78",p+outward*.018f,along,Vector3.up,w,h);
  for(int n=-1;n<=1;n++)a.Tube("RepairWindowBars","A3AE92",p+along*(n*w*.29f)+Vector3.down*h*.51f+outward*.041f,p+along*(n*w*.29f)+Vector3.up*h*.51f+outward*.041f,.025f);
  a.Tube("RepairWindowCrossbar","87987F",p-along*w*.5f+outward*.041f,p+along*w*.5f+outward*.041f,.026f);
 }
 void RepairShutter149(WorkshopWastelandArt a,Vector3 p,float width,float height){
  a.Cube("RepairShutterFrame","425D4A",p+new Vector3(0,height/2,-.21f),new Vector3(width+.22f,height+.18f,.19f));
  a.Cube("RepairClosedShutter","6C8067",p+new Vector3(0,height/2,-.325f),new Vector3(width,height,.075f));
  for(int i=0;i<17;i++)a.Cube("RepairShutterSlat","9AA58B",p+new Vector3(0,.12f+i*(height-.24f)/16,-.378f),new Vector3(width-.08f,.033f,.026f));
  a.Cube("RepairShutterRust","977D58",p+new Vector3(width*.27f,.31f,-.40f),new Vector3(.8f,.2f,.03f));
  a.Cube("RepairShutterLockBar","B2A271",p+new Vector3(0,.74f,-.41f),new Vector3(width,.11f,.075f));
 }
}}
