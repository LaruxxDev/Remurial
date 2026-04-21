Shader "Hidden/PSX_Camera_Effect"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
    }

    SubShader
    {
        Tags { "RenderType"="Opaque" "RenderPipeline"="UniversalPipeline" }
        ZTest Always ZWrite Off Cull Off

        Pass
        {
            Name "PSX_Camera"

            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            struct Attributes
            {
                float4 positionOS : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct Varyings
            {
                float4 positionHCS : SV_POSITION;
                float2 uv : TEXCOORD0;
            };

            TEXTURE2D(_MainTex);
            SAMPLER(sampler_MainTex);

            // --- Parametros controlados desde PSXCameraFeature.cs ---
            float _PixelWidth;      // Resolucion objetivo horizontal (ej: 320)
            float _PixelHeight;     // Resolucion objetivo vertical   (ej: 240)
            float _DitherStrength;  // Intensidad del dithering (0 a 1)
            float _ColorBits;       // Profundidad de color (5 = PS1 autentico)
            float _ScreenWidth;     // Resolucion real de pantalla
            float _ScreenHeight;

            // -------------------------------------------------------
            // Matriz Bayer 4x4 — genera el patron de dithering PS1
            // -------------------------------------------------------
            float BayerValue(int x, int y)
            {
                // Matriz estandar normalizada a [0..1]
                float m[16] = {
                     0.0/16.0,  8.0/16.0,  2.0/16.0, 10.0/16.0,
                    12.0/16.0,  4.0/16.0, 14.0/16.0,  6.0/16.0,
                     3.0/16.0, 11.0/16.0,  1.0/16.0,  9.0/16.0,
                    15.0/16.0,  7.0/16.0, 13.0/16.0,  5.0/16.0
                };
                return m[(x & 3) + (y & 3) * 4];
            }

            // -------------------------------------------------------
            // Reduccion de color a N bits (PS1 usaba 5 bits = 32 niveles)
            // -------------------------------------------------------
            float3 QuantizeColor(float3 color, float bits)
            {
                float levels = pow(2.0, bits) - 1.0;
                return round(color * levels) / levels;
            }

            Varyings vert(Attributes IN)
            {
                Varyings OUT;
                OUT.positionHCS = TransformObjectToHClip(IN.positionOS.xyz);
                OUT.uv = IN.uv;
                return OUT;
            }

            float4 frag(Varyings IN) : SV_Target
            {
                float2 uv = IN.uv;

                // ===================================================
                // EFECTO 1: PIXELACION — baja resolucion PS1
                // Snappea los UVs a la grid de la resolucion objetivo
                // ===================================================
                float2 pixelUV = floor(uv * float2(_PixelWidth, _PixelHeight))
                                 / float2(_PixelWidth, _PixelHeight);

                float4 color = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, pixelUV);

                // ===================================================
                // EFECTO 2: DITHERING BAYER
                // Anade ruido ordenado antes de cuantizar el color,
                // simulando el tramado de transparencias de PS1
                // ===================================================
                if (_DitherStrength > 0.001)
                {
                    // Coordenadas en pixeles de pantalla real
                    int px = (int)(uv.x * _ScreenWidth)  & 3;
                    int py = (int)(uv.y * _ScreenHeight) & 3;

                    float threshold = BayerValue(px, py) - 0.5; // centrado en 0
                    float spread   = _DitherStrength / pow(2.0, _ColorBits); // escala al bit depth

                    color.rgb += threshold * spread;
                }

                // ===================================================
                // EFECTO 3: CUANTIZACION DE COLOR
                // Reduce la paleta a N bits, como la VRAM de PS1
                // ===================================================
                color.rgb = QuantizeColor(color.rgb, _ColorBits);

                return color;
            }
            ENDHLSL
        }
    }
}
