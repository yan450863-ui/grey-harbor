using UnityEngine;
namespace GreyHarbor {
 public sealed partial class RaidUI {
  internal static Rect HudWeaponFrame329=>new Rect(1242,708,334,111);
  internal static Rect HudQuickFrame329=>new Rect(1242,828,334,48);
  internal static Rect QuickRect329(int slot)=>new Rect(1242+slot*67,828,66,48);
  internal static bool SprintReady329(SprintState2711 sprint)=>sprint!=null&&sprint.ReadyToRun;
  internal static Color AmmoTint329(ItemKind kind)=>RaidInventory.Tint(kind);
  internal static Color ArmorTint329(ItemStack armor)=>armor==null?PixelArt.Hex("8EA4A7"):RaidInventory.Tint(armor.Kind);
  internal static float HudFill329(float current,float maximum)=>maximum>0?Mathf.Clamp01(current/maximum):0;
  readonly Color hudSurface329=new Color(.047f,.074f,.083f,.82f),hudEdge329=new Color(.38f,.51f,.49f,.60f);
  void HudPanel329(Rect rect){GUI.DrawTexture(rect,Texture2D.whiteTexture,ScaleMode.StretchToFill,true,0,hudSurface329,0,8);GUI.DrawTexture(rect,Texture2D.whiteTexture,ScaleMode.StretchToFill,true,0,hudEdge329,1,8);}
  internal const float HudScale330=.75f;
  static Matrix4x4 HudTransform330(Vector2 anchor)=>Matrix4x4.TRS(new Vector3(anchor.x*(1-HudScale330),anchor.y*(1-HudScale330),0),Quaternion.identity,new Vector3(HudScale330,HudScale330,1));
  internal static Rect HudRect330(Rect rect,Vector2 anchor)=>new Rect(anchor+(rect.position-anchor)*HudScale330,rect.size*HudScale330);
  void HudIcon329(HudGlyph329 glyph,Rect rect,Color tint){var old=GUI.color;GUI.color=tint;GUI.DrawTexture(rect,HudIcons329.Get(glyph),ScaleMode.ScaleToFit,true);GUI.color=old;}
  internal int HudRepaints332 {get;private set;}
  void DrawRaidHud329(){
   if(!G.Player||G.Map==null||Event.current.type!=EventType.Repaint)return;
   HudRepaints332++;
   var prior=GUI.matrix;
   try {GUI.matrix=prior*HudTransform330(new Vector2(28,22));MiniMap();
    GUI.matrix=prior;Compass329();
    GUI.matrix=prior*HudTransform330(new Vector2(28,876));PlayerStatus329();
    GUI.matrix=prior*HudTransform330(new Vector2(1576,876));Weapons329();QuickSlotsHud329();
   } finally {GUI.matrix=prior;}
   if(!G.InventoryOpen&&!G.Paused&&!FullMapOpen318){EnemyStatus344();TrainingDummyLabels315();Interactions329();Notice329();}
  }
  void Compass329(){
   float yaw=Mathf.Repeat(G.Rig.Yaw,360);int center=Mathf.RoundToInt(yaw/15);string[] dirs={"N","NE","E","SE","S","SW","W","NW"};
   for(int i=center-5;i<=center+5;i++){float x=800+Mathf.DeltaAngle(yaw,i*15)*2;bool major=(i%3+3)%3==0;Fill(new Rect(x,35,1,major?10:5),major?white:muted);if(major)Label(x-20,51,40,18,dirs[((i/3)%8+8)%8],12,white,TextAnchor.MiddleCenter);}
   Fill(new Rect(797,27,7,2),mint);Fill(new Rect(799,29,3,3),mint);
  }
  void PlayerStatus329(){
   HudPanel329(new Rect(28,733,342,143));
   StatusRow329(748,HudGlyph329.Heart,G.Player.Health,PlayerLife321.Maximum,PixelArt.Hex("DF655B"),G.Player.Health<35?red:mint,Mathf.CeilToInt(G.Player.Health).ToString());
   Color ac=ArmorTint329(G.Armor);StatusRow329(789,HudGlyph329.Shield,G.TotalArmor,G.MaxArmor,ac,ac,Mathf.CeilToInt(G.TotalArmor).ToString());
   Color sc=SprintReady329(G.Sprint2711)?white:red;StatusRow329(830,HudGlyph329.Bolt,G.Sprint2711.Stamina,SprintState2711.Maximum,sc,sc,G.Sprint2711.Stamina.ToString("0.#"));
   HudIcon329(HudGlyph329.Pack,new Rect(40,694,24,24),muted);
   Label(73,694,155,25,(G.CarryGrams320/1000f).ToString("0.0")+" / 45 kg",18,G.CarryGrams320>30000?gold:white);
   float noise=G.Sound326.OwnNoise;for(int i=0;i<9;i++){float profile=1-Mathf.Abs(i-4)/5f;float pulse=.74f+.26f*Mathf.Sin(Time.time*9+i*.9f);float h=2+noise*(8+profile*17)*pulse;Fill(new Rect(234+i*7,706-h/2,4,h),noise<.02f?muted*.5f:mint);}
  }
  void StatusRow329(float y,HudGlyph329 glyph,float value,float maximum,Color iconColor,Color barColor,string number){HudIcon329(glyph,new Rect(48,y+1,32,32),iconColor);Fill(new Rect(94,y+18,193,5),new Color(.22f,.29f,.27f));Fill(new Rect(94,y+18,193*HudFill329(value,maximum),5),barColor);Label(297,y+2,55,30,number,22,barColor,TextAnchor.MiddleRight);}
  void Weapons329(){
   HudPanel329(HudWeaponFrame329);int selected=G.SelectedWeaponSlot329;
   for(int i=0;i<2;i++){Rect r=new Rect(1254+i*27,717,21,18);Fill(r,selected==i?new Color(.25f,.48f,.38f):new Color(.10f,.16f,.16f));Outline(r,selected==i?mint:muted*.7f,1);Label(r.x,r.y-1,r.width,r.height,(i+1).ToString(),12,selected==i?white:muted,TextAnchor.MiddleCenter);}
   var w=G.Equipped;var p=G.ActiveWeapon279;Color ammo=AmmoTint329(G.MagazineAmmo);
   Label(1317,715,106,25,p==null?"—":p.Name,18,w==null?muted:RaidInventory.Tint(w.Kind));
   Label(1424,717,140,22,p==null?"":p.CaliberLabel,13,muted,TextAnchor.MiddleRight);
   if(w!=null)ItemIcon(new Rect(1257,747,201,47),w.Kind);
   Label(1460,740,68,49,p==null?"—":G.Magazine.ToString(),40,ammo,TextAnchor.MiddleRight);
   Label(1528,751,42,34,p==null?"":"/"+G.MagazineCapacity279,22,ammo,TextAnchor.MiddleLeft);
   Label(1455,790,106,24,"备 "+(G.TrainingLoadoutAvailable301?"∞":WeaponHudReserve279(w,G.Chest).ToString()),16,ammo,TextAnchor.MiddleRight);
  }
  void QuickSlotsHud329(){
   for(int i=0;i<5;i++){var r=QuickRect329(i);HudPanel329(r);Label(r.x+5,r.y+2,16,17,(i+3).ToString(),12,muted);var kind=G.QuickSlotKind329(i);if(!kind.HasValue)continue;float amount=G.QuickSlotAmount329(i);var color=GUI.color;GUI.color=amount>0?Color.white:new Color(.42f,.42f,.42f,.65f);ItemIcon(new Rect(r.x+18,r.y+5,39,28),kind.Value);GUI.color=color;Label(r.x+16,r.y+30,r.width-23,16,amount.ToString("0.#"),12,amount>0?RaidInventory.Tint(kind.Value):muted,TextAnchor.MiddleRight);}
  }
  internal void SetScreenScale332(){scale=Mathf.Min(Screen.width/1600f,Screen.height/900f);}
  internal Vector2 PlayerHead332(){var body=G.Player.Body;if(body&&body.sprite){var b=body.sprite.bounds;return ScreenPoint(body.transform.TransformPoint(new Vector3(b.center.x,b.max.y,b.center.z)))+new Vector2(0,-8);}return ScreenPoint(G.Player.transform.position+Vector3.up*1.8f);}
  internal static string PlayerNotice332(string text)=>text!=null&&text.StartsWith("弹匣已空")?"无弹 · R 换弹":text!=null&&text.StartsWith("胸挂没有当前弹种")?"无可用备弹":text;
  void Notice329(){if(G.NoticeTime<=0||string.IsNullOrEmpty(G.Notice)||G.ReloadTime>0||G.RepairTime>0||G.ExtractTime>0)return;float alpha=Mathf.Clamp01(G.NoticeTime/.35f);var old=GUI.color;GUI.color=new Color(1,1,1,alpha);SmallBubble331(PlayerHead332(),PlayerNotice332(G.Notice),gold,-1,false,true);GUI.color=old;}
  void ActionRing329(HudGlyph329 glyph,Vector2 position,float amount,Color color,string contextLabel=null){
   string label=string.IsNullOrEmpty(contextLabel)?"交互":contextLabel.Replace("F · ","");
   if(label=="F")label="开关门";
   SmallBubble331(position,label,color,amount>0?amount:-1,true);
  }

