using UnityEngine;
namespace GreyHarbor {
 public enum GraphicsMode2721 { VSync=0,Fps60=1,Fps120=2 }
 public sealed partial class RaidGame {
  public GraphicsMode2721 CurrentGraphicsMode2721=>inventoryPreferences.GraphicsMode2721;
  public string GraphicsPreferencesPath2721=>InventoryPreferencesPath;
  public static bool IsGraphicsMode2721Valid(GraphicsMode2721 mode)=>mode==GraphicsMode2721.VSync||mode==GraphicsMode2721.Fps60||mode==GraphicsMode2721.Fps120;
  public void LoadGraphicsPreferences2721(){LoadInventoryPreferences();ApplyGraphicsMode2721();LoadDisplay311();}
  public void ApplyGraphicsMode2721(){
   var mode=CurrentGraphicsMode2721;
   QualitySettings.vSyncCount=mode==GraphicsMode2721.VSync?1:0;
   Application.targetFrameRate=mode==GraphicsMode2721.Fps60?60:mode==GraphicsMode2721.Fps120?120:-1;
  }
  public bool SetGraphicsPreferences2721(GraphicsMode2721 mode)=>SetPreferences2721(AutoSafeEnabled,AutoSafeThreshold,mode);
 }
}
