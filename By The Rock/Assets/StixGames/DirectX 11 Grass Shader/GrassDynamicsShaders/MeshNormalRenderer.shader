﻿Shader "Stix Games/Grass Dynamics/Mesh Normal Renderer"
{
	Properties
	{
		_InfluenceStrength("Influence Strength", float) = 1
		_BurnMap("Burn Map", 2D) = "white"  {}
		_BurnStrength("Burn Strength", Range(0, 1)) = 0
	}
	SubShader
	{
		Tags { "RenderType"="Opaque" }
		LOD 100
		Cull Off
		
		Blend One One, DstColor Zero
		BlendOp Add

		ZWrite Off
		ZTest Always

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			
			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				float3 normal : NORMAL;
				float2 uv : TEXCOORD0;
			};

			struct v2f
			{
				float4 vertex : SV_POSITION;
				float3 pos : TEXCOORD0;
				float3 normal : NORMAL;
				float2 uv : TEXCOORD1;
			};

			float _InfluenceStrength;

			sampler2D _BurnMap;
			float _BurnStrength;
			float4 _BurnMap_ST;

			float _GrassDisplacementBorderArea;
			float4 _GrassRenderTextureArea;

			v2f vert (appdata v)
			{
				v2f o;
#if UNITY_VERSION < 540
				o.vertex = mul(UNITY_MATRIX_MVP, v.vertex);
#else
				o.vertex = UnityObjectToClipPos(v.vertex);
#endif
				o.pos = mul(_Object2World, v.vertex);
				o.normal = UnityObjectToWorldNormal(v.normal);
				o.uv = TRANSFORM_TEX(v.uv, _BurnMap);
				return o;
			}
			
			float4 frag (v2f i) : SV_Target
			{
				//Smooth out the border of the displacement. 
				//If the displacement area too big to see the border, you should probably remove that for performance.
				float borderSmoothing = smoothstep(_GrassRenderTextureArea.x, _GrassRenderTextureArea.x + _GrassDisplacementBorderArea, i.pos.x);
				borderSmoothing *= smoothstep(_GrassRenderTextureArea.y, _GrassRenderTextureArea.y + _GrassDisplacementBorderArea, i.pos.z);

				float xBorder = _GrassRenderTextureArea.x + _GrassRenderTextureArea.z;
				borderSmoothing *= smoothstep(xBorder, xBorder - _GrassDisplacementBorderArea, i.pos.x);

				float yBorder = _GrassRenderTextureArea.y + _GrassRenderTextureArea.w;
				borderSmoothing *= smoothstep(yBorder, yBorder - _GrassDisplacementBorderArea, i.pos.z);

				float burnAmount = tex2D(_BurnMap, i.uv).r * _BurnStrength;

				float3 c = normalize(i.normal * borderSmoothing + float3(0,1,0) * (1-borderSmoothing)).xzy * _InfluenceStrength;

				return float4(c, 1-burnAmount);
			}
			ENDCG
		}
	}
}