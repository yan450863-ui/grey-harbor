using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using UnityEngine;
namespace GreyHarbor {public sealed partial class RaidVerification {
 string PixelHash305(Texture2D texture){using(var sha=SHA256.Create())return Convert.ToBase64String(sha.ComputeHash(texture.GetRawTextureData()));}
 void LoadedArtBoard305(List<Texture2D> textures,List<Rect> bounds){
  const int cols=5,tile=120,rows=4;var board=new Texture2D(cols*tile,rows*tile,TextureFormat.RGBA32,false);var pixels=new Color32[board.width*board.height];
  for(int i=0;i<pixels.Length;i++)pixels[i]=new Color32(18,27,29,255);
  for(int i=0;i<textures.Count;i++){
   var texture=textures[i];var uv=bounds[i];float aspect=texture.width*uv.width/(texture.height*uv.height);float w=Mathf.Min(72,72*aspect),h=w/aspect;int cx=i%cols*tile+tile/2,cy=board.height-(i/cols*tile+tile/2);
   for(int y=cy-45;y<cy+45;y++)for(int x=cx-45;x<cx+45;x++)pixels[y*board.width+x]=new Color32(57,80,87,255);
   for(int y=(int)(cy-h/2);y<cy+h/2;y++)for(int x=(int)(cx-w/2);x<cx+w/2;x++){
    float u=uv.x+(x+.5f-(cx-w/2))/w*uv.width,v=uv.y+(y+.5f-(cy-h/2))/h*uv.height;var c=texture.GetPixelBilinear(u,v);var b=(Color)pixels[y*board.width+x];pixels[y*board.width+x]=Color.Lerp(b,c,c.a);
   }
  }
  board.SetPixels32(pixels);board.Apply();File.WriteAllBytes(Path.Combine(folder,"3.0.5-actual-loaded-icons-72px.png"),board.EncodeToPNG());Destroy(board);
 }
 IEnumerator RunScopeArt305(){
  Check(G.SavePath.EndsWith("verification-stash-3.0.14.json"),"isolated 3.0.5 verification save");StandardRaid();G.enabled=false;G.InventoryOpen=false;G.Paused=false;foreach(var e in G.Enemies){e.Dead=true;e.HitCollider.enabled=false;}
  G.Equipped=new ItemStack(ItemKind.AR16,1);G.WeaponChanged279();G.Controls=new TouchInput{Aiming30=true};G.TickScope30(1);Check(!G.ScopeReady30&&!G.ScopeVisible305,"bare gun still has no aiming circle");
  int cameras=Resources.FindObjectsOfTypeAll<Camera>().Length;int targets=Resources.FindObjectsOfTypeAll<RenderTexture>().Length;
  var originalPosition=G.View.transform.position;var originalRotation=G.View.transform.rotation;float originalZoom=G.View.orthographicSize;var originalProjection=G.View.projectionMatrix;
  foreach(var optic in new[]{ItemKind.BlueOptic30}){
   G.Equipped.Attachments30.Clear();G.Equipped.Attachments30.Add(new ItemStack(optic,1));G.ResetScope30();G.TickScope30(0);Check(G.ScopeVisible305,"optic activates immediately "+optic);G.TickScope30(.02f);G.RenderScope30();Check(G.ScopeVisible305,"circle needs optic and held aim "+optic);
   Check(G.View.transform.position==originalPosition&&G.View.transform.rotation==originalRotation&&G.View.orthographicSize==originalZoom&&G.View.projectionMatrix==originalProjection,"aim never transforms or rescales original scene "+optic);
   Check(Resources.FindObjectsOfTypeAll<Camera>().Length==cameras&&Resources.FindObjectsOfTypeAll<RenderTexture>().Length==targets,"no extra camera or scene texture allocated "+optic);
   G.Paused=true;G.TickScope30(.1f);Check(!G.ScopeVisible305,"pause clears circle "+optic);G.Paused=false;G.TickScope30(1);G.InventoryOpen=true;G.TickScope30(.1f);Check(!G.ScopeVisible305,"inventory clears circle "+optic);G.InventoryOpen=false;G.TickScope30(1);G.ReloadTime=1;G.TickScope30(.1f);Check(!G.ScopeVisible305,"reload clears circle "+optic);G.ReloadTime=0;G.TickScope30(1);G.Controls=new TouchInput();G.TickScope30(.1f);Check(!G.ScopeVisible305,"right button release restores hip-fire crosshair "+optic);G.Controls=new TouchInput{Aiming30=true};
  }
  var mask=ScopePresentation305.CornerMask;Check(mask.GetPixel(128,128).a==0&&mask.GetPixel(0,0).a>.6f,"mask has transparent center and dark outer corners");
  foreach(var size in new[]{new Vector2Int(1600,900),new Vector2Int(2560,1410),new Vector2Int(1280,1024),new Vector2Int(2560,1080)})foreach(bool blue in new[]{false,true})foreach(var cursor in new[]{new Vector2(size.x*.5f,size.y*.5f),Vector2.zero,new Vector2(size.x,size.y),new Vector2(0,size.y),new Vector2(size.x,0),new Vector2(size.x*.7f,size.y+30)}){
   var frame=ScopePresentation305.Frame(size.x,size.y,cursor,blue);bool valid=Vector2.Distance(frame.center,new Vector2(cursor.x,size.y-cursor.y))<.001f&&Mathf.Abs(frame.width-(blue?120:100)*ScopePresentation305.Scale(size.x,size.y))<.001f;
   var regions=ScopePresentation305.OuterRegions(size.x,size.y,frame);
   for(int y=0;y<9;y++)for(int x=0;x<17;x++){
    var p=new Vector2((x+.5f)*size.x/17,(y+.5f)*size.y/9);int coverage=0;foreach(var r in regions)if(r.Contains(p))coverage++;bool inFrame=frame.Contains(p);valid&=coverage+(inFrame?1:0)==1;
    if(inFrame){float a=mask.GetPixelBilinear((p.x-frame.x)/frame.width,1-(p.y-frame.y)/frame.height).a;float distance=(p-frame.center).magnitude/frame.width*2;if(distance<.95f)valid&=a<.001f;else if(distance>1.04f)valid&=Mathf.Abs(a-ScopePresentation305.DimAlpha)<.005f;}
   }
   Check(valid,"original pixels preserved inside circle; outside coverage has no gaps/overlap "+size+" blue="+blue+" aim="+cursor);
  }
  G.TickScope30(1);foreach(float yaw in new[]{45,135,225,315}){
   G.Rig.ResetView(yaw);G.Rig.Tick(0);var world=G.Player.transform.position+G.Rig.Right*4+G.Rig.Forward*2+Vector3.up*RaidGame.CombatHeight;var screen=G.View.WorldToScreenPoint(world);G.Controls=new TouchInput{Aiming30=true,AimPosition=new Vector2(screen.x,screen.y)};G.AkHandling276.Reset();G.UpdateAim();var before=G.AkCrosshairScreen276;G.AkHandling276.Recoil=5;G.UpdateAim();var after=G.AkCrosshairScreen276;var frame=ScopePresentation305.Frame(Screen.width,Screen.height,after,true);Check(Vector2.Distance(before,after)>1&&Vector2.Distance(frame.center,new Vector2(after.x,Screen.height-after.y))<.001f,"one circle reticle stays at actual recoil point in original view yaw="+yaw);
  }
  var kindList=new List<ItemKind>{ItemKind.Green556,ItemKind.Blue556,ItemKind.Green12Gauge,ItemKind.Blue12Gauge};foreach(var mod in ModRules30.All)kindList.Add(mod.Kind);
  var textures=new List<Texture2D>();var crop=new List<Rect>();var names=new List<string>();foreach(var k in new[]{ItemKind.Ammo,ItemKind.GunParts,ItemKind.Green9mm}){textures.Add(ItemIcons.Get(k));crop.Add(ItemIcons.Bounds(k));names.Add("reference "+k);}
  foreach(var kind in kindList){
   string path=FormalWeaponArt305.FilePath(kind);Check(File.Exists(path),"formal PNG ships in running player "+kind);
   var actual=ItemIcons.Get(kind);var expected=new Texture2D(2,2,TextureFormat.RGBA32,false);Check(expected.LoadImage(File.ReadAllBytes(path),false),"formal PNG decodes "+kind);
   Check(actual.width>=512&&actual.height>=512&&actual.name=="FormalWeaponArt305/"+FormalWeaponArt305.AssetName(kind)&&PixelHash305(actual)==PixelHash305(expected),"actual inventory/market loader uses formal PNG pixels "+kind);Destroy(expected);
   var uv=ItemIcons.Bounds(kind);Check(uv.xMin>0&&uv.yMin>0&&uv.xMax<1&&uv.yMax<1&&uv.width>.25f&&uv.height>.25f,"transparent icon has useful bounds and unclipped margins "+kind);
   Check(object.ReferenceEquals(actual,BlueWeaponsArt30.Texture(kind)),"UI entry and accessory loader share actual art "+kind);
   textures.Add(actual);crop.Add(uv);names.Add(kind.ToString());yield return null;
  }
  Check(object.ReferenceEquals(ItemIcons.Get(ItemKind.Green556),ItemIcons.Get(ItemKind.Blue556))&&object.ReferenceEquals(ItemIcons.Get(ItemKind.Green12Gauge),ItemIcons.Get(ItemKind.Blue12Gauge)),"ammo grades share physical silhouette, quality remains on UI cards");
  Check(kindList.Count==17&&RaidInventory.KindCount==291,"all existing 4 ammo and13 mod definitions keep their IDs");LoadedArtBoard305(textures,crop);File.WriteAllLines(Path.Combine(folder,"3.0.5-actual-loaded-icons-order.txt"),names);
  G.Controls=new TouchInput();G.ResetScope30();G.Finish(false);results.Add("INFO hidden-window IMGUI visual verification remains manual; loaded texture board is an icon comparison, not a full GUI screenshot");results.Add("SCOPE AND ART 3.0.5 COMPLETE");
 }
}}
