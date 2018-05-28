﻿Shader "Unlit/QuadDeformationShader"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_Color("Color",Color)=(1.0,0.0,0.0,0.0)
		_Distance("Distance",Range(-4.0,4.0))=0.0
		_Position1("CollisionPos1",Vector)=(0.0,0.0,0.0)
		_Position2("CollisionPos2",Vector)=(0.0,0.0,0.0)
		_Position3("CollisionPos3",Vector)=(0.0,0.0,0.0)
		_Position4("CollisionPos4",Vector)=(0.0,0.0,0.0)
		_ViewRange("Range",Range(0.0,6.0))=0.0
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
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				UNITY_FOG_COORDS(1)
				float4 vertex : SV_POSITION;
			};

			sampler2D _MainTex;
			float4 _MainTex_ST;
			float _Distance;
			float3 _Position1;
			float3 _Position2;
			float3 _Position3;
			float3 _Position4;
			float4 _Color;
			float _ViewRange;
			
			v2f vert (appdata v)
			{
				
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				
				float3 worldPos=mul(unity_ObjectToWorld,v.vertex).xyz;
				if(worldPos.z>_Position1.z&&worldPos.x<_Position1.x){
					o.vertex.xyz=_Position1.xyz;
				}
				if(worldPos.z>_Position2.z&&worldPos.x>_Position2.x){
					o.vertex.xyz=_Position2.xyz;
				}
				if(worldPos.z<_Position3.z&&worldPos.x>_Position3.x){
					o.vertex.xyz=_Position3.xyz;
				}
				if(worldPos.x<_Position4.x&&worldPos.z<_Position4.z){
					o.vertex.xyz=_Position4.xyz;
				}
				//else{o.vertex.x-=dis;}
				
				//UNITY_TRANSFER_FOG(o,o.vertex);
				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				// sample the texture
				fixed4 col = tex2D(_MainTex, i.uv);
				// apply fog
				UNITY_APPLY_FOG(i.fogCoord, col);
				col=col*_Color;
				return col;
			}
			ENDCG
		}
	}
}
