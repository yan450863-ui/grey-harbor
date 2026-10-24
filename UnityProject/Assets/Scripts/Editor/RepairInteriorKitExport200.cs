using UnityEngine;
using UnityEditor;
using GreyHarbor;
using System.IO;
// Called by the main version build entry. It does not build or launch a player itself.
public static class RepairInteriorKitExport200 {
 public static void Export(){
  const string dir="Assets/Environment/RepairInteriorKit200";
  Directory.CreateDirectory(dir+"/Materials");Directory.CreateDirectory(dir+"/Meshes");AssetDatabase.Refresh();
  var map=new RaidMap{Root=new GameObject("RepairInteriorLibrary200").transform};
  try {
   var kit=new RepairInteriorKit200(map);
   kit.PartsRack(Vector3.zero);kit.ToolWallBench(Vector3.zero);kit.EngineBench(Vector3.zero);kit.WashStation(Vector3.zero);kit.WorkLockers(Vector3.zero);
   foreach(Transform child in map.Root){
    int index=0;
    foreach(var f in child.GetComponentsInChildren<MeshFilter>()){
     if(f.sharedMesh==null||AssetDatabase.Contains(f.sharedMesh))continue;
     string path=dir+"/Meshes/"+child.name+"_"+(index++)+".asset";
     var saved=AssetDatabase.LoadAssetAtPath<Mesh>(path);
     if(saved==null){saved=Object.Instantiate(f.sharedMesh);AssetDatabase.CreateAsset(saved,path);}else EditorUtility.CopySerialized(f.sharedMesh,saved);
     f.sharedMesh=saved;
    }
    foreach(var r in child.GetComponentsInChildren<Renderer>()){
     var material=r.sharedMaterial;if(material==null||AssetDatabase.Contains(material))continue;
     string path=dir+"/Materials/"+ColorUtility.ToHtmlStringRGBA(material.color)+"_"+material.GetFloat("_SolidFog")+".mat";
     var saved=AssetDatabase.LoadAssetAtPath<Material>(path);
     if(saved==null){saved=new Material(material);AssetDatabase.CreateAsset(saved,path);}else EditorUtility.CopySerialized(material,saved);
     r.sharedMaterial=saved;
    }
    // Keep persisted mesh/material copies; remove generated ownership components before saving.
    foreach(var owned in child.GetComponentsInChildren<WorkshopArtResources>()){
     foreach(var mesh in owned.Meshes)if(mesh!=null&&!AssetDatabase.Contains(mesh))Object.DestroyImmediate(mesh);
     foreach(var material in owned.Materials)if(material!=null&&!AssetDatabase.Contains(material))Object.DestroyImmediate(material);
     owned.Meshes.Clear();owned.Materials.Clear();Object.DestroyImmediate(owned);
    }
    PrefabUtility.SaveAsPrefabAsset(child.gameObject,dir+"/"+child.name+".prefab");
   }
   AssetDatabase.SaveAssets();
   string output=Path.GetFullPath(Path.Combine(Application.dataPath,"../../QA/RepairInteriorKit_2.0.unitypackage"));
   Directory.CreateDirectory(Path.GetDirectoryName(output));
   AssetDatabase.ExportPackage(dir,output,ExportPackageOptions.Recurse|ExportPackageOptions.IncludeDependencies);
   Debug.Log("REPAIR INTERIOR 2.0 EXPORT COMPLETE: "+output);
  } finally {Object.DestroyImmediate(map.Root.gameObject);}
 }
}
