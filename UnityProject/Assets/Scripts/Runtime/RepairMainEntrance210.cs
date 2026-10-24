using UnityEngine;
namespace GreyHarbor {public sealed partial class RaidMap {
 public static readonly Vector3 RepairMainApproach210=PortPoint(35,86.25f);
 public static readonly string[] RepairMainGateKeys220={"west","center","east"};
 public static readonly string[] RepairMainGateNames220={"西卷帘门","中卷帘门","东卷帘门"};
 public static float RepairMainEntryX220(int index)=>MainPoint273(new Vector3(index==0?-7.5f:index==1?0:8.5f,0,0)).x;
 public static Vector3 RepairMainApproach220(int index)=>PortPoint(index==0?19:index==1?26.5f:35,86.25f);
 public static Vector3 RepairMainEntryArrival220(int index)=>new Vector3(RepairMainEntryX220(index),0,MainPoint273(new Vector3(0,0,-14.5f)).z);
 public void AddRepairMainEntrance210(){for(int i=0;i<3;i++)Doors.Add(new WorldDoor{Name=RepairMainGateNames220[i]+" · 进入维修主楼",Destination="repair-entry:"+RepairMainGateKeys220[i],Portal=true,Position=PortPoint(i==0?19:i==1?26.5f:35,87.48f)});}
}}
