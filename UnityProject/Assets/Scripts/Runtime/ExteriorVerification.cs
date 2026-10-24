using System.Collections;
using System.IO;
using UnityEngine;
namespace GreyHarbor {
 public sealed partial class RaidVerification {
  IEnumerator VerifyExterior19(){
   FreshCamp();G.CloseInventory();G.NextMap=RaidDestination.Training;string stock=JsonUtility.ToJson(G.Save);Check(G.StartRaid()&&G.Map.ExteriorPreview&&!G.Map.IsHarbor,"Production training destination loads exterior yard");G.Paused=true;yield return null;
   Check(G.Enemies.Count==4&&G.Map.MonsterSites.Count==0&&G.Map.Loot.Count==6&&G.Map.SupplySites.Count==0,"Exterior has six placed search containers and four training monsters");
   Check(G.Map.Bounds.width==38&&G.Map.Bounds.height==34&&G.Map.ExteriorBuildings.Count==3&&G.Map.ClosedEntrances.Count==1,"Exterior keeps three collision shells with only the secondary warehouse entrance locked");
   bool routes=true;foreach(var p in G.Map.RegionAnchors)routes&=G.Navigation.FindPath(G.Map.Spawn,p).Count>0;Check(routes,"All six outdoor areas and extraction connect to spawn");
   foreach(var p in new[]{new Vector3(-7,0,-10),new Vector3(-7,0,4),new Vector3(7,0,11),new Vector3(15,0,9),new Vector3(15,0,-8)})Check(G.Navigation.FindPath(G.Map.Spawn,p).Count>0,"Loop route waypoint reachable: "+p);
   bool opaque=true;foreach(var b in G.Map.ExteriorBuildings)opaque&=Physics.CheckCapsule(new Vector3(b.center.x,.35f,b.center.y),new Vector3(b.center.x,1,b.center.y),.3f,1<<8);Check(opaque,"Buildings are completely sealed against entry");
   G.Player.transform.position=new Vector3(0,0,6);Physics.SyncTransforms();G.Player.Move(Vector3.forward*3);Check(G.Player.transform.position.z<7.1f,"Player cannot walk through A1 closed shutter");
   G.Player.transform.position=new Vector3(7,0,11);Physics.SyncTransforms();G.Player.Move(Vector3.left*3);Check(G.Player.transform.position.x>5.7f,"Player cannot enter A2 from the east");
   G.Player.transform.position=new Vector3(-8,0,1);Physics.SyncTransforms();G.Player.Move(Vector3.left*3);Check(G.Player.transform.position.x>-9.7f,"Player cannot enter repair building B");
   G.Vision.Refresh(new Vector3(0,0,4),Vector3.forward);Check(!G.Vision.CanSee(new Vector3(0,0,12)),"Flashlight is blocked by warehouse facade");
   G.Vision.Refresh(new Vector3(11,0,-.4f),Vector3.forward);Check(!G.Vision.CanSee(new Vector3(11,0,7.5f)),"Flashlight is blocked by sealed container");
   G.Vision.Refresh(G.Map.Spawn,Vector3.forward);bool explored=true;for(int i=0;i<G.Vision.Seen.Length;i++)explored&=G.Vision.Seen[i]&&G.Vision.SurfaceSeen[i];Check(explored&&!G.Vision.CanSee(new Vector3(-16,0,14)),"Whole exterior is grey-known without granting live omniscience");
   G.Player.transform.position=G.Map.Spawn;G.Controls=new TouchInput{Searching=true};G.InventoryOpen=false;G.TickInteraction(.2f);Check(G.Nearby==null&&!G.InventoryOpen,"F does not open absent containers or sealed buildings");G.Controls=new DesktopInput();G.Player.Aim=Vector3.forward;
   for(int n=0;n<4;n++){G.Rig.ResetView(45+n*90);G.Rig.Tick(0);yield return Capture("40-exterior-view-"+n+".png");}G.Rig.ResetView(45);
   yield return new WaitForEndOfFrame();var go=new GameObject("ExteriorOverviewCamera");var cam=go.AddComponent<Camera>();cam.CopyFrom(G.View);cam.transform.position=new Vector3(0,70,0);cam.transform.rotation=Quaternion.Euler(90,0,0);cam.orthographicSize=19;var rt=new RenderTexture(1440,1280,24);cam.targetTexture=rt;cam.Render();var prior=RenderTexture.active;RenderTexture.active=rt;var tex=new Texture2D(1440,1280,TextureFormat.RGB24,false);tex.ReadPixels(new Rect(0,0,1440,1280),0,0);tex.Apply();File.WriteAllBytes(Path.Combine(folder,"44-exterior-overview.png"),tex.EncodeToPNG());RenderTexture.active=prior;cam.targetTexture=null;rt.Release();Destroy(rt);Destroy(tex);Destroy(go);
   G.Player.transform.position=G.Map.Extraction;G.TickExtraction(5.1f);Check(G.Phase==GamePhase.Base&&!G.TrainingActive&&JsonUtility.ToJson(G.Save)==stock,"Exterior extraction restores exact original inventory and camp state");
   G.NextMap=RaidDestination.Training;G.StartRaid();G.Paused=true;G.Magazine=2;G.Finish(false);Check(G.Phase==GamePhase.Base&&JsonUtility.ToJson(G.Save)==stock,"Ending exterior inspection restores training equipment isolation");
  }
 }
}
