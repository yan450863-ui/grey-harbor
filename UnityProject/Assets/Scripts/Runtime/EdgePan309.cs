using UnityEngine;
namespace GreyHarbor {
 public static class EdgePan309 {
  public const float EdgeFraction=.2f,MaxDistance=4,SettleSeconds=1.2f,ReturnDelay=.65f;
  const float DampingRate=4.75f/SettleSeconds;
  public sealed class State {
   public Vector2 Offset,Velocity,Target;public float CenterTime;
   public void Reset(){Offset=Velocity=Target=Vector2.zero;CenterTime=0;}
  }
  public static Vector2 Desired(Vector2 cursor,int width,int height){
   if(width<=0||height<=0||cursor.x<0||cursor.y<0||cursor.x>width||cursor.y>height)return Vector2.zero;
   var delta=cursor-new Vector2(width*.5f,height*.5f);
   float edge=Mathf.Max(Mathf.Abs(delta.x)/(width*.5f),Mathf.Abs(delta.y)/(height*.5f)),start=1-2*EdgeFraction;
   if(edge<=start)return Vector2.zero;
   float amount=Mathf.SmoothStep(0,1,(edge-start)/(1-start));
   // Use the fixed screen center for all directions, including a single side edge.
   // Undo the ground plane's vertical foreshortening so world movement projects along the mouse.
   var direction=new Vector2(delta.x,delta.y/Mathf.Sin(RaidCamera.ViewTilt310*Mathf.Deg2Rad));
   return direction.normalized*(MaxDistance*amount);
  }
  public static void Step(State state,Vector2 cursor,int width,int height,bool enabled,float dt){
   if(!enabled||dt<=0)return;var desired=Desired(cursor,width,height);
   if(desired.sqrMagnitude>.000001f){state.CenterTime=0;state.Target=desired;Advance(state,dt);return;}
   // Keep the last edge target during a brief trip through the center. Only continuous
   // center dwell counts, and a frame crossing the delay integrates both parts exactly.
   float hold=Mathf.Min(dt,Mathf.Max(0,ReturnDelay-state.CenterTime));
   state.CenterTime=Mathf.Min(ReturnDelay,state.CenterTime+dt);
   if(hold>0)Advance(state,hold);
   float remaining=dt-hold;
   if(state.CenterTime>=ReturnDelay){state.Target=Vector2.zero;if(remaining>0)Advance(state,remaining);}
  }
  static void Advance(State s,float dt){
   // Exact critically damped spring: retain velocity when the mouse reverses direction.
   var error=s.Offset-s.Target;var transient=(s.Velocity+DampingRate*error)*dt;float decay=Mathf.Exp(-DampingRate*dt);
   s.Offset=s.Target+(error+transient)*decay;s.Velocity=(s.Velocity-DampingRate*transient)*decay;
   if(s.Offset.sqrMagnitude>MaxDistance*MaxDistance){s.Offset=s.Offset.normalized*MaxDistance;var normal=s.Offset.normalized;float outward=Vector2.Dot(s.Velocity,normal);if(outward>0)s.Velocity-=normal*outward;}
   if((s.Offset-s.Target).sqrMagnitude<.000001f&&s.Velocity.sqrMagnitude<.000025f){s.Offset=s.Target;s.Velocity=Vector2.zero;}
  }
 }
 public sealed partial class RaidGame {
  public bool? CameraFocusOverride309;
  public bool CameraLookAllowed309=>Phase==GamePhase.Raid&&!Paused&&!InventoryOpen&&!IsLoading2721&&Player&&!Player.Dead&&(VerifyMode&&CameraFocusOverride309.HasValue?CameraFocusOverride309.Value:Application.isFocused);
  public void TickCameraLook309(float dt){if(Rig&&Controls!=null)Rig.TickEdgePan309(Controls.AimScreen,Screen.width,Screen.height,CameraLookAllowed309,dt);}
 }
}
