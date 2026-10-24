using System.Collections.Generic;
using UnityEngine;
namespace GreyHarbor {
 public sealed partial class RaidMap {
  public const int OutdoorGroundRevision2749=2749;
  readonly List<Vector2[]> outdoorAccents2749=new List<Vector2[]>();
  void RegisterOutdoorAccent2749(List<Vector3> vertices,List<int> indices,Color color){
   if(!HarborWhitebox||IsInterior||vertices.Count<3)return;
   bool yellow=color.r>.60f&&color.g>.52f&&color.b<color.r*.82f;
   bool white=color.r>.72f&&color.g>.72f&&color.b>.65f&&Mathf.Abs(color.r-color.b)<.13f;
   if(!yellow&&!white)return;
   float xmin=float.MaxValue,xmax=float.MinValue,zmin=float.MaxValue,zmax=float.MinValue;
   foreach(var p in vertices){xmin=Mathf.Min(xmin,p.x);xmax=Mathf.Max(xmax,p.x);zmin=Mathf.Min(zmin,p.z);zmax=Mathf.Max(zmax,p.z);}
   float narrow=Mathf.Min(xmax-xmin,zmax-zmin),longSide=Mathf.Max(xmax-xmin,zmax-zmin);
   if(narrow>.65f||longSide<.35f||longSide/Mathf.Max(narrow,.001f)<4)return;
   for(int i=0;i<indices.Count;i+=3){var a=vertices[indices[i]];var b=vertices[indices[i+1]];var c=vertices[indices[i+2]];outdoorAccents2749.Add(new[]{new Vector2(a.x,a.z),new Vector2(b.x,b.z),new Vector2(c.x,c.z)});}
  }
  void OutdoorPreserveMask2749(byte[] mask,Vector2[] polygon,int width,int height,Rect bounds){
   var area=CargoGroundExtent2715(polygon);float dx=bounds.width/width,dz=bounds.height/height;
   int x0=Mathf.Clamp(Mathf.FloorToInt((area.xMin-bounds.xMin)/dx),0,width-1),x1=Mathf.Clamp(Mathf.CeilToInt((area.xMax-bounds.xMin)/dx),0,width-1);
   int z0=Mathf.Clamp(Mathf.FloorToInt((area.yMin-bounds.yMin)/dz),0,height-1),z1=Mathf.Clamp(Mathf.CeilToInt((area.yMax-bounds.yMin)/dz),0,height-1);
   for(int z=z0;z<=z1;z++)for(int x=x0;x<=x1;x++){int n=z*width+x;if(mask[n]==0&&InPolygon(new Vector2(bounds.xMin+(x+.5f)*dx,bounds.yMin+(z+.5f)*dz),polygon))mask[n]=255;}
  }
  public float OutdoorRoadWeight2749(Vector2 p){
   float weight=0;
   var polygons=OutdoorRoadPolygons2749();for(int i=0;i<polygons.Count;i++)if(outdoorRoadBounds2750[i].Contains(p))weight=Mathf.Max(weight,OutdoorPolygonWeight2749(p,polygons[i]));
   if(FreightGroundBuilt2716&&p.x>=-77&&p.x<=31&&p.y>=54.5f&&p.y<=86)weight=Mathf.Max(weight,FreightAsphaltWeight2719(p));
   return weight;
  }
  List<Vector2[]> outdoorRoadCache2750;
  readonly List<Rect> outdoorRoadBounds2750=new List<Rect>();
  List<Vector2[]> OutdoorRoadPolygons2749(){
   if(outdoorRoadCache2750!=null)return outdoorRoadCache2750;
   var polygons=new List<Vector2[]>(HarborRoadShapes);
   if(Root)foreach(var registry in Root.GetComponentsInChildren<GroundSurfaceRegions>())foreach(var region in registry.Regions)if(region.Finish==GroundFinish.Asphalt)polygons.Add(CargoGroundCorners2715(region.Area));
   foreach(var polygon in polygons){var area=CargoGroundExtent2715(polygon);outdoorRoadBounds2750.Add(Rect.MinMaxRect(area.xMin-.22f,area.yMin-.22f,area.xMax+.22f,area.yMax+.22f));}
   outdoorRoadCache2750=polygons;return outdoorRoadCache2750;
  }
  static float OutdoorPolygonWeight2749(Vector2 p,Vector2[] polygon){
   if(InPolygon(p,polygon))return 1;
   float distance=float.MaxValue;
   for(int i=0;i<polygon.Length;i++)distance=Mathf.Min(distance,CargoPointSegment2715(p,polygon[i],polygon[(i+1)%polygon.Length]));
   return 1-Mathf.SmoothStep(0,1,Mathf.Clamp01(distance/.22f));
  }
  void ApplyOutdoorGroundStyle2749(Color32[] pixels,float[] depth,int width,int height,Rect bounds){
   if(!HarborWhitebox||IsInterior)return;
   float dx=bounds.width/width,dz=bounds.height/height;
   var roads=new byte[pixels.Length];var polygons=OutdoorRoadPolygons2749();
   var preserve=new byte[pixels.Length];
   foreach(var registry in Root.GetComponentsInChildren<GroundSurfaceRegions>())foreach(var region in registry.Regions)if(region.Finish==GroundFinish.Steel)OutdoorPreserveMask2749(preserve,CargoGroundCorners2715(region.Area),width,height,bounds);
   foreach(string name in new[]{"Dock_FishingBoatDeck141","Dock_FishingGangway141","Dock_WorkBargeDeck141","Dock_WorkGangway141","Dock_PassengerFerryDeck143","Dock_PassengerGangway143"}){
    var platform=Root.Find(name);if(!platform)continue;var filter=platform.GetComponent<MeshFilter>();if(!filter||!filter.sharedMesh)continue;
    var rim=new List<Vector2>();foreach(var point in filter.sharedMesh.vertices){var p=platform.TransformPoint(point);rim.Add(new Vector2(p.x,p.z));}OutdoorPreserveMask2749(preserve,rim.ToArray(),width,height,bounds);
   }
   foreach(var accent in outdoorAccents2749)OutdoorPreserveMask2749(preserve,accent,width,height,bounds);
   // Rasterise only each lane's small bounding rectangle. This work runs offline;
   // the exported cache still supplies one filtered texture and one floor plane.
   foreach(var polygon in polygons){
    var area=CargoGroundExtent2715(polygon);
    int x0=Mathf.Clamp(Mathf.FloorToInt((area.xMin-.22f-bounds.xMin)/dx),0,width-1),x1=Mathf.Clamp(Mathf.CeilToInt((area.xMax+.22f-bounds.xMin)/dx),0,width-1);
    int z0=Mathf.Clamp(Mathf.FloorToInt((area.yMin-.22f-bounds.yMin)/dz),0,height-1),z1=Mathf.Clamp(Mathf.CeilToInt((area.yMax+.22f-bounds.yMin)/dz),0,height-1);
    for(int z=z0;z<=z1;z++)for(int x=x0;x<=x1;x++){
     int n=z*width+x;if(pixels[n].a==0||roads[n]==255)continue;
     float weight=OutdoorPolygonWeight2749(new Vector2(bounds.xMin+(x+.5f)*dx,bounds.yMin+(z+.5f)*dz),polygon);
     roads[n]=(byte)Mathf.Max(roads[n],Mathf.RoundToInt(weight*255));
    }
   }
   var types=new byte[pixels.Length];var previous=new byte[pixels.Length];var edgeBlend=new byte[pixels.Length];RasterOutdoorTypes2750(types,previous,edgeBlend,width,height,bounds);
   for(int z=0;z<height;z++)for(int x=0;x<width;x++){
    int n=z*width+x;if(pixels[n].a==0||preserve[n]!=0||depth[n]>=.40f)continue;
    float px=bounds.xMin+(x+.5f)*dx,pz=bounds.yMin+(z+.5f)*dz;Color source=pixels[n];
    float road=roads[n]/255f;
    if(FreightGroundBuilt2716&&px>=-77&&px<=31&&pz>=54.5f&&pz<=86)road=Mathf.Max(road,FreightAsphaltWeight2719(new Vector2(px,pz)));
    var type=(GroundFinish)types[n];
    // Preserve purpose and authored wear. Concrete is not substituted for earth,
    // gravel, patched shop aprons, or any non-cement material in the old library.
    if(type!=GroundFinish.Soil&&type!=GroundFinish.Gravel&&type!=GroundFinish.Concrete&&type!=GroundFinish.PatchedConcrete&&type!=GroundFinish.Asphalt)continue;
    Color cement=GroundMaterialStates2750.Sample(type,px,pz);
    if(edgeBlend[n]<255)cement=Color.Lerp(GroundMaterialStates2750.Sample((GroundFinish)previous[n],px,pz),cement,edgeBlend[n]/255f);
    Color c=Color.Lerp(cement,GroundMaterialStates2750.Sample(GroundFinish.Asphalt,px,pz),road);
    float oldTone=type==GroundFinish.Soil||type==GroundFinish.Gravel?0:Mathf.Clamp((source.r+source.g+source.b)/3-.55f,-.14f,.14f)*.28f;
    c.r+=oldTone;c.g+=oldTone;c.b+=oldTone*.94f;
    c.a=source.a;pixels[n]=c;
   }
  }
  static float OutdoorHash2749(int x,int z,int seed){unchecked{uint h=(uint)(x*374761393+z*668265263+seed*1442695041);h=(h^(h>>13))*1274126177;h^=h>>16;return (h&65535)/65535f;}}
 }
}
