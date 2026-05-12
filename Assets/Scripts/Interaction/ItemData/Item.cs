using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "NewItem", menuName = "Inventario/Item")]
public class Item : ScriptableObject
{
    public new string name = "item";
    public string description = "description";
    public int id = 0;
    public bool isKeyItem = false;
    public bool isUsable = false;
    public int quantity = 1;
    public int maxStack = 99;
    public Sprite sprite;

    public bool esFoto = false;
    public DatosFotos datosFoto; 

    public GameObject prefabItem;

    // Devuelve el GameObject listo para inspeccionar
    public GameObject ObtenerGameObjectParaInspeccion()
    {
        if (prefabItem == null)
        {
            Debug.LogError("El item " + name + " no tiene prefab de inspección asignado." + prefabItem);
            return null;
        }

        GameObject instancia = GameObject.Instantiate(prefabItem,Vector3.zero, Quaternion.identity);

        // Si es foto, le cargamos la textura
        if (esFoto && datosFoto != null)
        {
            if (datosFoto.textura == null)
            {
                datosFoto.CargarTextura();
            }

            MeshRenderer meshRenderer = instancia.GetComponentInChildren<MeshRenderer>();
            if (meshRenderer != null)
            {
                meshRenderer.material = new Material(meshRenderer.material);
                meshRenderer.material.mainTexture = datosFoto.textura;
                Debug.Log("Textura aplicada a la instancia de foto: " + name);
            }
            else
            {
                Debug.LogError("El prefab de foto no tiene MeshRenderer.");
            }
        }

        return instancia;
    }
}