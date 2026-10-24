using UnityEngine;
using UnityEngine.Rendering;
using System.Collections.Generic;

namespace GreyHarbor {
 public sealed partial class RaidMap {
  public Texture2D UnloadingFloorTexture2725;
  public MeshRenderer UnloadingFloorRenderer2725;
  public Color UnloadingFloorMeanColor2725;
  public int UnloadingFloorColors2725;
  void BuildUnloadingFloor2725(GameObject supportingFloor,bool warehouse,float width,float depth){
   var g=new GameObject("UnloadingFinishedFloor2725");g.transform.SetParent(Root,false);
   var owner=g.AddComponent<UnloadingFloor2725>();
   owner.Texture=UnloadingFloor2725.Paint(warehouse,width,depth,out UnloadingFloorMeanColor2725,out UnloadingFloorColors2725);
   owner.Mesh=new Mesh{name="UnloadingFloorSurface2725"};
   // One upward-facing surface just above the supporting box; no extra movement colliders.
   owner.Mesh.vertices=new[]{new Vector3(-width/2,-.009f,-depth/2),new Vector3(-width/2,-.009f,depth/2),new Vector3(width/2,-.009f,depth/2),new Vector3(width/2,-.009f,-depth/2)};
   owner.Mesh.uv=new[]{Vector2.zero,Vector2.up,Vector2.one,Vector2.right};
   owner.Mesh.triangles=new[]{0,1,2,0,2,3};owner.Mesh.RecalculateNormals();owner.Mesh.RecalculateBounds();
   g.AddComponent<MeshFilter>().sharedMesh=owner.Mesh;
   owner.Material=new Material(GreyHarbor.FlashlightShaders2763.Load("HarborWorld")){name="UnloadingFloorFogAware2725",color=Color.white};
   owner.Material.mainTexture=owner.Texture;owner.Material.SetFloat("_UseStableGround",1);owner.Material.SetFloat("_SolidFog",0);owner.Material.SetFloat("_Detail",0);
   UnloadingFloorRenderer2725=g.AddComponent<MeshRenderer>();UnloadingFloorRenderer2725.sharedMaterial=owner.Material;
   UnloadingFloorRenderer2725.shadowCastingMode=ShadowCastingMode.Off;UnloadingFloorRenderer2725.receiveShadows=true;
   supportingFloor.GetComponent<MeshRenderer>().enabled=false;
   UnloadingFloorTexture2725=owner.Texture;
  }
 }

