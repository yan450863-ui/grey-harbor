using System.Collections.Generic;
using UnityEngine;
namespace GreyHarbor {
 public sealed partial class RaidMap {
  public Vector3[] OfficeIncomingFalls2748(){
   if(OfficeLeft2741)return OfficeFloor2733==1?new[]{new Vector3(-14,0,7)}:OfficeFloor2733==2?new[]{new Vector3(10,0,-17)}:new Vector3[0];
   return OfficeFloor2733==1?new[]{new Vector3(14,0,-19),new Vector3(-16,0,2)}:OfficeFloor2733==2?new[]{new Vector3(8,0,-12),new Vector3(-15,0,7)}:new Vector3[0];
  }
  public IEnumerable<Vector3> OfficeFallReservedPoints2748(){foreach(var p in OfficeIncomingFalls2748())yield return p;foreach(var h in OfficeFallHoles2748)yield return h.Center;}
 }
 public sealed partial class RaidGame {
  public float OfficeFallTime2748;
  public bool OfficeFallBlocked2748;
  public OfficeFloorHole2748 ActiveOfficeFall2748;
  RaidMap officeFallMap2748;
  public void ResetOfficeFall2748(){OfficeFallTime2748=0;ActiveOfficeFall2748=null;officeFallMap2748=null;}
  public bool OfficeFallLandingClear2748(Vector3 p){
   if(!Map.Bounds.Contains(new Vector2(p.x,p.z))||Physics.CheckCapsule(p+Vector3.up*.35f,p+Vector3.up*1.35f,.34f,SceneObstacle.MovementMask,QueryTriggerInteraction.Ignore))return false;
   foreach(var e in Enemies)if(!e.Dead&&Vector3.Distance(e.transform.position,p)<.7f)return false;
   return Navigation!=null&&Navigation.ClearSegment(p,p);
  }
  public void TickOfficeFall2748(float dt){
   if(IsLoading2721||Phase!=GamePhase.Raid||Paused||InventoryOpen||!Player||Player.Dead||Map==null||!Map.OfficeWhitebox2726||Map.IsOfficeBridge2741||Map.OfficeFloor2733<2){ResetOfficeFall2748();return;}
   var p=Player.transform.position;p.y=0;var hole=Map.OfficeFallHoles2748.Find(h=>Vector3.Distance(p,h.Center)<=h.TriggerRadius);
   if(hole==null){ResetOfficeFall2748();return;}
   if(officeFallMap2748!=Map||ActiveOfficeFall2748!=hole){OfficeFallTime2748=0;officeFallMap2748=Map;ActiveOfficeFall2748=hole;}
   OfficeFallTime2748+=Mathf.Max(0,dt);
   if(OfficeFallTime2748+0.00001f<2)return;
   int target=Map.OfficeFloor2733-1;ResetOfficeFall2748();
   SwitchInterior("office-fall:"+target+":"+p.x.ToString("R",System.Globalization.CultureInfo.InvariantCulture)+":"+p.z.ToString("R",System.Globalization.CultureInfo.InvariantCulture));
  }
 }
 public sealed partial class RaidUI {
  void DrawOfficeFall2748(){
   if(G.Phase!=GamePhase.Raid||G.Paused||G.InventoryOpen||G.ActiveOfficeFall2748==null)return;
   Fill(new Rect(590,690,420,65),panel);Label(610,698,380,28,"站在破洞内 · 下落至"+(G.Map.OfficeFloor2733-1)+"楼",18,mint,TextAnchor.MiddleCenter);
   Fill(new Rect(610,739,380,4),muted);Fill(new Rect(610,739,380*Mathf.Clamp01(G.OfficeFallTime2748/2),4),mint);
  }
 }
}
