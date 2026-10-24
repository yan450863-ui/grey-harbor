#if GREY_HARBOR_TEST_CONSOLE
using System;
using UnityEngine;
namespace GreyHarbor {
 // Delete this folder and remove GREY_HARBOR_TEST_CONSOLE for a clean release.
 public static class TestConsole2761 {
  [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
  public static void Register(){
   OptionalFeatures2761.Clear();
   OptionalFeatures2761.Reset=g=>g.ResetConsoleState2761();
   OptionalFeatures2761.Shutdown=g=>g.ResetConsoleState2761();
   OptionalFeatures2761.Speed=g=>g.consoleSpeed2761;
   OptionalFeatures2761.SetSpeed=(g,value)=>g.SetConsoleSpeed2761(value);
   OptionalFeatures2761.IncomingDamageVeto=a=>RaidGame.Instance&&a==RaidGame.Instance.Player&&!a.Dead&&RaidGame.Instance.ConsoleGodMode2761;
   OptionalFeatures2761.SettingsEntry=ui=>ui.DrawConsoleEntry2761();
   OptionalFeatures2761.SettingsOverlay=ui=>ui.DrawConsole2761();
   OptionalFeatures2761.CloseOverlay=ui=>ui.CloseConsole2761();
   if(RaidGame.Instance)RaidGame.Instance.ResetConsoleState2761();
   OptionalFeatures2761.Status=g=>(g.consoleSpeed2761>1?"  ·  "+g.consoleSpeed2761+"×":"")+(g.ConsoleGodMode2761?"  ·  无敌":"");
  }
 }
 public sealed partial class RaidGame {
  internal int consoleSpeed2761=1;
  public bool ConsoleGodMode2761 {get;private set;}
  internal void ResetConsoleState2761(){consoleSpeed2761=1;ConsoleGodMode2761=false;Time.timeScale=1;}
  internal bool SetConsoleSpeed2761(int value){if(value!=1&&value!=2&&value!=5)return false;consoleSpeed2761=value;Time.timeScale=value;return true;}
  public void SetConsoleGodMode2761(bool enabled){ConsoleGodMode2761=enabled;}
  public bool AddConsoleMoney2761(int amount){
   if(amount<=0||amount>10000000||CampLocked||TrainingActive||IsLoading2721||Save==null||(Phase!=GamePhase.Base&&Phase!=GamePhase.Raid)){Notify("当前状态不能加钱，或金额无效。");return false;}
   if(Save.Coins<0||Save.Coins>int.MaxValue-amount){Notify("金币已达到上限。");return false;}
   if(Phase==GamePhase.Base)return CampChange(()=>{Save.Coins=checked(Save.Coins+amount);return true;},"控制台：已增加 "+amount+" 金币。");
   // The existing atomic save writer is the single transaction boundary in a raid too.
   string before=JsonUtility.ToJson(Save);
   Save.Coins=checked(Save.Coins+amount);
   if(!WriteSave()){Save=JsonUtility.FromJson<BaseSave>(before);return false;}
   Notify("控制台：已增加 "+amount+" 金币。");return true;
  }
 }
 public sealed partial class RaidUI {
  bool consoleOpen2761;
  string consoleMoneyDraft2761="10000",consoleResult2761="";
  public bool ConsoleOpen2761=>consoleOpen2761;
  public void OpenConsole2761(){consoleOpen2761=true;consoleMoneyDraft2761="10000";consoleResult2761="";}
  internal void CloseConsole2761(){consoleOpen2761=false;consoleResult2761="";}
  internal void DrawConsoleEntry2761(){
   Label(535,385,530,30,"试玩工具",22,mint);
   if(Button(new Rect(535,421,530,40),"控制台"))OpenConsole2761();
   WrappedLabel(new Rect(535,470,530,32),"加速、无敌与加钱仅供当前试玩版本使用。",16,muted);
  }
  internal bool DrawConsole2761(){
   if(!consoleOpen2761)return false;
   Fill(new Rect(460,72,680,756),panel);Label(495,94,610,42,"控制台 · 试玩工具",28,white);
   WrappedLabel(new Rect(495,148,610,50),"速度和无敌只在本次启动生效；加钱会写入当前存档。",17,muted);
   Label(495,212,610,30,"加速",22,mint);
   int[] speeds={1,2,5};for(int i=0;i<speeds.Length;i++){int speed=speeds[i];if(Button(new Rect(495+i*205,253,195,48),speed+"×",G.CheckSpeed142==speed))G.SetCheckSpeed142(speed);}
   if(Button(new Rect(495,335,605,48),"无敌："+(G.ConsoleGodMode2761?"开启":"关闭"),G.ConsoleGodMode2761))G.SetConsoleGodMode2761(!G.ConsoleGodMode2761);
   WrappedLabel(new Rect(495,396,605,45),"开启后，玩家生命与护甲不受攻击损伤；不会复活已死亡的角色。",16,muted);
   Label(495,458,605,30,"加钱 · 当前金币 "+(G.Save?.Coins??0).ToString("N0"),22,gold);
   var style=new GUIStyle(GUI.skin.textField){font=font,fontSize=22};consoleMoneyDraft2761=GUI.TextField(new Rect(495,504,360,44),consoleMoneyDraft2761,8,style);
   bool valid=int.TryParse(consoleMoneyDraft2761,out int amount)&&amount>0&&amount<=10000000;
   bool available=!G.CampLocked&&!G.TrainingActive&&!G.IsLoading2721&&(G.Phase==GamePhase.Base||G.Phase==GamePhase.Raid);
   bool previous=GUI.enabled;GUI.enabled=previous&&valid&&available;
   if(Button(new Rect(875,504,225,44),"增加金币",true))consoleResult2761=G.AddConsoleMoney2761(amount)?"已增加 "+amount.ToString("N0")+" 金币":"未增加金币，原余额保留。";
   GUI.enabled=previous&&available;
   if(Button(new Rect(495,571,290,44),"＋10,000"))consoleResult2761=G.AddConsoleMoney2761(10000)?"已增加 10,000 金币":"未增加金币，原余额保留。";
   if(Button(new Rect(810,571,290,44),"＋100,000"))consoleResult2761=G.AddConsoleMoney2761(100000)?"已增加 100,000 金币":"未增加金币，原余额保留。";
   GUI.enabled=previous;
   Label(495,638,610,28,!available?"训练和只读存档不能加钱。":!valid?"请输入1～10000000的整数。":consoleResult2761,17,!available||!valid?red:mint);
   if(Button(new Rect(495,752,605,48),"返回设置"))CloseConsole2761();
   return true;
  }
 }
}
#endif

