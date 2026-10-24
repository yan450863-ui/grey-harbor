using UnityEngine;
using System.Collections.Generic;
namespace GreyHarbor {
 public sealed class DockBoatPlan141 {public string Name;public Vector3 Center,Shore,Board,Deck;public Rect Gangway;public Transform Model;}
 public sealed partial class RaidMap {
  public readonly List<DockBoatPlan141> DockBoats141=new List<DockBoatPlan141>();
  public static readonly Vector3 DockPreviewSpawn141=PortPoint(160,29);
  void AddDockSurfaces141(){
   AddHarborFloor("Dock_FishingBoatDeck141","8D876C",180,132,184,132,184,139,183,142,181,142,180,139);
   AddHarborFloor("Dock_FishingGangway141","859B91",180.9f,129,183.1f,129,183.1f,133,180.9f,133);
   AddHarborFloor("Dock_WorkBargeDeck141","7F8877",177.5f,45,182.5f,45,182.5f,54,181.5f,57,178.5f,57,177.5f,54);
   AddHarborFloor("Dock_WorkGangway141","859B91",178.9f,42,181.1f,42,181.1f,46,178.9f,46);
   DockBoats141.Add(new DockBoatPlan141{Name="旧渔船",Center=PortPoint(182,137),Shore=PortPoint(182,128),Board=PortPoint(182,131),Deck=PortPoint(182,135),Gangway=new Rect(101.9f,56,2.2f,4)});
   DockBoats141.Add(new DockBoatPlan141{Name="作业驳船",Center=PortPoint(180,51),Shore=PortPoint(180,41),Board=PortPoint(180,44),Deck=PortPoint(180,48),Gangway=new Rect(99.9f,-31,2.2f,4)});
  }
  void DockCoastRail141(Vector2 start,Vector2 end){
   var delta=end-start;float length=delta.magnitude;var mid=(start+end)/2;var g=Box("Harbor_CoastRail",new Vector3(mid.x,.40f,mid.y),new Vector3(length+.01f,.8f,.16f),"6D8580",true);g.transform.rotation=Quaternion.Euler(0,-Mathf.Atan2(delta.y,delta.x)*Mathf.Rad2Deg,0);SceneObstacle.Apply(g,ObstacleKind.Low);g.GetComponent<Renderer>().enabled=false;
   var root=new GameObject("DockEdgeRail141").transform;root.SetParent(Root,false);var art=new WorkshopWastelandArt(this);Vector3 a=new Vector3(start.x,0,start.y),b=new Vector3(end.x,0,end.y);
   for(float d=.08f;d<length;d+=2.7f){var p=Vector3.Lerp(a,b,d/length);art.Cube("RailFoot","566E66",p+Vector3.up*.05f,new Vector3(.23f,.10f,.23f));art.Tube("WeatheredRailPost","7F9283",p+Vector3.up*.1f,p+Vector3.up*.83f,.034f);}
   art.Tube("TopRail","899D90",a+Vector3.up*.80f,b+Vector3.up*.80f,.033f);art.Tube("MidRail","6C857B",a+Vector3.up*.39f,b+Vector3.up*.39f,.022f);art.Bake(root.gameObject,root.gameObject.AddComponent<WorkshopArtResources>());
  }
  void PaintDockRect141(float x,float z,float w,float d,string color,float y=.031f){var p=PortPoint(x,z);var v=new List<Vector3>{p+new Vector3(-w/2,y,-d/2),p+new Vector3(-w/2,y,d/2),p+new Vector3(w/2,y,d/2),p+new Vector3(w/2,y,-d/2)};RegisterGround137(v,new List<int>{0,1,2,0,2,3},PixelArt.Hex(color));}
  void DockBasePaint141(string name,string color){var f=Root.Find(name).GetComponent<MeshFilter>();var points=new List<Vector3>(f.sharedMesh.vertices);for(int i=0;i<points.Count;i++)points[i]=f.transform.TransformPoint(points[i])+Vector3.up*.01f;RegisterGround137(points,new List<int>(f.sharedMesh.triangles),PixelArt.Hex(color));}
  void DockPallet141(float x,float z,float w,float d){Box("DockPallet",PortPoint(x,z)+Vector3.up*.06f,new Vector3(w,.12f,d),"8A7D60");for(float t=-w/2+.13f;t<w/2;t+=.26f)Box("PalletBoard",PortPoint(x+t,z)+Vector3.up*.13f,new Vector3(.21f,.035f,d),"A3916E");}
  void DockRope141(float x,float z,float radius){var root=new GameObject("DockRopeCoil141");root.transform.SetParent(Root,false);var art=new WorkshopWastelandArt(this);for(int ring=0;ring<3;ring++)for(int i=0;i<20;i++){float r=radius-ring*.042f,a=i*Mathf.PI/10,b=(i+1)*Mathf.PI/10;art.Tube("CoiledMooringRope","A3926D",PortPoint(x+Mathf.Cos(a)*r,z+Mathf.Sin(a)*r)+Vector3.up*.07f,PortPoint(x+Mathf.Cos(b)*r,z+Mathf.Sin(b)*r)+Vector3.up*.07f,.017f);}art.Bake(root,root.AddComponent<WorkshopArtResources>());}
  void BuildDockDistrict141(int seed){
   var kit=new DockLivingKit141(this);DockBoats141[0].Model=kit.FishingBoat(DockBoats141[0].Center);DockBoats141[1].Model=kit.WorkBarge(DockBoats141[1].Center);kit.Gangway(PortPoint(182,131));kit.Gangway(PortPoint(180,44));
   kit.OpenShelter(PortPoint(154,132));kit.OpenShelter(PortPoint(154,94));kit.OpenShelter(PortPoint(156,35));
   kit.Workbench(PortPoint(154,90.9f));kit.Workbench(PortPoint(181.35f,51.8f),90);
   kit.JibCrane(PortPoint(167,136),90);kit.JibCrane(PortPoint(167,33),-90);
   kit.CableDrum(PortPoint(166,119),15);kit.CableDrum(PortPoint(163,95),0);kit.CableDrum(PortPoint(174,31),20);
   kit.CargoTrolley(PortPoint(165,82),30);kit.CargoTrolley(PortPoint(158,38),-15);
   foreach(var p in new[]{new Vector2(175,123),new Vector2(185,123),new Vector2(179,85),new Vector2(176,42),new Vector2(183,20)})kit.Bollard(PortPoint(p.x,p.y));
   DockRope141(174,125,.48f);DockRope141(167,90,.42f);DockRope141(176,40,.48f);DockRope141(181.7f,48,.26f);
   DockPallet141(168,95,2.1f,.9f);DockPallet141(172.5f,39,1.6f,1.3f);DockPallet141(181,134,1.35f,.9f);DockPallet141(178.7f,49.2f,1.55f,1.1f);
   Box("DockHighCargoStack",PortPoint(165,24)+Vector3.up*1.45f,new Vector3(3.4f,2.9f,3.0f),"657E72",true);Box("DockStackBand",PortPoint(165,24)+Vector3.up*2.8f,new Vector3(3.5f,.12f,3.1f),"92947C");
   Box("DockLowSortingCrates",PortPoint(161,119)+Vector3.up*.47f,new Vector3(2.6f,.94f,1.4f),"8B8562",true);SceneObstacle.Apply(Root.Find("DockLowSortingCrates").gameObject,ObstacleKind.Low);
   Box("DockLowSortingCrateBand",PortPoint(161,119)+Vector3.up*.97f,new Vector3(2.7f,.06f,1.5f),"A89B74");
   DockBasePaint141("Dock_NorthService","77887D");DockBasePaint141("Dock_MiddleRepair","72877F");DockBasePaint141("Dock_SouthLoading","7F8B7C");DockBasePaint141("Dock_NorthLink","627D78");DockBasePaint141("Dock_SouthLink","627D78");DockBasePaint141("Dock_FishingBoatDeck141","91866A");DockBasePaint141("Dock_WorkBargeDeck141","738779");DockBasePaint141("Dock_FishingGangway141","8B9B8A");DockBasePaint141("Dock_WorkGangway141","8B9B8A");
   var floor=new GroundSurfaceArt(this,141017);floor.Region(new Rect(70.2f,43.2f,21.6f,21.6f),GroundFinish.PatchedConcrete);floor.Region(new Rect(70.2f,1.2f,21.6f,23.6f),GroundFinish.Concrete);floor.Region(new Rect(71.2f,-54.8f,27.6f,24.6f),GroundFinish.PatchedConcrete);floor.Region(new Rect(92.2f,49.2f,16.5f,7.6f),GroundFinish.Steel);floor.Region(new Rect(92.2f,7.2f,9.5f,6.6f),GroundFinish.Steel);floor.Region(new Rect(99.2f,-64.8f,11.6f,13.6f),GroundFinish.Concrete);floor.Region(new Rect(78.2f,-29.8f,3.6f,30.6f),GroundFinish.Steel);floor.Region(new Rect(78.2f,25.2f,3.6f,17.6f),GroundFinish.Steel);floor.Bake("DockGround141");
   for(int i=0;i<18;i++)PaintDockRect141(151.8f,118+i*.9f,.075f,.48f,"AEA174",.04f);for(int i=0;i<13;i++)PaintDockRect141(170,20+i*1.5f,.08f,.65f,"ADA271",.04f);
   for(float z=132.4f;z<139;z+=.32f)PaintDockRect141(182,z,3.8f,.025f,"756E57",.042f);for(float z=129.2f;z<132.9f;z+=.38f)PaintDockRect141(182,z,2.08f,.028f,"B1AC8A",.043f);for(float z=42.2f;z<45.9f;z+=.38f)PaintDockRect141(180,z,2.08f,.028f,"B1AC8A",.043f);
   var grass=new YardLivingKit(this);grass.Grass(PortPoint(148.5f,118.5f),1.3f,2.0f,false,14112);grass.Grass(PortPoint(147,38),1.2f,2.2f,false,14113);
   BuildDockLoot141(seed);
  }
 }
}
