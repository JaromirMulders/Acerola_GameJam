Shader"Distort/fade"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Fade ("Fade",Float) = 1.
        _Distort ("Distort",Float) = 1.
        _AmpRatio ("AmpRatio", Vector) = (1.0, 1.0, 1.0, 1.0)
        _Frequency ("Frequency",Float) = 1.
        _FreqRatio ("FreqRatio", Vector) = (1.0, 1.0, 1.0, 1.0)
        _StepSize ("StepSize",Float) = 1.
        _StepChance ("StepChance",Float) = 1.
    }
    SubShader
    {
        // No culling or depth
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
        float _Fade;
        float _Distort;
        float4 _FreqRatio;
        float _Frequency;
        float4 _AmpRatio;
        float _StepSize;
        float _StepChance;

        float hash11(float p)
        {
            p = frac(p * .1031);
            p *= p + 33.33;
            p *= p + p;
            return frac(p);
        }

        float getStepped(float n,float p, float stepSize, float stepChance)
        {
            n = floor(n * stepSize) / stepSize;
            float r = hash11(n);
            r = smoothstep(1.0 - stepChance, 1.0, r);
            n = lerp(n * r, n, stepChance);
            return n;
        }

        fixed4 frag(v2f i) : SV_Target
        {
            
            float2 uv = i.uv;
            float2 nUv = i.uv * 2.0 - 1.0;
    
            float iTime = _Time.y;
    
            float distP1 = 0.;
            float distP2 = 0.;
            float distP3 = 0.;
            float distP4 = 0.;

            float dist1 = 0.;
            float dist2 = 0.;
            float dist3 = 0.;
            float dist4 = 0.;
    
            distP1 = nUv.y * _FreqRatio.x * _Frequency + iTime * 0.876;
            distP2 = nUv.y * _FreqRatio.y * _Frequency + iTime * 0.543;
            distP3 = nUv.y * _FreqRatio.z * _Frequency + iTime * 0.231;
            distP4 = nUv.y * _FreqRatio.w * _Frequency + iTime * 0.131;

            dist1 = sin(distP1);
            dist2 = sin(distP2);
            dist3 = sin(distP3);
            dist4 = sin(distP4);
   
            dist1 = getStepped(dist1,distP1,_StepSize,_StepChance);
            dist2 = getStepped(dist2, distP2, _StepSize, _StepChance);
            dist3 = getStepped(dist3, distP3, _StepSize, _StepChance);
            dist4 = getStepped(dist4, distP4, _StepSize, _StepChance);
    
            float xOffset = (dist1 * _AmpRatio.x + dist2 * _AmpRatio.y + dist3 * _AmpRatio.z + dist4 * _AmpRatio.w) * _Distort;
    
            uv.x += xOffset;
    
            fixed4 col = tex2D(_MainTex, uv);
    
            return col;
}
            ENDCG
        }
    }
}
