Shader "EXP/Kurohitsugi/InnerDarknessSmartInterior"
{
    Properties
    {
        _BaseColor ("Base Color", Color) = (0.0, 0.0, 0.0, 1.0)
        _InsideBoundsMargin ("Inside Bounds Margin", Range(0, 0.5)) = 0.04
    }

    SubShader
    {
        Tags
        {
            "RenderType"="Opaque"
            "Queue"="Geometry+10"
            "RenderPipeline"="UniversalPipeline"
        }

        // PASS 1: normale interno.
        // Da fuori non domina, da dentro dà comunque pareti nere interne.
        Pass
        {
            Name "InteriorFaces"
            Tags { "LightMode"="UniversalForward" }

            Cull Front
            ZWrite On
            ZTest LEqual

            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma target 3.0

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            CBUFFER_START(UnityPerMaterial)
                float4 _BaseColor;
                float _InsideBoundsMargin;
            CBUFFER_END

            struct Attributes
            {
                float4 positionOS : POSITION;
            };

            struct Varyings
            {
                float4 positionHCS : SV_POSITION;
            };

            Varyings vert(Attributes IN)
            {
                Varyings OUT;
                VertexPositionInputs positionInputs = GetVertexPositionInputs(IN.positionOS.xyz);
                OUT.positionHCS = positionInputs.positionCS;
                return OUT;
            }

            half4 frag(Varyings IN) : SV_Target
            {
                return half4(_BaseColor.rgb, 1.0);
            }
            ENDHLSL
        }

        // PASS 2: maschera totale SOLO se la camera è dentro il cubo.
        // Serve a nascondere spike/nemico/oggetti davanti alla camera quando il player è imprigionato dentro.
        Pass
        {
            Name "InsideCameraBlackout"
            Tags { "LightMode"="SRPDefaultUnlit" }

            Cull Off
            ZWrite Off
            ZTest Always

            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma target 3.0

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            CBUFFER_START(UnityPerMaterial)
                float4 _BaseColor;
                float _InsideBoundsMargin;
            CBUFFER_END

            struct Attributes
            {
                float4 positionOS : POSITION;
            };

            struct Varyings
            {
                float4 positionHCS : SV_POSITION;
                float3 cameraOS : TEXCOORD0;
            };

            Varyings vert(Attributes IN)
            {
                Varyings OUT;
                VertexPositionInputs positionInputs = GetVertexPositionInputs(IN.positionOS.xyz);
                OUT.positionHCS = positionInputs.positionCS;

                // Trasformiamo la posizione camera in object space del cubo.
                OUT.cameraOS = TransformWorldToObject(_WorldSpaceCameraPos);

                return OUT;
            }

            half4 frag(Varyings IN) : SV_Target
            {
                float3 c = abs(IN.cameraOS);

                // Cube mesh standard: bounds locali circa -0.5..0.5 su XYZ.
                float limit = 0.5 + _InsideBoundsMargin;

                // Se la camera NON è dentro al volume, non disegniamo questo pass.
                clip(limit - c.x);
                clip(limit - c.y);
                clip(limit - c.z);

                return half4(_BaseColor.rgb, 1.0);
            }
            ENDHLSL
        }
    }

    FallBack Off
}
