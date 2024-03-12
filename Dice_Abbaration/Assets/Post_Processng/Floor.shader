Shader "Custom/Floor"
{
    Properties
    {
        _Color ("Color", Color) = (1,1,1,1)
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
        _Glossiness ("Smoothness", Range(0,1)) = 0.5
        _Metallic ("Metallic", Range(0,1)) = 0.0
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 200

        CGPROGRAM
        // Physically based Standard lighting model, and enable shadows on all light types
        #pragma surface surf Standard fullforwardshadows

        // Use shader model 3.0 target, to get nicer looking lighting
        #pragma target 3.0

        sampler2D _MainTex;

        struct Input
        {
            float2 uv_MainTex;
        };

        half _Glossiness;
        half _Metallic;
        fixed4 _Color;

        // GLSL-like functions adapted for Unity
        float hash22(float2 p)
        {
            float3 p3 = frac(float3(p.xyx) * float3(.1031, .1030, .0973));
            p3 += dot(p3, p3.yzx + 33.33);
            return frac((p3.x + p3.y) * p3.z);
        }

        float voronoi(float2 x, float w, float offset)
        {
            float2 n = floor(x);
            float2 f = frac(x);

            float m = 8.0;
            for (int j = -2; j <= 2; j++)
            {
                for (int i = -2; i <= 2; i++)
                {
                    float2 g = float2(float(i), float(j));
                    float2 o = hash22(n + g);

                    o = offset + 0.3 * sin(_Time.y * 0.2 + 6.2831 * o + x);

                    float d = length(g - f + o);

                    float h = smoothstep(-1.0, 1.0, (m - d) / w);
                    m = lerp(m, d, h) - h * (1.0 - h) * w / (1.0 + 3.0 * w);
                }
            }

            return m;
        }

        void surf(Input IN, inout SurfaceOutputStandard o)
        {
            // Normalized pixel coordinates (from 0 to 1)
            float2 uv = IN.uv_MainTex;

            float d = length(uv * 2. - 1.);
            d = smoothstep(-0.2, 1., d);
    
            uv *= 10.0;
    
            uv *= 75.;
            uv = floor(uv);
            uv /= 75.;

            float s1 = voronoi(uv, 0.001, 0.5);
            float s2 = voronoi(uv, 0.1, 0.5);
            float s3 = voronoi(uv + float2(s1,s2) - float2(0.,_Time.y) * 0.2, 0.05, 0.5);
            s3 = smoothstep(0.7, 0., s3);
    
            float s = smoothstep(0.0, 0.01, s1 - s2);
            s *= s3;
    
            s = lerp(s, 0., d );
    
            s = s * 0.2 + 0.8;
    
            // Albedo comes from a texture tinted by color
            fixed4 c = _Color;
            o.Albedo = c.rgb * s;
            // Metallic and smoothness come from slider variables
            o.Metallic = _Metallic;
            o.Smoothness = _Glossiness;
            o.Alpha = c.a;
        }
        ENDCG
    }
    FallBack "Diffuse"
}