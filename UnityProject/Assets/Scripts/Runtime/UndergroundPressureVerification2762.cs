using System;using System.IO;using UnityEngine;
namespace GreyHarbor { public sealed partial class RaidVerification {
 void UEPressure2762(RaidActor e){
  var home=e.transform.position;var player=G.Player.transform.position;var armor=G.Armor;var helmet=G.Helmet;float hp=G.Player.Health;bool paused=G.Paused;
  try {
   Vector3 direction=Vector3.zero;for(int i=0;i<48;i++){var d=Quaternion.Euler(0,i*7.5f,0)*Vector3.forward;var a=home+d*.85f;var b=home+d*6;bool clear=G.Navigation.ClearSegment(home,b);for(int j=0;j<=24&&clear;j++){var p=Vector3.Lerp(home,b,j/24f);clear=G.Map.UndergroundFloorContains2752(p,.34f)&&CargoClear2713(p);}if(clear){direction=d;break;}}
   Check(direction!=Vector3.zero,"2762 free supported six metre real pursuit lane "+e.Monster);if(direction==Vector3.zero)return;
   G.Armor=G.Helmet=null;G.Player.Health=100;G.Player.Dead=false;G.Player.HitCollider.enabled=true;G.Paused=true;G.HealTime=0;
   e.transform.position=home;G.Player.transform.position=home+direction*6;Physics.SyncTransforms();UEResetAttack2757(e);e.AttackCooldown=2;for(int n=0;n<40;n++){G.TickMonster(e,.025f);Physics.SyncTransforms();}
   float moved=Vector3.Distance(home,e.transform.position);Check(moved>=MonsterTuning.Speed(e.Monster)*.75f,"2762 actual one second path pursuit accelerated "+e.Monster+" travelled="+moved);
   foreach(float playerSpeed in new[]{3.4f,5.1f}){
    e.transform.position=home;float initial=e.Monster==MonsterKind.PaperbackCreeper?2.2f:.85f;G.Player.transform.position=home+direction*initial;Physics.SyncTransforms();G.Player.Health=100;G.Player.Dead=false;UEResetAttack2757(e);G.TickMonster(e,.001f);
    Check(e.Windup>0,"2762 real close range attack committed "+e.Monster+" retreat="+playerSpeed);
    float elapsed=0;for(int n=0;n<84;n++){G.Player.Move(direction*playerSpeed*.01f);Physics.SyncTransforms();G.TickMonster(e,.01f);Physics.SyncTransforms();elapsed+=.01f;if(e.Windup==0&&e.Lunge==0)break;}
    if(playerSpeed==3.4f)Check(G.Player.Health<100,"2762 actual first strike catches walking retreat "+e.Monster+" HP="+G.Player.Health+" elapsed="+elapsed);
    else Check(G.Player.Health==100,"2762 committed strike remains avoidable with sprint "+e.Monster+" HP="+G.Player.Health);
    UERow2757("pressure2762",e,"player-speed="+playerSpeed+";HP="+G.Player.Health+";elapsed="+elapsed);
   }
  } finally {e.transform.position=home;G.Player.transform.position=player;G.Player.Health=hp;G.Player.Dead=false;G.Armor=armor;G.Helmet=helmet;G.Paused=paused;G.HealTime=0;UEResetAttack2757(e);Physics.SyncTransforms();}
 }
}}
