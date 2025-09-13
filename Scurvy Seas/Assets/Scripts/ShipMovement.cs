using UnityEngine;

public class ShipMovement : MonoBehaviour
{
    public int steeringDirection = 0;

    public float turnSpeed = 50f;
    public float moveSpeed = 100f;
    public float thrustAmount = 0f;
    public float turnStrength = 0.1f;

    private Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        MoveShip();
        SteerShip();
    }

    public void HandleThrust(float amount)
    {
        thrustAmount = amount;
    }

    public void HandleSteer(int direction)
    {
        steeringDirection = -direction;
    }

    private void MoveShip()
    {
        if (thrustAmount <= 0)
            return;

        Vector3 forwardMovement = transform.forward * thrustAmount * moveSpeed * Time.deltaTime;

        rb.AddForce(forwardMovement, ForceMode.Acceleration);
    }

    private void SteerShip()
    {
        if (rb.linearVelocity.magnitude > 0.1f) //avoid rotation when stationary
        {
            float currentSpeed = rb.linearVelocity.magnitude;
            float steerSpeed = turnStrength * currentSpeed;

            float rotation = steeringDirection * steerSpeed * Time.deltaTime;
            rb.AddTorque(Vector3.up * rotation, ForceMode.VelocityChange);
        }
    }
}
