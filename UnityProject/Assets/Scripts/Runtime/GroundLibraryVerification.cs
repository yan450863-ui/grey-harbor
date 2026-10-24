using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
namespace GreyHarbor {
 public sealed partial class RaidVerification {
  void VerifyGroundPlacement117(RaidMap map){
   var surfaces=map.Root.GetComponentsInChildren<GroundSurfaceRegions>();var types=new HashSet<GroundFinish>();bool cosmetic=true;
   foreach(var s in surfaces){foreach(var r in s.Regions)types.Add(r.Finish);cosmetic&=s.GetComponentsInChildren<Collider>().Length==0;}
   Check(cosmetic,"Applied material surfaces add no blockers: "+map.InteriorId);
   if(map.IsInterior){Check(types.Contains(GroundFinish.Terrazzo)&&types.Contains(GroundFinish.Epoxy)&&types.Contains(GroundFinish.BrokenTile),"Main-house corridor terrazzo and clinic epoxy coexist with existing floors");}
   else Check(types.Contains(GroundFinish.Asphalt)&&types.Contains(GroundFinish.Gravel)&&types.Contains(GroundFinish.Soil)&&types.Contains(GroundFinish.Steel),"Yard applies asphalt gravel soil and loading steel in suitable locations");
  }
  IEnumerator VerifyGroundLibrary117(){
   var unique=new HashSet<GroundFinish>();foreach(var info in GroundMaterialCatalog.All)unique.Add(info.Finish);
   Check(unique.Count==9&&GroundMaterialCatalog.All.Length==9,"Ground catalog has three existing finishes and six new stable entries");
   float fog=Shader.GetGlobalFloat("_HarborFogEnabled");Shader.SetGlobalFloat("_HarborFogEnabled",0);
   Color ambient=RenderSettings.ambientLight;RenderSettings.ambientLight=new Color(.32f,.32f,.32f);var lights=Object.FindObjectsByType<Light>(FindObjectsSortMode.None);var powers=new float[lights.Length];var colors=new Color[lights.Length];for(int i=0;i<lights.Length;i++){powers[i]=lights[i].intensity;colors[i]=lights[i].color;lights[i].intensity=.55f;lights[i].color=Color.white;}
   var cameraRoot=new GameObject("QA_GroundLibraryCamera");var camera=cameraRoot.AddComponent<Camera>();camera.enabled=false;camera.orthographic=true;camera.orthographicSize=3.22f;camera.nearClipPlane=.1f;camera.farClipPlane=40;camera.backgroundColor=PixelArt.Hex("263630");camera.clearFlags=CameraClearFlags.SolidColor;camera.transform.SetPositionAndRotation(new Vector3(1000,14,1000),Quaternion.Euler(90,0,0));
   foreach(var info in GroundMaterialCatalog.All){
    var map=new RaidMap();map.Root=new GameObject("QA_Ground_"+info.Finish).transform;
    var art=new GroundSurfaceArt(map,117000+(int)info.Finish);art.Region(new Rect(-3,-3,6,6),info.Finish);art.Bake();
    bool geometry=true,fogged=true;int vertices=0;var surfaces=map.Root.GetComponentsInChildren<MeshFilter>();
    foreach(var f in surfaces){foreach(var v in f.sharedMesh.vertices)geometry&=!float.IsNaN(v.x)&&!float.IsNaN(v.z)&&v.x>=-3.001f&&v.x<=3.001f&&v.z>=-3.001f&&v.z<=3.001f&&v.y>=.014f&&v.y<=.026f;foreach(var n in f.sharedMesh.normals)geometry&=n.y>.98f||n.sqrMagnitude<.01f;vertices+=f.sharedMesh.vertexCount;var m=f.GetComponent<Renderer>().sharedMaterial;fogged&=m.shader.name=="GreyHarbor/World"&&m.GetFloat("_SolidFog")==0;}
    Check(geometry&&fogged&&map.Root.GetComponentsInChildren<Collider>().Length==0,"Sample geometry stays bounded flat upward and fog-aware: "+info.Finish);
    Check(vertices<50000&&surfaces.Length<40&&!string.IsNullOrEmpty(info.Category)&&!string.IsNullOrEmpty(info.Scenes),"Sample has scene classification and bounded rendering cost: "+info.Finish);
    map.Root.position=new Vector3(1000,0,1000);yield return null;
    var target=new RenderTexture(768,768,24,RenderTextureFormat.ARGB32);var previous=RenderTexture.active;camera.targetTexture=target;camera.Render();RenderTexture.active=target;
    var frame=new Texture2D(768,768,TextureFormat.RGB24,false);frame.ReadPixels(new Rect(0,0,768,768),0,0);frame.Apply();File.WriteAllBytes(Path.Combine(folder,"170-ground-"+info.Finish+".png"),frame.EncodeToPNG());
    camera.targetTexture=null;RenderTexture.active=previous;target.Release();Destroy(target);Destroy(frame);Destroy(map.Root.gameObject);yield return null;
   }
   Destroy(cameraRoot);RenderSettings.ambientLight=ambient;for(int i=0;i<lights.Length;i++)if(lights[i]){lights[i].intensity=powers[i];lights[i].color=colors[i];}Shader.SetGlobalFloat("_HarborFogEnabled",fog);G.Vision.Bind();yield return null;
  }
 }
}
