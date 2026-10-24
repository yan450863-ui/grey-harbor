using UnityEngine;
using UnityEngine.Rendering;
using System.Collections.Generic;
namespace GreyHarbor {public sealed partial class RaidMap {
 public readonly List<GameObject> OfficeRoomBatches2736=new List<GameObject>();
 public int OfficeOriginalRenderers2736,OfficeCombinedRenderers2736;
 void BatchOfficeFirstFurniture2736(){
  // Only immutable decorative meshes are combined. Search containers, doors and all colliders stay separate.
  foreach(var room in OfficeRooms2726){
   var go=new GameObject("OfficeRoomColourBatch2736_"+room.Id);go.transform.SetParent(Root,false);go.transform.position=room.Center;var life=go.AddComponent<WorkshopArtResources>();
   var groups=new Dictionary<string,List<CombineInstance>>();var materials=new Dictionary<string,Material>();var originals=new List<MeshRenderer>();
   foreach(var record in OfficeDressingRecords2736)if(record.RoomName==room.Name)foreach(var filter in record.View.GetComponentsInChildren<MeshFilter>()){
    var renderer=filter.GetComponent<MeshRenderer>();if(!renderer||!renderer.enabled||!filter.sharedMesh||!renderer.sharedMaterial||renderer.sharedMaterials.Length!=1)continue;
    var mat=renderer.sharedMaterial;string key=mat.shader.name+"|"+ColorUtility.ToHtmlStringRGBA(mat.color)+"|"+mat.GetFloat("_SolidFog").ToString("R")+"|"+mat.GetFloat("_Detail").ToString("R");
    if(!groups.ContainsKey(key)){groups.Add(key,new List<CombineInstance>());materials.Add(key,mat);}
    groups[key].Add(new CombineInstance{mesh=filter.sharedMesh,transform=go.transform.worldToLocalMatrix*filter.transform.localToWorldMatrix});originals.Add(renderer);
   }
   foreach(var group in groups){var mesh=new Mesh{name="OfficeRoomColourMesh2736"};mesh.indexFormat=IndexFormat.UInt32;mesh.CombineMeshes(group.Value.ToArray(),true,true);mesh.RecalculateBounds();life.Meshes.Add(mesh);var part=new GameObject("OfficeRoomColour2736");part.transform.SetParent(go.transform,false);part.AddComponent<MeshFilter>().sharedMesh=mesh;var renderer=part.AddComponent<MeshRenderer>();renderer.sharedMaterial=materials[group.Key];renderer.shadowCastingMode=ShadowCastingMode.Off;renderer.receiveShadows=true;OfficeCombinedRenderers2736++;}
   foreach(var renderer in originals)renderer.enabled=false;OfficeOriginalRenderers2736+=originals.Count;OfficeRoomBatches2736.Add(go);
  }
 }
}}
