using System;
using System.Collections.Generic;
using UnityEngine;
namespace GreyHarbor {
 // Append-only IDs 166..265. Manifest IDs pin save meaning independently of source JSON ordering.
 public static class FormalCollectibles2749 {
  public const int First=166,Count=100;
  public static readonly string[] AssetIds={"OfficeStamp","CarbonInvoiceBundle","BinderTrio","PrinterPaperCarton","PocketDictationRecorder","PortableLabelPrinter","RoutingTerminalBoard","MechanicalTimeClock","OpticalStorageChip","SealedAuditLedger","PrecisionDraftingSet","PortableMicrofilmReader","GoldSealRing","ExecutivePenCase","DeskChronometer","PortCeremonyPlaque","PrototypeCryptoKey","FoundersSealedLedger","MasterArchiveCanister","QuantumSecureModule","ECGContactPack","PerfusionValveRack","MicroneedleLatticeCase","SulfurPrillPouch","IonExchangeCartridge","PhotocatalystCeramicRack","XenoGillFanSample","XenoSporeCrown","XenoTendonLoopRig","ElastomerCoupler","CeramicInsulatorRail","HydraulicAccumulatorCapsule","CrankPencilSharpener2717A","CeramicInkWell2717A","WoodenCalendarBlock2717A","IndexCardCarousel2717A","MailSortingRack2717A","DispatchTubeCapsule2717A","CanvasLunchSatchel2717A","CableTieSpool2717A","ClothDocumentSleeve2717A","ManualTypewriter2717A","RotaryOfficePhone2717A","MechanicalAddingMachine2717A","SlideProjectorCase2717A","FlatbedDocumentScanner2717A","ArticulatedDeskLamp2717A","DeskVentilationFan2717A","TapeEditingSplicer2717A","RibbonCassetteCradle2717A","SwitchboardPatchPanel2717A","PressureChartRecorder2717A","GasLeakInspectionProbe2717A","AmpouleTransportCarrier2717A","XenoCarapaceSegment2717A","VacuumCoffeeFlask2717A","MagnifyingInspectionLamp2717A","PneumaticMessageSelector2717A","StenographKeyMachine2717A","CountingDrumAssembly2717A","CalibrationWeightCase2717A","BlueprintScrollCradle2717A","DeskSpectrumAnalyzer2717A","WaterComparatorKit2717A","PortableDensitometer2717A","XenoSensoryAntennaVial2717A","StapleRemover2717B","TicketPunch2717B","ExpandableReceiptWallet2717B","CottonApronRoll2717B","EnamelSnackTin2717B","TravelSewingKit2717B","CeramicFuseTray2717B","PackingSealCordBundle2717B","HandheldMultimeter2717B","PortableCassettePlayer2717B","DeskIntercomUnit2717B","ThermalReceiptPrinter2717B","DesktopVoltageRegulator2717B","ReelInspectionLoupe2717B","FilmSlideArchiveBox2717B","MagneticEraseWand2717B","SurveyTallyClipboard2717B","AnalogHumidityMeter2717B","AneroidBarometerCase2717B","OilCanPump2717B","CoilResistanceDecadeBox2717B","ThreadGaugeLeafSet2717B","RespiratorFilterPair2717B","XenoEggMembranePod2717B","OpticalBenchPrism2717B","PrecisionMicrometerCase2717B","TelegraphRelayKey2717B","MiniaturePlotterHead2717B","SealedChromatographyColumn2717B","VacuumTubeLogicModule2717B","XenoLuminousLensCapsule2717B","AcousticCalibrationPistonphone2717B","PocketStarNavigationDisc2717B","EngravedPortMasterSextant2717B"};
  static AssetCollectible2717[] records;
  public static bool IsNew(ItemKind k)=>(int)k>=First&&(int)k<First+Count;
  public static AssetCollectible2717 Get(ItemKind k){
   if(!IsNew(k))throw new ArgumentOutOfRangeException(nameof(k));
   if(records==null){var source=new Dictionary<string,AssetCollectible2717>(StringComparer.Ordinal);foreach(var item in AssetCollectibles2717.All)source.Add(item.Id,item);var result=new AssetCollectible2717[Count];for(int i=0;i<Count;i++){if(!source.TryGetValue(AssetIds[i],out result[i]))throw new InvalidOperationException("Missing formal collectible "+AssetIds[i]);CategoryOf(result[i].Category);}records=result;}
   return records[(int)k-First];
  }
  public static ItemKind Kind(string id){int index=Array.IndexOf(AssetIds,id);if(index<0)throw new ArgumentOutOfRangeException(nameof(id));return (ItemKind)(First+index);}
  public static CollectionCategory Category(ItemKind k)=>CategoryOf(Get(k).Category);
  static CollectionCategory CategoryOf(string category){switch(category){
   case "生活":return CollectionCategory.Living;
   case "工业":case "机械":case "物流":return CollectionCategory.ToolsParts;
   case "电子":case "仪表":return CollectionCategory.Electronics;
   case "医疗":return CollectionCategory.MedicalMaterials;
   case "化工":return CollectionCategory.EnergyChemicals;
   case "异种":return CollectionCategory.Biological;
   case "办公":case "资料":return CollectionCategory.Information;
   case "收藏":return CollectionCategory.Collectibles;
   default:throw new InvalidOperationException("Unknown collectible category: "+category);
  }}
  public static int Value(ItemKind k){var d=Get(k);decimal multiplier=d.Cells==1?1m:d.Cells==2?1.8m:d.Cells==3?2.75m:3.7m;return (int)Math.Round(new[]{100,300,1000,3000,20000}[d.Grade]*multiplier);}
  public static string Description(ItemKind k){var d=Get(k);return RaidInventory.CategoryName(Category(k))+"收藏品，占 "+d.Width+"×"+d.Height+" 格，可出售换取 "+Value(k)+" 金币。"+(d.Grade==4?"仅通过保险箱品质表或合法容器额外红品抽奖获得。":"")+(Category(k)==CollectionCategory.MedicalMaterials?"医疗研究材料，不能直接治疗或修复。":"");}
  public static Texture2D Texture(ItemKind k){var d=Get(k);var texture=d.Texture;if(!texture)throw new InvalidOperationException("Missing formal collectible texture: "+d.Resource);return texture;}
 }
}
