using System.Collections.Generic;using UnityEngine;
namespace GreyHarbor {public sealed partial class RaidGame {
 public const float DropMergeRadius314=1.5f;
 readonly List<LootContainer> dropViewsPending314=new List<LootContainer>();
 void BeginDropMutation314(){dropViewsPending314.Clear();}
 void CommitDropViews314(){foreach(var box in dropViewsPending314)if(box.View)Destroy(box.View);dropViewsPending314.Clear();}
 void DropToPile314(ItemStack item){
  Vector3 position=Player.transform.position;var nearby=new List<LootContainer>();foreach(var box in Map.Loot)if(box.PlayerDrop314&&Mathf.Abs(position.y-box.Position.y)<.8f&&Vector3.Distance(position,box.Position)<=DropMergeRadius314&&ClearSight(position,box.Position))nearby.Add(box);
  nearby.Sort((a,b)=>Vector3.Distance(position,a.Position).CompareTo(Vector3.Distance(position,b.Position)));LootContainer pile;
  if(nearby.Count==0){pile=Map.AddLoot(position,"丢弃物资",ItemKind.Scrap,0,ItemKind.Scrap,0,false);pile.Items.Clear();pile.PlayerDrop314=true;}else pile=nearby[0];
  for(int i=1;i<nearby.Count;i++){var old=nearby[i];foreach(var p in old.Items){var copy=RaidInventory.Copy(p);copy.Cell=-1;pile.Items.Add(copy);}Map.Loot.Remove(old);if(OpenLoot==old)OpenLoot=pile;dropViewsPending314.Add(old);}
  var dropped=RaidInventory.Copy(item);dropped.Cell=-1;pile.Items.Add(dropped);pile.Opened=pile.Known=true;pile.Search=pile.SearchDuration;EnsureLootGrid(pile);
 }
}}
