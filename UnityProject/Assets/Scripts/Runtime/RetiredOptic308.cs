using System.Collections.Generic;
namespace GreyHarbor {public static class RetiredOptic308 {
 public static void Convert(ItemStack item){if(item==null)return;if(item.Kind==ItemKind.GreenOptic30)item.Kind=ItemKind.BlueOptic30;if(item.Attachments30!=null)foreach(var p in item.Attachments30)Convert(p);}
 static void Convert(List<ItemStack> list){if(list!=null)foreach(var p in list)Convert(p);}
 public static void Normalize(BaseSave save){Convert(save.Warehouse);Convert(save.CampRig);Convert(save.CampBag);Convert(save.SafeItems);Convert(save.Prepared);if(save.CampDrops!=null)foreach(var d in save.CampDrops)Convert(d.Items);foreach(var p in new[]{save.Weapon,save.SecondWeapon,save.Helmet,save.Armor,save.RigGear,save.PackGear})Convert(p);}
}
 public static class SpreadReticle308 {public static float Gap(float radiusPixels,float scale)=>UnityEngine.Mathf.Clamp(radiusPixels/UnityEngine.Mathf.Max(scale,.0001f),4,65);}
}
