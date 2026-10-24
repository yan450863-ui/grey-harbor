using UnityEngine;
using System.Collections.Generic;
namespace GreyHarbor {public sealed partial class RaidMap {
 public LootContainer OfficeSmallSafeBox2737;public GameObject OfficeCorridorFurniture2737;public readonly List<GameObject> OfficeBrokenWallModels2737=new List<GameObject>();
 void OfficeSmallSafe2737(System.Random rng){
  var model=new Vector3(13.25f,0,16);var stand=new Vector3(11.9f,0,16);var access=new Vector3(11.3f,0,16);
  var box=AddContainer(stand,model,ContainerLook.Safe,SupplyType.Safe,rng,HarborDistrict.Office,90,false,ContainerTier.Safe,DropRegion230.Office,SafeSize240.Small);box.Name="员工休息室小保险箱";FreightPlaceOnFloor2717(box.View);OfficeSmallSafeBox2737=box;
  OfficeSearchRecords2736.Add(new FreightLootRecord2717{Container=box,Model=box.View,Stand=stand,AccessPoint=access,RoomName="员工休息室",Yaw=90});
 }
 void BuildOfficeFirstBarrier2737(){
  OfficeCorridorFurniture2737=new GameObject("OfficeCorridorCollapsedArchiveFurniture2737");OfficeCorridorFurniture2737.transform.SetParent(Root,false);
  foreach(float sign in new[]{-1f,1f}){
   var shelf=new FutureAssetKit2717(this).Build(FutureProp2717.OpenArchiveShelf,new Vector3(sign*.98f,0,-11),0);shelf.transform.rotation=Quaternion.Euler(0,0,sign*90);Physics.SyncTransforms();var body=shelf.GetComponentInChildren<Collider>();shelf.transform.position+=Vector3.right*(sign*.98f-body.bounds.center.x);FreightPlaceOnFloor2717(shelf);shelf.transform.SetParent(OfficeCorridorFurniture2737.transform,true);
  }
  var art=new WorkshopWastelandArt(this);foreach(float x in new[]{-1.4f,-.4f,.8f,1.5f}){art.Cube("FallenArchivePapers2737","B9AC8C",new Vector3(x,.04f,-11.45f),new Vector3(.25f,.025f,.16f),false);art.Cube("FallenBinder2737",x<0?"567C78":"896E4E",new Vector3(x+.12f,.07f,-10.6f),new Vector3(.24f,.08f,.16f),false);}art.Bake(OfficeCorridorFurniture2737,OfficeCorridorFurniture2737.AddComponent<WorkshopArtResources>());Physics.SyncTransforms();
 }
 void OfficeBrokenWall2737(OfficeOpening2726 opening){
  var along=opening.Orientation=="v"?Vector3.forward:Vector3.right;var normal=opening.Orientation=="v"?Vector3.right:Vector3.forward;
  float half=opening.Width/2,step=opening.Width/7,mid=opening.Orientation=="v"?opening.Z:opening.X,fix=opening.Orientation=="v"?opening.X:opening.Z;
  // Retain the upper wall, with an irregular broken underside instead of a straight doorway lintel.
  float[] underside={2.44f,2.30f,2.48f,2.55f,2.36f,2.46f,2.29f};
  for(int i=0;i<7;i++)OfficeWallPiece2726(opening.Orientation,fix,mid-half+i*step,mid-half+(i+1)*step,underside[i],3.2f,"OfficeBrokenLintel2737");
  var g=new GameObject("OfficeBrokenWallFragments2737");g.transform.SetParent(Root,false);var art=new WorkshopWastelandArt(this);
  foreach(float sign in new[]{-1f,1f})for(int i=0;i<7;i++){
   float width=i%3==0?.15f:.11f;var p=opening.Position+along*sign*(half+.02f)+Vector3.up*(.19f+i*.31f);var size=opening.Orientation=="v"?new Vector3(.23f,.23f,width):new Vector3(width,.23f,.23f);
   var bit=Box("OfficeBrokenMasonry2737",p,size,i%3==0?"9B7559":"9A9D87",true);bit.transform.SetParent(g.transform,true);
   foreach(float side in new[]{-1f,1f}){
    var scar=p+normal*side*.13f;art.Panel("ExposedBrokenBrick2737",i%3==0?"92704E":"ACA98D",scar,along,Vector3.up,width*.9f,.18f);
    if(i%2==0)art.Stroke("WallImpactCracks2737","656F60",scar+along*sign*.08f,scar+along*sign*.37f+Vector3.up*.13f,.016f,normal,true);
   }
  }
  // Small loose fragments remain cosmetic and outside the clear centre of the passage.
  foreach(float sign in new[]{-1f,1f})foreach(float side in new[]{-1f,1f})for(int i=0;i<3;i++){
   var p=opening.Position+along*sign*(half+.22f+i*.12f)+normal*side*(.2f+i*.15f);art.Cube("WallRubble2737",i%2==0?"A6A18A":"86694F",p+Vector3.up*.04f,new Vector3(.15f,.08f,.11f),false);
  }
  art.Bake(g,g.AddComponent<WorkshopArtResources>());OfficeBrokenWallModels2737.Add(g);
 }
}}
