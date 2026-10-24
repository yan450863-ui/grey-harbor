using UnityEditor;
using UnityEngine;
public sealed class ItemArtImporter:AssetPostprocessor {
 void OnPreprocessTexture(){
  if(!assetPath.StartsWith("Assets/Resources/ItemArt/"))return;
  var t=(TextureImporter)assetImporter;t.textureType=TextureImporterType.Default;t.isReadable=true;
  t.alphaSource=TextureImporterAlphaSource.FromInput;t.alphaIsTransparency=true;t.mipmapEnabled=true;
  t.textureCompression=TextureImporterCompression.Uncompressed;t.maxTextureSize=512;t.filterMode=FilterMode.Trilinear;t.wrapMode=TextureWrapMode.Clamp;
 }
}
