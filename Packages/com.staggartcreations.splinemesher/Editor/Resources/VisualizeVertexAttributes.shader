Shader "Hidden/VisualizeVertexAttributes"
{
	Properties
	{
		[KeywordEnum(Color,UV1,UV3)] _Display("Display",int) = 0
		
		[Space]
		
		[Enum(Red,0,Green,1,Blue,2,Alpha,3,All,4)] _ColorChannel ("Color Channel", Float) = 0
		[Enum(X,0,Y,1,Z,2,W,3)] _UVChannel ("UV Channel", Float) = 0
		
		[Space]

		[Toggle] _Transparent ("Transparent", Float) = 0
	}
	
	SubShader
	{
		Tags { "RenderType"="Transparent" "RenderQueue"="Transparent" }
		
		Blend SrcAlpha OneMinusSrcAlpha
		Cull Off 
		ZWrite On
		//ZTest Always
		
		Pass
		{
			CGPROGRAM
			#pragma vertex Vert
			#pragma fragment Frag
			
			#include "UnityCG.cginc"

			#pragma multi_compile_local_fragment _DISPLAY_COLOR _DISPLAY_UV1 _DISPLAY_UV3
			
			uint _ColorChannel;
			uint _UVChannel;
			bool _Transparent;
			
			struct Attributes
			{
				float4 vertex : POSITION;
				float4 color : COLOR;
				float4 uv1 : TEXCOORD0;
				float4 uv3 : TEXCOORD2;
			};

			struct Varyings
			{
				float4 vertex : SV_POSITION;
				float4 color : TEXCOORD0;
				float4 uv1 : TEXCOORD1;
				float4 uv3 : TEXCOORD2;
			};
			
			Varyings Vert (Attributes v)
			{
				Varyings o;
				
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.color = v.color;
				o.uv1 = v.uv1;
				o.uv3 = v.uv3;

				return o;
			}

			float4 RenderVertexColor(Varyings i)
			{
				float alpha = _Transparent ? i.color.a : 1.0;

				//RGB(A)
				if(_ColorChannel == 4) return float4(i.color.rgb, alpha);

				//Specific channel
				float value = i.color[_ColorChannel];
				
				return float4(value.xxx, alpha);
			}

			float4 RenderUV(float4 uv)
			{
				//Specific channel
				float value = uv[_UVChannel];
				
				return float4(value.xxx, 1.0);
			}
			
			float4 Frag(Varyings i) : SV_Target
			{
				#if _DISPLAY_COLOR
				return RenderVertexColor(i);
				#endif
				
				#if _DISPLAY_UV1
				//float center = 1-saturate(abs(i.uv1.z - 0.5) * 2.0);
				//return float4(center.xxx, 1.0);
				return RenderUV(i.uv1);
				#endif
				
				#if _DISPLAY_UV3
				return RenderUV(i.uv3);
				#endif
				
				return float4(0, 0, 0, 1.0);
			}
			ENDCG
		}
	}
}