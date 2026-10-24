using UnityEngine;
using System.Collections.Generic;
namespace GreyHarbor {
 public sealed partial class RaidUI {
  struct MapFootprint {public Rect Bounds;public bool Low;}
  int miniRevision=-1;Transform miniRoot;readonly List<MapFootprint> miniFootprints=new List<MapFootprint>();
  void CacheMiniMap(){
   if(G.Map==null||G.Map.Root==null)return;
   if(miniRoot==G.Map.Root&&miniRevision==G.Map.LayoutRevision)return;miniRevision=G.Map.LayoutRevision;miniRoot=G.Map.Root;miniFootprints.Clear();
   foreach(var c in miniRoot.GetComponentsInChildren<Collider>()){
    if(!c.enabled||c.isTrigger||c.GetComponentInParent<RaidActor>()||(SceneObstacle.MovementMask&(1<<c.gameObject.layer))==0)continue;
    var b=c.bounds;miniFootprints.Add(new MapFootprint{Bounds=new Rect(b.min.x,b.min.z,b.size.x,b.size.z),Low=c.gameObject.layer==SceneObstacle.LowLayer});
   }
  }
  void MiniMap(){
   Label(35,25,76,23,G.Map.TransitWhitebox346?"白模":G.TrainingActive?"训练":G.ActiveHardMode?"困难":"普通",16,G.ActiveHardMode?gold:mint);HudIcon329(HudGlyph329.Clock,new Rect(109,28,16,16),muted);Label(133,25,102,23,$"{Mathf.FloorToInt(G.RaidTime/60):00}:{Mathf.FloorToInt(G.RaidTime%60):00}",16,white);
   HudPanel329(MiniMapFrame326);Rect r=MiniMapViewport326;var projection=PreparedTerrain331?PreparedProjection331:new MiniMapProjection(NearbyBounds318(G.Player.transform.position),r,G.View.transform.rotation);DrawMapTerrain318(G.Map,projection,false);DrawHeardSounds326(projection,r);PlayerMarker318(projection.Point(G.Player.transform.position),MarkerDirection318(projection,G.Player.transform.position,G.Player.Aim));
   Vector2 north=(projection.Point(Vector3.forward)-projection.Point(Vector3.zero)).normalized;Vector2 compass=new Vector2(219,82),tip=compass+north*8;MiniLine(compass-north*4,tip,mint,1.5f);Vector2 side=new Vector2(-north.y,north.x);MiniLine(tip,tip-north*4+side*2.5f,mint,1.5f);MiniLine(tip,tip-north*4-side*2.5f,mint,1.5f);Label(209,61,20,15,"N",10,mint,TextAnchor.MiddleCenter);Label(198,228,37,15,"18m",10,muted,TextAnchor.MiddleRight);
  }
  void MiniDot(MiniMapProjection projection,Vector3 world,float size,Color color){Vector2 p=projection.Point(world);Fill(new Rect(p.x-size/2,p.y-size/2,size,size),color);}
  void MiniLine(Vector2 a,Vector2 b,Color color,float thickness){var matrix=GUI.matrix;GUI.matrix=matrix*Matrix4x4.TRS(new Vector3(a.x,a.y,0),Quaternion.Euler(0,0,Mathf.Atan2(b.y-a.y,b.x-a.x)*Mathf.Rad2Deg),Vector3.one);Fill(new Rect(0,-thickness/2,Vector2.Distance(a,b),thickness),color);GUI.matrix=matrix;}
 }
}
