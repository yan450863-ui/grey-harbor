using UnityEngine;
namespace GreyHarbor {
 public sealed partial class RaidMap {
  public Transform FerryBulkheads145;
  void DressFerryBulkheads145(){
   var go=new GameObject("MarineBulkheads145");go.transform.SetParent(Root,false);FerryBulkheads145=go.transform;
   var life=go.AddComponent<WorkshopArtResources>();var a=new WorkshopWastelandArt(this);
   foreach(var r in Root.GetComponentsInChildren<MeshRenderer>())if(r.name=="InteriorWall"){
    var mat=new Material(r.sharedMaterial);mat.color=PixelArt.Hex("B4B5A4");r.sharedMaterial=mat;life.Materials.Add(mat);
    var b=r.bounds;bool hz=b.size.x>b.size.z;var along=hz?Vector3.right:Vector3.forward;var normal=hz?Vector3.forward:Vector3.right;float length=hz?b.size.x:b.size.z;
    foreach(float side in new[]{-1f,1f}){
     var inward=normal*side;var p=new Vector3(b.center.x,0,b.center.z)+inward*((hz?b.size.z:b.size.x)/2+.033f);
     if(Mathf.Abs(p.x)>7.12f||Mathf.Abs(p.z)>14.92f)continue;
     if(InteriorId=="ferry2"&&new Rect(-7.15f,-14.95f,4.25f,6).Contains(new Vector2(p.x,p.z)))continue;
     a.Panel("MarineUpperPanel","AFB7AA",p+Vector3.up*1.75f,along,Vector3.up,length,1.0f);
     a.Panel("MarineLowerPanel","6E8C81",p+Vector3.up*.67f,along,Vector3.up,length,1.09f);
     a.Panel("MarineJointRail","D0CCB7",p+inward*.008f+Vector3.up*1.24f,along,Vector3.up,length,.055f);
     a.Panel("MarineTopRail","8F9D8B",p+inward*.01f+Vector3.up*2.21f,along,Vector3.up,length,.095f);
     for(float t=-length/2+.24f;t<length/2-.12f;t+=1.6f){
      var q=p+along*t+inward*.019f;
      a.Panel("BulkheadRib","98AA9C",q+Vector3.up*1.15f,along,Vector3.up,.065f,2.10f);
      foreach(float y in new[]{.28f,.95f,1.47f,2.04f})a.Panel("BulkheadRivets","C8C5AD",q+inward*.008f+Vector3.up*y,along,Vector3.up,.045f,.045f);
      a.Patch("BulkheadSaltWear","C3C6B0",q+along*.30f+inward*.028f+Vector3.up*.42f,along,Vector3.up,.22f,.17f,true);
      a.Stroke("BulkheadRustRun","947E61",q+along*.30f+inward*.034f+Vector3.up*.55f,q+along*.30f+inward*.034f+Vector3.up*.20f,.02f,inward,true);
     }
    }
   }
   // Perimeter caps have thickness and a visible top; surface details carry no new collision.
   foreach(float x in new[]{-7.2f,7.2f})a.Cube("PerimeterWallCap","BAC1AF",new Vector3(x,2.325f,0),new Vector3(.23f,.05f,30.1f));
   foreach(float z in new[]{-15f,15f})a.Cube("PerimeterWallCap","BAC1AF",new Vector3(0,2.325f,z),new Vector3(14.6f,.05f,.23f));
   a.Bake(go,life);
  }
 }
}
