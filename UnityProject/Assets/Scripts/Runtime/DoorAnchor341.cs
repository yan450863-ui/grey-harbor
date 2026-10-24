using UnityEngine;
namespace GreyHarbor {public sealed partial class RaidUI {
 Vector2 DoorAnchor341(WorldDoor door){
  // Stair portals use the actual ground-level interaction marker, not the player.
  bool stairs=(door.Name??"").Contains("楼梯")||(door.Destination??"").Contains("-floor:");
  if(stairs)return ScreenPoint(door.Position+Vector3.up*.45f);
  // Keep the label over the doorway even when the leaf swings aside.
  float height=2.2f;
  if(door.Collider)height=Mathf.Max(.5f,door.Collider.bounds.max.y-door.Position.y);
  return ScreenPoint(door.Position+Vector3.up*height);
 }
}}