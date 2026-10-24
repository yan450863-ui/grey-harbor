using UnityEngine;
namespace GreyHarbor {
 public sealed partial class ContainerArt {
  void DockContainerArt141(ContainerLook look){
   if(look==ContainerLook.RescueCabinet||look==ContainerLook.MarineControlBox){
    bool rescue=look==ContainerLook.RescueCabinet;float w=rescue?.85f:1.05f,h=rescue?1.6f:1.85f,d=.52f;Solid(h/2,w,h,d,ObstacleKind.High,rescue?"B67B53":"647D80");Part("WeatherFoot",0,.06f,0,w+.16f,.12f,d+.12f,"79857B");Part("CabinetInside",0,h/2,-d/2-.015f,w-.13f,h-.18f,.025f,"2C4849");
    Pivot(new Vector3(-w/2,h/2,-d/2-.05f),new Vector3(0,86,0));AttachLast(B("MarineDoor",new Vector3(0,h/2,-d/2-.055f),new Vector3(w-.02f,h-.10f,.055f),rescue?"D19062":"7D9697"));AttachLast(B("DoorLatch",new Vector3(w*.34f,h*.48f,-d/2-.11f),new Vector3(.055f,.23f,.075f),"BBC1AE"));
    Part("DripHood",0,h+.04f,-.03f,w+.18f,.07f,d+.16f,rescue?"93684D":"8C9D98");
    if(rescue){AttachLast(B("RescuePanel",new Vector3(0,h*.70f,-d/2-.09f),new Vector3(.42f,.38f,.02f),"C9C8A9"));AttachLast(B("CrossBar",new Vector3(0,h*.70f,-d/2-.111f),new Vector3(.28f,.075f,.02f),"A95B46"));AttachLast(B("CrossStem",new Vector3(0,h*.70f,-d/2-.112f),new Vector3(.075f,.28f,.02f),"A95B46"));for(int i=0;i<3;i++)Part("InsideShelf",0,.35f+i*.4f,-.285f,.68f,.04f,.09f,"A6B19C");}
    else{for(int i=0;i<3;i++){AttachLast(B("DialWindow",new Vector3(-.27f+i*.27f,h*.72f,-.32f),new Vector3(.16f,.16f,.025f),"B5B8A0"));AttachLast(B("DialNeedle",new Vector3(-.27f+i*.27f,h*.72f,-.341f),new Vector3(.02f,.11f,.017f),"455D5A"));Part("RelayUnit",-.23f+i*.24f,.75f,-.285f,.14f,.25f,.055f,"A7A99B");}for(int i=0;i<4;i++)AttachLast(B("Vent",new Vector3(0,.34f+i*.075f,-.30f),new Vector3(.65f,.025f,.025f),"405F60"));}
   }else if(look==ContainerLook.DocumentTube){
    Solid(.34f,.40f,.68f,.40f,ObstacleKind.Low,"8A997E");root.Find("Body").GetComponent<Renderer>().enabled=false;
    DrumCylinder140("SealedTube",new Vector3(0,.33f,0),new Vector3(.34f,.33f,.34f),"748D80");DrumCylinder140("TubeBottom",new Vector3(0,.025f,0),new Vector3(.39f,.025f,.39f),"435F59");
    Pivot(new Vector3(0,.7f,.20f),new Vector3(104,0,0));AttachLast(DrumCylinder140("ThreadedCap",new Vector3(0,.70f,0),new Vector3(.41f,.045f,.41f),"A2AF97"));Part("DocumentLabel",0,.40f,-.178f,.18f,.22f,.024f,"C1BDA0");Part("CarryStrap",.23f,.36f,0,.035f,.43f,.035f,"465E52");
   }else{
    bool cooler=look==ContainerLook.MarineCooler,fish=look==ContainerLook.FishingChest,weld=look==ContainerLook.WeldingToolbox;
    float w=cooler?1.1f:fish?1.7f:weld?.92f:1.25f,h=cooler?.62f:fish?.42f:weld?.31f:.62f,d=cooler?.72f:fish?.55f:weld?.58f:.85f;
    string body=cooler?"A5AD94":fish?"638085":weld?"875F4D":"4C6F72",lid=cooler?"D0CEAE":fish?"809796":weld?"B38862":"698C85";
    Solid(h/2,w,h,d,ObstacleKind.Low,body);Part("DeepInterior",0,h+.01f,0,w-.10f,.022f,d-.10f,cooler?"7A8C7D":"2C4847");
    Pivot(new Vector3(0,h+.03f,d/2),new Vector3(105,0,0));AttachLast(B("SealedLid",new Vector3(0,h+.035f,0),new Vector3(w+.05f,cooler?.12f:.065f,d+.05f),lid));
    if(cooler){foreach(float x in new[]{-w/2,w/2})Part("SideLiftGrip",x,h*.57f,0,.055f,.12f,.28f,"425F5A");for(int i=0;i<4;i++)Part("InsulatedRib",-.37f+i*.24f,h*.35f,-d/2-.019f,.025f,.27f,.024f,"7B9384");}
    else if(fish){Part("RustPatch",-.37f,.15f,-d/2-.025f,.24f,.11f,.017f,"9C7657");Part("LineLabel",.39f,.22f,-d/2-.026f,.24f,.10f,.02f,"B8B499");}
    else if(weld){AttachLast(B("ToolHandle",new Vector3(0,h+.14f,0),new Vector3(.31f,.09f,.07f),"4A594D"));Part("BurnMark",.28f,.14f,-d/2-.02f,.24f,.10f,.015f,"4C4E43");}
    else{foreach(float x in new[]{-w*.39f,w*.39f})foreach(float z in new[]{-d*.38f,d*.38f})Part("TransportCorner",x,.12f,z,.19f,.24f,.19f,"87978A");AttachLast(B("TransportStencil",new Vector3(0,h+.077f,0),new Vector3(.38f,.02f,.22f),"ABA88A"));}
    foreach(float x in new[]{-w*.32f,w*.32f}){Part("LockClasp",x,h*.71f,-d/2-.045f,.07f,.16f,.04f,"B9BA9A");AttachLast(B("LidSealBand",new Vector3(x,h+.10f,0),new Vector3(.038f,.018f,d+.07f),"4F6A5E"));}Part("FrontCarryGrip",0,h*.48f,-d/2-.077f,.27f,.06f,.07f,"334F4D");
   }
  }
 }
}
