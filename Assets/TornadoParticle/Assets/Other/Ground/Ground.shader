// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "VFX/Ground"
{
	Properties
	{
		_Albedo("Albedo", 2D) = "white" {}
		_AlbedoColor("AlbedoColor", Color) = (0,0,0,0)
		_Normal("Normal", 2D) = "bump" {}
		_NormalPower("NormalPower", Float) = 0
		_Noise("Noise", 2D) = "white" {}
		_NoiseNormal("NoiseNormal", 2D) = "bump" {}
		_Tiling("Tiling", Vector) = (1,1,0,0)
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Opaque"  "Queue" = "Geometry+0" }
		Cull Back
		CGPROGRAM
		#include "UnityStandardUtils.cginc"
		#include "UnityShaderVariables.cginc"
		#pragma target 3.0
		#pragma surface surf Standard keepalpha addshadow fullforwardshadows 
		struct Input
		{
			float2 uv_texcoord;
		};

		uniform sampler2D _Normal;
		uniform float2 _Tiling;
		uniform sampler2D _NoiseNormal;
		uniform float _NormalPower;
		uniform float4 _AlbedoColor;
		uniform sampler2D _Albedo;
		uniform sampler2D _Noise;

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float2 temp_output_3_0 = ( i.uv_texcoord * _Tiling );
			float2 panner4 = ( 1.0 * _Time.y * float2( 0.05,0.02 ) + temp_output_3_0);
			float2 panner5 = ( 1.0 * _Time.y * float2( -0.03,-0.04 ) + temp_output_3_0);
			float3 break17 = BlendNormals( UnpackNormal( tex2D( _Normal, temp_output_3_0 ) ) , BlendNormals( UnpackNormal( tex2D( _NoiseNormal, panner4 ) ) , UnpackNormal( tex2D( _NoiseNormal, panner5 ) ) ) );
			float2 appendResult25 = (float2(break17.x , break17.y));
			float3 appendResult30 = (float3(( appendResult25 * _NormalPower ) , break17.z));
			o.Normal = appendResult30;
			float4 temp_output_31_0 = ( _AlbedoColor * ( tex2D( _Albedo, temp_output_3_0 ).r + ( pow( tex2D( _Noise, panner4 ).r , 4.27 ) * ( ( _SinTime.y + 1.0 ) * 0.5 ) ) + ( pow( tex2D( _Noise, panner5 ).r , 4.27 ) * ( ( _SinTime.x + 1.0 ) * 0.5 ) ) ) );
			o.Albedo = temp_output_31_0.rgb;
			o.Smoothness = temp_output_31_0.r;
			o.Alpha = 1;
		}

		ENDCG
	}
	Fallback "Diffuse"
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=16400
-1673;63;1666;974;2902.834;75.16505;1.3;True;False
Node;AmplifyShaderEditor.TextureCoordinatesNode;32;-3329.125,-122.8719;Float;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.Vector2Node;1;-3248.198,1.002192;Float;False;Property;_Tiling;Tiling;6;0;Create;True;0;0;False;0;1,1;220,200;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;3;-3021.198,-16.99782;Float;False;2;2;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.PannerNode;5;-2399.964,573.3361;Float;False;3;0;FLOAT2;0,0;False;2;FLOAT2;-0.03,-0.04;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.PannerNode;4;-2432.745,-104.163;Float;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0.05,0.02;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SamplerNode;7;-1926.452,875.5842;Float;True;Property;_NoiseNormalRef;NoiseNormalRef;5;0;Create;True;0;0;False;0;cbd9b05e08fbb0042bffadffea982b18;cbd9b05e08fbb0042bffadffea982b18;True;0;True;bump;Auto;True;Instance;6;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;6;-1913.86,668.5792;Float;True;Property;_NoiseNormal;NoiseNormal;5;0;Create;True;0;0;False;0;cbd9b05e08fbb0042bffadffea982b18;cbd9b05e08fbb0042bffadffea982b18;True;0;True;bump;Auto;True;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.BlendNormalsNode;9;-1570.941,775.3452;Float;False;0;3;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SamplerNode;8;-1909.44,461.4461;Float;True;Property;_Normal;Normal;2;0;Create;True;0;0;False;0;9d1b55c53315e4949bf107dfd98eae5c;9d1b55c53315e4949bf107dfd98eae5c;True;0;True;bump;Auto;True;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.BlendNormalsNode;15;-1323.015,646.8571;Float;False;0;3;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SamplerNode;12;-1955.651,-19.04177;Float;True;Property;_NoiseRef;NoiseRef;4;0;Create;True;0;0;False;0;0c2a32b28f53f144d83b4e49f6472cd7;0c2a32b28f53f144d83b4e49f6472cd7;True;0;False;white;Auto;False;Instance;14;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SinTimeNode;13;-1779.653,-285.9562;Float;False;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;14;-1990.26,-638.8729;Float;True;Property;_Noise;Noise;4;0;Create;True;0;0;False;0;0c2a32b28f53f144d83b4e49f6472cd7;0c2a32b28f53f144d83b4e49f6472cd7;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;10;-1885.501,-398.6387;Float;False;Constant;_Float0;Float 0;4;0;Create;True;0;0;False;0;4.27;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;11;-1899.886,213.9332;Float;False;Constant;_Float1;Float 1;4;0;Create;True;0;0;False;0;4.27;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.BreakToComponentsNode;17;-1017.79,541.5241;Float;False;FLOAT3;1;0;FLOAT3;0,0,0;False;16;FLOAT;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4;FLOAT;5;FLOAT;6;FLOAT;7;FLOAT;8;FLOAT;9;FLOAT;10;FLOAT;11;FLOAT;12;FLOAT;13;FLOAT;14;FLOAT;15
Node;AmplifyShaderEditor.FunctionNode;21;-1369.091,-114.3077;Float;False;ConstantBiasScale;-1;;3;63208df05c83e8e49a48ffbdce2e43a0;0;3;3;FLOAT;0;False;1;FLOAT;1;False;2;FLOAT;0.5;False;1;FLOAT;0
Node;AmplifyShaderEditor.FunctionNode;20;-1493.646,-332.0864;Float;False;ConstantBiasScale;-1;;2;63208df05c83e8e49a48ffbdce2e43a0;0;3;3;FLOAT;0;False;1;FLOAT;1;False;2;FLOAT;0.5;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;18;-1924.491,-921.671;Float;True;Property;_Albedo;Albedo;0;0;Create;True;0;0;False;0;641531eaa101edf458ce7c68d6456dba;641531eaa101edf458ce7c68d6456dba;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.PowerNode;19;-1588.599,-617.6011;Float;True;2;0;FLOAT;0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.PowerNode;16;-1543.103,60.29615;Float;True;2;0;FLOAT;0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;23;-1206.761,-550.8338;Float;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;22;-1084.622,21.09619;Float;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;24;-670.1108,606.2242;Float;False;Property;_NormalPower;NormalPower;3;0;Create;True;0;0;False;0;0;0.29;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.DynamicAppendNode;25;-702.7039,437.7162;Float;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.WireNode;26;-918.6207,-646.804;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;27;-686.0349,-315.575;Float;False;3;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;29;-740.7438,-5.708882;Float;False;Property;_AlbedoColor;AlbedoColor;1;0;Create;True;0;0;False;0;0,0,0,0;0.161,0.09031707,0.01963415,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;28;-492.7749,323.7243;Float;False;2;2;0;FLOAT2;0,0;False;1;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;31;-388.9839,2.938212;Float;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.DynamicAppendNode;30;-283.3339,309.3353;Float;False;FLOAT3;4;0;FLOAT2;0,0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;0,0;Float;False;True;2;Float;ASEMaterialInspector;0;0;Standard;VFX/Ground;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Opaque;0.5;True;True;0;False;Opaque;;Geometry;All;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;True;0;0;False;-1;0;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;-1;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;3;0;32;0
WireConnection;3;1;1;0
WireConnection;5;0;3;0
WireConnection;4;0;3;0
WireConnection;7;1;5;0
WireConnection;6;1;4;0
WireConnection;9;0;6;0
WireConnection;9;1;7;0
WireConnection;8;1;3;0
WireConnection;15;0;8;0
WireConnection;15;1;9;0
WireConnection;12;1;5;0
WireConnection;14;1;4;0
WireConnection;17;0;15;0
WireConnection;21;3;13;1
WireConnection;20;3;13;2
WireConnection;18;1;3;0
WireConnection;19;0;14;1
WireConnection;19;1;10;0
WireConnection;16;0;12;1
WireConnection;16;1;11;0
WireConnection;23;0;19;0
WireConnection;23;1;20;0
WireConnection;22;0;16;0
WireConnection;22;1;21;0
WireConnection;25;0;17;0
WireConnection;25;1;17;1
WireConnection;26;0;18;1
WireConnection;27;0;26;0
WireConnection;27;1;23;0
WireConnection;27;2;22;0
WireConnection;28;0;25;0
WireConnection;28;1;24;0
WireConnection;31;0;29;0
WireConnection;31;1;27;0
WireConnection;30;0;28;0
WireConnection;30;2;17;2
WireConnection;0;0;31;0
WireConnection;0;1;30;0
WireConnection;0;4;31;0
ASEEND*/
//CHKSM=A648F3FACDC5297125008364542E51F23664E8E8