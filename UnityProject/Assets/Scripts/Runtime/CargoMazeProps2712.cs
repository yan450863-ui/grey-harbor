using UnityEngine;
namespace GreyHarbor {
 public sealed partial class RaidMap {
  GameObject CargoPart2712(Transform root,string name,Vector3 center,Vector3 size,string color,bool physical=false,ObstacleKind kind=ObstacleKind.High){var part=Box(name,root.position+center,size,color,physical);part.transform.SetParent(root,true);if(physical)SceneObstacle.Apply(part,kind);return part;}
  void CargoPropAdd2712(string type,float x,float z,float yaw){
   var root=new GameObject("CargoWhitebox_"+type+"_"+CargoProps2712.Count).transform;root.SetParent(Root,false);root.position=PortPoint(x,z);Vector3 size;ObstacleKind kind=ObstacleKind.High;
   if(type=="Forklift"){
    size=new Vector3(3.83f,2.75f,1.72f);
    CargoPart2712(root,"ForkliftBody",new Vector3(-.565f,.55f,0),new Vector3(2.7f,1.1f,1.72f),"AAA898",true);
    CargoPart2712(root,"ForkliftCabin",new Vector3(-.70f,1.7f,0),new Vector3(1.35f,1.2f,1.4f),"C4C5B9");
    CargoPart2712(root,"ForkliftMast",new Vector3(.915f,1.55f,0),new Vector3(.22f,2.4f,1.4f),"827F70",true);
    foreach(float side in new[]{-.55f,.55f})CargoPart2712(root,"ForkliftFork",new Vector3(1.365f,.14f,side),new Vector3(1.1f,.10f,.16f),"8B8777",true,ObstacleKind.Low);
    foreach(float wheelX in new[]{-1.35f,.34f})foreach(float side in new[]{-.73f,.73f})CargoPart2712(root,"ForkliftWheel",new Vector3(wheelX,.30f,side),new Vector3(.54f,.54f,.26f),"696C62");
   }else if(type=="CargoLoad"){
    size=new Vector3(3,2.05f,2.2f);
    CargoPart2712(root,"CargoPallet",new Vector3(0,.06f,0),new Vector3(3,.12f,2.2f),"8F8876",true,ObstacleKind.Low);
    CargoPart2712(root,"CargoLoad",new Vector3(0,1.085f,0),new Vector3(3,1.93f,2.2f),"B9B6A5",true);
    foreach(float band in new[]{-.8f,.8f})CargoPart2712(root,"CargoLoadBand",new Vector3(band,1.085f,0),new Vector3(.06f,1.96f,2.22f),"928E7E");
   }else if(type=="Barrels"){
    size=new Vector3(2.1f,1.05f,1.5f);kind=ObstacleKind.Low;
    foreach(float bx in new[]{-.725f,.725f})foreach(float bz in new[]{-.425f,.425f}){
     var drum=GameObject.CreatePrimitive(PrimitiveType.Cylinder);drum.name="CargoOilBarrel";drum.transform.SetParent(root,false);drum.transform.localPosition=new Vector3(bx,.525f,bz);drum.transform.localScale=new Vector3(.65f,.525f,.65f);drum.GetComponent<Renderer>().sharedMaterial=Mat("A29A86",true);SceneObstacle.Apply(drum,ObstacleKind.Low);
    }
   }else{
    size=new Vector3(2.1f,3.3f,1.65f);
    CargoPart2712(root,"HighServiceEquipment",new Vector3(0,1.65f,0),size,"999F95",true);
    CargoPart2712(root,"HighEquipmentPanel",new Vector3(0,1.45f,-.835f),new Vector3(1.6f,1.35f,.02f),"B9BDAF");
   }
   root.rotation=Quaternion.Euler(0,yaw,0);var bounds=new Bounds(root.position,Vector3.zero);bool first=true;
   foreach(var renderer in root.GetComponentsInChildren<Renderer>()){if(first){bounds=renderer.bounds;first=false;}else bounds.Encapsulate(renderer.bounds);}
   CargoProps2712.Add(new CargoProp2712{Type=type,View=root.gameObject,Center=root.position,Size=size,Yaw=yaw,Footprint=Rect.MinMaxRect(bounds.min.x,bounds.min.z,bounds.max.x,bounds.max.z),Kind=kind});
  }
 }
}
