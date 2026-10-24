using System;
using System.Linq;
using UnityEditor;
using UnityEditor.Build;
// These methods only change the build symbol. Start another Unity build after recompilation.
public static class ConsoleBuildProfile2761 {
 const string Symbol="GREY_HARBOR_TEST_CONSOLE";
 public static void Enable(){Set(true);}
 public static void Disable(){Set(false);}
 static void Set(bool enabled){
  var target=NamedBuildTarget.Standalone;
  var symbols=PlayerSettings.GetScriptingDefineSymbols(target).Split(';').Where(s=>!string.IsNullOrWhiteSpace(s)&&s!=Symbol).ToList();
  if(enabled)symbols.Add(Symbol);
  PlayerSettings.SetScriptingDefineSymbols(target,string.Join(";",symbols));AssetDatabase.SaveAssets();
  UnityEngine.Debug.Log("Console 2.7.61 profile: "+(enabled?"enabled":"disabled"));
 }
}
