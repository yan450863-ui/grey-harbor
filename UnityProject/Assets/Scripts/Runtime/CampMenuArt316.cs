using System;using System.IO;using UnityEngine;
namespace GreyHarbor {
 public sealed partial class RaidUI {
  Texture2D homeArt316;bool homeArtAttempted316;
  internal static readonly Rect[] HomeCards316={new Rect(296,230,320,256),new Rect(640,230,320,256),new Rect(984,230,320,256),new Rect(296,502,320,256),new Rect(640,502,320,256),new Rect(984,502,320,256)};
  internal static readonly Rect HomeSettings316=new Rect(26,28,157,54),HomeScene316=new Rect(1370,28,202,54);
  internal Texture2D HomeArt316(){if(homeArtAttempted316)return homeArt316;homeArtAttempted316=true;var path=Path.Combine(Application.streamingAssetsPath,"MenuArt316/CampMenuBase.png");try{if(File.Exists(path)){var t=new Texture2D(2,2,TextureFormat.RGBA32,false){name="CampMenuBase316",filterMode=FilterMode.Bilinear,wrapMode=TextureWrapMode.Clamp};if(t.LoadImage(File.ReadAllBytes(path),true))homeArt316=t;else Destroy(t);}if(homeArt316==null)homeArt316=Resources.Load<Texture2D>("MenuArt316/CampMenuBase");}catch(Exception ex){Debug.LogWarning("Menu art unavailable: "+ex.Message);}return homeArt316;}
  void HomeLabel316(Rect rect,string label,int size,Color color){var style=text.fontStyle;text.fontStyle=FontStyle.Bold;try{Label(rect.x+1,rect.y+2,rect.width,rect.height,label,size,new Color(.04f,.06f,.065f,.8f),TextAnchor.MiddleCenter);Label(rect.x,rect.y,rect.width,rect.height,label,size,color,TextAnchor.MiddleCenter);}finally{text.fontStyle=style;}}
  bool HomeButton316(Rect rect){if(GUI.enabled&&rect.Contains(Event.current.mousePosition)){Fill(new Rect(rect.x+4,rect.y+4,rect.width-8,rect.height-8),new Color(.85f,.95f,.94f,.055f));Outline(new Rect(rect.x+3,rect.y+3,rect.width-6,rect.height-6),new Color(.65f,.85f,.86f,.8f),2);}return GUI.Button(rect,GUIContent.none,GUIStyle.none);}
  bool DrawHomeArt316(){var art=HomeArt316();if(art==null)return false;
   // The illustration and hit regions share the existing centered 1600x900 UI transform.
   // Extend a solid sky color into letterbox margins without stretching the menu geometry.
   Fill(new Rect(-Screen.width/Mathf.Max(scale,.01f),-Screen.height/Mathf.Max(scale,.01f),3*Screen.width/Mathf.Max(scale,.01f),3*Screen.height/Mathf.Max(scale,.01f)),new Color(.26f,.38f,.46f));
   var tint=GUI.color;GUI.color=Color.white;GUI.DrawTexture(new Rect(0,0,1600,900),art,ScaleMode.StretchToFill,false);GUI.color=tint;
   HomeLabel316(new Rect(420,38,760,125),"灰港行动",84,new Color(.10f,.15f,.17f));Fill(new Rect(575,175,450,2),new Color(.16f,.22f,.24f,.8f));Fill(new Rect(782,173,36,5),new Color(.86f,.62f,.29f));
   HomeLabel316(new Rect(79,31,97,46),"设置",25,white);HomeLabel316(new Rect(1432,31,132,46),"进入营地",23,white);
   if(HomeButton316(HomeSettings316)){G.Paused=true;return true;}if(HomeButton316(HomeScene316)){G.ShowCampScene315();return true;}
   string[] names={"出击","仓库","交易","改装","营地","NPC"};CampPage315[] pages={CampPage315.Departure,CampPage315.Warehouse,CampPage315.Market,CampPage315.Modification,CampPage315.Camp,CampPage315.NPC};for(int i=0;i<6;i++){var r=HomeCards316[i];HomeLabel316(new Rect(r.x+24,r.yMax-82,r.width-48,62),names[i],36,white);if(HomeButton316(r)){G.OpenCampPage315(pages[i]);return true;}}
   // Only actionable notices are shown; cards have no descriptions or extra open buttons.
   if(G.NoticeTime>0||G.CampLocked){var message=G.CampLocked?"存档不可用，物资操作和出击已暂停。":G.Notice;Fill(new Rect(380,830,840,42),new Color(.05f,.09f,.10f,.86f));Label(396,830,808,42,message,17,gold,TextAnchor.MiddleCenter);}
   Label(1390,864,170,23,"3.0.28",12,new Color(.8f,.85f,.8f,.9f),TextAnchor.MiddleRight);return true;
  }
 }
}
