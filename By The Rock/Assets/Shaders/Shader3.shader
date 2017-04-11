﻿// Modified Unity version - WipeShader - mgear - http://unitycoder.com/blog/
// ORIGINAL GLSL SHADER "Postpro" WAS MADE BY: iq (2009)

Shader "Test"
{
	Properties
	{
		_tex0("Texture1", 2D) = "white" {}
		_tex1("Texture2", 2D) = "white" {}
		_animTimeParam("AnimTime", Float) = 10.0
		_test("Test", Float) = 10.0

		
	}

		SubShader
	{
		Tags{ Queue = Geometry }
		Pass
		{
			CGPROGRAM
			#pragma target 3.0
			#pragma vertex vert
			#pragma fragment frag
			#include "UnityCG.cginc"

			sampler2D _tex0;
			sampler2D _tex1;
			float _animTimeParam;
			float _test;

			struct v2f 
			{
				float4 pos : POSITION;
				float4 color : COLOR0;
				float4 fragPos : COLOR1;
				float2  uv : TEXCOORD0;
			};

			float4 _tex0_ST;

			v2f vert(appdata_base v)
			{
				v2f o;
				o.pos = mul(UNITY_MATRIX_MVP, v.vertex);
				o.fragPos = o.pos;
				o.uv = TRANSFORM_TEX(v.texcoord, _tex0);
				o.color = float4 (1.0, 1.0, 1.0, 1);
				return o;
			}

			
			half4 frag(v2f i) : COLOR
			{
				float animTime = _Time*_animTimeParam;
				float2 q = i.uv.xy / float2(1,1);
				float3 oricol = tex2D(_tex0,float2(q.x,q.y)).xyz;
				float3 col = tex2D(_tex1,float2(i.uv.x,i.uv.y)).xyz;
				float comp = smoothstep(0.2, 0.7, sin(animTime));

				col = lerp(col,oricol,  clamp(-2.0 + 2.0*q.x + 3.0*comp,0.0,1.0)); // ??

				return float4(col,1);
			}
				ENDCG
		}
	}
		FallBack "VertexLit"
}