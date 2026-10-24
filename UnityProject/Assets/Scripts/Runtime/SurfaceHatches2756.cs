using System.Collections.Generic;using UnityEngine;using UnityEngine.Rendering;
namespace GreyHarbor {public sealed partial class RaidMap {
 public readonly List<GameObject> UndergroundHatchViews2756=new List<GameObject>();
 void BuildOpenSurfaceHatch2756(Vector3 p,int index){
  bool round=index==0||index==2||index==5;var view=new GameObject("OpenSurfaceHatch2756_"+UndergroundSurfaceIds2752[index]);view.transform.SetParent(Root,false);
  var art=new WorkshopWastelandArt(this);var owner=view.AddComponent<WorkshopArtResources>();
  if(round){HatchDisk2756(view,Vector3.up*.068f,.59f,"121C18",owner);for(int i=0;i<20;i++){float a=i*Mathf.PI/10,b=(i+1)*Mathf.PI/10;art.Tube("Rim","6F7968",new Vector3(Mathf.Cos(a)*.65f,.076f,Mathf.Sin(a)*.65f),new Vector3(Mathf.Cos(b)*.65f,.076f,Mathf.Sin(b)*.65f),.055f);}
   HatchDisk2756(view,new Vector3(.89f,.12f,.28f),.57f,"54645B",owner);for(int i=0;i<6;i++)art.Stroke("LidGrain","34453B",new Vector3(.52f+i*.13f,.123f,-.12f),new Vector3(.52f+i*.13f,.123f,.64f),.022f,Vector3.up,false);
   art.Cube("LidHandle","9C8A64",new Vector3(.9f,.16f,.28f),new Vector3(.19f,.04f,.045f));
  }else{
   art.Floor("Opening","121C18",0,0,1.12f,1.35f,.068f);
   foreach(float x in new[]{-.60f,.60f})art.Cube("Rim","6E7A6B",new Vector3(x,.09f,0),new Vector3(.09f,.14f,1.47f));
   foreach(float z in new[]{-.72f,.72f})art.Cube("Rim","6E7A6B",new Vector3(0,.09f,z),new Vector3(1.28f,.14f,.09f));
   var up=new Vector3(.28f,.96f,0);art.Panel("OpenCover","596E63",new Vector3(.89f,.6f,0),Vector3.forward,up,1.38f,1.13f);
   art.Panel("CoverInset","394D43",new Vector3(.868f,.60f,0),Vector3.forward,up,1.12f,.84f);
   for(int i=0;i<5;i++)art.Tube("CoverRibs","8B8468",new Vector3(.78f,.22f,-.5f+i*.25f),new Vector3(1.00f,.98f,-.5f+i*.25f),.025f);
   foreach(float z in new[]{-.50f,.50f})art.Cube("CoverHinge","9B7450",new Vector3(.60f,.15f,z),new Vector3(.16f,.12f,.12f));
  }
  // Flattened visible descending ladder and inner ledge depict depth without floor collision changes.
  foreach(float x in new[]{-.21f,.21f})art.Tube("Ladder","A08E67",new Vector3(x,.108f,.41f),new Vector3(x,.080f,-.25f),.027f);
  for(int j=0;j<5;j++)art.Tube("LadderRung","A08E67",new Vector3(-.21f,.11f-j*.006f,.35f-j*.12f),new Vector3(.21f,.11f-j*.006f,.35f-j*.12f),.018f);
  art.Stroke("InnerLip","354A3E",new Vector3(-.38f,.077f,.48f),new Vector3(.38f,.077f,.48f),.09f,Vector3.up,false);
  art.Bake(view,owner);view.transform.position=p;UndergroundHatchViews2756.Add(view);
 }
 void HatchDisk2756(GameObject parent,Vector3 center,float radius,string color,WorkshopArtResources life){
  var vertices=new List<Vector3>{center};var triangles=new List<int>();for(int i=0;i<20;i++){float angle=i*Mathf.PI/10;vertices.Add(center+new Vector3(Mathf.Cos(angle),0,Mathf.Sin(angle))*radius);}
  for(int i=0;i<20;i++){int a=1+i,b=1+(i+1)%20;triangles.Add(0);triangles.Add(b);triangles.Add(a);}
  var mesh=new Mesh{name="InspectionOpeningDisk2756"};mesh.SetVertices(vertices);mesh.SetTriangles(triangles,0);mesh.RecalculateNormals();mesh.RecalculateBounds();life.Meshes.Add(mesh);
  var disk=new GameObject("HatchOpeningDisk2756");disk.transform.SetParent(parent.transform,false);disk.AddComponent<MeshFilter>().sharedMesh=mesh;var r=disk.AddComponent<MeshRenderer>();r.sharedMaterial=Mat(color);r.shadowCastingMode=ShadowCastingMode.Off;
 }
}}
