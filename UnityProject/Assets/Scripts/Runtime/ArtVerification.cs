using System.Collections;
using UnityEngine;
namespace GreyHarbor {
 public sealed partial class RaidVerification {
  IEnumerator VerifyArt(){
   FreshCamp();G.Paused=true;G.CloseInventory();
   Check(SoldierArt.Loaded&&PixelArt.Hero[0].texture==Resources.Load<Texture2D>("CharacterArt/SoldierShuffle"),"Approved soldier art is present in built player");
   bool complete=true;for(int i=0;i<15;i++){var k=(ItemKind)i;var t=Resources.Load<Texture2D>("ItemArt/"+k);complete&=t!=null&&ItemIcons.Get(k)==t&&ItemIcons.Bounds(k).width>0;}
   Check(complete,"All 15 inventory kinds load their authored transparent artwork");
   var p=G.Player;p.Move(Vector3.zero);p.Draw(G.View,0);var idle=p.Body.sprite;
   p.Move(Vector3.right*.65f);p.Draw(G.View,0);
   Check(p.Body.sprite!=idle&&Mathf.Abs(p.Body.sprite.bounds.min.y)<.0001f&&Mathf.Abs(idle.bounds.min.y)<.0001f,"Movement changes shuffle frame while both soles stay anchored");
   p.Move(Vector3.zero);p.Draw(G.View,0);Check(p.Body.sprite==idle,"Stopping restores standing pose");
   p.Facing=1;p.Draw(G.View,0);bool right=p.Body.flipX;p.Facing=-1;p.Draw(G.View,0);Check(right&&!p.Body.flipX,"Same soldier artwork mirrors for left and right aiming");
   Check(p.Gun.sprite.texture!=ItemIcons.Get(ItemKind.AK)&&p.HitCollider.enabled,"World weapon remains independent and character collision remains enabled");
   // Place a solid wall almost tangent to the movement capsule, then push into it.
   var wall=new GameObject("ArtVerificationWall");wall.layer=8;wall.transform.position=p.transform.position+new Vector3(.801f,.7f,0);
   wall.AddComponent<BoxCollider>().size=new Vector3(1,2,2);Physics.SyncTransforms();
   p.Move(Vector3.right*.1f);p.Draw(G.View,0);Check(p.Body.sprite==idle,"Pushing against a blocking wall does not play walking frames");
   Destroy(wall);yield return null;
   G.Paused=false;FreshCamp();
  }
 }
}
