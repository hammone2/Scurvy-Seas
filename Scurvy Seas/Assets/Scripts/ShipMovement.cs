using UnityEngine;

public class ShipMovement : MonoBehaviour
{
    public int steeringDirection = 0;

    public float turnSpeed = 50f;
    public float moveSpeed = 100f;
    public float turnStrength = 0f;

    private Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        if (steeringDirection != 0)
        {
            float currentAngle = Mathf.DeltaAngle(0, transform.eulerAngles.y);

            if (currentAngle > -45f && currentAngle < 45f)
                transform.Rotate(new Vector3(0, turnSpeed, 0) * Time.deltaTime * steeringDirection);

            //correction
            float newAngle = Mathf.Clamp(Mathf.DeltaAngle(0, transform.eulerAngles.y) + turnSpeed * Time.deltaTime * steeringDirection, -45f, 45f);
            transform.rotation = Quaternion.Euler(0, newAngle, 0);
        }


    }

    public void HandleSteer(int direction)
    {
        steeringDirection = -direction;
    }
}
