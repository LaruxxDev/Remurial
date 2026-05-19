using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace PSX
{
    public class AfterimagPass : ScriptableRenderPass
    {
        // Shader debe estar en Resources/Shaders/ o en una carpeta referenciada
        private static readonly string shaderPath = "PostEffect/Afterimage";
        private static readonly string k_RenderTag = "Render Afterimage Effect";

        // IDs de propiedades del shader
        private static readonly int MainTexId       = Shader.PropertyToID("_MainTex");
        private static readonly int CapturedTexId   = Shader.PropertyToID("_CapturedTex");
        private static readonly int TempTargetId    = Shader.PropertyToID("_TempTargetAfterimage");
        private static readonly int AlphaId         = Shader.PropertyToID("_Alpha");
        private static readonly int TintId          = Shader.PropertyToID("_Tint");

        private Material afterimageMaterial;
        private RenderTargetIdentifier currentTarget;

        // Textura capturada en el momento del flash
        private RenderTexture capturedFrame;

        // Control de estado
        private bool captureRequested = false;
        private bool isActive = false;
        private float timer = 0f;

        // Parámetros configurables desde el Feature
        private float decayRate;
        private float duration;
        private Color tintStart;
        private Color tintEnd;

        public bool IsActive => isActive;

        public AfterimagPass(float decayRate, float duration, Color tintStart, Color tintEnd)
        {
            this.decayRate  = decayRate;
            this.duration   = duration;
            this.tintStart  = tintStart;
            this.tintEnd    = tintEnd;

            var shader = Shader.Find(shaderPath);
            if (shader == null)
            {
                Debug.LogError($"[Afterimage] Shader no encontrado en: {shaderPath}");
                return;
            }
            afterimageMaterial = CoreUtils.CreateEngineMaterial(shader);
        }

        public void Setup(in RenderTargetIdentifier target)
        {
            currentTarget = target;
        }

        // Llamado por AfterimagFeature.TriggerFlash()
        public void RequestCapture()
        {
            captureRequested = true;
        }

        public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
        {
            if (afterimageMaterial == null) return;
            if (!renderingData.cameraData.postProcessEnabled) return;

            // Ignorar cámaras que no sean la principal
            if (renderingData.cameraData.cameraType != CameraType.Game) return;

            if (!captureRequested && !isActive) return;

            var cmd = CommandBufferPool.Get(k_RenderTag);

            ref var cameraData = ref renderingData.cameraData;
            var w = cameraData.camera.scaledPixelWidth;
            var h = cameraData.camera.scaledPixelHeight;

            // Si hay petición de captura, guardamos el frame actual
            if (captureRequested)
            {
                captureRequested = false;

                // Liberar la textura anterior si existía
                if (capturedFrame != null)
                    capturedFrame.Release();

                capturedFrame = new RenderTexture(w, h, 0, RenderTextureFormat.Default);
                cmd.Blit(currentTarget, capturedFrame);

                // Arrancar el decay
                isActive = true;
                timer = 0f;
            }

            // Renderizar el efecto si está activo
            if (isActive && capturedFrame != null)
            {
                timer += Time.deltaTime;
                float t = Mathf.Clamp01(timer / duration);

                // Alpha con curva exponencial (se siente más orgánico que lineal)
                float alpha = Mathf.Pow(decayRate, timer * 60f); // 60fps base

                // Tinte interpolado: blanco cálido → naranja → transparente
                Color tint = Color.Lerp(tintStart, tintEnd, t);
                tint.a = alpha;

                afterimageMaterial.SetTexture(CapturedTexId, capturedFrame);
                afterimageMaterial.SetFloat(AlphaId, alpha);
                afterimageMaterial.SetColor(TintId, tint);

                cmd.SetGlobalTexture(MainTexId, currentTarget);
                cmd.GetTemporaryRT(TempTargetId, w, h, 0, FilterMode.Bilinear, RenderTextureFormat.Default);
                cmd.Blit(currentTarget, TempTargetId);
                cmd.Blit(TempTargetId, currentTarget, afterimageMaterial, 0);
                cmd.ReleaseTemporaryRT(TempTargetId);

                // Desactivar al terminar
                if (t >= 1f)
                {
                    isActive = false;
                    capturedFrame.Release();
                    capturedFrame = null;
                }
            }

            context.ExecuteCommandBuffer(cmd);
            CommandBufferPool.Release(cmd);
        }

        // Limpieza al destruir el pass
        public void Cleanup()
        {
            if (capturedFrame != null)
            {
                capturedFrame.Release();
                capturedFrame = null;
            }
            CoreUtils.Destroy(afterimageMaterial);
        }
    }
}
