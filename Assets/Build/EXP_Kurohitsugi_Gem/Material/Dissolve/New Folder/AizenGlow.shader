Shader "Custom/AizenGlow"
{
    Properties
    {
        _Color ("Glow Color", Color) = (0.6,0.2,1,1)
        _BaseIntensity ("Base Intensity", Float) = 1
        _PulseStrength ("Pulse Strength", Float) = 2
        _PulseSpeed ("Pulse Speed", Float) = 6
        _RimPower ("Rim Power", Float) = 2
        _RimIntensity ("Rim Intensity", Float) = 3
    }

    SubShader
    {
        Tags { "Queue"="Transparent" "RenderType"="Transparent" }
        Blend One One
        ZWrite Off
        Cull Back

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            fixed4 _Color;
            float _BaseIntensity;
            float _PulseStrength;
            float _PulseSpeed;
            float _RimPower;
            float _RimIntensity;

            struct appdata {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
            };

            struct v2f {
                float4 pos : SV_POSITION;
                float3 normal : TEXCOORD0;
            };

            v2f vert (appdata v) {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.normal = UnityObjectToWorldNormal(v.normal);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // Pulsazione energetica
                float pulse = abs(sin(_Time.y * _PulseSpeed));
                float intensity = (_BaseIntensity + pulse * _PulseStrength) * 4.0;

                // Rim light stile anime
                float rim = pow(1.0 - saturate(dot(normalize(i.normal), float3(0,0,1))), _RimPower);
                rim *= _RimIntensity;

                return _Color * (intensity + rim);
            }
            ENDCG
        }
    }
}
