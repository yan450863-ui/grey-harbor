using System;
using System.IO;
using System.Collections.Generic;
using UnityEngine;
namespace GreyHarbor {
 public enum CampPanel { None,Warehouse,Departure,Workbench,Market }
 public enum ItemArea { Rig,Backpack,Weapon,Warehouse,Loot,Safe,Head,Armor,RigEquipment,PackEquipment,SecondWeapon,KeyCase314,CorpseWeapon325,CorpseArmor325,CorpseRigGear325,CorpsePackGear325,CorpseRig325,CorpseBag325 }
 public sealed partial class RaidGame {
  public RaidInventory Chest=new RaidInventory(10);public ItemStack Equipped;
  public int MagazineFound,ResultCertified;public bool CampLocked;public CampPanel Panel;
  public float GateTime;bool interactHeld;
  public void LoadSave(){
   try {
    bool isolated=VerifyMode||SavePath.Contains("ui-check");string parent=Path.GetDirectoryName(SavePath),v42=Path.Combine(parent,"stash-v42.json"),v41=Path.Combine(parent,"stash-v41.json"),v40=Path.Combine(parent,"stash-v40.json"),v39=Path.Combine(parent,"stash-v39.json"),v38=Path.Combine(parent,"stash-v38.json"),v37=Path.Combine(parent,"stash-v37.json"),v36=Path.Combine(parent,"stash-v36.json"),v35=Path.Combine(parent,"stash-v35.json"),v34=Path.Combine(parent,"stash-v34.json"),v33=Path.Combine(parent,"stash-v33.json"),v32=Path.Combine(parent,"stash-v32.json"),v31=Path.Combine(parent,"stash-v31.json"),v30=Path.Combine(parent,"stash-v30.json"),v29=Path.Combine(parent,"stash-v29.json"),v28=Path.Combine(parent,"stash-v28.json"),v27=Path.Combine(parent,"stash-v27.json"),v26=Path.Combine(parent,"stash-v26.json"),v25=Path.Combine(parent,"stash-v25.json"),v24=Path.Combine(parent,"stash-v24.json"),v23=Path.Combine(parent,"stash-v23.json"),v22=Path.Combine(parent,"stash-v22.json"),v21=Path.Combine(parent,"stash-v21.json"),v20=Path.Combine(parent,"stash-v20.json"),v19=Path.Combine(parent,"stash-v19.json"),v18=Path.Combine(parent,"stash-v18.json"),v17=Path.Combine(parent,"stash-v17.json"),v16=Path.Combine(parent,"stash-v16.json"),v15=Path.Combine(parent,"stash-v15.json"),v14=Path.Combine(parent,"stash-v14.json"),v13=Path.Combine(parent,"stash-v13.json"),v12=Path.Combine(parent,"stash-v12.json"),v11=Path.Combine(parent,"stash-v11.json"),v10=Path.Combine(parent,"stash-v10.json"),v9=Path.Combine(parent,"stash-v9.json"),v8=Path.Combine(parent,"stash-v8.json"),v7=Path.Combine(parent,"stash-v7.json"),v6=Path.Combine(parent,"stash-v6.json"),v5=Path.Combine(parent,"stash-v5.json"),v4=Path.Combine(parent,"stash-v4.json"),v3=Path.Combine(parent,"stash-v3.json"),v2=Path.Combine(parent,"stash-v2.json"),v1=Path.Combine(parent,"stash-v1.json");
    string path=File.Exists(SavePath)?SavePath:!isolated&&File.Exists(v42)?v42:!isolated&&File.Exists(v41)?v41:!isolated&&File.Exists(v40)?v40:!isolated&&File.Exists(v39)?v39:!isolated&&File.Exists(v38)?v38:!isolated&&File.Exists(v37)?v37:!isolated&&File.Exists(v36)?v36:!isolated&&File.Exists(v35)?v35:!isolated&&File.Exists(v34)?v34:!isolated&&File.Exists(v33)?v33:!isolated&&File.Exists(v32)?v32:!isolated&&File.Exists(v31)?v31:!isolated&&File.Exists(v30)?v30:!isolated&&File.Exists(v29)?v29:!isolated&&File.Exists(v28)?v28:!isolated&&File.Exists(v27)?v27:!isolated&&File.Exists(v26)?v26:!isolated&&File.Exists(v25)?v25:!isolated&&File.Exists(v24)?v24:!isolated&&File.Exists(v23)?v23:!isolated&&File.Exists(v22)?v22:!isolated&&File.Exists(v21)?v21:!isolated&&File.Exists(v20)?v20:!isolated&&File.Exists(v19)?v19:!isolated&&File.Exists(v18)?v18:!isolated&&File.Exists(v17)?v17:!isolated&&File.Exists(v16)?v16:!isolated&&File.Exists(v15)?v15:!isolated&&File.Exists(v14)?v14:!isolated&&File.Exists(v13)?v13:!isolated&&File.Exists(v12)?v12:!isolated&&File.Exists(v11)?v11:!isolated&&File.Exists(v10)?v10:!isolated&&File.Exists(v9)?v9:!isolated&&File.Exists(v8)?v8:!isolated&&File.Exists(v7)?v7:!isolated&&File.Exists(v6)?v6:!isolated&&File.Exists(v5)?v5:!isolated&&File.Exists(v4)?v4:!isolated&&File.Exists(v3)?v3:!isolated&&File.Exists(v2)?v2:!isolated&&File.Exists(v1)?v1:null;
    Save=path!=null?JsonUtility.FromJson<BaseSave>(File.ReadAllText(path)):new BaseSave();if(Save==null)throw new Exception("存档为空");if(path!=null&&!File.ReadAllText(path).Contains("\"EconomyVersion\""))Save.EconomyVersion=0;CampRules.Normalize(Save);CampLocked=false;
    if(!Save.StarterGranted){CampRules.Deposit(Save,ItemKind.AK,2);CampRules.Deposit(Save,ItemKind.GreenAmmo,300);CampRules.Deposit(Save,ItemKind.GreenArmorKit,1);CampRules.Deposit(Save,ItemKind.Scrap,4);CampRules.Deposit(Save,ItemKind.Powder,2);CampRules.Deposit(Save,ItemKind.Cola,2);Save.StarterGranted=true;}
    bool interrupted=Save.InRaid;if(interrupted){Save.AidAvailable=true;Save.CampRig.Clear();Save.CampBag.Clear();Save.Weapon=Save.SecondWeapon=Save.Helmet=Save.Armor=Save.RigGear=Save.PackGear=null;Save.CampMagazine=Save.CampMagazineFound=0;Save.InRaid=false;}
    MigrateBackpackBindings343();if(!WriteSave())CampLocked=true;if(interrupted)Notify("上次未撤离：安全箱与仓库保留，其余随身物资损失。");
   }catch(Exception ex){CampLocked=true;Save=new BaseSave();Notify("存档读取失败，已禁止写入，原文件保留。");Debug.LogWarning(ex.Message);}
  }
  bool WriteSave(){if(CampLocked)return false;try{Directory.CreateDirectory(Path.GetDirectoryName(SavePath));CampRules.SyncStock(Save);string temp=SavePath+".tmp";File.WriteAllText(temp,JsonUtility.ToJson(Save,true));if(File.Exists(SavePath))File.Replace(temp,SavePath,SavePath+".bak");else File.Move(temp,SavePath);return true;}catch(Exception ex){Notify("保存失败，操作已回滚，请检查磁盘。");Debug.LogWarning(ex.Message);return false;}}
  void CaptureCarry(){StoreActiveMagazine314();Save.CampRig=Chest.Snapshot();Save.CampBag=Bag.Snapshot();Save.Weapon=RaidInventory.Copy(Equipped);Save.CampMagazine=Magazine;Save.CampMagazineFound=MagazineFound;Save.CampMagazineAmmo=MagazineAmmo;CaptureGear();}
  void RestoreCarry(){RestoreGear();Chest=RaidInventory.Container(RigGear,true);Chest.Restore(Save.CampRig);Bag=RaidInventory.Container(PackGear,false);Bag.Restore(Save.CampBag);Equipped=RaidInventory.Copy(Save.Weapon);Magazine=Save.CampMagazine;MagazineFound=Save.CampMagazineFound;MagazineAmmo=Save.CampMagazineAmmo;}
  void CaptureDrops(){Save.CampDrops.Clear();foreach(var l in Map.Loot){if(l.Items.Count==0)continue;var d=new CampDrop{Position=l.Position};foreach(var p in l.Items)d.Items.Add(RaidInventory.Copy(p));Save.CampDrops.Add(d);}}
  void RestoreDrops(){foreach(var l in Map.Loot)if(l.View)Destroy(l.View);Map.Loot.Clear();foreach(var d in Save.CampDrops){var l=Map.AddLoot(d.Position,"丢弃物资",ItemKind.Scrap,0,ItemKind.Scrap,0,false);l.Items.Clear();foreach(var p in d.Items)l.Items.Add(RaidInventory.Copy(p));l.Opened=l.Known=true;l.PlayerDrop314=true;l.Search=1.1f;}}
  public bool CampChange(Func<bool> change,string success){
   if(CampLocked||Phase!=GamePhase.Base)return false;BeginDropMutation314();CaptureCarry();CaptureDrops();string before=JsonUtility.ToJson(Save);
   long priorWeight320=CarryGrams320;string previousNotice=Notice;bool changed=false;try{changed=change();if(changed&&!WeightMutationAllowed320(priorWeight320))changed=false;}catch(Exception ex){Debug.LogWarning(ex.Message);Notify(ex.Message);}if(!changed){Save=JsonUtility.FromJson<BaseSave>(before);RestoreCarry();RestoreDrops();OpenLoot=null;if(Notice==previousNotice)Notify("条件不足，请检查数量、空间或工作台等级。");return false;}
   CancelBackpackBindings343(Save.CampBag);CaptureCarry();CaptureDrops();if(!WriteSave()){Save=JsonUtility.FromJson<BaseSave>(before);RestoreCarry();RestoreDrops();OpenLoot=null;return false;}CommitDropViews314();if(!string.IsNullOrEmpty(success))Notify(success);return true;
  }
  public void BuildCamp(){
   ClearQuestRaid319();ResetSprint2711();ResetPlayerRecovery321();
   CancelArmorRepair();ClearInteriorSession();
   if(Vision!=null)Vision.Dispose();if(Map!=null&&Map.Root){Map.Root.gameObject.SetActive(false);Destroy(Map.Root.gameObject);}ClearShots();Enemies.Clear();
   Map=new RaidMap();Map.BuildCampMap();Phase=GamePhase.Base;Player=Actor("Operator",RaidMap.CampSpawn,false);Rig.ResetView(0);Rig.Target=Map.Root;View.orthographicSize=9.3f;Rig.Tick(0);
   RestoreCarry();RestoreDrops();Physics.SyncTransforms();Vision=new RaidVision();Navigation=null;GateTime=0;Panel=CampPanel.None;InventoryOpen=Paused=false;OpenLoot=Nearby=null;ReloadTime=HealTime=ExtractTime=0;TrainingDummies315.Clear();MenuCamp315=true;CampPage315=CampPage315.Home;InventoryOpen=true;GetComponent<RaidUI>()?.ClearNavigation315();
  }
  public void TickCamp(float dt){
   bool f=Controls.Interact,pressed=f&&!interactHeld;interactHeld=f;
   if(Controls.Pause){if(Paused)Paused=false;else if(GetComponent<RaidUI>()?.Back315()==true){}else if(MenuCamp315)Paused=true;else if(InventoryOpen)CloseInventory();else OpenCampPage315(CampPage315.Home);}if(Paused)return;
   if(MenuCamp315){Player.Move(Vector3.zero);Rig.Tick(0);return;}
   if(Controls.Inventory){if(InventoryOpen)CloseInventory();else{InventoryOpen=true;Panel=CampPanel.None;OpenLoot=null;}}
   if(InventoryOpen){Player.Move(Vector3.zero);Rig.Tick(0);return;}Rig.Turn(Controls.Turn);Rig.Tick(dt);UpdateAim();Vector2 movement=Vector2.ClampMagnitude(Controls.Move,1);Player.Move(PlayerWalkVelocity2751(movement)*dt);Rig.Tick(0);UpdateAim();Physics.SyncTransforms();
   TickGate(dt);Nearby=null;foreach(var l in Map.Loot)if(l.Items.Count>0&&Vector3.Distance(l.Position,Player.transform.position)<1.8f){Nearby=l;break;}
   if(pressed){if(Vector3.Distance(Player.transform.position,RaidMap.Warehouse)<2){Panel=CampPanel.Warehouse;InventoryOpen=true;}else if(Vector3.Distance(Player.transform.position,RaidMap.Workbench)<2){Panel=CampPanel.Workbench;InventoryOpen=true;}else if(Vector3.Distance(Player.transform.position,RaidMap.Market)<2){Panel=CampPanel.Market;InventoryOpen=true;OpenLoot=null;}else if(Nearby!=null){OpenLoot=Nearby;InventoryOpen=true;Panel=CampPanel.None;}}
  }
  public void TickGate(float dt){var d=Player.transform.position-RaidMap.Door;bool inside=Mathf.Abs(d.x)<1.45f&&Mathf.Abs(d.z)<.95f;if(!inside){GateTime=0;return;}GateTime+=dt;if(GateTime>=3){GateTime=0;OpenCampPage315(CampPage315.Departure);}}
  public void CloseInventory(){if(Phase==GamePhase.Base&&MenuCamp315){OpenCampPage315(CampPage315.Home);return;}CloseKeyCase314();ResetIdentification();bool departure=Panel==CampPanel.Departure;InventoryOpen=false;Panel=CampPanel.None;OpenLoot=null;if(departure){Player.transform.position=RaidMap.CampSpawn;Rig.Tick(0);GateTime=0;}}
  // Kept only for old verification source compatibility; removed from gameplay.
  public void QuickPrepare(){Notify("补齐配装功能已移除。");}
  public void EmergencyAid(){Notify("应急补给功能已移除。");}
  public bool Craft(int index,bool useCertified,int batches=1,bool autoProcess=false,bool useRarePowder=false){return CampChange(()=>{if(Panel!=CampPanel.Workbench||index<0||index>=CampRules.Recipes.Length)return false;if(!AmmoCrafting328.TryPlan(Save,CampRules.Recipes[index],batches,useCertified,autoProcess,useRarePowder,out var planned,out var reason)){Notify(reason);return false;}Save.Warehouse=planned.Snapshot();CampRules.SyncStock(Save);return true;},"制作完成，产物与剩余组件已入仓。");}
  public void UpgradeCamp(){CampChange(()=>{if(Panel!=CampPanel.Workbench||Save.CampLevel>=2)return false;if(!NpcProduction328.CanUpgrade(Save,Save.CampLevel+1,out var reason)){Notify(reason);return false;}if(!CampRules.Spend(Save,CampRules.UpgradeCosts(Save.CampLevel),true)){Notify("升级材料不足，只收已认证收藏品");return false;}Save.CampLevel++;return true;},"工作台升级完成。");}
  bool Depart(){
   if(IsLoading2721||CampLocked||Phase!=GamePhase.Base||Equipped==null){Notify("请先在仓库取出并装备武器。");return false;}

   if(CarryGrams320>CarryWeight320.LimitGrams){Notify("超过45kg，先卸下部分物资再出发。");return false;}
   var prohibited=CarriedCollection319();if(prohibited!=null){Notify("收藏品不能带入战局，请卸回仓库："+RaidInventory.Name(prohibited.Kind));return false;}
   CaptureCarry();string before=JsonUtility.ToJson(Save);CertifyGear();ResetRaidFound();Certification319.Normalize(Equipped);Save.SafeItems=Safe.Snapshot();
   // Departure carries the exact magazine; loading is an explicit inventory/reload action.
   MagazineFound=0;StoreActiveMagazine314();
   var carry=Bag;var rig=Chest;var gun=Equipped;int mag=Magazine;var ammoKind=MagazineAmmo;
   Save.CampBag.Clear();Save.CampRig.Clear();Save.Weapon=Save.SecondWeapon=Save.Helmet=Save.Armor=Save.RigGear=Save.PackGear=null;Save.CampMagazine=Save.CampMagazineFound=0;Save.InRaid=true;
   if(!WriteSave()){Save=JsonUtility.FromJson<BaseSave>(before);RestoreCarry();return false;}
   if(VerifyMode){BuildRaid();CompleteDeparture2721(carry,rig,gun,mag,ammoKind);}else StartCoroutine(RunMapLoad2721(()=>CompleteDeparture2721(carry,rig,gun,mag,ammoKind),()=>{Save=JsonUtility.FromJson<BaseSave>(before);RestoreCarry();WriteSave();BuildCamp();}));return true;
  }
  public void Finish(bool success){
   if(Phase!=GamePhase.Raid)return;if(TrainingActive){EndTraining();return;}string before=JsonUtility.ToJson(Save);int value=0,certified=0;
   if(success){Chest.Certify();Bag.Certify();Safe.Certify();CertifyGear();if(Equipped!=null){Certification319.Normalize(Equipped);}MagazineFound=Magazine;CaptureCarry();value=Bag.TotalValue+Chest.TotalValue+(Equipped!=null?RaidInventory.Value(Equipped):0);foreach(var s in Bag.Slots)if(Certification319.Collection(s.Kind))certified+=s.Count;foreach(var s in Chest.Slots)if(Certification319.Collection(s.Kind))certified+=s.Count;Save.SuccessfulRaids++;Save.TotalValue+=value;}
   else {Save.AidAvailable=true;Save.CampRig.Clear();Save.CampBag.Clear();Save.Weapon=Save.SecondWeapon=Save.Helmet=Save.Armor=Save.RigGear=Save.PackGear=null;Save.CampMagazine=Save.CampMagazineFound=0;}
   Save.SafeItems=Safe.Snapshot();
   Safe.Certify();Save.SafeItems=Safe.Snapshot();FinishQuests319(success);Save.InRaid=false;if(!WriteSave()){Save=JsonUtility.FromJson<BaseSave>(before);Paused=true;Notify("结算保存失败，暂停保留本局；恢复后重试。");return;}
   ResultValue=value;ResultCertified=certified;BuildCamp();Notify(success?"撤离成功！物资仍在身上，可在仓库整理或卸下物资。":"行动结束，安全箱物资已保留，其余随身物资损失。");NoticeTime=9;
  }
 }
}
