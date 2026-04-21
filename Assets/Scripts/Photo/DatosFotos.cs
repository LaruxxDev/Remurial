using UnityEngine;
using System.Collections.Generic;
[System.Serializable]
public class DatosFotos
{
    public string idFoto;
    public string rutaDelArchivoLocal;
    public float revealTime;
    public List<GameObject> enemigosCapturados = new();

    [System.NonSerialized] public float revealProgress;
    [System.NonSerialized] public Texture2D textura;
    [System.NonSerialized] public bool estaEnProceso;

    public bool EstaRevelada => revealProgress >= 1f;

    public DatosFotos(string id, string ruta, float tiempo, List<GameObject> enemigos)
    {
        idFoto = id;
        rutaDelArchivoLocal = ruta;
        revealTime = tiempo;
        revealProgress = 0f;
        enemigosCapturados = enemigos;
        textura = null;
    }

    // Carga la textura desde disco solo cuando la necesitas
    public void CargarTextura()
    {
        if (textura != null) return; // Ya está cargada, no duplicar

        if (!System.IO.File.Exists(rutaDelArchivoLocal))
        {
            Debug.LogError("No se encontró la foto en: " + rutaDelArchivoLocal);
            return;
        }

        byte[] bytes = System.IO.File.ReadAllBytes(rutaDelArchivoLocal);
        textura = new Texture2D(2, 2);
        textura.LoadImage(bytes);
        Debug.Log("Textura cargada desde disco: " + idFoto);
    }

    // Libera la textura de memoria cuando no la necesitas
    public void LiberarTextura()
    {
        if (textura == null) return;

        Object.Destroy(textura);
        textura = null;
        Debug.Log("Textura liberada de memoria: " + idFoto);
    }
}