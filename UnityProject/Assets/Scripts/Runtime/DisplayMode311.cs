using System;using UnityEngine;
namespace GreyHarbor {
 public enum DisplayMode311 { Borderless=0,Windowed=1 }
 public sealed partial class RaidGame {
  public DisplayMode311 CurrentDisplayMode311=>inventoryPreferences.DisplayMode311;
  public static bool ValidDisplay311(DisplayMode311 mode)=>mode==DisplayMode311.Borderless||mode==DisplayMode311.Windowed;
  public void LoadDisplay311(){if(!VerifyMode)ApplyDisplay311(Array.IndexOf(Environment.GetCommandLineArgs(),"--windowed")>=0?DisplayMode311.Windowed:CurrentDisplayMode311);}
  public void ApplyDisplay311(DisplayMode311 mode){
   var full=mode==DisplayMode311.Borderless?FullScreenMode.FullScreenWindow:FullScreenMode.Windowed;
   if(mode==DisplayMode311.Windowed&&Screen.fullScreenMode==full)return;
   var desktop=Display.main;var current=Screen.currentResolution;int w=desktop.systemWidth>0?desktop.systemWidth:Mathf.Max(800,current.width),h=desktop.systemHeight>0?desktop.systemHeight:Mathf.Max(600,current.height);
   if(mode==DisplayMode311.Windowed){w=Mathf.Min(1600,w);h=Mathf.Min(900,h);}
   if(Screen.fullScreenMode==full&&Screen.width==w&&Screen.height==h)return;
   Screen.SetResolution(w,h,full);
  }
  public bool SetDisplay311(DisplayMode311 mode)=>SetPreferences2721(AutoSafeEnabled,AutoSafeThreshold,CurrentGraphicsMode2721,mode);
  public bool ConfineCursor311(bool focused)=>focused&&Phase==GamePhase.Raid&&!Paused&&!InventoryOpen&&GetComponent<RaidUI>()?.FullMapOpen318!=true&&!IsLoading2721&&Player&&!Player.Dead;
  public void UpdateCursor311(){bool confine=ConfineCursor311(Application.isFocused);var state=confine?CursorLockMode.Confined:CursorLockMode.None;if(Cursor.lockState!=state)Cursor.lockState=state;Cursor.visible=!confine;}
  void TickDisplay311(){
   if(!VerifyMode&&(Input.GetKeyDown(KeyCode.F11)||(Input.GetKeyDown(KeyCode.Return)&&(Input.GetKey(KeyCode.LeftAlt)||Input.GetKey(KeyCode.RightAlt))))){SetDisplay311(Screen.fullScreenMode==FullScreenMode.Windowed?DisplayMode311.Borderless:DisplayMode311.Windowed);}
   UpdateCursor311();
  }
  void OnApplicationFocus(bool focused){if(!focused){Cursor.lockState=CursorLockMode.None;Cursor.visible=true;}else UpdateCursor311();}
 }
}
