using UnityEngine;
using System.Collections.Generic;
namespace GreyHarbor {public sealed partial class RaidMap {
 public GameObject OfficeBlockedCar2746,OfficeBridgeExterior2746,OfficeOutdoorRoot2746;
 GameObject OfficeGroup2746(string name,Vector3 p){var g=new GameObject(name);g.transform.SetParent(Root,false);g.transform.position=p;return g;}
 GameObject OfficePart2746(GameObject parent,string name,Vector3 p,Vector3 size,string color,bool solid=false){var g=Box(name,parent.transform.TransformPoint(p),size,color,solid);g.transform.SetParent(parent.transform,true);g.transform.localRotation=Quaternion.identity;return g;}
 void OfficeBatchExterior2746(GameObject root){
  var groups=new Dictionary<Material,List<CombineInstance>>();var originals=new List<MeshRenderer>();var life=root.AddComponent<WorkshopArtResources>();
  foreach(var f in root.GetComponentsInChildren<MeshFilter>()){var r=f.GetComponent<MeshRenderer>();if(!r||!r.enabled||!f.sharedMesh)continue;var m=r.sharedMaterial;if(!groups.ContainsKey(m))groups[m]=new List<CombineInstance>();groups[m].Add(new CombineInstance{mesh=f.sharedMesh,transform=root.transform.worldToLocalMatrix*f.transform.localToWorldMatrix});originals.Add(r);}
  foreach(var pair in groups){var mesh=new Mesh{name="OfficeOutdoorColour2746"};mesh.indexFormat=UnityEngine.Rendering.IndexFormat.UInt32;mesh.CombineMeshes(pair.Value.ToArray(),true,true);life.Meshes.Add(mesh);var go=new GameObject("OfficeOutdoorBatch2746");go.transform.SetParent(root.transform,false);go.AddComponent<MeshFilter>().sharedMesh=mesh;go.AddComponent<MeshRenderer>().sharedMaterial=pair.Key;}
  foreach(var r in originals)r.enabled=false;
 }
 void BuildOfficeBlockedCar2746(){
  var g=OfficeGroup2746("OfficeBlockedCar2746",PortPoint(70,153.5f));OfficeBlockedCar2746=g;
  OfficePart2746(g,"VanChassis",new Vector3(0,.58f,0),new Vector3(1.8f,.3f,4.8f),"374A49",true);
  OfficePart2746(g,"VanDeliveryBody",new Vector3(0,1.28f,.6f),new Vector3(1.84f,1.5f,3.15f),"5F7C80",true);
  OfficePart2746(g,"VanCab",new Vector3(0,1.23f,-1.53f),new Vector3(1.81f,1.27f,1.32f),"758E8B",true);
  OfficePart2746(g,"VanHood",new Vector3(0,.92f,-2.15f),new Vector3(1.8f,.48f,.62f),"667E79",true);
  OfficePart2746(g,"VanWindshield",new Vector3(0,1.48f,-2.205f),new Vector3(1.51f,.64f,.045f),"304B53");
  foreach(float x in new[]{-.93f,.93f}){
   OfficePart2746(g,"SideWindow",new Vector3(x,1.49f,-1.5f),new Vector3(.04f,.57f,.96f),"3B5960");
   OfficePart2746(g,"SlidingDoorSeam",new Vector3(x,1.32f,.25f),new Vector3(.05f,1.32f,.035f),"344C51");
   OfficePart2746(g,"RustPatch",new Vector3(x,.71f,1.5f),new Vector3(.055f,.16f,.67f),"947658");
   OfficePart2746(g,"DoorHandle",new Vector3(x*1.03f,1.05f,-.7f),new Vector3(.07f,.07f,.28f),"C0BA9A");
   foreach(float z in new[]{-1.65f,1.55f}){var w=GameObject.CreatePrimitive(PrimitiveType.Cylinder);w.name="VanWheel2746";w.transform.SetParent(g.transform,false);w.transform.localPosition=new Vector3(x,.5f,z);w.transform.localRotation=Quaternion.Euler(0,0,90);w.transform.localScale=new Vector3(.78f,.15f,.78f);w.GetComponent<Renderer>().sharedMaterial=Mat("263633",true);SceneObstacle.Apply(w,ObstacleKind.High);}
  }
  foreach(float z in new[]{-2.51f,2.21f})OfficePart2746(g,"VanBumper",new Vector3(0,.61f,z),new Vector3(1.94f,.21f,.14f),"838B7D");
  foreach(float z in new[]{-1.65f,1.55f})OfficePart2746(g,"UndersideAxle",new Vector3(0,.45f,z),new Vector3(1.89f,.12f,.12f),"8B9588");
  OfficePart2746(g,"Exhaust",new Vector3(.49f,.4f,.45f),new Vector3(.12f,.12f,2.3f),"887964");
  g.transform.rotation=Quaternion.Euler(0,90,83);Physics.SyncTransforms();float min=float.MaxValue;foreach(var r in g.GetComponentsInChildren<Renderer>())min=Mathf.Min(min,r.bounds.min.y);g.transform.position+=Vector3.up*(.06f-min);Physics.SyncTransforms();var envelope=new Bounds();bool first=true;foreach(var r in g.GetComponentsInChildren<Renderer>()){if(first){envelope=r.bounds;first=false;}else envelope.Encapsulate(r.bounds);}var target=PortPoint(70,153.6f);g.transform.position+=new Vector3(target.x-envelope.center.x,0,target.z-envelope.center.z);OfficeBatchExterior2746(g);
 }
 void BuildOfficeOutdoorDetails2746(){
  var b=OfficeGroup2746("OfficeBridgeExterior2746",PortPoint(81.5f,167.5f));OfficeBridgeExterior2746=b;
  OfficePart2746(b,"BridgeRoof",new Vector3(0,6.28f,0),new Vector3(7.15f,.18f,3.12f),"596F6A");
  foreach(float z in new[]{-1.47f,1.47f}){
   OfficePart2746(b,"BridgeWindowBand",new Vector3(0,5.37f,z),new Vector3(7,1.38f,.07f),"3C6068");
   OfficePart2746(b,"BridgeBottomTrim",new Vector3(0,3.62f,z),new Vector3(7,.24f,.16f),"A5AC98");
   foreach(float x in new[]{-3.45f,-2.3f,-1.15f,0,1.15f,2.3f,3.45f})OfficePart2746(b,"BridgeFrame",new Vector3(x,5.33f,z),new Vector3(.07f,1.74f,.12f),"ADB6A6");
  }
  OfficeBatchExterior2746(b);
  var r=OfficeGroup2746("OfficeOutdoorRoot2746",PortPoint(81.5f,164));OfficeOutdoorRoot2746=r;
  // Front benches are beside the entrances; service equipment is behind the buildings.
  foreach(float x in new[]{-17f,5.5f}){
   var bench=OfficeGroup2746("OfficeFrontBench2746",PortPoint(81.5f+x,152.4f));bench.transform.SetParent(r.transform,true);
   OfficePart2746(bench,"BenchSeat",new Vector3(0,.55f,0),new Vector3(2.1f,.13f,.64f),"907F61",true);
   OfficePart2746(bench,"BenchBack",new Vector3(0,.96f,.26f),new Vector3(2.1f,.65f,.10f),"A08D68");
   foreach(float lx in new[]{-.8f,.8f})OfficePart2746(bench,"BenchLeg",new Vector3(lx,.26f,0),new Vector3(.09f,.52f,.52f),"415B53");
  }
  foreach(float x in new[]{65f,97f}){
   var unit=OfficeGroup2746("OfficeServiceUnit2746",PortPoint(x,174.25f));unit.transform.SetParent(r.transform,true);
   OfficePart2746(unit,"ServiceHousing",new Vector3(0,.85f,0),new Vector3(1.7f,1.7f,1.1f),"647D76",true);
   OfficePart2746(unit,"LouvreFace",new Vector3(0,.91f,-.56f),new Vector3(1.48f,1.33f,.04f),"314D4A");
   for(int i=0;i<8;i++)OfficePart2746(unit,"VentBlade",new Vector3(0,.33f+i*.16f,-.595f),new Vector3(1.44f,.055f,.04f),"91A090");
  }
  var bin=OfficeGroup2746("OfficeRefuseBin2746",PortPoint(60.6f,165.5f));bin.transform.SetParent(r.transform,true);
  OfficePart2746(bin,"RefuseBody",new Vector3(0,.65f,0),new Vector3(1.2f,1.3f,1.1f),"547167",true);
  OfficePart2746(bin,"RefuseLid",new Vector3(0,1.34f,0),new Vector3(1.3f,.12f,1.2f),"829280");
  OfficeBatchExterior2746(r);
 }
}}
