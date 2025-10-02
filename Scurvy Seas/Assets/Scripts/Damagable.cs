using UnityEngine;
using UnityEngine.Events;

//[RequireComponent(typeof(Collider))]
public class Damagable : MonoBehaviour
{
    public float health;
    public UnityEvent OnDeath;
    [SerializeField] private Healthbar healthBar;


    private void Start()
    {
        if (healthBar != null)
            healthBar.InitializeHealth(health);
    }

    public void TakeDamage(float damage)
    {
        if (health <= 0)
            return;

        health -= damage;
        if (healthBar != null)
            healthBar.UpdateHealth(health);
        Debug.Log(health);

        if (health <= 0)
            OnDeath?.Invoke();
    }

    public void Heal(float _health)
    {
        health += _health;
    }

    private void OnCollisionEnter(Collision collision)
    {
        Damager damager = collision.gameObject.GetComponent<Damager>();

        if (damager)
        {
            TakeDamage(damager.GetDamage());
            Debug.Log("Contact");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Damager damager = other.gameObject.GetComponent<Damager>();

        if (damager)
        {
            TakeDamage(damager.GetDamage());
            Debug.Log("Contact");
        }
    }
}
