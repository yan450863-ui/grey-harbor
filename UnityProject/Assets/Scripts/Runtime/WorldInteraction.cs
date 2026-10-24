using UnityEngine;
namespace GreyHarbor {
 public sealed partial class RaidGame {
  public WorldDoor NearbyDoor;public float DoorHold;object interactionLock;bool requireInteractRelease;
  public const float InteractionRange=1.45f;
  public Bounds LootBounds(LootContainer loot){
   if(loot.Prop){foreach(string name in new[]{"CaseBody","WallCabinet","CoatBody","Body"}){var t=loot.Prop.transform.Find(name);if(t){var r=t.GetComponent<Renderer>();if(r)return r.bounds;}}}
   return new Bounds(loot.Position+Vector3.up*.3f,new Vector3(.7f,.6f,.55f));
  }
  Vector3 LootContact(LootContainer l){var b=LootBounds(l);var p=Player.transform.position;return new Vector3(Mathf.Clamp(p.x,b.min.x,b.max.x),0,Mathf.Clamp(p.z,b.min.z,b.max.z));}
  bool Reachable(Vector3 point,Transform own=null,Collider door=null){
   Vector3 from=Player.transform.position+Vector3.up*CombatHeight,to=new Vector3(point.x,CombatHeight,point.z);
   foreach(var overlap in Physics.OverlapSphere(from,.025f,SceneObstacle.SightMask,QueryTriggerInteraction.Ignore)){if(overlap==door)continue;if(own&&overlap.transform.IsChildOf(own)&&overlap.name!="MountingWall")continue;return false;}
   foreach(var hit in Physics.RaycastAll(from,(to-from).normalized,Vector3.Distance(from,to),SceneObstacle.SightMask,QueryTriggerInteraction.Ignore)){
    if(hit.collider==door)continue;if(own&&hit.collider.transform.IsChildOf(own)&&hit.collider.name!="MountingWall")continue;return false;
   }return true;
  }
  public bool CanInteractLoot(LootContainer l,float range=InteractionRange){return l!=null&&(l.Items.Count>0||l.Opened||l.Corpse325!=null)&&Vision.RoomAllowed(LootBounds(l).center)&&Vector3.Distance(LootContact(l),Player.transform.position)<=range&&Reachable(LootContact(l),l.Prop?l.Prop.transform:null);}
  bool CanInteractDoor(WorldDoor d){return d!=null&&Vector3.Distance(d.Position,Player.transform.position)<=InteractionRange&&Reachable(d.Position,null,d.Collider);}
  float MouseDistance(Vector3 p){var s=View.WorldToScreenPoint(p+Vector3.up*.65f);return Vector2.Distance(new Vector2(s.x,s.y),Controls.AimScreen);}
  void SelectInteraction(){
   Nearby=null;NearbyDoor=null;float best=999,mouse=999999;
   foreach(var l in Map.Loot)if(CanInteractLoot(l)){float d=Vector3.Distance(LootContact(l),Player.transform.position),m=MouseDistance(LootBounds(l).center-new Vector3(0,LootBounds(l).center.y,0));if(d<best-.16f||(Mathf.Abs(d-best)<=.16f&&m<mouse)){Nearby=l;NearbyDoor=null;best=d;mouse=m;}}
   foreach(var door in Map.Doors)if(CanInteractDoor(door)){float d=Vector3.Distance(door.Position,Player.transform.position),m=MouseDistance(door.Position);if(d<best-.16f||(Mathf.Abs(d-best)<=.16f&&m<mouse)){Nearby=null;NearbyDoor=door;best=d;mouse=m;}}
  }
  void ResetWorldInteraction(bool release){CancelQuestInteraction319();if(openingBox!=null&&!openingBox.Opened)openingBox.Search=0;openingBox=null;interactionLock=null;Nearby=null;NearbyDoor=null;DoorHold=0;lootInteractHeld=false;requireInteractRelease=release;}
  internal void TickInteraction(float dt){
   bool held=Controls.Interact,pressed=held&&!lootInteractHeld;lootInteractHeld=held;
   if(!InventoryOpen&&Controls.Fire){ResetWorldInteraction(true);TickExtraction(dt);return;}
   if(requireInteractRelease){Nearby=null;NearbyDoor=null;if(!held)requireInteractRelease=false;TickIdentification(dt);TickExtraction(dt);return;}
   if(OpenLoot!=null&&!CanInteractLoot(OpenLoot,2.0f))CloseInventory();
   if(!held||InventoryOpen||ReloadTime>0||HealTime>0){if(openingBox!=null&&!openingBox.Opened)openingBox.Search=0;openingBox=null;interactionLock=null;DoorHold=0;}
   SelectInteraction();if(!InventoryOpen&&ReloadTime<=0&&HealTime<=0&&TickQuestInteraction319(dt,held)){Nearby=null;NearbyDoor=null;openingBox=null;interactionLock=null;DoorHold=0;TickExtraction(dt);return;}
   if(interactionLock is LootContainer lockedBox){if(CanInteractLoot(lockedBox)){Nearby=lockedBox;NearbyDoor=null;}else{ResetWorldInteraction(true);return;}}
   if(interactionLock is WorldDoor lockedDoor){if(CanInteractDoor(lockedDoor)){Nearby=null;NearbyDoor=lockedDoor;}else{ResetWorldInteraction(true);return;}}
   if(!InventoryOpen&&ReloadTime<=0&&HealTime<=0){
    if(NearbyDoor!=null){
     if(NearbyDoor.Portal&&held){interactionLock=NearbyDoor;DoorHold+=dt;if(DoorHold>=2){SwitchInterior(NearbyDoor.Destination);return;}}
     else if(!NearbyDoor.Portal&&pressed){ToggleRoomDoor(NearbyDoor);requireInteractRelease=true;}
    }else if(Nearby!=null){
     if(Nearby.Opened){if(pressed){OpenLoot=Nearby;InventoryOpen=true;}}
     else if(held){interactionLock=openingBox=Nearby;Nearby.Search+=dt;if(Nearby.Search>=Nearby.SearchDuration){Nearby.Search=Nearby.SearchDuration;Nearby.Opened=true;Nearby.Prop?.SetSearched();OpenLoot=Nearby;InventoryOpen=true;openingBox=null;interactionLock=null;EnsureLootGrid(Nearby);audioPlayer.PlayOneShot(lootClip);return;}}
    }
   }
   TickIdentification(dt);TickExtraction(dt);
  }
 }
}