  void Interactions329(){
   var self=PlayerHead332();
   if(G.RepairTime>0){SmallBubble331(self,"维修 "+G.RepairTime.ToString("0.0")+"s",mint,1-G.RepairTime/4,false,true);return;}
   if(G.ReloadTime>0){SmallBubble331(self,"换弹 "+G.ReloadTime.ToString("0.0")+"s",gold,1-G.ReloadTime/(G.ActiveWeapon279?.ReloadSeconds??WeaponRules279.AK.ReloadSeconds),false,true);return;}
   if(G.ExtractTime>0){SmallBubble331(self,"撤离 "+Mathf.Max(0,5-G.ExtractTime).ToString("0.0")+"s",mint,G.ExtractTime/5,false,true);return;}
   if(G.NearbyQuest319!=null){ActionRing329(HudGlyph329.Task,ScreenPoint(G.NearbyQuest319.transform.position+Vector3.up*1.4f),G.QuestHold319/2,gold,"F · "+G.NearbyQuest319.Definition.Name);return;}
   if(G.NearbyDoor!=null){var d=G.NearbyDoor;string label=d.Locked?(KeyRules2760.IsKey(d.KeyKind2760)?"F · "+RaidInventory.Name(d.KeyKind2760):"已上锁"):d.Portal?"F · "+d.Name:"F";if(label.Length>19)label=label.Substring(0,18)+"…";ActionRing329(d.Locked?HudGlyph329.Lock:HudGlyph329.Door,DoorAnchor341(d),d.Portal?G.DoorHold/2:0,mint,label);InteractionFocus();return;}
   if(G.Nearby!=null)LootBubble331(G.Nearby);
  }
 }
}
