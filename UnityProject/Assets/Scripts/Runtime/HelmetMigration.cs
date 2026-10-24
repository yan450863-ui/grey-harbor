using System;
using System.Collections.Generic;
namespace GreyHarbor { public static partial class CampRules {
 // Keep legacy item IDs for old saves/history; no current equipment grants or drops them.
 public static void RetireHelmets(BaseSave s){
  long refund=0;
  foreach(var list in new[]{s.Warehouse,s.Prepared,s.SafeItems})refund+=RemoveOldHelmets(list);
  long carried=RemoveOldHelmets(s.CampRig)+RemoveOldHelmets(s.CampBag);
  if(s.Helmet!=null&&RaidInventory.IsHelmet(s.Helmet.Kind)){carried+=(long)Math.Max(0,s.Helmet.Count)*RaidInventory.Value(s.Helmet.Kind);s.Helmet=null;}
  if(!s.InRaid)refund+=carried;
  foreach(var drop in s.CampDrops)refund+=RemoveOldHelmets(drop.Items);
  s.Coins=checked(s.Coins+(int)refund);
 }
 static long RemoveOldHelmets(List<ItemStack> items){long refund=0;for(int i=items.Count-1;i>=0;i--){var p=items[i];if(p!=null&&RaidInventory.IsHelmet(p.Kind)){refund+=(long)Math.Max(0,p.Count)*RaidInventory.Value(p.Kind);items.RemoveAt(i);}}return refund;}
}}
