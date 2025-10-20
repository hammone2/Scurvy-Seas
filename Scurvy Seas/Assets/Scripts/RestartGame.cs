using UnityEngine;
using UnityEngine.SceneManagement;

public class RestartGame : MonoBehaviour
{
    public void NewGame()
    {
        GameManager.instance.RestartGame();
    }
}
