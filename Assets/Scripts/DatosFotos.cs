using UnityEngine;

[System.Serializable]
public class DatosFotos
{
    public string idFoto; // Id de la foto
    public string rutaDelArchivoLocal; // Ruta del archivo local donde se guardará el png

    public DatosFotos(string id, string ruta)
    {
        idFoto = id;
        rutaDelArchivoLocal = ruta;
    }
}
