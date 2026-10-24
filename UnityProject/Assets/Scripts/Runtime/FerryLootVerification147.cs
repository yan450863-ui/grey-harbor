using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
namespace GreyHarbor {
 public sealed partial class RaidVerification {
  sealed class BonusRoll147:System.Random {
   public double Value;public override double NextDouble()=>Value;public override int Next(int maxValue)=>maxValue-1;
  }
  void VerifyQuality147(){
   foreach(var tier in new[]{ContainerTier.Common,ContainerTier.Advanced}){
    var expected=tier==ContainerTier.Common?new double[]{60,32,7,1,0}:new double[]{20,40,30,10,0};
    var w=LootBalance.QualityWeights(tier);
    Check(Array.TrueForAll(new[]{0,1,2,3,4},i=>Math.Abs(w[i]-expected[i])<.00001),"147 Correct low/high normal quality table "+tier);
    foreach(double multiplier in new[]{1d,1.5d,2d}){
     var q=LootBalance.QualityWeights(tier,multiplier);double qualityTotal=0;foreach(var v in q)qualityTotal+=v;
     Check(Array.TrueForAll(q,v=>v>=0)&&Math.Abs(qualityTotal-100)<.00001&&q[4]==0,"147 Hard-mode quality normalization without normal red "+tier+" "+multiplier);
    }
    foreach(bool hard in new[]{false,true}){
     var bins=new int[5];var rng=new System.Random(147000+(int)tier);var target=LootBalance.QualityWeights(tier,hard?1.5:1);
     for(int i=0;i<50000;i++)bins[RaidInventory.Grade(LootBalance.Pick(SupplyType.Misc,rng,hard,tier))]++;
     Check(Array.TrueForAll(new[]{0,1,2,3,4},i=>Math.Abs(bins[i]/500d-target[i])<.8),"147 Actual normal quality samples match table "+tier+" hard="+hard);
    }
    int bonus=0,total=0;bool valid=true;var random=new System.Random(147100+(int)tier);
    for(int i=0;i<50000;i++){
     var items=HarborContent.Contents(SupplyType.Misc,random,false,tier);int red=items.FindAll(p=>RaidInventory.Grade(p.Kind)==4).Count;
     valid&=items.Count>=1&&items.Count<=7&&red<=1&&items.TrueForAll(p=>p.Identified==false&&p.RaidFound&&(RaidInventory.Grade(p.Kind)<4||RaidInventory.Group(p.Kind)==ItemGroup.Collection));
     bonus+=red;total+=items.Count;
    }
    Check(valid&&bonus>=25&&bonus<=80&&Math.Abs(total/50000d-2.211)<.03,"147 Independent per-box 0.1% bonus, at most one extra red and unchanged base quantity "+tier);
    results.Add("SAMPLE "+tier+" 50000 boxes; red bonuses="+bonus+" mean items="+total/50000d);
   }
   var forced=new BonusRoll147{Value=0};
   foreach(SupplyType type in Enum.GetValues(typeof(SupplyType))){
    if(type==SupplyType.Safe)continue;
    var items=HarborContent.Contents(type,forced,false);
    Check(items.Count==7&&items.FindAll(p=>RaidInventory.Grade(p.Kind)==4).Count==1&&items[6].Count==1,"147 Six base items plus one forced bonus supported "+type);
   }
   foreach(var category in new[]{CollectionCategory.Electronics,CollectionCategory.Collectibles}){
    var items=HarborContent.Contents(SupplyType.Misc,forced,false,ContainerTier.Advanced,category);
    Check(items.Count==7&&items.TrueForAll(p=>RaidInventory.Category(p.Kind)==category)&&RaidInventory.Grade(items[6].Kind)==4,"147 Classified high-rate case keeps category including bonus "+category);
   }
   var cooler=DockContainers141.Contents(ContainerLook.MarineCooler,forced,false,ContainerTier.Common);
   Check(cooler.Count==7&&cooler.TrueForAll(p=>RaidInventory.Category(p.Kind)==CollectionCategory.Living||RaidInventory.Category(p.Kind)==CollectionCategory.Biological),"147 Cooler's special draw path includes classified bonus");
   var boundary=new BonusRoll147{Value=.001};var boundaryItems=new List<ItemStack>();ContainerBonus147.Add(boundaryItems,SupplyType.Misc,ContainerTier.Common,boundary);
   Check(boundaryItems.Count==0,"147 Exact 0.1% boundary does not award bonus");
   var safeItems=new List<ItemStack>();ContainerBonus147.Add(safeItems,SupplyType.Safe,ContainerTier.Safe,forced);
   Check(safeItems.Count==0&&LootBalance.QualityWeights(ContainerTier.Safe)[4]==2,"147 Existing safe rule remains separate from new low/high bonus");
  }
  IEnumerator SearchFerry147(LootContainer loot){
   Check(Clear140(loot.Position)&&Walk140(loot.Position),"147 Actual walk to container "+loot.Name);
   G.Player.Aim=(loot.Prop.transform.position-G.Player.transform.position).normalized;G.Rig.Tick(0);G.Vision.Refresh(G.Player.transform.position,G.Player.Aim);
   F111(false,0);F111(false,0);Debug.Log("147 INTERACT "+loot.Name+" player="+G.Player.transform.position+" bounds="+G.LootBounds(loot)+" allowed="+G.Vision.RoomAllowed(G.LootBounds(loot).center)+" can="+G.CanInteractLoot(loot)+" selected="+(G.Nearby==null?"none":G.Nearby.Name)+" door="+(G.NearbyDoor==null?"none":G.NearbyDoor.Name));Check(G.CanInteractLoot(loot)&&G.Nearby==loot,"147 Stand selects intended container by distance "+loot.Name);F111(true,loot.SearchDuration+.02f);
   Check(G.OpenLoot==loot&&loot.Opened&&loot.Prop.Searched,"147 F opens correct searchable prop "+loot.Name);
   G.Paused=false;for(int i=0;i<7;i++)G.TickIdentification(3);G.Paused=true;
   Check(loot.Items.Count>=1&&loot.Items.Count<=7&&loot.Items.TrueForAll(p=>p.Identified)&&loot.Items.FindAll(p=>RaidInventory.Grade(p.Kind)==4).Count<=1,"147 Ordinary and bonus items identified "+loot.Name);
   var category=HighValueContainers139.Category(loot.Prop.Look);
   if(category.HasValue)Check(loot.Tier==ContainerTier.Advanced&&loot.Items.TrueForAll(p=>RaidInventory.Category(p.Kind)==category),"147 High-rate case category and tier match "+loot.Name);
   G.Paused=false;G.Bag.Slots.Clear();G.Chest.Slots.Clear();Check(G.QuickTransfer(ItemArea.Loot,loot.Items[0].Cell),"147 New container loot can be picked up "+loot.Name);G.Paused=true;G.CloseInventory();yield return null;
  }
  IEnumerator VerifyFerryLoot147(){
   VerifyQuality147();FreshCamp();string saved=JsonUtility.ToJson(G.Save);G.HarborWalkthrough=true;G.HardMode=false;G.BeginTraining();G.Paused=true;yield return null;
   var outside=G.Map;var box=outside.Loot[0];var items=box.Items;
   var original=new List<ItemStack>(items);items.Clear();
   for(int i=0;i<6;i++)items.Add(new ItemStack(ItemKind.BlueArmor,1){Identified=false,RaidFound=true});
   items.Add(new ItemStack(ItemKind.HarborDiamond,1){Identified=false,RaidFound=true});
   var grid=G.EnsureLootGrid(box);
   Check(grid.Slots.Count==7&&grid.UsedCells==55&&grid.Slots.TrueForAll(p=>grid.Fits(p.Kind,p.Cell,p)),"147 Seven-item grid expands for six large items and one red without overlap/loss");
   items.Clear();items.AddRange(original);G.EnsureLootGrid(box);
   Check(G.SavePath.Contains("1.47")&&outside.Loot.Count==30&&G.Enemies.Count==12,"147 Isolated preview and unchanged outdoor density");
   G.Player.transform.position=RaidMap.FerryApproach143;Physics.SyncTransforms();F111(false,0);F111(true,2.01f);G.Paused=true;yield return null;
   var first=G.Map;Check(first.FerryLoot146.Count==9&&first.FerryLoot146.TrueForAll(l=>l.Tier==ContainerTier.Common),"147 First floor now nine ordinary containers");
   VerifyFerryActors146(false);yield return VerifyFerryRooms145();VerifyFerryPatrol146();
   foreach(var loot in first.FerryLoot146.GetRange(7,2))yield return SearchFerry147(loot);
   yield return FerryPicture143("147-ferry-floor1.png");
   Check(Walk140(new Vector3(2.8f,0,12.3f)),"147 First-floor front stairs reachable");F111(false,0);F111(true,2.01f);G.Paused=true;yield return null;
   var second=G.Map;Check(second.FerryLoot146.Count==8&&second.FerryLoot146.FindAll(l=>l.Tier==ContainerTier.Common).Count==6&&second.FerryLoot146.FindAll(l=>l.Tier==ContainerTier.Advanced).Count==2,"147 Second floor six ordinary and two high-rate containers");
   VerifyFerryActors146(true);yield return VerifyFerryRooms145();VerifyFerryPatrol146();
   var key=second.FerryRooms143.Find(r=>r.Door.Locked);var center=new Vector3(key.Area.center.x,0,key.Area.center.y);
   Check(!G.Vision.RoomAllowed(center)&&!second.Loot.Exists(l=>key.Contains(new Vector2(l.Prop.transform.position.x,l.Prop.transform.position.z))),"147 Locked VIP room still black and empty");
   foreach(var loot in second.FerryLoot146.GetRange(5,3))yield return SearchFerry147(loot);
   // Search the old five stands because the added high-rate desk changes the furnished room.
   foreach(var loot in second.FerryLoot146.GetRange(0,5))Check(Clear140(loot.Position)&&Walk140(loot.Position),"147 Existing upper search stand remains reachable "+loot.Name);
   yield return FerryPicture143("147-ferry-floor2.png");
   var high=second.FerryLoot146.Find(l=>l.Prop.Look==ContainerLook.PrecisionCase);
   string contents=string.Join(",",high.Items.ConvertAll(p=>p.Id+":"+p.Kind+":"+p.Count+":"+p.Identified));
   Check(Walk140(new Vector3(0,0,-13.2f)),"147 Upper rear stairs reachable");F111(false,0);F111(true,2.01f);G.Paused=true;yield return null;
   Check(G.Map==first&&first.FerryLoot146.Count==9&&first.FerryLoot146.GetRange(7,2).TrueForAll(l=>l.Opened),"147 Added lower containers retain search state across floors");
   Check(Walk140(new Vector3(0,0,-13.2f)),"147 Lower rear stairs reachable");F111(false,0);F111(true,2.01f);G.Paused=true;yield return null;
   Check(G.Map==second&&second.Loot.Count==8&&high.Opened&&high.Prop.Searched&&contents==string.Join(",",high.Items.ConvertAll(p=>p.Id+":"+p.Kind+":"+p.Count+":"+p.Identified)),"147 High-rate roll and pickup state retained without reroll/respawn");
   G.SwitchInterior("ferry-floor:1:front");G.Paused=true;yield return null;Check(Walk140(new Vector3(-6,0,0)),"147 Ship exit reachable");
   F111(false,0);F111(true,2.01f);G.Paused=true;yield return null;
   Check(G.Map==outside&&G.Enemies.Count==12,"147 Outside map and actors restored");
   G.Player.transform.position=outside.Exits[1];G.Controls=new TouchInput();G.Paused=false;G.TickExtraction(5.1f);yield return null;
   Check(G.Phase==GamePhase.Base&&JsonUtility.ToJson(G.Save)==saved,"147 Preview restores pre-entry inventory");
  }
 }
}
