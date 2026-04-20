using UnityEngine;

[System.Serializable]
public class Item
{
    public string name = "item";
    public string description = "description";
    public int id = 0;
    public bool isKeyItem = false;
    public bool isUsable = false;
    public int quantity = 1;
    public int maxStack = 99;
    public Sprite sprite;

    public bool esFoto = false;
    public PhotoData datosFoto; 

    public GameObject prefabItem;

    // Devuelve el GameObject listo para inspeccionar
    public GameObject ObtenerGameObjectParaInspeccion()
    {
        if (prefabItem == null)
        {
            Debug.LogError("El item " + name + " no tiene prefab de inspección asignado.");
            return null;
        }

        GameObject instancia = GameObject.Instantiate(prefabItem,Vector3.zero, Quaternion.identity);

        // Si es foto, le cargamos la textura
        if (esFoto && datosFoto != null)
        {
            if (datosFoto.texture == null)
            {
                datosFoto.CargarTextura();
            }

            MeshRenderer meshRenderer = instancia.GetComponentInChildren<MeshRenderer>();
            if (meshRenderer != null)
            {
                meshRenderer.material = new Material(meshRenderer.material);
<<<<<<< HEAD:Assets/Scripts/Item.cs
                meshRenderer.material.mainTexture = datosFoto.texture;
=======
                meshRenderer.material.mainTexture = datosFoto.textura;
                Debug.Log("Textura aplicada a la instancia de foto: " + name);
>>>>>>> origin/SergioOrganizando:Assets/Scripts/Interaction/ItemData/Item.cs
            }
            else
            {
                Debug.LogError("El prefab de foto no tiene MeshRenderer.");
            }
        }

        return instancia;
    }
}