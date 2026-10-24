using UnityEngine;
using System.Collections.Generic;
namespace GreyHarbor {
 public sealed partial class RaidUI {
  Color32[] terrainSource325;int terrainWidth325,terrainHeight325,terrainRevision325=-1;Transform terrainRoot325;Rect terrainBounds325;Color32 terrainOutside325;
  sealed class TerrainFrame325 {public Texture2D Image;public Color32[] Pixels;public Vector3 Origin,StepX,StepY;public Transform Root;public int Revision;public float LastUpdate;public Rect SourceBounds;}
  sealed class TerrainShape327 {public Vector3[] Points;public Vector2[] Projected;public Rect WorldBounds;public bool Low;}
  sealed class TerrainSource327 {public Color32[] Pixels;public int Width,Height,Revision;public Transform Root;public Rect Bounds;public Color32 Outside;public readonly List<TerrainShape327> Shapes=new List<TerrainShape327>();}
  readonly TerrainFrame325 nearbyFrame325=new TerrainFrame325(),overviewFrame325=new TerrainFrame325();
  readonly TerrainSource327 nearbySource327=new TerrainSource327(),overviewSource327=new TerrainSource327();
  List<TerrainShape327> terrainShapes327;
  internal const float TerrainSourcePixelsPerMeter327=10;
  internal int TerrainRefreshes325 {get;private set;}
  internal Vector2Int TerrainSourceSize327=>new Vector2Int(terrainWidth325,terrainHeight325);
  internal int TerrainShapeCount327=>terrainShapes327?.Count??0;
  internal static float TerrainScreenScale327=>Mathf.Min(Screen.width/1600f,Screen.height/900f);
  internal static bool IsNearbyTerrain327(Rect viewport)=>viewport.width<=230&&viewport.height<=200;
  internal static Vector2Int TerrainOutputSize327(Rect viewport,float guiScale){
   if(!IsNearbyTerrain327(viewport))return new Vector2Int(Mathf.Clamp(Mathf.CeilToInt(viewport.width),16,1024),Mathf.Clamp(Mathf.CeilToInt(viewport.height),16,768));
   float density=Mathf.Clamp(guiScale,.25f,Mathf.Min(640/Mathf.Max(1,viewport.width),512/Mathf.Max(1,viewport.height)));
   return new Vector2Int(Mathf.Clamp(Mathf.CeilToInt(viewport.width*density),16,640),Mathf.Clamp(Mathf.CeilToInt(viewport.height*density),16,512));
  }
  internal Texture2D TerrainImage325(RaidMap map,MiniMapProjection projection,bool force325=false)=>TerrainImageScale327(map,projection,TerrainScreenScale327,force325);
  internal Texture2D TerrainImageScale327(RaidMap map,MiniMapProjection projection,float guiScale,bool force327=false){
   bool nearby=IsNearbyTerrain327(projection.Viewport);UseTerrainSource327(map,nearby);
   var frame=nearby?nearbyFrame325:overviewFrame325;
   var size=TerrainOutputSize327(projection.Viewport,guiScale);int w=size.x,h=size.y;
   Vector3 origin=projection.WorldPoint(projection.Viewport.min),sx=projection.WorldPoint(projection.Viewport.min+new Vector2(projection.Viewport.width/w,0))-origin,sy=projection.WorldPoint(projection.Viewport.min+new Vector2(0,projection.Viewport.height/h))-origin;
   bool changed=!frame.Image||frame.Image.width!=w||frame.Image.height!=h||frame.Root!=map.Root||frame.Revision!=map.LayoutRevision||frame.SourceBounds!=map.Bounds;
   bool moved=(frame.Origin-origin).sqrMagnitude>.0001f||(frame.StepX-sx).sqrMagnitude>.00000001f||(frame.StepY-sy).sqrMagnitude>.00000001f;
   if(!force327&&!changed&&(!moved||Time.unscaledTime-frame.LastUpdate<.05f))return frame.Image;
   if(!frame.Image||frame.Image.width!=w||frame.Image.height!=h){if(frame.Image)Destroy(frame.Image);frame.Image=new Texture2D(w,h,TextureFormat.RGBA32,false){name=nearby?"NearbyTerrain325":"OverviewTerrain325",filterMode=FilterMode.Bilinear,wrapMode=TextureWrapMode.Clamp};frame.Pixels=new Color32[w*h];}
   // Four real ground positions per displayed pixel antialias the rotated crop at
   // current screen density, rather than blurring a low-resolution crop.
   Vector3 quarterX=sx*.25f,quarterY=sy*.25f;
   for(int y=0;y<h;y++){Vector3 world=origin+sx*.5f+sy*(h-y-.5f);for(int x=0;x<w;x++,world+=sx){
    Color color=nearby?(TerrainSample327(world-quarterX-quarterY)+TerrainSample327(world+quarterX-quarterY)+TerrainSample327(world-quarterX+quarterY)+TerrainSample327(world+quarterX+quarterY))*.25f:TerrainSample327(world);
    frame.Pixels[y*w+x]=color;
   }}
   if(nearby&&map.HarborWhitebox){var worldBounds=TerrainViewportBounds327(projection);foreach(var shape in terrainShapes327){if(!shape.WorldBounds.Overlaps(worldBounds))continue;var polygon=shape.Projected;for(int i=0;i<polygon.Length;i++){Vector2 point=projection.Point(shape.Points[i]);polygon[i]=new Vector2((point.x-projection.Viewport.xMin)/projection.Viewport.width*w,(projection.Viewport.yMax-point.y)/projection.Viewport.height*h);}PaintTerrainShape327(frame.Pixels,w,h,polygon,PixelArt.Hex(shape.Low?"7F8F86":"D6DBD6"));}}
   frame.Image.SetPixels32(frame.Pixels);frame.Image.Apply(false,false);frame.Origin=origin;frame.StepX=sx;frame.StepY=sy;frame.Root=map.Root;frame.Revision=map.LayoutRevision;frame.SourceBounds=map.Bounds;frame.LastUpdate=Time.unscaledTime;TerrainRefreshes325++;return frame.Image;
  }
  void UseTerrainSource327(RaidMap map,bool nearby){
   var source=nearby?nearbySource327:overviewSource327;
   terrainShapes327=source.Shapes;
   if(source.Pixels!=null&&source.Root==map.Root&&source.Revision==map.LayoutRevision&&source.Bounds==map.Bounds){terrainSource325=source.Pixels;terrainWidth325=source.Width;terrainHeight325=source.Height;terrainRoot325=source.Root;terrainRevision325=source.Revision;terrainBounds325=source.Bounds;terrainOutside325=source.Outside;return;}
   EnsureTerrainSource325(map,nearby);source.Pixels=terrainSource325;source.Width=terrainWidth325;source.Height=terrainHeight325;source.Root=map.Root;source.Revision=map.LayoutRevision;source.Bounds=map.Bounds;source.Outside=terrainOutside325;
  }
  Color TerrainSample327(Vector3 world){
   float x=(world.x-terrainBounds325.xMin)/terrainBounds325.width*terrainWidth325-.5f,y=(world.z-terrainBounds325.yMin)/terrainBounds325.height*terrainHeight325-.5f;
   int x0=Mathf.FloorToInt(x),y0=Mathf.FloorToInt(y);float fx=x-x0,fy=y-y0;
   return Color.Lerp(Color.Lerp(TerrainSourcePixel327(x0,y0),TerrainSourcePixel327(x0+1,y0),fx),Color.Lerp(TerrainSourcePixel327(x0,y0+1),TerrainSourcePixel327(x0+1,y0+1),fx),fy);
  }
  Color TerrainSourcePixel327(int x,int y)=>x>=0&&y>=0&&x<terrainWidth325&&y<terrainHeight325?terrainSource325[y*terrainWidth325+x]:terrainOutside325;
  internal static Rect TerrainViewportBounds327(MiniMapProjection projection){var frame=projection.Viewport;float left=float.MaxValue,right=float.MinValue,bottom=float.MaxValue,top=float.MinValue;foreach(var screen in new[]{frame.min,new Vector2(frame.xMax,frame.yMin),frame.max,new Vector2(frame.xMin,frame.yMax)}){var point=projection.WorldPoint(screen);left=Mathf.Min(left,point.x);right=Mathf.Max(right,point.x);bottom=Mathf.Min(bottom,point.z);top=Mathf.Max(top,point.z);}return Rect.MinMaxRect(left,bottom,right,top);}
  internal static Vector3[] TerrainColliderPolygon327(Collider collider){
   var points=new List<Vector2>();var box=collider as BoxCollider;
   if(box){for(int i=0;i<8;i++){var p=box.transform.TransformPoint(box.center+Vector3.Scale(box.size*.5f,new Vector3((i&1)==0?-1:1,(i&2)==0?-1:1,(i&4)==0?-1:1)));points.Add(new Vector2(p.x,p.z));}}
   else {
    var sphere=collider as SphereCollider;var capsule=collider as CapsuleCollider;var mesh=collider as MeshCollider;
    if(sphere||capsule){var scale=collider.transform.lossyScale;scale=new Vector3(Mathf.Abs(scale.x),Mathf.Abs(scale.y),Mathf.Abs(scale.z));Vector3 center=collider.transform.TransformPoint(sphere?sphere.center:capsule.center),offset=Vector3.zero;float radius;
     if(sphere)radius=sphere.radius*Mathf.Max(scale.x,Mathf.Max(scale.y,scale.z));
     else {int axis=capsule.direction;float axial=axis==0?scale.x:axis==1?scale.y:scale.z,radial=axis==0?Mathf.Max(scale.y,scale.z):axis==1?Mathf.Max(scale.x,scale.z):Mathf.Max(scale.x,scale.y);radius=capsule.radius*radial;offset=collider.transform.TransformDirection(axis==0?Vector3.right:axis==1?Vector3.up:Vector3.forward).normalized*Mathf.Max(0,capsule.height*axial*.5f-radius);}
     for(int end=0;end<2;end++)for(int i=0;i<16;i++){float angle=i*Mathf.PI/8;var p=center+(end==0?-offset:offset)+new Vector3(Mathf.Cos(angle)*radius,0,Mathf.Sin(angle)*radius);points.Add(new Vector2(p.x,p.z));}
    }else if(mesh&&mesh.convex&&mesh.sharedMesh&&mesh.sharedMesh.isReadable){foreach(var vertex in mesh.sharedMesh.vertices){var p=mesh.transform.TransformPoint(vertex);points.Add(new Vector2(p.x,p.z));}}
    else return new Vector3[0];
   }
   points.Sort((a,b)=>{int x=a.x.CompareTo(b.x);return x==0?a.y.CompareTo(b.y):x;});for(int i=points.Count-1;i>0;i--)if((points[i]-points[i-1]).sqrMagnitude<.0000001f)points.RemoveAt(i);
   if(points.Count<3)return new Vector3[0];var hull=new List<Vector2>();
   foreach(var point in points){while(hull.Count>=2&&TerrainCross327(hull[hull.Count-1]-hull[hull.Count-2],point-hull[hull.Count-1])<=0)hull.RemoveAt(hull.Count-1);hull.Add(point);}int lower=hull.Count;
   for(int i=points.Count-2;i>=0;i--){var point=points[i];while(hull.Count>lower&&TerrainCross327(hull[hull.Count-1]-hull[hull.Count-2],point-hull[hull.Count-1])<=0)hull.RemoveAt(hull.Count-1);hull.Add(point);}hull.RemoveAt(hull.Count-1);
   var result=new Vector3[hull.Count];for(int i=0;i<result.Length;i++)result[i]=new Vector3(hull[i].x,0,hull[i].y);return result;
  }
  static float TerrainCross327(Vector2 a,Vector2 b)=>a.x*b.y-a.y*b.x;
  internal static bool TerrainPolygonContains327(Vector2 point,Vector2[] polygon){float sign=0;for(int i=0;i<polygon.Length;i++){float cross=TerrainCross327(polygon[(i+1)%polygon.Length]-polygon[i],point-polygon[i]);if(Mathf.Abs(cross)<.00001f)continue;if(sign==0)sign=Mathf.Sign(cross);else if(sign*cross<0)return false;}return polygon.Length>=3;}
  static void PaintTerrainShape327(Color32[] pixels,int width,int height,Vector2[] polygon,Color color){
   float left=float.MaxValue,right=float.MinValue,bottom=float.MaxValue,top=float.MinValue;foreach(var point in polygon){left=Mathf.Min(left,point.x);right=Mathf.Max(right,point.x);bottom=Mathf.Min(bottom,point.y);top=Mathf.Max(top,point.y);}
   int x0=Mathf.Clamp(Mathf.FloorToInt(left),0,width),x1=Mathf.Clamp(Mathf.CeilToInt(right),0,width),y0=Mathf.Clamp(Mathf.FloorToInt(bottom),0,height),y1=Mathf.Clamp(Mathf.CeilToInt(top),0,height);
   for(int y=y0;y<y1;y++)for(int x=x0;x<x1;x++){int covered=0;for(int j=0;j<2;j++)for(int i=0;i<2;i++)if(TerrainPolygonContains327(new Vector2(x+(i+.5f)*.5f,y+(j+.5f)*.5f),polygon))covered++;if(covered>0){int index=y*width+x;pixels[index]=Color.Lerp(pixels[index],color,covered*.25f);}}
  }
  void StripHarborObstacles327(RaidMap map,List<Rect> unsupported){
   Color32 high=PixelArt.Hex("D6DBD6"),low=PixelArt.Hex("7F8F86");var yard=map.Root.Find("Yard_GroundMaterials");var regionComponent=yard?yard.GetComponent<GroundSurfaceRegions>():null;
   for(int y=0;y<terrainHeight325;y++)for(int x=0;x<terrainWidth325;x++){
    int index=y*terrainWidth325+x;var original=terrainSource325[index];if(!original.Equals(high)&&!original.Equals(low))continue;
    var world=new Vector3(map.Bounds.xMin+(x+.5f)/terrainWidth325*map.Bounds.width,0,map.Bounds.yMin+(y+.5f)/terrainHeight325*map.Bounds.height);var point=new Vector2(world.x,world.z);bool retain=false;foreach(var bounds in unsupported)if(bounds.Contains(point)){retain=true;break;}if(retain)continue;
    Color color=terrainOutside325;foreach(var surface in map.HarborSurfaces)if(RaidMap.InPolygon(point,surface.Points))color=PixelArt.Hex(surface.Color);
    foreach(var road in map.HarborRoadShapes)if(RaidMap.InPolygon(point,road)&&map.OnHarborSurface(world))color=PixelArt.Hex("667077");
    if(regionComponent!=null)foreach(var region in regionComponent.Regions)if(region.Area.Contains(point))color=PixelArt.Hex(region.Finish==GroundFinish.Soil?"7D785E":region.Finish==GroundFinish.Gravel?"807E68":"858A78");
    if(map.StableGround137&&map.StableGroundBounds137.Contains(point)){var bounds=map.StableGroundBounds137;var paint=map.StableGround137.GetPixelBilinear((point.x-bounds.xMin)/bounds.width,(point.y-bounds.yMin)/bounds.height);if(paint.a>.1f)color=paint;}
    terrainSource325[index]=color;
   }
  }
  void EnsureTerrainSource325(RaidMap map,bool nearby){
   terrainRoot325=map.Root;terrainRevision325=map.LayoutRevision;terrainBounds325=map.Bounds;
   terrainShapes327.Clear();var unsupported=new List<Rect>();
   foreach(var collider in map.Root.GetComponentsInChildren<Collider>()){
    if(!collider.enabled||collider.isTrigger||collider.GetComponentInParent<RaidActor>()||(SceneObstacle.MovementMask&(1<<collider.gameObject.layer))==0)continue;
    var polygon=TerrainColliderPolygon327(collider);if(polygon.Length<3){var b=collider.bounds;unsupported.Add(new Rect(b.min.x,b.min.z,b.size.x,b.size.z));continue;}
    float left=float.MaxValue,right=float.MinValue,bottom=float.MaxValue,top=float.MinValue;foreach(var point in polygon){left=Mathf.Min(left,point.x);right=Mathf.Max(right,point.x);bottom=Mathf.Min(bottom,point.z);top=Mathf.Max(top,point.z);}terrainShapes327.Add(new TerrainShape327{Points=polygon,Projected=new Vector2[polygon.Length],WorldBounds=Rect.MinMaxRect(left,bottom,right,top),Low=collider.gameObject.layer==SceneObstacle.LowLayer});
   }
   Texture2D plan=map.HarborWhitebox?map.HarborPlanTexture():map.UndergroundWhitebox2752?map.UndergroundPlanTexture2752():null;
   terrainOutside325=map.HarborWhitebox?PixelArt.Hex("263E47"):new Color32(12,19,20,255);
   if(plan){terrainWidth325=plan.width;terrainHeight325=plan.height;terrainSource325=plan.GetPixels32();if(nearby&&map.HarborWhitebox)StripHarborObstacles327(map,unsupported);return;}
   terrainWidth325=Mathf.Clamp(Mathf.CeilToInt(map.Bounds.width*TerrainSourcePixelsPerMeter327),16,2048);terrainHeight325=Mathf.Clamp(Mathf.CeilToInt(map.Bounds.height*TerrainSourcePixelsPerMeter327),16,2048);terrainSource325=new Color32[terrainWidth325*terrainHeight325];
   Color32 ground=new Color(.23f,.29f,.26f);for(int i=0;i<terrainSource325.Length;i++)terrainSource325[i]=ground;
   // Rasterize the requested map's actual colliders once. M may request the
   // exterior source while G.Map is an interior, so do not borrow G.Map's cache.
   float sx=terrainWidth325/map.Bounds.width,sy=terrainHeight325/map.Bounds.height;
   foreach(var shape in terrainShapes327){var polygon=shape.Projected;for(int i=0;i<polygon.Length;i++)polygon[i]=new Vector2((shape.Points[i].x-map.Bounds.xMin)*sx,(shape.Points[i].z-map.Bounds.yMin)*sy);PaintTerrainShape327(terrainSource325,terrainWidth325,terrainHeight325,polygon,shape.Low?new Color(.52f,.46f,.33f):new Color(.60f,.64f,.54f));}
   // Unsupported imported mesh outlines remain in the original plan above.
   // Never call ClosestPoint on a nonconvex mesh, or replace it with a false box.
  }
  void ReleaseMapTerrain325(){if(nearbyFrame325.Image)Destroy(nearbyFrame325.Image);if(overviewFrame325.Image)Destroy(overviewFrame325.Image);nearbyFrame325.Pixels=overviewFrame325.Pixels=null;nearbySource327.Pixels=overviewSource327.Pixels=null;nearbySource327.Root=overviewSource327.Root=null;terrainSource325=null;terrainRoot325=null;}
 }
}
