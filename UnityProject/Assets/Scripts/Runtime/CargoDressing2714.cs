using System;
using System.Collections.Generic;
using UnityEngine;
namespace GreyHarbor {
 public sealed class CargoDressingRecord2714 {public string Type;public GameObject View;public Vector3 Anchor;public Rect Footprint;public ObstacleKind Kind;public string Group;}
 public sealed class CargoLootRecord2714 {public LootContainer Container;public GameObject Model;public Vector3 Stand,AccessPoint;public float Yaw;public ContainerLook Look;public bool Safe;}
 public sealed partial class RaidMap {
  public readonly List<GameObject> CargoDressingViews2714=new List<GameObject>();
  public readonly List<CargoDressingRecord2714> CargoDressingRecords2714=new List<CargoDressingRecord2714>();
  public readonly List<LootContainer> CargoLoot2714=new List<LootContainer>();
  public readonly List<CargoLootRecord2714> CargoLootRecords2714=new List<CargoLootRecord2714>();
  // All candidate anchors are fixed design coordinates. Physics only rejects occupied
  // pockets; it never moves a maze box, narrows a reserved lane, or randomizes scenery.
  // Accepted 2.7.14 live positions, including its two nearest-pocket fallbacks.
  readonly Vector4[] cargoLootPlan2714={new Vector4(85,89,90,0),new Vector4(75,109,90,1),new Vector4(76,118,90,2),new Vector4(78,65.5f,90,3),new Vector4(82,106,0,4),new Vector4(106,86,90,5),new Vector4(101,116.5f,180,6),new Vector4(109,79,270,7),new Vector4(109,121,180,2),new Vector4(108,104,0,8)};
  public void BuildCargoDressing2714(){
   var rng=new System.Random((RaidGame.Instance?RaidGame.Instance.RaidSeed:2714)^2714);Physics.SyncTransforms();
   foreach(var p in cargoLootPlan2714){bool made=CargoAddLoot2714(p.x,p.y,p.z,(int)p.w,rng);if(!made){foreach(var candidate in CargoCandidates2714(new Vector3(p.x,p.y,p.z))){if(CargoAddLoot2714(candidate.x,candidate.y,candidate.z,(int)p.w,rng)){made=true;break;}}}if(!made)Debug.LogWarning("Cargo2714 no safe pocket for loot type "+p.w);}
   string[] types={"Forklift","WrappedLoad","Pallets","BarrelRack","RopeCoil","Tires","Planks","Packaging","Spool","PalletJack","WrappedLoad","Packaging"};
   Vector3[] anchors={new Vector3(84,103,90),new Vector3(118.5f,81,0),new Vector3(67,67.5f,0),new Vector3(100,73.5f,0),new Vector3(81.5f,115.5f,90),new Vector3(68,124,0),new Vector3(78,111,15),new Vector3(88,83,0),new Vector3(117,108,0),new Vector3(112,93,0),new Vector3(113,117,0),new Vector3(75,72,0)};
   for(int i=0;i<types.Length;i++){string type=types[i];bool made=false;foreach(var candidate in CargoCandidates2714(anchors[i]))if(CargoAddDecor2714(type,candidate.x,candidate.y,candidate.z)){made=true;break;}if(!made)Debug.LogWarning("Cargo2714 no safe pocket for "+type);}
   BuildCargoDensity2715(rng);Physics.SyncTransforms();
  }
  IEnumerable<Vector3> CargoCandidates2714(Vector3 preferred){
   // Try the authored district pocket first, then its nearest fixed grid anchors.
   // This retains a stable distribution instead of filling the southern edge first.
   yield return preferred;var candidates=new List<Vector3>();
   for(int z=64;z<=126;z+=3)for(int x=64;x<=123;x+=3)for(int yaw=0;yaw<360;yaw+=90)candidates.Add(new Vector3(x,z,yaw));
   candidates.Sort((a,b)=>CargoAnchorDistance2714(a,preferred).CompareTo(CargoAnchorDistance2714(b,preferred)));
   foreach(var candidate in candidates)yield return candidate;
  }
  static float CargoAnchorDistance2714(Vector3 p,Vector3 target)=>(p.x-target.x)*(p.x-target.x)+(p.y-target.y)*(p.y-target.y)+Mathf.Abs(Mathf.DeltaAngle(p.z,target.z))*.0001f;
  bool CargoAddLoot2714(float x,float z,float yaw,int type,System.Random rng){
   var look=type==0||type==1?ContainerLook.CargoCrate:type==2?ContainerLook.OutdoorTote:type==3?ContainerLook.GardenChest:type==4?ContainerLook.Toolbox:type==5?ContainerLook.WaterproofTransitCase:type==6?ContainerLook.ElectricalCabinet:type==7?ContainerLook.FuelDrum:ContainerLook.Safe;
   bool safe=type==8;Vector3 model=PortPoint(x,z),stand=model+Quaternion.Euler(0,yaw,0)*Vector3.back*1.3f;
   Vector2 size=type==0?new Vector2(2,1.6f):type==1?new Vector2(1.4f,1.1f):type==3?new Vector2(1.8f,.8f):type==6?new Vector2(1.2f,.9f):new Vector2(1.2f,1.1f);
   Rect envelope=CargoRect2714(model,size,yaw);Vector3 access;if(!CargoPocket2714(envelope,null)||!CargoAccess2714(stand,out access))return false;
   foreach(var old in CargoLootRecords2714)if(Vector3.Distance(old.Stand,stand)<3.2f)return false;
   var box=AddContainer(stand,model,look,safe?SupplyType.Safe:SupplyType.Misc,rng,HarborDistrict.Containers,yaw,false,safe?ContainerTier.Safe:ContainerTier.Common,DropRegion230.Containers,safe?SafeSize240.Small:SafeSize240.Legacy);
   if(safe)box.Name="小型保险箱";if(type==0){box.Name="散落货物堆";CargoScatter2714(box.View);}
   Physics.SyncTransforms();Rect footprint=CargoBounds2714(box.View);if(!CargoPocket2714(footprint,box.View)||!CargoAccess2714(stand,out access)){Loot.Remove(box);box.View.SetActive(false);TransientObjects2721.Release(box.View);return false;}
   CargoLoot2714.Add(box);CargoLootRecords2714.Add(new CargoLootRecord2714{Container=box,Model=box.View,Stand=stand,AccessPoint=access,Yaw=yaw,Look=look,Safe=safe});return true;
  }
  Rect CargoRect2714(Vector3 p,Vector2 s,float yaw){float a=yaw*Mathf.Deg2Rad,w=Mathf.Abs(Mathf.Cos(a))*s.x+Mathf.Abs(Mathf.Sin(a))*s.y,d=Mathf.Abs(Mathf.Sin(a))*s.x+Mathf.Abs(Mathf.Cos(a))*s.y;return new Rect(p.x-w/2,p.z-d/2,w,d);}
  Rect CargoBounds2714(GameObject view){bool any=false;Bounds bounds=new Bounds();foreach(var r in view.GetComponentsInChildren<Renderer>()){if(!any){bounds=r.bounds;any=true;}else bounds.Encapsulate(r.bounds);}foreach(var c in view.GetComponentsInChildren<Collider>())if(c.enabled){if(!any){bounds=c.bounds;any=true;}else bounds.Encapsulate(c.bounds);}return new Rect(bounds.min.x,bounds.min.z,bounds.size.x,bounds.size.z);}
  float CargoSegmentRect2714(Vector3 a,Vector3 b,Rect r){
   Vector2 p=new Vector2(a.x,a.z),q=new Vector2(b.x,b.z);if(r.Contains(p)||r.Contains(q))return 0;
   Vector2 c0=new Vector2(r.xMin,r.yMin),c1=new Vector2(r.xMax,r.yMin),c2=new Vector2(r.xMax,r.yMax),c3=new Vector2(r.xMin,r.yMax);
   return Mathf.Min(CargoEdgeDistance2715(p,q,c0,c1),CargoEdgeDistance2715(p,q,c1,c2),CargoEdgeDistance2715(p,q,c2,c3),CargoEdgeDistance2715(p,q,c3,c0));
  }
  bool CargoPocket2714(Rect area,GameObject own){
   var bounds=CargoBounds2712;if(area.xMin<bounds.xMin+.05f||area.xMax>bounds.xMax-.05f||area.yMin<bounds.yMin+.05f||area.yMax>bounds.yMax-.05f)return false;
   foreach(var lane in CargoLanes2712)for(int i=1;i<lane.Points.Length;i++)if(CargoSegmentRect2714(lane.Points[i-1],lane.Points[i],area)<lane.Width/2+.16f)return false;
   foreach(var loot in CargoLootRecords2714){if(area.Contains(new Vector2(loot.Stand.x,loot.Stand.z))||CargoSegmentRect2714(loot.Stand,loot.AccessPoint,area)<.45f)return false;}
   var center=new Vector3(area.center.x,1.25f,area.center.y);foreach(var c in Physics.OverlapBox(center,new Vector3(area.width/2+.08f,1.22f,area.height/2+.08f),Quaternion.identity,SceneObstacle.MovementMask,QueryTriggerInteraction.Ignore))if(own==null||!c.transform.IsChildOf(own.transform))return false;
   return true;
  }
  bool CargoAccess2714(Vector3 stand,out Vector3 access){
   access=stand;Vector3 bottom=stand+Vector3.up*.36f,top=stand+Vector3.up*1.35f;if(Physics.CheckCapsule(bottom,top,.34f,SceneObstacle.MovementMask,QueryTriggerInteraction.Ignore))return false;
   float best=float.MaxValue;bool found=false;foreach(var lane in CargoLanes2712)for(int i=1;i<lane.Points.Length;i++){Vector3 a=lane.Points[i-1],b=lane.Points[i],d=b-a;Vector3 p=a+d*Mathf.Clamp01(Vector3.Dot(stand-a,d)/d.sqrMagnitude);Vector3 delta=p-stand;float distance=delta.magnitude;if(distance>best||distance>8)continue;if(distance>.001f&&Physics.CapsuleCast(bottom,top,.34f,delta/distance,distance,SceneObstacle.MovementMask,QueryTriggerInteraction.Ignore))continue;best=distance;access=p;found=true;}return found;
  }
  bool CargoAddDecor2714(string type,float x,float z,float yaw){
   Vector3 anchor=PortPoint(x,z);Vector2 size=CargoDecorSize2715(type);
   if(!CargoPocket2714(CargoRect2714(anchor,size,yaw),null))return false;
   GameObject view;var kit=new DockLoadingKit142(this);ObstacleKind kind=type=="Forklift"||type=="WrappedLoad"||type=="CrateStack"||type=="ScrapRack"?ObstacleKind.High:ObstacleKind.Low;
   if(type=="WrappedLoad")view=kit.WrappedLoad(anchor,yaw).gameObject;else if(type=="BarrelRack")view=kit.BarrelRack(anchor,yaw).gameObject;else if(type=="Spool")view=kit.SpoolStack(anchor,yaw).gameObject;else if(type=="PalletJack")view=kit.PalletJack(anchor,yaw).gameObject;else if(type=="PipeBundle")view=kit.PipeBundle(anchor,yaw).gameObject;else if(type=="CrateStack"||type=="ScrapRack"||type=="TimberStack")view=CargoFreightModel2715(type,anchor,yaw);else view=CargoSimple2714(type,anchor,yaw);
   view.name="CargoDressing2714_"+type+"_"+CargoDressingViews2714.Count;Physics.SyncTransforms();Rect area=CargoBounds2714(view);
   if(!CargoPocket2714(area,view)){view.SetActive(false);TransientObjects2721.Release(view);return false;}
   CargoDressingViews2714.Add(view);CargoDressingRecords2714.Add(new CargoDressingRecord2714{Type=type,View=view,Anchor=anchor,Footprint=area,Kind=kind});return true;
  }
  GameObject CargoSimple2714(string type,Vector3 anchor,float yaw){
   var root=new GameObject("Cargo2714_"+type);root.transform.SetParent(Root,false);root.transform.position=anchor;
   if(type=="Forklift"){
    CargoCube2714(root,"Chassis",new Vector3(-.565f,.55f,0),new Vector3(2.7f,1.1f,1.72f),"9E8255",ObstacleKind.High);
    foreach(float z in new[]{-.55f,.55f}){CargoCube2714(root,"Mast",new Vector3(.915f,1.55f,z),new Vector3(.14f,2.4f,.14f),"536758",ObstacleKind.High);CargoCube2714(root,"Fork",new Vector3(1.365f,.14f,z),new Vector3(1.1f,.10f,.16f),"929581",ObstacleKind.Low);}
    foreach(float x in new[]{-1.1f,.05f})foreach(float z in new[]{-.67f,.67f})CargoCube2714(root,"GuardPost",new Vector3(x,1.76f,z),new Vector3(.07f,1.3f,.07f),"536758",ObstacleKind.High);
    CargoCube2714(root,"GuardRoof",new Vector3(-.53f,2.44f,0),new Vector3(1.35f,.1f,1.45f),"748376",ObstacleKind.High);CargoCube2714(root,"Seat",new Vector3(-.7f,1.25f,0),new Vector3(.55f,.25f,.65f),"303E37",ObstacleKind.High);
    foreach(float x in new[]{-1.35f,.34f})foreach(float z in new[]{-.73f,.73f})CargoCylinder2714(root,"Wheel",new Vector3(x,.28f,z),.28f,.18f,"303E37",Quaternion.Euler(90,0,0),ObstacleKind.Low);
   }else if(type=="Pallets"){
    for(int layer=0;layer<2;layer++){for(int i=0;i<5;i++)CargoCube2714(root,"TimberSlat",new Vector3(-.58f+i*.29f,.12f+layer*.2f,0),new Vector3(.22f,.07f,1.2f),"A3926E",ObstacleKind.Low);foreach(float z in new[]{-.45f,0,.45f})CargoCube2714(root,"Skid",new Vector3(0,.045f+layer*.2f,z),new Vector3(1.35f,.09f,.12f),"807454",ObstacleKind.Low);}
   }else if(type=="Planks"){
    for(int i=0;i<4;i++)CargoCube2714(root,"WastePlank",new Vector3(0,.045f+i*.065f,-.3f+i*.2f),new Vector3(2.2f,.09f,.16f),i%2==0?"938366":"756A50",ObstacleKind.Low);
   }else if(type=="Tires"){
    foreach(float x in new[]{-.38f,.38f})CargoRing2714(root,"OldTire",new Vector3(x,.14f,0),.26f,.14f,"303E37");CargoRing2714(root,"StackedTire",new Vector3(-.38f,.42f,0),.26f,.14f,"39443B");
   }else if(type=="RopeCoil"){
    for(int i=0;i<3;i++)CargoRing2714(root,"MooringRope",new Vector3(-.15f,.035f+i*.045f,0),.36f-i*.03f,.025f,"A29A75");CargoCube2714(root,"WoodCleat",new Vector3(.4f,.14f,.15f),new Vector3(.45f,.28f,.35f),"857857",ObstacleKind.Low);
   }else{
    CargoCube2714(root,"BrokenCarton",new Vector3(-.45f,.23f,-.17f),new Vector3(.58f,.46f,.54f),"968263",ObstacleKind.Low);CargoCube2714(root,"PackingBox",new Vector3(.4f,.15f,.21f),new Vector3(.63f,.30f,.56f),"AA9977",ObstacleKind.Low);CargoCube2714(root,"FlattenedPackaging",new Vector3(-.09f,.07f,.4f),new Vector3(1.08f,.14f,.37f),"82735A",ObstacleKind.Low);CargoCube2714(root,"LooseLabel",new Vector3(.4f,.304f,.21f),new Vector3(.25f,.008f,.22f),"B9B39A",null);
   }
   root.transform.rotation=Quaternion.Euler(0,yaw,0);return root;
  }
  void CargoScatter2714(GameObject root){CargoCube2714(root,"TornPackingBox",new Vector3(.89f,.13f,0),new Vector3(.45f,.26f,.35f),"9B8766",ObstacleKind.Low);CargoCube2714(root,"DiscardedPlank",new Vector3(-.2f,.025f,.68f),new Vector3(1.3f,.05f,.14f),"817456",ObstacleKind.Low);CargoCube2714(root,"TornPaper",new Vector3(.44f,.017f,.65f),new Vector3(.36f,.025f,.27f),"BAB49B",null);}
  void CargoCube2714(GameObject parent,string name,Vector3 position,Vector3 size,string color,ObstacleKind? kind){var cube=GameObject.CreatePrimitive(PrimitiveType.Cube);cube.name=name;cube.transform.SetParent(parent.transform,false);cube.transform.localPosition=position;cube.transform.localScale=size;cube.GetComponent<Renderer>().sharedMaterial=Mat(color,true);if(kind.HasValue)SceneObstacle.Apply(cube,kind.Value);else{var c=cube.GetComponent<Collider>();c.enabled=false;TransientObjects2721.Release(c);}}
  void CargoCylinder2714(GameObject parent,string name,Vector3 position,float radius,float length,string color,Quaternion rotation,ObstacleKind kind){var c=GameObject.CreatePrimitive(PrimitiveType.Cylinder);c.name=name;c.transform.SetParent(parent.transform,false);c.transform.localPosition=position;c.transform.localRotation=rotation;c.transform.localScale=new Vector3(radius*2,length/2,radius*2);c.GetComponent<Renderer>().sharedMaterial=Mat(color,true);SceneObstacle.Apply(c,kind);if(name=="Wheel"){var rubber=new GameObject("VisibleWheelRubber");rubber.transform.SetParent(c.transform,false);rubber.transform.position=c.transform.position+parent.transform.forward*Mathf.Sign(position.z)*.16f;rubber.AddComponent<MeshFilter>().sharedMesh=c.GetComponent<MeshFilter>().sharedMesh;rubber.AddComponent<MeshRenderer>().sharedMaterial=c.GetComponent<Renderer>().sharedMaterial;c.GetComponent<Renderer>().enabled=false;}}
  void CargoRing2714(GameObject parent,string name,Vector3 center,float radius,float thickness,string color){for(int i=0;i<12;i++){float a=i*Mathf.PI/6,b=(i+1)*Mathf.PI/6;Vector3 p=center+new Vector3(Mathf.Cos(a),0,Mathf.Sin(a))*radius,q=center+new Vector3(Mathf.Cos(b),0,Mathf.Sin(b))*radius;CargoCylinder2714(parent,name,(p+q)/2,thickness,Vector3.Distance(p,q),color,Quaternion.FromToRotation(Vector3.up,q-p),ObstacleKind.Low);}}
 }
}



