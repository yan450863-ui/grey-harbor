Shader "GreyHarbor/DoorFreight2743" {
 Properties { _ScopeLight335("Circular scope light support",Float)=1  _HeightLight2763("Height aware light",Float)=1  _FreightInnerRect2718("Inner memory sampling rectangle",Vector)=(-8.07,-8.07,8.07,8.07) _Color("Color",Color)=(1,1,1,1) _SolidFog("Solid surface",Float)=1 _Detail("Wear detail",Float)=0 _MainTex("Stable ground paint",2D)="white"{} _UseStableGround("Use ground paint",Float)=0 }
 SubShader {
 Tags { "RenderType"="Opaque" "Queue"="Geometry" }
 CGPROGRAM
 #pragma surface surf Lambert noshadow
 #pragma target 3.0
 sampler2D _MainTex; float _UseStableGround;
 float _HarborScopeBlend335;float4 _HarborScopeCircle335,_HarborScopeViewport335;
 sampler2D _HarborVisibility,_HarborRayDepth;
 float4 _FreightInnerRect2718,_HarborMapRect,_HarborEye;
 float _HarborRoomGate,_HarborVisionRange,_HarborConeCos,_HarborConeInnerCos;
 float _HarborLightTop2763,_HeightLight2763;
 float _HarborFogEnabled,_SolidFog,_Detail,_HarborMemoryBrightness;
 fixed4 _Color;
 struct Input {float3 worldPos;float2 uv_MainTex;};
 void surf(Input IN,inout SurfaceOutput o){
  float2 delta=IN.worldPos.xz-_HarborEye.xy;float distance=length(delta);
  float cosine=dot(delta/max(distance,.0001),_HarborEye.zw);
  float cone=smoothstep(_HarborConeCos,_HarborConeInnerCos,cosine);
  cone=max(cone,1-smoothstep(.23,.32,distance));
   // Replace the flashlight cone with the scope circle, preserving native fog,
   // room memory, ray occlusion, overhead cutoff and the original range.
   float4 screen335=ComputeScreenPos(mul(UNITY_MATRIX_VP,float4(IN.worldPos,1)));
   float2 pixel335=screen335.xy/max(screen335.w,.0001)*_HarborScopeViewport335.xy;
   float scope335=1-smoothstep(_HarborScopeCircle335.z-1,_HarborScopeCircle335.z,length(pixel335-_HarborScopeCircle335.xy));
   cone=lerp(cone,scope335,saturate(_HarborScopeBlend335));
  float angle=frac(atan2(delta.y,delta.x)/6.2831853+1);
  float2 depth=tex2D(_HarborRayDepth,float2(angle,.5)).rg*_HarborVisionRange;
  float boundary=lerp(depth.r,depth.g,_SolidFog);
  float visible=cone*(1-smoothstep(boundary-.035,boundary+lerp(.012,.055,_SolidFog),distance))*(1-smoothstep(_HarborVisionRange-.3,_HarborVisionRange,distance));
   // Fog memory stays two-dimensional; the live beam cannot project onto overhead roofs.
   visible*=lerp(1,1-smoothstep(_HarborLightTop2763-.15,_HarborLightTop2763+.15,IN.worldPos.y),_HeightLight2763);
  // Wall vertices sit outside the reveal rectangle. Sample the real interior
  // side by a few centimetres, while ray depth still refers to the solid wall.
  // Closed/unopened rooms keep their zero memory; there is no eye-space hole.
  float2 towardEye=normalize(_HarborEye.xy-IN.worldPos.xz+float2(.00001,.00001));
  float2 sampleXZ=clamp(IN.worldPos.xz+towardEye*.26,_FreightInnerRect2718.xy,_FreightInnerRect2718.zw);
  float3 memory=tex2D(_HarborVisibility,(sampleXZ-_HarborMapRect.xy)/_HarborMapRect.zw).rgb;
  float allowed=lerp(1,step(.5,memory.g),_HarborRoomGate);
  float known=lerp(memory.g,memory.b,_SolidFog)*allowed;visible*=allowed;
  float brightness=lerp(1,lerp((_HarborMemoryBrightness+.055)*known,.75,visible),_HarborFogEnabled);
  float3 base=_Color.rgb*(_UseStableGround>.5?tex2D(_MainTex,IN.uv_MainTex).rgb:float3(1,1,1));float gray=dot(base,float3(.299,.587,.114));base=lerp(base,gray.xxx,.14*(1-visible)*_HarborFogEnabled);
  float3 cell=floor(IN.worldPos*18);float grain=frac(sin(dot(cell,float3(12.9898,37.719,78.233)))*43758.5453);base*=1-_Detail*grain;
  o.Albedo=base*brightness;o.Emission=base*brightness*.24;o.Alpha=1;
 }
 ENDCG
 }
 Fallback "Diffuse"
}
