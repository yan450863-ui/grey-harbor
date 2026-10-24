using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
namespace GreyHarbor {
 public sealed partial class RaidGame {
  public bool IsLoading2721 {get;private set;}
  public string LoadingStage2721 {get;private set;}="准备地图";
  public double LastMapLoadMilliseconds2721 {get;private set;}
  public double CampBootMilliseconds2721 {get;private set;}
  public readonly List<string> LoadingTimings2721=new List<string>();
  readonly Stopwatch bootWatch2721=Stopwatch.StartNew();
  void CampBootReady2721(){CampBootMilliseconds2721=bootWatch2721.Elapsed.TotalMilliseconds;UnityEngine.Debug.Log("BOOT2721 camp-ready ms="+CampBootMilliseconds2721.ToString("F2")+" harborGenerated="+Map.HarborWhitebox);}
  IEnumerable<MapBuildStep2721> RaidBuildSteps2721(){
   yield return new MapBuildStep2721("准备地图",()=>{
    ResetSprint2711();ResetPlayerRecovery321();ClearInteriorSession();if(Vision!=null)Vision.Dispose();
    if(Map!=null&&Map.Root){Map.Root.gameObject.SetActive(false);Destroy(Map.Root.gameObject);}
    ClearShots();Enemies.Clear();ActiveHardMode=HardMode;Map=new RaidMap{HardLoot=ActiveHardMode};RaidSeed=UnityEngine.Random.Range(1,int.MaxValue);
   });
   if(TrainingActive&&NextMap==RaidDestination.TransitCenter)yield return new MapBuildStep2721("准备转运中心一楼",()=>Map.BuildTransitWhitebox346());
   else if(AssetShowcaseMode2717)yield return new MapBuildStep2721("准备素材展示区",()=>Map.BuildAssetShowcase2717());
   else if(HarborWalkthrough||(!TrainingActive&&!(VerifyMode&&VerificationSmallMap))){foreach(var step in Map.HarborBuildSteps2721(RaidSeed))yield return step;}
   else if(TrainingActive)yield return new MapBuildStep2721("准备训练营",()=>{if(ModRange315)Map.BuildModRange315();else Map.BuildTrainingExterior();});
   else yield return new MapBuildStep2721("准备测试地图",()=>Map.Build());
   yield return new MapBuildStep2721("准备人物与物资",()=>{
    SetGalleryLighting2717(Map.AssetShowcase2717);Player=Actor("Operator",Map.Spawn,false);Rig.Target=Player.transform;Rig.ResetView(45);Rig.Tick(0);
    if(!Map.TransitWhitebox346&&!Map.AssetShowcase2717&&!ModRange315){if(Map.HarborWhitebox){PopulateRepairExteriorEnemies250();PopulateEntranceMonsters();PopulateYardOutdoorMonsters140();PopulateDockMonsters141();PopulateFreightEnemies2720();PopulateUnloadingEnemies2725();}else if(Map.ExteriorPreview)PopulateTrainingMonsters();else PopulateMonsters();}
    InitializeLootGrids();Bag=new RaidInventory();Magazine=MagazineFound=0;
    CancelArmorRepair();ReloadTime=HealTime=ExtractTime=RaidTime=0;Kills=ResultValue=0;OpenLoot=Nearby=null;InventoryOpen=Paused=false;nextShot=0;ResetAkHandling276();Physics.SyncTransforms();
   });
   yield return new MapBuildStep2721("准备附近通行与视野",()=>{
    Navigation=new RaidNavigation(Map.Bounds,Map.IsHarbor?.75f:.5f,Map.HarborWhitebox?new Func<Vector3,bool>(Map.OnHarborSurface):null);
    Vision=new RaidVision(Map.Bounds);if(Map.ExteriorPreview||Map.HarborWhitebox)Vision.RevealExterior();UpdateAim();
    if(Map.TransitWhitebox346)Vision.Refresh(Player.transform.position,Player.Aim);else Vision.RevealSpawn(Player.transform.position,Player.Aim);
   });
   if(Map.HarborWhitebox)yield return new MapBuildStep2721("准备港口地图",()=>Map.HarborPlanTexture());
  }
  bool PrepareTraining2721(){
   if(IsLoading2721||Phase!=GamePhase.Base||CampLocked)return false;CaptureCarry();CaptureDrops();if(!WriteSave())return false;
   trainingSnapshot=JsonUtility.ToJson(Save);TrainingActive=true;return true;
  }
  void CompleteTraining2721(){
   Equipped=new ItemStack(ItemKind.AK,1);RigGear=new ItemStack(ItemKind.StandardRig,1);PackGear=new ItemStack(ItemKind.StandardPack,1);Helmet=SecondWeapon=null;Armor=new ItemStack(ItemKind.BlueArmor,1);Safe=new RaidInventory(2,Save.Npc319.SafeRows);Chest=new RaidInventory(10);Chest.Add(ItemKind.GreenAmmo,170);Chest.Add(ItemKind.GreenArmorKit,1);Magazine=30;MagazineFound=0;MagazineAmmo=ItemKind.GreenAmmo;Bag=new RaidInventory(20);Bag.Add(ItemKind.ArmorKit,1);if(!Map.TransitWhitebox346&&!HarborWalkthrough&&!AssetShowcaseMode2717){CreateArmorDummy();CreateTrainingDummies315();}Phase=GamePhase.Raid;Panel=CampPanel.None;View.orthographicSize=RaidCamera.GameplaySize;
   if(ModRange315&&RangeGun315!=null){EquipTrainingWeapon301(RangeGun315);SelectTrainingAmmo301(RangeGun315.Loaded314>0&&WeaponRules279.Compatible(RangeGun315.Kind,RangeGun315.LoadedAmmo314)?RangeGun315.LoadedAmmo314:WeaponRules279.GreenAmmoFor(RangeGun315.Kind));}
   NoticeTime=0;
  }
  public IEnumerator BeginTrainingLoad2721(){
   if(!PrepareTraining2721())yield break;
   yield return RunMapLoad2721(CompleteTraining2721,()=>{
    Save=JsonUtility.FromJson<BaseSave>(trainingSnapshot);TrainingActive=false;trainingSnapshot=null;AssetShowcaseMode2717=false;HarborWalkthrough=false;ModRange315=false;RangeGun315=null;BuildCamp();GetComponent<RaidUI>()?.RestoreRangePage315();
   });
  }
  public bool RequestRaidLoad2721(){
   if(IsLoading2721||Phase!=GamePhase.Base)return false;
   if(NextMap==RaidDestination.Training||NextMap==RaidDestination.TransitCenter){StartCoroutine(BeginTrainingLoad2721());return IsLoading2721;}
   return Depart();
  }
  void CompleteDeparture2721(RaidInventory bag,RaidInventory chest,ItemStack gun,int magazine,ItemKind ammo){
   Bag=bag;Chest=chest;Equipped=gun;Magazine=magazine;MagazineFound=0;MagazineAmmo=ammo;Panel=CampPanel.None;Phase=GamePhase.Raid;ResultCertified=0;View.orthographicSize=RaidCamera.GameplaySize;
   StartQuestRaid319();NoticeTime=0;
  }
  IEnumerator RunMapLoad2721(Action complete,Action rollback){
   IsLoading2721=true;LoadingTimings2721.Clear();var watch=Stopwatch.StartNew();int mask=View.cullingMask;View.cullingMask=0;View.backgroundColor=PixelArt.Hex("152126");Exception failure=null;
   var steps=RaidBuildSteps2721().GetEnumerator();
   while(true){MapBuildStep2721 step=null;bool more=false;
    try{more=steps.MoveNext();if(more)step=steps.Current;}catch(Exception e){failure=e;}
    if(failure!=null||!more)break;
    LoadingStage2721=step.Name;yield return null;yield return null;
    var stageWatch=Stopwatch.StartNew();try{step.Build();}catch(Exception e){failure=e;}
    string row=step.Name+","+stageWatch.Elapsed.TotalMilliseconds.ToString("F3",System.Globalization.CultureInfo.InvariantCulture);LoadingTimings2721.Add(row);UnityEngine.Debug.Log("LOAD2721 "+row);
    if(failure!=null)break;
   }
   steps.Dispose();
   try{if(failure==null)complete();else rollback();}catch(Exception e){failure=e;}
   LastMapLoadMilliseconds2721=watch.Elapsed.TotalMilliseconds;View.cullingMask=mask;IsLoading2721=false;LoadingStage2721=failure==null?"地图已就绪":"地图加载失败";
   if(failure!=null){Notify("地图加载失败，已返回营地。请查看加载日志。");UnityEngine.Debug.LogException(failure);}
   UnityEngine.Debug.Log("LOAD2721 complete ms="+LastMapLoadMilliseconds2721.ToString("F2")+" success="+(failure==null));
  }
 }
 public sealed partial class RaidUI {
  void DrawMapLoading2721(){
   Fill(new Rect(0,0,1600,900),new Color(.04f,.065f,.075f,1));
   Label(350,310,900,70,"正在加载地图",32,white,TextAnchor.MiddleCenter);
   Label(350,395,900,48,G.LoadingStage2721,21,mint,TextAnchor.MiddleCenter);
   Label(350,477,900,42,"准备完成后自动进入",16,muted,TextAnchor.MiddleCenter);
   float x=550+Mathf.PingPong(Time.realtimeSinceStartup*90,380);Fill(new Rect(550,460,500,4),muted*.35f);Fill(new Rect(x,460,120,4),mint);
  }
 }
}
