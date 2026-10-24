using System.Collections.Generic;
using UnityEngine;
namespace GreyHarbor {
 public sealed class RoadDetailStamp2750 {public string Name;public Vector3 Position;public Vector2 Size;public float Priority;}
 public static class RoadMarkKit2750 {
  public static readonly string[] Names={"CenterDash","EdgeLine","StopLine","DirectionArrow","AsphaltPatch","RoadShoulder","DrainGrate"};
  public static string Pigment(string name)=>name=="CenterDash"||name=="DirectionArrow"?"BCAF78":name=="EdgeLine"||name=="StopLine"?"B5B69D":name=="AsphaltPatch"?"4D5658":name=="RoadShoulder"?"74756A":"343E3F";
  public static Vector2[] Arrow(){return new[]{new Vector2(-.12f,-1.15f),new Vector2(.12f,-1.15f),new Vector2(.12f,.36f),new Vector2(.52f,.36f),new Vector2(0,1.15f),new Vector2(-.52f,.36f),new Vector2(-.12f,.36f)};}
 }
 public sealed partial class RaidMap {
  public readonly List<RoadDetailStamp2750> RoadDetailStamps2750=new List<RoadDetailStamp2750>();
  void RoadPaint2750(string name,Vector3 center,Vector2[] polygon,string pigment,float priority){
   if(!OnHarborSurface(center))return;
   var vertices=new List<Vector3>();var indices=new List<int>();foreach(var p in polygon)vertices.Add(new Vector3(p.x,priority,p.y));
   for(int i=1;i<polygon.Length-1;i++){indices.Add(0);indices.Add(i);indices.Add(i+1);}RegisterGround137(vertices,indices,PixelArt.Hex(pigment));
   float xmin=float.MaxValue,zmin=float.MaxValue,xmax=float.MinValue,zmax=float.MinValue;foreach(var p in polygon){xmin=Mathf.Min(xmin,p.x);xmax=Mathf.Max(xmax,p.x);zmin=Mathf.Min(zmin,p.y);zmax=Mathf.Max(zmax,p.y);}
   RoadDetailStamps2750.Add(new RoadDetailStamp2750{Name=name,Position=center,Size=new Vector2(xmax-xmin,zmax-zmin),Priority=priority});
  }
  void RoadRect2750(string name,Vector3 p,Vector3 along,float width,float length,string pigment,float priority){
   var d=new Vector2(along.x,along.z).normalized;var n=new Vector2(-d.y,d.x);var c=new Vector2(p.x,p.z);RoadPaint2750(name,p,new[]{c-n*width/2-d*length/2,c+n*width/2-d*length/2,c+n*width/2+d*length/2,c-n*width/2+d*length/2},pigment,priority);
  }
  bool RoadJunction2750(Vector3 p,HarborRoadRecord2717 owner){
   foreach(var road in HarborRoadRecords2717){if(road==owner)continue;for(int i=1;i<road.Points.Length;i++){
    var a=road.Points[i-1];var d=road.Points[i]-a;float t=Mathf.Clamp01(Vector3.Dot(p-a,d)/Mathf.Max(.0001f,d.sqrMagnitude));if(Vector3.Distance(p,a+d*t)<road.Width*.5f+1.2f)return true;
   }}return false;
  }
  void RoadWornLine2750(string name,Vector3 p,Vector3 along,float width,float length,string pigment,int seed){
   // Separate short paint islands bake into the shared ground; chipped ends never need a decal renderer.
   float cell=length/4;for(int n=0;n<4;n++){if((seed+n*7)%11==0)continue;float span=cell*(.65f+.07f*((seed+n)%4));RoadRect2750(name,p+along*(-length*.5f+cell*(n+.5f)),along,width,span,pigment,.48f);}
  }
  void RoadPatch2750(Vector3 p,Vector3 along,float width,int seed){
   var d=new Vector2(along.x,along.z);var n=new Vector2(-d.y,d.x);var c=new Vector2(p.x,p.z);float half=width*.5f;
   RoadPaint2750("AsphaltPatch",p,new[]{c-n*half-d*.46f,c-n*(half*.86f)+d*.63f,c+n*(half*.7f)+d*.68f,c+n*half+d*.41f,c+n*(half*.92f)-d*.57f,c-n*(half*.57f)-d*.63f},"4D5658",.42f);
   for(int i=0;i<9;i++){float x=Mathf.Sin(seed*11+i*7)*width*.34f,z=Mathf.Cos(seed*17+i*3)*.43f;RoadRect2750("PatchAggregate",p+new Vector3(n.x*x+d.x*z,0,n.y*x+d.y*z),along,.055f,.075f,i%2==0?"748078":"3F4A4B",.43f);}
  }
  void RoadArrow2750(Vector3 p,Vector3 along,int seed){
   var d=new Vector2(along.x,along.z);var n=new Vector2(-d.y,d.x);var c=new Vector2(p.x,p.z);
   // Use three convex pieces rather than a fan of a concave outline.
   RoadRect2750("DirectionArrow",p-along*.4f,along,.22f,1.5f,"BCAF78",.48f);
   RoadPaint2750("DirectionArrow",p,new[]{c+n*.52f+d*.36f,c-n*.52f+d*.36f,c+d*1.15f},"BCAF78",.48f);
   RoadRect2750("ArrowWear",p-along*.53f,along,.13f,.13f,"66706D",.485f);
  }
  public void BuildHarborRoadDetails2750(){
   if(RoadDetailStamps2750.Count>0)return;
   int index=0;foreach(var road in HarborRoadRecords2717){index++;for(int segment=1;segment<road.Points.Length;segment++){
    var a=road.Points[segment-1];var b=road.Points[segment];var d=b-a;float length=d.magnitude;if(length<3)continue;d/=length;var n=new Vector3(-d.z,0,d.x);
    for(float s=3.8f;s<length-3;s+=6){var p=a+d*s;if(RoadJunction2750(p,road))continue;
     if(road.Width>=5)RoadWornLine2750("CenterDash",p,d,.14f,2.1f,"BCAF78",index*19+(int)s);
     if(road.Width>=4.5f)foreach(int side in new[]{-1,1}){var edge=p+n*(road.Width*.5f-.32f)*side;if(!RoadJunction2750(edge,road))RoadWornLine2750("EdgeLine",edge,d,.11f,4.2f,"B5B69D",index*31+(int)s+side);}
    }
    // One understated arrow per long approach; narrow dock/service lanes have no centreline.
    if(length>13&&road.Width>=4.5f){var p=a+d*Mathf.Min(length*.55f,length-5);if(!RoadJunction2750(p,road))RoadArrow2750(p-n*(road.Width*.23f),d,index);}
    for(float s=5;s<length-3;s+=11){var p=a+d*s;if(RoadJunction2750(p,road))continue;
     var patch=p+n*Mathf.Sin(index*7+s)*road.Width*.19f;RoadPatch2750(patch,d,.9f+((index+segment)%3)*.22f,index+segment+(int)s);
     RoadRect2750("PatchAggregate",patch-d*.13f,d,.48f,.10f,"67716D",.43f);
     RoadRect2750("AsphaltCrack",p+n*(road.Width*.22f),Quaternion.Euler(0,index%2==0?23:-19,0)*d,.07f,1.3f,"3E4748",.46f);
     foreach(int side in new[]{-1,1}){var shoulder=p+n*(road.Width*.5f+.17f)*side;if(!RoadJunction2750(shoulder,road)){RoadRect2750("RoadShoulder",shoulder,d,.34f,3.1f,"74756A",.42f);RoadRect2750("ShoulderGravel",shoulder+d*.8f,d,.17f,.21f,"969489",.44f);}}
    }
   }}
   // Stop bars at the two office approaches and camp entry; never across the central container maze.
   foreach(var p in new[]{PortPoint(70,142),PortPoint(93,142),PortPoint(66,50)})RoadWornLine2750("StopLine",p,Vector3.right,.21f,3.4f,"B5B69D",17);
   // Flush drain strips at traffic shoulders, not raised kerbs that would obstruct movement.
   foreach(var p in new[]{PortPoint(73.1f,143.5f),PortPoint(96.1f,143.5f),PortPoint(49.1f,96),PortPoint(130.2f,111)}){
    RoadRect2750("DrainGrate",p,Vector3.forward,.34f,1.45f,"343E3F",.50f);for(int i=0;i<8;i++)RoadRect2750("DrainBar",p+Vector3.forward*(-.61f+i*.17f),Vector3.right,.07f,.30f,"8C9489",.51f);
   }
  }
 }
}
