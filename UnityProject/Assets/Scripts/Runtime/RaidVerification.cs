using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
namespace GreyHarbor {
 public sealed partial class RaidVerification:MonoBehaviour {
  readonly List<string> results=new List<string>();string folder;RaidGame G=>RaidGame.Instance;
  void Check(bool pass,string message){results.Add((pass?"PASS ":"FAIL ")+message);Debug.Log(results[results.Count-1]);if(!pass)throw new Exception(message);}
  void FreshCamp(){G.VerificationSmallMap=true;G.NextMap=RaidDestination.Harbor;G.Save=new BaseSave{StarterGranted=true};CampRules.Normalize(G.Save);G.CampLocked=false;foreach(var k in new[]{ItemKind.AK,ItemKind.Ammo,ItemKind.GreenAmmo,ItemKind.GreenArmorKit})CampRules.Deposit(G.Save,k,k==ItemKind.AK?3:AmmoRules270.IsAmmo(k)?300:2);G.BuildCamp();G.MenuCamp315=false;G.Panel=CampPanel.Warehouse;G.InventoryOpen=true;CampRules.Deposit(G.Save,ItemKind.StandardRig,1);CampRules.Deposit(G.Save,ItemKind.StandardPack,1);CampRules.Deposit(G.Save,ItemKind.BlueArmor,1);foreach(var k in new[]{ItemKind.StandardRig,ItemKind.StandardPack,ItemKind.BlueArmor,ItemKind.AK}){var p=G.Save.Warehouse.Find(x=>x.Kind==k);G.UseItem(ItemArea.Warehouse,p.Cell);}G.Chest.Add(ItemKind.GreenAmmo,90);G.Chest.Add(ItemKind.GreenArmorKit,1);}
  void StandardRaid(){FreshCamp();if(!G.StartRaid())throw new Exception("fixture cannot depart");G.Paused=true;}
  IEnumerator LegacyStart(){
   folder=Path.GetFullPath(Path.Combine(Application.dataPath,"../../QA"));Directory.CreateDirectory(folder);Application.logMessageReceived+=OnLog;yield return new WaitForSeconds(.5f);
   FreshCamp();Check(G.Phase==GamePhase.Base&&G.Enemies.Count==0,"Camp is a safe separate map without enemies");Check(G.Chest.SlotCapacity==10&&G.Bag.SlotCapacity==20,"Chest rig is 2x5 and backpack is 4x5");
   Check(G.Equipped!=null&&G.Chest.Count(ItemKind.Ammo)==90&&G.Chest.Count(ItemKind.Medkit)==2,"Warehouse quick loadout equips AK and places consumables in rig");
   G.CloseInventory();G.Player.transform.position=RaidMap.Door;G.TickGate(2.9f);Check(!G.InventoryOpen,"Door requires full three second dwell");G.Player.transform.position=RaidMap.CampSpawn;G.TickGate(.1f);Check(G.GateTime==0,"Leaving door cancels dwell countdown");G.Player.transform.position=RaidMap.Door;G.TickGate(3.01f);Check(G.InventoryOpen&&G.Panel==CampPanel.Departure,"Three seconds at door opens loadout rather than starting raid");G.CloseInventory();Check(Vector3.Distance(G.Player.transform.position,RaidMap.Door)>1.5f&&G.GateTime==0,"Cancelling departure returns below gate without retrigger");
   var input=new TouchInput{AimPosition=new Vector2(Screen.width*.6f,Screen.height*.55f)};G.Controls=input;G.Player.transform.position=RaidMap.Warehouse;input.Searching=true;G.TickCamp(.01f);Check(G.Panel==CampPanel.Warehouse&&G.InventoryOpen,"F near warehouse opens external container");input.Searching=false;G.TickCamp(.01f);
   int warehouseAmmo=G.Save.Stored[0];G.Panel=CampPanel.Departure;int row=G.WarehouseItems().FindIndex(s=>s.Kind==ItemKind.Ammo);Check(!G.MoveItem(ItemArea.Warehouse,row,ItemArea.Backpack,0)&&G.Save.Stored[0]==warehouseAmmo,"Door loadout cannot remotely take warehouse supplies");G.Panel=CampPanel.Warehouse;
   row=G.WarehouseItems().FindIndex(s=>s.Kind==ItemKind.Ammo);Check(G.MoveItem(ItemArea.Warehouse,row,ItemArea.Backpack,7,30)&&G.Bag.At(7).Count==30,"Warehouse transfer targets exact backpack cell");
   Check(G.SplitItem(ItemArea.Backpack,7)&&G.Bag.At(7).Count==15&&G.Bag.At(0).Count==15,"Right-click split uses a free cell and preserves total quantity");
   Check(G.MoveItem(ItemArea.Backpack,0,ItemArea.Rig,7)&&G.Chest.At(7).Count==15&&G.Bag.At(0)==null,"Drag transfers between bag and rig without compacting cell layout");
   G.Bag.Add(ItemKind.Cola,2);int cola=G.Bag.Slots.Find(s=>s.Kind==ItemKind.Cola).Cell;Check(G.MoveItem(ItemArea.Backpack,cola,ItemArea.Rig,7)&&G.Chest.At(7).Kind==ItemKind.Cola&&G.Bag.At(cola).Kind==ItemKind.Ammo,"Occupied cells swap stacks without loss");
   int total=G.Chest.Count(ItemKind.Cola)+G.Bag.Count(ItemKind.Cola);Check(G.DropItem(ItemArea.Rig,7)&&G.Map.Loot.Count==1,"Player drag discard creates a ground stack");
   Check(!G.DropItem(ItemArea.Warehouse,0),"Discard rejects warehouse and external-container items");
   G.LoadSave();G.BuildCamp();Check(G.Map.Loot.Count==1&&G.Map.Loot[0].Items[0].Count==total,"Camp ground items persist across restart");
   var ground=G.Map.Loot[0];G.Player.transform.position=ground.Position;G.OpenLoot=ground;G.InventoryOpen=true;Check(G.MoveItem(ItemArea.Loot,0,ItemArea.Backpack,9)&&G.Bag.At(9).Count==total&&ground.Items.Count==0,"Ground items can be picked back up into a selected cell");
   G.Panel=CampPanel.Warehouse;G.Bag.Add(ItemKind.Scrap,3,ItemOrigin.Certified);int stored=G.Save.Stored[2];G.StoreMaterials();Check(G.Save.Stored[2]==stored+3&&G.Bag.Count(ItemKind.Scrap)==0&&G.Chest.Count(ItemKind.Ammo)>0&&G.Equipped!=null,"Store materials leaves equipment and combat supplies on player");
   FreshCamp();G.Bag.Add(ItemKind.Scrap,3);G.Chest.Add(ItemKind.Powder,2);int stashScrap=G.Save.Stored[2];Check(G.StartRaid(),"Prepared player confirms departure into combat map");G.Paused=true;
   Check(G.Magazine==30&&G.MagazineFound==30&&G.Chest.Count(ItemKind.Ammo)==60,"Initial magazine loads from rig and becomes certified");
   Check(G.Bag.Slots.TrueForAll(s=>s.Origin==ItemOrigin.Certified)&&G.Chest.Slots.TrueForAll(s=>s.Origin==ItemOrigin.Certified)&&G.Equipped.Origin==ItemOrigin.Certified,"All carried items become certified when entering raid");
   var disk=JsonUtility.FromJson<BaseSave>(File.ReadAllText(G.SavePath));Check(disk.InRaid&&disk.CampBag.Count==0&&disk.CampRig.Count==0&&(disk.Weapon==null||disk.Weapon.Count==0),"Raid entry clears persisted player items to prevent quit refunds");
   bool lootMarked=true;foreach(var box in G.Map.Loot)foreach(var s in box.Items)lootMarked&=s.Origin==ItemOrigin.Certified;Check(lootMarked,"World loot is certified immediately, with no pending state");
   G.Chest.Slots.Clear();G.Bag.Add(ItemKind.Ammo,30);G.Bag.Add(ItemKind.Medkit,1);G.Magazine=0;G.MagazineFound=0;G.Player.Health=50;G.BeginReload();G.BeginHeal();Check(G.ReloadTime==0&&G.HealTime==0,"Backpack consumables cannot be used by reload or heal shortcuts");
   G.Paused=false;int ammoCell=G.Bag.Slots.Find(s=>s.Kind==ItemKind.Ammo).Cell;Check(G.MoveItem(ItemArea.Backpack,ammoCell,ItemArea.Rig,2),"Backpack ammunition can be manually moved to rig");G.BeginReload();G.TickActions(2.2f);Check(G.Magazine==30&&G.Chest.Count(ItemKind.Ammo)==0,"Reload works once ammo has been moved into rig");
   int medCell=G.Bag.Slots.Find(s=>s.Kind==ItemKind.Medkit).Cell;G.MoveItem(ItemArea.Backpack,medCell,ItemArea.Rig,4);G.BeginHeal();G.TickActions(2.6f);Check(G.Player.Health==95&&G.Chest.Count(ItemKind.Medkit)==0,"Rig medical item is consumed to restore 45 HP");
   G.Bag.Add(ItemKind.Intel,1,ItemOrigin.Certified);G.Finish(true);Check(G.Phase==GamePhase.Base&&G.Save.Stored[2]==stashScrap&&G.Bag.Count(ItemKind.Scrap)==3&&G.Bag.Count(ItemKind.Intel)==1,"Extraction returns to physical camp with loot still in backpack");
   Check(Vector3.Distance(G.Player.transform.position,RaidMap.CampSpawn)<.01f&&!G.InventoryOpen,"Return position is below gate outside departure trigger");
   int carried=G.Bag.TotalValue;G.LoadSave();G.BuildCamp();Check(G.Bag.TotalValue==carried&&G.Magazine==30&&G.Equipped!=null,"Unstored extracted backpack and equipment survive restart");
   int successes=G.Save.SuccessfulRaids;G.Finish(true);Check(G.Save.SuccessfulRaids==successes,"Repeated settlement does not duplicate extracted items");
   G.Panel=CampPanel.Warehouse;int scrapCell=G.Bag.Slots.Find(s=>s.Kind==ItemKind.Scrap).Cell;Check(G.MoveItem(ItemArea.Backpack,scrapCell,ItemArea.Warehouse,0)&&G.Save.Certified[2]==3,"Manual warehouse deposit preserves certified status");
   Check(G.StartRaid(),"Loaded weapon permits next raid without automatic resupply");G.Paused=true;string safe=string.Join(",",G.Save.Stored);G.Finish(false);Check(G.Phase==GamePhase.Base&&G.Equipped==null&&G.Chest.Slots.Count==0&&G.Bag.Slots.Count==0&&G.Magazine==0&&safe==string.Join(",",G.Save.Stored),"Death returns empty-handed to camp and preserves warehouse");
   G.Save=new BaseSave{StarterGranted=true};G.BuildCamp();G.Panel=CampPanel.Warehouse;G.EmergencyAid();G.EmergencyAid();Check(G.Save.Stored[(int)ItemKind.AK]==1&&G.Save.Stored[0]==30,"Emergency support cannot be repeatedly accumulated");
   row=G.WarehouseItems().FindIndex(s=>s.Kind==ItemKind.AK);G.MoveItem(ItemArea.Warehouse,row,ItemArea.Weapon,0);G.DropItem(ItemArea.Weapon,0);G.EmergencyAid();Check(G.Save.Stored[(int)ItemKind.AK]==0,"Dropped camp weapons count against emergency eligibility");
   FreshCamp();G.Panel=CampPanel.Workbench;foreach(var c in CampRules.UpgradeCosts(0))CampRules.Deposit(G.Save,c.Kind,c.Count);G.UpgradeCamp();Check(G.Save.CampLevel==0,"Camp upgrades still reject unmarked materials");foreach(var c in CampRules.UpgradeCosts(0))CampRules.Deposit(G.Save,c.Kind,c.Count,true);G.UpgradeCamp();Check(G.Save.CampLevel==1&&G.Save.Certified[2]==0,"Marked materials upgrade camp and unlock workbench recipe");
   CampRules.Deposit(G.Save,ItemKind.Cola,2);int medStock=G.Save.Stored[1];G.Craft(2,false);Check(G.Save.Stored[1]==medStock+1&&G.Save.Certified[1]==0,"Camp barter outputs are ordinary and remain in warehouse");
   var legacy=JsonUtility.FromJson<BaseSave>("{\"Version\":2,\"StarterGranted\":true,\"Stored\":[90,2,6,1],\"Certified\":[0,0,3,1],\"Prepared\":[{\"Kind\":11,\"Count\":1,\"Origin\":0}]}" );CampRules.Normalize(legacy);Check(legacy.Stored[11]==1&&legacy.Prepared.Count==0&&legacy.Stored[2]==6&&legacy.Certified[2]==3,"Legacy preparation migrates safely back to warehouse with marks preserved");
   string real=G.SavePath;var path=Path.Combine(folder,"save-blocker");File.WriteAllText(path,"intentional test");G.SavePath=Path.Combine(path,"stash.json");G.Panel=CampPanel.Warehouse;int rigCount=G.Chest.Count(ItemKind.Ammo);int cell=G.Chest.Slots.Find(s=>s.Kind==ItemKind.Ammo).Cell;Check(!G.MoveItem(ItemArea.Rig,cell,ItemArea.Backpack,5)&&G.Chest.Count(ItemKind.Ammo)==rigCount&&G.Bag.At(5)==null,"Failed camp save rolls back inventory transfers");G.SavePath=real;
   StandardRaid();G.LoadSave();G.BuildCamp();Check(G.Equipped==null&&G.Bag.Slots.Count==0&&G.Chest.Slots.Count==0&&!G.Save.InRaid,"Reloading an interrupted raid never refunds player inventory");
   StandardRaid();G.Paused=true;G.Player.Health=100;Check(RaidActor.IsBackward(Vector3.left,Vector3.right)&&!RaidActor.IsBackward(Vector3.zero,Vector3.right),"Backward movement classification remains correct");Vector3 original=G.Player.transform.position;G.Player.transform.position=new Vector3(-11.45f,0,0);Physics.SyncTransforms();G.Player.Move(Vector3.left*.5f);Check(G.Player.transform.position.x>-11.6f,"Player cannot walk through warehouse wall");G.Player.transform.position=original;
   foreach(var e in G.Enemies){e.Dead=true;e.HitCollider.enabled=false;}var enemy=G.Enemies[0];enemy.Dead=false;enemy.Health=72;enemy.HitCollider.enabled=true;enemy.transform.position=original+Vector3.right*3;Physics.SyncTransforms();for(int i=0;i<3;i++){G.SpawnShot(G.Player,Vector3.right);G.TickProjectiles(.075f);}Check(enemy.Dead&&G.Kills==1,"Three AK hits still kill a basic enemy");Check(G.Map.Loot.Exists(l=>l.Name=="游荡者残骸"&&l.Items.TrueForAll(s=>s.Origin==ItemOrigin.Certified)),"Enemy drops are marked immediately");
   StandardRaid();yield return VerifyVisionAndRoutes();yield return VerifyPresentation03();yield return VerifyDepthAndMotion04();yield return VerifyWallsAndSpawn05();
   var fullRig=new RaidInventory(10);Check(fullRig.Add(ItemKind.Scrap,101)==1&&fullRig.Slots.Count==10,"Ten rig cells enforce capacity without losing overflow");var fullBag=new RaidInventory(20);Check(fullBag.Add(ItemKind.Scrap,201)==1&&fullBag.Slots.Count==20,"Twenty backpack cells enforce capacity without losing overflow");
   FreshCamp();G.Bag=new RaidInventory(20);G.Bag.Add(ItemKind.Scrap,200);G.Chest.Add(ItemKind.Scrap,1);int fromCell=G.Chest.Slots.Find(s=>s.Kind==ItemKind.Scrap).Cell;Check(!G.MoveItem(ItemArea.Rig,fromCell,ItemArea.Backpack,0)&&G.Chest.At(fromCell).Count==1&&G.Bag.Count(ItemKind.Scrap)==200,"Dropping a stack onto a full matching stack leaves both unchanged");
   FreshCamp();G.Magazine=30;int beforeShot=G.Magazine;G.FirePlayer();Check(G.Magazine==beforeShot,"Camp firing is disabled even with a loaded weapon");G.Magazine=0;G.CloseInventory();yield return null;yield return Capture("20-camp-map.png");
   yield return VerifyHarbor();
   yield return VerifyArt();
   VerifyInventorySearch();
   results.Add("VERIFICATION COMPLETE");File.WriteAllLines(Path.Combine(folder,"verification.txt"),results);Debug.Log("GREY HARBOR VERIFICATION COMPLETE");yield return new WaitForSeconds(.2f);Application.Quit(0);
  }
  void VerifyQuickTransfers(){
   FreshCamp();int row=G.WarehouseItems().FindIndex(s=>s.Kind==ItemKind.Ammo),stock=G.Save.Stored[0],rig=G.Chest.Count(ItemKind.Ammo);
   Check(G.QuickTransfer(ItemArea.Warehouse,row)&&G.Bag.Count(ItemKind.Ammo)==stock&&G.Chest.Count(ItemKind.Ammo)==rig,"Quick transfer takes warehouse items into backpack only");
   int cell=G.Bag.Slots[0].Cell,count=G.Bag.At(cell).Count;
   Check(G.QuickTransfer(ItemArea.Backpack,cell)&&G.Save.Stored[0]==count&&G.Bag.Count(ItemKind.Ammo)==stock-count,"Quick transfer deposits backpack stack into open warehouse");
   G.Panel=CampPanel.None;cell=G.Bag.Slots[0].Cell;int carried=G.Bag.TotalValue;
   Check(!G.QuickTransfer(ItemArea.Backpack,cell)&&G.Bag.TotalValue==carried,"Quick transfer without external container cannot move or discard items");
   G.Panel=CampPanel.Warehouse;G.Bag=new RaidInventory(20);G.Bag.Add(ItemKind.Scrap,199);CampRules.Deposit(G.Save,ItemKind.Scrap,8);row=G.WarehouseItems().FindIndex(s=>s.Kind==ItemKind.Scrap);
   Check(G.QuickTransfer(ItemArea.Warehouse,row)&&G.Bag.Count(ItemKind.Scrap)==200&&G.Save.Stored[2]==7,"Quick transfer fills partial stack and leaves overflow in source");
   Check(!G.QuickTransfer(ItemArea.Warehouse,row)&&G.Bag.Count(ItemKind.Scrap)==200&&G.Save.Stored[2]==7,"Full backpack rejects quick transfer without item loss");
   G.Bag=new RaidInventory(20);G.Bag.Add(ItemKind.Cola,2,ItemOrigin.Certified);var box=G.Map.AddLoot(G.Player.transform.position,"测试箱",ItemKind.Cola,1,ItemKind.Scrap,0,false);box.Items.Clear();box.Items.Add(new ItemStack(ItemKind.Cola,1));box.Opened=true;G.OpenLoot=box;G.Panel=CampPanel.None;cell=G.Bag.Slots[0].Cell;
   Check(G.QuickTransfer(ItemArea.Backpack,cell)&&G.Bag.Slots.Count==0&&box.Items.Count==2&&box.Items[0].Origin==ItemOrigin.Ordinary&&box.Items[1].Origin==ItemOrigin.Certified&&box.Items[1].Count==2,"Quick transfer into loot preserves separate certification stacks");
   Check(G.QuickTransfer(ItemArea.Loot,1)&&G.Bag.Slots[0].Count==2&&G.Bag.Slots[0].Origin==ItemOrigin.Certified&&box.Items.Count==1,"Loot double-click destination is backpack with certification preserved");
   box.Position+=Vector3.right*4;carried=G.Bag.TotalValue;Check(!G.QuickTransfer(ItemArea.Backpack,G.Bag.Slots[0].Cell)&&G.Bag.TotalValue==carried&&box.Items.Count==1,"Out-of-range container rejects quick deposit");box.Position=G.Player.transform.position;
   string real=G.SavePath;G.SavePath=Path.Combine(folder,"save-blocker","stash.json");Check(!G.QuickTransfer(ItemArea.Backpack,G.Bag.Slots[0].Cell)&&G.Bag.TotalValue==carried&&G.Map.Loot[0].Items.Count==1,"Failed save rolls back quick deposit and container contents");G.SavePath=real;
   StandardRaid();G.Paused=false;G.InventoryOpen=true;box=G.Map.AddLoot(G.Player.transform.position,"测试空箱",ItemKind.Scrap,0,ItemKind.Scrap,0,false);box.Items.Clear();box.Opened=true;G.OpenLoot=box;G.Bag.Add(ItemKind.Cola,2,ItemOrigin.Certified);G.Vision.Refresh(G.Player.transform.position,G.Player.Aim);
   Check(G.QuickTransfer(ItemArea.Backpack,G.Bag.Slots[0].Cell)&&box.Items.Count==1&&G.QuickTransfer(ItemArea.Loot,0)&&G.Bag.Count(ItemKind.Cola)==2&&box.Items.Count==0,"Live raid permits backpack-to-empty-box round trip without duplication");G.Paused=true;
  }
        IEnumerator VerifyVisionAndRoutes()
        {
            G.Player.transform.position=new Vector3(-6,0,-11);G.Player.Aim=Vector3.forward;G.Rig.Tick(0);Physics.SyncTransforms();
            G.Vision.Clear();G.Vision.Refresh(G.Player.transform.position,G.Player.Aim);
            Check(G.Vision.CanSee(new Vector3(-6,0,-9)),"Forward cone reveals unobstructed ground");
            Check(!G.Vision.CanSee(new Vector3(-6,0,-13))&&!G.Vision.Explored(new Vector3(-6,0,-13)),"Unseen rear ground stays black");
            G.Player.Aim=Vector3.back;G.Vision.Refresh(G.Player.transform.position,G.Player.Aim);
            Check(!G.Vision.CanSee(new Vector3(-6,0,-9))&&G.Vision.Explored(new Vector3(-6,0,-9)),"Previously seen ground becomes memory without live visibility");
            G.Player.transform.position=new Vector3(-13,0,0);G.Player.Aim=Vector3.right;G.Vision.Clear();G.Vision.Refresh(G.Player.transform.position,G.Player.Aim);
            Check(!G.Vision.CanSee(new Vector3(-10,0,0))&&!G.Vision.Explored(new Vector3(-10,0,0)),"Warehouse wall blocks discovery and live vision");
            var enemy=G.Enemies[0];enemy.transform.position=new Vector3(-10,0,0);yield return null;
            Check(!enemy.VisibleToPlayer&&!enemy.Figure.gameObject.activeSelf,"Enemy behind wall has no body, gun, alert or shadow renderer");
            Vector3 fixedPosition=G.Player.transform.position,fixedAim=G.Player.Aim;int explored=0;foreach(bool s in G.Vision.Seen)if(s)explored++;
            G.Rig.Turn(1);G.Rig.Tick(.3f);G.Vision.Refresh(fixedPosition,fixedAim);int after=0;foreach(bool s in G.Vision.Seen)if(s)after++;
            Check(explored==after,"Changing camera alone does not reveal extra map cells");
            G.Player.transform.position=new Vector3(-2.36f,0,-4.55f);G.Rig.Tick(0);G.Player.Aim=Vector3.forward;G.Vision.Refresh(G.Player.transform.position,G.Player.Aim);
            Shader.SetGlobalFloat("_HarborFogEnabled",1);
            for(int i=0;i<4;i++){G.Rig.Turn(1);G.Rig.Tick(.3f);yield return null;yield return Capture("06-doorway-fog-"+i+".png");}
            Check(!Physics.CheckCapsule(G.Player.transform.position+Vector3.up*.35f,G.Player.transform.position+Vector3.up,.29f,1<<8),"Doorframe corner remains outside solid collision across all four views");
            // Exercise the actual patrol controller for a full minute, not just a path query.
            foreach(var e in G.Enemies)
            {
                e.transform.position=e.Home;e.Route.Clear();e.PatrolIndex=0;e.WaitTime=0;float traveled=0;bool collision=false;
                for(int step=0;step<1200;step++){Vector3 prev=e.transform.position;G.Patrol(e,.05f);traveled+=Vector3.Distance(prev,e.transform.position);if(Physics.CheckCapsule(e.transform.position+Vector3.up*.35f,e.transform.position+Vector3.up,.29f,1<<8))collision=true;}
                Check(traveled>12&&!collision&&e.PatrolIndex>=2,e.name+" patrols multiple waypoints for 60 seconds without wall penetration");
            }
            var path=G.Navigation.FindPath(new Vector3(-8,0,5),new Vector3(-8,0,8));
            Check(path.Count>1,"Navigation finds a detour around stacked crates");
            Vector3 anchor=new Vector3(-8,0,5);bool valid=true;foreach(var p in path){valid&=G.Navigation.ClearSegment(anchor,p);anchor=p;}
            Check(valid,"Every detour segment includes actor-radius obstacle clearance");
            enemy.transform.position=new Vector3(-8,0,5);enemy.Route.Clear();
            for(int i=0;i<400;i++)G.FollowPath(enemy,new Vector3(-8,0,8),1.55f,.05f);
            Check(Vector3.Distance(enemy.transform.position,new Vector3(-8,0,8))<.3f,"Chasing actor actually reaches target behind cover");
            G.Player.transform.position=new Vector3(-5,0,0);G.Rig.Yaw=45;G.Rig.Tick(0);G.Vision.Clear();G.Player.Aim=Vector3.forward;G.Vision.Refresh(G.Player.transform.position,G.Player.Aim);yield return null;yield return Capture("07-live-cone.png");
            G.Player.Aim=Vector3.right;G.Vision.Refresh(G.Player.transform.position,G.Player.Aim);yield return null;yield return Capture("08-memory-and-unknown.png");
        }
        IEnumerator VerifyPresentation03()
        {
            G.Player.transform.position=new Vector3(-11,0,-11);G.Player.Aim=Vector3.forward;G.Vision.Clear();G.Vision.Refresh(G.Player.transform.position,G.Player.Aim);
            foreach(float sign in new[]{-1f,1f})
            {
                Vector3 inside=G.Player.transform.position+Quaternion.Euler(0,sign*34,0)*Vector3.forward*2;
                Vector3 outside=G.Player.transform.position+Quaternion.Euler(0,sign*36,0)*Vector3.forward*2;
                Check(G.Vision.CanSee(inside)&&!G.Vision.CanSee(outside),"70-degree cone boundary on side "+sign);
            }
            G.Player.transform.position=new Vector3(-7,0,-11);G.Player.Aim=Vector3.forward;G.Rig.Yaw=45;G.Rig.Tick(0);
            G.Vision.Clear();G.Vision.Refresh(G.Player.transform.position,G.Player.Aim);
            Vector3 top=new Vector3(-7,1.1f,-8),behind=new Vector3(-7,0,-6);
            Check(G.Vision.SurfaceVisible(top),"Visible crate top is not occluded by its own interior");
            Check(!G.Vision.CanSee(behind)&&!G.Vision.SurfaceVisible(behind)&&!G.Vision.Explored(behind),"Revealing crate surface does not reveal space behind it");
            Check(G.Vision.SurfaceSeen[G.Vision.Index(top)],"Crate surface has exploration memory");
            bool opaque=true;foreach(var r in G.Map.Occluders)opaque&=r.material.color.a==1;
            Check(opaque,"All walls stay opaque with no player transparency cutout");
            yield return null;yield return Capture("09-crate-top-visible.png");
            G.Player.Aim=Vector3.back;G.Vision.Refresh(G.Player.transform.position,G.Player.Aim);
            Check(!G.Vision.SurfaceVisible(top)&&G.Vision.SurfaceSeen[G.Vision.Index(top)],"Crate top becomes remembered after looking away");
            yield return null;yield return Capture("10-crate-top-memory.png");
        }
        IEnumerator VerifyDepthAndMotion04()
        {
            G.Phase=GamePhase.Base;G.Paused=true;
            G.Player.transform.position=new Vector3(100,0,100);G.Rig.Yaw=0;G.Rig.Tick(0);G.Player.Draw(G.View,0);
            Shader.SetGlobalFloat("_HarborFogEnabled",0);
            Vector3 head=G.Player.Figure.TransformPoint(new Vector3(0,1.05f,0));
            var baseline=RenderFrame();Vector3 screen=G.View.WorldToScreenPoint(head);int x=Mathf.RoundToInt(screen.x),y=Mathf.RoundToInt(screen.y);Color clear=baseline.GetPixel(x,y);Destroy(baseline);
            var desk=G.Map.Box("DepthTestDesk",new Vector3(100,.5f,100.8f),new Vector3(2,1,.8f),"A99C7A",true);
            var frame=RenderFrame();Color withDesk=frame.GetPixel(x,y);File.WriteAllBytes(Path.Combine(folder,"11-desk-depth.png"),frame.EncodeToPNG());Destroy(frame);
            Check(ColorDistance(clear,withDesk)<.03f,"GPU depth: desk behind character does not cover the helmet");desk.SetActive(false);
            var wall=G.Map.Box("DepthTestWall",new Vector3(100,1.5f,99.2f),new Vector3(2,3,.4f),"A3A799",true);
            frame=RenderFrame();Color withWall=frame.GetPixel(x,y);File.WriteAllBytes(Path.Combine(folder,"12-front-wall-depth.png"),frame.EncodeToPNG());Destroy(frame);
            Check(ColorDistance(clear,withWall)>.1f,"GPU depth: wall in front correctly occludes the helmet without transparency");wall.SetActive(false);
            G.Phase=GamePhase.Raid;G.Player.transform.position=new Vector3(-11,0,-12);G.Rig.Yaw=45;G.Rig.Tick(0);G.Vision.Clear();
            var input=new TouchInput{AimPosition=new Vector2(Screen.width*.65f,Screen.height*.6f)};G.Controls=input;
            bool synchronized=true,stable=true;Vector3 previousAim=Vector3.zero;var clock=new System.Diagnostics.Stopwatch();double totalMs=0;
            for(int i=0;i<120;i++)
            {
                G.Player.Move(Vector3.right*.015f);G.Rig.Tick(0);G.UpdateAim();
                clock.Restart();G.Vision.Refresh(G.Player.transform.position,G.Player.Aim);clock.Stop();totalMs+=clock.Elapsed.TotalMilliseconds;
                Vector4 eye=Shader.GetGlobalVector("_HarborEye");Vector3 p=G.Player.transform.position;
                synchronized&=Mathf.Abs(eye.x-p.x)<.00001f&&Mathf.Abs(eye.y-p.z)<.00001f;
                if(i>0)stable&=Vector3.Angle(previousAim,G.Player.Aim)<.1f;previousAim=G.Player.Aim;
                if(i%30==0){yield return null;yield return Capture("13-walking-"+(i/30)+".png");}
            }
            Check(synchronized,"Moving vision origin matches actor position on every simulated frame");
            Check(stable,"Fixed screen cursor maintains stable aim while camera follows walking actor");
            Debug.Log("VISION REFRESH MEAN MS: "+(totalMs/120).ToString("F3"));
            results.Add("PROFILE vision update mean milliseconds: "+(totalMs/120).ToString("F3"));
        }
        IEnumerator VerifyWallsAndSpawn05()
        {
            StandardRaid();G.Paused=true;
            bool around=true;for(int i=0;i<16;i++){Vector3 p=G.Map.Spawn+Quaternion.Euler(0,i*22.5f,0)*Vector3.forward;around&=G.Vision.Explored(p);}
            Check(around,"Fresh spawn surveys open ground in all 16 directions");
            G.Player.Aim=Vector3.forward;G.Vision.Clear();G.Vision.RevealSpawn(G.Map.Spawn,G.Player.Aim);
            Vector3 rear=G.Map.Spawn+Vector3.back*2;
            Check(G.Vision.Explored(rear)&&!G.Vision.CanSee(rear)&&!G.Vision.Current[G.Vision.Index(rear)],"Spawn rear is remembered ground, not live omnidirectional vision");
            Check(!G.Vision.Explored(new Vector3(-15,0,-11)),"Spawn survey does not reveal ground behind the cargo container");
            Check(!G.Vision.Explored(new Vector3(2,0,-11)),"Spawn survey cannot reveal ground beyond 12 metres");
            var seen=(bool[])G.Vision.Seen.Clone();G.Rig.Turn(1);G.Rig.Tick(.3f);G.Vision.Clear();G.Vision.RevealSpawn(G.Map.Spawn,Vector3.right);
            bool same=true;for(int i=0;i<seen.Length;i++)same&=seen[i]==G.Vision.Seen[i];
            Check(same,"Spawn exploration footprint is independent of initial aim and camera orientation");
            G.Rig.Yaw=45;G.Rig.Tick(0);yield return null;yield return Capture("14-spawn-circle.png");
            G.Player.transform.position=new Vector3(-14,0,0);G.Player.Aim=Vector3.left;G.Rig.Tick(0);G.Vision.Clear();G.Vision.Refresh(G.Player.transform.position,G.Player.Aim);
            bool continuous=true;int samples=0;
            for(float z=-2.5f;z<=2.5f;z+=.025f){continuous&=G.Vision.SurfaceVisible(new Vector3(-18.24f,1.5f,z));samples++;}
            Check(continuous,"Continuous perimeter wall has no visibility gaps across "+samples+" far-face samples");
            yield return null;yield return Capture("15-continuous-wall.png");
        }
        static float ColorDistance(Color a,Color b)=>Mathf.Abs(a.r-b.r)+Mathf.Abs(a.g-b.g)+Mathf.Abs(a.b-b.b);
        Texture2D RenderFrame()
        {
            var target=new RenderTexture(1600,900,24,RenderTextureFormat.ARGB32);
            var previous=RenderTexture.active;var previousTarget=G.View.targetTexture;
            G.View.targetTexture=target;G.View.Render();RenderTexture.active=target;
            var frame=new Texture2D(1600,900,TextureFormat.RGB24,false);frame.ReadPixels(new Rect(0,0,1600,900),0,0);frame.Apply();
            G.View.targetTexture=previousTarget;RenderTexture.active=previous;target.Release();Destroy(target);return frame;
        }
        IEnumerator Capture(string name)
        {
            yield return new WaitForEndOfFrame();
            // Hidden player windows skip swapchain rendering. Render the actual world
            // camera offscreen instead; these captures intentionally exclude IMGUI.
            var target=new RenderTexture(1600,900,24,RenderTextureFormat.ARGB32);
            var previous=RenderTexture.active;var previousTarget=G.View.targetTexture;
            G.View.targetTexture=target;G.View.Render();RenderTexture.active=target;
            var frame=new Texture2D(1600,900,TextureFormat.RGB24,false);
            frame.ReadPixels(new Rect(0,0,1600,900),0,0);frame.Apply();
            File.WriteAllBytes(Path.Combine(folder,name),frame.EncodeToPNG());
            G.View.targetTexture=previousTarget;RenderTexture.active=previous;target.Release();Destroy(target);Destroy(frame);yield return null;
        }
        void OnLog(string message,string trace,LogType type)
        {if(type!=LogType.Exception&&type!=LogType.Error)return;results.Add("ERROR "+message+"\n"+trace);File.WriteAllLines(Path.Combine(folder,"verification.txt"),results);Application.Quit(2);}
    }
}
