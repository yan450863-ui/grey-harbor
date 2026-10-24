using System;
using System.Collections.Generic;
namespace GreyHarbor {
 // A corpse rolls once on death; opening/searching it never rerolls its contents.
 public static class EnemyLoot146 {
  public static readonly ItemKind[] Fragments={ItemKind.Biomass,ItemKind.BoneFragment,ItemKind.Hide,ItemKind.AlienGland,ItemKind.Chitin,ItemKind.NerveBundle};
  static readonly int[,] FragmentWeights={{45,35,12,6,1,1},{30,35,25,7,1,2},{30,30,20,12,6,2},{40,30,10,17,1,2},{30,30,25,8,5,2},{35,20,20,10,12,3},{25,25,15,18,8,9}};
  static readonly ItemKind[] Supplies={ItemKind.Ammo,ItemKind.GreenArmorKit,ItemKind.ArmorKit};
  static readonly int[,] SupplyWeights={{75,22,3},{65,25,10},{65,25,10}};
  static ItemStack Stack(ItemKind k,int n=1)=>new ItemStack(k,n,ItemOrigin.Pending){RaidFound=true,Identified=false};
  public static List<ItemStack> Monster(MonsterKind kind,Random rng,int level=1){
   int roll=rng.Next(100),count=roll<75?1:roll<98?2:3;var list=new List<ItemStack>();
   for(int i=0;i<count;i++)list.Add(Stack(CollectibleData260.FragmentVariant(Fragments[HarborContent.Pick(rng,FragmentWeights,(int)kind)],rng)));NpcRules270.BoostLoot(list,level,rng);return list;
  }
  public static List<ItemStack> Soldier(RebelKind kind,Random rng,bool hard=false,int level=1){
   var list=new List<ItemStack>();int count=rng.Next(100)<70?1:2;
   for(int i=0;i<count;i++){var k=Supplies[HarborContent.Pick(rng,SupplyWeights,SoldierProfiles2745.IsNew(kind)?(level>=2?2:0):kind==RebelKind.BlueHeavy?1:(int)kind)];if(AmmoRules270.IsAmmo(k))k=SoldierProfiles2745.IsNew(kind)?SoldierProfiles2745.Ammo(kind):WeaponRules279.PickAmmo(rng,level>=2?1:0);list.Add(Stack(k,AmmoRules270.IsAmmo(k)?AmmoRules270.LootCount30(k,rng):1));}
   int chance=SoldierProfiles2745.IsNew(kind)?(level>=2?40:35):kind==RebelKind.Regular?35:kind==RebelKind.BlueRegular?40:45;
   if(rng.Next(100)<chance)list.Add(Stack(LootBalance.Pick(SupplyType.Misc,rng,hard,ContainerTier.Common,CollectionCategory.Collectibles)));
   NpcRules270.BoostLoot(list,level,rng);bool repair=false;for(int i=0;i<list.Count;i++)if(ConsumableRules.IsRepair(list[i].Kind)){if(repair){list.RemoveAt(i--);continue;}list[i].Count=1;repair=true;}return list;
  }
 }
}

