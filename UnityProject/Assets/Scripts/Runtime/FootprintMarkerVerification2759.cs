using System;using System.IO;using System.Collections;using UnityEngine;
namespace GreyHarbor {
 public sealed partial class RaidUI {
  internal void QAMarketShow2759(ItemKind kind){marketMode=0;marketMain=MarketCategories.Main(kind);marketSub=-1;marketWasOpen=true;marketConfirm=false;var goods=MarketGoodsSorted2758(marketMain,marketSub);int index=goods.IndexOf(kind);marketScroll=Mathf.Max(0,(index/4-1)*206);}
 }
 public sealed partial class RaidVerification {
  IEnumerator RunFootprintMarkers2759(){
   Check(G.VerifyMode&&G.SavePath.Contains("verification"),"2759 isolated current UI verification");
   string dir=Path.GetDirectoryName(G.SavePath);var names=new[]{"stash-v40.json","stash-v41.json","stash-v42.json"};var before=Array.ConvertAll(names,n=>File.Exists(Path.Combine(dir,n))?File.ReadAllBytes(Path.Combine(dir,n)):null);
   var card=new Rect(0,0,271,194);
   foreach(var shape in new[]{new Vector2Int(1,1),new Vector2Int(2,2),new Vector2Int(1,3),new Vector2Int(3,1),new Vector2Int(3,3)}){
    var cells=RaidUI.FootprintCells2759(card,shape.x,shape.y);Check(cells.Length==shape.x*shape.y,"2759 actual marker matches occupied cells "+shape);
    for(int i=0;i<cells.Length;i++)Check(cells[i].x==cells[0].x+i%shape.x*8&&cells[i].y==cells[0].y+i/shape.x*8,"2759 footprint rows columns follow real shape "+shape+"/"+i);
    Check(cells[cells.Length-1].xMax==card.xMax-5&&cells[cells.Length-1].yMax==card.yMax-5,"2759 marker anchored to outer card bottom right "+shape);
   }
   FreshCamp();G.enabled=false;ResetItems2758();
   var square=ItemKind.ShowcasePortableMicrofilmReader;var tall=ShapeKind2758(1,3);Check(RaidInventory.Width(square)==2&&RaidInventory.Height(square)==2,"2759 reference microfilm reader is actual two by two");
   Put2758(ItemArea.Warehouse,square,0);Put2758(ItemArea.Warehouse,tall,4);Put2758(ItemArea.Warehouse,tall,8,true);
   foreach(var kind in new[]{ItemKind.GreenMedkit,ItemKind.Medkit,ItemKind.Ammo,ItemKind.Cola})Put2758(ItemArea.Backpack,kind,G.Bag.FreeCell(kind),false,kind==ItemKind.Ammo?81:2);
   Put2758(ItemArea.Backpack,square,5);G.NoticeTime=0;var ui=G.GetComponent<RaidUI>();ui.enabled=true;
   yield return NativeInventoryPhoto2758("2762-inventory-without-markers.png");
   G.Panel=CampPanel.Market;G.Player.transform.position=RaidMap.Market;ui.QAMarketShow2759(square);yield return NativeInventoryPhoto2758("2762-market-keeps-size-marker.png");
   ui.QAMarketShow2759(tall);yield return NativeInventoryPhoto2758("2762-market-long-item-marker.png");
   for(int i=0;i<names.Length;i++)Check(MainBytes273(before[i],File.Exists(Path.Combine(dir,names[i]))?File.ReadAllBytes(Path.Combine(dir,names[i])):null),"2759 formal save byte protection "+names[i]);
   results.Add("FOOTPRINT MARKERS 2759 COMPLETE");File.WriteAllLines(Path.Combine(folder,"2762-footprint-ui-verification.txt"),results);
  }
 }
}
