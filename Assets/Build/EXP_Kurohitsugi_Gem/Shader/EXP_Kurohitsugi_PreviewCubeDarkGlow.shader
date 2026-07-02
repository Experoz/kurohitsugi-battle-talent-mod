Shader "EXP/Kurohitsugi/PreviewCubeDarkGlow"
{
    Properties
    {
        _BaseColor ("Base Color", Color) = (0.005, 0.0, 0.02, 1.0)
        _GlowColor ("Glow Color", Color) = (0.25, 0.03, 1.0, 1.0)
        _GlowStrength ("Glow Strength", Range(0, 3)) = 0.45
        _FresnelPower ("Fresnel Power", Range(0.5, 8)) = 2.5
    }

    SubShader
    {
        Tags
        {
            "RenderType"="Opaque"
            "Queue"="Geometry"
            "RenderPipeline"="UniversalPipeline"
        }

        Pass
        {
            Name "ForwardUnlit"
            Tags { "LightMode"="UniversalForward" }

            Cull Back
            ZWrite On
            ZTest LEqual

            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma target 3.0

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            CBUFFER_START(UnityPerMaterial)
                float4 _BaseColor;
                float4 _GlowColor;
                float _GlowStrength;
                float _FresnelPower;
            CBUFFER_END

            struct Attributes
            {
                float4 positionOS : POSITION;
                float3 normalOS : NORMAL;
            };

            struct Varyings
            {
                float4 positionHCS : SV_POSITION;
                float3 normalWS : TEXCOORD0;
                float3 viewDirWS : TEXCOORD1;
            };

            Varyings vert(Attributes IN)
            {
                Varyings OUT;
                VertexPositionInputs positionInputs = GetVertexPositionInputs(IN.positionOS.xyz);
                VertexNormalInputs normalInputs = GetVertexNormalInputs(IN.normalOS);
                OUT.positionHCS = positionInputs.positionCS;
                OUT.normalWS = normalize(normalInputs.normalWS);
                OUT.viewDirWS = normalize(GetWorldSpaceViewDir(positionInputs.positionWS));
                return OUT;
            }

            half4 frag(Varyings IN) : SV_Target
            {
                float ndotv = saturate(dot(normalize(IN.normalWS), normalize(IN.viewDirWS)));
                float fresnel = pow(1.0 - ndotv, _FresnelPower);
                float3 col = _BaseColor.rgb + (_GlowColor.rgb * fresnel * _GlowStrength);
                return half4(col, 1.0);
            }
            ENDHLSL
        }
    }

    FallBack Off
}
