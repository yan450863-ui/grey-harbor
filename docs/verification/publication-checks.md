# 2026-10-24 发布验证

- Unity 6000.6.3f1 从独立发布目录重新导入并构建 Windows x64：成功，进程退出码 0。
- 构建入口：`BuildDemo.Build347`；构建报告总大小：478,918,758 字节。
- 发布工程的 Assets、Packages、ProjectSettings 共 11,086 个文件，与本地 3.0.47 原工程逐文件 SHA-256 比较一致；构建后再次比较，没有缺失或变更。
- 常见 GitHub/OpenAI 令牌、私钥头及硬编码凭据模式扫描：未发现候选；日志、用户设置和个人存档不纳入仓库。
- 新构建通过 `--verify --verify-transit-fog-347 -force-d3d11` 的 12 项检查，进程退出码 0。新结果见 [publication-fog-checks.txt](publication-fog-checks.txt)。

后台隐藏窗口下，默认 DirectX 12 的截图阶段曾返回空纹理并退出。改用 DirectX 11 后程序检查通过，但隐藏窗口截图为黑屏；本次没有重新完成可见窗口的视觉验收，展示图沿用原版留存图。

没有把历史所有专项、旧版本报告或上述 12 项检查当作全游戏验收。
