using UnityEngine;

[RequireComponent(typeof(Damager))]
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

    private void OnCollisionEnter(Collision collision)
    {
        Despawn();
    }

    private void OnTriggerEnter(Collider other)
    {
        Despawn();
    }
}
