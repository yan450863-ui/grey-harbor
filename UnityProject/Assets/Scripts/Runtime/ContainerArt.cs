using UnityEngine;
namespace GreyHarbor {
 public sealed partial class ContainerArt {
  readonly RaidMap map;Transform root;ContainerProp prop;
  public ContainerArt(RaidMap map){this.map=map;}
  GameObject B(string n,Vector3 p,Vector3 s,string c,bool solid=false){var g=map.Box(n,root.position+p,s,c,solid);g.transform.SetParent(root,true);return g;}
  void Part(string n,float x,float y,float z,float w,float h,float d,string c){B(n,new Vector3(x,y,z),new Vector3(w,h,d),c);}
  void Solid(float y,float w,float h,float d,ObstacleKind kind,string color){var g=B("Body",new Vector3(0,y,0),new Vector3(w,h,d),color,true);SceneObstacle.Apply(g,kind);}
  void Table(){Part("TableTop",0,.72f,0,1.5f,.14f,.95f,"8F8165");foreach(float x in new[]{-.6f,.6f})foreach(float z in new[]{-.34f,.34f})Part("TableLeg",x,.34f,z,.10f,.68f,.10f,"485951");var g=B("FurnitureCollision",new Vector3(0,.36f,0),new Vector3(1.5f,.72f,.95f),"485951",true);g.GetComponent<Renderer>().enabled=false;SceneObstacle.Apply(g,ObstacleKind.Low);}
  void Pivot(Vector3 point,Vector3 angles){var g=new GameObject("OpenableLid");g.transform.SetParent(root,false);g.transform.localPosition=point;prop.Opening=g.transform;prop.OpenAngles=angles;}
  void AttachLast(GameObject g){g.transform.SetParent(prop.Opening,true);}
  public ContainerProp Build(ContainerLook look,Vector3 p,float yaw=0,bool support=true){
   root=new GameObject("Container_"+look).transform;root.SetParent(map.Root,false);root.position=p;prop=root.gameObject.AddComponent<ContainerProp>();prop.Look=look;
   if(DockContainers141.IsDock(look)){DockContainerArt141(look);}
   else if((int)look>=(int)ContainerLook.OutdoorTote){OutdoorContainer140(look);}
   else if((int)look>=(int)ContainerLook.BedsideDrawer){if((int)look>=(int)ContainerLook.JewelryBox)HighValueContainer139(look);else DomesticContainer139(look);}
   else if(look==ContainerLook.RollingSuitcase){RollingCase();}
   else if(look==ContainerLook.Suitcase||look==ContainerLook.Toolbox||look==ContainerLook.AmmoCase||look==ContainerLook.MilitaryCase){
    float floor=support?.80f:0;if(support)Table();bool suit=look==ContainerLook.Suitcase,tool=look==ContainerLook.Toolbox;float w=suit?.95f:tool?.78f:1.02f,d=suit?.64f:.48f,h=suit?.22f:.33f;string color=suit?"85735B":tool?"886353":look==ContainerLook.AmmoCase?"657453":"53696B";
    var body=B("CaseBody",new Vector3(0,floor+h/2,0),new Vector3(w,h,d),color,true);SceneObstacle.Apply(body,ObstacleKind.Low);
    Part("DarkInterior",0,floor+h+.012f,0,w-.08f,.02f,d-.08f,"283D38");Pivot(new Vector3(0,floor+h,d/2),new Vector3(100,0,0));AttachLast(B("CaseLid",new Vector3(0,floor+h+.035f,0),new Vector3(w+.035f,.065f,d+.035f),color));
    for(int side=-1;side<=1;side+=2){Part("Latch",side*w*.3f,floor+h*.7f,-d/2-.026f,.08f,.13f,.04f,"B9B59A");AttachLast(B("LidBand",new Vector3(side*w*.32f,floor+h+.075f,0),new Vector3(.06f,.025f,d+.045f),"485349"));}
    Part("Handle",0,floor+h*.65f,-d/2-.08f,.3f,.05f,.055f,"293C36");Part("HandleLeft",-.15f,floor+h*.65f,-d/2-.045f,.04f,.07f,.09f,"293C36");Part("HandleRight",.15f,floor+h*.65f,-d/2-.045f,.04f,.07f,.09f,"293C36");
    if(tool){Part("ToolEmblem",0,floor+h*.45f,-d/2-.031f,.16f,.045f,.014f,"C5AB73");}if(look==ContainerLook.AmmoCase)Part("AmmoStencil",0,floor+.12f,-d/2-.035f,.30f,.05f,.012f,"B6B58C");
   }else if(look==ContainerLook.Coat){
    if(support)B("MountingWall",new Vector3(0,1.2f,.16f),new Vector3(1.55f,2.4f,.18f),"858B7B",true);
    Part("CoatRail",0,1.95f,0,.9f,.1f,.1f,"796F54");Part("Hook",0,1.91f,-.10f,.055f,.18f,.13f,"B5B399");
    Part("CoatBody",0,1.35f,-.12f,.46f,.94f,.15f,"646E54");var arm=B("LeftSleeve",new Vector3(-.33f,1.43f,-.11f),new Vector3(.18f,.64f,.14f),"748063");arm.transform.localRotation=Quaternion.Euler(0,0,-18);arm=B("RightSleeve",new Vector3(.33f,1.43f,-.11f),new Vector3(.18f,.64f,.14f),"748063");arm.transform.localRotation=Quaternion.Euler(0,0,18);
    Part("Collar",0,1.81f,-.16f,.25f,.10f,.19f,"92957B");Part("ButtonSeam",0,1.34f,-.202f,.025f,.84f,.025f,"A3A185");foreach(float x in new[]{-.14f,.14f})Part("Pocket",x,1.21f,-.218f,.16f,.18f,.04f,"535F47");
   }else if(look==ContainerLook.MedicalCabinet){
    if(support)B("MountingWall",new Vector3(0,1.2f,.15f),new Vector3(1.6f,2.4f,.18f),"858B7B",true);
    var body=B("WallCabinet",new Vector3(0,1.4f,-.16f),new Vector3(.75f,.80f,.34f),"C4C6AC",true);SceneObstacle.Apply(body,ObstacleKind.High);Part("InnerCabinet",0,1.4f,-.34f,.65f,.69f,.035f,"495B52");Pivot(new Vector3(-.39f,1.4f,-.38f),new Vector3(0,80,0));AttachLast(B("MedicineDoor",new Vector3(0,1.4f,-.385f),new Vector3(.76f,.80f,.045f),"CDD0B7"));AttachLast(B("CrossHorizontal",new Vector3(0,1.48f,-.42f),new Vector3(.34f,.10f,.03f),"9B594B"));AttachLast(B("CrossVertical",new Vector3(0,1.48f,-.42f),new Vector3(.10f,.34f,.03f),"9B594B"));AttachLast(B("DoorHandle",new Vector3(.27f,1.28f,-.43f),new Vector3(.04f,.16f,.06f),"52675B"));
   }else{
    bool safe=look==ContainerLook.Safe,file=look==ContainerLook.FileCabinet;float height=safe?1.10f:file?1.4f:1.75f,width=safe?.93f:file?.78f:1.15f;string c=safe?"46595B":file?"737D70":"66735F";Solid(height/2,width,height,.65f,ObstacleKind.High,c);
    Part("Plinth",0,.07f,0,width+.09f,.14f,.73f,"3C5049");Part("DoorInset",0,height/2,-.337f,width-.08f,height-.12f,.025f,"2E423D");
    if(file){for(int i=0;i<4;i++){float y=.24f+i*.33f;Part("Drawer",0,y,-.366f,.70f,.29f,.045f,"879181");Part("DrawerPull",0,y-.035f,-.411f,.24f,.04f,.04f,"B8B9A0");Part("LabelHolder",0,y+.065f,-.395f,.18f,.07f,.017f,"B4AA85");}}
    else{Pivot(new Vector3(-width/2+.035f,height/2,-.37f),new Vector3(0,75,0));AttachLast(B("SteelDoor",new Vector3(0,height/2,-.38f),new Vector3(width-.07f,height-.11f,.075f),c));AttachLast(B("DoorPull",new Vector3(width*.30f,height*.51f,-.47f),new Vector3(.055f,.24f,.085f),"ADB399"));if(safe){AttachLast(B("CombinationDial",new Vector3(-.11f,height*.58f,-.455f),new Vector3(.19f,.19f,.055f),"B3B39A"));AttachLast(B("DialCenter",new Vector3(-.11f,height*.58f,-.489f),new Vector3(.07f,.07f,.025f),"43544C"));}else{for(int i=0;i<5;i++)AttachLast(B("Vent",new Vector3(0,height*.76f+i*.07f,-.425f),new Vector3(.52f,.025f,.018f),"31453B"));}}
   }
   root.rotation=Quaternion.Euler(0,yaw,0);return prop;
  }
 }
}
