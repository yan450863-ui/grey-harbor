using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
namespace GreyHarbor {
 public enum CargoVariant2714 {StandardClosed,RustWeathered,Faded,Patched,Refrigerated,SealedCargo}
 public sealed class CargoAppearance2714:MonoBehaviour {
  public CargoVariant2714 Variant;public int PaletteIndex;public string PaletteName,PaletteHex,ContainerNumber;
  public Vector3 PhysicalSize;public GameObject ArtRoot;public int DetailMeshCount,VertexCount;public float MaxSurfaceOffset=.03f;
 }
 public sealed class CargoArtResources2714:MonoBehaviour {
  public readonly List<Mesh> Meshes=new List<Mesh>();public Material AtlasMaterial;public Texture2D Atlas;
  void OnDestroy(){foreach(var mesh in Meshes)Release(mesh);Release(AtlasMaterial);Release(Atlas);}
  static void Release(Object value){if(!value)return;if(Application.isPlaying)Destroy(value);else DestroyImmediate(value);}
 }
 // Each box has one cosmetic mesh and one shared opaque, fog-aware atlas material.
 // Vertices use metre dimensions; the child cancels the primitive root's size scale.
 sealed class CargoShellMesh2714 {
  readonly List<Vector3> vertices=new List<Vector3>();readonly List<Vector2> uv=new List<Vector2>();readonly List<int> triangles=new List<int>();
  public void Face(Vector3 center,Vector3 right,Vector3 up,float width,float height,int color){
   right*=width*.5f;up*=height*.5f;int n=vertices.Count;
   vertices.Add(center-right-up);vertices.Add(center+right-up);vertices.Add(center+right+up);vertices.Add(center-right+up);
   var t=new Vector2((color%8+.5f)/8,(color/8+.5f)/4);for(int i=0;i<4;i++)uv.Add(t);
   triangles.Add(n);triangles.Add(n+1);triangles.Add(n+2);triangles.Add(n);triangles.Add(n+2);triangles.Add(n+3);
  }
  public void Cube(Vector3 p,Vector3 size,int color){
   Face(p+Vector3.forward*size.z*.5f,Vector3.right,Vector3.up,size.x,size.y,color);
   Face(p+Vector3.back*size.z*.5f,Vector3.left,Vector3.up,size.x,size.y,color);
   Face(p+Vector3.right*size.x*.5f,Vector3.back,Vector3.up,size.z,size.y,color);
   Face(p+Vector3.left*size.x*.5f,Vector3.forward,Vector3.up,size.z,size.y,color);
   Face(p+Vector3.up*size.y*.5f,Vector3.right,Vector3.back,size.x,size.z,color);
   Face(p+Vector3.down*size.y*.5f,Vector3.right,Vector3.forward,size.x,size.z,color);
  }
  public void Scar(Vector3 p,Vector3 right,Vector3 up,float width,float height,int color,int seed){
   var ring=new Vector3[9];for(int i=0;i<ring.Length;i++){float a=i*Mathf.PI*2/ring.Length,weight=.77f+((i*13+seed*7)%11)*.018f;ring[i]=p+right*Mathf.Cos(a)*width*.5f*weight+up*Mathf.Sin(a)*height*.5f*weight;}
   int n=vertices.Count;vertices.Add(p);var t=new Vector2((color%8+.5f)/8,(color/8+.5f)/4);uv.Add(t);
   for(int i=0;i<ring.Length;i++){vertices.Add(ring[i]);uv.Add(t);}
   for(int i=0;i<ring.Length;i++){triangles.Add(n);triangles.Add(n+i+1);triangles.Add(n+(i+1)%ring.Length+1);}
  }
  static readonly Dictionary<char,string[]> Glyphs=new Dictionary<char,string[]>{
   {'G',new[]{"01110","10000","10000","10111","10001","10001","01110"}},
   {'H',new[]{"10001","10001","10001","11111","10001","10001","10001"}},
   {'B',new[]{"11110","10001","10001","11110","10001","10001","11110"}},
   {'0',new[]{"01110","10001","10011","10101","11001","10001","01110"}},
   {'1',new[]{"00100","01100","00100","00100","00100","00100","01110"}},
   {'2',new[]{"01110","10001","00001","00010","00100","01000","11111"}},
   {'3',new[]{"11110","00001","00001","01110","00001","00001","11110"}},
   {'4',new[]{"00010","00110","01010","10010","11111","00010","00010"}},
   {'5',new[]{"11111","10000","10000","11110","00001","00001","11110"}},
   {'6',new[]{"01110","10000","10000","11110","10001","10001","01110"}},
   {'7',new[]{"11111","00001","00010","00100","01000","01000","01000"}},
   {'8',new[]{"01110","10001","10001","01110","10001","10001","01110"}},
   {'9',new[]{"01110","10001","10001","01111","00001","00001","01110"}}
  };
  public void Stamp(string text,Vector3 p,Vector3 right,Vector3 up,float pixel,int color,bool worn=false){
   float span=(text.Length*6-1)*pixel;
   for(int k=0;k<text.Length;k++){
    string[] rows;if(!Glyphs.TryGetValue(text[k],out rows))continue;
    for(int y=0;y<7;y++)for(int x=0;x<5;x++)if(rows[y][x]=='1'&&(!worn||(x+y*3+k*7)%13!=0))Face(p-right*((k*6+x)*pixel-span*.5f+pixel*.5f)+up*((3-y)*pixel),right,up,pixel*.83f,pixel*.83f,color);
   }
  }
  public Mesh Build(string name){var mesh=new Mesh{name=name};if(vertices.Count>65535)mesh.indexFormat=IndexFormat.UInt32;mesh.SetVertices(vertices);mesh.SetUVs(0,uv);mesh.SetTriangles(triangles,0);mesh.RecalculateNormals();mesh.RecalculateBounds();return mesh;}
 }
 public sealed partial class RaidMap {
  static readonly string[] CargoPaletteHex2714={"587181","8A5A4D","617B68","747F7D","9B916D","C5C3AF"};
  static readonly string[] CargoPaletteNames2714={"港蓝","氧化红","灰绿","石板灰","旧赭黄","象牙白"};
  public readonly List<CargoAppearance2714> CargoAppearances2714=new List<CargoAppearance2714>();
  static CargoArtResources2714 CargoAtlas2714(Transform parent){
   var owner=new GameObject("CargoContainerArtResources2714");owner.transform.SetParent(parent,false);var resources=owner.AddComponent<CargoArtResources2714>();
   var colors=new Color[32];for(int i=0;i<6;i++){colors[i]=PixelArt.Hex(CargoPaletteHex2714[i]);colors[i+6]=Color.Lerp(colors[i],PixelArt.Hex("34453F"),.24f);colors[i+12]=Color.Lerp(colors[i],PixelArt.Hex("CBC8B3"),.35f);}
   string[] details={"3E4E4D","919B8E","70513C","A77C54","D6D0AC","2F3D3A","899087","AA8B68","B1A576","D2D0C1","6D7975","9CA69C","966E5B","D3D3C2"};
   for(int i=0;i<details.Length;i++)colors[18+i]=PixelArt.Hex(details[i]);
   var atlas=new Texture2D(32,16,TextureFormat.RGBA32,false){name="CargoSurfaceSwatches2714",filterMode=FilterMode.Point,wrapMode=TextureWrapMode.Clamp};var pixels=new Color[32*16];
   for(int i=0;i<32;i++)for(int y=0;y<4;y++)for(int x=0;x<4;x++)pixels[((i/8)*4+y)*32+(i%8)*4+x]=colors[i];
   atlas.SetPixels(pixels);atlas.Apply(false,false);resources.Atlas=atlas;
   var material=new Material(GreyHarbor.FlashlightShaders2763.Load("HarborWorld")){name="CargoSurfaceWorld2714",color=Color.white};material.SetTexture("_MainTex",atlas);material.SetFloat("_UseStableGround",1);material.SetFloat("_SolidFog",1);material.SetFloat("_Detail",.035f);resources.AtlasMaterial=material;return resources;
  }
  public void DressCargoContainers2714(){
   if(!Root||CargoBoxes2712.Count==0||CargoAppearances2714.Count>0)return;
   var resources=CargoAtlas2714(Root);
   for(int i=0;i<CargoBoxes2712.Count;i++){
    var box=CargoBoxes2712[i];if(!box.View)continue;
    var variant=(CargoVariant2714)(i%6);int palette=(i*5+i/6)%6;if(variant==CargoVariant2714.Refrigerated)palette=5;
    var appearance=box.View.AddComponent<CargoAppearance2714>();appearance.Variant=variant;appearance.PaletteIndex=palette;appearance.PaletteName=CargoPaletteNames2714[palette];appearance.PaletteHex=CargoPaletteHex2714[palette];appearance.ContainerNumber="GHB"+(i+1).ToString("000");appearance.PhysicalSize=box.Size;
    // Never mutate root transform, collider or shared primitive mesh.
    var body=box.View.GetComponent<Renderer>();if(body)body.sharedMaterial=Mat(appearance.PaletteHex,true);
    var shape=new CargoShellMesh2714();CargoShellBase2714(shape,box.Size.x,palette,appearance.ContainerNumber,variant);
    CargoShellState2714(shape,box.Size.x,palette,variant,i);
    var child=new GameObject("CargoSurfaceDetail2714");child.transform.SetParent(box.View.transform,false);var scale=box.View.transform.localScale;child.transform.localScale=new Vector3(1/scale.x,1/scale.y,1/scale.z);
    var mesh=shape.Build("CargoSurface_"+appearance.ContainerNumber);child.AddComponent<MeshFilter>().sharedMesh=mesh;var renderer=child.AddComponent<MeshRenderer>();renderer.sharedMaterial=resources.AtlasMaterial;renderer.shadowCastingMode=ShadowCastingMode.On;renderer.receiveShadows=true;
    appearance.ArtRoot=child;appearance.DetailMeshCount=1;appearance.VertexCount=mesh.vertexCount;resources.Meshes.Add(mesh);CargoAppearances2714.Add(appearance);
   }
  }
  static void CargoShellBase2714(CargoShellMesh2714 art,float length,int palette,string number,CargoVariant2714 variant){
   float half=length*.5f;
   foreach(float side in new[]{-1f,1f}){
    var right=Vector3.right*side;var normal=Vector3.forward*side;
    if(variant==CargoVariant2714.Refrigerated){
     for(float y=-1.06f;y<1.1f;y+=.19f)art.Cube(new Vector3(0,y,side*1.310f),new Vector3(length-.25f,.047f,.020f),17);
    }else{
     int count=Mathf.FloorToInt((length-.4f)/.48f);
     for(int rib=0;rib<count;rib++){float x=Mathf.Lerp(-half+.24f,half-.24f,(rib+.5f)/count);art.Cube(new Vector3(x,0,side*1.310f),new Vector3(.075f,2.35f,.020f),palette+6);}
    }
    foreach(float y in new[]{-1.24f,1.24f})art.Cube(new Vector3(0,y,side*1.307f),new Vector3(length-.12f,.085f,.026f),18);
    art.Face(new Vector3(-length*.10f,.60f,side*1.325f),right,Vector3.up,length<7?2.0f:2.5f,.54f,palette+6);
    art.Stamp(number,new Vector3(-length*.10f,.60f,side*1.327f),right,Vector3.up,length<7?.043f:.054f,22,variant==CargoVariant2714.Faded);
    art.Stamp(length<7?"20":"40",new Vector3(half-.64f,-.73f,side*1.327f),right,Vector3.up,.042f,22);
    // A geometric port-company chevron stays in the same fog-aware atlas mesh.
    art.Face(new Vector3(-half+.60f,.28f,side*1.326f),right,Vector3.up,.36f,.29f,19);
    art.Face(new Vector3(-half+.60f,.28f,side*1.328f),right,Vector3.up,.20f,.12f,18);
   }
   // Roof raised seams are at most .024m above the unchanged physical shell.
   for(float x=-half+.28f;x<half-.2f;x+=.54f)art.Cube(new Vector3(x,1.310f,0),new Vector3(.075f,.020f,2.38f),palette+6);
   foreach(float x in new[]{-half+.055f,half-.055f})foreach(float y in new[]{-1.245f,1.245f})foreach(float z in new[]{-1.245f,1.245f})art.Cube(new Vector3(x,y,z),new Vector3(.15f,.15f,.15f),19);
   foreach(float end in new[]{-1f,1f}){
    foreach(float z in new[]{-1.24f,1.24f})art.Cube(new Vector3(end*(half+.006f),0,z),new Vector3(.026f,2.43f,.085f),18);
    foreach(float y in new[]{-1.24f,1.24f})art.Cube(new Vector3(end*(half+.006f),y,0),new Vector3(.026f,.085f,2.43f),18);
   }
   // One fixed sealed end and one pair of closed doors. Neither end is enterable.
   for(float z=-1.06f;z<1.1f;z+=.23f)art.Cube(new Vector3(-half-.009f,0,z),new Vector3(.020f,2.30f,.07f),palette+6);
   for(int door=0;door<2;door++){
    float z=door==0?-.59f:.59f;
    art.Face(new Vector3(half+.009f,0,z),Vector3.back,Vector3.up,1.13f,2.32f,variant==CargoVariant2714.Refrigerated?27:palette+12);
    foreach(float rib in new[]{-.35f,0,.35f})art.Cube(new Vector3(half+.015f,0,z+rib),new Vector3(.016f,2.20f,.043f),palette+6);
   }
   art.Face(new Vector3(half+.024f,0,0),Vector3.back,Vector3.up,.018f,2.35f,23);
   foreach(float z in new[]{-1.02f,-.44f,.44f,1.02f}){
    art.Cube(new Vector3(half+.022f,0,z),new Vector3(.012f,2.19f,.036f),19);
    art.Cube(new Vector3(half+.022f,-.30f,z+.08f),new Vector3(.014f,.045f,.20f),18);
   }
   foreach(float z in new[]{-1.15f,1.15f})foreach(float y in new[]{-.85f,0,.85f})art.Cube(new Vector3(half+.016f,y,z),new Vector3(.024f,.095f,.10f),19);
  }
  static void CargoShellState2714(CargoShellMesh2714 art,float length,int palette,CargoVariant2714 variant,int seed){
   float half=length*.5f;
   if(variant==CargoVariant2714.RustWeathered){
    foreach(float side in new[]{-1f,1f})for(int n=0;n<9;n++){
     float x=Mathf.Lerp(-half+.48f,half-.48f,((n*7+seed)%23)/22f),y=-1.02f+((n*11+seed)%17)*.10f;
     var p=new Vector3(x,y,side*1.325f);var right=Vector3.right*side;
     art.Scar(p,right,Vector3.up,.32f+(n%3)*.23f,.18f+(n%4)*.11f,20,n+seed);
     art.Scar(p+Vector3.forward*side*.001f,right,Vector3.up,.18f+(n%3)*.14f,.12f+(n%4)*.065f,21,n+3);
    }
    for(int i=0;i<5;i++)art.Scar(new Vector3(-half+.6f+i*(length-1.2f)/5,1.324f,.6f-i*.27f),Vector3.right,Vector3.back,.9f,.34f,20,seed+i);
   }else if(variant==CargoVariant2714.Faded){
   foreach(float side in new[]{-1f,1f}){
     var right=Vector3.right*side;
     float edgeMargin=Mathf.Max(.9f,length*.12f+.08f);
     for(int i=0;i<4;i++)art.Scar(new Vector3(Mathf.Lerp(-half+edgeMargin,half-edgeMargin,i/3f),-.25f+(i%2)*.30f,side*1.324f),right,Vector3.up,length*.22f,.75f,palette+12,seed+i);
     art.Stamp("GHB",new Vector3(0,-.25f,side*1.327f),right,Vector3.up,length<7?.13f:.22f,19,true);
    }
   }else if(variant==CargoVariant2714.Patched){
    foreach(float side in new[]{-1f,1f})foreach(float x in new[]{-length*.24f,length*.25f}){
     var p=new Vector3(x,-.28f,side*1.316f);art.Cube(p,new Vector3(length<7?.94f:1.55f,.86f,.015f),24);
     foreach(float bx in new[]{-1f,1f})foreach(float by in new[]{-1f,1f})art.Cube(p+new Vector3(bx*(length<7?.39f:.69f),by*.34f,side*.010f),new Vector3(.04f,.04f,.008f),18);
    }
   }else if(variant==CargoVariant2714.Refrigerated){
    art.Cube(new Vector3(-half-.012f,.05f,0),new Vector3(.018f,1.85f,1.82f),27);
    for(int row=0;row<12;row++)art.Face(new Vector3(-half-.025f,.73f-row*.092f,-.23f),Vector3.forward,Vector3.up,1.14f,.043f,28);
    art.Face(new Vector3(-half-.026f,-.61f,.37f),Vector3.forward,Vector3.up,.50f,.27f,18);
    art.Face(new Vector3(-half-.027f,-.61f,.37f),Vector3.forward,Vector3.up,.23f,.10f,2);
    foreach(float side in new[]{-1f,1f})art.Face(new Vector3(half-.66f,.88f,side*1.326f),Vector3.right*side,Vector3.up,.65f,.14f,28);
   }else if(variant==CargoVariant2714.SealedCargo){
    art.Cube(new Vector3(half+.022f,-.38f,.29f),new Vector3(.014f,.34f,.065f),25);
    art.Face(new Vector3(half+.029f,-.61f,.29f),Vector3.back,Vector3.up,.20f,.16f,30);
    foreach(float side in new[]{-1f,1f}){
     var p=new Vector3(half-.92f,-.37f,side*1.326f);art.Face(p,Vector3.right*side,Vector3.up,.52f,.43f,26);
     art.Face(p+Vector3.forward*side*.001f,Vector3.right*side,Vector3.up,.35f,.25f,18);
    }
   }else{
    foreach(float side in new[]{-1f,1f})art.Face(new Vector3(half-.76f,-.15f,side*1.326f),Vector3.right*side,Vector3.up,.40f,.20f,29);
   }
  }
 }
}

