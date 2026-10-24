using UnityEngine;
namespace GreyHarbor {public sealed partial class RaidUI {
 LootContainer anchorBox340;GameObject anchorRoot340;Renderer[] anchorRenderers340;
 Vector2 ContainerAnchor340(LootContainer box){
  GameObject root=box.Prop?box.Prop.gameObject:box.View;
  if(anchorBox340!=box||anchorRoot340!=root){anchorBox340=box;anchorRoot340=root;anchorRenderers340=root?root.GetComponentsInChildren<Renderer>(true):null;}
  float left=float.PositiveInfinity,right=float.NegativeInfinity,top=float.PositiveInfinity;
  if(anchorRenderers340!=null)foreach(var renderer in anchorRenderers340){
   if(!renderer||!renderer.enabled||!renderer.gameObject.activeInHierarchy||renderer is LineRenderer||renderer.GetComponent<TextMesh>())continue;
   var b=renderer.bounds;if(b.size.sqrMagnitude<.00001f)continue;
   for(int i=0;i<8;i++){var world=new Vector3((i&1)==0?b.min.x:b.max.x,(i&2)==0?b.min.y:b.max.y,(i&4)==0?b.min.z:b.max.z);var p=ScreenPoint(world);left=Mathf.Min(left,p.x);right=Mathf.Max(right,p.x);top=Mathf.Min(top,p.y);}
  }
  return float.IsPositiveInfinity(top)?ScreenPoint(box.Position+Vector3.up*.5f):new Vector2((left+right)*.5f,top);
 }
}}
