using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using UnityEngine;
namespace GreyHarbor {
 public sealed partial class RaidVerification {
  string RangeN278(float n)=>n.ToString("0.00000",CultureInfo.InvariantCulture);
  RaidActor RangeTarget278(string name,int level){
   var go=new GameObject(name);go.transform.SetParent(G.Map.Root,false);var actor=go.AddComponent<RaidActor>();actor.Initialize(true);
   actor.IsRebel=false;actor.IsMonster=false;actor.NpcLevel=level;actor.NpcArmorPoints=actor.NpcMaxArmor=0;actor.Health=actor.NpcMaxHealth=100;return actor;
  }
  void RangeDiscard278(RaidActor actor){actor.gameObject.SetActive(false);Destroy(actor.gameObject);}
  IEnumerator RangePlayer278(TouchInput input,ItemKind ammo,float distance,float dt,List<string> rows){
   G.Paused=true;G.ClearShots();G.ResetAkHandling276();G.MagazineAmmo=ammo;G.Magazine=30;G.ReloadTime=G.HealTime=0;G.CancelArmorRepair();G.InventoryOpen=false;
   G.Player.transform.position=new Vector3(500,0,500);G.Player.Dead=false;G.Player.Health=100;G.Rig.ResetView(45);G.Rig.Tick(0);
   var screen=G.View.WorldToScreenPoint(G.Player.transform.position+Vector3.up*RaidGame.CombatHeight);input.AimPosition=new Vector2(screen.x+180,screen.y);input.Shooting=false;input.Movement=Vector2.zero;G.UpdateAim();Physics.SyncTransforms();
   // Real FirePlayer cooldown is left intact; no automatic input shot is permitted.
   yield return new WaitForSeconds(.13f);G.Paused=false;G.FirePlayer();G.Paused=true;
   string label=ammo+" target="+distance+" step="+dt;
   Check(G.AkShots276.Count==1&&G.Magazine==29,"278 Actual FirePlayer creates exactly one projectile "+label);var shot=G.AkShots276[0];
   var target=RangeTarget278("QA278Target",RaidInventory.Grade(ammo)+1);var point=shot.Origin+shot.Direction*distance;target.transform.position=new Vector3(point.x,0,point.z);Physics.SyncTransforms();
   for(int i=0;i<120&&!shot.Ended;i++)G.TickProjectiles(dt);
   Check(shot.Ended,"278 Actual projectile ends within bounded integration "+label);
   if(distance<12){Check(shot.Hit==target.name&&Mathf.Abs(target.Health-82)<.002f,"278 Eleven-metre same-grade physical target receives18 damage "+label);}
   else {
    Check(target.Health==100&&shot.Hit=="RangeEnd","278 Thirteen-metre target remains unharmed and projectile reports range end "+label);
    Check(Mathf.Abs(Vector3.Distance(shot.Origin,shot.Impact)-12)<.002f&&Vector3.Distance(shot.Impact,shot.Origin+shot.Direction*12)<.002f,"278 Actual endpoint is12 metres from true muzzle, including large dt "+label);
   }
   rows.Add(ammo+","+RangeN278(distance)+","+RangeN278(dt)+","+RangeN278(target.Health)+","+shot.Hit+","+RangeN278(Vector3.Distance(shot.Origin,shot.Impact)));
   G.ClearShots();RangeDiscard278(target);
  }
  void RangeNpc278(List<string> rows){
   G.Paused=true;G.ClearShots();var owner=RangeTarget278("QA278NpcOwner",1);owner.NpcAmmo=ItemKind.GreenAmmo;owner.transform.position=new Vector3(500,0,500);owner.Aim=Vector3.right;Physics.SyncTransforms();
   G.SpawnShot(owner,Vector3.right);var traces=G.Map.Root.GetComponentsInChildren<LineRenderer>();LineRenderer trace=null;int count=0;
   foreach(var line in traces)if(line.name=="BulletTrace"){trace=line;count++;}
   Check(count==1&&trace!=null,"278 Independent NPC creates one real projectile with observable muzzle");var origin=trace.GetPosition(0);var target=RangeTarget278("QA278NpcFarTarget",1);var point=origin+Vector3.right*13;target.transform.position=new Vector3(point.x,0,point.z);Physics.SyncTransforms();
   G.TickProjectiles(.5f);Check(Mathf.Abs(target.Health-86)<.002f&&G.LastEnemyShotDamage276==14&&G.LastEnemyShotPen276==10,"278 Actual independent NPC projectile reaches13 metres for14 damage, unaffected by player12m cap");
   rows.Add("NPC,13,0.5,"+RangeN278(target.Health)+",physical-target,"+RangeN278(Vector3.Distance(origin,trace.GetPosition(0))));G.ClearShots();RangeDiscard278(target);RangeDiscard278(owner);
  }
  IEnumerator VerifyWeaponRange278(){
   var oldInput=G.Controls;var oldRandom=UnityEngine.Random.state;string dir=Path.GetDirectoryName(G.SavePath);string[] formal={Path.Combine(dir,"stash-v40.json"),Path.Combine(dir,"stash-v41.json")};byte[][] bytes=Array.ConvertAll(formal,p=>File.Exists(p)?File.ReadAllBytes(p):null);
   Check(Path.GetFileName(G.SavePath)=="verification-stash-2.7.8.json","278 Range verification uses isolated inventory");FreshCamp();string camp=JsonUtility.ToJson(G.Save);G.HardMode=false;G.HarborWalkthrough=false;Check(G.BeginTraining(),"278 Begin isolated actual projectile preview");G.Paused=true;yield return null;
   var rows=new List<string>{"ammo,targetMetres,stepSeconds,remainingHealth,termination,travelMetres"};var input=new TouchInput();
   try {
    G.Controls=input;UnityEngine.Random.InitState(278);foreach(var enemy in G.Enemies)enemy.Dead=true;foreach(var collider in G.Map.Root.GetComponentsInChildren<Collider>())collider.enabled=false;G.Equipped=new ItemStack(ItemKind.AK,1);G.ResetAkHandling276();
    foreach(var ammo in new[]{ItemKind.GreenAmmo,ItemKind.Ammo})foreach(float dt in new[]{.016f,1.5f})foreach(float distance in new[]{11f,13f})yield return RangePlayer278(input,ammo,distance,dt,rows);
    // Let Unity finish deferred trace destruction before identifying the one NPC shot.
    yield return null;RangeNpc278(rows);G.Controls=oldInput;G.Finish(false);yield return null;Check(JsonUtility.ToJson(G.Save)==camp,"278 Projectile preview restores exact inventory");
    for(int i=0;i<formal.Length;i++)Check(MainBytes273(bytes[i],File.Exists(formal[i])?File.ReadAllBytes(formal[i]):null),"278 Formal inventory remains unchanged "+Path.GetFileName(formal[i]));
   }finally{G.Controls=oldInput;UnityEngine.Random.state=oldRandom;G.Paused=true;File.WriteAllLines(Path.Combine(folder,"278-projectile-range.csv"),rows);}
  }
 }
}
