using UnityEngine;
namespace GreyHarbor {
 public static class KeyIcons2760 {
  public static Texture2D Make(ItemKind kind){
   var texture=Resources.Load<Texture2D>("ItemArt/"+kind);
   if(texture==null&&kind==ItemKind.YardHomeKey324){var file=System.IO.Path.Combine(Application.streamingAssetsPath,"KeyArt324",kind+".png");if(System.IO.File.Exists(file)){texture=new Texture2D(2,2,TextureFormat.RGBA32,false);if(!texture.LoadImage(System.IO.File.ReadAllBytes(file)))throw new System.IO.IOException("Invalid painted key sprite: "+kind);texture.filterMode=FilterMode.Point;texture.wrapMode=TextureWrapMode.Clamp;}}
   if(texture==null)throw new System.InvalidOperationException("Missing painted key sprite: "+kind);
   return texture;
  }
 }
}
