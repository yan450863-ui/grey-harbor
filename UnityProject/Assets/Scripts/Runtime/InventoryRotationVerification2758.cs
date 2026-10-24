using System;using System.IO;using System.Collections;using System.Collections.Generic;using UnityEngine;
namespace GreyHarbor {
 // QA-only dispatcher: Unity Event reads require an active native GUI context.
 // It invokes the same pointer production methods; no gameplay event handling is changed.
 public sealed class InventoryPointerGuiHarness2758:MonoBehaviour {
  public Action Work;public bool Done;public Exception Failure;
  void OnGUI(){if(Done||Work==null||Event.current.type!=EventType.Repaint)return;try{Work();}catch(Exception ex){Failure=ex;}finally{Work=null;Done=true;}}
 }
 public sealed partial class RaidVerification {
 readonly Rect qaGrid2758=new Rect(100,100,900,1000);
 ItemKind ShapeKind2758(int w,int h){for(int i=0;i<RaidInventory.KindCount;i++){var k=(ItemKind)i;if(!RaidInventory.IsWearable(k)&&RaidInventory.Width(k)==w&&RaidInventory.Height(k)==h)return k;}throw new Exception("No collectible shape "+w+"x"+h);}
 void ResetItems2758(){G.Panel=CampPanel.Warehouse;G.InventoryOpen=true;G.Paused=false;G.CampLocked=false;G.Chest.Slots.Clear();G.Bag.Slots.Clear();G.Safe.Slots.Clear();G.Save.Warehouse.Clear();G.Magazine=0;}
 ItemStack Put2758(ItemArea area,ItemKind kind,int cell,bool rotated=false,int count=1){var p=new ItemStack(kind,count,ItemOrigin.Certified){Cell=cell,Rotated=rotated,RaidFound=true};var inv=G.InventoryFor(area);Check(inv.Fits(p,cell),"2758 fixture footprint fits "+area+" "+kind);inv.Slots.Add(p);return p;}
 ItemStack Find2758(ItemArea area,string id)=>G.InventoryFor(area).Slots.Find(p=>p.Id==id);
 Vector2 Center2758(ItemArea area,int cell,ItemStack p){var inv=G.InventoryFor(area);return qaGrid2758.position+new Vector2((cell%inv.Columns+RaidInventory.Width(p)*.5f)*50,(cell/inv.Columns+RaidInventory.Height(p)*.5f)*50);}
 void Pointer2758(RaidUI ui,EventType type,Vector2 point,KeyCode key=KeyCode.None){
  // Unity's native Event setters can change the event category. Set only relevant data, then assign the requested type last.
  var ev=new Event();if(type==EventType.KeyDown||type==EventType.KeyUp)ev.keyCode=key;else {ev.button=0;ev.mousePosition=point;ev.clickCount=1;}ev.type=type;
  Debug.Log("INFO 2758 pointer before requested="+type+" actual="+ev.type+" button="+ev.button+" key="+ev.keyCode+" position="+ev.mousePosition+" "+ui.QADebugPointer2758(point));
  Check(ev.type==type,"2758 injected event retains requested native type "+type);ui.QAInput2758(ev);
  Debug.Log("INFO 2758 pointer after actual="+ev.type+" "+ui.QADebugPointer2758(point));
 }
 void Begin2758(RaidUI ui,ItemArea area,ItemStack p){ui.QAConfigureGrid2758(area,qaGrid2758);Pointer2758(ui,EventType.MouseDown,Center2758(area,p.Cell,p));Check(ui.QADragging2758&&ui.QADragItem2758!=p&&ui.QADragItem2758.Id==p.Id,"2758 actual pointer begins copied drag "+area+" "+p.Kind);}
 void Drop2758(RaidUI ui,ItemArea area,int cell){var p=RaidInventory.Copy(ui.QADragItem2758);ui.QAConfigureGrid2758(area,qaGrid2758);Pointer2758(ui,EventType.MouseUp,Center2758(area,cell,p));Check(!ui.QADragging2758,"2758 actual pointer release finishes drag");}
 string Payload2758(){var rows=new List<string>();foreach(var a in new[]{ItemArea.Backpack,ItemArea.Rig,ItemArea.Safe,ItemArea.Warehouse})foreach(var p in G.InventoryFor(a).Slots)rows.Add(p.Id+":"+p.Kind+":"+p.Count+":"+p.Rotated+":"+p.Origin+":"+p.RaidFound+":"+p.Identified);rows.Sort(StringComparer.Ordinal);return string.Join("|",rows);}
 void GridValid2758(ItemArea area){var inv=G.InventoryFor(area);var staged=new RaidInventory(inv.Columns,inv.Rows,inv.WarehouseMode);foreach(var p in inv.Slots){Check(staged.Fits(p,p.Cell),"2758 no overlapping or out-of-bounds footprint "+area+" "+p.Id);staged.Slots.Add(RaidInventory.Copy(p));}}
 void MarketOrder2758(int main,int sub){var list=RaidUI.MarketGoodsSorted2758(main,sub);var expected=MarketCategories.Goods(main,sub);Check(list.Count==expected.Count&&new HashSet<ItemKind>(list).SetEquals(expected),"2758 market category retains exact goods "+main+"/"+sub);bool ordered=true;for(int i=1;i<list.Count;i++){int before=MarketRules.UnitPrice(list[i-1]),after=MarketRules.UnitPrice(list[i]);ordered&=before<after||before==after&&(int)list[i-1]<(int)list[i];}Check(ordered,"2758 actual displayed buy prices ascending with stable ties "+main+"/"+sub);}
 IEnumerator NativeInventoryPhoto2758(string name){G.NoticeTime=0;yield return null;yield return null;yield return new WaitForEndOfFrame();var image=ScreenCapture.CaptureScreenshotAsTexture();int lit=0;if(image)foreach(var c in image.GetPixels32())if(c.r>20||c.g>20||c.b>20)lit++;if(image&&lit>5000){File.WriteAllBytes(Path.Combine(folder,name),image.EncodeToPNG());Check(true,"2758 native OnGUI screenshot saved "+name);}else {results.Add("INFO 2758 native UI screenshot unavailable from hidden swapchain; interface checks do not replace visual review.");}if(image)Destroy(image);}
 IEnumerator RunInventoryRotation2758(){
  string dir=Path.GetDirectoryName(G.SavePath);var names=new[]{"stash-v40.json","stash-v41.json","stash-v42.json"};var saves=Array.ConvertAll(names,n=>File.Exists(Path.Combine(dir,n))?File.ReadAllBytes(Path.Combine(dir,n)):null);bool enabled=G.enabled;var controls=G.Controls;
  Check(G.VerifyMode&&G.SavePath.EndsWith("verification-stash-2.7.58.json",StringComparison.OrdinalIgnoreCase),"2758 isolated verification save path");var harnessObject=new GameObject("QA-native-inventory-event-dispatcher-2758");var harness=harnessObject.AddComponent<InventoryPointerGuiHarness2758>();
  try{
   harness.Work=()=>{
   G.enabled=false;FreshCamp();G.enabled=false;G.Controls=new TouchInput();var ui=G.GetComponent<RaidUI>();ui.enabled=false;
   MarketOrder2758(-1,-1);for(int m=0;m<MarketCategories.Names.Length;m++){MarketOrder2758(m,-1);for(int s=0;s<MarketCategories.Children[m].Length;s++)MarketOrder2758(m,s);}
   for(int i=0;i<RaidInventory.KindCount;i++){var k=(ItemKind)i;Check(RaidUI.FootprintMarkerCount2758(k)==(WeaponRules279.IsWeapon(k)?0:RaidInventory.Width(k)*RaidInventory.Height(k)),"2758 footprint marker count/excluded weapon "+k);}
   var kinds=new[]{ShapeKind2758(1,2),ShapeKind2758(1,3),ShapeKind2758(2,2),ShapeKind2758(2,1),ItemKind.AK};
   // No 1x4/4x1 collectible currently exists; verify generic future anchor geometry without altering live item definitions.
   Check(RaidUI.CenteredCell(new Vector2(125,200),qaGrid2758,0,1,4)==Vector2Int.zero&&RaidUI.CenteredCell(new Vector2(200,125),qaGrid2758,0,4,1)==Vector2Int.zero,"2758 future four-cell strip centered geometry handles both orientations");
   foreach(var area in new[]{ItemArea.Backpack,ItemArea.Warehouse})foreach(var k in kinds){
    ResetItems2758();var source=Put2758(area,k,0);string id=source.Id;int w=RaidInventory.Width(k),h=RaidInventory.Height(k);Begin2758(ui,area,source);
    Pointer2758(ui,EventType.KeyDown,Vector2.zero,KeyCode.Q);var drag=ui.QADragItem2758;Check(drag.Rotated&&RaidInventory.Width(drag)==h&&RaidInventory.Height(drag)==w&&!source.Rotated,"2758 real Q changes drag footprint only "+area+" "+k);var ghost=RaidUI.CenteredGhost(new Vector2(400,400),drag);Check(ghost.width==h*50&&ghost.height==w*50&&ghost.center==new Vector2(400,400),"2758 rotated ghost uses complete centered footprint "+k);
    Pointer2758(ui,EventType.KeyDown,Vector2.zero,KeyCode.Q);Check(!drag.Rotated&&RaidInventory.Width(drag)==w&&RaidInventory.Height(drag)==h,"2758 second Q restores original orientation without 180 degrees "+k);
    Pointer2758(ui,EventType.KeyDown,Vector2.zero,KeyCode.Q);Drop2758(ui,area,0);Check(Find2758(area,id)?.Rotated==true&&Find2758(area,id).Cell==0,"2758 real same-anchor rotated drop committed "+area+" "+k);GridValid2758(area);
    source=Find2758(area,id);Begin2758(ui,area,source);Pointer2758(ui,EventType.KeyDown,Vector2.zero,KeyCode.Q);Pointer2758(ui,EventType.KeyDown,Vector2.zero,KeyCode.Escape);Check(!ui.QADragging2758&&Find2758(area,id).Rotated,"2758 Escape cancels preview without changing saved rotation "+area+" "+k);
   }
   // Actual drag from bag to warehouse; ID, count and certification survive.
   ResetItems2758();var rect=Put2758(ItemArea.Backpack,kinds[0],0);string rectId=rect.Id;Begin2758(ui,ItemArea.Backpack,rect);Pointer2758(ui,EventType.KeyDown,Vector2.zero,KeyCode.Q);Drop2758(ui,ItemArea.Warehouse,5);Check(G.Bag.Slots.Count==0&&Find2758(ItemArea.Warehouse,rectId)?.Rotated==true&&Find2758(ItemArea.Warehouse,rectId).Origin==ItemOrigin.Certified,"2758 actual cross-grid rotated drag preserves unique item");
   rect=Find2758(ItemArea.Warehouse,rectId);Begin2758(ui,ItemArea.Warehouse,rect);Drop2758(ui,ItemArea.Backpack,5);Check(Find2758(ItemArea.Backpack,rectId)?.Rotated==true&&G.Save.Warehouse.Count==0,"2758 actual reverse drag preserves orientation");
   // Edge and multi-item collision rejection must be transactional.
   ResetItems2758();rect=Put2758(ItemArea.Backpack,kinds[0],0);rectId=rect.Id;Begin2758(ui,ItemArea.Backpack,rect);Pointer2758(ui,EventType.KeyDown,Vector2.zero,KeyCode.Q);Pointer2758(ui,EventType.MouseUp,new Vector2(qaGrid2758.x+249,qaGrid2758.y+125));Check(Find2758(ItemArea.Backpack,rectId)?.Cell==0&&!Find2758(ItemArea.Backpack,rectId).Rotated&&G.Bag.Slots.Count==1,"2758 actual centered edge drop rejected without mutation");
   var a=Put2758(ItemArea.Backpack,ItemKind.Cola,7);var b=Put2758(ItemArea.Backpack,ItemKind.Cloth,8);string payload=Payload2758();Begin2758(ui,ItemArea.Backpack,Find2758(ItemArea.Backpack,rectId));Pointer2758(ui,EventType.KeyDown,Vector2.zero,KeyCode.Q);Drop2758(ui,ItemArea.Backpack,7);Check(Payload2758()==payload,"2758 actual rotated footprint covering two items rejected atomically");
   // Swap into a single occupied footprint and back keeps both rotations.
   ResetItems2758();rect=Put2758(ItemArea.Backpack,kinds[0],0,true);a=Put2758(ItemArea.Warehouse,ItemKind.Cola,4);string aId=a.Id;rectId=rect.Id;Check(G.MoveItem(ItemArea.Backpack,0,ItemArea.Warehouse,4,rotated:true)&&Find2758(ItemArea.Warehouse,rectId)?.Rotated==true&&Find2758(ItemArea.Backpack,aId)?.Cell==0,"2758 rotated swap exchanges real grid items while retaining metadata");GridValid2758(ItemArea.Backpack);GridValid2758(ItemArea.Warehouse);
   // Warehouse collectible stacking retains target orientation, splitting copies it.
   ResetItems2758();a=Put2758(ItemArea.Warehouse,kinds[0],0,true,6);b=Put2758(ItemArea.Warehouse,kinds[0],10,false,3);aId=a.Id;string bId=b.Id;Check(G.MoveItem(ItemArea.Warehouse,10,ItemArea.Warehouse,0,rotated:false)&&Find2758(ItemArea.Warehouse,aId)?.Count==9&&Find2758(ItemArea.Warehouse,aId).Rotated&&Find2758(ItemArea.Warehouse,bId)==null,"2758 rotated/unrotated compatible merge preserves destination orientation and exact quantity");
   Check(G.SplitItem(ItemArea.Warehouse,0,4),"2758 split rotated warehouse stack through real transaction");var pieces=G.Save.Warehouse.FindAll(p=>p.Kind==kinds[0]);Check(pieces.Count==2&&pieces.TrueForAll(p=>p.Rotated)&&pieces[0].Id!=pieces[1].Id&&pieces[0].Count+pieces[1].Count==9,"2758 split creates unique ID and keeps direction/quantity");GridValid2758(ItemArea.Warehouse);
   payload=Payload2758();Check(G.SortWarehouse()&&Payload2758()==payload,"2758 warehouse sort preserves IDs quantity provenance and orientation");GridValid2758(ItemArea.Warehouse);
   // Normal storage writes and loads, including carry orientation.
   Put2758(ItemArea.Backpack,kinds[2],0,true);payload=Payload2758();Check(G.SortWarehouse(),"2758 camp transaction persists rotated carry plus warehouse");G.LoadSave();G.BuildCamp();G.enabled=false;G.Panel=CampPanel.Warehouse;G.InventoryOpen=true;Check(Payload2758()==payload,"2758 real save/load rebuild restores all rotations IDs and amounts");GridValid2758(ItemArea.Backpack);GridValid2758(ItemArea.Warehouse);
   var old=JsonUtility.FromJson<ItemStack>("{\"Kind\":"+(int)kinds[0]+",\"Count\":1,\"Origin\":0,\"Cell\":0,\"RaidFound\":false,\"Identified\":true,\"Id\":\"old-no-rotation\"}");var oldGrid=new RaidInventory(5,4);oldGrid.Restore(new List<ItemStack>{old});Check(!oldGrid.Slots[0].Rotated&&oldGrid.Slots[0].Id=="old-no-rotation"&&oldGrid.W(oldGrid.Slots[0])==1&&oldGrid.H(oldGrid.Slots[0])==2,"2758 old JSON lacking Rotated defaults safely without ID/shape loss");
   // Save write failure rolls back changed orientation and the location together.
   ResetItems2758();rect=Put2758(ItemArea.Backpack,kinds[0],0);payload=Payload2758();string save=G.SavePath;string blocker=Path.Combine(folder,"2758-write-blocker");File.WriteAllText(blocker,"block");G.SavePath=Path.Combine(blocker,"stash.json");Check(!G.MoveItem(ItemArea.Backpack,0,ItemArea.Backpack,5,rotated:true)&&Payload2758()==payload&&G.Bag.At(0)?.Cell==0,"2758 failed disk transaction rolls back rotation position ID and quantity");G.SavePath=save;
   ResetItems2758();for(int i=0;i<kinds.Length;i++)Put2758(ItemArea.Warehouse,kinds[i],i*4,i%2==0);Put2758(ItemArea.Backpack,kinds[0],0,true);Put2758(ItemArea.Backpack,kinds[2],5,false);G.Panel=CampPanel.Warehouse;ui.enabled=true;
   };
   float deadline=Time.realtimeSinceStartup+20;while(!harness.Done&&Time.realtimeSinceStartup<deadline)yield return null;
   Check(harness.Done,"2758 native OnGUI Repaint dispatcher completed real pointer tests");if(harness.Failure!=null)throw new Exception("2758 native GUI test failed: "+harness.Failure.Message,harness.Failure);
   yield return NativeInventoryPhoto2758("2758-warehouse-rotated-footprints.png");G.Panel=CampPanel.Market;G.Player.transform.position=RaidMap.Market;yield return NativeInventoryPhoto2758("2758-market-sorted-footprints.png");
   for(int i=0;i<names.Length;i++)Check(MainBytes273(saves[i],File.Exists(Path.Combine(dir,names[i]))?File.ReadAllBytes(Path.Combine(dir,names[i])):null),"2758 formal save byte protection "+names[i]);
  }finally{Destroy(harnessObject);G.GetComponent<RaidUI>().enabled=true;G.enabled=enabled;G.Controls=controls;}
 }
}}
