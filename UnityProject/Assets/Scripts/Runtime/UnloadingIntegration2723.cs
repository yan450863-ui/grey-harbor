using UnityEngine;
namespace GreyHarbor {
 public static class UnloadingPreview2723 {
  public static bool Apply(RaidGame g){
   if(!g||g.IsLoading2721||!g.Map.HarborWhitebox)return false;
   var p=RaidMap.UnloadingPreviewSpawn2723;
   if(!g.Map.HarborSurfaceClear(p,.34f)||!g.Navigation.ClearSegment(p,p))return false;
   g.Player.transform.position=p;Physics.SyncTransforms();g.Rig.ResetView(45);g.Rig.Tick(0);g.Vision.Refresh(p,g.Player.Aim);
   g.NoticeTime=0;return true;
  }
 }
 public sealed partial class RaidUI {
  bool UnloadingGuide2723(){
   if(!G.Map.UnloadingInterior2723&&(!G.Map.HarborWhitebox||!RaidMap.UnloadingBounds2723.Contains(new Vector2(G.Player.transform.position.x,G.Player.transform.position.z))))return false;
   Fill(new Rect(28,109,353,90),panel);
   Label(48,118,320,24,G.Map.UnloadingInterior2723?(G.Map.InteriorId=="unloadingwarehouse"?"装卸仓库 · 单层大厅":"装卸值班间 · 办公与杂物间"):"装卸区 · 仓储与周转作业",18,mint);
   Label(48,145,320,20,G.Map.UnloadingInterior2723?"F开关房门 · 长按F两秒返回室外":"仓库／值班间：长按F进入 · M全图",13,muted);
   Label(48,169,320,20,"室内搜索 · 注意保持通行",13,muted);return true;
  }
 }
}
