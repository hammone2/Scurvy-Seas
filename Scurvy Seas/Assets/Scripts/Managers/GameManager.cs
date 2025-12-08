using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public bool isNewGame = false;
    public bool isNextLevelAPort = false;
    private int levelIterations = 2;
    private int currentLevelIteration = 0;


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

    public void GoToMapScreen()
    {
        SceneManager.LoadScene("TreasureMapScene");
    }

    public void NextLevel()
    {
        currentLevelIteration++;

        if (isNewGame)
            isNewGame = false;

        GoToMapScreen();
    }

    public void Retreat()
    {
        if (currentLevelIteration > 1)
            currentLevelIteration--;

        GoToMapScreen();
    }

    public void GoToNextLevel()
    {
        if (/*currentLevelIteration % levelIterations == 0*/ isNextLevelAPort)
        {
            //go to tavern
            SceneManager.LoadScene("Port");

            return;
        }

        SceneManager.LoadScene("BaseEncounterScene");
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

    public void QuitToTitle()
    {
        SceneManager.LoadScene("TitleScene");
    }

    public void EndGame()
    {
        SceneManager.LoadScene("GameOverScene");
    }
}
