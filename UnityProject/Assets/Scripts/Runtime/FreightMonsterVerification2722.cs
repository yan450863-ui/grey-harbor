using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace GreyHarbor {
 public sealed partial class RaidVerification {
  sealed class FreightGroup2722 {
   public RaidMap Map;public RaidActor[] Actors;public List<FreightMonsterCache2720> States=new List<FreightMonsterCache2720>();public LootContainer Corpse;
  }
  void FreightRoster2722(bool indoor){
   var plans=G.Map.FreightEnemies2720;Check(plans.Count==(indoor?4:7),"2722 exact freight roster "+(G.Map.InteriorId??"outdoor"));
   Check(indoor?G.Enemies.Count==4&&G.Map.Loot.Count==6:G.Enemies.Count==30&&G.Map.FreightSearchRecords2716.Count==4,"2722 added population preserves containers and other outdoor rosters");
   var unique=new HashSet<RaidActor>();
   foreach(var p in plans){
    var e=p.Actor;Check(e&&unique.Add(e)&&G.Enemies.Contains(e)&&e.gameObject.activeInHierarchy&&e.IsMonster&&!e.Dead&&e.Monster==p.Kind&&e.NpcLevel==p.Level&&Vector3.Distance(e.Home,p.Home)<.001f,"2722 live production actor matches plan "+p.Name);
    Check(e.NpcArmorPoints==e.NpcMaxArmor&&e.Health==e.NpcMaxHealth&&e.NpcAmmo==(p.Level==2?ItemKind.Ammo:ItemKind.GreenAmmo),"2722 unchanged level armor health and ammunition "+p.Name);
    Check(FreightMonsterClear2720(p.Home)&&p.Targets.Length==3,"2722 actual home has clear capsule "+p.Name);
    foreach(var door in G.Map.Doors)Check(Vector3.Distance(p.Home,door.Position)>1.4f,"2722 doorway remains clear "+p.Name+" / "+door.Name);
    if(indoor){foreach(var r in G.Map.FreightLootRecords2717)Check(Vector3.Distance(p.Home,r.Stand)>=1.5f,"2722 search stand clear "+p.Name+" / "+r.Container.Name);}
    else foreach(var r in G.Map.FreightSearchRecords2716)Check(Vector3.Distance(p.Home,r.Stand)>=1.5f,"2722 outdoor search stand clear "+p.Name);
    for(int i=0;i<p.Targets.Length;i++)Check(FreightMonsterClear2720(p.Targets[i])&&G.Navigation.ClearSegment(p.Targets[i],p.Targets[(i+1)%3]),"2722 actual patrol edge clear "+p.Name+" / "+i);
    FreightMonsterRow2720("spawn-2722",e,"room="+p.RoomName+" level="+p.Level+" armor="+e.NpcArmorPoints+" HP="+e.Health);
   }
  }
  FreightGroup2722 FreightGroupTake2722(LootContainer corpse){
   var s=new FreightGroup2722{Map=G.Map,Actors=G.Enemies.ToArray(),Corpse=corpse};foreach(var actor in s.Actors)s.States.Add(FreightMonsterCacheTake2720(actor,actor.Dead?corpse:null));return s;
  }
  void FreightGroupCheck2722(FreightGroup2722 s){
   Check(G.Map==s.Map&&G.Enemies.Count==4&&Array.TrueForAll(s.Actors,e=>G.Enemies.Contains(e)),"2722 cached building restores original four actors without duplicate spawn");
   foreach(var state in s.States){var e=state.Actor;Check(e.gameObject.activeInHierarchy&&e.Dead==state.Dead&&Vector3.Distance(e.transform.position,state.Position)<.001f&&CombatNear275(e.Health,state.Health)&&CombatNear275(e.NpcArmorPoints,state.Armor)&&e.PatrolIndex==state.PatrolIndex,"2722 per-actor position armor health death and patrol retained "+e.name);}
   var first=s.States[0];for(int i=0;i<first.Boxes.Length;i++)Check(G.Map.FreightLootRecords2717[i].Container==first.Boxes[i]&&first.BoxItems[i]==FreightMonsterItems2720(first.Boxes[i]),"2722 original box identity and contents preserved "+first.Boxes[i].Name);
   Check(G.Map.Loot.Count==7&&G.Map.Loot.Contains(s.Corpse)&&s.Corpse.Opened&&s.Corpse.Items.TrueForAll(i=>i.Identified),"2722 searched biological corpse retained without reroll or respawn");
  }
  IEnumerator RunFreightMonsters2722(){
   Check(Application.version=="2.7.22"&&G.VerifyMode&&G.SavePath.EndsWith("verification-stash-2.7.22.json"),"2722 isolated current-version save");
   string dir=Path.GetDirectoryName(G.SavePath);string[] formal={"stash-v40.json","stash-v41.json","stash-v42.json"};byte[][] before=Array.ConvertAll(formal,n=>File.Exists(Path.Combine(dir,n))?File.ReadAllBytes(Path.Combine(dir,n)):null);
   var controls=G.Controls;freightMonsterRows2720.Clear();freightMonsterRows2720.Add("type,map,name,x,y,z,detail");
   try{
    FreshCamp();string inventory=JsonUtility.ToJson(G.Save);G.Controls=new TouchInput();G.HarborWalkthrough=true;yield return G.BeginTrainingLoad2721();G.Paused=true;
    Check(G.TrainingActive&&G.Map.GroundCacheUsed2721&&G.Map.HarborPlanCacheUsed2721,"2722 actual asynchronous harbor spawn retains both loading caches");
    Check(FreightPreview2716.Apply(G),"2722 freight preview arrival remains clear");var outdoor=G.Map;var outdoorActors=G.Enemies.ToArray();FreightRoster2722(false);
    foreach(var p in outdoor.FreightEnemies2720)FreightMonsterPatrol2720(p,null);
    var extra=outdoor.FreightEnemies2720[3].Actor;FreightMonsterAttack2720(extra);var outdoorCorpse=FreightMonsterKill2720(extra);
    var states=new List<FreightGroup2722>();
    foreach(string key in new[]{"freightduty","freightwarehouse"}){
     var stand=key=="freightduty"?RaidMap.FreightDutyArrival2716:RaidMap.FreightWarehouseArrival2716;FreightMonsterPlace2720(stand,key+" arrival");FreightMonsterF2720(outdoor.Doors.Find(d=>d.Portal&&d.Destination==key),true);yield return null;
     Check(G.Map.InteriorId==key&&Array.TrueForAll(outdoorActors,e=>!e.gameObject.activeInHierarchy),"2722 actual held-F interior switch suspends all exterior actors");FreightRoster2722(true);
     foreach(var p in G.Map.FreightEnemies2720){var room=G.Map.FreightRooms2716.Find(r=>r.Name==p.RoomName);FreightMonsterPatrol2720(p,room);if(room!=null)FreightMonsterClosedDoor2720(p,room);}
     foreach(var room in G.Map.FreightRooms2716){if(room.Door.Open)continue;float side=room.Area.center.x<room.Door.Position.x?-1:1;FreightRoomWalk2717(room.Door.Position-Vector3.right*side*.95f,"2722 room door approach");FreightMonsterF2720(room.Door,false);Check(room.Door.Open,"2722 real F reveals occupied room "+room.Name);}
     for(int i=1;i<G.Map.FreightEnemies2720.Count;i++)FreightMonsterAttack2720(G.Map.FreightEnemies2720[i].Actor);
     var victim=G.Map.FreightEnemies2720[1].Actor;var corpse=FreightMonsterKill2720(victim);FreightMonsterSearch2720(corpse);FreightMonsterShots2720(G.Map.FreightEnemies2720[0].Actor,2);
     states.Add(FreightGroupTake2722(corpse));FreightRoomWalk2717(G.Map.Spawn,"2722 return to actual exit");FreightMonsterF2720(G.Map.FreightExit2716,true);yield return null;
     Check(G.Map==outdoor&&G.Enemies.Count==30&&Array.TrueForAll(outdoorActors,e=>G.Enemies.Contains(e))&&extra.Dead&&outdoor.Loot.Contains(outdoorCorpse),"2722 exterior roster and killed added monster persist across entry");
    }
    foreach(var state in states){G.SwitchInterior(state.Map.InteriorId);yield return null;FreightGroupCheck2722(state);G.SwitchInterior(state.Map.InteriorId);yield return null;}
    G.Finish(false);yield return null;Check(G.Phase==GamePhase.Base&&JsonUtility.ToJson(G.Save)==inventory,"2722 training restores exact original inventory");
    for(int i=0;i<formal.Length;i++)Check(MainBytes273(before[i],File.Exists(Path.Combine(dir,formal[i]))?File.ReadAllBytes(Path.Combine(dir,formal[i])):null),"2722 formal bytes unchanged "+formal[i]);
   }finally{G.ClearShots();G.Controls=controls;G.Paused=true;File.WriteAllLines(Path.Combine(folder,"2722-freight-monsters-actual.csv"),freightMonsterRows2720);}
  }
 }
}
