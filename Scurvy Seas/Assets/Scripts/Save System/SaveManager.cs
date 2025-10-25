using UnityEngine;
using System.IO;

public static class SaveManager
{
    private static string savePath = Application.persistentDataPath + "/savefile.json";

    public static void SaveGame(SaveData saveData)
    {
        File.WriteAllText(savePath, JsonUtility.ToJson(saveData, true)); //this line actually creates the file
        Debug.Log("Game Saved to: " + savePath);
    }

    public static SaveData LoadGame()
    {
        if (SaveFileExists())
        {
            string json = File.ReadAllText(savePath);
            SaveData data = JsonUtility.FromJson<SaveData>(json);
            Debug.Log("Game Loaded");
            return data;
        }

        Debug.LogWarning("Save file not found!");
        return null;
    }

    public static void DeleteSaveGame()
    {
        if (SaveFileExists())
        {
            File.Delete(savePath);
        }
    }

    public static bool SaveFileExists()
    {
        return File.Exists(savePath);
    }
}
