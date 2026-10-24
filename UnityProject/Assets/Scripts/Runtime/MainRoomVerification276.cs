using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Rendering;
namespace GreyHarbor {
 public sealed partial class RaidVerification {
  bool RoomsBounds276(GameObject root,out Bounds bounds){
   bounds=new Bounds();bool any=false;foreach(var r in root.GetComponentsInChildren<Renderer>())if(r.enabled){if(!any){bounds=r.bounds;any=true;}else bounds.Encapsulate(r.bounds);}return any;
  }
  float RoomsDistance276(Rect r,Vector3 p){float x=p.x-Mathf.Clamp(p.x,r.xMin,r.xMax),z=p.z-Mathf.Clamp(p.z,r.yMin,r.yMax);return Mathf.Sqrt(x*x+z*z);}
  void RoomsWallFurniture276(List<string> rows){
   int wallCount=0,centerCount=0;
   foreach(var f in G.Map.Root.GetComponentsInChildren<RepairMainFurniture274>()){
    Bounds b;Check(RoomsBounds276(f.gameObject,out b),"276 Furnishing has real rendered geometry "+f.name);
    if(!f.WallBack){centerCount++;continue;}wallCount++;
    Check(f.WallSide>=0&&f.WallSide<=3,"276 Wall furnishing declares service direction "+f.name);
    Vector3 outward=f.WallSide==0?Vector3.forward:f.WallSide==1?Vector3.right:f.WallSide==2?Vector3.back:Vector3.left;
    Vector3 edge=new Vector3(f.WallSide==1?b.max.x:f.WallSide==3?b.min.x:b.center.x,.8f,f.WallSide==0?b.max.z:f.WallSide==2?b.min.z:b.center.z);
    float gap=float.PositiveInfinity;Collider wall=null;
    foreach(var hit in Physics.RaycastAll(edge,outward,1,SceneObstacle.SightMask,QueryTriggerInteraction.Ignore))if(hit.collider.name=="InteriorWall"&&hit.distance<gap){gap=hit.distance;wall=hit.collider;}
    Check(wall!=null&&gap>=.049f&&gap<=.151f,"276 Cabinet/rack rear lies .05-.15m from actual solid wall "+f.RoomName+" / "+f.name+" gap="+gap);
    Check(Vector3.Dot(f.transform.forward,outward)>.99f,"276 Cabinet/rack front faces the room "+f.name);
   rows.Add(G.Map.RepairFloor220+","+Csv230(f.RoomName)+","+Csv230(f.name)+",wall,"+gap+","+b.center.x+","+b.center.z+","+b.size.x+","+b.size.z);
   }
   foreach(var c in G.Map.Root.GetComponentsInChildren<Collider>())if(c.name=="RepairOfficeDesk220"||c.name=="RepairMeetingTable220")centerCount++;
   Check(wallCount>=2&&centerCount>=1,"276 Floor retains wall furnishings and a grouped interior arrangement "+G.Map.RepairFloor220);
  }
  void RoomsShadow276(){
   int walls=0,contact=0;
   foreach(var r in G.Map.Root.GetComponentsInChildren<Renderer>()){
    if(r.sharedMaterial&&r.sharedMaterial.shader&&r.sharedMaterial.shader.name=="GreyHarbor/RepairWall220"){
     walls++;Check(r.shadowCastingMode==ShadowCastingMode.Off&&!r.receiveShadows,"276 Wall rejects outdoor sun shadow bands "+r.name);
    }else if(r.shadowCastingMode==ShadowCastingMode.On&&r.bounds.size.y>.2f)contact++;
   }
   Check(walls>4&&contact>0,"276 Physical wall shadow treatment retains furniture contact shadows floor "+G.Map.RepairFloor220);
   Check(RaidVision.HalfAngle==35&&G.Vision.RoomGate&&Shader.GetGlobalFloat("_HarborFogEnabled")==1,"276 Actual 70-degree flashlight and black room fog remain enabled");
  }
  void RoomsItems276(List<string> rows,HashSet<string> types){
   var map=G.Map;Check(map.MainDecorationItems274.Count>0,"276 Floor contains new decoration with declared occupancy "+map.RepairFloor220);
   int volumes=0;
   foreach(var item in map.MainDecorationItems274){
    Bounds b=new Bounds();Check(item.View&&RoomsBounds276(item.View,out b),"276 Actual decoration geometry exists "+item.Name);
    types.Add(item.Type.ToString());
    Check(item.WallMounted?b.size.magnitude>.15f:b.size.x>.02f&&b.size.y>.02f&&b.size.z>.005f,"276 Decoration has visible geometry and floor objects have volume "+item.Name);
    if(!item.WallMounted&&item.Kind!=ObstacleKind.Decoration){
     volumes++;bool collider=false;foreach(var c in item.View.GetComponentsInChildren<Collider>())if(c.enabled&&(SceneObstacle.MovementMask&(1<<c.gameObject.layer))!=0)collider=true;
     Check(collider,"276 Floor furnishing uses actual physical collision "+item.Name);
     foreach(var box in map.Loot)Check(RoomsDistance276(item.Footprint,box.Position)>=.34f,"276 Decoration keeps searchable actor stand clear "+item.Name+" / "+box.Name);
     foreach(var door in map.Doors)if(!door.Portal)Check(!item.Footprint.Overlaps(door.ClosedSafety)&&!item.Footprint.Overlaps(door.OpenSafety),"276 Decoration avoids door opening and closing occupancy "+item.Name+" / "+door.Name);
    }
    rows.Add(map.RepairFloor220+","+Csv230(item.RoomName)+","+Csv230(item.Name)+","+Csv230(item.Type.ToString())+","+item.Footprint.xMin+","+item.Footprint.yMin+","+item.Footprint.width+","+item.Footprint.height+","+item.Kind+","+item.WallMounted);
   }
   Check(volumes>=3,"276 New dressing includes substantial floor objects "+map.RepairFloor220);
   foreach(var box in map.Loot)Check(Clear140(box.Position),"276 All existing search stands remain physically clear "+box.Name);
  }
  void RoomsSearchPlacement276(){
   Check(G.Map.MainSearchPlacements274.Count==G.Map.Loot.Count,"276 Every moved searchable prop has an actual placement record");
   string[] expected=G.Map.RepairFloor220==1?new[]{"检修工具箱","精密拆修工具箱","待修零件储料箱","配电设备柜","维修值班记录柜","值班急救柜","巡检弹药箱"}:G.Map.RepairFloor220==2?new[]{"拆修台工具箱","部件运输箱","精密测试设备箱","校验工具箱","二楼 · 小保险箱","备件维护材料箱","楼层装备补给箱","二楼巡检弹药箱"}:G.Map.RepairFloor220==3?new[]{"电气装配设备柜","仪表测试电子箱","技术设计档案箱","维修登记文件柜","三楼 · 小保险箱","配电监测精密箱","楼层急救柜"}:new[]{"职工遗留手提箱","更衣室旧外套","人事排班档案箱","管理电子设备箱","四楼 · 大保险箱","值班管理文件柜"};
   foreach(var name in expected)Check(G.Map.Loot.FindAll(p=>p.Name==name).Count==1,"276 Preserved searchable identity appears exactly once "+name);
   foreach(var p in G.Map.MainSearchPlacements274){
    Bounds b=new Bounds();Check(p.Box.Prop&&RoomsBounds276(p.Box.Prop.gameObject,out b),"276 Search prop has real closed bounds "+p.Box.Name);
    if(p.Support){Bounds support;Check(RoomsBounds276(p.Support,out support),"276 Box support exists "+p.Box.Name);b.Encapsulate(support);}
    Check(p.RoomName==RoomsBoxPurpose276(p.Box.Name)&&RoomsWithin276(p.RoomArea,b)&&p.RoomArea.Contains(new Vector2(p.Box.Position.x,p.Box.Position.z)),"276 Actual box/support bounds and actor search stand fit intended use area "+p.Box.Name+" / "+p.RoomName+" actual="+b+" area="+p.RoomArea);
    var room=G.Map.RepairMainRooms210.Find(r=>r.Name==p.RoomName);
    if(room!=null)Check(RectClose200(room.Area,p.RoomArea),"276 Search room record matches physical named room "+p.Box.Name);
    else foreach(var r in G.Map.RepairMainRooms210){var area=r.Area;float dx=Mathf.Min(b.max.x,area.xMax)-Mathf.Max(b.min.x,area.xMin),dz=Mathf.Min(b.max.z,area.yMax)-Mathf.Max(b.min.z,area.yMin);Check((dx<=.001f||dz<=.001f)&&!area.Contains(new Vector2(p.Box.Position.x,p.Box.Position.z)),"276 Public box and actor stand avoid private room occupancy "+p.Box.Name+" / "+r.Name);}
    if(p.WallAligned){
     Vector3 outward=-p.Inward;var edge=new Vector3(outward.x>.5f?b.max.x:outward.x<-.5f?b.min.x:b.center.x,.8f,outward.z>.5f?b.max.z:outward.z<-.5f?b.min.z:b.center.z);
     float gap=float.PositiveInfinity;foreach(var hit in Physics.RaycastAll(edge,outward,1,SceneObstacle.SightMask,QueryTriggerInteraction.Ignore))if(hit.collider.name=="InteriorWall")gap=Mathf.Min(gap,hit.distance);
     Check(gap>=.049f&&gap<=.151f,"276 Search prop and support actually back onto solid wall "+p.Box.Name+" gap="+gap);
    }
    if(p.Box.Prop.transform.position.y>.1f){
     var anchor=p.Box.Prop.transform.position;float surface=float.NegativeInfinity;
     foreach(var hit in Physics.RaycastAll(anchor+Vector3.up*.1f,Vector3.down,2,SceneObstacle.MovementMask,QueryTriggerInteraction.Ignore))
      if(!hit.collider.transform.IsChildOf(p.Box.Prop.transform)&&hit.point.y<=anchor.y+.02f)surface=Mathf.Max(surface,hit.point.y);
     float gap=anchor.y-surface;Check(gap>=-.021f&&gap<=.071f,"276 Elevated search prop rests on actual support surface "+p.Box.Name+" gap="+gap);
    }
   }
  }
  string RoomsBoxPurpose276(string name){
   switch(name){
    case "待修零件储料箱":return "零件整理与待修件间";case "配电设备柜":return "设备与配电间";case "维修值班记录柜":case "值班急救柜":return "维修值班办公室";
    case "拆修台工具箱":case "部件运输箱":return "部件拆修车间";case "精密测试设备箱":return "精密测试间";case "校验工具箱":return "工具校验间";case "二楼 · 小保险箱":case "备件维护材料箱":return "维修工艺与备件整理间";
    case "电气装配设备柜":return "电气装配室";case "仪表测试电子箱":return "继电与仪表测试室";case "技术设计档案箱":return "技术设计办公室";case "维修登记文件柜":return "维修记录登记室";case "三楼 · 小保险箱":return "工程资料档案室";case "配电监测精密箱":return "配电监测办公室";
    case "职工遗留手提箱":return "职工休息室";case "更衣室旧外套":return "更衣与清洁室";case "人事排班档案箱":return "人事与排班办公室";case "管理电子设备箱":case "四楼 · 大保险箱":return "管理办公室";case "值班管理文件柜":return "值班管理室";default:return "维修大厅";
   }
  }
  IEnumerator RoomsGroups276(int floor){
   int groups=0;foreach(var group in G.Map.MainDecorationGroups274){if(!group.Center)continue;groups++;
    Check(group.Items.Count>=2,"276 Central daily-object group contains several real objects "+group.Name);
    var r=group.Footprint;const float aisle=.60f;
    var corners=new[]{new Vector3(r.xMin-aisle,0,r.yMin-aisle),new Vector3(r.xMax+aisle,0,r.yMin-aisle),new Vector3(r.xMax+aisle,0,r.yMax+aisle),new Vector3(r.xMin-aisle,0,r.yMax+aisle)};
    MainWalk273(corners[0],"276 central object group aisle "+group.Name);
    for(int i=1;i<=4;i++){
     Check(G.Navigation.ClearSegment(G.Player.transform.position,corners[i%4]),"276 Actual central group can be passed on side "+i+" "+group.Name);
     MainWalk273(corners[i%4],"276 around daily object group "+group.Name);
    }
    yield return null;
   }
   Check(groups>0,"276 Resized interior contains a passable central furnishing group floor "+floor);
  }
  void RoomsMeeting276(){
   var room=G.Map.RepairMainRooms210.Find(r=>r.Name=="会议室");Check(room!=null,"276 Fourth-floor meeting room exists");
   var table=Array.Find(G.Map.Root.GetComponentsInChildren<Collider>(),c=>c.name=="RepairMeetingTable220"&&room.Area.Contains(new Vector2(c.bounds.center.x,c.bounds.center.z)));
   Check(table!=null&&Vector3.Distance(table.bounds.size,new Vector3(4.4f,.84f,1.4f))<.001f,"276 Actual meeting table retains physical dimensions");
   var occupied=table.bounds;int chairs=0;
   foreach(var t in G.Map.Root.GetComponentsInChildren<Transform>())if(t.name=="Decor_Chair"&&room.Area.Contains(new Vector2(t.position.x,t.position.z))){
    chairs++;Bounds chair;Check(RoomsBounds276(t.gameObject,out chair),"276 Meeting chair has actual rendered geometry");occupied.Encapsulate(chair);
   }
   Check(chairs==3,"276 Actual meeting room still has exactly three chairs");
   // Follow the local table-and-chair footprint. Room-wide corner goals would
   // also cross independent daily-object islands and would not measure its aisle.
   const float clearance=.50f;var ring=new[]{new Vector3(occupied.min.x-clearance,0,occupied.min.z-clearance),new Vector3(occupied.max.x+clearance,0,occupied.min.z-clearance),new Vector3(occupied.max.x+clearance,0,occupied.max.z+clearance),new Vector3(occupied.min.x-clearance,0,occupied.max.z+clearance)};
   MainWalk273(room.WalkPoint,"276 enter local meeting table aisle");MainWalk273(ring[0],"276 meeting table and chair aisle start");
   for(int i=1;i<=4;i++){
    var goal=ring[i%4];Check(room.Area.Contains(new Vector2(goal.x,goal.z))&&G.Navigation.ClearSegment(G.Player.transform.position,goal),"276 Complete table and chair group has direct .34m aisle on side "+i);
    MainWalk273(goal,"276 actual local meeting group circuit side "+i);
   }
   MainWalk273(room.ApproachPoint,"276 leave local meeting group");
  }
  IEnumerator RoomsWorld276(string name,bool top){
   Check(Shader.GetGlobalFloat("_HarborFogEnabled")==1,"276 World capture uses live fog "+name);yield return new WaitForEndOfFrame();
   var go=new GameObject("QA276ActualFogCamera");var cam=go.AddComponent<Camera>();cam.CopyFrom(G.View);cam.enabled=false;
   cam.transform.rotation=Quaternion.Euler(top?90:55,top?0:45,0);cam.transform.position=Vector3.up-cam.transform.forward*130;
   cam.orthographicSize=top?26:36;cam.farClipPlane=250;SaveYardCamera136(cam,name,2000,top?2200:1600);Destroy(go);yield return null;
  }
  IEnumerator RoomsGameView276(string name,Vector3 target,float yaw){
   G.CloseInventory();G.Rig.ResetView(yaw);G.Rig.Tick(0);target.y=RaidGame.CombatHeight;
   var cursor=G.View.WorldToScreenPoint(target);G.Controls=new TouchInput{AimPosition=new Vector2(cursor.x,cursor.y)};G.UpdateAim();
   G.Vision.Refresh(G.Player.transform.position,G.Player.Aim);G.Player.Draw(G.View,0);
   G.Paused=true;G.NoticeTime=0;
   yield return null;yield return new WaitForEndOfFrame();
   var intended=target-G.Player.transform.position;intended.y=0;Check(Vector3.Dot(G.Player.Aim,intended.normalized)>.999f,"276 Actual rendered flashlight stays aimed at requested target "+name);
   Check(!G.InventoryOpen&&Shader.GetGlobalFloat("_HarborFogEnabled")==1,"276 Actual game camera keeps flashlight and fog "+name);
   // Hidden player windows do not render a swapchain/HUD framebuffer. Render
   // the unchanged gameplay camera directly; these images contain no IMGUI.
   var rt=new RenderTexture(1600,900,24,RenderTextureFormat.ARGB32);var prior=RenderTexture.active;var targetTexture=G.View.targetTexture;
   G.View.targetTexture=rt;G.View.Render();RenderTexture.active=rt;
   var screenshot=new Texture2D(1600,900,TextureFormat.RGB24,false);screenshot.ReadPixels(new Rect(0,0,1600,900),0,0);screenshot.Apply();
   G.View.targetTexture=targetTexture;RenderTexture.active=prior;rt.Release();Destroy(rt);
   int visible=0;foreach(var pixel in screenshot.GetPixels32())if(pixel.r>10||pixel.g>10||pixel.b>10)visible++;
   File.WriteAllBytes(Path.Combine(folder,name),screenshot.EncodeToPNG());Destroy(screenshot);
   Check(visible>=1600*900/1000,"276 Actual game-view image contains non-black scene pixels "+name+" count="+visible);F111(false,0);yield return null;
  }
  IEnumerator RoomsSearch276(LootContainer box,List<string> rows){
   int count=box.Items.Count;Check(count>0&&WalkMain273(box.Position),"276 Actual player reaches preserved search stand "+box.Name);
   G.Player.Aim=(box.Prop.transform.position-G.Player.transform.position).normalized;G.Rig.Tick(0);G.Vision.Refresh(G.Player.transform.position,G.Player.Aim);
   Check(G.CanInteractLoot(box),"276 Furniture permits interaction with actual box "+box.Name);F111(false,0);F111(true,box.SearchDuration+.02f);
   Check(box.Opened&&G.OpenLoot==box&&box.Prop.Searched,"276 Real F opens intended box among new dressing "+box.Name);
   G.Paused=false;for(int n=0;n<7;n++)G.TickIdentification(3);G.Paused=true;
   Check(box.Items.Count==count&&box.Items.TrueForAll(i=>i.Identified),"276 Rooms leaves searched contents intact "+box.Name);
   var prop=box.Prop.transform.position;rows.Add(G.Map.RepairFloor220+","+Csv230(box.Name)+","+box.Position.x+","+box.Position.z+","+prop.x+","+prop.y+","+prop.z+","+count+",true");G.CloseInventory();F111(false,0);yield return null;
  }

