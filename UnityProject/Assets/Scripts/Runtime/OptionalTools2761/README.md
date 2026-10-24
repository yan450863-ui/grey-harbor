# 隔离试玩控制台 2.7.61

入口：Esc → 设置 → 控制台。可用速度 1×、2×、5×；无敌只阻止存活玩家承受攻击，生命与护甲均保持，不复活；加钱范围 1～10000000，含 10000/100000 两个快捷按钮。速度和无敌不写入正式存档或常规设置，启动/退出恢复正常。加钱明确写入当前存档，训练、只读、加载期间和非营地/行动阶段拒绝；原子写入失败恢复完整 Save 快照。

## 构建隔离

所有功能代码位于 `TestConsole2761.cs`，整个文件受 `GREY_HARBOR_TEST_CONSOLE` 编译符号保护。普通运行核心仅保留中性的扩展委托：设置菜单入口/覆盖层、生命周期、状态显示、速度兼容门面和攻击否决；不保存控制台字段，不识别任何控制台状态或货币指令。

启用：独立 Unity 调用 `ConsoleBuildProfile2761.Enable` 后等待重新编译，另一进程调用现有 `BuildDemo.BuildOffice2726`。

屏蔽：调用 `ConsoleBuildProfile2761.Disable` 后重新编译，再构建。控制台 UI、免伤、加钱、加速实体均不编译；旧测试速度门面保持 1×，中性委托无人注册。

彻底移除：删掉本文件夹（包括测试控制台源码），移除上述符号，再构建。也可删除 Editor/ConsoleBuildProfile2761.cs 辅助文件。核心和旧检查源码不引用此文件夹中的类型。专项检查中涉及控制台 API 的代码同样必须放在对应编译条件内。

## 检查 API

- `OptionalFeatures2761.Registered`：是否注册可选设置入口。
- `OptionalFeatures2761.Clear()`：解除所有扩展委托并恢复 Time.timeScale=1。
- `TestConsole2761.Register()`：测试夹具可显式注册；正常启动由 BeforeSceneLoad 自动注册。
- `RaidGame.ResetOptionalFeatures2761()`：恢复 1× 与无敌关闭。
- `RaidGame.CheckSpeed142` / `SetCheckSpeed142(int)`：速度读取/设置；只接受 1、2、5。
- `RaidGame.ConsoleGodMode2761` / `SetConsoleGodMode2761(bool)`：会话免伤设置。
- `RaidGame.AddConsoleMoney2761(int)`：通过现有原子保存通道增加金币，布尔返回是否成功。
- `RaidUI.OpenConsole2761()` / `ConsoleOpen2761`：独立控制台页打开/状态。

攻击过滤位于 ApplyCombatHit 的任何护甲处理之前和 RaidActor.Damage 的直接伤害之前；只对当前玩家生效，敌人、训练靶与普通战斗规则保持。
