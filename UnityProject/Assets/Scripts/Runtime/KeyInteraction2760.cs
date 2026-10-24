using System;using UnityEngine;
namespace GreyHarbor {
 public sealed partial class RaidGame {
  System.Collections.Generic.IEnumerable<ItemStack> CarriedKeys314(RaidInventory inv){foreach(var p in inv.Slots){if(KeyCase314.IsCase(p.Kind)){foreach(var key in KeyCase314.Grid(p).Slots)yield return key;}else yield return p;}}
  RaidInventory NestedKeySource314(RaidInventory inv,ItemStack key){foreach(var bag in inv.Slots)if(KeyCase314.IsCase(bag.Kind)&&bag.Keys314.Contains(key))return KeyCase314.Grid(bag);return null;}
  internal ItemStack FindCarriedKey2760(ItemKind kind,out RaidInventory source){
   source=null;ItemStack best=null;
   foreach(var inv in new[]{Safe,Chest,Bag})if(inv!=null)foreach(var item in CarriedKeys314(inv))if(item.Kind==kind&&item.Identified&&KeyRules2760.Remaining(item)>0){if(best==null||KeyRules2760.Remaining(item)<KeyRules2760.Remaining(best)){best=item;source=inv.Slots.Contains(item)?inv:NestedKeySource314(inv,item);}}
   return best;
  }
  bool SpendDoorKey2760(WorldDoor door){
   if(Phase!=GamePhase.Raid||Paused)return false;
   var item=FindCarriedKey2760(door.KeyKind2760,out var inv);if(item==null)return false;
   var backup=inv.Snapshot();string before=JsonUtility.ToJson(Save);
   KeyRules2760.Ensure(item);item.KeyUses2760--;
   int left=item.KeyUses2760;if(left==0)inv.Slots.Remove(item);
   Save.SafeItems=Safe.Snapshot();
   if(!TrainingActive&&!WriteSave()){Save=JsonUtility.FromJson<BaseSave>(before);inv.Restore(backup);return false;}
   door.Locked=false;Notify(RaidInventory.Name(door.KeyKind2760)+"：剩余 "+left+" / 4 次"+(left==0?"，钥匙已耗尽。":"。"));return true;
  }
 }
 public sealed partial class RaidUI {
  internal static string ItemCornerText2760(ItemStack item)=>KeyRules2760.IsKey(item.Kind)?KeyRules2760.Remaining(item).ToString():item.Count.ToString();
 }
}
