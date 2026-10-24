using System;
using System.Collections.Generic;
using UnityEngine;
namespace GreyHarbor {
 public static class FormalCollectibleVerification2749 {
  [Serializable] sealed class Snapshot {public List<ItemStack> Items=new List<ItemStack>();}
  public static void Run(Action<bool,string> check){
   check(RaidInventory.KindCount==266&&FormalCollectibles2749.Count==100,"KindCount expanded and 100 formal definitions");
   check((int)ItemKind.Ammo==0&&(int)ItemKind.AK==11&&(int)ItemKind.AntiqueEnamelBox==160&&(int)ItemKind.GreenAmmo==161&&(int)ItemKind.Blue9mm==165,"Legacy item IDs remain stable");
   var ids=new HashSet<string>();var names=new HashSet<string>();var snapshot=new Snapshot();int collections=0;
   for(int i=0;i<RaidInventory.KindCount;i++)if(RaidInventory.Group((ItemKind)i)==ItemGroup.Collection)collections++;
   check(collections==244,"144 old + 100 new collection items");
   for(int n=0;n<100;n++){
    var k=(ItemKind)(166+n);var d=FormalCollectibles2749.Get(k);string label="Formal collectible "+k;
    check(ids.Add(d.Id)&&names.Add(d.Name)&&FormalCollectibles2749.Kind(d.Id)==k,label+" unique stable identity");
    check(RaidInventory.Name(k)==d.Name&&RaidInventory.Width(k)==d.Width&&RaidInventory.Height(k)==d.Height&&RaidInventory.Grade(k)==d.Grade,label+" actual inventory metadata");
    check(RaidInventory.Category(k)!=CollectionCategory.None&&RaidInventory.Use(k).Contains("收藏品")&&RaidInventory.Limit(k)==1,label+" category and description");
    int value=FormalCollectibles2749.Value(k);check(RaidInventory.Value(k)==value&&MarketRules.UnitPrice(k)==(value*115+99)/100&&MarketRules.Listed(k)&&MarketCategories.Main(k)==6,label+" real trade consumers");
    var texture=ItemIcons.Get(k);var uv=ItemIcons.Bounds(k);check(texture==d.Texture&&uv.width>0&&uv.height>0,label+" actual icon and alpha bounds");
    var pixels=texture.GetPixels32();int solid=0,colored=0;foreach(var p in pixels)if(p.a>16){solid++;if(Mathf.Max(p.r,Mathf.Max(p.g,p.b))-Mathf.Min(p.r,Mathf.Min(p.g,p.b))>15)colored++;}
    check(solid>0&&colored>solid*.03f,label+" colored visible pixels");
    var grid=new RaidInventory(5,4);check(grid.Add(k,1,ItemOrigin.Certified,true)==0&&grid.TotalValue==value&&grid.UsedCells==d.Cells,label+" add dimensions and sell quote");
    var clone=grid.Clone();check(clone.Count(k)==1&&clone.TotalValue==value&&clone.At(0)?.Kind==k,label+" retrieval snapshot restore");
    snapshot.Items.Add(RaidInventory.Copy(grid.Slots[0]));
    if(d.Grade==4)check(!LootBalance.Eligible(SupplyType.Misc,k)&&LootBalance.Eligible(SupplyType.Safe,k),label+" red remains special");
   }
   var recovered=JsonUtility.FromJson<Snapshot>(JsonUtility.ToJson(snapshot));bool exact=recovered.Items.Count==100;for(int i=0;i<100&&exact;i++)exact=recovered.Items[i].Kind==snapshot.Items[i].Kind&&recovered.Items[i].Id==snapshot.Items[i].Id&&recovered.Items[i].Origin==ItemOrigin.Certified;check(exact,"All 100 new IDs serialize and restore without remapping");
   var samples=new[]{ItemKind.ShowcaseOfficeStamp,ItemKind.ShowcaseBinderTrio};
   check(FootprintBalance272.Group(samples[0])==0&&FootprintBalance272.Group(samples[1])==1,"Formal items use existing footprint grouping");
  }
 }
}
