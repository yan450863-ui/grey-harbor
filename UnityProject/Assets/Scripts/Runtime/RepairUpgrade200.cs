using UnityEngine;
namespace GreyHarbor {public sealed partial class RaidMap {
 void BuildRepairUpgrade200(){
  FacadeClearance200.Create(RepairDistrictRoot149,"主楼",RepairRect149(13,88,27,32));FacadeClearance200.Create(RepairDistrictRoot149,"附属棚",RepairRect149(13,66,18,19));
  AddRepairAnnexEntrance200();AddRepairMainEntrance210();var kit=new RepairInteriorKit200(this);var repair=new RepairExteriorKit149(this);
  // Finish each side with functional facilities while preserving the existing walking loop.
  RepairParent149(kit.ToolWallBench(PortPoint(43.2f,83),0));
  RepairParent149(kit.EngineBench(PortPoint(41.8f,98),270));
  RepairParent149(kit.ToolWallBench(PortPoint(41.9f,114),270));
  RepairParent149(kit.PartsRack(PortPoint(30.8f,126.5f)));
  RepairParent149(repair.WeldingCart(PortPoint(41.7f,102.5f),90));
  RepairParent149(repair.AxleStand(PortPoint(41.6f,108.2f)));
  var a=new WorkshopWastelandArt(this);var p=PortPoint(26.5f,104);float left=p.x-13.5f,right=p.x+13.5f,rear=p.z+16;
  // Rear facade now reads as the fourth side of the same building, not a blank slab.
  for(int level=0;level<4;level++){
   float h=level*3.6f;a.Cube("RepairRearFloorBand200","9BA58C",new Vector3(p.x,h+3.4f,rear+.06f),new Vector3(27,.2f,.16f));
   for(int i=0;i<6;i++)RepairWindow149(a,new Vector3(left+2.5f+i*4.4f,h+1.7f,rear+.10f),Vector3.right,Vector3.forward,2.65f,1.3f);
  }
  foreach(float x in new[]{left+.28f,right-.28f})a.Tube("RepairRearDrain200","506A59",new Vector3(x,.2f,rear+.25f),new Vector3(x,14.2f,rear+.25f),.065f);
  foreach(float z in new[]{96f,109f}){var q=PortPoint(40,z);a.Cube("RepairExteriorAC200","82917A",q+new Vector3(.27f,2.25f,0),new Vector3(.5f,.75f,1.7f));for(int j=0;j<7;j++)a.Cube("RepairACSlat200","435E4E",q+new Vector3(.535f,1.98f+j*.08f,0),new Vector3(.03f,.03f,1.5f));}
  foreach(float z in new[]{93f,115f}){var q=PortPoint(13,z);a.Cube("RepairServicePanel200","566B5C",q+new Vector3(-.2f,1.35f,0),new Vector3(.28f,.8f,.65f));a.Panel("RepairServiceWarning200","A69B70",q+new Vector3(-.351f,1.42f,0),Vector3.forward,Vector3.up,.27f,.22f);}
  a.Tube("RepairWestWaterPipe200","857D63",new Vector3(left-.22f,.58f,p.z-14),new Vector3(left-.22f,.58f,p.z+14),.055f);
  foreach(float z in new[]{91f,105f,117f})a.Patch("RepairWestDamp200","657660",PortPoint(12.85f,z)+Vector3.up*1.3f,Vector3.forward,Vector3.up,2,1.3f,true);
  foreach(float x in new[]{18f,27f,36f})a.Patch("RepairRearWear200","A5A084",PortPoint(x,120.16f)+Vector3.up*1.35f,Vector3.right,Vector3.up,1.3f,1.8f,true);
  a.Bake(RepairDistrictRoot149.gameObject,RepairDistrictRoot149.gameObject.AddComponent<WorkshopArtResources>());
 }
}}
