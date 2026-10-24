using System;using System.IO;using System.Linq;using System.Collections;using UnityEngine;
namespace GreyHarbor {public sealed partial class RaidVerification {
 IEnumerator RunBalance344(){
  var sprint=new SprintState2711();sprint.Toggle();sprint.Tick(10,true,Vector3.right,1);Check(sprint.Stamina==0&&sprint.Exhausted,"344 sprint drains ten points and becomes exhausted");
  sprint.Tick(3.9f,false,Vector3.zero,0);Check(sprint.Stamina==0,"344 no stamina recovery during first3.9nonrunning seconds");sprint.Tick(.1f,false,Vector3.zero,0);Check(sprint.Stamina<.0001f,"344 four-second wait completes without immediate refund");
  sprint.Tick(2,false,Vector3.zero,0);Check(Mathf.Abs(sprint.Stamina-2)<.0001f&&!sprint.ReadyToRun,"344 recovery is one point per second and two points cannot resume sprint");sprint.Tick(1,false,Vector3.zero,0);Check(sprint.Stamina==3&&sprint.ReadyToRun,"344 three points release sprint gate");sprint.Tick(.5f,true,Vector3.right,1);float prior=sprint.Stamina;sprint.Tick(3,false,Vector3.zero,0);Check(sprint.Stamina==prior&&Mathf.Abs(sprint.RecoveryDelayRemaining-1)<.001f,"344 running restarts the full four-second wait");
  var rng=new System.Random(344);int kits=0,total=0;bool legal=true,atMostOne=true;
  foreach(var look in new[]{ContainerLook.MedicalCabinet,ContainerLook.RescueCabinet})foreach(var region in new[]{DropRegion230.Entrance,DropRegion230.Yard,DropRegion230.Repair,DropRegion230.Office}){
   var p=ContainerDropRules230.Profile(look,SupplyType.Medical,ContainerTier.Common,region);p.EnableBonusRed=false;ContainerDropRules230.Validate(p);
   for(int i=0;i<250;i++){var items=ContainerDropRules230.Contents(p,rng,i%2==0);int supplies=items.Count(x=>ConsumableRules.IsRepair(x.Kind));if(supplies>0)kits++;total++;atMostOne&=supplies<=1;legal&=items.All(x=>ConsumableRules.IsRepair(x.Kind)||RaidInventory.Group(x.Kind)==ItemGroup.Collection&&RaidInventory.Category(x.Kind)==CollectionCategory.MedicalMaterials);}
  }
  Check(legal&&atMostOne,"344 medical cabinets yield saleable medical collections with at most one repair pack");float rate=kits/(float)total;results.Add("METRIC medical pack containers "+kits+"/"+total+"="+rate);Check(rate>.17f&&rate<.23f,"344 seeded medical consumable frequency stays near20percent");
  FreshCamp();G.HarborWalkthrough=true;G.enabled=false;yield return G.BeginTrainingLoad2721();G.enabled=false;G.Paused=G.InventoryOpen=false;G.SwitchInterior("yardhome");yield return null;yield return null;
  var room=G.Map.YardRooms138.Find(x=>x.Name=="起居室");Check(room!=null&&room.Door.Open&&room.Door.Pivot==null&&room.Door.Collider==null&&!G.Map.Doors.Contains(room.Door),"344 living room entry no longer has a door leaf or interaction");
  Check(G.Map.KeyRooms2760.Count==1&&G.Map.KeyRooms2760[0].Door.Locked,"344 living-room opening preserves the separate locked keyroom");
  Check(G.Vision.RoomAllowed(new Vector3(-3,0,-3)),"344 open living room is revealed on entering the cottage");
  foreach(var e in G.Enemies)e.gameObject.SetActive(false);G.Enemies.Clear();Physics.SyncTransforms();Vector3 opening=new Vector3(-1.6f,0,-3);
  Check(!Physics.CheckCapsule(opening+Vector3.up*.35f,opening+Vector3.up,.3f,SceneObstacle.MovementMask,QueryTriggerInteraction.Ignore),"344 former door opening has no blocking collision");
  G.Player.transform.position=new Vector3(-.4f,0,-3);G.Rig.Target=G.Player.transform;G.Rig.ResetView(45);G.Rig.Tick(0);G.Player.Aim=Vector3.left;G.Equipped=new ItemStack(ItemKind.SG12,1);G.MagazineAmmo=ItemKind.Blue12Gauge;
  var target=RangeTarget278("344DamageTarget",1);target.transform.position=new Vector3(-3,0,-3);target.Health=target.NpcMaxHealth=200;G.ResolveShotgunTarget30(target,8);Check(Mathf.Abs(target.Health-132)<.001f,"344 eight landed shotgun pellets cause68health damage");target.Health=200;G.ResolveShotgunTarget30(target,4);Check(Mathf.Abs(target.Health-166)<.001f,"344 four landed pellets proportionally cause34health damage");target.NpcArmorPoints=target.NpcMaxArmor=20;target.Health=200;G.ResolveShotgunTarget30(target,8);Check(target.NpcArmorPoints==0&&target.Health==200,"344 armor-breaking shotgun shell still does not spill into health");
  target.ConfigureMonster(MonsterKind.Wanderer);target.Health=target.NpcMaxHealth*.7f;target.NpcArmorPoints=target.NpcMaxArmor*.5f;target.Alert=.6f;G.Enemies.Add(target);
  var soldier=G.SpawnRebel145(new Vector3(-3,0,-4.05f),RebelKind.Heavy);soldier.Alert=.6f;
  G.Vision.Refresh(G.Player.transform.position,G.Player.Aim);Call314(G,"LateUpdate");var ui=G.GetComponent<RaidUI>();ui.SetScreenScale332();
  Check(ui.EnemyStatusVisible344(target)&&ui.EnemyStatusVisible344(soldier),"344 visible monster and soldier both receive new overhead status UI");
  target.VisibleToPlayer=false;Check(!ui.EnemyStatusVisible344(target),"344 hidden enemy cannot display overhead information");target.VisibleToPlayer=true;target.Dead=true;Check(!ui.EnemyStatusVisible344(target),"344 dead enemy overhead information is hidden");target.Dead=false;
  Check(RaidUI.EnemyName344(target)==HarborContent.MonsterNames[(int)MonsterKind.Wanderer]&&RaidUI.EnemyName344(soldier).Contains("重甲"),"344 enemy label uses actual monster or soldier type");
  G.NoticeTime=0;yield return null;yield return null;yield return new WaitForEndOfFrame();var image=ScreenCapture.CaptureScreenshotAsTexture();File.WriteAllBytes(Path.Combine(folder,"native-344-yard-enemy-ui.png"),image.EncodeToPNG());Destroy(image);G.Finish(false);
  File.WriteAllLines(Path.Combine(folder,"balance-344.txt"),results);Application.Quit(0);
 }
}}