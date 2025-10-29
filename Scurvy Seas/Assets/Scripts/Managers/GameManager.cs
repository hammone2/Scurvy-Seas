using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public bool isNewGame = false;
    private int levelIterations = 1;
    private int currentLevelIteration = 1;


    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this); //make this persistent
            return;
        }
        Destroy(gameObject);
    }

    public void NextLevel(string levelName)
    {
        currentLevelIteration++;
        if (currentLevelIteration > levelIterations)
        {
            //go to tavern
            currentLevelIteration = 0;
            SceneManager.LoadScene("Port");

            return;
        }

        if (isNewGame)
            isNewGame = false;

        SceneManager.LoadScene(levelName);
    }

    public void Retreat()
    {
        if (currentLevelIteration > 1)
            currentLevelIteration--;

        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public int GetCurrentLevel()
    {
        return currentLevelIteration;
    }

    public void RestartGame()
    {
        currentLevelIteration = 1;
        SceneManager.LoadScene("BaseEncounterScene");
    }

    public void EndGame()
    {
        SceneManager.LoadScene("GameOverScene");
    }
}
