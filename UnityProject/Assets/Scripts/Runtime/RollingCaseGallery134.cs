using UnityEngine;
using System.Collections;
using System.IO;
namespace GreyHarbor {
 public sealed partial class RaidVerification {
  IEnumerator RollingCaseGallery134(){
   var gallery=new RaidMap();gallery.Root=new GameObject("RollingCaseGallery134").transform;
   var origin=new Vector3(200,0,200);gallery.Box("GalleryFloor",origin+Vector3.down*.14f,new Vector3(8,.25f,5),"667368");
   var art=new ContainerArt(gallery);art.Build(ContainerLook.RollingSuitcase,origin+Vector3.left*2,0,false);
   var opened=art.Build(ContainerLook.RollingSuitcase,origin,0,false);opened.SetSearched();art.Build(ContainerLook.Toolbox,origin+Vector3.right*2);
   var cameraRoot=new GameObject("QA_RollingCaseCamera");var cam=cameraRoot.AddComponent<Camera>();cam.CopyFrom(G.View);cam.enabled=false;cam.orthographicSize=2.5f;cam.farClipPlane=40;
   var rotation=Quaternion.Euler(32,25,0);cam.transform.SetPositionAndRotation(origin+Vector3.up*.75f-rotation*Vector3.forward*12,rotation);
   yield return new WaitForEndOfFrame();float fog=Shader.GetGlobalFloat("_HarborFogEnabled");Shader.SetGlobalFloat("_HarborFogEnabled",0);
   var target=new RenderTexture(1600,900,24);cam.targetTexture=target;cam.Render();var previous=RenderTexture.active;RenderTexture.active=target;
   var frame=new Texture2D(1600,900,TextureFormat.RGB24,false);frame.ReadPixels(new Rect(0,0,1600,900),0,0);frame.Apply();File.WriteAllBytes(Path.Combine(folder,"134-container-closeup.png"),frame.EncodeToPNG());
   RenderTexture.active=previous;cam.targetTexture=null;target.Release();Destroy(target);Destroy(frame);Destroy(cameraRoot);Destroy(gallery.Root.gameObject);Shader.SetGlobalFloat("_HarborFogEnabled",fog);G.Vision.Bind();yield return null;
  }
 }
}
