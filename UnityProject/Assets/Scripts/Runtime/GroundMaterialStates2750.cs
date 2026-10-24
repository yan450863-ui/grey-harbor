using UnityEngine;
namespace GreyHarbor {
 public static class GroundMaterialStates2750 {
  public static Color Palette(GroundFinish type){switch(type){case GroundFinish.Soil:return PixelArt.Hex("736B59");case GroundFinish.Gravel:return PixelArt.Hex("75786C");case GroundFinish.PatchedConcrete:return PixelArt.Hex("82877E");case GroundFinish.Asphalt:return PixelArt.Hex("46524F");case GroundFinish.Steel:return PixelArt.Hex("586F67");default:return PixelArt.Hex("969C92");}}
  static float Hash(int x,int z,int seed){unchecked{uint h=(uint)(x*374761393+z*668265263+seed*1442695041);h=(h^(h>>13))*1274126177;h^=h>>16;return (h&65535)/65535f;}}
  public static Color Sample(GroundFinish type,float x,float z,int state=0){
   var c=Palette(type);float broad=Mathf.PerlinNoise(x*.37f+19,z*.37f+71)-.5f;
   float grain=Hash(Mathf.FloorToInt(x*15),Mathf.FloorToInt(z*15),31)-.5f;
   float coarse=Hash(Mathf.FloorToInt(x*7),Mathf.FloorToInt(z*7),73);
   float shade=broad*.055f+grain*(type==GroundFinish.Gravel?.14f:type==GroundFinish.Asphalt?.095f:.055f);
   if(type==GroundFinish.Gravel){if(coarse>.87f)shade+=.09f;if(coarse<.15f)shade-=.07f;}
   if(type==GroundFinish.Soil){float rut=Mathf.Abs(Mathf.Repeat(x+Mathf.Sin(z*.7f)*.075f,2.9f)-1.45f);float broken=Mathf.SmoothStep(0,1,Mathf.InverseLerp(.53f,.71f,Mathf.PerlinNoise(x*.43f+77,z*.43f+41)));shade-=(1-Mathf.SmoothStep(0,1,Mathf.InverseLerp(.022f,.10f,rut)))*broken*.055f;shade+=broad*.035f;}
   if(type==GroundFinish.PatchedConcrete){float repair=Mathf.PerlinNoise(x*.69f+173,z*.69f+33);if(repair>.60f)shade+=(repair-.60f)*.24f;else if(repair<.27f)shade-=(.27f-repair)*.19f;}
   bool cement=type==GroundFinish.Concrete||type==GroundFinish.PatchedConcrete;
   if(cement){float j=Mathf.Min(Mathf.Abs(Mathf.Repeat(x+2.3f,5.8f)-2.9f),Mathf.Abs(Mathf.Repeat(z+1.7f,8.4f)-4.2f));shade-=(1-Mathf.SmoothStep(0,1,Mathf.InverseLerp(.018f,.052f,j)))*.065f;}
   if(cement||type==GroundFinish.Asphalt){int cx=Mathf.FloorToInt(x/6.7f),cz=Mathf.FloorToInt(z/6.7f);float seed=Hash(cx,cz,57),lx=Mathf.Repeat(x,6.7f),lz=Mathf.Repeat(z,6.7f);float distance=Mathf.Abs(lz-(1.1f+seed*3.4f+lx*(seed-.5f)*.7f+Mathf.Sin(lx*3.1f+seed*8)*.075f));if(seed>.68f&&lx>.6f&&lx<4.6f)shade-=(1-Mathf.SmoothStep(0,1,Mathf.InverseLerp(.015f,.043f,distance)))*.12f;}
   if(type==GroundFinish.Steel){float joint=Mathf.Min(Mathf.Repeat(x,1.6f),Mathf.Repeat(z,1.2f));shade-=joint<.024f?.11f:0;float tread=Mathf.Abs(Mathf.Repeat(x+z,.24f)-.12f);shade+=tread<.014f?.055f:0;}
   float oil=Mathf.InverseLerp(.72f,.85f,Mathf.PerlinNoise(x*.53f+317,z*.53f+73));shade-=oil*(type==GroundFinish.PatchedConcrete?.07f:.035f);
   if(state==1)shade-=Mathf.SmoothStep(0,1,Mathf.PerlinNoise(x*.7f+3,z*.7f+17))*.10f;
   if(state==2){float patch=Mathf.PerlinNoise(x*1.3f+53,z*1.3f+13);shade+=(patch>.55f?.06f:-.025f);}
   c.r+=shade;c.g+=shade;c.b+=shade*.93f;return c;
  }
  public static Color Transition(GroundFinish a,GroundFinish b,float x,float z){float irregular=(Mathf.PerlinNoise(x*1.4f+29,z*1.4f+61)-.5f)*.30f;float blend=Mathf.SmoothStep(0,1,Mathf.InverseLerp(1.55f,2.45f,x+irregular));return Color.Lerp(Sample(a,x,z),Sample(b,x,z),blend);}
 }
}
