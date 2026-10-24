using UnityEngine;
using System.Collections;
namespace GreyHarbor {
 public sealed partial class RaidVerification {
  IEnumerator ContainerGallery(){
   G.Paused=true;var map=new RaidMap();map.Root=new GameObject("ContainerGalleryQA").transform;var art=new ContainerArt(map);Vector3 origin=new Vector3(110,0,110);
   map.Box("GalleryFloor",origin+Vector3.down*.12f,new Vector3(16,.2f,15),"53665F");
   for(int i=0;i<9;i++){Vector3 p=origin+new Vector3((i%3-1)*4.4f,0,(1-i/3)*4.2f);art.Build((ContainerLook)i,p);var title=new GameObject("Label").AddComponent<TextMesh>();title.transform.SetParent(map.Root);title.transform.position=p+new Vector3(0,.08f,-1.15f);title.transform.rotation=Quaternion.Euler(90,0,0);title.text=(i+1)+" "+((ContainerLook)i).ToString();title.characterSize=.075f;title.fontSize=64;title.anchor=TextAnchor.MiddleCenter;title.color=new Color(.85f,.90f,.79f);}
   var oldTarget=G.Rig.Target;float oldYaw=G.Rig.Yaw;float oldSize=G.View.orthographicSize;var focus=new GameObject("GalleryFocus");focus.transform.position=origin;focus.transform.SetParent(map.Root);G.Rig.Target=focus.transform;G.Rig.ResetView(12);G.Rig.Tick(0);G.View.orthographicSize=7;Shader.SetGlobalFloat("_HarborFogEnabled",0);yield return Capture("91-container-library.png");G.Rig.Target=oldTarget;G.Rig.ResetView(oldYaw);G.Rig.Tick(0);G.View.orthographicSize=oldSize;Destroy(map.Root.gameObject);
  }
  IEnumerator VerifyContainers110(){
   FreshCamp();G.CloseInventory();string snapshot=JsonUtility.ToJson(G.Save);G.NextMap=RaidDestination.Training;G.StartRaid();G.Paused=true;yield return null;
   Check(G.Map.Loot.TrueForAll(l=>l.View.activeInHierarchy)&&G.Map.Loot.Count==6&&G.Map.Loot.FindAll(l=>l.Type==SupplyType.Safe).Count==1,"Training has six placed containers and exactly one safe");
   foreach(var l in G.Map.Loot){
    Check(l.Prop&&l.Items.Count>0&&l.Items.TrueForAll(p=>!p.Identified)&&G.Navigation.FindPath(G.Map.Spawn,l.Position).Count>0,"Placed container reachable with unknown loot: "+l.Name);
    G.Player.transform.position=l.Position;G.Player.Aim=(l.View.transform.position-l.Position).normalized;G.Player.Aim=new Vector3(G.Player.Aim.x,0,G.Player.Aim.z).normalized;Physics.SyncTransforms();G.Vision.Refresh(G.Player.transform.position,G.Player.Aim);G.InventoryOpen=false;G.Controls=new TouchInput{Searching=false};G.TickInteraction(.01f);G.Controls=new TouchInput{Searching=true};G.TickInteraction(l.SearchDuration+.01f);
    Check(l.Opened&&G.InventoryOpen&&G.OpenLoot==l&&l.Prop.Searched,"F search opens the actual placed container and its visual: "+l.Name);
    G.Paused=false;G.TickIdentification(RaidGame.IdentificationSeconds(l.Items[0].Kind)+.01f);G.Paused=true;Check(l.Items[0].Identified,"Placed container identifies its first item: "+l.Name);G.CloseInventory();
   }
   var tool=G.Map.Loot.Find(l=>l.Type==SupplyType.Tools);G.Player.transform.position=tool.Position;G.Player.Aim=Vector3.forward;G.Vision.Refresh(G.Player.transform.position,G.Player.Aim);G.OpenLoot=tool;G.InventoryOpen=true;var first=tool.Items[0];G.Paused=false;Check(G.QuickTransfer(ItemArea.Loot,first.Cell),"Identified training container loot transfers to player storage");G.Paused=true;G.CloseInventory();G.Controls=new DesktopInput();G.Finish(true);Check(JsonUtility.ToJson(G.Save)==snapshot,"Container training loot cannot change formal inventory");
   var rng=new System.Random(110);bool categories=true;for(int t=0;t<6;t++)for(int i=0;i<400;i++)foreach(var s in HarborContent.Contents((SupplyType)t,rng))if(RaidInventory.Group(s.Kind)==ItemGroup.Collection)categories&=ContainerLootRules.Weights[t,(int)RaidInventory.Category(s.Kind)]>0;Check(categories,"Container collection loot obeys existing category profiles");
   G.VerificationSmallMap=false;G.NextMap=RaidDestination.Harbor;Check(G.StartRaid()&&G.Map.IsHarbor,"Container check enters real harbor");G.Paused=true;yield return null;Check(G.Map.HarborWhitebox&&G.Map.Loot.Count==3,"134 Harbor entrance has three searchable containers");G.Finish(false);
   G.NextMap=RaidDestination.Training;G.StartRaid();G.Paused=true;yield return null;G.Player.transform.position=new Vector3(-10.3f,0,-6.6f);G.Player.Aim=new Vector3(-.6f,0,1).normalized;G.Rig.ResetView(45);G.Rig.Tick(0);G.Vision.Refresh(G.Player.transform.position,G.Player.Aim);yield return Capture("90-training-container-station.png");
   G.Player.transform.position=new Vector3(-11.2f,0,-1.8f);G.Player.Aim=Vector3.right;Physics.SyncTransforms();G.Vision.Refresh(G.Player.transform.position,G.Player.Aim);G.Controls=new TouchInput{Searching=true};G.InventoryOpen=false;G.TickInteraction(7);Check(G.Nearby==null&&!G.InventoryOpen,"Building wall prevents searching mounted medicine cabinet from behind");G.Controls=new DesktopInput();G.Finish(false);
   yield return ContainerGallery();
  }
 }
}
