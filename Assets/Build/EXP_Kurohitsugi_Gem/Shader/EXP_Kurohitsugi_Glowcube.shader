Shader "Custom/BTGlowPulsePro"
{
    Properties
    {
        _Color ("Glow Color", Color) = (1,1,1,1)
        _BaseIntensity ("Base Intensity", Float) = 1
        _PulseStrength ("Pulse Strength", Float) = 2
        _PulseSpeed ("Pulse Speed", Float) = 4
    }

    SubShader
    {
        Tags { "Queue"="Transparent" "RenderType"="Transparent" }
        Blend One One
        ZWrite Off
        Cull Off

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
                float pulse = abs(sin(_Time.y * _PulseSpeed));

                float intensity = (_BaseIntensity + pulse * _PulseStrength) * 4.0;

                float rim = pow(1.0 - saturate(dot(normalize(i.normal), float3(0,0,1))), 3.0);

                return _Color * intensity * (1.0 + rim * 2.0);
            }
            ENDCG
        }
    }
}
