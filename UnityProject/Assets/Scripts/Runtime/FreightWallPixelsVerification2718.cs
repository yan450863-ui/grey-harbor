using UnityEngine;
namespace GreyHarbor {
 public sealed partial class RaidVerification {
  // Call once immediately after entering (opened=false), then after the real
  // door/search walkthrough (opened=true). Uses real production wall meshes,
  // real visibility textures, shader, ray depth and lights. Fog stays enabled.
  void FreightWallPixels2718(bool opened){
   var map=G.Map;bool duty=map.InteriorId=="freightduty";
   Check(map.FreightInterior2716&&Shader.GetGlobalFloat("_HarborFogEnabled")>.5f,"2718 GPU wall probe uses actual freight interior with fog enabled");
   if(duty){
    FreightWallProbe2718("duty-corridor-north-"+opened,new Vector3(0,0,8.32f),Vector3.back,true);
    FreightWallProbe2718("duty-office-west-"+opened,new Vector3(-8.32f,0,-4.4f),Vector3.right,opened);
   }else{
    FreightWallProbe2718("warehouse-hall-west-"+opened,new Vector3(-8.82f,0,-4.4f),Vector3.right,true);
    FreightWallProbe2718("warehouse-receiving-west-"+opened,new Vector3(-8.82f,0,5.45f),Vector3.right,opened);
   }
  }
  void FreightWallProbe2718(string label,Vector3 face,Vector3 inward,bool expectedLit){
   Vector3 sample=face+inward*.30f;
   Check(G.Vision.RoomAllowed(sample)==expectedLit,"2718 GPU probe samples expected actual room reveal "+label);
   MeshRenderer wall=null,paint=null;
   foreach(var renderer in G.Map.Root.GetComponentsInChildren<MeshRenderer>()){
    if(renderer.name=="FreightWhiteboxWall2716"){
     var b=renderer.bounds;b.Expand(.06f);if(b.Contains(face+Vector3.up*1.65f))wall=renderer;
    }
    if(renderer.name=="WallPaint"&&renderer.sharedMaterial&&renderer.sharedMaterial.shader.name=="GreyHarbor/FreightWall2718")paint=renderer;
   }
   Check(wall&&paint,"2718 GPU probe finds production solid wall and painted lower face "+label);
   if(!wall||!paint)return;
   FreightWallRendererProbe2718(wall,label+"-upper",face+Vector3.up*1.65f,inward,expectedLit);
   FreightWallRendererProbe2718(paint,label+"-paint",face+inward*.012f+Vector3.up*.69f,inward,expectedLit);
  }
  void FreightWallRendererProbe2718(MeshRenderer renderer,string label,Vector3 point,Vector3 inward,bool expectedLit){
   int originalLayer=renderer.gameObject.layer;float fog=Shader.GetGlobalFloat("_HarborFogEnabled");
   var cameraObject=new GameObject("QA2718_actualWallPixelProbe");var camera=cameraObject.AddComponent<Camera>();camera.enabled=false;
   camera.orthographic=true;camera.orthographicSize=.13f;camera.nearClipPlane=.02f;camera.farClipPlane=3;
   camera.clearFlags=CameraClearFlags.SolidColor;camera.backgroundColor=Color.black;camera.cullingMask=1<<31;
   camera.transform.position=point+inward*1.7f;camera.transform.LookAt(point,Vector3.up);
   Texture2D frame=null;
   try{
    // Isolation prevents an unrelated foreground table/partition from making a
    // hidden-room sample black by accident. The actual wall keeps its position
    // and production material; only its temporary camera layer changes.
    renderer.gameObject.layer=31;
    frame=HvRender277(camera,"2718-wall-pixel-"+label+".png",64,64);
    double sum=0;float peak=0;int count=0;
    for(int y=16;y<48;y++)for(int x=16;x<48;x++){Color pixel=frame.GetPixel(x,y);float value=pixel.grayscale;sum+=value;peak=Mathf.Max(peak,value);count++;}
    float average=(float)(sum/count);
    Check(expectedLit?average>.02f:peak<.012f,"2718 actual GPU wall "+(expectedLit?"remains readable":"stays black before opening")+" "+label+" mean="+average+" peak="+peak);
    FreightRoomRow2717("actual GPU wall pixel",label,point,"fog="+fog+" mean="+average+" peak="+peak+" known="+expectedLit+" shader="+renderer.sharedMaterial.shader.name);
   }finally{
    renderer.gameObject.layer=originalLayer;if(frame)Destroy(frame);Destroy(cameraObject);
   }
   Check(renderer.gameObject.layer==originalLayer&&Shader.GetGlobalFloat("_HarborFogEnabled")==fog,"2718 GPU probe restores renderer layer and preserves fog "+label);
  }
 }
}
