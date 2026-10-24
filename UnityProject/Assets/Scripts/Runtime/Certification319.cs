using System;using System.Collections.Generic;using UnityEngine;
namespace GreyHarbor {
 public static class Certification319 {
  public static bool Collection(ItemKind k)=>RaidInventory.Group(k)==ItemGroup.Collection;
  public static ItemOrigin Canonical(ItemKind k,ItemOrigin origin)=>Collection(k)?origin:ItemOrigin.Ordinary;
  public static bool Show(ItemStack item,bool inRaid)=>item!=null&&!inRaid&&Collection(item.Kind)&&item.Origin==ItemOrigin.Certified;
  public static void Normalize(ItemStack p){if(p==null)return;p.Origin=Canonical(p.Kind,p.Origin);if(p.Attachments30!=null)foreach(var a in p.Attachments30)Normalize(a);if(p.Keys314!=null)foreach(var k in p.Keys314)Normalize(k);}
  public static void Resolve(ItemStack p){if(p==null)return;Normalize(p);if(Collection(p.Kind)&&(p.Origin!=ItemOrigin.Ordinary||p.RaidFound))p.Origin=ItemOrigin.Certified;}
  public static void Resolve(RaidInventory grid){foreach(var p in grid.Slots)Resolve(p);}
  public static void NormalizeSave(BaseSave s){foreach(var list in new[]{s.Warehouse,s.CampRig,s.CampBag,s.SafeItems,s.Prepared})foreach(var p in list)Normalize(p);foreach(var d in s.CampDrops)foreach(var p in d.Items)Normalize(p);foreach(var p in new[]{s.Weapon,s.SecondWeapon,s.Helmet,s.Armor,s.RigGear,s.PackGear})Normalize(p);}
 }
 public sealed partial class RaidGame {
  internal ItemStack CarriedCollection319(){foreach(var grid in new[]{Chest,Bag,Safe})foreach(var p in grid.Slots)if(Certification319.Collection(p.Kind))return p;return null;}
  public bool UnloadCollections319()=>CampChange(()=>{var wh=CampRules.Grid(Save).Clone();var items=new List<ItemStack>();foreach(var grid in new[]{Chest,Bag,Safe})foreach(var p in grid.Slots)if(Certification319.Collection(p.Kind)){if(wh.Add(p)>0)return false;items.Add(p);}if(items.Count==0)return false;Save.Warehouse=wh.Snapshot();foreach(var grid in new[]{Chest,Bag,Safe})grid.Slots.RemoveAll(p=>Certification319.Collection(p.Kind));return true;},"收藏品已卸回仓库，战斗装备和弹药保持。");
 }
 public sealed partial class RaidUI {
  internal bool ShowCertification319(ItemStack item)=>Certification319.Show(item,G.Phase==GamePhase.Raid);
  string CertificationText319(ItemStack item)=>!Certification319.Collection(item.Kind)?"可携带进入战局":G.Phase==GamePhase.Raid?"收藏品 · 出局后认证":item.Origin==ItemOrigin.Certified?"✓ 已认证收藏品":"未认证收藏品";
 }
}
