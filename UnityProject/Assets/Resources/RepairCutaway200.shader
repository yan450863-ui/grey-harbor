Shader "GreyHarbor/RepairCutaway200" {
 Properties { _Color("Color",Color)=(1,1,1,1) _SolidFog("Solid surface",Float)=0 _Detail("Wear detail",Float)=0 _MainTex("Stable ground paint",2D)="white"{} _UseStableGround("Use ground paint",Float)=0 }
 SubShader {
 Tags { "RenderType"="Opaque" "Queue"="Geometry" }
 CGPROGRAM
 #pragma surface surf Lambert fullforwardshadows
 #pragma target 3.0
 sampler2D _HarborVisibility,_HarborRayDepth,_MainTex;
 float _UseStableGround;
 float4 _RepairPlayer200;
 float4 _HarborMapRect,_HarborEye;
 float _HarborRoomGate,_HarborVisionRange,_HarborConeCos,_HarborConeInnerCos;
 float _HarborFogEnabled,_SolidFog,_Detail,_HarborMemoryBrightness;
 fixed4 _Color;
 struct Input {float3 worldPos;float2 uv_MainTex;};
 void surf(Input IN,inout SurfaceOutput o) {
   // Local player silhouette cutaway. It never changes the collider or room fog.
   float3 relative=IN.worldPos-_RepairPlayer200.xyz;
   float horizontal=dot(relative,UNITY_MATRIX_V[0].xyz);
   float vertical=dot(relative,UNITY_MATRIX_V[1].xyz);
   float towardsCamera=dot(relative,UNITY_MATRIX_V[2].xyz);
   float hole=horizontal*horizontal/(.72*.72)+(vertical-.68)*(vertical-.68)/(.94*.94);
   if(IN.worldPos.y>.26 && towardsCamera>.08 && hole<1)clip(-1);
   float2 delta=IN.worldPos.xz-_HarborEye.xy;
   float distance=length(delta);
   float cosine=dot(delta/max(distance,.0001),_HarborEye.zw);
   // CPU-shared cone with an inward three-degree feather.
   float cone=smoothstep(_HarborConeCos,_HarborConeInnerCos,cosine);
   cone=max(cone,1-smoothstep(.23,.32,distance));
   float angle=frac(atan2(delta.y,delta.x)/6.2831853+1);
   float2 depth=tex2D(_HarborRayDepth,float2(angle,.5)).rg*_HarborVisionRange;
   float boundary=lerp(depth.r,depth.g,_SolidFog);
   float visible=cone*(1-smoothstep(boundary-.035,boundary+lerp(.012,.055,_SolidFog),distance))*(1-smoothstep(_HarborVisionRange-.3,_HarborVisionRange,distance));
   float2 uv=(IN.worldPos.xz-_HarborMapRect.xy)/_HarborMapRect.zw;
   float3 memory=tex2D(_HarborVisibility,uv).rgb;
   float known=lerp(memory.g,memory.b,_SolidFog);
   visible*=lerp(1,step(.5,memory.g),_HarborRoomGate);
   float brightness=lerp(1,lerp(_HarborMemoryBrightness*known,.75,visible),_HarborFogEnabled);
   fixed4 paint=_Color;
   if(_UseStableGround>.5){paint=tex2D(_MainTex,IN.uv_MainTex);clip(paint.a-.1);paint.rgb=saturate(paint.rgb/max(paint.a,.001));}
   float gray=dot(paint.rgb,float3(.299,.587,.114));
   float3 base=lerp(paint.rgb,gray.xxx,.18*(1-visible)*_HarborFogEnabled);
   float3 cell=floor(IN.worldPos*18);float grain=frac(sin(dot(cell,float3(12.9898,37.719,78.233)))*43758.5453);base*=1-_Detail*grain;
   o.Albedo=base*brightness;o.Emission=base*brightness*.12;o.Alpha=1;
 }
 ENDCG
 }
 Fallback "Diffuse"
}
