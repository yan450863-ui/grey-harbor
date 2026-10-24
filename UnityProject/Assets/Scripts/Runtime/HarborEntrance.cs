using UnityEngine;
namespace GreyHarbor {
 public sealed partial class RaidMap {
  public static readonly Rect EntranceArtBounds=new Rect(-25,-65,65,41);
  public void BuildHarborEntrance(int seed){
   var kit=new PortEnvironmentKit(this);
   // Replacement is confined to the southern entrance; surrounding whitebox stays intact.
   var ground=Box("Entrance_Base",new Vector3(7.5f,.031f,-44.4f),new Vector3(64.4f,.008f,39.4f),"7B8073");
   var surface=new GroundSurfaceArt(this,128029);
   surface.Region(new Rect(-5,-62,15,18),GroundFinish.PatchedConcrete,.028f);
   surface.Region(new Rect(12,-44,23,17),GroundFinish.Concrete,.028f);
   surface.Region(new Rect(-17,-63.7f,8,39.7f),GroundFinish.Asphalt,.030f);
   surface.Region(new Rect(-9,-47,18,6),GroundFinish.Asphalt,.030f);
   surface.Region(new Rect(7,-60,33,6),GroundFinish.Asphalt,.030f);
   surface.Region(new Rect(-23.5f,-62,3.5f,30),GroundFinish.Gravel,.029f);
   surface.Region(new Rect(35.5f,-52,3.5f,19),GroundFinish.Soil,.029f);
   surface.Bake("Entrance_GroundMaterials");
   BuildEntranceLandscape();BuildEntranceFacilities();
   var booth=kit.Building(new Vector3(3,0,-53),6,7,"899486","647572");booth.name="Entrance_Gatehouse";
   var office=kit.Building(new Vector3(22,0,-37),14,9,"93917E","737D76");office.name="Entrance_ServiceBuilding";
   ExteriorBuildings.Add(new Rect(0,-56.5f,6,7));ExteriorBuildings.Add(new Rect(15,-41.5f,14,9));
   kit.Shutter(new Vector3(-.07f,0,-53),90,"Gatehouse");kit.Shutter(new Vector3(22,0,-41.58f),0,"Service");
   Doors.Add(new WorldDoor{Name="进入港口值班室",Destination="portbooth",Portal=true,Position=new Vector3(-.35f,0,-53)});Doors.Add(new WorldDoor{Name="进入港口附属房",Destination="portservice",Portal=true,Position=new Vector3(22,0,-41.9f)});
   kit.Wall(new Vector3(-22,0,-64),6,true);kit.Wall(new Vector3(16.5f,0,-64),47,true);
   // Closed arrival gate preserves the south map boundary; it is not an extraction.
   Box("Entrance_GateSolid",new Vector3(-13,1.05f,-64),new Vector3(12,2.1f,.28f),"51655F",true);
   for(float x=-18.7f;x<-7;x+=.5f)Box("Entrance_GateBars",new Vector3(x,1.17f,-64.18f),new Vector3(.07f,2.3f,.10f),"8C9687");
   foreach(float x in new[]{-19f,-7f})Box("Entrance_GatePillar",new Vector3(x,1.5f,-64),new Vector3(.75f,3,.75f),"7A8579",true);
   Box("Entrance_GateCrossbeam",new Vector3(-13,3.2f,-64),new Vector3(12.8f,.45f,.45f),"68776D");
   // Discontinuous curb edges leave road junctions and pedestrian access open.
   foreach(float x in new[]{-17.3f,-8.7f}){
    Curb(x,-55,.22f,16);Curb(x,-33,.22f,17);
   }
   for(float z=-60;z<-25;z+=5)Box("Entrance_WornLaneMark",new Vector3(-13,.059f,z),new Vector3(.12f,.008f,2),"C1B994");
   Box("Entrance_StopLine",new Vector3(-13,.061f,-58.7f),new Vector3(6.7f,.012f,.22f),"BBB79C");
   for(int i=0;i<5;i++)Box("Entrance_Crosswalk",new Vector3(-16.2f+i*1.45f,.062f,-48.5f),new Vector3(.65f,.008f,2),"B9BAA0");
   // Simple parking bay paint beside the service building.
   for(int i=0;i<4;i++)Box("Entrance_ParkingLine",new Vector3(13+i*3,.063f,-49.5f),new Vector3(.10f,.008f,4.4f),"A9AF96");
   Box("Entrance_ParkingBack",new Vector3(17.5f,.063f,-51.7f),new Vector3(9,.008f,.10f),"A9AF96");
   kit.Lamp(new Vector3(-20,0,-54),0);kit.Lamp(new Vector3(-5.5f,0,-38),180);
   kit.Bench(new Vector3(7.5f,0,-54));
   foreach(float z in new[]{-57.8f,-49f})foreach(float x in new[]{-.9f,6.8f}){
    var b=Box("Entrance_Bollard",new Vector3(x,.4f,z),new Vector3(.2f,.8f,.2f),"B2A579",true);SceneObstacle.Apply(b,ObstacleKind.Low);
    Box("Entrance_BollardBand",new Vector3(x,.53f,z),new Vector3(.21f,.16f,.21f),"4F6059");
   }
   BuildEntranceContent(seed);
   // Grates and vegetation are flush / non-blocking detail, away from the gate corridor.
   for(int i=0;i<12;i++)Box("Entrance_DrainGrate",new Vector3(-18.3f,.056f,-57+i*.23f),new Vector3(.48f,.018f,.055f),"46564E");
   var random=new System.Random(128);for(int i=0;i<24;i++){
    float x=i<12?-23+(float)random.NextDouble()*1.2f:36+(float)random.NextDouble()*2;
    float z=-61+(float)random.NextDouble()*30;
    for(int j=0;j<3;j++){var blade=Box("Entrance_Weed",new Vector3(x+j*.07f,.12f,z),new Vector3(.035f,.24f,.045f),j%2==0?"657058":"7A7D59");blade.transform.rotation=Quaternion.Euler(j*8,0,j*13-13);}
   }
  }
  void Curb(float x,float z,float w,float d){var b=Box("Entrance_Curb",new Vector3(x,.07f,z),new Vector3(w,.14f,d),"939A87");}
 }
}
