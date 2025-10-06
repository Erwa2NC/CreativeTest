// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Gate_Transparent"
{
	Properties
	{
		[Header(Base)]_BaseTint("Base Tint", Color) = (0.7529412,0.7529412,0.7529412,1)
		_BaseTexture("Base Texture", 2D) = "white" {}
		_TextureOpacity("Texture Opacity", Range( 0 , 1)) = 1
		[Header(Shadow)]_ShadowColor("Shadow Color", Color) = (0,0,0,0)
		_ShadowThreshold("Shadow Threshold", Range( -1 , 1)) = 0
		_ShadowSharpness("Shadow Smoothing", Range( 0.01 , 1)) = 1
		_ShadowOpacity("Shadow Opacity", Range( 0 , 1)) = 1
		_IndirectDiffuseContribution("Indirect Diffuse Contribution", Range( 0 , 1)) = 1
		[HDR][Header(Specular)]_SpecularTint("Specular Tint", Color) = (1,1,1,1)
		_SpecularTexture("Specular Texture", 2D) = "white" {}
		_SpecularThreshold("Specular Threshold", Range( -1 , -0.5)) = -0.7
		_SpecularSmoothing("Specular Smoothing", Range( 0.001 , 1)) = 1
		_SpecularOpacity("Specular Opacity", Range( 0 , 1)) = 1
		[HDR][Header(Rim)]_RimColor("Rim Color", Color) = (1,1,1,0)
		_RimPower("Rim Power", Range( 0.01 , 1)) = 0.4
		_RimOffset("Rim Offset", Range( 0 , 1)) = 0.6
		_RimOpacity("Rim Opacity", Range( 0 , 1)) = 1
		[Header(Properties)][Toggle(_VERTEXCOLOR_ON)] _VertexColor("Vertex Color", Float) = 0
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Transparent"  "Queue" = "Transparent+0" "IgnoreProjector" = "True" }
		Cull Back
		CGINCLUDE
		#include "UnityPBSLighting.cginc"
		#include "UnityCG.cginc"
		#include "UnityShaderVariables.cginc"
		#include "Lighting.cginc"
		#pragma target 3.0
		#pragma shader_feature _VERTEXCOLOR_ON
		#ifdef UNITY_PASS_SHADOWCASTER
			#undef INTERNAL_DATA
			#undef WorldReflectionVector
			#undef WorldNormalVector
			#define INTERNAL_DATA half3 internalSurfaceTtoW0; half3 internalSurfaceTtoW1; half3 internalSurfaceTtoW2;
			#define WorldReflectionVector(data,normal) reflect (data.worldRefl, half3(dot(data.internalSurfaceTtoW0,normal), dot(data.internalSurfaceTtoW1,normal), dot(data.internalSurfaceTtoW2,normal)))
			#define WorldNormalVector(data,normal) half3(dot(data.internalSurfaceTtoW0,normal), dot(data.internalSurfaceTtoW1,normal), dot(data.internalSurfaceTtoW2,normal))
		#endif
		struct Input
		{
			float3 worldNormal;
			INTERNAL_DATA
			float3 worldPos;
			float4 vertexColor : COLOR;
			float2 uv_texcoord;
		};

		struct SurfaceOutputCustomLightingCustom
		{
			half3 Albedo;
			half3 Normal;
			half3 Emission;
			half Metallic;
			half Smoothness;
			half Occlusion;
			half Alpha;
			Input SurfInput;
			UnityGIInput GIData;
		};

		uniform float _SpecularThreshold;
		uniform float _SpecularSmoothing;
		uniform float _SpecularOpacity;
		uniform float4 _BaseTint;
		uniform float _RimOffset;
		uniform float _RimPower;
		uniform float _RimOpacity;
		uniform float4 _SpecularTint;
		uniform sampler2D _SpecularTexture;
		uniform float4 _SpecularTexture_ST;
		uniform float _IndirectDiffuseContribution;
		uniform float _ShadowThreshold;
		uniform float _ShadowSharpness;
		uniform float4 _ShadowColor;
		uniform float _ShadowOpacity;
		uniform sampler2D _BaseTexture;
		uniform float4 _BaseTexture_ST;
		uniform float _TextureOpacity;
		uniform float4 _RimColor;

		inline half4 LightingStandardCustomLighting( inout SurfaceOutputCustomLightingCustom s, half3 viewDir, UnityGI gi )
		{
			UnityGIInput data = s.GIData;
			Input i = s.SurfInput;
			half4 c = 0;
			#ifdef UNITY_PASS_FORWARDBASE
			float ase_lightAtten = data.atten;
			if( _LightColor0.a == 0)
			ase_lightAtten = 0;
			#else
			float3 ase_lightAttenRGB = gi.light.color / ( ( _LightColor0.rgb ) + 0.000001 );
			float ase_lightAtten = max( max( ase_lightAttenRGB.r, ase_lightAttenRGB.g ), ase_lightAttenRGB.b );
			#endif
			#if defined(HANDLE_SHADOWS_BLENDING_IN_GI)
			half bakedAtten = UnitySampleBakedOcclusion(data.lightmapUV.xy, data.worldPos);
			float zDist = dot(_WorldSpaceCameraPos - data.worldPos, UNITY_MATRIX_V[2].xyz);
			float fadeDist = UnityComputeShadowFadeDistance(data.worldPos, zDist);
			ase_lightAtten = UnityMixRealtimeAndBakedShadows(data.atten, bakedAtten, UnityComputeShadowFade(fadeDist));
			#endif
			float3 ase_worldNormal = WorldNormalVector( i, float3( 0, 0, 1 ) );
			float3 ase_normWorldNormal = normalize( ase_worldNormal );
			float3 ase_worldPos = i.worldPos;
			#if defined(LIGHTMAP_ON) && UNITY_VERSION < 560 //aseld
			float3 ase_worldlightDir = 0;
			#else //aseld
			float3 ase_worldlightDir = normalize( UnityWorldSpaceLightDir( ase_worldPos ) );
			#endif //aseld
			float3 ase_worldViewDir = Unity_SafeNormalize( UnityWorldSpaceViewDir( ase_worldPos ) );
			float3 normalizeResult137_g12 = normalize( ( ase_worldlightDir + ase_worldViewDir ) );
			float dotResult141_g12 = dot( ase_normWorldNormal , normalizeResult137_g12 );
			float NdotLV140_g12 = dotResult141_g12;
			float temp_output_188_0_g12 = ( saturate( ( ( NdotLV140_g12 + _SpecularThreshold ) / _SpecularSmoothing ) ) * _SpecularOpacity );
			float zero113_g12 = 0.0;
			float one119_g12 = 1.0;
			#ifdef _VERTEXCOLOR_ON
				float staticSwitch116_g12 = one119_g12;
			#else
				float staticSwitch116_g12 = zero113_g12;
			#endif
			float lerpResult123_g12 = lerp( _BaseTint.a , i.vertexColor.a , staticSwitch116_g12);
			float dotResult10_g12 = dot( ase_normWorldNormal , ase_worldlightDir );
			float NdotL11_g12 = dotResult10_g12;
			float dotResult38_g12 = dot( ase_normWorldNormal , ase_worldViewDir );
			float NdotV138_g12 = dotResult38_g12;
			float temp_output_85_0_g12 = ( saturate( NdotL11_g12 ) * pow( ( 1.0 - saturate( ( NdotV138_g12 + _RimOffset ) ) ) , _RimPower ) * _RimOpacity );
			float3 HighlightColor83_g12 = (_SpecularTint).rgb;
			#if defined(LIGHTMAP_ON) && ( UNITY_VERSION < 560 || ( defined(LIGHTMAP_SHADOW_MIXING) && !defined(SHADOWS_SHADOWMASK) && defined(SHADOWS_SCREEN) ) )//aselc
			float4 ase_lightColor = 0;
			#else //aselc
			float4 ase_lightColor = _LightColor0;
			#endif //aselc
			float3 LightColorFalloff80_g12 = ( ase_lightColor.rgb * ase_lightAtten );
			float2 UV151_g12 = i.uv_texcoord;
			float3 temp_cast_1 = (1.0).xxx;
			UnityGI gi102_g12 = gi;
			float3 diffNorm102_g12 = WorldNormalVector( i , ase_normWorldNormal );
			gi102_g12 = UnityGI_Base( data, 1, diffNorm102_g12 );
			float3 indirectDiffuse102_g12 = gi102_g12.indirect.diffuse + diffNorm102_g12 * 0.0001;
			float3 lerpResult40_g12 = lerp( temp_cast_1 , indirectDiffuse102_g12 , _IndirectDiffuseContribution);
			float temp_output_31_0_g12 = ( 1.0 - ( ( 1.0 - 0.0 ) * _WorldSpaceLightPos0.w ) );
			float4 temp_cast_4 = (temp_output_31_0_g12).xxxx;
			float4 temp_cast_5 = (( saturate( ( ( NdotL11_g12 + _ShadowThreshold ) / _ShadowSharpness ) ) * ase_lightAtten )).xxxx;
			float4 lerpResult44_g12 = lerp( temp_cast_4 , max( temp_cast_5 , _ShadowColor ) , _ShadowOpacity);
			float4 lerpResult120_g12 = lerp( _BaseTint , i.vertexColor , staticSwitch116_g12);
			float4 lerpResult133_g12 = lerp( lerpResult120_g12 , ( tex2D( _BaseTexture, (UV151_g12*_BaseTexture_ST.xy + _BaseTexture_ST.zw) ) * lerpResult120_g12 ) , _TextureOpacity);
			c.rgb = ( float4( ( HighlightColor83_g12 * LightColorFalloff80_g12 * tex2D( _SpecularTexture, (UV151_g12*_SpecularTexture_ST.xy + _SpecularTexture_ST.zw) ).r * temp_output_188_0_g12 ) , 0.0 ) + ( ( float4( ( lerpResult40_g12 * ase_lightColor.a * temp_output_31_0_g12 ) , 0.0 ) + ( float4( ase_lightColor.rgb , 0.0 ) * lerpResult44_g12 ) ) * float4( (lerpResult133_g12).rgb , 0.0 ) ) + float4( ( temp_output_85_0_g12 * HighlightColor83_g12 * LightColorFalloff80_g12 * (_RimColor).rgb ) , 0.0 ) ).rgb;
			c.a = saturate( ( temp_output_188_0_g12 + lerpResult123_g12 + temp_output_85_0_g12 ) );
			return c;
		}

		inline void LightingStandardCustomLighting_GI( inout SurfaceOutputCustomLightingCustom s, UnityGIInput data, inout UnityGI gi )
		{
			s.GIData = data;
		}

		void surf( Input i , inout SurfaceOutputCustomLightingCustom o )
		{
			o.SurfInput = i;
			o.Normal = float3(0,0,1);
		}

		ENDCG
		CGPROGRAM
		#pragma surface surf StandardCustomLighting alpha:fade keepalpha fullforwardshadows 

		ENDCG
		Pass
		{
			Name "ShadowCaster"
			Tags{ "LightMode" = "ShadowCaster" }
			ZWrite On
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma target 3.0
			#pragma multi_compile_shadowcaster
			#pragma multi_compile UNITY_PASS_SHADOWCASTER
			#pragma skip_variants FOG_LINEAR FOG_EXP FOG_EXP2
			#include "HLSLSupport.cginc"
			#if ( SHADER_API_D3D11 || SHADER_API_GLCORE || SHADER_API_GLES || SHADER_API_GLES3 || SHADER_API_METAL || SHADER_API_VULKAN )
				#define CAN_SKIP_VPOS
			#endif
			#include "UnityCG.cginc"
			#include "Lighting.cginc"
			#include "UnityPBSLighting.cginc"
			sampler3D _DitherMaskLOD;
			struct v2f
			{
				V2F_SHADOW_CASTER;
				float2 customPack1 : TEXCOORD1;
				float4 tSpace0 : TEXCOORD2;
				float4 tSpace1 : TEXCOORD3;
				float4 tSpace2 : TEXCOORD4;
				half4 color : COLOR0;
				UNITY_VERTEX_INPUT_INSTANCE_ID
				UNITY_VERTEX_OUTPUT_STEREO
			};
			v2f vert( appdata_full v )
			{
				v2f o;
				UNITY_SETUP_INSTANCE_ID( v );
				UNITY_INITIALIZE_OUTPUT( v2f, o );
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO( o );
				UNITY_TRANSFER_INSTANCE_ID( v, o );
				Input customInputData;
				float3 worldPos = mul( unity_ObjectToWorld, v.vertex ).xyz;
				half3 worldNormal = UnityObjectToWorldNormal( v.normal );
				half3 worldTangent = UnityObjectToWorldDir( v.tangent.xyz );
				half tangentSign = v.tangent.w * unity_WorldTransformParams.w;
				half3 worldBinormal = cross( worldNormal, worldTangent ) * tangentSign;
				o.tSpace0 = float4( worldTangent.x, worldBinormal.x, worldNormal.x, worldPos.x );
				o.tSpace1 = float4( worldTangent.y, worldBinormal.y, worldNormal.y, worldPos.y );
				o.tSpace2 = float4( worldTangent.z, worldBinormal.z, worldNormal.z, worldPos.z );
				o.customPack1.xy = customInputData.uv_texcoord;
				o.customPack1.xy = v.texcoord;
				TRANSFER_SHADOW_CASTER_NORMALOFFSET( o )
				o.color = v.color;
				return o;
			}
			half4 frag( v2f IN
			#if !defined( CAN_SKIP_VPOS )
			, UNITY_VPOS_TYPE vpos : VPOS
			#endif
			) : SV_Target
			{
				UNITY_SETUP_INSTANCE_ID( IN );
				Input surfIN;
				UNITY_INITIALIZE_OUTPUT( Input, surfIN );
				surfIN.uv_texcoord = IN.customPack1.xy;
				float3 worldPos = float3( IN.tSpace0.w, IN.tSpace1.w, IN.tSpace2.w );
				half3 worldViewDir = normalize( UnityWorldSpaceViewDir( worldPos ) );
				surfIN.worldPos = worldPos;
				surfIN.worldNormal = float3( IN.tSpace0.z, IN.tSpace1.z, IN.tSpace2.z );
				surfIN.internalSurfaceTtoW0 = IN.tSpace0.xyz;
				surfIN.internalSurfaceTtoW1 = IN.tSpace1.xyz;
				surfIN.internalSurfaceTtoW2 = IN.tSpace2.xyz;
				surfIN.vertexColor = IN.color;
				SurfaceOutputCustomLightingCustom o;
				UNITY_INITIALIZE_OUTPUT( SurfaceOutputCustomLightingCustom, o )
				surf( surfIN, o );
				UnityGI gi;
				UNITY_INITIALIZE_OUTPUT( UnityGI, gi );
				o.Alpha = LightingStandardCustomLighting( o, worldViewDir, gi ).a;
				#if defined( CAN_SKIP_VPOS )
				float2 vpos = IN.pos;
				#endif
				half alphaRef = tex3D( _DitherMaskLOD, float3( vpos.xy * 0.25, o.Alpha * 0.9375 ) ).a;
				clip( alphaRef - 0.01 );
				SHADOW_CASTER_FRAGMENT( IN )
			}
			ENDCG
		}
	}
	Fallback "Diffuse"
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=17000
2126;97;956;773;-3441.872;1489.325;2.079234;True;False
Node;AmplifyShaderEditor.FunctionNode;107;4220.669,-774.7583;Float;False;SHA_BASE_Light;0;;12;c7128beafed4a4f4886b76c12c12c219;6,107,0,144,0,109,1,111,1,128,1,130,1;1;108;FLOAT4;0,0,0,0;False;2;COLOR;0;FLOAT;106
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;4664.34,-977.9761;Float;False;True;2;Float;ASEMaterialInspector;0;0;CustomLighting;_LUCEED/BASE_Transparent;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;False;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Transparent;0.5;True;True;0;False;Transparent;;Transparent;All;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;True;2;5;False;-1;10;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;-1;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;15;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT3;0,0,0;False;4;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;0;9;107;106
WireConnection;0;13;107;0
ASEEND*/
//CHKSM=D7AF9C5CD281A1DB84FC4AC11DC69CA1E43666F4