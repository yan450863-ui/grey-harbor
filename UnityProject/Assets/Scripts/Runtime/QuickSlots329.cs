using System;
using UnityEngine;
namespace GreyHarbor {
 public static class QuickSlots329 {
  public const int Count=5;
  public static bool Supports(ItemKind kind)=>ConsumableRules.IsRepair(kind)&&!RetiredMedicine321.IsRetired(kind);
  public static bool CarriedArea(ItemArea area)=>area==ItemArea.Rig;
  public static int KeySlot(KeyCode key){int k=(int)key;if(k>=(int)KeyCode.Alpha3&&k<=(int)KeyCode.Alpha7)return k-(int)KeyCode.Alpha3;if(k>=(int)KeyCode.Keypad3&&k<=(int)KeyCode.Keypad7)return k-(int)KeyCode.Keypad3;return -1;}
 }
 public sealed partial class RaidGame {
  bool QuickScreenBlocked329=>IsLoading2721||Paused||Player==null||Player.Dead||GetComponent<RaidUI>()?.FullMapOpen318==true||AssetShowcaseMode2717;
  public bool QuickBindingAllowed329=>Save!=null&&!CampLocked&&!QuickScreenBlocked329&&InventoryOpen&&(Phase==GamePhase.Raid||Phase==GamePhase.Base&&Panel==CampPanel.Warehouse&&(!MenuCamp315||CampPage315==CampPage315.Warehouse));
  public ItemKind? QuickSlotKind329(int slot){if(slot<0||slot>=QuickSlots329.Count||Save?.QuickSlots329==null||slot>=Save.QuickSlots329.Length)return null;int encoded=Save.QuickSlots329[slot];if(encoded<=0||encoded>RaidInventory.KindCount)return null;var kind=(ItemKind)(encoded-1);return QuickSlots329.Supports(kind)?kind:(ItemKind?)null;}
  public float QuickSlotAmount329(int slot){var kind=QuickSlotKind329(slot);if(!kind.HasValue)return 0;float total=0;if(Chest!=null)foreach(var item in Chest.Slots)if(item!=null&&item.Identified&&item.Kind==kind.Value&&item.Count>0)total+=ConsumableRules.IsRepair(item.Kind)?RepairKit320.Remaining(item):item.Count;return total;}
  public bool BindQuickSlot329(int slot,ItemArea area,int cell){
   if(!QuickBindingAllowed329||slot<0||slot>=QuickSlots329.Count)return false;
   if(!QuickSlots329.CarriedArea(area)){Notify("请把消耗品放到胸挂后绑定快捷键。");return false;}
   var item=ItemAt(area,cell);if(item==null||!item.Identified||item.Count<=0||!QuickSlots329.Supports(item.Kind)){Notify("快捷槽只能绑定可使用的消耗品。");return false;}
   var previous=Save.QuickSlots329;var next=new int[QuickSlots329.Count];if(previous!=null)Array.Copy(previous,next,Math.Min(next.Length,previous.Length));next[slot]=(int)item.Kind+1;Save.QuickSlots329=next;
   if(!TrainingActive&&!WriteSave()){Save.QuickSlots329=previous;return false;}
   Notify("快捷键 "+(slot+3)+" 已绑定："+RaidInventory.Name(item.Kind));return true;
  }
  public bool UseQuickSlot329(int slot){
   if(Phase!=GamePhase.Raid||InventoryOpen||QuickScreenBlocked329||ReloadTime>0||HealTime>0||RepairTime>0)return false;
   var kind=QuickSlotKind329(slot);if(!kind.HasValue)return false;
   if(Chest!=null)foreach(var item in Chest.Slots){if(item==null||!item.Identified||item.Count<=0||item.Kind!=kind.Value||RepairKit320.Remaining(item)<=0)continue;if(BeginArmorRepair(ItemArea.Rig,item.Cell))return true;Notify(RepairTarget()==null?"当前护甲无需维修。":"当前无法使用维修包。");return false;}
   Notify("胸挂没有可用的"+RaidInventory.Name(kind.Value)+"。");return false;
  }
  string quickExpectedActive329,quickExpectedOther329;BaseSave quickWeaponSave329;GamePhase quickWeaponPhase329;bool quickWeaponTraining329,quickWeaponsReady329;int selectedWeaponSlot329;
  void SyncWeaponSlots329(){
   string active=Equipped?.Id,other=SecondWeapon?.Id;
   if(!quickWeaponsReady329||quickWeaponSave329!=Save||quickWeaponPhase329!=Phase||quickWeaponTraining329!=TrainingActive||quickExpectedActive329!=active||quickExpectedOther329!=other){selectedWeaponSlot329=0;RememberWeaponSlots329();}
  }
  void RememberWeaponSlots329(){quickExpectedActive329=Equipped?.Id;quickExpectedOther329=SecondWeapon?.Id;quickWeaponSave329=Save;quickWeaponPhase329=Phase;quickWeaponTraining329=TrainingActive;quickWeaponsReady329=true;}
  public int ActiveWeaponSlot329=>SelectedWeaponSlot329;
  public int SelectedWeaponSlot329 {get{SyncWeaponSlots329();return selectedWeaponSlot329;}}
  public ItemStack WeaponSlot329(int slot){SyncWeaponSlots329();return slot<0||slot>1?null:slot==selectedWeaponSlot329?Equipped:SecondWeapon;}
  public bool SelectWeaponSlot329(int slot){
   if(slot<0||slot>1||Phase!=GamePhase.Raid||InventoryOpen||QuickScreenBlocked329||ReloadTime>0||HealTime>0||RepairTime>0)return false;
   SyncWeaponSlots329();if(slot==selectedWeaponSlot329)return Equipped!=null;
   if(SecondWeapon==null||!WeaponRules279.IsWeapon(SecondWeapon.Kind))return false;
   bool changed=MutateItems(()=>{StoreActiveMagazine314();var next=SecondWeapon;WeaponMagazine314.Ensure(next);SecondWeapon=Equipped;Equipped=next;RestoreActiveMagazine314();return true;});
   if(!changed){SyncWeaponSlots329();return false;}
   selectedWeaponSlot329=slot;RememberWeaponSlots329();WeaponChanged279();Player.RefreshWeaponArt279();return true;
  }
  public void TickQuickInput329(){
   SyncWeaponSlots329();if(Phase!=GamePhase.Raid||InventoryOpen||QuickScreenBlocked329)return;
   if(Input.GetKeyDown(KeyCode.Alpha1)||Input.GetKeyDown(KeyCode.Keypad1)){SelectWeaponSlot329(0);return;}
   if(Input.GetKeyDown(KeyCode.Alpha2)||Input.GetKeyDown(KeyCode.Keypad2)){SelectWeaponSlot329(1);return;}
   for(int i=0;i<QuickSlots329.Count;i++)if(Input.GetKeyDown((KeyCode)((int)KeyCode.Alpha3+i))||Input.GetKeyDown((KeyCode)((int)KeyCode.Keypad3+i))){UseQuickSlot329(i);return;}
  }
 }
 public sealed partial class RaidUI {
  string QuickBindingHint329(ItemStack item){string keys="";for(int i=0;i<QuickSlots329.Count;i++)if(G.QuickSlotKind329(i)==item.Kind)keys+=(keys.Length==0?"":"、")+(i+3);return "按 3–7 绑定快捷槽"+(keys.Length>0?" · 已绑 "+keys:"");}
  internal void QABindSelection329(ItemArea area,int cell){selectedArea=area;selectedId=G.ItemAt(area,cell)?.Id;dragging=context=splitOpen=false;}
  bool HandleQuickBinding329(Event ev){
   if(ev==null||ev.type!=EventType.KeyDown||splitOpen||(context&&!quickBind337)||dragging||selling||saleConfirm||modPanel30||!G.QuickBindingAllowed329)return false;
   int slot=QuickSlots329.KeySlot(ev.keyCode);if(slot<0)return false;var item=Selected();if(!G.CanBindItem337(selectedArea,item))return false;
   G.BindQuickSlot329(slot,selectedArea,item.Cell);ev.Use();return true;
  }
 }
}
