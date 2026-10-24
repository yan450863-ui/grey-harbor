using UnityEngine;
using System.Collections;
using System.IO;
namespace GreyHarbor {
 public sealed partial class RaidVerification {
  IEnumerator VerifySouthDock142(){
   Check(G.CheckSpeed142==1&&Time.timeScale==1,"142 Fresh launch defaults to normal speed");FreshCamp();string saved=JsonUtility.ToJson(G.Save);G.HarborWalkthrough=true;G.HardMode=false;G.BeginTraining();G.Paused=true;yield return null;var map=G.Map;
   Check(G.SavePath.Contains("1.42")&&map.Bounds.width==190&&map.Bounds.height==180,"142 Isolated check and unchanged map dimensions");
   Check(map.SouthDockLoot142.Count==12&&map.DockLoot141.Count==16&&map.Loot.Count==22,"142 South sites triple from four to twelve, four other pier and six prior sites retained");
   Check(map.SouthDockLoot142.FindAll(l=>l.Tier==ContainerTier.Common).Count==11&&map.SouthDockLoot142.FindAll(l=>l.Tier==ContainerTier.Advanced).Count==1,"142 Eight added sites common, existing advanced document tube retained");
   Check(map.DockBoats141.Count==2&&map.Doors.Count==4&&map.HarborSurfaces.Count==10,"142 No passenger vessel or new interior added");
   Check(map.DockLoot141[0].Prop.Look==ContainerLook.RescueCabinet&&Vector3.Distance(map.DockLoot141[0].Position,RaidMap.PortPoint(151,131.5f))<.01f&&map.DockLoot141[1].Prop.Look==ContainerLook.MarineCooler&&map.DockLoot141[2].Prop.Look==ContainerLook.WeldingToolbox&&Vector3.Distance(map.DockLoot141[2].Position,RaidMap.PortPoint(154,89.7f))<.01f&&map.DockLoot141[3].Prop.Look==ContainerLook.FishingChest,"142 North and middle search configuration retained");
   Check(map.LoadingProps142.Count==15&&map.LoadingProps142.TrueForAll(p=>p&&p.GetComponent<DockLoadingAsset142>()&&p.GetComponentsInChildren<MeshFilter>().Length>0),"142 New loading props have reusable identity and actual meshes");
   Check(Clear140(RaidMap.DockPreviewSpawn141),"142 Dock preview starts in clear space");
   foreach(var p in new[]{RaidMap.PortPoint(159,46),RaidMap.PortPoint(160,29),RaidMap.PortPoint(170,34),RaidMap.PortPoint(181,25),map.Exits[1]}){Check(Clear140(p)&&Walk140(p),"142 Actual player follows dock circulation "+p);}
   foreach(var loot in map.SouthDockLoot142){G.Bag.Slots.Clear();G.Chest.Slots.Clear();Check(Clear140(loot.Position),"142 Search standing space clear "+loot.Prop.Look+" "+loot.Position);yield return Search140(loot);for(int q=0;q<4;q++){G.Rig.ResetView(q*90+45);G.Rig.Tick(0);Check(G.CanInteractLoot(loot),"142 Search remains usable after view turn "+loot.Prop.Look+" "+q);}G.Rig.ResetView(225);G.Rig.Tick(0);yield return Capture("142-search-"+map.SouthDockLoot142.IndexOf(loot)+".png");}
   var boat=map.DockBoats141[1];G.Player.transform.position=boat.Shore;Physics.SyncTransforms();Check(Walk140(boat.Board)&&Walk140(boat.Deck)&&Walk140(boat.Shore),"142 Existing barge remains boardable and returnable after filling shore");
   var enemies=G.Enemies.FindAll(e=>e.name.StartsWith("码头 / 南部"));Check(enemies.Count==2&&G.Enemies.Count==12,"142 Existing monster population retained");foreach(var e in enemies)Patrol140(e);
   var forklift=map.LoadingProps142.Find(p=>p.GetComponent<DockLoadingAsset142>().Asset=="Forklift");Check(forklift.GetComponentInChildren<Collider>().gameObject.layer==SceneObstacle.HighLayer,"142 Forklift blocks walking sight and shots");var jack=map.LoadingProps142.Find(p=>p.GetComponent<DockLoadingAsset142>().Asset=="PalletJack");Check(jack.GetComponentInChildren<Collider>().gameObject.layer==SceneObstacle.LowLayer,"142 Pallet jack blocks only walking");
   yield return VerifyInspectionSpeed142();yield return SouthDockPictures142();
   G.Player.transform.position=map.Exits[1];G.Controls=new TouchInput();G.Paused=false;G.TickExtraction(5.1f);yield return null;Check(G.Phase==GamePhase.Base&&JsonUtility.ToJson(G.Save)==saved,"142 Southeast extraction still restores preview inventory");Check(G.CheckSpeed142==1&&Time.timeScale==1,"142 Checks restore normal speed");
  }
  IEnumerator VerifyInspectionSpeed142(){
   var actors=G.Enemies.ToArray();G.Enemies.Clear();foreach(var e in actors)e.gameObject.SetActive(false);var originalInput=G.Controls;var input=new TouchInput();G.Controls=input;G.Rig.ResetView(0);
   foreach(int speed in new[]{1,2,5}){
    G.Paused=true;Check(G.SetCheckSpeed142(speed)&&G.CheckSpeed142==speed,"142 Speed option applies "+speed);yield return null;
    G.Player.transform.position=RaidMap.PortPoint(160,29);Physics.SyncTransforms();G.Rig.Tick(0);input.AimPosition=G.View.WorldToScreenPoint(G.Player.transform.position+G.Rig.Right*8+Vector3.up);input.Movement=Vector2.up;G.Paused=false;
    float before=G.RaidTime,start=Time.realtimeSinceStartup;Vector3 p=G.Player.transform.position;while(Time.realtimeSinceStartup-start<.22f)yield return null;float elapsed=Time.realtimeSinceStartup-start,simulated=G.RaidTime-before;G.Paused=true;input.Movement=Vector2.zero;
    Check(simulated/elapsed>speed*.70f&&simulated/elapsed<speed*1.35f,"142 Live clock advances at selected speed "+speed);
    Check(Mathf.Abs(Vector3.Distance(p,G.Player.transform.position)-simulated*3.4f)<.16f&&Clear140(G.Player.transform.position),"142 Live movement follows accelerated clock "+speed);
    float time=G.RaidTime;Vector3 paused=G.Player.transform.position;input.Movement=Vector2.one;float wait=Time.realtimeSinceStartup;while(Time.realtimeSinceStartup-wait<.10f)yield return null;Check(G.Player.transform.position==paused&&G.RaidTime==time,"142 Pause freezes play at speed "+speed);input.Movement=Vector2.zero;
   }
   Check(!G.SetCheckSpeed142(3)&&G.CheckSpeed142==5,"142 Unsupported speed rejected without altering current value");
   G.Player.transform.position=RaidMap.PortPoint(150,26);G.Player.Facing=1;G.Rig.Tick(0);input.AimPosition=G.View.WorldToScreenPoint(G.Player.transform.position+G.Rig.Right*10+Vector3.up);input.Movement=Vector2.right;Physics.SyncTransforms();G.Paused=false;float started=Time.realtimeSinceStartup;while(Time.realtimeSinceStartup-started<.7f)yield return null;G.Paused=true;input.Movement=Vector2.zero;Debug.Log("142 Forklift contact "+G.Player.transform.position);Check(!Physics.CheckCapsule(G.Player.transform.position+Vector3.up*.35f,G.Player.transform.position+Vector3.up,.29f,SceneObstacle.MovementMask)&&G.Player.transform.position.x<RaidMap.PortPoint(153,26).x&&G.Player.transform.position.x>RaidMap.PortPoint(151,26).x,"142 5x live movement stops against new forklift");
   G.GetComponent<RaidUI>().OpenInventorySettings();yield return new WaitForEndOfFrame();File.WriteAllBytes(Path.Combine(folder,"142-speed-settings.png"),ScreenCapture.CaptureScreenshotAsTexture().EncodeToPNG());
   Check(G.SetCheckSpeed142(1),"142 Return to normal speed");G.Controls=originalInput;foreach(var e in actors){e.gameObject.SetActive(true);G.Enemies.Add(e);}G.Rig.ResetView(225);
  }
  IEnumerator SouthDockPictures142(){
   G.Player.transform.position=RaidMap.PortPoint(163,30);G.Player.Aim=Vector3.forward;G.Rig.ResetView(225);G.Rig.Tick(0);G.Vision.Refresh(G.Player.transform.position,G.Player.Aim);yield return Capture("142-south-dock-play.png");
   G.Player.transform.position=RaidMap.PortPoint(155,24);G.Rig.ResetView(45);G.Rig.Tick(0);G.Vision.Refresh(G.Player.transform.position,G.Player.Aim);yield return Capture("142-loading-facilities.png");
   yield return new WaitForEndOfFrame();var ambient=RenderSettings.ambientLight;var sun=GameObject.Find("LateAfternoon").GetComponent<Light>();float power=sun.intensity;RenderSettings.ambientLight=new Color(.28f,.32f,.29f);sun.intensity=.57f;Shader.SetGlobalFloat("_HarborFogEnabled",0);var go=new GameObject("SouthOverview142");var cam=go.AddComponent<Camera>();cam.CopyFrom(G.View);cam.enabled=false;cam.transform.rotation=Quaternion.Euler(55,45,0);cam.transform.position=RaidMap.PortPoint(168,32)-cam.transform.forward*100;cam.orthographicSize=24;SaveYardCamera136(cam,"142-south-dock-overview.png",1800,1300);Destroy(go);RenderSettings.ambientLight=ambient;sun.intensity=power;G.Vision.Bind();yield return null;
   var gallery=new RaidMap{Root=new GameObject("LoadingGallery142").transform};var k=new DockLoadingKit142(gallery);var o=new Vector3(240,0,180);gallery.Box("LibraryFloor",o-Vector3.up*.12f,new Vector3(29,.20f,18),"586F62");k.Forklift(o+new Vector3(-9,0,4));k.PalletJack(o+new Vector3(-4.5f,0,4));k.WrappedLoad(o+new Vector3(0,0,4));k.BarrelRack(o+new Vector3(4.5f,0,4));k.ShoreUtility(o+new Vector3(9,0,4));k.NetRack(o+new Vector3(-9,0,-2));k.WasteStation(o+new Vector3(-4.5f,0,-2));k.DockLamp(o+new Vector3(0,0,-2));k.SpoolStack(o+new Vector3(4.5f,0,-2));k.StrapBasket(o+new Vector3(9,0,-2));k.NoticeBoard(o+new Vector3(-4.5f,0,-6));k.PipeBundle(o+new Vector3(4.5f,0,-6));
   var target=G.Rig.Target;var focus=new GameObject("GalleryFocus142");focus.transform.SetParent(gallery.Root);focus.transform.position=o;G.Rig.Target=focus.transform;float zoom=G.View.orthographicSize;G.Rig.ResetView(15);G.Rig.Tick(0);G.View.orthographicSize=11;Shader.SetGlobalFloat("_HarborFogEnabled",0);yield return Capture("142-loading-assets.png");G.Rig.Target=target;G.Rig.ResetView(225);G.Rig.Tick(0);G.View.orthographicSize=zoom;G.Vision.Bind();Destroy(gallery.Root.gameObject);
  }
 }
}
