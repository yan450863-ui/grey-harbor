using System;
using System.Collections.Generic;
using UnityEngine;
namespace GreyHarbor {
 public static class MarketRules {
  public const int MaxQuantity=999;
  public static bool Listed(ItemKind kind)=>!AmmoCrafting328.IsComponent(kind)&& (int)kind>=0&&(int)kind<RaidInventory.KindCount&&!RetiredMedicine321.IsRetired(kind)&&kind!=ItemKind.GreenOptic30&&kind!=ItemKind.LargeRig&&kind!=ItemKind.LargePack&&!RaidInventory.IsHelmet(kind);
  public static List<ItemKind> Catalog(int filter){var list=new List<ItemKind>();for(int i=0;i<RaidInventory.KindCount;i++){var k=(ItemKind)i;if(Listed(k)&&(filter==0||(filter==1)==(RaidInventory.Group(k)==ItemGroup.Combat)))list.Add(k);}return list;}
  // Round unit price upward first, then multiply quantity.
  public static int UnitPrice(ItemKind kind)=>Listed(kind)?KeyCase314.IsCase(kind)?5000:KeyRules2760.IsKey(kind)?KeyRules2760.Get(kind).BuyPrice:(int)(((long)RaidInventory.Value(kind)*115+99)/100):-1;
  public static int Price(ItemKind kind,int quantity){if(!Listed(kind)||quantity<1||quantity>MaxQuantity)return -1;long price=(long)UnitPrice(kind)*quantity;return price>int.MaxValue?-1:(int)price;}
  public static bool Quote(BaseSave save,ItemKind kind,int quantity,out int price,out string reason){
   price=Price(kind,quantity);reason="";if(price<0){reason="请选择已上架物品，数量为1～999。";return false;}
   if(save.Coins<price){reason="金币不足。";return false;}
   var grid=CampRules.Grid(save).Clone();if(grid.Add(kind,quantity)!=0){reason="仓库空间不足，请整理或减少购买数量。";return false;}return true;
  }
 }
 public sealed partial class RaidGame {
  public bool BuyMarket(ItemKind kind,int quantity){return CampChange(()=>{
   if(Panel!=CampPanel.Market||!InventoryOpen||(!MenuCamp315&&Vector3.Distance(Player.transform.position,RaidMap.Market)>=2)){Notify("请靠近营地交易行，按 F 打开。");return false;}
   if(!MarketRules.Quote(Save,kind,quantity,out int price,out string reason)){Notify(reason);return false;}
   CampRules.Deposit(Save,kind,quantity);Save.Coins=checked(Save.Coins-price);TradeRecord.Append(Save,true,price,new[]{new ItemStack(kind,quantity)});return true;
  },"购买成功，物资已存入主仓库。");}
 }
}
