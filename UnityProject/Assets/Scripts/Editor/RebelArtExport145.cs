using System.IO;
using UnityEngine;
using UnityEditor;
using GreyHarbor;
public static class RebelArtExport145 {
 public static void Build(){
  string dest="Assets/Resources/Rebels145";Directory.CreateDirectory(dest);
  for(int k=0;k<3;k++)for(int f=0;f<4;f++){var t=RebelArt145.CreateFrame((RebelKind)k,f);string path=dest+"/"+((RebelKind)k)+"_"+f+".png";File.WriteAllBytes(path,t.EncodeToPNG());Object.DestroyImmediate(t);AssetDatabase.ImportAsset(path,ImportAssetOptions.ForceUpdate);var imp=(TextureImporter)AssetImporter.GetAtPath(path);imp.textureType=TextureImporterType.Sprite;imp.spriteImportMode=SpriteImportMode.Single;var settings=new TextureImporterSettings();imp.ReadTextureSettings(settings);settings.spriteAlignment=9;settings.spritePivot=new Vector2(.5f,0);imp.SetTextureSettings(settings);imp.spritePixelsPerUnit=55;imp.filterMode=FilterMode.Point;imp.mipmapEnabled=false;imp.alphaIsTransparency=true;imp.textureCompression=TextureImporterCompression.Uncompressed;imp.npotScale=TextureImporterNPOTScale.None;imp.SaveAndReimport();}
  AssetDatabase.SaveAssets();string qa=Path.GetFullPath(Path.Combine(Application.dataPath,"../../QA"));Directory.CreateDirectory(qa);
  AssetDatabase.ExportPackage(new[]{dest,"Assets/Scripts/Runtime/RebelArt145.cs","Assets/Scripts/Runtime/RebelCombat145.cs"},Path.Combine(qa,"RebelSprites_1.46.unitypackage"),ExportPackageOptions.Recurse);
 }
}
