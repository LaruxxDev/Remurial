using UnityEngine;

public class SavePoint : MonoBehaviour, IInteractable
{
    [Header("ID")]
    [SerializeField] private string ID;
    [SerializeField] private string interactText;

    [Header("World Transform")]
    [SerializeField] private Vector3 position;
    [SerializeField] private Quaternion rotation;

    public void SaveData(ref SavePointData data)
    {
        data.ID = ID;
        data.position = position;
        data.rotation = rotation;
    }

    // Borrable en futuro
    public void OnInteract()
    {
        SaveSystem.Save(this);
        Debug.Log("Saved in: " + this);
    }

    public void Interact(GameObject interactor)
    {
        SaveSystem.Save(this);
        Debug.Log("Saved in: " + this);
        Debug.Log("Saved through Interact!");
    }

    public string GetInteractText()
    {
        return interactText;
    }
}

[System.Serializable]
public struct SavePointData
{
    // ID
    public string ID;

    // World Transform
    public Vector3 position;
    public Quaternion rotation;
}