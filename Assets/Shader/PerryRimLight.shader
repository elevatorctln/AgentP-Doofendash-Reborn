Shader "Perry/RimLight" {
Properties {
 _Color ("Main Color", Color) = (1,1,1,1)
 _MainTex ("Base (RGB)", 2D) = "white" {}
 _Ramp ("Toon Ramp (RGB)", 2D) = "gray" {}
 _RimColor ("Rim Color", Color) = (0.5,0.5,0.5,0.3)
 _RimPower ("Rim Power", Float) = 0.5
 _SColor ("Shadow Color", Color) = (0,0,0,1)
 _LColor ("Highlight Color", Color) = (0.5,0.5,0.5,1)
}

// this is a reimplementation, it's not perfectly accurate

SubShader {
	Tags { "RenderType"="Opaque" }
	LOD 200

	CGPROGRAM
	#pragma surface surf ToonRamp
	#pragma target 3.0

	sampler2D _MainTex;
	sampler2D _Ramp;
	
	fixed4 _Color;
	fixed4 _RimColor;
	float _RimPower;
	fixed4 _SColor;
	fixed4 _LColor;

	struct Input {
		float2 uv_MainTex;
		float3 viewDir;
	};

	inline fixed4 LightingToonRamp (SurfaceOutput s, fixed3 lightDir, fixed atten)
	{
		fixed NdotL = dot(s.Normal, lightDir);

		fixed rampValue = NdotL * 0.5 + 0.5;
		fixed3 ramp = tex2D(_Ramp, fixed2(rampValue, rampValue)).rgb;
		
		fixed3 rampedLight = lerp(_SColor.rgb, _LColor.rgb, ramp);
		
		fixed4 c;
		c.rgb = s.Albedo * _LightColor0.rgb * rampedLight * atten;
		c.a = s.Alpha;
		return c;
	}

	void surf (Input IN, inout SurfaceOutput o)
	{
		fixed4 c = tex2D(_MainTex, IN.uv_MainTex) * _Color;
		o.Albedo = c.rgb;
		o.Alpha = c.a;
		
		half rim = 1.0 - saturate(dot(normalize(IN.viewDir), o.Normal));
		
		o.Emission = _RimColor.rgb * pow(rim, _RimPower);
	}
	ENDCG
}

Fallback "Diffuse"
}
