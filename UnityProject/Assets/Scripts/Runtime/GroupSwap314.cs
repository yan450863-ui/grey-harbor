using System;using System.Collections.Generic;using UnityEngine;
namespace GreyHarbor {public sealed partial class RaidGame {
 sealed class GroupSwapPlan314 {public RaidInventory Origin,Target;public List<ItemStack> OriginItems,TargetItems;}
 bool HasSwapTargets314(ItemArea from,ItemStack source,ItemArea to,int cell,ItemStack shape){if(IsGearArea(from)||IsGearArea(to)||cell<0)return false;var target=InventoryFor(to);return target!=null&&target.Slots.Exists(p=>p.Id!=source.Id&&Overlaps(target,shape,cell,p,p.Cell));}
 bool PlanSwap314(ItemArea from,ItemStack source,ItemArea to,int cell,ItemStack shape,out GroupSwapPlan314 plan){
  plan=null;var origin=InventoryFor(from);var target=InventoryFor(to);if(origin==null||target==null||cell<0||!KeyGridAllowed314(to,shape)||to==ItemArea.Safe&&RaidInventory.IsWearable(shape.Kind)||shape.Count>target.StackLimit(shape.Kind))return false;
  var displaced=target.Slots.FindAll(p=>p.Id!=source.Id&&Overlaps(target,shape,cell,p,p.Cell));if(displaced.Count==0||displaced.Exists(p=>!p.Identified||!KeyGridAllowed314(from,p)||from==ItemArea.Safe&&RaidInventory.IsWearable(p.Kind)))return false;
  bool same=from==to;var outTarget=target.Clone();outTarget.Slots.RemoveAll(p=>displaced.Exists(d=>d.Id==p.Id)||same&&p.Id==source.Id);var incoming=RaidInventory.Copy(shape);incoming.Cell=cell;if(!outTarget.Fits(incoming,cell))return false;outTarget.Slots.Add(incoming);
  var outOrigin=same?outTarget:origin.Clone();if(!same)outOrigin.Slots.RemoveAll(p=>p.Id==source.Id);
  var returnItems=new List<ItemStack>();foreach(var item in displaced){int left=item.Count;bool first=true;while(left>0){var copy=RaidInventory.Copy(item);copy.Count=Math.Min(left,origin.StackLimit(item.Kind));if(!first)copy.Id=Guid.NewGuid().ToString("N");copy.Cell=-1;returnItems.Add(copy);left-=copy.Count;first=false;}}
  returnItems.Sort((a,b)=>(origin.W(b)*origin.H(b)).CompareTo(origin.W(a)*origin.H(a)));
  int sx=source.Cell%origin.Columns,sy=source.Cell/origin.Columns,sw=origin.W(source),sh=origin.H(source),attempts=0;
  if(!PackSwap314(outOrigin,returnItems,0,sx,sy,sw,sh,ref attempts))return false;
  plan=new GroupSwapPlan314{Origin=origin,Target=target,OriginItems=outOrigin.Snapshot(),TargetItems=outTarget.Snapshot()};return true;
 }
 bool PackSwap314(RaidInventory grid,List<ItemStack> items,int index,int x,int y,int w,int h,ref int attempts){if(index==items.Count)return true;if(++attempts>10000)return false;var item=items[index];bool originalRotation=item.Rotated;for(int orientation=0;orientation<(grid.W(item)==grid.H(item)?1:2);orientation++){item.Rotated=orientation==0?originalRotation:!originalRotation;for(int row=y;row<y+h;row++)for(int col=x;col<x+w;col++){if(col+grid.W(item)>x+w||row+grid.H(item)>y+h)continue;int cell=row*grid.Columns+col;if(!grid.Fits(item,cell))continue;var placed=RaidInventory.Copy(item);placed.Cell=cell;grid.Slots.Add(placed);if(PackSwap314(grid,items,index+1,x,y,w,h,ref attempts))return true;grid.Slots.Remove(placed);}}item.Rotated=originalRotation;return false;}
 bool CommitSwap314(ItemArea from,ItemStack source,ItemArea to,int cell,ItemStack shape){if(!PlanSwap314(from,source,to,cell,shape,out var plan))return false;plan.Origin.Slots.Clear();plan.Origin.Slots.AddRange(plan.OriginItems);if(from!=to){plan.Target.Slots.Clear();plan.Target.Slots.AddRange(plan.TargetItems);}return true;}
}}
