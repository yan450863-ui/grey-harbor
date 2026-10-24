using System.Collections.Generic;using UnityEngine;
namespace GreyHarbor {public static class WeaponSocket307 {
 static readonly Dictionary<ItemKind,Vector3> sockets=new Dictionary<ItemKind,Vector3>();
 public static Vector3 Local(ItemKind kind){
  if(sockets.TryGetValue(kind,out var socket))return socket;var sprite=PixelArt.WeaponSprite279(kind);var rect=sprite.rect;var texture=sprite.texture;var pixels=texture.GetPixels32();int xEnd=-1;float ySum=0,weight=0;
  int x0=Mathf.RoundToInt(rect.x),x1=Mathf.RoundToInt(rect.xMax),y0=Mathf.RoundToInt(rect.y),y1=Mathf.RoundToInt(rect.yMax);
  for(int x=x1-1;x>=x0&&xEnd<0;x--)for(int y=y0;y<y1;y++)if(pixels[y*texture.width+x].a>96){xEnd=x;break;}
  int band=Mathf.Max(1,Mathf.RoundToInt(rect.width*.015f));for(int x=Mathf.Max(x0,xEnd-band+1);x<=xEnd;x++)for(int y=y0;y<y1;y++){var a=pixels[y*texture.width+x].a;if(a>96){ySum+=(y+.5f)*a;weight+=a;}}
  if(xEnd<0||weight==0)throw new System.InvalidOperationException("武器图片没有可定位的枪口："+kind);
  socket=new Vector3((xEnd+1-rect.x-sprite.pivot.x)/sprite.pixelsPerUnit,(ySum/weight-rect.y-sprite.pivot.y)/sprite.pixelsPerUnit,0);sockets[kind]=socket;return socket;
 }
}}
