using System;
using System.IO;
using UnityEditor;
using UnityEditor.Build.Reporting;
using UnityEditor.SceneManagement;
using UnityEngine;

public static class BuildDemo
{
    public static void Build347()=>BuildVersion(false,false);
    public static void Build345()=>BuildVersion(false,false);
    public static void Build344()=>BuildVersion(false,false);
    public static void Build343()=>BuildVersion(false,false);
    public static void Build342()=>BuildVersion(false,false);
    public static void Build341()=>BuildVersion(false,false);
    public static void Build340()=>BuildVersion(false,false);
    public static void Build339()=>BuildVersion(false,false);
    public static void Build338()=>BuildVersion(false,false);
    public static void Build337()=>BuildVersion(false,false);
 public static void Build336()=>BuildVersion(false,false);
    public static void Build335()=>BuildVersion(false,false);
    public static void Build()=>BuildVersion(true);
    public static void BuildRepair210()=>BuildVersion(false);
    public static void BuildRepair220()=>BuildVersion(false);
    public static void BuildOffice2726()=>BuildVersion(false,false);
    public static void BuildGroundRoads2750(){GreyHarbor.GroundSamples2750.Export();RoadDetailExport2750.Build();BuildVersion(false,true);}
    static void BuildVersion(bool exportExistingAssets,bool rebuildGround=true)
    {
        if(exportExistingAssets){RepairInteriorKitExport200.Export();RepairKitExport149.Build();RebelArtExport145.Build();FerryLivingExport144.Build();DockPassengerExport143.Build();DockLoadingExport142.Build();DockKitExport141.Build();DockContainerExport141.Build();OutdoorContainerExport140.Build();DomesticContainerExport139.Build();YardDomesticExport138.Build();YardBoundaryExport137.Build();YardLivingExport136.Build();GateParkingExport.Build();MonsterArtImport.Build();PortKitExport.Build();ContainerExport.Build();GroundMaterialExport.Build();}
        PlayerSettings.companyName="GreyHarborStudio";PlayerSettings.productName="灰港行动 Prototype";PlayerSettings.bundleVersion="3.0.47";
        PlayerSettings.defaultScreenWidth=1600;PlayerSettings.defaultScreenHeight=900;PlayerSettings.fullScreenMode=FullScreenMode.FullScreenWindow;PlayerSettings.resizableWindow=true;PlayerSettings.runInBackground=true;
        PlayerSettings.colorSpace=ColorSpace.Gamma;PlayerSettings.SetScriptingBackend(UnityEditor.Build.NamedBuildTarget.Standalone,ScriptingImplementation.Mono2x);
        var settings=new SerializedObject(AssetDatabase.LoadAllAssetsAtPath("ProjectSettings/ProjectSettings.asset")[0]);var input=settings.FindProperty("activeInputHandler");if(input!=null){input.intValue=0;settings.ApplyModifiedPropertiesWithoutUndo();}
        QualitySettings.antiAliasing=0;QualitySettings.shadowResolution=ShadowResolution.Medium;QualitySettings.shadows=ShadowQuality.All;
        // Keep runtime-created material shaders available in player builds.
        var graphics=new SerializedObject(AssetDatabase.LoadAllAssetsAtPath("ProjectSettings/GraphicsSettings.asset")[0]);var shaders=graphics.FindProperty("m_AlwaysIncludedShaders");
        for(int i=shaders.arraySize-1;i>=0;i--)if(shaders.GetArrayElementAtIndex(i).objectReferenceValue is Shader old && old.name=="GUI/Text Shader") {shaders.GetArrayElementAtIndex(i).objectReferenceValue=null;shaders.DeleteArrayElementAtIndex(i);}
        foreach(string name in new[]{"Standard","Sprites/Default"})
        {var shader=Shader.Find(name);if(shader==null)throw new Exception("Shader missing: "+name);bool has=false;for(int i=0;i<shaders.arraySize;i++)if(shaders.GetArrayElementAtIndex(i).objectReferenceValue==shader)has=true;if(!has){int n=shaders.arraySize;shaders.InsertArrayElementAtIndex(n);shaders.GetArrayElementAtIndex(n).objectReferenceValue=shader;}}
        graphics.ApplyModifiedPropertiesWithoutUndo();
        Directory.CreateDirectory("Assets/Scenes");var scene=EditorSceneManager.NewScene(NewSceneSetup.EmptyScene,NewSceneMode.Single);EditorSceneManager.SaveScene(scene,"Assets/Scenes/GreyHarbor.unity");
        if(rebuildGround)HarborGroundBake2721.Export();
        EditorBuildSettings.scenes=new[]{new EditorBuildSettingsScene("Assets/Scenes/GreyHarbor.unity",true)};AssetDatabase.SaveAssets();
        string output=Path.GetFullPath(Path.Combine(Application.dataPath,"../../Build/GreyHarbor.exe"));
        var report=BuildPipeline.BuildPlayer(new BuildPlayerOptions {scenes=new[]{"Assets/Scenes/GreyHarbor.unity"},locationPathName=output,target=BuildTarget.StandaloneWindows64,options=BuildOptions.None});
        Debug.Log("GREY HARBOR BUILD: "+report.summary.result+" / "+report.summary.totalSize);
        if(report.summary.result!=BuildResult.Succeeded)throw new Exception("Build failed");
    }
}








