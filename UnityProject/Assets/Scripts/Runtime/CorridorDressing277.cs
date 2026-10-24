using UnityEngine;
using UnityEngine.Rendering;
using System.Collections.Generic;
namespace GreyHarbor {
 public enum CorridorType277 { FireCabinet,Bench,CleaningCart,TransportCart,Planter,PaperBoxes,FallenChair,NoticeBoard }
 public sealed class CorridorItem277 {
  public string Name;public CorridorType277 Type;public Vector3 Anchor,BackingPoint;public Rect Footprint,PublicArea;
  public GameObject View;public ObstacleKind Kind;public bool WallMounted;public int BackingSide;
 }
 public sealed class CorridorLane277 {
  public string Name;public Rect Area;public Vector3[] Points;public float ClearWidth=1.8f;
 }
 public sealed partial class RaidMap {
  public readonly List<CorridorItem277> CorridorItems277=new List<CorridorItem277>();
  public readonly List<CorridorLane277> CorridorLanes277=new List<CorridorLane277>();
  public readonly List<Rect> CorridorProtectedConnections277=new List<Rect>();
  bool CorridorDressed277;
  void AddCorridorLane277(string name,Vector3 a,Vector3 b){
   bool vertical=Mathf.Abs(a.z-b.z)>Mathf.Abs(a.x-b.x);
   var area=vertical?Rect.MinMaxRect(a.x-.9f,Mathf.Min(a.z,b.z),a.x+.9f,Mathf.Max(a.z,b.z)):Rect.MinMaxRect(Mathf.Min(a.x,b.x),a.z-.9f,Mathf.Max(a.x,b.x),a.z+.9f);
   CorridorLanes277.Add(new CorridorLane277{Name=name,Area=area,Points=new[]{a,b}});
  }
  public void AddCorridorDressing277(){
   bool annex=InteriorId=="repairannex";
   if(!Root||!IsInterior||(!RepairMain220&&!annex)||CorridorDressed277)return;
   CorridorDressed277=true;Physics.SyncTransforms();
   Rect[] areas=annex?new[]{Rect.MinMaxRect(-8.79f,-9.29f,4.25f,9.29f)}:RepairMainPublicAreas276(RepairFloor220);
   foreach(var area in areas)if(area.width<1.8f||area.height<.5f)CorridorProtectedConnections277.Add(area);
   if(annex){
    AddCorridorLane277("附属房主通道",new Vector3(-2.5f,0,-7),new Vector3(-2.5f,0,6.6f));
    foreach(var room in RepairRooms200)CorridorProtectedConnections277.Add(Rect.MinMaxRect(-3.4f,room.Door.Position.z-.9f,3.4f,room.Door.Position.z+.9f));
   }else{
    AddCorridorLane277("中央纵向通道",new Vector3(0,0,-21.8f),new Vector3(0,0,21.8f));
    AddCorridorLane277("中央横向通道",new Vector3(RepairFloor220==1?-6:-8,0,0),new Vector3(8,0,0));
   }
   var walls=new List<Collider>();
   foreach(var col in Root.GetComponentsInChildren<Collider>())if(col.enabled&&col.name=="InteriorWall"&&col.bounds.min.y<.2f&&col.bounds.size.y>2)walls.Add(col);
   walls.Sort((a,b)=>{int n=a.bounds.center.z.CompareTo(b.bounds.center.z);return n!=0?n:a.bounds.center.x.CompareTo(b.bounds.center.x);});
   // Each map uses a different order, with one of each type at most; no repeated corridor template.
   var types=annex?new[]{CorridorType277.FireCabinet,CorridorType277.CleaningCart,CorridorType277.TransportCart,CorridorType277.Bench,CorridorType277.PaperBoxes,CorridorType277.NoticeBoard}:new[]{CorridorType277.Bench,CorridorType277.FireCabinet,CorridorType277.Planter,CorridorType277.CleaningCart,CorridorType277.TransportCart,CorridorType277.PaperBoxes,CorridorType277.FallenChair,CorridorType277.NoticeBoard};
   for(int index=0;index<types.Length;index++){
    int choice=annex?index:(index+RepairFloor220*2)%types.Length;var type=types[choice];
    string name="Corridor277_"+InteriorId+"_"+type;
    var view=CorridorModel277(type,name);Bounds local=CorridorBounds277(view);
    bool placed=false;
    for(int wi=0;wi<walls.Count&&!placed;wi++){
     var wall=walls[(wi+index*3)%walls.Count].bounds;bool horizontal=wall.size.x>wall.size.z;
     foreach(float fraction in new[]{.5f,.25f,.75f,.12f,.88f})foreach(int sign in new[]{1,-1}){
      if(placed)break;
      var surface=horizontal?new Vector3(Mathf.Lerp(wall.min.x,wall.max.x,fraction),0,sign>0?wall.max.z:wall.min.z):new Vector3(sign>0?wall.max.x:wall.min.x,0,Mathf.Lerp(wall.min.z,wall.max.z,fraction));
      Vector3 inward=(horizontal?Vector3.forward:Vector3.right)*sign;int side=horizontal?(sign>0?2:0):(sign>0?3:1);
      var rotation=Quaternion.Euler(0,side*90,0);var position=surface+inward*(local.max.z+.08f);
      Rect footprint=CorridorFootprint277(local,position,rotation),publicArea;
      if(!CorridorPublic277(areas,footprint,annex,out publicArea)||CorridorProtected277(footprint,annex)||!CorridorClear277(footprint,side,surface,view))continue;
      if(horizontal&&(footprint.xMin<wall.min.x+.12f||footprint.xMax>wall.max.x-.12f)||!horizontal&&(footprint.yMin<wall.min.z+.12f||footprint.yMax>wall.max.z-.12f))continue;
      if(CorridorWindow277(surface,annex))continue;
      view.transform.SetPositionAndRotation(position,rotation);
      CorridorItems277.Add(new CorridorItem277{Name=name,Type=type,Anchor=position,BackingPoint=surface,BackingSide=side,Footprint=footprint,PublicArea=publicArea,View=view,WallMounted=type==CorridorType277.FireCabinet||type==CorridorType277.NoticeBoard,Kind=type==CorridorType277.NoticeBoard||type==CorridorType277.FireCabinet?ObstacleKind.Decoration:type==CorridorType277.CleaningCart?ObstacleKind.High:ObstacleKind.Low});
      placed=true;Physics.SyncTransforms();
     }
    }
    if(!placed){view.SetActive(false);Object.Destroy(view);}
   }
   Physics.SyncTransforms();
  }
  bool CorridorPublic277(Rect[] areas,Rect footprint,bool annex,out Rect area){
   area=new Rect();
   if(annex){foreach(var room in RepairRooms200)if(footprint.Overlaps(room.Area))return false;}
   else foreach(var room in RepairMainRooms210)if(footprint.Overlaps(room.Area))return false;
   foreach(var candidate in areas)if(footprint.xMin>=candidate.xMin&&footprint.xMax<=candidate.xMax&&footprint.yMin>=candidate.yMin&&footprint.yMax<=candidate.yMax){area=candidate;return true;}
   return false;
  }
  bool CorridorProtected277(Rect area,bool annex){
   foreach(var lane in CorridorLanes277)if(area.Overlaps(DecorationExpand274(lane.Area,.08f)))return true;
   foreach(var bridge in CorridorProtectedConnections277)if(area.Overlaps(DecorationExpand274(bridge,.08f)))return true;
   if(DecorationNear274(area,Spawn,1.8f))return true;
   foreach(var door in Doors){
    if(DecorationNear274(area,door.Position,door.Portal?2:1.8f))return true;
    if(!door.Portal&&(area.Overlaps(DecorationExpand274(door.OpenSafety,.45f))||area.Overlaps(DecorationExpand274(door.ClosedSafety,.45f))))return true;
   }
   foreach(var box in Loot){
    if(DecorationNear274(area,box.Position,1.35f))return true;
    if(box.Prop&&DecorationSegment274(area,box.Position,box.Prop.transform.position,.75f))return true;
   }
   if(annex){
    // This map's accepted fixed soldier route is populated after its geometry is built.
    var home=new Vector3(-1,0,3);var a=new Vector3(-1,0,1);var b=new Vector3(1,0,4.5f);
    if(DecorationSegment274(area,home,a,1.05f)||DecorationSegment274(area,home,b,1.05f)||DecorationSegment274(area,a,b,1.05f))return true;
   }else{
    if(RepairFloor220==1&&area.Overlaps(DecorationExpand274(RepairMainInspection210,.45f)))return true;
    if(DecorationNear274(area,RepairStairArrival220(true),2.1f)||DecorationNear274(area,RepairStairArrival220(false),2.1f))return true;
    foreach(var point in RepairMainWalkPoints210)if(DecorationNear274(area,point,1.1f))return true;
    foreach(var room in RepairMainRooms210)if(DecorationNear274(area,room.ApproachPoint,1.1f)||DecorationNear274(area,room.WalkPoint,1.1f))return true;
    foreach(var plan in MainPatrolPlans276){
     if(DecorationNear274(area,plan.Home,1.05f))return true;var previous=plan.Home;
     if(plan.Targets!=null)foreach(var target in plan.Targets){if(DecorationSegment274(area,previous,target,1.05f)||DecorationSegment274(area,plan.Home,target,1.05f))return true;previous=target;}
     if(DecorationSegment274(area,previous,plan.Home,1.05f))return true;
    }
   }
   return false;
  }
  bool CorridorClear277(Rect footprint,int side,Vector3 wall,GameObject prototype){
   var area=DecorationExpand274(footprint,.28f);
   if(side==0)area.yMax=Mathf.Min(area.yMax,wall.z-.025f);else if(side==1)area.xMax=Mathf.Min(area.xMax,wall.x-.025f);else if(side==2)area.yMin=Mathf.Max(area.yMin,wall.z+.025f);else area.xMin=Mathf.Max(area.xMin,wall.x+.025f);
   foreach(var col in Root.GetComponentsInChildren<Collider>()){
    if(!col.enabled||!col.gameObject.activeInHierarchy||col.transform.IsChildOf(prototype.transform)||(SceneObstacle.MovementMask&(1<<col.gameObject.layer))==0)continue;
    var b=col.bounds;if(b.max.y<.15f)continue;
    if(area.Overlaps(new Rect(b.min.x,b.min.z,b.size.x,b.size.z)))return false;
   }
   foreach(var item in MainDecorationItems274)if(area.Overlaps(DecorationExpand274(item.Footprint,.15f)))return false;
   foreach(var item in CorridorItems277)if(area.Overlaps(DecorationExpand274(item.Footprint,.45f)))return false;
   return true;
  }
  bool CorridorWindow277(Vector3 wall,bool annex){
   if(annex)return Mathf.Abs(wall.x+8.79f)<.3f&&Mathf.Abs(wall.z-3)<2.1f;
   foreach(var window in RepairMainWindowPoints276(RepairFloor220))if(Mathf.Abs(wall.x-window.x)<.3f&&Mathf.Abs(wall.z-window.z)<(RepairFloor220==1?1.6f:1.35f))return true;
   return false;
  }
  static Bounds CorridorBounds277(GameObject view){
   var bounds=new Bounds();bool any=false;
   foreach(var r in view.GetComponentsInChildren<Renderer>()){if(!any){bounds=r.bounds;any=true;}else bounds.Encapsulate(r.bounds);}
   return bounds;
  }
  static Rect CorridorFootprint277(Bounds b,Vector3 p,Quaternion rotation){
   float minX=float.PositiveInfinity,minZ=minX,maxX=float.NegativeInfinity,maxZ=maxX;
   foreach(float x in new[]{b.min.x,b.max.x})foreach(float z in new[]{b.min.z,b.max.z}){var q=p+rotation*new Vector3(x,0,z);minX=Mathf.Min(minX,q.x);maxX=Mathf.Max(maxX,q.x);minZ=Mathf.Min(minZ,q.z);maxZ=Mathf.Max(maxZ,q.z);}
   return Rect.MinMaxRect(minX,minZ,maxX,maxZ);
  }
  static void CorridorCube277(WorkshopWastelandArt art,Transform root,string name,string color,Vector3 p,Vector3 size,ObstacleKind kind=ObstacleKind.Low,bool physical=true){
   art.Cube(name,color,p,size);
   if(!physical)return;var body=new GameObject(name+"Collider277");body.transform.SetParent(root,false);body.transform.localPosition=p;body.AddComponent<BoxCollider>().size=size;SceneObstacle.Apply(body,kind);
  }
  GameObject CorridorModel277(CorridorType277 type,string name){
   var view=new GameObject(name);view.transform.SetParent(Root,false);var art=new WorkshopWastelandArt(this);var root=view.transform;
   if(type==CorridorType277.FireCabinet){
    CorridorCube277(art,root,"FireEquipmentCase277","985B48",new Vector3(0,1.55f,0),new Vector3(.72f,.8f,.22f),ObstacleKind.Decoration,false);
    art.Cube("FireCabinetInset277","C08C69",new Vector3(0,1.55f,-.116f),new Vector3(.60f,.65f,.012f));
    art.Cube("FireCabinetGlass277","667C70",new Vector3(0,1.56f,-.125f),new Vector3(.45f,.48f,.012f));
    art.Cube("ExtinguisherSilhouette277","BF745B",new Vector3(-.08f,1.54f,-.135f),new Vector3(.15f,.37f,.012f));
    art.Cube("FireCabinetHandle277","A6AB91",new Vector3(.29f,1.55f,-.15f),new Vector3(.035f,.19f,.035f));
    art.Cube("FireSafetyCross277","D5CBA5",new Vector3(0,2.05f,-.05f),new Vector3(.29f,.08f,.025f));art.Cube("FireSafetyCross277","D5CBA5",new Vector3(0,2.05f,-.05f),new Vector3(.08f,.29f,.025f));
   }else if(type==CorridorType277.Bench){
    CorridorCube277(art,root,"WaitingBenchSeat277","92846A",new Vector3(0,.44f,0),new Vector3(1.85f,.09f,.62f));
    CorridorCube277(art,root,"WaitingBenchBack277","7D826B",new Vector3(0,.74f,.265f),new Vector3(1.85f,.47f,.09f));
    foreach(float x in new[]{-.72f,.72f})foreach(float z in new[]{-.23f,.23f})CorridorCube277(art,root,"WaitingBenchLeg277","4C6254",new Vector3(x,.21f,z),new Vector3(.075f,.42f,.075f));
    art.Cube("BenchWear277","B0A187",new Vector3(.48f,.49f,-.06f),new Vector3(.43f,.012f,.14f));
   }else if(type==CorridorType277.CleaningCart||type==CorridorType277.TransportCart){
    bool cleaning=type==CorridorType277.CleaningCart;float width=cleaning?.94f:1.16f,depth=cleaning?.76f:.72f;
    CorridorCube277(art,root,"CartLowerDeck277","637B68",new Vector3(0,.24f,0),new Vector3(width,.09f,depth));
    foreach(float x in new[]{-.36f,.36f})foreach(float z in new[]{-.26f,.26f})CorridorCube277(art,root,"CartRubberWheel277","34483E",new Vector3(x,.12f,z),new Vector3(.12f,.24f,.16f));
    if(cleaning){
     CorridorCube277(art,root,"CleaningCartBody277","7B927C",new Vector3(-.13f,.57f,.025f),new Vector3(.56f,.60f,.62f));
     CorridorCube277(art,root,"CleaningCartTop277","A0A487",new Vector3(0,.88f,0),new Vector3(width,.06f,depth));
     CorridorCube277(art,root,"CleaningBucket277","789291",new Vector3(.29f,.53f,-.04f),new Vector3(.26f,.48f,.40f));
     foreach(float x in new[]{-.36f,.36f})CorridorCube277(art,root,"CleaningCartHandle277","4C6659",new Vector3(x,.92f,.30f),new Vector3(.045f,.64f,.045f),ObstacleKind.High);
     CorridorCube277(art,root,"CleaningCartHandleBar277","708371",new Vector3(0,1.23f,.30f),new Vector3(.76f,.045f,.045f),ObstacleKind.High);
     art.Cube("CleaningBottle277","B4B78F",new Vector3(-.20f,1.03f,-.08f),new Vector3(.12f,.24f,.12f));art.Cube("FoldedCloth277","ABA482",new Vector3(.10f,.94f,.05f),new Vector3(.26f,.055f,.25f));
    }else{
     foreach(float x in new[]{-.49f,.49f})CorridorCube277(art,root,"TransportCartHandle277","526D5B",new Vector3(x,.63f,.30f),new Vector3(.055f,.77f,.055f));
     CorridorCube277(art,root,"TransportCartBar277","8A977F",new Vector3(0,1.0f,.30f),new Vector3(1.03f,.055f,.055f));
     CorridorCube277(art,root,"UnsearchableDeliveryParcel277","A08C69",new Vector3(-.18f,.50f,-.03f),new Vector3(.58f,.43f,.53f));
     CorridorCube277(art,root,"UnsearchableDeliveryParcel277","8E815F",new Vector3(.30f,.43f,-.04f),new Vector3(.29f,.29f,.40f));
     art.Cube("DeliveryParcelTape277","B8AA87",new Vector3(-.18f,.722f,-.03f),new Vector3(.065f,.012f,.53f));
    }
   }else if(type==CorridorType277.Planter){
    CorridorCube277(art,root,"HallPlantPot277","8C8067",new Vector3(0,.28f,0),new Vector3(.56f,.56f,.56f));
    art.Cube("PlantSoil277","485A48",new Vector3(0,.565f,0),new Vector3(.48f,.018f,.48f));
    art.Tube("PlantStem277","63765B",new Vector3(0,.55f,0),new Vector3(.04f,1.22f,0),.027f);
    for(int i=0;i<5;i++){float sign=i%2==0?1:-1;art.Cube("HallPlantLeaves277",i%2==0?"718564":"8D9572",new Vector3(sign*.14f,.76f+i*.12f,sign*.08f),new Vector3(.26f,.055f,.22f));}
   }else if(type==CorridorType277.PaperBoxes){
    DecorationPaper274(art);
    CorridorCollider277(root,"OpenPaperBox277",new Vector3(-.16f,.18f,0),new Vector3(.55f,.36f,.52f));
    CorridorCollider277(root,"PaperFolderStack277",new Vector3(-.16f,.40f,.02f),new Vector3(.38f,.075f,.31f));
    CorridorCollider277(root,"ClosedPaperBox277",new Vector3(.29f,.12f,-.13f),new Vector3(.32f,.24f,.35f));
   }else if(type==CorridorType277.FallenChair){DecorationChair274(art);CorridorChair277(root);}
   else DecorationNotice274(art);
   art.Bake(view,view.AddComponent<WorkshopArtResources>());
   foreach(var renderer in view.GetComponentsInChildren<Renderer>())renderer.shadowCastingMode=type==CorridorType277.NoticeBoard?ShadowCastingMode.Off:ShadowCastingMode.On;
   return view;
  }
  static void CorridorCollider277(Transform root,string name,Vector3 p,Vector3 size,Quaternion? rotation=null){
   var body=new GameObject(name+"Collider277");body.transform.SetParent(root,false);body.transform.localPosition=p;if(rotation.HasValue)body.transform.localRotation=rotation.Value;body.AddComponent<BoxCollider>().size=size;SceneObstacle.Apply(body,ObstacleKind.Low);
  }
  static void CorridorChair277(Transform root){
   var tilt=Quaternion.Euler(80,0,-12);var parts=new List<Vector3>{Vector3.zero,new Vector3(0,.36f,.23f)};var sizes=new List<Vector3>{new Vector3(.54f,.065f,.50f),new Vector3(.54f,.49f,.065f)};
   foreach(float x in new[]{-.23f,.23f})foreach(float z in new[]{-.21f,.21f}){parts.Add(new Vector3(x,-.22f,z));sizes.Add(new Vector3(.055f,.44f,.055f));}
   float minY=float.PositiveInfinity;
   for(int i=0;i<parts.Count;i++)foreach(float x in new[]{-1f,1f})foreach(float y in new[]{-1f,1f})foreach(float z in new[]{-1f,1f})minY=Mathf.Min(minY,(tilt*(parts[i]+Vector3.Scale(sizes[i],new Vector3(x,y,z))*.5f)).y);
   var origin=new Vector3(0,.01f-minY,0);for(int i=0;i<parts.Count;i++)CorridorCollider277(root,"FallenChairPart277",origin+tilt*parts[i],sizes[i],tilt);
  }
 }
}
