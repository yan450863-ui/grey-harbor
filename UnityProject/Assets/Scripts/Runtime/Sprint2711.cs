using UnityEngine;
namespace GreyHarbor {
 public sealed class SprintState2711 {
  public const float Maximum=10,RecoveryDelay=4f,ResumeAt=3,DrainPerSecond=1,RecoveryPerSecond=1f,SpeedMultiplier=1.5f;
  public float Stamina {get;private set;}=Maximum;
  public bool Intent {get;private set;}
  public bool Exhausted {get;private set;}
  public bool Running {get;private set;}
  public bool ReadyToRun=>!Exhausted&&Stamina>0&&(Running||Stamina>=ResumeAt-.000001f);
  public bool CanRun=>Intent&&ReadyToRun;
  float nonRunningSeconds=RecoveryDelay;
  public float RecoveryDelayRemaining=>Mathf.Max(0,RecoveryDelay-nonRunningSeconds);
  public float LastRunningSeconds {get;private set;}
  public void Reset(){Stamina=Maximum;Intent=Exhausted=Running=false;LastRunningSeconds=0;nonRunningSeconds=RecoveryDelay;}
  public void Toggle(){Intent=!Intent;if(!Intent)Running=false;}
  public void Stop(){Running=false;LastRunningSeconds=0;}
  public void Tick(float dt,bool wasRunning,Vector3 actualDelta,float plannedDistance,float drainScale=1,float recoveryScale=1){
   dt=Mathf.Max(0,dt);actualDelta.y=0;
   bool moved=plannedDistance>.000001f&&actualDelta.sqrMagnitude>.00000001f;
   // Actual displacement distinguishes movement from a fully blocked frame.
   // Sliding along a wall is still running for the whole segment, at1 point/s.
   LastRunningSeconds=wasRunning&&moved?dt:0;Running=LastRunningSeconds>.000001f;
   if(Running){nonRunningSeconds=0;Stamina=Mathf.Max(0,Stamina-LastRunningSeconds*DrainPerSecond*drainScale);}
   else {float waiting=Mathf.Min(dt,RecoveryDelayRemaining);nonRunningSeconds=Mathf.Min(RecoveryDelay,nonRunningSeconds+dt);Stamina=Mathf.Min(Maximum,Stamina+(dt-waiting)*RecoveryPerSecond*recoveryScale);}
   if(Stamina<=.000001f){Stamina=0;Exhausted=true;Running=false;}
   if(Exhausted&&Stamina>=ResumeAt-.000001f){Exhausted=false;}
  }
 }
 public sealed partial class RaidGame {
  public readonly SprintState2711 Sprint2711=new SprintState2711();
  public float LastRunningSeconds321 {get;private set;}
  public void ResetSprint2711(){Sprint2711.Reset();LastRunningSeconds321=0;}
  public void MovePlayer2711(Vector3 walkVelocity,float dt){LastRunningSeconds321=0;
   if(Phase!=GamePhase.Raid||Paused||!Player||Player.Dead){Sprint2711.Stop();return;}
   var tier=LoadTier320;walkVelocity*=tier.Move;
   if(InventoryOpen){Sprint2711.Tick(dt,false,Vector3.zero,0,tier.Drain,tier.Recovery);return;}
   dt=Mathf.Max(0,dt);walkVelocity.y=0;var start=Player.transform.position;
   // Split the exhaustion frame, so even a long frame cannot grant extra running
   // distance beyond the stamina available at its start.
   float runTime=walkVelocity.sqrMagnitude>.000001f&&Sprint2711.CanRun?Mathf.Min(dt,Sprint2711.Stamina/(SprintState2711.DrainPerSecond*tier.Drain)):0;
   if(runTime>0){var from=Player.transform.position;var planned=walkVelocity*(SprintState2711.SpeedMultiplier*runTime);Player.Move(planned);Sprint2711.Tick(runTime,true,Player.transform.position-from,planned.magnitude,tier.Drain,tier.Recovery);LastRunningSeconds321=Sprint2711.LastRunningSeconds;}
   float walkTime=dt-runTime;
   if(walkTime>0){var from=Player.transform.position;var planned=walkVelocity*walkTime;Player.Move(planned);Sprint2711.Tick(walkTime,false,Player.transform.position-from,planned.magnitude,tier.Drain,tier.Recovery);}
   LastActualMovement277=Player.transform.position-start;LastMovementDt277=dt;
  }
 }
 public sealed partial class RaidUI {
  public static string SprintStatus2711(SprintState2711 sprint,bool paused)=>paused?"暂停":!sprint.Running&&sprint.Stamina<SprintState2711.Maximum&&sprint.RecoveryDelayRemaining>0?$"恢复等待 {sprint.RecoveryDelayRemaining:0.0}秒":!sprint.ReadyToRun?(sprint.Intent?$"恢复至{SprintState2711.ResumeAt:0.#}自动跑 · Shift关闭":"恢复中 · 已关闭奔跑"):sprint.Running?"奔跑中 · Shift关闭":sprint.Intent?"奔跑开启 · 移动时跑":"步行 · Shift开启奔跑";
  public static float SprintFill2711(SprintState2711 sprint)=>Mathf.Clamp01(sprint.Stamina/SprintState2711.Maximum);
  void SprintHUD2711(){
   if(G.Phase!=GamePhase.Raid)return;WeightHUD320();var sprint=G.Sprint2711;
   string status=SprintStatus2711(sprint,G.Paused);
   Color color=sprint.Exhausted?gold:sprint.Running?mint:muted;
   Label(50,819,335,22,$"体力  {sprint.Stamina:0.0} / {SprintState2711.Maximum:0}",18,color);Label(50,842,335,17,status,12,color);
   Fill(new Rect(50,861,335,5),new Color(.15f,.23f,.23f));Fill(new Rect(50,861,335*SprintFill2711(sprint),5),color);
  }
 }
}
