using System;
using UnityEngine;
namespace GreyHarbor {
 public sealed partial class RaidMap {
  // Additional freight is organized around nine district pockets. Each six-piece
  // bay mixes an opaque load, low handling stock and packaging, rather than dots.
  void BuildCargoDensity2715(System.Random rng){
   Vector4[] searches={new Vector4(63,74,270,2),new Vector4(66,99,0,1),new Vector4(62,118,90,4),new Vector4(85,124,180,5),new Vector4(90,110,270,2),new Vector4(102,64,0,3),new Vector4(120,74,90,7),new Vector4(119,90,270,5),new Vector4(121,115,90,6),new Vector4(102,121,0,1)};
   foreach(var request in searches){bool made=false;foreach(var p in CargoCandidates2714(new Vector3(request.x,request.y,request.z)))if(CargoAddLoot2714(p.x,p.y,p.z,(int)request.w,rng)){made=true;break;}if(!made)Debug.LogWarning("Cargo2715 ordinary search pocket unavailable "+request);}
   Vector2[] bays={new Vector2(64,70),new Vector2(77,73),new Vector2(111,75),new Vector2(69,87),new Vector2(87,90),new Vector2(116,90),new Vector2(74,121),new Vector2(89,117),new Vector2(116,118)};
   Vector2[] offsets={new Vector2(-2.1f,-1.8f),new Vector2(1.5f,-1.8f),new Vector2(-2.1f,1.4f),new Vector2(1.2f,1.5f),new Vector2(-.5f,4.2f),new Vector2(3.8f,1.3f)};
   string[][] kits={new[]{"WrappedLoad","Pallets","BarrelRack","CrateStack","Packaging","RopeCoil"},new[]{"CrateStack","TimberStack","Pallets","Packaging","ScrapRack","Tires"},new[]{"WrappedLoad","BarrelRack","PipeBundle","Pallets","Spool","Packaging"}};
   for(int bay=0;bay<bays.Length;bay++)for(int slot=0;slot<offsets.Length;slot++){
    Vector2 p=bays[bay]+offsets[slot];CargoPlaceBay2715(kits[bay%kits.Length][slot],new Vector3(p.x,p.y,(bay%2)*90),"FreightBay"+(bay+1).ToString("00"));
   }
   // A difficult bay may reject a tall rack. Compact freight remains a grouped
   // fallback; accepted records are capped at84, never created in route reserves.
   for(int pass=0;CargoDressingRecords2714.Count<84&&pass<3;pass++)for(int bay=0;bay<bays.Length&&CargoDressingRecords2714.Count<84;bay++){
    Vector2 p=bays[bay]+new Vector2(pass*1.1f-1,5.6f);
    CargoPlaceBay2715(pass==0?"Packaging":pass==1?"Pallets":"CrateStack",new Vector3(p.x,p.y,(bay%2)*90),"FreightBay"+(bay+1).ToString("00"));
   }
   Debug.Log("Cargo2715 authored freight: "+CargoLoot2714.Count+" searchable / "+CargoDressingRecords2714.Count+" decoration groups");
  }
  bool CargoPlaceBay2715(string type,Vector3 preferred,string bay){
   int attempts=0;foreach(var p in CargoCandidates2714(preferred)){
    // Only inspect the local bay and nearby pocket, not thousands of world-wide
    // temporary models. All geometry/physics preflight precedes mesh creation.
    if((new Vector2(p.x,p.y)-new Vector2(preferred.x,preferred.y)).sqrMagnitude>144)break;
    if(attempts++>=220)break;
    if(!CargoAddDecor2714(type,p.x,p.y,p.z))continue;
    CargoDressingRecords2714[CargoDressingRecords2714.Count-1].Group=bay;return true;
   }return false;
  }
  Vector2 CargoDecorSize2715(string type){
   if(type=="Forklift")return new Vector2(3.9f,1.9f);if(type=="WrappedLoad")return new Vector2(2.7f,2);if(type=="BarrelRack")return new Vector2(2.8f,1.3f);if(type=="Spool")return new Vector2(2.1f,1.1f);if(type=="PalletJack")return new Vector2(1.2f,2.4f);if(type=="PipeBundle")return new Vector2(1.6f,2.85f);if(type=="CrateStack")return new Vector2(2.15f,1.6f);if(type=="ScrapRack")return new Vector2(2.45f,1.2f);if(type=="TimberStack")return new Vector2(2.4f,1.6f);if(type=="Planks")return new Vector2(2.4f,1);if(type=="Packaging")return new Vector2(1.9f,1.3f);if(type=="Tires")return new Vector2(1.7f,1.3f);return new Vector2(1.5f,1.3f);
  }
  GameObject CargoFreightModel2715(string type,Vector3 position,float yaw){
   var root=new GameObject("Cargo2715_"+type);root.transform.SetParent(Root,false);root.transform.position=position;
   if(type=="CrateStack"){
    CargoCube2714(root,"LeftTimberCrate",new Vector3(-.55f,.49f,0),new Vector3(.98f,.98f,1.4f),"897F60",ObstacleKind.Low);
    CargoCube2714(root,"RightTimberCrate",new Vector3(.55f,.49f,0),new Vector3(.98f,.98f,1.4f),"A4926E",ObstacleKind.Low);
    CargoCube2714(root,"UpperFreightCrate",new Vector3(-.25f,1.36f,.13f),new Vector3(1.05f,.74f,1.08f),"7C8264",ObstacleKind.High);
    foreach(float x in new[]{-.89f,-.21f,.21f,.89f})CargoCube2714(root,"CrateFaceBrace",new Vector3(x,.49f,-.711f),new Vector3(.065f,.94f,.025f),"B1A181",null);
    CargoCube2714(root,"ShippingLabel",new Vector3(.55f,.58f,-.735f),new Vector3(.28f,.19f,.018f),"BDB69B",null);
   }else if(type=="ScrapRack"){
    foreach(float x in new[]{-1.1f,1.1f})foreach(float z in new[]{-.48f,.48f})CargoCube2714(root,"MetalRackPost",new Vector3(x,.91f,z),new Vector3(.09f,1.82f,.09f),"526258",ObstacleKind.High);
    foreach(float y in new[]{.13f,.85f,1.66f})CargoCube2714(root,"SteelRackShelf",new Vector3(0,y,0),new Vector3(2.35f,.10f,1.06f),"707766",y<1?ObstacleKind.Low:ObstacleKind.High);
    for(int i=0;i<4;i++){CargoCube2714(root,"DiscardedMetalSheet",new Vector3(-.72f+i*.48f,.40f,0),new Vector3(.36f,.44f,.86f),i%2==0?"8D8870":"69796B",ObstacleKind.Low);CargoCube2714(root,"SpareIronStock",new Vector3(0,1.01f+i*.05f,-.31f+i*.20f),new Vector3(1.94f,.075f,.095f),"7D8170",ObstacleKind.Low);}
   }else{
    for(int layer=0;layer<5;layer++)for(int board=0;board<5;board++)CargoCube2714(root,"TimberStock",new Vector3(0,.055f+layer*.115f,-.60f+board*.30f),new Vector3(2.25f,.10f,.24f),layer%2==0?"95825F":"807251",ObstacleKind.Low);
    foreach(float x in new[]{-.72f,.72f})CargoCube2714(root,"CargoTie",new Vector3(x,.58f,0),new Vector3(.055f,.035f,1.48f),"545D4D",null);
   }
   root.transform.rotation=Quaternion.Euler(0,yaw,0);return root;
  }
  static float CargoPointSegment2715(Vector2 p,Vector2 a,Vector2 b){Vector2 d=b-a;float t=Mathf.Clamp01(Vector2.Dot(p-a,d)/Mathf.Max(d.sqrMagnitude,.000001f));return Vector2.Distance(p,a+d*t);}
  static float CargoCross2715(Vector2 a,Vector2 b)=>a.x*b.y-a.y*b.x;
  static float CargoEdgeDistance2715(Vector2 a,Vector2 b,Vector2 c,Vector2 d){
   Vector2 ab=b-a,cd=d-c;float den=CargoCross2715(ab,cd);
   if(Mathf.Abs(den)>.000001f){float t=CargoCross2715(c-a,cd)/den,u=CargoCross2715(c-a,ab)/den;if(t>=0&&t<=1&&u>=0&&u<=1)return 0;}
   return Mathf.Min(CargoPointSegment2715(a,c,d),CargoPointSegment2715(b,c,d),CargoPointSegment2715(c,a,b),CargoPointSegment2715(d,a,b));
  }
 }
}

