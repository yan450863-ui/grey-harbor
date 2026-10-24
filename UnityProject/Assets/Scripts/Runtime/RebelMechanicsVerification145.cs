using UnityEngine;
using System.Collections;
namespace GreyHarbor {
 public sealed partial class RaidVerification {
  void VerifyClosedCabinWalls145(){
   var map=G.Map;
   foreach(var room in map.FerryRooms143.GetRange(0,4)){
    var center=new Vector3(room.Area.center.x,0,room.Area.center.y);var path=G.Navigation.FindPath(G.Player.transform.position,center);
    Check(!room.Door.Open&&(path.Count==0||Vector3.Distance(path[path.Count-1],center)>.2f),"145 Closed side room cannot be bypassed "+room.Name);
   }
   foreach(float x in new[]{-4.35f,4.35f}){
    Vector3 a=new Vector3(x,0,-1),b=new Vector3(x,0,-3);
    Check(!G.ClearSight(a,b)&&!G.Navigation.ClearSegment(a,b),"145 Side-room lobby end wall blocks sight and movement "+x);
   }
   foreach(var room in map.FerryRooms143.GetRange(0,4)){
    var saved=G.Player.transform.position;G.Player.transform.position=room.Door.Position+(room.Door.Position.x<0?Vector3.right:Vector3.left)*1.1f;Physics.SyncTransforms();Check(G.ToggleRoomDoor(room.Door),"145 Intended side door opens "+room.Name);
    var center=new Vector3(room.Area.center.x,0,room.Area.center.y);var path=G.Navigation.FindPath(G.Player.transform.position,center);Check(path.Count>0&&Vector3.Distance(path[path.Count-1],center)<.2f,"145 Room reachable only through intended open door "+room.Name);
    Check(G.ToggleRoomDoor(room.Door),"145 Intended side door closes "+room.Name);G.Player.transform.position=saved;Physics.SyncTransforms();
   }
   Check(map.FerryBulkheads145&&map.FerryBulkheads145.Find("MarineUpperPanel")&&map.FerryBulkheads145.Find("BulkheadRib")&&map.FerryBulkheads145.Find("PerimeterWallCap"),"145 All-height metal panels ribs and perimeter caps present");
  }
  void ResetRebel145(RaidActor e){e.Dead=false;e.Health=100;e.HitCollider.enabled=true;e.RebelArmor=new ItemStack(RebelTuning145.Armor(e.Rebel),1);ArmorBalance.Ensure(e.RebelArmor);e.Alert=1;e.Memory=7;e.Windup=e.AttackCooldown=e.RebelShotDelay=0;e.RebelBurst=0;}
  void VerifyRebel145(RaidActor e){
   var home=e.transform.position;var pp=G.Player.transform.position;float hp=G.Player.Health;var armor=G.Armor;int kills=G.Kills;
   ResetRebel145(e);int breakHits=e.Rebel==RebelKind.Heavy?12:8,total=breakHits+8;
   for(int i=0;i<breakHits;i++)G.ApplyCombatHit(e,14,10,1,1);
   Check(e.RebelArmor.ArmorPoints==0&&e.Health==100&&!e.Dead,"145 Armor break bullet has no health overflow "+e.Rebel);
   for(int i=0;i<7;i++)G.ApplyCombatHit(e,14,10,1,1);
   Check(e.Health==2&&!e.Dead,"145 Fixed 100 health survives penultimate AK hit "+e.Rebel);G.ApplyCombatHit(e,14,10,1,1);Check(e.Dead&&e.Health==0,"145 AK kills in "+total+" hits "+e.Rebel);
   ResetRebel145(e);G.Armor=new ItemStack(ItemKind.BlueArmor,1);ArmorBalance.Ensure(G.Armor);G.Player.Health=100;
   e.transform.position=e.Rebel==RebelKind.Regular?new Vector3(0,0,8):new Vector3(2.1f,0,-8.45f);G.Player.transform.position=e.transform.position+Vector3.back*3;Physics.SyncTransforms();G.ClearShots();G.Rig.Tick(0);
   G.TickRebel145(e,.01f);Check(Mathf.Abs(e.Windup-RebelTuning145.Windup(e.Rebel))<.001f&&G.ActiveGunShots145==0,"145 Detection begins harmless configured windup "+e.Rebel);
   G.TickRebel145(e,RebelTuning145.Windup(e.Rebel)-.01f);Check(e.Windup>0&&G.ActiveGunShots145==0&&G.Player.Health==100,"145 No shot before windup expires "+e.Rebel);
   G.TickRebel145(e,.02f);Check(G.ActiveGunShots145==1&&e.RebelBurst==2,"145 Completed windup launches first actual round "+e.Rebel);G.TickProjectiles(.25f);Check(G.TotalArmor==110&&G.Player.Health==100,"145 Rebel bullet applies blue AK armor-first damage "+e.Rebel);
   G.TickRebel145(e,.2f);G.TickProjectiles(.25f);G.TickRebel145(e,.2f);G.TickProjectiles(.25f);Check(e.RebelBurst==0&&e.AttackCooldown>0&&G.TotalArmor==90,"145 Three-shot burst then cooldown "+e.Rebel);
   G.ClearShots();ResetRebel145(e);G.TickRebel145(e,.01f);var screen=G.Map.Box("RebelCoverCheck145",(e.transform.position+G.Player.transform.position)/2+Vector3.up, new Vector3(2,2,.2f),"687C6B",true);Physics.SyncTransforms();G.TickRebel145(e,2);Check(e.Windup==0&&e.RebelBurst==0&&G.ActiveGunShots145==0,"145 High cover cancels aim and prevents shooting through wall "+e.Rebel);screen.SetActive(false);Object.Destroy(screen);Physics.SyncTransforms();
   ResetRebel145(e);G.Player.transform.position=new Vector3(6.6f,0,0);e.Alert=e.Memory=0;e.WaitTime=0;e.PatrolIndex=1;var from=e.transform.position;for(int i=0;i<45;i++){G.TickRebel145(e,.05f);Physics.SyncTransforms();}Check(Vector3.Distance(from,e.transform.position)>.4f,"145 Actual patrol advances "+e.Rebel);
   ResetRebel145(e);e.transform.position=home;e.Alert=e.Memory=0;e.WaitTime=2;G.Player.transform.position=pp;G.Player.Health=hp;G.Armor=armor;G.Kills=kills;G.ClearShots();Physics.SyncTransforms();
  }
  IEnumerator VerifyRebelDeath145(RaidActor e){
   var pp=G.Player.transform.position;G.Player.transform.position=new Vector3(0,0,5);e.transform.position=new Vector3(0,0,8);ResetRebel145(e);G.Player.Aim=Vector3.forward;G.Rig.Tick(0);Physics.SyncTransforms();int kills=G.Kills,before=G.Map.Loot.Count;
   for(int i=0;i<16;i++){G.SpawnShot(G.Player,Vector3.forward);G.TickProjectiles(.2f);}
   Check(e.Dead&&G.Kills==kills+1&&G.Map.Loot.Count==before+1,"145 Actual player projectiles kill regular rebel once and create corpse");
   var loot=G.Map.Loot[G.Map.Loot.Count-1];Check(loot.Name.Contains("叛军")&&loot.Items.Count==2&&loot.Items.TrueForAll(p=>!p.Identified),"145 Rebel corpse uses human name and two unknown supplies");Check(Walk140(loot.Position),"145 Corpse approachable after firefight");G.EnsureLootGrid(loot);G.Player.Aim=Vector3.forward;G.Vision.Refresh(G.Player.transform.position,G.Player.Aim);F111(false,0);F111(true,loot.SearchDuration+.02f);Check(loot.Opened&&G.OpenLoot==loot,"145 F searches human corpse");G.Paused=false;G.TickIdentification(3);G.TickIdentification(3);Check(loot.Items.TrueForAll(p=>p.Identified),"145 Corpse items identified");Check(G.QuickTransfer(ItemArea.Loot,loot.Items[0].Cell),"145 Corpse ammunition can be picked up");G.Paused=true;G.CloseInventory();yield return null;G.Player.transform.position=pp;G.ClearShots();Physics.SyncTransforms();
  }
 }
}
