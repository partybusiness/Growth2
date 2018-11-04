Shader "Unlit/Colorize"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_ColorTex ("Colorize Texture", 2D) = "white" {}
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
			
			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				float4 vertex : SV_POSITION;
			};

			sampler2D _MainTex;
			float4 _MainTex_ST;


			sampler2D _ColorTex;
			float4 _ColorTex_ST;

			
			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				// sample the texture
				fixed4 tex = tex2D(_MainTex, i.uv);
				fixed4 col = tex2D(_ColorTex, i.uv);
				//tex grayscale
				fixed average = (tex.r + tex.g +tex.b)/3.0;
				fixed4 grayscale = fixed4(average, average, average, tex.a);

				return grayscale * col;
			}
			ENDCG
		}
	}
}
