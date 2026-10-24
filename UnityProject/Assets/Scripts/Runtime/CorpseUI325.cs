using System.Collections.Generic;
using UnityEngine;
namespace GreyHarbor {
 public sealed partial class RaidUI {
  float corpseRigScroll325,corpseBagScroll325;
  internal bool CorpsePanelVisible325=>G.Phase==GamePhase.Raid&&G.InventoryOpen&&!G.Paused&&G.OpenLoot!=null&&G.OpenLoot.Opened&&G.OpenLoot.Corpse325!=null;
  void ResetCorpseScroll325(){corpseRigScroll325=corpseBagScroll325=0;}

  void DrawCorpsePanel325(LootContainer container,bool searching){
   var corpse=container.Corpse325;float x=G.Phase==GamePhase.Raid?796:556;
   Frame(new Rect(x,98,776,737),inset,edge);
   Label(x+18,109,570,30,corpse.EnemyName,23,white);
   Label(x+582,113,173,23,"等级 "+corpse.Level,16,gold,TextAnchor.MiddleRight);
   if(!corpse.Soldier){DrawBiologicalCorpse325(container,x,searching);return;}

   DrawCorpsePortrait325(new Rect(x+18,153,102,144),corpse.Portrait);
   Label(x+18,303,102,20,"叛军尸体",13,muted,TextAnchor.MiddleCenter);
   Label(x+138,143,200,20,"武器",15,white);
   DrawCorpseGear325(new Rect(x+138,171,200,65),corpse.WeaponItems,ItemArea.CorpseWeapon325,corpse.WeaponRecoverable?"已取走":"武器损坏",searching);
   Label(x+138,246,200,40,"弹匣与配件随武器保留",12,muted);
   Label(x+355,143,100,20,"护甲",15,white);
   DrawCorpseGear325(new Rect(x+355,171,100,110),corpse.ArmorItems,ItemArea.CorpseArmor325,corpse.ArmorRecoverable?"已取走":"不可回收",searching);
   Label(x+473,143,100,20,"胸挂",15,white);
   DrawCorpseBody325(new Rect(x+473,171,100,150),corpse.RigGearItems,ItemArea.CorpseRigGear325,corpse.RigKind,corpse.RigRecoverable,searching);
   Label(x+591,143,100,20,"背包",15,white);
   DrawCorpseBody325(new Rect(x+591,171,100,150),corpse.PackGearItems,ItemArea.CorpsePackGear325,corpse.PackKind,corpse.PackRecoverable,searching);

   int unknown=CountCorpseUnknown325(corpse.WeaponItems,corpse.ArmorItems,corpse.RigGearItems,corpse.PackGearItems,corpse.RigItems,corpse.BagItems);
   Label(x+18,333,734,23,searching?"按住 F 搜索 · 完成后逐件鉴定":unknown>0?"正在逐件鉴定 · 剩余 "+unknown+" 件":"鉴定完成",15,searching||unknown>0?gold:mint);
   var rig=CorpseLoot325.FixedGrid(corpse.RigColumns,corpse.RigRows,corpse.RigItems);
   var bag=CorpseLoot325.FixedGrid(corpse.BagColumns,corpse.BagRows,corpse.BagItems);
   DrawCorpseContents325(x+18,373,"胸挂内物资",rig,ItemArea.CorpseRig325,ref corpseRigScroll325);
   DrawCorpseContents325(x+406,373,"背包内物资",bag,ItemArea.CorpseBag325,ref corpseBagScroll325);
   Label(x+18,784,733,20,"已鉴定物品可双击或拖动拿取 · 尸体只可取出",13,muted);
   Label(x+18,808,733,20,"取走胸挂或背包前，请先清空其内部物资",13,gold);
  }

  void DrawCorpseContents325(float x,float y,string title,RaidInventory inventory,ItemArea area,ref float scroll){
   Label(x,y,342,25,title,19,white);
   Label(x,y+31,342,20,"容量 "+inventory.Columns+"×"+inventory.Rows+" · 已用 "+inventory.UsedCells+" / "+inventory.SlotCapacity,13,muted);
   if(inventory.Columns<=0||inventory.Rows<=0){Label(x,y+62,342,50,"没有内部储物空间",14,muted);return;}
   // Keep the existing 50-pixel cells so drag targets and Q rotation use the same coordinates.
   ScrollGrid(inventory,area,new Rect(x,y+62,inventory.Columns*Tile,Mathf.Min(330,inventory.Rows*Tile)),ref scroll);
  }

