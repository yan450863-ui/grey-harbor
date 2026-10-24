using UnityEngine;
namespace GreyHarbor { public sealed partial class RaidUI {
 const float PackGap338=8;
 static float PackTile338(RaidInventory inv,Rect viewport)=>Mathf.Min(Tile,(viewport.width-PackGap338)/inv.Columns,(viewport.height-PackGap338)/inv.Rows);
 static Rect PackRegion338(RectInt region,float tile)=>new Rect(region.x*tile+(region.x>0?PackGap338:0),region.y*tile+(region.y>0?PackGap338:0),region.width*tile,region.height*tile);
 internal static Rect PackCell338(RaidInventory inv,Rect viewport,int cell,int w=1,int h=1){float tile=PackTile338(inv,viewport);int x=cell%inv.Columns,y=cell/inv.Columns;foreach(var part in inv.Compartments338){if(!part.Contains(new Vector2Int(x,y)))continue;var r=PackRegion338(part,tile);return new Rect(viewport.x+r.x+(x-part.x)*tile,viewport.y+r.y+(y-part.y)*tile,w*tile,h*tile);}return default;}
 void DrawPartitionGrid338(RaidInventory inv,ItemArea area,Rect viewport){
  gridTargets.Add(new GridTarget{Area=area,Viewport=viewport});
  GUI.BeginGroup(viewport);var local=new Rect(0,0,viewport.width,viewport.height);
  for(int cell=0;cell<inv.SlotCapacity;cell++){var r=PackCell338(inv,local,cell);Frame(r,surface,edge*.7f);hits.Add(new SlotHit{Area=area,Cell=cell,Rect=new Rect(viewport.x+r.x,viewport.y+r.y,r.width,r.height)});}
  foreach(var item in inv.Slots){var r=PackCell338(inv,local,item.Cell,inv.W(item),inv.H(item));r=new Rect(r.x+2,r.y+2,r.width-4,r.height-4);if(CorpseLoot325.IsArea(area)&&!item.Identified)DrawUnknown(r,item);else DrawItem(r,item,area,item.Cell);}
  foreach(var part in inv.Compartments338)Outline(PackRegion338(part,PackTile338(inv,viewport)),edge,2);
  GUI.EndGroup();
 }
 bool DropPartition338(Vector2 p,GridTarget g,RaidInventory inv,out int cell,out Rect frame,out bool valid){
  cell=-1;frame=default;valid=false;float tile=PackTile338(inv,g.Viewport);var local=p-g.Viewport.position;
  foreach(var part in inv.Compartments338){var r=PackRegion338(part,tile);if(!r.Contains(local))continue;
   int w=RaidInventory.Width(dragItem),h=RaidInventory.Height(dragItem);
   int x=part.x+Mathf.FloorToInt((local.x-r.x)/tile-w*.5f+.5f),y=part.y+Mathf.FloorToInt((local.y-r.y)/tile-h*.5f+.5f);
   frame=Intersection(new Rect(g.Viewport.x+r.x+(x-part.x)*tile,g.Viewport.y+r.y+(y-part.y)*tile,w*tile,h*tile),g.Viewport);
   if(x<part.x||y<part.y||x+w>part.xMax||y+h>part.yMax)return true;
   cell=y*inv.Columns+x;valid=G.CanMoveItem(dragArea,dragCell,g.Area,cell,dragItem.Rotated);return true;
  }return true;
 }
}}
namespace GreyHarbor { public sealed partial class RaidUI {
 void TrainingPacks338(){Label(822,674,700,24,"背包",18,white);var kinds=new[]{ItemKind.SmallPack,ItemKind.StandardPack,ItemKind.HikingPack338,ItemKind.ToolPack338,ItemKind.TacticalPack338,ItemKind.DutyPack338};for(int i=0;i<kinds.Length;i++){var k=kinds[i];if(PanelButton(new Rect(822+i%3*240,702+i/3*38,230,32),RaidInventory.GradeName(k)+" "+RaidInventory.Name(k),G.PackGear?.Kind==k)){ClearItemUI();G.SelectTrainingGear301(ItemArea.PackEquipment,k);}}}
}}
