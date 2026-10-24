using System.Collections.Generic;
namespace GreyHarbor {public sealed partial class RaidGame {
 // Called before the transaction commits so failed moves/saves restore bindings too.
 void CancelBackpackBindings343(List<ItemStack> before){
  if(Save?.QuickSlots329==null||Bag==null)return;
  foreach(var item in Bag.Slots){
   if(item==null||!QuickSlots329.Supports(item.Kind))continue;
   var old=before?.Find(p=>p.Id==item.Id&&p.Kind==item.Kind);
   if(old!=null&&old.Count>=item.Count)continue;
   for(int i=0;i<Save.QuickSlots329.Length;i++)if(Save.QuickSlots329[i]==(int)item.Kind+1)Save.QuickSlots329[i]=0;
  }
 }
 void MigrateBackpackBindings343(){
  if(Save?.QuickSlots329==null)return;
  for(int i=0;i<Save.QuickSlots329.Length;i++){
   int encoded=Save.QuickSlots329[i];if(encoded<=0)continue;
   bool inBag=Save.CampBag.Exists(p=>p!=null&&(int)p.Kind+1==encoded);
   bool inRig=Save.CampRig.Exists(p=>p!=null&&p.Identified&&p.Count>0&&(int)p.Kind+1==encoded);
   if(inBag&&!inRig)Save.QuickSlots329[i]=0;
  }
 }
}}
