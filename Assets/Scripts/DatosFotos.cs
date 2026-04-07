using UnityEngine;

[System.Serializable]
public class DatosFotos
{
    public string idFoto; // Id de la foto
    public string rutaDelArchivoLocal; // Ruta del archivo local donde se guardará el png
    public float revealTime; // Tiempo de revelado de la foto

     // — Estado en runtime (no se serializa) —
    [System.NonSerialized] public float revealProgress;  // 0 → 1
    [System.NonSerialized] public Texture2D textura;     // cargada en memoria
    [System.NonSerialized] public bool estaEnProceso;    // ¿se está revelando ahora?

    public bool EstaRevelada => revealProgress >= 1f;

    public DatosFotos(string id, string ruta, float tiempo)
    {
        idFoto = id;
        rutaDelArchivoLocal = ruta;
        revealTime = tiempo;
        revealProgress = 0f;
    }
}
