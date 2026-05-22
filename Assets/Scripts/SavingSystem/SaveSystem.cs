using UnityEngine;
using System.Collections.Generic;
using System.IO;

public class SaveSystem
{
    [System.Serializable]
    public struct SaveData
    {
        public PlayerSaveData PlayerData;       // Stats
        public SavePointData PointData;         // World Transform  
        public InventorySaveData InventoryData; // Inventory
    }

    private static SaveData _saveData = new SaveData();


    public static void Initialize()
    {
        Load();
    }


    #region Path and Folder
    // El path al que irán los save files
    public static string GetSaveFolderPath()
    {
        string path = Application.persistentDataPath + "/SavedFiles";
        return path;
    }

    // Genera el nombre del SaveFile. FALTA AÑADIR NUMERO POR NUMERO DE SAVE FUL
    public static string SaveFileName()
    {
        string saveFile = GetSaveFolderPath() + "/Save" +  /* Save Number + */ ".save";
        return saveFile;
    }

    // Se asegura de que exista el directorio
    public static void EnsureSaveFolder()
    {
        if (!Directory.Exists(GetSaveFolderPath()))
            Directory.CreateDirectory(GetSaveFolderPath());
    }
    #endregion

    #region Save
    public static void Save(SavePoint savePoint)
    {
        EnsureSaveFolder();

        // Recupera todos los datos que guardar
        HandleSaveData(savePoint);

        // Guardado
        File.WriteAllText(SaveFileName(), JsonUtility.ToJson(_saveData, true));
    }

    // Guardado de datos
    private static void HandleSaveData(SavePoint savePoint)
    {
        savePoint.SaveData(ref _saveData.PointData);
        GameManager.Instancia.PLAYER.SaveData(ref _saveData.PlayerData);
        InventoryManager.Instance.SaveData(ref _saveData.InventoryData);
    }
    #endregion

    #region Load
    public static void Load()
    {
        if (!File.Exists(SaveFileName()))
            return;

        // Lectura
        string jsonFile = File.ReadAllText(SaveFileName());
        _saveData = JsonUtility.FromJson<SaveData>(jsonFile);

        // Cargado
        HandleLoadData();
    }

    // Cargado de datos
    private static void HandleLoadData()
    {
        Debug.Log(_saveData.PointData.position);
        GameManager.Instancia.PLAYER.LoadData(_saveData.PointData, _saveData.PlayerData); 

        if (_saveData.InventoryData.items != null && _saveData.InventoryData.items.Count != 0)
            InventoryManager.Instance.LoadData(_saveData.InventoryData);
    }
    #endregion
}
