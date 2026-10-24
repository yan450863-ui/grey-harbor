using UnityEngine;

namespace GreyHarbor
{
    [DefaultExecutionOrder(400)]
    public sealed partial class RaidUI : MonoBehaviour
    {
        RaidGame G=>RaidGame.Instance;
        Font font;
        GUIStyle text;
        readonly Color panel=new Color(.047f,.074f,.083f,.94f),muted=PixelArt.Hex("8EA4A7"),white=PixelArt.Hex("ECF0DF"),mint=PixelArt.Hex("97D9BC"),gold=PixelArt.Hex("E8BB72"),red=PixelArt.Hex("EB8068");
        float scale;int campTab,recipePage;bool allowCertified,confirmDeparture;
        void Setup()
        {
            if(font!=null)return;font=Font.CreateDynamicFontFromOSFont(new[]{"Microsoft YaHei","SimHei","Arial"},24);
            text=new GUIStyle{font=font,richText=false,wordWrap=false,alignment=TextAnchor.UpperLeft};
        }
        void OnGUI()
        {
            Setup();if(!G.InventoryOpen||G.Panel!=CampPanel.Market)ResetMarketUI();scale=Mathf.Min(Screen.width/1600f,Screen.height/900f);GUI.matrix=Matrix4x4.TRS(new Vector3((Screen.width-1600*scale)/2,(Screen.height-900*scale)/2,0),Quaternion.identity,Vector3.one*scale);
            if(!G.InventoryOpen){modPanel30=false;G.CloseKeyCase314();}
            G.UpdateCursor311();if(G.IsLoading2721){DrawMapLoading2721();return;}

            if(MenuVisible315){GUI.enabled=!G.Paused;if(modPanel30)DrawModPanel30();else CampMenu315();GUI.enabled=true;if(G.Paused)Pause();else {inventorySettingsOpen=false;OptionalFeatures2761.CloseOverlay?.Invoke(this);}return;}
            if(G.Phase==GamePhase.Base)CampHUD();else {DrawScope30();HUD();DrawOfficeFall2748();}
            if(modPanel30){DrawModPanel30();if(G.Paused)Pause();return;}
            if(G.InventoryOpen){if(G.Panel==CampPanel.Workbench)WorkbenchUI();else if(G.Panel==CampPanel.Market)MarketUI();else Inventory();}else {dragging=false;context=false;lastItemClick=-10;tooltip=splitOpen=false;selectedId=null;if(G.ReticleMode306==AimReticleMode306.HipFire)Crosshair();}
            if(G.Paused)Pause();else {inventorySettingsOpen=false;OptionalFeatures2761.CloseOverlay?.Invoke(this);}
            if(G.Phase==GamePhase.Raid&&!G.InventoryOpen&&!G.Paused)WhiteboxOverlay();
            // M map now shares the task-capable full overview, including underground.
        }
        void Fill(Rect r,Color c){Color old=GUI.color;GUI.color=c;GUI.DrawTexture(r,Texture2D.whiteTexture);GUI.color=old;}
        void Label(float x,float y,float w,float h,string s,int size,Color c,TextAnchor align=TextAnchor.UpperLeft)
        {text.fontSize=size;text.normal.textColor=c;text.alignment=align;GUI.Label(new Rect(x,y,w,h),s,text);}
        void Line(float x,float y,float w,Color c)=>Fill(new Rect(x,y,w,1),c);
        bool Button(Rect r,string title,bool accent=false)
        {
            bool over=r.Contains(Event.current.mousePosition);Fill(r,accent?(over?mint:new Color(.38f,.68f,.55f)):over?new Color(.18f,.25f,.26f):new Color(.12f,.18f,.19f));
            Label(r.x,r.y,r.width,r.height,title,18,accent?panel:white,TextAnchor.MiddleCenter);
            return GUI.Button(r,GUIContent.none,GUIStyle.none);
        }
        void HUD() { DrawRaidHud329(); }
        void ExteriorGuide(){
            int nearest=0;float best=float.MaxValue;for(int i=0;i<G.Map.ExteriorAreaPoints.Count;i++){float d=(G.Player.transform.position-G.Map.ExteriorAreaPoints[i]).sqrMagnitude;if(d<best){best=d;nearest=i;}}
            Fill(new Rect(28,109,353,56),panel);Label(48,119,320,23,RaidMap.ExteriorAreaNames[nearest],17,mint);Label(48,144,320,18,"四种异种 · 建筑长按 F 两秒进入",12,muted);
            foreach(var p in G.Map.ClosedEntrances)if(Vector3.Distance(G.Player.transform.position,p)<2.7f){var s=ScreenPoint(p+Vector3.up*2);s.x=Mathf.Clamp(s.x,420,1120);s.y=Mathf.Clamp(s.y,185,610);Fill(new Rect(s.x-120,s.y,240,35),panel);Label(s.x-120,s.y,240,35,"建筑暂不可进入",16,gold,TextAnchor.MiddleCenter);break;}
        }
        string Direction() {int n=Mathf.RoundToInt((G.Rig.Yaw-45)/90);string[] s={"东北视角","东南视角","西南视角","西北视角"};return s[(n%4+4)%4];}
        void Progress(string title,float amount,Color color)
        {Fill(new Rect(590,673,420,62),panel);Label(590,679,420,32,title,18,color,TextAnchor.MiddleCenter);Fill(new Rect(610,722,380,4),muted);Fill(new Rect(610,722,380*Mathf.Clamp01(amount),4),color);}
        Vector2 ScreenPoint(Vector3 p)
        {var s=G.View.WorldToScreenPoint(p);return new Vector2((s.x-(Screen.width-1600*scale)/2)/scale,(Screen.height-s.y-(Screen.height-900*scale)/2)/scale);}
        void ExtractMarker()
        {
            if(G.Map.TransitWhitebox346)return;
            Vector2 p=ScreenPoint(G.NearestExit()+Vector3.up*.5f);p.x=Mathf.Clamp(p.x,440,1190);p.y=Mathf.Clamp(p.y,170,610);
            Label(p.x-80,p.y,160,30,$"◇ 撤离 {Vector3.Distance(G.Player.transform.position,G.NearestExit()):0}m",15,mint,TextAnchor.MiddleCenter);
        }
        void Crosshair()
        {
            if(G.ReticleMode306!=AimReticleMode306.HipFire)return;
            Vector2 m=G.AkCrosshairScreen276;float x=(m.x-(Screen.width-1600*scale)/2)/scale,y=(Screen.height-m.y-(Screen.height-900*scale)/2)/scale;Color c=G.HitMarker>0?red:white;
            float gap=SpreadReticle308.Gap(G.AkCrosshairRadius276,scale);
            Fill(new Rect(x-gap-6,y-1,6,2),c);Fill(new Rect(x+gap,y-1,6,2),c);Fill(new Rect(x-1,y-gap-6,2,6),c);Fill(new Rect(x-1,y+gap,2,6),c);
        }
        void Pause()
        {
            Fill(new Rect(0,0,1600,900),new Color(0,.02f,.025f,.7f));if(inventorySettingsOpen){DrawInventorySettings();return;}Fill(new Rect(555,255,490,430),panel);Label(555,292,490,52,"行动暂停",32,white,TextAnchor.MiddleCenter);
            if(Button(new Rect(605,376,390,58),"继续行动",true))G.Paused=false;
            if(Button(new Rect(605,450,390,58),"设置"))OpenInventorySettings();
            if(G.Phase==GamePhase.Raid){if(Button(new Rect(605,524,390,58),G.Map.TransitWhitebox346?"结束试玩 · 返回营地":G.TrainingActive?"结束训练 · 恢复原装备":"放弃行动 · 仅保留安全箱"))G.Finish(false);}else if(Button(new Rect(605,524,390,58),"退出游戏"))Application.Quit();
            Label(555,620,490,24,"单人离线模式 / Esc 返回",15,muted,TextAnchor.MiddleCenter);
        }
    }
}

