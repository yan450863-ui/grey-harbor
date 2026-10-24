using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace GreyHarbor {
 public sealed partial class RaidVerification {
  // Immutable world positions/length/yaw from 2.7.13 actual runtime CSV,
  // not recomputed from the new appearance/dressing placement implementation.
  readonly Vector4[] cargoBaseline2714={
    new Vector4(-21.90000f,-4.20000f,12.20000f,90f),
    new Vector4(-20.80000f,12.40000f,12.20000f,90f),
    new Vector4(-21.80000f,34.50000f,12.20000f,90f),
    new Vector4(-20.60000f,49.20000f,12.20000f,90f),
    new Vector4(-2.50000f,-9.80000f,12.20000f,5f),
    new Vector4(-2.20000f,34.00000f,12.20000f,348f),
    new Vector4(4.00000f,48.00000f,12.20000f,35f),
    new Vector4(-7.00000f,54.00000f,12.20000f,0f),
    new Vector4(47.80000f,-4.50000f,12.20000f,90f),
    new Vector4(47.70000f,12.00000f,12.20000f,90f),
    new Vector4(48.00000f,41.80000f,12.20000f,90f),
    new Vector4(47.90000f,52.60000f,6.20000f,90f),
    new Vector4(32.00000f,-9.80000f,12.20000f,0f),
    new Vector4(25.50000f,1.50000f,12.20000f,22f),
    new Vector4(28.00000f,11.00000f,6.20000f,350f),
    new Vector4(27.00000f,33.20000f,6.20000f,90f),
    new Vector4(27.40000f,43.90000f,12.20000f,332f),
    new Vector4(-18.90000f,-4.20000f,12.20000f,90f),
    new Vector4(-17.80000f,12.40000f,12.20000f,90f),
    new Vector4(-18.80000f,34.50000f,12.20000f,90f),
    new Vector4(-17.60000f,49.20000f,12.20000f,90f),
    new Vector4(-10.02130f,-9.14200f,6.20000f,94.99999f),
    new Vector4(6.81440f,-10.61490f,6.20000f,5f),
    new Vector4(5.72070f,50.45750f,12.20000f,35f),
    new Vector4(2.27930f,45.54250f,12.20000f,35f),
    new Vector4(10.18460f,43.66950f,6.20000f,125f),
    new Vector4(44.80000f,-4.50000f,12.20000f,90f),
    new Vector4(44.70000f,12.00000f,12.20000f,90f),
    new Vector4(47.70000f,21.35000f,6.20000f,90f),
    new Vector4(45.00000f,41.80000f,12.20000f,90f),
    new Vector4(44.90000f,52.60000f,6.20000f,90f),
    new Vector4(22.65000f,-9.80000f,6.20000f,0f),
    new Vector4(26.62380f,4.28160f,12.20000f,22f),
    new Vector4(18.49980f,4.32830f,6.20000f,112f),
    new Vector4(28.52090f,8.04560f,6.20000f,350f),
    new Vector4(21.74650f,9.89730f,6.20000f,350f),
    new Vector4(24.00000f,33.20000f,6.20000f,90f),
    new Vector4(27.00000f,37.75000f,6.20000f,180f),
    new Vector4(-2.60000f,7.40000f,6.20000f,0f),
    new Vector4(-4.00000f,11.90000f,6.20000f,0f),
    new Vector4(-5.50000f,24.50000f,6.20000f,0f),
    new Vector4(-14.70000f,36.90000f,6.20000f,90f),
    new Vector4(10.60000f,33.00000f,6.20000f,90f),
    new Vector4(-2.60000f,4.40000f,6.20000f,0f),
    new Vector4(1.95000f,7.40000f,6.20000f,90f),
    new Vector4(-11.85000f,24.50000f,6.20000f,0f),
    new Vector4(-14.70000f,30.55000f,6.20000f,90f),
    new Vector4(-14.70000f,43.25000f,6.20000f,90f),
    new Vector4(38.80000f,12.30000f,6.20000f,90f),
    new Vector4(35.80000f,18.65000f,6.20000f,90f),
    new Vector4(14.70000f,0.00000f,6.20000f,90f),
    new Vector4(21.00000f,36.80000f,6.20000f,90f),
    new Vector4(7.60000f,33.00000f,6.20000f,90f),
    new Vector4(10.60000f,28.45000f,6.20000f,180f),
    new Vector4(-15.10000f,3.80000f,6.20000f,0f),
    new Vector4(-10.80000f,46.50000f,6.20000f,90f),
    new Vector4(7.00000f,18.50000f,6.20000f,16f),
    new Vector4(10.00000f,54.00000f,6.20000f,0f),
    new Vector4(40.80000f,31.50000f,6.20000f,0f),
    new Vector4(32.50000f,55.00000f,12.20000f,0f),
    new Vector4(42.00000f,52.80000f,6.20000f,90f),
    new Vector4(41.00000f,-8.30000f,6.20000f,90f),
    new Vector4(12.00000f,19.00000f,6.20000f,90f),
    new Vector4(32.00000f,39.70000f,6.20000f,90f),
  };
  readonly List<string> cargoContentRows2714=new List<string>{"type,name,x,y,z,detail"};
  void CargoContentRow2714(string type,string name,Vector3 position,string detail){cargoContentRows2714.Add(type+","+Csv230(name)+","+WN279(position.x)+","+WN279(position.y)+","+WN279(position.z)+","+Csv230(detail));File.WriteAllLines(Path.Combine(folder,Array.IndexOf(Environment.GetCommandLineArgs(),"--capture-cargo-content-2714")>=0?"2714-cargo-capture-art.csv":"2714-cargo-actual-content.csv"),cargoContentRows2714);}
  void CargoOriginalAndArt2714(){
   var map=G.Map;Check(map.CargoBoxes2712.Count==cargoBaseline2714.Length,"2714 All64 original maze collision boxes retained");var variants=new HashSet<CargoVariant2714>();var colors=new HashSet<Color32>();var meshShapes=new HashSet<int>();int decorated=0;
   for(int i=0;i<cargoBaseline2714.Length;i++){
    var box=map.CargoBoxes2712[i];var expected=cargoBaseline2714[i];var position=box.View.transform.position;var collider=box.View.GetComponent<BoxCollider>();Check(collider!=null&&collider.enabled&&!collider.isTrigger,"2714 Original enabled cargo BoxCollider remains "+i);var size=Vector3.Scale(collider.size,box.View.transform.lossyScale);
    Check(box.View.name=="CargoMaze_"+i&&Vector3.Distance(position,new Vector3(expected.x,1.3f,expected.y))<.002f&&Mathf.Abs(Mathf.DeltaAngle(box.View.transform.eulerAngles.y,expected.w))<.002f&&Vector3.Distance(size,new Vector3(expected.z,2.6f,2.6f))<.002f,"2714 Original actual world position/yaw/physical size unchanged "+i+" position="+position);
    var appearance=box.View.GetComponent<CargoAppearance2714>();Check(appearance!=null&&appearance.ArtRoot!=null&&appearance.ArtRoot.transform.IsChildOf(box.View.transform),"2714 Every original container has attached real new surface art "+i);variants.Add(appearance.Variant);var shell=box.View.GetComponent<Renderer>();colors.Add((Color32)shell.sharedMaterial.color);
    Check(appearance.ArtRoot.GetComponentsInChildren<Collider>(true).Length==0&&box.View.GetComponentsInChildren<Collider>().Length==1,"2714 New cargo appearance adds no physical colliders "+i);
    bool validMesh=false,material=true,envelope=true;int shapeHash=17;foreach(var renderer in appearance.ArtRoot.GetComponentsInChildren<Renderer>())foreach(var m in renderer.sharedMaterials)material&=m!=null&&m.shader.name=="GreyHarbor/World"&&m.GetFloat("_SolidFog")>.5f;
    foreach(var filter in appearance.ArtRoot.GetComponentsInChildren<MeshFilter>()){var mesh=filter.sharedMesh;if(mesh==null||mesh.vertexCount==0)continue;validMesh=true;unchecked{shapeHash=shapeHash*31+mesh.vertexCount;shapeHash=shapeHash*31+mesh.triangles.Length;foreach(var v in mesh.vertices){var local=box.View.transform.InverseTransformPoint(filter.transform.TransformPoint(v));var physical=Vector3.Scale(local,size);envelope&=Mathf.Abs(physical.x)<=size.x*.5f+.035f&&Mathf.Abs(physical.y)<=size.y*.5f+.035f&&Mathf.Abs(physical.z)<=size.z*.5f+.035f;shapeHash=shapeHash*31+Mathf.RoundToInt(v.x*1000);shapeHash=shapeHash*31+Mathf.RoundToInt(v.y*1000);shapeHash=shapeHash*31+Mathf.RoundToInt(v.z*1000);}}}
    Check(validMesh&&material&&shell.sharedMaterial.shader.name=="GreyHarbor/World","2714 Actual new detail mesh uses world visibility shader "+i);Check(envelope,"2714 Actual surface details remain within3.5cm envelope of preserved cargo collider "+i);meshShapes.Add(shapeHash);decorated++;CargoContentRow2714("appearance",box.View.name,position,"variant="+appearance.Variant+" palette="+appearance.PaletteName+" actualShellColor="+shell.sharedMaterial.color+" meshHash="+shapeHash);
   }
   Check(decorated==64&&variants.Count>=4&&colors.Count>=4&&meshShapes.Count>=4,"2714 Actual64 container surfaces have multiple geometry variants and real shell colors variants="+variants.Count+" colors="+colors.Count+" meshes="+meshShapes.Count);
  }
  void CargoNewPhysical2714(){
   var map=G.Map;var area=RaidMap.CargoBounds2712;Check(map.CargoDressingRecords2714.Count>0&&map.CargoDressingViews2714.Count==map.CargoDressingRecords2714.Count,"2714 New port decoration records match actual view objects");Check(map.CargoLoot2714.Count>1&&map.CargoLoot2714.Count==map.CargoLootRecords2714.Count,"2714 Actual searchable containers match registered cargo content records");
   var colliders=new List<Collider>();var owners=new Dictionary<Collider,GameObject>();foreach(var box in map.CargoBoxes2712){var c=box.View.GetComponent<Collider>();colliders.Add(c);owners[c]=box.View;}
   foreach(var record in map.CargoDressingRecords2714){Check(record.View!=null&&map.CargoDressingViews2714.Contains(record.View),"2714 Decoration record refers to actual scene object "+record.Type);CargoNewColliders2714(record.View,area,colliders,owners);CargoContentRow2714("decoration",record.Type.ToString(),record.View.transform.position,"actualBounds="+record.Footprint+" kind="+record.Kind);}
   foreach(var record in map.CargoLootRecords2714){Check(record.Container!=null&&record.Model!=null&&map.CargoLoot2714.Contains(record.Container)&&map.Loot.Contains(record.Container)&&Vector3.Distance(record.Stand,record.Container.Position)<.002f,"2714 New searchable object is registered with actual stand "+record.Container.Name);CargoNewColliders2714(record.Model,area,colliders,owners);}
   int overlap=0;for(int i=0;i<colliders.Count;i++)for(int j=i+1;j<colliders.Count;j++){var a=colliders[i];var b=colliders[j];if(owners[a]==owners[b]||!a.bounds.Intersects(b.bounds))continue;Vector3 direction;float depth;if(Physics.ComputePenetration(a,a.transform.position,a.transform.rotation,b,b.transform.position,b.transform.rotation,out direction,out depth)&&depth>.005f){if(overlap++<20)Debug.Log("2714 ILLEGAL CONTENT OVERLAP "+a.name+" / "+b.name+" depth="+depth+" a="+a.bounds+" b="+b.bounds);}}
   Check(overlap==0,"2714 New physical decoration/search props do not intersect each other or preserved cargo boxes overlaps="+overlap);Check(!G.Enemies.Exists(e=>area.Contains(new Vector2(e.Home.x,e.Home.z))||area.Contains(new Vector2(e.transform.position.x,e.transform.position.z))),"2714 Central cargo content adds no new NPCs");Check(map.CargoReservations2712.Count==0&&map.CargoSearchReserved2712.Count==0&&map.CargoMonsterReserved2712.Count==0,"2714 Removed reservation records remain empty");
  }
  void CargoNewColliders2714(GameObject view,Rect area,List<Collider> colliders,Dictionary<Collider,GameObject> owners){
   bool rendererFound=false;foreach(var renderer in view.GetComponentsInChildren<Renderer>()){rendererFound=true;var b=renderer.bounds;Check(b.min.x>=area.xMin-.005f&&b.max.x<=area.xMax+.005f&&b.min.z>=area.yMin-.005f&&b.max.z<=area.yMax+.005f,"2714 Actual new visible content remains inside central boundary "+view.name+" "+renderer.name);}
   Check(rendererFound,"2714 New content has real visible renderers "+view.name);foreach(var c in view.GetComponentsInChildren<Collider>())if(c.enabled&&!c.isTrigger&&!owners.ContainsKey(c)&&(SceneObstacle.MovementMask&(1<<c.gameObject.layer))!=0){colliders.Add(c);owners[c]=view;}
  }
  void CargoExistingEconomy2714(){
   double[] common={60,32,7,1,0},small={5,30,40,23,0};int[] counts={200,500,220,55,20,5};var quality=LootBalance.QualityWeights(ContainerTier.Common);var safeQuality=MainSafeRules240.Quality(SafeSize240.Small);bool same=true;for(int i=0;i<5;i++)same&=quality[i]==common[i]&&safeQuality[i]==small[i];for(int i=0;i<6;i++)same&=LootBalance.CountWeights[i]==counts[i];Check(same&&ContainerDropRules230.BonusRedChance==.001&&MainSafeRules240.RedChance(SafeSize240.Small)==.01,"2714 Existing ordinary/small-safe quantity and rarity coefficients unchanged");
   int safes=0;foreach(var record in G.Map.CargoLootRecords2714){var box=record.Container;var profile=box.DropProfile230;Check(profile!=null&&profile.Region==DropRegion230.Containers&&profile.FixedCount==0&&profile.EnableBonusRed,"2714 New cargo box uses existing Containers region profile without fixed-count override "+box.Name);
    if(record.Safe){safes++;Check(box.Prop.Look==ContainerLook.Safe&&profile.Tier==ContainerTier.Safe&&profile.SafeSize==SafeSize240.Small,"2714 Unique small safe uses existing small-safe rules "+box.Name);}else Check(profile.Tier==ContainerTier.Common&&profile.SafeSize==SafeSize240.Legacy,"2714 Other new boxes use ordinary unchanged rules "+box.Name);
    Check(box.Items.Count>=1&&box.Items.Count<=7&&box.Items.TrueForAll(item=>ContainerDropRules230.Eligible(profile,item.Kind,RaidInventory.Grade(item.Kind)==4)),"2714 Actual generated cargo contents remain bounded and profile-eligible "+box.Name);}
   Check(safes==1,"2714 Exactly one actual small insurance safe; other new containers ordinary");
  }
  IEnumerator CargoSearch2714(){
   foreach(var record in G.Map.CargoLootRecords2714){var box=record.Container;int count=box.Items.Count;string identity=JsonUtility.ToJson(new CargoItems2714{Items=box.Items.ToArray()});CargoWalk2713(box.Position,"2714 actual new search stand "+box.Name);G.Rig.Tick(0);G.Player.Aim=(box.Prop.transform.position-G.Player.transform.position).normalized;G.Vision.Refresh(G.Player.transform.position,G.Player.Aim);Check(G.CanInteractLoot(box),"2714 Actual prop is visible and in interaction range "+box.Name);
    // Entry requires a released frame; a second released frame selects the target.
    SprintInteract2711(false,0,box.Prop.transform.position);SprintInteract2711(false,0,box.Prop.transform.position);Check(G.Nearby==box,"2714 Actual F selects intended cargo box without adjacent interaction theft "+box.Name+" selected="+(G.Nearby==null?"null":G.Nearby.Name));SprintInteract2711(true,box.SearchDuration+.02f,box.Prop.transform.position);
    Check(box.Opened&&G.OpenLoot==box&&box.Prop.Searched&&box.Items.Count==count,"2714 Actual F search opens intended new container and retains exact contents "+box.Name);Check(JsonUtility.ToJson(new CargoItems2714{Items=box.Items.ToArray()})==identity,"2714 Search animation does not reroll original contents "+box.Name);G.Paused=false;for(int i=0;i<7;i++)G.TickIdentification(3);G.Paused=true;Check(box.Items.Count==count&&box.Items.TrueForAll(item=>item.Identified),"2714 All actual searched cargo contents identify successfully "+box.Name);CargoContentRow2714("actual-F-search",box.Name,box.Position,"model="+box.Prop.transform.position+" look="+record.Look+" safe="+record.Safe+" count="+count+" searched=true");G.CloseInventory();SprintInteract2711(false,0,box.Prop.transform.position);yield return null;
   }
  }
  [Serializable] sealed class CargoItems2714 {public ItemStack[] Items;}
  IEnumerator CargoPicture2714(string name,Vector3 target,float yaw,float size,bool overhead,int width,int height){
   var go=new GameObject("CargoContentReviewCamera2714");var cam=go.AddComponent<Camera>();cam.CopyFrom(G.View);cam.enabled=false;cam.allowHDR=false;cam.orthographic=true;cam.nearClipPlane=.1f;cam.farClipPlane=250;cam.orthographicSize=size;cam.transform.rotation=Quaternion.Euler(overhead?90:52,overhead?0:yaw,0);cam.transform.position=target-cam.transform.forward*(overhead?100:110);
   float fog=Shader.GetGlobalFloat("_HarborFogEnabled");var ambient=RenderSettings.ambientLight;var sunObject=GameObject.Find("LateAfternoon");var sun=sunObject?sunObject.GetComponent<Light>():null;float power=sun?sun.intensity:0,shadowDistance=QualitySettings.shadowDistance;
   try{yield return null;Shader.SetGlobalFloat("_HarborFogEnabled",0);RenderSettings.ambientLight=new Color(.33f,.36f,.35f);if(sun)sun.intensity=.64f;QualitySettings.shadowDistance=170;var image=HvRender277(cam,name,width,height);try{CargoImageCheck2713(image,name);}finally{Destroy(image);}}finally{Shader.SetGlobalFloat("_HarborFogEnabled",fog);RenderSettings.ambientLight=ambient;if(sun)sun.intensity=power;QualitySettings.shadowDistance=shadowDistance;Destroy(go);}
  }
  IEnumerator CargoPictures2714(){
   var area=RaidMap.CargoBounds2712;var center=new Vector3(area.center.x,0,area.center.y);yield return CargoPicture2714("2714-cargo-overhead-scene.png",center,0,area.height*.5f+4,true,1800,1800);yield return CargoPicture2714("2714-cargo-oblique-scene.png",center,35,42,false,1800,1200);
   var reefer=G.Map.CargoBoxes2712.Find(b=>b.View.GetComponent<CargoAppearance2714>().Variant==CargoVariant2714.Refrigerated);Check(reefer!=null,"2714 Scene has actual refrigerated variant for detail review");yield return CargoPicture2714("2714-refrigerated-container-detail.png",reefer.View.transform.position,125,8.5f,false,1800,1200);
   var patched=G.Map.CargoBoxes2712.Find(b=>b.View.GetComponent<CargoAppearance2714>().Variant==CargoVariant2714.Patched);Check(patched!=null,"2714 Scene has actual patched variant for detail review");yield return CargoPicture2714("2714-patched-container-and-port-detail.png",patched.View.transform.position,35,12,false,1800,1200);
   var safe=G.Map.CargoLootRecords2714.Find(r=>r.Safe);Check(safe!=null,"2714 Actual small-safe detail subject exists");yield return CargoPicture2714("2714-port-clutter-and-search-safe-detail.png",safe.Model.transform.position+Vector3.up*.4f,safe.Yaw+25,8.5f,false,1800,1200);
   var clutter=G.Map.CargoDressingRecords2714.Find(r=>r.Type.ToString().Contains("Pile")||r.Type.ToString().Contains("堆")||r.Type.ToString().Contains("Forklift"))??G.Map.CargoDressingRecords2714[0];yield return CargoPicture2714("2714-port-decoration-and-scattered-items-detail.png",clutter.View.transform.position+Vector3.up*.65f,clutter.View.transform.eulerAngles.y+25,8.5f,false,1800,1200);
   File.WriteAllText(Path.Combine(folder,"2714-images-note.txt"),"Actual Camera.Render scene geometry; whole-layout/detail review lighting only: fog off, ambient(.33,.36,.35), sun .64, shadow distance170m, all restored. No synthetic HUD, OnGUI not captured. Original64 collision geometry tested against actual2.7.13 runtime baseline. Shared2713 route/width/image helpers only; old pure-container/no-loot geometry suite is not called.\n");
  }
  IEnumerator VerifyCargoContent2714(){
   Check(Application.version=="2.7.14"&&Path.GetFileName(G.SavePath)=="verification-stash-2.7.14.json","2714 Dedicated build and isolated content inventory");string directory=Path.GetDirectoryName(G.SavePath);string[] formal={"stash-v40.json","stash-v41.json","stash-v42.json"};byte[][] before=Array.ConvertAll(formal,n=>File.Exists(Path.Combine(directory,n))?File.ReadAllBytes(Path.Combine(directory,n)):null);var controls=G.Controls;var random=UnityEngine.Random.state;bool harbor=G.HarborWalkthrough;
   FreshCamp();string camp=JsonUtility.ToJson(G.Save);G.HarborWalkthrough=true;G.HardMode=false;Check(G.BeginTraining(),"2714 Actual isolated harbor content session");G.Paused=true;yield return null;
   try{G.Controls=new TouchInput();Check(CargoPreview2712.Apply(G),"2714 Actual cargo approach remains accessible");CargoOriginalAndArt2714();CargoNewPhysical2714();if(Array.IndexOf(Environment.GetCommandLineArgs(),"--capture-cargo-content-2714")<0){CargoExistingEconomy2714();yield return CargoRoutes2713();yield return CargoSearch2714();}else results.Add("INFO 2714 Capture-only: art and scene images checked; passed physical/search suites skipped.");yield return CargoPictures2714();File.WriteAllLines(Path.Combine(folder,Array.IndexOf(Environment.GetCommandLineArgs(),"--capture-cargo-content-2714")>=0?"2714-capture-photometry.csv":"2714-shared-actual-route-width-and-image.csv"),cargoRows2713);G.Finish(false);yield return null;Check(JsonUtility.ToJson(G.Save)==camp,"2714 Preview/search preserves original inventory without economy mutations");for(int i=0;i<formal.Length;i++)Check(MainBytes273(before[i],File.Exists(Path.Combine(directory,formal[i]))?File.ReadAllBytes(Path.Combine(directory,formal[i])):null),"2714 Formal save remains byte-identical "+formal[i]);}
   finally{G.Controls=controls;G.HarborWalkthrough=harbor;UnityEngine.Random.state=random;G.Paused=true;File.WriteAllLines(Path.Combine(folder,Array.IndexOf(Environment.GetCommandLineArgs(),"--capture-cargo-content-2714")>=0?"2714-cargo-capture-art.csv":"2714-cargo-actual-content.csv"),cargoContentRows2714);File.WriteAllLines(Path.Combine(folder,Array.IndexOf(Environment.GetCommandLineArgs(),"--capture-cargo-content-2714")>=0?"2714-capture-photometry.csv":"2714-shared-actual-route-width-and-image.csv"),cargoRows2713);}
  }
 }
}

