using UnityEngine;
using UnityEngine.Rendering;
using System.Collections.Generic;
namespace GreyHarbor {
 // Batched reusable surface art. Shallow wear and vegetation are decorative, never invisible blockers.
 public sealed class YardSurfaceArt {
  readonly RaidMap map;readonly GroundSurfaceArt surfaces;int roadLayer;readonly System.Random random=new System.Random(19207);
  sealed class Batch {public readonly List<Vector3> V=new List<Vector3>();public readonly List<int> T=new List<int>();}
  readonly Dictionary<string,Batch> batches=new Dictionary<string,Batch>();
  public YardSurfaceArt(RaidMap map){this.map=map;surfaces=new GroundSurfaceArt(map,117192);}
  float R(float a,float b)=>a+(float)random.NextDouble()*(b-a);
  Batch For(string group,string color){string key=group+"|"+color;if(!batches.TryGetValue(key,out var b)){b=new Batch();batches.Add(key,b);}return b;}
  void Tri(string group,string color,Vector3 a,Vector3 b,Vector3 c,bool both=false){var mesh=For(group,color);int n=mesh.V.Count;mesh.V.Add(a);mesh.V.Add(b);mesh.V.Add(c);mesh.T.Add(n);mesh.T.Add(n+1);mesh.T.Add(n+2);if(both){mesh.V.Add(c);mesh.V.Add(b);mesh.V.Add(a);mesh.T.Add(n+3);mesh.T.Add(n+4);mesh.T.Add(n+5);}}
  void Quad(string group,string color,Vector3 a,Vector3 b,Vector3 c,Vector3 d,bool both=false){Tri(group,color,a,b,c,both);Tri(group,color,a,c,d,both);}
  void Rect(string group,string color,float x,float z,float w,float d,float y){Quad(group,color,new Vector3(x-w/2,y,z-d/2),new Vector3(x-w/2,y,z+d/2),new Vector3(x+w/2,y,z+d/2),new Vector3(x+w/2,y,z-d/2));}
  void Line(string group,string color,Vector3 a,Vector3 b,float width){Vector3 side=Vector3.Cross(Vector3.up,(b-a).normalized)*width/2;Quad(group,color,a-side,b-side,b+side,a+side,true);}
  Vector3[] Rim(float x,float z,float rx,float rz,float y,int n=14){var v=new Vector3[n];for(int i=0;i<n;i++){float a=(float)i/n*Mathf.PI*2,k=R(.82f,1.14f);v[i]=new Vector3(x+Mathf.Cos(a)*rx*k,y,z+Mathf.Sin(a)*rz*k);}return v;}
  void Disc(string group,string color,Vector3 center,Vector3[] rim){for(int i=0;i<rim.Length;i++)Tri(group,color,center,rim[(i+1)%rim.Length],rim[i]);}
  void Patch(string group,string color,float x,float z,float rx,float rz,float y){Disc(group,color,new Vector3(x,y,z),Rim(x,z,rx,rz,y));}
  bool Building(float x,float z){return (x>-7.8f&&x<5.8f&&z>7.2f&&z<14.8f)||(x>-16.3f&&x<-9.7f&&z>-4.3f&&z<6.3f)||(x>6.2f&&x<9.8f&&z>-14.8f&&z<-11.2f);}
  bool Road(float x,float z)=>Mathf.Abs(x+7)<1.6f||Mathf.Abs(x-15)<1.7f||Mathf.Abs(z+9.5f)<1.3f||Mathf.Abs(z-6)<1.05f;
  void Asphalt(float x,float z,float width,float length,bool alongZ){
   float w=alongZ?width:length,d=alongZ?length:width;
   Rect("RoadShoulder","777A69",x,z,w+.30f,d+.18f,.011f);surfaces.Region(new Rect(x-w/2,z-d/2,w,d),GroundFinish.Asphalt,.006f+roadLayer++*.00012f);
   int pieces=Mathf.CeilToInt(length/.8f);for(int side=-1;side<=1;side+=2)for(int i=0;i<pieces;i++){
    if(i%3!=0)continue;float t=-length/2+(i+.5f)*length/pieces;float px=x+(alongZ?side*width/2:t),pz=z+(alongZ?t:side*width/2);
    Patch("EdgeChips",i%3==0?"6C7163":"535F5B",px,pz,alongZ?R(.05f,.13f):R(.22f,.48f),alongZ?R(.22f,.48f):R(.05f,.13f),.024f);
   }
   for(int i=0;i<12;i++){float px=x+R(-w*.43f,w*.43f),pz=z+R(-d*.43f,d*.43f);Patch("RoadRepairs",i%2==0?"59645D":"58635C",px,pz,alongZ?R(.25f,.65f):R(.45f,1),alongZ?R(.45f,1):R(.25f,.65f),.028f);}
   // Two broken wheel-worn tracks; gaps keep them from looking like painted stripes.
   for(int i=0;i<18;i++)for(int side=-1;side<=1;side+=2){float t=R(-length*.47f,length*.47f);float px=x+(alongZ?side*width*.23f:t),pz=z+(alongZ?t:side*width*.23f);Rect("WheelWear","5B655E",px,pz,alongZ?.23f:R(.25f,.65f),alongZ?R(.25f,.65f):.23f,.031f);}
  }
  void Pothole(float x,float z,float radius){
   var outer=Rim(x,z,radius,radius*.72f,.062f,13);var inner=new Vector3[outer.Length];
   for(int i=0;i<outer.Length;i++){inner[i]=new Vector3(x+(outer[i].x-x)*.74f,.041f,z+(outer[i].z-z)*.74f);int j=(i+1)%outer.Length;}
   for(int i=0;i<outer.Length;i++){int j=(i+1)%outer.Length;Quad("PotholeEdges",i%3==0?"86816B":"716E5B",outer[i],inner[i],inner[j],outer[j]);}
   Disc("PotholeBed","393F37",new Vector3(x,.035f,z),inner);Patch("DampSoil","474C3D",x+.08f,z-.07f,radius*.41f,radius*.27f,.047f);
   Patch("Puddle","52625C",x-.10f,z+.04f,radius*.27f,radius*.18f,.049f);Line("PuddleGlint","758075",new Vector3(x-radius*.28f,.052f,z+.05f),new Vector3(x+radius*.05f,.052f,z+.08f),.025f);
   for(int i=0;i<9;i++){float a=R(0,Mathf.PI*2),r=radius*R(.85f,1.28f);Stone(x+Mathf.Cos(a)*r,z+Mathf.Sin(a)*r,R(.025f,.075f));}
  }
  void Stone(float x,float z,float size){var rim=Rim(x,z,size,size*.75f,.065f,5);Disc("Gravel","858473",new Vector3(x,.065f+size*.65f,z),rim);}
  void Crack(float x,float z){GroundFracture.Draw(new Vector3(x,.041f,z),R(.55f,1.15f),R(.06f,.085f),R(0,Mathf.PI*2),random,(color,a,b,c)=>Tri("GroundFracture",color,a,b,c));}
  void Grass(float x,float z,float rx,float rz,int tufts){
   Patch("WeedBeds","6B7155",x,z,rx,rz,.036f);Patch("WeedBeds","747657",x+.07f,z-.05f,rx*.65f,rz*.7f,.039f);
   for(int i=0;i<tufts;i++){
    float a=R(0,Mathf.PI*2),r=Mathf.Sqrt(R(0,1)),px=x+Mathf.Cos(a)*rx*r,pz=z+Mathf.Sin(a)*rz*r;
    float height=R(.12f,.42f);string c=i%3==0?"8F9568":i%3==1?"697C50":"566C47";
    for(int blade=0;blade<4;blade++){float d=R(0,Mathf.PI*2);Vector3 side=new Vector3(Mathf.Cos(d),0,Mathf.Sin(d))*.038f;Vector3 bottom=new Vector3(px,.055f,pz);Vector3 tip=bottom+new Vector3(R(-.12f,.12f),height*R(.6f,1),R(-.12f,.12f));Tri("GrassBlades",c,bottom-side,tip,bottom+side,true);}
    if(i%8==0){var stem=new Vector3(px,.055f,pz);var tip=stem+new Vector3(.04f,height+.10f,.02f);Line("SeedStalks","929367",stem,tip,.018f);Tri("SeedHeads","B0A979",tip-new Vector3(.04f,0,0),tip+Vector3.up*.07f,tip+new Vector3(.04f,0,0),true);}
   }
  }
  void Drain(float x,float z,float length,bool alongZ){
   Rect("DrainBed","434F49",x,z,alongZ?.30f:length,alongZ?length:.30f,.033f);
   for(int side=-1;side<=1;side+=2)Rect("DrainLips","969785",x+(alongZ?side*.20f:0),z+(alongZ?0:side*.20f),alongZ?.10f:length,alongZ?length:.10f,.047f);
   for(float t=-length/2+.08f;t<length/2;t+=.18f)Rect("DrainGrates","788276",x+(alongZ?0:t),z+(alongZ?t:0),alongZ?.29f:.045f,alongZ?.045f:.29f,.056f);
  }
  public void Build(){
   map.Box("SurroundingApron",new Vector3(0,-.22f,0),new Vector3(78,.3f,70),"59644E");
   map.Box("YardConcrete",new Vector3(0,-.12f,0),new Vector3(38,.24f,34),"828879").GetComponent<Renderer>().sharedMaterial.SetFloat("_Detail",.045f);
   // Concrete slabs have muted variation, broken seams, and occasional aggregate exposure.
   for(int x=-16;x<=16;x+=4)for(int z=-14;z<=14;z+=4){Rect("ConcreteSlabs",(x+z)%3==0?"858A7B":"7E8576",x,z,3.94f,3.94f,.005f);}
   Asphalt(-7,-2,3.1f,23,true);Asphalt(15,-1,3.4f,27,true);Asphalt(3,-9.5f,2.5f,25,false);Asphalt(2,6,2,21,false);
   for(float z=-13;z<13;z+=3)foreach(float x in new[]{-8.35f,13.5f})for(int i=0;i<3;i++)Rect("FadedPaint","B1AA7D",x+R(-.014f,.014f),z+i*.27f,.065f,R(.18f,.23f),.040f);
   // Quiet parking/handling pads, faded corners rather than bright rectangles.
   foreach(float x in new[]{-2.4f,3.3f}){Rect("ParkingPaint","ADA986",x,0,.055f,4.3f,.041f);Rect("ParkingPaint","ADA986",x+.4f,2.15f,.8f,.055f,.041f);}
   Drain(-8.9f,-6.7f,7.4f,true);Drain(13.05f,4.3f,5.5f,true);Drain(.4f,7.0f,3.2f,false);
   foreach(var p in new[]{new Vector3(-7,0,-7.6f),new Vector3(14.7f,0,-1.3f),new Vector3(5,0,-9.6f),new Vector3(6.6f,0,6),new Vector3(-12.8f,0,-10.4f),new Vector3(-1.4f,0,-1.6f),new Vector3(11.4f,0,11.5f)})Pothole(p.x,p.z,R(.55f,1.0f));
   for(int i=0;i<115;i++){float x=R(-17,17),z=R(-15,15);if(Building(x,z))continue;if(!Road(x,z)&&i%3==0)Patch("ConcreteWear","777E6D",x,z,R(.15f,.6f),R(.12f,.5f),.030f);if(i%3==0)Crack(x,z);if(i%4==0)Stone(x,z,.045f);}
   // Vegetation stays concentrated at walls, corners and unused shoulders so paths remain readable.
   foreach(var p in new[]{new Vector3(-16.8f,0,-11),new Vector3(-16.8f,0,-7),new Vector3(-16.9f,0,10),new Vector3(-12.5f,0,-14.5f),new Vector3(-7.6f,0,14.8f),new Vector3(7.4f,0,14.6f),new Vector3(16.9f,0,5),new Vector3(16.8f,0,12.8f),new Vector3(10.7f,0,-14.5f),new Vector3(-10.5f,0,-6.4f),new Vector3(8.3f,0,-.2f)})Grass(p.x,p.z,R(.45f,1.0f),R(.45f,1.1f),24);
   // Beyond the closed fence: a weathered access road, dirt, ditches, gravel and larger weed patches.
   Asphalt(0,-21.2f,3.8f,73,false);Asphalt(23,3,3.2f,49,true);
   for(int i=0;i<45;i++){float x=R(-34,34),z=R(-30,30);if(Mathf.Abs(x)<19&&Mathf.Abs(z)<17)continue;Patch("OuterSoil",i%2==0?"74735A":"656A50",x,z,R(.8f,2.2f),R(.6f,1.6f),.008f);Grass(x,z,R(.5f,1.4f),R(.5f,1.5f),18);for(int n=0;n<5;n++)Stone(x+R(-1,1),z+R(-1,1),R(.04f,.12f));}
   Drain(-21,0,29,true);Drain(0,19,31,false);
   foreach(float x in new[]{-29f,-17f,-4f,10f,28f})Pothole(x,-21.1f,R(.6f,1.1f));
   surfaces.Region(new Rect(-17.4f,7.3f,7.5f,8.0f),GroundFinish.Gravel);
   surfaces.Region(new Rect(6.3f,9.0f,6.3f,6.3f),GroundFinish.Gravel);
   surfaces.Region(new Rect(-17.5f,-14.8f,1.15f,8.7f),GroundFinish.Soil);
   surfaces.Region(new Rect(10.0f,-15.3f,2.8f,3.7f),GroundFinish.Soil);
   surfaces.Region(new Rect(-18.0f,-25.5f,36,2.2f),GroundFinish.Soil);
   surfaces.Region(new Rect(25.5f,-13,3.0f,29),GroundFinish.Gravel);
   surfaces.Region(new Rect(9.65f,.32f,2.7f,.65f),GroundFinish.Steel,.012f);
   surfaces.Region(new Rect(9.65f,-7.52f,2.7f,.70f),GroundFinish.Steel,.012f);
   Bake();
  }
  public void BuildSample(){Asphalt(0,0,3,8,true);Pothole(.3f,.8f,.65f);Grass(-2.3f,1.5f,.6f,1.2f,24);Drain(2,0,6,true);Bake();}
  void Bake(){
   surfaces.Bake("YardMaterialSurfaces_117");
   var root=new GameObject("YardSurfaceDetails_192");root.transform.SetParent(map.Root,false);
   foreach(var pair in batches){var split=pair.Key.Split('|');var g=new GameObject(split[0]);g.transform.SetParent(root.transform,false);var mesh=new Mesh{name=pair.Key};if(pair.Value.V.Count>65535)mesh.indexFormat=IndexFormat.UInt32;mesh.SetVertices(pair.Value.V);mesh.SetTriangles(pair.Value.T,0);mesh.RecalculateNormals();mesh.RecalculateBounds();g.AddComponent<MeshFilter>().sharedMesh=mesh;var renderer=g.AddComponent<MeshRenderer>();renderer.sharedMaterial=map.Mat(split[1],false);renderer.sharedMaterial.SetFloat("_Detail",.03f);renderer.shadowCastingMode=ShadowCastingMode.Off;renderer.receiveShadows=true;}
  }
 }
}
