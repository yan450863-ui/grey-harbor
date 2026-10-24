using System;using System.Collections.Generic;using UnityEngine;
namespace GreyHarbor {
 public sealed class CorpseLoot325 {
  public bool Soldier,RigRecoverable,PackRecoverable,WeaponRecoverable,ArmorRecoverable;public string EnemyName;public int Level=1;public Sprite Portrait;
  public ItemKind RigKind=ItemKind.SmallRig,PackKind=ItemKind.SmallPack;
  public int RigColumns=4,RigRows=2,BagColumns=4,BagRows=2,BioColumns=3,BioRows=3;
  public readonly List<ItemStack> WeaponItems=new List<ItemStack>(),ArmorItems=new List<ItemStack>(),RigGearItems=new List<ItemStack>(),PackGearItems=new List<ItemStack>(),RigItems=new List<ItemStack>(),BagItems=new List<ItemStack>();

  public static RaidInventory FixedGrid(int columns,int rows,List<ItemStack> items,RectInt[] compartments=null){
   var occupied=new RaidInventory(columns,rows){Compartments338=compartments};var pending=new List<ItemStack>();var positions=new List<int>();
   // Reserve valid slots first. A missing/invalid position must never compact the other items.
   foreach(var item in items){
    if(item==null||item.Count<=0||item.Count>occupied.StackLimit(item.Kind))throw new InvalidOperationException("尸体物资数量无效");
    if(occupied.Fits(item,item.Cell))occupied.Slots.Add(item);else pending.Add(item);
   }
   foreach(var item in pending){
    var shape=RaidInventory.Copy(item);int cell=occupied.FreeCell(shape);
    if(cell<0)throw new InvalidOperationException("尸体物资超过固定容量");
    shape.Cell=cell;occupied.Slots.Add(shape);positions.Add(cell);
   }
   for(int i=0;i<pending.Count;i++)pending[i].Cell=positions[i];
   return new RaidInventory(columns,rows,items){Compartments338=compartments};
  }

  public static bool IsArea(ItemArea a)=>a==ItemArea.CorpseWeapon325||a==ItemArea.CorpseArmor325||a==ItemArea.CorpseRigGear325||a==ItemArea.CorpsePackGear325||a==ItemArea.CorpseRig325||a==ItemArea.CorpseBag325;
  public IEnumerable<ItemStack> AllItems(){foreach(var list in new[]{WeaponItems,ArmorItems,RigGearItems,PackGearItems,RigItems,BagItems})foreach(var p in list)yield return p;}
  public CorpseLoot325 Copy(){var result=new CorpseLoot325{Soldier=Soldier,RigRecoverable=RigRecoverable,PackRecoverable=PackRecoverable,WeaponRecoverable=WeaponRecoverable,ArmorRecoverable=ArmorRecoverable,EnemyName=EnemyName,Level=Level,Portrait=Portrait,RigKind=RigKind,PackKind=PackKind,RigColumns=RigColumns,RigRows=RigRows,BagColumns=BagColumns,BagRows=BagRows,BioColumns=BioColumns,BioRows=BioRows};result.Restore(this);return result;}
  public void Restore(CorpseLoot325 source){var from=new[]{source.WeaponItems,source.ArmorItems,source.RigGearItems,source.PackGearItems,source.RigItems,source.BagItems};var to=new[]{WeaponItems,ArmorItems,RigGearItems,PackGearItems,RigItems,BagItems};for(int i=0;i<to.Length;i++){to[i].Clear();foreach(var p in from[i])to[i].Add(RaidInventory.Copy(p));}}
 }
 public sealed partial class RaidGame {
  public RaidInventory CorpseInventory325(ItemArea area){
   var body=OpenLoot?.Corpse325;if(body==null||!body.Soldier)return null;
   switch(area){
    case ItemArea.CorpseWeapon325:return CorpseLoot325.FixedGrid(4,2,body.WeaponItems);
    case ItemArea.CorpseArmor325:return CorpseLoot325.FixedGrid(3,3,body.ArmorItems);
    case ItemArea.CorpseRigGear325:return CorpseLoot325.FixedGrid(2,2,body.RigGearItems);
    case ItemArea.CorpsePackGear325:return CorpseLoot325.FixedGrid(2,2,body.PackGearItems);
    case ItemArea.CorpseRig325:return CorpseLoot325.FixedGrid(body.RigColumns,body.RigRows,body.RigItems);
    case ItemArea.CorpseBag325:return CorpseLoot325.FixedGrid(body.BagColumns,body.BagRows,body.BagItems,PackLayouts338.IsNew(body.PackKind)?PackLayouts338.Create(body.PackKind).Compartments338:null);
    default:return null;
   }
  }
  bool CorpseContentEmpty325(ItemArea area,bool notify){
   var body=OpenLoot?.Corpse325;if(body==null)return false;
   bool empty=area==ItemArea.CorpseRigGear325?body.RigItems.Count==0:area==ItemArea.CorpsePackGear325?body.BagItems.Count==0:true;
   if(!empty&&notify)Notify("请先取走尸体容器内的物资，再回收胸挂或背包。");return empty;
  }
  bool CorpseSource325(ItemArea area){
   if(CorpseLoot325.IsArea(area)||area==ItemArea.Loot&&OpenLoot?.Corpse325!=null)return true;
   if(area!=ItemArea.KeyCase314||OpenKeyCase314==null)return false;
   return CorpseLoot325.IsArea(OpenKeyCaseArea314)||OpenKeyCaseArea314==ItemArea.Loot&&OpenLoot?.Corpse325!=null;
  }
 }
}
