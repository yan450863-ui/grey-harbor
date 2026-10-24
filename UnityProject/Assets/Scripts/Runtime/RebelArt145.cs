using UnityEngine;
namespace GreyHarbor {
 public enum RebelKind {Regular=0,Heavy=1,BlueRegular=2,BlueHeavy=3,Smg=4,Marksman=5,BlueSmg=6,BlueMarksman=7}
 public static class RebelArt145 {
  static Sprite[,] frames;
  static Color C(string c)=>PixelArt.Hex(c);
  static void R(Texture2D t,int x,int y,int w,int h,string c){for(int yy=y;yy<y+h;yy++)for(int xx=x;xx<x+w;xx++)if(xx>=0&&xx<64&&yy>=0&&yy<80)t.SetPixel(xx,yy,C(c));}
  static void P(Texture2D t,int x,int y,int w,int h,string c){R(t,x-1,y-1,w+2,h+2,"172329");R(t,x,y,w,h,c);}
  public static Texture2D CreateFrame(RebelKind k,int f){
   if(SoldierArt2745.IsNew(k))return SoldierArt2745.CreateFrame(k,f);
   var t=new Texture2D(64,80,TextureFormat.RGBA32,false);t.name=k+"_"+f;t.filterMode=FilterMode.Point;t.wrapMode=TextureWrapMode.Clamp;t.SetPixels32(new Color32[64*80]);
   bool heavy=k!=RebelKind.Regular;int b=f%2,s=f==1?3:f==3?-3:0;
   string uniform=heavy?"505D60":"82795E",light=heavy?"738486":"A29972",plate=heavy?"40545F":"556449";
   P(t,19+s,1,10,8,"34403F");P(t,36-s,1,11,8,"34403F");R(t,20+s,7,8,5,uniform);R(t,37-s,7,8,5,uniform);
   P(t,20,10+b,26,14,uniform);R(t,29,11+b,4,11,"3D4840");R(t,23,15+b,5,4,light);R(t,37,15+b,5,4,light);
   P(t,13,23+b,36,24,uniform);P(t,17,24+b,29,23,plate);R(t,20,42+b,23,3,light);
   if(heavy){P(t,18,29+b,27,12,"617B7F");R(t,21,37+b,20,2,"A0ACA0");R(t,20,30+b,23,2,"32434A");P(t,18,23+b,8,5,"5B6C69");P(t,29,23+b,8,5,"5B6C69");P(t,40,23+b,6,5,"5B6C69");P(t,9,36+b,8,11,"6C8282");P(t,46,36+b,8,11,"6C8282");}
   else{foreach(int x in new[]{18,28,38}){P(t,x,26+b,8,10,"8A8C62");R(t,x+1,33+b,6,2,"B0AC7D");}R(t,21,38+b,4,8,"939776");R(t,39,38+b,4,8,"939776");}
   P(t,8,24+b,8,16,uniform);P(t,47,24+b,9,16,uniform);R(t,8,32+b,8,4,"A35645");R(t,10,33+b,3,2,"C69C71");P(t,9,21+b,8,6,"3B4A43");P(t,48,21+b,8,6,"3B4A43");
   // Large helmet, simple neck/arms and no dangling cloth keep walking animation manageable.
   P(t,16,44+b,31,23,heavy?"4F646B":"8E8966");P(t,19,66+b,26,8,heavy?"64797C":"A5A078");R(t,23,74+b,18,2,light);
   R(t,17,59+b,30,7,heavy?"91A2A0":"B2AB81");R(t,20,66+b,8,4,uniform);R(t,35,67+b,7,4,light);
   P(t,16,48+b,28,11,"253841");R(t,18,54+b,24,3,heavy?"B59E65":"839C9D");R(t,20,56+b,8,1,"C6C7A8");
   if(heavy){P(t,18,44+b,26,7,"53696F");R(t,24,46+b,14,1,"A4AE9E");P(t,43,48+b,6,11,"647C7C");}
   else{P(t,17,43+b,27,8,"646B52");R(t,20,46+b,19,2,"818666");}
   R(t,14,59+b,35,3,"344641");R(t,44,59+b,4,13,plate);
   if(k==RebelKind.Heavy){
    // Recolor the old blue base before overlaying the green heavy silhouette.
    string[] blue={"505D60","738486","40545F","617B7F","A0ACA0","32434A","5B6C69","6C8282","4F646B","64797C","91A2A0","53696F","647C7C"};
    string[] green={"5F6B4B","8B9867","475C3C","647C50","AFB786","354B35","607148","718957","566E46","6B8050","98A574","596F43","708955"};
    for(int py=0;py<80;py++)for(int px=0;px<64;px++){var color=t.GetPixel(px,py);for(int ci=0;ci<blue.Length;ci++)if(color==C(blue[ci])){t.SetPixel(px,py,C(green[ci]));break;}}
    // Heavy green baseline: broad raised collar, reinforced helmet and a hand-held shield.
    P(t,18,66+b,28,8,"65734B");R(t,18,59+b,29,7,"899467");R(t,22,69+b,19,3,"A1AB78");
    P(t,17,44+b,29,9,"576342");R(t,19,49+b,24,2,"A7AD7A");
    P(t,17,28+b,30,14,"5D704C");R(t,21,38+b,22,2,"9BA77B");P(t,45,36+b,10,12,"768457");
    P(t,4,15+b,24,39,"3E513D");P(t,6,18+b,20,32,"697B4F");
    R(t,8,46+b,16,2,"A3AE7A");R(t,8,21+b,2,23,"8B9C66");R(t,21,21+b,2,23,"445B39");
    P(t,11,38+b,10,7,"293D32");R(t,13,42+b,6,1,"A5B7A0");
    R(t,8,20+b,15,3,"B6AB71");R(t,12,23+b,4,11,"566946");R(t,8,28+b,15,3,"7E905C");
    foreach(int y in new[]{23,34,45}){R(t,8,y+b,2,2,"BCC194");R(t,22,y+b,2,2,"BCC194");}
   }
   t.Apply();return t;
  }
  public static Sprite Get(RebelKind k,int f){if(k==RebelKind.BlueHeavy)k=RebelKind.Heavy;f=Mathf.Abs(f)%4;if(frames==null){frames=new Sprite[8,4];for(int type=0;type<8;type++)for(int i=0;i<4;i++){var kind=(RebelKind)type;var s=Resources.Load<Sprite>("Rebels145/"+kind+"_"+i);if(!s){var t=CreateFrame(kind,i);s=Sprite.Create(t,new Rect(0,0,64,80),new Vector2(.5f,0),55,0,SpriteMeshType.FullRect);}frames[type,i]=s;}}return frames[(int)k,f];}
 }
}
