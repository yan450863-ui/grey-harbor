using System;using System.Collections.Generic;using UnityEngine;using Random=System.Random;
namespace GreyHarbor {
 public static class CorpseDropRules325 {
  public static ItemStack Found(ItemKind kind,int count=1)=>new ItemStack(kind,count,ItemOrigin.Pending){RaidFound=true,Identified=false};
  static void Fit(List<ItemStack> target,int columns,int rows,IEnumerable<ItemStack> source){
   var grid=new RaidInventory(columns,rows,target);
   foreach(var item in source){if(item==null||item.Count<=0)continue;for(int c=0;c<grid.SlotCapacity;c++)if(grid.Fits(item,c)){item.Cell=c;target.Add(item);break;}}
  }
  public static CorpseLoot325 Soldier(RebelKind kind,int level,Random rng,bool hard,DropRegion230 region){
   level=Math.Max(1,level);bool advanced=level>1;
   var body=new CorpseLoot325{Soldier=true,EnemyName=RebelTuning145.Name(kind)+" · 遗体",Level=level,RigKind=advanced?ItemKind.StandardRig:ItemKind.SmallRig,PackKind=advanced?ItemKind.StandardPack:ItemKind.SmallPack};
   var rig=RaidInventory.Container(new ItemStack(body.RigKind,1),true);var bag=RaidInventory.Container(new ItemStack(body.PackKind,1),false);
   body.RigColumns=rig.Columns;body.RigRows=rig.Rows;body.BagColumns=bag.Columns;body.BagRows=bag.Rows;
   var ammoPool=ContainerDropRules230.Profile(ContainerLook.AmmoCase,SupplyType.Ammo,ContainerTier.Common,region);
   Fit(body.RigItems,rig.Columns,rig.Rows,ContainerDropRules230.Contents(ammoPool,rng,hard,false));
   if(rng.NextDouble()<(advanced?.30:.20)){
    var kit=Found(advanced&&rng.NextDouble()<.35?ItemKind.ArmorKit:ItemKind.GreenArmorKit);
    kit.RepairPoints320=(float)Math.Round(RepairKit320.Capacity(kit.Kind)*(.20+rng.NextDouble()*.60));Fit(body.RigItems,rig.Columns,rig.Rows,new[]{kit});
   }
   if(rng.NextDouble()<(advanced?.65:.55)){
    var coatPool=ContainerDropRules230.Profile(ContainerLook.Coat,SupplyType.Misc,ContainerTier.Common,region);
    coatPool.EnableBonusRed=false;ContainerDropRules230.Validate(coatPool);
    Fit(body.BagItems,bag.Columns,bag.Rows,ContainerDropRules230.Contents(coatPool,rng,hard,false));
   }
   if(rng.NextDouble()<(advanced?.30:.25)){
    ItemKind gunKind=SoldierProfiles2745.Weapon(kind);
    if(advanced&&rng.NextDouble()<.20)gunKind=gunKind==ItemKind.MP5?ItemKind.Dagger:ItemKind.AR16;
    var gun=Found(gunKind);int mods=rng.Next(3);var options=new List<ModDefinition30>();
    foreach(var mod in ModRules30.All)if(ModRules30.Compatible(gunKind,mod.Slot)&&(advanced||mod.Grade==0))options.Add(mod);
    while(mods-->0&&options.Count>0){var mod=options[rng.Next(options.Count)];var item=Found(mod.Kind);item.Identified=true;gun.Attachments30.Add(item);options.RemoveAll(d=>d.Slot==mod.Slot);}
    ModRules30.Validate(gun);
    // Loaded rounds come out of the same generated ammo budget, never a second free magazine.
    var matching=body.RigItems.Find(p=>WeaponRules279.Compatible(gunKind,p.Kind));
    if(matching!=null){gun.LoadedAmmo314=matching.Kind;gun.Loaded314=Math.Min(matching.Count,ModRules30.Profile(gun).MagazineSize);gun.LoadedFound314=gun.Loaded314;matching.Count-=gun.Loaded314;if(matching.Count==0)body.RigItems.Remove(matching);}
    WeaponMagazine314.Ensure(gun);Fit(body.WeaponItems,4,2,new[]{gun});
   }
   if(rng.NextDouble()<(advanced?.25:.20)){
    bool heavy=NpcRules270.Heavy(kind);var armor=Found(advanced?(heavy?ItemKind.HeavyArmor320:ItemKind.BlueArmor):(heavy?ItemKind.ReinforcedArmor320:ItemKind.Armor));
    armor.ArmorPoints=(float)Math.Round(ArmorBalance.Max(armor.Kind)*(.25+rng.NextDouble()*.45));Fit(body.ArmorItems,3,3,new[]{armor});
   }
   body.RigRecoverable=rng.NextDouble()<(advanced?.20:.12);body.PackRecoverable=rng.NextDouble()<(advanced?.15:.10);
   if(body.RigRecoverable)Fit(body.RigGearItems,2,2,new[]{Found(body.RigKind)});
   if(body.PackRecoverable)Fit(body.PackGearItems,2,2,new[]{Found(body.PackKind)});
   body.WeaponRecoverable=body.WeaponItems.Count>0;body.ArmorRecoverable=body.ArmorItems.Count>0;return body;
  }
  public static CorpseLoot325 Monster(MonsterKind kind,int level,Random rng,bool hard,DropRegion230 region,List<ItemStack> items){
   bool large=kind==MonsterKind.Shell||kind==MonsterKind.BallastHauler||kind==MonsterKind.Pipewhisker;bool advanced=level>1;
   var body=new CorpseLoot325{Soldier=false,EnemyName=HarborContent.MonsterNames[(int)kind]+" · 残骸",Level=Math.Max(1,level),BioColumns=advanced&&large?6:large||advanced?4:3,BioRows=large||advanced?4:3};
   var profile=new ContainerDropProfile230{Name="异种生物材料",Usage=ContainerUsage230.Biological,PrimaryCategory=CollectionCategory.Biological,Type=SupplyType.Misc,Tier=ContainerTier.Common,Region=region,MaxWidth=3,MaxHeight=3,EnableBonusRed=false};
   profile.AllowedCategories[(int)CollectionCategory.Biological]=true;
   for(int i=0;i<RaidInventory.KindCount;i++)if(RaidInventory.Grade((ItemKind)i)>(advanced?3:2))profile.Excluded.Add((ItemKind)i);
   profile.FixedCount=Math.Min(6,Math.Min(3,LootBalance.Count(rng))+(large?1:0)+(advanced?1:0));ContainerDropRules230.Validate(profile);
   Fit(items,body.BioColumns,body.BioRows,ContainerDropRules230.Contents(profile,rng,hard,false));return body;
  }
 }
 public sealed partial class RaidGame {
  DropRegion230 CorpseRegion325(Vector3 position){
   ContainerDropProfile230 closest=null;float best=float.MaxValue;
   foreach(var box in Map.Loot){if(box.Corpse325!=null||box.DropProfile230==null)continue;float d=(box.Position-position).sqrMagnitude;if(d<best){best=d;closest=box.DropProfile230;}}
   return closest!=null?closest.Region:Map.IsHarbor?DropRegion230.Entrance:DropRegion230.Training;
  }
  void CreateCorpse325(RaidActor enemy,bool soldier){
   if(enemy.CorpseContainer325!=null)return;EnemyQuestKill319(enemy);
   var rng=new Random(UnityEngine.Random.Range(0,int.MaxValue));var region=CorpseRegion325(enemy.transform.position);
   var box=Map.AddLoot(enemy.transform.position,"遗体",ItemKind.Scrap,0,ItemKind.Scrap,0,false);box.Items.Clear();
   box.Corpse325=soldier?CorpseDropRules325.Soldier(enemy.Rebel,enemy.NpcLevel,rng,ActiveHardMode,region):CorpseDropRules325.Monster(enemy.Monster,enemy.NpcLevel,rng,ActiveHardMode,region,box.Items);
   box.Corpse325.Portrait=enemy.Body!=null?enemy.Body.sprite:null;box.Name=box.Corpse325.EnemyName;box.SearchDuration=soldier?HarborContent.SearchSeconds(SupplyType.Ammo)+HarborContent.SearchSeconds(SupplyType.Misc):HarborContent.SearchSeconds(SupplyType.Misc);
   box.View.GetComponent<Renderer>().sharedMaterial=Map.Mat(soldier?"59675B":"695D51",true);enemy.CorpseContainer325=box;
  }
 }
}