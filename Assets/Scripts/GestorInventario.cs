using UnityEngine;
using System.Collections.Generic;

public class GestorInventario : MonoBehaviour
{
    #region Values
    public static GestorInventario Instance { get; private set; }

    [Header("Inventario")]
    public List<PhotoData> fotosEnInventario = new List<PhotoData>();

    [Header("Configuración")]
    [SerializeField] private int maxFotos = 20; // Límite de fotos en inventario
    #endregion

    #region Events
    public event System.Action<PhotoData> OnFotoAgregada;
    public event System.Action<PhotoData> OnFotoEliminada;
    public event System.Action OnInventarioLleno;
    #endregion

    #region Unity Methods
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void OnDestroy()
    {
        // Al destruirse, liberar todas las texturas de memoria
        LiberarTodasLasTexturas();
    }
    #endregion

    #region Inventory Methods
    public bool AgregarFoto(PhotoData foto)
    {
        if (foto == null || string.IsNullOrEmpty(foto.folderRoute))
        {
            Debug.LogError("No se puede agregar una foto nula al inventario.");
            return false;
        }

        if (fotosEnInventario.Count >= maxFotos)
        {
            Debug.LogWarning("Inventario lleno. No se puede agregar más fotos.");
            OnInventarioLleno?.Invoke();
            return false;
        }

        fotosEnInventario.Add(foto);
        OnFotoAgregada?.Invoke(foto);
        Debug.Log($"Foto agregada: {foto.photoID} | Total: {fotosEnInventario.Count}/{maxFotos}");
        return true;
    }

    public bool EliminarFoto(string idFoto)
    {
        PhotoData foto = BuscarFoto(idFoto);

        if (foto == null)
        {
            Debug.LogWarning("No se encontró la foto con id: " + idFoto);
            return false;
        }

        foto.LiberarTextura();
        fotosEnInventario.Remove(foto);
        OnFotoEliminada?.Invoke(foto);
        Debug.Log($"Foto eliminada: {idFoto} | Total: {fotosEnInventario.Count}/{maxFotos}");
        return true;
    }

    // Devuelve la foto por id
    public PhotoData BuscarFoto(string idFoto)
    {
        return fotosEnInventario.Find(f => f.photoID == idFoto);
    }

    // Carga la textura de una foto solo cuando se necesita mostrar
    public void MostrarFoto(string idFoto)
    {
        PhotoData foto = BuscarFoto(idFoto);
        if (foto == null) return;

        foto.CargarTextura();
    }

    // Libera la textura cuando dejas de mostrarla
    public void OcultarFoto(string idFoto)
    {
        PhotoData foto = BuscarFoto(idFoto);
        if (foto == null) return;

        foto.LiberarTextura();
    }

    public bool InventarioLleno() => fotosEnInventario.Count >= maxFotos;
    public int FotosRestantes() => maxFotos - fotosEnInventario.Count;

    private void LiberarTodasLasTexturas()
    {
        foreach (PhotoData foto in fotosEnInventario)
        {
            foto.LiberarTextura();
        }
    }
    #endregion
}