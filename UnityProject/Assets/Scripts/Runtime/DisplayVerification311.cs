using System;using System.Collections;using System.IO;using UnityEngine;
namespace GreyHarbor {public sealed partial class RaidVerification {
 IEnumerator WaitDisplay311(FullScreenMode mode){for(int i=0;i<120&&Screen.fullScreenMode!=mode;i++)yield return null;yield return null;}
 IEnumerator RunDisplay311(){
  G.enabled=false;var priorMode=Screen.fullScreenMode;int priorW=Screen.width,priorH=Screen.height;var preferencesField=typeof(RaidGame).GetField("inventoryPreferences",System.Reflection.BindingFlags.NonPublic|System.Reflection.BindingFlags.Instance);var priorPrefs=preferencesField.GetValue(G);
  string path=G.InventoryPreferencesPath;string saved=File.Exists(path)?File.ReadAllText(path):null;
  try{
   Check(path.Contains("verification-stash-3.0.14"),"display preferences use isolated test save");
   Check(new InventoryPreferences().DisplayMode311==DisplayMode311.Borderless,"new and migrated settings default to borderless fullscreen");
   File.WriteAllText(path,"{\"EconomyVersion\":1,\"AutoSafeEnabled\":true,\"AutoSafeThreshold\":2345,\"GraphicsMode2721\":1}");G.LoadInventoryPreferences();
   Check(G.CurrentDisplayMode311==DisplayMode311.Borderless&&G.AutoSafeEnabled&&G.AutoSafeThreshold==2345,"old settings migrate without losing safe threshold or graphics mode");
   Check(G.SetPreferences2721(true,3456,GraphicsMode2721.Fps60,DisplayMode311.Windowed),"atomic settings save accepts windowed mode");yield return WaitDisplay311(FullScreenMode.Windowed);
   Check(Screen.fullScreenMode==FullScreenMode.Windowed&&Screen.width<=1600&&Screen.height<=900,"live player enters bounded windowed mode");
   Check(G.SetGraphicsPreferences2721(GraphicsMode2721.Fps120)&&G.CurrentDisplayMode311==DisplayMode311.Windowed,"frame rate save preserves display selection");
   G.LoadInventoryPreferences();Check(G.CurrentDisplayMode311==DisplayMode311.Windowed&&G.AutoSafeThreshold==3456&&G.AutoSafeEnabled,"display mode and other settings persist through reload");
   string valid=File.ReadAllText(path);Check(!G.SetDisplay311((DisplayMode311)99)&&File.ReadAllText(path)==valid&&G.CurrentDisplayMode311==DisplayMode311.Windowed,"invalid mode leaves display and persistent settings intact");
   Check(G.SetDisplay311(DisplayMode311.Borderless),"fullscreen selection saves successfully");yield return WaitDisplay311(FullScreenMode.FullScreenWindow);
   var native=Screen.currentResolution;Check(Screen.fullScreenMode==FullScreenMode.FullScreenWindow&&Screen.width==native.width&&Screen.height==native.height,"live fullscreen fills monitor with no window titlebar mode");results.Add("INFO fullscreen "+Screen.width+"x"+Screen.height+" "+Screen.fullScreenMode);
   var phase=G.Phase;bool dead=G.Player.Dead;G.Phase=GamePhase.Raid;G.Player.Dead=false;G.Paused=G.InventoryOpen=false;
   Check(G.ConfineCursor311(true)&&!G.ConfineCursor311(false),"raid confines pointer while focused and releases on focus loss");
   G.Paused=true;Check(!G.ConfineCursor311(true),"pause frees pointer");G.Paused=false;G.InventoryOpen=true;G.UpdateCursor311();Check(!G.ConfineCursor311(true)&&Cursor.visible&&Cursor.lockState==CursorLockMode.None,"inventory frees and shows cursor for dragging");G.InventoryOpen=false;G.Player.Dead=true;Check(!G.ConfineCursor311(true),"dead player frees pointer");G.Player.Dead=dead;G.Phase=GamePhase.Base;G.UpdateCursor311();Check(!G.ConfineCursor311(true)&&Cursor.visible&&Cursor.lockState==CursorLockMode.None,"base menus keep free visible pointer");G.Phase=phase;
   G.SetDisplay311(DisplayMode311.Windowed);yield return WaitDisplay311(FullScreenMode.Windowed);Check(Screen.fullScreenMode==FullScreenMode.Windowed,"fullscreen can switch back to windowed mode");
   results.Add("INFO cursor confine policy tested; hidden unfocused player cannot prove physical OS pointer capture");
  }finally{preferencesField.SetValue(G,priorPrefs);G.ApplyGraphicsMode2721();if(saved==null){if(File.Exists(path))File.Delete(path);}else File.WriteAllText(path,saved);Cursor.lockState=CursorLockMode.None;Cursor.visible=true;Screen.SetResolution(priorW,priorH,priorMode);}
  results.Add("DISPLAY AND FOUR METRE CAMERA 3.0.14 COMPLETE");
 }
}}
