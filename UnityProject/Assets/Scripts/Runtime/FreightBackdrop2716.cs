using UnityEngine;
using UnityEngine.Rendering;
namespace GreyHarbor {
 public sealed partial class RaidMap {
  public GameObject FreightExternalRoad2716;public Texture2D FreightExternalBackgroundTexture2716;
  public static readonly Rect FreightExternalBackgroundBounds2716=new Rect(-91,60,17.04f,46);
  void FreightExternalBackdropBuild2716(){
   if(FreightExternalRoad2716)return;
   // Outside-world scenery only: never add this ground to HarborSurfaces, Bounds,
   // path records, physics, or exploration. The original x4.8 stop still blocks it.
   var r=FreightExternalBackgroundBounds2716;const int w=128,h=256;var pixels=new Color32[w*h];
   Color land=PixelArt.Hex("A0A7A4"),asphalt=PixelArt.Hex("667077"),dust=PixelArt.Hex("899185");
   for(int z=0;z<h;z++)for(int x=0;x<w;x++){
    float px=r.xMin+(x+.5f)*r.width/w,pz=r.yMin+(z+.5f)*r.height/h;
    float noise=Mathf.PerlinNoise((px+17)*.21f,(pz-43)*.21f)-.5f;
    var ground=land;ground.r+=noise*.045f;ground.g+=noise*.045f;ground.b+=noise*.045f;
    float edgeDust=(1-Mathf.SmoothStep(0,1,Mathf.InverseLerp(-77,-74.2f,px)))*Mathf.Clamp01(Mathf.PerlinNoise(px*.14f+31,pz*.14f-27)*1.5f);
    ground=Color.Lerp(ground,PixelArt.Hex("9D825E"),edgeDust*.74f);
    float road=1-Mathf.SmoothStep(0,1,Mathf.InverseLerp(3.83f,4.65f,Mathf.Abs(pz-88)));
    var paving=asphalt;float grain=(Mathf.PerlinNoise(px*1.8f+8,pz*1.8f+3)-.5f)*.04f;paving.r+=grain;paving.g+=grain;paving.b+=grain;
    float weather=Mathf.PerlinNoise(px*.38f+13,pz*.38f-19);paving=Color.Lerp(paving,dust,Mathf.Max(0,weather-.61f)*.43f);
    float rub=1-Mathf.SmoothStep(0,1,Mathf.InverseLerp(.09f,.21f,Mathf.Abs(Mathf.Abs(pz-88)-1.08f)));
    paving=Color.Lerp(paving,PixelArt.Hex("454C49"),rub*.12f);
    // A short dusty threshold matches the existing inner apron under the gate.
    road*=1-Mathf.SmoothStep(0,1,Mathf.InverseLerp(-74.9f,-73.96f,px));
    pixels[z*w+x]=Color.Lerp(ground,paving,road);
   }
   var texture=new Texture2D(w,h,TextureFormat.RGBA32,true){name="FreightOutsideRoadBackground2716",filterMode=FilterMode.Trilinear,wrapMode=TextureWrapMode.Clamp,anisoLevel=8};texture.SetPixels32(pixels);texture.Apply(true,false);FreightExternalBackgroundTexture2716=texture;
   var g=new GameObject("FreightOutsideRoadBackground2716");g.transform.SetParent(FreightExteriorRoot2716,false);FreightExternalRoad2716=g;g.AddComponent<StableGroundResources137>().Texture=texture;
   var resources=g.AddComponent<WorkshopArtResources>();var mesh=new Mesh{name="FreightNonTraversableBackgroundPlane2716"};
   // .04m is below the accepted .05m stable apron, avoiding competition where
   // the background meets the perimeter by four centimetres in plan.
   mesh.vertices=new[]{new Vector3(r.xMin,.04f,r.yMin),new Vector3(r.xMin,.04f,r.yMax),new Vector3(r.xMax,.04f,r.yMax),new Vector3(r.xMax,.04f,r.yMin)};
   mesh.uv=new[]{Vector2.zero,Vector2.up,Vector2.one,Vector2.right};mesh.triangles=new[]{0,1,2,0,2,3};mesh.RecalculateNormals();mesh.RecalculateBounds();g.AddComponent<MeshFilter>().sharedMesh=mesh;
   var material=new Material(GreyHarbor.FlashlightShaders2763.Load("HarborWorld"));material.color=Color.white;material.SetTexture("_MainTex",texture);material.SetFloat("_UseStableGround",1);material.SetFloat("_SolidFog",0);material.SetFloat("_Detail",0);
   var renderer=g.AddComponent<MeshRenderer>();renderer.sharedMaterial=material;renderer.shadowCastingMode=ShadowCastingMode.Off;renderer.receiveShadows=true;resources.Meshes.Add(mesh);resources.Materials.Add(material);
  }
 }
}
