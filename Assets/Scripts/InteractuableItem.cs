using UnityEngine;

public class InteractuableItem : MonoBehaviour
{
    [Header("Item Info")]
    public Item itemData; // Referencia al ScriptableObject del item
    
    void Start()
    {
        Debug.Log($"<color=green>Cargando item: {itemData.name}</color>");
        ConfigurarVisuales();
    }

    public void ConfigurarVisuales()
    {
        // 1. Verificamos si este item está marcado como foto y tiene sus datos
        if (itemData != null && itemData.esFoto && itemData.datosFoto != null)
        {
            // 2. Cargamos la imagen desde el disco
            itemData.datosFoto.CargarTextura();
            MeshRenderer meshRenderer = GetComponentInChildren<MeshRenderer>();
            // 3. Si tenemos donde pintarla, la aplicamos
            if (meshRenderer != null && itemData.datosFoto.textura != null)
            {
                Material materialInstanciado = new Material(meshRenderer.material);
                
                // Asignamos la textura principal
                materialInstanciado.mainTexture = itemData.datosFoto.textura;
                meshRenderer.material = materialInstanciado;

                FotoRevelado revelador = GetComponent<FotoRevelado>();
                if (revelador != null)
                {
                    revelador.datos = itemData.datosFoto; // Le damos acceso a los datos para que maneje el revelado
                } 

                meshRenderer.material = materialInstanciado;
                Debug.Log($"<color=green>Foto cargada correctamente: {itemData.name}");
            }
            else
            {
                Debug.LogWarning($"El item {itemData.name} es una foto pero le falta el MeshRenderer o la textura no cargó.");
            }
        }
        else
        {

            Debug.Log($"Item normal cargado: {itemData?.name}");
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        /*if (other.CompareTag("Player"))
        {
            bool agregada = InventarioManager.Instance.AgregarItem(itemData); // Intentamos agregar el item al inventario del jugador   

            if (agregada)            
            {
                Destroy(gameObject); 
            }
            else
            {
                Debug.Log("No se pudo agregar el item al inventario. Puede que esté lleno.");
            }
        }*/
    }

    private void OnDestroy()
    {
        if (itemData != null && itemData.esFoto && itemData.datosFoto != null)
        {
            itemData.datosFoto.LiberarTextura();
        }
    }
}
