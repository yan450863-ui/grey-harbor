using UnityEngine;
namespace GreyHarbor {
 public sealed partial class ContainerArt {
  void RollingCase(){
   var body=B("CaseBody",new Vector3(0,.65f,0),new Vector3(.66f,1.08f,.40f),"68766D",true);SceneObstacle.Apply(body,ObstacleKind.Low);
   Part("DarkLuggageInterior",0,.65f,-.211f,.55f,.95f,.023f,"2E3C36");
   Part("InteriorLining",0,.65f,-.225f,.46f,.84f,.025f,"9A9B82");
   Pivot(new Vector3(-.345f,.65f,-.24f),new Vector3(0,-95,0));
   AttachLast(B("FrontShell",new Vector3(0,.65f,-.25f),new Vector3(.68f,1.10f,.065f),"839083"));
   foreach(float x in new[]{-.235f,-.12f,0,.12f,.235f})AttachLast(B("ShellRib",new Vector3(x,.65f,-.294f),new Vector3(.038f,.86f,.025f),"A0A58D"));
   foreach(float x in new[]{-.33f,.33f}){AttachLast(B("ZipBorder",new Vector3(x,.65f,-.299f),new Vector3(.025f,1.04f,.018f),"3B4C44"));}
   foreach(float y in new[]{.13f,1.17f})AttachLast(B("ZipBorder",new Vector3(0,y,-.299f),new Vector3(.65f,.025f,.018f),"3B4C44"));
   AttachLast(B("ZipPull",new Vector3(.25f,.91f,-.321f),new Vector3(.032f,.12f,.022f),"C0B794"));
   AttachLast(B("LuggageTag",new Vector3(.12f,1.03f,-.328f),new Vector3(.17f,.12f,.015f),"C5B99A"));
   foreach(float x in new[]{-.26f,.26f})foreach(float z in new[]{-.15f,.15f}){Part("WheelHousing",x,.115f,z,.16f,.15f,.14f,"46554C");Part("RollingWheel",x,.058f,z,.12f,.115f,.115f,"293A33");Part("WheelHub",x+Mathf.Sign(x)*.065f,.058f,z,.012f,.055f,.055f,"929783");}
   foreach(float x in new[]{-.16f,.16f}){Part("HandleSleeve",x,1.14f,.18f,.07f,.15f,.08f,"3C5047");Part("TelescopicRail",x,1.43f,.18f,.04f,.69f,.04f,"ADB3A1");}
   Part("PullHandle",0,1.79f,.18f,.40f,.085f,.08f,"2D4138");
   foreach(float x in new[]{-.12f,.12f})Part("CarryHandleMount",x,1.22f,-.03f,.035f,.08f,.06f,"405249");
   Part("TopCarryHandle",0,1.27f,-.03f,.29f,.035f,.06f,"2C3D35");
   Part("SideCarryHandle",.351f,.76f,.02f,.045f,.28f,.09f,"33453B");
  }
 }
}
