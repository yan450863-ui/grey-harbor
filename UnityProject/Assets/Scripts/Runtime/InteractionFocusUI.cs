using UnityEngine;
namespace GreyHarbor {
 public sealed partial class RaidUI {
  void InteractionFocus(){
   if(G.Nearby==null&&G.NearbyDoor==null)return;
   Bounds b=G.Nearby!=null?G.LootBounds(G.Nearby):new Bounds(G.NearbyDoor.Position+Vector3.up,Vector3.one*1.15f);
   Vector2 min=new Vector2(99999,99999),max=new Vector2(-99999,-99999);
   for(int i=0;i<8;i++){var p=b.center+Vector3.Scale(b.extents,new Vector3((i&1)==0?-1:1,(i&2)==0?-1:1,(i&4)==0?-1:1));var s=ScreenPoint(p);min=Vector2.Min(min,s);max=Vector2.Max(max,s);}
   min-=Vector2.one*5;max+=Vector2.one*5;float n=8;Color c=new Color(.60f,.85f,.72f,.75f);
   foreach(float x in new[]{min.x,max.x})foreach(float y in new[]{min.y,max.y}){Fill(new Rect(x==min.x?x:x-n,y,n,1.5f),c);Fill(new Rect(x,y==min.y?y:y-n,1.5f,n),c);}
  }
 }
}
