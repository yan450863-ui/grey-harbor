using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
namespace GreyHarbor {
 public sealed partial class RaidMap {
  public int FreightWallRendererCount2718 {get;private set;}
  public int FreightFloorTexelCount2718 {get;private set;}
  void FinishFreightWalls2718(){
   var shader=GreyHarbor.FlashlightShaders2763.Load("FreightWall2718");
   if(!shader)throw new System.InvalidOperationException("FreightWall2718 shader is missing");
   var life=Root.gameObject.AddComponent<WorkshopArtResources>();var copies=new Dictionary<Material,Material>();
   float halfWidth=InteriorId=="freightduty"?8.5f:9;
   Vector4 inner=new Vector4(-halfWidth+.43f,-8.07f,halfWidth-.43f,8.07f);
   foreach(var r in Root.GetComponentsInChildren<MeshRenderer>()){
    bool wall=r.name=="FreightWhiteboxWall2716"||r.name=="FreightRoomDoor2716"||r.name=="FreightDoorPull2716"||r.name=="FreightReturnDoor2716"||r.name=="FreightReturnPull2716"||r.GetComponentInParent<FreightDoorFrame2719>();
    foreach(var item in FreightDressingRecords2717)if(item.WallMounted&&item.View&&r.transform.IsChildOf(item.View.transform)){wall=true;break;}
    foreach(var item in FreightLootRecords2717)if(item.WallMounted&&item.Model&&r.transform.IsChildOf(item.Model.transform)){wall=true;break;}
    if(!wall||!r.sharedMaterial)continue;
    var original=r.sharedMaterial;if(!copies.TryGetValue(original,out var material)){
     material=new Material(original){shader=shader,name="FreightWall2718_"+original.name};material.SetVector("_FreightInnerRect2718",inner);
     copies.Add(original,material);life.Materials.Add(material);
    }
    r.sharedMaterial=material;r.shadowCastingMode=ShadowCastingMode.Off;r.receiveShadows=false;FreightWallRendererCount2718++;
   }
  }
  // Invoked once by StableHarborGround137 after rasterisation, only on these two interiors.
  // No runtime material animation or extra surface mesh is introduced.
  public void FinishFreightFloorPixels2718(Color32[] pixels,int width,int height,Rect bounds){
   if(!FreightInterior2716)return;
   for(int z=0;z<height;z++)for(int x=0;x<width;x++){
    int index=z*width+x;if(pixels[index].a==0)continue;
    Vector2 p=new Vector2(bounds.xMin+(x+.5f)*bounds.width/width,bounds.yMin+(z+.5f)*bounds.height/height);
    FreightFloorZone2717 zone=null;foreach(var candidate in FreightFloorZones2717)if(candidate.Area.Contains(p))zone=candidate;
    if(zone==null)continue;pixels[index]=FreightFloorColor2718(zone,p);FreightFloorTexelCount2718++;
   }
  }
  static readonly Dictionary<string,Color> floorPalette2718=new Dictionary<string,Color>();
  static readonly Vector2[] floorCrackA2718={new Vector2(-7,-6.5f),new Vector2(3,-7.1f),new Vector2(5,1.5f),new Vector2(-5.2f,-1.8f)};
  static readonly Vector2[] floorCrackB2718={new Vector2(-4.9f,-5.8f),new Vector2(4.2f,-5.6f),new Vector2(6.8f,2.8f),new Vector2(-6.3f,-.1f)};
  static Color FloorHex2718(string hex){if(!floorPalette2718.TryGetValue(hex,out var c)){c=PixelArt.Hex(hex);floorPalette2718.Add(hex,c);}return c;}
  static float FloorHash2718(int x,int z,int seed){unchecked{uint a=(uint)(x*374761393+z*668265263+seed*144269);a=(a^(a>>13))*1274126177u;return (a^(a>>16))/(float)uint.MaxValue;}}
  static float FloorNoise2718(Vector2 p,float scale,int seed)=>Mathf.PerlinNoise(p.x*scale+seed*.713f+49,p.y*scale+seed*.411f+67);
  static float FloorLineDistance2718(Vector2 p,Vector2 a,Vector2 b){var d=b-a;float t=Mathf.Clamp01(Vector2.Dot(p-a,d)/d.sqrMagnitude);return Vector2.Distance(p,a+d*t);}
  public Color32 FreightFloorColor2718(FreightFloorZone2717 zone,Vector2 p){
   Vector2 q=p-zone.Area.min;float hash=FloorHash2718(Mathf.FloorToInt(p.x*24),Mathf.FloorToInt(p.y*24),2718);
   float grain=FloorNoise2718(p,7,17),wear=FloorNoise2718(p,.74f,19);Color c=zone.Color;
   switch(zone.Material){
    case "SpeckledTerrazzo": {
     c=FloorHex2718("94A499")*(.95f+grain*.10f);
     // Small embedded stone chips, with two pale and one dark aggregate tones.
     if(hash>.90f)c=Color.Lerp(c,FloorHex2718(hash>.97f?"657A71":hash>.945f?"BDC4AE":"839189"),.68f);
     float a=Mathf.Repeat(q.x,1.1f),b=Mathf.Repeat(q.y,1.1f);if(a<.02f||b<.02f)c=Color.Lerp(c,FloorHex2718("677C74"),.48f);
     c=Color.Lerp(c,FloorHex2718("748B7D"),Mathf.Max(0,wear-.62f)*.38f);break;
    }
    case "WarmTimber": {
     int row=Mathf.FloorToInt(q.y/.28f);float across=Mathf.Repeat(q.y,.28f),along=q.x+(row%3)*.79f;
     int plank=Mathf.FloorToInt(along/2.4f);float tint=FloorHash2718(plank,row,31);
     c=Color.Lerp(FloorHex2718("8A7659"),FloorHex2718("B09B73"),tint*.72f);
     float fibers=FloorNoise2718(new Vector2(q.x*.24f,q.y*8),2.8f,29);c*=.91f+fibers*.14f+hash*.035f;
     if(across<.018f||Mathf.Repeat(along,2.4f)<.022f)c=Color.Lerp(c,FloorHex2718("655E4C"),.60f);
     if(Mathf.Abs(fibers-.46f)<.018f)c*=.86f;
     c=Color.Lerp(c,FloorHex2718("BDAD87"),Mathf.Max(0,wear-.53f)*.37f);break;
    }
    case "PorcelainTile": {
     const float step=.52f;float a=Mathf.Repeat(q.x,step),b=Mathf.Repeat(q.y,step);int tx=Mathf.FloorToInt(q.x/step),tz=Mathf.FloorToInt(q.y/step);
     c=Color.Lerp(FloorHex2718("ADBAB2"),FloorHex2718("C1C7B7"),FloorHash2718(tx,tz,41)*.55f);c*=.973f+hash*.046f;
     if(a<.024f||b<.024f)c=FloorHex2718("738A81")*(.97f+grain*.06f);
     float damp=Mathf.Max(0,.48f-wear)*.55f;c=Color.Lerp(c,FloorHex2718("748F83"),damp);
     if(FloorHash2718(tx,tz,42)>.96f&&Mathf.Abs(a-(.12f+b*.44f))<.015f)c=Color.Lerp(c,FloorHex2718("647D75"),.38f);break;
    }
    case "OfficeVinyl": {
     const float step=.65f;float a=Mathf.Repeat(q.x,step),b=Mathf.Repeat(q.y,step);int tx=Mathf.FloorToInt(q.x/step),tz=Mathf.FloorToInt(q.y/step);
     c=Color.Lerp(FloorHex2718("929789"),FloorHex2718("A7AC98"),FloorHash2718(tx,tz,51)*.65f);c*=.955f+grain*.06f+hash*.04f;
     if(a<.016f||b<.016f)c=Color.Lerp(c,FloorHex2718("667A70"),.47f);
     float scuff=FloorNoise2718(new Vector2(p.x*.31f,p.y*2.8f),1.1f,53);c=Color.Lerp(c,FloorHex2718("657B73"),Mathf.Max(0,scuff-.70f)*.78f);
     c=Color.Lerp(c,FloorHex2718("B4B5A0"),Mathf.Max(0,wear-.57f)*.38f);break;
    }
    default: {
     // Concrete stays cool grey-green; fine aggregate and irregular worn patches
     // are continuous across the hall, rather than repeated square dirt stamps.
     c=FloorHex2718(zone.Material=="WornConcrete"?"8E9B95":"8D9B8C")*(.93f+grain*.11f+hash*.038f);
     c=Color.Lerp(c,FloorHex2718("657C76"),Mathf.Max(0,.50f-wear)*.50f);
     if(hash>.965f)c=Color.Lerp(c,FloorHex2718("BEC2B2"),.28f);

     for(int i=0;i<floorCrackA2718.Length;i++)if(FloorLineDistance2718(p,floorCrackA2718[i],floorCrackB2718[i])<.027f+hash*.014f)c=Color.Lerp(c,FloorHex2718("536B66"),.54f);
     if(zone.Material=="WornConcrete"){
      float axle=Mathf.Min(Mathf.Abs(p.x-.72f),Mathf.Abs(p.x+.72f));if(axle<.065f&&Mathf.Repeat(p.y+6,2.1f)<1.25f)c=Color.Lerp(c,FloorHex2718("677972"),.28f);
      bool vertical=Mathf.Abs(Mathf.Abs(p.x)-1.24f)<.028f&&p.y>-7.9f&&p.y<7.9f&&Mathf.Repeat(p.y+7.9f,.76f)<.43f;
      bool horizontal=Mathf.Abs(Mathf.Abs(p.y)-1.24f)<.028f&&p.x>-8&&p.x<8&&Mathf.Repeat(p.x+8,.76f)<.43f;
      if(vertical||horizontal)c=Color.Lerp(FloorHex2718("AFA064"),c,hash*.19f);
     }break;
    }
   }
   c.a=1;return c;
  }
 }
}

