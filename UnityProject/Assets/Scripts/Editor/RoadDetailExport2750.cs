using System;
using System.IO;
using UnityEditor;
using UnityEngine;
using GreyHarbor;
public static class RoadDetailExport2750 {
 public static void Build(){
  string folder="Assets/Environment/RoadDetailKit2750";Directory.CreateDirectory(folder);
  foreach(string name in RoadMarkKit2750.Names){
   Color baseColor=PixelArt.Hex(RoadMarkKit2750.Pigment(name));var texture=new Texture2D(256,256,TextureFormat.RGBA32,true);texture.filterMode=FilterMode.Point;
   var pixels=new Color32[256*256];for(int y=0;y<256;y++)for(int x=0;x<256;x++){
    float xx=(x+ .5f)/256*2.6f-1.3f,yy=(y+.5f)/256*2.6f-1.3f;bool inside;
    switch(name){case "DirectionArrow":inside=(Mathf.Abs(xx)<.12f&&yy> -1.15f&&yy<.36f)||(yy>=.36f&&yy<1.15f&&Mathf.Abs(xx)<.52f*(1.15f-yy)/.79f);break;
     case "CenterDash":inside=Mathf.Abs(xx)<.07f&&Mathf.Abs(yy)<1.05f;break;
     case "EdgeLine":inside=Mathf.Abs(xx)<.055f&&Mathf.Abs(yy)<1.25f;break;
     case "StopLine":inside=Mathf.Abs(yy)<.105f&&Mathf.Abs(xx)<1.25f;break;
     case "RoadShoulder":inside=Mathf.Abs(xx)<.17f&&Mathf.Abs(yy)<1.25f;break;
     case "DrainGrate":inside=Mathf.Abs(xx)<.17f&&Mathf.Abs(yy)<.725f;break;
     default:inside=Mathf.Abs(xx)<.55f&&Mathf.Abs(yy)<.65f;break;
    }
    if(!inside)continue;Color c=baseColor;float noise=Mathf.PerlinNoise(x*.24f+71,y*.24f+33)-.5f;
    c.r+=noise*.06f;c.g+=noise*.06f;c.b+=noise*.05f;
    if(name=="DrainGrate"&&(y%17)<4)c=PixelArt.Hex("8C9489");
    if((name=="CenterDash"||name=="EdgeLine"||name=="StopLine"||name=="DirectionArrow")&&((x*37+y*19)%61)<5)continue;
    pixels[y*256+x]=c;
   }
   texture.SetPixels32(pixels);texture.Apply();string imagePath=folder+"/"+name+".png";File.WriteAllBytes(imagePath,texture.EncodeToPNG());UnityEngine.Object.DestroyImmediate(texture);AssetDatabase.ImportAsset(imagePath,ImportAssetOptions.ForceUpdate);
   var importer=AssetImporter.GetAtPath(imagePath) as TextureImporter;importer.isReadable=true;importer.alphaIsTransparency=true;importer.mipmapEnabled=true;importer.textureCompression=TextureImporterCompression.Uncompressed;importer.filterMode=FilterMode.Trilinear;importer.SaveAndReimport();
   string materialPath=folder+"/"+name+".mat";var material=AssetDatabase.LoadAssetAtPath<Material>(materialPath);if(!material){material=new Material(GreyHarbor.FlashlightShaders2763.Load("HarborWorld"));AssetDatabase.CreateAsset(material,materialPath);}material.color=baseColor;material.SetFloat("_SolidFog",0);material.SetFloat("_Detail",.01f);
   var mesh=new Mesh{name=name+"ReusableStencil2750"};
   if(name=="DirectionArrow"){var outline=RoadMarkKit2750.Arrow();var vertices=new Vector3[outline.Length];for(int i=0;i<outline.Length;i++)vertices[i]=new Vector3(outline[i].x,0,outline[i].y);mesh.vertices=vertices;mesh.triangles=new[]{0,2,1,0,6,2,3,5,4};}
   else{float width=name=="CenterDash"?.14f:name=="EdgeLine"?.11f:name=="RoadShoulder"||name=="DrainGrate"?.34f:1.1f;float length=name=="DrainGrate"?1.45f:name=="AsphaltPatch"?1.3f:2.1f;if(name=="StopLine"){width=3.4f;length=.21f;}mesh.vertices=new[]{new Vector3(-width/2,0,-length/2),new Vector3(-width/2,0,length/2),new Vector3(width/2,0,length/2),new Vector3(width/2,0,-length/2)};mesh.triangles=new[]{0,1,2,0,2,3};}
   mesh.RecalculateNormals();mesh.RecalculateBounds();string meshPath=folder+"/"+name+".asset";var prior=AssetDatabase.LoadAssetAtPath<Mesh>(meshPath);if(prior){EditorUtility.CopySerialized(mesh,prior);UnityEngine.Object.DestroyImmediate(mesh);mesh=prior;}else AssetDatabase.CreateAsset(mesh,meshPath);
   var model=new GameObject(name+"_Stencil2750");model.AddComponent<MeshFilter>().sharedMesh=mesh;model.AddComponent<MeshRenderer>().sharedMaterial=material;PrefabUtility.SaveAsPrefabAsset(model,folder+"/"+name+".prefab");UnityEngine.Object.DestroyImmediate(model);
  }
  File.WriteAllText(folder+"/使用说明.md","# 道路细节素材 2.7.50\n\n7款道路标线、修补、路肩和排水格栅的可复用Mesh、Prefab、材质与彩色PNG样板。实际港口由HarborRoadDetails2750程序源注册这些形状进统一地面，不额外创建Renderer或碰撞。标线使用磨损片段；箭头和边线按道路宽度选择，中央迷宫不画公路。样板Prefab用于素材参考，生产使用RegisterGround137优先级0.42～0.51烘焙，避免多层地面闪烁。\n");
  AssetDatabase.SaveAssets();AssetDatabase.Refresh();
 }
}
