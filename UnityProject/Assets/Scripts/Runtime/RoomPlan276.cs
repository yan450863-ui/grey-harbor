using UnityEngine;
using System.Collections.Generic;
namespace GreyHarbor {
 public sealed partial class RaidMap {
  public const float MainWidth276=40.5f,MainDepth276=48f,MainNetHalfX276=20.04f,MainNetHalfZ276=23.79f;
  public const float MainShellX276=20.145f,MainShellZ276=23.895f,MainWallThickness276=.21f;
  public static readonly Rect MainBounds276=new Rect(-MainWidth276*.5f,-MainDepth276*.5f,MainWidth276,MainDepth276);
  public static readonly Rect MainNet276=new Rect(-MainNetHalfX276,-MainNetHalfZ276,MainNetHalfX276*2,MainNetHalfZ276*2);
  public static readonly Rect MainHall276=new Rect(-14.3f,-MainNetHalfZ276,27f,MainNetHalfZ276*2);
  public static bool TryMainRoomPlan276(int floor,string name,out Rect area,out Vector3 door,out int side){
   area=new Rect();door=Vector3.zero;side=name.StartsWith("西")||name=="零件整理与待修件间"?-1:1;
   if(name=="西北楼梯"||name=="西北楼梯间"){area=Rect.MinMaxRect(-MainNetHalfX276,16.2f,-14.4f,MainNetHalfZ276);door=new Vector3(-14.4f,0,19.3f);side=-1;return true;}
   if(name=="东南楼梯"||name=="东南楼梯间"){area=Rect.MinMaxRect(14.4f,-15f,MainNetHalfX276,-7.2f);door=new Vector3(14.4f,0,-11.4f);side=1;return true;}
   if(floor==1){
    if(name=="零件整理与待修件间"){area=Rect.MinMaxRect(-MainNetHalfX276,-MainNetHalfZ276,-14.4f,16.2f);door=new Vector3(-14.4f,0,-10.5f);side=-1;}
    else if(name=="维修值班办公室"){area=Rect.MinMaxRect(12.8f,-7.2f,MainNetHalfX276,2.6f);door=new Vector3(12.8f,0,-2f);}
    else if(name=="设备与配电间"){area=Rect.MinMaxRect(12.8f,2.6f,MainNetHalfX276,MainNetHalfZ276);door=new Vector3(12.8f,0,10f);}
    else return false;return true;
   }
   if(floor==2){
    if(name=="部件拆修车间"){area=Rect.MinMaxRect(-MainNetHalfX276,-MainNetHalfZ276,-2.6f,-1.8f);door=new Vector3(-2.6f,0,-7f);side=-1;}
    else if(name=="精密测试间"){area=Rect.MinMaxRect(-MainNetHalfX276,1.8f,-2.6f,16.2f);door=new Vector3(-2.6f,0,8.5f);side=-1;}
    else if(name=="工具校验间"){area=Rect.MinMaxRect(2.6f,-7.2f,MainNetHalfX276,-1.8f);door=new Vector3(2.6f,0,-4.5f);}
    else if(name=="维修工艺与备件整理间"){area=Rect.MinMaxRect(2.6f,8f,MainNetHalfX276,MainNetHalfZ276);door=new Vector3(2.6f,0,15.3f);}
    else return false;return true;
   }
   if(name=="电气装配室"||name=="职工休息室"){
    area=Rect.MinMaxRect(-MainNetHalfX276,-MainNetHalfZ276,-2.6f,floor==3?-9f:-10.5f);door=new Vector3(-2.6f,0,floor==3?-17f:-17.2f);side=-1;
   }else if(name=="继电与仪表测试室"){
    area=Rect.MinMaxRect(-MainNetHalfX276,-9f,-2.6f,-1.8f);door=new Vector3(-2.6f,0,-5.3f);side=-1;
   }else if(name=="更衣与清洁室"){
    area=Rect.MinMaxRect(-MainNetHalfX276,-10.5f,-10f,-1.8f);door=new Vector3(-10f,0,-6f);side=-1;
   }else if(name=="技术设计办公室"||name=="管理办公室"){
    area=Rect.MinMaxRect(-MainNetHalfX276,1.8f,-10f,10.4f);door=new Vector3(-10f,0,6.2f);side=-1;
   }else if(name=="维修记录登记室"||name=="值班管理室"){
    area=Rect.MinMaxRect(2.6f,-MainNetHalfZ276,12.8f,-15f);door=new Vector3(2.6f,0,-19.2f);
   }else if(name=="工程资料档案室"){
    area=Rect.MinMaxRect(2.6f,-7.2f,MainNetHalfX276,-1.8f);door=new Vector3(2.6f,0,-4.5f);
   }else if(name=="技术会议室"||name=="会议室"){
    area=Rect.MinMaxRect(2.6f,1.8f,MainNetHalfX276,11.8f);door=new Vector3(2.6f,0,6.4f);
   }else if(name=="人事与排班办公室"){
    area=Rect.MinMaxRect(2.6f,-7.2f,12.8f,-1.8f);door=new Vector3(2.6f,0,-4.5f);
   }else if(name=="配电监测办公室"){
    area=Rect.MinMaxRect(10f,11.8f,MainNetHalfX276,MainNetHalfZ276);door=new Vector3(10f,0,17.8f);
   }else if(name=="钥匙管理室"){
    area=Rect.MinMaxRect(12.8f,16.2f,MainNetHalfX276,MainNetHalfZ276);door=new Vector3(12.8f,0,19.6f);
   }else return false;return true;
  }
  public static Rect RepairMainRoomArea276(int floor,string name){Rect r;Vector3 p;int side;if(!TryMainRoomPlan276(floor,name,out r,out p,out side))throw new System.ArgumentException("Unknown main-building room: "+floor+" / "+name);return r;}
  public static Vector3[] RepairMainWindowPoints276(int floor){
   var points=new List<Vector3>();
   if(floor==1){foreach(float z in new[]{-5.4f,4.4f})points.Add(new Vector3(-MainNetHalfX276+.13f,2,MainPoint273(new Vector3(0,0,z)).z));}
   else foreach(float z in new[]{-12f,4.4f,12.5f})foreach(float side in new[]{-1f,1f})points.Add(new Vector3(side*(MainNetHalfX276-.14f),2,MainPoint273(new Vector3(0,0,z)).z));
   return points.ToArray();
  }
  public static Rect[] RepairMainPublicAreas276(int floor){
   if(floor==1)return new[]{MainHall276,new Rect(12.8f,-MainNetHalfZ276,7.24f,8.69f),new Rect(12.8f,-14.9f,1.5f,7.6f),Rect.MinMaxRect(12.8f,-15.1f,14.3f,-14.9f)};
   var areas=new List<Rect>{new Rect(-2.5f,-MainNetHalfZ276,5,MainNetHalfZ276*2),new Rect(-MainNetHalfX276,-1.7f,MainNetHalfX276*2,3.4f),new Rect(-14.3f,16.3f,11.8f,7.49f),new Rect(2.5f,-14.9f,11.8f,7.6f)};
   if(floor==2){areas.Add(new Rect(2.6f,1.8f,17.44f,6.1f));areas.Add(new Rect(2.5f,-MainNetHalfZ276,17.54f,8.69f));areas.Add(Rect.MinMaxRect(2.5f,-15.1f,14.3f,-14.9f));}
   else {
    areas.Add(new Rect(12.9f,-MainNetHalfZ276,7.14f,8.69f));
    areas.Add(Rect.MinMaxRect(12.9f,-15.1f,14.3f,-14.9f));
    areas.Add(Rect.MinMaxRect(-14.3f,16.2f,-2.5f,16.3f));
    areas.Add(new Rect(-9.9f,1.8f,7.4f,14.4f));areas.Add(new Rect(-MainNetHalfX276,10.5f,10.14f,5.7f));
    if(floor==3)areas.Add(new Rect(2.5f,11.9f,7.4f,11.89f));
    else {areas.Add(new Rect(-9.9f,-10.4f,7.4f,8.6f));areas.Add(new Rect(12.9f,-7.1f,7.14f,5.3f));areas.Add(new Rect(2.5f,11.9f,10.2f,11.89f));areas.Add(new Rect(12.8f,11.9f,7.24f,4.2f));}
   }return areas.ToArray();
  }
  readonly Dictionary<string,List<Vector2>> MainRoomEdges276=new Dictionary<string,List<Vector2>>();
  void MainRoomEdge276(float x,float z,float w,float d){
   bool horizontal=w>d;float axis=horizontal?z:x,thickness=horizontal?d:w,center=horizontal?x:z,length=horizontal?w:d;
   float low=center-length*.5f,high=center+length*.5f;
   string key=(horizontal?"H":"V")+Mathf.RoundToInt(axis*1000)+":"+Mathf.RoundToInt(thickness*1000);
   List<Vector2> spans;if(!MainRoomEdges276.TryGetValue(key,out spans)){spans=new List<Vector2>();MainRoomEdges276.Add(key,spans);}
   // Only uncovered solid intervals create geometry. Door openings are never registered.
   float cursor=low;
   foreach(var span in spans){
    if(span.y<=cursor)continue;if(span.x>=high)break;
    if(span.x>cursor+.0001f)MainRoomSegment276(horizontal,axis,thickness,cursor,Mathf.Min(span.x,high));
    cursor=Mathf.Max(cursor,span.y);if(cursor>=high)break;
   }
   if(cursor<high-.0001f)MainRoomSegment276(horizontal,axis,thickness,cursor,high);
   spans.Add(new Vector2(low,high));spans.Sort((a,b)=>a.x.CompareTo(b.x));
   for(int i=0;i+1<spans.Count;){
    var current=spans[i];var next=spans[i+1];
    if(next.x<=current.y+.0001f){current.y=Mathf.Max(current.y,next.y);spans[i]=current;spans.RemoveAt(i+1);}else i++;
   }
  }
  void MainRoomSegment276(bool horizontal,float axis,float thickness,float low,float high){
   float center=(low+high)*.5f,length=high-low;
   RepairMainWall210(horizontal?center:axis,horizontal?axis:center,horizontal?length:thickness,horizontal?thickness:length);
  }
  void MainShell276(string floorName){
   Box(floorName,new Vector3(0,-.12f,0),new Vector3(MainNet276.width,.24f,MainNet276.height),"777D70");
   RepairMainWall210(-MainShellX276,0,MainWallThickness276,MainDepth276);RepairMainWall210(MainShellX276,0,MainWallThickness276,MainDepth276);
   RepairMainWall210(0,-MainShellZ276,MainWidth276,MainWallThickness276);RepairMainWall210(0,MainShellZ276,MainWidth276,MainWallThickness276);
  }
 }
}
