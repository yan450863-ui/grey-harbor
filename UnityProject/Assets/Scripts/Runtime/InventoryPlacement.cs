using System;
using UnityEngine;
namespace GreyHarbor {
 public sealed partial class RaidGame {
  // A single preflight is shared by drag feedback and the committed operation.
  public bool CanMoveItem(ItemArea from,int sourceCell,ItemArea to,int targetCell,bool? rotated=null){
   if(CorpseSource325(to)||CorpseLoot325.IsArea(from)&&!CorpseContentEmpty325(from,false))return false;
   if(!Access(from)||!Access(to))return false;StoreActiveMagazine314();var s=ItemAt(from,sourceCell);if(s==null||!s.Identified||!KeyGridAllowed314(to,s))return false;
   if(CanLoadGun314(from,sourceCell,to,targetCell))return true;
   var caseTarget=InventoryFor(to)?.At(targetCell);if(caseTarget!=null&&KeyCase314.IsCase(caseTarget.Kind)&&KeyRules2760.IsKey(s.Kind))return CanInsertCase314(from,sourceCell,to,targetCell);
   var shape=RaidInventory.Copy(s);if(rotated.HasValue)shape.Rotated=rotated.Value;
   if(to==ItemArea.Safe&&RaidInventory.IsWearable(s.Kind))return false;
   if(IsGearArea(to))return CanEquipMove(from,s,to);
   var target=InventoryFor(to);if(target==null)return false;
   if(targetCell<0){if(IsGearArea(from)&&!CanRemoveGearQuiet(from))return false;return target.Clone().Add(shape,s.Count)<s.Count;}
   if(targetCell>=target.SlotCapacity)return false;
   if(from==to&&s.Cell==targetCell&&shape.Rotated==s.Rotated)return true;
   var d=target.At(targetCell);if(d==s)d=null;
   if(d!=null&&!d.Identified)return false;
   if(d!=null&&!RaidInventory.IsWearable(s.Kind)&&RaidInventory.Compatible(d,s)&&target.StackLimit(s.Kind)>1)return d.Count<target.StackLimit(s.Kind);
   if(HasSwapTargets314(from,s,to,targetCell,shape))return !CorpseSource325(from)&&PlanSwap314(from,s,to,targetCell,shape,out _);
   // A footprint may overlap an item even if its top-left cell is empty.
   foreach(var p in target.Slots){if(p==s||!Overlaps(target,shape,targetCell,p,p.Cell))continue;if(d!=null&&d!=p)return false;d=p;}
   if(d!=null){
    if(CorpseSource325(from))return false;
    if(!d.Identified||s.Count>target.StackLimit(s.Kind))return false;
    if(IsGearArea(from))return target.Fits(shape,targetCell,null,d)&&GearFits(from,d.Kind)&&CanEquipMove(to,d,from,shape);
    var origin=InventoryFor(from);if(origin==null||!KeyGridAllowed314(from,d)||d.Count>origin.StackLimit(d.Kind)||from==ItemArea.Safe&&RaidInventory.IsWearable(d.Kind))return false;
    if(!target.Fits(shape,targetCell,from==to?s:null,d)||!origin.Fits(d,s.Cell,s,from==to?d:null))return false;
    return from!=to||!Overlaps(target,shape,targetCell,d,s.Cell);
   }
   return (!IsGearArea(from)||CanRemoveGearQuiet(from))&&target.Fits(shape,targetCell,from==to?s:null);
  }
  static bool Overlaps(RaidInventory inv,ItemStack a,int ac,ItemStack b,int bc){if(inv.Columns<=0||ac<0||bc<0)return false;return new Rect(ac%inv.Columns,ac/inv.Columns,inv.W(a),inv.H(a)).Overlaps(new Rect(bc%inv.Columns,bc/inv.Columns,inv.W(b),inv.H(b)));}
  bool CanRemoveGearQuiet(ItemArea a)=>!(a==ItemArea.RigEquipment&&Chest.Slots.Count>0||a==ItemArea.PackEquipment&&Bag.Slots.Count>0);
  bool CanEquipMove(ItemArea from,ItemStack s,ItemArea to,ItemStack oldOverride=null){
   if(CorpseLoot325.IsArea(from)&&!CorpseContentEmpty325(from,false))return false;
   if(!GearFits(to,s.Kind)||from==ItemArea.Safe)return false;
   if(IsGearArea(from))return from!=to&&(from==ItemArea.Weapon||from==ItemArea.SecondWeapon)&&(to==ItemArea.Weapon||to==ItemArea.SecondWeapon);
   var origin=InventoryFor(from);if(origin==null)return false;var copy=origin.Clone();copy.Slots.RemoveAll(p=>p.Id==s.Id);var old=oldOverride??GearAt(to);if(CorpseSource325(from)&&old!=null)return false;
   bool rig=to==ItemArea.RigEquipment,pack=to==ItemArea.PackEquipment;
   if(rig||pack){var resized=RaidInventory.Container(s,rig);var carried=(rig?Chest:Bag).Snapshot();carried.RemoveAll(p=>p.Id==s.Id);if(!resized.Repack(carried))return false;if(from==(rig?ItemArea.Rig:ItemArea.Backpack))copy=resized;}
   if(old!=null){var removed=RaidInventory.Copy(old);removed.Cell=copy.Fits(old,s.Cell)?s.Cell:copy.FreeCell(old);if(removed.Cell<0)return false;copy.Slots.Add(removed);}
   return true;
  }
  bool SwapGridItems(ItemArea from,ItemStack s,ItemArea to,int targetCell,ItemStack d,bool? rotated=null){
   if(!CanMoveItem(from,s.Cell,to,targetCell,rotated))return false;
   if(IsGearArea(from)){if(rotated.HasValue)s.Rotated=rotated.Value;if(!EquipInto(to,d,from))return false;var moved=InventoryFor(to)?.Slots.Find(p=>p.Id==s.Id);if(moved==null)return false;moved.Cell=targetCell;return true;}
   var origin=InventoryFor(from);var target=InventoryFor(to);int original=s.Cell;
   origin.Slots.Remove(s);target.Slots.Remove(d);s.Cell=targetCell;if(rotated.HasValue)s.Rotated=rotated.Value;d.Cell=original;target.Slots.Add(s);origin.Slots.Add(d);return true;
  }
 }
}
