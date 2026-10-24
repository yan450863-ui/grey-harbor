using System.Collections.Generic;
using UnityEngine;
namespace GreyHarbor {
 public sealed partial class RaidUI {
  internal static Rect MiniMapFrame326=>new Rect(28,52,214,194);
  internal static Rect MiniMapViewport326=>new Rect(30,54,210,190);
  internal const int SoundMarkerMaximum326=8;
  internal const float SoundIconSize326=20,SoundFadeSeconds326=.4f;
  internal struct SoundMarker326 {public int Kind,Bucket;public Vector3 Direction;public float Distance,Remaining,Duration;public Vector2 Point;}
  readonly List<SoundMarker326> soundMarkers326=new List<SoundMarker326>();
  readonly Dictionary<int,int> soundBuckets326=new Dictionary<int,int>();
  static readonly Texture2D[] soundIcons326=new Texture2D[3];
  internal static float SoundFade326(float remaining)=>Mathf.Clamp01(remaining/SoundFadeSeconds326);
  internal static int NoiseSegments326(float noise)=>Mathf.Clamp(Mathf.CeilToInt(Mathf.Clamp01(noise)*8-.000001f),0,8);
  internal static Rect NoiseSegmentRect326(int index)=>new Rect(130+index*32,879,29,9);

  internal List<SoundMarker326> SoundMarkers326(MiniMapProjection projection,Rect frame){
   soundMarkers326.Clear();soundBuckets326.Clear();
   foreach(var sound in G.Sound326.Heard){
    if(sound.Remaining<=0||sound.Direction.sqrMagnitude<.000001f)continue;
    int kind=Mathf.Clamp((int)sound.Kind,0,2);Vector2 projected=projection.Point(sound.Direction)-projection.Point(Vector3.zero);
    if(projected.sqrMagnitude<.000001f)continue;
    int bucket=(Mathf.RoundToInt(Mathf.Atan2(projected.y,projected.x)*Mathf.Rad2Deg/45)+8)%8,key=kind*8+bucket;
    var marker=new SoundMarker326{Kind=kind,Bucket=bucket,Direction=sound.Direction,Distance=sound.Distance,Remaining=sound.Remaining,Duration=sound.Duration};
    if(soundBuckets326.TryGetValue(key,out int index)){
     var existing=soundMarkers326[index];if(marker.Distance<existing.Distance||Mathf.Abs(marker.Distance-existing.Distance)<.001f&&marker.Remaining>existing.Remaining)soundMarkers326[index]=marker;
    }else {soundBuckets326[key]=soundMarkers326.Count;soundMarkers326.Add(marker);}
   }
   // Prefer the closest current warning when several different sound types compete for space.
   soundMarkers326.Sort((a,b)=>{int distance=a.Distance.CompareTo(b.Distance);return distance!=0?distance:b.Remaining.CompareTo(a.Remaining);});
   if(soundMarkers326.Count>SoundMarkerMaximum326)soundMarkers326.RemoveRange(SoundMarkerMaximum326,soundMarkers326.Count-SoundMarkerMaximum326);
   for(int i=0;i<soundMarkers326.Count;i++){var marker=soundMarkers326[i];marker.Point=SoundRules326.EdgePoint(projection,marker.Direction,frame,marker.Kind);soundMarkers326[i]=marker;}
   return soundMarkers326;
  }

  void DrawHeardSounds326(MiniMapProjection projection,Rect frame){
   foreach(var marker in SoundMarkers326(projection,frame)){
    Color prior=GUI.color,tint=SoundRules326.DistanceColor(marker.Distance);
    try{GUI.color=new Color(tint.r,tint.g,tint.b,tint.a*SoundFade326(marker.Remaining));GUI.DrawTexture(new Rect(marker.Point.x-SoundIconSize326/2,marker.Point.y-SoundIconSize326/2,SoundIconSize326,SoundIconSize326),SoundIcon326(marker.Kind),ScaleMode.StretchToFill,true);}finally{GUI.color=prior;}
   }
  }
  void NoiseHUD326(){
   if(G.Phase!=GamePhase.Raid)return;int active=NoiseSegments326(G.Sound326.OwnNoise);
   Label(50,876,76,16,"自身噪声",11,mint);
   for(int i=0;i<8;i++)Fill(NoiseSegmentRect326(i),i<active?mint:new Color(mint.r*.20f,mint.g*.20f,mint.b*.20f,.80f));
  }

  // Vector silhouettes sampled at4x coverage, then drawn with bilinear filtering.
  internal const int SoundIconPixels327=128;
  internal static Texture2D SoundIcon326(int kind){
   kind=Mathf.Clamp(kind,0,2);if(soundIcons326[kind]!=null)return soundIcons326[kind];
   int size=SoundIconPixels327;var pixels=new Color32[size*size];
   for(int y=0;y<size;y++)for(int x=0;x<size;x++){
    int coverage=0;for(int sy=0;sy<4;sy++)for(int sx=0;sx<4;sx++)if(SoundInside327(kind,(x+(sx+.5f)/4)*32/size,(y+(sy+.5f)/4)*32/size))coverage++;
    // White transparent texels avoid dark halos during straight-alpha interpolation.
    pixels[(size-1-y)*size+x]=new Color32(255,255,255,(byte)Mathf.RoundToInt(coverage*255f/16));
   }
   var texture=new Texture2D(size,size,TextureFormat.RGBA32,true){name=kind==0?"Sound327_Footsteps":kind==1?"Sound327_TwoBullets":"Sound327_CreatureClaws",filterMode=FilterMode.Trilinear,wrapMode=TextureWrapMode.Clamp};texture.SetPixels32(pixels);texture.Apply(true,false);soundIcons326[kind]=texture;return texture;
  }
  static bool SoundInside327(int kind,float x,float y){
   if(kind==0)return SoundEllipse327(x,y,9,8,3,5)||SoundCapsule327(x,y,9,13,8,19,2.5f)||SoundEllipse327(x,y,7,25,3,3)||SoundEllipse327(x,y,24,5,3,4)||SoundCapsule327(x,y,23,10,21,16,2.5f)||SoundEllipse327(x,y,19,22,3,3);
   if(kind==1)return SoundBullet327(x,y,5,3)||SoundBullet327(x,y,19,6);
   return SoundCapsule327(x,y,6,4,4,11,1.5f)||SoundCapsule327(x,y,4,11,7,29,1.5f)||SoundCapsule327(x,y,17,2,13,12,1.5f)||SoundCapsule327(x,y,13,12,16,30,1.5f)||SoundCapsule327(x,y,28,4,23,14,1.5f)||SoundCapsule327(x,y,23,14,26,28,1.5f);
  }
  static bool SoundEllipse327(float x,float y,float cx,float cy,float rx,float ry){x=(x-cx)/rx;y=(y-cy)/ry;return x*x+y*y<=1;}
  static bool SoundCapsule327(float x,float y,float ax,float ay,float bx,float by,float radius){var point=new Vector2(x-ax,y-ay);var line=new Vector2(bx-ax,by-ay);float t=Mathf.Clamp01(Vector2.Dot(point,line)/line.sqrMagnitude);return (point-line*t).sqrMagnitude<=radius*radius;}
  static bool SoundBullet327(float x,float y,float ox,float oy){float dy=y-oy;return dy>=0&&dy<6&&Mathf.Abs(x-ox-3)<=Mathf.Min(3,dy*.6f)||x>=ox&&x<=ox+7&&dy>=6&&dy<=21||x>=ox-1&&x<=ox+8&&dy>=23&&dy<=26;}

 }
}
