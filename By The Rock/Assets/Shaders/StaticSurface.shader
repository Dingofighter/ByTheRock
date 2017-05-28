Shader "Custom/StaticSurface" {
	Properties {
		_Color ("Color", Color) = (1,1,1,1)
		_MainTex ("Albedo (RGB)", 2D) = "white" {}
		_BumpMap("Normal Map", 2D) = "bump" {}
			_WindPower("Wind Power", Float) = 0.02
			_WindSpeed("Wind Speed", Float) = 5.0
			_WindDirectionX("Wind Direction X", Float) = 1.0
			_WindDirectionZ("Wind Direction Z", Float) = 1.0
			_SwayVariation("Sway Variation", Range(0, 1)) = 0.05
			_Turbulence("Turbulence", Range(0, 1)) = 0.1
			_TurbulenceFrequency("Turbulence Frequency", Float) = 1.0
			_DeformVariation("Deform Variation", Range(0, 1)) = 0.3
			_PlantHeight("Plant Height", Float) = 1.0
	}
	SubShader {
		Tags { "RenderType"="Opaque" }
		LOD 300
		Cull Off
		
		CGPROGRAM
		// Physically based Standard lighting model, and enable shadows on all light types
		#pragma surface surf Lambert fullforwardshadows vertex:vert addshadow

		// Use shader model 3.0 target, to get nicer looking lighting
		#pragma target 3.0

		sampler2D _MainTex;
		sampler2D _BumpMap;

		struct Input {
			float2 uv_MainTex;
			float2 uv_BumpMap;
			float3 worldPos;
		};

		half _Glossiness;
		half _Metallic;
		fixed4 _Color;

		
#include "UnityCG.cginc"

		float random(float3 p) {
			float dt = dot(p, float3(12.9898, 78.233, 45.5432));
			float phase = dt / 3.14159;
			return frac(43758.5453 * sin(phase));
		}

		half noise(float t) {
			half x0 = floor(t);
			half x1 = x0 + 1.0;
			half v0 = frac(31718.927 * sin(0.014686 * x0) + x0);
			half v1 = frac(31718.927 * sin(0.014686 * x1) + x1);
			return 2 * (v0 * (1 - frac(t)) + v1 * frac(t)) - 1 * sin(t);
		}

		CBUFFER_START(WindVariables)
			float _WindPower;
		float _WindSpeed;
		CBUFFER_END

			CBUFFER_START(WindVector)
			float _WindDirectionX;
		float _WindDirectionZ;
		CBUFFER_END

			float _SwayVariation;
		float _Turbulence;
		float _TurbulenceFrequency;

		float _DeformVariation;
		float _PlantHeight;

		void vert(inout appdata_full v) {

		}

		void surf (Input IN, inout SurfaceOutput o) {
			// Albedo comes from a texture tinted by color
			fixed4 c = tex2D (_MainTex, IN.uv_MainTex) * _Color;
			o.Albedo = c.rgb;
			o.Alpha = c.a;
			o.Normal = UnpackNormal(tex2D(_BumpMap, IN.uv_BumpMap));
		}
		ENDCG
	}
	FallBack "Diffuse"
}
