using System.Collections;using System.IO;using UnityEngine;
namespace GreyHarbor {public sealed partial class RaidVerification {
 IEnumerator CaptureInventoryUI2758(){
  Check(G.VerifyMode&&Application.version=="2.7.58","2758 isolated final UI capture");FreshCamp();G.enabled=false;ResetItems2758();
  var kinds=new[]{ShapeKind2758(1,2),ShapeKind2758(1,3),ShapeKind2758(2,2),ShapeKind2758(2,1),ItemKind.AK};
  for(int i=0;i<kinds.Length;i++)Put2758(ItemArea.Warehouse,kinds[i],i*4,i%2==0);
  Put2758(ItemArea.Backpack,kinds[0],0,true);Put2758(ItemArea.Backpack,kinds[2],5,false);
  G.GetComponent<RaidUI>().enabled=true;G.NoticeTime=0;
  yield return NativeInventoryPhoto2758("2758-warehouse-rotated-footprints.png");
  G.Panel=CampPanel.Market;G.Player.transform.position=RaidMap.Market;
  yield return NativeInventoryPhoto2758("2758-market-sorted-footprints.png");
  results.Add("INVENTORY UI 2758 CAPTURE COMPLETE");File.WriteAllLines(Path.Combine(folder,"2758-ui-capture.txt"),results);
 }
}}
