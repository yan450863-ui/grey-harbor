using System;
using System.IO;
using UnityEngine;
namespace GreyHarbor {
 [Serializable] public class InventoryPreferences {
  public int EconomyVersion=1;
  public DisplayMode311 DisplayMode311=GreyHarbor.DisplayMode311.Borderless;
  public bool AutoSafeEnabled;
  public int AutoSafeThreshold=1000;
  public GraphicsMode2721 GraphicsMode2721=GreyHarbor.GraphicsMode2721.VSync;
 }
 public sealed partial class RaidGame {
  InventoryPreferences inventoryPreferences=new InventoryPreferences();
  public bool AutoSafeEnabled=>inventoryPreferences.AutoSafeEnabled;
  public int AutoSafeThreshold=>inventoryPreferences.AutoSafeThreshold;
  public string InventoryPreferencesPath=>Path.Combine(Path.GetDirectoryName(SavePath),VerifyMode||SavePath.Contains("ui-check")?Path.GetFileNameWithoutExtension(SavePath)+"-settings.json":"inventory-settings-2.7.json");
  public void LoadInventoryPreferences(){
   try{
    string source=InventoryPreferencesPath;
    if(!File.Exists(source)&&!VerifyMode&&!SavePath.Contains("ui-check"))source=Path.Combine(Path.GetDirectoryName(SavePath),"inventory-settings.json");
    if(File.Exists(source)){
     string json=File.ReadAllText(source);var p=JsonUtility.FromJson<InventoryPreferences>(json);
     if(p!=null){
      if(!json.Contains("\"EconomyVersion\""))p.AutoSafeThreshold=checked(p.AutoSafeThreshold*10);
      if(p.AutoSafeThreshold<0||p.AutoSafeThreshold>10000000)throw new Exception("设置金额越界");
      if(!IsGraphicsMode2721Valid(p.GraphicsMode2721))p.GraphicsMode2721=GraphicsMode2721.VSync;
      if(!ValidDisplay311(p.DisplayMode311))p.DisplayMode311=DisplayMode311.Borderless;
      p.EconomyVersion=1;inventoryPreferences=p;
     }
    }
   }catch(Exception e){Debug.LogWarning("读取设置失败："+e.Message);}
  }

  public bool SetInventoryPreferences(bool enabled,int threshold)=>SetPreferences2721(enabled,threshold,CurrentGraphicsMode2721);

  // One atomic file contains all persistent settings: a failed write leaves every draft unapplied.
  public bool SetPreferences2721(bool enabled,int threshold,GraphicsMode2721 graphicsMode,DisplayMode311? displayMode=null){
   var display=displayMode??CurrentDisplayMode311;
   if(!ValidDisplay311(display))return false;
   if(threshold<0||threshold>10000000||!IsGraphicsMode2721Valid(graphicsMode))return false;
   var p=new InventoryPreferences{AutoSafeEnabled=enabled,AutoSafeThreshold=threshold,GraphicsMode2721=graphicsMode,DisplayMode311=display};
   string temp=null;
   try{
    string path=InventoryPreferencesPath;temp=path+"."+Guid.NewGuid().ToString("N")+".tmp";
    File.WriteAllText(temp,JsonUtility.ToJson(p,true));
    if(File.Exists(path))File.Replace(temp,path,path+".bak");else File.Move(temp,path);
   }catch(Exception e){
    Notify("设置保存失败，原设置保留。");Debug.LogWarning(e.Message);return false;
   }finally{
    if(temp!=null&&File.Exists(temp)){try{File.Delete(temp);}catch(Exception e){Debug.LogWarning("清理设置临时文件失败："+e.Message);}}
   }
   inventoryPreferences=p;ApplyGraphicsMode2721();ApplyDisplay311(display);return true;
  }
 }
 public sealed partial class RaidUI {
  bool inventorySettingsOpen,autoSafeDraft;string safePriceDraft="1000";
  GraphicsMode2721 graphicsModeDraft2721=GraphicsMode2721.VSync;
  DisplayMode311 displayDraft311=DisplayMode311.Borderless;
  public void OpenInventorySettings(){inventorySettingsOpen=true;autoSafeDraft=G.AutoSafeEnabled;safePriceDraft=G.AutoSafeThreshold.ToString();OptionalFeatures2761.CloseOverlay?.Invoke(this);graphicsModeDraft2721=G.CurrentGraphicsMode2721;displayDraft311=Screen.fullScreenMode==FullScreenMode.Windowed?DisplayMode311.Windowed:DisplayMode311.Borderless;}
  void DrawInventorySettings(){
   if(OptionalFeatures2761.SettingsOverlay?.Invoke(this)??false)return;
   Fill(new Rect(500,72,600,756),panel);Label(525,91,550,44,"设置",28,white);
   Label(535,148,530,30,"画面 · 帧率模式",22,mint);
   GraphicsMode2721[] modes={GraphicsMode2721.Fps60,GraphicsMode2721.Fps120,GraphicsMode2721.VSync};
   string[] labels={"60帧","120帧","垂直同步"};
   for(int i=0;i<modes.Length;i++){var mode=modes[i];if(Button(new Rect(535+i*180,185,170,46),labels[i],graphicsModeDraft2721==mode))graphicsModeDraft2721=mode;}
   WrappedLabel(new Rect(535,241,530,52),"垂直同步跟随屏幕刷新率；60Hz屏幕约60帧。120帧需硬件支持，性能不足时仍会低于设定。",16,muted);
   Label(535,299,530,30,"显示模式 · F11 / Alt+Enter 切换",20,mint);
   if(Button(new Rect(535,335,255,40),"无边框全屏",displayDraft311==DisplayMode311.Borderless))displayDraft311=DisplayMode311.Borderless;
   if(Button(new Rect(810,335,255,40),"窗口模式",displayDraft311==DisplayMode311.Windowed))displayDraft311=DisplayMode311.Windowed;
   OptionalFeatures2761.SettingsEntry?.Invoke(this);
   if(Button(new Rect(535,515,530,48),"自动放入安全箱："+(autoSafeDraft?"开启":"关闭"),autoSafeDraft))autoSafeDraft=!autoSafeDraft;
   Label(535,584,280,30,"出售单价达到（金币）",18,white);var style=new GUIStyle(GUI.skin.textField){font=font,fontSize=22};safePriceDraft=GUI.TextField(new Rect(825,581,240,38),safePriceDraft,8,style);
   WrappedLabel(new Rect(535,638,530,64),"仅影响局内双击拾取的入箱优先级；按单件售价判断。背包与胸挂放不下时仍尝试安全箱，装备除外。手动拖动不受影响。",16,muted);
   bool valid=int.TryParse(safePriceDraft,out int amount)&&amount>=0&&amount<=10000000;
   if(!valid)Label(535,712,530,26,"请输入0～10000000的整数",16,red);
   if(Button(new Rect(535,752,240,48),"返回"))inventorySettingsOpen=false;
   bool old=GUI.enabled;GUI.enabled=valid;if(Button(new Rect(795,752,270,48),"保存设置",true)&&valid&&G.SetPreferences2721(autoSafeDraft,amount,graphicsModeDraft2721,displayDraft311)){inventorySettingsOpen=false;}GUI.enabled=old;
  }
 }
}
