using System;using System.IO;using System.Collections;using UnityEngine;
namespace GreyHarbor {public sealed partial class RaidVerification {
 IEnumerator SurfaceHatchPhoto2756(Vector3 p,int i){
  var go=new GameObject("SurfaceHatchCamera2756");var c=go.AddComponent<Camera>();c.CopyFrom(G.View);c.enabled=false;c.allowHDR=false;c.orthographic=true;c.orthographicSize=2.7f;c.nearClipPlane=.1f;c.farClipPlane=150;c.transform.rotation=Quaternion.Euler(58,35,0);c.transform.position=p-c.transform.forward*80;
  float fog=Shader.GetGlobalFloat("_HarborFogEnabled");var renderers=G.Player.GetComponentsInChildren<Renderer>();var enabled=Array.ConvertAll(renderers,r=>r.enabled);
  try{foreach(var r in renderers)r.enabled=false;yield return null;Shader.SetGlobalFloat("_HarborFogEnabled",0);var image=HvRender277(c,"2756-surface-open-hatch-"+i+".png",1500,1000);
   try{int dark=0,metal=0;for(int y=300;y<700;y++)for(int x=450;x<1050;x++){var px=image.GetPixel(x,y);float lum=(px.r+px.g+px.b)/3;if(lum<.25f)dark++;if(lum>.25f&&lum<.8f&&Mathf.Abs(px.g-px.b)>.02f)metal++;}Check(dark>100&&metal>100,"2756 actual opening photograph shows dark cavity and weathered metal "+i);}finally{Destroy(image);}
  }finally{for(int j=0;j<renderers.Length;j++)if(renderers[j])renderers[j].enabled=enabled[j];Shader.SetGlobalFloat("_HarborFogEnabled",fog);Destroy(go);}
 }
 IEnumerator RunSurfaceHatches2756(){
  string dir=Path.GetDirectoryName(G.SavePath);var names=new[]{"stash-v40.json","stash-v41.json","stash-v42.json"};var saves=Array.ConvertAll(names,n=>File.Exists(Path.Combine(dir,n))?File.ReadAllBytes(Path.Combine(dir,n)):null);
  var controls=G.Controls;bool enabled=G.enabled;
  try{
   Check(Application.version=="2.7.56"&&G.SavePath.EndsWith("verification-stash-2.7.56.json"),"2756 isolated hatch verification save");FreshCamp();G.Controls=new TouchInput();G.HarborWalkthrough=true;yield return G.BeginTrainingLoad2721();G.Paused=true;G.enabled=false;var outdoor=G.Map;var cache=outdoor.StableGround137;var fingerprint=outdoor.GroundPaintFingerprint2721;
   Check(outdoor.UndergroundHatchViews2756.Count==6&&outdoor.UndergroundSurfaceEntries2752.Count==6,"2756 six open inspection hatches replace old markers");RaidMap underground=null;
   for(int i=0;i<6;i++){
    var portal=outdoor.UndergroundSurfaceEntries2752[i];var view=outdoor.UndergroundHatchViews2756[i];Check(view&&Vector3.Distance(view.transform.position,portal.Position)<.001f&&view.GetComponentsInChildren<Collider>().Length==0,"2756 hatch preserves portal position without raised collisions "+i);
    var renderers=view.GetComponentsInChildren<MeshRenderer>();Check(renderers.Length>=4,"2756 real rim opening lid and ladder geometry "+i);bool dark=false,colored=false;foreach(var renderer in renderers){var c=renderer.sharedMaterial.color;dark|=c.r+c.g+c.b<.4f;colored|=Mathf.Abs(c.g-c.b)>.03f;Check(renderer.sharedMaterial.shader.isSupported,"2756 actual hatch shader supported "+i);}Check(dark&&colored,"2756 dark open cavity and distinct weathered metal colors "+i);
    foreach(var delta in new[]{Vector3.zero,Vector3.right*.65f,Vector3.left*.65f,Vector3.forward*.65f,Vector3.back*.65f})Check(outdoor.OnHarborSurface(portal.Position+delta)&&CargoClear2713(portal.Position+delta),"2756 safe real capsule around open hatch "+i+" "+delta);
    UndergroundPlace2752(portal.Position+Vector3.back*.6f,"2756 physical hatch approach "+i);G.Player.Move(Vector3.forward*.6f);Physics.SyncTransforms();Check(Vector3.Distance(G.Player.transform.position,portal.Position)<.08f,"2756 real Actor.Move reaches hatch without snagging "+i);
    FreightMonsterF2720(portal,true);yield return null;Check(G.Map.UndergroundWhitebox2752&&G.Map.Loot.Count==24&&G.Enemies.Count==0,"2756 actual F enters existing furnished underground "+i);if(underground==null)underground=G.Map;else Check(G.Map==underground,"2756 six hatches share cached underground "+i);
    var id=portal.Destination.Substring("underground-enter:".Length);var exit=G.Map.Doors.Find(d=>d.Destination=="underground-exit:"+id);UndergroundPlace2752(exit.Position,"2756 underground return exit "+i);FreightMonsterF2720(exit,true);yield return null;
    Check(G.Map==outdoor&&Vector3.Distance(G.Player.transform.position,portal.Position)<.03f&&CargoClear2713(G.Player.transform.position)&&outdoor.StableGround137==cache&&outdoor.GroundPaintFingerprint2721==fingerprint,"2756 exit restores same hatch and unchanged outdoor ground cache "+i);
    yield return SurfaceHatchPhoto2756(portal.Position,i);
   }
   for(int i=0;i<names.Length;i++)Check(MainBytes273(saves[i],File.Exists(Path.Combine(dir,names[i]))?File.ReadAllBytes(Path.Combine(dir,names[i])):null),"2756 formal save unchanged "+names[i]);
  }finally{G.Controls=controls;G.Paused=true;G.enabled=enabled;}
 }
}}
