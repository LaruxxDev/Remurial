using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace PSX
{
    public class AfterimagFeature : ScriptableRendererFeature
    {
        // Singleton para que CameraController pueda llamar a TriggerFlash()
        public static AfterimagFeature Instance { get; private set; }

        AfterimagPass afterimagePass;

        [Header("Configuración del Afterimage")]
        [Range(0.80f, 0.99f)]
        public float decayRate = 0.92f;          // Cuánto se desvanece por frame (0.92 = orgánico)
        public float duration = 1.8f;             // Duración total en segundos
        public Color tintStart = new Color(1f, 0.98f, 0.85f, 1f);   // Blanco cálido
        public Color tintEnd   = new Color(0.8f, 0.4f, 0.1f, 0f);   // Naranja desvanecido

        public override void Create()
        {
            Instance = this;
            afterimagePass = new AfterimagPass(decayRate, duration, tintStart, tintEnd)
            {
                // Después de todo el postprocess, incluyendo Pixelation
                renderPassEvent = RenderPassEvent.AfterRenderingPostProcessing
            };
        }

        public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
        {
            // Solo se encola si hay un afterimage activo
            //if (afterimagePass.IsActive)
            renderer.EnqueuePass(afterimagePass);
        }

        public override void SetupRenderPasses(ScriptableRenderer renderer, in RenderingData renderingData)
        {
            afterimagePass.Setup(renderer.cameraColorTargetHandle);
        }

        // CameraController llama a esto cuando dispara el flash
        public void TriggerFlash()
        {
            afterimagePass.RequestCapture();
        }
    }
}
