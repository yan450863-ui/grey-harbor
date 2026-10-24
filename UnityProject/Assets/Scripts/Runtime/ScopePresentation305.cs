using UnityEngine;
namespace GreyHarbor {
 // Only darken the original view outside a circle. No camera, copying, zoom or scene texture.
 public static class ScopePresentation305 {
  public const float DimAlpha=.62f;
  static Texture2D corners,ring,dot,fullscreenMask,fullscreenRing;
  public static float Scale(int width,int height)=>Mathf.Min(width/1600f,height/900f);
  public static Rect Frame(int width,int height,Vector2 adjustedAimScreen,bool blue){
   float diameter=(blue?120:100)*Scale(width,height);
   return new Rect(adjustedAimScreen.x-diameter*.5f,height-adjustedAimScreen.y-diameter*.5f,diameter,diameter);
  }
  public static Texture2D CornerMask {
   get {
    if(corners)return corners;
    const int size=256;corners=new Texture2D(size,size,TextureFormat.RGBA32,false){name="OriginalViewScopeMask305",filterMode=FilterMode.Bilinear,wrapMode=TextureWrapMode.Clamp};
    var pixels=new Color32[size*size];
    for(int y=0;y<size;y++)for(int x=0;x<size;x++){
     float dx=x+.5f-size*.5f,dy=y+.5f-size*.5f;
     float outside=Mathf.Clamp01(Mathf.Sqrt(dx*dx+dy*dy)-(size*.5f-1));
     pixels[y*size+x]=new Color32(0,0,0,(byte)Mathf.RoundToInt(255*DimAlpha*outside));
    }
    corners.SetPixels32(pixels);corners.Apply();return corners;
   }
  }
  // Alpha-only circle textures contain no rectangular edges or crosshair line arms.
  public static Texture2D RingMask306 {
   get {
    if(ring)return ring;const int size=256;ring=new Texture2D(size,size,TextureFormat.RGBA32,false){name="ScopeRingOnly306",filterMode=FilterMode.Bilinear,wrapMode=TextureWrapMode.Clamp};var pixels=new Color32[size*size];
    for(int y=0;y<size;y++)for(int x=0;x<size;x++){float dx=x+.5f-size*.5f,dy=y+.5f-size*.5f,distance=Mathf.Sqrt(dx*dx+dy*dy);float a=Mathf.Clamp01(size*.5f-distance)*Mathf.Clamp01(distance-(size*.5f-3));pixels[y*size+x]=new Color32(255,255,255,(byte)Mathf.RoundToInt(a*255));}
    ring.SetPixels32(pixels);ring.Apply();return ring;
   }
  }
  public static Texture2D DotMask306 {
   get {
    if(dot)return dot;const int size=16;dot=new Texture2D(size,size,TextureFormat.RGBA32,false){name="ScopeCenterDot306",filterMode=FilterMode.Bilinear,wrapMode=TextureWrapMode.Clamp};var pixels=new Color32[size*size];
    for(int y=0;y<size;y++)for(int x=0;x<size;x++){float dx=x+.5f-size*.5f,dy=y+.5f-size*.5f,a=Mathf.Clamp01(size*.5f-Mathf.Sqrt(dx*dx+dy*dy));pixels[y*size+x]=new Color32(255,255,255,(byte)Mathf.RoundToInt(a*255));}dot.SetPixels32(pixels);dot.Apply();return dot;
   }
  }
  public static Rect[] OuterRegions(int width,int height,Rect frame){
   float x0=Mathf.Clamp(frame.xMin,0,width),x1=Mathf.Clamp(frame.xMax,0,width),y0=Mathf.Clamp(frame.yMin,0,height),y1=Mathf.Clamp(frame.yMax,0,height);
   return new[]{new Rect(0,0,x0,height),new Rect(x1,0,width-x1,height),new Rect(x0,0,x1-x0,y0),new Rect(x0,y1,x1-x0,height-y1)};
  }
  // Border pixels of the padded masks are uniform, including the tangent rows/columns.
  public static Texture2D FullscreenMask307=>MakeFullscreen307(false);
  public static Texture2D FullscreenRing307=>MakeFullscreen307(true);
  static Texture2D MakeFullscreen307(bool rim){
   var cached=rim?fullscreenRing:fullscreenMask;if(cached)return cached;const int size=512;const float radius=128;
   var texture=new Texture2D(size,size,TextureFormat.RGBA32,false){name=rim?"WholeWindowScopeRing307":"WholeWindowScopeShade307",filterMode=FilterMode.Bilinear,wrapMode=TextureWrapMode.Clamp};var pixels=new Color32[size*size];
   for(int y=0;y<size;y++)for(int x=0;x<size;x++){float dx=x+.5f-size*.5f,dy=y+.5f-size*.5f,distance=Mathf.Sqrt(dx*dx+dy*dy);float a=rim?Mathf.Clamp01(radius-distance)*Mathf.Clamp01(distance-(radius-3)):DimAlpha*Mathf.Clamp01(distance-radius);pixels[y*size+x]=new Color32(rim?(byte)255:(byte)0,rim?(byte)255:(byte)0,rim?(byte)255:(byte)0,(byte)Mathf.RoundToInt(255*a));}
   texture.SetPixels32(pixels);texture.Apply();if(rim)fullscreenRing=texture;else fullscreenMask=texture;return texture;
  }
  public static Rect ViewportUV307(int width,int height,Rect frame){float support=frame.width*2;return new Rect(.5f-frame.center.x/support,.5f-(height-frame.center.y)/support,width/support,height/support);}
  public static void Draw(int width,int height,Rect frame,Color reticle,Color rim,float opacity=1,Texture2D shade=null,bool drawShade=true){
   if(Event.current.type!=EventType.Repaint)return;
   var matrix=GUI.matrix;var color=GUI.color;
   try {
    GUI.matrix=Matrix4x4.identity;opacity=Mathf.Clamp01(opacity);GUI.color=new Color(1,1,1,opacity);
    var screen=new Rect(0,0,width,height);var uv=ViewportUV307(width,height,frame);
    // Both layers use a single whole-window quad; recoil changes UVs, never joins between rectangles.
    if(drawShade)GUI.DrawTextureWithTexCoords(screen,shade?shade:FullscreenMask307,uv,true);
    GUI.color=new Color(rim.r,rim.g,rim.b,rim.a*opacity);GUI.DrawTextureWithTexCoords(screen,FullscreenRing307,uv,true);
    // RaidUI draws the shared live spread reticle after this shade/rim layer.
   } finally {GUI.matrix=matrix;GUI.color=color;}
  }
 }
}
