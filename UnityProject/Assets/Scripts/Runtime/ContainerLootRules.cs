using System;
using System.Collections.Generic;
namespace GreyHarbor {
 public static class ContainerLootRules {
  // Existing collection categories only: None, Living, ToolsParts, Electronics, Medical, Energy, Biological, Information, Collectibles.
  public static readonly int[,] Weights={
   {0,60,12,6,4,6,0,5,7}, {0,0,55,15,0,30,0,0,0}, {0,0,0,0,100,0,0,0,0},
   {0,0,0,0,0,0,0,0,0}, {0,0,35,30,0,20,0,15,0}, {0,10,10,10,10,10,5,25,25}
  };
  public static ItemKind Pick(SupplyType type,Random rng)=>LootBalance.Pick(type,rng,false);
 }
}
