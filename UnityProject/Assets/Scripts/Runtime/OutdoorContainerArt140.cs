using UnityEngine;
namespace GreyHarbor {
 public sealed partial class ContainerArt {
  GameObject DrumCylinder140(string name,Vector3 p,Vector3 size,string color){
   var g=GameObject.CreatePrimitive(PrimitiveType.Cylinder);g.name=name;g.transform.SetParent(root,false);g.transform.localPosition=p;g.transform.localScale=size;g.GetComponent<Renderer>().sharedMaterial=map.Mat(color,true);
   if(Application.isPlaying)Object.Destroy(g.GetComponent<Collider>());else Object.DestroyImmediate(g.GetComponent<Collider>());return g;
  }
  void OutdoorContainer140(ContainerLook look){
   if(look==ContainerLook.ElectricalCabinet){
    Solid(.87f,.88f,1.74f,.52f,ObstacleKind.High,"60796F");Part("ConcreteFoot",0,.07f,0,1.02f,.14f,.64f,"8A927F");
    Part("PanelInterior",0,.95f,-.278f,.73f,1.49f,.027f,"2D443B");
    for(int i=0;i<4;i++){Part("Breaker",-.16f,.57f+i*.23f,-.30f,.18f,.12f,.04f,"98A08A");Part("Cable",.12f,.63f+i*.23f,-.307f,.035f,.20f,.023f,"AB9261");}
    Pivot(new Vector3(-.45f,.95f,-.31f),new Vector3(0,82,0));AttachLast(B("WeatherproofDoor",new Vector3(0,.95f,-.32f),new Vector3(.86f,1.54f,.055f),"7F9480"));
    AttachLast(B("WarningPlate",new Vector3(0,1.30f,-.36f),new Vector3(.30f,.23f,.025f),"BAA470"));
    AttachLast(B("WarningSlash",new Vector3(0,1.30f,-.38f),new Vector3(.035f,.15f,.015f),"3C5143"));
    AttachLast(B("CabinetPull",new Vector3(.32f,.89f,-.37f),new Vector3(.04f,.20f,.065f),"B4B49B"));
    for(int i=0;i<4;i++)AttachLast(B("VentSlot",new Vector3(0,.42f+i*.055f,-.36f),new Vector3(.43f,.02f,.018f),"344E42"));
    Part("RainHood",0,1.80f,-.025f,1.03f,.075f,.64f,"809080");
   }else if(look==ContainerLook.FuelDrum){
    Solid(.535f,.74f,1.07f,.74f,ObstacleKind.Low,"64796B");root.Find("Body").GetComponent<Renderer>().enabled=false;
    DrumCylinder140("PaintedDrum",new Vector3(0,.53f,0),new Vector3(.70f,.53f,.70f),"738879");
    foreach(float y in new[]{.12f,.53f,.98f})DrumCylinder140("DrumBand",new Vector3(0,y,0),new Vector3(.74f,.025f,.74f),"3F594B");
    DrumCylinder140("ContentsSurface",new Vector3(0,1.075f,0),new Vector3(.57f,.009f,.57f),"2B4337");
    Pivot(new Vector3(0,1.10f,.35f),new Vector3(102,0,0));AttachLast(DrumCylinder140("DrumCap",new Vector3(0,1.11f,0),new Vector3(.75f,.025f,.75f),"8C9C83"));
    AttachLast(B("CapGrip",new Vector3(0,1.16f,-.10f),new Vector3(.19f,.05f,.06f),"C0B593"));
    Part("ChemicalLabel",0,.58f,-.36f,.26f,.21f,.025f,"C0AA76");Part("HazardStripe",0,.58f,-.378f,.055f,.14f,.012f,"425E4B");
   }else{
    bool tote=look==ContainerLook.OutdoorTote,garden=look==ContainerLook.GardenChest;
    float w=tote?.93f:garden?1.65f:1.28f,h=tote?.55f:garden?.66f:.86f,d=tote?.66f:garden?.65f:.95f;
    string c=tote?"687F79":garden?"7E8664":"8F7B58";Solid(h/2,w,h,d,ObstacleKind.Low,c);Part("InnerBox",0,h+.012f,0,w-.09f,.023f,d-.09f,"2F483B");
    if(tote){for(int i=0;i<4;i++)foreach(float x in new[]{-w/2,w/2})Part("MouldedRib",x,h*.45f,-d*.36f+i*d*.24f,.027f,h*.63f,.043f,"425E54");}
    if(!tote){for(int i=0;i<4;i++){float y=.12f+i*(h-.14f)/4;Part("TimberBoard",0,y,-d/2-.012f,w,.13f,.04f,i%2==0?"A18C63":"88754F");}foreach(float x in new[]{-w*.35f,w*.35f})Part("Reinforcement",x,h/2,-d/2-.046f,.08f,h+.035f,.035f,"566851");}
    Pivot(new Vector3(0,h+.035f,d/2),new Vector3(103,0,0));AttachLast(B("OutdoorLid",new Vector3(0,h+.04f,0),new Vector3(w+.045f,.075f,d+.045f),tote?"98A792":"A49672"));
    foreach(float x in new[]{-w*.32f,w*.32f}){
     AttachLast(B("LidBand",new Vector3(x,h+.087f,0),new Vector3(.055f,.021f,d+.055f),tote?"5A7568":"5C6B4C"));
     Part("Clasp",x,h*.78f,-d/2-.051f,.08f,.14f,.04f,"B7B190");
    }
    Part("GrabHandle",0,h*.53f,-d/2-.077f,.26f,.055f,.055f,"354D40");
    if(!tote)Part("PackingLabel",w*.22f,h*.44f,-d/2-.04f,.23f,.15f,.02f,"C0B08D");
   }
  }
 }
}
