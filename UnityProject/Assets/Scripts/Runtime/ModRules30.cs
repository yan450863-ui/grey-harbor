using System;
using System.Collections.Generic;
using UnityEngine;
namespace GreyHarbor {
 public enum ModSlot30 { Muzzle,Stock,Foregrip,Reargrip,Magazine,Laser,Optic }
 public sealed class ModDefinition30 {public ItemKind Kind;public ModSlot30 Slot;public string Name,Description;public int Grade,Price;}
 public static class ModRules30 {
  public static bool IsMod(ItemKind k)=>k>=ItemKind.GreenMuzzle30&&k<=ItemKind.BlueOptic30;
  static ModDefinition30 Def(ItemKind k,ModSlot30 slot,string name,int grade,int price,string text)=>new ModDefinition30{Kind=k,Slot=slot,Name=name,Grade=grade,Price=price,Description=text};
  public static readonly ModDefinition30[] All={
   Def(ItemKind.GreenMuzzle30,ModSlot30.Muzzle,"简易制退器",0,60,"后坐力降低8%，枪声警戒范围增加10%。"),
   Def(ItemKind.BlueMuzzle30,ModSlot30.Muzzle,"精密制退器",1,160,"后坐力降低15%，枪声警戒范围增加20%。"),
   Def(ItemKind.GreenStock30,ModSlot30.Stock,"轻型枪托",0,40,"后坐力与散布恢复速度提高8%。"),
   Def(ItemKind.BlueStock30,ModSlot30.Stock,"稳固枪托",1,120,"后坐力与散布恢复速度提高15%，持枪移动速度降低3%。"),
   Def(ItemKind.GreenForegrip30,ModSlot30.Foregrip,"简易前握把",0,50,"连射散布增长降低8%。"),
   Def(ItemKind.BlueForegrip30,ModSlot30.Foregrip,"稳定前握把",1,140,"连射散布增长降低15%，散布恢复速度降低5%。"),
   Def(ItemKind.GreenReargrip30,ModSlot30.Reargrip,"防滑后握把",0,45,"移动射击稳定性提高4点。"),
   Def(ItemKind.BlueReargrip30,ModSlot30.Reargrip,"人体工学后握把",1,120,"移动射击稳定性提高8点，持枪移动速度降低3%。"),
   Def(ItemKind.GreenMagazine30,ModSlot30.Magazine,"扩容弹匣",0,70,"容量增加20%向上取整，换弹增加0.15秒。按口径适配；拆卸先退回全部装填弹药。"),
   Def(ItemKind.BlueMagazine30,ModSlot30.Magazine,"大容量弹匣",1,180,"容量增加50%向上取整，换弹增加0.35秒。按口径适配；拆卸先退回全部装填弹药。"),
   Def(ItemKind.BlueLaser30,ModSlot30.Laser,"指向激光器",1,150,"显示真实枪口中心指向与当前射程，遇遮挡截断；不提升伤害、穿甲或精度。"),
   Def(ItemKind.BlueOptic30,ModSlot30.Optic,"精密瞄具",1,160,"右键按住立即瞄准，圆环和圈外压暗平滑显现；显示原画面，仍遵守视距与墙体遮挡。")
  };
  // ID 289 is retained only for old save compatibility; no active green optic definition.
  public static ModDefinition30 Get(ItemKind k)=>Array.Find(All,d=>d.Kind==(k==ItemKind.GreenOptic30?ItemKind.BlueOptic30:k));
  public static string SlotName(ModSlot30 s)=>new[]{"枪口","枪托","前握把","后握把","弹匣","激光器","瞄具"}[(int)s];
  public static int MarketSub(ItemKind k)=>new[]{1,5,3,4,2,6,0}[(int)Get(k).Slot];
  public static List<ItemStack> Copy(List<ItemStack> items){var list=new List<ItemStack>();if(items!=null)foreach(var p in items)if(p!=null)list.Add(RaidInventory.Copy(p));return list;}
  public static ItemStack Installed(ItemStack gun,ModSlot30 slot)=>gun?.Attachments30?.Find(p=>p!=null&&IsMod(p.Kind)&&Get(p.Kind).Slot==slot);
  // Old guns accept a subset; slot support is explicit and shared by UI and actions.
  public static bool Compatible(ItemKind gun,ModSlot30 slot)=>WeaponRules279.IsWeapon(gun)&&(BlueWeapons30.IsGun(gun)||gun==ItemKind.AK&&slot!=ModSlot30.Laser||gun==ItemKind.MP5&&(slot==ModSlot30.Muzzle||slot==ModSlot30.Stock||slot==ModSlot30.Magazine||slot==ModSlot30.Optic)||gun==ItemKind.SKS&&(slot==ModSlot30.Muzzle||slot==ModSlot30.Stock||slot==ModSlot30.Magazine||slot==ModSlot30.Optic));
  public static string Describe(ItemStack gun){var p=Profile(gun);return $"{RaidInventory.GradeName(gun.Kind)}{p.Name} · {p.CaliberLabel}；伤害{p.Damage}、穿甲{p.Penetration}；弹匣{p.MagazineSize}发，换弹{p.ReloadSeconds:0.##}秒；射程{p.Range:0.#}米，移动稳定性{p.MovementStability}。已安装{gun.Attachments30?.Count??0}件配件；右键选择枪械改装。"+(gun.Kind==ItemKind.SG12?"整发8颗弹丸，破甲该发不伤血。":"");}
  public static int TotalValue(ItemStack gun){int total=RaidInventory.Value(gun.Kind);if(gun.Attachments30!=null)foreach(var p in gun.Attachments30)if(p!=null)total=checked(total+RaidInventory.Value(p.Kind));return checked(total+WeaponMagazine314.LoadedValue(gun));}
  public static int FoundValue(ItemStack gun){int total=gun.RaidFound?RaidInventory.Value(gun.Kind):0;if(gun.Attachments30!=null)foreach(var p in gun.Attachments30)if(p.RaidFound)total+=RaidInventory.Value(p.Kind);return total;}
  public static void Certify(ItemStack gun){Certification319.Resolve(gun);}
  public static void ClearFound(ItemStack gun){if(gun?.Keys314!=null)foreach(var key in gun.Keys314)key.RaidFound=false;if(gun?.Attachments30!=null)foreach(var p in gun.Attachments30)p.RaidFound=false;}
  static readonly Dictionary<int,WeaponProfile279> profiles=new Dictionary<int,WeaponProfile279>();
  public static WeaponProfile279 Profile(ItemStack gun){
   if(gun==null)return null;var b=WeaponRules279.Get(gun.Kind);if(b==null)return null;int mask=0;if(gun.Attachments30!=null)foreach(var mod in gun.Attachments30)if(mod!=null&&IsMod(mod.Kind))mask|=1<<((int)mod.Kind-278);if(mask==0)return b;int key=((int)gun.Kind<<14)|mask;if(profiles.TryGetValue(key,out var cached))return cached;
   var p=b.Copy30();var m=Installed(gun,ModSlot30.Muzzle);if(m!=null){float f=Get(m.Kind).Grade==0?.92f:.85f;p.FirstKick*=f;p.KickGrowth*=f;p.MaxKickGrowth*=f;p.MaxRecoil*=f;}
   m=Installed(gun,ModSlot30.Stock);if(m!=null){float f=Get(m.Kind).Grade==0?1.08f:1.15f;p.RecoilRecovery*=f;p.HeatRecovery*=f;}
   m=Installed(gun,ModSlot30.Foregrip);if(m!=null){p.HeatPerShot*=Get(m.Kind).Grade==0?.92f:.85f;if(Get(m.Kind).Grade==1)p.HeatRecovery*=.95f;}
   m=Installed(gun,ModSlot30.Reargrip);if(m!=null)p.MovementStability=Math.Min(95,p.MovementStability+(Get(m.Kind).Grade==0?4:8));
   m=Installed(gun,ModSlot30.Magazine);if(m!=null){bool blue=Get(m.Kind).Grade==1;p.MagazineSize=(int)Math.Ceiling(b.MagazineSize*(blue?1.5:1.2));p.ReloadSeconds+=blue?.35f:.15f;}
   // No mod affects damage, penetration, firing rate or the shotgun's pellet fan.
   if(profiles.Count>256)profiles.Clear();profiles[key]=p;return p;
  }
  static readonly ModSlot30[] movementSlots={ModSlot30.Stock,ModSlot30.Reargrip};
  public static float MoveMultiplier(ItemStack gun){float result=1;foreach(var slot in movementSlots){var m=Installed(gun,slot);if(m!=null&&Get(m.Kind).Grade==1)result*=.97f;}return Mathf.Max(.9f,result);}
  public static float NoiseRange(ItemStack gun){float n=gun?.Kind==ItemKind.SG12?18:gun?.Kind==ItemKind.AR16?14:gun?.Kind==ItemKind.Dagger?9:12;var m=Installed(gun,ModSlot30.Muzzle);return n*(m==null?1:Get(m.Kind).Grade==0?1.1f:1.2f);}
  public static void Validate(ItemStack gun){RetiredOptic308.Convert(gun);if(gun==null||gun.Attachments30==null)return;var slots=new HashSet<ModSlot30>();foreach(var p in gun.Attachments30){if(p==null||!IsMod(p.Kind)||p.Count!=1||!Compatible(gun.Kind,Get(p.Kind).Slot)||!slots.Add(Get(p.Kind).Slot))throw new InvalidOperationException("枪械配件数据无效");}}
 }
 public sealed partial class RaidGame {
  bool ReturnMod30(ItemStack item,ItemArea origin){var inv=CorpseSource325(origin)?null:InventoryFor(origin);if(inv!=null&&inv.Add(item)==0)return true;if(Phase==GamePhase.Base){CampRules.Deposit(Save,item);return true;}if(Chest.Add(item)==0)return true;if(Bag.Add(item)==0)return true;Notify("空间不足，无法拆卸配件。");return false;}
  public bool InstallMod30(ItemArea gunArea,int gunCell,ItemArea modArea,string id){return MutateItems(()=>{
   if(CorpseSource325(gunArea)||!Access(gunArea)||!Access(modArea)||ReloadTime>0||HealTime>0||RepairTime>0)return false;
   var gun=ItemAt(gunArea,gunCell);var inv=InventoryFor(modArea);var mod=inv?.Slots.Find(x=>x.Id==id);if(gun==null||mod==null||!gun.Identified||!mod.Identified||!ModRules30.IsMod(mod.Kind))return false;var slot=ModRules30.Get(mod.Kind).Slot;
   if(!ModRules30.Compatible(gun.Kind,slot)){Notify("这把枪没有兼容接口。");return false;}
   if(slot==ModSlot30.Magazine&&!ReturnGunRoundsCarry314(gun)){Notify("胸挂和背包空间不足，不能退回弹药。");return false;}
   inv=InventoryFor(modArea);mod=inv?.Slots.Find(x=>x.Id==id);if(mod==null)return false;var installed=RaidInventory.Copy(mod);installed.Count=1;if(mod.Count>1)installed.Id=Guid.NewGuid().ToString("N");RemoveAmount(modArea,mod,1);var old=ModRules30.Installed(gun,slot);
   if(old!=null&&!ReturnMod30(old,modArea))return false;
   gun.Attachments30=gun.Attachments30??new List<ItemStack>();if(old!=null)gun.Attachments30.Remove(old);installed.Cell=-1;gun.Attachments30.Add(installed);ModRules30.Validate(gun);ResetScope30();ResetAkHandling276();return true;
  },"配件已安装，枪械占格与品质保持不变。");}
  public bool RemoveMod30(ItemArea gunArea,int gunCell,ModSlot30 slot){return MutateItems(()=>{
   if(!Access(gunArea)||ReloadTime>0||HealTime>0||RepairTime>0)return false;var gun=ItemAt(gunArea,gunCell);var mod=ModRules30.Installed(gun,slot);if(mod==null)return false;
   if(slot==ModSlot30.Magazine&&!ReturnGunRoundsCarry314(gun)){Notify("胸挂和背包空间不足，不能退回弹药。");return false;}
   if(!ReturnMod30(mod,gunArea))return false;gun.Attachments30.Remove(mod);ResetScope30();ResetAkHandling276();return true;
  },"配件已拆卸。");}
 }
}