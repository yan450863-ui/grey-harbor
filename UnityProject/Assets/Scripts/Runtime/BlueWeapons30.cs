using UnityEngine;
namespace GreyHarbor {
 public static class BlueWeapons30 {
  public static bool IsGun(ItemKind k)=>k>=ItemKind.AR16&&k<=ItemKind.SG12;
  public static bool IsNewAmmo(ItemKind k)=>k>=ItemKind.Green556&&k<=ItemKind.Blue12Gauge;
  public static bool IsNew(ItemKind k)=>IsGun(k)||IsNewAmmo(k);
  public static int Grade(ItemKind k)=>k==ItemKind.Green556||k==ItemKind.Green12Gauge?0:1;
  public static int AmmoPrice(ItemKind k)=>k==ItemKind.Green556?15:k==ItemKind.Blue556?20:k==ItemKind.Green12Gauge?30:40;
  public static readonly WeaponProfile279 AR=new WeaponProfile279{Kind=ItemKind.AR16,Name="AR-16",CaliberLabel="5.56 × 45",Caliber=AmmoCaliber279.Five56,Automatic=true,MagazineSize=30,MovementStability=75,SellPrice=2100,Damage=16,Penetration=9,Interval=.09f,ReloadSeconds=2.2f,Range=9.5f,ProjectileSpeed=54,FirstSpread=.9f,HeatPerShot=.15f,MaxHeat=2.6f,FirstKick=.60f,KickGrowth=.04f,MaxKickGrowth=.25f,MaxRecoil=12,RecoveryDelay=.18f,RecoilRecovery=13,HeatRecovery=3};
  public static readonly WeaponProfile279 SMG=new WeaponProfile279{Kind=ItemKind.Dagger,Name="短剑",CaliberLabel="9 × 19",Caliber=AmmoCaliber279.NineMM,Automatic=true,MagazineSize=40,MovementStability=65,SellPrice=1700,Damage=12,Penetration=8,Interval=.065f,ReloadSeconds=2.3f,Range=6,ProjectileSpeed=44,FirstSpread=1.75f,HeatPerShot=.11f,MaxHeat=1.35f,FirstKick=.65f,KickGrowth=.02f,MaxKickGrowth=.12f,MaxRecoil=9,RecoveryDelay=.16f,RecoilRecovery=13,HeatRecovery=2.4f};
  public static readonly WeaponProfile279 Shotgun=new WeaponProfile279{Kind=ItemKind.SG12,Name="SG-12",CaliberLabel="12号",Caliber=AmmoCaliber279.Gauge12,Automatic=false,MagazineSize=8,MovementStability=50,SellPrice=1800,Damage=68,Penetration=48,Interval=.42f,ReloadSeconds=2.8f,Range=5,ProjectileSpeed=48,FirstSpread=1.2f,HeatPerShot=.2f,MaxHeat=1,FirstKick=2.4f,KickGrowth=0,MaxKickGrowth=0,MaxRecoil=8,RecoveryDelay=.25f,RecoilRecovery=8,HeatRecovery=2};
  public static WeaponProfile279 Get(ItemKind k)=>k==ItemKind.AR16?AR:k==ItemKind.Dagger?SMG:Shotgun;
  public static string Name(ItemKind k)=>IsGun(k)?Get(k).Name+(k==ItemKind.Dagger?"冲锋枪":k==ItemKind.SG12?"霰弹枪":"步枪"):(Grade(k)==0?"绿色":"蓝色")+(k==ItemKind.Green556||k==ItemKind.Blue556?"5.56毫米弹药":"12号霰弹");
  public static string Description(ItemKind k){if(IsNewAmmo(k))return (k==ItemKind.Green556||k==ItemKind.Blue556?"AR-16使用的5.56×45弹药。":"SG-12使用的12号霰弹，每发8颗弹丸。")+"攻击等级"+(Grade(k)+1)+"；随身"+(WeaponRules279.AmmoCaliber(k)==AmmoCaliber279.Gauge12?50:100)+"发/格，仓库300发/格；R调用胸挂弹药。";var p=Get(k);return $"蓝色{p.Name} · {p.CaliberLabel}；伤害{p.Damage}、穿甲{p.Penetration}；{p.MagazineSize}发弹匣，换弹{p.ReloadSeconds:0.#}秒；射程{p.Range:0.#}米。右键枪械进行改装。"+(k==ItemKind.SG12?"整发弹丸合并结算，破甲该发不伤血；数值为8颗全命中的上限。":"");}
 }
}