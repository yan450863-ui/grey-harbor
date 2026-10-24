using UnityEngine;
namespace GreyHarbor {
 public enum ObstacleKind { Decoration, Low, High }
 // Movement is independent of shooting/visibility: low props never intercept shots or sight rays.
 public sealed class SceneObstacle : MonoBehaviour {
  public ObstacleKind Kind=ObstacleKind.High;
  public const int HighLayer=8, LowLayer=10;
  public const int MovementMask=(1<<HighLayer)|(1<<LowLayer), SightMask=1<<HighLayer, ShotMask=(1<<HighLayer)|(1<<9);
  public static void Apply(GameObject target,ObstacleKind kind){
   var rule=target.GetComponent<SceneObstacle>()??target.AddComponent<SceneObstacle>();rule.Kind=kind;
   foreach(var col in target.GetComponentsInChildren<Collider>(true)){col.enabled=kind!=ObstacleKind.Decoration;col.gameObject.layer=kind==ObstacleKind.High?HighLayer:kind==ObstacleKind.Low?LowLayer:0;}
  }
 }
}
