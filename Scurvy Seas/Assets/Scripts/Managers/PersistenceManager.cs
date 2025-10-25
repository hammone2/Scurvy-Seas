using UnityEngine;

public class PersistenceManager : MonoBehaviour
{
    public static PersistenceManager instance;

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


}
