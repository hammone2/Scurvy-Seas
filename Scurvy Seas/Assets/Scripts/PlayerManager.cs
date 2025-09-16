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

    private void Awake()
    {
        instance = this;
    }

    private void Update()
    {
        if (steeringDirection != 0)
        {
            float currentAngle = Mathf.DeltaAngle(0, wheelSprite.transform.eulerAngles.z);

            wheelSprite.transform.Rotate(new Vector3(0, 0, rotationSpeed) * Time.deltaTime * steeringDirection);
        }
    }

    public void HandleSteer(int direction)
    {
        steeringDirection = -direction;
        playerShip.HandleSteer(-direction);
    }

    public void HandleThrust()
    {
        float amount = sailSlider.value;
        playerShip.HandleThrust(amount);
    }
}
