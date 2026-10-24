using UnityEngine;
namespace GreyHarbor {public sealed partial class RaidUI {
 bool undergroundOverview2752;
 void UndergroundGuide2752(){
  Fill(new Rect(28,109,353,95),panel);Label(48,118,320,24,"地下管网 · 检修作业区",18,mint);
  Label(48,145,320,20,"长按F两秒上地面 · M结构图",13,muted);
  Label(48,170,320,20,"24处检修物资 · 三种异种出没",13,muted);
 }
 void UndergroundOverview2752(){
  if(Event.current.type==EventType.KeyDown&&Event.current.keyCode==KeyCode.M){undergroundOverview2752=!undergroundOverview2752;Event.current.Use();}
  if(!undergroundOverview2752)return;
  Fill(new Rect(350,80,900,740),panel);Label(375,94,840,30,"地下管网结构 · M关闭",22,white);
  var bounds=G.Map.Bounds;float scale=Mathf.Min(720/bounds.width,600/bounds.height);var frame=new Rect(800-bounds.width*scale/2,165,bounds.width*scale,bounds.height*scale);
  GUI.DrawTexture(frame,G.Map.UndergroundPlanTexture2752());
  foreach(var e in G.Map.UndergroundEntries2752){var p=WhitePoint(e.ExitPosition,frame);Fill(new Rect(p.x-5,p.y-5,10,10),mint);Label(p.x-70,p.y-25,140,22,e.Name,12,white,TextAnchor.MiddleCenter);}
  var gate=WhitePoint(G.Map.UndergroundGate2752.Position,frame);Fill(new Rect(gate.x-5,gate.y-4,10,8),gold);
  var player=WhitePoint(G.Player.transform.position,frame);Fill(new Rect(player.x-5,player.y-5,10,10),red);
  Label(375,787,840,22,"地下检查图 · 红：你  绿：地面连接  黄：南部入口门    北 ↑",14,white,TextAnchor.MiddleCenter);
 }
}}
