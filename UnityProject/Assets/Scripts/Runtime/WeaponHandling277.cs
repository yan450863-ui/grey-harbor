using UnityEngine;
namespace GreyHarbor {
 // Handling ratings are independent of damage, ammunition grade, base precision and recoil.
 public static class WeaponHandling277 {
  public const int AkMovementStability=60,MP5MovementStability=68,SKSMovementStability=30;
  public const float BaseWalkingSpread=2;
  // Live AK/MP5/SKS profiles share movement math; the active profile selects its rating.
  // Actual walking retains its existing penalty; sprinting scales it up to1.5x.
  public static float MovementExtraHalfAngle(float stability,float actualSpeedRatio)=>BaseWalkingSpread*(1-Mathf.Clamp(stability,0,100)/100f)*Mathf.Clamp(actualSpeedRatio,0,SprintState2711.SpeedMultiplier);
  public static bool ProjectScreenUpRecoil(Camera view,Vector3 anchor,Vector3 rawTarget,float degrees,out Vector3 adjustedTarget,out Vector2 rawScreen,out Vector2 adjustedScreen,out float pixelLift){
   var projected=view.WorldToScreenPoint(rawTarget);rawScreen=new Vector2(projected.x,projected.y);
   adjustedTarget=rawTarget;adjustedScreen=rawScreen;pixelLift=0;
   var planeUp=view.transform.up;planeUp.y=0;if(planeUp.sqrMagnitude<.000001f)return false;
   float distance=Vector3.Distance(anchor,rawTarget);
   float planeLift=distance*Mathf.Tan(Mathf.Clamp(degrees,0,80)*Mathf.Deg2Rad);
   var raised=view.WorldToScreenPoint(rawTarget+planeUp.normalized*planeLift);
   pixelLift=Mathf.Max(0,raised.y-projected.y);adjustedScreen=rawScreen+Vector2.up*pixelLift;
   var plane=new Plane(Vector3.up,anchor);var ray=view.ScreenPointToRay(adjustedScreen);
   if(!plane.Raycast(ray,out float enter)){adjustedScreen=rawScreen;pixelLift=0;return false;}
   adjustedTarget=ray.GetPoint(enter);return true;
  }
 }
}
