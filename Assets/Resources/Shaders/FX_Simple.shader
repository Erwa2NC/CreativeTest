Shader "FX/FX_Simple"
{
    Properties
	{
		[HDR]_Color("Color", Color) = (1,1,1,1)
		_Opacity("Opacity", Range( 0 , 1)) = 1
		_EmissionIntensity("Emission Intensity", Float) = 1
		[Header(Texture)][Toggle(_USETEXTURE_ON)] _UseTexture("Use Texture ?", Float) = 0
		_MainTex("MainTex", 2D) = "white" {}
		[Toggle(_USETEXTUREALPHAINSTEAD_ON)] _UseTextureAlphaInstead("Use Texture Alpha Instead ?", Float) = 0
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Transparent"  "Queue" = "Transparent+0" "IgnoreProjector" = "True" "IsEmissive" = "true"  }
		Cull Back
		CGPROGRAM
		#pragma target 3.0
		#pragma shader_feature _USETEXTURE_ON
		#pragma shader_feature _USETEXTUREALPHAINSTEAD_ON
		#pragma exclude_renderers xboxseries playstation switch nomrt 
		#pragma surface surf Unlit alpha:fade keepalpha noshadow 
		struct Input
		{
			float4 vertexColor : COLOR;
			float2 uv_texcoord;
		};

		uniform float4 _Color;
		uniform float _EmissionIntensity;
		uniform sampler2D _MainTex;
		uniform float4 _MainTex_ST;
		uniform float _Opacity;

		inline half4 LightingUnlit( SurfaceOutput s, half3 lightDir, half atten )
		{
			return half4 ( 0, 0, 0, s.Alpha );
		}

		void surf( Input i , inout SurfaceOutput o )
		{
			o.Emission = ( i.vertexColor * _Color * _EmissionIntensity ).rgb;
			float2 uv_MainTex = i.uv_texcoord * _MainTex_ST.xy + _MainTex_ST.zw;
			float4 tex2DNode4 = tex2D( _MainTex, uv_MainTex );
			#ifdef _USETEXTUREALPHAINSTEAD_ON
				float staticSwitch15 = tex2DNode4.a;
			#else
				float staticSwitch15 = tex2DNode4.r;
			#endif
			#ifdef _USETEXTURE_ON
				float staticSwitch2 = ( i.vertexColor.a * staticSwitch15 );
			#else
				float staticSwitch2 = i.vertexColor.a;
			#endif
			float lerpResult12 = lerp( 0.0 , staticSwitch2 , _Opacity);
			o.Alpha = lerpResult12;
		}

		ENDCG
	}
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=18921
1920;-679;1920;1019;2933.767;801.4943;2.297235;True;True
Node;AmplifyShaderEditor.TexturePropertyNode;3;-1634.943,291.6224;Float;True;Property;_MainTex;MainTex;4;0;Create;True;0;0;0;False;0;False;None;7eef1b66a64daee46b87f6f979353425;False;white;Auto;Texture2D;-1;0;2;SAMPLER2D;0;SAMPLERSTATE;1
Node;AmplifyShaderEditor.SamplerNode;4;-1311.694,289.6229;Inherit;True;Property;_TextureSample0;Texture Sample 0;3;0;Create;True;0;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.VertexColorNode;6;-1285.933,-350.1288;Inherit;False;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.StaticSwitch;15;-946.0969,301.093;Float;False;Property;_UseTextureAlphaInstead;Use Texture Alpha Instead ?;5;0;Create;True;0;0;0;False;0;False;0;0;0;True;;Toggle;2;Key0;Key1;Create;False;True;9;1;FLOAT;0;False;0;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT;0;False;7;FLOAT;0;False;8;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;13;-597.373,216.7377;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.StaticSwitch;2;-330.9949,201.6055;Float;False;Property;_UseTexture;Use Texture ?;3;0;Create;True;0;0;0;False;1;Header(Texture);False;0;0;1;True;;Toggle;2;Key0;Key1;Create;False;True;9;1;FLOAT;0;False;0;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT;0;False;7;FLOAT;0;False;8;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;11;-212.004,381.2946;Float;False;Property;_Opacity;Opacity;1;0;Create;True;0;0;0;False;0;False;1;1;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;10;-13.46743,194.4353;Float;False;Constant;_Float0;Float 0;3;0;Create;True;0;0;0;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;14;-212.0462,0.2835739;Float;False;Property;_EmissionIntensity;Emission Intensity;2;0;Create;True;0;0;0;False;0;False;1;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;1;-431.7798,-138.9157;Float;False;Property;_Color;Color;0;1;[HDR];Create;True;0;0;0;False;0;False;1,1,1,1;0.4764151,0.8186555,1,1;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.LerpOp;12;190.3136,257.8279;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;7;-40.36569,-171.9082;Inherit;False;3;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;495.4543,16.06459;Float;False;True;-1;2;ASEMaterialInspector;0;0;Unlit;_LUCEED/FX_Simple;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;False;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Transparent;0.5;True;False;0;False;Transparent;;Transparent;All;14;d3d9;d3d11_9x;d3d11;glcore;gles;gles3;metal;vulkan;xbox360;xboxone;ps4;psp2;n3ds;wiiu;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;False;2;5;False;-1;10;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;True;Relative;0;;-1;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;False;15;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;4;0;3;0
WireConnection;15;1;4;1
WireConnection;15;0;4;4
WireConnection;13;0;6;4
WireConnection;13;1;15;0
WireConnection;2;1;6;4
WireConnection;2;0;13;0
WireConnection;12;0;10;0
WireConnection;12;1;2;0
WireConnection;12;2;11;0
WireConnection;7;0;6;0
WireConnection;7;1;1;0
WireConnection;7;2;14;0
WireConnection;0;2;7;0
WireConnection;0;9;12;0
ASEEND*/
//CHKSM=86A91AA58FE9591564C0BD7844037C62D7DE0AAF
