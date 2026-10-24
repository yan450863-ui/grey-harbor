using System;
using UnityEngine;
namespace GreyHarbor {
 // Shared original-color assets for showcase and the three integrated 2.7.57 monster species.
 public static class AssetMonsterArt2717 {
  public static readonly string[] Ids={"PaperbackCreeper","BallastHauler","Pipewhisker"};
  static readonly string[] names={"纸背伏行体","负索驮背体","管须裂节体"};
  static readonly float[] heights={.95f,1.85f,.80f};
  static readonly float[] ppu={141.05263158f,101.62162162f,108.75f};
  static readonly Sprite[,] sprites=new Sprite[3,2];
  static int Index(string id){int i=Array.IndexOf(Ids,id);if(i<0)throw new ArgumentException("Unknown showcase monster id: "+id);return i;}
  public static string DisplayName(string id)=>names[Index(id)];
  // Idle opaque height, not padded texture height. Windup can extend naturally.
  public static float ArtHeight(string id)=>heights[Index(id)];
  public static float PixelsPerUnit(string id)=>ppu[Index(id)];
  public static string ResourcePath(string id,bool windup){Index(id);return "AssetShowcase2717/Monsters/"+id+(windup?"_Windup":"_Idle");}
  public static Sprite Get(string id,bool windup=false){int i=Index(id),frame=windup?1:0;if(!sprites[i,frame]){
   var texture=Resources.Load<Texture2D>(ResourcePath(id,windup));if(!texture)throw new InvalidOperationException("Missing showcase monster texture: "+ResourcePath(id,windup));
   texture.filterMode=FilterMode.Point;texture.wrapMode=TextureWrapMode.Clamp;
   sprites[i,frame]=Sprite.Create(texture,new Rect(0,0,texture.width,texture.height),new Vector2(.5f,9f/256),ppu[i],0,SpriteMeshType.FullRect);sprites[i,frame].name=id+(windup?"_Windup":"_Idle");
  }return sprites[i,frame];}
  // Keep generated RGB. The showcase must not apply the NPC blue/grey tint.
  public static void Apply(SpriteRenderer renderer,string id,bool windup=false){if(!renderer)throw new ArgumentNullException("renderer");renderer.sprite=Get(id,windup);renderer.color=Color.white;}
 }
}
