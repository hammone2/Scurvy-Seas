using UnityEngine;

public class PortManager : MonoBehaviour
{
    public void NextLevel()
    {
        GameManager.instance.NextLevel("BaseEncounterScene");
    }
}
