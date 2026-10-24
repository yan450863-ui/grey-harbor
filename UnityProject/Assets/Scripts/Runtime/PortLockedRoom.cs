using UnityEngine;
namespace GreyHarbor {
 public sealed partial class RaidMap {
  public static readonly Rect PortLockedRoomBounds=new Rect(-14,.5f,5,5);
  public static readonly Vector3 PortLockedDoorApproach=new Vector3(-7.7f,0,2.4f);
  void AddPortLockedRoom(){
   Bounds.xMin=-14.2f;
   // The existing 1.5 metre leaf now has a hinge and a real opening in the storage wall.
   Box("ReservedLockedRoomFloor",new Vector3(-11.5f,-.12f,3),new Vector3(5,.24f,5),"6A7066");
   InteriorWall(-14,3,.18f,5);InteriorWall(-11.5f,.5f,5,.18f);InteriorWall(-11.5f,5.5f,5,.18f);
   var hinge=new GameObject("PortStorageDoorHinge2760").transform;hinge.SetParent(Root,false);hinge.position=new Vector3(-9,0,3.15f);
   var slab=Box("PortLockedDoor",new Vector3(-9,1.075f,2.4f),new Vector3(.14f,2.15f,1.5f),"5C6B60",true);slab.transform.SetParent(hinge,true);
   Box("LockedDoorFrameTop",new Vector3(-8.9f,2.18f,2.4f),new Vector3(.1f,.12f,1.68f),"45574C");
   foreach(float z in new[]{1.6f,3.2f})Box("LockedDoorFrame",new Vector3(-8.9f,1.12f,z),new Vector3(.1f,2.24f,.11f),"45574C");
   Box("LockWarningPlate",new Vector3(-8.91f,1.6f,2.4f),new Vector3(.025f,.23f,.38f),"AA9D71").transform.SetParent(hinge,true);
   Doors.Add(new WorldDoor{Name="储藏室上锁房间",Locked=true,Position=new Vector3(-9,0,2.4f),Pivot=hinge,OpenAngle=100,Collider=slab.GetComponent<Collider>(),RevealRoom=PortLockedRoomBounds,ClosedSafety=new Rect(-9.4f,1.3f,.8f,2.2f),OpenSafety=new Rect(-10.95f,3.0f,2.05f,.9f)});
  }
 }
}
