using System;
namespace GreyHarbor {
 public enum ItemGroup { Combat, Collection, Crafting }
 public enum CollectionCategory { None, Living, ToolsParts, Electronics, MedicalMaterials, EnergyChemicals, Biological, Information, Collectibles }
 public sealed partial class RaidInventory {
  public static ItemGroup Group(ItemKind k)=>AmmoCrafting328.IsComponent(k)?ItemGroup.Crafting:RetiredMedicine321.IsRetired(k)?ItemGroup.Combat:KeyCase314.IsCase(k)?ItemGroup.Combat:KeyRules2760.IsKey(k)?ItemGroup.Combat:IsWearable(k)||ModRules30.IsMod(k)||AmmoRules270.IsAmmo(k)||ConsumableRules.IsSupply(k)?ItemGroup.Combat:ItemGroup.Collection;
  public static CollectionCategory Category(ItemKind k){if(Group(k)!=ItemGroup.Collection)return CollectionCategory.None;if(KeyCase314.IsCase(k)||KeyRules2760.IsKey(k))return CollectionCategory.None;if(FormalCollectibles2749.IsNew(k))return FormalCollectibles2749.Category(k);if(ModRules30.IsMod(k)||WeaponRules279.IsWeapon(k)||AmmoRules270.IsAmmo(k))return CollectionCategory.None;if(CollectibleData260.IsNew(k))return CollectibleData260.Get(k).Category;if((int)k>=32)return CollectionExpansion.Category(k);switch(k){
   case ItemKind.Cola:case ItemKind.Cloth:return CollectionCategory.Living;
   case ItemKind.Scrap:case ItemKind.GunParts:case ItemKind.Tools:return CollectionCategory.ToolsParts;
   case ItemKind.Circuit:return CollectionCategory.Electronics;
   case ItemKind.Medical:return CollectionCategory.MedicalMaterials;
   case ItemKind.Powder:return CollectionCategory.EnergyChemicals;
   case ItemKind.Biomass:case ItemKind.Hide:case ItemKind.Chitin:return CollectionCategory.Biological;
   case ItemKind.SecretArchive:case ItemKind.Intel:return CollectionCategory.Information;
   case ItemKind.HarborCoin:case ItemKind.BronzeStatuette:return CollectionCategory.Collectibles;
   default:if(Group(k)==ItemGroup.Collection)throw new ArgumentOutOfRangeException(nameof(k),k,"收集物品必须配置分类");return CollectionCategory.None;
  }}
  public static string CategoryName(CollectionCategory c)=>new[]{"","生活物资","工具与零件","电子物资","医疗材料","能源与化工","生物材料","资料与情报","工艺与收藏品"}[(int)c];
  public static string Classification(ItemKind k)=>AmmoCrafting328.IsComponent(k)?"制作材料 / 弹药组件":RetiredMedicine321.IsRetired(k)?"旧版物品":KeyCase314.IsCase(k)?"钥匙 / 收纳包":KeyRules2760.IsKey(k)?"钥匙 / 普通钥匙":ModRules30.IsMod(k)?"枪械配件 / "+ModRules30.SlotName(ModRules30.Get(k).Slot):Group(k)==ItemGroup.Combat?"战斗物品":"收集物品 / "+CategoryName(Category(k));
 }
}
