Shader "Custom/Pixelate"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Index ("Refraction Index", Vector) = (1.0, 1.0, 1.0, 1.0)
        _Amount ("Amount", Float) = 1.0
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

        float hash21(float2 p)
        {
            float3 p3 = frac(float3(p.xyx) * .1031);
            p3 += dot(p3, p3.yzx + 33.33);
            return frac((p3.x + p3.y) * p3.z);
        }

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
            float nD = d;
                  d = smoothstep(1.7, 0.9, d);

            fixed4 col = fixed4(tex2D(_MainTex, float2(redTexCoord.x, uv.y)).rgb, 1.0);
            col.g = tex2D(_MainTex, float2(greenTexCoord.x, uv.y)).g;
            col.b = tex2D(_MainTex, float2(blueTexCoord.x,uv.y)).b;
            
            col *= d;
    
            float2 nUv = sUv;
    
            nUv *= 500.;
            nUv = floor(nUv);
            
            nUv /= 500.;
    
            float n = hash21(nUv * iResolution) * 0.07 + 0.93;
            
            n = lerp(n, 1., smoothstep(1.5, 0.2, nD));
    
            return col * n ;
        }
            ENDCG
        }
    }
}
