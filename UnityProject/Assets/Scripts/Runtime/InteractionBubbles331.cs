using UnityEngine;
namespace GreyHarbor { public sealed partial class RaidUI {
  internal static string LootBubbleName331(LootContainer box)=>string.IsNullOrEmpty(box?.Name)?"物资容器":box.Name;
  internal static ItemKind? LootBubbleHighest331(LootContainer box){
   if(box==null||!box.Opened)return null;
   ItemKind? highest=null;int grade=-1;
   System.Collections.Generic.IEnumerable<ItemStack> items=box.Corpse325?.Soldier==true?box.Corpse325.AllItems():box.Items;
   foreach(var item in items){if(item==null||item.Count<=0||!item.Identified)continue;int g=RaidInventory.Grade(item.Kind);if(g>grade){grade=g;highest=item.Kind;}}
   return highest;
  }
  internal static Color LootBubbleTint331(LootContainer box){var item=LootBubbleHighest331(box);return item.HasValue?RaidInventory.Tint(item.Value):new Color(.70f,.76f,.73f);}
  void SmallBubble331(Vector2 anchor,string name,Color color,float progress=-1,bool interact=false,bool player=false){
   if(name.Length>(player?66:20))name=name.Substring(0,player?65:19)+"…";
   float width=Mathf.Clamp(name.Length*14+24+(interact?20:0),82,320),height=progress>=0?37:30;
   if(player&&name.Length>20){width=320;height=16+Mathf.CeilToInt(name.Length/21f)*19+(progress>=0?7:0);}
   float x=Mathf.Clamp(anchor.x-width/2,12,1588-width),y=Mathf.Clamp(anchor.y-height-9,12,888-height);var r=new Rect(x,y,width,height);
   GUI.DrawTexture(r,Texture2D.whiteTexture,ScaleMode.StretchToFill,true,0,new Color(.035f,.06f,.06f,.86f),0,6);
   GUI.DrawTexture(r,Texture2D.whiteTexture,ScaleMode.StretchToFill,true,0,new Color(color.r,color.g,color.b,.55f),1,6);
   var before=GUI.matrix;GUI.matrix=before*Matrix4x4.TRS(new Vector3(Mathf.Clamp(anchor.x,r.x+8,r.xMax-8),r.yMax-2,0),Quaternion.Euler(0,0,45),Vector3.one);Fill(new Rect(0,0,6,6),new Color(.035f,.06f,.06f,.86f));GUI.matrix=before;
   if(interact)Label(x+8,y+6,18,19,"F",12,muted,TextAnchor.MiddleCenter);
   if(player&&name.Length>20)WrappedLabel(new Rect(x+9,y+5,width-18,height-10-(progress>=0?7:0)),name,14,color);
   else Label(x+(interact?28:9),y+5,width-(interact?36:18),21,name,14,color,TextAnchor.MiddleCenter);
   if(progress>=0){Fill(new Rect(x+9,y+height-7,width-18,2),new Color(.24f,.30f,.28f));Fill(new Rect(x+9,y+height-7,(width-18)*Mathf.Clamp01(progress),2),color);}
  }
  void LootBubble331(LootContainer box){
   float progress=!box.Opened&&box.Search>0?HudFill329(box.Search,box.SearchDuration):-1;
   SmallBubble331(ContainerAnchor340(box),LootBubbleName331(box),LootBubbleTint331(box),progress,true);
  }
 }}
