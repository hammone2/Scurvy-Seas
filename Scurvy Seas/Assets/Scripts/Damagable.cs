using UnityEngine;
using UnityEngine.Events;

//[RequireComponent(typeof(Collider))]
public class Damagable : MonoBehaviour
{
    [SerializeField] private float HealthValue = 100f;
    [SerializeField] private GameObject damagePopup;
    
    private float _health;
    public float health
    {
        get { return _health; }
        set
        {
            if (value == _health) return;

            _health = value;

            if (healthBar != null)
            {
                healthBar.UpdateHealth(health);
            }

            Debug.Log(health);
        }
    }


    public UnityEvent OnDeath;
    public event System.Action<float> OnDamageTaken = delegate (float f) { };
    public event System.Action<float> OnHealed = delegate (float f) { };

    [SerializeField] private Healthbar healthBar;


    private void Start()
    {
        health = HealthValue;
        if (healthBar != null)
            healthBar.InitializeHealth(health);
    }

    public void TakeDamage(float damage)
    {
        if (health <= 0)
            return;

        health -= damage;

        TextPopup popup = Instantiate(damagePopup, transform.position, Quaternion.identity).GetComponent<TextPopup>();
        popup.SetTextValue(""+-damage, 36, Color.red);

        if (health <= 0)
            OnDeath?.Invoke();

        OnDamageTaken(damage);
    }

    public void Heal(float _health)
    {
        health += _health;
        if (health > HealthValue)
            health = HealthValue;

        OnHealed(_health);
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
