using UnityEngine;
using UnityEditor;
using GreyHarbor;
using System.IO;
public static class RepairKitExport149 {
 public static void Build(){
  const string dir="Assets/Environment/RepairExteriorKit149";Directory.CreateDirectory(dir+"/Materials");Directory.CreateDirectory(dir+"/Meshes");AssetDatabase.Refresh();
  var map=new RaidMap{Root=new GameObject("RepairLibrary149").transform};var kit=new RepairExteriorKit149(map);
  kit.ServiceCanopy(Vector3.zero);kit.VehicleLift(Vector3.zero);kit.EngineHoist(Vector3.zero);kit.Compressor(Vector3.zero);kit.TyreRack(Vector3.zero);kit.WeldingCart(Vector3.zero);kit.AxleStand(Vector3.zero);
  foreach(Transform child in map.Root){
   int index=0;foreach(var f in child.GetComponentsInChildren<MeshFilter>())if(!AssetDatabase.Contains(f.sharedMesh)){string path=dir+"/Meshes/"+child.name+"_"+(index++)+".asset";var saved=AssetDatabase.LoadAssetAtPath<Mesh>(path);if(saved==null){saved=Object.Instantiate(f.sharedMesh);AssetDatabase.CreateAsset(saved,path);}else EditorUtility.CopySerialized(f.sharedMesh,saved);f.sharedMesh=saved;}
   foreach(var r in child.GetComponentsInChildren<Renderer>()){var material=r.sharedMaterial;if(AssetDatabase.Contains(material))continue;string path=dir+"/Materials/"+ColorUtility.ToHtmlStringRGBA(material.color)+"_"+material.GetFloat("_SolidFog")+".mat";var saved=AssetDatabase.LoadAssetAtPath<Material>(path);if(saved==null){saved=new Material(material);AssetDatabase.CreateAsset(saved,path);}r.sharedMaterial=saved;}
   foreach(var owned in child.GetComponentsInChildren<WorkshopArtResources>()){owned.Meshes.Clear();owned.Materials.Clear();Object.DestroyImmediate(owned);}
   PrefabUtility.SaveAsPrefabAsset(child.gameObject,dir+"/"+child.name+".prefab");
  }
  Object.DestroyImmediate(map.Root.gameObject);AssetDatabase.SaveAssets();
  string output=Path.GetFullPath(Path.Combine(Application.dataPath,"../../QA/RepairExteriorKit_1.49.unitypackage"));Directory.CreateDirectory(Path.GetDirectoryName(output));AssetDatabase.ExportPackage(dir,output,ExportPackageOptions.Recurse|ExportPackageOptions.IncludeDependencies);Debug.Log("REPAIR EXTERIOR EXPORT COMPLETE");
 }
}
