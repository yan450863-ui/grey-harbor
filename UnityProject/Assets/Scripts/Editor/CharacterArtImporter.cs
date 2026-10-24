using UnityEditor;
using UnityEngine;
public sealed class CharacterArtImporter:AssetPostprocessor {
 void OnPreprocessTexture(){
  if(!assetPath.StartsWith("Assets/Resources/CharacterArt/"))return;
  var t=(TextureImporter)assetImporter;t.textureType=TextureImporterType.Default;t.isReadable=true;
  t.alphaSource=TextureImporterAlphaSource.FromInput;t.alphaIsTransparency=true;t.mipmapEnabled=false;
  t.textureCompression=TextureImporterCompression.Uncompressed;t.maxTextureSize=2048;t.filterMode=FilterMode.Point;t.wrapMode=TextureWrapMode.Clamp;
 }
}
