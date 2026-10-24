using System;using System.IO;using System.Collections;using System.Collections.Generic;using System.Globalization;using System.Text.RegularExpressions;using UnityEngine;
namespace GreyHarbor {public sealed partial class RaidVerification {
 IEnumerator GroundRoadPhoto2750(Vector3 center,string name,float zoom,bool top=false){
  var go=new GameObject("GroundRoadNormalLightCamera2750");var cam=go.AddComponent<Camera>();cam.CopyFrom(G.View);cam.enabled=false;cam.allowHDR=false;cam.orthographic=true;cam.orthographicSize=zoom;cam.nearClipPlane=.1f;cam.farClipPlane=500;cam.transform.rotation=top?Quaternion.Euler(90,0,0):Quaternion.Euler(58,35,0);cam.transform.position=center-cam.transform.forward*240;
  float fog=Shader.GetGlobalFloat("_HarborFogEnabled");Color ambient=RenderSettings.ambientLight;try{yield return null;Shader.SetGlobalFloat("_HarborFogEnabled",0);foreach(var f in G.Map.Root.GetComponentsInChildren<ShowcaseSpriteFacing2717>())f.SyncCamera(cam);var image=HvRender277(cam,name,top?2080:1600,top?1800:1100);try{CargoImageCheck2713(image,name);}finally{Destroy(image);}Check(RenderSettings.ambientLight==ambient,"2750 review camera retains actual scene light "+name);}finally{Shader.SetGlobalFloat("_HarborFogEnabled",fog);foreach(var f in G.Map.Root.GetComponentsInChildren<ShowcaseSpriteFacing2717>())f.SyncCamera(G.View);Destroy(go);}
 }
 void GroundRoadDefinitions2750(RaidMap map){
  string old=Path.GetFullPath(Path.Combine(Application.dataPath,"../../../灰港行动_2.7.49/UnityProject/Assets/Scripts/Runtime/HarborBuildings135.cs"));string source=File.ReadAllText(old);int previous=0;
  foreach(Match road in Regex.Matches(source,@"RoadPath\(([0-9.]+)f?,(.*?)\);")){
   var pts=new List<Vector3>();foreach(Match point in Regex.Matches(road.Groups[2].Value,@"PortPoint\(([0-9.]+)f?,([0-9.]+)f?\)"))pts.Add(RaidMap.PortPoint(float.Parse(point.Groups[1].Value,CultureInfo.InvariantCulture),float.Parse(point.Groups[2].Value,CultureInfo.InvariantCulture)));
   if(pts.Count<2||Mathf.Abs(pts[0].x-RaidMap.PortPoint(83,137).x)<.001f)continue;previous++;float width=float.Parse(road.Groups[1].Value,CultureInfo.InvariantCulture);bool match=false;
   foreach(var actual in map.HarborRoadRecords2717){if(Mathf.Abs(actual.Width-width)>.001f||actual.Points.Length!=pts.Count)continue;bool same=true;for(int i=0;i<pts.Count;i++)same&=Vector3.Distance(pts[i],actual.Points[i])<.001f;match|=same;}Check(match,"2750 previous approved road alignment and width preserved route "+previous);
  }
  Check(previous==11&&map.HarborRoadRecords2717.Count==13,"2750 only two office approaches replace old axis and turn");
  foreach(float x in new[]{70f,93f}){
   var branch=map.HarborRoadRecords2717.Find(r=>r.Points.Length==2&&Vector3.Distance(r.Points[0],RaidMap.PortPoint(x,137))<.001f&&Vector3.Distance(r.Points[1],RaidMap.PortPoint(x,155))<.001f);Check(branch!=null&&Mathf.Abs(branch.Width-5)<.001f,"2750 independent five-metre office approach north road to tower door x="+x);
   foreach(float z in new[]{140f,145f,150f})Check(map.OutdoorRoadWeight2749(new Vector2(x-79,z-73))>.95f,"2750 both office branches painted asphalt at correct world point "+x+","+z);
  }
  foreach(float z in new[]{143f,147f,150f})Check(map.OutdoorRoadWeight2749(new Vector2(83-79,z-73))<.01f,"2750 removed centre office axis is not a residual asphalt road z="+z);
 }
 IEnumerator GroundOfficeWalk2750(RaidMap outdoor){
  foreach(float x in new[]{70f,93f}){
   FreightRoomPlace2717(RaidMap.PortPoint(x,137),"2750 office approach starts on existing north road");
   foreach(float z in new[]{140f,145f,150.5f}){var p=RaidMap.PortPoint(x,z);Check(outdoor.OnHarborSurface(p)&&CargoClear2713(p),"2750 road shoulders and paint do not block approach capsule "+p);FreightRoomWalk2717(p,"2750 actual player walks new independent branch");yield return null;}
   if(x==70){var blocked=outdoor.Root.Find("OfficeBlockedCar2746");Check(blocked&&blocked.GetComponentsInChildren<Collider>().Length>0,"2750 left-door overturned vehicle still physical");var start=G.Player.transform.position;for(int i=0;i<50;i++)G.Player.Move(Vector3.forward*.1f);Physics.SyncTransforms();var stop=G.Player.transform.position;Check(stop.z>start.z+.2f&&stop.z<RaidMap.PortPoint(70,153.6f).z-.3f,"2750 actual left approach stops at original vehicle");Check(Physics.CapsuleCast(stop+Vector3.up*.35f,stop+Vector3.up,.3f,Vector3.forward,out var hit,.35f,SceneObstacle.MovementMask,QueryTriggerInteraction.Ignore)&&hit.collider.transform.IsChildOf(blocked),"2750 left entry remains blocked by car not new road dressing");G.Player.Move(Vector3.back*.4f);Physics.SyncTransforms();Check(CargoClear2713(G.Player.transform.position),"2750 blocked player can retreat from vehicle");}
  }
  FreightRoomWalk2717(RaidMap.OfficeArrival2726,"2750 right branch reaches existing arrival");var door=outdoor.Doors.Find(d=>d.Portal&&d.Destination=="officeright1");FreightMonsterF2720(door,true);yield return null;Check(G.Map.InteriorId=="officeright1"&&CargoClear2713(G.Player.transform.position),"2750 actual held F still enters right tower");FreightMonsterF2720(G.Map.OfficeExit2726,true);yield return null;Check(G.Map==outdoor&&CargoClear2713(G.Player.transform.position),"2750 actual held F returns safely to new right branch");
  foreach(var p in new[]{RaidMap.PortPoint(81.5f,157),RaidMap.PortPoint(81.5f,167.5f),RaidMap.PortPoint(81.5f,172)}){Check(outdoor.OnHarborSurface(p)&&CargoClear2713(p),"2750 bridge ground gap stays open after road shoulders "+p);FreightRoomWalk2717(p,"2750 actual walking through gap below bridge");yield return null;}
 }
 void GroundCache2750(RaidMap map){
  var cache=Resources.Load<HarborGroundCache2721>(HarborGroundCache2721.ResourceName);Check(cache&&cache.Version==2750&&HarborGroundCache2721.AlgorithmVersion==2750&&map.GroundCacheUsed2721,"2750 actual outdoor uses rebaked material and road cache");Check(cache.Texture==map.StableGround137&&cache.Bounds==map.StableGroundBounds137&&cache.PaintFingerprint==map.GroundPaintFingerprint2721,"2750 real rendering texture matches its cache identity");var plane=map.Root.Find("Harbor_StableGround137");Check(plane&&plane.GetComponent<MeshFilter>().sharedMesh.vertexCount==4&&plane.GetComponentsInChildren<Collider>().Length==0&&map.StableGround137.mipmapCount>1,"2750 unified six finishes keep one mipmapped nonblocking ground plane");
  var oldTexture=new Texture2D(2,2,TextureFormat.RGBA32,false);try{string old=Path.GetFullPath(Path.Combine(Application.dataPath,"../../../灰港行动_2.7.49/UnityProject/Assets/Environment/HarborGround2721/HarborStableGround2721.png"));Check(oldTexture.LoadImage(File.ReadAllBytes(old)),"2750 previous real cache loaded for platform and alpha preservation");Check(oldTexture.width==map.StableGround137.width&&oldTexture.height==map.StableGround137.height,"2750 material revision retains pixel geometry");
   foreach(var p in new[]{new Vector2(103,62),new Vector2(103,57),new Vector2(101,-23),new Vector2(114,31),new Vector2(100,53)}){var a=GroundPixel2749(map,oldTexture,p);var b=GroundPixel2749(map,map.StableGround137,p);Check(Mathf.Abs(a.r-b.r)+Mathf.Abs(a.g-b.g)+Mathf.Abs(a.b-b.b)+Mathf.Abs(a.a-b.a)<.012f,"2750 steel wood decks gangways keep original natural material "+p);}
   for(int z=0;z<9;z++)for(int x=0;x<11;x++){int px=Mathf.RoundToInt((oldTexture.width-1)*x/10f),pz=Mathf.RoundToInt((oldTexture.height-1)*z/8f);Check(Mathf.Abs(oldTexture.GetPixel(px,pz).a-map.StableGround137.GetPixel(px,pz).a)<.001f,"2750 shoreline and outside-port alpha preserved sample "+x+","+z);}
  }finally{Destroy(oldTexture);}File.WriteAllBytes(Path.Combine(folder,"2750-entire-port-ground-cache.png"),map.StableGround137.EncodeToPNG());
 }
 void GroundMaterials2750(RaidMap map){
  var finishes=new[]{GroundFinish.Asphalt,GroundFinish.Concrete,GroundFinish.PatchedConcrete,GroundFinish.Gravel,GroundFinish.Soil,GroundFinish.Steel};var preferred=new[]{RaidMap.PortPoint(51,96),RaidMap.PortPoint(105,170),RaidMap.PortPoint(92,90),RaidMap.PortPoint(43,132),RaidMap.PortPoint(24,25),new Vector3(100,0,53)};var means=new float[6];var deviations=new float[6];
  const int size=192;var sampleSheet=new Texture2D(size*6,size,TextureFormat.RGBA32,false);var sheet=new Color32[size*size*6];var labels=new List<string>{"Index\tMaterial\tWorldX\tWorldZ\tMean\tDeviation"};
  for(int n=0;n<finishes.Length;n++){
   Vector2 center=new Vector2(preferred[n].x,preferred[n].z);bool found=false;
   for(int radius=0;radius<=30&&!found;radius++)for(int zz=-radius;zz<=radius&&!found;zz++)for(int xx=-radius;xx<=radius&&!found;xx++){
    if(Mathf.Max(Mathf.Abs(xx),Mathf.Abs(zz))!=radius)continue;var p=new Vector2(preferred[n].x+xx,preferred[n].z+zz);if(!map.OnHarborSurface(new Vector3(p.x,0,p.y))||map.OutdoorGroundType2750(p)!=finishes[n])continue;bool same=true;
    foreach(var offset in new[]{new Vector2(1.2f,0),new Vector2(-1.2f,0),new Vector2(0,1.2f),new Vector2(0,-1.2f)})same&=map.OutdoorGroundType2750(p+offset)==finishes[n];if(same){center=p;found=true;}
   }
   Check(found,"2750 approved six-purpose material exists in assigned local area "+finishes[n]);double sum=0,squares=0;
   for(int y=0;y<size;y++)for(int x=0;x<size;x++){var c=GroundPixel2749(map,map.StableGround137,center+new Vector2((x/(float)(size-1)-.5f)*2.4f,(y/(float)(size-1)-.5f)*2.4f));sheet[y*sampleSheet.width+n*size+x]=c;float light=(c.r+c.g+c.b)/3;sum+=light;squares+=light*light;}
   means[n]=(float)(sum/(size*size));deviations[n]=(float)Math.Sqrt(Math.Max(0,squares/(size*size)-means[n]*means[n]));Check(deviations[n]>.004f,"2750 actual material retains aggregate joints or roughness rather than flat color "+finishes[n]);labels.Add(n+"\t"+finishes[n]+"\t"+center.x+"\t"+center.y+"\t"+means[n]+"\t"+deviations[n]);
  }
  sampleSheet.SetPixels32(sheet);sampleSheet.Apply();File.WriteAllBytes(Path.Combine(folder,"2750-six-actual-ground-material-samples.png"),sampleSheet.EncodeToPNG());Destroy(sampleSheet);File.WriteAllLines(Path.Combine(folder,"2750-ground-material-samples.tsv"),labels);
  Check(means[1]>means[0]+.1f&&means[1]>means[2]+.035f&&Mathf.Abs(means[3]-means[4])>.015f,"2750 real asphalt concrete repair gravel soil preserve distinct purpose values");
  var seen=new HashSet<string>();int visiblePaint=0;foreach(var stamp in map.RoadDetailStamps2750){seen.Add(stamp.Name);Check(stamp.Priority>=.42f&&stamp.Size.x>0&&stamp.Size.y>0,"2750 road art uses valid high-priority baked stamp "+stamp.Name);
   if(stamp.Name!="CenterDash"&&stamp.Name!="EdgeLine"&&stamp.Name!="StopLine")continue;var c=GroundPixel2749(map,map.StableGround137,new Vector2(stamp.Position.x,stamp.Position.z));if((c.r+c.g+c.b)/3>.56f)visiblePaint++;
  }
  foreach(var type in RoadMarkKit2750.Names)Check(seen.Contains(type),"2750 approved road detail family present without decal objects "+type);Check(visiblePaint>30,"2750 actual final cache visibly retains many painted dashes edge and stop lines");
  GroundRoadCollisionBaseline2750(map);
 }
 void GroundRoadCollisionBaseline2750(RaidMap map){
  // HarborSurfaceClear describes supporting land, not clearance from existing walls.
  // Replay only the new paint producer and compare actual collider identities and bounds.
  var flags=System.Reflection.BindingFlags.Instance|System.Reflection.BindingFlags.NonPublic;
  var paintField=typeof(RaidMap).GetField("groundPaint137",flags);var accentField=typeof(RaidMap).GetField("outdoorAccents2749",flags);
  Check(paintField!=null&&accentField!=null,"2750 QA can preserve ground paint state during real producer replay");
  var paint=paintField.GetValue(map) as IList;var accents=accentField.GetValue(map) as IList;
  Check(paint!=null&&accents!=null,"2750 real ground paint registries captured");
  int paintCount=paint.Count,accentCount=accents.Count;var texture=map.StableGround137;var fingerprint=map.GroundPaintFingerprint2721;
  var stamps=new List<RoadDetailStamp2750>(map.RoadDetailStamps2750);var baseline=new Dictionary<Collider,Bounds>();
  foreach(var c in UnityEngine.Object.FindObjectsOfType<Collider>())baseline.Add(c,c.bounds);
  int rendererCount=map.Root.GetComponentsInChildren<Renderer>(true).Length;bool replaySame=true,collidersSame=true;int clear=0,existingBlocked=0;
  try{
   // This API has a Count>0 idempotence guard; reset its records only for the replay.
   map.RoadDetailStamps2750.Clear();map.BuildHarborRoadDetails2750();Physics.SyncTransforms();
   replaySame=map.RoadDetailStamps2750.Count==stamps.Count;
   if(replaySame)for(int i=0;i<stamps.Count;i++){var a=stamps[i];var b=map.RoadDetailStamps2750[i];replaySame&=a.Name==b.Name&&a.Position==b.Position&&a.Size==b.Size&&a.Priority==b.Priority;}
   var after=UnityEngine.Object.FindObjectsOfType<Collider>();collidersSame=after.Length==baseline.Count;
   foreach(var c in after){Bounds before;collidersSame&=baseline.TryGetValue(c,out before)&&Vector3.Distance(before.center,c.bounds.center)<.00001f&&Vector3.Distance(before.size,c.bounds.size)<.00001f;}
   Check(replaySame,"2750 real road detail replay reproduces identical existing paint records");
   Check(collidersSame&&map.Root.GetComponentsInChildren<Renderer>(true).Length==rendererCount,"2750 real road producer creates no collider or renderer and moves no existing blocker");
   foreach(var stamp in map.RoadDetailStamps2750){
    if(stamp.Name!="RoadShoulder"&&stamp.Name!="DrainGrate")continue;
    var hits=Physics.OverlapCapsule(stamp.Position+Vector3.up*.35f,stamp.Position+Vector3.up*1.35f,.34f,SceneObstacle.MovementMask,QueryTriggerInteraction.Ignore);
    if(hits.Length==0){clear++;continue;}existingBlocked++;
    foreach(var hit in hits){Bounds old;Check(baseline.TryGetValue(hit,out old)&&Vector3.Distance(old.center,hit.bounds.center)<.00001f&&Vector3.Distance(old.size,hit.bounds.size)<.00001f,"2750 blocked cosmetic sample is identical pre-existing collider "+stamp.Position+" / "+hit.name);
     string hierarchy=hit.name;for(var t=hit.transform.parent;t!=null;t=t.parent)hierarchy=t.name+"/"+hierarchy;
     Debug.Log("2750 EXISTING BLOCKER retained: stamp="+stamp.Name+" point="+stamp.Position+" collider="+hierarchy+" bounds="+hit.bounds+"; land-support test is not obstacle clearance.");
    }
   }
   Check(clear>0,"2750 cosmetic shoulders and drains retain actual clear samples; clear="+clear+" pre-existing blocked="+existingBlocked);
  }finally{
   while(paint.Count>paintCount)paint.RemoveAt(paint.Count-1);while(accents.Count>accentCount)accents.RemoveAt(accents.Count-1);
   map.RoadDetailStamps2750.Clear();map.RoadDetailStamps2750.AddRange(stamps);
  }
  Check(paint.Count==paintCount&&accents.Count==accentCount&&map.StableGround137==texture&&map.GroundPaintFingerprint2721==fingerprint&&map.RoadDetailStamps2750.Count==stamps.Count,"2750 producer replay restores paint accent and stamp registries without rebake or texture changes");
 }
 IEnumerator RunGroundRoads2750(){
  var input=G.Controls;bool enabled=G.enabled;string dir=Path.GetDirectoryName(G.SavePath);string[] formal={"stash-v40.json","stash-v41.json","stash-v42.json"};var bytes=Array.ConvertAll(formal,n=>File.Exists(Path.Combine(dir,n))?File.ReadAllBytes(Path.Combine(dir,n)):null);
  try{Check(Application.version=="2.7.50"&&G.VerifyMode&&G.SavePath.EndsWith("verification-stash-2.7.50.json"),"2750 isolated changed-ground road verification");FreshCamp();G.Controls=new TouchInput();G.HarborWalkthrough=true;yield return G.BeginTrainingLoad2721();G.Paused=true;G.enabled=false;var map=G.Map;GroundRoadDefinitions2750(map);GroundCache2750(map);GroundMaterials2750(map);yield return GroundOfficeWalk2750(map);
   yield return GroundRoadPhoto2750(RaidMap.PortPoint(104,90),"2750-normal-light-entire-port-top.png",94,true);yield return GroundRoadPhoto2750(RaidMap.PortPoint(81.5f,151),"2750-normal-light-two-office-approaches.png",22);yield return GroundRoadPhoto2750(RaidMap.PortPoint(28,92),"2750-normal-light-repair-materials.png",18);yield return GroundRoadPhoto2750(RaidMap.PortPoint(25,28),"2750-normal-light-yard-soil-gravel.png",18);yield return GroundRoadPhoto2750(RaidMap.PortPoint(131,146),"2750-normal-light-loading-road-markings.png",17);
   FreightRoomPlace2717(RaidMap.OfficePreviewSpawn2726,"2750 safe gameplay view on actual right-tower road");G.Player.Aim=Vector3.forward;G.Rig.Target=G.Player.transform;G.Rig.ResetView(45);G.Rig.Tick(0);G.Vision.Bind();G.Vision.Refresh(G.Player.transform.position,G.Player.Aim);Shader.SetGlobalFloat("_HarborFogEnabled",1);G.Player.Draw(G.View,0);yield return null;var live=HvRender277(G.View,"2750-actual-gameplay-office-road-fog.png",1600,900);try{CargoImageCheck2713(live,"2750 actual unchanged gameplay camera and flashlight fog");}finally{Destroy(live);}Check(Shader.GetGlobalFloat("_HarborFogEnabled")==1,"2750 actual gameplay screenshot retains real visibility fog");
   for(int i=0;i<formal.Length;i++)Check(MainBytes273(bytes[i],File.Exists(Path.Combine(dir,formal[i]))?File.ReadAllBytes(Path.Combine(dir,formal[i])):null),"2750 formal save unchanged "+formal[i]);
  }finally{G.Controls=input;G.ClearShots();G.Paused=true;G.enabled=enabled;}
 }
}}
