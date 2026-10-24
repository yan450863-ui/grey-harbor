using UnityEngine;
using UnityEditor;
using GreyHarbor;
using System.IO;
public static class YardLivingExport136 {
 public static void Build(){
  const string dir="Assets/Environment/YardLivingKit136";Directory.CreateDirectory(dir+"/Materials");Directory.CreateDirectory(dir+"/Meshes");AssetDatabase.Refresh();
  var map=new RaidMap{Root=new GameObject("YardLivingLibrary").transform};var k=new YardLivingKit(map);
  k.Bicycle(Vector3.zero);k.Bicycle(Vector3.zero,0,true);k.GardenBed(Vector3.zero);k.GardenBed(Vector3.zero,true);k.WashStation(Vector3.zero);k.Clothesline(Vector3.zero);k.PicnicSet(Vector3.zero);k.WaterTank(Vector3.zero);k.Compost(Vector3.zero);k.LeanTo(Vector3.zero);k.Grass(Vector3.zero,4,4,false,136);k.Grass(Vector3.zero,4,4,true,136);k.Tree(Vector3.zero,0);k.Tree(Vector3.zero,1);
  foreach(Transform child in map.Root){
   int index=0;foreach(var f in child.GetComponentsInChildren<MeshFilter>())if(!AssetDatabase.Contains(f.sharedMesh)){string path=dir+"/Meshes/"+child.name+"_"+(index++)+".asset";var mesh=AssetDatabase.LoadAssetAtPath<Mesh>(path);if(mesh==null){mesh=Object.Instantiate(f.sharedMesh);AssetDatabase.CreateAsset(mesh,path);}else EditorUtility.CopySerialized(f.sharedMesh,mesh);f.sharedMesh=mesh;}
   foreach(var r in child.GetComponentsInChildren<Renderer>()){var m=r.sharedMaterial;if(AssetDatabase.Contains(m))continue;string path=dir+"/Materials/"+ColorUtility.ToHtmlStringRGBA(m.color)+"_"+m.GetFloat("_SolidFog")+".mat";var saved=AssetDatabase.LoadAssetAtPath<Material>(path);if(saved==null){saved=new Material(m);AssetDatabase.CreateAsset(saved,path);}r.sharedMaterial=saved;}
   foreach(var owned in child.GetComponentsInChildren<WorkshopArtResources>()){owned.Meshes.Clear();owned.Materials.Clear();Object.DestroyImmediate(owned);}
   var prefab=PrefabUtility.SaveAsPrefabAsset(child.gameObject,dir+"/"+child.name+".prefab");if(!prefab.GetComponent<YardAssetInfo>())throw new System.InvalidOperationException("Missing yard asset identity "+child.name);
  }
  Object.DestroyImmediate(map.Root.gameObject);AssetDatabase.SaveAssets();
  string output=Path.GetFullPath(Path.Combine(Application.dataPath,"../../QA/YardLivingKit_1.36.unitypackage"));Directory.CreateDirectory(Path.GetDirectoryName(output));AssetDatabase.ExportPackage(dir,output,ExportPackageOptions.Recurse|ExportPackageOptions.IncludeDependencies);
 }
}
