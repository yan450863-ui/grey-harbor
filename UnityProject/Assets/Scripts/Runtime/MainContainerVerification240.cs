using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace GreyHarbor {
 public sealed partial class RaidVerification {
  IEnumerator VerifyMainContainers240(){
   FreshCamp();string saved=JsonUtility.ToJson(G.Save),formal=Path.Combine(Path.GetDirectoryName(G.SavePath),"stash-v40.json"),before=File.Exists(formal)?File.ReadAllText(formal):null;
   Check(G.SavePath.Contains("2.7"),"240 Isolated verification inventory");
   foreach(var size in new[]{SafeSize240.Small,SafeSize240.Large}){
    var p=ContainerDropRules230.Profile(ContainerLook.Safe,SupplyType.Safe,ContainerTier.Safe,DropRegion230.Repair,size);var q=ContainerDropRules230.QualityWeights(p,false);var hard=ContainerDropRules230.QualityWeights(p,true);bool equal=true;for(int g=0;g<5;g++)equal&=Math.Abs(q[g]-hard[g])<.000001;
    Check(equal&&q[4]==0&&p.AllowRed,"240 Size safe has independent fixed grade table and separate red "+size);
    Check(size==SafeSize240.Large?q[0]==0&&Math.Abs(q[1]-.05)<.000001&&Math.Abs(q[2]-.45)<.000001&&Math.Abs(q[3]-.50)<.000001:Math.Abs(q[0]-5.0/98)<.000001&&Math.Abs(q[3]-23.0/98)<.000001,"240 Declared base safe probabilities "+size);
    var rng=new System.Random(24000+(int)size);int boxes=20000,reds=0,baseCount=0;var grades=new int[4];bool legal=true;
    for(int n=0;n<boxes;n++){var items=ContainerDropRules230.Contents(p,rng,false);var seen=new HashSet<ItemKind>();foreach(var item in items){int grade=RaidInventory.Grade(item.Kind);legal&=ContainerDropRules230.Eligible(p,item.Kind,grade==4)&&RaidInventory.Category(item.Kind)!=CollectionCategory.Biological;if(grade==4){reds++;legal&=seen.Add(item.Kind);}else {grades[grade]++;baseCount++;}}}
    Check(legal,"240 Safe contents legal and no biological samples "+size);Check(Math.Abs(reds/(double)boxes-MainSafeRules240.RedChance(size))<.006,"240 Independent safe bonus red "+size+" actual "+reds+"/"+boxes);
    Check(Math.Abs(baseCount/(double)boxes-2.21)<.04,"240 Safe base count retains 2.21 average "+size);for(int g=0;g<4;g++)Check(Math.Abs(grades[g]/(double)baseCount-q[g])<.015,"240 Sampled safe base grade "+size+"/"+g);
   }
   var old=ContainerDropRules230.Profile(ContainerLook.Safe,SupplyType.Safe,ContainerTier.Safe,DropRegion230.Training);Check(!old.AllowRed&&Math.Abs(ContainerDropRules230.QualityWeights(old,false)[4]-.02)<.000001,"240 Existing legacy safes retain original red quality and no extra bonus");
   G.HardMode=false;G.HarborWalkthrough=true;G.BeginTraining();G.Paused=true;yield return null;var outside=G.Map;Check(outside.Loot.Count==30&&G.Enemies.Count==16,"240 Exterior contents density and enemy count unchanged");G.SwitchInterior("repair-entry:center");G.Paused=true;yield return null;
   // The public entry keys are source-defined, not guessed.
   if(!G.Map.RepairMain220){G.SwitchInterior("repairmain1");G.Paused=true;yield return null;}
   var maps=new RaidMap[4];var snapshots=new string[4];int small=0,large=0,total=0;var rows=new List<string>{"floor,name,look,tier,safeSize,standX,standZ,modelX,modelY,modelZ,qualityNormal,extraRed"};
   for(int floor=1;floor<=4;floor++){
    var map=G.Map;maps[floor-1]=map;Check(map.RepairMain220&&map.RepairFloor220==floor,"240 Enter actual main building floor "+floor);Check(map.Loot.Count==(floor==4?6:floor==2?8:7)&&G.Enemies.Count==(floor==1||floor==4?3:2),"240 Intended container count and enemy population "+floor);
    foreach(var room in map.RepairMainRooms210){if(room.Door.Locked){Check(!map.Loot.Exists(l=>room.Area.Contains(new Vector2(l.Prop.transform.position.x,l.Prop.transform.position.z))),"240 Locked key room remains empty");continue;}if(!room.ReservedStair){foreach(var l in map.Loot)if(room.Area.Contains(new Vector2(l.Prop.transform.position.x,l.Prop.transform.position.z))){G.Player.transform.position=l.Position;Physics.SyncTransforms();Check(!G.CanInteractLoot(l),"240 Closed room does not reveal loot "+l.Name);}}
     G.Player.transform.position=room.ApproachPoint;Physics.SyncTransforms();Check(Clear140(room.ApproachPoint),"240 Room door approach clear "+room.Name);if(!room.Door.Open)Check(G.ToggleRoomDoor(room.Door),"240 Door opens with added containers "+room.Name);
    }
    G.Navigation=new RaidNavigation(map.Bounds,.25f);G.Player.transform.position=floor==1?map.Spawn:RaidMap.RepairStairArrival220(true);Physics.SyncTransforms();
    foreach(var diagnostic in map.Loot){foreach(var hit in Physics.OverlapCapsule(diagnostic.Position+Vector3.up*.35f,diagnostic.Position+Vector3.up,.34f,SceneObstacle.MovementMask))Debug.Log("240 BLOCKED "+diagnostic.Name+" by "+hit.name+" bounds "+hit.bounds);}
    foreach(var l in map.Loot){
     var p=l.DropProfile230;Check(p!=null&&p.Region==DropRegion230.Repair&&l.Items.Count>=1&&l.Items.Count<=7,"240 Container has declared regional profile and quantity "+l.Name);Check(l.Items.TrueForAll(i=>ContainerDropRules230.Eligible(p,i.Kind,RaidInventory.Grade(i.Kind)==4)),"240 Container loot respects purpose "+l.Name);
     if(p.SafeSize==SafeSize240.Small)small++;if(p.SafeSize==SafeSize240.Large)large++;total++;
     if(!ProbeMain240(Clear140(l.Position),"240 Search stand physically clear "+l.Name+" "+l.Position))continue;if(!ProbeMain240(Walk140(l.Position),"240 Actual player walk reaches container "+l.Name))continue;G.Player.Aim=(l.Prop.transform.position-G.Player.transform.position).normalized;G.Rig.Tick(0);G.Vision.Refresh(G.Player.transform.position,G.Player.Aim);
     if(!ProbeMain240(G.CanInteractLoot(l),"240 Search reachable without sight obstruction "+l.Name))continue;F111(false,0);F111(true,l.SearchDuration+.02f);if(!ProbeMain240(l.Opened&&G.OpenLoot==l&&l.Prop.Searched,"240 Real F selects and opens intended container "+l.Name)){G.CloseInventory();continue;}G.Paused=false;for(int n=0;n<7;n++)G.TickIdentification(3);G.Paused=true;Check(l.Items.TrueForAll(i=>i.Identified),"240 Container identification completes "+l.Name);G.CloseInventory();
     Vector3 model=l.Prop.transform.position;rows.Add(floor+","+Csv230(l.Name)+","+l.Prop.Look+","+l.Tier+","+p.SafeSize+","+l.Position.x+","+l.Position.z+","+model.x+","+model.y+","+model.z+","+Csv230(string.Join("/",ContainerDropRules230.QualityWeights(p,false)))+","+(p.SafeSize==SafeSize240.Legacy?p.AllowRed?.001:0:MainSafeRules240.RedChance(p.SafeSize)));
    }
    foreach(var door in map.Doors)if(door.Portal){var goal=door.Position;if(door.Destination.StartsWith("repair-exit:")){int index=Array.IndexOf(RaidMap.RepairMainGateKeys220,door.Destination.Substring(12));goal=RaidMap.RepairMainEntryArrival220(index);}Check(Clear140(goal)&&Walk140(goal),"240 Exit and staircase remains reachable "+door.Name);}
    snapshots[floor-1]=JsonUtility.ToJson(new ItemList230{Items=CollectMain240(map)});
    yield return RepairWorldPicture149("240-floor"+floor+"-containers.png",Vector3.zero,45,22,false,1800,1400);
    if(floor<4){G.SwitchInterior("repair-floor:"+(floor+1)+":north");G.Paused=true;yield return null;}
   }
   Check(total==28&&small==2&&large==1,"240 Four floors total 28 containers with two small and one large safe");File.WriteAllLines(Path.Combine(folder,"main-container-layout-240.csv"),rows);
   for(int floor=3;floor>=1;floor--){G.SwitchInterior("repair-floor:"+floor+":south");G.Paused=true;yield return null;Check(G.Map==maps[floor-1]&&snapshots[floor-1]==JsonUtility.ToJson(new ItemList230{Items=CollectMain240(G.Map)})&&G.Map.Loot.TrueForAll(l=>l.Opened),"240 Floor revisit preserves generated contents identities and opened states "+floor);}
   G.SwitchInterior("repair-exit:"+RaidMap.RepairMainGateKeys220[1]);G.Paused=true;yield return null;Check(G.Map==outside&&outside.Loot.Count==30&&G.Enemies.Count==16,"240 Leaving main building restores unchanged exterior");
   G.Controls=new DesktopInput();G.Finish(false);yield return null;Check(JsonUtility.ToJson(G.Save)==saved,"240 Preview restores camp inventory");Check(before==null?!File.Exists(formal):File.ReadAllText(formal)==before,"240 Formal save unchanged");Check(!results.Exists(s=>s.StartsWith("FAIL")),"240 All container placement and search checks passed");
  }
  bool ProbeMain240(bool pass,string message){results.Add((pass?"PASS ":"FAIL ")+message);Debug.Log(results[results.Count-1]);return pass;}
  List<ItemStack> CollectMain240(RaidMap m){var items=new List<ItemStack>();foreach(var b in m.Loot)items.AddRange(b.Items);return items;}
 }
}
