using System.Collections.Generic;
using UnityEngine;
namespace GreyHarbor {
 public sealed class FreightDoorFrame2719:MonoBehaviour {
  public string RoomName;public GameObject Header;public float ClearHeight,OpeningWidth,WallHeight;public Vector3 OpeningCenter;
 }
 public sealed partial class RaidMap {
  public readonly List<FreightDoorFrame2719> FreightDoorFrames2719=new List<FreightDoorFrame2719>();
  void FreightDoorHeader2719(string roomName,float x,float z,float openingWidth){
   // Independent static hierarchy: the upper wall and frame never rotate with
   // the moving slab. Its distinct name excludes the full-height WallSkin pass.
   const float slabHeight=2.15f,headerBottom=2.175f,wallThickness=.18f;
   var group=new GameObject("FreightStaticDoorFrame2719_"+roomName);group.transform.SetParent(Root,false);
   var info=group.AddComponent<FreightDoorFrame2719>();info.RoomName=roomName;info.ClearHeight=headerBottom;info.OpeningWidth=openingWidth;info.WallHeight=FreightCeilingHeight2716;info.OpeningCenter=new Vector3(x,0,z);
   float fillHeight=FreightCeilingHeight2716-headerBottom;
   var header=Box("FreightDoorHeader2719_"+roomName,new Vector3(x,headerBottom+fillHeight*.5f,z),new Vector3(wallThickness,fillHeight,openingWidth+.02f),"CBCAB7",true);
   header.transform.SetParent(group.transform,true);info.Header=header;FreightDoorFrames2719.Add(info);
   var art=new WorkshopWastelandArt(this);
   foreach(float side in new[]{-1f,1f}){
    float faceX=x+side*(wallThickness*.5f+.026f);
    // Jambs sit over the already solid side walls, preserving the full 1.6m
    // opening. The lintel trim starts at the existing slab top, with no new
    // physical obstruction below the upper wall.
    foreach(float end in new[]{-1f,1f}){
     float jambZ=z+end*(openingWidth*.5f+.038f);
     art.Cube("FreightDoorJambTrim2719","728779",new Vector3(faceX,slabHeight*.5f,jambZ),new Vector3(.038f,slabHeight,.072f));
     art.Cube("FreightJambWear2719","9EAA94",new Vector3(faceX+side*.021f,.24f,jambZ),new Vector3(.009f,.22f,.055f));
    }
    art.Cube("FreightLintelTrim2719","728779",new Vector3(faceX,slabHeight+.0375f,z),new Vector3(.040f,.075f,openingWidth+.148f));
    art.Cube("FreightLintelHighlight2719","9FAB95",new Vector3(faceX+side*.022f,slabHeight+.065f,z),new Vector3(.009f,.015f,openingWidth+.146f));
    // The narrow upper moulding belongs to the header only, unlike WallSkin's
    // lower green coat, skirt and piping extending below a normal wall.
    art.Cube("FreightHeaderCap2719","B3B3A0",new Vector3(faceX,FreightCeilingHeight2716-.03f,z),new Vector3(.027f,.055f,openingWidth+.025f));
   }
   art.Bake(group,group.AddComponent<WorkshopArtResources>());
  }
 }
 public sealed partial class RaidVerification {
  // Call with false before opening rooms and true after FreightRoomDoors2717.
  // This checks only the new headers/frame and the directly affected openings.
  void FreightDoorHeaderCheck2719(bool opened){
   var map=G.Map;Physics.SyncTransforms();Check(map.FreightDoorFrames2719.Count==map.FreightRooms2716.Count,"2719 one static header per existing freight room door");
   foreach(var frame in map.FreightDoorFrames2719){
    var room=map.FreightRooms2716.Find(r=>r.Name==frame.RoomName);var body=frame.Header.GetComponent<BoxCollider>();var bounds=body.bounds;var door=room.Door;
    Check(frame.transform.parent==map.Root&&!frame.transform.IsChildOf(door.Pivot),"2719 header remains separate from rotating door "+frame.RoomName);
    Check(body.enabled&&!body.isTrigger&&body.gameObject.layer==SceneObstacle.HighLayer,"2719 upper wall has real solid collision "+frame.RoomName);
    Check(Mathf.Abs(bounds.min.y-2.175f)<.001f&&Mathf.Abs(bounds.max.y-map.FreightCeilingHeight2716)<.001f&&Mathf.Abs(bounds.size.x-.18f)<.001f&&bounds.size.z>=1.619f,"2719 upper wall fills opening to actual wall top with side overlap "+frame.RoomName);
    foreach(var renderer in frame.GetComponentsInChildren<MeshRenderer>()){
     Check(renderer.sharedMaterial.shader.name=="GreyHarbor/FreightWall2718"&&!renderer.receiveShadows,"2719 door surround preserves known-room fog shader and stable wall shading "+renderer.name);
     Check(renderer.name!="WallPaint"&&renderer.name!="WallSkirt"&&renderer.name!="ServicePipes","2719 header has no full-height wall skin intruding into opening "+renderer.name);
    }
    float y=(bounds.min.y+bounds.max.y)*.5f;RaycastHit hit;
    bool solid=Physics.Raycast(new Vector3(door.Position.x-.65f,y,door.Position.z),Vector3.right,out hit,1.3f,SceneObstacle.SightMask,QueryTriggerInteraction.Ignore)&&hit.collider==body;
    Check(solid,"2719 actual upper-wall ray intersects door header "+frame.RoomName);
    if(opened){
     Check(door.Open,"2719 original room door still opens normally "+frame.RoomName);
     var a=door.Position+Vector3.left*.85f;var b=door.Position+Vector3.right*.85f;
     Check(G.Navigation.ClearSegment(a,b),"2719 real player capsule retains clearance through open door "+frame.RoomName);
     bool highClear=!Physics.Raycast(new Vector3(door.Position.x-.65f,1.90f,door.Position.z),Vector3.right,1.3f,SceneObstacle.SightMask,QueryTriggerInteraction.Ignore);
     Check(highClear,"2719 doorway below lintel remains open at 1.9m "+frame.RoomName);
    }
   }
  }
 }
}
