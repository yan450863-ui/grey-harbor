using UnityEngine;
namespace GreyHarbor {
 // A padded alpha mask uses the existing original-view UI path: no second
 // camera, scene copy, post-processing render target or extra shader bundle.
 public static class ScopeFog334 {
  public const float FogOpacity=.93f;public const int Size=256;
  static Texture2D mask;static Color32[] pixels;static Vector3 previousOrigin,previousX,previousY,previousEye;static float previousRadius=-1,previousSupport;public static int Uploads {get;private set;}
  public static Vector3 GroundPoint(Camera camera,Vector2 pixel,float y){var ray=camera.ScreenPointToRay(pixel);var plane=new Plane(Vector3.up,new Vector3(0,y,0));return plane.Raycast(ray,out float distance)?ray.GetPoint(distance):new Vector3(ray.origin.x,y,ray.origin.z);}
  public static float ClearAmount(Vector2 offsetPixels,float radiusPixels,Vector3 ground,Vector3 player){var d=ground-player;d.y=0;float circle=Mathf.Clamp01(radiusPixels-offsetPixels.magnitude);float range=Mathf.Clamp01((RaidVision.Range-d.magnitude)/.12f);return circle*range;}
  public static Texture2D Mask(Camera view,int width,int height,Rect frame,Vector3 eye){
   float support=frame.width*2,radius=frame.width*.5f;var center=new Vector2(frame.center.x,height-frame.center.y);var bottomLeft=center-Vector2.one*support*.5f;
   var origin=GroundPoint(view,bottomLeft,eye.y);var stepX=(GroundPoint(view,bottomLeft+Vector2.right*support,eye.y)-origin)/Size;var stepY=(GroundPoint(view,bottomLeft+Vector2.up*support,eye.y)-origin)/Size;
   if(mask&&previousOrigin==origin&&previousX==stepX&&previousY==stepY&&previousEye==eye&&previousRadius==radius&&previousSupport==support)return mask;
   if(!mask){mask=new Texture2D(Size,Size,TextureFormat.RGBA32,false){name="ScopeGrayFog334",filterMode=FilterMode.Bilinear,wrapMode=TextureWrapMode.Clamp};pixels=new Color32[Size*Size];}
   // Uniform padded edges prevent the old right/bottom tangent seam during recoil.
   var fog=new Color32(42,42,42,(byte)Mathf.RoundToInt(255*FogOpacity));for(int i=0;i<pixels.Length;i++)pixels[i]=fog;
   for(int y=Size/4-2;y<=Size*3/4+2;y++)for(int x=Size/4-2;x<=Size*3/4+2;x++){
    var offset=new Vector2(x+.5f-Size*.5f,y+.5f-Size*.5f)*(support/Size);var ground=origin+stepX*(x+.5f)+stepY*(y+.5f);float clear=ClearAmount(offset,radius,ground,eye);pixels[y*Size+x]=new Color32(42,42,42,(byte)Mathf.RoundToInt(255*FogOpacity*(1-clear)));
   }
   mask.SetPixels32(pixels);mask.Apply(false,false);previousOrigin=origin;previousX=stepX;previousY=stepY;previousEye=eye;previousRadius=radius;previousSupport=support;Uploads++;return mask;
  }
 }
}
