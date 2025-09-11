using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager instance;
    public int steeringDirection;
    public TextMeshProUGUI degreesText;
    [SerializeField] float rotationSpeed;
    [SerializeField] GameObject wheelSprite;
    [SerializeField] Slider sailSlider;

    public ShipMovement playerShip;

    private float newAngle = 0;

    private void Awake()
    {
        instance = this;
    }

    private void FixedUpdate()
    {
        if (steeringDirection != 0)
        {
            float currentAngle = Mathf.DeltaAngle(0, wheelSprite.transform.eulerAngles.z);

            if (currentAngle > -45f && currentAngle < 45f)
                wheelSprite.transform.Rotate(new Vector3(0, 0, rotationSpeed) * Time.deltaTime * steeringDirection);

            //correction
            newAngle = Mathf.Clamp(Mathf.DeltaAngle(0, wheelSprite.transform.eulerAngles.z) + rotationSpeed * Time.deltaTime * steeringDirection, -45f, 45f);
            wheelSprite.transform.rotation = Quaternion.Euler(0, 0, newAngle);

            degreesText.SetText("Deg: " + newAngle.ToString("F1")); //round angle to 1 decimal place
        }
    }

    public void HandleSteer(int direction)
    {
        steeringDirection = -direction;
        playerShip.HandleSteer(-direction, newAngle);
    }

    public void HandleThrust()
    {
        float amount = sailSlider.value;
        if (amount > 0)
            playerShip.HandleThrust(amount);
    }
}
