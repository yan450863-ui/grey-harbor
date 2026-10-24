using UnityEngine;
namespace GreyHarbor {
 public sealed class RaidVision {
  public const float Cell=.125f,Range=16,HalfAngle=30;
  public const int RayCount=2048;
  public const float LightTopHeight2763=1.8f;
  public static double LastRefreshMilliseconds2717;
  public readonly float MinX,MinZ;public readonly int Width,Height;
  public static readonly float ConeCos=Mathf.Cos(HalfAngle*Mathf.Deg2Rad);
  public static readonly float ConeInnerCos=Mathf.Cos((HalfAngle-3)*Mathf.Deg2Rad);
  public readonly bool[] Seen,Current,SurfaceSeen;
  readonly Color32[] pixels,miniPixels,miniDisplayPixels2717;
  readonly Color[] rayPixels=new Color[RayCount];
  readonly Vector3[] directions=new Vector3[RayCount];
  readonly int miniWidth2717,miniHeight2717;
  bool worldDirty2717=true;Vector3 runtimeEye2717,runtimeAim2717;int runtimeRevision2717=int.MinValue;bool runtimePose2717,runtimeRoomGate2717;
  public int DepthRefreshCount2717 {get;private set;}
  public readonly Texture2D Texture,MiniMapMask,RayTexture;
  public bool RoomGate; public float MemoryBrightness=.33f;
  public void Bind(){Shader.SetGlobalTexture("_HarborVisibility",Texture);Shader.SetGlobalTexture("_HarborRayDepth",RayTexture);Shader.SetGlobalVector("_HarborMapRect",new Vector4(MinX,MinZ,Width*Cell,Height*Cell));Shader.SetGlobalFloat("_HarborMemoryBrightness",MemoryBrightness);Shader.SetGlobalFloat("_HarborRoomGate",RoomGate?1:0);Shader.SetGlobalFloat("_HarborVisionRange",Range);Shader.SetGlobalFloat("_HarborConeCos",ConeCos);Shader.SetGlobalFloat("_HarborConeInnerCos",ConeInnerCos);}
  public bool RoomAllowed(Vector3 p){if(!RoomGate)return true;int i=Index(p);return i>=0&&i<Seen.Length&&Seen[i];}
  public void RevealArea(Rect area){for(int z=0;z<Height;z++)for(int x=0;x<Width;x++)if(area.Contains(new Vector2(MinX+(x+.5f)*Cell,MinZ+(z+.5f)*Cell))){int i=z*Width+x;Seen[i]=SurfaceSeen[i]=true;pixels[i]=new Color32(0,255,255,255);miniPixels[i]=new Color32(0,0,0,170);}Texture.SetPixels32(pixels);Texture.Apply(false,false);UpdateMiniMap2717();worldDirty2717=false;runtimePose2717=false;}
  Vector3 eye,forward,previousEye;bool hasPrevious;
  bool scopeActive335,scopeDirty335;Camera scopeCamera335;Vector2 scopeCenter335;float scopeRadius335;Matrix4x4 scopeProjection335;Rect scopePixelRect335;
  public void SetScope335(bool active,Camera view,Vector2 center,float radius){
   var projection=view?view.projectionMatrix*view.worldToCameraMatrix:Matrix4x4.identity;
   scopeDirty335|=scopeActive335!=active||(active&&(scopeCenter335!=center||scopeRadius335!=radius||scopeCamera335!=view||scopeProjection335!=projection||scopePixelRect335!=(view?view.pixelRect:new Rect())));
   scopeActive335=active;scopeCamera335=view;scopeCenter335=center;scopeRadius335=radius;scopeProjection335=projection;scopePixelRect335=view?view.pixelRect:new Rect();
  }
  public bool InScope335(Vector3 point){if(!scopeCamera335)return false;var ndc=scopeProjection335.MultiplyPoint(point+Vector3.up*RaidGame.CombatHeight);var screen=new Vector2(scopePixelRect335.x+(ndc.x*.5f+.5f)*scopePixelRect335.width,scopePixelRect335.y+(ndc.y*.5f+.5f)*scopePixelRect335.height);return (screen-scopeCenter335).sqrMagnitude<=scopeRadius335*scopeRadius335;}

