using UnityEngine;
namespace GreyHarbor {
 public sealed partial class RaidMap {
  // Resolve declared map/scene identity and fixed district bounds; never nearest-building distance.
  DropRegion230 DropRegionFor230(Vector3 position,HarborDistrict district){
   if(IsInterior){
    if(InteriorId=="yardhome"||InteriorId=="yardshed")return DropRegion230.Yard;
    if(InteriorId=="ferry1"||InteriorId=="ferry2")return DropRegion230.PassengerDock;
    if(InteriorId=="portbooth"||InteriorId=="portservice")return DropRegion230.Entrance;
    if(InteriorId=="repairannex"||InteriorId.StartsWith("repairmain"))return DropRegion230.Repair;
    return DropRegion230.Training;
   }
   if(ExteriorPreview)return DropRegion230.Training;
   if(HarborWhitebox&&LogisticsYardBounds.Contains(new Vector2(position.x,position.z)))return DropRegion230.Yard;
   switch(district){
    case HarborDistrict.Entrance:return DropRegion230.Entrance;
    case HarborDistrict.Repair:return DropRegion230.Repair;
    case HarborDistrict.Containers:return DropRegion230.Containers;
    case HarborDistrict.Loading:return DropRegion230.Loading;
    case HarborDistrict.Office:return DropRegion230.Office;
    case HarborDistrict.Coast:
     if(!HarborWhitebox)return DropRegion230.PassengerDock;
     float portZ=position.z+73;
     return portZ>=115?DropRegion230.FishingDock:portZ>=50?DropRegion230.PassengerDock:DropRegion230.FreightDock;
    default:throw new System.ArgumentOutOfRangeException(nameof(district));
   }
  }
 }
}
