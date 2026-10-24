namespace GreyHarbor {public sealed partial class RaidGame {
 public string StatusLabel=>Player==null?"正常":Player.Dead?"倒地":RepairTime>0?"维修中":ReloadTime>0?"换弹中":Recovery321.Regenerating321?"自动回血":Player.Health<35?"重伤":Armor!=null&&TotalArmor<=0?"破甲":Player.Health<PlayerLife321.Maximum?"受伤":"正常";
 public string RecoveryStatus321=>Player==null?"脱战恢复":Player.Dead?"生命耗尽":Paused?"暂停":Player.Health>=PlayerLife321.Maximum?"生命已满 · 脱战后自动恢复":Recovery321.WaitRemaining321>0?$"脱战等待 {Recovery321.WaitRemaining321:0.0}秒":Sprint2711.Running?"奔跑中 · 生命恢复暂停":Recovery321.Regenerating321?$"自动恢复 +{RecoveryRate321:0.0}生命/秒":"脱战后自动恢复生命";
}}
