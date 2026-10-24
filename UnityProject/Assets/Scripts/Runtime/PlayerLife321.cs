using UnityEngine;
namespace GreyHarbor {
 public static class PlayerLife321 {public const float Maximum=120,RecoveryDelay=8,Rate=1.5f;}
 public sealed class PlayerRecovery321 {
  float quietSeconds321;public float WaitRemaining321=>Mathf.Max(0,PlayerLife321.RecoveryDelay-quietSeconds321);public bool Regenerating321 {get;private set;}
  public void Reset321(){quietSeconds321=0;Regenerating321=false;}
  public void MarkCombat321()=>Reset321();
  public void Tick321(RaidActor player,float dt,bool paused,bool sprinting,float multiplier=1){Regenerating321=false;if(!player||player.Dead||player.Health<=0||paused||dt<=0)return;dt=Mathf.Max(0,dt);float waiting=Mathf.Min(dt,WaitRemaining321);quietSeconds321=Mathf.Min(PlayerLife321.RecoveryDelay,quietSeconds321+dt);float available=dt-waiting;if(sprinting||available<=0||player.Health>=PlayerLife321.Maximum)return;float before=player.Health;player.Health=Mathf.Min(PlayerLife321.Maximum,player.Health+available*PlayerLife321.Rate*Mathf.Max(0,multiplier));Regenerating321=player.Health>before;}
 }
 public sealed partial class RaidGame {
  public readonly PlayerRecovery321 Recovery321=new PlayerRecovery321();
  public float RecoveryRate321=>PlayerLife321.Rate*LoadTier320.FutureRegen;
  public void ResetPlayerRecovery321()=>Recovery321.Reset321();
  public void MarkPlayerCombat321()=>Recovery321.MarkCombat321();
  public void TickPlayerRecovery321(float dt){if(Phase!=GamePhase.Raid||Paused||!Player||Player.Dead){Recovery321.Tick321(Player,0,true,false);return;}float running=Mathf.Clamp(LastRunningSeconds321,0,Mathf.Max(0,dt));bool stationaryMenu=InventoryOpen||GetComponent<RaidUI>()?.FullMapOpen318==true;if(stationaryMenu)running=0;if(running>0)Recovery321.Tick321(Player,running,false,true,LoadTier320.FutureRegen);Recovery321.Tick321(Player,dt-running,false,!stationaryMenu&&Sprint2711.Running,LoadTier320.FutureRegen);}
 }
}
