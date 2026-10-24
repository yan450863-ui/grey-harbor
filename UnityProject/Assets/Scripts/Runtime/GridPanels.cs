using System.Collections.Generic;
using UnityEngine;
namespace GreyHarbor {
 public sealed partial class RaidUI {
  readonly Color surface=new Color(.067f,.082f,.094f),inset=new Color(.085f,.102f,.115f),edge=new Color(.24f,.29f,.31f);
  const float Tile=50;
  bool viewLocked,safeOpen,selling,saleConfirm;float warehouseScroll,lootScroll;object priorContainer;
  readonly HashSet<string> saleIds=new HashSet<string>();
  internal bool SearchVisible=>G.Phase==GamePhase.Raid&&!G.Paused&&!G.InventoryOpen&&G.Nearby!=null&&!G.Nearby.Opened&&G.Nearby.Search>0;
  public static bool ConcealLoot(LootContainer c)=>c!=null&&!c.Opened;
  void Outline(Rect r,Color c,float w=1){Fill(new Rect(r.x,r.y,r.width,w),c);Fill(new Rect(r.x,r.yMax-w,r.width,w),c);Fill(new Rect(r.x,r.y,w,r.height),c);Fill(new Rect(r.xMax-w,r.y,w,r.height),c);}
  void Frame(Rect r,Color fill,Color border){Fill(r,fill);Outline(r,border);}
  bool PanelButton(Rect r,string title,bool accent=false,bool disabled=false){bool locked=viewLocked||disabled;bool hover=!locked&&r.Contains(Event.current.mousePosition);Frame(r,locked?surface:accent?new Color(.24f,.36f,.31f):hover?new Color(.22f,.27f,.29f):inset,accent?mint*.65f:edge);Label(r.x,r.y,r.width,r.height,title,16,locked?muted*.55f:white,TextAnchor.MiddleCenter);return !locked&&GUI.Button(r,GUIContent.none,GUIStyle.none);}
  void Inventory(bool searching=false){
   viewLocked=searching;hits.Clear();gridTargets.Clear();bool raid=G.Phase==GamePhase.Raid;bool warehouse=G.Panel==CampPanel.Warehouse;
   object current=searching?(object)G.Nearby:warehouse?(object)G.Save:G.OpenLoot;
   if(priorContainer!=current){priorContainer=current;ClearItemUI();warehouseScroll=lootScroll=0;ResetCorpseScroll325();selling=saleConfirm=false;saleIds.Clear();}
   if(!searching&&!saleConfirm)HandlePopupInput337(Event.current);
   Fill(new Rect(0,0,1600,900),new Color(.025f,.034f,.045f,.985f));
   Label(28,22,650,43,raid?"局内搜索  /  灰港行动":warehouse?"营地仓库  /  灰港行动":"随身物资  /  灰港行动",29,white);
   Label(28,67,1380,20,(raid?"行动进行中 · 整理物资时仍可能受到攻击":"营地安全区域 · 物品位置与数量自动保存")+$"  ·  负重 {G.CarryGrams320/1000f:0.0}/45kg · {G.LoadTier320.Name}",13,raid?gold:muted);
   if(PanelButton(new Rect(1482,24,89,40),searching?"搜索中":"× 关闭")){G.CloseInventory();ClearItemUI();return;}
   float px=raid?264:28;DrawPersonal(px,searching);
   if(raid)DrawRaidAux();
   if(warehouse)DrawWarehouse();
   else if(searching)DrawLootPanel(G.Nearby,true);
   else if(G.OpenLoot!=null)DrawLootPanel(G.OpenLoot,ConcealLoot(G.OpenLoot));
   else if(G.Panel==CampPanel.Departure)DrawDeparturePanel();
   else if(G.TrainingLoadoutAvailable301)DrawTrainingLoadout301();
   else {float x=raid?800:568;Label(x,125,650,35,raid?"当前未打开搜索容器":"出发准备",24,white);Label(x,177,650,27,raid?"靠近物资点，按住 F 搜索。":"前往北门停留 3 秒，选择地图并确认出发。",17,muted);}
   if(!raid){if(PanelButton(new Rect(46,820,158,34),safeOpen?"关闭安全箱":"安全箱  "+G.Safe.UsedCells+" / "+G.Safe.SlotCapacity))safeOpen=!safeOpen;if(PanelButton(new Rect(214,820,284,34),"一键卸下物资",true,!warehouse)){G.UnloadCarry314();ClearItemUI();}if(safeOpen)DrawCampSafe();}
   if(G.NoticeTime>0)Label(28,861,1515,21,G.Notice,14,gold);
   else Label(28,861,1515,21,searching?"按住 F 搜索 · 松开取消 · 完成后显示物品":"单击 物品说明    双击 快捷转移    拖动 摆放 · Q 横竖旋转    右键 操作菜单 · 胸挂甲包点击 3–7 绑定    Tab / Esc 关闭",14,muted);
   if(!searching){DrawKeyCase314();if(saleConfirm)DrawSaleConfirm();else {HandleItemInput();DrawItemPopup();DrawDragGhost();}}
   viewLocked=false;
  }
  void DrawPersonal(float x,bool searching){
   Frame(new Rect(x,98,492,714),inset,edge);Label(x+16,107,250,25,"随身装备",19,white);Label(x+288,110,186,22,$"生命 {Mathf.CeilToInt(G.Player.Health)} / {PlayerLife321.Maximum:0}",15,muted,TextAnchor.MiddleRight);
   var portrait=new Rect(x+16,145,80,117);Frame(portrait,surface,edge);var sprite=PixelArt.Hero[0];if(sprite!=null){var uv=sprite.textureRect;uv=new Rect(uv.x/sprite.texture.width,uv.y/sprite.texture.height,uv.width/sprite.texture.width,uv.height/sprite.texture.height);GUI.DrawTextureWithTexCoords(new Rect(portrait.x+9,portrait.y+8,62,101),sprite.texture,uv,true);}
   DrawStatusPanel(new Rect(x+106,145,125,55));EquipmentSlot(new Rect(x+106,207,125,55),ItemArea.Armor,"整套防具");
   EquipmentSlot(new Rect(x+241,145,233,55),ItemArea.Weapon,"主武器");EquipmentSlot(new Rect(x+241,207,233,55),ItemArea.SecondWeapon,"副武器");
   Label(x+16,276,170,27,"胸挂",21,white);Label(x+230,278,244,22,$"快捷补给  {G.Chest.UsedCells} / {G.Chest.SlotCapacity}",14,muted,TextAnchor.MiddleRight);
   EquipmentSlot(new Rect(x+16,314,104,101),ItemArea.RigEquipment,"装备胸挂");DrawGrid(G.Chest,ItemArea.Rig,new Rect(x+140,309,300,150),6,3,0);
   Label(x+16,471,170,27,"背包",21,white);Label(x+230,473,244,22,$"储存物资  {G.Bag.UsedCells} / {G.Bag.SlotCapacity}",14,muted,TextAnchor.MiddleRight);
   EquipmentSlot(new Rect(x+16,509,104,101),ItemArea.PackEquipment,"装备背包");DrawGrid(G.Bag,ItemArea.Backpack,new Rect(x+140,504,300,300),6,6,0);
   // Leave the area below the raid backpack empty; no discard panel or extra cells.
  }
  void DrawStatusPanel(Rect r){Frame(r,surface,edge);Label(r.x+5,r.y+2,r.width-10,16,"状态 · "+G.StatusLabel,12,G.Player.Health<35?red:mint);Label(r.x+5,r.y+20,r.width-10,15,$"生命 {Mathf.CeilToInt(G.Player.Health)}/{PlayerLife321.Maximum:0}",11,white);Label(r.x+5,r.y+36,r.width-10,15,$"护甲 {Mathf.CeilToInt(G.TotalArmor)}/{G.MaxArmor:0}",11,muted);}
  void EquipmentSlot(Rect r,ItemArea a,string empty){var item=G.GearAt(a);Frame(r,surface,edge);if(item==null)Label(r.x,r.y,r.width,r.height,empty,14,muted,TextAnchor.MiddleCenter);else DrawItem(r,item,a,0);hits.Add(new SlotHit{Rect=r,Area=a,Cell=0});}
  Texture2D hatchTexture;
  void Hatch(Rect r){if(hatchTexture==null){hatchTexture=new Texture2D(32,32,TextureFormat.RGBA32,false){filterMode=FilterMode.Point,wrapMode=TextureWrapMode.Repeat};for(int y=0;y<32;y++)for(int x=0;x<32;x++)hatchTexture.SetPixel(x,y,(x+y)%8==0?new Color(.16f,.20f,.22f):new Color(.054f,.066f,.075f));hatchTexture.Apply();}GUI.DrawTextureWithTexCoords(r,hatchTexture,new Rect(0,0,r.width/32,r.height/32));Outline(r,edge*.65f);}

