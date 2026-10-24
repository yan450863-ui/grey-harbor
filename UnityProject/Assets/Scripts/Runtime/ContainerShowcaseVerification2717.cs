using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
namespace GreyHarbor {
 public sealed partial class RaidVerification {
  string FutureContents2717(LootContainer box)=>JsonUtility.ToJson(new CargoItems2715{Items=box.Items.ToArray()});
  void FutureSelect2717(FutureContainerRecord2717 record){
   G.CloseInventory();SprintInteract2711(false,0,record.Model);SprintInteract2711(false,0,record.Model);
   Check(G.Nearby==record.Container&&G.NearbyDoor==null,"2717 长按F精确选择新容器，无邻近抢目标："+record.Name);
  }
  void FutureContainerArtCheck2717(FutureContainerRecord2717 record){
   var box=record.Container;Check(record.View&&record.View.activeInHierarchy&&box.Prop&&box.View==record.View&&box.Prop.gameObject==record.View,"2717 搜索逻辑与唯一真实模型对应："+record.Name);
   var realBounds=new Bounds();bool first=true,colored=false;int renderCount=0;var colors=new HashSet<string>();
   foreach(var renderer in record.View.GetComponentsInChildren<Renderer>())if(renderer.enabled){if(first){realBounds=renderer.bounds;first=false;}else realBounds.Encapsulate(renderer.bounds);renderCount++;foreach(var material in renderer.sharedMaterials){Check(material&&material.shader.name=="GreyHarbor/World","2717 新容器使用实际世界/黑雾材质："+record.Name);var c=material.color;colors.Add(ColorUtility.ToHtmlStringRGB(c));colored|=Mathf.Max(c.r,c.g,c.b)-Mathf.Min(c.r,c.g,c.b)>.06f;}}
   Check(renderCount>0&&colored&&colors.Count>=4,"2717 新容器保留有色漆、木材、标签与金属细节："+record.Name);
   Check(Mathf.Abs(realBounds.min.y-.055f)<.01f&&Vector3.Distance(realBounds.center,record.VisualBounds.center)<.002f&&Vector3.Distance(realBounds.size,record.VisualBounds.size)<.002f,"2717 真实模型着地，记录来自实际外观："+record.Name);
   var bound=G.LootBounds(box);Check(Vector3.Distance(bound.center,realBounds.center)<.01f&&Vector3.Distance(bound.size,realBounds.size)<.03f,"2717 F交互包络吻合真实模型，不能用站位虚构远程箱子："+record.Name);
   float radius=0;foreach(var renderer in record.View.GetComponentsInChildren<Renderer>())if(renderer.enabled){var b=renderer.bounds;foreach(float x in new[]{b.min.x,b.max.x})foreach(float z in new[]{b.min.z,b.max.z})radius=Mathf.Max(radius,Vector2.Distance(new Vector2(x,z),new Vector2(record.Model.x,record.Model.z)));}Check(radius<=1.6f,"2717 新容器真实平面半径不超过1.6米："+record.Name+" radius="+radius);
   var colliders=record.View.GetComponentsInChildren<Collider>();Check(colliders.Length>0,"2717 体积容器有真实碰撞："+record.Name);foreach(var collider in colliders){var obstacle=collider.GetComponent<SceneObstacle>();Check(collider.enabled&&!collider.isTrigger&&obstacle&&obstacle.Kind!=ObstacleKind.Decoration&&collider.gameObject.layer==(obstacle.Kind==ObstacleKind.High?SceneObstacle.HighLayer:SceneObstacle.LowLayer),"2717 新容器分件碰撞使用自身高低层："+record.Name+" / "+collider.name);}
   var reference=ContainerDropRules230.Profile(record.Look,record.Type,ContainerTier.Common,record.Region);var profile=box.DropProfile230;
   Check(profile!=null&&profile.Tier==ContainerTier.Common&&box.Tier==ContainerTier.Common&&profile.FixedCount==0&&profile.SafeSize==SafeSize240.Legacy&&profile.Usage==reference.Usage&&profile.PrimaryCategory==reference.PrimaryCategory&&profile.MaxWidth==reference.MaxWidth&&profile.MaxHeight==reference.MaxHeight,"2717 新用途沿用旧Common数量、品质和合法尺寸，无新品高爆率："+record.Name);
   for(int c=0;c<9;c++)Check(profile.AllowedCategories[c]==reference.AllowedCategories[c],"2717 原用途类别白名单保持："+record.Name+" / "+c);
   foreach(var item in box.Items)Check((int)item.Kind>=0&&(int)item.Kind<RaidInventory.KindCount&&ContainerDropRules230.Eligible(profile,item.Kind,RaidInventory.Grade(item.Kind)==4),"2717 实际内容来自既有物品ID及用途："+record.Name+" / "+item.Kind);
  }
  // Called inside the already-started isolated gallery training. The outer
  // runner owns extraction/restoration; this suite never starts another session.
  IEnumerator RunContainerShowcase2717(){
   int start=results.Count;var map=G.Map;var controls=G.Controls;bool paused=G.Paused;var rows=new List<string>{"id,name,modelX,modelZ,standX,standZ,accessX,accessZ,type,look,usage,count"};
   Check(map!=null&&map.AssetShowcase2717&&G.AssetShowcaseMode2717&&G.TrainingActive,"2717 新容器仅在隔离素材展厅训练中试玩");
   Check(map.FutureContainerRecords2717.Count==8&&map.Loot.Count==8&&G.Enemies.Count==0,"2717 独立展厅恰8类普通可搜容器，不生成战斗怪物");
   bool fileExists=File.Exists(G.SavePath);byte[] saveBefore=fileExists?File.ReadAllBytes(G.SavePath):null;var cached=new Dictionary<LootContainer,string>();var ids=new HashSet<string>();
   try{
    G.Paused=true;GalleryFixture2717(new Vector3(31,0,-13.5f),"新容器展区起点");
    foreach(var record in map.FutureContainerRecords2717){
     Check(ids.Add(record.Id)&&map.Loot.Contains(record.Container),"2717 新容器名单唯一且注册真实Loot："+record.Name);FutureContainerArtCheck2717(record);
     Check(CargoClear2713(record.AccessPoint)&&CargoClear2713(record.Stand)&&G.Navigation.ClearSegment(record.AccessPoint,record.Stand),"2717 接入点与站位具备真实.34胶囊净空："+record.Name);
     GalleryWalk2717(record.AccessPoint,record.Name+" 接入通道");GalleryWalk2717(record.Stand,record.Name+" 实际搜索站位");
     var box=record.Container;string original=FutureContents2717(box);int count=box.Items.Count;Check(count>0,"2717 常规内容实际生成："+record.Name);FutureSelect2717(record);Check(G.CanInteractLoot(box),"2717 真实模型在F范围内且视线可达："+record.Name);
     SprintInteract2711(true,box.SearchDuration+.03f,record.Model);Check(box.Opened&&box.Prop.Searched&&G.InventoryOpen&&G.OpenLoot==box&&FutureContents2717(box)==original,"2717 真实长按F搜索/打开，不重掷内容："+record.Name);
     G.Paused=false;for(int i=0;i<7;i++)G.TickIdentification(3);G.Paused=true;Check(box.Items.Count==count&&box.Items.TrueForAll(item=>item.Identified),"2717 真实容器全部物品完成识别："+record.Name);cached.Add(box,FutureContents2717(box));
     rows.Add(record.Id+","+Csv230(record.Name)+","+WN279(record.Model.x)+","+WN279(record.Model.z)+","+WN279(record.Stand.x)+","+WN279(record.Stand.z)+","+WN279(record.AccessPoint.x)+","+WN279(record.AccessPoint.z)+","+record.Type+","+record.Look+","+box.DropProfile230.Usage+","+count);
     G.CloseInventory();SprintInteract2711(false,0,record.Model);GalleryWalk2717(record.AccessPoint,record.Name+" 搜索后退回通道");yield return null;
    }
    // Real close/reopen and changing between all eight cached UI inventories.
    for(int i=map.FutureContainerRecords2717.Count-1;i>=0;i--){var record=map.FutureContainerRecords2717[i];GalleryWalk2717(record.AccessPoint,record.Name+" 返回缓存入口");GalleryWalk2717(record.Stand,record.Name+" 返回缓存站位");FutureSelect2717(record);SprintInteract2711(true,.02f,record.Model);Check(G.Map==map&&G.OpenLoot==record.Container&&G.InventoryOpen&&record.Container.Prop.Searched&&FutureContents2717(record.Container)==cached[record.Container],"2717 切换/关闭/再次打开保留同一箱身份、内容和识别缓存："+record.Name);G.CloseInventory();SprintInteract2711(false,0,record.Model);GalleryWalk2717(record.AccessPoint,record.Name+" 缓存退回通道");yield return null;}
    var pickup=map.FutureContainerRecords2717[0];GalleryWalk2717(pickup.Stand,"独立库存拾取站位");FutureSelect2717(pickup);SprintInteract2711(true,.02f,pickup.Model);var source=pickup.Container.Items[0];var sourceKind=source.Kind;int amount=source.Count,carryBefore=G.Bag.Count(sourceKind)+G.Chest.Count(sourceKind)+G.Safe.Count(sourceKind);G.Paused=false;
    Check(G.QuickTransfer(ItemArea.Loot,source.Cell),"2717 真实已识别容器物品可进入临时试玩库存");G.Paused=true;
    Check(G.Bag.Count(sourceKind)+G.Chest.Count(sourceKind)+G.Safe.Count(sourceKind)==carryBefore+amount,"2717 真实拾取数量守恒，外层撤离负责清除临时收获");
    Check(G.TrainingActive&&File.Exists(G.SavePath)==fileExists&&MainBytes273(saveBefore,fileExists?File.ReadAllBytes(G.SavePath):null),"2717 展厅真实拾取不写持久库存文件");
    G.CloseInventory();SprintInteract2711(false,0,pickup.Model);GalleryWalk2717(pickup.AccessPoint,"拍照前退回安全通道");
    yield return GalleryPicture2717("2717-search-container-showcase.png",new Vector3(52.5f,.6f,-10),12,45,0,2400,1000);
    yield return GalleryPicture2717("2717-office-search-containers-detail.png",new Vector3(42.5f,.7f,-10),5.5f,40,15,1800,1200);
    yield return GalleryPicture2717("2717-loading-tunnel-search-containers-detail.png",new Vector3(62.5f,.7f,-10),5.5f,40,15,1800,1200);
    Check(G.Enemies.Count==0&&map.Loot.Count==8,"2717 搜索及UI缓存测试不增加怪物或额外箱子");
   }finally{G.CloseInventory();G.Controls=controls;G.Paused=paused;File.WriteAllLines(Path.Combine(folder,"2717-future-search-containers.csv"),rows);var report=new List<string>();for(int i=start;i<results.Count;i++)report.Add(results[i]);report.Add("INFO 范围：8类新搜索容器实际模型、分件碰撞、F搜索、识别、UI缓存切换、临时拾取及Camera.Render；未投放双子办公楼，未新增正式物品/怪物或历史经济大回归。外层训练撤离验证库存完整恢复。");File.WriteAllLines(Path.Combine(folder,"2717-future-search-container-verification.txt"),report);}
  }
 }
}
