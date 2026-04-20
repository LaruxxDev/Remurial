using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

// ================================================================
//  PSX Camera Feature (Actualizado para URP 2022/2023+)
//  Aplica pixelado + dithering Bayer a toda la camara (URP)
//  Compatible con Cinemachine — va despues de todos los efectos
// ================================================================

public class PSXCameraFeature : ScriptableRendererFeature
{
    // ---------------------------------------------------------
    // Ajustes expuestos en el Inspector del Renderer
    // ---------------------------------------------------------
    [System.Serializable]
    public class Settings
    {
        [Header("Pixelado (resolucion PS1)")]
        [Tooltip("Resolucion horizontal objetivo. PS1 clasico = 320")]
        [Range(64, 960)] public int targetWidth = 320;

        [Tooltip("Resolucion vertical objetivo. PS1 clasico = 240")]
        [Range(48, 540)] public int targetHeight = 240;

        [Header("Dithering Bayer")]
        [Tooltip("0 = sin dithering, 1 = maximo. Recomendado: 0.4 - 0.8")]
        [Range(0f, 1f)] public float ditherStrength = 0.6f;

        [Header("Profundidad de color")]
        [Tooltip("Bits de color. PS1 usaba 5 bits (32 niveles por canal)")]
        [Range(2f, 8f)] public float colorBits = 5f;
    }

    public Settings settings = new Settings();

    // ---------------------------------------------------------
    // Internos
    // ---------------------------------------------------------
    private PSXCameraPass _pass;

    public override void Create()
    {
        _pass = new PSXCameraPass(settings);
        // Se ejecuta despues de Cinemachine y post-process de URP
        _pass.renderPassEvent = RenderPassEvent.AfterRenderingPostProcessing;
    }

    public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
    {
        // Solo aplica en la camara de juego, no en Scene View ni previews
        if (renderingData.cameraData.cameraType != CameraType.Game) return;

        renderer.EnqueuePass(_pass);
    }

    // Es crucial liberar los recursos en el nuevo sistema RTHandle
    protected override void Dispose(bool disposing)
    {
        _pass?.Dispose();
    }
}


// ================================================================
//  Pass interno — hace el blit con el shader PSX
// ================================================================
internal class PSXCameraPass : ScriptableRenderPass
{
    private readonly PSXCameraFeature.Settings _settings;
    private Material _material;
    
    // Modern URP usa RTHandle en lugar de RenderTargetHandle/Identifier
    private RTHandle _tempHandle;

    private static readonly int ID_PixelWidth     = Shader.PropertyToID("_PixelWidth");
    private static readonly int ID_PixelHeight    = Shader.PropertyToID("_PixelHeight");
    private static readonly int ID_DitherStrength = Shader.PropertyToID("_DitherStrength");
    private static readonly int ID_ColorBits      = Shader.PropertyToID("_ColorBits");
    private static readonly int ID_ScreenWidth    = Shader.PropertyToID("_ScreenWidth");
    private static readonly int ID_ScreenHeight   = Shader.PropertyToID("_ScreenHeight");

    public PSXCameraPass(PSXCameraFeature.Settings settings)
    {
        _settings = settings;

        Shader shader = Shader.Find("Hidden/PSX_Camera_Effect");
        if (shader != null)
        {
            _material = new Material(shader) { hideFlags = HideFlags.HideAndDontSave };
        }
        else
        {
            Debug.LogError(
                "[PSXCameraFeature] No se encontro 'Hidden/PSX_Camera_Effect'.\n" +
                "Asegurate de que PSX_Camera_Effect.shader esta dentro de tu carpeta Assets."
            );
        }
    }

    public override void OnCameraSetup(CommandBuffer cmd, ref RenderingData renderingData)
    {
        // Inicializamos el RTHandle temporal basándonos en la cámara actual
        var desc = renderingData.cameraData.cameraTargetDescriptor;
        desc.depthBufferBits = 0; // No necesitamos buffer de profundidad para este efecto de color
        
        RenderingUtils.ReAllocateIfNeeded(ref _tempHandle, desc, name: "_PSX_TempRT");
    }

    public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
    {
        if (_material == null) return;

        var cmd = CommandBufferPool.Get("PSX Camera Effect");
        
        // Obtenemos el target de la cámara usando la API moderna
        var source = renderingData.cameraData.renderer.cameraColorTargetHandle;
        var desc = renderingData.cameraData.cameraTargetDescriptor;

        // Pasar todos los parametros al shader
        _material.SetFloat(ID_PixelWidth,     _settings.targetWidth);
        _material.SetFloat(ID_PixelHeight,    _settings.targetHeight);
        _material.SetFloat(ID_DitherStrength, _settings.ditherStrength);
        _material.SetFloat(ID_ColorBits,      _settings.colorBits);
        _material.SetFloat(ID_ScreenWidth,    desc.width);
        _material.SetFloat(ID_ScreenHeight,   desc.height);

        // Blit moderno en URP: source -> temp (con efecto) -> source
        Blitter.BlitCameraTexture(cmd, source, _tempHandle, _material, 0);
        Blitter.BlitCameraTexture(cmd, _tempHandle, source);

        context.ExecuteCommandBuffer(cmd);
        CommandBufferPool.Release(cmd);
    }

    public void Dispose()
    {
        // Limpiamos los RTHandles y materiales creados dinámicamente
        _tempHandle?.Release();
        if (_material != null) CoreUtils.Destroy(_material);
    }
}