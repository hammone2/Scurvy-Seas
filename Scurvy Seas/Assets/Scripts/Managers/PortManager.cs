using UnityEngine;

public class PortManager : MonoBehaviour
{
    private void Start()
    {
        LoadInventoryData();
    }

    public void NextLevel()
    {
        SaveInventoryData();

        GameManager.instance.NextLevel("BaseEncounterScene");
    }

    public void LoadInventoryData()
    {
        SaveData saveData = SaveManager.LoadGame();
        if (saveData is null)
            return;

        InventorySystem.instance.Load(saveData);
    }

    public void SaveInventoryData()
    {
        SaveData doNotOverrideThisData = SaveManager.LoadGame();
        SaveData saveData = new SaveData
        {
            PlayerShip = doNotOverrideThisData.PlayerShip,
            Inventory = InventorySystem.instance.Save()
        };
        SaveManager.SaveGame(saveData);
    }
}
