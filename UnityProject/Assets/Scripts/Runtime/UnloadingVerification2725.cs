using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace GreyHarbor {
 public sealed partial class RaidVerification {
  sealed class UnloadingActorMemory2725 {
   public RaidActor Actor;public Vector3 Position;public float Armor,Health;public bool Dead;public LootContainer Corpse;public string Contents;
  }
  UnloadingActorMemory2725 UnloadingRemember2725(RaidActor actor,LootContainer corpse){return new UnloadingActorMemory2725{Actor=actor,Position=actor.transform.position,Armor=actor.NpcArmorPoints,Health=actor.Health,Dead=actor.Dead,Corpse=corpse,Contents=corpse==null?null:FreightMonsterItems2720(corpse)};}
  void UnloadingRestoreCheck2725(List<UnloadingActorMemory2725> memories,string label){
   foreach(var m in memories){Check(G.Enemies.Contains(m.Actor)&&m.Actor.gameObject.activeInHierarchy&&Vector3.Distance(m.Actor.transform.position,m.Position)<.001f&&CombatNear275(m.Actor.NpcArmorPoints,m.Armor)&&CombatNear275(m.Actor.Health,m.Health)&&m.Actor.Dead==m.Dead,"2725 cached actual actor identity, position, armor, HP and death retained "+label+"/"+m.Actor.name);
    if(m.Corpse!=null)Check(G.Map.Loot.Contains(m.Corpse)&&m.Corpse.Opened&&m.Contents==FreightMonsterItems2720(m.Corpse)&&m.Corpse.Items.TrueForAll(i=>i.Identified)&&!m.Actor.HitCollider.enabled,"2725 original searched biological corpse remains without respawn "+m.Actor.name);
   }
  }
  void UnloadingRoster2725(int count){
   var map=G.Map;Check(map.UnloadingEnemies2725.Count==count,"2725 exact new unloading monster plans "+map.InteriorId+"="+count);var identities=new HashSet<RaidActor>();
   foreach(var p in map.UnloadingEnemies2725){var e=p.Actor;bool runner=p.Kind==MonsterKind.Runner;Check(e&&identities.Add(e)&&G.Enemies.Contains(e)&&e.gameObject.activeInHierarchy&&e.IsMonster&&!e.IsRebel&&!e.Dead&&e.Monster==p.Kind&&e.NpcLevel==1&&p.Level==1&&e.NpcAmmo==ItemKind.GreenAmmo&&Vector3.Distance(e.Home,p.Home)<.001f,"2725 real new ordinary green level-one actor "+p.Name);
    Check(CombatNear275(e.NpcMaxArmor,runner?20:30)&&CombatNear275(e.NpcArmorPoints,runner?20:30)&&CombatNear275(e.NpcMaxHealth,runner?64:96)&&CombatNear275(e.Health,runner?64:96),"2725 inherited green monster armor/HP "+p.Name);
    Check(p.Targets!=null&&p.Targets.Length==3&&e.PatrolTargets.Length==3&&CargoClear2713(p.Home),"2725 real clear spawn and three-point patrol "+p.Name);
    var searches=map.HarborWhitebox?map.UnloadingOutdoorSearch2724:map.UnloadingSearchRecords2724;
    for(int i=0;i<p.Targets.Length;i++){var point=p.Targets[i];Check(Vector3.Distance(point,e.PatrolTargets[i])<.001f&&CargoClear2713(point)&&G.Navigation.ClearSegment(point,p.Targets[(i+1)%p.Targets.Length]),"2725 actual authored patrol point and swept edge "+p.Name+"/"+i);
     foreach(var door in map.Doors)Check(Vector3.Distance(point,door.Position)>=1.5f,"2725 new patrol avoids doorway "+p.Name+"/"+door.Name+"/"+i);
     foreach(var box in searches)Check(Vector3.Distance(point,box.Stand)>=1.5f&&RaidMap.UnloadingEnemyDistance2725(point,box.Stand,box.AccessPoint)>=.85f,"2725 new patrol avoids real search stand and approach "+p.Name+"/"+box.Container.Name+"/"+i);
     if(map.HarborWhitebox){foreach(var entry in new[]{RaidMap.UnloadingPreviewSpawn2723,RaidMap.UnloadingWarehouseDoor2723,RaidMap.UnloadingWarehouseArrival2723,RaidMap.UnloadingDutyDoor2723,RaidMap.UnloadingDutyArrival2723})Check(Vector3.Distance(point,entry)>=6,"2725 exterior patrol preserves six-metre arrival safety "+p.Name+"/"+i);}
     else Check(Vector3.Distance(point,map.Spawn)>=6,"2725 warehouse patrol preserves six-metre spawn safety "+p.Name+"/"+i);
    }
    FreightMonsterPatrol2720(p,null);FreightMonsterAttack2720(e);
   }
  }
  void UnloadingFloorCheck2725(){
   var map=G.Map;var floor=map.UnloadingFloorRenderer2725;var texture=map.UnloadingFloorTexture2725;Check(floor&&floor.enabled&&texture&&texture.mipmapCount>1&&texture.filterMode==FilterMode.Trilinear&&texture.wrapMode==TextureWrapMode.Clamp,"2725 finished floor has actual mipmapped stable texture "+map.InteriorId);
   if(!floor||!texture)return;var material=floor.sharedMaterial;
   Check(material&&material.shader.name=="GreyHarbor/World"&&material.mainTexture==texture&&material.GetFloat("_UseStableGround")>.5f&&material.GetFloat("_SolidFog")<.5f,"2725 real floor shader retains texture and normal room fog "+map.InteriorId);
   var old=map.Root.Find("UnloadingWhiteboxFloor2723");Check(old&&!old.GetComponent<Renderer>().enabled&&old.GetComponent<Collider>()&&old.GetComponent<Collider>().enabled,"2725 old whitebox face hidden while supporting collision retained");
   var pixels=texture.GetPixels32();var palette=new HashSet<int>();float min=1,max=0;bool opaque=true;foreach(var c in pixels){palette.Add(c.r<<16|c.g<<8|c.b);float lum=(.2126f*c.r+.7152f*c.g+.0722f*c.b)/255f;min=Mathf.Min(min,lum);max=Mathf.Max(max,lum);opaque&=c.a==255;}
   Check(opaque&&palette.Count>32&&max-min>.06f&&min>.12f&&max<.90f,"2725 actual floor texture contains moderate coloured wear rather than white/black blank "+map.InteriorId+" colors="+palette.Count+" range="+min+".."+max);
   if(map.InteriorId=="unloadingduty"){Color tile=Color.clear,concrete=Color.clear;for(int y=0;y<16;y++)for(int x=0;x<16;x++){float v=.15f+(y+.5f)/16*.7f;tile+=texture.GetPixelBilinear(.15f+(x+.5f)/16*.35f,v);concrete+=texture.GetPixelBilinear(.70f+(x+.5f)/16*.20f,v);}tile/=256;concrete/=256;Check(FreightColorDistance2717(tile,concrete)>.035f,"2725 duty workroom tile and storage concrete differ across actual texture regions tile="+tile+" concrete="+concrete);}
  }
  void UnloadingFloorPixel2725(Vector3 point,bool lit,string label){
   var floor=G.Map.UnloadingFloorRenderer2725;Check(floor&&G.Vision.RoomAllowed(point)==lit&&Shader.GetGlobalFloat("_HarborFogEnabled")>.5f,"2725 real floor GPU sample uses current room reveal "+label);if(!floor)return;
   int layer=floor.gameObject.layer;float fog=Shader.GetGlobalFloat("_HarborFogEnabled");var obj=new GameObject("QA2725_FloorPixel");var cam=obj.AddComponent<Camera>();cam.enabled=false;cam.orthographic=true;cam.orthographicSize=.16f;cam.nearClipPlane=.02f;cam.farClipPlane=3;cam.clearFlags=CameraClearFlags.SolidColor;cam.backgroundColor=Color.black;cam.cullingMask=1<<31;point.y=-.009f;cam.transform.position=point+Vector3.up*1.7f;cam.transform.rotation=Quaternion.LookRotation(Vector3.down,Vector3.forward);Texture2D frame=null;
   try{floor.gameObject.layer=31;frame=HvRender277(cam,"2725-floor-pixel-"+label+".png",64,64);double sum=0;float peak=0;for(int y=16;y<48;y++)for(int x=16;x<48;x++){float c=frame.GetPixel(x,y).grayscale;sum+=c;peak=Mathf.Max(peak,c);}float mean=(float)(sum/1024);Check(lit?mean>.02f:peak<.012f,"2725 actual finished floor "+(lit?"readable":"black before opening")+" "+label+" mean="+mean+" peak="+peak);}
   finally{floor.gameObject.layer=layer;if(frame)Destroy(frame);Destroy(obj);}Check(Shader.GetGlobalFloat("_HarborFogEnabled")==fog&&floor.gameObject.layer==layer,"2725 GPU probe restores scene and fog "+label);
  }
  IEnumerator UnloadingPicture2725(string name,Vector3 center,float scale){
   G.Rig.Tick(0);G.Vision.Refresh(G.Player.transform.position,G.Player.Aim);G.Player.Draw(G.View,0);yield return null;float fog=Shader.GetGlobalFloat("_HarborFogEnabled");Check(fog>.5f,"2725 actual gameplay camera keeps world fog "+name);var frame=HvRender277(G.View,"2725-"+name+"-live-fog.png",1600,900);try{UnloadingLiveImage2724(frame,name);}finally{Destroy(frame);}Check(Shader.GetGlobalFloat("_HarborFogEnabled")==fog,"2725 live rendering preserves fog "+name);yield return CargoPicture2715("2725-"+name+"-overview.png",center,45,scale,false,1600,1100);
  }
  IEnumerator RunUnloading2725(){
   int start=results.Count;var controls=G.Controls;var random=UnityEngine.Random.state;string dir=Path.GetDirectoryName(G.SavePath);string[] formal={"stash-v40.json","stash-v41.json","stash-v42.json"};byte[][] original=Array.ConvertAll(formal,n=>File.Exists(Path.Combine(dir,n))?File.ReadAllBytes(Path.Combine(dir,n)):null);
   Check(Application.version=="2.7.25"&&G.VerifyMode&&G.SavePath.EndsWith("verification-stash-2.7.25.json"),"2725 isolated current-version unloading floor and monster verification");
   try{
    FreshCamp();string inventory=JsonUtility.ToJson(G.Save);G.Controls=new TouchInput();G.HarborWalkthrough=true;yield return G.BeginTrainingLoad2721();G.Paused=true;var outdoor=G.Map;var outsideActors=G.Enemies.ToArray();
    Check(outdoor.HarborWhitebox&&outdoor.GroundCacheUsed2721&&outdoor.HarborPlanCacheUsed2721&&G.Enemies.Count==33&&outdoor.Loot.Count==57,"2725 normal harbor loading preserves 57 boxes and adds exactly three exterior monsters");Check(UnloadingPreview2723.Apply(G),"2725 new district preview arrives safely");UnloadingRoster2725(3);
    yield return UnloadingPicture2725("unloading-outdoor",RaidMap.PortPoint(131,157),23);
    var outdoorStates=new List<UnloadingActorMemory2725>();var outsidePlans=outdoor.UnloadingEnemies2725;FreightMonsterShots2720(outsidePlans[0].Actor,4);var outsideCorpse=FreightMonsterKill2720(outsidePlans[1].Actor);FreightMonsterSearch2720(outsideCorpse);for(int i=0;i<outsidePlans.Count;i++)outdoorStates.Add(UnloadingRemember2725(outsidePlans[i].Actor,i==1?outsideCorpse:null));
    var outsideBoxes=outdoor.UnloadingOutdoorSearch2724.ConvertAll(r=>r.Container).ToArray();var outsideItems=Array.ConvertAll(outsideBoxes,box=>FreightMonsterItems2720(box));var maps=new List<RaidMap>();var floorTextures=new List<Texture2D>();var roomStates=new List<List<UnloadingActorMemory2725>>();var boxes=new List<LootContainer[]>();var items=new List<string[]>();
    foreach(string key in new[]{"unloadingwarehouse","unloadingduty"}){
     bool warehouse=key=="unloadingwarehouse";var arrival=warehouse?RaidMap.UnloadingWarehouseArrival2723:RaidMap.UnloadingDutyArrival2723;FreightRoomWalk2717(arrival,"2725 real exterior door approach "+key);FreightMonsterF2720(outdoor.Doors.Find(d=>d.Portal&&d.Destination==key),true);yield return null;var indoor=G.Map;
     Check(indoor.UnloadingInterior2723&&indoor.InteriorId==key&&G.Enemies.Count==(warehouse?2:0)&&indoor.Loot.Count==(warehouse?6:2)&&CargoClear2713(G.Player.transform.position),"2725 real held F enters correct floor/monster interior "+key);Check(Array.TrueForAll(outsideActors,e=>!e.gameObject.activeInHierarchy),"2725 exterior actors suspended indoors");UnloadingFloorCheck2725();UnloadingRoster2725(warehouse?2:0);UnloadingFloorPixel2725(new Vector3(0,0,0),true,key+"-main-room");
     if(!warehouse){var door=indoor.UnloadingStoreDoor2723;var near=door.Position-Vector3.right*.65f;var far=door.Position+Vector3.right*.8f;Check(!door.Open&&!G.Vision.RoomAllowed(far)&&!G.Navigation.ClearSegment(near,far),"2725 closed storage door keeps new floor unknown and passage blocked");UnloadingFloorPixel2725(new Vector3(2.4f,0,-.2f),false,"storage-closed");FreightRoomWalk2717(near,"2725 real storage door approach");FreightMonsterF2720(door,false);Check(door.Open&&G.Vision.RoomAllowed(far)&&G.Navigation.ClearSegment(near,far),"2725 actual opening reveals floor and enables passage");FreightRoomWalk2717(far,"2725 actual storage entry");UnloadingFloorPixel2725(new Vector3(2.4f,0,-.2f),true,"storage-open");}
     // Reuse only actual search accessibility/interaction, without any economy distribution suite.
     yield return UnloadingSearch2724(warehouse?6:2);FreightRoomWalk2717(indoor.Spawn,"2725 real floor-supported return to entrance");yield return UnloadingPicture2725(key,Vector3.zero,warehouse?13:5.8f);
     var states=new List<UnloadingActorMemory2725>();if(warehouse){var plans=indoor.UnloadingEnemies2725;FreightMonsterShots2720(plans[0].Actor,4);var corpse=FreightMonsterKill2720(plans[1].Actor);FreightMonsterSearch2720(corpse);states.Add(UnloadingRemember2725(plans[0].Actor,null));states.Add(UnloadingRemember2725(plans[1].Actor,corpse));}
     maps.Add(indoor);floorTextures.Add(indoor.UnloadingFloorTexture2725);roomStates.Add(states);var originals=indoor.UnloadingSearchRecords2724.ConvertAll(r=>r.Container).ToArray();boxes.Add(originals);items.Add(Array.ConvertAll(originals,box=>FreightMonsterItems2720(box)));FreightRoomWalk2717(indoor.Spawn,"2725 real interior exit walk");FreightMonsterF2720(indoor.UnloadingExit2723,true);yield return null;
     Check(G.Map==outdoor&&G.Enemies.Count==33&&Array.TrueForAll(outsideActors,e=>G.Enemies.Contains(e))&&outdoor.Loot.Count==58,"2725 return retains original exterior actors, boxes and one corpse");UnloadingRestoreCheck2725(outdoorStates,"outdoor");
    }
    for(int m=0;m<maps.Count;m++){var indoor=maps[m];string key=indoor.InteriorId;var arrival=key=="unloadingwarehouse"?RaidMap.UnloadingWarehouseArrival2723:RaidMap.UnloadingDutyArrival2723;FreightRoomWalk2717(arrival,"2725 cached building actual approach "+key);FreightMonsterF2720(outdoor.Doors.Find(d=>d.Portal&&d.Destination==key),true);yield return null;
     Check(G.Map==indoor&&indoor.UnloadingFloorTexture2725==floorTextures[m]&&G.Enemies.Count==(m==0?2:0)&&indoor.Loot.Count==(m==0?7:2),"2725 same interior/floor resources and expected corpse count restored "+key);UnloadingRestoreCheck2725(roomStates[m],key);if(m==1)Check(indoor.UnloadingStoreDoor2723.Open&&G.Vision.RoomAllowed(new Vector3(2.4f,0,-.2f)),"2725 storage door and new floor reveal survive crossing warehouse");
     for(int i=0;i<boxes[m].Length;i++){var box=boxes[m][i];Check(indoor.UnloadingSearchRecords2724[i].Container==box&&indoor.Loot.Contains(box)&&box.Opened&&box.Prop.Searched&&box.Items.TrueForAll(item=>item.Identified)&&items[m][i]==FreightMonsterItems2720(box),"2725 old box identity/search/contents remain across another building "+box.Name);}
     FreightRoomWalk2717(indoor.Spawn,"2725 cached interior real exit movement");FreightMonsterF2720(indoor.UnloadingExit2723,true);yield return null;Check(G.Map==outdoor&&G.Enemies.Count==33&&outdoor.Loot.Count==58,"2725 cached interior returns original exterior without respawn");UnloadingRestoreCheck2725(outdoorStates,"outdoor-revisit");
    }
    for(int i=0;i<outsideBoxes.Length;i++)Check(outdoor.UnloadingOutdoorSearch2724[i].Container==outsideBoxes[i]&&outdoor.Loot.Contains(outsideBoxes[i])&&outsideItems[i]==FreightMonsterItems2720(outsideBoxes[i]),"2725 original exterior box identity and contents unchanged "+outsideBoxes[i].Name);
    G.Finish(false);yield return null;Check(G.Phase==GamePhase.Base&&JsonUtility.ToJson(G.Save)==inventory,"2725 preview restores exact original camp inventory");for(int i=0;i<formal.Length;i++)Check(MainBytes273(original[i],File.Exists(Path.Combine(dir,formal[i]))?File.ReadAllBytes(Path.Combine(dir,formal[i])):null),"2725 formal save bytes unchanged "+formal[i]);
   }finally{G.ClearShots();G.CloseInventory();G.Controls=controls;UnityEngine.Random.state=random;G.Paused=true;var report=new List<string>();for(int i=start;i<results.Count;i++)report.Add(results[i]);report.Add("INFO Scope: unloading interior floors and five new green monsters only; actual 20-second patrol/combat/death/search, 6+2 box access, real F portals, cross-building cache, GPU floor fog and actual Camera.Render. No unrelated full-map economy or previous palette suite.");File.WriteAllLines(Path.Combine(folder,"2725-unloading-targeted-verification.txt"),report);}
  }
 }
}
