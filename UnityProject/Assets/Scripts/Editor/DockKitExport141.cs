using UnityEngine;
using UnityEditor;
using GreyHarbor;
using System.IO;
public static class DockKitExport141 {
 public static void Build(){
  const string dir="Assets/Environment/DockLivingKit141";Directory.CreateDirectory(dir+"/Materials");Directory.CreateDirectory(dir+"/Meshes");AssetDatabase.Refresh();
  var map=new RaidMap{Root=new GameObject("DockLibrary141").transform};var k=new DockLivingKit141(map);
  k.FishingBoat(Vector3.zero);k.WorkBarge(Vector3.zero);k.Gangway(Vector3.zero);k.OpenShelter(Vector3.zero);k.Workbench(Vector3.zero);k.JibCrane(Vector3.zero);k.Bollard(Vector3.zero);k.CableDrum(Vector3.zero);k.CargoTrolley(Vector3.zero);
  foreach(Transform child in map.Root){
   int index=0;foreach(var f in child.GetComponentsInChildren<MeshFilter>())if(!AssetDatabase.Contains(f.sharedMesh)){string path=dir+"/Meshes/"+child.name+"_"+(index++)+".asset";var mesh=AssetDatabase.LoadAssetAtPath<Mesh>(path);if(mesh==null){mesh=Object.Instantiate(f.sharedMesh);AssetDatabase.CreateAsset(mesh,path);}else EditorUtility.CopySerialized(f.sharedMesh,mesh);f.sharedMesh=mesh;}
   foreach(var r in child.GetComponentsInChildren<Renderer>()){var m=r.sharedMaterial;if(AssetDatabase.Contains(m))continue;string path=dir+"/Materials/"+ColorUtility.ToHtmlStringRGBA(m.color)+"_"+m.GetFloat("_SolidFog")+".mat";var saved=AssetDatabase.LoadAssetAtPath<Material>(path);if(saved==null){saved=new Material(m);AssetDatabase.CreateAsset(saved,path);}r.sharedMaterial=saved;}
   foreach(var owned in child.GetComponentsInChildren<WorkshopArtResources>()){owned.Meshes.Clear();owned.Materials.Clear();Object.DestroyImmediate(owned);}
   var prefab=PrefabUtility.SaveAsPrefabAsset(child.gameObject,dir+"/"+child.name+".prefab");if(!prefab.GetComponent<DockAssetInfo141>())throw new System.InvalidOperationException("Missing yard asset identity "+child.name);
  }
  Object.DestroyImmediate(map.Root.gameObject);AssetDatabase.SaveAssets();
  string output=Path.GetFullPath(Path.Combine(Application.dataPath,"../../QA/DockLivingKit_1.41.unitypackage"));Directory.CreateDirectory(Path.GetDirectoryName(output));AssetDatabase.ExportPackage(dir,output,ExportPackageOptions.Recurse|ExportPackageOptions.IncludeDependencies);
 }
}
