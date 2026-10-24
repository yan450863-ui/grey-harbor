using System;
using UnityEngine;
namespace GreyHarbor {
 public sealed partial class RaidUI {
  int marketMode,marketMain=-1,marketSub=-1,marketExpanded=-1;float marketScroll,marketCategoryScroll,marketHistoryScroll;
  ItemKind marketKind=ItemKind.Ammo;string marketQuantity="100";bool marketConfirm,marketWasOpen;
  bool CloseMarketDetail315(){if(!marketConfirm)return false;marketConfirm=false;return true;}
  void ResetMarketUI(){if(!marketWasOpen)return;marketConfirm=marketWasOpen=false;selling=saleConfirm=false;saleIds.Clear();ClearItemUI();}
  void MarketFilter(int main,int sub){marketMain=main;marketSub=sub;marketScroll=warehouseScroll=0;saleIds.Clear();}
  void MarketUI(){
   if(!marketWasOpen){ClearItemUI();marketConfirm=saleConfirm=false;selling=false;saleIds.Clear();marketMode=0;marketWasOpen=true;}viewLocked=false;
   Fill(new Rect(0,0,1600,900),new Color(.025f,.034f,.045f,.99f));
   Frame(new Rect(155,84,1418,753),panel,edge);Frame(new Rect(155,26,265,58),inset,edge);Label(177,40,225,32,$"金币  {G.Save.Coins:N0}",24,gold);
   Label(441,37,670,35,"交易行  /  "+new[]{"购买","出售","交易记录"}[marketMode],28,white);
   if(PanelButton(new Rect(1456,30,116,42),"× 退出")){G.CloseInventory();ResetMarketUI();return;}
   bool modal=marketConfirm||saleConfirm;GUI.enabled=!modal;
   string[] modes={"购买","出售","交易记录"};for(int i=0;i<3;i++)if(PanelButton(new Rect(28,103+i*77,112,64),modes[i],marketMode==i)){marketMode=i;marketScroll=warehouseScroll=marketHistoryScroll=0;saleIds.Clear();ClearItemUI();}
   if(marketMode<2)MarketSidebar();else {Label(177,113,218,31,"交易记录",23,mint);Label(177,163,218,28,"最近200笔 · 最新在上",15,muted);Label(177,206,218,28,"本版本起记录成功买卖",15,muted);Label(177,248,218,28,"关闭游戏后仍然保留",15,muted);}
   selling=marketMode==1;
   if(marketMode==0)MarketGoods();else if(marketMode==1)MarketWarehouse();else MarketHistory();
   Label(166,854,1388,26,G.NoticeTime>0?G.Notice:marketMode==0?"点击商品选择数量 · 普通物品买价加15%；钥匙按标价出售，每把4次":marketMode==1?"点击物品选择整叠出售 · 筛选不改变仓库位置 · 滚轮浏览":"成功交易才会记录，失败或取消不会产生记录。",16,G.NoticeTime>0?gold:muted);
   GUI.enabled=true;if(marketConfirm)MarketPurchase();else if(saleConfirm)DrawSaleConfirm();
  }
  void MarketSidebar(){
   if(PanelButton(new Rect(170,105,225,38),"全部物品",marketMain<0))MarketFilter(-1,-1);
   var viewport=new Rect(170,155,225,648);float content=7*53+(marketExpanded<0?0:MarketCategories.Children[marketExpanded].Length*38);float max=Mathf.Max(0,content-viewport.height);
   if(GUI.enabled&&Event.current.type==EventType.ScrollWheel&&viewport.Contains(Event.current.mousePosition)){marketCategoryScroll=Mathf.Clamp(marketCategoryScroll+Event.current.delta.y*30,0,max);Event.current.Use();}marketCategoryScroll=Mathf.Clamp(marketCategoryScroll,0,max);
   GUI.BeginGroup(viewport);float y=-marketCategoryScroll;for(int i=0;i<7;i++){
    if(PanelButton(new Rect(0,y,184,43),MarketCategories.Names[i],marketMain==i&&marketSub<0)){MarketFilter(i,-1);marketExpanded=i;}
    if(PanelButton(new Rect(185,y,39,43),marketExpanded==i?"−":"+"))marketExpanded=marketExpanded==i?-1:i;y+=53;
    if(marketExpanded==i)for(int s=0;s<MarketCategories.Children[i].Length;s++){if(PanelButton(new Rect(15,y,209,32),MarketCategories.Children[i][s],marketMain==i&&marketSub==s))MarketFilter(i,s);y+=38;}
   }GUI.EndGroup();
  }
  float MarketWheel(Rect viewport,float scroll,float height){float max=Mathf.Max(0,height-viewport.height);if(GUI.enabled&&Event.current.type==EventType.ScrollWheel&&viewport.Contains(Event.current.mousePosition)){scroll=Mathf.Clamp(scroll+Event.current.delta.y*55,0,max);Event.current.Use();}scroll=Mathf.Clamp(scroll,0,max);if(max>0){float h=Mathf.Max(32,viewport.height*viewport.height/height);Fill(new Rect(viewport.xMax+8,viewport.y,4,viewport.height),edge);Fill(new Rect(viewport.xMax+8,viewport.y+(viewport.height-h)*scroll/max,4,h),muted);}return scroll;}
  string MarketTitle()=>marketMain<0?"全部物品":MarketCategories.Names[marketMain]+(marketSub<0?"":" / "+MarketCategories.Children[marketMain][marketSub]);
  internal static System.Collections.Generic.List<ItemKind> MarketGoodsSorted2758(int main,int sub){var goods=MarketCategories.Goods(main,sub);goods.Sort((a,b)=>{int price=MarketRules.UnitPrice(a).CompareTo(MarketRules.UnitPrice(b));return price!=0?price:((int)a).CompareTo((int)b);});return goods;}
  void MarketGoods(){
   var goods=MarketGoodsSorted2758(marketMain,marketSub);Label(427,107,900,33,MarketTitle(),24,mint);Label(1350,113,198,27,$"{goods.Count} 种商品",16,muted,TextAnchor.MiddleRight);
   var viewport=new Rect(427,160,1120,642);marketScroll=MarketWheel(viewport,marketScroll,Mathf.CeilToInt(goods.Count/4f)*206);
   GUI.BeginGroup(viewport);for(int i=0;i<goods.Count;i++){var k=goods[i];float x=i%4*283,y=i/4*206-marketScroll;if(y+194<0||y>viewport.height)continue;var r=new Rect(x,y,271,194);Frame(r,inset,edge);
    if(WeaponRules279.IsWeapon(k)){var weapon=WeaponRules279.Get(k);ItemFrame(r,k);Label(x+11,y+10,125,27,weapon.Name,20,white);Label(x+132,y+13,126,24,weapon.CaliberLabel,16,muted,TextAnchor.MiddleRight);ItemIcon(new Rect(x+12,y+47,247,104),k);Label(x+11,y+158,110,24,$"仓库已有 {G.Save.Stored[(int)k]}",14,muted);Label(x+130,y+154,128,29,$"{MarketRules.UnitPrice(k):N0} 金币",20,gold,TextAnchor.MiddleRight);}
    else {ItemFrame(new Rect(x+11,y+17,106,115),k);ItemIcon(new Rect(x+17,y+23,94,103),k);if(KeyRules2760.IsKey(k))Label(x+86,y+18,27,19,"4",12,white,TextAnchor.MiddleRight);Label(x+130,y+19,131,29,ShortName(k),KeyRules2760.IsKey(k)?15:18,white);Label(x+130,y+58,131,43,MarketCategories.Names[MarketCategories.Main(k)],14,muted);Label(x+130,y+104,131,28,$"{MarketRules.UnitPrice(k):N0} 金币",18,gold);Label(x+13,y+156,245,25,$"仓库已有 {G.Save.Stored[(int)k]}",15,muted);ItemFootprint2758(r,k);}
    if(GUI.Button(r,GUIContent.none,GUIStyle.none)){marketKind=k;marketQuantity=AmmoRules270.IsAmmo(k)?"100":"1";marketConfirm=true;}
   }if(goods.Count==0)Label(0,155,1120,45,"暂无商品",26,muted,TextAnchor.MiddleCenter);GUI.EndGroup();
  }
  internal static string AdjustMarketQuantity314(string current,int delta){int.TryParse(current,out int q);return Mathf.Clamp(q+delta,1,MarketRules.MaxQuantity).ToString();}
  void MarketPurchase(){
   Fill(new Rect(0,0,1600,900),new Color(0,0,0,.73f));Frame(new Rect(455,156,690,588),panel,edge);
   Label(480,181,565,38,RaidInventory.Name(marketKind),28,white);if(PanelButton(new Rect(1070,177,49,38),"×")){marketConfirm=false;return;}
   ItemFrame(new Rect(480,239,155,155),marketKind);ItemIcon(new Rect(491,250,133,133),marketKind);if(KeyRules2760.IsKey(marketKind))Label(604,240,27,19,"4",12,white,TextAnchor.MiddleRight);Label(661,249,453,29,RaidInventory.Classification(marketKind),18,mint);
   var desc=new GUIStyle(text){wordWrap=true,fontSize=17,alignment=TextAnchor.UpperLeft};desc.normal.textColor=muted;GUI.Label(new Rect(661,295,453,99),RaidInventory.Use(marketKind),desc);
   Label(482,420,630,28,"出售单价 "+RaidInventory.Value(marketKind)+" 金币  ·  购买单价 "+MarketRules.UnitPrice(marketKind)+" 金币",18,muted);
   Label(482,451,630,24,"仓库已有 "+G.Save.Stored[(int)marketKind]+(AmmoRules270.IsAmmo(marketKind)?" 发":" 件"),17,mint);
   Label(482,487,184,33,AmmoRules270.IsAmmo(marketKind)?"购买数量（发）":"购买数量（件）",20,white);
   bool ammunition=AmmoRules270.IsAmmo(marketKind);
   if(ammunition){int[] deltas={-100,-10,-1,1,10,100};float[] xs={675,741,807,931,997,1063};for(int i=0;i<deltas.Length;i++){int delta=deltas[i];if(PanelButton(new Rect(xs[i],483,58,40),delta>0?"+"+delta:delta.ToString()))marketQuantity=AdjustMarketQuantity314(marketQuantity,delta);}}
   else {if(PanelButton(new Rect(807,483,58,40),"−1"))marketQuantity=AdjustMarketQuantity314(marketQuantity,-1);if(PanelButton(new Rect(997,483,58,40),"+1"))marketQuantity=AdjustMarketQuantity314(marketQuantity,1);}
   var input=new GUIStyle(GUI.skin.textField){font=font,fontSize=22,alignment=TextAnchor.MiddleCenter};marketQuantity=GUI.TextField(new Rect(873,483,50+(ammunition?0:58),40),marketQuantity,3,input);
   int.TryParse(marketQuantity,out int quantity);bool ready=MarketRules.Quote(G.Save,marketKind,quantity,out int price,out string reason);Label(482,546,631,42,price<0?"请输入有效数量":$"合计  {price:N0} 金币",28,gold);Label(482,602,631,29,ready?"确认后直接存入主仓库。":reason,17,ready?muted:red);
   if(PanelButton(new Rect(480,661,232,53),"取消"))marketConfirm=false;if(PanelButton(new Rect(730,661,387,53),"确认购买",true,!ready||G.CampLocked)){if(G.BuyMarket(marketKind,quantity))marketConfirm=false;}
   ItemFootprint2758(new Rect(455,156,690,588),marketKind);
  }
  void MarketWarehouse(){
   var grid=CampRules.Grid(G.Save);Label(427,107,660,34,"主仓库  /  "+MarketTitle(),24,mint);Label(1190,113,358,28,$"已用 {grid.UsedCells} / {grid.SlotCapacity} 格",17,muted,TextAnchor.MiddleRight);
   var viewport=new Rect(427,160,900,642);warehouseScroll=MarketWheel(viewport,warehouseScroll,grid.Rows*50);
   GUI.BeginGroup(viewport);for(int row=0;row<grid.Rows;row++){float y=row*50-warehouseScroll;if(y+50<0||y>viewport.height)continue;for(int col=0;col<18;col++)Frame(new Rect(col*50,y,50,50),surface,edge*.65f);}
   foreach(var item in grid.Slots){if(!MarketCategories.Matches(item.Kind,marketMain,marketSub))continue;var r=new Rect(item.Cell%18*50+2,item.Cell/18*50-warehouseScroll+2,grid.W(item)*50-4,grid.H(item)*50-4);if(r.yMax<0||r.y>viewport.height)continue;DrawItem(r,item,ItemArea.Warehouse,item.Cell);if(GUI.Button(r,GUIContent.none,GUIStyle.none)){if(!saleIds.Add(item.Id))saleIds.Remove(item.Id);}}
   GUI.EndGroup();Outline(viewport,edge);saleIds.RemoveWhere(id=>!G.Save.Warehouse.Exists(p=>p.Id==id));
   Frame(new Rect(1355,160,192,642),inset,edge);Label(1370,183,161,30,"出售清单",23,white);Label(1370,239,161,28,$"已选 {saleIds.Count} 叠",19,muted);Label(1370,284,161,28,"预计获得",17,muted);Label(1370,322,161,33,$"{SaleValue():N0} 金币",23,gold);
   if(PanelButton(new Rect(1370,393,161,42),"选择筛选结果")){foreach(var p in grid.Slots)if(MarketCategories.Matches(p.Kind,marketMain,marketSub))saleIds.Add(p.Id);}
   if(PanelButton(new Rect(1370,449,161,42),"清空选择"))saleIds.Clear();
   var note=new GUIStyle(text){wordWrap=true,fontSize=15,alignment=TextAnchor.UpperLeft};note.normal.textColor=muted;GUI.Label(new Rect(1370,520,161,130),"出售所选整叠物品。筛选只影响显示，保留实际存放位置。",note);
   if(PanelButton(new Rect(1370,726,161,53),"确认出售",true,saleIds.Count==0||G.CampLocked))saleConfirm=true;
  }
  void MarketHistory(){
   Label(427,107,1050,35,"已完成的交易",24,mint);var records=G.Save.Trades;float height=0;foreach(var record in records)height+=75+Mathf.CeilToInt(record.Items.Count/4f)*30;
   var viewport=new Rect(427,160,1120,642);marketHistoryScroll=MarketWheel(viewport,marketHistoryScroll,height);GUI.BeginGroup(viewport);float y=-marketHistoryScroll;
   foreach(var record in records){float h=65+Mathf.CeilToInt(record.Items.Count/4f)*30;if(y+h>=0&&y<viewport.height){Frame(new Rect(0,y,1120,h),inset,edge);string time=DateTime.TryParse(record.TimeUtc,out var date)?date.ToLocalTime().ToString("yyyy-MM-dd HH:mm:ss"):record.TimeUtc;
    Label(16,y+12,98,30,record.Buying?"购买":"出售",21,record.Buying?white:mint);Label(129,y+15,570,28,time,17,muted);Label(802,y+12,300,32,(record.Buying?"−":"+")+record.Coins.ToString("N0")+" 金币",23,record.Buying?gold:mint,TextAnchor.MiddleRight);
    for(int i=0;i<record.Items.Count;i++){var p=record.Items[i];Label(16+i%4*276,y+51+i/4*30,268,25,RaidInventory.Name(p.Kind)+" ×"+p.Count,16,white);}}
    y+=h+10;
   }if(records.Count==0)Label(0,155,1120,45,"暂无交易记录",26,muted,TextAnchor.MiddleCenter);GUI.EndGroup();
  }
 }
}
