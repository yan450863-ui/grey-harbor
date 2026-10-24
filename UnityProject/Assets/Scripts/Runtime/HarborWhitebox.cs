using UnityEngine;
using System.Collections.Generic;
namespace GreyHarbor {
 public sealed partial class RaidMap {
  public bool HarborWhitebox;
  public readonly List<Rect> WhiteboxRoads=new List<Rect>();
  public static readonly string[] WhiteboxNames={"南侧入口区","西侧维修区","中部集装箱区","办公楼","装卸区","堤岸主通道","后勤旧院","北部渔业码头","中部客运码头","南部装卸码头"};
  public static readonly Vector3 YardGate=PortPoint(28,48);
  public void BuildHarbor(int seed){foreach(var step in HarborBuildSteps2721(seed))step.Build();}
  void Fence(string name,float x,float z,float w,float d){Box(name,new Vector3(x,1.1f,z),new Vector3(w,2.2f,d),"BBC5C4",true);}
  GameObject Block(string name,float x,float z,float w,float d,float h){var g=Box(name,new Vector3(x,h/2,z),new Vector3(w,h,d),"D6DBD6",true);ExteriorBuildings.Add(new Rect(x-w/2,z-d/2,w,d));return g;}
 }
 public sealed class HarborWhiteboxSession:MonoBehaviour {
  System.Collections.IEnumerator Start(){var g=RaidGame.Instance;g.HarborWalkthrough=true;yield return g.BeginTrainingLoad2721();if(!g.TrainingActive||g.IsLoading2721){Destroy(this);yield break;}if(System.Array.IndexOf(System.Environment.GetCommandLineArgs(),"--dock-preview")>=0){g.Player.transform.position=RaidMap.DockPreviewSpawn141;g.Rig.ResetView(225);g.Rig.Tick(0);}
   if(System.Array.IndexOf(System.Environment.GetCommandLineArgs(),"--yard-preview")>=0){g.Player.transform.position=RaidMap.PortPoint(28,42);g.Rig.ResetView(225);g.Rig.Tick(0);}var args=System.Environment.GetCommandLineArgs();if(System.Array.IndexOf(args,"--repair-preview")>=0){g.Player.transform.position=RaidMap.RepairPreviewSpawn149;g.Rig.ResetView(45);g.Rig.Tick(0);}if(System.Array.IndexOf(args,"--yard-home-preview")>=0||System.Array.IndexOf(args,"--yard-shed-preview")>=0){bool home=System.Array.IndexOf(args,"--yard-home-preview")>=0;g.Player.transform.position=home?RaidMap.YardHomeApproach138:RaidMap.YardShedApproach138;g.Rig.ResetView(225);g.Rig.Tick(0);g.SwitchInterior(home?"yardhome":"yardshed");}if(System.Array.IndexOf(args,"--cargo-preview")>=0)CargoPreview2712.Apply(g);if(System.Array.IndexOf(args,"--freight-preview")>=0)FreightPreview2716.Apply(g);if(System.Array.IndexOf(args,"--unloading-preview")>=0)UnloadingPreview2723.Apply(g);if(System.Array.IndexOf(args,"--office-preview")>=0)OfficePreview2726.Apply(g);if(System.Array.IndexOf(args,"--surface-hatch-preview")>=0){var hatch=g.Map.UndergroundSurfaceEntries2752.Find(p=>p.Destination=="underground-enter:south");g.Player.transform.position=hatch.Position+Vector3.back*1.1f;g.Rig.ResetView(45);g.Rig.Tick(0);}if(System.Array.IndexOf(args,"--underground-preview")>=0){var access=g.Map.UndergroundSurfaceEntries2752.Find(p=>p.Destination=="underground-enter:south");g.Player.transform.position=access.Position;g.Rig.ResetView(45);g.Rig.Tick(0);g.SwitchInterior(access.Destination);}Destroy(this);}
 }
 public sealed partial class RaidUI {
  bool whiteboxOverview;
  void WhiteboxGuide(){
   if(OfficeGuide2726())return;if(UnloadingGuide2723())return;if(FreightOutdoorGuide2716())return;if(CargoGuide2712())return;
   int area=0;float best=float.MaxValue;for(int i=0;i<G.Map.RegionAnchors.Count;i++){float d=(G.Player.transform.position-G.Map.RegionAnchors[i]).sqrMagnitude;if(d<best){area=i;best=d;}}
   if(RaidMap.RepairBounds149.Contains(new Vector2(G.Player.transform.position.x,G.Player.transform.position.z)))area=1;if(RaidMap.LogisticsYardBounds.Contains(new Vector2(G.Player.transform.position.x,G.Player.transform.position.z)))area=6;Fill(new Rect(28,109,353,76),panel);Label(48,118,320,24,(new Rect(105,-6,18,42).Contains(new Vector2(G.Player.transform.position.x,G.Player.transform.position.z))?"客船 · 露天甲板":RaidMap.WhiteboxNames[area]),18,mint);Label(48,145,320,20,"M全图 · 维修主楼四层 / 附属房可进入",13,muted);
  }
  void WhiteboxOverlay(){if(whiteboxOverview)DrawOverview318();}
  Vector2 WhitePoint(Vector3 p,Rect r)=>new Vector2(r.x+(p.x-G.Map.Bounds.xMin)/G.Map.Bounds.width*r.width,r.y+(G.Map.Bounds.yMax-p.z)/G.Map.Bounds.height*r.height);
  Rect WhiteRect(Rect b,Rect r)=>new Rect(r.x+(b.x-G.Map.Bounds.xMin)/G.Map.Bounds.width*r.width,r.y+(G.Map.Bounds.yMax-b.yMax)/G.Map.Bounds.height*r.height,b.width/G.Map.Bounds.width*r.width,b.height/G.Map.Bounds.height*r.height);
 }
}






