using System.Collections;
using UnityEngine;
namespace GreyHarbor {
 // This session is command-line-only and uses the same disposable inventory as other previews.
 public sealed class SoldierPreview2745:MonoBehaviour {
  IEnumerator Start(){
   var g=RaidGame.Instance;g.HarborWalkthrough=false;yield return g.BeginTrainingLoad2721();
   if(!g.TrainingActive||g.IsLoading2721){Destroy(this);yield break;}
   Apply(g);Destroy(this);
  }
  public static void Apply(RaidGame g){
   foreach(var e in g.Enemies){e.gameObject.SetActive(false);Destroy(e.gameObject);}g.Enemies.Clear();
   if(g.ArmorDummy){g.ArmorDummy.gameObject.SetActive(false);Destroy(g.ArmorDummy.gameObject);g.ArmorDummy=null;}
   var kinds=new[]{RebelKind.Smg,RebelKind.Marksman,RebelKind.BlueSmg,RebelKind.BlueMarksman};
   var positions=new[]{new Vector3(-7,0,-6),new Vector3(-7,0,3),new Vector3(5,0,-7),new Vector3(6,0,6)};
   for(int i=0;i<kinds.Length;i++){var e=g.SpawnRebel145(positions[i],kinds[i],"新士兵独立试验");e.Home=positions[i];e.AttackCooldown=4;if(SoldierProfiles2745.IsSmg(kinds[i]))e.PatrolTargets=new[]{positions[i],positions[i]+Vector3.back*1.5f};}
   g.Chest=new RaidInventory(10);g.Chest.Add(ItemKind.GreenAmmo,170);g.Chest.Add(ItemKind.Ammo,170);g.Chest.Add(ItemKind.GreenArmorKit,1);g.Bag=new RaidInventory(20);g.Bag.Add(ItemKind.ArmorKit,4);g.Bag.Add(ItemKind.Green9mm,170);g.Bag.Add(ItemKind.Blue9mm,170);g.SecondWeapon=new ItemStack(ItemKind.MP5,1);
   g.Player.transform.position=new Vector3(0,0,-13);g.Player.Aim=Vector3.forward;g.Rig.ResetView(0);g.Rig.Tick(0);Physics.SyncTransforms();g.Vision.Refresh(g.Player.transform.position,g.Player.Aim);g.Player.Draw(g.View,0);
   g.Notify("新士兵独立测试：绿蓝MP5兵与SKS兵各一只。训练结束恢复装备；不影响正式存档。");
  }
 }
}
