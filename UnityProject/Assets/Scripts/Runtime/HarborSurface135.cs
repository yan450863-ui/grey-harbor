using UnityEngine;
using System.Collections.Generic;
using System.IO;
namespace GreyHarbor {
 public sealed partial class RaidMap {
  public sealed class HarborSurface {public Vector2[] Points;public string Color;public HarborSurface(Vector2[] p,string c){Points=p;Color=c;}}
  public readonly List<HarborSurface> HarborSurfaces=new List<HarborSurface>();
  public readonly List<Vector2[]> HarborRoadShapes=new List<Vector2[]>();
  public int HarborCoastEdges;
  public static Vector3 PortPoint(float x,float z)=>new Vector3(x-79,0,z-73);
  static Vector2 V2(Vector3 p)=>new Vector2(p.x,p.z);
  Vector2[] PortPolygon(params float[] xy){var p=new Vector2[xy.Length/2];for(int i=0;i<p.Length;i++)p[i]=V2(PortPoint(xy[i*2],xy[i*2+1]));return p;}
  public static bool InPolygon(Vector2 p,Vector2[] polygon){
   bool inside=false;
   for(int i=0,j=polygon.Length-1;i<polygon.Length;j=i++){
    Vector2 a=polygon[j],b=polygon[i],ab=b-a;float t=Mathf.Clamp01(Vector2.Dot(p-a,ab)/Mathf.Max(ab.sqrMagnitude,.0001f));
    if((p-a-ab*t).sqrMagnitude<.000001f)return true;
    if((a.y>p.y)!=(b.y>p.y)&&p.x<(b.x-a.x)*(p.y-a.y)/(b.y-a.y)+a.x)inside=!inside;
   }
   return inside;
  }
  public bool OnHarborSurface(Vector3 p){if(!HarborWhitebox)return true;var v=V2(p);foreach(var s in HarborSurfaces)if(InPolygon(v,s.Points))return true;return false;}
  public bool HarborSurfaceClear(Vector3 p,float radius=.34f){
   if(!OnHarborSurface(p))return false;
   for(int i=0;i<8;i++){float a=i*Mathf.PI/4;if(!OnHarborSurface(p+new Vector3(Mathf.Cos(a),0,Mathf.Sin(a))*radius))return false;}
   return true;
  }
  void AddHarborFloor(string name,string color,params float[] xy){
   var points=PortPolygon(xy);HarborSurfaces.Add(new HarborSurface(points,color));
   var g=new GameObject(name);g.transform.SetParent(Root,false);var life=g.AddComponent<WorkshopArtResources>();var mesh=new Mesh{name=name};
   var vertices=new Vector3[points.Length];for(int i=0;i<points.Length;i++)vertices[i]=new Vector3(points[i].x,0,points[i].y);
   var order=new List<int>();float area=0;for(int i=0;i<points.Length;i++){var a=points[i];var b=points[(i+1)%points.Length];area+=a.x*b.y-b.x*a.y;}
   for(int i=0;i<points.Length;i++)order.Add(area>0?i:points.Length-1-i);
   var triangles=new List<int>();int guard=0;
   while(order.Count>2&&guard++<points.Length*points.Length){
    bool clipped=false;
    for(int j=0;j<order.Count;j++){
     int a=order[(j+order.Count-1)%order.Count],b=order[j],c=order[(j+1)%order.Count];
     if(Cross(points[b]-points[a],points[c]-points[b])<=.0001f)continue;
     bool occupied=false;foreach(int k in order){if(k==a||k==b||k==c)continue;var p=points[k];if(Cross(points[b]-points[a],p-points[a])>=-.0001f&&Cross(points[c]-points[b],p-points[b])>=-.0001f&&Cross(points[a]-points[c],p-points[c])>=-.0001f){occupied=true;break;}}
     if(occupied)continue;triangles.Add(a);triangles.Add(c);triangles.Add(b);order.RemoveAt(j);clipped=true;break;
    }
    if(!clipped)throw new System.InvalidOperationException("Cannot triangulate "+name);
   }
   mesh.vertices=vertices;mesh.SetTriangles(triangles,0);mesh.RecalculateNormals();mesh.RecalculateBounds();life.Meshes.Add(mesh);
   g.AddComponent<MeshFilter>().sharedMesh=mesh;var renderer=g.AddComponent<MeshRenderer>();renderer.sharedMaterial=Mat(color,false);
   // Preserve the exact land/platform union, but render its base in the same baked plane as the regional finishes.
   RegisterGround137(new List<Vector3>(vertices),triangles,PixelArt.Hex(color));HarborBaseRenderers2717.Add(renderer);
  }
  static float Cross(Vector2 a,Vector2 b)=>a.x*b.y-a.y*b.x;
  void BuildHarborSurface(){
   AddHarborFloor("Harbor_Mainland","A0A7A4",0,55,0,125,5,128,5,175,48,180,108,180,110,176,152,176,154,152,150,140,153,125,146,110,149,92,140,78,143,60,148,54,148,34,136,30,124,30,124,27,118,27,118,8,91,8,91,0,55,0,55,8,8,8,8,52);
   AddHarborFloor("Dock_NorthService","B2B6AD",146,116,171,116,171,122,188,122,188,130,171,130,171,138,149,138,149,131,146,131);
   AddHarborFloor("Dock_MiddleRepair","A7ADA4",139,85,149,85,149,74,171,74,171,80,181,80,181,87,171,87,171,98,149,98,149,91,139,91);
   AddHarborFloor("Dock_SouthLoading","B9BBB0",146,18,178,18,178,8,190,8,190,22,183,22,183,43,150,43,150,49,146,49);
   AddHarborFloor("Dock_NorthLink","979F97",157,98,161,98,161,116,157,116);
   AddHarborFloor("Dock_SouthLink","979F97",157,43,161,43,161,74,157,74);
   AddDockSurfaces141();AddFerrySurfaces143();BuildHarborCoast();
  }
  void BuildHarborCoast(){
   // Split every boundary at intersections; only expose the outer edge of the union.
   // Overlapping platform entrances have no internal fence or invisible water shortcut.
   foreach(var surface in HarborSurfaces)for(int i=0;i<surface.Points.Length;i++){
    Vector2 a=surface.Points[i],b=surface.Points[(i+1)%surface.Points.Length],r=b-a;var cuts=new List<float>{0,1};
    if(surface==HarborSurfaces[0]){if(Mathf.Abs(r.y)>.0001f){float t=(-25-a.y)/r.y;if(t>0&&t<1)cuts.Add(t);}if(Mathf.Abs(r.x)>.0001f){float t=(-25-a.x)/r.x;if(t>0&&t<1)cuts.Add(t);}}
    if(surface==HarborSurfaces[0]&&Mathf.Abs(a.x+74)<.01f&&Mathf.Abs(b.x+74)<.01f)foreach(float z in new[]{84f,92f}){float t=(z-a.y)/r.y;if(t>0&&t<1)cuts.Add(t);}
    foreach(var other in HarborSurfaces)for(int j=0;j<other.Points.Length;j++){
     Vector2 c=other.Points[j],e=other.Points[(j+1)%other.Points.Length],s=e-c;float den=Cross(r,s);
     if(Mathf.Abs(den)>.00001f){float t=Cross(c-a,s)/den,u=Cross(c-a,r)/den;if(t>.0001f&&t<.9999f&&u>=0&&u<=1)cuts.Add(t);}
     else if(Mathf.Abs(Cross(c-a,r))<.0001f){foreach(var p in new[]{c,e}){float t=Vector2.Dot(p-a,r)/r.sqrMagnitude;if(t>.0001f&&t<.9999f)cuts.Add(t);}}
    }
    cuts.Sort();for(int j=1;j<cuts.Count;j++){
     float t0=cuts[j-1],t1=cuts[j];if(t1-t0<.0001f)continue;Vector2 start=a+r*t0,end=a+r*t1,mid=(start+end)*.5f,n=new Vector2(-r.y,r.x).normalized*.08f;
     bool left=OnHarborSurface(new Vector3(mid.x+n.x,0,mid.y+n.y)),right=OnHarborSurface(new Vector3(mid.x-n.x,0,mid.y-n.y));if(left==right)continue;
     if(surface==HarborSurfaces[0]&&((Mathf.Abs(mid.x+71)<.01f&&mid.y<=-25)||(Mathf.Abs(mid.y+65)<.01f&&mid.x<=-25)))continue;
     if(surface==HarborSurfaces[0]&&Mathf.Abs(mid.x+74)<.01f&&mid.y>84&&mid.y<92)continue; // Eight-metre freight gate; the exterior kit retains the land-edge stop.
     bool inland=surface==HarborSurfaces[0]&&(mid.x+79<8.1f||mid.y+73>174||mid.y+73<8.1f);
     if(surface!=HarborSurfaces[0]){DockCoastRail141(start,end);HarborCoastEdges++;continue;}
     float height=inland?2.2f:.78f;var d=end-start;
     var g=Box(inland?"Harbor_PerimeterFence":"Harbor_CoastRail",new Vector3(mid.x,height/2,mid.y),new Vector3(d.magnitude+.03f,height,.32f),inland?"BBC5C4":"B1BAB2",true);
     g.transform.rotation=Quaternion.Euler(0,-Mathf.Atan2(d.y,d.x)*Mathf.Rad2Deg,0);SceneObstacle.Apply(g,inland?ObstacleKind.High:ObstacleKind.Low);HarborCoastEdges++;
    }
   }
  }
  public bool HarborPlanCacheUsed2721 {get;private set;}
  public string HarborPlanFingerprint2721 {get;private set;}
  public Texture2D HarborPlanTexture(){
   var life=Root.GetComponent<HarborPlanResources>()??Root.gameObject.AddComponent<HarborPlanResources>();if(life.Texture)return life.Texture;
   HarborPlanCacheUsed2721=false;
   if(HarborWhitebox&&!IsInterior){
    HarborPlanFingerprint2721=PlanFingerprint2721();Texture2D cached;
    if(!BypassGroundCache2721&&GroundCacheUsed2721&&HarborGroundCache2721.TryGetPlan(Bounds,Mathf.RoundToInt(Bounds.width*2),Mathf.RoundToInt(Bounds.height*2),GroundPaintFingerprint2721,HarborPlanFingerprint2721,out cached)){
     life.Texture=cached;life.OwnsTexture=false;HarborPlanCacheUsed2721=true;return cached;
    }
   }
   var yardGround=Root.Find("Yard_GroundMaterials");var yardRegions=yardGround?yardGround.GetComponent<GroundSurfaceRegions>().Regions:null;int w=Mathf.RoundToInt(Bounds.width*2),h=Mathf.RoundToInt(Bounds.height*2);var colors=new Color32[w*h];Color32 sea=PixelArt.Hex("263E47");
   for(int z=0;z<h;z++)for(int x=0;x<w;x++){
    Vector3 p=new Vector3(Bounds.xMin+(x+.5f)/2,0,Bounds.yMin+(z+.5f)/2);Color32 col=sea;
    foreach(var s in HarborSurfaces)if(InPolygon(V2(p),s.Points))col=PixelArt.Hex(s.Color);
    foreach(var road in HarborRoadShapes)if(InPolygon(V2(p),road)&&OnHarborSurface(p))col=PixelArt.Hex("667077");
    if(yardRegions!=null)foreach(var r in yardRegions)if(r.Area.Contains(new Vector2(p.x,p.z)))col=PixelArt.Hex(r.Finish==GroundFinish.Soil?"7D785E":r.Finish==GroundFinish.Gravel?"807E68":"858A78");if(StableGround137&&StableGroundBounds137.Contains(V2(p))){var paint=StableGround137.GetPixelBilinear((p.x-StableGroundBounds137.xMin)/StableGroundBounds137.width,(p.z-StableGroundBounds137.yMin)/StableGroundBounds137.height);if(paint.a>.1f)col=paint;}colors[z*w+x]=col;
   }
   foreach(var c in Root.GetComponentsInChildren<Collider>()){
    if(!c.enabled||c.isTrigger||c.GetComponentInParent<RaidActor>()||(SceneObstacle.MovementMask&(1<<c.gameObject.layer))==0)continue;
    var b=c.bounds;int x0=Mathf.Clamp(Mathf.FloorToInt((b.min.x-Bounds.xMin)*2),0,w-1),x1=Mathf.Clamp(Mathf.CeilToInt((b.max.x-Bounds.xMin)*2),0,w-1);
    int z0=Mathf.Clamp(Mathf.FloorToInt((b.min.z-Bounds.yMin)*2),0,h-1),z1=Mathf.Clamp(Mathf.CeilToInt((b.max.z-Bounds.yMin)*2),0,h-1);
    for(int z=z0;z<=z1;z++)for(int x=x0;x<=x1;x++){
     var p=new Vector3(Bounds.xMin+(x+.5f)/2,Mathf.Clamp(.8f,b.min.y+.001f,b.max.y-.001f),Bounds.yMin+(z+.5f)/2);
     if((c.ClosestPoint(p)-p).sqrMagnitude<.0001f&&OnHarborSurface(p))colors[z*w+x]=PixelArt.Hex(c.gameObject.layer==SceneObstacle.LowLayer?"7F8F86":"D6DBD6");
    }
   }
   life.Texture=new Texture2D(w,h,TextureFormat.RGBA32,false){name="Harbor190Plan",filterMode=FilterMode.Point,wrapMode=TextureWrapMode.Clamp};life.Texture.SetPixels32(colors);life.Texture.Apply();return life.Texture;
  }
  string PlanFingerprint2721(){
   // Obstacle changes must invalidate the cached overlay even when the painted
   // land is identical. Hash geometry only; no per-pixel ClosestPoint queries.
   using(var bytes=new MemoryStream())using(var writer=new BinaryWriter(bytes)){
    WriteGroundRect2721(writer,Bounds);
    var yardGround=Root.Find("Yard_GroundMaterials");var regions=yardGround?yardGround.GetComponent<GroundSurfaceRegions>().Regions:null;
    writer.Write(regions==null?0:regions.Count);if(regions!=null)foreach(var region in regions){WriteGroundRect2721(writer,region.Area);writer.Write((int)region.Finish);}
    foreach(var c in Root.GetComponentsInChildren<Collider>()){
     if(!c.enabled||c.isTrigger||c.GetComponentInParent<RaidActor>()||(SceneObstacle.MovementMask&(1<<c.gameObject.layer))==0)continue;
     writer.Write(c.GetType().Name);writer.Write(c.gameObject.layer);
     var matrix=c.transform.localToWorldMatrix;for(int i=0;i<16;i++)writer.Write(matrix[i]);
     var b=c.bounds;WriteGroundVector2721(writer,b.center);WriteGroundVector2721(writer,b.size);
     var box=c as BoxCollider;if(box){WriteGroundVector2721(writer,box.center);WriteGroundVector2721(writer,box.size);}
     var sphere=c as SphereCollider;if(sphere){WriteGroundVector2721(writer,sphere.center);writer.Write(sphere.radius);}
     var capsule=c as CapsuleCollider;if(capsule){WriteGroundVector2721(writer,capsule.center);writer.Write(capsule.radius);writer.Write(capsule.height);writer.Write(capsule.direction);}
     var mesh=c as MeshCollider;if(mesh){writer.Write(mesh.convex);var shape=mesh.sharedMesh;writer.Write(shape?shape.vertexCount:0);if(shape){foreach(var p in shape.vertices)WriteGroundVector2721(writer,p);var indices=shape.triangles;writer.Write(indices.Length);foreach(var index in indices)writer.Write(index);}}
    }
    writer.Flush();return HarborGroundCache2721.HashBytes(bytes.ToArray());
   }
  }
 }
 public sealed class HarborPlanResources:MonoBehaviour {
  public Texture2D Texture;public bool OwnsTexture=true;
  void OnDestroy(){if(OwnsTexture)TransientObjects2721.Release(Texture);}
 }
}
