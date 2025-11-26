using UnityEngine;

public class YargManager : MonoBehaviour
{
    [SerializeField] private AudioSource sound;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Y))
        {
            sound.Play();
        }
    }
}
