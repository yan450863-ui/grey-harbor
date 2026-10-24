using UnityEngine;
namespace GreyHarbor {
 public static class PackLayouts338 {
  public static readonly ItemKind[] All={ItemKind.HikingPack338,ItemKind.ToolPack338,ItemKind.TacticalPack338,ItemKind.DutyPack338};
  public static bool IsNew(ItemKind k)=>k>=ItemKind.HikingPack338&&k<=ItemKind.DutyPack338;
  static int Index(ItemKind k)=>(int)k-(int)ItemKind.HikingPack338;
  public static string Name(ItemKind k)=>new[]{"轻便徒步包","民用工具背包","战术通用背包","多袋勤务背包"}[Index(k)];
  public static int Grade(ItemKind k)=>Index(k)<2?0:1;
  public static int Value(ItemKind k)=>new[]{1200,1600,1900,1700}[Index(k)];
  public static int Grams(ItemKind k)=>new[]{900,1500,1800,1600}[Index(k)];
  public static string Description(ItemKind k)=>new[]{"20格：2×4＋3×4。","25格：2×5＋3×5。","30格：4×5主仓＋2×5侧仓。","30格：3×4主仓＋两个2×3侧袋＋3×2底袋。"}[Index(k)]+"分仓可收纳各种物资，单件不能跨仓；Q旋转，双击自动寻找空位。更换背包保留内容，放不下则取消。";
  public static RaidInventory Create(ItemKind k){
   switch(k){
    case ItemKind.HikingPack338:return new RaidInventory(5,4){Compartments338=new[]{new RectInt(0,0,2,4),new RectInt(2,0,3,4)}};
    case ItemKind.ToolPack338:return new RaidInventory(5,5){Compartments338=new[]{new RectInt(0,0,2,5),new RectInt(2,0,3,5)}};
    case ItemKind.TacticalPack338:return new RaidInventory(6,5){Compartments338=new[]{new RectInt(0,0,4,5),new RectInt(4,0,2,5)}};
    default:return new RaidInventory(5,6){Compartments338=new[]{new RectInt(0,0,3,4),new RectInt(3,0,2,3),new RectInt(3,3,2,3),new RectInt(0,4,3,2)}};
   }
  }
 }
 public sealed partial class RaidInventory {
  public RectInt[] Compartments338;
  public bool Partitioned338=>Compartments338!=null&&Compartments338.Length>0;
  public bool WithinCompartment338(int x,int y,int width,int height){
   if(x<0||y<0||x+width>Columns||y+height>Rows)return false;
   if(!Partitioned338)return true;
   foreach(var r in Compartments338)if(x>=r.x&&y>=r.y&&x+width<=r.xMax&&y+height<=r.yMax)return true;
   return false;
  }
 }
}
