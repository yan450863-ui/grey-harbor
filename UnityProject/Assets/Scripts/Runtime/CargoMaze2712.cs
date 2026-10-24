using UnityEngine;
using System.Collections.Generic;
namespace GreyHarbor {
 public sealed class CargoBox2712 {public GameObject View;public Vector3 Center,Size;public float Yaw;public bool Stacked;public Rect Footprint;}
 public sealed class CargoProp2712 {public string Type;public GameObject View;public Vector3 Center,Size;public float Yaw;public Rect Footprint;public ObstacleKind Kind;}
 public sealed class CargoLane2712 {public string Name;public Vector3[] Points;public float Width;}
 public sealed class CargoGap2712 {public string Name;public Vector3 Center;public float Width;public bool Main;}
 public sealed class CargoReservation2712 {public string Name;public bool Monster;public Vector3 Position,Stand;public Rect ActivityArea;public Vector3[] Patrol;public GameObject Marker;}
 public sealed partial class RaidMap {
  public static readonly Rect CargoBounds2712=new Rect(-23.5f,-12.5f,74,69);
  public static readonly Vector3 CargoPreviewSpawn2712=PortPoint(92,54);
  public readonly List<CargoBox2712> CargoBoxes2712=new List<CargoBox2712>();
  public readonly List<CargoProp2712> CargoProps2712=new List<CargoProp2712>();
  public readonly List<CargoLane2712> CargoLanes2712=new List<CargoLane2712>();
  public readonly List<CargoGap2712> CargoGaps2712=new List<CargoGap2712>();
  public readonly List<Vector3> CargoSearchReserved2712=new List<Vector3>(),CargoMonsterReserved2712=new List<Vector3>();
  public readonly List<CargoReservation2712> CargoReservations2712=new List<CargoReservation2712>();
  static Rect CargoFootprint2712(Vector3 center,Vector3 size,float yaw){float a=yaw*Mathf.Deg2Rad,w=Mathf.Abs(Mathf.Cos(a))*size.x+Mathf.Abs(Mathf.Sin(a))*size.z,d=Mathf.Abs(Mathf.Sin(a))*size.x+Mathf.Abs(Mathf.Cos(a))*size.z;return new Rect(center.x-w*.5f,center.z-d*.5f,w,d);}
  void CargoBoxAdd2712(float x,float z,float length,float yaw=0,bool stacked=false,int support=-1){
   var center=PortPoint(x,z)+Vector3.up*(stacked?3.9f:1.3f);var size=new Vector3(length,2.6f,2.6f);
   string name=stacked?"CargoMaze_Stack_"+support:"CargoMaze_"+CargoBoxes2712.Count;
   var view=Box(name,center,size,stacked?"E1E3DC":length<7?"C8D1CA":"B8C2BD",true);view.transform.rotation=Quaternion.Euler(0,yaw,0);
   CargoMazeBoxes.Add(view);CargoBoxes2712.Add(new CargoBox2712{View=view,Center=center,Size=size,Yaw=yaw,Stacked=stacked,Footprint=CargoFootprint2712(center,size,yaw)});
  }
  void CargoLaneAdd2712(string name,float width,params Vector3[] points){CargoLanes2712.Add(new CargoLane2712{Name=name,Width=width,Points=points});CargoPassages.Add(points);CargoRouteChecks.AddRange(points);}
  void BuildCargoMaze2712(){
   if(CargoBoxes2712.Count>0)return;
   // 2.7.13 container-only sketch: fixed L/T/U groups around a central snake.
   // Offline OBB/whole-lane checks produce this authored array; no random fill.
   // Legacy props, reservation and gap APIs intentionally stay empty.
   var plan=new[]{
    new Vector4(57.1f,68.8f,12.2f,90f),
    new Vector4(58.2f,85.4f,12.2f,90f),
    new Vector4(57.2f,107.5f,12.2f,90f),
    new Vector4(58.4f,122.2f,12.2f,90f),
    new Vector4(76.5f,63.2f,12.2f,5f),
    new Vector4(76.8f,107f,12.2f,-12f),
    new Vector4(83f,121f,12.2f,35f),
    new Vector4(72f,127f,12.2f,0f),
    new Vector4(126.8f,68.5f,12.2f,90f),
    new Vector4(126.7f,85f,12.2f,90f),
    new Vector4(127f,114.8f,12.2f,90f),
    new Vector4(126.9f,125.6f,6.2f,90f),
    new Vector4(111f,63.2f,12.2f,0f),
    new Vector4(104.5f,74.5f,12.2f,22f),
    new Vector4(107f,84f,6.2f,-10f),
    new Vector4(106f,106.2f,6.2f,90f),
    new Vector4(106.4f,116.9f,12.2f,-28f),
    new Vector4(60.1f,68.8f,12.2f,90f),
    new Vector4(61.2f,85.4f,12.2f,90f),
    new Vector4(60.2f,107.5f,12.2f,90f),
    new Vector4(61.4f,122.2f,12.2f,90f),
    new Vector4(68.9787f,63.858f,6.2f,95f),
    new Vector4(85.8144f,62.3851f,6.2f,5f),
    new Vector4(84.7207f,123.4575f,12.2f,35f),
    new Vector4(81.2793f,118.5425f,12.2f,35f),
    new Vector4(89.1846f,116.6695f,6.2f,125f),
    new Vector4(123.8f,68.5f,12.2f,90f),
    new Vector4(123.7f,85f,12.2f,90f),
    new Vector4(126.7f,94.35f,6.2f,90f),
    new Vector4(124f,114.8f,12.2f,90f),
    new Vector4(123.9f,125.6f,6.2f,90f),
    new Vector4(101.65f,63.2f,6.2f,0f),
    new Vector4(105.6238f,77.2816f,12.2f,22f),
    new Vector4(97.4998f,77.3283f,6.2f,112f),
    new Vector4(107.5209f,81.0456f,6.2f,-10f),
    new Vector4(100.7465f,82.8973f,6.2f,-10f),
    new Vector4(103f,106.2f,6.2f,90f),
    new Vector4(106f,110.75f,6.2f,180f),
    new Vector4(76.4f,80.4f,6.2f,0f),
    new Vector4(75f,84.9f,6.2f,0f),
    new Vector4(73.5f,97.5f,6.2f,0f),
    new Vector4(64.3f,109.9f,6.2f,90f),
    new Vector4(89.6f,106f,6.2f,90f),
    new Vector4(76.4f,77.4f,6.2f,0f),
    new Vector4(80.95f,80.4f,6.2f,90f),
    new Vector4(67.15f,97.5f,6.2f,0f),
    new Vector4(64.3f,103.55f,6.2f,90f),
    new Vector4(64.3f,116.25f,6.2f,90f),
    new Vector4(117.8f,85.3f,6.2f,90f),
    new Vector4(114.8f,91.65f,6.2f,90f),
    new Vector4(93.7f,73f,6.2f,90f),
    new Vector4(100f,109.8f,6.2f,90f),
    new Vector4(86.6f,106f,6.2f,90f),
    new Vector4(89.6f,101.45f,6.2f,180f),
    new Vector4(63.9f,76.8f,6.2f,0f),
    new Vector4(68.2f,119.5f,6.2f,90f),
    new Vector4(86f,91.5f,6.2f,16f),
    new Vector4(89f,127f,6.2f,0f),
    new Vector4(119.8f,104.5f,6.2f,0f),
    new Vector4(111.5f,128f,12.2f,0f),
    new Vector4(121f,125.8f,6.2f,90f),
    new Vector4(120f,64.7f,6.2f,90f),
    new Vector4(91f,92f,6.2f,90f),
    new Vector4(111f,112.7f,6.2f,90f),
   };
   foreach(var box in plan)CargoBoxAdd2712(box.x,box.y,box.z,box.w);
   CargoLaneAdd2712("中央蛇形主路",3.5f,PortPoint(92f,54f),PortPoint(92f,64f),PortPoint(89f,70f),PortPoint(89f,78f),PortPoint(95f,82f),PortPoint(95f,90f),PortPoint(100f,94f),PortPoint(100f,101f),PortPoint(94f,105f),PortPoint(94f,112f),PortPoint(98f,117f),PortPoint(98f,126f),PortPoint(98f,137f));
   CargoLaneAdd2712("西侧折入",3.5f,PortPoint(51f,96f),PortPoint(60f,96f),PortPoint(66f,93f),PortPoint(76f,93f),PortPoint(84f,97f),PortPoint(100f,97f));
   CargoLaneAdd2712("东侧折出",3.5f,PortPoint(100f,100f),PortPoint(107f,97f),PortPoint(116f,97f),PortPoint(126f,102f),PortPoint(139f,102f));
   CargoLaneAdd2712("西南回环",2.2f,PortPoint(89f,76f),PortPoint(83f,76f),PortPoint(82f,69f),PortPoint(72f,69f),PortPoint(72f,78f),PortPoint(65f,81f),PortPoint(65f,89f),PortPoint(77f,89f),PortPoint(83f,85f),PortPoint(95f,85f));
   CargoLaneAdd2712("西北回环",2.2f,PortPoint(100f,97f),PortPoint(85f,97f),PortPoint(80f,102f),PortPoint(67f,102f),PortPoint(67f,112f),PortPoint(76f,116f),PortPoint(83f,112f),PortPoint(94f,112f));
   CargoLaneAdd2712("东南回环",2.2f,PortPoint(92f,64f),PortPoint(102f,68f),PortPoint(117f,68f),PortPoint(121f,76f),PortPoint(113f,81f),PortPoint(112f,88f),PortPoint(101f,89f),PortPoint(95f,89f));
   CargoLaneAdd2712("东北回环",2.2f,PortPoint(100f,100f),PortPoint(108f,100f),PortPoint(113f,108f),PortPoint(120f,111f),PortPoint(118f,121f),PortPoint(108f,125f),PortPoint(98f,125f));
   DressCargoContainers2714();
   BuildCargoDressing2714();
   Physics.SyncTransforms();
  }
 }
}


