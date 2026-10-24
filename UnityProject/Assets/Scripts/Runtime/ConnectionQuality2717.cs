using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
namespace GreyHarbor {
 public sealed partial class RaidVerification {
  readonly List<string> connectionRows2717=new List<string>();
  void ConnectionRow2717(string type,string name,Vector3 p,string detail){connectionRows2717.Add(type+","+name.Replace(",",";")+","+p.x.ToString("F4",CultureInfo.InvariantCulture)+","+p.z.ToString("F4",CultureInfo.InvariantCulture)+",\""+detail.Replace("\"","'")+"\"");}
  Color ConnectionPixel2717(Vector3 p){var b=G.Map.StableGroundBounds137;return G.Map.StableGround137.GetPixelBilinear((p.x-b.xMin)/b.width,(p.z-b.yMin)/b.height);}
  IEnumerator RunConnectionQuality2717(){
   var map=G.Map;Check(map!=null&&map.HarborWhitebox&&!map.IsInterior,"2717 Connection scope uses actual completed harbor exterior");
   connectionRows2717.Clear();connectionRows2717.Add("type,name,worldX,worldZ,detail");Vector3 savedPosition=G.Player.transform.position;Vector3 savedAim=G.Player.Aim;
   try{
    Check(map.Bounds==new Rect(-79,-73,208,180),"2717 Road smoothing retains exact existing playable harbor bounds");
    var plane=map.Root.Find("Harbor_StableGround137");Check(plane&&map.StableGround137&&plane.GetComponentsInChildren<Collider>().Length==0,"2717 Completed areas and roads share actual stable noncolliding floor");
    var filter=plane.GetComponent<MeshFilter>();var material=plane.GetComponent<Renderer>().sharedMaterial;
    Check(filter.sharedMesh.vertexCount==4&&material.GetTexture("_MainTex")==map.StableGround137&&material.GetFloat("_UseStableGround")>.5f,"2717 Real world-fixed baked texture is bound to the single presentation plane");
    int enabled=0;foreach(var renderer in map.HarborBaseRenderers2717)if(renderer&&renderer.enabled)enabled++;
    Check(map.HarborBaseRenderers2717.Count>=6&&enabled==0,"2717 Original whitebox base renderers replaced without layered flat surfaces");
    int rawRoads=0;foreach(var r in map.Root.GetComponentsInChildren<Renderer>())if(r.enabled&&r.name=="Road")rawRoads++;
    Check(rawRoads==0&&map.HarborRoadRecords2717.Count==11,"2717 Eleven original road centerlines preserved without independent overlapping road cubes");
    int sampled=0;bool coverage=true,roadsOnLand=true;
    foreach(var road in map.HarborRoadRecords2717){
     Check(road.Width>=3&&road.Points.Length>=2,"2717 Actual retained road width and centerline "+road.Name);
     for(int i=1;i<road.Points.Length;i++){
      var a=road.Points[i-1];var b=road.Points[i];var direction=(b-a).normalized;var normal=new Vector3(-direction.z,0,direction.x);int steps=Mathf.CeilToInt(Vector3.Distance(a,b)*2);
      for(int k=0;k<=steps;k++){var center=Vector3.Lerp(a,b,k/(float)Mathf.Max(1,steps));roadsOnLand&=map.OnHarborSurface(center);
       foreach(float offset in new[]{0f,-road.Width*.40f,road.Width*.40f}){var p=center+normal*offset;if(!map.HarborSurfaceClear(p,.18f))continue;Color pixel=ConnectionPixel2717(p);sampled++;coverage&=pixel.a>.98f;}
      }
      ConnectionRow2717("retained-road",road.Name+"/"+i,a,"width="+road.Width.ToString(CultureInfo.InvariantCulture)+" end="+b);
     }
     yield return null;
    }
    Check(roadsOnLand&&coverage&&sampled>300,"2717 Existing road centers and usable width retain legal land and opaque baked coverage; samples="+sampled);
    int waterSamples=0,landSamples=0;bool waterClear=true,landCovered=true;
    var bounds=map.StableGroundBounds137;
    // Independent grid of actual texture pixels. Boundary filtering is excluded with a 25cm geometric margin.
    for(float z=bounds.yMin+.6f;z<bounds.yMax;z+=1.3f)for(float x=bounds.xMin+.6f;x<bounds.xMax;x+=1.3f){var p=new Vector3(x,0,z);
     if(map.HarborSurfaceClear(p,.25f)){landSamples++;landCovered&=ConnectionPixel2717(p).a>.98f;}
     else if(!map.OnHarborSurface(p)){bool far=true;for(int n=0;n<8;n++){float angle=n*Mathf.PI/4;far&=!map.OnHarborSurface(p+new Vector3(Mathf.Cos(angle),0,Mathf.Sin(angle))*.25f);}if(far){waterSamples++;waterClear&=ConnectionPixel2717(p).a<.025f;}}
    }
    Check(landCovered&&landSamples>500,"2717 Completed mainland/platform union has no exposed unpainted whitebox holes; samples="+landSamples);
    Check(waterClear&&waterSamples>100,"2717 Actual stable paint clips to original land rather than forming road tongues over water; samples="+waterSamples);
    Check(map.HarborConnections2717.Count==12,"2717 Entrance, logistics, repair, cargo, freight and dock bridge connections registered");
    foreach(var connection in map.HarborConnections2717){
     var first=connection.Points[0];Physics.SyncTransforms();if(!CargoClear2713(first)){foreach(var hit in Physics.OverlapCapsule(first+Vector3.up*.35f,first+Vector3.up,.34f,SceneObstacle.MovementMask,QueryTriggerInteraction.Ignore))Debug.Log("CONNECTION2717 BLOCK "+connection.Name+" "+hit.name+" bounds="+hit.bounds);Debug.Log("CONNECTION2717 LAND "+map.OnHarborSurface(first));}Check(CargoClear2713(first),"2717 Actual .34m start clearance "+connection.Name+" "+first);
     G.Player.transform.position=first;Physics.SyncTransforms();ConnectionRow2717("fixture-start",connection.Name,first,"independent setup; route uses real Actor.Move");
     for(int i=1;i<connection.Points.Length;i++){
      Vector3 goal=connection.Points[i];bool walked=WalkMain273(goal);ConnectionRow2717("actual-walk",connection.Name+"/"+i,G.Player.transform.position,walked?"PASS .34m swept player":"FAIL "+mainWalkFailure273);
      Check(walked&&CargoClear2713(goal),"2717 Real connected-region player traversal "+connection.Name+"/"+i+" "+mainWalkFailure273);
     }
     // The named bridges are existing 4m land strips: check actual ground across their usable width, not an invented wider road.
     if(connection.Name.Contains("陆桥")){var a=connection.Points[0];var b=connection.Points[connection.Points.Length-1];for(int i=0;i<=24;i++){var center=Vector3.Lerp(a,b,i/24f);foreach(float offset in new[]{-1.6f,0f,1.6f})Check(map.OnHarborSurface(center+Vector3.right*offset)&&ConnectionPixel2717(center+Vector3.right*offset).a>.98f,"2717 Existing legal dock bridge width has continuous floor "+connection.Name+"/"+i+"/"+offset);}}
     yield return null;
    }
    yield return CargoPicture2715("2717-connections-full-port-overhead.png",RaidMap.PortPoint(104,90),0,108,true,2048,2048);
    yield return CargoPicture2715("2717-connections-entrance-yard-repair.png",RaidMap.PortPoint(48,54),0,32,true,1800,1500);
    yield return CargoPicture2715("2717-connections-cargo-freight.png",RaidMap.PortPoint(76,131),0,48,true,1800,1500);
    yield return CargoPicture2715("2717-connections-dock-bridges.png",RaidMap.PortPoint(153,88),0,49,true,1500,1800);
    yield return CargoPicture2715("2717-connections-natural-road-oblique.png",RaidMap.PortPoint(51,94),35,22,false,1800,1200);
   }finally{
    G.Player.transform.position=savedPosition;G.Player.Aim=savedAim;Physics.SyncTransforms();G.Rig.Tick(0);G.Vision.Refresh(savedPosition,savedAim);
    File.WriteAllLines(Path.Combine(folder,"2717-completed-region-connections.csv"),connectionRows2717);
   }
  }
 }
}
