using System.Collections.Generic;
using UnityEngine;
namespace GreyHarbor {
 public sealed class FreightSurfaceFeature2717 {public string Name,Kind;public Vector2[] Polygon;public Rect Area;public string PaletteHex;}
 public sealed partial class RaidMap {
  public readonly List<FreightSurfaceFeature2717> FreightSurfaceFeatures2717=new List<FreightSurfaceFeature2717>();
  public readonly List<GameObject> FreightEdgeGrass2717=new List<GameObject>();
  static float FreightNoise2717(float x,float z,float scale,float seed)=>Mathf.PerlinNoise(x*scale+seed,z*scale-seed*.17f);
  static Vector2 FreightSurfacePoint2717(float x,float z)=>new Vector2(x-79,z-73);
  void FreightSurfaceRegion2717(string name,string kind,string hex,params float[] coords){var polygon=CargoGroundPolygon2715(coords);var area=CargoGroundExtent2715(polygon);FreightSurfaceFeatures2717.Add(new FreightSurfaceFeature2717{Name=name,Kind=kind,Polygon=polygon,Area=area,PaletteHex=hex});FreightGroundRegions2716.Add(new FreightGroundRegion2716{Name=name,Finish=kind,Area=area});}
  float FreightSurfacePatchWeight2717(Vector2 p,FreightSurfaceFeature2717 f){if(!f.Area.Contains(p)||!InPolygon(p,f.Polygon))return 0;float d=float.MaxValue;for(int i=0;i<f.Polygon.Length;i++)d=Mathf.Min(d,CargoPointSegment2715(p,f.Polygon[i],f.Polygon[(i+1)%f.Polygon.Length]));float noise=FreightNoise2717(p.x+79,p.y+73,.57f,12.1f);return Mathf.SmoothStep(0,1,Mathf.Clamp01(d/(f.Kind=="Mud"?1.5f:f.Kind=="Gravel"?1.1f:f.Kind=="Oil"?.40f:.8f)))*Mathf.Lerp(.56f,.87f,noise);}
  Color FreightSurfaceColor2717(Vector2 p){float x=p.x+79,z=p.y+73;var c=FreightPigment2718("AEB1B3"); // Cool old cement, separate from warm soil and packing wood.
   float broad=FreightNoise2717(x,z,.12f,17.1f)-.5f,fine=FreightNoise2717(x,z,2.4f,43.6f)-.5f,grain=FreightNoise2717(x,z,8.9f,31.3f)-.5f;
   c.r+=broad*.13f+fine*.055f+grain*.032f;c.g+=broad*.12f+fine*.05f+grain*.03f;c.b+=broad*.11f+fine*.045f+grain*.03f;
   foreach(var f in FreightSurfaceFeatures2717){float weight=FreightSurfacePatchWeight2717(p,f);if(weight<=0)continue;var ink=FreightPigment2718(f.PaletteHex);float n=FreightNoise2717(x,z,f.Kind=="Mud"?1.1f:3.4f,71)-.5f;ink.r+=n*.09f;ink.g+=n*.075f;ink.b+=n*.055f;c=Color.Lerp(c,ink,weight);}
   float dirt=FreightNoise2717(x,z,.073f,81.7f);if(dirt>.63f)c=Color.Lerp(c,FreightPigment2718("888D89"),(dirt-.63f)*.75f);
   return FreightConnectedSurface2719(p,c);
  }
  void BuildFreightSurface2717(){
   FreightGroundRegions2716.Add(new FreightGroundRegion2716{Name="FreightWeatheredConcrete",Finish="ContinuousColdConcreteLocalOilAndRunoff2718",Area=FreightGroundBounds2716});
   // The worn strips follow the actual wall, cargo and rain runoff, not tile rows.
   FreightSurfaceRegion2717("TruckUndersideEarth","Mud","92704E",5.6f,146.2f,7.1f,145.1f,10.2f,145.8f,13.1f,149.3f,12.8f,154.9f,9.5f,156,5.7f,153.8f);
   FreightSurfaceRegion2717("NorthBoundarySoil","Mud","896848",5.5f,167.5f,8,165.9f,12.8f,168.1f,14.1f,172.5f,12.7f,174.8f,5.4f,174.8f);
   FreightSurfaceRegion2717("DutySideGravel","Gravel","A18B6B",20.9f,143.3f,23.2f,142.9f,24.9f,146.4f,24.3f,149.7f,25.2f,154.8f,21.3f,155.5f,20.7f,150.6f);
   FreightSurfaceRegion2717("WarehouseRainMud","Mud","806348",44.2f,157.3f,45.5f,158,45.4f,162.1f,44.8f,164.4f,45.5f,168.7f,43.6f,170.2f,42.8f,168.8f);
   FreightSurfaceRegion2717("EastDrainDust","Dust","AC936E",48.7f,143.2f,50,142.8f,50,174.9f,48.6f,173.8f,48.75f,167.7f,49.2f,161.8f,48.7f,155.1f);
   FreightSurfaceRegion2717("NorthDispatchDust","Dust","B59C79",27.9f,169.4f,32.8f,169.3f,35.4f,170.3f,41.3f,169.4f,44.1f,170.1f,43.1f,172.6f,38.8f,172.2f,33.5f,172.4f,29.2f,172.7f);
   FreightSurfaceRegion2717("DutyRepairPatch","ConcreteRepair","BCBCB6",40.9f,144.1f,44.1f,143.8f,44.8f,146.4f,44.3f,149.1f,41.2f,148.5f);
   FreightSurfaceRegion2717("OldGateTarmac","AsphaltRepair","707775",7.2f,158.1f,11.5f,157.5f,14.9f,158.5f,15.5f,163.8f,11.7f,164.9f,7.5f,163.5f);
   FreightCoolSurfacePalette2718();
   // One carrier polygon. Continuous world-space grain is baked per pixel in 2718;
   // colour-per-cell sampling produced visible checkerboards at every .43 metres.
   FreightGroundPaint2716(CargoGroundCorners2715(FreightGroundBounds2716),FreightPigment2718("AEB1B3"),.230f,false);
   // Irregular crazing and construction repairs are local networks. No field grid.
   FreightSurfaceCrack2717(new[]{new Vector2(23.4f,143.4f),new Vector2(23.7f,144.8f),new Vector2(23.5f,145.4f),new Vector2(24.4f,146.3f),new Vector2(24.2f,147.8f)},.05f,"6E7066");
   FreightSurfaceCrack2717(new[]{new Vector2(42.1f,145.6f),new Vector2(42.5f,146.2f),new Vector2(42.8f,147.6f),new Vector2(43.3f,148.1f),new Vector2(43.6f,149.3f)},.045f,"6F746F");
   FreightSurfaceCrack2717(new[]{new Vector2(27.8f,171.7f),new Vector2(29.1f,171.1f),new Vector2(30.1f,171.6f),new Vector2(31.2f,171.3f),new Vector2(31.7f,172.1f)},.055f,"6D7066");
   FreightSurfaceCrack2717(new[]{new Vector2(9.6f,142.4f),new Vector2(10.8f,142.8f),new Vector2(11.6f,142.4f),new Vector2(12.5f,143.1f)},.04f,"72776F");
   FreightSurfaceCrack2717(new[]{new Vector2(20.8f,165.5f),new Vector2(22.1f,166.1f),new Vector2(22.5f,167.4f),new Vector2(23.4f,167.8f)},.06f,"686D62");
   FreightSurfaceCrack2717(new[]{new Vector2(22.3f,152.1f),new Vector2(23.5f,152.7f),new Vector2(24.1f,152.6f),new Vector2(24.5f,153.4f)},.04f,"6D7165");
   // Bent paired wheel scars end at the crash; turning rubs continue to the gate.
   FreightSurfaceWheel2717(new[]{new Vector2(12.5f,145),new Vector2(12.2f,146.3f),new Vector2(11.5f,148.2f),new Vector2(10.2f,150.4f)},.64f,"5B5042");
   FreightSurfaceWheel2717(new[]{new Vector2(24.6f,163.1f),new Vector2(22.8f,162.6f),new Vector2(20.5f,161.1f),new Vector2(17.7f,160.3f),new Vector2(13.2f,160.5f),new Vector2(8.1f,161.1f)},1.02f,"60625A");
   FreightSurfaceWheel2717(new[]{new Vector2(44.5f,170.2f),new Vector2(43.5f,171.0f),new Vector2(41.8f,171.3f),new Vector2(39.9f,171.0f)},.36f,"807462");
   // One crash spill with a long irregular tail, not a repeated identical stamp.
   FreightSurfaceBlob2717("CrashOilPool","Oil","373832",new Vector2(9.6f,149.7f),2.4f,1.0f,13,.251f);
   FreightSurfaceBlob2717("OilSeepTail","Oil","514639",new Vector2(10.4f,153.5f),.8f,2.1f,-17,.252f);
   FreightSurfaceBlob2717("TruckRainwater","Water","556D73",new Vector2(6.8f,154.0f),1.0f,.48f,7,.253f);
   FreightSurfaceBlob2717("EastDrainPuddle","Water","617C82",new Vector2(49.2f,159.4f),.50f,1.8f,-5,.253f);
   FreightSurfaceBlob2717("WarehouseWetMud","Mud","665240",new Vector2(44.8f,167.3f),.53f,1.6f,11,.250f);
   // A low-reflectance pale edge suggests shallow water without another plane.
   FreightGroundLine2716(FreightSurfacePoint2717(6.05f,154.2f),FreightSurfacePoint2717(6.65f,154.44f),.065f,"8F9DA0",.254f);
   FreightGroundLine2716(FreightSurfacePoint2717(49.45f,158.5f),FreightSurfacePoint2717(49.4f,159.8f),.045f,"99A8AC",.254f);
   FreightCoolSurfacePalette2718();FreightSurfaceAggregate2717();FreightSurfaceEdgePlants2717();
  }
  void FreightSurfaceCrack2717(Vector2[] design,float width,string hex){for(int i=1;i<design.Length;i++){var a=FreightSurfacePoint2717(design[i-1].x,design[i-1].y);var b=FreightSurfacePoint2717(design[i].x,design[i].y);FreightGroundLine2716(a,b,width,hex,.246f);if(i%2==0){var center=Vector2.Lerp(a,b,.6f);FreightGroundLine2716(center,center+new Vector2(.28f,-.36f),width*.6f,hex,.246f);}}}
  void FreightSurfaceWheel2717(Vector2[] design,float spacing,string hex){for(int i=1;i<design.Length;i++){var a=FreightSurfacePoint2717(design[i-1].x,design[i-1].y);var b=FreightSurfacePoint2717(design[i].x,design[i].y);var n=new Vector2(-(b-a).y,(b-a).x).normalized*spacing;for(int side=-1;side<=1;side+=2){FreightGroundLine2716(a+n*side,b+n*side,.10f,hex,.247f);int steps=Mathf.CeilToInt((b-a).magnitude/.31f);for(int j=0;j<steps;j++){var p=Vector2.Lerp(a,b,j/(float)steps)+n*side;FreightGroundLine2716(p-n*.055f,p+n*.055f,.045f,hex,.247f);}}}}
  void FreightSurfaceBlob2717(string name,string kind,string hex,Vector2 design,float rx,float rz,float angle,float priority){const int count=17;var polygon=new Vector2[count];var center=FreightSurfacePoint2717(design.x,design.y);float a=angle*Mathf.Deg2Rad;for(int i=0;i<count;i++){float t=i*Mathf.PI*2/count,shape=.84f+.18f*Mathf.Sin(i*1.7f+design.x);var p=new Vector2(Mathf.Cos(t)*rx*shape,Mathf.Sin(t)*rz*shape);polygon[i]=center+new Vector2(p.x*Mathf.Cos(a)-p.y*Mathf.Sin(a),p.x*Mathf.Sin(a)+p.y*Mathf.Cos(a));}var extent=CargoGroundExtent2715(polygon);FreightSurfaceFeatures2717.Add(new FreightSurfaceFeature2717{Name=name,Kind=kind,PaletteHex=hex,Polygon=polygon,Area=extent});}
  void FreightSurfaceAggregate2717(){
   // Different material bands receive different fine texture; deterministic seed
   // does not imply an evenly spaced repeated stamp.
   var rng=new System.Random(271717);foreach(var f in FreightSurfaceFeatures2717){if(f.Kind!="Mud"&&f.Kind!="Gravel"&&f.Kind!="Dust")continue;int count=Mathf.CeilToInt(f.Area.width*f.Area.height*(f.Kind=="Gravel"?3:1.2f));for(int i=0;i<count;i++){var p=new Vector2(Mathf.Lerp(f.Area.xMin,f.Area.xMax,(float)rng.NextDouble()),Mathf.Lerp(f.Area.yMin,f.Area.yMax,(float)rng.NextDouble()));if(!InPolygon(p,f.Polygon))continue;float w=.025f+(float)rng.NextDouble()*.055f,h=.02f+(float)rng.NextDouble()*.045f;Color c=FreightPigment2718(f.Kind=="Mud"?(i%3==0?"9B9C90":"777E78"):(i%3==0?"BCC2BC":i%3==1?"7C8682":"9CA7A4"));FreightGroundPaint2716(new[]{p+new Vector2(-w,-h),p+new Vector2(w*.7f,-h*.7f),p+new Vector2(w,h*.4f),p+new Vector2(-w*.4f,h)},c,.245f,true);}}
   // Exposed cement grit is cool and sparse, concentrated in repairs and rough edges.
   for(int i=0;i<440;i++){var p=new Vector2(Mathf.Lerp(-73.2f,-29.8f,(float)rng.NextDouble()),Mathf.Lerp(61,101,(float)rng.NextDouble()));if(FreightGroundBlend2716(p)<.35f)continue;float size=.025f+(float)rng.NextDouble()*.065f;FreightGroundPaint2716(new[]{p+new Vector2(-size,0),p+new Vector2(size,-size*.7f),p+new Vector2(size*.4f,size)},FreightPigment2718(i%2==0?"C6C7BF":"90928A"),.242f,true);}
  }
  void FreightSurfaceEdgePlants2717(){
   var plants=new[]{new Vector3(5.7f,144.8f,0),new Vector3(5.6f,169.5f,0),new Vector3(7.1f,174.3f,0),new Vector3(11.8f,174.4f,0),new Vector3(29.2f,169.4f,0),new Vector3(43.7f,169.5f,0),new Vector3(49.5f,146.7f,0),new Vector3(49.5f,162.5f,0),new Vector3(49.4f,171.1f,0),new Vector3(24.65f,153.9f,0)};
   for(int i=0;i<plants.Length;i++){var g=FreightObject2716("FreightEdgeWeeds2717_"+i,PortPoint(plants[i].x,plants[i].y));var art=new WorkshopWastelandArt(this);var p=g.transform.position;for(int stem=0;stem<9;stem++){float angle=stem*2.399f+i*.8f,height=.10f+(stem%3)*.055f;var a=p+new Vector3(Mathf.Sin(angle)*.14f,.055f,Mathf.Cos(angle)*.13f);var b=a+new Vector3(Mathf.Sin(angle)*.07f,height,Mathf.Cos(angle)*.06f);art.Stroke("DryWeedBlades",stem%3==0?"9C9467":stem%3==1?"68754C":"827D51",a,b,.035f,Vector3.forward,false);}art.Bake(g,g.AddComponent<WorkshopArtResources>()); // Art vertices are world-space; keep this baked group's transform at origin.
    g.transform.position=Vector3.zero;FreightEdgeGrass2717.Add(g);
   }
  }
 }
}
