using UnityEngine;
namespace GreyHarbor {
 public static class FreightPreview2716 {
  public static bool Apply(RaidGame g){
   if(g==null||!g.Player||!g.Map.HarborWhitebox)return false;
   var p=RaidMap.FreightPreviewSpawn2716;Physics.SyncTransforms();
   if(!g.Map.HarborSurfaceClear(p)||Physics.CheckCapsule(p+Vector3.up*.36f,p+Vector3.up*1.35f,.34f,SceneObstacle.MovementMask,QueryTriggerInteraction.Ignore))return false;
   g.Player.transform.position=p;g.Player.Aim=Vector3.right;g.Rig.ResetView(45);g.Rig.Tick(0);g.ResetAkHandling276();g.Vision.Refresh(p,g.Player.Aim);g.Player.Draw(g.View,0);
   g.NoticeTime=0;return true;
  }
 }
 public sealed partial class RaidUI {
  bool FreightOutdoorGuide2716(){
   if(!G.Map.HarborWhitebox)return false;var p=G.Player.transform.position;
   if(!new Rect(-74,64,45,40).Contains(new Vector2(p.x,p.z)))return false;
   Fill(new Rect(28,109,353,76),panel);Label(48,118,320,24,"西北货运后场",18,mint);
   Label(48,145,320,18,"货车作业场 · 西侧铁门撤离",12,white);Label(48,165,320,17,"F搜索 · 长按F进屋 · M全图",12,muted);return true;
  }
 }
}

