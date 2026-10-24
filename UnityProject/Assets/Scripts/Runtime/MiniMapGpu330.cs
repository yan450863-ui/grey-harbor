using UnityEngine;
using UnityEngine.Rendering;
using System.Collections.Generic;
namespace GreyHarbor {
 public sealed partial class RaidUI {
  Texture2D terrainAtlas330;RenderTexture terrainTarget330,terrainBaked330;Mesh terrainQuad330,terrainObstacles330;Material terrainMaterial330,obstacleMaterial330;
  Transform gpuRoot330;Rect gpuBounds330;int gpuRevision330=-1;bool gpuWarned330;
  internal Texture PreparedTerrain331 {get;private set;}
  internal MiniMapProjection PreparedProjection331 {get;private set;}
  void LateUpdate(){PrepareNearbyMap331();}
  internal void PrepareNearbyMap331(){
   if(G==null||!G.Player||G.Map==null||!G.View||G.IsLoading2721||G.Phase!=GamePhase.Raid){PreparedTerrain331=null;return;}
   PreparedProjection331=new MiniMapProjection(NearbyBounds318(G.Player.transform.position),MiniMapViewport326,G.View.transform.rotation);
   PreparedTerrain331=TerrainPicture332(G.Map,PreparedProjection331,TerrainScreenScale327*HudScale330);
  }
  internal int TerrainAtlasBuilds330 {get;private set;}
  internal int TerrainGpuDraws330 {get;private set;}
  internal static Matrix4x4 TerrainMatrix330(MiniMapProjection projection){
   var r=projection.Viewport;var o=projection.Point(Vector3.zero);var x=projection.Point(Vector3.right)-o;var z=projection.Point(Vector3.forward)-o;
   var m=Matrix4x4.identity;m.SetColumn(0,new Vector4(x.x/r.width,-x.y/r.height,0,0));m.SetColumn(1,new Vector4(z.x/r.width,-z.y/r.height,0,0));m.SetColumn(3,new Vector4((o.x-r.x)/r.width,(r.yMax-o.y)/r.height,0,1));return m;
  }
  internal Texture TerrainGpuImage330(RaidMap map,MiniMapProjection projection,float guiScale){
   if(!EnsureTerrainGpu330(map))return TerrainImageScale327(map,projection,guiScale);
   var size=TerrainOutputSize327(projection.Viewport,guiScale);
   if(!terrainTarget330||terrainTarget330.width!=size.x||terrainTarget330.height!=size.y||!terrainTarget330.IsCreated()){
    if(terrainTarget330){terrainTarget330.Release();Destroy(terrainTarget330);}
    terrainTarget330=new RenderTexture(size.x,size.y,0,RenderTextureFormat.ARGB32){name="NearbyTerrain330",filterMode=FilterMode.Bilinear,wrapMode=TextureWrapMode.Clamp,antiAliasing=1};terrainTarget330.Create();
   }
   var prior=RenderTexture.active;GL.PushMatrix();
   try {
    RenderTexture.active=terrainTarget330;GL.Clear(false,true,nearbySource327.Outside);GL.LoadPixelMatrix(0,1,0,1);
    var matrix=TerrainMatrix330(projection);terrainMaterial330.SetPass(0);Graphics.DrawMeshNow(terrainQuad330,matrix);
    TerrainGpuDraws330++;
   }finally {GL.PopMatrix();RenderTexture.active=prior;}
   return terrainTarget330;
  }
  bool EnsureTerrainGpu330(RaidMap map){
   if(terrainAtlas330&&terrainBaked330&&terrainBaked330.IsCreated()&&gpuRoot330==map.Root&&gpuRevision330==map.LayoutRevision&&gpuBounds330==map.Bounds)return true;
   if(!terrainMaterial330){var textured=Shader.Find("Sprites/Default");var colored=Shader.Find("Sprites/Default");if(!textured||!colored||!textured.isSupported||!colored.isSupported){if(!gpuWarned330){Debug.LogWarning("Nearby map GPU shader unavailable; using CPU compatibility rendering.");gpuWarned330=true;}return false;}
    terrainMaterial330=new Material(textured){hideFlags=HideFlags.HideAndDontSave};obstacleMaterial330=new Material(colored){hideFlags=HideFlags.HideAndDontSave};obstacleMaterial330.SetInt("_SrcBlend",(int)BlendMode.SrcAlpha);obstacleMaterial330.SetInt("_DstBlend",(int)BlendMode.OneMinusSrcAlpha);obstacleMaterial330.SetInt("_Cull",(int)CullMode.Off);obstacleMaterial330.SetInt("_ZWrite",0);obstacleMaterial330.SetInt("_ZTest",(int)CompareFunction.Always);
   }
   UseTerrainSource327(map,true);
   if(terrainAtlas330)Destroy(terrainAtlas330);if(terrainQuad330)Destroy(terrainQuad330);if(terrainObstacles330)Destroy(terrainObstacles330);
   terrainAtlas330=new Texture2D(terrainWidth325,terrainHeight325,TextureFormat.RGBA32,false){name="NearbyAtlas330",filterMode=FilterMode.Bilinear,wrapMode=TextureWrapMode.Clamp};terrainAtlas330.SetPixels32(terrainSource325);terrainAtlas330.Apply(false,true);terrainMaterial330.mainTexture=terrainAtlas330;obstacleMaterial330.mainTexture=Texture2D.whiteTexture;
   var b=map.Bounds;terrainQuad330=new Mesh{name="NearbyGround330"};terrainQuad330.vertices=new[]{new Vector3(b.xMin,b.yMin,0),new Vector3(b.xMax,b.yMin,0),new Vector3(b.xMax,b.yMax,0),new Vector3(b.xMin,b.yMax,0)};terrainQuad330.uv=new[]{Vector2.zero,Vector2.right,Vector2.one,Vector2.up};
   // Both windings keep the built-in unlit shader valid with either camera basis.
   terrainQuad330.colors=new[]{Color.white,Color.white,Color.white,Color.white};terrainQuad330.triangles=new[]{0,1,2,0,2,3};terrainQuad330.UploadMeshData(true);
   if(map.HarborWhitebox){var verts=new List<Vector3>();var colors=new List<Color32>();var indices=new List<int>();foreach(var shape in nearbySource327.Shapes){int start=verts.Count;Color32 color=PixelArt.Hex(shape.Low?"7F8F86":"D6DBD6");foreach(var p in shape.Points){verts.Add(new Vector3(p.x,p.z,0));colors.Add(color);}for(int i=1;i<shape.Points.Length-1;i++){indices.Add(start);indices.Add(start+i);indices.Add(start+i+1);}}terrainObstacles330=new Mesh{name="NearbyObstacles330",indexFormat=IndexFormat.UInt32};terrainObstacles330.SetVertices(verts);terrainObstacles330.SetColors(colors);terrainObstacles330.SetTriangles(indices,0);terrainObstacles330.UploadMeshData(true);}
   BakeTerrainPicture330(map);
   gpuRoot330=map.Root;gpuRevision330=map.LayoutRevision;gpuBounds330=map.Bounds;TerrainAtlasBuilds330++;return true;
  }
  void BakeTerrainPicture330(RaidMap map){
   if(terrainBaked330){terrainBaked330.Release();Destroy(terrainBaked330);}
   int w=Mathf.Clamp(Mathf.CeilToInt(map.Bounds.width*20),64,4096),h=Mathf.Clamp(Mathf.CeilToInt(map.Bounds.height*20),64,4096);
   terrainBaked330=new RenderTexture(w,h,0,RenderTextureFormat.ARGB32){name="CachedMapPicture330",filterMode=FilterMode.Bilinear,wrapMode=TextureWrapMode.Clamp,useMipMap=true,autoGenerateMips=false};terrainBaked330.Create();
   var prior=RenderTexture.active;GL.PushMatrix();
   try{RenderTexture.active=terrainBaked330;GL.Clear(false,true,nearbySource327.Outside);GL.LoadPixelMatrix(0,1,0,1);
    var b=map.Bounds;var matrix=Matrix4x4.TRS(new Vector3(-b.xMin/b.width,-b.yMin/b.height,0),Quaternion.identity,new Vector3(1/b.width,1/b.height,1));
    terrainMaterial330.SetPass(0);Graphics.DrawMeshNow(terrainQuad330,matrix);
    if(terrainObstacles330&&terrainObstacles330.vertexCount>0){obstacleMaterial330.SetPass(0);Graphics.DrawMeshNow(terrainObstacles330,matrix);}
   }finally{GL.PopMatrix();RenderTexture.active=prior;}
   terrainBaked330.GenerateMips();terrainMaterial330.mainTexture=terrainBaked330;
  }
  void ReleaseMapGpu330(){if(terrainBaked330){terrainBaked330.Release();Destroy(terrainBaked330);}if(terrainTarget330){terrainTarget330.Release();Destroy(terrainTarget330);}if(terrainAtlas330)Destroy(terrainAtlas330);if(terrainQuad330)Destroy(terrainQuad330);if(terrainObstacles330)Destroy(terrainObstacles330);if(terrainMaterial330)Destroy(terrainMaterial330);if(obstacleMaterial330)Destroy(obstacleMaterial330);}
 }
}
