using UnityEngine;
namespace GreyHarbor {
 public sealed partial class ContainerArt {
  void DomesticContainer139(ContainerLook look){
   if(look==ContainerLook.BedsideDrawer){
    Solid(.39f,.82f,.78f,.68f,ObstacleKind.Low,"87785C");
    Part("DrawerTop",0,.815f,0,.89f,.07f,.73f,"A29371");
    foreach(float x in new[]{-.33f,.33f})foreach(float z in new[]{-.25f,.25f})Part("ShortLeg",x,.06f,z,.09f,.12f,.09f,"4D5947");
    Part("DarkDrawerCavity",0,.58f,-.349f,.70f,.32f,.022f,"293E35");
    Part("BottomDrawer",0,.23f,-.362f,.72f,.29f,.035f,"978568");Part("BottomPull",0,.23f,-.40f,.22f,.04f,.045f,"B8AC89");
    Pivot(new Vector3(0,.58f,0),Vector3.zero);prop.OpenOffset=new Vector3(0,0,-.38f);
    AttachLast(B("SlidingDrawerFront",new Vector3(0,.58f,-.367f),new Vector3(.74f,.31f,.045f),"A19070"));
    AttachLast(B("DrawerPull",new Vector3(0,.58f,-.418f),new Vector3(.24f,.045f,.06f),"C1B38D"));
    AttachLast(B("DrawerTray",new Vector3(0,.44f,-.05f),new Vector3(.67f,.025f,.55f),"514E39"));
    foreach(float x in new[]{-.34f,.34f})AttachLast(B("DrawerSide",new Vector3(x,.52f,-.05f),new Vector3(.025f,.15f,.55f),"736348"));
   }else if(look==ContainerLook.Wardrobe){
    Solid(1.07f,1.50f,2.14f,.72f,ObstacleKind.High,"71806B");
    Part("TopCornice",0,2.17f,0,1.61f,.09f,.78f,"9A9475");Part("Plinth",0,.08f,0,1.57f,.16f,.78f,"485A4A");
    Part("DarkWardrobeInside",-.37f,1.14f,-.372f,.68f,1.91f,.022f,"293F35");
    Part("RightDoor",.37f,1.14f,-.392f,.71f,1.95f,.045f,"819078");Part("RightPull",.16f,1.09f,-.437f,.035f,.25f,.06f,"C1B38D");
    Pivot(new Vector3(-.75f,1.14f,-.39f),new Vector3(0,78,0));
    AttachLast(B("WardrobeDoor",new Vector3(-.37f,1.14f,-.405f),new Vector3(.72f,1.95f,.055f),"90977B"));
    AttachLast(B("LeftPull",new Vector3(-.15f,1.09f,-.45f),new Vector3(.035f,.25f,.06f),"C1B38D"));
    foreach(float y in new[]{.60f,1.72f})AttachLast(B("TimberDoorRail",new Vector3(-.37f,y,-.439f),new Vector3(.58f,.045f,.017f),"68785D"));
    Part("HangingRail",-.37f,1.73f,-.37f,.59f,.035f,.03f,"9B9E82");
   }else if(look==ContainerLook.KitchenCupboard){
    var body=B("Body",new Vector3(0,1.67f,-.02f),new Vector3(1.04f,.72f,.43f),"AAA88B",true);SceneObstacle.Apply(body,ObstacleKind.High);
    Part("CabinetInterior",0,1.67f,-.245f,.93f,.62f,.02f,"4F6152");Part("InnerShelf",0,1.54f,-.255f,.91f,.025f,.025f,"C0B998");
    Part("CupA",-.24f,1.72f,-.264f,.10f,.14f,.035f,"B5AD8B");Part("CupB",.13f,1.72f,-.264f,.10f,.14f,.035f,"8EAA9A");
    Pivot(new Vector3(-.53f,1.67f,-.275f),new Vector3(0,72,0));
    AttachLast(B("EnamelCupboardDoor",new Vector3(0,1.67f,-.275f),new Vector3(1.02f,.70f,.045f),"BBB59B"));
    AttachLast(B("RecessedPanel",new Vector3(0,1.67f,-.306f),new Vector3(.81f,.48f,.022f),"9C9F83"));
    AttachLast(B("CupboardHandle",new Vector3(.39f,1.58f,-.34f),new Vector3(.045f,.16f,.07f),"526B59"));
    Part("TopTrim",0,2.06f,-.015f,1.11f,.055f,.49f,"6F806A");
   }else{
    Solid(.29f,1.04f,.58f,.74f,ObstacleKind.Low,"737D65");Part("InsideBox",0,.59f,0,.91f,.025f,.61f,"314A3C");
    foreach(float x in new[]{-.48f,.48f})Part("CarryHandle",x,.34f,-.03f,.065f,.08f,.25f,"B3A789");
    Pivot(new Vector3(0,.62f,.39f),new Vector3(100,0,0));
    AttachLast(B("StorageLid",new Vector3(0,.62f,0),new Vector3(1.10f,.07f,.81f),"98977A"));
    foreach(float x in new[]{-.32f,.32f})AttachLast(B("LidStrap",new Vector3(x,.665f,0),new Vector3(.065f,.025f,.82f),"4E6350"));
    Part("FrontLatch",0,.48f,-.405f,.12f,.17f,.045f,"B5A681");Part("LabelPatch",.25f,.29f,-.379f,.22f,.13f,.012f,"C0B792");
   }
  }
 }
}
