Shader "Unlit/YUV_Vijeo"
{
	Properties
	{
		_YTex ("Texture", 2D) = "white" {}
		_UTex ("Texture", 2D) = "white" {}
		_VTex ("Texture", 2D) = "white" {}
	}
	SubShader
	{
		Tags { "RenderType"="Opaque" }
		LOD 100

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			// make fog work
			#pragma multi_compile_fog
			
			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				UNITY_FOG_COORDS(1)
				float4 vertex : SV_POSITION;
			};

			sampler2D _YTex;
			sampler2D _UTex;
			sampler2D _VTex;
			
			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = v.uv;
				return o;
			}

			fixed4 frag (v2f i) : SV_Target
			{
				float y_value = 0;
				float u_value = 0;
				float v_value = 0;

				float4 packed_y = tex2D(_YTex, float2(i.uv.x, 1.0 - i.uv.y));

				y_value = packed_y.a;

				float4 u = tex2D(_UTex, float2(i.uv.x, 1.0 - i.uv.y));
				float4 v = tex2D(_VTex, float2(i.uv.x, 1.0 - i.uv.y));

				u_value = u.a;
				v_value = v.a;
				
				// The YUV to RBA conversion, please refer to: http://en.wikipedia.org/wiki/YUV
				// U and V are expected to be in range [-0.5;0.5] but since they are read from the texture their values
				// is in the range [0;1] which explain all the -0.5 in the following formulas.

				float r = y_value + 1.370705 * (v_value - 0.5);
				float g = y_value - 0.698001 * (v_value - 0.5) - (0.337633 * (u_value - 0.5));
				float b = y_value + 1.732446 * (u_value - 0.5);

				r = clamp(r, 0.0, 1.0);
				g = clamp(g, 0.0, 1.0);
				b = clamp(b, 0.0, 1.0);

				return float4(r, g, b, 1.0);
			}
			ENDCG
		}
	}
}