using UnityEngine;
namespace GreyHarbor {public sealed partial class RaidUI {
 void RepairAnnexGuide200(){
  string name="维修准备大厅";var p=new Vector2(G.Player.transform.position.x,G.Player.transform.position.z);
  foreach(var room in G.Map.RepairRooms200)if(room.Area.Contains(p)){name=room.Name;break;}
  Fill(new Rect(28,109,353,76),panel);Label(48,118,320,24,name,18,mint);Label(48,145,320,20,"F开关房门 · 长按F两秒返回室外",13,muted);
 }
}}
