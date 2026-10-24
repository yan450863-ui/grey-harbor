using UnityEngine;
using System.Collections.Generic;
namespace GreyHarbor {
 public sealed class HarborRoadRecord2717 {
  public string Name;public float Width;public Vector3[] Points;
 }
 public sealed class HarborConnection2717 {
  public string Name;public Vector3[] Points;public float ExistingLandWidth;
 }
 public sealed partial class RaidMap {
  public readonly List<HarborRoadRecord2717> HarborRoadRecords2717=new List<HarborRoadRecord2717>();
  public readonly List<Renderer> HarborBaseRenderers2717=new List<Renderer>();
  public readonly List<HarborConnection2717> HarborConnections2717=new List<HarborConnection2717>();
  void RegisterHarborRoadPolygon2717(Vector2[] shape){
   HarborRoadShapes.Add(shape);float xmin=float.MaxValue,zmin=float.MaxValue,xmax=float.MinValue,zmax=float.MinValue;
   var vertices=new List<Vector3>();foreach(var p in shape){vertices.Add(new Vector3(p.x,.012f,p.y));xmin=Mathf.Min(xmin,p.x);xmax=Mathf.Max(xmax,p.x);zmin=Mathf.Min(zmin,p.y);zmax=Mathf.Max(zmax,p.y);}
   var triangles=new List<int>();for(int i=1;i<shape.Length-1;i++){triangles.Add(0);triangles.Add(i+1);triangles.Add(i);}
   RegisterGround137(vertices,triangles,PixelArt.Hex("667077"));WhiteboxRoads.Add(Rect.MinMaxRect(xmin,zmin,xmax,zmax));
  }
  void RegisterHarborRoad2717(float width,Vector3[] points){
   HarborRoadRecords2717.Add(new HarborRoadRecord2717{Name="Road"+(HarborRoadRecords2717.Count+1).ToString("00"),Width=width,Points=(Vector3[])points.Clone()});
   float radius=width*.5f;
   for(int i=1;i<points.Length;i++){
    Vector2 a=V2(points[i-1]),b=V2(points[i]),d=b-a;if(d.sqrMagnitude<.0001f)continue;Vector2 n=new Vector2(-d.y,d.x).normalized*radius;
    // Constant-width strips stop at their actual endpoints. Round joins replace the former oversized square caps.
    RegisterHarborRoadPolygon2717(new[]{a-n,b-n,b+n,a+n});
   }
   foreach(var point in points){var disc=new Vector2[24];for(int k=0;k<disc.Length;k++){float angle=k*Mathf.PI*2/disc.Length;disc[k]=V2(point)+new Vector2(Mathf.Cos(angle),Mathf.Sin(angle))*radius;}RegisterHarborRoadPolygon2717(disc);}
  }
  void Connection2717(string name,float width,params float[] coordinates){
   var points=new Vector3[coordinates.Length/2];for(int i=0;i<points.Length;i++)points[i]=PortPoint(coordinates[i*2],coordinates[i*2+1]);
   HarborConnections2717.Add(new HarborConnection2717{Name=name,ExistingLandWidth=width,Points=points});
  }
  void RegisterHarborFlatBases2717(){
   // This existing entrance slab is presentation only. Its raised kerbs/paint and all colliders stay intact.
   var entrance=Root.Find("Entrance_Base");if(entrance){var renderer=entrance.GetComponent<Renderer>();if(renderer){Bounds b=renderer.bounds;RegisterHarborBaseRect2717(Rect.MinMaxRect(b.min.x,b.min.z,b.max.x,b.max.z),.013f,renderer.sharedMaterial.color);HarborBaseRenderers2717.Add(renderer);}}
   if(HarborConnections2717.Count>0)return;
   Connection2717("入口至南横路",7.5f,66,48,66,54);
   Connection2717("后勤门至横路",4.5f,28,42,28,48,28,54);
   Connection2717("维修区东侧至中央西路",7.5f,46,96,51,96,57.5f,96);
   Connection2717("中央南侧至南横路",6,92,62.5f,92,54);
   Connection2717("中央北侧至北横路",7.5f,98,127.5f,98,137);
   Connection2717("中央东侧至堤岸路",5,127.5f,102,139,102);
   Connection2717("西北货场至西侧旧路",6,17,137,17,144);
   Connection2717("货运场至中央集装箱北侧",6,17,145,17,137,51,137,98,137,98,127.5f);
   Connection2717("北码头陆桥",4,159,101,159,113);
   Connection2717("南码头陆桥",4,159,47,159,71);
   // The original net-drying rack at (150.5,126), yaw90, occupies the road-side service pocket.
   // Begin at the real coast-road junction and follow the existing open north side of that rack.
   Connection2717("北码头横向连接",4,139,126,148,126,148,128,153,128,153,126,159,126,169,126);
   Connection2717("中码头横向连接",3,145,88,159,88,166,88);
   Connection2717("南码头横向连接",5,146,41,155,41,164,41);
  }
  void RegisterHarborBaseRect2717(Rect r,float priority,Color color){RegisterGround137(new List<Vector3>{new Vector3(r.xMin,priority,r.yMin),new Vector3(r.xMin,priority,r.yMax),new Vector3(r.xMax,priority,r.yMax),new Vector3(r.xMax,priority,r.yMin)},new List<int>{0,1,2,0,2,3},color);}
  void WeatherHarborBasePixels2717(Color32[] pixels,float[] depth,int width,int height,Rect bounds){
   // World-anchored aggregate affects only previously plain bases and roads. Existing authored finishes, markings and blend strips retain their colours.
   for(int z=0;z<height;z++)for(int x=0;x<width;x++){int index=z*width+x;if(pixels[index].a==0||depth[index]>.0131f)continue;
    float px=bounds.xMin+(x+.5f)*bounds.width/width,pz=bounds.yMin+(z+.5f)*bounds.height/height;
    float grain=Mathf.PerlinNoise(px*3.7f+301,pz*3.7f+71)-.5f;
    float broad=Mathf.PerlinNoise(px*.16f+17,pz*.16f+53)-.5f;
    float amount=depth[index]>.001f?grain*.020f+broad*.015f:grain*.026f+broad*.045f;
    Color color=pixels[index];color.r=Mathf.Clamp01(color.r+amount);color.g=Mathf.Clamp01(color.g+amount);color.b=Mathf.Clamp01(color.b+amount*.92f);pixels[index]=color;
   }
  }
 }
}
