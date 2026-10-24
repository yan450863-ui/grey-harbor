using System;using System.Collections.Generic;using System.IO;using UnityEngine;
namespace GreyHarbor {public static class RenderShaders307 {
 static AssetBundle bundle;static readonly Dictionary<string,Shader> cache=new Dictionary<string,Shader>();
 public static Shader Load(string name){if(cache.TryGetValue(name,out var shader))return shader;shader=Resources.Load<Shader>(name);if(!shader){if(!bundle)bundle=AssetBundle.LoadFromFile(Path.Combine(Application.streamingAssetsPath,"renderfix307.bundle"));shader=bundle?bundle.LoadAsset<Shader>("assets/resources/"+name.ToLowerInvariant()+".shader"):null;}if(!shader||!shader.isSupported)throw new InvalidOperationException("缺少或不支持渲染修正着色器："+name);return cache[name]=shader;}
}}
