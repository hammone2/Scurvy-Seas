using UnityEngine;

[RequireComponent(typeof(Damager))]
public class Projectile : MonoBehaviour
{
    [SerializeField] private Transform trail;

    void Start()
    {
        Invoke("Despawn", 3f);
    }

    public void SetDamage(int amount)
    {
        GetComponent<Damager>().damage = amount;
    }

    private void Despawn()
    {
        if (trail)
            trail.SetParent(null, true); //unparent the trail so its not instantly destroyed
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
