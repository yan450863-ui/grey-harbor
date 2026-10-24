using System;
using System.Security.Cryptography;
using UnityEngine;

namespace GreyHarbor {
 public sealed class HarborGroundCache2721:ScriptableObject {
  public const int AlgorithmVersion=2750;
  public const string ResourceName="HarborGroundCache2721";
  public int Version,Width,Height;
  public Rect Bounds;
  public string PaintFingerprint,SourceFingerprint,PixelHash;
  public Texture2D Texture;
  public Rect PlanBounds;
  public string PlanPixelHash,PlanFingerprint;
  public Texture2D PlanTexture;
  static HarborGroundCache2721 loaded;
  public static bool TryGet(Rect bounds,int width,int height,string fingerprint,out Texture2D texture){
   texture=null;
   if(!loaded)loaded=Resources.Load<HarborGroundCache2721>(ResourceName);
   if(!loaded||loaded.Version!=AlgorithmVersion||loaded.Width!=width||loaded.Height!=height||
      !loaded.Bounds.Equals(bounds)||!String.Equals(loaded.PaintFingerprint,fingerprint,StringComparison.Ordinal))return false;
   var candidate=loaded.Texture;
   if(!candidate||candidate.width!=width||candidate.height!=height||candidate.format!=TextureFormat.RGBA32||
      !candidate.isReadable||candidate.mipmapCount<=1||candidate.filterMode!=FilterMode.Trilinear||
      candidate.wrapMode!=TextureWrapMode.Clamp||candidate.anisoLevel!=8)return false;
   texture=candidate;return true;
  }
  public static bool TryGetPlan(Rect bounds,int width,int height,string paintFingerprint,string planFingerprint,out Texture2D texture){
   texture=null;
   if(!loaded)loaded=Resources.Load<HarborGroundCache2721>(ResourceName);
   if(!loaded||loaded.Version!=AlgorithmVersion||!loaded.PlanBounds.Equals(bounds)||
      !String.Equals(loaded.PaintFingerprint,paintFingerprint,StringComparison.Ordinal)||
      !String.Equals(loaded.PlanFingerprint,planFingerprint,StringComparison.Ordinal)||String.IsNullOrEmpty(loaded.PlanPixelHash))return false;
   var candidate=loaded.PlanTexture;
   if(!candidate||candidate.width!=width||candidate.height!=height||candidate.format!=TextureFormat.RGBA32||
      !candidate.isReadable||candidate.mipmapCount!=1||candidate.filterMode!=FilterMode.Point||candidate.wrapMode!=TextureWrapMode.Clamp)return false;
   texture=candidate;return true;
  }
  public static string HashBytes(byte[] bytes){
   using(var hash=SHA256.Create())return BitConverter.ToString(hash.ComputeHash(bytes)).Replace("-","").ToLowerInvariant();
  }
 }
}
