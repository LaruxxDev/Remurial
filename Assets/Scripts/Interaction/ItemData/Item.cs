using UnityEngine;

public class Item
{
    public ItemDefinition definition;
    public int quantity;
    public DatosFotos datosFoto;

    // Custom Overrides
    public string customName;
    public string customDescription;

    // Values
    public int ID => definition.ID;
    public string itemName =>
        string.IsNullOrWhiteSpace(customName)
            ? definition.itemName
            : customName;

    public string description =>
        string.IsNullOrWhiteSpace(customDescription)
            ? definition.description
            : customDescription;
    public int maxStack => definition.maxStack;

    // Designations
    public bool isKeyItem => definition.isKeyItem;
    public bool isUsable => definition.isUsable;
    public bool isPhoto => definition.isPhoto;

    // References
    public Sprite sprite => definition.sprite;
    public GameObject prefabItem => definition.prefabInspectionItem;
    public GameObject pickupPrefab => definition.prefabPickableItem;


    public Item(ItemDefinition definition, int quantity = 1)
    {
        this.definition = definition;
        this.quantity = quantity;
    }

    // Intenta devolver el objeto para la inspección
    public GameObject GetObjectForInspection()
    {
        if (prefabItem == null)
            return null;

        GameObject instance = Object.Instantiate(prefabItem, Vector3.zero, Quaternion.identity);

        if (isPhoto && datosFoto != null)
        {
            if (datosFoto.textura == null)
                datosFoto.CargarTextura();

            MeshRenderer mRenderer = instance.GetComponentInChildren<MeshRenderer>();

            if (mRenderer != null)
            {
                mRenderer.material = new Material(mRenderer.material);
                mRenderer.material.mainTexture = datosFoto.textura;
            }
        }

        return instance;
    }

    public bool HasValidSprite() => definition.sprite != null;
}