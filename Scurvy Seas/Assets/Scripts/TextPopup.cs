using UnityEngine;
using TMPro;

public class TextPopup : MonoBehaviour
{
    private float lifetime = 10f;
    private float moveSpeed = 50f;
    [SerializeField] private TextMeshProUGUI damageText;

    private float timer;

    public void SetTextValue(string text, int fontSize = 36, Color color = default, float _lifeTime = 0.5f, float _moveSpeed = 50f)
    {
        damageText.text = text;
        damageText.fontSize = fontSize;

        if (color == default)
        {
            color = Color.white;
        }
        damageText.color = color;

        lifetime = _lifeTime;
        timer = lifetime;
        moveSpeed = _moveSpeed;
    }

    void Start()
    {
        timer = lifetime;
    }

    void Update()
    {
        transform.Translate(Vector3.up * moveSpeed * Time.deltaTime);

        timer -= Time.deltaTime;

        if (timer <= 0)
        {
            Destroy(gameObject); //send to obj pool later
        }
    }

    void Fade()
    {
        Color currentColor = damageText.color;
        currentColor.a = Mathf.Lerp(0f, 1f, timer / lifetime);
        damageText.color = currentColor;
    }
}