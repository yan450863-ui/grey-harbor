using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace GreyHarbor {
 public sealed partial class RaidVerification {
  void UnloadingPalette2724(IEnumerable<GameObject> views,string label){
   var colors=new HashSet<string>();bool warm=false,green=false,blue=false;int visible=0;
   foreach(var view in views){Check(view!=null,"2724 actual dressing model exists "+label);if(!view)continue;
    foreach(var renderer in view.GetComponentsInChildren<Renderer>())if(renderer.enabled){visible++;
     foreach(var mat in renderer.sharedMaterials){bool valid=mat&&mat.shader&&(mat.shader.name=="GreyHarbor/World"||mat.shader.name=="GreyHarbor/FreightWall2718");Check(valid,"2724 production material retains world fog "+label+"/"+renderer.name);if(!mat)continue;
      Color c=mat.color;colors.Add(ColorUtility.ToHtmlStringRGB(c));warm|=c.r>c.b+.065f&&c.r>c.g+.02f;green|=c.g>c.r+.025f&&c.g>c.b+.015f;blue|=c.b>c.r+.035f;
     }
    }
   }
   Check(visible>0&&colors.Count>=8&&warm&&green&&blue,"2724 actual models use wood/cargo browns, painted green and blue rather than all grey "+label+" colors="+colors.Count);
  }
  void UnloadingLiveImage2724(Texture2D image,string label){
   // Small interiors legitimately occupy less than 10% of the unchanged game camera.
   // Exclude black space outside the scene when measuring content contrast.
   var pixels=image.GetPixels32();var histogram=new int[256];int lit=0,white=0;double sum=0;
   foreach(var p in pixels){float lum=(.2126f*p.r+.7152f*p.g+.0722f*p.b)/255f;if(lum<=.04f)continue;lit++;sum+=lum;histogram[Mathf.Clamp(Mathf.RoundToInt(lum*255),0,255)]++;if(p.r>=248&&p.g>=248&&p.b>=248)white++;}
   int total=0,low=0,high=0;bool first=false;for(int i=0;i<256;i++){total+=histogram[i];if(!first&&total>=lit*.1f){low=i;first=true;}if(total>=lit*.9f){high=i;break;}}
   float mean=lit>0?(float)(sum/lit):0,clip=lit>0?white/(float)lit:1,contrast=(high-low)/255f;
   Check(lit>pixels.Length*.005f&&mean<.90f&&clip<.08f&&contrast>.06f,"2724 production camera contains visible unclipped room geometry "+label+" pixels="+lit+" mean="+mean+" white="+clip+" contrast="+contrast);
  }
  void UnloadingWallPixels2724(string label,Vector3 face,Vector3 inward,bool lit){
   Check(Shader.GetGlobalFloat("_HarborFogEnabled")>.5f&&G.Vision.RoomAllowed(face+inward*.30f)==lit,"2724 wall pixel sample uses production fog and actual room reveal "+label);
   MeshRenderer wall=null;
   foreach(var renderer in G.Map.Root.GetComponentsInChildren<MeshRenderer>())if(renderer.name=="UnloadingWhiteboxWall2723"){
    var bounds=renderer.bounds;bounds.Expand(.06f);if(bounds.Contains(face+Vector3.up*1.65f)){wall=renderer;break;}
   }
   Check(wall!=null,"2724 actual structural wall located for GPU probe "+label);
   if(wall)FreightWallRendererProbe2718(wall,"2724-"+label,face+Vector3.up*1.65f,inward,lit);
  }
  IEnumerator UnloadingSearch2724(int expected){yield return UnloadingSearchRecords2724(G.Map.UnloadingSearchRecords2724,expected,expected);}
  IEnumerator UnloadingSearchRecords2724(List<FreightLootRecord2717> records,int expected,int total){
   var map=G.Map;Check(map.Loot.Count==total&&records.Count==expected,"2724 exact real searchable container count "+map.InteriorId+"="+expected);var unique=new HashSet<LootContainer>();
   foreach(var r in records){var box=r.Container;
    Check(box!=null&&unique.Add(box)&&map.Loot.Contains(box)&&box.DropProfile230!=null&&box.DropProfile230.Tier==ContainerTier.Common&&box.DropProfile230.FixedCount==0,"2724 container keeps existing ordinary drop rules "+(box==null?"null":box.Name));
    if(box==null)continue;
    Check(r.Model&&box.Prop&&box.View==r.Model&&Vector3.Distance(box.Position,r.Stand)<.001f,"2724 recorded container is actual searchable model "+box.Name);
    FreightRoomWalk2717(r.AccessPoint,"2724 approach "+box.Name);Check(CargoClear2713(r.Stand)&&G.Navigation.ClearSegment(r.AccessPoint,r.Stand),"2724 real approach and search capsule clear "+box.Name);FreightRoomWalk2717(r.Stand,"2724 search stand "+box.Name);
    G.Rig.Tick(0);G.Player.Aim=(box.Prop.transform.position-G.Player.transform.position).normalized;G.Vision.Refresh(G.Player.transform.position,G.Player.Aim);Check(G.CanInteractLoot(box),"2724 actual visibility and interaction range "+box.Name);
    int count=box.Items.Count;string original=FreightMonsterItems2720(box);SprintInteract2711(false,0,box.Prop.transform.position);SprintInteract2711(false,0,box.Prop.transform.position);Check(G.Nearby==box,"2724 actual F chooses this container "+box.Name);
    SprintInteract2711(true,box.SearchDuration+.02f,box.Prop.transform.position);Check(box.Opened&&G.OpenLoot==box&&box.Prop.Searched&&box.Items.Count==count&&FreightMonsterItems2720(box)==original,"2724 real held F searches without rerolling contents "+box.Name);
    G.Paused=false;for(int i=0;i<7;i++)G.TickIdentification(3);G.Paused=true;Check(box.Items.Count==count&&box.Items.TrueForAll(item=>item.Identified),"2724 production item identification completes "+box.Name);G.CloseInventory();SprintInteract2711(false,0,box.Prop.transform.position);FreightRoomWalk2717(r.AccessPoint,"2724 leave searched box "+box.Name);yield return null;
   }
  }
  IEnumerator RunUnloading2724(){
   Check(Application.version=="2.7.24"&&G.VerifyMode&&G.SavePath.EndsWith("verification-stash-2.7.24.json"),"2724 isolated current-version unloading dressing verification");
   var input=G.Controls;string dir=Path.GetDirectoryName(G.SavePath);string[] formal={"stash-v40.json","stash-v41.json","stash-v42.json"};byte[][] before=Array.ConvertAll(formal,n=>File.Exists(Path.Combine(dir,n))?File.ReadAllBytes(Path.Combine(dir,n)):null);
   try{
    FreshCamp();string saved=JsonUtility.ToJson(G.Save);G.Controls=new TouchInput();G.HarborWalkthrough=true;yield return G.BeginTrainingLoad2721();G.Paused=true;
    var outdoor=G.Map;var actors=G.Enemies.ToArray();Check(outdoor.HarborWhitebox&&outdoor.GroundCacheUsed2721&&outdoor.HarborPlanCacheUsed2721,"2724 changed district loads current ground/minimap caches");
    Check(outdoor.BuildingPlans2723.Count==2&&G.Enemies.Count==30&&outdoor.Loot.Count==57,"2724 two unloading buildings preserve exterior actor and loot populations");
    Check(outdoor.MainBuildings.Count==3&&outdoor.MainBuildings[1].Name=="办公楼"&&outdoor.MainBuildings[2].Name=="装卸仓库"&&outdoor.MainBuildings[2].Floors==1,"2724 existing office reserve and single-storey warehouse preserved");
    Check(G.Enemies.FindAll(e=>RaidMap.UnloadingBounds2723.Contains(new Vector2(e.Home.x,e.Home.z))).Count==0,"2724 unloading district still has no monsters");
    Check(outdoor.UnloadingExteriorRecords2724.Count>0,"2724 actual outdoor loading equipment dressing exists");var outsideViews=new List<GameObject>();foreach(var r in outdoor.UnloadingExteriorRecords2724)outsideViews.Add(r.View);UnloadingPalette2724(outsideViews,"outdoor");
    Check(UnloadingPreview2723.Apply(G),"2724 outdoor preview arrival physically clear");foreach(var p in outdoor.BuildingPlans2723)Check(CargoClear2713(p.Arrival)&&outdoor.OnHarborSurface(p.Arrival),"2724 corresponding building arrival clear on land "+p.Key);
    for(float z=137;z<=158;z+=1){var center=RaidMap.PortPoint(128,z);Check(CargoClear2713(center)&&CargoClear2713(center+Vector3.left*2.5f)&&CargoClear2713(center+Vector3.right*2.5f),"2724 existing six-metre vehicle approach unobstructed "+z);}
    G.Player.transform.position=RaidMap.PortPoint(128,137);Physics.SyncTransforms();FreightRoomWalk2717(RaidMap.UnloadingPreviewSpawn2723,"2724 real main-road to loading apron movement");
    yield return UnloadingSearchRecords2724(outdoor.UnloadingOutdoorSearch2724,3,57);var outsideBoxes=outdoor.UnloadingOutdoorSearch2724.ConvertAll(r=>r.Container).ToArray();var outsideContents=Array.ConvertAll(outsideBoxes,box=>FreightMonsterItems2720(box));
    yield return CargoPicture2715("2724-unloading-outdoor-overhead.png",RaidMap.PortPoint(131,157),0,21,true,1600,1600);yield return CargoPicture2715("2724-unloading-outdoor-oblique.png",RaidMap.PortPoint(131,157),45,23,false,1600,1100);
    var maps=new List<RaidMap>();var contents=new List<string[]>();var visions=new List<RaidVision>();var memories=new List<bool[]>();
    foreach(string key in new[]{"unloadingwarehouse","unloadingduty"}){
     bool warehouse=key=="unloadingwarehouse";var arrival=warehouse?RaidMap.UnloadingWarehouseArrival2723:RaidMap.UnloadingDutyArrival2723;FreightRoomWalk2717(arrival,"2724 actual exterior entry "+key);FreightMonsterF2720(outdoor.Doors.Find(d=>d.Portal&&d.Destination==key),true);yield return null;
     var indoor=G.Map;Check(indoor.InteriorId==key&&indoor.UnloadingInterior2723&&indoor.IsInterior&&G.Enemies.Count==0,"2724 real held F enters correct decorated interior without monsters "+key);Check(Array.TrueForAll(actors,e=>!e.gameObject.activeInHierarchy)&&CargoClear2713(G.Player.transform.position),"2724 exterior actors suspend and interior arrival is clear");
     Check(Mathf.Abs(indoor.Bounds.width-(warehouse?22.4f:8.4f))<.01f&&Mathf.Abs(indoor.Bounds.height-(warehouse?14.4f:6.4f))<.01f&&indoor.UnloadingCeilingHeight2723==(warehouse?4.5f:3),"2724 approved whitebox dimensions and heights preserved "+key);
     var roomViews=new List<GameObject>();foreach(var r in indoor.UnloadingDressingRecords2724)roomViews.Add(r.View);foreach(var r in indoor.UnloadingSearchRecords2724)roomViews.Add(r.Model);UnloadingPalette2724(roomViews,key);
     UnloadingWallPixels2724(key+"-visible",warehouse?new Vector3(0,0,6.9f):new Vector3(-2,0,2.9f),Vector3.back,true);
     if(warehouse){
      FreightRoomWalk2717(new Vector3(0,0,4.8f),"2724 real warehouse central handling aisle");Check(!G.Navigation.ClearSegment(Vector3.zero,new Vector3(0,0,8)),"2724 warehouse structural wall blocks exterior escape");
     }else{
      var door=indoor.UnloadingStoreDoor2723;var near=door.Position-Vector3.right*.65f;var far=door.Position+Vector3.right*.8f;
      Check(!door.Open&&!G.Vision.RoomAllowed(far)&&!G.Navigation.ClearSegment(near,far),"2724 closed storage door blocks movement and retains room black fog");UnloadingWallPixels2724("storage-closed",new Vector3(3.9f,0,0),Vector3.left,false);
      FreightRoomWalk2717(near,"2724 actual storage door approach");FreightMonsterF2720(door,false);Check(door.Open&&G.Vision.RoomAllowed(far)&&G.Navigation.ClearSegment(near,far),"2724 real F reveals storage room and opens clear passage");FreightRoomWalk2717(far,"2724 actual movement through open storage doorway");UnloadingWallPixels2724("storage-open",new Vector3(3.9f,0,0),Vector3.left,true);
      var header=indoor.Root.Find("UnloadingWhiteboxDoorHeader2723");Check(header&&header.GetComponent<Collider>().bounds.min.y>=2.14f&&header.GetComponent<Collider>().bounds.max.y>=2.99f,"2724 actual door header remains connected to wall above player capsule");
     }
     yield return UnloadingSearch2724(warehouse?6:2);
     FreightRoomWalk2717(indoor.Spawn,"2724 actual decorated room exit approach");G.Rig.Tick(0);G.Vision.Refresh(G.Player.transform.position,G.Player.Aim);G.Player.Draw(G.View,0);yield return null;
     float fog=Shader.GetGlobalFloat("_HarborFogEnabled");Check(fog>.5f,"2724 actual gameplay render retains fog "+key);var frame=HvRender277(G.View,"2724-"+key+"-live-fog.png",1600,900);try{UnloadingLiveImage2724(frame,key);}finally{Destroy(frame);}Check(Shader.GetGlobalFloat("_HarborFogEnabled")==fog,"2724 real camera capture preserves fog "+key);
     yield return CargoPicture2715("2724-"+key+"-interior-oblique.png",Vector3.zero,45,warehouse?13:5.8f,false,1600,1100);
     maps.Add(indoor);visions.Add(G.Vision);memories.Add((bool[])G.Vision.Seen.Clone());contents.Add(indoor.UnloadingSearchRecords2724.ConvertAll(r=>FreightMonsterItems2720(r.Container)).ToArray());FreightMonsterF2720(indoor.UnloadingExit2723,true);yield return null;
     Check(G.Map==outdoor&&Vector3.Distance(G.Player.transform.position,arrival)<.01f&&G.Enemies.Count==30&&Array.TrueForAll(actors,e=>G.Enemies.Contains(e)),"2724 corresponding exterior return preserves every actor "+key);
    }
    Check(maps[0]!=maps[1]&&visions[0]!=visions[1],"2724 buildings retain independent map and exploration caches");
    for(int m=0;m<maps.Count;m++){
     var indoor=maps[m];string key=indoor.InteriorId;var arrival=key=="unloadingwarehouse"?RaidMap.UnloadingWarehouseArrival2723:RaidMap.UnloadingDutyArrival2723;FreightRoomWalk2717(arrival,"2724 real cached entry approach "+key);FreightMonsterF2720(outdoor.Doors.Find(d=>d.Portal&&d.Destination==key),true);yield return null;
     Check(G.Map==indoor&&G.Vision==visions[m]&&G.Enemies.Count==0&&G.Map.Loot.Count==(m==0?6:2),"2724 crossing between buildings restores same interior without duplicate boxes "+key);if(key=="unloadingduty")Check(indoor.UnloadingStoreDoor2723.Open&&G.Vision.RoomAllowed(indoor.UnloadingStoreRoom2723.center.x*Vector3.right+indoor.UnloadingStoreRoom2723.center.y*Vector3.forward),"2724 storage door and reveal persist across buildings");
     bool seen=true;for(int i=0;i<memories[m].Length;i++)if(memories[m][i]&&!G.Vision.Seen[i])seen=false;Check(seen,"2724 exploration memory preserved "+key);
     for(int i=0;i<indoor.UnloadingSearchRecords2724.Count;i++){var box=indoor.UnloadingSearchRecords2724[i].Container;Check(box.Opened&&box.Prop.Searched&&box.Items.TrueForAll(item=>item.Identified)&&FreightMonsterItems2720(box)==contents[m][i],"2724 searched container identity, contents and identification persist "+box.Name);}
     FreightRoomWalk2717(indoor.Spawn,"2724 cached room real exit movement");FreightMonsterF2720(indoor.UnloadingExit2723,true);yield return null;Check(G.Map==outdoor&&G.Enemies.Count==30&&outdoor.Loot.Count==57,"2724 cached room returns original populated exterior");
    }
    for(int i=0;i<outsideBoxes.Length;i++)Check(outdoor.Loot.Contains(outsideBoxes[i])&&outsideBoxes[i].Opened&&outsideBoxes[i].Prop.Searched&&outsideBoxes[i].Items.TrueForAll(item=>item.Identified)&&outsideContents[i]==FreightMonsterItems2720(outsideBoxes[i]),"2724 exterior original box identity and contents persist across both interiors "+outsideBoxes[i].Name);
    G.Finish(false);yield return null;Check(G.Phase==GamePhase.Base&&JsonUtility.ToJson(G.Save)==saved,"2724 unloading preview restores exact camp inventory");for(int i=0;i<formal.Length;i++)Check(MainBytes273(before[i],File.Exists(Path.Combine(dir,formal[i]))?File.ReadAllBytes(Path.Combine(dir,formal[i])):null),"2724 formal save unchanged "+formal[i]);
   }finally{G.CloseInventory();G.Controls=input;G.Paused=true;}
  }
 }
}
