using UnityEngine;
namespace GreyHarbor {public sealed partial class RaidUI {
 void OfficeStairLabels2733(){
  if(G.InventoryOpen||G.Paused)return;
  foreach(var d in G.Map.Doors){
   if(!d.Portal||!d.Destination.StartsWith("office-floor:")||!G.Vision.RoomAllowed(d.Position)||Vector3.Distance(G.Player.transform.position,d.Position)>8)continue;
   int target=int.Parse(d.Destination.Split(':')[1]);bool up=target>G.Map.OfficeFloor2733,selected=G.NearbyDoor==d;var c=up?gold:PixelArt.Hex("79C9E0");
   var s=G.View.WorldToScreenPoint(d.Position+Vector3.up*.35f);if(s.z<=0)continue;
   float x=(s.x-(Screen.width-1600*scale)*.5f)/scale,y=(Screen.height-s.y-(Screen.height-900*scale)*.5f)/scale;
   var r=new Rect(x-88,y-45,176,selected?58:34);Fill(r,panel);Fill(new Rect(r.x,r.y,4,r.height),c);
   Label(r.x+4,r.y,r.width-4,32,(up?"上楼 ↑ → ":"下楼 ↓ → ")+target+"层",16,c,TextAnchor.MiddleCenter);
   if(selected)Label(r.x+4,r.y+30,r.width-4,25,"长按 F 两秒",13,white,TextAnchor.MiddleCenter);
  }
 }
}}
