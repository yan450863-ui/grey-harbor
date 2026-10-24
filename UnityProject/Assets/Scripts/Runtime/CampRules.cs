using System;
using System.Collections.Generic;
using UnityEngine;
namespace GreyHarbor {
 public sealed class CampRecipe {
  public string Name;public ItemStack[] Costs;public ItemKind Output;public int Amount,Level;public bool CertifiedInputs328;
  public CampRecipe(string name,ItemKind output,int amount,int level,params ItemStack[] costs){Name=name;Output=output;Amount=amount;Level=level;Costs=costs;}
 }
 public static partial class CampRules {
  public static readonly CampRecipe[] Recipes=CreateRecipes328();
  static CampRecipe[] CreateRecipes328(){var list=new List<CampRecipe>(AmmoCrafting328.All);list.AddRange(new CampRecipe[]{
   new CampRecipe("制作简易甲包",ItemKind.GreenArmorKit,1,0,new ItemStack(ItemKind.Cloth,2),new ItemStack(ItemKind.GunParts,2),new ItemStack(ItemKind.Scrap,4)),
   new CampRecipe("组装AK",ItemKind.AK,1,1,new ItemStack(ItemKind.GunParts,4),new ItemStack(ItemKind.Scrap,6)),
   new CampRecipe("批量护甲维修包制作",ItemKind.ArmorKit,2,2,new ItemStack(ItemKind.Cloth,6),new ItemStack(ItemKind.GunParts,6),new ItemStack(ItemKind.Scrap,12),new ItemStack(ItemKind.Medical,2)),
   new CampRecipe("电子元件兑换",ItemKind.Powder,2,0,new ItemStack(ItemKind.Circuit,1)),
   new CampRecipe("工具兑换",ItemKind.GunParts,4,0,new ItemStack(ItemKind.Tools,1)),
   new CampRecipe("技术资料兑换",ItemKind.AK,2,0,new ItemStack(ItemKind.Intel,1)),
   new CampRecipe("组织交换",ItemKind.Medical,1,0,new ItemStack(ItemKind.Biomass,3)),
   new CampRecipe("韧皮加工",ItemKind.Cloth,3,0,new ItemStack(ItemKind.Hide,2)),
   new CampRecipe("硬壳交换",ItemKind.Tools,1,0,new ItemStack(ItemKind.Chitin,2))});return list.ToArray();}
  public static ItemStack[] UpgradeCosts(int level)=>level==0?new[]{new ItemStack(ItemKind.Scrap,6),new ItemStack(ItemKind.GunParts,3),new ItemStack(ItemKind.Circuit,1)}:new[]{new ItemStack(ItemKind.Scrap,10),new ItemStack(ItemKind.Powder,4),new ItemStack(ItemKind.Tools,2),new ItemStack(ItemKind.Intel,1)};

