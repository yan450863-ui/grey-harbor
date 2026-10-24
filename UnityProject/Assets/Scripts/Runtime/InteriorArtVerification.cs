using UnityEngine;
namespace GreyHarbor {
 public sealed partial class RaidVerification {
  void VerifyInteriorArt116(RaidMap map){
   var root=map.Root.Find("InteriorWastelandArt_116");Check(root!=null,"Interior has its own wasteland dressing: "+map.InteriorId);if(root==null)return;
   Check(root.GetComponentsInChildren<Collider>().Length==0&&root.GetComponentsInChildren<Light>().Length==0,"Dressing adds no colliders or leaking lights: "+map.InteriorId);
   bool fogged=true,bounded=true;int vertices=0;
   foreach(var r in root.GetComponentsInChildren<MeshRenderer>()){
    fogged&=r.sharedMaterial.shader.name=="GreyHarbor/World";
    var b=r.bounds;bounded&=map.Bounds.Contains(new Vector2(b.min.x,b.min.z))&&map.Bounds.Contains(new Vector2(b.max.x,b.max.z))&&b.min.y>=0&&b.max.y<2.31f;
    if(!map.Bounds.Contains(new Vector2(b.min.x,b.min.z))||!map.Bounds.Contains(new Vector2(b.max.x,b.max.z))||b.min.y<0||b.max.y>=2.31f)Debug.Log("ART BOUNDS "+r.name+" "+b);
    vertices+=r.GetComponent<MeshFilter>().sharedMesh.vertexCount;
   }
   Check(fogged&&bounded,"All new dressing stays inside map bounds with room fog: "+map.InteriorId);
   Check(root.Find("WallPaint")&&root.Find("PeelingPlaster")&&root.Find("ServicePipes")&&root.Find("DustyOpaqueGlass"),"Walls windows and pipe wear installed: "+map.InteriorId);
   Check(root.GetComponentsInChildren<MeshRenderer>().Length<120&&vertices<120000,"Decoration stays within draw and mesh budget: "+map.InteriorId);
   if(map.InteriorId=="mainhouse"){
    Check(root.Find("FadedEvacuationPlan")&&root.Find("SofaThrow")&&root.Find("ToolPegboard")&&root.Find("ClinicBlanket")&&root.Find("ArchiveDamp")&&root.Find("DryAlienScar"),"All six rooms have distinct purpose-specific art");
    Check(G.Navigation.ClearSegment(new Vector3(0,0,-8.85f),new Vector3(0,0,8.8f)),"Dressed main-house corridor remains clear end to end");
   }else Check(root.Find("DeadRadio")&&root.Find("ElectricalPanel")&&root.Find("StorageDamp"),"Booth retains watch-room and damp storage identity");
  }
 }
}
