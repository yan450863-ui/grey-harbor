using System;
using System.IO;
using System.Collections;
using UnityEngine;
namespace GreyHarbor {
 public sealed partial class RaidVerification {
  IEnumerator RunUnloading2723(){
   Check(Application.version=="2.7.23"&&G.VerifyMode&&G.SavePath.EndsWith("verification-stash-2.7.23.json"),"2723 isolated current-version whitebox verification");
   var input=G.Controls;string dir=Path.GetDirectoryName(G.SavePath);string[] formal={"stash-v40.json","stash-v41.json","stash-v42.json"};byte[][] before=Array.ConvertAll(formal,n=>File.Exists(Path.Combine(dir,n))?File.ReadAllBytes(Path.Combine(dir,n)):null);
   try{
    FreshCamp();string saved=JsonUtility.ToJson(G.Save);G.Controls=new TouchInput();G.HarborWalkthrough=true;yield return G.BeginTrainingLoad2721();G.Paused=true;
    Check(G.Map.HarborWhitebox&&G.Map.GroundCacheUsed2721&&G.Map.HarborPlanCacheUsed2721,"2723 changed building/road geometry uses regenerated ground and minimap caches");
    var outdoor=G.Map;var actors=G.Enemies.ToArray();
    Check(RaidMap.WhiteboxNames[3]=="办公楼"&&RaidMap.WhiteboxNames[4]=="装卸区"&&HarborContent.DistrictNames[3]=="办公楼"&&HarborContent.DistrictNames[4]=="装卸区","2723 district names swapped consistently");
    Check(outdoor.MainBuildings.Count==3&&outdoor.MainBuildings[1].Name=="办公楼"&&outdoor.MainBuildings[1].Floors==4&&outdoor.MainBuildings[2].Name=="装卸仓库"&&outdoor.MainBuildings[2].Floors==1,"2723 large office reserve and single-storey warehouse replace former loading/office blocks");
    Check(!outdoor.Root.Find("码头办公室主楼")&&!outdoor.Root.Find("办公室附属房")&&!outdoor.Root.Find("办公室设备房"),"2723 old small-area building blocks removed completely");
    Check(outdoor.BuildingPlans2723.Count==2&&outdoor.PlaceholderRecords2723.Count==4&&G.Enemies.Count==30&&outdoor.Loot.Count==54,"2723 whitebox adds only two buildings and four occupancy blocks, preserving prior population and loot");
    Check(G.Enemies.FindAll(e=>RaidMap.UnloadingBounds2723.Contains(new Vector2(e.Home.x,e.Home.z))).Count==0,"2723 new loading district has no monsters");
    Check(UnloadingPreview2723.Apply(G),"2723 outdoor preview arrival capsule clear");
    foreach(var plan in outdoor.BuildingPlans2723){Check(CargoClear2713(plan.Arrival)&&outdoor.OnHarborSurface(plan.Arrival),"2723 real entry arrival clear on land "+plan.Key);}
    for(float z=137;z<=158;z+=1){var center=RaidMap.PortPoint(128,z);Check(CargoClear2713(center)&&CargoClear2713(center+Vector3.left*2.5f)&&CargoClear2713(center+Vector3.right*2.5f),"2723 continuous six-metre vehicle approach capsule check "+z);}
    var preview=RaidMap.UnloadingPreviewSpawn2723;G.Player.transform.position=RaidMap.PortPoint(128,137);Physics.SyncTransforms();FreightRoomWalk2717(preview,"2723 existing main road to outdoor work apron");
    yield return CargoPicture2715("2723-unloading-outdoor-overhead.png",RaidMap.PortPoint(131,157),0,21,true,1600,1600);
    yield return CargoPicture2715("2723-unloading-outdoor-oblique.png",RaidMap.PortPoint(131,157),45,23,false,1600,1100);
    RaidMap warehouse=null,duty=null;
    foreach(string key in new[]{"unloadingwarehouse","unloadingduty"}){
     var arrival=key=="unloadingwarehouse"?RaidMap.UnloadingWarehouseArrival2723:RaidMap.UnloadingDutyArrival2723;FreightRoomWalk2717(arrival,"2723 actual exterior entry approach "+key);
     FreightMonsterF2720(outdoor.Doors.Find(d=>d.Portal&&d.Destination==key),true);yield return null;
     Check(G.Map.InteriorId==key&&G.Map.UnloadingInterior2723&&G.Map.IsInterior&&G.Enemies.Count==0&&G.Map.Loot.Count==0,"2723 real held F enters correct empty whitebox "+key);
     Check(Array.TrueForAll(actors,e=>!e.gameObject.activeInHierarchy)&&CargoClear2713(G.Player.transform.position),"2723 exterior actors suspend and interior spawn is physically clear");
     if(key=="unloadingwarehouse"){
      warehouse=G.Map;Check(Mathf.Abs(G.Map.Bounds.width-22.4f)<.01f&&Mathf.Abs(G.Map.Bounds.height-14.4f)<.01f&&G.Map.UnloadingCeilingHeight2723==4.5f,"2723 warehouse footprint and height match exterior");
      foreach(var p in new[]{new Vector3(0,0,4.8f),new Vector3(-8,0,0),new Vector3(8,0,0),G.Map.Spawn})FreightRoomWalk2717(p,"2723 actual warehouse circulation");
      Check(!G.Navigation.ClearSegment(new Vector3(0,0,0),new Vector3(0,0,8)),"2723 outer warehouse wall blocks walking outside shell");
     }else{
      duty=G.Map;Check(Mathf.Abs(G.Map.Bounds.width-8.4f)<.01f&&Mathf.Abs(G.Map.Bounds.height-6.4f)<.01f,"2723 duty footprint matches exterior");
      var door=G.Map.UnloadingStoreDoor2723;var near=door.Position-Vector3.right*.65f;var far=door.Position+Vector3.right*.8f;
      Check(!door.Open&&!G.Vision.RoomAllowed(far)&&!G.Navigation.ClearSegment(near,far),"2723 unknown storage room starts closed and solid");FreightRoomWalk2717(near,"2723 duty door approach");FreightMonsterF2720(door,false);
      Check(door.Open&&G.Vision.RoomAllowed(far)&&G.Navigation.ClearSegment(near,far),"2723 actual F opens storage door and reveals room");FreightRoomWalk2717(new Vector3(2.5f,0,1.9f),"2723 enter actual storage room");FreightRoomWalk2717(near,"2723 storage return through opening");
      var header=G.Map.Root.Find("UnloadingWhiteboxDoorHeader2723");Check(header&&header.GetComponent<Collider>().bounds.min.y>=2.14f&&header.GetComponent<Collider>().bounds.max.y>=2.99f,"2723 actual door header joins wall height without blocking player capsule");
     }
     yield return CargoPicture2715("2723-"+key+"-interior-oblique.png",Vector3.zero,45,key=="unloadingwarehouse"?13:5.8f,false,1600,1100);
     FreightRoomWalk2717(G.Map.Spawn,"2723 actual interior exit approach");FreightMonsterF2720(G.Map.UnloadingExit2723,true);yield return null;
     Check(G.Map==outdoor&&Vector3.Distance(G.Player.transform.position,arrival)<.01f&&G.Enemies.Count==30&&Array.TrueForAll(actors,e=>G.Enemies.Contains(e)),"2723 return uses corresponding exterior arrival and preserves all actors");
    }
    G.SwitchInterior("unloadingduty");yield return null;Check(G.Map==duty&&G.Map.UnloadingStoreDoor2723.Open,"2723 duty geometry, door and exploration persist on revisit");G.SwitchInterior("unloadingduty");yield return null;
    G.SwitchInterior("unloadingwarehouse");yield return null;Check(G.Map==warehouse&&G.Map.Loot.Count==0&&G.Enemies.Count==0,"2723 warehouse revisit preserves same empty map without duplicate generation");G.SwitchInterior("unloadingwarehouse");yield return null;
    G.Finish(false);yield return null;Check(G.Phase==GamePhase.Base&&JsonUtility.ToJson(G.Save)==saved,"2723 whitebox preview returns exact camp inventory");
    for(int i=0;i<formal.Length;i++)Check(MainBytes273(before[i],File.Exists(Path.Combine(dir,formal[i]))?File.ReadAllBytes(Path.Combine(dir,formal[i])):null),"2723 formal-save bytes unchanged "+formal[i]);
   }finally{G.Controls=input;G.Paused=true;}
  }
 }
}
