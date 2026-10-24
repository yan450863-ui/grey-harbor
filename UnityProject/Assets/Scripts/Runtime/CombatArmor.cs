using UnityEngine;
namespace GreyHarbor {
 public sealed partial class RaidGame {
  public RaidActor ArmorDummy;public int DummyHits;float dummyReset;
  public float TotalArmor {get{ArmorBalance.Ensure(Armor);return Armor?.ArmorPoints??0;}}
  public float MaxArmor=>ArmorBalance.Max(Armor?.Kind??ItemKind.Cola);
  public void ApplyCombatHit(RaidActor target,float damage,float penetration,int weaponGrade=1,int ammoGrade=1){
   if(target==null||OptionalFeatures2761.BlocksDamage(target))return;
   ItemStack protection=null;if(target==Player){if(damage>0||penetration>0)MarkPlayerCombat321();ArmorBalance.Ensure(Armor);protection=Armor;}
   else if(IsTrainingDummy315(target))protection=target.TestArmor;
   if(target.Enemy&&!IsTrainingDummy315(target)){NpcCombat275.ApplyHit(target,damage,penetration,ammoGrade);return;}
   float healthDamage=ArmorBalance.Hit(protection,damage,penetration,ammoGrade);target.Damage(healthDamage);
   if(target==Player){HurtFlash=.3f;CancelArmorRepair();}
   HitTrainingDummy315(target);
   if(target==ArmorDummy){DummyHits++;if(target.Dead)dummyReset=4;}
  }
  void CreateArmorDummy(){ArmorDummy=Actor("ArmorTestTarget",new Vector3(3,0,-12),true);ArmorDummy.Health=ArmorDummy.NpcMaxHealth=100;ArmorDummy.NpcArmorPoints=ArmorDummy.NpcMaxArmor=0;ArmorDummy.TestArmor=new ItemStack(ItemKind.BlueArmor,1);ArmorDummy.Gun.enabled=false;DummyHits=0;dummyReset=0;}
  void DrawArmorDummy(float dt){if(!ArmorDummy||!ArmorDummy.gameObject.activeInHierarchy)return;if(ArmorDummy.Dead&&!Paused){dummyReset-=dt;if(dummyReset<=0){ArmorDummy.Health=100;ArmorDummy.Dead=false;ArmorDummy.HitCollider.enabled=true;ArmorDummy.TestArmor=new ItemStack(ItemKind.BlueArmor,1);DummyHits=0;}}
   ArmorDummy.VisibleToPlayer=Vision.CanSee(ArmorDummy.transform.position);ArmorDummy.Draw(View,dt);ArmorDummy.Gun.enabled=ArmorDummy.Muzzle.enabled=false;
  }
 }
 public sealed partial class RaidUI {
  void ArmorHUD(){
   if(G.Phase!=GamePhase.Raid)return;Fill(new Rect(28,644,380,90),panel);Label(47,654,340,24,$"总护甲  {G.TotalArmor:0.#} / {G.MaxArmor:0}",20,mint);Fill(new Rect(50,689,335,7),new Color(.15f,.23f,.23f));Fill(new Rect(50,689,335*(G.MaxArmor>0?Mathf.Clamp01(G.TotalArmor/G.MaxArmor):0),7),PixelArt.Hex("719CCB"));Label(47,706,340,20,WeaponHudDamage279(G.Equipped)+(G.Equipped==null?"":" · "+RaidInventory.GradeName(G.MagazineAmmo)+"弹")+" · "+(G.ActiveHardMode?"困难掉落":"普通掉落"),13,muted);
   var d=G.ArmorDummy;if(!d||!d.gameObject.activeInHierarchy||!d.VisibleToPlayer||Vector3.Distance(G.Player.transform.position,d.transform.position)>8)return;var p=ScreenPoint(d.transform.position+Vector3.up*2);Fill(new Rect(p.x-143,p.y-71,286,68),panel);Label(p.x-137,p.y-66,274,24,$"试射靶 · 已命中 {G.DummyHits} 发",18,mint,TextAnchor.MiddleCenter);Label(p.x-137,p.y-37,274,24,d.Dead?"击倒 · 4秒后复位":$"护甲 {d.TestArmor.ArmorPoints:0}/120  生命 {d.Health:0}/100",16,white,TextAnchor.MiddleCenter);
  }
 }
}
