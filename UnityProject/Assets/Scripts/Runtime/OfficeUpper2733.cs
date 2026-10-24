using UnityEngine;
namespace GreyHarbor {
 public sealed partial class RaidMap {
  public int OfficeFloor2733;
  public static Vector3 OfficeStairArrival2733(bool north)=>north?new Vector3(17,0,20):new Vector3(-17,0,-20);
  void OfficeSecondPlan2733(){
   string[] names={"南部主楼梯","信息技术室","财务与人事","开放办公区","大会议室","男卫生间","女卫生间","项目办公室","资料档案室","网络设备间","主管办公室","北部后勤楼梯"};
   for(int i=0;i<names.Length;i++)OfficeRooms2726[i].Name=names[i];
   OfficeOpenings2726.Clear();
   string[] o={"v","v","h","h","v","v","v","h","v","h","v"};
   float[] x={-2,2,-13,-13,-2,-2,2,-5,2,17,-14};
   float[] z={-8,-14,-3,11,.5f,7.5f,7,14,19,16,-20};
   for(int i=0;i<o.Length;i++)OfficeOpenings2726.Add(new OfficeOpening2726{Orientation=o[i],X=x[i],Z=z[i],Width=2,Type="door"});
   OfficeOpenings2726.Add(new OfficeOpening2726{Orientation="h",X=-8,Z=-16,Width=2.2f,Type="hole"});
   OfficeOpenings2726.Add(new OfficeOpening2726{Orientation="v",X=-8,Z=19,Width=2.2f,Type="hole"});
   OfficeBlockers2726.Clear();OfficeBlockers2726.Add(new Rect(-2,4.4f,1.6f,1.2f));OfficeBlockers2726.Add(new Rect(-12,11,1.2f,3));
  }
  void OfficeFloorAccess2733(){
   foreach(bool north in new[]{false,true}){
    if(OfficeFloor2733<3)OfficeStairPortal2735(north,OfficeFloor2733+1);
    if(OfficeFloor2733>1)OfficeStairPortal2735(north,OfficeFloor2733-1);
   }
   if(OfficeFloor2733==2){BuildOfficeBridgePortal2741();}
   if(!OfficeLeft2741&&OfficeFloor2733==2){

    OfficeFloorCovers2735(new[]{new Vector3(14,0,-19),new Vector3(-16,0,2)});
   }
   if(!OfficeLeft2741&&OfficeFloor2733==3)OfficeFloorCovers2735(new[]{new Vector3(8,0,-12),new Vector3(-15,0,7)});
  }
 }
}
