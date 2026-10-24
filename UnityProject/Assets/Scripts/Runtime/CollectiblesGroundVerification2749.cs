using System;using System.IO;using System.Collections;using System.Collections.Generic;using UnityEngine;
namespace GreyHarbor {public sealed partial class RaidVerification {
 void FormalCollectibleConsumers2749(){
  Check(FormalCollectibles2749.First==166&&FormalCollectibles2749.Count==100&&RaidInventory.KindCount==266,"2749 exactly one hundred append-only formal collectible IDs");
  string[] legacy="Ammo|Medkit|Scrap|Circuit|GunParts|Cola|Powder|Cloth|Medical|Tools|Intel|AK|Biomass|Hide|Chitin|Helmet|Armor|SmallRig|StandardRig|LargeRig|SmallPack|StandardPack|LargePack|BlueHelmet|BlueArmor|HarborCoin|BronzeStatuette|SecretArchive|ArmorKit|GreenMedkit|PurpleMedkit|GreenArmorKit|CoffeeTin|SoapBox|VacuumFlask|PocketRadio|VintageCamera|RareTea|CellarWine|Bearing|CopperWire|Caliper|PrecisionMotor|AlloyCutter|NanoTool|ResistorPack|Fuse|Relay|MemoryStick|ControlBoard|Processor|EncryptedDrive|QuantumCore|CottonRoll|GlassVials|SterileSyringe|Reagent|CultureDish|LabEnzyme|SurgicalLens|PrototypeSerum|Charcoal|Solvent|Lubricant|BatteryCell|Resin|Catalyst|FuelCell|FusionSample|BoneFragment|AlienGland|NerveBundle|CrystalCarapace|DormantEgg|QueenCore|ShippingReceipt|CampNotes|RouteMap|AccessLog|ResearchNotes|SurveyData|EvacuationPlan|OldBadge|ShellPendant|SilverRing|MusicBox|GoldWatch|JadeSeal|HarborDiamond|TravelCanteen|FoldingUmbrella|VintageDeskLamp|TorqueWrench|HydraulicJack|ReductionGearAssembly|PowerAdapterSet|IndustrialInverter|UpsPowerModule|InstrumentSterilizationBox|LaboratoryMicroscope|PortableCentrifuge|RefrigerantCylinder|IndustrialBattery|HighPressureHydraulicPump|IntactXenoLimb|LargeCarapacePlate|SealedTissueJar|DocumentTube|EngineeringAtlas|ArchiveBriefcase|VintageShipModel|BrassTelescope|MechanicalMantelClock|VacuumTumbler|PortableElectricFan|VinylRecordPlayer|AdjustableWrenchSet|BenchVise|PrecisionGearbox|Multimeter|CommunicationBaseModule|IndustrialServerModule|BloodSampleTransportCase|SurgicalInstrumentTray|AutomaticAnalyzer|SmallGasCanister|ChemicalRawMaterialDrum|HighVoltageStorageModule|XenoSpine|XenoCompoundEye|IntactXenoSkull|EncryptedCommunicationRecord|HeavyLedger|BlackBoxRecorder|CeramicFigurine|AntiqueGlobe|BrassMarineCompass|CompressedBiscuits|PocketFlashlight|MechanicalWristwatch|MetalWasherSet|SmallRatchetWrench|PrecisionShaftBushing|DiodePacket|PhotoelectricSensor|MicroServoModule|MedicalTapeRoll|SamplePreservationFluid|RapidTestStrip|GraphiteRod|IndustrialDye|RareMetalPowder|XenoScale|XenoTentacleNodule|LuminousTissueChunk|WorkIdentityCard|EncryptedUsbDrive|ExperimentalDataChip|CopperCufflinks|SilverPendant|AntiqueEnamelBox|GreenAmmo|MP5|SKS|Green9mm|Blue9mm".Split('|');for(int i=0;i<legacy.Length;i++)Check(((ItemKind)i).ToString()==legacy[i],"2749 old persisted item ID retains meaning "+i);
  var unique=new HashSet<string>();var grades=new int[5];var categories=new HashSet<CollectionCategory>();var sizes=new HashSet<int>();var market=MarketRules.Catalog(2);
  const int cell=112,cols=10;var atlas=new Texture2D(cell*cols,cell*10,TextureFormat.RGBA32,false);var pixels=new Color32[atlas.width*atlas.height];for(int i=0;i<pixels.Length;i++)pixels[i]=new Color32(21,29,28,255);
  var data=new List<string>{"Index\tItemId\tAssetId\tName\tGrade\tWidth\tHeight\tSell\tBuy\tCategory\tColoredPixels"};
  Color32[] colors={new Color32(94,173,121,255),new Color32(91,148,209,255),new Color32(170,114,209,255),new Color32(219,179,83,255),new Color32(224,95,84,255)};
  for(int n=0;n<100;n++){
   var k=(ItemKind)(166+n);var d=FormalCollectibles2749.Get(k);int grade=RaidInventory.Grade(k),w=RaidInventory.Width(k),h=RaidInventory.Height(k),cells=w*h;
   Check(unique.Add(d.Id)&&FormalCollectibles2749.Kind(d.Id)==k&&Enum.IsDefined(typeof(ItemKind),k),"2749 formal manifest ID round trips "+d.Id);
   Check(RaidInventory.Name(k)==d.Name&&grade==d.Grade&&w==d.Width&&h==d.Height&&RaidInventory.Group(k)==ItemGroup.Collection&&RaidInventory.Category(k)==FormalCollectibles2749.Category(k),"2749 real inventory name grade footprint category "+d.Name);
   int expected=(int)Math.Round(new[]{100,300,1000,3000,20000}[grade]*(cells==1?1m:cells==2?1.8m:cells==3?2.75m:3.7m));
   Check(RaidInventory.Value(k)==expected&&MarketRules.UnitPrice(k)==(expected*115+99)/100&&market.Contains(k),"2749 actual sell and market price remain grade-size based "+d.Name);
   var inventory=new RaidInventory(5,4);Check(inventory.Add(k,1)==0&&inventory.Slots.Count==1,"2749 new multi-cell item inserts into real grid "+d.Name);var restored=new RaidInventory(5,4);restored.Restore(inventory.Slots);Check(restored.Slots[0].Kind==k&&restored.UsedCells==cells,"2749 saved stack retains appended item and actual occupied cells "+d.Name);
   var texture=ItemIcons.Get(k);Check(texture==FormalCollectibles2749.Texture(k)&&texture.isReadable&&texture.filterMode==FilterMode.Point,"2749 game icon consumer uses original readable colored sprite "+d.Name);
   var source=texture.GetPixels32();int opaque=0,colored=0;var shades=new HashSet<uint>();foreach(var p in source)if(p.a>32){opaque++;if(Mathf.Max(p.r,Mathf.Max(p.g,p.b))-Mathf.Min(p.r,Mathf.Min(p.g,p.b))>10)colored++;shades.Add((uint)(p.r<<16|p.g<<8|p.b));}
   Check(opaque>30&&colored>5&&shades.Count>6,"2749 actual icon contains object color and varied shading "+d.Name);
   grades[grade]++;categories.Add(RaidInventory.Category(k));sizes.Add(cells);int bx=(n%cols)*cell,by=(9-n/cols)*cell;
   for(int yy=0;yy<cell;yy++)for(int xx=0;xx<cell;xx++)if(xx<3||yy<3||xx>=cell-3||yy>=cell-3)pixels[(by+yy)*atlas.width+bx+xx]=colors[grade];
   float scale=Mathf.Min(88f/texture.width,82f/texture.height);int drawW=Mathf.RoundToInt(texture.width*scale),drawH=Mathf.RoundToInt(texture.height*scale);
   for(int yy=0;yy<drawH;yy++)for(int xx=0;xx<drawW;xx++){var p=source[Mathf.Min(texture.height-1,(int)(yy/scale))*texture.width+Mathf.Min(texture.width-1,(int)(xx/scale))];if(p.a>16)pixels[(by+20+yy)*atlas.width+bx+(cell-drawW)/2+xx]=p;}
   for(int mark=0;mark<cells;mark++)for(int yy=8;yy<14;yy++)for(int xx=0;xx<8;xx++)pixels[(by+yy)*atlas.width+bx+10+mark*12+xx]=colors[grade];
   data.Add(n+"\t"+(int)k+"\t"+d.Id+"\t"+d.Name+"\t"+grade+"\t"+w+"\t"+h+"\t"+expected+"\t"+MarketRules.UnitPrice(k)+"\t"+RaidInventory.Category(k)+"\t"+colored);
  }
  atlas.SetPixels32(pixels);atlas.Apply();File.WriteAllBytes(Path.Combine(folder,"2749-actual-100-collectible-icons.png"),atlas.EncodeToPNG());Destroy(atlas);File.WriteAllLines(Path.Combine(folder,"2749-collectible-contact-sheet.tsv"),data);
  Check(grades[0]==24&&grades[1]==39&&grades[2]==24&&grades[3]==7&&grades[4]==6&&categories.Count==8&&sizes.Count==4,"2749 green24 blue39 purple24 gold7 red6 with all eight categories and one-to-four cells");
 }
 void FormalCollectibleLoot2749(){
  var general=ContainerDropRules230.Profile(ContainerLook.CargoCrate,SupplyType.Misc,ContainerTier.Common,DropRegion230.Office);var single=ContainerDropRules230.Profile(ContainerLook.Coat,SupplyType.Misc,ContainerTier.Common,DropRegion230.Office);var safe=ContainerDropRules230.Profile(ContainerLook.Safe,SupplyType.Safe,ContainerTier.Safe,DropRegion230.Office);
  var profiles=new[]{general,single,safe};var rng=new System.Random(2749);int newHits=0;
  foreach(var profile in profiles){
   var quality=ContainerDropRules230.QualityWeights(profile,false);double sum=0;foreach(var q in quality)sum+=q;Check(Math.Abs(sum-1)<.00001&&quality[4]==(profile==safe?.02:0),"2749 established ordinary-safe grade weighting retained "+profile.Name);
   for(int grade=0;grade<4;grade++){
    var pool=new List<ItemKind>();for(int i=0;i<RaidInventory.KindCount;i++)if(RaidInventory.Grade((ItemKind)i)==grade&&ContainerDropRules230.Eligible(profile,(ItemKind)i))pool.Add((ItemKind)i);
    if(pool.Count==0||!FootprintBalance272.Applies(profile))continue;var weights=FootprintBalance272.Weights(pool);var present=new bool[3];foreach(var k in pool)present[FootprintBalance272.Group(k)]=true;double total=(present[0]?80:0)+(present[1]?15:0)+(present[2]?5:0);
    for(int g=0;g<3;g++)Check(Math.Abs(weights[g]-(present[g]?new[]{80d,15d,5d}[g]/total:0))<.00001,"2749 per-grade footprint probabilities 80/15/5 normalize only missing candidates "+profile.Name+" grade="+grade+" size="+g);
   }
   for(int roll=0;roll<180;roll++)foreach(var stack in ContainerDropRules230.Contents(profile,rng,false)){
    if(FormalCollectibles2749.IsNew(stack.Kind))newHits++;int grade=RaidInventory.Grade(stack.Kind);Check(ContainerDropRules230.Eligible(profile,stack.Kind,grade==4&&profile!=safe),"2749 actual container contents obey category size and red-channel eligibility "+profile.Name);
    if(profile==single)Check(RaidInventory.Width(stack.Kind)*RaidInventory.Height(stack.Kind)==1,"2749 coat remains single cell after collectible expansion");
   }
  }
  Check(newHits>20,"2749 actual container consumers draw new formal collectibles");
  for(int i=166;i<266;i++)if(RaidInventory.Grade((ItemKind)i)==4)Check(!ContainerDropRules230.Eligible(general,(ItemKind)i)&&ContainerDropRules230.Eligible(safe,(ItemKind)i)&&!LootBalance.Eligible(SupplyType.Misc,(ItemKind)i),"2749 new red item excluded from ordinary quality roll and allowed safe channel "+(ItemKind)i);
 }
 IEnumerator GroundPhoto2749(Vector3 center,string name,float zoom){
  var go=new GameObject("GroundReviewCamera2749");var camera=go.AddComponent<Camera>();camera.CopyFrom(G.View);camera.enabled=false;camera.allowHDR=false;camera.orthographic=true;camera.orthographicSize=zoom;camera.nearClipPlane=.1f;camera.farClipPlane=300;camera.transform.rotation=Quaternion.Euler(58,35,0);camera.transform.position=center-camera.transform.forward*150;
  float fog=Shader.GetGlobalFloat("_HarborFogEnabled");try{yield return null;Shader.SetGlobalFloat("_HarborFogEnabled",0);var image=HvRender277(camera,name,1600,1100);try{CargoImageCheck2713(image,name);}finally{Destroy(image);}}finally{Shader.SetGlobalFloat("_HarborFogEnabled",fog);Destroy(go);}
 }
 Color GroundPixel2749(RaidMap map,Texture2D texture,Vector2 p)=>texture.GetPixelBilinear((p.x-map.StableGroundBounds137.xMin)/map.StableGroundBounds137.width,(p.y-map.StableGroundBounds137.yMin)/map.StableGroundBounds137.height);
 void GroundStyleChecks2749(RaidMap map){
  Vector2[] road={new Vector2(-28,23),new Vector2(-19,64),new Vector2(49,73)};Vector2[] concrete={new Vector2(-19,23),new Vector2(-54,72),new Vector2(56,73)};float dark=0,light=0;
  for(int i=0;i<3;i++){Check(map.OutdoorRoadWeight2749(road[i])>.95f,"2749 original region connecting lane retains road geometry "+road[i]);var a=GroundPixel2749(map,map.StableGround137,road[i]);var b=GroundPixel2749(map,map.StableGround137,concrete[i]);dark+=(a.r+a.g+a.b)/3;light+=(b.r+b.g+b.b)/3;Check(a.a>.99f&&b.a>.99f,"2749 connected road and neighbouring ground remain opaque "+i);}
  Check(light-dark>.25f,"2749 three-region asphalt remains visibly darker than unified concrete");
  int outside=0;for(int z=0;z<5;z++)for(int x=0;x<5;x++){var p=new Vector3(Mathf.Lerp(map.StableGroundBounds137.xMin+.03f,map.StableGroundBounds137.xMax-.03f,x/4f),0,Mathf.Lerp(map.StableGroundBounds137.yMin+.03f,map.StableGroundBounds137.yMax-.03f,z/4f));if(map.OnHarborSurface(p))continue;var c=GroundPixel2749(map,map.StableGround137,new Vector2(p.x,p.z));if(c.a<.01f)outside++;}Check(outside>0,"2749 outside-port texture still transparent rather than painted as land");
  string oldPath=Path.GetFullPath(Path.Combine(Application.dataPath,"../../../灰港行动_2.7.48/UnityProject/Assets/Environment/HarborGround2721/HarborStableGround2721.png"));var previous=new Texture2D(2,2,TextureFormat.RGBA32,false);try{
   Check(File.Exists(oldPath)&&previous.LoadImage(File.ReadAllBytes(oldPath)),"2749 baseline48 ground texture available for protected natural platforms");Check(previous.width==map.StableGround137.width&&previous.height==map.StableGround137.height,"2749 ground replacement retains exact pixel geometry");
   foreach(var p in new[]{new Vector2(103,62),new Vector2(103,57),new Vector2(101,-23),new Vector2(114,31),new Vector2(100,53)}){var old=GroundPixel2749(map,previous,p);var now=GroundPixel2749(map,map.StableGround137,p);Check(Mathf.Abs(now.r-old.r)+Mathf.Abs(now.g-old.g)+Mathf.Abs(now.b-old.b)+Mathf.Abs(now.a-old.a)<.012f,"2749 authored steel wood deck and gangway retain baseline natural color "+p);}
  }finally{Destroy(previous);}
 }
 void CollectibleActualSale2749(){
  G.Save.Warehouse.Clear();int total=0;for(int i=166;i<266;i++){var kind=(ItemKind)i;CampRules.Deposit(G.Save,kind,1,true);total+=RaidInventory.Value(kind);}var ids=new HashSet<string>();foreach(var item in G.Save.Warehouse)ids.Add(item.Id);G.Panel=CampPanel.Warehouse;int coins=G.Save.Coins;Check(ids.Count==100&&G.SellWarehouse(ids)&&G.Save.Warehouse.Count==0&&G.Save.Coins==coins+total,"2749 actual warehouse sells all hundred collected types at correct prices");
 }
 IEnumerator RunCollectiblesGround2749(){
  var input=G.Controls;bool enabled=G.enabled;string dir=Path.GetDirectoryName(G.SavePath);string[] formal={"stash-v40.json","stash-v41.json","stash-v42.json"};var bytes=Array.ConvertAll(formal,n=>File.Exists(Path.Combine(dir,n))?File.ReadAllBytes(Path.Combine(dir,n)):null);
  try{
   Check(Application.version=="2.7.49"&&G.VerifyMode&&G.SavePath.EndsWith("verification-stash-2.7.49.json"),"2749 changed-scope validation uses isolated save");FormalCollectibleVerification2749.Run(Check);FormalCollectibleConsumers2749();FormalCollectibleLoot2749();FreshCamp();CollectibleActualSale2749();G.Controls=new TouchInput();G.HarborWalkthrough=true;yield return G.BeginTrainingLoad2721();G.Paused=true;G.enabled=false;
   var map=G.Map;var cache=Resources.Load<HarborGroundCache2721>(HarborGroundCache2721.ResourceName);Check(map.HarborWhitebox&&!map.IsInterior&&cache&&cache.Version==2749&&HarborGroundCache2721.AlgorithmVersion==2749&&map.GroundCacheUsed2721,"2749 outdoor uses newly baked unified ground cache");
   Check(map.StableGround137==cache.Texture&&map.StableGroundBounds137.Equals(cache.Bounds)&&map.GroundPaintFingerprint2721==cache.PaintFingerprint,"2749 cache texture bounds and fingerprint match actual outdoor renderer");
   Check(map.Root.Find("Harbor_StableGround137").GetComponentsInChildren<Collider>().Length==0,"2749 unified cosmetic floor adds no collision blockers");GroundStyleChecks2749(map);File.WriteAllBytes(Path.Combine(folder,"2749-unified-entire-port-ground-texture.png"),map.StableGround137.EncodeToPNG());
   yield return GroundPhoto2749(RaidMap.PortPoint(32,150),"2749-live-freight-ground.png",19);yield return GroundPhoto2749(RaidMap.PortPoint(77,90),"2749-live-central-ground.png",20);yield return GroundPhoto2749(RaidMap.PortPoint(118,151),"2749-live-office-unloading-ground.png",19);
   for(int i=0;i<formal.Length;i++)Check(MainBytes273(bytes[i],File.Exists(Path.Combine(dir,formal[i]))?File.ReadAllBytes(Path.Combine(dir,formal[i])):null),"2749 formal save unchanged "+formal[i]);
  }finally{G.Controls=input;G.ClearShots();G.Paused=true;G.enabled=enabled;}
 }
}}
