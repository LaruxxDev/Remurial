using System.Collections.Generic;
using UnityEngine;

public class DetectorEnemigosEnFoto : MonoBehaviour
{
    [Header("Configuración")]
    [SerializeField] private Camera         camaraFoto;      // la cámara con la que disparas
    [SerializeField] private LayerMask      capasEnemigos;
    [SerializeField] private LayerMask      capasObstaculos; // muros, geometría
    [SerializeField] private float          anguloMaximo    = 50f;  // FOV horizontal
    [SerializeField] private float          distanciaMaxima = 20f;
    [SerializeField] [Range(0f, 1f)] 
    private float                           visibilidadMinima = 0.5f; // 50% de visibilidad requerida
    [SerializeField] private bool           mostrarGizmos   = true;

    // Resultado del último disparo — otros sistemas lo leen
    public IReadOnlyList<GameObject> UltimaCaptura => _ultimaCaptura;
    private List<GameObject> _ultimaCaptura = new();

    // ── API pública ──────────────────────────────────────────────────────
    public List<GameObject> DetectarEnemigosEnFoto()
    {
        _ultimaCaptura.Clear();

        // Planos del frustum de la cámara para el test AABB
        Plane[] planosFrustum = GeometryUtility.CalculateFrustumPlanes(camaraFoto);

        // Cogemos todos los colliders en la esfera de distancia máxima
        Collider[] candidatos = Physics.OverlapSphere(
            camaraFoto.transform.position,
            distanciaMaxima,
            capasEnemigos
        );

        foreach (Collider col in candidatos)
        {
            // ── Filtro 1: ¿Está dentro del frustum? ─────────────────────
            if (!GeometryUtility.TestPlanesAABB(planosFrustum, col.bounds))
                continue;

            // ── Filtro 2: ¿Está dentro del ángulo de la cámara? ─────────
            Vector3 dirAlEnemigo = col.bounds.center - camaraFoto.transform.position;
            float angulo = Vector3.Angle(camaraFoto.transform.forward, dirAlEnemigo);
            if (angulo > anguloMaximo)
                continue;

            // ── Filtro 3: ¿Se ve más del 50% de su cuerpo? ──────────────
            float visibilidad = CalcularVisibilidad(col, camaraFoto.transform.position);
            if (visibilidad <= visibilidadMinima)
                continue;

            // ── Pasó los 3 filtros: está en la foto ──────────────────────
            float distancia   = dirAlEnemigo.magnitude;
            float prominencia = CalcularProminencia(col, camaraFoto);

            var resultado = col.gameObject.transform.parent?.gameObject ?? col.gameObject; // Si el enemigo tiene un collider hijo, tomamos el padre

            _ultimaCaptura.Add(resultado);

            Debug.Log($"[Foto] Enemigo capturado: {resultado.name} " +
                      $"| dist: {distancia:F1}m | ángulo: {angulo:F1}° " +
                      $"| visibilidad: {visibilidad:P0} | prominencia: {prominencia:P0}");
        }

        return _ultimaCaptura;
    }

    // ── Visibilidad: qué % del enemigo no está bloqueado por paredes ────
    private float CalcularVisibilidad(Collider col, Vector3 origenCamara)
    {
        Bounds b = col.bounds;
        
        // Comprobamos el centro y las 8 esquinas de su caja de colisión (9 puntos en total)
        Vector3[] puntosDePrueba = {
            b.center,
            new Vector3(b.min.x, b.min.y, b.min.z),
            new Vector3(b.max.x, b.min.y, b.min.z),
            new Vector3(b.min.x, b.max.y, b.min.z),
            new Vector3(b.max.x, b.max.y, b.min.z),
            new Vector3(b.min.x, b.min.y, b.max.z),
            new Vector3(b.max.x, b.min.y, b.max.z),
            new Vector3(b.min.x, b.max.y, b.max.z),
            new Vector3(b.max.x, b.max.y, b.max.z)
        };

        int puntosVisibles = 0;

        foreach (Vector3 punto in puntosDePrueba)
        {
            // Si el rayo NO choca con un obstáculo, significa que ese punto se ve
            if (!Physics.Linecast(origenCamara, punto, capasObstaculos))
            {
                puntosVisibles++;
            }
        }

        // Devolvemos la proporción (ej: 5 de 9 puntos = 0.55f)
        return (float)puntosVisibles / puntosDePrueba.Length;
    }

    // ── Prominencia: qué % de la pantalla ocupa el enemigo ──────────────
    private float CalcularProminencia(Collider col, Camera cam)
    {
        Bounds b = col.bounds;
        Vector3[] esquinas = {
            new(b.min.x, b.min.y, b.min.z), new(b.max.x, b.min.y, b.min.z),
            new(b.min.x, b.max.y, b.min.z), new(b.max.x, b.max.y, b.min.z),
            new(b.min.x, b.min.y, b.max.z), new(b.max.x, b.min.y, b.max.z),
            new(b.min.x, b.max.y, b.max.z), new(b.max.x, b.max.y, b.max.z),
        };

        float minX = 1f, maxX = 0f, minY = 1f, maxY = 0f;
        foreach (Vector3 esquina in esquinas)
        {
            Vector3 vp = cam.WorldToViewportPoint(esquina);
            if (vp.z < 0) continue; // detrás de la cámara
            minX = Mathf.Min(minX, vp.x);
            maxX = Mathf.Max(maxX, vp.x);
            minY = Mathf.Min(minY, vp.y);
            maxY = Mathf.Max(maxY, vp.y);
        }

        float anchoViewport  = Mathf.Clamp01(maxX - minX);
        float altoViewport   = Mathf.Clamp01(maxY - minY);
        return anchoViewport * altoViewport; // área en viewport space (0-1)
    }

    // ── Gizmos para debug ────────────────────────────────────────────────
    private void OnDrawGizmosSelected()
    {
        if (!mostrarGizmos || camaraFoto == null) return;

        // Cono de detección
        Gizmos.color = new Color(0f, 1f, 1f, 0.15f);
        Vector3 origen = camaraFoto.transform.position;
        Vector3 forward = camaraFoto.transform.forward;

        for (int i = 0; i < 16; i++)
        {
            float t   = i / 16f * 360f * Mathf.Deg2Rad;
            Vector3 dir = Quaternion.AngleAxis(anguloMaximo, camaraFoto.transform.right)
                          * forward;
            dir = Quaternion.AngleAxis(i / 16f * 360f, forward) * dir;
            Gizmos.DrawLine(origen, origen + dir * distanciaMaxima);
        }

        // Resultados del último disparo
        Gizmos.color = Color.red;
        foreach (var e in _ultimaCaptura)
            if (e.gameObject != null)
                Gizmos.DrawWireSphere(e.gameObject.transform.position, 0.5f);
    }
}