using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Healthbar : MonoBehaviour
{
    [SerializeField] Slider healthSlider;
    [SerializeField] Slider damageSlider;
    [SerializeField] TextMeshProUGUI healthText;
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
        SetMaxHealth(_maxHealth);
        UpdateHealth(_maxHealth);
    }

    public void UpdateHealth(float newHealth)
    {
        healthSlider.value = newHealth;
        UpdateText();
    }

    public void SetMaxHealth(float _maxHealth)
    {
        healthSlider.maxValue = _maxHealth;
        damageSlider.maxValue = _maxHealth;
        UpdateText();
    }

    private void UpdateText()
    {
        healthText.SetText(healthSlider.value.ToString() + " / " + healthSlider.maxValue.ToString());
    }
}
