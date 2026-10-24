using System;
using System.IO;
using System.Collections;
using UnityEngine;
namespace GreyHarbor {
 public sealed partial class RaidVerification {
  IEnumerator VerifyContainerTiers133(){
   double[][] expected={new double[]{60,32,7,1,0},new double[]{30,50,17,3,0},new double[]{5,30,40,23,2}};
   for(int tier=0;tier<3;tier++){
    var weights=LootBalance.QualityWeights((ContainerTier)tier);
    for(int g=0;g<5;g++)Check(Math.Abs(weights[g]-expected[tier][g])<.00001,"Normal quality table "+(ContainerTier)tier+" "+g);
    foreach(double multiplier in new[]{1d,1.5d,2d}){
     var w=LootBalance.QualityWeights((ContainerTier)tier,multiplier);double total=0;bool valid=true;foreach(var q in w){total+=q;valid&=q>=0&&!double.IsNaN(q);}Check(valid&&Math.Abs(total-100)<.00001,"Multiplier remains nonnegative and totals 100 "+tier+" "+multiplier);
    }
    foreach(bool hard in new[]{false,true}){
     var rng=new System.Random(1330+tier);var bins=new int[5];var w=LootBalance.QualityWeights((ContainerTier)tier,hard?1.5:1);SupplyType type=tier==2?SupplyType.Safe:SupplyType.Misc;
     for(int i=0;i<100000;i++)bins[RaidInventory.Grade(LootBalance.Pick(type,rng,hard,(ContainerTier)tier))]++;
     for(int g=0;g<5;g++)Check(Math.Abs(bins[g]/1000d-w[g])<.5,"Actual draws follow table "+tier+" "+hard+" "+g);
     results.Add("SAMPLE "+(ContainerTier)tier+" "+(hard?"hard":"normal")+" counts="+string.Join(",",bins));
    }
   }
   Check(LootBalance.TierFor(SupplyType.Misc)==ContainerTier.Common&&LootBalance.TierFor(SupplyType.Tools)==ContainerTier.Common&&LootBalance.TierFor(SupplyType.Medical)==ContainerTier.Common,"Ordinary material containers default common");
   Check(LootBalance.TierFor(SupplyType.Military)==ContainerTier.Advanced&&LootBalance.TierFor(SupplyType.Safe)==ContainerTier.Safe,"Military and safe default tiers");
   Check(LootBalance.TierFor(SupplyType.Misc,ContainerTier.Advanced)==ContainerTier.Advanced,"Important file cabinet supports explicit advanced override");
   bool rejected=false;try{LootBalance.TierFor(SupplyType.Misc,ContainerTier.Safe);}catch(ArgumentException){rejected=true;}Check(rejected,"Non-safe container cannot select red-bearing safe tier");
   var r=new System.Random(133);
   for(int t=0;t<6;t++){
    bool legal=true;for(int n=0;n<2000;n++)foreach(var item in HarborContent.Contents((SupplyType)t,r)){
     legal&=LootBalance.Eligible((SupplyType)t,item.Kind)&&item.Count>=1&&(t==5||RaidInventory.Grade(item.Kind)<4);
     if(t==3)legal&=item.Kind==ItemKind.Ammo&&item.Count>=10&&item.Count<=30;
    }Check(legal,"Container category, red restriction, ammo quantities retained "+(SupplyType)t);
   }
   var exact=new Roll122();int totalCount=0;int[] counts=new int[6];for(int i=0;i<1000;i++){exact.Roll=i;int c=LootBalance.Count(exact);counts[c-1]++;totalCount+=c;}
   Check(string.Join(",",counts)=="200,500,220,55,20,5"&&totalCount==2210,"Single-box quantity probabilities remain unchanged");
   var entryTypes=new[]{SupplyType.Misc,SupplyType.Misc,SupplyType.Medical,SupplyType.Misc,SupplyType.Misc,SupplyType.Misc,SupplyType.Misc,SupplyType.Tools,SupplyType.Tools,SupplyType.Medical,SupplyType.Misc};
   long value=0,items=0,rare=0;var rngEntry=new System.Random(133033);
   for(int raid=0;raid<5000;raid++)foreach(var type in entryTypes)foreach(var item in HarborContent.Contents(type,rngEntry,false,ContainerTier.Common)){value+=(long)RaidInventory.Value(item.Kind)*item.Count;items++;if(RaidInventory.Grade(item.Kind)>=2)rare++;}
   results.Add("ENTRY ESTIMATE 5000 raids; average all-container sale="+(value/5000d).ToString("F2")+" coins; items="+(items/5000d).ToString("F2")+"; purple/gold="+(rare/5000d).ToString("F2")+" (no monster drops or capacity limit)");
   Check(Math.Abs(items/5000d-24.31)<.35&&Math.Abs(rare/(double)items-.08)<.01,"Entry batch keeps item count but lowers purple/gold to about 8 percent");
   FreshCamp();G.HardMode=false;G.HarborWalkthrough=true;G.BeginTraining();G.Paused=true;yield return null;
   for(int n=0;n<2;n++){
    var approach=n==0?new Vector3(-1.3f,0,-53):new Vector3(22,0,-43);G.Player.transform.position=approach;Physics.SyncTransforms();F111(false,0);F111(true,2.01f);G.Paused=true;yield return null;
    Check(G.Map.Loot.Count==(n==0?3:8)&&G.Map.Loot.TrueForAll(l=>l.Tier==ContainerTier.Common),"All actual entrance containers use common table "+n);
    Check(G.Map.Loot.TrueForAll(l=>l.Items.TrueForAll(p=>RaidInventory.Grade(p.Kind)<4&&!p.Identified)),"New actual containers have unknown non-red loot "+n);
    if(n==1){G.Player.transform.position=new Vector3(-.9f,0,-3.5f);Check(G.ToggleRoomDoor(G.Map.Doors[0]),"Reception opens for loot check");}
    var l=G.Map.Loot[0];G.Player.transform.position=l.Position;Physics.SyncTransforms();F111(false,0);F111(true,l.SearchDuration+.01f);Check(l.Opened&&G.OpenLoot==l,"Changed loot still opens through F "+n);
    G.Paused=false;G.TickIdentification(RaidGame.IdentificationSeconds(l.Items[0].Kind)+.01f);var item=l.Items[0];Check(item.Identified&&G.QuickTransfer(ItemArea.Loot,item.Cell),"Changed loot identifies and transfers "+n);G.Paused=true;G.CloseInventory();
    G.Player.Aim=n==0?Vector3.forward:Vector3.back;G.Rig.ResetView(45);G.Rig.Tick(0);G.Vision.Refresh(G.Player.transform.position,G.Player.Aim);yield return Capture("133-entrance-loot-"+n+".png");G.SwitchInterior(n==0?"portbooth":"portservice");
   }
   G.Controls=new DesktopInput();G.Finish(false);yield return null;
  }
 }
}
