using UnityEngine;
namespace GreyHarbor {
 public sealed partial class YardLivingKit {
  public Transform PottingBench(Vector3 p,float yaw=0){
   Begin("PottingBench",p);C("TimberTop","918063",new Vector3(0,.87f,0),new Vector3(2.7f,.12f,.82f));
   foreach(float x in new[]{-1.1f,1.1f})foreach(float z in new[]{-.29f,.29f})C("BenchLeg","536352",new Vector3(x,.42f,z),new Vector3(.1f,.84f,.1f));
   C("LowerShelf","76694F",new Vector3(0,.22f,0),new Vector3(2.55f,.08f,.72f));
   foreach(float x in new[]{-.82f,.04f,.80f}){C("ClayPot","9A7956",new Vector3(x,1.08f,.1f),new Vector3(.42f,.30f,.40f));C("PotSoil","4F533D",new Vector3(x,1.24f,.1f),new Vector3(.34f,.025f,.33f));}
   C("FertilizerSack","A49A78",new Vector3(-.60f,.43f,0),new Vector3(.75f,.35f,.52f));C("SmallTray","6A7657",new Vector3(.71f,.36f,0),new Vector3(.58f,.20f,.52f));
   T("TrowelHandle","6B6950",new Vector3(.50f,.96f,-.13f),new Vector3(.94f,.96f,-.13f),.027f);
   C("TrowelBlade","798679",new Vector3(1.03f,.957f,-.13f),new Vector3(.22f,.023f,.14f));
   Hit(new Vector3(0,.47f,0),new Vector3(2.78f,.94f,.86f),ObstacleKind.Low);return End(yaw);
  }
  public Transform RainBarrels(Vector3 p,float yaw=0){
   Begin("RainBarrels",p);
   foreach(float x in new[]{-.65f,.65f}){
    T("FacetedDrum","5F7970",new Vector3(x,.08f,0),new Vector3(x,1.14f,0),.43f);
    C("CoveredDrum","859083",new Vector3(x,1.17f,0),new Vector3(.76f,.09f,.76f));
    foreach(float y in new[]{.19f,.70f,1.05f})for(int i=0;i<12;i++){
     float a=i*Mathf.PI/6,b=(i+1)*Mathf.PI/6;T("DrumBand","3D584D",new Vector3(x+Mathf.Cos(a)*.44f,y,Mathf.Sin(a)*.44f),new Vector3(x+Mathf.Cos(b)*.44f,y,Mathf.Sin(b)*.44f),.021f);
    }
    Hit(new Vector3(x,.61f,0),new Vector3(.9f,1.22f,.9f),ObstacleKind.Low);
   }
   T("DrainPipe","697D69",new Vector3(.65f,.3f,-.48f),new Vector3(.65f,.3f,-.70f),.028f);
   for(int i=0;i<30;i++){float a=i*Mathf.PI/15,b=(i+1)*Mathf.PI/15;T("CoiledHose","354F3E",new Vector3(Mathf.Cos(a)*.30f,.09f,-1+Mathf.Sin(a)*.24f),new Vector3(Mathf.Cos(b)*.30f,.09f,-1+Mathf.Sin(b)*.24f),.026f);}
   return End(yaw);
  }
  public Transform GardenCart(Vector3 p,float yaw=0){
   Begin("GardenCart",p);C("TrayBase","647A69",new Vector3(0,.58f,0),new Vector3(.85f,.10f,1.05f));
   foreach(float x in new[]{-.47f,.47f})C("TraySide","7D8770",new Vector3(x,.78f,0),new Vector3(.10f,.43f,1.13f));
   foreach(float z in new[]{-.55f,.55f})C("TrayEnd","6D7D65",new Vector3(0,.78f,z),new Vector3(.87f,.43f,.1f));
   foreach(float x in new[]{-.32f,.32f})T("PushHandles","455C4F",new Vector3(x,.56f,-.3f),new Vector3(x,.75f,-1.3f),.033f);
   T("Axle","7D8A7B",new Vector3(-.54f,.31f,.35f),new Vector3(.54f,.31f,.35f),.037f);
   foreach(float x in new[]{-.56f,.56f})for(int i=0;i<16;i++){float a=i*Mathf.PI/8,b=(i+1)*Mathf.PI/8;T("CartTyre","2F3D33",new Vector3(x,.31f+Mathf.Cos(a)*.26f,.35f+Mathf.Sin(a)*.26f),new Vector3(x,.31f+Mathf.Cos(b)*.26f,.35f+Mathf.Sin(b)*.26f),.046f);}
   T("ParkedLeg","596C57",new Vector3(0,.54f,-.40f),new Vector3(0,.04f,-.70f),.04f);
   C("SoilFill","716247",new Vector3(0,.65f,0),new Vector3(.70f,.12f,.92f));
   Hit(new Vector3(0,.47f,-.23f),new Vector3(1.22f,.94f,2),ObstacleKind.Low);return End(yaw);
  }
  public Transform FirewoodStack(Vector3 p,float yaw=0){
   Begin("FirewoodStack",p);foreach(float x in new[]{-1.2f,1.2f})C("RackUpright","536352",new Vector3(x,.63f,0),new Vector3(.09f,1.26f,.95f));
   C("RackBase","6C725B",new Vector3(0,.16f,0),new Vector3(2.5f,.12f,.95f));
   for(int row=0;row<4;row++)for(int col=0;col<6;col++){
    float x=-.94f+col*.36f+(row%2)*.1f,y=.30f+row*.23f;
    T("CutLogs",row%2==0?"827257":"947F5D",new Vector3(x,y,-.39f),new Vector3(x,y,.39f),.115f);
    C("CutEnd","B39C70",new Vector3(x,y,-.409f),new Vector3(.13f,.13f,.02f));
   }
   Hit(new Vector3(0,.64f,0),new Vector3(2.6f,1.28f,1.0f),ObstacleKind.Low);return End(yaw);
  }
  public Transform Bed(Vector3 p,float yaw=0){
   Begin("StaffBed",p);C("BedFrame","666D56",new Vector3(0,.25f,0),new Vector3(1.34f,.25f,2.45f));C("OldMattress","A49E7E",new Vector3(0,.46f,0),new Vector3(1.3f,.23f,2.38f));
   C("FadedBlanket","768576",new Vector3(0,.61f,-.30f),new Vector3(1.26f,.07f,1.65f));C("Pillow","B7B39B",new Vector3(0,.66f,.84f),new Vector3(.78f,.18f,.40f));
   C("Headboard","7D785E",new Vector3(0,.65f,1.20f),new Vector3(1.44f,1.08f,.10f));
   for(int i=0;i<6;i++)C("BlanketFolds","637565",new Vector3(-.5f+i*.2f,.655f,-.29f),new Vector3(.02f,.018f,1.60f));
   Hit(new Vector3(0,.53f,0),new Vector3(1.44f,1.06f,2.53f),ObstacleKind.Low);return End(yaw);
  }
  public Transform Stove(Vector3 p,float yaw=0){
   Begin("OldStove",p);C("EnamelBody","87937F",new Vector3(0,.46f,0),new Vector3(.85f,.92f,.75f));
   C("StoveTop","B2AE90",new Vector3(0,.95f,0),new Vector3(.90f,.07f,.79f));
   foreach(float x in new[]{-.22f,.22f})foreach(float z in new[]{-.20f,.20f})C("Burner","3B4A3F",new Vector3(x,1.001f,z),new Vector3(.27f,.025f,.27f));
   C("OvenWindow","30453E",new Vector3(0,.38f,-.384f),new Vector3(.64f,.37f,.02f));T("OvenPull","A5AB92",new Vector3(-.27f,.68f,-.42f),new Vector3(.27f,.68f,-.42f),.025f);
   Hit(new Vector3(0,.51f,0),new Vector3(.94f,1.02f,.84f),ObstacleKind.Low);return End(yaw);
  }
  public Transform Washer(Vector3 p,float yaw=0){
   Begin("LaundryWasher",p);C("MachineBody","A1A48B",new Vector3(0,.46f,0),new Vector3(.90f,.92f,.85f));C("TopPanel","B7B399",new Vector3(0,.96f,0),new Vector3(.93f,.07f,.87f));
   C("DoorInset","3E584C",new Vector3(0,.43f,-.44f),new Vector3(.50f,.50f,.023f));C("DoorGlass","627D6D",new Vector3(0,.43f,-.458f),new Vector3(.35f,.35f,.012f));
   C("Controls","657662",new Vector3(0,.80f,-.45f),new Vector3(.7f,.12f,.025f));C("Dial","B7B297",new Vector3(.23f,.8f,-.478f),new Vector3(.07f,.07f,.022f));
   Hit(new Vector3(0,.52f,0),new Vector3(.96f,1.04f,.91f),ObstacleKind.Low);return End(yaw);
  }
 }
 public sealed partial class RaidMap {
  void DressExpandedYard138(){
   var k=new YardLivingKit(this);
   k.PottingBench(PortPoint(47.3f,18.4f),90);
   k.RainBarrels(PortPoint(47.7f,25.9f),90);
   k.GardenCart(PortPoint(47.9f,33.4f),24);
   k.FirewoodStack(PortPoint(44.6f,22),90);
   var ground=new GroundSurfaceArt(this,138138);
   ground.Region(YardRect(45.8f,16.6f,3,6),GroundFinish.Gravel,.082f);
   ground.Region(YardRect(46.6f,24.8f,2.2f,2.4f),GroundFinish.PatchedConcrete,.083f);
   ground.Bake("Yard_DomesticWorkPaving138");
   var t=new GameObject("YardHouseExteriorPortals138");t.transform.SetParent(Root,false);
   var large=PortPoint(18,30.2f);var small=PortPoint(39,15.2f);
   Doors.Add(new WorldDoor{Name="进入后勤生活小屋",Destination="yardhome",Portal=true,Position=large});
   Doors.Add(new WorldDoor{Name="进入后勤工具房",Destination="yardshed",Portal=true,Position=small});
  }
 }
}
