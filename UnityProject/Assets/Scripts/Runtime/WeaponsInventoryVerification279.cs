using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
namespace GreyHarbor {
 public sealed partial class RaidVerification {
  int WICount279(ItemKind kind,ItemOrigin? origin=null)=>G.Chest.Count(kind,origin)+G.Bag.Count(kind,origin);
  string WIState279()=>JsonUtility.ToJson(new BaseSave{CampRig=G.Chest.Snapshot(),CampBag=G.Bag.Snapshot(),Weapon=RaidInventory.Copy(G.Equipped),SecondWeapon=RaidInventory.Copy(G.SecondWeapon),CampMagazine=G.Magazine,CampMagazineFound=G.MagazineFound,CampMagazineAmmo=G.MagazineAmmo});
  void WIEquip279(ItemKind gun,TouchInput input){
   WeaponsEquipFixture279(gun,input);G.Phase=GamePhase.Raid;G.Paused=false;G.InventoryOpen=true;G.Chest=new RaidInventory(10);G.Bag=new RaidInventory(20);G.RigGear=new ItemStack(ItemKind.StandardRig,1);G.PackGear=new ItemStack(ItemKind.StandardPack,1);G.SecondWeapon=null;G.Magazine=G.MagazineFound=0;G.ReloadTime=G.HealTime=0;
  }
  void WIFill279(RaidInventory inventory){while(inventory.FreeCell()>=0){int left=inventory.Add(ItemKind.Scrap,1);Check(left==0,"279 Full-container fixture places real occupied grid cells");}}
  void WeaponsReloadAndSwap279(TouchInput input){
   string save=JsonUtility.ToJson(G.Save);var phase=G.Phase;var panel=G.Panel;bool paused=G.Paused,inventory=G.InventoryOpen,locked=G.CampLocked;
   var chest=G.Chest;var bag=G.Bag;var safe=G.Safe;var weapon=G.Equipped;var secondary=G.SecondWeapon;var rig=G.RigGear;var pack=G.PackGear;var armor=G.Armor;var helmet=G.Helmet;
   int magazine=G.Magazine,found=G.MagazineFound;var ammo=G.MagazineAmmo;float reload=G.ReloadTime,heal=G.HealTime;var reloadArea=G.reloadArea;string reloadId=G.reloadId;
   var rows=new List<string>{"operation,weapon,ammo,capacity,seconds,ordinary_returned,certified_returned"};
   try {
    var guns=new[]{ItemKind.AK,ItemKind.MP5,ItemKind.SKS};int[] capacities={30,30,20};float[] seconds={2.1f,2,2.4f};
    for(int i=0;i<guns.Length;i++)foreach(var bullet in guns[i]==ItemKind.MP5?new[]{ItemKind.Green9mm,ItemKind.Blue9mm}:new[]{ItemKind.GreenAmmo,ItemKind.Ammo}){
     WIEquip279(guns[i],input);G.MagazineAmmo=bullet;G.Chest.Add(bullet,capacities[i]+5,ItemOrigin.Certified);G.BeginReload();
     Check(WNear279(G.ReloadTime,seconds[i]),"279 Actual R reload duration "+guns[i]+" / "+bullet);
     G.TickActions(seconds[i]-.01f);Check(G.Magazine==0&&G.ReloadTime>0,"279 R does not deliver rounds before its real completion "+guns[i]);G.TickActions(.02f);
     Check(G.ReloadTime==0&&G.Magazine==capacities[i]&&G.MagazineFound==capacities[i]&&G.MagazineAmmo==bullet&&G.Chest.Count(bullet)==5,"279 Actual R fills design capacity with selected grade and source "+guns[i]+" / "+bullet);
     G.BeginReload();Check(G.ReloadTime==0,"279 Full magazine refuses another R reload "+guns[i]);rows.Add("R,"+guns[i]+","+bullet+","+G.Magazine+","+WN279(seconds[i])+",0,0");
    }
    foreach(var gun in guns){
     var green=WeaponRules279.GreenAmmoFor(gun);var blue=gun==ItemKind.MP5?ItemKind.Blue9mm:ItemKind.Ammo;var wrong=gun==ItemKind.MP5?ItemKind.GreenAmmo:ItemKind.Green9mm;
     WIEquip279(gun,input);G.Chest.Add(wrong,100);G.Bag.Add(green,100);G.BeginReload();Check(G.ReloadTime==0&&G.Magazine==0,"279 R rejects wrong chest caliber and does not consume backpack rounds "+gun);
     var wrongItem=G.Chest.Slots.Find(p=>p.Kind==wrong);string before=WIState279();Check(!G.CanUse(ItemArea.Rig,wrongItem)&&!G.UseItem(ItemArea.Rig,wrongItem.Cell)&&WIState279()==before,"279 Right-click wrong caliber leaves all carry state unchanged "+gun);
     WIEquip279(gun,input);G.MagazineAmmo=green;G.Magazine=3;G.Chest.Add(blue,40);G.BeginReload();Check(G.ReloadTime==0&&G.Magazine==3&&G.MagazineAmmo==green&&G.Chest.Count(blue)==40,"279 R cannot mix another grade into existing rounds "+gun);
     G.Chest.Add(green,40);G.BeginReload();G.TickActions(WeaponRules279.Get(gun).ReloadSeconds+.01f);Check(G.Magazine==WeaponRules279.Get(gun).MagazineSize&&G.MagazineAmmo==green&&G.Chest.Count(blue)==40,"279 R tops up only existing matching grade "+gun);
     WIEquip279(gun,input);G.Phase=GamePhase.Base;G.CampLocked=false;G.Panel=CampPanel.Warehouse;G.MagazineAmmo=green;G.Magazine=5;G.MagazineFound=2;G.Chest.Add(blue,WeaponRules279.Get(gun).MagazineSize+5,ItemOrigin.Certified);var item=G.Chest.Slots[0];
     Check(G.UseItem(ItemArea.Rig,item.Cell),"279 Actual camp UseItem switches compatible ammo grade "+gun);
     Check(G.Magazine==WeaponRules279.Get(gun).MagazineSize&&G.MagazineFound==G.Magazine&&G.MagazineAmmo==blue&&WICount279(green,ItemOrigin.Ordinary)==3&&WICount279(green,ItemOrigin.Certified)==2,"279 Camp grade switch returns old ordinary/certified rounds separately and fills correct capacity "+gun);
     rows.Add("camp-grade-switch,"+gun+","+blue+","+G.Magazine+",0,3,2");
     WIEquip279(gun,input);G.Phase=GamePhase.Base;G.CampLocked=false;G.Chest.Add(green,WeaponRules279.Get(gun).MagazineSize+5);item=G.Chest.Slots[0];Check(G.UseItem(ItemArea.Rig,item.Cell)&&G.Magazine==WeaponRules279.Get(gun).MagazineSize&&G.MagazineFound==0&&G.Chest.Count(green)==5,"279 Camp ordinary ammo UseItem follows weapon capacity "+gun);
    }
    WIEquip279(ItemKind.AK,input);G.Magazine=27;G.MagazineFound=7;G.Bag.Add(ItemKind.SKS,1);var incoming=G.Bag.Slots.Find(p=>p.Kind==ItemKind.SKS);string akId=G.Equipped.Id,sksId=incoming.Id;
    Check(G.CanMoveItem(ItemArea.Backpack,incoming.Cell,ItemArea.Weapon,0)&&G.MoveItem(ItemArea.Backpack,incoming.Cell,ItemArea.Weapon,0),"279 Actual AK27 to SKS equipment operation succeeds");
    Check(G.Equipped.Kind==ItemKind.SKS&&G.Equipped.Id==sksId&&G.Magazine==0&&G.MagazineFound==0&&G.Bag.Slots.Exists(p=>p.Kind==ItemKind.AK&&p.Id==akId)&&WICount279(ItemKind.GreenAmmo,ItemOrigin.Ordinary)==20&&WICount279(ItemKind.GreenAmmo,ItemOrigin.Certified)==7,"279 AK27 swap returns all27 rounds without20-capacity truncation or source loss");rows.Add("AK27-to-SKS,SKS,GreenAmmo,0,0,20,7");
    WIEquip279(ItemKind.AK,input);G.Magazine=27;G.MagazineFound=7;G.SecondWeapon=new ItemStack(ItemKind.SKS,1);WIFill279(G.Chest);WIFill279(G.Bag);string carryBefore=WIState279(),saveBefore=JsonUtility.ToJson(G.Save);
    Check(!G.CanMoveItem(ItemArea.SecondWeapon,0,ItemArea.Weapon,0)&&!G.MoveItem(ItemArea.SecondWeapon,0,ItemArea.Weapon,0)&&WIState279()==carryBefore&&JsonUtility.ToJson(G.Save)==saveBefore,"279 Completely full rig and pack atomically reject main/secondary swap with exact snapshots");
    WIEquip279(ItemKind.AK,input);G.Magazine=27;G.MagazineFound=7;G.Chest.Add(ItemKind.GreenAmmo,80);WIFill279(G.Chest);G.Bag.Add(ItemKind.SKS,1);incoming=G.Bag.Slots.Find(p=>p.Kind==ItemKind.SKS);WIFill279(G.Bag);carryBefore=WIState279();saveBefore=JsonUtility.ToJson(G.Save);
    Check(!G.CanMoveItem(ItemArea.Backpack,incoming.Cell,ItemArea.Weapon,0)&&!G.MoveItem(ItemArea.Backpack,incoming.Cell,ItemArea.Weapon,0)&&WIState279()==carryBefore&&JsonUtility.ToJson(G.Save)==saveBefore,"279 Space for ordinary20 but no separate certified7 rejects swap without partial unloading");
    WIEquip279(ItemKind.MP5,input);G.Magazine=17;G.MagazineFound=4;G.SecondWeapon=new ItemStack(ItemKind.SKS,1);string mp5Id=G.Equipped.Id;sksId=G.SecondWeapon.Id;
    Check(G.MoveItem(ItemArea.SecondWeapon,0,ItemArea.Weapon,0)&&G.Equipped.Id==sksId&&G.SecondWeapon.Id==mp5Id&&G.Magazine==0&&G.MagazineAmmo==ItemKind.GreenAmmo&&WICount279(ItemKind.Green9mm,ItemOrigin.Ordinary)==13&&WICount279(ItemKind.Green9mm,ItemOrigin.Certified)==4,"279 Actual main/secondary swap unloads all9mm and preserves both gun identities");
    Check(G.MoveItem(ItemArea.SecondWeapon,0,ItemArea.Weapon,0)&&G.Equipped.Id==mp5Id&&G.SecondWeapon.Id==sksId&&G.Magazine==0&&G.MagazineAmmo==ItemKind.Green9mm&&WICount279(ItemKind.Green9mm)==17,"279 Empty reverse main/secondary swap preserves ammunition total");rows.Add("main-secondary,SKS,Green9mm,0,0,13,4");
   }finally{
    G.Save=JsonUtility.FromJson<BaseSave>(save);G.Phase=phase;G.Panel=panel;G.Paused=paused;G.InventoryOpen=inventory;G.CampLocked=locked;G.Chest=chest;G.Bag=bag;G.Safe=safe;G.Equipped=weapon;G.SecondWeapon=secondary;G.RigGear=rig;G.PackGear=pack;G.Armor=armor;G.Helmet=helmet;G.Magazine=magazine;G.MagazineFound=found;G.MagazineAmmo=ammo;G.ReloadTime=reload;G.HealTime=heal;G.reloadArea=reloadArea;G.reloadId=reloadId;G.ResetAkHandling276();input.Shooting=false;
    File.WriteAllLines(Path.Combine(folder,"279-real-reload-and-swap.csv"),rows);
   }
  }
 }
}
