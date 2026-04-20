using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class DatosFotos
{
    public string idFoto;
    public string folderRoute;
    public float revealTime;

    public List<GameObject> enemigosCapturados = new List<GameObject>();
    public List<EnemyCollision> enemiesCaught = new List<EnemyCollision>();

    [System.NonSerialized] public float revealProgress;
    [System.NonSerialized] public Texture2D textura;
    [System.NonSerialized] public bool inProcess;

    public bool EstaRevelada => revealProgress >= 1f;

    public DatosFotos(string idFoto, string folderRoute, float revealTime, List<GameObject> objectsCaught = null)
    {
        this.idFoto = idFoto;
        this.folderRoute = folderRoute;
        this.revealTime = revealTime;
        revealProgress = 0f;
        textura = null;

        if (objectsCaught == null)
            return;

        foreach (GameObject item in objectsCaught)
        {
            if (item.TryGetComponent(out EnemyCollision enemy))
            {
                enemiesCaught.Add(enemy);
                Debug.Log(enemy.name);
            }
        }      
    }

    // Carga la textura desde disco solo cuando la necesitas
    public void CargarTextura()
    {
        if (textura != null) return; // Ya está cargada, no duplicar

        if (!System.IO.File.Exists(folderRoute))
        {
            Debug.LogError("No se encontró la foto en: " + folderRoute);
            return;
        }

        // Cargar foto desde archivo
        byte[] bytes = System.IO.File.ReadAllBytes(folderRoute);
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