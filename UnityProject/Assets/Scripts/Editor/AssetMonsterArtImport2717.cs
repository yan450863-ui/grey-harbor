using UnityEngine;
using UnityEditor;
using GreyHarbor;
public sealed class AssetMonsterArtImport2717 : AssetPostprocessor {
 const string prefix="Assets/Resources/AssetShowcase2717/Monsters/";
 void OnPreprocessTexture(){if(!assetPath.StartsWith(prefix)||!assetPath.EndsWith(".png"))return;var t=(TextureImporter)assetImporter;
  t.textureType=TextureImporterType.Default;t.alphaSource=TextureImporterAlphaSource.FromInput;t.alphaIsTransparency=true;t.sRGBTexture=true;t.isReadable=true;t.mipmapEnabled=false;
  t.filterMode=FilterMode.Point;t.wrapMode=TextureWrapMode.Clamp;t.textureCompression=TextureImporterCompression.Uncompressed;t.npotScale=TextureImporterNPOTScale.None;t.maxTextureSize=1024;
 }
 // Optional explicit build-time import; the asset postprocessor also runs automatically.
 public static void Build(){foreach(var id in AssetMonsterArt2717.Ids)foreach(bool windup in new[]{false,true})AssetDatabase.ImportAsset("Assets/Resources/"+AssetMonsterArt2717.ResourcePath(id,windup)+".png",ImportAssetOptions.ForceUpdate);AssetDatabase.SaveAssets();}
}
