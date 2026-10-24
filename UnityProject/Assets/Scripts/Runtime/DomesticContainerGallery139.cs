using UnityEngine;
using System.Collections;
namespace GreyHarbor {
 public sealed partial class RaidVerification {
  IEnumerator DomesticContainerGallery139(){
   foreach(var look in new[]{ContainerLook.JewelryBox,ContainerLook.PrecisionCase,ContainerLook.ArchiveCase}){
    var cat=HighValueContainers139.Category(look);var rng=new System.Random(139+(int)look);bool correct=true,noRed=true;bool[] grades=new bool[4];
    for(int n=0;n<2000;n++){var k=LootBalance.Pick(SupplyType.Misc,rng,n>=1000,ContainerTier.Common,cat);correct&=RaidInventory.Group(k)==ItemGroup.Collection&&RaidInventory.Category(k)==cat.Value;noRed&=RaidInventory.Grade(k)<4;if(RaidInventory.Grade(k)<4)grades[RaidInventory.Grade(k)]=true;}
    Check(correct&&noRed,"139 High-value category and red exclusion "+look);
    Check(System.Array.TrueForAll(grades,b=>b),"139 High-value profiles retain all four eligible qualities "+look);
   }
   var map=new RaidMap{Root=new GameObject("ContainerGallery139").transform};Vector3 origin=new Vector3(110,0,110);
   map.Box("GalleryFloor",origin+Vector3.down*.12f,new Vector3(15,.2f,11),"53665F");var art=new ContainerArt(map);
   int i=0;foreach(var look in new[]{ContainerLook.BedsideDrawer,ContainerLook.Wardrobe,ContainerLook.KitchenCupboard,ContainerLook.StorageBin})art.Build(look,origin+new Vector3(-4.5f+3*i++,0,2.5f),0,false);
   i=0;foreach(var look in new[]{ContainerLook.JewelryBox,ContainerLook.PrecisionCase,ContainerLook.ArchiveCase}){
    var p=origin+new Vector3(-3+3*i++,0,-2.1f);map.Box("DisplayTable",p+Vector3.up*.38f,new Vector3(1.75f,.76f,1.1f),"7D8068");art.Build(look,p+Vector3.up*.8f,0,false);
   }
   var target=G.Rig.Target;var focus=new GameObject("GalleryFocus139");focus.transform.SetParent(map.Root);focus.transform.position=origin;
   float yaw=G.Rig.Yaw,zoom=G.View.orthographicSize;G.Rig.Target=focus.transform;G.Rig.ResetView(15);G.Rig.Tick(0);G.View.orthographicSize=6.3f;Shader.SetGlobalFloat("_HarborFogEnabled",0);
   yield return Capture("139-container-library-closed.png");
   foreach(var p in map.Root.GetComponentsInChildren<ContainerProp>()){var a=p.Opening.localPosition;p.SetSearched();Check(p.Opening.localRotation!=Quaternion.identity||p.Opening.localPosition!=a,"139 New container opening animation "+p.Look);var b=p.Opening.localPosition;p.SetSearched();Check(p.Opening.localPosition==b,"139 Container open state idempotent "+p.Look);}
   yield return Capture("139-container-library-open.png");
   focus.transform.position=origin+new Vector3(0,.6f,-2.1f);G.Rig.Tick(0);G.View.orthographicSize=2.65f;yield return Capture("139-high-value-library.png");
   G.Rig.Target=target;G.Rig.ResetView(yaw);G.Rig.Tick(0);G.View.orthographicSize=zoom;G.Vision.Bind();Destroy(map.Root.gameObject);
  }
 }
}
