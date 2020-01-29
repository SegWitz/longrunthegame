Shader "Custom/Unlit/Vertical Gradient"
{
	Properties
	{
		_ColorHigh("Color Top", Color) = (1,1,1,1)
		_ColorLow("Color Bottom", Color) = (1,1,1,1)
	}

	SubShader
	{
		Tags { "RenderType"="Background" }

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
			};

			struct v2f
			{
				half4 vertex : SV_POSITION;
				fixed4 color : COLOR;
			};

			fixed4 _ColorHigh;
			fixed4 _ColorLow;
			
			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.color = lerp(_ColorLow, _ColorHigh, v.uv.y);
				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				return i.color;
			}

			ENDCG
		}
	}
}