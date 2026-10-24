using UnityEngine;
namespace GreyHarbor {
 public static class HighValueContainers139 {
  public static CollectionCategory? Category(ContainerLook look){
   switch(look){case ContainerLook.JewelryBox:return CollectionCategory.Collectibles;case ContainerLook.PrecisionCase:return CollectionCategory.Electronics;case ContainerLook.ArchiveCase:return CollectionCategory.Information;default:return null;}
  }
 }
 public sealed partial class ContainerArt {
  void HighValueContainer139(ContainerLook look){
   bool jewel=look==ContainerLook.JewelryBox,precision=look==ContainerLook.PrecisionCase;
   float w=jewel?.64f:precision?1.1f:.95f,d=jewel?.46f:precision?.72f:.58f,h=jewel?.25f:precision?.48f:.37f;
   string c=jewel?"795F45":precision?"556D65":"777D66";
   Solid(h/2,w,h,d,ObstacleKind.Low,c);Part("DarkLinedInterior",0,h+.012f,0,w-.07f,.025f,d-.07f,jewel?"533C38":"263F38");
   Pivot(new Vector3(0,h,d/2),new Vector3(105,0,0));
   AttachLast(B("SpecialCaseLid",new Vector3(0,h+.04f,0),new Vector3(w+.025f,.065f,d+.025f),c));
   foreach(float x in new[]{-w*.38f,w*.38f}){
    Part("BrassClasp",x,h*.76f,-d/2-.022f,.055f,.10f,.035f,jewel?"C6AC70":"A4AD95");
    AttachLast(B("ReinforcedBand",new Vector3(x,h+.084f,0),new Vector3(.04f,.022f,d+.04f),jewel?"B29963":"3C5149"));
   }
   if(jewel){
    AttachLast(B("LidOrnament",new Vector3(0,h+.084f,0),new Vector3(.19f,.026f,.14f),"C1A56A"));
    foreach(float x in new[]{-.18f,.18f})Part("VelvetCompartment",x,h+.031f,0,.22f,.018f,.25f,"80644E");
   }else if(precision){
    foreach(float x in new[]{-w/2,w/2})foreach(float z in new[]{-d/2,d/2})Part("CaseCorner",x,h/2,z,.09f,h+.035f,.09f,"334C43");
    Part("CarryGrip",0,h*.55f,-d/2-.08f,.33f,.07f,.07f,"273E36");
    AttachLast(B("EquipmentPlate",new Vector3(0,h+.083f,0),new Vector3(.34f,.025f,.23f),"9AA992"));
    foreach(float x in new[]{-.23f,.23f})Part("FoamSlot",x,h+.028f,0,.27f,.028f,.40f,"42534A");
   }else{
    AttachLast(B("ArchiveSeal",new Vector3(0,h+.09f,0),new Vector3(.16f,.025f,d+.025f),"AA7660"));
    AttachLast(B("DocumentPlate",new Vector3(.22f,h+.085f,0),new Vector3(.23f,.023f,.15f),"B7B294"));
    foreach(float z in new[]{-.13f,.13f})Part("DocumentFolders",0,h+.03f,z,w-.17f,.028f,.15f,"B0A88A");
    Part("BriefcaseGrip",0,h*.6f,-d/2-.07f,.26f,.055f,.055f,"3B5143");
   }
  }
 }
}
