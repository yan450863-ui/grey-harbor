using UnityEngine;
namespace GreyHarbor {
 public sealed partial class RaidMap {
  void PortInteriorContainers(bool booth){
   var rng=new System.Random(RaidGame.Instance.RaidSeed^(booth?13201:13202));
   if(booth){
    AddContainer(new Vector3(-2.4f,0,.8f),new Vector3(-2.4f,.925f,1.9f),ContainerLook.Suitcase,SupplyType.Misc,rng,HarborDistrict.Office,0,false,ContainerTier.Common);
    AddContainer(new Vector3(2.65f,0,1.15f),new Vector3(3.8f,0,1.15f),ContainerLook.Coat,SupplyType.Misc,rng,HarborDistrict.Office,90,false,ContainerTier.Common);
    AddContainer(new Vector3(2.6f,0,-2.05f),new Vector3(3.8f,0,-2.05f),ContainerLook.MedicalCabinet,SupplyType.Medical,rng,HarborDistrict.Office,90,false,ContainerTier.Common);
   }else{
    AddContainer(new Vector3(-6.5f,0,-4.4f),new Vector3(-6.5f,.925f,-5.6f),ContainerLook.Suitcase,SupplyType.Misc,rng,HarborDistrict.Office,180,false,ContainerTier.Common);
    AddContainer(new Vector3(-6.9f,0,-1.3f),new Vector3(-7.8f,0,-.55f),ContainerLook.FileCabinet,SupplyType.Misc,rng,HarborDistrict.Office,0,false,ContainerTier.Common);
    AddContainer(new Vector3(5,0,-4.85f),new Vector3(5,.67f,-5.9f),ContainerLook.Suitcase,SupplyType.Misc,rng,HarborDistrict.Office,180,false,ContainerTier.Common);
    AddContainer(new Vector3(-5.6f,0,2.4f),new Vector3(-5.6f,.925f,1.2f),ContainerLook.Toolbox,SupplyType.Tools,rng,HarborDistrict.Repair,180,false,ContainerTier.Common);
    AddContainer(new Vector3(-6.1f,0,5.3f),new Vector3(-6.1f,0,6.45f),ContainerLook.SupplyLocker,SupplyType.Tools,rng,HarborDistrict.Loading,0,false,ContainerTier.Common);
    AddContainer(new Vector3(7.55f,0,4.35f),new Vector3(8.8f,0,4.35f),ContainerLook.MedicalCabinet,SupplyType.Medical,rng,HarborDistrict.Office,90,false,ContainerTier.Common);
   }
  }
 }
 public sealed partial class RaidGame {
  void PopulatePortInteriorMonsters(){
   if(Map.InteriorId=="portbooth"){
    InteriorMonster("港口值班室",MonsterKind.Wanderer,1.4f,1.5f,1.4f,2.4f,2.4f,1.6f);
   }else if(Map.InteriorId=="portservice"){
    InteriorMonster("附属房接待室",MonsterKind.Wanderer,-5.5f,-2.2f,-4.2f,-2.2f,-4.2f,-4.1f);
    InteriorMonster("附属房休息室",MonsterKind.Runner,4.2f,-1.7f,5.5f,-1.7f,3.4f,-3.5f);
    InteriorMonster("附属房洗手间",MonsterKind.Spitter,4.3f,1.7f,3.5f,1.2f,5.3f,2.9f);
   }
  }
 }
}
