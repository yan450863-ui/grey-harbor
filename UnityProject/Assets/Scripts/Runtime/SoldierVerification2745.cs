using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace GreyHarbor {
 public sealed partial class RaidVerification {
  readonly RebelKind[] SoldierKinds2745={RebelKind.Smg,RebelKind.Marksman,RebelKind.BlueSmg,RebelKind.BlueMarksman};
  RaidActor SoldierFixture2745(RebelKind kind){
   CombatDisable275();var e=G.SpawnRebel145(new Vector3(500,0,504),kind,"2745 isolated soldier");e.HitCollider.enabled=false;
   G.Player.transform.position=new Vector3(500,0,500);e.Home=e.transform.position;e.Alert=1;e.AttackCooldown=0;e.Windup=0;e.RebelBurst=0;G.Rig.Tick(0);Physics.SyncTransforms();return e;
  }
  void SoldierBehavior2745(RebelKind kind){
   var e=SoldierFixture2745(kind);bool smg=SoldierProfiles2745.IsSmg(kind);int expected=smg?4:1;float aim=smg?.45f:1,interval=smg?.13f:2.2f;
   G.TickRebel145(e,.01f);Check(CombatNear275(e.Windup,aim)&&G.ActiveGunShots145==0,"2745 actual AI begins visible aim before firing "+kind);
   G.TickRebel145(e,aim-.02f);Check(G.ActiveGunShots145==0&&e.Windup>0,"2745 aim delay prevents instant release "+kind);
   G.TickRebel145(e,.025f);Check(G.ActiveGunShots145==1&&e.RebelBurst==expected-1,"2745 actual first projectile and burst count "+kind);
   if(smg){G.TickRebel145(e,.10f);Check(G.ActiveGunShots145==1,"2745 MP5 does not fire before interval "+kind);G.TickRebel145(e,.035f);Check(G.ActiveGunShots145==2,"2745 MP5 second actual projectile after interval "+kind);G.TickRebel145(e,interval+.001f);G.TickRebel145(e,interval+.001f);}
   Check(G.ActiveGunShots145==expected&&e.RebelBurst==0&&CombatNear275(e.AttackCooldown,smg?1.4f:2.2f),"2745 actual burst ends with recovery "+kind);
   G.ClearShots();G.TickRebel145(e,.5f);Check(e.Windup==0&&G.ActiveGunShots145==0,"2745 recovery blocks immediate repeat "+kind);
   e.AttackCooldown=0;e.Windup=0;G.TickRebel145(e,.01f);Check(e.Windup>0,"2745 next aim can start "+kind);
   var wall=GameObject.CreatePrimitive(PrimitiveType.Cube);wall.name="2745 QA sight and projectile cover";wall.layer=SceneObstacle.HighLayer;wall.transform.position=new Vector3(500,1.5f,502);wall.transform.localScale=new Vector3(5,3,.25f);Physics.SyncTransforms();
   Check(!G.ClearSight(e.transform.position,G.Player.transform.position),"2745 real wall blocks sight "+kind);G.TickRebel145(e,2);Check(e.Windup==0&&e.RebelBurst==0&&G.ActiveGunShots145==0,"2745 interrupted aim cannot release through cover "+kind);
   G.Armor=G.Helmet=null;G.Player.Health=100;e.Aim=Vector3.back;G.SpawnShot(e,Vector3.back);G.TickProjectiles(.3f);Check(G.Player.Health==100&&G.ActiveGunShots145==0,"2745 real projectile intercepted by wall "+kind);wall.SetActive(false);Destroy(wall);Physics.SyncTransforms();
   e.transform.position=new Vector3(500,0,504);G.Player.transform.position=new Vector3(500,0,504-SoldierProfiles2745.Range(kind)-.25f);e.Alert=1;e.AttackCooldown=0;e.Windup=.2f;e.RebelBurst=1;e.RebelShotDelay=0;Physics.SyncTransforms();G.TickRebel145(e,.3f);
   Check(e.Windup==0&&e.RebelBurst==0&&G.ActiveGunShots145==0,"2745 leaving range cancels queued attack "+kind);
   G.Player.transform.position=new Vector3(500,0,500);e.transform.position=new Vector3(500,0,504);e.Alert=1;e.AttackCooldown=0;G.TickRebel145(e,.01f);G.TickRebel145(e,aim-.02f);Check(G.ActiveGunShots145==0,"2745 reacquisition requires complete fresh aim "+kind);G.ClearShots();CombatDiscard275(e);
  }
  void SoldierCombat2745(RebelKind kind){
   var e=SoldierFixture2745(kind);bool blue=kind==RebelKind.BlueSmg||kind==RebelKind.BlueMarksman;bool smg=SoldierProfiles2745.IsSmg(kind);float damage=smg?10:22;
   Check(e.NpcLevel==(blue?2:1)&&e.NpcWeapon==(smg?ItemKind.MP5:ItemKind.SKS)&&e.NpcAmmo==(smg?(blue?ItemKind.Blue9mm:ItemKind.Green9mm):(blue?ItemKind.Ammo:ItemKind.GreenAmmo)),"2745 actual spawn assigns model, caliber and grade "+kind);
   Check(e.VisualWeapon279==e.NpcWeapon,"2745 actual drawn weapon selects MP5 or SKS "+kind);
   Check(CombatNear275(e.NpcMaxArmor,blue?50:40)&&CombatNear275(e.NpcMaxHealth,blue?155:124),"2745 new troop retains same-level ordinary endurance "+kind);
   G.Armor=G.Helmet=null;G.Player.Health=100;CombatNpcShot275(e);CombatValue275(G.Player.Health,100-damage,"2745 actual independent unarmored projectile "+kind);
   e.NpcWeapon=ItemKind.AK;G.Player.Health=100;CombatNpcShot275(e);CombatValue275(G.Player.Health,100-damage,"2745 cosmetic AK does not replace independent damage "+kind);e.NpcWeapon=smg?ItemKind.MP5:ItemKind.SKS;
   e.NpcAmmo=SoldierProfiles2745.Ammo(kind);G.Armor=CombatArmor275(20);G.Player.Health=100;CombatNpcShot275(e);Check(CombatNear275(G.TotalArmor,blue?10:12)&&G.Player.Health==100,"2745 real bullet grade controls penetration "+kind);
   G.Armor=CombatArmor275(1);G.Player.Health=100;CombatNpcShot275(e);Check(G.TotalArmor==0&&G.Player.Health==100,"2745 armor breaking projectile has no overflow "+kind);CombatNpcShot275(e);CombatValue275(G.Player.Health,100-damage*(blue?1:.8f),"2745 exposed HP follows ammo quality "+kind);
   G.Armor=G.Helmet=null;G.Player.Health=100;e.transform.position=new Vector3(500,0,504);e.Aim=Vector3.back;G.Rig.Tick(0);e.Draw(G.View,0);Physics.SyncTransforms();G.SpawnShot(e,Vector3.back);var muzzle=e.MuzzleOnPlane(G.View);G.Player.transform.position=new Vector3(muzzle.x,0,muzzle.z-SoldierProfiles2745.Range(kind)-1);Physics.SyncTransforms();G.TickProjectiles(1);
   Check(G.Player.Health==100&&G.ActiveGunShots145==0,"2745 actual projectile expires at independent range "+kind);G.ClearShots();
   e.NpcArmorPoints=e.NpcMaxArmor;e.Health=e.NpcMaxHealth;e.Dead=false;var rows=new List<string>();CombatBudget275(e,blue?245:196,blue?1:0,rows);
   var drops=EnemyLoot146.Soldier(kind,new System.Random(145),false,blue?2:1);Check(drops.Count>0,"2745 new type has valid corpse supply pool "+kind);foreach(var stack in drops)if(AmmoRules270.IsAmmo(stack.Kind))Check(stack.Kind==SoldierProfiles2745.Ammo(kind),"2745 corpse ammo matches carried caliber and grade "+kind);
  }
  void SoldierArtCheck2745(RebelKind kind){
   for(int f=0;f<4;f++){
    var sprite=RebelArt145.Get(kind,f);Check(sprite&&sprite.texture&&sprite.rect.width==64&&sprite.rect.height==80,"2745 actual four-frame sprite loaded "+kind+" "+f);
    var tex=RebelArt145.CreateFrame(kind,f);int visible=0,colored=0,green=0,blue=0;
    foreach(var c in tex.GetPixels32()){if(c.a<128)continue;visible++;int high=Math.Max(c.r,Math.Max(c.g,c.b)),low=Math.Min(c.r,Math.Min(c.g,c.b));if(high-low>12)colored++;if(c.g>c.b+8&&c.g>=c.r-20)green++;if(c.b>c.r+10&&c.g>c.r+8)blue++;}
    Check(visible>900&&colored>350,"2745 body has colored pixel-art detail, not gray placeholder "+kind+" "+f);Check((kind==RebelKind.BlueSmg||kind==RebelKind.BlueMarksman)?blue>150:green>150,"2745 palette matches troop quality "+kind+" "+f);
    File.WriteAllBytes(Path.Combine(folder,"2745-"+kind+"-"+f+".png"),tex.EncodeToPNG());Destroy(tex);
   }
  }
  IEnumerator SoldierPicture2745(){
   CombatDisable275();var all=new List<RaidActor>();var centre=new Vector3(0,0,-10);var cameraObject=new GameObject("2745 isolated soldier art review camera");var camera=cameraObject.AddComponent<Camera>();camera.orthographic=true;camera.orthographicSize=3;camera.clearFlags=CameraClearFlags.SolidColor;camera.backgroundColor=PixelArt.Hex("344C4D");camera.transform.rotation=Quaternion.Euler(50,0,0);camera.transform.position=centre-camera.transform.forward*15;
   float fog=Shader.GetGlobalFloat("_HarborFogEnabled");Shader.SetGlobalFloat("_HarborFogEnabled",0);
   for(int i=0;i<4;i++){var e=G.SpawnRebel145(centre+new Vector3(-3+i*2,0,0),SoldierKinds2745[i],"2745 art review");e.HitCollider.enabled=false;e.Alert=0;e.VisibleToPlayer=true;e.Aim=Vector3.right;e.Draw(camera,0);all.Add(e);}
   yield return null;foreach(var e in all){e.VisibleToPlayer=true;e.Draw(camera,0);}var overview=HvRender277(camera,"2745-new-soldiers-overview.png",1600,900);UnloadingLiveImage2724(overview,"2745 four actual soldier models with carried guns");Destroy(overview);
   for(int i=0;i<all.Count;i++){foreach(var e in all)e.gameObject.SetActive(false);var actor=all[i];actor.gameObject.SetActive(true);camera.orthographicSize=1.1f;camera.transform.position=actor.transform.position+Vector3.up*.7f-camera.transform.forward*15;actor.Draw(camera,0);yield return null;actor.VisibleToPlayer=true;actor.Draw(camera,0);var image=HvRender277(camera,"2745-"+actor.Rebel+"-model.png",800,800);UnloadingLiveImage2724(image,"2745 actual model "+actor.Rebel);Destroy(image);}
   Shader.SetGlobalFloat("_HarborFogEnabled",fog);foreach(var e in all)CombatDiscard275(e);Destroy(cameraObject);
  }
  IEnumerator RunNewSoldiers2745(){
   string dir=Path.GetDirectoryName(G.SavePath);string[] formal={Path.Combine(dir,"stash-v40.json"),Path.Combine(dir,"stash-v41.json"),Path.Combine(dir,"stash-v42.json")};byte[][] before=Array.ConvertAll(formal,p=>File.Exists(p)?File.ReadAllBytes(p):null);
   Check(G.VerifyMode&&Path.GetFileName(G.SavePath).StartsWith("verification-stash"),"2745 new soldier QA uses isolated save");FreshCamp();string inventory=JsonUtility.ToJson(G.Save);G.HarborWalkthrough=false;G.HardMode=false;Check(G.BeginTraining(),"2745 isolated training begins");G.Paused=true;yield return null;
   foreach(var kind in SoldierKinds2745){SoldierArtCheck2745(kind);SoldierBehavior2745(kind);SoldierCombat2745(kind);}
   foreach(var kind in new[]{RebelKind.Regular,RebelKind.Heavy,RebelKind.BlueRegular,RebelKind.BlueHeavy}){var e=SoldierFixture2745(kind);Check(NpcCombat275.IndependentDamage(e)==14&&NpcCombat275.IndependentPenetration(e)==10&&e.VisualWeapon279==ItemKind.AK,"2745 existing two soldier roles retain original combat "+kind);float budget=kind==RebelKind.Regular?196:kind==RebelKind.Heavy?252:kind==RebelKind.BlueRegular?245:315;Check(CombatNear275(e.NpcMaxArmor,NpcCombat275.ArmorForBudget(budget))&&CombatNear275(e.NpcMaxHealth,NpcCombat275.HealthForBudget(budget)),"2745 existing endurance unchanged "+kind);G.TickRebel145(e,.01f);G.TickRebel145(e,RebelTuning145.Windup(kind)+.01f);Check(G.ActiveGunShots145==1&&e.RebelBurst==2,"2745 existing first burst remains three rounds "+kind);G.ClearShots();CombatDiscard275(e);}
   var paused=SoldierFixture2745(RebelKind.Marksman);paused.Windup=.8f;float hp=paused.Health,armor=paused.NpcArmorPoints;yield return new WaitForSeconds(.2f);Check(paused.Windup==.8f&&paused.Health==hp&&paused.NpcArmorPoints==armor&&G.ActiveGunShots145==0,"2745 paused gameplay freezes new troop combat");CombatDiscard275(paused);
   SoldierPreview2745.Apply(G);G.Paused=true;Check(G.Enemies.Count==4&&G.ArmorDummy==null,"2745 preview replaces old actors with exactly four new troops");foreach(var actor in G.Enemies){Check(SoldierProfiles2745.IsNew(actor.Rebel),"2745 preview actor uses new role "+actor.Rebel);Check(!Physics.CheckCapsule(actor.transform.position+Vector3.up*.36f,actor.transform.position+Vector3.up*1.35f,.34f,SceneObstacle.MovementMask,QueryTriggerInteraction.Ignore),"2745 preview guard position unobstructed "+actor.Rebel);if(actor.PatrolTargets!=null)foreach(var p in actor.PatrolTargets)Check(!Physics.CheckCapsule(p+Vector3.up*.36f,p+Vector3.up*1.35f,.34f,SceneObstacle.MovementMask,QueryTriggerInteraction.Ignore)&&G.Navigation.ClearSegment(actor.Home,p),"2745 preview actual patrol corridor clear "+actor.Rebel);}Check(!Physics.CheckCapsule(G.Player.transform.position+Vector3.up*.36f,G.Player.transform.position+Vector3.up*1.35f,.34f,SceneObstacle.MovementMask,QueryTriggerInteraction.Ignore),"2745 preview player spawn unobstructed");foreach(var actor in G.Enemies.ToArray())CombatDiscard275(actor);yield return SoldierPicture2745();G.Controls=new DesktopInput();G.Finish(false);yield return null;Check(JsonUtility.ToJson(G.Save)==inventory,"2745 training restores camp inventory");for(int i=0;i<formal.Length;i++)Check(MainBytes273(before[i],File.Exists(formal[i])?File.ReadAllBytes(formal[i]):null),"2745 formal inventory unchanged "+Path.GetFileName(formal[i]));
  }
 }
}
