namespace GreyHarbor {
 public enum AmmoCaliber279 { None, Seven62, NineMM, Five56, Gauge12 }
 public sealed class WeaponProfile279 {
  public WeaponProfile279 Copy30()=>(WeaponProfile279)MemberwiseClone();
  public ItemKind Kind; public string Name,CaliberLabel; public AmmoCaliber279 Caliber;
  public bool Automatic; public int MagazineSize,MovementStability,SellPrice;
  public float Damage,Penetration,Interval,ReloadSeconds,Range,ProjectileSpeed,FirstSpread,HeatPerShot,MaxHeat,FirstKick,KickGrowth,MaxKickGrowth,MaxRecoil,RecoveryDelay,RecoilRecovery,HeatRecovery;
 }
 public static class WeaponRules279 {
  public static readonly WeaponProfile279 AK=new WeaponProfile279{Kind=ItemKind.AK,Name="AK",CaliberLabel="7.62 × 39",Caliber=AmmoCaliber279.Seven62,Automatic=true,MagazineSize=30,MovementStability=60,SellPrice=1040,Damage=18,Penetration=10,Interval=.11f,ReloadSeconds=2.1f,Range=9,ProjectileSpeed=48,FirstSpread=1.2f,HeatPerShot=.22f,MaxHeat=3.75f,FirstKick=.85f,KickGrowth=.08f,MaxKickGrowth=.55f,MaxRecoil=16,RecoveryDelay=.20f,RecoilRecovery=12,HeatRecovery=2.8f};
  public static readonly WeaponProfile279 MP5=new WeaponProfile279{Kind=ItemKind.MP5,Name="MP5",CaliberLabel="9 × 19",Caliber=AmmoCaliber279.NineMM,Automatic=true,MagazineSize=30,MovementStability=68,SellPrice=780,Damage=12,Penetration=6,Interval=.095f,ReloadSeconds=2,Range=8,ProjectileSpeed=40,FirstSpread=1.9f,HeatPerShot=.12f,MaxHeat=1.4f,FirstKick=.55f,KickGrowth=.015f,MaxKickGrowth=.10f,MaxRecoil=10,RecoveryDelay=.15f,RecoilRecovery=12,HeatRecovery=2};
  public static readonly WeaponProfile279 SKS=new WeaponProfile279{Kind=ItemKind.SKS,Name="SKS",CaliberLabel="7.62 × 39",Caliber=AmmoCaliber279.Seven62,Automatic=false,MagazineSize=20,MovementStability=30,SellPrice=980,Damage=28,Penetration=10,Interval=.34f,ReloadSeconds=2.4f,Range=56f/3f,ProjectileSpeed=72,FirstSpread=.35f,HeatPerShot=.25f,MaxHeat=.85f,FirstKick=1.1f,KickGrowth=0,MaxKickGrowth=0,MaxRecoil=4,RecoveryDelay=.16f,RecoilRecovery=10,HeatRecovery=1.2f};
  public static WeaponProfile279 Get(ItemKind k)=>BlueWeapons30.IsGun(k)?BlueWeapons30.Get(k):k==ItemKind.AK?AK:k==ItemKind.MP5?MP5:k==ItemKind.SKS?SKS:null;
  public static bool IsWeapon(ItemKind k)=>Get(k)!=null;
  public static AmmoCaliber279 AmmoCaliber(ItemKind k)=>k==ItemKind.Green556||k==ItemKind.Blue556?AmmoCaliber279.Five56:k==ItemKind.Green12Gauge||k==ItemKind.Blue12Gauge?AmmoCaliber279.Gauge12:k==ItemKind.Ammo||k==ItemKind.GreenAmmo?AmmoCaliber279.Seven62:k==ItemKind.Green9mm||k==ItemKind.Blue9mm?AmmoCaliber279.NineMM:AmmoCaliber279.None;
  public static bool Compatible(ItemKind weapon,ItemKind ammo)=>Get(weapon)!=null&&AmmoCaliber(ammo)==Get(weapon).Caliber;
  public static ItemKind GreenAmmoFor(ItemKind weapon)=>Get(weapon)?.Caliber==AmmoCaliber279.Five56?ItemKind.Green556:Get(weapon)?.Caliber==AmmoCaliber279.Gauge12?ItemKind.Green12Gauge:Get(weapon)?.Caliber==AmmoCaliber279.NineMM?ItemKind.Green9mm:ItemKind.GreenAmmo;
  public static ItemKind PickWeapon(System.Random r){int n=r.Next(100);return n<40?ItemKind.AK:n<75?ItemKind.MP5:ItemKind.SKS;}
  public static ItemKind PickAmmo(System.Random r,int grade){int n=r.Next(100);return n<40?(grade>0?ItemKind.Ammo:ItemKind.GreenAmmo):n<70?(grade>0?ItemKind.Blue9mm:ItemKind.Green9mm):n<90?(grade>0?ItemKind.Blue556:ItemKind.Green556):(grade>0?ItemKind.Blue12Gauge:ItemKind.Green12Gauge);}
  // New variants divide one old family weight; they never add another family roll.
  public static double DropWeight(ItemKind k)=>k==ItemKind.ReinforcedArmor320?.6:k==ItemKind.HeavyArmor320?.35:ModRules30.IsMod(k)?(ModRules30.Get(k).Grade==0?.25/6:.25/7):k==ItemKind.AR16?.40:k==ItemKind.Dagger?.35:k==ItemKind.SG12?.25:k==ItemKind.Green556||k==ItemKind.Blue556?.20:k==ItemKind.Green12Gauge||k==ItemKind.Blue12Gauge?.10:k==ItemKind.AK?.40:k==ItemKind.MP5?.35:k==ItemKind.SKS?.25:k==ItemKind.Ammo||k==ItemKind.GreenAmmo?.40:k==ItemKind.Blue9mm||k==ItemKind.Green9mm?.30:1;
 }
}
