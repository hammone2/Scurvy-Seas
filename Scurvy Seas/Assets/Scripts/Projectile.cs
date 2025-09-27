using UnityEngine;

public class Projectile : MonoBehaviour
{
    void Start()
    {
        Invoke("Despawn", 3f);
    }

    private void Despawn()
    {
        Destroy(gameObject);
    }
}
