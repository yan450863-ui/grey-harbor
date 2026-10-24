using System;
using UnityEngine;
namespace GreyHarbor {
 public static class Economy270 {
  public const int Version=1,Scale=10;
  public static int Value(ItemKind kind,int legacy){if(kind==ItemKind.StandardPack)return 1400;if(BlueWeapons30.IsNewAmmo(kind))return BlueWeapons30.AmmoPrice(kind);if(FormalCollectibles2749.IsNew(kind))return FormalCollectibles2749.Value(kind);if(WeaponRules279.IsWeapon(kind))return WeaponRules279.Get(kind).SellPrice;if(AmmoRules270.IsAmmo(kind))return kind==ItemKind.GreenAmmo?15:kind==ItemKind.Ammo?20:kind==ItemKind.Green9mm?10:13;if(CollectibleData260.IsNew(kind)){var d=CollectibleData260.Get(kind);int cells=d.Width*d.Height;decimal multiplier=cells==1?1:cells==2?1.8m:cells==3?2.75m:3.7m;return (int)Math.Round(new[]{100,300,1000,3000}[d.Grade]*multiplier);}return checked(legacy*Scale);}
  public static void Migrate(BaseSave save){
   if(save.EconomyVersion>=Version)return;
   save.Coins=checked(save.Coins*Scale);save.TotalValue=checked(save.TotalValue*Scale);
   if(save.Trades!=null)foreach(var trade in save.Trades)trade.Coins=checked(trade.Coins*Scale);
   save.CampMagazineAmmo=ItemKind.Ammo;save.EconomyVersion=Version;
  }
 }
 public static class WeaponRules270 {
  public static float Damage(ItemKind weapon)=>WeaponRules279.Get(weapon)?.Damage??0;
  public static float Penetration(ItemKind weapon)=>WeaponRules279.Get(weapon)?.Penetration??0;
 }
 public static class NpcRules270 {
  public static int Level(RebelKind kind)=>kind==RebelKind.BlueRegular||kind==RebelKind.BlueHeavy||kind==RebelKind.BlueSmg||kind==RebelKind.BlueMarksman?2:1;
  public static bool Heavy(RebelKind kind)=>kind==RebelKind.Heavy||kind==RebelKind.BlueHeavy;
  public static float OldHealthBudget(RebelKind kind)=>(Heavy(kind)?252:196)*(Level(kind)==2?1.25f:1);
  public static float Health(RebelKind kind)=>NpcCombat275.HealthForBudget(OldHealthBudget(kind));
  public static float Armor(RebelKind kind)=>NpcCombat275.ArmorForBudget(OldHealthBudget(kind));
  public static void SetMonsterLevel(RaidActor actor,int level){
   actor.NpcLevel=Mathf.Clamp(level,1,2);actor.NpcAmmo=actor.NpcLevel==2?ItemKind.Ammo:ItemKind.GreenAmmo;
   float scale=actor.NpcLevel==2?1.25f:1;
   actor.NpcArmorPoints=actor.NpcMaxArmor=MonsterTuning.Armor(actor.Monster)*scale;
   actor.Health=actor.NpcMaxHealth=MonsterTuning.Health(actor.Monster)*scale;
  }
  public static float Damage(float raw,int ammoGrade,int level)=>raw*ArmorBalance.Multiplier(ammoGrade,Math.Max(0,level-1));
  // Preserve the base roll, then grant each rolled item an independent 25% additional unit.
  public static void BoostLoot(System.Collections.Generic.List<ItemStack> items,int level,System.Random rng){if(level<2)return;foreach(var item in items){int count=item.Count;for(int n=0;n<count;n++)if(rng.NextDouble()<.25)item.Count++;}}
 }
}

