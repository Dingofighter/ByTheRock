﻿Shader "Custom/SwaySurfaceCutoutCollisionFade" {
	Properties {
		_Color ("Color", Color) = (1,1,1,1)
		_MainTex("Color (RGB) Alpha (A)", 2D) = "white" {}
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

			_Cutoff("Base Alpha cutoff", Range(0,.9)) = .5

				_HeightMin("Height Min", Float) = -1
				_HeightMax("Height Max", Float) = 5
				_ColorMin("Tint Color At Min", Color) = (1,1,1,1)
				_ColorMax("Tint Color At Max", Color) = (0,0,0,1)
	}
	SubShader {
		Tags{ "RenderType" = "Opaque" }
		LOD 300
		Cull Off
		
		CGPROGRAM
		// Physically based Standard lighting model, and enable shadows on all light types
		#pragma surface surf Lambert fullforwardshadows vertex:vert addshadow alphatest:_Cutoff

		// Use shader model 3.0 target, to get nicer looking lighting
		#pragma target 3.0

		sampler2D _MainTex;
		sampler2D _BumpMap;
		fixed4 _ColorMin;
		fixed4 _ColorMax;
		float _HeightMin;
		float _HeightMax;

		struct Input {
			float2 uv_MainTex;
			float2 uv_BumpMap;
			float3 worldPos;
		};

		half _Glossiness;
		half _Metallic;
		fixed4 _Color;

		float3 _PlayerPos;

		
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
			// Time is offset by a random amount, seeded by the object position 
			// on the xz-plane (taken out of the model matrix's translation column)
			// so that different objects don't sway in perfect unison
			// given the same _Time value.
			float3 object_position = float3(_Object2World[0][3], _Object2World[1][3], _Object2World[2][3]);
			float sway = _SwayVariation * random(object_position);

			float turbulence = _Turbulence * noise(_TurbulenceFrequency * _Time);
			float time = _Time + sway + turbulence;

			float wind_strength = _WindPower * sin(time * _WindSpeed);
			float3 wind_direction = float3(_WindDirectionX, 0, _WindDirectionZ);
			float3 wind_offset = wind_strength * wind_direction;

			// The given wind direction is in world space, so the transformation
			// must be applied to the vertex in world space so all vertices and
			// objects appear to have the wind blowing from the same direction.
			float3 world_position = mul(_Object2World, v.vertex).xyz;

			// Vertices are deformed to make foliage look less solid/rigid
			// so leaves or blades sort-of bend in the wind.
			float deformation = _DeformVariation * random(world_position);
			float3 deformation_offset = wind_offset * deformation;

			// The wind's effect is based on vertex y-position in object space
			// such that at y=0 (where the plant's roots are) there is no
			// wind effect, and at y=_PlantHeight, there is maximum wind effect.
			v.vertex = mul(_Object2World, v.vertex);
			float height_factor = v.vertex.y / _PlantHeight;
			v.vertex = mul(_World2Object, v.vertex);

			// Convert to world coordinates, apply calculated changes, then convert back to local object space
			v.vertex = mul(_Object2World, v.vertex);
			v.vertex.xyz += lerp(0, wind_offset + deformation_offset, height_factor);
			v.vertex = mul(_World2Object, v.vertex);

			/* COLLISION DETECTION */
			v.vertex = mul(_Object2World, v.vertex);
			float d = distance(_PlayerPos, v.vertex.xyz);
			if (d < 1)
			{
				v.vertex.xyz += 0.2f;
			}
			v.vertex = mul(_World2Object, v.vertex);
		}

		void surf (Input IN, inout SurfaceOutput o) {
			// Tint based on height
			float3 localPos = IN.worldPos - mul(_Object2World, float4(0, 0, 0, 1)).xyz;
			float h = (_HeightMax - localPos.y) / (_HeightMax - _HeightMin);
			fixed4 tintColor = lerp(_ColorMax.rgba, _ColorMin.rgba, h);
			// Albedo comes from a texture tinted by color
			fixed4 c = tex2D(_MainTex, IN.uv_MainTex) * tintColor;
			o.Albedo = c.rgb;
			o.Alpha = c.a;
			o.Normal = UnpackNormal(tex2D(_BumpMap, IN.uv_BumpMap));
		}
		ENDCG
	}
	FallBack "Diffuse"
}
