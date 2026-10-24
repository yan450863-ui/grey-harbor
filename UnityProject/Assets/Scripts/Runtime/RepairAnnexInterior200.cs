using UnityEngine;
using System.Collections.Generic;
namespace GreyHarbor {
 public sealed class RepairRoom200 {public string Name;public Rect Area;public WorldDoor Door;public Vector3 WalkPoint;}
 public sealed partial class RaidMap {
  public static readonly Vector3 RepairAnnexApproach200=PortPoint(22,64.65f);
  public static readonly Rect RepairHall200=new Rect(-8.9f,-9.4f,13.34f,18.8f);
  public readonly List<RepairRoom200> RepairRooms200=new List<RepairRoom200>();
  public void AddRepairAnnexEntrance200(){Doors.Add(new WorldDoor{Name="进入维修附属房",Destination="repairannex",Portal=true,Position=PortPoint(22,65.48f)});}
  public void BuildRepairAnnex200(){
   IsInterior=true;InteriorId="repairannex";InteriorTitle="维修附属房 / 大维修大厅";Bounds=new Rect(-9.15f,-9.65f,18.3f,19.3f);InitialRoom=RepairHall200;
   Spawn=new Vector3(-2.5f,0,-8.05f);Root=new GameObject("RepairAnnex_Interior200").transform;
   Box("RepairAnnexFloor",new Vector3(0,-.12f,0),new Vector3(17.8f,.24f,18.8f),"757C6C");
   RepairWall200(-8.9f,0,.22f,18.8f);RepairWall200(8.9f,0,.22f,18.8f);RepairWall200(0,-9.4f,18,.22f);RepairWall200(0,9.4f,18,.22f);
   AddRepairRoom200("工具与备件间",new Rect(4.35f,2.8f,4.55f,6.6f),6);
   AddRepairRoom200("工人休息室",new Rect(4.35f,-3.2f,4.55f,6),0);
   AddRepairRoom200("更衣洗漱区",new Rect(4.35f,-9.4f,4.55f,6.2f),-6.1f);
   RepairWall200(6.62f,2.8f,4.55f,.20f);RepairWall200(6.62f,-3.2f,4.55f,.20f);
   var floor=new GroundSurfaceArt(this,200031);floor.Region(new Rect(-8.78f,-9.28f,13.01f,18.56f),GroundFinish.PatchedConcrete,.11f);
   foreach(var room in RepairRooms200){var r=room.Area;r.xMin+=.12f;r.xMax-=.12f;r.yMin+=.12f;r.yMax-=.12f;floor.Region(r,room.Name=="更衣洗漱区"?GroundFinish.BrokenTile:room.Name=="工人休息室"?GroundFinish.Concrete:GroundFinish.PatchedConcrete,.11f);}floor.Bake("RepairAnnex_Ground200");
   var k=new RepairInteriorKit200(this);var ext=new RepairExteriorKit149(this);
   // Keep the large central bay completely traversable: furnishings belong to the perimeter.
   k.ToolWallBench(new Vector3(-8.05f,0,-1.1f),270);k.PartsRack(new Vector3(-6.4f,0,8.75f));
   k.EngineBench(new Vector3(-2.4f,0,8.5f));ext.EngineHoist(new Vector3(-7.7f,0,5.45f));ext.Compressor(new Vector3(-7.9f,0,-5.8f),90);
   ext.AxleStand(new Vector3(-7.8f,0,2.3f));ext.AxleStand(new Vector3(-7.0f,0,2.3f));
   k.PartsRack(new Vector3(6.8f,0,8.65f));k.PartsRack(new Vector3(8.18f,0,5.6f),90);
   PortSofa(new Vector3(6.75f,0,1.9f));LowFurniture("RepairRestCoffeeTable",new Vector3(7,.3f,1),new Vector3(1.2f,.6f,.65f),"8A8066");Chair(new Vector3(6.5f,0,-2.05f),180);
   LowFurniture("RepairRestCounter",new Vector3(8.3f,.42f,-1.5f),new Vector3(.68f,.84f,1.3f),"82927B");
   k.WorkLockers(new Vector3(6.85f,0,-8.75f),180);k.WashStation(new Vector3(8.25f,0,-4.6f),90);Chair(new Vector3(6.8f,0,-7.4f),90);
   // A modest toilet niche shares the washroom, not another tiny gameplay room.
   LowFurniture("RepairToilet",new Vector3(8.12f,.27f,-7.55f),new Vector3(.62f,.54f,.83f),"AAB09A");Box("RepairToiletSeat",new Vector3(8.12f,.60f,-7.45f),new Vector3(.68f,.1f,.67f),"BFC0A7");Box("RepairCistern",new Vector3(8.12f,.82f,-8.05f),new Vector3(.65f,.65f,.3f),"A7AF9A");
   Box("RepairPrivacyPartition",new Vector3(7.4f,1,-7.95f),new Vector3(.12f,2,1.5f),"748774",true);
   DressRepairAnnex200();BuildRepairAnnexContainers2718();
   // A solid exterior shell is a separate map. The indoor shutter is the return interaction.
   var exit=new WorkshopWastelandArt(this);exit.Cube("RepairIndoorShutter","647863",new Vector3(-2.5f,1.6f,-9.22f),new Vector3(5.1f,3.2f,.08f));
   for(int i=0;i<19;i++)exit.Cube("RepairIndoorShutterSlat","949E83",new Vector3(-2.5f,.10f+i*.165f,-9.167f),new Vector3(5,.035f,.025f));
   exit.Cube("RepairIndoorShutterPull","B1A078",new Vector3(-2.5f,.85f,-9.12f),new Vector3(.55f,.11f,.06f));var exitRoot=new GameObject("RepairAnnexExitArt200");exitRoot.transform.SetParent(Root,false);exit.Bake(exitRoot,exitRoot.AddComponent<WorkshopArtResources>());
   Doors.Add(new WorldDoor{Name="返回维修区室外",Destination="repairannex",Portal=true,Position=new Vector3(-2.5f,0,-9.08f)});
   // Walls render normally; no player-centred cutaway.
   AddCorridorDressing277();FinalizeHarborGround137();Physics.SyncTransforms();
  }
  void RepairWall200(float x,float z,float w,float d){Box("InteriorWall",new Vector3(x,1.4f,z),new Vector3(w,2.8f,d),"92967F",true);}
  void AddRepairRoom200(string name,Rect r,float z){
   float x=4.35f;float low=z-.9f,high=z+.9f;if(low>r.yMin)RepairWall200(x,(r.yMin+low)/2,.20f,low-r.yMin);if(high<r.yMax)RepairWall200(x,(high+r.yMax)/2,.20f,r.yMax-high);
   var hinge=new GameObject("RepairRoom200_"+name).transform;hinge.SetParent(Root,false);hinge.position=new Vector3(x,0,low);
   var slab=Box("RepairSideRoomDoor200",new Vector3(x,1.075f,z),new Vector3(.12f,2.15f,1.8f),"647D65",true);slab.transform.SetParent(hinge,true);
   Box("RepairRoomDoorPull200",new Vector3(x-.105f,.95f,z+.65f),new Vector3(.08f,.18f,.07f),"BAA774").transform.SetParent(hinge,true);
   var d=new WorldDoor{Name=name+"门",Position=new Vector3(x,0,z),Pivot=hinge,Collider=slab.GetComponent<Collider>(),OpenAngle=100,RevealRoom=r,ClosedSafety=new Rect(x-.42f,z-1.18f,.84f,2.36f),OpenSafety=new Rect(x,z-1.2f,2.12f,2.4f)};
   Doors.Add(d);RepairRooms200.Add(new RepairRoom200{Name=name,Area=r,Door=d,WalkPoint=new Vector3(6.25f,0,z)});
  }
  void DressRepairAnnex200(){
   var a=new WorkshopWastelandArt(this);
   foreach(var r in Root.GetComponentsInChildren<MeshRenderer>())if(r.name=="InteriorWall"){var b=r.bounds;bool horizontal=b.size.x>b.size.z;var along=horizontal?Vector3.right:Vector3.forward;var normal=horizontal?Vector3.forward:Vector3.right;
    foreach(float side in new[]{-1f,1f})a.WallSkin(new Vector3(b.center.x,0,b.center.z)+normal*side*((horizontal?b.size.z:b.size.x)/2+.012f),along,normal*side,horizontal?b.size.x:b.size.z,"647963");}
   foreach(float x in new[]{-6f,-.8f})a.StripLight(new Vector3(x,2.05f,9.12f),Vector3.right,Vector3.back);
   foreach(var room in RepairRooms200)a.StripLight(new Vector3(6.6f,2.05f,room.Area.yMax-.14f),Vector3.right,Vector3.back);
   a.Panel("RepairWindowFrame200","3D5949",new Vector3(-8.76f,1.8f,3.0f),Vector3.forward,Vector3.up,3.8f,.90f);a.Panel("RepairOpaqueWindow200","708A79",new Vector3(-8.73f,1.8f,3.0f),Vector3.forward,Vector3.up,3.6f,.7f);
   for(int i=0;i<4;i++)a.Tube("RepairWindowBar200","435A4A",new Vector3(-8.70f,1.35f,1.5f+i),new Vector3(-8.70f,2.25f,1.5f+i),.03f);
   a.BrickScar(new Vector3(-8.72f,.7f,7),Vector3.forward,Vector3.right);a.Cube("RepairFuseBox200","587063",new Vector3(4.19f,1.3f,3.8f),new Vector3(.12f,.55f,.45f));
   a.Cube("RepairKettle200","A4AC97",new Vector3(8.3f,.98f,-1.6f),new Vector3(.25f,.30f,.27f));a.Tube("RepairKettleHandle200","445B4D",new Vector3(8.47f,1.05f,-1.67f),new Vector3(8.47f,1.10f,-1.45f),.035f);
   a.Panel("RepairNoticeBoard200","8F8468",new Vector3(7.6f,1.65f,2.64f),Vector3.right,Vector3.up,1.3f,.65f);
   for(int i=0;i<3;i++)a.Panel("RepairPinnedPaper200","BCB397",new Vector3(7.17f+i*.42f,1.65f,2.62f),Vector3.right,Vector3.up,.28f,.43f);
   a.Panel("RepairWashMirror200","8A9C8A",new Vector3(8.74f,1.65f,-4.6f),Vector3.forward,Vector3.up,1.8f,.7f);
   var wear=new GameObject("RepairAnnexWallWear200");wear.transform.SetParent(Root,false);a.Bake(wear,wear.AddComponent<WorkshopArtResources>());
   var paint=new WorkshopWastelandArt(this);
   // The bay outline is ground paint; its empty interior stays free of collision and props.
   foreach(float x in new[]{-5.4f,1.6f})paint.Floor("RepairIndoorBayPaint200","AEA176",x,-.3f,.1f,11.2f,.16f);
   foreach(float z in new[]{-5.9f,5.3f})paint.Floor("RepairIndoorBayPaint200","AEA176",-1.9f,z,7,.1f,.16f);
   foreach(var p in new[]{new Vector3(-7.1f,.165f,5.5f),new Vector3(-2.4f,.165f,8.2f)})paint.Patch("RepairIndoorOil200","545E4D",p,Vector3.right,Vector3.forward,1.5f,.9f,false);
   for(int i=0;i<11;i++)foreach(float x in new[]{-3.4f,-.4f})paint.Floor("RepairIndoorTyreTracks200","606954",x,-7.1f+i*.34f,.08f,.18f,.164f);
   RepairPaint149(paint);
  }
 }
}
