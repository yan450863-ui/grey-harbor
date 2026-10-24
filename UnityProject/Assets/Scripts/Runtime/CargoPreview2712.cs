using UnityEngine;
namespace GreyHarbor {
 // Preview changes the starting viewpoint only. Reserved labels are UI and have
 // no physical, search, combat or economy components.
 public static class CargoPreview2712 {
  public static bool Apply(RaidGame g){
   if(g==null||!g.Player||!g.Map.HarborWhitebox)return false;
   Physics.SyncTransforms();Vector3 start=RaidMap.CargoPreviewSpawn2712;
   if(!g.Map.OnHarborSurface(start)||Physics.CheckCapsule(start+Vector3.up*.36f,start+Vector3.up*1.35f,.34f,SceneObstacle.MovementMask,QueryTriggerInteraction.Ignore)){
    g.Notify("中央区试玩起点暂不可通行，已保留港口入口位置。");return false;
   }
   g.Player.transform.position=start;g.Rig.ResetView(45);g.Rig.Tick(0);
   var center=RaidMap.CargoBounds2712.center;var aim=new Vector3(center.x,0,center.y)-start;aim.y=0;g.Player.Aim=aim.normalized;g.ResetAkHandling276();
   Physics.SyncTransforms();g.Vision.Refresh(start,g.Player.Aim);g.Player.Draw(g.View,0);
   g.Notify("中央集装箱区：7只异种（5绿2蓝）；F搜索物资，M看全图，Shift切换奔跑。");return true;
  }
 }
 public sealed partial class RaidUI {
  public static bool CargoNear2712(RaidMap map,Vector3 position){
   if(map==null||!map.HarborWhitebox)return false;var r=RaidMap.CargoBounds2712;
   return position.x>=r.xMin-8&&position.x<=r.xMax+8&&position.z>=r.yMin-8&&position.z<=r.yMax+8;
  }
  public static string CargoReservationLabel2712(bool monster,int index)=>(monster?"M":"C")+(index+1).ToString("00");
  bool CargoGuide2712(){
   if(!CargoNear2712(G.Map,G.Player.transform.position))return false;
   Fill(new Rect(28,109,353,76),panel);Label(48,118,320,24,"中央集装箱区 · 货场迷宫",18,mint);
   Label(48,145,320,18,"可搜索货物与小保险箱 · 7只异种",12,white);Label(48,165,320,17,"F搜索 · M全图 · Shift切换奔跑",12,muted);
   return true;
  }
  void CargoNearbyLabels2712(){
   for(int pass=0;pass<2;pass++){
    var points=pass==0?G.Map.CargoSearchReserved2712:G.Map.CargoMonsterReserved2712;Color tint=pass==0?mint:gold;
    for(int i=0;i<points.Count;i++){
     var point=points[i];if(Vector3.Distance(G.Player.transform.position,point)>9||!G.Vision.CanSee(point))continue;
     var viewport=G.View.WorldToViewportPoint(point+Vector3.up*.45f);if(viewport.z<=0||viewport.x<.08f||viewport.x>.92f||viewport.y<.15f||viewport.y>.86f)continue;
     var p=ScreenPoint(point+Vector3.up*.45f);Fill(new Rect(p.x-48,p.y-10,96,20),panel);
     Label(p.x-48,p.y-10,96,20,CargoReservationLabel2712(pass==1,i)+(pass==0?" 搜索预留":" 怪物预留"),12,tint,TextAnchor.MiddleCenter);
    }
   }
  }
  void CargoOverviewMarkers2712(Rect frame){
   if(!CargoNear2712(G.Map,G.Player.transform.position))return;
   for(int pass=0;pass<2;pass++){
    var points=pass==0?G.Map.CargoSearchReserved2712:G.Map.CargoMonsterReserved2712;Color tint=pass==0?mint:gold;
    for(int i=0;i<points.Count;i++){var p=WhitePoint(points[i],frame);Fill(new Rect(p.x-2,p.y-2,4,4),tint);Label(p.x+3,p.y-8,30,16,CargoReservationLabel2712(pass==1,i),10,tint);}
   }
   Label(375,123,840,19,"中央货场：集装箱外观、港口杂物与搜索容器，暂未投放敌人",12,muted,TextAnchor.MiddleCenter);
  }
 }
}


