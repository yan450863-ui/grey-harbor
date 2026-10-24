using UnityEngine;
namespace GreyHarbor {
 public sealed partial class RaidUI {
  void CampHUD(){
   Fill(new Rect(28,25,495,78),panel);Label(48,36,450,30,"灰港营地  /  安全区域",24,mint);Label(48,75,450,22,"WASD移动 · Q/E视角 · Tab背包 · Esc暂停",15,white);
   Label(1060,32,480,25,$"V3.0.47  ·  工作台 {G.Save.CampLevel}/2  ·  撤离 {G.Save.SuccessfulRaids} 次",17,muted,TextAnchor.MiddleRight);
   if(!G.InventoryOpen){if(PanelButton(new Rect(1250,85,300,45),"打开营地主菜单"))G.OpenCampPage315(CampPage315.Home);CampTag(RaidMap.Door,"出发门",G.GateTime>0?$"准备中 {G.GateTime:0.0} / 3秒":"进入门区停留3秒");CampTag(RaidMap.Warehouse,"仓库","靠近后 F 打开");CampTag(RaidMap.Workbench,"工作台","靠近后 F 制作 / 升级");CampTag(RaidMap.Market,"交易行","靠近后 F 金币购买");
    if(G.Nearby!=null)Label(525,710,550,32,"F 拾取脚边物资",20,gold,TextAnchor.MiddleCenter);
    if(G.NoticeTime>0){Fill(new Rect(300,814,1000,50),panel);Label(316,824,968,30,G.Notice,18,gold,TextAnchor.MiddleCenter);}
    if(G.CampLocked)Label(290,160,1100,36,"存档不可用，物品操作与出发已禁止。",24,red,TextAnchor.MiddleCenter);
   }
  }
  void CampTag(Vector3 world,string title,string sub){Vector2 p=ScreenPoint(world+Vector3.up*.8f);p.y-=70;Fill(new Rect(p.x-122,p.y-36,244,67),panel);Label(p.x-117,p.y-30,234,28,title,22,mint,TextAnchor.MiddleCenter);Label(p.x-117,p.y+1,234,25,sub,15,white,TextAnchor.MiddleCenter);}
  string ShortName(ItemKind k)=>AmmoRules270.IsAmmo(k)?RaidInventory.GradeName(k)+(WeaponRules279.AmmoCaliber(k)==AmmoCaliber279.NineMM?"9mm弹":WeaponRules279.AmmoCaliber(k)==AmmoCaliber279.Five56?"5.56弹":WeaponRules279.AmmoCaliber(k)==AmmoCaliber279.Gauge12?"12号弹":"7.62弹"):RaidInventory.Name(k);
  void ItemIcon(Rect r,ItemKind k,bool rotated=false){
   var texture=rotated?ItemRotation314.Get(k):ItemIcons.Get(k);var uv=rotated?new Rect(0,0,1,1):ItemIcons.Bounds(k);float aspect=texture.width*uv.width/(texture.height*uv.height);
   // Rotate cropped pixels once. GUI group/scroll coordinates are never transformed.
   GUI.DrawTextureWithTexCoords(ItemRotation314.Fit(r,aspect),texture,uv,true);
   if(AmmoRules270.IsAmmo(k)&&r.width>=28&&r.height>=25){string caliber320=AmmoBadgeText320(k);int badgeFont320=Mathf.Clamp(Mathf.RoundToInt(r.width*.15f),10,15);float badgeWidth320=Mathf.Min(r.width,caliber320.Length*(badgeFont320*.65f)+6),badgeHeight320=badgeFont320+4;Label(r.x+2,r.yMax-badgeHeight320+1,badgeWidth320-4,badgeHeight320,caliber320,badgeFont320,white,TextAnchor.MiddleLeft);}
  }
  internal static int FootprintMarkerCount2758(ItemKind kind)=>WeaponRules279.IsWeapon(kind)?0:RaidInventory.Width(kind)*RaidInventory.Height(kind);
  internal static Rect[] FootprintCells2759(Rect r,int columns,int rows){
   const float size=6,gap=2;float width=columns*(size+gap)-gap,height=rows*(size+gap)-gap;
   float x=r.xMax-width-5,y=r.yMax-height-5;var cells=new Rect[columns*rows];
   for(int i=0;i<cells.Length;i++)cells[i]=new Rect(x+i%columns*(size+gap),y+i/columns*(size+gap),size,size);return cells;
  }
  void ItemFootprint2758(Rect r,ItemKind kind)=>ItemFootprint2759(r,kind,RaidInventory.Width(kind),RaidInventory.Height(kind));
  void ItemFootprint2758(Rect r,ItemStack item)=>ItemFootprint2759(r,item.Kind,RaidInventory.Width(item),RaidInventory.Height(item));
  void ItemFootprint2759(Rect r,ItemKind kind,int columns,int rows){
   if(WeaponRules279.IsWeapon(kind))return;var cells=FootprintCells2759(r,columns,rows);
   var first=cells[0];var last=cells[cells.Length-1];Fill(Rect.MinMaxRect(first.x-2,first.y-2,last.xMax+2,last.yMax+2),new Color(.035f,.045f,.043f,.85f));
   foreach(var cell in cells)Outline(cell,new Color(.88f,.91f,.85f),1);
  }
  Color ItemPaper(ItemKind k)=>new[]{new Color(.30f,.40f,.32f),new Color(.28f,.37f,.48f),new Color(.40f,.32f,.48f),new Color(.49f,.43f,.28f),new Color(.45f,.23f,.23f)}[RaidInventory.Grade(k)];
  Color ItemEdge(ItemKind k)=>new[]{new Color(.45f,.64f,.48f),new Color(.40f,.60f,.80f),new Color(.65f,.49f,.79f),new Color(.78f,.66f,.35f),new Color(.88f,.38f,.35f)}[RaidInventory.Grade(k)];
  void ItemFrame(Rect r,ItemKind k,bool selected=false,bool hover=false){
   Fill(r,ItemEdge(k));Fill(new Rect(r.x+2,r.y+2,r.width-4,r.height-4),Color.Lerp(ItemPaper(k),Color.white,hover?.12f:0));
   if(selected){var c=new Color(.96f,.94f,.77f);Fill(new Rect(r.x,r.y,r.width,3),c);Fill(new Rect(r.x,r.yMax-3,r.width,3),c);Fill(new Rect(r.x,r.y,3,r.height),c);Fill(new Rect(r.xMax-3,r.y,3,r.height),c);}
  }
  int workbenchCategory328,workbenchBatch328=1,selectedRecipe328=-1;bool autoProcess328,useRarePowder328;
  void WorkbenchUI(){
   Fill(new Rect(0,0,1600,900),new Color(.015f,.025f,.024f,.86f));Fill(new Rect(140,65,1320,770),panel);Label(175,91,1080,43,$"营地工作台 · {G.Save.CampLevel}/2级",30,mint);if(Button(new Rect(1305,88,115,41),"关闭"))G.CloseInventory();
   string[] categories={"组件加工","弹药制作","其他制作"};for(int c=0;c<3;c++)if(PanelButton(new Rect(180+c*142,150,132,37),(workbenchCategory328==c?"● ":"")+categories[c])){workbenchCategory328=c;recipePage=0;selectedRecipe328=-1;}
   int[] batches={1,3,5};for(int b=0;b<3;b++)if(PanelButton(new Rect(625+b*120,150,110,37),(workbenchBatch328==batches[b]?"● ":"")+batches[b]+"批"))workbenchBatch328=batches[b];
   if(PanelButton(new Rect(180,195,295,34),(allowCertified?"[允许]":"[保护]")+" 额外消耗认证原料"))allowCertified=!allowCertified;
   if(PanelButton(new Rect(485,195,290,34),(autoProcess328?"[开启]":"[关闭]")+" 连续加工缺少的组件"))autoProcess328=!autoProcess328;
   if(PanelButton(new Rect(785,195,245,34),"精密原料："+(useRarePowder328?"金属粉末":"精密轴套")))useRarePowder328=!useRarePowder328;
   var indices=new System.Collections.Generic.List<int>();for(int i=0;i<CampRules.Recipes.Length;i++){var r=CampRules.Recipes[i];int cat=AmmoCrafting328.IsComponent(r.Output)?0:AmmoRules270.IsAmmo(r.Output)?1:2;if(cat==workbenchCategory328)indices.Add(i);}
   int pages=Mathf.Max(1,Mathf.CeilToInt(indices.Count/5f));recipePage=Mathf.Clamp(recipePage,0,pages-1);
   if(selectedRecipe328<0||!indices.Contains(selectedRecipe328))selectedRecipe328=indices.Count>0?indices[recipePage*5]:-1;
   for(int row=0;row<5;row++){int slot=recipePage*5+row;if(slot>=indices.Count)break;int index=indices[slot];var r=CampRules.Recipes[index];float y=242+row*92;Frame(new Rect(175,y,855,84),selectedRecipe328==index?new Color(.13f,.23f,.20f):inset,edge);ItemIcon(new Rect(185,y+13,48,54),r.Output);
    Label(245,y+8,595,26,$"{r.Name} → ×{r.Amount*workbenchBatch328}",19,RaidInventory.Tint(r.Output));var text=new System.Text.StringBuilder();foreach(var cost in r.Costs){if(text.Length>0)text.Append("  ");text.Append(AmmoCrafting328.Certified(r,cost.Kind)?"✓ ":"").Append(ShortName(cost.Kind)).Append(" ").Append(AmmoCrafting328.Available(G.Save,r,cost.Kind,allowCertified)).Append("/").Append(cost.Count*workbenchBatch328);}
    WrappedLabel(new Rect(245,y+37,590,40),text.ToString(),15,muted);bool ready=AmmoCrafting328.TryPlan(G.Save,r,workbenchBatch328,allowCertified,autoProcess328,useRarePowder328,out var planned,out var reason);
    if(PanelButton(new Rect(850,y+11,160,28),"查看材料"))selectedRecipe328=index;
    if(PanelButton(new Rect(850,y+44,160,29),ready?"制作 "+workbenchBatch328+"批":G.Save.CampLevel<r.Level?"需"+r.Level+"级":"查看缺少条件")){selectedRecipe328=index;if(ready)G.Craft(index,allowCertified,workbenchBatch328,autoProcess328,useRarePowder328);else G.Notify(reason);}
   }
   if(PanelButton(new Rect(180,715,850,34),$"配方 {recipePage+1}/{pages} · 点击翻页"))recipePage=(recipePage+1)%pages;
   Label(180,758,850,26,"组件加工固定使用认证收藏品；连续加工保留整批剩余组件。",16,muted);
   Label(1065,152,360,30,"工作台升级",23,gold);
   if(G.Save.CampLevel<2){var costs=CampRules.UpgradeCosts(G.Save.CampLevel);for(int i=0;i<costs.Length;i++){var c=costs[i];Label(1065,194+i*28,365,25,$"✓ {ShortName(c.Kind)} {G.Save.Certified[(int)c.Kind]}/{c.Count}",16,white);}bool qualified=NpcProduction328.CanUpgrade(G.Save,G.Save.CampLevel+1,out var reason);WrappedLabel(new Rect(1065,322,355,65),qualified?"升级资格已解锁 · "+(G.Save.CampLevel==0?"1级开放绿色制作":"2级开放精密加工与蓝色制作"):reason,17,qualified?mint:muted);if(PanelButton(new Rect(1065,394,350,43),"提交认证材料并升级"))G.UpgradeCamp();}else Label(1065,210,350,32,"当前工作台已满级",20,mint);
   Label(1065,466,350,28,"本次制作预览",21,mint);
   if(selectedRecipe328>=0){var selected=CampRules.Recipes[selectedRecipe328];Label(1065,505,350,27,ShortName(selected.Output)+" ×"+(selected.Amount*workbenchBatch328),18,RaidInventory.Tint(selected.Output));bool valid=AmmoCrafting328.TryPlan(G.Save,selected,workbenchBatch328,allowCertified,autoProcess328,useRarePowder328,out var planned,out var reason);WrappedLabel(new Rect(1065,544,350,170),valid?"将从仓库消耗："+AmmoCrafting328.Consumption(G.Save,planned):reason,17,valid?white:gold);}
   WrappedLabel(new Rect(1065,730,350,53),"材料与产物都在局外仓库；商城不出售组件。",16,muted);if(G.NoticeTime>0)Label(180,799,1230,28,G.Notice,17,gold);
  }
 }
}
