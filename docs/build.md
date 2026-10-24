# 打开、构建与专项验证

使用 Unity 6000.6.3f1 和 Windows Build Support（Mono）。通过 Git LFS 获取完整资源后，在 Unity Hub 打开 `UnityProject`。

## Windows x64 构建

PowerShell 示例，在仓库根目录运行；按实际安装位置修改 `$unityEditor`：

```powershell
$unityEditor = 'D:\Unity\Editors\6000.6.3f1\Editor\Unity.exe'
$projectPath = Join-Path $PWD 'UnityProject'
$buildLog = Join-Path $PWD 'unity-build.log'
& $unityEditor -batchmode -quit -force-d3d11 -projectPath $projectPath -executeMethod BuildDemo.Build347 -logFile $buildLog
```

构建输出：`Build/GreyHarbor.exe`。需保留同目录中的 `_Data`、`MonoBleedingEdge`、UnityPlayer.dll 和其他运行依赖；分享时压缩整个 Build 文件夹。

`BuildDemo.Build347` 使用仓库内现有资源，不要求重新生成美术或地表。首次打开会重新导入 Assets，耗时取决于机器。不要将入口改成 `BuildDemo.Build`，该历史入口会重新执行素材导出流程。

## 黑雾专项

在构建完成后运行：

```powershell
& '.\Build\GreyHarbor.exe' --verify --verify-transit-fog-347 -force-d3d11 -screen-fullscreen 0 -screen-width 1600 -screen-height 900 -logFile '.\transit-fog.log'
```

此专项使用 `verification-stash-3.0.47.json` 隔离验证存档。输出位置以控制台/日志为准；代码的相对路径会将 QA 目录写在仓库根目录。专项需要图形环境，不建议加 `-nographics`。

本次在后台隐藏窗口中验证时，默认 DirectX 12 的截图步骤返回空纹理并退出；显式指定 DirectX 11 后，12 项程序检查通过，退出码为 0。隐藏窗口生成的截图为黑屏，不能据此判断画面效果。仓库展示的是原版保留的截图，本次未重新完成可见窗口的视觉验收。发布启动器默认使用 DirectX 11。

正式存档在 `%USERPROFILE%\AppData\LocalLow\GreyHarborStudio\灰港行动 Prototype\stash-v43.json`。发布包不携带个人存档。

历史验证模块保留在源码中；部分会校验特定旧版本号或读取旧版本基线。只有本版适用的专项才能用于判断当前版本，不能将旧版本的通过记录视为本版全游戏验收。
