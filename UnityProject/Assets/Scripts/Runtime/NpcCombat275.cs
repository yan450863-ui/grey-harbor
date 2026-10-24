using UnityEngine;
namespace GreyHarbor {
 public static class NpcCombat275 {
  // NPC endurance is split once at creation using a fixed same-level AK reference.
  public const float ReferenceDamage=18,ReferencePenetration=10;
  public static int BaselineHits(float oldTotalHealth){int old=Mathf.CeilToInt(Mathf.Max(0,oldTotalHealth)/ReferenceDamage);int reduced=old-(old<=8?2:old<=10?3:4);return Mathf.Min(old,Mathf.Max(4,reduced));}
  public static int ArmorHits(float oldTotalHealth){return Mathf.FloorToInt(BaselineHits(oldTotalHealth)*.4f);}
  public static float ArmorForBudget(float oldTotalHealth){return ArmorHits(oldTotalHealth)*ReferencePenetration;}
  public static float HealthForBudget(float oldTotalHealth){return (BaselineHits(oldTotalHealth)-ArmorHits(oldTotalHealth))*ReferenceDamage;}
  public static void Initialize(RaidActor actor,float oldTotalHealth){
   actor.NpcMaxArmor=ArmorForBudget(oldTotalHealth);actor.NpcArmorPoints=actor.NpcMaxArmor;
   actor.NpcMaxHealth=HealthForBudget(oldTotalHealth);actor.Health=actor.NpcMaxHealth;
  }
  public static void InitializeSoldier(RaidActor actor,RebelKind kind){
   actor.NpcLevel=NpcRules270.Level(kind);actor.NpcWeapon=SoldierProfiles2745.Weapon(kind);actor.NpcAmmo=SoldierProfiles2745.Ammo(kind);
   Initialize(actor,NpcRules270.OldHealthBudget(kind));
  }
  public static void ApplyHit(RaidActor target,float damage,float penetration,int ammoGrade){
   if(!target||target.Dead)return;
   float multiplier=ArmorBalance.Multiplier(ammoGrade,Mathf.Max(0,target.NpcLevel-1));
   if(target.NpcArmorPoints>0){
    target.NpcArmorPoints=Mathf.Max(0,target.NpcArmorPoints-Mathf.Max(0,penetration)*multiplier);
    // The armor-breaking round is absorbed completely, including its visual hit response.
    target.Damage(0);return;
   }
   target.Damage(Mathf.Max(0,damage)*multiplier);
  }
  // NPC carried guns are cosmetic; attacks retain their own fixed combat values.
  public static int AttackGrade345(RaidActor actor)=>actor?Mathf.Clamp(actor.NpcLevel-1,0,1):0;
  public static float IndependentDamage(RaidActor actor){return actor&&actor.IsMonster?MonsterTuning.Damage(actor.Monster):actor&&actor.IsRebel?SoldierProfiles2745.Damage(actor.Rebel):14;}
  public static float IndependentPenetration(RaidActor actor){return actor&&actor.IsMonster?MonsterTuning.Damage(actor.Monster):10;}
 }
}

