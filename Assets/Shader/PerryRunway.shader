Shader "Perry/Runway" {
Properties {
 _Color ("Main Color", Color) = (0.5,0.5,0.5,1)
 _DepthColor ("Depth Color", Color) = (0.5,0,0.5,1)
 _MainTex ("Base (RGB) Trans (A)", 2D) = "white" {}
 _ScreenTex ("Screen Texture", 2D) = "white" {}
 _GradientScale ("Gradient Scale", Float) = 0.0004
 _GradientOffset ("Gradient Offset", Float) = 0
}

// this is a reimplementation, it's not perfectly accurate but AssetRipper was not able to export the shader properly.
// This looks close enough in my opinion.

SubShader {
	Tags { "RenderType" = "Opaque" }
	LOD 200
	
	CGPROGRAM
	#pragma surface surf Lambert
	#pragma target 3.0
	
	sampler2D _MainTex;
	sampler2D _ScreenTex;
	fixed4 _Color;
	fixed4 _DepthColor;
	float _GradientScale;
	float _GradientOffset;
	
	struct Input {
		float2 uv_MainTex;
		float3 worldPos;
	};
	
	void surf(Input IN, inout SurfaceOutput o) {
		fixed4 c = tex2D(_MainTex, IN.uv_MainTex);

		float3 viewDir = IN.worldPos - _WorldSpaceCameraPos;
		float depth = length(viewDir);
		
		float gradientValue = (depth * _GradientScale) + _GradientOffset;
		
		fixed4 gradient;
		if (gradientValue >= 1.0) {
			gradient = fixed4(1, 1, 1, 1);
		} else {
			float gradientV = saturate(gradientValue);
			gradient = tex2D(_ScreenTex, float2(0.5, gradientV));
		}
		
		o.Albedo = c.rgb * gradient.rgb * _Color.rgb;
		o.Alpha = c.a * gradient.a * _Color.a;
	}
	ENDCG
}

Fallback "Diffuse"
}
