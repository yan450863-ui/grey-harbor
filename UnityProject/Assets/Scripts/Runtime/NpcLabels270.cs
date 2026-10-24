using UnityEngine;
namespace GreyHarbor {public sealed partial class RaidUI {
 void NpcLabels270(){if(G.Phase!=GamePhase.Raid||G.InventoryOpen)return;foreach(var e in G.Enemies){if(e.Dead||!e.VisibleToPlayer||(!e.IsRebel&&!e.IsMonster)||Vector3.Distance(G.Player.transform.position,e.transform.position)>12)continue;var p=ScreenPoint(e.transform.position+Vector3.up*2);if(p.x<120||p.x>1480||p.y<170||p.y>640)continue;Fill(new Rect(p.x-114,p.y-22,228,24),panel);Label(p.x-114,p.y-22,228,24,e.NpcLevel+"级 · 生命 "+Mathf.CeilToInt(e.Health)+" · 护甲 "+Mathf.CeilToInt(e.NpcArmorPoints),14,e.NpcLevel==2?PixelArt.Hex("719CCB"):mint,TextAnchor.MiddleCenter);}}
}}
