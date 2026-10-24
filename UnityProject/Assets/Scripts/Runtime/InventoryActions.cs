using System;
using System.Collections.Generic;
using UnityEngine;
namespace GreyHarbor {
 public sealed partial class RaidGame {
  public List<ItemStack> WarehouseItems()=>Save.Warehouse;
  public RaidInventory InventoryFor(ItemArea a)=>CorpseLoot325.IsArea(a)?CorpseInventory325(a):a==ItemArea.KeyCase314?KeyCase314.Grid(OpenKeyCase314):a==ItemArea.Rig?Chest:a==ItemArea.Backpack?Bag:a==ItemArea.Safe?Safe:a==ItemArea.Warehouse?CampRules.Grid(Save):a==ItemArea.Loot?EnsureLootGrid(OpenLoot):null;
  public ItemStack ItemAt(ItemArea a,int cell){var p=IsGearArea(a)?GearAt(a):InventoryFor(a)?.At(cell);return (a==ItemArea.Loot||CorpseLoot325.IsArea(a))&&p!=null&&!p.Identified?null:p;}
  bool CanLoot()=>OpenLoot!=null&&OpenLoot.Opened&&(Phase==GamePhase.Base?Vector3.Distance(Player.transform.position,OpenLoot.Position)<=2:CanInteractLoot(OpenLoot,2));
  bool Access(ItemArea a)=>CorpseLoot325.IsArea(a)?CanLoot()&&OpenLoot.Corpse325!=null&&OpenLoot.Corpse325.Soldier:a==ItemArea.KeyCase314?OpenKeyCase314!=null:a==ItemArea.Warehouse?Phase==GamePhase.Base&&Panel==CampPanel.Warehouse:a==ItemArea.Loot?CanLoot()&&OpenLoot.Corpse325?.Soldier!=true:true;
  void RemoveAmount(ItemArea a,ItemStack source,int amount){var inventory=IsGearArea(a)?null:InventoryFor(a);source.Count-=amount;if(source.Count>0)return;if(IsGearArea(a))SetGear(a,null);else inventory.Slots.Remove(source);}
  bool CanRemoveGear(ItemArea a){if(a==ItemArea.RigEquipment&&Chest.Slots.Count>0||a==ItemArea.PackEquipment&&Bag.Slots.Count>0){Notify("请先清空容器内容，再卸下或丢弃；更换装备可保留内容。");return false;}return true;}
  bool MutateItems(Func<bool> action,string message=""){
   if(Phase==GamePhase.Base)return CampChange(action,message);BeginDropMutation314();if(Phase!=GamePhase.Raid||Paused)return false;
   long priorWeight320=CarryGrams320;CaptureCarry();string before=JsonUtility.ToJson(Save);var boxes=new List<LootContainer>(Map.Loot);var contents=new List<List<ItemStack>>();var corpseContents=new List<CorpseLoot325>();foreach(var b in boxes){var list=new List<ItemStack>();foreach(var p in b.Items)list.Add(RaidInventory.Copy(p));contents.Add(list);corpseContents.Add(b.Corpse325?.Copy());}var open=OpenLoot;float health=Player.Health;
   bool done=false;try{done=action();if(done&&!WeightMutationAllowed320(priorWeight320))done=false;if(done){CancelBackpackBindings343(Save.CampBag);Save.SafeItems=Safe.Snapshot();if(!TrainingActive)done=WriteSave();}}catch(Exception ex){Notify(ex.Message);Debug.LogWarning(ex.Message);}
   if(!done){Save=JsonUtility.FromJson<BaseSave>(before);RestoreCarry();Player.Health=health;foreach(var b in Map.Loot)if(!boxes.Contains(b)&&b.View)Destroy(b.View);Map.Loot.Clear();Map.Loot.AddRange(boxes);for(int i=0;i<boxes.Count;i++){boxes[i].Items.Clear();boxes[i].Items.AddRange(contents[i]);if(corpseContents[i]!=null){if(boxes[i].Corpse325==null)boxes[i].Corpse325=corpseContents[i].Copy();else boxes[i].Corpse325.Restore(corpseContents[i]);}else boxes[i].Corpse325=null;}OpenLoot=open;return false;}
   CommitDropViews314();CancelArmorRepair();ReloadTime=HealTime=0;reloadId=healId=null;if(message!="")Notify(message);return true;
  }
  public bool MoveItem(ItemArea from,int sourceCell,ItemArea to,int targetCell,int amount=-1,bool? rotated=null){
   if(CorpseSource325(to)){Notify("尸体物资只能取出，不能放入或交换。");return false;}
   if(CorpseLoot325.IsArea(from)&&!CorpseContentEmpty325(from,true))return false;
   if(CorpseSource325(from)&&IsGearArea(to)&&GearAt(to)!=null){Notify("请先卸下当前装备，再装备尸体上的物品。");return false;}
   if(CanLoadGun314(from,sourceCell,to,targetCell))return LoadGun314(from,sourceCell,to,targetCell);
   if(CanInsertCase314(from,sourceCell,to,targetCell))return InsertCase314(from,sourceCell,to,targetCell);
   if(!Access(from)||!Access(to)||!CanMoveItem(from,sourceCell,to,targetCell,rotated))return false;
   return MutateItems(()=>{
    var source=ItemAt(from,sourceCell);if(source==null)return false;var shape=RaidInventory.Copy(source);if(rotated.HasValue)shape.Rotated=rotated.Value;if(from==to&&source.Cell==targetCell&&shape.Rotated==source.Rotated)return false;
    if(to==ItemArea.Safe&&RaidInventory.IsWearable(source.Kind)){Notify("安全箱不能存放防具、武器、胸挂或背包。");return false;}
    if(IsGearArea(to))return EquipInto(from,source,to);
    if(IsGearArea(from)&&targetCell<0&&!CanRemoveGear(from))return false;
    var target=InventoryFor(to);if(target==null)return false;int n=amount<0?source.Count:Math.Min(amount,source.Count);if(n<=0)return false;
    if(targetCell<0&&RaidInventory.IsWearable(source.Kind)){targetCell=target.FreeCell(shape);if(targetCell<0)return false;}
    if(targetCell<0){int left=target.Add(shape,n);int moved=n-left;if(moved<=0){Notify("空间不足，无法放入。");return false;}RemoveAmount(from,source,moved);return true;}
    var dest=target.At(targetCell);if(dest==source)dest=null;
    if(dest!=null&&!RaidInventory.IsWearable(source.Kind)&&target.StackLimit(source.Kind)>1&&RaidInventory.Compatible(dest,source)){int moved=Math.Min(n,target.StackLimit(source.Kind)-dest.Count);if(moved<=0)return false;dest.Count+=moved;RemoveAmount(from,source,moved);return true;}
    if(HasSwapTargets314(from,source,to,targetCell,shape)){if(n!=source.Count)return false;return CommitSwap314(from,source,to,targetCell,shape);}
    foreach(var p in target.Slots){if(p==source||!Overlaps(target,shape,targetCell,p,p.Cell))continue;if(dest!=null&&dest!=p){Notify("落点覆盖多件物品，无法互换。");return false;}dest=p;}
    if(dest!=null){if(n!=source.Count||!CanMoveItem(from,sourceCell,to,targetCell,rotated)){Notify("双方空间或装备类型不匹配，无法互换。");return false;}return SwapGridItems(from,source,to,targetCell,dest,rotated);}
    if(IsGearArea(from)&&!CanRemoveGear(from))return false;
    if(!target.Fits(shape,targetCell,from==to&&n==source.Count?source:null)){Notify("需要连续空间，不能越过边界或斜线格。");return false;}
    if(from==to&&n==source.Count){source.Cell=targetCell;source.Rotated=shape.Rotated;return true;}
    var copy=RaidInventory.Copy(shape);copy.Count=Math.Min(n,target.StackLimit(source.Kind));copy.Cell=targetCell;if(copy.Count<source.Count)copy.Id=Guid.NewGuid().ToString("N");target.Slots.Add(copy);int movedTotal=copy.Count;if(from==ItemArea.Warehouse&&!target.WarehouseMode&&n>copy.Count)movedTotal+=n-copy.Count-target.Add(shape,n-copy.Count);RemoveAmount(from,source,movedTotal);return true;
   });
  }
  bool EquipInto(ItemArea from,ItemStack source,ItemArea to){
   if(!CanEquipMove(from,source,to))return false;
   if(IsGearArea(from)){StoreActiveMagazine314();var other=GearAt(to);SetGear(to,source);SetGear(from,other);if(from==ItemArea.Weapon||to==ItemArea.Weapon)WeaponChanged279();return true;}
   var old=RaidInventory.Copy(GearAt(to));var incoming=RaidInventory.Copy(source);int originCell=source.Cell;var origin=InventoryFor(from);origin.Slots.Remove(source);
   RaidInventory resized=null;bool rig=to==ItemArea.RigEquipment,pack=to==ItemArea.PackEquipment;
   if(rig||pack){resized=RaidInventory.Container(incoming,rig);if(!resized.Repack((rig?Chest:Bag).Snapshot())){Notify("新容器装不下当前物资，请先腾出空间。");return false;}}
   SetGear(to,incoming);if(rig)Chest=resized;if(pack)Bag=resized;
   if(old!=null){origin=InventoryFor(from);old.Cell=origin.Fits(old,originCell)?originCell:origin.FreeCell(old);if(old.Cell<0){Notify("没有空间存放换下的装备。");return false;}origin.Slots.Add(old);}
   if(to==ItemArea.Weapon)WeaponChanged279();return true;
  }
  public bool QuickTransfer(ItemArea from,int cell){if(!InventoryOpen||!Access(from))return false;if(CorpseLoot325.IsArea(from)&&!CorpseContentEmpty325(from,true))return false;var source=ItemAt(from,cell);if(source==null)return false;if(KeyCase314.IsCase(source.Kind))return OpenKeyCase314At(from,cell);string id=source.Id;
   if(from==ItemArea.KeyCase314){foreach(var a in Phase==GamePhase.Base&&Panel==CampPanel.Warehouse?new[]{ItemArea.Warehouse,ItemArea.Backpack,ItemArea.Rig,ItemArea.Safe}:new[]{ItemArea.Backpack,ItemArea.Rig,ItemArea.Safe})if(MoveItem(from,cell,a,-1))return true;return false;}
   if(from==ItemArea.Warehouse||from==ItemArea.Loot||CorpseLoot325.IsArea(from)){
    foreach(var slot in new[]{ItemArea.Weapon,ItemArea.SecondWeapon,ItemArea.Armor,ItemArea.RigEquipment,ItemArea.PackEquipment})if(GearAt(slot)==null&&GearFits(slot,source.Kind)&&CanMoveItem(from,cell,slot,0))return MoveItem(from,cell,slot,0);
    bool supply=AmmoRules270.IsAmmo(source.Kind)||ConsumableRules.IsSupply(source.Kind);bool moved=false;
    var destinations=new List<ItemArea>();if((from==ItemArea.Loot||CorpseLoot325.IsArea(from))&&Phase==GamePhase.Raid&&AutoSafeEnabled&&!RaidInventory.IsWearable(source.Kind)&&RaidInventory.Value(source)>=AutoSafeThreshold)destinations.Add(ItemArea.Safe);
    destinations.Add(supply?ItemArea.Rig:ItemArea.Backpack);destinations.Add(supply?ItemArea.Backpack:ItemArea.Rig);
    if(!RaidInventory.IsWearable(source.Kind)&&!destinations.Contains(ItemArea.Safe))destinations.Add(ItemArea.Safe);
    foreach(var area in destinations){var remaining=InventoryFor(from)?.Slots.Find(p=>p.Id==id);if(remaining==null)break;if(CanMoveItem(from,remaining.Cell,area,-1)){bool done=MoveItem(from,remaining.Cell,area,-1);if(!done)return moved;moved=true;}}
    Notify(moved?"已拾取：补给优先胸挂，其余优先背包。":"随身空间不足。");return moved;
   }
   ItemArea to=Phase==GamePhase.Base&&Panel==CampPanel.Warehouse?ItemArea.Warehouse:OpenLoot!=null&&OpenLoot.Corpse325==null?ItemArea.Loot:from==ItemArea.Rig?ItemArea.Backpack:ItemArea.Rig;
   return MoveItem(from,cell,to,-1);
  }
  public bool SplitItem(ItemArea area,int cell)=>SplitItem(area,cell,Math.Max(1,(ItemAt(area,cell)?.Count??0)/2));
  public bool SplitItem(ItemArea area,int cell,int amount){if(!Access(area)||CorpseSource325(area))return false;return MutateItems(()=>{var inv=InventoryFor(area);var item=ItemAt(area,cell);if(item==null||amount<=0||amount>=item.Count)return false;int free=inv.FreeCell(item);if(free<0){Notify("同一容器没有可用空间。");return false;}var part=RaidInventory.Copy(item);part.Id=Guid.NewGuid().ToString("N");part.Count=amount;part.Cell=free;inv.Slots.Add(part);item.Count-=amount;return true;},"物品数量已分开。");}
  public bool DropItem(ItemArea area,int cell){if(!Access(area)||CorpseLoot325.IsArea(area)&&!CorpseContentEmpty325(area,true))return false;return MutateItems(()=>{var item=ItemAt(area,cell);if(item==null||!CanRemoveGear(area))return false;var copy=RaidInventory.Copy(item);DropToPile314(copy);RemoveAmount(area,item,item.Count);return true;},"整叠物品已丢在脚边，可按F拾回。");}
  public bool UnloadMagazine()=>MutateItems(TryUnloadMagazine270,"弹药已卸到随身物资。");
  bool StoreMagazine279(RaidInventory rig,RaidInventory bag){if(TrainingLoadoutAvailable301)return true;int remaining=rig.Add(MagazineAmmo,Magazine-MagazineFound);remaining=bag.Add(MagazineAmmo,remaining);if(remaining>0)return false;remaining=rig.Add(MagazineAmmo,MagazineFound,ItemOrigin.Certified);remaining=bag.Add(MagazineAmmo,remaining,ItemOrigin.Certified);return remaining==0;}
  bool TryUnloadMagazine270(){if(Magazine<=0)return false;if(TrainingLoadoutAvailable301){Magazine=MagazineFound=0;return true;}var tempRig=Chest.Clone();var tempBag=Bag.Clone();if(!StoreMagazine279(tempRig,tempBag))return false;Chest=tempRig;Bag=tempBag;Magazine=MagazineFound=0;StoreActiveMagazine314();return true;}
  public void StoreMaterials(){CampChange(()=>{if(Panel!=CampPanel.Warehouse)return false;foreach(var inv in new[]{Chest,Bag})for(int i=inv.Slots.Count-1;i>=0;i--){var s=inv.Slots[i];if(AmmoRules270.IsAmmo(s.Kind)||ConsumableRules.IsSupply(s.Kind)||RaidInventory.IsWearable(s.Kind))continue;CampRules.Deposit(Save,s);inv.Slots.RemoveAt(i);}return true;},"材料已入仓。");}
  public bool CanUse(ItemArea area,ItemStack item){if(item!=null&&KeyCase314.IsCase(item.Kind))return item.Identified&&Access(area);if(item==null||RetiredMedicine321.IsRetired(item.Kind)||RaidInventory.IsHelmet(item.Kind)||!item.Identified||!Access(area)||IsGearArea(area))return false;if(RaidInventory.IsWearable(item.Kind))return true;if(area==ItemArea.Loot||area==ItemArea.Warehouse||area==ItemArea.Safe||CorpseSource325(area))return false;if(ConsumableRules.IsRepair(item.Kind))return area==ItemArea.Rig&&Phase==GamePhase.Raid&&RepairTarget()!=null&&RepairKit320.Remaining(item)>0;return AmmoRules270.IsAmmo(item.Kind)&&ActiveWeapon279!=null&&WeaponRules279.Compatible(Equipped.Kind,item.Kind)&&(Magazine<MagazineCapacity279||MagazineAmmo!=item.Kind);}
  public bool UseItem(ItemArea area,int cell){if(KeyCase314.IsCase(ItemAt(area,cell)?.Kind??ItemKind.Cola))return OpenKeyCase314At(area,cell);var item=ItemAt(area,cell);if(!CanUse(area,item)){Notify("此物品当前无法直接使用；材料请到营地工作台兑换。");return false;}
   if(ConsumableRules.IsRepair(item.Kind))return BeginArmorRepair(area,cell);
   if(RaidInventory.IsWearable(item.Kind)){var to=RaidInventory.IsRig(item.Kind)?ItemArea.RigEquipment:RaidInventory.IsPack(item.Kind)?ItemArea.PackEquipment:RaidInventory.IsHelmet(item.Kind)?ItemArea.Head:RaidInventory.IsArmor(item.Kind)?ItemArea.Armor:ItemArea.Weapon;return MoveItem(area,cell,to,0);}
   if(!AmmoRules270.IsAmmo(item.Kind))return false;
   if(Phase==GamePhase.Base)return MutateItems(()=>LoadAmmoInCamp270(area,item),"装弹完成。");
   if(ReloadTime>0||RepairTime>0||!PrepareAmmo270(item.Kind))return false;reloadAmmo270=item.Kind;reloadArea=area;reloadId=item.Id;ReloadTime=ActiveWeapon279.ReloadSeconds;CloseInventory();return true;
  }
 }
}
