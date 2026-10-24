using UnityEngine;

namespace GreyHarbor
{
    public static class PixelArt
    {
        public static Sprite[] Hero,Guard;
        public static Sprite Rifle,MP5,SKS,Flash,Shadow,Marker;
        static Color32 Ink=new Color32(19,25,29,255);
        public static void Initialize()
        {
            if(Hero!=null)return;
            Hero=new Sprite[4]; Guard=new Sprite[4];
            for(int f=0;f<4;f++) { Hero[f]=Person(false,f); Guard[f]=Person(true,f); }
            var approvedHero=SoldierArt.Load();if(approvedHero!=null)Hero=approvedHero;
            // Side-on AK silhouette: wood stock/handguard, stamped receiver, curved magazine.
            var t=Texture(72,28);
            Rect(t,2,11,17,8,Ink);Rect(t,2,8,6,12,Ink);Rect(t,4,10,4,8,Hex("A46639"));
            Rect(t,7,13,12,5,Hex("AE7041"));Rect(t,6,17,9,2,Hex("D59B60"));Rect(t,16,14,8,4,Ink);
            Rect(t,20,12,24,9,Ink);Rect(t,22,14,20,5,Hex("50595C"));Rect(t,23,19,19,2,Hex("8D9693"));
            Rect(t,22,21,16,2,Ink);Rect(t,37,20,3,5,Ink);Rect(t,38,21,2,3,Hex("77837F"));
            Rect(t,25,8,10,4,Ink);Rect(t,27,9,5,2,Hex("777E72"));Rect(t,23,4,6,9,Ink);Rect(t,24,5,4,7,Hex("986038"));
            Rect(t,33,7,7,7,Ink);Rect(t,35,3,8,6,Ink);Rect(t,39,1,7,4,Ink);
            Rect(t,34,9,4,4,Hex("6C7471"));Rect(t,36,5,5,5,Hex("4C5654"));Rect(t,40,2,4,4,Hex("67716B"));
            Rect(t,44,12,11,8,Ink);Rect(t,45,13,9,5,Hex("A46535"));Rect(t,45,18,9,2,Hex("D29A58"));Rect(t,49,14,1,3,Hex("784A2D"));
            Rect(t,54,13,16,3,Ink);Rect(t,55,14,13,1,Hex("88958E"));Rect(t,54,18,10,2,Ink);Rect(t,60,15,3,6,Ink);
            Rect(t,64,14,3,10,Ink);Rect(t,65,21,2,3,Hex("929C8D"));Rect(t,69,12,2,5,Ink);
            Rifle=Finish(t,new Vector2(24f/72,14f/28),52);
            MP5=MakeMp5279();SKS=MakeSks279();
            t=Texture(12,12);Rect(t,1,4,10,4,Hex("FFAC43"));Rect(t,4,1,4,10,Hex("FFD978"));Rect(t,3,3,6,6,Color.white);Flash=Finish(t,new Vector2(.5f,.5f),24);
            t=Texture(32,16);for(int y=0;y<16;y++)for(int x=0;x<32;x++) { float d=Mathf.Pow((x-15.5f)/15.5f,2)+Mathf.Pow((y-7.5f)/7.5f,2); if(d<1)t.SetPixel(x,y,new Color(0,0,0,(1-d)*.32f)); } Shadow=Finish(t,new Vector2(.5f,.5f),28);
            t=Texture(12,16);Rect(t,4,6,4,9,Hex("FFB95D"));Rect(t,4,1,4,3,Hex("FFB95D"));Marker=Finish(t,new Vector2(.5f,0),25);
        }
        public static Sprite WeaponSprite279(ItemKind kind){Initialize();if(BlueWeapons30.IsGun(kind))return BlueWeaponsArt30.Sprite(kind);return kind==ItemKind.MP5?MP5:kind==ItemKind.SKS?SKS:Rifle;}
        public static Vector3 WeaponMuzzle279(ItemKind kind)=>WeaponSocket307.Local(kind);
        static Sprite MakeMp5279(){
            var t=Texture(56,28);
            // Compact black receiver, retractable stock and short curved 9mm magazine.
            Rect(t,2,10,5,12,Ink);Rect(t,3,12,3,8,Hex("46595A"));Rect(t,6,15,13,3,Ink);Rect(t,7,16,11,1,Hex("88978D"));
            Rect(t,17,11,20,10,Ink);Rect(t,19,13,16,6,Hex("566363"));Rect(t,20,19,14,2,Hex("919C92"));
            Rect(t,19,22,3,3,Ink);Rect(t,33,20,3,5,Ink);Rect(t,34,21,1,3,Hex("9FA990"));
            Rect(t,21,4,6,9,Ink);Rect(t,22,5,4,6,Hex("3C4C4B"));Rect(t,26,9,8,3,Ink);Rect(t,27,10,5,1,Hex("879486"));
            Rect(t,30,5,5,8,Ink);Rect(t,32,2,6,5,Ink);Rect(t,31,7,3,5,Hex("6A7671"));Rect(t,33,3,3,4,Hex("7F8A7B"));
            Rect(t,36,11,9,9,Ink);Rect(t,37,12,7,7,Hex("364B49"));for(int i=0;i<3;i++)Rect(t,38+i*2,13,1,5,Hex("61736A"));
            Rect(t,45,13,9,3,Ink);Rect(t,46,14,7,1,Hex("A0AB98"));Rect(t,45,16,3,7,Ink);Rect(t,46,19,1,3,Hex("8D9B8D"));
            var sprite=Finish(t,new Vector2(20f/56,14f/28),60);sprite.name="MP5Compact279";return sprite;
        }
        static Sprite MakeSks279(){
            var t=Texture(96,28);
            // Long wooden stock and fore-end, slim barrel and the game's 20-round magazine.
            Rect(t,2,8,8,13,Ink);Rect(t,4,10,4,9,Hex("915D3C"));Rect(t,8,11,20,8,Ink);Rect(t,9,12,17,6,Hex("B47E4D"));Rect(t,11,17,14,2,Hex("D0A06B"));
            Rect(t,26,10,10,9,Ink);Rect(t,28,11,6,6,Hex("A56D43"));Rect(t,24,6,9,7,Ink);Rect(t,26,7,5,4,Hex("94603B"));
            Rect(t,30,14,28,8,Ink);Rect(t,32,16,24,4,Hex("56635F"));Rect(t,34,21,19,2,Ink);Rect(t,35,20,19,1,Hex("A2AB97"));Rect(t,52,19,3,6,Ink);
            Rect(t,35,10,8,4,Ink);Rect(t,36,11,5,2,Hex("758073"));Rect(t,43,5,9,10,Ink);Rect(t,45,6,5,7,Hex("68776B"));Rect(t,49,3,7,5,Ink);Rect(t,50,4,4,3,Hex("909C88"));
            Rect(t,57,12,18,8,Ink);Rect(t,58,13,16,5,Hex("B1814F"));Rect(t,59,18,15,2,Hex("D4A56C"));for(int i=0;i<3;i++)Rect(t,62+i*4,13,1,4,Hex("855831"));
            Rect(t,74,13,20,3,Ink);Rect(t,75,14,18,1,Hex("A6AD99"));Rect(t,74,19,10,2,Ink);Rect(t,86,15,3,9,Ink);Rect(t,87,21,1,3,Hex("96A18F"));Rect(t,92,12,3,5,Ink);
            var sprite=Finish(t,new Vector2(27f/96,14f/28),62);sprite.name="SKSWoodStock279";return sprite;
        }
        static Sprite Person(bool enemy,int frame)
        {
            var t=Texture(32,40);Color coat=Hex(enemy?"96734E":"5C837D"),helmet=Hex(enemy?"A88B63":"769A8C");
            int bob=frame%2, step=frame==1?2:frame==3?-2:0;
            Rect(t,8+step,0,7,6,Ink);Rect(t,19-step,0,7,6,Ink);Rect(t,9+step,3,5,5,Hex("4C5556"));Rect(t,20-step,3,4,5,Hex("4C5556"));
            Rect(t,6,7+bob,21,12,Ink);Rect(t,8,8+bob,17,11,coat);
            Rect(t,10,9+bob,12,8,Hex("313F40"));Rect(t,11,11+bob,4,5,Hex("8D9684"));Rect(t,17,11+bob,4,5,Hex("8D9684"));
            Rect(t,3,9+bob,5,14,Ink);Rect(t,4,11+bob,4,10,Hex("535B4B"));
            Rect(t,5,19+bob,23,16,Ink);Rect(t,8,34+bob,16,3,Ink);
            Rect(t,6,23+bob,21,11,helmet);Rect(t,9,34+bob,14,2,helmet);Rect(t,8,28+bob,17,5,Hex(enemy?"BFA67A":"A3B9A0"));
            Rect(t,6,20+bob,20,7,Hex("27363D"));Rect(t,6,20+bob,10,5,Hex("C9AD85"));
            Rect(t,7,23+bob,3,2,Ink);Rect(t,5,27+bob,12,2,Ink);Rect(t,23,20+bob,3,6,Hex("637168"));
            Rect(t,24,15+bob,5,6,Ink);Rect(t,25,16+bob,3,4,coat);
            return Finish(t,new Vector2(.5f,0),27);
        }
        static Texture2D Texture(int w,int h) { var t=new Texture2D(w,h,TextureFormat.RGBA32,false);t.filterMode=FilterMode.Point;t.wrapMode=TextureWrapMode.Clamp;var clear=new Color32[w*h];t.SetPixels32(clear);return t; }
        static Sprite Finish(Texture2D t,Vector2 pivot,float ppu) {t.Apply();return Sprite.Create(t,new Rect(0,0,t.width,t.height),pivot,ppu);}
        static void Rect(Texture2D t,int x,int y,int w,int h,Color c) {for(int j=y;j<y+h&&j<t.height;j++)for(int i=x;i<x+w&&i<t.width;i++)if(i>=0&&j>=0)t.SetPixel(i,j,c);}
        public static Color Hex(string s) {ColorUtility.TryParseHtmlString("#"+s,out var c);return c;}
    }
}