 // These three GPU resources share the lifetime of their interior, including cached visits.
 public sealed class UnloadingFloor2725:MonoBehaviour {
  public Texture2D Texture;
  public Material Material;
  public Mesh Mesh;
  void OnDestroy(){Release(Texture);Release(Material);Release(Mesh);}
  static void Release(Object obj){if(!obj)return;if(Application.isPlaying)Destroy(obj);else DestroyImmediate(obj);}
  static float Hash(int x,int z){unchecked{uint n=(uint)(x*73856093)^(uint)(z*19349663)^2725u;n^=n>>13;n*=1274126177u;return(n&65535)/65535f;}}
  static float Noise(float x,float z){int ix=Mathf.FloorToInt(x),iz=Mathf.FloorToInt(z);float u=Mathf.SmoothStep(0,1,x-ix),v=Mathf.SmoothStep(0,1,z-iz);return Mathf.Lerp(Mathf.Lerp(Hash(ix,iz),Hash(ix+1,iz),u),Mathf.Lerp(Hash(ix,iz+1),Hash(ix+1,iz+1),u),v);}
  static float Segment(Vector2 p,Vector2 a,Vector2 b){Vector2 q=b-a;return Vector2.Distance(p,a+q*Mathf.Clamp01(Vector2.Dot(p-a,q)/q.sqrMagnitude));}
  public static Texture2D Paint(bool warehouse,float width,float depth,out Color mean,out int unique){
   int w=warehouse?704:384,h=warehouse?448:288;
   var pixels=new Color32[w*h];var colors=new HashSet<int>();Vector3 total=Vector3.zero;
   Color concrete=PixelArt.Hex("777E72"),tile=PixelArt.Hex("9B9D88"),grout=PixelArt.Hex("626D63"),oil=PixelArt.Hex("46564B"),paint=PixelArt.Hex("A59C66");
   Vector2[] stainCenters=warehouse?new[]{new Vector2(-7,-2.7f),new Vector2(7,2.6f),new Vector2(6,-3),new Vector2(-8.5f,5),new Vector2(2,4.8f)}:new[]{new Vector2(2.9f,-1.6f),new Vector2(-2.8f,1.8f)};
   Vector2[] cracks=warehouse?new[]{new Vector2(-10,4),new Vector2(-7.5f,3.65f),new Vector2(-6.5f,4.1f),new Vector2(-4.8f,3.6f),new Vector2(4.6f,-5.6f),new Vector2(5.3f,-4.2f),new Vector2(6.9f,-3.8f),new Vector2(7.8f,-2.4f)}:new[]{new Vector2(1.8f,2.7f),new Vector2(2.2f,1.7f),new Vector2(3,1.3f)};
   for(int iz=0;iz<h;iz++)for(int ix=0;ix<w;ix++){
    float x=(ix+.5f)/w*width-width/2,z=(iz+.5f)/h*depth-depth/2;Vector2 p=new Vector2(x,z);
    bool tiled=!warehouse&&x<1.03f;
    float mottling=Noise(x*.7f+17,z*.7f+4)-.5f;
    float fine=Hash(ix,iz)-.5f;
    Color c=(tiled?tile:concrete)*(1+mottling*.14f+fine*.045f);
    if(tiled){
     float tx=Mathf.Repeat(x+4,.6f),tz=Mathf.Repeat(z+3,.6f);
     // Small, aged tiles: colour varies slightly, without a chessboard pattern.
     c*=.96f+.08f*Hash(Mathf.FloorToInt((x+4)/.6f),Mathf.FloorToInt((z+3)/.6f));
     if(tx<.012f||tz<.012f)c=Color.Lerp(c,grout,.68f);
    }
    foreach(var center in stainCenters){Vector2 delta=p-center;delta.y*=1.5f;float edge=delta.magnitude+(.5f-Noise(x*5,z*5))*.22f;float stain=(1-Mathf.SmoothStep(.18f,warehouse?1.15f:.65f,edge))*.30f;c=Color.Lerp(c,oil,stain);}
    if(!tiled){for(int k=0;k<cracks.Length-1;k++){if(warehouse&&k==3)continue;float d=Segment(p,cracks[k],cracks[k+1]);if(d<.022f)c=Color.Lerp(c,grout,(1-d/.022f)*.47f);}}
    if(warehouse){
     // Two local staging rectangles and subtle forklift tyre wear, never a full-floor grid.
     float ax=Mathf.Abs(x),az=Mathf.Abs(z);
     bool stripe=(Mathf.Abs(ax-4.55f)<.04f&&az<4.6f)||(Mathf.Abs(az-4.6f)<.04f&&ax>4.5f&&ax<9.5f);
     if(stripe&&Noise(x*9+41,z*9+8)>.24f)c=Color.Lerp(c,paint,.69f);
     float track=Mathf.Min(Mathf.Abs(x-1.7f),Mathf.Abs(x-2.15f));
     if(track<.055f&&z>-5.8f&&z<3.2f)c*=.91f+.05f*Noise(x*12,z*10);
    }
    Color32 cc=c;cc.a=255;pixels[iz*w+ix]=cc;total+=new Vector3(cc.r,cc.g,cc.b);colors.Add((cc.r<<16)|(cc.g<<8)|cc.b);
   }
   mean=new Color(total.x/(pixels.Length*255f),total.y/(pixels.Length*255f),total.z/(pixels.Length*255f),1);unique=colors.Count;
   var result=new Texture2D(w,h,TextureFormat.RGBA32,true){name=warehouse?"UnloadingWarehouseConcrete2725":"UnloadingDutyTileAndConcrete2725",filterMode=FilterMode.Trilinear,wrapMode=TextureWrapMode.Clamp,anisoLevel=4};
   result.SetPixels32(pixels);result.Apply(true,false);return result;
  }
 }
}
