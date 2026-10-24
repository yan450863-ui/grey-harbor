using UnityEngine;
namespace GreyHarbor {public sealed partial class RaidUI {
 Transform pictureRoot333;int pictureRevision333=-1;Color32 pictureOutside333;
 internal int PictureBuilds333 {get;private set;}
 void EnsureMapPicture333(RaidMap map){
  if(picturePixels332!=null&&pictureRoot333==map.Root&&pictureRevision333==map.LayoutRevision&&pictureBounds332==map.Bounds)return;
  // Build from map geometry on the CPU once. Immediate-mode GPU drawing from
  // LateUpdate can capture only the clear color in the normal player render loop.
  // Keeping the source independent of the camera also survives M and scene changes.
  UseTerrainSource327(map,true);pictureBounds332=map.Bounds;pictureOutside333=nearbySource327.Outside;
  if(!map.HarborWhitebox){pictureWidth332=terrainWidth325;pictureHeight332=terrainHeight325;picturePixels332=terrainSource325;}
  else {
   pictureWidth332=Mathf.Clamp(Mathf.CeilToInt(map.Bounds.width*20),64,4096);pictureHeight332=Mathf.Clamp(Mathf.CeilToInt(map.Bounds.height*20),64,4096);picturePixels332=new Color32[pictureWidth332*pictureHeight332];
   float dx=(float)terrainWidth325/pictureWidth332,dy=(float)terrainHeight325/pictureHeight332;
   for(int y=0;y<pictureHeight332;y++){float sy=(y+.5f)*dy-.5f;int iy=Mathf.FloorToInt(sy),fy=(int)((sy-iy)*256);int y0=Mathf.Clamp(iy,0,terrainHeight325-1)*terrainWidth325,y1=Mathf.Clamp(iy+1,0,terrainHeight325-1)*terrainWidth325;
    for(int x=0;x<pictureWidth332;x++){float sx=(x+.5f)*dx-.5f;int ix=Mathf.FloorToInt(sx),fx=(int)((sx-ix)*256);int x0=Mathf.Clamp(ix,0,terrainWidth325-1),x1=Mathf.Clamp(ix+1,0,terrainWidth325-1);picturePixels332[y*pictureWidth332+x]=PictureBlend332(terrainSource325[y0+x0],terrainSource325[y0+x1],terrainSource325[y1+x0],terrainSource325[y1+x1],fx,fy);}
   }
   float scaleX=pictureWidth332/map.Bounds.width,scaleY=pictureHeight332/map.Bounds.height;
   foreach(var shape in nearbySource327.Shapes){var polygon=shape.Projected;for(int i=0;i<polygon.Length;i++)polygon[i]=new Vector2((shape.Points[i].x-map.Bounds.xMin)*scaleX,(shape.Points[i].z-map.Bounds.yMin)*scaleY);PaintTerrainShape327(picturePixels332,pictureWidth332,pictureHeight332,polygon,PixelArt.Hex(shape.Low?"7F8F86":"D6DBD6"));}
  }
  pictureRoot333=map.Root;pictureRevision333=map.LayoutRevision;pictureBuild332=++PictureBuilds333;
 }
}}
