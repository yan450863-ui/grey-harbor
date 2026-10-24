using UnityEngine;
namespace GreyHarbor {
 // Explicit level-one health and armor; level two scales both pools by 1.25.
 // Attack damage remains independent of player gun/ammunition tuning.
 public static class MonsterTuning {
  public static bool IsNew(MonsterKind k)=>k==MonsterKind.PaperbackCreeper||k==MonsterKind.BallastHauler||k==MonsterKind.Pipewhisker;
  public static string AssetId(MonsterKind k)=>IsNew(k)?k.ToString():null;
  public static float LungeDuration(MonsterKind k)=>k==MonsterKind.Runner?.48f:k==MonsterKind.PaperbackCreeper?.4f:0;
  public static float LungeSpeed(MonsterKind k)=>k==MonsterKind.PaperbackCreeper?9f:6;
  public static float HitReach(MonsterKind k)=>k==MonsterKind.BallastHauler?2.05f:k==MonsterKind.Pipewhisker?1.6f:k==MonsterKind.Shell?1.85f:k==MonsterKind.Runner||k==MonsterKind.PaperbackCreeper?1.2f:1.35f;
  public static float Health(MonsterKind k)=>k==MonsterKind.PaperbackCreeper?108:k==MonsterKind.BallastHauler?144:k==MonsterKind.Pipewhisker?90:k==MonsterKind.Shell?198:k==MonsterKind.Runner?54:k==MonsterKind.Spitter?72:90;
  public static float Armor(MonsterKind k)=>k==MonsterKind.BallastHauler?50:k==MonsterKind.Shell?70:20;
  public static float Damage(MonsterKind k)=>k==MonsterKind.PaperbackCreeper?28:k==MonsterKind.BallastHauler?38:k==MonsterKind.Pipewhisker?24:k==MonsterKind.Shell?42:k==MonsterKind.Runner?26:k==MonsterKind.Spitter?24:22;
  public static float Windup(MonsterKind k)=>k==MonsterKind.PaperbackCreeper?.28f:k==MonsterKind.BallastHauler?.3f:k==MonsterKind.Pipewhisker?.18f:k==MonsterKind.Shell?1.1f:k==MonsterKind.Runner?.65f:k==MonsterKind.Spitter?.8f:.55f;
  public static float Cooldown(MonsterKind k)=>k==MonsterKind.PaperbackCreeper?1.1f:k==MonsterKind.BallastHauler?1.35f:k==MonsterKind.Pipewhisker?.9f:k==MonsterKind.Spitter?3:k==MonsterKind.Shell?2.4f:1.7f;
  public static float PatrolSpeed(MonsterKind k)=>k==MonsterKind.PaperbackCreeper?1.5f:k==MonsterKind.BallastHauler?1.1f:k==MonsterKind.Pipewhisker?1.4f:k==MonsterKind.Runner?1.3f:.8f;
  public static float Speed(MonsterKind k)=>k==MonsterKind.PaperbackCreeper?4.2f:k==MonsterKind.BallastHauler?3.7f:k==MonsterKind.Pipewhisker?4.1f:k==MonsterKind.Runner?3.15f:k==MonsterKind.Shell?1.25f:k==MonsterKind.Spitter?1.15f:1.9f;
  public static float Reach(MonsterKind k)=>k==MonsterKind.PaperbackCreeper?2.4f:k==MonsterKind.BallastHauler?1f:k==MonsterKind.Pipewhisker?.95f:k==MonsterKind.Spitter?8:k==MonsterKind.Runner?4:k==MonsterKind.Shell?1.7f:1.2f;
  public static float ArtHeight(MonsterKind k)=>IsNew(k)?AssetMonsterArt2717.ArtHeight(AssetId(k)):k==MonsterKind.Shell?1.8f:k==MonsterKind.Runner?.93f:k==MonsterKind.Spitter?1.2f:1.55f;
 }
}
