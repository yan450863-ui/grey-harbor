using System.Collections.Generic;
using UnityEngine;
namespace GreyHarbor {
 public sealed partial class RaidMap {
  public const int OutdoorGroundRevision2750=2750;
  public readonly List<GroundSurfaceRegion> OutdoorGroundRegions2750=new List<GroundSurfaceRegion>();
  void PrepareOutdoorGroundRegions2750(){
   if(OutdoorGroundRegions2750.Count>0||!HarborWhitebox||IsInterior)return;
   // Purpose follows the approved harbour drawing; the old courtyard is earth,
   // working freight/container aprons are repaired cement, office fronts cement.
   OutdoorGroundRegions2750.Add(new GroundSurfaceRegion(new Rect(-28,-19,88,83),GroundFinish.PatchedConcrete,0));
   OutdoorGroundRegions2750.Add(new GroundSurfaceRegion(new Rect(-74,64,44,41),GroundFinish.PatchedConcrete,0));
   OutdoorGroundRegions2750.Add(new GroundSurfaceRegion(new Rect(-27,65,58,38),GroundFinish.Concrete,0));
   OutdoorGroundRegions2750.Add(new GroundSurfaceRegion(new Rect(31,64,43,39),GroundFinish.PatchedConcrete,0));
   OutdoorGroundRegions2750.Add(new GroundSurfaceRegion(new Rect(-24,-65,64,37),GroundFinish.Concrete,0));
   OutdoorGroundRegions2750.Add(new GroundSurfaceRegion(new Rect(-71,-65,41,42),GroundFinish.Soil,0));
   var authored=new List<GroundSurfaceRegion>();foreach(var registry in Root.GetComponentsInChildren<GroundSurfaceRegions>())authored.AddRange(registry.Regions);
   authored.Sort((a,b)=>a.Elevation.CompareTo(b.Elevation));OutdoorGroundRegions2750.AddRange(authored);
  }
  public GroundFinish OutdoorGroundType2750(Vector2 p){PrepareOutdoorGroundRegions2750();if(OutdoorRoadWeight2749(p)>.55f)return GroundFinish.Asphalt;for(int i=OutdoorGroundRegions2750.Count-1;i>=0;i--)if(OutdoorGroundRegions2750[i].Area.Contains(p))return OutdoorGroundRegions2750[i].Finish;return GroundFinish.Gravel;}
  void RasterOutdoorTypes2750(byte[] types,byte[] previous,byte[] blend,int width,int height,Rect bounds){
   PrepareOutdoorGroundRegions2750();for(int i=0;i<types.Length;i++){types[i]=(byte)GroundFinish.Gravel;previous[i]=types[i];blend[i]=255;}
   float dx=bounds.width/width,dz=bounds.height/height;
   foreach(var region in OutdoorGroundRegions2750){var a=region.Area;int x0=Mathf.Clamp(Mathf.FloorToInt((a.xMin-bounds.xMin)/dx),0,width-1),x1=Mathf.Clamp(Mathf.CeilToInt((a.xMax-bounds.xMin)/dx),0,width-1),z0=Mathf.Clamp(Mathf.FloorToInt((a.yMin-bounds.yMin)/dz),0,height-1),z1=Mathf.Clamp(Mathf.CeilToInt((a.yMax-bounds.yMin)/dz),0,height-1);
    for(int z=z0;z<=z1;z++)for(int x=x0;x<=x1;x++){float px=bounds.xMin+(x+.5f)*dx,pz=bounds.yMin+(z+.5f)*dz;if(!a.Contains(new Vector2(px,pz)))continue;int n=z*width+x;previous[n]=types[n];types[n]=(byte)region.Finish;float edge=Mathf.Min(px-a.xMin,a.xMax-px,pz-a.yMin,a.yMax-pz);float uneven=(Mathf.PerlinNoise(px*1.4f+29,pz*1.4f+61)-.5f)*.22f;blend[n]=(byte)Mathf.RoundToInt(Mathf.SmoothStep(0,1,Mathf.Clamp01((edge+uneven)/.70f))*255);}
   }
  }
 }
}
