using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
namespace GreyHarbor {
 public sealed partial class RaidVerification {
  readonly List<string> freightRoomRows2717=new List<string>();
  void FreightRoomRow2717(string kind,string name,Vector3 p,string detail){freightRoomRows2717.Add(Csv230(kind)+","+Csv230(name)+","+CargoNumber2713(p.x)+","+CargoNumber2713(p.y)+","+CargoNumber2713(p.z)+","+Csv230(detail));}
  void FreightRoomPlace2717(Vector3 p,string label){Check(CargoClear2713(p),"2717 室外夹具站位真实胶囊安全："+label);G.Player.transform.position=p;Physics.SyncTransforms();G.Rig.Tick(0);G.Vision.Refresh(p,G.Player.Aim);}
  void FreightRoomWalk2717(Vector3 p,string label){bool pass=WalkMain273(p);Check(pass&&CargoClear2713(p),"2717 真实Actor.Move通过："+label+" 目标="+p+" 原因="+mainWalkFailure273);FreightRoomRow2717("实际步行",label,G.Player.transform.position,".34m导航扫掠与实际Move");}
  void FreightRoomHold2717(WorldDoor door){Check(door!=null,"2717 往返门户存在");SprintInteract2711(false,0,door.Position);SprintInteract2711(false,0,door.Position);Check(G.NearbyDoor==door,"2717 长按F准确选中门户："+door.Name);SprintInteract2711(true,2.03f,door.Position);}
  static float FreightColorDistance2717(Color a,Color b){return Mathf.Max(Mathf.Abs(a.r-b.r),Mathf.Abs(a.g-b.g),Mathf.Abs(a.b-b.b));}
  void FreightRoomArtCheck2717(){
   var map=G.Map;bool duty=map.InteriorId=="freightduty";
   var floor=map.Root.Find("FreightInteriorFloor2717");Check(floor!=null,"2717 实体室内地板存在："+map.InteriorId);var floorBounds=floor.GetComponent<Renderer>().bounds;
   Check(Mathf.Abs(floorBounds.size.x-(duty?17:18))<.01f&&Mathf.Abs(floorBounds.size.z-17)<.01f,"2717 实际室内尺寸："+(duty?"值班房17×17m":"库房18×17m"));
   Check(map.FreightCeilingHeight2716==(duty?3:4.2f),"2717 保持值班房与高顶库房高度");
   bool noBeams=true;foreach(var t in map.Root.GetComponentsInChildren<Transform>()){string n=t.name.ToLowerInvariant();noBeams&=!n.Contains("roofbeam")&&!n.Contains("ceilingbeam");}Check(noBeams,"2717 室内无挡视线顶横梁："+map.InteriorId);
   Check(map.FreightRooms2716.Count==(duty?4:1)&&map.FreightDressingRecords2717.Count>=10,"2717 用途分区及实际装修物存在："+map.InteriorId);
   var palette=new HashSet<string>();int floorProps=0,mounted=0;bool warm=false,green=false,blue=false,paper=false;var views=new HashSet<GameObject>();
   foreach(var r in map.FreightDressingRecords2717){
    Check(r.View&&views.Add(r.View),"2717 装修记录对应独立模型："+r.Type);float minimum=float.MaxValue;int visible=0,physical=0;bool volume=false;
    foreach(var renderer in r.View.GetComponentsInChildren<Renderer>())if(renderer.enabled){visible++;minimum=Mathf.Min(minimum,renderer.bounds.min.y);volume|=renderer.bounds.size.x>.02f&&renderer.bounds.size.y>.02f&&renderer.bounds.size.z>.005f;
     foreach(var material in renderer.sharedMaterials){Check(material&&material.shader&&material.shader.name=="GreyHarbor/World","2717 家具与墙面保留世界黑雾材质："+r.Type+"/"+renderer.name);var c=material.color;string hex=ColorUtility.ToHtmlStringRGB(c);palette.Add(hex);float saturation=Mathf.Max(c.r,c.g,c.b)-Mathf.Min(c.r,c.g,c.b);warm|=c.r>c.b+.07f&&c.r>c.g+.025f;green|=c.g>c.r+.025f&&saturation>.05f;blue|=c.b>c.r+.035f&&saturation>.05f;paper|=c.r>.65f&&c.g>.63f&&c.b>.49f&&c.r>c.b+.055f;FreightRoomRow2717("实际材质",r.Type+"/"+renderer.name,renderer.bounds.center,"RGB=#"+hex+" shader="+material.shader.name);}
    }
    foreach(var c in r.View.GetComponentsInChildren<Collider>())if(c.enabled&&!c.isTrigger){physical++;var rule=c.GetComponentInParent<SceneObstacle>();Check(rule&&rule.Kind!=ObstacleKind.Decoration&&c.gameObject.layer==(rule.Kind==ObstacleKind.High?SceneObstacle.HighLayer:SceneObstacle.LowLayer),"2717 实体分件碰撞保持实际高低层："+r.Type+"/"+c.name);var b=c.bounds;const float epsilon=.005f;Check(b.min.x>=r.Footprint.xMin-epsilon&&b.max.x<=r.Footprint.xMax+epsilon&&b.min.z>=r.Footprint.yMin-epsilon&&b.max.z<=r.Footprint.yMax+epsilon,"2717 记录占地包含真实碰撞体："+r.Type);}
    Check(visible>0&&volume,"2717 装修物具有真实可见体积："+r.Type);
    if(r.WallMounted){mounted++;Check(physical==0,"2717 壁挂装饰不阻挡行走："+r.Type);}else{floorProps++;Check(physical>0&&Mathf.Abs(minimum-RaidMap.FreightFloorY2717)<.008f,"2717 落地家具真实最低点贴地："+r.Type+" minY="+minimum);}
    FreightRoomRow2717("装修占地",r.Type,r.View.transform.position,"room="+r.RoomName+" footprint="+r.Footprint+" minY="+minimum+" colliders="+physical);
   }
   Check(floorProps>=8&&mounted>=3&&palette.Count>=12&&warm&&green&&blue&&paper,"2717 实际家具材质包含木/棕纸箱、绿漆织物、蓝漆及米白纸张，并非全灰："+map.InteriorId+" 色数="+palette.Count);
   Check(map.StableGround137&&map.StableGround137.mipmapCount>1&&map.FreightFloorZones2717.Count==(duty?5:2),"2717 分区地板使用真实稳定纹理与不同材料");
   foreach(var zone in map.FreightFloorZones2717){float best=1;Color sample=Color.clear;Vector3 selected=Vector3.zero;for(int z=0;z<7;z++)for(int x=0;x<7;x++){var p=new Vector3(Mathf.Lerp(zone.Area.xMin,zone.Area.xMax,(x+.37f)/7),0,Mathf.Lerp(zone.Area.yMin,zone.Area.yMax,(z+.41f)/7));var c=CargoGroundPixel2715(p);float distance=FreightColorDistance2717(c,zone.Color);if(c.a>.98f&&distance<best){best=distance;sample=c;selected=p;}}Check(best<.08f,"2717 真实地板纹理保留分区配色："+zone.Name+" "+zone.Material+" 色差="+best);FreightRoomRow2717("真实地板纹理",zone.Name,selected,"material="+zone.Material+" sampled=#"+ColorUtility.ToHtmlStringRGB(sample)+" expected=#"+ColorUtility.ToHtmlStringRGB(zone.Color));}
  }
  IEnumerator FreightRoomDoors2717(){
   foreach(var room in G.Map.FreightRooms2716){var d=room.Door;float side=room.Area.center.x<d.Position.x?-1:1;var outside=d.Position-Vector3.right*side*.95f;var inside=d.Position+Vector3.right*side*1.1f;
    Check(!d.Open&&!G.Vision.RoomAllowed(room.WalkPoint),"2717 初次进入关闭房门保持房间黑雾："+room.Name);FreightRoomWalk2717(outside,room.Name+"门轴线外侧");SprintInteract2711(false,0,d.Position);SprintInteract2711(false,0,d.Position);Check(G.NearbyDoor==d,"2717 新家具不抢房门F选择："+room.Name);SprintInteract2711(true,0,d.Position);SprintInteract2711(false,0,d.Position);
    Check(d.Open&&G.Vision.RoomAllowed(room.WalkPoint)&&G.Navigation.ClearSegment(outside,inside),"2717 实际F开门后轴线可穿行并揭示房间："+room.Name);FreightRoomWalk2717(inside,room.Name+"穿过门轴线");FreightRoomWalk2717(room.WalkPoint,room.Name+"室内走到工作区");FreightRoomWalk2717(inside,room.Name+"返回门轴线");FreightRoomWalk2717(outside,room.Name+"实际离开房间");yield return null;
   }
   foreach(var route in G.Map.FreightWalkRoutes2717){Check(route.Points!=null&&route.Points.Length>=2,"2717 室内路线记录有效："+route.Name);FreightRoomWalk2717(route.Points[0],route.Name+"起点");for(int i=1;i<route.Points.Length;i++){Check(G.Navigation.ClearSegment(route.Points[i-1],route.Points[i]),"2717 室内路线真实胶囊扫掠："+route.Name);float radius=route.Width*.5f-.05f;bool clear=true;int steps=Mathf.Max(1,Mathf.CeilToInt(Vector3.Distance(route.Points[i-1],route.Points[i])/.35f));for(int j=0;j<=steps;j++){var p=Vector3.Lerp(route.Points[i-1],route.Points[i],j/(float)steps);clear&=!Physics.CheckCapsule(p+Vector3.up*.35f,p+Vector3.up,radius,SceneObstacle.MovementMask,QueryTriggerInteraction.Ignore);}Check(clear,"2717 装修后通道保留实际净宽："+route.Name+" "+route.Width+"m");FreightRoomWalk2717(route.Points[i],route.Name+"路线段"+i);}yield return null;}
  }
  IEnumerator FreightRoomSearch2717(){
   var map=G.Map;Check(map.Loot.Count==6&&map.FreightLootRecords2717.Count==6,"2717 本栋恰好六个真实室内搜索容器："+map.InteriorId);var boxes=new HashSet<LootContainer>();
   foreach(var r in map.FreightLootRecords2717){var box=r.Container;Check(box!=null&&boxes.Add(box)&&map.Loot.Contains(box)&&box.DropProfile230!=null&&box.DropProfile230.Tier==ContainerTier.Common&&box.DropProfile230.FixedCount==0&&box.DropProfile230.Look!=ContainerLook.Safe,"2717 室内搜索沿用Common普通掉落且无新增保险箱："+(box==null?"null":box.Name));Check(r.Model&&box.Prop&&box.View==r.Model&&Vector3.Distance(box.Position,r.Stand)<.001f,"2717 搜索记录与真实模型站位一致："+box.Name);
    if(!r.WallMounted){float lowest=float.MaxValue;foreach(var renderer in r.Model.GetComponentsInChildren<Renderer>())if(renderer.enabled)lowest=Mathf.Min(lowest,renderer.bounds.min.y);Check(Mathf.Abs(lowest-RaidMap.FreightFloorY2717)<.008f,"2717 可搜索模型真实贴地："+box.Name+" minY="+lowest);}
    FreightRoomWalk2717(r.AccessPoint,box.Name+"实际接近路线");Check(CargoClear2713(r.Stand)&&G.Navigation.ClearSegment(r.AccessPoint,r.Stand),"2717 搜索站位和接近线真实可走："+box.Name);FreightRoomWalk2717(r.Stand,box.Name+"真实搜索站位");G.Rig.Tick(0);G.Player.Aim=(box.Prop.transform.position-G.Player.transform.position).normalized;G.Vision.Refresh(G.Player.transform.position,G.Player.Aim);Check(G.CanInteractLoot(box),"2717 室内容器实际可见并在交互距离内："+box.Name);int count=box.Items.Count;string identity=JsonUtility.ToJson(new CargoItems2715{Items=box.Items.ToArray()});
    SprintInteract2711(false,0,box.Prop.transform.position);SprintInteract2711(false,0,box.Prop.transform.position);Check(G.Nearby==box,"2717 F精确选中本容器且不被家具或邻箱抢占："+box.Name);SprintInteract2711(true,box.SearchDuration+.02f,box.Prop.transform.position);Check(box.Opened&&G.OpenLoot==box&&box.Prop.Searched&&box.Items.Count==count&&identity==JsonUtility.ToJson(new CargoItems2715{Items=box.Items.ToArray()}),"2717 真实长按F完成搜索且不重抽内容："+box.Name);
    G.Paused=false;for(int i=0;i<7;i++)G.TickIdentification(3);G.Paused=true;Check(box.Items.Count==count&&box.Items.TrueForAll(item=>item.Identified),"2717 实际逐格识别完成："+box.Name);G.CloseInventory();SprintInteract2711(false,0,box.Prop.transform.position);FreightRoomRow2717("真实F搜索",box.Name,r.Stand,"items="+count+" room="+r.RoomName+" Common");FreightRoomWalk2717(r.AccessPoint,box.Name+"搜索后真实离开");yield return null;
   }
  }
  IEnumerator FreightRoomPictures2717(){
   bool duty=G.Map.InteriorId=="freightduty";string key=G.Map.InteriorId;Vector3 p=duty?G.Map.FreightRooms2716[0].WalkPoint:new Vector3(1.5f,0,3.2f);FreightRoomWalk2717(p,key+"真实拍摄站位");G.Rig.ResetView(0);G.Rig.Tick(0);G.Player.Aim=duty?new Vector3(-1,0,1).normalized:Vector3.right;G.Vision.Refresh(p,G.Player.Aim);G.Player.Draw(G.View,0);yield return null;
   float fog=Shader.GetGlobalFloat("_HarborFogEnabled");Check(fog>.5f,"2717 正常玩法截图保留实际黑雾："+key);var image=HvRender277(G.View,"2717-"+key+"-live-fog.png",1600,900);try{CargoImageCheck2713(image,"2717-"+key+"-live-fog");}finally{Destroy(image);}Check(Shader.GetGlobalFloat("_HarborFogEnabled")==fog,"2717 实镜头截图不更改黑雾状态");
   yield return CargoPicture2715("2717-"+key+"-interior-overview.png",new Vector3(0,.3f,0),0,11,true,1600,1600);yield return CargoPicture2715("2717-"+key+"-interior-materials.png",duty?new Vector3(-4.7f,.5f,-4):new Vector3(2,.5f,1),0,duty?6:9,false,1800,1200);
  }
  IEnumerator RunFreightRooms2717(){
   Check(G.Phase==GamePhase.Raid&&G.Map!=null&&!G.Map.IsInterior&&G.HarborWalkthrough,"2717 室内专项复用主流程已经建立的完整港口预览");var controls=G.Controls;bool paused=G.Paused;var outdoor=G.Map;var actors=G.Enemies.ToArray();var maps=new List<RaidMap>();var contents=new List<string[]>();var visions=new List<RaidVision>();var memories=new List<bool[]>();freightRoomRows2717.Clear();freightRoomRows2717.Add("类别,名称,x,y,z,详情");
   try{G.Paused=true;G.Controls=new TouchInput();
    foreach(string key in new[]{"freightduty","freightwarehouse"}){
     var portal=outdoor.Doors.Find(d=>d.Portal&&d.Destination==key);var stand=key=="freightduty"?RaidMap.FreightDutyArrival2716:RaidMap.FreightWarehouseArrival2716;FreightRoomPlace2717(stand,key+"室外门户");FreightRoomHold2717(portal);yield return null;var indoor=G.Map;
     Check(indoor!=outdoor&&indoor.FreightInterior2716&&indoor.InteriorId==key&&G.InsideBooth&&CargoClear2713(G.Player.transform.position),"2717 实际长按F进入正确独立室内并安全落地："+key);Check(G.Enemies.Count==0,"2717 装修室内不生成NPC："+key);FreightRoomArtCheck2717();yield return FreightRoomDoors2717();yield return FreightRoomSearch2717();yield return FreightRoomPictures2717();
     maps.Add(indoor);contents.Add(indoor.FreightLootRecords2717.ConvertAll(r=>JsonUtility.ToJson(new CargoItems2715{Items=r.Container.Items.ToArray()})).ToArray());visions.Add(G.Vision);memories.Add((bool[])G.Vision.Seen.Clone());FreightRoomWalk2717(indoor.Spawn,key+"真实走回出口");FreightRoomHold2717(indoor.FreightExit2716);yield return null;
     Check(G.Map==outdoor&&Vector3.Distance(G.Player.transform.position,stand)<.1f,"2717 出口返回对应的原室外站位："+key);Check(G.Enemies.Count==actors.Length&&Array.TrueForAll(actors,e=>G.Enemies.Contains(e)),"2717 室内往返不重复生成或替换室外怪物："+key);
    }
    Check(maps[0]!=maps[1]&&visions[0]!=visions[1],"2717 两栋室内具有独立地图及探索缓存");
    for(int m=0;m<maps.Count;m++){var indoor=maps[m];string key=indoor.InteriorId;var stand=key=="freightduty"?RaidMap.FreightDutyArrival2716:RaidMap.FreightWarehouseArrival2716;var portal=outdoor.Doors.Find(d=>d.Portal&&d.Destination==key);FreightRoomPlace2717(stand,key+"跨图后再次进入");FreightRoomHold2717(portal);yield return null;
     Check(G.Map==indoor&&G.Vision==visions[m]&&G.Enemies.Count==0&&G.Map.Loot.Count==6,"2717 跨另一栋后重新进入恢复原室内对象且不新增箱或NPC："+key);foreach(var room in indoor.FreightRooms2716)Check(room.Door.Open&&G.Vision.RoomAllowed(room.WalkPoint),"2717 原房门开关及揭示状态保留："+room.Name);
     bool preserved=true;for(int i=0;i<memories[m].Length;i++)if(memories[m][i]&&!G.Vision.Seen[i])preserved=false;Check(preserved,"2717 室内探索记忆跨图不丢失："+key);for(int i=0;i<indoor.FreightLootRecords2717.Count;i++){var box=indoor.FreightLootRecords2717[i].Container;Check(box.Opened&&box.Prop.Searched&&G.Map.Loot.Contains(box)&&contents[m][i]==JsonUtility.ToJson(new CargoItems2715{Items=box.Items.ToArray()}),"2717 原箱身份、搜索与识别结果不重置："+box.Name);}
     FreightRoomWalk2717(indoor.Spawn,key+"缓存室内真实走回出口");FreightRoomHold2717(indoor.FreightExit2716);yield return null;Check(G.Map==outdoor&&Vector3.Distance(G.Player.transform.position,stand)<.1f&&G.Enemies.Count==actors.Length&&Array.TrueForAll(actors,e=>G.Enemies.Contains(e)),"2717 二次退出仍返回原外场且怪物身份数量不变："+key);
    }
   }finally{G.CloseInventory();G.Controls=controls;G.Paused=paused;File.WriteAllLines(Path.Combine(folder,"2717-freight-rooms-actual.csv"),freightRoomRows2717);}
  }
 }
}
