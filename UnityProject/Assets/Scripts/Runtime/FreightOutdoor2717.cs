using System.Collections.Generic;
using UnityEngine;
namespace GreyHarbor {
 public sealed class FreightCluster2717 {public string Name,Purpose;public Vector3 Center;public int AcceptedGroups;}
 public sealed class FreightStockAppearance2717:MonoBehaviour {public string Type,Cluster;public int Variant,PartCount;public float Yaw;}
 public sealed partial class RaidMap {
  public readonly List<FreightCluster2717> FreightClusters2717=new List<FreightCluster2717>();
  // Coordinates are authored around causes of clutter, not a global placement grid.
  // Every request still passes the existing physical route/search/door exclusion.
  void BuildFreightOutdoor2717(){
   FreightStockCluster2717("翻车散货","Cargo scattered by the overturned truck",new Vector3(10.2f,146.1f,0),new[]{
    new Vector4(8.1f,143.3f,-17,0),new Vector4(10.7f,143.8f,28,1),new Vector4(12.9f,145.3f,-24,2),new Vector4(7.2f,146.1f,14,3),
    new Vector4(9.05f,145.6f,57,4),new Vector4(12.7f,148.3f,8,3),new Vector4(12.6f,151.25f,72,0),new Vector4(12.85f,153.95f,-31,4),
    new Vector4(6.6f,144.15f,38,5),new Vector4(11.9f,142.6f,-38,5),new Vector4(7.25f,142.1f,8,6),new Vector4(10.1f,145.95f,-12,6),
    new Vector4(13.45f,147.1f,46,6),new Vector4(13.15f,150.3f,-23,5),new Vector4(12.45f,154.9f,41,6),new Vector4(6.05f,146.6f,69,6),
    new Vector4(8.8f,141.8f,-14,6),new Vector4(13.45f,142f,23,6)});
   FreightStockCluster2717("北墙废弃角","Broken handling stock accumulated along the boundary",new Vector3(10.2f,170.6f,0),new[]{
    new Vector4(7.55f,168.1f,14,7),new Vector4(10.45f,168.4f,-27,8),new Vector4(12.7f,169.7f,64,1),new Vector4(8.2f,171.1f,41,9),
    new Vector4(11.35f,172.2f,-18,2),new Vector4(7.3f,173.1f,70,4),new Vector4(13.15f,173f,31,3),new Vector4(9.55f,173.4f,-32,3),
    new Vector4(6.45f,169.8f,16,5),new Vector4(9.25f,167.1f,-25,6),new Vector4(12.3f,167.75f,43,5),new Vector4(10.1f,170.5f,81,6),
    new Vector4(6.6f,172.25f,8,6),new Vector4(12.7f,171.5f,-48,6),new Vector4(8.85f,169.95f,27,5),new Vector4(6.35f,171.2f,53,6)});
   FreightStockCluster2717("值班房拆包边","Unpacked freight and discarded wrapping beside the duty house",new Vector3(22.6f,150.5f,0),new[]{
    new Vector4(22.6f,144.2f,83,10),new Vector4(22.9f,151.0f,74,0),new Vector4(22.6f,153.65f,101,2),new Vector4(22.15f,155f,-23,3),
    new Vector4(21.55f,145.85f,13,5),new Vector4(24f,144.35f,42,6),new Vector4(23.6f,151.9f,-28,5),new Vector4(21.6f,152.7f,31,6),
    new Vector4(24.2f,154.5f,7,6),new Vector4(22.05f,142.6f,-41,6),new Vector4(24.1f,152.95f,39,6),new Vector4(20.95f,154.25f,-6,6)});
   FreightStockCluster2717("库后待运货","Mixed-height dispatch cargo and abandoned packing at the warehouse",new Vector3(34.9f,171.25f,0),new[]{
    new Vector4(28.8f,171.05f,-9,11),new Vector4(31.8f,171.15f,19,12),new Vector4(34.25f,170.95f,-18,1),new Vector4(40.25f,171.25f,13,11),
    new Vector4(43.1f,171.3f,-12,13),new Vector4(29.3f,169.45f,22,2),new Vector4(41.1f,169.85f,28,3),new Vector4(32.8f,170.0f,43,5),
    new Vector4(35.05f,172.0f,19,6),new Vector4(42.25f,170.15f,-36,6),new Vector4(22.45f,167.1f,63,9),new Vector4(23.55f,172.45f,-19,3),
    new Vector4(21.55f,168.35f,-27,5),new Vector4(24f,166.8f,15,6),new Vector4(21.55f,172.35f,38,6),new Vector4(24.2f,168.2f,17,6),new Vector4(35.7f,169.65f,16,5)});
   FreightStockCluster2717("东墙废料沟","Rain-washed broken packaging and maintenance stock",new Vector3(43.1f,147.2f,0),new[]{
    new Vector4(42.85f,144.5f,79,7),new Vector4(43.05f,147.1f,61,2),new Vector4(42.75f,153.9f,104,0),new Vector4(44.4f,145.6f,26,3),
    new Vector4(41.75f,148.5f,-28,5),new Vector4(44.15f,148.05f,47,6),new Vector4(43.95f,153.6f,-18,6),new Vector4(41.5f,154.45f,12,6),
    new Vector4(48.9f,143.3f,88,3),new Vector4(49.4f,146.35f,76,4),new Vector4(48.95f,148.05f,112,5),new Vector4(49.35f,152.1f,93,3),
    new Vector4(48.75f,154.35f,77,6),new Vector4(49.4f,157.25f,91,4),new Vector4(48.95f,160.25f,99,5),new Vector4(49.4f,164.35f,108,3),
    new Vector4(48.8f,167.05f,79,6),new Vector4(49.35f,170.45f,94,4),new Vector4(48.95f,172.3f,109,5)});
  }
  static readonly string[] FreightStockTypes2717={"FallenCrate","BrokenPallet","SackHeap","SplitCartons","LooseBags","MetalOffcuts","ScatteredPacking","WasteCorner","BentSheets","TornTarpaulin","CrateStack","DispatchLoad","WrappedLoad","TimberStack"};
  void FreightStockCluster2717(string name,string purpose,Vector3 center,Vector4[] plan){
   var cluster=new FreightCluster2717{Name=name,Purpose=purpose,Center=PortPoint(center.x,center.y)};FreightClusters2717.Add(cluster);
   // Authored cause/area stays fixed; actual piles do not share the old row coordinates.
   // Seed is map-stable, so entering another room cannot move collisions or searched boxes.
   var rng=new System.Random(271800+(int)(center.x*101+center.y*43));
   for(int i=0;i<plan.Length;i++){
    var p=plan[i];string type=FreightStockTypes2717[(int)p.w];
    if(type=="SackHeap"&&i%3==0)type="TornTarpaulin";
    if(type=="LooseBags"&&i%2==0)type="MetalOffcuts";
    bool accepted=false;
    for(int attempt=0;attempt<7&&!accepted;attempt++){
     float radius=attempt<3?.92f:.48f,angle=(float)rng.NextDouble()*Mathf.PI*2;
     float reach=radius*Mathf.Sqrt((float)rng.NextDouble());
     var design=new Vector3(p.x+Mathf.Cos(angle)*reach,p.y+Mathf.Sin(angle)*reach,p.z+(float)rng.NextDouble()*68-34);
     accepted=FreightStockAdd2717(type,design,cluster,i);
    }
    // Compact irregular remnants keep density in a tight pocket, never on its corridor.
    for(int attempt=0;attempt<5&&!accepted;attempt++){
     var design=new Vector3(p.x+((float)rng.NextDouble()-.5f)*.9f,p.y+((float)rng.NextDouble()-.5f)*1.1f,(float)rng.NextDouble()*360);
     accepted=FreightStockAdd2717("ScatteredPacking",design,cluster,i);
    }
    if(accepted)cluster.AcceptedGroups++;
   }
  }
  Vector2 FreightStockSize2717(string type){if(type=="FallenCrate")return new Vector2(1.65f,1.25f);if(type=="BrokenPallet")return new Vector2(1.95f,1.23f);if(type=="SackHeap")return new Vector2(1.56f,1.08f);if(type=="SplitCartons")return new Vector2(1.12f,.78f);if(type=="LooseBags")return new Vector2(.88f,.62f);if(type=="ScatteredPacking")return new Vector2(.66f,.62f);if(type=="WasteCorner")return new Vector2(1.85f,1.4f);if(type=="BentSheets")return new Vector2(1.95f,1.35f);if(type=="TornTarpaulin")return new Vector2(1.45f,1.02f);if(type=="DispatchLoad")return new Vector2(2.25f,1.45f);if(type=="MetalOffcuts")return new Vector2(.94f,.72f);return CargoDecorSize2715(type);}
  bool FreightStockRoadClear2718(Rect area){
   foreach(var road in HarborRoadShapes){
    var bounds=CargoGroundExtent2715(road);bounds.xMin-=.12f;bounds.xMax+=.12f;bounds.yMin-=.12f;bounds.yMax+=.12f;if(!bounds.Overlaps(area))continue;
    foreach(var p in CargoGroundCorners2715(area))if(InPolygon(p,road))return false;
    foreach(var p in road)if(area.Contains(p))return false;
    for(int i=0;i<road.Length;i++){var a=road[i];var b=road[(i+1)%road.Length];if(CargoSegmentRect2714(new Vector3(a.x,0,a.y),new Vector3(b.x,0,b.y),area)<.12f)return false;}
   }return true;
  }
  bool FreightStockAdd2717(string type,Vector3 design,FreightCluster2717 cluster,int variant){var anchor=PortPoint(design.x,design.y);var area=CargoRect2714(anchor,FreightStockSize2717(type),design.z);if(!FreightPocket2716(area,null)||!FreightStockRoadClear2718(area))return false;GameObject view;
   if(type=="CrateStack"||type=="TimberStack")view=CargoFreightModel2715(type,anchor,design.z);else if(type=="WrappedLoad")view=new DockLoadingKit142(this).WrappedLoad(anchor,design.z).gameObject;else view=FreightStockModel2717(type,anchor,design.z,variant);
   view.transform.SetParent(FreightExteriorRoot2716,true);view.name="FreightDressing2717_"+type+"_"+FreightExteriorRecords2716.Count;Physics.SyncTransforms();area=CargoBounds2714(view);if(!FreightPocket2716(area,view)||!FreightStockRoadClear2718(area)){view.SetActive(false);TransientObjects2721.Release(view);return false;}
   var appearance=view.AddComponent<FreightStockAppearance2717>();appearance.Type=type;appearance.Cluster=cluster.Name;appearance.Variant=variant;appearance.Yaw=design.z;appearance.PartCount=view.GetComponentsInChildren<Renderer>().Length;
   FreightExteriorRecords2716.Add(new FreightExteriorRecord2716{Type=type,Group=cluster.Name,View=view,Anchor=anchor,Footprint=area,Kind=type=="WrappedLoad"||type=="CrateStack"||type=="DispatchLoad"?ObstacleKind.High:ObstacleKind.Low});return true;
  }
  GameObject FreightStockPart2717(GameObject root,string name,Vector3 p,Vector3 size,string color,Vector3 angle,ObstacleKind? kind=ObstacleKind.Low){FreightCube2716(root,name,p,size,color,kind);var g=root.transform.GetChild(root.transform.childCount-1).gameObject;g.transform.localRotation=Quaternion.Euler(angle);return g;}
  void FreightStockRest2717(GameObject g){Physics.SyncTransforms();float min=float.MaxValue;foreach(var r in g.GetComponentsInChildren<Renderer>())min=Mathf.Min(min,r.bounds.min.y);g.transform.position+=Vector3.up*(.06f-min);}
  void FreightCrate2717(GameObject parent,Vector3 p,Vector3 angle,int variant){var crate=new GameObject("ToppledTimberCrate");crate.transform.SetParent(parent.transform,false);crate.transform.localPosition=p;
   FreightStockPart2717(crate,"CrateBody",new Vector3(0,.4f,0),new Vector3(.98f,.8f,.86f),variant%2==0?"9C9C87":"7D816B",Vector3.zero);
   for(int i=0;i<5;i++){FreightStockPart2717(crate,"CrateSlatSeam",new Vector3(-.4f+i*.2f,.41f,-.438f),new Vector3(.013f,.73f,.012f),"615C47",Vector3.zero,null);FreightStockPart2717(crate,"CrateUpperSlat",new Vector3(-.4f+i*.2f,.808f,0),new Vector3(.016f,.016f,.79f),"ADAE95",Vector3.zero,null);}
   foreach(float x in new[]{-.39f,.39f})FreightStockPart2717(crate,"WoodCrateBrace",new Vector3(x,.4f,-.453f),new Vector3(.07f,.81f,.04f),"B0B29A",Vector3.zero,null);
   FreightStockPart2717(crate,"DirtyWaybill",new Vector3(.14f,.45f,-.477f),new Vector3(.26f,.22f,.016f),"C4BEA0",new Vector3(0,0,-7),null);
   crate.transform.localRotation=Quaternion.Euler(angle);FreightStockRest2717(crate);
  }
  void FreightSack2717(GameObject parent,Vector3 p,float width,float height,int variant,bool solid=true){var g=new GameObject("CreasedFreightSack");g.transform.SetParent(parent.transform,false);g.transform.localPosition=p;var life=g.AddComponent<WorkshopArtResources>();var mesh=new Mesh{name="LowPolyWrinkledSack2717"};var v=new List<Vector3>();var tris=new List<int>();float[] y={0,height*.18f,height*.76f,height},r={.58f,1,.88f,.3f};
   for(int row=0;row<4;row++)for(int i=0;i<8;i++){float a=(i*Mathf.PI/4)+(row%2)*.045f;float wrinkle=1+.055f*Mathf.Sin(i*2.3f+variant);v.Add(new Vector3(Mathf.Cos(a)*width*.5f*r[row]*wrinkle,y[row],Mathf.Sin(a)*width*.38f*r[row]));}
   for(int row=0;row<3;row++)for(int i=0;i<8;i++){int a=row*8+i,b=row*8+(i+1)%8,c=b+8,d=a+8;tris.Add(a);tris.Add(d);tris.Add(c);tris.Add(a);tris.Add(c);tris.Add(b);}for(int i=1;i<7;i++){tris.Add(0);tris.Add(i);tris.Add(i+1);tris.Add(24);tris.Add(25+i);tris.Add(24+i);}mesh.SetVertices(v);mesh.SetTriangles(tris,0);mesh.RecalculateNormals();mesh.RecalculateBounds();g.AddComponent<MeshFilter>().sharedMesh=mesh;g.AddComponent<MeshRenderer>().sharedMaterial=Mat(variant%3==0?"9CA793":variant%3==1?"718878":"ABB6A3",true);life.Meshes.Add(mesh);
   if(solid){var c=g.AddComponent<MeshCollider>();c.sharedMesh=mesh;c.convex=true;SceneObstacle.Apply(g,ObstacleKind.Low);}FreightStockPart2717(parent,"SackTie",p+new Vector3(0,height+.02f,0),new Vector3(width*.21f,.04f,width*.18f),"615F49",new Vector3(0,variant*17,0),null);
  }
  GameObject FreightStockModel2717(string type,Vector3 anchor,float yaw,int variant){var g=FreightObject2716(type,anchor);float sign=variant%2==0?1:-1;
   if(type=="FallenCrate"){FreightCrate2717(g,new Vector3(-.11f,0,0),new Vector3(11,9,sign*38),variant);FreightStockPart2717(g,"DetachedPackingBoard",new Vector3(.48f,.08f,.24f),new Vector3(.9f,.08f,.14f),"887858",new Vector3(0,24,3));}
   else if(type=="BrokenPallet"){for(int i=0;i<5;i++)FreightStockPart2717(g,"SnappedPalletSlat",new Vector3(-.55f+i*.27f,.08f+(i%2)*.035f,-.12f+i*.044f),new Vector3(.19f,.07f,i==2?.70f:1.04f),i%2==0?"8B967F":"687C71",new Vector3(i==3?9:0,(i-2)*7,0));FreightStockPart2717(g,"DetachedPalletRunner",new Vector3(.12f,.055f,.39f),new Vector3(1.68f,.10f,.14f),"685F47",new Vector3(0,17,0));FreightStockPart2717(g,"SplinteredTimber",new Vector3(-.37f,.1f,-.38f),new Vector3(1.06f,.075f,.11f),"A0A58E",new Vector3(0,-21,8));}
   else if(type=="SackHeap"||type=="LooseBags"){FreightSack2717(g,new Vector3(-.22f,.06f,0),.62f,.52f,variant);FreightSack2717(g,new Vector3(.26f,.06f,.12f),.47f,.36f,variant+1);if(type=="SackHeap"){FreightSack2717(g,new Vector3(.10f,.39f,-.1f),.54f,.41f,variant+2);FreightStockPart2717(g,"TornSackSpill",new Vector3(-.57f,.085f,.14f),new Vector3(.39f,.06f,.44f),"909C89",new Vector3(0,28,0));}}
   else if(type=="SplitCartons"){FreightStockPart2717(g,"CartonBase",new Vector3(-.2f,.06f,0),new Vector3(.50f,.07f,.54f),"987E5D",Vector3.zero);foreach(float z in new[]{-.255f,.255f})FreightStockPart2717(g,"TornCartonWall",new Vector3(-.2f,.24f,z),new Vector3(.49f,.34f,.025f),"AE936B",new Vector3(0,0,z<0?8:-9));FreightStockPart2717(g,"BentCardboardFlap",new Vector3(-.18f,.45f,.31f),new Vector3(.48f,.025f,.28f),"BFA47B",new Vector3(-39,5,0));FreightSack2717(g,new Vector3(.24f,.055f,.04f),.36f,.29f,variant);}
   else if(type=="WasteCorner"||type=="BentSheets"){for(int i=0;i<4;i++)FreightStockPart2717(g,"WarpedCorrugatedOffcut",new Vector3(-.39f+i*.26f,.16f+i*.12f,0),new Vector3(.76f,.055f,1.0f),i%2==0?"6D8487":"986447",new Vector3(5+i*8,-12+i*13,sign*(i*4)));FreightStockPart2717(g,"FallenSteelAngle",new Vector3(.19f,.10f,.47f),new Vector3(1.37f,.07f,.10f),"4F655B",new Vector3(0,-19,0));if(type=="WasteCorner")FreightSack2717(g,new Vector3(.57f,.06f,-.27f),.50f,.66f,variant);}
   else if(type=="TornTarpaulin"){FreightStockPart2717(g,"CrushedPackingCase",new Vector3(0,.30f,0),new Vector3(.98f,.48f,.69f),"897657",new Vector3(0,8,0));FreightStockPart2717(g,"CollapsedTarpaulin",new Vector3(0,.57f,.03f),new Vector3(1.19f,.055f,.83f),variant%2==0?"586F4C":"526F87",new Vector3(4,-6,6),null);FreightStockPart2717(g,"FoldedTarpTail",new Vector3(-.54f,.37f,.03f),new Vector3(.25f,.51f,.81f),variant%2==0?"708055":"677F98",new Vector3(0,-6,-8),null);FreightStockPart2717(g,"TarpaulinRope",new Vector3(.32f,.61f,.03f),new Vector3(.035f,.03f,.84f),"9B9472",new Vector3(0,-6,0),null);}
   else if(type=="DispatchLoad"){for(int i=0;i<5;i++)FreightStockPart2717(g,"DispatchPallet",new Vector3(-.77f+i*.38f,.15f,0),new Vector3(.27f,.11f,1.14f),"8A7656",Vector3.zero);FreightStockPart2717(g,"DispatchCrate",new Vector3(-.42f,.80f,0),new Vector3(1.05f,1.20f,1.05f),"617E91",new Vector3(0,-6,0),ObstacleKind.High);FreightSack2717(g,new Vector3(.61f,.22f,.12f),.61f,.73f,variant);FreightStockPart2717(g,"PackingPaperBundle",new Vector3(-.40f,1.45f,-.07f),new Vector3(.75f,.11f,.63f),"BBBBA7",new Vector3(0,14,0));FreightStockPart2717(g,"DispatchTie",new Vector3(-.40f,1.52f,0),new Vector3(.048f,.033f,.99f),"4D6153",new Vector3(0,-6,0),null);}
   else if(type=="MetalOffcuts"){for(int i=0;i<4;i++)FreightStockPart2717(g,"DiscardedPlate",new Vector3(-.06f+i*.035f,.09f+i*.075f,-.1f+i*.045f),new Vector3(.64f,.055f,.39f),i%2==0?"798B89":"97674C",new Vector3(i*3,sign*(i*11-9),i%2==0?3:-4));}
   else{FreightStockPart2717(g,"FlattenedWaybillCarton",new Vector3(-.11f,.08f,.05f),new Vector3(.45f,.07f,.43f),"9F9987",new Vector3(0,17,2));FreightStockPart2717(g,"BrokenPackingBlock",new Vector3(.20f,.15f,-.14f),new Vector3(.19f,.23f,.22f),"7C877A",new Vector3(0,-31,8));FreightStockPart2717(g,"DiscardedLabel",new Vector3(-.16f,.126f,.05f),new Vector3(.21f,.012f,.18f),"C6C0A5",new Vector3(0,29,0),null);}
   // Small remnants belong to their pile and do not form another regular row.
   if(type!="ScatteredPacking")for(int i=0;i<3;i++)FreightStockPart2717(g,"PackingFragment",new Vector3(Mathf.Sin(i*2+variant)*.31f,.078f,Mathf.Cos(i*3+variant)*.29f),new Vector3(.12f,.032f,.08f),i%2==0?"AAA994":"74857C",new Vector3(0,variant*13+i*37,0),null);
   g.transform.rotation=Quaternion.Euler(0,yaw,0);return g;
  }
 }
}
