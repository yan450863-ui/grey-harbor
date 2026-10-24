using UnityEngine;
namespace GreyHarbor {
 public sealed partial class RaidVerification {
  void VerifyInventorySearch(){
   StandardRaid();G.Paused=false;G.InventoryOpen=false;G.ReloadTime=G.HealTime=0;
   foreach(var enemy in G.Enemies){enemy.Dead=true;enemy.HitCollider.enabled=false;}
   var origin=new Vector3(-6,0,-11);G.Player.transform.position=origin;G.Player.Aim=Vector3.forward;
   var box=G.Map.AddLoot(origin+Vector3.forward*.6f,"搜索验证箱",ItemKind.Ammo,12,ItemKind.Cola,2,false);box.SearchDuration=2;
   var input=new TouchInput{Searching=true};G.Controls=input;G.Vision.Refresh(origin,Vector3.forward);
   var ui=G.GetComponent<RaidUI>();G.TickInteraction(.5f);
   Check(G.Nearby==box&&ui.SearchVisible&&!box.Opened&&!G.InventoryOpen&&RaidUI.ConcealLoot(box),"Search shows concealed read-only contents before completion");
   int ammo=G.Chest.Count(ItemKind.Ammo);G.OpenLoot=box;
   Check(!G.QuickTransfer(ItemArea.Loot,0)&&box.Items[0].Count==12&&G.Chest.Count(ItemKind.Ammo)==ammo,"Incomplete search cannot transfer hidden loot");G.OpenLoot=null;
   input.Searching=false;G.TickInteraction(.1f);
   Check(box.Search==0&&!ui.SearchVisible&&!box.Opened,"Releasing search cancels progress and hides search panel");
   input.Searching=true;G.TickInteraction(1.9f);Check(ui.SearchVisible&&!box.Opened,"Restarted search must complete its full duration");
   G.TickInteraction(.11f);Check(box.Opened&&G.OpenLoot==box&&G.InventoryOpen&&!ui.SearchVisible&&!RaidUI.ConcealLoot(box),"Completed search reveals normal transferable container UI");
   Check(G.QuickTransfer(ItemArea.Loot,0)&&G.Chest.Count(ItemKind.Ammo)==ammo+12&&box.Items.Count==1,"Revealed ammunition quick-transfers to chest rig without loss");
   G.Paused=true;
  }
 }
}
