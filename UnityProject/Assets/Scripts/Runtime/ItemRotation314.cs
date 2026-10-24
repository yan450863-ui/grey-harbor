using System.Collections.Generic;using UnityEngine;
namespace GreyHarbor {
 public static class ItemRotation314 {
  static readonly Dictionary<ItemKind,Texture2D> rotated=new Dictionary<ItemKind,Texture2D>();
  public static Texture2D Get(ItemKind kind){if(rotated.TryGetValue(kind,out var found)&&found)return found;var source=ItemIcons.Get(kind);var uv=ItemIcons.Bounds(kind);int x0=Mathf.RoundToInt(uv.x*source.width),y0=Mathf.RoundToInt(uv.y*source.height),w=Mathf.Max(1,Mathf.RoundToInt(uv.width*source.width)),h=Mathf.Max(1,Mathf.RoundToInt(uv.height*source.height));var input=source.GetPixels32();var output=new Color32[w*h];for(int y=0;y<h;y++)for(int x=0;x<w;x++)output[(w-1-x)*h+y]=input[(y0+y)*source.width+x0+x];var texture=new Texture2D(h,w,TextureFormat.RGBA32,false){filterMode=FilterMode.Point,wrapMode=TextureWrapMode.Clamp,name="RotatedInventory314_"+kind};texture.SetPixels32(output);texture.Apply();rotated[kind]=texture;return texture;}
  public static Rect Fit(Rect r,float aspect){float width=Mathf.Min(r.width,r.height*aspect),height=width/aspect;return new Rect(r.center.x-width*.5f,r.center.y-height*.5f,width,height);}
 }
}
