using System;
using System.IO;
using System.Text;
using UnityEditor;
using UnityEngine;
namespace GreyHarbor {
 public static class GroundSamples2750 {
  const string Folder="Assets/Environment/GroundStates2750";
  public static void Export(){
   Directory.CreateDirectory(Folder);Directory.CreateDirectory(Folder+"/Legacy9");
   foreach(var item in GroundMaterialCatalog.All)ExportLegacy(item);
   var families=new[]{GroundFinish.Soil,GroundFinish.Gravel,GroundFinish.PatchedConcrete,GroundFinish.Concrete,GroundFinish.Asphalt,GroundFinish.Steel};
   foreach(var family in families)ExportSample(family.ToString(),family,family,0,false);
   ExportSample("Soil_Damp",GroundFinish.Soil,GroundFinish.Soil,1,false);
   ExportSample("Gravel_Repaired",GroundFinish.Gravel,GroundFinish.Gravel,2,false);
   ExportSample("Asphalt_Repair",GroundFinish.Asphalt,GroundFinish.Asphalt,2,false);
   ExportSample("Concrete_OilWear",GroundFinish.Concrete,GroundFinish.Concrete,1,false);
   ExportSample("Soil_to_Gravel",GroundFinish.Soil,GroundFinish.Gravel,0,true);
   ExportSample("Gravel_to_PatchedConcrete",GroundFinish.Gravel,GroundFinish.PatchedConcrete,0,true);
   ExportSample("Concrete_to_Asphalt",GroundFinish.Concrete,GroundFinish.Asphalt,0,true);
   File.WriteAllText(Folder+"/GroundStates2750.md","# Ground states 2.7.50\n\n9 original library prefabs retain their original procedural geometry, palette, surface region metadata and materials. 13 new usable PNG + material + mesh + prefab samples use the exact production GroundMaterialStates2750.Sample/Transition functions. Each sample is 4 x 4 metres, 384 x 384 pixels. Soil/gravel/cement/asphalt/steel are distinct purpose families; wet soil, repair aggregate, asphalt repair, oil wear and 3 natural transition states extend the original library. Outdoor roads/details >= .40 paint priority are kept intact; indoor floors and special ship deck pixels are unchanged.\n",new UTF8Encoding(false));
   AssetDatabase.Refresh();AssetDatabase.SaveAssets();Debug.Log("GROUND SAMPLES 2750 COMPLETE: 9 original usable prefabs + 13 production PNG/material/mesh/prefab states");
  }
  static void Save(UnityEngine.Object asset,string path){AssetDatabase.DeleteAsset(path);AssetDatabase.CreateAsset(asset,path);}
  static void ExportLegacy(GroundMaterialInfo item){
   var map=new RaidMap{Root=new GameObject("OriginalGround2750_"+item.Finish).transform};
   var art=new GroundSurfaceArt(map,275000+(int)item.Finish);art.Region(new Rect(-2,-2,4,4),item.Finish);art.Bake("OriginalLibrary_"+item.Finish);
   int index=0;
   foreach(var filter in map.Root.GetComponentsInChildren<MeshFilter>()){
    string stem=Folder+"/Legacy9/"+item.Finish+"_"+(index++);
    var mesh=UnityEngine.Object.Instantiate(filter.sharedMesh);mesh.name=item.Finish+"_SourceMesh";Save(mesh,stem+".asset");filter.sharedMesh=mesh;
    var renderer=filter.GetComponent<Renderer>();var material=UnityEngine.Object.Instantiate(renderer.sharedMaterial);Save(material,stem+".mat");renderer.sharedMaterial=material;
   }
   PrefabUtility.SaveAsPrefabAsset(map.Root.gameObject,Folder+"/Legacy9/"+item.Finish+".prefab");UnityEngine.Object.DestroyImmediate(map.Root.gameObject);
  }
  static void ExportSample(string name,GroundFinish a,GroundFinish b,int state,bool transition){
   const int size=384;var texture=new Texture2D(size,size,TextureFormat.RGBA32,true){name=name,filterMode=FilterMode.Trilinear,anisoLevel=8,wrapMode=TextureWrapMode.Repeat};var pixels=new Color32[size*size];
   for(int z=0;z<size;z++)for(int x=0;x<size;x++){float px=(x+.5f)*4/size,pz=(z+.5f)*4/size;pixels[z*size+x]=transition?GroundMaterialStates2750.Transition(a,b,px,pz):GroundMaterialStates2750.Sample(a,px,pz,state);}
   texture.SetPixels32(pixels);texture.Apply(true,false);string stem=Folder+"/"+name;File.WriteAllBytes(stem+".png",texture.EncodeToPNG());UnityEngine.Object.DestroyImmediate(texture);
   AssetDatabase.ImportAsset(stem+".png",ImportAssetOptions.ForceSynchronousImport);var importer=(TextureImporter)AssetImporter.GetAtPath(stem+".png");importer.mipmapEnabled=true;importer.filterMode=FilterMode.Trilinear;importer.anisoLevel=8;importer.wrapMode=TextureWrapMode.Repeat;importer.textureCompression=TextureImporterCompression.Uncompressed;importer.SaveAndReimport();
   var material=new Material(GreyHarbor.FlashlightShaders2763.Load("HarborWorld")){name=name,color=Color.white};material.SetTexture("_MainTex",AssetDatabase.LoadAssetAtPath<Texture2D>(stem+".png"));material.SetFloat("_UseStableGround",1);material.SetFloat("_SolidFog",0);Save(material,stem+".mat");
   var mesh=new Mesh{name=name+"_4m"};mesh.vertices=new[]{new Vector3(-2,0,-2),new Vector3(-2,0,2),new Vector3(2,0,2),new Vector3(2,0,-2)};mesh.uv=new[]{Vector2.zero,Vector2.up,Vector2.one,Vector2.right};mesh.triangles=new[]{0,1,2,0,2,3};mesh.RecalculateNormals();mesh.RecalculateBounds();Save(mesh,stem+"_Mesh.asset");
   var view=new GameObject("GroundSample2750_"+name);view.AddComponent<MeshFilter>().sharedMesh=mesh;view.AddComponent<MeshRenderer>().sharedMaterial=material;view.AddComponent<GroundSurfaceRegions>().Regions.Add(new GroundSurfaceRegion(new Rect(-2,-2,4,4),a,0));PrefabUtility.SaveAsPrefabAsset(view,stem+".prefab");UnityEngine.Object.DestroyImmediate(view);
  }
 }
}
