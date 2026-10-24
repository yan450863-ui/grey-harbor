using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace GreyHarbor {
 public sealed partial class RaidVerification {
  readonly List<string> cargoRows2713=new List<string>{"type,name,x,y,z,sizeX,sizeY,sizeZ,detail"};
  bool CargoNear2713(float a,float b,float tolerance=.002f)=>Mathf.Abs(a-b)<=tolerance;
  string CargoNumber2713(float f)=>f.ToString("0.00000",System.Globalization.CultureInfo.InvariantCulture);
  string CargoCsvPath2713()=>Path.Combine(folder,Array.IndexOf(Environment.GetCommandLineArgs(),"--capture-cargo-maze-2713")>=0?"2713-cargo-capture-photometry.csv":"2713-cargo-actual-geometry-and-routes.csv");
  void CargoRow2713(string type,string name,Vector3 p,Vector3 size,string detail){cargoRows2713.Add(type+","+Csv230(name)+","+CargoNumber2713(p.x)+","+CargoNumber2713(p.y)+","+CargoNumber2713(p.z)+","+CargoNumber2713(size.x)+","+CargoNumber2713(size.y)+","+CargoNumber2713(size.z)+","+Csv230(detail));File.WriteAllLines(CargoCsvPath2713(),cargoRows2713);}
  bool CargoClear2713(Vector3 p)=>Clear140(p)&&G.Map.HarborSurfaceClear(p);
  void CargoWalk2713(Vector3 p,string label){bool walked=WalkMain273(p);CargoRow2713("walk",label,G.Player.transform.position,Vector3.zero,walked?"PASS .34 swept Actor.Move":"FAIL "+mainWalkFailure273);Check(walked&&CargoClear2713(p),"2713 Actual .34m capsule player traversal "+label+" goal="+p+" reason="+mainWalkFailure273);}
  void CargoGeometry2713(){
   var map=G.Map;var area=RaidMap.CargoBounds2712;Check(CargoNear2713(area.width,74)&&CargoNear2713(area.height,69)&&CargoNear2713(area.xMin,-23.5f)&&CargoNear2713(area.yMin,-12.5f),"2713 Central cargo footprint retains actual74x69 metres and preserved harbor interface");
   Check(map.CargoBoxes2712.Count>=24,"2713 Complex whitebox contains substantial real container groups count="+map.CargoBoxes2712.Count);Check(map.CargoProps2712.Count==0&&map.CargoReservations2712.Count==0&&map.CargoSearchReserved2712.Count==0&&map.CargoMonsterReserved2712.Count==0&&map.CargoGaps2712.Count==0,"2713 Container-only plan has no clutter, reservations or old fixed-screen gaps");
   var colliders=new List<Collider>();int shortBoxes=0,longBoxes=0,horizontal=0,vertical=0,oblique=0;
   foreach(var box in map.CargoBoxes2712){
    var c=box.View.GetComponent<BoxCollider>();Check(c!=null&&c.enabled&&!c.isTrigger,"2713 Container has real enabled BoxCollider "+box.View.name);var size=Vector3.Scale(c.size,box.View.transform.lossyScale);size=new Vector3(Mathf.Abs(size.x),Mathf.Abs(size.y),Mathf.Abs(size.z));
    bool shortBox=CargoNear2713(size.x,6.2f),longBox=CargoNear2713(size.x,12.2f);Check((shortBox||longBox)&&CargoNear2713(size.y,2.6f)&&CargoNear2713(size.z,2.6f),"2713 Real cargo collider uses6.2/12.2x2.6x2.6 dimensions "+box.View.name+" "+size);if(shortBox)shortBoxes++;if(longBox)longBoxes++;
    float yaw=Mathf.Repeat(box.View.transform.eulerAngles.y,180);if(Mathf.Min(yaw,180-yaw)<.1f)horizontal++;else if(Mathf.Abs(yaw-90)<.1f)vertical++;else oblique++;
    var bounds=c.bounds;Check(bounds.min.x>=area.xMin-.002f&&bounds.max.x<=area.xMax+.002f&&bounds.min.z>=area.yMin-.002f&&bounds.max.z<=area.yMax+.002f,"2713 Actual rotated container remains inside cargo boundary "+box.View.name);
    Check(!box.Stacked&&Mathf.Abs(bounds.min.y)<.003f,"2713 Container-only maze uses single ground-level boxes "+box.View.name);colliders.Add(c);CargoRow2713("container",box.View.name,box.View.transform.position,size,"actual yaw="+box.View.transform.eulerAngles.y);
   }
   Check(shortBoxes>0&&longBoxes>0&&horizontal>0&&vertical>0&&oblique>0,"2713 Actual maze has short/long horizontal/vertical/oblique container groups horizontal="+horizontal+" vertical="+vertical+" oblique="+oblique);
   int overlaps=0;for(int i=0;i<colliders.Count;i++)for(int j=i+1;j<colliders.Count;j++){
    var a=colliders[i];var b=colliders[j];if(!a.bounds.Intersects(b.bounds))continue;Vector3 direction;float depth;if(Physics.ComputePenetration(a,a.transform.position,a.transform.rotation,b,b.transform.position,b.transform.rotation,out direction,out depth)&&depth>.005f){if(overlaps++<20)Debug.Log("2713 ILLEGAL CONTAINER OVERLAP "+a.name+" / "+b.name+" depth="+depth+" a="+a.bounds+" b="+b.bounds);}
   }Check(overlaps==0,"2713 Actual differently oriented container colliders do not intersect overlaps="+overlaps);
   int unwanted=0;foreach(var t in map.Root.GetComponentsInChildren<Transform>(true))if(t.name.StartsWith("CargoReserved_")||t.name.StartsWith("ReservedRing_")||t.name.StartsWith("CargoWhitebox_"))unwanted++;
   Check(unwanted==0,"2713 Actual scene contains no old color-circle markers or clutter objects count="+unwanted);
   var known=new HashSet<Collider>(colliders);int foreign=0;foreach(var c in map.Root.GetComponentsInChildren<Collider>()){if(!c.enabled||c.isTrigger||known.Contains(c)||(SceneObstacle.MovementMask&(1<<c.gameObject.layer))==0)continue;var b=c.bounds;if(b.min.x>=area.xMin&&b.max.x<=area.xMax&&b.min.z>=area.yMin&&b.max.z<=area.yMax&&b.max.y>.3f){if(foreign++<10)Debug.Log("2713 NON-CONTAINER COLLIDER "+c.name+" bounds="+b);}}
   Check(foreign==0,"2713 Maze interior collision geometry consists only of actual containers foreign="+foreign);
   Check(!map.Loot.Exists(l=>area.Contains(new Vector2(l.Position.x,l.Position.z))||(l.Prop&&area.Contains(new Vector2(l.Prop.transform.position.x,l.Prop.transform.position.z))))&&!G.Enemies.Exists(e=>area.Contains(new Vector2(e.Home.x,e.Home.z))||area.Contains(new Vector2(e.transform.position.x,e.transform.position.z))),"2713 Container-only whitebox adds no real loot or NPC population");
  }
  int CargoNode2713(List<Vector3> nodes,Vector3 p){for(int i=0;i<nodes.Count;i++)if(Vector3.Distance(nodes[i],p)<.005f)return i;nodes.Add(p);return nodes.Count-1;}
  bool CargoOnSegment2713(Vector3 p,Vector3 a,Vector3 b){var d=b-a;float t=Vector3.Dot(p-a,d)/Mathf.Max(d.sqrMagnitude,.00001f);return t>=-.00001f&&t<=1.00001f&&Vector3.Distance(a+d*Mathf.Clamp01(t),p)<.005f;}
  void CargoGraph2713(){
   var nodes=new List<Vector3>();var segments=new List<Vector3[]>();foreach(var lane in G.Map.CargoLanes2712){Check(lane.Points!=null&&lane.Points.Length>=2&&lane.Width>=2.19f,"2713 Actual main/branch lane has usable width "+lane.Name);for(int i=1;i<lane.Points.Length;i++){segments.Add(new[]{lane.Points[i-1],lane.Points[i]});CargoNode2713(nodes,lane.Points[i-1]);CargoNode2713(nodes,lane.Points[i]);}}
   // Derive junctions from actual route intersections; do not assume named branches form cycles.
   for(int i=0;i<segments.Count;i++)for(int j=i+1;j<segments.Count;j++){var a=segments[i][0];var d=segments[i][1]-a;var b=segments[j][0];var e=segments[j][1]-b;float den=d.x*e.z-d.z*e.x;if(Mathf.Abs(den)<.00001f)continue;var q=b-a;float t=(q.x*e.z-q.z*e.x)/den,u=(q.x*d.z-q.z*d.x)/den;if(t>=0&&t<=1&&u>=0&&u<=1)CargoNode2713(nodes,a+d*t);}
   var edges=new HashSet<string>();var neighbors=new List<HashSet<int>>();for(int i=0;i<nodes.Count;i++)neighbors.Add(new HashSet<int>());
   foreach(var segment in segments){var ids=new List<int>();for(int i=0;i<nodes.Count;i++)if(CargoOnSegment2713(nodes[i],segment[0],segment[1]))ids.Add(i);ids.Sort((a,b)=>Vector3.Distance(nodes[a],segment[0]).CompareTo(Vector3.Distance(nodes[b],segment[0])));for(int i=1;i<ids.Count;i++){int a=Mathf.Min(ids[i-1],ids[i]),b=Mathf.Max(ids[i-1],ids[i]);if(a==b)continue;edges.Add(a+":"+b);neighbors[a].Add(b);neighbors[b].Add(a);}}
   int components=0,branches=0;var visited=new HashSet<int>();foreach(var ns in neighbors)if(ns.Count>=3)branches++;for(int i=0;i<nodes.Count;i++)if(visited.Add(i)){components++;var queue=new Queue<int>();queue.Enqueue(i);while(queue.Count>0)foreach(int n in neighbors[queue.Dequeue()])if(visited.Add(n))queue.Enqueue(n);}
   int cycles=edges.Count-nodes.Count+components;CargoRow2713("topology","derived lane graph",Vector3.zero,Vector3.zero,"nodes="+nodes.Count+" edges="+edges.Count+" components="+components+" branches="+branches+" cycles="+cycles);Check(components==1&&branches>=4&&cycles>=3,"2713 Actual route geometry has connected main path, branches and at least three independent loops nodes="+nodes.Count+" components="+components+" branches="+branches+" cycles="+cycles);
  }
  void CargoLaneWidth2713(Vector3 start,Vector3 end,float width,string label){
   float radius=width*.5f-.05f;int steps=Mathf.Max(1,Mathf.CeilToInt(Vector3.Distance(start,end)/.5f));string failure=null;Vector3 blocked=Vector3.zero;
   for(int i=0;i<=steps;i++){var point=Vector3.Lerp(start,end,i/(float)steps);if(!G.Map.HarborSurfaceClear(point,radius)){failure="full-width ground missing";blocked=point;break;}if(Physics.CheckCapsule(point+Vector3.up*.35f,point+Vector3.up,radius,SceneObstacle.MovementMask,QueryTriggerInteraction.Ignore)){failure="actual collider enters nominal-width corridor";blocked=point;break;}}
   if(failure!=null){Debug.Log("2713 LANE WIDTH FAILURE "+label+" width="+width+" sampled="+blocked+" reason="+failure);foreach(var c in Physics.OverlapCapsule(blocked+Vector3.up*.35f,blocked+Vector3.up,radius,SceneObstacle.MovementMask,QueryTriggerInteraction.Ignore))Debug.Log("2713 LANE WIDTH COLLIDER "+c.name+" bounds="+c.bounds);}
   CargoRow2713("actual-width",label,failure==null?(start+end)*.5f:blocked,new Vector3(width,0,0),"sample every<=.5m radius="+radius+" "+(failure??"PASS"));Check(failure==null,"2713 Actual lane supports declared width with .05m face margin "+label+" width="+width+" blocked="+blocked+" reason="+failure);
  }
  IEnumerator CargoRoutes2713(){
   var map=G.Map;var area=RaidMap.CargoBounds2712;bool south=false,north=false,west=false,east=false;int segments=0;
   foreach(var lane in map.CargoLanes2712){CargoWalk2713(lane.Points[0],lane.Name+" entrance");for(int i=0;i<lane.Points.Length;i++){var p=lane.Points[i];south|=p.z<area.yMin;north|=p.z>area.yMax;west|=p.x<area.xMin;east|=p.x>area.xMax;Check(CargoClear2713(p),"2713 Lane waypoint has actual ground and .34 capsule "+lane.Name+" "+p);if(i>0){CargoLaneWidth2713(lane.Points[i-1],p,lane.Width,lane.Name+"/"+i);Check(G.Navigation.ClearSegment(lane.Points[i-1],p),"2713 Reserved lane segment has actual .34m swept clearance "+lane.Name+" "+i);CargoWalk2713(p,lane.Name+" segment"+i);segments++;}CargoRow2713("lane",lane.Name+"/"+i,p,new Vector3(lane.Width,0,0),"actual walked");}yield return null;}
   Check(south&&north&&west&&east&&segments>8,"2713 Maze joins actual peripheral walking surfaces in all four directions");
  }
  void CargoSprint2713(){
   // Follow a real lane at 1cm steps. Sprint must remain a true Actor.Move route,
   // with the same .34 swept validation as walking; no waypoint teleporting.
   var lane=G.Map.CargoLanes2712[0];CargoWalk2713(lane.Points[0],"sprint lane start");G.ResetSprint2711();G.Sprint2711.Toggle();float distance=0,running=0;int moves=0;
   G.Paused=false;try{for(int i=1;i<lane.Points.Length;i++){var target=lane.Points[i];float length=Vector3.Distance(G.Player.transform.position,target);int budget=Mathf.CeilToInt(length/.01f)+256;int used=0;string failure=null;while(Vector3.Distance(G.Player.transform.position,target)>.0001f){if(++used>budget){failure="step budget "+budget;break;}var before=G.Player.transform.position;var delta=Vector3.ClampMagnitude(target-before,.01f);float speed=G.Sprint2711.CanRun?5.1f:3.4f;G.MovePlayer2711(delta.normalized*3.4f,delta.magnitude/speed);Physics.SyncTransforms();if(!CargoClear2713(G.Player.transform.position)){failure="actual .34 capsule blocked at "+G.Player.transform.position;break;}float moved=Vector3.Distance(before,G.Player.transform.position);if(moved<=.00001f){failure="actual Actor.Move stalled at "+before;break;}distance+=moved;running+=G.Sprint2711.LastRunningSeconds;moves++;}Check(failure==null,"2713 Actual sprint route segment maintains .34 capsule, exact waypoint and bounded movement segment="+i+" reason="+failure);}}finally{G.Paused=true;}
   Check(distance>10&&running>1,"2713 Current real sprint moves through maze without corner clipping");CargoRow2713("sprint",lane.Name,G.Player.transform.position,Vector3.zero,"distance="+distance+" runningSeconds="+running+" steps="+moves);
  }
  void CargoSight2713(){
   var south=RaidMap.CargoPreviewSpawn2712;var north=south;float maxZ=float.MinValue;foreach(var lane in G.Map.CargoLanes2712)foreach(var point in lane.Points)if(point.z>maxZ){maxZ=point.z;north=point;}Check(!G.ClearSight(south,north),"2713 Actual southern/northern maze entrances are not joined by a long unobstructed sight line");
   int blocks=0;foreach(var box in G.Map.CargoBoxes2712){if(box.Stacked)continue;var c=box.View.GetComponent<Collider>();var center=c.bounds.center;center.y=0;var normal=box.View.transform.forward;var half=c.bounds.size.magnitude+1;var a=center-normal*half;var b=center+normal*half;Check(!G.ClearSight(a,b),"2713 Real cargo wall blocks combat-height sight "+box.View.name);blocks++;}
   var area=RaidMap.CargoBounds2712;float maximum=0;int samples=0;var directions=new[]{Vector3.right,Vector3.left,Vector3.forward,Vector3.back};
   for(float z=area.yMin+1;z<area.yMax-1;z+=2)for(float x=area.xMin+1;x<area.xMax-1;x+=2){var origin=new Vector3(x,0,z);if(!CargoClear2713(origin))continue;foreach(var d in directions){float edge=d.x>0?area.xMax-x:d.x<0?x-area.xMin:d.z>0?area.yMax-z:z-area.yMin;RaycastHit hit;float length=Physics.Raycast(origin+Vector3.up*RaidGame.CombatHeight,d,out hit,edge,SceneObstacle.SightMask,QueryTriggerInteraction.Ignore)?hit.distance:edge;maximum=Mathf.Max(maximum,length);samples++;if(length>35)CargoRow2713("long-sight","axis ray",origin,d,"length="+length);}}
   CargoRow2713("sight","independent 2m grid axis rays",Vector3.zero,Vector3.zero,"samples="+samples+" max="+maximum+" blockers="+blocks);Check(blocks>0&&samples>100,"2713 Physical blockers independently sampled across real maze");
  }
  void CargoImageCheck2713(Texture2D image,string view){
   var pixels=image.GetPixels32();var histogram=new int[256];int lit=0,white=0;double sum=0;
   foreach(var p in pixels){float luminance=(.2126f*p.r+.7152f*p.g+.0722f*p.b)/255f;sum+=luminance;histogram[Mathf.Clamp(Mathf.RoundToInt(luminance*255),0,255)]++;if(luminance>.04f)lit++;if(p.r>=248&&p.g>=248&&p.b>=248)white++;}
   int count=0,p10=0,p90=0;bool first=false;for(int i=0;i<256;i++){count+=histogram[i];if(!first&&count>=pixels.Length*.10f){p10=i;first=true;}if(count>=pixels.Length*.90f){p90=i;break;}}
   float mean=(float)(sum/pixels.Length),whiteFraction=white/(float)pixels.Length,contrast=(p90-p10)/255f;string detail="mean="+mean+" whiteFraction="+whiteFraction+" p10="+p10+" p90="+p90+" contrast="+contrast;
   CargoRow2713("image",view,Vector3.zero,Vector3.zero,detail);Check(lit>pixels.Length*.25f&&mean<.90f&&whiteFraction<.08f&&contrast>.06f,"2713 Actual scene image contains usable geometry contrast without white clipping "+view+" "+detail);
  }
  IEnumerator CargoPictures2713(){
   var area=RaidMap.CargoBounds2712;var center=new Vector3(area.center.x,0,area.center.y);var go=new GameObject("VerificationCargoCamera2713");var cam=go.AddComponent<Camera>();cam.CopyFrom(G.View);cam.enabled=false;cam.orthographic=true;cam.allowHDR=false;cam.nearClipPlane=.1f;cam.farClipPlane=250;
   float fog=Shader.GetGlobalFloat("_HarborFogEnabled");var ambient=RenderSettings.ambientLight;var sunObject=GameObject.Find("LateAfternoon");var sun=sunObject?sunObject.GetComponent<Light>():null;float power=sun?sun.intensity:0;float shadowDistance=QualitySettings.shadowDistance;
   // HarborWorld's Lambert plus emission uses the game lighting with fog dimming.
   // Whole-layout fog-off captures need the same temporary review lighting used
   // by the older yard/exterior capture helpers, otherwise RGB clips to white.
   // Only capture lighting changes; actual production materials stay untouched.
   try{yield return null;Shader.SetGlobalFloat("_HarborFogEnabled",0);RenderSettings.ambientLight=new Color(.33f,.36f,.35f);if(sun)sun.intensity=.64f;QualitySettings.shadowDistance=170;
    cam.transform.position=center+Vector3.up*100;cam.transform.rotation=Quaternion.Euler(90,0,0);cam.orthographicSize=area.height/2+4;var top=HvRender277(cam,"2713-cargo-overhead-scene.png",1800,1800);try{CargoImageCheck2713(top,"overhead");}finally{Destroy(top);}
    cam.transform.rotation=Quaternion.Euler(52,35,0);cam.transform.position=center-cam.transform.forward*110;cam.orthographicSize=42;var oblique=HvRender277(cam,"2713-cargo-oblique-scene.png",1800,1200);try{CargoImageCheck2713(oblique,"oblique");}finally{Destroy(oblique);}
    File.WriteAllText(Path.Combine(folder,"2713-images-note.txt"),"Two actual Camera.Render scene views of the outdoor whitebox. Fog disabled for whole-layout geometry review only. Capture-only lighting: ambient(.33,.36,.35), LateAfternoon intensity .64, shadow distance170m; restored afterward. Production materials and geometry untouched. Container-only scene has no reservation markers or props. OnGUI/HUD numbers are not captured; no synthetic HUD. Image mean/white-clipping/10th-to-90th percentile contrast checked and recorded in actual CSV.\n");
   }finally{Shader.SetGlobalFloat("_HarborFogEnabled",fog);RenderSettings.ambientLight=ambient;if(sun)sun.intensity=power;QualitySettings.shadowDistance=shadowDistance;Destroy(go);}
  }
  IEnumerator VerifyCargoMaze2713(){
   Check(Application.version=="2.7.13"&&Path.GetFileName(G.SavePath)=="verification-stash-2.7.13.json","2713 Dedicated build and isolated verification inventory");string directory=Path.GetDirectoryName(G.SavePath);string[] formal={"stash-v40.json","stash-v41.json","stash-v42.json"};byte[][] before=Array.ConvertAll(formal,n=>File.Exists(Path.Combine(directory,n))?File.ReadAllBytes(Path.Combine(directory,n)):null);var controls=G.Controls;var random=UnityEngine.Random.state;bool harbor=G.HarborWalkthrough;
   FreshCamp();string camp=JsonUtility.ToJson(G.Save);G.HarborWalkthrough=true;G.HardMode=false;Check(G.BeginTraining(),"2713 Build actual complete harbor in isolated training session");G.Paused=true;yield return null;
   try{G.Controls=new TouchInput();Check(CargoPreview2712.Apply(G),"2713 Actual cargo preview positions player at clear southern approach");Physics.SyncTransforms();Check(G.Map.HarborWhitebox&&CargoClear2713(G.Player.transform.position),"2713 Preserved southern cargo approach has real walkable ground");if(Array.IndexOf(Environment.GetCommandLineArgs(),"--capture-cargo-maze-2713")<0){CargoGeometry2713();CargoGraph2713();yield return CargoRoutes2713();CargoSprint2713();CargoSight2713();}else{results.Add("INFO 2713 Capture-only: geometry/traversal suites skipped; isolated preview, true camera rendering and save protection retained.");}yield return CargoPictures2713();G.Finish(false);yield return null;Check(JsonUtility.ToJson(G.Save)==camp,"2713 Cargo-only preview restores inventory without economy changes");for(int i=0;i<formal.Length;i++)Check(MainBytes273(before[i],File.Exists(Path.Combine(directory,formal[i]))?File.ReadAllBytes(Path.Combine(directory,formal[i])):null),"2713 Formal save remains byte-identical "+formal[i]);}
   finally{G.Controls=controls;G.HarborWalkthrough=harbor;UnityEngine.Random.state=random;G.Paused=true;File.WriteAllLines(CargoCsvPath2713(),cargoRows2713);}
  }
 }
}
