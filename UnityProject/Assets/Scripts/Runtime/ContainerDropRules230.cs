using System;
using System.Collections.Generic;
namespace GreyHarbor {
 public enum DropRegion230 { Entrance,Yard,Repair,Freight,Loading,Office,Containers,PassengerDock,FreightDock,FishingDock,Training }
 public enum ContainerUsage230 { General,Luggage,Tools,Energy,Electronics,MedicalMaterials,Information,Collectibles,Biological,Safe,Combat }
 public sealed class ContainerDropProfile230 {
  public string Name;public DropRegion230 Region;public ContainerUsage230 Usage;public ContainerLook Look;public SupplyType Type;public ContainerTier Tier;
  public int FixedCount;public bool EnableBonusRed=true;public SafeSize240 SafeSize;public int MaxWidth=2,MaxHeight=2;public bool AllowRed;public string RedDisabledReason="";
  public bool[] AllowedCategories=new bool[9];public CollectionCategory PrimaryCategory;
  public bool UniqueValuables=true;public readonly HashSet<ItemKind> Whitelist=new HashSet<ItemKind>(),Excluded=new HashSet<ItemKind>();
  public double[] ActualQualityNormal,ActualQualityHard;public readonly List<string> Diagnostics=new List<string>();
 }
 public static class ContainerDropRules230 {
  public const double BonusRedChance=.001;
  public static ContainerDropProfile230 Profile(ContainerLook look,SupplyType type,ContainerTier tier,DropRegion230 region,SafeSize240 safeSize=SafeSize240.Legacy){
   var p=new ContainerDropProfile230{Name=look.ToString(),Look=look,Type=type,Tier=LootBalance.TierFor(type,tier),Region=region,SafeSize=safeSize};
   if(type==SupplyType.Safe){p.Usage=ContainerUsage230.Safe;p.MaxWidth=3;p.MaxHeight=3;for(int c=1;c<9;c++)p.AllowedCategories[c]=true;for(int i=0;i<RaidInventory.KindCount;i++){var k=(ItemKind)i;if(RaidInventory.Group(k)==ItemGroup.Collection&&LootBalance.Eligible(SupplyType.Safe,k))p.Whitelist.Add(k);}}
   else if(type==SupplyType.Medical||look==ContainerLook.MedicalCabinet||look==ContainerLook.RescueCabinet){Set(p,ContainerUsage230.MedicalMaterials,CollectionCategory.MedicalMaterials,CollectionCategory.MedicalMaterials);}
   else if(type==SupplyType.Ammo||type==SupplyType.Military){p.Usage=ContainerUsage230.Combat;p.MaxWidth=3;p.MaxHeight=3;for(int i=0;i<RaidInventory.KindCount;i++){var k=(ItemKind)i;if(RaidInventory.Group(k)==ItemGroup.Combat&&LootBalance.Eligible(type,k))p.Whitelist.Add(k);}for(int c=1;c<9;c++)p.AllowedCategories[c]=c!=(int)CollectionCategory.Biological;}
   else switch(look){
    case ContainerLook.Coat:Set(p,ContainerUsage230.Luggage,CollectionCategory.None,CollectionCategory.Living,CollectionCategory.Collectibles,CollectionCategory.Information);p.MaxWidth=1;p.MaxHeight=1;break;
    case ContainerLook.Suitcase:case ContainerLook.RollingSuitcase:case ContainerLook.Wardrobe:case ContainerLook.BedsideDrawer:Set(p,ContainerUsage230.Luggage,CollectionCategory.None,CollectionCategory.Living,CollectionCategory.Collectibles,CollectionCategory.Electronics,CollectionCategory.Information);break;
    case ContainerLook.OutdoorTote:case ContainerLook.KitchenCupboard:case ContainerLook.MarineCooler:Set(p,ContainerUsage230.Luggage,CollectionCategory.None,CollectionCategory.Living);break;
    case ContainerLook.JewelryBox:Set(p,ContainerUsage230.Collectibles,CollectionCategory.Collectibles,CollectionCategory.Collectibles);p.MaxWidth=1;p.MaxHeight=1;break;
    case ContainerLook.FileCabinet:case ContainerLook.ArchiveCase:case ContainerLook.DocumentTube:Set(p,ContainerUsage230.Information,CollectionCategory.Information,CollectionCategory.Information);p.MaxHeight=3;break;
    case ContainerLook.ElectricalCabinet:case ContainerLook.MarineControlBox:case ContainerLook.PrecisionCase:Set(p,ContainerUsage230.Electronics,CollectionCategory.Electronics,CollectionCategory.Electronics,CollectionCategory.Information);break;
    case ContainerLook.FuelDrum:Set(p,ContainerUsage230.Energy,CollectionCategory.EnergyChemicals,CollectionCategory.EnergyChemicals);break;
    case ContainerLook.MedicalCabinet:case ContainerLook.RescueCabinet:Set(p,ContainerUsage230.MedicalMaterials,CollectionCategory.MedicalMaterials,CollectionCategory.MedicalMaterials);break;
    case ContainerLook.Toolbox:case ContainerLook.GardenChest:case ContainerLook.WeldingToolbox:case ContainerLook.FishingChest:Set(p,ContainerUsage230.Tools,CollectionCategory.ToolsParts,CollectionCategory.ToolsParts,CollectionCategory.EnergyChemicals,CollectionCategory.Electronics);break;
    default:if(type==SupplyType.Tools)Set(p,ContainerUsage230.Tools,CollectionCategory.ToolsParts,CollectionCategory.ToolsParts,CollectionCategory.EnergyChemicals,CollectionCategory.Electronics);else{p.Usage=ContainerUsage230.General;for(int c=1;c<9;c++)p.AllowedCategories[c]=c!=(int)CollectionCategory.Biological;}break;
   }
   if(look==ContainerLook.CargoCrate||look==ContainerLook.StorageBin||look==ContainerLook.SupplyLocker||look==ContainerLook.OutdoorTote){p.MaxWidth=3;p.MaxHeight=3;}
   if(look==ContainerLook.MarineCooler){p.Excluded.Add(ItemKind.SoapBox);p.Excluded.Add(ItemKind.Cloth);p.Excluded.Add(ItemKind.VacuumFlask);p.Excluded.Add(ItemKind.PocketRadio);p.Excluded.Add(ItemKind.VintageCamera);for(int i=CollectibleData260.First;i<RaidInventory.KindCount;i++){var k=(ItemKind)i;if(RaidInventory.Category(k)==CollectionCategory.Living&&k!=ItemKind.CompressedBiscuits)p.Excluded.Add(k);}}
   if(safeSize!=SafeSize240.Legacy){if(type!=SupplyType.Safe)throw new ArgumentException("大小保险配置只能用于保险箱");for(int i=0;i<RaidInventory.KindCount;i++)if(RaidInventory.Category((ItemKind)i)==CollectionCategory.Biological)p.Excluded.Add((ItemKind)i);}
   Validate(p);return p;
  }
  static void Set(ContainerDropProfile230 p,ContainerUsage230 u,CollectionCategory primary,params CollectionCategory[] allowed){p.Usage=u;p.PrimaryCategory=primary;foreach(var c in allowed)p.AllowedCategories[(int)c]=true;}
  public static bool Eligible(ContainerDropProfile230 p,ItemKind k,bool red=false){
   if(RetiredMedicine321.IsRetired(k)||k==ItemKind.GreenOptic30||KeyRules2760.IsKey(k)||(int)k<0||(int)k>=RaidInventory.KindCount||p.Excluded.Contains(k)||RaidInventory.Width(k)>p.MaxWidth||RaidInventory.Height(k)>p.MaxHeight)return false;
   bool collection=RaidInventory.Group(k)==ItemGroup.Collection;int grade=RaidInventory.Grade(k);
   if(red)return collection&&grade==4&&(p.Usage!=ContainerUsage230.Safe||p.SafeSize!=SafeSize240.Legacy)&&p.AllowedCategories[(int)RaidInventory.Category(k)]&&(p.Usage==ContainerUsage230.Combat||p.Whitelist.Count==0||p.Whitelist.Contains(k));
   if(p.Usage==ContainerUsage230.Combat)return !collection&&p.Whitelist.Contains(k);
   if(!collection||(grade==4&&p.Usage!=ContainerUsage230.Safe))return false;
   return p.AllowedCategories[(int)RaidInventory.Category(k)]&&(p.Whitelist.Count==0||p.Whitelist.Contains(k));
  }
  public static void Validate(ContainerDropProfile230 p){
   if(p.FixedCount<0||p.FixedCount>6)throw new ArgumentOutOfRangeException("FixedCount");
   p.Diagnostics.Clear();p.ActualQualityNormal=QualityWeights(p,false);p.ActualQualityHard=QualityWeights(p,true);
   bool red=false;for(int i=0;i<RaidInventory.KindCount;i++)if(Eligible(p,(ItemKind)i,true)){red=true;break;}
   p.AllowRed=p.EnableBonusRed&&(p.Tier!=ContainerTier.Safe||p.SafeSize!=SafeSize240.Legacy)&&red;p.RedDisabledReason=!p.EnableBonusRed?"此容器禁用额外红品":p.Tier==ContainerTier.Safe&&p.SafeSize==SafeSize240.Legacy?"保险箱沿用独立红品品质表，不叠加额外红":red?"":"用途、尺寸与白名单内无合法红品";
   if(!p.AllowRed)p.Diagnostics.Add(p.RedDisabledReason);
   var basis=p.SafeSize==SafeSize240.Legacy?LootBalance.QualityWeights(p.Tier):MainSafeRules240.Quality(p.SafeSize);for(int g=0;g<5;g++)if(basis[g]>0&&p.ActualQualityNormal[g]==0)p.Diagnostics.Add("品质"+g+"无合法候选，已明确归一化");
  }
  public static double[] QualityWeights(ContainerDropProfile230 p,bool hard)=>Quality(p,hard,null);
  static double[] Quality(ContainerDropProfile230 p,bool hard,HashSet<ItemKind> used){
   var w=p.SafeSize==SafeSize240.Legacy?LootBalance.QualityWeights(p.Tier,hard?1.5:1):MainSafeRules240.Quality(p.SafeSize);bool[] present=new bool[5];
   for(int i=0;i<RaidInventory.KindCount;i++){var k=(ItemKind)i;if(Eligible(p,k)&&Available(p,k,used))present[RaidInventory.Grade(k)]=true;}
   for(int g=0;g<5;g++)if(!present[g])w[g]=0;Normalize(w,"容器所有品质均无合法候选："+p.Name);return w;
  }
  public static double[] CategoryWeights(ContainerDropProfile230 p,int grade)=>Categories(p,grade,null,false);
  static double[] Categories(ContainerDropProfile230 p,int grade,HashSet<ItemKind> used,bool red){
   var w=new double[9];if(p.Usage==ContainerUsage230.Combat&&!red)return w;bool[] present=new bool[9];
   for(int i=0;i<RaidInventory.KindCount;i++){var k=(ItemKind)i;if(RaidInventory.Grade(k)==grade&&Eligible(p,k,red)&&Available(p,k,used))present[(int)RaidInventory.Category(k)]=true;}
   if(p.Usage==ContainerUsage230.Safe){for(int i=0;i<RaidInventory.KindCount;i++){var k=(ItemKind)i;if(RaidInventory.Grade(k)==grade&&Eligible(p,k)&&Available(p,k,used))w[(int)RaidInventory.Category(k)]+=ContainerLootRules.Weights[(int)SupplyType.Safe,(int)RaidInventory.Category(k)];}}
   else if(p.PrimaryCategory!=CollectionCategory.None){int secondary=0;for(int c=1;c<9;c++)if(present[c]&&c!=(int)p.PrimaryCategory)secondary++;for(int c=1;c<9;c++)if(present[c])w[c]=c==(int)p.PrimaryCategory?.85:secondary>0?.15/secondary:0;}
   else {
    int[] main,second;RegionGroups(p.Region,out main,out second);
    if(main.Length==0&&second.Length==0){for(int c=1;c<9;c++)if(present[c])w[c]=1;}
    else {int[] count=new int[3];for(int c=1;c<9;c++)if(present[c])count[Group(c,main,second)]++;double[] target={.45,.30,.25};for(int c=1;c<9;c++)if(present[c]){int group=Group(c,main,second);w[c]=target[group]/count[group];}}
   }
   Normalize(w,"当前品质无合法类别："+p.Name+" / "+grade);return w;
  }
  static int Group(int c,int[] main,int[] second)=>Array.IndexOf(main,c)>=0?0:Array.IndexOf(second,c)>=0?1:2;
  public static void RegionGroups(DropRegion230 region,out int[] main,out int[] secondary){
   switch(region){
    case DropRegion230.Entrance:main=new[]{1};secondary=new[]{4};break;
    case DropRegion230.Yard:main=new[]{8};secondary=new[]{1,7};break;
    case DropRegion230.Repair:main=new[]{2};secondary=new[]{5};break;
    case DropRegion230.Freight:main=new[]{5};secondary=new[]{2};break;
    case DropRegion230.Loading:main=new[]{2};secondary=new[]{3,5};break;
    case DropRegion230.Office:main=new[]{7};secondary=new[]{3};break;
    case DropRegion230.PassengerDock:case DropRegion230.FreightDock:case DropRegion230.FishingDock:main=new[]{1,8};secondary=new[]{2,5};break;
    default:main=new int[0];secondary=new int[0];break;
   }
  }
  static bool Unique(ContainerDropProfile230 p,ItemKind k)=>p.UniqueValuables&&RaidInventory.Group(k)==ItemGroup.Collection&&(RaidInventory.Grade(k)==4||(RaidInventory.Grade(k)==3&&RaidInventory.Category(k)==CollectionCategory.Collectibles));
  static bool Available(ContainerDropProfile230 p,ItemKind k,HashSet<ItemKind> used)=>used==null||!Unique(p,k)||!used.Contains(k);
  static void Normalize(double[] w,string error){double total=0;foreach(double v in w)total+=v;if(total<=0)throw new InvalidOperationException(error);for(int i=0;i<w.Length;i++)w[i]/=total;}
  static int Roll(double[] w,Random rng){double roll=rng.NextDouble();int last=-1;for(int i=0;i<w.Length;i++)if(w[i]>0){last=i;roll-=w[i];if(roll<0)return i;}if(last<0)throw new InvalidOperationException("空概率表");return last;}
  public static double ItemRegionWeight(ContainerDropProfile230 p,ItemKind k){
   // Soft regional preferences only: no map-specific item exclusion.
   DropRegion230 natural=Natural(k);if(p.Region==natural)return 2.5;
   var c=RaidInventory.Category(k);int[] main,second;RegionGroups(p.Region,out main,out second);
   if(Array.IndexOf(main,(int)c)>=0||Array.IndexOf(second,(int)c)>=0)return 1.5;return 1;
  }
  static DropRegion230 Natural(ItemKind k){
   switch(k){
    case ItemKind.RouteMap:case ItemKind.ShippingReceipt:case ItemKind.BatteryCell:case ItemKind.Charcoal:case ItemKind.FuelCell:return DropRegion230.FreightDock;
    case ItemKind.ShellPendant:case ItemKind.HarborCoin:case ItemKind.BronzeStatuette:return DropRegion230.FishingDock;
    case ItemKind.VintageCamera:case ItemKind.CellarWine:case ItemKind.GoldWatch:case ItemKind.SilverRing:return DropRegion230.PassengerDock;
    case ItemKind.CampNotes:case ItemKind.OldBadge:case ItemKind.MusicBox:case ItemKind.JadeSeal:return DropRegion230.Yard;
    case ItemKind.Fuse:case ItemKind.Relay:case ItemKind.ControlBoard:case ItemKind.Circuit:return DropRegion230.Loading;
    case ItemKind.Resin:case ItemKind.Solvent:case ItemKind.Powder:case ItemKind.Catalyst:return DropRegion230.Freight;
   }
   switch(RaidInventory.Category(k)){
    case CollectionCategory.Living:case CollectionCategory.MedicalMaterials:return DropRegion230.Entrance;
    case CollectionCategory.ToolsParts:case CollectionCategory.EnergyChemicals:return DropRegion230.Repair;
    case CollectionCategory.Electronics:case CollectionCategory.Information:return DropRegion230.Office;
    case CollectionCategory.Collectibles:return DropRegion230.Yard;
    default:return DropRegion230.Training;
   }
  }
  static ItemKind Pick(ContainerDropProfile230 p,int grade,Random rng,HashSet<ItemKind> used,bool red=false){
   int category=p.Usage==ContainerUsage230.Combat&&!red?0:Roll(Categories(p,grade,used,red),rng);
   var pool=new List<ItemKind>();var weights=new List<double>();double total=0;
   for(int i=0;i<RaidInventory.KindCount;i++){var k=(ItemKind)i;if(RaidInventory.Grade(k)!=grade||!Eligible(p,k,red)||!Available(p,k,used))continue;if(category>0&&(int)RaidInventory.Category(k)!=category)continue;
    double weight=category>0?ItemRegionWeight(p,k):p.Type==SupplyType.Medical?(k==ItemKind.GreenArmorKit?80:20):1;
    // The safe retains its established white-list/category distribution and has no regional uplift.
    if(p.Usage==ContainerUsage230.Safe)weight=1;if(p.Usage==ContainerUsage230.Combat&&!red)weight*=WeaponRules279.DropWeight(k);pool.Add(k);weights.Add(weight);total+=weight;
   }
   if(pool.Count==0)throw new InvalidOperationException("当前容器候选为空："+p.Name);
   int sizeGroup=FootprintBalance272.Applies(p,red)?FootprintBalance272.PickGroup(pool,rng):-1;
   if(sizeGroup>=0){total=0;for(int i=0;i<pool.Count;i++)if(FootprintBalance272.Group(pool[i])==sizeGroup)total+=weights[i];}
   double roll=rng.NextDouble()*total;ItemKind last=pool[0];for(int i=0;i<pool.Count;i++){if(sizeGroup>=0&&FootprintBalance272.Group(pool[i])!=sizeGroup)continue;last=pool[i];roll-=weights[i];if(roll<0)return last;}return last;
  }
  public static List<ItemStack> Contents(ContainerDropProfile230 p,Random rng,bool hard,bool includeCoatKey=true){
   var result=new List<ItemStack>();var used=new HashSet<ItemKind>();int count=p.FixedCount>0?p.FixedCount:LootBalance.Count(rng);
   for(int i=0;i<count;i++){int grade=Roll(Quality(p,hard,used),rng);var k=Pick(p,grade,rng,used);used.Add(k);result.Add(Stack(k,rng));}
   // A medical cabinet offers saleable medical collectibles; at most one slot can be a repair pack.
   if((p.Type==SupplyType.Medical||p.Look==ContainerLook.MedicalCabinet||p.Look==ContainerLook.RescueCabinet)&&result.Count>0&&rng.NextDouble()<.20)
    result[rng.Next(result.Count)]=Stack(rng.NextDouble()<.80?ItemKind.GreenArmorKit:ItemKind.ArmorKit,rng);
   if(p.AllowRed&&rng.NextDouble()<(p.SafeSize==SafeSize240.Legacy?BonusRedChance:MainSafeRules240.RedChance(p.SafeSize))){var k=Pick(p,4,rng,used,true);result.Add(Stack(k,rng));}
   if(includeCoatKey)KeyRules2760.RollCoat(p.Look,rng,result);
   return result;
  }
  static ItemStack Stack(ItemKind k,Random rng)=>new ItemStack(k,AmmoRules270.IsAmmo(k)?AmmoRules270.LootCount30(k,rng):1,ItemOrigin.Pending){Identified=false,RaidFound=true};
 }
}
