using UnityEngine;

public class ShipMovement : MonoBehaviour
{
    public int steeringDirection = 0;

    public float turnSpeed = 50f;
    public float moveSpeed = 100f;
    public float thrustAmount = 0f;
    public float turnStrength = 0.1f;

    private Rigidbody rb;

    private float rudderAngle = 0;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        // Forward speed (signed)
        float speed = Vector3.Dot(rb.linearVelocity, transform.forward);

        // Rudder effect (more accurate than just using degrees directly)
        float rudderEffect = Mathf.Sin(rudderAngle * Mathf.Deg2Rad); // -1 to 1

        // Torque = rudder influence * speed * sensitivity
        float torqueAmount = rudderEffect * speed * turnStrength;

        rb.AddForce(transform.forward * thrustAmount * moveSpeed, ForceMode.Acceleration);

    }

    public void HandleSteer(int direction, float angle)
    {
        steeringDirection = -direction;
        rudderAngle = angle;
    }

    public void HandleThrust(float amount)
    {
        thrustAmount = amount;
    }
}
