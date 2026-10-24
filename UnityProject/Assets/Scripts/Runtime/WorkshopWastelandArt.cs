using UnityEngine;
using UnityEngine.Rendering;
using System.Collections.Generic;
namespace GreyHarbor {
 // Reusable, fog-aware surface dressing. All meshes are cosmetic and have no colliders.
 public sealed class WorkshopWastelandArt {
  readonly RaidMap map;readonly System.Random rng=new System.Random(114029);
  readonly Dictionary<string,Batch> batches=new Dictionary<string,Batch>();
  sealed class Batch {public readonly List<Vector3> V=new List<Vector3>();public readonly List<int> T=new List<int>();}
  public WorkshopWastelandArt(RaidMap m){map=m;}
  float R(float a,float b)=>a+(float)rng.NextDouble()*(b-a);
  void Tri(string group,string color,bool wall,Vector3 a,Vector3 b,Vector3 c){string key=group+"|"+color+"|"+(wall?"1":"0");if(!batches.TryGetValue(key,out var batch)){batch=new Batch();batches.Add(key,batch);}int n=batch.V.Count;batch.V.Add(a);batch.V.Add(b);batch.V.Add(c);batch.T.Add(n);batch.T.Add(n+1);batch.T.Add(n+2);}
  void Quad(string group,string color,bool wall,Vector3 a,Vector3 b,Vector3 c,Vector3 d){Tri(group,color,wall,a,b,c);Tri(group,color,wall,a,c,d);Tri(group,color,wall,c,b,a);Tri(group,color,wall,d,c,a);}
  internal void Panel(string group,string color,Vector3 p,Vector3 right,Vector3 up,float w,float h,bool wall=true){right*=w/2;up*=h/2;Quad(group,color,wall,p-right-up,p-right+up,p+right+up,p+right-up);}
  internal void Floor(string group,string color,float x,float z,float w,float d,float y=.04f){Panel(group,color,new Vector3(x,y,z),Vector3.right,Vector3.forward,w,d,false);}
  internal void Cube(string group,string color,Vector3 p,Vector3 s,bool wall=true){Vector3 x=Vector3.right*s.x/2,y=Vector3.up*s.y/2,z=Vector3.forward*s.z/2;Quad(group,color,wall,p-x-y-z,p-x+y-z,p+x+y-z,p+x-y-z);Quad(group,color,wall,p-x-y+z,p+x-y+z,p+x+y+z,p-x+y+z);Quad(group,color,wall,p-x-y-z,p-x-y+z,p-x+y+z,p-x+y-z);Quad(group,color,wall,p+x-y-z,p+x+y-z,p+x+y+z,p+x-y+z);Quad(group,color,wall,p-x+y-z,p-x+y+z,p+x+y+z,p+x+y-z);Quad(group,color,wall,p-x-y-z,p+x-y-z,p+x-y+z,p-x-y+z);}
  internal void Patch(string group,string color,Vector3 p,Vector3 right,Vector3 up,float w,float h,bool wall){int n=11;var ring=new Vector3[n];for(int i=0;i<n;i++){float angle=i*Mathf.PI*2/n;ring[i]=p+right*Mathf.Cos(angle)*w*.5f*R(.72f,1.14f)+up*Mathf.Sin(angle)*h*.5f*R(.72f,1.14f);}for(int i=0;i<n;i++){Tri(group,color,wall,p,ring[i],ring[(i+1)%n]);Tri(group,color,wall,p,ring[(i+1)%n],ring[i]);}}
  internal void Stroke(string group,string color,Vector3 a,Vector3 b,float width,Vector3 normal,bool wall){Vector3 side=Vector3.Cross(normal,(b-a).normalized)*width/2;Quad(group,color,wall,a-side,a+side,b+side,b-side);}
  internal void Tube(string group,string color,Vector3 a,Vector3 b,float radius){Vector3 forward=(b-a).normalized,right=Vector3.Cross(forward,Mathf.Abs(forward.y)>.9f?Vector3.forward:Vector3.up).normalized,up=Vector3.Cross(forward,right);int n=6;for(int i=0;i<n;i++){Vector3 v=(right*Mathf.Cos(i*Mathf.PI*2/n)+up*Mathf.Sin(i*Mathf.PI*2/n))*radius,w=(right*Mathf.Cos((i+1)*Mathf.PI*2/n)+up*Mathf.Sin((i+1)*Mathf.PI*2/n))*radius;Quad(group,color,true,a+v,b+v,b+w,a+w);}}
  internal void WallSkin(Vector3 origin,Vector3 along,Vector3 inward,float length,string paint="5F776C"){
   // The existing solid wall supplies all collision. Coatings protrude at most a few centimetres.
   Panel("WallPaint",paint,origin+Vector3.up*.69f,along,Vector3.up,length,1.1f);
   Panel("PaintBorder","364F49",origin+inward*.007f+Vector3.up*1.24f,along,Vector3.up,length,.055f);
   Panel("WallSkirt","46564E",origin+inward*.01f+Vector3.up*.13f,along,Vector3.up,length,.22f);
   for(int i=0;i<Mathf.CeilToInt(length*2);i++){float t=R(-length*.47f,length*.47f),height=R(.45f,2.02f);Vector3 p=origin+along*t+Vector3.up*height+inward*.018f;Patch("PeelingPlaster",i%3==0?"A09B80":"828575",p,along,Vector3.up,R(.16f,.52f),R(.12f,.40f),true);if(i%4==0)Stroke("DampStreaks","656D5E",p+inward*.004f,p+inward*.004f-Vector3.up*Mathf.Min(R(.2f,.55f),height-.16f),R(.025f,.09f),inward,true);}
   for(float t=-length*.44f;t<length*.44f;t+=2.5f){Cube("WallBrackets","5B665A",origin+along*t+inward*.06f+Vector3.up*2.09f,new Vector3(.08f,.17f,.08f));}
   Tube("ServicePipes","756A52",origin-along*length*.48f+inward*.09f+Vector3.up*2.08f,origin+along*length*.48f+inward*.09f+Vector3.up*2.08f,.035f);
   Tube("OldCable","343F38",origin-along*length*.48f+inward*.04f+Vector3.up*1.96f,origin+along*length*.48f+inward*.04f+Vector3.up*1.96f,.012f);
  }
  internal void BrickScar(Vector3 p,Vector3 along,Vector3 inward){Patch("BrokenPlaster","686A5A",p,along,Vector3.up,1.35f,.8f,true);for(int row=0;row<3;row++)for(int col=0;col<4;col++){if((row+col)%5==0)continue;Vector3 q=p+along*((col-1.5f)*.27f+(row%2)*.10f)+Vector3.up*((row-1)*.19f)+inward*.006f;Panel("ExposedMasonry",(row+col)%2==0?"85755E":"927E63",q,along,Vector3.up,.24f,.16f);}}
  internal void StripLight(Vector3 p,Vector3 along,Vector3 inward){Panel("LightHousing","3D5049",p,along,Vector3.up,1.1f,.18f);Panel("OldDiffuser","C1BB91",p+inward*.045f,along,Vector3.up,.89f,.085f);for(int i=0;i<5;i++)Panel("LightGuard","6A7567",p+inward*.05f+along*((i-2)*.19f),along,Vector3.up,.018f,.13f);}
  void FloorWear(){
   foreach(var p in new[]{new Vector3(-3.1f,.035f,-2.9f),new Vector3(3.5f,.035f,.0f),new Vector3(-2.1f,.035f,-5.5f)}){Patch("OilStains","424C40",p,Vector3.right,Vector3.forward,1.4f,.85f,false);Patch("OilStains","35433B",p+Vector3.up*.003f,Vector3.right,Vector3.forward,.75f,.4f,false);}
   // Broken ochre work-bay markings, never the saturated inventory rarity colours.
   for(int i=0;i<15;i++)if(i%4!=0)Floor("WornSafetyPaint","9D9268",-2.68f,-4.25f+i*.27f,.055f,.19f,.043f);
   for(int i=0;i<7;i++){float x=-.7f+i*.22f;Floor("DoorSill","9A9068",x,1.28f,.12f,.18f,.05f);}
   Floor("DrainBed","34433B",3.65f,3.65f,.30f,3.7f,.04f);for(float z=1.95f;z<5.5f;z+=.17f)Floor("DrainGrate","7D806C",3.65f,z,.28f,.048f,.051f);
   // Paper and aggregate stay flat and passable; density is concentrated along edges.
   for(int i=0;i<27;i++){float x=i<14?R(-4.22f,-3.8f):R(3.75f,4.18f),z=R(-6.5f,6.5f);Cube("SmallRubble",i%2==0?"99947B":"777864",new Vector3(x,.05f,z),new Vector3(R(.04f,.14f),R(.025f,.065f),R(.04f,.12f)),false);}
   for(int i=0;i<7;i++){float x=R(-.7f,1.4f),z=R(-6.7f,-5.5f);Floor("DiscardedPaper","ADA68A",x,z,.20f,.27f,.06f);for(int line=0;line<3;line++)Floor("PaperPrint","787A64",x,z-.06f+line*.045f,.13f,.011f,.061f);}
   // Cart wheel marks use short segments so they don't read as collision rails.
   for(int side=-1;side<=1;side+=2)for(int i=0;i<10;i++)Floor("WheelTracks","5B6658",2.1f+side*.28f,-1.3f+i*.22f,.07f,.14f,.042f);
  }
  void FurnitureWear(){
   // Existing tables, cabinet bodies and tools keep their silhouettes and interaction bounds.
   Cube("BenchFrame","32483E",new Vector3(-3.7f,.12f,-2.4f),new Vector3(.97f,.1f,2.64f));
   for(int i=0;i<7;i++){float z=-3.5f+i*.34f;Panel("BenchRust",i%2==0?"8E7154":"746650",new Vector3(-3.216f,R(.25f,.65f),z),Vector3.forward,Vector3.up,R(.10f,.21f),R(.06f,.18f));}
   for(int i=0;i<12;i++)Floor("BenchScars","706851",R(-4.13f,-3.28f),R(-3.6f,-1.16f),R(.07f,.18f),.018f,.926f);
   for(int i=0;i<6;i++)Cube("ShelfRust","826B50",new Vector3(-4.14f,.3f+i*.24f,5.25f),new Vector3(.074f,.06f,.074f));
   Panel("CartScuff","8C7B5A",new Vector3(3.6f,.45f,-.657f),Vector3.right,Vector3.up,.30f,.19f);
   // A coiled cable is laid flat beside the bench and remains walkable.
   for(int i=0;i<20;i++){float a=i*Mathf.PI*2/20,b=(i+1)*Mathf.PI*2/20;Stroke("CoiledCable","303F36",new Vector3(-3.3f+Mathf.Cos(a)*.32f,.056f,-4.5f+Mathf.Sin(a)*.25f),new Vector3(-3.3f+Mathf.Cos(b)*.32f,.056f,-4.5f+Mathf.Sin(b)*.25f),.035f,Vector3.up,false);}
  }
  void AlienCorner(){
   // Localised, dried infestation around a broken vent in the spare-parts room. No lights or particles leak through fog.
   Vector3 p=new Vector3(4.395f,1.48f,4.2f),along=Vector3.forward,inward=Vector3.left;
   Panel("BreachedVent","34473E",p,along,Vector3.up,1.13f,.72f);
   Panel("VentInterior","202E29",p+inward*.018f,along,Vector3.up,.96f,.55f);
   for(int i=0;i<7;i++){float z=3.78f+i*.14f;if(i==3||i==4)continue;Stroke("BentVentBars","7B8169",new Vector3(4.345f,1.24f,z),new Vector3(4.31f,1.72f,z+.025f),.045f,inward,true);}
   Patch("AlienScarring","554F3F",p+inward*.035f+Vector3.down*.41f,along,Vector3.up,1.30f,.62f,true);
   for(int branch=0;branch<7;branch++){Vector3 a=new Vector3(4.31f,1.2f,4.15f);for(int j=0;j<5;j++){Vector3 b=a+new Vector3(R(-.018f,.012f),R(-.20f,-.10f),(branch-3)*.07f+R(-.04f,.04f));Tube("AlienTendrils",branch%2==0?"343D32":"555A42",a,b,.024f-j*.003f);a=b;}}
   foreach(float z in new[]{3.88f,4.28f,4.62f}){Patch("AlienDryResidue","50533E",new Vector3(4.12f,.048f,z),Vector3.right,Vector3.forward,.45f,.55f,false);Cube("AlienMoltedShell","76775A",new Vector3(4.18f,.08f,z),new Vector3(.20f,.10f,.14f));}
   for(int i=0;i<3;i++)Stroke("ClawScars","39473C",new Vector3(4.37f,.83f,3.05f+i*.13f),new Vector3(4.37f,1.25f,3.29f+i*.13f),.025f,Vector3.left,true);
  }
  public void Build(){
   // Only this map receives the materials, preserving approved booth/main-house artwork.
   var resources=new GameObject("WorkshopWastelandArt_114");resources.transform.SetParent(map.Root,false);var lifetime=resources.AddComponent<WorkshopArtResources>();
   var skins=new Dictionary<Material,Material>();foreach(var r in map.Root.GetComponentsInChildren<MeshRenderer>()){var original=r.sharedMaterial;if(original==null||!original.HasProperty("_Detail"))continue;if(!skins.TryGetValue(original,out var skin)){skin=new Material(original);skin.name="WorkshopWeathered_"+original.name;skin.SetFloat("_Detail",.07f);skins.Add(original,skin);lifetime.Materials.Add(skin);}r.sharedMaterial=skin;}
   // Opaque walls stay solid, while the added coatings define the age and purpose of the room.
   foreach(var r in map.Root.GetComponentsInChildren<MeshRenderer>())if(r.name=="InteriorWall"){var skin=new Material(r.sharedMaterial);skin.color=PixelArt.Hex("8E927F");r.sharedMaterial=skin;lifetime.Materials.Add(skin);}
   FloorWear();
   WallSkin(new Vector3(-4.395f,0,0),Vector3.forward,Vector3.right,13.8f);WallSkin(new Vector3(4.395f,0,0),Vector3.forward,Vector3.left,13.8f);
   WallSkin(new Vector3(0,0,-6.895f),Vector3.right,Vector3.forward,8.8f);WallSkin(new Vector3(0,0,6.895f),Vector3.right,Vector3.back,8.8f);
   foreach(float side in new[]{-1f,1f})foreach(var seg in new[]{new Vector2(-2.65f,3.7f),new Vector2(2.75f,3.5f)})WallSkin(new Vector3(seg.x,0,1.3f+side*.105f),Vector3.right,Vector3.forward*side,seg.y);
   BrickScar(new Vector3(-4.365f,.7f,-4.8f),Vector3.forward,Vector3.right);BrickScar(new Vector3(1.6f,.73f,6.86f),Vector3.right,Vector3.back);
   StripLight(new Vector3(-4.29f,1.96f,-2.4f),Vector3.forward,Vector3.right);StripLight(new Vector3(2.3f,1.96f,6.77f),Vector3.right,Vector3.back);
   // Dusty sealed window above the sitting area; it does not expose another map.
   Cube("WindowFrame","3E534A",new Vector3(.6f,1.66f,-6.81f),new Vector3(2.2f,.80f,.10f));Panel("OpaqueDustyGlass","738579",new Vector3(.6f,1.66f,-6.747f),Vector3.right,Vector3.up,2.0f,.64f);
   for(int i=0;i<3;i++)Panel("WindowMullion","455B50",new Vector3(-.35f+i*.95f,1.66f,-6.73f),Vector3.right,Vector3.up,.045f,.72f);
   Stroke("CrackedGlass","A8AB91",new Vector3(.78f,1.96f,-6.722f),new Vector3(.41f,1.42f,-6.722f),.016f,Vector3.forward,true);
   Cube("FuseBox","647668",new Vector3(4.26f,1.25f,-.15f),new Vector3(.18f,.6f,.43f));Panel("FuseWarning","B0A476",new Vector3(4.161f,1.31f,-.15f),Vector3.forward,Vector3.up,.2f,.18f);
   Tube("FuseConduit","737663",new Vector3(4.3f,1.55f,-.15f),new Vector3(4.3f,2.07f,-.15f),.026f);
   // Exit portal must remain readable; do not add a panel or collider over it.
   for(int i=0;i<7;i++)Panel("ExitWarningStripes",i%2==0?"A89A6F":"44574D",new Vector3(4.265f,.26f+i*.22f,-2.67f),Vector3.forward,Vector3.up,.09f,.16f);
   FurnitureWear();AlienCorner();
   Bake(resources,lifetime);
  }
  internal void Bake(GameObject resources,WorkshopArtResources lifetime){
   foreach(var entry in batches){var parts=entry.Key.Split('|');var g=new GameObject(parts[0]);g.transform.SetParent(resources.transform,false);var mesh=new Mesh{name="Workshop_"+entry.Key};if(entry.Value.V.Count>65535)mesh.indexFormat=IndexFormat.UInt32;mesh.SetVertices(entry.Value.V);mesh.SetTriangles(entry.Value.T,0);mesh.RecalculateNormals();mesh.RecalculateBounds();g.AddComponent<MeshFilter>().sharedMesh=mesh;var r=g.AddComponent<MeshRenderer>();var mat=new Material(GreyHarbor.FlashlightShaders2763.Load("HarborWorld"));mat.color=PixelArt.Hex(parts[1]);mat.SetFloat("_SolidFog",parts[2]=="1"?1:0);mat.SetFloat("_Detail",.045f);r.sharedMaterial=mat;r.shadowCastingMode=ShadowCastingMode.Off;r.receiveShadows=true;lifetime.Meshes.Add(mesh);lifetime.Materials.Add(mat);}
  }
 }
 public sealed class WorkshopArtResources:MonoBehaviour {
  public readonly List<Mesh> Meshes=new List<Mesh>();public readonly List<Material> Materials=new List<Material>();
  void OnDestroy(){foreach(var m in Meshes)if(m){if(Application.isPlaying)Destroy(m);else DestroyImmediate(m);}foreach(var m in Materials)if(m){if(Application.isPlaying)Destroy(m);else DestroyImmediate(m);}}
 }
}
