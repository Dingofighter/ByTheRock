Shader "DoubleSided" {
	Properties{
		_Color("Main Color", Color) = (1,1,1,1)
		_MainTex("Base (RGB)", 2D) = "white" {}
	_BumpMap("Normalmap", 2D) = "bump" {}
	_Cutoff("Base Alpha cutoff", Range(0, .9)) = .5
	}

		SubShader{
		Tags{ "RenderType" = "Opaque" }
		LOD 300
		Cull Off

		CGPROGRAM

#pragma surface surf Lambert addshadow alphatest:_Cutoff


	sampler2D _MainTex;
	sampler2D _BumpMap;
	fixed4 _Color;

	struct Input {
		float2 uv_MainTex;
		float2 uv_BumpMap;
		float3 viewDir;
	};

	void surf(Input IN, inout SurfaceOutput o) {
		fixed4 c = tex2D(_MainTex, IN.uv_MainTex) * _Color;
		o.Albedo = c.rgb;
		o.Alpha = c.a;
	
		float3 n = UnpackNormal(tex2D(_BumpMap, IN.uv_BumpMap));
		o.Normal = dot(IN.viewDir, float3(0, 0, 1)) > 0 ? n : -n;
	}
	ENDCG
	}

		FallBack "Diffuse"
}