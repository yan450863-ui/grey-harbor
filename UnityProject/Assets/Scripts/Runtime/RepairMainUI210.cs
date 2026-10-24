using UnityEngine;
namespace GreyHarbor {public sealed partial class RaidUI {
 void RepairMainGuide210(){
  string name=G.Map.RepairFloor220+"层 / "+(G.Map.RepairFloor220==1?"大型检修大厅":G.Map.RepairFloor220==2?"零件维修与检测":G.Map.RepairFloor220==3?"电气与技术层":"管理与生活层");var p=new Vector2(G.Player.transform.position.x,G.Player.transform.position.z);
  foreach(var room in G.Map.RepairMainRooms210)if(room.Area.Contains(p)){name=room.Name;break;}
  Fill(new Rect(28,109,353,76),panel);Label(48,118,320,24,name,18,mint);Label(48,145,320,20,"F开关房门 · 上下楼看方向标签，长按F两秒",13,muted);RepairStairLabels223();
 }
 void RepairStairLabels223(){
  if(G.InventoryOpen||G.Paused)return;
  foreach(var d in G.Map.Doors){
   if(!d.Portal||!d.Destination.StartsWith("repair-floor:"))continue;
   if(!G.Vision.RoomAllowed(d.Position)||Vector3.Distance(G.Player.transform.position,d.Position)>8)continue;
   var parts=d.Destination.Split(':');int target;if(parts.Length!=3||!int.TryParse(parts[1],out target))continue;
   bool up=target>G.Map.RepairFloor220,selected=G.NearbyDoor==d;var c=up?gold:PixelArt.Hex("79C9E0");
   var screen=G.View.WorldToScreenPoint(d.Position+Vector3.up*.35f);if(screen.z<=0)continue;
   float x=(screen.x-(Screen.width-1600*scale)*.5f)/scale,y=(Screen.height-screen.y-(Screen.height-900*scale)*.5f)/scale;
   var r=new Rect(x-88,y-45,176,selected?58:34);Fill(r,panel);Fill(new Rect(r.x,r.y,4,r.height),c);
   Label(r.x+4,r.y,r.width-4,32,(up?"上楼 ↑":"下楼 ↓")+" → "+target+"层",16,c,TextAnchor.MiddleCenter);
   if(selected)Label(r.x+4,r.y+30,r.width-4,25,"长按 F 两秒",13,white,TextAnchor.MiddleCenter);
  }
 }
}}
