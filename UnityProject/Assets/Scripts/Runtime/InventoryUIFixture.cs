using System.Collections;
using UnityEngine;
namespace GreyHarbor {
 // Only attached by the local QA flag; all saves use the isolated UI-check path.
 public sealed class InventoryUIFixture:MonoBehaviour {
  IEnumerator Start(){
   var g=RaidGame.Instance;
   if(!g.SavePath.EndsWith("ui-check-stash-1.10.json")){Destroy(this);yield break;}
   g.NextMap=RaidDestination.Training;if(!g.StartRaid()){Destroy(this);yield break;}
   foreach(var enemy in g.Enemies){enemy.Dead=true;enemy.HitCollider.enabled=false;}
   var box=g.Map.AddLoot(g.Player.transform.position+Vector3.forward*.6f,"军用物资箱",ItemKind.Ammo,20,ItemKind.GreenArmorKit,1,false);
   box.Items.Add(new ItemStack(ItemKind.Circuit,1,ItemOrigin.Certified));box.SearchDuration=6;foreach(var p in box.Items){p.Identified=false;p.RaidFound=true;}g.EnsureLootGrid(box);
   var input=new TouchInput{Searching=true};g.Controls=input;
   while(!box.Opened){input.AimPosition=g.View.WorldToScreenPoint(box.Position+Vector3.up*RaidGame.CombatHeight);yield return null;}
   g.Controls=new DesktopInput();Destroy(this);
  }
 }
}
