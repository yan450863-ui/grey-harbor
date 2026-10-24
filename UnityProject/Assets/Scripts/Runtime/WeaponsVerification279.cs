using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using UnityEngine;
namespace GreyHarbor {
 public sealed partial class RaidVerification {
  readonly List<string> weaponRows279=new List<string>();
  static bool WNear279(float a,float b,float tolerance=.002f)=>Mathf.Abs(a-b)<=tolerance;
  static string WN279(float n)=>n.ToString("0.00000",CultureInfo.InvariantCulture);
  void WeaponsData279(){
   var guns=new[]{ItemKind.AK,ItemKind.MP5,ItemKind.SKS};int[] sell={1040,780,980},buy={1196,897,1127},capacity={30,30,20},stability={60,80,30};float[] damage={18,12,24},pen={10,6,10},interval={.11f,.095f,.34f},reload={2.1f,2,2.4f},range={12,8,56f/3},first={1.2f,1.6f,.35f},max={4.95f,2.7f,1.2f},kick={.85f,.38f,1.1f},recoil={16,7,4};
   Check((int)ItemKind.AK==11&&(int)ItemKind.GreenAmmo==161&&(int)ItemKind.MP5==162&&(int)ItemKind.SKS==163&&(int)ItemKind.Green9mm==164&&(int)ItemKind.Blue9mm==165&&RaidInventory.KindCount==166,"279 Existing IDs stay fixed and four new IDs append");
   for(int i=0;i<guns.Length;i++){
    var p=WeaponRules279.Get(guns[i]);Check(p!=null&&p.Automatic==(i!=2)&&p.MagazineSize==capacity[i]&&p.MovementStability==stability[i]&&WNear279(p.Damage,damage[i])&&WNear279(p.Penetration,pen[i])&&WNear279(p.Interval,interval[i])&&WNear279(p.ReloadSeconds,reload[i])&&WNear279(p.Range,range[i]),"279 Design combat profile "+guns[i]);
    Check(WNear279(p.FirstSpread,first[i])&&WNear279(p.FirstSpread+p.MaxHeat,max[i])&&WNear279(p.FirstKick,kick[i])&&WNear279(p.MaxRecoil,recoil[i]),"279 Independent accuracy and recoil profile "+guns[i]);
    Check(RaidInventory.Value(guns[i])==sell[i]&&MarketRules.UnitPrice(guns[i])==buy[i]&&RaidInventory.Grade(guns[i])==0&&RaidInventory.Group(guns[i])==ItemGroup.Combat&&MarketCategories.Main(guns[i])==0&&MarketCategories.Sub(guns[i])==i&&RaidInventory.Width(guns[i])==(i==1?2:3)&&RaidInventory.Height(guns[i])==2&&MarketRules.Catalog(0).Contains(guns[i]),"279 Green weapon price, dimensions, category and listing "+guns[i]);
   }
   var ammo=new[]{ItemKind.GreenAmmo,ItemKind.Ammo,ItemKind.Green9mm,ItemKind.Blue9mm};int[] sale={15,20,10,13},purchase={18,23,12,15};var carry=new RaidInventory(8,4);var warehouse=new RaidInventory(18,20,true);
   for(int i=0;i<ammo.Length;i++){
    Check(RaidInventory.Value(ammo[i])==sale[i]&&MarketRules.UnitPrice(ammo[i])==purchase[i]&&RaidInventory.Grade(ammo[i])==i%2&&RaidInventory.Width(ammo[i])==1&&RaidInventory.Height(ammo[i])==1&&MarketCategories.Main(ammo[i])==4&&MarketCategories.Sub(ammo[i])==(i<2?0:1),"279 Ammunition price, grade, caliber and footprint "+ammo[i]);
    carry.Add(ammo[i],101);warehouse.Add(ammo[i],301);Check(carry.Slots.FindAll(s=>s.Kind==ammo[i]).Count==2&&carry.Count(ammo[i])==101&&carry.Slots.Exists(s=>s.Kind==ammo[i]&&s.Count==100)&&warehouse.Slots.Exists(s=>s.Kind==ammo[i]&&s.Count==300),"279 Distinct caliber and grade stack100 carried/300 warehouse "+ammo[i]);
    foreach(var gun in guns)Check(WeaponRules279.Compatible(gun,ammo[i])==(gun==ItemKind.MP5?i>=2:i<2),"279 Exact caliber compatibility "+gun+" / "+ammo[i]);
   }
  }
  BaseSave WeaponSaveFixture279(ItemKind? gun,ItemKind ammo,int rounds,int certified){return new BaseSave{Version=8,EconomyVersion=1,GridInitialized=true,GearGranted=true,StarterGranted=true,Weapon=gun.HasValue?new ItemStack(gun.Value,1):null,RigGear=new ItemStack(ItemKind.StandardRig,1),PackGear=new ItemStack(ItemKind.StandardPack,1),CampMagazineAmmo=ammo,CampMagazine=rounds,CampMagazineFound=certified};}
  void WeaponsMigration279(){
   var old=WeaponSaveFixture279(ItemKind.AK,ItemKind.Ammo,27,7);old.Stored=new int[162];old.Certified=new int[162];old.Warehouse.Add(new ItemStack(ItemKind.GreenAmmo,31,ItemOrigin.Certified){Cell=0});old.Warehouse.Add(new ItemStack(ItemKind.AK,1){Cell=1});string id=old.Weapon.Id;CampRules.Normalize(old);
   Check(old.Version==9&&old.Stored.Length==166&&old.Certified.Length==166&&old.Weapon.Id==id&&old.CampMagazine==27&&old.CampMagazineFound==7&&old.CampMagazineAmmo==ItemKind.Ammo&&old.Stored[161]==31&&old.Certified[161]==31&&old.Stored[162]==0&&old.Stored[164]==0,"279 v41 schema migration preserves old identity, quantities and quality without new gifts");
   foreach(var gun in new ItemKind?[]{ItemKind.SKS,ItemKind.MP5,null}){
    var s=WeaponSaveFixture279(gun,ItemKind.GreenAmmo,27,7);CampRules.Normalize(s);int kept=gun==ItemKind.SKS?20:0;
    Check(s.Version==9&&s.CampMagazine==kept&&CampRules.Available(s,ItemKind.GreenAmmo)==27&&s.CampMagazineFound+s.Certified[(int)ItemKind.GreenAmmo]==7&&s.Stored[(int)ItemKind.GreenAmmo]==27-kept,"279 Overflow, wrong-caliber and no-weapon rounds migrate without loss "+gun);
    string snapshot=JsonUtility.ToJson(s);var roundtrip=JsonUtility.FromJson<BaseSave>(snapshot);CampRules.Normalize(roundtrip);Check(JsonUtility.ToJson(roundtrip)==snapshot,"279 v42 normalization and JSON round trip are idempotent "+gun);
   }
   var valid=WeaponSaveFixture279(ItemKind.MP5,ItemKind.Blue9mm,30,9);CampRules.Normalize(valid);Check(valid.CampMagazine==30&&valid.CampMagazineFound==9&&valid.CampMagazineAmmo==ItemKind.Blue9mm,"279 Valid new9mm magazine survives normalization");
   bool rejected=false;try{CampRules.Normalize(WeaponSaveFixture279(ItemKind.SKS,ItemKind.GreenAmmo,-1,0));}catch(InvalidOperationException){rejected=true;}Check(rejected,"279 Negative magazine rejects invalid save instead of discarding rounds");
   string copyPath=Path.Combine(folder,"legacy-v41-test-copy.json");Check(File.Exists(copyPath),"279 Read-only real v41 migration copy exists");byte[] copyBytes=File.ReadAllBytes(copyPath);var real=JsonUtility.FromJson<BaseSave>(System.Text.Encoding.UTF8.GetString(copyBytes).TrimStart('\uFEFF'));string fingerprint=WeaponsSaveFingerprint279(real);CampRules.Normalize(real);var reread=JsonUtility.FromJson<BaseSave>(JsonUtility.ToJson(real));CampRules.Normalize(reread);
   Check(real.Version==9&&WeaponsSaveFingerprint279(real)==fingerprint&&WeaponsSaveFingerprint279(reread)==fingerprint&&MainBytes273(copyBytes,File.ReadAllBytes(copyPath)),"279 Real v41 copy migrates and roundtrips with coins, old IDs, source counts, identity and armor preserved");
  }
  string WeaponsSaveFingerprint279(BaseSave save){
   var rows=new List<string>{"money="+save.Coins+";total="+save.TotalValue+";economy="+save.EconomyVersion,"mag="+(int)save.CampMagazineAmmo+":"+save.CampMagazine+":"+save.CampMagazineFound};
   Action<string,ItemStack> record=(label,p)=>{if(p!=null)rows.Add(label+":"+p.Id+":"+(int)p.Kind+":"+p.Count+":"+p.Origin+":"+p.Identified+":"+p.RaidFound+":"+WN279(p.ArmorPoints)+":"+p.ArmorReady);};
   foreach(var p in save.Warehouse)record("warehouse",p);foreach(var p in save.CampRig)record("rig",p);foreach(var p in save.CampBag)record("bag",p);foreach(var p in save.SafeItems)record("safe",p);foreach(var p in save.Prepared)record("prepared",p);foreach(var d in save.CampDrops)foreach(var p in d.Items)record("drop",p);
   record("weapon",save.Weapon);record("second",save.SecondWeapon);record("armor",save.Armor);record("helmet",save.Helmet);record("rigGear",save.RigGear);record("packGear",save.PackGear);for(int i=0;i<162;i++)rows.Add("old="+i+":"+save.Stored[i]+":"+save.Certified[i]);rows.Sort(StringComparer.Ordinal);return string.Join("\n",rows);
  }
  void WeaponsMarketPrepare279(){
   FreshCamp();G.Save.Coins=10000;G.Player.transform.position=RaidMap.Market;G.Panel=CampPanel.Market;G.InventoryOpen=true;int money=G.Save.Coins;var goods=new[]{ItemKind.MP5,ItemKind.SKS,ItemKind.Green9mm,ItemKind.Blue9mm};int[] counts={1,1,31,31};
   for(int i=0;i<goods.Length;i++){int previous=G.Save.Stored[(int)goods[i]];Check(G.BuyMarket(goods[i],counts[i])&&G.Save.Stored[(int)goods[i]]==previous+counts[i]&&G.Save.Coins==money-MarketRules.Price(goods[i],counts[i]),"279 Actual market purchase persists exact new item quantity and quote "+goods[i]);money=G.Save.Coins;}
   G.Panel=CampPanel.Warehouse;G.Player.transform.position=RaidMap.Warehouse;G.Equipped=new ItemStack(ItemKind.MP5,1);G.Magazine=G.MagazineFound=0;G.MagazineAmmo=ItemKind.Blue9mm;G.Chest.Slots.Clear();G.Chest.Add(ItemKind.Medkit,2);G.Chest.Add(ItemKind.Blue9mm,50);G.Bag.Slots.Clear();CampRules.Deposit(G.Save,ItemKind.Blue9mm,200);int stock=G.Save.Stored[(int)ItemKind.Blue9mm];string id=G.Equipped.Id;G.QuickPrepare();
   Check(G.Equipped.Id==id&&G.Chest.Count(ItemKind.Blue9mm)==200&&G.Save.Stored[(int)ItemKind.Blue9mm]==stock-150&&G.Chest.Count(ItemKind.GreenAmmo)==0&&G.Chest.Count(ItemKind.Green9mm)==0,"279 Actual quick prepare preserves MP5 and selected blue9mm while filling200 rounds");
  }
  void WeaponsDrops279(){
   var military=ContainerDropRules230.Profile(ContainerLook.SupplyLocker,SupplyType.Military,ContainerTier.Common,DropRegion230.Repair);military.FixedCount=1;military.EnableBonusRed=false;ContainerDropRules230.Validate(military);
   var ammo=ContainerDropRules230.Profile(ContainerLook.SupplyLocker,SupplyType.Ammo,ContainerTier.Common,DropRegion230.Repair);ammo.FixedCount=1;ammo.EnableBonusRed=false;ContainerDropRules230.Validate(ammo);
   var ordinary=ContainerDropRules230.Profile(ContainerLook.Toolbox,SupplyType.Tools,ContainerTier.Common,DropRegion230.Repair);
   foreach(var k in new[]{ItemKind.MP5,ItemKind.SKS,ItemKind.Green9mm,ItemKind.Blue9mm})Check(ContainerDropRules230.Eligible(military,k)&&!ContainerDropRules230.Eligible(ordinary,k)&&ContainerDropRules230.Eligible(ammo,k)==AmmoRules270.IsAmmo(k),"279 New items enter only legal combat categories "+k);
   var quality=LootBalance.QualityWeights(ContainerTier.Common);Check(quality[0]==60&&quality[1]==32&&quality[2]==7&&quality[3]==1&&quality[4]==0,"279 Base quality table remains60/32/7/1/0");
   var rng=new System.Random(279);int[] weapons=new int[3],calibers=new int[2],corpse=new int[2];bool legal=true,grades=true;
   for(int i=0;i<30000;i++){
    foreach(var p in ContainerDropRules230.Contents(military,rng,false)){legal&=ContainerDropRules230.Eligible(military,p.Kind);if(WeaponRules279.IsWeapon(p.Kind))weapons[p.Kind==ItemKind.AK?0:p.Kind==ItemKind.MP5?1:2]++;}
    foreach(var p in ContainerDropRules230.Contents(ammo,rng,false)){legal&=AmmoRules270.IsAmmo(p.Kind);calibers[WeaponRules279.AmmoCaliber(p.Kind)==AmmoCaliber279.Seven62?0:1]++;}
    if(i<6000)foreach(var p in EnemyLoot146.Soldier(RebelKind.Regular,rng,false,i%2+1))if(AmmoRules270.IsAmmo(p.Kind)){corpse[WeaponRules279.AmmoCaliber(p.Kind)==AmmoCaliber279.Seven62?0:1]++;grades&=RaidInventory.Grade(p.Kind)==i%2;}
   }
   int total=weapons[0]+weapons[1]+weapons[2],ct=corpse[0]+corpse[1];Check(legal&&grades&&total>1000&&ct>1000,"279 Actual container and corpse rolls are legal with original soldier ammo grade");
   Check(Math.Abs((double)weapons[0]/total-.40)<.035&&Math.Abs((double)weapons[1]/total-.35)<.035&&Math.Abs((double)weapons[2]/total-.25)<.035,"279 Actual military green weapon subdistribution40/35/25");
   Check(Math.Abs((double)calibers[0]/30000-.65)<.02&&Math.Abs((double)corpse[0]/ct-.65)<.035,"279 Actual ammo container and corpse caliber subdistribution65/35");
   File.WriteAllLines(Path.Combine(folder,"279-new-item-drop-distribution.csv"),new[]{"source,AK,MP5,SKS,seven62,nineMM","military,"+string.Join(",",weapons)+",0,0","ammo,0,0,0,"+string.Join(",",calibers),"soldier,0,0,0,"+string.Join(",",corpse)});
  }
  void WeaponsRemote279(){
   G.Paused=true;G.ClearShots();foreach(var enemy in G.Enemies)enemy.Dead=true;foreach(var c in G.Map.Root.GetComponentsInChildren<Collider>())c.enabled=false;
   G.Player.transform.position=new Vector3(500,0,500);G.Player.Health=100;G.Player.Dead=false;G.Rig.ResetView(45);G.Rig.Tick(0);Physics.SyncTransforms();
  }
  void WeaponsAim279(TouchInput input){var p=G.View.WorldToScreenPoint(G.Player.transform.position+Vector3.up*RaidGame.CombatHeight);input.AimPosition=new Vector2(p.x+180,p.y);}
  void WeaponsEquipFixture279(ItemKind kind,TouchInput input){G.Paused=true;G.ClearShots();G.Equipped=new ItemStack(kind,1);G.ResetAkHandling276();G.MagazineAmmo=WeaponRules279.GreenAmmoFor(kind);G.Magazine=WeaponRules279.Get(kind).MagazineSize;G.MagazineFound=0;G.ReloadTime=G.HealTime=0;G.CancelArmorRepair();G.InventoryOpen=false;input.Shooting=false;input.Movement=Vector2.zero;WeaponsAim279(input);G.UpdateAim();}
  IEnumerator WeaponsShot279(ItemKind kind,ItemKind ammo,float distance,bool armor,List<string> rows,TouchInput input,int targetLevel=0){
   WeaponsEquipFixture279(kind,input);G.MagazineAmmo=ammo;yield return new WaitForSeconds(.36f);G.Paused=false;G.FirePlayer();G.Paused=true;Check(G.AkShots276.Count==1,"279 Real single FirePlayer creates "+kind+" projectile");var shot=G.AkShots276[0];var p=WeaponRules279.Get(kind);
   int level=targetLevel==0?RaidInventory.Grade(ammo)+1:targetLevel;float multiplier=RaidInventory.Grade(ammo)>=level-1?1:.8f;var target=RangeTarget278("QA279Target",level);target.NpcArmorPoints=target.NpcMaxArmor=armor?20:0;var point=shot.Origin+shot.Direction*distance;target.transform.position=new Vector3(point.x,0,point.z);Physics.SyncTransforms();G.TickProjectiles(1.5f);
   if(distance>p.Range)Check(target.Health==100&&shot.Ended&&shot.Hit=="RangeEnd"&&WNear279(Vector3.Distance(shot.Origin,shot.Impact),p.Range),"279 Real large-step hard range endpoint and unharmed outside target "+kind+" "+ammo);
   else if(armor)Check(shot.Hit==target.name&&target.Health==100&&WNear279(target.NpcArmorPoints,20-p.Penetration*multiplier),"279 Actual projectile armor-only multiplier "+kind+" "+ammo+" targetLevel="+level);
   else Check(shot.Hit==target.name&&WNear279(target.Health,100-p.Damage*multiplier),"279 Actual projectile health multiplier "+kind+" "+ammo+" targetLevel="+level);
   Check(shot.Weapon==kind&&shot.Ammo==ammo&&Mathf.Abs(shot.Direction.y)<.001f,"279 Fired weapon and ammo snapshot matches planar projectile "+kind);rows.Add(kind+","+ammo+","+level+","+WN279(multiplier)+","+WN279(distance)+","+armor+","+WN279(target.Health)+","+WN279(target.NpcArmorPoints)+","+shot.Hit+","+WN279(Vector3.Distance(shot.Origin,shot.Impact)));G.ClearShots();RangeDiscard278(target);
  }
  IEnumerator WeaponsBallistics279(TouchInput input){
   var rows=new List<string>{"weapon,ammo,targetLevel,expectedGradeMultiplier,targetMetres,armored,remainingHealth,remainingArmor,termination,travelMetres"};
   foreach(var kind in new[]{ItemKind.AK,ItemKind.MP5,ItemKind.SKS})foreach(var ammo in kind==ItemKind.MP5?new[]{ItemKind.Green9mm,ItemKind.Blue9mm}:new[]{ItemKind.GreenAmmo,ItemKind.Ammo}){
    yield return WeaponsShot279(kind,ammo,WeaponRules279.Get(kind).Range-1,false,rows,input);yield return WeaponsShot279(kind,ammo,3,true,rows,input);yield return WeaponsShot279(kind,ammo,WeaponRules279.Get(kind).Range+1,false,rows,input);
   }
   foreach(var kind in new[]{ItemKind.MP5,ItemKind.SKS})foreach(bool blue in new[]{false,true}){var ammo=kind==ItemKind.MP5?(blue?ItemKind.Blue9mm:ItemKind.Green9mm):(blue?ItemKind.Ammo:ItemKind.GreenAmmo);int level=blue?1:2;yield return WeaponsShot279(kind,ammo,3,false,rows,input,level);yield return WeaponsShot279(kind,ammo,3,true,rows,input,level);}
   yield return null;var npcRows=new List<string>{"ammo,targetMetres,stepSeconds,remainingHealth,termination,travelMetres"};RangeNpc278(npcRows);File.WriteAllLines(Path.Combine(folder,"279-independent-npc-shot.csv"),npcRows);File.WriteAllLines(Path.Combine(folder,"279-real-projectile-hits.csv"),rows);
  }
  IEnumerator WeaponsInputTime279(TouchInput input,bool held,float seconds){
   input.Shooting=held;float end=Time.realtimeSinceStartup+seconds;do{WeaponsAim279(input);yield return null;}while(Time.realtimeSinceStartup<end);
  }
  IEnumerator WeaponsWaitShot279(TouchInput input,int count){
   input.Shooting=true;float deadline=Time.realtimeSinceStartup+3;while(G.AkShots276.Count<count){if(Time.realtimeSinceStartup>deadline)throw new Exception("279 Real input shot timeout expected="+count+" actual="+G.AkShots276.Count);WeaponsAim279(input);yield return null;}
  }
  float WeaponsActualRatio279(AkShotSample276 sample)=>sample.MoveDt>0?Mathf.Clamp01(sample.MoveDelta.magnitude/(3.4f*sample.MoveDt)):0;
  void WeaponsRecord279(string name){foreach(var s in G.AkShots276)weaponRows279.Add(name+","+s.Weapon+","+s.Ammo+","+s.Number+","+WN279(s.Clock)+","+WN279(s.Spread)+","+WN279(s.Recoil)+","+WN279(s.MovementExtraSpread)+","+WN279(s.Move)+","+WN279(s.RandomOffset)+","+WN279(s.AdjustedScreen.y-s.RawScreen.y)+","+s.MovementStability+","+WN279(s.MoveDt)+","+WN279(s.MoveDelta.x)+","+WN279(s.MoveDelta.y)+","+WN279(s.MoveDelta.z)+","+WN279(s.MoveDelta.magnitude)+","+WN279(WeaponsActualRatio279(s)));}
  void WeaponsWalkingCheck279(string mode,float firstSpread,float fullSpeedExtra,int stability){
   WeaponsRecord279(mode);File.WriteAllLines(Path.Combine(folder,"279-live-trigger-handling.csv"),weaponRows279);var sample=G.AkShots276[0];float actualRatio=WeaponsActualRatio279(sample);
   string evidence=" Move="+sample.Move.ToString("R",CultureInfo.InvariantCulture)+" actualRatio="+actualRatio.ToString("R",CultureInfo.InvariantCulture)+" Spread="+sample.Spread.ToString("R",CultureInfo.InvariantCulture)+" Extra="+sample.MovementExtraSpread.ToString("R",CultureInfo.InvariantCulture)+" Stability="+sample.MovementStability+" dt="+sample.MoveDt.ToString("R",CultureInfo.InvariantCulture)+" delta="+sample.MoveDelta+" distance="+sample.MoveDelta.magnitude.ToString("R",CultureInfo.InvariantCulture)+" position="+G.Player.transform.position;
   // At very short hidden-window frames, world-coordinate quantization can make
   // displacement differ slightly from ideal speed*dt. Check the measured pipeline.
   Debug.Log("279 Actual walking sample "+mode+evidence);
   Check(sample.Move>.95f&&sample.MoveDt>.00001f&&sample.MoveDelta.magnitude>0&&sample.MovementStability==stability&&WNear279(sample.Move,actualRatio)&&WNear279(sample.MovementExtraSpread,fullSpeedExtra*actualRatio)&&WNear279(sample.Spread,firstSpread+fullSpeedExtra*actualRatio),"279 Real walking displacement drives first-shot spread and movement stability "+mode+evidence);
  }
  IEnumerator WeaponsTrigger279(TouchInput input){
   weaponRows279.Clear();weaponRows279.Add("mode,weapon,ammo,shot,clock,spread,recoil,movementExtra,actualMovement,randomYaw,screenUpPixels,movementStability,moveDt,moveDeltaX,moveDeltaY,moveDeltaZ,moveDistance,recomputedActualRatio");
   WeaponsEquipFixture279(ItemKind.MP5,input);G.Paused=false;yield return WeaponsInputTime279(input,false,.36f);yield return WeaponsWaitShot279(input,25);input.Shooting=false;G.Paused=true;var shots=G.AkShots276;
   bool cadence=true,upward=true,spread=true;float min=100,max=-100;for(int i=0;i<shots.Count;i++){var s=shots[i];if(i>0)cadence&=s.Clock-shots[i-1].Clock>=.094f;upward&=s.Recoil<.001f||s.AdjustedScreen.y>s.RawScreen.y;spread&=Mathf.Abs(s.RandomOffset)<=s.Spread+.002f;min=Mathf.Min(min,s.RandomOffset);max=Mathf.Max(max,s.RandomOffset);}
   Check(shots.Count==25&&G.Magazine==5&&WNear279(shots[0].Spread,1.6f)&&WNear279(shots[24].Spread,2.7f)&&WNear279(G.AkHandling276.Recoil,7)&&cadence&&upward&&spread&&max-min>.5f,"279 Actual MP5 held fire has25 shots, .095 cadence, low recoil, random spread1.6..2.7 and screen-up aim");WeaponsRecord279("MP5_stationary");
   WeaponsEquipFixture279(ItemKind.MP5,input);G.Paused=false;input.Movement=new Vector2(0,1);yield return WeaponsInputTime279(input,false,.36f);yield return WeaponsWaitShot279(input,1);input.Shooting=false;input.Movement=Vector2.zero;G.Paused=true;WeaponsWalkingCheck279("MP5_walking",1.6f,.4f,80);
   WeaponsEquipFixture279(ItemKind.SKS,input);G.Paused=false;yield return WeaponsInputTime279(input,false,.36f);yield return WeaponsWaitShot279(input,1);G.Paused=true;Check(WNear279(G.AkShots276[0].Spread,.35f)&&WNear279(G.AkHandling276.Recoil,1.1f),"279 Actual SKS first shot has .35 precision and1.1 recoil");G.Paused=false;
   yield return WeaponsInputTime279(input,true,.08f);yield return WeaponsInputTime279(input,false,.02f);yield return WeaponsInputTime279(input,true,.5f);
   Check(G.AkShots276.Count==1&&G.Magazine==19,"279 SKS held key and cooldown click do not auto-fire or queue later shot");yield return WeaponsInputTime279(input,false,.03f);yield return WeaponsWaitShot279(input,2);input.Shooting=false;G.Paused=true;
   Check(G.AkShots276.Count==2&&G.AkShots276[1].Clock-G.AkShots276[0].Clock>=.339f,"279 New SKS press after release fires once and keeps .34 cooldown");WeaponsRecord279("SKS_edges");
   foreach(bool inventory in new[]{false,true}){
    WeaponsEquipFixture279(ItemKind.SKS,input);G.Paused=!inventory;G.InventoryOpen=inventory;yield return WeaponsInputTime279(input,true,.1f);G.Paused=false;G.InventoryOpen=false;yield return WeaponsInputTime279(input,true,.4f);
    Check(G.AkShots276.Count==0,"279 SKS press while "+(inventory?"inventory":"paused")+" is swallowed after returning to action");yield return WeaponsInputTime279(input,false,.03f);yield return WeaponsWaitShot279(input,1);input.Shooting=false;G.Paused=true;Check(G.AkShots276.Count==1,"279 Release and new press restores SKS firing after menu");WeaponsRecord279(inventory?"SKS_inventory":"SKS_pause");
   }
   WeaponsEquipFixture279(ItemKind.SKS,input);G.Paused=false;input.Movement=new Vector2(0,1);yield return WeaponsInputTime279(input,false,.36f);yield return WeaponsWaitShot279(input,1);input.Movement=Vector2.zero;input.Shooting=false;G.Paused=true;
   WeaponsWalkingCheck279("SKS_walking",.35f,1.4f,30);
   G.Finish(false);yield return null;yield return WeaponsInputTime279(input,true,.1f);Check(G.BeginTraining(),"279 Begin preview while trigger remains held from camp");G.Paused=true;WeaponsRemote279();WeaponsEquipFixture279(ItemKind.SKS,input);G.Paused=false;yield return WeaponsInputTime279(input,true,.4f);Check(G.AkShots276.Count==0,"279 Camp-to-departure held press cannot become a queued SKS shot");yield return WeaponsInputTime279(input,false,.03f);yield return WeaponsWaitShot279(input,1);input.Shooting=false;G.Paused=true;Check(G.AkShots276.Count==1,"279 Fresh released press fires after actual departure");WeaponsRecord279("SKS_departure");
  }
  IEnumerator WeaponsVisual279(TouchInput input){
   foreach(var gun in new[]{ItemKind.MP5,ItemKind.SKS}){
    WeaponsEquipFixture279(gun,input);G.Player.transform.position=G.Map.Spawn;G.Rig.Tick(0);WeaponsAim279(input);G.UpdateAim();G.Player.Draw(G.View,0);G.Vision.Refresh(G.Player.transform.position,G.Player.Aim);yield return null;yield return new WaitForEndOfFrame();
    Check(PixelArt.WeaponSprite279(gun)!=null&&PixelArt.WeaponSprite279(gun)!=PixelArt.WeaponSprite279(ItemKind.AK)&&PixelArt.WeaponMuzzle279(gun)!=PixelArt.WeaponMuzzle279(ItemKind.AK),"279 New held weapon has its own rendered silhouette and muzzle "+gun);
    Check(G.Player.VisualWeapon279==gun&&G.Player.Gun.sprite==PixelArt.WeaponSprite279(gun)&&Vector3.Distance(G.Player.Muzzle.transform.localPosition,PixelArt.WeaponMuzzle279(gun)+new Vector3(0,0,-.01f))<.002f&&RaidUI.WeaponHudTitle279(G.Equipped).Contains(gun.ToString())&&RaidUI.WeaponHudMode279(gun).Contains(gun==ItemKind.SKS?"半自动":"自动"),"279 Actual held sprite, barrel endpoint and HUD text follow equipped gun "+gun);
    var tex=HvRender277(G.View,"279-"+gun+"-actual-game-camera.png",1600,900);int lit=0;foreach(var c in tex.GetPixels32())if(c.r>10||c.g>10||c.b>10)lit++;Destroy(tex);Check(lit>1440&&WNear279(G.View.orthographicSize,RaidCamera.GameplaySize),"279 Actual current-perspective new weapon image renders without manufactured HUD "+gun);
   }
   var reserve=new RaidInventory(5,2);reserve.Add(ItemKind.GreenAmmo,40);reserve.Add(ItemKind.Green9mm,20);reserve.Add(ItemKind.Blue9mm,10);reserve.Slots.Find(p=>p.Kind==ItemKind.Blue9mm).Identified=false;Check(RaidUI.WeaponHudReserve279(new ItemStack(ItemKind.MP5,1),reserve)==20&&RaidUI.WeaponHudReserve279(new ItemStack(ItemKind.SKS,1),reserve)==40,"279 HUD reserve counts only matching identified ammo");
   foreach(var ammo in new[]{ItemKind.Green9mm,ItemKind.Blue9mm}){var icon=ItemIcons.Get(ammo);Check(icon!=null&&icon!=ItemIcons.Get(ItemKind.Ammo),"279 New9mm has distinct rendered icon "+ammo);File.WriteAllBytes(Path.Combine(folder,"279-"+ammo+"-actual-icon.png"),icon.EncodeToPNG());}
   Check(!MainBytes273(ItemIcons.Get(ItemKind.Green9mm).EncodeToPNG(),ItemIcons.Get(ItemKind.Blue9mm).EncodeToPNG()),"279 Actual green and blue9mm icon pixels visibly differ");
   File.WriteAllText(Path.Combine(folder,"279-image-notes.txt"),"279-*-actual-game-camera.png uses the actual game camera and Camera.Render with real scene/fog. This capture does not include the separate OnGUI HUD; no complete HUD is claimed.\n");
  }
  IEnumerator VerifyWeapons279(){
   string dir=Path.GetDirectoryName(G.SavePath);string[] formal={Path.Combine(dir,"stash-v40.json"),Path.Combine(dir,"stash-v41.json"),Path.Combine(dir,"stash-v42.json")};byte[][] bytes=Array.ConvertAll(formal,p=>File.Exists(p)?File.ReadAllBytes(p):null);var oldInput=G.Controls;var oldRandom=UnityEngine.Random.state;
   Check(Path.GetFileName(G.SavePath)=="verification-stash-2.7.9.json","279 Dedicated isolated new-weapon inventory");WeaponsData279();WeaponsMigration279();WeaponsDrops279();WeaponsMarketPrepare279();FreshCamp();string camp=JsonUtility.ToJson(G.Save);G.HardMode=false;G.HarborWalkthrough=false;Check(G.BeginTraining(),"279 Isolated preview for actual weapon paths");G.Paused=true;yield return null;var input=new TouchInput();
   try {
    G.Controls=input;UnityEngine.Random.InitState(279);WeaponsRemote279();yield return WeaponsBallistics279(input);yield return WeaponsTrigger279(input);WeaponsReloadAndSwap279(input);yield return WeaponsVisual279(input);G.Controls=oldInput;G.Finish(false);yield return null;Check(JsonUtility.ToJson(G.Save)==camp,"279 New-weapon preview restores exact original inventory");
    for(int i=0;i<formal.Length;i++)Check(MainBytes273(bytes[i],File.Exists(formal[i])?File.ReadAllBytes(formal[i]):null),"279 Formal save remains untouched "+Path.GetFileName(formal[i]));
   }finally{G.Controls=oldInput;UnityEngine.Random.state=oldRandom;G.Paused=true;File.WriteAllLines(Path.Combine(folder,"279-live-trigger-handling.csv"),weaponRows279);}
  }
 }
}
