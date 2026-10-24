using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;
namespace GreyHarbor {
 public sealed partial class RaidVerification {
  struct CorpseHit325 {public ItemArea Area;public int Cell;public Rect Rect;}
  List<CorpseHit325> CorpseHits325(RaidUI ui){
   var result=new List<CorpseHit325>();var values=(IEnumerable)ui.GetType().GetField("hits",BindingFlags.Instance|BindingFlags.NonPublic).GetValue(ui);
   foreach(var hit in values){var type=hit.GetType();result.Add(new CorpseHit325{Area=(ItemArea)type.GetField("Area").GetValue(hit),Cell=(int)type.GetField("Cell").GetValue(hit),Rect=(Rect)type.GetField("Rect").GetValue(hit)});}return result;
  }
  void CorpsePointer325(RaidUI ui,EventType type,Vector2 point,int clicks=1){var ev=new Event{button=0,mousePosition=point,clickCount=clicks};ev.type=type;ui.QAInput2758(ev);}
  IEnumerator CorpseGuiWork325(Action work,string label){
   var ui=G.GetComponent<RaidUI>();bool enabled=ui.enabled;ui.enabled=false;var go=new GameObject("CorpsePanel325NativeGUI");var harness=go.AddComponent<InventoryPointerGuiHarness2758>();
   harness.Work=()=>{var matrix=GUI.matrix;try{GUI.matrix=Matrix4x4.identity;Call314(ui,"Setup");work();}finally{GUI.matrix=matrix;}};
   float deadline=Time.realtimeSinceStartup+10;while(!harness.Done&&Time.realtimeSinceStartup<deadline)yield return null;
   ui.enabled=enabled;Destroy(go);Check(harness.Done&&harness.Failure==null,"325 native corpse GUI "+label+(harness.Failure==null?"":" / "+harness.Failure));
  }
  IEnumerator RunCorpsePanel325(){
   var ui=G.GetComponent<RaidUI>();var soldier=G.Map.Loot.LastOrDefault(p=>p.Corpse325!=null&&p.Corpse325.Soldier);var monster=G.Map.Loot.LastOrDefault(p=>p.Corpse325!=null&&!p.Corpse325.Soldier);
   Check(soldier!=null&&monster!=null&&G.Phase==GamePhase.Raid,"325 UI uses real soldier and biological death containers in active raid");
   var body=soldier.Corpse325;var backup=body.Copy();bool previousEnabled=ui.enabled;var biologicalBackup=monster.Items.Select(RaidInventory.Copy).ToList();
   var chestBackup=G.Chest.Snapshot();var bagBackup=G.Bag.Snapshot();int magazine=G.Magazine;
   try{
    G.CloseInventory();G.Paused=false;G.Panel=CampPanel.None;G.MenuCamp315=false;G.Player.transform.position=soldier.Position;G.OpenLoot=soldier;G.InventoryOpen=true;soldier.Opened=true;G.Chest.Slots.Clear();G.Bag.Slots.Clear();ui.enabled=true;
    body.RigColumns=4;body.RigRows=2;body.BagColumns=4;body.BagRows=3;body.RigRecoverable=body.PackRecoverable=true;
    foreach(var list in new[]{body.WeaponItems,body.ArmorItems,body.RigGearItems,body.PackGearItems,body.RigItems,body.BagItems})list.Clear();
    body.WeaponItems.Add(CorpseItem325(ItemKind.AK,1,0,false));body.ArmorItems.Add(CorpseItem325(ItemKind.Armor,1,0,false));body.RigGearItems.Add(CorpseItem325(ItemKind.SmallRig,1,0,false));body.PackGearItems.Add(CorpseItem325(ItemKind.SmallPack,1,0,false));body.RigItems.Add(CorpseItem325(ItemKind.GreenAmmo,20,0,false));body.BagItems.Add(CorpseItem325(ItemKind.Medical,1,0,false));
    var gun=body.WeaponItems[0];gun.MagazineReady314=true;gun.Loaded314=7;G.Magazine=29;
    yield return null;yield return null;yield return null;
    Check(ui.CorpsePanelVisible325,"325 soldier corpse panel is visible through actual raid Inventory OnGUI");
    yield return CorpseGuiWork325(()=>{
     Call314(ui,"Inventory",false);var hits=CorpseHits325(ui);var panel=new Rect(796,98,776,737);var sources=hits.Where(h=>CorpseLoot325.IsArea(h.Area)).ToList();
     Check(CorpseAreas325.All(a=>sources.Any(h=>h.Area==a&&h.Cell==0)),"325 actual corpse layout registers all six real source areas");
     Check(sources.All(h=>h.Rect.width>0&&h.Rect.height>0&&h.Rect.xMin>=panel.xMin&&h.Rect.yMin>=panel.yMin&&h.Rect.xMax<=panel.xMax&&h.Rect.yMax<=panel.yMax),"325 all corpse slots are contained in the right-hand panel");
     Check(sources.Count(h=>h.Area==ItemArea.CorpseRig325)==8&&sources.Count(h=>h.Area==ItemArea.CorpseBag325)==12,"325 corpse content layout draws real 4x2 rig and 4x3 backpack capacities");
     var unknown=sources.First(h=>h.Area==ItemArea.CorpseWeapon325);CorpsePointer325(ui,EventType.MouseDown,unknown.Rect.center);
     Check(!ui.QADragging2758&&G.ItemAt(ItemArea.CorpseWeapon325,0)==null,"325 pointer cannot begin a drag from hidden unidentified corpse weapon");
     Check(RaidUI.CorpseWeaponRounds325(gun)=="7/"+ModRules30.Profile(gun).MagazineSize+" · 弹匣","325 corpse weapon display uses its seven real rounds instead of player magazine or reserves");
    },"unknown layout and source hit bounds");
    foreach(var item in body.AllItems())item.Identified=true;
    yield return CorpseGuiWork325(()=>{
     Call314(ui,"ClearItemUI");Call314(ui,"Inventory",false);var hits=CorpseHits325(ui);var material=body.BagItems[0];var itemHit=hits.First(h=>h.Area==ItemArea.CorpseBag325&&h.Cell==material.Cell);string id=material.Id;
     CorpsePointer325(ui,EventType.MouseDown,itemHit.Rect.center);CorpsePointer325(ui,EventType.MouseUp,itemHit.Rect.center);
     CorpsePointer325(ui,EventType.MouseDown,itemHit.Rect.center,2);
     Check(body.BagItems.Count==0&&G.Bag.Slots.Concat(G.Chest.Slots).Any(p=>p.Id==id),"325 real doubleclick takes a corpse backpack collectible into player storage once");
     Call314(ui,"ClearItemUI");Call314(ui,"Inventory",false);hits=CorpseHits325(ui);var ammo=body.RigItems[0];id=ammo.Id;var from=hits.First(h=>h.Area==ItemArea.CorpseRig325&&h.Cell==ammo.Cell);var to=hits.First(h=>h.Area==ItemArea.Rig&&h.Cell==0);
     CorpsePointer325(ui,EventType.MouseDown,from.Rect.center);Check(ui.QADragging2758&&ui.QADragItem2758.Id==id,"325 real pointer starts drag from corpse rig content");CorpsePointer325(ui,EventType.MouseUp,to.Rect.center);
     Check(!ui.QADragging2758&&body.RigItems.Count==0&&G.Chest.Count(ItemKind.GreenAmmo)==20,"325 real corpse drag commits to player rig without corpse duplication");
    },"doubleclick and drag pickup");
    yield return null;yield return NativeInventoryPhoto2758("325-corpse-soldier-ui-reference.png");
    G.CloseInventory();G.Player.transform.position=monster.Position;G.OpenLoot=monster;G.InventoryOpen=true;monster.Opened=true;foreach(var item in monster.Items)item.Identified=false;
    yield return null;yield return null;
    Check(ui.CorpsePanelVisible325,"325 biological corpse panel is visible through actual raid inventory");
    yield return CorpseGuiWork325(()=>{
     Call314(ui,"Inventory",false);var hits=CorpseHits325(ui);var source=hits.Where(h=>h.Area==ItemArea.Loot).ToList();var inventory=G.EnsureLootGrid(monster);
     Check(source.Count==monster.Corpse325.BioColumns*monster.Corpse325.BioRows&&inventory.Slots==monster.Items&&!hits.Any(h=>CorpseLoot325.IsArea(h.Area)),"325 actual biological layout uses only its fixed real material grid and no soldier equipment slots");
     Check(source.All(h=>h.Rect.xMin>=796&&h.Rect.xMax<=1572&&h.Rect.yMin>=98&&h.Rect.yMax<=835),"325 biological source slots stay inside right-hand panel");
     var material=monster.Items.FirstOrDefault();Check(material!=null&&G.ItemAt(ItemArea.Loot,material.Cell)==null,"325 unknown biological material remains concealed and cannot be taken");
    },"biological layout and identification protection");
    yield return null;yield return NativeInventoryPhoto2758("325-corpse-biological-ui-reference.png");results.Add("INFO corpse UI captures are visual references; pixel appearance requires manual review and a black hidden-window capture is not visual acceptance");
   }finally{
    body.Restore(backup);body.RigColumns=backup.RigColumns;body.RigRows=backup.RigRows;body.BagColumns=backup.BagColumns;body.BagRows=backup.BagRows;body.RigRecoverable=backup.RigRecoverable;body.PackRecoverable=backup.PackRecoverable;
    monster.Items.Clear();monster.Items.AddRange(biologicalBackup);G.Chest.Restore(chestBackup);G.Bag.Restore(bagBackup);G.Magazine=magazine;G.CloseInventory();Call314(ui,"ClearItemUI");ui.enabled=previousEnabled;
   }
  }
 }
}
