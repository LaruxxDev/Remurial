using UnityEngine;

public class SavePoint : MonoBehaviour
{
    [Header("ID")]
    [SerializeField] private string ID;

    [Header("World Transform")]
    [SerializeField] private Vector3 position;
    [SerializeField] private Quaternion rotation;

    public void SaveData(ref SavePointData data)
    {
        data.ID = ID;
        data.position = position;
        data.rotation = rotation;
    }


    public void OnInteract()
    {
        SaveSystem.Save(this);
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