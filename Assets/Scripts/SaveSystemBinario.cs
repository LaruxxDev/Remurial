using UnityEngine;
using System.IO;
using System.Collections.Generic;
public class SaveSystemBinario : MonoBehaviour {
    string filePath;

    void Awake() {
        // Definimos la ruta persistente
        filePath = Application.persistentDataPath + "/savegame.dat";
    }

    public void SaveGame(PlayerData data) {
        using (BinaryWriter writer = new BinaryWriter(File.Open(filePath, FileMode.Create))) {
            writer.Write(data.inventory.Count);
            foreach (Item item in data.inventory) {
                writer.Write(item.name);
                writer.Write(item.description);
                writer.Write(item.id);
                writer.Write(item.isKeyItem);
                writer.Write(item.isUsable);
                writer.Write(item.quantity);
                writer.Write(item.maxStack);
            }
            writer.Write(data.health);
            writer.Write(data.checkpoint);
            writer.Write(data.configuraciones.Count);
            foreach (string config in data.configuraciones) {
                writer.Write(config);
            }
        }
        Debug.Log("Partida guardada en: " + filePath);
    }

    public PlayerData LoadGame() {
        if (File.Exists(filePath)) {
            PlayerData data = new PlayerData();
            using (BinaryReader reader = new BinaryReader(File.Open(filePath, FileMode.Open))) {
                // IMPORTANTE: Debes leer en el mismo orden exacto en que escribiste
                int inventoryCount = reader.ReadInt32();
                data.inventory = new List<Item>();
                for (int i = 0; i < inventoryCount; i++) {
                    Item item = new Item();
                    item.name = reader.ReadString();
                    item.description = reader.ReadString();
                    item.id = reader.ReadInt32();
                    item.isKeyItem = reader.ReadBoolean();
                    item.isUsable = reader.ReadBoolean();
                    item.quantity = reader.ReadInt32();
                    item.maxStack = reader.ReadInt32();
                    data.inventory.Add(item);
                }
                data.health = reader.ReadSingle();
                data.checkpoint = reader.ReadString();
                int configuracionesCount = reader.ReadInt32();
                data.configuraciones = new List<string>();
                for (int i = 0; i < configuracionesCount; i++) {
                    data.configuraciones.Add(reader.ReadString());
                }
            }
            return data;
        }
        return null;
    }
}