using System.Collections.Generic;
using UnityEngine;

namespace GreyHarbor {
 public sealed partial class RaidMap {
  public bool UnloadingInterior2723;
  public float UnloadingCeilingHeight2723;
  public WorldDoor UnloadingExit2723;
  public WorldDoor UnloadingStoreDoor2723;
  public Rect UnloadingStoreRoom2723;
  public readonly List<Rect> InitialAreas2723=new List<Rect>();
  public static bool IsUnloadingInteriorKey2723(string key)=>key=="unloadingwarehouse"||key=="unloadingduty";

  public void BuildUnloadingInterior2723(string key){
   if(!IsUnloadingInteriorKey2723(key))throw new System.ArgumentException("Unknown unloading interior",nameof(key));
   bool warehouse=key=="unloadingwarehouse";
   float width=warehouse?22:8,depth=warehouse?14:6;
   IsInterior=UnloadingInterior2723=true;InteriorId=key;
   InteriorTitle=warehouse?"装卸仓库 / 收货与分拣":"装卸值班间 / 调度与杂物间";
   UnloadingCeilingHeight2723=warehouse?4.5f:3;
   Bounds=new Rect(-width/2-.2f,-depth/2-.2f,width+.4f,depth+.4f);
   Spawn=new Vector3(0,0,warehouse?-5.5f:-1.8f);
   Root=new GameObject("UnloadingInterior2723_"+key).transform;
   var floor=Box("UnloadingWhiteboxFloor2723",new Vector3(0,-.13f,0),new Vector3(width,.24f,depth),"E2E4DE");
   // A supporting collider lies wholly below the movement capsule and stays off obstacle layers.
   floor.AddComponent<BoxCollider>();
   BuildUnloadingFloor2725(floor,warehouse,width,depth);
   UnloadingWall2723(-width/2+.10f,0,.20f,depth);
   UnloadingWall2723(width/2-.10f,0,.20f,depth);
   UnloadingWall2723(0,-depth/2+.10f,width,.20f);
   UnloadingWall2723(0,depth/2-.10f,width,.20f);
   if(warehouse){
    InitialRoom=new Rect(-10.8f,-6.8f,21.6f,13.6f);InitialAreas2723.Add(InitialRoom);
   }else{
    InitialRoom=new Rect(-3.8f,-2.8f,4.7f,5.6f);InitialAreas2723.Add(InitialRoom);
    UnloadingStoreRoom2723=new Rect(1.1f,-2.8f,2.7f,5.6f);
    BuildUnloadingStoreDoor2723();
   }
   float returnZ=warehouse?-6.3f:-2.4f,slabZ=-depth/2+.24f;
   Box("UnloadingWhiteboxReturnSlab2723",new Vector3(0,1.1f,slabZ),new Vector3(warehouse?2.4f:1.5f,2.2f,.08f),"748F96");
   Box("UnloadingWhiteboxReturnPull2723",new Vector3(.45f,.95f,slabZ+.08f),new Vector3(.24f,.075f,.06f),"F1F2EE");
   UnloadingExit2723=new WorldDoor{Name=warehouse?"返回装卸仓库室外":"返回装卸值班间室外",Destination=key,Portal=true,Position=new Vector3(0,0,returnZ)};
   Doors.Add(UnloadingExit2723);
   BuildUnloadingDressing2724(warehouse);
   // No roof or cross-beams: the full whitebox circulation remains visible from the game camera.
   Physics.SyncTransforms();
  }

  void UnloadingWall2723(float x,float z,float width,float depth){
   Box("UnloadingWhiteboxWall2723",new Vector3(x,UnloadingCeilingHeight2723/2,z),new Vector3(width,UnloadingCeilingHeight2723,depth),"D4D7C7",true);
  }

  void BuildUnloadingStoreDoor2723(){
   const float x=1,z=.8f,halfDoor=.7f,low=.1f,high=1.5f;
   UnloadingWall2723(x,(-2.8f+low)/2,.20f,low+2.8f);
   UnloadingWall2723(x,(high+2.8f)/2,.20f,2.8f-high);
   const float doorHeight=2.15f;
   Box("UnloadingWhiteboxDoorHeader2723",new Vector3(x,(doorHeight+3)/2,z),new Vector3(.20f,3-doorHeight,halfDoor*2),"D4D7C7",true);
   var hinge=new GameObject("UnloadingWhiteboxStorageHinge2723").transform;
   hinge.SetParent(Root,false);hinge.position=new Vector3(x,0,low);
   var slab=Box("UnloadingWhiteboxStorageDoor2723",new Vector3(x,doorHeight/2,z),new Vector3(.10f,doorHeight,halfDoor*2),"C9D0C9",true);
   slab.transform.SetParent(hinge,true);
   Box("UnloadingWhiteboxStoragePull2723",new Vector3(x-.08f,.95f,z+.48f),new Vector3(.06f,.16f,.07f),"F1F2EE").transform.SetParent(hinge,true);
   UnloadingStoreDoor2723=new WorldDoor{
    Name="装卸杂物间门",Destination=InteriorId,Position=new Vector3(x,0,z),Pivot=hinge,Collider=slab.GetComponent<Collider>(),
    OpenAngle=95,InstantToggle=true,RevealRoom=UnloadingStoreRoom2723,
    ClosedSafety=new Rect(x-.08f,low-.05f,.16f,halfDoor*2+.10f),
    OpenSafety=new Rect(x-.1f,low-.12f,1.65f,.32f)
   };
   Doors.Add(UnloadingStoreDoor2723);
  }
 }
}
