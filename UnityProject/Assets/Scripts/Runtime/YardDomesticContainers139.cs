using UnityEngine;
namespace GreyHarbor {
 public sealed partial class RaidMap {
  void YardDomesticContainers139(bool home){
   var rng=new System.Random(RaidGame.Instance.RaidSeed^(home?13901:13902));
   if(home){
    YardContainer139("起居室边柜抽屉",new Vector3(-5.3f,0,-.95f),new Vector3(-6.3f,0,-.95f),ContainerLook.BedsideDrawer,SupplyType.Misc,rng,-90);
    YardContainer139("床头抽屉",new Vector3(-5.8f,0,.65f),new Vector3(-5.8f,0,1.65f),ContainerLook.BedsideDrawer,SupplyType.Misc,rng);
    YardContainer139("卧室旧衣柜",new Vector3(-5.3f,0,3.7f),new Vector3(-6.3f,0,3.7f),ContainerLook.Wardrobe,SupplyType.Misc,rng,-90);
    YardContainer139("厨房壁柜",new Vector3(5.1f,0,-3.7f),new Vector3(6.45f,0,-3.7f),ContainerLook.KitchenCupboard,SupplyType.Misc,rng,90);
    YardContainer139("生活储物箱",new Vector3(4.4f,0,-.15f),new Vector3(4.4f,0,.65f),ContainerLook.StorageBin,SupplyType.Misc,rng);
    YardContainer139("家庭药柜",new Vector3(5.85f,0,.9f),new Vector3(6.8f,0,1),ContainerLook.MedicalCabinet,SupplyType.Medical,rng,90);
   }else{
    YardContainer139("园艺工具箱",new Vector3(-3.2f,0,2.25f),new Vector3(-3.2f,.93f,3.5f),ContainerLook.Toolbox,SupplyType.Tools,rng);
    YardContainer139("园艺用品箱",new Vector3(-3.5f,0,-2.65f),new Vector3(-3.5f,0,-3.55f),ContainerLook.StorageBin,SupplyType.Tools,rng,180);
    YardContainer139("洗涤用品壁柜",new Vector3(3.35f,0,-2.75f),new Vector3(4.85f,0,-2.5f),ContainerLook.KitchenCupboard,SupplyType.Misc,rng,90);
    YardContainer139("杂物储物箱",new Vector3(3.8f,0,2),new Vector3(3.8f,0,3),ContainerLook.StorageBin,SupplyType.Misc,rng);
   }
  }
  void YardContainer139(string name,Vector3 stand,Vector3 model,ContainerLook look,SupplyType type,System.Random rng,float yaw=0){
   var loot=AddContainer(stand,model,look,type,rng,HarborDistrict.Entrance,yaw,false,ContainerTier.Common);loot.Name=name;
  }
 }
}
