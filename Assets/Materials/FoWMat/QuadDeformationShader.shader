Shader "Unlit/QuadDeformationShader"
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
		Lighting Off
		Tags { "RenderType"="Transparency" }
		LOD 150

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

			sampler2D _MainTex;
			float4 _MainTex_ST;
			float _Distance;
			float4 _Position1;
			float4 _Position2;
			float4 _Position3;
			float4 _Position4;
			float4 _Color;
			
			v2f vert (appdata v)
			{
				
				v2f o;
				//o.vertex = UnityObjectToClipPos(v.vertex);
				//o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				//o.uv=v.uv;
				float3 worldPos=mul(unity_ObjectToWorld,v.vertex).xyz;
				if(worldPos.x<_Position1.x&&worldPos.z>_Position1.z){
					/*float offSetX,offSetZ;
					offSetX=length(_Position1.x-worldPos.x);
					offSetZ=length(_Position1.z-worldPos.z);
					o.vertex.x+=0.01f;
					o.vertex.z-=0.01f;
					*/
					v.vertex=_Position1;

				}
				if(worldPos.x<_Position4.x&&worldPos.z<_Position4.z){
					//o.vertex.x=_Position4.x;
					//o.vertex.z=_Position4.z;
					/*float offSetX,offSetZ;
					offSetX=length(_Position4.x-worldPos.x);
					offSetZ=length(_Position4.z-worldPos.z);
					v.vertex.x+=offSetX;
					v.vertex.z+=offSetZ;*/
					v.vertex=_Position4;
				}
				
				if(worldPos.x>_Position2.x&&worldPos.z>_Position2.z){
					//o.vertex.x=_Position2.x;
					//o.vertex.z=_Position2.z;
					/*float offSetX,offSetZ;
					offSetX=length(_Position2.x-worldPos.x);
					offSetZ=length(_Position2.z-worldPos.z);
					v.vertex.x-=offSetX;
					v.vertex.z-=offSetZ;
					*/
					v.vertex=_Position2;
				}
				if(worldPos.x>_Position3.x&&worldPos.z<_Position3.z){
					//o.vertex.x=_Position3.x;
					//o.vertex.z=_Position3.z;
					/*float offSetX,offSetZ;
					offSetX=length(_Position3.x-worldPos.x);
					offSetZ=length(_Position3.z-worldPos.z);
					v.vertex.x-=offSetX;
					v.vertex.z+=offSetZ;
					*/
					v.vertex=_Position3;
				}
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				
				
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
