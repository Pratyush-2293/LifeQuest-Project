using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveManager : MonoBehaviour
{
    public static SaveManager instance = null;

    private void Awake()
    {
        if (instance)
        {
            Debug.LogError("Trying to create more than one SaveManager instance!");
            Destroy(gameObject);
            return;
        }
        instance = this;

        DontDestroyOnLoad(gameObject);
    }

    public void SavePlayerTasksData()
    {
        // Data Input
        PlayerTasksData playerTasksSaveData = TaskListMenu.instance.playerTasksData;

        // Data Write
        string writeData = JsonUtility.ToJson(playerTasksSaveData);
        string filePath = Application.persistentDataPath + "/PlayerTasksData.json";
        System.IO.File.WriteAllText(filePath, writeData);
        Debug.Log("Player Tasks Data successfully stored at: " + filePath);
    }

    public void LoadPlayerTasksData()
    {
        // Data Read
        string filePath = Application.persistentDataPath + "/PlayerTasksData.json";
        if (System.IO.File.Exists(filePath))
        {
            string readData = System.IO.File.ReadAllText(filePath);

            // Data Deserialise
            PlayerTasksData data = JsonUtility.FromJson<PlayerTasksData>(readData);
            TaskListMenu.instance.playerTasksData = data;
            Debug.Log("Player Tasks Data loaded successfully.");
        }
        else
        {
            Debug.Log("No Existing PlayerTasksData Save File Was Found.");
        }
    }

    public void SaveGameData()
    {
        GameDataSave gameDataSaveObject = GameData.instance.ToSaveData();

        string writeData = JsonUtility.ToJson(gameDataSaveObject);
        string filePath = Application.persistentDataPath + "/GameData.json";
        System.IO.File.WriteAllText(filePath, writeData);
        Debug.Log("Game Data successfully stored at: " + filePath);
    }

    public void LoadGameData()
    {
        string filePath = Application.persistentDataPath + "/GameData.json";
        if (System.IO.File.Exists(filePath))
        {
            string readData = System.IO.File.ReadAllText(filePath);

            GameDataSave loadedData = JsonUtility.FromJson<GameDataSave>(readData);

            // Apply loaded data to the GameData instance
            GameData.instance.LoadFromSaveData(loadedData);

            Debug.Log("GameData loaded successfully.");
        }
        else
        {
            Debug.Log("No Existing GameData Save File Was Found.");
        }
    }
}
