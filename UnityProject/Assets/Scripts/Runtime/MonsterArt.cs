using UnityEngine;
namespace GreyHarbor {
 public static class MonsterArt {
  static Sprite[,] sprites;
  public static Sprite Get(MonsterKind kind,int frame){
   if(MonsterTuning.IsNew(kind))return AssetMonsterArt2717.Get(MonsterTuning.AssetId(kind),frame==4);
   if(sprites==null){sprites=new Sprite[4,2];for(int k=0;k<4;k++)for(int f=0;f<2;f++){
    sprites[k,f]=Resources.Load<Sprite>("Monsters/"+((MonsterKind)k)+"_"+(f==0?"Idle":"Windup"));
    if(!sprites[k,f])Debug.LogError("Missing monster art "+(MonsterKind)k+" / "+f);
   }}return sprites[(int)kind,frame==4?1:0];
  }
  public static void Animate(RaidActor actor,float clock){
   bool windup=actor.Windup>0,attack=actor.AttackPose>0||actor.Lunge>0;
   actor.Body.sprite=Get(actor.Monster,windup||attack?4:0);
   float wave=Mathf.Sin(clock*(actor.MonsterMoving?11:3.2f));
   float squash=actor.MonsterMoving?.025f:.009f;
   actor.Body.transform.localScale=new Vector3(1-wave*squash,1+wave*squash,1);
   // Figure and Body share a transform. Keep the ground pivot fixed; tiny bob only while moving.
   actor.Figure.localPosition=Vector3.up*(actor.MonsterMoving?Mathf.Abs(wave)*.025f:0);
   if(actor.Lunge>0)actor.Figure.localPosition+=Vector3.up*.065f;
   actor.Warning.transform.localPosition=new Vector3(0,MonsterTuning.ArtHeight(actor.Monster)+.16f,0);
  }
 }
}
