using System;
using System.Collections.Generic;
namespace GreyHarbor {
 public static class MarketCategories {
  public static readonly string[] Names={"武器","消耗品","装备","配件","弹药","钥匙","收集品"};
  public static readonly string[][] Children={new[]{"突击步枪","冲锋枪","半自动步枪","霰弹枪"},new[]{"维修用品"},new[]{"防具","胸挂","背包"},new[]{"瞄具","枪口","弹匣","前握把","后握把","枪托","激光器"},new[]{"7.62×39","9×19","5.56","12号"},new[]{"普通钥匙","门禁卡","钥匙包"},new[]{"生活物资","工具与零件","电子物资","医疗材料","能源与化工","生物材料","资料与情报","工艺与收藏品"}};
  public static int Main(ItemKind k){if(KeyCase314.IsCase(k))return 5;if(ModRules30.IsMod(k))return 3;if(KeyRules2760.IsKey(k))return 5;if(WeaponRules279.IsWeapon(k))return 0;if(ConsumableRules.IsSupply(k))return 1;if(RaidInventory.IsWearable(k))return 2;if(AmmoRules270.IsAmmo(k))return 4;return 6;}
  public static int Sub(ItemKind k){if(KeyCase314.IsCase(k))return 2;int m=Main(k);if(m==0)return k==ItemKind.MP5||k==ItemKind.Dagger?1:k==ItemKind.SKS?2:k==ItemKind.SG12?3:0;if(m==3)return ModRules30.MarketSub(k);if(m==4)return (int)WeaponRules279.AmmoCaliber(k)-1;if(ConsumableRules.IsRepair(k))return 0;if(m==2)return RaidInventory.IsArmor(k)?0:RaidInventory.IsRig(k)?1:2;if(m==6)return (int)RaidInventory.Category(k)-1;return 0;}
  public static bool Matches(ItemKind k,int main,int sub)=>main<0||(Main(k)==main&&(sub<0||Sub(k)==sub));
  public static List<ItemKind> Goods(int main,int sub){var list=new List<ItemKind>();foreach(var k in MarketRules.Catalog(0))if(Matches(k,main,sub))list.Add(k);return list;}
 }
 [Serializable] public sealed class TradeLine { public ItemKind Kind;public int Count; }
 [Serializable] public sealed class TradeRecord {
  public string TimeUtc;public bool Buying;public int Coins;public List<TradeLine> Items=new List<TradeLine>();
  public static void Append(BaseSave save,bool buying,int coins,IEnumerable<ItemStack> items){
   var entry=new TradeRecord{TimeUtc=DateTime.UtcNow.ToString("o"),Buying=buying,Coins=coins};foreach(var item in items){var line=entry.Items.Find(p=>p.Kind==item.Kind);if(line==null)entry.Items.Add(new TradeLine{Kind=item.Kind,Count=item.Count});else line.Count=checked(line.Count+item.Count);}
   save.Trades=save.Trades??new List<TradeRecord>();save.Trades.Insert(0,entry);if(save.Trades.Count>200)save.Trades.RemoveRange(200,save.Trades.Count-200);
  }
 }
}
