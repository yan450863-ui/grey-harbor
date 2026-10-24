using System.Collections.Generic;
using UnityEngine;
namespace GreyHarbor {
 public sealed class FreightGroundRegion2716 {public string Name,Finish;public Rect Area;}
 public sealed partial class RaidMap {
  public readonly List<FreightGroundRegion2716> FreightGroundRegions2716=new List<FreightGroundRegion2716>();
  public int FreightGroundTriangleCount2716;public bool FreightGroundBuilt2716;
  public static readonly Rect FreightGroundBounds2716=new Rect(-74,60,45,42);
  float FreightGroundBlend2716(Vector2 p){float distance=float.MaxValue;foreach(var road in freightLocalRoads2718??HarborRoadShapes){if(InPolygon(p,road))return 0;for(int i=0;i<road.Length;i++)distance=Mathf.Min(distance,CargoPointSegment2715(p,road[i],road[(i+1)%road.Length]));}var r=FreightGroundBounds2716;float edge=Mathf.Min(p.x-r.xMin,r.xMax-p.x,p.y-r.yMin,r.yMax-p.y);return Mathf.SmoothStep(0,1,Mathf.Clamp01(Mathf.Min(edge/2.5f,distance/3.6f)));}
  Color FreightGroundBase2716(Vector2 p){Color c=FreightPigment2718("A0A7A4");foreach(var s in HarborSurfaces)if(InPolygon(p,s.Points))c=FreightPigment2718(s.Color);foreach(var r in freightLocalRoads2718??HarborRoadShapes)if(InPolygon(p,r))return FreightPigment2718("667077");return c;}
  void FreightGroundPaint2716(Vector2[] polygon,Color color,float priority,bool fade){var clipped=new List<Vector2>(polygon);for(int side=0;side<4&&clipped.Count>0;side++)clipped=CargoGroundClip2715(clipped,FreightGroundBounds2716,side);if(clipped.Count<3)return;float area=0;Vector2 center=Vector2.zero;for(int i=0;i<clipped.Count;i++){center+=clipped[i];area+=Cross(clipped[i],clipped[(i+1)%clipped.Count]);}if(Mathf.Abs(area)<.000001f)return;center/=clipped.Count;if(!OnHarborSurface(new Vector3(center.x,0,center.y)))return;var v=new List<Vector3>();var ix=new List<int>();foreach(var p in clipped)v.Add(new Vector3(p.x,priority,p.y));for(int i=1;i<clipped.Count-1;i++){ix.Add(0);ix.Add(i);ix.Add(i+1);}Color ink=fade?Color.Lerp(FreightSurfaceColor2717(center),color,FreightGroundBlend2716(center)):color;RegisterGround137(v,ix,ink);FreightGroundTriangleCount2716+=ix.Count/3;}
  void FreightGroundLine2716(Vector2 a,Vector2 b,float width,string hex,float depth){var d=b-a;var n=new Vector2(-d.y,d.x).normalized*width*.5f;int count=Mathf.Max(1,Mathf.CeilToInt(d.magnitude/.6f));for(int i=0;i<count;i++){var p=Vector2.Lerp(a,b,(float)i/count);var q=Vector2.Lerp(a,b,(float)(i+1)/count);FreightGroundPaint2716(new[]{p+n,q+n,q-n,p-n},FreightPigment2718(hex),depth,true);}}
  public void BuildFreightGround2716(){if(FreightGroundBuilt2716)return;FreightGroundBuilt2716=true;BuildFreightSurface2717();}
 }
}
