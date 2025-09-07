using TMPro;
using UnityEngine;

public class SteeringWheelUI : MonoBehaviour
{
    public TextMeshProUGUI degreesText;
    [SerializeField] float rotationSpeed;

    private void FixedUpdate()
    {
        if (SteeringManager.instance.steeringDirection != 0)
        {
            float currentAngle = Mathf.DeltaAngle(0, transform.eulerAngles.z);

            if (currentAngle > -45f && currentAngle < 45f)
                transform.Rotate(new Vector3(0, 0, rotationSpeed) * Time.deltaTime * SteeringManager.instance.steeringDirection);

            //correction
            float newAngle = Mathf.Clamp(Mathf.DeltaAngle(0, transform.eulerAngles.z) + rotationSpeed * Time.deltaTime * SteeringManager.instance.steeringDirection, -45f, 45f);
            transform.rotation = Quaternion.Euler(0, 0, newAngle);

            degreesText.SetText("Deg: " + newAngle.ToString("F1")); //round angle to 1 decimal place
        }
    }
}
