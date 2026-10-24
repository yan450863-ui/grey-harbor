using UnityEngine;
namespace GreyHarbor {public sealed partial class RaidUI {
 internal static string EnemyName344(RaidActor enemy)=>enemy.IsRebel?RebelTuning145.Name(enemy.Rebel).Replace("二级","").Replace("绿色","").Replace("蓝色",""):enemy.IsMonster?HarborContent.MonsterNames[(int)enemy.Monster]:"叛军士兵";
 internal bool EnemyStatusVisible344(RaidActor enemy)=>enemy&&enemy.gameObject.activeInHierarchy&&!enemy.Dead&&enemy.VisibleToPlayer&&G.Vision!=null&&G.Vision.CanSee(enemy.transform.position);
 internal Vector2 EnemyHead344(RaidActor enemy){var body=enemy.Body;if(body&&body.sprite){var b=body.sprite.bounds;return ScreenPoint(body.transform.TransformPoint(new Vector3(b.center.x,b.max.y,b.center.z)))+new Vector2(0,-10);}return ScreenPoint(enemy.transform.position+Vector3.up*1.8f);}
 void EnemyStatus344(){
  foreach(var enemy in G.Enemies){
   if(!EnemyStatusVisible344(enemy))continue;var p=EnemyHead344(enemy);if(p.x<30||p.x>1570||p.y<35||p.y>890)continue;
   bool attacking=enemy.Windup>0||enemy.Lunge>0||enemy.AttackPose>.1f;
   string state=attacking?"攻击":enemy.Alert>.2f?"警戒":"巡逻";Color accent=attacking?red:enemy.Alert>.2f?gold:mint;
   const float width=156;var r=new Rect(p.x-width/2,p.y-36,width,30);
   GUI.DrawTexture(r,Texture2D.whiteTexture,ScaleMode.StretchToFill,true,0,new Color(.035f,.06f,.06f,.78f),0,5);
   Label(r.x+5,r.y+2,114,18,EnemyName344(enemy)+" · "+Mathf.Max(1,enemy.NpcLevel),12,white);
   Label(r.xMax-35,r.y+3,30,18,state,11,accent,TextAnchor.MiddleRight);
   float health=HudFill329(enemy.Health,enemy.NpcMaxHealth),armor=HudFill329(enemy.NpcArmorPoints,enemy.NpcMaxArmor);
   var healthRect=new Rect(r.x+6,r.y+22,width-12,3);Fill(healthRect,new Color(.20f,.25f,.23f,.8f));Fill(new Rect(healthRect.x,healthRect.y,healthRect.width*health,3),PixelArt.Hex("97D9BC"));
   if(enemy.NpcMaxArmor>0){var armorRect=new Rect(r.x+6,r.y+27,width-12,2);Fill(armorRect,new Color(.18f,.23f,.27f,.8f));Fill(new Rect(armorRect.x,armorRect.y,armorRect.width*armor,2),PixelArt.Hex("719CCB"));}
  }
 }
}}
