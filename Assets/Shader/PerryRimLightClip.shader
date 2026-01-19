Shader "Perry/RimLightClip" {
Properties {
 _Color ("Main Color", Color) = (1,1,1,1)
 _MainTex ("Base (RGB)", 2D) = "white" {}
 _Ramp ("Toon Ramp (RGB)", 2D) = "gray" {}
 _RimColor ("Rim Color", Color) = (0.8,0.8,0.8,0.6)
 _RimPower ("Rim Power", Float) = 1.4
 _SColor ("Shadow Color", Color) = (0,0,0,1)
 _LColor ("Highlight Color", Color) = (0.5,0.5,0.5,1)
 _ClipMinX ("Clip Min X", Float) = 8.5
 _ClipMinY ("Clip Min Y", Float) = 2.6
 _Cutoff ("alpha cutoff", Range(0,1)) = 0.5
}
	//DummyShaderTextExporter
	
	SubShader{
		Tags { "RenderType" = "Opaque" }
		LOD 200
		CGPROGRAM
#pragma surface surf Lambert
#pragma target 3.0
		sampler2D _MainTex;
		struct Input
		{
			float2 uv_MainTex;
		};
		void surf(Input IN, inout SurfaceOutput o)
		{
			float4 c = tex2D(_MainTex, IN.uv_MainTex);
			o.Albedo = c.rgb;
		}
		ENDCG
	}
}