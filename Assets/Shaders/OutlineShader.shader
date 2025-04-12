Shader "Shaders/OutlineShader"
{
    Properties
    {
        _MainTex ("Base (RGB)", 2D) = "white" {}
        _OutlineColor ("Outline Color", Color) = (0,0,0,1)
        _OutlineThickness ("Thickness", Float) = 0.02
        _DepthSensitivity ("Depth Sensitivity", Float) = 0.5
    }

    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

        Pass
        {
            ZTest Always
            ZWrite Off
            Cull Off
            Blend SrcAlpha OneMinusSrcAlpha

            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Common.hlsl"

            TEXTURE2D(_MainTex);
            SAMPLER(sampler_MainTex);
            float4 _MainTex_TexelSize;
            float4 _OutlineColor;
            float _OutlineThickness;
            float _DepthSensitivity;

            struct Attributes {
                float4 positionOS : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct Varyings {
                float4 positionHCS : SV_POSITION;
                float2 uv : TEXCOORD0;
            };

            Varyings vert(Attributes IN) {
                Varyings OUT;
                OUT.positionHCS = TransformObjectToHClip(IN.positionOS.xyz);
                OUT.uv = IN.uv;
                return OUT;
            }

            float luminance(float3 color) {
                return dot(color, float3(0.299, 0.587, 0.114));
            }

            half4 frag(Varyings i) : SV_Target
            {
                float2 uv = i.uv;
                float2 offset = _OutlineThickness * _MainTex_TexelSize.xy;

                float lumCenter = luminance(SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, uv).rgb);
                float lumRight = luminance(SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, uv + float2(offset.x, 0)).rgb);
                float lumLeft = luminance(SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, uv - float2(offset.x, 0)).rgb);
                float lumUp = luminance(SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, uv + float2(0, offset.y)).rgb);
                float lumDown = luminance(SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, uv - float2(0, offset.y)).rgb);

                float edge = abs(lumCenter - lumRight) + abs(lumCenter - lumLeft) +
                             abs(lumCenter - lumUp) + abs(lumCenter - lumDown);

                float edgeStrength = step(_DepthSensitivity, edge);

                float4 baseCol = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, uv);
                return lerp(baseCol, _OutlineColor, edgeStrength);
            }
            ENDHLSL
        }
    }
}
