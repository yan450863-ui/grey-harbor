using UnityEngine;
namespace GreyHarbor {
 public static class SoldierArt {
  public static bool Loaded {get;private set;}
  public static Sprite[] Load(){
   var texture=Resources.Load<Texture2D>("CharacterArt/SoldierShuffle");
   if(texture==null){Debug.LogError("Missing SoldierShuffle character art");return null;}
   var pixels=texture.GetPixels32();var rects=new Rect[2];float maxHeight=0;
   for(int cell=0;cell<2;cell++){
    int x0=texture.width,y0=texture.height,x1=-1,y1=-1;
    for(int y=0;y<texture.height;y++)for(int x=cell*texture.width/2;x<(cell+1)*texture.width/2;x++)
     if(pixels[y*texture.width+x].a>32){x0=Mathf.Min(x0,x);y0=Mathf.Min(y0,y);x1=Mathf.Max(x1,x);y1=Mathf.Max(y1,y);}
    if(x1<0){Debug.LogError("Empty soldier animation cell");return null;}
    rects[cell]=new Rect(x0,y0,x1-x0+1,y1-y0+1);maxHeight=Mathf.Max(maxHeight,rects[cell].height);
   }
   var first=Sprite.Create(texture,rects[0],new Vector2(.5f,0),maxHeight/1.42f,0,SpriteMeshType.FullRect);
   var second=Sprite.Create(texture,rects[1],new Vector2(.5f,0),maxHeight/1.42f,0,SpriteMeshType.FullRect);
   first.name="SoldierStanding";second.name="SoldierCompressed";Loaded=true;
   return new[]{first,second,first,second};
  }
 }
}