  static Rect Intersection(Rect a,Rect b){float x=Mathf.Max(a.x,b.x),y=Mathf.Max(a.y,b.y);return new Rect(x,y,Mathf.Max(0,Mathf.Min(a.xMax,b.xMax)-x),Mathf.Max(0,Mathf.Min(a.yMax,b.yMax)-y));}
  void DrawGrid(RaidInventory inv,ItemArea area,Rect viewport,int visualCols,int visualRows,float scroll){
   if(inv.Partitioned338){DrawPartitionGrid338(inv,area,viewport);return;}
   gridTargets.Add(new GridTarget{Area=area,Viewport=viewport,Scroll=scroll});
   GUI.BeginGroup(viewport);
   for(int row=0;row<visualRows;row++)for(int col=0;col<visualCols;col++){
    var r=new Rect(col*Tile,row*Tile-scroll,Tile,Tile);if(r.yMax<=0||r.y>=viewport.height)continue;bool active=col<inv.Columns&&row<inv.Rows;
    if(!active)Hatch(new Rect(r.x+1,r.y+1,r.width-2,r.height-2));else {Frame(r,surface,edge*.65f);int cell=row*inv.Columns+col;var hit=Intersection(new Rect(viewport.x+r.x,viewport.y+r.y,r.width,r.height),viewport);hits.Add(new SlotHit{Rect=hit,Area=area,Cell=cell});}
   }
   foreach(var item in inv.Slots){int col=item.Cell%inv.Columns,row=item.Cell/inv.Columns;var r=new Rect(col*Tile+2,row*Tile-scroll+2,inv.W(item)*Tile-4,inv.H(item)*Tile-4);if(r.yMax<=0||r.y>=viewport.height)continue;if((area==ItemArea.Loot||CorpseLoot325.IsArea(area))&&!item.Identified)DrawUnknown(r,item);else DrawItem(r,item,area,item.Cell);}
   GUI.EndGroup();Outline(viewport,edge);
  }
  static string AmmoBadgeText320(ItemKind kind){switch(WeaponRules279.AmmoCaliber(kind)){case AmmoCaliber279.NineMM:return "9mm";case AmmoCaliber279.Five56:return "5.56";case AmmoCaliber279.Seven62:return "7.62";case AmmoCaliber279.Gauge12:return "12号";default:return "";}}
  void DrawItem(Rect r,ItemStack item,ItemArea area,int cell){bool chosen=selectedId==item.Id||selling&&saleIds.Contains(item.Id);ItemFrame(r,item.Kind,chosen);if(WeaponRules279.IsWeapon(item.Kind)){DrawInventoryWeapon301(r,item,area);return;}ItemIcon(new Rect(r.x+3,r.y+3,r.width-6,r.height-7),item.Kind,item.Rotated);
   if(r.height>80&&r.width>=72&&!ArmorBalance.Defensive(item.Kind)){var clipping=text.clipping;text.clipping=TextClipping.Clip;try{Label(r.x+(ShowCertification319(item)?22:5),r.y+4,r.width-(item.Count>1?40:10)-(ShowCertification319(item)?17:0),22,ShortName(item.Kind),13,white);}finally{text.clipping=clipping;}}
   if(ConsumableRules.IsRepair(item.Kind))Label(r.x+2,r.y+2,r.width-6,16,RepairKit320.Remaining(item).ToString("0.#"),11,mint,TextAnchor.MiddleRight);
   if(ArmorBalance.Defensive(item.Kind)){ArmorBalance.Ensure(item);string durability320=item.ArmorPoints.ToString("0")+"/"+ArmorBalance.Max(item.Kind).ToString("0");float badgeWidth320=Mathf.Min(r.width-6,durability320.Length*6+4);Label(r.xMax-badgeWidth320-3,r.y+3,badgeWidth320-2,17,durability320,11,white,TextAnchor.MiddleRight);if(r.height>80&&r.width>=72){var clipping=text.clipping;var wrap=text.wordWrap;text.clipping=TextClipping.Clip;text.wordWrap=true;try{Label(r.x+5,r.y+4,Mathf.Max(0,r.width-badgeWidth320-13),32,ShortName(item.Kind),11,white);}finally{text.clipping=clipping;text.wordWrap=wrap;}}}

   if(item.Count>1||KeyRules2760.IsKey(item.Kind)){float countY=selling&&area==ItemArea.Warehouse?r.y+23:r.y+3;Label(r.xMax-31,countY-2,27,19,ItemCornerText2760(item),12,white,TextAnchor.MiddleRight);}
   if(ShowCertification319(item))Label(r.x+3,r.y+2,18,18,"✓",13,mint);
   if(selling&&area==ItemArea.Warehouse){Frame(new Rect(r.xMax-20,r.y+3,17,17),surface,saleIds.Contains(item.Id)?mint:muted);if(saleIds.Contains(item.Id))Label(r.xMax-20,r.y+1,17,19,"✓",14,mint);}
  }
  void ScrollGrid(RaidInventory inv,ItemArea area,Rect rect,ref float scroll){float max=Mathf.Max(0,inv.Rows*Tile-rect.height);if(Event.current.type==EventType.ScrollWheel&&rect.Contains(Event.current.mousePosition)&&!context&&!splitOpen){scroll=Mathf.Clamp(scroll+Event.current.delta.y*Tile,0,max);Event.current.Use();}scroll=Mathf.Clamp(scroll,0,max);DrawGrid(inv,area,rect,inv.Columns,inv.Rows,scroll);if(max>0){Fill(new Rect(rect.xMax+5,rect.y,3,rect.height),edge*.5f);float h=Mathf.Max(32,rect.height*rect.height/(inv.Rows*Tile));Fill(new Rect(rect.xMax+5,rect.y+(rect.height-h)*scroll/max,3,h),muted);}}
  void DrawWarehouse(){
   Frame(new Rect(556,98,1016,737),inset,edge);var grid=CampRules.Grid(G.Save);Label(573,107,340,32,"主仓库",24,white);Label(969,110,519,28,$"金币 {G.Save.Coins:N0}    已用 {grid.UsedCells} / {grid.SlotCapacity} 格",18,gold,TextAnchor.MiddleRight);
   ScrollGrid(grid,ItemArea.Warehouse,new Rect(572,150,900,610),ref warehouseScroll);
   Frame(new Rect(1492,150,62,610),surface,edge);Frame(new Rect(1496,156,54,68),new Color(.16f,.24f,.22f),mint*.6f);Label(1496,162,54,30,"⌂",25,mint,TextAnchor.MiddleCenter);Label(1496,194,54,24,"主仓库",13,white,TextAnchor.MiddleCenter);
   if(!selling){Label(573,784,340,28,"滚轮浏览 · 空位与物品位置保持保存",14,muted);if(PanelButton(new Rect(1020,780,206,40),"整理物品")){G.SortWarehouse();ClearItemUI();}if(PanelButton(new Rect(1238,780,234,40),"批量出售")){selling=true;saleIds.Clear();ClearItemUI();}}
   else {saleIds.RemoveWhere(id=>!G.Save.Warehouse.Exists(p=>p.Id==id));int sum=SaleValue();Label(573,786,410,27,$"已选 {saleIds.Count} 叠 / 共 {sum:N0} 金币",16,gold);if(PanelButton(new Rect(1060,780,160,40),"取消选择")){selling=false;saleIds.Clear();}if(PanelButton(new Rect(1232,780,240,40),"确认出售",true,saleIds.Count==0))saleConfirm=true;}
  }
  int SaleValue(){int n=0;foreach(var p in G.Save.Warehouse)if(saleIds.Contains(p.Id))n+=p.Count*RaidInventory.Value(p);return n;}
  void DrawSaleConfirm(){Fill(new Rect(0,0,1600,900),new Color(0,0,0,.65f));Frame(new Rect(530,300,540,240),inset,edge);Label(554,326,490,38,"出售所选物品",25,white);Label(554,382,490,33,$"{saleIds.Count} 叠物资，共获得 {SaleValue():N0} 金币",20,gold);if(PanelButton(new Rect(554,460,221,46),"返回选择"))saleConfirm=false;if(PanelButton(new Rect(793,460,251,46),"出售",true)){G.SellWarehouse(saleIds);selling=saleConfirm=false;saleIds.Clear();ClearItemUI();}}
  void DrawRaidAux(){Frame(new Rect(28,98,210,132),inset,edge);Label(44,111,170,30,"本局收获",23,white);Label(44,151,170,35,G.RaidGain.ToString("N0"),27,gold);Label(44,197,184,21,"新获随身物资估值",13,muted);
   Frame(new Rect(28,534,210,264),inset,edge);Label(44,547,173,28,"安全箱  "+G.Safe.Columns+"×"+G.Safe.Rows,20,white);DrawGrid(G.Safe,ItemArea.Safe,new Rect(57,587,150,150),G.Safe.Columns,G.Safe.Rows,0);Label(42,748,190,22,"死亡 / 放弃 / 退出保留",13,mint);Label(42,773,190,21,"禁止穿戴装备",13,muted);
  }
  void DrawCampSafe(){var r=new Rect(528,530,220,270);Frame(r,inset,edge);Label(r.x+18,r.y+13,180,26,"安全箱  "+G.Safe.Columns+"×"+G.Safe.Rows,21,white);DrawGrid(G.Safe,ItemArea.Safe,new Rect(r.x+35,r.y+56,150,150),G.Safe.Columns,G.Safe.Rows,0);Label(r.x+16,r.y+220,200,20,"死亡、放弃、退出均保留",13,mint);Label(r.x+16,r.y+243,200,20,"禁止放入穿戴装备",13,muted);}
  void DrawUnknown(Rect r,ItemStack item){Frame(r,surface,edge);Label(r.x,r.y,r.width,r.height,"?",21,muted,TextAnchor.MiddleCenter);
   if(G.OpenLoot!=null&&G.OpenLoot.IdentifyingId==item.Id){float progress=Mathf.Clamp01(G.OpenLoot.IdentifyProgress/RaidGame.IdentificationSeconds(item.Kind));Vector2 center=r.center;float radius=Mathf.Min(r.width,r.height)*.30f;for(int i=0;i<32;i++){float angle=(i/32f*360-90)*Mathf.Deg2Rad;var dot=center+new Vector2(Mathf.Cos(angle),Mathf.Sin(angle))*radius;Fill(new Rect(dot.x-1.5f,dot.y-1.5f,3,3),i/32f<progress?gold:edge);} }
  }
  void DrawLootPanel(LootContainer container,bool searching){if(container.Corpse325!=null){DrawCorpsePanel325(container,searching);return;}float x=G.Phase==GamePhase.Raid?796:556;Frame(new Rect(x,98,374,624),inset,edge);Label(x+18,112,333,30,container.Name,23,white);
   var inv=G.EnsureLootGrid(container);int unknown=inv.Slots.FindAll(p=>!p.Identified).Count;
   Label(x+18,154,330,23,unknown>0?"正在逐件鉴定 · 剩余 "+unknown+" 件":"鉴定完成",15,unknown>0?gold:mint);
   ScrollGrid(inv,ItemArea.Loot,new Rect(x+24,192,300,450),ref lootScroll);
   Label(x+24,664,330,22,"已鉴定物品可双击或拖动拿取",14,muted);
   Label(x+24,691,330,20,unknown>0?"退出后，未完成的鉴定重新计时":G.TrainingActive?"练习物资不能带回营地":"物品已确认",13,gold);
  }
  void DrawDeparturePanel(){Frame(new Rect(556,98,1016,550),inset,edge);Label(583,123,720,40,"选择行动地图",27,white);if(PanelButton(new Rect(582,195,450,64),"货运码头 · 正式行动",G.NextMap==RaidDestination.Harbor))G.NextMap=RaidDestination.Harbor;if(PanelButton(new Rect(1050,195,490,64),"训练营 · 室内外试战",G.NextMap==RaidDestination.Training))G.NextMap=RaidDestination.Training;
   if(PanelButton(new Rect(582,281,450,49),"普通掉落",!G.HardMode))G.HardMode=false;if(PanelButton(new Rect(1050,281,490,49),"困难掉落 · 高品质概率×1.5",G.HardMode))G.HardMode=true;
   Label(584,349,940,30,G.NextMap==RaidDestination.Training?"训练结束恢复原装备，训练物资不能带出。":"正式行动：死亡仅保留安全箱；撤离带回物资和装备剩余护甲值。",18,white);Label(584,396,940,28,"本版难度选项只影响掉落，怪物数值相同；普通容器不出红。",17,muted);Label(584,439,940,29,"每箱1～6件 · 绿色AK伤害18 / 穿甲10 · 蓝枪可购买改装 · 蓝色标准防具120",18,gold);if(PanelButton(new Rect(584,510,956,61),"确认出发",true))G.StartRaid();}

 }
}
