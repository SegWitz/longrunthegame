Shader "Custom/Unlit/Additive Billboard"
{
	Properties
	{
		_Color ("Color", Color) = (1, 1, 1, 1)
		_Intensity("Intensity", Float) = 1
		_Params("Size (X), Speed (Y), Tex Rows (Z), Variation (W)", Vector) = (1.0, 1.0, 1.0, 0.0)
		_Params2("Billboard Offset (X, Y), Distance Mult (Z)", Vector) = (0.0, 0.0, 1.0, 0.0)
		_MainTex ("Texture", 2D) = "white" {}
	}

	SubShader
	{
		Tags { "Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent" "PreviewType"="Plane" "DisableBatching"="True" }
		Blend SrcAlpha One
		ColorMask RGB
		Cull Off Lighting Off ZWrite Off

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			
			#include "UnityCG.cginc"

			struct appdata
			{
				half4 vertex : POSITION;
				half2 uv : TEXCOORD0;
				fixed4 color : COLOR;
			};

			struct v2f
			{
				half2 uv : TEXCOORD0;
				half4 vertex : SV_POSITION;
				fixed4 color : COLOR;
			};

			half4 _Color;
			half _Intensity;
			half4 _Params;
			half4 _Params2;
			sampler2D _MainTex;
			
			v2f vert (appdata v)
			{
				v2f o;
				//o.vertex = UnityObjectToClipPos(v.vertex);
				
				half3 VertexViewSpace = UnityObjectToViewPos(float3(0.0, 0.0, 0.0));
				o.vertex = mul(UNITY_MATRIX_P, float4(VertexViewSpace, 1.0) + float4(v.vertex.x + _Params2.x, v.vertex.y + _Params2.y, 0.0, 0.0) * float4(_Params.x, _Params.x, 1.0, 1.0));
				
				half3 ObjPos = mul(UNITY_MATRIX_M, float4(0.0, 0.0, 0.0, 1.0));
				half Variation = (ObjPos.x + ObjPos.y + ObjPos.z) * _Params.w;
				half Offset = floor((_Time.y + Variation) * _Params.y);
				o.uv = (v.uv + half2(Offset % _Params.z, -floor(Offset / _Params.z) % _Params.z)) * (1 / _Params.z);
				
				half Dist = distance(_WorldSpaceCameraPos, ObjPos) * _Params2.z;
				half Fade = lerp(1, 0, clamp(Dist * Dist * Dist * Dist, 0, 1));
				
				o.color = _Color * _Intensity * Fade;
				
				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				return tex2D(_MainTex, i.uv) * i.color;
			}

			ENDCG
		}
	}
}
