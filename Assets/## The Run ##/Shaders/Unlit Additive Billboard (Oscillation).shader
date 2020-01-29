Shader "Custom/Unlit/Additive Billboard (Oscillation)"
{
	Properties
	{
		_Color ("Color", Color) = (1, 1, 1, 1)
		_Intensity("Intensity", Float) = 1
		_Params("Min Scale (X), Max Scale (Y), Speed (Z)", Vector) = (1.0, 1.0, 1.0, 0.0)
		[NoScaleOffset]_MainTex ("Texture", 2D) = "white" {}
	}

	SubShader
	{
		Tags { "RenderType"="Opaque" "PreviewType"="Plane" "DisableBatching"="True" }
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
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				float4 vertex : SV_POSITION;
			};

			half4 _Color;
			half _Intensity;
			half4 _Params;
			sampler2D _MainTex;
			
			v2f vert (appdata v)
			{
				v2f o;
				//o.vertex = UnityObjectToClipPos(v.vertex);
				float scale = length(float3(unity_ObjectToWorld[0].x, unity_ObjectToWorld[1].x, unity_ObjectToWorld[2].x));
				half Osc = (sin(_Time.y * _Params.z) + 1) * 0.5;
				Osc = lerp(_Params.x, _Params.y, Osc) * scale;
				o.vertex = mul(UNITY_MATRIX_P, float4(UnityObjectToViewPos(float3(0.0, 0.0, 0.0)), 1.0) + float4(v.vertex.x, v.vertex.y, 0.0, 0.0) * float4(Osc, Osc, 1.0, 1.0));
				o.uv = v.uv;
				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				return tex2D(_MainTex, i.uv) * _Color * _Intensity;
			}

			ENDCG
		}
	}
}
