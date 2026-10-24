using System;using System.IO;using System.Collections.Generic;using UnityEngine;
namespace GreyHarbor {
 public static class ScopeLightShaders335 {
  static AssetBundle bundle;static readonly Dictionary<string,Shader> cache=new Dictionary<string,Shader>();
  public static Shader Load(string name){if(cache.TryGetValue(name,out var shader))return shader;shader=Resources.Load<Shader>(name);if(shader&&shader.FindPropertyIndex("_ScopeLight335")>=0)return cache[name]=shader;if(!bundle)bundle=AssetBundle.LoadFromFile(Path.Combine(Application.streamingAssetsPath,"scopelight335.bundle"));shader=bundle?bundle.LoadAsset<Shader>("assets/resources/"+name.ToLowerInvariant()+".shader"):null;if(!shader||!shader.isSupported)throw new InvalidOperationException("Circular flashlight shader missing: "+name);return cache[name]=shader;}
 }
 public sealed partial class RaidGame {
  internal void SyncScopeLight335(){
   bool active=ScopeShadeVisible309;float opacity=active?ScopeOpacity309:0;Vector2 center=Player&&View?ScopeCenter336:Vector2.zero;float radius=Player&&View?ScopeFrame336.width*.5f:0;
   Shader.SetGlobalFloat("_HarborScopeBlend335",opacity);Shader.SetGlobalVector("_HarborScopeCircle335",new Vector4(center.x,center.y,radius,0));Shader.SetGlobalVector("_HarborScopeViewport335",new Vector4(Screen.width,Screen.height,0,0));Vision?.SetScope335(active,View,center,radius);
  }
 }
}
