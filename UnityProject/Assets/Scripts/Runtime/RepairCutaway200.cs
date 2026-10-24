using UnityEngine;
using UnityEngine.Rendering;
using System.Collections.Generic;
namespace GreyHarbor {
 public sealed class RepairCutaway200:MonoBehaviour {
  static readonly int PlayerId=Shader.PropertyToID("_RepairPlayer200");
  readonly List<Material> materials=new List<Material>();
  public void Prepare(Transform mapRoot){
   var shader=Resources.Load<Shader>("RepairCutaway200");if(!shader)return;var copies=new Dictionary<Material,Material>();
   foreach(var r in mapRoot.GetComponentsInChildren<MeshRenderer>()){
    bool wall=r.name=="RepairMainDoor210"||r.name=="RepairMainDoorPull210"||r.name=="RepairMainStairDoor210"||Under210(r.transform,mapRoot,"RepairMainWallWear210")||Under210(r.transform,mapRoot,"RepairMainExitArt210")||r.name=="InteriorWall"||r.name=="RepairSideRoomDoor200"||r.name=="RepairRoomDoorPull200"||Under210(r.transform,mapRoot,"RepairAnnexWallWear200")||Under210(r.transform,mapRoot,"RepairAnnexExitArt200");
    if(!wall)continue;var original=r.sharedMaterial;if(!copies.TryGetValue(original,out var material)){material=new Material(original){shader=shader,name="RepairCutaway200_"+original.name};copies.Add(original,material);materials.Add(material);}r.sharedMaterial=material;r.shadowCastingMode=ShadowCastingMode.Off;
   }
  }
  static bool Under210(Transform t,Transform root,string name){var group=root.Find(name);return group&&t.IsChildOf(group);}
  void LateUpdate(){var g=RaidGame.Instance;if(g&&g.Map!=null&&(g.Map.InteriorId=="repairannex"||g.Map.InteriorId=="repairmain1")&&g.Player)Shader.SetGlobalVector(PlayerId,g.Player.transform.position);}
  void OnDestroy(){foreach(var m in materials)if(m)Destroy(m);}
 }
}
