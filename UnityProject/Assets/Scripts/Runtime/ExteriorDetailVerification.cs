using UnityEngine;
using System.Collections;
namespace GreyHarbor {
 public sealed partial class RaidVerification {
  IEnumerator VerifyExteriorDetails192(){
   FreshCamp();G.CloseInventory();G.NextMap=RaidDestination.Training;G.StartRaid();G.Paused=true;yield return null;
   foreach(var p in new[]{new Vector3(-3.4f,0,-5),new Vector3(-4,0,3.1f)}){
    G.Player.transform.position=p-Vector3.forward*2.5f;Physics.SyncTransforms();G.Player.Move(Vector3.forward*5);
    Check(G.Player.transform.position.z<p.z-.9f,"High timber blocks walking: "+p);
    G.Vision.Refresh(p-Vector3.forward*2.5f,Vector3.forward);Check(!G.Vision.CanSee(p+Vector3.forward*2.5f)&&!G.ClearSight(p-Vector3.forward*2.5f,p+Vector3.forward*2.5f),"High timber blocks player and enemy visibility: "+p);
    Check(!G.Navigation.ClearSegment(p-Vector3.forward*2.5f,p+Vector3.forward*2.5f)&&G.Navigation.FindPath(p-Vector3.forward*2.5f,p+Vector3.forward*2.5f).Count>0,"High timber supports a navigable detour: "+p);
   }
   G.Player.transform.position=new Vector3(-3.4f,0,-8);G.Player.Aim=Vector3.forward;G.Rig.ResetView(45);G.Rig.Tick(0);G.Player.Draw(G.View,0);
   var dummyGO=new GameObject("QA high timber target");dummyGO.transform.SetParent(G.Map.Root);var dummy=dummyGO.AddComponent<RaidActor>();dummy.Initialize(true);Vector3 muzzle=G.Player.MuzzleOnPlane(G.View);dummy.transform.position=new Vector3(muzzle.x,0,-2.5f);Physics.SyncTransforms();G.SpawnShot(G.Player,Vector3.forward);G.TickProjectiles(.2f);Check(dummy.Health==72,"Actual AK projectile is stopped by the high timber bundle");dummyGO.SetActive(false);Destroy(dummyGO);
   int high=0,low=0;bool sizes=true;foreach(Transform t in G.Map.Root){if(t.name=="PortKit_HighTimberStack"){high++;sizes&=t.Find("DenseTimberCore").GetComponent<Collider>().bounds.size.y>1.6f;}if(t.name=="PortKit_LowPallets"){low++;sizes&=t.Find("PalletCollider").GetComponent<Collider>().bounds.size.y<.6f;}}
   Check(high==2&&low==3&&sizes,"Two tall dense stacks and three distinctly low pallet stacks are present");
   var details=G.Map.Root.Find("YardSurfaceDetails_192");Check(details&&details.GetComponentsInChildren<Collider>().Length==0&&details.childCount<48,"Surface detailing is batched and adds no invisible collision");
   G.Player.transform.position=new Vector3(-7,0,-9);Physics.SyncTransforms();G.Player.Move(Vector3.forward*2.5f);Check(G.Player.transform.position.z>-6.6f,"Shallow road pothole remains walkable");
   G.Player.transform.position=new Vector3(-12.5f,0,-14.5f);Physics.SyncTransforms();G.Player.Move(Vector3.right);Check(G.Player.transform.position.x>-11.6f,"Wall-side grass remains walkable");
   VerifyYardFractures115();VerifyGroundPlacement117(G.Map);
   Check(G.Map.Loot.Count==6&&G.Enemies.Count==4&&G.Map.ClosedEntrances.Count==1,"Training keeps four-monster scope and only the locked secondary warehouse entrance");
   G.Player.transform.position=new Vector3(.5f,0,-6.4f);G.Player.Aim=Vector3.forward;G.Vision.Refresh(G.Player.transform.position,G.Player.Aim);
   for(int i=0;i<4;i++){G.Rig.ResetView(45+i*90);G.Rig.Tick(0);yield return Capture("70-detailed-yard-"+i+".png");}G.Finish(false);
  }
 }
}
