using System;
using System.Collections.Generic;
using UnityEngine;
using Random=System.Random;
namespace GreyHarbor {
 public sealed class KeyDefinition2760 {
  public readonly ItemKind Kind;public readonly string Title,Room;public readonly int BuyPrice;
  public KeyDefinition2760(ItemKind kind,string title,string room,int price){Kind=kind;Title=title;Room=room;BuyPrice=price;}
 }
 public static class KeyRules2760 {
  public const int MaxUses=4;public const double CoatChance=.01;
  public static readonly KeyDefinition2760[] All={
   new KeyDefinition2760(ItemKind.EntranceStorageKey2760,"港口储藏室钥匙","入口附属房储藏室",12000),
   new KeyDefinition2760(ItemKind.FerryVipKey2760,"客轮贵宾舱钥匙","客轮二楼贵宾舱",12000),
   new KeyDefinition2760(ItemKind.RepairManagementKey2760,"维修管理室钥匙","维修主楼四楼钥匙管理室",20000),
   new KeyDefinition2760(ItemKind.OfficeDutyKey2760,"后勤值班室钥匙","办公左塔一楼后勤值班室",24000),
   new KeyDefinition2760(ItemKind.OfficeResearchKey2760,"项目研究室钥匙","办公右塔三楼项目研究室",32000),
   new KeyDefinition2760(ItemKind.YardHomeKey324,"生活小屋储藏室钥匙","后勤院生活小屋私人储藏室",12000)
  };
  public static bool IsKey(ItemKind kind)=>((int)kind>=266&&(int)kind<=270)||kind==ItemKind.YardHomeKey324;
  public static KeyDefinition2760 Get(ItemKind kind){if(!IsKey(kind))throw new ArgumentOutOfRangeException(nameof(kind));return All[kind==ItemKind.YardHomeKey324?5:(int)kind-266];}
  // Missing fields in legacy JSON default to a fresh key; explicit zero remains exhausted.
  public static int Remaining(ItemStack item)=>item==null||!IsKey(item.Kind)?0:item.KeyReady2760?Mathf.Clamp(item.KeyUses2760,0,MaxUses):MaxUses;
  public static void Ensure(ItemStack item){if(item==null||!IsKey(item.Kind))return;item.KeyUses2760=Remaining(item);item.KeyReady2760=true;}
  public static int FullSellPrice(ItemKind kind)=>Get(kind).BuyPrice*85/100;
  public static int SellPrice(ItemStack item)=>FullSellPrice(item.Kind)*Remaining(item)/MaxUses;
  public static string Description(ItemKind kind)=>"开启"+Get(kind).Room+"。每把4次，成功开锁扣1次，耗尽消失；同局重复开关不扣。占1格、不叠加。外套1%额外掉钥匙；营地可购买。";
  // Called only when a container's contents are initially generated; cached contents prevent rerolls.
  public static void RollCoat(ContainerLook look,Random rng,List<ItemStack> result){
   if(look!=ContainerLook.Coat||rng.NextDouble()>=CoatChance)return;
   result.Add(new ItemStack(All[rng.Next(All.Length)].Kind,1,ItemOrigin.Certified){RaidFound=true,Identified=false});
  }
 }
}
