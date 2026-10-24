using UnityEngine;
namespace GreyHarbor {
 public sealed partial class RaidMap {
  // Expand the wings while retaining the five-metre spine and cross passage.
  public static Vector3 MainPoint273(Vector3 p){
   if(Mathf.Abs(p.x)>2.6f)p.x=Mathf.Sign(p.x)*(2.6f+(Mathf.Abs(p.x)-2.6f)*((MainNetHalfX276-2.6f)/10.69f));
   if(Mathf.Abs(p.z)>1.8f)p.z=Mathf.Sign(p.z)*(1.8f+(Mathf.Abs(p.z)-1.8f)*((MainNetHalfZ276-1.8f)/13.99f));
   return p;
  }
  public static Rect MainRect273(Rect r){
   var a=MainPoint273(new Vector3(r.xMin,0,r.yMin));var b=MainPoint273(new Vector3(r.xMax,0,r.yMax));
   return Rect.MinMaxRect(a.x,a.z,b.x,b.z);
  }
  // Legacy partition spans expand, but wall thickness remains twenty centimetres.
  void MainPartition273(float x,float z,float w,float d){
   var r=MainRect273(new Rect(x-w*.5f,z-d*.5f,w,d));var p=MainPoint273(new Vector3(x,0,z));
   RepairMainWall210(p.x,p.z,w>d?r.width:w,w>d?d:r.height);
  }
 }
}
