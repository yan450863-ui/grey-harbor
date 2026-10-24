using System;using System.Collections;using System.Collections.Generic;using System.Linq;using UnityEngine;
namespace GreyHarbor {public sealed partial class RaidVerification {
 IEnumerator RunCorpseLayout325(){
  FreshCamp();G.enabled=false;G.Chest.Slots.Clear();G.Bag.Slots.Clear();G.Safe.Slots.Clear();Check(G.StartRaid(),"325 fixed-slot test isolated raid starts");G.enabled=false;G.Paused=false;yield return null;
  EmptyCorpseCarry325();
  var bio=new LootContainer{Position=G.Player.transform.position,Opened=true,Corpse325=new CorpseLoot325{BioColumns=3,BioRows=3}};
  ItemStack a=CorpseItem325(ItemKind.XenoScale,1,0),b=CorpseItem325(ItemKind.Chitin,1,4),missing=CorpseItem325(ItemKind.AlienGland,1,-1);
  bio.Items.AddRange(new[]{a,b,missing});G.Map.Loot.Add(bio);G.OpenLoot=bio;
  G.EnsureLootGrid(bio);
  Check(a.Cell==0&&b.Cell==4&&bio.Items.Find(p=>p.Id==b.Id).Cell==4,"325 repairing one unset biological cell preserves all valid original cells");
  Check(missing.Cell>=0&&missing.Cell!=0&&missing.Cell!=4,"325 unset biological item uses only a free cell");
  string survivor=b.Id;
  Check(G.QuickTransfer(ItemArea.Loot,a.Cell),"325 biological first item can be taken");
  for(int i=0;i<10;i++){var grid=G.EnsureLootGrid(bio);Check(grid.At(0)==null&&grid.At(4)?.Id==survivor&&b.Cell==4,"325 biological empty cell stays empty on repeated reads "+i);}
  G.CloseInventory();G.OpenLoot=bio;G.InventoryOpen=true;G.TickIdentification(10);
  Check(G.EnsureLootGrid(bio).At(0)==null&&G.EnsureLootGrid(bio).At(4)?.Id==survivor,"325 biological close reopen and identification preserve vacant cells");
  var box=CorpseFixture325();var body=box.Corpse325;
  var rigLater=CorpseItem325(ItemKind.Blue9mm,15,5,false);body.RigItems.Add(rigLater);
  var umbrella=CorpseItem325(ItemKind.FoldingUmbrella,1,5,false);umbrella.Rotated=true;body.BagItems.Add(umbrella);
  Check(G.MoveItem(ItemArea.CorpseRig325,0,ItemArea.Rig,-1,5)&&body.RigItems[0].Cell==0&&body.RigItems[0].Count==15&&rigLater.Cell==5,"325 partial ammo pickup keeps both corpse rig stack positions");
  Check(G.QuickTransfer(ItemArea.CorpseRig325,0)&&body.RigItems.Count==1&&rigLater.Cell==5,"325 complete ammo pickup leaves rig cell0 vacant");
  Check(G.QuickTransfer(ItemArea.CorpseBag325,0)&&body.BagItems.Count==1&&umbrella.Cell==5&&umbrella.Rotated,"325 backpack pickup leaves gap and preserves later rotated item");
  for(int i=0;i<8;i++){G.TickIdentification(2);Check(G.InventoryFor(ItemArea.CorpseRig325).At(0)==null&&G.InventoryFor(ItemArea.CorpseRig325).At(5)?.Id==rigLater.Id&&G.InventoryFor(ItemArea.CorpseBag325).At(0)==null&&G.InventoryFor(ItemArea.CorpseBag325).At(5)?.Id==umbrella.Id,"325 repeated source reads and identification do not compact soldier contents "+i);}
  G.CloseInventory();G.OpenLoot=box;G.InventoryOpen=true;
  Check(rigLater.Cell==5&&umbrella.Cell==5&&umbrella.Rotated,"325 reopening soldier corpse preserves original cell and orientation");
  // Hidden windows can suppress Repaint; the fixed-cell regression checks actual inventory references.
  results.Add("INFO fixed-slot regression uses actual pickup, source reads, identification and reopen; native GUI was unavailable in the hidden-window probe.");
  CorpseIdentificationChecks325();CorpseTransferChecks325();CorpseTransactionChecks325();CorpseWeaponChecks325();CorpseGeneratedChecks325();CorpseDeathChecks325();
  G.CloseInventory();G.Finish(false);Check(G.Phase==GamePhase.Base,"325 fixed-slot verification returns to camp");results.Add("CORPSE FIXED SLOTS325 COMPLETE");yield return null;
 }
}}
