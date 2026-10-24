using UnityEngine;
using UnityEditor;
using System.IO;
using GreyHarbor;
public static class GroundMaterialExport {
 public static void Build(){
  const string dir="Assets/Environment/GroundMaterialsV1";
  Directory.CreateDirectory(dir+"/Meshes");Directory.CreateDirectory(dir+"/Materials");AssetDatabase.Refresh();
  foreach(var info in GroundMaterialCatalog.All){
   var map=new RaidMap();map.Root=new GameObject("Ground_"+info.Finish).transform;
   var art=new GroundSurfaceArt(map,117000+(int)info.Finish);art.Region(new Rect(-3,-3,6,6),info.Finish);art.Bake("Surface");
   foreach(var f in map.Root.GetComponentsInChildren<MeshFilter>()){
    string path=dir+"/Meshes/"+info.Finish+"_"+f.sharedMesh.name.Replace("|","_")+".asset";
    var saved=AssetDatabase.LoadAssetAtPath<Mesh>(path);if(saved==null){saved=Object.Instantiate(f.sharedMesh);AssetDatabase.CreateAsset(saved,path);}else EditorUtility.CopySerialized(f.sharedMesh,saved);f.sharedMesh=saved;
   }
   foreach(var r in map.Root.GetComponentsInChildren<MeshRenderer>()){
    var m=r.sharedMaterial;string path=dir+"/Materials/"+ColorUtility.ToHtmlStringRGBA(m.color)+".mat";
    var saved=AssetDatabase.LoadAssetAtPath<Material>(path);if(saved==null){saved=new Material(m);AssetDatabase.CreateAsset(saved,path);}r.sharedMaterial=saved;
   }
   // Prefab meshes/materials are persistent assets; the lifetime component only owns temporary originals.
   foreach(var owned in map.Root.GetComponentsInChildren<WorkshopArtResources>())Object.DestroyImmediate(owned);
   PrefabUtility.SaveAsPrefabAsset(map.Root.gameObject,dir+"/Ground_"+info.Finish+".prefab");Object.DestroyImmediate(map.Root.gameObject);
  }
  AssetDatabase.SaveAssets();var output=Path.GetFullPath(Path.Combine(Application.dataPath,"../../QA/GroundMaterials_1.17.unitypackage"));
  AssetDatabase.ExportPackage(new[]{dir,"Assets/Scripts/Runtime/GroundSurfaceArt.cs","Assets/Scripts/Runtime/GroundSurfaceLibrary.cs","Assets/Scripts/Runtime/GroundMaterialCatalog.cs","Assets/Scripts/Runtime/GroundSurfaceRegions.cs"},output,ExportPackageOptions.Recurse|ExportPackageOptions.IncludeDependencies);
  Debug.Log("GROUND MATERIAL LIBRARY EXPORTED: "+output);
 }
}