  public RaidVision():this(new Rect(-19,-17,38,34)){}
  public RaidVision(Rect bounds) {
   Shader.SetGlobalFloat("_HarborRoomGate",0);Shader.SetGlobalFloat("_HarborMemoryBrightness",.33f);MinX=bounds.xMin;MinZ=bounds.yMin;Width=Mathf.CeilToInt(bounds.width/Cell);Height=Mathf.CeilToInt(bounds.height/Cell);Seen=new bool[Width*Height];Current=new bool[Width*Height];SurfaceSeen=new bool[Width*Height];pixels=new Color32[Width*Height];int stride=Mathf.Max(1,Mathf.CeilToInt(Mathf.Max(Width,Height)/512f));miniWidth2717=Mathf.CeilToInt(Width/(float)stride);miniHeight2717=Mathf.CeilToInt(Height/(float)stride);miniPixels=new Color32[Width*Height];miniDisplayPixels2717=new Color32[miniWidth2717*miniHeight2717];
   Texture=new Texture2D(Width,Height,TextureFormat.RGBA32,false){filterMode=FilterMode.Bilinear,wrapMode=TextureWrapMode.Clamp};
   MiniMapMask=new Texture2D(miniWidth2717,miniHeight2717,TextureFormat.RGBA32,false){filterMode=FilterMode.Bilinear,wrapMode=TextureWrapMode.Clamp};
   RayTexture=new Texture2D(RayCount,1,TextureFormat.RGBAFloat,false){filterMode=FilterMode.Bilinear,wrapMode=TextureWrapMode.Repeat};
   for(int i=0;i<miniPixels.Length;i++)miniPixels[i]=new Color32(0,0,0,255);
   for(int i=0;i<RayCount;i++){float a=(i+.5f)/RayCount*Mathf.PI*2;directions[i]=new Vector3(Mathf.Cos(a),0,Mathf.Sin(a));}
   Bind();
  }
  public void RevealExterior(){MemoryBrightness=.52f;for(int i=0;i<Seen.Length;i++){Seen[i]=SurfaceSeen[i]=true;pixels[i]=new Color32(0,255,255,255);miniPixels[i]=new Color32(0,0,0,120);}Texture.SetPixels32(pixels);Texture.Apply(false,false);UpdateMiniMap2717();Shader.SetGlobalFloat("_HarborMemoryBrightness",.52f);worldDirty2717=false;runtimePose2717=false;}
  void UpdateMiniMap2717(){for(int z=0;z<miniHeight2717;z++)for(int x=0;x<miniWidth2717;x++){int sx=Mathf.Min(Width-1,(int)((x+.5f)*Width/miniWidth2717)),sz=Mathf.Min(Height-1,(int)((z+.5f)*Height/miniHeight2717));miniDisplayPixels2717[z*miniWidth2717+x]=miniPixels[sz*Width+sx];}MiniMapMask.SetPixels32(miniDisplayPixels2717);MiniMapMask.Apply(false,false);}
  public void SetPose(Vector3 p,Vector3 aim){eye=p;forward=aim.normalized;Shader.SetGlobalFloat("_HarborLightTop2763",p.y+LightTopHeight2763);Shader.SetGlobalVector("_HarborEye",new Vector4(p.x,p.z,forward.x,forward.z));}
  bool InCone(Vector3 point){var d=point-eye;d.y=0;float distance=d.magnitude;return distance<=Range&&(scopeActive335?InScope335(point):(distance<=.32f||Vector3.Dot(d/Mathf.Max(distance,.001f),forward)>=ConeCos));}
  public bool LaserPath338(Vector3 point){var d=point-eye;d.y=0;return d.magnitude<=Range&&RoomAllowed(point)&&!Physics.Linecast(eye+Vector3.up*RaidGame.CombatHeight,new Vector3(point.x,eye.y+RaidGame.CombatHeight,point.z),SceneObstacle.SightMask,QueryTriggerInteraction.Ignore);}
  public bool CanSee(Vector3 point){if(!RoomAllowed(point)||!InCone(point))return false;return !Physics.Linecast(eye+Vector3.up*RaidGame.CombatHeight,new Vector3(point.x,eye.y+RaidGame.CombatHeight,point.z),SceneObstacle.SightMask,QueryTriggerInteraction.Ignore);}
  Color Depth(Vector3 point){Vector3 d=point-eye;float u=Mathf.Repeat(Mathf.Atan2(d.z,d.x)/(2*Mathf.PI),1);return rayPixels[Mathf.Clamp(Mathf.FloorToInt(u*RayCount),0,RayCount-1)];}
  public bool SurfaceVisible(Vector3 point){if(!InCone(point))return false;Vector3 d=point-eye;d.y=0;return d.magnitude<=Depth(point).g*Range+.015f;}
  public int Index(Vector3 p){int x=Mathf.FloorToInt((p.x-MinX)/Cell),z=Mathf.FloorToInt((p.z-MinZ)/Cell);return x<0||z<0||x>=Width||z>=Height?-1:z*Width+x;}
  public bool Explored(Vector3 p){int i=Index(p);return i>=0&&Seen[i];}
  public void RevealSpawn(Vector3 position,Vector3 aim){
   // One initial 360-degree survey, still occluded by physical walls.
   Refresh(position,aim,true);Refresh(position,aim);
  }
  public void Refresh(Vector3 position,Vector3 aim,bool survey=false){RefreshCore2717(position,aim,survey,false);runtimePose2717=false;}
  public void RefreshRuntime2717(Vector3 position,Vector3 aim,int layoutRevision){
   bool sameEye=runtimePose2717&&position==runtimeEye2717&&layoutRevision==runtimeRevision2717&&RoomGate==runtimeRoomGate2717;
   if(sameEye&&aim==runtimeAim2717&&!scopeDirty335){Bind();SetPose(position,aim);LastRefreshMilliseconds2717=0;return;}
   RefreshCore2717(position,aim,false,sameEye);runtimeEye2717=position;runtimeAim2717=aim;runtimeRevision2717=layoutRevision;runtimeRoomGate2717=RoomGate;runtimePose2717=true;scopeDirty335=false;
  }
  void RefreshCore2717(Vector3 position,Vector3 aim,bool survey,bool reuseDepth){
   long refreshStart2717=System.Diagnostics.Stopwatch.GetTimestamp();
   Bind();SetPose(position,aim);Vector3 origin=eye+Vector3.up*RaidGame.CombatHeight;
   if(!reuseDepth){DepthRefreshCount2717++;for(int i=0;i<RayCount;i++){
    float entry=Range,exit=Range;
    if(Physics.Raycast(origin,directions[i],out var hit,Range,SceneObstacle.SightMask,QueryTriggerInteraction.Ignore)){
     entry=hit.distance;var bounds=hit.collider.bounds;
     // Only the first blocking object's volume may reveal its upper surfaces.
     float far=Range;Vector3 d=directions[i];
     if(Mathf.Abs(d.x)>.00001f)far=Mathf.Min(far,((d.x>0?bounds.max.x:bounds.min.x)-origin.x)/d.x);
     if(Mathf.Abs(d.z)>.00001f)far=Mathf.Min(far,((d.z>0?bounds.max.z:bounds.min.z)-origin.z)/d.z);
     exit=Mathf.Max(entry,far);
    }
    rayPixels[i]=new Color(entry/Range,exit/Range,0,1);
   }
   RayTexture.SetPixels(rayPixels);RayTexture.Apply(false,false);}
   Vector3 prev=hasPrevious?previousEye:eye;float margin=Range+Cell*2;
   int minX=Mathf.Max(0,Mathf.FloorToInt((Mathf.Min(eye.x,prev.x)-margin-MinX)/Cell)),maxX=Mathf.Min(Width-1,Mathf.CeilToInt((Mathf.Max(eye.x,prev.x)+margin-MinX)/Cell));
   int minZ=Mathf.Max(0,Mathf.FloorToInt((Mathf.Min(eye.z,prev.z)-margin-MinZ)/Cell)),maxZ=Mathf.Min(Height-1,Mathf.CeilToInt((Mathf.Max(eye.z,prev.z)+margin-MinZ)/Cell));
   for(int z=minZ;z<=maxZ;z++)for(int x=minX;x<=maxX;x++){
    int i=z*Width+x;var p=new Vector3(MinX+(x+.5f)*Cell,0,MinZ+(z+.5f)*Cell);Vector3 d=p-eye;d.y=0;
    float distance=d.magnitude;bool cone=(!RoomGate||Seen[i])&&(survey?distance<=Range:InCone(p));if(!cone&&!Current[i])continue;Color depth=cone?Depth(p):Color.clear;
    bool visible=cone&&distance<=depth.r*Range+.01f,surface=cone&&distance<=depth.g*Range+.015f;
    worldDirty2717|=visible&&!Seen[i]||surface&&!SurfaceSeen[i];Current[i]=visible;Seen[i]|=visible;SurfaceSeen[i]|=surface;
    pixels[i]=new Color32((byte)(visible?255:0),(byte)(Seen[i]?255:0),(byte)(SurfaceSeen[i]?255:0),255);
    miniPixels[i]=new Color32(0,0,0,(byte)(visible?55:Seen[i]?170:255));
   }
   previousEye=eye;hasPrevious=true;if(worldDirty2717){Texture.SetPixels32(pixels);Texture.Apply(false,false);worldDirty2717=false;}UpdateMiniMap2717();
   LastRefreshMilliseconds2717=(System.Diagnostics.Stopwatch.GetTimestamp()-refreshStart2717)*1000.0/System.Diagnostics.Stopwatch.Frequency;
  }
  public void Clear(){System.Array.Clear(Seen,0,Seen.Length);System.Array.Clear(Current,0,Current.Length);System.Array.Clear(SurfaceSeen,0,SurfaceSeen.Length);System.Array.Clear(pixels,0,pixels.Length);for(int i=0;i<miniPixels.Length;i++)miniPixels[i]=new Color32(0,0,0,255);worldDirty2717=true;runtimePose2717=false;}
  public void Dispose(){Object.Destroy(Texture);Object.Destroy(MiniMapMask);Object.Destroy(RayTexture);}
 }
}
