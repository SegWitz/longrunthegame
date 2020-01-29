Shader "Custom/Mobile Water"
{
	Properties
	{
	    _Color("Ambient Color", Color) = (0,0,0,1)
	    [NoScaleOffset]_MainTex ("Base (RGB)", 2D) = "white" {}
	    _Speeds ("X1, Y1, X2, Y2", Vector) = (1,1,-1,-1)
	    _Scale ("Scale", Float) = 1
        _Intensity ("Intensity", Float) = 1
	}
	
	SubShader
	{
	    Tags { "RenderType"="Opaque" }
	    LOD 150

		CGPROGRAM
		#pragma surface surf Lambert noforwardadd vertex:vert
			
		fixed4 _Color;
		sampler2D _MainTex;
		fixed4 _Speeds;
		fixed _Scale;
		fixed _Intensity;
		
		struct appdata
		{
			half4 vertex : POSITION;
			half3 normal : NORMAL;
			half4 texcoord : TEXCOORD0;
			half4 texcoord1 : TEXCOORD1;
			half4 texcoord2 : TEXCOORD2;
			fixed4 color : COLOR;
		};
		
		struct Input
		{
		    half2 tex1;
            half2 tex2;
            fixed4 color : COLOR;
		};
		
		void vert(inout appdata v, out Input o)
		{
			UNITY_INITIALIZE_OUTPUT(Input,o);

            o.tex1 = (v.texcoord * _Scale) + (_Time.y * _Speeds.xy);
			o.tex2 = (v.texcoord * _Scale) + (_Time.y * _Speeds.zw);
		}

		void surf (Input IN, inout SurfaceOutput o)
		{
		    fixed Tex1 = tex2D(_MainTex, IN.tex1).r;
		    fixed Tex2 = tex2D(_MainTex, IN.tex2).g;
		    
		    fixed4 c = clamp(_Color + (Tex1 * Tex2 * 2) - 0.15, 0, 1);
		    o.Albedo = c.rgb;
		    o.Alpha = c.a;
		}
		
		ENDCG
	}

	Fallback "Mobile/VertexLit"
}