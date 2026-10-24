Shader "GreyHarbor/LaserCharacterOverlay307" {
 Properties {[PerRendererData] _MainTex("Laser",2D)="white"{} _Color("Tint",Color)=(1,1,1,1) _Anchor("Character depth anchor",Vector)=(0,0,0,0)}
 SubShader {
  Tags {"Queue"="Transparent+50" "RenderType"="Transparent"}
  Cull Off ZWrite Off Blend SrcAlpha OneMinusSrcAlpha
  CGINCLUDE
  #include "UnityCG.cginc"
  struct appdata {float4 vertex:POSITION;float2 uv:TEXCOORD0;fixed4 color:COLOR;};
  struct v2f {float4 pos:SV_POSITION;float2 uv:TEXCOORD0;fixed4 color:COLOR;float4 screen:TEXCOORD1;};
  sampler2D _MainTex;fixed4 _Color;float4 _Anchor;float _HarborScopeBlend335;float4 _HarborScopeCircle335;
  v2f vert(appdata v){v2f o;o.pos=UnityObjectToClipPos(v.vertex);o.screen=ComputeScreenPos(o.pos);o.uv=v.uv;o.color=v.color*_Color;return o;}
  v2f bodyOverlay(appdata v){
   v2f o=vert(v);float3 world=mul(unity_ObjectToWorld,v.vertex).xyz;
   float3 up=UNITY_MATRIX_V[1].xyz,back=UNITY_MATRIX_V[2].xyz;
   float height=dot(world-_Anchor.xyz,up)/max(up.y,.1);
   float4 depth=mul(UNITY_MATRIX_VP,float4(_Anchor.xyz+float3(0,height,0)+back*.003,1));
   o.pos.z=depth.z/depth.w*o.pos.w;return o;
  }
  v2f gunOverlay(appdata v){
   v2f o=vert(v);float3 back=UNITY_MATRIX_V[2].xyz,up=UNITY_MATRIX_V[1].xyz;
   float3 world=mul(unity_ObjectToWorld,v.vertex).xyz;
   float height=max(1.07,dot(world-_Anchor.xyz,up)/max(up.y,.1));
   float4 depth=mul(UNITY_MATRIX_VP,float4(_Anchor.xyz+float3(0,height,0)+back*.05,1));
   o.pos.z=depth.z/depth.w*o.pos.w;return o;
  }
  fixed4 frag(v2f i):SV_Target{float2 pixel=(i.screen.xy/i.screen.w)*_ScreenParams.xy;float outside=step(_HarborScopeCircle335.z,length(pixel-_HarborScopeCircle335.xy));fixed4 c=tex2D(_MainTex,i.uv)*i.color;c.a*=1-outside*_HarborScopeBlend335;return c;}
  ENDCG
  // Outside character pixels, the beam retains its normal world depth.
  Pass {ZTest LEqual Stencil {Ref 0 ReadMask 3 Comp Equal Pass Keep}
   CGPROGRAM
   #pragma vertex vert
   #pragma fragment frag
   ENDCG
  }
  // Reconstruct the actor depth at this screen pixel instead of disabling depth testing.
  // Foreground geometry can render after sprites, so a stencil match alone is insufficient.
  Pass {ZTest LEqual Stencil {Ref 1 ReadMask 3 Comp Equal Pass Keep}
   CGPROGRAM
   #pragma vertex bodyOverlay
   #pragma fragment frag
   ENDCG
  }
  Pass {ZTest LEqual Stencil {Ref 2 ReadMask 3 Comp Equal Pass Keep}
   CGPROGRAM
   #pragma vertex gunOverlay
   #pragma fragment frag
   ENDCG
  }
 }
}
