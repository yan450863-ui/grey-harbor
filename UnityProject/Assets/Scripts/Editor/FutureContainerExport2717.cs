using System;
using System.IO;
using System.Text;
using System.Globalization;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using GreyHarbor;

// Appearance-only library. Loot lives on RaidMap, not inside a prefab.
public static class FutureContainerExport2717 {
 const string Directory2717="Assets/Environment/FutureSearchContainers2717";
 static string Q(string s)=>"\""+(s??"").Replace("\"","\"\"")+"\"";
 static string F(float value)=>value.ToString("R",CultureInfo.InvariantCulture);
 static string V(Vector3 p)=>F(p.x)+","+F(p.y)+","+F(p.z);
 static void ClearOwners(GameObject view,HashSet<UnityEngine.Object> transient){
  foreach(var owner in view.GetComponentsInChildren<WorkshopArtResources>(true)){
   foreach(var mesh in owner.Meshes)if(mesh&&!AssetDatabase.Contains(mesh))transient.Add(mesh);
   foreach(var material in owner.Materials)if(material&&!AssetDatabase.Contains(material))transient.Add(material);
   owner.Meshes.Clear();owner.Materials.Clear();UnityEngine.Object.DestroyImmediate(owner);
  }
 }
 public static void Build(){
  Directory.CreateDirectory(Directory2717+"/Meshes");Directory.CreateDirectory(Directory2717+"/Materials");AssetDatabase.Refresh();
  var map=new RaidMap{Root=new GameObject("FutureSearchContainerExport2717").transform};var transient=new HashSet<UnityEngine.Object>();var paths=new List<string>();
  var rows=new List<string>{"Kind,Name,Look,SupplyType,Region,Tier,RelativeStandX,RelativeStandY,RelativeStandZ,RelativeAccessX,RelativeAccessY,RelativeAccessZ,Width,Height,Depth,Usage,Prefab,Registration"};
  try{
   var rng=new System.Random(2717008);
   for(int i=0;i<8;i++){
    var record=map.BuildFutureSearchContainer2717((FutureContainerKind2717)i,Vector3.zero,0,rng);var view=record.View;string id=record.Kind.ToString();
    // Search state is held by the runtime LootContainer. Remove it deliberately:
    // a prefab must not pretend it is searchable without map registration.
    foreach(var prop in view.GetComponentsInChildren<ContainerProp>(true)){
     var script=MonoScript.FromMonoBehaviour(prop);bool serializable=script&&script.GetClass()==typeof(ContainerProp);
     Debug.Log("FUTURE CONTAINER EXPORT "+id+": ContainerProp script="+(serializable?"valid":"unavailable")+"; removed for appearance-only prefab");
     UnityEngine.Object.DestroyImmediate(prop);
    }
    // CSV keeps the eight logical types; remove kit metadata so this appearance
    // package does not require the complete 112-type catalog to compile on import.
    foreach(var info in view.GetComponentsInChildren<FutureAssetInfo2717>(true))UnityEngine.Object.DestroyImmediate(info);
    var interaction=view.transform.Find("Body");if(interaction&&!interaction.GetComponent<Renderer>().enabled){foreach(var material in interaction.GetComponent<Renderer>().sharedMaterials)if(material&&!AssetDatabase.Contains(material))transient.Add(material);UnityEngine.Object.DestroyImmediate(interaction.gameObject);}
    var meshes=new Dictionary<Mesh,Mesh>();int meshIndex=0;
    foreach(var filter in view.GetComponentsInChildren<MeshFilter>(true)){
     var original=filter.sharedMesh;if(!original)throw new Exception("Missing generated mesh: "+id);
     if(!meshes.TryGetValue(original,out var saved)){
      string path=Directory2717+"/Meshes/"+id+"_Mesh_"+(meshIndex++)+".asset";saved=AssetDatabase.LoadAssetAtPath<Mesh>(path);
      if(!saved){saved=UnityEngine.Object.Instantiate(original);saved.name=id+"_Mesh_"+(meshIndex-1);AssetDatabase.CreateAsset(saved,path);}else EditorUtility.CopySerialized(original,saved);
      meshes.Add(original,saved);if(!AssetDatabase.Contains(original))transient.Add(original);
     }filter.sharedMesh=saved;
    }
    var materials=new Dictionary<Material,Material>();int materialIndex=0;
    foreach(var renderer in view.GetComponentsInChildren<Renderer>(true)){
     var array=renderer.sharedMaterials;for(int n=0;n<array.Length;n++){
      var original=array[n];if(!original||!original.shader)throw new Exception("Missing generated material/shader: "+id);
      if(!materials.TryGetValue(original,out var saved)){
       string path=Directory2717+"/Materials/"+id+"_Material_"+(materialIndex++)+".mat";saved=AssetDatabase.LoadAssetAtPath<Material>(path);
       if(!saved){saved=new Material(original);AssetDatabase.CreateAsset(saved,path);}else EditorUtility.CopySerialized(original,saved);
       materials.Add(original,saved);if(!AssetDatabase.Contains(original))transient.Add(original);
      }array[n]=saved;
     }renderer.sharedMaterials=array;
    }
    ClearOwners(view,transient);
    foreach(var component in view.GetComponentsInChildren<MonoBehaviour>(true)){
     if(!component)throw new Exception("Missing source script: "+id);var script=MonoScript.FromMonoBehaviour(component);
     if(!script||script.GetClass()!=component.GetType())throw new Exception("Nonserializable source component: "+id+" / "+component.GetType().Name);
    }
    string prefabPath=Directory2717+"/"+id+".prefab";var prefab=PrefabUtility.SaveAsPrefabAsset(view,prefabPath);if(!prefab)throw new Exception("Failed prefab save: "+id);
    foreach(var child in prefab.GetComponentsInChildren<Transform>(true))if(GameObjectUtility.GetMonoBehavioursWithMissingScriptCount(child.gameObject)!=0)throw new Exception("Missing prefab scripts: "+id);
    if(prefab.GetComponentsInChildren<ContainerProp>(true).Length!=0||prefab.GetComponentsInChildren<WorkshopArtResources>(true).Length!=0)throw new Exception("Unexpected runtime search/resource owner: "+id);
    foreach(var filter in prefab.GetComponentsInChildren<MeshFilter>(true))if(!filter.sharedMesh||!AssetDatabase.Contains(filter.sharedMesh))throw new Exception("Unstored prefab mesh: "+id);
    foreach(var renderer in prefab.GetComponentsInChildren<Renderer>(true))foreach(var material in renderer.sharedMaterials)if(!material||!material.shader||!AssetDatabase.Contains(material))throw new Exception("Unstored prefab material/shader: "+id);
    paths.Add(prefabPath);rows.Add(id+","+Q(record.Name)+","+record.Look+","+record.Type+","+record.Region+",Common,"+V(record.Stand-record.Model)+","+V(record.AccessPoint-record.Model)+","+V(record.VisualBounds.size)+","+record.Container.DropProfile230.Usage+","+Q(prefabPath)+","+Q("RaidMap.BuildFutureSearchContainer2717(kind,model,yaw,rng)"));
    UnityEngine.Object.DestroyImmediate(view);
   }
   string readme=Directory2717+"/README2717.txt";
   File.WriteAllText(readme,"灰港行动2.7.17：8种搜索容器外观资源。\n包含原生Prefab、Mesh、Material及必要Shader/有效碰撞脚本依赖；类型、用途和尺寸元数据保存在CSV。\n预制件仅为外观与分件碰撞，不含Loot、搜索内容或可独立使用的搜索组件；ContainerProp、隐藏交互包络、112类kit元数据组件及临时资源owner已移除。\n实际搜索注册请在完整游戏工程使用 RaidMap.BuildFutureSearchContainer2717(kind,model,yaw,rng)，它创建唯一精细外观并注册现有Common规则。不要把本外观再与AddContainer外壳叠加。\n相对Stand/Access和实际外观尺寸见FutureSearchContainers2717.csv；它们由原运行时生成函数取得，未凭手工近似填写。\n展厅使用隔离训练，真实物品不带回营地；这些外观不表示正式双子楼已投放。\n",new UTF8Encoding(false));
   string csv=Directory2717+"/FutureSearchContainers2717.csv";File.WriteAllLines(csv,rows,new UTF8Encoding(false));AssetDatabase.Refresh();AssetDatabase.SaveAssets();
   string output=Path.GetFullPath(Path.Combine(Application.dataPath,"../../QA/FutureSearchContainers_2.7.17.unitypackage"));Directory.CreateDirectory(Path.GetDirectoryName(output));
   File.WriteAllLines(Path.Combine(Path.GetDirectoryName(output),"2717-future-search-container-prefabs.csv"),rows,new UTF8Encoding(false));
   AssetDatabase.ExportPackage(Directory2717,output,ExportPackageOptions.Recurse|ExportPackageOptions.IncludeDependencies);
   Debug.Log("FUTURE SEARCH CONTAINER EXPORT 2717 COMPLETE: "+paths.Count+" appearance prefabs; "+output);
  }finally{
   if(map.Root){foreach(var owner in map.Root.GetComponentsInChildren<WorkshopArtResources>(true))ClearOwners(owner.gameObject,transient);UnityEngine.Object.DestroyImmediate(map.Root.gameObject);}
   foreach(var resource in transient)if(resource&&!AssetDatabase.Contains(resource))UnityEngine.Object.DestroyImmediate(resource);
  }
 }
}
