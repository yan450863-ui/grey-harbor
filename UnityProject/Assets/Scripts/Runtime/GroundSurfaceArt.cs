using UnityEngine;
using System;
using System.Collections.Generic;
using UnityEngine.Rendering;
namespace GreyHarbor {
 public enum GroundFinish { Concrete,PatchedConcrete,BrokenTile,Asphalt,Gravel,Soil,Terrazzo,Epoxy,Steel }
 // Broad chipped edges + a recessed-looking core, all on horizontal planes. No branching stalks.
 public static class GroundFracture {
  public static void Draw(Vector3 origin,float length,float width,float angle,System.Random random,Action<string,Vector3,Vector3,Vector3> tri){
   var points=new Vector3[7];var direction=new Vector3(Mathf.Cos(angle),0,Mathf.Sin(angle));var side=new Vector3(-direction.z,0,direction.x);
   for(int i=0;i<7;i++)points[i]=origin+direction*(length*i/6)+side*((float)random.NextDouble()-.5f)*length*.13f;
   for(int layer=0;layer<3;layer++){float multiplier=layer==0?1:layer==1?.60f:.27f;string color=layer==0?"818574":layer==1?"686F5E":"485448";for(int i=0;i<6;i++){
    Vector3 a=points[i]+Vector3.up*(layer*.001f),b=points[i+1]+Vector3.up*(layer*.001f);Vector3 offset=Vector3.Cross(Vector3.up,(b-a).normalized);float wa=width*multiplier*(i==0?.14f:.8f+(float)random.NextDouble()*.2f),wb=width*multiplier*(i==5?.06f:.8f+(float)random.NextDouble()*.2f);
    tri(color,a-offset*wa,b-offset*wb,b+offset*wb);tri(color,a-offset*wa,b+offset*wb,a+offset*wa);
   }}
  }
 }
 public sealed partial class GroundSurfaceArt {
  readonly RaidMap map;readonly System.Random random;readonly Dictionary<string,Batch> batches=new Dictionary<string,Batch>();
  sealed class Batch {public readonly List<Vector3> V=new List<Vector3>();public readonly List<int> T=new List<int>();}
  public GroundSurfaceArt(RaidMap m,int seed=115029){map=m;random=new System.Random(seed);}
  float R(float a,float b)=>a+(float)random.NextDouble()*(b-a);
  void Tri(string group,string color,Vector3 a,Vector3 b,Vector3 c){a=SurfacePoint(a);b=SurfacePoint(b);c=SurfacePoint(c);if(Vector3.Cross(b-a,c-a).y<0){var swap=b;b=c;c=swap;}string key=group+"|"+color;if(!batches.TryGetValue(key,out var batch)){batch=new Batch();batches.Add(key,batch);}int n=batch.V.Count;batch.V.Add(a);batch.V.Add(b);batch.V.Add(c);batch.T.Add(n);batch.T.Add(n+1);batch.T.Add(n+2);}
  void Polygon(string group,string color,List<Vector3> rim){Vector3 center=Vector3.zero;foreach(var p in rim)center+=p;center/=rim.Count;for(int i=0;i<rim.Count;i++)Tri(group,color,center,rim[(i+1)%rim.Count],rim[i]);}
  void Quad(string group,string color,float x,float z,float w,float d,float y){Vector3 a=new Vector3(x-w/2,y,z-d/2),b=new Vector3(x-w/2,y,z+d/2),c=new Vector3(x+w/2,y,z+d/2),e=new Vector3(x+w/2,y,z-d/2);Tri(group,color,a,b,c);Tri(group,color,a,c,e);}
  void Blotch(string group,string color,Rect bounds,float x,float z,float size,float y){var points=new List<Vector3>();for(int i=0;i<9;i++){float a=i*Mathf.PI*2/9,r=R(.65f,1)*size;points.Add(new Vector3(Mathf.Clamp(x+Mathf.Cos(a)*r,bounds.xMin,bounds.xMax),y,Mathf.Clamp(z+Mathf.Sin(a)*r,bounds.yMin,bounds.yMax)));}Polygon(group,color,points);}
  void OriginalRegion(Rect r,GroundFinish finish){
   if(finish==GroundFinish.BrokenTile){Tiles(r);return;}
   bool patched=finish==GroundFinish.PatchedConcrete;string group=patched?"PatchedConcrete":"PouredConcrete";
   Quad(group,patched?"727A6D":"7B8073",r.center.x,r.center.y,r.width,r.height,.016f);
   for(int i=0;i<r.width*r.height*.7f;i++)Blotch("ConcreteMottling",i%2==0?"777D70":"7E8274",r,R(r.xMin,r.xMax),R(r.yMin,r.yMax),R(.15f,.7f),.017f);
   // Fine aggregate breaks up colour without tiled bands or long grid lines.
   for(int i=0;i<r.width*r.height*25;i++){float x=R(r.xMin+.03f,r.xMax-.03f),z=R(r.yMin+.03f,r.yMax-.03f),size=R(.008f,.025f);Quad("ConcreteAggregate",i%3==0?"8E907F":i%3==1?"6B7468":"828777",x,z,size,size*R(.6f,1.2f),.018f);}
   if(patched){for(int i=0;i<Mathf.Max(2,r.width*r.height/14);i++){float x=R(r.xMin+.7f,r.xMax-.7f),z=R(r.yMin+.7f,r.yMax-.7f);float radius=R(.45f,.75f);var edgePoints=new List<Vector3>();for(int v=0;v<9;v++){float a=v*Mathf.PI*2/9,rr=radius*R(.76f,1);edgePoints.Add(new Vector3(Mathf.Clamp(x+Mathf.Cos(a)*rr,r.xMin,r.xMax),.019f,Mathf.Clamp(z+Mathf.Sin(a)*rr,r.yMin,r.yMax)));}Polygon("CementPatchEdge","6C7567",edgePoints);var facePoints=new List<Vector3>();foreach(var p in edgePoints)facePoints.Add(new Vector3(x+(p.x-x)*.92f,.020f,z+(p.z-z)*.92f));Polygon("CementRepair","858A79",facePoints);}}
   for(int i=0;i<Mathf.Max(1,r.width*r.height/22);i++){float length=R(.55f,1.0f),x=R(r.xMin+1.2f,r.xMax-1.2f),z=R(r.yMin+1.2f,r.yMax-1.2f);GroundFracture.Draw(new Vector3(x,.021f,z),length,.055f,R(0,6.28f),random,(color,a,b,c)=>Tri("ConcreteFracture",color,a,b,c));}
  }
  void Tiles(Rect r){
   Quad("TileMortar","676D5F",r.center.x,r.center.y,r.width,r.height,.015f);
   const float cell=.64f;int ix=0;for(float x=r.xMin;x<r.xMax-.04f;x+=cell,ix++){int iz=0;for(float z=r.yMin;z<r.yMax-.04f;z+=cell,iz++){
    float w=Mathf.Min(cell,r.xMax-x)-.026f,d=Mathf.Min(cell,r.yMax-z)-.026f;if(w<.035f||d<.035f)continue;float left=x+.013f,right=left+w,back=z+.013f,front=back+d;int state=random.Next(100);
    if(state<8){Quad("MissingTileBed","575F52",(left+right)/2,(back+front)/2,w,d,.017f);for(int k=0;k<5;k++){float px=R(left+.025f,right-.025f),pz=R(back+.025f,front-.025f);Tri("LooseTileFragments","A29D86",new Vector3(px,.022f,pz),new Vector3(px-.024f,.022f,pz+.03f),new Vector3(px+.025f,.022f,pz+.042f));}continue;}
    float chip=state<32?Mathf.Min(w,d)*R(.18f,.4f):.006f;var rim=new List<Vector3>{new Vector3(left,.019f,back),new Vector3(right-chip,.019f,back),new Vector3(right,.019f,back+chip),new Vector3(right,.019f,front),new Vector3(left,.019f,front)};
    string color=(ix+iz)%4==0?"8D9988":state%3==0?"AEAE97":state%3==1?"A5A78F":"B3B09A";Polygon("OldCeramicTiles",color,rim);
    if(chip>.03f){Tri("TileChippedEdge","CDC4A6",new Vector3(right-chip,.020f,back),new Vector3(right-chip+.018f,.020f,back+.03f),new Vector3(right-.018f,.020f,back+chip+.014f));}
    if(state>=32&&state<43&&w>.25f&&d>.3f)GroundFracture.Draw(new Vector3(left+.08f,.020f,back+d*.45f),w-.16f,.022f,0,random,(c,a,b,e)=>Tri("TileFracture",c,a,b,e));
    if(state%4==0)Quad("TileScuff","999F87",left+w*.5f,back+d*.5f,w*.22f,.025f,.023f);
   }}
  }
  public void Bake(string rootName="GroundSurfaces_115"){var root=new GameObject(rootName);root.AddComponent<GroundSurfaceRegions>().Regions.AddRange(regions);root.transform.SetParent(map.Root,false);if(map.HarborWhitebox||map.YardInterior138||map.FerryWhitebox143||map.InteriorId=="repairannex"||map.RepairMain220){foreach(var pair in batches)map.RegisterGround137(pair.Value.V,pair.Value.T,PixelArt.Hex(pair.Key.Split('|')[1]));return;}var lifetime=root.AddComponent<WorkshopArtResources>();foreach(var pair in batches){var names=pair.Key.Split('|');var g=new GameObject(names[0]);g.transform.SetParent(root.transform,false);var mesh=new Mesh{name=pair.Key};if(pair.Value.V.Count>65535)mesh.indexFormat=IndexFormat.UInt32;mesh.SetVertices(pair.Value.V);mesh.SetTriangles(pair.Value.T,0);mesh.RecalculateNormals();mesh.RecalculateBounds();g.AddComponent<MeshFilter>().sharedMesh=mesh;var renderer=g.AddComponent<MeshRenderer>();var material=new Material(GreyHarbor.FlashlightShaders2763.Load("HarborWorld"));material.color=PixelArt.Hex(names[1]);material.SetFloat("_SolidFog",0);material.SetFloat("_Detail",.022f);renderer.sharedMaterial=material;renderer.shadowCastingMode=ShadowCastingMode.Off;renderer.receiveShadows=true;lifetime.Meshes.Add(mesh);lifetime.Materials.Add(material);}}
 }
}
