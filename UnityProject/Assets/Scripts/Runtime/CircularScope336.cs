using UnityEngine;
namespace GreyHarbor {
 public static class CircularScope336 {
  public const float CrossLength=15f,CrossThickness=1.1f;
  const int Size=288;const float Density=2f,Radius=60f;
  static Texture2D border;
  static float Band(float radius,float middle,float width)=>Mathf.Clamp01((width*.5f-Mathf.Abs(radius-middle))*Density+.5f);
  static Color Over(Color under,Color top,float alpha){alpha*=top.a;float a=alpha+under.a*(1-alpha);if(a<=0)return Color.clear;return new Color((top.r*alpha+under.r*under.a*(1-alpha))/a,(top.g*alpha+under.g*under.a*(1-alpha))/a,(top.b*alpha+under.b*under.a*(1-alpha))/a,a);}
  public static Texture2D Border {
   get {if(border)return border;border=new Texture2D(Size,Size,TextureFormat.RGBA32,false){name="CircularScopeMetalRim336",filterMode=FilterMode.Bilinear,wrapMode=TextureWrapMode.Clamp};var pixels=new Color32[Size*Size];
    for(int y=0;y<Size;y++)for(int x=0;x<Size;x++){
     float px=(x+.5f-Size*.5f)/Density,py=(y+.5f-Size*.5f)/Density,r=Mathf.Sqrt(px*px+py*py);Color c=Color.clear;
     c=Over(c,PixelArt.Hex("10191B"),Band(r,63,4));c=Over(c,PixelArt.Hex("586967"),Band(r,61,1));c=Over(c,PixelArt.Hex("97AEB5"),Band(r,59,.8f)*.85f);c=Over(c,PixelArt.Hex("1F2C2D"),Band(r,58,1.5f));
     float axis=Mathf.Min(Mathf.Abs(px),Mathf.Abs(py)),longAxis=Mathf.Max(Mathf.Abs(px),Mathf.Abs(py));float tick=Mathf.Clamp01((.5f-axis)*Density+.5f)*Mathf.Clamp01((longAxis-53)*Density+.5f)*Mathf.Clamp01((57-longAxis)*Density+.5f);
     pixels[y*Size+x]=Over(c,PixelArt.Hex("B1BABA"),tick);
    }
    border.SetPixels32(pixels);border.Apply(false,false);return border;
   }
  }
  public static Rect BorderRect(Rect aperture){float support=aperture.width*(Size/(Density*Radius*2));return new Rect(aperture.center.x-support*.5f,aperture.center.y-support*.5f,support,support);}
  public static Rect CrossHorizontal(Rect aperture){float s=aperture.width/120;return new Rect(aperture.center.x-CrossLength*s*.5f,aperture.center.y-CrossThickness*s*.5f,CrossLength*s,CrossThickness*s);}
  public static Rect CrossVertical(Rect aperture){float s=aperture.width/120;return new Rect(aperture.center.x-CrossThickness*s*.5f,aperture.center.y-CrossLength*s*.5f,CrossThickness*s,CrossLength*s);}
  public static void Draw(Rect aperture,Color crossColor,float opacity,bool showCross){
   if(Event.current.type!=EventType.Repaint)return;var matrix=GUI.matrix;var color=GUI.color;
   try {GUI.matrix=Matrix4x4.identity;GUI.color=new Color(1,1,1,Mathf.Clamp01(opacity));GUI.DrawTexture(BorderRect(aperture),Border,ScaleMode.StretchToFill,true);if(showCross){crossColor.a*=Mathf.Clamp01(opacity);GUI.color=crossColor;GUI.DrawTexture(CrossHorizontal(aperture),Texture2D.whiteTexture);GUI.DrawTexture(CrossVertical(aperture),Texture2D.whiteTexture);}}
   finally{GUI.matrix=matrix;GUI.color=color;}
  }
 }
 public sealed partial class RaidGame {
  // Reuse the projected ballistic aim after the existing ADS recoil multiplier.
  // No second shake, camera movement or texture copy is added here.
  public Vector2 ScopeCenter336=>AkCrosshairScreen276;
  public Rect ScopeFrame336=>ScopePresentation305.Frame(Screen.width,Screen.height,ScopeCenter336,true);
 }
}
