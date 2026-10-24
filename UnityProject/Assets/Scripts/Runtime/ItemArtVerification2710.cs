using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
namespace GreyHarbor {
 public sealed partial class RaidVerification {
  IEnumerator VerifyItemArt2710(){
   Check(Application.version=="2.7.10"&&Path.GetFileName(G.SavePath)=="verification-stash-2.7.10.json","2710 Current build and isolated inventory");
   string dir=Path.GetDirectoryName(G.SavePath);string[] formal={"stash-v40.json","stash-v41.json","stash-v42.json"};byte[][] before=Array.ConvertAll(formal,n=>File.Exists(Path.Combine(dir,n))?File.ReadAllBytes(Path.Combine(dir,n)):null);
   var kinds=new[]{ItemKind.AK,ItemKind.MP5,ItemKind.SKS,ItemKind.GreenAmmo,ItemKind.Ammo,ItemKind.Green9mm,ItemKind.Blue9mm};
   var paths=new[]{"AK","MP5","SKS","Ammo","Ammo","NineMM","NineMM"};var rows=new List<string>{"kind,resource,width,height,uvX,uvY,uvWidth,uvHeight,grade,sell,buy"};
   foreach(var kind in kinds){int i=Array.IndexOf(kinds,kind);var asset=Resources.Load<Texture2D>("ItemArt/"+paths[i]);var icon=ItemIcons.Get(kind);
    Check(asset!=null&&asset.width>=512&&asset.height>=(i<3?128:512),"2710 High-detail image resource exists "+kind);
    Check(icon==asset,"2710 Inventory/market icon uses approved raster texture "+kind);
    int clear=0,solid=0;foreach(var p in asset.GetPixels32()){if(p.a==0)clear++;if(p.a>16)solid++;}
    Check(clear>100&&solid>10000,"2710 Real transparency and substantive artwork "+kind);
    var uv=ItemIcons.Bounds(kind);float aspect=asset.width*uv.width/(asset.height*uv.height);
    Check(uv.width>0&&uv.height>0&&uv.xMin>=0&&uv.yMin>=0&&uv.xMax<=1.001f&&uv.yMax<=1.001f,"2710 UI alpha cropping is valid "+kind);
    Check(i<3?aspect>2:aspect>.3f&&aspect<2,"2710 ItemIcon fitting preserves gun/ammunition silhouette "+kind);
    rows.Add(kind+","+paths[i]+","+asset.width+","+asset.height+","+uv.x+","+uv.y+","+uv.width+","+uv.height+","+RaidInventory.Grade(kind)+","+RaidInventory.Value(kind)+","+MarketRules.UnitPrice(kind));
   }
   Check(ItemIcons.Get(ItemKind.Green9mm)==ItemIcons.Get(ItemKind.Blue9mm)&&RaidInventory.Grade(ItemKind.Green9mm)==0&&RaidInventory.Grade(ItemKind.Blue9mm)==1&&RaidInventory.Tint(ItemKind.Green9mm)!=RaidInventory.Tint(ItemKind.Blue9mm),"2710 Shared9mm brass artwork uses distinct green/blue UI quality colors");
   Check(ItemIcons.Get(ItemKind.Green9mm)!=ItemIcons.Get(ItemKind.Ammo),"2710 Short pistol cartridges retain distinct7.62 counterpart");
   Check(ItemIcons.Get(ItemKind.MP5)!=PixelArt.WeaponSprite279(ItemKind.MP5).texture&&ItemIcons.Get(ItemKind.SKS)!=PixelArt.WeaponSprite279(ItemKind.SKS).texture,"2710 New detailed inventory art does not fall back to block sprites");
   Check(RaidInventory.Width(ItemKind.MP5)==2&&RaidInventory.Height(ItemKind.MP5)==2&&RaidInventory.Width(ItemKind.SKS)==3&&RaidInventory.Height(ItemKind.SKS)==2&&RaidInventory.Width(ItemKind.Green9mm)==1&&RaidInventory.Height(ItemKind.Green9mm)==1,"2710 Inventory footprints stay unchanged");
   Check(WeaponRules279.MP5.Damage==12&&WeaponRules279.MP5.Range==8&&WeaponRules279.SKS.Damage==24&&Mathf.Abs(WeaponRules279.SKS.Range-56f/3)<.001f&&MarketRules.UnitPrice(ItemKind.MP5)==897&&MarketRules.UnitPrice(ItemKind.SKS)==1127,"2710 Art patch leaves weapon parameters and prices intact");
   File.WriteAllLines(Path.Combine(folder,"2710-image-resource-checks.csv"),rows);
   ItemArtContact2710(kinds);
   G.InventoryOpen=true;G.Panel=CampPanel.Market;G.Player.transform.position=RaidMap.Market;yield return null;yield return new WaitForEndOfFrame();Check(G.InventoryOpen&&G.Panel==CampPanel.Market,"2710 Actual market interface renders without missing-texture errors");
   G.Panel=CampPanel.Warehouse;G.Player.transform.position=RaidMap.Warehouse;yield return null;yield return new WaitForEndOfFrame();Check(G.InventoryOpen&&G.Panel==CampPanel.Warehouse,"2710 Actual warehouse interface renders without missing-texture errors");
   G.CloseInventory();for(int i=0;i<formal.Length;i++)Check(MainBytes273(before[i],File.Exists(Path.Combine(dir,formal[i]))?File.ReadAllBytes(Path.Combine(dir,formal[i])):null),"2710 Formal save unchanged "+formal[i]);
  }
  void ItemArtContact2710(ItemKind[] kinds){
   // This is an inventory-art comparison using the live imported assets, not a full game UI screenshot.
   int width=1200,height=660;var t=new Texture2D(width,height,TextureFormat.RGBA32,false);var pixels=new Color32[width*height];
   for(int i=0;i<pixels.Length;i++)pixels[i]=new Color32(19,28,31,255);
   for(int n=0;n<kinds.Length;n++){
    int x=n<3?20+n*395:20+(n-3)*295,y=n<3?350:20,w=n<3?375:275,h=290;
    var tint=RaidInventory.Tint(kinds[n]);Color32 border=tint,paper=Color.Lerp(new Color(.12f,.16f,.17f),tint,.25f);
    for(int yy=y;yy<y+h;yy++)for(int xx=x;xx<x+w;xx++)pixels[yy*width+xx]=xx<x+3||xx>=x+w-3||yy<y+3||yy>=y+h-3?border:paper;
    var tex=ItemIcons.Get(kinds[n]);var uv=ItemIcons.Bounds(kinds[n]);float aspect=tex.width*uv.width/(tex.height*uv.height);int drawW=Mathf.RoundToInt(Mathf.Min(w-28,(h-28)*aspect)),drawH=Mathf.RoundToInt(drawW/aspect);var src=tex.GetPixels32();
    for(int yy=0;yy<drawH;yy++)for(int xx=0;xx<drawW;xx++){
     int sx=Mathf.Clamp(Mathf.FloorToInt((uv.x+(xx+.5f)/drawW*uv.width)*tex.width),0,tex.width-1),sy=Mathf.Clamp(Mathf.FloorToInt((uv.y+(yy+.5f)/drawH*uv.height)*tex.height),0,tex.height-1);Color c=src[sy*tex.width+sx];int dest=(y+(h-drawH)/2+yy)*width+x+(w-drawW)/2+xx;pixels[dest]=Color.Lerp((Color)pixels[dest],c,c.a);
    }
   }
   t.SetPixels32(pixels);t.Apply();File.WriteAllBytes(Path.Combine(folder,"2710-live-imported-item-art.png"),t.EncodeToPNG());Destroy(t);
   File.WriteAllText(Path.Combine(folder,"2710-preview-notes.txt"),"Imported asset comparison: top AK / MP5 / SKS; bottom green7.62 / blue7.62 / green9mm / blue9mm. Quality frames and alpha crops use live game rules. This is not a full HUD or inventory screen capture.\n");
  }
 }
}
