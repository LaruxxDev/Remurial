using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class DatosFotos
{
    [Header("Datos")]
    public string idFoto;
    public string folderRoute;
    public float revealTime;

    [Header("Enemigos")]
    public List<EnemyCollision> enemiesCaught = new List<EnemyCollision>();

    [System.NonSerialized] public float revealProgress;
    [System.NonSerialized] public Texture2D textura;
    [System.NonSerialized] public bool inProcess;
    [System.NonSerialized] public Sprite spriteCache;

    public bool EstaRevelada => revealProgress >= 1f;

    public DatosFotos(string idFoto, string folderRoute, float revealTime, List<EnemyCollision> objectsCaught = null)
    {
        this.idFoto = idFoto;
        this.folderRoute = folderRoute;
        this.revealTime = revealTime;
        revealProgress = 0f;
        textura = null;

        if (objectsCaught == null)
            return;

        foreach (EnemyCollision enemy in objectsCaught)
        {
            enemiesCaught.Add(enemy);
        }      
    }

    // Carga la textura desde disco solo cuando la necesitas
    public void CargarTextura()
    {
        if (textura != null && spriteCache != null) return; // Ya está cargada, no duplicar

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

        spriteCache = Sprite.Create(textura, new Rect(0, 0, textura.width, textura.height), new Vector2(0.5f, 0.5f));
    }

    // Libera la textura de memoria cuando no la necesitas
    public void LiberarTextura()
    {
        if (spriteCache != null)
        {
            Object.Destroy(spriteCache);
            spriteCache = null;
        }

        if (textura != null)
        {
            Object.Destroy(textura);
            textura = null;

            Debug.Log("Textura liberada de memoria: " + idFoto);
        }
    }
}