using UnityEngine;
namespace GreyHarbor {
 public sealed partial class RaidUI {
  public static string WeaponHudTitle279(ItemStack weapon){var p=weapon==null?null:WeaponRules279.Get(weapon.Kind);return p==null?"未装备枪械":RaidInventory.GradeName(weapon.Kind)+" "+p.Name+"  /  "+p.CaliberLabel;}
  public static string WeaponHudMode279(ItemKind kind){var p=WeaponRules279.Get(kind);return p==null?"拖入主武器槽":p.Automatic?"自动":"半自动 · 单击一发";}
  public static int WeaponHudReserve279(ItemStack weapon,RaidInventory chest)=>weapon==null||chest==null?0:AmmoRules270.Count(chest,weapon.Kind);
  public static string WeaponHudDamage279(ItemStack weapon){var p=weapon==null?null:WeaponRules279.Get(weapon.Kind);return p==null?"未装备枪械":$"{p.Name} 伤害{p.Damage:0.#} / 穿甲{p.Penetration:0.#}";}
  void WeaponHUD279(){
   var weapon=G.Equipped;var p=G.ActiveWeapon279;
   Fill(new Rect(1250,743,322,129),panel);
   Label(1272,757,280,24,WeaponHudTitle279(weapon),16,p==null?muted:RaidInventory.Tint(weapon.Kind));
   Label(1272,790,88,49,p==null?"--":G.Magazine.ToString("00"),43,white);
   Label(1362,807,62,26,p==null?"":"/ "+G.MagazineCapacity279,18,muted);
   Label(1425,801,127,35,"备 "+(G.TrainingLoadoutAvailable301?"∞":WeaponHudReserve279(weapon,G.Chest).ToString("00")),22,muted,TextAnchor.MiddleRight);
   string detail=p==null?WeaponHudMode279(ItemKind.Cola):RaidInventory.GradeName(G.MagazineAmmo)+"弹 · "+WeaponHudMode279(weapon.Kind)+" · R换弹";
   Label(1272,846,280,22,detail,12,muted);
  }
 }
}
