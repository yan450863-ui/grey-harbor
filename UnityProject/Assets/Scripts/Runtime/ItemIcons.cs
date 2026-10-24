using UnityEngine;
namespace GreyHarbor {
 public static class ItemIcons {
  static Texture2D[] images;static Color Ink=>PixelArt.Hex("1B2423");
  static void R(Texture2D t,int x,int y,int w,int h,string hex){Color c=PixelArt.Hex(hex);for(int j=Mathf.Max(0,y);j<Mathf.Min(40,y+h);j++)for(int i=Mathf.Max(0,x);i<Mathf.Min(48,x+w);i++)t.SetPixel(i,j,c);}
  static void Box(Texture2D t,int x,int y,int w,int h,string hex){R(t,x-1,y-1,w+2,h+2,"1B2423");R(t,x,y,w,h,hex);R(t,x,y+h-2,w,2,"B2BAAA");}
  public static Texture2D Get(ItemKind k){if(images==null)images=new Texture2D[RaidInventory.KindCount];if(images[(int)k]==null)images[(int)k]=Make(k);return images[(int)k];}
  static readonly System.Collections.Generic.Dictionary<ItemKind,Rect> bounds=new System.Collections.Generic.Dictionary<ItemKind,Rect>();
  public static Rect Bounds(ItemKind k){
   if(bounds.TryGetValue(k,out var uv))return uv;
   var t=Get(k);var pixels=t.GetPixels32();int minX=t.width,minY=t.height,maxX=-1,maxY=-1;
   for(int y=0;y<t.height;y++)for(int x=0;x<t.width;x++)if(pixels[y*t.width+x].a>16){minX=Mathf.Min(minX,x);minY=Mathf.Min(minY,y);maxX=Mathf.Max(maxX,x);maxY=Mathf.Max(maxY,y);}
   uv=maxX<0?new Rect(0,0,1,1):new Rect((float)minX/t.width,(float)minY/t.height,(float)(maxX-minX+1)/t.width,(float)(maxY-minY+1)/t.height);
   bounds[k]=uv;return uv;
  }
  static Texture2D Make(ItemKind k){if(AmmoCrafting328.IsComponent(k))return Get(AmmoCrafting328.IconSource(k));if(ArmorLoadout320.IsNew(k))return ArmorLoadout320.Texture(k);if(KeyCase314.IsCase(k))return KeyCase314.Texture();if(BlueWeapons30.IsNew(k)||ModRules30.IsMod(k))return BlueWeaponsArt30.Texture(k);if(KeyRules2760.IsKey(k))return KeyIcons2760.Make(k);if(FormalCollectibles2749.IsNew(k))return FormalCollectibles2749.Texture(k);if(k==ItemKind.Green9mm||k==ItemKind.Blue9mm){var ammoArt=Resources.Load<Texture2D>("ItemArt/NineMM");if(ammoArt!=null)return ammoArt;}if(k==ItemKind.GreenAmmo)return Get(ItemKind.Ammo);var asset=Resources.Load<Texture2D>("ItemArt/"+k);if(asset!=null)return asset;if(WeaponRules279.IsWeapon(k))return PixelArt.WeaponSprite279(k).texture;var t=new Texture2D(48,40,TextureFormat.RGBA32,false){filterMode=FilterMode.Point};t.SetPixels32(new Color32[48*40]);
   if(RaidInventory.IsRig(k)||k==ItemKind.Armor){Box(t,10,5,28,27,k==ItemKind.LargeRig?"677958":"7C805C");R(t,12,28,5,9,"59664D");R(t,31,28,5,9,"59664D");R(t,18,27,12,10,"1B2423");for(int q=0;q<3;q++)Box(t,12+q*8,9,6,12,"A0A17A");R(t,22,6,2,22,"3A473B");t.Apply();return t;}
   if(RaidInventory.IsPack(k)){Box(t,12,3,25,31,k==ItemKind.SmallPack?"738B6A":k==ItemKind.LargePack?"737E55":"A0926D");Box(t,16,29,17,5,"B1AF84");Box(t,16,7,17,13,"817C57");Box(t,7,8,5,17,"616D53");Box(t,37,8,5,17,"616D53");R(t,18,17,13,2,"C2B889");R(t,16,35,4,3,"394738");R(t,29,35,4,3,"394738");t.Apply();return t;}
   switch(k){
    case ItemKind.Helmet:Box(t,8,10,33,17,"697B5A");Box(t,13,26,24,8,"919A72");R(t,18,33,15,3,"A6AC83");Box(t,8,9,33,5,"A3A782");R(t,12,4,6,7,"4B5D47");R(t,33,4,6,7,"4B5D47");break;
    case ItemKind.Biomass:Box(t,11,9,25,20,"9A6B60");Box(t,17,24,15,8,"B18D78");R(t,18,12,6,15,"CFB5A0");break;
    case ItemKind.Hide:Box(t,8,8,29,23,"8D7854");R(t,15,13,16,13,"B5A079");R(t,10,28,7,6,"8D7854");R(t,29,4,8,6,"8D7854");break;
    case ItemKind.Chitin:Box(t,7,10,33,23,"647C75");R(t,13,29,21,5,"93A698");R(t,20,12,3,19,"364B49");R(t,9,20,29,3,"364B49");break;
    case ItemKind.Ammo:for(int i=0;i<3;i++){int x=8+i*12,y=4+i*3;Box(t,x,y,7,22,"AD955B");R(t,x+2,y+22,3,5,"E5D5A4");R(t,x+1,y+2,2,16,"D5C28E");}break;
    case ItemKind.Green9mm:case ItemKind.Blue9mm:
     Box(t,7,4,35,13,k==ItemKind.Green9mm?"527A59":"4D7296");R(t,10,7,29,3,k==ItemKind.Green9mm?"80AC7D":"84AED0");
     for(int i=0;i<3;i++){int x=11+i*11,y=16+i%2*2;Box(t,x,y,7,10,"AD955B");R(t,x+1,y+2,2,7,"DBC98D");R(t,x+1,y+10,5,3,"D8BF80");R(t,x+2,y+13,3,2,"EFE0AE");}
     // A small stamped 9 on the colour-coded tray distinguishes calibre without relying on tint.
     R(t,21,7,6,7,"E5E6CD");R(t,22,11,3,2,"283F3C");R(t,21,7,4,2,"283F3C");break;
    case ItemKind.Medkit:Box(t,7,7,34,24,"B34F49");Box(t,17,31,14,4,"575F54");R(t,21,12,6,14,"F0E4C8");R(t,17,16,14,6,"F0E4C8");break;
    case ItemKind.Cola:Box(t,15,4,19,31,"A74138");R(t,17,6,15,3,"AAB5AB");R(t,20,10,5,19,"F1DCC0");R(t,25,14,4,12,"E0CFB3");break;
    case ItemKind.Powder:Box(t,12,4,26,29,"717B56");Box(t,15,32,20,4,"414B44");R(t,15,11,20,13,"D3C7A0");R(t,23,14,4,7,"68543A");break;
    case ItemKind.Circuit:Box(t,7,6,35,28,"45757A");for(int i=0;i<4;i++){R(t,10+i*8,9,2,22,"97BDA3");}Box(t,18,13,14,13,"253B3E");break;
    case ItemKind.GunParts:Box(t,6,15,36,11,"68746F");for(int i=0;i<6;i++)R(t,8+i*5,17,2,7,"303C38");R(t,12,9,8,5,"8C9B8B");break;
    case ItemKind.Scrap:Box(t,5,7,17,12,"6D7668");Box(t,23,16,19,13,"8A8B73");Box(t,12,25,17,10,"A0A18A");R(t,8,10,8,3,"363F38");break;
    case ItemKind.Cloth:Box(t,7,7,33,12,"858F7D");Box(t,9,15,29,11,"A4AE97");Box(t,11,24,25,7,"C1C7AF");break;
    case ItemKind.Medical:Box(t,8,7,13,25,"CAD5BE");Box(t,11,31,7,4,"547C6C");Box(t,26,7,13,21,"96BCA9");R(t,29,18,7,3,"E8EACD");R(t,31,16,3,7,"E8EACD");break;
    case ItemKind.Tools:R(t,10,7,6,27,"95B0AF");R(t,6,28,14,6,"BDC9B8");R(t,25,6,7,21,"5E7F88");R(t,27,25,3,11,"BBC7B8");break;
    case ItemKind.Intel:Box(t,11,5,27,31,"B59753");R(t,14,29,20,3,"E3CC8B");for(int i=0;i<5;i++)R(t,15,10+i*3,17,1,"776839");R(t,17,25,14,2,"473E29");break;
   }t.Apply();return t;
  }
 }
}
