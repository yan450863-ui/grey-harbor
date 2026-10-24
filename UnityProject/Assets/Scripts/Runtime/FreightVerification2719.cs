using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace GreyHarbor {
 public sealed partial class RaidVerification {
  IEnumerator RunFreightRepair2719(){
   Check(Application.version=="2.7.19"&&Path.GetFileName(G.SavePath)=="verification-stash-2.7.19.json","2719 isolated current-version save");
   string dir=Path.GetDirectoryName(G.SavePath);string[] formal={"stash-v40.json","stash-v41.json","stash-v42.json"};byte[][] bytes=Array.ConvertAll(formal,n=>File.Exists(Path.Combine(dir,n))?File.ReadAllBytes(Path.Combine(dir,n)):null);
   var controls=G.Controls;var random=UnityEngine.Random.state;FreshCamp();string inventory=JsonUtility.ToJson(G.Save);G.HarborWalkthrough=true;Check(G.BeginTraining(),"2719 actual harbor preview starts");G.Paused=true;G.Controls=new TouchInput();yield return null;
   try{
    Check(FreightPreview2716.Apply(G),"2719 freight preview anchor is safe");
    bool followup=Array.IndexOf(Environment.GetCommandLineArgs(),"--road-header-only-2719")>=0;
    if(!followup){FreightOperationsCheck2719();yield return FreightRoutesCheck2716();yield return FreightSearchCheck2716();}
    yield return FreightCargoLink2718();FreightRoadPixelsCheck2719();
    yield return CargoPicture2715("2719-freight-yard-plan.png",RaidMap.PortPoint(27.5f,155),0,23,true,1600,1600);
    yield return CargoPicture2715("2719-freight-yard-northeast.png",RaidMap.PortPoint(25,154),45,23,false,1800,1200);
    yield return FreightLiveImage2718("2719-outdoor-live",RaidMap.FreightPreviewSpawn2716,new Vector3(1,0,1),45);
    yield return FreightRooms2719();
    FreightPlace2716(RaidMap.FreightExitPoint2716,"2719 final extraction");G.ExtractTime=0;G.TickExtraction(5.05f);yield return null;
    Check(G.Phase==GamePhase.Base&&!G.TrainingActive&&JsonUtility.ToJson(G.Save)==inventory,"2719 preview extraction restores exact inventory");
    for(int i=0;i<formal.Length;i++)Check(MainBytes273(bytes[i],File.Exists(Path.Combine(dir,formal[i]))?File.ReadAllBytes(Path.Combine(dir,formal[i])):null),"2719 formal save unchanged "+formal[i]);
   }finally{G.Controls=controls;UnityEngine.Random.state=random;G.Paused=true;File.WriteAllLines(Path.Combine(folder,"2719-freight-actual.csv"),freightRows2716);File.WriteAllLines(Path.Combine(folder,"2719-freight-rooms.csv"),freightRoomRows2717);}
  }
  void FreightOperationsCheck2719(){
   var map=G.Map;Check(map.FreightSearchRecords2716.Count==4,"2719 four real outdoor searches preserved");
   Check(map.FreightClusters2717.Count==0&&map.Root.GetComponentsInChildren<FreightStockAppearance2717>().Length==0,"2719 rejected loose roadside stock generator is absent");
   var operations=map.Root.GetComponentsInChildren<FreightOperation2719>();Check(operations.Length==7,"2719 all seven functional working bays successfully placed count="+operations.Length);
   foreach(var op in operations){Check(!string.IsNullOrEmpty(op.Purpose)&&op.Units>0&&op.GetComponentsInChildren<Collider>().Length>0,"2719 working bay has actual equipment, purpose and collision "+op.Equipment);
    float floor=float.MaxValue;foreach(var renderer in op.GetComponentsInChildren<Renderer>())if(renderer.enabled)floor=Mathf.Min(floor,renderer.bounds.min.y);Check(Mathf.Abs(floor-.055f)<.009f,"2719 working equipment rests on the final ground plane "+op.Equipment);
   }
   var groups=new HashSet<GameObject>();var palette=new HashSet<string>();int overlaps=0;
   var all=map.Root.GetComponentsInChildren<Collider>();
   foreach(var r in map.FreightExteriorRecords2716){
    Check(r.View&&groups.Add(r.View),"2719 registered visible distinct yard group "+r.Type);FreightRow2716("functional-yard",r.Type,r.View.transform.position,r.Footprint.ToString());
    foreach(var renderer in r.View.GetComponentsInChildren<Renderer>())foreach(var mat in renderer.sharedMaterials)if(mat)palette.Add(ColorUtility.ToHtmlStringRGB(mat.color));
    foreach(var c in r.View.GetComponentsInChildren<Collider>())if(c.enabled&&!c.isTrigger&&(SceneObstacle.MovementMask&(1<<c.gameObject.layer))!=0)foreach(var other in all){if(!other.enabled||other.isTrigger||other.transform.IsChildOf(r.View.transform)||(SceneObstacle.MovementMask&(1<<other.gameObject.layer))==0||!c.bounds.Intersects(other.bounds))continue;Vector3 d;float distance;if(Physics.ComputePenetration(c,c.transform.position,c.transform.rotation,other,other.transform.position,other.transform.rotation,out d,out distance)&&distance>.015f){overlaps++;Debug.Log("2719 yard overlap "+r.Type+" "+c.name+" / "+other.name+" "+distance);}}
   }
   Check(overlaps==0,"2719 functional scenery has no real collider overlap count="+overlaps);Check(palette.Count>8,"2719 functional yard materials retain varied colors count="+palette.Count);
   Check(map.FreightTruck2716&&map.FreightGate2716&&Vector3.Dot(map.FreightTruck2716.transform.up,Vector3.up)<.65f,"2719 gate and overturned cargo truck remain");
   Check(CargoClear2713(RaidMap.FreightExitPoint2716)&&map.Exits.Contains(RaidMap.FreightExitPoint2716),"2719 truck gate extraction remains accessible");
  }
  void FreightRoadPixelsCheck2719(){
   var rows=new List<string>{"designX,designZ,material,sampledRGB"};
   foreach(var p in new[]{new Vector2(6,130.3f),new Vector2(6,134),new Vector2(11,137),new Vector2(20,137),new Vector2(36,137),new Vector2(50,137),new Vector2(54,137),new Vector2(80,137),new Vector2(98,137),new Vector2(17,145)}){
    Color c=ConnectionPixel2717(RaidMap.PortPoint(p.x,p.y));rows.Add(p.x+","+p.y+",asphalt,#"+ColorUtility.ToHtmlStringRGB(c));Check(c.a>.98f&&c.grayscale>.23f&&c.grayscale<.40f,"2719 actual connecting road is aged repair-palette asphalt "+p+" #"+ColorUtility.ToHtmlStringRGB(c));
   }
   foreach(var p in new[]{new Vector2(17,161),new Vector2(17,167),new Vector2(17,170)}){
    Color c=ConnectionPixel2717(RaidMap.PortPoint(p.x,p.y));rows.Add(p.x+","+p.y+",concrete apron,#"+ColorUtility.ToHtmlStringRGB(c));Check(c.a>.98f&&c.grayscale>.5f&&RaidMap.FreightAsphaltWeight2719(new Vector2(p.x-79,p.y-73))<.01f,"2719 internal road cap becomes continuous concrete working apron "+p);
   }
   foreach(var x in new[]{49.9f,50.1f,50.9f,51.1f}){Color a=ConnectionPixel2717(RaidMap.PortPoint(x,137)),b=ConnectionPixel2717(RaidMap.PortPoint(x+.15f,137));Check(Mathf.Abs(a.grayscale-b.grayscale)<.10f,"2719 east connection material seam is smooth "+x);}
   File.WriteAllLines(Path.Combine(folder,"2719-road-material-samples.csv"),rows);
   Check(G.Map.StableGround137.mipmapCount>1,"2719 road/apron finishes remain in a single filtered stable ground texture");
  }
 }
}
