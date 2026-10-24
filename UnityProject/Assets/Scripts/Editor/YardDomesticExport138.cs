using UnityEngine;
using UnityEditor;
using GreyHarbor;
using System.IO;
public static class YardDomesticExport138 {
 public static void Build(){
  const string dir="Assets/Environment/YardDomesticKit138";Directory.CreateDirectory(dir+"/Materials");Directory.CreateDirectory(dir+"/Meshes");AssetDatabase.Refresh();
  var map=new RaidMap{Root=new GameObject("YardDomesticLibrary138").transform};var kit=new YardLivingKit(map);
  kit.PottingBench(Vector3.zero);kit.RainBarrels(Vector3.zero);kit.GardenCart(Vector3.zero);kit.FirewoodStack(Vector3.zero);kit.Bed(Vector3.zero);kit.Stove(Vector3.zero);kit.Washer(Vector3.zero);
  foreach(Transform child in map.Root){
   int index=0;foreach(var f in child.GetComponentsInChildren<MeshFilter>())if(!AssetDatabase.Contains(f.sharedMesh)){
    string path=dir+"/Meshes/"+child.name+"_"+(index++)+".asset";var mesh=AssetDatabase.LoadAssetAtPath<Mesh>(path);
    if(mesh==null){mesh=Object.Instantiate(f.sharedMesh);AssetDatabase.CreateAsset(mesh,path);}else EditorUtility.CopySerialized(f.sharedMesh,mesh);f.sharedMesh=mesh;
   }
   foreach(var r in child.GetComponentsInChildren<Renderer>()){
    var m=r.sharedMaterial;if(AssetDatabase.Contains(m))continue;string path=dir+"/Materials/"+ColorUtility.ToHtmlStringRGBA(m.color)+".mat";
    var saved=AssetDatabase.LoadAssetAtPath<Material>(path);if(saved==null){saved=new Material(m);AssetDatabase.CreateAsset(saved,path);}r.sharedMaterial=saved;
   }
   foreach(var owned in child.GetComponentsInChildren<WorkshopArtResources>()){owned.Meshes.Clear();owned.Materials.Clear();Object.DestroyImmediate(owned);}
   var prefab=PrefabUtility.SaveAsPrefabAsset(child.gameObject,dir+"/"+child.name+".prefab");if(!prefab.GetComponent<YardAssetInfo>())throw new System.InvalidOperationException("Domestic identity missing");
  }
  Object.DestroyImmediate(map.Root.gameObject);AssetDatabase.SaveAssets();
  string output=Path.GetFullPath(Path.Combine(Application.dataPath,"../../QA/YardDomesticKit_1.38.unitypackage"));Directory.CreateDirectory(Path.GetDirectoryName(output));
  AssetDatabase.ExportPackage(dir,output,ExportPackageOptions.Recurse|ExportPackageOptions.IncludeDependencies);Debug.Log("YARD DOMESTIC EXPORT COMPLETE");
 }
}
