using UnityEngine;
namespace GreyHarbor {
 public sealed partial class RaidMap {
  // Design coordinates: west joins the authored repair asphalt at z130;
  // east joins the north cargo collector; north becomes the concrete work apron.
  public static float FreightAsphaltWeight2719(Vector2 world){
   var p=world+new Vector2(79,73);
   float west=Mathf.Min(CargoPointSegment2715(p,new Vector2(6,128),new Vector2(6,137)),CargoPointSegment2715(p,new Vector2(6,137),new Vector2(17,137)));
   float half=Mathf.Lerp(3.3f,3.75f,Mathf.SmoothStep(0,1,Mathf.InverseLerp(47,56,p.x)));
   float east=CargoPointSegment2715(p,new Vector2(17,137),new Vector2(109,137));
   float south=Mathf.Max(1-Mathf.SmoothStep(0,1,Mathf.InverseLerp(3.10f,3.50f,west)),1-Mathf.SmoothStep(0,1,Mathf.InverseLerp(half-.2f,half+.2f,east)));
   float north=(1-Mathf.SmoothStep(0,1,Mathf.InverseLerp(3.10f,3.50f,Mathf.Abs(p.x-17))))*Mathf.SmoothStep(0,1,Mathf.InverseLerp(133,137,p.y))*(1-Mathf.SmoothStep(0,1,Mathf.InverseLerp(150,158,p.y)));
   float fadeWest=Mathf.SmoothStep(0,1,Mathf.InverseLerp(128,130.1f,p.y));
   float fadeEast=1-Mathf.SmoothStep(0,1,Mathf.InverseLerp(101,109,p.x));
   return Mathf.Clamp01(Mathf.Max(south,north)*fadeWest*fadeEast);
  }
  Color FreightAsphaltColor2719(Vector2 world){
   var p=world+new Vector2(79,73);var c=FreightPigment2718("4C5753");
   float broad=FreightNoise2717(p.x,p.y,.45f,37)-.5f,grain=FreightNoise2717(p.x,p.y,12.5f,53)-.5f;
   c.r+=broad*.055f+grain*.11f;c.g+=broad*.047f+grain*.10f;c.b+=broad*.047f+grain*.09f;
   float patch=FreightNoise2717(p.x,p.y,1.12f,9.5f);if(patch>.66f)c=Color.Lerp(c,FreightPigment2718("45524E"),(patch-.66f)*1.8f);
   // Worn asphalt remains a working service lane, not a new pristine boulevard.
   float track=Mathf.Min(Mathf.Abs(p.y-137-1.25f),Mathf.Abs(p.y-137+1.25f));
   float wear=(1-Mathf.SmoothStep(0,1,Mathf.InverseLerp(.18f,.62f,track)))*Mathf.InverseLerp(.35f,.70f,FreightNoise2717(p.x,p.y,1.9f,87));
   return Color.Lerp(c,FreightPigment2718("788073"),wear*.065f);
  }
  Color FreightConnectedSurface2719(Vector2 p,Color cement){
   var r=FreightGroundBounds2716;float edge=Mathf.Min(p.x-r.xMin,r.xMax-p.x,p.y-r.yMin,r.yMax-p.y);
   Color yard=Color.Lerp(FreightGroundBase2716(p),cement,Mathf.SmoothStep(0,1,Mathf.Clamp01(edge/2.5f)));
   // Do not inherit the obsolete round road cap in the middle of the loading yard.
   if(p.y+73>151&&Mathf.Abs(p.x+79-17)<7)yard=cement;
   return Color.Lerp(yard,FreightAsphaltColor2719(p),FreightAsphaltWeight2719(p));
  }
  void FinishFreightServiceRoad2719(Color32[] pixels,float[] depth,int w,int h,Rect bounds){
   if(!FreightGroundBuilt2716||IsInterior)return;
   float dx=bounds.width/w,dz=bounds.height/h;
   int x0=Mathf.Clamp(Mathf.FloorToInt((-77-bounds.xMin)/dx),0,w-1),x1=Mathf.Clamp(Mathf.CeilToInt((31-bounds.xMin)/dx),0,w-1);
   int z0=Mathf.Clamp(Mathf.FloorToInt((54.5f-bounds.yMin)/dz),0,h-1),z1=Mathf.Clamp(Mathf.CeilToInt((86-bounds.yMin)/dz),0,h-1);
   for(int z=z0;z<=z1;z++)for(int x=x0;x<=x1;x++){
    int n=z*w+x;if(pixels[n].a==0)continue;var p=new Vector2(bounds.xMin+(x+.5f)*dx,bounds.yMin+(z+.5f)*dz);
    // Replace only the old road contribution inside the cargo apron blend.
    // Its concrete, joints, chips and dirt remain the authored final pixel.
    float designX=p.x+79,designZ=p.y+73;
    if(designX>=51&&designX<=109&&designZ>=127.5f&&designZ<=137&&depth[n]>=.199f&&depth[n]<=.225f){
     float contribution=(1-CargoGroundBlend2715(p))*(1-Mathf.SmoothStep(0,1,Mathf.InverseLerp(101,109,designX)));
     Color c=pixels[n],delta=FreightAsphaltColor2719(p)-FreightPigment2718("667077");c+=delta*contribution;c.a=1;pixels[n]=c;continue;
    }
    // Preserve authored repair grit and freight oil/cracks.
    bool carrier=depth[n]>=.229f&&depth[n]<=.235f;
    // Repair soil extends beyond the finished asphalt at z130. Its old overlay
    // must not erase the short connector being finished in this version.
    bool westConnector=designX<20.5f&&designZ>=130&&designZ<=140.5f&&depth[n]<=.14f;
    if(!carrier&&!westConnector&&depth[n]>.0131f)continue;
    float amount=FreightAsphaltWeight2719(p);if(amount<=0||carrier)continue;
    pixels[n]=Color.Lerp(pixels[n],FreightAsphaltColor2719(p),amount);
   }
  }
 }
}
