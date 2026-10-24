using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
namespace GreyHarbor {
 public sealed class UndergroundPropInfo2754:MonoBehaviour {public string Kind;public Vector2 Footprint;}
 public static class UndergroundProps2754 {
  public static readonly string[] Kinds={"pipe-large","pipe-small","valve","pipe-support","pump","electric-cabinet","cable-reel","workbench","tool-rack","vent-box","safety-barrier","drain-channel","service-cart","ladder","spare-parts","broken-fan","hose-coil","pressure-gauge","copper-junction","utility-light"};
  public static Vector2 Footprint(string kind){switch(kind){case "pipe-large":return new Vector2(2.6f,.48f);case "pipe-small":return new Vector2(2.1f,.26f);case "valve":return new Vector2(.8f,.65f);case "pipe-support":return new Vector2(.8f,.52f);case "pump":return new Vector2(1.55f,.86f);case "electric-cabinet":return new Vector2(.86f,.50f);case "cable-reel":return new Vector2(.9f,.9f);case "workbench":return new Vector2(1.7f,.8f);case "tool-rack":return new Vector2(1.05f,.42f);case "vent-box":return new Vector2(1.15f,.55f);case "safety-barrier":return new Vector2(1.7f,.50f);case "drain-channel":return new Vector2(2.3f,.50f);case "service-cart":return new Vector2(.95f,.70f);case "ladder":return new Vector2(.63f,.32f);case "spare-parts":return new Vector2(1.0f,.75f);case "broken-fan":return new Vector2(.75f,.55f);case "hose-coil":return new Vector2(.85f,.80f);case "pressure-gauge":return new Vector2(.40f,.22f);case "copper-junction":return new Vector2(.9f,.45f);case "utility-light":return new Vector2(1.25f,.22f);default:throw new System.ArgumentException("Unknown underground prop: "+kind);}}
  public static GameObject Build(RaidMap map,string kind,Vector3 position,float yaw=0){
   var view=new GameObject("UndergroundProp2754_"+kind);view.transform.SetParent(map.Root,false);var info=view.AddComponent<UndergroundPropInfo2754>();info.Kind=kind;info.Footprint=Footprint(kind);
   var kit=new Kit(map,view);kit.Make(kind);kit.Bake();view.transform.position=position;view.transform.rotation=Quaternion.Euler(0,yaw,0);return view;
  }
  sealed class Kit {
   readonly GameObject view;readonly WorkshopWastelandArt art;
   const string Green="5B786D",Dark="344A42",Steel="8C9A87",Rust="99674C",Copper="AD925D",Yellow="C6B064",Black="263830";
   public Kit(RaidMap map,GameObject root){view=root;art=new WorkshopWastelandArt(map);}
   void Cube(string color,Vector3 p,Vector3 size){art.Cube("Underground",color,p,size);}
   void Pipe(string color,Vector3 a,Vector3 b,float radius){art.Tube("Underground",color,a,b,radius);}
   void Panel(string color,Vector3 p,float width,float height){art.Panel("Underground",color,p,Vector3.right,Vector3.up,width,height);}
   void Body(string color,Vector3 p,Vector3 size,ObstacleKind kind=ObstacleKind.High){Cube(color,p,size);var body=new GameObject("BodyCollider2754");body.transform.SetParent(view.transform,false);body.transform.localPosition=p;body.AddComponent<BoxCollider>().size=size;SceneObstacle.Apply(body,kind);}
   void Ring(string color,Vector3 p,float radius,Vector3 right,Vector3 up,float thickness){for(int i=0;i<12;i++){float a=i*Mathf.PI/6,b=(i+1)*Mathf.PI/6;Pipe(color,p+(right*Mathf.Cos(a)+up*Mathf.Sin(a))*radius,p+(right*Mathf.Cos(b)+up*Mathf.Sin(b))*radius,thickness);}}
   void Leg(float x,float z,float top){Body(Dark,new Vector3(x,top/2,z),new Vector3(.075f,top,.075f));}
   void Warning(Vector3 p,float w,float h){Panel(Yellow,p,w,h);for(int i=0;i<5;i++)art.Stroke("Underground",Dark,p+new Vector3(-w*.44f+i*w*.20f,-h*.4f,-.005f),p+new Vector3(-w*.33f+i*w*.20f,h*.4f,-.005f),.035f,Vector3.back,true);}
   public void Make(string kind){switch(kind){
    case "pipe-large":
     Pipe(Green,new Vector3(-1.3f,1.04f,0),new Vector3(1.3f,1.04f,0),.21f);
     foreach(float x in new[]{-.85f,.85f}){Pipe(Rust,new Vector3(x-.06f,1.04f,0),new Vector3(x+.06f,1.04f,0),.24f);Cube(Steel,new Vector3(x,.72f,0),new Vector3(.12f,.23f,.38f));}break;
    case "pipe-small":
     for(int i=0;i<2;i++)Pipe(i==0?Copper:Dark,new Vector3(-1.05f,.8f+i*.22f,0),new Vector3(1.05f,.8f+i*.22f,0),i==0?.055f:.034f);
     foreach(float x in new[]{-.65f,.65f})Cube(Steel,new Vector3(x,.92f,.03f),new Vector3(.065f,.43f,.15f));break;
    case "valve":
     Body(Green,new Vector3(0,.52f,0),new Vector3(.36f,.46f,.36f));Pipe(Rust,new Vector3(-.4f,.50f,0),new Vector3(.4f,.5f,0),.14f);Pipe(Steel,new Vector3(0,.66f,0),new Vector3(0,.97f,0),.043f);Ring(Rust,new Vector3(0,1.01f,0),.28f,Vector3.right,Vector3.forward,.035f);for(int i=0;i<3;i++){float a=i*Mathf.PI/3;Pipe(Steel,new Vector3(0,1.01f,0),new Vector3(Mathf.Cos(a)*.26f,1.01f,Mathf.Sin(a)*.26f),.023f);}break;
    case "pipe-support":
     foreach(float x in new[]{-.3f,.3f})Body(Rust,new Vector3(x,.42f,0),new Vector3(.11f,.84f,.16f));Cube(Steel,new Vector3(0,.86f,0),new Vector3(.8f,.09f,.50f));Cube(Dark,new Vector3(0,.12f,0),new Vector3(.72f,.12f,.46f));break;
    case "pump":
     Body(Dark,new Vector3(0,.13f,0),new Vector3(1.5f,.23f,.78f),ObstacleKind.Low);Body(Green,new Vector3(-.23f,.49f,0),new Vector3(.65f,.52f,.66f));Pipe(Rust,new Vector3(.08f,.49f,0),new Vector3(.69f,.49f,0),.29f);for(int i=0;i<5;i++)Cube(Steel,new Vector3(.2f+i*.10f,.76f,0),new Vector3(.035f,.07f,.38f));Pipe(Copper,new Vector3(-.45f,.61f,0),new Vector3(-.45f,1.05f,0),.09f);Ring(Rust,new Vector3(-.45f,1.02f,0),.13f,Vector3.right,Vector3.forward,.035f);Warning(new Vector3(-.25f,.56f,-.338f),.20f,.16f);break;
    case "electric-cabinet":
     Body(Green,new Vector3(0,.86f,0),new Vector3(.82f,1.7f,.48f));Panel(Steel,new Vector3(0,.90f,-.246f),.68f,1.42f);Cube(Dark,new Vector3(.25f,.87f,-.26f),new Vector3(.035f,.19f,.045f));Warning(new Vector3(0,1.29f,-.262f),.33f,.23f);for(int i=0;i<5;i++)Panel(Dark,new Vector3(-.02f,.37f+i*.06f,-.27f),.40f,.02f);break;
    case "cable-reel":
     Pipe(Black,new Vector3(0,.41f,-.30f),new Vector3(0,.41f,.30f),.31f);foreach(float z in new[]{-.32f,.32f}){Ring(Copper,new Vector3(0,.41f,z),.42f,Vector3.right,Vector3.up,.07f);for(int i=0;i<3;i++){float a=i*Mathf.PI/3;Pipe(Rust,new Vector3(-Mathf.Cos(a)*.37f,.41f-Mathf.Sin(a)*.37f,z),new Vector3(Mathf.Cos(a)*.37f,.41f+Mathf.Sin(a)*.37f,z),.055f);}}Body(Dark,new Vector3(0,.36f,0),new Vector3(.48f,.57f,.52f),ObstacleKind.Low);Pipe(Black,new Vector3(.21f,.12f,.27f),new Vector3(.42f,.06f,.42f),.025f);break;
    case "workbench":
     foreach(float x in new[]{-.72f,.72f})foreach(float z in new[]{-.28f,.28f})Leg(x,z,.85f);Body(Copper,new Vector3(0,.89f,0),new Vector3(1.7f,.12f,.75f));Cube(Green,new Vector3(-.45f,1.02f,0),new Vector3(.35f,.16f,.24f));Pipe(Steel,new Vector3(-.58f,1.11f,-.09f),new Vector3(-.29f,1.11f,-.09f),.03f);Cube(Dark,new Vector3(.31f,.97f,.05f),new Vector3(.40f,.05f,.27f));Pipe(Steel,new Vector3(.12f,1.03f,-.20f),new Vector3(.52f,1.03f,-.13f),.025f);break;
    case "tool-rack":
     foreach(float x in new[]{-.47f,.47f})Body(Dark,new Vector3(x,.9f,0),new Vector3(.065f,1.8f,.30f));Panel(Green,new Vector3(0,1.13f,.10f),1.02f,1.22f);for(int i=0;i<5;i++){float x=-.38f+i*.18f;Pipe(i%2==0?Steel:Rust,new Vector3(x,1.30f,0),new Vector3(x,1.63f,0),.035f);Ring(Steel,new Vector3(x,1.65f,0),.055f,Vector3.right,Vector3.up,.017f);}Body(Rust,new Vector3(0,.30f,0),new Vector3(.90f,.16f,.40f),ObstacleKind.Low);break;
    case "vent-box":
     Body(Green,new Vector3(0,.73f,0),new Vector3(1.08f,.88f,.50f));Panel(Black,new Vector3(0,.78f,-.258f),.89f,.61f);for(int i=0;i<7;i++)Panel(Steel,new Vector3(0,.52f+i*.08f,-.27f),.84f,.036f);Cube(Rust,new Vector3(0,1.20f,.02f),new Vector3(.65f,.13f,.36f));break;
    case "safety-barrier":
     foreach(float x in new[]{-.72f,.72f}){Body(Rust,new Vector3(x,.5f,0),new Vector3(.065f,1,.08f),ObstacleKind.Low);Body(Dark,new Vector3(x,.05f,0),new Vector3(.38f,.09f,.47f),ObstacleKind.Low);}Cube(Yellow,new Vector3(0,.86f,0),new Vector3(1.7f,.20f,.07f));for(int i=0;i<8;i++)Panel(Dark,new Vector3(-.72f+i*.2f,.86f,-.042f),.07f,.20f);Pipe(Rust,new Vector3(-.72f,.42f,0),new Vector3(.72f,.42f,0),.024f);break;
    case "drain-channel":
     art.Floor("Underground",Black,0,0,2.3f,.48f,.014f);foreach(float z in new[]{-.25f,.25f})Cube(Steel,new Vector3(0,.034f,z),new Vector3(2.3f,.044f,.038f));for(int i=0;i<17;i++)Cube(i%4==0?Rust:Green,new Vector3(-1.08f+i*.135f,.052f,0),new Vector3(.045f,.033f,.44f));break;
    case "service-cart":
     Body(Green,new Vector3(0,.34f,0),new Vector3(.9f,.16f,.66f),ObstacleKind.Low);foreach(float x in new[]{-.37f,.37f})foreach(float z in new[]{-.25f,.25f})Pipe(Black,new Vector3(x-.035f,.13f,z),new Vector3(x+.035f,.13f,z),.13f);foreach(float z in new[]{-.28f,.28f})Pipe(Rust,new Vector3(-.4f,.42f,z),new Vector3(-.4f,.92f,z),.032f);Pipe(Rust,new Vector3(-.4f,.92f,-.28f),new Vector3(-.4f,.92f,.28f),.035f);Cube(Copper,new Vector3(.16f,.5f,0),new Vector3(.42f,.16f,.45f));break;
    case "ladder":
     foreach(float x in new[]{-.26f,.26f})Pipe(Rust,new Vector3(x,.03f,-.1f),new Vector3(x,2.15f,.1f),.033f);for(int i=0;i<8;i++)Pipe(Steel,new Vector3(-.26f,.20f+i*.25f,-.075f+i*.025f),new Vector3(.26f,.20f+i*.25f,-.075f+i*.025f),.025f);break;
    case "spare-parts":
     Body(Dark,new Vector3(0,.12f,0),new Vector3(.96f,.20f,.71f),ObstacleKind.Low);for(int i=0;i<3;i++){Pipe(i==1?Copper:Rust,new Vector3(-.35f,.33f+i*.18f,-.15f),new Vector3(.35f,.33f+i*.18f,-.15f),.10f);Ring(Steel,new Vector3(.25f,.28f+i*.12f,.19f),.13f,Vector3.right,Vector3.forward,.045f);}break;
    case "broken-fan":
     Body(Green,new Vector3(0,.18f,0),new Vector3(.68f,.22f,.50f),ObstacleKind.Low);Ring(Rust,new Vector3(0,.63f,0),.29f,Vector3.right,Vector3.up,.036f);for(int i=0;i<4;i++){float a=i*Mathf.PI/2+.2f;Pipe(Steel,new Vector3(0,.63f,0),new Vector3(Mathf.Cos(a)*.23f,.63f+Mathf.Sin(a)*.23f,i==2?.1f:0),.06f);}Pipe(Dark,new Vector3(0,.28f,0),new Vector3(0,.67f,0),.035f);break;
    case "hose-coil":
     for(int i=0;i<3;i++)Ring(i==1?Green:Black,new Vector3(0,.04f+i*.035f,0),.35f-i*.07f,Vector3.right,Vector3.forward,.023f);Pipe(Rust,new Vector3(.30f,.065f,.05f),new Vector3(.4f,.065f,.20f),.028f);break;
    case "pressure-gauge":
     Pipe(Copper,new Vector3(0,.70f,.025f),new Vector3(0,1.18f,.025f),.04f);Ring(Rust,new Vector3(0,1.24f,0),.15f,Vector3.right,Vector3.up,.035f);Panel(Steel,new Vector3(0,1.24f,-.01f),.23f,.23f);art.Stroke("Underground",Dark,new Vector3(0,1.24f,-.03f),new Vector3(.07f,1.31f,-.03f),.017f,Vector3.back,true);break;
    case "copper-junction":
     Pipe(Copper,new Vector3(-.44f,1.08f,0),new Vector3(.44f,1.08f,0),.075f);Pipe(Copper,new Vector3(0,.75f,0),new Vector3(0,1.42f,0),.075f);foreach(float x in new[]{-.36f,.36f})Pipe(Rust,new Vector3(x-.035f,1.08f,0),new Vector3(x+.035f,1.08f,0),.10f);Cube(Green,new Vector3(0,1.08f,.03f),new Vector3(.18f,.20f,.22f));break;
    case "utility-light":
     Cube(Dark,new Vector3(0,1.9f,0),new Vector3(1.22f,.17f,.19f));Panel(Yellow,new Vector3(0,1.9f,-.104f),1.02f,.085f);for(int i=0;i<6;i++)Panel(Steel,new Vector3(-.47f+i*.19f,1.9f,-.11f),.022f,.15f);break;
   }}
   public void Bake(){
    var life=view.AddComponent<WorkshopArtResources>();art.Bake(view,life);var renderers=view.GetComponentsInChildren<MeshRenderer>();var colors=new List<Color>();foreach(var r in renderers)if(!colors.Contains(r.sharedMaterial.color))colors.Add(r.sharedMaterial.color);
    var palette=new Texture2D(Mathf.Max(1,colors.Count),1,TextureFormat.RGBA32,false){name="UndergroundPalette2754",filterMode=FilterMode.Point,wrapMode=TextureWrapMode.Clamp};palette.SetPixels(colors.ToArray());palette.Apply(false,false);
    var vertices=new List<Vector3>();var triangles=new List<int>();var normals=new List<Vector3>();var uv=new List<Vector2>();
    foreach(var r in renderers){var f=r.GetComponent<MeshFilter>();var mesh=f.sharedMesh;int start=vertices.Count;var matrix=view.transform.worldToLocalMatrix*f.transform.localToWorldMatrix;int color=colors.IndexOf(r.sharedMaterial.color);foreach(var p in mesh.vertices){vertices.Add(matrix.MultiplyPoint3x4(p));uv.Add(new Vector2((color+.5f)/colors.Count,.5f));}foreach(var n in mesh.normals)normals.Add(matrix.MultiplyVector(n).normalized);foreach(int t in mesh.triangles)triangles.Add(start+t);r.enabled=false;Destroy(r.gameObject);}
    foreach(var mesh in life.Meshes)Destroy(mesh);foreach(var material in life.Materials)Destroy(material);life.Meshes.Clear();life.Materials.Clear();
    var combined=new Mesh{name="UndergroundCombined2754"};if(vertices.Count>65535)combined.indexFormat=IndexFormat.UInt32;combined.SetVertices(vertices);combined.SetTriangles(triangles,0);combined.SetNormals(normals);combined.SetUVs(0,uv);combined.RecalculateBounds();view.AddComponent<MeshFilter>().sharedMesh=combined;life.Meshes.Add(combined);
    var mat=new Material(GreyHarbor.FlashlightShaders2763.Load("HarborWorld")){name="UndergroundPaletteMaterial2754",color=Color.white};mat.SetTexture("_MainTex",palette);mat.SetFloat("_UseStableGround",1);mat.SetFloat("_SolidFog",view.GetComponent<UndergroundPropInfo2754>().Kind=="drain-channel"||view.GetComponent<UndergroundPropInfo2754>().Kind=="hose-coil"?0:1);mat.SetFloat("_Detail",.035f);life.Materials.Add(mat);var visible=view.AddComponent<MeshRenderer>();visible.sharedMaterial=mat;visible.shadowCastingMode=ShadowCastingMode.Off;visible.receiveShadows=true;view.AddComponent<StableGroundResources137>().Texture=palette;
   }
   static void Destroy(Object value){if(Application.isPlaying)Object.Destroy(value);else Object.DestroyImmediate(value);}
  }
 }
 public sealed partial class RaidMap {
  public bool UndergroundShowcase2754;
  public readonly List<GameObject> UndergroundShowcaseProps2754=new List<GameObject>();
  public void BuildShowcase2754(){
   UndergroundShowcase2754=IsInterior=true;InteriorId="undergroundshowcase2754";InteriorTitle="地下管网素材展示区";Bounds=new Rect(-14,-12,28,24);InitialRoom=Bounds;Root=new GameObject("UndergroundPropShowcase2754").transform;Spawn=new Vector3(0,0,-10);
   Box("UndergroundShowcaseFloor2754",new Vector3(0,-.08f,0),new Vector3(27,.16f,23),"727E74");
   for(int i=0;i<UndergroundProps2754.Kinds.Length;i++){int row=i/5,col=i%5;var p=new Vector3(-10+col*5,0,8-row*5);UndergroundShowcaseProps2754.Add(UndergroundProps2754.Build(this,UndergroundProps2754.Kinds[i],p));}
   Physics.SyncTransforms();
  }
 }
}
