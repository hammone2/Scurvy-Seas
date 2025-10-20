using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class LevelManager : MonoBehaviour
{
    public static LevelManager instance;
    public UnityEvent OnEncounterComplete;
    private List<GameObject> enemies = new List<GameObject>();

    [SerializeField] private GameObject[] enemyPrefabs; //array of spawnable enemies
    [SerializeField] private Transform[] enemySpawnPoints;

    private void Awake()
    {
        //SaveSystem.Load();
        instance = this;
    }

    private void Start()
    {
        int maxEnemies = enemySpawnPoints.Length;
        int minEnemies = 1;
        List<int> visitedPoints = new List<int>();
        int enemiesToSpawn = Random.Range(minEnemies, maxEnemies);

        for (int i = 0; i < enemiesToSpawn; i++)
        {
            int spawnPointIndex = Random.Range(0,enemySpawnPoints.Length);
            if (visitedPoints.Contains(spawnPointIndex))
            {
                for (int j = 0; j < enemySpawnPoints.Length; j++)
                {
                    if (!visitedPoints.Contains(j))
                        spawnPointIndex = j;
                }
            }

            visitedPoints.Add(spawnPointIndex);
            int randomEnemyIndex = Random.Range(0,enemyPrefabs.Length);
            GameObject randomEnemy = Instantiate(enemyPrefabs[randomEnemyIndex], 
                enemySpawnPoints[spawnPointIndex].position, Quaternion.identity);
        }
    }

    public void NextEncounter()
    {
        SaveGame();

        Invoke("LoadNextScene", 2f);
    }

    public void Retreat()
    {
        SaveGame();
        Invoke("MoveBackLevel", 1f);
    }

    private void MoveBackLevel()
    {
        GameManager.instance.Retreat();
    }

    private void LoadNextScene()
    {
        GameManager.instance.NextLevel(SceneManager.GetActiveScene().name);
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

    private void SaveGame()
    {
        SaveData saveData = new SaveData
        {
            PlayerShip = PlayerManager.instance.playerShip.Save(),
            Inventory = PlayerManager.instance.inventorySystem.Save()
        };
        SaveManager.SaveGame(saveData);
    }
}
