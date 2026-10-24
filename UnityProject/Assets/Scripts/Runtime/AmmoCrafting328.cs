using System;
using System.Collections.Generic;
namespace GreyHarbor {
 public static class AmmoCrafting328 {
  public static bool IsComponent(ItemKind k)=>k==ItemKind.BasicAmmoComponent328||k==ItemKind.PrecisionAmmoComponent328;
  public static string Name(ItemKind k)=>k==ItemKind.BasicAmmoComponent328?"基础弹药组件":"精密弹药组件";
  public static int Grade(ItemKind k)=>k==ItemKind.BasicAmmoComponent328?0:1;
  public static int Value(ItemKind k)=>k==ItemKind.BasicAmmoComponent328?30:60;
  public static string Description(ItemKind k)=>Name(k)+"：局外工作台加工的制作材料，非收藏品、不认证；占1格，每格100个，每个20克。商城不出售。"+(k==ItemKind.BasicAmmoComponent328?"用于绿色弹药及精密组件。认证机械废料15、枪械零件5、火药5加工100个。":"用于蓝色弹药。基础组件50加认证精密轴套5或稀有金属粉末5加工100个，需2级工作台。");
  public static ItemKind IconSource(ItemKind k)=>k==ItemKind.BasicAmmoComponent328?ItemKind.GunParts:ItemKind.PrecisionShaftBushing;
  static CampRecipe Component(string name,ItemKind output,int level,params ItemStack[] costs)=>new CampRecipe(name,output,100,level,costs){CertifiedInputs328=true};
  static CampRecipe Ammo(string name,ItemKind output,int amount,int level,params ItemStack[] costs)=>new CampRecipe(name,output,amount,level,costs);
  public static readonly CampRecipe[] All={
   Component("基础组件加工",ItemKind.BasicAmmoComponent328,0,new ItemStack(ItemKind.Scrap,15),new ItemStack(ItemKind.GunParts,5),new ItemStack(ItemKind.Powder,5)),
   Component("精密组件 · 轴套加工",ItemKind.PrecisionAmmoComponent328,2,new ItemStack(ItemKind.BasicAmmoComponent328,50),new ItemStack(ItemKind.PrecisionShaftBushing,5)),
   Component("精密组件 · 金属粉末加工",ItemKind.PrecisionAmmoComponent328,2,new ItemStack(ItemKind.BasicAmmoComponent328,50),new ItemStack(ItemKind.RareMetalPowder,5)),
   Ammo("绿色9毫米",ItemKind.Green9mm,300,1,new ItemStack(ItemKind.BasicAmmoComponent328,64)),
   Ammo("绿色5.56",ItemKind.Green556,300,1,new ItemStack(ItemKind.BasicAmmoComponent328,73),new ItemStack(ItemKind.GunParts,6)),
   Ammo("绿色7.62",ItemKind.GreenAmmo,300,1,new ItemStack(ItemKind.BasicAmmoComponent328,70),new ItemStack(ItemKind.Powder,3)),
   Ammo("绿色12号霰弹",ItemKind.Green12Gauge,150,1,new ItemStack(ItemKind.BasicAmmoComponent328,70),new ItemStack(ItemKind.Powder,3)),
   Ammo("蓝色9毫米",ItemKind.Blue9mm,300,2,new ItemStack(ItemKind.PrecisionAmmoComponent328,43)),
   Ammo("蓝色5.56",ItemKind.Blue556,300,2,new ItemStack(ItemKind.PrecisionAmmoComponent328,54),new ItemStack(ItemKind.GunParts,6)),
   Ammo("蓝色7.62",ItemKind.Ammo,300,2,new ItemStack(ItemKind.PrecisionAmmoComponent328,53),new ItemStack(ItemKind.Powder,3)),
   Ammo("蓝色12号霰弹",ItemKind.Blue12Gauge,150,2,new ItemStack(ItemKind.PrecisionAmmoComponent328,53),new ItemStack(ItemKind.Powder,3))};
  public static int Available(BaseSave save,CampRecipe recipe,ItemKind k,bool allowCertified)=>Certified(recipe,k)?save.Certified[(int)k]:!Certification319.Collection(k)||allowCertified?save.Stored[(int)k]:save.Stored[(int)k]-save.Certified[(int)k];
  public static bool Certified(CampRecipe recipe,ItemKind k)=>recipe.CertifiedInputs328&&Certification319.Collection(k);
  static bool Consume(RaidInventory grid,CampRecipe recipe,int batches,bool allowCertified,out string reason){
   reason="";foreach(var c in recipe.Costs){int n=checked(c.Count*batches);ItemOrigin? origin=Certified(recipe,c.Kind)?ItemOrigin.Certified:Certification319.Collection(c.Kind)&&!allowCertified?ItemOrigin.Ordinary:(ItemOrigin?)null;
    if(!grid.Consume(c.Kind,n,origin)){reason=(Certified(recipe,c.Kind)?"需要已认证的":"缺少")+RaidInventory.Name(c.Kind)+"："+grid.Count(c.Kind,origin)+" / "+n;return false;}}
   return true;
  }
  static bool ProduceComponent(BaseSave save,RaidInventory grid,ItemKind k,int needed,bool allowCertified,bool rare,out string reason){
   reason="";int missing=needed-grid.Count(k);if(missing<=0)return true;int batches=(missing+99)/100;var recipe=k==ItemKind.BasicAmmoComponent328?All[0]:All[rare?2:1];
   if(save.CampLevel<recipe.Level){reason="加工"+Name(k)+"需要"+recipe.Level+"级工作台";return false;}
   if(k==ItemKind.PrecisionAmmoComponent328&&!ProduceComponent(save,grid,ItemKind.BasicAmmoComponent328,50*batches,allowCertified,rare,out reason))return false;
   if(!Consume(grid,recipe,batches,allowCertified,out reason))return false;
   if(grid.Add(k,100*batches)>0){reason="仓库空间不足，无法存放加工后的组件";return false;}return true;
  }
  public static bool TryPlan(BaseSave save,CampRecipe recipe,int batches,bool allowCertified,bool autoProcess,bool useRarePowder,out RaidInventory result,out string reason){
   result=null;reason="";if(save==null||recipe==null||Array.IndexOf(CampRules.Recipes,recipe)<0||batches!=1&&batches!=3&&batches!=5){reason="请选择有效配方及1、3或5批";return false;}
   if(save.CampLevel<recipe.Level){reason="需要"+recipe.Level+"级工作台";return false;}if(!NpcProduction328.CanMake(save,recipe,out reason))return false;
   var grid=CampRules.Grid(save).Clone();
   if(autoProcess)foreach(var c in recipe.Costs)if(IsComponent(c.Kind)&&!ProduceComponent(save,grid,c.Kind,checked(c.Count*batches),allowCertified,useRarePowder,out reason))return false;
   if(!Consume(grid,recipe,batches,allowCertified,out reason))return false;
   if(grid.Add(recipe.Output,checked(recipe.Amount*batches))>0){reason="仓库空间不足，无法存放全部产物";return false;}result=grid;return true;
  }
  public static string Consumption(BaseSave save,RaidInventory planned){var text=new List<string>();foreach(var cost in save.Warehouse){if(text.Exists(x=>x.StartsWith(RaidInventory.Name(cost.Kind)+" ×")))continue;int n=CampRules.Grid(save).Count(cost.Kind)-planned.Count(cost.Kind);if(n>0)text.Add(RaidInventory.Name(cost.Kind)+" ×"+n);}return text.Count==0?"无材料消耗":string.Join("、",text.ToArray());}
 }
}
