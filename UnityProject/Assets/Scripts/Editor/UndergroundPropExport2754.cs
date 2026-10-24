using System.IO;
using System.Text;
using UnityEngine;
using UnityEditor;
namespace GreyHarbor {
 public static class UndergroundPropExport2754 {
  public const string AssetFolder="Assets/Environment/UndergroundProps2754";
  [MenuItem("Grey Harbor/Export Underground Props 2754")]
  public static void Build(){
   Directory.CreateDirectory(AssetFolder);AssetDatabase.Refresh();
   var map=new RaidMap();map.BuildShowcase2754();
   var manifest=new StringBuilder("# 地下管网装饰物素材 2.7.54\n\n20 款独立可复用 Prefab；每件一个合并网格及调色纹理材质。灰绿、锈棕、铜色与黄色警示色配合既有低多边形画风。未生成 NPC 或容器。\n\n运行接口：UndergroundProps2754.Build(map, kind, position, yaw)。footprint 为预留占地，碰撞只覆盖实际支腿或设备实体。细管、梯子、排水沟等不增加整块碰撞。\n\n| Kind | 占地米 | 实体碰撞数 | 顶点数 | 调色数 |\n|---|---|---:|---:|---:|\n");
   try {
    foreach(var source in map.UndergroundShowcaseProps2754){
     var info=source.GetComponent<UndergroundPropInfo2754>();var filter=source.GetComponent<MeshFilter>();var renderer=source.GetComponent<MeshRenderer>();
     string stem=AssetFolder+"/"+info.Kind;
     var palette=(Texture2D)renderer.sharedMaterial.mainTexture;
     File.WriteAllBytes(stem+"_Palette.png",palette.EncodeToPNG());AssetDatabase.ImportAsset(stem+"_Palette.png",ImportAssetOptions.ForceSynchronousImport);
     var importer=(TextureImporter)AssetImporter.GetAtPath(stem+"_Palette.png");importer.textureType=TextureImporterType.Default;importer.filterMode=FilterMode.Point;importer.wrapMode=TextureWrapMode.Clamp;importer.mipmapEnabled=false;importer.textureCompression=TextureImporterCompression.Uncompressed;importer.isReadable=true;importer.SaveAndReimport();
     var mesh=Object.Instantiate(filter.sharedMesh);mesh.name=info.Kind+"_Mesh";
     var material=Object.Instantiate(renderer.sharedMaterial);material.name=info.Kind+"_Material";material.mainTexture=AssetDatabase.LoadAssetAtPath<Texture2D>(stem+"_Palette.png");
     Replace(mesh,stem+"_Mesh.asset");Replace(material,stem+".mat");
     manifest.AppendFormat(System.Globalization.CultureInfo.InvariantCulture,"| {0} | {1:0.00} × {2:0.00} | {3} | {4} | {5} |\n",info.Kind,info.Footprint.x,info.Footprint.y,source.GetComponentsInChildren<Collider>().Length,mesh.vertexCount,palette.width);
     // Save a separate clone without transient-resource owners. Persistent assets are owned by Unity.
     var prefab=Object.Instantiate(source);prefab.name="UndergroundProp2754_"+info.Kind;prefab.transform.SetParent(null);prefab.transform.SetPositionAndRotation(Vector3.zero,Quaternion.identity);
     var transient=prefab.GetComponent<StableGroundResources137>();if(transient){transient.OwnsTexture=false;Object.DestroyImmediate(transient);}
     var lifetime=prefab.GetComponent<WorkshopArtResources>();if(lifetime){lifetime.Meshes.Clear();lifetime.Materials.Clear();Object.DestroyImmediate(lifetime);}
     prefab.GetComponent<MeshFilter>().sharedMesh=mesh;prefab.GetComponent<MeshRenderer>().sharedMaterial=material;
     PrefabUtility.SaveAsPrefabAsset(prefab,stem+".prefab");Object.DestroyImmediate(prefab);
    }
    manifest.Append("\n素材目录中的每款 _Palette.png、_Mesh.asset、.mat 与 .prefab 均为持久资产。Prefab 可直接拖入 Unity；独立展示地图通过 map.BuildShowcase2754() 创建。墙装管道建议朝向墙体放置，大型设备优先靠扩宽凹位边缘，避开主通道。灯具为装饰，无新增实时灯光。\n");
    File.WriteAllText(AssetFolder+"/素材清单2754.md",manifest.ToString(),new UTF8Encoding(false));AssetDatabase.SaveAssets();AssetDatabase.Refresh();Debug.Log("UNDERGROUND_PROPS_2754_EXPORTED:20");
   } finally {if(map.Root)Object.DestroyImmediate(map.Root.gameObject);}
  }
  static void Replace(Object value,string path){if(AssetDatabase.LoadMainAssetAtPath(path))AssetDatabase.DeleteAsset(path);AssetDatabase.CreateAsset(value,path);}
 }
}
