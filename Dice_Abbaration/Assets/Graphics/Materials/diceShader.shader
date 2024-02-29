Shader "Unlit/diceShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _SideOne ("Texture", 2D) = "white" {}
        _SideTwo ("Texture", 2D) = "white" {}
        _SideThree ("Texture", 2D) = "white" {}
        _SideFour ("Texture", 2D) = "white" {}
        _SideFive ("Texture", 2D) = "white" {}
        _SideSix ("Texture", 2D) = "white" {}
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
            sampler2D _SideOne;
            sampler2D _SideTwo;
            sampler2D _SideThree;
            sampler2D _SideFour;
            sampler2D _SideFive;
            sampler2D _SideSix;


            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 col = tex2D(_MainTex, i.uv);

                float4 newCol = float4(0.,0.,0.,0.);
    
                if (col.r == 1.)
                {
                    newCol = tex2D(_SideOne, i.uv);
                }
    
    
                return newC;
            }
            ENDCG
        }
    }
}
