Shader "Custom/Pixelate"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Index ("Refraction Index", Vector) = (1.0, 1.0, 1.0, 1.0)
        _Amount ("Amount", Range(0,1)) = 1.0
    }
    SubShader
    {
    Cull Off
    ZWrite Off
    ZTest Always

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

        v2f vert(appdata v)
        {
            v2f o;
            o.vertex = UnityObjectToClipPos(v.vertex);
            o.uv = v.uv;
            return o;
        }

        sampler2D _MainTex;
        float3 _Index;
        float _Amount;

        fixed4 frag(v2f i) : SV_Target
        {
            float2 uv = i.uv;    
            float2 iResolution = _ScreenParams.xy;
            float2 sUv = uv * 2.0 - 1.0;

            fixed3 refractiveIndex = lerp(fixed3(1.0, 1.0, 1.0), _Index, _Amount);
            fixed2 normalizedTexCoord = uv * 2.0 - 1.0; // [0, 1] -> [-1, 1]
            fixed3 texVec = fixed3(normalizedTexCoord, 1.0);
            fixed3 normalVec = fixed3(0.0, 0.0, -1.0);
            fixed3 redRefractionVec = refract(texVec, normalVec, refractiveIndex.r);
            fixed3 greenRefractionVec = refract(texVec, normalVec, refractiveIndex.g);
            fixed3 blueRefractionVec = refract(texVec, normalVec, refractiveIndex.b);
            fixed2 redTexCoord = ((redRefractionVec.xy / redRefractionVec.z) + 1.0) / 2.0;
            fixed2 greenTexCoord = ((greenRefractionVec.xy / greenRefractionVec.z) + 1.0) / 2.0;
            fixed2 blueTexCoord = ((blueRefractionVec.xy / blueRefractionVec.z) + 1.0) / 2.0;
    
    
            float d = length(sUv);
                  d = smoothstep(1.7, 0.9, d);

            fixed4 col = fixed4(tex2D(_MainTex, redTexCoord).rgb, 1.0);
            col.g = tex2D(_MainTex, greenTexCoord).g;
            col.b = tex2D(_MainTex, blueTexCoord).b;
            
            col *= d;
    
            return col;
        }
            ENDCG
        }
    }
}