  void DrawCorpsePortrait325(Rect r,Sprite sprite){
   Frame(r,surface,edge);
   if(sprite==null){Label(r.x,r.y,r.width,r.height,"尸体",16,muted,TextAnchor.MiddleCenter);return;}
   Rect pixels=sprite.textureRect;var uv=new Rect(pixels.x/sprite.texture.width,pixels.y/sprite.texture.height,pixels.width/sprite.texture.width,pixels.height/sprite.texture.height);
   GUI.DrawTextureWithTexCoords(ItemRotation314.Fit(new Rect(r.x+7,r.y+7,r.width-14,r.height-14),pixels.width/pixels.height),sprite.texture,uv,true);
  }

  void DrawCorpseGear325(Rect r,List<ItemStack> items,ItemArea area,string empty,bool searching){
   Frame(r,surface,edge);var item=items.Count>0?items[0]:null;
   if(item==null){Label(r.x+3,r.y+3,r.width-6,r.height-6,empty,13,muted,TextAnchor.MiddleCenter);return;}
   if(searching||!item.Identified)DrawUnknown(new Rect(r.x+2,r.y+2,r.width-4,r.height-4),item);
   else if(WeaponRules279.IsWeapon(item.Kind))DrawCorpseWeapon325(new Rect(r.x+2,r.y+2,r.width-4,r.height-4),item);
   else DrawItem(new Rect(r.x+2,r.y+2,r.width-4,r.height-4),item,area,item.Cell);
   hits.Add(new SlotHit{Rect=r,Area=area,Cell=item.Cell});
  }

  void DrawCorpseWeapon325(Rect r,ItemStack item){
   ItemFrame(r,item.Kind,selectedId==item.Id);var profile=ModRules30.Profile(item);
   ItemIcon(new Rect(r.x+3,r.y+17,r.width-6,r.height-35),item.Kind,item.Rotated);
   var clipping=text.clipping;text.clipping=TextClipping.Clip;
   try{
    Label(r.x+4,r.y+1,r.width*.44f,14,profile.Name,11,white);
    Label(r.x+r.width*.44f,r.y+1,r.width*.56f-4,14,profile.CaliberLabel.Replace(" ",""),10,white,TextAnchor.UpperRight);
    Label(r.x+4,r.yMax-16,r.width-8,14,CorpseWeaponRounds325(item),11,white,TextAnchor.MiddleRight);
   }finally{text.clipping=clipping;}
  }
  internal static string CorpseWeaponRounds325(ItemStack item)=>(item.MagazineReady314?item.Loaded314:0)+"/"+ModRules30.Profile(item).MagazineSize+" · 弹匣";

  void DrawCorpseBody325(Rect r,List<ItemStack> items,ItemArea area,ItemKind kind,bool recoverable,bool searching){
   if(items.Count>0){DrawCorpseGear325(r,items,area,"已取走",searching);return;}
   Frame(r,surface,edge);var color=GUI.color;
   try{GUI.color=new Color(color.r,color.g,color.b,color.a*.30f);ItemIcon(new Rect(r.x+6,r.y+6,r.width-12,r.height-42),kind);}finally{GUI.color=color;}
   Label(r.x+3,r.yMax-36,r.width-6,32,recoverable?"已取走":"损坏 / 不可回收",12,muted,TextAnchor.MiddleCenter);
   // This is a visual placeholder, never a fabricated ItemStack or an interactive slot.
  }

  void DrawBiologicalCorpse325(LootContainer container,float x,bool searching){
   var corpse=container.Corpse325;DrawCorpsePortrait325(new Rect(x+18,155,126,168),corpse.Portrait);
   var inventory=G.EnsureLootGrid(container);int unknown=inventory.Slots.FindAll(p=>!p.Identified).Count;
   Label(x+171,155,545,29,"生物材料",21,mint);
   Label(x+171,197,545,23,"容量 "+inventory.Columns+"×"+inventory.Rows+" · 已用 "+inventory.UsedCells+" / "+inventory.SlotCapacity,15,muted);
   Label(x+171,235,545,25,searching?"按住 F 搜索":unknown>0?"正在逐件鉴定 · 剩余 "+unknown+" 件":"鉴定完成",15,searching||unknown>0?gold:mint);
   ScrollGrid(inventory,ItemArea.Loot,new Rect(x+171,281,inventory.Columns*Tile,Mathf.Min(350,inventory.Rows*Tile)),ref lootScroll);
   Label(x+18,744,734,24,"异种尸体只可搜索生物材料，没有武器、胸挂和背包",14,muted);
   Label(x+18,782,734,24,"已鉴定材料可双击或拖动拿取",14,mint);
  }

  static int CountCorpseUnknown325(params List<ItemStack>[] lists){int count=0;foreach(var list in lists)foreach(var item in list)if(!item.Identified)count++;return count;}
 }
}
