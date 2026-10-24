using UnityEngine;
using UnityEditor;
using System.IO;
using GreyHarbor;
public static class OutdoorContainerExport140 {
 public static void Build(){
  const string dir="Assets/Environment/OutdoorContainers140";Directory.CreateDirectory(dir+"/Materials");AssetDatabase.Refresh();var map=new RaidMap();map.Root=new GameObject("ContainerLibrary").transform;var art=new ContainerArt(map);
  foreach(ContainerLook look in System.Enum.GetValues(typeof(ContainerLook)))if(OutdoorContainers140.IsOutdoor(look))art.Build(look,Vector3.zero,0,false);
  foreach(Transform child in map.Root){foreach(var r in child.GetComponentsInChildren<Renderer>()){var m=r.sharedMaterial;if(AssetDatabase.Contains(m))continue;string key=ColorUtility.ToHtmlStringRGBA(m.color)+"_"+m.GetFloat("_SolidFog").ToString("0");string path=dir+"/Materials/"+key+".mat";var saved=AssetDatabase.LoadAssetAtPath<Material>(path);if(saved==null){saved=new Material(m);AssetDatabase.CreateAsset(saved,path);}r.sharedMaterial=saved;}PrefabUtility.SaveAsPrefabAsset(child.gameObject,dir+"/"+child.name+".prefab");}
  Object.DestroyImmediate(map.Root.gameObject);AssetDatabase.SaveAssets();string output=Path.GetFullPath(Path.Combine(Application.dataPath,"../../QA/OutdoorContainers_1.40.unitypackage"));AssetDatabase.ExportPackage(dir,output,ExportPackageOptions.Recurse|ExportPackageOptions.IncludeDependencies);Debug.Log("CONTAINERS EXPORTED: "+output);
 }
}
