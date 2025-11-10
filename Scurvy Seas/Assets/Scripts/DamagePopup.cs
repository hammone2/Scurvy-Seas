using UnityEngine;
using TMPro;

public class DamagePopup : MonoBehaviour
{
    [SerializeField] private float lifetime = 1.5f;
    [SerializeField] private float moveSpeed = 1f;
    [SerializeField] private TextMeshProUGUI damageText;

    private float timer;

    public void SetDamageValue(float damage)
    {
        damageText.text = damage.ToString();
    }

    void Start()
    {
        timer = lifetime;
    }

    void Update()
    {
        transform.Translate(Vector3.up * moveSpeed * Time.deltaTime);

        /*/fade
        Color currentColor = damageText.color;
        currentColor.a = Mathf.Lerp(0f, 1f, timer / lifetime);
        damageText.color = currentColor;*/

        timer -= Time.deltaTime;
        if (timer <= 0)
        {
            Destroy(gameObject); //send to obj pool later
        }
    }
}