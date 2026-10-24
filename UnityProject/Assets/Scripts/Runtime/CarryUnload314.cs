using System;using UnityEngine;
namespace GreyHarbor {public sealed partial class RaidGame {
 public bool UnloadCarry314()=>CampChange(()=>{if(Panel!=CampPanel.Warehouse)return false;var warehouse=CampRules.Grid(Save).Clone();int stacks=0;foreach(var inv in new[]{Chest,Bag,Safe})foreach(var item in inv.Slots){if(warehouse.Add(item)!=0){Notify("仓库空间不足，物资保持原位。");return false;}stacks++;}if(stacks==0){Notify("随身容器已经为空。");return false;}Save.Warehouse=warehouse.Snapshot();Chest.Slots.Clear();Bag.Slots.Clear();Safe.Slots.Clear();CloseKeyCase314();return true;},"背包、胸挂和安全箱物资已卸入仓库，装备与枪内弹药保持不变。");
}}
