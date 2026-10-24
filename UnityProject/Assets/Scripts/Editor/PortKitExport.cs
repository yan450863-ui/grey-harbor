using UnityEngine;
using UnityEditor;
using System.IO;
using GreyHarbor;
public static class PortKitExport {
 public static void Build(){
  const string dir="Assets/Environment/PortKit";Directory.CreateDirectory(dir+"/Materials");AssetDatabase.Refresh();
  var map=new RaidMap();map.Root=new GameObject("PortKitLibrary").transform;var k=new PortEnvironmentKit(map);
  k.Building(Vector3.zero,13,7,"939585","687875");k.Shutter(Vector3.zero,0,"A1");k.Container(Vector3.zero,3.2f,5.4f,"527B76");k.Wall(Vector3.zero,6,true);k.Pallets(Vector3.zero,2);k.TimberStack(Vector3.zero);k.Barrel(Vector3.zero,"687754");k.Bench(Vector3.zero);k.Pickup(Vector3.zero);k.Lamp(Vector3.zero);new YardSurfaceArt(map).BuildSample();
  foreach(Transform child in map.Root){foreach(var f in child.GetComponentsInChildren<MeshFilter>()){if(AssetDatabase.Contains(f.sharedMesh))continue;string meshDir=dir+"/Meshes";Directory.CreateDirectory(meshDir);string meshPath=meshDir+"/"+f.sharedMesh.name.Replace("|","_")+".asset";var savedMesh=AssetDatabase.LoadAssetAtPath<Mesh>(meshPath);if(savedMesh==null){savedMesh=Object.Instantiate(f.sharedMesh);AssetDatabase.CreateAsset(savedMesh,meshPath);}else EditorUtility.CopySerialized(f.sharedMesh,savedMesh);f.sharedMesh=savedMesh;}foreach(var r in child.GetComponentsInChildren<Renderer>()){var m=r.sharedMaterial;if(AssetDatabase.Contains(m))continue;string key=ColorUtility.ToHtmlStringRGBA(m.color)+"_"+m.GetFloat("_SolidFog").ToString("0")+"_"+Mathf.RoundToInt(m.GetFloat("_Detail")*100);string path=dir+"/Materials/"+key+".mat";var saved=AssetDatabase.LoadAssetAtPath<Material>(path);if(saved==null){saved=new Material(m);AssetDatabase.CreateAsset(saved,path);}r.sharedMaterial=saved;}foreach(var owned in child.GetComponentsInChildren<WorkshopArtResources>())Object.DestroyImmediate(owned);PrefabUtility.SaveAsPrefabAsset(child.gameObject,dir+"/"+child.name+".prefab");}
  Object.DestroyImmediate(map.Root.gameObject);AssetDatabase.SaveAssets();string output=Path.GetFullPath(Path.Combine(Application.dataPath,"../../QA/PortEnvironmentKit_1.9.2.unitypackage"));Directory.CreateDirectory(Path.GetDirectoryName(output));AssetDatabase.ExportPackage(dir,output,ExportPackageOptions.Recurse|ExportPackageOptions.IncludeDependencies);Debug.Log("PORT KIT EXPORTED: "+output);
 }
}
