using System.Collections.Generic;
using UnityEngine;
namespace GreyHarbor {
 public sealed class CargoGroundRegion2715 {
  public string Name,Finish;public Rect Area;public Vector2[] Polygon;
 }
 public sealed class CargoGroundTransition2715 {
  public string Name;public Rect Area;public Vector3 Inner,Outer,SeamStart,SeamEnd;public Color OuterColor;
 }
 public sealed partial class RaidMap {
  public readonly List<CargoGroundRegion2715> CargoGroundRegions2715=new List<CargoGroundRegion2715>();
  public readonly List<CargoGroundTransition2715> CargoGroundTransitions2715=new List<CargoGroundTransition2715>();
  public bool CargoGroundBuilt2715 {get;private set;}
  public int CargoGroundTriangleCount2715 {get;private set;}
  public const float CargoGroundRenderHeight2715=.05f;
  public static readonly Rect CargoGroundDrawBounds2715=new Rect(-28,-19,88,83);
  readonly List<Rect> cargoGroundPieces2715=new List<Rect>();
  readonly List<Vector2[]> cargoAsphalt2715=new List<Vector2[]>();
  readonly List<Vector3> cargoPaintVertices2715=new List<Vector3>(12);
  readonly List<int> cargoPaintIndices2715=new List<int>(18);
  static Rect CargoGroundRect2715(float x0,float z0,float x1,float z1)=>Rect.MinMaxRect(x0-79,z0-73,x1-79,z1-73);
  static Vector2[] CargoGroundPolygon2715(params float[] coords){var p=new Vector2[coords.Length/2];for(int i=0;i<p.Length;i++)p[i]=new Vector2(coords[i*2]-79,coords[i*2+1]-73);return p;}
  static Vector2[] CargoGroundCorners2715(Rect r)=>new[]{new Vector2(r.xMin,r.yMin),new Vector2(r.xMax,r.yMin),new Vector2(r.xMax,r.yMax),new Vector2(r.xMin,r.yMax)};
  static Rect CargoGroundExtent2715(Vector2[] p){float x0=p[0].x,x1=x0,z0=p[0].y,z1=z0;foreach(var q in p){x0=Mathf.Min(x0,q.x);x1=Mathf.Max(x1,q.x);z0=Mathf.Min(z0,q.y);z1=Mathf.Max(z1,q.y);}return Rect.MinMaxRect(x0,z0,x1,z1);}
  public bool OnCargoGround2715(Vector3 p)=>CargoGroundDrawBounds2715.Contains(new Vector2(p.x,p.z))&&OnHarborSurface(p);
  Color CargoUnderlyingGround2715(Vector2 p){foreach(var road in HarborRoadShapes)if(InPolygon(p,road))return PixelArt.Hex("667077");Color c=PixelArt.Hex("A0A7A4");foreach(var s in HarborSurfaces)if(InPolygon(p,s.Points))c=PixelArt.Hex(s.Color);return c;}
  void CargoGroundRegionAdd2715(string name,string finish,Vector2[] p){CargoGroundRegions2715.Add(new CargoGroundRegion2715{Name=name,Finish=finish,Area=CargoGroundExtent2715(p),Polygon=p});}
  void CargoGroundTransitionAdd2715(string name,Rect area,Vector3 outer,Vector3 inner,bool vertical){
   var v=new Vector3(vertical?area.width*.5f:0,0,vertical?0:area.height*.5f);
   CargoGroundTransitions2715.Add(new CargoGroundTransition2715{Name=name,Area=area,Outer=outer,Inner=inner,SeamStart=outer-v,SeamEnd=outer+v,OuterColor=CargoUnderlyingGround2715(new Vector2(outer.x,outer.z))});
   CargoGroundRegionAdd2715(name,"RoadToConcrete",CargoGroundCorners2715(area));
  }
  float CargoGroundBlend2715(Vector2 p){
   // The road union, rather than four entrance rectangles, defines the apron edge.
   // Preserve every road polygon exactly; blend continuously across the whole perimeter.
   float roadDistance=float.MaxValue;
   foreach(var road in HarborRoadShapes){if(InPolygon(p,road))return 0;for(int i=0;i<road.Length;i++){var a=road[i];var d=road[(i+1)%road.Length]-a;float t=Mathf.Clamp01(Vector2.Dot(p-a,d)/Mathf.Max(d.sqrMagnitude,.000001f));roadDistance=Mathf.Min(roadDistance,(p-a-d*t).magnitude);}}
   var r=CargoGroundDrawBounds2715;float edge=Mathf.Min(p.x-r.xMin,r.xMax-p.x,p.y-r.yMin,r.yMax-p.y);
   return Mathf.SmoothStep(0,1,Mathf.Clamp01(Mathf.Min(roadDistance/5.5f,edge/3.2f)));
  }
  bool CargoGroundAsphalt2715(Vector2 p){foreach(var a in cargoAsphalt2715)if(InPolygon(p,a))return true;return false;}
  static float CargoGroundNoise2715(float x,float z,float scale,float seed)=>Mathf.PerlinNoise(x*scale+seed,z*scale-seed*.37f);
  Color CargoGroundBaseColor2715(Vector2 p){
   float x=p.x+79,z=p.y+73;bool asphalt=CargoGroundAsphalt2715(p);
   Color c=PixelArt.Hex(asphalt?"747970":"ABB0A6");
   float broad=CargoGroundNoise2715(x,z,.13f,11.4f)-.5f,grain=CargoGroundNoise2715(x,z,1.7f,87.3f)-.5f;
   // Subtle slab tone follows the poured panels; broad damp mottling avoids a checkerboard.
   float slab=asphalt?0:(CargoGroundNoise2715(Mathf.Floor(x/5.8f),Mathf.Floor(z/8.4f),.81f,20.3f)-.5f)*.065f;
   float shade=broad*(asphalt?.14f:.10f)+grain*.035f+slab;
   c.r+=shade;c.g+=shade;c.b+=shade;
   float damp=CargoGroundNoise2715(x,z,.057f,19.9f);if(damp>.59f)c=Color.Lerp(c,PixelArt.Hex(asphalt?"60685F":"858F83"),(damp-.59f)*1.6f);
   return Color.Lerp(CargoUnderlyingGround2715(p),c,CargoGroundBlend2715(p));
  }
  // Priority is consumed by the existing raster bake. It is not a rendered height.
  void CargoGroundPaint2715(Vector2[] polygon,Color color,float priority,bool fade=true,bool concreteOnly=false){
   foreach(var piece in cargoGroundPieces2715){var p=new List<Vector2>(polygon);for(int side=0;side<4&&p.Count>0;side++)p=CargoGroundClip2715(p,piece,side);if(p.Count<3)continue;
    float area=0;for(int i=0;i<p.Count;i++)area+=Cross(p[i],p[(i+1)%p.Count]);if(Mathf.Abs(area)<.000001f)continue;
    Vector2 center=Vector2.zero;foreach(var q in p)center+=q;center/=p.Count;
    if(concreteOnly&&CargoGroundAsphalt2715(center))continue;
    Color ink=fade?Color.Lerp(CargoGroundBaseColor2715(center),color,CargoGroundBlend2715(center)):color;
    cargoPaintVertices2715.Clear();cargoPaintIndices2715.Clear();foreach(var q in p)cargoPaintVertices2715.Add(new Vector3(q.x,priority,q.y));for(int i=1;i<p.Count-1;i++){cargoPaintIndices2715.Add(0);cargoPaintIndices2715.Add(i);cargoPaintIndices2715.Add(i+1);}
    RegisterGround137(cargoPaintVertices2715,cargoPaintIndices2715,ink);CargoGroundTriangleCount2715+=cargoPaintIndices2715.Count/3;
   }
  }
  static List<Vector2> CargoGroundClip2715(List<Vector2> p,Rect r,int side){var result=new List<Vector2>();float boundary=side==0?r.xMin:side==1?r.xMax:side==2?r.yMin:r.yMax;for(int i=0;i<p.Count;i++){
   var a=p[i];var b=p[(i+1)%p.Count];float av=side<2?a.x:a.y,bv=side<2?b.x:b.y;bool ai=side==0||side==2?av>=boundary:av<=boundary,bi=side==0||side==2?bv>=boundary:bv<=boundary;
   if(ai)result.Add(a);if(ai!=bi){float t=(boundary-av)/(bv-av);result.Add(Vector2.Lerp(a,b,t));}
  }return result;}
  void CargoGroundStrip2715(Vector2 a,Vector2 b,float width,Color c,float priority,bool concreteOnly=false){var delta=b-a;if(delta.sqrMagnitude<.00001f)return;var n=new Vector2(-delta.y,delta.x).normalized*width*.5f;int parts=Mathf.Max(1,Mathf.CeilToInt(delta.magnitude/.6f));for(int i=0;i<parts;i++){var start=Vector2.Lerp(a,b,(float)i/parts);var end=Vector2.Lerp(a,b,(float)(i+1)/parts);CargoGroundPaint2715(new[]{start+n,end+n,end-n,start-n},c,priority,true,concreteOnly);}}
  void CargoGroundOval2715(Vector2 center,float xradius,float zradius,float angle,Color c,float priority){var p=new Vector2[10];float a=angle*Mathf.Deg2Rad;for(int i=0;i<p.Length;i++){float t=i*Mathf.PI*2/p.Length,rx=Mathf.Cos(t)*xradius*(1+.12f*Mathf.Sin(i*2.3f)),rz=Mathf.Sin(t)*zradius;p[i]=center+new Vector2(rx*Mathf.Cos(a)-rz*Mathf.Sin(a),rx*Mathf.Sin(a)+rz*Mathf.Cos(a));}CargoGroundPaint2715(p,c,priority);}
  public void BuildCargoGround2715(){
   if(CargoGroundBuilt2715)return;CargoGroundBuilt2715=true;
   CargoGroundRegionAdd2715("CargoConcreteApron","JointedWeatheredConcrete",CargoGroundCorners2715(CargoBounds2712));
   CargoGroundTransitionAdd2715("SouthRoadBlend",CargoGroundRect2715(87.5f,54,96.5f,62.5f),PortPoint(92,54),PortPoint(92,62.5f),true);
   CargoGroundTransitionAdd2715("NorthRoadBlend",CargoGroundRect2715(93.5f,127.5f,102.5f,137),PortPoint(98,137),PortPoint(98,127.5f),true);
   CargoGroundTransitionAdd2715("WestRoadBlend",CargoGroundRect2715(51,91.5f,57.5f,100.5f),PortPoint(51,96),PortPoint(57.5f,96),false);
   CargoGroundTransitionAdd2715("EastRoadBlend",CargoGroundRect2715(127.5f,97.5f,139,106.5f),PortPoint(139,102),PortPoint(127.5f,102),false);
   // A continuous surrounding apron replaces the four narrow rectangular tongues.
   // North/south strips include the corners; east/west only span the core's depth.
   cargoGroundPieces2715.Add(CargoBounds2712);
   var perimeter=new[]{CargoGroundRect2715(51,54,139,60.5f),CargoGroundRect2715(51,129.5f,139,137),CargoGroundRect2715(51,60.5f,55.5f,129.5f),CargoGroundRect2715(129.5f,60.5f,139,129.5f)};
   for(int i=0;i<perimeter.Length;i++){cargoGroundPieces2715.Add(perimeter[i]);CargoGroundRegionAdd2715("ContinuousApronEdge"+(i+1),"NaturalRoadAndLandBlend",CargoGroundCorners2715(perimeter[i]));}
   cargoAsphalt2715.Add(CargoGroundPolygon2715(60.7f,72.2f,74.8f,69.5f,83.3f,73.7f,82,82.4f,74.7f,88.1f,63.3f,85.5f));
   cargoAsphalt2715.Add(CargoGroundPolygon2715(108.3f,87.5f,124.8f,89.2f,125.9f,99.1f,118.5f,105.2f,105.1f,101.7f,103.7f,93.1f));
   cargoAsphalt2715.Add(CargoGroundPolygon2715(68.1f,108.7f,82.6f,105.9f,88.5f,113.8f,84.7f,123.9f,72.2f,125,65.9f,116.4f));
   for(int i=0;i<cargoAsphalt2715.Count;i++)CargoGroundRegionAdd2715("OldAsphaltRepair"+(i+1),"PatchedAsphalt",cargoAsphalt2715[i]);
   const float tile=.65f;
   foreach(var r in cargoGroundPieces2715)for(float z=r.yMin;z<r.yMax;z+=tile)for(float x=r.xMin;x<r.xMax;x+=tile){var cell=Rect.MinMaxRect(x,z,Mathf.Min(x+tile,r.xMax),Mathf.Min(z+tile,r.yMax));CargoGroundPaint2715(CargoGroundCorners2715(cell),CargoGroundBaseColor2715(cell.center),.20f,false);}
   // Poured concrete joints, faded edge lips and short repairs across the whole field.
   Color joint=PixelArt.Hex("747E74"),lip=PixelArt.Hex("BDC0B6");
   for(float x=58.2f;x<129.5f;x+=5.8f){CargoGroundStrip2715(new Vector2(x-79,-12.5f),new Vector2(x-79,56.5f),.065f,joint,.211f,true);CargoGroundStrip2715(new Vector2(x-78.91f,-12.5f),new Vector2(x-78.91f,56.5f),.035f,lip,.212f,true);}
   for(float z=64.6f;z<129.5f;z+=8.4f){CargoGroundStrip2715(new Vector2(-23.5f,z-73),new Vector2(50.5f,z-73),.065f,joint,.211f,true);}
   // Deterministic surface wear: repair chips, grime, exposed aggregate and cracked joints.
   for(int iz=0;iz<12;iz++)for(int ix=0;ix<13;ix++){
    float x=58+ix*5.6f+(CargoGroundNoise2715(ix,iz,1.31f,7.2f)-.5f)*3,z=63+iz*5.6f+(CargoGroundNoise2715(ix,iz,1.14f,45.1f)-.5f)*3;
    var p=new Vector2(x-79,z-73);float n=CargoGroundNoise2715(ix,iz,.73f,61.7f);
    CargoGroundOval2715(p,.38f+n*.74f,.18f+n*.4f,ix*31+iz*17,PixelArt.Hex(n>.56f?"899286":"98A094"),.214f);
    if((ix+iz)%3==0){var end=p+new Vector2(.9f+n,.3f-n);CargoGroundStrip2715(p,end,.045f,PixelArt.Hex("7D887C"),.215f);CargoGroundStrip2715(end,end+new Vector2(.35f,.4f),.04f,PixelArt.Hex("7D887C"),.215f);}
    for(int j=0;j<5;j++){var q=p+new Vector2(Mathf.Sin(ix*9+iz*3+j*2)*1.35f,Mathf.Cos(ix*3+iz*5+j*3)*1.2f);CargoGroundOval2715(q,.065f+j*.008f,.04f,ix+j*37,PixelArt.Hex(j%2==0?"C0C3B8":"8D9689"),.216f);}
   }
   // Incomplete yellow cargo-bay corners replace fresh, unbroken parking rectangles.
   var bays=new[]{new Vector3(68,96,0),new Vector3(86,103,12),new Vector3(116,77,-10),new Vector3(114,114,0),new Vector3(73,114,-12),new Vector3(99,67,0),new Vector3(117,125,0)};
   foreach(var bay in bays){var center=new Vector2(bay.x-79,bay.y-73);float a=bay.z*Mathf.Deg2Rad;var right=new Vector2(Mathf.Cos(a),Mathf.Sin(a));var up=new Vector2(-right.y,right.x);
    foreach(float side in new[]{-1f,1f}){var corner=center+right*side*3.8f+up*2.1f;CargoGroundStrip2715(corner,corner-right*side*1.35f,.13f,PixelArt.Hex("B4A575"),.223f);CargoGroundStrip2715(corner,corner-up*.9f,.13f,PixelArt.Hex("B4A575"),.223f);}
    for(int j=0;j<4;j++){var p=center+right*(j*1.45f-2.6f)-up*2.1f;CargoGroundStrip2715(p,p+right*.75f,.12f,PixelArt.Hex(j%2==0?"BEAE7D":"A6A07B"),.223f);}
   }
   // Forklift tyre rubs follow turns rather than advertising a perfectly straight new road.
   var rubs=new[]{new Vector4(87,75,4.7f,74),new Vector4(96,88,5.4f,80),new Vector4(96,103,5,135),new Vector4(98,115,4.8f,63),new Vector4(65,94,4.2f,7),new Vector4(118,99,5.2f,22),new Vector4(76,112,4.1f,-20)};
   foreach(var rub in rubs){float a=rub.w*Mathf.Deg2Rad;var d=new Vector2(Mathf.Cos(a),Mathf.Sin(a));var n=new Vector2(-d.y,d.x);var p=new Vector2(rub.x-79,rub.y-73);
    for(int side=-1;side<=1;side+=2)for(int j=0;j<6;j++){var q=p+n*(side*.64f)+d*(j*rub.z/6-rub.z*.5f);CargoGroundStrip2715(q,q+d*(rub.z/6*.67f),.11f,PixelArt.Hex("707B70"),.224f);}
   }
   foreach(var p in new[]{new Vector3(70,82,0),new Vector3(113,92,0),new Vector3(80,119,0),new Vector3(120,110,0),new Vector3(100,80,0),new Vector3(62,105,0)}){
    var c=new Vector2(p.x-79,p.y-73);CargoGroundOval2715(c,1.05f,.48f,24,PixelArt.Hex("707E70"),.219f);CargoGroundOval2715(c+new Vector2(.17f,-.03f),.54f,.29f,38,PixelArt.Hex("59685B"),.220f);
   }
  }
 }
}
