using UnityEngine;
namespace GreyHarbor {
 public sealed partial class RaidMap {
  public static readonly Vector3 CampSpawn=new Vector3(0,0,5.3f),Door=new Vector3(0,0,8.3f),Warehouse=new Vector3(5.1f,0,0),Workbench=new Vector3(-5,0,1),Market=new Vector3(-3.3f,0,-5.3f);
  public void BuildCampMap(){
   Root=new GameObject("SafeCamp").transform;Spawn=CampSpawn;
   Box("CampGround",new Vector3(0,-.2f,0),new Vector3(22,.4f,21),"485957");
   Box("Courtyard",new Vector3(0,.005f,0),new Vector3(19,.01f,18),"65736A");
   for(int x=-10;x<=10;x+=2)Box("CampJoint",new Vector3(x,.019f,0),new Vector3(.025f,.01f,19),"414E49");
   for(int z=-9;z<=9;z+=2)Box("CampJoint",new Vector3(0,.019f,z),new Vector3(20,.01f,.025f),"414E49");
   Box("NorthFenceL",new Vector3(-6, .7f,10),new Vector3(9,1.4f,.4f),"899181",true);
   Box("NorthFenceR",new Vector3(6,.7f,10),new Vector3(9,1.4f,.4f),"899181",true);
   Box("WestFence",new Vector3(-10.5f,.6f,0),new Vector3(.4f,1.2f,20),"899181",true);
   Box("EastFence",new Vector3(10.5f,.6f,0),new Vector3(.4f,1.2f,20),"899181",true);
   Box("SouthFence",new Vector3(0,.35f,-10),new Vector3(21,.7f,.4f),"899181",true);
   Box("ExitGate",new Vector3(0,1.3f,10),new Vector3(3,2.6f,.35f),"314744",true);
   Box("GateLamp",new Vector3(0,2.5f,9.75f),new Vector3(2,.13f,.16f),"A0D9B2");Zone(Door,new Vector2(3,2),"99C2A4");
   Box("Warehouse",new Vector3(8.1f,.9f,0),new Vector3(4,1.8f,6),"485C55",true);
   Box("WarehouseRoof",new Vector3(8.1f,1.9f,0),new Vector3(4.2f,.2f,6.2f),"899487");
   for(int i=0;i<3;i++)Box("StorageCrate",new Vector3(6.8f+i,.4f,-4),new Vector3(.8f,.8f,.8f),"92835F",true);
   Zone(Warehouse,new Vector2(1.5f,2),"88B5A4");
   Box("CampWorkbench",new Vector3(-7,.5f,1),new Vector3(2,1,3),"76694D",true);
   Box("CampWorkbenchTop",new Vector3(-7,1.05f,1),new Vector3(2.2f,.1f,3.2f),"ADA27B");
   Zone(Workbench,new Vector2(1.5f,2),"B4AD85");
   // South-west trading stall. The eastern counter faces the courtyard.
   Box("MarketBack",new Vector3(-7.5f,1.05f,-5.3f),new Vector3(.2f,2.1f,3.8f),"354842",true);
   Box("MarketCounter",new Vector3(-5,.48f,-5.3f),new Vector3(1,.96f,3.8f),"536355",true);
   Box("MarketCounterTop",new Vector3(-5,1.01f,-5.3f),new Vector3(1.15f,.1f,4),"ADA27B");
   for(int i=0;i<2;i++)Box("MarketPost",new Vector3(-7.3f,1.25f,-7.2f+i*3.8f),new Vector3(.15f,2.5f,.15f),"394942",true);
   Box("MarketCanopy",new Vector3(-6.3f,2.5f,-5.3f),new Vector3(3,.12f,4.3f),"697759");
   Box("MarketSign",new Vector3(-4.82f,1.55f,-5.3f),new Vector3(.12f,.6f,1.3f),"B6A071");
   for(int i=0;i<3;i++)Box("MarketDisplay",new Vector3(-5,1.18f,-6.5f+i*1.15f),new Vector3(.52f,.25f,.58f),i==1?"637E89":"8A805C");
   Zone(Market,new Vector2(1.5f,2.8f),"B6A071");
   
  }
 }
}