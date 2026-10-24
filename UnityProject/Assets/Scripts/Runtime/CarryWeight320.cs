using System;using System.Collections.Generic;using UnityEngine;
namespace GreyHarbor {
 public struct WeightTier320 {public readonly string Name;public readonly float Move,Drain,Recovery,FutureRegen;public WeightTier320(string name,float move,float drain,float recovery,float regen){Name=name;Move=move;Drain=drain;Recovery=recovery;FutureRegen=regen;}}
 public static class CarryWeight320 {
  public const long LimitGrams=45000;public const int SafeBodyGrams=500;
  public static WeightTier320 Tier(long grams)=>grams<=18000?new WeightTier320("轻装",1,1,1,1):grams<=24000?new WeightTier320("负重",.95f,1,1,1):grams<=30000?new WeightTier320("重载",.9f,1.2f,1,1):grams<=36000?new WeightTier320("超重",.82f,1.4f,.75f,1):new WeightTier320("极限负重",.72f,1.7f,.5f,.6f);
  public static int UnitGrams(ItemKind k){
   if(AmmoCrafting328.IsComponent(k))return 20;
   if(k==ItemKind.AK)return 2500;if(k==ItemKind.MP5)return 2200;if(k==ItemKind.SKS)return 2800;if(k==ItemKind.AR16)return 2400;if(k==ItemKind.Dagger)return 2100;if(k==ItemKind.SG12)return 3000;
   if(ModRules30.IsMod(k))return 200;if(AmmoRules270.IsAmmo(k)){switch(WeaponRules279.AmmoCaliber(k)){case AmmoCaliber279.NineMM:return 10;case AmmoCaliber279.Five56:return 12;case AmmoCaliber279.Gauge12:return 40;default:return 16;}}
   if(k==ItemKind.Armor)return 2000;if(k==ItemKind.ReinforcedArmor320)return 3000;if(k==ItemKind.BlueArmor)return 3500;if(k==ItemKind.HeavyArmor320)return 5000;
   if(PackLayouts338.IsNew(k))return PackLayouts338.Grams(k);if(RaidInventory.IsPack(k))return 1000;if(RaidInventory.IsRig(k))return 800;if(k==ItemKind.GreenArmorKit)return 800;if(k==ItemKind.ArmorKit)return 1000;
   if(k==ItemKind.GreenMedkit)return 250;if(k==ItemKind.Medkit)return 350;if(k==ItemKind.PurpleMedkit)return 400;if(KeyRules2760.IsKey(k))return 20;if(KeyCase314.IsCase(k))return 100;
   if(Certification319.Collection(k))return 500*RaidInventory.Width(k)*RaidInventory.Height(k);
   return 500;
  }
  public static long ItemGrams(ItemStack item,HashSet<string> seen=null,ItemStack active=null,int activeLoaded=0,ItemKind activeAmmo=ItemKind.GreenAmmo){
   if(item==null||item.Count<=0)return 0;seen=seen??new HashSet<string>();if(!string.IsNullOrEmpty(item.Id)&&!seen.Add(item.Id))return 0;
   long grams=(long)UnitGrams(item.Kind)*item.Count;
   if(WeaponRules279.IsWeapon(item.Kind)){bool current=active!=null&&(ReferenceEquals(active,item)||active.Id==item.Id);int loaded=current?activeLoaded:item.MagazineReady314?item.Loaded314:0;var ammo=current?activeAmmo:item.LoadedAmmo314;grams+=(long)UnitGrams(ammo)*Math.Max(0,loaded);if(item.Attachments30!=null)foreach(var mod in item.Attachments30)grams+=ItemGrams(mod,seen);}
   if(KeyCase314.IsCase(item.Kind)&&item.Keys314!=null)foreach(var key in item.Keys314)grams+=ItemGrams(key,seen);
   return grams;
  }
  public static long Sum(IEnumerable<ItemStack> items){var seen=new HashSet<string>();long grams=0;foreach(var item in items)grams+=ItemGrams(item,seen);return grams;}
 }
 public sealed partial class RaidGame {
  public long CarryGrams320 {get{var seen=new HashSet<string>();long n=CarryWeight320.SafeBodyGrams;foreach(var p in new[]{Equipped,SecondWeapon,Armor,RigGear,PackGear})n+=CarryWeight320.ItemGrams(p,seen,Equipped,Magazine,MagazineAmmo);foreach(var inv in new[]{Chest,Bag,Safe})if(inv!=null)foreach(var p in inv.Slots)n+=CarryWeight320.ItemGrams(p,seen,Equipped,Magazine,MagazineAmmo);return n;}}
  public WeightTier320 LoadTier320=>CarryWeight320.Tier(CarryGrams320);
  public bool ProjectedWeightAllowed320(long before,long after){if(after<=CarryWeight320.LimitGrams||after<=before)return true;Notify("超过45kg携带上限，请减少数量或卸下物资。");return false;}
  public bool WeightMutationAllowed320(long before){long now=CarryGrams320;if(now<=CarryWeight320.LimitGrams||now<=before)return true;Notify("超过45kg携带上限，请减少数量或卸下物资。");return false;}
 }
 public sealed partial class RaidUI {
  void WeightHUD320(){if(G.Phase!=GamePhase.Raid)return;var t=G.LoadTier320;Fill(new Rect(420,780,355,62),panel);Label(436,789,325,24,$"负重 {G.CarryGrams320/1000f:0.0} / 45kg · {t.Name}",17,G.CarryGrams320>30000?gold:muted);Label(436,820,325,18,$"移动 {t.Move:0%} · 跑耗 {t.Drain:0%} · 体力恢复 {t.Recovery:0%}",12,muted);}
 }
}
