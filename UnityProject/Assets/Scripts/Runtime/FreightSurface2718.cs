using System.Collections.Generic;
using UnityEngine;
namespace GreyHarbor {
 public sealed partial class RaidMap {
  readonly Dictionary<string,Color> freightPigments2718=new Dictionary<string,Color>();
  List<Vector2[]> freightLocalRoads2718;
  Color FreightPigment2718(string hex){Color c;if(!freightPigments2718.TryGetValue(hex,out c)){c=PixelArt.Hex(hex);freightPigments2718.Add(hex,c);}return c;}
  void FreightCoolSurfacePalette2718(){
   foreach(var f in FreightSurfaceFeatures2717){
    switch(f.Kind){case "Mud":f.PaletteHex=f.Name=="WarehouseWetMud"?"616E69":"7D8580";break;
     case "Gravel":f.PaletteHex="969F9D";break;case "Dust":f.PaletteHex="A0A79F";break;
     case "ConcreteRepair":f.PaletteHex="BEC2BF";break;case "AsphaltRepair":f.PaletteHex="777E83";break;
     case "Oil":f.PaletteHex=f.Name=="CrashOilPool"?"3D4647":"59615E";break;}
   }
  }
  // A local one-time CPU bake. There are no extra planes, draw calls or frame updates.
  void WeatherFreightPixels2718(Color32[] pixels,float[] depth,int w,int h,Rect bounds,float scale){
   if(!FreightGroundBuilt2716||IsInterior)return;
   freightLocalRoads2718=new List<Vector2[]>();var vicinity=FreightGroundBounds2716;vicinity.xMin-=5;vicinity.xMax+=5;vicinity.yMin-=5;vicinity.yMax+=5;
   foreach(var road in HarborRoadShapes)if(vicinity.Overlaps(CargoGroundExtent2715(road)))freightLocalRoads2718.Add(road);
   var area=FreightGroundBounds2716;float dx=bounds.width/w,dz=bounds.height/h;
   int x0=Mathf.Clamp(Mathf.FloorToInt((area.xMin-bounds.xMin)/dx),0,w-1),x1=Mathf.Clamp(Mathf.CeilToInt((area.xMax-bounds.xMin)/dx),0,w-1);
   int z0=Mathf.Clamp(Mathf.FloorToInt((area.yMin-bounds.yMin)/dz),0,h-1),z1=Mathf.Clamp(Mathf.CeilToInt((area.yMax-bounds.yMin)/dz),0,h-1);
   for(int z=z0;z<=z1;z++)for(int x=x0;x<=x1;x++){
    int n=z*w+x;if(pixels[n].a==0||depth[n]<.229f||depth[n]>.235f)continue;
    var p=new Vector2(bounds.xMin+(x+.5f)*dx,bounds.yMin+(z+.5f)*dz);if(!area.Contains(p))continue;
    var c=FreightSurfaceColor2717(p);
    // Keep road geometry exactly intact, while restoring asphalt grain over its carrier.
    float road=1-FreightGroundBlend2716(p),grain=FreightNoise2717(p.x,p.y,11.2f,48)-.5f;
    c.r+=grain*.025f*road;c.g+=grain*.028f*road;c.b+=grain*.028f*road;
    pixels[n]=c;
   }
  }
 }
}
