Shader "GreyHarbor/ActorStencil307" {
 Properties {
  [PerRendererData] _MainTex("Sprite",2D)="white" {}
  _Color("Tint",Color)=(1,1,1,1)
  _Anchor("Depth anchor",Vector)=(0,0,0,0)
  _DepthBias("Layer depth bias",Float)=0
  _CharacterMask("Own character stencil",Float)=1
 }
 SubShader {
 Tags {"Queue"="AlphaTest+10" "RenderType"="TransparentCutout" "CanUseSpriteAtlas"="True"}
 Stencil {Ref [_CharacterMask] Comp Always Pass Replace ZFail Keep}
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
  float3 world=mul(unity_ObjectToWorld,v.vertex).xyz;
  float3 cameraUp=UNITY_MATRIX_V[1].xyz;
  float3 cameraBack=UNITY_MATRIX_V[2].xyz;
  // Preserve the billboard silhouette, but reconstruct depth on an upright plane.
  // A helmet has its own height/depth; it must not inherit the feet's depth.
  float height=dot(world-_Anchor.xyz,cameraUp)/max(cameraUp.y,.1);
  float3 logical=_Anchor.xyz+float3(0,height,0)+cameraBack*_DepthBias;
  float4 projected=mul(UNITY_MATRIX_VP,float4(logical,1));
  o.pos.z=projected.z/projected.w*o.pos.w;o.uv=v.uv;o.color=v.color*_Color;return o;
 }
 fixed4 frag(v2f i):SV_Target {fixed4 c=tex2D(_MainTex,i.uv)*i.color;clip(c.a-.1);return c;}
 ENDCG
 }
 }
}
