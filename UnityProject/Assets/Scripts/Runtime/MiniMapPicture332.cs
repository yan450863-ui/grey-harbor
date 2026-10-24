using UnityEngine;
namespace GreyHarbor {public sealed partial class RaidUI {
 Color32[] picturePixels332,cropPixels332;Texture2D pictureCrop332;int pictureWidth332,pictureHeight332,pictureBuild332=-1;Rect pictureBounds332;Vector3 cropOrigin332,cropStepX332,cropStepY332;int cropBuild332=-1;
 internal int PictureUploads332 {get;private set;}
 internal Texture2D TerrainPicture332(RaidMap map,MiniMapProjection projection,float density){
  EnsureMapPicture333(map);
  var size=TerrainOutputSize327(projection.Viewport,density);int w=size.x,h=size.y;bool resize=!pictureCrop332||pictureCrop332.width!=w||pictureCrop332.height!=h;
  Vector3 origin=projection.WorldPoint(projection.Viewport.min),sx=projection.WorldPoint(projection.Viewport.min+new Vector2(projection.Viewport.width/w,0))-origin,sy=projection.WorldPoint(projection.Viewport.min+new Vector2(0,projection.Viewport.height/h))-origin;
  if(!resize&&cropBuild332==pictureBuild332&&(origin-cropOrigin332).sqrMagnitude<.000001f&&(sx-cropStepX332).sqrMagnitude<.0000000001f&&(sy-cropStepY332).sqrMagnitude<.0000000001f)return pictureCrop332;
  if(resize){if(pictureCrop332)Destroy(pictureCrop332);pictureCrop332=new Texture2D(w,h,TextureFormat.RGBA32,false){name="NearbyPicture332",filterMode=FilterMode.Bilinear,wrapMode=TextureWrapMode.Clamp};cropPixels332=new Color32[w*h];}
  float scaleX=pictureWidth332/pictureBounds332.width,scaleY=pictureHeight332/pictureBounds332.height;
  float dx=sx.x*scaleX,dy=sx.z*scaleY,rx=sy.x*scaleX,ry=sy.z*scaleY;
  float startX=(origin.x-pictureBounds332.xMin)*scaleX-.5f+dx*.5f+rx*(h-.5f),startY=(origin.z-pictureBounds332.yMin)*scaleY-.5f+dy*.5f+ry*(h-.5f);
  Color32 outside=pictureOutside333;
  for(int row=0;row<h;row++){float x=startX-rx*row,y=startY-ry*row;int index=row*w;for(int col=0;col<w;col++,x+=dx,y+=dy){int ix=(int)x,iy=(int)y;if(x<ix)ix--;if(y<iy)iy--;int fx=(int)((x-ix)*256),fy=(int)((y-iy)*256);
   if(ix>=0&&iy>=0&&ix+1<pictureWidth332&&iy+1<pictureHeight332){int at=iy*pictureWidth332+ix;cropPixels332[index+col]=PictureBlend332(picturePixels332[at],picturePixels332[at+1],picturePixels332[at+pictureWidth332],picturePixels332[at+pictureWidth332+1],fx,fy);}
   else cropPixels332[index+col]=PictureBlend332(PicturePixel332(ix,iy,outside),PicturePixel332(ix+1,iy,outside),PicturePixel332(ix,iy+1,outside),PicturePixel332(ix+1,iy+1,outside),fx,fy);
  }}
  pictureCrop332.SetPixels32(cropPixels332);pictureCrop332.Apply(false,false);cropOrigin332=origin;cropStepX332=sx;cropStepY332=sy;cropBuild332=pictureBuild332;PictureUploads332++;return pictureCrop332;
 }
 Color32 PicturePixel332(int x,int y,Color32 outside)=>x>=0&&y>=0&&x<pictureWidth332&&y<pictureHeight332?picturePixels332[y*pictureWidth332+x]:outside;
 static Color32 PictureBlend332(Color32 a,Color32 b,Color32 c,Color32 d,int fx,int fy){int lx=256-fx,ly=256-fy;return new Color32((byte)(((a.r*lx+b.r*fx)*ly+(c.r*lx+d.r*fx)*fy)>>16),(byte)(((a.g*lx+b.g*fx)*ly+(c.g*lx+d.g*fx)*fy)>>16),(byte)(((a.b*lx+b.b*fx)*ly+(c.b*lx+d.b*fx)*fy)>>16),255);}
 void ReleaseMapPicture332(){if(pictureCrop332)Destroy(pictureCrop332);picturePixels332=cropPixels332=null;pictureBuild332=cropBuild332=-1;pictureRoot333=null;pictureRevision333=-1;}
}}
