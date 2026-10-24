using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

namespace GreyHarbor {
 public sealed class OfficeFloorHole2748 {
  public Vector3 Center;
  public GameObject View;
  public float TriggerRadius=.65f;
 }
 public sealed partial class RaidMap {
  public readonly List<OfficeFloorHole2748> OfficeFallHoles2748=new List<OfficeFloorHole2748>();
  public void BuildOfficeFloorHole2748(Vector3 center){
   var view=new GameObject("OfficeFloorHole2748");view.transform.SetParent(Root,false);view.transform.position=center;
   var life=view.AddComponent<WorkshopArtResources>();
   var vertices=new List<Vector3>();var triangles=new[]{new List<int>(),new List<int>(),new List<int>()};
   // The raised overlay hides the baked floor without altering the movement plane.
   var inner=new Vector3[16];var outer=new Vector3[16];
   float[] radius={.93f,.86f,1.02f,.9f,.95f,.83f,1.01f,.91f,.98f,.84f,.96f,.86f,1.01f,.89f,.97f,.85f};
   for(int i=0;i<16;i++){
    float angle=i*Mathf.PI/8;float c=Mathf.Cos(angle),s=Mathf.Sin(angle);
    inner[i]=new Vector3(c*radius[i],.072f,s*radius[i]*.84f);
    outer[i]=new Vector3(c*(radius[i]+.17f+(i%3)*.025f),.095f+(i%3)*.011f,s*(radius[i]+.17f+(i%3)*.025f)*.84f);
   }
   for(int i=0;i<16;i++){
    int next=(i+1)%16;
    HoleTri2748(vertices,triangles[0],new Vector3(0,.071f,0),inner[next],inner[i]);
    HoleQuad2748(vertices,triangles[i%3==0?2:1],inner[i],inner[next],outer[next],outer[i]);
    var lowA=inner[i];var lowB=inner[next];lowA.y=.073f;lowB.y=.073f;
    var lipA=inner[i];var lipB=inner[next];lipA.y=.12f;lipB.y=.12f;
    HoleQuad2748(vertices,triangles[2],lowA,lowB,lipB,lipA);
   }
   // Branching hairline cracks and short broken reinforcing rods remain flush.
   for(int i=0;i<6;i++){
    int index=(i*3+1)%16;var from=outer[index];from.y=.125f;
    var direction=new Vector3(from.x,0,from.z).normalized;
    var end=from+direction*(.2f+(i%3)*.065f);end.y=.125f;
    HoleStrip2748(vertices,triangles[2],from,end,.027f);
    var branch=end+new Vector3(direction.z,0,-direction.x)*.13f+direction*.08f;
    HoleStrip2748(vertices,triangles[2],Vector3.Lerp(from,end,.6f),branch,.018f);
   }
   for(int i=0;i<3;i++){
    var edge=inner[i*5+1];edge.y=.128f;
    HoleStrip2748(vertices,triangles[2],edge,edge*.78f+Vector3.up*.028f,.036f);
   }
   var mesh=new Mesh{name="OfficeCrackedFloorMesh2748"};mesh.SetVertices(vertices);mesh.subMeshCount=3;
   for(int i=0;i<3;i++)mesh.SetTriangles(triangles[i],i);mesh.RecalculateNormals();mesh.RecalculateBounds();life.Meshes.Add(mesh);
   view.AddComponent<MeshFilter>().sharedMesh=mesh;
   var renderer=view.AddComponent<MeshRenderer>();renderer.sharedMaterials=new[]{Mat("020303"),Mat("7C8079"),Mat("444941")};renderer.shadowCastingMode=ShadowCastingMode.Off;renderer.receiveShadows=true;
   OfficeFallHoles2748.Add(new OfficeFloorHole2748{Center=center,View=view});
  }
  static void HoleTri2748(List<Vector3> vertices,List<int> triangles,Vector3 a,Vector3 b,Vector3 c){int start=vertices.Count;vertices.Add(a);vertices.Add(b);vertices.Add(c);triangles.Add(start);triangles.Add(start+1);triangles.Add(start+2);}
  static void HoleQuad2748(List<Vector3> vertices,List<int> triangles,Vector3 a,Vector3 b,Vector3 c,Vector3 d){HoleTri2748(vertices,triangles,a,b,c);HoleTri2748(vertices,triangles,a,c,d);}
  static void HoleStrip2748(List<Vector3> vertices,List<int> triangles,Vector3 a,Vector3 b,float width){var side=Vector3.Cross(Vector3.up,b-a).normalized*width*.5f;HoleQuad2748(vertices,triangles,a-side,b-side,b+side,a+side);}
 }
}
