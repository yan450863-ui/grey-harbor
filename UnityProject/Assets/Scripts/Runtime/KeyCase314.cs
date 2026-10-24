using System;using System.IO;using System.Collections.Generic;using UnityEngine;
namespace GreyHarbor {
 public static class KeyCase314 {
  public static bool IsCase(ItemKind kind)=>kind==ItemKind.GreenKeyCase314;
  public static List<ItemStack> CopyKeys(List<ItemStack> keys){var result=new List<ItemStack>();if(keys==null)return result;if(keys.Count>4)throw new InvalidOperationException("钥匙包超过4格");foreach(var key in keys){if(key==null||!KeyRules2760.IsKey(key.Kind)||key.Count!=1)throw new InvalidOperationException("钥匙包只能收纳钥匙");result.Add(RaidInventory.Copy(key));}return result;}
  public static RaidInventory Grid(ItemStack bag){if(bag==null||!IsCase(bag.Kind))return null;if(bag.Keys314==null)bag.Keys314=new List<ItemStack>();return new RaidInventory(2,2,bag.Keys314);}
  public static void Ensure(ItemStack bag){if(bag==null)return;if(!IsCase(bag.Kind)){if(bag.Keys314!=null&&bag.Keys314.Count>0)throw new InvalidOperationException("只有钥匙包能保存内部钥匙");return;}if(bag.Count!=1)throw new InvalidOperationException("钥匙包不能堆叠");var keys=CopyKeys(bag.Keys314);var grid=new RaidInventory(2,2);grid.Restore(keys);bag.Keys314=grid.Snapshot();}
  public static int TotalValue(ItemStack bag){int n=4250;if(bag.Keys314!=null)foreach(var key in bag.Keys314)n=checked(n+RaidInventory.Value(key));return n;}
  public static int FoundValue(ItemStack bag){int n=bag.RaidFound?4250:0;if(bag.Keys314!=null)foreach(var key in bag.Keys314)if(key.RaidFound)n+=RaidInventory.Value(key);return n;}
  static Texture2D texture;
  public static Texture2D Texture(){if(texture)return texture;texture=Resources.Load<Texture2D>("ItemArt/GreenKeyCase314");if(texture)return texture;var path=Path.Combine(Application.streamingAssetsPath,"ItemArt314/GreenKeyCase314.png");if(!File.Exists(path))throw new IOException("钥匙包图片缺失");texture=new Texture2D(2,2,TextureFormat.RGBA32,false);if(!texture.LoadImage(File.ReadAllBytes(path)))throw new IOException("钥匙包图片无效");texture.filterMode=FilterMode.Point;texture.wrapMode=TextureWrapMode.Clamp;return texture;}
 }
 public sealed partial class RaidGame {
  public string OpenKeyCaseId314;public ItemArea OpenKeyCaseArea314;
  public ItemStack OpenKeyCase314 {get {if(string.IsNullOrEmpty(OpenKeyCaseId314))return null;foreach(var a in new[]{OpenKeyCaseArea314,ItemArea.Safe,ItemArea.Backpack,ItemArea.Rig,ItemArea.Warehouse,ItemArea.Loot}){if(!Access(a))continue;var found=InventoryFor(a)?.Slots.Find(p=>p.Id==OpenKeyCaseId314&&KeyCase314.IsCase(p.Kind));if(found!=null){OpenKeyCaseArea314=a;return found;}}return null;}}
  public void CloseKeyCase314(){OpenKeyCaseId314=null;}
  public bool OpenKeyCase314At(ItemArea area,int cell){if(!InventoryOpen||!Access(area))return false;var bag=ItemAt(area,cell);if(bag==null||!bag.Identified||!KeyCase314.IsCase(bag.Kind))return false;OpenKeyCaseId314=bag.Id;OpenKeyCaseArea314=area;return true;}
  bool KeyGridAllowed314(ItemArea area,ItemStack item)=>area!=ItemArea.KeyCase314||item!=null&&KeyRules2760.IsKey(item.Kind);
  bool CanInsertCase314(ItemArea from,int cell,ItemArea to,int targetCell){var key=ItemAt(from,cell);var bag=InventoryFor(to)?.At(targetCell);return !CorpseSource325(to)&&Access(from)&&Access(to)&&key!=null&&key.Identified&&KeyRules2760.IsKey(key.Kind)&&bag!=null&&bag.Identified&&KeyCase314.IsCase(bag.Kind)&&KeyCase314.Grid(bag).FreeCell(key)>=0;}
  bool InsertCase314(ItemArea from,int cell,ItemArea to,int targetCell){return MutateItems(()=>{var key=ItemAt(from,cell);var bag=InventoryFor(to)?.At(targetCell);if(!CanInsertCase314(from,cell,to,targetCell))return false;var grid=KeyCase314.Grid(bag);if(grid.Add(key)!=0)return false;RemoveAmount(from,key,1);return true;},"钥匙已收入钥匙包。");}
 }
 public sealed partial class RaidUI {
  readonly Rect keyCasePanel314=new Rect(1188,496,352,282);
  void DrawKeyCase314(){var bag=G.OpenKeyCase314;if(bag==null){G.CloseKeyCase314();return;}hits.RemoveAll(h=>h.Rect.Overlaps(keyCasePanel314));Frame(keyCasePanel314,inset,mint*.65f);Label(1206,512,246,26,"钥匙包 · "+bag.Keys314.Count+" / 4",21,mint);if(PanelButton(new Rect(1480,507,42,32),"×")){G.CloseKeyCase314();return;}ItemIcon(new Rect(1207,558,88,96),bag.Kind);DrawGrid(KeyCase314.Grid(bag),ItemArea.KeyCase314,new Rect(1350,561,100,100),2,2,0);WrappedLabel(new Rect(1206,681,310,66),"仅收纳钥匙；拖入、拖出或双击取出。安全箱内的钥匙包和钥匙一起保留。",14,muted);}
 }
}
