using UnityEngine;
using System.Collections.Generic;
namespace GreyHarbor {
 public sealed class RepairWallModel220:MonoBehaviour {public readonly List<Renderer> PerimeterWalls=new List<Renderer>();public int CapCount;}
 public sealed partial class RaidMap {
  public void FinishRepairWalls220(){
   var model=Root.gameObject.AddComponent<RepairWallModel220>();
   var a=new WorkshopWastelandArt(this);var walls=Root.GetComponentsInChildren<MeshRenderer>();
   foreach(var r in walls)if(r.name=="InteriorWall"){
    var b=r.bounds;bool horizontal=b.size.x>b.size.z;float length=horizontal?b.size.x:b.size.z;
    var along=horizontal?Vector3.right:Vector3.forward;var normal=horizontal?Vector3.forward:Vector3.right;
    a.Cube("RepairWallCap220","AFAB90",new Vector3(b.center.x,b.max.y+.035f,b.center.z),new Vector3(b.size.x+.045f,.07f,b.size.z+.045f));model.CapCount++;
    if(b.min.y>.1f)continue;
    bool perimeter=b.center.x<Bounds.xMin+.2f||b.center.x>Bounds.xMax-.2f||b.center.z<Bounds.yMin+.2f||b.center.z>Bounds.yMax-.2f;if(perimeter)model.PerimeterWalls.Add(r);
    foreach(float side in new[]{-1f,1f}){
     var face=new Vector3(b.center.x,0,b.center.z)+normal*side*((horizontal?b.size.z:b.size.x)*.5f+.025f);
     a.Panel("RepairWallUpperCoat220","9B9B81",face+Vector3.up*2.68f,along,Vector3.up,length,.95f);
     a.Panel("RepairWallUpperBand220","737E6A",face+normal*side*.008f+Vector3.up*2.25f,along,Vector3.up,length,.08f);
     a.Panel("RepairWallBase220","4B6053",face+normal*side*.01f+Vector3.up*.14f,along,Vector3.up,length,.26f);
     for(float t=-length*.5f+.06f;t<length*.5f;t+=4.3f){var p=face+along*t+normal*side*.014f;a.Cube("RepairWallPiers220","838D76",p+Vector3.up*1.56f,new Vector3(horizontal?.12f:.045f,3.12f,horizontal?.045f:.12f));}
    }
   }
   var group=new GameObject("RepairMainWallArt220");group.transform.SetParent(Root,false);var resources=group.AddComponent<WorkshopArtResources>();a.Bake(group,resources);
   var shader=GreyHarbor.FlashlightShaders2763.Load("RepairWall220");var copies=new Dictionary<Material,Material>();
   foreach(var r in Root.GetComponentsInChildren<MeshRenderer>()){
    bool wall=r.name=="InteriorWall"||r.name=="RepairMainDoor210"||r.name=="RepairMainDoorPull210";
    for(var t=r.transform;t&&t!=Root;t=t.parent)if(t.name.Contains("WallArt220")||t.name.Contains("WallWear220")||t.name=="RepairMainWallWear210"||t.name=="RepairMainExitArt210")wall=true;
    foreach(var item in MainDecorationItems274)if(item.WallMounted&&item.View&&r.transform.IsChildOf(item.View.transform)){wall=true;break;}
    if(!wall)continue;var original=r.sharedMaterial;if(!copies.TryGetValue(original,out var m)){m=new Material(original){shader=shader,name="RepairWall220_"+original.name};m.SetVector("_RepairWallRect220",new Vector4(Bounds.xMin+.35f,Bounds.yMin+.35f,Bounds.xMax-.35f,Bounds.yMax-.35f));copies.Add(original,m);resources.Materials.Add(m);}r.sharedMaterial=m;
    // Indoor visibility comes from the flashlight/fog rays. Exterior sun shadows
    // must not add an unrelated dark stripe along these closed-room walls.
    r.shadowCastingMode=UnityEngine.Rendering.ShadowCastingMode.Off;r.receiveShadows=false;
   }
  }
 }
}
