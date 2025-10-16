using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    private int levelIterations = 2;
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

        SceneManager.LoadScene(levelName);
    }
}
