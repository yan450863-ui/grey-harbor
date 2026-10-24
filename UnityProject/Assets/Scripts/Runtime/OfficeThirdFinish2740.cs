using UnityEngine;
namespace GreyHarbor {public sealed partial class RaidMap {
 public GameObject OfficeThirdCorridorFurniture2740;
 public WorldDoor OfficeResearchDoor2740;
 void LockOfficeResearch2740(){OfficeResearchDoor2740=OfficeOpenings2726.Find(o=>o.Door!=null&&o.X==2&&o.Z==19).Door;OfficeResearchDoor2740.Locked=true;OfficeResearchDoor2740.Name="项目研究室门 · 需要钥匙";}
 void BuildOfficeThirdBarrier2740(){
  OfficeThirdCorridorFurniture2740=new GameObject("OfficeThirdCollapsedArchiveObstruction2740");OfficeThirdCorridorFurniture2740.transform.SetParent(Root,false);
  foreach(float sign in new[]{-1f,1f}){var shelf=new FutureAssetKit2717(this).Build(FutureProp2717.OpenArchiveShelf,new Vector3(sign*.98f,0,3),0);shelf.transform.rotation=Quaternion.Euler(0,0,sign*90);Physics.SyncTransforms();var body=shelf.GetComponentInChildren<Collider>();shelf.transform.position+=Vector3.right*(sign*.98f-body.bounds.center.x);FreightPlaceOnFloor2717(shelf);shelf.transform.SetParent(OfficeThirdCorridorFurniture2740.transform,true);}
  OfficePanel2736("01",new Vector3(-7,2.15f,-23.82f),180,"Window");OfficePanel2736("02",new Vector3(-19.82f,2.15f,-7.5f),270,"Window");OfficePanel2736("03",new Vector3(15,2.15f,-23.82f),180,"Window");OfficePanel2736("04",new Vector3(-19.82f,2.15f,7),270,"Window");OfficePanel2736("05",new Vector3(-7.82f,1.55f,.5f),270,"Mirror");
  var art=new WorkshopWastelandArt(this);foreach(float x in new[]{-1.4f,-.4f,.8f,1.5f}){art.Cube("FallenAdministrativePaper2740","D5C7A2",new Vector3(x,.04f,2.55f),new Vector3(.25f,.025f,.16f),false);art.Cube("FallenAdministrativeBinder2740",x<0?"547D99":"708969",new Vector3(x,.08f,3.5f),new Vector3(.24f,.1f,.16f),false);}art.Bake(OfficeThirdCorridorFurniture2740,OfficeThirdCorridorFurniture2740.AddComponent<WorkshopArtResources>());Physics.SyncTransforms();
 }
}}
