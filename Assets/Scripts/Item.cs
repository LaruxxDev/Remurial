using UnityEngine;

[System.Serializable]
public class Item {
    public string name = "item";
    public string description = "description";
    public int id = 0;
    public bool isKeyItem = false;
    public bool isUsable = false;
    public int quantity = 1;
    public int maxStack = 99;
    public Sprite sprite;
}