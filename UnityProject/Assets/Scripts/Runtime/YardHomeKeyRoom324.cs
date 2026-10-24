using UnityEngine;
namespace GreyHarbor {
 public sealed partial class RaidMap {
  void PopulateYardHomeKeyRoom324(){
   if(InteriorId!="yardhome")return;
   var yardRoom=YardRooms138.Find(r=>r.Door.Locked);
   yardRoom.Name="私人储藏室";
   var room=KeyRoom2760("生活小屋私人储藏室",yardRoom.Area,yardRoom.Door,ItemKind.YardHomeKey324);
   room.Door.InstantToggle=true;room.Door.OpenAngle=180;room.Door.ClosedSafety=new Rect(1.54f,3.1f,.12f,1.8f);
   var rng=KeyRng2760(324001);
   // Keep the doorway and centre aisle clear; containers face inward from the walls.
   KeyLoot2760(room,ContainerLook.Safe,6.35f,5.15f,90,rng,SafeSize240.Small);
   KeyLoot2760(room,ContainerLook.Wardrobe,6.35f,3.0f,90,rng);
   KeyLoot2760(room,ContainerLook.RollingSuitcase,4.1f,2.65f,180,rng);
   KeyLoot2760(room,ContainerLook.JewelryBox,2.5f,2.65f,180,rng);
   YardShelf138(new Vector3(4.05f,0,5.62f));
   Box("PrivateStorageFoldedBlankets324",new Vector3(4.15f,1.60f,5.62f),new Vector3(.55f,.15f,.40f),"8D9677");
   Box("PrivateStorageRug324",new Vector3(3.6f,.068f,4.35f),new Vector3(1.8f,.018f,1.05f),"646F58");
   NoticeBoard114(new Vector3(4.0f,1.6f,5.82f));
   var lights=new WorkshopWastelandArt(this);lights.StripLight(new Vector3(4.05f,1.96f,5.83f),Vector3.right,Vector3.back);
   var lightRoot=new GameObject("PrivateStorageLight324");lightRoot.transform.SetParent(Root,false);lights.Bake(lightRoot,lightRoot.AddComponent<WorkshopArtResources>());
  }
 }
}
