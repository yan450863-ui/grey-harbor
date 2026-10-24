using System;using System.Collections;using System.Collections.Generic;using System.IO;using UnityEngine;
namespace GreyHarbor {public sealed partial class RaidVerification {
 IEnumerator ShotRange312(ItemKind kind,ItemKind ammo,bool modified,int mode,float dt,List<string> rows){
  G.ClearShots();WeaponFixture30(kind,ammo);if(modified)G.Equipped.Attachments30.Add(new ItemStack(ItemKind.BlueMuzzle30,1));G.ResetAkHandling276();G.ReloadTime=G.HealTime=G.RepairTime=0;G.InventoryOpen=false;G.Paused=false;
  float range=kind==ItemKind.AK?9:9.5f;G.Player.Aim=G.Rig.Right;G.RawPlayerAim276=G.Player.Aim;var input=(TouchInput)G.Controls;var screen=G.View.WorldToScreenPoint(G.Player.transform.position+Vector3.up*RaidGame.CombatHeight);input.AimPosition=new Vector2(screen.x+180,screen.y);G.UpdateAim();
  yield return new WaitForSecondsRealtime(.15f);G.FirePlayer();G.Paused=true;
  string label=kind+" "+ammo+" modified="+modified+" mode="+mode+" step="+dt;
  Check(G.AkShots276.Count==1&&G.Magazine==G.MagazineCapacity279-1,"312 real trigger creates one shot "+label);if(G.AkShots276.Count!=1)yield break;
  var shot=G.AkShots276[0];RaidActor target=null;
  if(mode!=0){target=RangeTarget278("RifleTarget312",RaidInventory.Grade(ammo)+1);var point=shot.Origin+shot.Direction*(mode==1?range-1:range+1);target.transform.position=new Vector3(point.x,0,point.z);Physics.SyncTransforms();}
  for(int i=0;i<120&&!shot.Ended;i++)G.TickProjectiles(dt);
  Check(shot.Ended,"312 real shot terminates "+label);
  if(mode==1)Check(shot.Hit==target.name&&Mathf.Abs(target.Health-(100-G.ActiveWeapon279.Damage))<.002f,"312 same-grade hit inside range retains damage "+label);
  else {Check(shot.Hit=="RangeEnd"&&Mathf.Abs(Vector3.Distance(shot.Origin,shot.Impact)-range)<.002f,"312 endpoint stops at muzzle range even with large frame "+label);if(target!=null)Check(target.Health==100,"312 target beyond new range takes no damage "+label);}
  rows.Add(label+",travel="+Vector3.Distance(shot.Origin,shot.Impact).ToString("0.000")+",termination="+shot.Hit+",health="+(target?target.Health.ToString():"none"));G.ClearShots();if(target)RangeDiscard278(target);G.Paused=false;yield return null;
 }
 IEnumerator RunRifleRange312(){
  Check(G.SavePath.EndsWith("verification-stash-3.0.14.json"),"312 isolated save");
  Check(WeaponRules279.AK.Range==9&&BlueWeapons30.AR.Range==9.5f&&WeaponRange278.Ak==9,"312 AK9m and AR16 9.5m definitions agree");
  Check(WeaponRules279.MP5.Range==8&&WeaponRules279.SKS.Range==56f/3f&&BlueWeapons30.SMG.Range==6&&BlueWeapons30.Shotgun.Range==5,"312 other weapon ranges preserved");
  Check(WeaponRules279.AK.Damage==18&&WeaponRules279.AK.Penetration==10&&WeaponRules279.AK.Interval==.11f&&BlueWeapons30.AR.Damage==16&&BlueWeapons30.AR.Penetration==9&&BlueWeapons30.AR.Interval==.09f,"312 damage penetration and firing rates preserved");
  foreach(var k in new[]{ItemKind.AK,ItemKind.AR16}){string r=k==ItemKind.AK?"9":"9.5";Check(RaidInventory.Use(k).Contains(r+"米")&&ModRules30.Describe(new ItemStack(k,1)).Contains("射程"+r+"米"),"312 inventory and modification descriptions show current range "+k);}
  StandardRaid();G.enabled=false;G.CameraFocusOverride309=true;G.InventoryOpen=G.Paused=false;foreach(var c in G.Map.Root.GetComponentsInChildren<Collider>())c.enabled=false;foreach(var e in G.Enemies){e.Dead=true;e.HitCollider.enabled=false;}
  G.Player.transform.position=new Vector3(500,0,500);G.Rig.ResetView(45);G.Rig.Tick(0);G.Controls=new TouchInput();var rows=new List<string>();
  foreach(var k in new[]{ItemKind.AK,ItemKind.AR16})foreach(bool mod in new[]{false,true})foreach(var ammo in k==ItemKind.AK?new[]{ItemKind.GreenAmmo,ItemKind.Ammo}:new[]{ItemKind.Green556,ItemKind.Blue556})foreach(float dt in new[]{.016f,1.5f})foreach(int mode in new[]{0,1,2})yield return ShotRange312(k,ammo,mod,mode,dt,rows);
  // Actual viewport projections use the current camera. Test centered and edge-look cases.
  foreach(var k in new[]{ItemKind.AK,ItemKind.AR16})foreach(float yaw in new[]{45,135,225,315})foreach(bool edge in new[]{false,true}){
   WeaponFixture30(k,k==ItemKind.AK?ItemKind.GreenAmmo:ItemKind.Green556);G.Paused=false;G.Rig.ResetView(yaw);G.Rig.Tick(0);bool visible=true;
   for(int a=0;a<360;a+=30){G.Rig.ResetView(yaw);G.Rig.Tick(0);float radians=a*Mathf.Deg2Rad;var direction=(G.Rig.Right*Mathf.Cos(radians)+G.Rig.Forward*Mathf.Sin(radians)).normalized;G.Player.Aim=direction;G.Player.Facing=Mathf.Cos(radians)>=0?1:-1;G.Player.Draw(G.View,0);if(edge){var end=G.View.WorldToScreenPoint(G.Player.MuzzleOnPlane(G.View)+direction*G.ActiveWeapon279.Range);var delta=new Vector2(end.x-Screen.width*.5f,end.y-Screen.height*.5f);float scale=Mathf.Min(Mathf.Abs(delta.x)<.001f?float.PositiveInfinity:Screen.width*.5f/Mathf.Abs(delta.x),Mathf.Abs(delta.y)<.001f?float.PositiveInfinity:Screen.height*.5f/Mathf.Abs(delta.y));var cursor=new Vector2(Screen.width*.5f,Screen.height*.5f)+delta*scale*.9999f;G.Rig.TickEdgePan309(cursor,Screen.width,Screen.height,true,2);G.Rig.Tick(0);G.Player.Draw(G.View,0);}
    var endpoint=G.View.WorldToViewportPoint(G.Player.MuzzleOnPlane(G.View)+direction*G.ActiveWeapon279.Range);if(endpoint.x<0||endpoint.x>1||endpoint.y<0||endpoint.y>1)Debug.Log("RANGE312_VIEWPORT "+k+" angle="+a+" endpoint="+endpoint+" origin="+G.Player.MuzzleOnPlane(G.View)+" player="+G.Player.transform.position);visible&=endpoint.x>=0&&endpoint.x<=1&&endpoint.y>=0&&endpoint.y<=1;
   }
   Check(visible,"312 actual camera shows range endpoints in12 directions "+k+" yaw="+yaw+" edge="+edge);
  }
  var npcRows=new List<string>();RangeNpc278(npcRows);rows.AddRange(npcRows);G.Paused=false;G.CameraFocusOverride309=null;G.Controls=new TouchInput();G.Finish(false);File.WriteAllLines(Path.Combine(folder,"3.0.14-rifle-actual-projectiles.csv"),rows);results.Add("RIFLE RANGE 3.0.14 COMPLETE");
 }
}}
