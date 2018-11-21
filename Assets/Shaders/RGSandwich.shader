Shader "Unlit/RGSandwich"
{
   Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_BumpMap("Normal Map", 2D) = "bump" {}
		_LightMap("Lighting Map", 2D) = "white" {}
	}
	SubShader
	{
		Tags { "RenderType"="Opaque" }
		LOD 100
		Cull Off

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			
			#include "UnityCG.cginc"

			struct v2f
			{
				float3 worldPos : TEXCOORD0;
                // these three vectors will hold a 3x3 rotation matrix
                // that transforms from tangent to world space
                half3 tspace0 : TEXCOORD1; // tangent.x, bitangent.x, normal.x
                half3 tspace1 : TEXCOORD2; // tangent.y, bitangent.y, normal.y
                //half3 tspace2 : TEXCOORD3; // tangent.z, bitangent.z, normal.z
                // texture coordinate for the normal map
                float2 uv : TEXCOORD3;
                float4 pos : SV_POSITION;
                float4 colour : COLOR;
			};

			sampler2D _MainTex;
			float4 _MainTex_ST;

			sampler2D _BumpMap;
			float4 _BumpMap_ST;

			sampler2D _LightMap;
			float4 _LightMap_ST;


			v2f vert (float4 vertex : POSITION, float3 normal : NORMAL, float4 tangent : TANGENT, float2 uv : TEXCOORD0, float4 col : COLOR)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(vertex);
                o.worldPos = mul(unity_ObjectToWorld, vertex).xyz;
                half3 wNormal = UnityObjectToWorldNormal(normal);
                half3 wTangent = UnityObjectToWorldDir(tangent.xyz);
                // compute bitangent from cross product of normal and tangent
                half tangentSign = tangent.w * unity_WorldTransformParams.w;
                half3 wBitangent = cross(wNormal, wTangent) * tangentSign;
                // output the tangent space matrix
                o.tspace0 = half3(wTangent.x, wBitangent.x, wNormal.x);
                o.tspace1 = half3(wTangent.y, wBitangent.y, wNormal.y);
                //o.tspace2 = half3(wTangent.z, wBitangent.z, wNormal.z);
                o.uv = uv;
                o.colour = col;
                return o;
            }
			
			fixed4 frag (v2f i) : SV_Target
			{
				fixed4 col = tex2D(_MainTex, i.uv);
				// sample the normal map, and decode from the Unity encoding
                half3 tnormal = UnpackNormal(tex2D(_BumpMap, i.uv));
                // transform normal from tangent to world space
                //half3 worldNormal;
                //worldNormal.x = dot(i.tspace0, tnormal);
                //worldNormal.y = dot(i.tspace1, tnormal);
                //worldNormal.z = dot(i.tspace2, tnormal);

                half2 paintUV = half2( dot(i.tspace0, tnormal), dot(i.tspace1, tnormal))*0.5 +0.5;
                fixed4 paintcol = tex2D(_LightMap, paintUV);
				
                fixed4 target = col.r * i.colour;

				fixed4 result = (paintcol > 0.5) * (target + 2*(paintcol-0.5)) +
						(paintcol <= 0.5) * (target + 2*paintcol - 1);
				fixed4 lightened = 1 - (1 - col.g) * (1 - result);
				return lightened * clamp(1-i.worldPos.z/35.0, 0, 1);
			}
			ENDCG
		}
    }
}