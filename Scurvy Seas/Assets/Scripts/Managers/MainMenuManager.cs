using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    public void OnNewGamePressed()
    {
        GameManager.instance.isNewGame = true;
        SceneManager.LoadScene("BaseEncounterScene");
    }

    public void OnLoadGamePressed()
    {
        if (!SaveManager.SaveFileExists())
            return;

        GameManager.instance.isNewGame = false;
        SceneManager.LoadScene("BaseEncounterScene");
    }
}
