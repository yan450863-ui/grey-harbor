using System;
namespace GreyHarbor {
 public static partial class CampRules {
  static void NormalizeMagazine279(BaseSave s){
   if(!AmmoRules270.IsAmmo(s.CampMagazineAmmo))throw new InvalidOperationException("未知弹匣弹药");
   if(s.CampMagazine<0||s.CampMagazineFound<0||s.CampMagazineFound>s.CampMagazine)throw new InvalidOperationException("弹匣数量无效");
   int capacity=s.Weapon==null?0:ModRules30.Profile(s.Weapon).MagazineSize;
   bool incompatible=s.Weapon==null||!WeaponRules279.Compatible(s.Weapon.Kind,s.CampMagazineAmmo);
   int kept=incompatible?0:Math.Min(capacity,s.CampMagazine),returned=s.CampMagazine-kept;
   if(returned>0){int certified=Math.Min(returned,s.CampMagazineFound);DepositGrowing(s,s.CampMagazineAmmo,returned-certified,false);DepositGrowing(s,s.CampMagazineAmmo,certified,true);s.CampMagazineFound-=certified;}
   s.CampMagazine=kept;
   if(kept==0&&s.Weapon!=null&&incompatible)s.CampMagazineAmmo=WeaponRules279.GreenAmmoFor(s.Weapon.Kind);
  }
 }
}
