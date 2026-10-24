using UnityEngine;
using System.Collections.Generic;
namespace GreyHarbor {
 public sealed partial class GroundSurfaceArt {
  readonly List<GroundSurfaceRegion> regions=new List<GroundSurfaceRegion>();
  Rect activeRegion;float elevation;bool clipping;
  Vector3 SurfacePoint(Vector3 p){if(clipping){p.x=Mathf.Clamp(p.x,activeRegion.xMin,activeRegion.xMax);p.z=Mathf.Clamp(p.z,activeRegion.yMin,activeRegion.yMax);p.y+=elevation;}return p;}
  public void Region(Rect r,GroundFinish finish,float heightOffset=0){
   if(r.width<=.08f||r.height<=.08f)throw new System.ArgumentOutOfRangeException(nameof(r));
   activeRegion=r;elevation=heightOffset;clipping=true;regions.Add(new GroundSurfaceRegion(r,finish,heightOffset));
   try{switch(finish){case GroundFinish.Asphalt:AsphaltSurface(r);break;case GroundFinish.Gravel:GravelSurface(r);break;case GroundFinish.Soil:SoilSurface(r);break;case GroundFinish.Terrazzo:TerrazzoSurface(r);break;case GroundFinish.Epoxy:EpoxySurface(r);break;case GroundFinish.Steel:SteelSurface(r);break;default:OriginalRegion(r,finish);break;}}
   finally{clipping=false;elevation=0;}
  }
  float Area(Rect r)=>r.width*r.height;
  void Speckles(Rect r,string group,string a,string b,int density,float small,float large,float y){
   for(int i=0;i<Area(r)*density;i++){float x=R(r.xMin,r.xMax),z=R(r.yMin,r.yMax),s=R(small,large);Quad(group,i%2==0?a:b,x,z,s,s*R(.6f,1.25f),y);}
  }
  void Ribbon(string group,string color,Vector3 a,Vector3 b,float width){var side=Vector3.Cross(Vector3.up,(b-a).normalized)*width*.5f;Tri(group,color,a-side,b-side,b+side);Tri(group,color,a-side,b+side,a+side);}
  void AsphaltSurface(Rect r){
   Quad("AsphaltBase","4C5753",r.center.x,r.center.y,r.width,r.height,.016f);
   for(int i=0;i<Area(r)*.18f;i++)Blotch("AsphaltWeathering",i%2==0?"525C56":"46534E",r,R(r.xMin,r.xMax),R(r.yMin,r.yMax),R(.3f,.9f),.017f);
   Speckles(r,"AsphaltGrit","788073","374940",16,.015f,.045f,.019f);
   for(int i=0;i<Area(r)/24;i++)Blotch("AsphaltRepair","45524E",r,R(r.xMin,r.xMax),R(r.yMin,r.yMax),R(.35f,.64f),.020f);
  }
  void GravelSurface(Rect r){
   Quad("GravelSoilBase","757662",r.center.x,r.center.y,r.width,r.height,.016f);
   for(int i=0;i<Area(r)*.35f;i++)Blotch("CompactedGravelDust","80806B",r,R(r.xMin,r.xMax),R(r.yMin,r.yMax),R(.28f,.65f),.017f);
   for(int i=0;i<Area(r)*20;i++){
    float x=R(r.xMin,r.xMax),z=R(r.yMin,r.yMax),s=R(.025f,.10f);var rim=new List<Vector3>();
    for(int j=0;j<5;j++){float a=j*Mathf.PI*2/5;rim.Add(new Vector3(x+Mathf.Cos(a)*s,.019f,z+Mathf.Sin(a)*s*.78f));}
    Polygon("EmbeddedGravel",i%3==0?"989681":i%3==1?"616F61":"858975",rim);
    Tri("GravelFacet","A2A18B",new Vector3(x,.021f,z),rim[1]+Vector3.up*.002f,rim[0]+Vector3.up*.002f);
   }
   Speckles(r,"GravelFineGrit","69735F","94937C",8,.009f,.026f,.022f);
  }
  void SoilSurface(Rect r){
   Quad("CompactedSoil","7D785E",r.center.x,r.center.y,r.width,r.height,.016f);
   for(int i=0;i<Area(r)*.4f;i++)Blotch("SoilVariation",i%3==0?"686D54":i%3==1?"898267":"77755A",r,R(r.xMin,r.xMax),R(r.yMin,r.yMax),R(.25f,.85f),.017f);
   Speckles(r,"SoilGranules","999073","696C51",14,.01f,.04f,.019f);
   // Shallow broken ruts stay at ground height and create no collision or slowing.
   for(int i=0;i<Area(r)/10;i++){
    float x=R(r.xMin+.15f,r.xMax-.15f),z=R(r.yMin,r.yMax),len=R(.3f,.8f);
    foreach(float side in new[]{-.16f,.16f}){Ribbon("ShallowSoilRuts","676B51",new Vector3(x+side,.021f,z),new Vector3(x+side+.06f,.021f,z+len),.065f);}
   }
  }
  void TerrazzoSurface(Rect r){
   Quad("TerrazzoBase","A4A795",r.center.x,r.center.y,r.width,r.height,.016f);
   for(int i=0;i<Area(r)*.18f;i++)Blotch("TerrazzoMottling","9BA28E",r,R(r.xMin,r.xMax),R(r.yMin,r.yMax),R(.3f,.7f),.017f);
   for(int i=0;i<Area(r)*65;i++){
    float x=R(r.xMin,r.xMax),z=R(r.yMin,r.yMax),s=R(.009f,.041f);string color=i%4==0?"C4C0A7":i%4==1?"7A8978":i%4==2?"92917B":"B2B19A";
    Tri("TerrazzoStoneChips",color,new Vector3(x-s,.019f,z-s*.4f),new Vector3(x,.019f,z+s),new Vector3(x+s*.8f,.019f,z));
   }
   // Widely spaced divider strips, not dense wooden-looking bands.
   for(float x=r.xMin+2.6f;x<r.xMax-.12f;x+=2.6f)Quad("TerrazzoDividers","7C8977",x,r.center.y,.014f,r.height,.020f);
   for(float z=r.yMin+2.6f;z<r.yMax-.12f;z+=2.6f)Quad("TerrazzoDividers","7C8977",r.center.x,z,r.width,.014f,.020f);
  }
  void EpoxySurface(Rect r){
   Quad("EpoxyUndercoat","858A78",r.center.x,r.center.y,r.width,r.height,.015f);
   Quad("WornEpoxyPaint","677E73",r.center.x,r.center.y,r.width,r.height,.017f);
   for(int i=0;i<Area(r)*.20f;i++)Blotch("EpoxyDullPatches","6D8376",r,R(r.xMin,r.xMax),R(r.yMin,r.yMax),R(.18f,.5f),.018f);
   for(int i=0;i<Mathf.Max(1,Area(r)/10);i++){
    float x=R(r.xMin,r.xMax),z=R(r.yMin,r.yMax),s=R(.12f,.32f);
    Blotch("EpoxyFlakedEdge","536D60",r,x,z,s,.020f);Blotch("EpoxyExposedCement","929680",r,x,z,s*.72f,.021f);
   }
   for(int i=0;i<Area(r)*1.2f;i++){float x=R(r.xMin,r.xMax),z=R(r.yMin,r.yMax);Ribbon("EpoxyScratches","8B9681",new Vector3(x,.022f,z),new Vector3(x+R(.04f,.12f),.022f,z+R(.05f,.14f)),.008f);}
  }
  void SteelSurface(Rect r){
   Quad("SteelPlateBed","384F49",r.center.x,r.center.y,r.width,r.height,.015f);
   int ix=0;for(float x=r.xMin;x<r.xMax-.04f;x+=1.6f,ix++){int iz=0;for(float z=r.yMin;z<r.yMax-.04f;z+=1.2f,iz++){
    float w=Mathf.Min(1.6f,r.xMax-x)-.025f,d=Mathf.Min(1.2f,r.yMax-z)-.025f;if(w<=.025f||d<=.025f)continue;
    Quad("SteelPlates",(ix+iz)%2==0?"697B72":"62776E",x+.0125f+w/2,z+.0125f+d/2,w,d,.017f);
    if(w>.16f&&d>.16f)foreach(float px in new[]{x+.065f,x+w-.04f})foreach(float pz in new[]{z+.065f,z+d-.04f}){
     Quad("PlateFasteners","8D9781",px,pz,.036f,.036f,.022f);Quad("BoltRecess","455E51",px,pz,.014f,.014f,.023f);
    }
    for(float px=x+.17f;px<x+w-.10f;px+=.26f)for(float pz=z+.15f;pz<z+d-.09f;pz+=.24f){
     float flip=((int)((px-x)/.26f)+(int)((pz-z)/.24f))%2==0?1:-1;
     Ribbon("SteelTreadShadow","435E53",new Vector3(px-.048f,.019f,pz-.03f*flip),new Vector3(px+.048f,.019f,pz+.03f*flip),.025f);
     Ribbon("SteelRaisedTread","9BA18A",new Vector3(px-.048f,.021f,pz-.03f*flip+.008f),new Vector3(px+.048f,.021f,pz+.03f*flip+.008f),.012f);
    }
   }}
   for(int i=0;i<Area(r)*.30f;i++)Blotch("SteelRust","83705A",r,R(r.xMin,r.xMax),R(r.yMin,r.yMax),R(.04f,.14f),.024f);
  }
 }
}
