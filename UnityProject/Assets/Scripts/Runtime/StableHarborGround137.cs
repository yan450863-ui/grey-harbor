using UnityEngine;
using UnityEngine.Rendering;
using System.Collections.Generic;
using System.IO;
namespace GreyHarbor {
 public sealed partial class RaidMap {
  struct GroundTriangle137 {public Vector3 A,B,C;public Color32 Color;}
  readonly List<GroundTriangle137> groundPaint137=new List<GroundTriangle137>();
  public Texture2D StableGround137;public Rect StableGroundBounds137;
  public bool BypassGroundCache2721;
  public bool GroundCacheUsed2721 {get;private set;}
  public string GroundPaintFingerprint2721 {get;private set;}
  public void RegisterGround137(List<Vector3> v,List<int> indices,Color color){
   RegisterOutdoorAccent2749(v,indices,color);
   for(int i=0;i<indices.Count;i+=3)groundPaint137.Add(new GroundTriangle137{A=v[indices[i]],B=v[indices[i+1]],C=v[indices[i+2]],Color=color});
  }
  void FinalizeHarborGround137(){
   if(HarborWhitebox)RegisterHarborFlatBases2717();
   // Bake flat meadow and damp stains into the same plane, so they never compete for depth.
   var dressing=Root.Find("Yard_GroundDressing");
   if(dressing)foreach(var f in dressing.GetComponentsInChildren<MeshFilter>()){
    var r=f.GetComponent<Renderer>();var v=f.sharedMesh.vertices;for(int i=0;i<v.Length;i++)v[i]=f.transform.TransformPoint(v[i]);
    RegisterGround137(new List<Vector3>(v),new List<int>(f.sharedMesh.triangles),r.sharedMaterial.color);r.enabled=false;
   }
   if(groundPaint137.Count==0)return;
   float xmin=float.MaxValue,zmin=float.MaxValue,xmax=float.MinValue,zmax=float.MinValue;
   foreach(var t in groundPaint137)foreach(var p in new[]{t.A,t.B,t.C}){xmin=Mathf.Min(xmin,p.x);xmax=Mathf.Max(xmax,p.x);zmin=Mathf.Min(zmin,p.z);zmax=Mathf.Max(zmax,p.z);}
   StableGroundBounds137=new Rect(xmin,zmin,xmax-xmin,zmax-zmin);
   float scale=Mathf.Min(24,(HarborWhitebox?4096:2048)/Mathf.Max(xmax-xmin,zmax-zmin));
   int w=Mathf.CeilToInt((xmax-xmin)*scale),h=Mathf.CeilToInt((zmax-zmin)*scale);
   GroundCacheUsed2721=false;
   if(HarborWhitebox&&!IsInterior){
    GroundPaintFingerprint2721=GroundFingerprint2721(w,h);
    Texture2D cached;
    if(!BypassGroundCache2721&&HarborGroundCache2721.TryGet(StableGroundBounds137,w,h,GroundPaintFingerprint2721,out cached)){
     StableGround137=cached;GroundCacheUsed2721=true;
    }
   }
   if(!GroundCacheUsed2721){
   var pixels=new Color32[w*h];var depth=new float[w*h];for(int i=0;i<depth.Length;i++)depth[i]=float.MinValue;
   foreach(var t in groundPaint137){
    Vector2 a=new Vector2((t.A.x-xmin)*w/(xmax-xmin),(t.A.z-zmin)*h/(zmax-zmin));
    Vector2 b=new Vector2((t.B.x-xmin)*w/(xmax-xmin),(t.B.z-zmin)*h/(zmax-zmin));
    Vector2 c=new Vector2((t.C.x-xmin)*w/(xmax-xmin),(t.C.z-zmin)*h/(zmax-zmin));
    float area=Cross(b-a,c-a);if(Mathf.Abs(area)<.00001f)continue;
    int x0=Mathf.Clamp(Mathf.FloorToInt(Mathf.Min(a.x,b.x,c.x)),0,w-1),x1=Mathf.Clamp(Mathf.CeilToInt(Mathf.Max(a.x,b.x,c.x)),0,w-1);
    int z0=Mathf.Clamp(Mathf.FloorToInt(Mathf.Min(a.y,b.y,c.y)),0,h-1),z1=Mathf.Clamp(Mathf.CeilToInt(Mathf.Max(a.y,b.y,c.y)),0,h-1);
    for(int z=z0;z<=z1;z++)for(int x=x0;x<=x1;x++){
     var p=new Vector2(x+.5f,z+.5f);float u=Cross(b-p,c-p)/area,vv=Cross(c-p,a-p)/area,ww=1-u-vv;
     if(u<-.00001f||vv<-.00001f||ww<-.00001f)continue;
     float y=u*t.A.y+vv*t.B.y+ww*t.C.y;int index=z*w+x;
     if(y<depth[index]-.00001f)continue;depth[index]=y;pixels[index]=t.Color;
    }
   }
   // Clip the baked plane to the real land/platform union; rectangular paint must not colour water.
   if(HarborWhitebox)for(int z=0;z<h;z++)for(int x=0;x<w;x++){int n=z*w+x;if(pixels[n].a==0)continue;var p=new Vector3(xmin+(x+.5f)*(xmax-xmin)/w,0,zmin+(z+.5f)*(zmax-zmin)/h);if(!OnHarborSurface(p))pixels[n]=new Color32(0,0,0,0);}
   if(HarborWhitebox)WeatherHarborBasePixels2717(pixels,depth,w,h,StableGroundBounds137);
   if(FreightInterior2716)FinishFreightFloorPixels2718(pixels,w,h,StableGroundBounds137);
   if(HarborWhitebox)WeatherFreightPixels2718(pixels,depth,w,h,StableGroundBounds137,scale);
   if(HarborWhitebox)FinishFreightServiceRoad2719(pixels,depth,w,h,StableGroundBounds137);
   if(HarborWhitebox&&!IsInterior)ApplyOutdoorGroundStyle2749(pixels,depth,w,h,StableGroundBounds137);
   StableGround137=new Texture2D(w,h,TextureFormat.RGBA32,true){name="HarborStableGround137",filterMode=FilterMode.Trilinear,anisoLevel=8,wrapMode=TextureWrapMode.Clamp};
   StableGround137.SetPixels32(pixels);StableGround137.Apply(true,false);
   }
   var go=new GameObject("Harbor_StableGround137");go.transform.SetParent(Root,false);var textureLife=go.AddComponent<StableGroundResources137>();textureLife.Texture=StableGround137;textureLife.OwnsTexture=!GroundCacheUsed2721;
   var life=go.AddComponent<WorkshopArtResources>();var mesh=new Mesh{name="SingleHarborGroundPlane137"};
   mesh.vertices=new[]{new Vector3(xmin,.05f,zmin),new Vector3(xmin,.05f,zmax),new Vector3(xmax,.05f,zmax),new Vector3(xmax,.05f,zmin)};
   mesh.uv=new[]{Vector2.zero,Vector2.up,Vector2.one,Vector2.right};mesh.triangles=new[]{0,1,2,0,2,3};mesh.RecalculateNormals();mesh.RecalculateBounds();
   go.AddComponent<MeshFilter>().sharedMesh=mesh;var renderer=go.AddComponent<MeshRenderer>();var material=new Material(GreyHarbor.FlashlightShaders2763.Load("HarborWorld"));
   material.color=Color.white;material.SetTexture("_MainTex",StableGround137);material.SetFloat("_UseStableGround",1);material.SetFloat("_SolidFog",0);material.SetFloat("_Detail",0);
   renderer.sharedMaterial=material;renderer.shadowCastingMode=ShadowCastingMode.Off;life.Meshes.Add(mesh);life.Materials.Add(material);groundPaint137.Clear();
   if(HarborWhitebox)foreach(var previous in HarborBaseRenderers2717)if(previous)previous.enabled=false;
  }
  string GroundFingerprint2721(int width,int height){
   // Hash ordered paint and all data consulted by the existing land/weather passes.
   // This is proportional to authored geometry, never to the 4096-pixel raster.
   using(var bytes=new MemoryStream())using(var writer=new BinaryWriter(bytes)){
    writer.Write(HarborGroundCache2721.AlgorithmVersion);writer.Write(width);writer.Write(height);
    WriteGroundRect2721(writer,StableGroundBounds137);writer.Write(FreightGroundBuilt2716);
    writer.Write(groundPaint137.Count);
    foreach(var t in groundPaint137){WriteGroundVector2721(writer,t.A);WriteGroundVector2721(writer,t.B);WriteGroundVector2721(writer,t.C);writer.Write(t.Color.r);writer.Write(t.Color.g);writer.Write(t.Color.b);writer.Write(t.Color.a);}
    writer.Write(HarborSurfaces.Count);foreach(var surface in HarborSurfaces){writer.Write(surface.Color??"");WriteGroundPolygon2721(writer,surface.Points);}
    writer.Write(HarborRoadShapes.Count);foreach(var road in HarborRoadShapes)WriteGroundPolygon2721(writer,road);
    writer.Write(FreightSurfaceFeatures2717.Count);foreach(var f in FreightSurfaceFeatures2717){writer.Write(f.Name??"");writer.Write(f.Kind??"");writer.Write(f.PaletteHex??"");WriteGroundRect2721(writer,f.Area);WriteGroundPolygon2721(writer,f.Polygon);}
    writer.Flush();return HarborGroundCache2721.HashBytes(bytes.ToArray());
   }
  }
  static void WriteGroundRect2721(BinaryWriter w,Rect r){w.Write(r.x);w.Write(r.y);w.Write(r.width);w.Write(r.height);}
  static void WriteGroundVector2721(BinaryWriter w,Vector3 p){w.Write(p.x);w.Write(p.y);w.Write(p.z);}
  static void WriteGroundPolygon2721(BinaryWriter w,Vector2[] points){w.Write(points.Length);foreach(var p in points){w.Write(p.x);w.Write(p.y);}}
 }
}
