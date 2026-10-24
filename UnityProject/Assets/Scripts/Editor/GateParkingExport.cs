using UnityEngine;
using UnityEditor;
using GreyHarbor;
using System.IO;
public static class GateParkingExport {
 public static void Build(){
  const string dir="Assets/Environment/GateParkingKit";Directory.CreateDirectory(dir+"/Materials");Directory.CreateDirectory(dir+"/Meshes");AssetDatabase.Refresh();
  var map=new RaidMap{Root=new GameObject("GateParkingLibrary").transform};var k=new GateParkingKit(map);
  for(int i=0;i<3;i++){k.Tree(Vector3.zero,i);k.Vehicle(Vector3.zero,i);}k.Grass(Vector3.zero,true);k.Grass(Vector3.zero,false);k.WheelStop(Vector3.zero);k.Barrier(Vector3.zero);k.Shelter(Vector3.zero,false);k.Shelter(Vector3.zero,true);k.Planter(Vector3.zero,5);k.LitterBin(Vector3.zero);
  foreach(Transform child in map.Root){int index=0;foreach(var f in child.GetComponentsInChildren<MeshFilter>())if(!AssetDatabase.Contains(f.sharedMesh)){string path=dir+"/Meshes/"+child.name+"_"+(index++)+".asset";var mesh=AssetDatabase.LoadAssetAtPath<Mesh>(path);if(mesh==null){mesh=Object.Instantiate(f.sharedMesh);AssetDatabase.CreateAsset(mesh,path);}else EditorUtility.CopySerialized(f.sharedMesh,mesh);f.sharedMesh=mesh;}
   foreach(var r in child.GetComponentsInChildren<Renderer>()){var m=r.sharedMaterial;if(AssetDatabase.Contains(m))continue;string path=dir+"/Materials/"+ColorUtility.ToHtmlStringRGBA(m.color)+"_"+m.GetFloat("_SolidFog")+".mat";var saved=AssetDatabase.LoadAssetAtPath<Material>(path);if(saved==null){saved=new Material(m);AssetDatabase.CreateAsset(saved,path);}r.sharedMaterial=saved;}
   foreach(var owned in child.GetComponentsInChildren<WorkshopArtResources>()){owned.Meshes.Clear();owned.Materials.Clear();Object.DestroyImmediate(owned);}PrefabUtility.SaveAsPrefabAsset(child.gameObject,dir+"/"+child.name+".prefab");
  }
  Object.DestroyImmediate(map.Root.gameObject);AssetDatabase.SaveAssets();string output=Path.GetFullPath(Path.Combine(Application.dataPath,"../../QA/GateParkingKit_1.30.unitypackage"));Directory.CreateDirectory(Path.GetDirectoryName(output));AssetDatabase.ExportPackage(dir,output,ExportPackageOptions.Recurse|ExportPackageOptions.IncludeDependencies);
 }
}
