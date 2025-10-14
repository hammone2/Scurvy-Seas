using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class LevelManager : MonoBehaviour
{
    public static LevelManager instance;
    public UnityEvent OnEncounterComplete;
    private List<GameObject> enemies = new List<GameObject>();

    private void Awake()
    {
        //SaveSystem.Load();
        instance = this;
    }

    public void NextEncounter()
    {
        SaveData saveData = new SaveData
        {
            PlayerShip = PlayerManager.instance.playerShip.Save()
        };
        SaveManager.SaveGame(saveData);

        Invoke("LoadNextScene", 2f);
    }

    private void LoadNextScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void AddEnemy(GameObject enemy)
    {
        enemies.Add(enemy);
    }

    public void RemoveEnemy(GameObject enemy)
    {
        enemies.Remove(enemy);

        if (enemies.Count == 0)
            OnEncounterComplete?.Invoke();
    }
}
