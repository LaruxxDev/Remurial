Shader "PostEffect/Afterimage"
{
    Properties
    {
        _MainTex      ("Frame actual",    2D) = "white" {}
        _CapturedTex  ("Frame capturado", 2D) = "black" {}
        _Alpha        ("Alpha decay",   Float) = 1.0
        _Tint         ("Tint color",    Color) = (1,0.95,0.8,1)
    }

    SubShader
    {
        Tags { "RenderType"="Opaque" "RenderPipeline"="UniversalPipeline" }
        ZTest Always ZWrite Off Cull Off

        Pass
        {
            Name "Afterimage"

            HLSLPROGRAM
            #pragma vertex   Vert
            #pragma fragment Frag

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            TEXTURE2D(_MainTex);
            SAMPLER(sampler_MainTex);

            TEXTURE2D(_CapturedTex);
            SAMPLER(sampler_CapturedTex);

            float4 _Tint;
            float  _Alpha;

            struct Attributes { float4 positionOS : POSITION; float2 uv : TEXCOORD0; };
            struct Varyings   { float4 positionCS : SV_POSITION; float2 uv : TEXCOORD0; };

            Varyings Vert(Attributes IN)
            {
                Varyings OUT;
                OUT.positionCS = TransformObjectToHClip(IN.positionOS.xyz);
                OUT.uv = IN.uv;
                return OUT;
            }

            half4 Frag(Varyings IN) : SV_Target
            {
                // Frame actual (ya procesado por Pixelation y demás)
                half4 current  = SAMPLE_TEXTURE2D(_MainTex,     sampler_MainTex,     IN.uv);

                // Frame capturado en el momento del flash
                half4 captured = SAMPLE_TEXTURE2D(_CapturedTex, sampler_CapturedTex, IN.uv);

                // Aplicar tinte al frame capturado (efecto de quemado de retina)
                // En zonas muy brillantes, la persistencia es mayor
                float luminance = dot(captured.rgb, float3(0.299, 0.587, 0.114));
                float persistence = _Alpha * (0.5 + luminance * 0.5);

                // Mezclar: la imagen capturada se superpone sobre la actual con el tinte
                half4 tinted = half4(captured.rgb * _Tint.rgb, persistence * _Tint.a);

                // Blend: el afterimage se superpone en modo Screen para no oscurecer
                half3 result = 1.0 - (1.0 - current.rgb) * (1.0 - tinted.rgb * tinted.a);

                return half4(result, current.a);
            }
            ENDHLSL
        }
    }
}
