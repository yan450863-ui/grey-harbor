using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
namespace GreyHarbor {
 public sealed class FreightOperationExhibit2719 {public string Type,Name;public GameObject View;public Vector3 Anchor,Approach;public Rect Footprint;}
 public sealed partial class RaidMap {
  public readonly List<FreightOperationExhibit2719> FreightOperationExhibits2719=new List<FreightOperationExhibit2719>();
  public GameObject FreightOperationExample2719(string type,Vector3 anchor,float yaw){
   var view=type=="ForkliftParking"?CargoSimple2714("Forklift",anchor,yaw):FreightOperationModel2719(type,anchor,yaw);
   view.transform.SetParent(Root,true);view.name="FreightOperationGallery2719_"+type;Physics.SyncTransforms();
   float min=float.MaxValue;foreach(var r in view.GetComponentsInChildren<Renderer>())min=Mathf.Min(min,r.bounds.min.y);
   view.transform.position+=Vector3.up*(.055f-min);Physics.SyncTransforms();
   var marker=view.AddComponent<FreightOperation2719>();marker.Equipment=type;marker.Purpose="独立素材地图实际碰撞、颜色与旋转复用样例";marker.Units=type=="DispatchPallets"||type=="RollCageBay"||type=="ReturnPalletBay"?2:1;marker.DesignAnchor=view.transform.position;
   FreightOperationBake2719(view);return view;
  }
  public void BuildFreightOperationGallery2719(){
   if(!AssetShowcase2717||FreightOperationExhibits2719.Count>0)return;
   string[] types={"DispatchPallets","RollCageBay","ReturnPalletBay","ForkliftParking","DispatchBench","WheelServiceBay","LoadingStaging"};
   string[] names={"待运捆扎货物","周转笼车","周转托盘","叉车停位","清点打包台","车轮维护台","装卸暂存位"};
   for(int i=0;i<types.Length;i++){
    var p=new Vector3(37+(i%4)*11,0,-45-(i/4)*10);var view=FreightOperationExample2719(types[i],p,0);var rect=CargoBounds2714(view);
    FreightOperationExhibits2719.Add(new FreightOperationExhibit2719{Type=types[i],Name=names[i],View=view,Anchor=view.transform.position,Approach=new Vector3(p.x,0,rect.yMin-1.0f),Footprint=rect});
    GalleryLabel2717(names[i],new Vector3(p.x,.071f,rect.yMin-.45f),.11f,"D3D7C2");
   }
   GalleryLabel2717("货运作业设备 · 2.7.19",new Vector3(52,.071f,-40),.18f,"D3D7C2");
   ShowcaseSectorStarts2717.Add(new Vector3(31,0,-43));Physics.SyncTransforms();
  }
 }
 public sealed partial class RaidVerification {
  Bounds FreightGalleryBounds2719(GameObject view){bool any=false;var bounds=new Bounds();foreach(var r in view.GetComponentsInChildren<MeshRenderer>())if(r.enabled){if(!any){bounds=r.bounds;any=true;}else bounds.Encapsulate(r.bounds);}return bounds;}
  void FreightGalleryInspect2719(GameObject view,string name,HashSet<string> palette){
   bool meshOK=true,materialOK=true,colored=false,layersOK=true;int visible=0,triangles=0,solid=0;
   foreach(var f in view.GetComponentsInChildren<MeshFilter>()){
    var r=f.GetComponent<MeshRenderer>();if(!r||!r.enabled)continue;visible++;var mesh=f.sharedMesh;if(!mesh){meshOK=false;continue;}
    var v=mesh.vertices;var t=mesh.triangles;triangles+=t.Length/3;meshOK&=v.Length>=3&&t.Length>=3&&t.Length%3==0&&mesh.normals.Length==v.Length;
    foreach(var p in v)meshOK&=GalleryFinite2717(p);foreach(int index in t)meshOK&=index>=0&&index<v.Length;
    foreach(var m in r.sharedMaterials){if(!m||!m.shader||!m.HasProperty("_Color")){materialOK=false;continue;}var c=m.color;materialOK&=GalleryFinite2717(new Vector3(c.r,c.g,c.b))&&c.a>.99f&&m.shader==GreyHarbor.FlashlightShaders2763.Load("HarborWorld");colored|=Mathf.Max(c.r,c.g,c.b)-Mathf.Min(c.r,c.g,c.b)>.035f;palette.Add(ColorUtility.ToHtmlStringRGB(c));}
   }
   foreach(var c in view.GetComponentsInChildren<Collider>()){
    if(!c.enabled)continue;solid++;var rule=c.GetComponentInParent<SceneObstacle>();layersOK&=rule&&!c.isTrigger&&GalleryFinite2717(c.bounds.center)&&GalleryFinite2717(c.bounds.size);
    if(rule)layersOK&=c.gameObject.layer==(rule.Kind==ObstacleKind.High?SceneObstacle.HighLayer:SceneObstacle.LowLayer);
    foreach(var other in Physics.OverlapBox(c.bounds.center,c.bounds.extents*.98f,Quaternion.identity,SceneObstacle.MovementMask,QueryTriggerInteraction.Ignore))if(!other.transform.IsChildOf(view.transform))layersOK=false;
   }
   var b=FreightGalleryBounds2719(view);
   Check(visible>0&&meshOK&&triangles>0,"2719 独立货运样例合批后有效网格："+name+" draws="+visible+" triangles="+triangles);
   Check(materialOK&&colored,"2719 独立货运样例使用正常彩色材质："+name);
   Check(solid>0&&layersOK,"2719 独立货运样例有实际碰撞、层级正确且不与相邻展品穿插："+name);
   Check(GalleryFinite2717(b.center)&&GalleryFinite2717(b.size)&&Mathf.Abs(b.min.y-.055f)<.009f,"2719 独立货运样例落地，不悬空或埋地："+name+" bottom="+b.min.y);
  }
  void FreightGalleryWalkRing2719(GameObject view,string name){
   var b=FreightGalleryBounds2719(view);const float clearance=.95f;
   var ring=new[]{new Vector3(b.min.x-clearance,0,b.min.z-clearance),new Vector3(b.max.x+clearance,0,b.min.z-clearance),new Vector3(b.max.x+clearance,0,b.max.z+clearance),new Vector3(b.min.x-clearance,0,b.max.z+clearance),new Vector3(b.min.x-clearance,0,b.min.z-clearance)};
   GalleryFixture2717(ring[0],name+"绕行起点");for(int i=1;i<ring.Length;i++){Check(G.Navigation.ClearSegment(ring[i-1],ring[i]),"2719 新货运样例外沿0.34米玩家胶囊实际路径："+name+"/"+i);GalleryWalk2717(ring[i],name+"外沿实际Actor.Move/"+i);}
  }
  IEnumerator RunFreightOperationGallery2719(){
   int start=results.Count;var map=G.Map;var palette=new HashSet<string>();
   Check(map!=null&&map.AssetShowcase2717&&G.AssetShowcaseMode2717&&G.TrainingActive,"2719 仅在独立素材地图测试七种新货运作业模型");if(map==null||!map.AssetShowcase2717)yield break;
   int permanent=map.FreightOperationExhibits2719.Count,loot=map.Loot.Count,oldRecords=map.ShowcaseRecords2717.Count;bool paused=G.Paused;var originalPlayer=G.Player.transform.position;
   try{
    G.Paused=true;Check(permanent==7,"2719 独立测试区七种新货运作业模型全部实例化");
    GalleryFixture2717(new Vector3(31,0,-43),"新货运设备专区入口");GalleryWalk2717(new Vector3(31,0,-58),"新专区纵向通道");
    foreach(var record in map.FreightOperationExhibits2719){
     FreightGalleryInspect2719(record.View,record.Name,palette);FreightGalleryWalkRing2719(record.View,record.Name);yield return null;
    }
    Check(palette.Count>=15,"2719 七种新货运样例颜色正常、有蓝绿木色与设备黄，颜色数="+palette.Count);
    foreach(var record in map.FreightOperationExhibits2719){
     if(record.Type!="ForkliftParking"&&record.Type!="RollCageBay")continue;
     var initial=FreightGalleryBounds2719(record.View);record.View.SetActive(false);GameObject extra=null;
     try{
      extra=map.FreightOperationExample2719(record.Type,new Vector3(record.Anchor.x,0,record.Anchor.z),90);Physics.SyncTransforms();yield return null;
      var rotated=FreightGalleryBounds2719(extra);Check(Mathf.Abs(initial.size.x-rotated.size.z)<.04f&&Mathf.Abs(initial.size.z-rotated.size.x)<.04f,"2719 叉车/笼车90度复用模型实际包围盒正确交换："+record.Name);
      FreightGalleryInspect2719(extra,record.Name+" 90度",palette);FreightGalleryWalkRing2719(extra,record.Name+" 90度");
     }finally{if(extra){extra.SetActive(false);UnityEngine.Object.Destroy(extra);}record.View.SetActive(true);Physics.SyncTransforms();}
     yield return null;Check(map.Root.GetComponentsInChildren<FreightOperation2719>().Length==7,"2719 临时旋转测试模型清理后仅保留七件独立样例："+record.Name);
    }
    Check(map.ShowcaseRecords2717.Count==oldRecords&&map.Loot.Count==loot&&map.FreightOperationExhibits2719.Count==permanent,"2719 新样例不改变旧展示记录或八个搜索容器");
    yield return GalleryPicture2717("2719-freight-operation-gallery.png",new Vector3(52,.7f,-49),15,58,0,2000,1300);
    GalleryFixture2717(new Vector3(31,0,-43),"测试后安全通道");
   }finally{
    G.Player.transform.position=originalPlayer;Physics.SyncTransforms();G.Rig.Tick(0);G.Paused=paused;
    var report=new List<string>();for(int i=start;i<results.Count;i++)report.Add(results[i]);report.Add("INFO Scope: seven new freight operation exhibits only; material/mesh/layer/real Actor.Move perimeter, forklift/cage 90-degree reuse, cleanup. Old gallery assets are not retested.");File.WriteAllLines(Path.Combine(folder,"2719-freight-operation-gallery-verification.txt"),report);
   }
  }
 }
}
