using System;
using System.Collections.Generic;
namespace GreyHarbor {
 // Retain persisted IDs and artwork while removing the former healing items from live saves.
 public static class RetiredMedicine321 {
  public static bool IsRetired(ItemKind kind)=>kind==ItemKind.GreenMedkit||kind==ItemKind.Medkit||kind==ItemKind.PurpleMedkit;
  static void Remove(List<ItemStack> items){
   if(items==null)return;
   for(int i=items.Count-1;i>=0;i--){var item=items[i];if(item==null)continue;if(IsRetired(item.Kind)){items.RemoveAt(i);continue;}Remove(item.Attachments30);Remove(item.Keys314);}
  }
  static ItemStack Clean(ItemStack item){if(item==null||IsRetired(item.Kind))return null;Remove(item.Attachments30);Remove(item.Keys314);return item;}
  public static void Normalize(BaseSave save){
   if(save==null)return;
   foreach(var list in new[]{save.Warehouse,save.CampRig,save.CampBag,save.SafeItems,save.Prepared})Remove(list);
   if(save.CampDrops!=null){foreach(var drop in save.CampDrops)if(drop!=null)Remove(drop.Items);save.CampDrops.RemoveAll(drop=>drop==null||drop.Items==null||drop.Items.Count==0);}
   save.Weapon=Clean(save.Weapon);save.SecondWeapon=Clean(save.SecondWeapon);save.Helmet=Clean(save.Helmet);save.Armor=Clean(save.Armor);save.RigGear=Clean(save.RigGear);save.PackGear=Clean(save.PackGear);
   if(save.Npc319!=null){Remove(save.Npc319.PendingItems);if(save.Npc319.Quests!=null)foreach(var q in save.Npc319.Quests){if(q==null||!IsRetired(q.Reward))continue;bool advanced=q.Reward==ItemKind.PurpleMedkit;q.Reward=advanced?ItemKind.ArmorKit:ItemKind.GreenArmorKit;q.RewardCount=!q.Claimed&&q.RewardCount>0?1:0;}}
   if(save.Stored!=null)foreach(var kind in new[]{ItemKind.GreenMedkit,ItemKind.Medkit,ItemKind.PurpleMedkit})if((int)kind<save.Stored.Length)save.Stored[(int)kind]=0;
   if(save.Certified!=null)foreach(var kind in new[]{ItemKind.GreenMedkit,ItemKind.Medkit,ItemKind.PurpleMedkit})if((int)kind<save.Certified.Length)save.Certified[(int)kind]=0;
  }
 }
}
