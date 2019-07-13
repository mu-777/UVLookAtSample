Shader "Unlit/UVLookAtSample"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		[HideInInspector]
		_UserPosition("User Position", Vector) = (0.0, 0.0, 0.0)
		[HideInInspector]
		_UpDirectionOnTex("Up Vector in Texture(Axis before NormalVec Axis (if Normal is Z, this is Y))", Vector) = (0.0, 1.0, 0.0)
		[HideInInspector]
		_UVOffsetScale("Scale for UV Offset", Vector) = (1.0, 1.0, 0.0)

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
				float2 uv0 : TEXCOORD0;
				float3 normal : NORMAL;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				float2 uvOffset : TEXCOORD1;
				UNITY_FOG_COORDS(1)
				float4 vertex : SV_POSITION;
			};

			sampler2D _MainTex;
			float4 _MainTex_ST;

			float3 _UserPosition;
			float3 _UpDirectionOnTex;
			float3 _UVOffsetScale;

			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				UNITY_TRANSFER_FOG(o,o.vertex);

				float3 upvec = normalize(_UpDirectionOnTex);
				float3 rightvec = normalize(cross(upvec, v.normal));
				float3x3 texRori = float3x3(
					rightvec.x, rightvec.y, rightvec.z,
					upvec.x, upvec.y, upvec.z,
					v.normal.x, v.normal.y, v.normal.z);
				float3 texPori = mul(texRori, v.vertex);
				float4x4 texTori = float4x4(
					rightvec.x, rightvec.y, rightvec.z, -texPori.x,
					upvec.x, upvec.y, upvec.z, -texPori.y,
					v.normal.x, v.normal.y, v.normal.z, -texPori.z,
					0, 0, 0, 1);
				float3 vInTexCoord = mul(texTori, float4(_UserPosition, 1)).xyz;
				o.uvOffset = float2(-vInTexCoord.x*_UVOffsetScale.x, vInTexCoord.y*_UVOffsetScale.y);
				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				// sample the texture
				fixed4 col = tex2D(_MainTex, i.uv - i.uvOffset);
				// apply fog
				UNITY_APPLY_FOG(i.fogCoord, col);
				return col;
			}
			ENDCG
		}
	}
}
