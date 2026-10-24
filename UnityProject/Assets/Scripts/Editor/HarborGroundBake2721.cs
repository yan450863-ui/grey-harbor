using System;
using System.IO;
using System.Diagnostics;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using GreyHarbor;

public static class HarborGroundBake2721 {
 const string TexturePath="Assets/Resources/HarborStableGround2721.asset";
 const string PlanTexturePath="Assets/Resources/HarborPlan2721.asset";
 const string CachePath="Assets/Resources/HarborGroundCache2721.asset";
 const string PreviewPath="Assets/Environment/HarborGround2721/HarborStableGround2721.png";
 // The seed affects loot placement; the ground's ordered paint fingerprint must
 // match independently of the raid seed, otherwise runtime falls back safely.
 public static void Export(){
  if(Application.isPlaying)throw new InvalidOperationException("Ground export must run outside play mode");
  var timer=Stopwatch.StartNew();var active=SceneManager.GetActiveScene();var random=UnityEngine.Random.state;
  var temporary=EditorSceneManager.NewScene(NewSceneSetup.EmptyScene,NewSceneMode.Additive);
  SceneManager.SetActiveScene(temporary);var map=new RaidMap{BypassGroundCache2721=true};
  Texture2D exact=null,exactPlan=null;RaidMap verification=null;
  try{
   UnityEngine.Random.InitState(2721);map.BuildHarbor(2721);
   if(!map.StableGround137||map.GroundCacheUsed2721||String.IsNullOrEmpty(map.GroundPaintFingerprint2721))throw new InvalidOperationException("Ground export did not run the original harbor bake");
   var source=map.StableGround137;
   var planSource=map.HarborPlanTexture();
   if(map.HarborPlanCacheUsed2721||!planSource||String.IsNullOrEmpty(map.HarborPlanFingerprint2721))throw new InvalidOperationException("Plan export did not run the original harbor plan raster");
   Directory.CreateDirectory("Assets/Resources");Directory.CreateDirectory(Path.GetDirectoryName(PreviewPath));
   File.WriteAllBytes(PreviewPath,source.EncodeToPNG());AssetDatabase.ImportAsset(PreviewPath,ImportAssetOptions.ForceSynchronousImport);
   var importer=(TextureImporter)AssetImporter.GetAtPath(PreviewPath);
   importer.textureType=TextureImporterType.Default;importer.sRGBTexture=true;importer.isReadable=true;
   importer.alphaSource=TextureImporterAlphaSource.FromInput;importer.alphaIsTransparency=false;
   importer.mipmapEnabled=true;importer.mipmapFilter=TextureImporterMipFilter.BoxFilter;
   importer.mipMapsPreserveCoverage=false;importer.npotScale=TextureImporterNPOTScale.None;
   importer.textureCompression=TextureImporterCompression.Uncompressed;importer.crunchedCompression=false;
   importer.maxTextureSize=4096;importer.filterMode=FilterMode.Trilinear;importer.anisoLevel=8;importer.wrapMode=TextureWrapMode.Clamp;
   var platform=importer.GetPlatformTextureSettings("Standalone");platform.overridden=true;platform.maxTextureSize=4096;
   platform.format=TextureImporterFormat.RGBA32;platform.textureCompression=TextureImporterCompression.Uncompressed;importer.SetPlatformTextureSettings(platform);
   importer.SaveAndReimport();
   var preview=AssetDatabase.LoadAssetAtPath<Texture2D>(PreviewPath);
   if(preview.width!=source.width||preview.height!=source.height)throw new InvalidOperationException("Imported ground dimensions changed");
   VerifyPixels(source.GetPixels32(0),preview.GetPixels32(0),"PNG base RGBA");
   // Preserve the original CPU bake's complete mip chain byte for byte rather
   // than allowing the PNG importer to choose another mip generation method.
   exact=new Texture2D(source.width,source.height,TextureFormat.RGBA32,true,false){name="HarborStableGround2721",filterMode=source.filterMode,anisoLevel=source.anisoLevel,wrapMode=source.wrapMode};
   exact.LoadRawTextureData(source.GetRawTextureData());exact.Apply(false,false);
   var persisted=AssetDatabase.LoadAssetAtPath<Texture2D>(TexturePath);
   if(persisted){EditorUtility.CopySerialized(exact,persisted);UnityEngine.Object.DestroyImmediate(exact);exact=null;EditorUtility.SetDirty(persisted);}
   else{persisted=exact;AssetDatabase.CreateAsset(persisted,TexturePath);exact=null;}
   exactPlan=new Texture2D(planSource.width,planSource.height,TextureFormat.RGBA32,false,false){name="HarborPlan2721",filterMode=FilterMode.Point,wrapMode=TextureWrapMode.Clamp};
   exactPlan.LoadRawTextureData(planSource.GetRawTextureData());exactPlan.Apply(false,false);
   var persistedPlan=AssetDatabase.LoadAssetAtPath<Texture2D>(PlanTexturePath);
   if(persistedPlan){EditorUtility.CopySerialized(exactPlan,persistedPlan);UnityEngine.Object.DestroyImmediate(exactPlan);exactPlan=null;EditorUtility.SetDirty(persistedPlan);}
   else{persistedPlan=exactPlan;AssetDatabase.CreateAsset(persistedPlan,PlanTexturePath);exactPlan=null;}
   var metadata=AssetDatabase.LoadAssetAtPath<HarborGroundCache2721>(CachePath);
   if(!metadata){metadata=ScriptableObject.CreateInstance<HarborGroundCache2721>();AssetDatabase.CreateAsset(metadata,CachePath);}
   metadata.Version=HarborGroundCache2721.AlgorithmVersion;metadata.Width=source.width;metadata.Height=source.height;metadata.Bounds=map.StableGroundBounds137;
   metadata.PaintFingerprint=map.GroundPaintFingerprint2721;metadata.SourceFingerprint=SourceFingerprint();
   metadata.PixelHash=HarborGroundCache2721.HashBytes(source.GetRawTextureData());metadata.Texture=persisted;
   metadata.PlanBounds=map.Bounds;metadata.PlanTexture=persistedPlan;metadata.PlanFingerprint=map.HarborPlanFingerprint2721;
   metadata.PlanPixelHash=HarborGroundCache2721.HashBytes(planSource.GetRawTextureData());
   EditorUtility.SetDirty(metadata);AssetDatabase.SaveAssets();
   AssetDatabase.ImportAsset(TexturePath,ImportAssetOptions.ForceSynchronousImport|ImportAssetOptions.ForceUpdate);
   var reloaded=AssetDatabase.LoadAssetAtPath<Texture2D>(TexturePath);
   if(reloaded.width!=source.width||reloaded.height!=source.height||reloaded.mipmapCount!=source.mipmapCount||
      HarborGroundCache2721.HashBytes(reloaded.GetRawTextureData())!=metadata.PixelHash)throw new InvalidOperationException("Persisted ground or mip pixels differ from original bake");
   for(int mip=0;mip<source.mipmapCount;mip++)VerifyPixels(source.GetPixels32(mip),reloaded.GetPixels32(mip),"asset mip "+mip);
   AssetDatabase.ImportAsset(PlanTexturePath,ImportAssetOptions.ForceSynchronousImport|ImportAssetOptions.ForceUpdate);
   var reloadedPlan=AssetDatabase.LoadAssetAtPath<Texture2D>(PlanTexturePath);
   if(reloadedPlan.width!=planSource.width||reloadedPlan.height!=planSource.height||reloadedPlan.mipmapCount!=1||
      HarborGroundCache2721.HashBytes(reloadedPlan.GetRawTextureData())!=metadata.PlanPixelHash)throw new InvalidOperationException("Persisted harbor plan pixels differ from original raster");
   VerifyPixels(planSource.GetPixels32(),reloadedPlan.GetPixels32(),"harbor plan RGBA");
   Texture2D cached;
   if(!HarborGroundCache2721.TryGet(map.StableGroundBounds137,source.width,source.height,map.GroundPaintFingerprint2721,out cached))throw new InvalidOperationException("Exported ground metadata does not match runtime cache");
   // A second real map with another raid seed proves the runtime hit, including
   // material/plane creation; destroy the bake first to keep physics isolated.
   UnityEngine.Object.DestroyImmediate(map.Root.gameObject);map.Root=null;
   verification=new RaidMap();verification.BuildHarbor(918273);
   if(!verification.GroundCacheUsed2721||verification.StableGround137!=cached||verification.GroundPaintFingerprint2721!=metadata.PaintFingerprint)throw new InvalidOperationException("Second-seed runtime map did not reuse the exact ground cache");
   var cachedPlan=verification.HarborPlanTexture();
   if(!verification.HarborPlanCacheUsed2721||cachedPlan!=metadata.PlanTexture||verification.HarborPlanFingerprint2721!=metadata.PlanFingerprint)throw new InvalidOperationException("Second-seed runtime map did not reuse the exact harbor plan cache");
   int mips=cached.mipmapCount;
   UnityEngine.Object.DestroyImmediate(verification.Root.gameObject);verification.Root=null;
   if(!cached||cached.GetRawTextureData().Length==0)throw new InvalidOperationException("Map destruction destroyed the shared ground cache");
   if(!cachedPlan||HarborGroundCache2721.HashBytes(cachedPlan.GetRawTextureData())!=metadata.PlanPixelHash)throw new InvalidOperationException("Map destruction destroyed or changed the shared harbor plan cache");
   UnityEngine.Debug.Log("PLAN CACHE 2721: exact RGBA verified; runtime second-seed hit + shared lifetime verified; "+cachedPlan.width+"x"+cachedPlan.height+" bounds="+metadata.PlanBounds+" geometry="+metadata.PlanFingerprint+" pixels="+metadata.PlanPixelHash);
   UnityEngine.Debug.Log("GROUND CACHE 2721: exact RGBA + all "+mips+" mips verified; runtime second-seed hit + shared lifetime verified; "+metadata.Width+"x"+metadata.Height+" bounds="+metadata.Bounds+" paint="+metadata.PaintFingerprint+" pixels="+metadata.PixelHash+" source="+metadata.SourceFingerprint+" bakeMs="+timer.ElapsedMilliseconds);
  }finally{
   if(exact)UnityEngine.Object.DestroyImmediate(exact);
   if(exactPlan)UnityEngine.Object.DestroyImmediate(exactPlan);
   if(verification!=null&&verification.Root)UnityEngine.Object.DestroyImmediate(verification.Root.gameObject);
   if(map.Root)UnityEngine.Object.DestroyImmediate(map.Root.gameObject);
   EditorSceneManager.CloseScene(temporary,true);
   if(active.IsValid()&&active.isLoaded)SceneManager.SetActiveScene(active);
   UnityEngine.Random.state=random;
  }
 }
 static void VerifyPixels(Color32[] a,Color32[] b,string label){
  if(a.Length!=b.Length)throw new InvalidOperationException(label+" pixel count differs");
  for(int i=0;i<a.Length;i++)if(a[i].r!=b[i].r||a[i].g!=b[i].g||a[i].b!=b[i].b||a[i].a!=b[i].a)throw new InvalidOperationException(label+" changed pixel "+i);
 }
 static string SourceFingerprint(){
  var files=Directory.GetFiles("Assets/Scripts/Runtime","*.cs",SearchOption.AllDirectories);Array.Sort(files,StringComparer.Ordinal);
  using(var bytes=new MemoryStream())using(var writer=new BinaryWriter(bytes)){
   foreach(var file in files){writer.Write(file.Replace('\\','/'));writer.Write(File.ReadAllBytes(file));}
   writer.Flush();return HarborGroundCache2721.HashBytes(bytes.ToArray());
  }
 }
}
