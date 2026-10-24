using UnityEngine;
namespace GreyHarbor {
 public sealed partial class RaidVerification {
  void VerifyFloor115(RaidMap map){
   if(map.InteriorId!="repair")VerifyInteriorArt116(map);
   var root=map.Root.Find("GroundSurfaces_115");Check(root!=null&&root.GetComponentsInChildren<Collider>().Length==0,"New floor finishes stay cosmetic: "+map.InteriorId);
   bool fog=true,flat=true;foreach(var renderer in root.GetComponentsInChildren<MeshRenderer>()){fog&=renderer.sharedMaterial.shader.name=="GreyHarbor/World"&&renderer.sharedMaterial.GetFloat("_SolidFog")==0;flat&=renderer.bounds.min.y>=.014f&&renderer.bounds.max.y<=.026f;}
   Check(fog&&flat,"Floor details remain flat and obey room fog: "+map.InteriorId);
   Check(map.Labels.Count==0&&map.Root.Find("FloorJoint")==null&&map.Root.Find("ConcreteJoint")==null,"Old ground labels and full-length strips removed: "+map.InteriorId);
   Check(root.Find("OldCeramicTiles")&&root.Find("MissingTileBed"),"Chipped ceramic and missing tiles present: "+map.InteriorId);
  }
  void VerifyYardFractures115(){
   var art=G.Map.Root.Find("YardSurfaceDetails_192");bool found=false,flat=true;foreach(var renderer in art.GetComponentsInChildren<MeshRenderer>())if(renderer.name=="GroundFracture"){found=true;flat&=renderer.bounds.size.y<.004f&&renderer.bounds.min.y>=.040f&&renderer.bounds.max.y<.045f;}
   Check(found&&flat,"Outdoor cracks are horizontal chipped fissures, not upright grass geometry");Check(art.Find("Cracks")==null&&G.Map.Labels.Count==0,"Old branch cracks and yard English ground names removed");
  }
 }
}
