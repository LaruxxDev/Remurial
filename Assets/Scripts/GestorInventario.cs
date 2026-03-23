using UnityEngine;
using System.Collections.Generic;
public class GestorInventario : MonoBehaviour
{
    #region Values
    public static GestorInventario Instance { get; private set; }

    [Header("Inventario")]
    public List<DatosFotos> fotosEnInventario = new List<DatosFotos>(); // Lista para almacenar las fotos en el inventario

    #endregion
    #region Unity Methods
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Mantener el gestor de inventario entre escenas
        }
        else
        {
            Destroy(gameObject); // Asegurar que solo haya una instancia del gestor de inventario
        }
    }
    #endregion

    #region Inventory Methods
    public void AgregarFoto(DatosFotos foto)
    {
        if (foto != null && !string.IsNullOrEmpty(foto.rutaDelArchivoLocal))
        {
            fotosEnInventario.Add(foto);
            Debug.Log("Foto agregada al inventario. Total de fotos: " + fotosEnInventario.Count);
        }
        else
        {
            Debug.LogError("No se puede agregar una foto nula al inventario.");
        }
    }
    #endregion
}
