namespace GreyHarbor {
 // Player range is measured from the actual muzzle, independent of flashlight and lifetime.
 public static class WeaponRange278 {
  public const float Ak=9,MP5Draft=8,SKSDraft=56f/3f;
  public static float Maximum(ItemKind weapon)=>WeaponRules279.Get(weapon)?.Range??0;
 }
}
