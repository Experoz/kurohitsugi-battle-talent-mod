Shader "EXP/Kurohitsugi/DissolveRadialTop"
{
    Properties
    {
        _BaseColor ("Base Color", Color) = (0.01, 0.0, 0.03, 1)
        _NoiseTex ("Noise Texture", 2D) = "white" {}
        _NoiseScale ("Noise Scale", Float) = 1.75
        _NoiseStrength ("Noise Strength", Range(0, 1)) = 0.35

        _DissolveAmount ("Dissolve Amount", Range(-0.5, 1.5)) = 0
        _EdgeWidth ("Edge Width", Range(0.001, 0.25)) = 0.035
        _EdgeColor ("Edge Color", Color) = (0.25, 0.05, 1.0, 1)
        _EmissionStrength ("Emission Strength", Range(0, 5)) = 1.5

        _Alpha ("Alpha", Range(0, 1)) = 0.8
    }

    SubShader
    {
        Tags
        {
            "RenderType"="TransparentCutout"
            "Queue"="AlphaTest"
            "RenderPipeline"="UniversalPipeline"
        }

        Pass
        {
            Name "ForwardUnlit"
            Tags { "LightMode"="UniversalForward" }

            Cull Off
            ZWrite On
            ZTest LEqual

            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma target 3.0

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            TEXTURE2D(_NoiseTex);
            SAMPLER(sampler_NoiseTex);

            CBUFFER_START(UnityPerMaterial)
                float4 _BaseColor;
                float4 _EdgeColor;

                float _NoiseScale;
                float _NoiseStrength;

                float _DissolveAmount;
                float _EdgeWidth;
                float _EmissionStrength;

                float _Alpha;
            CBUFFER_END

            struct Attributes
            {
                float4 positionOS : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct Varyings
            {
                float4 positionHCS : SV_POSITION;
                float2 uv : TEXCOORD0;
                float3 positionOS : TEXCOORD1;
            };

            Varyings vert(Attributes IN)
            {
                Varyings OUT;

                VertexPositionInputs positionInputs = GetVertexPositionInputs(IN.positionOS.xyz);

                OUT.positionHCS = positionInputs.positionCS;
                OUT.uv = IN.uv;
                OUT.positionOS = IN.positionOS.xyz;

                return OUT;
            }

            half4 frag(Varyings IN) : SV_Target
            {
                float2 noiseUV = IN.uv * _NoiseScale;
                float noise = SAMPLE_TEXTURE2D(_NoiseTex, sampler_NoiseTex, noiseUV).r;

                // Forma quadrata/radiale sul piano XZ del cubo.
                // Centro = 0, bordo esterno = 1.
                float squareRadius = max(abs(IN.positionOS.x), abs(IN.positionOS.z)) * 2.0;
                squareRadius = saturate(squareRadius);

                float noisyRadius = saturate(squareRadius + ((noise - 0.5) * _NoiseStrength));

                // Dissolve dall'esterno verso il centro:
                // amount basso = nascosto, amount alto = pieno.
                float threshold = 1.0 - _DissolveAmount;

                float visibleMask = step(threshold, noisyRadius);
                float edgeMask = step(threshold - _EdgeWidth, noisyRadius) - visibleMask;

                clip(visibleMask + edgeMask - 0.01);

                float3 baseColor = _BaseColor.rgb * visibleMask;
                float3 edgeColor = _EdgeColor.rgb * edgeMask * _EmissionStrength;

                float3 finalColor = baseColor + edgeColor;

                return half4(finalColor, _Alpha);
            }
            ENDHLSL
        }
    }

    FallBack Off
}
