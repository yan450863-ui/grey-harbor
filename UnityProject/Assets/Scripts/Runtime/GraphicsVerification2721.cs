using System;
using System.IO;
using UnityEngine;
namespace GreyHarbor {
 public sealed partial class RaidVerification {
  void VerifyGraphicsPreferences2721(){G.VerifyGraphicsPreferences2721(Check);}
 }
 public sealed partial class RaidGame {
  // All writes are redirected to a fresh temporary directory; gameplay save APIs are never called.
  public void VerifyGraphicsPreferences2721(Action<bool,string> check){
   string originalPath=SavePath,originalNotice=Notice;bool originalVerify=VerifyMode;
   float originalNoticeTime=NoticeTime,originalTimeScale=Time.timeScale;
   int originalVSync=QualitySettings.vSyncCount,originalTarget=Application.targetFrameRate,originalSpeed=CheckSpeed142;
   var originalPreferences=inventoryPreferences;
   string scratch=Path.GetFullPath(Path.Combine(Path.GetTempPath(),"GreyHarborGraphics2721-"+Guid.NewGuid().ToString("N")));
   Directory.CreateDirectory(scratch);
   try{
    VerifyMode=false;SavePath=Path.Combine(scratch,"stash-v42.json");
    const string stashSentinel="formal-inventory-sentinel";File.WriteAllText(SavePath,stashSentinel);
    inventoryPreferences=new InventoryPreferences();LoadGraphicsPreferences2721();
    check(CurrentGraphicsMode2721==GraphicsMode2721.VSync&&QualitySettings.vSyncCount==1&&Application.targetFrameRate==-1,"2721 Default VSync follows display refresh with target -1");
    check(!AutoSafeEnabled&&AutoSafeThreshold==1000,"2721 Existing default auto-safe preferences preserved");
    string normalPath=InventoryPreferencesPath;
    check(normalPath==Path.Combine(scratch,"inventory-settings-2.7.json")&&GraphicsPreferencesPath2721==normalPath&&normalPath!=SavePath,"2721 Persistent graphics and item settings share one atomic file separate from inventory");
    foreach(var mode in new[]{GraphicsMode2721.Fps60,GraphicsMode2721.Fps120,GraphicsMode2721.VSync}){
     check(SetGraphicsPreferences2721(mode),"2721 Save graphics mode "+mode);
     int expectedVSync=mode==GraphicsMode2721.VSync?1:0,expectedTarget=mode==GraphicsMode2721.Fps60?60:mode==GraphicsMode2721.Fps120?120:-1;
     check(CurrentGraphicsMode2721==mode&&QualitySettings.vSyncCount==expectedVSync&&Application.targetFrameRate==expectedTarget,"2721 Real Unity frame control matches "+mode);
     inventoryPreferences=new InventoryPreferences();LoadGraphicsPreferences2721();
     check(CurrentGraphicsMode2721==mode&&QualitySettings.vSyncCount==expectedVSync&&Application.targetFrameRate==expectedTarget,"2721 Mode persists through preference reload "+mode);
    }
    File.WriteAllText(normalPath,"{\"EconomyVersion\":1,\"AutoSafeEnabled\":true,\"AutoSafeThreshold\":4321}");
    inventoryPreferences=new InventoryPreferences();LoadGraphicsPreferences2721();
    check(CurrentGraphicsMode2721==GraphicsMode2721.VSync&&AutoSafeEnabled&&AutoSafeThreshold==4321,"2721 Existing settings without a graphics field gain VSync and keep auto-safe settings");
    File.WriteAllText(normalPath,"{\"EconomyVersion\":1,\"AutoSafeEnabled\":true,\"AutoSafeThreshold\":4321,\"GraphicsMode2721\":999}");
    inventoryPreferences=new InventoryPreferences();LoadGraphicsPreferences2721();
    check(CurrentGraphicsMode2721==GraphicsMode2721.VSync&&AutoSafeEnabled&&AutoSafeThreshold==4321,"2721 Invalid stored graphics mode falls back to VSync without dropping valid item settings");
    File.Delete(normalPath);File.WriteAllText(Path.Combine(scratch,"inventory-settings.json"),"{\"AutoSafeEnabled\":true,\"AutoSafeThreshold\":450}");
    inventoryPreferences=new InventoryPreferences();LoadGraphicsPreferences2721();
    check(AutoSafeEnabled&&AutoSafeThreshold==4500&&CurrentGraphicsMode2721==GraphicsMode2721.VSync,"2721 Legacy economy migration still multiplies auto-safe amount once");
    check(SetPreferences2721(true,2345,GraphicsMode2721.Fps120),"2721 Combined graphics and auto-safe commit succeeds");
    string savedJson=File.ReadAllText(normalPath);
    check(AutoSafeEnabled&&AutoSafeThreshold==2345&&CurrentGraphicsMode2721==GraphicsMode2721.Fps120&&savedJson.Contains("2345")&&savedJson.Contains("GraphicsMode2721"),"2721 Both persistent drafts commit together");
    check(SetInventoryPreferences(false,5678)&&CurrentGraphicsMode2721==GraphicsMode2721.Fps120,"2721 Existing item-only setter preserves the graphics selection");
    savedJson=File.ReadAllText(normalPath);
    check(!SetPreferences2721(true,-1,GraphicsMode2721.VSync)&&!SetGraphicsPreferences2721((GraphicsMode2721)999)&&File.ReadAllText(normalPath)==savedJson&&!AutoSafeEnabled&&AutoSafeThreshold==5678&&CurrentGraphicsMode2721==GraphicsMode2721.Fps120,"2721 Invalid drafts change neither the file nor active settings");
    string blocked=Path.Combine(scratch,"blocked-directory");File.WriteAllText(blocked,"not a directory");
    VerifyMode=true;SavePath=Path.Combine(blocked,"verification-stash-2.7.21.json");
    check(!SetPreferences2721(true,8888,GraphicsMode2721.Fps60),"2721 Simulated write failure is reported");
    check(!AutoSafeEnabled&&AutoSafeThreshold==5678&&CurrentGraphicsMode2721==GraphicsMode2721.Fps120&&QualitySettings.vSyncCount==0&&Application.targetFrameRate==120&&File.ReadAllText(normalPath)==savedJson,"2721 Failed save rolls back graphics and item settings together");
    SavePath=Path.Combine(scratch,"verification-stash-2.7.21.json");string verifyPath=InventoryPreferencesPath;
    check(SetPreferences2721(true,42,GraphicsMode2721.Fps60),"2721 Verify preferences save independently");
    VerifyMode=false;SavePath=Path.Combine(scratch,"ui-check-stash-2.7.21.json");string uiPath=InventoryPreferencesPath;
    check(SetPreferences2721(false,84,GraphicsMode2721.VSync),"2721 UI-check preferences save independently");
    check(normalPath!=verifyPath&&normalPath!=uiPath&&verifyPath!=uiPath&&File.ReadAllText(normalPath)==savedJson,"2721 Formal, Verify and UI-check settings have independent paths");
    VerifyMode=true;SavePath=Path.Combine(scratch,"verification-stash-2.7.21.json");inventoryPreferences=new InventoryPreferences();LoadGraphicsPreferences2721();
    check(AutoSafeEnabled&&AutoSafeThreshold==42&&CurrentGraphicsMode2721==GraphicsMode2721.Fps60,"2721 Verify reload reads its own settings");
    VerifyMode=false;SavePath=Path.Combine(scratch,"ui-check-stash-2.7.21.json");inventoryPreferences=new InventoryPreferences();LoadGraphicsPreferences2721();
    check(!AutoSafeEnabled&&AutoSafeThreshold==84&&CurrentGraphicsMode2721==GraphicsMode2721.VSync,"2721 UI-check reload reads its own settings");
    check(CheckSpeed142==originalSpeed&&Time.timeScale==originalTimeScale,"2721 Frame settings leave inspection speed and time scale unchanged");
    check(File.ReadAllText(Path.Combine(scratch,"stash-v42.json"))==stashSentinel,"2721 Preference operations never write the inventory file");
    check(Directory.GetFiles(scratch,"*.tmp",SearchOption.AllDirectories).Length==0,"2721 Atomic settings saves leave no temporary files");
   }finally{
    SavePath=originalPath;VerifyMode=originalVerify;inventoryPreferences=originalPreferences;
    QualitySettings.vSyncCount=originalVSync;Application.targetFrameRate=originalTarget;
    Notice=originalNotice;NoticeTime=originalNoticeTime;
    if(Path.GetFullPath(scratch)==scratch&&Path.GetDirectoryName(scratch)==Path.GetFullPath(Path.GetTempPath()).TrimEnd(Path.DirectorySeparatorChar,Path.AltDirectorySeparatorChar))Directory.Delete(scratch,true);
   }
  }
 }
}
