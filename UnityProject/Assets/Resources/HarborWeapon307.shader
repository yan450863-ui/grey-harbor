Shader "GreyHarbor/WeaponDepth307" {
 Properties {
  [PerRendererData] _MainTex("Sprite",2D)="white" {}
  _Color("Tint",Color)=(1,1,1,1)
  _Anchor("Depth anchor",Vector)=(0,0,0,0)
  _DepthBias("Layer depth bias",Float)=0
  _CharacterMask("Own character stencil",Float)=2
 }
 SubShader {
 Tags {"Queue"="AlphaTest+11" "RenderType"="TransparentCutout" "CanUseSpriteAtlas"="True"}
 Stencil {Ref [_CharacterMask] WriteMask 3 Comp Always Pass Replace ZFail Keep}
 Cull Off Lighting Off ZWrite On Blend SrcAlpha OneMinusSrcAlpha
 Pass {
 CGPROGRAM
 #pragma vertex vert
 #pragma fragment frag
 #include "UnityCG.cginc"
 struct appdata {float4 vertex:POSITION;float2 uv:TEXCOORD0;fixed4 color:COLOR;};
 struct v2f {float4 pos:SV_POSITION;float2 uv:TEXCOORD0;fixed4 color:COLOR;};
 sampler2D _MainTex;float4 _Anchor;fixed4 _Color;float _DepthBias;
 v2f vert(appdata v) {
  v2f o;o.pos=UnityObjectToClipPos(v.vertex);
  float3 cameraBack=UNITY_MATRIX_V[2].xyz;
  float3 cameraUp=UNITY_MATRIX_V[1].xyz;
  float3 world=mul(unity_ObjectToWorld,v.vertex).xyz;
  // Upper gun pixels follow the upright character plane, ahead of the head.
  // Lower pixels retain the hold-plane floor so downward fire never sinks into ground.
  float height=max(0,dot(world-_Anchor.xyz,cameraUp)/max(cameraUp.y,.1));
  float4 projected=mul(UNITY_MATRIX_VP,float4(_Anchor.xyz+float3(0,height,0)+cameraBack*_DepthBias,1));
  o.pos.z=projected.z/projected.w*o.pos.w;o.uv=v.uv;o.color=v.color*_Color;return o;
 }
 fixed4 frag(v2f i):SV_Target {fixed4 c=tex2D(_MainTex,i.uv)*i.color;clip(c.a-.1);return c;}
 ENDCG
 }
 }
}
