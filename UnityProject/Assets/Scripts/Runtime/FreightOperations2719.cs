using System.Collections.Generic;
using UnityEngine;
namespace GreyHarbor {
 public sealed class FreightOperation2719:MonoBehaviour {public string Purpose,Equipment;public int Units;public Vector3 DesignAnchor;}
 public sealed partial class RaidMap {
  // Sparse operational bays: a truck needs turning room, not a perimeter carpet of rubbish.
  void BuildFreightOperations2719(){
   FreightOperationAdd2719("DispatchPallets","库后待运货","两组封装并捆扎的待运托盘",new Vector3(30,170.85f,0),new Vector2(3.30f,2.05f),2);
   FreightOperationAdd2719("RollCageBay","库后周转笼车","两台归拢停放的滚轮物流笼车",new Vector3(41.2f,170.7f,0),new Vector2(2.35f,1.80f),2);
   FreightOperationAdd2719("ReturnPalletBay","周转托盘区","完整托盘按可继续使用与待修分两垛",new Vector3(10.0f,170.5f,-4),new Vector2(3.1f,2.2f),2);
   FreightOperationAdd2719("ForkliftParking","叉车停位","货运场唯一叉车，降下货叉靠边停放",new Vector3(22.6f,152.8f,90),new Vector2(3.9f,2.1f),1);
   FreightOperationAdd2719("DispatchBench","清点打包台","值班房外货物清点桌和胶带封箱台",new Vector3(22.7f,144.1f,-3),new Vector2(2.3f,1.7f),1);
   FreightOperationAdd2719("WheelServiceBay","车轮维护台","备件台、轮胎和小型空气压缩机",new Vector3(42.9f,145.6f,4),new Vector2(2.65f,2.0f),1);
   FreightOperationAdd2719("LoadingStaging","装卸暂存位","靠近两栋房之间作业通道的货板与托盘车",new Vector3(42.8f,153.8f,-2),new Vector2(2.9f,1.75f),1);
  }
  bool FreightOperationAdd2719(string type,string title,string purpose,Vector3 design,Vector2 size,int units){
   // Near-pocket attempts may accommodate real model bounds; no relocation into other bays.
   foreach(var offset in new[]{Vector2.zero,new Vector2(.18f,0),new Vector2(-.18f,0),new Vector2(0,.18f),new Vector2(0,-.18f)}){
    var anchor=PortPoint(design.x+offset.x,design.y+offset.y);var envelope=CargoRect2714(anchor,size,design.z);
    if(!FreightPocket2716(envelope,null)||!FreightStockRoadClear2718(envelope))continue;
    GameObject view=type=="ForkliftParking"?CargoSimple2714("Forklift",anchor,design.z):FreightOperationModel2719(type,anchor,design.z);
    view.transform.SetParent(FreightExteriorRoot2716,true);view.name="FreightOperation2719_"+type;Physics.SyncTransforms();float floor=float.MaxValue;foreach(var renderer in view.GetComponentsInChildren<Renderer>())if(renderer.enabled)floor=Mathf.Min(floor,renderer.bounds.min.y);view.transform.position+=Vector3.up*(.055f-floor);Physics.SyncTransforms();var actual=CargoBounds2714(view);
    if(!FreightPocket2716(actual,view)||!FreightStockRoadClear2718(actual)){view.SetActive(false);TransientObjects2721.Release(view);continue;}
    var marker=view.AddComponent<FreightOperation2719>();marker.Purpose=purpose;marker.Equipment=title;marker.Units=units;marker.DesignAnchor=new Vector3(design.x+offset.x,0,design.y+offset.y);
    FreightOperationBake2719(view);
    FreightExteriorRecords2716.Add(new FreightExteriorRecord2716{Type=type,Group=title,View=view,Anchor=anchor,Footprint=actual,Kind=type=="DispatchPallets"||type=="ForkliftParking"?ObstacleKind.High:ObstacleKind.Low});
    return true;
   }
   Debug.LogWarning("Freight2719 operation bay rejected; preserve truck/search routes: "+title);return false;
  }
  void FreightOperationPart2719(GameObject parent,string name,Vector3 p,Vector3 size,string color,ObstacleKind? kind=ObstacleKind.Low){FreightCube2716(parent,name,p,size,color,kind);}
  void FreightOperationPallet2719(GameObject parent,Vector3 p,float width,float depth,int layers){
   for(int level=0;level<layers;level++){
    float y=.075f+level*.18f;for(int slat=0;slat<6;slat++)FreightOperationPart2719(parent,"ReusablePalletSlat",p+new Vector3(-width*.42f+slat*width*.168f,y+.09f,0),new Vector3(width*.13f,.065f,depth),level%2==0?"9C9379":"827C68",null);
    foreach(float z in new[]{-depth*.37f,0,depth*.37f})FreightOperationPart2719(parent,"PalletRunner",p+new Vector3(0,y-.04f,z),new Vector3(width,.105f,.13f),"706F5D",null);
   }
   FreightOperationPart2719(parent,"PalletCollision",p+new Vector3(0,layers*.09f,0),new Vector3(width,layers*.18f,depth),"827C68");
   var solid=parent.transform.GetChild(parent.transform.childCount-1);solid.GetComponent<Renderer>().enabled=false;
  }
  void FreightOperationWheel2719(GameObject parent,string name,Vector3 p,float radius,float width,bool upright=true){CargoCylinder2714(parent,name,p,radius,width,"303D3B",upright?Quaternion.Euler(90,0,0):Quaternion.identity,ObstacleKind.Low);}
  void FreightOperationCage2719(GameObject parent,Vector3 p,Vector2 size,float height,string color){
   FreightOperationPart2719(parent,"RollCageSteelDeck",p+Vector3.up*.18f,new Vector3(size.x,.10f,size.y),"687F83");
   foreach(float x in new[]{-size.x*.46f,size.x*.46f})foreach(float z in new[]{-size.y*.46f,size.y*.46f}){
    FreightOperationPart2719(parent,"RollCageCorner",p+new Vector3(x,height*.5f+.2f,z),new Vector3(.055f,height,.055f),color);
    FreightOperationWheel2719(parent,"CageCastor",p+new Vector3(x,.07f,z),.065f,.06f);
   }
   for(float y=.42f;y<=height+.16f;y+=.30f)foreach(float z in new[]{-size.y*.46f,size.y*.46f})FreightOperationPart2719(parent,"RollCageMeshRail",p+new Vector3(0,y,z),new Vector3(size.x,.023f,.028f),color,null);
   for(float x=-size.x*.4f;x<=size.x*.41f;x+=.20f)foreach(float z in new[]{-size.y*.46f,size.y*.46f})FreightOperationPart2719(parent,"RollCageWire",p+new Vector3(x,height*.5f+.2f,z),new Vector3(.023f,height,.023f),color,null);
   foreach(float x in new[]{-size.x*.46f,size.x*.46f})foreach(float y in new[]{.48f,.98f,height+.19f})FreightOperationPart2719(parent,"CageSideRail",p+new Vector3(x,y,0),new Vector3(.045f,.045f,size.y),color,null);
  }
  void FreightOperationBench2719(GameObject parent,Vector3 p,Vector2 size,string color){
   FreightOperationPart2719(parent,"IndustrialWorkTop",p+Vector3.up*.95f,new Vector3(size.x,.12f,size.y),color);
   foreach(float x in new[]{-size.x*.43f,size.x*.43f})foreach(float z in new[]{-size.y*.39f,size.y*.39f})FreightOperationPart2719(parent,"WorkbenchSteelLeg",p+new Vector3(x,.46f,z),new Vector3(.09f,.88f,.09f),"536D69");
   FreightOperationPart2719(parent,"WorkbenchLowerShelf",p+Vector3.up*.3f,new Vector3(size.x*.89f,.07f,size.y*.87f),"75827B",null);
  }
  GameObject FreightOperationModel2719(string type,Vector3 anchor,float yaw){
   var g=FreightObject2716(type,anchor);
   if(type=="DispatchPallets"){
    FreightOperationPallet2719(g,new Vector3(-.82f,0,0),1.5f,1.6f,1);FreightOperationPallet2719(g,new Vector3(.85f,0,.15f),1.35f,1.55f,1);
    foreach(float x in new[]{-.82f,.85f}){
     bool crate=x<0;FreightOperationPart2719(g,crate?"SealedShippingCrate":"WeatherproofWrappedDispatch",new Vector3(x,crate?.96f:.79f,crate?0:.15f),new Vector3(crate?1.37f:1.22f,crate?1.45f:1.12f,1.43f),crate?"8D8064":"507E8B",ObstacleKind.High);
     foreach(float z in new[]{-.49f,.49f})FreightOperationPart2719(g,"FreightSteelStrap",new Vector3(x,crate?.96f:.79f,z+(crate?0:.15f)),new Vector3(crate?1.39f:1.24f,crate?1.48f:1.15f,.045f),"3D5655",null);
     FreightOperationPart2719(g,"DispatchWaybill",new Vector3(x+.22f,.9f,-.728f+(crate?0:.15f)),new Vector3(.30f,.23f,.012f),"C0C4AD",null);
    }
   }else if(type=="RollCageBay"){
    FreightOperationCage2719(g,new Vector3(-.62f,0,0),new Vector2(1.10f,1.40f),1.60f,"8AA3A1");FreightOperationCage2719(g,new Vector3(.62f,0,.20f),new Vector2(.90f,1.16f),1.38f,"63888C");
    FreightOperationPart2719(g,"CageReusablePlasticTote",new Vector3(-.62f,.44f,0),new Vector3(.87f,.47f,.9f),"687F6C");
   }else if(type=="ReturnPalletBay"){
    FreightOperationPallet2719(g,new Vector3(-.73f,0,0),1.35f,1.65f,6);FreightOperationPallet2719(g,new Vector3(.76f,0,.16f),1.30f,1.50f,3);
    FreightOperationPart2719(g,"PalletGradeTag",new Vector3(-.73f,.58f,-.831f),new Vector3(.32f,.18f,.02f),"78958B",null);
   }else if(type=="DispatchBench"){
    FreightOperationBench2719(g,new Vector3(0,0,.05f),new Vector2(2.0f,1.0f),"9B957F");
    FreightOperationPart2719(g,"SealedCartonOnPackingDesk",new Vector3(.54f,1.22f,.08f),new Vector3(.68f,.47f,.65f),"A29677");
    FreightOperationPart2719(g,"PackingTape",new Vector3(.54f,1.465f,.08f),new Vector3(.08f,.016f,.66f),"777F71",null);
    FreightOperationPart2719(g,"GoodsChecklist",new Vector3(-.38f,1.022f,-.20f),new Vector3(.46f,.014f,.35f),"C2C5B1",null);
    FreightOperationPart2719(g,"PackingScale",new Vector3(-.64f,1.08f,.21f),new Vector3(.4f,.12f,.4f),"587E80",null);
    FreightOperationPart2719(g,"ToteUnderDispatchDesk",new Vector3(.54f,.48f,.13f),new Vector3(.66f,.3f,.65f),"54776A");
   }else if(type=="WheelServiceBay"){
    FreightOperationBench2719(g,new Vector3(.40f,0,.31f),new Vector2(1.45f,.86f),"6F8E8D");
    FreightOperationPart2719(g,"BenchToolTray",new Vector3(.36f,1.055f,.31f),new Vector3(.59f,.08f,.34f),"425B5A",null);
    foreach(float z in new[]{-.37f,.15f})FreightOperationWheel2719(g,"SpareTruckWheel",new Vector3(-.78f,.38f,z),.36f,.20f);
    CargoCylinder2714(g,"AirCompressorReceiver",new Vector3(.5f,.34f,-.50f),.24f,.88f,"A07F4D",Quaternion.Euler(0,0,90),ObstacleKind.Low);
    FreightOperationPart2719(g,"CompressorMotor",new Vector3(.43f,.65f,-.50f),new Vector3(.48f,.21f,.32f),"496A6B");
    FreightOperationPart2719(g,"CompressorHandle",new Vector3(.98f,.65f,-.50f),new Vector3(.05f,.58f,.34f),"394F4D",null);
   }else if(type=="LoadingStaging"){
    FreightOperationPallet2719(g,new Vector3(-.62f,0,.08f),1.43f,1.34f,1);
    FreightOperationPart2719(g,"SingleAwaitingPickupCrate",new Vector3(-.62f,.70f,.08f),new Vector3(1.27f,.98f,1.15f),"7C8878",ObstacleKind.High);
    foreach(float x in new[]{.47f,.93f})FreightOperationPart2719(g,"ParkedPalletJackFork",new Vector3(x,.14f,.10f),new Vector3(.16f,.08f,1.28f),"A29058");
    FreightOperationPart2719(g,"PalletJackPump",new Vector3(.70f,.38f,.56f),new Vector3(.61f,.58f,.35f),"A29058");
    FreightOperationPart2719(g,"PalletJackRaisedHandle",new Vector3(.70f,1.02f,.63f),new Vector3(.06f,.81f,.05f),"394F4D",null);
    FreightOperationPart2719(g,"PalletJackGrip",new Vector3(.70f,1.42f,.63f),new Vector3(.38f,.07f,.07f),"394F4D",null);
    FreightOperationWheel2719(g,"PalletJackSteeringWheel",new Vector3(.7f,.10f,.56f),.10f,.14f);
   }
   g.transform.rotation=Quaternion.Euler(0,yaw,0);return g;
  }
  void FreightOperationBake2719(GameObject root){
   var grouped=new Dictionary<Material,List<CombineInstance>>();var original=new List<Renderer>();
   foreach(var f in root.GetComponentsInChildren<MeshFilter>()){
    var r=f.GetComponent<MeshRenderer>();if(!r||!r.enabled||!f.sharedMesh||!r.sharedMaterial)continue;
    List<CombineInstance> parts;if(!grouped.TryGetValue(r.sharedMaterial,out parts)){parts=new List<CombineInstance>();grouped.Add(r.sharedMaterial,parts);}
    parts.Add(new CombineInstance{mesh=f.sharedMesh,transform=root.transform.worldToLocalMatrix*f.transform.localToWorldMatrix});original.Add(r);
   }
   var life=root.AddComponent<WorkshopArtResources>();foreach(var group in grouped){
    var mesh=new Mesh{name="FreightOperationCombined2719"};mesh.CombineMeshes(group.Value.ToArray(),true,true);mesh.RecalculateBounds();life.Meshes.Add(mesh);
    var go=new GameObject("OperationColourBatch2719");go.transform.SetParent(root.transform,false);go.AddComponent<MeshFilter>().sharedMesh=mesh;
    var r=go.AddComponent<MeshRenderer>();r.sharedMaterial=group.Key;r.shadowCastingMode=UnityEngine.Rendering.ShadowCastingMode.Off;
   }
   foreach(var r in original)r.enabled=false;
  }
 }
}
