using UnityEngine;
namespace GreyHarbor {
 // Project ground-plane positions using the live camera basis, including its in-between Q/E angles.
 public struct MiniMapProjection {
  readonly Vector3 center,right,up; readonly Vector2 screenCenter; readonly float scale;
  public readonly Rect Viewport;
  public MiniMapProjection(Rect bounds,Rect viewport,Quaternion view){
   Viewport=viewport;center=new Vector3(bounds.center.x,0,bounds.center.y);right=view*Vector3.right;up=view*Vector3.up;screenCenter=viewport.center;
   float width=Mathf.Abs(right.x)*bounds.width+Mathf.Abs(right.z)*bounds.height;
   float height=Mathf.Abs(up.x)*bounds.width+Mathf.Abs(up.z)*bounds.height;
   scale=Mathf.Min((viewport.width-12)/Mathf.Max(width,.001f),(viewport.height-12)/Mathf.Max(height,.001f));
  }
  public Vector2 Point(Vector3 world){world.y=0;Vector3 d=world-center;return screenCenter+new Vector2(Vector3.Dot(d,right),-Vector3.Dot(d,up))*scale;}
  // Invert the same continuous camera basis used by Point. GUI coordinates run downward.
  public Vector3 WorldPoint(Vector2 screen){
   Vector2 delta=(screen-screenCenter)/scale;float determinant=right.x*up.z-right.z*up.x;
   if(Mathf.Abs(determinant)<.000001f)return center;
   return center+new Vector3((delta.x*up.z+delta.y*right.z)/determinant,0,(-delta.y*right.x-delta.x*up.x)/determinant);
  }
  public Matrix4x4 UnitSquare(Rect bounds){
   Vector2 p=Point(new Vector3(bounds.xMin,0,bounds.yMax)),x=Point(new Vector3(bounds.xMax,0,bounds.yMax))-p,y=Point(new Vector3(bounds.xMin,0,bounds.yMin))-p;
   var m=Matrix4x4.identity;m.SetColumn(0,new Vector4(x.x,x.y,0,0));m.SetColumn(1,new Vector4(y.x,y.y,0,0));m.SetColumn(3,new Vector4(p.x,p.y,0,1));return m;
  }
 }
}
