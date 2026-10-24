using UnityEngine;
using System.Collections.Generic;
namespace GreyHarbor {
 [System.Serializable] public sealed class GroundSurfaceRegion {
  public Rect Area;public GroundFinish Finish;public float Elevation;
  public GroundSurfaceRegion(Rect r,GroundFinish f,float elevation){Area=r;Finish=f;Elevation=elevation;}
 }
 public sealed class GroundSurfaceRegions:MonoBehaviour {public List<GroundSurfaceRegion> Regions=new List<GroundSurfaceRegion>();}
}
