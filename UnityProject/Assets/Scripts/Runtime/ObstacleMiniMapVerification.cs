using UnityEngine;
using System.Collections;
namespace GreyHarbor {
 public sealed partial class RaidVerification {
  IEnumerator VerifyObstacleMiniMap191(){
   FreshCamp();G.CloseInventory();G.NextMap=RaidDestination.Training;G.StartRaid();G.Paused=true;yield return null;
   foreach(var p in new[]{new Vector3(8,0,7.6f),new Vector3(4.6f,0,-4.4f),new Vector3(7.7f,0,3.5f)}){
    G.Player.transform.position=p-Vector3.forward*2;Physics.SyncTransforms();G.Player.Move(Vector3.forward*4);
    Check(G.Player.transform.position.z<p.z-.7f,"Pallet stack blocks real player sweep: "+p);
    Check(!G.Navigation.ClearSegment(p-Vector3.forward*2,p+Vector3.forward*2)&&G.Navigation.FindPath(p-Vector3.forward*2,p+Vector3.forward*2).Count>0,"Navigation routes around pallet: "+p);
    G.Vision.Refresh(p-Vector3.forward*2,Vector3.forward);Check(G.Vision.CanSee(p+Vector3.forward*2)&&G.ClearSight(p-Vector3.forward*2,p+Vector3.forward*2),"Pallet does not block player or enemy sight: "+p);
   }
   // Real projectiles must pass a pallet and hit the target behind it.
   G.Player.transform.position=new Vector3(4.6f,0,-7.4f);G.Player.Aim=Vector3.forward;G.Rig.ResetView(45);G.Rig.Tick(0);G.Player.Draw(G.View,0);
   var targetGO=new GameObject("QA projectile target");targetGO.transform.SetParent(G.Map.Root);var target=targetGO.AddComponent<RaidActor>();target.Initialize(true);
   Vector3 muzzle=G.Player.MuzzleOnPlane(G.View);target.transform.position=new Vector3(muzzle.x,0,-1.9f);Physics.SyncTransforms();G.SpawnShot(G.Player,Vector3.forward);G.TickProjectiles(.2f);Check(target.Health==72-ArmorBalance.AKDamage,"Actual AK projectile passes LOW pallet stack and hits target");
   G.Player.transform.position=new Vector3(0,0,4.5f);G.Rig.Tick(0);G.Player.Draw(G.View,0);muzzle=G.Player.MuzzleOnPlane(G.View);target.transform.position=new Vector3(muzzle.x,0,11);target.Health=72;Physics.SyncTransforms();G.SpawnShot(G.Player,Vector3.forward);G.TickProjectiles(.25f);Check(target.Health==72,"Actual AK projectile cannot hit target behind warehouse");targetGO.SetActive(false);Destroy(targetGO);
   var decoration=G.Map.Box("QA Decoration",new Vector3(3,.6f,-11),new Vector3(1,1.2f,1),"617451",true);SceneObstacle.Apply(decoration,ObstacleKind.Decoration);Physics.SyncTransforms();
   G.Player.transform.position=new Vector3(3,0,-13);G.Player.Move(Vector3.forward*4);G.Vision.Refresh(new Vector3(3,0,-13),Vector3.forward);
   Check(G.Player.transform.position.z>-9.1f&&G.Vision.CanSee(new Vector3(3,0,-9))&&!Physics.Linecast(new Vector3(3,.95f,-13),new Vector3(3,.95f,-9),SceneObstacle.ShotMask&~(1<<9)),"Decoration permits movement, vision and projectiles");decoration.SetActive(false);Destroy(decoration);
   bool lamps=true;int count=0;foreach(Transform t in G.Map.Root)if(t.name=="PortKit_YardLamp"){count++;Vector3 inward=t.position.x<0?Vector3.right:Vector3.left;Vector3 arm=t.Find("Glass").position-t.position;arm.y=0;lamps&=Vector3.Dot(arm.normalized,inward)>.99f;}Check(lamps&&count==5,"All five lamp heads point toward entrance or inner roads");
   G.Player.transform.position=G.Map.Spawn;G.Player.Aim=Vector3.forward;Physics.SyncTransforms();G.Vision.Refresh(G.Player.transform.position,G.Player.Aim);
   var rect=new Rect(1352,72,210,168);bool aligned=true,contained=true;
   for(int i=0;i<16;i++){
    G.Rig.ResetView(45+i*22.5f);G.Rig.Tick(0);var map=new MiniMapProjection(G.Map.Bounds,rect,G.View.transform.rotation);
    foreach(var d in new[]{Vector3.forward,Vector3.right,Vector3.forward+Vector3.right}){
     Vector2 mini=map.Point(G.Map.Spawn+d)-map.Point(G.Map.Spawn);Vector3 delta=G.View.WorldToScreenPoint(G.Map.Spawn+d)-G.View.WorldToScreenPoint(G.Map.Spawn);Vector2 main=new Vector2(delta.x,-delta.y);aligned&=Vector2.Dot(mini.normalized,main.normalized)>.9999f;
    }
    var b=G.Map.Bounds;foreach(float x in new[]{b.xMin,b.xMax})foreach(float z in new[]{b.yMin,b.yMax})contained&=rect.Contains(map.Point(new Vector3(x,0,z)));
   }
   Check(aligned,"Minimap matches actual camera projection at four views and twelve intermediate angles");Check(contained,"All map corners stay within minimap while turning");
   for(int n=0;n<4;n++){G.Rig.ResetView(45+n*90);G.Rig.Tick(0);yield return Capture("50-minimap-synced-"+n+".png");}G.Rig.ResetView(45);G.Finish(false);
  }
 }
}
