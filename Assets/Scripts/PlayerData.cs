using System.Collections.Generic;
using System.IO;

[System.Serializable]
public class PlayerData {
    public List<Item> inventory = new List<Item>();
    public float health = 100.0f;
    public string checkpoint = "hospital";
    public List<string> configuraciones = new List<string>();
}