  sealed class RoomState276 {
   public MainState273 Core;public bool[] Seen,SurfaceSeen;public float[] Armor,MaxArmor,MaxHealth;
  }
  float RoomsOldAxis276(float v,float middle,float factor){return Mathf.Abs(v)<=middle?v:Mathf.Sign(v)*(middle+(Mathf.Abs(v)-middle)*factor);}
  Rect RoomsOldArea276(int floor,int index){
   // Frozen 2.7.5 partitions. Do not derive the baseline from the new mapping.
   Rect[] first={new Rect(-13.29f,-15.79f,4.49f,23.59f),new Rect(-13.29f,7.8f,4.49f,7.99f),new Rect(9.8f,-11.5f,3.49f,6),new Rect(9.8f,-5.5f,3.49f,9),new Rect(9.8f,3.5f,3.49f,12.29f)};
   Rect[] second={new Rect(-13.29f,7.8f,4.49f,7.99f),new Rect(9.8f,-11.5f,3.49f,6),new Rect(-13.29f,-15.79f,10.69f,13.99f),new Rect(-13.29f,1.8f,10.69f,6),new Rect(2.6f,-5.5f,10.69f,3.7f),new Rect(2.6f,8.7f,10.69f,7.09f)};
   Rect[] upper={new Rect(-13.29f,7.8f,4.49f,7.99f),new Rect(9.8f,-11.5f,3.49f,6),new Rect(-13.29f,-15.79f,10.69f,7.39f),new Rect(-13.29f,-8.4f,10.69f,6.6f),new Rect(-13.29f,1.8f,10.69f,6),new Rect(2.6f,-15.79f,10.69f,4.29f),new Rect(2.6f,-5.5f,10.69f,3.7f),new Rect(2.6f,1.8f,10.69f,6.9f),new Rect(2.6f,8.7f,10.69f,7.09f)};
   var list=floor==1?first:floor==2?second:upper;Check(index<list.Length,"276 Room purpose has frozen 275 baseline "+floor+" / "+index);var r=list[index];
   return Rect.MinMaxRect(RoomsOldAxis276(r.xMin,2.6f,24.19f/10.69f),RoomsOldAxis276(r.yMin,1.8f,29.99f/13.99f),RoomsOldAxis276(r.xMax,2.6f,24.19f/10.69f),RoomsOldAxis276(r.yMax,1.8f,29.99f/13.99f));
  }
  int RoomsOldIndex276(int floor,string name){
   string[] first={"零件整理与待修件间","西北楼梯","东南楼梯","维修值班办公室","设备与配电间"};string[] second={"西北楼梯","东南楼梯","部件拆修车间","精密测试间","工具校验间","维修工艺与备件整理间"};
   string[] third={"西北楼梯","东南楼梯","电气装配室","继电与仪表测试室","技术设计办公室","维修记录登记室","工程资料档案室","技术会议室","配电监测办公室"};string[] fourth={"西北楼梯","东南楼梯","职工休息室","更衣与清洁室","管理办公室","值班管理室","会议室","人事与排班办公室","钥匙管理室"};
   int index=Array.IndexOf(floor==1?first:floor==2?second:floor==3?third:fourth,name);Check(index>=0,"276 Named purpose maps to original 275 room "+name);return index;
  }
  float RoomsAreaOf276(string name){var r=G.Map.RepairMainRooms210.Find(p=>p.Name==name);Check(r!=null,"276 Purpose room exists "+name);return r.Area.width*r.Area.height;}
  void RoomsPurpose276(int floor){
   var stairs=G.Map.RepairMainRooms210.FindAll(r=>r.ReservedStair);Check(stairs.Count==2,"276 Both human-scale stair rooms remain");foreach(var s in stairs)Check(Mathf.Abs(s.Area.width-5.64f)<.01f&&Mathf.Abs(s.Area.height-(s.Area.center.x<0?7.59f:7.8f))<.01f,"276 Compact stair enclosure retains usable landing area "+s.Name);
   if(floor==2)Check(RoomsAreaOf276("部件拆修车间")>RoomsAreaOf276("精密测试间")&&RoomsAreaOf276("精密测试间")>RoomsAreaOf276("工具校验间"),"276 Workshop and test space exceed small calibration room by purpose");
   if(floor==3)Check(RoomsAreaOf276("电气装配室")>RoomsAreaOf276("技术设计办公室")*2&&RoomsAreaOf276("技术会议室")>RoomsAreaOf276("配电监测办公室"),"276 Assembly and meeting use more area than individual offices");
   if(floor==4){float meeting=RoomsAreaOf276("会议室");Check(meeting>RoomsAreaOf276("管理办公室")*1.5f&&meeting>RoomsAreaOf276("更衣与清洁室")*1.5f&&meeting>RoomsAreaOf276("人事与排班办公室")*2&&meeting>RoomsAreaOf276("钥匙管理室")*2&&RoomsAreaOf276("职工休息室")>meeting,"276 Fourth-floor areas match meeting, small office, key and staff-rest uses");}
  }
  bool RoomsWithin276(Rect r,Bounds b){return b.min.x>=r.xMin-.006f&&b.max.x<=r.xMax+.006f&&b.min.z>=r.yMin-.006f&&b.max.z<=r.yMax+.006f;}
  void RoomsShell276(int floor,float zoom){
   var m=G.Map;var net=new Rect(-20.04f,-23.79f,40.08f,47.58f);
   Check(RectClose200(m.Bounds,new Rect(-20.25f,-24,40.5f,48))&&RectClose200(RaidMap.RepairMainNet210,net),"276 Exact 40.5 x 48m shell and 40.08 x 47.58m net floor "+floor);
   var ground=Array.Find(m.Root.GetComponentsInChildren<Renderer>(),r=>r.name=="RepairMainFloor210"||r.name=="RepairMainFloor220");
   Check(ground!=null&&Vector3.Distance(ground.bounds.size,new Vector3(40.08f,.24f,47.58f))<.001f,"276 Actual floor slab matches reduced physical net "+floor);
   var shell=m.Root.GetComponent<RepairWallModel220>();Check(shell&&shell.PerimeterWalls.Count==4,"276 Four physical perimeter walls "+floor);
   foreach(var r in shell.PerimeterWalls)Check(Vector4.Distance(r.sharedMaterial.GetVector("_RepairWallRect220"),new Vector4(-19.9f,-23.65f,19.9f,23.65f))<.001f,"276 Wall shader follows reduced perimeter "+r.name);
   var paint=m.StableGroundBounds137;Check(m.StableGround137&&Mathf.Abs(paint.xMin-net.xMin)<=.2f&&Mathf.Abs(paint.xMax-net.xMax)<=.2f&&Mathf.Abs(paint.yMin-net.yMin)<=.2f&&Mathf.Abs(paint.yMax-net.yMax)<=.2f,"276 Ground painting covers net within decorative border "+floor);
   foreach(float x in new[]{net.xMin+.65f,net.xMax-.65f})foreach(float z in new[]{net.yMin+.65f,net.yMax-.65f})Check(RepairGroundPixel149(m,new Vector3(x,0,z)).a>.5f,"276 Far-corner ground remains opaque "+floor+" "+x+"/"+z);
   Check(Mathf.Abs(G.View.orthographicSize-zoom)<.001f&&G.Player.transform.lossyScale==Vector3.one,"276 Rooms retain player scale and gameplay magnification "+floor);
   foreach(var d in m.Doors)if(!d.Portal)Check(Vector3.Distance(d.Collider.transform.lossyScale,new Vector3(.12f,2.3f,1.8f))<.001f,"276 Doors retain physical dimensions "+d.Name);
  }
  void RoomsAreas276(int floor,List<string> rows){
   var rooms=G.Map.RepairMainRooms210;Check(rooms.Count==(floor==1?5:floor==2?6:9),"276 Complete purpose-room count floor "+floor);float min=100,max=0,oldTotal=0,newTotal=0;
   for(int i=0;i<rooms.Count;i++){
    var room=rooms[i];var a=room.Area;var old=RoomsOldArea276(floor,RoomsOldIndex276(floor,room.Name));float prior=old.width*old.height,now=a.width*a.height,ratio=now/prior;oldTotal+=prior;newTotal+=now;
    Check(a.width>2&&a.height>3&&a.xMin>=RaidMap.RepairMainNet210.xMin-.001f&&a.xMax<=RaidMap.RepairMainNet210.xMax+.001f&&a.yMin>=RaidMap.RepairMainNet210.yMin-.001f&&a.yMax<=RaidMap.RepairMainNet210.yMax+.001f,"276 Purpose room fits physical floor "+floor+" "+room.Name);
    for(int j=0;j<i;j++){var b=rooms[j].Area;float dx=Mathf.Min(a.xMax,b.xMax)-Mathf.Max(a.xMin,b.xMin),dz=Mathf.Min(a.yMax,b.yMax)-Mathf.Max(a.yMin,b.yMin);Check(dx<=.001f||dz<=.001f,"276 Purpose rooms do not overlap "+room.Name+" / "+rooms[j].Name);}
    if(!room.ReservedStair){min=Mathf.Min(min,ratio);max=Mathf.Max(max,ratio);}rows.Add(floor+","+Csv230(room.Name)+","+room.ReservedStair+","+prior+","+now+","+ratio+","+a.xMin+","+a.yMin+","+a.width+","+a.height);
   }
   Check(max-min>.04f,"276 Purpose areas redistribute rather than uniformly shrink floor "+floor+" ratios="+min+".."+max);RoomsPurpose276(floor);
   rows.Add(floor+",open_hall_and_circulation,false,"+(53.58f*63.58f-oldTotal)+","+(40.08f*47.58f-newTotal)+",,"+RaidMap.RepairMainNet210.xMin+","+RaidMap.RepairMainNet210.yMin+",,");
  }
  bool RoomsPublicPoint276(Vector3 p){
   var net=RaidMap.RepairMainNet210;
   return p.x>=net.xMin+.4f&&p.x<=net.xMax-.4f&&p.z>=net.yMin+.4f&&p.z<=net.yMax-.4f&&Clear140(p)&&!G.Map.RepairMainRooms210.Exists(r=>r.Area.Contains(new Vector2(p.x,p.z)));
  }
  void RoomsPublicFog276(int floor){
   // Derive candidates from physical capsule clearance and actual private rooms,
   // never from the public reveal rectangles whose completeness is under test.
   Physics.SyncTransforms();var net=RaidMap.RepairMainNet210;int tested=0,missing=0,bridge=0;
   var points=new List<Vector3>();
   for(float x=net.xMin+.5f;x<=net.xMax-.4f;x+=.5f)for(float z=net.yMin+.5f;z<=net.yMax-.4f;z+=.5f)points.Add(new Vector3(x,0,z));
   // Probe the visibility-cell centres straddling the southeast public junction.
   foreach(float z in new[]{-15.0625f,-14.9375f}){var p=new Vector3(13.4f,0,z);if(RoomsPublicPoint276(p)){points.Add(p);bridge++;}}
   foreach(var p in points){
    if(!RoomsPublicPoint276(p))continue;tested++;
    bool allowed=G.Vision.RoomAllowed(p),explored=G.Vision.Explored(p);
    if(allowed&&explored)continue;missing++;
    if(missing<=30)Debug.Log("276 PUBLIC FOG GAP floor="+floor+" point="+p+" visionCell="+G.Vision.Index(p)+" RoomAllowed="+allowed+" Explored="+explored);
   }
   File.AppendAllText(Path.Combine(folder,"276-public-fog-coverage.csv"),floor+","+tested+","+missing+","+bridge+",0.5,0.34"+Environment.NewLine);
   Check(tested>0&&missing==0,"276 All actual standable public floor is allowed and explored before room doors open floor "+floor+" samples="+tested+" missing="+missing+" preciseBridgeSamples="+bridge);
  }
  void RoomsWallOverlap276(int floor){
   var walls=new List<Collider>();foreach(var c in G.Map.Root.GetComponentsInChildren<Collider>())if(c.enabled&&c.name=="InteriorWall"&&c.bounds.min.y<=.005f&&c.bounds.size.y>=2.9f)walls.Add(c);
   int overlap=0;for(int i=0;i<walls.Count;i++)for(int j=0;j<i;j++){
    var a=walls[i].bounds;var b=walls[j].bounds;bool horizontal=a.size.x>a.size.z;
    if(horizontal!=(b.size.x>b.size.z))continue;
    if(Mathf.Abs(horizontal?a.center.z-b.center.z:a.center.x-b.center.x)>.005f)continue;
    float along=horizontal?Mathf.Min(a.max.x,b.max.x)-Mathf.Max(a.min.x,b.min.x):Mathf.Min(a.max.z,b.max.z)-Mathf.Max(a.min.z,b.min.z);
    float across=horizontal?Mathf.Min(a.max.z,b.max.z)-Mathf.Max(a.min.z,b.min.z):Mathf.Min(a.max.x,b.max.x)-Mathf.Max(a.min.x,b.min.x);
    if(along<=.005f||across<=.005f)continue;overlap++;
    if(overlap<=30)Debug.Log("276 COLLINEAR WALL OVERLAP floor="+floor+" axis="+(horizontal?"x":"z")+" along="+along+" thicknessOverlap="+across+" a="+a+" b="+b);
   }
   Check(overlap==0,"276 Actual floor-level collinear main walls have no duplicate overlap over 5mm floor "+floor+" walls="+walls.Count+" overlaps="+overlap);
  }
  void RoomsWallCollision276(){
   var colliders=G.Map.Root.GetComponentsInChildren<Collider>();foreach(var c in colliders){
    if(!c.enabled||c.name=="InteriorWall"||c.name.Contains("Door")||c.name.Contains("Floor")||(SceneObstacle.MovementMask&(1<<c.gameObject.layer))==0)continue;
    Check(RoomsWithin276(RaidMap.RepairMainNet210,c.bounds),"276 Actual furniture/search/stair collision stays within ground "+c.name+" "+c.bounds);
    foreach(var wall in colliders)if(wall.enabled&&wall.name=="InteriorWall"&&c.bounds.Intersects(wall.bounds)){
     Vector3 direction;float depth;bool overlap=Physics.ComputePenetration(c,c.transform.position,c.transform.rotation,wall,wall.transform.position,wall.transform.rotation,out direction,out depth);
     Check(!overlap||depth<=.005f,"276 Physical object avoids solid wall penetration "+c.name+" wall="+wall.bounds+" depth="+depth);
    }
   }
  }
  void RoomsActors276(int floor,List<string> rows){
   Check(G.Map.MainPatrolPlans276.Count==G.Enemies.Count,"276 Every actual floor NPC has a placement and patrol record "+floor);
   foreach(var e in G.Enemies){
    var plan=G.Map.MainPatrolPlans276.Find(p=>Vector3.Distance(p.Home,e.Home)<.001f);Check(plan!=null&&e.NpcLevel==plan.Level&&e.IsRebel==plan.Soldier&&(plan.Soldier?e.Rebel==plan.Rebel:e.IsMonster&&e.Monster==plan.Monster),"276 Actual NPC class and defense level match intended room role "+e.name);
    var room=G.Map.RepairMainRooms210.Find(r=>r.Name==plan.RoomName);
    foreach(var p in e.PatrolTargets)Check(room!=null?room.Area.Contains(new Vector2(p.x,p.z)):RaidMap.RepairMainNet210.Contains(new Vector2(p.x,p.z))&&!G.Map.RepairMainRooms210.Exists(r=>r.Area.Contains(new Vector2(p.x,p.z))),"276 Actual patrol stays in its named room or hall "+e.name+" "+plan.RoomName+" "+p);
    for(int i=0;i<e.PatrolTargets.Length;i++)rows.Add(floor+","+Csv230(e.name)+","+Csv230(plan.RoomName)+","+(e.IsRebel?e.Rebel.ToString():e.Monster.ToString())+","+e.NpcLevel+","+e.Home.x+","+e.Home.z+","+e.NpcArmorPoints+","+e.Health+","+e.NpcMaxArmor+","+e.NpcMaxHealth+","+i+","+e.PatrolTargets[i].x+","+e.PatrolTargets[i].z);
   }
  }
  RoomState276 RoomsSnapshot276(){
   var s=new RoomState276{Core=MainSnapshot273(),Seen=(bool[])G.Vision.Seen.Clone(),SurfaceSeen=(bool[])G.Vision.SurfaceSeen.Clone()};s.Armor=Array.ConvertAll(s.Core.Actors,e=>e.NpcArmorPoints);s.MaxArmor=Array.ConvertAll(s.Core.Actors,e=>e.NpcMaxArmor);s.MaxHealth=Array.ConvertAll(s.Core.Actors,e=>e.NpcMaxHealth);return s;
  }
  void RoomsCache276(RoomState276 s){
   MainCache273(s.Core);bool memory=true;for(int i=0;i<s.Seen.Length;i++)memory&=(!s.Seen[i]||G.Vision.Seen[i])&&(!s.SurfaceSeen[i]||G.Vision.SurfaceSeen[i]);Check(memory&&G.Vision.RoomGate&&Shader.GetGlobalTexture("_HarborVisibility")==G.Vision.Texture,"276 Cached exploration and surface fog survive floor revisit "+G.Map.RepairFloor220);
   foreach(var room in G.Map.RepairMainRooms210)if(room.Door.Locked){var center=new Vector3(room.Area.center.x,0,room.Area.center.y);Check(!room.Door.Open&&!G.Vision.RoomAllowed(center)&&!G.Vision.Explored(center),"276 Unentered locked room remains black across cache revisit "+room.Name);}
   for(int i=0;i<s.Core.Actors.Length;i++){var e=G.Enemies[i];Check(e.NpcArmorPoints==s.Armor[i]&&e.NpcMaxArmor==s.MaxArmor[i]&&e.NpcMaxHealth==s.MaxHealth[i],"276 Cached NPC damage and maximum pools persist "+e.name);}
  }
  void RoomsWound276(){
   // Make only the two damaged states required for the cache probe; no combat suite.
   var e=G.Enemies[0];float hp=e.Health;G.ApplyCombatHit(e,0,e.NpcArmorPoints,0,e.NpcLevel-1);G.ApplyCombatHit(e,18,10,0,e.NpcLevel-1);Check(e.NpcArmorPoints==0&&e.Health==hp-18&&!e.Dead,"276 Cache fixture has actual exposed NPC HP wound");
   e=G.Enemies[1];float armor=e.NpcArmorPoints;G.ApplyCombatHit(e,18,10,0,e.NpcLevel-1);Check(e.NpcArmorPoints==armor-10&&e.NpcArmorPoints>0&&!e.Dead,"276 Cache fixture has partial NPC armor damage");
  }
  IEnumerator RoomsCloseViews276(int floor){
   var room=G.Map.RepairMainRooms210.Find(r=>r.Name=="维修值班办公室"||r.Name=="技术设计办公室"||r.Name=="管理办公室");
   if(room!=null){MainWalk273(room.WalkPoint,"276 office camera stand");yield return RoomsGameView276("276-floor"+floor+"-ordinary-office-game-view.png",new Vector3(room.Area.center.x,0,room.Area.center.y),45);}
   if(floor==1){var group=G.Map.MainDecorationGroups274.Find(g=>g.Center&&g.RoomName=="维修大厅");Check(group!=null,"276 Hall contains actual central daily objects");MainWalk273(new Vector3(group.Footprint.center.x,0,group.Footprint.yMin-.6f),"276 hall close-view stand");yield return RoomsGameView276("276-floor1-hall-game-view.png",group.Anchor,45);}
  }
  IEnumerator VerifyMainRooms276(){
   string directory=Path.GetDirectoryName(G.SavePath);string[] formal={Path.Combine(directory,"stash-v40.json"),Path.Combine(directory,"stash-v41.json")};byte[][] before=Array.ConvertAll(formal,p=>File.Exists(p)?File.ReadAllBytes(p):null);Check(Path.GetFileName(G.SavePath)=="verification-stash-2.7.6.json","276 Room verification uses isolated inventory");
   File.WriteAllLines(Path.Combine(folder,"276-capture-notes.txt"),new[]{"Actual runtime cameras render directly with the real 70-degree flashlight and black fog retained; no HUD/IMGUI.","Area CSV freezes 2.7.5 per-purpose partitions. Open hall/circulation is net slab minus named rooms, before wall thickness.","Only resized rooms and affected furnishing, circulation, interaction, patrol and caching are exercised."});File.WriteAllText(Path.Combine(folder,"276-public-fog-coverage.csv"),"floor,standableSamples,unexploredSamples,preciseBridgeSamples,gridPitch,capsuleRadius"+Environment.NewLine);
   FreshCamp();string camp=JsonUtility.ToJson(G.Save);G.HardMode=false;G.HarborWalkthrough=true;Check(G.BeginTraining(),"276 Start actual harbor preview");G.Paused=true;yield return null;var outside=G.Map;float zoom=G.View.orthographicSize;var floors=new RoomState276[4];int boxes=0,enemies=0;
   var areas=new List<string>{"floor,purpose,stair,area275,area276,ratio,xMin,zMin,width,depth"};var items=new List<string>{"floor,room,name,type,xMin,zMin,width,depth,obstacleKind,wallMounted"};var furniture=new List<string>{"floor,room,name,placement,rearGap,centerX,centerZ,width,depth"};var searches=new List<string>{"floor,name,standX,standZ,propX,propY,propZ,itemCount,actualFOpened"};var patrol=new List<string>{"floor,actor,actualDistance60seconds,collision"};var positions=new List<string>{"floor,actor,room,kind,level,homeX,homeZ,armor,health,maxArmor,maxHealth,patrolIndex,targetX,targetZ"};var types=new HashSet<string>();
   G.Player.transform.position=RaidMap.RepairMainApproach220(1);Physics.SyncTransforms();yield return MainPortal273(outside.Doors.Find(d=>d.Destination=="repair-entry:"+RaidMap.RepairMainGateKeys220[1]),"276 initial entrance");
   for(int floor=1;floor<=4;floor++){
    Check(G.Map.RepairMain220&&G.Map.RepairFloor220==floor,"276 Actual resized floor "+floor);RoomsShell276(floor,zoom);RoomsAreas276(floor,areas);RoomsPublicFog276(floor);RoomsWallOverlap276(floor);File.WriteAllLines(Path.Combine(folder,"276-room-areas.csv"),areas);Check(G.Map.Loot.Count==(floor==2?8:floor==4?6:7)&&G.Enemies.Count==(floor==1||floor==4?3:2),"276 Per-floor container and actor counts "+floor);
    RoomsWallCollision276();RoomsWallFurniture276(furniture);RoomsShadow276();RoomsItems276(items,types);RoomsSearchPlacement276();yield return MainRooms273(floor);if(floor==4)RoomsMeeting276();yield return RoomsGroups276(floor);MainPatrol273(floor,patrol);enemies+=G.Enemies.Count;
    foreach(var box in G.Map.Loot){yield return RoomsSearch276(box,searches);boxes++;}foreach(var d in G.Map.Doors)if(d.Portal)MainWalk273(d.Destination.StartsWith("repair-exit:")?d.Position+Vector3.forward:d.Position,"276 actual portal access "+d.Name);
    yield return RoomsCloseViews276(floor);yield return RoomsWorld276("276-floor"+floor+"-overview-live-fog.png",false);yield return RoomsWorld276("276-floor"+floor+"-plan-live-fog.png",true);if(floor==1)RoomsWound276();
    RoomsActors276(floor,positions);File.WriteAllLines(Path.Combine(folder,"276-enemy-positions.csv"),positions);File.WriteAllLines(Path.Combine(folder,"276-box-searches.csv"),searches);File.WriteAllLines(Path.Combine(folder,"276-enemy-patrols.csv"),patrol);floors[floor-1]=RoomsSnapshot276();if(floor<4)yield return MainStair273(floor+1,true);
   }
   Check(boxes==28&&enemies==10,"276 All 28 real F searches and 10 actual patrols pass");for(int f=3;f>=1;f--){yield return MainStair273(f,false);RoomsCache276(floors[f-1]);}for(int f=2;f<=4;f++){yield return MainStair273(f,false);RoomsCache276(floors[f-1]);}for(int f=3;f>=1;f--){yield return MainStair273(f,true);RoomsCache276(floors[f-1]);}
   for(int entry=0;entry<3;entry++){var exit=G.Map.Doors.Find(d=>d.Destination=="repair-exit:"+RaidMap.RepairMainGateKeys220[entry]);Check(exit!=null,"276 All entrances have interior exits "+entry);MainWalk273(exit.Position+Vector3.forward,"276 exit approach "+entry);yield return MainPortal273(exit,"276 real exit "+entry);Check(G.Map==outside,"276 Exterior restores after selected exit "+entry);G.Player.transform.position=RaidMap.RepairMainApproach220(entry);Physics.SyncTransforms();yield return MainPortal273(outside.Doors.Find(d=>d.Destination=="repair-entry:"+RaidMap.RepairMainGateKeys220[entry]),"276 real entrance "+entry);RoomsCache276(floors[0]);}
   File.WriteAllLines(Path.Combine(folder,"276-room-areas.csv"),areas);File.WriteAllLines(Path.Combine(folder,"276-decoration-occupancy.csv"),items);File.WriteAllLines(Path.Combine(folder,"276-wall-furniture.csv"),furniture);File.WriteAllLines(Path.Combine(folder,"276-box-searches.csv"),searches);File.WriteAllLines(Path.Combine(folder,"276-enemy-patrols.csv"),patrol);File.WriteAllLines(Path.Combine(folder,"276-enemy-positions.csv"),positions);G.Controls=new DesktopInput();G.Finish(false);yield return null;Check(JsonUtility.ToJson(G.Save)==camp,"276 Preview restores exact camp inventory");for(int i=0;i<formal.Length;i++)Check(MainBytes273(before[i],File.Exists(formal[i])?File.ReadAllBytes(formal[i]):null),"276 Formal save stays byte-for-byte unchanged "+Path.GetFileName(formal[i]));
  }
 }
}