  public static RaidInventory Grid(BaseSave s)=>new RaidInventory(18,Math.Max(20,s.WarehouseRows),s.Warehouse,true);
  public static void SyncStock(BaseSave s){s.Stored=new int[RaidInventory.KindCount];s.Certified=new int[RaidInventory.KindCount];foreach(var p in s.Warehouse){s.Stored[(int)p.Kind]+=p.Count;if(Certification319.Collection(p.Kind)&&p.Origin==ItemOrigin.Certified)s.Certified[(int)p.Kind]+=p.Count;}}
  static void DepositGrowing(BaseSave s,ItemKind k,int count,bool certified){int left=Grid(s).Add(k,0);while(count>0){var g=Grid(s);left=g.Add(k,count,certified?ItemOrigin.Certified:ItemOrigin.Ordinary);s.Warehouse=g.Snapshot();count=left;if(left>0)s.WarehouseRows+=10;}SyncStock(s);}
  public static void Normalize(BaseSave s){
   s.Trades=s.Trades??new List<TradeRecord>();Economy270.Migrate(s);
   bool legacy=!s.GridInitialized;bool migrateCarry=s.Version<13||legacy;Array.Resize(ref s.Stored,RaidInventory.KindCount);Array.Resize(ref s.Certified,RaidInventory.KindCount);
   s.CampRig=s.CampRig??new List<ItemStack>();s.CampBag=s.CampBag??new List<ItemStack>();s.SafeItems=s.SafeItems??new List<ItemStack>();s.CampDrops=s.CampDrops??new List<CampDrop>();s.Prepared=s.Prepared??new List<ItemStack>();s.Warehouse=s.Warehouse??new List<ItemStack>();s.WarehouseRows=Math.Max(20,s.WarehouseRows);s.Coins=Math.Max(0,s.Coins);s.CampLevel=Mathf.Clamp(s.CampLevel,0,2);
   if(legacy){var stock=(int[])s.Stored.Clone();var marked=(int[])s.Certified.Clone();s.Warehouse.Clear();for(int i=0;i<stock.Length;i++){int n=Math.Max(0,stock[i]),c=Mathf.Clamp(marked[i],0,n);DepositGrowing(s,(ItemKind)i,n-c,false);DepositGrowing(s,(ItemKind)i,c,true);}foreach(var p in s.Prepared)DepositGrowing(s,p,p.Count);s.Prepared.Clear();s.RigGear=new ItemStack(ItemKind.StandardRig,1);s.PackGear=new ItemStack(ItemKind.StandardPack,1);s.GridInitialized=true;}
   RetiredMedicine321.Normalize(s);
   RetiredOptic308.Normalize(s);
   RetireHelmets(s);
   foreach(var list in new[]{s.Warehouse,s.CampRig,s.CampBag,s.SafeItems,s.Prepared})foreach(var kit in list)RepairKit320.Ensure(kit);foreach(var d in s.CampDrops)foreach(var kit in d.Items)RepairKit320.Ensure(kit);
   foreach(var p in s.Warehouse)p.Identified=true;foreach(var d in s.CampDrops)foreach(var p in d.Items)p.Identified=true;
   RaidInventory valid;while(true){valid=new RaidInventory(18,s.WarehouseRows,true);try{valid.Restore(s.Warehouse);break;}catch(InvalidOperationException ex){if(ex.Message!="物品空间不足")throw;s.WarehouseRows+=10;}}s.Warehouse=valid.Snapshot();
   if(!s.GearGranted){foreach(var k in RaidInventory.AvailableGear)DepositGrowing(s,k,1,false);s.GearGranted=true;}
   s.CampRig=NormalizeCarry(s,s.CampRig,RaidInventory.Container(s.RigGear,true),migrateCarry);s.CampBag=NormalizeCarry(s,s.CampBag,RaidInventory.Container(s.PackGear,false),migrateCarry);
   var safe=new RaidInventory(3,3);safe.Restore(NormalizeCarry(s,s.SafeItems,new RaidInventory(3,3),migrateCarry));if(safe.Slots.Exists(p=>RaidInventory.IsWearable(p.Kind)))throw new InvalidOperationException("安全箱含有不允许的装备");s.SafeItems=safe.Snapshot();
   if(s.Weapon!=null&&(s.Weapon.Count<=0||!WeaponRules279.IsWeapon(s.Weapon.Kind)))s.Weapon=null;NormalizeMagazine314(s);s.Version=14;foreach(var list in new[]{s.Warehouse,s.CampRig,s.CampBag,s.SafeItems})foreach(var p in list)ModRules30.Validate(p);ModRules30.Validate(s.Weapon);ModRules30.Validate(s.SecondWeapon);ArmorBalance.Normalize(s);Certification319.NormalizeSave(s);if(s.InRaid)foreach(var p in s.SafeItems)Certification319.Resolve(p);QuestRules319.Normalize(s,QuestRules319.DayAt(DateTime.UtcNow));SyncStock(s);
  }
  static List<ItemStack> NormalizeCarry(BaseSave save,List<ItemStack> items,RaidInventory target,bool migrate){if(!migrate){target.Restore(items);return target.Snapshot();}foreach(var s in items){var copy=RaidInventory.Copy(s);if(copy==null)continue;copy.Identified=true;if(target.Fits(copy,copy.Cell)&&copy.Count<=target.StackLimit(copy.Kind))target.Slots.Add(copy);else {int left=target.Add(copy,copy.Count);if(left>0)DepositGrowing(save,copy,left);}}return target.Snapshot();}
  public static void Deposit(BaseSave s,ItemKind k,int count,bool certified=false){if(count<=0)return;var g=Grid(s).Clone();if(g.Add(k,count,certified?ItemOrigin.Certified:ItemOrigin.Ordinary)>0)throw new InvalidOperationException("仓库空间不足，请整理后重试");s.Warehouse=g.Snapshot();SyncStock(s);}
  public static void Deposit(BaseSave s,ItemStack item){if(item==null||item.Count<=0)return;var copy=RaidInventory.Copy(item);copy.RaidFound=false;copy.Identified=true;if(copy.Origin!=ItemOrigin.Ordinary)copy.Origin=ItemOrigin.Certified;var g=Grid(s).Clone();if(g.Add(copy)>0)throw new InvalidOperationException("仓库空间不足，请整理后重试");s.Warehouse=g.Snapshot();SyncStock(s);}
  static void DepositGrowing(BaseSave s,ItemStack item,int count){var copy=RaidInventory.Copy(item);copy.RaidFound=false;copy.Identified=true;if(copy.Origin!=ItemOrigin.Ordinary)copy.Origin=ItemOrigin.Certified;while(count>0){var g=Grid(s);int left=g.Add(copy,count);s.Warehouse=g.Snapshot();count=left;if(left>0)s.WarehouseRows+=10;}SyncStock(s);}
  public static bool Spend(BaseSave s,ItemStack[] costs,bool onlyCertified,bool allowCertified=false){var g=Grid(s).Clone();foreach(var c in costs){if(c.Count<=0)continue;if(onlyCertified){if(!g.Consume(c.Kind,c.Count,ItemOrigin.Certified))return false;}else if(!allowCertified){if(!g.Consume(c.Kind,c.Count,ItemOrigin.Ordinary))return false;}else if(!g.Consume(c.Kind,c.Count))return false;}s.Warehouse=g.Snapshot();SyncStock(s);return true;}
  public static int Available(BaseSave s,ItemKind k){int n=s.Stored[(int)k];foreach(var list in new[]{s.CampRig,s.CampBag,s.SafeItems})foreach(var p in list)if(p.Kind==k)n+=p.Count;foreach(var p in new[]{s.Weapon,s.SecondWeapon,s.Helmet,s.Armor,s.RigGear,s.PackGear})if(p!=null&&p.Kind==k)n+=p.Count;if(k==s.CampMagazineAmmo)n+=s.CampMagazine;foreach(var drop in s.CampDrops)foreach(var p in drop.Items)if(p.Kind==k)n+=p.Count;return n;}
  public static bool Aid(BaseSave s){if(!s.AidAvailable)return false;bool changed=false;if(Available(s,ItemKind.AK)==0){Deposit(s,ItemKind.AK,1);changed=true;}if(Available(s,ItemKind.Ammo)+Available(s,ItemKind.GreenAmmo)==0){Deposit(s,ItemKind.GreenAmmo,30);changed=true;}if(s.RigGear==null&&Available(s,ItemKind.SmallRig)==0){Deposit(s,ItemKind.SmallRig,1);changed=true;}if(s.PackGear==null&&Available(s,ItemKind.SmallPack)==0){Deposit(s,ItemKind.SmallPack,1);changed=true;}if(changed)s.AidAvailable=false;return changed;}
 }
}
