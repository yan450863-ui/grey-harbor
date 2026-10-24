using System;
using UnityEngine;
namespace GreyHarbor {
 public static class AmmoRules270 {
  public static int StackLimit314(ItemKind kind,bool warehouse)=>warehouse?300:WeaponRules279.AmmoCaliber(kind)==AmmoCaliber279.Gauge12?50:100;
  public static bool IsAmmo(ItemKind kind)=>WeaponRules279.AmmoCaliber(kind)!=AmmoCaliber279.None;
  public static int Count(RaidInventory inv){int count=0;foreach(var p in inv.Slots)if(IsAmmo(p.Kind))count+=p.Count;return count;}
  public static int LootCount30(ItemKind kind,System.Random rng)=>WeaponRules279.AmmoCaliber(kind)==AmmoCaliber279.Gauge12?rng.Next(4,9):rng.Next(8,19);
  public static int Count(RaidInventory inv,ItemKind weapon){int count=0;foreach(var item in inv.Slots)if(item.Identified&&WeaponRules279.Compatible(weapon,item.Kind))count+=item.Count;return count;}
  public static ItemStack Select(RaidInventory inv,ItemKind current,int magazine)=>Select(inv,current,magazine,ItemKind.AK);
  public static ItemStack Select(RaidInventory inv,ItemKind current,int magazine,ItemKind weapon){ItemStack best=null;foreach(var item in inv.Slots)if(item.Identified&&WeaponRules279.Compatible(weapon,item.Kind)&&(magazine==0||item.Kind==current)&&(best==null||item.Cell<best.Cell))best=item;return best;}
 }
 public sealed partial class RaidGame {
  public ItemKind MagazineAmmo=ItemKind.GreenAmmo;
  ItemKind reloadAmmo270=ItemKind.GreenAmmo;
  public bool PrepareAmmo270(ItemKind kind){if(ActiveWeapon279==null||!WeaponRules279.Compatible(Equipped.Kind,kind))return false;if(Magazine>0&&MagazineAmmo!=kind&&!UnloadMagazine())return false;MagazineAmmo=kind;return true;}
  public void CompleteReload270(){
   if(TrainingLoadoutAvailable301&&ActiveWeapon279!=null&&WeaponRules279.Compatible(Equipped.Kind,reloadAmmo270)){long before320=CarryGrams320;long body320=before320-(long)CarryWeight320.UnitGrams(MagazineAmmo)*Magazine;int room320=(int)Math.Max(0,(CarryWeight320.LimitGrams-body320)/CarryWeight320.UnitGrams(reloadAmmo270));int filled320=Math.Min(MagazineCapacity279,room320);if(filled320>Magazine){MagazineAmmo=reloadAmmo270;Magazine=filled320;MagazineFound=0;StoreActiveMagazine314();}if(filled320<MagazineCapacity279)Notify("负重已接近45kg，训练弹匣只装入能够携带的弹药。");ReloadTime=0;reloadId=null;return;}
   var inv=InventoryFor(reloadArea);var item=reloadId==null?null:inv?.Slots.Find(p=>p.Id==reloadId);
   if(ActiveWeapon279!=null&&WeaponRules279.Compatible(Equipped.Kind,reloadAmmo270)&&inv!=null&&(item==null&&reloadId==null||item!=null&&item.Kind==reloadAmmo270)&&
      (Magazine==0||MagazineAmmo==reloadAmmo270)){
    int available=reloadId==null?inv.Count(reloadAmmo270):item.Count;
    int count=Math.Max(0,Math.Min(MagazineCapacity279-Magazine,available));
    int ordinary=reloadId==null?inv.Count(reloadAmmo270,ItemOrigin.Ordinary):item.Origin==ItemOrigin.Ordinary?item.Count:0;
    if(reloadId==null)inv.Consume(reloadAmmo270,count);else RemoveAmount(reloadArea,item,count);
    MagazineAmmo=reloadAmmo270;Magazine+=count;MagazineFound+=Math.Max(0,count-ordinary);
   }
   StoreActiveMagazine314();ReloadTime=0;reloadId=null;
  }
  public bool LoadAmmoInCamp270(ItemArea area,ItemStack item){string id=item.Id;var kind=item.Kind;if(ActiveWeapon279==null||!WeaponRules279.Compatible(Equipped.Kind,kind)||(Magazine>0&&MagazineAmmo!=kind&&!TryUnloadMagazine270()))return false;MagazineAmmo=kind;item=InventoryFor(area)?.Slots.Find(p=>p.Id==id);if(item==null||item.Kind!=kind)return false;int count=Math.Max(0,Math.Min(MagazineCapacity279-Magazine,item.Count));Magazine+=count;if(item.Origin!=ItemOrigin.Ordinary)MagazineFound+=count;RemoveAmount(area,item,count);return count>0;}
 }
}
