using System;
using System.Collections.Generic;
using UnityEngine;
namespace GreyHarbor {
 public sealed partial class RaidUI {
  bool modPanel30;ItemArea modGunArea30;string modGunId30;ModSlot30 modSlot30;Vector2 modScroll30;
  ItemStack ModGun30(){var g=G.GearAt(modGunArea30);if(g?.Id==modGunId30)return g;return G.InventoryFor(modGunArea30)?.Slots.Find(p=>p.Id==modGunId30);}
  void OpenMod30(){var item=Selected();if(item==null||!WeaponRules279.IsWeapon(item.Kind))return;modGunArea30=selectedArea;modGunId30=item.Id;modSlot30=ModSlot30.Optic;ClearItemUI();modPanel30=true;}
  void DrawModPanel30(){var gun=ModGun30();if(gun==null||!G.InventoryOpen){modPanel30=false;return;}
   Fill(new Rect(0,0,1600,900),new Color(.02f,.03f,.03f,.97f));Frame(new Rect(80,65,1440,765),panel,mint*.5f);
   Label(112,85,950,40,"枪械改装 / "+RaidInventory.Name(gun.Kind),28,RaidInventory.Tint(gun.Kind));if(PanelButton(new Rect(1355,82,130,42),"返回")){modPanel30=false;return;}
   ItemIcon(new Rect(112,145,580,190),gun.Kind);var p=ModRules30.Profile(gun);
   Label(112,350,590,28,$"伤害 {p.Damage} / 穿甲 {p.Penetration} · {p.MagazineSize}发 · 换弹{p.ReloadSeconds:0.##}秒",18,white);
   Label(112,385,590,26,$"射程 {p.Range:0.#}米 · 移动稳定性 {p.MovementStability} · 整枪售价 {RaidInventory.Value(gun)}",17,gold);
   foreach(ModSlot30 slot in Enum.GetValues(typeof(ModSlot30))){int i=(int)slot;var r=new Rect(112,430+i*45,435,39);bool legal=ModRules30.Compatible(gun.Kind,slot);var item=ModRules30.Installed(gun,slot);
    if(PanelButton(r,ModRules30.SlotName(slot)+" / "+(!legal?"无接口":item==null?"未安装":RaidInventory.Name(item.Kind)),modSlot30==slot,!legal))modSlot30=slot;
    if(item!=null&&PanelButton(new Rect(558,r.y,120,39),"拆卸")){G.RemoveMod30(modGunArea30,gun.Cell,slot);return;}}
   if(G.Phase==GamePhase.Base&&PanelButton(new Rect(735,780,700,40),"进入训练场试射这把枪",true)){returnMod315=true;if(!G.StartModRange315(gun))returnMod315=false;return;}
   Label(735,145,700,30,"选择 "+ModRules30.SlotName(modSlot30)+" 配件",24,mint);
   var options=new List<Tuple<ItemArea,ItemStack>>();var areas=G.Phase==GamePhase.Base&&G.Panel==CampPanel.Warehouse?new[]{ItemArea.Warehouse,ItemArea.Rig,ItemArea.Backpack}:new[]{ItemArea.Rig,ItemArea.Backpack};
   foreach(var area in areas){var inv=G.InventoryFor(area);if(inv!=null)foreach(var item in inv.Slots)if(item.Identified&&ModRules30.IsMod(item.Kind)&&ModRules30.Get(item.Kind).Slot==modSlot30)options.Add(Tuple.Create(area,item));}
   modScroll30=GUI.BeginScrollView(new Rect(735,190,700,450),modScroll30,new Rect(0,0,675,Mathf.Max(440,options.Count*100)));
   for(int i=0;i<options.Count;i++){var option=options[i];var item=option.Item2;float y=i*100;ItemFrame(new Rect(4,y+4,80,80),item.Kind);ItemIcon(new Rect(8,y+8,72,72),item.Kind);Label(102,y+4,425,28,RaidInventory.Name(item.Kind),20,RaidInventory.Tint(item.Kind));WrappedLabel(new Rect(102,y+36,425,52),RaidInventory.Use(item.Kind),15,white);
    if(PanelButton(new Rect(548,y+22,110,42),"安装",true,!ModRules30.Compatible(gun.Kind,modSlot30))){G.InstallMod30(modGunArea30,gun.Cell,option.Item1,item.Id);GUI.EndScrollView();return;}}
   GUI.EndScrollView();if(options.Count==0)Label(755,215,660,28,"没有可用配件；可到交易行购买。",18,muted);
   WrappedLabel(new Rect(735,670,710,102),"安装不改变占格和品质。配件随枪存放与掉落，整枪按枪械加配件合价出售。拆卸弹匣先将原弹药退回胸挂、再放背包；空间不足时取消操作。",18,muted);
  }
 }
}