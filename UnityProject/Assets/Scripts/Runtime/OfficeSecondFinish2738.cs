using UnityEngine;
namespace GreyHarbor {public sealed partial class RaidMap {
 public GameObject OfficeSecondCentreCabinets2738,OfficeSecondWestCabinets2738;
 void OfficeSecondBarrierPart2738(GameObject root,FutureProp2717 prop,Vector3 p,float yaw){var g=new FutureAssetKit2717(this).Build(prop,p,yaw);FreightPlaceOnFloor2717(g);g.transform.SetParent(root.transform,true);}
 void BuildOfficeSecondBarrier2738(){
  OfficeSecondCentreCabinets2738=new GameObject("OfficeSecondCentralCabinetObstruction2738");OfficeSecondCentreCabinets2738.transform.SetParent(Root,false);
  var shelf=new FutureAssetKit2717(this).Build(FutureProp2717.OpenArchiveShelf,new Vector3(-1.2f,0,5),0);
  shelf.transform.rotation=Quaternion.Euler(0,0,90);Physics.SyncTransforms();var body=shelf.GetComponentInChildren<Collider>();shelf.transform.position+=Vector3.right*(-1.2f-body.bounds.center.x);FreightPlaceOnFloor2717(shelf);shelf.transform.SetParent(OfficeSecondCentreCabinets2738.transform,true);
  var art=new WorkshopWastelandArt(this);foreach(float z in new[]{4.45f,5.35f}){art.Cube("FallenOfficeFile2739","547D99",new Vector3(-.7f,.065f,z),new Vector3(.28f,.09f,.21f),false);art.Cube("LooseOfficePaper2739","D5C7A2",new Vector3(-1.5f,.035f,z),new Vector3(.28f,.025f,.20f),false);}art.Bake(OfficeSecondCentreCabinets2738,OfficeSecondCentreCabinets2738.AddComponent<WorkshopArtResources>());
  OfficeSecondWestCabinets2738=new GameObject("OfficeSecondWestLockerObstruction2738");OfficeSecondWestCabinets2738.transform.SetParent(Root,false);
  foreach(float z in new[]{11.5f,12.5f,13.5f})OfficeSecondBarrierPart2738(OfficeSecondWestCabinets2738,FutureProp2717.PersonalLocker,new Vector3(-11.4f,0,z),90);
  Physics.SyncTransforms();
 }
 void OfficeSecondWallDetails2738(){
  OfficePanel2736("01",new Vector3(-7,2.15f,-23.82f),180,"Window");OfficePanel2736("02",new Vector3(-19.82f,2.15f,-9),270,"Window");
  OfficePanel2736("03",new Vector3(6,2.15f,-23.82f),180,"Window");OfficePanel2736("03",new Vector3(16,2.15f,-23.82f),180,"Window");
  OfficePanel2736("04",new Vector3(-19.82f,2.15f,8.5f),270,"Window");OfficePanel2736("07",new Vector3(19.82f,2.15f,2),90,"Window");OfficePanel2736("10",new Vector3(4,2.15f,23.82f),0,"Window");
  OfficePanel2736("05",new Vector3(-5.82f,1.55f,.5f),270,"Mirror");OfficePanel2736("06",new Vector3(-5.82f,1.55f,7.5f),270,"Mirror");
 }
}}
