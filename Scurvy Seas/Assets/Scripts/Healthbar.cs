using UnityEngine;
using UnityEngine.UI;

public class Healthbar : MonoBehaviour
{
    [SerializeField] Slider healthSlider;
    [SerializeField] Slider damageSlider;
    private float lerpSpeed = 1.5f;

    void Update()
    {
        if (healthSlider.value != damageSlider.value)
        {
            damageSlider.value = Mathf.MoveTowards(damageSlider.value, healthSlider.value, lerpSpeed * Time.deltaTime);
        }
    }

    public void InitializeHealth(float _maxHealth)
    {
        healthSlider.maxValue = _maxHealth;
        damageSlider.maxValue = _maxHealth;
        UpdateHealth(_maxHealth);
    }

    public void UpdateHealth(float newHealth)
    {
        healthSlider.value = newHealth;
    }
}
