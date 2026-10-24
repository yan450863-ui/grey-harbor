using System;
using UnityEngine;
namespace GreyHarbor {
 // Neutral extension boundary: no optional feature behavior or persisted state lives here.
 public static class OptionalFeatures2761 {
  public static Action<RaidGame> Reset, Shutdown;
  public static Func<RaidActor,bool> IncomingDamageVeto;
  public static Func<RaidUI,bool> SettingsOverlay;
  public static Action<RaidUI> SettingsEntry, CloseOverlay;
  public static Func<RaidGame,string> Status;
  public static Func<RaidGame,int> Speed;
  public static Func<RaidGame,int,bool> SetSpeed;
  public static bool Registered=>SettingsEntry!=null;
  public static bool BlocksDamage(RaidActor actor)=>IncomingDamageVeto?.Invoke(actor)??false;
  public static void Clear(){Reset=null;Shutdown=null;IncomingDamageVeto=null;SettingsOverlay=null;SettingsEntry=null;CloseOverlay=null;Status=null;Speed=null;SetSpeed=null;Time.timeScale=1;}
 }
 public sealed partial class RaidGame {
  // Compatibility facade for older verification fixtures. Without a module it stays neutral.
  public int CheckSpeed142=>OptionalFeatures2761.Speed?.Invoke(this)??1;
  public bool SetCheckSpeed142(int value)=>OptionalFeatures2761.SetSpeed?.Invoke(this,value)??value==1;
  public void ResetOptionalFeatures2761(){Time.timeScale=1;OptionalFeatures2761.Reset?.Invoke(this);}
  void OnDestroy(){DisposeScope30();OptionalFeatures2761.Shutdown?.Invoke(this);Time.timeScale=1;}
 }
}
