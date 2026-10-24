using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace GreyHarbor {
 public sealed partial class RaidVerification {
  IEnumerator VerifyContainerDrop230(){
   FreshCamp();string inventory=JsonUtility.ToJson(G.Save);string formal=Path.Combine(Path.GetDirectoryName(G.SavePath),"stash-v40.json");string before=File.Exists(formal)?File.ReadAllText(formal):null;
   Check(G.SavePath.Contains("2.7.2")&&!G.SavePath.EndsWith("stash-v41.json"),"230 Isolated verification save");
   var random=new System.Random(23009);
   foreach(DropRegion230 region in Enum.GetValues(typeof(DropRegion230)))foreach(ContainerLook look in Enum.GetValues(typeof(ContainerLook))){
    SupplyType type=look==ContainerLook.AmmoCase?SupplyType.Ammo:look==ContainerLook.MilitaryCase?SupplyType.Military:look==ContainerLook.Safe?SupplyType.Safe:look==ContainerLook.MedicalCabinet||look==ContainerLook.RescueCabinet?SupplyType.Medical:SupplyType.Misc;
    var profile=ContainerDropRules230.Profile(look,type,type==SupplyType.Safe?ContainerTier.Safe:ContainerTier.Common,region);
    var quality=ContainerDropRules230.QualityWeights(profile,false);double sum=0;foreach(double w in quality)sum+=w;
    Check(Math.Abs(sum-1)<.000001,"230 Profile has normalized actual quality "+region+"/"+look);
    bool legal=true,quantity=true;for(int n=0;n<25;n++){
     var items=ContainerDropRules230.Contents(profile,random,false);quantity&=items.Count>=1&&items.Count<=7;
     foreach(var item in items)legal&=ContainerDropRules230.Eligible(profile,item.Kind,RaidInventory.Grade(item.Kind)==4&&profile.Usage!=ContainerUsage230.Safe);
    }
    Check(legal&&quantity,"230 Generated contents respect legal pool size and quality "+region+"/"+look);
   }
   var general=ContainerDropRules230.Profile(ContainerLook.StorageBin,SupplyType.Misc,ContainerTier.Common,DropRegion230.Entrance);
   var groups=ContainerDropRules230.CategoryWeights(general,1);Check(Math.Abs(groups[(int)CollectionCategory.Living]-.45)<.000001&&Math.Abs(groups[(int)CollectionCategory.MedicalMaterials]-.30)<.000001,"230 Generic entrance box has actual 45 percent living and 30 percent medical");
   Check(groups[(int)CollectionCategory.Biological]==0,"230 Generic box excludes alien materials");
   var file=ContainerDropRules230.Profile(ContainerLook.FileCabinet,SupplyType.Misc,ContainerTier.Common,DropRegion230.Repair);var fileWeights=ContainerDropRules230.CategoryWeights(file,2);
   Check(Math.Abs(fileWeights[(int)CollectionCategory.Information]-1)<.000001,"230 Repair file cabinet stays 100 percent information");
   var tools=ContainerDropRules230.Profile(ContainerLook.Toolbox,SupplyType.Tools,ContainerTier.Common,DropRegion230.Entrance);var toolWeights=ContainerDropRules230.CategoryWeights(tools,1);
   Check(Math.Abs(toolWeights[(int)CollectionCategory.ToolsParts]-.85)<.000001,"230 Mixed professional toolbox keeps 85 percent tools");
   var luggage=ContainerDropRules230.Profile(ContainerLook.Suitcase,SupplyType.Misc,ContainerTier.Common,DropRegion230.Entrance);var luggageWeights=ContainerDropRules230.CategoryWeights(luggage,1);
   Check(luggageWeights[(int)CollectionCategory.MedicalMaterials]==0&&luggageWeights[(int)CollectionCategory.Living]>.45,"230 Missing allowed secondary group renormalizes luggage without changing quality");
   var cooler=ContainerDropRules230.Profile(ContainerLook.MarineCooler,SupplyType.Misc,ContainerTier.Common,DropRegion230.FishingDock);
   bool noAlien=true;for(int k=0;k<RaidInventory.KindCount;k++)if(RaidInventory.Group((ItemKind)k)==ItemGroup.Collection&&RaidInventory.Category((ItemKind)k)==CollectionCategory.Biological)noAlien&=!ContainerDropRules230.Eligible(cooler,(ItemKind)k,true)&&!ContainerDropRules230.Eligible(cooler,(ItemKind)k);
   Check(noAlien,"230 Marine cooler rejects biological items including red alien material");
   var advanced=ContainerDropRules230.Profile(ContainerLook.StorageBin,SupplyType.Misc,ContainerTier.Advanced,DropRegion230.Yard);var q=ContainerDropRules230.QualityWeights(advanced,false);var hard=ContainerDropRules230.QualityWeights(advanced,true);
   Check(Math.Abs(q[0]-.6)<.000001&&Math.Abs(q[1]-.32)<.000001&&Math.Abs(q[2]-.07)<.000001&&Math.Abs(q[3]-.01)<.000001&&q[4]==0,"230 Legacy advanced box uses common 60 32 7 1 without basic red");
   Check(Math.Abs(hard[2]-.105)<.000001&&Math.Abs(hard[3]-.015)<.000001,"230 Hard multiplier applies only to quality once");
   var sampleGrades=new int[5];var sampleCategories=new int[9];var counts=new int[8];int reds=0,total=0;const int boxes=30000;
   for(int n=0;n<boxes;n++){
    var items=ContainerDropRules230.Contents(general,random,false);int basics=0;foreach(var item in items){int grade=RaidInventory.Grade(item.Kind);if(grade==4){reds++;continue;}basics++;total++;sampleGrades[grade]++;sampleCategories[(int)RaidInventory.Category(item.Kind)]++;}counts[basics]++;
   }
   Check(reds>=8&&reds<=60,"230 Independent red bonus statistically matches 0.1 percent per box, actual "+reds+"/"+boxes);
   Check(Math.Abs(total/(double)boxes-2.21)<.04,"230 Base count average remains 2.21, actual "+total/(double)boxes);
   var expected=ContainerDropRules230.QualityWeights(general,false);for(int i=0;i<4;i++)Check(Math.Abs(sampleGrades[i]/(double)total-expected[i])<.012,"230 Sampled normal quality agrees with declared grade "+i);
   Check(Math.Abs(sampleCategories[(int)CollectionCategory.Living]/(double)total-.45)<.015&&Math.Abs(sampleCategories[(int)CollectionCategory.MedicalMaterials]/(double)total-.30)<.015,"230 Sampled category probabilities match target despite unequal item counts");
   for(int i=1;i<=6;i++)Check(Math.Abs(counts[i]/(double)boxes-LootBalance.CountWeights[i-1]/1000.0)<.015,"230 Quantity probability retained count "+i);
   G.HarborWalkthrough=true;G.BeginTraining();G.Paused=true;yield return null;Check(G.Map.Loot.Count==30&&G.Enemies.Count==16,"230 Exterior density and population unchanged");
   var outside=G.Map;bool linked=true;foreach(var box in outside.Loot)linked&=box.DropProfile230!=null;Check(linked,"230 Every exterior container uses new declared profile");
   var categories=new List<string>{"region,look,grade,living,tools,electronics,medical,energy,biological,information,collectibles"};
   foreach(DropRegion230 region in Enum.GetValues(typeof(DropRegion230)))foreach(ContainerLook look in Enum.GetValues(typeof(ContainerLook))){var p=ContainerDropRules230.Profile(look,look==ContainerLook.AmmoCase?SupplyType.Ammo:look==ContainerLook.MilitaryCase?SupplyType.Military:look==ContainerLook.Safe?SupplyType.Safe:look==ContainerLook.MedicalCabinet||look==ContainerLook.RescueCabinet?SupplyType.Medical:SupplyType.Misc,ContainerTier.Auto,region);var qw=ContainerDropRules230.QualityWeights(p,false);for(int g=0;g<5;g++)if(qw[g]>0&&p.Usage!=ContainerUsage230.Combat){var cw=ContainerDropRules230.CategoryWeights(p,g);var fields=new List<string>{region.ToString(),look.ToString(),g.ToString()};for(int c=1;c<9;c++)fields.Add(cw[c].ToString(System.Globalization.CultureInfo.InvariantCulture));categories.Add(string.Join(",",fields));}}
   File.WriteAllLines(Path.Combine(folder,"container-category-weights-230.csv"),categories);
   var itemRows=new List<string>{"item,name,category,grade,sellPrice,width,height,regionWeights"};for(int i=0;i<RaidInventory.KindCount;i++){var k=(ItemKind)i;if(RaidInventory.Group(k)!=ItemGroup.Collection)continue;var weights=new List<string>();foreach(DropRegion230 r in Enum.GetValues(typeof(DropRegion230))){var p=ContainerDropRules230.Profile(ContainerLook.StorageBin,SupplyType.Misc,ContainerTier.Common,r);weights.Add(r+":"+ContainerDropRules230.ItemRegionWeight(p,k).ToString(System.Globalization.CultureInfo.InvariantCulture));}itemRows.Add(k+","+Csv230(RaidInventory.Name(k))+","+RaidInventory.Category(k)+","+RaidInventory.Grade(k)+","+RaidInventory.Value(k)+","+RaidInventory.Width(k)+","+RaidInventory.Height(k)+","+Csv230(string.Join("/",weights)));}File.WriteAllLines(Path.Combine(folder,"collection-item-config-230.csv"),itemRows);
   var catalog=new List<string>{"map,name,region,usage,maxWidth,maxHeight,tier,actualQualityNormal,actualQualityHard,allowRed,redDisabledReason"};ExportDropMap230(catalog,outside);
   foreach(string destination in new[]{"portbooth","portservice","yardhome","yardshed","ferry1"}){
    G.SwitchInterior(destination);G.Paused=true;yield return null;var map=G.Map;bool legal=true;var snapshots=new Dictionary<LootContainer,string>();foreach(var box in map.Loot){legal&=box.DropProfile230!=null;foreach(var item in box.Items)legal&=ContainerDropRules230.Eligible(box.DropProfile230,item.Kind,RaidInventory.Grade(item.Kind)==4&&box.DropProfile230.Usage!=ContainerUsage230.Safe);snapshots[box]=JsonUtility.ToJson(new ItemList230{Items=box.Items});}
    Check(legal&&map.Loot.Count>0,"230 Interior has legal profiled contents "+destination);ExportDropMap230(catalog,map);
    if(destination=="ferry1"){G.SwitchInterior("ferry-floor:2:front");G.Paused=true;yield return null;ExportDropMap230(catalog,G.Map);Check(G.Map.InteriorId=="ferry2"&&G.Map.Loot.Count==8&&G.Map.Loot.TrueForAll(b=>b.DropProfile230!=null),"230 Ferry second floor profiles present");G.SwitchInterior("ferry-floor:1:front");G.Paused=true;yield return null;}
    G.SwitchInterior(destination);G.Paused=true;yield return null;G.SwitchInterior(destination);G.Paused=true;yield return null;
    bool stable=G.Map==map;foreach(var box in map.Loot)stable&=snapshots[box]==JsonUtility.ToJson(new ItemList230{Items=box.Items});Check(stable,"230 Leaving and revisiting preserves exact items identities and grid "+destination);
    G.SwitchInterior(destination);G.Paused=true;yield return null;
   }
   File.WriteAllLines(Path.Combine(folder,"container-config-230.csv"),catalog);
   var large=outside.Loot[0];var savedItems=new List<ItemStack>(large.Items);large.Items.Clear();for(int n=0;n<7;n++)large.Items.Add(new ItemStack(ItemKind.BlueArmor,1));var grid=G.EnsureLootGrid(large);Check(grid.Slots.Count==7&&grid.UsedCells==63,"230 Search grid expands without deleting seven large items");large.Items.Clear();large.Items.AddRange(savedItems);
   G.Controls=new DesktopInput();G.Finish(false);yield return null;Check(JsonUtility.ToJson(G.Save)==inventory,"230 Preview restores inventory exactly");Check(before==null?!File.Exists(formal):File.ReadAllText(formal)==before,"230 Formal save unchanged");
  }
  [Serializable] sealed class ItemList230 {public List<ItemStack> Items;}
  string Csv230(string s)=>"\""+(s??"").Replace("\"","\"\"")+"\"";
  void ExportDropMap230(List<string> rows,RaidMap map){foreach(var box in map.Loot){var p=box.DropProfile230;rows.Add(Csv230(map.IsInterior?map.InteriorId:"harbor")+","+Csv230(box.Name)+","+Csv230(p.Region.ToString())+","+Csv230(p.Usage.ToString())+","+p.MaxWidth+","+p.MaxHeight+","+box.Tier+","+Csv230(string.Join("/",ContainerDropRules230.QualityWeights(p,false)))+","+Csv230(string.Join("/",ContainerDropRules230.QualityWeights(p,true)))+","+p.AllowRed+","+Csv230(p.RedDisabledReason));}}
 }
}
