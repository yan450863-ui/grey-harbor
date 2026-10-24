using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace GreyHarbor {
 public sealed partial class RaidVerification {
  readonly List<string> cargoRows2712=new List<string>{"type,name,x,y,z,sizeX,sizeY,sizeZ,detail"};
  bool CargoNear2712(float a,float b,float tolerance=.002f)=>Mathf.Abs(a-b)<=tolerance;
  string CargoNumber2712(float f)=>f.ToString("0.00000",System.Globalization.CultureInfo.InvariantCulture);
  string CargoCsvPath2712()=>Path.Combine(folder,Array.IndexOf(Environment.GetCommandLineArgs(),"--capture-cargo-maze-2712")>=0?"2712-cargo-capture-photometry.csv":"2712-cargo-actual-geometry-and-routes.csv");
  void CargoRow2712(string type,string name,Vector3 p,Vector3 size,string detail){cargoRows2712.Add(type+","+Csv230(name)+","+CargoNumber2712(p.x)+","+CargoNumber2712(p.y)+","+CargoNumber2712(p.z)+","+CargoNumber2712(size.x)+","+CargoNumber2712(size.y)+","+CargoNumber2712(size.z)+","+Csv230(detail));File.WriteAllLines(CargoCsvPath2712(),cargoRows2712);}
  bool CargoClear2712(Vector3 p)=>Clear140(p)&&G.Map.HarborSurfaceClear(p);
  void CargoWalk2712(Vector3 p,string label){bool walked=WalkMain273(p);CargoRow2712("walk",label,G.Player.transform.position,Vector3.zero,walked?"PASS .34 swept Actor.Move":"FAIL "+mainWalkFailure273);Check(walked&&CargoClear2712(p),"2712 Actual .34m capsule player traversal "+label+" goal="+p+" reason="+mainWalkFailure273);}
  void CargoGeometry2712(){
   var map=G.Map;var area=RaidMap.CargoBounds2712;Check(CargoNear2712(area.width,74)&&CargoNear2712(area.height,69)&&CargoNear2712(area.xMin,-23.5f)&&CargoNear2712(area.yMin,-12.5f),"2712 Central cargo footprint is actual74x69 metres at preserved harbor interface");
   Check(map.CargoBoxes2712.Count>0&&map.CargoProps2712.Count>0,"2712 Actual whitebox contains containers and physical clutter");var colliders=new List<Collider>();var owners=new Dictionary<Collider,GameObject>();int shortBoxes=0,longBoxes=0,stacked=0,tilted=0;
   foreach(var box in map.CargoBoxes2712){
    var c=box.View.GetComponent<BoxCollider>();Check(c!=null&&c.enabled&&!c.isTrigger,"2712 Container has real enabled BoxCollider "+box.View.name);var size=Vector3.Scale(c.size,box.View.transform.lossyScale);size=new Vector3(Mathf.Abs(size.x),Mathf.Abs(size.y),Mathf.Abs(size.z));
    bool shortBox=CargoNear2712(size.x,6.2f),longBox=CargoNear2712(size.x,12.2f);Check((shortBox||longBox)&&CargoNear2712(size.y,2.6f)&&CargoNear2712(size.z,2.6f),"2712 Real cargo collider uses6.2/12.2x2.6x2.6 dimensions "+box.View.name+" "+size);if(shortBox)shortBoxes++;if(longBox)longBoxes++;if(box.Stacked)stacked++;if(Mathf.Abs(Mathf.DeltaAngle(box.View.transform.eulerAngles.y,0))>.1f&&Mathf.Abs(Mathf.DeltaAngle(box.View.transform.eulerAngles.y,90))>.1f)tilted++;
    var bounds=c.bounds;Check(bounds.min.x>=area.xMin-.002f&&bounds.max.x<=area.xMax+.002f&&bounds.min.z>=area.yMin-.002f&&bounds.max.z<=area.yMax+.002f,"2712 Real rotated container remains inside cargo boundary "+box.View.name);
    Check(box.Stacked?bounds.min.y>=2.59f:Mathf.Abs(bounds.min.y)<.003f,"2712 Cargo stacks remain overhead and lower floor has one walking elevation "+box.View.name);colliders.Add(c);owners[c]=box.View;CargoRow2712(box.Stacked?"stack":"container",box.View.name,box.View.transform.position,size,"yaw="+box.View.transform.eulerAngles.y);
   }
   Check(shortBoxes>0&&longBoxes>0&&stacked>0&&tilted>0,"2712 Actual maze retains short/long/stacked/turned container variety");
   foreach(var prop in map.CargoProps2712){var actual=prop.View.GetComponentsInChildren<Collider>();Check(actual.Length>0,"2712 Clutter is physical geometry "+prop.View.name);foreach(var c in actual)if(c.enabled&&!c.isTrigger){colliders.Add(c);owners[c]=prop.View;var b=c.bounds;Check(b.min.x>=area.xMin-.002f&&b.max.x<=area.xMax+.002f&&b.min.z>=area.yMin-.002f&&b.max.z<=area.yMax+.002f,"2712 Physical clutter remains within maze boundary "+prop.View.name);}CargoRow2712("clutter",prop.View.name,prop.View.transform.position,prop.Size,prop.Type+" "+prop.Kind);}
   int overlaps=0;for(int i=0;i<colliders.Count;i++)for(int j=i+1;j<colliders.Count;j++){
    var a=colliders[i];var b=colliders[j];if(owners[a]==owners[b]||!a.bounds.Intersects(b.bounds))continue;if(a.transform.IsChildOf(b.transform)||b.transform.IsChildOf(a.transform))continue;
    Vector3 direction;float depth;if(Physics.ComputePenetration(a,a.transform.position,a.transform.rotation,b,b.transform.position,b.transform.rotation,out direction,out depth)&&depth>.005f){if(overlaps++<20)Debug.Log("2712 ILLEGAL OVERLAP "+a.name+" / "+b.name+" depth="+depth+" a="+a.bounds+" b="+b.bounds);}
   }Check(overlaps==0,"2712 Real containers/clutter do not illegally intersect; overhead stack contact allowed overlaps="+overlaps);
   Check(!map.Loot.Exists(l=>area.Contains(new Vector2(l.Position.x,l.Position.z))||(l.Prop&&area.Contains(new Vector2(l.Prop.transform.position.x,l.Prop.transform.position.z))))&&!G.Enemies.Exists(e=>area.Contains(new Vector2(e.Home.x,e.Home.z))),"2712 Cargo reservation whitebox adds no real loot or NPC population");
  }
  void CargoGaps2712(){
   // Inspect the five physical barrier cross-sections, not the lane.Width metadata.
   // Nearest left/right collider faces give the actual opening width. Unbounded
   // peripheral strips are excluded because they do not have two barrier faces.
   var area=RaidMap.CargoBounds2712;
   foreach(float portZ in new[]{68f,82f,96f,110f,124f}){float z=portZ-73;var gaps=new List<Vector2>();
    for(float x=area.xMin+.4f;x<area.xMax-.4f;x+=.10f){var p=new Vector3(x,0,z);if(!CargoClear2712(p))continue;RaycastHit left,right;if(!Physics.Raycast(p+Vector3.up*.65f,Vector3.left,out left,area.width,SceneObstacle.MovementMask,QueryTriggerInteraction.Ignore)||!Physics.Raycast(p+Vector3.up*.65f,Vector3.right,out right,area.width,SceneObstacle.MovementMask,QueryTriggerInteraction.Ignore))continue;
     if(left.point.x<area.xMin-.002f||right.point.x>area.xMax+.002f)continue;float width=left.distance+right.distance,center=x+(right.distance-left.distance)*.5f;if(width<.68f||width>20)continue;bool duplicate=false;foreach(var gap in gaps)if(Mathf.Abs(gap.x-center)<.03f){duplicate=true;break;}if(!duplicate){gaps.Add(new Vector2(center,width));CargoRow2712("physical-gap","barrier z="+z,new Vector3(center,0,z),new Vector3(width,0,0),"actual collider faces "+left.collider.name+" / "+right.collider.name);}}
    bool main=false,secondary=false;int usable=0;foreach(var gap in gaps){if(gap.y>=3.99f)main=true;if(gap.y>=2.39f)usable++;}secondary=usable>=2;Check(main&&secondary,"2712 Every physical barrier has actual4m main and separate2.4m secondary opening z="+z+" measured="+string.Join(";",gaps.ConvertAll(g=>g.x+":"+g.y).ToArray()));
   }
  }
  int CargoNode2712(List<Vector3> nodes,Vector3 p){for(int i=0;i<nodes.Count;i++)if(Vector3.Distance(nodes[i],p)<.005f)return i;nodes.Add(p);return nodes.Count-1;}
  bool CargoOnSegment2712(Vector3 p,Vector3 a,Vector3 b){var d=b-a;float t=Vector3.Dot(p-a,d)/Mathf.Max(d.sqrMagnitude,.00001f);return t>=-.00001f&&t<=1.00001f&&Vector3.Distance(a+d*Mathf.Clamp01(t),p)<.005f;}
  void CargoGraph2712(){
   var nodes=new List<Vector3>();var segments=new List<Vector3[]>();foreach(var lane in G.Map.CargoLanes2712){Check(lane.Points!=null&&lane.Points.Length>=2&&lane.Width>=2.39f,"2712 Actual main/branch lane has usable width "+lane.Name);for(int i=1;i<lane.Points.Length;i++){segments.Add(new[]{lane.Points[i-1],lane.Points[i]});CargoNode2712(nodes,lane.Points[i-1]);CargoNode2712(nodes,lane.Points[i]);}}
   // Derive junctions from actual route intersections; do not assume named branches form cycles.
   for(int i=0;i<segments.Count;i++)for(int j=i+1;j<segments.Count;j++){var a=segments[i][0];var d=segments[i][1]-a;var b=segments[j][0];var e=segments[j][1]-b;float den=d.x*e.z-d.z*e.x;if(Mathf.Abs(den)<.00001f)continue;var q=b-a;float t=(q.x*e.z-q.z*e.x)/den,u=(q.x*d.z-q.z*d.x)/den;if(t>=0&&t<=1&&u>=0&&u<=1)CargoNode2712(nodes,a+d*t);}
   var edges=new HashSet<string>();var neighbors=new List<HashSet<int>>();for(int i=0;i<nodes.Count;i++)neighbors.Add(new HashSet<int>());
   foreach(var segment in segments){var ids=new List<int>();for(int i=0;i<nodes.Count;i++)if(CargoOnSegment2712(nodes[i],segment[0],segment[1]))ids.Add(i);ids.Sort((a,b)=>Vector3.Distance(nodes[a],segment[0]).CompareTo(Vector3.Distance(nodes[b],segment[0])));for(int i=1;i<ids.Count;i++){int a=Mathf.Min(ids[i-1],ids[i]),b=Mathf.Max(ids[i-1],ids[i]);if(a==b)continue;edges.Add(a+":"+b);neighbors[a].Add(b);neighbors[b].Add(a);}}
   int components=0,branches=0;var visited=new HashSet<int>();foreach(var ns in neighbors)if(ns.Count>=3)branches++;for(int i=0;i<nodes.Count;i++)if(visited.Add(i)){components++;var queue=new Queue<int>();queue.Enqueue(i);while(queue.Count>0)foreach(int n in neighbors[queue.Dequeue()])if(visited.Add(n))queue.Enqueue(n);}
   int cycles=edges.Count-nodes.Count+components;CargoRow2712("topology","derived lane graph",Vector3.zero,Vector3.zero,"nodes="+nodes.Count+" edges="+edges.Count+" components="+components+" branches="+branches+" cycles="+cycles);Check(components==1&&branches>=2&&cycles>=1,"2712 Actual route geometry has connected main path, branches and alternative loop nodes="+nodes.Count+" components="+components+" branches="+branches+" cycles="+cycles);
  }
  IEnumerator CargoRoutes2712(){
   var map=G.Map;var area=RaidMap.CargoBounds2712;bool south=false,north=false,west=false,east=false;int segments=0;
   foreach(var lane in map.CargoLanes2712){CargoWalk2712(lane.Points[0],lane.Name+" entrance");for(int i=0;i<lane.Points.Length;i++){var p=lane.Points[i];south|=p.z<area.yMin;north|=p.z>area.yMax;west|=p.x<area.xMin;east|=p.x>area.xMax;Check(CargoClear2712(p),"2712 Lane waypoint has actual ground and .34 capsule "+lane.Name+" "+p);if(i>0){Check(G.Navigation.ClearSegment(lane.Points[i-1],p),"2712 Reserved lane segment has actual .34m swept clearance "+lane.Name+" "+i);CargoWalk2712(p,lane.Name+" segment"+i);segments++;}CargoRow2712("lane",lane.Name+"/"+i,p,new Vector3(lane.Width,0,0),"actual walked");}yield return null;}
   Check(south&&north&&west&&east&&segments>8,"2712 Maze joins actual peripheral walking surfaces in all four directions");
   Check(map.CargoSearchReserved2712.Count==12&&map.CargoMonsterReserved2712.Count==8&&map.CargoReservations2712.Count==20,"2712 Exactly12 search and8 monster design reservations");var unique=new HashSet<string>();int searches=0,monsters=0;
   foreach(var reservation in map.CargoReservations2712){Check(unique.Add(reservation.Name)&&area.Contains(new Vector2(reservation.Position.x,reservation.Position.z)),"2712 Unique reservation lies inside maze "+reservation.Name);if(reservation.Monster)monsters++;else searches++;
    CargoWalk2712(reservation.Stand,"reservation stand "+reservation.Name);Check(CargoClear2712(reservation.Position),"2712 Reservation center is physically clear "+reservation.Name);Check(reservation.ActivityArea.Contains(new Vector2(reservation.Stand.x,reservation.Stand.z)),"2712 Real reservation stand stays in its activity patch "+reservation.Name);CargoRow2712(reservation.Monster?"monster-reserved":"search-reserved",reservation.Name,reservation.Position,new Vector3(reservation.ActivityArea.width,0,reservation.ActivityArea.height),"stand="+reservation.Stand);
    if(reservation.Monster){Check(reservation.Patrol!=null&&reservation.Patrol.Length>=2,"2712 Monster reservation has actual patrol route "+reservation.Name);foreach(var target in reservation.Patrol)CargoWalk2712(target,"reserved patrol "+reservation.Name);}
    yield return null;
   }Check(searches==12&&monsters==8,"2712 Reservation records match search/monster lists");
  }
  void CargoSprint2712(){
   // Follow a real lane at 1cm steps. Sprint must remain a true Actor.Move route,
   // with the same .34 swept validation as walking; no waypoint teleporting.
   var lane=G.Map.CargoLanes2712[0];CargoWalk2712(lane.Points[0],"sprint lane start");G.ResetSprint2711();G.Sprint2711.Toggle();float distance=0,running=0;int moves=0;
   G.Paused=false;try{for(int i=1;i<lane.Points.Length;i++){var target=lane.Points[i];float length=Vector3.Distance(G.Player.transform.position,target);int budget=Mathf.CeilToInt(length/.01f)+256;int used=0;string failure=null;while(Vector3.Distance(G.Player.transform.position,target)>.0001f){if(++used>budget){failure="step budget "+budget;break;}var before=G.Player.transform.position;var delta=Vector3.ClampMagnitude(target-before,.01f);float speed=G.Sprint2711.CanRun?5.1f:3.4f;G.MovePlayer2711(delta.normalized*3.4f,delta.magnitude/speed);Physics.SyncTransforms();if(!CargoClear2712(G.Player.transform.position)){failure="actual .34 capsule blocked at "+G.Player.transform.position;break;}float moved=Vector3.Distance(before,G.Player.transform.position);if(moved<=.00001f){failure="actual Actor.Move stalled at "+before;break;}distance+=moved;running+=G.Sprint2711.LastRunningSeconds;moves++;}Check(failure==null,"2712 Actual sprint route segment maintains .34 capsule, exact waypoint and bounded movement segment="+i+" reason="+failure);}}finally{G.Paused=true;}
   Check(distance>10&&running>1,"2712 Current real sprint moves through maze without corner clipping");CargoRow2712("sprint",lane.Name,G.Player.transform.position,Vector3.zero,"distance="+distance+" runningSeconds="+running+" steps="+moves);
  }
  void CargoSight2712(){
   var south=RaidMap.CargoPreviewSpawn2712;var north=south;float maxZ=float.MinValue;foreach(var lane in G.Map.CargoLanes2712)foreach(var point in lane.Points)if(point.z>maxZ){maxZ=point.z;north=point;}Check(!G.ClearSight(south,north),"2712 Actual southern/northern maze entrances are not joined by a long unobstructed sight line");
   int blocks=0;foreach(var box in G.Map.CargoBoxes2712){if(box.Stacked)continue;var c=box.View.GetComponent<Collider>();var center=c.bounds.center;center.y=0;var normal=box.View.transform.forward;var half=c.bounds.size.magnitude+1;var a=center-normal*half;var b=center+normal*half;Check(!G.ClearSight(a,b),"2712 Real cargo wall blocks combat-height sight "+box.View.name);blocks++;}
   var area=RaidMap.CargoBounds2712;float maximum=0;int samples=0;var directions=new[]{Vector3.right,Vector3.left,Vector3.forward,Vector3.back};
   for(float z=area.yMin+1;z<area.yMax-1;z+=2)for(float x=area.xMin+1;x<area.xMax-1;x+=2){var origin=new Vector3(x,0,z);if(!CargoClear2712(origin))continue;foreach(var d in directions){float edge=d.x>0?area.xMax-x:d.x<0?x-area.xMin:d.z>0?area.yMax-z:z-area.yMin;RaycastHit hit;float length=Physics.Raycast(origin+Vector3.up*RaidGame.CombatHeight,d,out hit,edge,SceneObstacle.SightMask,QueryTriggerInteraction.Ignore)?hit.distance:edge;maximum=Mathf.Max(maximum,length);samples++;if(length>35)CargoRow2712("long-sight","axis ray",origin,d,"length="+length);}}
   CargoRow2712("sight","independent 2m grid axis rays",Vector3.zero,Vector3.zero,"samples="+samples+" max="+maximum+" blockers="+blocks);Check(blocks>0&&samples>100,"2712 Physical blockers independently sampled across real maze");
  }
  void CargoImageCheck2712(Texture2D image,string view){
   var pixels=image.GetPixels32();var histogram=new int[256];int lit=0,white=0;double sum=0;
   foreach(var p in pixels){float luminance=(.2126f*p.r+.7152f*p.g+.0722f*p.b)/255f;sum+=luminance;histogram[Mathf.Clamp(Mathf.RoundToInt(luminance*255),0,255)]++;if(luminance>.04f)lit++;if(p.r>=248&&p.g>=248&&p.b>=248)white++;}
   int count=0,p10=0,p90=0;bool first=false;for(int i=0;i<256;i++){count+=histogram[i];if(!first&&count>=pixels.Length*.10f){p10=i;first=true;}if(count>=pixels.Length*.90f){p90=i;break;}}
   float mean=(float)(sum/pixels.Length),whiteFraction=white/(float)pixels.Length,contrast=(p90-p10)/255f;string detail="mean="+mean+" whiteFraction="+whiteFraction+" p10="+p10+" p90="+p90+" contrast="+contrast;
   CargoRow2712("image",view,Vector3.zero,Vector3.zero,detail);Check(lit>pixels.Length*.25f&&mean<.90f&&whiteFraction<.08f&&contrast>.06f,"2712 Actual scene image contains usable geometry contrast without white clipping "+view+" "+detail);
  }
  IEnumerator CargoPictures2712(){
   var area=RaidMap.CargoBounds2712;var center=new Vector3(area.center.x,0,area.center.y);var go=new GameObject("VerificationCargoCamera2712");var cam=go.AddComponent<Camera>();cam.CopyFrom(G.View);cam.enabled=false;cam.orthographic=true;cam.allowHDR=false;cam.nearClipPlane=.1f;cam.farClipPlane=250;
   float fog=Shader.GetGlobalFloat("_HarborFogEnabled");var ambient=RenderSettings.ambientLight;var sunObject=GameObject.Find("LateAfternoon");var sun=sunObject?sunObject.GetComponent<Light>():null;float power=sun?sun.intensity:0;float shadowDistance=QualitySettings.shadowDistance;
   // HarborWorld's Lambert plus emission uses the game lighting with fog dimming.
   // Whole-layout fog-off captures need the same temporary review lighting used
   // by the older yard/exterior capture helpers, otherwise RGB clips to white.
   // Only capture lighting changes; actual production materials stay untouched.
   try{yield return null;Shader.SetGlobalFloat("_HarborFogEnabled",0);RenderSettings.ambientLight=new Color(.33f,.36f,.35f);if(sun)sun.intensity=.64f;QualitySettings.shadowDistance=170;
    cam.transform.position=center+Vector3.up*100;cam.transform.rotation=Quaternion.Euler(90,0,0);cam.orthographicSize=area.height/2+4;var top=HvRender277(cam,"2712-cargo-overhead-scene.png",1800,1800);try{CargoImageCheck2712(top,"overhead");}finally{Destroy(top);}
    cam.transform.rotation=Quaternion.Euler(52,35,0);cam.transform.position=center-cam.transform.forward*110;cam.orthographicSize=42;var oblique=HvRender277(cam,"2712-cargo-oblique-scene.png",1800,1200);try{CargoImageCheck2712(oblique,"oblique");}finally{Destroy(oblique);}
    File.WriteAllText(Path.Combine(folder,"2712-images-note.txt"),"Two actual Camera.Render scene views of the outdoor whitebox. Fog disabled for whole-layout geometry review only. Capture-only lighting: ambient(.33,.36,.35), LateAfternoon intensity .64, shadow distance170m; restored afterward. Production materials and geometry untouched. Physical reservation color markers retained. OnGUI/HUD numbers are not captured; no synthetic HUD. Image mean/white-clipping/10th-to-90th percentile contrast checked and recorded in actual CSV.\n");
   }finally{Shader.SetGlobalFloat("_HarborFogEnabled",fog);RenderSettings.ambientLight=ambient;if(sun)sun.intensity=power;QualitySettings.shadowDistance=shadowDistance;Destroy(go);}
  }
  IEnumerator VerifyCargoMaze2712(){
   Check(Application.version=="2.7.12"&&Path.GetFileName(G.SavePath)=="verification-stash-2.7.12.json","2712 Dedicated build and isolated verification inventory");string directory=Path.GetDirectoryName(G.SavePath);string[] formal={"stash-v40.json","stash-v41.json","stash-v42.json"};byte[][] before=Array.ConvertAll(formal,n=>File.Exists(Path.Combine(directory,n))?File.ReadAllBytes(Path.Combine(directory,n)):null);var controls=G.Controls;var random=UnityEngine.Random.state;bool harbor=G.HarborWalkthrough;
   FreshCamp();string camp=JsonUtility.ToJson(G.Save);G.HarborWalkthrough=true;G.HardMode=false;Check(G.BeginTraining(),"2712 Build actual complete harbor in isolated training session");G.Paused=true;yield return null;
   try{G.Controls=new TouchInput();Check(CargoPreview2712.Apply(G),"2712 Actual cargo preview positions player at clear southern approach");Physics.SyncTransforms();Check(G.Map.HarborWhitebox&&CargoClear2712(G.Player.transform.position),"2712 Preserved southern cargo approach has real walkable ground");if(Array.IndexOf(Environment.GetCommandLineArgs(),"--capture-cargo-maze-2712")<0){CargoGeometry2712();CargoGaps2712();CargoGraph2712();yield return CargoRoutes2712();CargoSprint2712();CargoSight2712();}else{results.Add("INFO 2712 Capture-only: geometry/traversal suites skipped; isolated preview, true camera rendering and save protection retained.");}yield return CargoPictures2712();G.Finish(false);yield return null;Check(JsonUtility.ToJson(G.Save)==camp,"2712 Cargo-only preview restores inventory without economy changes");for(int i=0;i<formal.Length;i++)Check(MainBytes273(before[i],File.Exists(Path.Combine(directory,formal[i]))?File.ReadAllBytes(Path.Combine(directory,formal[i])):null),"2712 Formal save remains byte-identical "+formal[i]);}
   finally{G.Controls=controls;G.HarborWalkthrough=harbor;UnityEngine.Random.state=random;G.Paused=true;File.WriteAllLines(CargoCsvPath2712(),cargoRows2712);}
  }
 }
}
