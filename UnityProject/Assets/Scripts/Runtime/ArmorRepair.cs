using UnityEngine;
namespace GreyHarbor {
 public sealed partial class RaidGame {
  public float RepairTime;ItemArea repairArea;string repairKitId,repairTargetId;
  public ItemStack RepairTarget(){ArmorBalance.Ensure(Armor);return Armor!=null&&Armor.ArmorPoints<ArmorBalance.Max(Armor.Kind)?Armor:null;}
  public bool BeginArmorRepair(ItemArea area,int cell){var kit=ItemAt(area,cell);var target=RepairTarget();if(Phase!=GamePhase.Raid||Paused||kit==null||!kit.Identified||!ConsumableRules.IsRepair(kit.Kind)||RepairKit320.Remaining(kit)<=0||target==null||area!=ItemArea.Rig||HealTime>0||ReloadTime>0||RepairTime>0)return false;repairArea=area;repairKitId=kit.Id;repairTargetId=target.Id;RepairTime=4;CloseInventory();Notify("正在维修护甲，受击会中断。");return true;}
  public void CancelArmorRepair(){RepairTime=0;repairKitId=repairTargetId=null;}
  public void TickArmorRepair(float dt){if(RepairTime<=0)return;RepairTime-=dt;if(RepairTime>0)return;var area=repairArea;var id=repairKitId;var targetId=repairTargetId;CancelArmorRepair();MutateItems(()=>{var kit=InventoryFor(area)?.Slots.Find(p=>p.Id==id);var target=Armor?.Id==targetId?Armor:null;if(kit==null||target==null||!ConsumableRules.IsRepair(kit.Kind)||target.ArmorPoints>=ArmorBalance.Max(target.Kind))return false;if(RepairKit320.Apply(kit,target)<=0)return false;if(kit.RepairPoints320<=.0001f)RemoveAmount(area,kit,1);return true;},"维修完成，按甲包品质恢复护甲。");}
  public bool RepairAtCamp(ItemArea area,int cell){return CampChange(()=>{if(!Access(area))return false;var p=ItemAt(area,cell);if(p==null||!ArmorBalance.Defensive(p.Kind))return false;int price=ArmorBalance.RepairPrice(p);if(price<=0||Save.Coins<price){Notify("金币不足，或装备无需维修。");return false;}Save.Coins-=price;p.ArmorPoints=ArmorBalance.Max(p.Kind);return true;},"装备已修复至原护甲上限。");}
 }
}
