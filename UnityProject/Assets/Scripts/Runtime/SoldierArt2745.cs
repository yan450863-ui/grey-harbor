using UnityEngine;
namespace GreyHarbor {
 // Uses the same 64 x 80 pixel grid, animation offsets and feet pivot as the existing rebels.
 // Weapons remain separate RaidActor gun sprites so aiming, recoil and muzzle placement stay consistent.
 public static class SoldierArt2745 {
  static Color C(string value)=>PixelArt.Hex(value);
  static void R(Texture2D t,int x,int y,int w,int h,string color){for(int py=y;py<y+h;py++)for(int px=x;px<x+w;px++)if(px>=0&&px<64&&py>=0&&py<80)t.SetPixel(px,py,C(color));}
  static void P(Texture2D t,int x,int y,int w,int h,string color){R(t,x-1,y-1,w+2,h+2,"172329");R(t,x,y,w,h,color);}
  public static bool IsNew(RebelKind kind)=>kind==RebelKind.Smg||kind==RebelKind.Marksman||kind==RebelKind.BlueSmg||kind==RebelKind.BlueMarksman;
  public static Texture2D CreateFrame(RebelKind kind,int frame){
   var t=RebelArt145.CreateFrame(RebelKind.Regular,frame);t.name=kind+"_"+frame;
   bool blue=kind==RebelKind.BlueSmg||kind==RebelKind.BlueMarksman;
   bool marksman=kind==RebelKind.Marksman||kind==RebelKind.BlueMarksman;
   int b=frame%2;
   string uniform=blue?"516B86":"68734F",light=blue?"88A5BD":"9CA77A",plate=blue?"344D66":"455B3D",pouch=blue?"587A95":"7E885A",trim=blue?"B1C9DA":"B8BE88";
   string[] from={"82795E","A29972","556449","8A8C62","B0AC7D","939776","8E8966","A5A078","B2AB81","646B52","818666","3B4A43","344641"};
   string[] to={uniform,light,plate,pouch,trim,light,uniform,light,light,plate,pouch,plate,blue?"2D4657":"344A35"};
   for(int y=0;y<80;y++)for(int x=0;x<64;x++){Color pixel=t.GetPixel(x,y);for(int i=0;i<from.Length;i++)if(pixel==C(from[i])){t.SetPixel(x,y,C(to[i]));break;}}
   if(!marksman){
    // Light harness and three slim straight magazines distinguish an MP5 patrol soldier.
    P(t,19,25+b,26,18,plate);R(t,21,39+b,22,3,pouch);
    foreach(int x in new[]{20,29,38}){P(t,x,26+b,5,11,pouch);R(t,x+1,33+b,3,2,trim);}
    R(t,21,41+b,3,6,light);R(t,40,40+b,3,7,light);
    // Low-profile helmet, ear protectors and short boom microphone, no heavy shoulder plates.
    P(t,18,66+b,26,7,uniform);R(t,21,70+b,20,2,light);
    P(t,12,53+b,5,11,plate);R(t,13,60+b,3,2,pouch);
    P(t,44,53+b,6,11,plate);R(t,45,60+b,4,2,pouch);
    R(t,13,49+b,2,7,"24333A");R(t,13,49+b,10,2,"24333A");R(t,20,49+b,3,2,"BAC8BE");
    // Visible quality armband follows the body palette rather than relying on a global tint.
    R(t,48,33+b,7,4,blue?"6DA9D2":"A9B76E");R(t,50,34+b,3,2,trim);
   }else{
    // Folded camouflage shoulder cape and a narrow chest rig keep this distinct from the shield soldier.
    P(t,11,35+b,42,10,pouch);R(t,13,36+b,5,5,plate);R(t,18,39+b,8,3,light);R(t,31,36+b,6,4,plate);R(t,40,39+b,9,3,light);
    P(t,20,25+b,24,10,plate);P(t,21,26+b,10,6,pouch);P(t,34,26+b,8,6,pouch);R(t,22,30+b,8,1,trim);
    // Distinct fabric helmet cover, high-contrast goggles and compact rangefinder on the shoulder.
    P(t,17,66+b,29,8,pouch);R(t,19,70+b,5,3,plate);R(t,29,68+b,8,3,light);R(t,40,71+b,5,2,plate);
    R(t,17,59+b,30,5,pouch);R(t,20,59+b,7,2,plate);R(t,35,61+b,8,2,light);
    P(t,16,49+b,28,10,"253841");R(t,19,54+b,9,3,blue?"92BAD2":"A5B69B");R(t,31,54+b,10,3,blue?"92BAD2":"A5B69B");R(t,19,56+b,6,1,"D1D8BA");
    P(t,48,30+b,7,12,plate);R(t,49,38+b,5,2,light);R(t,50,40+b,3,2,"AFC8C8");
    // Rolled shooting mat and sling; these are body equipment, the long SKS is a separate gun sprite.
    P(t,9,23+b,7,10,pouch);R(t,10,29+b,5,2,light);R(t,15,29+b,2,13,plate);
    R(t,48,24+b,7,4,blue?"6DA9D2":"A9B76E");R(t,50,25+b,3,2,trim);
   }
   t.Apply();return t;
  }
 }
}
