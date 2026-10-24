using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
namespace GreyHarbor {
 public sealed class UndergroundEntry2752 {public string Id,Name;public Vector3 Arrival,ExitPosition;}
 public sealed partial class RaidMap {
  public bool UndergroundWhitebox2752;
  public WorldDoor UndergroundGate2752;
  public readonly List<UndergroundEntry2752> UndergroundEntries2752=new List<UndergroundEntry2752>();
  public readonly List<Vector3[]> UndergroundRoutes2752=new List<Vector3[]>();
  public readonly List<Rect> UndergroundOpenAreas2753=new List<Rect>();
  public readonly List<Rect> UndergroundFloorAreas2752=new List<Rect>();
  Texture2D undergroundPlan2752;
  const float UndergroundCell2752=.1f,UndergroundWallHeight2752=2.6f;
  public void BuildUnderground2752(){
   UndergroundWhitebox2752=IsInterior=true;InteriorId="underground2752";InteriorTitle="港口地下管网";
   Bounds=new Rect(-60,-60,120,120);InitialRoom=Bounds;Root=new GameObject("UndergroundConnectedWhitebox2752").transform;
   UndergroundPath2752(3,-28,-42,-28,-30,-31,-30,-31,-16,-28,-16,-28,15,-31,15,-31,29,-28,29,-28,42);
   UndergroundPath2752(3,28,-42,28,-28,31,-28,31,-14,28,-14,28,18,31,18,31,31,28,31,28,42);
   UndergroundPath2752(2.8f,-28,42,-11,42,-11,45,12,45,12,42,28,42);
   UndergroundPath2752(2.8f,-28,16,-16,16,-16,12,-7,12,-7,16,15,16,15,20,28,20);
   UndergroundPath2752(2.8f,-28,-12,-19,-12,-19,-17,-8,-17,-8,-12,8,-12,8,-8,20,-8,20,-12,28,-12);
   UndergroundPath2752(2.8f,-28,-42,-15,-42,-15,-45,0,-45,0,-42,16,-42,16,-45,28,-45,28,-42);
   // Open work platforms and junctions retain the continuous tunnel layout.
   // No enclosing doors: container edges and combat lanes will be furnished later.
   UndergroundOpenAreas2753.Add(new Rect(-38,23,14,12));
   UndergroundOpenAreas2753.Add(new Rect(24,0,14,12));
   UndergroundOpenAreas2753.Add(new Rect(-19,8,16,12));
   UndergroundOpenAreas2753.Add(new Rect(-16,-23,18,12));
   UndergroundOpenAreas2753.Add(new Rect(19,-34,18,12));
   UndergroundOpenAreas2753.Add(new Rect(-11,36,22,12));
   UndergroundFloorAreas2752.AddRange(UndergroundOpenAreas2753);
   UndergroundPath2752(1.8f,-31,28,-42,28,-42,34,-47,34);
   UndergroundPath2752(1.8f,31,-28,43,-28,43,-23,49,-23);
   UndergroundPath2752(1.8f,-28,-30,-41,-30,-41,-27,-47,-27);
   UndergroundPath2752(1.8f,31,18,42,18,42,24,47,24);
   UndergroundEntryPath2752("freight","西北货运场",new Vector3(-51,0,46),new Vector3(-51,0,48),2.2f,-28,42,-43,42,-43,46,-51,46,-51,48);
   UndergroundEntryPath2752("repair","维修附属房",new Vector3(-53,0,-8),new Vector3(-55,0,-8),2.2f,-28,-8,-40,-8,-40,-5,-47,-5,-47,-8,-55,-8);
   UndergroundEntryPath2752("cargo","中央集装箱检修井",new Vector3(0,0,17.5f),new Vector3(0,0,20),2.2f,0,16,0,20);
   UndergroundEntryPath2752("office","办公楼设备井",new Vector3(28,0,51),new Vector3(28,0,53),2.2f,28,42,28,53);
   UndergroundEntryPath2752("loading","装卸区",new Vector3(52,0,46),new Vector3(52,0,48),2.2f,28,42,44,42,44,46,52,46,52,48);
   UndergroundEntryPath2752("south","南部入口附近",new Vector3(0,0,-53),new Vector3(0,0,-55),3,0,-42,0,-55);
   Spawn=UndergroundEntries2752.Find(e=>e.Id=="south").Arrival;
   UndergroundBuildUnion2752();UndergroundBuildExitSteps2752();UndergroundBuildGate2752();FinishUnderground2754();Physics.SyncTransforms();
  }
  void UndergroundPath2752(float width,params float[] points){
   var route=new Vector3[points.Length/2];float half=width/2;
   for(int i=0;i<route.Length;i++)route[i]=new Vector3(points[i*2],0,points[i*2+1]);
   for(int i=1;i<route.Length;i++){var a=route[i-1];var b=route[i];if(Mathf.Abs(a.x-b.x)>.001f&&Mathf.Abs(a.z-b.z)>.001f)throw new System.InvalidOperationException("Underground route must be orthogonal");
    UndergroundFloorAreas2752.Add(Rect.MinMaxRect(Mathf.Min(a.x,b.x)-half,Mathf.Min(a.z,b.z)-half,Mathf.Max(a.x,b.x)+half,Mathf.Max(a.z,b.z)+half));
   }
   UndergroundRoutes2752.Add(route);
  }
  void UndergroundEntryPath2752(string id,string name,Vector3 arrival,Vector3 exit,float width,params float[] points){
   UndergroundPath2752(width,points);UndergroundFloorAreas2752.Add(new Rect(exit.x-1.5f,exit.z-1.5f,3,3));
   UndergroundEntries2752.Add(new UndergroundEntry2752{Id=id,Name=name,Arrival=arrival,ExitPosition=exit});
   Doors.Add(new WorldDoor{Name=name+" · 返回地面",Destination="underground-exit:"+id,Position=exit,Portal=true});
  }
  bool UndergroundPoint2752(Vector3 p){var q=new Vector2(p.x,p.z);foreach(var area in UndergroundFloorAreas2752)if(q.x>=area.xMin-.00001f&&q.x<=area.xMax+.00001f&&q.y>=area.yMin-.00001f&&q.y<=area.yMax+.00001f)return true;return false;}
  public bool UndergroundFloorContains2752(Vector3 p,float radius){
   if(!UndergroundWhitebox2752||!UndergroundPoint2752(p))return false;
   if(radius<=0)return true;for(int i=0;i<24;i++){float angle=i*Mathf.PI/12;if(!UndergroundPoint2752(p+new Vector3(Mathf.Cos(angle),0,Mathf.Sin(angle))*radius))return false;}return true;
  }
  void UndergroundBuildUnion2752(){
   int width=Mathf.RoundToInt(Bounds.width/UndergroundCell2752),height=Mathf.RoundToInt(Bounds.height/UndergroundCell2752);var occupied=new bool[width*height];
   foreach(var area in UndergroundFloorAreas2752){int x0=Mathf.Clamp(Mathf.RoundToInt((area.xMin-Bounds.xMin)/UndergroundCell2752),0,width),x1=Mathf.Clamp(Mathf.RoundToInt((area.xMax-Bounds.xMin)/UndergroundCell2752),0,width),z0=Mathf.Clamp(Mathf.RoundToInt((area.yMin-Bounds.yMin)/UndergroundCell2752),0,height),z1=Mathf.Clamp(Mathf.RoundToInt((area.yMax-Bounds.yMin)/UndergroundCell2752),0,height);
    for(int z=z0;z<z1;z++)for(int x=x0;x<x1;x++)occupied[z*width+x]=true;
   }
   var floorV=new List<Vector3>();var floorT=new List<int>();var active=new Dictionary<long,Rect>();
   for(int z=0;z<=height;z++){
    var next=new Dictionary<long,Rect>();if(z<height)for(int x=0;x<width;){if(!occupied[z*width+x]){x++;continue;}int start=x;while(x<width&&occupied[z*width+x])x++;long key=((long)start<<32)|(uint)x;
     if(active.TryGetValue(key,out var rectangle)){rectangle.height+=UndergroundCell2752;next.Add(key,rectangle);active.Remove(key);}else next.Add(key,new Rect(Bounds.xMin+start*UndergroundCell2752,Bounds.yMin+z*UndergroundCell2752,(x-start)*UndergroundCell2752,UndergroundCell2752));
    }
    foreach(var area in active.Values)UndergroundQuad2752(floorV,floorT,new Vector3(area.xMin,.012f,area.yMin),new Vector3(area.xMin,.012f,area.yMax),new Vector3(area.xMax,.012f,area.yMax),new Vector3(area.xMax,.012f,area.yMin));active=next;
   }
   UndergroundMesh2752("UndergroundUnionFloor2752",floorV,floorT,"78847D",false);
   var wallV=new List<Vector3>();var wallT=new List<int>();int wallIndex=0;
   for(int z=0;z<=height;z++)for(int x=0;x<width;){bool lower=z>0&&occupied[(z-1)*width+x],upper=z<height&&occupied[z*width+x];if(lower==upper){x++;continue;}int start=x;bool side=lower;while(x<width){lower=z>0&&occupied[(z-1)*width+x];upper=z<height&&occupied[z*width+x];if(lower==upper||lower!=side)break;x++;}
    float low=Bounds.xMin+start*UndergroundCell2752,high=Bounds.xMin+x*UndergroundCell2752,line=Bounds.yMin+z*UndergroundCell2752+(side?.09f:-.09f);
    UndergroundWall2752(wallV,wallT,new Vector3((low+high)/2,1.3f,line),new Vector3(high-low+.18f,2.6f,.18f),wallIndex++);
   }
   for(int x=0;x<=width;x++)for(int z=0;z<height;){bool left=x>0&&occupied[z*width+x-1],right=x<width&&occupied[z*width+x];if(left==right){z++;continue;}int start=z;bool side=left;while(z<height){left=x>0&&occupied[z*width+x-1];right=x<width&&occupied[z*width+x];if(left==right||left!=side)break;z++;}
    float low=Bounds.yMin+start*UndergroundCell2752,high=Bounds.yMin+z*UndergroundCell2752,line=Bounds.xMin+x*UndergroundCell2752+(side?.09f:-.09f);
    UndergroundWall2752(wallV,wallT,new Vector3(line,1.3f,(low+high)/2),new Vector3(.18f,2.6f,high-low+.18f),wallIndex++);
   }
   UndergroundMesh2752("UndergroundUnionWalls2752",wallV,wallT,"98AAA1",true);
  }
  void UndergroundWall2752(List<Vector3> v,List<int> t,Vector3 center,Vector3 size,int index,bool collision=true){
   if(collision){var cube=new GameObject("UndergroundWall2752_"+index);cube.transform.SetParent(Root,false);cube.transform.position=center;var col=cube.AddComponent<BoxCollider>();col.size=size;SceneObstacle.Apply(cube,ObstacleKind.High);}
   Vector3 a=center-size/2,b=center+size/2;
   UndergroundQuad2752(v,t,new Vector3(a.x,a.y,a.z),new Vector3(a.x,b.y,a.z),new Vector3(b.x,b.y,a.z),new Vector3(b.x,a.y,a.z));
   UndergroundQuad2752(v,t,new Vector3(b.x,a.y,b.z),new Vector3(b.x,b.y,b.z),new Vector3(a.x,b.y,b.z),new Vector3(a.x,a.y,b.z));
   UndergroundQuad2752(v,t,new Vector3(a.x,a.y,b.z),new Vector3(a.x,b.y,b.z),new Vector3(a.x,b.y,a.z),new Vector3(a.x,a.y,a.z));
   UndergroundQuad2752(v,t,new Vector3(b.x,a.y,a.z),new Vector3(b.x,b.y,a.z),new Vector3(b.x,b.y,b.z),new Vector3(b.x,a.y,b.z));
   UndergroundQuad2752(v,t,new Vector3(a.x,b.y,a.z),new Vector3(a.x,b.y,b.z),new Vector3(b.x,b.y,b.z),new Vector3(b.x,b.y,a.z));
  }
  void UndergroundBuildExitSteps2752(){
   var vertices=new List<Vector3>();var triangles=new List<int>();int index=0;
   foreach(var entry in UndergroundEntries2752){var direction=(entry.ExitPosition-entry.Arrival).normalized;
    for(int i=0;i<5;i++){float h=.12f*(i+1);var p=entry.ExitPosition+direction*(.40f+i*.16f)+Vector3.up*(h/2+.015f);var size=Mathf.Abs(direction.x)>.5f?new Vector3(.19f,h,1.4f):new Vector3(1.4f,h,.19f);UndergroundWall2752(vertices,triangles,p,size,index++,false);}
   }
   UndergroundMesh2752("UndergroundExitSteps2752",vertices,triangles,"B9C3B9",false);
  }
  static void UndergroundQuad2752(List<Vector3> v,List<int> t,Vector3 a,Vector3 b,Vector3 c,Vector3 d){int start=v.Count;v.Add(a);v.Add(b);v.Add(c);v.Add(d);t.Add(start);t.Add(start+1);t.Add(start+2);t.Add(start);t.Add(start+2);t.Add(start+3);}
  void UndergroundMesh2752(string name,List<Vector3> vertices,List<int> triangles,string color,bool wall){
   var view=new GameObject(name);view.transform.SetParent(Root,false);var mesh=new Mesh{name=name};if(vertices.Count>65535)mesh.indexFormat=IndexFormat.UInt32;mesh.SetVertices(vertices);mesh.SetTriangles(triangles,0);mesh.RecalculateNormals();mesh.RecalculateBounds();view.AddComponent<MeshFilter>().sharedMesh=mesh;
   var renderer=view.AddComponent<MeshRenderer>();renderer.sharedMaterial=Mat(color,wall);renderer.shadowCastingMode=ShadowCastingMode.Off;renderer.receiveShadows=true;var life=Root.GetComponent<WorkshopArtResources>()??Root.gameObject.AddComponent<WorkshopArtResources>();life.Meshes.Add(mesh);
  }
  void UndergroundBuildGate2752(){
   foreach(float x in new[]{-1.4f,1.4f})Box("UndergroundGateJamb2752",new Vector3(x,1.3f,-49),new Vector3(.2f,2.6f,.18f),"98AAA1",true);
   var pivot=new GameObject("UndergroundGateHinge2752").transform;pivot.SetParent(Root,false);pivot.position=new Vector3(-1.3f,0,-49);
   var slab=Box("UndergroundGateLeaf2752",new Vector3(0,1.15f,-49),new Vector3(2.6f,2.3f,.10f),"83978D",true);slab.transform.SetParent(pivot,true);var safety=new Rect(-1.3f,-49.08f,2.6f,.16f);
   UndergroundGate2752=new WorldDoor{Name="南部入口检修门",Destination=InteriorId,Position=new Vector3(0,0,-49),Pivot=pivot,Collider=slab.GetComponent<Collider>(),OpenAngle=-90,Locked=false,Portal=false,InstantToggle=true,ClosedSafety=safety,OpenSafety=safety};Doors.Add(UndergroundGate2752);
  }
  public Texture2D UndergroundPlanTexture2752(){
   if(undergroundPlan2752)return undergroundPlan2752;
   const int size=640;var pixels=new Color32[size*size];var open=new bool[size*size];float step=Bounds.width/size;
   for(int z=0;z<size;z++)for(int x=0;x<size;x++)open[z*size+x]=UndergroundPoint2752(new Vector3(Bounds.xMin+(x+.5f)*step,0,Bounds.yMin+(z+.5f)*Bounds.height/size));
   for(int z=0;z<size;z++)for(int x=0;x<size;x++){int n=z*size+x;bool edge=!open[n]&&((x>0&&open[n-1])||(x<size-1&&open[n+1])||(z>0&&open[n-size])||(z<size-1&&open[n+size]));pixels[n]=open[n]?new Color32(119,137,129,255):edge?new Color32(160,177,164,255):new Color32(17,26,25,255);}
   undergroundPlan2752=new Texture2D(size,size,TextureFormat.RGBA32,false){name="UndergroundPlan2752",filterMode=FilterMode.Point,wrapMode=TextureWrapMode.Clamp};undergroundPlan2752.SetPixels32(pixels);undergroundPlan2752.Apply(false,false);
   var owner=new GameObject("UndergroundPlanLifetime2752");owner.transform.SetParent(Root,false);owner.AddComponent<StableGroundResources137>().Texture=undergroundPlan2752;return undergroundPlan2752;
  }
 }
}
