using System;
using System.IO;
using UnityEngine;
using UnityEditor;
using GreyHarbor;
public static class FutureAssetExport2717 {
 public static void Repackage(){const string dir="Assets/Environment/FutureAssetKit2717";string output=Path.GetFullPath(Path.Combine(Application.dataPath,"../../QA/FutureAssetKit_2.7.17.unitypackage"));AssetDatabase.ExportPackage(new[]{dir,"Assets/Scripts/Runtime/FutureAssetCatalog2717.cs","Assets/Scripts/Runtime/FutureAssetInfo2717.cs","Assets/Scripts/Runtime/SceneObstacle.cs","Assets/Resources/HarborWorld.shader"},output,ExportPackageOptions.Recurse|ExportPackageOptions.IncludeDependencies);Debug.Log("FUTURE ASSET REPACKAGE 2717 COMPLETE: explicit enum/catalog dependency");}
 public static void Build(){
  const string dir="Assets/Environment/FutureAssetKit2717";Directory.CreateDirectory(dir+"/Meshes");Directory.CreateDirectory(dir+"/Materials");AssetDatabase.Refresh();
  var map=new RaidMap{Root=new GameObject("FutureAssetLibrary2717").transform};var kit=new FutureAssetKit2717(map);
  try{foreach(var spec in FutureAssetCatalog2717.All){
   var view=kit.Build(spec.Id,Vector3.zero);int n=0;
   foreach(var f in view.GetComponentsInChildren<MeshFilter>()){string path=dir+"/Meshes/"+spec.Id+"_"+n+++".asset";var saved=AssetDatabase.LoadAssetAtPath<Mesh>(path);if(!saved){saved=UnityEngine.Object.Instantiate(f.sharedMesh);AssetDatabase.CreateAsset(saved,path);}else EditorUtility.CopySerialized(f.sharedMesh,saved);f.sharedMesh=saved;}
   foreach(var r in view.GetComponentsInChildren<Renderer>()){var mats=r.sharedMaterials;for(int i=0;i<mats.Length;i++){var m=mats[i];if(!m||AssetDatabase.Contains(m))continue;string path=dir+"/Materials/"+spec.Id+"_"+n+++".mat";var saved=AssetDatabase.LoadAssetAtPath<Material>(path);if(!saved){saved=new Material(m);AssetDatabase.CreateAsset(saved,path);}else EditorUtility.CopySerialized(m,saved);mats[i]=saved;}r.sharedMaterials=mats;}
   foreach(var owned in view.GetComponentsInChildren<WorkshopArtResources>()){foreach(var mesh in owned.Meshes)if(mesh&&!AssetDatabase.Contains(mesh))UnityEngine.Object.DestroyImmediate(mesh);foreach(var material in owned.Materials)if(material&&!AssetDatabase.Contains(material))UnityEngine.Object.DestroyImmediate(material);owned.Meshes.Clear();owned.Materials.Clear();UnityEngine.Object.DestroyImmediate(owned);}
   var prefab=PrefabUtility.SaveAsPrefabAsset(view,dir+"/"+spec.Id+".prefab");if(!prefab||!prefab.GetComponent<FutureAssetInfo2717>())throw new Exception("Missing reusable prefab metadata: "+spec.Id);
   foreach(var child in prefab.GetComponentsInChildren<Transform>(true))if(GameObjectUtility.GetMonoBehavioursWithMissingScriptCount(child.gameObject)!=0)throw new Exception("Missing prefab script: "+spec.Id);
   foreach(var filter in prefab.GetComponentsInChildren<MeshFilter>())if(!filter.sharedMesh||!AssetDatabase.Contains(filter.sharedMesh))throw new Exception("Unstored mesh: "+spec.Id);
   foreach(var renderer in prefab.GetComponentsInChildren<Renderer>())foreach(var material in renderer.sharedMaterials)if(!material||!material.shader||!AssetDatabase.Contains(material))throw new Exception("Unstored material/shader: "+spec.Id);
  }AssetDatabase.SaveAssets();string output=Path.GetFullPath(Path.Combine(Application.dataPath,"../../QA/FutureAssetKit_2.7.17.unitypackage"));Directory.CreateDirectory(Path.GetDirectoryName(output));AssetDatabase.ExportPackage(dir,output,ExportPackageOptions.Recurse|ExportPackageOptions.IncludeDependencies);Debug.Log("FUTURE ASSET EXPORT 2717 COMPLETE: "+FutureAssetCatalog2717.All.Length);
  }finally{UnityEngine.Object.DestroyImmediate(map.Root.gameObject);}
 }
}
