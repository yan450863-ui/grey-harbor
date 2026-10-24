using UnityEditor;
using UnityEngine;
public sealed class AssetCollectibleImport2717:AssetPostprocessor {
 void OnPreprocessTexture(){if(!assetPath.StartsWith("Assets/Resources/AssetShowcase2717/Collectibles/"))return;var t=(TextureImporter)assetImporter;t.textureType=TextureImporterType.Default;t.isReadable=true;t.alphaSource=TextureImporterAlphaSource.FromInput;t.alphaIsTransparency=true;t.mipmapEnabled=true;t.textureCompression=TextureImporterCompression.Uncompressed;t.maxTextureSize=512;t.filterMode=FilterMode.Point;t.wrapMode=TextureWrapMode.Clamp;t.sRGBTexture=true;}
 public static void Build(){foreach(string guid in AssetDatabase.FindAssets("t:Texture2D",new[]{"Assets/Resources/AssetShowcase2717/Collectibles"})){string path=AssetDatabase.GUIDToAssetPath(guid);var t=AssetImporter.GetAtPath(path) as TextureImporter;if(!t)continue;t.textureType=TextureImporterType.Default;t.isReadable=true;t.alphaSource=TextureImporterAlphaSource.FromInput;t.alphaIsTransparency=true;t.mipmapEnabled=true;t.textureCompression=TextureImporterCompression.Uncompressed;t.maxTextureSize=512;t.filterMode=FilterMode.Point;t.wrapMode=TextureWrapMode.Clamp;t.sRGBTexture=true;t.SaveAndReimport();}}
}
