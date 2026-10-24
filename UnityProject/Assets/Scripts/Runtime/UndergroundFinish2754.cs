using System.Collections.Generic;
using UnityEngine;
namespace GreyHarbor { public sealed partial class RaidMap {
 public Texture2D UndergroundFloorTexture2754,UndergroundWallTexture2754;
 public readonly List<GameObject> UndergroundDecor2754=new List<GameObject>();
 static float UGNoise2754(int x,int z){unchecked{uint h=(uint)(x*73856093)^(uint)(z*19349663)^2754u;h^=h>>13;h*=1274126177;return(h&65535)/65535f;}}
 static float UGSoftNoise2755(float x,float z){int ix=Mathf.FloorToInt(x),iz=Mathf.FloorToInt(z);float fx=Mathf.SmoothStep(0,1,x-ix),fz=Mathf.SmoothStep(0,1,z-iz);return Mathf.Lerp(Mathf.Lerp(UGNoise2754(ix,iz),UGNoise2754(ix+1,iz),fx),Mathf.Lerp(UGNoise2754(ix,iz+1),UGNoise2754(ix+1,iz+1),fx),fz);}
 void FinishUnderground2754(){
  UndergroundFloorTexture2754=UGTexture2754(false);UndergroundWallTexture2754=UGTexture2754(true);
  foreach(var view in Root.GetComponentsInChildren<MeshRenderer>()){
   bool floor=view.name=="UndergroundUnionFloor2752",wall=view.name=="UndergroundUnionWalls2752";if(!floor&&!wall)continue;
   var mesh=view.GetComponent<MeshFilter>().sharedMesh;var vertices=mesh.vertices;var uv=new Vector2[vertices.Length];
   if(floor){for(int i=0;i<vertices.Length;i++)uv[i]=new Vector2((vertices[i].x-Bounds.xMin)/Bounds.width,(vertices[i].z-Bounds.yMin)/Bounds.height);}
   else for(int i=0;i<vertices.Length;i+=4){float w=Vector3.Distance(vertices[i],vertices[i+3]),h=Vector3.Distance(vertices[i],vertices[i+1]);uv[i]=Vector2.zero;uv[i+1]=new Vector2(0,h/4);uv[i+2]=new Vector2(w/4,h/4);uv[i+3]=new Vector2(w/4,0);}
   mesh.uv=uv;var life=view.gameObject.AddComponent<UnloadingFloor2725>();life.Texture=floor?UndergroundFloorTexture2754:UndergroundWallTexture2754;
   life.Material=new Material(GreyHarbor.FlashlightShaders2763.Load("HarborWorld")){name=floor?"UndergroundDampConcrete2754":"UndergroundMasonry2754",color=Color.white,mainTexture=life.Texture};life.Material.SetFloat("_UseStableGround",1);life.Material.SetFloat("_SolidFog",wall?1:0);life.Material.SetFloat("_Detail",0);view.sharedMaterial=life.Material;
  }
  FinishInteriorDoors2743();BuildUndergroundLayout2754();Physics.SyncTransforms();
 }
 Texture2D UGTexture2754(bool wall){
  int n=wall?128:1440;var pixels=new Color32[n*n];
  for(int z=0;z<n;z++)for(int x=0;x<n;x++){
   Color c;if(wall){float y=z/(float)n*4;int row=z/12;bool seam=z%12<1||(x+(row%2)*16)%32<1;
    c=PixelArt.Hex(y<.9f?"3F5646":y<2.3f?"697260":"80816E");c*=.91f+.15f*UGNoise2754(x/32,row)+.06f*UGNoise2754(x,z);if(seam)c*=.75f;
    if(UGNoise2754(x/7,z/10)>.66f)c=Color.Lerp(c,PixelArt.Hex("645136"),.35f);
    float runoff=Mathf.Pow(Mathf.Max(0,Mathf.Sin(x*.17f+Mathf.Sin(x*.04f)*3)),8);c=Color.Lerp(c,PixelArt.Hex("334A3C"),runoff*Mathf.Clamp01(1-y/3)*.6f);
   }else{float wx=Bounds.xMin+(x+.5f)*Bounds.width/n,wz=Bounds.yMin+(z+.5f)*Bounds.height/n;
    c=PixelArt.Hex("687063")*(.94f+.09f*UGNoise2754(x,z)+.065f*(UGNoise2754(x/10,z/10)-.5f));
    if((Mathf.Repeat(wx+60,3)<.055f||Mathf.Repeat(wz+60,3)<.055f)&&UGNoise2754(x/35,z/35)>.6f)c*=.91f;
    float damp=Mathf.Sin(wx*.53f+Mathf.Sin(wz*.31f))*Mathf.Sin(wz*.44f+Mathf.Sin(wx*.18f));if(damp>.40f)c=Color.Lerp(c,PixelArt.Hex("334A3D"),Mathf.Clamp01((damp-.40f)*2));
    float muck=UGSoftNoise2755(wx*.55f,wz*.45f);if(muck>.61f)c=Color.Lerp(c,PixelArt.Hex("59503C"),(muck-.61f)*1.4f);
    if(UGNoise2754(x/27,z/19)>.93f&&Mathf.Abs(Mathf.Sin(wx*2.7f+wz*.9f))<.055f)c*=.65f;
   }c.a=1;pixels[z*n+x]=c;
  }
  var t=new Texture2D(n,n,TextureFormat.RGBA32,true){name=wall?"UndergroundBrickDamp2754":"UndergroundConcreteWet2754",filterMode=FilterMode.Trilinear,anisoLevel=4,wrapMode=wall?TextureWrapMode.Repeat:TextureWrapMode.Clamp};t.SetPixels32(pixels);t.Apply(true,false);return t;
 }
 void BuildUndergroundLayout2754(){
  var rng=new System.Random((RaidGame.Instance?RaidGame.Instance.RaidSeed:2754)^2754);int index=0;
  var looks=new[]{ContainerLook.Toolbox,ContainerLook.WeldingToolbox,ContainerLook.ElectricalCabinet,ContainerLook.SupplyLocker,ContainerLook.WaterproofTransitCase,ContainerLook.RescueCabinet,ContainerLook.CargoCrate,ContainerLook.PrecisionCase};
  var models=new List<Vector3>();
  int zone=0;var layoutRandom=new System.Random(2755);
  foreach(var area in UndergroundOpenAreas2753){for(int slot=0;slot<4;slot++){
   int edge=(zone+slot*3)%4;Vector3 model=Vector3.zero,stand=Vector3.zero;float yaw=0;bool found=false;
   for(int attempt=0;attempt<80&&!found;attempt++){
    int e=(edge+attempt/20)%4;float along=.12f+(float)layoutRandom.NextDouble()*.76f;
    model=e<2?new Vector3(e==0?area.xMin+1:area.xMax-1,0,Mathf.Lerp(area.yMin,area.yMax,along)):new Vector3(Mathf.Lerp(area.xMin,area.xMax,along),0,e==2?area.yMin+1:area.yMax-1);
    yaw=(e==0?270:e==1?90:e==2?180:0)+(float)layoutRandom.NextDouble()*14-7;
    stand=model+Quaternion.Euler(0,yaw,0)*Vector3.back*1.35f;
    if(UGNearRoutes2754(model,1.15f)||!UndergroundFloorContains2752(stand,.4f)||Physics.CheckCapsule(stand+Vector3.up*.4f,stand+Vector3.up*1.4f,.45f,SceneObstacle.MovementMask,QueryTriggerInteraction.Ignore))continue;
    bool close=false;foreach(var m in models)if(Vector3.Distance(m,model)<2.7f){close=true;break;}if(close)continue;found=true;
   }
   if(!found)throw new System.InvalidOperationException("No safe underground container placement "+zone+"/"+slot);
   var loot=AddContainer(stand,model,looks[index%looks.Length],SupplyType.Tools,rng,HarborDistrict.Repair,yaw,false,ContainerTier.Common,DropRegion230.Repair);loot.Name="地下检修"+loot.Name+" "+(++index);FreightPlaceOnFloor2717(loot.View);models.Add(model);
  }zone++;}
  // Continuous wall services, occasional valves, rather than identical spaced exhibits.
  for(int routeIndex=0;routeIndex<2;routeIndex++){var route=UndergroundRoutes2752[routeIndex];for(int j=1;j<route.Length;j++){var d=route[j]-route[j-1];float length=d.magnitude;if(length<8)continue;var side=Vector3.Cross(Vector3.up,d.normalized)*(routeIndex==0?1.22f:-1.22f);float yaw=Mathf.Atan2(-d.z,d.x)*Mathf.Rad2Deg;
   for(float offset=1.4f;offset<length-1;offset+=2.55f){var p=route[j-1]+d.normalized*offset+side;UndergroundDecor2754.Add(UndergroundProps2754.Build(this,"pipe-large",p,yaw));}
   var v=Vector3.Lerp(route[j-1],route[j],.43f)+side;UndergroundDecor2754.Add(UndergroundProps2754.Build(this,"valve",v+Vector3.up*.35f,yaw));
  }}
  string[][] themes={new[]{"pump","hose-coil","pressure-gauge","spare-parts","broken-fan"},new[]{"electric-cabinet","cable-reel","copper-junction","vent-box","spare-parts"},new[]{"workbench","tool-rack","service-cart","spare-parts","hose-coil"},new[]{"pump","spare-parts","broken-fan","hose-coil","drain-channel"},new[]{"vent-box","service-cart","broken-fan","cable-reel","spare-parts"},new[]{"workbench","tool-rack","electric-cabinet","cable-reel","spare-parts"}};
  zone=0;foreach(var area in UndergroundOpenAreas2753){int placed=0;
   var anchors=new[]{new Vector2(area.xMin+3,area.yMin+3),new Vector2(area.xMax-3,area.yMax-3),new Vector2(area.xMin+3,area.yMax-3)};
   for(int attempt=0;attempt<180&&placed<10;attempt++){
    var anchor=anchors[attempt%anchors.Length];var p=new Vector3(anchor.x+((float)layoutRandom.NextDouble()-.5f)*5,0,anchor.y+((float)layoutRandom.NextDouble()-.5f)*5);
    string kind=themes[zone][placed%themes[zone].Length];var fp=UndergroundProps2754.Footprint(kind);float radius=Mathf.Max(.4f,fp.magnitude/2+.1f);
    if(!UndergroundFloorContains2752(p,radius)||UGNearRoutes2754(p,radius+.5f))continue;
    bool occupied=false;foreach(var m in models)if(Vector3.Distance(m,p)<radius+.9f){occupied=true;break;}if(!occupied)foreach(var l in Loot)if(Vector3.Distance(l.Position,p)<radius+.7f){occupied=true;break;}
    if(occupied||Physics.CheckCapsule(p+Vector3.up*.4f,p+Vector3.up*1.4f,radius,SceneObstacle.MovementMask,QueryTriggerInteraction.Ignore))continue;
    float yaw=(float)layoutRandom.NextDouble()*85-42.5f;var view=UndergroundProps2754.Build(this,kind,p,yaw);UndergroundDecor2754.Add(view);models.Add(p);placed++;
   }
   // Flush embedded drains stay out of physics; use irregular, purposeful orientations.
   var drain=new Vector3(area.center.x+area.width*.18f,0,area.center.y-area.height*.18f);UndergroundDecor2754.Add(UndergroundProps2754.Build(this,"drain-channel",drain,zone%2==0?90:0));
   zone++;
  }

 }
 bool UGNearRoutes2754(Vector3 p,float distance){foreach(var route in UndergroundRoutes2752)for(int i=1;i<route.Length;i++){var d=route[i]-route[i-1];float t=Mathf.Clamp01(Vector3.Dot(p-route[i-1],d)/d.sqrMagnitude);if(Vector3.Distance(p,route[i-1]+d*t)<distance)return true;}return false;}
}}
