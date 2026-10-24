using System.Collections.Generic;
using System.IO;
using UnityEngine;
namespace GreyHarbor {
 public static class BlueWeaponsArt30 {
  static readonly Dictionary<ItemKind,Texture2D> textures=new Dictionary<ItemKind,Texture2D>();
  static readonly Dictionary<ItemKind,Sprite> sprites=new Dictionary<ItemKind,Sprite>();
  public static Texture2D Texture(ItemKind k){if(textures.TryGetValue(k,out var t))return t;
   if(BlueWeapons30.IsGun(k)){t=Resources.Load<Texture2D>("ItemArt/"+k);if(t==null){string file=Path.Combine(Application.streamingAssetsPath,"Weapons30",k+".png");if(!File.Exists(file))throw new FileNotFoundException("缺少3.0武器图标",file);t=new Texture2D(2,2,TextureFormat.RGBA32,false);if(!t.LoadImage(File.ReadAllBytes(file),false))throw new IOException("无法读取武器图标");}t.filterMode=FilterMode.Bilinear;t.wrapMode=TextureWrapMode.Clamp;}
   else t=FormalWeaponArt305.Texture(k);
   textures[k]=t;return t;
  }
  public static Sprite Sprite(ItemKind k){if(sprites.TryGetValue(k,out var s))return s;var t=Texture(k);var uv=ItemIcons.Bounds(k);var rect=new Rect(uv.x*t.width,uv.y*t.height,uv.width*t.width,uv.height*t.height);float width=k==ItemKind.Dagger?.9f:1.4f;s=UnityEngine.Sprite.Create(t,rect,new Vector2(.35f,.48f),rect.width/width);s.name=k+"30";sprites[k]=s;return s;}
  public static float Muzzle(ItemKind k)=>k==ItemKind.Dagger?.9f*.65f:1.4f*.65f;
 }
}