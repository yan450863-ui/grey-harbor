using UnityEngine;
namespace GreyHarbor {
 public enum ContainerLook { Suitcase,Coat,Toolbox,MedicalCabinet,FileCabinet,SupplyLocker,Safe,AmmoCase,MilitaryCase,RollingSuitcase,BedsideDrawer,Wardrobe,KitchenCupboard,StorageBin,JewelryBox,PrecisionCase,ArchiveCase,OutdoorTote,GardenChest,CargoCrate,ElectricalCabinet,FuelDrum,MarineCooler,FishingChest,WaterproofTransitCase,WeldingToolbox,RescueCabinet,MarineControlBox,DocumentTube }
 public sealed class ContainerProp:MonoBehaviour {
  public ContainerLook Look;public Transform Opening;public Vector3 OpenAngles;public Vector3 OpenOffset;public bool Searched;
  public void SetSearched(){if(Searched)return;Searched=true;if(Opening){Opening.localRotation=Quaternion.Euler(OpenAngles);Opening.localPosition+=OpenOffset;}}
 }
}
