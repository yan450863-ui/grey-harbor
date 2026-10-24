using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
namespace GreyHarbor {
 public sealed partial class RaidVerification {
  readonly List<string> annexRows2718=new List<string>();
  void AnnexWalk2718(Vector3 point,string label){bool ok=WalkMain273(point);Check(ok&&CargoClear2713(point),"2718 附属房实际行走/胶囊通过："+label+" "+mainWalkFailure273);}
  void AnnexDoorF2718(WorldDoor door,bool portal=false){
   Check(door!=null,"2718 附属房交互门存在");if(door==null)return;
   SprintInteract2711(false,0,door.Position);SprintInteract2711(false,0,door.Position);Check(G.NearbyDoor==door,"2718 附属房新容器不抢房门F："+door.Name);
   SprintInteract2711(true,portal?2.03f:.01f,door.Position);SprintInteract2711(false,0,door.Position);
  }
  void AnnexPatrol2718(){
   Check(G.Enemies.Count==2,"2718 附属房保留原2敌人，没有新增敌人");
   foreach(var actor in G.Enemies){var position=actor.transform.position;bool moving=actor.MonsterMoving;try{
    Check(actor.PatrolTargets!=null&&actor.PatrolTargets.Length>=2,"2718 原NPC巡逻点保留："+actor.name);
    if(actor.PatrolTargets==null)continue;actor.transform.position=actor.Home;Physics.SyncTransforms();
    var destinations=new List<Vector3>(actor.PatrolTargets);destinations.Add(actor.Home);
    foreach(var target in destinations){Check(CargoClear2713(target)&&G.Navigation.ClearSegment(actor.transform.position,target),"2718 新容器不挡原NPC巡逻胶囊路径："+actor.name+" "+target);
     int steps=0;bool clear=true;while(Vector3.Distance(actor.transform.position,target)>.02f&&steps++<1000){var previous=actor.transform.position;actor.Move(Vector3.ClampMagnitude(target-previous,.04f));Physics.SyncTransforms();if(Vector3.Distance(previous,actor.transform.position)<.00001f||!CargoClear2713(actor.transform.position)){clear=false;break;}}
     Check(clear&&Vector3.Distance(actor.transform.position,target)<=.02f,"2718 原NPC实际Actor.Move通过既有巡逻段："+actor.name);
    }
   }finally{actor.transform.position=position;actor.MonsterMoving=moving;Physics.SyncTransforms();}}
  }
  IEnumerator RunAnnexContainers2718(){
   Check(G.Phase==GamePhase.Raid&&G.Map!=null&&!G.Map.IsInterior&&G.HarborWalkthrough,"2718 附属房专项在独立港口预览运行");
   var outdoor=G.Map;var outsideActors=G.Enemies.ToArray();var controls=G.Controls;bool paused=G.Paused;var bag=G.Bag.Snapshot();annexRows2718.Clear();annexRows2718.Add("房间,容器,x,z,用途,掉落件数,拾取数量");
   try{
    G.Paused=true;G.Controls=new TouchInput();FreightRoomPlace2717(RaidMap.RepairAnnexApproach200,"附属房独立门口夹具");var entrance=outdoor.Doors.Find(d=>d.Portal&&d.Destination=="repairannex");AnnexDoorF2718(entrance,true);yield return null;
    var indoor=G.Map;Check(indoor!=outdoor&&indoor.InteriorId=="repairannex"&&CargoClear2713(G.Player.transform.position),"2718 真实长按F进入维修附属房并安全落地");
    Check(indoor.Loot.Count==8&&indoor.AnnexLootRecords2718.Count==8,"2718 附属房恰好8个已接入实际搜索容器");
    foreach(var room in indoor.RepairRooms200){Check(!room.Door.Open&&!G.Vision.RoomAllowed(room.WalkPoint),"2718 侧房初始关门保持未探索："+room.Name);var outside=room.Door.Position-Vector3.right;var inside=room.Door.Position+Vector3.right*1.1f;
     AnnexWalk2718(outside,room.Name+"门外");AnnexDoorF2718(room.Door);Check(room.Door.Open&&G.Vision.RoomAllowed(room.WalkPoint)&&G.Navigation.ClearSegment(outside,inside),"2718 实际F开启侧房且门洞可穿行："+room.Name);AnnexWalk2718(inside,room.Name+"穿门");AnnexWalk2718(room.WalkPoint,room.Name+"工作区");AnnexWalk2718(outside,room.Name+"离开");
     AnnexDoorF2718(room.Door);Check(!room.Door.Open,"2718 新容器不阻止侧房正常关闭："+room.Name);AnnexDoorF2718(room.Door);Check(room.Door.Open,"2718 侧房关闭后可重新打开："+room.Name);yield return null;
    }
    AnnexPatrol2718();var unique=new HashSet<LootContainer>();var content=new List<string>();var pickedIds=new List<string>();int hall=0,tools=0,rest=0,wash=0;
    foreach(var record in indoor.AnnexLootRecords2718){var box=record.Container;if(record.RoomName=="大维修大厅")hall++;else if(record.RoomName=="工具与备件间")tools++;else if(record.RoomName=="工人休息室")rest++;else if(record.RoomName=="更衣洗漱区")wash++;
     Check(unique.Add(box)&&indoor.Loot.Contains(box)&&box.View&&box.Prop&&box.DropProfile230!=null&&box.DropProfile230.Tier==ContainerTier.Common&&box.DropProfile230.Region==DropRegion230.Repair&&box.DropProfile230.FixedCount==0&&box.DropProfile230.Look!=ContainerLook.Safe,"2718 附属房真实容器沿用维修区Common用途掉落："+box.Name);
     var area=record.RoomName=="大维修大厅"?RaidMap.RepairHall200:indoor.RepairRooms200.Find(r=>r.Name==record.RoomName).Area;foreach(var renderer in box.View.GetComponentsInChildren<Renderer>())if(renderer.enabled){var bounds=renderer.bounds;Check(bounds.min.x>=area.xMin-.02f&&bounds.max.x<=area.xMax+.02f&&bounds.min.z>=area.yMin-.02f&&bounds.max.z<=area.yMax+.02f,"2718 容器模型位于对应房间内部："+box.Name+"/"+renderer.name);}
     foreach(var obstacle in box.View.GetComponentsInChildren<Collider>())if(obstacle.enabled&&!obstacle.isTrigger){var bounds=obstacle.bounds;float margin=.006f;var query=Physics.OverlapBox(bounds.center,bounds.extents-Vector3.one*margin,Quaternion.identity,SceneObstacle.MovementMask,QueryTriggerInteraction.Ignore);bool noClipping=true;foreach(var hit in query)if(!hit.transform.IsChildOf(box.View.transform))noClipping=false;Check(noClipping,"2718 容器实体不嵌入既有家具/墙体："+box.Name+"/"+obstacle.name);}
     AnnexWalk2718(record.AccessPoint,box.Name+"接近通道");Check(G.Navigation.ClearSegment(record.AccessPoint,box.Position),"2718 容器搜索站位接近线可走："+box.Name);AnnexWalk2718(box.Position,box.Name+"搜索站位");
     G.Rig.Tick(0);G.Player.Aim=(box.Prop.transform.position-G.Player.transform.position).normalized;G.Vision.Refresh(G.Player.transform.position,G.Player.Aim);Check(G.CanInteractLoot(box),"2718 容器真实可见且在F范围内："+box.Name);int count=box.Items.Count;string before=JsonUtility.ToJson(new CargoItems2715{Items=box.Items.ToArray()});
     SprintInteract2711(false,0,box.Prop.transform.position);SprintInteract2711(false,0,box.Prop.transform.position);Check(G.Nearby==box,"2718 F准确选中附属房容器："+box.Name);SprintInteract2711(true,box.SearchDuration+.02f,box.Prop.transform.position);Check(box.Opened&&box.Prop.Searched&&G.OpenLoot==box&&before==JsonUtility.ToJson(new CargoItems2715{Items=box.Items.ToArray()}),"2718 真实搜索打开且没有重抽内容："+box.Name);
     G.Paused=false;for(int i=0;i<7;i++)G.TickIdentification(3);G.Paused=true;Check(count>0&&box.Items.TrueForAll(i=>i.Identified),"2718 容器真实掉落鉴定完成："+box.Name);
     G.Bag.Slots.Clear();int picked=0;string id="";if(box.Items.Count>0){var item=box.Items[0];picked=item.Count;id=item.Id;var kind=item.Kind;int sourceCell=item.Cell;int destinationCell=G.Bag.FreeCell(kind);
      Check(destinationCell>=0&&G.CanMoveItem(ItemArea.Loot,sourceCell,ItemArea.Backpack,destinationCell),"2718 实际拾取预检查：已鉴定掉落可放入连续背包空位："+box.Name);
      // Inventory commits are intentionally disabled while the game is paused; perform the
      // actual player operation during an unpaused synchronous step, without ticking NPCs.
      bool transferred=false;G.Paused=false;try{transferred=G.MoveItem(ItemArea.Loot,sourceCell,ItemArea.Backpack,destinationCell);}finally{G.Paused=true;}
      Check(transferred&&G.Bag.Count(kind)==picked&&G.Bag.Slots.Exists(i=>i.Id==id&&i.Kind==kind&&i.Count==picked)&&!box.Items.Exists(i=>i.Id==id),"2718 实际拾取容器掉落进背包并扣除原物品："+box.Name+" transfer="+transferred+" kind="+kind+" expected="+picked+" carried="+G.Bag.Count(kind));}
     pickedIds.Add(id);content.Add(JsonUtility.ToJson(new CargoItems2715{Items=box.Items.ToArray()}));annexRows2718.Add(Csv230(record.RoomName)+","+Csv230(box.Name)+","+CargoNumber2713(box.View.transform.position.x)+","+CargoNumber2713(box.View.transform.position.z)+","+Csv230(box.DropProfile230.Usage.ToString())+","+count+","+picked);G.CloseInventory();SprintInteract2711(false,0,box.Prop.transform.position);AnnexWalk2718(record.AccessPoint,box.Name+"离开");yield return null;
    }
    Check(hall==4&&tools==2&&rest==1&&wash==1,"2718 八容器分布大厅4/工具2/休息1/洗漱1，中央维修区保持空地");
    foreach(var point in new[]{new Vector3(-2.5f,0,-8.05f),new Vector3(-2.5f,0,0),new Vector3(-2.5f,0,6.8f),new Vector3(3.1f,0,6.8f),new Vector3(3.1f,0,-6.1f)})AnnexWalk2718(point,"大厅中央和侧房通行带");
    AnnexWalk2718(new Vector3(-3.1f,0,1),"正常黑雾实镜头站位");G.Rig.ResetView(0);G.Rig.Tick(0);G.Player.Aim=Vector3.forward;G.Vision.Refresh(G.Player.transform.position,G.Player.Aim);G.Player.Draw(G.View,0);yield return null;var picture=HvRender277(G.View,"2718-annex-containers-live-fog.png",1600,900);Destroy(picture);
    yield return CargoPicture2715("2718-annex-containers-placement.png",new Vector3(0,.3f,0),0,12,true,1600,1600);
    var actors=G.Enemies.ToArray();var vision=G.Vision;var seen=(bool[])vision.Seen.Clone();var exit=indoor.Doors.Find(d=>d.Portal);AnnexWalk2718(indoor.Spawn,"走回附属房出口");AnnexDoorF2718(exit,true);yield return null;Check(G.Map==outdoor&&Array.TrueForAll(outsideActors,e=>G.Enemies.Contains(e))&&G.Enemies.Count==outsideActors.Length,"2718 实际退出保留原室外地图和敌人身份");
    AnnexDoorF2718(entrance,true);yield return null;Check(G.Map==indoor&&G.Vision==vision&&G.Map.Loot.Count==8&&G.Enemies.Count==2&&Array.TrueForAll(actors,e=>G.Enemies.Contains(e)),"2718 二次长按F进入恢复原附属房/8箱/2敌人且不重复生成");
    bool memory=true;for(int i=0;i<seen.Length;i++)if(seen[i]&&!vision.Seen[i])memory=false;Check(memory,"2718 附属房探索缓存跨图保留");foreach(var room in indoor.RepairRooms200)Check(room.Door.Open,"2718 附属房侧门开关缓存保留："+room.Name);
    for(int i=0;i<indoor.AnnexLootRecords2718.Count;i++){var box=indoor.AnnexLootRecords2718[i].Container;Check(box.Opened&&box.Prop.Searched&&content[i]==JsonUtility.ToJson(new CargoItems2715{Items=box.Items.ToArray()})&&!box.Items.Exists(item=>item.Id==pickedIds[i]),"2718 跨图保持搜索/鉴定/拾取扣除，没有重刷："+box.Name);}
    AnnexWalk2718(indoor.Spawn,"缓存附属房出口");AnnexDoorF2718(exit,true);yield return null;Check(G.Map==outdoor,"2718 专项结束正常回到港口");
   }finally{G.CloseInventory();G.Bag.Restore(bag);G.Controls=controls;G.Paused=paused;File.WriteAllLines(Path.Combine(folder,"2718-annex-containers-actual.csv"),annexRows2718);}
  }
 }
}
