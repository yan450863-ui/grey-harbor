using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
namespace GreyHarbor {
 public static class FormalWeaponArt305 {
  static readonly Dictionary<string,Texture2D> cache=new Dictionary<string,Texture2D>();
  public static string AssetName(ItemKind kind){
   if(kind==ItemKind.Green556||kind==ItemKind.Blue556)return "Five56";
   if(kind==ItemKind.Green12Gauge||kind==ItemKind.Blue12Gauge)return "Gauge12";
   if(ModRules30.IsMod(kind))return ModRules30.Get(kind).Slot+"30";
   throw new ArgumentOutOfRangeException(nameof(kind));
  }
  public static string FilePath(ItemKind kind)=>Path.Combine(Application.streamingAssetsPath,"ItemArt305",AssetName(kind)+".png");
  public static Texture2D Texture(ItemKind kind){
   string name=AssetName(kind);if(cache.TryGetValue(name,out var texture))return texture;
   texture=Resources.Load<Texture2D>("ItemArt/"+name);
   if(!texture){string path=FilePath(kind);if(!File.Exists(path))throw new FileNotFoundException("缺少正式弹药或配件图片",path);
    texture=new Texture2D(2,2,TextureFormat.RGBA32,false);if(!texture.LoadImage(File.ReadAllBytes(path),false)){UnityEngine.Object.Destroy(texture);throw new IOException("无法读取正式物品图片："+path);}
   }
   texture.name="FormalWeaponArt305/"+name;texture.filterMode=FilterMode.Bilinear;texture.wrapMode=TextureWrapMode.Clamp;cache[name]=texture;return texture;
  }
 }
}
