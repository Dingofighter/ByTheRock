Shader "RimLightning Lerp" {
	Properties{
		_MainTex("Texture", 2D) = "white" {}
		_BumpMap("Bumpmap", 2D) = "bump" {}
		_RimColor("Rim Color", Color) = (0.26,0.19,0.16,0.0)
		_RimPower("Rim Power", Range(0.0,10.0)) = 3.0
		_RimPower2("Rim Power 2", Range(0.0, 10.0)) = 7.0
		_Value("Value", Float) = 1.0
		_AnimTime("Anim time", Float) = 1.0
	}
		SubShader{
		Tags{ "RenderType" = "Opaque" }
		
		


		CGPROGRAM
#pragma surface surf Standard
#pragma target 3.0
			struct Input 
		{
			float2 uv_MainTex;
			float2 uv_BumpMap;
			float3 viewDir;
		};

	sampler2D _MainTex;
	sampler2D _BumpMap;
	float4 _RimColor;
	float _RimPower;
	float _RimPower2;
	float _AnimTime;
	float _Value;

	void surf(Input IN, inout SurfaceOutputStandard o) 
	{
		o.Albedo = tex2D(_MainTex, IN.uv_MainTex).rgb;
		o.Normal = UnpackNormal(tex2D(_BumpMap, IN.uv_BumpMap));
		half rim = 1.0 - saturate(dot(normalize(IN.viewDir), o.Normal));
		half pow1 = pow(rim, _RimPower);
		half pow2 = pow(rim, _RimPower2);
		float3 final1 = _Value * (pow1 * _RimColor.rgb);
		float3 final2 = pow2 * _RimColor.rgb;

		float animTime = _Time * _AnimTime;
		float t = smoothstep(0.0, 1.0, sin(animTime));

		
		o.Emission = lerp(final1, final2, t);
	}
	ENDCG
	}
		Fallback "Diffuse"
}