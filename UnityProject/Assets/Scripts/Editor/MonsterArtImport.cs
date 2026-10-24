using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEditor;
using GreyHarbor;
public static class MonsterArtImport {
 public static void Build(){
  const string source="Assets/Art/Monsters/MonsterAtlas.png",dest="Assets/Resources/Monsters";
  var importer=(TextureImporter)AssetImporter.GetAtPath(source);if(importer==null)throw new Exception("Monster atlas missing");
  importer.textureType=TextureImporterType.Default;importer.isReadable=true;importer.alphaIsTransparency=true;importer.mipmapEnabled=false;
  importer.filterMode=FilterMode.Point;importer.textureCompression=TextureImporterCompression.Uncompressed;importer.maxTextureSize=2048;importer.npotScale=TextureImporterNPOTScale.None;importer.SaveAndReimport();
  var t=AssetDatabase.LoadAssetAtPath<Texture2D>(source);var p=t.GetPixels32();int w=t.width,h=t.height;
  var seen=new bool[p.Length];var parts=new List<RectInt>();var queue=new Queue<int>();
  for(int start=0;start<p.Length;start++){
   if(seen[start]||p[start].a<128)continue;seen[start]=true;queue.Enqueue(start);int count=0,x0=w,y0=h,x1=0,y1=0;
   while(queue.Count>0){int v=queue.Dequeue(),x=v%w,y=v/w;count++;x0=Math.Min(x0,x);x1=Math.Max(x1,x);y0=Math.Min(y0,y);y1=Math.Max(y1,y);
    for(int dy=-1;dy<=1;dy++)for(int dx=-1;dx<=1;dx++){int nx=x+dx,ny=y+dy;if(nx<0||ny<0||nx>=w||ny>=h)continue;int j=ny*w+nx;if(!seen[j]&&p[j].a>=128){seen[j]=true;queue.Enqueue(j);}}
   }if(count>1500)parts.Add(new RectInt(x0,y0,x1-x0+1,y1-y0+1));
  }
  if(parts.Count!=8)throw new Exception("Expected 8 isolated monster frames, found "+parts.Count);
  parts.Sort((a,b)=>b.center.y.CompareTo(a.center.y));var idle=parts.GetRange(0,4);var attack=parts.GetRange(4,4);idle.Sort((a,b)=>a.x.CompareTo(b.x));attack.Sort((a,b)=>a.x.CompareTo(b.x));
  Directory.CreateDirectory(dest);var log=new List<string>();
  for(int k=0;k<4;k++)for(int frame=0;frame<2;frame++){
   var r=(frame==0?idle:attack)[k];string name=((MonsterKind)k)+"_"+(frame==0?"Idle":"Windup");
   var s=Sprite.Create(t,new Rect(r.x,r.y,r.width,r.height),new Vector2(.5f,0),idle[k].height/MonsterTuning.ArtHeight((MonsterKind)k),0,SpriteMeshType.FullRect);s.name=name;
   string path=dest+"/"+name+".asset";if(AssetDatabase.LoadAssetAtPath<Sprite>(path))AssetDatabase.DeleteAsset(path);AssetDatabase.CreateAsset(s,path);log.Add(name+" rect="+r+" physicalHeight="+s.bounds.size.y);
  }
  AssetDatabase.SaveAssets();string qa=Path.GetFullPath(Path.Combine(Application.dataPath,"../../QA"));Directory.CreateDirectory(qa);File.WriteAllLines(Path.Combine(qa,"monster-art-import.txt"),log);
  AssetDatabase.ExportPackage(new[]{dest,source},Path.Combine(qa,"MonsterArt_1.18.unitypackage"),ExportPackageOptions.Recurse|ExportPackageOptions.IncludeDependencies);
 }
}
