using UnityEngine;
using System.Collections.Generic;
namespace GreyHarbor {
 public sealed partial class RaidMap {
  public readonly List<Transform> FerryFurniture144=new List<Transform>();
  public readonly List<Transform> FerryPierProps144=new List<Transform>();
  public WorldDoor FerryKeyDoor144;
  void DressFerryInterior144(){
   var floor=new GroundSurfaceArt(this,144101+(InteriorId=="ferry1"?0:1));floor.Region(new Rect(-7.1f,-14.9f,14.2f,29.8f),GroundFinish.Epoxy);floor.Region(FerryLobby143,GroundFinish.Terrazzo);
   if(InteriorId=="ferry1"){floor.Region(new Rect(-1.42f,-14.8f,2.84f,12.7f),GroundFinish.Epoxy);floor.Region(FerryRooms143[0].Area,GroundFinish.Steel);floor.Region(FerryRooms143[1].Area,GroundFinish.Steel);floor.Region(FerryRooms143[2].Area,GroundFinish.BrokenTile);}floor.Bake("FerryGround144");
   var k=new FerryLivingKit144(this);
   if(InteriorId=="ferry1"){
    FerryFurniture144.Add(k.LuggageRack(new Vector3(-6.4f,0,-13),90));FerryFurniture144.Add(k.LuggageRack(new Vector3(-6.4f,0,-8.4f),90));FerryFurniture144.Add(k.LuggageRack(new Vector3(-4.5f,0,-14.1f),0));
    FerryFurniture144.Add(k.SupplyShelf(new Vector3(-6.6f,0,-5),90));FerryFurniture144.Add(k.SupplyShelf(new Vector3(-4.5f,0,-6.45f)));FerryFurniture144.Add(k.Table(new Vector3(-4.8f,0,-2.8f)));
    FerryFurniture144.Add(k.Toilet(new Vector3(4.8f,0,-13.6f)));FerryFurniture144.Add(k.Toilet(new Vector3(6.2f,0,-13.6f)));FerryFurniture144.Add(k.WashUnit(new Vector3(6.6f,0,-10),90));
    foreach(float x in new[]{4.1f,5.5f,6.9f}){var divider=Box("ToiletPrivacyScreen",new Vector3(x,.82f,-13.6f),new Vector3(.08f,1.64f,2.3f),"748C7D",true);SceneObstacle.Apply(divider,ObstacleKind.High);}
    FerryFurniture144.Add(k.ServiceCounter(new Vector3(6.5f,0,-4.9f),90));FerryFurniture144.Add(k.Table(new Vector3(3.8f,0,-7)));FerryFurniture144.Add(k.SeatRow(new Vector3(4.4f,0,-3.0f)));
    for(float z=3.8f;z<10;z+=2.7f)foreach(float x in new[]{-4.5f,4.5f})FerryFurniture144.Add(k.SeatRow(new Vector3(x,0,z)));
    FerryFurniture144.Add(k.Sofa(new Vector3(-5.5f,0,13.3f),180));FerryFurniture144.Add(k.Table(new Vector3(-5.2f,0,11.6f)));
   }else{
    for(float z=4.1f;z<8;z+=3.2f)foreach(float x in new[]{-4.5f,4.5f})FerryFurniture144.Add(k.SeatRow(new Vector3(x,0,z)));
    FerryFurniture144.Add(k.Sofa(new Vector3(6.4f,0,-11.1f),90));FerryFurniture144.Add(k.Sofa(new Vector3(6.4f,0,-5.0f),90));FerryFurniture144.Add(k.Table(new Vector3(4.0f,0,-11.1f),90));FerryFurniture144.Add(k.Table(new Vector3(4.0f,0,-5.0f),90,false));FerryFurniture144.Add(k.Sofa(new Vector3(-5.6f,0,-7.6f)));FerryFurniture144.Add(k.Table(new Vector3(-5.6f,0,-4.7f)));
    FerryFurniture144.Add(k.BridgeConsole(new Vector3(-2.2f,0,14.0f)));FerryFurniture144.Add(k.SeatRow(new Vector3(-2.1f,0,12.6f),180));FerryKeyDoor144=FerryRooms143.Find(r=>r.Door.Locked).Door;
   }
   DressFerryWalls144();DressFerryBulkheads145();FinalizeHarborGround137();
  }
  void DressFerryWalls144(){
   var g=new GameObject("FerryCabinWear144");g.transform.SetParent(Root,false);var a=new WorkshopWastelandArt(this);var keyArea=new Rect(-7.15f,-14.95f,4.25f,6.0f);
   foreach(var r in Root.GetComponentsInChildren<MeshRenderer>())if(r.name=="InteriorWall"){
    var b=r.bounds;bool horizontal=b.size.x>b.size.z;Vector3 along=horizontal?Vector3.right:Vector3.forward,normal=horizontal?Vector3.forward:Vector3.right;
    foreach(float side in new[]{-1f,1f}){Vector3 p=new Vector3(b.center.x,0,b.center.z)+normal*side*((horizontal?b.size.z:b.size.x)/2+.015f);if(Mathf.Abs(p.x)>7.11f||Mathf.Abs(p.z)>14.91f)continue;a.WallSkin(p,along,normal*side,horizontal?b.size.x:b.size.z,"5D7B75");}
   }
   for(float z=-12;z<14;z+=3.3f)foreach(float x in new[]{-7.09f,7.09f}){
    if(InteriorId=="ferry2"&&x<0&&z<-9)continue;Vector3 p=new Vector3(x,1.62f,z),normal=x<0?Vector3.right:Vector3.left;
    a.Panel("MarineWindowFrame","344F50",p,Vector3.forward,Vector3.up,1.42f,.91f);a.Panel("SaltOpaqueGlass","6E8B83",p+normal*.022f,Vector3.forward,Vector3.up,1.22f,.70f);a.Panel("WindowMullion","3B5D56",p+normal*.032f,Vector3.forward,Vector3.up,.045f,.80f);
    a.Stroke("SaltStreak","A5AF99",p+normal*.035f+Vector3.up*.30f,p+normal*.035f+Vector3.down*.28f,.018f,normal,true);
   }
   foreach(var room in FerryRooms143){var r=room.Area;a.StripLight(new Vector3(r.center.x,2.06f,r.yMax-.14f),Vector3.right,Vector3.back);}
   foreach(float z in new[]{-11f,-4f,7f})a.StripLight(new Vector3(0,2.04f,z),Vector3.right,Vector3.back);
   a.Panel("LifeJacketLockerBack","395C56",new Vector3(-5.8f,1.35f,1.88f),Vector3.right,Vector3.up,1.8f,1.05f);for(int i=0;i<4;i++){float x=-6.4f+i*.4f;a.Panel("StoredLifeJacket","AC8962",new Vector3(x,1.35f,1.85f),Vector3.right,Vector3.up,.30f,.78f);a.Panel("LifeJacketStrap","DFBD81",new Vector3(x,1.35f,1.82f),Vector3.right,Vector3.up,.035f,.70f);}
   a.Panel("PassengerEvacuationDiagram","A0AB92",new Vector3(-6.1f,1.60f,-1.90f),Vector3.right,Vector3.up,1.14f,.69f);for(int i=0;i<4;i++)a.Panel("DiagramBlocks","486E61",new Vector3(-6.5f+i*.26f,1.62f,-1.878f),Vector3.right,Vector3.up,.18f,.26f);
   if(InteriorId=="ferry2"){a.Panel("BridgeRadarMap","91A38B",new Vector3(-5.2f,1.65f,14.88f),Vector3.right,Vector3.up,1.8f,1.0f);a.Panel("BridgeControlCabinet","587A6A",new Vector3(7.085f,1.22f,12.8f),Vector3.forward,Vector3.up,.7f,1.2f);}
   a.Bake(g,g.AddComponent<WorkshopArtResources>());
   foreach(var d in Doors)if(!d.Portal){var p=d.Position;bool vertical=Mathf.Abs(p.x)>1;var pull=Box("CabinDoorHandle",p+Vector3.up*.96f+(vertical?new Vector3(.10f,0,.65f):new Vector3(.65f,0,-.10f)),vertical?new Vector3(.12f,.08f,.22f):new Vector3(.22f,.08f,.12f),"B2AC8B");pull.transform.SetParent(d.Pivot,true);}
   var dirt=new GameObject("FerryFloorWear144");dirt.transform.SetParent(Root,false);var paint=new WorkshopWastelandArt(this);foreach(var p in new[]{new Vector3(-5.7f,.055f,.3f),new Vector3(.9f,.055f,-12),new Vector3(3.1f,.055f,11.6f)}){paint.Patch("BootSaltDirt","596D60",p,Vector3.right,Vector3.forward,1.1f,.55f,false);}
   for(int i=0;i<12;i++){float z=-12.5f+i*2.1f;paint.Floor("LooseTicket","B8B29A",6.7f,z,.19f,.26f,.06f);}paint.Bake(dirt,dirt.AddComponent<WorkshopArtResources>());
   foreach(var f in dirt.GetComponentsInChildren<MeshFilter>()){RegisterGround137(new List<Vector3>(f.sharedMesh.vertices),new List<int>(f.sharedMesh.triangles),f.GetComponent<Renderer>().sharedMaterial.color);f.GetComponent<Renderer>().enabled=false;}
  }
  void DressPassengerPier144(){
   var k=new FerryLivingKit144(this);var baseKit=new DockLivingKit141(this);
   FerryPierProps144.Add(k.LifebuoyStation(PortPoint(180,80.7f),90));FerryPierProps144.Add(k.LifebuoyStation(PortPoint(170,97),0));FerryPierProps144.Add(k.PierSign(PortPoint(172.0f,86.1f),90));
   foreach(float z in new[]{78f,92f,99f})FerryPierProps144.Add(k.Fender(PortPoint(184,z),90));
   baseKit.Bollard(PortPoint(180.2f,86.2f));baseKit.Bollard(PortPoint(187,101.5f));
   var root=new GameObject("PassengerPierDetail144");root.transform.SetParent(Root,false);var a=new WorkshopWastelandArt(this);
   for(int j=0;j<2;j++)for(int i=0;i<22;i++){float t=i/21f;var start=PortPoint(180.2f,86.2f)+Vector3.up*.65f;var end=PortPoint(187,101.5f)+Vector3.up*.60f;var p=Vector3.Lerp(start,end,t);var q=Vector3.Lerp(start,end,(i+1)/21f);p.y-=Mathf.Sin(t*Mathf.PI)*.28f;q.y-=Mathf.Sin((i+1)/21f*Mathf.PI)*.28f;a.Tube("MooredShipRope","9D9271",p+Vector3.right*j*.08f,q+Vector3.right*j*.08f,.025f);}
   // Shore details stay outside boarding movement space.
   foreach(float x in new[]{151f,158f,165f}){var p=PortPoint(x,97.4f);a.Cube("PassengerBollardCap","939C7D",p+Vector3.up*.07f,new Vector3(.34f,.14f,.34f),false);}
   a.Bake(root,root.AddComponent<WorkshopArtResources>());
   for(float z=69;z<99;z+=.38f){PaintDockRect141(184.8f,z,1.40f,.025f,"6A7E70",.052f);PaintDockRect141(201.2f,z,1.40f,.025f,"6A7E70",.052f);}
   for(float x=187;x<200;x+=.34f)PaintDockRect141(x,104,.022f,6.0f,"778575",.052f);
   for(float z=81;z<87;z+=.7f)PaintDockRect141(171.8f,z,.10f,.38f,"B8AD82",.052f);
  }
 }
}
