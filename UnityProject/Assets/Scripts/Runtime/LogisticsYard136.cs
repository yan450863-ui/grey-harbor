using UnityEngine;
using System.Collections.Generic;
namespace GreyHarbor {
 public sealed partial class RaidMap {
  public static readonly Rect LogisticsYardBounds=new Rect(-70.8f,-64.8f,45.6f,39.6f);
  public readonly List<Vector3> YardWalkPoints=new List<Vector3>();
  Rect YardRect(float x,float z,float w,float d){var p=PortPoint(x,z);return new Rect(p.x,p.z,w,d);}
  void YardHouse(string name,float x,float z,float w,float d,float height,bool cottage){
   var g=Block(name,PortPoint(x,z).x,PortPoint(x,z).z,w,d,height);g.GetComponent<Renderer>().sharedMaterial=Mat(cottage?"A49B83":"8F967D",true);
   var p=g.transform.position;var parent=new GameObject(cottage?"Yard_StaffCottage":"Yard_UtilityShed").transform;parent.SetParent(Root,false);
   var a=new WorkshopWastelandArt(this);float front=p.z-d/2;
   a.Cube("BrickPlinth","6F6852",new Vector3(p.x,.3f,p.z),new Vector3(w+.08f,.6f,d+.08f));
   a.Panel("ClosedDoor","506358",new Vector3(p.x,1.17f,front-.065f),Vector3.right,Vector3.up,1.25f,2.2f);
   a.Cube("DoorStep","96927B",new Vector3(p.x,.07f,front-.65f),new Vector3(1.7f,.14f,1.1f),false);
   foreach(float dx in new[]{-w*.28f,w*.28f}){a.Panel("OldWindowFrame","546456",new Vector3(p.x+dx,1.9f,front-.07f),Vector3.right,Vector3.up,1.5f,.9f);a.Panel("DustyGlass","65807C",new Vector3(p.x+dx,1.9f,front-.09f),Vector3.right,Vector3.up,1.27f,.66f);a.Tube("WindowMullion","A3A28B",new Vector3(p.x+dx,1.56f,front-.11f),new Vector3(p.x+dx,2.24f,front-.11f),.022f);}
   a.Tube("RainDownpipe","5B7165",new Vector3(p.x+w/2-.2f,.1f,front-.12f),new Vector3(p.x+w/2-.2f,height,front-.12f),.045f);
   for(int i=0;i<14;i++){float dx=-w*.46f+i*w*.07f;a.Patch("PeelingPlaster",i%3==0?"7E806A":"B1A68B",new Vector3(p.x+dx,.6f+(i%4)*.43f,front-.13f),Vector3.right,Vector3.up,.30f+(i%3)*.15f,.2f+(i%2)*.19f,true);}
   a.Bake(parent.gameObject,parent.gameObject.AddComponent<WorkshopArtResources>());
   // Roof is an exterior silhouette only; the full original solid footprint remains closed.
   float slope=cottage?15:8,half=d/2+.4f,rise=Mathf.Sin(slope*Mathf.Deg2Rad)*half;
   foreach(int side in new[]{-1,1}){var sheet=Box("YardHouse_Roof",new Vector3(p.x,height+.10f+rise/2,p.z+side*half/2),new Vector3(w+.8f,.12f,half/Mathf.Cos(slope*Mathf.Deg2Rad)),cottage?"827966":"677A6F");sheet.transform.rotation=Quaternion.Euler(side*slope,0,0);sheet.transform.SetParent(parent,true);
    for(float dx=-w/2;dx<=w/2;dx+=.45f){var seam=Box("YardHouse_RoofSeam",sheet.transform.position+new Vector3(dx,.09f,0),new Vector3(.035f,.035f,half/Mathf.Cos(slope*Mathf.Deg2Rad)),cottage?"A29373":"879584");seam.transform.rotation=sheet.transform.rotation;seam.transform.SetParent(parent,true);}}
   Box("YardHouse_Ridge",new Vector3(p.x,height+rise+.17f,p.z),new Vector3(w+.9f,.11f,.18f),"8E8971").transform.SetParent(parent,true);
  }
  void BuildLogisticsYard(){
   YardHouse("旧院棚屋A",18,36,10,10,3.4f,true);YardHouse("旧院棚屋B",39,20,8,8,3,false);
   var ground=new GroundSurfaceArt(this,136136);
   ground.Region(LogisticsYardBounds,GroundFinish.Soil,.043f);
   ground.Region(YardRect(11.5f,12.5f,12.7f,15.2f),GroundFinish.Soil,.068f);
   ground.Region(YardRect(26.7f,25,2.6f,23),GroundFinish.Gravel,.071f);
   ground.Region(YardRect(16.5f,28,12.8f,2.4f),GroundFinish.Gravel,.072f);
   ground.Region(YardRect(28,24,4,2.4f),GroundFinish.Gravel,.072f);
   ground.Region(YardRect(30,12.2f,2.4f,13.8f),GroundFinish.Gravel,.071f);
   ground.Region(YardRect(31,12.2f,9.2f,2.4f),GroundFinish.Gravel,.072f);
   ground.Region(YardRect(32.5f,27.2f,5,3),GroundFinish.PatchedConcrete,.072f);
   ground.Bake("Yard_GroundMaterials");
   var k=new YardLivingKit(this);
   k.GardenBed(PortPoint(18,17)+Vector3.up*.07f);k.GardenBed(PortPoint(18,24)+Vector3.up*.07f,true);
   k.Bicycle(PortPoint(24,35),90);k.Bicycle(PortPoint(24,37.6f),90,true);
   k.WashStation(PortPoint(34,28.5f),180);k.Clothesline(PortPoint(38,38),0);k.PicnicSet(PortPoint(34,33));
   k.WaterTank(PortPoint(42.5f,27.5f));k.Compost(PortPoint(12,28.5f));k.LeanTo(PortPoint(38,44),0);
   var dressing=new WorkshopWastelandArt(this);
   dressing.Floor("Yard_MeadowBase","66774D",-65,-32,9,3,.076f);
   dressing.Floor("Yard_MeadowBase","6C784D",-36.5f,-31.5f,5,8,.076f);
   dressing.Floor("Yard_MeadowBase","6F7953",-37.5f,-59.5f,6,4,.076f);
   dressing.Patch("Yard_DampGround","616A50",PortPoint(35,30)+Vector3.up*.084f,Vector3.right,Vector3.forward,2.6f,1.5f,false);
   dressing.Patch("Yard_WornSoil","9B8C6A",PortPoint(28,30)+Vector3.up*.085f,Vector3.right,Vector3.forward,3.5f,5,false);
   var dressRoot=new GameObject("Yard_GroundDressing");dressRoot.transform.SetParent(Root,false);dressing.Bake(dressRoot,dressRoot.AddComponent<WorkshopArtResources>());
   var trees=new[]{PortPoint(10.7f,43.5f),PortPoint(43,43),PortPoint(43.3f,12.1f),PortPoint(10.7f,12.2f),PortPoint(33,11.3f),PortPoint(11,33)};
   for(int i=0;i<trees.Length;i++)k.Tree(trees[i],i%3==0?1:0,136+i);
   var patches=new[]{new Vector4(10.3f,22,2.2f,13),new Vector4(44.4f,35,1.6f,15),new Vector4(38,11.3f,11,2),new Vector4(17,44.3f,11,3),new Vector4(33,44.5f,4,3),new Vector4(10.5f,37,2,8)};
   for(int i=0;i<patches.Length;i++){var p=patches[i];k.Grass(PortPoint(p.x,p.y),p.z,p.w,true,136+i);}
   foreach(var p in new[]{new Vector4(14,42,6,3),new Vector4(42,41,5,4),new Vector4(33.5f,17,2,5),new Vector4(42,11.8f,6,2),new Vector4(25,18,2,8)})k.Grass(PortPoint(p.x,p.y),p.z,p.w,false,136);
   YardWalkPoints.AddRange(new[]{PortPoint(28,42),PortPoint(28,30),PortPoint(18,29),PortPoint(28,25),PortPoint(31.2f,20),PortPoint(39,13.3f),PortPoint(30.5f,33),PortPoint(35,36),PortPoint(18,11.3f),PortPoint(50,20),PortPoint(49,38),PortPoint(18,9.2f)});
   Physics.SyncTransforms();
  }
 }
}
