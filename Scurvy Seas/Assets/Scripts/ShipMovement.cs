using UnityEngine;

public class ShipMovement : MonoBehaviour
{

    public float turnSpeed = 50f;
    public float moveSpeed = 100f;
    public float turnStrength = 0f;

    void FixedUpdate()
    {
        if (PlayerManager.instance.steeringDirection != 0)
        {
            float currentAngle = Mathf.DeltaAngle(0, transform.eulerAngles.y);

            if (currentAngle > -45f && currentAngle < 45f)
                transform.Rotate(new Vector3(0, turnSpeed, 0) * Time.deltaTime * PlayerManager.instance.steeringDirection);

            //correction
            float newAngle = Mathf.Clamp(Mathf.DeltaAngle(0, transform.eulerAngles.y) + turnSpeed * Time.deltaTime * PlayerManager.instance.steeringDirection, -45f, 45f);
            transform.rotation = Quaternion.Euler(0, newAngle, 0);
        }
    }
}
