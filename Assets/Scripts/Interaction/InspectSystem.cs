using UnityEngine;

public class InspectSystem : MonoBehaviour
{
    #region Values
    [Header("References")]
    public Transform AparitionPoint;
    public GameObject rawImageUI; 

    [Header("Settings")]
    public float rotationSpeed = 100f; 

    private GameObject currentInspectedObject; 
    public bool isInspecting { get; private set; } 
    #endregion



    [Header("Prueba")]
    [SerializeField] private PlayerGeneral PLAYER;
    [SerializeField] private GameObject inspectionLight;

    public void CustomUpdate()
    {
        if (!isInspecting || currentInspectedObject == null)
            return;

        Vector2 finalRotation = PLAYER.INPUTTRANSFORMER.INPUTNAVIGATENORMAL;

        if (finalRotation == Vector2.zero)
            return;

        // Rotaciones
        float rotationX = finalRotation.x * rotationSpeed * Time.unscaledDeltaTime;
        float rotationY = finalRotation.y * rotationSpeed * Time.unscaledDeltaTime;

        // Aplicación
        currentInspectedObject.transform.Rotate(Vector3.up, -rotationX, Space.World);
        currentInspectedObject.transform.Rotate(Vector3.right, -rotationY, Space.World);
    }

    public void EnterInspectionMode(GameObject objectToInspect)
    {
        if (isInspecting) 
            return; 
        
        Debug.Log("Entrando en modo inspección con el objeto: " + objectToInspect.name);
        currentInspectedObject = Instantiate(objectToInspect, AparitionPoint.position, Quaternion.identity);
        
        FotoRevelado reveladorNuevo = currentInspectedObject.GetComponent<FotoRevelado>();
        if (reveladorNuevo != null)
        {
            reveladorNuevo.isInspecting = true;
            Debug.Log("Progreso rescatado: " + reveladorNuevo.datos.revealProgress);
        }
        
        MeshRenderer mrOriginal = objectToInspect.GetComponentInChildren<MeshRenderer>();
        MeshRenderer mrNuevo = currentInspectedObject.GetComponentInChildren<MeshRenderer>();
        if (mrOriginal != null && mrNuevo != null)
        {
            mrNuevo.material = new Material(mrOriginal.material);
            mrNuevo.material.mainTexture = mrOriginal.material.mainTexture;
        }

        inspectionLight.SetActive(true);
        rawImageUI.SetActive(true); 
        isInspecting = true;
    }

    public void ExitInspectionMode()
    {
        if (!isInspecting) 
            return;

        FotoRevelado cloneRevelado = currentInspectedObject.GetComponent<FotoRevelado>();
        if (cloneRevelado != null && PLAYER.inspectionItem != null)
        {
            FotoRevelado originalRevelado = PLAYER.inspectionItem.GetComponentInChildren<FotoRevelado>();
            if (originalRevelado != null)
                originalRevelado.datos.revealProgress = cloneRevelado.datos.revealProgress;
        }

        MeshRenderer mr = currentInspectedObject.GetComponentInChildren<MeshRenderer>();
        if (mr != null)
            Destroy(mr.material);

        Destroy(currentInspectedObject);

        inspectionLight.SetActive(false);
        rawImageUI.SetActive(false); 
        isInspecting = false;
    }
}