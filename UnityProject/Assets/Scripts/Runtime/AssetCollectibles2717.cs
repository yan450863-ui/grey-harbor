using System;
using System.Collections.Generic;
using UnityEngine;
namespace GreyHarbor {
 [Serializable] public sealed class AssetCollectible2717 {public string Id,Name,Category,Description,Resource;public int Grade,Width,Height;public int Cells=>Width*Height;public Texture2D Texture=>Resources.Load<Texture2D>(Resource);}
 [Serializable] public sealed class AssetCollectibleList2717 {public AssetCollectible2717[] Items;}
 public static class AssetCollectibles2717 {
  static AssetCollectible2717[] all;
  public static AssetCollectible2717[] All {get {if(all==null){var list=new List<AssetCollectible2717>();var ids=new HashSet<string>();foreach(string path in new[]{"AssetShowcase2717/OfficeCollectibles2717","AssetShowcase2717/ExtraCollectibles2717","AssetShowcase2717/ExpansionACollectibles2717","AssetShowcase2717/ExpansionBCollectibles2717"}){var file=Resources.Load<TextAsset>(path);if(!file)continue;var data=JsonUtility.FromJson<AssetCollectibleList2717>(file.text);if(data?.Items!=null)foreach(var item in data.Items){if(string.IsNullOrEmpty(item.Id)||!ids.Add(item.Id)||item.Grade<0||item.Grade>4||item.Width<1||item.Height<1||item.Cells>4)throw new InvalidOperationException("Invalid showcase collectible record: "+item.Id);list.Add(item);}}all=list.ToArray();}return all;}}
  public static string GradeName(int grade)=>new[]{"绿色","蓝色","紫色","金色","红色"}[grade];
  public static Color GradeColor(int grade)=>new[]{PixelArt.Hex("7EB98A"),PixelArt.Hex("6A9ACD"),PixelArt.Hex("AA7CC8"),PixelArt.Hex("D4B25D"),PixelArt.Hex("C77465")}[grade];
 }
}
