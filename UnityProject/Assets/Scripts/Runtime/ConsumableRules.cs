using UnityEngine;
namespace GreyHarbor {
 public static class ConsumableRules {
  public static readonly ItemKind[] Medicines={};
  public static readonly ItemKind[] Repairs={ItemKind.GreenArmorKit,ItemKind.ArmorKit};
  public static bool IsMedicine(ItemKind k)=>false;
  public static bool IsRepair(ItemKind k)=>k==ItemKind.GreenArmorKit||k==ItemKind.ArmorKit;
  public static bool IsSupply(ItemKind k)=>IsRepair(k);
  public static int Healing(ItemKind k)=>0;
  public static float HealSeconds(ItemKind k)=>0;
  public static int RepairBase(ItemKind k)=>k==ItemKind.GreenArmorKit?30:k==ItemKind.ArmorKit?40:0;
  public static float RepairEfficiency(int kitGrade,int armorGrade)=>kitGrade>=armorGrade?1:armorGrade-kitGrade==1?.8f:.5f;
  public static float RepairAmount(ItemKind kit,ItemKind armor)=>!IsRepair(kit)||!RaidInventory.IsArmor(armor)?0:RepairBase(kit)*RepairEfficiency(RaidInventory.Grade(kit),RaidInventory.Grade(armor));
  public static int MedicineCount(RaidInventory inv)=>0;
 }
 public sealed partial class RaidGame {
  // Compatibility for older verification and serialized action state; manual healing has been retired.
  public float HealDuration=2.5f;
  public bool BeginItemHeal(ItemArea area,int cell)=>false;
  public void CancelHeal(){HealTime=0;healId=null;}
  public void TickHealing(float dt){if(HealTime>0)CancelHeal();}
 }
}
