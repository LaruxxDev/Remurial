using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class PhotoData
{
    public string photoID;
    public string folderRoute;
    public float revealTime;

    public List<EnemyCollision> enemiesCaught = new List<EnemyCollision>();

    [System.NonSerialized] public float revealProgress;
    [System.NonSerialized] public Texture2D texture;
    [System.NonSerialized] public bool inProcess;

    public bool EstaRevelada => revealProgress >= 1f;

    public PhotoData(string photoID, string folderRoute, float revealTime, List<EnemyCollision> enemiesCaught = null)
    {
        this.photoID = photoID;
        this.folderRoute = folderRoute;
        this.revealTime = revealTime;
        revealProgress = 0f;
        texture = null;

        if (enemiesCaught != null)
            this.enemiesCaught = enemiesCaught;
    }

    // Carga la textura desde disco solo cuando la necesitas
    public void CargarTextura()
    {
        if (texture != null) return; // Ya está cargada, no duplicar

        if (!System.IO.File.Exists(folderRoute))
        {
            Debug.LogError("No se encontró la foto en: " + folderRoute);
            return;
        }

        // Cargar foto desde archivo
        byte[] bytes = System.IO.File.ReadAllBytes(folderRoute);
        texture = new Texture2D(2, 2);
        texture.LoadImage(bytes);

        Debug.Log("Textura cargada desde disco: " + photoID);
    }

    // Libera la textura de memoria cuando no la necesitas
    public void LiberarTextura()
    {
        if (texture == null) return;

        Object.Destroy(texture);
        texture = null;

        Debug.Log("Textura liberada de memoria: " + photoID);
    }
}