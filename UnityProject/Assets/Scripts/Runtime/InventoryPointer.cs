using System.Collections.Generic;
using UnityEngine;
namespace GreyHarbor {
 public sealed partial class RaidUI {
  struct SlotHit {public Rect Rect;public ItemArea Area;public int Cell;}
  readonly List<SlotHit> hits=new List<SlotHit>();
  bool dragging,context,tooltip,splitOpen;ItemArea dragArea,selectedArea;int dragCell;string selectedId,splitText="1";ItemStack dragItem;Vector2 dragStart,popupPos;
  float lastItemClick=-10;string lastClickId;object lastClickContainer;
  void ClearItemUI(){dragging=context=tooltip=splitOpen=quickBind337=false;selectedId=null;lastItemClick=-10;dragItem=null;}
  ItemStack Selected(){if(string.IsNullOrEmpty(selectedId))return null;var gear=G.GearAt(selectedArea);if(gear!=null&&gear.Id==selectedId)return gear;return G.InventoryFor(selectedArea)?.Slots.Find(p=>p.Id==selectedId);}
  bool Hit(Vector2 p,out SlotHit hit){for(int i=hits.Count-1;i>=0;i--)if(hits[i].Rect.Contains(p)){hit=hits[i];return true;}hit=default;return false;}
  bool RepairAvailable()=>G.Phase==GamePhase.Base&&Selected()!=null&&ArmorBalance.Defensive(Selected().Kind)&&ArmorBalance.RepairPrice(Selected())>0;
  Rect PopupRect()=>ItemPopupRect337();
  internal void QAConfigureGrid2758(ItemArea area,Rect viewport,float scroll=0){hits.Clear();gridTargets.Clear();gridTargets.Add(new GridTarget{Area=area,Viewport=viewport,Scroll=scroll});var inv=G.InventoryFor(area);for(int i=0;i<inv.SlotCapacity;i++)hits.Add(new SlotHit{Area=area,Cell=i,Rect=new Rect(viewport.x+i%inv.Columns*Tile,viewport.y+i/inv.Columns*Tile-scroll,Tile,Tile)});}
  internal string QADebugPointer2758(Vector2 p){string hit=Hit(p,out var h)?h.Area+"/"+h.Cell+"/"+(G.ItemAt(h.Area,h.Cell)?.Id??"empty"):"none";return "hit="+hit+" dragging="+dragging+" context="+context+" tooltip="+tooltip+" split="+splitOpen+" selling="+selling;}
  internal ItemStack QADragItem2758=>dragItem;
  internal bool QADragging2758=>dragging;
  internal void QAInput2758(Event ev)=>HandleItemInput(ev);
  void HandleItemInput()=>HandleItemInput(Event.current);
  void HandleItemInput(Event ev){
   if(HandleQuickBinding329(ev))return;
   if(HandlePopupInput337(ev))return;
   if(dragging&&ev.type==EventType.KeyDown&&ev.keyCode==KeyCode.Escape){ClearItemUI();ev.Use();return;}
   if(dragging&&dragItem!=null&&ev.type==EventType.KeyDown&&ev.keyCode==KeyCode.Q){dragItem.Rotated=!dragItem.Rotated;tooltip=false;ev.Use();return;}
   if(G.OpenKeyCase314!=null&&keyCasePanel314.Contains(ev.mousePosition)&&!gridTargets[gridTargets.Count-1].Viewport.Contains(ev.mousePosition)){if(ev.type==EventType.MouseDown){ev.Use();return;}}
   if(context||splitOpen){if(ev.type==EventType.MouseDown&&!PopupRect().Contains(ev.mousePosition)){ClearItemUI();ev.Use();}return;}
   if(tooltip&&ev.type==EventType.MouseDown&&PopupRect().Contains(ev.mousePosition)){ev.Use();return;}
   if(ev.type==EventType.MouseDown&&(ev.button==0||ev.button==1)){
    tooltip=false;if(!Hit(ev.mousePosition,out var h)){selectedId=null;return;}var item=G.ItemAt(h.Area,h.Cell);if(item==null){selectedId=null;return;}
    if(selling){if(h.Area==ItemArea.Warehouse&&ev.button==0){if(!saleIds.Add(item.Id))saleIds.Remove(item.Id);}ev.Use();return;}
    selectedArea=h.Area;selectedId=item.Id;popupPos=new Vector2(Mathf.Clamp(ev.mousePosition.x+18,28,1240),Mathf.Clamp(ev.mousePosition.y+12,100,587));
    if(ev.button==1){quickBind337=false;context=true;dragging=false;lastItemClick=-10;ev.Use();return;}
    object container=G.Panel==CampPanel.Warehouse?(object)G.Save:G.OpenLoot;
    bool twice=ev.clickCount>=2&&Time.unscaledTime-lastItemClick<.65f&&lastClickId==item.Id&&ReferenceEquals(lastClickContainer,container);
    if(twice){G.QuickTransfer(h.Area,item.Cell);ClearItemUI();}
    else {lastItemClick=Time.unscaledTime;lastClickId=item.Id;lastClickContainer=container;dragging=true;dragArea=h.Area;dragCell=item.Cell;dragItem=RaidInventory.Copy(item);dragStart=ev.mousePosition;}
    ev.Use();
   }
   if(ev.type==EventType.MouseUp&&ev.button==0&&dragging){dragging=false;bool moved=Vector2.Distance(ev.mousePosition,dragStart)>6||G.ItemAt(dragArea,dragCell)?.Rotated!=dragItem.Rotated;if(moved){var actual=G.ItemAt(dragArea,dragCell);if(actual!=null&&actual.Id==dragItem.Id&&actual.Count==dragItem.Count&&DropTarget(ev.mousePosition,out var area,out int cell,out var frame,out bool valid)&&valid){G.MoveItem(dragArea,dragCell,area,cell,rotated:dragItem.Rotated);}ClearItemUI();}else tooltip=true;ev.Use();}
  }
  void WrappedLabel(Rect r,string message,int size,Color c){text.wordWrap=true;Label(r.x,r.y,r.width,r.height,message,size,c);text.wordWrap=false;}
  void DrawItemPopup(){if(!tooltip&&!context&&!splitOpen)return;var item=Selected();if(item==null){ClearItemUI();return;}var r=PopupRect();Frame(r,new Color(.045f,.058f,.069f,.99f),RaidInventory.Tint(item.Kind)*.8f);
   Label(r.x+14,r.y+12,r.width-28,28,ShortName(item.Kind),20,RaidInventory.Tint(item.Kind));
   if(context){DrawContext337(r,item);return;}
   if(splitOpen){Label(r.x+14,r.y+52,299,23,$"当前 {item.Count}，可分出 1～{item.Count-1}",16,muted);var inputStyle=new GUIStyle(GUI.skin.textField){font=font,fontSize=20,alignment=TextAnchor.MiddleCenter};splitText=GUI.TextField(new Rect(r.x+74,r.y+91,180,39),splitText,6,inputStyle);if(PanelButton(new Rect(r.x+14,r.y+91,48,39),"−")){int.TryParse(splitText,out int n);splitText=Mathf.Max(1,n-1).ToString();}if(PanelButton(new Rect(r.x+267,r.y+91,48,39),"+")){int.TryParse(splitText,out int n);splitText=Mathf.Min(item.Count-1,n+1).ToString();}Label(r.x+14,r.y+143,301,24,"分出的物品放入同一容器的空位",14,muted);bool valid=int.TryParse(splitText,out int amount)&&amount>0&&amount<item.Count;if(PanelButton(new Rect(r.x+14,r.y+181,132,36),"取消"))ClearItemUI();if(PanelButton(new Rect(r.x+158,r.y+181,158,36),"确认分开",true,!valid)){G.SplitItem(selectedArea,item.Cell,amount);ClearItemUI();}return;}
   DrawDetails337(r,item);
  }
  struct GridTarget {public ItemArea Area;public Rect Viewport;public float Scroll;}
  readonly List<GridTarget> gridTargets=new List<GridTarget>();
  internal static Vector2Int CenteredCell(Vector2 p,Rect viewport,float scroll,int width,int height)=>new Vector2Int(Mathf.FloorToInt((p.x-viewport.x)/Tile-width*.5f+.5f),Mathf.FloorToInt((p.y-viewport.y+scroll)/Tile-height*.5f+.5f));
  internal static Rect CenteredGhost(Vector2 p,ItemKind kind)=>new Rect(p.x-RaidInventory.Width(kind)*Tile*.5f,p.y-RaidInventory.Height(kind)*Tile*.5f,RaidInventory.Width(kind)*Tile,RaidInventory.Height(kind)*Tile);
  internal static Rect CenteredGhost(Vector2 p,ItemStack item)=>new Rect(p.x-RaidInventory.Width(item)*Tile*.5f,p.y-RaidInventory.Height(item)*Tile*.5f,RaidInventory.Width(item)*Tile,RaidInventory.Height(item)*Tile);
  bool DropTarget(Vector2 p,out ItemArea area,out int cell,out Rect frame,out bool valid){
   area=default;cell=-1;frame=default;valid=false;
   if(G.OpenKeyCase314!=null&&keyCasePanel314.Contains(p)&&!new Rect(1350,561,100,100).Contains(p))return false;
   for(int i=gridTargets.Count-1;i>=0;i--){var g=gridTargets[i];if(!g.Viewport.Contains(p))continue;area=g.Area;var inv=G.InventoryFor(area);if(inv.Partitioned338)return DropPartition338(p,g,inv,out cell,out frame,out valid);var xy=CenteredCell(p,g.Viewport,g.Scroll,RaidInventory.Width(dragItem),RaidInventory.Height(dragItem));
    frame=Intersection(new Rect(g.Viewport.x+xy.x*Tile,g.Viewport.y+xy.y*Tile-g.Scroll,RaidInventory.Width(dragItem)*Tile,RaidInventory.Height(dragItem)*Tile),g.Viewport);
    if(xy.x<0||xy.y<0||xy.x+RaidInventory.Width(dragItem)>inv.Columns||xy.y+RaidInventory.Height(dragItem)>inv.Rows)return true;
    cell=xy.y*inv.Columns+xy.x;valid=G.CanMoveItem(dragArea,dragCell,area,cell,dragItem.Rotated);return true;
   }
   if(G.OpenKeyCase314!=null&&keyCasePanel314.Contains(p))return false;
   if(Hit(p,out var target)){area=target.Area;cell=0;frame=target.Rect;valid=G.CanMoveItem(dragArea,dragCell,area,cell,dragItem.Rotated);return true;}return false;
  }
  void DrawDragGhost(){if(!dragging||dragItem==null||(Vector2.Distance(Event.current.mousePosition,dragStart)<6&&G.ItemAt(dragArea,dragCell)?.Rotated==dragItem.Rotated))return;var p=Event.current.mousePosition;var r=CenteredGhost(p,dragItem);bool hasTarget=DropTarget(p,out var area,out int cell,out var frame,out bool okay);
   var old=GUI.color;GUI.color=new Color(1,1,1,.65f);ItemFrame(r,dragItem.Kind);ItemIcon(new Rect(r.x+3,r.y+3,r.width-6,r.height-6),dragItem.Kind,dragItem.Rotated);if(KeyRules2760.IsKey(dragItem.Kind))Label(r.xMax-31,r.y+1,27,19,ItemCornerText2760(dragItem),12,white,TextAnchor.MiddleRight);GUI.color=old;
   if(hasTarget){Fill(frame,okay?new Color(.4f,.9f,.65f,.12f):new Color(1,.25f,.25f,.15f));Outline(frame,okay?mint:red,3);}
   string label=(okay?"松开放入 / 互换":"无法放入")+" · Q 横竖旋转";Label(r.x,r.yMax+4,300,23,label,14,okay?mint:red);
  }
 }
}
