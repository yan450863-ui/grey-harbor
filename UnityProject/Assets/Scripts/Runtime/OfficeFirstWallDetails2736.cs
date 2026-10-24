using UnityEngine;
namespace GreyHarbor {public sealed partial class RaidMap {
 void OfficeFirstWallDetails2736(){
  OfficePanel2736("02",new Vector3(-19.82f,2.1f,-8),270,"Window");
  OfficePanel2736("03",new Vector3(6,2.15f,-23.82f),180,"Window");OfficePanel2736("03",new Vector3(16,2.15f,-23.82f),180,"Window");
  OfficePanel2736("04",new Vector3(-19.82f,2.05f,8),270,"Window");OfficePanel2736("07",new Vector3(19.82f,2.1f,6),90,"Window");
  OfficePanel2736("10",new Vector3(4,2.15f,23.82f),0,"Window");
  OfficePanel2736("05",new Vector3(-5.82f,1.55f,.5f),270,"Mirror");OfficePanel2736("06",new Vector3(-5.82f,1.55f,7.5f),270,"Mirror");
 }
 void OfficePanel2736(string id,Vector3 p,float yaw,string type){
  var room=OfficeRooms2726.Find(r=>r.Id==id);int before=FreightDressingRecords2717.Count;FreightWallPanel2717("Office"+type+"2736",room.Name,p,yaw,type);
  for(int i=before;i<FreightDressingRecords2717.Count;i++)OfficeDressingRecords2736.Add(FreightDressingRecords2717[i]);
 }
}}
