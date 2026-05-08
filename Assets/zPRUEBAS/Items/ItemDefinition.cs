using UnityEngine;

[CreateAssetMenu(menuName = "Game/Item Definition")]
public class ItemDefinition : ScriptableObject
{
    [Header("ID")]
    public int ID;

    [Header("Values")]
    public string itemName;
    public string description; 
    public int maxStack = 99;

    [Header("Designations")]
    public bool isKeyItem;
    public bool isUsable;
    public bool isPhoto;

    [Header("References")]
    public Sprite sprite;
    public GameObject prefabPickableItem;
    public GameObject prefabInspectionItem;
}
