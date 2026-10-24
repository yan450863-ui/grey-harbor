using System;using UnityEngine;
namespace GreyHarbor {
 public static class ArmorLoadout320 {
  public static readonly ItemKind[] All={ItemKind.Armor,ItemKind.ReinforcedArmor320,ItemKind.BlueArmor,ItemKind.HeavyArmor320};
  public static bool IsNew(ItemKind k)=>k==ItemKind.ReinforcedArmor320||k==ItemKind.HeavyArmor320;
  public static bool IsArmor(ItemKind k)=>k==ItemKind.Armor||k==ItemKind.BlueArmor||IsNew(k);
  public static float Maximum(ItemKind k)=>k==ItemKind.Armor?80:k==ItemKind.ReinforcedArmor320?100:k==ItemKind.BlueArmor?120:k==ItemKind.HeavyArmor320?150:0;
  public static int Grade(ItemKind k)=>k==ItemKind.BlueArmor||k==ItemKind.HeavyArmor320?1:0;
  public static int Price(ItemKind k)=>k==ItemKind.ReinforcedArmor320?3000:k==ItemKind.HeavyArmor320?5000:0;
  public static string Name(ItemKind k)=>k==ItemKind.Armor?"轻型防弹衣":k==ItemKind.ReinforcedArmor320?"加固防弹衣":k==ItemKind.BlueArmor?"标准战术护甲":"重型战术护甲";
  public static string Description(ItemKind k)=>$"{Name(k)}：{RaidInventory.GradeName(k)}{Grade(k)+1}级，耐久上限{Maximum(k):0}，重量{CarryWeight320.UnitGrams(k)/1000f:0.0}kg；护甲先吸收攻击，破甲该发不伤血。机动性只受总负重档位影响。";
  public static Texture2D Texture(ItemKind k){string name=k.ToString();var t=Resources.Load<Texture2D>("ItemArt/"+name);if(t!=null)return t;string file=System.IO.Path.Combine(Application.streamingAssetsPath,"ArmorArt320",name+".png");if(System.IO.File.Exists(file)){t=new Texture2D(2,2,TextureFormat.RGBA32,false);if(t.LoadImage(System.IO.File.ReadAllBytes(file))){t.filterMode=FilterMode.Point;t.wrapMode=TextureWrapMode.Clamp;return t;}}return ItemIcons.Get(k==ItemKind.HeavyArmor320?ItemKind.BlueArmor:ItemKind.Armor);}
 }
 public static class RepairKit320 {
  public static float Capacity(ItemKind k)=>k==ItemKind.GreenArmorKit?120:k==ItemKind.ArmorKit?150:0;
  public static void Ensure(ItemStack kit){if(kit==null||!ConsumableRules.IsRepair(kit.Kind))return;if(!kit.RepairReady320){kit.RepairPoints320=Capacity(kit.Kind);kit.RepairReady320=true;}kit.RepairPoints320=Mathf.Clamp(kit.RepairPoints320,0,Capacity(kit.Kind));}
  public static float Remaining(ItemStack kit){Ensure(kit);return kit?.RepairPoints320??0;}
  public static float Cost(ItemStack kit,ItemStack armor){if(kit==null||armor==null||!ConsumableRules.IsRepair(kit.Kind)||!RaidInventory.IsArmor(armor.Kind))return 0;Ensure(kit);ArmorBalance.Ensure(armor);float e=ConsumableRules.RepairEfficiency(RaidInventory.Grade(kit.Kind),RaidInventory.Grade(armor.Kind));return Mathf.Min(kit.RepairPoints320,Mathf.Min(ConsumableRules.RepairBase(kit.Kind),Mathf.Max(0,ArmorBalance.Max(armor.Kind)-armor.ArmorPoints)/e));}
  public static float Apply(ItemStack kit,ItemStack armor){float cost=Cost(kit,armor);if(cost<=0)return 0;float restored=cost*ConsumableRules.RepairEfficiency(RaidInventory.Grade(kit.Kind),RaidInventory.Grade(armor.Kind));armor.ArmorPoints=Mathf.Min(ArmorBalance.Max(armor.Kind),armor.ArmorPoints+restored);kit.RepairPoints320=Mathf.Max(0,kit.RepairPoints320-cost);return restored;}
  public static int Value(ItemStack kit){Ensure(kit);return Mathf.FloorToInt(RaidInventory.Value(kit.Kind)*kit.RepairPoints320/Capacity(kit.Kind));}
 }
}
