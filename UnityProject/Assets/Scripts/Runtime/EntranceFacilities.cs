using UnityEngine;
namespace GreyHarbor {
 public sealed partial class GateParkingKit {
  public void Shelter(Vector3 p,bool bikes){
   Begin(bikes?"GateKit_CycleShelter":"GateKit_WaitingShelter",p);float w=bikes?6:8,d=3.4f;
   foreach(float x in new[]{-w/2,w/2}){B("SteelPost",new Vector3(x,1.4f,1.2f),new Vector3(.14f,2.8f,.14f),"4E655E");Solid(new Vector3(x,1.4f,1.2f),new Vector3(.18f,2.8f,.18f),ObstacleKind.High);}
   B("Roof",new Vector3(0,2.9f,0),new Vector3(w+.4f,.12f,d),"77877E");
   for(float x=-w/2;x<=w/2;x+=.5f)B("RoofSeam",new Vector3(x,2.99f,0),new Vector3(.035f,.04f,d),"919D8C");
   B("BackRail",new Vector3(0,.95f,1.3f),new Vector3(w,.07f,.08f),"5B7065");
   if(!bikes){
    foreach(float x in new[]{-2.3f,2.3f}){B("Seat",new Vector3(x,.48f,.55f),new Vector3(2.5f,.12f,.62f),"8B8F73");B("SeatBack",new Vector3(x,.88f,.84f),new Vector3(2.5f,.63f,.09f),"778568");foreach(float dx in new[]{-.9f,.9f})B("SeatLeg",new Vector3(x+dx,.23f,.55f),new Vector3(.1f,.46f,.45f),"455D53");Solid(new Vector3(x,.51f,.62f),new Vector3(2.5f,1.02f,.75f),ObstacleKind.Low);}
    B("NoticeBoard",new Vector3(0,1.55f,1.31f),new Vector3(1.5f,.9f,.09f),"3F5652");B("FadedNotice",new Vector3(0,1.55f,1.25f),new Vector3(1.25f,.66f,.015f),"B0B398");
   }else{
    for(int i=0;i<5;i++){float x=-2+i;B("CycleRack",new Vector3(x,.24f,.2f),new Vector3(.09f,.48f,1.4f),"708A80");Solid(new Vector3(x,.24f,.2f),new Vector3(.13f,.48f,1.4f),ObstacleKind.Low);}
    // Two simple parked bicycles: open polygon rims, frame and handlebar.
    foreach(float x in new[]{-1.5f,1.5f}){
     foreach(float z in new[]{-.48f,.55f})for(int i=0;i<12;i++){float a=i*Mathf.PI/6;var rim=B("BicycleRim",new Vector3(x,.37f+Mathf.Sin(a)*.32f,z+Mathf.Cos(a)*.32f),new Vector3(.045f,.06f,.18f),"324A42");rim.transform.localRotation=Quaternion.Euler(-i*30,0,0);}
     B("CycleFrame",new Vector3(x,.56f,0),new Vector3(.06f,.07f,1.1f),"80795D");B("CycleStem",new Vector3(x,.81f,.52f),new Vector3(.06f,.55f,.06f),"6C8377");B("Handlebar",new Vector3(x,1.08f,.52f),new Vector3(.53f,.05f,.06f),"364C45");B("Saddle",new Vector3(x,.9f,-.18f),new Vector3(.20f,.08f,.29f),"33483E");Solid(new Vector3(x,.52f,.1f),new Vector3(.55f,1.04f,1.8f),ObstacleKind.Low);
    }
   }
  }
  public void Planter(Vector3 p,float length){Begin("GateKit_Planter",p);B("Soil",new Vector3(0,.19f,0),new Vector3(length,.30f,1.3f),"666D4F");foreach(float z in new[]{-.72f,.72f})B("PlanterRim",new Vector3(0,.23f,z),new Vector3(length+.2f,.46f,.14f),"969D87");foreach(float x in new[]{-length/2,length/2})B("EndRim",new Vector3(x,.23f,0),new Vector3(.14f,.46f,1.5f),"969D87");Solid(new Vector3(0,.23f,0),new Vector3(length+.2f,.46f,1.5f),ObstacleKind.Low);for(float x=-length/2+.4f;x<length/2;x+=.7f){B("LowShrub",new Vector3(x,.53f,0),new Vector3(.68f,.54f,.8f),"6F7D54");}}
  public void LitterBin(Vector3 p){Begin("GateKit_LitterBin",p);B("BinBody",new Vector3(0,.45f,0),new Vector3(.58f,.9f,.5f),"5A7566");B("Lid",new Vector3(0,.94f,0),new Vector3(.66f,.12f,.58f),"788573");B("Opening",new Vector3(0,.76f,-.256f),new Vector3(.4f,.14f,.02f),"293F36");Solid(new Vector3(0,.5f,0),new Vector3(.66f,1,.58f),ObstacleKind.Low);}
 }
 public sealed partial class RaidMap {
  void BuildEntranceFacilities(){
   var surface=new GroundSurfaceArt(this,130);
   surface.Region(new Rect(-5.5f,-39,12,10),GroundFinish.Concrete,.035f);
   surface.Region(new Rect(16,-31,8,6),GroundFinish.PatchedConcrete,.035f);
   surface.Region(new Rect(6.5f,-38,8.5f,3),GroundFinish.Concrete,.035f);
   surface.Bake("Entrance_FacilityPaving");var k=new GateParkingKit(this);
   k.Shelter(new Vector3(.5f,0,-33),false);k.Shelter(new Vector3(20,0,-28),true);
   k.Planter(new Vector3(.5f,0,-39),7);k.Planter(new Vector3(11.5f,0,-27),5);
   k.LitterBin(new Vector3(5.5f,0,-32));
   // Floor anchors for the two upright rolling suitcases.
   foreach(var p in new[]{new Vector3(-1,0,-34),new Vector3(2,0,-34)}){var a=new GameObject("WaitingLuggageAnchor");a.transform.SetParent(Root,false);a.transform.position=p;}
  }
 }
}
