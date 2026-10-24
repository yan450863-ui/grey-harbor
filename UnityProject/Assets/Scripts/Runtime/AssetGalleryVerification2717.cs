using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using UnityEngine;
namespace GreyHarbor {
 public sealed partial class RaidVerification {
  // This suite is entered after the real BeginTraining gallery branch. It does
  // not construct a substitute map or touch combat, loot tables or saved items.
  readonly List<string> galleryRows2717=new List<string>();
  readonly HashSet<string> galleryPalette2717=new HashSet<string>();
  bool GalleryContentOnly2717=>Array.IndexOf(Environment.GetCommandLineArgs(),"--verify-gallery-content-2717")>=0;
  static readonly HashSet<FutureProp2717> changedGalleryProps2717=new HashSet<FutureProp2717>{FutureProp2717.SingleWorkstation,FutureProp2717.DoubleWorkstation,FutureProp2717.CornerWorkstation,FutureProp2717.CRTRig,FutureProp2717.FlatMonitorRig,FutureProp2717.BrokenMonitor,FutureProp2717.ExecutiveDesk,FutureProp2717.SecretaryIsland,FutureProp2717.FourSeatBench,FutureProp2717.CubiclePair,FutureProp2717.SecurityDesk,FutureProp2717.DraftingTable,FutureProp2717.PlanMapBoard,FutureProp2717.ProjectionScreen,FutureProp2717.LobbyDirectory,FutureProp2717.Turnstile};
  static bool GalleryFinite2717(float f)=>!float.IsNaN(f)&&!float.IsInfinity(f);
  static bool GalleryFinite2717(Vector3 p)=>GalleryFinite2717(p.x)&&GalleryFinite2717(p.y)&&GalleryFinite2717(p.z);
  static string GalleryQuote2717(string s)=>"\""+(s??"").Replace("\"","\"\"")+"\"";
  void GalleryRow2717(string type,string id,Vector3 p,string detail){galleryRows2717.Add(GalleryQuote2717(type)+","+GalleryQuote2717(id)+","+p.x.ToString("R",CultureInfo.InvariantCulture)+","+p.y.ToString("R",CultureInfo.InvariantCulture)+","+p.z.ToString("R",CultureInfo.InvariantCulture)+","+GalleryQuote2717(detail));}
  Bounds GalleryBounds2717(GameObject view){var renderers=view.GetComponentsInChildren<MeshRenderer>();Check(renderers.Length>0,"2717 Actual baked renderers exist "+view.name);var b=renderers[0].bounds;foreach(var r in renderers)b.Encapsulate(r.bounds);return b;}
  void GalleryModel2717(GameObject view,FutureAssetSpec2717 spec,string label){
   Check(view&&view.activeInHierarchy,"2717 Actual model is active "+label);var info=view.GetComponent<FutureAssetInfo2717>();
   Check(info&&info.Id==spec.Id&&info.DisplayName==spec.Name&&info.Envelope==spec.Size,"2717 Reusable model metadata matches catalog "+label);
   var filters=view.GetComponentsInChildren<MeshFilter>();bool geometry=filters.Length>0;int vertices=0,triangles=0;
   foreach(var filter in filters){var mesh=filter.sharedMesh;if(!mesh){geometry=false;continue;}var v=mesh.vertices;var n=mesh.normals;var t=mesh.triangles;vertices+=v.Length;triangles+=t.Length/3;geometry&=v.Length>=3&&t.Length>=3&&t.Length%3==0&&n.Length==v.Length&&GalleryFinite2717(mesh.bounds.center)&&GalleryFinite2717(mesh.bounds.size);
    foreach(var p in v)geometry&=GalleryFinite2717(p)&&p.sqrMagnitude<10000;foreach(var normal in n)geometry&=GalleryFinite2717(normal);foreach(int index in t)geometry&=index>=0&&index<v.Length;}
   Check(geometry,"2717 Actual meshes contain finite vertices/normals and valid triangles "+label+" vertices="+vertices+" triangles="+triangles);
   bool materials=true,colored=false;int materialCount=0;foreach(var r in view.GetComponentsInChildren<MeshRenderer>())foreach(var m in r.sharedMaterials){materialCount++;if(!m||!m.shader||!m.HasProperty("_Color")){materials=false;continue;}var c=m.color;materials&=GalleryFinite2717(new Vector3(c.r,c.g,c.b))&&GalleryFinite2717(c.a)&&c.a>.99f&&m.shader==GreyHarbor.FlashlightShaders2763.Load("HarborWorld");colored|=Mathf.Max(c.r,c.g,c.b)-Mathf.Min(c.r,c.g,c.b)>.035f;galleryPalette2717.Add(ColorUtility.ToHtmlStringRGB(c));}
   Check(materials&&materialCount>0&&colored,"2717 Model retains colored opaque world-shader materials "+label+" materials="+materialCount);
   var bounds=GalleryBounds2717(view);Check(GalleryFinite2717(bounds.center)&&GalleryFinite2717(bounds.size)&&bounds.size.sqrMagnitude>0,"2717 Actual model bounds are finite "+label);
   // Some reusable wall fittings deliberately have a short drain/tray below
   // their local origin. Check actual ground contact, not catalog dimensions.
   Check(Mathf.Abs(bounds.min.y-view.transform.position.y)<.012f&&Vector3.Distance(info.ActualSize,info.LocalBounds.size)<.001f,"2717 Model sits exactly at its physical floor origin and records measured size "+label+" bottom="+bounds.min.y);
   int physical=0;bool layers=true;foreach(var c in view.GetComponentsInChildren<Collider>()){var rule=c.GetComponentInParent<SceneObstacle>();layers&=rule&&GalleryFinite2717(c.bounds.center)&&GalleryFinite2717(c.bounds.size)&&!c.isTrigger;if(!rule)continue;int expected=rule.Kind==ObstacleKind.High?SceneObstacle.HighLayer:rule.Kind==ObstacleKind.Low?SceneObstacle.LowLayer:0;layers&=c.gameObject.layer==expected&&c.enabled==(rule.Kind!=ObstacleKind.Decoration);if(c.enabled)physical++;}
   Check(layers,"2717 Each actual collider has its declared movement/sight layer "+label+" active="+physical);
   foreach(var collider in view.GetComponentsInChildren<Collider>()){var rule=collider.GetComponentInParent<SceneObstacle>();if(!rule||rule.Kind!=ObstacleKind.High)continue;var c=collider.bounds.center;var extents=collider.bounds.extents;RaycastHit hit;bool struck=collider.Raycast(new Ray(c-Vector3.forward*(extents.z+1),Vector3.forward),out hit,2*(extents.z+1));Check(struck&&(SceneObstacle.SightMask&(1<<collider.gameObject.layer))!=0&&(SceneObstacle.ShotMask&(1<<collider.gameObject.layer))!=0,"2717 Actual raised body blocks sight and shots "+label+"/"+collider.name);}
   if(spec.Id==FutureProp2717.Forklift||spec.Id==FutureProp2717.TallFileCabinet||spec.Id==FutureProp2717.SingleWorkstation||spec.Id==FutureProp2717.SluiceGate)Check(physical>0,"2717 Large service/body model has actual movement collision "+label);
   GalleryRow2717("model",label,view.transform.position,"vertices="+vertices+"; triangles="+triangles+"; bakedMeshes="+filters.Length+"; materials="+materialCount+"; movementColliders="+physical+"; bounds="+bounds);
  }
  void GalleryFixture2717(Vector3 p,string label){Physics.SyncTransforms();Check(CargoClear2713(p),"2717 Independent fixture has real ground and .34m capsule clearance "+label);G.Player.transform.position=p;Physics.SyncTransforms();G.Rig.Tick(0);GalleryRow2717("fixture",label,p,"setup only; following route uses actual Actor.Move");}
  void GalleryWalk2717(Vector3 p,string label){bool walked=WalkMain273(p);Check(walked&&CargoClear2713(p),"2717 Actual player .34m swept traversal "+label+" reason="+mainWalkFailure273);GalleryRow2717("walk",label,G.Player.transform.position,"actual Actor.Move; no teleport in tested segment");}
  void GalleryRing2717(AssetShowcaseRecord2717 record){
   var p=record.Position;p.y=0;float x=record.Prop!=null?3.1f:record.Collectible!=null?1.65f:4.9f,z=record.Prop!=null?3.1f:record.Collectible!=null?1.8f:4.9f;
   var points=new[]{record.Approach,p+new Vector3(-x,0,-z),p+new Vector3(-x,0,z),p+new Vector3(x,0,z),p+new Vector3(x,0,-z),record.Approach};
   foreach(var point in points)Check(record.Cell.Contains(new Vector2(point.x,point.z))&&CargoClear2713(point),"2717 Exhibit cell has safe real surrounding stand "+record.Id+" "+point);
   GalleryFixture2717(record.Approach,record.Id+" approach");for(int i=1;i<points.Length;i++){Check(G.Navigation.ClearSegment(points[i-1],points[i]),"2717 Surrounding route is actually swept clear "+record.Id+"/"+i);GalleryWalk2717(points[i],record.Id+" perimeter/"+i);}
  }
  Color32[] GalleryPixels2717(Texture2D source){
   // Runtime imported PNGs may intentionally be non-readable. Reading the GPU
   // texture tests what the build actually imports, including its alpha.
   var rt=RenderTexture.GetTemporary(source.width,source.height,0,RenderTextureFormat.ARGB32,RenderTextureReadWrite.Default);var previous=RenderTexture.active;Texture2D copy=null;
   try{Graphics.Blit(source,rt);RenderTexture.active=rt;copy=new Texture2D(source.width,source.height,TextureFormat.RGBA32,false);copy.ReadPixels(new Rect(0,0,source.width,source.height),0,0);copy.Apply();return copy.GetPixels32();}
   finally{RenderTexture.active=previous;RenderTexture.ReleaseTemporary(rt);if(copy)Destroy(copy);}
  }
  void GalleryImage2717(Texture2D texture,string label){
   Check(texture&&texture.width>=64&&texture.height>=64&&texture.width<=2048&&texture.height<=2048,"2717 Actual imported RGB/alpha texture exists at useful resolution "+label);
   var pixels=GalleryPixels2717(texture);int opaque=0,clear=0,colorful=0;var colors=new HashSet<int>();foreach(var p in pixels){if(p.a<8)clear++;if(p.a<200)continue;opaque++;int maximum=Math.Max(p.r,Math.Max(p.g,p.b)),minimum=Math.Min(p.r,Math.Min(p.g,p.b));if(maximum-minimum>20)colorful++;colors.Add((p.r/16<<8)|(p.g/16<<4)|(p.b/16));}
   Check(opaque>pixels.Length*.015f&&clear>pixels.Length*.015f&&colors.Count>=12&&colorful>opaque*.08f,"2717 Imported image retains visible varied RGB and transparent background "+label+" opaque="+opaque+" transparent="+clear+" colored="+colorful+" colors="+colors.Count);
   GalleryRow2717("texture",label,Vector3.zero,"size="+texture.width+"x"+texture.height+"; opaque="+opaque+"; transparent="+clear+"; colored="+colorful+"; colors="+colors.Count);
  }
  void GallerySprite2717(SpriteRenderer face,string label){
   Check(face&&face.sprite&&face.enabled&&face.sharedMaterial&&face.sharedMaterial.shader==Resources.Load<Shader>("HarborSprite"),"2717 Actual sprite uses depth/fog-compatible shader "+label);
   Check(ColorDistance(face.color,Color.white)<.0001f&&Mathf.Abs(face.color.a-1)<.0001f&&(!face.sharedMaterial.HasProperty("_Color")||ColorDistance(face.sharedMaterial.color,Color.white)<.0001f),"2717 Original image RGB is displayed with white tint "+label);
   var facing=face.GetComponent<ShowcaseSpriteFacing2717>();Check(facing&&facing.Face==face,"2717 Sprite has active camera-facing and depth-anchor component "+label);facing.SyncCamera(G.View);var block=new MaterialPropertyBlock();face.GetPropertyBlock(block);Vector4 anchor=block.GetVector("_Anchor");var expected=facing.transform.position+Vector3.up*facing.GroundOffset;
   Check(Vector3.Distance(new Vector3(anchor.x,anchor.y,anchor.z),expected)<.0001f&&Quaternion.Angle(face.transform.rotation,G.View.transform.rotation)<.001f,"2717 Actual sprite camera direction and depth anchor are synchronized "+label);
  }
  IEnumerator GalleryQuarterTurns2717(){var map=G.Map;var kit=new FutureAssetKit2717(map);foreach(var record in map.ShowcaseRecords2717){if(record.Prop==null||(GalleryContentOnly2717&&!changedGalleryProps2717.Contains(record.Prop.Id)))continue;GameObject extra=null;try{
    extra=kit.Build(record.Prop.Id,new Vector3(-24,.055f,-56),90);Physics.SyncTransforms();GalleryModel2717(extra,record.Prop,record.Id+" yaw90");var original=GalleryBounds2717(record.View);var rotated=GalleryBounds2717(extra);
    Check(Mathf.Abs(rotated.size.x-original.size.z)<.002f&&Mathf.Abs(rotated.size.z-original.size.x)<.002f&&Mathf.Abs(rotated.size.y-original.size.y)<.002f,"2717 Reused 90-degree model preserves real metre dimensions "+record.Id);
    var p=extra.transform.position;p.y=0;var ring=new[]{p+new Vector3(-3.1f,0,-3.1f),p+new Vector3(-3.1f,0,3.1f),p+new Vector3(3.1f,0,3.1f),p+new Vector3(3.1f,0,-3.1f),p+new Vector3(-3.1f,0,-3.1f)};
    GalleryFixture2717(ring[0],record.Id+" yaw90 reuse");for(int i=1;i<ring.Length;i++){Check(G.Navigation.ClearSegment(ring[i-1],ring[i]),"2717 Rotated placement perimeter has actual swept clearance "+record.Id+"/"+i);GalleryWalk2717(ring[i],record.Id+" yaw90 perimeter/"+i);}
   }finally{if(extra){extra.SetActive(false);Destroy(extra);}Physics.SyncTransforms();}yield return null;}}
  IEnumerator GalleryPicture2717(string filename,Vector3 target,float size,float pitch=55,float yaw=35,int width=1800,int height=1200){
   yield return new WaitForEndOfFrame();float fog=Shader.GetGlobalFloat("_HarborFogEnabled");var go=new GameObject("GalleryQACamera2717");var cam=go.AddComponent<Camera>();cam.CopyFrom(G.View);cam.enabled=false;cam.orthographic=true;cam.orthographicSize=size;cam.farClipPlane=300;cam.transform.rotation=Quaternion.Euler(pitch,yaw,0);cam.transform.position=target-cam.transform.forward*110;var faces=G.Map.Root.GetComponentsInChildren<ShowcaseSpriteFacing2717>();
   try{Shader.SetGlobalFloat("_HarborFogEnabled",0);foreach(var face in faces)face.SyncCamera(cam);SaveYardCamera136(cam,filename,width,height);GalleryRow2717("actual-camera",filename,target,"orthographicSize="+size+"; synchronized SpriteFacing/_Anchor; real Camera.Render; excludes IMGUI");}
   finally{foreach(var face in faces)if(face)face.SyncCamera(G.View);Shader.SetGlobalFloat("_HarborFogEnabled",fog);Destroy(go);}yield return null;
  }
  IEnumerator GalleryPictures2717(){
   var area=G.Map.Bounds;yield return GalleryPicture2717("2717-asset-gallery-overview.png",new Vector3(area.center.x,0,area.center.y),Mathf.Max(area.width,area.height)*.62f,65,15,2200,1800);
   yield return GalleryPicture2717("2717-office-gallery-preview.png",new Vector3(-10,0,18),27,55,35,2000,1400);
   yield return GalleryPicture2717("2717-office-core-gallery-preview.png",new Vector3(-10,0,59),27,55,35,2000,1400);
   yield return GalleryPicture2717("2717-loading-gallery-preview.png",new Vector3(-10,0,-18),23,55,35,2000,1200);
   yield return GalleryPicture2717("2717-tunnel-gallery-preview.png",new Vector3(-10,0,-41),23,55,35,2000,1200);
   int imageCount=AssetCollectibles2717.All.Length;float lastRow=Mathf.Floor((imageCount-1)/8f);yield return GalleryPicture2717("2717-collectible-gallery-preview.png",new Vector3(52.5f,1,2+lastRow*3),Mathf.Max(17,lastRow*3+6),45,0,2000,1800);
   yield return GalleryPicture2717("2717-office-workstation-preview.png",new Vector3(-30,.5f,3),6,50,25,1800,1200);
   foreach(var record in G.Map.ShowcaseRecords2717)if(record.MonsterId!=null)yield return GalleryPicture2717("2717-monster-"+record.MonsterId+"-poses.png",record.Position+Vector3.up*.7f,3.4f,38,0,1800,1000);
   // Separate sheets keep all real sprite imports legible, rather than
   // substituting their source PNGs for a claimed in-game rendering.
   for(int row=0;row<Mathf.CeilToInt(imageCount/8f);row++)yield return GalleryPicture2717("2717-collectibles-row-"+(row+1)+".png",new Vector3(52.5f,1.1f,2+row*6),5.3f,25,0,2400,650);
  }
  IEnumerator RunAssetGallery2717(){
   int start=results.Count;galleryRows2717.Clear();galleryPalette2717.Clear();galleryRows2717.Add("type,id,x,y,z,detail");var map=G.Map;
   Check(G.AssetShowcaseMode2717&&map!=null&&map.AssetShowcase2717&&G.TrainingActive,"2717 Actual training asset gallery mode is active");
   int expectedProps=FutureAssetCatalog2717.All.Length,expectedImages=AssetCollectibles2717.All.Length,expectedMonsters=AssetMonsterArt2717.Ids.Length,expectedTotal=expectedProps+expectedImages+expectedMonsters;
   bool modelsOnly=Array.IndexOf(Environment.GetCommandLineArgs(),"--verify-models-2717")>=0;
   Check(expectedProps==112&&expectedImages==100&&expectedMonsters==3&&map.ShowcaseRecords2717.Count==expectedTotal,"2717 Complete gallery contains manifest counts: props="+expectedProps+" collectibles="+expectedImages+" monsters="+expectedMonsters+" records="+map.ShowcaseRecords2717.Count);
   Check(map.Loot.Count==map.FutureContainerRecords2717.Count&&map.Loot.Count==8&&G.Enemies.Count==0,"2717 Showcase contains only eight isolated sample containers and no combat actors");
   var ids=new HashSet<string>();int props=0,collectibles=0,monsters=0;var grades=new int[5];var sizes=new Dictionary<int,int>();var dimensions=new HashSet<string>();var resources=new HashSet<string>();
   try{
    foreach(var record in map.ShowcaseRecords2717){Check(record.View&&ids.Add(record.Id)&&GalleryFinite2717(record.Position)&&GalleryFinite2717(record.Approach)&&record.Cell.width>0&&record.Cell.height>0,"2717 Unique real exhibit and finite placement record "+record.Id);
     if(modelsOnly&&record.Prop==null)continue;
     if(GalleryContentOnly2717&&record.Prop!=null&&!changedGalleryProps2717.Contains(record.Prop.Id))continue;
     if(record.Prop!=null){props++;GalleryModel2717(record.View,record.Prop,record.Id);}
     else if(record.Collectible!=null){collectibles++;var item=record.Collectible;Check(item.Grade>=0&&item.Grade<5&&item.Width>=1&&item.Height>=1,"2717 Valid collectible quality and grid dimensions "+record.Id);grades[item.Grade]++;if(!sizes.ContainsKey(item.Cells))sizes[item.Cells]=0;sizes[item.Cells]++;dimensions.Add(item.Width+"x"+item.Height);Check(resources.Add(item.Resource),"2717 Collectible has its own image resource "+record.Id);GalleryImage2717(item.Texture,record.Id);var faces=record.View.GetComponentsInChildren<SpriteRenderer>();Check(faces.Length==1&&faces[0].sprite.texture==item.Texture,"2717 Actual collectible display uses its imported resource "+record.Id);GallerySprite2717(faces[0],record.Id);}
     else {monsters++;Check(Array.IndexOf(AssetMonsterArt2717.Ids,record.MonsterId)>=0&&record.View.GetComponentsInChildren<Collider>().Length==0,"2717 New monster is a noncombat two-pose display "+record.Id);var faces=record.View.GetComponentsInChildren<SpriteRenderer>();Check(faces.Length==2,"2717 Monster idle and windup are both actually instantiated "+record.Id);
      foreach(var face in faces){bool windup=face.name=="Windup";Check(face.sprite==AssetMonsterArt2717.Get(record.MonsterId,windup),"2717 Actual monster pose uses its correct sprite "+record.Id+" "+face.name);GalleryImage2717(face.sprite.texture,record.Id+" "+face.name);GallerySprite2717(face,record.Id+" "+face.name);}Check(faces[0].sprite.texture!=faces[1].sprite.texture,"2717 Monster poses use distinct imported images "+record.Id);}
     GalleryRing2717(record);yield return null;
    }
    Check(props==(GalleryContentOnly2717?changedGalleryProps2717.Count:expectedProps)&&(modelsOnly||(collectibles==expectedImages&&monsters==expectedMonsters))&&galleryPalette2717.Count>=12,"2717 Scoped real exhibit coverage and varied material palette colors="+galleryPalette2717.Count+" props="+props+" images="+collectibles+" monsters="+monsters);
    if(!modelsOnly){Check(grades[1]==39&&grades[0]==24&&grades[2]==24&&grades[3]==7&&grades[4]==6,"2717 Blue-most, green/purple-next, few gold and retained special red asset counts");for(int i=0;i<5;i++)Check(grades[i]>0,"2717 Actual collectible quality coverage "+AssetCollectibles2717.GradeName(i)+" count="+grades[i]);for(int i=1;i<=4;i++)Check(sizes.ContainsKey(i)&&sizes[i]>0,"2717 Actual collectible grid-size coverage cells="+i+" count="+(sizes.ContainsKey(i)?sizes[i]:0));foreach(var size in sizes)GalleryRow2717("grid-size",size.Key+" cells",Vector3.zero,"count="+size.Value);foreach(string dimension in dimensions)GalleryRow2717("grid-dimensions",dimension,Vector3.zero,"actually instantiated manifest coverage; larger multi-cell images permitted");}
    foreach(var startPoint in map.ShowcaseSectorStarts2717){GalleryFixture2717(new Vector3(26,0,-54),"central spine start");GalleryWalk2717(startPoint,"sector access");}
    yield return GalleryQuarterTurns2717();Check(map.ShowcaseRecords2717.Count==expectedTotal&&map.Root.GetComponentsInChildren<FutureAssetInfo2717>().Length==expectedProps+6,"2717 Temporary quarter-turn instances are cleaned up without changing gallery records");
    yield return GalleryPictures2717();GalleryFixture2717(map.Spawn,"gallery spawn");GalleryWalk2717(map.Extraction,"real southern exit access");
   }finally{File.WriteAllLines(Path.Combine(folder,"2717-asset-gallery-scene.csv"),galleryRows2717);var report=new List<string>();for(int i=start;i<results.Count;i++)report.Add(results[i]);report.Add("INFO Scope: new asset gallery only; mesh/material/collider/import validation, real .34m Actor.Move perimeter traversal, 90-degree reuse and actual scene Camera.Render. No historical combat/loot suites.");File.WriteAllLines(Path.Combine(folder,"2717-asset-gallery-verification.txt"),report);}
  }
 }
}
