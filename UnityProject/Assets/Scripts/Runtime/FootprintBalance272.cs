using System;using System.Collections.Generic;
namespace GreyHarbor {
 public static class FootprintBalance272 {
  public static int Group(ItemKind kind){int cells=RaidInventory.Width(kind)*RaidInventory.Height(kind);if(cells==1||cells==2)return 0;if(cells==3)return 1;if(cells==4)return 2;throw new InvalidOperationException("收集品占格尚未配置尺寸档："+kind+" / "+cells);}
  public static bool Applies(ContainerDropProfile230 p,bool red=false)=>!red&&p.Usage!=ContainerUsage230.Safe&&p.Usage!=ContainerUsage230.Combat;
  public static double[] Weights(IList<ItemKind> pool){var present=new bool[3];foreach(var kind in pool)present[Group(kind)]=true;var weights=new[]{present[0]?80.0:0,present[1]?15.0:0,present[2]?5.0:0};double sum=weights[0]+weights[1]+weights[2];if(sum==0)throw new InvalidOperationException("尺寸档没有合法候选");for(int i=0;i<3;i++)weights[i]/=sum;return weights;}
  public static int PickGroup(IList<ItemKind> pool,Random rng){var weights=Weights(pool);double roll=rng.NextDouble();int last=-1;for(int i=0;i<3;i++)if(weights[i]>0){last=i;roll-=weights[i];if(roll<0)return i;}return last;}
 }
}
