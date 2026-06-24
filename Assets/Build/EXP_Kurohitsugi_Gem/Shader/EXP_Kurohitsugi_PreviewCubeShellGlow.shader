Shader "EXP/Kurohitsugi/PreviewCubeShellNoiseGlow"
{
    Properties
    {
        _GlowColor ("Glow Color", Color) = (0.35, 0.02, 1.0, 1.0)
        _CoreColor ("Hot Core Color", Color) = (1.0, 0.65, 1.0, 1.0)
        _Intensity ("Intensity", Range(0, 10)) = 2.0
        _RimPower ("Rim Power", Range(0.5, 10)) = 3.5
        _PulseStrength ("Pulse Strength", Range(0, 3)) = 0.35
        _PulseSpeed ("Pulse Speed", Range(0, 12)) = 3.0
        _Alpha ("Alpha", Range(0, 1)) = 0.35

        _NoiseTex ("Noise Texture", 2D) = "white" {}
        _NoiseStrength ("Noise Strength", Range(0, 2)) = 0.65
        _NoiseScale ("Noise Scale", Range(0.2, 8)) = 2.0
        _NoiseSpeed ("Noise Speed", Range(0, 5)) = 0.7
    }

    SubShader
    {
        Tags
        {
            "Queue"="Transparent"
            "RenderType"="Transparent"
            "IgnoreProjector"="True"
        }

        Blend One One
        ZWrite Off
        Cull Front
        Lighting Off

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma target 3.0
            #include "UnityCG.cginc"

            sampler2D _NoiseTex;
            float4 _NoiseTex_ST;

            fixed4 _GlowColor;
            fixed4 _CoreColor;
            float _Intensity;
            float _RimPower;
            float _PulseStrength;
            float _PulseSpeed;
            float _Alpha;
            float _NoiseStrength;
            float _NoiseScale;
            float _NoiseSpeed;

            struct appdata
            {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float4 pos : SV_POSITION;
                float3 normalWS : TEXCOORD0;
                float3 viewDirWS : TEXCOORD1;
                float2 uv : TEXCOORD2;
            };

            v2f vert(appdata v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);

                float3 worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
                o.normalWS = UnityObjectToWorldNormal(v.normal);
                o.viewDirWS = normalize(_WorldSpaceCameraPos.xyz - worldPos);

                o.uv = v.uv * _NoiseScale + float2(_Time.y * _NoiseSpeed, _Time.y * _NoiseSpeed * 0.37);
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                float ndotv = saturate(dot(normalize(i.normalWS), normalize(i.viewDirWS)));
                float rim = pow(1.0 - ndotv, _RimPower);

                float noise = tex2D(_NoiseTex, i.uv).r;
                float noiseBoost = lerp(1.0, noise * 2.0, _NoiseStrength);

                float pulse = 1.0 + abs(sin(_Time.y * _PulseSpeed)) * _PulseStrength;

                float3 col = lerp(_CoreColor.rgb * 0.08, _GlowColor.rgb, rim);
                col *= rim * noiseBoost * _Intensity * pulse * _Alpha;

                return fixed4(col, 1.0);
            }
            ENDCG
        }
    }

    FallBack Off
}
